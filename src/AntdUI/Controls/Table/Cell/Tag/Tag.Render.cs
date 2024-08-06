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
    partial class CellTag
    {
        internal override void PaintBack(Graphics g) { }

        internal override void Paint(Graphics g, Font font, SolidBrush fore)
        {
            using (var path = Rect.RoundPath(6))
            {
                #region 绘制背景

                Color _fore, _back, _bor;
                switch (Type)
                {
                    case TTypeMini.Default:
                        _back = Style.Db.TagDefaultBg;
                        _fore = Style.Db.TagDefaultColor;
                        _bor = Style.Db.DefaultBorder;
                        break;
                    case TTypeMini.Error:
                        _back = Style.Db.ErrorBg;
                        _fore = Style.Db.Error;
                        _bor = Style.Db.ErrorBorder;
                        break;
                    case TTypeMini.Success:
                        _back = Style.Db.SuccessBg;
                        _fore = Style.Db.Success;
                        _bor = Style.Db.SuccessBorder;
                        break;
                    case TTypeMini.Info:
                        _back = Style.Db.InfoBg;
                        _fore = Style.Db.Info;
                        _bor = Style.Db.InfoBorder;
                        break;
                    case TTypeMini.Warn:
                        _back = Style.Db.WarningBg;
                        _fore = Style.Db.Warning;
                        _bor = Style.Db.WarningBorder;
                        break;
                    case TTypeMini.Primary:
                    default:
                        _back = Style.Db.PrimaryBg;
                        _fore = Style.Db.Primary;
                        _bor = Style.Db.Primary;
                        break;
                }

                if (Fore.HasValue) _fore = Fore.Value;
                if (Back.HasValue) _back = Back.Value;

                using (var brush = new SolidBrush(_back))
                {
                    g.FillPath(brush, path);
                }

                if (BorderWidth > 0)
                {
                    float border = BorderWidth * Config.Dpi;
                    using (var brush = new Pen(_bor, border))
                    {
                        g.DrawPath(brush, path);
                    }
                }

                #endregion

                using (var brush = new SolidBrush(_fore))
                {
                    g.DrawString(Text, font, brush, Rect, Table.stringCenter);
                }
            }
        }

        internal override Size GetSize(Graphics g, Font font, int gap, int gap2)
        {
            var size = g.MeasureString(Text, font);
            return new Size((int)Math.Ceiling(size.Width) + gap2 * 2, (int)Math.Ceiling(size.Height) + gap);
        }

        Rectangle Rect;
        internal override void SetRect(Graphics g, Font font, Rectangle rect, Size size, int gap, int gap2)
        {
            Rect = new Rectangle(rect.X + gap, rect.Y + (rect.Height - size.Height) / 2, rect.Width - gap2, size.Height);
        }
    }
}