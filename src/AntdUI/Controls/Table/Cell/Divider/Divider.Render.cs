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
    partial class CellDivider
    {
        internal override void PaintBack(Canvas g) { }

        internal override void Paint(Canvas g, Font font, SolidBrush fore)
        {
            using (var brush = new SolidBrush(Style.Db.Split))
            {
                g.Fill(brush, Rect);
            }
        }

        internal override Size GetSize(Canvas g, Font font, int gap, int gap2)
        {
            var size = g.MeasureString(Config.NullText, font);
            return new Size(gap, size.Height + gap);
        }

        Rectangle Rect;
        internal override void SetRect(Canvas g, Font font, Rectangle rect, Size size, int gap, int gap2)
        {
            int h = size.Height - gap2;
            Rect = new Rectangle(rect.X + (rect.Width - 1) / 2, rect.Y + (rect.Height - h) / 2, 1, h);
        }
    }
}