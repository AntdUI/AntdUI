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
    /// HyperlinkCheckbox 多选框
    /// </summary>
    /// <remarks>超链接多选框。</remarks>
    [Description("HyperlinkCheckbox 多选框")]
    [ToolboxItem(true)]
    [DefaultProperty("Checked")]
    [DefaultEvent("CheckedChanged")]
    public class HyperlinkCheckbox : Checkbox
    {
        #region 链接属性

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
            get => _linkPadding;
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

        #region 自动大小

        /// <summary>
        /// 需要修改父类的 PSize() 为 virtual
        /// </summary>
        public override Size PSize
        {
            get
            {
                return this.GDI(g =>
                {
                    var font_size = g.MeasureString(Config.NullText, Font);
                    int gap = (int)(font_size.Height * 1.02F);
                    var linkPadding = Helper.SetPadding(Dpi, _linkPadding);
                    if (string.IsNullOrWhiteSpace(Text)) return new Size(font_size.Height + gap, font_size.Height + gap);
                    var textSize = CalculateTextSize(g, linkPadding);
                    return new Size(textSize.Width + font_size.Height + gap, font_size.Height + gap);
                });
            }
        }

        #endregion

        #region 私有实现

        LinkPart[] _linkParts = new LinkPart[0];

        private void ParseText()
        {
            var text = base.Text;
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

            _linkParts = linkParts.ToArray();
        }

        Size CalculateTextSize(Canvas g, Rectangle rect, Padding linkPadding)
        {
            if (_linkParts.Length == 0) return Size.Empty;
            int usex = 0, line = 0, height = 0;
            bool bold = HasBoldFont();
            foreach (var part in _linkParts)
            {
                Size size;
                if (part.Href == null) size = g.MeasureText(part.Text, Font).DeflateSize(linkPadding);
                else
                {
                    if (part.Hover)
                    {
                        if (hoverStyle == null) size = g.MeasureText(part.Text, Font).DeflateSize(linkPadding);
                        else
                        {
                            var style = hoverStyle.LinkStyle & ~FontStyle.Underline;
                            if (bold) style |= FontStyle.Bold;
                            using (var font = new Font(Font, style))
                            {
                                size = g.MeasureText(part.Text, font).DeflateSize(linkPadding);
                            }
                        }
                    }
                    else
                    {
                        if (normalStyle == null) size = g.MeasureText(part.Text, Font).DeflateSize(linkPadding);
                        else
                        {
                            var style = normalStyle.LinkStyle & ~FontStyle.Underline;
                            if (bold) style |= FontStyle.Bold;
                            using (var font = new Font(Font, style))
                            {
                                size = g.MeasureText(part.Text, font).DeflateSize(linkPadding);
                            }
                        }
                    }
                }
                part.Width = size.Width;
                if (usex + size.Width > rect.Width)
                {
                    usex = 0;
                    line++;
                }
                usex += size.Width;
                height = Math.Max(height, size.Height);
            }
            foreach (var it in _linkParts) it.Height = height;
            return new Size(usex, height * (line + 1));
        }
        Size CalculateTextSize(Canvas g, Padding linkPadding)
        {
            if (_linkParts.Length == 0) return Size.Empty;
            int usex = 0, height = 0;
            bool bold = HasBoldFont();
            foreach (var part in _linkParts)
            {
                Size size;
                if (part.Href == null) size = g.MeasureText(part.Text, Font).DeflateSize(linkPadding);
                else
                {
                    if (part.Hover)
                    {
                        if (hoverStyle == null) size = g.MeasureText(part.Text, Font).DeflateSize(linkPadding);
                        else
                        {
                            var style = hoverStyle.LinkStyle & ~FontStyle.Underline;
                            if (bold) style |= FontStyle.Bold;
                            using (var font = new Font(Font, style))
                            {
                                size = g.MeasureText(part.Text, font).DeflateSize(linkPadding);
                            }
                        }
                    }
                    else
                    {
                        if (normalStyle == null) size = g.MeasureText(part.Text, Font).DeflateSize(linkPadding);
                        else
                        {
                            var style = normalStyle.LinkStyle & ~FontStyle.Underline;
                            if (bold) style |= FontStyle.Bold;
                            using (var font = new Font(Font, style))
                            {
                                size = g.MeasureText(part.Text, font).DeflateSize(linkPadding);
                            }
                        }
                    }
                }
                part.Width = size.Width;
                usex += size.Width;
                height = Math.Max(height, size.Height);
            }
            foreach (var it in _linkParts) it.Height = height;
            return new Size(usex, height);
        }
        bool HasBoldFont()
        {
            if (hoverStyle != null)
            {
                if (hoverStyle.LinkStyle.HasFlag(FontStyle.Bold)) return true;
            }
            if (normalStyle != null)
            {
                if (normalStyle.LinkStyle.HasFlag(FontStyle.Bold)) return true;
            }
            return false;
        }

        #endregion

        #region 重写绘制与事件

        public override string? Text
        {
#pragma warning disable CS8764, CS8766
            get => base.Text;
#pragma warning restore CS8764, CS8766
            set
            {
                if (base.Text == value) return;
                base.Text = value;
                ParseText();
                Invalidate();
                OnPropertyChanged(nameof(Text));
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            ParseText();
        }

        protected override void OnDraw(DrawEventArgs e)
        {
            //base.OnDraw(e);

            // Avoid complex rendering in design-time which can trigger re-entrancy or GDI issues.
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;

            if (_linkParts.Length == 0) return;

            var g = e.Canvas;
            var rect = ReadRectangle;

            // 预留文本高度以避免与复选框重叠
            var font_size = g.MeasureString(Config.NullText, Font);
            rect.IconRectL(font_size.Height, out var icon_rect, out var text_rect);
            bool right = RightToLeft == RightToLeft.Yes;
            if (right) text_rect.X = rect.Width - text_rect.X - text_rect.Width;
            PaintChecked(g, rect, Enabled, icon_rect, right);

            Color _fore = ForeColor ?? Colour.Text.Get(nameof(HyperlinkCheckbox), ColorScheme);
            var linkPadding = Helper.SetPadding(Dpi, _linkPadding);
            PaintText(g, _fore, text_rect, linkPadding);
        }

        void PaintText(Canvas g, Color color, Rectangle rect, Padding linkPadding)
        {
            // 计算文本的总尺寸以支持对齐
            var totalSize = CalculateTextSize(g, rect, linkPadding);

            // 根据 TextAlign 计算起始位置
            int startX = rect.X, startY = rect.Y;

            switch (TextAlign)
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
                if (usex + part.Width > rect.Right && usex > startX)
                {
                    usex = startX;
                    usey += part.Height;
                }
                part.Bounds = new Rectangle(usex, usey, part.Width, part.Height);
                usex += part.Width;
                if (part.Href == null) g.DrawText(part.Text, Font, color, part.Bounds);
                else
                {
                    if (part.Hover)
                    {
                        if (hoverStyle == null) g.DrawText(part.Text, Font, color, part.Bounds);
                        else
                        {
                            var colorlink = hoverStyle.LinkColor ?? Style.Get(Colour.PrimaryActive, nameof(HyperlinkLabel));
                            using (var font = new Font(Font, hoverStyle.LinkStyle))
                            {
                                g.DrawText(part.Text, font, colorlink, part.Bounds);
                            }
                            PaintText(g, hoverStyle, colorlink, part.Bounds, linkPadding);
                        }
                    }
                    else
                    {
                        if (normalStyle == null) g.DrawText(part.Text, Font, color, part.Bounds);
                        else
                        {
                            var colorlink = normalStyle.LinkColor ?? Style.Get(Colour.Primary, nameof(HyperlinkLabel));
                            using (var font = new Font(Font, normalStyle.LinkStyle & ~FontStyle.Underline))
                            {
                                g.DrawText(part.Text, font, colorlink, part.Bounds);
                            }
                            PaintText(g, normalStyle, colorlink, part.Bounds, linkPadding);
                        }
                    }
                }
            }
        }
        void PaintText(Canvas g, LinkAppearance style, Color color, Rectangle rect, Padding linkPadding)
        {
            if (style.UnderlineThickness > 0)
            {
                using var pen = new Pen(style.UnderlineColor ?? color, style.UnderlineThickness * Dpi);
                int y = rect.Bottom;
                g.DrawLine(pen, rect.Left + linkPadding.Left, y, rect.Right - linkPadding.Right, y);
            }
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
            foreach (var part in _linkParts)
            {
                if (part.Href != null && part.Bounds.Contains(e.Location))
                {
                    OnLinkClicked(part.Href, part.Text);
                    return;
                }
            }
            base.OnMouseClick(e);
        }

        bool suppressClick = false;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            // 如果在链接部分发生鼠标按下事件，则抑制随后的点击操作。
            suppressClick = false;
            foreach (var part in _linkParts)
            {
                if (part.Href != null && part.Bounds.Contains(e.Location))
                {
                    suppressClick = true;
                    break;
                }
            }
            base.OnMouseDown(e);
        }

        protected override void OnClick(EventArgs e)
        {
            if (suppressClick)
            {
                // 抑制点击 — 不切换选中状态
                suppressClick = false;
                return;
            }
            base.OnClick(e);
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

        #endregion

        #region 链接事件

        public event LinkClickedEventHandler? LinkClicked;

        protected virtual void OnLinkClicked(string href, string text)
        {
            if (LinkAutoNavigation && Uri.TryCreate(href, UriKind.Absolute, out _))
            {
                if (href.ProcessShellOpen()) return;
            }
            LinkClicked?.Invoke(this, new LinkClickedEventArgs(href, text));
        }

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
            public Rectangle Bounds { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
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