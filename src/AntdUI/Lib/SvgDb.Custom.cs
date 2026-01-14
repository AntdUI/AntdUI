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
            foreach (string s in datas)
            {
                var i = s.IndexOf(":");
                Custom.Add(s.Substring(0, i), Rest(s.Substring(i + 1)));
            }
        }

        static string Rest(string svg) => "<svg " + svg.Replace("[VBD", "viewBox=\"").Replace("[VB2", "viewBox=\"0 0 1024 1024\">").Replace("[PD", "<path d=\"").Replace("[PE", "</path>").Replace("[PG", "\"></path>") + "</svg>";

        static string YS(string svg)
        {
            return svg.Replace("viewBox=\"0 0 1024 1024\">", "[VB2").Replace("viewBox=\"", "[VBD").Replace("<path d=\"", "[PD").Replace("\"></path>", "[PG").Replace("</path>", "[PE");
        }

        public static Dictionary<string, string> Custom;

        public static Dictionary<string, string> Emoji = new Dictionary<string, string>(0);
    }
}