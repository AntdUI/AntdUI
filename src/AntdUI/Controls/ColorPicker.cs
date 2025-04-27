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
    /// ColorPicker 颜色选择器
    /// </summary>
    /// <remarks>提供颜色选取的组件。</remarks>
    [Description("ColorPicker 颜色选择器")]
    [ToolboxItem(true)]
    [DefaultProperty("Value")]
    [DefaultEvent("ValueChanged")]
    public class ColorPicker : IControl, SubLayeredForm
    {
        public ColorPicker() : base(ControlType.Select)
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
            }
        }

        #region 背景

        internal Color? back;
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
            }
        }

        #endregion

        #region 边框

        internal float borderWidth = 1F;
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

        /// <summary>
        /// 边框颜色
        /// </summary>
        internal Color? borderColor;
        [Description("边框颜色"), Category("边框"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? BorderColor
        {
            get => borderColor;
            set
            {
                if (borderColor == value) return;
                borderColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 悬停边框颜色
        /// </summary>
        [Description("悬停边框颜色"), Category("边框"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? BorderHover { get; set; }

        /// <summary>
        /// 激活边框颜色
        /// </summary>
        [Description("激活边框颜色"), Category("边框"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? BorderActive { get; set; }

        #endregion

        /// <summary>
        /// 波浪大小
        /// </summary>
        [Description("波浪大小"), Category("外观"), DefaultValue(4)]
        public int WaveSize { get; set; } = 4;

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
            }
        }

        Color _value = Colour.Primary.Get("ColorPicker");
        [Description("颜色的值"), Category("值"), DefaultValue(typeof(Color), "Transparent")]
        public Color Value
        {
            get => _value;
            set
            {
                hasvalue = true;
                if (value == _value) return;
                if (DisabledAlpha && value.A != 255) value = Color.FromArgb(255, value);
                _value = value;
                if (BeforeAutoSize()) Invalidate();
                ValueChanged?.Invoke(this, new ColorEventArgs(value));
                OnPropertyChanged(nameof(Value));
            }
        }

        bool showText = false;
        /// <summary>
        /// 显示Hex文字
        /// </summary>
        [Description("显示Hex文字"), Category("值"), DefaultValue(false)]
        public bool ShowText
        {
            get => showText;
            set
            {
                if (showText == value) return;
                showText = value;
                if (BeforeAutoSize()) Invalidate();
            }
        }

        /// <summary>
        /// 禁用透明度
        /// </summary>
        [Description("禁用透明度"), Category("行为"), DefaultValue(false)]
        public bool DisabledAlpha { get; set; }

        bool allowclear = false;
        /// <summary>
        /// 支持清除
        /// </summary>
        [Description("支持清除"), Category("行为"), DefaultValue(false)]
        public bool AllowClear
        {
            get => allowclear;
            set
            {
                if (allowclear == value) return;
                allowclear = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 显示关闭按钮
        /// </summary>
        [Description("显示关闭按钮"), Category("行为"), DefaultValue(false)]
        public bool ShowClose { get; set; }

        bool hasvalue = false;
        /// <summary>
        /// 是否包含值
        /// </summary>
        public bool HasValue
        {
            get
            {
                if (allowclear) return hasvalue;
                return true;
            }
        }

        /// <summary>
        /// 获取颜色值
        /// </summary>
        public Color? ValueClear
        {
            get
            {
                if (allowclear && !hasvalue) return null;
                return _value;
            }
        }

        /// <summary>
        /// 清空值
        /// </summary>
        public void ClearValue() => ClearValue(Colour.Primary.Get("ColorPicker", ColorScheme));

        /// <summary>
        /// 清空值
        /// </summary>
        /// <param name="def">默认色</param>
        public void ClearValue(Color def)
        {
            if (allowclear)
            {
                if (hasvalue)
                {
                    hasvalue = false;
                    _value = def;
                    Invalidate();
                    ValueChanged?.Invoke(this, new ColorEventArgs(_value));
                }
                else _value = def;
            }
        }

        TColorMode mode = TColorMode.Hex;
        /// <summary>
        /// 颜色模式
        /// </summary>
        [Description("颜色模式"), Category("行为"), DefaultValue(TColorMode.Hex)]
        public TColorMode Mode
        {
            get => mode;
            set
            {
                if (mode == value) return;
                mode = value;
                if (BeforeAutoSize()) Invalidate();
            }
        }

        /// <summary>
        /// 触发下拉的行为
        /// </summary>
        [Description("触发下拉的行为"), Category("行为"), DefaultValue(Trigger.Click)]
        public Trigger Trigger { get; set; } = Trigger.Click;

        /// <summary>
        /// 菜单弹出位置
        /// </summary>
        [Description("菜单弹出位置"), Category("行为"), DefaultValue(TAlignFrom.BL)]
        public TAlignFrom Placement { get; set; } = TAlignFrom.BL;

        /// <summary>
        /// 下拉箭头是否显示
        /// </summary>
        [Description("下拉箭头是否显示"), Category("外观"), DefaultValue(true)]
        public bool DropDownArrow { get; set; } = true;

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

        #region 事件

        /// <summary>
        /// Value 属性值更改时发生
        /// </summary>
        [Description("Value 属性值更改时发生"), Category("行为")]
        public event ColorEventHandler? ValueChanged = null;

        /// <summary>
        /// Value格式化时发生
        /// </summary>
        [Description("Value格式化时发生"), Category("行为")]
        public event ColorFormatEventHandler? ValueFormatChanged;

        #endregion

        #region 渲染

        bool init = false;
        internal StringFormat stringLeft = Helper.SF_NoWrap(lr: StringAlignment.Near);
        protected override void OnPaint(PaintEventArgs e)
        {
            init = true;
            var rect = ClientRectangle.PaddingRect(Padding);
            var g = e.Graphics.High();
            var rect_read = ReadRectangle;
            float _radius = round ? rect_read.Height : radius * Config.Dpi;
            using (var path = Path(rect_read, _radius))
            {
                Color _fore = fore ?? Colour.Text.Get("ColorPicker", ColorScheme), _back = back ?? Colour.BgContainer.Get("ColorPicker", ColorScheme),
                    _border = borderColor ?? Colour.BorderColor.Get("ColorPicker", ColorScheme),
                    _borderHover = BorderHover ?? Colour.PrimaryHover.Get("ColorPicker", ColorScheme),
                _borderActive = BorderActive ?? Colour.Primary.Get("ColorPicker", ColorScheme);
                PaintClick(g, path, rect, _borderActive, _radius);
                int size_color = (int)(rect_read.Height * .75F);
                if (Enabled)
                {
                    if (hasFocus && WaveSize > 0)
                    {
                        float wave = (WaveSize * Config.Dpi / 2), wave2 = wave * 2;
                        using (var path_focus = new RectangleF(rect_read.X - wave, rect_read.Y - wave, rect_read.Width + wave2, rect_read.Height + wave2).RoundPath(_radius + wave))
                        {
                            g.Draw(Colour.PrimaryBorder.Get("ColorPicker", ColorScheme), wave, path_focus);
                        }
                    }
                    g.Fill(_back, path);
                    if (borderWidth > 0)
                    {
                        var borWidth = borderWidth * Config.Dpi;
                        if (AnimationHover) g.Draw(_border.BlendColors(AnimationHoverValue, _borderHover), borWidth, path);
                        else if (ExtraMouseDown) g.Draw(_borderActive, borWidth, path);
                        else if (ExtraMouseHover) g.Draw(_borderHover, borWidth, path);
                        else g.Draw(_border, borWidth, path);
                    }
                }
                else
                {
                    _fore = Colour.TextQuaternary.Get("ColorPicker", ColorScheme);
                    g.Fill(Colour.FillTertiary.Get("ColorPicker", ColorScheme), path);
                    if (borderWidth > 0) g.Draw(_border, borderWidth * Config.Dpi, path);
                }
                var r = _radius * .75F;
                if (showText)
                {
                    int gap = (rect_read.Height - size_color) / 2;
                    var rect_color = new Rectangle(rect_read.X + gap, rect_read.Y + gap, size_color, size_color);
                    PaintValue(g, r, rect_color);
                    using (var brush = new SolidBrush(_fore))
                    {
                        var wi = gap * 2 + size_color;
                        if (ValueFormatChanged == null)
                        {
                            if (HasValue)
                            {
                                switch (mode)
                                {
                                    case TColorMode.Hex:
                                        g.String("#" + _value.ToHex(), Font, brush, new Rectangle(rect_read.X + wi, rect_read.Y, rect_read.Width - wi, rect_read.Height), stringLeft);
                                        break;
                                    case TColorMode.Rgb:
                                        if (_value.A == 255) g.String(string.Format("rgb({0},{1},{2})", _value.R, _value.G, _value.B), Font, brush, new Rectangle(rect_read.X + wi, rect_read.Y, rect_read.Width - wi, rect_read.Height), stringLeft);
                                        else g.String(string.Format("rgba({0},{1},{2},{3})", _value.R, _value.G, _value.B, Math.Round(_value.A / 255D, 2)), Font, brush, new Rectangle(rect_read.X + wi, rect_read.Y, rect_read.Width - wi, rect_read.Height), stringLeft);
                                        break;
                                }
                            }
                            else g.String("Transparent", Font, brush, new Rectangle(rect_read.X + wi, rect_read.Y, rect_read.Width - wi, rect_read.Height), stringLeft);
                        }
                        else g.String(ValueFormatChanged(this, new ColorEventArgs(_value)), Font, brush, new Rectangle(rect_read.X + wi, rect_read.Y, rect_read.Width - wi, rect_read.Height), stringLeft);
                    }
                }
                else
                {
                    int size_colorw = (int)(rect_read.Width * .75F);
                    var rect_color = new RectangleF(rect_read.X + (rect_read.Width - size_colorw) / 2F, rect_read.Y + (rect_read.Height - size_color) / 2F, size_colorw, size_color);
                    PaintValue(g, r, rect_color);
                }
            }
            this.PaintBadge(g);
            base.OnPaint(e);
        }

        void PaintValue(Canvas g, float r, RectangleF rect_color)
        {
            using (var path = rect_color.RoundPath(r))
            {
                var has = HasValue;
                if (allowclear && !hasvalue)
                {
                    g.SetClip(path);
                    using (var pen = new Pen(Color.FromArgb(245, 34, 45), rect_color.Height * .12F))
                    {
                        g.DrawLine(pen, new PointF(rect_color.X, rect_color.Bottom), new PointF(rect_color.Right, rect_color.Y));
                    }
                    g.ResetClip();
                    g.Draw(Colour.Split.Get("ColorPicker", ColorScheme), Config.Dpi, path);
                }
                else
                {
                    PaintAlpha(g, r, rect_color);
                    g.Fill(_value, path);
                }
            }
        }

        Bitmap? bmp_alpha = null;
        void PaintAlpha(Canvas g, float radius, RectangleF rect)
        {
            if (bmp_alpha == null || bmp_alpha.Width != rect.Width || bmp_alpha.Height != rect.Height)
            {
                bmp_alpha?.Dispose();
                bmp_alpha = new Bitmap((int)rect.Width, (int)rect.Height);
                using (var tmp = new Bitmap(bmp_alpha.Width, bmp_alpha.Height))
                {
                    using (var g2 = Graphics.FromImage(tmp).High())
                    {
                        PaintAlpha(g2, rect);
                    }
                    using (var g2 = Graphics.FromImage(bmp_alpha).High())
                    {
                        g2.Image(new Rectangle(0, 0, bmp_alpha.Width, bmp_alpha.Height), tmp, TFit.Fill, radius, false);
                    }
                }
            }
            g.Image(bmp_alpha, rect);
        }

        void PaintAlpha(Canvas g, RectangleF rect)
        {
            float u_y = 0, size = rect.Height / 4;
            bool ad = false;
            using (var brush = new SolidBrush(Colour.FillSecondary.Get("ColorPicker", ColorScheme)))
            {
                while (u_y < rect.Height)
                {
                    float u_x = 0;
                    bool adsub = ad;
                    while (u_x < rect.Width)
                    {
                        if (adsub) g.Fill(brush, new RectangleF(u_x, u_y, size, size));
                        u_x += size;
                        adsub = !adsub;
                    }
                    u_y += size;
                    ad = !ad;
                }
            }
        }

        #region 渲染帮助

        internal GraphicsPath Path(Rectangle rect_read, float _radius)
        {
            if (JoinLeft && JoinRight) return rect_read.RoundPath(0);
            else if (JoinRight) return rect_read.RoundPath(_radius, true, false, false, true);
            else if (JoinLeft) return rect_read.RoundPath(_radius, false, true, true, false);
            return rect_read.RoundPath(_radius);
        }

        #region 点击动画

        internal void PaintClick(Canvas g, GraphicsPath path, Rectangle rect, Color color, float radius)
        {
            if (AnimationFocus)
            {
                if (AnimationFocusValue > 0)
                {
                    using (var path_click = rect.RoundPath(radius, round))
                    {
                        path_click.AddPath(path, false);
                        g.Fill(Helper.ToColor(AnimationFocusValue, color), path_click);
                    }
                }
            }
            else if (ExtraMouseDown && WaveSize > 0)
            {
                using (var path_click = rect.RoundPath(radius, round))
                {
                    path_click.AddPath(path, false);
                    g.Fill(Color.FromArgb(30, color), path_click);
                }
            }
        }

        #endregion

        #endregion

        public override Rectangle ReadRectangle => ClientRectangle.PaddingRect(Padding).ReadRect((WaveSize + borderWidth / 2F) * Config.Dpi, JoinLeft, JoinRight);

        public override GraphicsPath RenderRegion
        {
            get
            {
                var rect_read = ReadRectangle;
                float _radius = round ? rect_read.Height : radius * Config.Dpi;
                return Path(rect_read, _radius);
            }
        }

        #endregion

        #region 事件/焦点

        bool AnimationFocus = false;
        int AnimationFocusValue = 0;
        bool hasFocus = false;
        /// <summary>
        /// 是否存在焦点
        /// </summary>
        [Browsable(false)]
        [Description("是否存在焦点"), Category("行为"), DefaultValue(false)]
        public bool HasFocus
        {
            get => hasFocus;
            private set
            {
                if (value && (_mouseDown || _mouseHover)) value = false;
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
            base.OnLostFocus(e);
            HasFocus = false;
        }

        #endregion

        #region 鼠标

        internal bool _mouseDown = false;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal bool ExtraMouseDown
        {
            get => _mouseDown;
            set
            {
                if (_mouseDown == value) return;
                _mouseDown = value;
                if (Config.HasAnimation(nameof(ColorPicker)) && WaveSize > 0)
                {
                    ThreadFocus?.Dispose();
                    AnimationFocus = true;
                    if (value)
                    {
                        ThreadFocus = new ITask(this, () =>
                        {
                            AnimationFocusValue += 4;
                            if (AnimationFocusValue > 30) return false;
                            Invalidate();
                            return true;
                        }, 20, () =>
                        {
                            AnimationFocus = false;
                            Invalidate();
                        });
                    }
                    else
                    {
                        ThreadFocus = new ITask(this, () =>
                        {
                            AnimationFocusValue -= 4;
                            if (AnimationFocusValue < 1) return false;
                            Invalidate();
                            return true;
                        }, 20, () =>
                        {
                            AnimationFocus = false;
                            Invalidate();
                        });
                    }
                }
                else Invalidate();
            }
        }

        internal int AnimationHoverValue = 0;
        internal bool AnimationHover = false;
        internal bool _mouseHover = false;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal bool ExtraMouseHover
        {
            get => _mouseHover;
            set
            {
                if (_mouseHover == value) return;
                _mouseHover = value;
                if (Enabled)
                {
                    if (Config.HasAnimation(nameof(ColorPicker)) && !ExtraMouseDown)
                    {
                        ThreadHover?.Dispose();
                        AnimationHover = true;
                        if (value)
                        {
                            ThreadHover = new ITask(this, () =>
                            {
                                AnimationHoverValue += 20;
                                if (AnimationHoverValue > 255) { AnimationHoverValue = 255; return false; }
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
                                AnimationHoverValue -= 20;
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
                    else AnimationHoverValue = 255;
                    Invalidate();
                }
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            SetCursor(true);
            base.OnMouseEnter(e);
            ExtraMouseHover = true;
            if (Trigger == Trigger.Hover && subForm == null) ClickDown();
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            SetCursor(false);
            base.OnMouseLeave(e);
            ExtraMouseHover = false;
        }
        protected override void OnLeave(EventArgs e)
        {
            SetCursor(false);
            base.OnLeave(e);
            ExtraMouseHover = false;
        }


        LayeredFormColorPicker? subForm = null;
        public ILayeredForm? SubForm() => subForm;
        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && Trigger == Trigger.Click)
            {
                init = false;
                ImeMode = ImeMode.Disable;
                Focus();
                ClickDown();
            }
            base.OnMouseClick(e);
        }

        void ClickDown()
        {
            ExtraMouseDown = true;
            if (subForm == null)
            {
                subForm = new LayeredFormColorPicker(this, ReadRectangle, color =>
                {
                    Value = color;
                });
                subForm.Disposed += (a, b) =>
                {
                    ExtraMouseDown = false;
                    subForm = null;
                };
                subForm.Show(this);
            }
        }

        #endregion

        #region 释放/动画

        protected override void Dispose(bool disposing)
        {
            ThreadFocus?.Dispose();
            ThreadHover?.Dispose();
            base.Dispose(disposing);
        }
        ITask? ThreadHover = null;
        ITask? ThreadFocus = null;

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
                    Size font_size;
                    if (ValueFormatChanged == null)
                    {
                        if (HasValue)
                        {
                            switch (mode)
                            {
                                case TColorMode.Rgb:
                                    font_size = g.MeasureString(_value.A == 255 ? "rgb(255,255,255)" : "rgba(255,255,255,0.99)", Font);
                                    break;
                                case TColorMode.Hex:
                                default:
                                    font_size = g.MeasureString(_value.A == 255 ? "#DDDCCC" : "#DDDDCCCC", Font);
                                    break;
                            }
                        }
                        else font_size = g.MeasureString("Transparent", Font);
                    }
                    else font_size = g.MeasureString(ValueFormatChanged(this, new ColorEventArgs(_value)), Font);
                    int gap = (int)((20 + WaveSize) * Config.Dpi);
                    if (showText)
                    {
                        int s = font_size.Height + gap;
                        return new Size(s + font_size.Width, s);
                    }
                    else
                    {
                        int s = font_size.Height + gap;
                        return new Size(s, s);
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

        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)
        {
            subForm?.IProcessCmdKey(ref msg, keyData);
            return base.ProcessCmdKey(ref msg, keyData);
        }
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            subForm?.IKeyPress(e);
            base.OnKeyPress(e);
        }
    }
}