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
    /// Button 按钮
    /// </summary>
    /// <remarks>按钮用于开始一个即时操作。</remarks>
    [Description("Button 按钮")]
    [ToolboxItem(true)]
    [DefaultProperty("Text")]
    public class Button : IControl, IButtonControl
    {
        public Button()
        {
            base.BackColor = Color.Transparent;
        }

        #region 属性

        #region 系统

        /// <summary>
        /// 背景颜色
        /// </summary>
        [Description("背景颜色"), Category("外观"), DefaultValue(null)]
        public new Color? BackColor
        {
            get => back;
            set
            {
                if (back == value) return;
                back = value;
                Invalidate();
            }
        }

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

        #region 背景

        Color? back;
        /// <summary>
        /// 背景颜色
        /// </summary>
        [Description("背景颜色"), Category("外观"), DefaultValue(null)]
        [Obsolete("使用 BackColor 属性替代"), Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

        /// <summary>
        /// 悬停背景颜色
        /// </summary>
        [Description("悬停背景颜色"), Category("外观"), DefaultValue(null)]
        public Color? BackHover { get; set; }

        /// <summary>
        /// 激活背景颜色
        /// </summary>
        [Description("激活背景颜色"), Category("外观"), DefaultValue(null)]
        public Color? BackActive { get; set; }

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

        #region 默认样式

        Color? defaultback;
        /// <summary>
        /// Default模式背景颜色
        /// </summary>
        [Description("Default模式背景颜色"), Category("外观"), DefaultValue(null)]
        public Color? DefaultBack
        {
            get => defaultback;
            set
            {
                if (defaultback == value) return;
                defaultback = value;
                if (type == TTypeMini.Default) Invalidate();
            }
        }

        Color? defaultbordercolor;
        /// <summary>
        /// Default模式边框颜色
        /// </summary>
        [Description("Default模式边框颜色"), Category("外观"), DefaultValue(null)]
        public Color? DefaultBorderColor
        {
            get => defaultbordercolor;
            set
            {
                if (defaultbordercolor == value) return;
                defaultbordercolor = value;
                if (type == TTypeMini.Default) Invalidate();
            }
        }

        #endregion

        #region 边框

        internal float borderWidth = 0;
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
            }
        }

        #endregion

        int margins = 4;
        /// <summary>
        /// 边距，用于激活动画
        /// </summary>
        [Description("边距，用于激活动画"), Category("外观"), DefaultValue(4)]
        public int Margins
        {
            get => margins;
            set
            {
                if (margins == value) return;
                margins = value;
                if (BeforeAutoSize()) Invalidate();
            }
        }

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

        internal TShape shape = TShape.Default;
        /// <summary>
        /// 形状
        /// </summary>
        [Description("形状"), Category("外观"), DefaultValue(TShape.Default)]
        public TShape Shape
        {
            get => shape;
            set
            {
                if (shape == value) return;
                shape = value;
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

        bool ghost = false;
        /// <summary>
        /// 幽灵属性，使按钮背景透明
        /// </summary>
        [Description("幽灵属性，使按钮背景透明"), Category("外观"), DefaultValue(false)]
        public bool Ghost
        {
            get => ghost;
            set
            {
                if (ghost == value) return;
                ghost = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 响应真实区域
        /// </summary>
        [Description("响应真实区域"), Category("行为"), DefaultValue(false)]
        public bool RespondRealAreas { get; set; }

        internal float ArrowProg = -1F;
        bool showArrow = false;
        /// <summary>
        /// 显示箭头
        /// </summary>
        [Description("显示箭头"), Category("行为"), DefaultValue(false)]
        public bool ShowArrow
        {
            get => showArrow;
            set
            {
                if (showArrow == value) return;
                showArrow = value;
                if (BeforeAutoSize()) Invalidate();
            }
        }

        bool isLink = false;
        /// <summary>
        /// 箭头链接样式
        /// </summary>
        [Description("箭头链接样式"), Category("行为"), DefaultValue(false)]
        public bool IsLink
        {
            get => isLink;
            set
            {
                if (isLink == value) return;
                isLink = value;
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
                if (string.IsNullOrEmpty(value)) value = null;
                if (text == value) return;
                text = value;
                if (BeforeAutoSize()) Invalidate();
            }
        }

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
                if (BeforeAutoSize()) Invalidate();
            }
        }

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
        /// <summary>
        /// 图像SVG
        /// </summary>
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

        /// <summary>
        /// 悬停图像
        /// </summary>
        [Description("悬停图像"), Category("外观"), DefaultValue(null)]
        public Image? ImageHover { get; set; } = null;

        /// <summary>
        /// 悬停图像SVG
        /// </summary>
        [Description("悬停图像SVG"), Category("外观"), DefaultValue(null)]
        public string? ImageHoverSvg { get; set; } = null;

        /// <summary>
        /// 悬停图像动画时长
        /// </summary>
        [Description("悬停图像动画时长"), Category("外观"), DefaultValue(200)]
        public int ImageHoverAnimation { get; set; } = 200;

        #endregion

        #region 加载动画

        bool loading = false;
        int AnimationLoadingValue = 0;
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
                SetCursor(_mouseHover && Enabled && !value);
                BeforeAutoSize();
                ThreadLoading?.Dispose();
                if (loading)
                {
                    AnimationClickValue = 0;
                    ThreadLoading = new ITask(this, i =>
                    {
                        AnimationLoadingValue = i;
                        Invalidate();
                        return loading;
                    }, 10, 360, 6, () =>
                    {
                        Invalidate();
                    });
                }
                else Invalidate();
            }
        }

        #endregion

        bool joinLeft = false;
        /// <summary>
        /// 连接左边
        /// </summary>
        [Description("连接左边"), Category("外观"), DefaultValue(false)]
        public bool JoinLeft
        {
            get => joinLeft;
            set
            {
                if (joinLeft == value) return;
                joinLeft = value;
                if (BeforeAutoSize()) Invalidate();
            }
        }

        bool joinRight = false;
        /// <summary>
        /// 连接右边
        /// </summary>
        [Description("连接右边"), Category("外观"), DefaultValue(false)]
        public bool JoinRight
        {
            get => joinRight;
            set
            {
                if (joinRight == value) return;
                joinRight = value;
                if (BeforeAutoSize()) Invalidate();
            }
        }

        #endregion

        #region 渲染

        protected override void OnPaint(PaintEventArgs e)
        {
            RectangleF rect = ClientRectangle.PaddingRect(Padding);
            var g = e.Graphics.High();
            bool enabled = Enabled;

            var rect_read = ReadRectangle;

            float _radius = (shape == TShape.Round || shape == TShape.Circle) ? rect_read.Height : radius * Config.Dpi;

            if (backImage != null) g.PaintImg(rect_read, backImage, backFit, _radius, shape);

            if (type == TTypeMini.Default)
            {
                Color _fore = Style.Db.DefaultColor, _color = Style.Db.Primary, _back_hover, _back_active;
                if (borderWidth > 0)
                {
                    _back_hover = Style.Db.PrimaryHover;
                    _back_active = Style.Db.PrimaryActive;
                }
                else
                {
                    _back_hover = Style.Db.FillSecondary;
                    _back_active = Style.Db.Fill;
                }
                if (fore.HasValue) _fore = fore.Value;
                if (BackHover.HasValue) _back_hover = BackHover.Value;
                if (BackActive.HasValue) _back_active = BackActive.Value;
                if (loading)
                {
                    _fore = Color.FromArgb(165, _fore);
                    _color = Color.FromArgb(165, _color);
                }

                using (var path = Path(rect_read, _radius))
                {
                    #region 动画

                    if (AnimationClick)
                    {
                        float maxw = rect_read.Width + ((rect.Width - rect_read.Width) * AnimationClickValue), maxh = rect_read.Height + ((rect.Height - rect_read.Height) * AnimationClickValue);
                        if (shape == TShape.Circle)
                        {
                            if (maxw > maxh) maxw = maxh;
                            else maxh = maxw;
                        }
                        int a = (int)(100 * (1f - AnimationClickValue));
                        using (var brush = new SolidBrush(Color.FromArgb(a, _color)))
                        {
                            using (var path_click = new RectangleF(rect.X + (rect.Width - maxw) / 2F, rect.Y + (rect.Height - maxh) / 2F, maxw, maxh).RoundPath(_radius, shape))
                            {
                                path_click.AddPath(path, false);
                                g.FillPath(brush, path_click);
                            }
                        }
                    }

                    #endregion

                    if (enabled)
                    {
                        if (!ghost)
                        {
                            #region 绘制阴影

                            if (margins > 0)
                            {
                                using (var path_shadow = new RectangleF(rect_read.X, rect_read.Y + 3, rect_read.Width, rect_read.Height).RoundPath(_radius))
                                {
                                    path_shadow.AddPath(path, false);
                                    using (var brush = new SolidBrush(Style.Db.FillQuaternary))
                                    {
                                        g.FillPath(brush, path_shadow);
                                    }
                                }
                            }

                            #endregion

                            using (var brush = new SolidBrush(defaultback ?? Style.Db.DefaultBg))
                            {
                                g.FillPath(brush, path);
                            }
                        }
                        if (borderWidth > 0)
                        {
                            float border = borderWidth * Config.Dpi;
                            if (ExtraMouseDown)
                            {
                                using (var brush = new Pen(_back_active, border))
                                {
                                    g.DrawPath(brush, path);
                                }
                                PaintTextLoading(g, text, _back_active, rect_read);
                            }
                            else if (AnimationHover)
                            {
                                var colorHover = Color.FromArgb(AnimationHoverValue, _back_hover);
                                using (var brush = new Pen(Style.Db.DefaultBorder, border))
                                {
                                    g.DrawPath(brush, path);
                                }
                                using (var brush = new Pen(colorHover, border))
                                {
                                    g.DrawPath(brush, path);
                                }
                                PaintTextLoading(g, text, _fore, colorHover, rect_read);
                            }
                            else if (ExtraMouseHover)
                            {
                                using (var brush = new Pen(_back_hover, border))
                                {
                                    g.DrawPath(brush, path);
                                }
                                PaintTextLoading(g, text, _back_hover, rect_read);
                            }
                            else
                            {
                                using (var brush = new Pen(defaultbordercolor ?? Style.Db.DefaultBorder, border))
                                {
                                    g.DrawPath(brush, path);
                                }
                                PaintTextLoading(g, text, _fore, rect_read);
                            }
                        }
                        else
                        {
                            if (ExtraMouseDown)
                            {
                                using (var brush = new SolidBrush(_back_active))
                                {
                                    g.FillPath(brush, path);
                                }
                            }
                            else if (AnimationHover)
                            {
                                using (var brush = new SolidBrush(Color.FromArgb(AnimationHoverValue, _back_hover)))
                                {
                                    g.FillPath(brush, path);
                                }
                            }
                            else if (ExtraMouseHover)
                            {
                                using (var brush = new SolidBrush(_back_hover))
                                {
                                    g.FillPath(brush, path);
                                }
                            }
                            PaintTextLoading(g, text, _fore, rect_read);
                        }
                    }
                    else
                    {
                        if (borderWidth > 0)
                        {
                            using (var brush = new SolidBrush(Style.Db.FillTertiary))
                            {
                                g.FillPath(brush, path);
                            }
                        }
                        PaintTextLoading(g, text, Style.Db.TextQuaternary, rect_read);
                    }
                }
            }
            else
            {
                Color _fore, _back, _back_hover, _back_active;
                switch (type)
                {
                    case TTypeMini.Error:
                        _back = Style.Db.Error;
                        _fore = Style.Db.ErrorColor;
                        _back_hover = Style.Db.ErrorHover;
                        _back_active = Style.Db.ErrorActive;
                        break;
                    case TTypeMini.Success:
                        _back = Style.Db.Success;
                        _fore = Style.Db.SuccessColor;
                        _back_hover = Style.Db.SuccessHover;
                        _back_active = Style.Db.SuccessActive;
                        break;
                    case TTypeMini.Info:
                        _back = Style.Db.Info;
                        _fore = Style.Db.InfoColor;
                        _back_hover = Style.Db.InfoHover;
                        _back_active = Style.Db.InfoActive;
                        break;
                    case TTypeMini.Warn:
                        _back = Style.Db.Warning;
                        _fore = Style.Db.WarningColor;
                        _back_hover = Style.Db.WarningHover;
                        _back_active = Style.Db.WarningActive;
                        break;
                    case TTypeMini.Primary:
                    default:
                        _back = Style.Db.Primary;
                        _fore = Style.Db.PrimaryColor;
                        _back_hover = Style.Db.PrimaryHover;
                        _back_active = Style.Db.PrimaryActive;
                        break;
                }

                if (fore.HasValue) _fore = fore.Value;
                if (back.HasValue) _back = back.Value;
                if (BackHover.HasValue) _back_hover = BackHover.Value;
                if (BackActive.HasValue) _back_active = BackActive.Value;
                if (loading) _back = Color.FromArgb(165, _back);

                using (var path = Path(rect_read, _radius))
                {
                    #region 动画

                    if (AnimationClick)
                    {
                        float maxw = rect_read.Width + ((rect.Width - rect_read.Width) * AnimationClickValue), maxh = rect_read.Height + ((rect.Height - rect_read.Height) * AnimationClickValue);
                        if (shape == TShape.Circle)
                        {
                            if (maxw > maxh) maxw = maxh;
                            else maxh = maxw;
                        }
                        int a = (int)(100 * (1f - AnimationClickValue));
                        using (var brush = new SolidBrush(Color.FromArgb(a, _back)))
                        {
                            using (var path_click = new RectangleF(rect.X + (rect.Width - maxw) / 2F, rect.Y + (rect.Height - maxh) / 2F, maxw, maxh).RoundPath(_radius, shape))
                            {
                                path_click.AddPath(path, false);
                                g.FillPath(brush, path_click);
                            }
                        }
                    }

                    #endregion

                    if (ghost)
                    {
                        #region 绘制背景

                        if (borderWidth > 0)
                        {
                            float border = borderWidth * Config.Dpi;
                            if (ExtraMouseDown)
                            {
                                using (var brush = new Pen(_back_active, border))
                                {
                                    g.DrawPath(brush, path);
                                }
                                PaintTextLoading(g, text, _back_active, rect_read);
                            }
                            else if (AnimationHover)
                            {
                                var colorHover = Color.FromArgb(AnimationHoverValue, _back_hover);
                                using (var brush = new Pen(enabled ? _back : Style.Db.FillTertiary, border))
                                {
                                    g.DrawPath(brush, path);
                                }
                                using (var brush = new Pen(colorHover, border))
                                {
                                    g.DrawPath(brush, path);
                                }
                                PaintTextLoading(g, text, _back, colorHover, rect_read);
                            }
                            else if (ExtraMouseHover)
                            {
                                using (var brush = new Pen(_back_hover, border))
                                {
                                    g.DrawPath(brush, path);
                                }
                                PaintTextLoading(g, text, _back_hover, rect_read);
                            }
                            else
                            {
                                using (var brush = new Pen(enabled ? _back : Style.Db.FillTertiary, border))
                                {
                                    g.DrawPath(brush, path);
                                }
                                PaintTextLoading(g, text, enabled ? _back : Style.Db.TextQuaternary, rect_read);
                            }
                        }
                        else PaintTextLoading(g, text, enabled ? _back : Style.Db.TextQuaternary, rect_read);

                        #endregion
                    }
                    else
                    {
                        #region 绘制阴影

                        if (enabled && margins > 0)
                        {
                            using (var path_shadow = new RectangleF(rect_read.X, rect_read.Y + 3, rect_read.Width, rect_read.Height).RoundPath(_radius))
                            {
                                path_shadow.AddPath(path, false);
                                using (var brush = new SolidBrush(_back.rgba(Config.Mode == TMode.Dark ? 0.15F : 0.1F)))
                                {
                                    g.FillPath(brush, path_shadow);
                                }
                            }
                        }

                        #endregion

                        #region 绘制背景

                        using (var brush = new SolidBrush(enabled ? _back : Style.Db.FillTertiary))
                        {
                            g.FillPath(brush, path);
                        }

                        if (ExtraMouseDown)
                        {
                            using (var brush = new SolidBrush(_back_active))
                            {
                                g.FillPath(brush, path);
                            }
                        }
                        else if (AnimationHover)
                        {
                            var colorHover = Color.FromArgb(AnimationHoverValue, _back_hover);
                            using (var brush = new SolidBrush(colorHover))
                            {
                                g.FillPath(brush, path);
                            }
                        }
                        else if (ExtraMouseHover)
                        {
                            using (var brush = new SolidBrush(_back_hover))
                            {
                                g.FillPath(brush, path);
                            }
                        }

                        #endregion

                        PaintTextLoading(g, text, enabled ? _fore : Style.Db.TextQuaternary, rect_read);
                    }
                }
            }
            this.PaintBadge(g);
            base.OnPaint(e);
        }

        #region 渲染帮助

        void PaintTextLoading(Graphics g, string? text, Color color, Rectangle rect_read)
        {
            var font_size = g.MeasureString(text ?? Config.NullText, Font).Size();
            if (text == null)
            {
                //没有文字
                var rect = GetImageRectCenter(font_size, rect_read);
                if (loading)
                {
                    float loading_size = rect_read.Height * 0.06F;
                    if (loading_size < 1F) loading_size = 1F;
                    using (var brush = new Pen(color, loading_size))
                    {
                        brush.StartCap = brush.EndCap = LineCap.Round;
                        g.DrawArc(brush, rect, AnimationLoadingValue, 100);
                    }
                }
                else
                {
                    if (PaintImageNoText(g, color, rect) && showArrow)
                    {
                        float size = font_size.Height * IconRatio;
                        var rect_arrow = new RectangleF(rect_read.X + (rect_read.Width - size) / 2F, rect_read.Y + (rect_read.Height - size) / 2F, size, size);
                        using (var pen = new Pen(color, 2F * Config.Dpi))
                        {
                            pen.StartCap = pen.EndCap = LineCap.Round;
                            if (isLink)
                            {
                                float size2 = rect_arrow.Width / 2F;
                                g.TranslateTransform(rect_arrow.X + size2, rect_arrow.Y + size2);
                                g.RotateTransform(-90F);
                                g.DrawLines(pen, new RectangleF(-size2, -size2, rect_arrow.Width, rect_arrow.Height).TriangleLines(ArrowProg));
                                g.ResetTransform();
                            }
                            else
                            {
                                g.DrawLines(pen, rect_arrow.TriangleLines(ArrowProg));
                            }
                        }
                    }
                }
            }
            else
            {
                bool right = RightToLeft == RightToLeft.Yes;
                bool has_left = loading || HasImage, has_rigth = showArrow;
                Rectangle rect_text;
                if (has_left || has_rigth)
                {
                    int font_width = font_size.Width;
                    int icon_size = (int)(font_size.Height * iconratio), sps = (int)(font_size.Height * .4F), sps2 = sps * 2, sp = (int)(font_size.Height * .25F);

                    if (has_left && has_rigth)
                    {
                        int read_width = font_width + icon_size + (sp * 2) + sps2, read_x = rect_read.X + ((rect_read.Width - read_width) / 2);

                        Rectangle rect_l, rect_r;

                        if (right)
                        {
                            rect_text = new Rectangle(read_x + sps, rect_read.Y + sps, font_width, rect_read.Height - sps2);
                            rect_l = new Rectangle(read_x + read_width - icon_size - sps, rect_read.Y + (rect_read.Height - icon_size) / 2, icon_size, icon_size);

                            rect_r = new Rectangle(rect_read.X + sps, rect_l.Y, icon_size, icon_size);
                        }
                        else
                        {
                            rect_text = new Rectangle(read_x + sps + icon_size + sp, rect_read.Y + sps, font_width, rect_read.Height - sps2);
                            rect_l = new Rectangle(read_x + sps, rect_read.Y + (rect_read.Height - icon_size) / 2, icon_size, icon_size);

                            rect_r = new Rectangle((rect_read.X + sps + rect_read.Width - sps2 - icon_size - sp) + sp, rect_l.Y, icon_size, icon_size);
                        }

                        if (loading)
                        {
                            float loading_size = rect_read.Height * 0.06F;
                            if (loading_size < 1F) loading_size = 1F;
                            using (var brush = new Pen(color, loading_size))
                            {
                                brush.StartCap = brush.EndCap = LineCap.Round;
                                g.DrawArc(brush, rect_l, AnimationLoadingValue, 100);
                            }
                        }
                        else PaintImage(g, color, rect_l);

                        #region ARROW

                        using (var pen = new Pen(color, 2F * Config.Dpi))
                        {
                            pen.StartCap = pen.EndCap = LineCap.Round;
                            if (isLink)
                            {
                                float size2 = rect_r.Width / 2F;
                                g.TranslateTransform(rect_r.X + size2, rect_r.Y + size2);
                                g.RotateTransform(-90F);
                                g.DrawLines(pen, new RectangleF(-size2, -size2, rect_r.Width, rect_r.Height).TriangleLines(ArrowProg));
                                g.ResetTransform();
                            }
                            else
                            {
                                g.DrawLines(pen, rect_r.TriangleLines(ArrowProg));
                            }
                        }

                        #endregion
                    }
                    else if (has_left)
                    {
                        int read_width = font_width + icon_size + (sp * 2) + sps2, read_x = rect_read.X + ((rect_read.Width - read_width) / 2);

                        Rectangle rect_l;
                        if (right)
                        {
                            rect_text = new Rectangle(read_x + sps, rect_read.Y + sps, font_width, rect_read.Height - sps2);
                            rect_l = new Rectangle(read_x + read_width - icon_size - sps, rect_read.Y + (rect_read.Height - icon_size) / 2, icon_size, icon_size);
                        }
                        else
                        {
                            rect_text = new Rectangle(read_x + sps + icon_size + sp, rect_read.Y + sps, font_width, rect_read.Height - sps2);
                            rect_l = new Rectangle(read_x + sps, rect_read.Y + (rect_read.Height - icon_size) / 2, icon_size, icon_size);
                        }
                        if (loading)
                        {
                            float loading_size = rect_read.Height * 0.06F;
                            if (loading_size < 1F) loading_size = 1F;
                            using (var brush = new Pen(color, loading_size))
                            {
                                brush.StartCap = brush.EndCap = LineCap.Round;
                                g.DrawArc(brush, rect_l, AnimationLoadingValue, 100);
                            }
                        }
                        else PaintImage(g, color, rect_l);
                    }
                    else
                    {
                        Rectangle rect_r;

                        if (right)
                        {
                            rect_text = new Rectangle(rect_read.X + sps + icon_size + sp, rect_read.Y + sps, rect_read.Width - sps2 - icon_size - sp, rect_read.Height - sps2);
                            rect_r = new Rectangle(rect_read.X + sps, rect_read.Y + (rect_read.Height - icon_size) / 2, icon_size, icon_size);
                        }
                        else
                        {
                            rect_text = new Rectangle(rect_read.X + sps, rect_read.Y + sps, rect_read.Width - sps2 - icon_size - sp, rect_read.Height - sps2);
                            rect_r = new Rectangle(rect_text.Right + sp, rect_read.Y + (rect_read.Height - icon_size) / 2, icon_size, icon_size);
                        }

                        #region ARROW

                        using (var pen = new Pen(color, 2F * Config.Dpi))
                        {
                            pen.StartCap = pen.EndCap = LineCap.Round;
                            if (isLink)
                            {
                                float size2 = rect_r.Width / 2F;
                                g.TranslateTransform(rect_r.X + size2, rect_r.Y + size2);
                                g.RotateTransform(-90F);
                                g.DrawLines(pen, new RectangleF(-size2, -size2, rect_r.Width, rect_r.Height).TriangleLines(ArrowProg));
                                g.ResetTransform();
                            }
                            else
                            {
                                g.DrawLines(pen, rect_r.TriangleLines(ArrowProg));
                            }
                        }

                        #endregion
                    }
                }
                else
                {
                    int sps = (int)(font_size.Height * .4F), sps2 = sps * 2;
                    rect_text = new Rectangle(rect_read.X + sps, rect_read.Y + sps, rect_read.Width - sps2, rect_read.Height - sps2);
                }
                PaintTextAlign(rect_read, ref rect_text);
                using (var brush = new SolidBrush(color))
                {
                    g.DrawString(text, Font, brush, rect_text, stringFormat);
                }
            }
        }
        void PaintTextLoading(Graphics g, string? text, Color color, Color colorHover, Rectangle rect_read)
        {
            var font_size = g.MeasureString(text ?? Config.NullText, Font).Size();
            if (text == null)
            {
                var rect = GetImageRectCenter(font_size, rect_read);
                if (loading)
                {
                    float loading_size = rect_read.Height * 0.06F;
                    if (loading_size < 1F) loading_size = 1F;
                    using (var brush = new Pen(color, loading_size))
                    {
                        brush.StartCap = brush.EndCap = LineCap.Round;
                        g.DrawArc(brush, rect, AnimationLoadingValue, 100);
                    }
                }
                else
                {
                    if (PaintImageNoText(g, color, rect))
                    {
                        if (showArrow)
                        {
                            float size = font_size.Height * IconRatio;
                            var rect_arrow = new RectangleF(rect_read.X + (rect_read.Width - size) / 2F, rect_read.Y + (rect_read.Height - size) / 2F, size, size);
                            using (var pen = new Pen(color, 2F * Config.Dpi))
                            using (var penHover = new Pen(colorHover, pen.Width))
                            {
                                pen.StartCap = pen.EndCap = LineCap.Round;
                                if (isLink)
                                {
                                    float size2 = rect_arrow.Width / 2F;
                                    g.TranslateTransform(rect_arrow.X + size2, rect_arrow.Y + size2);
                                    g.RotateTransform(-90F);
                                    var rect_arrow_lines = new RectangleF(-size2, -size2, rect_arrow.Width, rect_arrow.Height).TriangleLines(ArrowProg);
                                    g.DrawLines(pen, rect_arrow_lines);
                                    g.DrawLines(penHover, rect_arrow_lines);
                                    g.ResetTransform();
                                }
                                else
                                {
                                    var rect_arrow_lines = rect_arrow.TriangleLines(ArrowProg);
                                    g.DrawLines(pen, rect_arrow_lines);
                                    g.DrawLines(penHover, rect_arrow_lines);
                                }
                            }
                        }
                    }
                    else PaintImageNoText(g, colorHover, rect);
                }
            }
            else
            {
                bool right = RightToLeft == RightToLeft.Yes;
                bool has_left = loading || HasImage, has_rigth = showArrow;
                Rectangle rect_text;
                if (has_left || has_rigth)
                {
                    int font_width = font_size.Width;
                    int icon_size = (int)(font_size.Height * iconratio), sps = (int)(font_size.Height * .4F), sps2 = sps * 2, sp = (int)(font_size.Height * .25F);

                    if (has_left && has_rigth)
                    {
                        int read_width = font_width + icon_size + (sp * 2) + sps2, read_x = rect_read.X + ((rect_read.Width - read_width) / 2);

                        Rectangle rect_l, rect_r;

                        if (right)
                        {
                            rect_text = new Rectangle(read_x + sps, rect_read.Y + sps, font_width, rect_read.Height - sps2);
                            rect_l = new Rectangle(read_x + read_width - icon_size - sps, rect_read.Y + (rect_read.Height - icon_size) / 2, icon_size, icon_size);

                            rect_r = new Rectangle(rect_read.X + sps, rect_l.Y, icon_size, icon_size);
                        }
                        else
                        {
                            rect_text = new Rectangle(read_x + sps + icon_size + sp, rect_read.Y + sps, font_width, rect_read.Height - sps2);
                            rect_l = new Rectangle(read_x + sps, rect_read.Y + (rect_read.Height - icon_size) / 2, icon_size, icon_size);

                            rect_r = new Rectangle((rect_read.X + sps + rect_read.Width - sps2 - icon_size - sp) + sp, rect_l.Y, icon_size, icon_size);
                        }

                        if (loading)
                        {
                            float loading_size = rect_read.Height * 0.06F;
                            if (loading_size < 1F) loading_size = 1F;
                            using (var brush = new Pen(color, loading_size))
                            {
                                brush.StartCap = brush.EndCap = LineCap.Round;
                                g.DrawArc(brush, rect_l, AnimationLoadingValue, 100);
                            }
                        }
                        else
                        {
                            PaintImage(g, color, rect_l);
                            PaintImage(g, colorHover, rect_l);
                        }

                        #region ARROW

                        using (var pen = new Pen(color, 2F * Config.Dpi))
                        using (var penHover = new Pen(colorHover, pen.Width))
                        {
                            penHover.StartCap = penHover.EndCap = pen.StartCap = pen.EndCap = LineCap.Round;
                            if (isLink)
                            {
                                float size2 = rect_r.Width / 2F;
                                g.TranslateTransform(rect_r.X + size2, rect_r.Y + size2);
                                g.RotateTransform(-90F);
                                var rect_arrow = new RectangleF(-size2, -size2, rect_r.Width, rect_r.Height).TriangleLines(ArrowProg);
                                g.DrawLines(pen, rect_arrow);
                                g.DrawLines(penHover, rect_arrow);
                                g.ResetTransform();
                            }
                            else
                            {
                                var rect_arrow = rect_r.TriangleLines(ArrowProg);
                                g.DrawLines(pen, rect_arrow);
                                g.DrawLines(penHover, rect_arrow);
                            }
                        }

                        #endregion
                    }
                    else if (has_left)
                    {
                        int read_width = font_width + icon_size + (sp * 2) + sps2, read_x = rect_read.X + ((rect_read.Width - read_width) / 2);
                        Rectangle rect_l;
                        if (right)
                        {
                            rect_text = new Rectangle(read_x + sps, rect_read.Y + sps, font_width, rect_read.Height - sps2);
                            rect_l = new Rectangle(read_x + read_width - icon_size - sps, rect_read.Y + (rect_read.Height - icon_size) / 2, icon_size, icon_size);
                        }
                        else
                        {
                            rect_text = new Rectangle(read_x + sps + icon_size + sp, rect_read.Y + sps, font_width, rect_read.Height - sps2);
                            rect_l = new Rectangle(read_x + sps, rect_read.Y + (rect_read.Height - icon_size) / 2, icon_size, icon_size);
                        }

                        if (loading)
                        {
                            float loading_size = rect_read.Height * 0.06F;
                            if (loading_size < 1F) loading_size = 1F;
                            using (var brush = new Pen(color, loading_size))
                            {
                                brush.StartCap = brush.EndCap = LineCap.Round;
                                g.DrawArc(brush, rect_l, AnimationLoadingValue, 100);
                            }
                        }
                        else
                        {
                            PaintImage(g, color, rect_l);
                            PaintImage(g, colorHover, rect_l);
                        }
                    }
                    else
                    {
                        Rectangle rect_r;

                        if (right)
                        {
                            rect_text = new Rectangle(rect_read.X + sps + icon_size + sp, rect_read.Y + sps, rect_read.Width - sps2 - icon_size - sp, rect_read.Height - sps2);
                            rect_r = new Rectangle(rect_read.X + sps, rect_read.Y + (rect_read.Height - icon_size) / 2, icon_size, icon_size);
                        }
                        else
                        {
                            rect_text = new Rectangle(rect_read.X + sps, rect_read.Y + sps, rect_read.Width - sps2 - icon_size - sp, rect_read.Height - sps2);
                            rect_r = new Rectangle(rect_text.Right + sp, rect_read.Y + (rect_read.Height - icon_size) / 2, icon_size, icon_size);
                        }

                        #region ARROW

                        using (var pen = new Pen(color, 2F * Config.Dpi))
                        using (var penHover = new Pen(colorHover, pen.Width))
                        {
                            penHover.StartCap = penHover.EndCap = pen.StartCap = pen.EndCap = LineCap.Round;
                            if (isLink)
                            {
                                float size2 = rect_r.Width / 2F;
                                g.TranslateTransform(rect_r.X + size2, rect_r.Y + size2);
                                g.RotateTransform(-90F);
                                var rect_arrow = new RectangleF(-size2, -size2, rect_r.Width, rect_r.Height).TriangleLines(ArrowProg);
                                g.DrawLines(pen, rect_arrow);
                                g.DrawLines(penHover, rect_arrow);
                                g.ResetTransform();
                            }
                            else
                            {
                                var rect_arrow = rect_r.TriangleLines(ArrowProg);
                                g.DrawLines(pen, rect_arrow);
                                g.DrawLines(penHover, rect_arrow);
                            }
                        }

                        #endregion
                    }
                }
                else
                {
                    int sps = (int)(font_size.Height * .4F), sps2 = sps * 2;
                    rect_text = new Rectangle(rect_read.X + sps, rect_read.Y + sps, rect_read.Width - sps2, rect_read.Height - sps2);
                }
                PaintTextAlign(rect_read, ref rect_text);
                using (var brush = new SolidBrush(color))
                using (var brushHover = new SolidBrush(colorHover))
                {
                    g.DrawString(text, Font, brush, rect_text, stringFormat);
                    g.DrawString(text, Font, brushHover, rect_text, stringFormat);
                }
            }
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

        #region 渲染图片

        /// <summary>
        /// 渲染图片（没有文字）
        /// </summary>
        /// <param name="g">GDI</param>
        /// <param name="color">颜色</param>
        /// <param name="rect">区域</param>
        bool PaintImageNoText(Graphics g, Color? color, Rectangle rect)
        {
            if (AnimationImageHover)
            {
                PaintCoreImage(g, rect, color, 1F - AnimationImageHoverValue);
                PaintCoreImageHover(g, rect, color, AnimationImageHoverValue);
                return false;
            }
            else
            {
                if (ExtraMouseHover)
                {
                    if (PaintCoreImageHover(g, rect, color)) return false;
                }
                if (PaintCoreImage(g, rect, color)) return false;
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
                int w = (int)(font_size.Height * IconRatio * 1.125F);
                return new Rectangle(rect_read.X + (rect_read.Width - w) / 2, rect_read.Y + (rect_read.Height - w) / 2, w, w);
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
            var rect = GetImageRect(rectl);
            if (AnimationImageHover)
            {
                PaintCoreImage(g, rect, color, 1F - AnimationImageHoverValue);
                PaintCoreImageHover(g, rect, color, AnimationImageHoverValue);
                return;
            }
            else
            {
                if (ExtraMouseHover)
                {
                    if (PaintCoreImageHover(g, rect, color)) return;
                }
                PaintCoreImage(g, rect, color);
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

        bool PaintCoreImage(Graphics g, Rectangle rect, Color? color, float opacity = 1F)
        {
            if (imageSvg != null)
            {
                using (var _bmp = SvgExtend.GetImgExtend(imageSvg, rect, color))
                {
                    if (_bmp != null)
                    {
                        g.DrawImage(_bmp, rect, opacity);
                        return true;
                    }
                }
            }
            else if (image != null)
            {
                g.DrawImage(image, rect, opacity);
                return true;
            }
            return false;
        }

        bool PaintCoreImageHover(Graphics g, Rectangle rect, Color? color, float opacity = 1F)
        {
            if (ImageHoverSvg != null)
            {
                using (var _bmp = SvgExtend.GetImgExtend(ImageHoverSvg, rect, color))
                {
                    if (_bmp != null)
                    {
                        g.DrawImage(_bmp, rect, opacity);
                        return true;
                    }
                }
            }
            else if (ImageHover != null)
            {
                g.DrawImage(ImageHover, rect, opacity);
                return true;
            }
            return false;
        }

        #endregion

        internal GraphicsPath Path(RectangleF rect_read, float _radius)
        {
            if (shape == TShape.Circle)
            {
                var path = new GraphicsPath();
                path.AddEllipse(rect_read);
                return path;
            }
            if (joinLeft && joinRight) return rect_read.RoundPath(0);
            else if (joinRight) return rect_read.RoundPath(_radius, true, false, false, true);
            else if (joinLeft) return rect_read.RoundPath(_radius, false, true, true, false);
            return rect_read.RoundPath(_radius);
        }

        #endregion

        public override Rectangle ReadRectangle
        {
            get => ClientRectangle.PaddingRect(Padding).ReadRect(margins + (int)(borderWidth * Config.Dpi / 2F), shape, joinLeft, joinRight);
        }

        public override GraphicsPath RenderRegion
        {
            get
            {
                var rect_read = ReadRectangle;
                float _radius = (shape == TShape.Round || shape == TShape.Circle) ? rect_read.Height : radius * Config.Dpi;
                return Path(rect_read, _radius);
            }
        }

        #endregion

        #region 鼠标

        bool _mouseDown = false;
        bool ExtraMouseDown
        {
            get => _mouseDown;
            set
            {
                if (_mouseDown == value) return;
                _mouseDown = value;
                Invalidate();
            }
        }

        int AnimationHoverValue = 0;
        bool AnimationHover = false;
        bool AnimationImageHover = false;
        float AnimationImageHoverValue = 0F;
        bool _mouseHover = false;
        bool ExtraMouseHover
        {
            get => _mouseHover;
            set
            {
                if (_mouseHover == value) return;
                _mouseHover = value;
                var enabled = Enabled;
                SetCursor(value && enabled && !loading);
                if (enabled)
                {
                    Color _back_hover;
                    switch (type)
                    {
                        case TTypeMini.Default:
                            if (borderWidth > 0) _back_hover = Style.Db.PrimaryHover;
                            else _back_hover = Style.Db.FillSecondary;
                            break;
                        case TTypeMini.Success:
                            _back_hover = Style.Db.SuccessHover;
                            break;
                        case TTypeMini.Error:
                            _back_hover = Style.Db.ErrorHover;
                            break;
                        case TTypeMini.Info:
                            _back_hover = Style.Db.InfoHover;
                            break;
                        case TTypeMini.Warn:
                            _back_hover = Style.Db.WarningHover;
                            break;
                        case TTypeMini.Primary:
                        default:
                            _back_hover = Style.Db.PrimaryHover;
                            break;
                    }

                    if (BackHover.HasValue) _back_hover = BackHover.Value;
                    if (Config.Animation)
                    {
                        if (ImageHoverAnimation > 0 && HasImage && (ImageHoverSvg != null || ImageHover != null))
                        {
                            ThreadImageHover?.Dispose();
                            AnimationImageHover = true;
                            var t = Animation.TotalFrames(10, ImageHoverAnimation);
                            if (value)
                            {
                                ThreadImageHover = new ITask((i) =>
                                {
                                    AnimationImageHoverValue = Animation.Animate(i, t, 1F, AnimationType.Ball);
                                    Invalidate();
                                    return true;
                                }, 10, t, () =>
                                {
                                    AnimationImageHoverValue = 1F;
                                    AnimationImageHover = false;
                                    Invalidate();
                                });
                            }
                            else
                            {
                                ThreadImageHover = new ITask((i) =>
                                {
                                    AnimationImageHoverValue = 1F - Animation.Animate(i, t, 1F, AnimationType.Ball);
                                    Invalidate();
                                    return true;
                                }, 10, t, () =>
                                {
                                    AnimationImageHoverValue = 0F;
                                    AnimationImageHover = false;
                                    Invalidate();
                                });
                            }
                        }
                        if (_back_hover.A > 0)
                        {
                            int addvalue = _back_hover.A / 12;
                            ThreadHover?.Dispose();
                            AnimationHover = true;
                            if (value)
                            {
                                ThreadHover = new ITask(this, () =>
                                {
                                    AnimationHoverValue += addvalue;
                                    if (AnimationHoverValue > _back_hover.A) { AnimationHoverValue = _back_hover.A; return false; }
                                    Invalidate();
                                    return true;
                                }, 10, () =>
                                {
                                    AnimationHover = false;
                                    Invalidate();
                                });
                            }
                            else
                            {
                                ThreadHover = new ITask(this, () =>
                                {
                                    AnimationHoverValue -= addvalue;
                                    if (AnimationHoverValue < 1) { AnimationHoverValue = 0; return false; }
                                    Invalidate();
                                    return true;
                                }, 10, () =>
                                {
                                    AnimationHover = false;
                                    Invalidate();
                                });
                            }
                        }
                        else
                        {
                            AnimationHoverValue = _back_hover.A;
                            Invalidate();
                        }
                    }
                    else AnimationHoverValue = _back_hover.A;
                    Invalidate();
                }
            }
        }

        #region 动画

        protected override void Dispose(bool disposing)
        {
            ThreadClick?.Dispose();
            ThreadHover?.Dispose();
            ThreadImageHover?.Dispose();
            ThreadLoading?.Dispose();
            base.Dispose(disposing);
        }
        ITask? ThreadHover = null;
        ITask? ThreadImageHover = null;
        ITask? ThreadClick = null;
        ITask? ThreadLoading = null;

        #endregion

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (RespondRealAreas)
            {
                var rect_read = ReadRectangle;
                using (var path = Path(rect_read, radius * Config.Dpi))
                {
                    ExtraMouseHover = path.IsVisible(e.Location);
                }
            }
            base.OnMouseMove(e);

        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            if (RespondRealAreas) return;
            ExtraMouseHover = true;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            ExtraMouseHover = false;
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            ExtraMouseHover = false;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (CanClick(e.Location))
            {
                Focus();
                base.OnMouseDown(e);
                ExtraMouseDown = true;
            }
        }

        bool AnimationClick = false;
        float AnimationClickValue = 0;
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (CanClick(e.Location))
            {
                base.OnMouseUp(e);
                if (ExtraMouseDown && margins > 0 && Config.Animation && e.Button == MouseButtons.Left)
                {
                    ThreadClick?.Dispose();
                    AnimationClickValue = 0;
                    AnimationClick = true;
                    ThreadClick = new ITask(this, () =>
                    {
                        if (AnimationClickValue > 0.6) AnimationClickValue = AnimationClickValue.Calculate(0.04F);
                        else AnimationClickValue += AnimationClickValue = AnimationClickValue.Calculate(0.1F);
                        if (AnimationClickValue > 1) { AnimationClickValue = 0F; return false; }
                        Invalidate();
                        return true;
                    }, 50, () =>
                    {
                        AnimationClick = false;
                        Invalidate();
                    });
                }
            }
            ExtraMouseDown = false;
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
                return Helper.GDI(g =>
                {
                    var font_size = g.MeasureString(text ?? Config.NullText, Font).Size();
                    int gap = (int)(20 * Config.Dpi);
                    if (shape == TShape.Circle)
                    {
                        int s = font_size.Height + margins + gap;
                        return new Size(s, s);
                    }
                    else
                    {
                        int m = margins * 2;
                        if (joinLeft || joinRight) m = 0;
                        int count = 0;
                        if (loading || HasImage) count++;
                        if (showArrow) count++;
                        return new Size(font_size.Width + m + gap + ((int)Math.Ceiling(font_size.Height * 1.2F) * count), font_size.Height + margins + gap);
                    }
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

        #region 按钮点击

        [DefaultValue(DialogResult.None)]
        public DialogResult DialogResult { get; set; } = DialogResult.None;

        /// <summary>
        /// 是否默认按钮
        /// </summary>
        public void NotifyDefault(bool value)
        {

        }

        public void PerformClick()
        {
            OnClick(EventArgs.Empty);
        }

        protected override void OnClick(EventArgs e)
        {
            if (CanClick()) base.OnClick(e);
        }
        protected override void OnDoubleClick(EventArgs e)
        {
            if (CanClick()) base.OnDoubleClick(e);
        }
        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (CanClick(e.Location)) base.OnMouseClick(e);
        }
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            if (CanClick(e.Location)) base.OnMouseDoubleClick(e);
        }

        bool CanClick()
        {
            if (loading) return false;
            else
            {
                if (RespondRealAreas)
                {
                    var e = PointToClient(MousePosition);
                    var rect_read = ReadRectangle;
                    using (var path = Path(rect_read, radius * Config.Dpi))
                    {
                        return path.IsVisible(e);
                    }
                }
                else return true;
            }
        }
        bool CanClick(Point e)
        {
            if (loading) return false;
            else
            {
                if (RespondRealAreas)
                {
                    var rect_read = ReadRectangle;
                    using (var path = Path(rect_read, radius * Config.Dpi))
                    {
                        return path.IsVisible(e);
                    }
                }
                else return true;
            }
        }

        #endregion
    }
}