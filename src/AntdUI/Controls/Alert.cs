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
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// Alert 警告提示
    /// </summary>
    /// <remarks>警告提示，展现需要关注的信息。</remarks>
    [Description("Alert 警告提示")]
    [ToolboxItem(true)]
    [DefaultProperty("Text")]
    [Designer(typeof(IControlDesigner))]
    public class Alert : IControl, IEventListener
    {
        public Alert()
        {
            hover_close = new ITaskOpacity(nameof(Alert), this);
        }

        #region 属性

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
                Invalidate();
                OnPropertyChanged(nameof(Radius));
            }
        }

        float borWidth = 0F;
        /// <summary>
        /// 边框宽度
        /// </summary>
        [Description("边框宽度"), Category("边框"), DefaultValue(0F)]
        public float BorderWidth
        {
            get => borWidth;
            set
            {
                if (borWidth == value) return;
                borWidth = value;
                Invalidate();
                OnPropertyChanged(nameof(BorderWidth));
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
                if (loop && string.IsNullOrEmpty(value))
                {
                    task?.Dispose();
                    task = null;
                }
                text = value;
                ClearFont();
                OnTextChanged(EventArgs.Empty);
                OnPropertyChanged(nameof(Text));
                Invalidate();
            }
        }

        [Description("文本"), Category("国际化"), DefaultValue(null)]
        public string? LocalizationText { get; set; }

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
                sf = textAlign.SetAlignment(sf);
                sc = Helper.SetVerticalAlignment(sf, sc);
                Invalidate();
                OnPropertyChanged(nameof(TextAlign));
            }
        }

        string? textTitle;
        /// <summary>
        /// 标题
        /// </summary>
        [Description("标题"), Category("外观"), DefaultValue(null)]
        [Localizable(true)]
        public string? TextTitle
        {
            get => this.GetLangI(LocalizationTextTitle, textTitle);
            set
            {
                if (textTitle == value) return;
                textTitle = value;
                Invalidate();
                OnPropertyChanged(nameof(TextTitle));
            }
        }

        [Description("标题"), Category("国际化"), DefaultValue(null)]
        public string? LocalizationTextTitle { get; set; }

        TType icon = TType.None;
        /// <summary>
        /// 样式
        /// </summary>
        [Description("样式"), Category("外观"), DefaultValue(TType.None)]
        public TType Icon
        {
            get => icon;
            set
            {
                if (icon == value) return;
                icon = value;
                Invalidate();
                OnPropertyChanged(nameof(Icon));
            }
        }

        #region 图标

        string? iconSvg;
        /// <summary>
        /// 自定义图标SVG
        /// </summary>
        [Description("自定义图标SVG"), Category("外观"), DefaultValue(null)]
        public string? IconSvg
        {
            get => iconSvg;
            set
            {
                if (iconSvg == value) return;
                iconSvg = value;
                Invalidate();
                OnPropertyChanged(nameof(IconSvg));
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
                Invalidate();
                OnPropertyChanged(nameof(CloseIcon));
            }
        }

        float? iconratio;
        /// <summary>
        /// 图标比例
        /// </summary>
        [Description("图标比例"), Category("外观"), DefaultValue(null)]
        public float? IconRatio
        {
            get => iconratio;
            set
            {
                if (iconratio == value) return;
                iconratio = value;
                Invalidate();
                OnPropertyChanged(nameof(IconRatio));
            }
        }

        float? icongap;
        /// <summary>
        /// 图标与文字间距比例
        /// </summary>
        [Description("图标与文字间距比例"), Category("外观"), DefaultValue(null)]
        public float? IconGap
        {
            get => icongap;
            set
            {
                if (icongap == value) return;
                icongap = value;
                Invalidate();
                OnPropertyChanged(nameof(IconGap));
            }
        }

        /// <summary>
        /// 是否包含自定义图标
        /// </summary>
        public bool HasIcon => iconSvg != null;

        #endregion

        bool loop = false, loopState = false;
        /// <summary>
        /// 文本轮播
        /// </summary>
        [Description("文本轮播"), Category("外观"), DefaultValue(false)]
        public bool Loop
        {
            get => loop;
            set
            {
                if (loop == value) return;
                loop = value;
                if (IsHandleCreated) StartTask();
                OnPropertyChanged(nameof(Loop));
            }
        }

        /// <summary>
        /// 溢出轮播
        /// </summary>
        [Description("溢出轮播"), Category("外观"), DefaultValue(false)]
        public bool LoopOverflow { get; set; }

        /// <summary>
        /// 轮播文本无尽
        /// </summary>
        [Description("轮播文本无尽"), Category("外观"), DefaultValue(true)]
        public bool LoopInfinite { get; set; } = true;

        /// <summary>
        /// 鼠标移入时暂停轮播
        /// </summary>
        [Description("鼠标移入时暂停轮播"), Category("外观"), DefaultValue(true)]
        public bool LoopPauseOnMouseEnter { get; set; } = true;

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            this.AddListener();
            val = icon == TType.None ? 0 : -Height;
            if (loop) StartTask();
        }

        protected override void OnFontChanged(EventArgs e)
        {
            ClearFont();
            base.OnFontChanged(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            ClearFont();
            base.OnSizeChanged(e);
        }

        void ClearFont()
        {
            font_size = null;
            if (IsHandleCreated && loop && task == null) StartTask();
        }

        int loopSpeed = 10;
        /// <summary>
        /// 文本轮播速率
        /// </summary>
        [Description("文本轮播速率"), Category("外观"), DefaultValue(10)]
        public int LoopSpeed
        {
            get => loopSpeed;
            set
            {
                if (value < 1) value = 1;
                loopSpeed = value;
            }
        }

        #region 动画

        AnimationTask? task;
        void StartTask()
        {
            task?.Dispose();
            if (loop) task = new AnimationTask(new AnimationLoopConfig(this, TextAnimation, LoopSpeed).SetEnd(EndTask));
            else Invalidate();
        }

        void EndTask()
        {
            task = null;
            Invalidate();
        }

        bool TextAnimation()
        {
            if (font_size.HasValue && font_size.Value.Width > 0)
            {
                if (LoopOverflow && font_size.Value.Width < DisplayRectangle.Width)
                {
                    loopState = true;
                    val = 0;
                    return false;
                }
                else loopState = false;
                val += 1;
                if (val > font_size.Value.Width)
                {
                    if (LoopInfinite)
                    {
                        if (Width > font_size.Value.Width) val = 0;
                        else val = -(Width - Padding.Horizontal);
                    }
                    else val = -(Width - Padding.Horizontal);
                }
                Invalidate();
            }
            else System.Threading.Thread.Sleep(1000);
            return loop;
        }

        #endregion

        #region 参数

        int val;
        Size? font_size;

        #endregion

        /// <summary>
        /// 显示区域（容器）
        /// </summary>
        public override Rectangle DisplayRectangle => ClientRectangle.PaddingRect(Padding, borWidth / 2F * Config.Dpi);

        #endregion

        #region 事件

        /// <summary>
        /// Close时发生
        /// </summary>
        [Description("Close时发生"), Category("行为")]
        public event RBoolEventHandler? CloseChanged;

        protected virtual bool OnCloseChanged() => CloseChanged == null || CloseChanged(this, EventArgs.Empty);

        #endregion

        #region 鼠标

        ITaskOpacity hover_close;
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
                hover_close.MaxValue = Colour.Text.Get(nameof(Alert), ColorScheme).A - Colour.TextQuaternary.Get(nameof(Alert), ColorScheme).A;
                hover_close.Switch = rect_close.Contains(e.X, e.Y);
                SetCursor(hover_close.Switch);
            }
            else SetCursor(false);
            base.OnMouseMove(e);
        }

        #endregion

        #region 渲染

        protected override void OnDraw(DrawEventArgs e)
        {
            var rect = DisplayRectangle;
            var g = e.Canvas;
            bool hasText = string.IsNullOrEmpty(Text);
            if (icon == TType.None && !HasIcon)
            {
                if (loop)
                {
                    if (font_size == null && !hasText) font_size = g.MeasureText(Text, Font);
                    if (font_size.HasValue)
                    {
                        g.SetClip(rect);
                        PaintText(g, rect, font_size.Value, ForeColor);
                        g.ResetClip();
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(TextTitle))
                    {
                        if (!hasText)
                        {
                            var size = g.MeasureText(Text, Font);
                            font_size = size;
                            int icon_size = (int)(size.Height * .86F), gap = (int)(icon_size * .4F);
                            var rect_txt = new Rectangle(rect.X + gap, rect.Y, rect.Width - gap * 2, rect.Height);
                            g.DrawText(Text, Font, ForeColor, rect_txt, sf);
                        }
                    }
                    else
                    {
                        using (var font_title = new Font(Font.FontFamily, Font.Size * 1.14F, Font.Style))
                        {
                            var size_title = g.MeasureText(TextTitle, font_title);
                            int icon_size = (int)(size_title.Height * 1.2F), gap = (int)(icon_size * .5F);
                            using (var brush = new SolidBrush(ForeColor))
                            {
                                var rect_txt = new Rectangle(rect.X + gap, rect.Y + gap, rect.Width - (gap * 2), size_title.Height);
                                g.DrawText(TextTitle, font_title, brush, rect_txt, sf);

                                int desc_y = rect_txt.Bottom + (int)(icon_size * .33F);
                                var rect_txt_desc = new Rectangle(rect_txt.X, desc_y, rect_txt.Width, rect.Height - (desc_y + gap));
                                g.DrawText(Text, Font, brush, rect_txt_desc, sEllipsis);
                            }
                        }
                    }
                }
            }
            else
            {
                float _radius = radius * Config.Dpi;
                Color back, bor_color, color = Colour.Text.Get(nameof(Alert), ColorScheme);
                switch (icon)
                {
                    case TType.Success:
                        back = Colour.SuccessBg.Get(nameof(Alert), ColorScheme);
                        bor_color = Colour.SuccessBorder.Get(nameof(Alert), ColorScheme);
                        break;
                    case TType.Info:
                        back = Colour.InfoBg.Get(nameof(Alert), ColorScheme);
                        bor_color = Colour.InfoBorder.Get(nameof(Alert), ColorScheme);
                        break;
                    case TType.Warn:
                        back = Colour.WarningBg.Get(nameof(Alert), ColorScheme);
                        bor_color = Colour.WarningBorder.Get(nameof(Alert), ColorScheme);
                        break;
                    case TType.Error:
                        back = Colour.ErrorBg.Get(nameof(Alert), ColorScheme);
                        bor_color = Colour.ErrorBorder.Get(nameof(Alert), ColorScheme);
                        break;
                    default:
                        back = Colour.SuccessBg.Get(nameof(Alert), ColorScheme);
                        bor_color = Colour.SuccessBorder.Get(nameof(Alert), ColorScheme);
                        break;
                }
                using (var path = rect.RoundPath(_radius))
                {
                    var sizeT = g.MeasureString(Config.NullText, Font);
                    g.Fill(back, path);
                    if (loop)
                    {
                        if (font_size == null && !hasText) font_size = g.MeasureText(Text, Font);
                        if (font_size.HasValue)
                        {
                            int icon_size = (int)(sizeT.Height * (iconratio ?? .86F)), gap = (int)(icon_size * (icongap ?? .4F));
                            var rect_icon = new Rectangle(gap, rect.Y + (rect.Height - icon_size) / 2, icon_size, icon_size);
                            PaintText(g, rect, rect_icon, font_size.Value, color, back, _radius);
                            g.ResetClip();
                            g.PaintIcons(icon, iconSvg, rect_icon, Colour.BgBase, nameof(Alert), ColorScheme);
                        }
                    }
                    else
                    {
                        if (!hasText)
                        {
                            var size = g.MeasureText(Text, Font);
                            font_size = size;
                        }
                        if (string.IsNullOrEmpty(TextTitle))
                        {
                            int icon_size = (int)(sizeT.Height * (iconratio ?? .86F)), gap = (int)(icon_size * (icongap ?? .4F));
                            var rect_icon = new Rectangle(rect.X + gap, rect.Y + (rect.Height - icon_size) / 2, icon_size, icon_size);
                            g.PaintIcons(icon, iconSvg, rect_icon, Colour.BgBase, nameof(Alert), ColorScheme);
                            var rect_txt = new Rectangle(rect_icon.X + rect_icon.Width + gap, rect.Y, rect.Width - (rect_icon.Width + gap * 3), rect.Height);
                            g.DrawText(Text, Font, color, rect_txt, sf);
                            PaintCloseIcon(g, rect_txt, .4F);
                        }
                        else
                        {
                            using (var font_title = new Font(Font.FontFamily, Font.Size * 1.14F, Font.Style))
                            {
                                int icon_size = (int)(sizeT.Height * (iconratio ?? 1.2F)), gap = (int)(icon_size * (icongap ?? .5F));

                                var rect_icon = new Rectangle(rect.X + gap, rect.Y + gap, icon_size, icon_size);
                                g.PaintIcons(icon, iconSvg, rect_icon, Colour.BgBase, nameof(Alert), ColorScheme);

                                using (var brush = new SolidBrush(color))
                                {
                                    var rect_txt = new Rectangle(rect_icon.X + rect_icon.Width + icon_size / 2, rect_icon.Y, rect.Width - (rect_icon.Width + gap * 3), rect_icon.Height);
                                    g.DrawText(TextTitle, font_title, brush, rect_txt, sf);

                                    var desc_y = rect_txt.Bottom + (int)(icon_size * .2F);
                                    var rect_txt_desc = new Rectangle(rect_txt.X, desc_y, rect_txt.Width, rect.Height - (desc_y + gap));
                                    g.DrawText(Text, Font, brush, rect_txt_desc, sEllipsis);
                                    PaintCloseIcon(g, rect_txt, .7F);
                                }
                            }
                        }
                    }
                    if (borWidth > 0) g.Draw(bor_color, borWidth * Config.Dpi, path);
                }
            }
            base.OnDraw(e);
        }

        #region 渲染帮助

        readonly FormatFlags sEllipsis = FormatFlags.Left | FormatFlags.Top | FormatFlags.EllipsisCharacter;
        FormatFlags sc = FormatFlags.Center | FormatFlags.NoWrap, sf = FormatFlags.Left | FormatFlags.VerticalCenter | FormatFlags.NoWrapEllipsis;

        /// <summary>
        /// 渲染文字
        /// </summary>
        /// <param name="g">GDI</param>
        /// <param name="rect">区域</param>
        /// <param name="size">文字大小</param>
        /// <param name="fore">文字颜色</param>
        void PaintText(Canvas g, Rectangle rect, Size size, Color fore)
        {
            using (var brush = new SolidBrush(fore))
            {
                if (string.IsNullOrEmpty(TextTitle))
                {
                    bool c = rect.Width > size.Width;
                    if (loopState && c) g.DrawText(Text, Font, brush, rect, sf);
                    else
                    {
                        var rect_txt = new Rectangle(rect.X - val, rect.Y, size.Width, rect.Height);
                        g.DrawText(Text, Font, brush, rect_txt, sc);
                        if (LoopInfinite && c)
                        {
                            g.DrawText(Text, Font, brush, rect_txt, sc);
                            var maxw = rect.Width + rect_txt.Width / 2;
                            var rect_txt2 = new Rectangle(rect_txt.Right, rect_txt.Y, rect_txt.Width, rect_txt.Height);
                            while (rect_txt2.X < maxw)
                            {
                                g.DrawText(Text, Font, brush, rect_txt2, sc);
                                rect_txt2.X = rect_txt2.Right;
                            }
                        }
                    }
                }
                else
                {
                    var size_title = g.MeasureText(TextTitle, Font);
                    var rect_txt = new Rectangle(rect.X + size_title.Width - val, rect.Y, size.Width, rect.Height);
                    g.DrawText(Text, Font, brush, rect_txt, sc);
                    if (LoopInfinite && rect.Width > size.Width)
                    {
                        var maxw = rect.Width + rect_txt.Width / 2;
                        var rect_txt2 = new Rectangle(rect_txt.Right, rect_txt.Y, rect_txt.Width, rect_txt.Height);
                        while (rect_txt2.X < maxw)
                        {
                            g.DrawText(Text, Font, brush, rect_txt2, sc);
                            rect_txt2.X = rect_txt2.Right;
                        }
                    }

                    var rect_icon_l = new Rectangle(rect.X, rect.Y, (size.Height + size_title.Width) * 2, rect.Height);
                    using (var brush2 = new LinearGradientBrush(rect_icon_l, BackColor, Color.Transparent, 0F))
                    {
                        g.Fill(brush2, rect_icon_l);
                        g.Fill(brush2, rect_icon_l);
                    }
                    g.DrawText(TextTitle, Font, brush, new Rectangle(rect.X, rect.Y, size_title.Width, rect.Height), sc);
                }
            }
        }

        /// <summary>
        /// 渲染文字（渐变遮罩）
        /// </summary>
        /// <param name="g">GDI</param>
        /// <param name="rect">区域</param>
        /// <param name="size">文字大小</param>
        /// <param name="fore">文字颜色</param>
        /// <param name="back">背景颜色</param>
        void PaintText(Canvas g, Rectangle rect, Rectangle rect_icon, Size size, Color fore, Color back, float radius)
        {
            using (var brush_fore = new SolidBrush(fore))
            {
                var rect_txt = new Rectangle(rect.X - val, rect.Y, size.Width, rect.Height);

                g.SetClip(new Rectangle(rect.X, rect_txt.Y + ((rect.Height - size.Height) / 2), rect.Width, size.Height));

                g.DrawText(Text, Font, brush_fore, rect_txt, sc);
                if (LoopInfinite && rect.Width > size.Width)
                {
                    var maxw = rect.Width + rect_txt.Width / 2;
                    var rect_txt2 = new Rectangle(rect_txt.Right, rect_txt.Y, rect_txt.Width, rect_txt.Height);
                    while (rect_txt2.X < maxw)
                    {
                        g.DrawText(Text, Font, brush_fore, rect_txt2, sc);
                        rect_txt2.X = rect_txt2.Right;
                    }
                }

                Rectangle rect_icon_l;
                if (string.IsNullOrEmpty(TextTitle))
                {
                    rect_icon_l = new Rectangle(rect.X, rect.Y, size.Height * 2, rect.Height);
                    using (var brush = new LinearGradientBrush(rect_icon_l, back, Color.Transparent, 0F))
                    {
                        rect_icon_l.Width -= 1;
                        g.Fill(brush, rect_icon_l);
                        g.Fill(brush, rect_icon_l);
                    }
                }
                else
                {
                    var size_title = g.MeasureText(TextTitle, Font);
                    rect_icon_l = new Rectangle(rect.X, rect.Y, (size.Height + size_title.Width) * 2, rect.Height);
                    using (var brush = new LinearGradientBrush(rect_icon_l, back, Color.Transparent, 0F))
                    {
                        g.Fill(brush, rect_icon_l);
                        g.Fill(brush, rect_icon_l);
                    }
                    g.DrawText(TextTitle, Font, brush_fore, new Rectangle(rect_icon.Right, rect.Y, size_title.Width, rect.Height), sc);
                }
                var rect_icon_r = new Rectangle(rect.Right - rect_icon_l.Width, rect_icon_l.Y, rect_icon_l.Width, rect_icon_l.Height);
                using (var brush = new LinearGradientBrush(rect_icon_r, Color.Transparent, back, 0F))
                {
                    rect_icon_r.X += 1;
                    rect_icon_r.Width -= 1;
                    g.Fill(brush, rect_icon_r);
                    g.Fill(brush, rect_icon_r);
                }
            }
        }

        void PaintCloseIcon(Canvas g, Rectangle rect, float ratio)
        {
            if (closeIcon)
            {
                int size = (int)(rect.Height * ratio);
                var rect_arrow = new Rectangle(rect.Right - size, rect.Y + (rect.Height - size) / 2, size, size);
                rect_close = rect_arrow;
                if (hover_close.Animation) g.PaintIconClose(rect_arrow, Helper.ToColor(hover_close.Value + Colour.TextQuaternary.Get(nameof(Alert), ColorScheme).A, Colour.Text.Get(nameof(Alert), ColorScheme)));
                else if (hover_close.Switch) g.PaintIconClose(rect_arrow, Colour.Text.Get(nameof(Alert), ColorScheme));
                else g.PaintIconClose(rect_arrow, Colour.TextQuaternary.Get(nameof(Alert), ColorScheme));
            }
        }

        #endregion

        public override Rectangle ReadRectangle => DisplayRectangle;

        public override GraphicsPath RenderRegion => DisplayRectangle.RoundPath(radius * Config.Dpi);

        #endregion

        #region 事件

        protected override void OnMouseEnter(EventArgs e)
        {
            if (LoopPauseOnMouseEnter) task?.Dispose();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (LoopPauseOnMouseEnter && loop) StartTask();
            base.OnMouseLeave(e);
        }

        #endregion

        #region 语言变化

        public void HandleEvent(EventType id, object? tag)
        {
            switch (id)
            {
                case EventType.LANG:
                    ClearFont();
                    break;
            }
        }

        #endregion
    }
}