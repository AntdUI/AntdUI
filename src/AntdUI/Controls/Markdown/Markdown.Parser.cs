// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections.Generic;
using System.Text;

namespace AntdUI
{
    /// <summary>
    /// Markdown 解析器（零正则、单遍扫描，面向流式输出优化）
    /// </summary>
    internal sealed class MarkdownParser
    {
        /// <summary>
        /// 解析 Markdown 文本
        /// </summary>
        public MDDocument Parse(string text)
        {
            var doc = new MDDocument();
            if (string.IsNullOrEmpty(text)) return doc;
            doc.Blocks = ParseBlocks(SplitLines(text));
            AssignAnchors(doc.Blocks);
            doc.BuildHash();
            return doc;
        }

        #region 锚点

        /// <summary>
        /// 为全部标题分配锚点（GitHub 风格 slug，重复标题追加 -1、-2）
        /// </summary>
        public static void AssignAnchors(List<MDBlock> blocks)
        {
            AssignAnchors(blocks, new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase));
        }

        static void AssignAnchors(List<MDBlock> blocks, Dictionary<string, int> used)
        {
            foreach (var block in blocks)
            {
                if (block.Kind >= MDBlockKind.Heading1 && block.Kind <= MDBlockKind.Heading6)
                {
                    string text = PlainText(block.Inlines);
                    if (text.Length == 0) text = block.Raw ?? "";
                    block.Anchor = UniqueSlug(Slug(text), used);
                }
                if (block.Children != null) AssignAnchors(block.Children, used);
            }
        }

        static string PlainText(List<MDInline> inlines)
        {
            if (inlines.Count == 0) return "";
            var sb = new StringBuilder();
            foreach (var inline in inlines) sb.Append(inline.Text);
            return sb.ToString().Trim();
        }

        static string UniqueSlug(string slug, Dictionary<string, int> used)
        {
            if (slug.Length == 0) slug = "section";
            int count;
            if (used.TryGetValue(slug, out count))
            {
                used[slug] = count + 1;
                return slug + "-" + count;
            }
            used[slug] = 1;
            return slug;
        }

        /// <summary>
        /// GitHub 风格锚点：小写、空格转连字符、剔除标点（保留字母数字、下划线、连字符与文字类字符）
        /// </summary>
        public static string Slug(string? text)
        {
            if (string.IsNullOrEmpty(text)) return "";
            var sb = new StringBuilder(text!.Length);
            foreach (char c in text)
            {
                if (c == ' ' || c == '\t' || c == '\n' || c == '\r') sb.Append('-');
                else if (char.IsLetterOrDigit(c) || c == '_' || c == '-') sb.Append(char.ToLowerInvariant(c));
            }
            return sb.ToString().Trim('-');
        }

        #endregion

        #region 行拆分

