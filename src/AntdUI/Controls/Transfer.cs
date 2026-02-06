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
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// Transfer 穿梭框
    /// </summary>
    /// <remarks>双栏穿梭选择框，用于在两个区域之间移动元素。</remarks>
    [Description("Transfer 穿梭框")]
    [ToolboxItem(true)]
    [DefaultEvent("TransferChanged")]
    [Designer(typeof(IControlDesigner))]
    public class Transfer : IControl
    {
        #region 属性

        /// <summary>
        /// 左侧列表标题
        /// </summary>
        [Description("左侧列表标题"), Category("数据"), DefaultValue(null), Localizable(true)]
        public string? SourceTitle { get; set; }

        /// <summary>
        /// 右侧列表标题
        /// </summary>
        [Description("右侧列表标题"), Category("数据"), DefaultValue(null), Localizable(true)]
        public string? TargetTitle { get; set; }

        #region 搜索框

        bool showSearch = false;
        /// <summary>
        /// 是否显示搜索框
        /// </summary>
        [Description("是否显示搜索框"), Category("行为"), DefaultValue(false)]
        public bool ShowSearch
        {
            get => showSearch;
            set
            {
                if (showSearch == value) return;
                showSearch = value;
                LoadLayout();
            }
        }

        string? placeholderSource, placeholderTarget;
        /// <summary>
        /// 搜索的水印文本
        /// </summary>
        [Description("搜索的水印文本 源"), Category("行为"), DefaultValue(null)]
        [Localizable(true)]
        public string? SearchPlaceholderSource
        {
            get => placeholderSource;
            set
            {
                if (placeholderSource == value) return;
                placeholderSource = value;
                if (input_source == null) return;
                input_source.LocalizationPlaceholderText = placeholderSourceLocalization;
                input_source.PlaceholderText = placeholderSource;
            }
        }

        /// <summary>
        /// 搜索的水印文本
        /// </summary>
        [Description("搜索的水印文本 目标"), Category("行为"), DefaultValue(null)]
        [Localizable(true)]
        public string? SearchPlaceholderTarget
        {
            get => placeholderTarget;
            set
            {
                if (placeholderTarget == value) return;
                placeholderTarget = value;
                if (input_target == null) return;
                input_target.LocalizationPlaceholderText = placeholderTargetLocalization;
                input_target.PlaceholderText = placeholderTarget;
            }
        }

        string? placeholderSourceLocalization, placeholderTargetLocalization;
        /// <summary>
        /// 搜索的水印文本 源
        /// </summary>
        [Description("搜索的水印文本 源"), Category("国际化"), DefaultValue(null)]
        public string? LocalizationSearchPlaceholderSource
        {
            get => placeholderSourceLocalization;
            set
            {
                if (placeholderSourceLocalization == value) return;
                placeholderSourceLocalization = value;
                if (input_source == null) return;
                input_source.LocalizationPlaceholderText = placeholderSourceLocalization;
                input_source.PlaceholderText = placeholderSource;
            }
        }

        /// <summary>
        /// 搜索的水印文本 目标
        /// </summary>
        [Description("搜索的水印文本 目标"), Category("国际化"), DefaultValue(null)]
        public string? LocalizationSearchPlaceholderTarget
        {
            get => placeholderTargetLocalization;
            set
            {
                if (placeholderTargetLocalization == value) return;
                placeholderTargetLocalization = value;
                if (input_target == null) return;
                input_target.LocalizationPlaceholderText = placeholderTargetLocalization;
                input_target.PlaceholderText = placeholderTarget;
            }
        }

        #endregion

        /// <summary>
        /// 是否显示全选勾选框
        /// </summary>
        [Description("是否显示全选勾选框"), Category("行为"), DefaultValue(true)]
        public bool ShowSelectAll { get; set; } = true;

        /// <summary>
        /// 是否单向模式（只能从左到右）
        /// </summary>
        [Description("是否单向模式（只能从左到右）"), Category("行为"), DefaultValue(false)]
        public bool OneWay { get; set; }

        /// <summary>
        /// 改变后到最下面
        /// </summary>
        [Description("改变后到最下面"), Category("行为"), DefaultValue(false)]
        public bool ChangeToBottom { get; set; }

        /// <summary>
        /// 列表项高度
        /// </summary>
        [Description("列表项高度"), Category("外观"), DefaultValue(null)]
        public int? ItemHeight { get; set; }

        /// <summary>
        /// 列表框圆角
        /// </summary>
        [Description("列表框圆角"), Category("外观"), DefaultValue(6)]
        public int PanelRadius { get; set; } = 6;

        /// <summary>
        /// 列表框背景颜色
        /// </summary>
        [Description("列表框背景颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? PanelBack { get; set; }

        /// <summary>
        /// 列表框边框颜色
        /// </summary>
        [Description("列表框边框颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? BorderColor { get; set; }

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

        /// <summary>
        /// 文字颜色
        /// </summary>
        [Description("文字颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public new Color? ForeColor { get; set; }

        /// <summary>
        /// 激活文字颜色
        /// </summary>
        [Description("激活文字颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? ForeActive { get; set; }

        /// <summary>
        /// 按钮文字颜色
        /// </summary>
        [Description("按钮文字颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? ButtonForeColor { get; set; }

        /// <summary>
        /// 按钮背景颜色
        /// </summary>
        [Description("按钮背景颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? ButtonBackColor { get; set; }

        /// <summary>
        /// 按钮悬停背景颜色
        /// </summary>
        [Description("按钮悬停背景颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? ButtonBackHover { get; set; }

        /// <summary>
        /// 按钮激活背景颜色
        /// </summary>
        [Description("按钮激活背景颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? ButtonBackActive { get; set; }

        /// <summary>
        /// 按钮禁用背景颜色
        /// </summary>
        [Description("按钮禁用背景颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? ButtonBackDisable { get; set; }

        TransferItemCollection? items;
        /// <summary>
        /// 数据源
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("数据源"), Category("数据"), DefaultValue(null)]
        public TransferItemCollection Items
        {
            get
            {
                items ??= new TransferItemCollection(this);
                return items;
            }
            set => items = value.BindData(this);
        }

        #endregion

        #region 布局

        public override Rectangle DisplayRectangle => ClientRectangle.DeflateRect(Padding);

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            InitializeItems();
            LoadLayout(false);
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

        bool CanLayout()
        {
            if (IsHandleCreated)
            {
                var rect = ClientRectangle;
                if (rect.Width == 0 || rect.Height == 0) return false;
                return true;
            }
            return false;
        }

        Rectangle rect_source, rect_source_com, rect_source_input, rect_sourceTitle, rect_sourceCheckbox, rect_sourceCheckboxText, rect_toRight;
        Rectangle rect_target, rect_target_com, rect_target_input, rect_targetTitle, rect_targetCheckbox, rect_targetCheckboxText, rect_toLeft;
        Point[]? rect_source_line, rect_target_line;
        Input? input_source, input_target;
        internal void LoadLayout(bool print = true)
        {
            if (pause) return;
            if (CanLayout())
            {
                var rect = DisplayRectangle;
                int buttonWidth = (int)(80 * Dpi), buttonSize = (int)(36 * Dpi), buttonGap = (int)(10 * Dpi),
                    buttonY = rect.Y + (rect.Height - (buttonSize * 2 + buttonGap)) / 2, buttonX = rect.X + (rect.Width - buttonSize) / 2,
                    panelWidth = (rect.Width - buttonWidth) / 2;

                // 列表区域
                rect_source = new Rectangle(rect.X, rect.Y, panelWidth, rect.Height);
                rect_target = new Rectangle(rect.Right - panelWidth, rect.Y, panelWidth, rect.Height);

                // 操作按钮区域
                rect_toRight = new Rectangle(buttonX, buttonY, buttonSize, buttonSize);
                rect_toLeft = new Rectangle(buttonX, buttonY + buttonSize + buttonGap, buttonSize, buttonSize);

                this.GDI(g =>
                {
                    int useY = rect.Y, txt_height = g.MeasureString(Config.NullText, Font).Height, check_size = (int)(txt_height * 0.8F), gap = (int)(txt_height * 0.4F), gap2 = gap * 2;
                    int gapx = (int)(txt_height * 0.6F), gapx2 = gapx * 2;
                    int head_height = txt_height * 2, item_height = (int)(txt_height * 1.6F);

                    #region 头部

                    if (ShowSelectAll)
                    {
                        int yi = useY + (head_height - check_size) / 2;
                        rect_sourceCheckbox = new Rectangle(rect_source.X + gapx, yi, check_size, check_size);
                        rect_targetCheckbox = new Rectangle(rect_target.X + gapx, yi, check_size, check_size);
                        int ux = gapx2 + check_size, ux2 = ux + gapx;
                        rect_sourceCheckboxText = new Rectangle(rect_source.X + ux, useY, rect_source.Width - ux2, head_height);
                        rect_targetCheckboxText = new Rectangle(rect_target.X + ux, useY, rect_target.Width - ux2, head_height);

                        rect_sourceTitle = new Rectangle(rect_source.X + ux, useY, rect_source.Width - ux2, head_height);
                        rect_targetTitle = new Rectangle(rect_target.X + ux, useY, rect_target.Width - ux2, head_height);
                    }
                    else
                    {
                        rect_sourceTitle = new Rectangle(rect_source.X + gapx, useY, rect_source.Width - gapx2, head_height);
                        rect_targetTitle = new Rectangle(rect_target.X + gapx, useY, rect_target.Width - gapx2, head_height);
                    }

                    rect_source_line = new Point[] { new Point(rect_source.X, rect_sourceTitle.Bottom), new Point(rect_source.Right, rect_sourceTitle.Bottom) };
                    rect_target_line = new Point[] { new Point(rect_target.X, rect_targetTitle.Bottom), new Point(rect_target.Right, rect_targetTitle.Bottom) };

                    useY += head_height;

                    #endregion

                    #region 搜索栏

                    if (showSearch)
                    {
                        int h = head_height;
                        rect_source_input = new Rectangle(rect_source.X + gap, useY + gap, rect_source.Width - gap2, h);
                        rect_target_input = new Rectangle(rect_target.X + gap, useY + gap, rect_target.Width - gap2, h);
                        if (input_source == null)
                        {
                            input_source = new Input
                            {
                                PrefixSvg = "SearchOutlined",
                                LocalizationPlaceholderText = placeholderSourceLocalization,
                                PlaceholderText = placeholderSource,
                                Location = rect_source_input.Location,
                                Size = rect_source_input.Size,
                                Text = sourceSearchText
                            };
                            OnInputStyle(input_source, true);
                            Controls.Add(input_source);
                            input_source.TextChanged += input_source_TextChanged;
                        }
                        else
                        {
                            input_source.Location = rect_source_input.Location;
                            input_source.Size = rect_source_input.Size;
                        }
                        if (input_target == null)
                        {
                            input_target = new Input
                            {
                                PrefixSvg = "SearchOutlined",
                                LocalizationPlaceholderText = placeholderTargetLocalization,
                                PlaceholderText = placeholderTarget,
                                Location = rect_target_input.Location,
                                Size = rect_target_input.Size,
                                Text = targetSearchText
                            };
                            OnInputStyle(input_target, false);
                            Controls.Add(input_target);
                            input_target.TextChanged += input_target_TextChanged;
                        }
                        else
                        {
                            input_target.Location = rect_target_input.Location;
                            input_target.Size = rect_target_input.Size;
                        }
                        useY += h + gap2;
                    }
                    else
                    {
                        input_source?.Dispose();
                        input_target?.Dispose();
                        input_source = input_target = null;
                    }

                    #endregion

                    int listHeight = rect_source.Height - useY + rect.Y, lh;
                    if (ItemHeight.HasValue) lh = (int)(ItemHeight.Value * Dpi);
                    else lh = item_height;

                    rect_source_com = new Rectangle(rect_source.X, useY, rect_source.Width, listHeight);
                    rect_target_com = new Rectangle(rect_target.X, useY, rect_target.Width, listHeight);

                    if (items == null) return;
                    List<TransferItem> sourceItems = new List<TransferItem>(items.Count), targetItems = new List<TransferItem>(items.Count);
                    foreach (var it in items)
                    {
                        if (it.Visible)
                        {
                            if (it.IsTarget) targetItems.Add(it);
                            else sourceItems.Add(it);
                        }
                    }

                    int y1 = CalculateLayout(g, sourceItems, rect_source_com, lh, gap, gap2, gapx, gapx2, check_size);
                    int y2 = CalculateLayout(g, targetItems, rect_target_com, lh, gap, gap2, gapx, gapx2, check_size);

                    ScrollBarSource.SizeChange(rect_source_com);
                    ScrollBarTarget.SizeChange(rect_target_com);

                    ScrollBarSource.SetVrSize(y1);
                    ScrollBarTarget.SetVrSize(y2);
                });

                if (print) Invalidate();
            }
        }

        int CalculateLayout(Canvas g, List<TransferItem> list, Rectangle rect, int rh, int gap, int gap2, int gapx, int gapx2, int check_size)
        {
            int usey = 0;
            foreach (var it in list)
            {
                it.PARENT = this;
                it.rect = new Rectangle(rect.X, rect.Y + usey, rect.Width, rh);
                it.rect_check = new Rectangle(rect.X + gapx, rect.Y + usey + (rh - check_size) / 2, check_size, check_size);
                int ux = gapx2 + check_size;
                it.rect_text = new Rectangle(rect.X + ux, rect.Y + usey, rect.Width - ux - gapx, rh);
                usey += rh;
            }
            return usey;
        }

        #endregion

        #region 渲染

        protected override void OnDraw(DrawEventArgs e)
        {
            base.OnDraw(e);
            var g = e.Canvas;

            // 绘制源列表
            PaintListPanel(g, rect_source, rect_source_com, rect_sourceTitle, rect_sourceCheckbox, rect_source_line, SourceTitle ?? Localization.Get("Transfer.Source", "源列表"), ScrollBarSource, sourceSelectAll, false);

            // 绘制目标列表
            PaintListPanel(g, rect_target, rect_target_com, rect_targetTitle, rect_targetCheckbox, rect_target_line, TargetTitle ?? Localization.Get("Transfer.Target", "目标列表"), ScrollBarTarget, targetSelectAll, true);

            // 绘制操作按钮
            PaintOperationButtons(g);
        }

        readonly FormatFlags sf = FormatFlags.Left | FormatFlags.VerticalCenter | FormatFlags.NoWrapEllipsis, sfL = FormatFlags.Left | FormatFlags.VerticalCenter, sfR = FormatFlags.Right | FormatFlags.VerticalCenter;
        private void PaintListPanel(Canvas g, Rectangle rect, Rectangle rect_com, Rectangle rect_title, Rectangle rect_checkbox, Point[]? rect_line, string title, ScrollBar scroll, CheckState selectAll, bool isTarget)
        {
            // 绘制面板边框和背景
            var panelRadius = (int)(PanelRadius * Dpi);
            using (var path = rect.RoundPath(panelRadius))
            {
                ScrollBarSource.Radius = ScrollBarTarget.Radius = panelRadius;
                using (var brush = new SolidBrush(PanelBack ?? Colour.BgContainer.Get(nameof(Transfer), ColorScheme)))
                {
                    g.Fill(brush, path);
                }

                PaintListItems(g, rect_com, rect_title, title, scroll, isTarget, panelRadius);

                using (var pen = new Pen(BorderColor ?? Colour.BorderColor.Get(nameof(Transfer), ColorScheme), Dpi))
                {
                    g.Draw(pen, path);
                    g.DrawLines(pen, rect_line!);
                }
            }

            // 绘制全选复选框
            if (ShowSelectAll) PaintCheck(g, rect_checkbox, selectAll);
        }

        private void PaintListItems(Canvas g, Rectangle rect_com, Rectangle rect_title, string title, ScrollBar scroll, bool isTarget, int panelRadius)
        {
            if (items == null) return;
            int count = 0, selectedCount = 0;
            var state = g.Save();
            if (scroll.ShowY)
            {
                using (var path = rect_com.RoundPath(panelRadius, false, false, true, true))
                {
                    g.SetClip(path);
                }
            }
            else g.SetClip(rect_com);
            g.TranslateTransform(0, -scroll.ValueY);
            var name = nameof(Transfer);
            using (var fore = new SolidBrush(ForeColor ?? Colour.Text.Get(name, ColorScheme)))
            using (var foreActive = new SolidBrush(ForeActive ?? Colour.Text.Get(name, ColorScheme)))
            {
                foreach (var it in items)
                {
                    if (it.Visible && it.IsTarget == isTarget)
                    {
                        count++;
                        // 绘制项背景
                        if (it.selected)
                        {
                            selectedCount++;
                            using (var brush = new SolidBrush(BackActive ?? Colour.PrimaryBg.Get(name, ColorScheme)))
                            {
                                g.Fill(brush, it.rect);
                            }
                        }
                        if (it.Hover)
                        {
                            using (var brush = new SolidBrush(BackHover ?? Colour.FillTertiary.Get(name, ColorScheme)))
                            {
                                g.Fill(brush, it.rect);
                            }
                        }

                        PaintItemCheck(g, it);

                        // 绘制项文本
                        if (it.Enabled) g.String(it.Text, Font, it.selected ? foreActive : fore, it.rect_text, sf);
                        else g.String(it.Text, Font, Colour.TextQuaternary.Get(name, ColorScheme), it.rect_text, sf);
                    }
                }
                g.Restore(state);

                // 绘制标题/数量
                string countText = $"{selectedCount}/{count}";

                g.String(title, Font, fore, rect_title, sfL);
                g.String(countText, Font, fore, rect_title, sfR);
            }

            // 绘制滚动条
            if (scroll.Show) scroll.Paint(g, ColorScheme);
        }

        private void PaintCheck(Canvas g, Rectangle rect_checkbox, CheckState state)
        {
            using (var path = rect_checkbox.RoundPath(rect_checkbox.Height * .2F))
            {
                switch (state)
                {
                    case CheckState.Checked:
                        g.Fill(Colour.Primary.Get(nameof(Transfer), ColorScheme), path);
                        g.DrawLines(Colour.BgBase.Get(nameof(Transfer), ColorScheme), 2.6F * Dpi, rect_checkbox.CheckArrow());
                        break;
                    case CheckState.Indeterminate:
                        g.Draw(Colour.BorderColor.Get(nameof(Transfer), ColorScheme), 2F * Dpi, path);
                        g.Fill(Colour.Primary.Get(nameof(Transfer), ColorScheme), Checkbox.PaintBlock(rect_checkbox));
                        break;
                    case CheckState.Unchecked:
                        g.Draw(Colour.BorderColor.Get("FillTertiary", ColorScheme), 2F * Dpi, path);
                        break;
                }
            }
        }

        private void PaintItemCheck(Canvas g, TransferItem it)
        {
            var name = nameof(Transfer);
            using (var path = it.rect_check.RoundPath(it.rect_check.Height * .2F))
            {
                if (it.Enabled)
                {
                    if (it.selected)
                    {
                        g.Fill(Colour.Primary.Get(name, ColorScheme), path);
                        g.DrawLines(Colour.BgBase.Get(name, ColorScheme), 2.6F * Dpi, it.rect_check.CheckArrow());
                    }
                    else g.Draw(Colour.BorderColor.Get(name, ColorScheme), 2F * Dpi, path);
                }
                else
                {
                    g.Fill(Colour.FillQuaternary.Get(name, ColorScheme), path);
                    if (it.selected) g.DrawLines(Colour.TextQuaternary.Get(name, ColorScheme), 2.6F * Dpi, it.rect_check.CheckArrow());
                    g.Draw(Colour.BorderColorDisable.Get(name, ColorScheme), 2F * Dpi, path);
                }
            }
        }

        private void PaintOperationButtons(Canvas g)
        {
            // 获取颜色
            var buttonForeColor = ButtonForeColor ?? Colour.Text.Get(nameof(Transfer), ColorScheme);
            var buttonBackColor = ButtonBackColor ?? Colour.FillTertiary.Get(nameof(Transfer), ColorScheme);
            var buttonBackHover = ButtonBackHover ?? Colour.Primary.Get(nameof(Transfer), ColorScheme);
            var buttonBackActive = ButtonBackActive ?? Colour.PrimaryActive.Get(nameof(Transfer), ColorScheme);
            var buttonBackDisable = ButtonBackDisable ?? Colour.FillTertiary.Get(nameof(Transfer), ColorScheme);

            // 绘制向右按钮
            PaintOperationButton(g, rect_toRight, "RightOutlined", hover_to_right, buttonForeColor, buttonBackColor, buttonBackHover, buttonBackActive, buttonBackDisable);

            // 绘制向左按钮
            PaintOperationButton(g, rect_toLeft, "LeftOutlined", hover_to_left, buttonForeColor, buttonBackColor, buttonBackHover, buttonBackActive, buttonBackDisable);
        }

        private void PaintOperationButton(Canvas g, Rectangle rect, string icon, ITaskOpacity hove, Color foreColor, Color backColor, Color backHover, Color backActive, Color backDisable)
        {
            int gap = (int)(8 * Dpi), gap2 = gap * 2;
            var iconRect = new Rectangle(rect.X + gap, rect.Y + gap, rect.Width - gap2, rect.Height - gap2);
            if (hove.Animation)
            {
                g.FillEllipse(backColor, rect);
                g.FillEllipse(backColor.BlendColors(hove.Value, backHover), rect);
                DrawSvg(g, icon, iconRect, Colour.PrimaryColor.Get(nameof(Transfer), ColorScheme));
            }
            else if (hove.Down)
            {
                g.FillEllipse(backActive, rect);
                DrawSvg(g, icon, iconRect, Colour.PrimaryColor.Get(nameof(Transfer), ColorScheme));
            }
            else if (hove.Switch)
            {
                g.FillEllipse(backHover, rect);
                DrawSvg(g, icon, iconRect, Colour.PrimaryColor.Get(nameof(Transfer), ColorScheme));
            }
            else if (hove.Enable)
            {
                g.FillEllipse(backColor, rect);
                DrawSvg(g, icon, iconRect, foreColor);
            }
            else
            {
                g.FillEllipse(backDisable, rect);
                DrawSvg(g, icon, iconRect, Colour.TextQuaternary.Get(nameof(Transfer), ColorScheme));
            }
        }

        /// <summary>
        /// 绘制SVG图标
        /// </summary>
        /// <param name="g">图形</param>
        /// <param name="icon">图标名称</param>
        /// <param name="rect">矩形</param>
        /// <param name="color">颜色</param>
        void DrawSvg(Canvas g, string icon, Rectangle rect, Color color)
        {
            using (var svg = SvgExtend.GetImgExtend(icon, rect, color))
            {
                if (svg == null) return;
                g.Image(svg, rect);
            }
        }

        #endregion

        #region 鼠标

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (ScrollBarSource.MouseMoveY(e.X, e.Y) && ScrollBarTarget.MouseMoveY(e.X, e.Y))
            {
                base.OnMouseMove(e);

                hover_to_left.Switch = rect_toLeft.Contains(e.X, e.Y);
                hover_to_right.Switch = rect_toRight.Contains(e.X, e.Y);

                int sy1 = ScrollBarSource.ValueY, sy2 = ScrollBarTarget.ValueY;
                if (items == null) return;
                foreach (var it in items)
                {
                    if (it.Visible && it.Enabled)
                    {
                        if (it.IsTarget) it.Hover = it.rect.Contains(e.X, e.Y + sy2);
                        else it.Hover = it.rect.Contains(e.X, e.Y + sy1);
                    }
                    else it.Hover = false;
                }
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (showSearch) Focus();
            if (ScrollBarSource.MouseDownY(e.X, e.Y) && ScrollBarTarget.MouseDownY(e.X, e.Y))
            {
                base.OnMouseDown(e);
                if (e.Button == MouseButtons.Left)
                {
                    if (items == null) return;

                    if (ShowSelectAll)
                    {
                        if (rect_sourceCheckbox.Contains(e.X, e.Y) || rect_sourceCheckboxText.Contains(e.X, e.Y))
                        {
                            // 切换全选状态
                            bool newState = sourceSelectAll == CheckState.Unchecked ? true : false;
                            int count = 0, check_count = 0;
                            // 更新所有项的选中状态
                            foreach (var it in items)
                            {
                                if (it.Visible && it.Enabled)
                                {
                                    if (it.IsTarget) continue;
                                    count++;
                                    it.selected = newState;
                                    if (it.selected) check_count++;
                                }
                            }
                            if (count > 0)
                            {
                                sourceSelectAll = newState ? CheckState.Checked : CheckState.Unchecked;
                                hover_to_right.Enable = check_count > 0;
                                LoadLayout();
                            }
                            return;
                        }
                        if (rect_targetCheckbox.Contains(e.X, e.Y) || rect_targetCheckboxText.Contains(e.X, e.Y))
                        {
                            // 切换全选状态
                            bool newState = targetSelectAll == CheckState.Unchecked ? true : false;
                            int count = 0, check_count = 0;
                            // 更新所有项的选中状态
                            foreach (var it in items)
                            {
                                if (it.Visible && it.Enabled && it.IsTarget)
                                {
                                    count++;
                                    it.selected = newState;
                                    if (it.selected) check_count++;
                                }
                            }
                            if (count > 0)
                            {
                                targetSelectAll = newState ? CheckState.Checked : CheckState.Unchecked;
                                hover_to_left.Enable = !OneWay && check_count > 0;
                                LoadLayout();
                            }
                            return;
                        }
                    }
                    // 处理源列表项点击
                    if (rect_source.Contains(e.X, e.Y))
                    {
                        int sy = ScrollBarSource.ValueY;
                        int change = 0, count = 0, check_count = 0;
                        foreach (var it in items)
                        {
                            if (it.Visible && it.Enabled)
                            {
                                if (it.IsTarget) continue;
                                if (it.rect.Contains(e.X, e.Y + sy))
                                {
                                    it.selected = !it.selected;
                                    change++;
                                }
                                count++;
                                if (it.selected) check_count++;
                            }
                        }
                        if (change > 0)
                        {
                            // 更新全选状态
                            if (check_count > 0) sourceSelectAll = check_count >= count ? CheckState.Checked : CheckState.Indeterminate;
                            else sourceSelectAll = CheckState.Unchecked;
                            hover_to_right.Enable = check_count > 0;
                            Invalidate();
                        }
                    }

                    // 处理目标列表项点击
                    if (rect_target.Contains(e.X, e.Y))
                    {
                        int sy = ScrollBarTarget.ValueY;
                        int change = 0, count = 0, check_count = 0;
                        foreach (var it in items)
                        {
                            if (it.Visible && it.Enabled && it.IsTarget)
                            {
                                if (it.rect.Contains(e.X, e.Y + sy))
                                {
                                    it.selected = !it.selected;
                                    change++;
                                }
                                count++;
                                if (it.selected) check_count++;
                            }
                        }
                        if (change > 0)
                        {
                            if (check_count > 0) targetSelectAll = check_count >= count ? CheckState.Checked : CheckState.Indeterminate;
                            else targetSelectAll = CheckState.Unchecked;
                            hover_to_left.Enable = !OneWay && check_count > 0;
                            Invalidate();
                        }
                    }
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (ScrollBarSource.MouseUp() && ScrollBarTarget.MouseUp())
            {
                base.OnMouseUp(e);
                if (e.Button == MouseButtons.Left)
                {
                    // 执行向右移动操作
                    if (rect_toRight.Contains(e.X, e.Y)) MoveSelectedItemsToRight();
                    if (rect_toLeft.Contains(e.X, e.Y) && !OneWay) MoveSelectedItemsToLeft();
                }
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            if (rect_source.Contains(e.X, e.Y)) ScrollBarSource.MouseWheel(e);
            else if (rect_target.Contains(e.X, e.Y)) ScrollBarTarget.MouseWheel(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            hover_to_left.Switch = false;
            hover_to_right.Switch = false;
            if (items == null) return;
            foreach (var it in items) it.Hover = false;
        }

        #endregion

        #region 事件

        /// <summary>
        /// 穿梭框选项变化事件参数
        /// </summary>
        public class TransferEventArgs : EventArgs
        {
            /// <summary>
            /// 源列表项
            /// </summary>
            public List<TransferItem> SourceItems { get; }

            /// <summary>
            /// 目标列表项
            /// </summary>
            public List<TransferItem> TargetItems { get; }

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="sourceItems">源列表项</param>
            /// <param name="targetItems">目标列表项</param>
            public TransferEventArgs(List<TransferItem> sourceItems, List<TransferItem> targetItems)
            {
                SourceItems = sourceItems;
                TargetItems = targetItems;
            }
        }

        /// <summary>
        /// 穿梭框选项变化事件
        /// </summary>
        [Description("穿梭框选项变化事件"), Category("行为")]
        public event EventHandler<TransferEventArgs>? TransferChanged;

        /// <summary>
        /// 触发穿梭框选项变化事件
        /// </summary>
        /// <param name="sourceItems">源列表项</param>
        /// <param name="targetItems">目标列表项</param>
        protected virtual void OnTransferChanged(List<TransferItem> sourceItems, List<TransferItem> targetItems) => TransferChanged?.Invoke(this, new TransferEventArgs(sourceItems, targetItems));

        /// <summary>
        /// 搜索事件参数
        /// </summary>
        public class SearchEventArgs : EventArgs
        {
            /// <summary>
            /// 搜索文本
            /// </summary>
            public string SearchText { get; }

            /// <summary>
            /// 是否为源列表搜索
            /// </summary>
            public bool IsSource { get; }

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="searchText">搜索文本</param>
            /// <param name="isSource">是否为源列表搜索</param>
            public SearchEventArgs(string searchText, bool isSource)
            {
                SearchText = searchText;
                IsSource = isSource;
            }
        }

        /// <summary>
        /// 搜索事件
        /// </summary>
        [Description("搜索事件"), Category("行为")]
        public event EventHandler<SearchEventArgs>? Search;

        /// <summary>
        /// 触发搜索事件
        /// </summary>
        /// <param name="searchText">搜索文本</param>
        /// <param name="isSource">是否为源列表搜索</param>
        protected virtual void OnSearch(string searchText, bool isSource) => Search?.Invoke(this, new SearchEventArgs(searchText, isSource));

        /// <summary>
        /// 输入样式事件参数
        /// </summary>
        public class InputStyleEventArgs : EventArgs
        {
            /// <summary>
            /// 文本框
            /// </summary>
            public Input Input { get; }

            /// <summary>
            /// 是否为源列表样式
            /// </summary>
            public bool IsSource { get; }

            public InputStyleEventArgs(Input input, bool isSource)
            {
                Input = input;
                IsSource = isSource;
            }
        }

        /// <summary>
        /// 输入样式事件
        /// </summary>
        [Description("输入样式事件"), Category("行为")]
        public event EventHandler<InputStyleEventArgs>? InputStyle;

        /// <summary>
        /// 触发搜索事件
        /// </summary>
        /// <param name="e">事件参数</param>
        protected virtual void OnInputStyle(Input input, bool isSource) => InputStyle?.Invoke(this, new InputStyleEventArgs(input, isSource));

        #endregion

        #region 构造函数

        ScrollBar ScrollBarSource, ScrollBarTarget;
        public Transfer()
        {
            hover_to_right = new ITaskOpacity(nameof(Transfer), this);
            hover_to_left = new ITaskOpacity(nameof(Transfer), this);
            ScrollBarSource = new ScrollBar(this);
            ScrollBarTarget = new ScrollBar(this);
            ScrollBarSource.Back = ScrollBarTarget.Back = false;
            ScrollBarSource.RT = ScrollBarTarget.RT = false;
            ScrollBarSource.RB = ScrollBarTarget.RB = true;
        }

        #endregion

        #region 私有变量

        private string sourceSearchText = "", targetSearchText = "";

        private CheckState sourceSelectAll = CheckState.Unchecked, targetSelectAll = CheckState.Unchecked;

        private ITaskOpacity hover_to_right, hover_to_left;

        #endregion

        #region 私有方法

        private void InitializeItems()
        {
            sourceSelectAll = targetSelectAll = CheckState.Unchecked;
            // 应用过滤
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            if (items == null)
            {
                hover_to_right.Enable = hover_to_left.Enable = false;
                return;
            }
            // 过滤源列表
            int check_source = 0, check_target = 0;
            bool es = string.IsNullOrEmpty(sourceSearchText), et = string.IsNullOrEmpty(targetSearchText);
            if (es && et)
            {
                foreach (var it in items)
                {
                    it.Visible = true;
                    if (it.selected)
                    {
                        if (it.IsTarget) check_target++;
                        else check_source++;
                    }
                }
            }
            else
            {
                foreach (var it in items)
                {
                    if (it.IsTarget)
                    {
                        it.Visible = et || it.Text?.IndexOf(targetSearchText, StringComparison.OrdinalIgnoreCase) >= 0;
                        if (it.Visible && it.Enabled && it.selected) check_target++;
                    }
                    else
                    {
                        it.Visible = es || it.Text?.IndexOf(sourceSearchText, StringComparison.OrdinalIgnoreCase) >= 0;
                        if (it.Visible && it.Enabled && it.selected) check_source++;
                    }
                }
            }
            hover_to_right.Enable = check_source > 0;
            hover_to_left.Enable = !OneWay && check_target > 0;
            LoadLayout();
            ScrollBarSource.ValueY = 0;
            ScrollBarTarget.ValueY = 0;
        }

        private void MoveSelectedItemsToRight()
        {
            if (items == null) return;
            sourceSelectAll = targetSelectAll = CheckState.Unchecked;
            List<TransferItem> sourceItems = new List<TransferItem>(items.Count), targetItems = new List<TransferItem>(items.Count),
                changeItems = new List<TransferItem>(items.Count);
            foreach (var it in items)
            {
                if (it.Visible && it.Enabled)
                {
                    if (it.IsTarget) targetItems.Add(it);
                    else if (it.selected)
                    {
                        changeItems.Add(it);
                        targetItems.Add(it);
                        it.selected = false;
                        it.IsTarget = true;
                    }
                    else sourceItems.Add(it);
                }
            }
            if (changeItems.Count > 0)
            {
                if (ChangeToBottom)
                {
                    pause = true;
                    foreach (var it in changeItems) items.Remove(it);
                    items.AddRange(changeItems);
                    pause = false;
                }
                ApplyFilter();
                OnTransferChanged(sourceItems, targetItems);
            }
        }

        bool pause = false;
        private void MoveSelectedItemsToLeft()
        {
            if (items == null || OneWay) return;
            List<TransferItem> sourceItems = new List<TransferItem>(items.Count), targetItems = new List<TransferItem>(items.Count),
                changeItems = new List<TransferItem>(items.Count);
            sourceSelectAll = targetSelectAll = CheckState.Unchecked;
            foreach (var it in items)
            {
                if (it.Visible && it.Enabled)
                {
                    if (it.IsTarget)
                    {
                        if (it.selected)
                        {
                            changeItems.Add(it);
                            sourceItems.Add(it);
                            it.selected = false;
                            it.IsTarget = false;
                        }
                        else targetItems.Add(it);
                    }
                    else sourceItems.Add(it);
                }
            }
            if (changeItems.Count > 0)
            {
                if (ChangeToBottom)
                {
                    pause = true;
                    foreach (var it in changeItems) items.Remove(it);
                    items.AddRange(changeItems);
                    pause = false;
                }
                ApplyFilter();
                OnTransferChanged(sourceItems, targetItems);
            }
        }

        #endregion

        #region 资源释放

        protected override void Dispose(bool disposing)
        {
            hover_to_right.Dispose();
            hover_to_left.Dispose();
            input_source?.Dispose();
            input_target?.Dispose();
            base.Dispose(disposing);
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 重新加载数据
        /// </summary>
        public void Reload()
        {
            InitializeItems();
            LoadLayout();
        }

        /// <summary>
        /// 获取源列表项
        /// </summary>
        /// <returns>源列表项</returns>
        public List<TransferItem> GetSourceItems()
        {
            if (items == null) return new List<TransferItem>(0);
            var sourceItems = new List<TransferItem>(items.Count);
            foreach (var it in items)
            {
                if (it.Visible && it.Enabled)
                {
                    if (it.IsTarget) continue;
                    sourceItems.Add(it);
                }
            }
            return sourceItems;
        }

        /// <summary>
        /// 获取目标列表项
        /// </summary>
        /// <returns>目标列表项</returns>
        public List<TransferItem> GetTargetItems()
        {
            if (items == null) return new List<TransferItem>(0);
            var targetItems = new List<TransferItem>(items.Count);
            foreach (var it in items)
            {
                if (it.Visible && it.Enabled && it.IsTarget) targetItems.Add(it);
            }
            return targetItems;
        }

        /// <summary>
        /// 设置源列表搜索文本
        /// </summary>
        /// <param name="text">搜索文本</param>
        public void SetSourceSearchText(string text)
        {
            if (sourceSearchText == text) return;
            sourceSearchText = text;
            if (input_source != null) input_source.Text = text;
            OnSearch(text, true);
            ApplyFilter();
        }

        /// <summary>
        /// 设置目标列表搜索文本
        /// </summary>
        /// <param name="text">搜索文本</param>
        public void SetTargetSearchText(string text)
        {
            if (targetSearchText == text) return;
            targetSearchText = text;
            if (input_target != null) input_target.Text = text;
            OnSearch(text, false);
            ApplyFilter();
        }

        private void input_source_TextChanged(object? sender, EventArgs e)
        {
            if (sender is Input input) SetSourceSearchText(input.Text);
        }

        private void input_target_TextChanged(object? sender, EventArgs e)
        {
            if (sender is Input input) SetTargetSearchText(input.Text);
        }

        #endregion
    }

    public class TransferItemCollection : iCollection<TransferItem>
    {
        public TransferItemCollection(Transfer it)
        {
            BindData(it);
        }

        internal TransferItemCollection BindData(Transfer it)
        {
            action = render =>
            {
                if (render) it.LoadLayout(true);
                else it.Invalidate();
            };
            return this;
        }
    }

    /// <summary>
    /// 穿梭框项
    /// </summary>
    public class TransferItem
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public TransferItem()
        {
            Text = "";
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="text">文本</param>
        public TransferItem(string text)
        {
            Text = text;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="value">值</param>
        public TransferItem(string text, object value)
        {
            Text = text;
            Value = value;
        }

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

        string? text;
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
                PARENT?.Invalidate();
            }
        }

        [Description("文本"), Category("国际化"), DefaultValue(null)]
        public string? LocalizationText { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public object? Value { get; set; }

        internal bool selected = false;
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool Selected
        {
            get => selected;
            set
            {
                if (selected == value) return;
                selected = value;
                PARENT?.Invalidate();
            }
        }

        #region 悬浮态

        bool hover = false;
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
                PARENT?.Invalidate();
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
                PARENT?.LoadLayout();
            }
        }

        #region 禁用

        bool enabled = true;
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
                PARENT?.Invalidate();
            }
        }

        #endregion

        /// <summary>
        /// 是否在目标列表
        /// </summary>
        public bool IsTarget { get; set; }

        public object? Tag { get; set; }

        #region 内部

        internal Transfer? PARENT { get; set; }
        internal Rectangle rect { get; set; }
        internal Rectangle rect_text { get; set; }
        internal Rectangle rect_check { get; set; }

        #endregion

        #region 设置

        public TransferItem SetID(string? value)
        {
            ID = value;
            return this;
        }

        public TransferItem SetName(string? value)
        {
            Name = value;
            return this;
        }

        public TransferItem SetText(string? value, string? localization = null)
        {
            text = value;
            LocalizationText = localization;
            return this;
        }

        public TransferItem SetVisible(bool value = false)
        {
            visible = value;
            return this;
        }

        public TransferItem SetEnabled(bool value = false)
        {
            enabled = value;
            return this;
        }

        public TransferItem SetSelected(bool value = true)
        {
            selected = value;
            return this;
        }

        public TransferItem SetTarget(bool value = true)
        {
            IsTarget = value;
            return this;
        }

        public TransferItem SetTag(object? value)
        {
            Tag = value;
            return this;
        }

        #endregion
    }
}