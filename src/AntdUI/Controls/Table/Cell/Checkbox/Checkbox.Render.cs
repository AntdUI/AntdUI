﻿// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
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
    partial class CellCheckbox
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
            float radius = dot_size * .2F;
            using (var path = icon_rect.RoundPath(radius))
            {
                var bor2 = 2F * Config.Dpi;
                if (enabled)
                {
                    var color = fill ?? Colour.Primary.Get("Checkbox", ColorScheme);
                    if (AnimationCheck)
                    {
                        float dot = dot_size * 0.3F, alpha = 255 * AnimationCheckValue;
                        g.Fill(Helper.ToColor(alpha, color), path);
                        using (var pen = new Pen(Helper.ToColor(alpha, Colour.BgBase.Get("Checkbox", ColorScheme)), 2.6F * Config.Dpi))
                        {
                            g.DrawLines(pen, icon_rect.CheckArrow());
                        }
                        if (_checked)
                        {
                            float max = icon_rect.Height + ((rect.Height - icon_rect.Height) * AnimationCheckValue), alpha2 = 100 * (1F - AnimationCheckValue);
                            using (var brush = new SolidBrush(Helper.ToColor(alpha2, color)))
                            {
                                g.FillEllipse(brush, new RectangleF(icon_rect.X + (icon_rect.Width - max) / 2F, icon_rect.Y + (icon_rect.Height - max) / 2F, max, max));
                            }
                        }
                        g.Draw(color, bor2, path);
                    }
                    else if (_checked)
                    {
                        g.Fill(color, path);
                        g.DrawLines(Colour.BgBase.Get("Checkbox", ColorScheme), 2.6F * Config.Dpi, icon_rect.CheckArrow());
                    }
                    else
                    {
                        if (AnimationHover) g.Draw(Colour.BorderColor.Get("Checkbox", ColorScheme).BlendColors(AnimationHoverValue, color), bor2, path);
                        else if (ExtraMouseHover) g.Draw(color, bor2, path);
                        else g.Draw(Colour.BorderColor.Get("Checkbox", ColorScheme), bor2, path);
                    }
                }
                else
                {
                    g.Fill(Colour.FillQuaternary.Get("Checkbox", ColorScheme), path);
                    if (_checked) g.DrawLines(Colour.TextQuaternary.Get("Checkbox", ColorScheme), 2.6F * Config.Dpi, icon_rect.CheckArrow());
                    g.Draw(Colour.BorderColorDisable.Get("Checkbox", ColorScheme), bor2, path);
                }
            }
        }

        bool nullText = false;
        public override Size GetSize(Canvas g, Font font, int gap, int gap2)
        {
            nullText = string.IsNullOrWhiteSpace(Text);
            if (nullText)
            {
                var font_size = g.MeasureString(Config.NullText, Font ?? font, 0, PARENT.PARENT.sf);
                return new Size(font_size.Height + gap, font_size.Height + gap);
            }
            else
            {
                var font_size = g.MeasureText(Text, Font ?? font, 0, PARENT.PARENT.sf);
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
                var font_size = g.MeasureString(Config.NullText, Font ?? font, 0, PARENT.PARENT.sf);
                RectIcon = new Rectangle(Rect.X + (Rect.Width - font_size.Height) / 2, Rect.Y + (Rect.Height - font_size.Height) / 2, font_size.Height, font_size.Height);
            }
            else
            {
                var font_size = g.MeasureText(Text, Font ?? font, 0, PARENT.PARENT.sf);
                Rect.IconRectL(font_size.Height, out var icon_rect, out var text_rect);
                RectIcon = icon_rect;
                RectText = text_rect;
            }
        }
    }
}