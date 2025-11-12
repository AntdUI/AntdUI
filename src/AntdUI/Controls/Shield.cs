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
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// Shield 徽章
    /// </summary>
    /// <remarks>展示徽章图标。</remarks>
    [Description("Shield 徽章")]
    [ToolboxItem(true)]
    public class Shield : IControl, IEventListener
    {
        #region 属性默认值

        public Shield()
        {
            AutoSize = true;
        }

        #endregion

        #region 属性

        Color? fore;
        /// <summary>
        /// 右侧部分的文本颜色
        /// </summary>
        [Description("右侧部分的文本颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? Fore
        {
            get => fore;
            set
            {
                if (fore == value) return;
                fore = value;
                Invalidate();
                OnPropertyChanged(nameof(Fore));
            }
        }

        Color? color;
        /// <summary>
        /// 右侧部分的背景颜色
        /// </summary>
        [Description("右侧部分的背景颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? Color
        {
            get => color;
            set
            {
                if (color == value) return;
                color = value;
                Invalidate();
                OnPropertyChanged(nameof(Color));
            }
        }

        Color? labelFore;
        /// <summary>
        /// 左侧部分的文本颜色
        /// </summary>
        [Description("左侧部分的文本颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? LabelFore
        {
            get => labelFore;
            set
            {
                if (labelFore == value) return;
                labelFore = value;
                Invalidate();
                OnPropertyChanged(nameof(LabelFore));
            }
        }

        Color? labelColor;
        /// <summary>
        /// 左侧部分的背景颜色
        /// </summary>
        [Description("左侧部分的背景颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? LabelColor
        {
            get => labelColor;
            set
            {
                if (labelColor == value) return;
                labelColor = value;
                Invalidate();
                OnPropertyChanged(nameof(LabelColor));
            }
        }

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

        #region 文本

        string text = "AntdUI";
        /// <summary>
        /// 文本
        /// </summary>
        [Description("文本"), Category("外观"), DefaultValue("AntdUI")]
        public override string Text
        {
            get => this.GetLangIN(LocalizationText, text);
#pragma warning disable CS8765
            set
#pragma warning restore CS8765
            {
                if (text == value) return;
                text = value;
                if (BeforeAutoSize()) Invalidate();
                OnPropertyChanged(nameof(Text));
            }
        }

        [Description("文本"), Category("国际化"), DefaultValue(null)]
        public string? LocalizationText { get; set; }

        string? label;
        /// <summary>
        /// 左侧文本
        /// </summary>
        [Description("左侧文本"), Category("外观"), DefaultValue(null), Localizable(true)]
        public string? Label
        {
            get => this.GetLangI(LocalizationLabel, label);
            set
            {
                if (label == value) return;
                label = value;
                if (BeforeAutoSize()) Invalidate();
                OnPropertyChanged(nameof(Label));
            }
        }

        /// <summary>
        /// 左侧文本
        /// </summary>
        [Description("左侧文本"), Category("国际化"), DefaultValue(null)]
        public string? LocalizationLabel { get; set; }

        /// <summary>
        /// 左侧文本粗体
        /// </summary>
        [Description("左侧文本粗体"), Category("外观"), DefaultValue(false)]
        public bool Bold { get; set; }

        /// <summary>
        /// 阴影偏移X
        /// </summary>
        [Description("阴影偏移X"), Category("阴影"), DefaultValue(0)]
        public int ShadowOffsetX { get; set; }

        /// <summary>
        /// 阴影偏移Y
        /// </summary>
        [Description("阴影偏移Y"), Category("阴影"), DefaultValue(0)]
        public int ShadowOffsetY { get; set; }

        #endregion

        Image? logo;
        [Description("Logo"), Category("外观"), DefaultValue(null)]
        public Image? Logo
        {
            get => logo;
            set
            {
                if (logo == value) return;
                logo = value;
                if (BeforeAutoSize()) Invalidate();
                OnPropertyChanged(nameof(Logo));
            }
        }

        string? logoSvg;
        [Description("LogoSvg"), Category("外观"), DefaultValue(null)]
        public string? LogoSvg
        {
            get => logoSvg;
            set
            {
                if (logoSvg == value) return;
                logoSvg = value;
                if (BeforeAutoSize()) Invalidate();
                OnPropertyChanged(nameof(LogoSvg));
            }
        }

        #endregion

        #region 渲染

        protected override void OnDraw(DrawEventArgs e)
        {
            var g = e.Canvas;
            var _rect = e.Rect.DeflateRect(Padding);
            if (label == null)
            {
                var size = g.MeasureString(Text, Font);
                int bw = (int)(size.Width * .14F);
                int sizeR = size.Width + bw;
                if (logoSvg == null && logo == null)
                {
                    int x = _rect.X + (_rect.Width - sizeR) / 2, h = size.Height + (int)(size.Height * .66F), y = _rect.Y + (_rect.Height - h) / 2;
                    Rectangle rect_r = new Rectangle(x, y, sizeR, h);
                    PaintBack(g, rect_r);
                    PaintFore(g, Text, Font, fore ?? System.Drawing.Color.White, rect_r, Bold);
                }
                else
                {
                    int logosize = (int)(size.Height * 1.12F), logosp = (int)(logosize * .274F);
                    int xc = _rect.X + (_rect.Width - (sizeR + logosize + logosp)) / 2, h = size.Height + (int)(size.Height * .66F), y = _rect.Y + (_rect.Height - h) / 2;
                    Rectangle rect_l_full = new Rectangle(xc, y, sizeR + logosize + logosp, h),
                        rect_logo = new Rectangle(xc + bw / 2, _rect.Y + (_rect.Height - logosize) / 2, logosize, logosize),
                        rect_r = new Rectangle(xc + logosize + logosp, y, sizeR, h);
                    PaintBack(g, rect_l_full);
                    Color _fore = fore ?? System.Drawing.Color.White;
                    if (logo != null) g.Image(logo, rect_logo);
                    if (logoSvg != null) g.GetImgExtend(logoSvg, rect_logo, _fore);
                    PaintFore(g, Text, Font, _fore, rect_r, Bold);
                }
            }
            else
            {
                var size = g.MeasureString(Config.NullText, Font);
                int bw = (int)(size.Width * .4F);
                int sizeR = g.MeasureString(Text, Font).Width + bw, sizeL = g.MeasureString(Label, Font).Width + bw;
                if (logoSvg == null && logo == null)
                {
                    int xc = _rect.X + (_rect.Width - (sizeR + sizeL)) / 2, h = size.Height + (int)(size.Height * .66F), y = _rect.Y + (_rect.Height - h) / 2;
                    Rectangle rect_l = new Rectangle(xc, y, sizeL, h), rect_r = new Rectangle(xc + sizeL, y, sizeR, h);
                    PaintBack(g, rect_l, rect_r);
                    PaintFore(g, Label, Font, labelFore ?? System.Drawing.Color.White, rect_l);
                    PaintFore(g, Text, Font, fore ?? System.Drawing.Color.White, rect_r, Bold);
                }
                else
                {
                    int logosize = (int)(size.Height * 1.12F), logosp = (int)(logosize * .274F);
                    int xc = _rect.X + (_rect.Width - (sizeL + sizeR + logosize + logosp)) / 2, h = size.Height + (int)(size.Height * .66F), y = _rect.Y + (_rect.Height - h) / 2;
                    Rectangle rect_l_full = new Rectangle(xc, y, sizeL + logosize + logosp, h),
                        rect_logo = new Rectangle(xc + bw / 2, _rect.Y + (_rect.Height - logosize) / 2, logosize, logosize),
                        rect_l = new Rectangle(xc + logosize + logosp, y, sizeL, h),
                        rect_r = new Rectangle(xc + logosize + logosp + sizeL, y, sizeR, h);
                    PaintBack(g, rect_l_full, rect_r);
                    Color _label = labelFore ?? System.Drawing.Color.White;
                    if (logo != null) g.Image(logo, rect_logo);
                    if (logoSvg != null) g.GetImgExtend(logoSvg, rect_logo, _label);
                    PaintFore(g, Label, Font, _label, rect_l);
                    PaintFore(g, Text, Font, fore ?? System.Drawing.Color.White, rect_r, Bold);
                }
            }
            base.OnDraw(e);
        }

        void PaintBack(Canvas g, Rectangle rect_l, Rectangle rect_r)
        {
            if (radius > 0)
            {
                float _radius = radius * Config.Dpi;
                using (var path = rect_l.RoundPath(_radius, true, false, false, true))
                {
                    g.Fill(labelColor ?? System.Drawing.Color.FromArgb(85, 85, 85), path);
                }
                using (var path = rect_r.RoundPath(_radius, false, true, true, false))
                {
                    g.Fill(color ?? System.Drawing.Color.FromArgb(68, 204, 17), path);
                }
            }
            else
            {
                g.Fill(labelColor ?? System.Drawing.Color.FromArgb(85, 85, 85), rect_l);
                g.Fill(color ?? System.Drawing.Color.FromArgb(68, 204, 17), rect_r);
            }
        }
        void PaintBack(Canvas g, Rectangle rect)
        {
            if (radius > 0)
            {
                float _radius = radius * Config.Dpi;
                using (var path = rect.RoundPath(_radius))
                {
                    g.Fill(color ?? System.Drawing.Color.FromArgb(68, 204, 17), path);
                }
            }
            else g.Fill(color ?? System.Drawing.Color.FromArgb(68, 204, 17), rect);
        }
        void PaintFore(Canvas g, string? text, Font font, Color color, Rectangle rect, bool bold)
        {
            if (bold)
            {
                using (var fontb = new Font(font, FontStyle.Bold))
                {
                    PaintFore(g, text, fontb, color, rect);
                }
            }
            else PaintFore(g, text, font, color, rect);
        }
        void PaintFore(Canvas g, string? text, Font font, Color color, Rectangle rect)
        {
            if (ShadowOffsetX > 0 || ShadowOffsetY > 0) g.String(text, font, System.Drawing.Color.FromArgb(77, 1, 1, 1), new Rectangle(rect.X + (int)(ShadowOffsetX * Config.Dpi), rect.Y + (int)(ShadowOffsetY * Config.Dpi), rect.Width, rect.Height));
            g.String(text, font, color, rect);
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
                    if (label == null)
                    {
                        var size = g.MeasureString(Text, Font);
                        int textSize = size.Width + (int)(size.Width * .14F);
                        if (logoSvg == null && logo == null) return new Size(textSize + Padding.Horizontal, (int)(size.Height + (size.Height * .66F) + Padding.Vertical));
                        else
                        {
                            int logosize = (int)(size.Height * 1.12F), logosp = (int)(logosize * .274F);
                            return new Size(textSize + logosize + logosp + Padding.Horizontal, (int)(size.Height + (size.Height * .66F) + Padding.Vertical));
                        }
                    }
                    else
                    {
                        var size = g.MeasureString(Config.NullText, Font);
                        int bw = (int)(size.Width * .4F);
                        int textSize = g.MeasureString(Text, Font).Width + bw, subSize = g.MeasureString(Label, Font).Width + bw;
                        if (logoSvg == null && logo == null) return new Size(textSize + subSize + Padding.Horizontal, (int)(size.Height + (size.Height * .66F) + Padding.Vertical));
                        else
                        {
                            int logosize = (int)(size.Height * 1.12F), logosp = (int)(logosize * .274F);
                            return new Size(textSize + subSize + logosize + logosp + Padding.Horizontal, (int)(size.Height + (size.Height * .66F) + Padding.Vertical));
                        }
                    }
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