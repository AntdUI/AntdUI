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

namespace AntdUI
{
    partial class CellText
    {
        internal override void PaintBack(Graphics g)
        {
            if (PARENT == null) return;
            if (Back.HasValue)
            {
                using (var brush = new SolidBrush(Back.Value))
                {
                    g.FillRectangle(brush, PARENT.RECT);
                }
            }
        }

        internal override void Paint(Graphics g, Font font, SolidBrush fore)
        {
            if (PARENT == null) return;
            if (Fore.HasValue)
            {
                using (var brush = new SolidBrush(Fore.Value))
                {
                    g.DrawString(Text, Font ?? font, brush, Rect, Table.StringF(PARENT.column));
                }
            }
            else g.DrawString(Text, Font ?? font, fore, Rect, Table.StringF(PARENT.column));

            if (PrefixSvg != null)
            {
                using (var _bmp = SvgExtend.GetImgExtend(PrefixSvg, RectL, Fore ?? fore.Color))
                {
                    if (_bmp != null) g.DrawImage(_bmp, RectL);
                }
            }
            else if (Prefix != null) g.DrawImage(Prefix, RectL);

            if (SuffixSvg != null)
            {
                using (var _bmp = SvgExtend.GetImgExtend(SuffixSvg, RectR, Fore ?? fore.Color))
                {
                    if (_bmp != null) g.DrawImage(_bmp, RectR);
                }
            }
            else if (Suffix != null) g.DrawImage(Suffix, RectR);
        }

        internal override Size GetSize(Graphics g, Font font, int gap, int gap2)
        {
            var size = g.MeasureString(Text, Font ?? font);
            bool has_prefix = HasPrefix, has_suffix = HasSuffix;
            if (has_prefix && has_suffix)
            {
                int icon_size = (int)(size.Height * IconRatio);
                return new Size((icon_size * 2) + gap2 + (int)Math.Ceiling(size.Width) + gap2, (int)Math.Ceiling(size.Height));
            }
            else if (has_prefix || has_suffix)
            {
                int icon_size = (int)(size.Height * IconRatio);
                return new Size(icon_size + gap + (int)Math.Ceiling(size.Width) + gap2, (int)Math.Ceiling(size.Height));
            }
            return new Size((int)Math.Ceiling(size.Width) + gap2, (int)Math.Ceiling(size.Height));
        }

        Rectangle Rect, RectL, RectR;
        internal override void SetRect(Graphics g, Font font, Rectangle rect, Size size, int gap, int gap2)
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