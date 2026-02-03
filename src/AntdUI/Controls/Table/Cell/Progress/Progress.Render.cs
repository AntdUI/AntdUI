// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.Drawing;
using System.Drawing.Drawing2D;

namespace AntdUI
{
    partial class CellProgress
    {
        public override void PaintBack(Canvas g) { }

        public override void Paint(Canvas g, Font font, bool enable, SolidBrush fore)
        {
            Color _color = Fill ?? Colour.Primary.Get(nameof(Progress), PARENT.PARENT.ColorScheme), _back = Back ?? Colour.FillSecondary.Get(nameof(Progress), PARENT.PARENT.ColorScheme);
            if (Shape == TShape.Circle)
            {
                float w = Radius * g.Dpi;
                g.DrawEllipse(_back, w, Rect);
                if (Value > 0)
                {
                    int max = (int)(360 * Value);
                    using (var brush = new Pen(_color, w))
                    {
                        brush.StartCap = brush.EndCap = LineCap.Round;
                        g.DrawArc(brush, Rect, -90, max);
                    }
                }
            }
            else
            {
                float radius = Radius * g.Dpi;
                if (Shape == TShape.Round) radius = Rect.Height;

                using (var path = Rect.RoundPath(radius))
                {
                    g.Fill(_back, path);
                    if (Value > 0)
                    {
                        var _w = Rect.Width * Value;
                        if (_w > radius)
                        {
                            using (var path_prog = new RectangleF(Rect.X, Rect.Y, _w, Rect.Height).RoundPath(radius))
                            {
                                g.Fill(_color, path_prog);
                            }
                        }
                        else
                        {
                            using (var bmp = new Bitmap(Rect.Width, Rect.Height))
                            {
                                using (var g2 = Graphics.FromImage(bmp).High(g.Dpi))
                                {
                                    using (var brush = new SolidBrush(_color))
                                    {
                                        g2.FillEllipse(brush, new RectangleF(0, 0, _w, Rect.Height));
                                    }
                                }
                                using (var brush = new TextureBrush(bmp, WrapMode.Clamp))
                                {
                                    brush.TranslateTransform(Rect.X, Rect.Y);
                                    g.Fill(brush, path);
                                }
                            }
                        }
                    }
                }
            }
        }

        public override Size GetSize(Canvas g, Font font, TableGaps gap)
        {
            if (Size.HasValue) return new Size((int)(Size.Value.Width * g.Dpi), (int)(Size.Value.Height * g.Dpi));
            int height = g.MeasureString(Config.NullText, font).Height;
            if (Shape == TShape.Circle)
            {
                int size = gap.x2 + height;
                return new Size(size, size);
            }
            else return new Size(height * 2, height / 2);
        }

        public override void SetRect(Canvas g, Font font, Rectangle rect, Size size, int maxwidth, TableGaps gap)
        {
            int w = rect.Width, h = size.Height;
            if (Shape == TShape.Circle)
            {
                w = size.Width - gap.x2;
                h = size.Height - gap.y2;
            }
            Rect = new Rectangle(rect.X + (rect.Width - w) / 2, rect.Y + (rect.Height - h) / 2, w, h);
        }
    }
}