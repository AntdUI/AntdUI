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
    /// Tabs 标签页
    /// </summary>
    /// <remarks>选项卡切换组件。</remarks>
    [Description("Tabs 标签页")]
    [ToolboxItem(true)]
    [DefaultEvent("SelectedIndexChanged")]
    [DefaultProperty("Pages")]
    [Designer(typeof(TabControlDesigner))]
    public partial class Tabs : IControl
    {
        public Tabs() { style = SetType(type); }

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
                OnPropertyChanged("ForeColor");
            }
        }

        Color? fill;
        /// <summary>
        /// 颜色
        /// </summary>
        [Description("颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? Fill
        {
            get => fill;
            set
            {
                if (fill == value) return;
                fill = value;
                Invalidate();
                OnPropertyChanged("Fill");
            }
        }

        /// <summary>
        /// 悬停颜色
        /// </summary>
        [Description("悬停颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? FillHover { get; set; }

        /// <summary>
        /// 激活颜色
        /// </summary>
        [Description("激活颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? FillActive { get; set; }

        TabAlignment alignment = TabAlignment.Top;
        /// <summary>
        /// 位置
        /// </summary>
        [Description("位置"), Category("外观"), DefaultValue(TabAlignment.Top)]
        public TabAlignment Alignment
        {
            get => alignment;
            set
            {
                if (alignment == value) return;
                alignment = value;
                LoadLayout();
                OnPropertyChanged("Alignment");
            }
        }

        bool centered = false;
        /// <summary>
        /// 标签居中展示
        /// </summary>
        [Description("标签居中展示"), Category("外观"), DefaultValue(false)]
        public bool Centered
        {
            get => centered;
            set
            {
                if (centered == value) return;
                centered = value;
                LoadLayout();
                OnPropertyChanged("Centered");
            }
        }

        TabTypExceed typExceed = TabTypExceed.Button;
        /// <summary>
        /// 超出UI类型
        /// </summary>
        [Description("超出UI类型"), Category("外观"), DefaultValue(TabTypExceed.Button)]
        public TabTypExceed TypExceed
        {
            get => typExceed;
            set
            {
                if (typExceed == value) return;
                typExceed = value;
                bitblock_l?.Dispose();
                bitblock_r?.Dispose();
                bitblock_l = bitblock_r = null;
                LoadLayout();
                OnPropertyChanged("TypExceed");
            }
        }

        Color? scrollback;
        /// <summary>
        /// 滚动条颜色
        /// </summary>
        [Description("滚动条颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? ScrollBack
        {
            get => scrollback;
            set
            {
                if (scrollback == value) return;
                scrollback = value;
                bitblock_l?.Dispose();
                bitblock_r?.Dispose();
                bitblock_l = bitblock_r = null;
                Invalidate();
                OnPropertyChanged("ScrollBack");
            }
        }

        Color? scrollfore;
        /// <summary>
        /// 滚动条文本颜色
        /// </summary>
        [Description("滚动条文本颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? ScrollFore
        {
            get => scrollfore;
            set
            {
                if (scrollfore == value) return;
                scrollfore = value;
                Invalidate();
                OnPropertyChanged("ScrollFore");
            }
        }

        /// <summary>
        /// 滚动条悬停文本颜色
        /// </summary>
        [Description("滚动条悬停文本颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? ScrollForeHover { get; set; }

        /// <summary>
        /// 滚动条悬停颜色
        /// </summary>
        [Description("滚动条悬停颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? ScrollBackHover { get; set; }

        #region 样式

        IStyle style;
        /// <summary>
        /// 样式
        /// </summary>
        [Description("样式"), Category("外观")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public IStyle Style
        {
            get => style;
            set => style = value;
        }

        TabType type = TabType.Line;
        /// <summary>
        /// 样式类型
        /// </summary>
        [Description("样式类型"), Category("外观"), DefaultValue(TabType.Line)]
        public TabType Type
        {
            get => type;
            set
            {
                if (type == value) return;
                type = value;
                style = SetType(value);
                LoadLayout();
                OnPropertyChanged("Type");
            }
        }

        IStyle SetType(TabType type)
        {
            switch (type)
            {
                case TabType.Card:
                    if (style is StyleCard stylec) return stylec;
                    return new StyleCard(this);
                case TabType.Card2:
                    if (style is StyleCard2 stylec2) return stylec2;
                    return new StyleCard2(this);
                case TabType.Line:
                default:
                    if (style is StyleLine stylel) return stylel;
                    return new StyleLine(this);
            }
        }

        #endregion

        int _gap = 8;
        /// <summary>
        /// 间距
        /// </summary>
        [Description("间距"), Category("外观"), DefaultValue(8)]
        public int Gap
        {
            get => _gap;
            set
            {
                if (_gap == value) return;
                _gap = value;
                LoadLayout();
                OnPropertyChanged("Gap");
            }
        }

        float iconratio = .7F;
        /// <summary>
        /// 图标比例
        /// </summary>
        [Description("图标比例"), Category("外观"), DefaultValue(.7F)]
        public float IconRatio
        {
            get => iconratio;
            set
            {
                if (iconratio == value) return;
                iconratio = value;
                LoadLayout();
                OnPropertyChanged("IconRatio");
            }
        }

        bool _tabMenuVisible = true;
        [Description("是否显示头"), Category("外观"), DefaultValue(true)]
        public bool TabMenuVisible
        {
            get => _tabMenuVisible;
            set
            {
                _tabMenuVisible = value;
                LoadLayout();
                OnPropertyChanged("TabMenuVisible");
            }
        }

        int? _itemSize = null;
        /// <summary>
        /// 自定义项宽度
        /// </summary>
        [Description("自定义项大小"), Category("外观"), DefaultValue(null)]
        public int? ItemSize
        {
            get => _itemSize;
            set
            {
                if (_itemSize == value) return;
                _itemSize = value;
                LoadLayout();
                OnPropertyChanged("ItemSize");
            }
        }

        internal Dictionary<TabPage, Size> HandItemSize(Dictionary<TabPage, Size> rect_dir, ref int sizewh)
        {
            if (_itemSize.HasValue)
            {
                int Size = (int)Math.Ceiling(_itemSize.Value * Config.Dpi);
                var rect_dirtmp = new Dictionary<TabPage, Size>(rect_dir.Count);
                foreach (var it in rect_dir) rect_dirtmp.Add(it.Key, new Size(Size, it.Value.Height));
                if (alignment == TabAlignment.Left || alignment == TabAlignment.Right) sizewh = Size;
                return rect_dirtmp;
            }
            return rect_dir;
        }

        public override Rectangle DisplayRectangle => ClientRectangle.PaddingRect(Margin, Padding, _padding);

        #region 数据

        TabCollection? items;
        /// <summary>
        /// 数据
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("集合"), Category("数据")]
        public TabCollection Pages
        {
            get
            {
                items ??= new TabCollection(this);
                return items;
            }
            set => items = value.BindData(this);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TabPage? SelectedTab
        {
            get
            {
                if (items == null || items.Count <= _select || _select < 0) return null;
                return items[_select];
            }
            set
            {
                if (items == null || value == null) return;
                var index = items.IndexOf(value);
                SelectedIndex = index;
                OnPropertyChanged("SelectedTab");
            }
        }

        public void SelectTab(string tabPageName)
        {
            if (items == null) return;
            foreach (var it in items)
            {
                if (it.Text == tabPageName)
                {
                    SelectedTab = it;
                    return;
                }
            }
        }

        public void SelectTab(TabPage tabPage) => SelectedTab = tabPage;

        public void SelectTab(int index) => SelectedIndex = index;

        #region 动画

        int _select = 0;
        [Description("选中序号"), Category("数据"), DefaultValue(0)]
        public int SelectedIndex
        {
            get => _select;
            set
            {
                if (_select == value) return;
                int old = _select;
                _select = value;
                style.SelectedIndexChanged(value, old);
                SelectedIndexChanged?.Invoke(this, new IntEventArgs(value));
                Invalidate();
                ShowPage(_select);
                OnPropertyChanged("SelectedIndex");
            }
        }

        internal void ShowPage(int index)
        {
            if (showok)
            {
                if (items == null) return;
                if (items.Count > 1)
                {
                    for (int i = 0; i < items.Count; i++)
                    {
                        if (i == index) continue;
                        items[i].SetDock(false);
                    }
                }
                if (items.Count <= _select || _select < 0) return;
                items[_select].SetDock(true);
            }
        }

        protected override void Dispose(bool disposing)
        {
            style.Dispose();
            bitblock_l?.Dispose();
            bitblock_r?.Dispose();
            if (items == null || items.Count == 0) return;
            foreach (var it in items) it.Dispose();
            items.Clear();
            base.Dispose(disposing);
        }

        #endregion

        #endregion

        #endregion

        #region 布局

        bool showok = false;
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            LoadLayout(false);
            showok = true;
            ShowPage(_select);
        }

        protected override void OnMarginChanged(EventArgs e)
        {
            LoadLayout(true);
            base.OnMarginChanged(e);
        }

        protected override void OnFontChanged(EventArgs e)
        {
            LoadLayout(false);
            base.OnFontChanged(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            LoadLayout(false);
            base.OnSizeChanged(e);
        }

        Padding _padding = new Padding(0);
        bool SetPadding(int x, int y, int r, int b)
        {
            if (_padding.Left == x && _padding.Top == y && _padding.Right == r && _padding.Bottom == b) return true;
            _padding.Left = x;
            _padding.Top = y;
            _padding.Right = r;
            _padding.Bottom = b;
            base.OnSizeChanged(EventArgs.Empty);
            return false;
        }

        internal void LoadLayout(bool r = true)
        {
            if (IsHandleCreated)
            {
                if (items == null) return;
                if (_tabMenuVisible)
                {
                    var rect = ClientRectangle;
                    if (rect.Width > 0 && rect.Height > 0)
                    {
                        var rect_t = rect.DeflateRect(Margin);
                        style.LoadLayout(this, rect_t, items);
                        if (r) Invalidate();
                    }
                }
                else SetPadding(0, 0, 0, 0);
            }
        }

        #endregion

        #region 渲染

        readonly StringFormat s_c = Helper.SF_ALL(), s_f = Helper.SF_NoWrap();
        protected override void OnPaint(PaintEventArgs e)
        {
            if (items == null || items.Count == 0 || !_tabMenuVisible) return;
            var g = e.Graphics.High();
            style.Paint(this, g, items);
            this.PaintBadge(g);
            base.OnPaint(e);
        }

        void PaintBadge(Canvas g, TabPage page, Rectangle rect)
        {
            if (page.Badge != null)
            {
                var color = page.BadgeBack ?? Colour.Error.Get("Tabs");
                using (var brush_fore = new SolidBrush(Colour.ErrorColor.Get("Tabs")))
                {
                    using (var font = new Font(Font.FontFamily, Font.Size * page.BadgeSize))
                    {
                        if (string.IsNullOrEmpty(page.Badge) || page.Badge == "" || page.Badge == " ")
                        {
                            var size = g.MeasureString(Config.NullText, font).Width / 2;
                            var rect_badge = new RectangleF(rect.Right - size - page.BadgeOffsetX * Config.Dpi, rect.Y + page.BadgeOffsetY * Config.Dpi, size, size);
                            using (var brush = new SolidBrush(color))
                            {
                                g.FillEllipse(brush, rect_badge);
                                g.DrawEllipse(brush_fore.Color, Config.Dpi, rect_badge);
                            }
                        }
                        else
                        {
                            var size = g.MeasureString(page.Badge, font);
                            int size_badge = (int)(size.Height * 1.2F);
                            if (size.Height > size.Width)
                            {
                                var rect_badge = new Rectangle(rect.Right - size_badge - (int)(page.BadgeOffsetX * Config.Dpi), rect.Y + (int)(page.BadgeOffsetY * Config.Dpi), size_badge, size_badge);
                                using (var brush = new SolidBrush(color))
                                {
                                    g.FillEllipse(brush, rect_badge);
                                    g.DrawEllipse(brush_fore.Color, Config.Dpi, rect_badge);
                                }
                                g.String(page.Badge, font, brush_fore, rect_badge, s_f);
                            }
                            else
                            {
                                int w_badge = size.Width + (size_badge - size.Height);
                                var rect_badge = new Rectangle(rect.Right - w_badge - (int)(page.BadgeOffsetX * Config.Dpi), rect.Y + (int)(page.BadgeOffsetY * Config.Dpi), w_badge, size_badge);
                                using (var brush = new SolidBrush(color))
                                {
                                    using (var path = rect_badge.RoundPath(rect_badge.Height))
                                    {
                                        g.Fill(brush, path);
                                        g.Draw(brush_fore.Color, Config.Dpi, path);
                                    }
                                }
                                g.String(page.Badge, font, brush_fore, rect_badge, s_f);
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region 鼠标

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (items == null || MouseDownPre(e.X, e.Y)) return;
            if (_tabMenuVisible)
            {
                int i = 0, x = e.X + scroll_x, y = e.Y + scroll_y;
                foreach (var item in items)
                {
                    if (item.Visible && item.Contains(x, y))
                    {
                        item.MDown = true;
                        Invalidate();
                        return;
                    }
                    i++;
                }
            }
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (items == null) return;
            if (_tabMenuVisible)
            {
                int i = 0, x = e.X + scroll_x, y = e.Y + scroll_y;
                foreach (var item in items)
                {
                    if (item.MDown)
                    {
                        item.MDown = false;
                        if (item.Contains(x, y))
                        {
                            if (style.MouseClick(item, i, x, y)) return;
                            TabClick?.Invoke(this, new TabsItemEventArgs(item, style, e));
                            SelectedIndex = i;
                        }
                        else Invalidate();
                        return;
                    }
                    i++;
                }
            }
        }

        /// <summary>
        /// 点击标签时发生
        /// </summary>
        [Description("点击标签时发生"), Category("行为")]
        public event TabsItemEventHandler? TabClick = null;

        int hover_i = -1;
        int Hover_i
        {
            get => hover_i;
            set
            {
                if (hover_i == value) return;
                hover_i = value;
                Invalidate();
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (items == null) return;
            if (MouseMovePre(e.X, e.Y))
            {
                Hover_i = -1;
                SetCursor(true);
                return;
            }
            int i = 0, x = e.X + scroll_x, y = e.Y + scroll_y;
            foreach (var item in items)
            {
                if (item.Visible && item.Contains(x, y))
                {
                    SetCursor(true);
                    Hover_i = i;
                    style.MouseMove(x, y);
                    return;
                }
                i++;
            }
            style.MouseMove(x, y);
            SetCursor(false);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            style.MouseLeave();
            Hover_l = Hover_r = false;
            Hover_i = -1;
            SetCursor(false);
            base.OnMouseLeave(e);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            style.MouseWheel(e.Delta);
            base.OnMouseWheel(e);
        }

        #endregion

        #region 滚动条

        internal bool scroll_show = false;

        int _scroll_x = 0, _scroll_y = 0, scroll_max = 0;
        int scroll_x
        {
            get => _scroll_x;
            set
            {
                if (value < 0) value = 0;
                else if (scroll_max > 0 && value > scroll_max) value = scroll_max;
                if (value == _scroll_x) return;
                _scroll_x = value;
                Invalidate();
            }
        }

        int scroll_y
        {
            get => _scroll_y;
            set
            {
                if (value < 0) value = 0;
                else if (scroll_max > 0 && value > scroll_max) value = scroll_max;
                if (value == _scroll_y) return;
                _scroll_y = value;
                Invalidate();
            }
        }

        bool TabFocusMove(TabPageRect oldTab, TabPageRect newTab, int value, int max)
        {
            if (scroll_show)
            {
                switch (alignment)
                {
                    case TabAlignment.Left:
                    case TabAlignment.Right:
                        int sy = scroll_y;
                        bool showy = newTab.Rect.Y > sy && newTab.Rect.Bottom < sy + ClientRectangle.Height;
                        if (showy) return false;
                        if (value == 0) scroll_y = 0;
                        else if (value == max - 1) scroll_y = scroll_max;
                        else scroll_y = newTab.Rect.Y;
                        return true;
                    case TabAlignment.Top:
                    case TabAlignment.Bottom:
                    default:
                        int sx = scroll_x;
                        bool showx = newTab.Rect.X > sx && newTab.Rect.Right < sx + ClientRectangle.Width;
                        if (showx) return false;
                        if (value == 0) scroll_x = 0;
                        else if (value == max - 1) scroll_x = scroll_max;
                        else scroll_x = newTab.Rect.X;
                        return true;
                }
            }
            return false;
        }

        #region 超出

        LayeredFormSelectDown? subForm = null;
        bool MouseMovePre(int x, int y)
        {
            if (items == null) return false;
            switch (typExceed)
            {
                case TabTypExceed.Button:
                    if (scroll_show && rect_r.Contains(x, y))
                    {
                        if (subForm == null)
                        {
                            var objs = new List<SelectItem>(items.Count);
                            foreach (var item in items) objs.Add(new SelectItem(item.Text, item));
                            subForm = new LayeredFormSelectDown(this, 6, objs.ToArray(), SelectedTab, rect_r);
                            subForm.Disposed += (a, b) =>
                            {
                                subForm = null;
                            };
                            subForm.MouseEnter += (a, b) =>
                            {
                                if (a is LayeredFormSelectDown form) form.tag1 = false;
                            };
                            subForm.MouseLeave += (a, b) =>
                            {
                                if (a is LayeredFormSelectDown form) form.IClose();
                            };
                            subForm.Leave += (a, b) =>
                            {
                                if (a is LayeredFormSelectDown form) form.IClose();
                            };
                            subForm.Show(this);
                        }
                        return true;
                    }
                    else
                    {
                        subForm?.IClose();
                        subForm = null;
                    }
                    break;
                case TabTypExceed.LR:
                case TabTypExceed.LR_Shadow:
                    if (scroll_show)
                    {
                        if (MouseDownLRL(x, y, out bool lr))
                        {
                            Hover_l = true;
                            return true;
                        }
                        else Hover_l = false;
                        if (MouseDownLRR(x, y, out _))
                        {
                            Hover_r = true;
                            return true;
                        }
                        else Hover_r = false;
                    }
                    break;
            }
            return false;
        }
        bool MouseDownPre(int x, int y)
        {
            if (items == null) return false;
            if (scroll_show && (typExceed == TabTypExceed.LR || typExceed == TabTypExceed.LR_Shadow))
            {
                if (MouseDownLRL(x, y, out bool lr))
                {
                    if (lr) scroll_y -= 120;
                    else scroll_x -= 120;
                    return true;
                }
                else if (MouseDownLRR(x, y, out bool lr2))
                {
                    if (lr2) scroll_y += 120;
                    else scroll_x += 120;
                    return true;
                }
            }
            return false;
        }

        bool MouseDownLRL(int x, int y, out bool lr)
        {
            switch (alignment)
            {
                case TabAlignment.Left:
                case TabAlignment.Right:
                    lr = true;
                    if (scroll_y > 0 && rect_l.Contains(x, y)) return true;
                    return false;
                case TabAlignment.Top:
                case TabAlignment.Bottom:
                default:
                    lr = false;
                    if (scroll_x > 0 && rect_l.Contains(x, y)) return true;
                    return false;
            }
        }
        bool MouseDownLRR(int x, int y, out bool lr)
        {
            switch (alignment)
            {
                case TabAlignment.Left:
                case TabAlignment.Right:
                    lr = true;
                    if (scroll_max != scroll_y && rect_r.Contains(x, y)) return true;
                    return false;
                case TabAlignment.Top:
                case TabAlignment.Bottom:
                default:
                    lr = false;
                    if (scroll_max != scroll_x && rect_r.Contains(x, y)) return true;
                    return false;
            }
        }

        #region 绘制超出部分

        public virtual int SizeExceed(Rectangle rect, Rectangle first, Rectangle last)
        {
            switch (typExceed)
            {
                case TabTypExceed.Button:
                    int size = last.Height;
                    switch (alignment)
                    {
                        case TabAlignment.Left:
                        case TabAlignment.Right:
                            rect_r = new Rectangle(last.X, rect.Bottom - size, last.Width, size);
                            break;
                        case TabAlignment.Top:
                        case TabAlignment.Bottom:
                        default:
                            rect_r = new Rectangle(rect.Right - size, last.Y, size, size);
                            break;
                    }
                    return size;
                case TabTypExceed.None:
                default:
                    rect_r = Rectangle.Empty;
                    return 0;
            }
        }

        public virtual Rectangle PaintExceedPre(Rectangle rect, int size)
        {
            switch (typExceed)
            {
                case TabTypExceed.Button:
                    switch (alignment)
                    {
                        case TabAlignment.Left:
                        case TabAlignment.Right:
                            if (scroll_max != scroll_y) return new Rectangle(rect.X, rect.Y, rect.Width, rect.Height - size);
                            else return rect;
                        case TabAlignment.Top:
                        case TabAlignment.Bottom:
                        default:
                            if (scroll_max != scroll_x) return new Rectangle(rect.X, rect.Y, rect.Width - size, rect.Height);
                            else return rect;
                    }
                case TabTypExceed.LR:
                case TabTypExceed.LR_Shadow:
                    switch (alignment)
                    {
                        case TabAlignment.Left:
                        case TabAlignment.Right:
                            int sizeLR2 = (int)(size * .6F);
                            if (scroll_max != scroll_y)
                            {
                                if (scroll_y > 0) return new Rectangle(rect.X, rect.Y + sizeLR2, rect.Width, rect.Height - sizeLR2 * 2);
                                else return new Rectangle(rect.X, rect.Y, rect.Width, rect.Height - sizeLR2);
                            }
                            else if (scroll_y > 0) return new Rectangle(rect.X, rect.Y + sizeLR2, rect.Width, rect.Height - sizeLR2);
                            else return rect;
                        case TabAlignment.Top:
                        case TabAlignment.Bottom:
                        default:
                            int sizeLR = (int)(size * .6F);
                            if (scroll_max != scroll_x)
                            {
                                if (scroll_x > 0) return new Rectangle(rect.X + sizeLR, rect.Y, rect.Width - sizeLR * 2, rect.Height);
                                else return new Rectangle(rect.X, rect.Y, rect.Width - sizeLR, rect.Height);
                            }
                            else if (scroll_x > 0) return new Rectangle(rect.X + sizeLR, rect.Y, rect.Width - sizeLR, rect.Height);
                            else return rect;
                    }
                case TabTypExceed.None:
                default:
                    return rect;
            }
        }

        Bitmap? bitblock_l = null, bitblock_r = null;
        public virtual void PaintExceed(Canvas g, Color color, int radius, Rectangle rect, Rectangle first, Rectangle last, bool full)
        {
            switch (typExceed)
            {
                case TabTypExceed.Button:
                    PaintExceedButton(g, color, radius, rect, first, last, full);
                    break;
                case TabTypExceed.LR:
                    PaintExceedLR(g, color, radius, rect, first, last, full);
                    break;
                case TabTypExceed.LR_Shadow:
                    PaintExceedLR_Shadow(g, color, radius, rect, first, last, full);
                    break;
                case TabTypExceed.None:
                default:
                    break;
            }
        }

        public virtual void PaintExceedButton(Canvas g, Color color, int radius, Rectangle rect, Rectangle first, Rectangle last, bool full)
        {
            g.ResetClip();
            g.ResetTransform();
            switch (alignment)
            {
                case TabAlignment.Left:
                case TabAlignment.Right:
                    if (scroll_y > 0 || scroll_max != scroll_y)
                    {
                        int gap = (int)(_gap * Config.Dpi), gap2 = gap * 2;
                        int size = last.Height, icosize = (int)(size * 0.4F);
                        var rect_cr = new Rectangle(last.X, rect.Bottom - size, last.Width, size);
                        if (scroll_y > 0)
                        {
                            var rect_l = new Rectangle(first.X, first.Y, rect_cr.Width, gap2);
                            if (full) rect_l.Y = 0;
                            if (bitblock_l == null || bitblock_l.Width != rect_l.Width || bitblock_l.Height != rect_l.Height)
                            {
                                bitblock_l?.Dispose();
                                bitblock_l = new Bitmap(rect_l.Width, rect_l.Height);
                                using (var g_bmp = Graphics.FromImage(bitblock_l).HighLay())
                                {
                                    using (var brush = new SolidBrush(color))
                                    {
                                        using (var path = new Rectangle(0, 0, bitblock_l.Width, gap).RoundPath(gap, false, false, true, true))
                                        {
                                            g_bmp.Fill(brush, path);
                                        }
                                    }
                                }
                                Helper.Blur(bitblock_l, gap);
                            }
                            g.Image(bitblock_l, rect_l, .1F);
                        }
                        if (scroll_max != scroll_y)
                        {
                            var rect_r = new Rectangle(rect_cr.X, rect_cr.Y - gap2, rect_cr.Width, gap2);
                            if (bitblock_r == null || bitblock_r.Width != rect_r.Width || bitblock_r.Height != rect_r.Height)
                            {
                                bitblock_r?.Dispose();
                                bitblock_r = new Bitmap(rect_r.Width, rect_r.Height);
                                using (var g_bmp = Graphics.FromImage(bitblock_r).HighLay())
                                {
                                    using (var brush = new SolidBrush(color))
                                    {
                                        using (var path = new Rectangle(0, gap, bitblock_r.Width, gap).RoundPath(gap, true, true, false, false))
                                        {
                                            g_bmp.Fill(brush, path);
                                        }
                                    }
                                }
                                Helper.Blur(bitblock_r, gap);
                            }
                            g.Image(bitblock_r, rect_r, .1F);
                        }
                        var rect_ico = new Rectangle(rect_cr.X + (rect_cr.Width - icosize) / 2, rect_cr.Y + (rect_cr.Height - icosize) / 2, icosize, icosize);
                        g.GetImgExtend(SvgDb.IcoMore, rect_ico, color);
                    }
                    break;
                case TabAlignment.Top:
                case TabAlignment.Bottom:
                default:
                    if (scroll_x > 0 || scroll_max != scroll_x)
                    {
                        int gap = (int)(_gap * Config.Dpi), gap2 = gap * 2;
                        int size = last.Height, icosize = (int)(size * 0.4F);
                        var rect_cr = new Rectangle(rect.Right - size, last.Y, size, size);
                        if (scroll_x > 0)
                        {
                            var rect_l = new Rectangle(first.X, first.Y, gap2, size);
                            if (full) rect_l.X = 0;
                            if (bitblock_l == null || bitblock_l.Width != rect_l.Width || bitblock_l.Height != rect_l.Height)
                            {
                                bitblock_l?.Dispose();
                                bitblock_l = new Bitmap(rect_l.Width, rect_l.Height);
                                using (var g_bmp = Graphics.FromImage(bitblock_l).HighLay())
                                {
                                    using (var brush = new SolidBrush(color))
                                    {
                                        using (var path = new Rectangle(0, 0, gap, bitblock_l.Height).RoundPath(gap, false, true, true, false))
                                        {
                                            g_bmp.Fill(brush, path);
                                        }
                                    }
                                }
                                Helper.Blur(bitblock_l, gap);
                            }
                            g.Image(bitblock_l, rect_l, .1F);
                        }
                        if (scroll_max != scroll_x)
                        {
                            var rect_r = new Rectangle(rect_cr.X - gap2, rect_cr.Y, gap2, size);
                            if (bitblock_r == null || bitblock_r.Width != rect_r.Width || bitblock_r.Height != rect_r.Height)
                            {
                                bitblock_r?.Dispose();
                                bitblock_r = new Bitmap(rect_r.Width, rect_r.Height);
                                using (var g_bmp = Graphics.FromImage(bitblock_r).HighLay())
                                {
                                    using (var brush = new SolidBrush(color))
                                    {
                                        using (var path = new Rectangle(gap, 0, gap, bitblock_r.Height).RoundPath(gap, true, false, false, true))
                                        {
                                            g_bmp.Fill(brush, path);
                                        }
                                    }
                                }
                                Helper.Blur(bitblock_r, gap);
                            }
                            g.Image(bitblock_r, rect_r, .1F);
                        }
                        var rect_ico = new Rectangle(rect_cr.X + (rect_cr.Width - icosize) / 2, rect_cr.Y + (rect_cr.Height - icosize) / 2, icosize, icosize);
                        g.GetImgExtend(SvgDb.IcoMore, rect_ico, color);
                    }
                    break;
            }
        }
        public virtual void PaintExceedLR(Canvas g, Color color, int radius, Rectangle rect, Rectangle first, Rectangle last, bool full)
        {
            g.ResetClip();
            g.ResetTransform();
            switch (alignment)
            {
                case TabAlignment.Left:
                case TabAlignment.Right:
                    if (scroll_y > 0 || scroll_max != scroll_y)
                    {
                        int size = (int)(last.Height * .6F);
                        using (var brush = new SolidBrush(scrollback ?? Colour.FillSecondary.Get("Tabs")))
                        using (var brush_hover = new SolidBrush(ScrollBackHover ?? Colour.Primary.Get("Tabs")))
                        using (var pen = new Pen(scrollfore ?? color, 2F * Config.Dpi))
                        using (var pen_hover = new Pen(ScrollForeHover ?? Colour.PrimaryColor.Get("Tabs"), 2F * Config.Dpi))
                        {
                            if (scroll_y > 0)
                            {
                                rect_l = new Rectangle(last.X, rect.Y, last.Width, size);
                                using (var path = Helper.RoundPath(rect_l, radius))
                                {
                                    if (hover_l)
                                    {
                                        g.Fill(brush_hover, path);
                                        g.DrawLines(pen_hover, TAlignMini.Top.TriangleLines(rect_l, .5F));
                                    }
                                    else
                                    {
                                        g.Fill(brush, path);
                                        g.DrawLines(pen, TAlignMini.Top.TriangleLines(rect_l, .5F));
                                    }
                                }
                            }
                            if (scroll_max != scroll_y)
                            {
                                rect_r = new Rectangle(last.X, rect.Y + rect.Height - size, last.Width, size);
                                using (var path = Helper.RoundPath(rect_r, radius))
                                {
                                    if (hover_r)
                                    {
                                        g.Fill(brush_hover, path);
                                        g.DrawLines(pen_hover, TAlignMini.Bottom.TriangleLines(rect_r, .5F));
                                    }
                                    else
                                    {
                                        g.Fill(brush, path);
                                        g.DrawLines(pen, TAlignMini.Bottom.TriangleLines(rect_r, .5F));
                                    }
                                }
                            }
                        }
                    }
                    break;
                case TabAlignment.Top:
                case TabAlignment.Bottom:
                default:
                    if (scroll_x > 0 || scroll_max != scroll_x)
                    {
                        int size = (int)(last.Height * .6F);
                        using (var brush = new SolidBrush(scrollback ?? Colour.FillSecondary.Get("Tabs")))
                        using (var brush_hover = new SolidBrush(ScrollBackHover ?? Colour.Primary.Get("Tabs")))
                        using (var pen = new Pen(scrollfore ?? color, 2F * Config.Dpi))
                        using (var pen_hover = new Pen(ScrollForeHover ?? Colour.PrimaryColor.Get("Tabs"), 2F * Config.Dpi))
                        {
                            if (scroll_x > 0)
                            {
                                rect_l = new Rectangle(rect.X, last.Y, size, last.Height);
                                using (var path = Helper.RoundPath(rect_l, radius))
                                {
                                    if (hover_l)
                                    {
                                        g.Fill(brush_hover, path);
                                        g.DrawLines(pen_hover, TAlignMini.Left.TriangleLines(rect_l, .5F));
                                    }
                                    else
                                    {
                                        g.Fill(brush, path);
                                        g.DrawLines(pen, TAlignMini.Left.TriangleLines(rect_l, .5F));
                                    }
                                }
                            }
                            if (scroll_max != scroll_x)
                            {
                                rect_r = new Rectangle(rect.X + rect.Width - size, last.Y, size, last.Height);
                                using (var path = Helper.RoundPath(rect_r, radius))
                                {
                                    if (hover_r)
                                    {
                                        g.Fill(brush_hover, path);
                                        g.DrawLines(pen_hover, TAlignMini.Right.TriangleLines(rect_r, .5F));
                                    }
                                    else
                                    {
                                        g.Fill(brush, path);
                                        g.DrawLines(pen, TAlignMini.Right.TriangleLines(rect_r, .5F));
                                    }
                                }
                            }
                        }
                    }
                    break;
            }
        }
        public virtual void PaintExceedLR_Shadow(Canvas g, Color color, int radius, Rectangle rect, Rectangle first, Rectangle last, bool full)
        {
            g.ResetClip();
            g.ResetTransform();
            switch (alignment)
            {
                case TabAlignment.Left:
                case TabAlignment.Right:
                    if (scroll_y > 0 || scroll_max != scroll_y)
                    {
                        int gap = (int)(_gap * Config.Dpi), gap2 = gap * 2, size = (int)(last.Height * .6F);
                        using (var brush = new SolidBrush(scrollback ?? Colour.FillSecondary.Get("Tabs")))
                        using (var brush_hover = new SolidBrush(ScrollBackHover ?? Colour.Primary.Get("Tabs")))
                        using (var pen = new Pen(scrollfore ?? color, 2F * Config.Dpi))
                        using (var pen_hover = new Pen(ScrollForeHover ?? Colour.PrimaryColor.Get("Tabs"), 2F * Config.Dpi))
                        {
                            if (scroll_y > 0)
                            {
                                rect_l = new Rectangle(last.X, rect.Y, last.Width, size);

                                var Rect_l = new Rectangle(rect_l.X, rect_l.Bottom, rect_l.Width, gap2);
                                if (bitblock_l == null || bitblock_l.Width != Rect_l.Width || bitblock_l.Height != Rect_l.Height)
                                {
                                    bitblock_l?.Dispose();
                                    bitblock_l = new Bitmap(Rect_l.Width, Rect_l.Height);
                                    using (var g_bmp = Graphics.FromImage(bitblock_l).HighLay())
                                    {
                                        using (var path = new Rectangle(0, 0, bitblock_l.Width, gap).RoundPath(gap, false, false, true, true))
                                        {
                                            g_bmp.Fill(brush, path);
                                        }
                                    }
                                    Helper.Blur(bitblock_l, gap);
                                }
                                g.Image(bitblock_l, Rect_l, .1F);

                                using (var path = Helper.RoundPath(rect_l, radius, true, true, false, false))
                                {
                                    if (hover_l)
                                    {
                                        g.Fill(brush_hover, path);
                                        g.DrawLines(pen_hover, TAlignMini.Top.TriangleLines(rect_l, .5F));
                                    }
                                    else
                                    {
                                        g.Fill(brush, path);
                                        g.DrawLines(pen, TAlignMini.Top.TriangleLines(rect_l, .5F));
                                    }
                                }
                            }
                            if (scroll_max != scroll_y)
                            {
                                rect_r = new Rectangle(last.X, rect.Y + rect.Height - size, last.Width, size);

                                var Rect_r = new Rectangle(rect_r.X, rect_r.Y - gap2, rect_r.Width, gap2);
                                if (bitblock_r == null || bitblock_r.Width != Rect_r.Width || bitblock_r.Height != Rect_r.Height)
                                {
                                    bitblock_r?.Dispose();
                                    bitblock_r = new Bitmap(Rect_r.Width, Rect_r.Height);
                                    using (var g_bmp = Graphics.FromImage(bitblock_r).HighLay())
                                    {
                                        using (var path = new Rectangle(0, gap, bitblock_r.Width, gap).RoundPath(gap, true, true, false, false))
                                        {
                                            g_bmp.Fill(brush, path);
                                        }
                                    }
                                    Helper.Blur(bitblock_r, gap);
                                }
                                g.Image(bitblock_r, Rect_r, .1F);

                                using (var path = Helper.RoundPath(rect_r, radius, false, false, true, true))
                                {
                                    if (hover_r)
                                    {
                                        g.Fill(brush_hover, path);
                                        g.DrawLines(pen_hover, TAlignMini.Bottom.TriangleLines(rect_r, .5F));
                                    }
                                    else
                                    {
                                        g.Fill(brush, path);
                                        g.DrawLines(pen, TAlignMini.Bottom.TriangleLines(rect_r, .5F));
                                    }
                                }
                            }
                        }
                    }
                    break;
                case TabAlignment.Top:
                case TabAlignment.Bottom:
                default:
                    if (scroll_x > 0 || scroll_max != scroll_x)
                    {
                        int gap = (int)(_gap * Config.Dpi), gap2 = gap * 2, size = (int)(last.Height * .6F);
                        using (var brush = new SolidBrush(scrollback ?? Colour.FillSecondary.Get("Tabs")))
                        using (var brush_hover = new SolidBrush(ScrollBackHover ?? Colour.Primary.Get("Tabs")))
                        using (var pen = new Pen(scrollfore ?? color, 2F * Config.Dpi))
                        using (var pen_hover = new Pen(ScrollForeHover ?? Colour.PrimaryColor.Get("Tabs"), 2F * Config.Dpi))
                        {
                            if (scroll_x > 0)
                            {
                                rect_l = new Rectangle(rect.X, last.Y, size, last.Height);

                                var Rect_l = new Rectangle(rect_l.Right, rect_l.Y, gap2, rect_l.Height);
                                if (bitblock_l == null || bitblock_l.Width != Rect_l.Width || bitblock_l.Height != Rect_l.Height)
                                {
                                    bitblock_l?.Dispose();
                                    bitblock_l = new Bitmap(Rect_l.Width, Rect_l.Height);
                                    using (var g_bmp = Graphics.FromImage(bitblock_l).HighLay())
                                    {
                                        using (var path = new Rectangle(0, 0, gap, bitblock_l.Height).RoundPath(gap, false, true, true, false))
                                        {
                                            g_bmp.Fill(brush, path);
                                        }
                                    }
                                    Helper.Blur(bitblock_l, gap);
                                }
                                g.Image(bitblock_l, Rect_l, .1F);

                                using (var path = Helper.RoundPath(rect_l, radius, true, false, false, true))
                                {
                                    if (hover_l)
                                    {
                                        g.Fill(brush_hover, path);
                                        g.DrawLines(pen_hover, TAlignMini.Left.TriangleLines(rect_l, .5F));
                                    }
                                    else
                                    {
                                        g.Fill(brush, path);
                                        g.DrawLines(pen, TAlignMini.Left.TriangleLines(rect_l, .5F));
                                    }
                                }
                            }
                            if (scroll_max != scroll_x)
                            {
                                rect_r = new Rectangle(rect.X + rect.Width - size, last.Y, size, last.Height);

                                var Rect_r = new Rectangle(rect_r.X - gap2, rect_r.Y, gap2, rect_r.Height);
                                if (bitblock_r == null || bitblock_r.Width != Rect_r.Width || bitblock_r.Height != Rect_r.Height)
                                {
                                    bitblock_r?.Dispose();
                                    bitblock_r = new Bitmap(Rect_r.Width, Rect_r.Height);
                                    using (var g_bmp = Graphics.FromImage(bitblock_r).HighLay())
                                    {
                                        using (var path = new Rectangle(gap, 0, gap, bitblock_r.Height).RoundPath(gap, true, false, false, true))
                                        {
                                            g_bmp.Fill(brush, path);
                                        }
                                    }
                                    Helper.Blur(bitblock_r, gap);
                                }
                                g.Image(bitblock_r, Rect_r, .1F);

                                using (var path = Helper.RoundPath(rect_r, radius, false, true, true, false))
                                {
                                    if (hover_r)
                                    {
                                        g.Fill(brush_hover, path);
                                        g.DrawLines(pen_hover, TAlignMini.Right.TriangleLines(rect_r, .5F));
                                    }
                                    else
                                    {
                                        g.Fill(brush, path);
                                        g.DrawLines(pen, TAlignMini.Right.TriangleLines(rect_r, .5F));
                                    }
                                }
                            }
                        }
                    }
                    break;
            }
        }

        Rectangle rect_l, rect_r;
        bool hover_l, hover_r = false;
        bool Hover_l
        {
            get => hover_l;
            set
            {
                if (hover_l == value) return;
                hover_l = value;
                Invalidate();
            }
        }
        bool Hover_r
        {
            get => hover_r;
            set
            {
                if (hover_r == value) return;
                hover_r = value;
                Invalidate();
            }
        }

        #endregion

        #endregion

        #endregion

        #region 事件

        /// <summary>
        /// SelectedIndex 属性值更改时发生
        /// </summary>
        [Description("SelectedIndex 属性值更改时发生"), Category("行为")]
        public event IntEventHandler? SelectedIndexChanged = null;

        /// <summary>
        /// 关闭页面前发生
        /// </summary>
        [Description("关闭页面前发生"), Category("行为")]
        public event ClosingPageEventHandler? ClosingPage;

        #endregion
    }

    public class TabCollection : iCollection<TabPage>
    {
        public TabCollection(Tabs it)
        {
            BindData(it);
        }

        internal TabCollection BindData(Tabs it)
        {
            action = render =>
            {
                if (render) it.LoadLayout(false);
                it.Invalidate();
            };
            action_add = item =>
            {
                item.PARENT = it;
                item.SetDock(it.Controls.Count == 0);
                it.Controls.Add(item);
            };
            action_del = (item, index) =>
            {
                if (index == -1) it.SelectedIndex = 0;
                else
                {
                    int old = it.SelectedIndex;
                    if (old == index)
                    {
                        int _new = index - 1;
                        if (_new > -1) it.SelectedIndex = _new;
                        else it.ShowPage(_new);
                    }
                    else if (old > index) it.SelectedIndex = old - 1;
                }
                // 针对 #IBLKEA 修正
                it.Controls.Remove(item);
            };
            return this;
        }
    }

    [ToolboxItem(false)]
    [Designer(typeof(IControlDesigner))]
    public class TabPage : ScrollableControl
    {
        public TabPage()
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

        #region 属性

        DockStyle dock = DockStyle.Fill;
        /// <summary>
        /// 定义要绑定到容器的控件边框
        /// </summary>
        [Category("布局"), Description("定义要绑定到容器的控件边框"), DefaultValue(DockStyle.Fill)]
        public new DockStyle Dock
        {
            get => dock;
            set => dock = value;
        }

        Image? icon = null;
        /// <summary>
        /// 图标
        /// </summary>
        [Category("外观"), Description("图标"), DefaultValue(null)]
        public Image? Icon
        {
            get => icon;
            set
            {
                if (icon == value) return;
                icon = value;
                PARENT?.LoadLayout();
            }
        }

        string? iconSvg = null;
        /// <summary>
        /// 图标
        /// </summary>
        [Category("外观"), Description("图标SVG"), DefaultValue(null)]
        public string? IconSvg
        {
            get => iconSvg;
            set
            {
                if (iconSvg == value) return;
                iconSvg = value;
                PARENT?.LoadLayout();
            }
        }

        /// <summary>
        /// 是否包含图标
        /// </summary>
        public bool HasIcon => iconSvg != null || icon != null;

        bool readOnly = false;
        /// <summary>
        /// 只读
        /// </summary>
        [Description("只读"), Category("行为"), DefaultValue(false)]
        public bool ReadOnly
        {
            get => readOnly;
            set
            {
                if (readOnly == value) return;
                readOnly = value;
                PARENT?.LoadLayout();
            }
        }

        #region 徽标

        string? badge = null;
        [Description("徽标内容"), Category("徽标"), DefaultValue(null)]
        public string? Badge
        {
            get => badge;
            set
            {
                if (badge == value) return;
                badge = value;
                PARENT?.Invalidate();
            }
        }

        float badgeSize = .6F;
        [Description("徽标比例"), Category("徽标"), DefaultValue(.6F)]
        public float BadgeSize
        {
            get => badgeSize;
            set
            {
                if (badgeSize != value)
                {
                    badgeSize = value;
                    if (badge != null) PARENT?.Invalidate();
                }
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
                if (badgeback != value)
                {
                    badgeback = value;
                    if (badge != null) PARENT?.Invalidate();
                }
            }
        }

        /// <summary>
        /// 徽标偏移X
        /// </summary>
        [Description("徽标偏移X"), Category("徽标"), DefaultValue(1)]
        public int BadgeOffsetX { get; set; } = 1;

        /// <summary>
        /// 徽标偏移Y
        /// </summary>
        [Description("徽标偏移Y"), Category("徽标"), DefaultValue(1)]
        public int BadgeOffsetY { get; set; } = 1;

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
                PARENT?.LoadLayout();
            }
        }

        [Description("文本"), Category("国际化"), DefaultValue(null)]
        public string? LocalizationText { get; set; }

        #endregion

        #endregion

        #region 坐标

        internal bool MDown = false;
        internal Rectangle Rect = new Rectangle(-10, -10, 0, 0);
        internal bool Contains(int x, int y)
        {
            return Rect.Contains(x, y);
        }
        internal Rectangle SetRect(Rectangle rect)
        {
            Rect = rect;
            return Rect;
        }
        internal Rectangle SetOffset(int x, int y)
        {
            Rect.Offset(x, y);
            return Rect;
        }

        #endregion

        #region 变更

        internal Tabs? PARENT;
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

        #endregion

        public void SetDock(bool isdock)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => SetDock(isdock)));
                return;
            }
            if (isdock) base.Dock = dock;
            else
            {
                base.Dock = DockStyle.None;
                Location = new Point(-Width, -Height);
            }
        }

        public override string ToString() => Text;
    }
}