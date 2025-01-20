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
// GITEE: https://gitee.com/antdui/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace AntdUI
{
    /// <summary>
    /// Collapse 折叠面板
    /// </summary>
    /// <remarks>可以折叠/展开的内容区域。</remarks>
    [Description("Collapse 折叠面板")]
    [ToolboxItem(true)]
    [DefaultProperty("Items")]
    [DefaultEvent("ExpandChanged")]
    [Designer(typeof(CollapseDesigner))]
    public class Collapse : IControl
    {
        #region 属性

        Color? fore;
        /// <summary>
        /// 文字颜色
        /// </summary>
        [Description("文字颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public new Color? ForeColor
        {
            get => fore;
            set
            {
                if (fore == value) return;
                fore = value;
                Invalidate();
            }
        }

        Color? headerBg = null;
        /// <summary>
        /// 折叠面板头部背景
        /// </summary>
        [Description("折叠面板头部背景"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? HeaderBg
        {
            get => headerBg;
            set
            {
                if (headerBg == value) return;
                headerBg = value;
                Invalidate();
            }
        }

        Size headerPadding { get; set; } = new Size(16, 12);
        /// <summary>
        /// 折叠面板头部内边距
        /// </summary>
        [Description("折叠面板头部内边距"), Category("外观"), DefaultValue(typeof(Size), "16, 12")]
        public Size HeaderPadding
        {
            get => headerPadding;
            set
            {
                if (headerPadding == value) return;
                headerPadding = value;
                LoadLayout();
            }
        }

        Size contentPadding { get; set; } = new Size(16, 16);
        /// <summary>
        /// 折叠面板内容内边距
        /// </summary>
        [Description("折叠面板内容内边距"), Category("外观"), DefaultValue(typeof(Size), "16, 16")]
        public Size ContentPadding
        {
            get => contentPadding;
            set
            {
                if (contentPadding == value) return;
                contentPadding = value;
                LoadLayout();
            }
        }

        #region 边框

        float borderWidth = 1F;
        /// <summary>
        /// 边框宽度
        /// </summary>
        [Description("边框宽度"), Category("边框"), DefaultValue(1F)]
        public float BorderWidth
        {
            get => borderWidth;
            set
            {
                if (borderWidth == value) return;
                borderWidth = value;
                Invalidate();
            }
        }

        Color? borderColor = null;
        /// <summary>
        /// 边框颜色
        /// </summary>
        [Description("边框颜色"), Category("边框"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? BorderColor
        {
            get => borderColor;
            set
            {
                if (borderColor == value) return;
                borderColor = value;
                Invalidate();
            }
        }

        #endregion

        int radius = 6;
        /// <summary>
        /// 圆角
        /// </summary>
        [Description("圆角"), Category("外观"), DefaultValue(6)]
        public int Radius
        {
            get => radius;
            set
            {
                if (radius == value) return;
                radius = value;
                Invalidate();
            }
        }

        int _gap = 0;
        /// <summary>
        /// 间距
        /// </summary>
        [Description("间距"), Category("外观"), DefaultValue(0)]
        public int Gap
        {
            get => _gap;
            set
            {
                if (_gap == value) return;
                _gap = value;
                LoadLayout();
            }
        }

        /// <summary>
        /// 只保持一个展开
        /// </summary>
        [Description("只保持一个展开"), Category("外观"), DefaultValue(false)]
        public bool Unique { get; set; }

        public override Rectangle DisplayRectangle => ClientRectangle.PaddingRect(Margin, Padding);

        #region 数据

        CollapseItemCollection? items;
        /// <summary>
        /// 获取列表中所有列表项的集合
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("集合"), Category("数据"), DefaultValue(null)]
        public CollapseItemCollection Items
        {
            get
            {
                items ??= new CollapseItemCollection(this);
                return items;
            }
            set => items = value.BindData(this);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new ControlCollection Controls => base.Controls;

        #endregion

        #endregion

        #region 布局

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            LoadLayout(false);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            LoadLayout(false);
            base.OnSizeChanged(e);
        }

        internal void UniqueOne(CollapseItem item)
        {
            if (Unique)
            {
                if (items == null) return;
                foreach (var it in items)
                {
                    if (it == item) continue;
                    it.Expand = false;
                }
            }
        }
        internal void LoadLayout(bool r = true)
        {
            if (IsHandleCreated)
            {
                if (items == null) return;
                var rect = ClientRectangle;
                if (rect.Width > 0 && rect.Height > 0)
                {
                    var rect_t = rect.DeflateRect(Margin);
                    LoadLayout(rect_t, items);
                    if (r) Invalidate();
                }
            }
        }

        internal void LoadLayout(Rectangle rect, CollapseItemCollection items)
        {
            var size = Helper.GDI(g => g.MeasureString(Config.NullText, Font));
            int gap = (int)(_gap * Config.Dpi), gap_x = (int)(HeaderPadding.Width * Config.Dpi), gap_y = (int)(HeaderPadding.Height * Config.Dpi),
                content_x = (int)(ContentPadding.Width * Config.Dpi), content_y = (int)(ContentPadding.Height * Config.Dpi), use_x = 0;
            int title_height = size.Height + gap_y * 2;
            int full_count = 0, useh = 0, full_h = 0;
            foreach (var it in items)
            {
                if (it.Full) full_count++;
            }
            if (full_count > 0)
            {
                foreach (var it in items)
                {
                    if (!it.Full)
                    {
                        useh += title_height + gap;
                        if (it.ExpandThread) useh += (int)((content_y * 2 + it.Height) * it.ExpandProg);
                        else if (it.Expand) useh += content_y * 2 + it.Height;
                    }
                }
                full_h = (rect.Height - useh) / full_count;
            }
            foreach (var it in items)
            {
                int y = rect.Y + use_x;
                it.RectTitle = new Rectangle(rect.X, y, rect.Width, title_height);
                it.RectArrow = new Rectangle(rect.X + gap_x, y + gap_y, size.Height, size.Height);
                it.RectText = new Rectangle(rect.X + gap_x + size.Height + gap_y / 2, y + gap_y, rect.Width - (gap_x * 2 - size.Height - gap_y / 2), size.Height);

                Rectangle Rect;
                if (it.Full)
                {
                    if (it.ExpandThread) it.Rect = Rect = new Rectangle(rect.X, y, rect.Width, title_height + (int)((full_h - title_height) * it.ExpandProg));
                    else if (it.Expand)
                    {
                        it.Rect = Rect = new Rectangle(rect.X, y, rect.Width, full_h);
                        it.RectCcntrol = new Rectangle(rect.X + content_x, y + title_height + content_y, rect.Width - content_x * 2, full_h - (title_height + content_y * 2));
                        it.SetSize();
                    }
                    else it.Rect = Rect = it.RectTitle;
                }
                else
                {
                    if (it.ExpandThread) it.Rect = Rect = new Rectangle(rect.X, y, rect.Width, title_height + (int)((content_y * 2 + it.Height) * it.ExpandProg));
                    else if (it.Expand)
                    {
                        it.RectCcntrol = new Rectangle(rect.X + content_x, y + title_height + content_y, rect.Width - content_x * 2, it.Height);
                        it.Rect = Rect = new Rectangle(rect.X, y, rect.Width, title_height + content_y * 2 + it.Height);
                        it.SetSize();
                    }
                    else it.Rect = Rect = it.RectTitle;
                }
                use_x += Rect.Height + gap;
            }
        }

        #endregion

        #region 事件

        /// <summary>
        /// Expand 属性值更改时发生
        /// </summary>
        [Description("Expand 属性值更改时发生"), Category("行为")]
        public event CollapseExpandEventHandler? ExpandChanged = null;

        internal void OnExpandChanged(CollapseItem value, bool expand) => ExpandChanged?.Invoke(this, new CollapseExpandEventArgs(value, expand));

        #endregion

        #region 渲染

        StringFormat s_l = Helper.SF_ALL(lr: StringAlignment.Near);
        protected override void OnPaint(PaintEventArgs e)
        {
            if (items == null || items.Count == 0)
            {
                base.OnPaint(e);
                return;
            }
            var g = e.Graphics.High();
            float r = radius * Config.Dpi;
            using (var forebrush = new SolidBrush(fore ?? Colour.Text.Get("Collapse")))
            using (var brush = new SolidBrush(headerBg ?? Colour.FillQuaternary.Get("Collapse")))
            {
                if (borderWidth > 0)
                {
                    using (var pen = new Pen(borderColor ?? Colour.BorderColor.Get("Collapse"), borderWidth * Config.Dpi))
                    using (var pen_arr = new Pen(forebrush.Color, 1.2F * Config.Dpi))
                    {
                        pen.StartCap = pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                        if (items.Count == 1 || _gap > 0)
                        {
                            foreach (var item in items)
                            {
                                if (item.Expand)
                                {
                                    using (var path = item.Rect.RoundPath(r))
                                    {
                                        g.Draw(pen, path);
                                    }
                                    using (var path = item.RectTitle.RoundPath(r, true, true, false, false))
                                    {
                                        g.Fill(brush, path);
                                        g.Draw(pen, path);
                                    }
                                }
                                else
                                {
                                    using (var path = item.RectTitle.RoundPath(r))
                                    {
                                        g.Fill(brush, path);
                                        g.Draw(pen, path);
                                    }
                                }
                                PaintItem(g, item, forebrush, pen_arr);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < items.Count; i++)
                            {
                                var item = items[i];
                                if (i == 0)
                                {
                                    if (item.Expand)
                                    {
                                        using (var path = item.Rect.RoundPath(r, true, true, false, false))
                                        {
                                            g.Draw(pen, path);
                                        }
                                        using (var path = item.RectTitle.RoundPath(r, true, true, false, false))
                                        {
                                            g.Fill(brush, path);
                                            g.Draw(pen, path);
                                        }
                                    }
                                    else
                                    {
                                        using (var path = item.RectTitle.RoundPath(r, true, true, false, false))
                                        {
                                            g.Fill(brush, path);
                                            g.Draw(pen, path);
                                        }
                                    }
                                    PaintItem(g, item, forebrush, pen_arr);
                                }
                                else if (i == items.Count - 1)
                                {
                                    if (item.Expand)
                                    {
                                        using (var path = item.Rect.RoundPath(r, false, false, true, true))
                                        {
                                            g.Draw(pen, path);
                                        }
                                        g.Fill(brush, item.RectTitle);
                                        g.Draw(pen, item.RectTitle);
                                    }
                                    else
                                    {
                                        using (var path = item.RectTitle.RoundPath(r, false, false, true, true))
                                        {
                                            g.Fill(brush, path);
                                            g.Draw(pen, path);
                                        }
                                    }
                                    PaintItem(g, item, forebrush, pen_arr);
                                }
                                else
                                {
                                    if (item.Expand)
                                    {
                                        g.Draw(pen, item.Rect);
                                        g.Fill(brush, item.RectTitle);
                                        g.Draw(pen, item.RectTitle);
                                    }
                                    else
                                    {
                                        using (var path = item.RectTitle.RoundPath(r, false, false, true, true))
                                        {
                                            g.Fill(brush, item.RectTitle);
                                            g.Draw(pen, item.RectTitle);
                                        }
                                    }
                                    PaintItem(g, item, forebrush, pen_arr);
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (items.Count == 1 || _gap > 0)
                    {
                        foreach (var item in items)
                        {
                            if (item.Expand)
                            {
                                using (var path = item.RectTitle.RoundPath(r, true, true, false, false))
                                {
                                    g.Fill(brush, path);
                                }
                            }
                            else
                            {
                                using (var path = item.RectTitle.RoundPath(r))
                                {
                                    g.Fill(brush, path);
                                }
                            }
                            PaintItem(g, item, forebrush);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < items.Count; i++)
                        {
                            var item = items[i];
                            if (i == 0)
                            {
                                if (item.Expand)
                                {
                                    using (var path = item.RectTitle.RoundPath(r, true, true, false, false))
                                    {
                                        g.Fill(brush, path);
                                    }
                                }
                                else
                                {
                                    using (var path = item.RectTitle.RoundPath(r, true, true, false, false))
                                    {
                                        g.Fill(brush, path);
                                    }
                                }
                                PaintItem(g, item, forebrush);
                            }
                            else if (i == items.Count - 1)
                            {
                                if (item.Expand) g.Fill(brush, item.RectTitle);
                                else
                                {
                                    using (var path = item.RectTitle.RoundPath(r, false, false, true, true))
                                    {
                                        g.Fill(brush, path);
                                    }
                                }
                                PaintItem(g, item, forebrush);
                            }
                            else
                            {
                                if (item.Expand) g.Fill(brush, item.RectTitle);
                                else
                                {
                                    using (var path = item.RectTitle.RoundPath(r, false, false, true, true))
                                    {
                                        g.Fill(brush, item.RectTitle);
                                    }
                                }
                                PaintItem(g, item, forebrush);
                            }
                        }
                    }
                }
            }
            this.PaintBadge(g);
            base.OnPaint(e);
        }

        void PaintItem(Canvas g, CollapseItem item, SolidBrush fore, Pen pen_arr)
        {
            if (item.ExpandThread) PaintArrow(g, item, pen_arr, -90 + (90F * item.ExpandProg));
            else if (item.Expand) g.DrawLines(pen_arr, item.RectArrow.TriangleLines(-1, .56F));
            else PaintArrow(g, item, pen_arr, -90F);

            g.String(item.Text, Font, fore, item.RectText, s_l);
        }

        void PaintItem(Canvas g, CollapseItem item, SolidBrush fore)
        {
            if (item.ExpandThread) PaintArrow(g, item, fore, -90 + (90F * item.ExpandProg));
            else if (item.Expand) g.FillPolygon(fore, item.RectArrow.TriangleLines(-1, .56F));
            else PaintArrow(g, item, fore, -90F);

            g.String(item.Text, Font, fore, item.RectText, s_l);
        }

        void PaintArrow(Canvas g, CollapseItem item, Pen pen, float rotate)
        {
            var rect_arr = item.RectArrow;
            int size_arrow = rect_arr.Width / 2;
            g.TranslateTransform(rect_arr.X + size_arrow, rect_arr.Y + size_arrow);
            g.RotateTransform(rotate);
            g.DrawLines(pen, new Rectangle(-size_arrow, -size_arrow, rect_arr.Width, rect_arr.Height).TriangleLines(-1, .56F));
            g.ResetTransform();
        }
        void PaintArrow(Canvas g, CollapseItem item, SolidBrush brush, float rotate)
        {
            var rect_arr = item.RectArrow;
            int size_arrow = rect_arr.Width / 2;
            g.TranslateTransform(rect_arr.X + size_arrow, rect_arr.Y + size_arrow);
            g.RotateTransform(rotate);
            g.FillPolygon(brush, new Rectangle(-size_arrow, -size_arrow, rect_arr.Width, rect_arr.Height).TriangleLines(-1, .56F));
            g.ResetTransform();
        }

        #endregion

        #region 鼠标

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (items == null || items.Count == 0) return;
            foreach (var item in items)
            {
                if (item.Contains(e.X, e.Y))
                {
                    item.MDown = true;
                    return;
                }
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (items == null || items.Count == 0) return;
            foreach (var item in items)
            {
                if (item.MDown)
                {
                    if (item.Contains(e.X, e.Y)) item.Expand = !item.Expand;
                    item.MDown = false;
                    return;
                }
            }
            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (items == null || items.Count == 0) return;
            foreach (var item in items)
            {
                if (item.Contains(e.X, e.Y))
                {
                    SetCursor(true);
                    return;
                }
            }
            SetCursor(false);
            base.OnMouseMove(e);
        }

        #endregion

        #region 设计器

        internal class CollapseDesigner : ParentControlDesigner
        {
            public new Collapse Control => (Collapse)base.Control;

            protected override bool GetHitTest(Point point)
            {
                var point_ = Control.PointToClient(point);
                foreach (var tab in Control.Items)
                {
                    if (tab.Contains(point_.X, point_.Y)) return true;
                }
                return base.GetHitTest(point);
            }
        }

        #endregion
    }

    public class CollapseItemCollection : iCollection<CollapseItem>
    {
        public CollapseItemCollection(Collapse it)
        {
            BindData(it);
        }

        internal CollapseItemCollection BindData(Collapse it)
        {
            action = render =>
            {
                if (render) it.LoadLayout(false);
                it.Invalidate();
            };
            action_add = item =>
            {
                item.PARENT = it;
                item.Location = new Point(-item.Width, -item.Height);
                it.Controls.Add(item);
            };
            action_del = (item, index) =>
            {
                it.Controls.Remove(item);
            };
            return this;
        }
    }

    [ToolboxItem(false)]
    [Designer(typeof(IControlDesigner))]
    public class CollapseItem : ScrollableControl
    {
        public CollapseItem()
        {
            SetStyle(
               ControlStyles.AllPaintingInWmPaint |
               ControlStyles.OptimizedDoubleBuffer |
               ControlStyles.ResizeRedraw |
               ControlStyles.DoubleBuffer |
               ControlStyles.SupportsTransparentBackColor |
               ControlStyles.ContainerControl |
               ControlStyles.UserPaint, true);
            UpdateStyles();
        }

        protected override Size DefaultSize => new Size(100, 60);

        #region 属性

        #region 展开

        ITask? ThreadExpand = null;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Category("外观"), Description("展开进度"), DefaultValue(0F)]
        internal float ExpandProg { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Category("外观"), Description("展开状态"), DefaultValue(false)]
        internal bool ExpandThread { get; set; }

        bool expand = false;
        /// <summary>
        /// 是否展开
        /// </summary>
        [Category("外观"), Description("是否展开"), DefaultValue(false)]
        public bool Expand
        {
            get => expand;
            set
            {
                if (expand == value) return;
                expand = value;
                PARENT?.OnExpandChanged(this, expand);
                if (value) PARENT?.UniqueOne(this);
                if (PARENT != null && PARENT.IsHandleCreated && Config.Animation)
                {
                    Location = new Point(-Width, -Height);
                    ThreadExpand?.Dispose();
                    float oldval = -1;
                    if (ThreadExpand?.Tag is float oldv) oldval = oldv;
                    ExpandThread = true;
                    var t = Animation.TotalFrames(10, 200);
                    if (value)
                    {
                        ThreadExpand = new ITask(false, 10, t, oldval, AnimationType.Ball, (i, val) =>
                        {
                            ExpandProg = val;
                            PARENT.LoadLayout();
                        }, () =>
                        {
                            ExpandProg = 1F;
                            ExpandThread = false;
                            PARENT.LoadLayout();
                        });
                    }
                    else
                    {
                        ThreadExpand = new ITask(true, 10, t, oldval, AnimationType.Ball, (i, val) =>
                        {
                            ExpandProg = val;
                            PARENT.LoadLayout();
                        }, () =>
                        {
                            ExpandProg = 1F;
                            ExpandThread = false;
                            PARENT.LoadLayout();
                        });
                    }
                }
                else
                {
                    PARENT?.LoadLayout();
                    if (!value) Location = new Point(-Width, -Height);
                }
            }
        }

        bool full = false;
        /// <summary>
        /// 是否铺满剩下空间
        /// </summary>
        [Category("外观"), Description("是否铺满剩下空间"), DefaultValue(false)]
        public bool Full
        {
            get => full;
            set
            {
                if (full == value) return;
                full = value;
                PARENT?.LoadLayout();
            }
        }

        #endregion

        #region 国际化

        string text = "";
        /// <summary>
        /// 文本
        /// </summary>
        [Description("文本"), Category("外观"), DefaultValue("")]
        public override string Text
        {
            get => this.GetLangIN(LocalizationText, text);
            set
            {
                if (text == value) return;
                base.Text = text = value;
            }
        }

        [Description("文本"), Category("国际化"), DefaultValue(null)]
        public string? LocalizationText { get; set; }

        #endregion

        #endregion

        #region 坐标

        internal bool MDown = false;
        internal Rectangle Rect = new Rectangle(-10, -10, 0, 0);
        internal Rectangle RectArrow, RectCcntrol, RectTitle, RectText;
        internal bool Contains(int x, int y) => RectTitle.Contains(x, y);

        #endregion

        #region 变更

        internal Collapse? PARENT;
        protected override void OnTextChanged(EventArgs e)
        {
            PARENT?.LoadLayout();
            base.OnTextChanged(e);
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            PARENT?.LoadLayout();
            base.OnVisibleChanged(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if (canset) PARENT?.LoadLayout();
            base.OnSizeChanged(e);
        }

        bool canset = true;
        public void SetSize()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(SetSize));
                return;
            }
            canset = false;
            Size = RectCcntrol.Size;
            Location = RectCcntrol.Location;
            canset = true;
        }

        #endregion

        public override string ToString() => Text;
    }
}