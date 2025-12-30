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
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// HyperlinkLabel 超链接文本
    /// </summary>
    /// <remarks>超链接文本 <a></remarks>
    [Description("HyperlinkLabel 超链接文本")]
    [ToolboxItem(true)]
    [DefaultProperty("Text")]
    [DefaultEvent("LinkClicked")]
    [Designer(typeof(IControlDesigner))]
    public class HyperlinkLabel : IControl
    {
        #region 属性

        Color? fore;
        /// <summary>
        /// 文字颜色
        /// </summary>
        [Description("文字颜色"), Category("Appearance"), DefaultValue(null)]
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

        #region 文本

        string? text;
        /// <summary>
        /// 文本
        /// </summary>
        [Editor(typeof(System.ComponentModel.Design.MultilineStringEditor), typeof(UITypeEditor))]
        [Description("文本"), Category("Appearance"), DefaultValue(null)]
        public override string? Text
        {
#pragma warning disable CS8764, CS8766
            get => this.GetLangI(LocalizationText, text);
#pragma warning restore CS8764, CS8766
            set
            {
                if (text == value) return;
                text = value;
                if (BeforeAutoSize()) Invalidate();
                OnTextChanged(EventArgs.Empty);
                ParseText();
                OnPropertyChanged(nameof(Text));
            }
        }

        [Description("文本"), Category("国际化"), DefaultValue(null)]
        public string? LocalizationText { get; set; }

        #endregion

        #region 文本对齐

        ContentAlignment textAlign = ContentAlignment.MiddleLeft;
        /// <summary>
        /// 文本位置
        /// </summary>
        [Description("文本位置"), Category("Appearance"), DefaultValue(ContentAlignment.MiddleLeft)]
        public ContentAlignment TextAlign
        {
            get => textAlign;
            set
            {
                if (textAlign == value) return;
                textAlign = value;
                Invalidate();
                OnPropertyChanged(nameof(TextAlign));
            }
        }

        #endregion

        #region 阴影

        int shadow = 0;
        /// <summary>
        /// 阴影大小
        /// </summary>
        [Description("阴影大小"), Category("阴影"), DefaultValue(0)]
        public int Shadow
        {
            get => shadow;
            set
            {
                if (shadow == value) return;
                shadow = value;
                Invalidate();
                OnPropertyChanged(nameof(Shadow));
            }
        }

        /// <summary>
        /// 阴影颜色
        /// </summary>
        [Description("阴影颜色"), Category("阴影"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? ShadowColor { get; set; }

        float shadowOpacity = 0.3F;
        /// <summary>
        /// 阴影透明度
        /// </summary>
        [Description("阴影透明度"), Category("阴影"), DefaultValue(0.3F)]
        public float ShadowOpacity
        {
            get => shadowOpacity;
            set
            {
                if (shadowOpacity == value) return;
                if (value < 0) value = 0;
                else if (value > 1) value = 1;
                shadowOpacity = value;
                Invalidate();
                OnPropertyChanged(nameof(ShadowOpacity));
            }
        }

        int shadowOffsetX = 0;
        /// <summary>
        /// 阴影偏移X
        /// </summary>
        [Description("阴影偏移X"), Category("阴影"), DefaultValue(0)]
        public int ShadowOffsetX
        {
            get => shadowOffsetX;
            set
            {
                if (shadowOffsetX == value) return;
                shadowOffsetX = value;
                Invalidate();
                OnPropertyChanged(nameof(ShadowOffsetX));
            }
        }

        int shadowOffsetY = 0;
        /// <summary>
        /// 阴影偏移Y
        /// </summary>
        [Description("阴影偏移Y"), Category("阴影"), DefaultValue(0)]
        public int ShadowOffsetY
        {
            get => shadowOffsetY;
            set
            {
                if (shadowOffsetY == value) return;
                shadowOffsetY = value;
                Invalidate();
                OnPropertyChanged(nameof(ShadowOffsetY));
            }
        }

        #endregion

        LinkAppearance? normalStyle;
        /// <summary>
        /// 常规状态下链接的样式
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("常规状态下链接的样式"), Category("Appearance"), DefaultValue(null)]
        public LinkAppearance? NormalStyle
        {
            get
            {
                normalStyle ??= new LinkAppearance();
                return normalStyle;
            }
            set
            {
                if (normalStyle == value) return;
                normalStyle = value;
                Invalidate();
                OnPropertyChanged(nameof(NormalStyle));
            }
        }

        private bool ShouldSerializeNormalStyle() => normalStyle != null && !normalStyle.IsDefault();

        private void ResetNormalStyle() => normalStyle = null;

        LinkAppearance? hoverStyle;
        /// <summary>
        /// 鼠标悬停时链接的样式
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("鼠标悬停时链接的样式"), Category("Appearance"), DefaultValue(null)]
        public LinkAppearance? HoverStyle
        {
            get
            {
                hoverStyle ??= new LinkAppearance();
                return hoverStyle;
            }
            set
            {
                if (hoverStyle == value) return;
                hoverStyle = value;
                Invalidate();
                OnPropertyChanged(nameof(HoverStyle));
            }
        }

        private bool ShouldSerializeHoverStyle() => hoverStyle != null && !hoverStyle.IsDefault();

        private void ResetHoverStyle() => hoverStyle = null;

        Padding _linkPadding = new(2, 0, 2, 0);
        /// <summary>
        /// 链接与周围字符之间的距离
        /// </summary>
        [Description("链接与周围字符之间的距离"), Category("Appearance"), DefaultValue(typeof(Padding), "2, 0, 2, 0")]
        public Padding LinkPadding
        {
            get { return _linkPadding; }
            set
            {
                _linkPadding = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 自动调用默认浏览器打开超链接
        /// </summary>
        [Description("自动调用默认浏览器打开超链接"), Category("Behavior"), DefaultValue(typeof(bool), "False")]
        public bool LinkAutoNavigation { get; set; }

        #endregion

        #region 渲染

        protected override void OnDraw(DrawEventArgs e)
        {
            base.OnDraw(e);

            var g = e.Canvas;
            var rect = ReadRectangle;

            Color _fore = fore ?? Style.Get(Colour.Text, nameof(HyperlinkLabel));
            if (shadow > 0)
            {
                using (var bmp = new Bitmap(Width, Height))
                {
                    using (var g2 = Graphics.FromImage(bmp).HighLay())
                    {
                        PaintText(g2, Text, ShadowColor ?? _fore, rect);
                    }
                    Helper.Blur(bmp, shadow);
                    g.Image(bmp, new Rectangle(shadowOffsetX, shadowOffsetY, bmp.Width, bmp.Height), shadowOpacity);
                }
            }
            PaintText(g, Text, _fore, rect);
        }

        void PaintText(Canvas g, string? text, Color color, Rectangle rect)
        {
            // 计算文本的总尺寸以支持对齐
            var totalSize = CalculateTextSize(g, text);

            // 根据 TextAlign 计算起始位置
            int startX = rect.X, startY = rect.Y;

            switch (textAlign)
            {
                case ContentAlignment.TopLeft:
                    startX = rect.X;
                    startY = rect.Y;
                    break;
                case ContentAlignment.TopCenter:
                    startX = rect.X + (rect.Width - totalSize.Width) / 2;
                    startY = rect.Y;
                    break;
                case ContentAlignment.TopRight:
                    startX = rect.Right - totalSize.Width;
                    startY = rect.Y;
                    break;
                case ContentAlignment.MiddleLeft:
                    startX = rect.X;
                    startY = rect.Y + (rect.Height - totalSize.Height) / 2;
                    break;
                case ContentAlignment.MiddleCenter:
                    startX = rect.X + (rect.Width - totalSize.Width) / 2;
                    startY = rect.Y + (rect.Height - totalSize.Height) / 2;
                    break;
                case ContentAlignment.MiddleRight:
                    startX = rect.Right - totalSize.Width;
                    startY = rect.Y + (rect.Height - totalSize.Height) / 2;
                    break;
                case ContentAlignment.BottomLeft:
                    startX = rect.X;
                    startY = rect.Bottom - totalSize.Height;
                    break;
                case ContentAlignment.BottomCenter:
                    startX = rect.X + (rect.Width - totalSize.Width) / 2;
                    startY = rect.Bottom - totalSize.Height;
                    break;
                case ContentAlignment.BottomRight:
                    startX = rect.Right - totalSize.Width;
                    startY = rect.Bottom - totalSize.Height;
                    break;
            }

            int usex = startX, usey = startY;

            foreach (var part in _linkParts)
            {
                if (part.Href == null)
                {
                    var size = g.MeasureText(part.Text, Font);
                    if (usex + size.Width > rect.Right && usex > startX)
                    {
                        usex = startX;
                        usey += size.Height;
                    }
                    part.Bounds = new Rectangle(usex, usey, size.Width, size.Height);
                    g.DrawText(part.Text, Font, color, part.Bounds);
                    usex += size.Width;
                }
                else if (part.Hover) PaintText(g, rect, part, startX, ref usex, ref usey, Colour.PrimaryActive, hoverStyle);
                else PaintText(g, rect, part, startX, ref usex, ref usey, Colour.Primary, normalStyle);
            }
        }
        void PaintText(Canvas g, Rectangle rect, LinkPart part, int startX, ref int usex, ref int usey, Colour colour, LinkAppearance? style)
        {
            if (style == null)
            {
                var size = g.MeasureText(part.Text, Font).DeflateSize(LinkPadding);
                if (usex + size.Width > rect.Right && usex > startX)
                {
                    usex = startX;
                    usey += size.Height;
                }
                part.Bounds = new Rectangle(usex, usey, size.Width, size.Height);
                g.DrawText(part.Text, Font, Style.Get(colour, nameof(HyperlinkLabel)), part.Bounds);

                usex += size.Width;
            }
            else
            {
                using (var font = new Font(Font, style.LinkStyle & ~FontStyle.Underline))
                {
                    var size = g.MeasureText(part.Text, Font).DeflateSize(LinkPadding);
                    if (usex + size.Width > rect.Right && usex > startX)
                    {
                        usex = startX;
                        usey += size.Height;
                    }
                    part.Bounds = new Rectangle(usex, usey, size.Width, size.Height);
                    g.DrawText(part.Text, font, style.LinkColor ?? Style.Get(colour, nameof(HyperlinkLabel)), part.Bounds);

                    PaintText(g, style, Style.Get(colour, nameof(HyperlinkLabel)), part.Bounds);

                    usex += size.Width;
                }
            }
        }

        private Size CalculateTextSize(Canvas g, string? text)
        {
            if (string.IsNullOrEmpty(text) || _linkParts.Length == 0) return Size.Empty;

            int totalWidth = 0, maxHeight = 0;

            foreach (var part in _linkParts)
            {
                Size partSize;
                if (part.Href == null || normalStyle == null) partSize = g.MeasureText(part.Text, Font);
                else
                {
                    using (var font = new Font(Font, normalStyle.LinkStyle & ~FontStyle.Underline))
                    {
                        partSize = g.MeasureText(part.Text, font).DeflateSize(LinkPadding);
                    }
                }
                totalWidth += partSize.Width;
                maxHeight = Math.Max(maxHeight, partSize.Height);
            }
            return new Size(totalWidth, maxHeight);
        }
        void PaintText(Canvas g, LinkAppearance style, Color color, Rectangle rect)
        {
            if (style.UnderlineThickness > 0)
            {
                using var pen = new Pen(style.UnderlineColor ?? color, style.UnderlineThickness * Dpi);
                int y = rect.Bottom;
                g.DrawLine(pen, rect.Left + LinkPadding.Left, y, rect.Right - LinkPadding.Right, y);
            }
        }

        public override Rectangle ReadRectangle => ClientRectangle.PaddingRect(Padding);

        #endregion

        #region 事件

        public event LinkClickedEventHandler? LinkClicked;

        #endregion

        #region 自动大小

        /// <summary>
        /// 自动大小
        /// </summary>
        [Browsable(true)]
        [Description("自动大小"), Category("Appearance"), DefaultValue(false)]
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
        [Description("自动大小模式"), Category("Appearance"), DefaultValue(TAutoSize.None)]
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
            base.OnFontChanged(e);
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            if (autoSize == TAutoSize.None) return base.GetPreferredSize(proposedSize);
            else if (autoSize == TAutoSize.Width) return new Size(PSize.Width, base.GetPreferredSize(proposedSize).Height);
            else if (autoSize == TAutoSize.Height) return new Size(base.GetPreferredSize(proposedSize).Width, PSize.Height);
            return PSize;
        }

        public Size PSize
        {
            get
            {
                return Helper.GDI(g =>
                {
                    var font_size = g.MeasureText(_plainText ?? Config.NullText, Font);
                    if (string.IsNullOrWhiteSpace(Text)) font_size.Width = 0;

                    // 应用链接内边距和控件边距
                    int totalWidth = font_size.Width + (_linkParts.Length * LinkPadding.Horizontal) + Padding.Horizontal;
                    int totalHeight = font_size.Height + LinkPadding.Vertical + Padding.Vertical;

                    return new Size(totalWidth, totalHeight);
                });
            }
        }

        protected override void OnResize(EventArgs e)
        {
            BeforeAutoSize();
            base.OnResize(e);
        }

        bool BeforeAutoSize()
        {
            if (autoSize == TAutoSize.None) return true;
            if (InvokeRequired) return ITask.Invoke(this, BeforeAutoSize);
            var PS = PSize;
            switch (autoSize)
            {
                case TAutoSize.Width:
                    if (Width == PS.Width) return true;
                    Width = PS.Width;
                    break;
                case TAutoSize.Height:
                    if (Height == PS.Height) return true;
                    Height = PS.Height;
                    break;
                case TAutoSize.Auto:
                default:
                    if (Width == PS.Width && Height == PS.Height) return true;
                    Size = PS;
                    break;
            }
            return false;
        }

        #endregion

        #region 私有方法

        LinkPart[] _linkParts = new LinkPart[0];
        string _plainText = string.Empty;
        private void ParseText()
        {
            if (text == null)
            {
                _linkParts = new LinkPart[0];
                return;
            }
            var linkParts = new List<LinkPart>();
            string pattern = @"<a\s+[^>]*href=[""']?([^""'\s>]+)[""']?[^>]*>([^<]+)</a>";

            var matches = Regex.Matches(text, pattern);
            int lastIndex = 0;
            var plainTextBuilder = new System.Text.StringBuilder();

            foreach (Match match in matches)
            {
                // 添加普通文本
                if (match.Index > lastIndex)
                {
                    string plainText = text.Substring(lastIndex, match.Index - lastIndex);
                    linkParts.Add(new LinkPart(plainText, null));
                    plainTextBuilder.Append(plainText);
                }

                // 添加链接
                string href = match.Groups[1].Value, linkText = match.Groups[2].Value;
                linkParts.Add(new LinkPart(linkText, href));
                plainTextBuilder.Append(linkText);

                lastIndex = match.Index + match.Length;
            }

            // 添加尾部普通文本
            if (lastIndex < text.Length)
            {
                string plainText = text.Substring(lastIndex);
                linkParts.Add(new LinkPart(plainText, null));
                plainTextBuilder.Append(plainText);
            }

            _plainText = plainTextBuilder.ToString(); // 保存纯文本
            _linkParts = linkParts.ToArray();
        }

        #endregion

        #region 事件重写

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            ParseText();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            int count = 0, hand = 0;
            foreach (var part in _linkParts)
            {
                if (part.Href == null) continue;
                if (part.Bounds.Contains(e.X, e.Y))
                {
                    hand++;
                    if (part.Hover) continue;
                    count++;
                    part.Hover = true;
                }
                else if (part.Hover)
                {
                    count++;
                    part.Hover = false;
                }
            }
            SetCursor(hand > 0);
            if (count > 0) Invalidate();
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            foreach (var part in _linkParts)
            {
                if (part.Href != null && part.Bounds.Contains(e.Location))
                {
                    OnLinkClicked(part.Href, part.Text);
                    return;
                }
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            int count = 0;
            foreach (var part in _linkParts)
            {
                if (part.Href == null) continue;
                if (part.Hover)
                {
                    count++;
                    part.Hover = false;
                }
            }
            if (count > 0) Invalidate();
        }

        protected virtual void OnLinkClicked(string href, string text)
        {
            if (LinkAutoNavigation && Uri.TryCreate(href, UriKind.Absolute, out _))
            {
                try
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = href,
                        UseShellExecute = true
                    });
                    return;
                }
                catch { }
            }
            LinkClicked?.Invoke(this, new LinkClickedEventArgs(href, text));
        }

        #endregion

        #region 类

        public class LinkClickedEventArgs : EventArgs
        {
            public string Href { get; private set; }
            public string Text { get; private set; }
            public LinkClickedEventArgs(string href, string text)
            {
                Href = href;
                Text = text;
            }
        }

        // 定义事件委托
        public delegate void LinkClickedEventHandler(object sender, LinkClickedEventArgs e);

        public class LinkPart
        {
            public LinkPart(string text, string? href)
            {
                Text = text;
                Href = href;
            }

            public string Text { get; set; }

            public string? Href { get; set; }

            public bool Hover { get; set; }

            // 用于点击检测
            public Rectangle Bounds { get; set; }
        }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class LinkAppearance
        {
            [Description("链接呈现的文本颜色"), DefaultValue(null)]
            public Color? LinkColor { get; set; }

            [Description("链接的字体样式"), DefaultValue(FontStyle.Regular)]
            public FontStyle LinkStyle { get; set; } = FontStyle.Regular;

            [Description("下划线颜色"), DefaultValue(null)]
            public Color? UnderlineColor { get; set; }

            [Description("下划线厚度（0为不显示下划线）"), DefaultValue(1)]
            public int UnderlineThickness { get; set; } = 1;

            internal bool IsDefault()
            {
                return LinkColor == null
                       && LinkStyle == FontStyle.Regular
                       && UnderlineColor == null
                       && UnderlineThickness == 1;
            }

            public override string ToString() => "LinkAppearance";
        }

        #endregion
    }
}