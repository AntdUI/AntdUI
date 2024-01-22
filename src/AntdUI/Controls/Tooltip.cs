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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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
        [Description("箭头大小"), Category("外观"), DefaultValue(8F)]
        public float ArrowSize { get; set; } = 8F;

        /// <summary>
        /// 箭头方向
        /// </summary>
        [Description("箭头方向"), Category("外观"), DefaultValue(TAlign.Top)]
        public TAlign ArrowAlign { get; set; } = TAlign.Top;

        #endregion

        #region 渲染

        protected override void OnPaint(PaintEventArgs e)
        {
            var rect = ClientRectangle;
            var g = e.Graphics.High();
            MaximumSize = MinimumSize = this.RenderMeasure(g);
            this.Render(g, rect, Helper.stringFormatCenter2);
            base.OnPaint(e);
        }

        #endregion
    }

    internal class TooltipForm : ILayeredFormOpacity, ITooltip
    {
        readonly Control? ocontrol = null;
        public TooltipForm(Control control, string txt, ITooltipConfig component)
        {
            ocontrol = control;
            Text = txt;
            if (component.Font != null) Font = component.Font;
            else if (Config.Font != null) Font = Config.Font;
            ArrowSize = component.ArrowSize;
            Radius = component.Radius;
            ArrowAlign = component.ArrowAlign;

            using (var g = Graphics.FromHwnd(Handle).High())
            {
                SetSize(this.RenderMeasure(g));
            }
            var point = control.PointToScreen(Point.Empty);
            SetLocation(ArrowAlign.AlignPoint(point, control.Size, TargetRect.Width, TargetRect.Height));

            control.LostFocus += Control_LostFocus;
            control.MouseLeave += Control_LostFocus;
        }
        public TooltipForm(Rectangle rect, string txt, ITooltipConfig component)
        {
            Text = txt;
            if (component.Font != null) Font = component.Font;
            else if (Config.Font != null) Font = Config.Font;
            ArrowSize = component.ArrowSize;
            Radius = component.Radius;
            ArrowAlign = component.ArrowAlign;

            using (var g = Graphics.FromHwnd(Handle).High())
            {
                SetSize(this.RenderMeasure(g));
            }
            SetLocation(ArrowAlign.AlignPoint(rect, TargetRect));
        }

        public void SetText(Rectangle rect, string text)
        {
            Text = text;
            using (var g = Graphics.FromHwnd(Handle).High())
            {
                SetSize(this.RenderMeasure(g));
            }
            SetLocation(ArrowAlign.AlignPoint(rect, TargetRect));
            Print();
        }

        private void Control_LostFocus(object? sender, EventArgs e)
        {
            IClose();
        }

        #region 参数

        /// <summary>
        /// 圆角
        /// </summary>
        [Description("圆角"), Category("外观"), DefaultValue(6)]
        public int Radius { get; set; } = 6;

        /// <summary>
        /// 箭头大小
        /// </summary>
        [Description("箭头大小"), Category("外观"), DefaultValue(8F)]
        public float ArrowSize { get; set; } = 8F;

        /// <summary>
        /// 箭头方向
        /// </summary>
        [Description("箭头方向"), Category("外观"), DefaultValue(TAlign.Top)]
        public TAlign ArrowAlign { get; set; } = TAlign.Top;

        #endregion

        #region 渲染

        public override Bitmap PrintBit()
        {
            var rect = TargetRectXY;
            Bitmap original_bmp = new Bitmap(rect.Width, rect.Height);
            using (var g = Graphics.FromImage(original_bmp).High())
            {
                this.Render(g, rect, Helper.stringFormatCenter2);
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
        public Font? Font { get; set; } = null;

        /// <summary>
        /// 圆角
        /// </summary>
        [Description("圆角"), Category("外观"), DefaultValue(6)]
        public int Radius { get; set; } = 6;

        /// <summary>
        /// 箭头大小
        /// </summary>
        [Description("箭头大小"), Category("外观"), DefaultValue(8F)]
        public float ArrowSize { get; set; } = 8F;

        /// <summary>
        /// 箭头方向
        /// </summary>
        [Description("箭头方向"), Category("外观"), DefaultValue(TAlign.Top)]
        public TAlign ArrowAlign { get; set; } = TAlign.Top;

        #endregion

        readonly Dictionary<Control, string> dic = new Dictionary<Control, string>();
        [Description("设置是否提示"), DefaultValue(null)]
        public string? GetTip(Control item)
        {
            if (dic.TryGetValue(item, out string? value))
            {
                return value;
            }
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
            if (dic.ContainsKey(control)) dic[control] = val;
            else
            {
                dic.Add(control, val);
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

        public static int Padding = 20;

        public static Size RenderMeasure(this ITooltip core, Graphics g)
        {
            var font_size = g.MeasureString(core.Text, core.Font);
            if (core.ArrowAlign == TAlign.None) return new Size((int)Math.Ceiling(font_size.Width + Padding), (int)Math.Ceiling(font_size.Height + Padding));
            if (core.ArrowAlign == TAlign.Bottom || core.ArrowAlign == TAlign.BL || core.ArrowAlign == TAlign.BR || core.ArrowAlign == TAlign.Top || core.ArrowAlign == TAlign.TL || core.ArrowAlign == TAlign.TR)
                return new Size((int)Math.Ceiling(font_size.Width + Padding), (int)Math.Ceiling(font_size.Height + Padding + core.ArrowSize));
            else return new Size((int)Math.Ceiling(font_size.Width + Padding + core.ArrowSize), (int)Math.Ceiling(font_size.Height + Padding));
        }
        public static void Render(this ITooltip core, Graphics g, Rectangle rect, StringFormat stringFormat)
        {
            RectangleF rect_read;
            using (var brush = new SolidBrush(Config.Mode == TMode.Dark ? Color.FromArgb(66, 66, 66) : Color.FromArgb(38, 38, 38)))
            {
                if (core.ArrowAlign == TAlign.None)
                {
                    rect_read = new RectangleF(rect.X + 5, rect.Y + 5, rect.Width - 10, rect.Height - 10);
                    using (var path = rect_read.RoundPath(core.Radius))
                    {
                        DrawShadow(core, g, rect, rect_read, 3F, path);
                        g.FillPath(brush, path);
                    }
                }
                else
                {
                    switch (core.ArrowAlign.AlignMini())
                    {
                        case TAlignMini.Top:
                            rect_read = new RectangleF(rect.X + 5, rect.Y + 5, rect.Width - 10, rect.Height - 10 - core.ArrowSize);
                            break;
                        case TAlignMini.Bottom:
                            rect_read = new RectangleF(rect.X + 5, rect.Y + 5 + core.ArrowSize, rect.Width - 10, rect.Height - 10 - core.ArrowSize);
                            break;
                        case TAlignMini.Left:
                            //左
                            rect_read = new RectangleF(rect.X + 5, rect.Y + 5, rect.Width - 10 - core.ArrowSize, rect.Height - 10);
                            break;
                        default:
                            //右
                            rect_read = new RectangleF(rect.X + 5 + core.ArrowSize, rect.Y + 5, rect.Width - 10 - core.ArrowSize, rect.Height - 10);
                            break;
                    }
                    using (var path = rect_read.RoundPath(core.Radius))
                    {
                        DrawShadow(core, g, rect, rect_read, 3F, path);
                        g.FillPath(brush, path);
                    }
                    g.FillPolygon(brush, core.ArrowAlign.AlignLines(core.ArrowSize, rect, rect_read));
                }
            }
            g.DrawString(core.Text, core.Font, Brushes.White, rect_read, stringFormat);
        }
        static void DrawShadow(this ITooltip core, Graphics _g, Rectangle brect, RectangleF rect, float size, GraphicsPath path2)
        {
            using (var bmp = new Bitmap(brect.Width, brect.Height))
            {
                using (var g = Graphics.FromImage(bmp))
                {
                    float size2 = size * 2;
                    using (var path = new RectangleF(rect.X - size, rect.Y - size + 2, rect.Width + size2, rect.Height + size2).RoundPath(core.Radius))
                    {
                        path.AddPath(path2, false);
                        using (var brush = new PathGradientBrush(path))
                        {
                            brush.CenterColor = Color.Black;
                            brush.SurroundColors = new Color[] { Color.Transparent };
                            g.FillPath(brush, path);
                        }
                    }
                }
                _g.DrawImage(bmp, brect);
            }
        }

        #endregion
    }

    public class TooltipConfig : ITooltipConfig
    {
        public Font? Font { get; set; }
        public int Radius { get; set; } = 6;
        public float ArrowSize { get; set; } = 8F;
        public TAlign ArrowAlign { get; set; } = TAlign.Top;
    }

    internal interface ITooltipConfig
    {
        Font? Font { get; set; }
        int Radius { get; set; }
        float ArrowSize { get; set; }
        TAlign ArrowAlign { get; set; }
    }
    internal interface ITooltip
    {
        string Text { get; set; }
        Font Font { get; set; }
        int Radius { get; set; }
        float ArrowSize { get; set; }
        TAlign ArrowAlign { get; set; }
    }

    #endregion
}