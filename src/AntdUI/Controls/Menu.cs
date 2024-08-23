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
using System.Drawing.Design;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// Menu 导航菜单
    /// </summary>
    /// <remarks>为页面和功能提供导航的菜单列表。</remarks>
    [Description("Menu 导航菜单")]
    [ToolboxItem(true)]
    [DefaultProperty("Items")]
    [DefaultEvent("SelectChanged")]
    public class Menu : IControl, SubLayeredForm
    {
        #region 属性

        #region 系统

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
                if (fore == value) fore = value;
                fore = value;
                Invalidate();
            }
        }

        #endregion

        /// <summary>
        /// 悬停背景颜色
        /// </summary>
        [Description("悬停背景颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? BackHover { get; set; }

        /// <summary>
        /// 激活背景颜色
        /// </summary>
        [Description("激活背景颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? BackActive { get; set; }

        Color? fore;
        /// <summary>
        /// 文字颜色
        /// </summary>
        [Description("文字颜色"), Category("外观"), DefaultValue(null)]
        [Obsolete("使用 ForeColor 属性替代"), Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
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

        TAMode theme = TAMode.Auto;
        /// <summary>
        /// 色彩模式
        /// </summary>
        [Description("色彩模式"), Category("外观"), DefaultValue(TAMode.Auto)]
        public TAMode Theme
        {
            get => theme;
            set
            {
                if (theme == value) return;
                theme = value;
                Invalidate();
            }
        }

        TMenuMode mode = TMenuMode.Inline;
        /// <summary>
        /// 菜单类型
        /// </summary>
        [Description("菜单类型"), Category("外观"), DefaultValue(TMenuMode.Inline)]
        public TMenuMode Mode
        {
            get => mode;
            set
            {
                if (mode == value) return;
                mode = value;
                ChangeList();
                Invalidate();
            }
        }

        /// <summary>
        /// 常规缩进
        /// </summary>
        [Description("常规缩进"), Category("外观"), DefaultValue(false)]
        public bool Indent { get; set; } = false;

        /// <summary>
        /// 只保持一个子菜单的展开
        /// </summary>
        [Description("只保持一个子菜单的展开"), Category("外观"), DefaultValue(false)]
        public bool Unique { get; set; }

        /// <summary>
        /// 显示子菜单背景
        /// </summary>
        [Description("显示子菜单背景"), Category("外观"), DefaultValue(false)]
        public bool ShowSubBack { get; set; } = false;

        /// <summary>
        /// 自动折叠
        /// </summary>
        [Description("自动折叠"), Category("外观"), DefaultValue(false)]
        public bool AutoCollapse { get; set; }

        bool collapsed = false;
        /// <summary>
        /// 是否折叠
        /// </summary>
        [Description("是否折叠"), Category("外观"), DefaultValue(false)]
        public bool Collapsed
        {
            get => collapsed;
            set
            {
                if (collapsed == value) return;
                collapsed = value;
                Width = value ? CollapseWidth : CollapsedWidth;
                OnSizeChanged(EventArgs.Empty);
            }
        }

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
        /// Select 属性值更改时发生
        /// </summary>
        [Description("Select 属性值更改时发生"), Category("行为")]
        public event SelectEventHandler? SelectChanged = null;

        internal void OnSelectIndexChanged(MenuItem item)
        {
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
                    ChangeList();
                    Invalidate();
                }
            }
        }

        #endregion

        #region 布局

        protected override void OnFontChanged(EventArgs e)
        {
            var rect = ChangeList();
            scroll.SizeChange(rect);
            base.OnFontChanged(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            var rect = ChangeList();
            scroll.SizeChange(rect);
            base.OnSizeChanged(e);
        }

        internal int CollapseWidth = 0, CollapsedWidth = 0;
        internal Rectangle ChangeList()
        {
            var _rect = ClientRectangle;
            if (pauseLayout || items == null || items.Count == 0) return _rect;
            if (_rect.Width == 0 || _rect.Height == 0) return _rect;
            var rect = _rect.PaddingRect(Padding);

            int y = 0;
            int icon_count = 0;
            Helper.GDI(g =>
            {
                var lists = items;
                var size = g.MeasureString(Config.NullText, Font);
                int icon_size = (int)Math.Ceiling(size.Height * 1.2F), gap = icon_size / 2, gapI = gap / 2, height = (int)Math.Ceiling(size.Height + gap * 2);
                if (mode == TMenuMode.Horizontal) ChangeListHorizontal(rect, g, lists, 0, icon_size, gap, gapI);
                else
                {
                    CollapseWidth = icon_size * 2 + gap + gapI + Padding.Horizontal;
                    CollapsedWidth = ChangeList(rect, g, null, lists, ref y, ref icon_count, height, icon_size, gap, gapI, 0) + Padding.Horizontal;
                    if (AutoCollapse)
                    {
                        if (icon_count > 0) collapsed = CollapsedWidth > _rect.Width;
                        else collapsed = false;
                    }
                    if (collapsed) ChangeUTitle(lists);
                }
            });
            scroll.SetVrSize(y);
            return _rect;
        }

        int ChangeList(Rectangle rect, Graphics g, MenuItem? Parent, MenuItemCollection items, ref int y, ref int icon_count, int height, int icon_size, int gap, int gapI, int depth)
        {
            int collapsedWidth = 0;
            foreach (MenuItem it in items)
            {
                it.PARENT = this;
                it.PARENTITEM = Parent;
                if (it.HasIcon) icon_count++;
                it.SetRect(depth, Indent, new Rectangle(rect.X, rect.Y + y, rect.Width, height), icon_size, gap);
                if (it.Visible)
                {
                    int size = (int)Math.Ceiling(g.MeasureString(it.Text, it.Font ?? Font).Width + gap * 4 + icon_size + it.arr_rect.Width);
                    if (size > collapsedWidth) collapsedWidth = size;
                    y += height + gapI;
                    if (mode == TMenuMode.Inline && it.CanExpand)
                    {
                        if (!collapsed)
                        {
                            int y_item = y;

                            int size2 = ChangeList(rect, g, it, it.Sub, ref y, ref icon_count, height, icon_size, gap, gapI, depth + 1);
                            if (size2 > collapsedWidth) collapsedWidth = size2;

                            it.SubY = y_item - gapI / 2;
                            it.SubHeight = y - y_item;

                            if ((it.Expand || it.ExpandThread) && it.ExpandProg > 0)
                            {
                                it.ExpandHeight = y - y_item;
                                y = y_item + (int)Math.Ceiling(it.ExpandHeight * it.ExpandProg);
                            }
                            else if (!it.Expand) y = y_item;
                        }
                        else
                        {
                            int oldy = y;
                            int size2 = ChangeList(rect, g, it, it.Sub, ref y, ref icon_count, height, icon_size, gap, gapI, depth + 1);
                            if (size2 > collapsedWidth) collapsedWidth = size2;
                            y = oldy;
                        }
                    }
                }
            }
            return collapsedWidth;
        }
        void ChangeListHorizontal(Rectangle rect, Graphics g, MenuItemCollection items, int x, int icon_size, int gap, int gapI)
        {
            foreach (MenuItem it in items)
            {
                it.PARENT = this;
                int size;
                if (it.HasIcon) size = (int)Math.Ceiling(g.MeasureString(it.Text, it.Font ?? Font).Width + gap * 3 + icon_size);
                else size = (int)Math.Ceiling(g.MeasureString(it.Text, it.Font ?? Font).Width + gap * 2);
                it.SetRectNoArr(0, new Rectangle(rect.X + x, rect.Y, size, rect.Height), icon_size, gap);
                if (it.Visible) x += size;
            }
        }

        void ChangeUTitle(MenuItemCollection items)
        {
            foreach (MenuItem it in items)
            {
                var rect = it.Rect;
                it.ico_rect = new Rectangle(rect.X + (rect.Width - it.ico_rect.Width) / 2, it.ico_rect.Y, it.ico_rect.Width, it.ico_rect.Height);
                if (it.Visible && it.CanExpand) ChangeUTitle(it.Sub);
            }
        }

        #endregion

        #region 渲染

        internal ScrollBar scroll;
        public Menu() { scroll = new ScrollBar(this); }
        protected override void OnPaint(PaintEventArgs e)
        {
            if (items == null || items.Count == 0) return;
            var rect = ClientRectangle;
            if (rect.Width == 0 || rect.Height == 0) return;
            var g = e.Graphics.High();
            int sy = scroll.Value;
            g.TranslateTransform(0, -sy);
            Color scroll_color, color_fore, color_fore_active, fore_enabled, back_hover, back_active;

            switch (theme)
            {
                case TAMode.Light:
                    scroll_color = Color.Black;
                    fore_enabled = Style.rgba(0, 0, 0, 0.25F);
                    color_fore = fore ?? Color.Black;
                    color_fore_active = ForeActive ?? "#1677FF".ToColor();
                    back_hover = BackHover ?? Style.rgba(0, 0, 0, 0.06F);
                    back_active = BackActive ?? "#E6F4FF".ToColor();
                    break;
                case TAMode.Dark:
                    scroll_color = Color.White;
                    fore_enabled = Style.rgba(255, 255, 255, 0.25F);
                    color_fore = fore ?? Style.rgba(255, 255, 255, 0.85F);
                    back_hover = color_fore_active = ForeActive ?? Color.White;
                    back_active = BackActive ?? "#1668DC".ToColor();
                    break;
                default:
                    scroll_color = Style.Db.TextBase;
                    fore_enabled = Style.Db.TextQuaternary;
                    if (Config.IsDark)
                    {
                        color_fore = fore ?? Style.Db.Text;
                        back_hover = color_fore_active = ForeActive ?? Style.Db.TextBase;
                        back_active = BackActive ?? Style.Db.Primary;
                    }
                    else
                    {
                        color_fore = fore ?? Style.Db.TextBase;
                        color_fore_active = ForeActive ?? Style.Db.Primary;
                        back_hover = BackHover ?? Style.Db.FillSecondary;
                        back_active = BackActive ?? Style.Db.PrimaryBg;
                    }
                    break;
            }
            float _radius = radius * Config.Dpi;
            using (var sub_bg = new SolidBrush(Style.Db.FillQuaternary))
            {
                PaintItems(g, rect, sy, items, color_fore, color_fore_active, fore_enabled, back_hover, back_active, _radius, sub_bg);
            }
            g.ResetTransform();
            scroll.Paint(g, scroll_color);
            this.PaintBadge(g);
            base.OnPaint(e);
        }

        void PaintItems(Graphics g, Rectangle rect, float sy, MenuItemCollection items, Color fore, Color fore_active, Color fore_enabled, Color back_hover, Color back_active, float radius, SolidBrush sub_bg)
        {
            foreach (MenuItem it in items)
            {
                it.show = it.Show && it.Visible && it.rect.Y > sy - rect.Height - (it.Expand ? it.SubHeight : 0) && it.rect.Bottom < sy + rect.Height + it.rect.Height;
                if (it.show)
                {
                    PaintIt(g, it, fore, fore_active, fore_enabled, back_hover, back_active, radius);
                    if (!collapsed && it.Expand && it.Sub != null && it.Sub.Count > 0)
                    {
                        if (ShowSubBack) g.FillRectangle(sub_bg, new RectangleF(rect.X, it.SubY, rect.Width, it.SubHeight));
                        PaintItemExpand(g, rect, sy, it.Sub, fore, fore_active, fore_enabled, back_hover, back_active, radius);
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
        void PaintItemExpand(Graphics g, Rectangle rect, float sy, MenuItemCollection items, Color fore, Color fore_active, Color fore_enabled, Color back_hover, Color back_active, float radius)
        {
            foreach (MenuItem it in items)
            {
                it.show = it.Show && it.Visible && it.rect.Y > sy - rect.Height - (it.Expand ? it.SubHeight : 0) && it.rect.Bottom < sy + rect.Height + it.rect.Height;
                if (it.show)
                {
                    PaintIt(g, it, fore, fore_active, fore_enabled, back_hover, back_active, radius);
                    if (it.Expand && it.Sub != null && it.Sub.Count > 0)
                    {
                        PaintItemExpand(g, rect, sy, it.Sub, fore, fore_active, fore_enabled, back_hover, back_active, radius);
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

        void PaintIt(Graphics g, MenuItem it, Color fore, Color fore_active, Color fore_enabled, Color back_hover, Color back_active, float radius)
        {
            if (collapsed) PaintItemMini(g, it, fore, fore_active, fore_enabled, back_hover, back_active, radius);
            else PaintItem(g, it, fore, fore_active, fore_enabled, back_hover, back_active, radius);
        }

        void PaintItemMini(Graphics g, MenuItem it, Color fore, Color fore_active, Color fore_enabled, Color back_hover, Color back_active, float radius)
        {
            if (it.Enabled)
            {
                if (Config.IsDark || theme == TAMode.Dark)
                {
                    if (it.Select)
                    {
                        PaintBack(g, back_active, it.rect, radius);
                        PaintIcon(g, it, fore_active);
                    }
                    else
                    {
                        if (it.AnimationHover)
                        {
                            PaintIcon(g, it, fore);
                            PaintIcon(g, it, Helper.ToColorN(it.AnimationHoverValue, back_hover));
                        }
                        else if (it.Hover) PaintIcon(g, it, back_hover);
                        else PaintIcon(g, it, fore);
                    }
                }
                else
                {
                    if (it.Select)
                    {
                        PaintBack(g, back_active, it.rect, radius);
                        PaintIcon(g, it, fore_active);
                    }
                    else
                    {
                        if (it.AnimationHover) PaintBack(g, Helper.ToColorN(it.AnimationHoverValue, back_hover), it.rect, radius);
                        else if (it.Hover) PaintBack(g, back_hover, it.rect, radius);
                        PaintIcon(g, it, fore);
                    }
                }
            }
            else
            {
                if (it.Select) PaintBack(g, back_active, it.rect, radius);
                PaintIcon(g, it, fore_enabled);
            }
        }

        void PaintItem(Graphics g, MenuItem it, Color fore, Color fore_active, Color fore_enabled, Color back_hover, Color back_active, float radius)
        {
            if (it.Enabled)
            {
                if (Config.IsDark || theme == TAMode.Dark)
                {
                    if (it.Select)
                    {
                        if (it.CanExpand) PaintTextIconExpand(g, it, fore_active);
                        else
                        {
                            PaintBack(g, back_active, it.rect, radius);
                            PaintTextIcon(g, it, fore_active);
                        }
                    }
                    else
                    {
                        if (it.AnimationHover)
                        {
                            PaintTextIconExpand(g, it, fore);
                            PaintTextIconExpand(g, it, Helper.ToColorN(it.AnimationHoverValue, back_hover));
                        }
                        else if (it.Hover) PaintTextIconExpand(g, it, back_hover);
                        else PaintTextIconExpand(g, it, fore);
                    }
                }
                else
                {
                    if (it.Select)
                    {
                        if (it.CanExpand) PaintTextIconExpand(g, it, fore_active);
                        else
                        {
                            PaintBack(g, back_active, it.rect, radius);
                            PaintTextIcon(g, it, fore_active);
                        }
                    }
                    else
                    {
                        if (it.AnimationHover) PaintBack(g, Helper.ToColorN(it.AnimationHoverValue, back_hover), it.rect, radius);
                        else if (it.Hover) PaintBack(g, back_hover, it.rect, radius);
                        PaintTextIconExpand(g, it, fore);
                    }
                }
            }
            else
            {
                if (it.Select)
                {
                    if (it.CanExpand)
                    {
                        using (var pen = new Pen(fore_active, 2F))
                        {
                            pen.StartCap = pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                            g.DrawLines(pen, it.arr_rect.TriangleLines(it.ArrowProg, .4F));
                        }
                    }
                    else PaintBack(g, back_active, it.rect, radius);
                }
                else if (it.CanExpand)
                {
                    using (var pen = new Pen(fore_enabled, 2F))
                    {
                        pen.StartCap = pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                        g.DrawLines(pen, it.arr_rect.TriangleLines(it.ArrowProg, .4F));
                    }
                }
                PaintTextIcon(g, it, fore_enabled);
            }
        }

        readonly StringFormat SL = Helper.SF_ALL(lr: StringAlignment.Near);
        void PaintTextIcon(Graphics g, MenuItem it, Color fore)
        {
            using (var brush = new SolidBrush(fore))
            {
                g.DrawStr(it.Text, it.Font ?? Font, brush, it.txt_rect, SL);
            }
            PaintIcon(g, it, fore);
        }
        void PaintTextIconExpand(Graphics g, MenuItem it, Color fore)
        {
            if (it.CanExpand)
            {
                if (mode == TMenuMode.Inline)
                {
                    using (var pen = new Pen(fore, 2F))
                    {
                        pen.StartCap = pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                        g.DrawLines(pen, it.arr_rect.TriangleLines(it.ArrowProg, .4F));
                    }
                }
                else if (mode == TMenuMode.Vertical)
                {
                    using (var pen = new Pen(fore, 2F))
                    {
                        pen.StartCap = pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                        g.DrawLines(pen, TAlignMini.Right.TriangleLines(it.arr_rect, .4F));
                    }
                }
            }
            using (var brush = new SolidBrush(fore))
            {
                g.DrawStr(it.Text, it.Font ?? Font, brush, it.txt_rect, SL);
            }
            PaintIcon(g, it, fore);
        }
        void PaintIcon(Graphics g, MenuItem it, Color fore)
        {
            if (it.Icon != null) g.DrawImage(it.Icon, it.ico_rect);
            else if (it.IconSvg != null)
            {
                using (var _bmp = SvgExtend.GetImgExtend(it.IconSvg, it.ico_rect, fore))
                {
                    if (_bmp != null) g.DrawImage(_bmp, it.ico_rect);
                }
            }
        }

        void PaintBack(Graphics g, Color color, Rectangle rect, float radius)
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
            if (scroll.MouseDown(e.Location))
            {
                if (items == null || items.Count == 0) return;
                foreach (MenuItem it in items)
                {
                    var list = new List<MenuItem> { it };
                    if (IMouseDown(it, list, e.Location)) return;
                }
            }
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            scroll.MouseUp();
        }

        bool IMouseDown(MenuItem item, List<MenuItem> list, Point point)
        {
            if (item.Visible)
            {
                bool can = item.CanExpand;
                if (item.Enabled && item.Contains(point, 0, scroll.Value, out _))
                {
                    if (can) item.Expand = !item.Expand;
                    else
                    {
                        IUSelect();
                        if (list.Count > 1)
                        {
                            foreach (MenuItem it in list) it.Select = true;
                        }
                        item.Select = true;
                        OnSelectIndexChanged(item);
                        Invalidate();
                    }
                    return true;
                }
                if (can && item.Expand && !collapsed)
                {
                    foreach (MenuItem sub in item.Sub)
                    {
                        var list_ = new List<MenuItem>(list.Count + 1);
                        list_.AddRange(list);
                        list_.Add(sub);
                        if (IMouseDown(sub, list_, point)) return true;
                    }
                }
            }
            return false;
        }

        int hoveindexold = -1;
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (scroll.MouseMove(e.Location))
            {
                if (items == null || items.Count == 0) return;
                int count = 0, hand = 0;
                if (collapsed)
                {
                    int i = 0, hoveindex = -1;
                    foreach (MenuItem it in items)
                    {
                        if (it.show)
                        {
                            if (it.Contains(e.Location, 0, scroll.Value, out var change))
                            {
                                hoveindex = i;
                                hand++;
                            }
                            if (change) count++;
                        }
                        i++;
                    }
                    if (hoveindex != hoveindexold)
                    {
                        hoveindexold = hoveindex;

                        subForm?.Close();
                        subForm = null;
                        tooltipForm?.Close();
                        tooltipForm = null;
                        if (hoveindex > -1)
                        {
                            var _rect = RectangleToScreen(ClientRectangle);
                            var it = items[hoveindex];
                            if (it == null) return;
                            var Rect = it.Rect;
                            var rect = new Rectangle(_rect.X + Rect.X, _rect.Y + Rect.Y, Rect.Width, Rect.Height);
                            if (it.Sub != null && it.Sub.Count > 0)
                            {
                                select_x = 0;
                                subForm = new LayeredFormMenuDown(this, radius, rect, it.Sub);
                                subForm.Show(this);
                            }
                            else
                            {
                                if (it.Text != null)
                                {
                                    if (tooltipForm == null)
                                    {
                                        tooltipForm = new TooltipForm(this, rect, it.Text, new TooltipConfig
                                        {
                                            Font = it.Font ?? Font,
                                            ArrowAlign = TAlign.Right,
                                        });
                                        tooltipForm.Show(this);
                                    }
                                    else tooltipForm.SetText(rect, it.Text);
                                }
                            }
                        }
                    }
                }
                else if (mode == TMenuMode.Inline)
                {
                    foreach (MenuItem it in items) IMouseMove(it, e.Location, ref count, ref hand);
                }
                else
                {
                    int i = 0, hoveindex = -1;
                    foreach (MenuItem it in items)
                    {
                        if (it.show)
                        {
                            if (it.Contains(e.Location, 0, scroll.Value, out var change))
                            {
                                hoveindex = i;
                                hand++;
                            }
                            if (change) count++;
                        }
                        i++;
                    }
                    if (hoveindex != hoveindexold)
                    {
                        hoveindexold = hoveindex;

                        subForm?.Close();
                        subForm = null;
                        tooltipForm?.Close();
                        tooltipForm = null;
                        if (hoveindex > -1)
                        {
                            var _rect = RectangleToScreen(ClientRectangle);
                            var it = items[hoveindex];
                            if (it == null) return;
                            var Rect = it.Rect;
                            var rect = new Rectangle(_rect.X + Rect.X, _rect.Y + Rect.Y, Rect.Width, Rect.Height);
                            if (it.Sub != null && it.Sub.Count > 0)
                            {
                                select_x = 0;
                                subForm = new LayeredFormMenuDown(this, radius, rect, it.Sub);
                                subForm.Show(this);
                            }
                        }
                    }
                }
                SetCursor(hand > 0);
                if (count > 0) Invalidate();
            }
            else ILeave();
        }

        void IMouseMove(MenuItem it, Point point, ref int count, ref int hand)
        {
            if (it.show)
            {
                if (it.Contains(point, 0, scroll.Value, out var change))
                {
                    hand++;
                }
                if (change) count++;
                if (it.Sub != null && it.Sub.Count > 0) foreach (MenuItem sub in it.Sub) IMouseMove(sub, point, ref count, ref hand);
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            hoveindexold = -1;
            tooltipForm?.Close();
            tooltipForm = null;
            scroll.Leave();
            ILeave();
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            scroll.Leave();
            ILeave();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            scroll.MouseWheel(e.Delta);
            base.OnMouseWheel(e);
        }

        void ILeave()
        {
            SetCursor(false);
            if (items == null || items.Count == 0) return;
            int count = 0;
            foreach (MenuItem it in items) ILeave(it, ref count);
            if (count > 0) Invalidate();
        }
        void ILeave(MenuItem it, ref int count)
        {
            if (it.Hover) count++;
            it.Hover = false;
            if (it.Sub != null && it.Sub.Count > 0) foreach (MenuItem sub in it.Sub) ILeave(sub, ref count);
        }

        void IUSelect()
        {
            foreach (MenuItem it in Items) IUSelect(it);
        }
        void IUSelect(MenuItem it)
        {
            it.Select = false;
            if (it.Sub != null && it.Sub.Count > 0) foreach (MenuItem sub in it.Sub) IUSelect(sub);
        }

        #endregion

        #region 子窗口

        TooltipForm? tooltipForm = null;
        ILayeredForm? subForm = null;
        public ILayeredForm? SubForm() => subForm;
        internal int select_x = 0;

        internal void DropDownChange(MenuItem value)
        {
            select_x = 0;
            subForm = null;
            IUSelect();
            if (items == null || items.Count == 0) return;
            foreach (MenuItem it in items)
            {
                var list = new List<MenuItem> { it };
                if (IDropDownChange(it, list, value)) return;
            }
            Invalidate();
        }
        bool IDropDownChange(MenuItem item, List<MenuItem> list, MenuItem value)
        {
            bool can = item.CanExpand;
            if (item.Enabled && item == value)
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
            if (can)
            {
                foreach (MenuItem sub in item.Sub)
                {
                    var list_ = new List<MenuItem>(list.Count + 1);
                    list_.AddRange(list);
                    list_.Add(sub);
                    if (IDropDownChange(sub, list_, value)) return true;
                }
            }
            return false;
        }


        #endregion

        protected override void Dispose(bool disposing)
        {
            scroll.Dispose();
            base.Dispose(disposing);
        }
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

    public class MenuItem : NotifyProperty
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
        public MenuItem(string text, string? icon_svg)
        {
            Text = text;
            IconSvg = icon_svg;
        }

        #region 图标

        Image? icon = null;
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

        string? iconSvg = null;
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
        internal bool HasIcon
        {
            get => iconSvg != null || icon != null;
        }

        #endregion

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

        /// <summary>
        /// 自定义字体
        /// </summary>
        [Description("自定义字体"), Category("外观"), DefaultValue(null)]
        public Font? Font { get; set; }

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
        /// 用户定义数据
        /// </summary>
        [Description("用户定义数据"), Category("数据"), DefaultValue(null)]
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
                if (expand == value) return;
                expand = value;
                if (items != null && items.Count > 0)
                {
                    if (value && PARENT != null && PARENT.Unique)
                    {
                        if (PARENTITEM == null)
                        {
                            foreach (var it in PARENT.Items)
                            {
                                if (it != this) it.Expand = false;
                            }
                        }
                        else
                        {
                            foreach (var it in PARENTITEM.Sub)
                            {
                                if (it != this) it.Expand = false;
                            }
                        }
                    }
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
                        ExpandProg = 1F;
                        ArrowProg = value ? 1F : -1F;
                        Invalidates();
                    }
                }
                else
                {
                    expand = false;
                    ExpandProg = 1F;
                    ArrowProg = -1F;
                    Invalidates();
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
        public Rectangle Rect
        {
            get
            {
                if (PARENT == null) return rect;
                int y = PARENT.scroll.Value;
                if (y != 0F) return new Rectangle(rect.X, rect.Y - y, rect.Width, rect.Height);
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
            if (PARENT == null) return;
            PARENT.ChangeList();
            PARENT.Invalidate();
        }

        internal float SubY { get; set; }
        internal float SubHeight { get; set; }

        internal int ExpandHeight { get; set; }
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
        public bool Select { get; set; }
        internal int Depth { get; set; }
        internal float ArrowProg { get; set; } = 1F;
        internal Menu? PARENT { get; set; }
        public MenuItem? PARENTITEM { get; set; }

        internal void SetRect(int depth, bool indent, Rectangle _rect, int icon_size, int gap)
        {
            Depth = depth;
            rect = _rect;
            if (HasIcon)
            {
                if (indent || depth > 1)
                {
                    ico_rect = new Rectangle(_rect.X + (gap * (depth + 1)), _rect.Y + (_rect.Height - icon_size) / 2, icon_size, icon_size);
                    txt_rect = new Rectangle(ico_rect.X + ico_rect.Width + gap, _rect.Y, _rect.Width - (ico_rect.Width + gap * 2), _rect.Height);
                }
                else
                {
                    ico_rect = new Rectangle(_rect.X + gap, _rect.Y + (_rect.Height - icon_size) / 2, icon_size, icon_size);
                    txt_rect = new Rectangle(ico_rect.X + ico_rect.Width + gap, _rect.Y, _rect.Width - (ico_rect.Width + gap * 2), _rect.Height);
                }
                arr_rect = new Rectangle(_rect.Right - ico_rect.Height - (int)(ico_rect.Height * 0.9F), _rect.Y + (_rect.Height - ico_rect.Height) / 2, ico_rect.Height, ico_rect.Height);
            }
            else
            {
                if (indent || depth > 1) txt_rect = new Rectangle(_rect.X + (gap * (depth + 1)), _rect.Y, _rect.Width - (gap * 2), _rect.Height);
                else txt_rect = new Rectangle(_rect.X + gap, _rect.Y, _rect.Width - (gap * 2), _rect.Height);
                arr_rect = new Rectangle(_rect.Right - icon_size - (int)(icon_size * 0.9F), _rect.Y + (_rect.Height - icon_size) / 2, icon_size, icon_size);
            }
            Show = true;
        }
        internal void SetRectNoArr(int depth, Rectangle _rect, int icon_size, int gap)
        {
            Depth = depth;
            rect = _rect;
            if (HasIcon)
            {
                ico_rect = new Rectangle(_rect.X + gap, _rect.Y + (_rect.Height - icon_size) / 2, icon_size, icon_size);
                txt_rect = new Rectangle(ico_rect.X + ico_rect.Width + gap, _rect.Y, _rect.Width - (ico_rect.Width + gap * 2), _rect.Height);
            }
            else txt_rect = new Rectangle(_rect.X + gap, _rect.Y, _rect.Width - (gap * 2), _rect.Height);
            Show = true;
        }
        internal Rectangle rect { get; set; }
        internal Rectangle arr_rect { get; set; }

        internal bool Contains(Point point, int x, int y, out bool change)
        {
            if (rect.Contains(point.X + x, point.Y + y))
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

        internal Rectangle txt_rect { get; set; }
        internal Rectangle ico_rect { get; set; }
    }
}