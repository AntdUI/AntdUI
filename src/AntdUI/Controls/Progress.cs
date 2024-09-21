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
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
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
            }
        }

        #endregion

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

        /// <summary>
        /// MINI模式
        /// </summary>
        [Description("MINI模式"), Category("外观"), DefaultValue(false)]
        public bool Mini { get; set; } = false;

        TShape shape = TShape.Round;
        /// <summary>
        /// 形状
        /// </summary>
        [Description("形状"), Category("外观"), DefaultValue(TShape.Round)]
        public TShape Shape
        {
            get => shape;
            set
            {
                if (shape == value) return;
                shape = value;
                Invalidate();
            }
        }

        #region 显示文本

        string? text = null;
        /// <summary>
        /// 文本
        /// </summary>
        [Description("文本"), Category("外观"), DefaultValue(null)]
        public override string? Text
        {
            get => text;
            set
            {
                if (text == value) return;
                text = value;
                Invalidate();
                OnTextChanged(EventArgs.Empty);
            }
        }

        bool showText = false;
        /// <summary>
        /// 显示进度文本
        /// </summary>
        [Description("显示进度文本"), Category("外观"), DefaultValue(false)]
        public bool ShowText
        {
            get => showText;
            set
            {
                if (showText == value) return;
                showText = value;
                Invalidate();
            }
        }

        bool showInfo = true;
        /// <summary>
        /// 显示信息
        /// </summary>
        [Description("显示信息"), Category("外观"), DefaultValue(true)]
        public bool ShowInfo
        {
            get => showInfo;
            set
            {
                if (showInfo == value) return;
                showInfo = value;
                if (Mini || shape == TShape.Circle) return;
                Invalidate();
            }
        }

        /// <summary>
        /// 显示进度文本小数点位数
        /// </summary>
        [Description("显示进度文本小数点位数"), Category("外观"), DefaultValue(0)]
        public int ShowTextDot { get; set; } = 0;

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
                if (Config.Animation && IsHandleCreated && Animation > 0)
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
            }
        }

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
            }
        }

        /// <summary>
        /// 动画铺满
        /// </summary>
        [Description("动画铺满"), Category("外观"), DefaultValue(false)]
        public bool LoadingFull { get; set; } = false;

        /// <summary>
        /// 动画时长
        /// </summary>
        [Description("动画时长"), Category("外观"), DefaultValue(200)]
        public int Animation { get; set; } = 200;

        #region 动画

        protected override void Dispose(bool disposing)
        {
            ThreadLoading?.Dispose();
            ThreadValue?.Dispose();
            base.Dispose(disposing);
        }
        ITask? ThreadLoading = null;
        ITask? ThreadValue = null;

        #endregion

        #region 任务栏

        ContainerControl? ownerForm;
        /// <summary>
        /// 窗口对象
        /// </summary>
        [Description("窗口对象"), Category("任务栏"), DefaultValue(null)]
        public ContainerControl? ContainerControl
        {
            get => ownerForm;
            set => ownerForm = value;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (showInTaskbar)
            {
                if (ownerForm == null) ownerForm = Parent.FindPARENT();
                ITask.Run(() =>
                {
                    Thread.Sleep(100);
                    canTaskbar = true;
                    ShowTaskbar();
                });
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
                    else if (ownerForm != null) TaskbarProgressState(ownerForm, ThumbnailProgressState.NoProgress);
                }
            }
        }

        bool canTaskbar = false;
        void ShowTaskbar(bool sl = false)
        {
            if (canTaskbar)
            {
                if (ownerForm == null) return;
                if (state == TType.None)
                {
                    if (_value_show == 0 && loading)
                    {
                        TaskbarProgressValue(ownerForm, 0);
                        TaskbarProgressState(ownerForm, ThumbnailProgressState.Indeterminate);
                    }
                    else
                    {
                        if (sl && old_state == ThumbnailProgressState.Indeterminate) TaskbarProgressState(ownerForm, ThumbnailProgressState.NoProgress);
                        TaskbarProgressState(ownerForm, ThumbnailProgressState.Normal);
                        TaskbarProgressValue(ownerForm, (ulong)(_value_show * 100));
                    }
                }
                else
                {
                    switch (state)
                    {
                        case TType.Error:
                            TaskbarProgressState(ownerForm, ThumbnailProgressState.Error);
                            break;
                        case TType.Warn:
                            TaskbarProgressState(ownerForm, ThumbnailProgressState.Paused);
                            break;
                        default:
                            TaskbarProgressState(ownerForm, ThumbnailProgressState.Normal);
                            break;
                    }
                    TaskbarProgressValue(ownerForm, (ulong)(_value_show * 100));
                }
            }
        }

        void TaskbarProgressState(ContainerControl hwnd, ThumbnailProgressState state)
        {
            if (old_state == state) return;
            old_state = state;
            if (InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    Windows7Taskbar.SetProgressState(hwnd.Handle, state);
                }));
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
                Invoke(new Action(() =>
                {
                    Windows7Taskbar.SetProgressValue(hwnd.Handle, value);
                }));
                return;
            }
            Windows7Taskbar.SetProgressValue(hwnd.Handle, value);
        }

        #endregion

        #endregion

        #region 渲染

        readonly StringFormat s_c = Helper.SF_ALL(), s_r = Helper.SF_ALL(lr: StringAlignment.Far), s_l = Helper.SF_ALL(lr: StringAlignment.Near);
        protected override void OnPaint(PaintEventArgs e)
        {
            var rect_t = ClientRectangle;
            var rect = rect_t.PaddingRect(Padding);
            var g = e.Graphics.High();
            Color _color, _back;
            switch (state)
            {
                case TType.Success:
                    _color = fill ?? Style.Db.Success;
                    break;
                case TType.Info:
                    _color = fill ?? Style.Db.Info;
                    break;
                case TType.Warn:
                    _color = fill ?? Style.Db.Warning;
                    break;
                case TType.Error:
                    _color = fill ?? Style.Db.Error;
                    break;
                case TType.None:
                default:
                    _color = fill ?? Style.Db.Primary;
                    break;
            }
            if (Mini)
            {
                _back = back ?? Color.FromArgb(40, _color);
                var font_size = g.MeasureString(text, Font);
                rect.IconRectL(font_size, out var icon_rect, out var text_rect, iconratio);
                if (icon_rect.Width == 0 || icon_rect.Height == 0) return;
                using (var brush = new SolidBrush(fore ?? Style.Db.Text))
                {
                    if (showText) g.DrawStr((_value_show * 100F).ToString("F" + ShowTextDot) + text, Font, brush, new RectangleF(text_rect.X + 8, text_rect.Y, text_rect.Width - 8, text_rect.Height), s_l);
                    else g.DrawStr(text, Font, brush, new RectangleF(text_rect.X + 8, text_rect.Y, text_rect.Width - 8, text_rect.Height), s_l);
                }

                float w = radius * Config.Dpi;
                using (var brush = new Pen(_back, w))
                {
                    g.DrawEllipse(brush, icon_rect);
                }

                #region 进度条

                int max = 0;
                if (_value_show > 0)
                {
                    max = (int)Math.Round(360 * _value_show);
                    using (var brush = new Pen(_color, w))
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
                        using (var brush = new Pen(Helper.ToColor(alpha, Style.Db.BgBase), w))
                        {
                            brush.StartCap = brush.EndCap = LineCap.Round;
                            g.DrawArc(brush, icon_rect, -90, (int)(max * AnimationLoadingValue));
                        }
                    }
                    else if (LoadingFull)
                    {
                        max = 360;
                        float alpha = 80 * (1F - AnimationLoadingValue);
                        using (var brush = new Pen(Helper.ToColor(alpha, Style.Db.BgBase), w))
                        {
                            brush.StartCap = brush.EndCap = LineCap.Round;
                            g.DrawArc(brush, icon_rect, -90, (int)(max * AnimationLoadingValue));
                        }
                    }
                }

                #endregion
            }
            else
            {
                _back = back ?? Style.Db.FillSecondary;
                if (shape == TShape.Circle)
                {
                    float w = radius * Config.Dpi;
                    float prog_size;
                    if (rect.Width == rect.Height) prog_size = rect.Width - w;
                    else if (rect.Width > rect.Height) prog_size = rect.Height - w;
                    else prog_size = rect.Width - w;
                    var rect_prog = new RectangleF(rect.X + (rect.Width - prog_size) / 2, rect.Y + (rect.Height - prog_size) / 2, prog_size, prog_size);
                    using (var brush = new Pen(_back, w))
                    {
                        g.DrawEllipse(brush, rect_prog);
                    }

                    #region 进度条

                    int max = 0;
                    if (_value_show > 0)
                    {
                        max = (int)Math.Round(360 * _value_show);
                        using (var brush = new Pen(_color, w))
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
                            using (var brush = new Pen(Helper.ToColor(alpha, Style.Db.BgBase), w))
                            {
                                brush.StartCap = brush.EndCap = LineCap.Round;
                                g.DrawArc(brush, rect_prog, -90, (int)(max * AnimationLoadingValue));
                            }
                        }
                        else if (LoadingFull)
                        {
                            max = 360;
                            float alpha = 80 * (1F - AnimationLoadingValue);
                            using (var brush = new Pen(Helper.ToColor(alpha, Style.Db.BgBase), w))
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
                            using (var brush = new SolidBrush(fore ?? Style.Db.Text))
                            {
                                if (showText) g.DrawStr((_value_show * 100F).ToString("F" + ShowTextDot) + text, Font, brush, rect, s_c);
                                else g.DrawStr(text, Font, brush, rect, s_c);
                            }
                        }
                        else
                        {
                            int size = (int)(rect_prog.Width * .26F);
                            g.PaintIconGhosts(state, new Rectangle(rect.X + (rect.Width - size) / 2, rect.Y + (rect.Height - size) / 2, size, size), _color);
                        }
                    }
                    else
                    {
                        using (var brush = new SolidBrush(fore ?? Style.Db.Text))
                        {
                            if (showText) g.DrawStr((_value_show * 100F).ToString("F" + ShowTextDot) + text, Font, brush, rect, s_c);
                            else g.DrawStr(text, Font, brush, rect, s_c);
                        }
                    }
                }
                else
                {
                    float _radius = radius * Config.Dpi;
                    if (shape == TShape.Round) _radius = rect.Height;
                    if (showText)
                    {
                        if (showInfo)
                        {
                            if (state == TType.None)
                            {
                                string basetext = (_value_show * 100F).ToString("F" + ShowTextDot);
                                var chars = new char[basetext.Length];
                                chars[0] = basetext[0];
                                for (int i = 1; i < basetext.Length; i++)
                                {
                                    if (basetext[i] == '.') chars[i] = '.';
                                    else chars[i] = '0';
                                }
                                string showtmp = string.Join("", chars) + text;

                                var sizef = g.MeasureString(showtmp, Font);
                                int pro_h = (int)Math.Ceiling(sizef.Height / 2);
                                int size_font_w = (int)Math.Ceiling(sizef.Width + sizef.Height * .2F);
                                var rect_rext = new Rectangle(rect.Right - size_font_w, rect_t.Y, size_font_w, rect_t.Height);
                                rect.Y = rect.Y + (rect.Height - pro_h) / 2;
                                rect.Height = pro_h;
                                rect.Width -= size_font_w;
                                PaintProgress(g, _radius, rect, _back, _color);

                                using (var brush = new SolidBrush(fore ?? Style.Db.Text))
                                {
                                    g.DrawStr(basetext + text, Font, brush, rect_rext, s_r);
                                }
                            }
                            else
                            {
                                string showtext = (_value_show * 100F).ToString("F" + ShowTextDot) + text;
                                var sizef = g.MeasureString(showtext, Font);
                                int pro_h = (int)Math.Ceiling(sizef.Height / 2), ico_size = (int)Math.Ceiling(sizef.Height);
                                int size_font_w = (int)Math.Ceiling(pro_h + sizef.Height);
                                var rect_rext = new Rectangle(rect.Right - size_font_w, rect_t.Y, size_font_w, rect_t.Height);
                                rect.Y = rect.Y + (rect.Height - pro_h) / 2;
                                rect.Height = pro_h;
                                rect.Width -= size_font_w;
                                PaintProgress(g, _radius, rect, _back, _color);
                                g.PaintIcons(state, new Rectangle(rect_rext.Right - ico_size, rect_rext.Y + (rect_rext.Height - ico_size) / 2, ico_size, ico_size));
                            }
                        }
                        else
                        {
                            PaintProgress(g, _radius, rect, _back, _color);
                            using (var brush = new SolidBrush(fore ?? Style.Db.Text))
                            {
                                g.DrawStr((_value_show * 100F).ToString("F" + ShowTextDot) + text, Font, brush, rect, s_c);
                            }
                        }
                    }
                    else
                    {
                        PaintProgress(g, _radius, rect, _back, _color);
                        using (var brush = new SolidBrush(fore ?? Style.Db.Text))
                        {
                            g.DrawStr(text, Font, brush, rect, s_c);
                        }
                    }
                }
            }
            this.PaintBadge(g);
            base.OnPaint(e);
        }

        void PaintProgress(Graphics g, float radius, Rectangle rect, Color back, Color color)
        {
            using (var path = rect.RoundPath(radius))
            {
                using (var brush = new SolidBrush(back))
                {
                    g.FillPath(brush, path);
                }
                bool handloading = true;
                if (_value_show > 0)
                {
                    var _w = rect.Width * _value_show;
                    if (_w > radius)
                    {
                        using (var path_prog = new RectangleF(rect.X, rect.Y, _w, rect.Height).RoundPath(radius))
                        {
                            using (var brush = new SolidBrush(color))
                            {
                                g.FillPath(brush, path_prog);
                            }
                        }
                        if (loading && AnimationLoadingValue > 0)
                        {
                            handloading = false;
                            var alpha = 60 * (1F - AnimationLoadingValue);
                            using (var brush = new SolidBrush(Helper.ToColor(alpha, Style.Db.BgBase)))
                            {
                                using (var path_prog = new RectangleF(rect.X, rect.Y, _w * AnimationLoadingValue, rect.Height).RoundPath(radius))
                                {
                                    g.FillPath(brush, path_prog);
                                }
                            }
                        }
                    }
                    else
                    {
                        using (var bmp = new Bitmap(rect.Width, rect.Height))
                        {
                            using (var g2 = Graphics.FromImage(bmp).High())
                            {
                                using (var brush = new SolidBrush(color))
                                {
                                    using (var path_prog = new RectangleF(-_w, 0, _w * 2, rect.Height).RoundPath(radius))
                                    {
                                        g2.FillPath(brush, path_prog);
                                    }
                                }
                                if (loading && AnimationLoadingValue > 0)
                                {
                                    handloading = false;
                                    var alpha = 60 * (1F - AnimationLoadingValue);
                                    using (var brush = new SolidBrush(Helper.ToColor(alpha, Style.Db.BgBase)))
                                    {
                                        using (var path_prog = new RectangleF(-_w, 0, _w * 2 * AnimationLoadingValue, rect.Height).RoundPath(radius))
                                        {
                                            g2.FillPath(brush, path_prog);
                                        }
                                    }
                                }
                            }
                            using (var brush = new TextureBrush(bmp, WrapMode.Clamp))
                            {
                                brush.TranslateTransform(rect.X, rect.Y);
                                g.FillPath(brush, path);
                            }
                        }
                    }
                }

                if (loading && AnimationLoadingValue > 0 && handloading && LoadingFull)
                {
                    var alpha = 80 * (1F - AnimationLoadingValue);
                    using (var brush = new SolidBrush(Helper.ToColor(alpha, Style.Db.BgBase)))
                    {
                        using (var path_prog = new RectangleF(rect.X, rect.Y, rect.Width * AnimationLoadingValue, rect.Height).RoundPath(radius))
                        {
                            g.FillPath(brush, path_prog);
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
        public static void SetProgressState(IntPtr hwnd, ThumbnailProgressState state)
        {
            TaskbarList.SetProgressState(hwnd, state);
        }

        /// <summary>
        /// Sets the progress value of the specified window's
        /// taskbar button.
        /// </summary>
        /// <param name="hwnd">The window handle.</param>
        /// <param name="current">The current value.</param>
        /// <param name="maximum">The maximum value.</param>
        public static void SetProgressValue(IntPtr hwnd, ulong current, ulong maximum)
        {
            TaskbarList.SetProgressValue(hwnd, current, maximum);
        }

        public static void SetProgressValue(IntPtr hwnd, ulong current)
        {
            TaskbarList.SetProgressValue(hwnd, current, 100);
        }
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

    [Guid("56FDF344-FD6D-11d0-958A-006097C9A090")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComImport]
    internal class CTaskbarList { }

    [ComImport]
    [Guid("ea1afb91-9e28-4b86-90e9-9e9f8a5eefaf")]
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
        void SetProgressValue(IntPtr hwnd, UInt64 ullCompleted, UInt64 ullTotal);
        void SetProgressState(IntPtr hwnd, ThumbnailProgressState tbpFlags);
    }

    #endregion
}