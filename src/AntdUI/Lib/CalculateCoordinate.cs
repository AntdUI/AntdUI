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
using System.Windows.Forms;

namespace AntdUI
{
    public class CalculateCoordinate
    {
        public CalculateCoordinate(Rectangle rect, Rectangle drop, int ArrowSize, int Shadow, int Shadow2, Rectangle? rect_real = null)
        {
            sx = rect.X;
            sy = rect.Y;
            cw = rect.Width;
            ch = rect.Height;
            dw = drop.Width;
            dh = drop.Height;
            arrow = ArrowSize;
            shadow = Shadow;
            shadow2 = Shadow2;
            creal = rect_real;
        }
        public CalculateCoordinate(Control control, Rectangle drop, int ArrowSize, int Shadow, int Shadow2, Rectangle? rect_real = null)
        {
            var point = control.PointToScreen(Point.Empty);
            var size = control.ClientSize;
            sx = point.X;
            sy = point.Y;
            cw = size.Width;
            ch = size.Height;
            if (control is IControl icontrol) padd = GetPadding(icontrol);
            dw = drop.Width;
            dh = drop.Height;
            arrow = ArrowSize;
            shadow = Shadow;
            shadow2 = Shadow2;
            creal = rect_real;
        }

        /// <summary>
        /// 屏幕坐标X（控件）
        /// </summary>
        public int sx { get; set; }

        /// <summary>
        /// 屏幕坐标Y（控件）
        /// </summary>
        public int sy { get; set; }

        /// <summary>
        /// 控件宽度
        /// </summary>
        public int cw { get; set; }

        /// <summary>
        /// 控件高度
        /// </summary>
        public int ch { get; set; }

        /// <summary>
        /// 控件边距（阴影+边框）
        /// </summary>
        public int padd { get; set; }

        /// <summary>
        /// 下拉/气泡 宽度（包含阴影）
        /// </summary>
        public int dw { get; set; }

        /// <summary>
        /// 下拉/气泡 高度（包含阴影）
        /// </summary>
        public int dh { get; set; }

        /// <summary>
        /// 下拉/气泡 箭头大小
        /// </summary>
        public int arrow { get; set; }

        /// <summary>
        /// 下拉/气泡 阴影大小（单边）
        /// </summary>
        public int shadow { get; set; }

        /// <summary>
        /// Popover 阴影大小*2（两边一起）
        /// </summary>
        public int shadow2 { get; set; }

        /// <summary>
        /// 内容区域（控件内部某个区域，如纯绘制控件列表中的某一项）
        /// </summary>
        public Rectangle? creal { get; set; }

        /// <summary>
        /// 自定义屏幕区域（用于屏幕碰撞）
        /// </summary>
        public Rectangle? screen { get; set; }

        int GetPadding(IControl control)
        {
            if (control is Button button) return (int)((button.WaveSize + button.BorderWidth / 2F) * Config.Dpi);
            else if (control is Input input) return (int)((input.WaveSize + input.BorderWidth / 2F) * Config.Dpi);
            else if (control is ColorPicker colorPicker) return (int)((colorPicker.WaveSize + colorPicker.BorderWidth / 2F) * Config.Dpi);
            else if (control is Switch _switch) return (int)(_switch.WaveSize * Config.Dpi);
            else if (control is Panel panel) return (int)(panel.BorderWidth * Config.Dpi);
            else if (control is Alert alert) return (int)(alert.BorderWidth * Config.Dpi);
            else if (control is Avatar avatar) return (int)(avatar.BorderWidth * Config.Dpi);
            else if (control is Tag tag) return (int)(tag.BorderWidth * Config.Dpi);
            else if (control is Table table) return (int)(table.BorderWidth * Config.Dpi);
            else if (control is Tabs tab) return (int)(tab.Gap * Config.Dpi);
            else if (control is ContainerPanel containerPanel) return (int)(containerPanel.BorderWidth * Config.Dpi);
            return 0;
        }

        #region 设置

