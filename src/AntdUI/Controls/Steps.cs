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
    /// Steps 步骤条
    /// </summary>
    /// <remarks>引导用户按照流程完成任务的导航条。</remarks>
    [Description("Steps 步骤条")]
    [ToolboxItem(true)]
    [DefaultProperty("Current")]
    [DefaultEvent("ItemClick")]
    public class Steps : IControl
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
                OnPropertyChanged(nameof(Current));
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
                OnPropertyChanged(nameof(Status));
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
                OnPropertyChanged(nameof(Vertical));
            }
        }

        /// <summary>
        /// 间距
        /// </summary>
        [Description("间距"), Category("外观"), DefaultValue(8)]
        public int Gap { get; set; } = 8;

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

        internal void ChangeList()
        {
            var rect = ClientRectangle.DeflateRect(Padding);
            if (pauseLayout || items == null || items.Count == 0) return;
            if (rect.Width == 0 || rect.Height == 0) return;
            Helper.GDI(g =>
            {
                int gap = (int)(Gap * Config.Dpi), split = (int)Config.Dpi;
                var _splits = new List<RectangleF>(items.Count);
                using (var font_description = new Font(Font.FontFamily, Font.Size * 0.875F))
                {
                    int gap2 = gap * 2;
                    int i = 0, ri = 0, count = 0;
                    foreach (var it in items)
                    {
                        it.PARENT = this;
                        if (it.Visible) count++;
                    }
                    if (vertical)
                    {
                        int t_height_one = rect.Height / count, iod = 0;
                        foreach (var it in items)
                        {
                            if (it.Visible)
                            {
                                it.TitleSize = g.MeasureText(it.Title, Font);
                                int ico_size = (int)(it.TitleSize.Height * 1.6F);
                                it.pen_w = it.TitleSize.Height * 0.136F;
                                int width_one = it.TitleSize.Width + gap, height_one = ico_size, width_ex = 0;

                                if (it.showSub)
                                {
                                    it.SubTitleSize = g.MeasureText(it.SubTitle, Font);
                                    height_one += it.SubTitleSize.Height;
                                }
                                if (it.showDescription)
                                {
                                    it.DescriptionSize = g.MeasureText(it.Description, font_description);
                                    width_ex = it.DescriptionSize.Width;
                                }

                                int centery = rect.Y + t_height_one * i + t_height_one / 2;//居中X
                                it.title_rect = new Rectangle(rect.X + gap + ico_size, centery - height_one / 2, it.TitleSize.Width, height_one);
                                int read_y = it.title_rect.Y - gap - ico_size;

                                it.ico_rect = new Rectangle(rect.X, it.title_rect.Y + (it.title_rect.Height - ico_size) / 2, ico_size, ico_size);

                                int tmp_max_width = it.title_rect.Width, tmp_max_height = it.ico_rect.Height, tmp_max_wr = it.title_rect.Right;

                                if (it.showSub)
                                {
                                    it.subtitle_rect = new Rectangle(it.title_rect.X + it.TitleSize.Width, it.title_rect.Y, it.SubTitleSize.Width, height_one);
                                    tmp_max_width = it.subtitle_rect.Width + it.title_rect.Width;
                                    tmp_max_wr = it.subtitle_rect.Right;
                                }
                                if (it.showDescription)
                                {
                                    it.description_rect = new Rectangle(it.title_rect.X, it.title_rect.Y + (height_one - it.TitleSize.Height) / 2 + it.TitleSize.Height + gap / 2, it.DescriptionSize.Width, it.DescriptionSize.Height);
                                    if (it.description_rect.Width > tmp_max_width)
                                    {
                                        tmp_max_width = it.description_rect.Width;
                                        tmp_max_wr = it.description_rect.Right;
                                    }
                                    tmp_max_height += it.DescriptionSize.Height;
                                }
                                it.rect = new Rectangle(it.ico_rect.X - gap, it.ico_rect.Y - gap, tmp_max_wr - it.ico_rect.X + gap2, tmp_max_height + gap2);

                                if (ri > 0)
                                {
                                    var old = items[iod];
                                    if (old != null) _splits.Add(new RectangleF(it.ico_rect.X + (ico_size - split) / 2F, old.ico_rect.Bottom + gap, split, it.ico_rect.Y - old.ico_rect.Bottom - gap2));
                                }
                                i++;
                                iod = ri;
                            }
                            ri++;
                        }
                    }
                    else
                    {
                        //横向
                        int read_width = MaxHeight(g, font_description, gap, out var maxHeight);
                        int sp = (rect.Width - read_width) / count, spline = sp - gap;
                        int has_x = rect.X + sp / 2;
                        count -= 1;
                        foreach (var it in items)
                        {
                            if (it.Visible)
                            {
                                int icon_size = it.IconSize ?? (int)(it.TitleSize.Height * 1.6F);
                                int y = rect.Y + (rect.Height - maxHeight) / 2;
                                it.ico_rect = new Rectangle(has_x, y + (it.TitleSize.Height - icon_size) / 2, icon_size, icon_size);
                                it.title_rect = new Rectangle(it.ico_rect.Right + gap, y, it.TitleSize.Width, it.TitleSize.Height);

                                int tmp_max_height = it.ico_rect.Height;
                                if (it.showSub) it.subtitle_rect = new Rectangle(it.title_rect.X + it.TitleSize.Width, it.title_rect.Y, it.SubTitleSize.Width, it.title_rect.Height);

                                if (it.showDescription)
                                {
                                    it.description_rect = new Rectangle(it.title_rect.X, it.title_rect.Bottom + gap / 2, it.DescriptionSize.Width, it.DescriptionSize.Height);
                                    tmp_max_height += it.DescriptionSize.Height;
                                }

                                it.rect = new Rectangle(it.ico_rect.X - gap, it.ico_rect.Y - gap, it.ReadWidth + gap2, tmp_max_height + gap2);
                                if (spline > 0 && i < count) _splits.Add(new RectangleF(it.rect.Right - gap, it.ico_rect.Y + (it.ico_rect.Height - split) / 2F, spline, split));
                                has_x += it.ReadWidth + sp;
                                i++;
                            }
                            ri++;
                        }
                    }
                }
                splits = _splits.ToArray();
            });
            return;
        }

        int MaxHeight(Canvas g, Font font_description, int gap, out int height)
        {
            int w = 0, temp_t = 0, temp = 0;
            foreach (var it in Items)
            {
                if (it.Visible)
                {
                    #region 计算

                    it.TitleSize = g.MeasureText(it.Title, Font);
                    if (it.showSub) it.SubTitleSize = g.MeasureText(it.SubTitle, Font);
                    if (it.showDescription) it.DescriptionSize = g.MeasureText(it.Description, font_description);

                    int icon_size = it.IconSize ?? (int)(it.TitleSize.Height * 1.6F);
                    int width_top = it.TitleSize.Width + (it.showSub ? it.SubTitleSize.Width : 0), width_buttom = (it.showDescription ? it.DescriptionSize.Width : 0);

                    it.ReadWidth = icon_size + gap + (width_top > width_buttom ? width_top : width_buttom);

                    #endregion

                    it.pen_w = it.TitleSize.Height * 0.136F;
                    w += it.ReadWidth;
                    if (temp_t == 0) temp_t = it.TitleSize.Height;
                    if (temp == 0 && it.showDescription) temp = it.DescriptionSize.Height + gap / 2;
                }
            }
            height = temp_t + temp;
            return w;
        }

        RectangleF[] splits = new RectangleF[0];

        #endregion

        #region 渲染

        readonly StringFormat stringLeft = Helper.SF(lr: StringAlignment.Near);
        readonly StringFormat stringCenter = Helper.SF_NoWrap();

        protected override void OnDraw(DrawEventArgs e)
        {
            if (items == null || items.Count == 0)
            {
                base.OnDraw(e);
                return;
            }
            var g = e.Canvas;
            Color color_fore = fore ?? Colour.Text.Get("Steps", ColorScheme);
            using (var brush_fore = new SolidBrush(color_fore))
            using (var brush_primarybg = new SolidBrush(Colour.PrimaryBg.Get("Steps", ColorScheme)))
            using (var brush_primary = new SolidBrush(Colour.Primary.Get("Steps", ColorScheme)))
            using (var brush_primary_fore = new SolidBrush(Colour.PrimaryColor.Get("Steps", ColorScheme)))
            using (var brush_dotback = new SolidBrush(Colour.BgBase.Get("Steps", ColorScheme)))
            using (var brush_fore2 = new SolidBrush(Colour.TextTertiary.Get("Steps", ColorScheme)))
            using (var brush_fore3 = new SolidBrush(Colour.TextSecondary.Get("Steps", ColorScheme)))
            using (var brush_bg2 = new SolidBrush(Colour.FillSecondary.Get("Steps", ColorScheme)))
            using (var font_description = new Font(Font.FontFamily, Font.Size * 0.875F))
            {
                using (var brush_split = new SolidBrush(Colour.Split.Get("Steps", ColorScheme)))
                {
                    for (int sp = 0; sp < splits.Length; sp++)
                    {
                        if (sp < current) g.Fill(brush_primary, splits[sp]);
                        else g.Fill(brush_split, splits[sp]);
                    }
                }
                int i = 0;
                foreach (var it in items)
                {
                    if (it.Visible)
                    {
                        Color ccolor;
                        if (i == current)
                        {
                            switch (status)
                            {
                                case TStepState.Finish:
                                    g.DrawText(it.Title, Font, brush_fore, it.title_rect, stringLeft);
                                    g.DrawText(it.SubTitle, Font, brush_fore2, it.subtitle_rect, stringLeft);
                                    g.DrawText(it.Description, font_description, brush_fore2, it.description_rect, stringLeft);
                                    ccolor = brush_primary.Color;
                                    break;
                                case TStepState.Wait:
                                    g.DrawText(it.Title, Font, brush_fore2, it.title_rect, stringLeft);
                                    g.DrawText(it.SubTitle, Font, brush_fore2, it.subtitle_rect, stringLeft);
                                    g.DrawText(it.Description, font_description, brush_fore2, it.description_rect, stringLeft);
                                    ccolor = brush_fore2.Color;
                                    break;
                                case TStepState.Error:
                                    using (var brush_error = new SolidBrush(Colour.Error.Get("Steps", ColorScheme)))
                                    {
                                        g.DrawText(it.Title, Font, brush_error, it.title_rect, stringLeft);
                                        g.DrawText(it.SubTitle, Font, brush_fore2, it.subtitle_rect, stringLeft);
                                        g.DrawText(it.Description, font_description, brush_error, it.description_rect, stringLeft);
                                        ccolor = brush_error.Color;
                                    }
                                    break;
                                case TStepState.Process:
                                default:

                                    g.DrawText(it.Title, Font, brush_fore, it.title_rect, stringLeft);
                                    g.DrawText(it.SubTitle, Font, brush_fore2, it.subtitle_rect, stringLeft);
                                    g.DrawText(it.Description, font_description, brush_fore, it.description_rect, stringLeft);

                                    ccolor = brush_primary.Color;
                                    break;
                            }
                        }
                        else if (i < current)
                        {
                            //过
                            g.DrawText(it.Title, Font, brush_fore, it.title_rect, stringLeft);
                            g.DrawText(it.SubTitle, Font, brush_fore2, it.subtitle_rect, stringLeft);
                            g.DrawText(it.Description, font_description, brush_fore2, it.description_rect, stringLeft);
                            ccolor = brush_primary.Color;
                        }
                        else
                        {
                            //未
                            g.DrawText(it.Title, Font, brush_fore2, it.title_rect, stringLeft);
                            g.DrawText(it.SubTitle, Font, brush_fore2, it.subtitle_rect, stringLeft);
                            g.DrawText(it.Description, font_description, brush_fore2, it.description_rect, stringLeft);
                            ccolor = brush_fore2.Color;
                        }

                        if (PaintIcon(g, it, ccolor))
                        {
                            if (i == current)
                            {
                                switch (status)
                                {
                                    case TStepState.Finish:
                                        g.PaintIconCore(it.ico_rect, SvgDb.IcoSuccess, brush_primary.Color, brush_primarybg.Color);
                                        break;
                                    case TStepState.Wait:
                                        g.FillEllipse(brush_bg2, it.ico_rect);
                                        g.DrawText((i + 1).ToString(), font_description, brush_fore3, it.ico_rect, stringCenter);
                                        break;
                                    case TStepState.Error:
                                        g.PaintIconCore(it.ico_rect, SvgDb.IcoError, Colour.ErrorColor.Get("Steps", ColorScheme), Colour.Error.Get("Steps", ColorScheme));
                                        break;
                                    case TStepState.Process:
                                    default:
                                        g.FillEllipse(brush_primary, it.ico_rect);
                                        g.DrawText((i + 1).ToString(), font_description, brush_primary_fore, it.ico_rect, stringCenter);
                                        break;
                                }
                            }
                            else if (i < current) g.PaintIconCore(it.ico_rect, SvgDb.IcoSuccess, brush_primary.Color, brush_primarybg.Color);
                            else
                            {
                                g.FillEllipse(brush_bg2, it.ico_rect);
                                g.DrawText((i + 1).ToString(), font_description, brush_fore3, it.ico_rect, stringCenter);
                            }
                        }
                        i++;
                    }
                }
            }
            base.OnDraw(e);
        }

        bool PaintIcon(Canvas g, StepsItem it, Color fore)
        {
            int count = 0;
            if (it.Icon != null) { g.Image(it.Icon, it.ico_rect); count++; }
            if (it.IconSvg != null && g.GetImgExtend(it.IconSvg, it.ico_rect, fore)) count++;
            return count == 0;
        }

        #endregion

        #region 事件

        /// <summary>
        /// 点击项时发生
        /// </summary>
        [Description("点击项时发生"), Category("行为")]
        public event StepsItemEventHandler? ItemClick;

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (items == null || items.Count == 0 || ItemClick == null) return;
            foreach (var it in items)
            {
                if (it.Visible && it.rect.Contains(e.Location))
                {
                    ItemClick(this, new StepsItemEventArgs(it, e));
                    return;
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (items == null || items.Count == 0 || ItemClick == null) return;
            foreach (var it in items)
            {
                if (it.Visible && it.rect.Contains(e.Location))
                {
                    SetCursor(true);
                    return;
                }
            }
            SetCursor(false);
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
        /// ID
        /// </summary>
        [Description("ID"), Category("数据"), DefaultValue(null)]
        public string? ID { get; set; }

        Image? icon;
        /// <summary>
        /// 图标，可选
        /// </summary>
        [Description("图标，可选"), Category("外观"), DefaultValue(null)]
        public Image? Icon
        {
            get => icon;
            set
            {
                if (icon == value) return;
                icon = value;
                PARENT?.Invalidate();
            }
        }

        string? iconSvg;
        /// <summary>
        /// 图标SVG，可选
        /// </summary>
        [Description("图标SVG，可选"), Category("外观"), DefaultValue(null)]
        public string? IconSvg
        {
            get => iconSvg;
            set
            {
                if (iconSvg == value) return;
                iconSvg = value;
                PARENT?.Invalidate();
            }
        }

        int? iconsize;
        /// <summary>
        /// 图标的大小，可选
        /// </summary>
        [Description("图标的大小，可选"), Category("外观"), DefaultValue(null)]
        public int? IconSize
        {
            get => iconsize;
            set
            {
                if (iconsize == value) return;
                iconsize = value;
                Invalidate();
            }
        }

        internal int ReadWidth { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Description("名称"), Category("数据"), DefaultValue(null)]
        public string? Name { get; set; }

        string title = "Title";
        /// <summary>
        /// 标题
        /// </summary>
        [Description("标题"), Category("外观"), DefaultValue("Title")]
        public string Title
        {
            get => Localization.GetLangIN(LocalizationTitle, title, new string?[] { "{id}", ID });
            set
            {
                if (title == value) return;
                title = value;
                Invalidate();
            }
        }

        [Description("标题"), Category("国际化"), DefaultValue(null)]
        public string? LocalizationTitle { get; set; }

        internal Size TitleSize { get; set; }

        string? subTitle;
        internal bool showSub = false;
        /// <summary>
        /// 子标题
        /// </summary>
        [Description("子标题"), Category("外观"), DefaultValue(null)]
        public string? SubTitle
        {
            get => Localization.GetLangI(LocalizationSubTitle, subTitle, new string?[] { "{id}", ID });
            set
            {
                if (string.IsNullOrEmpty(value)) value = null;
                if (subTitle == value) return;
                subTitle = value;
                showSub = subTitle != null;
                Invalidate();
            }
        }

        [Description("子标题"), Category("国际化"), DefaultValue(null)]
        public string? LocalizationSubTitle { get; set; }
        internal Size SubTitleSize { get; set; }

        string? description;
        internal bool showDescription = false;
        /// <summary>
        /// 详情描述，可选
        /// </summary>
        [Description("详情描述，可选"), Category("外观"), DefaultValue(null)]
        public string? Description
        {
            get => Localization.GetLangI(LocalizationDescription, description, new string?[] { "{id}", ID });
            set
            {
                if (string.IsNullOrEmpty(value)) value = null;
                if (description == value) return;
                description = value;
                showDescription = description != null;
                Invalidate();
            }
        }

        [Description("详情描述"), Category("国际化"), DefaultValue(null)]
        public string? LocalizationDescription { get; set; }

        internal Size DescriptionSize { get; set; }

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
                Invalidate();
            }
        }

        /// <summary>
        /// 用户定义数据
        /// </summary>
        [Description("用户定义数据"), Category("数据"), DefaultValue(null)]
        public object? Tag { get; set; }

        void Invalidate()
        {
            if (PARENT == null) return;
            PARENT.ChangeList();
            PARENT.Invalidate();
        }

        internal Steps? PARENT { get; set; }

        internal float pen_w { get; set; } = 3F;
        internal Rectangle rect { get; set; }
        internal Rectangle title_rect { get; set; }
        internal Rectangle subtitle_rect { get; set; }
        internal Rectangle description_rect { get; set; }
        internal Rectangle ico_rect { get; set; }

        public override string ToString() => Title + " " + SubTitle;
    }
}