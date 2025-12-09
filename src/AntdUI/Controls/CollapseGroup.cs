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
// GITCODE: https://gitcode.com/AntdUI/AntdUI
// GITEE: https://gitee.com/AntdUI/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// CollapseGroup 折叠分组面板
    /// </summary>
    /// <remarks>可以折叠/展开的内容区域。</remarks>
    [Description("CollapseGroup 折叠分组面板")]
    [ToolboxItem(true)]
    [DefaultProperty("Items")]
    [DefaultEvent("ItemClick")]
    public class CollapseGroup : IControl, ICollapse
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
        public new Color? ForeColor
        {
            get => fore;
            set
            {
                if (fore == value) return;
                fore = value;
                Invalidate();
                OnPropertyChanged(nameof(ForeColor));
            }
        }

        /// <summary>
        /// 激活字体颜色
        /// </summary>
        [Description("激活字体颜色"), Category("外观"), DefaultValue(null)]
        public Color? ForeActive { get; set; }

        /// <summary>
        /// 只保持一个子菜单的展开
        /// </summary>
        [Description("只保持一个子菜单的展开"), Category("外观"), DefaultValue(false)]
        public bool Unique { get; set; }

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
                OnPropertyChanged(nameof(Radius));
            }
        }

        int columnCount = 6;
        /// <summary>
        /// 列数量
        /// </summary>
        [Description("列数量"), Category("外观"), DefaultValue(6)]
        public int ColumnCount
        {
            get => columnCount;
            set
            {
                if (columnCount == value) return;
                columnCount = value;
                LoadLayout();
                Invalidate();
                OnPropertyChanged(nameof(ColumnCount));
            }
        }

        CollapseGroupItemCollection? items;
        /// <summary>
        /// 集合
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("集合"), Category("数据")]
        public CollapseGroupItemCollection Items
        {
            get
            {
                items ??= new CollapseGroupItemCollection(this);
                return items;
            }
            set => items = value.BindData(this);
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
                    LoadLayout();
                    Invalidate();
                }
                OnPropertyChanged(nameof(PauseLayout));
            }
        }

        /// <summary>
        /// 滚动条
        /// </summary>
        [Browsable(false)]
        public ScrollBar ScrollBar;

        #endregion

        #region 布局

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            LoadLayout(false);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            LoadLayout();
            base.OnSizeChanged(e);
        }

        public void LoadLayout(bool r = true)
        {
            if (IsHandleCreated)
            {
                var _rect = ClientRectangle;
                if (pauseLayout || items == null || items.Count == 0 || (_rect.Width == 0 || _rect.Height == 0)) return;
                var rect = ClientRectangle.DeflateRect(Padding);
                int y = rect.Y;
                Helper.GDI(g =>
                {
                    var size = g.MeasureString(Config.NullText, Font);
                    int gap = (int)(4 * Config.Dpi), csize = (rect.Width - (gap * (columnCount - 1))) / columnCount, icon_size = csize / 2, height = size.Height + gap * 2;
                    foreach (var it in items)
                    {
                        it.PARENT = this;
                        it.SetRect(g, new Rectangle(rect.X, y, rect.Width, height), size.Height, gap);
                        y += height;
                        if (it.CanExpand)
                        {
                            int y_item = y;
                            ChangeList(g, rect, it, it.Sub, ref y, size.Height, csize, icon_size, gap);
                            it.SubY = y_item;
                            it.SubHeight = y - y_item;
                            if ((it.Expand || it.ExpandThread) && it.ExpandProg > 0)
                            {
                                it.ExpandHeight = y - y_item;
                                y = y_item + (int)(it.ExpandHeight * it.ExpandProg);
                            }
                            else if (!it.Expand) y = y_item;
                        }
                    }
                });
                ScrollBar.SetVrSize(0, y);
                ScrollBar.SizeChange(_rect);
                if (r) Invalidate();
            }
        }

        void ChangeList(Canvas g, Rectangle rect, CollapseGroupItem Parent, CollapseGroupSubCollection items, ref int y, int font_height, int csize, int icon_size, int gap)
        {
            int hasI = 0, tmp = 0;
            foreach (var it in items)
            {
                it.PARENT = this;
                it.ParentItem = Parent;
                var size = g.MeasureString(it.Text, Font, csize, s_c);
                int xc = size.Height - font_height;
                if (xc > 0 && tmp < xc) tmp = xc;
                it.SetRect(g, new Rectangle(rect.X + ((csize + gap) * hasI), y, csize, csize), font_height, xc, icon_size);
                hasI++;
                if (hasI > columnCount - 1)
                {
                    y += csize + gap + tmp;
                    hasI = tmp = 0;
                }
            }
            if (hasI > 0) y += csize + gap + tmp;
        }

        #endregion

        #region 渲染

        public CollapseGroup()
        {
            ScrollBar = new ScrollBar(this, true, true);
        }

        protected override void OnDraw(DrawEventArgs e)
        {
            if (items == null || items.Count == 0)
            {
                base.OnDraw(e);
                return;
            }
            var g = e.Canvas;
            float _radius = radius * Config.Dpi;
            int sx = ScrollBar.ValueX, sy = ScrollBar.ValueY;
            g.TranslateTransform(-sx, -sy);
            using (var brush_fore = new SolidBrush(fore ?? Colour.TextBase.Get(nameof(CollapseGroup), ColorScheme)))
            using (var brush_fore_active = new SolidBrush(ForeActive ?? Colour.Primary.Get(nameof(CollapseGroup), ColorScheme)))
            using (var brush_hover = new SolidBrush(BackHover ?? Colour.FillSecondary.Get(nameof(CollapseGroup), ColorScheme)))
            using (var brush_active = new SolidBrush(BackActive ?? Colour.PrimaryBg.Get(nameof(CollapseGroup), ColorScheme)))
            using (var brush_TextQuaternary = new SolidBrush(Colour.TextQuaternary.Get(nameof(CollapseGroup), ColorScheme)))
            {
                PaintItem(g, e.Rect, sx, sy, items, brush_fore, brush_fore_active, brush_hover, brush_active, brush_TextQuaternary, _radius);
            }
            g.ResetTransform();
            ScrollBar.Paint(g, ColorScheme);
            base.OnDraw(e);
        }

        void PaintItem(Canvas g, Rectangle rect, int sx, int sy, CollapseGroupItemCollection items, SolidBrush fore, SolidBrush fore_active, SolidBrush hover, SolidBrush active, SolidBrush brush_TextQuaternary, float radius)
        {
            foreach (var it in items)
            {
                if (it.Show)
                {
                    PaintArrow(g, it, fore, sx, sy);
                    g.String(it.Text, Font, fore, it.txt_rect, s_l);
                    if ((it.Expand || it.ExpandThread) && it.items != null && it.items.Count > 0)
                    {
                        if (it.ExpandThread) g.SetClip(new RectangleF(rect.X, it.rect.Bottom, rect.Width, it.ExpandHeight * it.ExpandProg));
                        foreach (var sub in it.items)
                        {
                            if (sub.Show)
                            {
                                if (sub.Enabled)
                                {
                                    if (sub.Select)
                                    {
                                        PaintBack(g, sub, active, radius);
                                        if (sub.AnimationHover)
                                        {
                                            using (var brush = new SolidBrush(Helper.ToColorN(sub.AnimationHoverValue, hover.Color)))
                                            {
                                                PaintBack(g, sub, brush, radius);
                                            }
                                        }
                                        else if (sub.Hover) PaintBack(g, sub, hover, radius);

                                        if (sub.Icon != null) g.Image(sub.Icon, sub.ico_rect);
                                        if (sub.IconSvg != null) g.GetImgExtend(sub.IconSvg, sub.ico_rect, fore_active.Color);
                                        g.String(sub.Text, Font, fore_active, sub.txt_rect, s_c);
                                    }
                                    else
                                    {
                                        if (sub.AnimationHover)
                                        {
                                            using (var brush = new SolidBrush(Helper.ToColorN(sub.AnimationHoverValue, hover.Color)))
                                            {
                                                PaintBack(g, sub, brush, radius);
                                            }
                                        }
                                        else if (sub.Hover) PaintBack(g, sub, hover, radius);

                                        if (sub.Icon != null) g.Image(sub.Icon, sub.ico_rect);
                                        if (sub.IconSvg != null) g.GetImgExtend(sub.IconSvg, sub.ico_rect, fore.Color);
                                        g.String(sub.Text, Font, fore, sub.txt_rect, s_c);
                                    }
                                }
                                else
                                {
                                    if (sub.Icon != null) g.Image(sub.Icon, sub.ico_rect);
                                    if (sub.IconSvg != null) g.GetImgExtend(sub.IconSvg, sub.ico_rect, brush_TextQuaternary.Color);
                                    g.String(sub.Text, Font, brush_TextQuaternary, sub.txt_rect, s_c);
                                }
                            }
                        }
                        g.ResetClip();
                    }
                }
            }
        }

        public static void PaintBack(Canvas g, CollapseGroupSub sub, SolidBrush brush, float radius)
        {
            if (radius > 0)
            {
                using (var path = sub.rect.RoundPath(radius))
                {
                    g.Fill(brush, path);
                }
            }
            else g.Fill(brush, sub.rect);
        }

        readonly FormatFlags s_c = FormatFlags.HorizontalCenter | FormatFlags.Top, s_l = FormatFlags.Left | FormatFlags.VerticalCenter | FormatFlags.NoWrapEllipsis;

        internal PointF[] PaintArrow(RectangleF rect)
        {
            float size = rect.Height * 0.15F, size2 = rect.Height * 0.2F, size3 = rect.Height * 0.26F;
            return new PointF[] {
                new PointF(rect.X+size,rect.Y+rect.Height/2),
                new PointF(rect.X+rect.Width*0.4F,rect.Y+(rect.Height-size3)),
                new PointF(rect.X+rect.Width-size2,rect.Y+size2),
            };
        }

        void PaintArrow(Canvas g, CollapseGroupItem item, SolidBrush color, int sx, int sy)
        {
            using (var pen = new Pen(color, 2F))
            {
                pen.StartCap = pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                if (item.ExpandThread) g.DrawLines(pen, item.arr_rect.TriangleLinesVertical(-(1F - (2F * item.ExpandProg)), .4F));
                else if (item.Expand) g.DrawLines(pen, item.arr_rect.TriangleLinesVertical(1, .4F));
                else g.DrawLines(pen, item.arr_rect.TriangleLinesVertical(-1, .4F));
            }
        }

        #endregion

        #region 鼠标

        object? MDown;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            MDown = null;
            if (ScrollBar.MouseDownY(e.X, e.Y) && ScrollBar.MouseDownX(e.X, e.Y))
            {
                if (items == null || items.Count == 0) return;
                int y = ScrollBar.ValueY;
                OnTouchDown(e.X, e.Y);
                foreach (var it in items)
                {
                    if (it.rect.Contains(e.X, e.Y + y))
                    {
                        MDown = it;
                        return;
                    }
                    else if (it.Expand && it.items != null && it.items.Count > 0)
                    {
                        foreach (var sub in it.items)
                        {
                            if (sub.Show && sub.Enabled && sub.rect.Contains(e.X, e.Y + y))
                            {
                                MDown = sub;
                                return;
                            }
                        }
                    }
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (ScrollBar.MouseMoveY(e.X, e.Y) && ScrollBar.MouseMoveX(e.X, e.Y))
            {
                int hand = 0;
                if (items == null || items.Count == 0) return;
                int y = ScrollBar.ValueY;
                if (OnTouchMove(e.X, e.Y))
                {
                    foreach (var it in items)
                    {
                        if (it.rect.Contains(e.X, e.Y + y)) hand++;
                        else if (it.Expand && it.items != null && it.items.Count > 0)
                        {
                            foreach (var sub in it.items)
                            {
                                sub.Hover = sub.Show && sub.Enabled && sub.rect.Contains(e.X, e.Y + y);
                                if (sub.Hover) hand++;
                            }
                        }
                    }
                }
                SetCursor(hand > 0);
            }
            else ILeave();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (ScrollBar.MouseUpY() && ScrollBar.MouseUpX() && OnTouchUp())
            {
                if (items == null || items.Count == 0 || MDown == null) return;
                int y = ScrollBar.ValueY;
                foreach (var it in items)
                {
                    if (it == MDown)
                    {
                        if (it.rect.Contains(e.X, e.Y + y))
                        {
                            it.Expand = !it.Expand;
                            if (it.Expand && Unique)
                            {
                                foreach (var it2 in items)
                                {
                                    if (it2 != it) it2.Expand = false;
                                }
                            }
                        }
                        MDown = null;
                        return;
                    }
                    if (it.items != null && it.items.Count > 0)
                    {
                        foreach (var sub in it.items)
                        {
                            if (MDown == sub)
                            {
                                if (sub.rect.Contains(e.X, e.Y + y))
                                {
                                    sub.Select = true;
                                    OnItemClick(sub, new Rectangle(sub.rect.X, sub.rect.Y - y, sub.rect.Width, sub.rect.Height), e);
                                }
                                MDown = null;
                                return;
                            }
                        }
                    }
                }
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            ScrollBar.Leave();
            ILeave();
        }

        void ILeave()
        {
            if (items == null || items.Count == 0) return;
            foreach (var it in items)
            {
                if (it.items != null && it.items.Count > 0)
                {
                    foreach (var sub in it.items) sub.Hover = false;
                }
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            ScrollBar.MouseWheel(e);
            base.OnMouseWheel(e);
        }
        protected override bool OnTouchScrollX(int value) => ScrollBar.MouseWheelXCore(value);
        protected override bool OnTouchScrollY(int value) => ScrollBar.MouseWheelYCore(value);

        #endregion

        #region 事件

        public class ItemClickEventArgs : VMEventArgs<CollapseGroupSub>
        {
            public ItemClickEventArgs(CollapseGroupSub item, Rectangle rect, MouseEventArgs e) : base(item, e) { Rect = rect; }
            public Rectangle Rect { get; private set; }
        }

        public delegate void ItemClickEventHandler(object sender, ItemClickEventArgs e);

        /// <summary>
        /// 点击项事件
        /// </summary>
        [Description("点击项事件"), Category("行为")]
        public event ItemClickEventHandler? ItemClick;

        protected virtual void OnItemClick(CollapseGroupSub item, Rectangle rect, MouseEventArgs e) => ItemClick?.Invoke(this, new ItemClickEventArgs(item, rect, e));

        #endregion

        #region 方法

        public void IUSelect()
        {
            if (items == null || items.Count == 0) return;
            foreach (var it in items)
            {
                if (it.items != null && it.items.Count > 0)
                {
                    foreach (var sub in it.items) sub.Select = false;
                }
            }
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            ScrollBar.Dispose();
            base.Dispose(disposing);
        }

        void ICollapse.LoadLayout(bool r) => LoadLayout(r);
    }

    public class CollapseGroupItemCollection : iCollection<CollapseGroupItem>
    {
        public CollapseGroupItemCollection(CollapseGroup it)
        {
            BindData(it);
        }
        public CollapseGroupItemCollection(CollapseGroupItem it)
        {
            BindData(it);
        }

        internal CollapseGroupItemCollection BindData(CollapseGroup it)
        {
            action = render =>
            {
                if (render) it.LoadLayout();
                it.Invalidate();
            };
            return this;
        }

        internal CollapseGroupItemCollection BindData(CollapseGroupItem it)
        {
            action = render =>
            {
                if (it.PARENT == null) return;
                if (render) it.PARENT.LoadLayout();
                it.PARENT.Invalidate();
            };
            return this;
        }
    }

    public interface ICollapseItem
    {
        string? Text { get; set; }
    }

    public class CollapseGroupItem : ICollapseItem
    {
        public CollapseGroupItem() { }
        public CollapseGroupItem(string text)
        {
            Text = text;
        }

        string? text;
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
                Invalidates();
            }
        }

        /// <summary>
        /// 用户定义数据
        /// </summary>
        [Description("用户定义数据"), Category("数据"), DefaultValue(null)]
        public object? Tag { get; set; }

        internal CollapseGroupSubCollection? items;
        /// <summary>
        /// 获取列表中所有列表项的集合
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("子集合"), Category("外观")]
        public CollapseGroupSubCollection Sub
        {
            get
            {
                items ??= new CollapseGroupSubCollection(this);
                return items;
            }
            set => items = value.BindData(this);
        }

        #region 展开

        AnimationTask? ThreadExpand;
        bool expand = false;
        /// <summary>
        /// 展开
        /// </summary>
        [Description("展开"), Category("行为"), DefaultValue(false)]
        public bool Expand
        {
            get => expand;
            set
            {
                if (expand == value) return;
                expand = value;
                if (items != null && items.Count > 0)
                {
                    if (PARENT != null && PARENT.IsHandleCreated && Config.HasAnimation(nameof(CollapseGroup)))
                    {
                        ThreadExpand?.Dispose();
                        float oldval = -1;
                        if (ThreadExpand?.Tag is float oldv) oldval = oldv;
                        ExpandThread = true;
                        ThreadExpand = new AnimationTask(new AnimationFixed2Config((i, val) =>
                        {
                            ExpandProg = val;
                            Invalidates();
                        }, 10, Animation.TotalFrames(10, 200), oldval, value).SetEnd(() =>
                        {
                            ExpandProg = 1F;
                            ExpandThread = false;
                            Invalidates();
                        }));
                    }
                    else
                    {
                        ExpandProg = 1F;
                        Invalidates();
                    }
                }
                else
                {
                    ExpandProg = 1F;
                    Invalidates();
                }
            }
        }

        [Description("是否可以展开"), Category("行为"), DefaultValue(false)]
        public bool CanExpand => items != null && items.Count > 0;

        #endregion

        #region 样式

        Color? fore;
        /// <summary>
        /// 文本颜色
        /// </summary>
        [Description("文本颜色"), Category("外观"), DefaultValue(null)]
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

        #endregion

        void Invalidate()
        {
            PARENT?.Invalidate();
        }
        void Invalidates()
        {
            if (PARENT == null) return;
            PARENT.LoadLayout();
            PARENT.Invalidate();
        }

        internal float SubY { get; set; }
        internal float SubHeight { get; set; }
        internal float ExpandHeight { get; set; }
        internal float ExpandProg { get; set; }
        internal bool ExpandThread { get; set; }
        internal bool Show { get; set; }

        internal CollapseGroup? PARENT { get; set; }

        internal void SetRect(Canvas g, Rectangle _rect, int icon_size, int gap)
        {
            rect = _rect;
            int x = _rect.X + gap, y = _rect.Y + (_rect.Height - icon_size) / 2;
            arr_rect = new Rectangle(x, y, icon_size, icon_size);
            x += icon_size + gap;

            txt_rect = new Rectangle(x, _rect.Y, _rect.Width - x - gap, _rect.Height);

            Show = true;
        }
        internal Rectangle rect { get; set; }
        internal Rectangle arr_rect { get; set; }

        internal Rectangle txt_rect { get; set; }

        public override string? ToString() => text;
    }

    public interface ICollapse
    {
        bool IsHandleCreated { get; }
        void SetCursor(bool val);
        void IUSelect();
        void LoadLayout(bool r = true);
        void Invalidate();
    }
    public class CollapseGroupSubCollection : iCollection<CollapseGroupSub>
    {
        public CollapseGroupSubCollection(CollapseGroupItem it)
        {
            BindData(it);
        }

        internal CollapseGroupSubCollection BindData(CollapseGroupItem it)
        {
            action = render =>
            {
                if (it.PARENT == null) return;
                if (render) it.PARENT.LoadLayout();
                it.PARENT.Invalidate();
            };
            return this;
        }
    }
    public class CollapseGroupButtonCollection : iCollection<CollapseGroupButton>
    {
        Collapse? PARENT { get; set; }
        CollapseItem? PARENTITEM { get; set; }
        public CollapseGroupButtonCollection(CollapseItem it)
        {
            BindData(it);
        }

        internal CollapseGroupButtonCollection BindData(CollapseItem it)
        {
            PARENT = it?.PARENT;
            PARENTITEM = it;
            action = render =>
            {
                if (it?.PARENT == null) return;
                if (render) it.PARENT.LoadLayout();
                it.PARENT.Invalidate();
            };
            return this;
        }

        public override void Add(CollapseGroupButton item)
        {
            base.Add(item);

            item.PARENT = PARENT;
            item.ParentItem = PARENTITEM;
        }
    }
    /// <summary>
    /// 右侧按钮编辑器模式
    /// </summary>
    public enum EButtonEditTypes
    {
        /// <summary>
        /// 默认：CheckButton
        /// </summary>
        Default = 0,

        /// <summary>
        /// 开关模式。同等于SwitchMode=true
        /// </summary>
        Switch = 1,
        /// <summary>
        /// 文本编辑模式
        /// </summary>
        Input = 2,
        /// <summary>
        /// 自定义继承自IControl的编辑器
        /// </summary>
        Custom = 3,
        /// <summary>
        /// 正常按钮，按下后弹起
        /// </summary>
        Button = 4,
    }
    [DefaultEvent(nameof(TextChanged))]
    [DefaultProperty(nameof(EditType))]
    public class CollapseGroupButton : CollapseGroupSub
    {
        /// <summary>
        /// Checked 属性值更改时发生
        /// </summary>
        [Description("Checked 属性值更改时发生"), Category("行为")]
        public event CollapseSwitchCheckedChangedEventHandler? CheckedChanged;
        [Description("文本改变时发生"), Category("行为")]
        public event CollapseEditChangedEventHandler? TextChanged;
        [Description("自定义编辑器"), Category("行为")]
        public event CollapseCustomInputEditEventHandler? CustomInputEdit;
        public CollapseGroupButton() : base() { }
        public CollapseGroupButton(string text) : base(text) { }

        internal AnimationTask? ThreadHover, ThreadCheck;
        public CollapseGroupButton(string text, Image? icon) : base(text, icon) { }
        internal bool AnimationClick = false;
        internal float AnimationClickValue = 0;
        internal bool AnimationCheck = false;
        internal float AnimationCheckValue = 0;
        internal bool hasFocus = false;
        public override bool Select
        {
            get => base.Select;
            set
            {
                if (select == value) return;
                if (value && PARENT is Collapse collapse && ParentItem is CollapseItem collapseItem) collapse.IUSelect(collapseItem);
                select = value;
                Invalidate();
            }
        }

        int? width;
        /// <summary>
        /// 自定义宽度
        /// </summary>
        [Description("自定义宽度"), Category("外观"), DefaultValue(null)]
        public int? Width
        {
            get => width; set
            {
                if (width == value) return;

                width = value;
                Invalidate();
            }
        }

        protected EButtonEditTypes m_editType = EButtonEditTypes.Default;
        [Description("编辑类型"), Category("外观"), DefaultValue(typeof(EButtonEditTypes), "Default")]
        public EButtonEditTypes EditType
        {
            get => m_editType;
            set
            {
                if (m_editType == value) return;

                m_editType = value;
                Invalidate();
                OnCreateEdit();
            }
        }

        bool visible = true;
        /// <summary>
        /// 是否可见
        /// </summary>
        [Description("是否可见"), Category("行为"), DefaultValue(true)]
        public bool Visible
        {
            get => visible;
            set
            {
                if (visible == value) return;
                visible = value;
                Invalidate();
            }
        }

        bool switchMode = false;
        [Browsable(false)]
        [Description("Switch切换模式"), Category("行为"), DefaultValue(false)]
        public bool SwitchMode
        {
            get
            {
                if (EditType == EButtonEditTypes.Switch) return true;
                return switchMode;
            }
            set
            {
                if (switchMode == value) return;
                switchMode = value;
                EditType = EButtonEditTypes.Switch;
                Invalidate();
            }
        }

        /// <summary>
        /// 编辑器, 参考EditType
        /// </summary>
        [Browsable(false)]
        public IControl? Edit { get; protected set; }

        [Description("工具提示内容"), Category("外观"), DefaultValue(null)]
        public string? Tooltip { get; set; }
        string? _checkedText, _unCheckedText;

        [Description("选中时显示的文本"), Category("外观"), DefaultValue(null)]
        [Localizable(true)]
        public string? CheckedText
        {
            get => _checkedText;
            set
            {
                if (_checkedText == value) return;
                _checkedText = value;
                if (_checked) Invalidate();
            }
        }

        [Description("未选中时显示的文本"), Category("外观"), DefaultValue(null)]
        [Localizable(true)]
        public string? UnCheckedText
        {
            get => _unCheckedText;
            set
            {
                if (_unCheckedText == value) return;
                _unCheckedText = value;
                if (!_checked) Invalidate();
            }
        }

        /// <summary>
        /// 波浪大小
        /// </summary>
        [Description("波浪大小"), Category("外观"), DefaultValue(0)]
        public int WaveSize { get; set; } = 0;
        bool _checked = false;
        [Description("勾选状态"), Category("行为"), DefaultValue(false)]
        public bool Checked
        {
            get => _checked;
            set
            {
                if (_checked == value) return;
                _checked = value;
                if (PARENT == null || ParentItem == null) return;
                try
                {
                    ThreadCheck?.Dispose();
                    if (PARENT.IsHandleCreated && Config.HasAnimation(nameof(Switch)))
                    {
                        AnimationCheck = true;
                        ThreadCheck = new AnimationTask(new AnimationLinearFConfig((Collapse)PARENT, i =>
                        {
                            AnimationCheckValue = i;
                            Invalidate();
                            return true;
                        }, 10).SetValue(AnimationCheckValue, value, 0.1F).SetEnd(() => AnimationCheck = false));
                    }
                    else AnimationCheckValue = value ? 1F : 0F;
                }
                finally
                {
                    if (ParentItem is CollapseItem collapse) CheckedChanged?.Invoke(this, new CollapseSwitchCheckedChangedEventArgs(this, collapse, value));
                }
            }
        }

        bool _mouseHover = false;
        internal bool ExtraMouseHover
        {
            get => _mouseHover;
            set
            {
                if (_mouseHover == value) return;
                _mouseHover = value;
                var enabled = Enabled;
                PARENT?.SetCursor(value && enabled);
                if (enabled)
                {
                    if (PARENT == null) return;
                    if (Config.HasAnimation(nameof(Switch)))
                    {
                        ThreadHover?.Dispose();
                        AnimationHover = true;
                        ThreadHover = new AnimationTask(new AnimationLinearFConfig((Collapse)PARENT, i =>
                        {
                            AnimationHoverValue = i;
                            Invalidate();
                            return true;
                        }, 10).SetValue(AnimationHoverValue, value, 0.1F).SetEnd(() => AnimationHover = false));
                    }
                    else AnimationHoverValue = 255;
                    Invalidate();
                }
            }
        }

        internal override void SetRect(Canvas g, Rectangle rect_read, int font_height, int xc, int icon_size)
        {
            bool emptyIcon = string.IsNullOrEmpty(IconSvg) && Icon == null;
            bool emptyText = SwitchMode ? string.IsNullOrEmpty(Checked ? CheckedText : UnCheckedText) : string.IsNullOrEmpty(Text);
            if (emptyIcon) icon_size = 0;
            if (emptyText) font_height = 0;

            rect = rect_read;
            if (EditType == EButtonEditTypes.Input || EditType == EButtonEditTypes.Custom)
            {
                if (Edit == null)
                {
                    OnCreateEdit();
                }
                else
                {
                    if (Edit != null && PARENT != null)
                    {
                        ((Collapse)PARENT).Invoke(new Action(() => { Edit.Location = rect.Location; Edit.Height = rect.Height; Edit.Refresh(); PARENT.Invalidate(); }));
                    }
                }
            }
            else
            {
                //int sp = (int)(font_height * .25F), t_x = rect_read.Y + (emptyIcon ? 0 : ((rect_read.Height - (font_height + icon_size + sp)) / 2));
                ico_rect = emptyIcon ? Rectangle.Empty : new Rectangle(emptyText == false ? rect_read.X + (int)(2 * Config.Dpi) : (rect_read.X + ((rect_read.Height - icon_size) / 2)), rect_read.Y + ((rect_read.Height - icon_size) / 2), icon_size, icon_size);
                txt_rect = emptyText ? Rectangle.Empty : new Rectangle(rect_read.X + icon_size, rect_read.Y + ((rect_read.Height - font_height) / 2), rect_read.Width - icon_size, rect_read.Height);

                if (xc > 0) rect = new Rectangle(rect_read.X, rect_read.Y, rect_read.Width, rect_read.Height + xc);
            }

            Show = true;

        }
        internal bool Contains(int x, int y)
        {
            if (rect.Contains(x, y))
            {
                Hover = true;
                return true;
            }
            else
            {
                Hover = false;
                return false;
            }
        }

        internal void OnCreateEdit()
        {
            if (PARENT is Collapse parent)
            {
                if (Edit != null)
                {
                    if (parent.Controls.Contains(Edit)) parent.Controls.Remove(Edit);
                    Edit.TextChanged -= edit_TextChanged;
                    Edit.Dispose();
                    Edit = null;
                }
                switch (EditType)
                {
                    case EButtonEditTypes.Input:
                        Input input = new Input()
                        {
                            Location = rect.Location,
                            TabIndex = parent.Controls.Count,
                            Size = rect.Size,
                            PrefixSvg = IconSvg,
                            PrefixFore = Fore,
                            PlaceholderText = Text,
                            Anchor = AnchorStyles.Top | AnchorStyles.Right,
                            IconRatio = ico_rect.Height == 0 || rect.Height == 0 ? 1f : ico_rect.Height / rect.Height,
                        };

                        Edit = input;
                        break;

                    case EButtonEditTypes.Custom:
                        if (CustomInputEdit != null)
                        {
                            CollapseCustomInputEditEventArgs args = new CollapseCustomInputEditEventArgs();
                            CustomInputEdit(this, args);
                            if (args.Edit != null)
                            {
                                args.Edit.Location = rect.Location;
                                args.Edit.Size = new Size(Width ?? args.Edit.Width, rect.Size.Height);
                                args.Edit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                                Edit = args.Edit;
                            }
                        }
                        break;
                    default:
                        break;
                }

                if (Edit != null)
                {
                    Edit.TextChanged += edit_TextChanged;
                    if (Select) Edit.Focus();
                    parent.Controls.Add(Edit);
                }
            }
        }

        private void edit_TextChanged(object? sender, EventArgs e)
        {
            if (TextChanged == null) return;
            if (PARENT is Collapse parent && ParentItem is CollapseItem item)
            {
                if (sender is Input input) TextChanged(this, new CollapseEditChangedEventArgs(parent, item, input.Text));
                else if (sender is IControl control) TextChanged(this, new CollapseEditChangedEventArgs(parent, item, control.Text));
            }
        }
    }
    public class CollapseGroupSub : ICollapseItem
    {
        public CollapseGroupSub() { }
        public CollapseGroupSub(string text)
        {
            Text = text;
        }
        public CollapseGroupSub(string text, Image? icon)
        {
            Text = text;
            Icon = icon;
        }

        Image? icon;
        /// <summary>
        /// 图标
        /// </summary>
        [Description("图标"), Category("外观"), DefaultValue(null)]
        public Image? Icon
        {
            get => icon;
            set
            {
                if (icon == value) return;
                icon = value;
                Invalidates();
            }
        }

        string? iconSvg;
        /// <summary>
        /// 图标
        /// </summary>
        [Description("图标SVG"), Category("外观"), DefaultValue(null)]
        public string? IconSvg
        {
            get => iconSvg;
            set
            {
                if (iconSvg == value) return;
                iconSvg = value;
                Invalidates();
            }
        }

        /// <summary>
        /// 是否包含图片
        /// </summary>
        internal bool HasIcon => iconSvg != null || Icon != null;

        /// <summary>
        /// 名称
        /// </summary>
        [Description("名称"), Category("数据"), DefaultValue(null)]
        public string? Name { get; set; }

        string? text;
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
                Invalidates();
            }
        }

        /// <summary>
        /// 用户定义数据
        /// </summary>
        [Description("用户定义数据"), Category("数据"), DefaultValue(null)]
        public object? Tag { get; set; }

        #region 禁用

        bool enabled = true;
        /// <summary>
        /// 禁掉响应
        /// </summary>
        [Description("禁掉响应"), Category("行为"), DefaultValue(true)]
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

        #region 样式

        Color? fore;
        /// <summary>
        /// 文本颜色
        /// </summary>
        [Description("文本颜色"), Category("外观"), DefaultValue(null)]
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

        Color? back;
        /// <summary>
        /// 背景颜色
        /// </summary>
        [Description("背景颜色"), Category("外观"), DefaultValue(null)]
        public Color? Back
        {
            get => back;
            set
            {
                if (back == value) return;
                back = value;
                Invalidate();
            }
        }

        #endregion

        protected void Invalidate() => PARENT?.Invalidate();
        protected void Invalidates()
        {
            if (PARENT == null) return;
            PARENT.LoadLayout();
            PARENT.Invalidate();
        }

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
                if (Config.HasAnimation(nameof(CollapseGroup)))
                {
                    ThreadHover?.Dispose();
                    AnimationHover = true;
                    ThreadHover = new AnimationTask(new AnimationFixedConfig(i =>
                    {
                        AnimationHoverValue = i;
                        Invalidate();
                    }, 20, Animation.TotalFrames(20, 200), value, AnimationType.Ball).SetEnd(() => AnimationHover = false));
                }
                else Invalidate();
            }
        }

        protected bool select = false;
        [Description("激活状态"), Category("行为"), DefaultValue(false)]
        public virtual bool Select
        {
            get => select;
            set
            {
                if (select == value) return;
                if (value)
                {
                    PARENT?.IUSelect();
                }
                select = value;
                Invalidate();
            }
        }


        internal ICollapse? PARENT { get; set; }

        public ICollapseItem? ParentItem { get; internal set; }

        internal virtual void SetRect(Canvas g, Rectangle rect_read, int font_height, int xc, int icon_size)
        {
            rect = rect_read;
            int sp = (int)(font_height * .25F), t_x = rect_read.Y + ((rect_read.Height - (font_height + icon_size + sp)) / 2);
            txt_rect = new Rectangle(rect_read.X, t_x + icon_size + sp, rect_read.Width, rect_read.Height);
            ico_rect = new Rectangle(rect_read.X + (rect_read.Width - icon_size) / 2, t_x, icon_size, icon_size);
            if (xc > 0) rect = new Rectangle(rect_read.X, rect_read.Y, rect_read.Width, rect_read.Height + xc);
            Show = true;
        }

        internal virtual bool Show { get; set; }
        internal Rectangle rect { get; set; }

        internal bool Contains(int x, int y, int sx, int sy)
        {
            if (rect.Contains(x + sx, y + sy))
            {
                Hover = true;
                return true;
            }
            else
            {
                Hover = false;
                return false;
            }
        }

        internal float AnimationHoverValue = 0;
        internal bool AnimationHover = false;

        AnimationTask? ThreadHover;

        internal Rectangle txt_rect { get; set; }
        internal Rectangle ico_rect { get; set; }

        public override string? ToString() => text;
    }
}