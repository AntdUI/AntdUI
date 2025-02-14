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
                OnPropertyChanged("Radius");
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
                OnPropertyChanged("BorderWidth");
            }
        }

        string? text = null;
        /// <summary>
        /// 文本
        /// </summary>
        [Description("文本"), Category("外观"), DefaultValue(null)]
        public override string? Text
        {
            get => this.GetLangI(LocalizationText, text);
            set
            {
                if (text == value) return;
                if (loop && string.IsNullOrEmpty(value))
                {
                    task?.Dispose();
                    task = null;
                }
                font_size = null;
                text = value;
                Invalidate();
                OnTextChanged(EventArgs.Empty);
                OnPropertyChanged("Text");
            }
        }

        [Description("文本"), Category("国际化"), DefaultValue(null)]
        public string? LocalizationText { get; set; }

        string? textTitle = null;
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
                OnPropertyChanged("TextTitle");
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
                OnPropertyChanged("Icon");
            }
        }

        bool loop = false;
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
                OnPropertyChanged("Loop");
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            this.AddListener();
            if (loop) StartTask();
        }

        protected override void OnFontChanged(EventArgs e)
        {
            font_size = null;
            base.OnFontChanged(e);
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

        ITask? task = null;
        private void StartTask()
        {
            task?.Dispose();
            if (loop)
            {
                task = new ITask(this, () =>
                {
                    if (font_size.HasValue && font_size.Value.Width > 0)
                    {
                        val += 1;
                        if (val > font_size.Value.Width)
                        {
                            if (Width > font_size.Value.Width) val = 0;
                            else val = -(Width - Padding.Horizontal);
                        }
                        Invalidate();
                    }
                    else System.Threading.Thread.Sleep(1000);
                    return loop;
                }, LoopSpeed);
            }
            else Invalidate();
        }

        #endregion

        #region 参数

        int val = 0;
        Size? font_size = null;

        #endregion

        /// <summary>
        /// 显示区域（容器）
        /// </summary>
        public override Rectangle DisplayRectangle => ClientRectangle.PaddingRect(Padding, borWidth / 2F * Config.Dpi);

        #endregion

        #region 渲染

        protected override void OnPaint(PaintEventArgs e)
        {
            var rect = DisplayRectangle;
            var g = e.Graphics.High();
            bool hasText = string.IsNullOrEmpty(Text);
            if (icon == TType.None)
            {
                if (loop)
                {
                    if (font_size == null && !hasText) font_size = g.MeasureString(Text, Font);
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
                            var size = g.MeasureString(Text, Font);
                            font_size = size;
                            int icon_size = (int)(size.Height * .86F), gap = (int)(icon_size * .4F);
                            var rect_txt = new Rectangle(rect.X + gap, rect.Y, rect.Width - gap * 2, rect.Height);
                            g.String(Text, Font, ForeColor, rect_txt, stringLeft);
                        }
                    }
                    else
                    {
                        using (var font_title = new Font(Font.FontFamily, Font.Size * 1.14F, Font.Style))
                        {
                            var size_title = g.MeasureString(TextTitle, font_title);
                            int icon_size = (int)(size_title.Height * 1.2F), gap = (int)(icon_size * .5F);
                            using (var brush = new SolidBrush(ForeColor))
                            {
                                var rect_txt = new Rectangle(rect.X + gap, rect.Y + gap, rect.Width - (gap * 2), size_title.Height);
                                g.String(TextTitle, font_title, brush, rect_txt, stringLeft);

                                int desc_y = rect_txt.Bottom + (int)(icon_size * .33F);
                                var rect_txt_desc = new Rectangle(rect_txt.X, desc_y, rect_txt.Width, rect.Height - (desc_y + gap));
                                g.String(Text, Font, brush, rect_txt_desc, stringLTEllipsis);
                            }
                        }
                    }
                }
            }
            else
            {
                float _radius = radius * Config.Dpi;
                Color back, bor_color, color = Colour.Text.Get("Alert");
                switch (icon)
                {
                    case TType.Success:
                        back = Colour.SuccessBg.Get("Alert");
                        bor_color = Colour.SuccessBorder.Get("Alert");
                        break;
                    case TType.Info:
                        back = Colour.InfoBg.Get("Alert");
                        bor_color = Colour.InfoBorder.Get("Alert");
                        break;
                    case TType.Warn:
                        back = Colour.WarningBg.Get("Alert");
                        bor_color = Colour.WarningBorder.Get("Alert");
                        break;
                    case TType.Error:
                        back = Colour.ErrorBg.Get("Alert");
                        bor_color = Colour.ErrorBorder.Get("Alert");
                        break;
                    default:
                        back = Colour.SuccessBg.Get("Alert");
                        bor_color = Colour.SuccessBorder.Get("Alert");
                        break;
                }
                using (var path = rect.RoundPath(_radius))
                {
                    var sizeT = g.MeasureString(Config.NullText, Font);
                    g.Fill(back, path);
                    if (loop)
                    {
                        if (font_size == null && !hasText) font_size = g.MeasureString(Text, Font);
                        if (font_size.HasValue)
                        {
                            int icon_size = (int)(sizeT.Height * .86F), gap = (int)(icon_size * .4F);
                            var rect_icon = new Rectangle(gap, rect.Y + (rect.Height - icon_size) / 2, icon_size, icon_size);
                            PaintText(g, rect, rect_icon, font_size.Value, color, back, _radius);
                            g.ResetClip();
                            g.PaintIcons(icon, rect_icon, Colour.BgBase.Get("Alert"), "Alert");
                        }
                    }
                    else
                    {
                        if (!hasText)
                        {
                            var size = g.MeasureString(Text, Font);
                            font_size = size;
                        }
                        if (string.IsNullOrEmpty(TextTitle))
                        {
                            int icon_size = (int)(sizeT.Height * .86F), gap = (int)(icon_size * .4F);
                            var rect_icon = new Rectangle(rect.X + gap, rect.Y + (rect.Height - icon_size) / 2, icon_size, icon_size);
                            g.PaintIcons(icon, rect_icon, Colour.BgBase.Get("Alert"), "Alert");
                            var rect_txt = new Rectangle(rect_icon.X + rect_icon.Width + gap, rect.Y, rect.Width - (rect_icon.Width + gap * 3), rect.Height);
                            g.String(Text, Font, color, rect_txt, stringLeft);
                        }
                        else
                        {
                            using (var font_title = new Font(Font.FontFamily, Font.Size * 1.14F, Font.Style))
                            {
                                int icon_size = (int)(sizeT.Height * 1.2F), gap = (int)(icon_size * .5F);

                                var rect_icon = new Rectangle(rect.X + gap, rect.Y + gap, icon_size, icon_size);
                                g.PaintIcons(icon, rect_icon, Colour.BgBase.Get("Alert"), "Alert");

                                using (var brush = new SolidBrush(color))
                                {
                                    var rect_txt = new Rectangle(rect_icon.X + rect_icon.Width + icon_size / 2, rect_icon.Y, rect.Width - (rect_icon.Width + gap * 3), rect_icon.Height);
                                    g.String(TextTitle, font_title, brush, rect_txt, stringLeft);

                                    var desc_y = rect_txt.Bottom + (int)(icon_size * .2F);
                                    var rect_txt_desc = new Rectangle(rect_txt.X, desc_y, rect_txt.Width, rect.Height - (desc_y + gap));
                                    g.String(Text, Font, brush, rect_txt_desc, stringLTEllipsis);
                                }
                            }
                        }
                    }
                    if (borWidth > 0) g.Draw(bor_color, borWidth * Config.Dpi, path);
                }
            }
            this.PaintBadge(g);
            base.OnPaint(e);
        }

        #region 渲染帮助

        readonly StringFormat stringLTEllipsis = Helper.SF_Ellipsis(tb: StringAlignment.Near, lr: StringAlignment.Near);
        readonly StringFormat stringCenter = Helper.SF_NoWrap();
        readonly StringFormat stringLeft = Helper.SF_ALL(lr: StringAlignment.Near);

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
                    var rect_txt = new Rectangle(rect.X - val, rect.Y, size.Width, rect.Height);
                    g.String(Text, Font, brush, rect_txt, stringCenter);
                    if (rect.Width > size.Width)
                    {
                        var maxw = rect.Width + rect_txt.Width / 2;
                        var rect_txt2 = new Rectangle(rect_txt.Right, rect_txt.Y, rect_txt.Width, rect_txt.Height);
                        while (rect_txt2.X < maxw)
                        {
                            g.String(Text, Font, brush, rect_txt2, stringCenter);
                            rect_txt2.X = rect_txt2.Right;
                        }
                    }
                }
                else
                {
                    var size_title = g.MeasureString(TextTitle, Font);
                    var rect_txt = new Rectangle(rect.X + size_title.Width - val, rect.Y, size.Width, rect.Height);
                    g.String(Text, Font, brush, rect_txt, stringCenter);
                    if (rect.Width > size.Width)
                    {
                        var maxw = rect.Width + rect_txt.Width / 2;
                        var rect_txt2 = new Rectangle(rect_txt.Right, rect_txt.Y, rect_txt.Width, rect_txt.Height);
                        while (rect_txt2.X < maxw)
                        {
                            g.String(Text, Font, brush, rect_txt2, stringCenter);
                            rect_txt2.X = rect_txt2.Right;
                        }
                    }

                    var rect_icon_l = new Rectangle(rect.X, rect.Y, (size.Height + size_title.Width) * 2, rect.Height);
                    using (var brush2 = new LinearGradientBrush(rect_icon_l, BackColor, Color.Transparent, 0F))
                    {
                        g.Fill(brush2, rect_icon_l);
                        g.Fill(brush2, rect_icon_l);
                    }
                    g.String(TextTitle, Font, brush, new Rectangle(rect.X, rect.Y, size_title.Width, rect.Height), stringCenter);
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

                g.String(Text, Font, brush_fore, rect_txt, stringCenter);
                if (rect.Width > size.Width)
                {
                    var maxw = rect.Width + rect_txt.Width / 2;
                    var rect_txt2 = new Rectangle(rect_txt.Right, rect_txt.Y, rect_txt.Width, rect_txt.Height);
                    while (rect_txt2.X < maxw)
                    {
                        g.String(Text, Font, brush_fore, rect_txt2, stringCenter);
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
                    var size_title = g.MeasureString(TextTitle, Font);
                    rect_icon_l = new Rectangle(rect.X, rect.Y, (size.Height + size_title.Width) * 2, rect.Height);
                    using (var brush = new LinearGradientBrush(rect_icon_l, back, Color.Transparent, 0F))
                    {
                        g.Fill(brush, rect_icon_l);
                        g.Fill(brush, rect_icon_l);
                    }
                    g.String(TextTitle, Font, brush_fore, new Rectangle(rect_icon.Right, rect.Y, size_title.Width, rect.Height), stringCenter);
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

        #endregion

        public override Rectangle ReadRectangle => DisplayRectangle;

        public override GraphicsPath RenderRegion => DisplayRectangle.RoundPath(radius * Config.Dpi);

        #endregion

        #region 事件

        protected override void OnMouseEnter(EventArgs e)
        {
            task?.Dispose();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (loop) StartTask();
            base.OnMouseLeave(e);
        }

        #endregion

        #region 语言变化

        public void HandleEvent(EventType id, object? tag)
        {
            switch (id)
            {
                case EventType.LANG:
                    font_size = null;
                    break;
            }
        }

        #endregion
    }
}