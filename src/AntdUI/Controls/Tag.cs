// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

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
        [Description("原装背景颜色"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(typeof(Color), "Transparent")]
        public Color OriginalBackColor
        {
            get => base.BackColor;
            set => base.BackColor = value;
        }

        Color? fore;
        /// <summary>
        /// 文字颜色
        /// </summary>
        [Description("文字颜色"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null)]
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
        [Description("背景颜色"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null)]
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
        [Description("背景图片"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null)]
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
        [Description("背景图片布局"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(TFit.Fill)]
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
        [Description("圆角"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(6)]
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
        [Description("类型"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(TTypeMini.Default)]
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

        TShape shape = TShape.Default;
        /// <summary>
        /// 形状
        /// </summary>
        [Description("形状"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(TShape.Default)]
        public TShape Shape
        {
            get => shape;
            set
            {
                if (shape == value) return;
                shape = value;
                if (BeforeAutoSize()) Invalidate();
                OnPropertyChanged(nameof(Shape));
            }
        }

        int? virtualWidth;
        /// <summary>
        /// 虚拟宽度
        /// </summary>
        [Description("虚拟宽度"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null)]
        public int? VirtualWidth
        {
            get => virtualWidth;
            set
            {
                if (virtualWidth == value) return;
                virtualWidth = value;
                Invalidate();
                OnPropertyChanged(nameof(VirtualWidth));
            }
        }

        bool textCenterHasIcon = false;
        /// <summary>
        /// 文本居中显示(包含图标后)
        /// </summary>
        [Description("文本居中显示(包含图标后)"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(false)]
        public bool TextCenterHasIcon
        {
            get => textCenterHasIcon;
            set
            {
                if (textCenterHasIcon == value) return;
                textCenterHasIcon = value;
                if (HasImage) Invalidate();
                OnPropertyChanged(nameof(TextCenterHasIcon));
            }
        }

        float iconratio = .7F;
        /// <summary>
        /// 图标比例
        /// </summary>
        [Description("图标比例"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(.7F)]
        public float IconRatio
        {
            get => iconratio;
            set
            {
                if (iconratio == value) return;
                iconratio = value;
                if (BeforeAutoSize()) Invalidate();
                OnPropertyChanged(nameof(IconRatio));
            }
        }

        float icongap = .25F;
        /// <summary>
        /// 图标与文字间距比例
        /// </summary>
        [Description("图标与文字间距比例"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(.25F)]
        public float IconGap
        {
            get => icongap;
            set
            {
                if (icongap == value) return;
                icongap = value;
                if (BeforeAutoSize()) Invalidate();
                OnPropertyChanged(nameof(IconGap));
            }
        }

        bool closeIcon = false;
        /// <summary>
        /// 是否显示关闭图标
        /// </summary>
        [Description("是否显示关闭图标"), Category(nameof(CategoryAttribute.Behavior)), DefaultValue(false)]
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
        [Description("文本"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null)]
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
                OnPropertyChanged(nameof(Text));
            }
        }

        [Description("文本"), Category("国际化"), DefaultValue(null)]
        public string? LocalizationText { get; set; }

        FormatFlags sf = FormatFlags.Center | FormatFlags.NoWrap;

        ContentAlignment textAlign = ContentAlignment.MiddleCenter;
        /// <summary>
        /// 文本位置
        /// </summary>
        [Description("文本位置"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(ContentAlignment.MiddleCenter)]
        public ContentAlignment TextAlign
        {
            get => textAlign;
            set
            {
                if (textAlign == value) return;
                textAlign = value;
                sf = textAlign.SetAlignment(sf);
                Invalidate();
                OnPropertyChanged(nameof(TextAlign));
            }
        }

        bool autoEllipsis = false;
        /// <summary>
        /// 文本超出自动处理
        /// </summary>
        [Description("文本超出自动处理"), Category(nameof(CategoryAttribute.Behavior)), DefaultValue(false)]
        public bool AutoEllipsis
        {
            get => autoEllipsis;
            set
            {
                if (autoEllipsis == value) return;
                autoEllipsis = value;
                if (value) sf |= FormatFlags.EllipsisCharacter;
                else sf &= ~FormatFlags.EllipsisCharacter;
                OnPropertyChanged(nameof(AutoEllipsis));
            }
        }

        bool textMultiLine = false;
        /// <summary>
        /// 是否多行
        /// </summary>
        [Description("是否多行"), Category(nameof(CategoryAttribute.Behavior)), DefaultValue(false)]
        public bool TextMultiLine
        {
            get => textMultiLine;
            set
            {
                if (textMultiLine == value) return;
                textMultiLine = value;
                if (value) sf &= ~FormatFlags.NoWrap;
                else sf |= FormatFlags.NoWrap;
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
        [Description("图像"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null)]
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
        [Description("图像SVG"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null)]
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
        [Description("图像大小"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(typeof(Size), "0, 0")]
        public Size ImageSize { get; set; }

        #endregion

        #endregion

        #region 事件

        /// <summary>
        /// Close时发生
        /// </summary>
        [Description("Close时发生"), Category(nameof(CategoryAttribute.Behavior))]
        public event RBoolEventHandler? CloseChanged;

        protected virtual bool OnCloseChanged() => CloseChanged?.Invoke(this, EventArgs.Empty) ?? true;

        #endregion

        #region 渲染

        protected override void OnDraw(DrawEventArgs e)
        {
            var g = e.Canvas;
            var rect_read = ReadRectangle;
            if (rect_read.Width == 0 || rect_read.Height == 0) return;
            float _radius = GetRadius(rect_read);
            if (backImage != null) g.Image(rect_read, backImage, backFit, _radius, shape);
            Color _fore, _back, _bor;
            switch (type)
            {
                case TTypeMini.Default:
                    _back = Colour.TagDefaultBg.Get(ColorScheme, nameof(AntdUI.Tag), Name);
                    _fore = Colour.TagDefaultColor.Get(ColorScheme, nameof(AntdUI.Tag), Name);
                    _bor = Colour.DefaultBorder.Get(ColorScheme, nameof(AntdUI.Tag), Name);
                    break;
                case TTypeMini.Error:
                    _back = Colour.ErrorBg.Get(ColorScheme, nameof(AntdUI.Tag), Name);
                    _fore = Colour.Error.Get(ColorScheme, nameof(AntdUI.Tag), Name);
                    _bor = Colour.ErrorBorder.Get(ColorScheme, nameof(AntdUI.Tag), Name);
                    break;
                case TTypeMini.Success:
                    _back = Colour.SuccessBg.Get(ColorScheme, nameof(AntdUI.Tag), Name);
                    _fore = Colour.Success.Get(ColorScheme, nameof(AntdUI.Tag), Name);
                    _bor = Colour.SuccessBorder.Get(ColorScheme, nameof(AntdUI.Tag), Name);
                    break;
                case TTypeMini.Info:
                    _back = Colour.InfoBg.Get(ColorScheme, nameof(AntdUI.Tag), Name);
                    _fore = Colour.Info.Get(ColorScheme, nameof(AntdUI.Tag), Name);
                    _bor = Colour.InfoBorder.Get(ColorScheme, nameof(AntdUI.Tag), Name);
                    break;
                case TTypeMini.Warn:
                    _back = Colour.WarningBg.Get(ColorScheme, nameof(AntdUI.Tag), Name);
                    _fore = Colour.Warning.Get(ColorScheme, nameof(AntdUI.Tag), Name);
                    _bor = Colour.WarningBorder.Get(ColorScheme, nameof(AntdUI.Tag), Name);
                    break;
                case TTypeMini.Primary:
                default:
                    _back = Colour.PrimaryBg.Get(ColorScheme, nameof(AntdUI.Tag), Name);
                    _fore = Colour.Primary.Get(ColorScheme, nameof(AntdUI.Tag), Name);
                    _bor = Colour.Primary.Get(ColorScheme, nameof(AntdUI.Tag), Name);
                    break;
            }
            if (fore.HasValue) _fore = fore.Value;
            if (back.HasValue) _back = back.Value;
            using (var path = ((RectangleF)rect_read).RoundPath(_radius, shape))
            {
                #region 绘制背景

                g.Fill(_back, path);

                if (borderWidth > 0) g.Draw(_bor, borderWidth * Dpi, path);

                #endregion

                PaintText(g, Text, _fore, rect_read);
            }
            base.OnDraw(e);
        }

        #region 渲染帮助

        internal void PaintText(Canvas g, string? text, Color color, Rectangle rect_read)
        {
            var font_size = g.MeasureText(text ?? Config.NullText, Font);
            if (virtualWidth.HasValue) font_size.Width = virtualWidth.Value;
            if (text == null)
            {
                if (PaintImageNoText(g, color, font_size, rect_read))
                {
                    if (closeIcon)
                    {
                        int size = (int)(rect_read.Height * .4F);
                        var rect_arrow = new Rectangle(rect_read.X + (rect_read.Width - size) / 2, rect_read.Y + (rect_read.Height - size) / 2, size, size);
                        rect_close = rect_arrow;
                        if (hover_close.Animation) g.Svg(SvgDb.IcoErrorGhost, rect_arrow, Helper.ToColor(hover_close.Value + Colour.TextQuaternary.Get(ColorScheme, nameof(AntdUI.Tag), Name).A, Colour.Text.Get(ColorScheme, nameof(AntdUI.Tag), Name)));
                        else if (hover_close.Switch) g.Svg(SvgDb.IcoErrorGhost, rect_arrow, Colour.Text.Get(ColorScheme, nameof(AntdUI.Tag), Name));
                        else g.Svg(SvgDb.IcoErrorGhost, rect_arrow, Colour.TextQuaternary.Get(ColorScheme, nameof(AntdUI.Tag), Name));
                    }
                }
            }
            else
            {
                // 与 Button 对齐：按图标比例/间距计算文本、图标、关闭图标的区域
                int txt_height = g.MeasureText(Config.NullText, Font).Height;
                var iconPosition = RightToLeft == RightToLeft.Yes ? TAlignMini.Right : TAlignMini.Left;
                Rectangle rect_text, rectClose = Rectangle.Empty;
                if (HasImage && closeIcon)
                {
                    rect_text = Button.RectAlignLR(g, txt_height, textMultiLine, Font, iconPosition, iconratio, icongap, font_size, rect_read, out var rect_l, out var rect_r);
                    rectClose = rect_r;
                    PaintImage(g, color, rect_l);
                }
                else if (HasImage)
                {
                    rect_text = Button.RectAlignL(g, txt_height, textMultiLine, textCenterHasIcon, Font, iconPosition, iconratio, icongap, font_size, rect_read, out var rect_l);
                    PaintImage(g, color, rect_l);
                }
                else if (closeIcon)
                {
                    rect_text = Button.RectAlignR(g, txt_height, textMultiLine, Font, iconPosition, iconratio, icongap, font_size, rect_read, out var rect_r);
                    rectClose = rect_r;
                }
                else
                {
                    int sps = (int)(txt_height * .4F), sps2 = sps * 2;
                    rect_text = new Rectangle(rect_read.X + sps, rect_read.Y + sps, rect_read.Width - sps2, rect_read.Height - sps2);
                }
                rect_close = rectClose;
                PaintTextAlign(rect_read, ref rect_text);
                if (closeIcon) PaintClose(g, rectClose);
                using (var brush = new SolidBrush(color))
                {
                    g.DrawText(text, Font, brush, rect_text, sf);
                }
            }
        }

        void PaintClose(Canvas g, Rectangle rect)
        {
            if (hover_close.Animation) g.PaintIconCore(rect, SvgDb.IcoErrorGhost, Helper.ToColor(hover_close.Value + Colour.TextQuaternary.Get(ColorScheme, nameof(AntdUI.Tag), Name).A, Colour.Text.Get(ColorScheme, nameof(AntdUI.Tag), Name)), .8F);
            else if (hover_close.Switch) g.PaintIconCore(rect, SvgDb.IcoErrorGhost, Colour.Text.Get(ColorScheme, nameof(AntdUI.Tag), Name), .8F);
            else g.PaintIconCore(rect, SvgDb.IcoErrorGhost, Colour.TextQuaternary.Get(ColorScheme, nameof(AntdUI.Tag), Name), .8F);
        }

        void PaintTextAlign(Rectangle rect_read, ref Rectangle rect_text)
        {
            switch (textAlign)
            {
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.MiddleRight:
                    rect_text.Y = rect_read.Y;
                    rect_text.Height = rect_read.Height;
                    break;
                case ContentAlignment.TopLeft:
                case ContentAlignment.TopRight:
                case ContentAlignment.TopCenter:
                    rect_text.Height = rect_read.Height - rect_text.Y;
                    break;
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
                g.Svg(imageSvg, GetImageRectCenter(font_size, rect_read), color);
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
                int w = (int)(ImageSize.Width * Dpi), h = (int)(ImageSize.Height * Dpi);
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
            if (imageSvg != null) g.Svg(imageSvg, GetImageRect(rectl), color);
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
                int w = (int)(ImageSize.Width * Dpi), h = (int)(ImageSize.Height * Dpi);
                return new Rectangle(rectl.X + (rectl.Width - w) / 2, rectl.Y + (rectl.Height - h) / 2, w, h);
            }
            else return rectl;
        }

        #endregion

        public override Rectangle ReadRectangle => ClientRectangle.PaddingRect(Padding, Dpi, borderWidth / 2F).ReadRect(0, shape, TJoinMode.None, false, false);

        public override GraphicsPath RenderRegion
        {
            get
            {
                var rect_read = ReadRectangle;
                return ((RectangleF)rect_read).RoundPath(GetRadius(rect_read), shape);
            }
        }

        float GetRadius(Rectangle rect_read) => (shape == TShape.Round || shape == TShape.Circle) ? rect_read.Height : radius * Dpi;

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
            if (e.Button == MouseButtons.Left && closeIcon && rect_close.Contains(e.X, e.Y))
            {
                if (OnCloseChanged()) Dispose();
                return;
            }
            base.OnMouseClick(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (closeIcon)
            {
                hover_close.MaxValue = Colour.Text.Get(ColorScheme, nameof(AntdUI.Tag), Name).A - Colour.TextQuaternary.Get(ColorScheme, nameof(AntdUI.Tag), Name).A;
                hover_close.Switch = rect_close.Contains(e.X, e.Y);
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
            base.OnFontChanged(e);
        }

        protected override void OnPaddingChanged(EventArgs e)
        {
            BeforeAutoSize();
            Invalidate();
            base.OnPaddingChanged(e);
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
                    var font_size = g.MeasureText(Text ?? Config.NullText, Font);
                    if (virtualWidth.HasValue) font_size.Width = virtualWidth.Value;
                    int txt_height = g.MeasureText(Config.NullText, Font).Height;
                    int icon_size = (int)(txt_height * iconratio), sp = (int)(txt_height * icongap);
                    int count = 0;
                    if (HasImage) count++;
                    if (closeIcon) count++;
                    int width = font_size.Width + (int)(14F * Dpi) + (icon_size + sp) * count;
                    int height = Math.Max(font_size.Height, icon_size) + (int)(8F * Dpi);
                    if (shape == TShape.Circle)
                    {
                        int s = Math.Max(width, height);
                        width = height = s;
                    }
                    // 与 Button / Label 保持一致：自动大小时把 Padding 边距计算在内
                    return new Size(width, height).DeflateSize(Padding);
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
            return this.SetAutoSize(autoSize, PSize);
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