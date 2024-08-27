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
    [DefaultProperty("TabPages")]
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
                if (fore == value) fore = value;
                fore = value;
                Invalidate();
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
            }
        }

        #region 样式

        IStyle style;
        /// <summary>
        /// 样式
        /// </summary>
        [Description("样式"), Category("外观")]
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
            }
        }

        internal Dictionary<TabPage, Size> HandItemSize(Dictionary<TabPage, Size> rect_dir)
        {
            if (_itemSize.HasValue)
            {
                int Size = (int)Math.Ceiling(_itemSize.Value * Config.Dpi);
                switch (alignment)
                {
                    case TabAlignment.Left:
                    case TabAlignment.Right:
                        var rect_dirtmp = new Dictionary<TabPage, Size>(rect_dir.Count);
                        foreach (var it in rect_dir) rect_dirtmp.Add(it.Key, new Size(it.Value.Width, Size));
                        return rect_dirtmp;
                    case TabAlignment.Top:
                    case TabAlignment.Bottom:
                    default:
                        var rect_dirtmph = new Dictionary<TabPage, Size>(rect_dir.Count);
                        foreach (var it in rect_dir) rect_dirtmph.Add(it.Key, new Size(Size, it.Value.Height));
                        return rect_dirtmph;
                }
            }
            return rect_dir;
        }

        public override Rectangle DisplayRectangle
        {
            get => ClientRectangle.PaddingRect(Margin, Padding, _padding);
        }

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

        public void SelectTab(TabPage tabPage)
        {
            SelectedTab = tabPage;
        }

        public void SelectTab(int index)
        {
            SelectedIndex = index;
        }

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
                ShowPage();
            }
        }

        internal void ShowPage()
        {
            if (IsHandleCreated)
            {
                BeginInvoke(new Action(() =>
                {
                    Controls.Clear();
                    if (items == null) return;
                    if (items.Count <= _select || _select < 0) return;
                    var item = items[_select];
                    Controls.Add(item);
                }));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new ControlCollection Controls => base.Controls;

        protected override void Dispose(bool disposing)
        {
            style.Dispose();
            base.Dispose(disposing);
        }

        #endregion

        #endregion

        #endregion

        #region 布局

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            LoadLayout(false);
            ShowPage();
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

        StringFormat s_c = Helper.SF_ALL();
        protected override void OnPaint(PaintEventArgs e)
        {
            if (items == null || items.Count == 0 || !_tabMenuVisible) return;
            var g = e.Graphics.High();
            style.Paint(this, g, items);
            base.OnPaint(e);
        }

        void PaintBadge(Graphics g, TabPage page, Rectangle rect)
        {
            if (page.Badge != null)
            {
                var color = page.BadgeBack ?? AntdUI.Style.Db.Error;
                using (var brush_fore = new SolidBrush(AntdUI.Style.Db.ErrorColor))
                {
                    float borsize = 1F * Config.Dpi;
                    using (var font = new Font(Font.FontFamily, Font.Size * page.BadgeSize))
                    {
                        if (string.IsNullOrEmpty(page.Badge) || page.Badge == "" || page.Badge == " ")
                        {
                            var size = (int)Math.Floor(g.MeasureString(Config.NullText, font).Width / 2);
                            var rect_badge = new RectangleF(rect.Right - size - page.BadgeOffsetX * Config.Dpi, rect.Y + page.BadgeOffsetY * Config.Dpi, size, size);
                            using (var brush = new SolidBrush(color))
                            {
                                g.FillEllipse(brush, rect_badge);
                                using (var pen = new Pen(brush_fore.Color, borsize))
                                {
                                    g.DrawEllipse(pen, rect_badge);
                                }
                            }
                        }
                        else
                        {
                            var size = g.MeasureString(page.Badge, font);
                            var size_badge = size.Height * 1.2F;
                            if (size.Height > size.Width)
                            {
                                var rect_badge = new RectangleF(rect.Right - size_badge - page.BadgeOffsetX * Config.Dpi, rect.Y + page.BadgeOffsetY * Config.Dpi, size_badge, size_badge);
                                using (var brush = new SolidBrush(color))
                                {
                                    g.FillEllipse(brush, rect_badge);
                                    using (var pen = new Pen(brush_fore.Color, borsize))
                                    {
                                        g.DrawEllipse(pen, rect_badge);
                                    }
                                }
                                g.DrawStr(page.Badge, font, brush_fore, rect_badge, Helper.stringFormatCenter2);
                            }
                            else
                            {
                                var w_badge = size.Width * 1.2F;
                                var rect_badge = new RectangleF(rect.Right - w_badge - page.BadgeOffsetX * Config.Dpi, rect.Y + page.BadgeOffsetY * Config.Dpi, w_badge, size_badge);
                                using (var brush = new SolidBrush(color))
                                {
                                    using (var path = rect_badge.RoundPath(rect_badge.Height))
                                    {
                                        g.FillPath(brush, path);
                                        using (var pen = new Pen(brush_fore.Color, borsize))
                                        {
                                            g.DrawPath(pen, path);
                                        }
                                    }
                                }
                                g.DrawStr(page.Badge, font, brush_fore, rect_badge, Helper.stringFormatCenter2);
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
            if (items == null) { base.OnMouseDown(e); return; }
            if (style.ScrollMouseEvent("down", e.X, e.Y))
            {
                Invalidate();
                base.OnMouseDown(e);
                return;
            }
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
            base.OnMouseDown(e);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (items == null) { base.OnMouseUp(e); return; }
            if (style.ScrollMouseEvent("up", e.X, e.Y))
            {
                //Invalidate();
                base.OnMouseUp(e);
                return;
            }
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
                            SelectedIndex = i;
                        }
                        else Invalidate();
                        return;
                    }
                    i++;
                }
            }
            base.OnMouseUp(e);
        }

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
            if (items == null) return;
            if (style.ScrollMouseEvent("move", e.X, e.Y))
            {
                hover_i = -1;
                Invalidate();
                Cursor = DefaultCursor;
                base.OnMouseMove(e);
                return;
            }
            if (style.MouseMovePre(e.X, e.Y))
            {
                Cursor = DefaultCursor;
                base.OnMouseMove(e);
                return;
            }
            int i = 0, x = e.X + scroll_x, y = e.Y + scroll_y;
            foreach (var item in items)
            {
                if (item.Visible && item.Contains(x, y))
                {
                    Cursor = Cursors.Hand;
                    Hover_i = i;
                    style.MouseMove(x, y);
                    return;
                }
                i++;
            }
            style.MouseMove(x, y);
            Cursor = DefaultCursor;
            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            style.ScrollMouseEvent("leave");
            if (Hover_i == -1) Invalidate();
            Hover_i = -1;
            style.MouseLeave();
            Cursor = DefaultCursor;
            base.OnMouseLeave(e);
        }

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
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            style.MouseWheel(e.Delta);
            base.OnMouseWheel(e);
        }

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
        public bool HasIcon
        {
            get => iconSvg != null || icon != null;
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
                Invalidate();
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

        #endregion

        #region 坐标

        internal bool HDPI = false;
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
    }
}