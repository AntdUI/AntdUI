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

namespace AntdUI
{
    partial class CellBadge
    {
        internal override void PaintBack(Canvas g) { }

        internal override void Paint(Canvas g, Font font, SolidBrush fore)
        {
            if (PARENT == null) return;
            Color color;
            if (Fill.HasValue) color = Fill.Value;
            else
            {
                switch (State)
                {
                    case TState.Success:
                        color = Style.Db.Success; break;
                    case TState.Error:
                        color = Style.Db.Error; break;
                    case TState.Primary:
                    case TState.Processing:
                        color = Style.Db.Primary; break;
                    case TState.Warn:
                        color = Style.Db.Warning; break;
                    default:
                        color = Style.Db.TextQuaternary; break;
                }
            }
            using (var brush = new SolidBrush(color))
            {
                if (State == TState.Processing && PARENT.PARENT != null)
                {
                    float max = (TxtHeight - 6F) * PARENT.PARENT.AnimationStateValue, alpha = 255 * (1F - PARENT.PARENT.AnimationStateValue);
                    g.DrawEllipse(Helper.ToColor(alpha, brush.Color), 4F * Config.Dpi, new RectangleF(RectDot.X + (RectDot.Width - max) / 2F, RectDot.Y + (RectDot.Height - max) / 2F, max, max));
                }
                g.FillEllipse(brush, RectDot);
            }
            if (Fore.HasValue)
            {
                using (var brush = new SolidBrush(Fore.Value))
                {
                    g.String(Text, font, brush, Rect, Table.StringF(PARENT.COLUMN));
                }
            }
            else g.String(Text, font, fore, Rect, Table.StringF(PARENT.COLUMN));
        }

        internal override Size GetSize(Canvas g, Font font, int gap, int gap2)
        {
            if (string.IsNullOrEmpty(Text))
            {
                var size = g.MeasureString(Config.NullText, font);
                int height = size.Height;
                return new Size(height + gap2, size.Height);
            }
            else
            {
                var size = g.MeasureString(Text, font);
                int height = size.Height;
                return new Size(size.Width + height + gap2, height);
            }
        }

        int TxtHeight = 0;
        Rectangle Rect;
        Rectangle RectDot;
        internal override void SetRect(Canvas g, Font font, Rectangle rect, Size size, int gap, int gap2)
        {
            TxtHeight = size.Height;
            int dot_size = (int)(size.Height / 2.5F);
            if (string.IsNullOrEmpty(Text)) RectDot = new Rectangle(rect.X + (rect.Width - dot_size) / 2, rect.Y + (rect.Height - dot_size) / 2, dot_size, dot_size);
            else
            {
                Rect = new Rectangle(rect.X + gap + size.Height, rect.Y, rect.Width - size.Height - gap2, rect.Height);
                if (PARENT == null) return;
                switch (PARENT.COLUMN.Align)
                {
                    case ColumnAlign.Center:
                        var sizec = g.MeasureString(Text, font);
                        RectDot = new Rectangle(rect.X + (rect.Width - sizec.Width - sizec.Height + gap2) / 2, rect.Y + (rect.Height - dot_size) / 2, dot_size, dot_size);
                        break;
                    case ColumnAlign.Right:
                        var sizer = g.MeasureString(Text, font);
                        RectDot = new Rectangle(Rect.Right - sizer.Width - gap2, rect.Y + (rect.Height - dot_size) / 2, dot_size, dot_size);
                        break;
                    case ColumnAlign.Left:
                    default:
                        RectDot = new Rectangle(rect.X + gap + (size.Height - dot_size) / 2, rect.Y + (rect.Height - dot_size) / 2, dot_size, dot_size);
                        break;
                }
            }
        }
    }
}