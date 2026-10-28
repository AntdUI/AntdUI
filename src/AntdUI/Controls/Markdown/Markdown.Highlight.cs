// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections.Generic;

namespace AntdUI
{
    /// <summary>
    /// 代码片段
    /// </summary>
    internal struct MDSeg
    {
        public int Start;
        public int Length;
        public MDToken Token;

        public MDSeg(int start, int length, MDToken token)
        {
            Start = start;
            Length = length;
            Token = token;
        }
    }

    /// <summary>
    /// 代码 / Diff 高亮（零分配复用，面向流式刷新）
    /// </summary>
    internal static class MarkdownHighlighter
    {
        #region 词法表

        static readonly Dictionary<string, string> Families = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "c", "c" }, { "cc", "c" }, { "cpp", "c" }, { "c++", "c" }, { "cs", "c" }, { "c#", "c" }, { "csharp", "c" },
            { "java", "c" }, { "js", "c" }, { "jsx", "c" }, { "javascript", "c" }, { "ts", "c" }, { "tsx", "c" }, { "typescript", "c" },
            { "go", "c" }, { "golang", "c" }, { "rust", "c" }, { "rs", "c" }, { "php", "c" }, { "swift", "c" },
            { "kt", "c" }, { "kotlin", "c" }, { "scala", "c" }, { "dart", "c" }, { "vb", "vb" },
            { "py", "py" }, { "python", "py" }, { "rb", "py" }, { "ruby", "py" }, { "pl", "py" }, { "perl", "py" },
            { "sh", "py" }, { "bash", "py" }, { "shell", "py" }, { "zsh", "py" }, { "yaml", "py" }, { "yml", "py" }, { "toml", "py" }, { "ini", "py" },
            { "sql", "sql" }, { "html", "xml" }, { "xml", "xml" }, { "xaml", "xml" }, { "vue", "xml" }, { "svg", "xml" }, { "csproj", "xml" },
            { "css", "css" }, { "scss", "css" }, { "less", "css" }, { "json", "json" }, { "jsonc", "json" },
        };

        static readonly HashSet<string> KwC = new HashSet<string>(StringComparer.Ordinal)
        {
            "abstract","as","async","await","base","bool","break","byte","case","catch","char","checked","class","const","continue",
            "decimal","default","delegate","do","double","dynamic","else","enum","event","explicit","extern","false","finally","fixed",
            "float","for","foreach","from","get","goto","if","implicit","in","int","interface","internal","is","let","lock","long",
            "namespace","new","null","object","operator","out","override","params","partial","private","protected","public","readonly",
            "ref","return","sbyte","sealed","set","short","sizeof","stackalloc","static","string","struct","switch","this","throw",
            "true","try","typeof","uint","ulong","unchecked","unsafe","ushort","using","var","virtual","void","volatile","when","where",
            "while","yield","func","package","import","interface","struct","map","range","defer","chan","select","nil","fn","let","mut",
            "impl","pub","use","mod","trait","match","val","fun","when","init","self","print","echo","var","function","define","undef",
            "include","public","private","class","new","delete","template","typename","namespace","using","virtual","override","final",
        };

        static readonly HashSet<string> KwPy = new HashSet<string>(StringComparer.Ordinal)
        {
            "and","as","assert","async","await","break","class","continue","def","del","elif","else","except","False","finally","for",
            "from","global","if","import","in","is","lambda","None","nonlocal","not","or","pass","raise","return","True","try","while",
            "with","yield","self","print","elif","end","then","do","done","fi","esac","function","case","export","local","source","sudo",
        };

        static readonly HashSet<string> KwSql = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "select","from","where","insert","into","values","update","set","delete","create","table","alter","drop","add","column",
            "join","left","right","inner","outer","full","cross","on","group","by","order","having","limit","offset","as","and","or",
            "not","null","is","in","like","between","distinct","union","all","case","when","then","else","end","primary","key","foreign",
            "references","default","index","view","truncate","exists","if","begin","commit","rollback","declare","exec","with","returning",
        };

        static readonly HashSet<string> KwVb = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "if","then","else","elseif","end","sub","function","dim","as","for","each","next","do","while","loop","select","case","class",
            "module","namespace","imports","public","private","protected","friend","shared","static","overrides","overloads","new","nothing",
            "true","false","return","try","catch","finally","throw","with","using","property","get","set","byval","byref","integer","string",
            "boolean","double","single","long","short","byte","object","ctype","directcast","typeof","addressof","handles","implements","inherits",
        };

        static string Family(string? lang)
        {
            if (string.IsNullOrEmpty(lang)) return "";
            if (Families.TryGetValue(lang!, out var result)) return result ?? "";
            return "";
        }

        static HashSet<string>? Keywords(string family)
        {
            switch (family)
            {
                case "c": return KwC;
                case "py": return KwPy;
                case "sql": return KwSql;
                case "vb": return KwVb;
            }
            return null;
        }

        static string? LineComment(string family)
        {
            switch (family)
            {
                case "c": return "//";
                case "py": return "#";
                case "sql": return "--";
                case "vb": return "'";
            }
            return null;
        }

        static string BlockStart(string family)
        {
            switch (family)
            {
                case "xml": return "<!--";
                default: return "/*";
            }
        }

        static string BlockEnd(string family)
        {
            switch (family)
            {
                case "xml": return "-->";
                default: return "*/";
            }
        }

        #endregion

        #region Diff

        /// <summary>
        /// 是否为 Diff 内容
        /// </summary>
        public static bool IsDiff(string? lang, string text)
        {
            if (!string.IsNullOrEmpty(lang) && lang!.Equals("diff", StringComparison.OrdinalIgnoreCase)) return true;
            if (!string.IsNullOrEmpty(lang)) return false;
            var lines = MarkdownParser.SplitLines(text);
            bool any = false;
            foreach (var line in lines)
            {
                if (line.Length == 0) continue;
                for (int i = 0; i < line.Length; i++)
                {
                    if (!char.IsWhiteSpace(line[i]))
                    {
                        char c = line[i];
                        if (c == '+' || c == '-' || c == '@') any = true;
                        else return false;
                        break;
                    }
                }
            }
            return any;
        }

        /// <summary>
        /// Diff 行类型
        /// </summary>
        public static MDToken DiffLine(string line)
        {
            if (line.Length == 0) return MDToken.Plain;
            char c = line[0];
            if (c == '@') return MDToken.Meta;
            if (line.StartsWith("+++", StringComparison.Ordinal) || line.StartsWith("---", StringComparison.Ordinal)) return MDToken.Meta;
            if (c == '+') return MDToken.Add;
            if (c == '-') return MDToken.Del;
            return MDToken.Plain;
        }

        #endregion

        /// <summary>
        /// 将一行代码切分为高亮片段
        /// </summary>
        public static void Tokenize(string text, string? lang, ref bool inComment, List<MDSeg> segs)
        {
            segs.Clear();
            int n = text.Length;
            if (n == 0) return;
            string family = Family(lang);
            var keywords = Keywords(family);
            string? lineComment = LineComment(family);
            string blockStart = BlockStart(family);
            string blockEnd = BlockEnd(family);
            bool supportBlock = family != "py" && family != "json" && family.Length > 0;
            int i = 0;
            while (i < n)
            {
                if (inComment)
                {
                    int end = text.IndexOf(blockEnd, i, StringComparison.Ordinal);
                    if (end < 0)
                    {
                        Add(segs, i, n - i, MDToken.Comment);
                        i = n;
                    }
                    else
                    {
                        end += blockEnd.Length;
                        Add(segs, i, end - i, MDToken.Comment);
                        i = end;
                        inComment = false;
                    }
                    continue;
                }
                char c = text[i];
                if (char.IsWhiteSpace(c))
                {
                    Add(segs, i, 1, MDToken.Plain);
                    i++;
                    continue;
                }
                if (lineComment != null && MatchAt(text, i, lineComment))
                {
                    Add(segs, i, n - i, MDToken.Comment);
                    i = n;
                    continue;
                }
                if (supportBlock && MatchAt(text, i, blockStart))
                {
                    int end = text.IndexOf(blockEnd, i + blockStart.Length, StringComparison.Ordinal);
                    if (end < 0)
                    {
                        Add(segs, i, n - i, MDToken.Comment);
                        inComment = true;
                        i = n;
                    }
                    else
                    {
                        end += blockEnd.Length;
                        Add(segs, i, end - i, MDToken.Comment);
                        i = end;
                    }
                    continue;
                }
                if (c == '"' || c == '\'' || c == '`')
                {
                    int j = i + 1;
                    while (j < n)
                    {
                        if (text[j] == '\\')
                        {
                            j += 2;
                            continue;
                        }
                        if (text[j] == c)
                        {
                            j++;
                            break;
                        }
                        if (text[j] == '\n') break;
                        j++;
                    }
                    if (j > n) j = n;
                    Add(segs, i, j - i, MDToken.String);
                    i = j;
                    continue;
                }
                if (c >= '0' && c <= '9')
                {
                    int j = i;
                    while (j < n && (char.IsDigit(text[j]) || text[j] == '.' || text[j] == 'x' || text[j] == 'X' || text[j] == '_' ||
                        (text[j] >= 'a' && text[j] <= 'f') || (text[j] >= 'A' && text[j] <= 'F'))) j++;
                    Add(segs, i, j - i, MDToken.Number);
                    i = j;
                    continue;
                }
                if (char.IsLetter(c) || c == '_' || c == '@')
                {
                    int j = i;
                    while (j < n && (char.IsLetterOrDigit(text[j]) || text[j] == '_')) j++;
                    string word = text.Substring(i, j - i);
                    MDToken token = MDToken.Plain;
                    if (keywords != null && (keywords.Contains(word) || keywords.Contains(word.ToLowerInvariant()))) token = MDToken.Keyword;
                    else if (family == "c" && char.IsUpper(word[0])) token = MDToken.Type;
                    Add(segs, i, j - i, token);
                    i = j;
                    continue;
                }
                Add(segs, i, 1, MDToken.Plain);
                i++;
            }
        }

        public static string Text(string line, MDSeg seg) => line.Substring(seg.Start, seg.Length);

        static bool MatchAt(string text, int i, string value)
        {
            if (i + value.Length > text.Length) return false;
            for (int k = 0; k < value.Length; k++)
            {
                if (text[i + k] != value[k]) return false;
            }
            return true;
        }

        static void Add(List<MDSeg> segs, int start, int length, MDToken token)
        {
            if (length <= 0) return;
            if (segs.Count > 0)
            {
                var last = segs[segs.Count - 1];
                if (last.Token == token)
                {
                    segs[segs.Count - 1] = new MDSeg(last.Start, last.Length + length, token);
                    return;
                }
            }
            segs.Add(new MDSeg(start, length, token));
        }
    }
}