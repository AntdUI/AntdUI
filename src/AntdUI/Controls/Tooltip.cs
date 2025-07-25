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
        [Description("箭头大小"), Category("外观"), DefaultValue(null)]
        public int? ArrowSize { get; set; }

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

        /// <summary>
        /// 背景色
        /// </summary>
        [Description("背景色"), Category("外观"), DefaultValue(null)]
        public Color? Back { get; set; }
        /// <summary>
        /// 前景色
        /// </summary>
        [Description("前景色"), Category("外观"), DefaultValue(null)]
        public Color? Fore { get; set; }

        #endregion

        #region 渲染

        int arrowSize = 0;
        readonly StringFormat s_c = Helper.SF_NoWrap(), s_l = Helper.SF(lr: StringAlignment.Near);
        protected override void OnDraw(DrawEventArgs e)
        {
            var g = e.Canvas;
            MaximumSize = MinimumSize = this.RenderMeasure(g, null, out var multiline, out _, out arrowSize);
            this.Render(g, e.Rect, multiline, arrowSize, -1, s_c, s_l);
            base.OnDraw(e);
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
            public Rectangle? Offset { get; set; }

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
            public int? ArrowSize { get; set; }

            /// <summary>
            /// 箭头方向
            /// </summary>
            public TAlign ArrowAlign { get; set; } = TAlign.Top;

            /// <summary>
            /// 自定义宽度
            /// </summary>
            public int? CustomWidth { get; set; }

            /// <summary>
            /// 背景色
            /// </summary>
            public Color? Back { get; set; }

            /// <summary>
            /// 前景色
            /// </summary>
            public Color? Fore { get; set; }
        }

        #endregion
    }

    internal class TooltipForm : ILayeredFormOpacity, ITooltip
    {
        Control ocontrol;
        bool multiline = false;
        int? maxWidth;
        int arrowSize = 0, arrowX = -1;
        public override bool MessageEnable => true;
        public TooltipForm(Control control, string txt, ITooltipConfig component) : base(240)
        {
            ocontrol = control;
            control.Parent.SetTopMost(Handle);
            MessageCloseMouseLeave = true;
            Text = txt;
            Font = component.Font ?? Config.Font ?? control.Font;
            ArrowSize = component.ArrowSize;
            Radius = component.Radius;
            ArrowAlign = component.ArrowAlign;
            CustomWidth = component.CustomWidth;
            Back = component.Back;
            Fore = component.Fore;
            var point = control.PointToScreen(Point.Empty);
            var screen = Screen.FromPoint(point).WorkingArea;
            maxWidth = screen.Width;
            int gap = 0;
            Helper.GDI(g => SetSize(this.RenderMeasure(g, maxWidth, out multiline, out gap, out arrowSize)));
            if (component is Tooltip.Config config && config.Offset.HasValue)
            {
                var align = ArrowAlign;
                new CalculateCoordinate(control, TargetRect, arrowSize, gap, gap * 2, config.Offset.Value) { iscreen = screen }.Auto(ref align, gap + (int)(Radius * Config.Dpi), out int x, out int y, out arrowX);
                ArrowAlign = align;
                SetLocation(x, y);
            }
            else
            {
                var align = ArrowAlign;
                new CalculateCoordinate(control, TargetRect, arrowSize, gap, gap * 2).Auto(ref align, gap + (int)(Radius * Config.Dpi), out int x, out int y, out arrowX);
                ArrowAlign = align;
                SetLocation(x, y);
            }
            control.Disposed += Control_Close;
        }
        public TooltipForm(Control control, Rectangle rect, string txt, ITooltipConfig component, bool hasmax = true) : base(240)
        {
            ocontrol = control;
            control.SetTopMost(Handle);
            MessageCloseMouseLeave = true;
            Text = txt;
            Font = component.Font ?? Config.Font ?? control.Font;
            ArrowSize = component.ArrowSize;
            Radius = component.Radius;
            ArrowAlign = component.ArrowAlign;
            CustomWidth = component.CustomWidth;
            Back = component.Back;
            Fore = component.Fore;
            if (hasmax) maxWidth = control.Width;
            int gap = 0;
            Helper.GDI(g => SetSize(this.RenderMeasure(g, maxWidth, out multiline, out gap, out arrowSize)));
            var align = ArrowAlign;
            new CalculateCoordinate(control, TargetRect, arrowSize, gap, gap * 2, rect).Auto(ref align, gap + (int)(Radius * Config.Dpi), out int x, out int y, out arrowX);
            ArrowAlign = align;
            SetLocation(x, y);
            control.Disposed += Control_Close;
        }

        public override string name => nameof(Tooltip);

        public void SetText(Rectangle rect, string text)
        {
            Text = text;
            int gap = 0;
            Helper.GDI(g => SetSize(this.RenderMeasure(g, maxWidth, out multiline, out gap, out arrowSize)));
            var align = ArrowAlign;
            new CalculateCoordinate(ocontrol, TargetRect, arrowSize, gap, gap * 2, rect).Auto(ref align, gap + (int)(Radius * Config.Dpi), out int x, out int y, out arrowX);
            ArrowAlign = align;
            SetLocation(x, y);
            Print();
        }

        private void Control_Close(object? sender, EventArgs e) => IClose();

        #region 参数

        /// <summary>
        /// 圆角
        /// </summary>
        [Description("圆角"), Category("外观"), DefaultValue(6)]
        public int Radius { get; set; } = 6;

        /// <summary>
        /// 箭头大小
        /// </summary>
        [Description("箭头大小"), Category("外观"), DefaultValue(null)]
        public int? ArrowSize { get; set; }

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

        /// <summary>
        /// 背景色
        /// </summary>
        [Description("背景色"), Category("外观"), DefaultValue(null)]
        public Color? Back { get; set; }

        /// <summary>
        /// 前景色
        /// </summary>
        [Description("前景色"), Category("外观"), DefaultValue(null)]
        public Color? Fore { get; set; }

        #endregion

        #region 渲染

        readonly StringFormat s_c = Helper.SF_NoWrap(), s_l = Helper.SF(lr: StringAlignment.Near);
        public override Bitmap PrintBit()
        {
            var rect = TargetRectXY;
            Bitmap original_bmp = new Bitmap(rect.Width, rect.Height);
            using (var g = Graphics.FromImage(original_bmp).High())
            {
                this.Render(g, rect, multiline, arrowSize, arrowX, s_c, s_l);
            }
            return original_bmp;
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            ocontrol.Disposed -= Control_Close;
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
        [Description("箭头大小"), Category("外观"), DefaultValue(null)]
        public int? ArrowSize { get; set; }

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

        /// <summary>
        /// 背景色
        /// </summary>
        [Description("背景色"), Category("外观"), DefaultValue(null)]
        public Color? Back { get; set; }

        /// <summary>
        /// 前景色
        /// </summary>
        [Description("前景色"), Category("外观"), DefaultValue(null)]
        public Color? Fore { get; set; }

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

        public static Size RenderMeasure(this ITooltip core, Canvas g, int? maxWidth, out bool multiline, out int gap, out int arrowSize)
        {
            multiline = core.Text.Contains("\n");
            gap = (int)(3 * Config.Dpi);
            int gap2 = gap * 2, paddingy = (int)(6 * Config.Dpi) * 2 + gap2, paddingx = (int)(8 * Config.Dpi) * 2 + gap2;
            var font_size = g.MeasureText(core.Text, core.Font);
            if (core.ArrowSize.HasValue) arrowSize = (int)(core.ArrowSize * Config.Dpi);
            else arrowSize = (int)(font_size.Height * 0.3F);
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
                int width = maxWidth.Value - paddingx;
                if (font_size.Width > width)
                {
                    font_size = g.MeasureText(core.Text, core.Font, width);
                    multiline = true;
                }
            }
            switch (core.ArrowAlign)
            {
                case TAlign.None:
                    return new Size(font_size.Width + paddingx, font_size.Height + paddingy);
                case TAlign.Bottom:
                case TAlign.BL:
                case TAlign.BR:
                case TAlign.Top:
                case TAlign.TL:
                case TAlign.TR:
                    return new Size(font_size.Width + paddingx, font_size.Height + paddingy + arrowSize);
                default:
                    return new Size(font_size.Width + paddingx + arrowSize, font_size.Height + paddingy);
            }
        }

        public static void Render(this ITooltip core, Canvas g, Rectangle rect, bool multiline, int arrowSize, int arrowX, StringFormat s_c, StringFormat s_l)
        {
            int gap = (int)(3 * Config.Dpi), paddingy = (int)(6 * Config.Dpi), paddingx = (int)(8 * Config.Dpi), gap2 = gap * 2, paddingy2 = paddingy * 2, paddingx2 = paddingx * 2;
            int radius = (int)(core.Radius * Config.Dpi);
            using (var brush = new SolidBrush(core.Back ?? (Config.Mode == TMode.Dark ? Color.FromArgb(66, 66, 66) : Color.FromArgb(38, 38, 38))))
            {
                if (core.ArrowAlign == TAlign.None)
                {
                    Rectangle rect_shadow = new Rectangle(rect.X + gap, rect.Y + gap, rect.Width - gap2, rect.Height - gap2),
                        rect_text = new Rectangle(rect_shadow.X + paddingx, rect_shadow.Y + paddingy, rect_shadow.Width - paddingx2, rect_shadow.Height - paddingy2);
                    using (var path = rect_shadow.RoundPath(radius))
                    {
                        DrawShadow(core, g, radius, rect, rect_shadow, path);
                        g.Fill(brush, path);
                    }
                    g.DrawText(core.Text, core.Font, core.Fore ?? Color.White, rect_text, multiline ? s_l : s_c);
                }
                else
                {
                    int arrows2 = arrowSize / 2;
                    Rectangle rectf, rect_shadow;
                    switch (core.ArrowAlign.AlignMini())
                    {
                        case TAlignMini.Top:
                            rect_shadow = new Rectangle(rect.X + gap, rect.Y + gap, rect.Width - gap2, rect.Height - gap2 - arrowSize);
                            rectf = new Rectangle(rect.X, rect.Y, rect.Width, rect.Height - arrows2);
                            break;
                        case TAlignMini.Bottom:
                            rect_shadow = new Rectangle(rect.X + gap, rect.Y + gap + arrowSize, rect.Width - gap2, rect.Height - gap2 - arrowSize);
                            rectf = new Rectangle(rect.X, rect.Y + arrows2, rect.Width, rect.Height - arrows2);
                            break;
                        case TAlignMini.Left:
                            //左
                            rect_shadow = new Rectangle(rect.X + gap, rect.Y + gap, rect.Width - gap2 - arrowSize, rect.Height - gap2);
                            rectf = new Rectangle(rect.X, rect.Y, rect.Width - arrows2, rect.Height);
                            break;
                        default:
                            //右
                            rect_shadow = new Rectangle(rect.X + gap + arrowSize, rect.Y + gap, rect.Width - gap2 - arrowSize, rect.Height - gap2);
                            rectf = new Rectangle(rect.X + arrows2, rect.Y, rect.Width - arrows2, rect.Height);
                            break;
                    }
                    var rect_text = new Rectangle(rect_shadow.X + paddingx, rect_shadow.Y + paddingy, rect_shadow.Width - paddingx2, rect_shadow.Height - paddingy2);
                    using (var path = rect_shadow.RoundPath(radius))
                    {
                        DrawShadow(core, g, radius, rectf, rect_shadow, path);
                        g.Fill(brush, path);
                        if (arrowX > -1) g.FillPolygon(brush, core.ArrowAlign.AlignLines(arrowSize, rect, rect_shadow, arrowX));
                        else g.FillPolygon(brush, core.ArrowAlign.AlignLines(arrowSize, rect, rect_shadow));
                    }
                    g.DrawText(core.Text, core.Font, core.Fore ?? Color.White, rect_text, multiline ? s_l : s_c);
                }
            }
        }

        static void DrawShadow(this ITooltip core, Canvas g, int radius, Rectangle rect, Rectangle rect_shadow, GraphicsPath path2)
        {
            using (var path = rect.RoundPath(radius))
            {
                path.AddPath(path2, false);
                Color ct = Color.Transparent, ctb = Color.FromArgb(120, 0, 0, 0);
                using (var brush = new PathGradientBrush(path))
                {
                    brush.CenterPoint = new PointF(rect.Width / 2F, rect.Height / 2F);
                    brush.CenterColor = ctb;
                    brush.SurroundColors = new Color[] { ct };
                    g.Fill(brush, path);
                }
            }
        }

        #endregion
    }

    public class TooltipConfig : ITooltipConfig
    {
        public Font? Font { get; set; }
        public int Radius { get; set; } = 6;
        public int? ArrowSize { get; set; }
        public TAlign ArrowAlign { get; set; } = TAlign.Top;
        public int? CustomWidth { get; set; }
        public Color? Back { get; set; }
        public Color? Fore { get; set; }
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
        int? ArrowSize { get; set; }

        /// <summary>
        /// 箭头方向
        /// </summary>
        TAlign ArrowAlign { get; set; }

        /// <summary>
        /// 设定宽度
        /// </summary>
        int? CustomWidth { get; set; }

        /// <summary>
        /// 背景色
        /// </summary>
        Color? Back { get; set; }

        /// <summary>
        /// 前景色
        /// </summary>
        Color? Fore { get; set; }
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
        int? ArrowSize { get; set; }

        /// <summary>
        /// 箭头方向
        /// </summary>
        TAlign ArrowAlign { get; set; }

        /// <summary>
        /// 设定宽度
        /// </summary>
        int? CustomWidth { get; set; }

        /// <summary>
        /// 背景色
        /// </summary>
        Color? Back { get; set; }

        /// <summary>
        /// 前景色
        /// </summary>
        Color? Fore { get; set; }
    }

    #endregion
}