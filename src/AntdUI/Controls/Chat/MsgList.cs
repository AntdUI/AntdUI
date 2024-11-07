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

        protected override void OnPaint(PaintEventArgs e)
        {
            if (items == null || items.Count == 0) return;
            var rect = ClientRectangle;
            if (rect.Width == 0 || rect.Height == 0) return;

            var g = e.Graphics.High();
            int sy = ScrollBar.Value;
            g.TranslateTransform(0, -sy);
            using (var font_text = new Font(Font.FontFamily, Font.Size * 0.9F))
            using (var font_time = new Font(Font.FontFamily, Font.Size * 0.82F))
            {
                foreach (MsgItem it in items) PaintItem(g, it, rect, sy, font_text, font_time);
            }

            g.ResetTransform();
            ScrollBar.Paint(g);
            base.OnPaint(e);
        }

        StringFormat SFBage = Helper.SF();
        StringFormat SFL = Helper.SF_ALL(lr: StringAlignment.Near);
        StringFormat SFR = Helper.SF_ALL(lr: StringAlignment.Far);
        void PaintItem(Canvas g, MsgItem it, Rectangle rect, float sy, Font font_text, Font font_time)
        {
            it.show = it.Show && it.Visible && it.rect.Y > sy - rect.Height && it.rect.Bottom < ScrollBar.Value + ScrollBar.ReadSize + it.rect.Height;
            if (it.show)
            {
                if (it.Select)
                {
                    using (var brush = new SolidBrush(Color.FromArgb(0, 153, 255)))
                    {
                        g.Fill(brush, it.rect);
                    }
                    using (var brush = new SolidBrush(Color.White))
                    {

                        try
                        {
                            g.String(it.Name, Font, brush, it.rect_name, SFL);
                            g.String(it.Text, font_text, brush, it.rect_text, SFL);

                            g.String(it.Time, font_time, brush, it.rect_time, SFR);
                        }
                        catch { }
                    }

                }
                else
                {
                    if (it.Hover)
                    {
                        using (var brush = new SolidBrush(Style.Db.FillTertiary))
                        {
                            g.Fill(brush, it.rect);
                        }
                    }
                    using (var brush = new SolidBrush(ForeColor))
                    {
                        try
                        {
                            g.String(it.Name, Font, brush, it.rect_name, SFL);
                            g.String(it.Text, font_text, brush, it.rect_text, SFL);

                            g.String(it.Time, font_time, brush, it.rect_time, SFR);
                        }
                        catch { }
                    }
                }
                if (it.Icon != null)
                {
                    g.Image(it.rect_icon, it.Icon, TFit.Cover, 0, true);
                    if (it.Count > 0)
                    {
                        if (it.Count > 99)
                        {
                            var badgesize = g.MeasureString("99+", font_time);
                            var rect_badge = new Rectangle(it.rect_icon.Right - badgesize.Width + badgesize.Width / 3, it.rect_icon.Y - badgesize.Height / 3, badgesize.Width, badgesize.Height);
                            using (var path = rect_badge.RoundPath(6 * Config.Dpi))
                            {
                                g.Fill(Brushes.Red, path);
                            }
                            g.String("99+", font_time, Brushes.White, rect_badge, SFBage);
                        }
                        else if (it.Count > 1)
                        {
                            var badgesize = g.MeasureString(it.Count.ToString(), font_time);
                            int badge_size = badgesize.Width > badgesize.Height ? badgesize.Width : badgesize.Height, xy = badge_size / 3;
                            var rect_badge = new Rectangle(it.rect_icon.Right - badge_size + xy, it.rect_icon.Y - xy, badge_size, badge_size);
                            g.FillEllipse(Brushes.Red, rect_badge);
                            g.String(it.Count.ToString(), font_time, Brushes.White, rect_badge, SFBage);
                        }
                        else
                        {
                            int badge_size = it.rect_time.Height / 2, xy = badge_size / 3;
                            var rect_badge = new Rectangle(it.rect_icon.Right - badge_size + xy, it.rect_icon.Y - xy, badge_size, badge_size);
                            g.FillEllipse(Brushes.Red, rect_badge);
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
            if (ScrollBar.MouseDown(e.Location))
            {
                if (items == null || items.Count == 0) return;
                foreach (MsgItem it in Items)
                {
                    if (it.Visible && it.Contains(e.Location, 0, ScrollBar.Value, out _))
                    {
                        it.Select = true;
                        // 触发ItemSelected事件
                        OnItemSelected(it);
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
            if (ScrollBar.MouseMove(e.Location))
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
            ScrollBar.MouseWheel(e.Delta);
            base.OnMouseWheel(e);
        }

        #endregion

        #region 事件

        public event ItemSelectedEventHandler? ItemSelected;
        protected virtual void OnItemSelected(MsgItem selectedItem)
        {
            ItemSelected?.Invoke(this, new MsgItemEventArgs(selectedItem));
        }

        #endregion

        #region 布局

        protected override void OnFontChanged(EventArgs e)
        {
            var rect = ChangeList();
            ScrollBar.SizeChange(rect);
            base.OnFontChanged(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            var rect = ChangeList();
            ScrollBar.SizeChange(rect);
            base.OnSizeChanged(e);
        }
        internal Rectangle ChangeList()
        {
            var rect = ClientRectangle;
            if (items == null || items.Count == 0) return rect;
            if (rect.Width == 0 || rect.Height == 0) return rect;

            int y = 0;
            Helper.GDI(g =>
            {
                var size = g.MeasureString(Config.NullText, Font).Height;
                int item_height = (int)Math.Ceiling(size * 3.856),
                    gap = (int)Math.Round(item_height * 0.212),
                    spilt = (int)Math.Round(gap * 0.478),
                    gap_name = (int)Math.Round(gap * 0.304),
                    gap_desc = (int)Math.Round(gap * 0.217),
                    name_height = (int)Math.Round(item_height * 0.185),
                    desc_height = (int)Math.Round(item_height * 0.157),
                    image_size = item_height - gap * 2;

                using (var font_time = new Font(Font.FontFamily, Font.Size * 0.82F))
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
            return rect;
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

    public class MsgItem : NotifyProperty
    {
        public MsgItem() { }
        public MsgItem(string name)
        {
            _name = name;
        }
        public MsgItem(string name, Bitmap? icon)
        {
            _name = name;
            _icon = icon;
        }

        Image? _icon = null;
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
                OnPropertyChanged("Icon");
            }
        }

        string _name;
        /// <summary>
        /// 名称
        /// </summary>
        [Description("名称"), Category("外观")]
        public string Name
        {
            get => _name;
            set
            {
                if (_name == value) return;
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        string? _text = null;
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
                OnPropertyChanged("Text");
            }
        }

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
                if (count == value) return;
                count = value;
                OnPropertyChanged("Count");
            }
        }

        string? time = null;
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

            int text_width = _rect.Width - image_size - gap - spilt * 2;
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