        public CalculateCoordinate SetScreen(Rectangle? value)
        {
            screen = value;
            return this;
        }

        #endregion

        #region 核心

        /// <summary>
        /// 居中X
        /// </summary>
        public int CenterX()
        {
            if (creal.HasValue) return sx + creal.Value.X + (creal.Value.Width - dw) / 2;
            return sx + (cw - dw) / 2;
        }

        /// <summary>
        /// 居中Y
        /// </summary>
        public int CenterY()
        {
            if (creal.HasValue) return sy + creal.Value.Y + (creal.Value.Height - dh) / 2;
            return sy + (ch - dh) / 2;
        }

        /// <summary>
        /// 左X ← （齐头）
        /// </summary>
        public int L()
        {
            if (creal.HasValue) return sx - shadow + padd + creal.Value.X;
            return sx - shadow + padd;
        }

        /// <summary>
        /// 右X → （齐头）
        /// </summary>
        public int R()
        {
            if (creal.HasValue) return sx + creal.Value.X + creal.Value.Width - dw - padd + shadow;
            return sx + cw - dw - padd + shadow;
        }

        /// <summary>
        /// 上Y ↑ （齐头）
        /// </summary>
        public int Y()
        {
            if (creal.HasValue) return sy - shadow + padd + creal.Value.Y;
            return sy - shadow + padd;
        }

        /// <summary>
        /// 下Y ↓ （齐头）
        /// </summary>
        public int B()
        {
            if (creal.HasValue) return sy + creal.Value.Y + creal.Value.Height - dh + padd + shadow;
            return sy + ch - dh + padd + shadow;
        }

        /// <summary>
        /// 上Y ↑
        /// </summary>
        public int TopY()
        {
            if (creal.HasValue) return sy - dh + padd + shadow - arrow + creal.Value.Y;
            return sy - dh + padd + shadow - arrow;
        }

        /// <summary>
        /// 下Y ↓
        /// </summary>
        public int BottomY()
        {
            if (creal.HasValue) return sy + creal.Value.Bottom + arrow - shadow - padd;
            return sy + ch + arrow - shadow - padd;
        }

        /// <summary>
        /// 左X ←
        /// </summary>
        public int Left()
        {
            if (creal.HasValue) return sx - dw + padd + shadow - arrow + creal.Value.X;
            return sx - dw + padd + shadow - arrow;
        }

        /// <summary>
        /// 右X → 
        /// </summary>
        public int Right()
        {
            if (creal.HasValue) return sx + creal.Value.Right + arrow - shadow - padd;
            return sx + cw + arrow - shadow - padd;
        }

        #endregion

        #region 计算坐标

        public void Auto(TAlign align, bool collision, out int x, out int y, out Point[]? rect_arrow)
        {
            bool inverted = false;
            switch (align)
            {
                case TAlign.Top:
                    Top(collision, out x, out y, ref inverted, out rect_arrow);
                    break;
                case TAlign.Bottom:
                    Bottom(collision, out x, out y, ref inverted, out rect_arrow);
                    break;

                case TAlign.Left:
                    Left(collision, out x, out y, ref inverted, out rect_arrow);
                    break;
                case TAlign.Right:
                    Right(collision, out x, out y, ref inverted, out rect_arrow);
                    break;

                case TAlign.TL:
                    TL(collision, out x, out y, ref inverted, out rect_arrow);
                    break;
                case TAlign.BL:
                    BL(collision, out x, out y, ref inverted, out rect_arrow);
                    break;

                case TAlign.TR:
                    TR(collision, out x, out y, ref inverted, out rect_arrow);
                    break;
                case TAlign.BR:
                    BR(collision, out x, out y, ref inverted, out rect_arrow);
                    break;

                case TAlign.LT:
                    LT(collision, out x, out y, ref inverted, out rect_arrow);
                    break;
                case TAlign.RT:
                    RT(collision, out x, out y, ref inverted, out rect_arrow);
                    break;

                case TAlign.LB:
                    LB(collision, out x, out y, ref inverted, out rect_arrow);
                    break;
                case TAlign.RB:
                    RB(collision, out x, out y, ref inverted, out rect_arrow);
                    break;
                default:
                    Top(collision, out x, out y, ref inverted, out rect_arrow);
                    break;
            }
        }
        public void Auto(TAlignFrom placement, PushAnimateConfig animate, bool collision, out int x, out int y, out Point[]? rect_arrow)
        {
            bool inverted = false;
            switch (placement)
            {
                case TAlignFrom.Top:
                    Top(collision, out x, out y, ref inverted, out rect_arrow);
                    break;
                case TAlignFrom.Bottom:
                    Bottom(collision, out x, out y, ref inverted, out rect_arrow);
                    break;

                case TAlignFrom.TL:
                    TL(collision, out x, out y, ref inverted, out rect_arrow);
                    break;
                case TAlignFrom.BL:
                    BL(collision, out x, out y, ref inverted, out rect_arrow);
                    break;

                case TAlignFrom.TR:
                    TR(collision, out x, out y, ref inverted, out rect_arrow);
                    break;
                case TAlignFrom.BR:
                    BR(collision, out x, out y, ref inverted, out rect_arrow);
                    break;

                default:
                    Top(collision, out x, out y, ref inverted, out rect_arrow);
                    break;
            }
            animate.Inverted = inverted;
        }

