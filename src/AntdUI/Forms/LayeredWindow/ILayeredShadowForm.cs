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
        int shadow = 0, shadow2 = 0;
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
            var point = control.PointToScreen(Point.Empty);
            var size = control.ClientSize;
            var rect = control.ReadRectangle;
            int padd = 0, width = TargetRect.Width, height = TargetRect.Height, tmpArrowSize = DropDownArrow ? ArrowSize : 0;
            if (control is Button button) padd = (int)((button.WaveSize + button.BorderWidth) * Config.Dpi);
            else if (control is Input input) padd = (int)((input.WaveSize + input.BorderWidth) * Config.Dpi);
            else if (control is ColorPicker colorPicker) padd = (int)((colorPicker.WaveSize + colorPicker.BorderWidth) * Config.Dpi);
            else if (control is Switch _switch) padd = (int)(_switch.WaveSize * Config.Dpi);
            else if (control is Panel panel) padd = (int)(panel.BorderWidth * Config.Dpi);
            else if (control is Alert alert) padd = (int)(alert.BorderWidth * Config.Dpi);
            else if (control is Avatar avatar) padd = (int)(avatar.BorderWidth * Config.Dpi);
            else if (control is ContainerPanel containerPanel) padd = (int)(containerPanel.BorderWidth * Config.Dpi);
            else if (control is Tag tag) padd = (int)(tag.BorderWidth * Config.Dpi);
            switch (Placement)
            {
                case TAlignFrom.Top:
                    Inverted = true;
                    return CLocationTop(point, size, rect, width, height, padd, tmpArrowSize, Collision);
                case TAlignFrom.TL:
                    Inverted = true;
                    return CLocationTL(point, size, rect, width, height, padd, tmpArrowSize, Collision);
                case TAlignFrom.TR:
                    Inverted = true;
                    return CLocationTR(point, size, rect, width, height, padd, tmpArrowSize, Collision);
                case TAlignFrom.Bottom:
                    return CLocationBottom(point, size, rect, width, height, padd, tmpArrowSize, Collision);
                case TAlignFrom.BR:
                    return CLocationBR(point, size, rect, width, height, padd, tmpArrowSize, Collision);
                case TAlignFrom.BL:
                default:
                    return CLocationBL(point, size, rect, width, height, padd, tmpArrowSize, Collision);
            }
        }

        TAlign CLocationTop(Point point, Size size, Rectangle rect, int width, int height, int padd, int ArrowSize, bool Collision = false)
        {
            base.SetLocation(point.X + (size.Width - width) / 2, point.Y - height + rect.Y + padd - ArrowSize);
            return TAlign.Top;
        }
        TAlign CLocationTL(Point point, Size size, Rectangle rect, int width, int height, int padd, int ArrowSize, bool Collision = false)
        {
            int x = point.X + rect.X, y = point.Y - height + rect.Y + padd - ArrowSize;
            SetLocationX(x);
            base.SetLocationY(y);
            if (Collision)
            {
                var screen = Screen.FromPoint(TargetRect.Location).WorkingArea;
                if (x > (screen.X + screen.Width) - TargetRect.Width)
                {
                    x = point.X + (rect.X + rect.Width) - width + shadow2;
                    SetLocationX(x);
                    return TAlign.TR;
                }
            }
            return TAlign.TL;
        }
        TAlign CLocationTR(Point point, Size size, Rectangle rect, int width, int height, int padd, int ArrowSize, bool Collision = false)
        {
            int x = point.X + (rect.X + rect.Width) - width + shadow2, y = point.Y - height + rect.Y + padd - ArrowSize;
            SetLocationX(x);
            base.SetLocationY(y);
            if (Collision)
            {
                var screen = Screen.FromPoint(TargetRect.Location).WorkingArea;
                if (x < 0)
                {
                    x = point.X + rect.X;
                    SetLocationX(x);
                    return TAlign.TL;
                }
            }
            return TAlign.TR;
        }
        TAlign CLocationBottom(Point point, Size size, Rectangle rect, int width, int height, int padd, int ArrowSize, bool Collision = false)
        {
            base.SetLocation(point.X + (size.Width - width) / 2, point.Y + rect.Bottom - padd + ArrowSize);
            return TAlign.Bottom;
        }
        TAlign CLocationBR(Point point, Size size, Rectangle rect, int width, int height, int padd, int ArrowSize, bool Collision = false)
        {
            int x = point.X + (rect.X + rect.Width) - width + shadow2, y = point.Y + rect.Bottom - padd + ArrowSize;
            SetLocationX(x);
            base.SetLocationY(y);
            if (Collision)
            {
                var screen = Screen.FromPoint(TargetRect.Location).WorkingArea;
                if (x < 0)
                {
                    x = point.X + rect.X;
                    SetLocationX(x);
                    return TAlign.BL;
                }
            }
            return TAlign.BR;
        }
        TAlign CLocationBL(Point point, Size size, Rectangle rect, int width, int height, int padd, int ArrowSize, bool Collision = false)
        {
            int x = point.X + rect.X, y = point.Y + rect.Bottom - padd + ArrowSize;
            SetLocationX(x);
            base.SetLocationY(y);
            if (Collision)
            {
                var screen = Screen.FromPoint(TargetRect.Location).WorkingArea;
                if (x > (screen.X + screen.Width) - TargetRect.Width)
                {
                    x = point.X + (rect.X + rect.Width) - width + shadow2;
                    SetLocationX(x);
                    return TAlign.BR;
                }
            }
            return TAlign.BL;
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
        [Description("圆角"), Category("外观"), DefaultValue(0F)]
        public float Radius { get; set; }

        public abstract void PrintContent(Canvas g, Rectangle rect);
        public abstract void PrintBg(Canvas g, Rectangle rect, GraphicsPath path);

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
}