// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Windows.Forms;

#if NET6_0_OR_GREATER
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
#endif

namespace AntdUI
{
    partial class Helper
    {
        #region 文本布局

        #region FormatFlags

        static ConcurrentDictionary<string, StringFormat> ffs = new ConcurrentDictionary<string, StringFormat>();
        /// <summary>
        /// 文本布局
        /// </summary>
        /// <param name="measure">是否测量</param>
        public static StringFormat TF(FormatFlags flags, bool measure = false)
        {
            var key = (int)flags + "_" + (measure ? "1" : "0");
            if (ffs.TryGetValue(key, out var r)) return r;
            var sf = new StringFormat(StringFormat.GenericTypographic);
            sf.FormatFlags &= ~StringFormatFlags.LineLimit;

            // 处理垂直对齐（LineAlignment）
            if (flags.HasFlag(FormatFlags.VerticalCenter)) sf.LineAlignment = StringAlignment.Center;
            else if (flags.HasFlag(FormatFlags.Top)) sf.LineAlignment = StringAlignment.Near;
            else if (flags.HasFlag(FormatFlags.Bottom)) sf.LineAlignment = StringAlignment.Far;

            // 处理水平对齐（Alignment）
            if (flags.HasFlag(FormatFlags.HorizontalCenter)) sf.Alignment = StringAlignment.Center;
            else if (flags.HasFlag(FormatFlags.Left)) sf.Alignment = StringAlignment.Near;
            else if (flags.HasFlag(FormatFlags.Right)) sf.Alignment = StringAlignment.Far;

            // 处理文本截断方式
            if (flags.HasFlag(FormatFlags.EllipsisCharacter)) sf.Trimming = StringTrimming.EllipsisCharacter;

            if (flags.HasFlag(FormatFlags.HotkeyPrefixShow)) sf.HotkeyPrefix |= System.Drawing.Text.HotkeyPrefix.Show;

            if (flags.HasFlag(FormatFlags.DirectionVertical)) sf.FormatFlags |= StringFormatFlags.DirectionVertical;

            if (flags.HasFlag(FormatFlags.DirectionRightToLeft)) sf.FormatFlags |= StringFormatFlags.DirectionRightToLeft;

            // 处理换行设置
            if (flags.HasFlag(FormatFlags.NoWrap)) sf.FormatFlags |= StringFormatFlags.NoWrap | StringFormatFlags.MeasureTrailingSpaces;
            else
            {
                if (measure) sf.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
                else sf.FormatFlags &= ~StringFormatFlags.MeasureTrailingSpaces;
            }

            if (!ffs.TryAdd(key, sf)) sf.Dispose();

            return sf;
        }

        /// <summary>
        /// 文本布局
        /// </summary>
        public static FormatFlags SF(TAlign align)
        {
            switch (align)
            {
                case TAlign.Left:
                    return FormatFlags.Left | FormatFlags.VerticalCenter;
                case TAlign.TL:
                case TAlign.LT:
                    return FormatFlags.Left | FormatFlags.Top;
                case TAlign.Top:
                    return FormatFlags.HorizontalCenter | FormatFlags.Top;
                case TAlign.TR:
                case TAlign.RT:
                    return FormatFlags.Right | FormatFlags.Top;
                case TAlign.Right:
                    return FormatFlags.Right | FormatFlags.VerticalCenter;
                case TAlign.BR:
                case TAlign.RB:
                    return FormatFlags.Right | FormatFlags.Bottom;
                case TAlign.Bottom:
                    return FormatFlags.HorizontalCenter | FormatFlags.Bottom;
                case TAlign.BL:
                case TAlign.LB:
                    return FormatFlags.Left | FormatFlags.Bottom;
                default: return FormatFlags.Center;
            }
        }

        #endregion

        #region StringFormat Obsolete

        [Obsolete("use FormatFlags enum")]
        public static readonly StringFormat m_sf = SF_MEASURE_FONT();

        /// <summary>
        /// 文本布局
        /// </summary>
        /// <param name="tb">垂直（上下）</param>
        /// <param name="lr">水平（前后）</param>
        [Obsolete("use FormatFlags enum")]
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
        [Obsolete("use FormatFlags enum")]
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
        [Obsolete("use FormatFlags enum")]
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
        [Obsolete("use FormatFlags enum")]
        public static StringFormat SF_ALL(StringAlignment tb = StringAlignment.Center, StringAlignment lr = StringAlignment.Center)
        {
            var sf = new StringFormat(StringFormat.GenericTypographic) { LineAlignment = tb, Alignment = lr, Trimming = StringTrimming.EllipsisCharacter };
            sf.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces | StringFormatFlags.NoWrap;
            return sf;
        }

