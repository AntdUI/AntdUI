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
                OnPropertyChanged("ForeColor");
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
                text = value;
                spin_core.Clear();
                Invalidate();
                OnTextChanged(EventArgs.Empty);
                OnPropertyChanged("Text");
            }
        }

        [Description("文本"), Category("国际化"), DefaultValue(null)]
        public string? LocalizationText { get; set; }

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

        protected override void OnPaint(PaintEventArgs e)
        {
            var rect = ClientRectangle.PaddingRect(Padding);
            if (rect.Width == 0 || rect.Height == 0) return;
            config.Text = this.GetLangI(LocalizationText, text);
            spin_core.Paint(e.Graphics.High(), rect, config, this);
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
            /// 进度
            /// </summary>
            public float? Value { get; set; }
        }

        #region 静态方法

        /// <summary>
        /// Spin 加载中
        /// </summary>
        /// <param name="control">控件主体</param>
        /// <param name="action">需要等待的委托</param>
        /// <param name="end">运行结束后的回调</param>
        public static Task open(Control control, Action<Config> action, Action? end = null) => open(control, new Config(), action, end);

        /// <summary>
        /// Spin 加载中
        /// </summary>
        /// <param name="control">控件主体</param>
        /// <param name="text">加载文本</param>
        /// <param name="action">需要等待的委托</param>
        /// <param name="end">运行结束后的回调</param>
        public static Task open(Control control, string text, Action<Config> action, Action? end = null) => open(control, new Config { Text = text }, action, end);

        /// <summary>
        /// Spin 加载中
        /// </summary>
        /// <param name="control">控件主体</param>
        /// <param name="config">自定义配置</param>
        /// <param name="action">需要等待的委托</param>
        /// <param name="end">运行结束后的回调</param>
        public static Task open(Control control, Config config, Action<Config> action, Action? end = null)
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
                        open_core(control, true, parent, config, action, end)?.Wait();
                    });
                }
                else return open_core(control, control.InvokeRequired, parent, config, action, end);
            }
            return open_core(control, control.InvokeRequired, parent, config, action, end);
        }

        static SpinForm open_core(Control control, bool InvokeRequired, Form? parent, Config config)
        {
            SpinForm frm;
            if (InvokeRequired) return ITask.Invoke(control, new Func<SpinForm>(() => open_core(control, false, parent, config)));
            frm = new SpinForm(control, parent, config);
            frm.Show(control);
            return frm;
        }
        static Task open_core(Control control, bool InvokeRequired, Form? parent, Config config, Action<Config> action, Action? end = null)
        {
            var frm = open_core(control, InvokeRequired, parent, config);
            return ITask.Run(() =>
            {
                if (frm == null) return;
                Exception? ex = null;
                try
                {
                    action(config);
                }
                catch (Exception e) { ex = e; }
                if (frm.IsDisposed) return;
                frm.Invoke(new Action(() =>
                {
                    frm.Dispose();
                }));
                if (ex != null) throw ex;
            }, end);
        }

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
            bool ProgState = false;
            thread = new ITask(control, () =>
            {
                if (lnull) LineAngle = LineAngle.Calculate(2F);
                else
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
                }
                if (LineAngle >= 360) LineAngle = 0;
                control.Invalidate();
                return true;
            }, 10);
        }
        public void Start(ILayeredForm control)
        {
            bool ProgState = false;
            thread = new ITask(control, () =>
            {
                if (lnull) LineAngle = LineAngle.Calculate(2F);
                else
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
                }
                if (LineAngle >= 360) LineAngle = 0;
                control.Print();
                return true;
            }, 10);
        }

        readonly StringFormat s_f = Helper.SF_ALL();

        bool lnull = false;
        public void Paint(Canvas g, Rectangle rect, Spin.Config config, Control control)
        {
            var font = config.Font ?? control.Font;
            if (prog_size == 0) prog_size = g.MeasureString(config.Text ?? Config.NullText, font).Height;
            int rprog_size = (int)(prog_size * 1.6F), size = (int)(prog_size * .2F), size2 = rprog_size / 2;
            var rect_prog = new Rectangle(rect.X + (rect.Width - rprog_size) / 2, rect.Y + (rect.Height - rprog_size) / 2, rprog_size, rprog_size);
            if (config.Text != null)
            {
                var y = rect_prog.Bottom;
                rect_prog.Offset(0, -size2);
                g.String(config.Text, font, config.Fore ?? Colour.Primary.Get("Spin"), new Rectangle(rect.X, y, rect.Width, prog_size), s_f);
            }
            g.DrawEllipse(Colour.Fill.Get("Spin"), size, rect_prog);
            using (var brush = new Pen(config.Color ?? Colour.Primary.Get("Spin"), size))
            {
                brush.StartCap = brush.EndCap = LineCap.Round;
                if (config.Value.HasValue)
                {
                    lnull = true;
                    g.DrawArc(brush, rect_prog, LineAngle, config.Value.Value * 360F);
                }
                else
                {
                    lnull = false;
                    g.DrawArc(brush, rect_prog, LineAngle, LineWidth * 3.6F);
                }
            }
        }

        public void Dispose() => thread?.Dispose();
    }

    internal class SpinForm : ILayeredFormOpacity
    {
        Control control;
        Form? parent = null;

        Spin.Config config;
        public SpinForm(Control _control, Form? _parent, Spin.Config _config)
        {
            maxalpha = 255;
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
                else if (_control is IControl icontrol) gpath = icontrol.RenderRegion;
                else HasBor = form.FormFrame(out Radius, out Bor);
            }
            else
            {
                SetSize(_control.Size);
                SetLocation(_control.PointToScreen(Point.Empty));
                if (_config.Radius.HasValue) Radius = _config.Radius.Value;
                else if (_control is IControl icontrol) gpath = icontrol.RenderRegion;
            }
        }

        GraphicsPath? gpath = null;
        int Radius = 0, Bor = 0;
        bool HasBor = false;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            spin_core.Start(this);
            if (parent != null)
            {
                parent.LocationChanged += Parent_LocationChanged;
                parent.SizeChanged += Parent_SizeChanged;
            }
        }

        private void Parent_LocationChanged(object? sender, EventArgs e)
        {
            if (control is Form form) SetLocation(form.Location);
            else SetLocation(control.PointToScreen(Point.Empty));
        }
        private void Parent_SizeChanged(object? sender, EventArgs e)
        {
            if (control is Form form)
            {
                SetSize(form.Size);
                SetLocation(form.Location);
            }
            else
            {
                SetLocation(control.PointToScreen(Point.Empty));
                SetSize(control.Size);
                if (!config.Radius.HasValue && control is IControl icontrol)
                {
                    gpath?.Dispose();
                    gpath = icontrol.RenderRegion;
                }
            }
        }

        #region 渲染

        SpinCore spin_core = new SpinCore();
        public override Bitmap PrintBit()
        {
            Rectangle rect_read = TargetRectXY, rect = HasBor ? new Rectangle(Bor, 0, rect_read.Width - Bor * 2, rect_read.Height - Bor) : rect_read;
            var original_bmp = new Bitmap(rect_read.Width, rect_read.Height);
            using (var g = Graphics.FromImage(original_bmp).HighLay(true))
            {
                using (var brush = new SolidBrush(config.Back ?? Style.rgba(Colour.BgBase.Get("Spin"), .8F)))
                {
                    if (gpath != null) g.Fill(brush, gpath);
                    else if (Radius > 0)
                    {
                        using (var path = rect.RoundPath(Radius))
                        {
                            g.Fill(brush, path);
                        }
                    }
                    else g.Fill(brush, rect);
                }
                spin_core.Paint(g, rect, config, this);
            }
            return original_bmp;
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            spin_core.Dispose();
            if (parent != null)
            {
                parent.LocationChanged -= Parent_LocationChanged;
                parent.SizeChanged -= Parent_SizeChanged;
            }
            gpath?.Dispose();
            base.Dispose(disposing);
            if (control == null) return;
        }
    }
}