        void Top(bool collision, out int x, out int y, ref bool inverted, out Point[]? rect_arrow)
        {
            x = CenterX();
            y = TopY();
            if (collision && ScreenArea.IsBottom(x, y))
            {
                y = BottomY();
                rect_arrow = ArrowBottom(x, y);
            }
            else
            {
                inverted = true;
                rect_arrow = ArrowTop(x, y);
            }
        }
        void Bottom(bool collision, out int x, out int y, ref bool inverted, out Point[]? rect_arrow)
        {
            x = CenterX();
            y = BottomY();
            if (collision && ScreenArea.IsTop(x, y))
            {
                y = TopY();
                inverted = true;
                rect_arrow = ArrowTop(x, y);
            }
            else rect_arrow = ArrowBottom(x, y);
        }

        void Left(bool collision, out int x, out int y, ref bool inverted, out Point[]? rect_arrow)
        {
            x = Left();
            y = CenterY();
            if (collision && ScreenArea.IsRight(x, y))
            {
                x = Right();
                rect_arrow = ArrowRight(x, y);
            }
            else rect_arrow = ArrowLeft(x, y);
        }
        void Right(bool collision, out int x, out int y, ref bool inverted, out Point[]? rect_arrow)
        {
            x = Right();
            y = CenterY();
            if (collision && ScreenArea.IsLeft(x, y))
            {
                x = Left();
                rect_arrow = ArrowLeft(x, y);
            }
            else rect_arrow = ArrowRight(x, y);
        }

        void TL(bool collision, out int x, out int y, ref bool inverted, out Point[]? rect_arrow)
        {
            x = L();
            y = TopY();
            if (collision && ScreenArea.IsBottom(x, y))
            {
                y = BottomY();
                rect_arrow = ArrowBL(x, y);
            }
            else
            {
                inverted = true;
                rect_arrow = ArrowTL(x, y);
            }
        }
        void BL(bool collision, out int x, out int y, ref bool inverted, out Point[]? rect_arrow)
        {
            x = L();
            y = BottomY();
            if (collision && ScreenArea.IsTop(x, y))
            {
                y = TopY();
                inverted = true;
                rect_arrow = ArrowTL(x, y);
            }
            else rect_arrow = ArrowBL(x, y);
        }

        void TR(bool collision, out int x, out int y, ref bool inverted, out Point[]? rect_arrow)
        {
            x = R();
            y = TopY();
            if (collision && ScreenArea.IsBottom(x, y))
            {
                y = BottomY();
                rect_arrow = ArrowBR(x, y);
            }
            else
            {
                inverted = true;
                rect_arrow = ArrowTR(x, y);
            }
        }
        void BR(bool collision, out int x, out int y, ref bool inverted, out Point[]? rect_arrow)
        {
            x = R();
            y = BottomY();
            if (collision && ScreenArea.IsTop(x, y))
            {
                y = TopY();
                inverted = true;
                rect_arrow = ArrowTR(x, y);
            }
            else rect_arrow = ArrowBR(x, y);
        }

