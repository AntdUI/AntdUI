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
// GITCODE: https://gitcode.com/AntdUI/AntdUI
// GITEE: https://gitee.com/AntdUI/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

using System.Drawing;
using System.Drawing.Drawing2D;

namespace AntdUI
{
    partial class CellRadio
    {
        public override void PaintBack(Canvas g)
        {
        }

        public override void Paint(Canvas g, Font font, bool enable, SolidBrush fore)
        {
            if (nullText) PaintChecked(g, Rect, enabled, RectIcon);
            else
            {
                PaintChecked(g, Rect, enabled, RectIcon);
                if (Fore.HasValue) g.DrawText(Text, Font ?? font, Fore.Value, RectText, Table.StringFormat(PARENT.COLUMN));
                else g.DrawText(Text, Font ?? font, fore, RectText, Table.StringFormat(PARENT.COLUMN));
            }
        }

        void PaintChecked(Canvas g, Rectangle rect, bool enabled, Rectangle icon_rect)
        {
            var ColorScheme = PARENT.PARENT.ColorScheme;
            float dot_size = icon_rect.Height;
            var bor2 = 2F * Config.Dpi;
            if (enabled)
            {
                var color = fill ?? Colour.Primary.Get("Radio", ColorScheme);
                if (AnimationCheck)
                {
                    float dot = dot_size * 0.3F;
                    using (var path = new GraphicsPath())
                    {
                        float dot_ant = dot_size - dot * AnimationCheckValue, dot_ant2 = dot_ant / 2F, alpha = 255 * AnimationCheckValue;
                        path.AddEllipse(icon_rect);
                        path.AddEllipse(new RectangleF(icon_rect.X + dot_ant2, icon_rect.Y + dot_ant2, icon_rect.Width - dot_ant, icon_rect.Height - dot_ant));
                        g.Fill(Helper.ToColor(alpha, color), path);
                    }
                    if (_checked)
                    {
                        float max = icon_rect.Height + ((rect.Height - icon_rect.Height) * AnimationCheckValue), alpha2 = 100 * (1F - AnimationCheckValue);
                        g.FillEllipse(Helper.ToColor(alpha2, color), new RectangleF(icon_rect.X + (icon_rect.Width - max) / 2F, icon_rect.Y + (icon_rect.Height - max) / 2F, max, max));
                    }
                    g.DrawEllipse(color, bor2, icon_rect);
                }
                else if (_checked)
                {
                    float dot = dot_size * 0.3F, dot2 = dot / 2F;
                    g.DrawEllipse(Color.FromArgb(250, color), dot, new RectangleF(icon_rect.X + dot2, icon_rect.Y + dot2, icon_rect.Width - dot, icon_rect.Height - dot));
                    g.DrawEllipse(color, bor2, icon_rect);
                }
                else
                {
                    if (AnimationHover) g.DrawEllipse(Colour.BorderColor.Get("Radio", ColorScheme).BlendColors(AnimationHoverValue, color), bor2, icon_rect);
                    else if (ExtraMouseHover) g.DrawEllipse(color, bor2, icon_rect);
                    else g.DrawEllipse(Colour.BorderColor.Get("Radio", ColorScheme), bor2, icon_rect);
                }
            }
            else
            {
                g.FillEllipse(Colour.FillQuaternary.Get("Radio", ColorScheme), icon_rect);
                if (_checked)
                {
                    float dot = dot_size / 2F, dot2 = dot / 2F;
                    g.FillEllipse(Colour.TextQuaternary.Get("Radio", ColorScheme), new RectangleF(icon_rect.X + dot2, icon_rect.Y + dot2, icon_rect.Width - dot, icon_rect.Height - dot));
                }
                g.DrawEllipse(Colour.BorderColorDisable.Get("Radio", ColorScheme), bor2, icon_rect);
            }
        }

        bool nullText = false;
        public override Size GetSize(Canvas g, Font font, int gap, int gap2)
        {
            nullText = string.IsNullOrWhiteSpace(Text);
            if (nullText)
            {
                var font_size = g.MeasureString(Config.NullText, Font ?? font);
                return new Size(font_size.Height + gap, font_size.Height + gap);
            }
            else
            {
                var font_size = g.MeasureText(Text, Font ?? font);
                return new Size(font_size.Width + font_size.Height + gap, font_size.Height + gap);
            }
        }

        Rectangle RectText, RectIcon;
        public override void SetRect(Canvas g, Font font, Rectangle rect, Size size, int maxwidth, int gap, int gap2)
        {
            int width = rect.Width;
            if (width > maxwidth) width = maxwidth;
            Rect = new Rectangle(rect.X, rect.Y + (rect.Height - size.Height) / 2, width, size.Height);
            if (nullText)
            {
                var font_size = g.MeasureString(Config.NullText, Font ?? font);
                RectIcon = new Rectangle(Rect.X + (Rect.Width - font_size.Height) / 2, Rect.Y + (Rect.Height - font_size.Height) / 2, font_size.Height, font_size.Height);
            }
            else
            {
                var font_size = g.MeasureText(Text, Font ?? font);
                Rect.IconRectL(font_size.Height, out var icon_rect, out var text_rect);
                RectIcon = icon_rect;
                RectText = text_rect;
            }
        }
    }
}