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
    /// Switch 开关
    /// </summary>
    /// <remarks>开关选择器。</remarks>
    [Description("Switch 开关")]
    [ToolboxItem(true)]
    [DefaultProperty("Checked")]
    [DefaultEvent("CheckedChanged")]
    public class Switch : IControl
    {
        public Switch() : base(ControlType.Select) { }

        #region 属性

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

        Color? fill;
        /// <summary>
        /// 颜色
        /// </summary>
        [Description("颜色"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? Fill
        {
            get => fill;
            set
            {
                if (fill == value) return;
                fill = value;
                Invalidate();
                OnPropertyChanged(nameof(Fill));
            }
        }

        /// <summary>
        /// 悬停颜色
        /// </summary>
        [Description("悬停颜色"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? FillHover { get; set; }

        bool AnimationCheck = false;
        float AnimationCheckValue = 0;

        bool _checked = false;
        /// <summary>
        /// 选中状态
        /// </summary>
        [Description("选中状态"), Category(nameof(CategoryAttribute.Data)), DefaultValue(false)]
        public bool Checked
        {
            get => _checked;
            set
            {
                if (_checked == value) return;
                _checked = value;
                ThreadCheck?.Dispose();
                if (IsHandleCreated && Config.HasAnimation(nameof(Switch), Name))
                {
                    AnimationCheck = true;
                    ThreadCheck = new AnimationTask(new AnimationLinearFConfig(this, i =>
                    {
                        AnimationCheckValue = i;
                        Invalidate();
                        return true;
                    }, 10).SetValue(AnimationCheckValue, value, 0.1F).SetEnd(() => AnimationCheck = false));
                }
                else AnimationCheckValue = value ? 1F : 0F;
                Invalidate();
                OnCheckedChanged(value);
                OnPropertyChanged(nameof(Checked));
            }
        }

        /// <summary>
        /// 点击时自动改变选中状态
        /// </summary>
        [Description("点击时自动改变选中状态"), Category(nameof(CategoryAttribute.Behavior)), DefaultValue(true)]
        public bool AutoCheck { get; set; } = true;

        /// <summary>
        /// 波浪大小
        /// </summary>
        [Description("波浪大小"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(4)]
        public int WaveSize { get; set; } = 4;

        [Description("间距"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(2)]
        public int Gap { get; set; } = 2;

        string? _checkedText, _unCheckedText;

        [Description("选中时显示的文本"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null)]
        [Localizable(true)]
        public string? CheckedText
        {
            get => this.GetLangI(LocalizationCheckedText, _checkedText);
            set
            {
                if (_checkedText == value) return;
                _checkedText = value;
                if (_checked) Invalidate();
                OnPropertyChanged(nameof(CheckedText));
            }
        }

        [Description("选中时显示的文本"), Category("国际化"), DefaultValue(null)]
        public string? LocalizationCheckedText { get; set; }

        [Description("未选中时显示的文本"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(null)]
        [Localizable(true)]
        public string? UnCheckedText
        {
            get => this.GetLangI(LocalizationUnCheckedText, _unCheckedText);
            set
            {
                if (_unCheckedText == value) return;
                _unCheckedText = value;
                if (!_checked) Invalidate();
                OnPropertyChanged(nameof(UnCheckedText));
            }
        }

        [Description("未选中时显示的文本"), Category("国际化"), DefaultValue(null)]
        public string? LocalizationUnCheckedText { get; set; }

        #region 加载中

        bool loading = false;
        /// <summary>
        /// 加载中
        /// </summary>
        [Description("加载中"), Category(nameof(CategoryAttribute.Appearance)), DefaultValue(false)]
        public bool Loading
        {
            get => loading;
            set
            {
                if (loading == value) return;
                loading = value;
                if (IsHandleCreated)
                {
                    if (loading)
                    {
                        bool ProgState = false;
                        ThreadLoading = new AnimationTask(new AnimationLoopConfig(this, () =>
                        {
                            if (ProgState)
                            {
                                LineAngle = LineAngle.Calculate(9F);
                                LineWidth = LineWidth.Calculate(0.6F);
                                if (LineWidth > 75) ProgState = false;
                            }
                            else
                            {
                                LineAngle = LineAngle.Calculate(9.6F);
                                LineWidth = LineWidth.Calculate(-0.6F);
                                if (LineWidth < 6) ProgState = true;
                            }
                            if (LineAngle >= 360) LineAngle = 0;
                            Invalidate();
                            return true;
                        }, 10).SetPriority());
                    }
                    else ThreadLoading?.Dispose();
                }
                Invalidate();
            }
        }

        AnimationTask? ThreadLoading;
        internal float LineWidth = 6, LineAngle = 0;

        #endregion

        #endregion

        #region 事件

        /// <summary>
        /// Checked 属性值更改时发生
        /// </summary>
        [Description("Checked 属性值更改时发生"), Category(nameof(CategoryAttribute.Behavior))]
        public event BoolEventHandler? CheckedChanged;

        protected virtual void OnCheckedChanged(bool e) => CheckedChanged?.Invoke(this, new BoolEventArgs(e));

        #endregion

        #region 渲染

        bool init = false;
        protected override void OnDraw(DrawEventArgs e)
        {
            init = true;
            var g = e.Canvas;
            var rect = e.Rect.PaddingRect(Padding);
            var rect_read = ReadRectangle;
            bool enabled = Enabled;
            using (var path = rect_read.RoundPath(rect_read.Height))
            {
                Color _color = fill ?? Colour.Primary.Get(ColorScheme, nameof(Switch), Name);
                PaintClick(g, path, rect, rect_read, _color);
                if (enabled && (hasFocus && Config.FocusBorderEnabled) && WaveSize > 0)
                {
                    float wave = (WaveSize * Dpi / 2), wave2 = wave * 2;
                    using (var path_focus = new RectangleF(rect_read.X - wave, rect_read.Y - wave, rect_read.Width + wave2, rect_read.Height + wave2).RoundPath(0, TShape.Round))
                    {
                        g.Draw(Colour.PrimaryBorder.Get(ColorScheme, nameof(Switch), Name), wave, path_focus);
                    }
                }
                using (var brush = new SolidBrush(Colour.TextQuaternary.Get(ColorScheme, nameof(Switch), Name)))
                {
                    g.Fill(brush, path);
                    if (AnimationHover) g.Fill(Helper.ToColorN(AnimationHoverValue, brush.Color), path);
                    else if (ExtraMouseHover) g.Fill(brush, path);
                }
                int gap = (int)(Gap * Dpi), gap2 = gap * 2;
                if (AnimationCheck)
                {
                    var alpha = 255 * AnimationCheckValue;
                    g.Fill(Helper.ToColor(alpha, _color), path);
                    var dot_rect = new RectangleF(rect_read.X + gap + (rect_read.Width - rect_read.Height) * AnimationCheckValue, rect_read.Y + gap, rect_read.Height - gap2, rect_read.Height - gap2);
                    g.FillEllipse(enabled ? Colour.SwitchHandleBg.Get(ColorScheme, nameof(Switch), Name) : Helper.ToColorN(0.65f, Colour.SwitchHandleBg.Get(ColorScheme, nameof(Switch), Name)), dot_rect);
                    if (loading) PaintLoading(g, rect_read, dot_rect, gap, gap2, _color);
                    PaintText(g, null, rect_read, dot_rect);
                }
                else if (_checked)
                {
                    if (enabled) PaintChecked(g, rect_read, path, gap, gap2, Colour.SwitchHandleBg.Get(ColorScheme, nameof(Switch), Name), _color);
                    else PaintChecked(g, rect_read, path, gap, gap2, Helper.ToColorN(0.8f, Colour.SwitchHandleBg.Get(ColorScheme, nameof(Switch), Name)), Helper.ToColorN(0.65f, _color));
                }
                else
                {
                    if (enabled) PaintUnChecked(g, rect_read, gap, gap2, Colour.SwitchHandleBg.Get(ColorScheme, nameof(Switch), Name), _color);
                    else PaintUnChecked(g, rect_read, gap, gap2, Helper.ToColorN(0.8f, Colour.SwitchHandleBg.Get(ColorScheme, nameof(Switch), Name)), Helper.ToColorN(0.65f, _color));
                }
            }
            base.OnDraw(e);
        }

        void PaintChecked(Canvas g, Rectangle rect, GraphicsPath path, int gap, int gap2, Color handBg, Color color)
        {
            var colorhover = FillHover ?? Colour.PrimaryHover.Get(ColorScheme, nameof(Switch), Name);
            g.Fill(color, path);
            if (AnimationHover) g.Fill(Helper.ToColorN(AnimationHoverValue, colorhover), path);
            else if (ExtraMouseHover) g.Fill(colorhover, path);
            var dot_rect = new RectangleF(rect.X + gap + rect.Width - rect.Height, rect.Y + gap, rect.Height - gap2, rect.Height - gap2);
            g.FillEllipse(handBg, dot_rect);
            if (loading) PaintLoading(g, rect, dot_rect, gap, gap2, color);
            PaintText(g, true, rect, dot_rect);
        }
        void PaintUnChecked(Canvas g, Rectangle rect, int gap, int gap2, Color handBg, Color color)
        {
            var dot_rect = new RectangleF(rect.X + gap, rect.Y + gap, rect.Height - gap2, rect.Height - gap2);
            g.FillEllipse(handBg, dot_rect);
            if (loading) PaintLoading(g, rect, dot_rect, gap, gap2, color);
            PaintText(g, false, rect, dot_rect);
        }
        void PaintLoading(Canvas g, Rectangle rect, RectangleF dot_rect, int gap, int gap2, Color color)
        {
            var loading_rect = new RectangleF(dot_rect.X + gap, dot_rect.Y + gap, dot_rect.Height - gap2, dot_rect.Height - gap2);
            float size = rect.Height * .1F;
            using (var brush = new Pen(color, size))
            {
                brush.StartCap = brush.EndCap = LineCap.Round;
                g.DrawArc(brush, loading_rect, LineAngle, LineWidth * 3.6F);
            }
        }

        /// <summary>
        /// 绘制文本
        /// </summary>
        void PaintText(Canvas g, bool? check, Rectangle rect, RectangleF dot_rect)
        {
            if (check.HasValue)
            {
                if (check.Value)
                {
                    var text = CheckedText;
                    if (text == null) return;
                    int padd = (int)(dot_rect.Height * 1.1F);
                    g.DrawText(text, Font, fore ?? Colour.PrimaryColor.Get(ColorScheme, nameof(Switch), Name), new Rectangle(rect.X, rect.Y, rect.Width - padd, rect.Height));
                }
                else
                {
                    var text = UnCheckedText;
                    if (text == null) return;
                    int padd = (int)(dot_rect.Height * 1.1F);
                    g.DrawText(text, Font, fore ?? Colour.PrimaryColor.Get(ColorScheme, nameof(Switch), Name), new Rectangle(rect.X + padd, rect.Y, rect.Width - padd, rect.Height));
                }
            }
            else
            {
                g.SetClip(rect);
                string? text = CheckedText, untext = UnCheckedText;
                if (text == null && untext == null) return;
                int padd = (int)(dot_rect.Height * 1.1F), prog = (int)(rect.Width * AnimationCheckValue);
                using (var brush = new SolidBrush(fore ?? Colour.PrimaryColor.Get(ColorScheme, nameof(Switch), Name)))
                {
                    int tmp = rect.Width - padd;
                    g.DrawText(untext, Font, brush, new Rectangle((rect.X + padd) + prog, rect.Y, tmp, rect.Height));
                    g.DrawText(text, Font, brush, new Rectangle(rect.X + prog - rect.Width, rect.Y, tmp, rect.Height));
                }
                g.ResetClip();
            }
        }

        internal void PaintClick(Canvas g, GraphicsPath path, Rectangle rect, RectangleF rect_read, Color color)
        {
            if (AnimationClick || true)
            {
                float alpha = 100 * (1F - AnimationClickValue),
                    maxw = rect_read.Width + ((rect.Width - rect_read.Width) * AnimationClickValue), maxh = rect_read.Height + ((rect.Height - rect_read.Height) * AnimationClickValue);
                using (var path_click = new RectangleF(rect.X + (rect.Width - maxw) / 2F, rect.Y + (rect.Height - maxh) / 2F, maxw, maxh).RoundPath(maxh))
                {
                    path_click.AddPath(path, false);
                    g.Fill(Helper.ToColor(alpha, color), path_click);
                }
            }
        }

        public override Rectangle ReadRectangle => ClientRectangle.PaddingRect(Padding, WaveSize * Dpi);

        public override GraphicsPath RenderRegion
        {
            get
            {
                var rect_read = ReadRectangle;
                return rect_read.RoundPath(rect_read.Height);
            }
        }

        #endregion

        #region 鼠标

        protected override void OnClick(EventArgs e)
        {
            if (AutoCheck) Checked = !_checked;
            base.OnClick(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            init = false;
            Focus();
            base.OnMouseDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (e.KeyCode is Keys.Space || e.KeyCode is Keys.Enter)
            {
                OnClick(EventArgs.Empty);
                e.Handled = true;
            }
        }

        float AnimationHoverValue = 0;
        bool AnimationHover = false;
        bool _mouseHover = false;
        bool ExtraMouseHover
        {
            get => _mouseHover;
            set
            {
                if (_mouseHover == value) return;
                _mouseHover = value;
                var enabled = Enabled;
                SetCursor(value && enabled);
                if (enabled)
                {
                    if (Config.HasAnimation(nameof(Switch), Name))
                    {
                        ThreadHover?.Dispose();
                        AnimationHover = true;
                        ThreadHover = new AnimationTask(new AnimationLinearFConfig(this, i =>
                        {
                            AnimationHoverValue = i;
                            Invalidate();
                            return true;
                        }, 10).SetValue(AnimationHoverValue, value, 0.1F).SetEnd(() => AnimationHover = false));
                    }
                    else AnimationHoverValue = 255;
                    Invalidate();
                }
            }
        }

        #region 动画

        protected override void Dispose(bool disposing)
        {
            ThreadClick?.Dispose();
            ThreadCheck?.Dispose();
            ThreadHover?.Dispose();
            ThreadLoading?.Dispose();
            base.Dispose(disposing);
        }
        AnimationTask? ThreadHover, ThreadCheck, ThreadClick;

        bool AnimationClick = false;
        float AnimationClickValue = 0;
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (Config.HasAnimation(nameof(Switch), Name) && e.Button == MouseButtons.Left)
            {
                ThreadClick?.Dispose();
                AnimationClickValue = 0;
                AnimationClick = true;
                ThreadClick = new AnimationTask(new AnimationLoopConfig(this, () =>
                {
                    if (AnimationClickValue > 0.6) AnimationClickValue = AnimationClickValue.Calculate(0.04F);
                    else AnimationClickValue = AnimationClickValue.Calculate(0.1F);
                    if (AnimationClickValue > 1) { AnimationClickValue = 0F; return false; }
                    Invalidate();
                    return true;
                }, 50).SetEnd(() =>
                {
                    AnimationClick = false;
                    Invalidate();
                }));
            }
        }

        #endregion

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
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

        #endregion

        #region 焦点

        bool hasFocus = false;
        /// <summary>
        /// 是否存在焦点
        /// </summary>
        [Browsable(false)]
        [Description("是否存在焦点"), Category(nameof(CategoryAttribute.Behavior)), DefaultValue(false)]
        public bool HasFocus
        {
            get => hasFocus;
            private set
            {
                if (value && _mouseHover) value = false;
                if (hasFocus == value) return;
                hasFocus = value;
                Invalidate();
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            if (init) HasFocus = true;
        }

        protected override void OnLostFocus(EventArgs e)
        {
            HasFocus = false;
            base.OnLostFocus(e);
        }

        #endregion
    }
}