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
using System.Drawing;
using System.Drawing.Drawing2D;

namespace AntdUI
{
    partial class CellImage
    {
        internal override void PaintBack(Canvas g) { }

        internal override void Paint(Canvas g, Font font, SolidBrush fore)
        {
            float radius = Radius * Config.Dpi;
            using (var path = Rect.RoundPath(radius))
            {
                using (var bmp = new Bitmap(Rect.Width, Rect.Height))
                {
                    using (var g2 = Graphics.FromImage(bmp).High())
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
                        else
                        {
                            g.Fill(brush, path);
                        }
                    }
                }

                if (BorderWidth > 0 && BorderColor.HasValue) g.Draw(BorderColor.Value, BorderWidth * Config.Dpi, path);
            }
        }

        internal override Size GetSize(Canvas g, Font font, int gap, int gap2)
        {
            if (Size.HasValue)
            {
                return new Size((int)Math.Ceiling(Size.Value.Width * Config.Dpi) + gap2, (int)Math.Ceiling(Size.Value.Height * Config.Dpi) + gap2);
            }
            else
            {
                int size = gap2 + g.MeasureString(Config.NullText, font).Height;
                return new Size(size, size);
            }
        }

        internal Rectangle Rect;
        internal override void SetRect(Canvas g, Font font, Rectangle rect, Size size, int gap, int gap2)
        {
            int w = size.Width - gap2, h = size.Height - gap2;
            Rect = new Rectangle(rect.X + (rect.Width - w) / 2, rect.Y + (rect.Height - h) / 2, w, h);
        }
    }
}