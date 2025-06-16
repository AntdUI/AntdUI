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
    /// Timeline 时间轴
    /// </summary>
    /// <remarks>垂直展示的时间流信息。</remarks>
    [Description("Timeline 时间轴")]
    [ToolboxItem(true)]
    [DefaultProperty("Items")]
    [DefaultEvent("ItemClick")]
    public class Timeline : IControl
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

        [Description("描述字体"), Category("外观"), DefaultValue(null)]
        public Font? FontDescription { get; set; }

        /// <summary>
        /// 间距
        /// </summary>
        [Description("间距"), Category("外观"), DefaultValue(null)]
        public int? Gap { get; set; }

        TimelineItemCollection? items;
        /// <summary>
        /// 集合
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("集合"), Category("数据")]
        public TimelineItemCollection Items
        {
            get
            {
                items ??= new TimelineItemCollection(this);
                return items;
            }
            set => items = value.BindData(this);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            ChangeList();
            base.OnSizeChanged(e);
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

        internal void ChangeList()
        {
            var rect = ClientRectangle.DeflateRect(Padding);
            if (pauseLayout || items == null || items.Count == 0 || (rect.Width == 0 || rect.Height == 0)) return;
            int y = rect.Y;
            Helper.GDI(g =>
            {
                var size_def = g.MeasureString(Config.NullText, Font);
                int text_size = size_def.Height;
                float pen_w = text_size * 0.136F, split = pen_w * 0.666F, split_gap = split * 2F;
                int gap = (int)Math.Round(8 * Config.Dpi), gap_x = (int)Math.Round(text_size * 1.1D), gap_x_icon = (int)Math.Round(text_size * .846D), gap_y = (int)Math.Round((Gap * Config.Dpi) ?? (text_size * .91D)), ico_size = (int)Math.Round(text_size * .636D);

                int max_w = rect.Width - ico_size - gap_x_icon - (gap_x * 2);
                y += gap_x;
                var _splits = new List<RectangleF>(items.Count);
                int i = 0;
                var font_Description = FontDescription ?? Font;
                int gap2 = gap * 2;
                foreach (TimelineItem it in items)
                {
                    it.PARENT = this;
                    it.pen_w = pen_w;

                    if (it.Visible)
                    {
                        var size = g.MeasureText(it.Text, Font, max_w);
                        int ytmp = y, htmp = size.Height;
                        it.ico_rect = new Rectangle(rect.X + gap_x, y + (text_size - ico_size) / 2, ico_size, ico_size);
                        it.txt_rect = new Rectangle(it.ico_rect.Right + gap_x_icon, y, size.Width, size.Height);
                        if (!string.IsNullOrEmpty(it.Description))
                        {
                            var DescriptionSize = g.MeasureText(it.Description, font_Description, max_w);
                            it.description_rect = new Rectangle(it.txt_rect.X, it.txt_rect.Bottom + gap, DescriptionSize.Width, DescriptionSize.Height);
                            y += gap * 2 + DescriptionSize.Height;
                            htmp += DescriptionSize.Height + gap;
                        }
                        it.rect = new Rectangle(it.ico_rect.X - gap, ytmp - gap, it.txt_rect.Width + ico_size + gap_x_icon + gap2, htmp + gap2);
                        y += size.Height + gap_y;

                        if (i > 0)
                        {
                            var old = items[i - 1];
                            if (old != null) _splits.Add(new RectangleF(it.ico_rect.X + (ico_size - split) / 2F, old.ico_rect.Bottom + split_gap, split, it.ico_rect.Y - old.ico_rect.Bottom - (split_gap * 2F)));
                        }
                    }
                    i++;
                }
                splits = _splits.ToArray();
                y = y - gap_y + gap_x;
            });
            ScrollBar.SetVrSize(y);
            ScrollBar.SizeChange(rect);
        }

        RectangleF[] splits = new RectangleF[0];
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            ScrollBar.MouseWheel(e.Delta);
            base.OnMouseWheel(e);
        }

        #endregion

        #region 渲染

        public Timeline() { ScrollBar = new ScrollBar(this); }

        readonly StringFormat stringFormatLeft = Helper.SF(lr: StringAlignment.Near);

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
            g.TranslateTransform(0, -ScrollBar.Value);
            Color color_fore = fore ?? Colour.Text.Get("Timeline", ColorScheme);
            using (var brush_split = new SolidBrush(Colour.Split.Get("Timeline", ColorScheme)))
            {
                foreach (var it in splits) g.Fill(brush_split, it);
            }
            var font_Description = FontDescription ?? Font;
            using (var brush_fore = new SolidBrush(color_fore))
            using (var brush_fore2 = new SolidBrush(Colour.TextTertiary.Get("Timeline", ColorScheme)))
            using (var brush_dotback = new SolidBrush(Colour.BgBase.Get("Timeline", ColorScheme)))
            {
                foreach (var it in items)
                {
                    if (it.Visible)
                    {
                        g.DrawText(it.Text, Font, brush_fore, it.txt_rect, stringFormatLeft);
                        g.DrawText(it.Description, font_Description, brush_fore2, it.description_rect, stringFormatLeft);
                        if (PaintIcon(g, it, color_fore))
                        {
                            Color fill;
                            if (it.Fill.HasValue) fill = it.Fill.Value;
                            else
                            {
                                switch (it.Type)
                                {
                                    case TTypeMini.Error:
                                        fill = Colour.Error.Get("Timeline", ColorScheme);
                                        break;
                                    case TTypeMini.Success:
                                        fill = Colour.Success.Get("Timeline", ColorScheme);
                                        break;
                                    case TTypeMini.Info:
                                        fill = Colour.Info.Get("Timeline", ColorScheme);
                                        break;
                                    case TTypeMini.Warn:
                                        fill = Colour.Warning.Get("Timeline", ColorScheme);
                                        break;
                                    case TTypeMini.Default:
                                        fill = Colour.TextQuaternary.Get("Timeline", ColorScheme);
                                        break;
                                    case TTypeMini.Primary:
                                    default:
                                        fill = Colour.Primary.Get("Timeline", ColorScheme);
                                        break;
                                }
                            }

                            g.FillEllipse(brush_dotback, it.ico_rect);
                            g.DrawEllipse(fill, it.pen_w, it.ico_rect);
                        }
                    }
                }
            }
            g.ResetTransform();
            ScrollBar.Paint(g);
            this.PaintBadge(g);
            base.OnPaint(e);
        }

        bool PaintIcon(Canvas g, TimelineItem it, Color fore)
        {
            int count = 0;
            if (it.Icon != null) { g.Image(it.Icon, it.ico_rect); count++; }
            if (it.IconSvg != null && g.GetImgExtend(it.IconSvg, it.ico_rect, fore)) count++;
            return count == 0;
        }

        #endregion

        #region 鼠标

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (ScrollBar.MouseDown(e.Location)) OnTouchDown(e.X, e.Y);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            ScrollBar.MouseUp();
            OnTouchUp();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (ScrollBar.MouseMove(e.Location))
            {
                if (items == null || items.Count == 0 || ItemClick == null) return;
                if (OnTouchMove(e.X, e.Y))
                {
                    for (int i = 0; i < items.Count; i++)
                    {
                        var it = items[i];
                        if (it != null)
                        {
                            if (it.rect.Contains(e.X, e.Y + ScrollBar.Value))
                            {
                                SetCursor(true);
                                return;
                            }
                        }
                    }
                }
                SetCursor(false);
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            ScrollBar.Leave();
        }
        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            ScrollBar.Leave();
        }
        protected override bool OnTouchScrollX(int value) => ScrollBar.MouseWheelXCore(value);
        protected override bool OnTouchScrollY(int value) => ScrollBar.MouseWheelYCore(value);

        #endregion

        #region 事件

        /// <summary>
        /// 点击项时发生
        /// </summary>
        [Description("点击项时发生"), Category("行为")]
        public event TimelineEventHandler? ItemClick;

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (items == null || items.Count == 0 || ItemClick == null) return;
            for (int i = 0; i < items.Count; i++)
            {
                var it = items[i];
                if (it != null)
                {
                    if (it.rect.Contains(e.X, e.Y + ScrollBar.Value))
                    {
                        ItemClick(this, new TimelineItemEventArgs(it, e));
                        return;
                    }
                }
            }
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            ScrollBar.Dispose();
            base.Dispose(disposing);
        }
    }

    public class TimelineItemCollection : iCollection<TimelineItem>
    {
        public TimelineItemCollection(Timeline it)
        {
            BindData(it);
        }

        internal TimelineItemCollection BindData(Timeline it)
        {
            action = render =>
            {
                if (render) it.ChangeList();
                it.Invalidate();
            };
            return this;
        }
    }

    public class TimelineItem
    {
        public TimelineItem() { }
        public TimelineItem(string text)
        {
            Text = text;
        }
        public TimelineItem(string text, string description)
        {
            Text = text;
            Description = description;
        }
        public TimelineItem(string text, string description, Image? icon)
        {
            Text = text;
            Description = description;
            Icon = icon;
        }
        public TimelineItem(string text, Image? icon)
        {
            Text = text;
            Icon = icon;
        }

        /// <summary>
        /// ID
        /// </summary>
        [Description("ID"), Category("数据"), DefaultValue(null)]
        public string? ID { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [Description("图标"), Category("外观"), DefaultValue(null)]
        public Image? Icon { get; set; }

        /// <summary>
        /// 图标SVG
        /// </summary>
        [Description("图标SVG"), Category("外观"), DefaultValue(null)]
        public string? IconSvg { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Description("名称"), Category("数据"), DefaultValue(null)]
        public string? Name { get; set; }

        string? description;
        /// <summary>
        /// 描述，可选
        /// </summary>
        [Description("描述，可选"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(System.ComponentModel.Design.MultilineStringEditor), typeof(UITypeEditor))]
        public string? Description
        {
            get => Localization.GetLangI(LocalizationDescription, description, new string?[] { "{id}", ID });
            set
            {
                if (description == value) return;
                description = value;
                Invalidates();
            }
        }

        [Description("详情描述"), Category("国际化"), DefaultValue(null)]
        public string? LocalizationDescription { get; set; }


        string? text;
        /// <summary>
        /// 文本
        /// </summary>
        [Description("文本"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(System.ComponentModel.Design.MultilineStringEditor), typeof(UITypeEditor))]
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

        [Description("颜色类型"), Category("外观"), DefaultValue(TTypeMini.Primary)]
        public TTypeMini Type { get; set; } = TTypeMini.Primary;

        [Description("填充颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? Fill { get; set; }

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

        void Invalidates()
        {
            if (PARENT == null) return;
            PARENT.ChangeList();
            PARENT.Invalidate();
        }

        internal Timeline? PARENT { get; set; }

        internal float pen_w { get; set; } = 3F;
        internal Rectangle rect { get; set; }
        internal Rectangle txt_rect { get; set; }
        internal Rectangle description_rect { get; set; }
        internal Rectangle ico_rect { get; set; }

        public override string? ToString() => Text;
    }
}