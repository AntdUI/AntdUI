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

namespace AntdUI.Chat
{
    /// <summary>
    /// MsgList 好友消息列表
    /// </summary>
    /// <remarks>好友消息列表。</remarks>
    [Description("MsgList 好友消息列表")]
    [ToolboxItem(true)]
    public class MsgList : IControl
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
                OnPropertyChanged(nameof(ForeColor));
            }
        }

        /// <summary>
        /// 悬停背景色
        /// </summary>
        [Description("悬停背景色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? BackHover { get; set; }

        /// <summary>
        /// 激活背景颜色
        /// </summary>
        [Description("激活背景颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? BackActive { get; set; }

        /// <summary>
        /// 激活文字颜色
        /// </summary>
        [Description("激活文字颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? ForeActive { get; set; }

        /// <summary>
        /// 头像圆角
        /// </summary>
        [Description("头像圆角"), Category("外观"), DefaultValue(6)]
        public int IconRadius { get; set; } = 6;

        /// <summary>
        /// 圆形头像
        /// </summary>
        [Description("圆形头像"), Category("外观"), DefaultValue(true)]
        public bool IconRound { get; set; } = true;

        /// <summary>
        /// 圆形布局
        /// </summary>
        [Description("圆形布局"), Category("外观"), DefaultValue(TFit.Cover)]
        public TFit IconFit { get; set; } = TFit.Cover;

        MsgItemCollection? items;
        /// <summary>
        /// 数据集合
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("数据集合"), Category("数据")]
        public MsgItemCollection Items
        {
            get
            {
                items ??= new MsgItemCollection(this);
                return items;
            }
            set => items = value.BindData(this);
        }

        /// <summary>
        /// 滚动条
        /// </summary>
        [Browsable(false)]
        public ScrollBar ScrollBar;

        #endregion

        #region 渲染

        protected override void OnDraw(DrawEventArgs e)
        {
            if (items == null || items.Count == 0)
            {
                base.OnDraw(e);
                return;
            }
            var g = e.Canvas;
            int sy = ScrollBar.Value, radius = (int)Math.Ceiling(IconRadius * Config.Dpi);
            g.TranslateTransform(0, -sy);
            using (var font_text = new Font(Font.FontFamily, Font.Size * .9F))
            using (var font_time = new Font(Font.FontFamily, Font.Size * .82F))
            {
                foreach (var it in items) PaintItem(g, it, e.Rect, sy, font_text, font_time, radius);
            }
            g.ResetTransform();
            ScrollBar.Paint(g, ColorScheme);
            base.OnDraw(e);
        }

        StringFormat SFBage = Helper.SF();
        StringFormat SFL = Helper.SF_ALL(lr: StringAlignment.Near);
        StringFormat SFR = Helper.SF_ALL(lr: StringAlignment.Far);
        void PaintItem(Canvas g, MsgItem it, Rectangle rect, float sy, Font font_text, Font font_time, int radius)
        {
            it.show = it.Show && it.Visible && it.rect.Y > sy - rect.Height && it.rect.Bottom < ScrollBar.Value + ScrollBar.ReadSize + it.rect.Height;
            if (it.show)
            {
                if (it.Select)
                {
                    g.Fill(BackActive ?? Color.FromArgb(0, 153, 255), it.rect);
                    using (var brush = new SolidBrush(ForeActive ?? Color.White))
                    {
                        try
                        {
                            g.String(it.Name, Font, brush, it.rect_name, SFL);
                            g.String(it.Text, it.TextFont ?? font_text, brush, it.rect_text, it.TextFormat ?? SFL);
                            g.String(it.Time, it.TimeFont ?? font_time, brush, it.rect_time, SFR);
                        }
                        catch { }
                    }
                }
                else
                {
                    if (it.Hover) g.Fill(BackHover ?? Colour.FillTertiary.Get(nameof(MsgList), ColorScheme), it.rect);
                    using (var brush = new SolidBrush(fore ?? Colour.Text.Get(nameof(MsgList), ColorScheme)))
                    {
                        try
                        {
                            g.String(it.Name, Font, brush, it.rect_name, SFL);
                            if (it.TextColor.HasValue) g.String(it.Text, it.TextFont ?? font_text, it.TextColor.Value, it.rect_text, it.TextFormat ?? SFL);
                            else g.String(it.Text, it.TextFont ?? font_text, brush, it.rect_text, it.TextFormat ?? SFL);
                            if (it.TimeColor.HasValue) g.String(it.Time, it.TimeFont ?? font_time, it.TimeColor.Value, it.rect_time, SFR);
                            else g.String(it.Time, it.TimeFont ?? font_time, brush, it.rect_time, SFR);
                        }
                        catch { }
                    }
                }
                if (it.Icon != null)
                {
                    g.Image(it.rect_icon, it.Icon, IconFit, radius, IconRound);
                    if (it.Badge != null)
                    {
                        if (string.IsNullOrEmpty(it.Badge))
                        {
                            int badge_size = it.rect_time.Height / 2, xy = badge_size / 3;
                            var rect_badge = new Rectangle(it.rect_icon.Right - badge_size + xy, it.rect_icon.Y - xy, badge_size, badge_size);
                            g.FillEllipse(it.BadgeBack ?? Color.Red, rect_badge);
                        }
                        else
                        {
                            var badgesize = g.MeasureString(it.Badge, font_time).Size(4, 2);
                            Rectangle rect_badge;
                            if (badgesize.Width > badgesize.Height)
                            {
                                rect_badge = new Rectangle(it.rect_icon.Right - badgesize.Width + badgesize.Width / 3, it.rect_icon.Y - badgesize.Height / 3, badgesize.Width, badgesize.Height);
                                using (var path = rect_badge.RoundPath(6 * Config.Dpi))
                                {
                                    g.Fill(it.BadgeBack ?? Color.Red, path);
                                }
                            }
                            else
                            {
                                int badge_size = badgesize.Width > badgesize.Height ? badgesize.Width : badgesize.Height, xy = badge_size / 3;
                                rect_badge = new Rectangle(it.rect_icon.Right - badge_size + xy, it.rect_icon.Y - xy, badge_size, badge_size);
                                g.FillEllipse(it.BadgeBack ?? Color.Red, rect_badge);
                            }
                            g.String(it.Badge, font_time, it.BadgeFore ?? Color.White, rect_badge, SFBage);
                        }
                    }
                }
            }
        }

        public MsgList() { ScrollBar = new ScrollBar(this); }

        protected override void Dispose(bool disposing)
        {
            ScrollBar.Dispose();
            base.Dispose(disposing);
        }

        #endregion

        #region 鼠标

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (ScrollBar.MouseDown(e.X, e.Y))
            {
                if (items == null || items.Count == 0) return;
                foreach (MsgItem it in Items)
                {
                    if (it.Visible && it.Contains(e.Location, 0, ScrollBar.Value, out _))
                    {
                        if (e.Button == MouseButtons.Left)
                        {
                            // 左键点击
                            it.Select = true;
                            OnItemSelected(it);
                            // 检查是否为双击
                            if (e.Clicks > 1)
                            {
                                OnItemClick(it, e);
                                OnItemDoubleClick(it, e);
                                return;
                            }
                        }
                        // 触发通用点击事件
                        OnItemClick(it, e);
                        return;
                    }
                }
            }
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            ScrollBar.MouseUp();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (ScrollBar.MouseMove(e.X, e.Y))
            {
                if (items == null || items.Count == 0) return;
                int count = 0, hand = 0;
                foreach (MsgItem it in Items)
                {
                    if (it.show)
                    {
                        if (it.Contains(e.Location, 0, ScrollBar.Value, out var change))
                        {
                            hand++;
                        }
                        if (change) count++;
                    }
                }
                SetCursor(hand > 0);
                if (count > 0) Invalidate();
            }
            else ILeave();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            ScrollBar.Leave();
            ILeave();
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            ScrollBar.Leave();
            ILeave();
        }

        void ILeave()
        {
            SetCursor(false);
            if (items == null || items.Count == 0) return;
            int count = 0;
            foreach (MsgItem it in Items)
            {
                if (it.Hover) count++;
                it.Hover = false;
            }
            if (count > 0) Invalidate();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            ScrollBar.MouseWheel(e);
            base.OnMouseWheel(e);
        }

        #endregion

        #region 事件

        /// <summary>
        /// 项目选中事件
        /// </summary>
        [Description("项目选中事件"), Category("行为")]
        public event ItemSelectedEventHandler? ItemSelected;

        /// <summary>
        /// 项目点击事件（包含鼠标信息）
        /// </summary>
        [Description("项目点击事件"), Category("行为")]
        public event ItemClickEventHandler? ItemClick;

        /// <summary>
        /// 项目双击事件
        /// </summary>
        [Description("项目双击事件"), Category("行为")]
        public event ItemClickEventHandler? ItemDoubleClick;

        protected virtual void OnItemSelected(MsgItem item) => ItemSelected?.Invoke(this, new MsgItemEventArgs(item));

        protected virtual void OnItemClick(MsgItem item, MouseEventArgs e) => ItemClick?.Invoke(this, new MsgItemClickEventArgs(item, e));

        protected virtual void OnItemDoubleClick(MsgItem item, MouseEventArgs e) => ItemDoubleClick?.Invoke(this, new MsgItemClickEventArgs(item, e));

        #endregion

        #region 布局

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
        internal void ChangeList()
        {
            var rect = ClientRectangle;
            if (items == null || items.Count == 0 || (rect.Width == 0 || rect.Height == 0)) return;
            int y = 0;
            Helper.GDI(g =>
            {
                var size = g.MeasureString(Config.NullText, Font).Height;
                int item_height = (int)Math.Ceiling(size * 3.856),
                    gap = (int)Math.Round(item_height * .212),
                    spilt = (int)Math.Round(gap * .478),
                    gap_name = (int)Math.Round(gap * .304),
                    gap_desc = (int)Math.Round(gap * .217),
                    name_height = (int)Math.Round(item_height * .185),
                    desc_height = (int)Math.Round(item_height * .157),
                    image_size = item_height - gap * 2;

                using (var font_time = new Font(Font.FontFamily, Font.Size * .82F))
                {
                    foreach (MsgItem it in items)
                    {
                        it.PARENT = this;
                        int time_width = 0;
                        if (!string.IsNullOrEmpty(it.Time)) time_width = g.MeasureString(it.Time, font_time).Width;

                        it.SetRect(new Rectangle(rect.X, rect.Y + y, rect.Width, item_height), time_width, gap, spilt, gap_name, gap_desc, image_size, name_height, desc_height);
                        if (it.Visible) y += item_height;
                    }
                }
            });
            ScrollBar.SetVrSize(y);
            ScrollBar.SizeChange(rect);
        }

        #endregion
    }

    public class MsgItemCollection : iCollection<MsgItem>
    {
        public MsgItemCollection(MsgList it)
        {
            BindData(it);
        }

        internal MsgItemCollection BindData(MsgList it)
        {
            action = render =>
            {
                if (render) it.ChangeList();
                it.Invalidate();
            };
            return this;
        }
    }

    public class MsgItem
    {
        public MsgItem() { }
        public MsgItem(string name)
        {
            _name = name;
        }
        public MsgItem(string name, Image? icon)
        {
            _name = name;
            _icon = icon;
        }

        /// <summary>
        /// ID
        /// </summary>
        [Description("ID"), Category("数据"), DefaultValue(null)]
        public string? ID { get; set; }

        Image? _icon;
        /// <summary>
        /// 图标
        /// </summary>
        [Description("图标"), Category("外观"), DefaultValue(null)]
        public Image? Icon
        {
            get => _icon;
            set
            {
                if (_icon == value) return;
                _icon = value;
                Invalidates();
            }
        }

        string? _name;
        /// <summary>
        /// 名称
        /// </summary>
        [Description("名称"), Category("外观"), DefaultValue(null)]
        public string? Name
        {
            get => _name;
            set
            {
                if (_name == value) return;
                _name = value;
                Invalidate();
            }
        }

        #region 文本

        string? _text;
        /// <summary>
        /// 文本
        /// </summary>
        [Description("文本"), Category("外观"), DefaultValue(null)]
        public string? Text
        {
            get => _text;
            set
            {
                if (_text == value) return;
                _text = value;
                Invalidate();
            }
        }

        StringFormat? _textFormat;
        /// <summary>
        /// 文本对齐方式
        /// </summary>
        [Description("文本对齐方式"), Category("外观"), DefaultValue(null)]
        public StringFormat? TextFormat
        {
            get => _textFormat;
            set
            {
                if (_textFormat == value) return;
                _textFormat = value;
                Invalidate();
            }
        }

        Font? _textFont;
        /// <summary>
        /// 文本字体
        /// </summary>
        [Description("文本字体"), Category("外观"), DefaultValue(null)]
        public Font? TextFont
        {
            get => _textFont;
            set
            {
                if (_textFont == value) return;
                _textFont = value;
                Invalidates();
            }
        }

        Color? _textColor;
        /// <summary>
        /// 消息文本颜色
        /// </summary>
        [Description("消息文本颜色"), Category("外观"), DefaultValue(null)]
        public Color? TextColor
        {
            get => _textColor;
            set
            {
                if (_textColor == value) return;
                _textColor = value;
                Invalidate();
            }
        }

        #endregion

        #region 徽标

        int count = 0;
        /// <summary>
        /// 消息数量
        /// </summary>
        [Description("消息数量"), Category("外观"), DefaultValue(0)]
        public int Count
        {
            get => count;
            set
            {
                count = value;
                if (value > 0)
                {
                    if (value > 99) Badge = "99+";
                    else if (value > 1) Badge = value.ToString();
                    else Badge = "";
                }
                else Badge = null;
            }
        }

        string? badge;
        /// <summary>
        /// 徽标
        /// </summary>
        [Description("徽标"), Category("外观"), DefaultValue(null)]
        public string? Badge
        {
            get => badge;
            set
            {
                if (badge == value) return;
                badge = value;
                Invalidates();
            }
        }

        Color? badgeBack;
        /// <summary>
        /// 徽标背景色
        /// </summary>
        [Description("徽标背景色"), Category("外观"), DefaultValue(null)]
        public Color? BadgeBack
        {
            get => badgeBack;
            set
            {
                if (badgeBack == value) return;
                badgeBack = value;
                Invalidate();
            }
        }

        Color? badgeFore;
        /// <summary>
        /// 徽标文本色
        /// </summary>
        [Description("徽标文本色"), Category("外观"), DefaultValue(null)]
        public Color? BadgeFore
        {
            get => badgeFore;
            set
            {
                if (badgeFore == value) return;
                badgeFore = value;
                Invalidate();
            }
        }

        #endregion

        #region 时间

        string? time;
        /// <summary>
        /// 时间
        /// </summary>
        [Description("时间"), Category("外观"), DefaultValue(null)]
        public string? Time
        {
            get => time;
            set
            {
                if (time == value) return;
                time = value;
                Invalidates();
            }
        }

        Color? _timeColor;
        /// <summary>
        /// 时间文本颜色
        /// </summary>
        [Description("时间文本颜色"), Category("外观"), DefaultValue(null)]
        public Color? TimeColor
        {
            get => _timeColor;
            set
            {
                if (_timeColor == value) return;
                _timeColor = value;
                Invalidate();
            }
        }

        Font? _timeFont;
        /// <summary>
        /// 时间字体
        /// </summary>
        [Description("时间字体"), Category("外观"), DefaultValue(null)]
        public Font? TimeFont
        {
            get => _timeFont;
            set
            {
                if (_timeFont == value) return;
                _timeFont = value;
                Invalidates();
            }
        }

        #endregion

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

        void Invalidate() => PARENT?.Invalidate();
        void Invalidates()
        {
            if (PARENT == null) return;
            PARENT.ChangeList();
            PARENT.Invalidate();
        }

        internal bool show { get; set; }
        internal bool Show { get; set; }

        /// <summary>
        /// 是否移动
        /// </summary>
        internal bool Hover = false;

        internal bool select = false;
        [Description("是否选中"), Category("外观"), DefaultValue(false)]
        public bool Select
        {
            get => select;
            set
            {
                if (select == value) return;
                select = value;
                if (value && PARENT != null)
                {
                    foreach (MsgItem it in PARENT.Items)
                    {
                        if (it != this) it.select = false;
                    }
                }
                Invalidate();
            }
        }

        internal MsgList? PARENT { get; set; }
        internal void SetRect(Rectangle _rect, int time_width, int gap, int spilt, int gap_name, int gap_desc, int image_size, int name_height, int desc_height)
        {
            rect = _rect;

            int text_width = _rect.Width - image_size - gap - spilt * 3;
            rect_icon = new Rectangle(_rect.X + gap, _rect.Y + gap, image_size, image_size);

            rect_name = new Rectangle(rect_icon.Right + spilt, rect_icon.Y + gap_name - gap_desc, text_width - time_width, name_height + gap_desc * 2);
            rect_time = new Rectangle(rect_name.Right, rect_name.Y, time_width, rect_name.Height);

            rect_text = new Rectangle(rect_name.X, rect_icon.Bottom - gap_desc - desc_height - gap_desc, text_width, desc_height + gap_desc * 2);

            Show = true;
        }

        internal Rectangle rect { get; set; }

        internal bool Contains(Point point, int x, int y, out bool change)
        {
            if (rect.Contains(new Point(point.X + x, point.Y + y)))
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
                if (!Hover) change = true;
                Hover = true;
            }
            else
            {
                if (Hover) change = true;
                Hover = false;
            }
            return change;
        }

        internal Rectangle rect_name { get; set; }
        internal Rectangle rect_time { get; set; }
        internal Rectangle rect_text { get; set; }
        internal Rectangle rect_icon { get; set; }
    }
}