        /// <summary>
        /// 按行拆分，兼容 \n 与 \r\n
        /// </summary>
        public static List<string> SplitLines(string text)
        {
            var list = new List<string>();
            int start = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '\n')
                {
                    int end = i;
                    if (end > start && text[end - 1] == '\r') end--;
                    list.Add(text.Substring(start, end - start));
                    start = i + 1;
                }
            }
            if (start < text.Length) list.Add(text.Substring(start));
            return list;
        }

        #endregion

        #region 块级解析

        public static List<MDBlock> ParseBlocks(List<string> lines)
        {
            return ParseBlocks(lines, false);
        }

        /// <summary>
        /// 解析块级元素
        /// </summary>
        /// <param name="hardLineBreak">
        /// 为 true 时段落内的每个换行都渲染为硬换行（各自成行、行距紧凑），用于引用内部：
        /// 中文等无空格语言的相邻两行若按软换行合并，会被拼接成同一行而“粘连”
        /// </param>
        public static List<MDBlock> ParseBlocks(List<string> lines, bool hardLineBreak)
        {
            var blocks = new List<MDBlock>();
            var indentStack = new List<int>();
            int i = 0;
            while (i < lines.Count)
            {
                string line = lines[i];

                #region 空行

                if (IsBlank(line))
                {
                    indentStack.Clear();
                    blocks.Add(new MDBlock(MDBlockKind.Blank));
                    i++;
                    continue;
                }

                #endregion

                #region 代码围栏

                char fenceChar;
                int fenceCount;
                string lang;
                if (MatchFence(line, out fenceChar, out fenceCount, out lang))
                {
                    indentStack.Clear();
                    var sb = new StringBuilder();
                    bool closed = false;
                    int j = i + 1;
                    for (; j < lines.Count; j++)
                    {
                        string cur = lines[j];
                        if (IsFenceEnd(cur, fenceChar, fenceCount))
                        {
                            closed = true;
                            break;
                        }
                        if (sb.Length > 0) sb.Append('\n');
                        sb.Append(cur);
                    }
                    var block = new MDBlock(MDBlockKind.Code, sb.ToString())
                    {
                        Lang = lang.Length > 0 ? lang : null,
                        Closed = closed,
                    };
                    blocks.Add(block);
                    i = closed ? j + 1 : lines.Count;
                    continue;
                }

                #endregion

                string trim = line.Trim();

                #region HTML 块

                if (trim.Length > 0 && trim[0] == '<')
                {
                    var htmls = ParseHtml(lines, ref i);
                    if (htmls != null)
                    {
                        indentStack.Clear();
                        blocks.AddRange(htmls);
                        continue;
                    }
                    // 内联标签跨行容器（如 <sub>…</sub> 分多行书写）：聚合后按段落内联解析
                    string? inlineTag = InlineContainerTag(trim);
                    if (inlineTag != null && trim.IndexOf("</" + inlineTag, StringComparison.OrdinalIgnoreCase) < 0)
                    {
                        int closeLine = -1;
                        for (int j = i + 1; j < lines.Count; j++)
                        {
                            if (lines[j].IndexOf("</" + inlineTag, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                closeLine = j;
                                break;
                            }
                        }
                        if (closeLine > i)
                        {
                            var sb = new StringBuilder();
                            for (int j = i; j <= closeLine; j++)
                            {
                                string cur = lines[j].Trim();
                                if (sb.Length > 0 && cur.Length > 0 && !char.IsWhiteSpace(sb[sb.Length - 1])) sb.Append(' ');
                                sb.Append(cur);
                            }
                            string joined = sb.ToString();
                            indentStack.Clear();
                            blocks.Add(new MDBlock(MDBlockKind.Paragraph, joined) { Inlines = ParseInlines(joined) });
                            i = closeLine + 1;
                            continue;
                        }
                    }
                    // 未识别的 HTML 标签：剔除标签后按普通内容处理
                    line = StripTags(line);
                    trim = line.Trim();
                    if (trim.Length == 0)
                    {
                        blocks.Add(new MDBlock(MDBlockKind.Blank));
                        i++;
                        continue;
                    }
                }

                #endregion

                #region 表格

                if (line.IndexOf('|') >= 0 && i + 1 < lines.Count && IsTableSeparator(lines[i + 1]))
                {
                    var table = ParseTable(lines, ref i);
                    if (table != null)
                    {
                        indentStack.Clear();
                        blocks.Add(table);
                        continue;
                    }
                }

                #endregion

                #region 分隔线

                if (IsRule(trim))
                {
                    indentStack.Clear();
                    blocks.Add(new MDBlock(MDBlockKind.Rule));
                    i++;
                    continue;
                }

                #endregion

                #region 标题

                string headText;
                int level = MatchHeading(line, out headText);
                if (level > 0)
                {
                    indentStack.Clear();
                    blocks.Add(new MDBlock((MDBlockKind)((int)MDBlockKind.Heading1 + level - 1), headText)
                    {
                        Inlines = ParseInlines(headText),
                    });
                    i++;
                    continue;
                }

                #endregion

                #region 引用

                if (trim[0] == '>')
                {
                    indentStack.Clear();
                    blocks.Add(ParseQuote(lines, ref i));
                    continue;
                }

                #endregion

                #region 列表

                var item = MatchList(line, indentStack);
                if (item != null)
                {
                    blocks.Add(item);
                    i++;
                    continue;
                }

                #endregion

                #region 段落（惰性延续）

                indentStack.Clear();
                var rawText = new StringBuilder(line.TrimEnd());
                var inlines = ParseInlines(line.TrimEnd());
                // 软换行：段落内的单行换行合并到同一段落（不产生换行，宽度不足时才自动折行），与 CommonMark 一致
                // 行尾两个及以上空格视为硬换行，插入换行符
                // 引用内部（hardLineBreak）每个换行都按硬换行处理：各自成行、行距紧凑，
                // 避免中文等无空格语言相邻两行被软换行拼接成同一行而“粘连”
                while (i + 1 < lines.Count && ParseAlert(line.Trim()) == MDAlert.None)
                {
                    string next = lines[i + 1];
                    if (IsBlank(next) || IsSpecial(next)) break;
                    // 下一行是表格标题行（再下一行为分隔行）时不并入段落
                    if (StartsTableBlock(lines, i + 1)) break;
                    bool hard = HasHardBreak(line);
                    string prevText = line.TrimEnd();
                    i++;
                    line = lines[i];
                    string curText = line.TrimEnd();
                    rawText.Append('\n').Append(curText);
                    if (hard || hardLineBreak) inlines.Add(new MDInline(MDInlineKind.Break, ""));
                    else if (SoftBreakSpace(LastChar(prevText), FirstChar(curText))) inlines.Add(new MDInline(MDInlineKind.Text, " "));
                    inlines.AddRange(ParseInlines(curText));
                }
                var paragraph = new MDBlock(MDBlockKind.Paragraph, rawText.ToString()) { Inlines = inlines };
                blocks.Add(paragraph);
                i++;

                #endregion
            }
            return blocks;
        }

        /// <summary>
        /// 判断是否为特殊行（会中断段落）
        /// </summary>
        static bool IsSpecial(string line)
        {
            if (IsBlank(line)) return true;
            char fc;
            int fc2;
            string lang;
            if (MatchFence(line, out fc, out fc2, out lang)) return true;
            string trim = line.Trim();
            if (IsRule(trim)) return true;
            if (trim[0] == '>') return true;
            if (trim[0] == '<') return true;
            string ht;
            if (MatchHeading(line, out ht) > 0) return true;
            int indent;
            string rest;
            CountIndent(line, out indent, out rest);
            return IsListMark(rest);
        }

        static bool HasHardBreak(string line) => line.Length >= 2 && line[line.Length - 1] == ' ' && line[line.Length - 2] == ' ';

        /// <summary>
        /// 软换行连接符是否需要插入空格（中日韩文字之间不插空格）
        /// </summary>
        static bool SoftBreakSpace(char prev, char next)
        {
            if (prev == '\0' || next == '\0') return false;
            if (char.IsWhiteSpace(prev) || char.IsWhiteSpace(next)) return false;
            return !(IsCjkChar(prev) && IsCjkChar(next));
        }

        /// <summary>
        /// 是否为中日韩字符（含全角标点）
        /// </summary>
        static bool IsCjkChar(char c)
        {
            if (c >= 0x2E80 && c <= 0x9FFF) return true;   // 部首扩展 / 假名 / 统一表意文字
            if (c >= 0x3000 && c <= 0x303F) return true;   // CJK 标点符号
            if (c >= 0xAC00 && c <= 0xD7AF) return true;   // 谚文音节
            if (c >= 0xF900 && c <= 0xFAFF) return true;   // 兼容表意文字
            if (c >= 0xFE30 && c <= 0xFE4F) return true;   // CJK 兼容形式
            if (c >= 0xFF00 && c <= 0xFF65) return true;   // 全角符号
            if (c >= 0xFFE0 && c <= 0xFFE6) return true;   // 全角货币符号
            return false;
        }

        static char LastChar(string value) => value.Length == 0 ? '\0' : value[value.Length - 1];

        static char FirstChar(string value) => value.Length == 0 ? '\0' : value[0];

        /// <summary>
        /// 是否为表格起始行（本行含 | 且下一行为分隔行）
        /// </summary>
        static bool StartsTableBlock(List<string> lines, int index)
        {
            if (index + 1 >= lines.Count) return false;
            if (lines[index].IndexOf('|') < 0) return false;
            return IsTableSeparator(lines[index + 1]);
        }

        static bool IsBlank(string line)
        {
            for (int i = 0; i < line.Length; i++)
            {
                if (!char.IsWhiteSpace(line[i])) return false;
            }
            return true;
        }

        static bool IsRule(string trim)
        {
            if (trim.Length < 3) return false;
            char c = trim[0];
            if (c != '*' && c != '-' && c != '_') return false;
            int count = 0;
            for (int i = 0; i < trim.Length; i++)
            {
                char x = trim[i];
                if (x == c) count++;
                else if (x != ' ' && x != '\t') return false;
            }
            return count >= 3;
        }

        static bool MatchFence(string line, out char ch, out int count, out string lang)
        {
            ch = '\0';
            count = 0;
            lang = "";
            int k = 0;
            while (k < line.Length && (line[k] == ' ' || line[k] == '\t')) k++;
            if (k > 3) return false;
            if (k >= line.Length) return false;
            char c = line[k];
            if (c != '`' && c != '~') return false;
            int n = 0;
            while (k < line.Length && line[k] == c)
            {
                n++;
                k++;
            }
            if (n < 3) return false;
            string rest = line.Substring(k).Trim();
            if (c == '`' && rest.IndexOf('`') >= 0) return false;
            ch = c;
            count = n;
            lang = rest;
            return true;
        }

        static bool IsFenceEnd(string line, char c, int count)
        {
            int k = 0;
            while (k < line.Length && (line[k] == ' ' || line[k] == '\t')) k++;
            if (k > 3) return false;
            if (k >= line.Length) return false;
            int n = 0;
            while (k < line.Length && line[k] == c)
            {
                n++;
                k++;
            }
            if (n < count) return false;
            for (; k < line.Length; k++)
            {
                if (line[k] != ' ' && line[k] != '\t') return false;
            }
            return true;
        }

        static int MatchHeading(string line, out string text)
        {
            text = "";
            int k = 0;
            while (k < line.Length && line[k] == ' ') k++;
            int count = 0;
            while (k < line.Length && line[k] == '#')
            {
                count++;
                k++;
            }
            if (count < 1 || count > 6) return 0;
            if (k < line.Length && line[k] != ' ' && line[k] != '\t') return 0;
            while (k < line.Length && (line[k] == ' ' || line[k] == '\t')) k++;
            string val = line.Substring(k).TrimEnd();
            // 去除闭合 #
            int end = val.Length;
            while (end > 0 && val[end - 1] == '#') end--;
            if (end > 0 && val[end - 1] == ' ') end--;
            else if (end == 0) end = val.Length;
            text = end > 0 ? val.Substring(0, end) : "";
            return count;
        }

        static void CountIndent(string line, out int indent, out string rest)
        {
            indent = 0;
            int k = 0;
            while (k < line.Length && (line[k] == ' ' || line[k] == '\t'))
            {
                if (line[k] == '\t') indent += 4 - (indent % 4);
                else indent++;
                k++;
            }
            rest = k < line.Length ? line.Substring(k) : "";
        }

        static bool IsListMark(string rest)
        {
            if (rest.Length == 0) return false;
            char c = rest[0];
            if (c == '-' || c == '*' || c == '+')
            {
                if (rest.Length == 1) return true;
                return rest[1] == ' ' || rest[1] == '\t';
            }
            if (c >= '0' && c <= '9')
            {
                int k = 0;
                while (k < rest.Length && rest[k] >= '0' && rest[k] <= '9') k++;
                if (k == 0 || k >= rest.Length) return false;
                if (rest[k] != '.' && rest[k] != ')') return false;
                if (k + 1 >= rest.Length) return true;
                return rest[k + 1] == ' ' || rest[k + 1] == '\t';
            }
            return false;
        }

        static MDBlock? MatchList(string line, List<int> indentStack)
        {
            int indent;
            string rest;
            CountIndent(line, out indent, out rest);
            if (!IsListMark(rest)) return null;
            char c = rest[0];
            string text;
            int order = 0;
            MDBlockKind kind;
            if (c == '-' || c == '*' || c == '+')
            {
                kind = MDBlockKind.ListUnordered;
                text = rest.Substring(1).TrimStart(' ', '\t');
            }
            else
            {
                kind = MDBlockKind.ListOrdered;
                int k = 0;
                while (k < rest.Length && rest[k] >= '0' && rest[k] <= '9') k++;
                int.TryParse(rest.Substring(0, k), out order);
                text = rest.Substring(k + 1).TrimStart(' ', '\t');
            }
            int level = ResolveLevel(indent, indentStack);
            return new MDBlock(kind, text)
            {
                Inlines = ParseInlines(text),
                Level = level,
                Order = order,
            };
        }

        static int ResolveLevel(int indent, List<int> stack)
        {
            if (stack.Count == 0)
            {
                stack.Add(indent);
                return 0;
            }
            for (int i = 0; i < stack.Count; i++)
            {
                if (stack[i] == indent)
                {
                    if (stack.Count > i + 1) stack.RemoveRange(i + 1, stack.Count - i - 1);
                    return i;
                }
            }
            while (stack.Count > 0 && stack[stack.Count - 1] > indent) stack.RemoveAt(stack.Count - 1);
            if (stack.Count == 0 || indent > stack[stack.Count - 1]) stack.Add(indent);
            return Math.Max(0, stack.Count - 1);
        }

        /// <summary>
        /// 解析块引用：聚合**连续**引用行（中间没有空行）为同一个引用块，内部递归解析。
        /// 空行（上一个换行没有内容）才是块分隔：后续引用行另起一个引用块。
        /// </summary>
        static MDBlock ParseQuote(List<string> lines, ref int index)
        {
            var inner = new List<string>();
            int i = index;
            while (i < lines.Count)
            {
                string line = lines[i];
                string t = line.Trim();
                if (t.Length > 0 && t[0] == '>')
                {
                    // 剥离引用标记（>> 剥离一层后由递归解析继续处理）
                    int k = 0;
                    while (k < t.Length && t[k] == '>') k++;
                    string content = t.Substring(k);
                    if (content.Length > 0 && content[0] == ' ') content = content.Substring(1);
                    // 引用内的空行（> 后没有内容）：上一个换行没有内容 → 当前引用块到此为止
                    if (content.Trim().Length == 0)
                    {
                        i++;
                        if (inner.Count == 0) continue;
                        break;
                    }
                    // 新的提示块标记：结束当前引用块，另起一个（支持连续 [!NOTE] / [!TIP] ...）
                    if (inner.Count > 0 && ParseAlert(content) != MDAlert.None) break;
                    inner.Add(content);
                    i++;
                    continue;
                }
                if (IsBlank(line) && inner.Count > 0)
                {
                    // 引用外部空行同样结束当前引用块（后续若仍是引用行，由下一轮另起一块）
                    int j = i;
                    while (j < lines.Count && IsBlank(lines[j])) j++;
                    if (j < lines.Count && lines[j].Trim().Length > 0 && lines[j].Trim()[0] == '>') break;
                }
                break;
            }
            index = i;
            // 引用内部每个换行都按硬换行渲染：各自成行、行距紧凑（同一引用块内不粘连成一行）
            var children = ParseBlocks(inner, true);
            var block = new MDBlock(MDBlockKind.Quote);
            // GitHub 提示块：首个子段落为 [!XXX] 时转换为提示头
            if (children.Count > 0 && children[0].Kind == MDBlockKind.Paragraph)
            {
                string head = children[0].Raw?.Trim() ?? "";
                var kind = ParseAlert(head);
                if (kind != MDAlert.None)
                {
                    block.Alert = kind;
                    for (int c = 0; c < children.Count; c++) children[c].Alert = kind;
                    children[0].AlertHeader = true;
                    children[0].Inlines = ParseInlines(AlertLabel(kind));
                    children[0].Raw = AlertLabel(kind);
                }
            }
            block.Children = children;
            return block;
        }

        #endregion

        #region 表格

        static bool IsTableSeparator(string line)
        {
            string trim = line.Trim();
            if (trim.Length == 0) return false;
            int dash = 0;
            for (int i = 0; i < trim.Length; i++)
            {
                char c = trim[i];
                if (c == '-') dash++;
                else if (c != '|' && c != ':' && c != ' ' && c != '\t') return false;
            }
            return dash >= 1;
        }

        static List<string> SplitCells(string line)
        {
            var list = new List<string>();
            string trim = line.Trim();
            if (trim.Length > 0 && trim[0] == '|') trim = trim.Substring(1);
            if (trim.Length > 0 && trim[trim.Length - 1] == '|' && !trim.EndsWith("\\|", StringComparison.Ordinal))
                trim = trim.Substring(0, trim.Length - 1);
            var sb = new StringBuilder();
            for (int i = 0; i < trim.Length; i++)
            {
                char c = trim[i];
                if (c == '\\' && i + 1 < trim.Length && trim[i + 1] == '|')
                {
                    sb.Append('|');
                    i++;
                }
                else if (c == '|')
                {
                    list.Add(sb.ToString());
                    sb.Length = 0;
                }
                else sb.Append(c);
            }
            list.Add(sb.ToString());
            return list;
        }

        static MDAlign ParseCellAlign(string cell)
        {
            string t = cell.Trim();
            bool left = t.Length > 0 && t[0] == ':';
            bool right = t.Length > 0 && t[t.Length - 1] == ':';
            if (left && right) return MDAlign.Center;
            if (right) return MDAlign.Right;
            if (left) return MDAlign.Left;
            return MDAlign.None;
        }

        static MDBlock? ParseTable(List<string> lines, ref int index)
        {
            var header = SplitCells(lines[index]);
            int cols = header.Count;
            if (cols == 0) return null;
            var aligns = new MDAlign[cols];
            var sep = SplitCells(lines[index + 1]);
            for (int c = 0; c < cols; c++) aligns[c] = c < sep.Count ? ParseCellAlign(sep[c]) : MDAlign.None;

            var rows = new List<List<MDCell>>();
            var row = new List<MDCell>(cols);
            foreach (var text in header)
            {
                var cell = new MDCell { Inlines = ParseInlines(text.Trim()) };
                row.Add(cell);
            }
            rows.Add(row);

            int i = index + 2;
            while (i < lines.Count)
            {
                string cur = lines[i];
                if (cur.IndexOf('|') < 0 || IsBlank(cur)) break;
                var cells = SplitCells(cur);
                var list = new List<MDCell>(cols);
                for (int c = 0; c < cols; c++)
                {
                    var cell = new MDCell();
                    if (c < cells.Count) cell.Inlines = ParseInlines(cells[c].Trim());
                    list.Add(cell);
                }
                rows.Add(list);
                i++;
            }
            index = i;
            return new MDBlock(MDBlockKind.Table)
            {
                Rows = rows,
                ColAlign = aligns,
                HasHeader = true,
            };
        }

        #endregion

        #region HTML 扩展

        /// <summary>
        /// 解析 HTML 块（div / table / h1-h6 / img / br / hr 等）
        /// </summary>
        static List<MDBlock>? ParseHtml(List<string> lines, ref int index)
        {
            string line = lines[index];
            string trim = line.Trim();
            if (trim.Length < 2 || trim[0] != '<') return null;
            int gt = FindTagEnd(trim, 0);
            if (gt < 0) return null;
            string tag = trim.Substring(0, gt + 1);
            string name = TagName(tag);
            string attrs = gt > 1 ? trim.Substring(name.Length + 1, gt - name.Length - 1) : "";

            switch (name)
            {
                case "div":
                case "center":
                case "p":
                case "blockquote":
                    return ParseHtmlContainer(lines, ref index, name, name == "center" ? MDAlign.Center : ParseAlignAttr(attrs));
                case "table":
                    return ParseHtmlTableBlock(lines, ref index);
                case "br":
                    index++;
                    return new List<MDBlock>(1) { new MDBlock(MDBlockKind.Blank) };
                case "hr":
                    index++;
                    return new List<MDBlock>(1) { new MDBlock(MDBlockKind.Rule) };
                case "img":
                    {
                        index++;
                        var block = new MDBlock(MDBlockKind.Paragraph);
                        block.Inlines.Add(BuildImage(attrs));
                        return new List<MDBlock>(1) { block };
                    }
                case "h1":
                case "h2":
                case "h3":
                case "h4":
                case "h5":
                case "h6":
                    {
                        int level = name[1] - '0';
                        string inner = InnerHtml(lines[index], name);
                        index++;
                        inner = StripTags(inner).Trim();
                        var block = new MDBlock((MDBlockKind)((int)MDBlockKind.Heading1 + level - 1), inner)
                        {
                            Inlines = ParseInlines(inner),
                        };
                        return new List<MDBlock>(1) { block };
                    }
            }
            return null;
        }

        /// <summary>
        /// 容器标签：将其中的行作为 Markdown 继续解析，并继承对齐方式
        /// </summary>
        static List<MDBlock>? ParseHtmlContainer(List<string> lines, ref int index, string name, MDAlign align)
        {
            int i = index;
            string closeTag = "</" + name;
            string line = lines[i];
            int gt = FindTagEnd(line, 0);
            if (gt < 0) return null;
            string first = line.Substring(gt + 1);
            int closeLine = -1, closePos = -1;
            for (int j = i + 1; j < lines.Count; j++)
            {
                int p = lines[j].IndexOf(closeTag, StringComparison.OrdinalIgnoreCase);
                if (p >= 0)
                {
                    closeLine = j;
                    closePos = p;
                    break;
                }
            }
            if (closeLine < 0) return null;
            var inner = new List<string>();
            if (first.Trim().Length > 0) inner.Add(first);
            for (int j = i + 1; j < closeLine; j++) inner.Add(lines[j]);
            string last = lines[closeLine].Substring(0, closePos);
            if (last.Trim().Length > 0) inner.Add(last);

            var blocks = ParseBlocks(inner);
            foreach (var block in blocks)
            {
                if (block.Align == MDAlign.None) block.Align = align;
            }
            index = closeLine + 1;
            return blocks;
        }

        static List<MDBlock>? ParseHtmlTableBlock(List<string> lines, ref int index)
        {
            int closeLine = -1, closePos = -1;
            for (int j = index + 1; j < lines.Count; j++)
            {
                int p = lines[j].IndexOf("</table", StringComparison.OrdinalIgnoreCase);
                if (p >= 0)
                {
                    closeLine = j;
                    closePos = p;
                    break;
                }
            }
            if (closeLine < 0) return null;
            var sb = new StringBuilder();
            for (int j = index; j <= closeLine; j++)
            {
                string cur = lines[j];
                if (j == closeLine) cur = cur.Substring(0, closePos);
                if (sb.Length > 0) sb.Append('\n');
                sb.Append(cur);
            }
            index = closeLine + 1;
            var block = ParseHtmlTable(sb.ToString());
            if (block == null) return null;
            return new List<MDBlock>(1) { block };
        }

        static MDBlock? ParseHtmlTable(string html)
        {
            var rows = new List<List<MDCell>>();
            var widths = new List<float>();
            var aligns = new List<MDAlign>();
            bool hasHeader = false;
            int pos = 0;
            while (true)
            {
                int trs = html.IndexOf("<tr", pos, StringComparison.OrdinalIgnoreCase);
                if (trs < 0) break;
                int gt = FindTagEnd(html, trs);
                int tre = html.IndexOf("</tr", gt < 0 ? trs : gt, StringComparison.OrdinalIgnoreCase);
                if (tre < 0) tre = html.Length;
                string rowHtml = html.Substring(trs, tre - trs);
                var cells = new List<MDCell>();
                int p = 0;
                while (true)
                {
                    int c1 = IndexOfOpenTag(rowHtml, p, out string cellName);
                    if (c1 < 0) break;
                    int cgt = FindTagEnd(rowHtml, c1);
                    if (cgt < 0) break;
                    string tagHtml = rowHtml.Substring(c1, cgt - c1 + 1);
                    int ce = rowHtml.IndexOf("</" + cellName, cgt, StringComparison.OrdinalIgnoreCase);
                    if (ce < 0) ce = rowHtml.Length;
                    string content = rowHtml.Substring(cgt + 1, ce - cgt - 1);
                    var cell = new MDCell { Inlines = ParseInlines(StripTags(content).Replace("\n", " ").Trim()) };
                    cells.Add(cell);

                    int index = cells.Count - 1;
                    float width = ParseWidthAttr(tagHtml);
                    while (widths.Count <= index) widths.Add(0);
                    if (width > 0) widths[index] = width;
                    var align = ParseAlignAttr(tagHtml);
                    while (aligns.Count <= index) aligns.Add(MDAlign.None);
                    if (align != MDAlign.None) aligns[index] = align;
                    if (cellName == "th") hasHeader = true;

                    int next = ce < rowHtml.Length ? rowHtml.IndexOf('>', ce) : -1;
                    if (next < 0) break;
                    p = next + 1;
                }
                if (cells.Count > 0) rows.Add(cells);
                int treEnd = tre < html.Length ? html.IndexOf('>', tre) : -1;
                if (treEnd < 0) break;
                pos = treEnd + 1;
            }
            if (rows.Count == 0) return null;
            return new MDBlock(MDBlockKind.Table)
            {
                Rows = rows,
                ColAlign = aligns.ToArray(),
                ColWidth = widths.ToArray(),
                HasHeader = hasHeader,
            };
        }

        static int IndexOfOpenTag(string html, int from, out string name)
        {
            name = "";
            int td = html.IndexOf("<td", from, StringComparison.OrdinalIgnoreCase);
            int th = html.IndexOf("<th", from, StringComparison.OrdinalIgnoreCase);
            int pos;
            if (td < 0 && th < 0) return -1;
            if (td < 0) pos = th;
            else if (th < 0) pos = td;
            else pos = td < th ? td : th;
            name = html[pos + 1] == 't' && html[pos + 2] == 'd' ? "td" : "th";
            return pos;
        }

        /// <summary>
        /// 取标签内的文本（自动匹配同名闭合标签）
        /// </summary>
        static string InnerHtml(string line, string name)
        {
            int open = line.IndexOf("<" + name, StringComparison.OrdinalIgnoreCase);
            if (open < 0) return line;
            int gt = FindTagEnd(line, open);
            if (gt < 0) return line;
            int close = line.IndexOf("</" + name, gt, StringComparison.OrdinalIgnoreCase);
            if (close < 0) close = line.Length;
            return line.Substring(gt + 1, close - gt - 1);
        }

        /// <summary>
        /// 查找标签结束符 &gt;（跳过引号内的内容）
        /// </summary>
        static int FindTagEnd(string text, int from)
        {
            char quote = '\0';
            for (int i = from; i < text.Length; i++)
            {
                char c = text[i];
                if (quote != '\0')
                {
                    if (c == quote) quote = '\0';
                }
                else if (c == '"' || c == '\'') quote = c;
                else if (c == '>') return i;
                else if (c == '<' && i > from) return -1;
            }
            return -1;
        }

        static string TagName(string tag)
        {
            int i = 1;
            while (i < tag.Length && (tag[i] == '/' || tag[i] == ' ' || tag[i] == '\t')) i++;
            int start = i;
            while (i < tag.Length && char.IsLetterOrDigit(tag[i])) i++;
            return tag.Substring(start, i - start).ToLowerInvariant();
        }

        static string AttrValue(string tag, string name)
        {
            int p = 0;
            while (true)
            {
                p = tag.IndexOf(name, p, StringComparison.OrdinalIgnoreCase);
                if (p < 0) return "";
                int end = p + name.Length;
                bool ok = true;
                if (p > 0)
                {
                    char before = tag[p - 1];
                    if (char.IsLetterOrDigit(before) || before == '-') ok = false;
                }
                while (end < tag.Length && (tag[end] == ' ' || tag[end] == '\t')) end++;
                if (end < tag.Length && tag[end] == '=')
                {
                    end++;
                    while (end < tag.Length && (tag[end] == ' ' || tag[end] == '\t')) end++;
                    if (end < tag.Length && (tag[end] == '"' || tag[end] == '\''))
                    {
                        char q = tag[end];
                        int close = tag.IndexOf(q, end + 1);
                        if (close < 0) return tag.Substring(end + 1);
                        return tag.Substring(end + 1, close - end - 1);
                    }
                    int s = end;
                    while (end < tag.Length && tag[end] != ' ' && tag[end] != '\t' && tag[end] != '>') end++;
                    return tag.Substring(s, end - s);
                }
                if (!ok) p = end;
                else p = p + 1;
            }
        }

        static MDAlign ParseAlignAttr(string attrs)
        {
            string val = AttrValue(attrs, "align");
            if (val.Length == 0)
            {
                // style="text-align:center"
                val = AttrValue(attrs, "text-align");
            }
            switch (val.ToLowerInvariant())
            {
                case "center": return MDAlign.Center;
                case "right": return MDAlign.Right;
                case "left": return MDAlign.Left;
            }
            return MDAlign.None;
        }

        static float ParseWidthAttr(string tag)
        {
            string val = AttrValue(tag, "width");
            if (val.Length == 0) return 0;
            val = val.Trim();
            bool percent = val.EndsWith("%", StringComparison.Ordinal);
            if (percent) val = val.Substring(0, val.Length - 1);
            float result;
            if (float.TryParse(val, out result)) return percent ? result : -result;
            return 0;
        }

        static int ParseSizeAttr(string tag, string name)
        {
            string val = AttrValue(tag, name);
            if (val.Length == 0) return 0;
            val = val.Trim();
            if (val.EndsWith("px", StringComparison.OrdinalIgnoreCase)) val = val.Substring(0, val.Length - 2);
            int result;
            if (int.TryParse(val, out result)) return result;
            return 0;
        }

        static MDInline BuildImage(string tag)
        {
            string src = AttrValue(tag, "src");
            string alt = AttrValue(tag, "alt");
            var inline = new MDInline(MDInlineKind.Image, DecodeEntities(alt), DecodeEntities(src));
            inline.Width = ParseSizeAttr(tag, "width");
            inline.Height = ParseSizeAttr(tag, "height");
            return inline;
        }

        /// <summary>
        /// 内联格式标签（剔除 HTML 时保留，交由内联解析器处理）
        /// </summary>
        static bool IsInlineTagName(string name)
        {
            switch (name)
            {
                case "b":
                case "strong":
                case "i":
                case "em":
                case "code":
                case "kbd":
                case "del":
                case "s":
                case "strike":
                case "sub":
                case "sup":
                case "a":
                case "span":
                case "u":
                case "small":
                case "mark":
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 行首若为内联标签的开启标记，返回标签名（用于跨行容器聚合，如 &lt;sub&gt;…&lt;/sub&gt; 分多行）
        /// </summary>
        static string? InlineContainerTag(string trim)
        {
            int gt = FindTagEnd(trim, 0);
            if (gt < 0) return null;
            string tag = trim.Substring(0, gt + 1);
            string name = TagName(tag);
            if (name.Length == 0 || !IsInlineTagName(name)) return null;
            if (tag.EndsWith("/>", StringComparison.Ordinal)) return null;
            return name;
        }

        /// <summary>
        /// 剔除 HTML 标签（保留内联格式标签）
        /// </summary>
        public static string StripTags(string text)
        {
            if (text.IndexOf('<') < 0) return text;
            var sb = new StringBuilder(text.Length);
            int i = 0;
            while (i < text.Length)
            {
                char c = text[i];
                if (c == '<')
                {
                    int gt = FindTagEnd(text, i);
                    if (gt < 0)
                    {
                        sb.Append(c);
                        i++;
                    }
                    else
                    {
                        string tag = text.Substring(i, gt - i + 1);
                        string name = TagName(tag);
                        if (name == "br") sb.Append('\n');
                        else if (name == "img")
                        {
                            var img = BuildImage(tag);
                            sb.Append('!').Append('[').Append(img.Text).Append(']').Append('(').Append(img.Url).Append(')');
                        }
                        else if (IsInlineTagName(name)) sb.Append(tag);
                        i = gt + 1;
                    }
                }
                else
                {
                    sb.Append(c);
                    i++;
                }
            }
            return sb.ToString();
        }

        #endregion

        #region 内联解析

        public static List<MDInline> ParseInlines(string text)
        {
            var list = new List<MDInline>();
            if (string.IsNullOrEmpty(text)) return list;
            int n = text.Length, i = 0, start = 0;
            while (i < n)
            {
                char c = text[i];

                #region 转义

                if (c == '\\' && i + 1 < n && IsEscapable(text[i + 1]))
                {
                    if (i > start) AddText(list, text, start, i);
                    list.Add(new MDInline(MDInlineKind.Text, text.Substring(i + 1, 1)));
                    i += 2;
                    start = i;
                    continue;
                }

                #endregion

                #region 行内代码

                if (c == '`')
                {
                    int run = RunLen(text, i, '`');
                    string marker = text.Substring(i, run);
                    int close = text.IndexOf(marker, i + run, StringComparison.Ordinal);
                    if (close > i + run)
                    {
                        string content = text.Substring(i + run, close - i - run);
                        if (content.Length == 0)
                        {
                            i += run;
                            continue;
                        }
                        if (content.Length > 2 && content[0] == ' ' && content[content.Length - 1] == ' ')
                            content = content.Substring(1, content.Length - 2);
                        if (i > start) AddText(list, text, start, i);
                        list.Add(new MDInline(MDInlineKind.Code, content) { Md = marker + content + marker });
                        i = close + run;
                        start = i;
                        continue;
                    }
                }

                #endregion

                #region 强调

                if (c == '*' || c == '_')
                {
                    int run = RunLen(text, i, c);
                    int r = run > 3 ? 3 : run;
                    // CommonMark：* 允许词内开启（Love**is**bold），_ 仍需词边界
                    if (c == '*' || i == 0 || !IsWordChar(text[i - 1]))
                    {
                        string marker = new string(c, r);
                        int close = FindEmphasisClose(text, i + r, marker, c);
                        if (close > i + r)
                        {
                            string content = text.Substring(i + r, close - i - r);
                            if (i > start) AddText(list, text, start, i);
                            var kind = r == 1 ? MDInlineKind.Italic : (r == 2 ? MDInlineKind.Bold : MDInlineKind.BoldItalic);
                            list.Add(new MDInline(kind, content) { Md = marker + content + marker });
                            i = close + r;
                            start = i;
                            continue;
                        }
                    }
                }

                #endregion

                #region 删除线

                if (c == '~' && i + 1 < n && text[i + 1] == '~')
                {
                    int close = text.IndexOf("~~", i + 2, StringComparison.Ordinal);
                    if (close > i + 2 && text[close - 1] != ' ')
                    {
                        if (i > start) AddText(list, text, start, i);
                        string strikeText = text.Substring(i + 2, close - i - 2);
                        list.Add(new MDInline(MDInlineKind.Strike, strikeText) { Md = "~~" + strikeText + "~~" });
                        i = close + 2;
                        start = i;
                        continue;
                    }
                }

                #endregion

                #region 图片 / 超链接

                if (c == '!' && i + 1 < n && text[i + 1] == '[')
                {
                    string label, url;
                    int next;
                    if (MatchLink(text, i + 1, out label, out url, out next))
                    {
                        if (i > start) AddText(list, text, start, i);
                        var img = new MDInline(MDInlineKind.Image, StripTags(label), DecodeEntities(url.Trim()));
                        ParseImageSize(label, img);
                        img.Md = text.Substring(i, next - i);
                        list.Add(img);
                        i = next;
                        start = i;
                        continue;
                    }
                }
                if (c == '[')
                {
                    string label, url;
                    int next;
                    if (MatchLink(text, i, out label, out url, out next))
                    {
                        if (i > start) AddText(list, text, start, i);
                        var inners = ParseInlines(label);
                        // [![alt](img)](href) 图片链接
                        if (inners.Count == 1 && inners[0].Kind == MDInlineKind.Image)
                        {
                            inners[0].Href = DecodeEntities(url.Trim());
                            list.Add(inners[0]);
                        }
                        else
                        {
                            if (inners.Count == 0) inners.Add(new MDInline(MDInlineKind.Text, StripTags(label)));
                            foreach (var it in inners)
                            {
                                if (it.Kind == MDInlineKind.Text) it.Kind = MDInlineKind.Link;
                                if (it.Url == null) it.Url = DecodeEntities(url.Trim());
                                if (it.Md == null) it.Md = it.Text;
                            }
                            list.AddRange(inners);
                        }
                        i = next;
                        start = i;
                        continue;
                    }
                }

                #endregion

                #region HTML 内联

                if (c == '<')
                {
                    int gt = FindTagEnd(text, i);
                    if (gt > i)
                    {
                        string tag = text.Substring(i, gt - i + 1);
                        string name = TagName(tag);
                        if (name.Length > 0)
                        {
                            int after = TryHtmlInline(list, text, i, gt, tag, name, ref start);
                            if (after > i)
                            {
                                i = after;
                                continue;
                            }
                        }
                    }
                }

                #endregion

                #region 自动链接

                if ((c == 'h' || c == 'w' || c == 'f') && (i == 0 || !IsWordChar(text[i - 1])))
                {
                    string? prefix = MatchAutoLinkPrefix(text, i);
                    if (prefix != null)
                    {
                        int end = i + prefix.Length;
                        while (end < n && !char.IsWhiteSpace(text[end]) && text[end] != '<' && text[end] != '>') end++;
                        int cut = end;
                        while (cut > i + prefix.Length && IsTailPunct(text[cut - 1])) cut--;
                        string url = text.Substring(i, cut - i);
                        if (url.Length > prefix.Length)
                        {
                            if (i > start) AddText(list, text, start, i);
                            list.Add(new MDInline(MDInlineKind.Link, url, url) { Md = url });
                            if (cut < end) list.Add(new MDInline(MDInlineKind.Text, text.Substring(cut, end - cut)));
                            i = end;
                            start = i;
                            continue;
                        }
                    }
                }

                #endregion

                i++;
            }
            if (n > start) AddText(list, text, start, n);
            return list;
        }

        static void AddText(List<MDInline> list, string text, int start, int end)
        {
            if (end <= start) return;
            string val = text.Substring(start, end - start);
            if (val.IndexOf('&') >= 0) val = DecodeEntities(val);
            list.Add(new MDInline(MDInlineKind.Text, val));
        }

        static int TryHtmlInline(List<MDInline> list, string text, int i, int gt, string tag, string name, ref int start)
        {
            switch (name)
            {
                case "br":
                case "hr":
                    if (i > start) AddText(list, text, start, i);
                    list.Add(new MDInline(MDInlineKind.Break, ""));
                    start = gt + 1;
                    return gt + 1;
                case "img":
                    {
                        if (i > start) AddText(list, text, start, i);
                        var htmlImg = BuildImage(tag);
                        htmlImg.Md = tag;
                        list.Add(htmlImg);
                        start = gt + 1;
                        return gt + 1;
                    }
                case "b":
                case "strong":
                    return WrapHtmlInline(list, text, i, gt, name, MDInlineKind.Bold, ref start);
                case "i":
                case "em":
                    return WrapHtmlInline(list, text, i, gt, name, MDInlineKind.Italic, ref start);
                case "code":
                case "kbd":
                    return WrapHtmlInline(list, text, i, gt, name, MDInlineKind.Code, ref start);
                case "del":
                case "s":
                case "strike":
                    return WrapHtmlInline(list, text, i, gt, name, MDInlineKind.Strike, ref start);
                case "sub":
                    return WrapHtmlInline(list, text, i, gt, name, MDInlineKind.Sub, ref start);
                case "sup":
                    return WrapHtmlInline(list, text, i, gt, name, MDInlineKind.Sup, ref start);
                case "a":
                    {
                        string href = AttrValue(tag, "href");
                        int close = text.IndexOf("</a", gt, StringComparison.OrdinalIgnoreCase);
                        if (close < 0) return -1;
                        string inner = text.Substring(gt + 1, close - gt - 1);
                        int end = text.IndexOf('>', close);
                        if (end < 0) return -1;
                        if (i > start) AddText(list, text, start, i);
                        var inners = ParseInlines(inner);
                        if (inners.Count == 0) inners.Add(new MDInline(MDInlineKind.Text, StripTags(inner)));
                        foreach (var it in inners)
                        {
                            if (it.Kind == MDInlineKind.Text) it.Kind = MDInlineKind.Link;
                            if (it.Url == null) it.Url = DecodeEntities(href);
                            if (it.Md == null) it.Md = it.Text;
                        }
                        list.AddRange(inners);
                        start = end + 1;
                        return end + 1;
                    }
            }
            return -1;
        }

        static int WrapHtmlInline(List<MDInline> list, string text, int i, int gt, string name, MDInlineKind kind, ref int start)
        {
            int close = text.IndexOf("</" + name, gt, StringComparison.OrdinalIgnoreCase);
            if (close < 0) return -1;
            int end = text.IndexOf('>', close);
            if (end < 0) return -1;
            string inner = text.Substring(gt + 1, close - gt - 1);
            if (i > start) AddText(list, text, start, i);
            var inners = ParseInlines(inner);
            if (inners.Count == 0) inners.Add(new MDInline(MDInlineKind.Text, StripTags(inner)));
            string open = text.Substring(i, gt - i + 1);
            foreach (var it in inners)
            {
                if (it.Kind == MDInlineKind.Text) it.Kind = kind;
                if (it.Md == null) it.Md = open + it.Text + "</" + name + ">";
            }
            list.AddRange(inners);
            start = end + 1;
            return end + 1;
        }

        static void ParseImageSize(string label, MDInline img)
        {
            // ![alt](url "title = 100x200") 形态不做处理，仅识别 HTML 属性
            int p = label.IndexOf("width", StringComparison.OrdinalIgnoreCase);
            if (p >= 0)
            {
                int value;
                if (int.TryParse(AttrValue(label, "width"), out value)) img.Width = value;
            }
            int p2 = label.IndexOf("height", StringComparison.OrdinalIgnoreCase);
            if (p2 >= 0)
            {
                int value;
                if (int.TryParse(AttrValue(label, "height"), out value)) img.Height = value;
            }
        }

        static bool MatchLink(string text, int i, out string label, out string url, out int next)
        {
            label = "";
            url = "";
            next = i;
            if (i >= text.Length || text[i] != '[') return false;
            int depth = 0, j = i;
            int labelEnd = -1;
            for (; j < text.Length; j++)
            {
                char c = text[j];
                if (c == '\\')
                {
                    j++;
                    continue;
                }
                if (c == '[') depth++;
                else if (c == ']')
                {
                    depth--;
                    if (depth == 0)
                    {
                        labelEnd = j;
                        break;
                    }
                }
            }
            if (labelEnd < 0 || labelEnd + 1 >= text.Length || text[labelEnd + 1] != '(') return false;
            int urlStart = labelEnd + 2;
            int depth2 = 1, k = urlStart;
            int urlEnd = -1;
            for (; k < text.Length; k++)
            {
                char c = text[k];
                if (c == '\\')
                {
                    k++;
                    continue;
                }
                if (c == '(') depth2++;
                else if (c == ')')
                {
                    depth2--;
                    if (depth2 == 0)
                    {
                        urlEnd = k;
                        break;
                    }
                }
            }
            if (urlEnd < 0) return false;
            label = text.Substring(i + 1, labelEnd - i - 1);
            string raw = text.Substring(urlStart, urlEnd - urlStart).Trim();
            int sp = raw.IndexOf(' ');
            if (sp > 0) raw = raw.Substring(0, sp);
            raw = raw.Trim('"', '\'', '<', '>');
            url = raw;
            next = urlEnd + 1;
            return true;
        }

        static string? MatchAutoLinkPrefix(string text, int i)
        {
            if (MatchAt(text, i, "https://")) return "https://";
            if (MatchAt(text, i, "http://")) return "http://";
            if (MatchAt(text, i, "ftp://")) return "ftp://";
            if (MatchAt(text, i, "www.")) return "www.";
            return null;
        }

        static bool MatchAt(string text, int i, string value)
        {
            if (i + value.Length > text.Length) return false;
            for (int k = 0; k < value.Length; k++)
            {
                if (char.ToLowerInvariant(text[i + k]) != value[k]) return false;
            }
            return true;
        }

        static bool IsTailPunct(char c) => c == '.' || c == ',' || c == ';' || c == ':' || c == '!' || c == '?' || c == ')' || c == ']' || c == '>' || c == '"' || c == '\'' || c == '。' || c == '，' || c == '、' || c == '；';

        static bool IsEscapable(char c)
        {
            switch (c)
            {
                case '\\':
                case '`':
                case '*':
                case '_':
                case '{':
                case '}':
                case '[':
                case ']':
                case '(':
                case ')':
                case '#':
                case '+':
                case '-':
                case '.':
                case '!':
                case '~':
                case '|':
                case '>':
                case '<':
                case '$':
                case '/':
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 强调标记边界判定，仅 ASCII 字母数字视为词字符（中文等全角字符不应阻断强调）
        /// </summary>
        static bool IsWordChar(char c) => (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9') || c == '_';

        static int RunLen(string text, int i, char c)
        {
            int n = 0;
            while (i + n < text.Length && text[i + n] == c) n++;
            return n;
        }

        /// <summary>
        /// 查找强调闭合标记（闭合前不得为空白；_ 不得为词内闭合，* 允许 Love**is**bold）
        /// </summary>
        static int FindEmphasisClose(string text, int from, string marker, char open)
        {
            int idx = from;
            while (idx < text.Length)
            {
                idx = text.IndexOf(marker, idx, StringComparison.Ordinal);
                if (idx < 0) return -1;
                if (idx == from || char.IsWhiteSpace(text[idx - 1]))
                {
                    idx += marker.Length;
                    continue;
                }
                int after = idx + marker.Length;
                if (after < text.Length && IsWordChar(text[after]) && open == '_')
                {
                    idx += marker.Length;
                    continue;
                }
                return idx;
            }
            return -1;
        }

        static MDAlert ParseAlert(string text)
        {
            if (text.Length < 4 || text[0] != '[' || text[1] != '!') return MDAlert.None;
            int end = text.IndexOf(']');
            if (end < 0) end = text.Length - 1;
            string name = text.Substring(2, end - 2).Trim().ToUpperInvariant();
            switch (name)
            {
                case "NOTE": return MDAlert.Note;
                case "TIP": return MDAlert.Tip;
                case "IMPORTANT": return MDAlert.Important;
                case "WARNING": return MDAlert.Warning;
                case "CAUTION": return MDAlert.Caution;
            }
            return MDAlert.None;
        }

        static string AlertLabel(MDAlert alert)
        {
            switch (alert)
            {
                case MDAlert.Note: return "Note";
                case MDAlert.Tip: return "Tip";
                case MDAlert.Important: return "Important";
                case MDAlert.Warning: return "Warning";
                case MDAlert.Caution: return "Caution";
            }
            return "";
        }

        #endregion

        #region 实体转义

        public static string DecodeEntities(string text)
        {
            if (text.IndexOf('&') < 0) return text;
            var sb = new StringBuilder(text.Length);
            int i = 0;
            while (i < text.Length)
            {
                char c = text[i];
                if (c == '&')
                {
                    int end = text.IndexOf(';', i + 1);
                    if (end > i && end - i <= 10)
                    {
                        string name = text.Substring(i + 1, end - i - 1);
                        string? val = EntityValue(name);
                        if (val != null)
                        {
                            sb.Append(val);
                            i = end + 1;
                            continue;
                        }
                    }
                }
                sb.Append(c);
                i++;
            }
            return sb.ToString();
        }

        static string? EntityValue(string name)
        {
            switch (name)
            {
                case "amp": return "&";
                case "lt": return "<";
                case "gt": return ">";
                case "quot": return "\"";
                case "apos": return "'";
                case "nbsp": return " ";
                case "#39": return "'";
                case "#34": return "\"";
            }
            if (name.Length > 1 && name[0] == '#' && name[1] == 'x')
            {
                int code;
                if (int.TryParse(name.Substring(2), System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out code))
                    return ((char)code).ToString();
            }
            else if (name.Length > 1 && name[0] == '#')
            {
                int code;
                if (int.TryParse(name.Substring(1), out code)) return ((char)code).ToString();
            }
            return null;
        }

        #endregion
    }
}