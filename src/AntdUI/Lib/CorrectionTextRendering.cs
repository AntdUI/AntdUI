// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections.Generic;
using System.Drawing;

namespace AntdUI
{
    /// <summary>
    /// 修正文本渲染
    /// </summary>
    public static class CorrectionTextRendering
    {
        internal static Dictionary<string, float> tmpChinese = new Dictionary<string, float>();
        internal static Dictionary<string, float> tmpEnglish = new Dictionary<string, float>();

        static bool enable = false;
        static string defTextChinese = "不";
        static string defTextEnglish = "X";
        static int defFontSize = 20;
        internal static void Set(string familie)
        {
            enable = true;
            using (var font = new Font(familie, defFontSize))
            {
                int size = Helper.GDI(g =>
                {
                    return (int)(g.MeasureString(defTextChinese, font).Height * 1.2F);
                });
                float valChinese = GetFontOffset(font, size, defTextChinese);
                float valEnglish = GetFontOffset(font, size, defTextEnglish);
                if (valChinese <= 1 && valChinese >= -1)
                {
                    //误差 1以内可以接受
                    lock (tmpChinese)
                    {
                        if (tmpChinese.ContainsKey(familie)) tmpChinese.Remove(familie);
                    }
                }
                else
                {
                    var oy = valChinese / defFontSize;
                    lock (tmpChinese)
                    {
                        if (tmpChinese.ContainsKey(familie)) tmpChinese[familie] = oy;
                        else tmpChinese.Add(familie, oy);
                    }
                }

                if (valEnglish <= 1 && valEnglish >= -1)
                {
                    //误差 1以内可以接受
                    lock (tmpEnglish)
                    {
                        if (tmpEnglish.ContainsKey(familie)) tmpEnglish.Remove(familie);
                    }
                }
                else
                {
                    var oy = valEnglish / defFontSize;
                    lock (tmpEnglish)
                    {
                        if (tmpEnglish.ContainsKey(familie)) tmpEnglish[familie] = oy;
                        else tmpEnglish.Add(familie, oy);
                    }
                }
            }
        }

        static float GetFontOffset(Font font, int size, string text)
        {
            float cs = size / 2F;
            using (var bmp = new Bitmap(size, size))
            {
                using (var g_o = Graphics.FromImage(bmp).High())
                {
                    g_o.String(text, font, Brushes.Black, new Rectangle(0, 0, bmp.Width, bmp.Height), FormatFlags.Center | FormatFlags.NoWrap);
                }
                TextRealY(bmp, out var ry, out var rheight);
                float ready = ry + rheight / 2F;
                return cs - ready;
            }
        }

        /// <summary>
        /// 获取文本真实Y高度
        /// </summary>
        /// <param name="bmp">图片</param>
        /// <param name="y">Y</param>
        /// <param name="h">高度</param>
        public static void TextRealY(Bitmap bmp, out int y, out int h)
        {
            y = TextRealY(bmp);
            h = TextRealHeight(bmp, y);
        }

        static int TextRealY(Bitmap bmp)
        {
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    if (bmp.GetPixel(x, y).A > 0) return y - 1;
                }
            }
            return 0;
        }

        static int TextRealHeight(Bitmap bmp, int _y)
        {
            for (int y = bmp.Height - 1; y > _y; y--)
            {
                int count = 0;
                for (int x = 0; x < bmp.Width; x++)
                {
                    if (bmp.GetPixel(x, y).A > 0) count++;
                }
                if (count > 0) return (y + 1) - _y;
            }
            return bmp.Height - _y;
        }

        #region 误差核心

        internal static void CORE(Font font, string? text, ref RectangleF layoutRectangle)
        {
            if (enable && text != null && (tmpEnglish.Count > 0 || tmpChinese.Count > 0))
            {
                if (text.ContainsChinese())
                {
                    if (tmpChinese.TryGetValue(font.Name, out var oy)) layoutRectangle.Offset(0, oy * font.Size);
                }
                else if (tmpEnglish.TryGetValue(font.Name, out var oy)) layoutRectangle.Offset(0, oy * font.Size);
            }
        }

        internal static void CORE(Font font, string? text, ref Rectangle layoutRectangle)
        {
            if (enable && text != null)
            {
                if (text.ContainsChinese())
                {
                    if (tmpChinese.TryGetValue(font.Name, out var oy)) layoutRectangle.Offset(0, (int)Math.Round(oy * font.Size));
                }
                else if (tmpEnglish.TryGetValue(font.Name, out var oy)) layoutRectangle.Offset(0, (int)Math.Round(oy * font.Size));
            }
        }

        static bool ContainsChinese(this string input)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(input, @"[\u4e00-\u9fa5]");
        }

        #endregion
    }
}