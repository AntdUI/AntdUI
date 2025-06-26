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
                if (ScrollBar != null && ScrollBar.Show) rect.Width -= ScrollBar.SIZE;
                return rect;
            }
        }

        /// <summary>
        /// 布局方向
        /// </summary>
        [Description("布局方向"), Category("外观"), DefaultValue(TAlignFlow.LeftCenter)]
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

        #endregion

        protected override void OnHandleCreated(EventArgs e)
        {
            IOnSizeChanged();
            base.OnHandleCreated(e);
        }

        protected override void OnDraw(DrawEventArgs e)
        {
            var g = e.Canvas;
            PaintBack(g);
            ScrollBar?.Paint(g);
            base.OnDraw(e);
        }

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

        #region 布局

        protected override void OnSizeChanged(EventArgs e)
        {
            var rect = ClientRectangle;
            base.OnSizeChanged(e);
            if (rect.Width == 0 || rect.Height == 0) return;
            ScrollBar?.SizeChange(rect);
        }

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
                if (container is FlowPanel parent && parent.IsHandleCreated && parent.Controls.Count > 0)
                {
                    if (parent.PauseLayout) return false;
                    var controls = SyncMap(parent);

                    if (controls.Count > 0)
                    {
                        int val = HandLayout(parent, controls);
                        if (parent.ScrollBar != null)
                        {
                            bool old_show = parent.ScrollBar.Show;
                            float old_vr = parent.ScrollBar.Max;
                            parent.ScrollBar.SetVrSize(val);
                            if (old_show != parent.ScrollBar.Show || old_vr != parent.ScrollBar.Max) parent.BeginInvoke(parent.IOnSizeChanged);
                        }
                    }
                }
                return false;
            }

            #region 排序

            List<Control> SyncMap(FlowPanel parent)
            {
                int count = parent.Controls.Count, i = count;
                var tmp = new List<IList>(count);
                foreach (Control it in parent.Controls)
                {
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

            int HandLayout(FlowPanel parent, List<Control> controls)
            {
                var rect = parent.DisplayRectangle;
                int offset = 0, use_x = 0, use_y = 0, last_len = 0, gap = 0;
                if (parent.ScrollBar != null) offset = parent.ScrollBar.Value;
                if (Gap > 0 && controls.Count > 1) gap = (int)Math.Round(Gap * Config.Dpi);
                var cps = new List<CP>();
                var dir = new List<CP>(controls.Count);
                int oldx = 0;
                int cpsmaxheight = 0;
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
                                foreach (var item in cps)
                                {
                                    item.Point = new Point(item.Point.X + x, item.Point.Y);
                                }
                            }
                            else if (Align == TAlignFlow.Right)
                            {
                                int x = rect.Width - use_x + gap;
                                oldx = x;
                                foreach (var item in cps)
                                {
                                    item.Point = new Point(item.Point.X + x, item.Point.Y);
                                }
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
                            foreach (var item in cps)
                            {
                                item.Point = new Point(item.Point.X + oldx, item.Point.Y);
                            }
                        }
                    }
                    else if (Align == TAlignFlow.RightCenter)
                    {
                        int x = rect.X + (rect.Width - use_x + gap) - oldx;
                        foreach (var item in cps)
                        {
                            item.Point = new Point(item.Point.X + x, item.Point.Y);
                        }
                    }
                    else if (Align == TAlignFlow.Center)
                    {
                        int x = ((rect.Width - use_x + gap) / 2);
                        foreach (var item in cps)
                        {
                            item.Point = new Point(item.Point.X + x, item.Point.Y);
                        }
                    }
                    else if (Align == TAlignFlow.Right)
                    {
                        int x = rect.Width - use_x + gap;
                        foreach (var item in cps)
                        {
                            item.Point = new Point(item.Point.X + x, item.Point.Y);
                        }
                    }
                }
                cps.Clear();
                parent.SuspendLayout();
                foreach (var it in dir)
                {
                    it.Control.Location = it.Point;
                }
                parent.ResumeLayout();
                return last_len;
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

        #region 鼠标

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (ScrollBar != null && ScrollBar.MouseDown(e.Location)) return;
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (ScrollBar != null && ScrollBar.MouseMove(e.Location)) return;
            base.OnMouseMove(e);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            ScrollBar?.MouseUp();
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
                if (controlRect.Top < clientRect.Top) ScrollBar.ValueY = Math.Max(0, ScrollBar.ValueY + controlRect.Top - clientRect.Top);
                else if (controlRect.Bottom > clientRect.Bottom) ScrollBar.ValueY = Math.Min(ScrollBar.MaxY, ScrollBar.ValueY + controlRect.Bottom - clientRect.Bottom);
            }
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            ScrollBar?.Dispose();
            base.Dispose(disposing);
        }
    }
}