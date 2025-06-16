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
// GITEE: https://gitee.com/AntdUI/AntdUI
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
    /// Tag 标签
    /// </summary>
    /// <remarks>进行标记和分类的小标签。</remarks>
    [Description("Tag 标签")]
    [ToolboxItem(true)]
    [DefaultProperty("Text")]
    public class Tag : IControl, IEventListener
    {
        #region 属性

        /// <summary>
        /// 原装背景颜色
        /// </summary>
        [Description("原装背景颜色"), Category("外观"), DefaultValue(typeof(Color), "Transparent")]
        public Color OriginalBackColor
        {
            get => base.BackColor;
            set => base.BackColor = value;
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

        #region 背景

        Color? back;
        /// <summary>
        /// 背景颜色
        /// </summary>
        [Description("背景颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public new Color? BackColor
        {
            get => back;
            set
            {
                if (back == value) return;
                back = value;
                Invalidate();
                OnPropertyChanged(nameof(BackColor));
            }
        }

        Image? backImage;
        /// <summary>
        /// 背景图片
        /// </summary>
        [Description("背景图片"), Category("外观"), DefaultValue(null)]
        public new Image? BackgroundImage
        {
            get => backImage;
            set
            {
                if (backImage == value) return;
                backImage = value;
                Invalidate();
                OnPropertyChanged(nameof(BackgroundImage));
            }
        }

        TFit backFit = TFit.Fill;
        /// <summary>
        /// 背景图片布局
        /// </summary>
        [Description("背景图片布局"), Category("外观"), DefaultValue(TFit.Fill)]
        public new TFit BackgroundImageLayout
        {
            get => backFit;
            set
            {
                if (backFit == value) return;
                backFit = value;
                Invalidate();
                OnPropertyChanged(nameof(BackgroundImageLayout));
            }
        }

        #endregion

        #region 边框

        internal float borderWidth = 1;
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
                borderWidth = value;
                Invalidate();
                OnPropertyChanged(nameof(BorderWidth));
            }
        }

        #endregion

        int radius = 6;
        /// <summary>
        /// 圆角
        /// </summary>
        [Description("圆角"), Category("外观"), DefaultValue(6)]
        public int Radius
        {
            get => radius;
            set
            {
                if (radius == value) return;
                radius = value;
                if (BeforeAutoSize()) Invalidate();
                OnPropertyChanged(nameof(Radius));
            }
        }

        internal TTypeMini type = TTypeMini.Default;
        /// <summary>
        /// 类型
        /// </summary>
        [Description("类型"), Category("外观"), DefaultValue(TTypeMini.Default)]
        public TTypeMini Type
        {
            get => type;
            set
            {
                if (type == value) return;
                type = value;
                Invalidate();
                OnPropertyChanged(nameof(Type));
            }
        }

        bool closeIcon = false;
        /// <summary>
        /// 是否显示关闭图标
        /// </summary>
        [Description("是否显示关闭图标"), Category("行为"), DefaultValue(false)]
        public bool CloseIcon
        {
            get => closeIcon;
            set
            {
                if (closeIcon == value) return;
                closeIcon = value;
                if (BeforeAutoSize()) Invalidate();
                OnPropertyChanged(nameof(CloseIcon));
            }
        }

        #region 文本

        string? text;
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

        StringFormat stringFormat = Helper.SF_NoWrap();

        ContentAlignment textAlign = ContentAlignment.MiddleCenter;
        /// <summary>
        /// 文本位置
        /// </summary>
        [Description("文本位置"), Category("外观"), DefaultValue(ContentAlignment.MiddleCenter)]
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
                OnPropertyChanged(nameof(AutoEllipsis));
            }
        }

        bool textMultiLine = false;
        /// <summary>
        /// 是否多行
        /// </summary>
        [Description("是否多行"), Category("行为"), DefaultValue(false)]
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

        #endregion

        #region 图片

        Image? image;
        /// <summary>
        /// 图像
        /// </summary>
        [Description("图像"), Category("外观"), DefaultValue(null)]
        public Image? Image
        {
            get => image;
            set
            {
                if (image == value) return;
                image = value;
                if (BeforeAutoSize()) Invalidate();
                OnPropertyChanged(nameof(Image));
            }
        }

        string? imageSvg;
        [Description("图像SVG"), Category("外观"), DefaultValue(null)]
        public string? ImageSvg
        {
            get => imageSvg;
            set
            {
                if (imageSvg == value) return;
                imageSvg = value;
                if (BeforeAutoSize()) Invalidate();
                OnPropertyChanged(nameof(ImageSvg));
            }
        }

        /// <summary>
        /// 是否包含图片
        /// </summary>
        public bool HasImage => imageSvg != null || image != null;

        /// <summary>
        /// 图像大小
        /// </summary>
        [Description("图像大小"), Category("外观"), DefaultValue(typeof(Size), "0, 0")]
        public Size ImageSize { get; set; } = new Size(0, 0);

        #endregion

        #endregion

        #region 事件

        /// <summary>
        /// Close时发生
        /// </summary>
        [Description("Close时发生"), Category("行为")]
        public event RBoolEventHandler? CloseChanged;

        #endregion

        #region 渲染

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics.High();
            var rect_read = ReadRectangle;
            if (backImage != null) g.Image(rect_read, backImage, backFit, radius, false);
            float _radius = radius * Config.Dpi;
            Color _fore, _back, _bor;
            switch (type)
            {
                case TTypeMini.Default:
                    _back = Colour.TagDefaultBg.Get("Tag", ColorScheme);
                    _fore = Colour.TagDefaultColor.Get("Tag", ColorScheme);
                    _bor = Colour.DefaultBorder.Get("Tag", ColorScheme);
                    break;
                case TTypeMini.Error:
                    _back = Colour.ErrorBg.Get("Tag", ColorScheme);
                    _fore = Colour.Error.Get("Tag", ColorScheme);
                    _bor = Colour.ErrorBorder.Get("Tag", ColorScheme);
                    break;
                case TTypeMini.Success:
                    _back = Colour.SuccessBg.Get("Tag", ColorScheme);
                    _fore = Colour.Success.Get("Tag", ColorScheme);
                    _bor = Colour.SuccessBorder.Get("Tag", ColorScheme);
                    break;
                case TTypeMini.Info:
                    _back = Colour.InfoBg.Get("Tag", ColorScheme);
                    _fore = Colour.Info.Get("Tag", ColorScheme);
                    _bor = Colour.InfoBorder.Get("Tag", ColorScheme);
                    break;
                case TTypeMini.Warn:
                    _back = Colour.WarningBg.Get("Tag", ColorScheme);
                    _fore = Colour.Warning.Get("Tag", ColorScheme);
                    _bor = Colour.WarningBorder.Get("Tag", ColorScheme);
                    break;
                case TTypeMini.Primary:
                default:
                    _back = Colour.PrimaryBg.Get("Tag", ColorScheme);
                    _fore = Colour.Primary.Get("Tag", ColorScheme);
                    _bor = Colour.Primary.Get("Tag", ColorScheme);
                    break;
            }
            if (fore.HasValue) _fore = fore.Value;
            if (back.HasValue) _back = back.Value;
            if (backImage != null) g.Image(rect_read, backImage, backFit, _radius, false);
            using (var path = rect_read.RoundPath(_radius))
            {
                #region 绘制背景

                g.Fill(_back, path);

                if (borderWidth > 0) g.Draw(_bor, borderWidth * Config.Dpi, path);

                #endregion

                PaintText(g, Text, _fore, rect_read);
            }
            this.PaintBadge(g);
            base.OnPaint(e);
        }

        #region 渲染帮助

        internal void PaintText(Canvas g, string? text, Color color, Rectangle rect_read)
        {
            var font_size = g.MeasureText(text ?? Config.NullText, Font);
            if (text == null)
            {
                if (PaintImageNoText(g, color, font_size, rect_read))
                {
                    if (closeIcon)
                    {
                        int size = (int)(rect_read.Height * .4F);
                        var rect_arrow = new Rectangle(rect_read.X + (rect_read.Width - size) / 2, rect_read.Y + (rect_read.Height - size) / 2, size, size);
                        rect_close = rect_arrow;
                        if (hover_close.Animation) g.PaintIconClose(rect_arrow, Helper.ToColor(hover_close.Value + Colour.TextQuaternary.Get("Tag", ColorScheme).A, Colour.Text.Get("Tag", ColorScheme)));
                        else if (hover_close.Switch) g.PaintIconClose(rect_arrow, Colour.Text.Get("Tag", ColorScheme));
                        else g.PaintIconClose(rect_arrow, Colour.TextQuaternary.Get("Tag", ColorScheme));
                    }
                }
            }
            else
            {
                bool right = RightToLeft == RightToLeft.Yes;
                var rect = ReadRectangle.IconRect(font_size.Height, HasImage, closeIcon, right, false);
                rect_close = rect.r;
                if (closeIcon)
                {
                    if (hover_close.Animation) g.PaintIconClose(rect.r, Helper.ToColor(hover_close.Value + Colour.TextQuaternary.Get("Tag", ColorScheme).A, Colour.Text.Get("Tag", ColorScheme)), .8F);
                    else if (hover_close.Switch) g.PaintIconClose(rect.r, Colour.Text.Get("Tag", ColorScheme), .8F);
                    else g.PaintIconClose(rect.r, Colour.TextQuaternary.Get("Tag", ColorScheme), .8F);
                }
                PaintImage(g, color, rect.l);
                using (var brush = new SolidBrush(color))
                {
                    g.DrawText(text, Font, brush, rect.text, stringFormat);
                }
            }
        }

        /// <summary>
        /// 渲染图片（没有文字）
        /// </summary>
        /// <param name="g">GDI</param>
        /// <param name="color">颜色</param>
        /// <param name="font_size">字体大小</param>
        /// <param name="rect_read">客户区域</param>
        bool PaintImageNoText(Canvas g, Color? color, Size font_size, Rectangle rect_read)
        {
            if (imageSvg != null)
            {
                g.GetImgExtend(imageSvg, GetImageRectCenter(font_size, rect_read), color);
                return false;
            }
            else if (image != null)
            {
                g.Image(image, GetImageRectCenter(font_size, rect_read));
                return false;
            }
            return true;
        }

        /// <summary>
        /// 居中的图片绘制区域
        /// </summary>
        /// <param name="font_size">字体大小</param>
        /// <param name="rect_read">客户区域</param>
        Rectangle GetImageRectCenter(Size font_size, Rectangle rect_read)
        {
            if (ImageSize.Width > 0 && ImageSize.Height > 0)
            {
                int w = (int)(ImageSize.Width * Config.Dpi), h = (int)(ImageSize.Height * Config.Dpi);
                return new Rectangle(rect_read.X + (rect_read.Width - w) / 2, rect_read.Y + (rect_read.Height - h) / 2, w, h);
            }
            else
            {
                int w = (int)(font_size.Height * 0.8F);
                return new Rectangle(rect_read.X + (rect_read.Width - w) / 2, rect_read.Y + (rect_read.Height - w) / 2, w, w);
            }
        }

        /// <summary>
        /// 渲染图片
        /// </summary>
        /// <param name="g">GDI</param>
        /// <param name="color">颜色</param>
        /// <param name="rectl">图标区域</param>
        void PaintImage(Canvas g, Color? color, Rectangle rectl)
        {
            if (imageSvg != null) g.GetImgExtend(imageSvg, GetImageRect(rectl), color);
            else if (image != null) g.Image(image, GetImageRect(rectl));
        }

        /// <summary>
        /// 图片绘制区域
        /// </summary>
        /// <param name="rectl">图标区域</param>
        Rectangle GetImageRect(Rectangle rectl)
        {
            if (ImageSize.Width > 0 && ImageSize.Height > 0)
            {
                int w = (int)(ImageSize.Width * Config.Dpi), h = (int)(ImageSize.Height * Config.Dpi);
                return new Rectangle(rectl.X + (rectl.Width - w) / 2, rectl.Y + (rectl.Height - h) / 2, w, h);
            }
            else return rectl;
        }

        #endregion

        public override Rectangle ReadRectangle => ClientRectangle.PaddingRect(Padding, borderWidth / 2F * Config.Dpi);

        public override GraphicsPath RenderRegion
        {
            get
            {
                var rect_read = ReadRectangle;
                float _radius = radius * Config.Dpi;
                return rect_read.RoundPath(_radius);
            }
        }

        #endregion

        #region 鼠标

        ITaskOpacity hover_close;
        public Tag()
        {
            base.BackColor = Color.Transparent;
            hover_close = new ITaskOpacity(nameof(AntdUI.Tag), this);
        }

        RectangleF rect_close;
        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && closeIcon && rect_close.Contains(e.Location))
            {
                bool isclose = false;
                if (CloseChanged == null || CloseChanged(this, EventArgs.Empty)) isclose = true;
                if (isclose) Dispose();
                return;
            }
            base.OnMouseClick(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (closeIcon)
            {
                hover_close.MaxValue = Colour.Text.Get("Tag", ColorScheme).A - Colour.TextQuaternary.Get("Tag", ColorScheme).A;
                hover_close.Switch = rect_close.Contains(e.Location);
                SetCursor(hover_close.Switch);
            }
            else SetCursor(false);
            base.OnMouseMove(e);
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
                    var font_size = g.MeasureText(Text ?? Config.NullText, Font);
                    int count = 0;
                    if (HasImage) count++;
                    if (closeIcon) count++;
                    return new Size(font_size.Width + (int)(14F * Config.Dpi) + (font_size.Height * count), font_size.Height + (int)(8F * Config.Dpi));
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