        [Obsolete("use FormatFlags enum")]
        public static StringFormat SF_MEASURE_FONT()
        {
            var sf = new StringFormat(StringFormat.GenericTypographic) { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            sf.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
            return sf;
        }

        #endregion

        #region TextFormatFlags

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

        #endregion

        #endregion

        /// <summary>
        /// 画刷（渐变色）
        /// </summary>
        /// <param name="code">渐变代码H5</param>
        /// <param name="rect">区域</param>
        /// <param name="def">默认颜色</param>
        public static Brush BrushEx(this string? code, Rectangle rect, Color def, float defdeg = 0F)
        {
            if (code != null)
            {
                var arr = BrushEx(code);
                if (arr.Length > 1) return BrushEx(code, arr, rect, defdeg);
            }
            return new SolidBrush(def);
        }
        public static Brush BrushEx(this string? code, RectangleF rect, Color def, float defdeg = 0F)
        {
            if (code != null)
            {
                var arr = BrushEx(code);
                if (arr.Length > 1) return BrushEx(code, arr, new Rectangle((int)Math.Floor(rect.X), (int)Math.Floor(rect.Y), (int)Math.Ceiling(rect.Width), (int)Math.Ceiling(rect.Height)), defdeg);
            }
            return new SolidBrush(def);
        }

        /// <summary>
        /// 画刷（渐变色）
        /// </summary>
        /// <param name="code">渐变代码H5</param>
        /// <param name="rect">区域</param>
        /// <param name="defn">默认空颜色</param>
        /// <param name="def">默认颜色</param>
        public static Brush BrushEx(this string? code, Rectangle rect, Color? defn, Color def)
        {
            if (code != null)
            {
                var arr = BrushEx(code);
                if (arr.Length > 1) return BrushEx(code, arr, rect);
            }
            return new SolidBrush(defn ?? def);
        }

        /// <summary>
        /// 画刷（渐变色）
        /// </summary>
        /// <param name="code">渐变代码H5</param>
        /// <param name="rect">区域</param>
        public static LinearGradientBrush? BrushEx(this string? code, Rectangle rect)
        {
            if (code != null)
            {
                var arr = BrushEx(code);
                if (arr.Length > 1) return BrushEx(code, arr, rect);
            }
            return null;
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

        /// <summary>
        /// 解析渐变颜色代码，将其拆分为颜色和位置数组
        /// </summary>
        /// <param name="code">渐变颜色代码</param>
        /// <returns>拆分后的字符串数组</returns>
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

        /// <summary>
        /// 根据渐变代码和颜色数组创建线性渐变画刷
        /// </summary>
        /// <param name="code">渐变颜色代码</param>
        /// <param name="cs">颜色数组</param>
        /// <param name="rect">绘制区域</param>
        /// <returns>线性渐变画刷</returns>
        static LinearGradientBrush BrushEx(string code, string[] cs, Rectangle rect, float defdeg = 0F)
        {
            if (cs.Length > 2 && BrushExAngle(cs[0], rect, out float deg)) return BrushEx(rect, deg, cs, code.Contains("%"), 1);
            else return BrushEx(rect, defdeg, cs, code.Contains("%"));
        }

        /// <summary>
        /// 解析渐变方向，转换为 CSS 角度
        /// 支持 to top / to right / to bottom / to left 及四角组合（如 to left top、to top right）
        /// 支持 deg、grad、g、rad、turn 角度单位，以及纯数字（默认按 deg 处理）
        /// </summary>
        /// <param name="token">方向代码</param>
        /// <param name="rect">绘制区域（四角方向按 CSS 规则结合实际宽高计算角度）</param>
        /// <param name="deg">返回的 CSS 角度</param>
        static bool BrushExAngle(string token, Rectangle rect, out float deg)
        {
            deg = 0F;
            string val = token.Trim();
            if (val.Length == 0) return false;
            if (val.StartsWith("to"))
            {
                string? h = null, v = null;
                foreach (var part in val.Substring(2).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    string p = part.ToLowerInvariant();
                    if (p == "left" || p == "right") h = p;
                    else if (p == "top" || p == "bottom") v = p;
                    else return false;
                }
                if (h == null)
                {
                    if (v == "top") { deg = 0F; return true; }
                    else if (v == "bottom") { deg = 180F; return true; }
                    return false;
                }
                else if (v == null)
                {
                    deg = h == "right" ? 90F : 270F;
                    return true;
                }
                // 四角方向：按 CSS 规则，渐变线沿对角线，角度由区域宽高决定
                float corner = (rect.Width > 0 && rect.Height > 0) ? (float)(Math.Atan((double)rect.Width / rect.Height) * 180D / Math.PI) : 45F;
                if (h == "right") deg = v == "top" ? corner : 180F - corner;
                else deg = v == "bottom" ? 180F + corner : 360F - corner;
                return true;
            }
            float mul;
            if (val.EndsWith("deg")) { val = val.Substring(0, val.Length - 3); mul = 1F; }
            else if (val.EndsWith("grad")) { val = val.Substring(0, val.Length - 4); mul = 0.9F; }
            else if (val.EndsWith("g")) { val = val.Substring(0, val.Length - 1); mul = 0.9F; }
            else if (val.EndsWith("rad")) { val = val.Substring(0, val.Length - 3); mul = (float)(180D / Math.PI); }
            else if (val.EndsWith("turn")) { val = val.Substring(0, val.Length - 4); mul = 360F; }
            else mul = 1F;
            if (float.TryParse(val, out float num))
            {
                deg = num * mul;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 根据矩形区域、角度、颜色数组创建线性渐变画刷
        /// </summary>
        /// <param name="rect">绘制区域</param>
        /// <param name="deg">渐变角度</param>
        /// <param name="cs">颜色数组</param>
        /// <param name="_in">是否包含百分比位置</param>
        /// <param name="start">颜色数组的起始索引</param>
        /// <returns>线性渐变画刷</returns>
        static LinearGradientBrush BrushEx(Rectangle rect, float deg, string[] cs, bool _in, int start = 0)
        {
            BrushEx(cs, _in, out var colors, out var positions, start);
            return new LinearGradientBrush(rect, Color.Transparent, Color.Transparent, 270 + deg)
            {
                InterpolationColors = new ColorBlend(colors.Length)
                {
                    Colors = colors,
                    Positions = positions
                }
            };
        }

        static void BrushEx(string[] cs, bool _in, out Color[] colors, out float[] positions, int start = 0)
        {
            if (cs.Length > (2 + start))
            {
                int len = cs.Length - start;
                var Colors = new List<Color>(len);
                var Positions = new List<float>(len);
                for (int i = start; i < cs.Length; i++)
                {
                    var arr2 = cs[i].Split(' ');
                    Colors.Add(arr2[0].ToColor());
                    if (arr2.Length > 1 && float.TryParse(arr2[1].TrimEnd('%'), out var result)) Positions.Add(result / 100F);
                    else if (i == start) Positions.Add(0F);
                    else if (i == cs.Length - 1) Positions.Add(1F);
                }
                if (Positions.Count != Colors.Count)
                {
                    Positions.Clear();
                    var tmp = 100F / Colors.Count / 100F;
                    Positions.Add(0F);
                    var use = tmp;
                    for (int i = 1; i < Colors.Count - 1; i++)
                    {
                        Positions.Add(use);
                        use += tmp;
                    }
                    Positions.Add(1F);
                }
                colors = Colors.ToArray();
                positions = Positions.ToArray();
            }
            else if (_in)
            {
                int len = cs.Length - start;
                var Colors = new List<Color>(len);
                float position = -1;
                for (int i = start; i < cs.Length; i++)
                {
                    var arr2 = cs[i].Split(' ');
                    Colors.Add(arr2[0].ToColor());
                    if (arr2.Length > 1 && float.TryParse(arr2[1].TrimEnd('%'), out var result)) position = result / 100F;
                }
                if (position > -1)
                {
                    Colors.Add(Colors[Colors.Count - 1]);
                    colors = Colors.ToArray();
                    positions = new float[] { 0, position, 1 };
                }
                else
                {
                    colors = new Color[] { Colors[0], Colors[Colors.Count - 1] };
                    positions = new float[] { 0, 1 };
                }
            }
            else
            {
                colors = new Color[] { cs[start].Trim().ToColor(), cs[start + 1].Trim().ToColor() };
                positions = new float[] { 0, 1 };
            }
        }

        /// <summary>
        /// 画刷（渐变色）
        /// </summary>
        /// <param name="code">渐变代码H5</param>
#if NET40 || NET46 || NET48
        public static bool BrushEx(this string? code, out Color[]? colors, out float[]? positions)
#else
        public static bool BrushEx(this string? code, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out Color[]? colors, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out float[]? positions)
#endif
        {
            if (code != null)
            {
                var arr = BrushEx(code);
                if (arr.Length > 1)
                {
                    BrushEx(arr, code.Contains("%"), out colors, out positions);
                    return true;
                }
            }
            colors = null;
            positions = null;
            return false;
        }

        public static Canvas High(this Graphics g, float dpi)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            if (Config.TextRenderingHint.HasValue) g.TextRenderingHint = Config.TextRenderingHint.Value;
            return new Core.CanvasGDI(g, dpi);
        }

        public static Canvas High(this Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            if (Config.TextRenderingHint.HasValue) g.TextRenderingHint = Config.TextRenderingHint.Value;
            return new Core.CanvasGDI(g);
        }

        public static Canvas HighLay(this Graphics g, float dpi, bool text = false)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            if (text) g.TextRenderingHint = Config.TextRenderingHint ?? System.Drawing.Text.TextRenderingHint.AntiAlias;
            return new Core.CanvasGDI(g, dpi);
        }

        public static Canvas HighLay(this Graphics g, bool text = false)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            if (text) g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            return new Core.CanvasGDI(g);
        }

        public static void GDI(this IControl control, Action<Canvas> action)
        {
            using (var bmp = new Bitmap(1, 1))
            {
                using (var g = Graphics.FromImage(bmp))
                {
                    action(g.HighLay(control.Dpi));
                }
            }
        }
        public static void GDI(this BaseForm form, Action<Canvas> action)
        {
            using (var bmp = new Bitmap(1, 1))
            {
                using (var g = Graphics.FromImage(bmp))
                {
                    action(g.HighLay(form.Dpi));
                }
            }
        }
        public static void GDI(this ILayeredForm form, Action<Canvas> action)
        {
            using (var bmp = new Bitmap(1, 1))
            {
                using (var g = Graphics.FromImage(bmp))
                {
                    action(g.HighLay(form.Dpi));
                }
            }
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

        public static T GDI<T>(this IControl control, Func<Canvas, T> action)
        {
            using (var bmp = new Bitmap(1, 1))
            {
                using (var g = Graphics.FromImage(bmp))
                {
                    return action(g.HighLay(control.Dpi));
                }
            }
        }
        public static T GDI<T>(this BaseForm form, Func<Canvas, T> action)
        {
            using (var bmp = new Bitmap(1, 1))
            {
                using (var g = Graphics.FromImage(bmp))
                {
                    return action(g.HighLay(form.Dpi));
                }
            }
        }
        public static T GDI<T>(this ILayeredForm form, Func<Canvas, T> action)
        {
            using (var bmp = new Bitmap(1, 1))
            {
                using (var g = Graphics.FromImage(bmp))
                {
                    return action(g.HighLay(form.Dpi));
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
        /// <param name="roundTL">↖</param>
        /// <param name="roundTR">↗</param>
        /// <param name="roundBR">↘</param>
        /// <param name="roundBL">↙</param>
        public static GraphicsPath RoundPath(this Rectangle rect, float radius, bool roundTL, bool roundTR, bool roundBR, bool roundBL)
        {
            var path = new GraphicsPath();
            if (radius <= 0F) path.AddRectangle(rect);
            else
            {
                float diameter = radius * 2F;
                var arc = new RectangleF(rect.X, rect.Y, diameter, diameter);

                // TL
                if (roundTL) path.AddArc(arc, 180, 90);
                else path.AddLine(rect.X, rect.Y, rect.Right - diameter, rect.Y);

                // TR
                arc.X = rect.Right - diameter;
                if (roundTR) path.AddArc(arc, 270, 90);
                else path.AddLine(rect.Right, rect.Y, rect.Right, rect.Bottom - diameter);

                // BR
                arc.Y = rect.Bottom - diameter;
                if (roundBR) path.AddArc(arc, 0, 90);
                else path.AddLine(rect.Right, rect.Bottom, rect.X + diameter, rect.Bottom);

                // BL
                arc.X = rect.Left;
                if (roundBL) path.AddArc(arc, 90, 90);
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
        /// <param name="roundTL">↖</param>
        /// <param name="roundTR">↗</param>
        /// <param name="roundBR">↘</param>
        /// <param name="roundBL">↙</param>
        public static GraphicsPath RoundPath(this RectangleF rect, float radius, bool roundTL, bool roundTR, bool roundBR, bool roundBL)
        {
            var path = new GraphicsPath();
            if (radius > 0)
            {
                radius = Math.Min(radius, Math.Min(rect.Width, rect.Height) / 2);

                float diameter = radius * 2F;
                var arc = new RectangleF(rect.X, rect.Y, diameter, diameter);

                // TL
                if (roundTL) path.AddArc(arc, 180, 90);
                else path.AddLine(rect.X, rect.Y, rect.Right - diameter, rect.Y);

                // TR
                arc.X = rect.Right - diameter;
                if (roundTR) path.AddArc(arc, 270, 90);
                else path.AddLine(rect.Right, rect.Y, rect.Right, rect.Bottom - diameter);

                // BR
                arc.Y = rect.Bottom - diameter;
                if (roundBR) path.AddArc(arc, 0, 90);
                else path.AddLine(rect.Right, rect.Bottom, rect.X + diameter, rect.Bottom);

                // BL
                arc.X = rect.Left;
                if (roundBL) path.AddArc(arc, 90, 90);
                else path.AddLine(rect.X, rect.Bottom, rect.X, rect.Y + diameter);

                path.CloseFigure();
            }
            else path.AddRectangle(rect);
            return path;
        }

        /// <summary>
        /// 创建具有自定义圆角的路径
        /// </summary>
        /// <param name="rect">矩形区域</param>
        /// <param name="radiusTL">左上角圆角半径</param>
        /// <param name="radiusTR">右上角圆角半径</param>
        /// <param name="radiusBR">右下角圆角半径</param>
        /// <param name="radiusBL">左下角圆角半径</param>
        /// <returns>圆角路径</returns>
        public static GraphicsPath RoundPath(this Rectangle rect, float radiusTL, float radiusTR, float radiusBR, float radiusBL)
        {
            var path = new GraphicsPath();

            // 处理无效值：限制圆角在[0, min(width/2, height/2)]范围内
            float maxRadius = Math.Min(rect.Width / 2f, rect.Height / 2f);
            radiusTL = Math.Max(0, Math.Min(radiusTL, maxRadius));
            radiusTR = Math.Max(0, Math.Min(radiusTR, maxRadius));
            radiusBR = Math.Max(0, Math.Min(radiusBR, maxRadius));
            radiusBL = Math.Max(0, Math.Min(radiusBL, maxRadius));

            // 如果所有圆角都是0，直接添加矩形
            if (radiusTL <= 0f && radiusTR <= 0f && radiusBR <= 0f && radiusBL <= 0f)
            {
                path.AddRectangle(rect);
                return path;
            }

            float x = rect.X;
            float y = rect.Y;
            float right = x + rect.Width;
            float bottom = y + rect.Height;

            // 从顶部边的起点开始
            path.StartFigure();
            path.AddLine(x + radiusTL, y, right - radiusTR, y); // 顶部边

            // 右上角
            if (radiusTR > 0) path.AddArc(right - radiusTR * 2, y, radiusTR * 2, radiusTR * 2, 270, 90);

            // 右侧边
            path.AddLine(right, y + radiusTR, right, bottom - radiusBR);

            // 右下角
            if (radiusBR > 0) path.AddArc(right - radiusBR * 2, bottom - radiusBR * 2, radiusBR * 2, radiusBR * 2, 0, 90);

            // 底部边
            path.AddLine(right - radiusBR, bottom, x + radiusBL, bottom);

            // 左下角
            if (radiusBL > 0) path.AddArc(x, bottom - radiusBL * 2, radiusBL * 2, radiusBL * 2, 90, 90);

            // 左侧边
            path.AddLine(x, bottom - radiusBL, x, y + radiusTL);

            // 左上角
            if (radiusTL > 0) path.AddArc(x, y, radiusTL * 2, radiusTL * 2, 180, 90);

            path.CloseFigure();
            return path;
        }

        /// <summary>
        /// 创建具有自定义圆角的路径
        /// </summary>
        /// <param name="rect">矩形区域</param>
        /// <param name="radiusTL">左上角圆角半径</param>
        /// <param name="radiusTR">右上角圆角半径</param>
        /// <param name="radiusBR">右下角圆角半径</param>
        /// <param name="radiusBL">左下角圆角半径</param>
        /// <returns>圆角路径</returns>
        public static GraphicsPath RoundPath(this RectangleF rect, float radiusTL, float radiusTR, float radiusBR, float radiusBL)
        {
            var path = new GraphicsPath();

            // 处理无效值：限制圆角在[0, min(width/2, height/2)]范围内
            float maxRadius = Math.Min(rect.Width / 2f, rect.Height / 2f);
            radiusTL = Math.Max(0, Math.Min(radiusTL, maxRadius));
            radiusTR = Math.Max(0, Math.Min(radiusTR, maxRadius));
            radiusBR = Math.Max(0, Math.Min(radiusBR, maxRadius));
            radiusBL = Math.Max(0, Math.Min(radiusBL, maxRadius));

            // 如果所有圆角都是0，直接添加矩形
            if (radiusTL <= 0f && radiusTR <= 0f && radiusBR <= 0f && radiusBL <= 0f)
            {
                path.AddRectangle(rect);
                return path;
            }

            float x = rect.X;
            float y = rect.Y;
            float right = x + rect.Width;
            float bottom = y + rect.Height;

            // 从顶部边的起点开始
            path.StartFigure();
            path.AddLine(x + radiusTL, y, right - radiusTR, y); // 顶部边

            // 右上角
            if (radiusTR > 0) path.AddArc(right - radiusTR * 2, y, radiusTR * 2, radiusTR * 2, 270, 90);

            // 右侧边
            path.AddLine(right, y + radiusTR, right, bottom - radiusBR);

            // 右下角
            if (radiusBR > 0) path.AddArc(right - radiusBR * 2, bottom - radiusBR * 2, radiusBR * 2, radiusBR * 2, 0, 90);

            // 底部边
            path.AddLine(right - radiusBR, bottom, x + radiusBL, bottom);

            // 左下角
            if (radiusBL > 0) path.AddArc(x, bottom - radiusBL * 2, radiusBL * 2, radiusBL * 2, 90, 90);

            // 左侧边
            path.AddLine(x, bottom - radiusBL, x, y + radiusTL);

            // 左上角
            if (radiusTL > 0) path.AddArc(x, y, radiusTL * 2, radiusTL * 2, 180, 90);

            path.CloseFigure();
            return path;
        }

        /// <summary>
        /// 创建圆角矩形路径的核心方法
        /// </summary>
        /// <param name="rect">矩形区域</param>
        /// <param name="radius">圆角半径</param>
        /// <returns>圆角矩形路径</returns>
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

        /// <summary>
        /// 创建胶囊形状路径的核心方法
        /// </summary>
        /// <param name="rect">矩形区域</param>
        /// <returns>胶囊形状路径</returns>
        static GraphicsPath CapsulePathCore(RectangleF rect)
        {
            var path = new GraphicsPath();
            AddCapsule(path, rect);
            return path;
        }

        /// <summary>
        /// 向路径添加胶囊形状
        /// </summary>
        /// <param name="path">要添加胶囊形状的路径</param>
        /// <param name="rect">矩形区域</param>
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

        public static GraphicsPath PathJoin(RectangleF rect, int radius, float dpi, TShape shape, TJoinMode joinMode, bool joinLeft, bool joinRight)
        {
            switch (shape)
            {
                case TShape.Circle:
                    var path = new GraphicsPath();
                    path.AddEllipse(rect);
                    return path;
                case TShape.Round:
                    return PathJoin(rect, rect.Height, joinMode, joinLeft, joinRight);
            }
            return PathJoin(rect, radius * dpi, joinMode, joinLeft, joinRight);
        }
        public static GraphicsPath PathJoin(RectangleF rect, float radius, TShape shape, TJoinMode joinMode, bool joinLeft, bool joinRight)
        {
            switch (shape)
            {
                case TShape.Circle:
                    var path = new GraphicsPath();
                    path.AddEllipse(rect);
                    return path;
                case TShape.Round:
                    return PathJoin(rect, rect.Height, joinMode, joinLeft, joinRight);
            }
            return PathJoin(rect, radius, joinMode, joinLeft, joinRight);
        }
        public static GraphicsPath PathJoin(RectangleF rect, int radius, float dpi, bool round, TJoinMode joinMode, bool joinLeft, bool joinRight)
        {
            if (round) return PathJoin(rect, rect.Height, joinMode, joinLeft, joinRight);
            return PathJoin(rect, radius * dpi, joinMode, joinLeft, joinRight);
        }
        internal static GraphicsPath PathJoin(RectangleF rect, float radius, TJoinMode joinMode, bool joinLeft, bool joinRight)
        {
            switch (joinMode)
            {
                case TJoinMode.Left:
                    return rect.RoundPath(radius, true, false, false, true);
                case TJoinMode.Right:
                    return rect.RoundPath(radius, false, true, true, false);
                case TJoinMode.LR:
                case TJoinMode.TB:
                    return rect.RoundPath(0);
                case TJoinMode.Top:
                    return rect.RoundPath(radius, true, true, false, false);
                case TJoinMode.Bottom:
                    return rect.RoundPath(radius, false, false, true, true);
                case TJoinMode.None:
                default:
                    if (joinLeft && joinRight) return rect.RoundPath(0);
                    else if (joinLeft) return rect.RoundPath(radius, false, true, true, false);
                    else if (joinRight) return rect.RoundPath(radius, true, false, false, true);
                    return rect.RoundPath(radius);
            }
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
            g.Svg(icon.Svg, rect, icon.Fill);
        }
        internal static void PaintIcons(this Canvas g, TType icon, Rectangle rect, string keyid, TAMode colorScheme, string name)
        {
            switch (icon)
            {
                case TType.Success:
                    g.Svg(SvgDb.IcoSuccess, rect, Colour.Success.Get(colorScheme, keyid, name));
                    break;
                case TType.Info:
                    g.Svg(SvgDb.IcoInfo, rect, Colour.Info.Get(colorScheme, keyid, name));
                    break;
                case TType.Warn:
                    g.Svg(SvgDb.IcoWarn, rect, Colour.Warning.Get(colorScheme, keyid, name));
                    break;
                case TType.Error:
                    g.Svg(SvgDb.IcoError, rect, Colour.Error.Get(colorScheme, keyid, name));
                    break;
            }
        }
        internal static void PaintIcons(this Canvas g, TType icon, Rectangle rect, string keyid, TAMode colorScheme)
        {
            switch (icon)
            {
                case TType.Success:
                    g.Svg(SvgDb.IcoSuccess, rect, Colour.Success.Get(colorScheme, keyid));
                    break;
                case TType.Info:
                    g.Svg(SvgDb.IcoInfo, rect, Colour.Info.Get(colorScheme, keyid));
                    break;
                case TType.Warn:
                    g.Svg(SvgDb.IcoWarn, rect, Colour.Warning.Get(colorScheme, keyid));
                    break;
                case TType.Error:
                    g.Svg(SvgDb.IcoError, rect, Colour.Error.Get(colorScheme, keyid));
                    break;
            }
        }
        internal static void PaintIcons(this Canvas g, TType icon, string? svg, Rectangle rect, Colour colour, string keyid, TAMode colorScheme, string name)
        {
            if (svg == null)
            {
                using (var brush = new SolidBrush(Colour.BgBase.Get(colorScheme, keyid, name)))
                {
                    g.FillEllipse(brush, new Rectangle(rect.X + 1, rect.Y + 1, rect.Width - 2, rect.Height - 2));
                }
                switch (icon)
                {
                    case TType.Success:
                        g.Svg(SvgDb.IcoSuccess, rect, Colour.Success.Get(colorScheme, keyid, name));
                        break;
                    case TType.Info:
                        g.Svg(SvgDb.IcoInfo, rect, Colour.Info.Get(colorScheme, keyid, name));
                        break;
                    case TType.Warn:
                        g.Svg(SvgDb.IcoWarn, rect, Colour.Warning.Get(colorScheme, keyid, name));
                        break;
                    case TType.Error:
                        g.Svg(SvgDb.IcoError, rect, Colour.Error.Get(colorScheme, keyid, name));
                        break;
                }
            }
            else
            {
                switch (icon)
                {
                    case TType.Success:
                        g.Svg(svg, rect, Colour.Success.Get(colorScheme, keyid, name));
                        break;
                    case TType.Info:
                        g.Svg(svg, rect, Colour.Info.Get(colorScheme, keyid, name));
                        break;
                    case TType.Warn:
                        g.Svg(svg, rect, Colour.Warning.Get(colorScheme, keyid, name));
                        break;
                    case TType.Error:
                        g.Svg(svg, rect, Colour.Error.Get(colorScheme, keyid, name));
                        break;
                    default:
                        g.Svg(svg, rect);
                        break;
                }
            }
        }
        internal static void PaintIcons(this Canvas g, TType icon, string? svg, Rectangle rect, Colour colour, string keyid, TAMode colorScheme)
        {
            if (svg == null)
            {
                using (var brush = new SolidBrush(Colour.BgBase.Get(colorScheme, keyid)))
                {
                    g.FillEllipse(brush, new Rectangle(rect.X + 1, rect.Y + 1, rect.Width - 2, rect.Height - 2));
                }
                switch (icon)
                {
                    case TType.Success:
                        g.Svg(SvgDb.IcoSuccess, rect, Colour.Success.Get(colorScheme, keyid));
                        break;
                    case TType.Info:
                        g.Svg(SvgDb.IcoInfo, rect, Colour.Info.Get(colorScheme, keyid));
                        break;
                    case TType.Warn:
                        g.Svg(SvgDb.IcoWarn, rect, Colour.Warning.Get(colorScheme, keyid));
                        break;
                    case TType.Error:
                        g.Svg(SvgDb.IcoError, rect, Colour.Error.Get(colorScheme, keyid));
                        break;
                }
            }
            else
            {
                switch (icon)
                {
                    case TType.Success:
                        g.Svg(svg, rect, Colour.Success.Get(colorScheme, keyid));
                        break;
                    case TType.Info:
                        g.Svg(svg, rect, Colour.Info.Get(colorScheme, keyid));
                        break;
                    case TType.Warn:
                        g.Svg(svg, rect, Colour.Warning.Get(colorScheme, keyid));
                        break;
                    case TType.Error:
                        g.Svg(svg, rect, Colour.Error.Get(colorScheme, keyid));
                        break;
                    default:
                        g.Svg(svg, rect);
                        break;
                }
            }
        }
        internal static void PaintIconGhosts(this Canvas g, TType icon, Rectangle rect, Color color)
        {
            switch (icon)
            {
                case TType.Success:
                    g.Svg(SvgDb.IcoSuccessGhost, rect, color);
                    break;
                case TType.Info:
                    g.Svg(SvgDb.IcoInfoGhost, rect, color);
                    break;
                case TType.Warn:
                    g.Svg(SvgDb.IcoWarnGhost, rect, color);
                    break;
                case TType.Error:
                    g.Svg(SvgDb.IcoErrorGhost, rect, color);
                    break;
            }
        }
        internal static void PaintIconCore(this Canvas g, Rectangle rect, string svg, Color back, Color fore)
        {
            using (var brush = new SolidBrush(back))
            {
                g.FillEllipse(brush, new Rectangle(rect.X + 1, rect.Y + 1, rect.Width - 2, rect.Height - 2));
            }
            g.Svg(svg, rect, fore);
        }
        internal static void PaintIconCore(this Canvas g, Rectangle rect, string svg, Color color, float dot)
        {
            int size = (int)(rect.Height * dot);
            var rect_ico = new Rectangle(rect.X + (rect.Width - size) / 2, rect.Y + (rect.Height - size) / 2, size, size);
            g.Svg(svg, rect_ico, color);
        }

        #endregion

        #region 阴影/徽标

        #region 徽标

        public static void PaintBadge(this IControl control, Canvas g) => PaintBadge(control, control.Font, control.ClientRectangle, g, control.ColorScheme, control.Name);
        public static void PaintBadge(this BadgeConfig badegConfig, Font Font, Rectangle Rect, Canvas g, TAMode colorScheme, string cname)
        {
            if (badegConfig.BadgeSvg != null)
            {
                int hasx = (int)(badegConfig.BadgeOffsetX * g.Dpi), hasy = (int)(badegConfig.BadgeOffsetY * g.Dpi);
                using (var font = new Font(Font.FontFamily, Font.Size * badegConfig.BadgeSize))
                {
                    var size_badge = g.MeasureString(Config.NullText, font).Height;
                    var rect_badge = PaintBadge(Rect, badegConfig.BadgeAlign, hasx, hasy, size_badge, size_badge);
                    g.Svg(badegConfig.BadgeSvg, rect_badge, badegConfig.BadgeBack ?? Colour.Error.Get(colorScheme, nameof(Badge), cname));
                }
            }
            else if (badegConfig.Badge != null)
            {
                Color color = badegConfig.BadgeBack ?? Colour.Error.Get(colorScheme, nameof(Badge), cname),
                    borcolor = badegConfig.BadgeBorderColor ?? Colour.ErrorColor.Get(colorScheme, nameof(Badge), cname);
                var rect = Rect;
                float borsize = g.Dpi;
                if (badegConfig.BadgeBorderWidth.HasValue) borsize = badegConfig.BadgeBorderWidth.Value * g.Dpi;
                using (var font = new Font(Font.FontFamily, Font.Size * badegConfig.BadgeSize))
                {
                    int hasx = (int)(badegConfig.BadgeOffsetX * g.Dpi), hasy = (int)(badegConfig.BadgeOffsetY * g.Dpi);
                    if (string.IsNullOrWhiteSpace(badegConfig.Badge))
                    {
                        var size = g.MeasureString(Config.NullText, font).Height / 2;
                        var rect_badge = PaintBadge(rect, badegConfig.BadgeAlign, hasx, hasy, size, size);
                        using (var brush = new SolidBrush(color))
                        {
                            if (badegConfig.BadgeMode)
                            {
                                float b2 = borsize * 2, rr = size * 0.2F, rr2 = rr * 2;
                                g.FillEllipse(borcolor, new RectangleF(rect_badge.X - borsize, rect_badge.Y - borsize, rect_badge.Width + b2, rect_badge.Height + b2));
                                using (var path = rect_badge.RoundPath(1, true))
                                {
                                    path.AddEllipse(new RectangleF(rect_badge.X + rr, rect_badge.Y + rr, rect_badge.Width - rr2, rect_badge.Height - rr2));
                                    g.Fill(color, path);
                                }
                            }
                            else
                            {
                                g.FillEllipse(color, rect_badge);
                                if (borsize > 0) g.DrawEllipse(borcolor, borsize, rect_badge);
                            }
                        }
                    }
                    else
                    {
                        Color fore = badegConfig.BadgeFore ?? Colour.ErrorColor.Get(colorScheme, nameof(Badge), cname);
                        var size = g.MeasureString(badegConfig.Badge, font).Size(g, 4, 2);
                        int size_badge = (int)(size.Height * 1.2F);
                        if (size.Height > size.Width)
                        {
                            var rect_badge = PaintBadge(rect, badegConfig.BadgeAlign, hasx, hasy, size_badge, size_badge);
                            g.FillEllipse(color, rect_badge);
                            if (borsize > 0) g.DrawEllipse(borcolor, borsize, rect_badge);
                            g.String(badegConfig.Badge, font, fore, rect_badge, FormatFlags.Center | FormatFlags.NoWrap);
                        }
                        else
                        {
                            int w_badge = size.Width + (size_badge - size.Height);
                            var rect_badge = PaintBadge(rect, badegConfig.BadgeAlign, hasx, hasy, w_badge, size_badge);
                            using (var path = rect_badge.RoundPath(rect_badge.Height))
                            {
                                g.Fill(color, path);
                                if (borsize > 0) g.Draw(borcolor, borsize, path);
                            }
                            g.String(badegConfig.Badge, font, fore, rect_badge, FormatFlags.Center | FormatFlags.NoWrap);
                        }
                    }
                }
            }
        }

        public static void PaintBadge(this IControl control, DateBadge badge, Rectangle rect, Canvas g)
        {
            var color = badge.Fill ?? control.BadgeBack ?? Colour.Error.Get(control.ColorScheme, nameof(Badge), control.Name);
            float Dpi = control.Dpi, borsize = Dpi;
            using (var font = new Font(control.Font.FontFamily, control.Font.Size * badge.Size))
            {
                int hasx = (int)(badge.OffsetX * Dpi), hasy = (int)(badge.OffsetY * Dpi);
                if (string.IsNullOrWhiteSpace(badge.Content))
                {
                    var size_badge = g.MeasureString(Config.NullText, font).Height / 2;
                    var rect_badge = PaintBadge(rect, badge.Align, hasx, hasy, size_badge, size_badge);
                    g.FillEllipse(color, rect_badge);
                    g.DrawEllipse(Colour.ErrorColor.Get(control.ColorScheme, nameof(Badge), control.Name), borsize, rect_badge);
                }
                else
                {
                    var size = g.MeasureString(badge.Content, font).Size(g, 4, 2);
                    int size_badge = (int)(size.Height * 1.2F);
                    if (size.Height > size.Width)
                    {
                        var rect_badge = PaintBadge(rect, badge.Align, hasx, hasy, size_badge, size_badge);
                        g.FillEllipse(color, rect_badge);
                        g.DrawEllipse(Colour.ErrorColor.Get(control.ColorScheme, nameof(Badge), control.Name), borsize, rect_badge);
                        g.String(badge.Content, font, Colour.ErrorColor.Get(control.ColorScheme, nameof(Badge), control.Name), rect_badge, FormatFlags.Center | FormatFlags.NoWrap);
                    }
                    else
                    {
                        int w_badge = size.Width + (size_badge - size.Height);
                        var rect_badge = PaintBadge(rect, badge.Align, hasx, hasy, w_badge, size_badge);
                        using (var path = rect_badge.RoundPath(rect_badge.Height))
                        {
                            g.Fill(color, path);
                            g.Draw(Colour.ErrorColor.Get(control.ColorScheme, nameof(Badge), control.Name), borsize, path);
                        }
                        g.String(badge.Content, font, Colour.ErrorColor.Get(control.ColorScheme, nameof(Badge), control.Name), rect_badge, FormatFlags.Center | FormatFlags.NoWrap);
                    }
                }
            }
        }

        /// <summary>
        /// 根据对齐方式计算徽章的位置
        /// </summary>
        /// <param name="rect">父容器矩形</param>
        /// <param name="align">对齐方式</param>
        /// <param name="x">X轴偏移量</param>
        /// <param name="y">Y轴偏移量</param>
        /// <param name="w">徽章宽度</param>
        /// <param name="h">徽章高度</param>
        /// <returns>徽章的最终位置和大小</returns>
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
            int shadow = (int)(config.Shadow * g.Dpi), shadowOffsetX = (int)(config.ShadowOffsetX * g.Dpi), shadowOffsetY = (int)(config.ShadowOffsetY * g.Dpi);
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
                g.Image(bmp_shadow, new Rectangle(_rect.X + shadowOffsetX, _rect.Y + shadowOffsetY, _rect.Width, _rect.Height), config.ShadowOpacity);
            }
        }

        #endregion

        public static void PaintEmpty(this Canvas g, Rectangle rect, Font font, Color fore, string? text = null, Image? image = null, int offset = 0) => PaintEmpty(g, rect, font, fore, text, image, offset, FormatFlags.Center);

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
                    int gap = (int)(8 * g.Dpi);
                    var size = g.MeasureString(emptytext, font);
                    Rectangle rect_img = new Rectangle(rect.X + (rect.Width - bmp.Width) / 2, rect.Y + (rect.Height - bmp.Height) / 2 - size.Height, bmp.Width, bmp.Height), rect_font = new Rectangle(rect.X, rect_img.Bottom + gap, rect.Width, size.Height);
                    g.Image(bmp, rect_img);
                    g.String(emptytext, font, brush, rect_font, format);
                }
                else if (Config.EmptyImageSvg != null)
                {
                    var size = g.MeasureString(emptytext, font);
                    int gap = (int)(8 * g.Dpi), icon_size = (int)(size.Height * Config.EmptyImageRatio);

                    Rectangle rect_img = new Rectangle(rect.X + (rect.Width - icon_size) / 2, rect.Y + (rect.Height - icon_size) / 2 - size.Height, icon_size, icon_size),
                        rect_font = new Rectangle(rect.X, rect_img.Bottom + gap, rect.Width, size.Height);

                    if (!g.Svg(Config.IsDark ? Config.EmptyImageSvg[1] : Config.EmptyImageSvg[0], rect_img, fore))
                    {
                        g.String(emptytext, font, brush, rect, format);
                        return;
                    }
                    g.String(emptytext, font, brush, rect_font, format);
                }
                else g.String(emptytext, font, brush, rect, format);
            }
        }

        public static void PaintLoading(this Canvas g, Image? icon, string? svg, Rectangle rect, float angle, float value, Color color, int size, float ratio, Color? bg = null)
        {
            if (icon == null && svg == null)
            {
                using (var pen = new Pen(color, size * ratio))
                {
                    if (bg.HasValue)
                    {
                        using (var brush_bg = new Pen(bg.Value, pen.Width))
                        {
                            g.DrawEllipse(brush_bg, rect);
                        }
                    }
                    pen.StartCap = pen.EndCap = LineCap.Round;
                    g.DrawArc(pen, rect, angle * 360F, value * 360F);
                }
            }
            else
            {
                var cirSize2 = rect.Height / 2F;
                int cirSizeNum = (int)cirSize2;
                g.TranslateTransform(rect.X + cirSize2, rect.Y + cirSize2);
                g.RotateTransform(angle);
                var rect_center = new Rectangle(-cirSizeNum, -cirSizeNum, rect.Width, rect.Height);
                if (icon != null) g.Image(icon, rect_center);
                if (svg != null) g.Svg(svg, rect_center, color);
                g.ResetTransform();
            }
        }
        public static void PaintLoadingWave(this Canvas g, GraphicsPath path, Rectangle rect, float radius, Color color, float dpi, float LoadingWaveValue, float AnimationLoadingValue, int LoadingWaveCount, int LoadingWaveSize, bool LoadingWaveVertical)
        {
            using (var brush = new SolidBrush(color))
            {
                if (LoadingWaveValue >= 1) g.Fill(brush, path);
                else if (LoadingWaveCount > 0)
                {
                    var state = g.Save();
                    g.SetClip(path);
                    g.ResetTransform();
                    int len = (int)(LoadingWaveSize * dpi), count = LoadingWaveCount * 2 + 2;
                    if (count < 6) count = 6;
                    if (LoadingWaveVertical)
                    {
                        int pvalue = (int)(rect.Height * LoadingWaveValue);
                        if (pvalue > 0)
                        {
                            pvalue = rect.Height - pvalue + rect.Y;
                            int wd = rect.Width / LoadingWaveCount, wd2 = wd * 2, pvalue2 = pvalue - len, rr = rect.X + wd * count;
                            using (var path_line = new GraphicsPath())
                            {
                                g.TranslateTransform(-(wd + wd2 * AnimationLoadingValue), 0);
                                path_line.AddLine(rr, pvalue, rr, rect.Bottom);
                                path_line.AddLine(rr, rect.Bottom, rect.X, rect.Bottom);
                                path_line.AddLine(rect.X, rect.Bottom, rect.X, pvalue);
                                bool to = true;
                                var line = new List<PointF>(count);
                                for (int i = 0; i < count + 1; i++)
                                {
                                    line.Add(new PointF(rect.X + wd * i, to ? pvalue : pvalue2));
                                    to = !to;
                                }
                                path_line.AddCurve(line.ToArray());
                                g.Fill(brush, path_line);
                            }
                        }
                    }
                    else
                    {
                        int pvalue = (int)(rect.Width * LoadingWaveValue);
                        if (pvalue > 0)
                        {
                            pvalue += rect.X;
                            int wd = rect.Height / LoadingWaveCount, wd2 = wd * 2, pvalue2 = pvalue + len, rb = rect.Y + wd * count;

                            using (var path_line = new GraphicsPath())
                            {
                                g.TranslateTransform(0, -(wd + wd2 * AnimationLoadingValue));
                                path_line.AddLine(pvalue, rb, rect.X, rb);
                                path_line.AddLine(rect.X, rb, rect.X, rect.Y);
                                path_line.AddLine(rect.X, rect.Y, pvalue, rect.Y);
                                bool to = true;
                                var line = new List<PointF>(count);
                                for (int i = 0; i < count + 1; i++)
                                {
                                    line.Add(new PointF(to ? pvalue : pvalue2, rect.Y + wd * i));
                                    to = !to;
                                }
                                path_line.AddCurve(line.ToArray());
                                g.Fill(brush, path_line);
                            }
                        }
                    }
                    g.Restore(state);
                }
                else
                {
                    if (LoadingWaveVertical)
                    {
                        int pvalue = (int)(rect.Height * LoadingWaveValue);
                        if (pvalue > 0)
                        {
                            var state = g.Save();
                            g.SetClip(new Rectangle(rect.X, rect.Y + rect.Height - pvalue, rect.Width, pvalue));
                            g.Fill(brush, path);
                            g.Restore(state);
                        }
                    }
                    else
                    {
                        int pvalue = (int)(rect.Width * LoadingWaveValue);
                        if (pvalue > 0)
                        {
                            var state = g.Save();
                            g.SetClip(new Rectangle(rect.X, rect.Y, pvalue, rect.Height));
                            g.Fill(brush, path);
                            g.Restore(state);
                        }
                    }
                }
            }
        }

        #region 图像处理

        #region 模糊

        /// <summary>
        /// 通用模糊入口
        /// </summary>
        /// <param name="bmp">目标位图（就地修改）</param>
        /// <param name="range">模糊范围（支撑直径，实际 σ ≈ range/2.31）</param>
        public static void Blur(Bitmap bmp, int range)
        {
            if (Config.ShadowModeCompatible) BlurColor(bmp, range);
            else BlurAlpha(bmp, range);
        }

        #region 模糊扩散半径（给阴影预留画布）

        public static int BlurRadius(int range) => BlurColorRadius(range);

        /// <summary>
        /// <see cref="BlurColor"/>（以及转发到它的 <see cref="BlurColor"/>）模糊后向外的完整扩散半径（像素，单边）
        /// <para>四趟一维盒模糊、半径 h = range / 2，横纵各两趟，所以单边扩散 2h = range（range 为奇数时少 1 像素，
        /// 与实现里的整数除法一致）。结果本身是整数，不需要向上取整。</para>
        /// <para>实测（range=16/32/64/128）alpha 仍 &gt; 0 的最远距离是 15/30/59/114，
        /// 比返回值略小 —— 因为更外面的尾巴已经 &lt; 0.5/255，四舍五入就是 0。留白按返回值取即可。</para>
        /// </summary>
        /// <param name="range">与传给 <see cref="BlurColor"/> 的 range 完全相同</param>
        public static int BlurColorRadius(int range) => range > 1 ? 2 * (range / 2) : 0;

        /// <summary>
        /// <see cref="BlurAlpha"/> 模糊后向外的完整扩散半径（像素，单边）
        /// <para>只模糊 alpha，但核与 <see cref="BlurColor"/> 一致：两级盒式级联、半径 h = range / 2，
        /// 因此扩散同样是 2h = range（奇数少 1 像素）。</para>
        /// <para>实测（range=16/32/64/128）alpha &gt; 0 的最远距离为 16/29/61/115：range ≤ 64 时与返回值相差不超过 3 像素，
        /// range 更大时尾部（每像素 &lt; 1/255）会被逐趟的字节取整吃掉，返回值略偏保守——留白宁多勿少。</para>
        /// </summary>
        /// <param name="range">与传给 <see cref="BlurAlpha"/> 的 range 完全相同</param>
        public static int BlurAlphaRadius(int range) => range > 1 ? 2 * (range / 2) : 0;

        /// <summary>
        /// <see cref="BlurCSS"/> 模糊后向外的完整扩散半径（像素，单边）
        /// <para><b>扩散范围怎么来的</b>：三次盒模糊的支撑半径就是三个半径之和 r1 + r2 + r3。
        /// 半径由「方差匹配」反推：三次盒模糊的方差和为 (w1²-1 + w2²-1 + w3²-1)/12 = σ²，
        /// 于是窗宽 w ≈ √(4σ²+1)（取奇数），半径 r = (w-1)/2。σ 较大时 r ≈ σ，三者之和 ≈ 3σ。</para>
        /// <para>σ 过大时（255·w1·w2·w3 溢出 int32）会拆成 n 组级联，总支撑再乘 n；大区域还会先降采样，
        /// 块平均多覆盖半格 ((s-1)/2)、双线性放大再外扩一格 (s)，本方法取两条路径的较大值。</para>
        /// <para><b>实测对照</b>（range=8/16/32/64）返回值 25/49/98/194，alpha &gt; 0 的最远距离 21/41/83/163，
        /// 前者约为后者的 1.19 倍——多出来的部分全是 &lt; 1/255 的尾巴。窗口阴影常用 range 10~30，
        /// 多预留的十几像素可以忽略；实在想省空间，按可见距离取（约 0.84 倍）也不会看出被切。</para>
        /// <para><b>注意</b>：range &gt; 100 时（255·w1·w2·w3 会溢出 int32）内核自动拆成 2 组级联，支撑半径随之翻倍，
        /// 而可见边界只到 300 左右（range=128 时），即返回值 544 偏保守约 1.8 倍——超大 σ 一般只用于效果调试，正常。</para>
        /// </summary>
        /// <param name="range">与传给 <see cref="BlurCSS"/> 的 range 完全相同（即 σ）</param>
        public static int BlurCSSRadius(int range)
        {
            if (range < 1) return 0;

            int passes, r1, r2, r3, pad;
            uint weight;
            BlurPlan(range, out passes, out r1, out r2, out r3, out weight, out pad);
            int radius = pad * passes;// 全分辨率路径：三级半径之和 × 级联组数

            // 降采样路径（σ ≥ 8 且区域够大时才会真正走）：小图的核按 range/s 重算，
            // 换算回原尺寸后再加上块平均的半格与双线性的一格，与全分辨率路径取大
            int s = range >= 24 ? 4 : (range >= 8 ? 2 : 1);
            if (s > 1)
            {
                int ss = range / s;
                BlurPlan(ss < 1 ? 1 : ss, out passes, out r1, out r2, out r3, out weight, out pad);
                int down = (int)Math.Ceiling(pad * passes * (double)s + (s - 1) / 2.0 + s);
                if (down > radius) radius = down;
            }
            return radius;
        }

        #endregion

        #region 全彩模糊（旧版实现，保留）

        /// <summary>
        /// 全通道模糊（历史实现，本库最快的全彩版本）：横 → 纵 → 横 → 纵，四趟一维盒式滑动窗
        /// <para><b>算法</b>：每趟对整行/整列做半径 h = range / 2 的盒式滑动窗——进一个像素、出一个像素，
        /// 单像素 O(1) 与半径无关；B/G/R/A 四个通道在同一个累加器组里一起算，最后一次打包写回。
        /// 横两趟 + 纵两趟，方差叠加后已经很接近高斯，肉眼与三次盒模糊几乎无差。</para>
        /// <para><b>为什么快</b>：每像素 16 次加减（4 趟 × 4 通道）、中段用 ceil(2³²/d) 魔数乘法代替除法、
        /// 无临时图、无预乘/反预乘、横纵都是就地读写且顺序访存。1080p 一张 range=16 的图约 29ms。</para>
        /// <para><b>语义</b>：range 是「支撑直径」，实际 σ ≈ range / 2.31；range ≤ 1 直接返回。
        /// 图外按「窗口截断」处理（边缘除数随之变小），不像 CSS 那样向图外淡出，所以形状贴边时边缘不会被压暗。</para>
        /// <para><b>色彩</b>：直接在直通 alpha 空间平均 RGB，透明像素的 RGB=0 会被算进均值，
        /// 因此彩色/半透明内容模糊后边缘会发暗。要颜色正确用 <see cref="BlurCSS"/>；
        /// 纯色（黑白）阴影用更快的 <see cref="BlurAlpha"/>。</para>
        /// <para>给阴影预留画布时，向外扩散的完整半径用 <see cref="BlurColorRadius"/> 计算。</para>
        /// </summary>
        /// <param name="bmp">目标位图（就地修改，统一按 32bppArgb 处理）</param>
        /// <param name="range">模糊范围（支撑直径，实际 σ ≈ range/2.31）</param>
        public static unsafe void BlurColor(Bitmap bmp, int range)
        {
            if (range > 1)
            {
                int width = bmp.Width, height = bmp.Height;
                using (var unsafeBitmap = new UnsafeBitmap(bmp, true))
                {
                    int halfRange = range / 2;
                    ColorBgra* pointer = unsafeBitmap.Pointer;
                    var tmp = new ColorBgra[width > height ? width : height];
                    fixed (ColorBgra* line = tmp)
                    {
                        // 横向 → 纵向 → 横向 → 纵向
                        for (int y = 0; y < height; y++) BlurLine(pointer + y * width, 1, width, halfRange, line);
                        for (int x = 0; x < width; x++) BlurLine(pointer + x, width, height, halfRange, line);
                        for (int y = 0; y < height; y++) BlurLine(pointer + y * width, 1, width, halfRange, line);
                        for (int x = 0; x < width; x++) BlurLine(pointer + x, width, height, halfRange, line);
                    }
                }
            }
        }

        /// <summary>
        /// 单行/单列盒式模糊（滑动窗口，单像素 O(1)）
        /// </summary>
        /// <param name="p">起始像素指针</param>
        /// <param name="stride">步进（横向为 1，纵向为宽度）</param>
        /// <param name="n">像素个数</param>
        /// <param name="h">半径</param>
        /// <param name="tmp">长度不小于 n 的临时缓冲</param>
        static unsafe void BlurLine(ColorBgra* p, int stride, int n, int h, ColorBgra* tmp)
        {
            if (n < 1 || h < 1) return;

            // 复制到临时缓冲（后面要就地写回），顺便检测整条线是否全透明
            // 全透明时模糊结果依旧全透明，可直接跳过
            uint any = 0;
            ColorBgra* s = p;
            for (int i = 0; i < n; i++)
            {
                uint bgra = s->Bgra;
                tmp[i].Bgra = bgra;
                any |= bgra;
                s += stride;
            }
            if (any == 0) return;

            int w = h + h + 1;// 完整窗口大小
            int r = 0, g = 0, b = 0, a = 0, hits = 0;
            if (n >= w)
            {
                // 起始边缘：窗口尚未填满，除数递增
                for (int j = 0; j < w - 1; j++)
                {
                    ColorBgra color = tmp[j];
                    b += color.Blue;
                    g += color.Green;
                    r += color.Red;
                    a += color.Alpha;
                    hits++;
                    if (j >= h) (p + ((j - h) * stride))->Bgra = BlurPack(b, g, r, a, hits);
                }

                // 中段：窗口恒为 w，用 乘法+移位 取代整除
                uint m = (uint)((((ulong)1 << 32) + (uint)w - 1) / (uint)w);
                ColorBgra colorAdd = tmp[w - 1];
                b += colorAdd.Blue;
                g += colorAdd.Green;
                r += colorAdd.Red;
                a += colorAdd.Alpha;
                hits++;
                (p + ((w - 1 - h) * stride))->Bgra = BlurPackFast(b, g, r, a, m);
                for (int j = w; j < n; j++)
                {
                    ColorBgra ca = tmp[j], cr = tmp[j - w];
                    b += ca.Blue - cr.Blue;
                    g += ca.Green - cr.Green;
                    r += ca.Red - cr.Red;
                    a += ca.Alpha - cr.Alpha;
                    (p + ((j - h) * stride))->Bgra = BlurPackFast(b, g, r, a, m);
                }

                // 结束边缘：窗口逐步滑出，除数递减
                for (int j = n; j < n + h; j++)
                {
                    ColorBgra cr = tmp[j - w];
                    b -= cr.Blue;
                    g -= cr.Green;
                    r -= cr.Red;
                    a -= cr.Alpha;
                    hits--;
                    (p + ((j - h) * stride))->Bgra = BlurPack(b, g, r, a, hits);
                }
            }
            else
            {
                // 长度小于窗口的退化场景
                for (int j = 0; j < n + h; j++)
                {
                    if (j < n)
                    {
                        ColorBgra color = tmp[j];
                        b += color.Blue;
                        g += color.Green;
                        r += color.Red;
                        a += color.Alpha;
                        hits++;
                    }
                    if (j >= w)
                    {
                        ColorBgra color = tmp[j - w];
                        b -= color.Blue;
                        g -= color.Green;
                        r -= color.Red;
                        a -= color.Alpha;
                        hits--;
                    }
                    if (j >= h)
                    {
                        int x = j - h;
                        if (x < n) (p + (x * stride))->Bgra = BlurPack(b, g, r, a, hits);
                    }
                }
            }
        }

        static uint BlurPack(int b, int g, int r, int a, int d) => (uint)(b / d) | ((uint)(g / d) << 8) | ((uint)(r / d) << 16) | ((uint)(a / d) << 24);

        /// <summary>定窗口求均值：用 ceil(2^32/d) 魔数乘法代替整除</summary>
        static uint BlurPackFast(int b, int g, int r, int a, uint m) => (uint)(((ulong)(uint)b * m) >> 32) | ((uint)(((ulong)(uint)g * m) >> 32) << 8) | ((uint)(((ulong)(uint)r * m) >> 32) << 16) | ((uint)(((ulong)(uint)a * m) >> 32) << 24);

        #endregion

        #region 阴影模糊（单通道）

        /// <summary>
        /// 阴影专用模糊：只模糊 alpha 通道，颜色原样保留
        /// <para>阴影是一整块纯色，所以「模糊 RGB 再叠加」与「只模糊 alpha 再按原色合成」在数学上完全等价（直通 alpha 合成：bg·(1-a) + color·a）。
        /// 因此这里完全不碰 RGB：每像素只读写 1 字节（原来的 1/4），只做 2 趟遍历（原来 4 趟），并跳过预乘/反预乘。</para>
        /// <para>图外按透明处理（与 CSS 一致），窗口阴影的形状边缘距离画布边缘至少有 range，两种边界处理在该处都已衰减到 0，因此没有可见差异。</para>
        /// <para><b>只适用于纯色阴影</b>（典型是黑色窗口阴影）：颜色不做模糊，所以彩色阴影会出现「形状清晰、只有透明度渐隐」的错误观感。
        /// 彩色/半透明场景请用 <see cref="BlurColor"/>（全通道）或 <see cref="BlurCSS"/>（预乘 alpha 空间）。</para>
        /// </summary>
        /// <param name="bmp">目标位图（就地修改 alpha，颜色不动）</param>
        /// <param name="range">模糊范围，语义与 <see cref="BlurColor"/> 相同（支撑直径，实际 σ ≈ range/2.31）</param>
        public static unsafe void BlurAlpha(Bitmap bmp, int range)
        {
            int h = range / 2;
            if (h < 1) return;

            int width = bmp.Width, height = bmp.Height;
            if (width < 1 || height < 1) return;

            int m = h + h + 1;// 窗宽，与 Blur 的 halfRange 一致
            long area = (long)m * m;
            if (area * 255 > int.MaxValue)
            {
                // 半径太大，32 位累加器装不下，退回四通道实现（正确优先）
                BlurColor(bmp, range);
                return;
            }
            uint weight = (uint)(((1UL << 32) + (ulong)(area / 2)) / (ulong)area);
            int pad = h + h;

            using (var unsafeBitmap = new UnsafeBitmap(bmp, true))
            {
                byte* pointer = (byte*)unsafeBitmap.Pointer + 3;// 直指 alpha 分量
                const int bpp = 4;
                int stride = width * bpp;

                #region 非透明包围盒（由外向内探测）

                // 阴影的形状总在中间、四周留白，从四条边往里探到第一个非空行/列即可，
                // 铺满时只需 2 行 2 列的检查（全扫则要 w×h 次）。结果与全扫完全一致：
                // by0..by1 覆盖了所有非空行，所以任何非空列在这个范围内一定有内容。
                int by0 = 0;
                while (by0 < height && BlurEmpty(pointer + (long)by0 * stride, width, bpp)) by0++;
                if (by0 >= height) return;// 全透明，无需处理

                int by1 = height - 1;
                while (by1 > by0 && BlurEmpty(pointer + (long)by1 * stride, width, bpp)) by1--;

                int rows = by1 - by0 + 1;
                byte* baseRow = pointer + (long)by0 * stride;

                int bx0 = 0;
                while (bx0 < width && BlurColEmpty(baseRow + (long)bx0 * bpp, rows, stride)) bx0++;

                int bx1 = width - 1;
                while (bx1 > bx0 && BlurColEmpty(baseRow + (long)bx1 * bpp, rows, stride)) bx1--;

                #endregion

                // 只有包围盒 ±pad 内的像素可能被影响，其余保持全透明
                int x0 = bx0 - pad; if (x0 < 0) x0 = 0;
                int x1 = bx1 + pad; if (x1 >= width) x1 = width - 1;
                int y0 = by0 - pad; if (y0 < 0) y0 = 0;
                int y1 = by1 + pad; if (y1 >= height) y1 = height - 1;

                int w = x1 - x0 + 1, hh = y1 - y0 + 1;

                // 这里不做大 σ 降采样：单通道模糊每像素只读写 1 字节、只有 2 趟遍历，
                // 已经比「块平均 + 双线性放大」两趟重采样还便宜（实测降采样后反而慢 15~40%）。
                // 降采样只对四通道的 BlurCSS 有意义，见 BlurCSSDown。
                var rings = new int[m * 2];// 两级延迟线

                fixed (int* ring = rings)
                {
                    // 横向：整行原地（写出位置永远落后读游标 pad 个），stride=4 顺序访问，缓存行被完整利用
                    for (int y = y0; y <= y1; y++)
                    {
                        byte* row = pointer + (long)y * stride + (long)x0 * bpp;
                        if (BlurEmpty(row, w, bpp)) continue;
                        BlurCascade(row, bpp, w, h, weight, ring);
                    }

                    // 纵向：直接原地（写出位置在上方 pad 行处，早已读过）。跨行访问只慢 30%，
                    // 但省掉 gather + scatter 两趟全量搬运，实测比「批量攒成紧凑缓冲」更快
                    int xs = x0;
#if NET6_0_OR_GREATER
                    // 级联是「串行」的（f0 依赖上一个 f0），单条列内没有 SIMD 空间；
                    // 唯一的并行轴是相邻列 —— 一次 4 列，27 条指令干完原来 48 条活
                    if (BlurSimd(h))
                    {
                        uint weight24 = BlurWeight24(m);
                        var rings4 = new int[m * 8];
                        fixed (int* ring4 = rings4)
                        {
                            byte* baseCol = pointer + (long)y0 * stride;
                            for (; xs + 3 <= x1; xs += 4)
                                BlurCascade4(baseCol + (long)xs * bpp, stride, hh, h, weight24, ring4);
                        }
                    }
#endif
                    for (; xs <= x1; xs++)
                    {
                        byte* col = pointer + (long)y0 * stride + (long)xs * bpp;
                        if (BlurColEmpty(col, hh, stride)) continue;
                        BlurCascade(col, stride, hh, h, weight, ring);
                    }
                }
            }
        }

        /// <summary>
        /// 两次盒模糊的单趟级联（只处理一个 8 位通道）
        /// <para>f0、f1 两级累加器 + 两条长度 = 窗宽 m 的环形延迟线：f0 = f0 + x[i] - x[i-m]，f1 = f1 + f0 - f0[i-m]，与 <see cref="BlurColor"/> 连续两次滑窗完全等价</para>
        /// <para>图外按下标越界视为透明；写出位置始终是 i-pad，永远落后读游标，所以能原地完成</para>
        /// </summary>
        /// <param name="src">行/列首地址（就地读写）</param>
        /// <param name="stride">元素步进（横向 4 字节，纵向 = 位图行跨距）</param>
        /// <param name="len">有效长度</param>
        /// <param name="h">盒半径</param>
        /// <param name="weight">定点权重 round(2³² / m²)</param>
        /// <param name="ring">环形延迟线（长度 = m×2）</param>
        static unsafe void BlurCascade(byte* src, int stride, int len, int h, uint weight, int* ring)
        {
            if (len <= 0) return;

            int m = h + h + 1;
            int pad = h + h;

            int* a0 = ring;
            int* a1 = a0 + m;
            int* e0 = a0 + m;
            int* e1 = a1 + m;
            int* p0 = a0;
            int* p1 = a1;

            // 延迟线必须从全零开始（等价于「图外全透明」），这块缓冲会被下一行/列复用
            for (int i = 0, n = m + m; i < n; i++) ring[i] = 0;

            int f0 = 0, f1 = 0;
            byte* s = src;
            byte* d = src;
            for (int i = -pad, to = len - 1 + pad; i <= to; i++)
            {
                int c = 0;
                if ((uint)i < (uint)len)
                {
                    c = *s;
                    s += stride;
                }

                f0 += c - *p0; *p0 = c;
                f1 += f0 - *p1; *p1 = f0;

                if (i >= pad)
                {
                    *d = (byte)(((ulong)(uint)f1 * weight + 0x80000000UL) >> 32);
                    d += stride;
                }

                p0++; if (p0 >= e0) p0 = a0;
                p1++; if (p1 >= e1) p1 = a1;
            }
        }

        /// <summary>整条线是否全透明</summary>
        static unsafe bool BlurEmpty(byte* p, int n, int stride)
        {
            for (int i = 0; i < n; i++, p += stride) if (*p != 0) return false;
            return true;
        }

        /// <summary>整列是否全透明（n 为列高）</summary>
        static unsafe bool BlurColEmpty(byte* p, int n, int stride)
        {
            for (int i = 0; i < n; i++, p += stride) if (*p != 0) return false;
            return true;
        }

#if NET6_0_OR_GREATER

        /// <summary>读取 16 字节（net6 没有 Vector128.Load/Store，用 Unsafe 直接读写，编译成同一条 movdqu）</summary>
        static unsafe Vector128<int> VLoad(int* p) => System.Runtime.CompilerServices.Unsafe.ReadUnaligned<Vector128<int>>(p);

        /// <summary>写入 16 字节</summary>
        static unsafe void VStore(int* p, Vector128<int> v) => System.Runtime.CompilerServices.Unsafe.WriteUnaligned(p, v);

        /// <summary>
        /// 纵向 SIMD 是否可用：需要 SSE4.1 的 32 位乘法，半径 ≥ 3（更小的核填不满流水线，打包反而更慢）、
        /// 且 ≤ 128（保证 2²⁴ 定点权重不失真）
        /// </summary>
        static bool BlurSimd(int h) => h >= 3 && h <= 128 && Sse41.IsSupported;

        /// <summary>
        /// 纵向级联的 4 列版本（SSE，4 × int32 通道）
        /// <para>级联内部 f0 = f0 + x[i] - x[i-m] 是串行依赖，列内无法并行；相邻 4 列互相独立，
        /// 正好塞满一个 128 位寄存器的 4 个 32 位通道，归一化用 2²⁴ 定点（乘积仍在 32 位内）</para>
        /// </summary>
        /// <param name="src">4 列中第一列的 alpha 首地址（就地读写）</param>
        /// <param name="stride">行跨距（字节）</param>
        /// <param name="len">列高</param>
        /// <param name="h">盒半径</param>
        /// <param name="weight24">定点权重 round(2²⁴ / m²)</param>
        /// <param name="ring">环形延迟线（长度 = m×8）</param>
        static unsafe void BlurCascade4(byte* src, int stride, int len, int h, uint weight24, int* ring)
        {
            int m = h + h + 1, pad = h + h;
            int* a0 = ring, a1 = ring + m * 4;
            int* e0 = a0 + m * 4, e1 = a1 + m * 4;
            int* p0 = a0, p1 = a1;

            for (int i = 0, n = m * 8; i < n; i++) ring[i] = 0;

            var f0 = Vector128<int>.Zero;
            var f1 = Vector128<int>.Zero;
            var zero = Vector128<int>.Zero;
            var wv = Vector128.Create(weight24);

            byte* s = src, d = src;
            for (int i = -pad, to = len - 1 + pad; i <= to; i++)
            {
                var c = zero;
                if ((uint)i < (uint)len)
                {
                    c = Vector128.Create(s[0], s[4], s[8], s[12]);
                    s += stride;
                }

                f0 = Sse2.Add(f0, Sse2.Subtract(c, VLoad(p0)));
                VStore(p0, c);
                f1 = Sse2.Add(f1, Sse2.Subtract(f0, VLoad(p1)));
                VStore(p1, f0);

                if (i >= pad)
                {
                    var v = Sse2.ShiftRightLogical(
                        Sse41.MultiplyLow(f1.AsUInt32(), wv), 24);
                    var p16 = Sse2.PackSignedSaturate(v.AsInt32(), v.AsInt32());
                    var p8 = Sse2.PackUnsignedSaturate(p16, p16);
                    uint pk = p8.AsUInt32().GetElement(0);
                    d[0] = (byte)pk;
                    d[4] = (byte)(pk >> 8);
                    d[8] = (byte)(pk >> 16);
                    d[12] = (byte)(pk >> 24);
                    d += stride;
                }

                p0 += 4; if (p0 >= e0) p0 = a0;
                p1 += 4; if (p1 >= e1) p1 = a1;
            }
        }

#endif

        #endregion

        #region CSS 模糊

        /// <summary>纵向一次处理的列数：4 列正好填满一个 128 位寄存器（4 × int32 通道）</summary>
        const int BlurBatch = 4;

        /// <summary>
        /// CSS 风格高斯模糊，对齐 <c>filter: blur(range px)</c>（默认单线程，结果确定）
        /// <para>range 即 CSS 中的模糊半径，等同于高斯标准差 σ（单位：像素）</para>
        /// <para>与 <see cref="BlurColor"/> 的区别：全程在「预乘 alpha」空间计算，透明像素（RGB=0）不会被算进颜色均值，因此不会发黑、不会出现黑边；图外视为透明，边缘自然淡出。</para>
        /// <para>算法：三次盒模糊逼近高斯，三级用「单趟级联」（三组嵌套累加器 + 三条环形延迟线，CIC 结构）一次遍历算完，与 Skia 的 CPU 模糊实现同构；横、纵各只遍历一次数据。</para>
        /// <para>性能：纵向 4 列一起走 SIMD（SSE4.1，一次解交错出 4 个通道向量），σ ≥ 8 再叠一层大 σ 降采样，1080p 一张 σ=16 的图约 30ms。</para>
        /// <para>旧 <see cref="BlurColor"/> 的 range 约为 2.45σ，迁移时取旧值 0.4 倍左右可得到相近的模糊强度。</para>
        /// <para><b>扩散范围</b>：三次盒模糊的支撑半径 = r1 + r2 + r3（半径由方差匹配反推，σ 大时 ≈ 3σ），
        /// 降采样路径再叠加块平均的半格和双线性的一格；准确保留空间请用 <see cref="BlurCSSRadius"/>。</para>
        /// </summary>
        /// <param name="bmp">目标位图（就地修改，统一按 32bppArgb 处理）</param>
        /// <param name="range">模糊半径 = 高斯标准差 σ（像素），已按 DPI 缩放后的像素值</param>
        public static void BlurCSS(Bitmap bmp, int range) => BlurCSS(bmp, range, false, true);

        /// <summary>
        /// CSS 风格高斯模糊，对齐 <c>filter: blur(range px)</c>
        /// </summary>
        /// <param name="bmp">目标位图（就地修改，统一按 32bppArgb 处理）</param>
        /// <param name="range">模糊半径 = 高斯标准差 σ（像素），已按 DPI 缩放后的像素值</param>
        /// <param name="parallel">是否对大区域启用多线程。默认 <c>false</c>：在绘制线程里调用更稳，不受线程池负载/嵌套并行影响；仅离屏大图（≥512×512）且不介意线程池时再开</param>
        /// <param name="downsample">σ ≥ 8 且区域够大时先「预乘 + 块平均缩小 → 小图模糊 → 双线性放大 + 反预乘」。
        /// 预乘/反预乘本来就有一趟读写，折进重采样后几乎零额外开销，而模糊量按 1/s² 下降；重采样只会让边缘再柔和 1% 左右</param>
        public static unsafe void BlurCSS(Bitmap bmp, int range, bool parallel, bool downsample = true)
        {
            if (range < 1) return;

            int width = bmp.Width, height = bmp.Height;
            if (width < 1 || height < 1) return;

            // 三次盒模糊的方差之和 = 高斯方差 σ²。累加器是 32 位，σ 过大时 255 × w1 × w2 × w3 会溢出，
            // 此时按「方差可叠加」把模糊拆成 n 组级联（每组仍是三次盒模糊，总方差不变、核形状更接近高斯）
            int passes, r1, r2, r3, pad;
            uint weight;
            BlurPlan(range, out passes, out r1, out r2, out r3, out weight, out pad);
            int expand = pad * passes;

            using (var unsafeBitmap = new UnsafeBitmap(bmp, true))
            {
                ColorBgra* pointer = unsafeBitmap.Pointer;

                #region 非透明包围盒（只扫 alpha，不写内存）

                bool opaque = true;
                int bx0 = width, bx1 = -1, by0 = height, by1 = -1;
                for (int y = 0; y < height; y++)
                {
                    byte* alpha = (byte*)pointer + ((long)y * width * 4) + 3;
                    int rowMin = -1, rowMax = -1;
                    for (int x = 0; x < width; x++, alpha += 4)
                    {
                        if (*alpha == 0) continue;
                        if (*alpha != 255) opaque = false;
                        if (rowMin < 0) rowMin = x;
                        rowMax = x;
                    }
                    if (rowMax >= 0)
                    {
                        if (rowMin < bx0) bx0 = rowMin;
                        if (rowMax > bx1) bx1 = rowMax;
                        if (by1 < 0) by0 = y;
                        by1 = y;
                    }
                }
                if (bx1 < 0) return;// 全透明，无需处理

                #endregion

                // 只有包围盒 + 模糊半径内的像素可能被影响，其余保持全透明，避免整图空跑
                int x0 = bx0 - expand; if (x0 < 0) x0 = 0;
                int x1 = bx1 + expand; if (x1 >= width) x1 = width - 1;
                int y0 = by0 - expand; if (y0 < 0) y0 = 0;
                int y1 = by1 + expand; if (y1 >= height) y1 = height - 1;

                int w = x1 - x0 + 1, h = y1 - y0 + 1;

                #region 大 σ 降采样

                // 预乘 + 块平均 → 小图三次盒模糊 → 双线性放大 + 反预乘。
                // 预乘/反预乘本来就要各扫一遍，正好折进「块平均」和「放大」，所以额外成本只是放大时的插值，
                // 而模糊量按 1/s² 下降（σ=32、s=4 时只模糊 1/16 的像素）
                int scale = downsample && !parallel ? (range >= 24 ? 4 : (range >= 8 ? 2 : 1)) : 1;
                if (scale > 1 && w >= 16 * scale && h >= 16 * scale && (long)w * h >= 16384)
                {
                    BlurCSSDown(pointer, width, x0, y0, w, h, range, scale);
                    return;
                }

                #endregion

                // 预乘 alpha：只在包围盒内做
                // 非预乘时透明像素的 RGB=0 会被算进颜色均值，模糊后发黑（CSS 在预乘空间计算，不会）
                if (!opaque)
                {
                    for (int y = by0; y <= by1; y++)
                    {
                        ColorBgra* row = pointer + y * width;
                        for (int x = bx0; x <= bx1; x++)
                        {
                            uint a = row[x].Alpha;
                            if (a == 0)
                            {
                                // 全透明像素的 RGB 可能是脏数据，清零保证「全 0 = 空」的判断成立
                                row[x].Bgra = 0;
                                continue;
                            }
                            if (a == 255) continue;
                            int alpha = (int)a;
                            row[x].Blue = MulDiv255(row[x].Blue, alpha);
                            row[x].Green = MulDiv255(row[x].Green, alpha);
                            row[x].Red = MulDiv255(row[x].Red, alpha);
                        }
                    }
                }

                // 大区域按行/列切块并行，每块独占一份 gather 缓冲 + 一份环形延迟线
                int parts = 1;
                if (parallel && (long)w * h >= 262144)
                {
                    parts = Environment.ProcessorCount;
                    if (parts > 8) parts = 8;
                    if (parts > h) parts = h;
                    if (parts < 1) parts = 1;
                }

                int ringLen = BlurRingSize(r1, r2, r3), boff = h * BlurBatch;
                var buffer = new ColorBgra[boff * parts];
                var rings = new int[ringLen * parts];
                fixed (ColorBgra* pin = buffer)
                fixed (int* rpin = rings)
                {
                    for (int k = 0; k < passes; k++)
                    {
                        // 只有最后一趟才反预乘（中间趟必须保持预乘，否则颜色会被重复放大）
                        bool unpremul = k == passes - 1;
                        if (parts == 1)
                        {
                            BlurCSSHorizontal(pointer, width, x0, w, y0, y1, r1, r2, r3, pad, weight, rpin);
                            BlurCSSVertical(pointer, width, x0, x1, y0, h, r1, r2, r3, pad, weight, unpremul, pin, rpin);
                        }
                        else
                        {
                            ColorBgra* ptr = pointer, tmp = pin;
                            int* rtmp = rpin;
                            int stride = width, xs0 = x0, xs1 = x1, ys0 = y0, ys1 = y1;
                            int rows = (h + parts - 1) / parts, cols = (w + parts - 1) / parts;
                            Parallel.For(0, parts, pi =>
                            {
                                int ys = ys0 + pi * rows, ye = ys + rows - 1;
                                if (ye > ys1) ye = ys1;
                                if (ys <= ye) BlurCSSHorizontal(ptr, stride, xs0, w, ys, ye, r1, r2, r3, pad, weight, rtmp + pi * ringLen);
                            });
                            Parallel.For(0, parts, pi =>
                            {
                                int xs = xs0 + pi * cols, xe = xs + cols - 1;
                                if (xe > xs1) xe = xs1;
                                if (xs <= xe) BlurCSSVertical(ptr, stride, xs, xe, ys0, h, r1, r2, r3, pad, weight, unpremul, tmp + pi * boff, rtmp + pi * ringLen);
                            });
                        }
                    }
                }
            }
        }

        /// <summary>块平均的定点倒数（下标 = 块内像素数，2²² 精度，误差 &lt; 0.01）</summary>
        static readonly int[] BlurRecip = CreateBlurRecip(1);

        /// <summary>预乘块平均的定点倒数（额外除以 255）</summary>
        static readonly int[] BlurRecip255 = CreateBlurRecip(255);

        static int[] CreateBlurRecip(int mul)
        {
            var table = new int[17];
            for (int i = 1; i <= 16; i++)
            {
                long d = (long)i * mul;
                table[i] = (int)(((1L << 22) + d / 2) / d);
            }
            return table;
        }

        /// <summary>2²⁴ 精度的定点权重（乘积仍在 32 位内，整段可用 SIMD 归一化）</summary>
        static uint BlurWeight24(int m)
        {
            long d = (long)m * m;
            return (uint)(((1UL << 24) + (ulong)(d / 2)) / (ulong)d);
        }

        /// <summary>2²⁴ 精度的定点权重（三级级联）</summary>
        static uint BlurWeight24(int r1, int r2, int r3)
        {
            long d = (long)(2 * r1 + 1) * (2 * r2 + 1) * (2 * r3 + 1);
            return (uint)(((1UL << 24) + (ulong)(d / 2)) / (ulong)d);
        }

        /// <summary>
        /// 降采样路径：预乘 + 块平均缩小 → 小图三次盒模糊 → 双线性放大 + 反预乘
        /// <para>小图在「预乘」空间里模糊，与全分辨率路径完全同构，因此不会发黑/黑边</para>
        /// </summary>
        static unsafe void BlurCSSDown(ColorBgra* pointer, int stride, int x0, int y0, int w, int hh, int sigma, int s)
        {
            int sw = (w + s - 1) / s, sh = (hh + s - 1) / s;
            var small = new ColorBgra[sw * sh];
            fixed (ColorBgra* sp = small)
            {
                #region 预乘 + 块平均

                for (int sy = 0; sy < sh; sy++)
                {
                    int fy0 = sy * s, fy1 = fy0 + s; if (fy1 > hh) fy1 = hh;
                    int cy = fy1 - fy0;
                    ColorBgra* dst = sp + sy * sw;
                    for (int sx = 0; sx < sw; sx++, dst++)
                    {
                        int fx0 = sx * s, fx1 = fx0 + s; if (fx1 > w) fx1 = w;
                        int cx = fx1 - fx0, cnt = cx * cy;
                        int sa = 0, sb = 0, sg = 0, sr = 0;
                        ColorBgra* row = pointer + (long)(y0 + fy0) * stride + (x0 + fx0);
                        for (int yy = 0; yy < cy; yy++, row += stride)
                        {
                            ColorBgra* q = row;
                            for (int xx = 0; xx < cx; xx++, q++)
                            {
                                int a = q->Alpha;
                                if (a == 0) continue;
                                sa += a;
                                sb += q->Blue * a;// ≤ 255×255×16 = 1.04M，乘倒数后仍 < 2³¹
                                sg += q->Green * a;
                                sr += q->Red * a;
                            }
                        }
                        if (sa == 0) { dst->Bgra = 0; continue; }
                        int inv = BlurRecip255[cnt];
                        dst->Alpha = (byte)((sa * BlurRecip[cnt]) >> 22);
                        dst->Blue = (byte)((sb * inv) >> 22);
                        dst->Green = (byte)((sg * inv) >> 22);
                        dst->Red = (byte)((sr * inv) >> 22);
                    }
                }

                #endregion

                // 小图上做同样的三次盒模糊（半径按比例缩小），结果仍是预乘
                int passes, r1, r2, r3, pad;
                uint weight;
                int ss = sigma / s;
                BlurPlan(ss < 1 ? 1 : ss, out passes, out r1, out r2, out r3, out weight, out pad);
                int ringLen = BlurRingSize(r1, r2, r3);
                var buffer = new ColorBgra[sh * BlurBatch];
                var rings = new int[ringLen];
                fixed (ColorBgra* bp = buffer)
                fixed (int* rp = rings)
                {
                    for (int k = 0; k < passes; k++)
                    {
                        BlurCSSHorizontal(sp, sw, 0, sw, 0, sh - 1, r1, r2, r3, pad, weight, rp);
                        BlurCSSVertical(sp, sw, 0, sw - 1, 0, sh, r1, r2, r3, pad, weight, false, bp, rp);
                    }
                }

                BlurCSSUp(pointer, stride, x0, y0, w, hh, sp, sw, sh, s);
            }
        }

        /// <summary>双线性放大回原尺寸，同时反预乘（还原成直通 alpha，与 GDI+ 的 32bppArgb 一致）</summary>
        static unsafe void BlurCSSUp(ColorBgra* pointer, int stride, int x0, int y0, int w, int hh, ColorBgra* sp, int sw, int sh, int s)
        {
            int step = 65536 / s;
            int off = ((s - 1) * 65536) / (2 * s);
            int lastx = sw - 1, lasty = sh - 1;
            for (int y = 0; y < hh; y++)
            {
                int fy = y * step - off;
                int iy0 = fy >> 16, wy = (fy >> 8) & 0xFF, iy1 = iy0 + 1;
                if (iy0 < 0) { iy0 = 0; wy = 0; }
                if (iy1 > lasty) iy1 = lasty;
                if (iy0 > iy1) iy0 = iy1;
                ColorBgra* r0 = sp + iy0 * sw, r1 = sp + iy1 * sw;
                ColorBgra* d = pointer + (long)(y0 + y) * stride + x0;
                int vy = 256 - wy, fx = -off;
                for (int x = 0; x < w; x++, d++, fx += step)
                {
                    int ix0 = fx >> 16, wx = (fx >> 8) & 0xFF, ix1 = ix0 + 1;
                    if (ix0 < 0) { ix0 = 0; wx = 0; }
                    if (ix1 > lastx) ix1 = lastx;
                    if (ix0 > lastx) ix0 = lastx;
                    int vx = 256 - wx;
                    ColorBgra c00 = r0[ix0], c01 = r0[ix1], c10 = r1[ix0], c11 = r1[ix1];
                    ColorBgra c = default;
                    c.Blue = (byte)(((c00.Blue * vx + c01.Blue * wx) * vy + (c10.Blue * vx + c11.Blue * wx) * wy + 32768) >> 16);
                    c.Green = (byte)(((c00.Green * vx + c01.Green * wx) * vy + (c10.Green * vx + c11.Green * wx) * wy + 32768) >> 16);
                    c.Red = (byte)(((c00.Red * vx + c01.Red * wx) * vy + (c10.Red * vx + c11.Red * wx) * wy + 32768) >> 16);
                    c.Alpha = (byte)(((c00.Alpha * vx + c01.Alpha * wx) * vy + (c10.Alpha * vx + c11.Alpha * wx) * wy + 32768) >> 16);
                    *d = BlurUnpremul(c);
                }
            }
        }

        /// <summary>横向：整行就地模糊（级联写在下标 i-pad 处，永远落在读游标后面，可原地完成）</summary>
        static unsafe void BlurCSSHorizontal(ColorBgra* p, int stride, int x0, int w, int ys, int ye, int r1, int r2, int r3, int pad, uint weight, int* ring)
        {
            for (int y = ys; y <= ye; y++)
            {
                ColorBgra* row = p + y * stride + x0;
                if (BlurIsEmpty(row, w)) continue;
                BlurCascade(row, w, r1, r2, r3, pad, weight, ring);
            }
        }

        /// <summary>
        /// 纵向：一次收 4 列成「像素交错」缓冲（buf[i×4 + k]，同一个 i 的 4 列在 16 字节内连续），
        /// 级联在缓冲内完成后再写回并反预乘
        /// <para>交错布局是为了让 4 列同时塞进一个 128 位寄存器：级联的 f0/f1/f2 都是串行依赖、
        /// 列内没有并行空间，唯一能并行的就是相邻列</para>
        /// </summary>
        static unsafe void BlurCSSVertical(ColorBgra* p, int stride, int xs, int xe, int y0, int h, int r1, int r2, int r3, int pad, uint weight, bool unpremul, ColorBgra* buf, int* ring)
        {
            int x = xs;
            while (x <= xe)
            {
                int n = xe - x + 1;
                if (n > BlurBatch) n = BlurBatch;

                ColorBgra* baseCol = p + y0 * stride + x;
                uint any = 0;
                for (int i = 0, o = 0; i < h; i++, o += stride)
                {
                    ColorBgra* s = baseCol + o;
                    ColorBgra* d = buf + i * BlurBatch;
                    for (int k = 0; k < n; k++)
                    {
                        ColorBgra color = s[k];
                        d[k] = color;
                        any |= color.Bgra;
                    }
                }

                if (any != 0)
                {
                    bool done = false;
#if NET6_0_OR_GREATER
                    if (n == BlurBatch && BlurCssSimd(r1, r2, r3))
                    {
                        BlurCascade4(buf, h, r1, r2, r3, pad, BlurWeight24(r1, r2, r3), ring);
                        done = true;
                    }
#endif
                    if (!done)
                    {
                        for (int k = 0; k < n; k++)
                            BlurCascadeStride(buf + k, BlurBatch, h, r1, r2, r3, pad, weight, ring);
                    }

                    if (unpremul)
                    {
                        // 还原为非预乘 alpha：模糊后边缘 alpha 会小于 255，不还原颜色就会偏暗
                        // （GDI+ 的 32bppArgb 是直通 alpha；alpha=255 的像素直接跳过）
                        for (int i = 0, o = 0; i < h; i++, o += stride)
                        {
                            ColorBgra* d = baseCol + o;
                            ColorBgra* s = buf + i * BlurBatch;
                            for (int k = 0; k < n; k++) d[k] = BlurUnpremul(s[k]);
                        }
                    }
                    else
                    {
                        for (int i = 0, o = 0; i < h; i++, o += stride)
                        {
                            ColorBgra* d = baseCol + o;
                            ColorBgra* s = buf + i * BlurBatch;
                            for (int k = 0; k < n; k++) d[k] = s[k];
                        }
                    }
                }

                x += n;
            }
        }

        /// <summary>
        /// 交错布局下的标量级联（步进 <paramref name="stride"/> 个像素，SIMD 不可用时或尾部不足 4 列时使用）
        /// </summary>
        static unsafe void BlurCascadeStride(ColorBgra* buf, int stride, int len, int r1, int r2, int r3, int pad, uint weight, int* ring)
        {
            int m0 = r1 + r1 + 1, m1 = r2 + r2 + 1, m2 = r3 + r3 + 1;

            int* a0 = ring;
            int* a1 = a0 + m0 * 4;
            int* a2 = a1 + m1 * 4;
            int* e0 = a0 + m0 * 4;
            int* e1 = a1 + m1 * 4;
            int* e2 = a2 + m2 * 4;

            for (int i = 0, n = (m0 + m1 + m2) * 4; i < n; i++) ring[i] = 0;

            int* p0 = a0;
            int* p1 = a1;
            int* p2 = a2;

            int f0b = 0, f0g = 0, f0r = 0, f0a = 0;
            int f1b = 0, f1g = 0, f1r = 0, f1a = 0;
            int f2b = 0, f2g = 0, f2r = 0, f2a = 0;

            for (int i = -pad, to = len - 1 + pad; i <= to; i++)
            {
                int cb = 0, cg = 0, cr = 0, ca = 0;
                if ((uint)i < (uint)len)
                {
                    ColorBgra color = buf[(long)i * stride];
                    cb = color.Blue; cg = color.Green; cr = color.Red; ca = color.Alpha;
                }

                f0b += cb - p0[0]; f0g += cg - p0[1]; f0r += cr - p0[2]; f0a += ca - p0[3];
                p0[0] = cb; p0[1] = cg; p0[2] = cr; p0[3] = ca;
                f1b += f0b - p1[0]; f1g += f0g - p1[1]; f1r += f0r - p1[2]; f1a += f0a - p1[3];
                p1[0] = f0b; p1[1] = f0g; p1[2] = f0r; p1[3] = f0a;
                f2b += f1b - p2[0]; f2g += f1g - p2[1]; f2r += f1r - p2[2]; f2a += f1a - p2[3];
                p2[0] = f1b; p2[1] = f1g; p2[2] = f1r; p2[3] = f1a;

                int o = i - pad;
                if (o >= 0) buf[(long)o * stride].Bgra = BlurPackFixed(f2b, f2g, f2r, f2a, weight);

                p0 += 4; if (p0 >= e0) p0 = a0;
                p1 += 4; if (p1 >= e1) p1 = a1;
                p2 += 4; if (p2 >= e2) p2 = a2;
            }
        }

        /// <summary>
        /// 三次盒模糊的「单趟级联」：三组嵌套累加器（CIC 结构）+ 三条环形延迟线，只遍历一次数据
        /// <para>数学上等价于连续做三次 box(2r+1) 滑窗：f0 = f0 + x[i] - x[i-w1]，f1 = f1 + f0 - f0[i-w2]，f2 同理</para>
        /// <para>图外按下标越界即视为透明，边缘自然淡出；写出位置在下标 i-pad 处、永远落后于读游标，可原地模糊</para>
        /// <para>与 Skia 的 SkMaskBlurFilter::PlanGauss::Scan::blur 同构（它同样用 uint32 累加 + 三次累加 + 三条环形缓冲）</para>
        /// <para>实测把三段分开写（头部喂 0 / 主循环 / 尾部喂 0）反而更慢：合并成一段后循环体小、读写落在同一批缓存行，快约 10%</para>
        /// </summary>
        /// <param name="buf">行/列缓冲（就地读写）</param>
        /// <param name="len">有效长度</param>
        /// <param name="r1">第一级半径</param>
        /// <param name="r2">第二级半径</param>
        /// <param name="r3">第三级半径</param>
        /// <param name="pad">总延迟半径 r1+r2+r3</param>
        /// <param name="weight">定点权重 round(2³² / (w1·w2·w3))</param>
        /// <param name="ring">环形延迟线（长度 = BlurRingSize）</param>
        static unsafe void BlurCascade(ColorBgra* buf, int len, int r1, int r2, int r3, int pad, uint weight, int* ring)
        {
            // 延迟线长度必须等于该级窗宽 w = 2r+1（读-减-写-前移的顺序让窗口覆盖 [i-w+1, i]）
            int m0 = r1 + r1 + 1, m1 = r2 + r2 + 1, m2 = r3 + r3 + 1;

            int* a0 = ring;
            int* a1 = a0 + m0 * 4;
            int* a2 = a1 + m1 * 4;
            int* e0 = a0 + m0 * 4;
            int* e1 = a1 + m1 * 4;
            int* e2 = a2 + m2 * 4;

            for (int i = 0, n = (m0 + m1 + m2) * 4; i < n; i++) ring[i] = 0;

            int* p0 = a0;
            int* p1 = a1;
            int* p2 = a2;

            int f0b = 0, f0g = 0, f0r = 0, f0a = 0;
            int f1b = 0, f1g = 0, f1r = 0, f1a = 0;
            int f2b = 0, f2g = 0, f2r = 0, f2a = 0;

            for (int i = -pad, to = len - 1 + pad; i <= to; i++)
            {
                int cb = 0, cg = 0, cr = 0, ca = 0;
                if ((uint)i < (uint)len)
                {
                    ColorBgra color = buf[i];
                    cb = color.Blue; cg = color.Green; cr = color.Red; ca = color.Alpha;
                }

                f0b += cb - p0[0]; f0g += cg - p0[1]; f0r += cr - p0[2]; f0a += ca - p0[3];
                p0[0] = cb; p0[1] = cg; p0[2] = cr; p0[3] = ca;
                f1b += f0b - p1[0]; f1g += f0g - p1[1]; f1r += f0r - p1[2]; f1a += f0a - p1[3];
                p1[0] = f0b; p1[1] = f0g; p1[2] = f0r; p1[3] = f0a;
                f2b += f1b - p2[0]; f2g += f1g - p2[1]; f2r += f1r - p2[2]; f2a += f1a - p2[3];
                p2[0] = f1b; p2[1] = f1g; p2[2] = f1r; p2[3] = f1a;

                int o = i - pad;
                if (o >= 0) buf[o].Bgra = BlurPackFixed(f2b, f2g, f2r, f2a, weight);

                p0 += 4; if (p0 >= e0) p0 = a0;
                p1 += 4; if (p1 >= e1) p1 = a1;
                p2 += 4; if (p2 >= e2) p2 = a2;
            }
        }

#if NET6_0_OR_GREATER

        /// <summary>
        /// 四通道纵向级联能否走 SIMD：需要 SSE4.1 的 32 位乘法，且 d = w1·w2·w3 ≤ 2¹⁶
        /// （此时 2²⁴ 权重 ≥ 256，定点误差 &lt; 0.5/255；更大的核退回 2³² 标量，保证精度）
        /// </summary>
        static bool BlurCssSimd(int r1, int r2, int r3) => Sse41.IsSupported && (long)(2 * r1 + 1) * (2 * r2 + 1) * (2 * r3 + 1) <= 65536;

        /// <summary>
        /// 4 列合一的纵向级联（SSE，每列一组 4×int32 通道）
        /// <para>缓冲是像素交错布局（buf[i×4 + k]），一个下标 i 的 4 列正好 16 字节，
        /// 一次加载就拿到 4 列像素，解包成 B/G/R/A 四个 32 位通道向量后各自跑三级累加，
        /// 最后打包写回。指令数约为标量的 1/3（实测整趟纵向快 3 倍以上）</para>
        /// </summary>
        /// <param name="buf">交错布局的列缓冲（就地读写）</param>
        /// <param name="len">列高</param>
        /// <param name="r1">第一级半径</param>
        /// <param name="r2">第二级半径</param>
        /// <param name="r3">第三级半径</param>
        /// <param name="pad">总延迟半径</param>
        /// <param name="weight24">定点权重 round(2²⁴ / d)</param>
        /// <param name="ring">环形延迟线（长度 = BlurRingSize）</param>
        static unsafe void BlurCascade4(ColorBgra* buf, int len, int r1, int r2, int r3, int pad, uint weight24, int* ring)
        {
            int m0 = r1 + r1 + 1, m1 = r2 + r2 + 1, m2 = r3 + r3 + 1;

            int* a0 = ring;
            int* a1 = a0 + m0 * 16;
            int* a2 = a1 + m1 * 16;
            int* e0 = a0 + m0 * 16;
            int* e1 = a1 + m1 * 16;
            int* e2 = a2 + m2 * 16;

            for (int i = 0, n = (m0 + m1 + m2) * 16; i < n; i++) ring[i] = 0;

            int* p0 = a0;
            int* p1 = a1;
            int* p2 = a2;

            var mask = Vector128.Create(255);
            var round = Vector128.Create(1 << 23);
            var wv = Vector128.Create(weight24);
            var zero = Vector128<int>.Zero;

            var f0b = zero; var f0g = zero; var f0r = zero; var f0a = zero;
            var f1b = zero; var f1g = zero; var f1r = zero; var f1a = zero;
            var f2b = zero; var f2g = zero; var f2r = zero; var f2a = zero;

            for (int i = -pad, to = len - 1 + pad; i <= to; i++)
            {
                var cb = zero; var cg = zero; var cr = zero; var ca = zero;
                if ((uint)i < (uint)len)
                {
                    // 4 列的 BGRA 连续排在一起：一次 16 字节加载 + 4 次移位掩码即完成「解交错」
                    var px = VLoad((int*)(buf + i * 4));
                    cb = Sse2.And(px, mask);
                    cg = Sse2.And(Sse2.ShiftRightLogical(px, 8), mask);
                    cr = Sse2.And(Sse2.ShiftRightLogical(px, 16), mask);
                    ca = Sse2.ShiftRightLogical(px, 24);
                }

                f0b = Sse2.Add(f0b, Sse2.Subtract(cb, VLoad(p0))); VStore(p0, cb);
                f0g = Sse2.Add(f0g, Sse2.Subtract(cg, VLoad(p0 + 4))); VStore(p0 + 4, cg);
                f0r = Sse2.Add(f0r, Sse2.Subtract(cr, VLoad(p0 + 8))); VStore(p0 + 8, cr);
                f0a = Sse2.Add(f0a, Sse2.Subtract(ca, VLoad(p0 + 12))); VStore(p0 + 12, ca);
                f1b = Sse2.Add(f1b, Sse2.Subtract(f0b, VLoad(p1))); VStore(p1, f0b);
                f1g = Sse2.Add(f1g, Sse2.Subtract(f0g, VLoad(p1 + 4))); VStore(p1 + 4, f0g);
                f1r = Sse2.Add(f1r, Sse2.Subtract(f0r, VLoad(p1 + 8))); VStore(p1 + 8, f0r);
                f1a = Sse2.Add(f1a, Sse2.Subtract(f0a, VLoad(p1 + 12))); VStore(p1 + 12, f0a);
                f2b = Sse2.Add(f2b, Sse2.Subtract(f1b, VLoad(p2))); VStore(p2, f1b);
                f2g = Sse2.Add(f2g, Sse2.Subtract(f1g, VLoad(p2 + 4))); VStore(p2 + 4, f1g);
                f2r = Sse2.Add(f2r, Sse2.Subtract(f1r, VLoad(p2 + 8))); VStore(p2 + 8, f1r);
                f2a = Sse2.Add(f2a, Sse2.Subtract(f1a, VLoad(p2 + 12))); VStore(p2 + 12, f1a);

                int o = i - pad;
                if (o >= 0)
                {
                    var vb = Sse2.ShiftRightLogical(Sse2.Add(Sse41.MultiplyLow(f2b.AsUInt32(), wv), round.AsUInt32()), 24);
                    var vg = Sse2.ShiftRightLogical(Sse2.Add(Sse41.MultiplyLow(f2g.AsUInt32(), wv), round.AsUInt32()), 24);
                    var vr = Sse2.ShiftRightLogical(Sse2.Add(Sse41.MultiplyLow(f2r.AsUInt32(), wv), round.AsUInt32()), 24);
                    var va = Sse2.ShiftRightLogical(Sse2.Add(Sse41.MultiplyLow(f2a.AsUInt32(), wv), round.AsUInt32()), 24);
                    var px = Sse2.Or(Sse2.Or(vb.AsInt32(), Sse2.ShiftLeftLogical(vg.AsInt32(), 8)),
                        Sse2.Or(Sse2.ShiftLeftLogical(vr.AsInt32(), 16), Sse2.ShiftLeftLogical(va.AsInt32(), 24)));
                    VStore((int*)(buf + o * 4), px);
                }

                p0 += 16; if (p0 >= e0) p0 = a0;
                p1 += 16; if (p1 >= e1) p1 = a1;
                p2 += 16; if (p2 >= e2) p2 = a2;
            }
        }

#endif

        /// <summary>定窗口求均值：一次 64 位乘法（weight = round(2³²/d)）代替三次除法</summary>
        static uint BlurPackFixed(int b, int g, int r, int a, uint m) => (uint)((((ulong)(uint)b * m + 0x80000000UL) >> 32))
            | ((uint)(((ulong)(uint)g * m + 0x80000000UL) >> 32) << 8)
            | ((uint)(((ulong)(uint)r * m + 0x80000000UL) >> 32) << 16)
            | ((uint)(((ulong)(uint)a * m + 0x80000000UL) >> 32) << 24);

        /// <summary>
        /// 三次盒模糊的规划：半径、定点权重、总延迟
        /// <para>σ 过大时 255 × w1 × w2 × w3 会溢出 32 位累加器，按「方差可叠加」拆成多趟（每趟仍是三次盒模糊）</para>
        /// </summary>
        static void BlurPlan(int sigma, out int passes, out int r1, out int r2, out int r3, out uint weight, out int pad)
        {
            passes = 1;
            BlurRadii(sigma, out r1, out r2, out r3);
            while (!BlurSafe(r1, r2, r3))
            {
                passes++;
                BlurRadii((int)Math.Round(sigma / Math.Sqrt(passes)), out r1, out r2, out r3);
            }
            // 半径为 0 的级是恒等变换，级联会退化；抬到 1（σ≈1 时等效约 1.41，肉眼无法区分）
            if (r1 < 1) r1 = 1;
            if (r2 < 1) r2 = 1;
            if (r3 < 1) r3 = 1;
            weight = BlurWeight(r1, r2, r3);
            pad = r1 + r2 + r3;
        }

        /// <summary>环形延迟线所需长度（三级延迟 × 4 列 × 4 通道，SIMD 与标量都够用）</summary>
        static int BlurRingSize(int r1, int r2, int r3) => ((r1 + r1 + 1) + (r2 + r2 + 1) + (r3 + r3 + 1)) * 16;

        /// <summary>预乘 alpha 还原为直通 alpha</summary>
        static ColorBgra BlurUnpremul(ColorBgra c)
        {
            uint a = c.Alpha;
            if (a == 255) return c;
            if (a == 0)
            {
                c.Bgra = 0;
                return c;
            }
            c.Blue = UnmulDiv255(c.Blue, a);
            c.Green = UnmulDiv255(c.Green, a);
            c.Red = UnmulDiv255(c.Red, a);
            return c;
        }

        /// <summary>
        /// 计算三次盒模糊的半径，使三次盒模糊的方差之和等于高斯方差 σ²
        /// <para>单次宽度 w 的盒模糊方差为 (w²-1)/12</para>
        /// </summary>
        static void BlurRadii(int sigma, out int r1, out int r2, out int r3)
        {
            // 理想宽度：3 次盒模糊时 w = sqrt(12σ²/3 + 1) = sqrt(4σ² + 1)，取奇数
            int wl = (int)Math.Floor(Math.Sqrt(4.0 * sigma * sigma + 1));
            if ((wl & 1) == 0) wl--;
            if (wl < 1) wl = 1;
            int wu = wl + 2;

            // 在「m 个窄核 + (3-m) 个宽核」中挑方差最接近 σ² 的组合
            double target = 12.0 * sigma * sigma;
            int best = 0;
            double bestErr = double.MaxValue;
            for (int m = 0; m <= 3; m++)
            {
                double v = m * (wl * wl - 1) + (3 - m) * (wu * wu - 1);
                double err = Math.Abs(v - target);
                if (err < bestErr)
                {
                    bestErr = err;
                    best = m;
                }
            }

            int nl = (wl - 1) / 2, nu = (wu - 1) / 2;
            r1 = best > 0 ? nl : nu;
            r2 = best > 1 ? nl : nu;
            r3 = best > 2 ? nl : nu;
        }

        /// <summary>32 位累加器是否够用：最大中间值 255 × w1 × w2 × w3（溢出就拆成多趟级联）</summary>
        static bool BlurSafe(int r1, int r2, int r3) => 255L * (2 * r1 + 1) * (2 * r2 + 1) * (2 * r3 + 1) <= int.MaxValue;

        /// <summary>定点权重 round(2³² / (w1·w2·w3))</summary>
        static uint BlurWeight(int r1, int r2, int r3)
        {
            long d = (long)(2 * r1 + 1) * (2 * r2 + 1) * (2 * r3 + 1);
            return (uint)(((1UL << 32) + (ulong)(d / 2)) / (ulong)d);
        }

        /// <summary>整条线是否全透明（预乘后全 0 即全透明）</summary>
        static unsafe bool BlurIsEmpty(ColorBgra* p, int n)
        {
            for (int i = 0; i < n; i++) if (p[i].Bgra != 0) return false;
            return true;
        }

        /// <summary>c * a / 255（四舍五入）</summary>
        static byte MulDiv255(byte c, int a)
        {
            int t = c * a + 128;
            return (byte)((t + (t >> 8)) >> 8);
        }

        /// <summary>反预乘查表：round(255 * 65536 / a)，避免逐像素整除</summary>
        static readonly uint[] UnmulMagic = CreateUnmulMagic();

        static uint[] CreateUnmulMagic()
        {
            var table = new uint[256];
            for (int a = 1; a < 256; a++) table[a] = (uint)((((ulong)255 << 16) + (uint)(a >> 1)) / (uint)a);
            return table;
        }

        /// <summary>c * 255 / a（查表 + 定点乘法）</summary>
        static byte UnmulDiv255(byte c, uint a)
        {
            uint v = (uint)((((ulong)c * UnmulMagic[a]) + 32768) >> 16);
            return v > 255 ? (byte)255 : (byte)v;
        }

        #endregion

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

        /// <summary>
        /// 将图片转换为指定大小的点阵矩阵
        /// </summary>
        /// <param name="bmp">图片</param>
        /// <param name="horizontalMode">是否横向取模（true=横向，false=纵向）</param>
        /// <returns>点阵数据（每个字节表示8个像素）</returns>
        public static byte[] ConvertImageToDotMatrix(Bitmap bmp, bool horizontalMode = true)
        {
            // 转换为点阵
            int width = bmp.Width, height = bmp.Height;
            byte[] dotMatrix = new byte[(width * height + 7) / 8];
            int index = 0;
            using (var unsafeBitmap = new UnsafeBitmap(bmp, false))
            {
                if (horizontalMode)
                {
                    // 横向取模：按行扫描（宽度方向）
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            var pixelColor = unsafeBitmap.GetPixel(x, y);
                            bool isBlack = (pixelColor.Red < 128 && pixelColor.Green < 128 && pixelColor.Blue < 128);
                            if (isBlack) dotMatrix[index / 8] |= (byte)(1 << (7 - (index % 8)));// 将像素添加到点阵（高位在前）
                            index++;
                        }
                    }
                }
                else
                {
                    // 纵向取模：按列扫描（高度方向）
                    for (int x = 0; x < width; x++)
                    {
                        for (int y = 0; y < height; y++)
                        {
                            var pixelColor = unsafeBitmap.GetPixel(x, y);
                            bool isBlack = (pixelColor.Red < 128 && pixelColor.Green < 128 && pixelColor.Blue < 128);
                            if (isBlack) dotMatrix[index / 8] |= (byte)(1 << (7 - (index % 8)));// 将像素添加到点阵（高位在前）
                            index++;
                        }
                    }
                }
            }
            return dotMatrix;
        }

