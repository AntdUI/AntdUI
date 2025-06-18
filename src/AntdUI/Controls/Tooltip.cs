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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// Tooltip 文字提示
    /// </summary>
    /// <remarks>简单的文字提示气泡框。</remarks>
    [Description("Tooltip 文字提示")]
    [ToolboxItem(true)]
    public class Tooltip : IControl, ITooltip
    {
        #region 参数

        string? text;
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
                Invalidate();
            }
        }

        [Description("文本"), Category("国际化"), DefaultValue(null)]
        public string? LocalizationText { get; set; }

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
        /// 箭头大小
        /// </summary>
        [Description("箭头大小"), Category("外观"), DefaultValue(8)]
        public int ArrowSize { get; set; } = 8;

        /// <summary>
        /// 箭头方向
        /// </summary>
        [Description("箭头方向"), Category("外观"), DefaultValue(TAlign.Top)]
        public TAlign ArrowAlign { get; set; } = TAlign.Top;

        /// <summary>
        /// 自定义宽度
        /// </summary>
        [Description("自定义宽度"), Category("外观"), DefaultValue(null)]
        public int? CustomWidth { get; set; }

        #endregion

        #region 渲染

        readonly StringFormat s_c = Helper.SF_NoWrap(), s_l = Helper.SF(lr: StringAlignment.Near);
        protected override void OnPaint(PaintEventArgs e)
        {
            var rect = ClientRectangle;
            var g = e.Graphics.High();
            MaximumSize = MinimumSize = this.RenderMeasure(g, null, out var multiline);
            this.Render(g, rect, multiline, s_c, s_l);
            base.OnPaint(e);
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// Tooltip 文字提示
        /// </summary>
        /// <param name="control">所属控件</param>
        /// <param name="text">文本</param>
        /// <param name="ArrowAlign">箭头方向</param>
        public static Form? open(Control control, string text, TAlign ArrowAlign = TAlign.Top) => open(new Config(control, text) { ArrowAlign = ArrowAlign });

        /// <summary>
        /// Tooltip 文字提示
        /// </summary>
        /// <param name="control">所属控件</param>
        /// <param name="text">文本</param>
        /// <param name="rect">偏移量</param>
        /// <param name="ArrowAlign">箭头方向</param>
        public static Form? open(Control control, string text, Rectangle rect, TAlign ArrowAlign = TAlign.Top) => open(new Config(control, text) { Offset = rect, ArrowAlign = ArrowAlign });

        /// <summary>
        /// Tooltip 文字提示
        /// </summary>
        /// <param name="config">配置</param>
        public static Form? open(Config config)
        {
            if (config.Control.IsHandleCreated)
            {
                if (config.Control.InvokeRequired) return ITask.Invoke(config.Control, new Func<Form?>(() => open(config)));
                var tip = new TooltipForm(config.Control, config.Text, config);
                tip.Show(config.Control);
                return tip;
            }
            return null;
        }

        /// <summary>
        /// 配置
        /// </summary>
        public class Config : ITooltipConfig
        {
            /// <summary>
            /// Tooltip 配置
            /// </summary>
            /// <param name="control">所属控件</param>
            /// <param name="text">文本</param>
            public Config(Control control, string text)
            {
                Font = control.Font;
                Control = control;
                Text = text;
            }

            /// <summary>
            /// 所属控件
            /// </summary>
            public Control Control { get; set; }

            /// <summary>
            /// 偏移量
            /// </summary>
            public object? Offset { get; set; }

            /// <summary>
            /// 字体
            /// </summary>
            public Font? Font { get; set; }

            /// <summary>
            /// 文本
            /// </summary>
            public string Text { get; set; }

            /// <summary>
            /// 圆角
            /// </summary>
            public int Radius { get; set; } = 6;

            /// <summary>
            /// 箭头大小
            /// </summary>
            public int ArrowSize { get; set; } = 8;

            /// <summary>
            /// 箭头方向
            /// </summary>
            public TAlign ArrowAlign { get; set; } = TAlign.Top;

            /// <summary>
            /// 自定义宽度
            /// </summary>
            public int? CustomWidth { get; set; }
        }

        #endregion
    }

    internal class TooltipForm : ILayeredFormOpacity, ITooltip
    {
        readonly Control? ocontrol = null;
        bool multiline = false;
        int? maxWidth;
        public TooltipForm(Control control, string txt, ITooltipConfig component)
        {
            ocontrol = control;
            control.Parent.SetTopMost(Handle);
            Text = txt;
            if (component.Font != null) Font = component.Font;
            else if (Config.Font != null) Font = Config.Font;
            else Font = control.Font;
            ArrowSize = component.ArrowSize;
            Radius = component.Radius;
            ArrowAlign = component.ArrowAlign;
            CustomWidth = component.CustomWidth;
            var point = control.PointToScreen(Point.Empty);
            var screen = Screen.FromPoint(point).WorkingArea;
            maxWidth = screen.Width;
            Helper.GDI(g => SetSize(this.RenderMeasure(g, maxWidth, out multiline)));
            if (component is Tooltip.Config config)
            {
                if (config.Offset is RectangleF rectf) SetLocation(ArrowAlign.AlignPoint(new Rectangle(point.X + (int)rectf.X, point.Y + (int)rectf.Y, (int)rectf.Width, (int)rectf.Height), TargetRect.Width, TargetRect.Height));
                else if (config.Offset is Rectangle rect) SetLocation(ArrowAlign.AlignPoint(new Rectangle(point.X + rect.X, point.Y + rect.Y, rect.Width, rect.Height), TargetRect.Width, TargetRect.Height));
                else SetLocation(ArrowAlign.AlignPoint(point, control.Size, TargetRect.Width, TargetRect.Height));
            }
            else SetLocation(ArrowAlign.AlignPoint(point, control.Size, TargetRect.Width, TargetRect.Height));
            control.LostFocus += Control_LostFocus;
            control.MouseLeave += Control_LostFocus;
            if (component.ArrowAlign == TAlign.Left || component.ArrowAlign == TAlign.Right || component.ArrowAlign == TAlign.RB || component.ArrowAlign == TAlign.RT || component.ArrowAlign == TAlign.LT || component.ArrowAlign == TAlign.LB) return;
            if (TargetRect.X < screen.X) SetLocationX(screen.X);
            else if (TargetRect.X > (screen.X + screen.Width) - TargetRect.Width) SetLocationX(screen.Right - TargetRect.Width);
        }
        public TooltipForm(Control control, Rectangle rect, string txt, ITooltipConfig component)
        {
            ocontrol = control;
            control.SetTopMost(Handle);
            Text = txt;
            if (component.Font != null) Font = component.Font;
            else if (Config.Font != null) Font = Config.Font;
            else Font = control.Font;
            ArrowSize = component.ArrowSize;
            Radius = component.Radius;
            ArrowAlign = component.ArrowAlign;
            CustomWidth = component.CustomWidth;
            var point = control.PointToScreen(Point.Empty);
            var screen = Screen.FromPoint(point).WorkingArea;
            maxWidth = screen.Width;
            Helper.GDI(g =>
            {
                SetSize(this.RenderMeasure(g, maxWidth, out multiline));
            });
            SetLocation(ArrowAlign.AlignPoint(rect, TargetRect));
            if (component.ArrowAlign == TAlign.Left || component.ArrowAlign == TAlign.Right || component.ArrowAlign == TAlign.RB || component.ArrowAlign == TAlign.RT || component.ArrowAlign == TAlign.LT || component.ArrowAlign == TAlign.LB) return;
            if (TargetRect.X < screen.X) SetLocationX(screen.X);
            else if (TargetRect.X > (screen.X + screen.Width) - TargetRect.Width) SetLocationX(screen.Right - TargetRect.Width);
        }

        public override string name => nameof(Tooltip);

        public void SetText(Rectangle rect, string text)
        {
            Text = text;
            Helper.GDI(g =>
            {
                SetSize(this.RenderMeasure(g, maxWidth, out multiline));
            });
            SetLocation(ArrowAlign.AlignPoint(rect, TargetRect));
            Print();
        }

        private void Control_LostFocus(object? sender, EventArgs e) => IClose();

        #region 参数

        /// <summary>
        /// 圆角
        /// </summary>
        [Description("圆角"), Category("外观"), DefaultValue(6)]
        public int Radius { get; set; } = 6;

        /// <summary>
        /// 箭头大小
        /// </summary>
        [Description("箭头大小"), Category("外观"), DefaultValue(8)]
        public int ArrowSize { get; set; } = 8;

        /// <summary>
        /// 箭头方向
        /// </summary>
        [Description("箭头方向"), Category("外观"), DefaultValue(TAlign.Top)]
        public TAlign ArrowAlign { get; set; } = TAlign.Top;

        /// <summary>
        /// 自定义宽度
        /// </summary>
        [Description("自定义宽度"), Category("外观"), DefaultValue(null)]
        public int? CustomWidth { get; set; }

        #endregion

        #region 渲染

        readonly StringFormat s_c = Helper.SF_NoWrap(), s_l = Helper.SF(lr: StringAlignment.Near);
        public override Bitmap PrintBit()
        {
            var rect = TargetRectXY;
            Bitmap original_bmp = new Bitmap(rect.Width, rect.Height);
            using (var g = Graphics.FromImage(original_bmp).High())
            {
                this.Render(g, rect, multiline, s_c, s_l);
            }
            return original_bmp;
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (ocontrol == null) return;
            ocontrol.LostFocus -= Control_LostFocus;
            ocontrol.MouseLeave -= Control_LostFocus;
        }
    }

    [ProvideProperty("Tip", typeof(Control)), Description("提示")]
    public partial class TooltipComponent : Component, IExtenderProvider, ITooltipConfig
    {
        public bool CanExtend(object target) => target is Control;

        #region 属性

        /// <summary>
        /// 字体
        /// </summary>
        [Description("字体"), DefaultValue(null)]
        public Font? Font { get; set; }

        /// <summary>
        /// 圆角
        /// </summary>
        [Description("圆角"), Category("外观"), DefaultValue(6)]
        public int Radius { get; set; } = 6;

        /// <summary>
        /// 箭头大小
        /// </summary>
        [Description("箭头大小"), Category("外观"), DefaultValue(8)]
        public int ArrowSize { get; set; } = 8;

        /// <summary>
        /// 箭头方向
        /// </summary>
        [Description("箭头方向"), Category("外观"), DefaultValue(TAlign.Top)]
        public TAlign ArrowAlign { get; set; } = TAlign.Top;

        /// <summary>
        /// 自定义宽度
        /// </summary>
        [Description("自定义宽度"), Category("外观"), DefaultValue(null)]
        public int? CustomWidth { get; set; }

        #endregion

        readonly Dictionary<Control, string> dic = new Dictionary<Control, string>();

        [Description("设置是否提示"), DefaultValue(null)]
        [Editor(typeof(System.ComponentModel.Design.MultilineStringEditor), typeof(UITypeEditor))]
        [Localizable(true)]
        public string? GetTip(Control item)
        {
            if (dic.TryGetValue(item, out string? value)) return value;
            return null;
        }
        public void SetTip(Control control, string? val)
        {
            if (val == null)
            {
                if (dic.ContainsKey(control))
                {
                    dic.Remove(control);
                    control.MouseEnter -= Control_Enter;
                    control.MouseLeave -= Control_Leave;
                    control.Leave -= Control_Leave;
                }
                return;
            }
            if (dic.ContainsKey(control)) dic[control] = val.Trim();
            else
            {
                dic.Add(control, val.Trim());
                control.MouseEnter += Control_Enter;
                control.MouseLeave += Control_Leave;
                control.Leave -= Control_Leave;
            }
        }

        readonly List<Control> dic_in = new List<Control>();
        private void Control_Leave(object? sender, EventArgs e)
        {
            if (sender != null && sender is Control obj)
                lock (dic_in) dic_in.Remove(obj);
        }

        private void Control_Enter(object? sender, EventArgs e)
        {
            if (sender != null && sender is Control obj)
            {
                lock (dic_in) dic_in.Add(obj);
                ITask.Run(() =>
                {
                    Thread.Sleep(500);
                    if (dic_in.Contains(obj))
                    {
                        obj.BeginInvoke(new Action(() =>
                        {
                            new TooltipForm(obj, dic[obj], this).Show();
                        }));
                    }
                });
            }
        }
    }

    #region 核心渲染

    internal static class ITooltipLib
    {
        #region 渲染

        public static Size RenderMeasure(this ITooltip core, Canvas g, int? maxWidth, out bool multiline)
        {
            multiline = core.Text.Contains("\n");
            int padding = (int)Math.Ceiling(20 * Config.Dpi), paddingx = (int)Math.Ceiling(24 * Config.Dpi);
            var font_size = g.MeasureText(core.Text, core.Font);
            if (core.CustomWidth.HasValue)
            {
                int width = (int)Math.Ceiling(core.CustomWidth.Value * Config.Dpi);
                if (font_size.Width > width)
                {
                    font_size = g.MeasureText(core.Text, core.Font, width);
                    multiline = true;
                }
            }
            else if (maxWidth.HasValue)
            {
                int width = maxWidth.Value - padding;
                if (font_size.Width > width)
                {
                    font_size = g.MeasureText(core.Text, core.Font, width);
                    multiline = true;
                }
            }
            if (core.ArrowAlign == TAlign.None) return new Size(font_size.Width + paddingx, font_size.Height + padding);
            if (core.ArrowAlign == TAlign.Bottom || core.ArrowAlign == TAlign.BL || core.ArrowAlign == TAlign.BR || core.ArrowAlign == TAlign.Top || core.ArrowAlign == TAlign.TL || core.ArrowAlign == TAlign.TR)
                return new Size(font_size.Width + paddingx, font_size.Height + padding + core.ArrowSize);
            else return new Size(font_size.Width + paddingx + core.ArrowSize, font_size.Height + padding);
        }

        public static void Render(this ITooltip core, Canvas g, Rectangle rect, bool multiline, StringFormat s_c, StringFormat s_l)
        {
            int gap = (int)Math.Ceiling(5 * Config.Dpi), padding = gap * 2, padding2 = padding * 2;
            using (var brush = new SolidBrush(Config.Mode == TMode.Dark ? Color.FromArgb(66, 66, 66) : Color.FromArgb(38, 38, 38)))
            {
                if (core.ArrowAlign == TAlign.None)
                {
                    var rect_read = new Rectangle(rect.X + 5, rect.Y + 5, rect.Width - 10, rect.Height - 10);
                    using (var path = rect_read.RoundPath(core.Radius))
                    {
                        DrawShadow(core, g, rect, rect_read, 3, path);
                        g.Fill(brush, path);
                    }
                    RenderText(core, g, rect_read, multiline, padding, padding2, s_c, s_l);
                }
                else
                {
                    Rectangle rect_text;
                    switch (core.ArrowAlign.AlignMini())
                    {
                        case TAlignMini.Top:
                            rect_text = new Rectangle(rect.X, rect.Y, rect.Width, rect.Height - core.ArrowSize);
                            break;
                        case TAlignMini.Bottom:
                            rect_text = new Rectangle(rect.X, rect.Y + core.ArrowSize, rect.Width, rect.Height - core.ArrowSize);
                            break;
                        case TAlignMini.Left:
                            //左
                            rect_text = new Rectangle(rect.X, rect.Y, rect.Width - core.ArrowSize, rect.Height);
                            break;
                        default:
                            //右
                            rect_text = new Rectangle(rect.X + core.ArrowSize, rect.Y, rect.Width - core.ArrowSize, rect.Height);
                            break;
                    }
                    var rect_read = new Rectangle(rect_text.X + gap, rect_text.Y + gap, rect_text.Width - padding, rect_text.Height - padding);
                    using (var path = rect_read.RoundPath(core.Radius))
                    {
                        DrawShadow(core, g, rect, rect_read, 3, path);
                        g.Fill(brush, path);
                    }
                    g.FillPolygon(brush, core.ArrowAlign.AlignLines(core.ArrowSize, rect, rect_read));
                    RenderText(core, g, rect_text, multiline, padding, padding2, s_c, s_l);
                }
            }
        }

        static void RenderText(ITooltip core, Canvas g, Rectangle rect, bool multiline, int padding, int padding2, StringFormat s_c, StringFormat s_l)
        {
            if (multiline) g.DrawText(core.Text, core.Font, Brushes.White, new Rectangle(rect.X + padding, rect.Y + padding, rect.Width - padding2, rect.Height - padding2), s_l);
            else g.DrawText(core.Text, core.Font, Brushes.White, rect, s_c);
        }

        static void DrawShadow(this ITooltip core, Canvas _g, Rectangle brect, Rectangle rect, int size, GraphicsPath path2)
        {
            using (var bmp = new Bitmap(brect.Width, brect.Height))
            {
                using (var g = Graphics.FromImage(bmp).HighLay())
                {
                    int size2 = size * 2;
                    using (var path = new Rectangle(rect.X - size, rect.Y - size + 2, rect.Width + size2, rect.Height + size2).RoundPath(core.Radius))
                    {
                        path.AddPath(path2, false);
                        using (var brush = new PathGradientBrush(path))
                        {
                            brush.CenterColor = Color.Black;
                            brush.SurroundColors = new Color[] { Color.Transparent };
                            g.Fill(brush, path);
                        }
                    }
                }
                _g.Image(bmp, brect);
            }
        }

        #endregion
    }

    public class TooltipConfig : ITooltipConfig
    {
        public Font? Font { get; set; }
        public int Radius { get; set; } = 6;
        public int ArrowSize { get; set; } = 8;
        public TAlign ArrowAlign { get; set; } = TAlign.Top;
        public int? CustomWidth { get; set; }
    }

    internal interface ITooltipConfig
    {
        /// <summary>
        /// 字体
        /// </summary>
        Font? Font { get; set; }

        /// <summary>
        /// 圆角
        /// </summary>
        int Radius { get; set; }

        /// <summary>
        /// 箭头大小
        /// </summary>
        int ArrowSize { get; set; }

        /// <summary>
        /// 箭头方向
        /// </summary>
        TAlign ArrowAlign { get; set; }

        /// <summary>
        /// 设定宽度
        /// </summary>
        int? CustomWidth { get; set; }
    }

    internal interface ITooltip
    {
        /// <summary>
        /// 文本
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// 字体
        /// </summary>
        Font Font { get; set; }

        /// <summary>
        /// 圆角
        /// </summary>
        int Radius { get; set; }

        /// <summary>
        /// 箭头大小
        /// </summary>
        int ArrowSize { get; set; }

        /// <summary>
        /// 箭头方向
        /// </summary>
        TAlign ArrowAlign { get; set; }

        /// <summary>
        /// 设定宽度
        /// </summary>
        int? CustomWidth { get; set; }
    }

    #endregion
}