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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace AntdUI
{
    partial class Helper
    {
        #region 文本布局

        public static readonly StringFormat m_sf = SF_MEASURE_FONT();

        /// <summary>
        /// 文本布局
        /// </summary>
        public static StringFormat SF(TAlign align)
        {
            switch (align)
            {
                case TAlign.Left:
                    return SF(tb: StringAlignment.Center, lr: StringAlignment.Near);
                case TAlign.TL:
                case TAlign.LT:
                    return SF(tb: StringAlignment.Near, lr: StringAlignment.Near);
                case TAlign.Top:
                    return SF(tb: StringAlignment.Near, lr: StringAlignment.Center);
                case TAlign.TR:
                case TAlign.RT:
                    return SF(tb: StringAlignment.Near, lr: StringAlignment.Far);
                case TAlign.Right:
                    return SF(tb: StringAlignment.Center, lr: StringAlignment.Far);
                case TAlign.BR:
                case TAlign.RB:
                    return SF(tb: StringAlignment.Far, lr: StringAlignment.Far);
                case TAlign.Bottom:
                    return SF(tb: StringAlignment.Far, lr: StringAlignment.Center);
                case TAlign.BL:
                case TAlign.LB:
                    return SF(tb: StringAlignment.Far, lr: StringAlignment.Near);
                default: return SF();
            }
        }

        /// <summary>
        /// 文本布局
        /// </summary>
        /// <param name="tb">垂直（上下）</param>
        /// <param name="lr">水平（前后）</param>
        public static StringFormat SF(StringAlignment tb = StringAlignment.Center, StringAlignment lr = StringAlignment.Center)
        {
            var sf = new StringFormat(StringFormat.GenericTypographic) { LineAlignment = tb, Alignment = lr };
            sf.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
            return sf;
        }

        /// <summary>
        /// 文本布局（不换行）
        /// </summary>
        /// <param name="tb">垂直（上下）</param>
        /// <param name="lr">水平（前后）</param>
        public static StringFormat SF_NoWrap(StringAlignment tb = StringAlignment.Center, StringAlignment lr = StringAlignment.Center)
        {
            var sf = new StringFormat(StringFormat.GenericTypographic) { LineAlignment = tb, Alignment = lr };
            sf.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces | StringFormatFlags.NoWrap;
            return sf;
        }

        /// <summary>
        /// 文本布局（超出省略号）
        /// </summary>
        /// <param name="tb">垂直（上下）</param>
        /// <param name="lr">水平（前后）</param>
        public static StringFormat SF_Ellipsis(StringAlignment tb = StringAlignment.Center, StringAlignment lr = StringAlignment.Center)
        {
            var sf = new StringFormat(StringFormat.GenericTypographic) { LineAlignment = tb, Alignment = lr, Trimming = StringTrimming.EllipsisCharacter };
            sf.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
            return sf;
        }

        /// <summary>
        /// 文本布局（超出省略号+不换行）
        /// </summary>
        /// <param name="tb">垂直（上下）</param>
        /// <param name="lr">水平（前后）</param>
        public static StringFormat SF_ALL(StringAlignment tb = StringAlignment.Center, StringAlignment lr = StringAlignment.Center)
        {
            var sf = new StringFormat(StringFormat.GenericTypographic) { LineAlignment = tb, Alignment = lr, Trimming = StringTrimming.EllipsisCharacter };
            sf.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces | StringFormatFlags.NoWrap;
            return sf;
        }

        public static StringFormat SF_MEASURE_FONT()
        {
            var sf = new StringFormat(StringFormat.GenericTypographic) { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            sf.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
            return sf;
        }

        /// <summary>
        /// 文本布局
        /// </summary>
        /// <param name="tb">垂直（上下）</param>
        /// <param name="lr">水平（前后）</param>
        public static TextFormatFlags TF(StringAlignment tb = StringAlignment.Center, StringAlignment lr = StringAlignment.Center)
        {
            TextFormatFlags flags = TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl;
            switch (tb)
            {
                case StringAlignment.Center:
                    flags |= TextFormatFlags.VerticalCenter;
                    break;
                case StringAlignment.Near:
                    flags |= TextFormatFlags.Top;
                    break;
                case StringAlignment.Far:
                default:
                    flags |= TextFormatFlags.Bottom;
                    break;
            }
            switch (lr)
            {
                case StringAlignment.Center:
                    flags |= TextFormatFlags.HorizontalCenter;
                    break;
                case StringAlignment.Near:
                    flags |= TextFormatFlags.Left;
                    break;
                case StringAlignment.Far:
                default:
                    flags |= TextFormatFlags.Right;
                    break;
            }
            return flags;
        }

        /// <summary>
        /// 文本布局
        /// </summary>
        public static TextFormatFlags TF(StringFormat sf)
        {
            TextFormatFlags flags = TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl;
            switch (sf.LineAlignment)
            {
                case StringAlignment.Center:
                    flags |= TextFormatFlags.VerticalCenter;
                    break;
                case StringAlignment.Near:
                    flags |= TextFormatFlags.Top;
                    break;
                case StringAlignment.Far:
                default:
                    flags |= TextFormatFlags.Bottom;
                    break;
            }
            switch (sf.Alignment)
            {
                case StringAlignment.Center:
                    flags |= TextFormatFlags.HorizontalCenter;
                    break;
                case StringAlignment.Near:
                    flags |= TextFormatFlags.Left;
                    break;
                case StringAlignment.Far:
                default:
                    flags |= TextFormatFlags.Right;
                    break;
            }
            if (sf.Trimming.HasFlag(StringTrimming.EllipsisCharacter)) flags |= TextFormatFlags.EndEllipsis;
            if (sf.FormatFlags.HasFlag(StringFormatFlags.NoWrap)) flags |= TextFormatFlags.SingleLine;
            return flags;
        }

        /// <summary>
        /// 文本布局（不换行）
        /// </summary>
        /// <param name="tb">垂直（上下）</param>
        /// <param name="lr">水平（前后）</param>
        public static TextFormatFlags TF_NoWrap(StringAlignment tb = StringAlignment.Center, StringAlignment lr = StringAlignment.Center) => TF(tb, lr) | TextFormatFlags.SingleLine;

        /// <summary>
        /// 文本布局（超出省略号）
        /// </summary>
        /// <param name="tb">垂直（上下）</param>
        /// <param name="lr">水平（前后）</param>
        public static TextFormatFlags TF_Ellipsis(StringAlignment tb = StringAlignment.Center, StringAlignment lr = StringAlignment.Center) => TF(tb, lr) | TextFormatFlags.EndEllipsis;

        /// <summary>
        /// 文本布局（超出省略号+不换行）
        /// </summary>
        /// <param name="tb">垂直（上下）</param>
        /// <param name="lr">水平（前后）</param>
        public static TextFormatFlags TF_ALL(StringAlignment tb = StringAlignment.Center, StringAlignment lr = StringAlignment.Center) => TF(tb, lr) | TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis;

        public static TextFormatFlags CreateTextFormatFlags(this ContentAlignment alignment, bool showEllipsis, bool multiLine)
        {
            TextFormatFlags flags = ConvertAlignmentToTextFormat(alignment);
            if (multiLine) flags |= TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl;
            else flags |= TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl | TextFormatFlags.SingleLine;
            if (showEllipsis) flags |= TextFormatFlags.EndEllipsis;
            //if (control.RightToLeft == RightToLeft.Yes) flags |= TextFormatFlags.RightToLeft;
            return flags;
        }

        const ContentAlignment AnyRight = ContentAlignment.TopRight | ContentAlignment.MiddleRight | ContentAlignment.BottomRight;
        const ContentAlignment AnyBottom = ContentAlignment.BottomLeft | ContentAlignment.BottomCenter | ContentAlignment.BottomRight;
        const ContentAlignment AnyCenter = ContentAlignment.TopCenter | ContentAlignment.MiddleCenter | ContentAlignment.BottomCenter;
        const ContentAlignment AnyMiddle = ContentAlignment.MiddleLeft | ContentAlignment.MiddleCenter | ContentAlignment.MiddleRight;
        public static TextFormatFlags ConvertAlignmentToTextFormat(this ContentAlignment alignment)
        {
            TextFormatFlags flags = TextFormatFlags.Top | TextFormatFlags.Left;
            if ((alignment & AnyBottom) != 0) flags |= TextFormatFlags.Bottom;
            else if ((alignment & AnyMiddle) != 0) flags |= TextFormatFlags.VerticalCenter;

            if ((alignment & AnyRight) != 0) flags |= TextFormatFlags.Right;
            else if ((alignment & AnyCenter) != 0) flags |= TextFormatFlags.HorizontalCenter;
            return flags;
        }


        static Dictionary<string, StringFormat> ffs = new Dictionary<string, StringFormat>();
        /// <summary>
        /// 文本布局
        /// </summary>
        /// <param name="measure">是否测量</param>
        public static StringFormat TF(FormatFlags flags, bool measure = false)
        {
            var key = (int)flags + "_" + measure;
            if (ffs.TryGetValue(key, out var r)) return r;
            StringFormat sf = new StringFormat(StringFormat.GenericTypographic);
#if NET40 || NET46 || NET48
            ffs.Add(key, sf);
#else
            ffs.TryAdd(key, sf);
#endif
            // 处理垂直对齐（LineAlignment）
            if (flags.HasFlag(FormatFlags.Top)) sf.LineAlignment = StringAlignment.Near;
            else if (flags.HasFlag(FormatFlags.VerticalCenter)) sf.LineAlignment = StringAlignment.Center;
            else if (flags.HasFlag(FormatFlags.Bottom)) sf.LineAlignment = StringAlignment.Far;

            // 处理水平对齐（Alignment）
            if (flags.HasFlag(FormatFlags.Left)) sf.Alignment = StringAlignment.Near;
            else if (flags.HasFlag(FormatFlags.HorizontalCenter)) sf.Alignment = StringAlignment.Center;
            else if (flags.HasFlag(FormatFlags.Right)) sf.Alignment = StringAlignment.Far;

            // 处理文本截断方式
            if (flags.HasFlag(FormatFlags.EllipsisCharacter)) sf.Trimming = StringTrimming.EllipsisCharacter;

            // 处理换行设置
            if (flags.HasFlag(FormatFlags.NoWrap)) sf.FormatFlags |= StringFormatFlags.NoWrap;

            if (!measure) sf.FormatFlags &= ~StringFormatFlags.MeasureTrailingSpaces;

            return sf;
        }

        #endregion

        /// <summary>
        /// 画刷（渐变色）
        /// </summary>
        /// <param name="code">渐变代码H5</param>
        /// <param name="rect">区域</param>
        /// <param name="def">默认颜色</param>
        public static Brush BrushEx(this string? code, Rectangle rect, Color def)
        {
            if (code != null)
            {
                var arr = BrushEx(code);
                if (arr.Length > 1) return BrushEx(code, arr, rect);
            }
            return new SolidBrush(def);
        }

        /// <summary>
        /// 画刷（渐变色）
        /// </summary>
        public static bool BrushEx(this string? code, Rectangle rect, Canvas g)
        {
            if (code != null)
            {
                var arr = BrushEx(code);
                if (arr.Length > 1)
                {
                    using (var brush = BrushEx(code, arr, rect))
                    {
                        g.Fill(brush, rect);
                    }
                    return true;
                }
            }
            return false;
        }

        static string[] BrushEx(string code)
        {
            var arr = code.Split(new string[] { " , ", ", ", "," }, StringSplitOptions.RemoveEmptyEntries);
            if (arr.Length > 1)
            {
                if (code.Contains("rgb") && code.Contains("("))
                {
                    List<string> list = new List<string>(arr.Length), tmp = new List<string>(arr.Length);
                    foreach (var it in arr)
                    {
                        if (it.StartsWith("rgb") && it.Contains("(")) tmp.Add(it);
                        else if (tmp.Count > 1 && it.Contains(")"))
                        {
                            tmp.Add(it);
                            list.Add(string.Join(",", tmp));
                            tmp.Clear();
                        }
                        else if (tmp.Count > 0) tmp.Add(it);
                        else list.Add(it);
                    }
                    return list.ToArray();
                }
                return arr;
            }
            return new string[0];
        }

        static LinearGradientBrush BrushEx(string code, string[] cs, Rectangle rect)
        {
            if (cs.Length > 2 && float.TryParse(cs[0], out float deg)) return BrushEx(rect, deg, cs, code.Contains("%"), 1);
            else if (cs.Length > 2 && cs[0].EndsWith("deg") && float.TryParse(cs[0].Substring(0, cs[0].Length - 3), out float deg2)) return BrushEx(rect, deg2, cs, code.Contains("%"), 1);
            else return BrushEx(rect, 0, cs, code.Contains("%"));
        }

        static LinearGradientBrush BrushEx(Rectangle rect, float deg, string[] cs, bool _in, int start = 0)
        {
            if (cs.Length > (2 + start))
            {
                int len = cs.Length - start;
                var colors = new List<Color>(len);
                var positions = new List<float>(len);
                for (int i = start; i < cs.Length; i++)
                {
                    var arr2 = cs[i].Split(' ');
                    colors.Add(arr2[0].ToColor());
                    if (arr2.Length > 1 && float.TryParse(arr2[1].TrimEnd('%'), out var result)) positions.Add(result / 100F);
                    else if (i == start) positions.Add(0F);
                    else if (i == cs.Length - 1) positions.Add(1F);
                }
                if (positions.Count != colors.Count)
                {
                    positions.Clear();
                    var tmp = 100F / colors.Count / 100F;
                    positions.Add(0F);
                    var use = tmp;
                    for (int i = 1; i < colors.Count - 1; i++)
                    {
                        positions.Add(use);
                        use += tmp;
                    }
                    positions.Add(1F);
                }
                return new LinearGradientBrush(rect, Color.Transparent, Color.Transparent, 270 + deg)
                {
                    InterpolationColors = new ColorBlend(colors.Count)
                    {
                        Colors = colors.ToArray(),
                        Positions = positions.ToArray()
                    }
                };
            }
            else if (_in)
            {
                int len = cs.Length - start;
                var colors = new List<Color>(len);
                float position = -1;
                for (int i = start; i < cs.Length; i++)
                {
                    var arr2 = cs[i].Split(' ');
                    colors.Add(arr2[0].ToColor());
                    if (arr2.Length > 1 && float.TryParse(arr2[1].TrimEnd('%'), out var result)) position = result / 100F;
                }
                if (position > -1)
                {
                    colors.Add(colors[colors.Count - 1]);
                    return new LinearGradientBrush(rect, colors[0], colors[colors.Count - 1], 270 + deg)
                    {
                        InterpolationColors = new ColorBlend(colors.Count)
                        {
                            Colors = colors.ToArray(),
                            Positions = new float[] { 0, position, 1 }
                        }
                    };
                }
                return new LinearGradientBrush(rect, colors[0], colors[colors.Count - 1], 270 + deg);
            }
            else return new LinearGradientBrush(rect, cs[start].Trim().ToColor(), cs[start + 1].Trim().ToColor(), 270 + deg);
        }

        public static Canvas High(this Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            if (Config.TextRenderingHint.HasValue) g.TextRenderingHint = Config.TextRenderingHint.Value;
            return new Core.CanvasGDI(g);
        }

        public static Canvas HighLay(this Graphics g, bool text = false)
        {
            Config.SetDpi(g);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            if (text) g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            return new Core.CanvasGDI(g);
        }

        public static void GDI(Action<Canvas> action)
        {
            using (var bmp = new Bitmap(1, 1))
            {
                using (var g = Graphics.FromImage(bmp))
                {
                    action(g.HighLay());
                }
            }
        }

        public static T GDI<T>(Func<Canvas, T> action)
        {
            using (var bmp = new Bitmap(1, 1))
            {
                using (var g = Graphics.FromImage(bmp))
                {
                    return action(g.HighLay());
                }
            }
        }

        public static SolidBrush Brush(this Color? color, Color default_color)
        {
            if (color.HasValue) return new SolidBrush(color.Value);
            return new SolidBrush(default_color);
        }

        public static SolidBrush Brush(this Color? color, Color default_color, Color enabled_color, bool enabled)
        {
            if (enabled)
            {
                if (color.HasValue) return new SolidBrush(color.Value);
                return new SolidBrush(default_color);
            }
            else return new SolidBrush(enabled_color);
        }

        #region 圆角

        public static GraphicsPath RoundPath(this Rectangle rect, float radius) => RoundPathCore(rect, radius);

        public static GraphicsPath RoundPath(this RectangleF rect, float radius) => RoundPathCore(rect, radius);

        public static GraphicsPath RoundPath(this RectangleF rect, float radius, TShape shape) => RoundPath(rect, radius, shape == TShape.Round);

        public static GraphicsPath RoundPath(this Rectangle rect, float radius, bool round)
        {
            if (round) return CapsulePathCore(rect);
            return RoundPathCore(rect, radius);
        }

        public static GraphicsPath RoundPath(this RectangleF rect, float radius, bool round)
        {
            if (round) return CapsulePathCore(rect);
            return RoundPathCore(rect, radius);
        }

        public static GraphicsPath RoundPath(this Rectangle rect, float radius, TAlignMini shadowAlign, TAlignRound align)
        {
            if (align == TAlignRound.ALL)
            {
                switch (shadowAlign)
                {
                    case TAlignMini.Top: return RoundPath(rect, radius, true, true, false, false);
                    case TAlignMini.Bottom: return RoundPath(rect, radius, false, false, true, true);
                    case TAlignMini.Left: return RoundPath(rect, radius, true, false, false, true);
                    case TAlignMini.Right: return RoundPath(rect, radius, false, true, true, false);
                    case TAlignMini.None:
                    default: return RoundPath(rect, radius, align);
                }
            }
            return RoundPath(rect, radius, align);
        }

        /// <summary>
        /// 自定义圆角
        /// </summary>
        /// <param name="rect">区域</param>
        /// <param name="radius">圆角大小</param>
        /// <param name="align">圆角方向</param>
        public static GraphicsPath RoundPath(this Rectangle rect, float radius, TAlignRound align)
        {
            switch (align)
            {
                case TAlignRound.Top: return RoundPath(rect, radius, true, true, false, false);
                case TAlignRound.Bottom: return RoundPath(rect, radius, false, false, true, true);
                case TAlignRound.Left: return RoundPath(rect, radius, true, false, false, true);
                case TAlignRound.Right: return RoundPath(rect, radius, false, true, true, false);
                case TAlignRound.TL: return RoundPath(rect, radius, true, false, false, false);
                case TAlignRound.TR: return RoundPath(rect, radius, false, true, false, false);
                case TAlignRound.BR: return RoundPath(rect, radius, false, false, true, false);
                case TAlignRound.BL: return RoundPath(rect, radius, false, false, false, true);
                case TAlignRound.ALL:
                default: return RoundPathCore(rect, radius);
            }
        }

        /// <summary>
        /// 自定义圆角
        /// </summary>
        /// <param name="rect">区域</param>
        /// <param name="radius">圆角大小</param>
        /// <param name="TL">↖</param>
        /// <param name="TR">↗</param>
        /// <param name="BR">↘</param>
        /// <param name="BL">↙</param>
        public static GraphicsPath RoundPath(this Rectangle rect, float radius, bool TL, bool TR, bool BR, bool BL)
        {
            var path = new GraphicsPath();
            if (radius <= 0F) path.AddRectangle(rect);
            else
            {
                float diameter = radius * 2F;
                var arc = new RectangleF(rect.X, rect.Y, diameter, diameter);

                // TL
                if (TL) path.AddArc(arc, 180, 90);
                else path.AddLine(rect.X, rect.Y, rect.Right - diameter, rect.Y);

                // TR
                arc.X = rect.Right - diameter;
                if (TR) path.AddArc(arc, 270, 90);
                else path.AddLine(rect.Right, rect.Y, rect.Right, rect.Bottom - diameter);

                // BR
                arc.Y = rect.Bottom - diameter;
                if (BR) path.AddArc(arc, 0, 90);
                else path.AddLine(rect.Right, rect.Bottom, rect.X + diameter, rect.Bottom);

                // BL
                arc.X = rect.Left;
                if (BL) path.AddArc(arc, 90, 90);
                else path.AddLine(rect.X, rect.Bottom, rect.X, rect.Y + diameter);

                path.CloseFigure();
            }
            return path;
        }

        /// <summary>
        /// 自定义圆角
        /// </summary>
        /// <param name="rect">区域</param>
        /// <param name="radius">圆角大小</param>
        /// <param name="TL">↖</param>
        /// <param name="TR">↗</param>
        /// <param name="BR">↘</param>
        /// <param name="BL">↙</param>
        public static GraphicsPath RoundPath(this RectangleF rect, float radius, bool TL, bool TR, bool BR, bool BL)
        {
            var path = new GraphicsPath();
            if (radius > 0)
            {
                radius = Math.Min(radius, Math.Min(rect.Width, rect.Height) / 2);

                float diameter = radius * 2F;
                var arc = new RectangleF(rect.X, rect.Y, diameter, diameter);

                // TL
                if (TL) path.AddArc(arc, 180, 90);
                else path.AddLine(rect.X, rect.Y, rect.Right - diameter, rect.Y);

                // TR
                arc.X = rect.Right - diameter;
                if (TR) path.AddArc(arc, 270, 90);
                else path.AddLine(rect.Right, rect.Y, rect.Right, rect.Bottom - diameter);

                // BR
                arc.Y = rect.Bottom - diameter;
                if (BR) path.AddArc(arc, 0, 90);
                else path.AddLine(rect.Right, rect.Bottom, rect.X + diameter, rect.Bottom);

                // BL
                arc.X = rect.Left;
                if (BL) path.AddArc(arc, 90, 90);
                else path.AddLine(rect.X, rect.Bottom, rect.X, rect.Y + diameter);

                path.CloseFigure();
            }
            else path.AddRectangle(rect);
            return path;
        }

        static GraphicsPath RoundPathCore(RectangleF rect, float radius)
        {
            var path = new GraphicsPath();
            if (radius > 0F)
            {
                if (radius >= (Math.Min(rect.Width, rect.Height) / 2F)) AddCapsule(path, rect);
                else
                {
                    float diameter = radius * 2F;
                    var arc = new RectangleF(rect.X, rect.Y, diameter, diameter);

                    // TL
                    path.AddArc(arc, 180, 90);

                    // TR
                    arc.X = rect.Right - diameter;
                    path.AddArc(arc, 270, 90);

                    // BR
                    arc.Y = rect.Bottom - diameter;
                    path.AddArc(arc, 0, 90);

                    // BL
                    arc.X = rect.Left;
                    path.AddArc(arc, 90, 90);

                    path.CloseFigure();
                }
            }
            else path.AddRectangle(rect);
            return path;
        }
        static GraphicsPath CapsulePathCore(RectangleF rect)
        {
            var path = new GraphicsPath();
            AddCapsule(path, rect);
            return path;
        }
        static void AddCapsule(GraphicsPath path, RectangleF rect)
        {
            float diameter;
            RectangleF arc;
            if (rect.Width > 0 && rect.Height > 0)
            {
                if (rect.Width > rect.Height)
                {
                    // Horizontal capsule
                    diameter = rect.Height;
                    SizeF sizeF = new SizeF(diameter, diameter);
                    arc = new RectangleF(rect.Location, sizeF);
                    path.AddArc(arc, 90, 180);
                    arc.X = rect.Right - diameter;
                    path.AddArc(arc, 270, 180);
                }
                else if (rect.Width < rect.Height)
                {
                    // Vertical capsule
                    diameter = rect.Width;
                    SizeF sizeF = new SizeF(diameter, diameter);
                    arc = new RectangleF(rect.Location, sizeF);
                    path.AddArc(arc, 180, 180);
                    arc.Y = rect.Bottom - diameter;
                    path.AddArc(arc, 0, 180);
                }
                else path.AddEllipse(rect);// Circle
            }
            else path.AddEllipse(rect);
            path.CloseFigure();
        }

        #endregion

        #region 图标渲染

        internal static void PaintIcons(this Canvas g, IconInfo icon, Rectangle rect)
        {
            if (icon.Back.HasValue)
            {
                using (var brush = new SolidBrush(icon.Back.Value))
                {
                    int offset = icon.Offset * 2;
                    var rectreal = new Rectangle(rect.X + icon.Offset, rect.Y + icon.Offset, rect.Width - offset, rect.Height - offset);
                    if (icon.Round) g.FillEllipse(brush, rectreal);
                    else
                    {
                        using (var path = rectreal.RoundPath(icon.Radius))
                        {
                            g.Fill(brush, path);
                        }
                    }
                }
            }
            using (var bmp = SvgExtend.GetImgExtend(icon.Svg, rect, icon.Fill))
            {
                if (bmp == null) return;
                g.Image(bmp, rect);
            }
        }
        internal static void PaintIcons(this Canvas g, TType icon, Rectangle rect, string keyid, TAMode colorScheme)
        {
            switch (icon)
            {
                case TType.Success:
                    using (var bmp = SvgExtend.GetImgExtend(SvgDb.IcoSuccess, rect, Colour.Success.Get(keyid, colorScheme)))
                    {
                        if (bmp == null) return;
                        g.Image(bmp, rect);
                    }
                    break;
                case TType.Info:
                    using (var bmp = SvgExtend.GetImgExtend(SvgDb.IcoInfo, rect, Colour.Info.Get(keyid, colorScheme)))
                    {
                        if (bmp == null) return;
                        g.Image(bmp, rect);
                    }
                    break;
                case TType.Warn:
                    using (var bmp = SvgExtend.GetImgExtend(SvgDb.IcoWarn, rect, Colour.Warning.Get(keyid, colorScheme)))
                    {
                        if (bmp == null) return;
                        g.Image(bmp, rect);
                    }
                    break;
                case TType.Error:
                    using (var bmp = SvgExtend.GetImgExtend(SvgDb.IcoError, rect, Colour.Error.Get(keyid, colorScheme)))
                    {
                        if (bmp == null) return;
                        g.Image(bmp, rect);
                    }
                    break;
            }
        }
        internal static void PaintIcons(this Canvas g, TType icon, string? svg, Rectangle rect, Colour colour, string keyid, TAMode colorScheme)
        {
            if (svg == null)
            {
                using (var brush = new SolidBrush(Colour.BgBase.Get(keyid, colorScheme)))
                {
                    g.FillEllipse(brush, new Rectangle(rect.X + 1, rect.Y + 1, rect.Width - 2, rect.Height - 2));
                }
                switch (icon)
                {
                    case TType.Success:
                        using (var bmp = SvgExtend.GetImgExtend(SvgDb.IcoSuccess, rect, Colour.Success.Get(keyid, colorScheme)))
                        {
                            if (bmp == null) return;
                            g.Image(bmp, rect);
                        }
                        break;
                    case TType.Info:
                        using (var bmp = SvgExtend.GetImgExtend(SvgDb.IcoInfo, rect, Colour.Info.Get(keyid, colorScheme)))
                        {
                            if (bmp == null) return;
                            g.Image(bmp, rect);
                        }
                        break;
                    case TType.Warn:
                        using (var bmp = SvgExtend.GetImgExtend(SvgDb.IcoWarn, rect, Colour.Warning.Get(keyid, colorScheme)))
                        {
                            if (bmp == null) return;
                            g.Image(bmp, rect);
                        }
                        break;
                    case TType.Error:
                        using (var bmp = SvgExtend.GetImgExtend(SvgDb.IcoError, rect, Colour.Error.Get(keyid, colorScheme)))
                        {
                            if (bmp == null) return;
                            g.Image(bmp, rect);
                        }
                        break;
                }
            }
            else
            {
                switch (icon)
                {
                    case TType.Success:
                        using (var bmp = SvgExtend.GetImgExtend(svg, rect, Colour.Success.Get(keyid, colorScheme)))
                        {
                            if (bmp == null) return;
                            g.Image(bmp, rect);
                        }
                        break;
                    case TType.Info:
                        using (var bmp = SvgExtend.GetImgExtend(svg, rect, Colour.Info.Get(keyid, colorScheme)))
                        {
                            if (bmp == null) return;
                            g.Image(bmp, rect);
                        }
                        break;
                    case TType.Warn:
                        using (var bmp = SvgExtend.GetImgExtend(svg, rect, Colour.Warning.Get(keyid, colorScheme)))
                        {
                            if (bmp == null) return;
                            g.Image(bmp, rect);
                        }
                        break;
                    case TType.Error:
                        using (var bmp = SvgExtend.GetImgExtend(svg, rect, Colour.Error.Get(keyid, colorScheme)))
                        {
                            if (bmp == null) return;
                            g.Image(bmp, rect);
                        }
                        break;
                    default:
                        using (var bmp = SvgExtend.GetImgExtend(svg, rect))
                        {
                            if (bmp == null) return;
                            g.Image(bmp, rect);
                        }
                        break;
                }
            }
        }
        internal static void PaintIconGhosts(this Canvas g, TType icon, Rectangle rect, Color color)
        {
            switch (icon)
            {
                case TType.Success:
                    using (var bmp = SvgExtend.GetImgExtend(SvgDb.IcoSuccessGhost, rect, color))
                    {
                        if (bmp == null) return;
                        g.Image(bmp, rect);
                    }
                    break;
                case TType.Info:
                    using (var bmp = SvgExtend.GetImgExtend(SvgDb.IcoInfoGhost, rect, color))
                    {
                        if (bmp == null) return;
                        g.Image(bmp, rect);
                    }
                    break;
                case TType.Warn:
                    using (var bmp = SvgExtend.GetImgExtend(SvgDb.IcoWarnGhost, rect, color))
                    {
                        if (bmp == null) return;
                        g.Image(bmp, rect);
                    }
                    break;
                case TType.Error:
                    using (var bmp = SvgExtend.GetImgExtend(SvgDb.IcoErrorGhost, rect, color))
                    {
                        if (bmp == null) return;
                        g.Image(bmp, rect);
                    }
                    break;
            }
        }
        internal static void PaintIconClose(this Canvas g, Rectangle rect, Color color)
        {
            PaintIconCore(g, rect, SvgDb.IcoErrorGhost, color);
        }
        internal static void PaintIconClose(this Canvas g, Rectangle rect, Color color, float dot)
        {
            PaintIconCore(g, rect, SvgDb.IcoErrorGhost, color, dot);
        }
        internal static void PaintIconReset(this Canvas g, Rectangle rect, Color color, float dot)
        {
            PaintIconCore(g, rect, SvgDb.IcoStar, color, dot);
        }
        /// <summary>
        /// 绘制带圆背景的镂空图标
        /// </summary>
        internal static void PaintIconCoreGhost(this Canvas g, Rectangle rect, string svg, Color back, Color fore)
        {
            using (var brush = new SolidBrush(back))
            {
                g.FillEllipse(brush, rect);
            }
            g.GetImgExtend(svg, rect, fore);
        }
        internal static void PaintIconCore(this Canvas g, Rectangle rect, string svg, Color back, Color fore)
        {
            using (var brush = new SolidBrush(back))
            {
                g.FillEllipse(brush, new Rectangle(rect.X + 1, rect.Y + 1, rect.Width - 2, rect.Height - 2));
            }
            g.GetImgExtend(svg, rect, fore);
        }
        internal static void PaintIconCore(this Canvas g, Rectangle rect, string svg, Color color) => g.GetImgExtend(svg, rect, color);
        internal static void PaintIconCore(this Canvas g, Rectangle rect, string svg, Color color, float dot)
        {
            int size = (int)(rect.Height * dot);
            var rect_ico = new Rectangle(rect.X + (rect.Width - size) / 2, rect.Y + (rect.Height - size) / 2, size, size);
            g.GetImgExtend(svg, rect_ico, color);
        }

        #endregion

        #region 阴影/徽标

        #region 徽标

        public static void PaintBadge(this IControl control, Canvas g) => PaintBadge(control, control.Font, control.ClientRectangle, g, control.ColorScheme);
        public static void PaintBadge(this BadgeConfig badegConfig, Font Font, Rectangle Rect, Canvas g, TAMode colorScheme)
        {
            if (badegConfig.BadgeSvg != null)
            {
                int hasx = (int)(badegConfig.BadgeOffsetX * Config.Dpi), hasy = (int)(badegConfig.BadgeOffsetY * Config.Dpi);
                using (var font = new Font(Font.FontFamily, Font.Size * badegConfig.BadgeSize))
                {
                    var size_badge = g.MeasureString(Config.NullText, font).Height;
                    var rect_badge = PaintBadge(Rect, badegConfig.BadgeAlign, hasx, hasy, size_badge, size_badge);
                    g.GetImgExtend(badegConfig.BadgeSvg, rect_badge, badegConfig.BadgeBack ?? Colour.Error.Get(nameof(Badge), colorScheme));
                }
            }
            else if (badegConfig.Badge != null)
            {
                var color = badegConfig.BadgeBack ?? Colour.Error.Get(nameof(Badge), colorScheme);
                var rect = Rect;
                float borsize = Config.Dpi;
                using (var font = new Font(Font.FontFamily, Font.Size * badegConfig.BadgeSize))
                {
                    int hasx = (int)(badegConfig.BadgeOffsetX * Config.Dpi), hasy = (int)(badegConfig.BadgeOffsetY * Config.Dpi);
                    if (string.IsNullOrWhiteSpace(badegConfig.Badge))
                    {
                        var size = g.MeasureString(Config.NullText, font).Height / 2;
                        var rect_badge = PaintBadge(rect, badegConfig.BadgeAlign, hasx, hasy, size, size);
                        using (var brush = new SolidBrush(color))
                        {
                            if (badegConfig.BadgeMode)
                            {
                                float b2 = borsize * 2, rr = size * 0.2F, rr2 = rr * 2;
                                g.FillEllipse(Colour.ErrorColor.Get(nameof(Badge), colorScheme), new RectangleF(rect_badge.X - borsize, rect_badge.Y - borsize, rect_badge.Width + b2, rect_badge.Height + b2));
                                using (var path = rect_badge.RoundPath(1, true))
                                {
                                    path.AddEllipse(new RectangleF(rect_badge.X + rr, rect_badge.Y + rr, rect_badge.Width - rr2, rect_badge.Height - rr2));
                                    g.Fill(color, path);
                                }
                            }
                            else
                            {
                                g.FillEllipse(color, rect_badge);
                                g.DrawEllipse(Colour.ErrorColor.Get(nameof(Badge), colorScheme), borsize, rect_badge);
                            }
                        }
                    }
                    else
                    {
                        var size = g.MeasureString(badegConfig.Badge, font).Size(4, 2);
                        using (var s_f = SF_NoWrap())
                        {
                            int size_badge = (int)(size.Height * 1.2F);
                            if (size.Height > size.Width)
                            {
                                var rect_badge = PaintBadge(rect, badegConfig.BadgeAlign, hasx, hasy, size_badge, size_badge);
                                g.FillEllipse(color, rect_badge);
                                g.DrawEllipse(Colour.ErrorColor.Get(nameof(Badge), colorScheme), borsize, rect_badge);
                                g.String(badegConfig.Badge, font, Colour.ErrorColor.Get(nameof(Badge), colorScheme), rect_badge, s_f);
                            }
                            else
                            {
                                int w_badge = size.Width + (size_badge - size.Height);
                                var rect_badge = PaintBadge(rect, badegConfig.BadgeAlign, hasx, hasy, w_badge, size_badge);
                                using (var path = rect_badge.RoundPath(rect_badge.Height))
                                {
                                    g.Fill(color, path);
                                    g.Draw(Colour.ErrorColor.Get(nameof(Badge), colorScheme), borsize, path);
                                }
                                g.String(badegConfig.Badge, font, Colour.ErrorColor.Get(nameof(Badge), colorScheme), rect_badge, s_f);
                            }
                        }
                    }
                }
            }
        }

        public static void PaintBadge(this IControl control, DateBadge badge, Rectangle rect, Canvas g)
        {
            var color = badge.Fill ?? control.BadgeBack ?? Colour.Error.Get(nameof(Badge), control.ColorScheme);
            float borsize = Config.Dpi;
            using (var font = new Font(control.Font.FontFamily, control.Font.Size * badge.Size))
            {
                int hasx = (int)(badge.OffsetX * Config.Dpi), hasy = (int)(badge.OffsetY * Config.Dpi);
                if (string.IsNullOrWhiteSpace(badge.Content))
                {
                    var size_badge = g.MeasureString(Config.NullText, font).Height / 2;
                    var rect_badge = PaintBadge(rect, badge.Align, hasx, hasy, size_badge, size_badge);
                    g.FillEllipse(color, rect_badge);
                    g.DrawEllipse(Colour.ErrorColor.Get(nameof(Badge), control.ColorScheme), borsize, rect_badge);
                }
                else
                {
                    var size = g.MeasureString(badge.Content, font).Size(4, 2);
                    using (var s_f = SF_NoWrap())
                    {
                        int size_badge = (int)(size.Height * 1.2F);
                        if (size.Height > size.Width)
                        {
                            var rect_badge = PaintBadge(rect, badge.Align, hasx, hasy, size_badge, size_badge);
                            g.FillEllipse(color, rect_badge);
                            g.DrawEllipse(Colour.ErrorColor.Get(nameof(Badge), control.ColorScheme), borsize, rect_badge);
                            g.String(badge.Content, font, Colour.ErrorColor.Get(nameof(Badge), control.ColorScheme), rect_badge, s_f);
                        }
                        else
                        {
                            int w_badge = size.Width + (size_badge - size.Height);
                            var rect_badge = PaintBadge(rect, badge.Align, hasx, hasy, w_badge, size_badge);
                            using (var path = rect_badge.RoundPath(rect_badge.Height))
                            {
                                g.Fill(color, path);
                                g.Draw(Colour.ErrorColor.Get(nameof(Badge), control.ColorScheme), borsize, path);
                            }
                            g.String(badge.Content, font, Colour.ErrorColor.Get(nameof(Badge), control.ColorScheme), rect_badge, s_f);
                        }
                    }
                }
            }
        }

        static Rectangle PaintBadge(Rectangle rect, TAlign align, int x, int y, int w, int h)
        {
            switch (align)
            {
                case TAlign.TL:
                case TAlign.LT: return new Rectangle(rect.X + x, rect.Y + y, w, h);
                case TAlign.BL:
                case TAlign.LB: return new Rectangle(rect.X + x, rect.Bottom - y - h, w, h);
                case TAlign.BR:
                case TAlign.RB: return new Rectangle(rect.Right - x - w, rect.Bottom - y - h, w, h);
                case TAlign.Top: return new Rectangle(rect.X + (rect.Width - w) / 2, rect.Y + y, w, h);
                case TAlign.Bottom: return new Rectangle(rect.X + (rect.Width - w) / 2, rect.Bottom - y - h, w, h);
                case TAlign.TR:
                case TAlign.RT: return new Rectangle(rect.Right - x - w, rect.Y + y, w, h);
                case TAlign.Left: return new Rectangle(rect.X + x, rect.Y + (rect.Height - h) / 2, w, h);
                case TAlign.Right: return new Rectangle(rect.Right - x - w, rect.Y + (rect.Height - h) / 2, w, h);
                default:
                    return new Rectangle(rect.X + (rect.Width - w) / 2, rect.Y + (rect.Height - h) / 2, w, h);
            }
        }

        #endregion

        public static void PaintShadow(this Canvas g, ShadowConfig config, Rectangle _rect, Rectangle rect, float radius, bool round)
        {
            int shadow = (int)(config.Shadow * Config.Dpi), shadowOffsetX = (int)(config.ShadowOffsetX * Config.Dpi), shadowOffsetY = (int)(config.ShadowOffsetY * Config.Dpi);
            using (var bmp_shadow = new Bitmap(_rect.Width, _rect.Height))
            {
                using (var g_shadow = Graphics.FromImage(bmp_shadow))
                {
                    using (var path = RoundPath(rect, radius, round))
                    {
                        using (var brush = config.ShadowColor.Brush(Colour.TextBase.Get()))
                        {
                            g_shadow.FillPath(brush, path);
                        }
                    }
                    Blur(bmp_shadow, shadow);
                }
                using (var attributes = new ImageAttributes())
                {
                    var matrix = new ColorMatrix
                    {
                        Matrix33 = config.ShadowOpacity
                    };
                    attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                    g.Image(bmp_shadow, new Rectangle(_rect.X + shadowOffsetX, _rect.Y + shadowOffsetY, _rect.Width, _rect.Height), 0, 0, _rect.Width, _rect.Height, GraphicsUnit.Pixel, attributes);
                }
            }
        }

        #endregion

        public static void PaintEmpty(this Canvas g, Rectangle rect, Font font, Color fore, string? text = null, Image? image = null, int offset = 0)
        {
            PaintEmpty(g, rect, font, fore, text, image, offset, FormatFlags.Center | FormatFlags.NoWrap);
        }

        [Obsolete("use FormatFlags")]
        public static void PaintEmpty(this Canvas g, Rectangle rect, Font font, Color fore, string? text, Image? image, int offset, StringFormat sf)
        {
            using (var brush = new SolidBrush(fore))
            {
                if (offset > 0)
                {
                    rect.Offset(0, offset);
                    rect.Height -= offset;
                }
                string emptytext = text ?? Localization.Get("NoData", "暂无数据");
                var bmp = image ?? Config.EmptyImage;
                if (bmp != null)
                {
                    int gap = (int)(8 * Config.Dpi);
                    var size = g.MeasureString(emptytext, font);
                    Rectangle rect_img = new Rectangle(rect.X + (rect.Width - bmp.Width) / 2, rect.Y + (rect.Height - bmp.Height) / 2 - size.Height, bmp.Width, bmp.Height), rect_font = new Rectangle(rect.X, rect_img.Bottom + gap, rect.Width, size.Height);
                    g.Image(bmp, rect_img);
                    g.String(emptytext, font, brush, rect_font, sf);
                }
                else if (Config.EmptyImageSvg != null)
                {
                    var size = g.MeasureString(emptytext, font);
                    int gap = (int)(8 * Config.Dpi), icon_size = (int)(size.Height * Config.EmptyImageRatio);

                    Rectangle rect_img = new Rectangle(rect.X + (rect.Width - icon_size) / 2, rect.Y + (rect.Height - icon_size) / 2 - size.Height, icon_size, icon_size),
                        rect_font = new Rectangle(rect.X, rect_img.Bottom + gap, rect.Width, size.Height);

                    using (var _bmp = SvgExtend.GetImgExtend(Config.IsDark ? Config.EmptyImageSvg[1] : Config.EmptyImageSvg[0], rect_img, fore))
                    {
                        if (_bmp == null)
                        {
                            g.String(emptytext, font, brush, rect, sf);
                            return;
                        }
                        else g.Image(_bmp, rect_img);
                    }
                    g.String(emptytext, font, brush, rect_font, sf);
                }
                else g.String(emptytext, font, brush, rect, sf);
            }
        }
        public static void PaintEmpty(this Canvas g, Rectangle rect, Font font, Color fore, string? text, Image? image, int offset, FormatFlags format)
        {
            using (var brush = new SolidBrush(fore))
            {
                if (offset > 0)
                {
                    rect.Offset(0, offset);
                    rect.Height -= offset;
                }
                string emptytext = text ?? Localization.Get("NoData", "暂无数据");
                var bmp = image ?? Config.EmptyImage;
                if (bmp != null)
                {
                    int gap = (int)(8 * Config.Dpi);
                    var size = g.MeasureString(emptytext, font);
                    Rectangle rect_img = new Rectangle(rect.X + (rect.Width - bmp.Width) / 2, rect.Y + (rect.Height - bmp.Height) / 2 - size.Height, bmp.Width, bmp.Height), rect_font = new Rectangle(rect.X, rect_img.Bottom + gap, rect.Width, size.Height);
                    g.Image(bmp, rect_img);
                    g.String(emptytext, font, brush, rect_font, format);
                }
                else if (Config.EmptyImageSvg != null)
                {
                    var size = g.MeasureString(emptytext, font);
                    int gap = (int)(8 * Config.Dpi), icon_size = (int)(size.Height * Config.EmptyImageRatio);

                    Rectangle rect_img = new Rectangle(rect.X + (rect.Width - icon_size) / 2, rect.Y + (rect.Height - icon_size) / 2 - size.Height, icon_size, icon_size),
                        rect_font = new Rectangle(rect.X, rect_img.Bottom + gap, rect.Width, size.Height);

                    using (var _bmp = SvgExtend.GetImgExtend(Config.IsDark ? Config.EmptyImageSvg[1] : Config.EmptyImageSvg[0], rect_img, fore))
                    {
                        if (_bmp == null)
                        {
                            g.String(emptytext, font, brush, rect, format);
                            return;
                        }
                        else g.Image(_bmp, rect_img);
                    }
                    g.String(emptytext, font, brush, rect_font, format);
                }
                else g.String(emptytext, font, brush, rect, format);
            }
        }

        #region 图像处理

        #region 模糊

        public static void Blur(Bitmap bmp, int range) => Blur(bmp, range, new Rectangle(0, 0, bmp.Width, bmp.Height));
        public static void Blur(Bitmap bmp, int range, Rectangle rect)
        {
            if (range > 1)
            {
                using (var unsafeBitmap = new UnsafeBitmap(bmp, true))
                {
                    int halfRange = range / 2;
                    BlurHorizontal(unsafeBitmap, halfRange, rect.X, rect.Y, rect.Right, rect.Bottom);
                    BlurVertical(unsafeBitmap, halfRange, rect.X, rect.Y, rect.Right, rect.Bottom);
                    BlurHorizontal(unsafeBitmap, halfRange, rect.X, rect.Y, rect.Right, rect.Bottom);
                    BlurVertical(unsafeBitmap, halfRange, rect.X, rect.Y, rect.Right, rect.Bottom);
                }
            }
        }

        static void BlurHorizontal(UnsafeBitmap unsafeBitmap, int halfRange, int left, int top, int right, int bottom)
        {
            ColorBgra[] newColors = new ColorBgra[unsafeBitmap.Width];

            for (int y = top; y < bottom; y++)
            {
                int hits = 0;
                int r = 0;
                int g = 0;
                int b = 0;
                int a = 0;

                for (int x = left - halfRange; x < right; x++)
                {
                    int oldPixel = x - halfRange - 1;
                    if (oldPixel >= left)
                    {
                        ColorBgra color = unsafeBitmap.GetPixel(oldPixel, y);

                        if (color.Bgra != 0)
                        {
                            r -= color.Red;
                            g -= color.Green;
                            b -= color.Blue;
                            a -= color.Alpha;
                        }

                        hits--;
                    }

                    int newPixel = x + halfRange;
                    if (newPixel < right)
                    {
                        ColorBgra color = unsafeBitmap.GetPixel(newPixel, y);

                        if (color.Bgra != 0)
                        {
                            r += color.Red;
                            g += color.Green;
                            b += color.Blue;
                            a += color.Alpha;
                        }

                        hits++;
                    }

                    if (x >= left) newColors[x] = new ColorBgra((byte)(b / hits), (byte)(g / hits), (byte)(r / hits), (byte)(a / hits));
                }

                for (int x = left; x < right; x++) unsafeBitmap.SetPixel(x, y, newColors[x]);
            }
        }

        static void BlurVertical(UnsafeBitmap unsafeBitmap, int halfRange, int left, int top, int right, int bottom)
        {
            ColorBgra[] newColors = new ColorBgra[unsafeBitmap.Height];

            for (int x = left; x < right; x++)
            {
                int hits = 0;
                int r = 0;
                int g = 0;
                int b = 0;
                int a = 0;

                for (int y = top - halfRange; y < bottom; y++)
                {
                    int oldPixel = y - halfRange - 1;
                    if (oldPixel >= top)
                    {
                        ColorBgra color = unsafeBitmap.GetPixel(x, oldPixel);

                        if (color.Bgra != 0)
                        {
                            r -= color.Red;
                            g -= color.Green;
                            b -= color.Blue;
                            a -= color.Alpha;
                        }

                        hits--;
                    }

                    int newPixel = y + halfRange;
                    if (newPixel < bottom)
                    {
                        ColorBgra color = unsafeBitmap.GetPixel(x, newPixel);

                        if (color.Bgra != 0)
                        {
                            r += color.Red;
                            g += color.Green;
                            b += color.Blue;
                            a += color.Alpha;
                        }

                        hits++;
                    }

                    if (y >= top) newColors[y] = new ColorBgra((byte)(b / hits), (byte)(g / hits), (byte)(r / hits), (byte)(a / hits));
                }

                for (int y = top; y < bottom; y++) unsafeBitmap.SetPixel(x, y, newColors[y]);
            }
        }

        #endregion

        #region 阴影

        public static SafeBitmap PaintShadow(this GraphicsPath path, int width, int height, int range = 10) => PaintShadow(path, width, height, Color.Black, range);
        public static SafeBitmap PaintShadow(this GraphicsPath path, int width, int height, Color color, int range = 10)
        {
            var bmp_shadow = new SafeBitmap(width, height);
            using (var g = bmp_shadow.Graphics)
            {
                using (var brush = new SolidBrush(color))
                {
                    g.FillPath(brush, path);
                }
                Blur(bmp_shadow.Bitmap, range);
            }
            return bmp_shadow;
        }

        public static Bitmap PaintShadowO(this GraphicsPath path, int width, int height, int range = 10) => PaintShadowO(path, width, height, Color.Black, range);
        public static Bitmap PaintShadowO(this GraphicsPath path, int width, int height, Color color, int range = 10)
        {
            var bmp_shadow = new Bitmap(width, height);
            using (var g = Graphics.FromImage(bmp_shadow))
            {
                using (var brush = new SolidBrush(color))
                {
                    g.FillPath(brush, path);
                }
                Blur(bmp_shadow, range);
            }
            return bmp_shadow;
        }

        #endregion

        #endregion
    }
}