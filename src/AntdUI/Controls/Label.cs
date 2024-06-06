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
// GITEE: https://gitee.com/antdui/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// Label 标签
    /// </summary>
    /// <remarks>显示一段文本。</remarks>
    [Description("Label 文本")]
    [ToolboxItem(true)]
    [DefaultProperty("Text")]
    public class Label : IControl, ShadowConfig
    {
        #region 属性

        #region 系统

        /// <summary>
        /// 文字颜色
        /// </summary>
        [Description("文字颜色"), Category("外观"), DefaultValue(null)]
        public new Color? ForeColor
        {
            get => fore;
            set
            {
                if (fore == value) fore = value;
                fore = value;
                Invalidate();
            }
        }

        #endregion

        Color? fore;
        /// <summary>
        /// 文字颜色
        /// </summary>
        [Description("文字颜色"), Category("外观"), DefaultValue(null)]
        [Obsolete("使用 ForeColor 属性替代"), Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color? Fore
        {
            get => fore;
            set
            {
                if (fore == value) fore = value;
                fore = value;
                Invalidate();
            }
        }

        #region 文本

        internal string? text = null;
        /// <summary>
        /// 文本
        /// </summary>
        [Editor(typeof(System.ComponentModel.Design.MultilineStringEditor), typeof(UITypeEditor))]
        [Description("文本"), Category("外观"), DefaultValue(null)]
        public new string? Text
        {
            get => text;
            set
            {
                if (text == value) return;
                text = value;
                if (BeforeAutoSize()) Invalidate();
            }
        }

        StringFormat stringCNoWrap = Helper.SF_NoWrap();
        StringFormat stringFormat = new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Near };
        ContentAlignment textAlign = ContentAlignment.MiddleLeft;
        /// <summary>
        /// 文本位置
        /// </summary>
        [Description("文本位置"), Category("外观"), DefaultValue(ContentAlignment.MiddleLeft)]
        public ContentAlignment TextAlign
        {
            get => textAlign;
            set
            {
                if (textAlign == value) return;
                textAlign = value;
                textAlign.SetAlignment(ref stringFormat);
                Invalidate();
            }
        }

        bool autoEllipsis = false;
        /// <summary>
        /// 文本超出自动处理
        /// </summary>
        [Description("文本超出自动处理"), Category("行为"), DefaultValue(false)]
        public bool AutoEllipsis
        {
            get => autoEllipsis;
            set
            {
                if (autoEllipsis == value) return;
                autoEllipsis = value;
                stringFormat.Trimming = value ? StringTrimming.EllipsisCharacter : StringTrimming.None;
            }
        }

        bool textMultiLine = true;
        /// <summary>
        /// 是否多行
        /// </summary>
        [Description("是否多行"), Category("行为"), DefaultValue(true)]
        public bool TextMultiLine
        {
            get { return textMultiLine; }
            set
            {
                if (textMultiLine == value) return;
                textMultiLine = value;
                stringFormat.FormatFlags = value ? 0 : StringFormatFlags.NoWrap;
                Invalidate();
            }
        }

        float iconratio = .7F;
        /// <summary>
        /// 图标比例
        /// </summary>
        [Description("图标比例"), Category("外观"), DefaultValue(.7F)]
        public float IconRatio
        {
            get => iconratio;
            set
            {
                if (iconratio == value) return;
                iconratio = value;
                IOnSizeChanged();
                Invalidate();
            }
        }

        string? prefix = null;
        [Description("前缀"), Category("外观"), DefaultValue(null)]
        public string? Prefix
        {
            get => prefix;
            set
            {
                if (prefix == value) return;
                prefix = value;
                IOnSizeChanged();
                Invalidate();
            }
        }

        string? prefixSvg = null;
        [Description("前缀SVG"), Category("外观"), DefaultValue(null)]
        public string? PrefixSvg
        {
            get => prefixSvg;
            set
            {
                if (prefixSvg == value) return;
                prefixSvg = value;
                IOnSizeChanged();
                Invalidate();
            }
        }

        [Description("前缀颜色"), Category("外观"), DefaultValue(null)]
        public Color? PrefixColor { get; set; }

        /// <summary>
        /// 是否包含前缀
        /// </summary>
        public bool HasPrefix
        {
            get => prefixSvg != null || prefix != null;
        }

        string? suffix = null;
        /// <summary>
        /// 后缀
        /// </summary>
        [Description("后缀"), Category("外观"), DefaultValue(null)]
        public string? Suffix
        {
            get => suffix;
            set
            {
                if (suffix == value) return;
                suffix = value;
                IOnSizeChanged();
                Invalidate();
            }
        }

        string? suffixSvg = null;
        [Description("后缀SVG"), Category("外观"), DefaultValue(null)]
        public string? SuffixSvg
        {
            get => suffixSvg;
            set
            {
                if (suffixSvg == value) return;
                suffixSvg = value;
                IOnSizeChanged();
                Invalidate();
            }
        }

        [Description("后缀颜色"), Category("外观"), DefaultValue(null)]
        public Color? SuffixColor { get; set; }

        [Description("缀标完全展示"), Category("外观"), DefaultValue(true)]
        public bool Highlight { get; set; } = true;

        /// <summary>
        /// 是否包含后缀
        /// </summary>
        public bool HasSuffix
        {
            get => suffixSvg != null || suffix != null;
        }

        #endregion

        #region 阴影

        int shadow = 0;
        [Description("阴影大小"), Category("阴影"), DefaultValue(0)]
        public int Shadow
        {
            get => shadow;
            set
            {
                if (shadow == value) return;
                shadow = value;
                Invalidate();
            }
        }

        [Description("阴影颜色"), Category("阴影"), DefaultValue(null)]
        public Color? ShadowColor { get; set; }

        float shadowOpacity = 0.3F;
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
            }
        }

        int shadowOffsetX = 0;
        [Description("阴影偏移X"), Category("阴影"), DefaultValue(0)]
        public int ShadowOffsetX
        {
            get => shadowOffsetX;
            set
            {
                if (shadowOffsetX == value) return;
                shadowOffsetX = value;
                Invalidate();
            }
        }

        int shadowOffsetY = 0;
        [Description("阴影偏移Y"), Category("阴影"), DefaultValue(0)]
        public int ShadowOffsetY
        {
            get => shadowOffsetY;
            set
            {
                if (shadowOffsetY == value) return;
                shadowOffsetY = value;
                Invalidate();
            }
        }

        #endregion

        #endregion

        #region 渲染

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics.High();
            var rect_read = ReadRectangle;
            Color _fore = Style.Db.DefaultColor;
            if (fore.HasValue) _fore = fore.Value;
            PaintText(g, text, _fore, rect_read);
            if (shadow > 0)
            {
                using (var bmp = new Bitmap(Width, Height))
                {
                    using (var g2 = Graphics.FromImage(bmp))
                    {
                        PaintText(g2, text, ShadowColor ?? _fore, rect_read);
                    }
                    Helper.Blur(bmp, shadow);
                    g.DrawImage(bmp, new Rectangle(shadowOffsetX, shadowOffsetY, bmp.Width, bmp.Height), shadowOpacity);
                }
            }
            this.PaintBadge(g);
            base.OnPaint(e);
        }

        #region 渲染帮助

        void PaintText(Graphics g, string? text, Color color, Rectangle rect_read)
        {
            if (!string.IsNullOrEmpty(text))
            {
                Rectangle rec;
                bool has_prefixText = prefix != null, has_suffixText = suffix != null, has_prefix = HasPrefix, has_suffix = HasSuffix;
                if (has_prefixText || has_suffixText || has_prefix || has_suffix)
                {
                    var font_size = g.MeasureString(text, Font);
                    switch (textAlign)
                    {
                        case ContentAlignment.TopLeft:
                        case ContentAlignment.MiddleLeft:
                        case ContentAlignment.BottomLeft:
                            rec = PaintTextLeft(g, color, rect_read, font_size, has_prefixText, has_suffixText, has_prefix, has_suffix);
                            break;
                        case ContentAlignment.TopRight:
                        case ContentAlignment.MiddleRight:
                        case ContentAlignment.BottomRight:
                            rec = PaintTextRight(g, color, rect_read, font_size, has_prefixText, has_suffixText, has_prefix, has_suffix);
                            break;
                        default:
                            rec = PaintTextCenter(g, color, rect_read, font_size, has_prefixText, has_suffixText, has_prefix, has_suffix);
                            break;
                    }
                }
                else rec = rect_read;
                using (var brush = new SolidBrush(color))
                {
                    g.DrawString(text, Font, brush, rec, stringFormat);
                }
            }
        }

        Rectangle PaintTextLeft(Graphics g, Color color, Rectangle rect_read, SizeF font_size, bool has_prefixText, bool has_suffixText, bool has_prefix, bool has_suffix)
        {
            int hx = 0;
            if (has_prefixText)
            {
                var font_size_prefix = g.MeasureString(prefix, Font);
                float x = rect_read.X - font_size_prefix.Width, w = font_size_prefix.Width;
                var rect_l = RecFixAuto(x, w, rect_read, font_size);
                if (Highlight)
                {
                    hx = (int)Math.Ceiling(font_size_prefix.Width);
                    rect_l.X = 0;
                }
                using (var brush = new SolidBrush(PrefixColor ?? color))
                {
                    g.DrawString(prefix, Font, brush, rect_l, stringCNoWrap);
                }
            }
            else if (has_prefix)
            {
                int icon_size = (int)(font_size.Height * iconratio);
                float x = rect_read.X - icon_size, w = icon_size;
                var rect_l = RecFixAuto(x, w, rect_read, font_size);
                if (Highlight)
                {
                    hx = icon_size;
                    rect_l.X = 0;
                }
                using (var _bmp = SvgExtend.GetImgExtend(prefixSvg, rect_l, PrefixColor ?? color))
                {
                    if (_bmp != null) g.DrawImage(_bmp, rect_l);
                }
            }
            if (has_suffixText)
            {
                var font_size_suffix = g.MeasureString(suffix, Font);
                float x = rect_read.X + hx + font_size.Width, w = font_size_suffix.Width;
                using (var brush = new SolidBrush(SuffixColor ?? color))
                {
                    g.DrawString(suffix, Font, brush, RecFixAuto(x, w, rect_read, font_size), stringCNoWrap);
                }
            }
            else if (has_suffix)
            {
                int icon_size = (int)(font_size.Height * iconratio);
                float x = rect_read.X + hx + font_size.Width, w = icon_size;
                var rect_r = RecFixAuto(x, w, rect_read, font_size);
                using (var _bmp = SvgExtend.GetImgExtend(suffixSvg, rect_r, SuffixColor ?? color))
                {
                    if (_bmp != null) g.DrawImage(_bmp, rect_r);
                }
            }
            if (hx > 0) return new Rectangle(rect_read.X + hx, rect_read.Y, rect_read.Width - hx, rect_read.Height);
            return rect_read;
        }
        Rectangle PaintTextRight(Graphics g, Color color, Rectangle rect_read, SizeF font_size, bool has_prefixText, bool has_suffixText, bool has_prefix, bool has_suffix)
        {
            int hr = 0;
            if (has_suffixText)
            {
                var font_size_suffix = g.MeasureString(suffix, Font);
                float x = rect_read.Right, w = font_size_suffix.Width;
                var rect_l = RecFixAuto(x, w, rect_read, font_size);
                if (Highlight)
                {
                    hr = (int)Math.Ceiling(font_size_suffix.Width);
                    rect_l.X = rect_read.Right - hr;
                }
                using (var brush = new SolidBrush(SuffixColor ?? color))
                {
                    g.DrawString(suffix, Font, brush, rect_l, stringCNoWrap);
                }
            }
            else if (has_suffix)
            {
                int icon_size = (int)(font_size.Height * iconratio);
                float x = rect_read.Right, w = icon_size;
                var rect_r = RecFixAuto(x, w, rect_read, font_size);
                if (Highlight)
                {
                    hr = icon_size;
                    rect_r.X = rect_read.Right - icon_size;
                }
                using (var _bmp = SvgExtend.GetImgExtend(suffixSvg, rect_r, SuffixColor ?? color))
                {
                    if (_bmp != null) g.DrawImage(_bmp, rect_r);
                }
            }
            if (has_prefixText)
            {
                var font_size_prefix = g.MeasureString(prefix, Font);
                float x = rect_read.Right - hr - font_size.Width - font_size_prefix.Width, w = font_size_prefix.Width;
                var rect_l = RecFixAuto(x, w, rect_read, font_size);
                using (var brush = new SolidBrush(PrefixColor ?? color))
                {
                    g.DrawString(prefix, Font, brush, rect_l, stringCNoWrap);
                }
            }
            else if (has_prefix)
            {
                int icon_size = (int)(font_size.Height * iconratio);
                float x = rect_read.Right - hr - font_size.Width - icon_size, w = icon_size;
                var rect_l = RecFixAuto(x, w, rect_read, font_size);
                using (var _bmp = SvgExtend.GetImgExtend(prefixSvg, rect_l, PrefixColor ?? color))
                {
                    if (_bmp != null) g.DrawImage(_bmp, rect_l);
                }
            }
            if (hr > 0) return new Rectangle(rect_read.X, rect_read.Y, rect_read.Width - hr, rect_read.Height);
            return rect_read;
        }
        Rectangle PaintTextCenter(Graphics g, Color color, Rectangle rect_read, SizeF font_size, bool has_prefixText, bool has_suffixText, bool has_prefix, bool has_suffix)
        {
            float cex = rect_read.X + (rect_read.Width - font_size.Width) / 2F;
            if (has_prefixText)
            {
                var font_size_prefix = g.MeasureString(prefix, Font);
                var rect_l = RecFixAuto(cex - font_size_prefix.Width, font_size_prefix.Width, rect_read, font_size);
                using (var brush = new SolidBrush(PrefixColor ?? color))
                {
                    g.DrawString(prefix, Font, brush, rect_l, stringCNoWrap);
                }
            }
            else if (has_prefix)
            {
                int icon_size = (int)(font_size.Height * iconratio);
                var rect_l = RecFixAuto(cex - icon_size, icon_size, rect_read, font_size);
                using (var _bmp = SvgExtend.GetImgExtend(prefixSvg, rect_l, PrefixColor ?? color))
                {
                    if (_bmp != null) g.DrawImage(_bmp, rect_l);
                }
            }
            if (has_suffixText)
            {
                var font_size_suffix = g.MeasureString(suffix, Font);
                using (var brush = new SolidBrush(SuffixColor ?? color))
                {
                    g.DrawString(suffix, Font, brush, RecFixAuto(cex + font_size.Width, font_size_suffix.Width, rect_read, font_size), stringCNoWrap);
                }
            }
            else if (has_suffix)
            {
                int icon_size = (int)(font_size.Height * iconratio);
                var rect_r = RecFixAuto(cex + font_size.Width, icon_size, rect_read, font_size);
                using (var _bmp = SvgExtend.GetImgExtend(suffixSvg, rect_r, SuffixColor ?? color))
                {
                    if (_bmp != null) g.DrawImage(_bmp, rect_r);
                }
            }
            return rect_read;
        }

        RectangleF RecFixAuto(float x, float w, Rectangle rect_read, SizeF font_size)
        {
            switch (textAlign)
            {
                case ContentAlignment.TopLeft:
                case ContentAlignment.TopRight:
                case ContentAlignment.TopCenter:
                    return RecFixT(x, w, rect_read, font_size);
                case ContentAlignment.BottomLeft:
                case ContentAlignment.BottomCenter:
                case ContentAlignment.BottomRight:
                    return RecFixB(x, w, rect_read, font_size);
                default: return RecFix(x, w, rect_read);
            }
        }
        RectangleF RecFix(float x, float w, Rectangle rect_read)
        {
            return new RectangleF(x, rect_read.Y, w, rect_read.Height);
        }
        RectangleF RecFixT(float x, float w, Rectangle rect_read, SizeF font_size)
        {
            return new RectangleF(x, rect_read.Y, w, font_size.Height);
        }
        RectangleF RecFixB(float x, float w, Rectangle rect_read, SizeF font_size)
        {
            return new RectangleF(x, rect_read.Bottom - font_size.Height, w, font_size.Height);
        }

        #endregion

        public override Rectangle ReadRectangle
        {
            get => ClientRectangle.PaddingRect(Padding);
        }

        #endregion

        #region 自动大小

        /// <summary>
        /// 自动大小
        /// </summary>
        [Browsable(true)]
        [Description("自动大小"), Category("外观"), DefaultValue(false)]
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
        [Description("自动大小模式"), Category("外观"), DefaultValue(TAutoSize.None)]
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
            return PSize;
        }

        internal Size PSize
        {
            get
            {
                bool has_prefix = string.IsNullOrEmpty(prefix), has_suffix = string.IsNullOrEmpty(suffix);
                return Helper.GDI(g =>
                {
                    var font_size = g.MeasureString(text ?? Config.NullText, Font);
                    if (has_prefix && has_suffix) return font_size.Size();
                    float add = 0;
                    if (has_prefix)
                    {
                        var font_size_prefix = g.MeasureString(prefix, Font);
                        add += font_size_prefix.Width;
                    }
                    if (has_suffix)
                    {
                        var font_size_suffix = g.MeasureString(suffix, Font);
                        add += font_size_suffix.Width;
                    }
                    return new Size((int)Math.Ceiling(font_size.Width + add), (int)Math.Ceiling(font_size.Height));
                });
            }
        }

        protected override void OnResize(EventArgs e)
        {
            BeforeAutoSize();
            base.OnResize(e);
        }

        internal bool BeforeAutoSize()
        {
            if (autoSize == TAutoSize.None) return true;
            if (InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    BeforeAutoSize();
                }));
                return false;
            }
            switch (autoSize)
            {
                case TAutoSize.Width:
                    Width = PSize.Width;
                    break;
                case TAutoSize.Height:
                    Height = PSize.Height;
                    break;
                case TAutoSize.Auto:
                default:
                    Size = PSize;
                    break;
            }
            Invalidate();
            return false;
        }

        #endregion
    }
}