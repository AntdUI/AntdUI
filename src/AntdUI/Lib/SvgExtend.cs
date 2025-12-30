// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

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
        public static bool GetImgCanvas(this Canvas g, string svg, Rectangle rect, Color? color = null)
        {
            if (rect.Width > 0 && rect.Height > 0)
            {
                var doc = SvgDocument(svg);
                if (doc == null) return false;
                if (color.HasValue) doc.Fill = new Svg.SvgColourServer(color.Value);
                doc.Width = rect.Width;
                doc.Height = rect.Height;
                var state = g.Save();
                g.TranslateTransform(rect.X, rect.Y);
                doc.Draw(g, rect.Size);
                g.Restore(state);
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
            if (dpi == 1F) return doc.Draw();
            Svg.SvgUnitType tw = doc.Width.Type, th = doc.Height.Type;
            if (tw == Svg.SvgUnitType.Percentage && th == Svg.SvgUnitType.Percentage)
            {
                var size = doc.Bounds;
                doc.Width = size.Width * dpi;
                doc.Height = size.Height * dpi;
            }
            else
            {
                doc.Width *= dpi;
                doc.Height *= dpi;
            }
            return doc.Draw();
        }

        /// <summary>
        /// SVG转图片
        /// </summary>
        /// <param name="svg">代码</param>
        /// <param name="g">绘制</param>
        /// <returns>Bitmap</returns>
        public static Bitmap? SvgToBmp(this string svg, Canvas g)
        {
            var doc = SvgDocument(svg);
            if (doc == null) return null;
            if (g.Dpi == 1F) return doc.Draw();
            Svg.SvgUnitType tw = doc.Width.Type, th = doc.Height.Type;
            if (tw == Svg.SvgUnitType.Percentage && th == Svg.SvgUnitType.Percentage)
            {
                var size = doc.Bounds;
                doc.Width = size.Width * g.Dpi;
                doc.Height = size.Height * g.Dpi;
            }
            else
            {
                doc.Width *= g.Dpi;
                doc.Height *= g.Dpi;
            }
            return doc.Draw();
        }

        static Svg.SvgDocument? SvgDocument(string svg)
        {
            if (svg.StartsWith("<svg")) return Svg.SvgDocument.FromSvg<Svg.SvgDocument>(svg);
            else if (svg.Contains("svg+xml;base64,"))
            {
                var xml = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(svg.Substring(svg.IndexOf("base64,") + 7)));
                return Svg.SvgDocument.FromSvg<Svg.SvgDocument>(xml);
            }
            else if (SvgDb.Custom.TryGetValue(svg, out var rsvg)) return Svg.SvgDocument.FromSvg<Svg.SvgDocument>(rsvg);
            return null;
        }
    }
}