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
    partial class CellText
    {
        internal override void PaintBack(Canvas g)
        {
            if (PARENT == null) return;
            if (Back.HasValue)
            {
                using (var brush = new SolidBrush(Back.Value))
                {
                    g.Fill(brush, PARENT.RECT);
                }
            }
        }

        internal override void Paint(Canvas g, Font font, SolidBrush fore)
        {
            if (PARENT == null) return;
            var state = g.Save();
            g.SetClip(Rect);
            if (Fore.HasValue)
            {
                using (var brush = new SolidBrush(Fore.Value))
                {
                    g.String(Text, Font ?? font, brush, Rect, Table.StringF(PARENT.COLUMN));
                }
            }
            else g.String(Text, Font ?? font, fore, Rect, Table.StringF(PARENT.COLUMN));
            g.Restore(state);
            if (PrefixSvg != null) g.GetImgExtend(PrefixSvg, RectL, Fore ?? fore.Color);
            else if (Prefix != null) g.Image(Prefix, RectL);

            if (SuffixSvg != null) g.GetImgExtend(SuffixSvg, RectR, Fore ?? fore.Color);
            else if (Suffix != null) g.Image(Suffix, RectR);
        }

        internal override Size GetSize(Canvas g, Font font, int gap, int gap2)
        {
            var size = g.MeasureString(Text, Font ?? font);
            bool has_prefix = HasPrefix, has_suffix = HasSuffix;
            if (has_prefix && has_suffix)
            {
                int icon_size = (int)(size.Height * IconRatio);
                return new Size((icon_size * 2) + gap2 + size.Width + gap2, size.Height);
            }
            else if (has_prefix || has_suffix)
            {
                int icon_size = (int)(size.Height * IconRatio);
                return new Size(icon_size + gap + size.Width + gap2, size.Height);
            }
            return new Size(size.Width + gap2, size.Height);
        }

        Rectangle Rect, RectL, RectR;
        internal override void SetRect(Canvas g, Font font, Rectangle rect, Size size, int gap, int gap2)
        {
            bool has_prefix = HasPrefix, has_suffix = HasSuffix;
            if (has_prefix && has_suffix)
            {
                int icon_size = (int)(size.Height * IconRatio);
                RectL = new Rectangle(rect.X + gap, rect.Y + (rect.Height - icon_size) / 2, icon_size, icon_size);
                RectR = new Rectangle(rect.Right - gap - icon_size, RectL.Y, icon_size, icon_size);

                Rect = new Rectangle(RectL.Right + gap, rect.Y + (rect.Height - size.Height) / 2, rect.Width - gap2 - (icon_size * 2 + gap2), size.Height);
            }
            else if (has_prefix)
            {
                int icon_size = (int)(size.Height * IconRatio);
                RectL = new Rectangle(rect.X + gap, rect.Y + (rect.Height - icon_size) / 2, icon_size, icon_size);
                Rect = new Rectangle(RectL.Right + gap, rect.Y + (rect.Height - size.Height) / 2, rect.Width - gap2 - icon_size - gap, size.Height);
            }
            else if (has_suffix)
            {
                int icon_size = (int)(size.Height * IconRatio);
                RectR = new Rectangle(rect.Right - gap - icon_size, rect.Y + (rect.Height - icon_size) / 2, icon_size, icon_size);
                Rect = new Rectangle(rect.X + gap, rect.Y + (rect.Height - size.Height) / 2, rect.Width - gap2 - icon_size - gap, size.Height);
            }
            else Rect = new Rectangle(rect.X + gap, rect.Y + (rect.Height - size.Height) / 2, rect.Width - gap2, size.Height);
        }
    }
}