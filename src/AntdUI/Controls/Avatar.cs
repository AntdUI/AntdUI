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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Threading;

namespace AntdUI
{
    /// <summary>
    /// Avatar 头像
    /// </summary>
    /// <remarks>用来代表用户或事物，支持图片、图标或字符展示。</remarks>
    [Description("Avatar 头像")]
    [ToolboxItem(true)]
    [DefaultProperty("Image")]
    [Designer(typeof(IControlDesigner))]
    public class Avatar : IControl, ShadowConfig
    {
        public Avatar()
        {
            base.BackColor = Color.Transparent;
        }

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

        Color back = Color.Transparent;
        /// <summary>
        /// 背景颜色
        /// </summary>
        [Description("背景颜色"), Category("外观"), DefaultValue(typeof(Color), "Transparent")]
        public new Color BackColor
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

        string? text;
        /// <summary>
        /// 文本
        /// </summary>
        [Description("文本"), Category("外观"), DefaultValue(null)]
        public override string? Text
        {
#pragma warning disable CS8764, CS8766
            get => this.GetLangI(LocalizationText, text);
#pragma warning restore CS8764, CS8766
            set
            {
                if (text == value) return;
                if (value != null && value.Length > 1) value = value.Substring(0, 1);
                text = value;
                Invalidate();
                OnTextChanged(EventArgs.Empty);
                OnPropertyChanged(nameof(Text));
            }
        }

        [Description("文本"), Category("国际化"), DefaultValue(null)]
        public string? LocalizationText { get; set; }

        int radius = 0;
        /// <summary>
        /// 圆角
        /// </summary>
        [Description("圆角"), Category("外观"), DefaultValue(0)]
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

        bool round = false;
        /// <summary>
        /// 圆角样式
        /// </summary>
        [Description("圆角样式"), Category("外观"), DefaultValue(false)]
        public bool Round
        {
            get => round;
            set
            {
                if (round == value) return;
                round = value;
                Invalidate();
                OnPropertyChanged(nameof(Round));
            }
        }

        #region 图片

        Image? image;
        /// <summary>
        /// 图片
        /// </summary>
        [Description("图片"), Category("外观"), DefaultValue(null)]
        public Image? Image
        {
            get => image;
            set
            {
                if (image == value) return;
                image = value;
                if (IsHandleCreated) LoadGif();
                OnPropertyChanged(nameof(Image));
            }
        }

        string? imageSvg;
        /// <summary>
        /// 图片SVG
        /// </summary>
        [Description("图片SVG"), Category("外观"), DefaultValue(null)]
        public string? ImageSvg
        {
            get => imageSvg;
            set
            {
                if (imageSvg == value) return;
                imageSvg = value;
                Invalidate();
                OnPropertyChanged(nameof(ImageSvg));
            }
        }

        TFit imageFit = TFit.Cover;
        /// <summary>
        /// 图片布局
        /// </summary>
        [Description("图片布局"), Category("外观"), DefaultValue(TFit.Cover)]
        public TFit ImageFit
        {
            get => imageFit;
            set
            {
                if (imageFit == value) return;
                imageFit = value;
                Invalidate();
                OnPropertyChanged(nameof(ImageFit));
            }
        }

        public Avatar SetImage(Image? value, string? svg = null)
        {
            image = value;
            imageSvg = svg;
            Invalidate();
            if (image != null && IsHandleCreated) LoadGif();
            return this;
        }
        public Avatar SetImage(string? value, Image? img = null)
        {
            image = img;
            imageSvg = value;
            Invalidate();
            if (image != null && IsHandleCreated) LoadGif();
            return this;
        }

        #region GIF

        bool playGIF = true;
        /// <summary>
        /// 播放GIF
        /// </summary>
        [Description("播放GIF"), Category("行为"), DefaultValue(true)]
        public bool PlayGIF
        {
            get => playGIF;
            set
            {
                if (playGIF == value) return;
                playGIF = value;
                if (IsHandleCreated) LoadGif();
            }
        }
        void LoadGif()
        {
            if (image == null) return;
            if (playGIF)
            {
                var fd = new FrameDimension(image.FrameDimensionsList[0]);
                int count = image.GetFrameCount(fd);
                if (count > 1) PlayGif(image, fd, count);
                else Invalidate();
            }
        }

