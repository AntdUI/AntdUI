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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// Spin 加载中
    /// </summary>
    /// <remarks>用于页面和区块的加载中状态。</remarks>
    [Description("Spin 加载中")]
    [ToolboxItem(true)]
    public class Spin : IControl
    {
        Config config = new Config();

        #region 属性

        [Description("颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? Fill
        {
            get => config.Color;
            set => config.Color = value;
        }

        /// <summary>
        /// 文字颜色
        /// </summary>
        [Description("文字颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public new Color? ForeColor
        {
            get => config.Fore;
            set
            {
                if (config.Fore == value) return;
                config.Fore = value;
                Invalidate();
                OnPropertyChanged(nameof(ForeColor));
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
                text = value;
                spin_core.Clear();
                Invalidate();
                OnTextChanged(EventArgs.Empty);
                OnPropertyChanged(nameof(Text));
            }
        }

        [Description("文本"), Category("国际化"), DefaultValue(null)]
        public string? LocalizationText { get; set; }

        [Description("加载指示符"), Category("外观"), DefaultValue(null)]
        public Image? Indicator
        {
            get => config.Indicator;
            set => config.Indicator = value;
        }

        [Description("加载指示符SVG"), Category("外观"), DefaultValue(null)]
        public string? IndicatorSvg
        {
            get => config.IndicatorSvg;
            set => config.IndicatorSvg = value;
        }

        #endregion

        #region 动画

        SpinCore spin_core = new SpinCore();
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            spin_core.Start(this);
        }

        protected override void OnFontChanged(EventArgs e)
        {
            spin_core.Clear();
            base.OnFontChanged(e);
        }

        #endregion

        protected override void OnDraw(DrawEventArgs e)
        {
            var rect = e.Rect.PaddingRect(Padding);
            if (rect.Width == 0 || rect.Height == 0) return;
            config.Text = this.GetLangI(LocalizationText, text);
            spin_core.Paint(e.Canvas, rect, config, this);
        }

        protected override void Dispose(bool disposing)
        {
            spin_core.Dispose();
            base.Dispose(disposing);
        }

        /// <summary>
        /// 配置
        /// </summary>
        public class Config
        {
            /// <summary>
            /// 文本
            /// </summary>
            public string? Text { get; set; }

            /// <summary>
            /// 背景颜色
            /// </summary>
            public Color? Back { get; set; }

            /// <summary>
            /// 文本颜色
            /// </summary>
            public Color? Fore { get; set; }

            /// <summary>
            /// 颜色
            /// </summary>
            public Color? Color { get; set; }

            /// <summary>
            /// 字体
            /// </summary>

            public Font? Font { get; set; }

            /// <summary>
            /// 圆角
            /// </summary>
            public int? Radius { get; set; }

            /// <summary>
            /// 加载指示符
            /// </summary>
            public Image? Indicator { get; set; }

            /// <summary>
            /// 加载指示符SVG
            /// </summary>
            public string? IndicatorSvg { get; set; }

            /// <summary>
            /// 进度
            /// </summary>
            public float? Value { get; set; }

            /// <summary>
            /// 进度速率
            /// </summary>
            public float? Rate { get; set; }

            #region 设置

            public Config SetText(string? value)
            {
                Text = value;
                return this;
            }
            public Config SetBack(Color? value)
            {
                Back = value;
                return this;
            }
            public Config SetFore(Color? value)
            {
                Fore = value;
                return this;
            }
            public Config SetColor(Color? value)
            {
                Color = value;
                return this;
            }
            public Config SetFont(Font? value)
            {
                Font = value;
                return this;
            }
            public Config SetRadius(int? value)
            {
                Radius = value;
                return this;
            }
            public Config SetValue(float? value)
            {
                Value = value;
                return this;
            }
            public Config SetIndicator(Image? value)
            {
                Indicator = value;
                return this;
            }
            public Config SetIndicator(string? value)
            {
                IndicatorSvg = value;
                return this;
            }
            public Config SetRate(float? value)
            {
                Rate = value;
                return this;
            }

            #endregion
        }

        #region 静态方法

        #region 同步

        /// <summary>
        /// Spin 加载中
        /// </summary>
        /// <param name="control">控件主体</param>
        /// <param name="action">需要等待的委托</param>
        /// <param name="end">运行结束后的回调</param>
        /// <param name="error">发生错误时的回调</param>
        public static Task open(Control control, Action<Config> action, Action? end = null, Action<Exception>? error = null) => open(control, new Config(), action, end, error);

        /// <summary>
        /// Spin 加载中
        /// </summary>
        /// <param name="control">控件主体</param>
        /// <param name="text">加载文本</param>
        /// <param name="action">需要等待的委托</param>
        /// <param name="end">运行结束后的回调</param>
        /// <param name="error">发生错误时的回调</param>
        public static Task open(Control control, string text, Action<Config> action, Action? end = null, Action<Exception>? error = null) => open(control, new Config { Text = text }, action, end, error);

        /// <summary>
        /// Spin 加载中
        /// </summary>
        /// <param name="control">控件主体</param>
        /// <param name="config">自定义配置</param>
        /// <param name="action">需要等待的委托</param>
        /// <param name="end">运行结束后的回调</param>
        /// <param name="error">发生错误时的回调</param>
        public static Task open(Control control, Config config, Action<Config> action, Action? end = null, Action<Exception>? error = null)
        {
            var parent = control.FindPARENT();
            if (parent is LayeredFormAsynLoad model)
            {
                if (model.IsLoad)
                {
                    var Event = new ManualResetEvent(false);
                    model.LoadCompleted += () => Event.SetWait();
                    return ITask.Run(() =>
                    {
                        if (Event.Wait(1000)) return;
                        open_core(control, true, parent, config, action, end, error)?.Wait();
                    });
                }
                else return open_core(control, control.InvokeRequired, parent, config, action, end, error);
            }
            return open_core(control, control.InvokeRequired, parent, config, action, end, error);
        }

        static SpinForm open_core(Control control, bool InvokeRequired, Form? parent, Config config)
        {
            SpinForm frm;
            if (InvokeRequired) return ITask.Invoke(control, new Func<SpinForm>(() => open_core(control, false, parent, config)))!;
            frm = new SpinForm(control, parent, config);
            frm.Show(control);
            return frm;
        }
        static Task open_core(Control control, bool InvokeRequired, Form? parent, Config config, Action<Config> action, Action? end = null, Action<Exception>? error = null)
        {
            var frm = open_core(control, InvokeRequired, parent, config);
            bool hasError = false;
            return ITask.Run(() =>
            {
                if (frm == null) return;
                Exception? ex = null;
                try
                {
                    action(config);
                }
                catch (Exception e)
                {
                    ex = e;
                    hasError = true;
                    try
                    {
                        error?.Invoke(e);
                    }
                    catch { }
                }
                if (frm.IsDisposed) return;
                try
                {
                    frm.Invoke(() => frm.Dispose());
                }
                catch { }
                // 如果没有提供错误回调，则重新抛出异常
                if (ex != null && error == null) throw ex;
            }, () =>
            {
                if (end == null || hasError) return;
                // 只有在没有错误且提供了完成回调时才执行
                try
                {
                    end();
                }
                catch
                { }
            });
        }

        #endregion

        #region 异步

        /// <summary>
        /// Spin 加载中
        /// </summary>
        /// <param name="control">控件主体</param>
        /// <param name="action">需要等待的委托</param>
        /// <param name="end">运行结束后的回调</param>
        /// <param name="error">发生错误时的回调</param>
        public static Task open(Control control, Func<Config, Task> action, Action? end = null, Action<Exception>? error = null) => open(control, new Config(), action, end, error);

        /// <summary>
        /// Spin 加载中
        /// </summary>
        /// <param name="control">控件主体</param>
        /// <param name="text">加载文本</param>
        /// <param name="action">需要等待的委托</param>
        /// <param name="end">运行结束后的回调</param>
        /// <param name="error">发生错误时的回调</param>
        public static Task open(Control control, string text, Func<Config, Task> action, Action? end = null, Action<Exception>? error = null) => open(control, new Config { Text = text }, action, end, error);

        /// <summary>
        /// Spin 加载中
        /// </summary>
        /// <param name="control">控件主体</param>
        /// <param name="config">自定义配置</param>
        /// <param name="action">需要等待的委托</param>
        /// <param name="end">运行结束后的回调</param>
        /// <param name="error">发生错误时的回调</param>
        public static Task open(Control control, Config config, Func<Config, Task> action, Action? end = null, Action<Exception>? error = null)
        {
            var parent = control.FindPARENT();
            if (parent is LayeredFormAsynLoad model)
            {
                if (model.IsLoad)
                {
                    var Event = new ManualResetEvent(false);
                    model.LoadCompleted += () => Event.SetWait();
                    return ITask.Run(() =>
                    {
                        if (Event.Wait(1000)) return;
                        open_core(control, true, parent, config, action, end, error)?.Wait();
                    });
                }
                else return open_core(control, control.InvokeRequired, parent, config, action, end, error);
            }
            return open_core(control, control.InvokeRequired, parent, config, action, end, error);
        }

        static Task open_core(Control control, bool InvokeRequired, Form? parent, Config config, Func<Config, Task> action, Action? end = null, Action<Exception>? error = null)
        {
            var frm = open_core(control, InvokeRequired, parent, config);
            bool hasError = false;
            return ITask.Run(() =>
            {
                if (frm == null) return;
                Exception? ex = null;
                try
                {
                    var task = action(config);
                    if (task != null) task.Wait();
                }
                catch (Exception e)
                {
                    ex = e;
                    hasError = true;
                    try
                    {
                        error?.Invoke(e);
                    }
                    catch { }
                }
                if (frm.IsDisposed) return;
                try
                {
                    frm.Invoke(() => frm.Dispose());
                }
                catch { }
                // 如果没有提供错误回调，则重新抛出异常
                if (ex != null && error == null) throw ex;
            }, () =>
            {
                if (end == null || hasError) return;
                // 只有在没有错误且提供了完成回调时才执行
                try
                {
                    end();
                }
                catch
                { }
            });
        }

        #endregion

        #endregion
    }

    internal class SpinCore : IDisposable
    {
        ITask? thread = null;

        float LineWidth = 6, LineAngle = 0;
        int prog_size = 0;
        public void Clear() => prog_size = 0;

        public void Start(IControl control)
        {
            Stop();
            bool ProgState = false;
            thread = new ITask(control, () =>
            {
                Animation(ref ProgState);
                control.Invalidate();
                return true;
            }, 10);
        }
        public void Start(ILayeredForm control)
        {
            Stop();
            bool ProgState = false;
            thread = new ITask(control, () =>
            {
                Animation(ref ProgState);
                control.Print();
                return true;
            }, 10);
        }

        void Animation(ref bool ProgState)
        {
            switch (mode)
            {
                case 0:
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
                    break;
                case 1:
                    LineAngle = LineAngle.Calculate(2F);
                    break;
                case 2:
                default:
                    LineAngle = LineAngle.Calculate(rate ?? 8F);
                    break;
            }
            if (LineAngle >= 360) LineAngle = 0;
        }

        public void Stop()
        {
            thread?.Dispose();
            thread = null;
        }

        readonly FormatFlags s_f = FormatFlags.Center | FormatFlags.NoWrapEllipsis;

        float? rate;
        int mode = 0;
        public void Paint(Canvas g, Rectangle rect, Spin.Config config, Control control)
        {
            var font = config.Font ?? control.Font;
            if (prog_size == 0) prog_size = g.MeasureText(config.Text ?? Config.NullText, font).Height;
            int rprog_size = (int)(prog_size * 1.6F), size = (int)(prog_size * .2F), size2 = rprog_size / 2;
            var rect_prog = new Rectangle(rect.X + (rect.Width - rprog_size) / 2, rect.Y + (rect.Height - rprog_size) / 2, rprog_size, rprog_size);
            if (config.Text != null)
            {
                var y = rect_prog.Bottom;
                rect_prog.Offset(0, -size2);
                g.DrawText(config.Text, font, config.Fore ?? Colour.Primary.Get(nameof(Spin)), new Rectangle(rect.X, y, rect.Width, prog_size), s_f);
            }
            var color = config.Color ?? Colour.Primary.Get(nameof(Spin));
            if (config.Indicator != null || config.IndicatorSvg != null)
            {
                rate = config.Rate;
                mode = 2;
                var size22 = rprog_size / 2F;
                g.TranslateTransform(rect_prog.X + size22, rect_prog.Y + size22);
                g.RotateTransform(LineAngle);
                var rect_center = new Rectangle(-size2, -size2, rprog_size, rprog_size);
                if (config.Indicator != null) g.Image(config.Indicator, rect_center);
                if (config.IndicatorSvg != null) g.GetImgExtend(config.IndicatorSvg, rect_center, color);
                g.ResetTransform();
            }
            else
            {
                g.DrawEllipse(Colour.Fill.Get(nameof(Spin)), size, rect_prog);
                using (var brush = new Pen(color, size))
                {
                    brush.StartCap = brush.EndCap = LineCap.Round;
                    if (config.Value.HasValue)
                    {
                        mode = 1;
                        g.DrawArc(brush, rect_prog, LineAngle, config.Value.Value * 360F);
                    }
                    else
                    {
                        mode = 0;
                        g.DrawArc(brush, rect_prog, LineAngle, LineWidth * 3.6F);
                    }
                }
            }
        }

        public void Dispose() => Stop();
    }

    internal class SpinForm : ILayeredFormOpacity
    {
        Control control;
        Form? parent;

        Spin.Config config;
        public SpinForm(Control _control, Form? _parent, Spin.Config _config)
        {
            control = _control;
            parent = _parent;
            Font = _control.Font;
            config = _config;
            _control.SetTopMost(Handle);
            if (_control is Form form)
            {
                SetSize(form.Size);
                SetLocation(form.Location);
                if (_config.Radius.HasValue) Radius = _config.Radius.Value;
                else HasBor = form.FormFrame(out Radius, out Bor);
            }
            else
            {
                SetSize(_control.Size);
                SetLocation(_control.PointToScreen(Point.Empty));
                if (_config.Radius.HasValue) Radius = _config.Radius.Value;
                else if (_control is IControl icontrol) RenderRegion = () => icontrol.RenderRegion;
            }
        }

        public override string name => nameof(Spin);

        Func<GraphicsPath>? RenderRegion;
        int Radius = 0, Bor = 0;
        bool HasBor = false;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            control.VisibleChanged += Parent_VisibleChanged;
            control.LocationChanged += Parent_LocationChanged;
            control.SizeChanged += Parent_SizeChanged;
            if (control is TabPage page) page.ShowedChanged += Parent_VisibleChanged;
            if (parent != null)
            {
                parent.VisibleChanged += Parent_VisibleChanged;
                parent.LocationChanged += Parent_LocationChanged;
                parent.SizeChanged += Parent_SizeChanged;
            }
            LoadVisible();
        }

        bool visible = false;
        void LoadVisible()
        {
            var tmp = GetVisible();
            if (visible == tmp) return;
            visible = tmp;
            if (tmp) spin_core.Start(this);
            else
            {
                spin_core.Stop();
                Print();
            }
        }
        bool GetVisible()
        {
            if (control is TabPage page) return page.Showed && page.Visible;
            return control.Visible;
        }

        private void Parent_VisibleChanged(object? sender, EventArgs e) => LoadVisible();
        private void Parent_LocationChanged(object? sender, EventArgs e)
        {
            LoadVisible();
            if (control is Form form) SetLocation(form.Location);
            else SetLocation(control.PointToScreen(Point.Empty));
        }
        private void Parent_SizeChanged(object? sender, EventArgs e)
        {
            LoadVisible();
            if (control is Form form)
            {
                SetSize(form.Size);
                SetLocation(form.Location);
            }
            else
            {
                SetLocation(control.PointToScreen(Point.Empty));
                SetSize(control.Size);
            }
        }

        #region 渲染

        SpinCore spin_core = new SpinCore();
        public override Bitmap? PrintBit()
        {
            Rectangle rect_read = TargetRectXY, rect = HasBor ? new Rectangle(Bor, 0, rect_read.Width - Bor * 2, rect_read.Height - Bor) : rect_read;
            var rbmp = new Bitmap(rect_read.Width, rect_read.Height);
            if (visible)
            {
                using (var g = Graphics.FromImage(rbmp).HighLay(true))
                {
                    using (var brush = new SolidBrush(config.Back ?? Style.rgba(Colour.BgBase.Get(nameof(Spin)), .8F)))
                    {
                        if (RenderRegion == null)
                        {
                            if (Radius > 0)
                            {
                                using (var path = rect.RoundPath(Radius))
                                {
                                    g.Fill(brush, path);
                                }
                            }
                            else g.Fill(brush, rect);
                        }
                        else
                        {
                            using (var path = RenderRegion())
                            {
                                g.Fill(brush, path);
                            }
                        }
                    }
                    spin_core.Paint(g, rect, config, this);
                }
            }
            return rbmp;
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            spin_core.Dispose();
            control.VisibleChanged -= Parent_VisibleChanged;
            control.LocationChanged -= Parent_LocationChanged;
            control.SizeChanged -= Parent_SizeChanged;
            if (control is TabPage page) page.ShowedChanged -= Parent_VisibleChanged;
            if (parent != null)
            {
                parent.VisibleChanged -= Parent_VisibleChanged;
                parent.LocationChanged -= Parent_LocationChanged;
                parent.SizeChanged -= Parent_SizeChanged;
            }
            base.Dispose(disposing);
            if (control == null) return;
        }
    }
}