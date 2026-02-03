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
    /// Breadcrumb 面包屑
    /// </summary>
    /// <remarks>显示当前页面在系统层级结构中的位置，并能向上返回。</remarks>
    [Description("Breadcrumb 面包屑")]
    [ToolboxItem(true)]
    [DefaultProperty("Items")]
    [DefaultEvent("ItemClick")]
    public class Breadcrumb : IControl
    {
        #region 属性

        int gap = 12;
        [Description("间距"), Category("外观"), DefaultValue(12)]
        public int Gap
        {
            get => gap;
            set
            {
                if (gap == value) return;
                gap = value;
                ChangeItems(true);
                OnPropertyChanged(nameof(Gap));
            }
        }

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
        /// 激活文字颜色
        /// </summary>
        [Description("激活文字颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? ForeActive { get; set; }

        int radius = 4;
        /// <summary>
        /// 圆角
        /// </summary>
        [Description("圆角"), Category("外观"), DefaultValue(4)]
        public int Radius
        {
            get => radius;
            set
            {
                if (radius == value) return;
                radius = value;
                Invalidate();
                OnPropertyChanged(nameof(Radius));
            }
        }

        BreadcrumbItemCollection? items;
        /// <summary>
        /// 获取列表中所有列表项的集合
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("集合"), Category("数据"), DefaultValue(null)]
        public BreadcrumbItemCollection Items
        {
            get
            {
                items ??= new BreadcrumbItemCollection(this);
                return items;
            }
            set => items = value.BindData(this);
        }

        /// <summary>
        /// 点击项时发生
        /// </summary>
        [Description("点击项时发生"), Category("行为")]
        public event BreadcrumbItemEventHandler? ItemClick;

        protected virtual void OnItemClick(BreadcrumbItem item, MouseEventArgs e) => ItemClick?.Invoke(this, new BreadcrumbItemEventArgs(item, e));

        #region Change

        protected override void OnSizeChanged(EventArgs e)
        {
            ChangeItems();
            base.OnSizeChanged(e);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            ChangeItems();
        }

        protected override void OnMarginChanged(EventArgs e)
        {
            ChangeItems();
            base.OnMarginChanged(e);
        }

        protected override void OnPaddingChanged(EventArgs e)
        {
            ChangeItems();
            base.OnPaddingChanged(e);
        }

        protected override void OnFontChanged(EventArgs e)
        {
            ChangeItems();
            base.OnFontChanged(e);
        }

        #endregion

        #endregion

        #region 渲染

        readonly FormatFlags s_f = FormatFlags.Center | FormatFlags.NoWrapEllipsis;
        protected override void OnDraw(DrawEventArgs e)
        {
            if (items == null || items.Count == 0)
            {
                base.OnDraw(e);
                return;
            }
            var g = e.Canvas;
            float _radius = radius * Dpi;
            using (var brush = new SolidBrush(fore ?? Colour.TextSecondary.Get(nameof(Breadcrumb), ColorScheme)))
            using (var brush_active = new SolidBrush(ForeActive ?? Colour.Text.Get(nameof(Breadcrumb), ColorScheme)))
            {
                foreach (var it in hs) g.DrawText("/", Font, brush, it, s_f);
                for (int i = 0; i < items.Count; i++)
                {
                    var it = items[i];
                    if (it == null) continue;
                    if (i == items.Count - 1)
                    {
                        //最后一个
                        PaintImg(g, it, brush_active.Color, it.IconSvg, it.Icon);
                        g.DrawText(it.Text, Font, brush_active, it.RectText, s_f);
                    }
                    else
                    {
                        if (it.Hover)
                        {
                            using (var path = it.Rect.RoundPath(_radius))
                            {
                                g.Fill(Colour.FillSecondary.Get(nameof(Breadcrumb), ColorScheme), path);
                            }
                            PaintImg(g, it, brush_active.Color, it.IconSvg, it.Icon);
                            g.DrawText(it.Text, Font, brush_active, it.RectText, s_f);
                        }
                        else
                        {
                            PaintImg(g, it, brush.Color, it.IconSvg, it.Icon);
                            g.DrawText(it.Text, Font, brush, it.RectText, s_f);
                        }
                    }
                }
            }
            base.OnDraw(e);
        }

        bool PaintImg(Canvas g, BreadcrumbItem it, Color color, string? svg, Image? bmp)
        {
            int count = 0;
            if (bmp != null)
            {
                g.Image(bmp, it.RectImg);
                count++;
            }
            if (svg != null && g.GetImgExtend(svg, it.RectImg, color)) count++;
            return count == 0;
        }

        #endregion

        #region 布局

        bool pauseLayout = false;
        [Browsable(false), Description("暂停布局"), Category("行为"), DefaultValue(false)]
        public bool PauseLayout
        {
            get => pauseLayout;
            set
            {
                if (pauseLayout == value) return;
                pauseLayout = value;
                if (!value) ChangeItems(true);
                OnPropertyChanged(nameof(PauseLayout));
            }
        }

        Rectangle[] hs = new Rectangle[0];
        bool CanLayout()
        {
            if (IsHandleCreated)
            {
                var rect = ClientRectangle;
                if (pauseLayout || items == null || items.Count == 0 || rect.Width == 0 || rect.Height == 0) return false;
                return true;
            }
            return false;
        }
        internal void ChangeItems(bool print = false)
        {
            if (CanLayout())
            {
                var rect = ClientRectangle.PaddingRect(Padding).PaddingRect(Margin);
                hs = this.GDI(g =>
                {
                    var hs = new List<Rectangle>(items!.Count);
                    var size_t = g.MeasureString(Config.NullText, Font);
                    int sp = (int)(4 * Dpi), sp2 = sp * 2, imgsize = (int)(size_t.Height * .8F), h = size_t.Height + sp, y = rect.Y + (rect.Height - h) / 2, y_img = rect.Y + (rect.Height - imgsize) / 2, _gap = (int)(gap * Dpi);
                    int x = 0, tmpx = 0;
                    foreach (var it in items)
                    {
                        it.PARENT = this;

                        if (it.Text == null || string.IsNullOrEmpty(it.Text))
                        {
                            var Rect = new Rectangle(rect.X + x, y, imgsize + sp2, h);
                            if (it.HasIcon)
                            {
                                it.Rect = it.RectText = Rect;
                                it.RectImg = new Rectangle(Rect.X + sp, y_img, imgsize, imgsize);
                            }
                            else it.Rect = it.RectText = Rect;
                        }
                        else
                        {
                            var size = g.MeasureText(it.Text, Font);
                            if (it.HasIcon)
                            {
                                var Rect = new Rectangle(rect.X + x, y, imgsize + sp + size.Width + sp2, h);
                                it.Rect = Rect;
                                it.RectImg = new Rectangle(Rect.X + sp, y_img, imgsize, imgsize);
                                it.RectText = new Rectangle(it.RectImg.Right + sp, Rect.Y, size.Width, Rect.Height);
                            }
                            else it.Rect = it.RectText = new Rectangle(rect.X + x, y, size.Width + sp2, h);
                        }
                        x += it.Rect.Width + _gap;

                        if (tmpx > 0) hs.Add(new Rectangle(tmpx - _gap + sp, y, _gap, h));

                        tmpx = x;
                    }
                    return hs.ToArray();
                });
            }
            if (print) Invalidate();
        }

        #endregion

        #region 鼠标

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (items == null || items.Count == 0) return;
            int hand = 0, change = 0;
            foreach (BreadcrumbItem it in items)
            {
                bool hover = it.Rect.Contains(e.X, e.Y);
                if (it.Hover != hover)
                {
                    it.Hover = hover;
                    change++;
                }
                if (it.Hover) hand++;
            }
            SetCursor(hand > 0);
            if (change > 0) Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            SetCursor(false);
            if (items == null || items.Count == 0) return;
            int change = 0;
            foreach (BreadcrumbItem it in items)
            {
                if (it.Hover)
                {
                    it.Hover = false;
                    change++;
                }
            }
            if (change > 0) Invalidate();
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            SetCursor(false);
            if (items == null || items.Count == 0) return;
            int change = 0;
            foreach (BreadcrumbItem it in items)
            {
                if (it.Hover)
                {
                    it.Hover = false;
                    change++;
                }
            }
            if (change > 0) Invalidate();
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (items == null || items.Count == 0) return;
            for (int i = 0; i < items.Count; i++)
            {
                var it = items[i];
                if (it != null && it.Rect.Contains(e.X, e.Y))
                {
                    OnItemClick(it, e);
                    return;
                }
            }
        }

        #endregion
    }

    public class BreadcrumbItemCollection : iCollection<BreadcrumbItem>
    {
        public BreadcrumbItemCollection(Breadcrumb it)
        {
            BindData(it);
        }

        internal BreadcrumbItemCollection BindData(Breadcrumb it)
        {
            action = render =>
            {
                if (render) it.ChangeItems(true);
                else it.Invalidate();
            };
            return this;
        }
    }

    public class BreadcrumbItem
    {
        /// <summary>
        /// ID
        /// </summary>
        [Description("ID"), Category("数据"), DefaultValue(null)]
        public string? ID { get; set; }

        Image? icon;
        /// <summary>
        /// 图标
        /// </summary>
        [Description("图标"), Category("外观"), DefaultValue(null)]
        public Image? Icon
        {
            get => icon;
            set
            {
                if (icon == value) return;
                icon = value;
                Invalidates();
            }
        }

        string? iconSvg;
        /// <summary>
        /// 图标SVG
        /// </summary>
        [Description("图标SVG"), Category("外观"), DefaultValue(null)]
        public string? IconSvg
        {
            get => iconSvg;
            set
            {
                if (iconSvg == value) return;
                iconSvg = value;
                Invalidates();
            }
        }

        /// <summary>
        /// 是否包含图标
        /// </summary>
        public bool HasIcon => IconSvg != null || Icon != null;

        string? text;
        /// <summary>
        /// 文本
        /// </summary>
        [Description("文本"), Category("外观"), DefaultValue(null)]
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

        /// <summary>
        /// 用户定义数据
        /// </summary>
        [Description("用户定义数据"), Category("数据"), DefaultValue(null)]
        public object? Tag { get; set; }

        #region 内部

        internal bool Hover { get; set; }

        internal Rectangle Rect { get; set; }
        internal Rectangle RectImg { get; set; }
        internal Rectangle RectText { get; set; }

        internal Breadcrumb? PARENT { get; set; }

        void Invalidates()
        {
            if (PARENT == null) return;
            PARENT.ChangeItems();
            PARENT.Invalidate();
        }

        #endregion

        #region 设置

        #region 图标

        public BreadcrumbItem SetIcon(Image? img)
        {
            icon = img;
            return this;
        }

        public BreadcrumbItem SetIcon(string? svg)
        {
            iconSvg = svg;
            return this;
        }

        #endregion

        public BreadcrumbItem SetID(string? value)
        {
            ID = value;
            return this;
        }

        public BreadcrumbItem SetText(string? value, string? localization = null)
        {
            text = value;
            LocalizationText = localization;
            return this;
        }

        public BreadcrumbItem SetTag(object? value)
        {
            Tag = value;
            return this;
        }

        #endregion

        public override string? ToString() => Text;
    }
}