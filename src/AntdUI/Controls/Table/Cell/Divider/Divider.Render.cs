// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.Drawing;

namespace AntdUI
{
    partial class CellDivider
    {
        public override void PaintBack(Canvas g) { }

        public override void Paint(Canvas g, Font font, bool enable, SolidBrush fore)
        {
            g.Fill(Colour.Split.Get(nameof(Divider), PARENT.PARENT.ColorScheme), Rect);
        }

        public override Size GetSize(Canvas g, Font font, TableGaps gap)
        {
            var size = g.MeasureString(Config.NullText, font);
            return new Size(0, size.Height - gap.y);
        }

        public override void SetRect(Canvas g, Font font, Rectangle rect, Size size, int maxwidth, TableGaps gap)
        {
            Rect = new Rectangle(rect.X + (rect.Width - 1) / 2, rect.Y + (rect.Height - size.Height) / 2, 1, size.Height);
        }
    }
}