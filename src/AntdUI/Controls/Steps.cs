// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
// THE AntdUI PROJECT IS AN WINFORM LIBRARY LICENSED UNDER THE GPL-3.0 License.
// LICENSED UNDER THE GPL License, VERSION 3.0 (THE "License")
// YOU MAY NOT USE THIS FILE EXCEPT IN COMPLIANCE WITH THE License.
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
    /// Steps 步骤条
    /// </summary>
    /// <remarks>引导用户按照流程完成任务的导航条。</remarks>
    [Description("Steps 步骤条")]
    [ToolboxItem(true)]
    [DefaultProperty("Current")]
    public class Steps : IControl
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

        int current = 0;
        /// <summary>
        /// 指定当前步骤，从 0 开始记数。在子 Step 元素中，可以通过 status 属性覆盖状态
        /// </summary>
        [Description("指定当前步骤"), Category("外观"), DefaultValue(0)]
        public int Current
        {
            get => current;
            set
            {
                if (current == value) return;
                current = value;
                Invalidate();
            }
        }

        TStepState status = TStepState.Process;
        /// <summary>
        /// 指定当前步骤的状态
        /// </summary>
        [Description("指定当前步骤的状态"), Category("外观"), DefaultValue(TStepState.Process)]
        public TStepState Status
        {
            get => status;
            set
            {
                if (status == value) return;
                status = value;
                Invalidate();
            }
        }

        bool vertical = false;
        /// <summary>
        /// 垂直方向
        /// </summary>
        [Description("垂直方向"), Category("外观"), DefaultValue(false)]
        public bool Vertical
        {
            get => vertical;
            set
            {
                if (vertical == value) return;
                vertical = value;
                ChangeList();
                Invalidate();
            }
        }

        StepsItemCollection? items;
        /// <summary>
        /// 集合
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("集合"), Category("数据")]
        public StepsItemCollection Items
        {
            get
            {
                items ??= new StepsItemCollection(this);
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

        internal void ChangeList()
        {
            var rect = ClientRectangle.DeflateRect(Padding);
            if (pauseLayout || items == null || items.Count == 0) return;
            if (rect.Width == 0 || rect.Height == 0) return;
            using (var bmp = new Bitmap(1, 1))
            {
                splits.Clear();
                using (var g = Graphics.FromImage(bmp))
                {
                    float gap = 8F * Config.Dpi, split = 1F * Config.Dpi;
                    var _splits = new List<RectangleF>();
                    using (var font_description = new Font(Font.FontFamily, Font.Size * 0.875F))
                    {
                        if (vertical)
                        {
                            var t_height_one = rect.Height / Items.Count;
                            int i = 0;
                            float split_gap = 8F * Config.Dpi;
                            foreach (StepsItem it in Items)
                            {
                                it.PARENT = this;
                                it.TitleSize = g.MeasureString(it.Title, Font);
                                float ico_size = it.TitleSize.Height * 1.6F, pen_w = it.TitleSize.Height * 0.136F;
                                it.pen_w = pen_w;
                                float width_one = it.TitleSize.Width + gap, height_one = ico_size, width_ex = 0;

                                bool showSub = false, showDescription = false;
                                if (!string.IsNullOrEmpty(it.SubTitle))
                                {
                                    it.SubTitleSize = g.MeasureString(it.SubTitle, Font);
                                    showSub = true;
                                    height_one += it.SubTitleSize.Height;
                                }
                                if (!string.IsNullOrEmpty(it.Description))
                                {
                                    showDescription = true;
                                    it.DescriptionSize = g.MeasureString(it.Description, font_description);
                                    width_ex = it.DescriptionSize.Width;
                                }

                                float centery = rect.Y + t_height_one * i + t_height_one / 2;//居中X
                                it.title_rect = new RectangleF(rect.X + gap + ico_size, centery - height_one / 2, it.TitleSize.Width, height_one);
                                float read_y = it.title_rect.Y - gap - ico_size;

                                it.ico_rect = new RectangleF(rect.X, it.title_rect.Y + (it.title_rect.Height - ico_size) / 2F, ico_size, ico_size);
                                if (showSub) it.subtitle_rect = new RectangleF(it.title_rect.X + it.TitleSize.Width, it.title_rect.Y, it.SubTitleSize.Width, height_one);
                                if (showDescription) it.description_rect = new RectangleF(it.title_rect.X, it.title_rect.Y + (height_one - it.TitleSize.Height) / 2 + it.TitleSize.Height + gap / 2, it.DescriptionSize.Width, it.DescriptionSize.Height);

                                if (i > 0)
                                {
                                    var old = Items[i - 1];
                                    if (old != null) _splits.Add(new RectangleF(it.ico_rect.X + (ico_size - split) / 2F, old.ico_rect.Bottom + split_gap, split, it.ico_rect.Y - old.ico_rect.Bottom - (split_gap * 2F)));
                                }
                                i++;
                            }
                        }
                        else
                        {
                            //横向
                            var t_width_one = rect.Width / Items.Count;
                            int i = 0;
                            float has_x = rect.X, maxHeight = MaxHeight(g, font_description, gap);
                            foreach (StepsItem it in Items)
                            {
                                it.PARENT = this;
                                it.TitleSize = g.MeasureString(it.Title, Font);
                                float icon_size = it.IconSize.HasValue ? it.IconSize.Value : it.TitleSize.Height * 1.6F, pen_w = it.TitleSize.Height * 0.136F;
                                it.pen_w = pen_w;
                                float width_one = it.TitleSize.Width + gap, width_sub = width_one, width_ex = 0;
                                bool showSub = false, showDescription = false;
                                if (!string.IsNullOrEmpty(it.SubTitle))
                                {
                                    it.SubTitleSize = g.MeasureString(it.SubTitle, Font);
                                    showSub = true;
                                    width_sub += it.SubTitleSize.Width;
                                }
                                if (!string.IsNullOrEmpty(it.Description))
                                {
                                    showDescription = true;
                                    it.DescriptionSize = g.MeasureString(it.Description, font_description);
                                    width_ex = it.DescriptionSize.Width;
                                }
                                if (width_ex > width_sub) width_one = width_ex;
                                else width_one = width_sub;

                                float centerx = rect.X + t_width_one * i + t_width_one / 2;//居中X
                                it.title_rect = new RectangleF(centerx - width_one / 2, rect.Y + (rect.Height - maxHeight) / 2, it.TitleSize.Width, it.TitleSize.Height);
                                float read_x = it.title_rect.X - gap - icon_size;
                                it.ico_rect = new RectangleF(read_x, it.title_rect.Y + (it.title_rect.Height - icon_size) / 2F, icon_size, icon_size);
                                if (showSub) it.subtitle_rect = new RectangleF(it.title_rect.X + it.TitleSize.Width, it.title_rect.Y, it.SubTitleSize.Width, it.title_rect.Height);
                                if (showDescription) it.description_rect = new RectangleF(it.title_rect.X, it.title_rect.Bottom + gap / 2, it.DescriptionSize.Width, it.DescriptionSize.Height);

                                if (i > 0)
                                {
                                    var old = Items[i - 1];
                                    if (old != null) _splits.Add(new RectangleF(has_x, it.ico_rect.Y + (it.ico_rect.Height - split) / 2F, read_x - gap - has_x, split));
                                }
                                has_x = it.title_rect.X + width_sub;
                                i++;
                            }
                        }
                    }
                    splits = _splits;
                }
            }
            return;
        }

        float MaxHeight(Graphics g, Font font_description, float gap)
        {
            float temp_t = 0, temp = 0;
            foreach (StepsItem it in Items)
            {
                if (temp_t == 0)
                {
                    it.TitleSize = g.MeasureString(it.Title, Font);
                    temp_t = it.TitleSize.Height;
                }
                if (temp == 0 && !string.IsNullOrEmpty(it.Description))
                {
                    it.DescriptionSize = g.MeasureString(it.Description, font_description);
                    temp = it.DescriptionSize.Height + gap / 2;
                }
                if (temp_t > 0 && temp > 0) return temp_t + temp;
            }
            return temp_t + temp;
        }

        List<RectangleF> splits = new List<RectangleF>();

        #endregion

        #region 渲染


        readonly StringFormat stringLeft = new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Near };
        readonly StringFormat stringCenter = new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center, FormatFlags = StringFormatFlags.NoWrap };

        protected override void OnPaint(PaintEventArgs e)
        {
            if (items == null || items.Count == 0) return;
            var rect = ClientRectangle;
            if (rect.Width == 0 || rect.Height == 0) return;
            var g = e.Graphics.High();
            Color color_fore = fore.HasValue ? fore.Value : Style.Db.Text;
            using (var brush_fore = new SolidBrush(color_fore))
            using (var brush_primarybg = new SolidBrush(Style.Db.PrimaryBg))
            using (var brush_primary = new SolidBrush(Style.Db.Primary))
            using (var brush_primary_fore = new SolidBrush(Style.Db.PrimaryColor))
            using (var brush_dotback = new SolidBrush(Style.Db.BgBase))
            using (var brush_fore2 = new SolidBrush(Style.Db.TextTertiary))
            using (var brush_fore3 = new SolidBrush(Style.Db.TextSecondary))
            using (var brush_bg2 = new SolidBrush(Style.Db.FillSecondary))
            using (var font_description = new Font(Font.FontFamily, Font.Size * 0.875F))
            {
                using (var brush_split = new SolidBrush(Style.Db.Split))
                {
                    for (int sp = 0; sp < splits.Count; sp++)
                    {
                        if (sp < current) g.FillRectangle(brush_primary, splits[sp]);
                        else g.FillRectangle(brush_split, splits[sp]);
                    }
                }
                int i = 0;
                foreach (StepsItem it in Items)
                {
                    if (it.Visible)
                    {
                        if (i == current)
                        {
                            switch (status)
                            {
                                case TStepState.Finish:
                                    g.DrawString(it.Title, Font, brush_fore, it.title_rect, stringLeft);
                                    g.DrawString(it.SubTitle, Font, brush_fore2, it.subtitle_rect, stringLeft);
                                    g.DrawString(it.Description, font_description, brush_fore2, it.description_rect, stringLeft);
                                    break;
                                case TStepState.Wait:
                                    g.DrawString(it.Title, Font, brush_fore2, it.title_rect, stringLeft);
                                    g.DrawString(it.SubTitle, Font, brush_fore2, it.subtitle_rect, stringLeft);
                                    g.DrawString(it.Description, font_description, brush_fore2, it.description_rect, stringLeft);
                                    break;
                                case TStepState.Error:
                                    using (var brush_error = new SolidBrush(Style.Db.Error))
                                    {
                                        g.DrawString(it.Title, Font, brush_error, it.title_rect, stringLeft);
                                        g.DrawString(it.SubTitle, Font, brush_fore2, it.subtitle_rect, stringLeft);
                                        g.DrawString(it.Description, font_description, brush_error, it.description_rect, stringLeft);
                                    }
                                    break;
                                case TStepState.Process:
                                default:

                                    g.DrawString(it.Title, Font, brush_fore, it.title_rect, stringLeft);
                                    g.DrawString(it.SubTitle, Font, brush_fore2, it.subtitle_rect, stringLeft);
                                    g.DrawString(it.Description, font_description, brush_fore, it.description_rect, stringLeft);

                                    break;
                            }
                        }
                        else if (i < current)
                        {
                            //过
                            g.DrawString(it.Title, Font, brush_fore, it.title_rect, stringLeft);
                            g.DrawString(it.SubTitle, Font, brush_fore2, it.subtitle_rect, stringLeft);
                            g.DrawString(it.Description, font_description, brush_fore2, it.description_rect, stringLeft);
                        }
                        else
                        {
                            //未
                            g.DrawString(it.Title, Font, brush_fore2, it.title_rect, stringLeft);
                            g.DrawString(it.SubTitle, Font, brush_fore2, it.subtitle_rect, stringLeft);
                            g.DrawString(it.Description, font_description, brush_fore2, it.description_rect, stringLeft);
                        }

                        if (it.Icon != null) g.DrawImage(it.Icon, it.ico_rect);
                        else
                        {
                            if (i == current)
                            {
                                switch (status)
                                {
                                    case TStepState.Finish:
                                        g.FillEllipse(brush_primarybg, it.ico_rect);
                                        g.PaintIconComplete(it.ico_rect, brush_primary);
                                        break;
                                    case TStepState.Wait:
                                        g.FillEllipse(brush_bg2, it.ico_rect);
                                        g.DrawString((i + 1).ToString(), font_description, brush_fore3, it.ico_rect, stringCenter);
                                        break;
                                    case TStepState.Error:
                                        using (var brush_error = new SolidBrush(Style.Db.Error))
                                        {
                                            g.FillEllipse(brush_error, it.ico_rect);
                                            g.PaintIconError(it.ico_rect, Style.Db.ErrorColor, 0.34F, 0.05F);
                                        }
                                        break;
                                    case TStepState.Process:
                                    default:
                                        g.FillEllipse(brush_primary, it.ico_rect);
                                        g.DrawString((i + 1).ToString(), font_description, brush_primary_fore, it.ico_rect, stringCenter);
                                        break;
                                }
                            }
                            else if (i < current)
                            {
                                g.FillEllipse(brush_primarybg, it.ico_rect);
                                g.PaintIconComplete(it.ico_rect, brush_primary);
                            }
                            else
                            {
                                g.FillEllipse(brush_bg2, it.ico_rect);
                                g.DrawString((i + 1).ToString(), font_description, brush_fore3, it.ico_rect, stringCenter);
                            }
                        }
                    }
                    i++;
                }
            }
            this.PaintBadge(g);
            base.OnPaint(e);
        }

        #endregion
    }

    public class StepsItemCollection : iCollection<StepsItem>
    {
        public StepsItemCollection(Steps it)
        {
            BindData(it);
        }

        internal StepsItemCollection BindData(Steps it)
        {
            action = render =>
            {
                if (render) it.ChangeList();
                it.Invalidate();
            };
            return this;
        }
    }

    public class StepsItem
    {
        public StepsItem() { }
        public StepsItem(string title)
        {
            Title = title;
        }
        public StepsItem(string title, string subTitle)
        {
            Title = title;
            SubTitle = subTitle;
        }
        public StepsItem(string title, string subTitle, string description)
        {
            Title = title;
            SubTitle = subTitle;
            Description = description;
        }

        /// <summary>
        /// 步骤图标的类型，可选
        /// </summary>
        [Description("步骤图标的类型，可选"), Category("外观"), DefaultValue(null)]
        public Image? Icon { get; set; }

        /// <summary>
        /// 步骤图标的大小，可选
        /// </summary>
        [Description("步骤图标的大小，可选"), Category("外观"), DefaultValue(null)]
        public int? IconSize { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Description("名称"), Category("数据"), DefaultValue(null)]
        public string? Name { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [Description("标题"), Category("外观"), DefaultValue("Title")]
        public string Title { get; set; } = "Title";
        internal SizeF TitleSize { get; set; }

        /// <summary>
        /// 子标题
        /// </summary>
        [Description("子标题"), Category("外观"), DefaultValue(null)]
        public string? SubTitle { get; set; }
        internal SizeF SubTitleSize { get; set; }

        /// <summary>
        /// 步骤的详情描述，可选
        /// </summary>
        [Description("步骤的详情描述，可选"), Category("外观"), DefaultValue(null)]
        public string? Description { get; set; }
        internal SizeF DescriptionSize { get; set; }

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

        internal Steps? PARENT { get; set; }

        internal float pen_w { get; set; } = 3F;
        internal RectangleF title_rect { get; set; }
        internal RectangleF subtitle_rect { get; set; }
        internal RectangleF description_rect { get; set; }
        internal RectangleF ico_rect { get; set; }
    }
}