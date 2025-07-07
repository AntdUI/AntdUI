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

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AntdUI
{
    public abstract class ILayeredShadowForm : ILayeredFormOpacityDown
    {
        public int shadow = 0, shadow2 = 0;
        bool ShadowEnabled = Config.ShadowEnabled;
        public ILayeredShadowForm()
        {
            if (ShadowEnabled)
            {
                shadow = (int)(10 * Config.Dpi);
                shadow2 = shadow * 2;
            }
        }

        #region 坐标

        public TAlign CLocation(IControl control, TAlignFrom Placement, bool DropDownArrow, int ArrowSize, ref bool Inverted, bool Collision = false)
        {
            var calculateCoordinate = new CalculateCoordinate(control, TargetRect, DropDownArrow ? ArrowSize : 0, shadow, shadow2);
            calculateCoordinate.Auto(Placement, ref Inverted, Collision, out var align, out int x, out int y);
            SetLocationX(x);
            base.SetLocationY(y);
            return align;
        }
        public TAlign CLocation(IControl control, TAlignFrom Placement, Rectangle rect_real, bool DropDownArrow, int ArrowSize, ref bool Inverted, bool Collision = false)
        {
            var calculateCoordinate = new CalculateCoordinate(control, TargetRect, DropDownArrow ? ArrowSize : 0, shadow, shadow2, rect_real);
            calculateCoordinate.Auto(Placement, ref Inverted, Collision, out var align, out int x, out int y);
            SetLocationX(x);
            base.SetLocationY(y);
            return align;
        }
        public TAlign CLocation(ILayeredShadowForm control, Rectangle rect, bool DropDownArrow, int ArrowSize, ref bool Inverted)
        {
            var trect = control.TargetRect;
            var screen = Screen.FromPoint(trect.Location).WorkingArea;
            int x = trect.X + trect.Width - rect.X - control.shadow + (DropDownArrow ? ArrowSize : 0), y = trect.Y + rect.Y + control.shadow;
            if (screen.Right < x + TargetRect.Width) x = x - ((x + TargetRect.Width) - screen.Right) + control.shadow;
            if (screen.Bottom < y + TargetRect.Height) y = y - ((y + TargetRect.Height) - screen.Bottom) + control.shadow;
            SetLocation(x, y);
            return TAlign.LT;
        }

        #endregion

        #region 布局

        public override void SetRect(Rectangle rect)
        {
            if (ShadowEnabled)
            {
                base.SetLocation(rect.X - shadow, rect.Y - shadow);
                base.SetSize(rect.Width + shadow2, rect.Height + shadow2);
            }
            else base.SetRect(rect);
        }
        public override void SetSize(Size size)
        {
            if (ShadowEnabled) base.SetSize(size.Width + shadow2, size.Height + shadow2);
            else base.SetSize(size);
        }
        public override void SetSize(int w, int h)
        {
            if (ShadowEnabled) base.SetSize(w + shadow2, h + shadow2);
            else base.SetSize(w, h);
        }
        public override void SetSize(int size)
        {
            if (ShadowEnabled) base.SetSize(size + shadow2);
            else base.SetSize(size);
        }
        public override void SetSizeW(int w)
        {
            if (ShadowEnabled) base.SetSizeW(w + shadow2);
            else base.SetSizeW(w);
        }
        public override void SetSizeH(int h)
        {
            if (ShadowEnabled) base.SetSizeH(h + shadow2);
            else base.SetSizeH(h);
        }

        public override void SetLocation(Point point)
        {
            if (ShadowEnabled) base.SetLocation(point.X - shadow, point.Y - shadow);
            else base.SetLocation(point);
        }
        public override void SetLocationX(int x)
        {
            if (ShadowEnabled) base.SetLocationX(x - shadow);
            else base.SetLocationX(x);
        }
        public override void SetLocationY(int y)
        {
            if (ShadowEnabled) base.SetLocationY(y - shadow);
            else base.SetLocationY(y);
        }
        public override void SetLocation(int x, int y)
        {
            if (ShadowEnabled) base.SetLocation(x - shadow, y - shadow);
            else base.SetLocation(x, y);
        }

        #endregion

        #region 鼠标

        protected virtual void OnMouseDown(MouseButtons button, int clicks, int x, int y, int delta) { }
        protected virtual void OnMouseMove(MouseButtons button, int clicks, int x, int y, int delta) { }
        protected virtual void OnMouseUp(MouseButtons button, int clicks, int x, int y, int delta) { }
        protected virtual void OnMouseWheel(MouseButtons button, int clicks, int x, int y, int delta) { }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (RunAnimation) return;
            base.OnMouseDown(e);
            OnMouseDown(e.Button, e.Clicks, e.X - shadow, e.Y - shadow, e.Delta);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (RunAnimation) return;
            base.OnMouseUp(e);
            OnMouseUp(e.Button, e.Clicks, e.X - shadow, e.Y - shadow, e.Delta);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (RunAnimation) return;
            base.OnMouseMove(e);
            OnMouseMove(e.Button, e.Clicks, e.X - shadow, e.Y - shadow, e.Delta);
        }
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (RunAnimation) return;
            base.OnMouseWheel(e);
            OnMouseWheel(e.Button, e.Clicks, e.X - shadow, e.Y - shadow, e.Delta);
        }

        #endregion

        /// <summary>
        /// 圆角
        /// </summary>
        [Description("圆角"), Category("外观"), DefaultValue(0)]
        public int Radius { get; set; }

        public abstract void PrintContent(Canvas g, Rectangle rect);
        public abstract void PrintBg(Canvas g, Rectangle rect, GraphicsPath path);
        public void PrintAndClear()
        {
            shadow_temp?.Dispose();
            shadow_temp = null;
            Print();
        }

        public override Bitmap PrintBit()
        {
            var rect = TargetRectXY;
            Bitmap original_bmp = new Bitmap(rect.Width, rect.Height);
            using (var g = Graphics.FromImage(original_bmp).High())
            {
                if (ShadowEnabled)
                {
                    var rect_read = new Rectangle(shadow, shadow, rect.Width - shadow2, rect.Height - shadow2);
                    using (var path = rect_read.RoundPath(Radius))
                    {
                        if (shadow_temp == null)
                        {
                            shadow_temp?.Dispose();
                            shadow_temp = path.PaintShadow(rect.Width, rect.Height, shadow);
                        }

                        g.Image(shadow_temp.Bitmap, rect, .2F);
                        PrintBg(g, rect_read, path);
                    }
                    using (var bmp = new Bitmap(rect_read.Width, rect_read.Height))
                    {
                        using (var g2 = Graphics.FromImage(bmp).High())
                        {
                            PrintContent(g2, new Rectangle(0, 0, rect_read.Width, rect_read.Height));
                        }
                        g.Image(bmp, rect_read);
                    }
                }
                else PrintContent(g, rect);
            }
            return original_bmp;
        }

        SafeBitmap? shadow_temp;
    }

    public class CalculateCoordinate
    {
        public CalculateCoordinate(IControl control, Rectangle drop, int ArrowSize, int Shadow, int Shadow2, Rectangle? rect_real = null)
        {
            var point = control.PointToScreen(Point.Empty);
            var size = control.ClientSize;
            sx = point.X;
            sy = point.Y;
            cw = size.Width;
            ch = size.Height;
            crect = control.ReadRectangle;
            padd = GetPadding(control);
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
            return sx + (cw - dw) / 2;
        }

        /// <summary>
        /// 左X ←
        /// </summary>
        public int LeftX()
        {
            if (creal.HasValue) return sx + creal.Value.X + crect.X;
            return sx + crect.X;
        }

        /// <summary>
        /// 右X →
        /// </summary>
        public int RightX()
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

        #endregion

        public void Auto(TAlignFrom Placement, ref bool Inverted, bool Collision, out TAlign align, out int x, out int y)
        {
            switch (Placement)
            {
                case TAlignFrom.Top:
                    Top(ref Inverted, Collision, out align, out x, out y);
                    break;
                case TAlignFrom.TL:
                    TL(ref Inverted, Collision, out align, out x, out y);
                    break;
                case TAlignFrom.TR:
                    TR(ref Inverted, Collision, out align, out x, out y);
                    break;
                case TAlignFrom.Bottom:
                    Bottom(ref Inverted, Collision, out align, out x, out y);
                    break;
                case TAlignFrom.BR:
                    BR(ref Inverted, Collision, out align, out x, out y);
                    break;
                case TAlignFrom.BL:
                default:
                    BL(ref Inverted, Collision, out align, out x, out y);
                    break;
            }
        }

        public void Top(ref bool Inverted, bool Collision, out TAlign align, out int x, out int y)
        {
            align = TAlign.Bottom;
            x = CenterX();
            y = TopY();
            Inverted = true;
            if (Collision)
            {
                var screen = Screen.FromPoint(new Point(x, y)).WorkingArea;
                if (y < screen.Top)
                {
                    Inverted = false;
                    y = BottomY();
                    align = TAlign.Top;
                }
            }
        }

        public void TL(ref bool Inverted, bool Collision, out TAlign align, out int x, out int y)
        {
            align = TAlign.BL;
            x = LeftX();
            y = TopY();
            Inverted = true;
            if (Collision)
            {
                var screen = Screen.FromPoint(new Point(x, y)).WorkingArea;
                if (x + dw > screen.Right)
                {
                    x = RightX();
                    align = TAlign.BR;
                }
            }
        }

        public void TR(ref bool Inverted, bool Collision, out TAlign align, out int x, out int y)
        {
            align = TAlign.BR;
            x = RightX();
            y = TopY();
            Inverted = true;
            if (Collision)
            {
                var screen = Screen.FromPoint(new Point(x, y)).WorkingArea;
                if (x < screen.Left)
                {
                    x = LeftX();
                    align = TAlign.BL;
                }
            }
        }

        public void Bottom(ref bool Inverted, bool Collision, out TAlign align, out int x, out int y)
        {
            align = TAlign.Top;
            x = CenterX();
            y = BottomY();
            if (Collision)
            {
                var screen = Screen.FromPoint(new Point(x, y)).WorkingArea;
                if (y + dh > screen.Bottom)
                {
                    Inverted = true;
                    y = TopY();
                    align = TAlign.Bottom;
                }
            }
        }

        public void BR(ref bool Inverted, bool Collision, out TAlign align, out int x, out int y)
        {
            align = TAlign.TR;
            x = RightX();
            y = BottomY();
            if (Collision)
            {
                var screen = Screen.FromPoint(new Point(x, y)).WorkingArea;
                if (x < screen.Left)
                {
                    x = LeftX();
                    align = TAlign.TL;
                }
            }
        }

        public void BL(ref bool Inverted, bool Collision, out TAlign align, out int x, out int y)
        {
            align = TAlign.TL;
            x = LeftX();
            y = BottomY();
            if (Collision)
            {
                var screen = Screen.FromPoint(new Point(x, y)).WorkingArea;
                if (x + dw > screen.Right)
                {
                    x = RightX();
                    align = TAlign.TR;
                }
            }
        }
    }
}