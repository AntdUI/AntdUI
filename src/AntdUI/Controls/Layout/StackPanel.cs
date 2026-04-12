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
        [Description("是否显示滚动条"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(false)]
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

        public override Rectangle DisplayRectangle => ClientRectangle.PaddingRect(Padding, BorderWidth);

        /// <summary>
        /// 是否垂直方向
        /// </summary>
        [Description("是否垂直方向"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(false)]
        public bool Vertical
        {
            get => layoutengine.Vertical;
            set
            {
                if (layoutengine.Vertical == value) return;
                layoutengine.Vertical = value;
                InitScroll();
                if (IsHandleCreated) IOnSizeChanged();
                OnPropertyChanged(nameof(Vertical));
            }
        }

        [Description("反向"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(RightToLeft.No)]
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
        [Description("内容大小"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null)]
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
        [Description("间距"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(0)]
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
        [Browsable(false), Description("暂停布局"), Category(nameof(CategoryAttribute.Behavior)), DefaultValue(false)]
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

        /// <summary>
        /// 内部容器背景透明
        /// </summary>
        [Description("内部容器背景透明"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(false)]
        public bool AutoContainerBgTransparent { get; set; }

        #endregion

        #region 原生

        protected override void OnContextMenuStripChanged(EventArgs e)
        {
            base.OnContextMenuStripChanged(e);
            if (Panel != null) Panel.ContextMenuStrip = ContextMenuStrip;
        }

        #endregion

        #region 布局

        StackLayout layoutengine = new StackLayout();
        public override LayoutEngine LayoutEngine => layoutengine;

        #endregion

        protected override void OnDraw(DrawEventArgs e)
        {
            var g = e.Canvas;
            PaintBack(g);
            base.OnDraw(e);
        }

        #region 方法

        public void ScrollControlIntoView(Control activeControl) => Panel.ScrollControlIntoView(activeControl);

        #endregion

        #region 核心

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            InitScroll();
            if (AutoContainerBgTransparent) Panel.BackColor = Color.Transparent;
            IOnSizeChanged();
        }

        void InitScroll()
        {
            if (autoscroll)
            {
                if (Vertical)
                {
                    if (XScroll != null)
                    {
                        XScroll.ValueChanged -= Scroll_ValueChanged;
                        XScroll.Dispose();
                        XScroll = null;
                    }
                    if (YScroll == null)
                    {
                        YScroll = new YScrollBar
                        {
                            Name = "__BARY__",
                            Visible = false,
                            Radius = Radius
                        };
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
                        XScroll = new XScrollBar
                        {
                            Name = "__BARX__",
                            Visible = false,
                            Radius = Radius
                        };
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

        private void Scroll_ValueChanged(object? sender, IntEventArgs e) => PerformLayout();

        private StackPanelCore Panel;
        public XScrollBar? XScroll;
        public YScrollBar? YScroll;

        public StackPanel()
        {
            Panel = new StackPanelCore(this)
            {
                Name = "__IN__"
            };
            base.Controls.Add(Panel);
        }

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

        public void Add(Control control, bool focus = false)
        {
            Panel.Controls.Add(control);
            if (focus) control.Focus();
        }
        public void AddToBack(Control control, bool focus = false)
        {
            var old = pauseLayout;
            PauseLayout = true;
            Panel.Controls.Add(control);
            pauseLayout = old;
            control.BringToFront();
            if (focus) control.Focus();
        }
        public void AddToFront(Control control, bool focus = false)
        {
            var old = pauseLayout;
            PauseLayout = true;
            Panel.Controls.Add(control);
            pauseLayout = old;
            control.SendToBack();
            if (focus) control.Focus();
        }

        public void Clear() => Panel.Controls.Clear();

        [Browsable(false)]
        public new ControlCollection Controls => Panel.Controls;

        internal ControlCollection ControlsBase => base.Controls;

        [Browsable(false)]
        public Control? this[string name] => Panel.Controls.ContainsKey(name) ? Panel.Controls[name] : null;

        [Browsable(false)]
        public Control? this[int index] => (index >= 0 && index < Panel.Controls.Count) ? Panel.Controls[index] : null;

        #endregion

        #endregion

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
                if (container is StackPanel parent && parent.IsHandleCreated && parent.ControlsBase.Count > 0)
                {
                    if (parent.PauseLayout) return false;
                    var controls = new List<Control>(parent.ControlsBase.Count);
                    foreach (Control it in parent.ControlsBase)
                    {
                        if (it.Visible)
                        {
                            if (it.Name == "__IN__" || it.Name == "__BARY__" || it.Name == "__BARX__") continue;
                            controls.Insert(0, it);
                        }
                    }
                    if (controls.Count > 0)
                    {
                        var rect = parent.DisplayRectangle;
                        Rectangle[] rects;
                        if (ItemSize == null || string.IsNullOrEmpty(ItemSize)) rects = ConvertToRects(0, parent.Dpi, controls, rect, out _);
                        else
                        {
                            if (ItemSize.EndsWith("%") && float.TryParse(ItemSize.TrimEnd('%'), out var f)) rects = ConvertToRects(0, parent.Dpi, controls, rect, (int)Math.Round((Vertical ? rect.Height : rect.Width) * (f / 100F)), out _);
                            else if (int.TryParse(ItemSize, out var i)) rects = ConvertToRects(0, parent.Dpi, controls, rect, (int)Math.Round(i * parent.Dpi), out _);
                            else rects = ConvertToRectsFill(parent.Dpi, controls, rect);
                        }
                        HandLayout(controls, rects);
                    }
                    else if (parent.Panel.Controls.Count > 0)
                    {
                        while (true)
                        {
                            var rect = parent.DisplayRectangle;
                            if (parent.XScroll != null)
                            {
                                if (parent.XScroll.Visible)
                                {
                                    int h = SystemInformation.HorizontalScrollBarHeight;
                                    parent.XScroll.Bounds = new Rectangle(rect.X, rect.Bottom - h, rect.Width, h);
                                    rect.Height -= h;
                                }
                                if (parent.XScroll.SetSize(rect.Width)) continue;
                            }
                            if (parent.YScroll != null)
                            {
                                if (parent.YScroll.Visible)
                                {
                                    int w = SystemInformation.VerticalScrollBarWidth;
                                    parent.YScroll.Bounds = new Rectangle(rect.Right - w, rect.Y, w, rect.Height);
                                    rect.Width -= w;
                                }
                                if (parent.YScroll.SetSize(rect.Height)) continue;
                            }
                            parent.Panel.Bounds = rect;
                            if (Layout(parent.Panel, new Rectangle(0, 0, rect.Width, rect.Height))) return true;
                        }
                    }
                }
                return false;
            }
            bool Layout(StackPanelCore panel, Rectangle rect)
            {
                var controls = new List<Control>(panel.Controls.Count);
                foreach (Control it in panel.Controls)
                {
                    if (it.Visible) controls.Insert(0, it);
                }
                if (controls.Count > 0)
                {
                    int val = 0;
                    Rectangle[] rects;
                    if (ItemSize == null || string.IsNullOrEmpty(ItemSize)) rects = ConvertToRects(GetOffset(panel), panel.Dpi, controls, rect, out val);
                    else
                    {
                        if (ItemSize.EndsWith("%") && float.TryParse(ItemSize.TrimEnd('%'), out var f)) rects = ConvertToRects(GetOffset(panel), panel.Dpi, controls, rect, (int)Math.Round((Vertical ? rect.Height : rect.Width) * (f / 100F)), out val);
                        else if (int.TryParse(ItemSize, out var i)) rects = ConvertToRects(GetOffset(panel), panel.Dpi, controls, rect, (int)Math.Round(i * panel.Dpi), out val);
                        else rects = ConvertToRectsFill(panel.Dpi, controls, rect);
                    }
                    if (SetMax(panel, val, rect)) return false;
                    HandLayout(controls, rects);
                    return true;
                }
                return true;
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
            bool SetMax(StackPanelCore parent, int val, Rectangle rect)
            {
                if (Vertical) return parent.Panel.YScroll?.SetShow(val, rect.Height) ?? false;
                else return parent.Panel.XScroll?.SetShow(val, rect.Width) ?? false;
            }
            void HandLayout(List<Control> controls, Rectangle[] rects)
            {
                for (int i = 0; i < controls.Count; i++)
                {
                    var control = controls[i];
                    var rect = rects[i];
                    control.Location = rect.Location;
                    control.Size = rect.Size;
                    if (i >= rects.Length) return;
                }
            }
            Rectangle[] ConvertToRects(int offset, float dpi, List<Control> controls, Rectangle rect, out int use)
            {
                int count = controls.Count;
                use = 0;
                int gap = 0;
                if (Gap > 0 && count > 1) gap = (int)Math.Round(Gap * dpi);
                var rectsList = new List<Rectangle>(count);
                if (Vertical)
                {
                    if (Reverse)
                    {
                        int startY = rect.Bottom;
                        foreach (var control in controls)
                        {
                            int controlHeight = control.Height, marginVertical = control.Margin.Vertical, y = startY - controlHeight - control.Margin.Bottom - use;

                            rectsList.Add(new Rectangle(rect.Left + control.Margin.Left, y + offset, rect.Width - control.Margin.Horizontal, controlHeight));

                            use += controlHeight + gap + marginVertical;
                        }
                    }
                    else
                    {
                        int startY = rect.Top;
                        foreach (var control in controls)
                        {
                            int controlHeight = control.Height, marginVertical = control.Margin.Vertical, y = startY + control.Margin.Top + use;

                            rectsList.Add(new Rectangle(rect.Left + control.Margin.Left, y - offset, rect.Width - control.Margin.Horizontal, controlHeight));

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
                            int controlWidth = control.Width, marginHorizontal = control.Margin.Horizontal, x = startX - controlWidth - control.Margin.Right - use;

                            rectsList.Add(new Rectangle(x + offset, rect.Top + control.Margin.Top, controlWidth, rect.Height - control.Margin.Vertical));

                            use += controlWidth + gap + marginHorizontal;
                        }
                    }
                    else
                    {
                        int startX = rect.Left;
                        foreach (var control in controls)
                        {
                            int controlWidth = control.Width, marginHorizontal = control.Margin.Horizontal, x = startX + control.Margin.Left + use;

                            rectsList.Add(new Rectangle(x - offset, rect.Top + control.Margin.Top, controlWidth, rect.Height - control.Margin.Vertical));

                            use += controlWidth + gap + marginHorizontal;
                        }
                    }
                }
                return rectsList.ToArray();
            }
            Rectangle[] ConvertToRects(int offset, float dpi, List<Control> controls, Rectangle rect, int size, out int use)
            {
                int count = controls.Count;
                use = 0;
                int gap = 0;
                if (Gap > 0 && count > 1) gap = (int)Math.Round(Gap * dpi);
                var rectsList = new List<Rectangle>(count);
                if (Vertical)
                {
                    if (Reverse)
                    {
                        int startY = rect.Bottom;
                        foreach (var control in controls)
                        {
                            int controlHeight = size, marginVertical = control.Margin.Vertical, y = startY - controlHeight - control.Margin.Bottom - use;

                            rectsList.Add(new Rectangle(rect.Left + control.Margin.Left, y + offset, rect.Width - control.Margin.Horizontal, controlHeight));

                            use += controlHeight + gap + marginVertical;
                        }
                    }
                    else
                    {
                        int startY = rect.Top;
                        foreach (var control in controls)
                        {
                            int controlHeight = size, marginVertical = control.Margin.Vertical, y = startY + control.Margin.Top + use;

                            rectsList.Add(new Rectangle(rect.Left + control.Margin.Left, y - offset, rect.Width - control.Margin.Horizontal, controlHeight));

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
                            int controlWidth = size, marginHorizontal = control.Margin.Horizontal, x = startX - controlWidth - control.Margin.Right - use;

                            rectsList.Add(new Rectangle(x + offset, rect.Top + control.Margin.Top, controlWidth, rect.Height - control.Margin.Vertical));

                            use += controlWidth + gap + marginHorizontal;
                        }
                    }
                    else
                    {
                        int startX = rect.Left;
                        foreach (var control in controls)
                        {
                            int controlWidth = size, marginHorizontal = control.Margin.Horizontal, x = startX + control.Margin.Left + use;

                            rectsList.Add(new Rectangle(x - offset, rect.Top + control.Margin.Top, controlWidth, rect.Height - control.Margin.Vertical));

                            use += controlWidth + gap + marginHorizontal;
                        }
                    }
                }
                return rectsList.ToArray();
            }
            Rectangle[] ConvertToRectsFill(float dpi, List<Control> controls, Rectangle rect)
            {
                int count = controls.Count;
                int usex = 0, usey = 0, gap = 0;
                if (Gap > 0 && count > 1) gap = (int)Math.Round(Gap * dpi);
                var rectsList = new List<Rectangle>(count);
                if (Vertical)
                {
                    int size = (rect.Height - (gap * (count - 1))) / count;
                    if (Reverse)
                    {
                        int startY = rect.Bottom;
                        foreach (var control in controls)
                        {
                            int controlHeight = size - control.Margin.Vertical, y = startY - controlHeight - control.Margin.Bottom - usey;

                            rectsList.Add(new Rectangle(rect.Left + control.Margin.Left, y, rect.Width - control.Margin.Horizontal, controlHeight));

                            usey += size + gap;
                        }
                    }
                    else
                    {
                        int startY = rect.Top;
                        foreach (var control in controls)
                        {
                            int controlHeight = size - control.Margin.Vertical, y = startY + control.Margin.Top + usey;

                            rectsList.Add(new Rectangle(rect.Left + control.Margin.Left, y, rect.Width - control.Margin.Horizontal, controlHeight));

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
                            int controlWidth = size - control.Margin.Horizontal, x = startX - controlWidth - control.Margin.Right - usex;

                            rectsList.Add(new Rectangle(x, rect.Top + control.Margin.Top, controlWidth, rect.Height - control.Margin.Vertical));

                            usex += size + gap;
                        }
                    }
                    else
                    {
                        int startX = rect.Left;
                        foreach (var control in controls)
                        {
                            int controlWidth = size - control.Margin.Horizontal, x = startX + control.Margin.Left + usex;

                            rectsList.Add(new Rectangle(x, rect.Top + control.Margin.Top, controlWidth, rect.Height - control.Margin.Vertical));

                            usex += size + gap;
                        }
                    }
                }
                return rectsList.ToArray();
            }
        }

        #endregion

        #region 内部

        class StackPanelCore : IControl
        {
            public StackPanel Panel;
            public StackPanelCore(StackPanel core)
            {
                Panel = core;
            }

            #region 控件添加和移除

            protected override void OnControlAdded(ControlEventArgs e)
            {
                base.OnControlAdded(e);
                e.Control!.GotFocus += Control_GotFocus;
            }

            protected override void OnLayout(LayoutEventArgs levent) => Panel.IOnLayout(levent);

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
                if (Panel.Vertical)
                {
                    if (Panel.YScroll == null) return;
                    if (Panel.YScroll.Visible) Panel.YScroll.SetValue(ClientRectangle, activeControl.Bounds);
                }
                else
                {
                    if (Panel.XScroll == null) return;
                    if (Panel.XScroll.Visible) Panel.XScroll.SetValue(ClientRectangle, activeControl.Bounds);
                }
            }

            protected override void OnMouseWheel(MouseEventArgs e)
            {
                base.OnMouseWheel(e);
                if (Panel.Vertical)
                {
                    if (Panel.YScroll == null) return;
                    if (Panel.YScroll.Visible) Panel.YScroll.MouseWheelY(e.Delta);
                }
                else
                {
                    if (Panel.XScroll == null) return;
                    if (Panel.XScroll.Visible) Panel.XScroll.MouseWheelX(e.Delta);
                }
            }

            #endregion

            protected override void Dispose(bool disposing)
            {
                foreach (Control c in Controls) c.GotFocus -= Control_GotFocus;
                base.Dispose(disposing);
            }
        }

        #endregion
    }
}