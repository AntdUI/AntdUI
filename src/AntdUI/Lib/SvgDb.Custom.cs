// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.Collections.Generic;

namespace AntdUI
{
    partial class SvgDb
    {
        static SvgDb()
        {
            var datas = Properties.Resources.Custom.Split('|');
            Custom = new Dictionary<string, string>(datas.Length);
#if DEBUG
            var text = new List<string>(datas.Length);
#endif
            foreach (string s in datas)
            {
                var i = s.IndexOf(":");
#if DEBUG
                text.Add(s.Substring(0, i) + ":" + YS(Rest(s.Substring(i + 1))));
#endif
                Custom.Add(s.Substring(0, i), Rest(s.Substring(i + 1)));
            }
#if DEBUG
            //System.IO.File.WriteAllText("txt.txt", string.Join("|", text));
#endif
        }

        static string[][] rule = new string[][] {
            new string[] { "viewBox=\"0 0 1024 1024\">", "[VB2" },
            new string[] { "viewBox=\"0 0 32 32\" fill=\"none\">", "[VB3N" },
            new string[] { "viewBox=\"0 0 32 32\">", "[VB3" },
            new string[] { "viewBox=\"64 64 896 896\">", "[VB69" },
            new string[] { "viewBox=\"", "[VBD" },
            new string[] { "<path d=\"", "[PD" },
            new string[] { "\"></path>", "[PG" },
            new string[] { "</path>", "[PE" },
            new string[] { "fill=\"none\"", "fillNA" },
            new string[] { "stroke=\"none\"", "strokeNA" },
            new string[] { "fill-rule=\"evenodd\"" ,"freOD" },
            new string[] { "fill=\"currentColor\"", "fillCC" },
            new string[] { "stroke=\"currentColor\"", "strokeCC" },
            new string[] { "<circle", "[CIRC" },
            new string[] { "<rect", "[RECT" },
            new string[] { "<line", "[LINE" },
            new string[] { "<polyline", "[PLINE" },
            new string[] { "<polygon", "[PGON" },
            new string[] { "<ellipse", "[ELLIP" },
            new string[] { "<g", "[G" },
            new string[] { "</g>", "[GE" }
        };

#if DEBUG
        static string YS(string svg)
        {
            var sb = new System.Text.StringBuilder(svg);
            foreach (var it in rule) sb.Replace(it[0], it[1]);
            var txt = sb.ToString();
            txt = txt.Substring(5);
            return txt.Substring(0, txt.Length - 6);
        }
#endif

        static string Rest(string svg)
        {
            var sb = new System.Text.StringBuilder(svg);
            for (int i = rule.Length - 1; i >= 0; i--)
            {
                var it = rule[i];
                sb.Replace(it[1], it[0]);
            }
            return "<svg " + sb.ToString() + "</svg>";
        }

        public static Dictionary<string, string> Custom;

        public static Dictionary<string, string> Emoji = new Dictionary<string, string>(0);
    }
}