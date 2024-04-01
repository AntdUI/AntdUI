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
    /// Spin 加载中
    /// </summary>
    /// <remarks>用于页面和区块的加载中状态。</remarks>
    [Description("Spin 加载中")]
    [ToolboxItem(true)]
    public class Spin : IControl
    {
        #region 属性

        [Description("颜色"), Category("外观"), DefaultValue(null)]
        public Color? Fill { get; set; }

        string? text = null;
        /// <summary>
        /// 文本
        /// </summary>
        [Description("文本"), Category("外观"), DefaultValue(null)]
        public new string? Text
        {
            get => text;
            set
            {
                if (text == value) return;
                text = value;
                spin_core.Clear();
                Invalidate();
            }
        }

        #endregion

        #region 动画

        SpinCore spin_core = new SpinCore();
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
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
            spin_core.Paint(e.Graphics.High(), rect, text, Fill.HasValue ? Fill.Value : Style.Db.Primary, this);
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
            /// 颜色
            /// </summary>
            public Color? Color { get; set; }

            /// <summary>
            /// 圆角
            /// </summary>
            public int? Radius { get; set; }
        }
    }

    internal class SpinCore : IDisposable
    {
        ITask? thread = null;

        float LineWidth = 6, LineAngle = 0;
        float prog_size = 0;
        public void Clear()
        {
            prog_size = 0;
        }

        public void Start(IControl control)
        {
            bool ProgState = false;
            thread = new ITask(control, () =>
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
                control.Invalidate();
                return true;
            }, 10);
        }
        public void Start(ILayeredForm control)
        {
            bool ProgState = false;
            thread = new ITask(control, () =>
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
                control.Print();
                return true;
            }, 10);
        }

        public void Paint(Graphics g, Rectangle rect, string? text, Color color, Control control)
        {
            if (prog_size == 0) prog_size = g.MeasureString(text ?? Config.NullText, control.Font).Height;

            float rprog_size = prog_size * 1.4F, size = prog_size * .1F, size2 = prog_size / 2F;

            var rect_prog = new RectangleF(rect.X + (rect.Width - rprog_size) / 2, rect.Y + (rect.Height - rprog_size) / 2, rprog_size, rprog_size);
            if (text != null)
            {
                var y = rect_prog.Bottom;
                rect_prog.Offset(0, -size2);
                using (var brush = new SolidBrush(control.ForeColor))
                {
                    g.DrawString(text, control.Font, brush, new RectangleF(rect.X, y, rect.Width, prog_size), Helper.stringFormatCenter);
                }
            }
            using (var brush = new Pen(color, size))
            {
                //g.DrawEllipse(brush, rect_prog);
                //brush.Color = Color;
                brush.StartCap = brush.EndCap = LineCap.Round;
                g.DrawArc(brush, rect_prog, LineAngle, LineWidth * 3.6F);
            }
        }

        public void Dispose()
        {
            thread?.Dispose();
        }
    }
    internal class SpinForm : ILayeredFormOpacity
    {
        readonly Control? ocontrol = null;

        Spin.Config config;
        public SpinForm(Control control, Spin.Config _config)
        {
            Font = control.Font;
            config = _config;
            ocontrol = control;
            SetSize(control.Size);
            SetLocation(control.PointToScreen(Point.Empty));
            if (_config.Radius.HasValue) Radius = _config.Radius.Value;
            else
            {
                if (control is IControl _control) gpath = _control.RenderRegion;
            }
        }

        GraphicsPath? gpath = null;
        int Radius = 0;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            spin_core.Start(this);
        }

        #region 渲染

        SpinCore spin_core = new SpinCore();
        public override Bitmap PrintBit()
        {
            var rect = TargetRectXY;
            var original_bmp = new Bitmap(rect.Width, rect.Height);
            using (var g = Graphics.FromImage(original_bmp).High())
            {
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                using (var brush = new SolidBrush(config.Back.HasValue ? config.Back.Value : Color.FromArgb(100, Style.Db.TextBase)))
                {
                    if (gpath != null)
                    {
                        g.FillPath(brush, gpath);
                    }
                    else if (Radius > 0)
                    {
                        using (var path = rect.RoundPath(Radius)) { g.FillPath(brush, path); }
                    }
                    else g.FillRectangle(brush, rect);
                }
                spin_core.Paint(g, rect, config.Text, config.Color.HasValue ? config.Color.Value : Style.Db.Primary, this);
            }
            return original_bmp;
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            spin_core.Dispose();
            base.Dispose(disposing);
            if (ocontrol == null) return;
        }
    }
}