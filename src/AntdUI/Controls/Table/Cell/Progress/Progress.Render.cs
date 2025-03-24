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

using System.Drawing;
using System.Drawing.Drawing2D;

namespace AntdUI
{
    partial class CellProgress
    {
        public override void PaintBack(Canvas g) { }

        public override void Paint(Canvas g, Font font, bool enable, SolidBrush fore)
        {
            Color _color = Fill ?? Colour.Primary.Get("Progress"), _back = Back ?? Colour.FillSecondary.Get("Progress");
            if (Shape == TShape.Circle)
            {
                float w = Radius * Config.Dpi;
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
                float radius = Radius * Config.Dpi;
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
                                using (var g2 = Graphics.FromImage(bmp).High())
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

        public override Size GetSize(Canvas g, Font font, int gap, int gap2)
        {
            if (Size.HasValue) return new Size((int)(Size.Value.Width * Config.Dpi), (int)(Size.Value.Height * Config.Dpi));
            int height = g.MeasureString(Config.NullText, font).Height;
            if (Shape == TShape.Circle)
            {
                int size = gap2 + height;
                return new Size(size, size);
            }
            else return new Size(height * 2, height / 2);
        }

        public override void SetRect(Canvas g, Font font, Rectangle rect, Size size, int maxwidth, int gap, int gap2)
        {
            int w = rect.Width, h = size.Height;
            if (Shape == TShape.Circle)
            {
                w = size.Width - gap2;
                h = size.Height - gap2;
            }
            Rect = new Rectangle(rect.X + (rect.Width - w) / 2, rect.Y + (rect.Height - h) / 2, w, h);
        }
    }
}