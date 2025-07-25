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
        public CalculateCoordinate(Control control, Rectangle drop, int ArrowSize, int Shadow, int Shadow2, Rectangle? rect_real = null)
        {
            var point = control.PointToScreen(Point.Empty);
            var size = control.ClientSize;
            sx = point.X;
            sy = point.Y;
            cw = size.Width;
            ch = size.Height;
            if (control is IControl icontrol)
            {
                crect = icontrol.ReadRectangle;
                padd = GetPadding(icontrol);
            }
            else crect = control.ClientRectangle;
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
        /// 控件真实容器
        /// </summary>
        public Rectangle crect { get; set; }

        /// <summary>
        /// 控件边距
        /// </summary>
        public int padd { get; set; }

        /// <summary>
        /// 下拉宽度
        /// </summary>
        public int dw { get; set; }

        /// <summary>
        /// 下拉高度
        /// </summary>
        public int dh { get; set; }
        public int arrow { get; set; }
        public int shadow { get; set; }
        public int shadow2 { get; set; }

        /// <summary>
        /// 内容区域
        /// </summary>
        public Rectangle? creal { get; set; }
        public Rectangle? iscreen { get; set; }

        int GetPadding(IControl control)
        {
            if (control is Button button) return (int)((button.WaveSize + button.BorderWidth) * Config.Dpi);
            else if (control is Input input) return (int)((input.WaveSize + input.BorderWidth) * Config.Dpi);
            else if (control is ColorPicker colorPicker) return (int)((colorPicker.WaveSize + colorPicker.BorderWidth) * Config.Dpi);
            else if (control is Switch _switch) return (int)(_switch.WaveSize * Config.Dpi);
            else if (control is Panel panel) return (int)(panel.BorderWidth * Config.Dpi);
            else if (control is Alert alert) return (int)(alert.BorderWidth * Config.Dpi);
            else if (control is Avatar avatar) return (int)(avatar.BorderWidth * Config.Dpi);
            else if (control is Tag tag) return (int)(tag.BorderWidth * Config.Dpi);
            else if (control is Table table) return (int)(table.BorderWidth * Config.Dpi);
            else if (control is ContainerPanel containerPanel) return (int)(containerPanel.BorderWidth * Config.Dpi);
            return 0;
        }

        #region 方向

        /// <summary>
        /// 居中X
        /// </summary>
        public int CenterX()
        {
            if (creal.HasValue) return sx + creal.Value.X + (creal.Value.Width - dw) / 2 + shadow;
            return (sx + (cw - dw) / 2) + shadow;
        }

        /// <summary>
        /// 居中Y
        /// </summary>
        public int CenterY()
        {
            if (creal.HasValue) return sy + creal.Value.Y + (creal.Value.Height - dh) / 2 + shadow;
            return (sy + (ch - dh) / 2) + shadow;
        }

        /// <summary>
        /// 左X ← （齐头）
        /// </summary>
        public int L()
        {
            if (creal.HasValue) return sx + creal.Value.X + crect.X;
            return sx + crect.X;
        }

        /// <summary>
        /// 右X → （齐头）
        /// </summary>
        public int R()
        {
            if (creal.HasValue) return sx + creal.Value.X + creal.Value.Width - dw + shadow2;
            return sx + (crect.X + crect.Width) - dw + shadow2;
        }

        /// <summary>
        /// 上Y ↑
        /// </summary>
        public int TopY()
        {
            if (creal.HasValue) return sy - dh + creal.Value.Y + crect.Y + padd - arrow;
            return sy - dh + crect.Y + padd - arrow;
        }

        /// <summary>
        /// 下Y ↓
        /// </summary>
        public int BottomY()
        {
            if (creal.HasValue) return sy + creal.Value.Bottom - padd + arrow;
            return sy + crect.Bottom - padd + arrow;
        }

        /// <summary>
        /// 左X ←
        /// </summary>
        public int Left()
        {
            if (creal.HasValue) return sx + creal.Value.X - dw;
            return sx - dw;
        }

        /// <summary>
        /// 右X → 
        /// </summary>
        public int Right()
        {
            if (creal.HasValue) return sx + creal.Value.Right + shadow + arrow;
            return sx + cw;
        }

        /// <summary>
        /// 上Y ↑ （齐头）
        /// </summary>
        public int Y()
        {
            if (creal.HasValue) return sy + creal.Value.Y + padd - arrow;
            return sy;
        }

        /// <summary>
        /// 下Y ↓ （齐头）
        /// </summary>
        public int B()
        {
            if (creal.HasValue) return sy + creal.Value.Bottom - dh;
            return sy + ch - dh;
        }

        #endregion

        public void Auto(TAlignFrom placement, PushAnimateConfig animate, bool collision, out TAlign align, out int x, out int y)
        {
            bool inverted = false;
            switch (placement)
            {
                case TAlignFrom.Top:
                    Top(collision, out align, out x, out y, ref inverted);
                    break;
                case TAlignFrom.TL:
                    TL(collision, out align, out x, out y, ref inverted);
                    break;
                case TAlignFrom.TR:
                    TR(collision, out align, out x, out y, ref inverted);
                    break;
                case TAlignFrom.Bottom:
                    Bottom(collision, out align, out x, out y, ref inverted);
                    break;
                case TAlignFrom.BR:
                    BR(collision, out align, out x, out y, ref inverted);
                    break;
                case TAlignFrom.BL:
                default:
                    BL(collision, out align, out x, out y, ref inverted);
                    break;
            }
            animate.Inverted = inverted;
        }

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

        #region 核心

        #region TAlignFrom

        public void Top(bool collision, out TAlign align, out int x, out int y, ref bool inverted)
        {
            align = TAlign.Bottom;
            x = CenterX();
            y = TopY();
            inverted = true;
            if (collision)
            {
                var screen = iscreen ?? Screen.FromPoint(new Point(x, y)).WorkingArea;
                if (y < screen.Y)
                {
                    inverted = false;
                    y = BottomY();
                    align = TAlign.Top;
                }
            }
        }

        public void TL(bool collision, out TAlign align, out int x, out int y, ref bool inverted)
        {
            align = TAlign.BL;
            x = L();
            y = TopY();
            inverted = true;
            if (collision)
            {
                var screen = iscreen ?? Screen.FromPoint(new Point(x, y)).WorkingArea;
                if (x + dw > screen.Right)
                {
                    x = R();
                    align = TAlign.BR;
                }
            }
        }

        public void TR(bool collision, out TAlign align, out int x, out int y, ref bool inverted)
        {
            align = TAlign.BR;
            x = R();
            y = TopY();
            inverted = true;
            if (collision)
            {
                var screen = iscreen ?? Screen.FromPoint(new Point(x, y)).WorkingArea;
                if (x < screen.X)
                {
                    x = L();
                    align = TAlign.BL;
                }
            }
        }

        public void Bottom(bool collision, out TAlign align, out int x, out int y, ref bool inverted)
        {
            align = TAlign.Top;
            x = CenterX();
            y = BottomY();
            if (collision)
            {
                var screen = iscreen ?? Screen.FromPoint(new Point(x, y)).WorkingArea;
                if (y + dh > screen.Bottom)
                {
                    inverted = true;
                    y = TopY();
                    align = TAlign.Bottom;
                }
            }
        }

        public void BR(bool collision, out TAlign align, out int x, out int y, ref bool inverted)
        {
            align = TAlign.TR;
            x = R();
            y = BottomY();
            if (collision)
            {
                var screen = iscreen ?? Screen.FromPoint(new Point(x, y)).WorkingArea;
                if (x < screen.X)
                {
                    x = L();
                    align = TAlign.TL;
                }
            }
        }

        public void BL(bool collision, out TAlign align, out int x, out int y, ref bool inverted)
        {
            align = TAlign.TL;
            x = L();
            y = BottomY();
            if (collision)
            {
                var screen = iscreen ?? Screen.FromPoint(new Point(x, y)).WorkingArea;
                if (x + dw > screen.Right)
                {
                    x = R();
                    align = TAlign.TR;
                }
            }
        }

        #endregion

        #region TAlign

        public void Top(bool collision, int gap, out TAlign align, out int x, out int y, out int arrowX)
        {
            x = CenterX();
            y = TopY();
            align = TAlign.Top;
            var screen = iscreen ?? Screen.FromPoint(new Point(sx, sy)).WorkingArea;
            if (collision && y < screen.Y)
            {
                y = BottomY();
                align = TAlign.Bottom;
            }
            int cx = dw / 2, tsize = arrow + gap;
            arrowX = cx;
            if (x < screen.X)
            {
                arrowX = cx - (screen.X - x);
                if (arrowX < tsize) arrowX = tsize;
                x = screen.X;
            }
            else if (x > screen.Right - dw)
            {
                int rx = screen.Right - dw;
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
            var screen = iscreen ?? Screen.FromPoint(new Point(sx, sy)).WorkingArea;
            if (collision && y + dh > screen.Bottom)
            {
                y = TopY();
                align = TAlign.Top;
            }
            int cx = dw / 2, tsize = arrow + gap;
            arrowX = cx;
            if (x < screen.X)
            {
                arrowX = cx - (screen.X - x);
                if (arrowX < tsize) arrowX = tsize;
                x = screen.X;
            }
            else if (x > screen.Right - dw)
            {
                int rx = screen.Right - dw;
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
            var screen = iscreen ?? Screen.FromPoint(new Point(sx, sy)).WorkingArea;
            if (collision && x + dw > screen.Right)
            {
                x = R();
                align = TAlign.TR;
            }
            arrowX = -1;
            if (x < screen.X) x = screen.X;
            else if (x > screen.Right - dw) x = screen.Right - dw;
        }
        public void TR(bool collision, int gap, out TAlign align, out int x, out int y, out int arrowX)
        {
            x = R();
            y = TopY();
            align = TAlign.TR;
            var screen = iscreen ?? Screen.FromPoint(new Point(sx, sy)).WorkingArea;
            if (collision && x < screen.X)
            {
                x = L();
                align = TAlign.TL;
            }
            arrowX = -1;
            if (x < screen.X) x = screen.X;
            else if (x > screen.Right - dw) x = screen.Right - dw;
        }
        public void LT(bool collision, int gap, out TAlign align, out int x, out int y, out int arrowX)
        {
            x = Left();
            y = Y();
            align = TAlign.LT;
            var screen = iscreen ?? Screen.FromPoint(new Point(sx, sy)).WorkingArea;
            if (collision && x < screen.X)
            {
                x = Right();
                align = TAlign.RT;
            }
            arrowX = -1;
            if (x < screen.X) x = screen.X;
            else if (x > screen.Right - dw) x = screen.Right - dw;
        }
        public void Left(bool collision, int gap, out TAlign align, out int x, out int y, out int arrowX)
        {
            x = Left();
            y = CenterY();
            align = TAlign.Left;
            var screen = iscreen ?? Screen.FromPoint(new Point(sx, sy)).WorkingArea;
            if (collision && x < screen.X)
            {
                x = Right();
                align = TAlign.Right;
            }
            arrowX = -1;
            if (x < screen.X) x = screen.X;
            else if (x > screen.Right - dw) x = screen.Right - dw;
        }
        public void LB(bool collision, int gap, out TAlign align, out int x, out int y, out int arrowX)
        {
            x = Left();
            y = B();
            align = TAlign.LB;
            var screen = iscreen ?? Screen.FromPoint(new Point(sx, sy)).WorkingArea;
            if (collision && x < screen.X)
            {
                x = Right();
                align = TAlign.RB;
            }
            arrowX = -1;
            if (x < screen.X) x = screen.X;
            else if (x > screen.Right - dw) x = screen.Right - dw;
        }
        public void RT(bool collision, int gap, out TAlign align, out int x, out int y, out int arrowX)
        {
            x = Right();
            y = Y();
            align = TAlign.RT;
            var screen = iscreen ?? Screen.FromPoint(new Point(sx, sy)).WorkingArea;
            if (collision && x + dw > screen.Right)
            {
                x = Left();
                align = TAlign.LT;
            }
            arrowX = -1;
            if (x < screen.X) x = screen.X;
            else if (x > screen.Right - dw) x = screen.Right - dw;
        }
        public void Right(bool collision, int gap, out TAlign align, out int x, out int y, out int arrowX)
        {
            x = Right();
            y = CenterY();
            align = TAlign.Right;
            var screen = iscreen ?? Screen.FromPoint(new Point(sx, sy)).WorkingArea;
            if (collision && x + dw > screen.Right)
            {
                x = Left();
                align = TAlign.Left;
            }
            arrowX = -1;
            if (x < screen.X) x = screen.X;
            else if (x > screen.Right - dw) x = screen.Right - dw;
        }
        public void RB(bool collision, int gap, out TAlign align, out int x, out int y, out int arrowX)
        {
            x = Right();
            y = B();
            align = TAlign.RB;
            var screen = iscreen ?? Screen.FromPoint(new Point(sx, sy)).WorkingArea;
            if (collision && x + dw > screen.Right)
            {
                x = Left();
                align = TAlign.LB;
            }
            arrowX = -1;
            if (x < screen.X) x = screen.X;
            else if (x > screen.Right - dw) x = screen.Right - dw;
        }
        public void BL(bool collision, int gap, out TAlign align, out int x, out int y, out int arrowX)
        {
            align = TAlign.BL;
            x = L();
            y = BottomY();
            var screen = iscreen ?? Screen.FromPoint(new Point(sx, sy)).WorkingArea;
            if (collision && x + dw > screen.Right)
            {
                x = R();
                align = TAlign.BR;
            }
            arrowX = -1;
            if (x < screen.X) x = screen.X;
            else if (x > screen.Right - dw) x = screen.Right - dw;
        }
        public void BR(bool collision, int gap, out TAlign align, out int x, out int y, out int arrowX)
        {
            align = TAlign.BR;
            x = R();
            y = BottomY();
            var screen = iscreen ?? Screen.FromPoint(new Point(sx, sy)).WorkingArea;
            if (collision && x < screen.X)
            {
                x = L();
                align = TAlign.BL;
            }
            arrowX = -1;
            if (x < screen.X) x = screen.X;
            else if (x > screen.Right - dw) x = screen.Right - dw;
        }

        #endregion

        #endregion
    }
}