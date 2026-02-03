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

        ///// <summary>
        ///// 是否显示搜索框
        ///// </summary>
        //[Description("是否显示搜索框"), Category("行为"), DefaultValue(false)]
        //public bool ShowSearch { get; set; } = false;

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
        /// 列表项高度
        /// </summary>
        [Description("列表项高度"), Category("外观"), DefaultValue(null)]
        public int? ItemHeight { get; set; }

        /// <summary>
        /// 列表项圆角
        /// </summary>
        [Description("列表项圆角"), Category("外观"), DefaultValue(4)]
        public int ItemRadius { get; set; } = 4;

        /// <summary>
        /// 列表框圆角
        /// </summary>
        [Description("列表框圆角"), Category("外观"), DefaultValue(6)]
        public int PanelRadius { get; set; } = 6;

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

        Rectangle rect_source, rect_source_com, rect_sourceTitle, rect_sourceCheckbox, rect_sourceCheckboxText, rect_toRight;
        Rectangle rect_target, rect_target_com, rect_targetTitle, rect_targetCheckbox, rect_targetCheckboxText, rect_toLeft;
        internal void LoadLayout(bool print = true)
        {
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
                    int useY = rect.Y, rh = g.MeasureString(Config.NullText, Font).Height, check_size = (int)(rh * 0.7), gap = (int)(rh * 0.26), gap2 = gap * 2;

                    if (ShowSelectAll)
                    {
                        int y = useY + gap, yi = y + (rh - check_size) / 2;
                        rect_sourceCheckbox = new Rectangle(rect_source.X + gap, yi, check_size, check_size);
                        rect_targetCheckbox = new Rectangle(rect_target.X + gap, yi, check_size, check_size);
                        int ux = gap2 + check_size;
                        rect_sourceCheckboxText = new Rectangle(rect_source.X + ux, y, rect_source.Width - ux - gap, rh);
                        rect_targetCheckboxText = new Rectangle(rect_target.X + ux, y, rect_target.Width - ux - gap, rh);

                        rect_sourceTitle = new Rectangle(rect_source.X + ux, y, rect_source.Width - ux - gap2, rh);
                        rect_targetTitle = new Rectangle(rect_target.X + ux, y, rect_target.Width - ux - gap2, rh);
                    }
                    else
                    {
                        rect_sourceTitle = new Rectangle(rect_source.X + gap, useY + gap, rect_source.Width - gap2, rh);
                        rect_targetTitle = new Rectangle(rect_target.X + gap, useY + gap, rect_target.Width - gap2, rh);
                    }
                    useY += rh + gap2;

                    int listHeight = rect_source.Height - useY - gap2, lh;
                    if (ItemHeight.HasValue) lh = (int)(ItemHeight.Value * Dpi);
                    else lh = rh + gap2;

                    rect_source_com = new Rectangle(rect_source.X + gap, useY, rect_source.Width - gap2, listHeight);
                    rect_target_com = new Rectangle(rect_target.X + gap, useY, rect_target.Width - gap2, listHeight);

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
                    int y1 = CalculateLayout(g, sourceItems, rect_source_com, lh, gap, gap2, check_size);
                    int y2 = CalculateLayout(g, targetItems, rect_target_com, lh, gap, gap2, check_size);

                    ScrollBarSource.SizeChange(rect_source_com);
                    ScrollBarTarget.SizeChange(rect_target_com);

                    ScrollBarSource.SetVrSize(y1);
                    ScrollBarTarget.SetVrSize(y2);
                });

                if (print) Invalidate();
            }
        }

        int CalculateLayout(Canvas g, List<TransferItem> list, Rectangle rect, int rh, int gap, int gap2, int check_size)
        {
            int usey = 0;
            foreach (var it in list)
            {
                it.PARENT = this;
                it.rect = new Rectangle(rect.X, rect.Y + usey, rect.Width, rh);
                it.rect_check = new Rectangle(rect.X + gap, rect.Y + usey + (rh - check_size) / 2, check_size, check_size);
                int ux = gap2 + check_size;
                it.rect_text = new Rectangle(rect.X + ux, rect.Y + usey, rect.Width - ux - gap, rh);
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
            PaintListPanel(g, rect_source, rect_source_com, rect_sourceTitle, rect_sourceCheckbox, SourceTitle ?? Localization.Get("Transfer.Source", "源列表"), ScrollBarSource, sourceSelectAll, false);

            // 绘制目标列表
            PaintListPanel(g, rect_target, rect_target_com, rect_targetTitle, rect_targetCheckbox, TargetTitle ?? Localization.Get("Transfer.Target", "目标列表"), ScrollBarTarget, targetSelectAll, true);

            // 绘制操作按钮
            PaintOperationButtons(g);
        }

        readonly FormatFlags sf = FormatFlags.Left | FormatFlags.VerticalCenter | FormatFlags.NoWrapEllipsis, sfL = FormatFlags.Left | FormatFlags.VerticalCenter, sfR = FormatFlags.Right | FormatFlags.VerticalCenter;
        private void PaintListPanel(Canvas g, Rectangle rect, Rectangle rect_com, Rectangle rect_title, Rectangle rect_checkbox, string title, ScrollBar scroll, bool selectAll, bool isTarget)
        {
            // 获取颜色
            Color borderColor = BorderColor ?? Colour.BorderColor.Get(nameof(Transfer), ColorScheme), foreColor = ForeColor ?? Colour.Text.Get(nameof(Transfer), ColorScheme), foreActive = ForeActive ?? Colour.Primary.Get(nameof(Transfer), ColorScheme);

            // 绘制面板边框和背景
            using (var path = rect.RoundPath(PanelRadius * Dpi))
            {
                using (var brush = new SolidBrush(Colour.FillQuaternary.Get(nameof(Transfer), ColorScheme)))
                {
                    g.Fill(brush, path);
                }
                using (var pen = new Pen(borderColor))
                {
                    g.Draw(pen, path);
                }
            }

            if (items != null)
            {
                int count = 0, selectedCount = 0;
                var state = g.Save();
                g.SetClip(rect_com);
                g.TranslateTransform(0, -scroll.ValueY);
                float radius = ItemRadius * Dpi;
                foreach (var it in items)
                {
                    if (it.Visible && it.IsTarget == isTarget)
                    {
                        count++;
                        if (it.selected) selectedCount++;
                        // 绘制项背景
                        using (var path = it.rect.RoundPath(radius))
                        {
                            if (it.selected)
                            {
                                using (var brush = new SolidBrush(BackActive ?? Colour.FillTertiary.Get(nameof(Transfer), ColorScheme)))
                                {
                                    g.Fill(brush, path);
                                }
                            }
                            else if (it.Hover)
                            {
                                using (var brush = new SolidBrush(BackHover ?? Colour.FillSecondary.Get(nameof(Transfer), ColorScheme)))
                                {
                                    g.Fill(brush, path);
                                }
                            }
                        }

                        // 绘制复选框
                        using (var path = it.rect_check.RoundPath(it.rect_check.Height * .2F))
                        {
                            if (it.selected)
                            {
                                g.Fill(Colour.Primary.Get(nameof(Transfer), ColorScheme), path);
                                g.DrawLines(Colour.BgBase.Get(nameof(Transfer), ColorScheme), 2.6F * Dpi, it.rect_check.CheckArrow());
                            }
                            else g.Draw(Colour.BorderColor.Get(nameof(Transfer), ColorScheme), 2F * Dpi, path);
                        }

                        // 绘制项文本
                        using (var brush = new SolidBrush(it.selected ? foreActive : foreColor))
                        {
                            g.String(it.Text, Font, brush, it.rect_text, sf);
                        }
                    }
                }
                g.Restore(state);

                // 绘制标题/数量
                string countText = $"{selectedCount}/{count}";
                using (var brush = new SolidBrush(foreColor))
                {
                    g.String(title, Font, brush, rect_title, sfL);
                    g.String(countText, Font, brush, rect_title, sfR);
                }

                // 绘制滚动条
                if (scroll.Show) scroll.Paint(g, ColorScheme);
            }

            #region 绘制搜索框

            // 绘制搜索框
            //if (ShowSearch)
            //{
            //    var searchRect = new Rectangle(rect.X + 10, contentTop, rect.Width - 20, 30);
            //    using (var path = new GraphicsPath())
            //    {
            //        path.AddRoundedRectangle(searchRect, 4);
            //        using (var brush = new SolidBrush(Style.Db.FillTertiary))
            //        {
            //            g.FillPath(brush, path);
            //        }
            //        using (var pen = new Pen(borderColor))
            //        {
            //            g.DrawPath(pen, path);
            //        }
            //    }

            //    // 绘制搜索图标
            //    var searchIconRect = new Rectangle(searchRect.X + 5, searchRect.Y + 5, 20, 20);
            //    g.DrawSvg(g,"search", searchIconRect, foreColor);

            //    // 绘制搜索文本
            //    var searchTextRect = new Rectangle(searchRect.X + 30, searchRect.Y, searchRect.Width - 35, searchRect.Height);
            //    string searchText = isSource ? sourceSearchText : targetSearchText;
            //    if (string.IsNullOrEmpty(searchText))
            //    {
            //        using (var brush = new SolidBrush(Color.FromArgb(150, foreColor.R, foreColor.G, foreColor.B)))
            //        using (var font = new Font(Font.FontFamily, 9))
            //        {
            //            g.DrawString("搜索", font, brush, searchTextRect, new StringFormat { LineAlignment = StringAlignment.Center });
            //        }
            //    }
            //    else
            //    {
            //        using (var brush = new SolidBrush(foreColor))
            //        using (var font = new Font(Font.FontFamily, 9))
            //        {
            //            g.DrawString(searchText, font, brush, searchTextRect, new StringFormat { LineAlignment = StringAlignment.Center });
            //        }
            //    }

            //    contentTop += 40;
            //}

            #endregion

            // 绘制全选复选框
            if (ShowSelectAll)
            {
                // 绘制复选框
                using (var path = rect_checkbox.RoundPath(rect_checkbox.Height * .2F))
                {
                    if (selectAll)
                    {
                        g.Fill(Colour.Primary.Get(nameof(Transfer), ColorScheme), path);
                        g.DrawLines(Colour.BgBase.Get(nameof(Transfer), ColorScheme), 2.6F * Dpi, rect_checkbox.CheckArrow());
                    }
                    else g.Draw(Colour.BorderColor.Get("FillTertiary", ColorScheme), 2F * Dpi, path);
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
                    if (it.Visible)
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
                            bool newState = sourceSelectAll = !sourceSelectAll;
                            int check_count = 0;
                            // 更新所有项的选中状态
                            foreach (var it in items)
                            {
                                if (it.Visible)
                                {
                                    if (it.IsTarget) continue;
                                    it.selected = newState;
                                    if (it.selected) check_count++;
                                }
                            }
                            hover_to_right.Enable = check_count > 0;
                            LoadLayout();
                            return;
                        }
                        if (rect_targetCheckbox.Contains(e.X, e.Y) || rect_targetCheckboxText.Contains(e.X, e.Y))
                        {
                            // 切换全选状态
                            bool newState = targetSelectAll = !targetSelectAll;
                            int check_count = 0;
                            // 更新所有项的选中状态
                            foreach (var it in items)
                            {
                                if (it.Visible && it.IsTarget)
                                {
                                    it.selected = newState;
                                    if (it.selected) check_count++;
                                }
                            }
                            hover_to_left.Enable = !OneWay && check_count > 0;
                            LoadLayout();
                            return;
                        }
                    }
                    // 处理源列表项点击
                    if (rect_source.Contains(e.X, e.Y))
                    {
                        int sy = ScrollBarSource.ValueY;
                        int count = 0, check_count = 0;
                        foreach (var it in items)
                        {
                            if (it.Visible)
                            {
                                if (it.IsTarget) continue;
                                if (it.rect.Contains(e.X, e.Y + sy))
                                {
                                    // 切换选中状态
                                    it.selected = !it.selected;
                                    count++;
                                    if (it.selected) check_count++;
                                }
                            }
                        }
                        if (count > 0)
                        {
                            // 更新全选状态
                            hover_to_right.Enable = sourceSelectAll = check_count > 0;
                            Invalidate();
                        }
                    }

                    // 处理目标列表项点击
                    if (rect_target.Contains(e.X, e.Y))
                    {
                        int sy = ScrollBarTarget.ValueY;
                        int count = 0, check_count = 0;
                        foreach (var it in items)
                        {
                            if (it.Visible && it.IsTarget)
                            {
                                if (it.rect.Contains(e.X, e.Y + sy))
                                {
                                    // 切换选中状态
                                    it.selected = !it.selected;
                                    count++;
                                    if (it.selected) check_count++;
                                }
                            }
                        }
                        if (count > 0)
                        {
                            targetSelectAll = check_count > 0;
                            hover_to_left.Enable = !OneWay && targetSelectAll;
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
        /// <param name="e">事件参数</param>
        protected virtual void OnTransferChanged(TransferEventArgs e) => TransferChanged?.Invoke(this, e);

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
        /// <param name="e">事件参数</param>
        protected virtual void OnSearch(SearchEventArgs e) => Search?.Invoke(this, e);

        #endregion

        #region 构造函数

        ScrollBar ScrollBarSource, ScrollBarTarget;
        public Transfer()
        {
            hover_to_right = new ITaskOpacity(nameof(Transfer), this);
            hover_to_left = new ITaskOpacity(nameof(Transfer), this);
            ScrollBarSource = new ScrollBar(this);
            ScrollBarTarget = new ScrollBar(this);
        }

        #endregion

        #region 私有变量

        private string sourceSearchText = "", targetSearchText = "";

        private bool sourceSelectAll = false, targetSelectAll = false;

        private ITaskOpacity hover_to_right, hover_to_left;

        #endregion

        #region 私有方法

        private void InitializeItems()
        {
            sourceSelectAll = targetSelectAll = false;
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
                        if (it.Visible && it.selected) check_target++;
                    }
                    else
                    {
                        it.Visible = es || it.Text?.IndexOf(sourceSearchText, StringComparison.OrdinalIgnoreCase) >= 0;
                        if (it.Visible && it.selected) check_source++;
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
            sourceSelectAll = targetSelectAll = false;
            int changed = 0;
            List<TransferItem> sourceItems = new List<TransferItem>(items.Count), targetItems = new List<TransferItem>(items.Count);
            foreach (var it in items)
            {
                if (it.Visible)
                {
                    if (it.IsTarget) targetItems.Add(it);
                    else if (it.selected)
                    {
                        targetItems.Add(it);
                        it.selected = false;
                        it.IsTarget = true;
                        changed++;
                    }
                    else sourceItems.Add(it);
                }
            }

            if (changed > 0)
            {
                ApplyFilter();
                OnTransferChanged(new TransferEventArgs(sourceItems, targetItems));
            }
        }

        private void MoveSelectedItemsToLeft()
        {
            if (items == null || OneWay) return;

            int changed = 0;
            List<TransferItem> sourceItems = new List<TransferItem>(items.Count), targetItems = new List<TransferItem>(items.Count);
            sourceSelectAll = targetSelectAll = false;
            foreach (var it in items)
            {
                if (it.Visible)
                {
                    if (it.IsTarget)
                    {
                        if (it.selected)
                        {
                            sourceItems.Add(it);
                            it.selected = false;
                            it.IsTarget = false;
                            changed++;
                        }
                        else targetItems.Add(it);
                    }
                    else sourceItems.Add(it);
                }
            }
            if (changed > 0)
            {
                ApplyFilter();
                OnTransferChanged(new TransferEventArgs(sourceItems, targetItems));
            }
        }

        #endregion

        #region 资源释放

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                hover_to_right.Dispose();
                hover_to_left.Dispose();
            }
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
                if (it.Visible)
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
                if (it.Visible && it.IsTarget) targetItems.Add(it);
            }
            return targetItems;
        }

        /// <summary>
        /// 设置源列表搜索文本
        /// </summary>
        /// <param name="text">搜索文本</param>
        public void SetSourceSearchText(string text)
        {
            sourceSearchText = text;
            OnSearch(new SearchEventArgs(text, true));
            ApplyFilter();
        }

        /// <summary>
        /// 设置目标列表搜索文本
        /// </summary>
        /// <param name="text">搜索文本</param>
        public void SetTargetSearchText(string text)
        {
            targetSearchText = text;
            OnSearch(new SearchEventArgs(text, false));
            ApplyFilter();
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
                Invalidate();
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
                Invalidate();
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
                Invalidate();
            }
        }

        #endregion

        /// <summary>
        /// 是否可见
        /// </summary>
        [Description("是否可见"), Category("行为"), DefaultValue(true)]
        public bool Visible { get; set; } = true;

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
        void Invalidate() => PARENT?.Invalidate();

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
            Visible = value;
            return this;
        }

        public TransferItem SetSelected(bool value = true)
        {
            selected = value;
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