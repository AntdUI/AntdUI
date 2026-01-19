// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.Collections.Concurrent;
using System.Drawing;

namespace AntdUI
{
    public class TFixFont
    {
        ConcurrentDictionary<string, int> font_dir = new ConcurrentDictionary<string, int>();
        int? font_null;

        /// <summary>
        /// 计算字体宽度
        /// </summary>
        public int Width(Canvas g, Font font, string txt)
        {
            if (txt == "\t") return WidthCore(g, font, " ") * 8;
            else if (txt == "\n" || txt == "\r\n") return WidthCore(g, font, " ");
            else return WidthCore(g, font, txt);
        }
        int WidthCore(Canvas g, Font font, string txt)
        {
            if (font_dir.TryGetValue(txt, out var find)) return find;
            else
            {
                int tmp = g.MeasureString(txt, font).Width;
                font_dir.TryAdd(txt, tmp);
                return tmp;
            }
        }

        /// <summary>
        /// 计算字体高度
        /// </summary>
        public int Height(Canvas g, Font font)
        {
            if (font_null.HasValue) return font_null.Value;
            int tmp = g.MeasureString(Config.NullText, font).Height;
            font_null = tmp;
            return tmp;
        }

        public void Clear()
        {
            font_null = null;
            font_dir.Clear();
        }
    }
}