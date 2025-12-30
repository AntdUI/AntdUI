// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.Drawing;

namespace AntdUI
{
    partial class CellTag
    {
        public override void PaintBack(Canvas g) { }

        public override void Paint(Canvas g, Font font, bool enable, SolidBrush fore)
        {
            using (var path = Rect.RoundPath(6))
            {
                #region 绘制背景

                Color _fore, _back, _bor;
                switch (Type)
                {
                    case TTypeMini.Default:
                        _back = Colour.TagDefaultBg.Get(nameof(Tag), PARENT.PARENT.ColorScheme);
                        _fore = Colour.TagDefaultColor.Get(nameof(Tag), PARENT.PARENT.ColorScheme);
                        _bor = Colour.DefaultBorder.Get(nameof(Tag), PARENT.PARENT.ColorScheme);
                        break;
                    case TTypeMini.Error:
                        _back = Colour.ErrorBg.Get(nameof(Tag), PARENT.PARENT.ColorScheme);
                        _fore = Colour.Error.Get(nameof(Tag), PARENT.PARENT.ColorScheme);
                        _bor = Colour.ErrorBorder.Get(nameof(Tag), PARENT.PARENT.ColorScheme);
                        break;
                    case TTypeMini.Success:
                        _back = Colour.SuccessBg.Get(nameof(Tag), PARENT.PARENT.ColorScheme);
                        _fore = Colour.Success.Get(nameof(Tag), PARENT.PARENT.ColorScheme);
                        _bor = Colour.SuccessBorder.Get(nameof(Tag), PARENT.PARENT.ColorScheme);
                        break;
                    case TTypeMini.Info:
                        _back = Colour.InfoBg.Get(nameof(Tag), PARENT.PARENT.ColorScheme);
                        _fore = Colour.Info.Get(nameof(Tag), PARENT.PARENT.ColorScheme);
                        _bor = Colour.InfoBorder.Get(nameof(Tag), PARENT.PARENT.ColorScheme);
                        break;
                    case TTypeMini.Warn:
                        _back = Colour.WarningBg.Get(nameof(Tag), PARENT.PARENT.ColorScheme);
                        _fore = Colour.Warning.Get(nameof(Tag), PARENT.PARENT.ColorScheme);
                        _bor = Colour.WarningBorder.Get(nameof(Tag), PARENT.PARENT.ColorScheme);
                        break;
                    case TTypeMini.Primary:
                    default:
                        _back = Colour.PrimaryBg.Get(nameof(Tag), PARENT.PARENT.ColorScheme);
                        _fore = Colour.Primary.Get(nameof(Tag), PARENT.PARENT.ColorScheme);
                        _bor = Colour.Primary.Get(nameof(Tag), PARENT.PARENT.ColorScheme);
                        break;
                }

                if (Fore.HasValue) _fore = Fore.Value;
                if (Back.HasValue) _back = Back.Value;

                g.Fill(_back, path);

                if (borderWidth > 0) g.Draw(_bor, borderWidth * g.Dpi, path);

                #endregion

                g.DrawText(Text, font, _fore, Rect, Table.StringFormat(ColumnAlign.Center));
            }
        }

        public override Size GetSize(Canvas g, Font font, TableGaps gap)
        {
            var size = g.MeasureText(Text, font);
            if (Gap.HasValue)
            {
                int sp = (int)(Gap.Value * g.Dpi);
                return new Size(size.Width + sp * 2, size.Height + sp);
            }
            else if (PARENT.PARENT.GapCell.HasValue)
            {
                int sp = (int)(PARENT.PARENT.GapCell.Value * g.Dpi);
                return new Size(size.Width + sp * 2, size.Height + sp);
            }
            else return new Size(size.Width + gap.x2, size.Height + gap.y);
        }

        public override void SetRect(Canvas g, Font font, Rectangle rect, Size size, int maxwidth, TableGaps gap)
        {
            Rect = new Rectangle(rect.X, rect.Y + (rect.Height - size.Height) / 2, rect.Width, size.Height);
        }
    }
}