        void LT(bool collision, out int x, out int y, ref bool inverted, out Point[]? rect_arrow)
        {
            x = Left();
            y = Y();
            if (collision && ScreenArea.IsRight(x, y))
            {
                x = Right();
                rect_arrow = ArrowRT(x, y);
            }
            else rect_arrow = ArrowLT(x, y);
        }
        void RT(bool collision, out int x, out int y, ref bool inverted, out Point[]? rect_arrow)
        {
            x = Right();
            y = Y();
            if (collision && ScreenArea.IsLeft(x, y))
            {
                x = Left();
                rect_arrow = ArrowLT(x, y);
            }
            else rect_arrow = ArrowRT(x, y);
        }

        void LB(bool collision, out int x, out int y, ref bool inverted, out Point[]? rect_arrow)
        {
            x = Left();
            y = B();
            if (collision && ScreenArea.IsRight(x, y))
            {
                x = Right();
                rect_arrow = ArrowRB(x, y);
            }
            else rect_arrow = ArrowLB(x, y);
        }
        void RB(bool collision, out int x, out int y, ref bool inverted, out Point[]? rect_arrow)
        {
            x = Right();
            y = B();
            if (collision && ScreenArea.IsLeft(x, y))
            {
                x = Left();
                rect_arrow = ArrowLB(x, y);
            }
            else rect_arrow = ArrowRB(x, y);
        }

        #region 箭头

