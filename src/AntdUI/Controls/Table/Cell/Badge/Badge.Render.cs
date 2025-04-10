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
// GITEE: https://gitee.com/AntdUI/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

using System.Drawing;

namespace AntdUI
{
    partial class CellBadge
    {
        public override void PaintBack(Canvas g) { }

        public override void Paint(Canvas g, Font font, bool enable, SolidBrush fore)
        {
            Color color;
            if (Fill.HasValue) color = Fill.Value;
            else
            {
                switch (State)
                {
                    case TState.Success:
                        color = Colour.Success.Get("Badge"); break;
                    case TState.Error:
                        color = Colour.Error.Get("Badge"); break;
                    case TState.Primary:
                    case TState.Processing:
                        color = Colour.Primary.Get("Badge"); break;
                    case TState.Warn:
                        color = Colour.Warning.Get("Badge"); break;
                    default:
                        color = Colour.TextQuaternary.Get("Badge"); break;
                }
            }
            using (var brush = new SolidBrush(color))
            {
                if (State == TState.Processing && PARENT.PARENT != null)
                {
                    float max = TxtHeight * PARENT.PARENT.AnimationStateValue, alpha = 255 * (1F - PARENT.PARENT.AnimationStateValue);
                    g.DrawEllipse(Helper.ToColor(alpha, brush.Color), 4F * Config.Dpi, new RectangleF(RectDot.X + (RectDot.Width - max) / 2F, RectDot.Y + (RectDot.Height - max) / 2F, max, max));
                }
                g.FillEllipse(brush, RectDot);
            }
            if (Fore.HasValue) g.String(Text, font, Fore.Value, Rect, Table.StringFormat(PARENT.COLUMN));
            else g.String(Text, font, fore, Rect, Table.StringFormat(PARENT.COLUMN));
        }

        public override Size GetSize(Canvas g, Font font, int gap, int gap2)
        {
            if (string.IsNullOrEmpty(Text))
            {
                var size = g.MeasureString(Config.NullText, font);
                return new Size(size.Height, size.Height);
            }
            else
            {
                var size = g.MeasureString(Text, font);
                return new Size(size.Width + size.Height, size.Height);
            }
        }

        int TxtHeight = 0;
        Rectangle RectDot;
        public override void SetRect(Canvas g, Font font, Rectangle rect, Size size, int maxwidth, int gap, int gap2)
        {
            TxtHeight = size.Height;
            int dot_size = (int)(size.Height * dotratio);
            if (string.IsNullOrEmpty(Text)) RectDot = new Rectangle(rect.X + (rect.Width - dot_size) / 2, rect.Y + (rect.Height - dot_size) / 2, dot_size, dot_size);
            else
            {
                Rect = new Rectangle(rect.X + size.Height, rect.Y, rect.Width - size.Height, rect.Height);
                RectDot = new Rectangle(rect.X + (size.Height - dot_size) / 2, rect.Y + (rect.Height - dot_size) / 2, dot_size, dot_size);
            }
        }
    }
}