// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
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
                shadow = (int)(10 * Dpi);
                shadow2 = shadow * 2;
            }
        }

        #region 坐标

        public void CLocation(IControl control, TAlignFrom Placement, bool DropDownArrow, int ArrowSize)
        {
            var calculateCoordinate = new CalculateCoordinate(this, control, TargetRect, Radius, DropDownArrow ? ArrowSize : 0, shadow, shadow2);
            calculateCoordinate.Auto(Placement, animateConfig, true, out int x, out int y, out ArrowLine);
            SetLocation(x, y);
        }
        public void CLocation(IControl control, TAlignFrom Placement, Rectangle rect_real, bool DropDownArrow, int ArrowSize, bool Collision = false)
        {
            var calculateCoordinate = new CalculateCoordinate(this, control, TargetRect, Radius, DropDownArrow ? ArrowSize : 0, shadow, shadow2, rect_real);
            calculateCoordinate.Auto(Placement, animateConfig, Collision, out int x, out int y, out ArrowLine);
            SetLocation(x, y);
        }
        public TAlign CLocation(ILayeredShadowForm control, Rectangle rect, bool DropDownArrow, int ArrowSize)
        {
            var trect = control.TargetRect;
            var screen = Screen.FromPoint(new Point(trect.X + control.shadow, trect.Y + control.shadow)).WorkingArea;
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
            int x = e.X - shadow, y = e.Y - shadow;
            if (EnableSafetyTriangleZone)
            {
                bool ret = false;
                if (SafetyTriangleZone != null && Helper.IsPointInTriangle(x, y, SafetyTriangleZone)) ret = true;
                var rect = OnSafetyTriangleZone(x, y);
                if (rect.HasValue)
                {
                    var now = DateTime.Now;
                    if ((now - SafetyTriangleFlag).TotalMilliseconds > 500)
                    {
                        SafetyTriangleFlag = now;
                        int x2 = rect.Value.X - TargetRect.X, y2 = rect.Value.Y - TargetRect.Y;
                        SafetyTriangleZone = new Point[] {
                            new Point(x, y),
                            new Point(x2, y2),
                            new Point(x2, y2 + rect.Value.Height)
                        };
                    }
                }
                if (ret) return;
            }
            OnMouseMove(e.Button, e.Clicks, x, y, e.Delta);
        }
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (RunAnimation) return;
            base.OnMouseWheel(e);
            OnMouseWheel(e.Button, e.Clicks, e.X - shadow, e.Y - shadow, e.Delta);
        }

        #endregion

        #region 安全三角区

        Point[]? SafetyTriangleZone;
        DateTime SafetyTriangleFlag;

        /// <summary>
        /// 是否启用安全三角区
        /// </summary>
        public virtual bool EnableSafetyTriangleZone => false;
        public virtual Rectangle? OnSafetyTriangleZone(int x, int y) => null;

        #endregion

        /// <summary>
        /// 圆角
        /// </summary>
        [Description("圆角"), Category("外观"), DefaultValue(0)]
        public int Radius { get; set; }

        public Point[]? ArrowLine;

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

        public override Bitmap? PrintBit()
        {
            var rect = TargetRectXY;
            Bitmap rbmp = new Bitmap(rect.Width, rect.Height);
            using (var g = Graphics.FromImage(rbmp).HighLay())
            {
                if (ShadowEnabled)
                {
                    var rect_read = new Rectangle(shadow, shadow, rect.Width - shadow2, rect.Height - shadow2);
                    using (var path = rect_read.RoundPath(Radius))
                    {
                        shadow_temp ??= path.PaintShadow(rect.Width, rect.Height, shadow);
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
            return rbmp;
        }

        SafeBitmap? shadow_temp;

        protected override void Dispose(bool disposing)
        {
            ClearShadow();
            base.Dispose(disposing);
        }
    }

    public abstract class ILayeredShadowFormOpacity : ILayeredFormOpacity
    {
        public int shadow = 0, shadow2 = 0;
        bool ShadowEnabled = Config.ShadowEnabled;
        public ILayeredShadowFormOpacity(byte maxalpha = 255) : base(maxalpha)
        {
            if (ShadowEnabled)
            {
                shadow = (int)(10 * Dpi);
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
            int x = e.X - shadow, y = e.Y - shadow;
            if (EnableSafetyTriangleZone)
            {
                bool ret = false;
                if (SafetyTriangleZone != null && Helper.IsPointInTriangle(x, y, SafetyTriangleZone)) ret = true;
                var rect = OnSafetyTriangleZone(x, y);
                if (rect.HasValue)
                {
                    var now = DateTime.Now;
                    if ((now - SafetyTriangleFlag).TotalMilliseconds > 500)
                    {
                        SafetyTriangleFlag = now;
                        int x2 = rect.Value.X - TargetRect.X, y2 = rect.Value.Y - TargetRect.Y;
                        SafetyTriangleZone = new Point[] {
                            new Point(x, y),
                            new Point(x2, y2),
                            new Point(x2, y2 + rect.Value.Height)
                        };
                    }
                }
                if (ret) return;
            }
            OnMouseMove(e.Button, e.Clicks, x, y, e.Delta);
        }
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (RunAnimation) return;
            base.OnMouseWheel(e);
            OnMouseWheel(e.Button, e.Clicks, e.X - shadow, e.Y - shadow, e.Delta);
        }

        #endregion

        #region 安全三角区

        Point[]? SafetyTriangleZone;
        DateTime SafetyTriangleFlag;

        /// <summary>
        /// 是否启用安全三角区
        /// </summary>
        public virtual bool EnableSafetyTriangleZone => false;
        public virtual Rectangle? OnSafetyTriangleZone(int x, int y) => null;

        #endregion

        /// <summary>
        /// 圆角
        /// </summary>
        [Description("圆角"), Category("外观"), DefaultValue(0)]
        public int Radius { get; set; }

        public Point[]? ArrowLine;

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

        public override Bitmap? PrintBit()
        {
            var rect = TargetRectXY;
            Bitmap rbmp = new Bitmap(rect.Width, rect.Height);
            using (var g = Graphics.FromImage(rbmp).HighLay())
            {
                if (ShadowEnabled)
                {
                    var rect_read = new Rectangle(shadow, shadow, rect.Width - shadow2, rect.Height - shadow2);
                    using (var path = rect_read.RoundPath(Radius))
                    {
                        shadow_temp ??= path.PaintShadow(rect.Width, rect.Height, shadow);
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
            return rbmp;
        }

        SafeBitmap? shadow_temp;

        protected override void Dispose(bool disposing)
        {
            ClearShadow();
            base.Dispose(disposing);
        }
    }
}