        Point[]? ArrowTop(int x, int y)
        {
            if (arrow > 0)
            {
                int x_arrow = dw / 2, y_arrow = dh - shadow;
                return new Point[] {
                    new Point(x_arrow, y_arrow +  arrow),
                    new Point(x_arrow - arrow, y_arrow),
                    new Point(x_arrow + arrow, y_arrow)
                };
            }
            return null;
        }
        Point[]? ArrowBottom(int x, int y)
        {
            if (arrow > 0)
            {
                int x_arrow = dw / 2, y_arrow = shadow - arrow;
                return new Point[] {
                    new Point(x_arrow, y_arrow),
                    new Point(x_arrow - arrow, shadow),
                    new Point(x_arrow + arrow, shadow)
                };
            }
            return null;
        }
        Point[]? ArrowLeft(int x, int y)
        {
            if (arrow > 0)
            {
                int x_arrow = dw - shadow, y_arrow = dh / 2;
                return new Point[] {
                    new Point(x_arrow + arrow, y_arrow),
                    new Point(x_arrow, y_arrow - arrow),
                    new Point(x_arrow, y_arrow + arrow)
                };
            }
            return null;
        }
        Point[]? ArrowRight(int x, int y)
        {
            if (arrow > 0)
            {
                int x_arrow = shadow - arrow, y_arrow = dh / 2;
                return new Point[] {
                    new Point(x_arrow, y_arrow),
                    new Point(shadow, y_arrow - shadow),
                    new Point(shadow, y_arrow + shadow)
                };
            }
            return null;
        }
        Point[]? ArrowTL(int x, int y)
        {
            if (arrow > 0)
            {
                int x_arrow = shadow + arrow + 12, y_arrow = dh - shadow;
                return new Point[] {
                    new Point(x_arrow, y_arrow +  arrow),
                    new Point(x_arrow - arrow, y_arrow),
                    new Point(x_arrow + arrow, y_arrow)
                };
            }
            return null;
        }
        Point[]? ArrowBL(int x, int y)
        {
            if (arrow > 0)
            {
                int x_arrow = shadow + arrow + 12, y_arrow = shadow - arrow;
                return new Point[] {
                    new Point(x_arrow, y_arrow),
                    new Point(x_arrow - arrow, shadow),
                    new Point(x_arrow + arrow, shadow)
                };
            }
            return null;
        }
        Point[]? ArrowTR(int x, int y)
        {
            if (arrow > 0)
            {
                int x_arrow = dw - (shadow + arrow + 12), y_arrow = dh - shadow;
                return new Point[] {
                    new Point(x_arrow, y_arrow +  arrow),
                    new Point(x_arrow - arrow, y_arrow),
                    new Point(x_arrow + arrow, y_arrow)
                };
            }
            return null;
        }
        Point[]? ArrowBR(int x, int y)
        {
            if (arrow > 0)
            {
                int x_arrow = dw - (shadow + arrow + 12), y_arrow = shadow - arrow;
                return new Point[] {
                    new Point(x_arrow, y_arrow),
                    new Point(x_arrow - arrow, shadow),
                    new Point(x_arrow + arrow, shadow)
                };
            }
            return null;
        }
        Point[]? ArrowLT(int x, int y)
        {
            if (arrow > 0)
            {
                int x_arrow = dw - shadow, y_arrow = shadow + arrow + 12;
                return new Point[] {
                    new Point(x_arrow + arrow, y_arrow),
                    new Point(x_arrow, y_arrow - arrow),
                    new Point(x_arrow, y_arrow + arrow)
                };
            }
            return null;
        }
        Point[]? ArrowRT(int x, int y)
        {
            if (arrow > 0)
            {
                int x_arrow = shadow - arrow, y_arrow = shadow + arrow + 12;
                return new Point[] {
                    new Point(x_arrow, y_arrow),
                    new Point(shadow, y_arrow - shadow),
                    new Point(shadow, y_arrow + shadow)
                };
            }
            return null;
        }
        Point[]? ArrowLB(int x, int y)
        {
            if (arrow > 0)
            {
                int x_arrow = dw - shadow, y_arrow = dh - (shadow + arrow + 12);
                return new Point[] {
                    new Point(x_arrow + arrow, y_arrow),
                    new Point(x_arrow, y_arrow - arrow),
                    new Point(x_arrow, y_arrow + arrow)
                };
            }
            return null;
        }
        Point[]? ArrowRB(int x, int y)
        {
            if (arrow > 0)
            {
                int x_arrow = shadow - arrow, y_arrow = dh - (shadow + arrow + 12);
                return new Point[] {
                    new Point(x_arrow, y_arrow),
                    new Point(shadow, y_arrow - shadow),
                    new Point(shadow, y_arrow + shadow)
                };
            }
            return null;
        }

        #endregion

        #endregion

        #region TAlign

        public void Auto(ref TAlign align, int gap, out int x, out int y, out int ox)
        {
            switch (align)
            {
                case TAlign.Top:
                    Top(true, gap, out align, out x, out y, out ox);
                    break;
                case TAlign.TL:
                    TL(true, gap, out align, out x, out y, out ox);
                    break;
                case TAlign.TR:
                    TR(true, gap, out align, out x, out y, out ox);
                    break;
                case TAlign.LT:
                    LT(true, gap, out align, out x, out y, out ox);
                    break;
                case TAlign.Left:
                    Left(true, gap, out align, out x, out y, out ox);
                    break;
                case TAlign.LB:
                    LB(true, gap, out align, out x, out y, out ox);
                    break;
                case TAlign.RT:
                    RT(true, gap, out align, out x, out y, out ox);
                    break;
                case TAlign.Right:
                    Right(true, gap, out align, out x, out y, out ox);
                    break;
                case TAlign.RB:
                    RB(true, gap, out align, out x, out y, out ox);
                    break;
                case TAlign.Bottom:
                    Bottom(true, gap, out align, out x, out y, out ox);
                    break;
                case TAlign.BL:
                    BL(true, gap, out align, out x, out y, out ox);
                    break;
                case TAlign.BR:
                    BR(true, gap, out align, out x, out y, out ox);
                    break;
                default:
                    Top(true, gap, out align, out x, out y, out ox);
                    break;
            }

        }

