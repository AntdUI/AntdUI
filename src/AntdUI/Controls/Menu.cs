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

        private Color? fore;

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

        private int radius = 6;

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

        private bool round = false;

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
        /// 焦点标识块
        /// </summary>
        [Description("绘制焦点标识块"), Category("外观"), DefaultValue(false)]
        public bool FocusedMark { get; set; }

        /// <summary>
        /// 色彩模式
        /// </summary>
        [Obsolete("use ColorScheme"), Description("色彩模式"), Category("外观"), DefaultValue(TAMode.Auto)]
        public TAMode Theme
        {
            get => ColorScheme;
            set => ColorScheme = value;
        }

        private int? _gap;

        /// <summary>
        /// 间距
        /// </summary>
        [Description("间距"), Category("外观"), DefaultValue(null)]
        public int? Gap
        {
            get => _gap;
            set
            {
                if (_gap == value) return;
                _gap = value;
                ChangeList(true);
                OnPropertyChanged(nameof(Gap));
            }
        }

        private int? icongap;

        /// <summary>
        /// 图标与文字间距比例
        /// </summary>
        [Description("图标与文字间距比例"), Category("外观"), DefaultValue(null)]
        public int? IconGap
        {
            get => icongap;
            set
            {
                if (icongap == value) return;
                icongap = value;
                ChangeList(true);
                OnPropertyChanged(nameof(IconGap));
            }
        }

        private int? _itemMargin;

        /// <summary>
        /// 菜单项外间距
        /// </summary>
        [Description("菜单项外间距"), Category("外观"), DefaultValue(null)]
        public int? itemMargin
        {
            get => _itemMargin;
            set
            {
                if (_itemMargin == value) return;
                _itemMargin = value;
                ChangeList(true);
                OnPropertyChanged(nameof(itemMargin));
            }
        }

        private int? _inlineIndent;

        /// <summary>
        /// 缩进宽度
        /// </summary>
        [Description("缩进宽度"), Category("外观"), DefaultValue(null)]
        public int? InlineIndent
        {
            get => _inlineIndent;
            set
            {
                if (_inlineIndent == value) return;
                _inlineIndent = value;
                ChangeList(true);
                OnPropertyChanged(nameof(InlineIndent));
            }
        }

        private float iconratio = 1.2F;

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
                ChangeList(true);
                OnPropertyChanged(nameof(IconRatio));
            }
        }

        private float? arrowRatio;

        /// <summary>
        /// 箭头比例
        /// </summary>
        [Description("箭头比例"), Category("外观"), DefaultValue(null)]
        public float? ArrowRatio
        {
            get => arrowRatio;
            set
            {
                if (arrowRatio == value) return;
                arrowRatio = value;
                ChangeList(true);
                OnPropertyChanged(nameof(ArrowRatio));
            }
        }

        private TMenuMode mode = TMenuMode.Inline;

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
                ChangeList(true);
                OnPropertyChanged(nameof(Mode));
            }
        }

        /// <summary>
        /// 触发下拉的行为
        /// </summary>
        [Description("触发下拉的行为"), Category("行为"), DefaultValue(Trigger.Hover)]
        public Trigger Trigger { get; set; } = Trigger.Hover;

        private bool indent = false;

        /// <summary>
        /// 常规缩进
        /// </summary>
        [Description("常规缩进"), Category("外观"), DefaultValue(false)]
        public bool Indent
        {
            get { return indent; }
            set
            {
                if (indent == value) return;
                indent = value;
                ChangeList(true);
                OnPropertyChanged(nameof(Indent));
            }
        }

        private bool unique = false;

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

        private void UniqueHand(MenuItemCollection? items)
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

        private bool collapsed = false;

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
                ChangeList(true);
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
            SelectChanged?.Invoke(this, new MenuSelectEventArgs(it1));
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
            SelectChanged?.Invoke(this, new MenuSelectEventArgs(it2));
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
            SelectChanged?.Invoke(this, new MenuSelectEventArgs(it3));
            if (focus && ScrollBar.ShowY) ScrollBar.ValueY = it3.rect.Y;
            Invalidate();
        }

        #endregion 集合操作

        private MenuItemCollection? items;

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

        private bool pauseLayout = false;

        [Browsable(false), Description("暂停布局"), Category("行为"), DefaultValue(false)]
        public bool PauseLayout
        {
            get => pauseLayout;
            set
            {
                if (pauseLayout == value) return;
                pauseLayout = value;
                if (!value) ChangeList(true);
                OnPropertyChanged(nameof(PauseLayout));
            }
        }

        /// <summary>
        /// 滚动条
        /// </summary>
        [Browsable(false)]
        public ScrollBar ScrollBar;

        #endregion 属性

        #region 布局

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            ChangeList();
            var item = GetSelectItem(out var sub);
            if (item != null)
            {
                foreach (var it in sub) it.Select = true;
            }
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

        private MenuItem? GetSelectItem(ref List<MenuItem> list, MenuItemCollection? items)
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

        #endregion 获取选中项目

        protected override void OnFontChanged(EventArgs e)
        {
            ChangeList();
            base.OnFontChanged(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            ChangeList();
            base.OnSizeChanged(e);
        }

        private int collapseWidth = 0, collapsedWidth = 0;

        /// <summary>
        /// 展开之前宽度
        /// </summary>
        public int CollapseWidth => collapseWidth;

        /// <summary>
        /// 展开后宽度
        /// </summary>
        public int CollapsedWidth => collapsedWidth;

        private bool scroll_show = false, hover_r = false;
        private Rectangle rect_r, rect_r_ico;

        private bool CanLayout()
        {
            if (IsHandleCreated)
            {
                var rect = ClientRectangle;
                if (pauseLayout || items == null || items.Count == 0 || rect.Width == 0 || rect.Height == 0) return false;
                return true;
            }
            return false;
        }

        internal void ChangeList(bool print = false)
        {
            if (CanLayout())
            {
                var _rect = ClientRectangle;
                var rect = _rect.PaddingRect(Padding);
                int x = 0, y = 0, icon_count = 0;
                Helper.GDI(g =>
                {
                    var size = g.MeasureString(Config.NullText, Font);
                    int icon_size = (int)(size.Height * iconratio);
                    int gap = (_gap.HasValue ? (int)(_gap.Value * Config.Dpi) : (int)(size.Height * .8F)), gap2 = gap * 2, sp = (_itemMargin.HasValue ? (int)(_itemMargin.Value * Config.Dpi) : (int)(size.Height * .2F)), sp2 = sp * 2, height = size.Height + gap2;
                    int inlineIndent = (_inlineIndent.HasValue ? (int)(_inlineIndent.Value * Config.Dpi) : (int)(size.Height * 1.2F)), iconsp = (icongap.HasValue ? (int)(icongap.Value * Config.Dpi) : size.Height / 2);
                    if (mode == TMenuMode.Horizontal)
                    {
                        ChangeListHorizontal(rect, g, items!, ref x, icon_size, gap, gap2, sp, sp2, iconsp);
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
                        collapseWidth = icon_size + gap2 + Padding.Horizontal;
                        if (mode == TMenuMode.InlineNoText)
                        {
                            int arrow_size = arrowRatio.HasValue ? (int)(size.Height * arrowRatio.Value) : (int)(icon_size * 0.85F);
                            collapsedWidth = ChangeListInlineNoText(rect, g, null, items!, ref y, height, icon_size, arrow_size, gap, gap2, sp, sp2, iconsp) + Padding.Horizontal;
                        }
                        else
                        {
                            int arrow_size = arrowRatio.HasValue ? (int)(size.Height * arrowRatio.Value) : icon_size;
                            int yr = ChangeListY(rect, items!, ref icon_count, height, sp) + Padding.Vertical;
                            int scx = yr > _rect.Height ? ScrollBar.SIZE : 0;
                            collapsedWidth = ChangeList(rect, g, null, items!, ref y, height, icon_size, arrow_size, gap, gap2, sp, sp2, inlineIndent, iconsp, scx, 0) + Padding.Horizontal;
                        }
                        if (AutoCollapse)
                        {
                            if (icon_count > 0) collapsed = collapsedWidth >= _rect.Width;
                            else collapsed = false;
                        }
                        if (collapsed) ChangeUTitle(items!);
                    }
                });
                ScrollBar.SetVrSize(y + Padding.Vertical);
                ScrollBar.SizeChange(_rect);
            }
            if (print) Invalidate();
        }

        private int ChangeListY(Rectangle rect, MenuItemCollection items, ref int icon_count, int height, int sp)
        {
            int y = 0;
            foreach (var it in items)
            {
                if (it.HasIcon) icon_count++;
                if (it.Visible)
                {
                    y += height + sp;
                    if ((mode == TMenuMode.Inline || mode == TMenuMode.InlineNoText) && it.Expand) y += ChangeListY(rect, it.Sub, ref icon_count, height, sp);
                }
            }
            return y;
        }

        private int ChangeList(Rectangle rect, Canvas g, MenuItem? Parent, MenuItemCollection items, ref int y, int height, int icon_size, int arrow_size, int gap, int gap2, int sp, int sp2, int inlineIndent, int iconsp, int scx, int depth)
        {
            int collapsedWidth = 0, i = 0;
            foreach (var it in items)
            {
                it.Index = i;
                i++;
                it.PARENT = this;
                it.PARENTITEM = Parent;
                int uw = it.SetRect(depth, Indent, new Rectangle(rect.X, rect.Y + y, rect.Width, height), icon_size, arrow_size, gap, gap2, sp, sp2, inlineIndent, iconsp, scx);
                if (it.Visible)
                {
                    int size = g.MeasureText(it.Text, it.Font ?? Font).Width + uw + gap2 + scx;
                    // 为自定义按钮预留空间
                    if (it.HasCustomButton)
                    {
                        size += icon_size + gap; // 按钮大小 + 间距
                    }
                    if (size > collapsedWidth) collapsedWidth = size;
                    y += height + sp;
                    if (mode == TMenuMode.Inline && it.CanExpand)
                    {
                        if (collapsed)
                        {
                            int oldy = y;
                            int size2 = ChangeList(rect, g, it, it.Sub, ref y, height, icon_size, arrow_size, gap, gap2, sp, sp2, inlineIndent, iconsp, scx, depth + 1);
                            if (size2 > collapsedWidth) collapsedWidth = size2;
                            y = oldy;
                        }
                        else
                        {
                            int y_item = y;
                            int size2 = ChangeList(rect, g, it, it.Sub, ref y, height, icon_size, arrow_size, gap, gap2, sp, sp2, inlineIndent, iconsp, scx, depth + 1);
                            if (size2 > collapsedWidth) collapsedWidth = size2;

                            it.SubY = rect.Y + y_item - sp / 2;
                            it.SubHeight = y - y_item;

                            if ((it.Expand || it.ExpandThread) && it.ExpandProg > 0)
                            {
                                it.ExpandHeight = y - y_item;
                                y = y_item + (int)Math.Ceiling(it.ExpandHeight * it.ExpandProg);
                            }
                            else if (!it.Expand) y = y_item;
                        }
                    }
                }
            }
            return collapsedWidth;
        }

        private int ChangeListInlineNoText(Rectangle rect, Canvas g, MenuItem? Parent, MenuItemCollection items, ref int y, int height, int icon_size, int arrow_size, int gap, int gap2, int sp, int sp2, int iconsp)
        {
            int collapsedWidth = 0, i = 0;
            foreach (var it in items)
            {
                it.Index = i;
                i++;
                it.PARENT = this;
                it.PARENTITEM = Parent;
                int uw = it.SetRectInlineNoText(new Rectangle(rect.X, rect.Y + y, rect.Width, height), icon_size, arrow_size, gap, gap2, sp, sp2, iconsp);
                if (it.Visible)
                {
                    int size = g.MeasureText(it.Text, it.Font ?? Font).Width + uw + gap2;
                    // 为自定义按钮预留空间
                    if (it.HasCustomButton)
                    {
                        size += icon_size + gap; // 按钮大小 + 间距
                    }
                    if (size > collapsedWidth) collapsedWidth = size;
                    y += height + sp;
                    if (it.CanExpand)
                    {
                        if (collapsed)
                        {
                            int oldy = y;
                            int size2 = ChangeListInlineNoText(rect, g, it, it.Sub, ref y, height, icon_size, arrow_size, gap, gap2, sp, sp2, iconsp);
                            if (size2 > collapsedWidth) collapsedWidth = size2;
                            y = oldy;
                        }
                        else
                        {
                            int y_item = y;

                            int size2 = ChangeListInlineNoText(rect, g, it, it.Sub, ref y, height, icon_size, arrow_size, gap, gap2, sp, sp2, iconsp);
                            if (size2 > collapsedWidth) collapsedWidth = size2;

                            it.SubY = rect.Y + y_item - sp / 2;
                            it.SubHeight = y - y_item;

                            if ((it.Expand || it.ExpandThread) && it.ExpandProg > 0)
                            {
                                it.ExpandHeight = y - y_item;
                                y = y_item + (int)Math.Ceiling(it.ExpandHeight * it.ExpandProg);
                            }
                            else if (!it.Expand) y = y_item;
                        }
                    }
                }
            }
            return collapsedWidth;
        }

        private void ChangeListHorizontal(Rectangle rect, Canvas g, MenuItemCollection items, ref int x, int icon_size, int gap, int gap2, int sp, int sp2, int iconsp)
        {
            int i = 0;
            foreach (var it in items)
            {
                it.Index = i;
                i++;
                it.PARENT = this;
                int width = g.MeasureText(it.Text, it.Font ?? Font).Width;
                if (it.HasIcon)
                {
                    int tmp = icon_size + iconsp;
                    int usew = gap2 + tmp, y = (rect.Height - icon_size) / 2;
                    int size = width + gap2 + tmp;
                    var _rect = new Rectangle(rect.X + x, rect.Y, size, rect.Height);
                    it.ico_rect = new Rectangle(_rect.X + gap, _rect.Y + y, icon_size, icon_size);
                    it.SetRectNoArr(_rect, new Rectangle(_rect.X + gap + tmp, _rect.Y, _rect.Width - usew, _rect.Height));
                    if (it.Visible) x += size + sp;
                }
                else
                {
                    int size = width + gap2;
                    var _rect = new Rectangle(rect.X + x, rect.Y, size, rect.Height);
                    it.SetRectNoArr(_rect, new Rectangle(_rect.X + gap, _rect.Y, _rect.Width - gap2, _rect.Height));
                    if (it.Visible) x += size + sp;
                }
            }
        }

        private void ChangeUTitle(MenuItemCollection items)
        {
            foreach (var it in items)
            {
                var rect = it.Rect;
                it.ico_rect = new Rectangle(rect.X + (rect.Width - it.ico_rect.Width) / 2, it.ico_rect.Y, it.ico_rect.Width, it.ico_rect.Height);
                if (it.Visible && it.CanExpand) ChangeUTitle(it.Sub);
            }
        }

        #endregion 布局

        #region 渲染

        public Menu()
        { ScrollBar = new ScrollBar(this); }

        protected override void OnDraw(DrawEventArgs e)
        {
            if (items == null || items.Count == 0)
            {
                base.OnDraw(e);
                return;
            }
            var g = e.Canvas;
            if (scroll_show) g.SetClip(new Rectangle(e.Rect.X, e.Rect.Y, rect_r.Right - rect_r.Height, e.Rect.Height));
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
                PaintItems(g, e.Rect, sy, items, color_fore, color_fore_active, fore_enabled, back_hover, back_active, _radius, sub_bg);
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
            base.OnDraw(e);
        }

        private void PaintItems(Canvas g, Rectangle rect, int sy, MenuItemCollection items, Color fore, Color fore_active, Color fore_enabled, Color back_hover, Color back_active, float radius, SolidBrush sub_bg)
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

        private void PaintItemExpand(Canvas g, Rectangle rect, float sy, MenuItemCollection items, Color fore, Color fore_active, Color fore_enabled, Color back_hover, Color back_active, float radius)
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

        private void PaintIt(Canvas g, MenuItem it, Color fore, Color fore_active, Color fore_enabled, Color back_hover, Color back_active, float radius)
        {
            if (collapsed) PaintItemMini(g, it, fore, fore_active, fore_enabled, back_hover, back_active, radius);
            else PaintItem(g, it, fore, fore_active, fore_enabled, back_hover, back_active, radius);
            it.PaintBadge(Font, it.rect, g, ColorScheme);
        }

        private void PaintItemMini(Canvas g, MenuItem it, Color fore, Color fore_active, Color fore_enabled, Color back_hover, Color back_active, float radius)
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

        private void PaintItem(Canvas g, MenuItem it, Color fore, Color fore_active, Color fore_enabled, Color back_hover, Color back_active, float radius)
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
                        using (var pen = new Pen(fore_active, Config.Dpi * 2))
                        {
                            pen.StartCap = pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                            g.DrawLines(pen, it.arr_rect.TriangleLines(it.ArrowProg, .4F));
                        }
                    }
                    else PaintBack(g, back_active, it.rect, radius);
                }
                else if (it.CanExpand)
                {
                    using (var pen = new Pen(fore_enabled, Config.Dpi * 2))
                    {
                        pen.StartCap = pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                        g.DrawLines(pen, it.arr_rect.TriangleLines(it.ArrowProg, .4F));
                    }
                }
                PaintTextIcon(g, it, fore_enabled);
            }
        }

        private readonly StringFormat SL = Helper.SF_ALL(lr: StringAlignment.Near);

        private void PaintTextIcon(Canvas g, MenuItem it, Color fore)
        {
            using (var brush = new SolidBrush(fore))
            {
                if (mode != TMenuMode.InlineNoText) g.DrawText(it.Text, it.Font ?? Font, brush, it.txt_rect, SL);
                if (FocusedMark) //增加焦点块
                {
                    int fh = it.rect.Height - (it.rect.Height / 3);
                    Rectangle rectFocused = new Rectangle(0, it.rect.Top + (it.rect.Height - fh) / 2, 6, fh);
                    g.Fill(brush, rectFocused);
                }
            }
            PaintIcon(g, it, fore);
            PaintCustomButton(g, it, fore);
        }

        private void PaintTextIconExpand(Canvas g, MenuItem it, Color fore)
        {
            if (it.CanExpand)
            {
                if (mode == TMenuMode.Inline || mode == TMenuMode.InlineNoText)
                {
                    using (var pen = new Pen(fore, Config.Dpi * 2))
                    {
                        pen.StartCap = pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                        g.DrawLines(pen, it.arr_rect.TriangleLines(it.ArrowProg, .4F));
                    }
                }
                else if (mode == TMenuMode.Vertical)
                {
                    using (var pen = new Pen(fore, Config.Dpi * 2))
                    {
                        pen.StartCap = pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                        g.DrawLines(pen, TAlignMini.Right.TriangleLines(it.arr_rect, .4F));
                    }
                }
            }
            if (mode == TMenuMode.InlineNoText) PaintIcon(g, it, fore);
            else
            {
                g.DrawText(it.Text, it.Font ?? Font, fore, it.txt_rect, SL);
                PaintIcon(g, it, fore);
            }
            PaintCustomButton(g, it, fore);
        }

        private void PaintIcon(Canvas g, MenuItem it, Color fore)
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

        private void PaintCustomButton(Canvas g, MenuItem it, Color fore)
        {
            if (!it.HasCustomButton || !it.CustomButtonVisible) return;

            // 绘制自定义按钮背景（如果需要）
            if (it.custom_btn_hover)
            {
                var hoverColor = Color.FromArgb(30, fore.R, fore.G, fore.B);
                PaintBack(g, hoverColor, it.custom_btn_rect, Radius);
            }
            if(it.Text== "Item 2")
            {

            }
            // 绘制自定义按钮图标
            if (it.CustomButtonEnabled)
            {
                if (it.CustomButtonIcon != null)
                {
                    g.Image(it.CustomButtonIcon, it.custom_btn_rect);
                }
                else if (!string.IsNullOrEmpty(it.CustomButtonIconSvg))
                {
                    //g.GetImgExtend(it.CustomButtonIconSvg, it.custom_btn_rect, fore);

                    using (var bmp = it.CustomButtonIconSvg.SvgToBmp(it.custom_btn_rect.Width, it.custom_btn_rect.Height, fore))
                    {
                        if (bmp != null) g.Image(bmp, it.custom_btn_rect);
                    }
                }
                else if (!string.IsNullOrEmpty(it.CustomButtonText))
                {
                    // 如果没有图标，绘制文本
                    using (var brush = new SolidBrush(fore))
                    {
                        g.DrawText(it.CustomButtonText, it.Font ?? Font, brush, it.custom_btn_rect, Helper.SF_ALL());
                    }
                }
            }
            else
            {
                // 禁用状态，使用灰色
                var disabledColor = Color.FromArgb(128, fore.R, fore.G, fore.B);
                if (it.CustomButtonIcon != null)
                {
                    using (var attr = new System.Drawing.Imaging.ImageAttributes())
                    {
                        var matrix = new System.Drawing.Imaging.ColorMatrix
                        {
                            Matrix33 = 0.5f // 设置透明度
                        };
                        attr.SetColorMatrix(matrix);
                        g.Image(it.CustomButtonIcon, it.custom_btn_rect, 0, 0, it.CustomButtonIcon.Width, it.CustomButtonIcon.Height, GraphicsUnit.Pixel, attr);
                    }
                }
                else if (!string.IsNullOrEmpty(it.CustomButtonIconSvg))
                {
                    g.GetImgExtend(it.CustomButtonIconSvg, it.custom_btn_rect, disabledColor);
                }
                else if (!string.IsNullOrEmpty(it.CustomButtonText))
                {
                    using (var brush = new SolidBrush(disabledColor))
                    {
                        g.DrawText(it.CustomButtonText, it.Font ?? Font, brush, it.custom_btn_rect, Helper.SF_ALL());
                    }
                }
            }
        }

        private void PaintBack(Canvas g, Color color, Rectangle rect, float radius)
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

        #endregion 渲染

        #region 鼠标

        private MenuItem? MDown;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            CloseTip();
            CloseDropDown();
            if (e.Button == MouseButtons.Right && !MouseRightCtrl) return;
            if (ScrollBar.MouseDown(e.X, e.Y))
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

        private bool IMouseDown(MenuItemCollection items, MenuItem item, int x, int y)
        {
            if (item.Visible)
            {
                bool can = item.CanExpand;
                if (item.Enabled && item.Contains(x, y, 0, ScrollBar.Value, out _, out bool customButtonHit))
                {
                    MDown = item;
                    // 记录是否点击了自定义按钮
                    item.Tag = customButtonHit ? "CustomButtonClick" : null;
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

        private bool IMouseUp(MenuItemCollection items, MenuItem item, List<MenuItem> list, int x, int y, MenuItem MDown)
        {
            if (item.Visible)
            {
                bool can = item.CanExpand;
                if (MDown == item)
                {
                    if (item.Enabled && item.Contains(x, y, 0, ScrollBar.Value, out _, out bool customButtonHit))
                    {
                        // 检查是否是自定义按钮点击
                        bool wasCustomButtonClick = item.Tag as string == "CustomButtonClick";
                        item.Tag = null; // 清除标记

                        if (wasCustomButtonClick && customButtonHit && item.HasCustomButton)
                        {
                            // 触发自定义按钮点击事件
                            CustomButtonClick?.Invoke(this, new MenuCustomButtonEventArgs(item));
                            Invalidate();
                            return true;
                        }

                        if (IsCanChang(item))
                        {
                            if (can)
                            {
                                if ((mode == TMenuMode.Horizontal || mode == TMenuMode.Vertical) && Trigger == Trigger.Click && item.items != null && item.items.Count > 0)
                                {
                                    if (subForm == null) OpenDropDown(item);
                                    else CloseDropDown();
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
                                SelectChanged?.Invoke(this, new MenuSelectEventArgs(item));
                                Invalidate();
                            }
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

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (ScrollBar.MouseMove(e.X, e.Y) && OnTouchMove(e.X, e.Y))
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
                    foreach (var it in items)
                    {
                        if (it.show)
                        {
                            if (it.Contains(e.X, e.Y, 0, ScrollBar.Value, out var change, out _)) hand++;
                            if (change) count++;
                        }
                    }
                }
                else if (mode == TMenuMode.Inline)
                {
                    foreach (var it in items) IMouseMove(it, e.X, e.Y, ref count, ref hand);
                }
                else if (mode == TMenuMode.InlineNoText)
                {
                    foreach (var it in items)
                    {
                        if (it.show)
                        {
                            if (it.Contains(e.X, e.Y, 0, ScrollBar.Value, out var change, out _)) hand++;
                            if (change) count++;
                            if (it.items != null && it.items.Count > 0) foreach (var sub in it.items) IMouseMove(sub, e.X, e.Y, ref count, ref hand);
                        }
                    }
                }
                else
                {
                    foreach (var it in items)
                    {
                        if (it.show)
                        {
                            if (it.Contains(e.X, e.Y, 0, ScrollBar.Value, out var change, out _)) hand++;
                            if (change) count++;
                        }
                    }
                }
                SetCursor(hand > 0);
                if (count > 0) Invalidate();
            }
            else ILeave();
        }

        private void IMouseMove(MenuItem it, int x, int y, ref int count, ref int hand)
        {
            if (it.show)
            {
                if (it.Contains(x, y, 0, ScrollBar.Value, out var change, out bool customButtonHit))
                {
                    hand++;
                    return;
                }
                if (change) count++;
                if (it.items != null && it.items.Count > 0) foreach (var sub in it.items) IMouseMove(sub, x, y, ref count, ref hand);
            }
        }

        #region 鼠标悬浮

        protected override bool CanMouseMove { get; set; } = true;

        protected override void OnMouseHover(int x, int y)
        {
            CloseTip();
            if (x == -1 || y == -1) return;
            CloseDropDown();
            if (items == null || items.Count == 0) return;
            if (scroll_show)
            {
                if (rect_r.Contains(x, y))
                {
                    var list = new List<MenuItem>(items.Count);
                    foreach (var it in items)
                    {
                        if (it.Rect.X > (rect_r.X - it.Rect.Width)) list.Add(it);
                    }
                    subForm = new LayeredFormMenuDown(this, radius, rect_r, list);
                    subForm.Show(this);
                    return;
                }
            }
            int sy = ScrollBar.Value;
            if (collapsed)
            {
                foreach (var it in items)
                {
                    if (it.show && it.rect.Contains(x, y + sy))
                    {
                        if (OpenDropDown(it)) OpenTip(it, it.Rect);
                        return;
                    }
                }
            }
            else if (mode == TMenuMode.Inline) return;
            else if (mode == TMenuMode.InlineNoText)
            {
                foreach (var it in items) IMouseHover(it, x, y, sy);
            }
            else
            {
                foreach (var it in items)
                {
                    if (it.show && it.rect.Contains(x, y + sy))
                    {
                        if (Trigger == Trigger.Hover) OpenDropDown(it);
                        return;
                    }
                }
            }
        }

        private void IMouseHover(MenuItem it, int x, int y, int sy)
        {
            if (it.show && it.rect.Contains(x, y + sy))
            {
                var rect = new Rectangle(it.rect.X, it.rect.Y + (it.rect.Height / 2) - ScrollBar.Value, it.rect.Width, rect_r.Height);
                OpenTip(it, rect);
                return;
            }
            if (it.items != null && it.items.Count > 0) foreach (var sub in it.items) IMouseHover(sub, x, y, sy);
        }

        #region Tip

        private TooltipForm? toolTip;

        public void CloseTip()
        {
            toolTip?.IClose();
            toolTip = null;
        }

        private bool OpenTip(MenuItem it, Rectangle rect)
        {
            if (it.Text == null) return true;
            if (toolTip == null)
            {
                toolTip = new TooltipForm(this, rect, it.Text, TooltipConfig ?? new TooltipConfig
                {
                    Font = it.Font ?? Font,
                    ArrowAlign = TAlign.Right,
                });
                toolTip.Show(this);
            }
            else if (toolTip.SetText(rect, it.Text))
            {
                CloseTip();
                OpenTip(it, rect);
            }
            return false;
        }

        #endregion Tip

        #endregion 鼠标悬浮

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (RectangleToScreen(ClientRectangle).Contains(MousePosition)) return;
            CloseTip();
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
            ScrollBar.MouseWheel(e);
            base.OnMouseWheel(e);
        }

        protected override bool OnTouchScrollX(int value) => ScrollBar.MouseWheelXCore(value);

        protected override bool OnTouchScrollY(int value) => ScrollBar.MouseWheelYCore(value);

        private void ILeave()
        {
            SetCursor(false);
            if (items == null || items.Count == 0) return;
            int count = 0;
            foreach (var it in items) ILeave(it, ref count);
            if (count > 0) Invalidate();
        }

        private void ILeave(MenuItem it, ref int count)
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

        private void IUSelect(MenuItemCollection items)
        {
            foreach (var it in items) IUSelect(it);
        }

        private void IUSelect(MenuItem it)
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

        private bool IHitTest(int x, int y, MenuItem item, out MenuItem? mdown)
        {
            if (item.Visible)
            {
                bool can = item.CanExpand;
                if (item.Enabled && item.Contains(x, y, 0, ScrollBar.Value, out _, out _))
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

        #endregion 鼠标

        #region 方法

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

        private void Select(MenuItem item, bool focus, MenuItemCollection items)
        {
            foreach (var it in items)
            {
                if (it == item)
                {
                    it.Select = true;
                    tmpAM = true;
                    SelectChanged?.Invoke(this, new MenuSelectEventArgs(it));
                    if (SelectEx(it.PARENTITEM) > 0) ChangeList(true);
                    tmpAM = false;
                    if (focus && ScrollBar.ShowY) ScrollBar.ValueY = it.rect.Y;
                    return;
                }
                else if (it.items != null && it.items.Count > 0) Select(item, focus, it.items);
            }
        }

        internal bool tmpAM = false;

        private int SelectEx(MenuItem? it)
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

        private void Remove(MenuItem item, MenuItemCollection items)
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

        #endregion 方法

        #region 事件

        /// <summary>
        /// Select 属性值更改时发生
        /// </summary>
        [Description("Select 属性值更改时发生"), Category("行为")]
        public event SelectEventHandler? SelectChanged;

        /// <summary>
        /// Select 属性值更改前发生
        /// </summary>
        [Description("Select 属性值更改前发生"), Category("行为")]
        public event SelectBoolEventHandler? SelectChanging;

        /// <summary>
        /// 自定义按钮点击时发生
        /// </summary>
        [Description("自定义按钮点击时发生"), Category("行为")]
        public event MenuCustomButtonEventHandler? CustomButtonClick;

        private bool IsCanChang(MenuItem it)
        {
            bool pass = false;
            if (SelectChanging == null) pass = true;
            else if (SelectChanging(this, new MenuSelectEventArgs(it))) pass = true;
            return pass;
        }

        #endregion 事件

        #region 子窗口

        private ILayeredForm? subForm;

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

        private bool IDropDownChange(MenuItemCollection items, MenuItem item, List<MenuItem> list, MenuItem value)
        {
            bool can = item.CanExpand;
            if (item.Enabled && item == value)
            {
                if (IsCanChang(item))
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
                        SelectChanged?.Invoke(this, new MenuSelectEventArgs(item));
                        Invalidate();
                    }
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

        private bool OpenDropDown(MenuItem it)
        {
            if (it.items == null || it.items.Count == 0) return true;
            select_x = 0;
            subForm = new LayeredFormMenuDown(this, radius, it.Rect, it.items);
            subForm.Show(this);
            return false;
        }

        private void CloseDropDown()
        {
            subForm?.Close();
            subForm = null;
        }

        #endregion 子窗口

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
                if (render) it.ChangeList(true);
                else it.Invalidate();
            };
            return this;
        }

        internal MenuItemCollection BindData(MenuItem it)
        {
            action = render =>
            {
                if (it.PARENT == null) return;
                if (render) it.PARENT.ChangeList(true);
                else it.PARENT.Invalidate();
            };
            return this;
        }
    }

    public class MenuItem : BadgeConfig
    {
        public MenuItem()
        { }

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

        /// <summary>
        /// 名称
        /// </summary>
        [Description("名称"), Category("数据"), DefaultValue(null)]
        public string? Name { get; set; }

        #region 图标

        private Image? icon;

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

        private string? iconSvg;

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

        #endregion 图标

        private string? text;

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

        private bool visible = true;

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

        #region 自定义按钮

        private string? customButtonText;

        /// <summary>
        /// 自定义按钮文本
        /// </summary>
        [Description("自定义按钮文本"), Category("自定义按钮"), DefaultValue(null), Localizable(true)]
        public string? CustomButtonText
        {
            get => customButtonText;
            set
            {
                if (customButtonText == value) return;
                customButtonText = value;
                Invalidates();
            }
        }

        private string? customButtonIconSvg;

        /// <summary>
        /// 自定义按钮图标SVG
        /// </summary>
        [Description("自定义按钮图标SVG"), Category("自定义按钮"), DefaultValue(null)]
        public string? CustomButtonIconSvg
        {
            get => customButtonIconSvg;
            set
            {
                if (customButtonIconSvg == value) return;
                customButtonIconSvg = value;
                Invalidates();
            }
        }

        private Image? customButtonIcon;

        /// <summary>
        /// 自定义按钮图标
        /// </summary>
        [Description("自定义按钮图标"), Category("自定义按钮"), DefaultValue(null)]
        public Image? CustomButtonIcon
        {
            get => customButtonIcon;
            set
            {
                if (customButtonIcon == value) return;
                customButtonIcon = value;
                Invalidates();
            }
        }

        private bool customButtonVisible = false;

        /// <summary>
        /// 自定义按钮是否可见
        /// </summary>
        [Description("自定义按钮是否可见"), Category("自定义按钮"), DefaultValue(false)]
        public bool CustomButtonVisible
        {
            get => customButtonVisible;
            set
            {
                if (customButtonVisible == value) return;
                customButtonVisible = value;
                Invalidates();
            }
        }

        private bool customButtonEnabled = true;

        /// <summary>
        /// 自定义按钮是否启用
        /// </summary>
        [Description("自定义按钮是否启用"), Category("自定义按钮"), DefaultValue(true)]
        public bool CustomButtonEnabled
        {
            get => customButtonEnabled;
            set
            {
                if (customButtonEnabled == value) return;
                customButtonEnabled = value;
                Invalidates();
            }
        }

        /// <summary>
        /// 是否包含自定义按钮
        /// </summary>
        internal bool HasCustomButton => customButtonVisible && (!string.IsNullOrWhiteSpace(customButtonText) || !string.IsNullOrWhiteSpace(customButtonIconSvg) || customButtonIcon != null);

        #endregion 自定义按钮

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

        private bool enabled = true;

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

        #endregion 禁用

        #region 展开

        private ITask? ThreadExpand;
        private bool expand = true;

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
                    if (PARENT == null) return;
                    if (value && PARENT.Unique)
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
                    if (((PARENT.Mode == TMenuMode.Inline && !PARENT.Collapsed) || PARENT.Mode == TMenuMode.InlineNoText) && Config.HasAnimation(nameof(Menu)))
                    {
                        if (PARENT.tmpAM)
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

        #endregion 展开

        #region 徽标

        private string? badge;

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

        private string? badgeSvg;

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

        private TAlign badgeAlign = TAlign.Right;

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

        private float badgeSize = .6F;

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

        private bool badgeMode = false;

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

        private Color? badgeback;

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

        private int badgeOffsetX = 1, badgeOffsetY = 1;

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

        #endregion 徽标

        #region 方法

        public void Remove()
        {
            if (PARENTITEM == null) PARENT?.Items.Remove(this);
            else PARENTITEM.items?.Remove(this);
        }

        public void UpdateText(string newText)
        {
            Text = newText;
            Invalidate();
        }

        #endregion 方法

        #region 悬浮态

        private bool hover = false;

        /// <summary>
        /// 是否悬浮
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

        #endregion 悬浮态

        /// <summary>
        /// 是否选中
        /// </summary>
        [Description("是否选中"), Category("外观"), DefaultValue(false)]
        public bool Select { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MenuItem? PARENTITEM { get; set; }

        #region 内部

        internal int Depth { get; set; }
        internal float ArrowProg { get; set; } = 1F;
        internal Menu? PARENT { get; set; }

        #region 布局

        internal int SetRect(int depth, bool indent, Rectangle _rect, int icon_size, int arrow_size, int gap, int gap2, int sp, int sp2, int inlineIndent, int iconsp, int scx)
        {
            Depth = depth;
            rect = _rect;
            int x = gap, usew = gap2, y = (_rect.Height - icon_size) / 2;
            if (indent && depth > 0)
            {
                int tmp = inlineIndent * depth;
                x += tmp;
                usew += tmp;
            }
            else if (depth > 1)
            {
                int tmp = inlineIndent * (depth - 1);
                x += tmp;
                usew += tmp;
            }

            // 计算自定义按钮空间
            int customBtnWidth = 0;
            if (HasCustomButton)
            {
                customBtnWidth = icon_size + gap; // 按钮大小 + 间距
            }

            if (HasIcon)
            {
                int tmp = icon_size + iconsp;
                ico_rect = new Rectangle(_rect.X + x, _rect.Y + y, icon_size, icon_size);

                x += tmp;
                usew += tmp;

                txt_rect = new Rectangle(_rect.X + x, _rect.Y, _rect.Width - usew - customBtnWidth, _rect.Height);
            }
            else txt_rect = new Rectangle(_rect.X + x, _rect.Y, _rect.Width - usew - customBtnWidth, _rect.Height);

            // 设置自定义按钮位置（在文本右侧，箭头左侧）
            if (HasCustomButton)
            {
                custom_btn_rect = new Rectangle(_rect.Right - icon_size - gap - scx - customBtnWidth, _rect.Y + y, icon_size, icon_size);
            }

            arr_rect = new Rectangle(_rect.Right - icon_size - gap - scx, _rect.Y + y, icon_size, icon_size);
            Show = true;
            return usew;
        }

        internal int SetRectInlineNoText(Rectangle _rect, int icon_size, int arrow_size, int gap, int gap2, int sp, int sp2, int iconsp)
        {
            Depth = 0;
            rect = _rect;
            int x = gap, usew = gap2;

            // 计算自定义按钮空间
            int customBtnWidth = 0;
            if (HasCustomButton)
            {
                customBtnWidth = icon_size + gap; // 按钮大小 + 间距
            }

            if (HasIcon)
            {
                int tmp = icon_size + iconsp, y = (_rect.Height - icon_size) / 2;
                ico_rect = new Rectangle(_rect.X + x, _rect.Y + y, icon_size, icon_size);

                x += tmp;
                usew += tmp;

                txt_rect = new Rectangle(_rect.X + x, _rect.Y, _rect.Width - usew - customBtnWidth, _rect.Height);
            }
            else txt_rect = new Rectangle(_rect.X + x, _rect.Y, _rect.Width - usew - customBtnWidth, _rect.Height);

            // 设置自定义按钮位置（在文本右侧，箭头左侧）
            if (HasCustomButton)
            {
                int y = (_rect.Height - icon_size) / 2;
                custom_btn_rect = new Rectangle(_rect.Right - (arrow_size - sp) - customBtnWidth, _rect.Y + y, icon_size, icon_size);
            }

            arr_rect = new Rectangle(_rect.Right - (arrow_size - sp), _rect.Y + (_rect.Height - arrow_size) / 2, arrow_size, arrow_size);
            Show = true;
            return usew;
        }

        internal void SetRectNoArr(Rectangle _rect, Rectangle rect_text)
        {
            Depth = 0;
            rect = _rect;
            txt_rect = rect_text;
            Show = true;
        }

        internal Rectangle rect { get; set; }
        internal Rectangle arr_rect { get; set; }
        internal Rectangle custom_btn_rect { get; set; }
        internal bool custom_btn_hover { get; set; }

        internal bool Contains(int x, int y, int sx, int sy, out bool change)
        {
            return Contains(x, y, sx, sy, out change, out _);
        }

        internal bool Contains(int x, int y, int sx, int sy, out bool change, out bool customButtonHit)
        {
            customButtonHit = false;
            if (rect.Contains(x + sx, y + sy))
            {
                // 检查是否点击了自定义按钮区域
                if (HasCustomButton && custom_btn_rect.Contains(x + sx, y + sy))
                {
                    customButtonHit = true;
                    var btnHoverChange = custom_btn_hover != true;
                    custom_btn_hover = true;
                    change = SetHover(true) || btnHoverChange;
                }
                else
                {
                    var btnHoverChange = custom_btn_hover != false;
                    custom_btn_hover = false;
                    change = SetHover(true) || btnHoverChange;
                }
                return true;
            }
            else
            {
                var btnHoverChange = custom_btn_hover != false;
                custom_btn_hover = false;
                change = SetHover(false) || btnHoverChange;
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
        private ITask? ThreadHover;

        internal Rectangle txt_rect { get; set; }
        internal Rectangle ico_rect { get; set; }

        #endregion 布局

        internal float SubY { get; set; }
        internal float SubHeight { get; set; }

        internal int ExpandHeight { get; set; }
        internal float ExpandProg { get; set; }
        internal bool ExpandThread { get; set; }
        internal bool show { get; set; }
        internal bool Show { get; set; }

        private void Invalidate() => PARENT?.Invalidate();

        private void Invalidates() => PARENT?.ChangeList(true);

        #endregion 内部

        #region 设置

        public MenuItem SetFont(Font? value)
        {
            Font = value;
            return this;
        }

        #region 图标

        public MenuItem SetIcon(Image? img)
        {
            icon = img;
            return this;
        }

        public MenuItem SetIcon(string? svg)
        {
            iconSvg = svg;
            return this;
        }

        public MenuItem SetIcon(Image? img, Image? hover)
        {
            icon = img;
            IconActive = hover;
            return this;
        }

        public MenuItem SetIcon(string? svg, string? hover)
        {
            iconSvg = svg;
            IconActiveSvg = hover;
            return this;
        }

        #endregion 图标

        public MenuItem SetID(string? value)
        {
            ID = value;
            return this;
        }

        public MenuItem SetName(string? value)
        {
            Name = value;
            return this;
        }

        public MenuItem SetText(string? value, string? localization = null)
        {
            text = value;
            LocalizationText = localization;
            return this;
        }

        public MenuItem SetVisible(bool value = false)
        {
            visible = value;
            return this;
        }

        public MenuItem SetEnabled(bool value = false)
        {
            enabled = value;
            return this;
        }

        public MenuItem SetExpand(bool value = true)
        {
            expand = value;
            return this;
        }

        public MenuItem SetSub(MenuItem value)
        {
            Sub.Add(value);
            return this;
        }

        public MenuItem SetSub(params MenuItem[] value)
        {
            Sub.AddRange(value);
            return this;
        }

        public MenuItem SetSub(IList<MenuItem> value)
        {
            Sub.AddRange(value);
            return this;
        }

        #region 徽标

        public MenuItem SetBadge(string? value = " ", TAlign align = TAlign.TR)
        {
            badge = value;
            badgeAlign = align;
            return this;
        }

        public MenuItem SetBadgeSvg(string? value, TAlign align = TAlign.TR)
        {
            badgeSvg = value;
            badgeAlign = align;
            return this;
        }

        public MenuItem SetBadgeOffset(int x, int y)
        {
            BadgeOffsetX = x;
            BadgeOffsetY = y;
            return this;
        }

        public MenuItem SetBadgeSize(float value)
        {
            BadgeSize = value;
            return this;
        }

        public MenuItem SetBadgeBack(Color? value)
        {
            BadgeBack = value;
            return this;
        }

        #endregion 徽标

        public MenuItem SetTag(object? value)
        {
            Tag = value;
            return this;
        }

        #endregion 设置

        public override string? ToString() => Text;
    }
}