﻿// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
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

        public TAlign CLocation(IControl control, TAlignFrom Placement, bool DropDownArrow, int ArrowSize, bool Collision = false)
        {
            var calculateCoordinate = new CalculateCoordinate(control, TargetRect, DropDownArrow ? ArrowSize : 0, shadow, shadow2);
            calculateCoordinate.Auto(Placement, animateConfig, Collision, out var align, out int x, out int y);
            SetLocation(x - shadow, y);
            return align;
        }
        public TAlign CLocation(IControl control, TAlignFrom Placement, Rectangle rect_real, bool DropDownArrow, int ArrowSize, bool Collision = false)
        {
            var calculateCoordinate = new CalculateCoordinate(control, TargetRect, DropDownArrow ? ArrowSize : 0, shadow, shadow2, rect_real);
            calculateCoordinate.Auto(Placement, animateConfig, Collision, out var align, out int x, out int y);
            SetLocation(x - shadow, y);
            return align;
        }
        public TAlign CLocation(ILayeredShadowForm control, Rectangle rect, bool DropDownArrow, int ArrowSize)
        {
            var trect = control.TargetRect;
            var screen = Screen.FromPoint(trect.Location).WorkingArea;
            int x = trect.X + trect.Width - rect.X - control.shadow + (DropDownArrow ? ArrowSize : 0), y = trect.Y + rect.Y + control.shadow;
            if (screen.Right < x + TargetRect.Width) x = x - ((x + TargetRect.Width) - screen.Right) + control.shadow;
            if (screen.Bottom < y + TargetRect.Height) y = y - ((y + TargetRect.Height) - screen.Bottom) + control.shadow;
            SetLocation(x - shadow, y - shadow);
            return TAlign.LT;
        }

        #endregion

        #region 布局

        public override void SetRect(Rectangle rect)
        {
            if (ShadowEnabled)
            {
                SetLocation(rect.X - shadow, rect.Y - shadow);
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

        public void SetLocationO(Point point) => SetLocationO(point.X, point.Y);
        public void SetLocationOX(int x)
        {
            if (ShadowEnabled) SetLocationX(x - shadow);
            else SetLocationX(x);
        }
        public void SetLocationOY(int y)
        {
            if (ShadowEnabled) SetLocationY(y - shadow);
            else SetLocationY(y);
        }
        public void SetLocationO(int x, int y)
        {
            if (ShadowEnabled) SetLocation(x - shadow, y - shadow);
            else SetLocation(x, y);
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

        public abstract void PrintContent(Canvas g, Rectangle rect, GraphicsState state);
        public abstract void PrintBg(Canvas g, Rectangle rect, GraphicsPath path);
        public void PrintAndClear()
        {
            ClearShadow();
            Print();
        }
        public void ClearShadow()
        {
            shadow_temp?.Dispose();
            shadow_temp = null;
        }

        public override Bitmap PrintBit()
        {
            var rect = TargetRectXY;
            Bitmap original_bmp = new Bitmap(rect.Width, rect.Height);
            using (var g = Graphics.FromImage(original_bmp).HighLay())
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
                    g.SetClip(rect_read);
                    g.TranslateTransform(shadow, shadow);
                    var state = g.Save();
                    PrintContent(g, new Rectangle(0, 0, rect_read.Width, rect_read.Height), state);
                }
                else
                {
                    using (var path = rect.RoundPath(Radius))
                    {
                        PrintBg(g, rect, path);
                    }
                    PrintContent(g, rect, g.Save());
                }
            }
            return original_bmp;
        }

        SafeBitmap? shadow_temp;
    }

    public abstract class ILayeredShadowFormOpacity : ILayeredFormOpacity
    {
        public int shadow = 0, shadow2 = 0;
        bool ShadowEnabled = Config.ShadowEnabled;
        public ILayeredShadowFormOpacity(byte maxalpha = 255) : base(maxalpha)
        {
            if (ShadowEnabled)
            {
                shadow = (int)(10 * Config.Dpi);
                shadow2 = shadow * 2;
            }
        }

        #region 布局

        public override void SetRect(Rectangle rect)
        {
            if (ShadowEnabled)
            {
                SetLocation(rect.X - shadow, rect.Y - shadow);
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

        public void SetLocationO(Point point) => SetLocationO(point.X, point.Y);
        public void SetLocationOX(int x)
        {
            if (ShadowEnabled) SetLocationX(x - shadow);
            else SetLocationX(x);
        }
        public void SetLocationOY(int y)
        {
            if (ShadowEnabled) SetLocationY(y - shadow);
            else SetLocationY(y);
        }
        public void SetLocationO(int x, int y)
        {
            if (ShadowEnabled) SetLocation(x - shadow, y - shadow);
            else SetLocation(x, y);
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

        public abstract void PrintContent(Canvas g, Rectangle rect, GraphicsState state);
        public abstract void PrintBg(Canvas g, Rectangle rect, GraphicsPath path);
        public void PrintAndClear()
        {
            ClearShadow();
            Print();
        }
        public void ClearShadow()
        {
            shadow_temp?.Dispose();
            shadow_temp = null;
        }

        public override Bitmap PrintBit()
        {
            var rect = TargetRectXY;
            Bitmap original_bmp = new Bitmap(rect.Width, rect.Height);
            using (var g = Graphics.FromImage(original_bmp).HighLay())
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
                    g.SetClip(rect_read);
                    g.TranslateTransform(shadow, shadow);
                    var state = g.Save();
                    PrintContent(g, new Rectangle(0, 0, rect_read.Width, rect_read.Height), state);
                }
                else
                {
                    using (var path = rect.RoundPath(Radius))
                    {
                        PrintBg(g, rect, path);
                    }
                    PrintContent(g, rect, g.Save());
                }
            }
            return original_bmp;
        }

        SafeBitmap? shadow_temp;
    }
}