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
// GITEE: https://gitee.com/antdui/AntdUI
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
    public class FlowPanel : IControl
    {
        internal ScrollBar? scroll;

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
                if (autoscroll) scroll = new ScrollBar(this);
                else scroll = null;
                IOnSizeChanged();
            }
        }
        public override Rectangle DisplayRectangle
        {
            get
            {
                var rect = ClientRectangle.DeflateRect(Padding);
                if (scroll != null && scroll.Show) rect.Width -= scroll.SIZE;
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
                IOnSizeChanged();
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
                IOnSizeChanged();
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


        protected override void OnPaint(PaintEventArgs e)
        {
            scroll?.Paint(e.Graphics.High());
            base.OnPaint(e);
        }

        #region 布局

        protected override void OnSizeChanged(EventArgs e)
        {
            var rect = ClientRectangle;
            scroll?.SizeChange(rect);
            base.OnSizeChanged(e);
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
                    var controls = new List<Control>(parent.Controls.Count);
                    foreach (Control it in parent.Controls)
                    {
                        if (it.Visible) controls.Insert(0, it);
                    }
                    if (controls.Count > 0)
                    {
                        int val = HandLayout(parent, controls);
                        if (parent.scroll != null)
                        {
                            bool old_show = parent.scroll.Show;
                            float old_vr = parent.scroll.Max;
                            parent.scroll.SetVrSize(val);
                            if (old_show != parent.scroll.Show || old_vr != parent.scroll.Max) parent.BeginInvoke(new Action(() => { parent.IOnSizeChanged(); }));
                        }
                    }
                }
                return false;
            }

            int HandLayout(FlowPanel parent, List<Control> controls)
            {
                var rect = parent.DisplayRectangle;
                int offset = 0, use_x = 0, use_y = 0, last_len = 0, gap = 0;
                if (parent.scroll != null) offset = (int)parent.scroll.Value;
                if (Gap > 0 && controls.Count > 1) gap = (int)Math.Round(Gap * Config.Dpi);
                var cps = new List<CP>();
                var dir = new List<CP>(controls.Count);
                int oldx = 0;
                foreach (var control in controls)
                {
                    var point = rect.Location;
                    if (use_x + control.Width > rect.Width)
                    {
                        if (cps.Count > 0)
                        {
                            if (Align == TAlignFlow.LeftCenter || Align == TAlignFlow.Center || Align == TAlignFlow.RightCenter)
                            {
                                int x = ((rect.Width - use_x) / 2);
                                oldx = x;
                                foreach (var item in cps)
                                {
                                    item.Point = new Point(item.Point.X + x, item.Point.Y);
                                }
                            }
                            else if (Align == TAlignFlow.Right)
                            {
                                int x = rect.Width - use_x;
                                oldx = x;
                                foreach (var item in cps)
                                {
                                    item.Point = new Point(item.Point.X + x, item.Point.Y);
                                }
                            }
                        }
                        cps.Clear();
                        use_x = 0;
                        use_y += control.Height + gap + control.Margin.Vertical;
                    }
                    point.Offset(control.Margin.Left + use_x, -offset + control.Margin.Top + use_y);
                    var it = new CP(control, point);
                    dir.Add(it);
                    cps.Add(it);
                    use_x += control.Width + gap + control.Margin.Horizontal;

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
                        int x = rect.X + (rect.Width - use_x) - oldx;
                        foreach (var item in cps)
                        {
                            item.Point = new Point(item.Point.X + x, item.Point.Y);
                        }
                    }
                    else if (Align == TAlignFlow.Center)
                    {
                        int x = ((rect.Width - use_x) / 2);
                        foreach (var item in cps)
                        {
                            item.Point = new Point(item.Point.X + x, item.Point.Y);
                        }
                    }
                    else if (Align == TAlignFlow.Right)
                    {
                        int x = rect.Width - use_x;
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
            if (scroll != null && scroll.MouseDown(e.Location)) return;
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (scroll != null && scroll.MouseMove(e.Location)) return;
            base.OnMouseMove(e);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            scroll?.MouseUp();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            scroll?.Leave();
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            scroll?.Leave();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            scroll?.MouseWheel(e.Delta);
            base.OnMouseWheel(e);
        }

        #endregion
    }
}