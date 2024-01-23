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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// Menu 导航菜单
    /// </summary>
    /// <remarks>为页面和功能提供导航的菜单列表。</remarks>
    [Description("Menu 导航菜单")]
    [ToolboxItem(true)]
    [DefaultEvent("SelectChanged")]
    public class Menu : IControl
    {
        #region 属性

        /// <summary>
        /// 悬停背景颜色
        /// </summary>
        [Description("悬停背景颜色"), Category("外观"), DefaultValue(null)]
        public Color? BackHover { get; set; }

        /// <summary>
        /// 激活背景颜色
        /// </summary>
        [Description("激活背景颜色"), Category("外观"), DefaultValue(null)]
        public Color? BackActive { get; set; }

        Color? fore;
        /// <summary>
        /// 文字颜色
        /// </summary>
        [Description("文字颜色"), Category("外观"), DefaultValue(null)]
        public Color? Fore
        {
            get => fore;
            set
            {
                if (fore == value) return;
                fore = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 激活字体颜色
        /// </summary>
        [Description("激活字体颜色"), Category("外观"), DefaultValue(null)]
        public Color? ForeActive { get; set; }

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

        bool round = false;
        /// <summary>
        /// 圆角样式
        /// </summary>
        [Description("圆角样式"), Category("外观"), DefaultValue(false)]
        public bool Round
        {
            get => round;
            set
            {
                if (round == value) return;
                round = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 常规缩进
        /// </summary>
        [Description("常规缩进"), Category("外观"), DefaultValue(false)]
        public bool Indent { get; set; } = false;

        /// <summary>
        /// 显示子菜单背景
        /// </summary>
        [Description("显示子菜单背景"), Category("外观"), DefaultValue(false)]
        public bool ShowSubBack { get; set; } = false;

        #region 集合操作

        public void SelectIndex(int i1)
        {
            if (items == null || items.Count == 0) return;
            var it1 = items[i1];
            if (it1 != null)
            {
                IUSelect();
                it1.Select = true;
                OnSelectIndexChanged(it1);
                Invalidate();
            }
        }
        public void SelectIndex(int i1, int i2)
        {
            if (items == null || items.Count == 0) return;
            var it1 = items[i1];
            if (it1 != null)
            {
                if (it1.Sub == null || it1.Sub.Count == 0) return;
                var it2 = it1.Sub[i2];
                if (it2 != null)
                {
                    IUSelect();
                    it1.Select = it2.Select = true;
                    OnSelectIndexChanged(it2);
                    Invalidate();
                }
            }
        }
        public void SelectIndex(int i1, int i2, int i3)
        {
            if (items == null || items.Count == 0) return;
            var it1 = items[i1];
            if (it1 != null)
            {
                if (it1.Sub == null || it1.Sub.Count == 0) return;
                var it2 = it1.Sub[i2];
                if (it2 != null)
                {
                    if (it2.Sub == null || it2.Sub.Count == 0) return;
                    var it3 = it2.Sub[i3];
                    if (it3 != null)
                    {
                        IUSelect();
                        it1.Select = it2.Select = it3.Select = true;
                        OnSelectIndexChanged(it3);
                        Invalidate();
                    }
                }
            }
        }

        #region 事件

        public delegate void SelectEventHandler(object sender, MenuItem item);
        /// <summary>
        /// SelectIndex 属性值更改时发生
        /// </summary>
        [Description("SelectIndex 属性值更改时发生"), Category("行为")]
        public event SelectEventHandler? SelectIndexChanged = null;

        /// <summary>
        /// Select 属性值更改时发生
        /// </summary>
        [Description("Select 属性值更改时发生"), Category("行为")]
        public event SelectEventHandler? SelectChanged = null;

        internal void OnSelectIndexChanged(MenuItem item)
        {
            SelectIndexChanged?.Invoke(this, item);
            SelectChanged?.Invoke(this, item);
        }

        #endregion

        #endregion

        MenuItemCollection? items;
        /// <summary>
        /// 菜单集合
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("菜单集合"), Category("数据")]
        public MenuItemCollection Items
        {
            get
            {
                items ??= new MenuItemCollection(this);
                return items;
            }
            set => items = value.BindData(this);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            var rect = ChangeList();
            scrollY.SizeChange(rect);
            base.OnSizeChanged(e);
        }

        bool pauseLayout = false;
        [Description("暂停布局"), Category("行为"), DefaultValue(false)]
        public bool PauseLayout
        {
            get => pauseLayout;
            set
            {
                if (pauseLayout == value) return;
                pauseLayout = value;
                if (!value)
                {
                    ChangeList();
                    Invalidate();
                }
            }
        }

        internal Rectangle ChangeList()
        {
            var rect = ClientRectangle;
            if (pauseLayout || items == null || items.Count == 0) return rect;
            if (rect.Width == 0 || rect.Height == 0) return rect;

            float y = 0;
            using (var bmp = new Bitmap(1, 1))
            {
                using (var g = Graphics.FromImage(bmp))
                {
                    var size = g.MeasureString(Config.NullText, Font);
                    float icon_size = size.Height * 1.2F, gap = icon_size * 0.5F;
                    int height = (int)Math.Ceiling(size.Height + gap * 2);
                    int gapI = (int)(gap / 2);
                    ChangeList(rect, Items, ref y, height, icon_size, gap, gapI, 0);
                }
            }
            scrollY.SetVrSize(y, rect.Height);
            return rect;
        }

        void ChangeList(Rectangle rect, MenuItemCollection items, ref float y, int height, float icon_size, float gap, int gapI, int depth)
        {
            foreach (MenuItem it in items)
            {
                it.PARENT = this;
                it.SetRect(depth, Indent, new RectangleF(0, y, rect.Width, height), icon_size, gap);
                if (it.Visible)
                {
                    y += height + gapI;
                    if (it.CanExpand)
                    {
                        float y_item = y;

                        ChangeList(rect, it.Sub, ref y, height, icon_size, gap, gapI, depth + 1);

                        it.SubY = y_item - gapI / 2;
                        it.SubHeight = y - y_item;

                        if ((it.Expand || it.ExpandThread) && it.ExpandProg > 0)
                        {
                            it.ExpandHeight = y - y_item;
                            y = y_item + it.ExpandHeight * it.ExpandProg;
                        }
                        else if (!it.Expand) y = y_item;
                    }
                }
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            scrollY.MouseWheel(e.Delta);
            base.OnMouseWheel(e);
        }

        #endregion

        #region 渲染

        internal ScrollY scrollY;
        public Menu() { scrollY = new ScrollY(this); }
        protected override void OnPaint(PaintEventArgs e)
        {
            if (items == null || items.Count == 0) return;
            var rect = ClientRectangle;
            if (rect.Width == 0 || rect.Height == 0) return;
            var g = e.Graphics.High();
            float sy = scrollY.Value;
            g.TranslateTransform(0, -sy);
            Color color_fore = fore.HasValue ? fore.Value : Style.Db.TextBase, color_fore_active = ForeActive.HasValue ? ForeActive.Value : Style.Db.Primary, color_hover = BackHover.HasValue ? BackHover.Value : Style.Db.FillSecondary;
            float _radius = radius * Config.Dpi;
            using (var sub_bg = new SolidBrush(Style.Db.FillQuaternary))
            {
                PaintItem(g, rect, sy, Items, color_fore, color_fore_active, color_hover, _radius, sub_bg);
            }
            g.ResetTransform();
            scrollY.Paint(g);
            this.PaintBadge(g);
            base.OnPaint(e);
        }

        void PaintItem(Graphics g, Rectangle rect, float sy, MenuItemCollection items, Color fore, Color fore_active, Color hover, float radius, SolidBrush sub_bg)
        {
            foreach (MenuItem it in items)
            {
                it.show = it.Show && it.Visible && it.rect.Y > sy - rect.Height - (it.Expand ? it.ExpandHeight : 0) && it.rect.Bottom < scrollY.Value + scrollY.Height + it.rect.Height;
                if (it.show)
                {
                    PaintItem(g, it, fore, fore_active, hover, radius);
                    if (it.Expand && it.Sub != null)
                    {
                        if (ShowSubBack) g.FillRectangle(sub_bg, new RectangleF(rect.X, it.SubY, rect.Width, it.SubHeight));

                        PaintItem(g, rect, sy, it.Sub, fore, fore_active, hover, radius);
                        if (it.ExpandThread)
                        {
                            using (var brush = new SolidBrush(BackColor))
                            {
                                g.FillRectangle(brush, new RectangleF(rect.X, it.rect.Bottom + it.ExpandHeight * it.ExpandProg, rect.Width, it.ExpandHeight));
                            }
                        }
                    }
                }
            }
        }
        void PaintItem(Graphics g, Rectangle rect, float sy, MenuItemCollection items, Color fore, Color fore_active, Color hover, float radius)
        {
            foreach (MenuItem it in items)
            {
                it.show = it.Show && it.Visible && it.rect.Y > sy - rect.Height - (it.Expand ? it.ExpandHeight : 0) && it.rect.Bottom < scrollY.Value + scrollY.Height + it.rect.Height;
                if (it.show)
                {
                    PaintItem(g, it, fore, fore_active, hover, radius);
                    if (it.Expand && it.Sub != null)
                    {
                        PaintItem(g, rect, sy, it.Sub, fore, fore_active, hover, radius);
                        if (it.ExpandThread)
                        {
                            using (var brush = new SolidBrush(BackColor))
                            {
                                g.FillRectangle(brush, new RectangleF(rect.X, it.rect.Bottom + it.ExpandHeight * it.ExpandProg, rect.Width, it.ExpandHeight));
                            }
                        }
                    }
                }
            }
        }
        void PaintItem(Graphics g, MenuItem item, Color fore, Color fore_active, Color hover, float radius)
        {
            if (item.Select)
            {
                if (item.CanExpand)
                {
                    using (var pen = new Pen(fore_active, 2F))
                    {
                        pen.StartCap = pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                        g.DrawLines(pen, item.arr_rect.TriangleLines(item.ArrowProg, .4F));
                    }
                }
                else PaintBack(g, BackActive.HasValue ? BackActive.Value : Style.Db.PrimaryBg, item.rect, radius);

                using (var brush = new SolidBrush(fore_active))
                {
                    g.DrawString(item.Text, Font, brush, item.txt_rect, Helper.stringFormatLeft);
                }
            }
            else
            {
                if (item.AnimationHover)
                {
                    PaintBack(g, Color.FromArgb((int)(item.AnimationHoverValue * hover.A), hover), item.rect, radius);
                }
                else if (item.Hover) PaintBack(g, hover, item.rect, radius);
                if (item.CanExpand)
                {
                    using (var pen = new Pen(fore, 2F))
                    {
                        pen.StartCap = pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                        g.DrawLines(pen, item.arr_rect.TriangleLines(item.ArrowProg, .4F));
                    }
                }
                using (var brush = new SolidBrush(item.Enabled ? fore : Style.Db.TextQuaternary))
                {
                    g.DrawString(item.Text, Font, brush, item.txt_rect, Helper.stringFormatLeft);
                }
            }
            if (item.Icon != null) g.DrawImage(item.Icon, item.ico_rect);
        }

        void PaintBack(Graphics g, Color color, RectangleF rect, float radius)
        {
            using (var brush = new SolidBrush(color))
            {
                if (Round || radius > 0)
                {
                    using (var path = rect.RoundPath(radius, Round))
                    {
                        g.FillPath(brush, path);
                    }
                }
                else g.FillRectangle(brush, rect);
            }
        }

        #endregion

        #region 鼠标

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (scrollY.MouseDown(e.Location))
            {
                if (items == null || items.Count == 0) return;
                foreach (MenuItem it in Items)
                {
                    var list = new List<MenuItem> { it };
                    if (IMouseDown(it, list, e.Location))
                    {
                        return;
                    }
                }
            }
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            scrollY.MouseUp(e.Location);
        }

        bool IMouseDown(MenuItem item, List<MenuItem> list, Point point)
        {
            bool can = item.CanExpand;
            if (item.Enabled && item.Contains(point, 0, scrollY.Value, out _))
            {
                if (can) item.Expand = !item.Expand;
                else
                {
                    IUSelect();
                    if (list.Count > 1)
                    {
                        foreach (MenuItem it in list)
                        {
                            it.Select = true;
                        }
                    }
                    item.Select = true;
                    OnSelectIndexChanged(item);
                    Invalidate();
                }
                return true;
            }
            if (can && item.Expand)
                foreach (MenuItem sub in item.Sub)
                {
                    var list_ = new List<MenuItem>();
                    list_.AddRange(list);
                    list_.Add(sub);
                    if (IMouseDown(sub, list_, point)) return true;
                }
            return false;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (scrollY.MouseMove(e.Location))
            {
                if (items == null || items.Count == 0) return;
                int count = 0, hand = 0;
                foreach (MenuItem it in Items)
                {
                    IMouseMove(it, e.Location, ref count, ref hand);
                }
                SetCursor(hand > 0);
                if (count > 0) Invalidate();
            }
            else ILeave();
        }

        void IMouseMove(MenuItem item, Point point, ref int count, ref int hand)
        {
            if (item.show)
            {
                if (item.Contains(point, 0, scrollY.Value, out var change))
                {
                    hand++;
                }
                if (change) count++;
                if (item.Sub != null)
                    foreach (MenuItem sub in item.Sub)
                    {
                        IMouseMove(sub, point, ref count, ref hand);
                    }
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            scrollY.Leave();
            ILeave();
        }
        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            scrollY.Leave();
            ILeave();
        }

        void ILeave()
        {
            SetCursor(false);
            if (items == null || items.Count == 0) return;
            int count = 0;
            foreach (MenuItem it in Items)
            {
                ILeave(it, ref count);
            }
            if (count > 0) Invalidate();
        }
        void ILeave(MenuItem item, ref int count)
        {
            if (item.Hover) count++;
            item.Hover = false;
            if (item.Sub != null)
                foreach (MenuItem sub in item.Sub)
                {
                    ILeave(sub, ref count);
                }
        }

        void IUSelect()
        {
            foreach (MenuItem it in Items)
            {
                IUSelect(it);
            }
        }
        void IUSelect(MenuItem item)
        {
            item.Select = false;
            if (item.Sub != null)
                foreach (MenuItem sub in item.Sub)
                {
                    IUSelect(sub);
                }
        }

        #endregion
    }

    public class MenuItemCollection : iCollection<MenuItem>
    {
        public MenuItemCollection(Menu it)
        {
            BindData(it);
        }
        public MenuItemCollection(MenuItem it)
        {
            BindData(it);
        }

        internal MenuItemCollection BindData(Menu it)
        {
            action = render =>
            {
                if (render) it.ChangeList();
                it.Invalidate();
            };
            return this;
        }
        internal MenuItemCollection BindData(MenuItem it)
        {
            action = render =>
            {
                if (it.PARENT == null) return;
                if (render) it.PARENT.ChangeList();
                it.PARENT.Invalidate();
            };
            return this;
        }
    }

    public class MenuItem : NotifyPropertyChanged
    {
        public MenuItem() { }
        public MenuItem(string text)
        {
            Text = text;
        }
        public MenuItem(string text, Bitmap? icon)
        {
            Text = text;
            Icon = icon;
        }

        Bitmap? icon = null;
        /// <summary>
        /// 图标
        /// </summary>
        [Description("图标"), Category("外观"), DefaultValue(null)]
        public Bitmap? Icon
        {
            get => icon;
            set
            {
                if (icon == value) return;
                icon = value;
                OnPropertyChanged("Icon");
            }
        }

        string? text = null;
        /// <summary>
        /// 文本
        /// </summary>
        [Description("文本"), Category("外观"), DefaultValue(null)]
        public string? Text
        {
            get => text;
            set
            {
                if (text == value) return;
                text = value;
                OnPropertyChanged("Text");
            }
        }

        bool visible = true;
        /// <summary>
        /// 是否显示
        /// </summary>
        [Description("是否显示"), Category("外观"), DefaultValue(true)]
        public bool Visible
        {
            get => visible;
            set
            {
                if (visible == value) return;
                visible = value;
                Invalidates();
            }
        }

        /// <summary>
        /// 与对象关联的用户定义数据
        /// </summary>
        [Description("与对象关联的用户定义数据"), Category("数据"), DefaultValue(null)]
        public object? Tag { get; set; }

        MenuItemCollection? items;
        /// <summary>
        /// 获取列表中所有列表项的集合
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("子集合"), Category("外观")]
        public MenuItemCollection Sub
        {
            get
            {
                items ??= new MenuItemCollection(this);
                return items;
            }
            set => items = value.BindData(this);
        }

        #region 禁用

        bool enabled = true;
        /// <summary>
        /// 禁用状态
        /// </summary>
        [Description("禁用状态"), Category("行为"), DefaultValue(true)]
        public bool Enabled
        {
            get => enabled;
            set
            {
                if (enabled == value) return;
                enabled = value;
                Invalidate();
            }
        }

        #endregion

        #region 展开

        ITask? ThreadExpand = null;
        bool expand = true;
        /// <summary>
        /// 展开
        /// </summary>
        [Description("展开"), Category("行为"), DefaultValue(true)]
        public bool Expand
        {
            get => expand;
            set
            {
                if (items != null && items.Count > 0)
                {
                    expand = value;
                    if (Config.Animation)
                    {
                        ThreadExpand?.Dispose();
                        float oldval = -1;
                        if (ThreadExpand?.Tag is float oldv) oldval = oldv;
                        ExpandThread = true;
                        if (value)
                        {
                            var t = Animation.TotalFrames(10, ExpandCount(this) * 50);
                            ThreadExpand = new ITask(false, 10, t, oldval, AnimationType.Ball, (i, val) =>
                            {
                                ExpandProg = val;
                                ArrowProg = Animation.Animate(i, t, 2F, AnimationType.Ball) - 1F;
                                Invalidates();
                            }, () =>
                            {
                                ArrowProg = 1;
                                ExpandProg = 1F;
                                ExpandThread = false;
                                Invalidates();
                            });
                        }
                        else
                        {
                            var t = Animation.TotalFrames(10, 200);
                            ThreadExpand = new ITask(true, 10, t, oldval, AnimationType.Ball, (i, val) =>
                            {
                                ExpandProg = val;
                                ArrowProg = -(Animation.Animate(i, t, 2F, AnimationType.Ball) - 1F);
                                Invalidates();
                            }, () =>
                            {
                                ExpandProg = 1F;
                                ExpandThread = false;
                                ArrowProg = -1;
                                Invalidates();
                            });
                        }
                    }
                    else
                    {
                        ExpandProg = 1F; ArrowProg = value ? 1F : -1F;
                        Invalidates();
                    }
                }
            }
        }

        internal int ExpandCount(MenuItem it)
        {
            int count = 0;
            if (it.Sub != null && it.Sub.Count > 0)
            {
                count += it.Sub.Count;
                foreach (MenuItem item in it.Sub)
                {
                    if (item.Expand) count += ExpandCount(item);
                }
            }
            return count;
        }

        [Description("是否可以展开"), Category("行为"), DefaultValue(false)]
        public bool CanExpand
        {
            get => visible && items != null && items.Count > 0;
        }

        /// <summary>
        /// 菜单坐标位置
        /// </summary>
        public RectangleF Rect
        {
            get
            {
                if (PARENT == null) return rect;
                float y = PARENT.scrollY.Value;
                if (y != 0F) return new RectangleF(rect.X, rect.Y - y, rect.Width, rect.Height);
                return rect;
            }
        }

        #endregion

        void Invalidate()
        {
            PARENT?.Invalidate();
        }
        void Invalidates()
        {
            if (PARENT != null)
            {
                PARENT.ChangeList();
                PARENT.Invalidate();
            }
        }

        internal float SubY { get; set; }
        internal float SubHeight { get; set; }

        internal float ExpandHeight { get; set; }
        internal float ExpandProg { get; set; }
        internal bool ExpandThread { get; set; }
        internal bool show { get; set; }
        internal bool Show { get; set; }

        bool hover = false;
        /// <summary>
        /// 是否移动
        /// </summary>
        internal bool Hover
        {
            get => hover;
            set
            {
                if (hover == value) return;
                hover = value;
                if (Config.Animation)
                {
                    ThreadHover?.Dispose();
                    AnimationHover = true;
                    var t = Animation.TotalFrames(20, 200);
                    if (value)
                    {
                        ThreadHover = new ITask((i) =>
                        {
                            AnimationHoverValue = Animation.Animate(i, t, 1F, AnimationType.Ball);
                            Invalidate();
                            return true;
                        }, 20, t, () =>
                        {
                            AnimationHover = false;
                            AnimationHoverValue = 1;
                            Invalidate();
                        });
                    }
                    else
                    {
                        ThreadHover = new ITask((i) =>
                        {
                            AnimationHoverValue = 1F - Animation.Animate(i, t, 1F, AnimationType.Ball);
                            Invalidate();
                            return true;
                        }, 20, t, () =>
                        {
                            AnimationHover = false;
                            AnimationHoverValue = 0;
                            Invalidate();
                        });
                    }
                }
            }
        }
        internal bool Select { get; set; }
        internal int Depth { get; set; }
        internal float ArrowProg { get; set; } = 1F;
        internal Menu? PARENT { get; set; }
        internal void SetRect(int depth, bool indent, RectangleF _rect, float icon_size, float gap)
        {
            Depth = depth;
            rect = _rect;
            if (indent || depth > 1)
            {
                ico_rect = new RectangleF(_rect.X + (gap * depth), _rect.Y + (_rect.Height - icon_size) / 2F, icon_size, icon_size);
                txt_rect = new RectangleF(ico_rect.X + ico_rect.Width + gap, _rect.Y, _rect.Width - (ico_rect.Width + gap * 2), _rect.Height);
            }
            else
            {
                ico_rect = new RectangleF(_rect.X + gap, _rect.Y + (_rect.Height - icon_size) / 2F, icon_size, icon_size);
                txt_rect = new RectangleF(ico_rect.X + ico_rect.Width + gap, _rect.Y, _rect.Width - (ico_rect.Width + gap * 2), _rect.Height);
            }
            arr_rect = new RectangleF(_rect.Right - ico_rect.Height - (ico_rect.Height * 0.9F), _rect.Y + (_rect.Height - ico_rect.Height) / 2, ico_rect.Height, ico_rect.Height);
            Show = true;
        }
        internal RectangleF rect { get; set; }
        internal RectangleF arr_rect { get; set; }

        internal bool Contains(Point point, float x, float y, out bool change)
        {
            if (rect.Contains(new PointF(point.X + x, point.Y + y)))
            {
                change = SetHover(true);
                return true;
            }
            else
            {
                change = SetHover(false);
                return false;
            }
        }

        internal bool SetHover(bool val)
        {
            bool change = false;
            if (val)
            {
                if (!hover) change = true;
                Hover = true;
            }
            else
            {
                if (hover) change = true;
                Hover = false;
            }
            return change;
        }

        internal float AnimationHoverValue = 0;
        internal bool AnimationHover = false;
        ITask? ThreadHover = null;

        internal RectangleF txt_rect { get; set; }
        internal RectangleF ico_rect { get; set; }
    }
}