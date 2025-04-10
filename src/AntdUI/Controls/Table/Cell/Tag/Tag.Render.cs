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
                        _back = Colour.TagDefaultBg.Get("Tag");
                        _fore = Colour.TagDefaultColor.Get("Tag");
                        _bor = Colour.DefaultBorder.Get("Tag");
                        break;
                    case TTypeMini.Error:
                        _back = Colour.ErrorBg.Get("Tag");
                        _fore = Colour.Error.Get("Tag");
                        _bor = Colour.ErrorBorder.Get("Tag");
                        break;
                    case TTypeMini.Success:
                        _back = Colour.SuccessBg.Get("Tag");
                        _fore = Colour.Success.Get("Tag");
                        _bor = Colour.SuccessBorder.Get("Tag");
                        break;
                    case TTypeMini.Info:
                        _back = Colour.InfoBg.Get("Tag");
                        _fore = Colour.Info.Get("Tag");
                        _bor = Colour.InfoBorder.Get("Tag");
                        break;
                    case TTypeMini.Warn:
                        _back = Colour.WarningBg.Get("Tag");
                        _fore = Colour.Warning.Get("Tag");
                        _bor = Colour.WarningBorder.Get("Tag");
                        break;
                    case TTypeMini.Primary:
                    default:
                        _back = Colour.PrimaryBg.Get("Tag");
                        _fore = Colour.Primary.Get("Tag");
                        _bor = Colour.Primary.Get("Tag");
                        break;
                }

                if (Fore.HasValue) _fore = Fore.Value;
                if (Back.HasValue) _back = Back.Value;

                g.Fill(_back, path);

                if (borderWidth > 0) g.Draw(_bor, borderWidth * Config.Dpi, path);

                #endregion

                g.String(Text, font, _fore, Rect, Table.StringFormat(ColumnAlign.Center));
            }
        }

        public override Size GetSize(Canvas g, Font font, int gap, int gap2)
        {
            var size = g.MeasureString(Text, font);
            return new Size(size.Width + gap2, size.Height + gap);
        }

        public override void SetRect(Canvas g, Font font, Rectangle rect, Size size, int maxwidth, int gap, int gap2)
        {
            Rect = new Rectangle(rect.X, rect.Y + (rect.Height - size.Height) / 2, rect.Width, size.Height);
        }
    }
}