        void PlayGif(Image value, FrameDimension fd, int count)
        {
            ITask.Run(() =>
            {
                int[] delays = GifDelays(value, count);
                while (PlayGIF && image == value)
                {
                    for (int i = 0; i < count; i++)
                    {
                        if (PlayGIF && image == value)
                        {
                            lock (_lock) { value.SelectActiveFrame(fd, i); }
                            Invalidate();
                            Thread.Sleep(Math.Max(delays[i], 10));
                        }
                        else
                        {
                            value.SelectActiveFrame(fd, 0);
                            return;
                        }
                    }
                }
            }, Invalidate);
        }

        object _lock = new object();
        int[] GifDelays(Image value, int count)
        {
            int PropertyTagFrameDelay = 0x5100;
            var propItem = value.GetPropertyItem(PropertyTagFrameDelay);
            if (propItem != null)
            {
                var bytes = propItem.Value;
                if (bytes != null)
                {
                    int[] delays = new int[count];
                    for (int i = 0; i < delays.Length; i++) delays[i] = BitConverter.ToInt32(bytes, i * 4) * 10;
                    return delays;
                }
            }
            int[] delaysd = new int[count];
            for (int i = 0; i < delaysd.Length; i++) delaysd[i] = 100;
            return delaysd;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            LoadGif();
        }

        #endregion

        #endregion

        #region 加载

        bool loading = false;
        /// <summary>
        /// 加载状态
        /// </summary>
        [Description("加载状态"), Category("外观"), DefaultValue(false)]
        public bool Loading
        {
            get => loading;
            set
            {
                if (loading == value) return;
                loading = value;
                Invalidate();
                OnPropertyChanged(nameof(Loading));
            }
        }

        float _value = 0F;
        /// <summary>
        /// 加载进度
        /// </summary>
        [Description("加载进度 0F-1F"), Category("数据"), DefaultValue(0F)]
        public float LoadingProgress
        {
            get => _value;
            set
            {
                if (_value == value) return;
                if (value < 0) value = 0;
                else if (value > 1) value = 1;
                _value = value;
                if (loading) Invalidate();
                OnPropertyChanged(nameof(LoadingProgress));
            }
        }

        #endregion

        #region 边框

        float borderWidth = 0F;
        /// <summary>
        /// 边框宽度
        /// </summary>
        [Description("边框宽度"), Category("边框"), DefaultValue(0F)]
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

