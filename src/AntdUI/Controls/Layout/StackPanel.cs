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
    public class StackPanel : IControl
    {
        internal IScroll? scroll;

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
                if (autoscroll)
                {
                    var action = new Action(() =>
                    {
                        OSizeChanged();
                    });
                    scroll = Vertical ? new ScrollY(this) { Change = action } : new ScrollX(this) { Change = action };
                }
                else scroll = null;
                OSizeChanged();
            }
        }
        public override Rectangle DisplayRectangle
        {
            get
            {
                var rect = ClientRectangle.DeflateRect(Padding);
                if (scroll != null && scroll.Show)
                {
                    if (scroll is ScrollY) rect.Width -= scroll.SIZE;
                    else rect.Height -= scroll.SIZE;
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
                if (autoscroll)
                {
                    var action = new Action(() =>
                    {
                        OSizeChanged();
                    });
                    scroll = Vertical ? new ScrollY(this) { Change = action } : new ScrollX(this) { Change = action };
                }
                else scroll = null;
                OSizeChanged();
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
                OSizeChanged();
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
                OSizeChanged();
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

        internal void OSizeChanged()
        {
            OnSizeChanged(EventArgs.Empty);
        }

        StackLayout layoutengine = new StackLayout();
        public override LayoutEngine LayoutEngine => layoutengine;
        internal class StackLayout : LayoutEngine
        {
            /// <summary>
            /// 是否垂直方向
            /// </summary>
            public bool Vertical { get; set; } = false;
            /// <summary>
            /// 内容大小
            /// </summary>
            public string? ItemSize { get; set; }

            /// <summary>
            /// 间距
            /// </summary>
            public int Gap { get; set; }

            public override bool Layout(object container, LayoutEventArgs layoutEventArgs)
            {
                if (container is StackPanel parent)
                {
                    var rect = parent.DisplayRectangle;
                    int val = 0;
                    if (string.IsNullOrEmpty(ItemSize)) val = HandLayout(parent, rect);
                    else
                    {
                        if (ItemSize.EndsWith("%") && float.TryParse(ItemSize.TrimEnd('%'), out var f)) val = HandLayout(parent, rect, (int)Math.Round((Vertical ? rect.Height : rect.Width) * (f / 100F)));
                        else if (int.TryParse(ItemSize, out var i)) val = HandLayout(parent, rect, (int)Math.Round(i * Config.Dpi));
                        else val = HandLayoutFill(parent, rect);
                    }
                    if (parent.scroll != null)
                    {
                        bool old = parent.scroll.Show;
                        parent.scroll.SetVrSize(val);
                        if (old != parent.scroll.Show) parent.Invoke(new Action(() => { parent.OSizeChanged(); }));
                    }
                }
                return false;
            }

            int HandLayout(StackPanel parent, Rectangle rect)
            {
                int count = parent.Controls.Count;
                if (count > 0)
                {
                    int offset = 0, use = 0, last_len = 0, gap = 0;
                    if (parent.scroll != null) offset = (int)parent.scroll.Value;
                    if (Gap > 0 && count > 1) gap = (int)Math.Round(Gap * Config.Dpi);
                    if (Vertical)
                    {
                        foreach (Control control in parent.Controls)
                        {
                            if (!control.Visible) continue;

                            var point = rect.Location;
                            point.Offset(control.Margin.Left, -offset + control.Margin.Top + use);
                            control.Location = point;
                            control.Width = rect.Width - control.Margin.Horizontal;

                            use += control.Height + gap + control.Margin.Vertical;
                            last_len = point.Y + offset + control.Height;
                        }
                    }
                    else
                    {
                        foreach (Control control in parent.Controls)
                        {
                            if (!control.Visible) continue;

                            Point point = rect.Location;
                            point.Offset(-offset + control.Margin.Left + use, control.Margin.Top);
                            control.Location = point;
                            control.Height = rect.Height - control.Margin.Vertical;

                            use += control.Width + gap + control.Margin.Horizontal;
                            last_len = control.Left + offset + control.Width;
                        }
                    }
                    return last_len;
                }
                return 0;
            }
            int HandLayout(StackPanel parent, Rectangle rect, int size)
            {
                int count = parent.Controls.Count;
                if (count > 0)
                {
                    int offset = 0, use = 0, last_len = 0, gap = 0;
                    if (parent.scroll != null) offset = (int)parent.scroll.Value;
                    if (Gap > 0 && count > 1) gap = (int)Math.Round(Gap * Config.Dpi);
                    if (Vertical)
                    {
                        foreach (Control control in parent.Controls)
                        {
                            if (!control.Visible) continue;

                            var point = rect.Location;
                            point.Offset(control.Margin.Left, -offset + control.Margin.Top + use);
                            control.Location = point;
                            control.Size = new Size(rect.Width - control.Margin.Horizontal, size);

                            use += control.Height + gap + control.Margin.Vertical;
                            last_len = point.Y + offset + control.Height;
                        }
                    }
                    else
                    {
                        foreach (Control control in parent.Controls)
                        {
                            if (!control.Visible) continue;

                            Point point = rect.Location;
                            point.Offset(-offset + control.Margin.Left + use, control.Margin.Top);
                            control.Location = point;
                            control.Size = new Size(size, rect.Height - control.Margin.Vertical);

                            use += control.Width + gap + control.Margin.Horizontal;
                            last_len = control.Left + offset + control.Width;
                        }
                    }
                    return last_len;
                }
                return 0;
            }
            int HandLayoutFill(StackPanel parent, Rectangle rect)
            {
                int count = parent.Controls.Count;
                if (count > 0)
                {
                    int usex = 0, usey = 0, gap = 0;
                    if (Gap > 0 && count > 1) gap = (int)Math.Round(Gap * Config.Dpi);
                    if (Vertical)
                    {
                        int size = (rect.Height - (gap * (count - 1))) / count;
                        foreach (Control control in parent.Controls)
                        {
                            if (!control.Visible) continue;

                            Point point = rect.Location;
                            point.Offset(control.Margin.Left + usex, control.Margin.Top + usey);
                            control.Location = point;
                            control.Size = new Size(rect.Width - control.Margin.Horizontal, size - control.Margin.Vertical);

                            usey += size + gap;
                        }
                    }
                    else
                    {
                        int size = (rect.Width - (gap * (count - 1))) / count;
                        foreach (Control control in parent.Controls)
                        {
                            if (!control.Visible) continue;

                            Point point = rect.Location;
                            point.Offset(control.Margin.Left + usex, control.Margin.Top + usey);
                            control.Location = point;
                            control.Size = new Size(size - control.Margin.Horizontal, rect.Height - control.Margin.Vertical);

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
            scroll?.MouseUp(e.Location);
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
            if (scroll is ScrollY scrollY) scrollY.MouseWheel(e.Delta);
            base.OnMouseWheel(e);
        }

        #endregion
    }
}