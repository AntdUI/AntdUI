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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// Progress 进度条
    /// </summary>
    /// <remarks>展示操作的当前进度。</remarks>
    [Description("Progress 进度条")]
    [ToolboxItem(true)]
    [DefaultProperty("Value")]
    public class Progress : IControl
    {
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

        #region 背景

        Color? back;
        /// <summary>
        /// 背景颜色
        /// </summary>
        [Description("背景颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? Back
        {
            get => back;
            set
            {
                if (back == value) return;
                back = value;
                Invalidate();
                OnPropertyChanged(nameof(Back));
            }
        }

        Color? fill;
        /// <summary>
        /// 进度条颜色
        /// </summary>
        [Description("进度条颜色"), Category("外观"), DefaultValue(null)]
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

        #endregion

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

        TShapeProgress shape = TShapeProgress.Round;
        /// <summary>
        /// 形状
        /// </summary>
        [Description("形状"), Category("外观"), DefaultValue(TShapeProgress.Round)]
        public TShapeProgress Shape
        {
            get => shape;
            set
            {
                if (shape == value) return;
                shape = value;
                Invalidate();
                OnPropertyChanged(nameof(Shape));
            }
        }

        #region 显示文本

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
                text = value;
                if (useSystemText) Invalidate();
                OnTextChanged(EventArgs.Empty);
                OnPropertyChanged(nameof(Text));
            }
        }

        [Description("文本"), Category("国际化"), DefaultValue(null)]
        public string? LocalizationText { get; set; }

        string? textUnit = "%";
        /// <summary>
        /// 单位文本
        /// </summary>
        [Description("单位文本"), Category("外观"), DefaultValue("%")]
        [Localizable(true)]
        public string? TextUnit
        {
            get => this.GetLangI(LocalizationTextUnit, textUnit);
            set
            {
                if (textUnit == value) return;
                textUnit = value;
                Invalidate();
                OnPropertyChanged(nameof(TextUnit));
            }
        }

        [Description("单位文本"), Category("国际化"), DefaultValue(null)]
        public string? LocalizationTextUnit { get; set; }

        bool useSystemText = false;
        /// <summary>
        /// 使用系统文本
        /// </summary>
        [Description("使用系统文本"), Category("外观"), DefaultValue(false)]
        public bool UseSystemText
        {
            get => useSystemText;
            set
            {
                if (useSystemText == value) return;
                useSystemText = value;
                Invalidate();
                OnPropertyChanged(nameof(UseSystemText));
            }
        }

        /// <summary>
        /// 使文本居中显示
        /// </summary>
        [Description("使文本居中显示"), Category("外观"), DefaultValue(false)]
        public bool UseTextCenter { get; set; }

        /// <summary>
        /// 显示进度文本小数点位数
        /// </summary>
        [Description("显示进度文本小数点位数"), Category("外观"), DefaultValue(0)]
        public int ShowTextDot { get; set; }

        #endregion

        TType state = TType.None;
        /// <summary>
        /// 状态
        /// </summary>
        [Description("状态"), Category("外观"), DefaultValue(TType.None)]
        public TType State
        {
            get => state;
            set
            {
                if (state == value) return;
                state = value;
                Invalidate();
                if (showInTaskbar) ShowTaskbar();
                OnPropertyChanged(nameof(State));
            }
        }

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
                Invalidate();
                OnPropertyChanged(nameof(IconRatio));
            }
        }

        #region Icon

        Image? icon;
        /// <summary>
        /// 圆形进度下的图标
        /// </summary>
        [Description("圆形进度下的图标"), Category("外观"), DefaultValue(null)]
        public Image? IconCircle
        {
            get => icon;
            set
            {
                if (icon == value) return;
                icon = value;
                if (shape == TShapeProgress.Circle) Invalidate();
            }
        }

        string? iconSvg;
        /// <summary>
        /// 圆形进度下的图标SVG
        /// </summary>
        [Description("圆形进度下的图标SVG"), Category("外观"), DefaultValue(null)]
        public string? IconSvgCircle
        {
            get => iconSvg;
            set
            {
                if (iconSvg == value) return;
                iconSvg = value;
                if (shape == TShapeProgress.Circle) Invalidate();
            }
        }

        /// <summary>
        /// 圆形图标是否旋转
        /// </summary>
        [Description("圆形图标是否旋转"), Category("外观"), DefaultValue(false)]
        public bool IconCircleAngle { get; set; }

        /// <summary>
        /// 圆形图标边距
        /// </summary>
        [Description("圆形图标边距"), Category("外观"), DefaultValue(8)]
        public int IconCirclePadding { get; set; } = 8;

        /// <summary>
        /// 圆形图标颜色
        /// </summary>
        [Description("圆形图标颜色"), Category("外观"), DefaultValue(null)]
        public Color? IconCircleColor { get; set; }

        /// <summary>
        /// 是否包含图片
        /// </summary>
        internal bool HasIcon => iconSvg != null || icon != null;

        #endregion

        float valueratio = .4F;
        /// <summary>
        /// 进度条比例
        /// </summary>
        [Description("进度条比例"), Category("外观"), DefaultValue(.4F)]
        public float ValueRatio
        {
            get => valueratio;
            set
            {
                if (valueratio == value) return;
                valueratio = value;
                Invalidate();
                OnPropertyChanged(nameof(ValueRatio));
            }
        }

        #region 进度条

        float _value = 0F;
        float _value_show = 0F;
        /// <summary>
        /// 进度条
        /// </summary>
        [Description("进度条 0F-1F"), Category("数据"), DefaultValue(0F)]
        public float Value
        {
            get => _value;
            set
            {
                if (_value == value) return;
                if (value < 0) value = 0;
                else if (value > 1) value = 1;
                _value = value;
                ThreadValue?.Dispose();
                ThreadValue = null;
                if (Config.HasAnimation(nameof(Progress)) && IsHandleCreated && Animation > 0)
                {
                    var t = AntdUI.Animation.TotalFrames(10, Animation);
                    if (_value > _value_show)
                    {
                        float s = _value_show, v = Math.Abs(_value - s);
                        ThreadValue = new ITask((i) =>
                        {
                            _value_show = s + AntdUI.Animation.Animate(i, t, v, AnimationType.Ball);
                            Invalidate();
                            return true;
                        }, 10, t, () =>
                        {
                            _value_show = _value;
                            Invalidate();
                        });
                    }
                    else
                    {
                        float s = _value_show, v = Math.Abs(_value_show - _value);
                        ThreadValue = new ITask((i) =>
                        {
                            _value_show = s - AntdUI.Animation.Animate(i, t, v, AnimationType.Ball);
                            Invalidate();
                            return true;
                        }, 10, t, () =>
                        {
                            _value_show = _value;
                            Invalidate();
                        });
                    }
                }
                else
                {
                    _value_show = _value;
                    Invalidate();
                }
                if (showInTaskbar) ShowTaskbar();
                OnPropertyChanged(nameof(Value));
            }
        }

        /// <summary>
        /// Value格式化时发生
        /// </summary>
        [Description("Value格式化时发生"), Category("行为")]
        public event ProgressFormatEventHandler? ValueFormatChanged;

        #endregion

        bool loading = false;
        float AnimationLoadingValue = 0F;
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
                ThreadLoading?.Dispose();
                if (showInTaskbar) ShowTaskbar(!loading);
                if (loading)
                {
                    ThreadLoading = new ITask(this, () =>
                    {
                        AnimationLoadingValue = AnimationLoadingValue.Calculate(0.01F);
                        if (AnimationLoadingValue > 1)
                        {
                            AnimationLoadingValue = 0;
                            Invalidate();
                            Thread.Sleep(1000);
                        }
                        Invalidate();
                        return loading;
                    }, 10, () =>
                    {
                        Invalidate();
                    });
                }
                else Invalidate();
                OnPropertyChanged(nameof(Loading));
            }
        }

        /// <summary>
        /// 动画铺满
        /// </summary>
        [Description("动画铺满"), Category("外观"), DefaultValue(false)]
        public bool LoadingFull { get; set; }

        /// <summary>
        /// 动画时长
        /// </summary>
        [Description("动画时长"), Category("外观"), DefaultValue(200)]
        public int Animation { get; set; } = 200;

        #region 步骤

        int steps = 3;
        /// <summary>
        /// 进度条总共步数
        /// </summary>
        [Description("进度条总共步数"), Category("外观"), DefaultValue(3)]
        public int Steps
        {
            get => steps;
            set
            {
                if (steps == value) return;
                steps = value;
                if (shape == TShapeProgress.Steps) Invalidate();
                OnPropertyChanged(nameof(Steps));
            }
        }

        int stepSize = 14;
        /// <summary>
        /// 步数大小
        /// </summary>
        [Description("步数大小"), Category("外观"), DefaultValue(14)]
        public int StepSize
        {
            get => stepSize;
            set
            {
                if (stepSize == value) return;
                stepSize = value;
                if (shape == TShapeProgress.Steps) Invalidate();
                OnPropertyChanged(nameof(StepSize));
            }
        }

        int stepGap = 2;
        /// <summary>
        /// 步数间隔
        /// </summary>
        [Description("步数间隔"), Category("外观"), DefaultValue(2)]
        public int StepGap
        {
            get => stepGap;
            set
            {
                if (stepGap == value) return;
                stepGap = value;
                if (shape == TShapeProgress.Steps) Invalidate();
                OnPropertyChanged(nameof(StepGap));
            }
        }

        #endregion

        #region 动画

        protected override void Dispose(bool disposing)
        {
            ThreadLoading?.Dispose();
            ThreadValue?.Dispose();
            base.Dispose(disposing);
        }
        ITask? ThreadLoading;
        ITask? ThreadValue;

        #endregion

        #region 任务栏

        object? ownerTmp;
        /// <summary>
        /// 窗口对象
        /// </summary>
        [Description("窗口对象"), Category("任务栏"), DefaultValue(null)]
        public ContainerControl? ContainerControl { get; set; }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            ITask.Run(() =>
            {
                Thread.Sleep(100);
                canTaskbar = true;
                if (showInTaskbar) ShowTaskbar();
            });
        }

        bool GetOwner(out ContainerControl? owner)
        {
            if (ownerTmp is ContainerControl tmp)
            {
                owner = tmp;
                return true;
            }
            else
            {
                owner = ContainerControl ?? Parent.FindPARENT();
                if (owner == null)
                {
                    ownerTmp = 1;
                    return false;
                }
                else
                {
                    ownerTmp = owner;
                    return true;
                }
            }
        }

        bool showInTaskbar = false;
        /// <summary>
        /// 任务栏中显示进度
        /// </summary>
        [Description("任务栏中显示进度"), Category("外观"), DefaultValue(false)]
        public bool ShowInTaskbar
        {
            get => showInTaskbar;
            set
            {
                if (showInTaskbar == value) return;
                showInTaskbar = value;
                if (canTaskbar)
                {
                    if (showInTaskbar) ShowTaskbar();
                    else if (GetOwner(out var owner)) TaskbarProgressState(owner!, ThumbnailProgressState.NoProgress);
                }
                OnPropertyChanged(nameof(ShowInTaskbar));
            }
        }

        bool canTaskbar = false;
        void ShowTaskbar(bool sl = false)
        {
            if (canTaskbar && GetOwner(out var owner))
            {
                if (state == TType.None)
                {
                    if (_value == 0 && loading)
                    {
                        TaskbarProgressValue(owner!, 0);
                        TaskbarProgressState(owner!, ThumbnailProgressState.Indeterminate);
                    }
                    else
                    {
                        if (sl && old_state == ThumbnailProgressState.Indeterminate) TaskbarProgressState(owner!, ThumbnailProgressState.NoProgress);
                        TaskbarProgressState(owner!, ThumbnailProgressState.Normal);
                        TaskbarProgressValue(owner!, (ulong)(_value * 100));
                    }
                }
                else
                {
                    switch (state)
                    {
                        case TType.Error:
                            TaskbarProgressState(owner!, ThumbnailProgressState.Error);
                            break;
                        case TType.Warn:
                            TaskbarProgressState(owner!, ThumbnailProgressState.Paused);
                            break;
                        default:
                            TaskbarProgressState(owner!, ThumbnailProgressState.Normal);
                            break;
                    }
                    TaskbarProgressValue(owner!, (ulong)(_value * 100));
                }
            }
        }

        void TaskbarProgressState(ContainerControl hwnd, ThumbnailProgressState state)
        {
            if (old_state == state) return;
            old_state = state;
            if (InvokeRequired)
            {
                Invoke(() => Windows7Taskbar.SetProgressState(hwnd.Handle, state));
                return;
            }
            Windows7Taskbar.SetProgressState(hwnd.Handle, state);
        }

        ulong old_value = 0;
        ThumbnailProgressState old_state = ThumbnailProgressState.NoProgress;
        void TaskbarProgressValue(ContainerControl hwnd, ulong value)
        {
            if (old_value == value) return;
            old_value = value;
            if (InvokeRequired)
            {
                Invoke(() => Windows7Taskbar.SetProgressValue(hwnd.Handle, value));
                return;
            }
            Windows7Taskbar.SetProgressValue(hwnd.Handle, value);
        }

        #endregion

        #endregion

        #region 渲染

        readonly FormatFlags s_c = FormatFlags.Center | FormatFlags.NoWrap;
        protected override void OnDraw(DrawEventArgs e)
        {
            var g = e.Canvas;
            var rect = e.Rect.PaddingRect(Padding);
            if (rect.Width == 0 || rect.Height == 0) return;
            Color color;
            switch (state)
            {
                case TType.Success:
                    color = fill ?? Colour.Success.Get(nameof(Progress), ColorScheme);
                    break;
                case TType.Info:
                    color = fill ?? Colour.Info.Get(nameof(Progress), ColorScheme);
                    break;
                case TType.Warn:
                    color = fill ?? Colour.Warning.Get(nameof(Progress), ColorScheme);
                    break;
                case TType.Error:
                    color = fill ?? Colour.Error.Get(nameof(Progress), ColorScheme);
                    break;
                case TType.None:
                default:
                    color = fill ?? Colour.Primary.Get(nameof(Progress), ColorScheme);
                    break;
            }
            switch (shape)
            {
                case TShapeProgress.Circle:
                    PaintShapeCircle(g, rect, color);
                    break;
                case TShapeProgress.Mini:
                    PaintShapeMini(g, rect, color);
                    break;
                case TShapeProgress.Steps:
                    PaintShapeSteps(g, e.Rect, rect, color);
                    break;
                case TShapeProgress.Round:
                    PaintShapeRound(g, e.Rect, rect, color, true);
                    break;
                case TShapeProgress.Default:
                default:
                    PaintShapeRound(g, e.Rect, rect, color, false);
                    break;
            }
            base.OnDraw(e);
        }

        void PaintShapeMini(Canvas g, Rectangle rect, Color color)
        {
            var _back = back ?? Color.FromArgb(40, color);
            var font_size = g.MeasureString("100" + TextUnit, Font);
            rect.IconRectL(font_size.Height, out var icon_rect, out var text_rect, iconratio);
            if (icon_rect.Width == 0 || icon_rect.Height == 0) return;
            using (var brush = new SolidBrush(fore ?? Colour.Text.Get(nameof(Progress), ColorScheme)))
            {
                string textShow = ValueFormatChanged?.Invoke(this, new FloatEventArgs(_value_show)) ?? (useSystemText ? Text ?? "" : (_value_show * 100F).ToString("F" + ShowTextDot) + TextUnit);
                g.String(textShow, Font, brush, new Rectangle(text_rect.X + 8, text_rect.Y, text_rect.Width - 8, text_rect.Height), FormatFlags.Left | FormatFlags.VerticalCenter | FormatFlags.NoWrap);
            }

            int w = radius == 0 ? (int)Math.Round(icon_rect.Width * .2F) : (int)(radius * Config.Dpi);
            g.DrawEllipse(_back, w, icon_rect);

            #region 进度条

            int max = 0;
            if (_value_show > 0)
            {
                max = (int)Math.Round(360 * _value_show);
                using (var brush = new Pen(color, w))
                {
                    brush.StartCap = brush.EndCap = LineCap.Round;
                    g.DrawArc(brush, icon_rect, -90, max);
                }
            }
            if (loading && AnimationLoadingValue > 0)
            {
                if (_value_show > 0)
                {
                    float alpha = 60 * (1F - AnimationLoadingValue);
                    using (var brush = new Pen(Helper.ToColor(alpha, Colour.BgBase.Get(nameof(Progress), ColorScheme)), w))
                    {
                        brush.StartCap = brush.EndCap = LineCap.Round;
                        g.DrawArc(brush, icon_rect, -90, (int)(max * AnimationLoadingValue));
                    }
                }
                else if (LoadingFull)
                {
                    max = 360;
                    float alpha = 80 * (1F - AnimationLoadingValue);
                    using (var brush = new Pen(Helper.ToColor(alpha, Colour.BgBase.Get(nameof(Progress), ColorScheme)), w))
                    {
                        brush.StartCap = brush.EndCap = LineCap.Round;
                        g.DrawArc(brush, icon_rect, -90, (int)(max * AnimationLoadingValue));
                    }
                }
            }

            #endregion
        }
        void PaintShapeSteps(Canvas g, Rectangle rect_t, Rectangle rect, Color color)
        {
            var _back = back ?? Colour.FillSecondary.Get(nameof(Progress), ColorScheme);
            var font_size = g.MeasureString("100" + TextUnit, Font);
            int pro_gap = (int)(stepGap * Config.Dpi), pro_h = (int)(font_size.Height * valueratio);
            float pro_w = (int)(stepSize * Config.Dpi), has_x = 0;
            int pro_y = rect.Y + (rect.Height - pro_h) / 2;

            var prog = steps * _value_show;
            using (var brush = new SolidBrush(_back))
            using (var brush_fill = new SolidBrush(color))
            {
                if (pro_w <= 0)
                {
                    float w = rect.Width;
                    if (state == TType.None)
                    {
                        string textShow = ValueFormatChanged?.Invoke(this, new FloatEventArgs(_value_show)) ?? (useSystemText ? Text ?? "" : (_value_show * 100F).ToString("F" + ShowTextDot) + TextUnit);
                        w -= g.MeasureString(textShow, Font).Width + pro_h / 2;
                    }
                    else
                    {
                        int ico_size = (int)(font_size.Height * (iconratio + 0.1F));
                        w -= ico_size + pro_h * 2 + pro_h / 2;
                    }
                    pro_w = (w - pro_gap * (steps - 1F)) / steps;
                }
                if (pro_w > 0)
                {
                    var rects = new List<RectangleF>(steps);
                    for (int i = 0; i < steps; i++)
                    {
                        rects.Add(new RectangleF(rect.X + has_x, pro_y, pro_w, pro_h));
                        has_x += pro_w + pro_gap;
                    }
                    if (prog > 0)
                    {
                        float tmpw = LoadingFull ? rect.Width : 0;
                        for (int i = 0; i < steps; i++)
                        {
                            if (prog > i) g.Fill(brush_fill, rects[i]);
                            else
                            {
                                g.Fill(brush, rects[i]);
                                tmpw = rects[i].Right;
                            }
                        }
                        if (loading && AnimationLoadingValue > 0 && tmpw > 0)
                        {
                            using (var path = new GraphicsPath())
                            {
                                foreach (var it in rects) path.AddRectangle(it);
                                var alpha = 60 * (1F - AnimationLoadingValue);
                                using (var brush_prog = new SolidBrush(Helper.ToColor(alpha, Colour.TextBase.Get(nameof(Progress), ColorScheme))))
                                {
                                    var state = g.Save();
                                    g.SetClip(new RectangleF(rect.X, rect.Y, tmpw * _value_show * AnimationLoadingValue, rect.Height));
                                    g.Fill(brush_prog, path);
                                    g.Restore(state);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (loading && LoadingFull)
                        {
                            using (var path = new GraphicsPath())
                            {
                                foreach (var it in rects) path.AddRectangle(it);
                                var alpha = 80 * (1F - AnimationLoadingValue);
                                using (var brush_prog = new SolidBrush(Helper.ToColor(alpha, Colour.TextBase.Get(nameof(Progress), ColorScheme))))
                                {
                                    var state = g.Save();
                                    g.SetClip(new RectangleF(rect.X, rect.Y, rect.Width * AnimationLoadingValue, rect.Height));
                                    g.Fill(brush_prog, path);
                                    g.Restore(state);
                                }
                            }
                        }
                        for (int i = 0; i < steps; i++)
                        {
                            g.Fill(brush, rects[i]);
                        }
                    }
                }
            }

            if (state == TType.None)
            {
                int has_x2 = (int)Math.Ceiling(has_x + pro_h / 2);
                using (var brush = new SolidBrush(fore ?? Colour.Text.Get(nameof(Progress), ColorScheme)))
                {
                    string textShow = ValueFormatChanged?.Invoke(this, new FloatEventArgs(_value_show)) ?? (useSystemText ? Text ?? "" : (_value_show * 100F).ToString("F" + ShowTextDot) + TextUnit);
                    g.String(textShow, Font, brush, new Rectangle(rect.X + has_x2, rect.Y, rect.Width - has_x2, rect.Height), FormatFlags.Left | FormatFlags.VerticalCenter | FormatFlags.NoWrap);
                }
            }
            else
            {
                int has_x2 = (int)Math.Ceiling(has_x);
                int ico_size = (int)(font_size.Height * (iconratio + 0.1F)), size_font_w = pro_h + ico_size;
                g.PaintIcons(state, new Rectangle((rect.X + has_x2 + size_font_w) - ico_size, rect_t.Y + (rect_t.Height - ico_size) / 2, ico_size, ico_size), "Progress", ColorScheme);
            }
        }
        void PaintShapeRound(Canvas g, Rectangle rect_t, Rectangle rect, Color color, bool round)
        {
            var _back = back ?? Colour.FillSecondary.Get(nameof(Progress), ColorScheme);
            float _radius = radius * Config.Dpi;
            if (round) _radius = rect.Height;

            if (state == TType.None)
            {
                if (UseTextCenter)
                {
                    string? textShow;
                    if (ValueFormatChanged == null)
                    {
                        if (useSystemText) textShow = Text;
                        else textShow = (_value_show * 100F).ToString("F" + ShowTextDot) + TextUnit;
                    }
                    else textShow = ValueFormatChanged(this, new FloatEventArgs(_value_show));

                    var sizef = g.MeasureString(Config.NullText, Font);
                    int pro_h = (int)(sizef.Height * valueratio);
                    rect.Y += (rect.Height - pro_h) / 2;
                    rect.Height = pro_h;
                    PaintProgress(g, _radius, rect, _back, color);

                    using (var brush = new SolidBrush(fore ?? Colour.Text.Get(nameof(Progress), ColorScheme)))
                    {
                        g.String(textShow, Font, brush, rect_t, s_c);
                    }
                }
                else
                {
                    string? showtmp, textShow;

                    if (ValueFormatChanged == null)
                    {
                        if (useSystemText) showtmp = textShow = Text;
                        else
                        {
                            string basetext = (_value_show * 100F).ToString("F" + ShowTextDot);
                            var chars = new char[basetext.Length];
                            chars[0] = basetext[0];
                            for (int i = 1; i < basetext.Length; i++)
                            {
                                if (basetext[i] == '.') chars[i] = '.';
                                else chars[i] = '0';
                            }
                            showtmp = string.Join("", chars) + TextUnit;
                            textShow = basetext + TextUnit;
                        }
                    }
                    else showtmp = textShow = ValueFormatChanged(this, new FloatEventArgs(_value_show));

                    if (showtmp == null && textShow == null)
                    {
                        var sizef = g.MeasureString(Config.NullText, Font);
                        int pro_h = (int)(sizef.Height * valueratio);
                        rect.Y += (rect.Height - pro_h) / 2;
                        rect.Height = pro_h;
                        PaintProgress(g, _radius, rect, _back, color);
                    }
                    else
                    {
                        var sizef = g.MeasureString(showtmp, Font);
                        int pro_h = (int)(sizef.Height * valueratio), size_font_w = (int)Math.Ceiling(sizef.Width + sizef.Height * .2F);
                        var rect_rext = new Rectangle(rect.Right - size_font_w, rect_t.Y, size_font_w, rect_t.Height);
                        rect.Y += (rect.Height - pro_h) / 2;
                        rect.Height = pro_h;
                        rect.Width -= size_font_w;
                        if (rect.Width > 0) PaintProgress(g, _radius, rect, _back, color);

                        using (var brush = new SolidBrush(fore ?? Colour.Text.Get(nameof(Progress), ColorScheme)))
                        {
                            g.String(textShow, Font, brush, rect_rext, FormatFlags.Right | FormatFlags.VerticalCenter | FormatFlags.NoWrap);
                        }
                    }
                }
            }
            else
            {
                string showtext = (_value_show * 100F).ToString("F" + ShowTextDot) + TextUnit;
                var sizef = g.MeasureString(showtext, Font);
                int pro_h = (int)(sizef.Height * valueratio), ico_size = (int)(sizef.Height * (iconratio + 0.1F));
                int size_font_w = pro_h + ico_size;
                var rect_rext = new Rectangle(rect.Right - size_font_w, rect_t.Y, size_font_w, rect_t.Height);
                rect.Y += (rect.Height - pro_h) / 2;
                rect.Height = pro_h;
                rect.Width -= size_font_w;
                if (rect.Width > 0) PaintProgress(g, _radius, rect, _back, color);
                g.PaintIcons(state, new Rectangle(rect_rext.Right - ico_size, rect_rext.Y + (rect_rext.Height - ico_size) / 2, ico_size, ico_size), "Progress", ColorScheme);
            }
        }
        void PaintShapeCircle(Canvas g, Rectangle rect, Color color)
        {
            var _back = back ?? Colour.FillSecondary.Get(nameof(Progress), ColorScheme);

            int prog_size;
            if (rect.Width == rect.Height) prog_size = rect.Width;
            else if (rect.Width > rect.Height) prog_size = rect.Height;
            else prog_size = rect.Width;
            int w = radius == 0 ? (int)Math.Round(prog_size * .04F) : (int)(radius * Config.Dpi);
            prog_size -= w;
            var rect_prog = new Rectangle(rect.X + (rect.Width - prog_size) / 2, rect.Y + (rect.Height - prog_size) / 2, prog_size, prog_size);

            if (HasIcon)
            {
                int iconSize = prog_size - radius - (int)(IconCirclePadding * Config.Dpi), gap = (rect_prog.Width - iconSize) / 2;
                if (IconCircleAngle)
                {
                    var state = g.Save();
                    try
                    {
                        float xy = rect_prog.Width / 2F;
                        int xy2 = -iconSize / 2;
                        g.TranslateTransform(rect_prog.X + xy, rect_prog.Y + xy);
                        g.RotateTransform(_value_show * 360F);
                        var ico_rect = new Rectangle(xy2, xy2, iconSize, iconSize);
                        if (icon != null) g.Image(icon, ico_rect);
                        if (iconSvg != null) g.GetImgExtend(iconSvg, ico_rect, IconCircleColor ?? Color.FromArgb(30, _back));
                    }
                    finally { g.Restore(state); }
                }
                else
                {
                    int xy = (rect_prog.Width - iconSize) / 2;
                    var ico_rect = new Rectangle(rect_prog.X + xy, rect_prog.Y + xy, iconSize, iconSize);
                    if (icon != null) g.Image(icon, ico_rect);
                    if (iconSvg != null) g.GetImgExtend(iconSvg, ico_rect, Color.FromArgb(30, _back));//增加透明度}
                }
            }
            g.DrawEllipse(_back, w, rect_prog);

            #region 进度条

            int max = 0;
            if (_value_show > 0)
            {
                max = (int)Math.Round(360 * _value_show);
                using (var brush = new Pen(color, w))
                {
                    brush.StartCap = brush.EndCap = LineCap.Round;
                    g.DrawArc(brush, rect_prog, -90, max);
                }
            }
            if (loading && AnimationLoadingValue > 0)
            {
                if (_value_show > 0)
                {
                    float alpha = 60 * (1F - AnimationLoadingValue);
                    using (var brush = new Pen(Helper.ToColor(alpha, Colour.BgBase.Get(nameof(Progress), ColorScheme)), w))
                    {
                        brush.StartCap = brush.EndCap = LineCap.Round;
                        g.DrawArc(brush, rect_prog, -90, (int)(max * AnimationLoadingValue));
                    }
                }
                else if (LoadingFull)
                {
                    max = 360;
                    float alpha = 80 * (1F - AnimationLoadingValue);
                    using (var brush = new Pen(Helper.ToColor(alpha, Colour.BgBase.Get(nameof(Progress), ColorScheme)), w))
                    {
                        brush.StartCap = brush.EndCap = LineCap.Round;
                        g.DrawArc(brush, rect_prog, -90, (int)(max * AnimationLoadingValue));
                    }
                }
            }

            #endregion

            if (_value_show > 0)
            {
                if (state == TType.None)
                {
                    using (var brush = new SolidBrush(fore ?? Colour.Text.Get(nameof(Progress), ColorScheme)))
                    {
                        string textShow = ValueFormatChanged?.Invoke(this, new FloatEventArgs(_value_show)) ?? (useSystemText ? Text ?? "" : (_value_show * 100F).ToString("F" + ShowTextDot) + TextUnit);
                        g.String(textShow, Font, brush, rect, s_c);
                    }
                }
                else
                {
                    int size = (int)(rect_prog.Width * .26F);
                    g.PaintIconGhosts(state, new Rectangle(rect.X + (rect.Width - size) / 2, rect.Y + (rect.Height - size) / 2, size, size), color);
                }
            }
            else
            {
                using (var brush = new SolidBrush(fore ?? Colour.Text.Get(nameof(Progress), ColorScheme)))
                {
                    string textShow = ValueFormatChanged?.Invoke(this, new FloatEventArgs(_value_show)) ?? (useSystemText ? Text ?? "" : (_value_show * 100F).ToString("F" + ShowTextDot) + TextUnit);
                    g.String(textShow, Font, brush, rect, s_c);
                }
            }
        }

        void PaintProgress(Canvas g, float radius, Rectangle rect, Color back, Color color)
        {
            using (var path = rect.RoundPath(radius))
            {
                g.Fill(back, path);
                bool handloading = true;
                if (_value_show > 0)
                {
                    var _w = rect.Width * _value_show;
                    if (_w > radius)
                    {
                        using (var path_prog = new RectangleF(rect.X, rect.Y, _w, rect.Height).RoundPath(radius))
                        {
                            g.Fill(color, path_prog);
                        }
                        if (loading && AnimationLoadingValue > 0)
                        {
                            handloading = false;
                            var alpha = 60 * (1F - AnimationLoadingValue);
                            using (var path_prog = new RectangleF(rect.X, rect.Y, _w * AnimationLoadingValue, rect.Height).RoundPath(radius))
                            {
                                g.Fill(Helper.ToColor(alpha, Colour.BgBase.Get(nameof(Progress), ColorScheme)), path_prog);
                            }
                        }
                    }
                    else
                    {
                        using (var bmp = new Bitmap(rect.Width, rect.Height))
                        {
                            using (var g2 = Graphics.FromImage(bmp).High())
                            {
                                using (var path_prog = new RectangleF(-_w, 0, _w * 2, rect.Height).RoundPath(radius))
                                {
                                    g2.Fill(color, path_prog);
                                }
                                if (loading && AnimationLoadingValue > 0)
                                {
                                    handloading = false;
                                    var alpha = 60 * (1F - AnimationLoadingValue);
                                    using (var path_prog = new RectangleF(-_w, 0, _w * 2 * AnimationLoadingValue, rect.Height).RoundPath(radius))
                                    {
                                        g2.Fill(Helper.ToColor(alpha, Colour.BgBase.Get(nameof(Progress), ColorScheme)), path_prog);
                                    }
                                }
                            }
                            using (var brush = new TextureBrush(bmp, WrapMode.Clamp))
                            {
                                brush.TranslateTransform(rect.X, rect.Y);
                                g.Fill(brush, path);
                            }
                        }
                    }
                }

                if (loading && AnimationLoadingValue > 0 && handloading && LoadingFull)
                {
                    var alpha = 80 * (1F - AnimationLoadingValue);
                    using (var brush = new SolidBrush(Helper.ToColor(alpha, Colour.BgBase.Get(nameof(Progress), ColorScheme))))
                    {
                        using (var path_prog = new RectangleF(rect.X, rect.Y, rect.Width * AnimationLoadingValue, rect.Height).RoundPath(radius))
                        {
                            g.Fill(brush, path_prog);
                        }
                    }
                }
            }
        }

        #endregion
    }

    #region 任务栏

    internal static class Windows7Taskbar
    {
        static ITaskbarList3? _taskbarList;
        internal static ITaskbarList3 TaskbarList
        {
            get
            {
                if (_taskbarList == null)
                {
                    _taskbarList = (ITaskbarList3)new CTaskbarList();
                    _taskbarList.HrInit();
                }
                return _taskbarList;
            }
        }

        /// <summary>
        /// Sets the progress state of the specified window's
        /// taskbar button.
        /// </summary>
        /// <param name="hwnd">The window handle.</param>
        /// <param name="state">The progress state.</param>
        public static void SetProgressState(IntPtr hwnd, ThumbnailProgressState state) => TaskbarList.SetProgressState(hwnd, state);

        /// <summary>
        /// Sets the progress value of the specified window's
        /// taskbar button.
        /// </summary>
        /// <param name="hwnd">The window handle.</param>
        /// <param name="current">The current value.</param>
        /// <param name="maximum">The maximum value.</param>
        public static void SetProgressValue(IntPtr hwnd, ulong current, ulong maximum) => TaskbarList.SetProgressValue(hwnd, current, maximum);

        public static void SetProgressValue(IntPtr hwnd, ulong current) => TaskbarList.SetProgressValue(hwnd, current, 100);
    }

    /// <summary>
    /// 表示缩略图进度条状态。
    /// </summary>
    internal enum ThumbnailProgressState
    {
        /// <summary>
        /// 没有进度。
        /// </summary>
        NoProgress = 0,
        /// <summary>
        /// 不确定的进度 (marquee)。
        /// </summary>
        Indeterminate = 0x1,
        /// <summary>
        /// 正常进度
        /// </summary>
        Normal = 0x2,
        /// <summary>
        /// 错误进度 (red).
        /// </summary>
        Error = 0x4,
        /// <summary>
        /// 暂停进度 (yellow).
        /// </summary>
        Paused = 0x8
    }

    [ComImport]
    [Guid("56FDF344-FD6D-11d0-958A-006097C9A090")]
    [ClassInterface(ClassInterfaceType.None)]
    internal class CTaskbarList { }

    [ComImport]
    [Guid("EA1AFB91-9E28-4B86-90E9-9E9F8A5EEFAF")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface ITaskbarList3
    {
        [PreserveSig]
        void HrInit();

        [PreserveSig]
        void AddTab(IntPtr hwnd);

        [PreserveSig]
        void DeleteTab(IntPtr hwnd);

        [PreserveSig]
        void ActivateTab(IntPtr hwnd);

        [PreserveSig]
        void SetActiveAlt(IntPtr hwnd);

        // ITaskbarList2
        [PreserveSig]
        void MarkFullscreenWindow(IntPtr hwnd, [MarshalAs(UnmanagedType.Bool)] bool fFullscreen);

        // ITaskbarList3
        void SetProgressValue(IntPtr hwnd, ulong ullCompleted, ulong ullTotal);
        void SetProgressState(IntPtr hwnd, ThumbnailProgressState tbpFlags);
    }

    #endregion
}