        Color borColor = Color.FromArgb(246, 248, 250);
        /// <summary>
        /// 边框颜色
        /// </summary>
        [Description("边框颜色"), Category("边框"), DefaultValue(typeof(Color), "246, 248, 250")]
        public Color BorderColor
        {
            get => borColor;
            set
            {
                if (borColor == value) return;
                borColor = value;
                if (borderWidth > 0) Invalidate();
                OnPropertyChanged(nameof(BorderColor));
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

        #region 悬浮

        /// <summary>
        /// 启用悬浮交互
        /// </summary>
        [Description("启用悬浮交互"), Category("外观"), DefaultValue(false)]
        public bool EnableHover { get; set; }

        /// <summary>
        /// 悬浮前景
        /// </summary>
        [Description("悬浮前景"), Category("外观"), DefaultValue(null)]
        public Color? HoverFore { get; set; }

        /// <summary>
        /// 悬浮背景
        /// </summary>
        [Description("悬浮背景"), Category("外观"), DefaultValue(null)]
        public Color? HoverBack { get; set; }

        /// <summary>
        /// 悬浮图标
        /// </summary>
        [Description("悬浮图标"), Category("外观"), DefaultValue(null)]
        public Image? HoverImage { get; set; }

        /// <summary>
        /// 悬浮图标SVG
        /// </summary>
        [Description("悬浮图标SVG"), Category("外观"), DefaultValue(null)]
        public string? HoverImageSvg { get; set; }

        /// <summary>
        /// 悬浮图标比例
        /// </summary>
        [Description("悬浮图标比例"), Category("外观"), DefaultValue(.4F)]
        public float HoverImageRatio { get; set; } = .4F;

        #endregion

        #endregion

        #region 渲染

        protected override void OnDraw(DrawEventArgs e)
        {
            var g = e.Canvas;
            float _radius = radius * Dpi;

            var rect = ReadRectangle;
            if (shadow > 0 && shadowOpacity > 0) g.PaintShadow(this, e.Rect, rect, _radius, round);
            FillRect(g, rect, back, _radius, round);

            if (PaintImage(g, rect, _radius)) g.DrawText(Text, Font, Enabled ? ForeColor : Colour.TextQuaternary.Get(nameof(Avatar), "foreDisabled", ColorScheme), rect, stringCenter);
            if (borderWidth > 0) DrawRect(g, rect, borColor, borderWidth * Dpi, _radius, round);
            if (Hover)
            {
                int size = (int)((rect.Width > rect.Height ? rect.Height : rect.Width) * HoverImageRatio);
                FillRect(g, rect, HoverBack ?? Colour.TextTertiary.Get(nameof(Avatar), ColorScheme), _radius, round);
                var rect_hover = new Rectangle(rect.X + (rect.Width - size) / 2, rect.Y + (rect.Height - size) / 2, size, size);
                if (HoverImage != null) g.Image(HoverImage, rect_hover);
                if (HoverImageSvg != null) g.GetImgExtend(HoverImageSvg, rect_hover, HoverFore ?? Colour.BgBase.Get(nameof(Avatar), ColorScheme));
            }
            if (loading)
            {
                var bor6 = 6F * Dpi;
                int loading_size = (int)(40 * Dpi);
                var rect_loading = new Rectangle(rect.X + (rect.Width - loading_size) / 2, rect.Y + (rect.Height - loading_size) / 2, loading_size, loading_size);
                g.DrawEllipse(Color.FromArgb(220, Colour.PrimaryColor.Get(nameof(Avatar), ColorScheme)), bor6, rect_loading);
                using (var penpro = new Pen(Colour.Primary.Get(nameof(Avatar), ColorScheme), bor6))
                {
                    g.DrawArc(penpro, rect_loading, -90, 360F * _value);
                }
            }
            base.OnDraw(e);
        }

        #region 渲染帮助

        readonly FormatFlags stringCenter = FormatFlags.Center | FormatFlags.NoWrapEllipsis;

        bool PaintImage(Canvas g, Rectangle rect, float _radius)
        {
            int count = 0;
            if (image != null)
            {
                lock (_lock)
                {
                    g.Image(rect, image, imageFit, _radius, round);
                }
                count++;
            }
            if (imageSvg != null)
            {
                using (var bmp = SvgExtend.GetImgExtend(imageSvg, rect, ForeColor))
                {
                    if (bmp != null)
                    {
                        g.Image(rect, bmp, imageFit, _radius, round);
                        count++;
                    }
                }
            }
            return count == 0;
        }

        void FillRect(Canvas g, Rectangle rect, Color color, float radius, bool round)
        {
            if (round) g.FillEllipse(color, rect);
            else if (radius > 0)
            {
                using (var path = rect.RoundPath(radius))
                {
                    g.Fill(color, path);
                }
            }
            else g.Fill(color, rect);
        }

        void DrawRect(Canvas g, Rectangle rect, Color color, float width, float radius, bool round)
        {
            if (round) g.DrawEllipse(color, width, rect);
            else if (radius > 0)
            {
                using (var path = rect.RoundPath(radius))
                {
                    g.Draw(color, width, path);
                }
            }
            else g.Draw(color, width, rect);
        }

        #endregion

        public override Rectangle ReadRectangle
        {
            get
            {
                if (borderWidth > 0) return ClientRectangle.PaddingRect(Padding, borderWidth * Dpi / 2F);
                else return ClientRectangle.PaddingRect(Padding);
            }
        }

        public override GraphicsPath RenderRegion
        {
            get
            {
                if (borderWidth > 0)
                {
                    var rect = ReadRectangle;
                    if (round)
                    {
                        var path = new GraphicsPath();
                        path.AddEllipse(rect);
                        return path;
                    }
                    else if (radius > 0) return rect.RoundPath(radius * Dpi);
                    else
                    {
                        var path = new GraphicsPath();
                        path.AddRectangle(rect);
                        return path;
                    }
                }
                else
                {
                    var rect = ReadRectangle;
                    if (round)
                    {
                        var path = new GraphicsPath();
                        path.AddEllipse(rect);
                        return path;
                    }
                    else if (radius > 0) return rect.RoundPath(radius * Dpi);
                    else
                    {
                        var path = new GraphicsPath();
                        path.AddRectangle(rect);
                        return path;
                    }
                }
            }
        }

        #endregion

        #region 键盘

        bool hover = false;
        bool Hover
        {
            get => hover;
            set
            {
                if (hover == value) return;
                hover = value;
                Invalidate();
            }
        }
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            if (EnableHover) Hover = true;
            else if (Hover) Hover = false;
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            Hover = false;
        }

        #endregion
    }
}