        /// <summary>
        /// GDI+原生实现：真正的沿路径走向渐变（多色）
        /// </summary>
        public static void DrawNativePathGradient(this Canvas g, GraphicsPath path, float lineWidth, Color[] colors, float[] positions)
        {
            // 步骤1：将路径扁平化，分解为连续的小线段（精度0.05f，几乎无锯齿）
            using (var flatPath = (GraphicsPath)path.Clone())
            {
                flatPath.Flatten(new Matrix(), 0.05f);
                var flatPoints = flatPath.PathPoints;
                int segmentCount = flatPoints.Length - 1;

                if (segmentCount < 1) return;

                // 步骤3：预计算所有线段的累积长度和总长度
                var cumulativeLengths = new float[segmentCount + 1];
                float totalLength = 0;
                cumulativeLengths[0] = 0;

                for (int i = 0; i < segmentCount; i++)
                {
                    float dx = flatPoints[i + 1].X - flatPoints[i].X, dy = flatPoints[i + 1].Y - flatPoints[i].Y, length = (float)Math.Sqrt(dx * dx + dy * dy);
                    totalLength += length;
                    cumulativeLengths[i + 1] = totalLength;
                }

                // 步骤4：逐段绘制，每段使用与切线方向一致的渐变画笔
                for (int i = 0; i < segmentCount; i++)
                {
                    PointF p1 = flatPoints[i], p2 = flatPoints[i + 1];
                    if (p1 == p2) continue;

                    // 计算当前段的起点和终点颜色（基于路径总长度的百分比）
                    float t1 = cumulativeLengths[i] / totalLength, t2 = cumulativeLengths[i + 1] / totalLength;
                    Color c1 = InterpolateColor(colors, positions, t1), c2 = InterpolateColor(colors, positions, t2);

                    // 创建与当前线段方向完全一致的线性渐变画刷
                    using (var brush = new LinearGradientBrush(p1, p2, c1, c2))
                    using (var pen = new Pen(brush, lineWidth))
                    {
                        pen.StartCap = pen.EndCap = LineCap.Round;
                        g.DrawLine(pen, p1, p2);
                    }
                }
            }
        }

        /// <summary>
        /// 精确颜色插值：根据位置百分比从渐变中获取颜色
        /// </summary>
        static Color InterpolateColor(Color[] colors, float[] positions, float t)
        {
            if (t <= 0.0f) return colors[0];
            if (t >= 1.0f) return colors[colors.Length - 1];

            // 找到t所在的颜色区间
            int index = 0;
            for (int i = 0; i < positions.Length - 1; i++)
            {
                if (t >= positions[i] && t <= positions[i + 1])
                {
                    index = i;
                    break;
                }
            }

            // 计算区间内的相对位置
            float range = positions[index + 1] - positions[index], localT = range == 0 ? 0 : (t - positions[index]) / range;

            // ARGB四通道分别插值
            Color c1 = colors[index], c2 = colors[index + 1];

            return Color.FromArgb(
                (int)Math.Round(c1.A + (c2.A - c1.A) * localT),
                (int)Math.Round(c1.R + (c2.R - c1.R) * localT),
                (int)Math.Round(c1.G + (c2.G - c1.G) * localT),
                (int)Math.Round(c1.B + (c2.B - c1.B) * localT)
            );
        }
    }
}