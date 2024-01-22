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
    /// Tag 标签
    /// </summary>
    /// <remarks>进行标记和分类的小标签。</remarks>
    [Description("Tag 标签")]
    [ToolboxItem(true)]
    [DefaultProperty("Text")]
    public class Tag : IControl
    {
        #region 属性

        Color? fore;
        /// <summary>
        /// 文字颜色
        /// </summary>
        [Description("文字颜色"), Category("外观"), DefaultValue(null)]
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

        #region 背景

        Color? back;
        /// <summary>
        /// 背景颜色
        /// </summary>
        [Description("背景颜色"), Category("外观"), DefaultValue(null)]
        public Color? Back
        {
            get => back;
            set
            {
                if (back == value) return;
                back = value;
                Invalidate();
            }
        }

        Image? backImage = null;
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
            }
        }

        #endregion

        internal int radius = 6;
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

        StringFormat stringFormat = new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center, Trimming = StringTrimming.None, FormatFlags = StringFormatFlags.NoWrap };

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

        bool textMultiLine = false;
        /// <summary>
        /// 是否多行
        /// </summary>
        [Description("是否多行"), Category("行为"), DefaultValue(false)]
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

        #endregion

        #region 图片

        Image? image = null;
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
            }
        }

        string? imageSvg = null;
        [Description("图像SVG"), Category("外观"), DefaultValue(null)]
        public string? ImageSvg
        {
            get => imageSvg;
            set
            {
                if (imageSvg == value) return;
                imageSvg = value;
                if (BeforeAutoSize()) Invalidate();
            }
        }

        /// <summary>
        /// 是否包含图片
        /// </summary>
        public bool HasImage
        {
            get => imageSvg != null || image != null;
        }

        /// <summary>
        /// 图像大小
        /// </summary>
        [Description("图像大小"), Category("外观"), DefaultValue(typeof(Size), "0, 0")]
        public Size ImageSize { get; set; } = new Size(0, 0);

        #endregion

        #endregion

        #region 渲染

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics.High();

            var rect_read = ReadRectangle;

            if (backImage != null) g.PaintImg(rect_read, backImage, backFit, radius, false);

            float _radius = radius * Config.Dpi;

            Color _fore, _back, _bor;
            switch (type)
            {
                case TTypeMini.Default:
                    _back = Style.Db.TagDefaultBg;
                    _fore = Style.Db.TagDefaultColor;
                    _bor = Style.Db.DefaultBorder;
                    break;
                case TTypeMini.Error:
                    _back = Style.Db.ErrorBg;
                    _fore = Style.Db.Error;
                    _bor = Style.Db.ErrorBorder;
                    break;
                case TTypeMini.Success:
                    _back = Style.Db.SuccessBg;
                    _fore = Style.Db.Success;
                    _bor = Style.Db.SuccessBorder;
                    break;
                case TTypeMini.Info:
                    _back = Style.Db.InfoBg;
                    _fore = Style.Db.Info;
                    _bor = Style.Db.InfoBorder;
                    break;
                case TTypeMini.Warn:
                    _back = Style.Db.WarningBg;
                    _fore = Style.Db.Warning;
                    _bor = Style.Db.WarningBorder;
                    break;
                case TTypeMini.Primary:
                default:
                    _back = Style.Db.PrimaryBg;
                    _fore = Style.Db.Primary;
                    _bor = Style.Db.Primary;
                    break;
            }

            if (fore.HasValue) _fore = fore.Value;
            if (back.HasValue) _back = back.Value;

            using (var path = rect_read.RoundPath(_radius))
            {
                #region 绘制背景

                using (var brush = new SolidBrush(_back))
                {
                    g.FillPath(brush, path);
                }

                if (borderWidth > 0)
                {
                    float border = borderWidth * Config.Dpi;
                    using (var brush = new Pen(_bor, border))
                    {
                        g.DrawPath(brush, path);
                    }
                }

                #endregion

                PaintText(g, text, _fore, rect_read);
            }

            this.PaintBadge(g);
            base.OnPaint(e);
        }

        #region 渲染帮助

        internal void PaintText(Graphics g, string? text, Color color, RectangleF rect_read)
        {
            var font_size = g.MeasureString(text ?? Config.NullText, Font);
            if (text == null)
            {
                if (PaintImageNoText(g, color, font_size, rect_read))
                {
                    if (closeIcon)
                    {
                        float size = rect_read.Height * 0.6F;
                        var rect_arrow = new RectangleF(rect_read.X + (rect_read.Width - size) / 2F, rect_read.Y + (rect_read.Height - size) / 2F, size, size);
                        rect_close = rect_arrow;
                        if (hover_close.Animation)
                        {
                            g.PaintIconError(rect_arrow, Color.FromArgb(hover_close.Value + Style.Db.TextQuaternary.A, Style.Db.Text));
                        }
                        else if (hover_close.Switch)
                        {
                            g.PaintIconError(rect_arrow, Style.Db.Text);
                        }
                        else g.PaintIconError(rect_arrow, Style.Db.TextQuaternary);
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
                    if (hover_close.Animation)
                    {
                        g.PaintIconError(rect.r, Color.FromArgb(hover_close.Value + Style.Db.TextQuaternary.A, Style.Db.Text), 0.76F);
                    }
                    else if (hover_close.Switch)
                    {
                        g.PaintIconError(rect.r, Style.Db.Text, 0.76F);
                    }
                    else g.PaintIconError(rect.r, Style.Db.TextQuaternary, 0.76F);
                }
                PaintImage(g, color, rect.l);
                using (var brush = new SolidBrush(color))
                {
                    g.DrawString(text, Font, brush, rect.text, stringFormat);
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
        bool PaintImageNoText(Graphics g, Color? color, SizeF font_size, RectangleF rect_read)
        {
            if (imageSvg != null)
            {
                var rect = GetImageRectCenter(font_size, rect_read);
                using (var _bmp = SvgExtend.GetImgExtend(imageSvg, rect, color))
                {
                    if (_bmp != null) g.DrawImage(_bmp, rect);
                }
                return false;
            }
            else if (image != null)
            {
                g.DrawImage(image, GetImageRectCenter(font_size, rect_read));
                return false;
            }
            return true;
        }

        /// <summary>
        /// 居中的图片绘制区域
        /// </summary>
        /// <param name="font_size">字体大小</param>
        /// <param name="rect_read">客户区域</param>
        RectangleF GetImageRectCenter(SizeF font_size, RectangleF rect_read)
        {
            if (ImageSize.Width > 0 && ImageSize.Height > 0)
            {
                int w = (int)(ImageSize.Width * Config.Dpi), h = (int)(ImageSize.Height * Config.Dpi);
                return new RectangleF(rect_read.X + (rect_read.Width - w) / 2, rect_read.Y + (rect_read.Height - h) / 2, w, h);
            }
            else
            {
                var w = font_size.Height * 0.8F;
                return new RectangleF(rect_read.X + (rect_read.Width - w) / 2, rect_read.Y + (rect_read.Height - w) / 2, w, w);
            }
        }

        /// <summary>
        /// 渲染图片
        /// </summary>
        /// <param name="g">GDI</param>
        /// <param name="color">颜色</param>
        /// <param name="rectl">图标区域</param>
        void PaintImage(Graphics g, Color? color, Rectangle rectl)
        {
            if (imageSvg != null)
            {
                var rect = GetImageRect(rectl);
                using (var _bmp = SvgExtend.GetImgExtend(imageSvg, rect, color))
                {
                    if (_bmp != null) g.DrawImage(_bmp, rect);
                }
                return;
            }
            else if (image != null)
            {
                g.DrawImage(image, GetImageRect(rectl));
                return;
            }
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

        public override Rectangle ReadRectangle
        {
            get => ClientRectangle.PaddingRect(Padding).ReadRect((int)(borderWidth * Config.Dpi));
        }

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
        public Tag() { hover_close = new ITaskOpacity(this); }

        RectangleF rect_close;
        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && closeIcon && rect_close.Contains(e.Location))
            {
                if (Parent is Control control) control.Controls.Remove(this);
                return;
            }
            base.OnMouseClick(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (closeIcon)
            {
                hover_close.MaxValue = Style.Db.Text.A - Style.Db.TextQuaternary.A;
                hover_close.Switch = rect_close.Contains(e.Location);
                SetCursor(hover_close.Switch);
            }
            else SetCursor(false);
            base.OnMouseMove(e);
        }
        #endregion

        #region 自动大小

        TAutoSize autoSize = TAutoSize.None;
        [Browsable(true)]
        [Description("自动大小"), Category("外观"), DefaultValue(TAutoSize.None)]
        public new TAutoSize AutoSize
        {
            get => autoSize;
            set
            {
                if (autoSize == value) return;
                autoSize = value;
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
                using (var bmp = new Bitmap(1, 1))
                {
                    using (var g = Graphics.FromImage(bmp))
                    {
                        var font_size = g.MeasureString(text ?? Config.NullText, Font);
                        int count = 0;
                        if (HasImage) count++;
                        if (closeIcon) count++;
                        return new Size((int)Math.Ceiling(font_size.Width + (14 * Config.Dpi) + (font_size.Height * count)), (int)(font_size.Height + (8 * Config.Dpi)));
                    }
                }
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
                case TAutoSize.Auto:
                    Size = PSize;
                    break;
                case TAutoSize.Width:
                    Width = PSize.Width;
                    break;
                case TAutoSize.Height:
                    Height = PSize.Height;
                    break;
            }
            Invalidate();
            return false;
        }

        #endregion
    }
}