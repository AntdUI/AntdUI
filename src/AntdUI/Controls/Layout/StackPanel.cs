// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace AntdUI
{
    /// <summary>
    /// StackPanel 堆栈布局
    /// </summary>
    [Description("StackPanel 堆栈布局")]
    [ToolboxItem(true)]
    [DefaultProperty("Vertical")]
    [Designer(typeof(IControlDesigner))]
    public class StackPanel : ContainerPanel
    {
        bool autoscroll = false;
        /// <summary>
        /// 是否显示滚动条
        /// </summary>
        [Description("是否显示滚动条"), Category("外观"), DefaultValue(false)]
        public bool AutoScroll
        {
            get => autoscroll;
            set
            {
                if (autoscroll == value) return;
                autoscroll = value;
                if (autoscroll) ScrollBar = new ScrollBar(this);
                else ScrollBar = null;
                if (IsHandleCreated) IOnSizeChanged();
                OnPropertyChanged(nameof(AutoScroll));
            }
        }

        /// <summary>
        /// 滚动条
        /// </summary>
        [Browsable(false)]
        public ScrollBar? ScrollBar;

        public override Rectangle DisplayRectangle
        {
            get
            {
                var rect = ClientRectangle.DeflateRect(Padding);
                if (ScrollBar != null && ScrollBar.Show)
                {
                    if (ScrollBar.EnabledY) rect.Width -= ScrollBar.SIZE;
                    else rect.Height -= ScrollBar.SIZE;
                }
                return rect;
            }
        }

        /// <summary>
        /// 是否垂直方向
        /// </summary>
        [Description("是否垂直方向"), Category("外观"), DefaultValue(false)]
        public bool Vertical
        {
            get => layoutengine.Vertical;
            set
            {
                if (layoutengine.Vertical == value) return;
                layoutengine.Vertical = value;
                if (autoscroll) ScrollBar = new ScrollBar(this);
                else ScrollBar = null;
                if (IsHandleCreated) IOnSizeChanged();
                OnPropertyChanged(nameof(Vertical));
            }
        }

        [Description("反向"), Category("外观"), DefaultValue(RightToLeft.No)]
        public override RightToLeft RightToLeft
        {
            get => layoutengine.Reverse ? RightToLeft.Yes : RightToLeft.No;
            set
            {
                var reverse = value == RightToLeft.Yes;
                if (layoutengine.Reverse == reverse) return;
                layoutengine.Reverse = reverse;
                if (IsHandleCreated) IOnSizeChanged();
                OnPropertyChanged(nameof(RightToLeft));
            }
        }

        /// <summary>
        /// 内容大小
        /// </summary>
        [Description("内容大小"), Category("外观"), DefaultValue(null)]
        public string? ItemSize
        {
            get => layoutengine.ItemSize;
            set
            {
                if (layoutengine.ItemSize == value) return;
                layoutengine.ItemSize = value;
                if (IsHandleCreated) IOnSizeChanged();
                OnPropertyChanged(nameof(ItemSize));
            }
        }

        /// <summary>
        /// 间距
        /// </summary>
        [Description("间距"), Category("外观"), DefaultValue(0)]
        public int Gap
        {
            get => layoutengine.Gap;
            set
            {
                if (layoutengine.Gap == value) return;
                layoutengine.Gap = value;
                if (IsHandleCreated) IOnSizeChanged();
                OnPropertyChanged(nameof(Gap));
            }
        }

        bool pauseLayout = false;
        [Browsable(false), Description("暂停布局"), Category("行为"), DefaultValue(false)]
        public bool PauseLayout
        {
            get => pauseLayout;
            set
            {
                if (pauseLayout == value) return;
                pauseLayout = value;
                if (!value)
                {
                    Invalidate();
                    IOnSizeChanged();
                }
                OnPropertyChanged(nameof(PauseLayout));
            }
        }

        protected override void OnDraw(DrawEventArgs e)
        {
            var g = e.Canvas;
            PaintBack(g);
            base.OnDraw(e);
            ScrollBar?.Paint(g, ColorScheme);
        }

        #region 布局

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            IOnSizeChanged();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            var rect = ClientRectangle;
            base.OnSizeChanged(e);
            if (rect.Width == 0 || rect.Height == 0) return;
            ScrollBar?.SizeChange(rect);
        }

        StackLayout layoutengine = new StackLayout();
        public override LayoutEngine LayoutEngine => layoutengine;
        internal class StackLayout : LayoutEngine
        {
            /// <summary>
            /// 是否垂直方向
            /// </summary>
            public bool Vertical { get; set; }

            /// <summary>
            /// 内容大小
            /// </summary>
            public string? ItemSize { get; set; }

            /// <summary>
            /// 间距
            /// </summary>
            public int Gap { get; set; }

            public bool Reverse { get; set; }

            public override bool Layout(object container, LayoutEventArgs layoutEventArgs)
            {
                if (container is StackPanel parent && parent.IsHandleCreated && parent.Controls.Count > 0)
                {
                    if (parent.PauseLayout) return false;
                    var controls = new List<Control>(parent.Controls.Count);
                    foreach (Control it in parent.Controls)
                    {
                        if (it.Visible) controls.Insert(0, it);
                    }
                    if (controls.Count > 0)
                    {
                        var rect = parent.DisplayRectangle;
                        int val = 0;
                        if (ItemSize == null || string.IsNullOrEmpty(ItemSize)) val = HandLayout(parent, controls, rect);
                        else
                        {
                            if (ItemSize.EndsWith("%") && float.TryParse(ItemSize.TrimEnd('%'), out var f)) val = HandLayout(parent, controls, rect, (int)Math.Round((Vertical ? rect.Height : rect.Width) * (f / 100F)));
                            else if (int.TryParse(ItemSize, out var i)) val = HandLayout(parent, controls, rect, (int)Math.Round(i * parent.Dpi));
                            else val = HandLayoutFill(parent, controls, rect);
                        }
                        if (parent.ScrollBar != null)
                        {
                            bool old = parent.ScrollBar.Show;
                            parent.ScrollBar.SetVrSize(val);
                            if (old != parent.ScrollBar.Show) parent.BeginInvoke(parent.IOnSizeChanged);
                        }
                    }
                }
                return false;
            }

            int HandLayout(StackPanel parent, List<Control> controls, Rectangle rect)
            {
                int count = controls.Count;
                int offset = 0, use = 0, gap = 0;
                if (parent.ScrollBar != null) offset = parent.ScrollBar.Value;
                if (Gap > 0 && count > 1) gap = (int)Math.Round(Gap * parent.Dpi);
                if (Vertical)
                {
                    if (Reverse)
                    {
                        int startY = rect.Bottom;
                        foreach (var control in controls)
                        {
                            int controlHeight = control.Height;
                            int marginVertical = control.Margin.Vertical;

                            int y = startY - controlHeight - control.Margin.Bottom - use;

                            control.Location = new Point(rect.Left + control.Margin.Left, y + offset);
                            control.Width = rect.Width - control.Margin.Horizontal;

                            use += controlHeight + gap + marginVertical;
                        }
                    }
                    else
                    {
                        int startY = rect.Top;
                        foreach (var control in controls)
                        {
                            int controlHeight = control.Height;
                            int marginVertical = control.Margin.Vertical;

                            int y = startY + control.Margin.Top + use;

                            control.Location = new Point(rect.Left + control.Margin.Left, y - offset);
                            control.Width = rect.Width - control.Margin.Horizontal;

                            use += controlHeight + gap + marginVertical;
                        }
                    }
                }
                else
                {
                    if (Reverse)
                    {
                        int startX = rect.Right;
                        foreach (var control in controls)
                        {
                            int controlWidth = control.Width;
                            int marginHorizontal = control.Margin.Horizontal;

                            int x = startX - controlWidth - control.Margin.Right - use;

                            control.Location = new Point(x + offset, rect.Top + control.Margin.Top);
                            control.Height = rect.Height - control.Margin.Vertical;

                            use += controlWidth + gap + marginHorizontal;
                        }
                    }
                    else
                    {
                        int startX = rect.Left;
                        foreach (var control in controls)
                        {
                            int controlWidth = control.Width;
                            int marginHorizontal = control.Margin.Horizontal;

                            int x = startX + control.Margin.Left + use;

                            control.Location = new Point(x - offset, rect.Top + control.Margin.Top);
                            control.Height = rect.Height - control.Margin.Vertical;

                            use += controlWidth + gap + marginHorizontal;
                        }
                    }
                }
                return use;
            }
            int HandLayout(StackPanel parent, List<Control> controls, Rectangle rect, int size)
            {
                int count = controls.Count;
                int offset = 0, use = 0, gap = 0;
                if (parent.ScrollBar != null) offset = parent.ScrollBar.Value;
                if (Gap > 0 && count > 1) gap = (int)Math.Round(Gap * parent.Dpi);
                if (Vertical)
                {
                    if (Reverse)
                    {
                        int startY = rect.Bottom;
                        foreach (var control in controls)
                        {
                            int controlHeight = size;
                            int marginVertical = control.Margin.Vertical;

                            int y = startY - controlHeight - control.Margin.Bottom - use;

                            control.Location = new Point(rect.Left + control.Margin.Left, y + offset);
                            control.Size = new Size(rect.Width - control.Margin.Horizontal, controlHeight);

                            use += controlHeight + gap + marginVertical;
                        }
                    }
                    else
                    {
                        int startY = rect.Top;
                        foreach (var control in controls)
                        {
                            int controlHeight = size;
                            int marginVertical = control.Margin.Vertical;

                            int y = startY + control.Margin.Top + use;

                            control.Location = new Point(rect.Left + control.Margin.Left, y - offset);
                            control.Size = new Size(rect.Width - control.Margin.Horizontal, controlHeight);

                            use += controlHeight + gap + marginVertical;
                        }
                    }
                }
                else
                {
                    if (Reverse)
                    {
                        int startX = rect.Right;
                        foreach (var control in controls)
                        {
                            int controlWidth = size;
                            int marginHorizontal = control.Margin.Horizontal;

                            int x = startX - controlWidth - control.Margin.Right - use;

                            control.Location = new Point(x + offset, rect.Top + control.Margin.Top);
                            control.Size = new Size(controlWidth, rect.Height - control.Margin.Vertical);

                            use += controlWidth + gap + marginHorizontal;
                        }
                    }
                    else
                    {
                        int startX = rect.Left;
                        foreach (var control in controls)
                        {
                            int controlWidth = size;
                            int marginHorizontal = control.Margin.Horizontal;

                            int x = startX + control.Margin.Left + use;

                            control.Location = new Point(x - offset, rect.Top + control.Margin.Top);
                            control.Size = new Size(controlWidth, rect.Height - control.Margin.Vertical);

                            use += controlWidth + gap + marginHorizontal;
                        }
                    }
                }
                return use;
            }
            int HandLayoutFill(StackPanel parent, List<Control> controls, Rectangle rect)
            {
                int count = controls.Count;
                int usex = 0, usey = 0, gap = 0;
                if (Gap > 0 && count > 1) gap = (int)Math.Round(Gap * parent.Dpi);
                if (Vertical)
                {
                    int size = (rect.Height - (gap * (count - 1))) / count;
                    if (Reverse)
                    {
                        int startY = rect.Bottom;
                        foreach (var control in controls)
                        {
                            int controlHeight = size - control.Margin.Vertical;

                            int y = startY - controlHeight - control.Margin.Bottom - usey;

                            control.Location = new Point(rect.Left + control.Margin.Left, y);
                            control.Size = new Size(rect.Width - control.Margin.Horizontal, controlHeight);

                            usey += size + gap;
                        }
                    }
                    else
                    {
                        int startY = rect.Top;
                        foreach (var control in controls)
                        {
                            int controlHeight = size - control.Margin.Vertical;

                            int y = startY + control.Margin.Top + usey;

                            control.Location = new Point(rect.Left + control.Margin.Left, y);
                            control.Size = new Size(rect.Width - control.Margin.Horizontal, controlHeight);

                            usey += size + gap;
                        }
                    }
                }
                else
                {
                    int size = (rect.Width - (gap * (count - 1))) / count;
                    if (Reverse)
                    {
                        int startX = rect.Right;
                        foreach (var control in controls)
                        {
                            int controlWidth = size - control.Margin.Horizontal;

                            int x = startX - controlWidth - control.Margin.Right - usex;

                            control.Location = new Point(x, rect.Top + control.Margin.Top);
                            control.Size = new Size(controlWidth, rect.Height - control.Margin.Vertical);

                            usex += size + gap;
                        }
                    }
                    else
                    {
                        int startX = rect.Left;
                        foreach (var control in controls)
                        {
                            int controlWidth = size - control.Margin.Horizontal;

                            int x = startX + control.Margin.Left + usex;

                            control.Location = new Point(x, rect.Top + control.Margin.Top);
                            control.Size = new Size(controlWidth, rect.Height - control.Margin.Vertical);

                            usex += size + gap;
                        }
                    }
                }
                return 0;
            }
        }

        #endregion

        #region 鼠标

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (ScrollBar != null && ScrollBar.MouseDown(e.X, e.Y)) { OnTouchDown(e.X, e.Y); return; }
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (ScrollBar != null && ScrollBar.MouseMove(e.X, e.Y) && OnTouchMove(e.X, e.Y)) return;
            base.OnMouseMove(e);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            ScrollBar?.MouseUp();
            OnTouchUp();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            ScrollBar?.Leave();
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            ScrollBar?.Leave();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            ScrollBar?.MouseWheel(e);
            base.OnMouseWheel(e);
        }
        protected override bool OnTouchScrollX(int value)
        {
            if (ScrollBar != null && ScrollBar.EnabledX) return ScrollBar.MouseWheelXCore(value);
            return false;
        }
        protected override bool OnTouchScrollY(int value)
        {
            if (ScrollBar != null && ScrollBar.EnabledY) return ScrollBar.MouseWheelYCore(value);
            return false;
        }

        #endregion

        #region 控件添加和移除

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            e.Control!.GotFocus += Control_GotFocus;
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            base.OnControlRemoved(e);
            e.Control!.GotFocus -= Control_GotFocus;
        }

        private void Control_GotFocus(object? sender, EventArgs e)
        {
            if (sender is Control control) ScrollControlIntoView(control);
        }

        #endregion

        #region 滚动控件到视图

        public void ScrollControlIntoView(Control activeControl)
        {
            if (ScrollBar == null) return;
            if (ScrollBar.Show)
            {
                Rectangle clientRect = ClientRectangle, controlRect = activeControl.Bounds;
                if (Vertical)
                {
                    if (controlRect.Top < clientRect.Top) ScrollBar.ValueY = Math.Max(0, ScrollBar.ValueY + controlRect.Top - clientRect.Top);
                    else if (controlRect.Bottom > clientRect.Bottom) ScrollBar.ValueY = Math.Min(ScrollBar.MaxY, ScrollBar.ValueY + controlRect.Bottom - clientRect.Bottom);
                }
                else
                {
                    if (controlRect.Left < clientRect.Left) ScrollBar.ValueX = Math.Max(0, ScrollBar.ValueX + controlRect.Left - clientRect.Left);
                    else if (controlRect.Right > clientRect.Right) ScrollBar.ValueX = Math.Min(ScrollBar.MaxX, ScrollBar.ValueX + controlRect.Right - clientRect.Right);
                }
            }
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            foreach (Control c in Controls) c.GotFocus -= Control_GotFocus;
            ScrollBar?.Dispose();
            base.Dispose(disposing);
        }
    }
}