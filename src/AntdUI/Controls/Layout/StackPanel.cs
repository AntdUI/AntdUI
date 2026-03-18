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
        #region 属性

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
                InitScroll();
                OnPropertyChanged(nameof(AutoScroll));
            }
        }

        public override Rectangle DisplayRectangle => ClientRectangle.DeflateRect(Padding);

        /// <summary>
        /// 是否垂直方向
        /// </summary>
        [Description("是否垂直方向"), Category("外观"), DefaultValue(false)]
        public bool Vertical
        {
            get => Panel.Vertical;
            set
            {
                Panel.Vertical = value;
                InitScroll();
                OnPropertyChanged(nameof(Vertical));
            }
        }

        [Description("反向"), Category("外观"), DefaultValue(RightToLeft.No)]
        public override RightToLeft RightToLeft
        {
            get => Panel.RightToLeft;
            set
            {
                Panel.RightToLeft = value;
                OnPropertyChanged(nameof(RightToLeft));
            }
        }

        /// <summary>
        /// 内容大小
        /// </summary>
        [Description("内容大小"), Category("外观"), DefaultValue(null)]
        public string? ItemSize
        {
            get => Panel.ItemSize;
            set
            {
                Panel.ItemSize = value;
                OnPropertyChanged(nameof(ItemSize));
            }
        }

        /// <summary>
        /// 间距
        /// </summary>
        [Description("间距"), Category("外观"), DefaultValue(0)]
        public int Gap
        {
            get => Panel.Gap;
            set
            {
                Panel.Gap = value;
                OnPropertyChanged(nameof(Gap));
            }
        }

        [Browsable(false), Description("暂停布局"), Category("行为"), DefaultValue(false)]
        public bool PauseLayout
        {
            get => Panel.PauseLayout;
            set
            {
                Panel.PauseLayout = value;
                OnPropertyChanged(nameof(PauseLayout));
            }
        }

        #endregion

        protected override void OnDraw(DrawEventArgs e)
        {
            var g = e.Canvas;
            PaintBack(g);
            base.OnDraw(e);
        }

        #region 方法

        #region 滚动控件到视图

        public void ScrollControlIntoView(Control activeControl) => Panel?.ScrollControlIntoView(activeControl);

        #endregion

        #endregion

        protected override void Dispose(bool disposing)
        {
            Panel.Dispose();
            base.Dispose(disposing);
        }

        #region 核心

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            Panel.Size = Size;
            //Panel.IOnSizeChanged();
            InitScroll();
            IOnSizeChanged();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            XScroll?.SetSize(Panel.Width);
            YScroll?.SetSize(Panel.Height);
        }

        void InitScroll()
        {
            if (autoscroll)
            {
                if (Panel.Vertical)
                {
                    if (XScroll != null)
                    {
                        XScroll.ValueChanged -= Scroll_ValueChanged;
                        XScroll.Dispose();
                        XScroll = null;
                    }
                    if (YScroll == null)
                    {
                        int w = SystemInformation.VerticalScrollBarWidth, h = Panel.Height;
                        YScroll = new YScrollBar
                        {
                            Name = "__BARY__",
                            Visible = false,
                            Radius = Radius,
                            Dock = DockStyle.Right,
                            MinimumSize = new Size(w, 0),
                            Size = new Size(w, h)
                        };
                        YScroll.SetSize(h);
                        YScroll.ValueChanged += Scroll_ValueChanged;
                        base.Controls.Add(YScroll);
                    }
                }
                else
                {
                    if (YScroll != null)
                    {
                        YScroll.ValueChanged -= Scroll_ValueChanged;
                        YScroll.Dispose();
                        YScroll = null;
                    }
                    if (XScroll == null)
                    {
                        int h = SystemInformation.HorizontalScrollBarHeight, w = Panel.Width;
                        XScroll = new XScrollBar
                        {
                            Name = "__BARX__",
                            Visible = false,
                            Radius = Radius,
                            Dock = DockStyle.Bottom,
                            MinimumSize = new Size(0, h),
                            Size = new Size(w, h)
                        };
                        XScroll.SetSize(w);
                        XScroll.ValueChanged += Scroll_ValueChanged;
                        base.Controls.Add(XScroll);
                    }
                }
            }
            else
            {
                if (YScroll != null)
                {
                    YScroll.ValueChanged -= Scroll_ValueChanged;
                    YScroll.Dispose();
                    YScroll = null;
                }
                if (XScroll != null)
                {
                    XScroll.ValueChanged -= Scroll_ValueChanged;
                    XScroll.Dispose();
                    XScroll = null;
                }
            }
        }

        private void Scroll_ValueChanged(object? sender, EventArgs e) => Panel?.IOnSizeChanged();

        private StackPanelCore Panel;
        public XScrollBar? XScroll;
        public YScrollBar? YScroll;

        public StackPanel()
        {
            Panel = new StackPanelCore(this)
            {
                Name = "__IN__",
                Dock = DockStyle.Fill
            };
            base.Controls.Add(Panel);
        }

        class StackPanelCore : IControl
        {
            public StackPanel Panel;
            public StackPanelCore(StackPanel core)
            {
                Panel = core;
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
                    if (IsHandleCreated) IOnSizeChanged();
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
                }
            }

            #region 布局

            StackLayout layoutengine = new StackLayout();
            public override LayoutEngine LayoutEngine => layoutengine;

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
                if (Vertical)
                {
                    if (Panel.YScroll == null) return;
                    if (Panel.YScroll.Visible)
                    {
                        Rectangle clientRect = ClientRectangle, controlRect = activeControl.Bounds;
                        int value = Panel.YScroll.Value, max = Panel.YScroll.Maximum;
                        if (controlRect.Top < clientRect.Top) Panel.YScroll.Value = Math.Max(0, value + controlRect.Top - clientRect.Top);
                        else if (controlRect.Bottom > clientRect.Bottom) Panel.YScroll.Value = Math.Min(max, value + controlRect.Bottom - clientRect.Bottom);
                    }
                }
                else
                {
                    if (Panel.XScroll == null) return;
                    if (Panel.XScroll.Visible)
                    {
                        Rectangle clientRect = ClientRectangle, controlRect = activeControl.Bounds;
                        int value = Panel.XScroll.Value, max = Panel.XScroll.Maximum;
                        if (controlRect.Left < clientRect.Left) Panel.XScroll.Value = Math.Max(0, value + controlRect.Left - clientRect.Left);
                        else if (controlRect.Right > clientRect.Right) Panel.XScroll.Value = Math.Min(max, value + controlRect.Right - clientRect.Right);
                    }
                }
            }

            #endregion

            protected override void Dispose(bool disposing)
            {
                foreach (Control c in Controls) c.GotFocus -= Control_GotFocus;
                base.Dispose(disposing);
            }
        }

        #region 布局

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
                if (container is StackPanelCore parent && parent.Panel.IsHandleCreated && parent.Controls.Count > 0)
                {
                    if (parent.PauseLayout) return false;
                    var controls = new List<Control>(parent.Controls.Count);
                    foreach (Control it in parent.Controls)
                    {
                        if (it.Visible) controls.Insert(0, it);
                    }
                    if (controls.Count > 0)
                    {
                        var rect = parent.ClientRectangle;
                        int val = 0;
                        if (ItemSize == null || string.IsNullOrEmpty(ItemSize)) val = HandLayout(parent, controls, rect);
                        else
                        {
                            if (ItemSize.EndsWith("%") && float.TryParse(ItemSize.TrimEnd('%'), out var f)) val = HandLayout(parent, controls, rect, (int)Math.Round((Vertical ? rect.Height : rect.Width) * (f / 100F)));
                            else if (int.TryParse(ItemSize, out var i)) val = HandLayout(parent, controls, rect, (int)Math.Round(i * parent.Dpi));
                            else val = HandLayoutFill(parent, controls, rect);
                        }
                        SetMax(parent, val);
                    }
                }
                return false;
            }

            int GetOffset(StackPanelCore parent)
            {
                if (Vertical)
                {
                    if (parent.Panel.YScroll == null) return 0;
                    if (parent.Panel.YScroll.Visible) return parent.Panel.YScroll.Value;
                }
                else
                {
                    if (parent.Panel.XScroll == null) return 0;
                    if (parent.Panel.XScroll.Visible) return parent.Panel.XScroll.Value;
                }
                return 0;
            }
            void SetMax(StackPanelCore parent, int val)
            {
                if (Vertical) parent.Panel.YScroll?.SetShow(val);
                else parent.Panel.XScroll?.SetShow(val);
            }
            int HandLayout(StackPanelCore parent, List<Control> controls, Rectangle rect)
            {
                int count = controls.Count;
                int offset = GetOffset(parent), use = 0, gap = 0;
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
            int HandLayout(StackPanelCore parent, List<Control> controls, Rectangle rect, int size)
            {
                int count = controls.Count;
                int offset = GetOffset(parent), use = 0, gap = 0;
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
            int HandLayoutFill(StackPanelCore parent, List<Control> controls, Rectangle rect)
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

        #region 控件

        protected override void OnControlAdded(ControlEventArgs e)
        {
            if (e.Control == null) return;
            if (e.Control.Name == "__IN__" || e.Control.Name == "__BARX__" || e.Control.Name == "__BARY__")
            {
                base.OnControlAdded(e);
                return;
            }
            if (DesignMode)
            {
                base.OnControlAdded(e);
                return;
            }
            Add(e.Control);
        }

        public void Remove(Control control) => Panel.Controls.Remove(control);
        public void RemoveAt(int index) => Panel.Controls.RemoveAt(index);

        public void Add(Control control) => Panel.Controls.Add(control);

        public void Clear() => Panel.Controls.Clear();

        [Browsable(false)]
        public new ControlCollection Controls => Panel.Controls;

        [Browsable(false)]
        public Control? this[string name] => Panel.Controls.ContainsKey(name) ? Panel.Controls[name] : null;

        [Browsable(false)]
        public Control? this[int index] => (index >= 0 && index < Panel.Controls.Count) ? Panel.Controls[index] : null;

        #endregion

        #endregion
    }
}