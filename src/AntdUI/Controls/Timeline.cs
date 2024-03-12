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
    /// Timeline 时间轴
    /// </summary>
    /// <remarks>垂直展示的时间流信息。</remarks>
    [Description("Timeline 时间轴")]
    [ToolboxItem(true)]
    [DefaultProperty("Items")]
    public class Timeline : IControl
    {
        #region 属性

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

        [Description("描述字体"), Category("外观"), DefaultValue(null)]
        public Font? FontDescription { get; set; }

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
            var rect = ClientRectangle.DeflateRect(Padding);
            if (pauseLayout || items == null || items.Count == 0) return rect;
            if (rect.Width == 0 || rect.Height == 0) return rect;

            float y = rect.Y;
            Helper.GDI(g =>
            {
                var size_def = g.MeasureString(Config.NullText, Font);
                float text_size = size_def.Height, pen_w = text_size * 0.136F, split = pen_w * 0.666F, split_gap = split * 2F, gap = 8F * Config.Dpi;
                int gap_x = (int)Math.Round(text_size * 1.1D), gap_x_icon = (int)Math.Round(text_size * 0.846D), gap_y = (int)Math.Round(text_size * 0.91D),
                    ico_size = (int)Math.Round(text_size * 0.636D);

                int max_w = rect.Width - ico_size - gap_x_icon - (gap_x * 2);
                y += gap_x;
                var _splits = new List<RectangleF>(Items.Count);
                int i = 0;
                var font_Description = FontDescription == null ? Font : FontDescription;
                foreach (TimelineItem it in Items)
                {
                    it.PARENT = this;
                    it.pen_w = pen_w;

                    if (it.Visible)
                    {
                        var size = g.MeasureString(it.Text, Font, max_w);

                        it.ico_rect = new RectangleF(rect.X + gap_x, y + (text_size - ico_size) / 2F, ico_size, ico_size);
                        it.txt_rect = new RectangleF(it.ico_rect.Right + gap_x_icon, y, size.Width, size.Height);
                        if (!string.IsNullOrEmpty(it.Description))
                        {
                            var DescriptionSize = g.MeasureString(it.Description, font_Description, max_w);
                            it.description_rect = new RectangleF(it.txt_rect.X, it.txt_rect.Bottom + gap, DescriptionSize.Width, DescriptionSize.Height);
                            y += gap * 2 + DescriptionSize.Height;
                        }
                        y += size.Height + gap_y;

                        if (i > 0)
                        {
                            var old = Items[i - 1];
                            if (old != null)
                            {
                                _splits.Add(new RectangleF(it.ico_rect.X + (ico_size - split) / 2F, old.ico_rect.Bottom + split_gap, split, it.ico_rect.Y - old.ico_rect.Bottom - (split_gap * 2F)));
                            }
                        }
                    }

                    i++;
                }
                splits = _splits.ToArray();
                y = y - gap_y + gap_x;
            });
            scrollY.SetVrSize(y, rect.Height);
            return rect;
        }

        RectangleF[] splits = new RectangleF[0];

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            scrollY.MouseWheel(e.Delta);
            base.OnMouseWheel(e);
        }

        #endregion

        #region 渲染

        ScrollY scrollY;
        public Timeline() { scrollY = new ScrollY(this); }

        readonly StringFormat stringFormatLeft = Helper.SF(lr: StringAlignment.Near);

        protected override void OnPaint(PaintEventArgs e)
        {
            if (items == null || items.Count == 0) return;
            var rect = ClientRectangle;
            if (rect.Width == 0 || rect.Height == 0) return;
            var g = e.Graphics.High();
            g.TranslateTransform(0, -scrollY.Value);
            Color color_fore = fore.HasValue ? fore.Value : Style.Db.Text;
            using (var brush_split = new SolidBrush(Style.Db.Split))
            {
                foreach (var it in splits)
                {
                    g.FillRectangle(brush_split, it);
                }
            }
            var font_Description = FontDescription == null ? Font : FontDescription;
            using (var brush_fore = new SolidBrush(color_fore))
            using (var brush_fore2 = new SolidBrush(Style.Db.TextTertiary))
            using (var brush_dotback = new SolidBrush(Style.Db.BgBase))
            {
                foreach (TimelineItem it in Items)
                {
                    if (it.Visible)
                    {
                        g.DrawString(it.Text, Font, brush_fore, it.txt_rect, stringFormatLeft);
                        g.DrawString(it.Description, font_Description, brush_fore2, it.description_rect, stringFormatLeft);
                        if (it.Icon != null) g.DrawImage(it.Icon, it.ico_rect);
                        else
                        {
                            Color fill;
                            if (it.Fill.HasValue) fill = it.Fill.Value;
                            else
                            {
                                switch (it.Type)
                                {
                                    case TTypeMini.Error:
                                        fill = Style.Db.Error;
                                        break;
                                    case TTypeMini.Success:
                                        fill = Style.Db.Success;
                                        break;
                                    case TTypeMini.Info:
                                        fill = Style.Db.Info;
                                        break;
                                    case TTypeMini.Warn:
                                        fill = Style.Db.Warning;
                                        break;
                                    case TTypeMini.Default:
                                        fill = Style.Db.TextQuaternary;
                                        break;
                                    case TTypeMini.Primary:
                                    default:
                                        fill = Style.Db.Primary;
                                        break;
                                }
                            }

                            g.FillEllipse(brush_dotback, it.ico_rect);
                            using (var pen = new Pen(fill, it.pen_w))
                            {
                                g.DrawEllipse(pen, it.ico_rect);
                            }
                        }
                    }
                }
            }
            g.ResetTransform();
            scrollY.Paint(g);
            this.PaintBadge(g);
            base.OnPaint(e);
        }

        #endregion

        #region 鼠标

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            scrollY.MouseDown(e.Location);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            scrollY.MouseUp(e.Location);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            scrollY.MouseMove(e.Location);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            scrollY.Leave();
        }
        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            scrollY.Leave();
        }

        #endregion
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
        /// 图标
        /// </summary>
        [Description("图标"), Category("外观"), DefaultValue(null)]
        public Image? Icon { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Description("名称"), Category("数据"), DefaultValue(null)]
        public string? Name { get; set; }

        /// <summary>
        /// 描述，可选
        /// </summary>
        [Description("描述，可选"), Category("外观"), DefaultValue(null)]
        public string? Description { get; set; }

        /// <summary>
        /// 文本
        /// </summary>
        [Description("文本"), Category("外观"), DefaultValue(null)]
        public string? Text { get; set; }

        [Description("颜色类型"), Category("外观"), DefaultValue(TTypeMini.Primary)]
        public TTypeMini Type { get; set; } = TTypeMini.Primary;

        [Description("填充颜色"), Category("外观"), DefaultValue(null)]
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
            if (PARENT != null)
            {
                PARENT.ChangeList();
                PARENT.Invalidate();
            }
        }

        internal Timeline? PARENT { get; set; }

        internal float pen_w { get; set; } = 3F;
        internal RectangleF txt_rect { get; set; }
        internal RectangleF description_rect { get; set; }
        internal RectangleF ico_rect { get; set; }
    }
}