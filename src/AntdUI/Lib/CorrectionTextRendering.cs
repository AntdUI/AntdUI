// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
// THE AntdUI PROJECT IS AN WINFORM LIBRARY LICENSED UNDER THE Apache-2.0 License.
// LICENSED UNDER THE Apache License, VERSION 2.0 (THE "License")
// YOU MAY NOT USE THIS FILE EXCEPT IN COMPLIANCE WITH THE License.
// YOU MAY OBTAIN A COPY OF THE LICENSE AT
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// UNLESS REQUIRED BY APPLICABLE LAW OR AGREED TO IN WRITING, SOFTWARE
// DISTRIBUTED UNDER THE LICENSE IS DISTRIBUTED ON AN "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED.
// SEE THE LICENSE FOR THE SPECIFIC LANGUAGE GOVERNING PERMISSIONS AND
// LIMITATIONS UNDER THE License.
// GITEE: https://gitee.com/antdui/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

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
            using (var s_f = Helper.SF_NoWrap())
            {
                using (var bmp = new Bitmap(size, size))
                {
                    using (var g_o = Graphics.FromImage(bmp).High())
                    {
                        g_o.DrawString(text, font, Brushes.Black, new Rectangle(0, 0, bmp.Width, bmp.Height), s_f);
                    }
                    TextRealY(bmp, out var ry, out var rheight);
                    float ready = ry + rheight / 2F;
                    return cs - ready;
                }
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

        #region 渲染字体

        public static void DrawStr(this Graphics g, string? s, Font font, Brush brush, RectangleF layoutRectangle)
        {
            CORE(font, s, ref layoutRectangle);
            g.DrawString(s, font, brush, layoutRectangle, null);
        }

        public static void DrawStr(this Graphics g, string? s, Font font, Brush brush, Rectangle layoutRectangle, StringFormat? format)
        {
            CORE(font, s, ref layoutRectangle);
            g.DrawString(s, font, brush, layoutRectangle, format);
        }

        public static void DrawStr(this Graphics g, string? s, Font font, Brush brush, RectangleF layoutRectangle, StringFormat? format)
        {
            CORE(font, s, ref layoutRectangle);
            g.DrawString(s, font, brush, layoutRectangle, format);
        }

        #endregion

        #region 误差核心

        static void CORE(Font font, string? text, ref RectangleF layoutRectangle)
        {
            if (enable && text != null)
            {
                if (text.ContainsChinese())
                {
                    if (tmpChinese.TryGetValue(font.Name, out var oy)) layoutRectangle.Offset(0, oy * font.Size);
                }
                else if (tmpEnglish.TryGetValue(font.Name, out var oy)) layoutRectangle.Offset(0, oy * font.Size);
            }
        }

        static void CORE(Font font, string? text, ref Rectangle layoutRectangle)
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