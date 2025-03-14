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
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// Label 文本
    /// </summary>
    /// <remarks>显示一段文本。</remarks>
    [Description("Label 文本")]
    [ToolboxItem(true)]
    [DefaultProperty("Text")]
    public class Label : IControl, ShadowConfig, IEventListener
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

        string? colorExtend = null;
        /// <summary>
        /// 文字渐变色
        /// </summary>
        [Description("文字渐变色"), Category("外观"), DefaultValue(null)]
        public string? ColorExtend
        {
            get => colorExtend;
            set
            {
                if (colorExtend == value) return;
                colorExtend = value;
                Invalidate();
                OnPropertyChanged(nameof(ColorExtend));
            }
        }

        #region 文本

        string? text = null;
        /// <summary>
        /// 文本
        /// </summary>
        [Editor(typeof(System.ComponentModel.Design.MultilineStringEditor), typeof(UITypeEditor))]
        [Description("文本"), Category("外观"), DefaultValue(null)]
        public override string? Text
        {
            get => this.GetLangI(LocalizationText, text);
            set
            {
                if (text == value) return;
                text = value;
                if (BeforeAutoSize()) Invalidate();
                OnTextChanged(EventArgs.Empty);
                OnPropertyChanged(nameof(Text));
            }
        }

        [Description("文本"), Category("国际化"), DefaultValue(null)]
        public string? LocalizationText { get; set; }

        StringFormat stringCNoWrap = new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center, FormatFlags = StringFormatFlags.NoWrap },
            stringFormat = new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Near };
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
                OnPropertyChanged(nameof(TextAlign));
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
                Invalidate();
                OnPropertyChanged(nameof(AutoEllipsis));
            }
        }

        bool textMultiLine = true;
        /// <summary>
        /// 是否多行
        /// </summary>
        [Description("是否多行"), Category("行为"), DefaultValue(true)]
        public bool TextMultiLine
        {
            get => textMultiLine;
            set
            {
                if (textMultiLine == value) return;
                textMultiLine = value;
                stringFormat.FormatFlags = value ? 0 : StringFormatFlags.NoWrap;
                Invalidate();
                OnPropertyChanged(nameof(TextMultiLine));
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
                OnPropertyChanged(nameof(IconRatio));
            }
        }

        string? prefix = null;
        /// <summary>
        /// 前缀
        /// </summary>
        [Description("前缀"), Category("外观"), DefaultValue(null)]
        [Localizable(true)]
        public string? Prefix
        {
            get => this.GetLangI(LocalizationPrefix, prefix);
            set
            {
                if (prefix == value) return;
                prefix = value;
                IOnSizeChanged();
                if (BeforeAutoSize()) Invalidate();
                OnPropertyChanged(nameof(Prefix));
            }
        }

        [Description("前缀"), Category("国际化"), DefaultValue(null)]
        public string? LocalizationPrefix { get; set; }

        string? prefixSvg = null;
        /// <summary>
        /// 前缀SVG
        /// </summary>
        [Description("前缀SVG"), Category("外观"), DefaultValue(null)]
        public string? PrefixSvg
        {
            get => prefixSvg;
            set
            {
                if (prefixSvg == value) return;
                prefixSvg = value;
                IOnSizeChanged();
                if (BeforeAutoSize()) Invalidate();
                OnPropertyChanged(nameof(PrefixSvg));
            }
        }

        /// <summary>
        /// 前缀颜色
        /// </summary>
        [Description("前缀颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? PrefixColor { get; set; }

        /// <summary>
        /// 是否包含前缀
        /// </summary>
        public bool HasPrefix => prefixSvg != null || Prefix != null;

        string? suffix = null;
        /// <summary>
        /// 后缀
        /// </summary>
        [Description("后缀"), Category("外观"), DefaultValue(null)]
        [Localizable(true)]
        public string? Suffix
        {
            get => this.GetLangI(LocalizationSuffix, suffix);
            set
            {
                if (suffix == value) return;
                suffix = value;
                IOnSizeChanged();
                if (BeforeAutoSize()) Invalidate();
                OnPropertyChanged(nameof(Suffix));
            }
        }

        [Description("后缀"), Category("国际化"), DefaultValue(null)]
        public string? LocalizationSuffix { get; set; }

        string? suffixSvg = null;
        /// <summary>
        /// 后缀SVG
        /// </summary>
        [Description("后缀SVG"), Category("外观"), DefaultValue(null)]
        public string? SuffixSvg
        {
            get => suffixSvg;
            set
            {
                if (suffixSvg == value) return;
                suffixSvg = value;
                IOnSizeChanged();
                if (BeforeAutoSize()) Invalidate();
                OnPropertyChanged(nameof(SuffixSvg));
            }
        }

        /// <summary>
        /// 后缀颜色
        /// </summary>
        [Description("后缀颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? SuffixColor { get; set; }

        /// <summary>
        /// 缀标完全展示
        /// </summary>
        [Description("缀标完全展示"), Category("外观"), DefaultValue(true)]
        public bool Highlight { get; set; } = true;

        /// <summary>
        /// 是否包含后缀
        /// </summary>
        public bool HasSuffix => suffixSvg != null || Suffix != null;

        /// <summary>
        /// 超出文字显示 Tooltip
        /// </summary>
        [Description("超出文字显示 Tooltip"), Category("外观"), DefaultValue(true)]
        public bool ShowTooltip { get; set; } = true;

        /// <summary>
        /// 超出文字提示配置
        /// </summary>
        [Browsable(false)]
        [Description("超出文字提示配置"), Category("行为"), DefaultValue(null)]
        public TooltipConfig? TooltipConfig { get; set; }

        TRotate rotate = TRotate.None;
        /// <summary>
        /// 旋转
        /// </summary>
        [Description("旋转"), Category("外观"), DefaultValue(TRotate.None)]
        public TRotate Rotate
        {
            get => rotate;
            set
            {
                if (rotate == value) return;
                rotate = value;
                IOnSizeChanged();
                if (BeforeAutoSize()) Invalidate();
            }
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
                OnPropertyChanged(nameof(Shadow));
            }
        }

        [Description("阴影颜色"), Category("阴影"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
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
                OnPropertyChanged(nameof(ShadowOpacity));
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
                OnPropertyChanged(nameof(ShadowOffsetX));
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
                OnPropertyChanged(nameof(ShadowOffsetY));
            }
        }

        #endregion

        #endregion

        #region 渲染

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics.High();
            var rect_read = ReadRectangle;
            if (rect_read.Width == 0 || rect_read.Height == 0) return;
            if (rotate == TRotate.Clockwise_90)
            {
                using (var rotationMatrix = new Matrix())
                {
                    rotationMatrix.RotateAt(90, new PointF(Width / 2, Height / 2));
                    e.Graphics.Transform = rotationMatrix;
                }
            }
            else if (rotate == TRotate.CounterClockwise_90)
            {
                using (var rotationMatrix = new Matrix())
                {
                    rotationMatrix.RotateAt(-90, new PointF(Width / 2, Height / 2));
                    e.Graphics.Transform = rotationMatrix;
                }
            }

            Color _fore = Colour.DefaultColor.Get("Label");
            if (fore.HasValue) _fore = fore.Value;
            PaintText(g, Text, _fore, rect_read);
            if (shadow > 0)
            {
                using (var bmp = new Bitmap(Width, Height))
                {
                    using (var g2 = Graphics.FromImage(bmp).HighLay())
                    {
                        PaintText(g2, Text, ShadowColor ?? _fore, rect_read);
                    }
                    Helper.Blur(bmp, shadow);
                    g.Image(bmp, new Rectangle(shadowOffsetX, shadowOffsetY, bmp.Width, bmp.Height), shadowOpacity);
                }
            }
            this.PaintBadge(g);
            base.OnPaint(e);
        }

        #region 渲染帮助

        bool ellipsis = false;
        void PaintText(Canvas g, string? text, Color color, Rectangle rect_read)
        {
            if (!string.IsNullOrEmpty(text))
            {
                Rectangle rec;
                var font_size = g.MeasureString(text, Font);
                bool has_prefixText = Prefix != null, has_suffixText = Suffix != null, has_prefix = prefixSvg != null, has_suffix = suffixSvg != null;
                if (has_prefixText || has_suffixText || has_prefix || has_suffix)
                {
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

                switch (rotate)
                {
                    case TRotate.Clockwise_90:
                    case TRotate.CounterClockwise_90:

                        if (autoEllipsis) ellipsis = rec.Height < font_size.Width;
                        else ellipsis = false;
                        int off = (rec.Width - rec.Height) / 2, tmp = rec.Width, tmpx = rec.X;
                        rec.X = rec.Y + off;
                        rec.Width = rec.Height;
                        rec.Height = tmp;
                        rec.Y = tmpx - off;
                        break;
                    default:
                        if (autoEllipsis) ellipsis = rec.Width < font_size.Width;
                        else ellipsis = false;
                        break;
                }

                using (var brush = colorExtend.BrushEx(rec, color))
                {
                    g.String(text, Font, brush, rec, stringFormat);
                }
            }
        }

        Rectangle PaintTextLeft(Canvas g, Color color, Rectangle rect_read, Size font_size, bool has_prefixText, bool has_suffixText, bool has_prefix, bool has_suffix)
        {
            int hx = 0;
            if (has_prefixText)
            {
                var prefix = Prefix;
                var font_size_prefix = g.MeasureString(prefix, Font);
                int x = rect_read.X - font_size_prefix.Width, w = font_size_prefix.Width;
                var rect_l = RecFixAuto(x, w, rect_read, font_size);
                if (Highlight)
                {
                    hx = font_size_prefix.Width;
                    rect_l.X = 0;
                }
                using (var brush = new SolidBrush(PrefixColor ?? color))
                {
                    g.String(prefix, Font, brush, rect_l, stringCNoWrap);
                }
            }
            else if (has_prefix)
            {
                int icon_size = (int)(font_size.Height * iconratio);
                int x = rect_read.X - icon_size, w = icon_size;
                var rect_l = RecFixAuto(x, w, rect_read, font_size);
                if (Highlight)
                {
                    hx = icon_size;
                    rect_l.X = 0;
                }
                g.GetImgExtend(prefixSvg, rect_l, PrefixColor ?? color);
            }
            if (has_suffixText)
            {
                var suffix = Suffix;
                var font_size_suffix = g.MeasureString(suffix, Font);
                int x = rect_read.X + hx + font_size.Width, w = font_size_suffix.Width;
                using (var brush = new SolidBrush(SuffixColor ?? color))
                {
                    g.String(suffix, Font, brush, RecFixAuto(x, w, rect_read, font_size), stringCNoWrap);
                }
            }
            else if (has_suffix)
            {
                int icon_size = (int)(font_size.Height * iconratio);
                int x = rect_read.X + hx + font_size.Width, w = icon_size;
                var rect_r = RecFixAuto(x, w, rect_read, font_size);
                using (var _bmp = SvgExtend.GetImgExtend(suffixSvg, rect_r, SuffixColor ?? color))
                {
                    if (_bmp != null) g.Image(_bmp, rect_r);
                }
            }
            if (hx > 0) return new Rectangle(rect_read.X + hx, rect_read.Y, rect_read.Width - hx, rect_read.Height);
            return rect_read;
        }
        Rectangle PaintTextRight(Canvas g, Color color, Rectangle rect_read, Size font_size, bool has_prefixText, bool has_suffixText, bool has_prefix, bool has_suffix)
        {
            int hr = 0;
            if (has_suffixText)
            {
                var suffix = Suffix;
                var font_size_suffix = g.MeasureString(suffix, Font);
                int x = rect_read.Right, w = font_size_suffix.Width;
                var rect_l = RecFixAuto(x, w, rect_read, font_size);
                if (Highlight)
                {
                    hr = font_size_suffix.Width;
                    rect_l.X = rect_read.Right - hr;
                }
                using (var brush = new SolidBrush(SuffixColor ?? color))
                {
                    g.String(suffix, Font, brush, rect_l, stringCNoWrap);
                }
            }
            else if (has_suffix)
            {
                int icon_size = (int)(font_size.Height * iconratio);
                int x = rect_read.Right, w = icon_size;
                var rect_r = RecFixAuto(x, w, rect_read, font_size);
                if (Highlight)
                {
                    hr = icon_size;
                    rect_r.X = rect_read.Right - icon_size;
                }
                g.GetImgExtend(suffixSvg, rect_r, SuffixColor ?? color);
            }
            if (has_prefixText)
            {
                var prefix = Prefix;
                var font_size_prefix = g.MeasureString(prefix, Font);
                int x = rect_read.Right - hr - font_size.Width - font_size_prefix.Width, w = font_size_prefix.Width;
                var rect_l = RecFixAuto(x, w, rect_read, font_size);
                using (var brush = new SolidBrush(PrefixColor ?? color))
                {
                    g.String(prefix, Font, brush, rect_l, stringCNoWrap);
                }
            }
            else if (has_prefix)
            {
                int icon_size = (int)(font_size.Height * iconratio);
                int x = rect_read.Right - hr - font_size.Width - icon_size, w = icon_size;
                var rect_l = RecFixAuto(x, w, rect_read, font_size);
                g.GetImgExtend(prefixSvg, rect_l, PrefixColor ?? color);
            }
            if (hr > 0) return new Rectangle(rect_read.X, rect_read.Y, rect_read.Width - hr, rect_read.Height);
            return rect_read;
        }
        Rectangle PaintTextCenter(Canvas g, Color color, Rectangle rect_read, Size font_size, bool has_prefixText, bool has_suffixText, bool has_prefix, bool has_suffix)
        {
            int cex = rect_read.X + (rect_read.Width - font_size.Width) / 2;
            if (has_prefixText)
            {
                var prefix = Prefix;
                var font_size_prefix = g.MeasureString(prefix, Font);
                var rect_l = RecFixAuto(cex - font_size_prefix.Width, font_size_prefix.Width, rect_read, font_size);
                using (var brush = new SolidBrush(PrefixColor ?? color))
                {
                    g.String(prefix, Font, brush, rect_l, stringCNoWrap);
                }
            }
            else if (has_prefix)
            {
                int icon_size = (int)(font_size.Height * iconratio);
                var rect_l = RecFixAuto(cex - icon_size, icon_size, rect_read, font_size);
                g.GetImgExtend(prefixSvg, rect_l, PrefixColor ?? color);
            }
            if (has_suffixText)
            {
                var suffix = Suffix;
                var font_size_suffix = g.MeasureString(suffix, Font);
                using (var brush = new SolidBrush(SuffixColor ?? color))
                {
                    g.String(suffix, Font, brush, RecFixAuto(cex + font_size.Width, font_size_suffix.Width, rect_read, font_size), stringCNoWrap);
                }
            }
            else if (has_suffix)
            {
                int icon_size = (int)(font_size.Height * iconratio);
                var rect_r = RecFixAuto(cex + font_size.Width, icon_size, rect_read, font_size);
                g.GetImgExtend(suffixSvg, rect_r, SuffixColor ?? color);
            }
            return rect_read;
        }

        Rectangle RecFixAuto(int x, int w, Rectangle rect_read, Size font_size)
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
        Rectangle RecFix(int x, int w, Rectangle rect_read)
        {
            return new Rectangle(x, rect_read.Y, w, rect_read.Height);
        }
        Rectangle RecFixT(int x, int w, Rectangle rect_read, Size font_size)
        {
            return new Rectangle(x, rect_read.Y, w, font_size.Height);
        }
        Rectangle RecFixB(int x, int w, Rectangle rect_read, Size font_size)
        {
            return new Rectangle(x, rect_read.Bottom - font_size.Height, w, font_size.Height);
        }

        #endregion

        TooltipForm? tooltipForm = null;
        protected override void OnMouseHover(EventArgs e)
        {
            tooltipForm?.Close();
            tooltipForm = null;
            if (ellipsis && ShowTooltip && Text != null)
            {
                if (tooltipForm == null)
                {
                    tooltipForm = new TooltipForm(this, Text, TooltipConfig ?? new TooltipConfig
                    {
                        Font = Font,
                        ArrowAlign = TAlign.Top,
                    });
                    tooltipForm.Show(this);
                }
            }
            base.OnMouseHover(e);
        }

        public override Rectangle ReadRectangle => ClientRectangle.PaddingRect(Padding);

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
            if (autoSize == TAutoSize.None) return base.GetPreferredSize(proposedSize);
            else if (autoSize == TAutoSize.Width) return new Size(PSize.Width, base.GetPreferredSize(proposedSize).Height);
            else if (autoSize == TAutoSize.Height) return new Size(base.GetPreferredSize(proposedSize).Width, PSize.Height);
            return PSize;
        }

        public Size PSize
        {
            get
            {
                bool has_prefixText = Prefix != null, has_suffixText = Suffix != null, has_prefix = prefixSvg != null, has_suffix = suffixSvg != null;
                return Helper.GDI(g =>
                {
                    var font_size = g.MeasureString(Text ?? Config.NullText, Font);
                    if (string.IsNullOrWhiteSpace(Text)) font_size.Width = 0;
                    if (has_prefixText || has_suffixText || has_prefix || has_suffix)
                    {
                        float add = 0;
                        if (has_prefix) add += font_size.Height;
                        else if (has_prefixText)
                        {
                            var font_size_prefix = g.MeasureString(Prefix, Font).Width;
                            add += font_size_prefix;
                        }
                        if (has_suffix) add += font_size.Height;
                        else if (has_suffixText)
                        {
                            var font_size_suffix = g.MeasureString(Suffix, Font).Width;
                            add += font_size_suffix;
                        }
                        return new Size((int)Math.Ceiling(font_size.Width + add), font_size.Height);
                    }
                    else return font_size;
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
            if (InvokeRequired) return ITask.Invoke(this, new Func<bool>(BeforeAutoSize));
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

        #region 语言变化

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            this.AddListener();
        }

        public void HandleEvent(EventType id, object? tag)
        {
            switch (id)
            {
                case EventType.LANG:
                    BeforeAutoSize();
                    break;
            }
        }

        #endregion
    }
}