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
    partial class CellSwitch
    {
        public override void PaintBack(Canvas g)
        {
        }

        public override void Paint(Canvas g, Font font, bool enable, SolidBrush fore)
        {
            var colorScheme = PARENT.PARENT.ColorScheme;
            bool enabled = Enabled;
            var color = Colour.Primary.Get(nameof(Switch), colorScheme);
            using (var path = Rect.RoundPath(Rect.Height))
            {
                using (var brush = new SolidBrush(Colour.TextQuaternary.Get(nameof(Switch), colorScheme)))
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
                    g.FillEllipse(enable ? Colour.BgBase.Get(nameof(Switch), colorScheme) : Color.FromArgb(200, Colour.BgBase.Get(nameof(Switch), colorScheme)), dot_rect);
                }
                else if (Checked)
                {
                    var colorhover = Colour.PrimaryHover.Get(nameof(Switch), colorScheme);
                    g.Fill(color, path);
                    if (AnimationHover) g.Fill(Helper.ToColorN(AnimationHoverValue, colorhover), path);
                    else if (ExtraMouseHover) g.Fill(colorhover, path);
                    var dot_rect = new RectangleF(Rect.X + gap + Rect.Width - Rect.Height, Rect.Y + gap, Rect.Height - gap2, Rect.Height - gap2);
                    g.FillEllipse(enable ? Colour.BgBase.Get(nameof(Switch), colorScheme) : Color.FromArgb(200, Colour.BgBase.Get(nameof(Switch), colorScheme)), dot_rect);
                    if (Loading)
                    {
                        var dot_rect2 = new RectangleF(dot_rect.X + gap, dot_rect.Y + gap, dot_rect.Height - gap2, dot_rect.Height - gap2);
                        float size = Rect.Height * .1F;
                        using (var brush = new Pen(color, size))
                        {
                            brush.StartCap = brush.EndCap = LineCap.Round;
                            g.DrawArc(brush, dot_rect2, LineAngle, LineWidth * 3.6F);
                        }
                    }
                }
                else
                {
                    var dot_rect = new RectangleF(Rect.X + gap, Rect.Y + gap, Rect.Height - gap2, Rect.Height - gap2);
                    g.FillEllipse(enable ? Colour.BgBase.Get(nameof(Switch), colorScheme) : Color.FromArgb(200, Colour.BgBase.Get(nameof(Switch), colorScheme)), dot_rect);
                    if (Loading)
                    {
                        var dot_rect2 = new RectangleF(dot_rect.X + gap, dot_rect.Y + gap, dot_rect.Height - gap2, dot_rect.Height - gap2);
                        float size = Rect.Height * .1F;
                        using (var brush = new Pen(color, size))
                        {
                            brush.StartCap = brush.EndCap = LineCap.Round;
                            g.DrawArc(brush, dot_rect2, LineAngle, LineWidth * 3.6F);
                        }
                    }
                }

                // 绘制文本
                string? textToRender = Checked ? CheckedText : UnCheckedText;
                if (textToRender != null)
                {
                    Color _fore_ = _fore ?? Colour.PrimaryColor.Get(nameof(Switch), colorScheme);
                    using (var brush = new SolidBrush(_fore_))
                    {
                        var textSize = g.MeasureString(textToRender, font);
                        var textRect = Checked
                            ? new Rectangle(Rect.X + (Rect.Width - Rect.Height + gap2) / 2 - textSize.Width / 2, Rect.Y + Rect.Height / 2 - textSize.Height / 2, textSize.Width, textSize.Height)
                            : new Rectangle(Rect.X + (Rect.Height - gap + (Rect.Width - Rect.Height + gap) / 2 - textSize.Width / 2), Rect.Y + Rect.Height / 2 - textSize.Height / 2, textSize.Width, textSize.Height);
                        g.String(textToRender, font, brush, textRect);
                    }
                }
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
            //nullText = string.IsNullOrWhiteSpace(Text);
            //if (nullText)
            //{
            //}
            //else
            //{
            //    var font_size = g.MeasureText(Text, Font ?? font);
            //    return new Size(font_size.Width + font_size.Height + gap.x, font_size.Height + gap.x);
            //}
        }

        public override void SetRect(Canvas g, Font font, Rectangle rect, Size size, int maxwidth, TableGaps gap)
        {
            Rect = new Rectangle(rect.X + (rect.Width - size.Width) / 2, rect.Y + (rect.Height - size.Height) / 2, size.Width, size.Height);
        }
    }
}