        public void Top(bool collision, int gap, out TAlign align, out int x, out int y, out int arrowX)
        {
            x = CenterX();
            y = TopY();
            align = TAlign.Top;
            var screenArea = ScreenArea;
            if (collision && screenArea.IsBottom(x, y))
            {
                y = BottomY();
                align = TAlign.Bottom;
            }
            int cx = dw / 2, tsize = arrow + gap;
            arrowX = cx;
            if (screenArea.IsRight(x, y))
            {
                arrowX = cx - (screenArea.X - x);
                if (arrowX < tsize) arrowX = tsize;
                x = screenArea.X;
            }
            else if (x > screenArea.Right - dw)
            {
                int rx = screenArea.Right - dw;
                arrowX = cx - (rx - x);
                if (arrowX > dw - tsize) arrowX = dw - tsize;
                x = rx;
            }
        }
        public void Bottom(bool collision, int gap, out TAlign align, out int x, out int y, out int arrowX)
        {
            x = CenterX();
            y = BottomY();
            align = TAlign.Bottom;
            var screenArea = ScreenArea;
            if (collision && screenArea.IsTop(x, y))
            {
                y = TopY();
                align = TAlign.Top;
            }
            int cx = dw / 2, tsize = arrow + gap;
            arrowX = cx;
            if (screenArea.IsRight(x, y))
            {
                arrowX = cx - (screenArea.X - x);
                if (arrowX < tsize) arrowX = tsize;
                x = screenArea.X;
            }
            else if (x > screenArea.Right - dw)
            {
                int rx = screenArea.Right - dw;
                arrowX = cx - (rx - x);
                if (arrowX > dw - tsize) arrowX = dw - tsize;
                x = rx;
            }
        }
        public void TL(bool collision, int gap, out TAlign align, out int x, out int y, out int arrowX)
        {
            x = L();
            y = TopY();
            align = TAlign.TL;
            var screenArea = ScreenArea;
            if (collision && screenArea.IsLeft(x, y))
            {
                x = R();
                align = TAlign.TR;
            }
            arrowX = -1;
            if (screenArea.IsRight(x, y)) x = screenArea.X;
            else if (x > screenArea.Right - dw) x = screenArea.Right - dw;
        }
        public void TR(bool collision, int gap, out TAlign align, out int x, out int y, out int arrowX)
        {
            x = R();
            y = TopY();
            align = TAlign.TR;
            var screenArea = ScreenArea;
            if (collision && screenArea.IsRight(x, y))
            {
                x = L();
                align = TAlign.TL;
            }
            arrowX = -1;
            if (screenArea.IsRight(x, y)) x = screenArea.X;
            else if (x > screenArea.Right - dw) x = screenArea.Right - dw;
        }
        public void LT(bool collision, int gap, out TAlign align, out int x, out int y, out int arrowX)
        {
            x = Left();
            y = Y();
            align = TAlign.LT;
            var screenArea = ScreenArea;
            if (collision && screenArea.IsRight(x, y))
            {
                x = Right();
                align = TAlign.RT;
            }
            arrowX = -1;
            if (screenArea.IsRight(x, y)) x = screenArea.X;
            else if (x > screenArea.Right - dw) x = screenArea.Right - dw;
        }
        public void Left(bool collision, int gap, out TAlign align, out int x, out int y, out int arrowX)
        {
            x = Left();
            y = CenterY();
            align = TAlign.Left;
            var screenArea = ScreenArea;
            if (collision && screenArea.IsRight(x, y))
            {
                x = Right();
                align = TAlign.Right;
            }
            arrowX = -1;
            if (screenArea.IsRight(x, y)) x = screenArea.X;
            else if (x > screenArea.Right - dw) x = screenArea.Right - dw;
        }
        public void LB(bool collision, int gap, out TAlign align, out int x, out int y, out int arrowX)
        {
            x = Left();
            y = B();
            align = TAlign.LB;
            var screenArea = ScreenArea;
            if (collision && screenArea.IsRight(x, y))
            {
                x = Right();
                align = TAlign.RB;
            }
            arrowX = -1;
            if (screenArea.IsRight(x, y)) x = screenArea.X;
            else if (x > screenArea.Right - dw) x = screenArea.Right - dw;
        }
        public void RT(bool collision, int gap, out TAlign align, out int x, out int y, out int arrowX)
        {
            x = Right();
            y = Y();
            align = TAlign.RT;
            var screenArea = ScreenArea;
            if (collision && screenArea.IsLeft(x, y))
            {
                x = Left();
                align = TAlign.LT;
            }
            arrowX = -1;
            if (screenArea.IsRight(x, y)) x = screenArea.X;
            else if (x > screenArea.Right - dw) x = screenArea.Right - dw;
        }
        public void Right(bool collision, int gap, out TAlign align, out int x, out int y, out int arrowX)
        {
            x = Right();
            y = CenterY();
            align = TAlign.Right;
            var screenArea = ScreenArea;
            if (collision && screenArea.IsLeft(x, y))
            {
                x = Left();
                align = TAlign.Left;
            }
            arrowX = -1;
            if (screenArea.IsRight(x, y)) x = screenArea.X;
            else if (x > screenArea.Right - dw) x = screenArea.Right - dw;
        }
        public void RB(bool collision, int gap, out TAlign align, out int x, out int y, out int arrowX)
        {
            x = Right();
            y = B();
            align = TAlign.RB;
            var screenArea = ScreenArea;
            if (collision && screenArea.IsLeft(x, y))
            {
                x = Left();
                align = TAlign.LB;
            }
            arrowX = -1;
            if (screenArea.IsRight(x, y)) x = screenArea.X;
            else if (x > screenArea.Right - dw) x = screenArea.Right - dw;
        }
        public void BL(bool collision, int gap, out TAlign align, out int x, out int y, out int arrowX)
        {
            align = TAlign.BL;
            x = L();
            y = BottomY();
            var screenArea = ScreenArea;
            if (collision && screenArea.IsLeft(x, y))
            {
                x = R();
                align = TAlign.BR;
            }
            arrowX = -1;
            if (screenArea.IsRight(x, y)) x = screenArea.X;
            else if (x > screenArea.Right - dw) x = screenArea.Right - dw;
        }
        public void BR(bool collision, int gap, out TAlign align, out int x, out int y, out int arrowX)
        {
            align = TAlign.BR;
            x = R();
            y = BottomY();
            var screenArea = ScreenArea;
            if (collision && screenArea.IsRight(x, y))
            {
                x = L();
                align = TAlign.BL;
            }
            arrowX = -1;
            if (screenArea.IsRight(x, y)) x = screenArea.X;
            else if (x > screenArea.Right - dw) x = screenArea.Right - dw;
        }

        #endregion

        public ScreenCollision ScreenArea => new ScreenCollision(this);

        public class ScreenCollision
        {
            CalculateCoordinate coordinate;
            public ScreenCollision(CalculateCoordinate c)
            {
                coordinate = c;
                Rect = c.screen ?? Screen.FromPoint(new Point(c.sx, c.sy)).WorkingArea;
            }
            public Rectangle Rect { get; set; }

            public int X => Rect.X;
            public int Y => Rect.Y;
            public int Right => Rect.Right;
            public int Bottom => Rect.Bottom;

            /// <summary>
            /// 是否碰撞底部
            /// </summary>
            public bool IsBottom(int x, int y) => y < Rect.Y;

            /// <summary>
            /// 是否碰撞右侧
            /// </summary>
            public bool IsRight(int x, int y) => x < Rect.X;

            /// <summary>
            /// 是否碰撞左侧
            /// </summary>
            public bool IsLeft(int x, int y) => x + coordinate.dw > Rect.Right;

            /// <summary>
            /// 是否碰撞顶部
            /// </summary>
            public bool IsTop(int x, int y) => y + coordinate.dh > Rect.Bottom;
        }
    }
}