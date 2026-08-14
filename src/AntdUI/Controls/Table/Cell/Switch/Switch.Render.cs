// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.Drawing;
using System.Drawing.Drawing2D;

namespace AntdUI
{
    partial class CellSwitch
    {
        public override void PaintBack(Canvas g)
        {
        }

        public override void Paint(Canvas g, Font font, bool enable, SolidBrush fore)
        {
            var colorScheme = PARENT.PARENT.ColorScheme;
            bool enabled = Enabled;
            var name = PARENT.PARENT.Name;
            var color = Colour.Primary.Get(colorScheme, nameof(Switch), name);
            using (var path = Rect.RoundPath(Rect.Height))
            {
                using (var brush = new SolidBrush(Colour.TextQuaternary.Get(colorScheme, nameof(Switch), name)))
                {
                    g.Fill(brush, path);
                    if (AnimationHover) g.Fill(Helper.ToColorN(AnimationHoverValue, brush.Color), path);
                    else if (ExtraMouseHover) g.Fill(brush, path);
                }
                int gap = (int)(2 * g.Dpi), gap2 = gap * 2;
                if (AnimationCheck)
                {
                    var alpha = 255 * AnimationCheckValue;
                    g.Fill(Helper.ToColor(alpha, color), path);
                    var dot_rect = new RectangleF(Rect.X + gap + (Rect.Width - Rect.Height) * AnimationCheckValue, Rect.Y + gap, Rect.Height - gap2, Rect.Height - gap2);
                    g.FillEllipse(enabled ? Colour.SwitchHandleBg.Get(colorScheme, nameof(Switch), name) : Helper.ToColorN(0.65f, Colour.SwitchHandleBg.Get(colorScheme, nameof(Switch), name)), dot_rect);
                    if (loading) PaintLoading(g, Rect, dot_rect, gap, gap2, color);
                    PaintText(g, font, colorScheme, name, null, Rect, dot_rect);
                }
                else if (Checked)
                {
                    if (enable) PaintChecked(g, font, colorScheme, name, Rect, path, gap, gap2, Colour.SwitchHandleBg.Get(colorScheme, nameof(Switch), name), color);
                    else PaintChecked(g, font, colorScheme, name, Rect, path, gap, gap2, Helper.ToColorN(0.8f, Colour.SwitchHandleBg.Get(colorScheme, nameof(Switch), name)), Helper.ToColorN(0.65f, color));
                }
                else
                {
                    if (enable) PaintUnChecked(g, font, colorScheme, name, Rect, gap, gap2, Colour.SwitchHandleBg.Get(colorScheme, nameof(Switch), name), color);
                    else PaintUnChecked(g, font, colorScheme, name, Rect, gap, gap2, Helper.ToColorN(0.8f, Colour.SwitchHandleBg.Get(colorScheme, nameof(Switch), name)), Helper.ToColorN(0.65f, color));
                }
            }
        }

        void PaintChecked(Canvas g, Font font, TAMode colorScheme, string name, Rectangle rect, GraphicsPath path, int gap, int gap2, Color handBg, Color color)
        {
            var colorhover = FillHover ?? Colour.PrimaryHover.Get(colorScheme, nameof(Switch), name);
            g.Fill(color, path);
            if (AnimationHover) g.Fill(Helper.ToColorN(AnimationHoverValue, colorhover), path);
            else if (ExtraMouseHover) g.Fill(colorhover, path);
            var dot_rect = new RectangleF(rect.X + gap + rect.Width - rect.Height, rect.Y + gap, rect.Height - gap2, rect.Height - gap2);
            g.FillEllipse(handBg, dot_rect);
            if (loading) PaintLoading(g, rect, dot_rect, gap, gap2, color);
            PaintText(g, font, colorScheme, name, true, rect, dot_rect);
        }
        void PaintUnChecked(Canvas g, Font font, TAMode colorScheme, string name, Rectangle rect, int gap, int gap2, Color handBg, Color color)
        {
            var dot_rect = new RectangleF(rect.X + gap, rect.Y + gap, rect.Height - gap2, rect.Height - gap2);
            g.FillEllipse(handBg, dot_rect);
            if (loading) PaintLoading(g, rect, dot_rect, gap, gap2, color);
            PaintText(g, font, colorScheme, name, false, rect, dot_rect);
        }
        void PaintLoading(Canvas g, Rectangle rect, RectangleF dot_rect, int gap, int gap2, Color color)
        {
            var loading_rect = new RectangleF(dot_rect.X + gap, dot_rect.Y + gap, dot_rect.Height - gap2, dot_rect.Height - gap2);
            float size = rect.Height * .1F;
            using (var brush = new Pen(color, size))
            {
                brush.StartCap = brush.EndCap = LineCap.Round;
                g.DrawArc(brush, loading_rect, LineAngle, LineWidth * 3.6F);
            }
        }

        /// <summary>
        /// 绘制文本
        /// </summary>
        void PaintText(Canvas g, Font font, TAMode colorScheme, string name, bool? check, Rectangle rect, RectangleF dot_rect)
        {
            if (check.HasValue)
            {
                if (check.Value)
                {
                    var text = CheckedText;
                    if (text == null) return;
                    int padd = (int)(dot_rect.Height * 1.1F);
                    g.DrawText(text, font, _fore ?? Colour.PrimaryColor.Get(colorScheme, nameof(Switch), name), new Rectangle(rect.X, rect.Y, rect.Width - padd, rect.Height));
                }
                else
                {
                    var text = UnCheckedText;
                    if (text == null) return;
                    int padd = (int)(dot_rect.Height * 1.1F);
                    g.DrawText(text, font, _fore ?? Colour.PrimaryColor.Get(colorScheme, nameof(Switch), name), new Rectangle(rect.X + padd, rect.Y, rect.Width - padd, rect.Height));
                }
            }
            else
            {
                g.SetClip(rect);
                string? text = CheckedText, untext = UnCheckedText;
                if (text == null && untext == null) return;
                int padd = (int)(dot_rect.Height * 1.1F), prog = (int)(rect.Width * AnimationCheckValue);
                using (var brush = new SolidBrush(_fore ?? Colour.PrimaryColor.Get(colorScheme, nameof(Switch), name)))
                {
                    int tmp = rect.Width - padd;
                    g.DrawText(untext, font, brush, new Rectangle((rect.X + padd) + prog, rect.Y, tmp, rect.Height));
                    g.DrawText(text, font, brush, new Rectangle(rect.X + prog - rect.Width, rect.Y, tmp, rect.Height));
                }
                g.ResetClip();
            }
        }

        public override Size GetSize(Canvas g, Font font, TableGaps gap)
        {
            string? checkedText = CheckedText, uncheckedText = UnCheckedText;
            if (checkedText == null || uncheckedText == null)
            {
                var font_size = g.MeasureString(Config.NullText, Font ?? font);
                return new Size(font_size.Height * 2, font_size.Height);
            }
            else
            {
                var font_size = g.MeasureString(checkedText.Length > uncheckedText.Length ? checkedText : uncheckedText, Font ?? font);
                return new Size(font_size.Width + (int)(font_size.Height * 1.2F), font_size.Height);
            }
        }

        public override void SetRect(Canvas g, Font font, Rectangle rect, Size size, int maxwidth, TableGaps gap)
        {
            Rect = new Rectangle(rect.X + (rect.Width - size.Width) / 2, rect.Y + (rect.Height - size.Height) / 2, size.Width, size.Height);
        }
    }
}