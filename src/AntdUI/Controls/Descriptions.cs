// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Globalization;

namespace AntdUI
{
    /// <summary>
    /// Descriptions 描述列表
    /// </summary>
    /// <remarks>成组展示多个只读字段，常见于详情页的信息展示。</remarks>
    [Description("Descriptions 描述列表")]
    [ToolboxItem(true)]
    [DefaultProperty("Items")]
    [Obsolete("预览版：未经充分测试 Preview version: Not fully tested", false)]
    public class Descriptions : IControl, IEventListener
    {
        #region 属性

        Color? back;
        /// <summary>
        /// 背景颜色
        /// </summary>
        [Description("背景颜色"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? Back
        {
            get => back;
            set
            {
                if (back == value) return;
                back = value;
                Invalidate();
                OnPropertyChanged(nameof(Back));
            }
        }

        string? backExtend;
        /// <summary>
        /// 背景渐变色
        /// </summary>
        [Description("背景渐变色"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null)]
        public string? BackExtend
        {
            get => backExtend;
            set
            {
                if (backExtend == value) return;
                backExtend = value;
                Invalidate();
                OnPropertyChanged(nameof(BackExtend));
            }
        }

        Color? fore;
        /// <summary>
        /// 内容文字颜色
        /// </summary>
        [Description("内容文字颜色"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null)]
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

        Color? labelFore;
        /// <summary>
        /// 标签文字颜色
        /// </summary>
        [Description("标签文字颜色"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? LabelFore
        {
            get => labelFore;
            set
            {
                if (labelFore == value) return;
                labelFore = value;
                Invalidate();
                OnPropertyChanged(nameof(LabelFore));
            }
        }

        Color? labelBack;
        /// <summary>
        /// 标签背景颜色（带边框模式生效）
        /// </summary>
        [Description("标签背景颜色（带边框模式生效）"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? LabelBack
        {
            get => labelBack;
            set
            {
                if (labelBack == value) return;
                labelBack = value;
                Invalidate();
                OnPropertyChanged(nameof(LabelBack));
            }
        }

        #region 标题

        string? title;
        /// <summary>
        /// 标题
        /// </summary>
        [Description("标题"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null), Localizable(true)]
        public string? Title
        {
            get => this.GetLangI(LocalizationTitle, title);
            set
            {
                if (title == value) return;
                title = value;
                ChangeList(true);
                OnPropertyChanged(nameof(Title));
            }
        }

        /// <summary>
        /// 标题
        /// </summary>
        [Description("标题"), Category("国际化"), DefaultValue(null)]
        public string? LocalizationTitle { get; set; }

        Color? titleFore;
        /// <summary>
        /// 标题文字颜色
        /// </summary>
        [Description("标题文字颜色"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? TitleFore
        {
            get => titleFore;
            set
            {
                if (titleFore == value) return;
                titleFore = value;
                Invalidate();
                OnPropertyChanged(nameof(TitleFore));
            }
        }

        float titleFontRatio = 1.15F;
        /// <summary>
        /// 标题字体比例
        /// </summary>
        [Description("标题字体比例"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(1.15F)]
        public float TitleFontRatio
        {
            get => titleFontRatio;
            set
            {
                if (titleFontRatio == value) return;
                if (value <= 0) return;
                titleFontRatio = value;
                ChangeList(true);
                OnPropertyChanged(nameof(TitleFontRatio));
            }
        }

        #endregion

        #region 数据

        DescriptionsItemCollection? items;
        /// <summary>
        /// 集合
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("集合"), Category(nameof(CategoryAttribute.Data))]
        public DescriptionsItemCollection Items
        {
            get
            {
                items ??= new DescriptionsItemCollection(this);
                return items;
            }
            set => items = value.BindData(this);
        }

        #endregion

        #region 布局

        int column = 3;
        /// <summary>
        /// 一行的 DescriptionsItem 数量
        /// </summary>
        [Description("一行的 DescriptionsItem 数量"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(3)]
        public int Column
        {
            get => column;
            set
            {
                if (column == value) return;
                if (value < 1) value = 1;
                column = value;
                ChangeList(true);
                OnPropertyChanged(nameof(Column));
            }
        }

        TDescriptionsColumnWidth columnWidth = TDescriptionsColumnWidth.Auto;
        /// <summary>
        /// 列宽模式
        /// </summary>
        [Description("列宽模式"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(TDescriptionsColumnWidth.Auto)]
        public TDescriptionsColumnWidth ColumnWidth
        {
            get => columnWidth;
            set
            {
                if (columnWidth == value) return;
                columnWidth = value;
                ChangeList(true);
                OnPropertyChanged(nameof(ColumnWidth));
            }
        }

        float maxColumnRatio = .6F;
        /// <summary>
        /// 单列最大占比（列宽模式为内容时生效）
        /// </summary>
        [Description("单列最大占比（列宽模式为内容时生效）"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(.6F)]
        public float MaxColumnRatio
        {
            get => maxColumnRatio;
            set
            {
                if (maxColumnRatio == value) return;
                if (value < 0.1F) value = 0.1F;
                else if (value > 1F) value = 1F;
                maxColumnRatio = value;
                if (columnWidth == TDescriptionsColumnWidth.Content) ChangeList(true);
                OnPropertyChanged(nameof(MaxColumnRatio));
            }
        }

        int labelWidth = 0;
        /// <summary>
        /// 标签宽度（0=自动宽度）
        /// </summary>
        [Description("标签宽度（0=自动宽度）"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(0)]
        public int LabelWidth
        {
            get => labelWidth;
            set
            {
                if (labelWidth == value) return;
                if (value < 0) value = 0;
                labelWidth = value;
                ChangeList(true);
                OnPropertyChanged(nameof(LabelWidth));
            }
        }

        float labelRatio = .65F;
        /// <summary>
        /// 标签最大占比（自动宽度时生效）
        /// </summary>
        [Description("标签最大占比（自动宽度时生效）"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(.65F)]
        public float LabelRatio
        {
            get => labelRatio;
            set
            {
                if (labelRatio == value) return;
                if (value < 0) value = 0;
                else if (value > 1F) value = 1F;
                labelRatio = value;
                if (labelWidth == 0) ChangeList(true);
                OnPropertyChanged(nameof(LabelRatio));
            }
        }

        bool pauseLayout = false;
        /// <summary>
        /// 暂停布局
        /// </summary>
        [Description("暂停布局"), Category(nameof(CategoryAttribute.Behavior)), DefaultValue(false)]
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

        #endregion

        #region 样式

        TDescriptionsLayout itemLayout = TDescriptionsLayout.Horizontal;
        /// <summary>
        /// 描述布局
        /// </summary>
        [Description("描述布局"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(TDescriptionsLayout.Horizontal)]
        public TDescriptionsLayout ItemLayout
        {
            get => itemLayout;
            set
            {
                if (itemLayout == value) return;
                itemLayout = value;
                ChangeList(true);
                OnPropertyChanged(nameof(ItemLayout));
            }
        }

        TSizeMode sizeMode = TSizeMode.Default;
        /// <summary>
        /// 设置列表的大小
        /// </summary>
        [Description("设置列表的大小"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(TSizeMode.Default)]
        public TSizeMode SizeMode
        {
            get => sizeMode;
            set
            {
                if (sizeMode == value) return;
                sizeMode = value;
                ChangeList(true);
                OnPropertyChanged(nameof(SizeMode));
            }
        }

        bool bordered = false;
        /// <summary>
        /// 是否展示边框
        /// </summary>
        [Description("是否展示边框"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(false)]
        public bool Bordered
        {
            get => bordered;
            set
            {
                if (bordered == value) return;
                bordered = value;
                ChangeList(true);
                OnPropertyChanged(nameof(Bordered));
            }
        }

        bool colon = true;
        /// <summary>
        /// 是否显示标签后面的冒号
        /// </summary>
        [Description("是否显示标签后面的冒号"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(true)]
        public bool Colon
        {
            get => colon;
            set
            {
                if (colon == value) return;
                colon = value;
                ChangeList(true);
                OnPropertyChanged(nameof(Colon));
            }
        }

        bool textWrap = true;
        /// <summary>
        /// 内容超出是否换行（否=显示省略号）
        /// </summary>
        [Description("内容超出是否换行（否=显示省略号）"), Category(nameof(CategoryAttribute.Behavior)), DefaultValue(true)]
        public bool TextWrap
        {
            get => textWrap;
            set
            {
                if (textWrap == value) return;
                textWrap = value;
                ChangeList(true);
                OnPropertyChanged(nameof(TextWrap));
            }
        }

        ContentAlignment labelAlign = ContentAlignment.MiddleLeft;
        FormatFlags labelAl = FormatFlags.Left | FormatFlags.VerticalCenter;
        /// <summary>
        /// 标签文本位置
        /// </summary>
        [Description("标签文本位置"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(ContentAlignment.MiddleLeft)]
        public ContentAlignment LabelAlign
        {
            get => labelAlign;
            set
            {
                if (labelAlign == value) return;
                labelAlign = value;
                labelAl = value.SetAlignment(FormatFlags.Left | FormatFlags.VerticalCenter);
                Invalidate();
                OnPropertyChanged(nameof(LabelAlign));
            }
        }

        ContentAlignment textAlign = ContentAlignment.MiddleLeft;
        FormatFlags textAl = FormatFlags.Left | FormatFlags.VerticalCenter;
        /// <summary>
        /// 内容文本位置
        /// </summary>
        [Description("内容文本位置"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(ContentAlignment.MiddleLeft)]
        public ContentAlignment TextAlign
        {
            get => textAlign;
            set
            {
                if (textAlign == value) return;
                textAlign = value;
                textAl = value.SetAlignment(FormatFlags.Left | FormatFlags.VerticalCenter);
                Invalidate();
                OnPropertyChanged(nameof(TextAlign));
            }
        }

        #endregion

        #region 徽标

        float badgeRatio = .4F;
        /// <summary>
        /// 徽标点比例
        /// </summary>
        [Description("徽标点比例"), Category("徽标"), DefaultValue(.4F)]
        public float BadgeRatio
        {
            get => badgeRatio;
            set
            {
                if (badgeRatio == value) return;
                if (value <= 0) return;
                badgeRatio = value;
                Invalidate();
                OnPropertyChanged(nameof(BadgeRatio));
            }
        }

        int badgeGap = 6;
        /// <summary>
        /// 徽标与文本间距
        /// </summary>
        [Description("徽标与文本间距"), Category("徽标"), DefaultValue(6)]
        public int BadgeGap
        {
            get => badgeGap;
            set
            {
                if (badgeGap == value) return;
                badgeGap = value;
                Invalidate();
                OnPropertyChanged(nameof(BadgeGap));
            }
        }

        #endregion

        #region 边框

        int radius = 6;
        /// <summary>
        /// 圆角
        /// </summary>
        [Description("圆角"), Category("边框"), DefaultValue(6)]
        public int Radius
        {
            get => radius;
            set
            {
                if (radius == value) return;
                if (value < 0) value = 0;
                radius = value;
                Invalidate();
                OnPropertyChanged(nameof(Radius));
            }
        }

        float borderWidth = 1F;
        /// <summary>
        /// 边框宽度
        /// </summary>
        [Description("边框宽度"), Category("边框"), DefaultValue(1F)]
        public float BorderWidth
        {
            get => borderWidth;
            set
            {
                if (borderWidth == value) return;
                if (value < 0) value = 0;
                borderWidth = value;
                ChangeList(true);
                OnPropertyChanged(nameof(BorderWidth));
            }
        }

        Color? borderColor;
        /// <summary>
        /// 边框颜色
        /// </summary>
        [Description("边框颜色"), Category("边框"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? BorderColor
        {
            get => borderColor;
            set
            {
                if (borderColor == value) return;
                borderColor = value;
                if (borderWidth > 0) Invalidate();
                OnPropertyChanged(nameof(BorderColor));
            }
        }

        DashStyle borderStyle = DashStyle.Solid;
        /// <summary>
        /// 边框样式
        /// </summary>
        [Description("边框样式"), Category("边框"), DefaultValue(DashStyle.Solid)]
        public DashStyle BorderStyle
        {
            get => borderStyle;
            set
            {
                if (borderStyle == value) return;
                borderStyle = value;
                if (borderWidth > 0) Invalidate();
                OnPropertyChanged(nameof(BorderStyle));
            }
        }

        #endregion

        #endregion

        #region 布局

        DescriptionLayout layout = new DescriptionLayout();

        bool CanLayout(out bool canPrint)
        {
            if (IsHandleCreated)
            {
                var rect = ClientRectangle;
                if (pauseLayout || rect.Width == 0 || rect.Height == 0)
                {
                    canPrint = false;
                    return false;
                }
                canPrint = true;
                return true;
            }
            canPrint = false;
            return false;
        }

        internal void ChangeList(bool print = false)
        {
            BeforeAutoSize();
            if (CanLayout(out var canPrint))
            {
                var rect = ClientRectangle.PaddingRect(Padding);
                if (rect.Width > 0)
                {
                    var tmp = new DescriptionLayout();
                    this.GDI(g => LayoutCore(g, rect, tmp));
                    layout = tmp;
                }
            }
            if (print && canPrint) Invalidate();
        }

        /// <summary>
        /// 重新布局
        /// </summary>
        public void Relayout() => ChangeList(true);

        /// <summary>
        /// 计算布局
        /// </summary>
        /// <param name="g">画布</param>
        /// <param name="rect">可用区域</param>
        /// <param name="result">布局结果</param>
        /// <returns>实际占用尺寸</returns>
        Size LayoutCore(Canvas g, Rectangle rect, DescriptionLayout result)
        {
            int column_max = column < 1 ? 1 : column;
            var list = new List<DescriptionsItem>(items == null ? 0 : items.Count);
            if (items != null)
            {
                foreach (var it in items)
                {
                    if (!it.Visible) continue;
                    it.PARENT = this;
                    list.Add(it);
                }
            }
            if (list.Count == 0 && string.IsNullOrEmpty(Title)) return Size.Empty;

            bool vertical = itemLayout == TDescriptionsLayout.Vertical;
            bool unlimited = rect.Width >= 100000;
            float dpi = Dpi, sfactor = SizeFactor();

            int unit = (int)Math.Round(16F * dpi * sfactor);
            int pad_row = bordered ? 0 : unit;
            int pad_h = bordered ? (int)Math.Round(14F * dpi * sfactor) : (int)Math.Round(4F * dpi * sfactor);
            int pad_v = bordered ? (int)Math.Round(10F * dpi * sfactor) : 0;
            int gap = (int)Math.Round((vertical ? 6F : 8F) * dpi * sfactor);
            int line_h = g.MeasureString(Config.NullText, Font).Height;
            int badge_reserve = line_h;

            #region 标题

            int use_y = rect.Y;
            string? title_str = Title;
            if (!string.IsNullOrEmpty(title_str))
            {
                using (var font_title = new Font(Font.FontFamily, Font.Size * titleFontRatio, Font.Style | FontStyle.Bold))
                {
                    int tw = rect.Width > 0 && rect.Width < 100000 ? rect.Width - pad_h * 2 : 9999;
                    if (tw < 10) tw = 10;
                    var size_title = g.MeasureText(title_str, font_title, tw);
                    result.RectTitle = new Rectangle(rect.X + pad_h, use_y, size_title.Width, size_title.Height);
                    use_y += size_title.Height + (int)Math.Round(unit * 0.8F);
                }
            }

            #endregion

            #region 预处理

            foreach (var it in list)
            {
                it.Prepare(this, g, Font, vertical, colon && !vertical, badge_reserve);
            }

            #endregion

            #region 分行（处理合并列）

            var rows = new List<DescriptionRow>(list.Count);
            {
                DescriptionRow cur = new DescriptionRow();
                int used = 0;
                foreach (var it in list)
                {
                    int span = it.Span;
                    if (span <= 0)
                    {
                        //铺满当前行剩余部分
                        span = column_max - used;
                        if (span <= 0) span = column_max;
                    }
                    if (span > column_max) span = column_max;
                    if (used > 0 && used + span > column_max)
                    {
                        rows.Add(cur);
                        cur = new DescriptionRow();
                        used = 0;
                        if (it.Span <= 0) span = column_max;
                        if (span > column_max) span = column_max;
                    }
                    if (used + span > column_max) span = column_max - used;
                    it.ColStart = used;
                    it.ColSpan = span < 1 ? 1 : span;
                    cur.Items.Add(it);
                    used += it.ColSpan;
                    if (used >= column_max)
                    {
                        rows.Add(cur);
                        cur = new DescriptionRow();
                        used = 0;
                    }
                }
                if (cur.Items.Count > 0) rows.Add(cur);
            }
            if (rows.Count == 0) return Size.Empty;

            #endregion

            #region 列宽

            var unit_w = new float[column_max];
            bool mode_content = unlimited || columnWidth == TDescriptionsColumnWidth.Content || (columnWidth == TDescriptionsColumnWidth.Auto && bordered);
            if (mode_content)
            {
                foreach (var row in rows)
                {
                    foreach (var it in row.Items)
                    {
                        float need = pad_h * 2;
                        if (vertical) need += Math.Max(it.LabelW, it.ContentW);
                        else
                        {
                            need += it.LabelW;
                            if (it.ContentW > 0) need += gap + it.ContentW;
                        }
                        float per = need / it.ColSpan;
                        for (int k = 0; k < it.ColSpan; k++)
                        {
                            int index = it.ColStart + k;
                            if (index < 0 || index >= column_max) continue;
                            if (unit_w[index] < per) unit_w[index] = per;
                        }
                    }
                }
                if (!unlimited)
                {
                    float avail = rect.Width;
                    float share = avail / column_max, min = share * .5F, max = avail * maxColumnRatio, total = 0;
                    if (max < min) max = min;
                    for (int i = 0; i < column_max; i++)
                    {
                        float val = unit_w[i];
                        if (val <= 0) val = share;
                        if (val < min) val = min;
                        else if (val > max) val = max;
                        unit_w[i] = val;
                        total += val;
                    }
                    if (total <= 0)
                    {
                        for (int i = 0; i < column_max; i++) unit_w[i] = share;
                    }
                    else
                    {
                        float scale = avail / total;
                        for (int i = 0; i < column_max; i++) unit_w[i] *= scale;
                    }
                }
            }
            else
            {
                float share = rect.Width * 1F / column_max;
                for (int i = 0; i < column_max; i++) unit_w[i] = share;
            }
            var widths = new int[column_max];
            {
                int sum = 0;
                for (int i = 0; i < column_max; i++)
                {
                    int wi = (int)Math.Round(unit_w[i]);
                    if (wi < 1) wi = 1;
                    widths[i] = wi;
                    sum += wi;
                }
                if (!unlimited && sum != rect.Width && rect.Width > 0)
                {
                    //误差补给最后一列，保证铺满
                    int diff = rect.Width - sum;
                    if (widths[column_max - 1] + diff > 0) widths[column_max - 1] += diff;
                }
            }

            #endregion

            #region 标签宽度

            var label_ws = new int[column_max];
            if (!vertical)
            {
                if (labelWidth > 0)
                {
                    int lw_v = (int)Math.Round(labelWidth * dpi) + pad_h * 2;
                    for (int i = 0; i < column_max; i++) label_ws[i] = lw_v;
                }
                else
                {
                    foreach (var row in rows)
                    {
                        foreach (var it in row.Items)
                        {
                            int index = it.ColStart;
                            if (index < 0 || index >= column_max) continue;
                            int lw_it = it.LabelW + pad_h * 2;
                            if (label_ws[index] < lw_it) label_ws[index] = lw_it;
                        }
                    }
                }
                //统一限制，保证同一列的标签宽度一致（跨行对齐）
                int min_content = (int)Math.Round(48F * dpi * sfactor) + pad_h * 2;
                for (int i = 0; i < column_max; i++)
                {
                    int limit = (int)Math.Round(widths[i] * labelRatio), limit2 = widths[i] - min_content;
                    if (limit2 < 0) limit2 = (int)Math.Round(widths[i] * .5F);
                    if (limit2 < limit) limit = limit2;
                    if (limit < 0) limit = 0;
                    if (label_ws[i] > limit) label_ws[i] = limit;
                }
            }

            #endregion

            #region 计算矩形

            int y = use_y, right = rect.X;
            foreach (var row in rows)
            {
                int row_h = 0;
                foreach (var it in row.Items)
                {
                    int x = rect.X + Sum(widths, 0, it.ColStart);
                    int iw = Sum(widths, it.ColStart, it.ColSpan);
                    if (iw < 1) iw = 1;

                    var cell = new DescriptionCell { Item = it };
                    if (vertical)
                    {
                        int lw = iw;
                        int avail_text = lw - pad_h * 2;
                        if (avail_text < 1) avail_text = 1;
                        int h_label = string.IsNullOrEmpty(it.LabelText) ? 0 : g.MeasureText(it.LabelText, Font, avail_text).Height;
                        int h_content = MeasureContent(g, it, line_h, out int _cw, avail_text - it.BadgeW);
                        int h_label_cell = string.IsNullOrEmpty(it.LabelText) ? 0 : pad_v + h_label + pad_v;
                        int h_content_cell = _cw > 0 ? pad_v + h_content + pad_v : pad_v + line_h + pad_v;
                        var rect_label = new Rectangle(x, y, lw, h_label_cell);
                        var rect_content = new Rectangle(x, y + h_label_cell, lw, h_content_cell);
                        cell.CellLabel = rect_label;
                        cell.CellContent = rect_content;
                        cell.RectLabel = new Rectangle(x + pad_h, y + pad_v, lw - pad_h * 2, h_label_cell - pad_v * 2 < 0 ? 0 : h_label_cell - pad_v * 2);
                        cell.RectContent = ContentRect(it, rect_content.X + pad_h, rect_content.Y + pad_v, rect_content.Width - pad_h * 2, rect_content.Height - pad_v * 2 < 0 ? 0 : rect_content.Height - pad_v * 2);
                        cell.RectDot = DotRect(it, line_h, cell.RectContent);
                        int h = h_label_cell + h_content_cell;
                        if (h > row_h) row_h = h;
                    }
                    else
                    {
                        int lw = label_ws[it.ColStart < 0 ? 0 : (it.ColStart >= column_max ? column_max - 1 : it.ColStart)];
                        if (lw > iw) lw = iw;
                        if (lw < 0) lw = 0;

                        int avail_label = lw - pad_h * 2, avail_content = iw - lw - pad_h * 2;
                        if (avail_label < 1) avail_label = 1;
                        if (avail_content < 1) avail_content = 1;

                        int h_label = string.IsNullOrEmpty(it.LabelText) ? 0 : g.MeasureText(it.LabelText, Font, avail_label).Height;
                        int h_content = it.ContentW > 0 ? MeasureContent(g, it, line_h, out int _cw2, avail_content - it.BadgeW) : 0;
                        int cell_h = (h_label > h_content ? h_label : h_content) + pad_v * 2;
                        if (cell_h < line_h + pad_v * 2) cell_h = line_h + pad_v * 2;

                        cell.CellLabel = new Rectangle(x, y, lw, cell_h);
                        cell.CellContent = new Rectangle(x + lw, y, iw - lw, cell_h);
                        cell.RectLabel = new Rectangle(x + pad_h, y + pad_v, lw - pad_h * 2 < 0 ? 0 : lw - pad_h * 2, cell_h - pad_v * 2 < 0 ? 0 : cell_h - pad_v * 2);
                        cell.RectContent = ContentRect(it, cell.CellContent.X + pad_h, cell.CellContent.Y + pad_v, cell.CellContent.Width - pad_h * 2 < 0 ? 0 : cell.CellContent.Width - pad_h * 2, cell_h - pad_v * 2 < 0 ? 0 : cell_h - pad_v * 2);
                        cell.RectDot = DotRect(it, line_h, cell.RectContent);
                        if (cell_h > row_h) row_h = cell_h;
                    }
                    row.Cells.Add(cell);
                }
                if (!vertical)
                {
                    //水平布局：标签与内容左右并排，整行等高
                    foreach (var cell in row.Cells)
                    {
                        var c_cell_label = cell.CellLabel;
                        c_cell_label.Height = row_h;
                        cell.CellLabel = c_cell_label;
                        var c_cell_content = cell.CellContent;
                        c_cell_content.Height = row_h;
                        cell.CellContent = c_cell_content;
                        var c_label = cell.RectLabel;
                        c_label.Height = row_h - pad_v * 2 < 0 ? 0 : row_h - pad_v * 2;
                        cell.RectLabel = c_label;
                        var c_content = cell.RectContent;
                        c_content.Height = row_h - pad_v * 2 < 0 ? 0 : row_h - pad_v * 2;
                        cell.RectContent = c_content;
                        cell.RectDot = DotRect(cell.Item, line_h, cell.RectContent);
                    }
                }
                row.Rect = new Rectangle(rect.X, y, Sum(widths, 0, column_max), row_h);
                if (row.Rect.Right > right) right = row.Rect.Right;
                y += row_h + pad_row;
            }
            result.Rows.AddRange(rows);

            #endregion

            if (rows.Count > 0)
            {
                var first = rows[0].Rect;
                int bottom = rows[rows.Count - 1].Rect.Bottom;
                result.RectTable = new Rectangle(first.X, first.Y, right - first.X, bottom - first.Y);
            }
            int w_total = right - rect.X;
            if (result.RectTitle.Width > w_total) w_total = result.RectTitle.Width;
            return new Size(w_total, rows.Count > 0 ? y - pad_row - rect.Y : y - rect.Y);
        }

        float SizeFactor()
        {
            switch (sizeMode)
            {
                case TSizeMode.Middle: return .8F;
                case TSizeMode.Small: return .62F;
                default: return 1F;
            }
        }

        FormatFlags WrapFlags() => textWrap ? 0 : FormatFlags.NoWrapEllipsis;

        static int Sum(int[] arr, int start, int count)
        {
            int sum = 0;
            for (int i = start; i < start + count && i >= 0 && i < arr.Length; i++) sum += arr[i];
            return sum;
        }

        int MeasureContent(Canvas g, DescriptionsItem it, int line_h, out int content_width, int avail)
        {
            if (avail < 1) avail = 1;
            if (it.IsBadge)
            {
                content_width = it.BadgeW;
                if (string.IsNullOrEmpty(it.ContentText)) return line_h;
                var size_badge = g.MeasureText(it.ContentText, Font, avail);
                content_width += size_badge.Width;
                return size_badge.Height;
            }
            if (string.IsNullOrEmpty(it.ContentText))
            {
                content_width = 0;
                return 0;
            }
            var size = g.MeasureText(it.ContentText, Font, avail);
            content_width = size.Width;
            return size.Height;
        }

        Rectangle ContentRect(DescriptionsItem it, int x, int y, int w, int h)
        {
            if (h < 0) h = 0;
            if (w < 0) w = 0;
            if (it.IsBadge && !string.IsNullOrEmpty(it.ContentText))
            {
                //有文本时前方预留徽标位置，仅圆点时占满以便居中
                int w2 = w - it.BadgeW;
                if (w2 < 0) w2 = 0;
                return new Rectangle(x + it.BadgeW, y, w2, h);
            }
            return new Rectangle(x, y, w, h);
        }

        Rectangle DotRect(DescriptionsItem it, int line_h, Rectangle rect_content)
        {
            if (!it.IsBadge) return Rectangle.Empty;
            int size = (int)Math.Round(line_h * badgeRatio);
            int x = rect_content.X - it.BadgeW + (line_h - size) / 2;
            if (string.IsNullOrEmpty(it.ContentText))
            {
                return new Rectangle(rect_content.X + (rect_content.Width - size) / 2, rect_content.Y + (rect_content.Height - size) / 2, size, size);
            }
            return new Rectangle(x, rect_content.Y + (rect_content.Height - size) / 2, size, size);
        }

        #endregion

        #region 渲染

        protected override void OnDraw(DrawEventArgs e)
        {
            if (layout.Rows.Count == 0 && layout.RectTitle.Width == 0) ChangeList(false);
            PaintBack(e);

            var g = e.Canvas;
            bool enabled = Enabled;
            float dpi = Dpi, bor_w = borderWidth * dpi;

            #region 标题

            if (layout.RectTitle.Width > 0)
            {
                using (var font_title = new Font(Font.FontFamily, Font.Size * titleFontRatio, Font.Style | FontStyle.Bold))
                {
                    using (var brush = new SolidBrush(enabled ? (titleFore ?? Colour.TextBase.Get(ColorScheme, nameof(Descriptions), Name)) : Colour.TextQuaternary.Get(ColorScheme, nameof(Descriptions), Name)))
                    {
                        g.DrawText(Title, font_title, brush, layout.RectTitle, textAl | WrapFlags());
                    }
                }
            }

            #endregion

            var color_border = borderColor ?? Colour.Split.Get(ColorScheme, nameof(Descriptions), Name);
            var color_label_bg = labelBack ?? Colour.FillQuaternary.Get(ColorScheme, nameof(Descriptions), Name);
            bool vertical = itemLayout == TDescriptionsLayout.Vertical;
            var rows = layout.Rows;

            #region 背景/线条

            if (bordered && rows.Count > 0)
            {
                var rect_table = layout.RectTable;
                if (rect_table.Width > 0 && rect_table.Height > 0)
                {
                    using (var path = rect_table.RoundPath(radius * dpi))
                    {
                        g.SetClip(path);
                        if (back.HasValue || backExtend != null)
                        {
                            using (var brush = backExtend.BrushEx(rect_table, back ?? Color.Transparent))
                            {
                                g.Fill(brush, rect_table);
                            }
                        }
                        foreach (var row in rows)
                        {
                            foreach (var cell in row.Cells)
                            {
                                if (cell.CellLabel.Width > 0 && cell.CellLabel.Height > 0) g.Fill(color_label_bg, cell.CellLabel);
                            }
                        }
                        if (bor_w > 0)
                        {
                            using (var pen = new Pen(color_border, bor_w))
                            {
                                pen.DashStyle = borderStyle;
                                for (int i = 0; i < rows.Count - 1; i++)
                                {
                                    float ly = rows[i].Rect.Bottom;
                                    g.DrawLine(pen, rect_table.X, ly, rect_table.Right, ly);
                                }
                                foreach (var row in rows)
                                {
                                    for (int i = 0; i < row.Cells.Count; i++)
                                    {
                                        var cell = row.Cells[i];
                                        if (vertical)
                                        {
                                            if (i > 0) g.DrawLine(pen, cell.CellLabel.X, row.Rect.Y, cell.CellLabel.X, row.Rect.Bottom);
                                            if (cell.CellLabel.Height > 0)
                                            {
                                                float ly = cell.CellLabel.Bottom;
                                                g.DrawLine(pen, cell.CellLabel.X, ly, cell.CellLabel.Right, ly);
                                            }
                                        }
                                        else
                                        {
                                            if (i > 0) g.DrawLine(pen, cell.CellLabel.X, row.Rect.Y, cell.CellLabel.X, row.Rect.Bottom);
                                            if (cell.CellLabel.Width > 0 && cell.CellContent.Width > 0)
                                            {
                                                float lx = cell.CellLabel.Right;
                                                g.DrawLine(pen, lx, row.Rect.Y, lx, row.Rect.Bottom);
                                            }
                                        }
                                    }
                                    //行未铺满时补齐右侧竖线（空单元格）
                                    if (row.Cells.Count > 0)
                                    {
                                        var last = row.Cells[row.Cells.Count - 1];
                                        int rx = vertical ? last.CellLabel.Right : last.CellContent.Right;
                                        if (rx < rect_table.Right) g.DrawLine(pen, rx, row.Rect.Y, rx, row.Rect.Bottom);
                                    }
                                }
                            }
                        }
                        g.ResetClip();
                        if (bor_w > 0)
                        {
                            using (var pen = new Pen(color_border, bor_w))
                            {
                                pen.DashStyle = borderStyle;
                                g.Draw(pen, path);
                            }
                        }
                    }
                }
            }

            #endregion

            #region 文本

            foreach (var row in rows)
            {
                foreach (var cell in row.Cells)
                {
                    var it = cell.Item;
                    if (it.IsBadge && cell.RectDot.Width > 0)
                    {
                        using (var brush = new SolidBrush(enabled ? GetColor(it.Fill, it.State) : Colour.TextQuaternary.Get(ColorScheme, nameof(Descriptions), Name)))
                        {
                            g.FillEllipse(brush, cell.RectDot);
                        }
                    }
                    if (!string.IsNullOrEmpty(it.LabelText))
                    {
                        using (var brush = new SolidBrush(enabled ? ReadLabelFore(it) : Colour.TextQuaternary.Get(ColorScheme, nameof(Descriptions), Name)))
                        {
                            g.DrawText(it.LabelText, Font, brush, cell.RectLabel, labelAl | WrapFlags());
                        }
                    }
                    if (!string.IsNullOrEmpty(it.ContentText) && cell.RectContent.Width > 0)
                    {
                        using (var brush = new SolidBrush(enabled ? ReadFore(it) : Colour.TextQuaternary.Get(ColorScheme, nameof(Descriptions), Name)))
                        {
                            g.DrawText(it.ContentText, Font, brush, cell.RectContent, textAl | WrapFlags());
                        }
                    }
                }
            }

            #endregion

            base.OnDraw(e);
        }

        void PaintBack(DrawEventArgs e)
        {
            if (back.HasValue || backExtend != null)
            {
                var rect = e.Rect;
                if (rect.Width == 0 || rect.Height == 0) return;
                using (var brush = backExtend.BrushEx(rect, back ?? Color.Transparent))
                {
                    e.Canvas.Fill(brush, rect);
                }
            }
        }

        #region 渲染帮助

        Color ReadFore(DescriptionsItem it)
        {
            if (it.ForeColor.HasValue) return it.ForeColor.Value;
            if (fore.HasValue) return fore.Value;
            return Colour.Text.Get(ColorScheme, nameof(Descriptions), Name);
        }

        Color ReadLabelFore(DescriptionsItem it)
        {
            if (it.LabelFore.HasValue) return it.LabelFore.Value;
            if (labelFore.HasValue) return labelFore.Value;
            return Colour.TextSecondary.Get(ColorScheme, nameof(Descriptions), Name);
        }

        Color GetColor(Color? fill, TState state)
        {
            if (fill.HasValue) return fill.Value;
            switch (state)
            {
                case TState.Success: return Colour.Success.Get(ColorScheme, nameof(Descriptions), Name);
                case TState.Error: return Colour.Error.Get(ColorScheme, nameof(Descriptions), Name);
                case TState.Primary:
                case TState.Processing: return Colour.Primary.Get(ColorScheme, nameof(Descriptions), Name);
                case TState.Warn: return Colour.Warning.Get(ColorScheme, nameof(Descriptions), Name);
                default: return Colour.TextQuaternary.Get(ColorScheme, nameof(Descriptions), Name);
            }
        }

        #endregion

        #endregion

        #region 自动大小

        /// <summary>
        /// 自动大小
        /// </summary>
        [Browsable(true)]
        [Description("自动大小"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(false)]
        public override bool AutoSize
        {
            get => base.AutoSize;
            set
            {
                if (base.AutoSize == value) return;
                base.AutoSize = value;
                if (value)
                {
                    if (autoSize == TAutoSize.None) autoSize = TAutoSize.Auto;
                }
                else autoSize = TAutoSize.None;
                BeforeAutoSize();
            }
        }

        TAutoSize autoSize = TAutoSize.None;
        /// <summary>
        /// 自动大小模式
        /// </summary>
        [Description("自动大小模式"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(TAutoSize.None)]
        public TAutoSize AutoSizeMode
        {
            get => autoSize;
            set
            {
                if (autoSize == value) return;
                autoSize = value;
                base.AutoSize = autoSize != TAutoSize.None;
                BeforeAutoSize();
            }
        }

        protected override void OnFontChanged(EventArgs e)
        {
            BeforeAutoSize();
            ChangeList();
            base.OnFontChanged(e);
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            switch (autoSize)
            {
                case TAutoSize.None: return base.GetPreferredSize(proposedSize);
                case TAutoSize.Width: return new Size(PSize.Width, base.GetPreferredSize(proposedSize).Height);
                case TAutoSize.Height: return new Size(base.GetPreferredSize(proposedSize).Width, PSize.Height);
                default: return PSize;
            }
        }

        public virtual Size PSize
        {
            get
            {
                return this.GDI(g =>
                {
                    int avail = ClientRectangle.Width;
                    if (autoSize == TAutoSize.Height && avail > 0)
                    {
                        //仅高度自适应：按当前宽度换行计算所需高度
                        var size = LayoutCore(g, new Rectangle(0, 0, avail, 1), new DescriptionLayout());
                        return new Size(avail, Deflate(size.Height, true));
                    }
                    //宽高自适应：按内容自然宽度计算
                    var used = LayoutCore(g, new Rectangle(0, 0, 100000, 1), new DescriptionLayout());
                    return new Size(Deflate(used.Width, false), Deflate(used.Height, true));
                });
            }
        }

        /// <summary>
        /// 补回 Padding
        /// </summary>
        int Deflate(int value, bool is_h)
        {
            int val = is_h ? value + Padding.Vertical : value + Padding.Horizontal;
            return val < 1 ? 1 : val + 1;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            ChangeList();
            base.OnSizeChanged(e);
        }

        bool BeforeAutoSize()
        {
            if (autoSize == TAutoSize.None) return true;
            if (InvokeRequired) return ITask.Invoke(this, BeforeAutoSize);
            var size = PSize;
            if (size.Width < 1) size.Width = 1;
            if (size.Height < 1) size.Height = 1;
            return this.SetAutoSize(autoSize, size);
        }

        #endregion

        #region 语言变化

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            ChangeList();
            this.AddListener();
        }

        public void HandleEvent(EventType id, object? tag)
        {
            switch (id)
            {
                case EventType.LANG:
                    ChangeList(true);
                    break;
            }
        }

        #endregion
    }

    #region 布局结果

    internal sealed class DescriptionLayout
    {
        /// <summary>
        /// 标题区域
        /// </summary>
        public Rectangle RectTitle { get; set; }

        /// <summary>
        /// 表格区域
        /// </summary>
        public Rectangle RectTable { get; set; }

        /// <summary>
        /// 行集合
        /// </summary>
        public List<DescriptionRow> Rows { get; } = new List<DescriptionRow>();
    }

    internal sealed class DescriptionRow
    {
        public Rectangle Rect { get; set; }
        public List<DescriptionsItem> Items { get; } = new List<DescriptionsItem>();
        public List<DescriptionCell> Cells { get; } = new List<DescriptionCell>();
    }

    internal sealed class DescriptionCell
    {
        /// <summary>
        /// 所属项
        /// </summary>
        public DescriptionsItem Item { get; set; } = new DescriptionsItem();

        /// <summary>
        /// 标签单元格（含内边距）
        /// </summary>
        public Rectangle CellLabel { get; set; }

        /// <summary>
        /// 内容单元格（含内边距）
        /// </summary>
        public Rectangle CellContent { get; set; }

        /// <summary>
        /// 标签文本区域
        /// </summary>
        public Rectangle RectLabel { get; set; }

        /// <summary>
        /// 内容文本区域
        /// </summary>
        public Rectangle RectContent { get; set; }

        /// <summary>
        /// 徽标圆点区域
        /// </summary>
        public Rectangle RectDot { get; set; }
    }

    #endregion

    #region 集合

    public class DescriptionsItemCollection : iCollection<DescriptionsItem>
    {
        public DescriptionsItemCollection(Descriptions it)
        {
            BindData(it);
        }

        internal DescriptionsItemCollection BindData(Descriptions it)
        {
            action = render =>
            {
                if (render) it.ChangeList(true);
                else it.Invalidate();
            };
            return this;
        }

        /// <summary>
        /// 按 ID 查找项
        /// </summary>
        /// <param name="id">ID</param>
        public DescriptionsItem? GetByID(string id) => Find(a => a.ID == id);

        /// <summary>
        /// 按名称查找项
        /// </summary>
        /// <param name="name">名称</param>
        public DescriptionsItem? GetByName(string name) => Find(a => a.Name == name);
    }

    #endregion

    #region 项

    /// <summary>
    /// Descriptions 描述列表项
    /// </summary>
    public class DescriptionsItem
    {
        /// <summary>
        /// 描述列表项
        /// </summary>
        public DescriptionsItem() { }
        public DescriptionsItem(string? label, string? text)
        {
            Label = label;
            Text = text;
        }
        public DescriptionsItem(string? label, string? text, int span)
        {
            Label = label;
            Text = text;
            Span = span;
        }
        public DescriptionsItem(string? label, object? value, string? format)
        {
            Label = label;
            Value = value;
            Format = format;
        }

        /// <summary>
        /// ID
        /// </summary>
        [Description("ID"), Category(nameof(CategoryAttribute.Data)), DefaultValue(null)]
        public string? ID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Description("名称"), Category(nameof(CategoryAttribute.Data)), DefaultValue(null)]
        public string? Name { get; set; }

        /// <summary>
        /// 用户定义数据
        /// </summary>
        [Description("用户定义数据"), Category(nameof(CategoryAttribute.Data)), DefaultValue(null)]
        [TypeConverter(typeof(StringConverter))]
        public object? Tag { get; set; }

        internal Descriptions? PARENT { get; set; }

        void Change(bool relayout = true)
        {
            if (relayout) PARENT?.ChangeList(true);
            else PARENT?.Invalidate();
        }

        bool visible = true;
        /// <summary>
        /// 是否可见
        /// </summary>
        [Description("是否可见"), Category(nameof(CategoryAttribute.Behavior)), DefaultValue(true)]
        public bool Visible
        {
            get => visible;
            set
            {
                if (visible == value) return;
                visible = value;
                Change();
            }
        }

        #region 标签

        string? label;
        /// <summary>
        /// 标签
        /// </summary>
        [Description("标签"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null), Localizable(true)]
        public string? Label
        {
            get => label;
            set
            {
                if (label == value) return;
                label = value;
                Change();
            }
        }

        /// <summary>
        /// 标签
        /// </summary>
        [Description("标签"), Category("国际化"), DefaultValue(null)]
        public string? LocalizationLabel { get; set; }

        Color? labelFore;
        /// <summary>
        /// 标签文字颜色
        /// </summary>
        [Description("标签文字颜色"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? LabelFore
        {
            get => labelFore;
            set
            {
                if (labelFore == value) return;
                labelFore = value;
                Change(false);
            }
        }

        #endregion

        #region 内容

        string? text;
        /// <summary>
        /// 文本
        /// </summary>
        [Description("文本"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null), Localizable(true)]
        public string? Text
        {
            get => text;
            set
            {
                if (text == value) return;
                text = value;
                Change();
            }
        }

        /// <summary>
        /// 文本
        /// </summary>
        [Description("文本"), Category("国际化"), DefaultValue(null)]
        public string? LocalizationText { get; set; }

        object? value;
        /// <summary>
        /// 原始值（优先于文本，配合格式化）
        /// </summary>
        [Description("原始值（优先于文本，配合格式化）"), Category(nameof(CategoryAttribute.Data)), DefaultValue(null)]
        public object? Value
        {
            get => value;
            set
            {
                if (this.value == value) return;
                this.value = value;
                Change();
            }
        }

        string? format;
        /// <summary>
        /// 文本格式化（标准格式字符串或含 {0} 的复合格式）
        /// </summary>
        [Description("文本格式化（标准格式字符串或含 {0} 的复合格式）"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null)]
        public string? Format
        {
            get => format;
            set
            {
                if (format == value) return;
                format = value;
                Change();
            }
        }

        Color? fore;
        /// <summary>
        /// 内容文字颜色
        /// </summary>
        [Description("内容文字颜色"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? ForeColor
        {
            get => fore;
            set
            {
                if (fore == value) return;
                fore = value;
                Change(false);
            }
        }

        TState state = TState.Default;
        /// <summary>
        /// 状态（徽标色彩）
        /// </summary>
        [Description("状态（徽标色彩）"), Category("徽标"), DefaultValue(TState.Default)]
        public TState State
        {
            get => state;
            set
            {
                if (state == value) return;
                state = value;
                Change(false);
            }
        }

        Color? fill;
        /// <summary>
        /// 徽标颜色
        /// </summary>
        [Description("徽标颜色"), Category("徽标"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? Fill
        {
            get => fill;
            set
            {
                if (fill == value) return;
                fill = value;
                Change(false);
            }
        }

        TDescriptionsContentType contentType = TDescriptionsContentType.Text;
        /// <summary>
        /// 内容类型（文本/徽标）
        /// </summary>
        [Description("内容类型（文本/徽标）"), Category("徽标"), DefaultValue(TDescriptionsContentType.Text)]
        public TDescriptionsContentType ContentType
        {
            get => contentType;
            set
            {
                if (contentType == value) return;
                contentType = value;
                Change();
            }
        }

        #endregion

        #region 布局

        int span = 1;
        /// <summary>
        /// 包含列的数量（小于等于0=铺满当前行剩余部分）
        /// </summary>
        [Description("包含列的数量（小于等于0=铺满当前行剩余部分）"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(1)]
        public int Span
        {
            get => span;
            set
            {
                if (span == value) return;
                span = value;
                Change();
            }
        }

        #endregion

        #region 内部

        internal string LabelText { get; private set; } = string.Empty;
        internal string ContentText { get; private set; } = string.Empty;
        internal int LabelW { get; private set; }
        internal int ContentW { get; private set; }
        internal int BadgeW { get; private set; }
        internal int ColStart { get; set; }
        internal int ColSpan { get; set; } = 1;
        internal bool IsBadge => contentType == TDescriptionsContentType.Badge;

        internal void Prepare(Descriptions control, Canvas g, Font font, bool vertical, bool colon, int badge_reserve)
        {
            string? label_str = Localization.GetLangI(LocalizationLabel, label, new string?[] { "{id}", ID }, new string?[] { "{name}", Name });
            if (colon && !string.IsNullOrEmpty(label_str)) label_str += ":";
            LabelText = label_str ?? string.Empty;
            ContentText = GetContentText();

            LabelW = LabelText.Length == 0 ? 0 : g.MeasureText(LabelText, font).Width;
            BadgeW = IsBadge ? badge_reserve : 0;
            ContentW = 0;
            if (IsBadge) ContentW = badge_reserve;
            if (ContentText.Length > 0) ContentW += g.MeasureText(ContentText, font).Width;
        }

        /// <summary>
        /// 获取格式化后的内容文本
        /// </summary>
        public string GetContentText()
        {
            string? tmp;
            if (value != null)
            {
                string? format_str = format;
                if (string.IsNullOrEmpty(format_str)) tmp = Convert.ToString(value, CultureInfo.CurrentCulture);
                else if (format_str!.IndexOf("{0", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    try { tmp = string.Format(CultureInfo.CurrentCulture, format_str, value); }
                    catch { tmp = Convert.ToString(value, CultureInfo.CurrentCulture); }
                }
                else if (value is IFormattable fmt) tmp = fmt.ToString(format_str, CultureInfo.CurrentCulture);
                else tmp = Convert.ToString(value, CultureInfo.CurrentCulture);
            }
            else tmp = Localization.GetLangI(LocalizationText, text, new string?[] { "{id}", ID }, new string?[] { "{name}", Name });
            string ret = tmp ?? string.Empty;
            if (!string.IsNullOrEmpty(ID)) ret = ret.Replace("{id}", ID!);
            if (!string.IsNullOrEmpty(Name)) ret = ret.Replace("{name}", Name!);
            return ret;
        }

        public override string ToString() => Label ?? Text ?? string.Empty;

        #endregion

        #region 流式赋值

        public DescriptionsItem SetID(string? id) { ID = id; return this; }
        public DescriptionsItem SetName(string? name) { Name = name; return this; }
        public DescriptionsItem SetLabel(string? text, string? localization = null) { Label = text; LocalizationLabel = localization; return this; }
        public DescriptionsItem SetText(string? text, string? localization = null) { Text = text; LocalizationText = localization; return this; }
        public DescriptionsItem SetValue(object? val, string? fmt) { Value = val; Format = fmt; return this; }
        public DescriptionsItem SetSpan(int value) { Span = value; return this; }
        public DescriptionsItem SetVisible(bool value = true) { Visible = value; return this; }
        public DescriptionsItem SetTag(object? value) { Tag = value; return this; }
        public DescriptionsItem SetFore(Color? color) { ForeColor = color; return this; }
        public DescriptionsItem SetState(TState value) { State = value; return this; }
        public DescriptionsItem SetFill(Color? color) { Fill = color; return this; }
        public DescriptionsItem SetContentType(TDescriptionsContentType type) { ContentType = type; return this; }

        #endregion
    }

    #endregion

    #region 枚举

    /// <summary>
    /// 描述布局
    /// </summary>
    public enum TDescriptionsLayout
    {
        /// <summary>
        /// 水平
        /// </summary>
        Horizontal,
        /// <summary>
        /// 垂直
        /// </summary>
        Vertical
    }

    /// <summary>
    /// 列宽模式
    /// </summary>
    public enum TDescriptionsColumnWidth
    {
        /// <summary>
        /// 自动（带边框按内容比例，无边框等分）
        /// </summary>
        Auto,
        /// <summary>
        /// 等分
        /// </summary>
        Equal,
        /// <summary>
        /// 按内容比例
        /// </summary>
        Content
    }

    /// <summary>
    /// 描述列表内容类型
    /// </summary>
    public enum TDescriptionsContentType
    {
        /// <summary>
        /// 文本
        /// </summary>
        Text,
        /// <summary>
        /// 徽标（圆点+文本）
        /// </summary>
        Badge
    }

    #endregion
}