// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace AntdUI
{
    partial class CellImage
    {
        public override void PaintBack(Canvas g) { }

        public override void Paint(Canvas g, Font font, bool enable, SolidBrush fore)
        {
            float radius = Radius * g.Dpi;
            using (var path = Rect.RoundPath(radius))
            {
                using (var bmp = new Bitmap(Rect.Width, Rect.Height))
                {
                    using (var g2 = Graphics.FromImage(bmp).High(g.Dpi))
                    {
                        if (ImageSvg != null)
                        {
                            using (var bmpsvg = ImageSvg.SvgToBmp(Rect.Width, Rect.Height, FillSvg))
                            {
                                if (bmpsvg != null) g2.Image(new RectangleF(0, 0, Rect.Width, Rect.Height), bmpsvg, ImageFit);
                            }
                        }
                        else if (image != null) g2.Image(new RectangleF(0, 0, Rect.Width, Rect.Height), image, ImageFit);
                    }
                    using (var brush = new TextureBrush(bmp, WrapMode.Clamp))
                    {
                        brush.TranslateTransform(Rect.X, Rect.Y);
                        if (Round) g.FillEllipse(brush, Rect);
                        else g.Fill(brush, path);
                    }
                }

                if (BorderWidth > 0 && BorderColor.HasValue) g.Draw(BorderColor.Value, BorderWidth * g.Dpi, path);
            }
        }

        public override Size GetSize(Canvas g, Font font, TableGaps gap)
        {
            if (Size.HasValue) return new Size((int)Math.Ceiling(Size.Value.Width * g.Dpi), (int)Math.Ceiling(Size.Value.Height * g.Dpi));
            else
            {
                int size = gap.x2 + g.MeasureString(Config.NullText, font).Height;
                return new Size(size, size);
            }
        }

        public override void SetRect(Canvas g, Font font, Rectangle rect, Size size, int maxwidth, TableGaps gap)
        {
            int w = size.Width - gap.y2, h = size.Height - gap.y2;
            Rect = new Rectangle(rect.X + (rect.Width - w) / 2, rect.Y + (rect.Height - h) / 2, w, h);
        }
    }
}