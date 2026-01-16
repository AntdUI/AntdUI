// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

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
            var Dpi = PARENT.PARENT.Dpi;
            float dot_size = icon_rect.Height;
            float radius = dot_size * .2F;
            using (var path = icon_rect.RoundPath(radius))
            {
                var bor2 = 2F * Dpi;
                if (enabled)
                {
                    var color = fill ?? Colour.Primary.Get(nameof(Checkbox), ColorScheme);
                    if (AnimationCheck)
                    {
                        var alpha = 255 * AnimationCheckValue;
                        if (checkState == System.Windows.Forms.CheckState.Indeterminate || (checkStateOld == System.Windows.Forms.CheckState.Indeterminate && !_checked))
                        {
                            g.Draw(Colour.BorderColor.Get(nameof(Checkbox), ColorScheme), bor2, path);
                            g.Fill(Helper.ToColor(alpha, Colour.Primary.Get(nameof(Checkbox), ColorScheme)), Checkbox.PaintBlock(icon_rect));
                        }
                        else
                        {
                            var dot = dot_size * 0.3F;
                            g.Fill(Helper.ToColor(alpha, color), path);
                            using (var pen = new Pen(Helper.ToColor(alpha, Colour.BgBase.Get(nameof(Checkbox), ColorScheme)), 2.6F * Dpi))
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
                    }
                    else if (checkState == System.Windows.Forms.CheckState.Indeterminate)
                    {
                        g.Draw(Colour.BorderColor.Get(nameof(Checkbox), ColorScheme), bor2, path);
                        g.Fill(Colour.Primary.Get(nameof(Checkbox), ColorScheme), Checkbox.PaintBlock(icon_rect));
                    }
                    else if (_checked)
                    {
                        g.Fill(color, path);
                        g.DrawLines(Colour.BgBase.Get(nameof(Checkbox), ColorScheme), 2.6F * Dpi, icon_rect.CheckArrow());
                    }
                    else
                    {
                        if (AnimationHover) g.Draw(Colour.BorderColor.Get(nameof(Checkbox), ColorScheme).BlendColors(AnimationHoverValue, color), bor2, path);
                        else if (ExtraMouseHover) g.Draw(color, bor2, path);
                        else g.Draw(Colour.BorderColor.Get(nameof(Checkbox), ColorScheme), bor2, path);
                    }
                }
                else
                {
                    g.Fill(Colour.FillQuaternary.Get(nameof(Checkbox), ColorScheme), path);
                    if (_checked) g.DrawLines(Colour.TextQuaternary.Get(nameof(Checkbox), ColorScheme), 2.6F * Dpi, icon_rect.CheckArrow());
                    g.Draw(Colour.BorderColorDisable.Get(nameof(Checkbox), ColorScheme), bor2, path);
                }
            }
        }

        bool nullText = false;
        public override Size GetSize(Canvas g, Font font, TableGaps gap)
        {
            nullText = string.IsNullOrWhiteSpace(Text);
            if (nullText)
            {
                var font_size = g.MeasureString(Config.NullText, Font ?? font);
                return new Size(font_size.Height + gap.x, font_size.Height + gap.x);
            }
            else
            {
                var font_size = g.MeasureText(Text, Font ?? font);
                return new Size(font_size.Width + font_size.Height + gap.x, font_size.Height + gap.x);
            }
        }

        Rectangle RectText, RectIcon;
        public override void SetRect(Canvas g, Font font, Rectangle rect, Size size, int maxwidth, TableGaps gap)
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