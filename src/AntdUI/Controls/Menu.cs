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
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
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
                OnPropertyChanged(nameof(Radius));
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
                OnPropertyChanged(nameof(Round));
            }
        }

        /// <summary>
        /// 色彩模式
        /// </summary>
        [Obsolete("use ColorScheme"), Description("色彩模式"), Category("外观"), DefaultValue(TAMode.Auto)]
        public TAMode Theme
        {
            get => ColorScheme;
            set => ColorScheme = value;
        }

        /// <summary>
        /// 间距
        /// </summary>
        [Description("间距"), Category("外观"), DefaultValue(null)]
        public int? Gap { get; set; }

        float iconratio = 1.2F;
        /// <summary>
        /// 图标比例
        /// </summary>
        [Description("图标比例"), Category("外观"), DefaultValue(1.2F)]
        public float IconRatio
        {
            get => iconratio;
            set
            {
                if (iconratio == value) return;
                iconratio = value;
                if (IsHandleCreated)
                {
                    ChangeList();
                    Invalidate();
                }
                OnPropertyChanged(nameof(IconRatio));
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
                if (IsHandleCreated)
                {
                    ChangeList();
                    Invalidate();
                }
                OnPropertyChanged(nameof(Mode));
            }
        }

        /// <summary>
        /// 触发下拉的行为
        /// </summary>
        [Description("触发下拉的行为"), Category("行为"), DefaultValue(Trigger.Hover)]
        public Trigger Trigger { get; set; } = Trigger.Hover;

        /// <summary>
        /// 常规缩进
        /// </summary>
        [Description("常规缩进"), Category("外观"), DefaultValue(false)]
        public bool Indent { get; set; }

        bool unique = false;
        /// <summary>
        /// 只保持一个子菜单的展开
        /// </summary>
        [Description("只保持一个子菜单的展开"), Category("外观"), DefaultValue(false)]
        public bool Unique
        {
            get => unique;
            set
            {
                if (unique == value) return;
                unique = value;
                if (unique) UniqueHand(items);
            }
        }

        void UniqueHand(MenuItemCollection? items)
        {
            if (items == null) return;
            foreach (var item in items)
            {
                if (item.Expand)
                {
                    UniqueHand(item.Sub);
                    foreach (var it in items)
                    {
                        if (item == it) continue;
                        it.Expand = false;
                    }
                    return;
                }
            }
        }

        /// <summary>
        /// 显示子菜单背景
        /// </summary>
        [Description("显示子菜单背景"), Category("外观"), DefaultValue(false)]
        public bool ShowSubBack { get; set; }

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
                if (IsHandleCreated)
                {
                    ChangeList();
                    Invalidate();
                }
                OnPropertyChanged(nameof(Collapsed));
            }
        }

        /// <summary>
        /// 超出文字提示配置
        /// </summary>
        [Browsable(false)]
        [Description("超出文字提示配置"), Category("行为"), DefaultValue(null)]
        public TooltipConfig? TooltipConfig { get; set; }

        /// <summary>
        /// 鼠标右键控制
        /// </summary>
        [Description("鼠标右键控制"), Category("交互"), DefaultValue(true)]
        public bool MouseRightCtrl { get; set; } = true;

        #region 集合操作

        public void SelectIndex(int i1, bool focus = true)
        {
            if (items == null || items.Count == 0) return;
            IUSelect(items);
            if (items.ListExceed(i1))
            {
                Invalidate(); return;
            }
            var it1 = items[i1];
            it1.Select = true;
            OnSelectIndexChanged(it1);
            if (focus && ScrollBar.ShowY) ScrollBar.ValueY = it1.rect.Y;
            Invalidate();
        }
        public void SelectIndex(int i1, int i2, bool focus = true)
        {
            if (items == null || items.Count == 0) return;
            IUSelect(items);
            if (items.ListExceed(i1))
            {
                Invalidate(); return;
            }
            var it1 = items[i1];
            if (it1.items.ListExceed(i2))
            {
                Invalidate(); return;
            }
            var it2 = it1.Sub[i2];
            it1.Select = it2.Select = true;
            OnSelectIndexChanged(it2);
            if (focus && ScrollBar.ShowY) ScrollBar.ValueY = it2.rect.Y;
            Invalidate();
        }
        public void SelectIndex(int i1, int i2, int i3, bool focus = true)
        {
            if (items == null || items.Count == 0) return;
            IUSelect(items);
            if (items.ListExceed(i1))
            {
                Invalidate();
                return;
            }
            var it1 = items[i1];
            if (it1.items.ListExceed(i2))
            {
                Invalidate(); return;
            }
            var it2 = it1.Sub[i2];
            if (it2.items.ListExceed(i3))
            {
                Invalidate();
                return;
            }
            var it3 = it2.Sub[i3];
            it1.Select = it2.Select = it3.Select = true;
            OnSelectIndexChanged(it3);
            if (focus && ScrollBar.ShowY) ScrollBar.ValueY = it3.rect.Y;
            Invalidate();
        }

        /// <summary>
        /// 获取选中项索引
        /// </summary>
        public int GetSelectIndex(MenuItem item)
        {
            if (items != null) return items.IndexOf(item);
            else return -1;
        }

        /// <summary>
        /// 选中菜单
        /// </summary>
        /// <param name="item">项</param>
        /// <param name="focus">设置焦点</param>
        public void Select(MenuItem item, bool focus = true)
        {
            if (items == null || items.Count == 0) return;
            IUSelect(items);
            Select(item, focus, items);
        }
        void Select(MenuItem item, bool focus, MenuItemCollection items)
        {
            foreach (var it in items)
            {
                if (it == item)
                {
                    it.Select = true;
                    tmpAM = true;
                    OnSelectIndexChanged(it);
                    if (SelectEx(it.PARENTITEM) > 0)
                    {
                        ChangeList();
                        Invalidate();
                    }
                    tmpAM = false;
                    if (focus && ScrollBar.ShowY) ScrollBar.ValueY = it.rect.Y;
                    return;
                }
                else if (it.items != null && it.items.Count > 0) Select(item, focus, it.items);
            }
        }

        internal bool tmpAM = false;
        int SelectEx(MenuItem? it)
        {
            int count = 0;
            if (it == null) return count;
            if (it.Expand) it.Select = true;
            else
            {
                count++;
                it.Expand = it.Select = true;
            }
            count += SelectEx(it.PARENTITEM);
            return count;
        }

        /// <summary>
        /// 移除菜单
        /// </summary>
        /// <param name="item">项</param>
        public void Remove(MenuItem item)
        {
            if (items == null || items.Count == 0) return;
            Remove(item, items);
        }
        void Remove(MenuItem item, MenuItemCollection items)
        {
            foreach (var it in items)
            {
                if (it == item)
                {
                    items.Remove(it);
                    return;
                }
                else if (it.items != null && it.items.Count > 0) Remove(item, it.items);
            }
        }

        #region 事件

        /// <summary>
        /// Select 属性值更改时发生
        /// </summary>
        [Description("Select 属性值更改时发生"), Category("行为")]
        public event SelectEventHandler? SelectChanged = null;

        internal void OnSelectIndexChanged(MenuItem item) => SelectChanged?.Invoke(this, new MenuSelectEventArgs(item));

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
            ChangeList();
            var item = GetSelectItem(out var sub);
            if (item != null)
            {
                foreach (var it in sub) it.Select = true;
            }
            base.OnHandleCreated(e);
        }

        #region 获取选中项目

        public MenuItem? GetSelectItem()
        {
            var list = new List<MenuItem>(0);
            return GetSelectItem(ref list, items);
        }

        public MenuItem? GetSelectItem(out List<MenuItem> list)
        {
            list = new List<MenuItem>(0);
            return GetSelectItem(ref list, items);
        }

        MenuItem? GetSelectItem(ref List<MenuItem> list, MenuItemCollection? items)
        {
            if (items == null || items.Count == 0) return null;
            foreach (var it in items)
            {
                var list_ = new List<MenuItem>(list.Count + 1);
                list_.AddRange(list);
                list_.Add(it);
                var select = GetSelectItem(ref list_, it.Sub);
                if (select == null)
                {
                    if (it.Select)
                    {
                        list = list_;
                        return it;
                    }
                }
                else
                {
                    list = list_;
                    return select;
                }
            }
            return null;
        }

        #endregion

        protected override void OnFontChanged(EventArgs e)
        {
            ChangeList();
            base.OnFontChanged(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if (IsHandleCreated) ChangeList();
            base.OnSizeChanged(e);
        }

        int collapseWidth = 0, collapsedWidth = 0;
        /// <summary>
        /// 展开之前宽度
        /// </summary>
        public int CollapseWidth => collapseWidth;

        /// <summary>
        /// 展开后宽度
        /// </summary>
        public int CollapsedWidth => collapsedWidth;

        bool scroll_show = false, hover_r = false;
        Rectangle rect_r, rect_r_ico;
        internal void ChangeList()
        {
            var _rect = ClientRectangle;
            if (_rect.Width == 0 || _rect.Height == 0 || pauseLayout || items == null || items.Count == 0) return;
            var rect = _rect.PaddingRect(Padding);
            int x = 0, y = 0;
            int icon_count = 0;
            Helper.GDI(g =>
            {
                var size = g.MeasureString(Config.NullText, Font);
                int icon_size = (int)Math.Ceiling(size.Height * iconratio), gap = icon_size / 2, gapI = gap / 2, gapy = Gap == null ? gapI : (int)(Gap * Config.Dpi), height = size.Height + gap * 2;
                if (mode == TMenuMode.Horizontal)
                {
                    ChangeListHorizontal(rect, g, items, ref x, icon_size, gap, gapI);
                    scroll_show = x > rect.Width;
                    if (scroll_show)
                    {
                        rect_r = new Rectangle(rect.Right - rect.Height, rect.Y, rect.Height, rect.Height);
                        int ico_size = (int)(rect_r.Height * .6F), ico_xy = (rect_r.Height - ico_size) / 2;
                        rect_r_ico = new Rectangle(rect_r.X + ico_xy, rect_r.Y + ico_xy, ico_size, ico_size);
                    }
                }
                else
                {
                    scroll_show = false;
                    collapseWidth = icon_size * 2 + gap + gapI + Padding.Horizontal;
                    collapsedWidth = ChangeList(rect, g, null, items, ref y, ref icon_count, height, icon_size, gap, gapy, 0) + Padding.Horizontal;
                    if (AutoCollapse)
                    {
                        if (icon_count > 0) collapsed = collapsedWidth >= _rect.Width;
                        else collapsed = false;
                    }
                    if (collapsed) ChangeUTitle(items);
                }
            });
            ScrollBar.SetVrSize(y + Padding.Vertical);
            ScrollBar.SizeChange(_rect);
        }

        int ChangeList(Rectangle rect, Canvas g, MenuItem? Parent, MenuItemCollection items, ref int y, ref int icon_count, int height, int icon_size, int gap, int gapy, int depth)
        {
            int collapsedWidth = 0, i = 0;
            foreach (var it in items)
            {
                it.Index = i;
                i++;
                it.PARENT = this;
                it.PARENTITEM = Parent;
                if (it.HasIcon) icon_count++;
                it.SetRect(depth, Indent, new Rectangle(rect.X, rect.Y + y, rect.Width, height), icon_size, gap);
                if (it.Visible)
                {
                    int size = g.MeasureText(it.Text, it.Font ?? Font).Width + gap * 4 + icon_size + it.arr_rect.Width;
                    if (size > collapsedWidth) collapsedWidth = size;
                    y += height + gapy;
                    if (mode == TMenuMode.Inline && it.CanExpand)
                    {
                        if (!collapsed)
                        {
                            int y_item = y;

                            int size2 = ChangeList(rect, g, it, it.Sub, ref y, ref icon_count, height, icon_size, gap, gapy, depth + 1);
                            if (size2 > collapsedWidth) collapsedWidth = size2;

                            it.SubY = y_item - gapy / 2;
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
                            int size2 = ChangeList(rect, g, it, it.Sub, ref y, ref icon_count, height, icon_size, gap, gapy, depth + 1);
                            if (size2 > collapsedWidth) collapsedWidth = size2;
                            y = oldy;
                        }
                    }
                }
            }
            return collapsedWidth;
        }
        void ChangeListHorizontal(Rectangle rect, Canvas g, MenuItemCollection items, ref int x, int icon_size, int gap, int gapI)
        {
            int i = 0;
            foreach (var it in items)
            {
                it.Index = i;
                i++;
                it.PARENT = this;
                int size;
                if (it.HasIcon) size = g.MeasureText(it.Text, it.Font ?? Font).Width + gap * 3 + icon_size;
                else size = g.MeasureText(it.Text, it.Font ?? Font).Width + gap * 2;
                it.SetRectNoArr(0, new Rectangle(rect.X + x, rect.Y, size, rect.Height), icon_size, gap);
                if (it.Visible) x += size;
            }
        }

        void ChangeUTitle(MenuItemCollection items)
        {
            foreach (var it in items)
            {
                var rect = it.Rect;
                it.ico_rect = new Rectangle(rect.X + (rect.Width - it.ico_rect.Width) / 2, it.ico_rect.Y, it.ico_rect.Width, it.ico_rect.Height);
                if (it.Visible && it.CanExpand) ChangeUTitle(it.Sub);
            }
        }

        #endregion

        #region 渲染

        public Menu() { ScrollBar = new ScrollBar(this); }
        protected override void OnPaint(PaintEventArgs e)
        {
            var rect = ClientRectangle;
            if (rect.Width == 0 || rect.Height == 0) return;
            if (items == null || items.Count == 0)
            {
                base.OnPaint(e);
                return;
            }
            var g = e.Graphics.High();
            if (scroll_show) g.SetClip(new Rectangle(rect.X, rect.Y, rect_r.Right - rect_r.Height, rect.Height));
            int sy = ScrollBar.Value;
            g.TranslateTransform(0, -sy);
            Color scroll_color = Colour.TextBase.Get("Menu", ColorScheme), color_fore, color_fore_active, fore_enabled = Colour.TextQuaternary.Get("Menu", ColorScheme), back_hover, back_active;
            if (Config.IsDark || ColorScheme == TAMode.Dark)
            {
                color_fore = fore ?? Colour.Text.Get("Menu", ColorScheme);
                back_hover = color_fore_active = ForeActive ?? Colour.TextBase.Get("Menu", ColorScheme);
                back_active = BackActive ?? Colour.Primary.Get("Menu", ColorScheme);
            }
            else
            {
                color_fore = fore ?? Colour.TextBase.Get("Menu", ColorScheme);
                color_fore_active = ForeActive ?? Colour.Primary.Get("Menu", ColorScheme);
                back_hover = BackHover ?? Colour.FillSecondary.Get("Menu", ColorScheme);
                back_active = BackActive ?? Colour.PrimaryBg.Get("Menu", ColorScheme);
            }
            float _radius = radius * Config.Dpi;
            using (var sub_bg = new SolidBrush(Colour.FillQuaternary.Get("Menu", ColorScheme)))
            {
                PaintItems(g, rect, sy, items, color_fore, color_fore_active, fore_enabled, back_hover, back_active, _radius, sub_bg);
            }
            g.ResetTransform();
            if (scroll_show)
            {
                g.ResetClip();
                if (hover_r)
                {
                    using (var path = Helper.RoundPath(rect_r, _radius))
                    {
                        g.Fill(back_hover, path);
                    }
                }
                SvgExtend.GetImgExtend(g, "EllipsisOutlined", rect_r_ico, color_fore);
            }
            ScrollBar.Paint(g, scroll_color);
            this.PaintBadge(g);
            base.OnPaint(e);
        }

        void PaintItems(Canvas g, Rectangle rect, int sy, MenuItemCollection items, Color fore, Color fore_active, Color fore_enabled, Color back_hover, Color back_active, float radius, SolidBrush sub_bg)
        {
            foreach (var it in items)
            {
                it.show = it.Show && it.Visible && it.rect.Y > sy - rect.Height - (it.Expand ? it.SubHeight : 0) && it.rect.Bottom < sy + rect.Height + it.rect.Height;
                if (it.show)
                {
                    PaintIt(g, it, fore, fore_active, fore_enabled, back_hover, back_active, radius);
                    if (!collapsed && (it.Expand || it.ExpandThread) && it.items != null && it.items.Count > 0)
                    {
                        if (ShowSubBack) g.Fill(sub_bg, new RectangleF(rect.X, it.SubY, rect.Width, it.SubHeight));
                        var state = g.Save();
                        if (it.ExpandThread) g.SetClip(new RectangleF(rect.X, it.rect.Bottom, rect.Width, it.ExpandHeight * it.ExpandProg));
                        PaintItemExpand(g, rect, sy, it.items, fore, fore_active, fore_enabled, back_hover, back_active, radius);
                        g.Restore(state);
                    }
                }
            }
        }
        void PaintItemExpand(Canvas g, Rectangle rect, float sy, MenuItemCollection items, Color fore, Color fore_active, Color fore_enabled, Color back_hover, Color back_active, float radius)
        {
            foreach (var it in items)
            {
                it.show = it.Show && it.Visible && it.rect.Y > sy - rect.Height - (it.Expand ? it.SubHeight : 0) && it.rect.Bottom < sy + rect.Height + it.rect.Height;
                if (it.show)
                {
                    PaintIt(g, it, fore, fore_active, fore_enabled, back_hover, back_active, radius);
                    if (it.Expand && it.items != null && it.items.Count > 0)
                    {
                        PaintItemExpand(g, rect, sy, it.items, fore, fore_active, fore_enabled, back_hover, back_active, radius);
                        if (it.ExpandThread)
                        {
                            using (var brush = new SolidBrush(BackColor))
                            {
                                g.Fill(brush, new RectangleF(rect.X, it.rect.Bottom + it.ExpandHeight * it.ExpandProg, rect.Width, it.ExpandHeight));
                            }
                        }
                    }
                }
            }
        }

        void PaintIt(Canvas g, MenuItem it, Color fore, Color fore_active, Color fore_enabled, Color back_hover, Color back_active, float radius)
        {
            if (collapsed) PaintItemMini(g, it, fore, fore_active, fore_enabled, back_hover, back_active, radius);
            else PaintItem(g, it, fore, fore_active, fore_enabled, back_hover, back_active, radius);
            it.PaintBadge(Font, it.rect, g, ColorScheme);
        }

        void PaintItemMini(Canvas g, MenuItem it, Color fore, Color fore_active, Color fore_enabled, Color back_hover, Color back_active, float radius)
        {
            if (it.Enabled)
            {
                if (Config.IsDark || ColorScheme == TAMode.Dark)
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

        void PaintItem(Canvas g, MenuItem it, Color fore, Color fore_active, Color fore_enabled, Color back_hover, Color back_active, float radius)
        {
            if (it.Enabled)
            {
                if (Config.IsDark || ColorScheme == TAMode.Dark)
                {
                    if (it.Select)
                    {
                        if (it.CanExpand)
                        {
                            if (mode == TMenuMode.Horizontal || mode == TMenuMode.Vertical) PaintBack(g, back_active, it.rect, radius);
                            PaintTextIconExpand(g, it, fore_active);
                        }
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
                        if (it.CanExpand)
                        {
                            if (mode == TMenuMode.Horizontal || mode == TMenuMode.Vertical) PaintBack(g, back_active, it.rect, radius);
                            PaintTextIconExpand(g, it, fore_active);
                        }
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
                        if (mode == TMenuMode.Horizontal || mode == TMenuMode.Vertical) PaintBack(g, back_active, it.rect, radius);
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
        void PaintTextIcon(Canvas g, MenuItem it, Color fore)
        {
            using (var brush = new SolidBrush(fore))
            {
                g.DrawText(it.Text, it.Font ?? Font, brush, it.txt_rect, SL);
            }
            PaintIcon(g, it, fore);
        }
        void PaintTextIconExpand(Canvas g, MenuItem it, Color fore)
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
                g.DrawText(it.Text, it.Font ?? Font, brush, it.txt_rect, SL);
            }
            PaintIcon(g, it, fore);
        }
        void PaintIcon(Canvas g, MenuItem it, Color fore)
        {
            if (it.Select)
            {
                int count = 0;
                if (it.IconActive != null)
                {
                    g.Image(it.IconActive, it.ico_rect); count++;
                }
                if (it.IconActiveSvg != null)
                {
                    if (g.GetImgExtend(it.IconActiveSvg, it.ico_rect, fore)) count++;
                }
                if (count > 0) return;
            }


            if (it.Icon != null) g.Image(it.Icon, it.ico_rect);
            if (it.IconSvg != null) g.GetImgExtend(it.IconSvg, it.ico_rect, fore);
        }

        void PaintBack(Canvas g, Color color, Rectangle rect, float radius)
        {
            if (Round || radius > 0)
            {
                using (var path = rect.RoundPath(radius, Round))
                {
                    g.Fill(color, path);
                }
            }
            else g.Fill(color, rect);
        }

        #endregion

        #region 鼠标

        MenuItem? MDown = null;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Right && !MouseRightCtrl) return;
            if (ScrollBar.MouseDown(e.Location))
            {
                if (items == null || items.Count == 0) return;
                if (scroll_show)
                {
                    if (rect_r.Contains(e.X, e.Y)) return;
                }
                OnTouchDown(e.X, e.Y);
                foreach (var it in items)
                {
                    if (IMouseDown(items, it, e.X, e.Y)) return;
                }
            }
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button == MouseButtons.Right && !MouseRightCtrl) return;
            if (ScrollBar.MouseUp() && OnTouchUp())
            {
                if (items == null || items.Count == 0 || MDown == null) return;
                foreach (var it in items)
                {
                    var list = new List<MenuItem> { it };
                    if (IMouseUp(items, it, list, e.X, e.Y, MDown)) return;
                }
            }
        }

        bool IMouseDown(MenuItemCollection items, MenuItem item, int x, int y)
        {
            if (item.Visible)
            {
                bool can = item.CanExpand;
                if (item.Enabled && item.Contains(x, y, 0, ScrollBar.Value, out _))
                {
                    MDown = item;
                    return true;
                }
                if (can && item.Expand && !collapsed)
                {
                    foreach (var sub in item.Sub)
                    {
                        if (IMouseDown(items, sub, x, y)) return true;
                    }
                }
            }
            return false;
        }

        bool IMouseUp(MenuItemCollection items, MenuItem item, List<MenuItem> list, int x, int y, MenuItem MDown)
        {
            if (item.Visible)
            {
                bool can = item.CanExpand;
                if (MDown == item)
                {
                    if (item.Enabled && item.Contains(x, y, 0, ScrollBar.Value, out _))
                    {
                        if (can)
                        {
                            if ((mode == TMenuMode.Horizontal || mode == TMenuMode.Vertical) && Trigger == Trigger.Click && item.items != null && item.items.Count > 0)
                            {
                                if (subForm == null)
                                {
                                    var _rect = RectangleToScreen(ClientRectangle);
                                    var Rect = item.Rect;
                                    var rect = new Rectangle(_rect.X + Rect.X, _rect.Y + Rect.Y, Rect.Width, Rect.Height);
                                    select_x = 0;
                                    subForm = new LayeredFormMenuDown(this, radius, rect, item.items);
                                    subForm.Show(this);
                                }
                                else { subForm.IClose(); subForm = null; }
                            }
                            else item.Expand = !item.Expand;
                        }
                        else
                        {
                            IUSelect(items);
                            if (list.Count > 1)
                            {
                                foreach (var it in list) it.Select = true;
                            }
                            item.Select = true;
                            OnSelectIndexChanged(item);
                            Invalidate();
                        }
                    }
                    return true;
                }
                if (can && item.Expand && !collapsed)
                {
                    foreach (var sub in item.Sub)
                    {
                        var list_ = new List<MenuItem>(list.Count + 1);
                        list_.AddRange(list);
                        list_.Add(sub);
                        if (IMouseUp(items, sub, list_, x, y, MDown)) return true;
                    }
                }
            }
            return false;
        }

        int hoveindexold = -1;
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (ScrollBar.MouseMove(e.Location))
            {
                if (OnTouchMove(e.X, e.Y))
                {
                    if (items == null || items.Count == 0) return;
                    int count = 0, hand = 0;
                    if (scroll_show)
                    {
                        if (rect_r.Contains(e.X, e.Y))
                        {
                            if (!hover_r)
                            {
                                hover_r = true;
                                Invalidate();
                                tooltipForm?.Close();
                                tooltipForm = null;
                                subForm?.Close();
                                subForm = null;
                                var list = new List<MenuItem>(items.Count);
                                foreach (var it in items)
                                {
                                    if (it.Rect.X > (rect_r.X - it.Rect.Width)) list.Add(it);
                                }
                                var _rect = RectangleToScreen(ClientRectangle);
                                var rect = new Rectangle(_rect.X + rect_r.X, _rect.Y + rect_r.Y, rect_r.Width, rect_r.Height);
                                subForm = new LayeredFormMenuDown(this, radius, rect, list);
                                subForm.Show(this);
                            }
                            foreach (var it in items) it.Hover = false;
                            SetCursor(true);
                            return;
                        }
                        else
                        {
                            if (hover_r) count++;
                            hover_r = false;
                        }
                    }
                    if (collapsed)
                    {
                        int i = 0, hoveindex = -1;
                        foreach (var it in items)
                        {
                            if (it.show)
                            {
                                if (it.Contains(e.X, e.Y, 0, ScrollBar.Value, out var change))
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
                                if (it.items != null && it.items.Count > 0)
                                {
                                    select_x = 0;
                                    subForm = new LayeredFormMenuDown(this, radius, rect, it.items);
                                    subForm.Show(this);
                                }
                                else if (it.Text != null)
                                {
                                    if (tooltipForm == null)
                                    {
                                        tooltipForm = new TooltipForm(this, rect, it.Text, TooltipConfig ?? new TooltipConfig
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
                    else if (mode == TMenuMode.Inline)
                    {
                        foreach (var it in items) IMouseMove(it, e.X, e.Y, ref count, ref hand);
                    }
                    else
                    {
                        int i = 0, hoveindex = -1;
                        foreach (var it in items)
                        {
                            if (it.show)
                            {
                                if (it.Contains(e.X, e.Y, 0, ScrollBar.Value, out var change))
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
                                if (Trigger == Trigger.Hover && it.items != null && it.items.Count > 0)
                                {
                                    select_x = 0;
                                    subForm = new LayeredFormMenuDown(this, radius, rect, it.items);
                                    subForm.Show(this);
                                }
                            }
                        }
                    }
                    SetCursor(hand > 0);
                    if (count > 0) Invalidate();
                }
            }
            else ILeave();
        }

        void IMouseMove(MenuItem it, int x, int y, ref int count, ref int hand)
        {
            if (it.show)
            {
                if (it.Contains(x, y, 0, ScrollBar.Value, out var change))
                {
                    hand++;
                }
                if (change) count++;
                if (it.items != null && it.items.Count > 0) foreach (var sub in it.items) IMouseMove(sub, x, y, ref count, ref hand);
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            hoveindexold = -1;
            tooltipForm?.Close();
            tooltipForm = null;
            ScrollBar.Leave();
            ILeave();
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            ScrollBar.Leave();
            ILeave();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            ScrollBar.MouseWheel(e.Delta);
            base.OnMouseWheel(e);
        }
        protected override bool OnTouchScrollX(int value) => ScrollBar.MouseWheelXCore(value);
        protected override bool OnTouchScrollY(int value) => ScrollBar.MouseWheelYCore(value);

        void ILeave()
        {
            SetCursor(false);
            if (items == null || items.Count == 0) return;
            int count = 0;
            foreach (var it in items) ILeave(it, ref count);
            if (count > 0) Invalidate();
        }
        void ILeave(MenuItem it, ref int count)
        {
            if (it.Hover) count++;
            it.Hover = false;
            if (it.items != null && it.items.Count > 0) foreach (var sub in it.items) ILeave(sub, ref count);
        }

        /// <summary>
        /// 取消全部选择
        /// </summary>
        public void USelect()
        {
            if (items == null || items.Count == 0) return;
            IUSelect(items);
        }

        void IUSelect(MenuItemCollection items)
        {
            foreach (var it in items) IUSelect(it);
        }
        void IUSelect(MenuItem it)
        {
            it.Select = false;
            if (it.items != null && it.items.Count > 0) foreach (var sub in it.items) IUSelect(sub);
        }

        public MenuItem? HitTest(int x, int y)
        {
            if (items == null || items.Count == 0) return null;

            foreach (var it in items)
            {
                if (IHitTest(x, y, it, out var md)) return md;
            }
            return null;
        }
        bool IHitTest(int x, int y, MenuItem item, out MenuItem? mdown)
        {
            if (item.Visible)
            {
                bool can = item.CanExpand;
                if (item.Enabled && item.Contains(x, y, 0, ScrollBar.Value, out _))
                {
                    mdown = item;
                    return true;
                }
                if (can && item.Expand && !collapsed)
                {
                    foreach (var sub in item.Sub)
                    {
                        if (IHitTest(x, y, sub, out mdown)) return true;
                    }
                }
            }
            mdown = null;
            return false;
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
            if (items == null || items.Count == 0) return;
            IUSelect(items);
            foreach (var it in items)
            {
                var list = new List<MenuItem> { it };
                if (IDropDownChange(items, it, list, value)) return;
            }
            Invalidate();
        }
        bool IDropDownChange(MenuItemCollection items, MenuItem item, List<MenuItem> list, MenuItem value)
        {
            bool can = item.CanExpand;
            if (item.Enabled && item == value)
            {
                if (can) item.Expand = !item.Expand;
                else
                {
                    IUSelect(items);
                    if (list.Count > 1)
                    {
                        foreach (var it in list) it.Select = true;
                    }
                    item.Select = true;
                    OnSelectIndexChanged(item);
                    Invalidate();
                }
                return true;
            }
            if (can)
            {
                foreach (var sub in item.Sub)
                {
                    var list_ = new List<MenuItem>(list.Count + 1);
                    list_.AddRange(list);
                    list_.Add(sub);
                    if (IDropDownChange(items, sub, list_, value)) return true;
                }
            }
            return false;
        }


        #endregion

        protected override void Dispose(bool disposing)
        {
            ScrollBar.Dispose();
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

    public class MenuItem : BadgeConfig
    {
        public MenuItem() { }
        public MenuItem(string text)
        {
            Text = text;
        }
        public MenuItem(string text, Image? icon)
        {
            Text = text;
            Icon = icon;
        }
        public MenuItem(string text, string? icon_svg)
        {
            Text = text;
            IconSvg = icon_svg;
        }

        /// <summary>
        /// 序号
        /// </summary>
        public int Index { get; internal set; }

        /// <summary>
        /// ID
        /// </summary>
        [Description("ID"), Category("数据"), DefaultValue(null)]
        public string? ID { get; set; }

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
        internal bool HasIcon => !string.IsNullOrWhiteSpace(iconSvg) || icon != null;

        /// <summary>
        /// 图标激活
        /// </summary>
        [Description("图标激活"), Category("外观"), DefaultValue(null)]
        public Image? IconActive { get; set; }

        /// <summary>
        /// 图标激活SVG
        /// </summary>
        [Description("图标激活SVG"), Category("外观"), DefaultValue(null)]
        public string? IconActiveSvg { get; set; }

        #endregion

        string? text = null;
        /// <summary>
        /// 文本
        /// </summary>
        [Description("文本"), Category("外观"), DefaultValue(null), Localizable(true)]
        public string? Text
        {
            get => Localization.GetLangI(LocalizationText, text, new string?[] { "{id}", ID });
            set
            {
                if (text == value) return;
                text = value;
                Invalidates();
            }
        }

        [Description("文本"), Category("国际化"), DefaultValue(null)]
        public string? LocalizationText { get; set; }

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

        internal MenuItemCollection? items;
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
                    if (Config.HasAnimation(nameof(Menu)))
                    {
                        if (PARENT != null && PARENT.tmpAM)
                        {
                            ExpandProg = 1F;
                            ArrowProg = value ? 1F : -1F;
                            return;
                        }
                        ThreadExpand?.Dispose();
                        float oldval = -1;
                        if (ThreadExpand?.Tag is float oldv) oldval = oldv;
                        ExpandThread = true;
                        if (value)
                        {
                            int time = ExpandCount(this) * 10;
                            if (time > 1000) time = 1000;
                            int t = Animation.TotalFrames(10, time);
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
                foreach (var item in it.Sub)
                {
                    if (item.Expand) count += ExpandCount(item);
                }
            }
            return count;
        }

        [Description("是否可以展开"), Category("行为"), DefaultValue(false)]
        public bool CanExpand => visible && items != null && items.Count > 0;

        /// <summary>
        /// 菜单坐标位置
        /// </summary>
        public Rectangle Rect
        {
            get
            {
                if (PARENT == null) return rect;
                int y = PARENT.ScrollBar.Value;
                if (y != 0F) return new Rectangle(rect.X, rect.Y - y, rect.Width, rect.Height);
                return rect;
            }
        }

        #endregion

        #region 徽标

        string? badge = null;
        [Description("徽标内容"), Category("徽标"), DefaultValue(null), Localizable(true)]
        public string? Badge
        {
            get => badge;
            set
            {
                if (badge == value) return;
                badge = value;
                Invalidate();
            }
        }

        string? badgeSvg = null;
        [Description("徽标SVG"), Category("徽标"), DefaultValue(null)]
        public string? BadgeSvg
        {
            get => badgeSvg;
            set
            {
                if (badgeSvg == value) return;
                badgeSvg = value;
                Invalidate();
            }
        }

        TAlign badgeAlign = TAlign.Right;
        [Description("徽标方向"), Category("徽标"), DefaultValue(TAlign.Right)]
        public TAlign BadgeAlign
        {
            get => badgeAlign;
            set
            {
                if (badgeAlign == value) return;
                badgeAlign = value;
                if (badge != null || badgeSvg != null) Invalidate();
            }
        }

        float badgeSize = .6F;
        [Description("徽标比例"), Category("徽标"), DefaultValue(.6F)]
        public float BadgeSize
        {
            get => badgeSize;
            set
            {
                if (badgeSize == value) return;
                badgeSize = value;
                if (badge != null || badgeSvg != null) Invalidate();
            }
        }

        bool badgeMode = false;
        [Description("徽标模式（镂空）"), Category("徽标"), DefaultValue(false)]
        public bool BadgeMode
        {
            get => badgeMode;
            set
            {
                if (badgeMode == value) return;
                badgeMode = value;
                if (badge != null || badgeSvg != null) Invalidate();
            }
        }

        Color? badgeback = null;
        [Description("徽标背景颜色"), Category("徽标"), DefaultValue(null)]
        public Color? BadgeBack
        {
            get => badgeback;
            set
            {
                if (badgeback == value) return;
                badgeback = value;
                if (badge != null || badgeSvg != null) Invalidate();
            }
        }

        int badgeOffsetX = 1, badgeOffsetY = 1;
        /// <summary>
        /// 徽标偏移X
        /// </summary>
        [Description("徽标偏移X"), Category("徽标"), DefaultValue(1)]
        public int BadgeOffsetX
        {
            get => badgeOffsetX;
            set
            {
                if (badgeOffsetX == value) return;
                badgeOffsetX = value;
                if (badge != null || badgeSvg != null) Invalidate();
            }
        }

        /// <summary>
        /// 徽标偏移Y
        /// </summary>
        [Description("徽标偏移Y"), Category("徽标"), DefaultValue(1)]
        public int BadgeOffsetY
        {
            get => badgeOffsetY;
            set
            {
                if (badgeOffsetY == value) return;
                badgeOffsetY = value;
                if (badge != null || badgeSvg != null) Invalidate();
            }
        }

        #endregion

        void Invalidate() => PARENT?.Invalidate();
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
                if (Config.HasAnimation(nameof(Menu)))
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

        /// <summary>
        /// 是否选中
        /// </summary>
        [Description("是否选中"), Category("外观"), DefaultValue(false)]
        public bool Select { get; set; }
        internal int Depth { get; set; }
        internal float ArrowProg { get; set; } = 1F;
        internal Menu? PARENT { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

        internal bool Contains(int x, int y, int sx, int sy, out bool change)
        {
            if (rect.Contains(x + sx, y + sy))
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

        public override string? ToString() => Text;
    }
}