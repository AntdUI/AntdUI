// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace AntdUI
{
    /// <summary>
    /// FlowPanel 流动布局
    /// </summary>
    [Description("FlowPanel 流动布局")]
    [ToolboxItem(true)]
    [DefaultProperty("Align")]
    [Designer(typeof(IControlDesigner))]
    [ProvideProperty("Index", typeof(Control))]
    public class FlowPanel : ContainerPanel, IExtenderProvider
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
        /// 布局方向
        /// </summary>
        [Description("布局方向"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(TAlignFlow.LeftCenter)]
        public TAlignFlow Align
        {
            get => layoutengine.Align;
            set
            {
                if (layoutengine.Align == value) return;
                layoutengine.Align = value;
                if (IsHandleCreated) IOnSizeChanged();
                OnPropertyChanged(nameof(Align));
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

        protected override void OnDraw(DrawEventArgs e)
        {
            var g = e.Canvas;
            PaintBack(g);
            base.OnDraw(e);
        }

        #region 方法

        public void ScrollControlIntoView(Control activeControl) => Panel.ScrollControlIntoView(activeControl);

        #endregion

        #region Index 排序

        public bool CanExtend(object extendee) => extendee is Control control && control.Parent == this;

        Dictionary<Control, int> Map = new Dictionary<Control, int>();

        /// <summary>
        /// 排序
        /// </summary>
        [DisplayName("Index"), Description("排序"), DefaultValue(-1)]
        public int GetIndex(Control control) => IndexExists(control);

        public void SetIndex(Control control, int index)
        {
            int old = IndexExists(control);
            if (old == index) return;
            if (index > -1)
            {
                if (old == -1) Map.Add(control, index);
                else Map[control] = index;
            }
            else if (old > -1) Map.Remove(control);
            if (IsHandleCreated) IOnSizeChanged();
        }

        int IndexExists(Control control)
        {
            if (Map.TryGetValue(control, out int index)) return index;
            return -1;
        }

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
            }
        }

        private void Scroll_ValueChanged(object? sender, IntEventArgs e) => PerformLayout();

        private FlowPanelCore Panel;
        public YScrollBar? YScroll;

        public FlowPanel()
        {
            Panel = new FlowPanelCore(this)
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

        FlowLayout layoutengine = new FlowLayout();
        public override LayoutEngine LayoutEngine => layoutengine;
        internal class FlowLayout : LayoutEngine
        {
            /// <summary>
            /// 间距
            /// </summary>
            public int Gap { get; set; }
            public TAlignFlow Align { get; set; } = TAlignFlow.LeftCenter;
            public override bool Layout(object container, LayoutEventArgs layoutEventArgs)
            {
                if (container is FlowPanel parent && parent.IsHandleCreated && parent.ControlsBase.Count > 0)
                {
                    if (parent.PauseLayout) return false;
                    var controls = SyncMap(parent);
                    if (controls.Count > 0)
                    {
                        var rect = parent.DisplayRectangle;
                        var rects = ConvertToRects(GetOffset(parent.Panel), parent.Dpi, controls, rect, out int use);
                        HandLayout(controls, rects);
                        if (SetMax(parent.Panel, use, rect)) return false;
                    }
                    else if (parent.Panel.Controls.Count > 0)
                    {
                        while (true)
                        {
                            var rect = parent.DisplayRectangle;
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
            bool Layout(FlowPanelCore panel, Rectangle rect)
            {
                var controls = new List<Control>(panel.Controls.Count);
                foreach (Control it in panel.Controls)
                {
                    if (it.Visible) controls.Insert(0, it);
                }
                if (controls.Count > 0)
                {
                    var rects = ConvertToRects(GetOffset(panel), panel.Dpi, controls, rect, out int use);
                    if (SetMax(panel, use, rect)) return false;
                    HandLayout(controls, rects);
                    return true;
                }
                return true;
            }

            int GetOffset(FlowPanelCore parent)
            {
                if (parent.Panel.YScroll == null) return 0;
                if (parent.Panel.YScroll.Visible) return parent.Panel.YScroll.Value;
                return 0;
            }

            bool SetMax(FlowPanelCore parent, int val, Rectangle rect) => parent.Panel.YScroll?.SetShow(val, rect.Height) ?? false;

            #region 排序

            List<Control> SyncMap(FlowPanel parent)
            {
                int count = parent.ControlsBase.Count, i = count;
                var tmp = new List<IList>(count);
                foreach (Control it in parent.ControlsBase)
                {
                    if (it.Name == "__IN__" || it.Name == "__BARY__" || it.Name == "__BARX__") continue;
                    if (it.Visible)
                    {
                        if (parent.Map.TryGetValue(it, out int index)) tmp.Insert(0, new IList(it, index));
                        else tmp.Insert(0, new IList(it, i));
                        i--;
                    }
                }
                tmp.Sort((a, b) => a.Index.CompareTo(b.Index));
                var controls = new List<Control>(tmp.Count);
                foreach (var it in tmp) controls.Add(it.Control);
                return controls;
            }

            class IList
            {
                public IList(Control control, int index)
                {
                    Control = control;
                    Index = index;
                }

                public Control Control { get; set; }
                public int Index { get; set; }
            }

            #endregion

            Point[] ConvertToRects(int offset, float dpi, List<Control> controls, Rectangle rect, out int use)
            {
                int use_x = 0, use_y = 0, last_len = 0, gap = 0;
                if (Gap > 0 && controls.Count > 1) gap = (int)Math.Round(Gap * dpi);
                var cps = new List<CP>();
                var dir = new List<CP>(controls.Count);
                int oldx = 0, cpsmaxheight = 0;
                foreach (var control in controls)
                {
                    var point = rect.Location;
                    if (use_x + control.Width + control.Margin.Horizontal > rect.Width)
                    {
                        if (cps.Count > 0)
                        {
                            if (Align == TAlignFlow.LeftCenter || Align == TAlignFlow.Center || Align == TAlignFlow.RightCenter)
                            {
                                int x = ((rect.Width - use_x + gap) / 2);
                                oldx = x;
                                foreach (var item in cps) item.Point = new Point(item.Point.X + x, item.Point.Y);
                            }
                            else if (Align == TAlignFlow.Right)
                            {
                                int x = rect.Width - use_x + gap;
                                oldx = x;
                                foreach (var item in cps) item.Point = new Point(item.Point.X + x, item.Point.Y);
                            }
                        }
                        cps.Clear();
                        use_x = 0;
                        use_y += cpsmaxheight + gap + control.Margin.Vertical;
                        cpsmaxheight = 0;
                    }
                    point.Offset(control.Margin.Left + use_x, -offset + control.Margin.Top + use_y);
                    var it = new CP(control, point);
                    dir.Add(it);
                    cps.Add(it);
                    use_x += control.Width + gap + control.Margin.Horizontal;
                    cpsmaxheight = cpsmaxheight > control.Height ? cpsmaxheight : control.Height;
                    last_len = point.Y + offset + control.Height;
                }
                if (cps.Count > 0)
                {
                    if (Align == TAlignFlow.LeftCenter)
                    {
                        if (oldx > 0)
                        {
                            foreach (var item in cps) item.Point = new Point(item.Point.X + oldx, item.Point.Y);
                        }
                    }
                    else if (Align == TAlignFlow.RightCenter)
                    {
                        int x = rect.X + (rect.Width - use_x + gap) - oldx;
                        foreach (var item in cps) item.Point = new Point(item.Point.X + x, item.Point.Y);
                    }
                    else if (Align == TAlignFlow.Center)
                    {
                        int x = ((rect.Width - use_x + gap) / 2);
                        foreach (var item in cps) item.Point = new Point(item.Point.X + x, item.Point.Y);
                    }
                    else if (Align == TAlignFlow.Right)
                    {
                        int x = rect.Width - use_x + gap;
                        foreach (var item in cps) item.Point = new Point(item.Point.X + x, item.Point.Y);
                    }
                }
                cps.Clear();
                use = last_len;
                var r = new List<Point>(dir.Count);
                foreach (var it in dir) r.Add(it.Point);
                return r.ToArray();
            }

            void HandLayout(List<Control> controls, Point[] rects)
            {
                for (int i = 0; i < controls.Count; i++)
                {
                    var control = controls[i];
                    var rect = rects[i];
                    control.Location = rect;
                    if (i >= rects.Length) return;
                }
            }
        }

        class CP
        {
            public CP(Control control, Point point)
            {
                Control = control;
                Point = point;
            }
            public Control Control { get; set; }
            public Point Point { get; set; }
        }

        #endregion

        #region 内部

        class FlowPanelCore : IControl
        {
            public FlowPanel Panel;
            public FlowPanelCore(FlowPanel core)
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
                if (Panel.YScroll == null) return;
                if (Panel.YScroll.Visible) Panel.YScroll.SetValue(ClientRectangle, activeControl.Bounds);
            }

            protected override void OnMouseWheel(MouseEventArgs e)
            {
                base.OnMouseWheel(e);
                if (Panel.YScroll == null) return;
                if (Panel.YScroll.Visible) Panel.YScroll.MouseWheelY(e.Delta);
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