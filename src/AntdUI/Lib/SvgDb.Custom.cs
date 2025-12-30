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
                Custom.Add(s.Substring(0, i), s.Substring(i + 1));
            }
        }

        public static Dictionary<string, string> Custom;

        public static Dictionary<string, string> Emoji = new Dictionary<string, string>(0);
    }
}