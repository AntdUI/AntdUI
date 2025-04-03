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

        Color? fill;
        /// <summary>
        /// 颜色
        /// </summary>
        [Description("颜色"), Category("外观"), DefaultValue(null)]
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
        [Description("悬停颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? FillHover { get; set; }

        bool AnimationCheck = false;
        float AnimationCheckValue = 0;

        bool _checked = false;
        /// <summary>
        /// 选中状态
        /// </summary>
        [Description("选中状态"), Category("数据"), DefaultValue(false)]
        public bool Checked
        {
            get => _checked;
            set
            {
                if (_checked == value) return;
                _checked = value;
                ThreadCheck?.Dispose();
                if (IsHandleCreated && Config.HasAnimation(nameof(Switch)))
                {
                    AnimationCheck = true;
                    if (value)
                    {
                        ThreadCheck = new ITask(this, () =>
                        {
                            AnimationCheckValue = AnimationCheckValue.Calculate(0.1F);
                            if (AnimationCheckValue > 1) { AnimationCheckValue = 1F; return false; }
                            Invalidate();
                            return true;
                        }, 10, () =>
                        {
                            AnimationCheck = false;
                            Invalidate();
                        });
                    }
                    else
                    {
                        ThreadCheck = new ITask(this, () =>
                        {
                            AnimationCheckValue = AnimationCheckValue.Calculate(-0.1F);
                            if (AnimationCheckValue <= 0) { AnimationCheckValue = 0F; return false; }
                            Invalidate();
                            return true;
                        }, 10, () =>
                        {
                            AnimationCheck = false;
                            Invalidate();
                        });
                    }
                }
                else AnimationCheckValue = value ? 1F : 0F;
                Invalidate();
                CheckedChanged?.Invoke(this, new BoolEventArgs(value));
                OnPropertyChanged(nameof(Checked));
            }
        }

        /// <summary>
        /// 点击时自动改变选中状态
        /// </summary>
        [Description("点击时自动改变选中状态"), Category("行为"), DefaultValue(true)]
        public bool AutoCheck { get; set; } = true;

        /// <summary>
        /// 波浪大小
        /// </summary>
        [Description("波浪大小"), Category("外观"), DefaultValue(4)]
        public int WaveSize { get; set; } = 4;

        [Description("间距"), Category("外观"), DefaultValue(2)]
        public int Gap { get; set; } = 2;

        string? _checkedText = null, _unCheckedText = null;

        [Description("选中时显示的文本"), Category("外观"), DefaultValue(null)]
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

        [Description("未选中时显示的文本"), Category("外观"), DefaultValue(null)]
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
        [Description("加载中"), Category("外观"), DefaultValue(false)]
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
                        ThreadLoading = new ITask(this, () =>
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
                        }, 10);
                    }
                    else ThreadLoading?.Dispose();
                }
                Invalidate();
            }
        }

        ITask? ThreadLoading = null;
        internal float LineWidth = 6, LineAngle = 0;

        #endregion

        #endregion

        #region 事件

        /// <summary>
        /// Checked 属性值更改时发生
        /// </summary>
        [Description("Checked 属性值更改时发生"), Category("行为")]
        public event BoolEventHandler? CheckedChanged = null;

        #endregion

        #region 渲染

        bool init = false;
        protected override void OnPaint(PaintEventArgs e)
        {
            init = true;
            var g = e.Graphics.High();
            var rect = ClientRectangle.PaddingRect(Padding);
            var rect_read = ReadRectangle;
            bool enabled = Enabled;
            using (var path = rect_read.RoundPath(rect_read.Height))
            {
                Color _color = fill ?? Colour.Primary.Get("Switch");
                PaintClick(g, path, rect, rect_read, _color);
                if (enabled && hasFocus && WaveSize > 0)
                {
                    float wave = (WaveSize * Config.Dpi / 2), wave2 = wave * 2;
                    using (var path_focus = new RectangleF(rect_read.X - wave, rect_read.Y - wave, rect_read.Width + wave2, rect_read.Height + wave2).RoundPath(0, TShape.Round))
                    {
                        g.Draw(Colour.PrimaryBorder.Get("Switch"), wave, path_focus);
                    }
                }
                using (var brush = new SolidBrush(Colour.TextQuaternary.Get("Switch")))
                {
                    g.Fill(brush, path);
                    if (AnimationHover) g.Fill(Helper.ToColorN(AnimationHoverValue, brush.Color), path);
                    else if (ExtraMouseHover) g.Fill(brush, path);
                }
                int gap = (int)(Gap * Config.Dpi), gap2 = gap * 2;
                if (AnimationCheck)
                {
                    var alpha = 255 * AnimationCheckValue;
                    g.Fill(Helper.ToColor(alpha, _color), path);
                    var dot_rect = new RectangleF(rect_read.X + gap + (rect_read.Width - rect_read.Height) * AnimationCheckValue, rect_read.Y + gap, rect_read.Height - gap2, rect_read.Height - gap2);
                    g.FillEllipse(enabled ? Colour.BgBase.Get("Switch") : Color.FromArgb(200, Colour.BgBase.Get("Switch")), dot_rect);
                    if (loading)
                    {
                        var dot_rect2 = new RectangleF(dot_rect.X + gap, dot_rect.Y + gap, dot_rect.Height - gap2, dot_rect.Height - gap2);
                        float size = rect_read.Height * .1F;
                        using (var brush = new Pen(_color, size))
                        {
                            brush.StartCap = brush.EndCap = LineCap.Round;
                            g.DrawArc(brush, dot_rect2, LineAngle, LineWidth * 3.6F);
                        }
                    }
                }
                else if (_checked)
                {
                    var colorhover = FillHover ?? Colour.PrimaryHover.Get("Switch");
                    g.Fill(enabled ? _color : Color.FromArgb(200, _color), path);
                    if (AnimationHover) g.Fill(Helper.ToColorN(AnimationHoverValue, colorhover), path);
                    else if (ExtraMouseHover) g.Fill(colorhover, path);
                    var dot_rect = new RectangleF(rect_read.X + gap + rect_read.Width - rect_read.Height, rect_read.Y + gap, rect_read.Height - gap2, rect_read.Height - gap2);
                    g.FillEllipse(enabled ? Colour.BgBase.Get("Switch") : Color.FromArgb(200, Colour.BgBase.Get("Switch")), dot_rect);
                    if (loading)
                    {
                        var dot_rect2 = new RectangleF(dot_rect.X + gap, dot_rect.Y + gap, dot_rect.Height - gap2, dot_rect.Height - gap2);
                        float size = rect_read.Height * .1F;
                        using (var brush = new Pen(_color, size))
                        {
                            brush.StartCap = brush.EndCap = LineCap.Round;
                            g.DrawArc(brush, dot_rect2, LineAngle, LineWidth * 3.6F);
                        }
                    }
                }
                else
                {
                    var dot_rect = new RectangleF(rect_read.X + gap, rect_read.Y + gap, rect_read.Height - gap2, rect_read.Height - gap2);
                    g.FillEllipse(enabled ? Colour.BgBase.Get("Switch") : Color.FromArgb(200, Colour.BgBase.Get("Switch")), dot_rect);
                    if (loading)
                    {
                        var dot_rect2 = new RectangleF(dot_rect.X + gap, dot_rect.Y + gap, dot_rect.Height - gap2, dot_rect.Height - gap2);
                        float size = rect_read.Height * .1F;
                        using (var brush = new Pen(_color, size))
                        {
                            brush.StartCap = brush.EndCap = LineCap.Round;
                            g.DrawArc(brush, dot_rect2, LineAngle, LineWidth * 3.6F);
                        }
                    }
                }

                // 绘制文本
                string? textToRender = Checked ? CheckedText : UnCheckedText;
                if (textToRender != null)
                {
                    Color _fore = fore ?? Colour.PrimaryColor.Get("Switch");
                    using (var brush = new SolidBrush(_fore))
                    {
                        var textSize = g.MeasureString(textToRender, Font);
                        var textRect = Checked
                            ? new Rectangle(rect_read.X + (rect_read.Width - rect_read.Height + gap2) / 2 - textSize.Width / 2, rect_read.Y + rect_read.Height / 2 - textSize.Height / 2, textSize.Width, textSize.Height)
                            : new Rectangle(rect_read.X + (rect_read.Height - gap + (rect_read.Width - rect_read.Height + gap) / 2 - textSize.Width / 2), rect_read.Y + rect_read.Height / 2 - textSize.Height / 2, textSize.Width, textSize.Height);
                        g.String(textToRender, Font, brush, textRect);
                    }
                }
            }
            this.PaintBadge(g);
            base.OnPaint(e);
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

        public override Rectangle ReadRectangle => ClientRectangle.PaddingRect(Padding, WaveSize * Config.Dpi);

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
                    if (Config.HasAnimation(nameof(Switch)))
                    {
                        ThreadHover?.Dispose();
                        AnimationHover = true;
                        if (value)
                        {
                            ThreadHover = new ITask(this, () =>
                            {
                                AnimationHoverValue = AnimationHoverValue.Calculate(0.1F);
                                if (AnimationHoverValue > 1) { AnimationHoverValue = 1F; return false; }
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
                                AnimationHoverValue = AnimationHoverValue.Calculate(-0.1F);
                                if (AnimationHoverValue <= 0) { AnimationHoverValue = 0F; return false; }
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

        #region 动画

        protected override void Dispose(bool disposing)
        {
            ThreadClick?.Dispose();
            ThreadCheck?.Dispose();
            ThreadHover?.Dispose();
            ThreadLoading?.Dispose();
            base.Dispose(disposing);
        }
        ITask? ThreadHover = null;
        ITask? ThreadCheck = null;
        ITask? ThreadClick = null;

        bool AnimationClick = false;
        float AnimationClickValue = 0;
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (Config.HasAnimation(nameof(Switch)) && e.Button == MouseButtons.Left)
            {
                ThreadClick?.Dispose();
                AnimationClickValue = 0;
                AnimationClick = true;
                ThreadClick = new ITask(this, () =>
                {
                    if (AnimationClickValue > 0.6) AnimationClickValue = AnimationClickValue.Calculate(0.04F);
                    else AnimationClickValue = AnimationClickValue.Calculate(0.1F);
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
        [Description("是否存在焦点"), Category("行为"), DefaultValue(false)]
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
            base.OnLostFocus(e);
            HasFocus = false;
        }

        #endregion
    }
}