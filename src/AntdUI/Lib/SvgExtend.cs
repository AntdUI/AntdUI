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
// GITEE: https://gitee.com/AntdUI/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

using System.Drawing;

namespace AntdUI
{
    public static class SvgExtend
    {
        public static Bitmap? GetImgExtend(string svg, Rectangle rect, Color? color = null)
        {
            if (rect.Width > 0 && rect.Height > 0) return SvgToBmp(svg, rect.Width, rect.Height, color);
            return null;
        }
        public static bool GetImgExtend(this Canvas g, string svg, Rectangle rect, Color? color = null)
        {
            if (rect.Width > 0 && rect.Height > 0)
            {
                using (var bmp = SvgToBmp(svg, rect.Width, rect.Height, color))
                {
                    if (bmp == null) return false;
                    g.Image(bmp, rect);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// SVG转图片
        /// </summary>
        /// <param name="svg">代码</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        /// <param name="color">颜色</param>
        /// <returns>Bitmap</returns>
        public static Bitmap? SvgToBmp(this string svg, int width, int height, Color? color)
        {
            var doc = SvgDocument(svg);
            if (doc == null) return null;
            if (color.HasValue) doc.Fill = new Svg.SvgColourServer(color.Value);
            doc.Width = width;
            doc.Height = height;
            return doc.Draw();
        }

        /// <summary>
        /// SVG转图片
        /// </summary>
        /// <param name="svg">代码</param>
        /// <returns>Bitmap</returns>
        public static Bitmap? SvgToBmp(this string svg)
        {
            var doc = SvgDocument(svg);
            if (doc == null) return null;
            float dpi = Config.Dpi;
            if (dpi != 1F)
            {
                doc.Width = doc.Width * dpi;
                doc.Height = doc.Height * dpi;
            }
            return doc.Draw();
        }

        static Svg.SvgDocument? SvgDocument(string svg)
        {
            if (svg.StartsWith("<svg")) return Svg.SvgDocument.FromSvg<Svg.SvgDocument>(svg);
            else if (SvgDb.Custom.TryGetValue(svg, out var rsvg)) return Svg.SvgDocument.FromSvg<Svg.SvgDocument>(rsvg);
            return null;
        }
    }
}