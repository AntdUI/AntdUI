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
// GITCODE: https://gitcode.com/AntdUI/AntdUI
// GITEE: https://gitee.com/AntdUI/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace AntdUI.Controls
{
    [Designer(typeof(HyperlinkLabelDesigner))]
    public class HyperlinkLabel : System.Windows.Forms.Label
    {
        #region 私有属性
        private LinkAppearance _normalStyle = new();
        private LinkAppearance _hoverStyle = new()
        {
            LinkColor = Color.Red,
            UnderlineColor = Color.Red
        };
        private Padding _linkPadding = new(2, 0, 2, 2);
        private readonly List<LinkPart> _linkParts = new();
        private LinkPart? _hoveredLink = null;
        private string _originalText = string.Empty;
        private string _plainText = string.Empty;
        /// <summary>
        /// 下划线的高度
        /// </summary>
        private int _underlineHeight = 0;
        public event LinkClickedEventHandler LinkClicked;
        #endregion
        #region 公开属性
        [Category("Appearance")]
        [Description("常规状态下链接的样式")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public LinkAppearance NormalStyle
        {
            get { return _normalStyle; }
            set
            {
                _normalStyle = value;
                CalcUnderlineHeight();
                this.Invalidate();
            }
        }

        [Category("Appearance")]
        [Description("鼠标悬停时链接的样式")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public LinkAppearance HoverStyle
        {
            get { return _hoverStyle; }
            set
            {
                _hoverStyle = value;
                CalcUnderlineHeight();
                this.Invalidate();
            }
        }

        [Category("Appearance")]
        [Description("链接与周围字符之间的距离")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Padding LinkPadding
        {
            get { return _linkPadding; }
            set
            {
                _linkPadding = value;
                this.Invalidate();
            }
        }

        [Bindable(true)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public override string Text
        {
            get { return _originalText; }
            set
            {
                _originalText = value ?? string.Empty;
                base.Text = string.Empty;
                ParseText();
                this.Invalidate();
            }
        }

        #endregion
        #region 构造函数
        public HyperlinkLabel()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
            this.Cursor = Cursors.Default;
            this.AutoEllipsis = false;
            this.AutoSize = true;
        }
        #endregion

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            var font = this.Font;
            var foreColor = this.ForeColor;
            var point = new Point(0, 0);

            foreach (var part in _linkParts)
            {
                var currentStyle = part.Href != null && part == _hoveredLink ? HoverStyle : NormalStyle;
                var baseFontStyle = part.Href != null ? currentStyle.LinkStyle : font.Style;
                // 处理可能出现的双眼皮下划线
                var currentFont = new Font(font, baseFontStyle & ~FontStyle.Underline);
                var currentColor = part.Href != null ? currentStyle.LinkColor : foreColor;

                var size = e.Graphics.MeasureString(part.Text, currentFont).ToSize();
                var rect = part.Href != null ? new Rectangle(point, new Size(size.Width + LinkPadding.Horizontal, size.Height + LinkPadding.Vertical + _underlineHeight)) : new Rectangle(point, size);

                part.Bounds = rect;

                // 绘制文本
                Point drawPoint = part.Href != null ? new Point(rect.X + LinkPadding.Left, rect.Y + LinkPadding.Top) : rect.Location;
                TextRenderer.DrawText(g, part.Text, currentFont, drawPoint, currentColor);

                // 绘制下划线
                if (part.Href != null && currentStyle.UnderlineThickness > 0)
                {
                    using var pen = new Pen(currentStyle.UnderlineColor, currentStyle.UnderlineThickness);
                    int y = rect.Bottom;// + LinkPadding.Bottom;
                    g.DrawLine(pen, rect.Left + LinkPadding.Left + 3, y, rect.Right - LinkPadding.Right, y);
                }

                point.X += rect.Width;
            }
        }

        #region 私有方法
        private void ParseText()
        {
            _linkParts.Clear();
            string pattern = @"<a\s+href=(\w+)>([^<]+)</a>";
            string text = _originalText;

            var matches = Regex.Matches(text, pattern);
            int lastIndex = 0;
            System.Text.StringBuilder plainTextBuilder = new System.Text.StringBuilder();

            foreach (Match match in matches)
            {
                // 添加普通文本
                if (match.Index > lastIndex)
                {
                    string plainText = text.Substring(lastIndex, match.Index - lastIndex);
                    _linkParts.Add(new LinkPart { Text = plainText, Href = null });
                    plainTextBuilder.Append(plainText);
                }

                // 添加链接
                string href = match.Groups[1].Value;
                string linkText = match.Groups[2].Value;
                _linkParts.Add(new LinkPart { Text = linkText, Href = href });
                plainTextBuilder.Append(linkText);

                lastIndex = match.Index + match.Length;
            }

            // 添加尾部普通文本
            if (lastIndex < text.Length)
            {
                string plainText = text.Substring(lastIndex);
                _linkParts.Add(new LinkPart { Text = plainText, Href = null });
                plainTextBuilder.Append(plainText);
            }

            _plainText = plainTextBuilder.ToString(); // 保存纯文本
        }
        private void UpdateHoverState(Point mouseLocation)
        {
            LinkPart? hovered = null;
            foreach (var part in _linkParts)
            {
                if (part.Href != null && part.Bounds.Contains(mouseLocation))
                {
                    hovered = part;
                    break;
                }
            }

            if (_hoveredLink != hovered)
            {
                _hoveredLink = hovered;
                this.Cursor = _hoveredLink != null ? Cursors.Hand : Cursors.Default;
                this.Invalidate();
            }
        }
        private void ClearHoverState()
        {
            if (_hoveredLink != null)
            {
                _hoveredLink = null;
                this.Cursor = Cursors.Default;
                this.Invalidate();
            }
        }
        private void CalcUnderlineHeight()
        {
            if (NormalStyle == null && HoverStyle == null)
            {
                _underlineHeight = 0;
                return;
            }
            int normalThickness = NormalStyle?.UnderlineThickness ?? 0;
            int hoverThickness = HoverStyle?.UnderlineThickness ?? 0;
            // 取两者中的最大值作为下划线高度
            _underlineHeight = Math.Max(normalThickness, hoverThickness);
        }

        #endregion
        #region 事件重写
        // 计算AutoSize的尺寸
        public override Size GetPreferredSize(Size proposedSize)
        {
            if (!this.AutoSize)
                return base.GetPreferredSize(proposedSize);
            using var g = this.CreateGraphics();
            // 测量纯文本尺寸
            var textSize = TextRenderer.MeasureText(g, _plainText, this.Font);

            // 应用链接内边距和控件边距
            int totalWidth = textSize.Width + (_linkParts.Count * LinkPadding.Horizontal) + this.Padding.Horizontal;
            int totalHeight = textSize.Height + LinkPadding.Vertical + this.Padding.Vertical + _underlineHeight;

            return new Size(totalWidth, totalHeight);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            UpdateHoverState(e.Location);  // 复用悬停状态更新逻辑
        }
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            foreach (var part in _linkParts)
            {
                if (part.Href != null && part.Bounds.Contains(e.Location))
                {
                    OnLinkClicked(new LinkClickedEventArgs(part.Href));
                    return;
                }
            }
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            ClearHoverState();  // 清除悬停状态
        }
        protected virtual void OnLinkClicked(LinkClickedEventArgs e)
        {
            LinkClicked?.Invoke(this, e);
        }
        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            this.Invalidate();
        }
        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);
            this.Invalidate();
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.Invalidate();
        }

        #endregion
        #region 累
        public class HyperlinkLabelDesigner : ControlDesigner
        {
            public override void Initialize(IComponent component)
            {
                base.Initialize(component);
            }
        }
        public class LinkClickedEventArgs : EventArgs
        {
            public string Href { get; private set; }

            public LinkClickedEventArgs(string href)
            {
                Href = href;
            }
        }
        // 定义事件委托
        public delegate void LinkClickedEventHandler(object sender, LinkClickedEventArgs e);
        public class LinkPart
        {
            public string? Text { get; set; }
            public string? Href { get; set; }
            // 用于点击检测
            public Rectangle Bounds { get; set; }
        }
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class LinkAppearance
        {
            private Color _linkColor = Color.Blue;
            private FontStyle _linkStyle = FontStyle.Regular;
            private Color _underlineColor = Color.Blue;
            private int _underlineThickness = 1;

            [Description("链接呈现的文本颜色")]
            [DefaultValue(typeof(Color), "Blue")]
            public Color LinkColor
            {
                get { return _linkColor; }
                set { _linkColor = value; }
            }

            [Description("链接的字体样式")]
            [DefaultValue(FontStyle.Regular)]
            public FontStyle LinkStyle
            {
                get { return _linkStyle; }
                set { _linkStyle = value; }
            }

            [Description("下划线颜色")]
            [DefaultValue(typeof(Color), "Blue")]
            public Color UnderlineColor
            {
                get { return _underlineColor; }
                set { _underlineColor = value; }
            }

            [Description("下划线厚度（0为不显示下划线）")]
            [DefaultValue(1)]
            public int UnderlineThickness
            {
                get { return _underlineThickness; }
                set { _underlineThickness = value; }
            }

            public override string ToString()
            {
                return "LinkAppearance";
            }
        }
        #endregion
    }
}
