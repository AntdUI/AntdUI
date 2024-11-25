﻿// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace AntdUI
{
    partial class Helper
    {
        #region 文本布局

        /// <summary>
        /// 文本布局
        /// </summary>
        /// <param name="tb">垂直（上下）</param>
        /// <param name="lr">水平（前后）</param>
        public static StringFormat SF(StringAlignment tb = StringAlignment.Center, StringAlignment lr = StringAlignment.Center)
        {
            return new StringFormat(StringFormat.GenericTypographic) { LineAlignment = tb, Alignment = lr };
        }

        /// <summary>
        /// 文本布局（不换行）
        /// </summary>
        /// <param name="tb">垂直（上下）</param>
        /// <param name="lr">水平（前后）</param>
        public static StringFormat SF_NoWrap(StringAlignment tb = StringAlignment.Center, StringAlignment lr = StringAlignment.Center)
        {
            var sf = new StringFormat(StringFormat.GenericTypographic) { LineAlignment = tb, Alignment = lr };
            sf.FormatFlags |= StringFormatFlags.NoWrap;
            return sf;
        }

        /// <summary>
        /// 文本布局（超出省略号）
        /// </summary>
        /// <param name="tb">垂直（上下）</param>
        /// <param name="lr">水平（前后）</param>
        public static StringFormat SF_Ellipsis(StringAlignment tb = StringAlignment.Center, StringAlignment lr = StringAlignment.Center)
        {
            return new StringFormat(StringFormat.GenericTypographic) { LineAlignment = tb, Alignment = lr, Trimming = StringTrimming.EllipsisCharacter };
        }

        /// <summary>
        /// 文本布局（超出省略号+不换行）
        /// </summary>
        /// <param name="tb">垂直（上下）</param>
        /// <param name="lr">水平（前后）</param>
        public static StringFormat SF_ALL(StringAlignment tb = StringAlignment.Center, StringAlignment lr = StringAlignment.Center)
        {
            var sf = new StringFormat(StringFormat.GenericTypographic) { LineAlignment = tb, Alignment = lr, Trimming = StringTrimming.EllipsisCharacter };
            sf.FormatFlags |= StringFormatFlags.NoWrap;
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
                var arr = code.Split(',');
                if (arr.Length > 1)
                {
                    if (arr.Length > 2 && float.TryParse(arr[0], out float deg)) return new LinearGradientBrush(rect, arr[1].Trim().ToColor(), arr[2].Trim().ToColor(), 270 + deg);
                    else return new LinearGradientBrush(rect, arr[0].Trim().ToColor(), arr[1].Trim().ToColor(), 270F);
                }
            }
            return new SolidBrush(def);
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

        public static GraphicsPath RoundPath(this Rectangle rect, float radius)
        {
            return RoundPathCore(rect, radius);
        }

        public static GraphicsPath RoundPath(this RectangleF rect, float radius)
        {
            return RoundPathCore(rect, radius);
        }

        internal static GraphicsPath RoundPath(this RectangleF rect, float radius, TShape shape)
        {
            return RoundPath(rect, radius, shape == TShape.Round);
        }

        internal static GraphicsPath RoundPath(this Rectangle rect, float radius, bool round)
        {
            if (round) return CapsulePathCore(rect);
            return RoundPathCore(rect, radius);
        }

        internal static GraphicsPath RoundPath(this RectangleF rect, float radius, bool round)
        {
            if (round) return CapsulePathCore(rect);
            return RoundPathCore(rect, radius);
        }

        internal static GraphicsPath RoundPath(this Rectangle rect, float radius, TAlignMini shadowAlign)
        {
            switch (shadowAlign)
            {
                case TAlignMini.Top: return RoundPath(rect, radius, true, true, false, false);
                case TAlignMini.Bottom: return RoundPath(rect, radius, false, false, true, true);
                case TAlignMini.Left: return RoundPath(rect, radius, true, false, false, true);
                case TAlignMini.Right: return RoundPath(rect, radius, false, true, true, false);
                case TAlignMini.None:
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
                else
                {
                    // Circle
                    path.AddEllipse(rect);
                }
            }
            else path.AddEllipse(rect);
            path.CloseFigure();
        }

        #endregion

        #region 图标渲染

        internal static void PaintIcons(this Canvas g, TType icon, Rectangle rect)
        {
            switch (icon)
            {
                case TType.Success:
                    using (var bmp = SvgExtend.GetImgExtend(SvgDb.IcoSuccess, rect, Style.Db.Success))
                    {
                        if (bmp == null) return;
                        g.Image(bmp, rect);
                    }
                    break;
                case TType.Info:
                    using (var bmp = SvgExtend.GetImgExtend(SvgDb.IcoInfo, rect, Style.Db.Info))
                    {
                        if (bmp == null) return;
                        g.Image(bmp, rect);
                    }
                    break;
                case TType.Warn:
                    using (var bmp = SvgExtend.GetImgExtend(SvgDb.IcoWarn, rect, Style.Db.Warning))
                    {
                        if (bmp == null) return;
                        g.Image(bmp, rect);
                    }
                    break;
                case TType.Error:
                    using (var bmp = SvgExtend.GetImgExtend(SvgDb.IcoError, rect, Style.Db.Error))
                    {
                        if (bmp == null) return;
                        g.Image(bmp, rect);
                    }
                    break;
            }
        }
        internal static void PaintIcons(this Canvas g, TType icon, Rectangle rect, Color back)
        {
            using (var brush = new SolidBrush(back))
            {
                g.FillEllipse(brush, new Rectangle(rect.X + 1, rect.Y + 1, rect.Width - 2, rect.Height - 2));
            }
            switch (icon)
            {
                case TType.Success:
                    using (var bmp = SvgExtend.GetImgExtend(SvgDb.IcoSuccess, rect, Style.Db.Success))
                    {
                        if (bmp == null) return;
                        g.Image(bmp, rect);
                    }
                    break;
                case TType.Info:
                    using (var bmp = SvgExtend.GetImgExtend(SvgDb.IcoInfo, rect, Style.Db.Info))
                    {
                        if (bmp == null) return;
                        g.Image(bmp, rect);
                    }
                    break;
                case TType.Warn:
                    using (var bmp = SvgExtend.GetImgExtend(SvgDb.IcoWarn, rect, Style.Db.Warning))
                    {
                        if (bmp == null) return;
                        g.Image(bmp, rect);
                    }
                    break;
                case TType.Error:
                    using (var bmp = SvgExtend.GetImgExtend(SvgDb.IcoError, rect, Style.Db.Error))
                    {
                        if (bmp == null) return;
                        g.Image(bmp, rect);
                    }
                    break;
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

        public static void PaintBadge(this IControl control, Canvas g)
        {
            if (control.BadgeSvg != null)
            {
                int hasx = (int)(control.BadgeOffsetX * Config.Dpi), hasy = (int)(control.BadgeOffsetY * Config.Dpi);
                using (var font = new Font(control.Font.FontFamily, control.Font.Size * control.BadgeSize))
                {
                    var size_badge = g.MeasureString(Config.NullText, font).Height;
                    var rect_badge = PaintBadge(control.ClientRectangle, control.BadgeAlign, hasx, hasy, size_badge, size_badge);
                    g.GetImgExtend(control.BadgeSvg, rect_badge, control.BadgeBack ?? Style.Db.Error);
                }
            }
            else if (control.Badge != null)
            {
                var color = control.BadgeBack ?? Style.Db.Error;
                var rect = control.ClientRectangle;
                float borsize = Config.Dpi;
                using (var font = new Font(control.Font.FontFamily, control.Font.Size * control.BadgeSize))
                {
                    int hasx = (int)(control.BadgeOffsetX * Config.Dpi), hasy = (int)(control.BadgeOffsetY * Config.Dpi);
                    if (string.IsNullOrWhiteSpace(control.Badge))
                    {
                        var size = g.MeasureString(Config.NullText, font).Height;
                        var rect_badge = PaintBadge(rect, control.BadgeAlign, hasx, hasy, size, size);
                        using (var brush = new SolidBrush(color))
                        {
                            if (control.BadgeMode)
                            {
                                float b2 = borsize * 2, rr = size * 0.2F, rr2 = rr * 2;
                                g.FillEllipse(Style.Db.ErrorColor, new RectangleF(rect_badge.X - borsize, rect_badge.Y - borsize, rect_badge.Width + b2, rect_badge.Height + b2));
                                using (var path = rect_badge.RoundPath(1, true))
                                {
                                    path.AddEllipse(new RectangleF(rect_badge.X + rr, rect_badge.Y + rr, rect_badge.Width - rr2, rect_badge.Height - rr2));
                                    g.Fill(color, path);
                                }
                            }
                            else
                            {
                                g.FillEllipse(color, rect_badge);
                                g.DrawEllipse(Style.Db.ErrorColor, borsize, rect_badge);
                            }
                        }
                    }
                    else
                    {
                        var size = g.MeasureString(control.Badge, font);
                        using (var s_f = SF_NoWrap())
                        {
                            int size_badge = (int)(size.Height * 1.2F);
                            if (size.Height > size.Width)
                            {
                                var rect_badge = PaintBadge(rect, control.BadgeAlign, hasx, hasy, size_badge, size_badge);
                                g.FillEllipse(color, rect_badge);
                                g.DrawEllipse(Style.Db.ErrorColor, borsize, rect_badge);
                                g.String(control.Badge, font, Style.Db.ErrorColor, rect_badge, s_f);
                            }
                            else
                            {
                                int w_badge = size.Width + (size_badge - size.Height);
                                var rect_badge = PaintBadge(rect, control.BadgeAlign, hasx, hasy, w_badge, size_badge);
                                using (var path = rect_badge.RoundPath(rect_badge.Height))
                                {
                                    g.Fill(color, path);
                                    g.Draw(Style.Db.ErrorColor, borsize, path);
                                }
                                g.String(control.Badge, font, Style.Db.ErrorColor, rect_badge, s_f);
                            }
                        }
                    }
                }
            }
        }

        static Rectangle PaintBadge(Rectangle rect, TAlignFrom align, int x, int y, int w, int h)
        {
            switch (align)
            {
                case TAlignFrom.TL:
                    return new Rectangle(rect.X + x, rect.Y + y, w, h);
                case TAlignFrom.BL:
                    return new Rectangle(rect.X + x, rect.Bottom - y - h, w, h);
                case TAlignFrom.BR:
                    return new Rectangle(rect.Right - x - w, rect.Bottom - y - h, w, h);
                case TAlignFrom.Top:
                    return new Rectangle(rect.X + (rect.Width - w) / 2, rect.Y + y, w, h);
                case TAlignFrom.Bottom:
                    return new Rectangle(rect.X + (rect.Width - w) / 2, rect.Bottom - y - h, w, h);
                case TAlignFrom.TR:
                default:
                    return new Rectangle(rect.Right - x - w, rect.Y + y, w, h);
            }
        }
        public static void PaintBadge(this IControl control, DateBadge badge, Font font, Rectangle rect, Canvas g)
        {
            var color = badge.Fill ?? control.BadgeBack ?? Style.Db.Error;
            float borsize = Config.Dpi;
            if (badge.Count == 0)
            {
                var rect_badge = new RectangleF(rect.Right - 10F, rect.Top + 2F, 8, 8);
                g.FillEllipse(color, rect_badge);
                g.DrawEllipse(color, borsize, rect_badge);
            }
            else
            {
                using (var s_f = SF_NoWrap())
                {
                    string countStr;
                    if (badge.Count == 999) countStr = "999";
                    else if (badge.Count > 1000) countStr = (badge.Count / 1000).ToString().Substring(0, 1) + "K+";
                    else if (badge.Count > 99) countStr = "99+";
                    else countStr = badge.Count.ToString();

                    var size = g.MeasureString(countStr, font);
                    int size_badge = (int)(size.Height * 1.2F);
                    if (size.Height > size.Width)
                    {
                        var rect_badge = new Rectangle(rect.Right - size_badge + 6, rect.Top - 8, size_badge, size_badge);
                        g.FillEllipse(color, rect_badge);
                        g.DrawEllipse(Style.Db.ErrorColor, borsize, rect_badge);
                        g.String(countStr, font, Style.Db.ErrorColor, rect_badge, s_f);
                    }
                    else
                    {
                        int w_badge = size.Width + (size_badge - size.Height);
                        var rect_badge = new Rectangle(rect.Right - w_badge + 6, rect.Top - 8, w_badge, size_badge);
                        using (var path = rect_badge.RoundPath(rect_badge.Height))
                        {
                            g.Fill(color, path);
                            g.Draw(Style.Db.ErrorColor, borsize, path);
                        }
                        g.String(countStr, font, Style.Db.ErrorColor, rect_badge, s_f);
                    }
                }
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
                        using (var brush = config.ShadowColor.Brush(Style.Db.TextBase))
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

        #region 图像处理

        #region 模糊

        public static void Blur(Bitmap bmp, int range)
        {
            Blur(bmp, range, new Rectangle(0, 0, bmp.Width, bmp.Height));
        }
        public static void Blur(Bitmap bmp, int range, Rectangle rect)
        {
            if (range > 1)
            {
                using (UnsafeBitmap unsafeBitmap = new UnsafeBitmap(bmp, true))
                {
                    BlurHorizontal(unsafeBitmap, range, rect);
                    BlurVertical(unsafeBitmap, range, rect);
                    BlurHorizontal(unsafeBitmap, range, rect);
                    BlurVertical(unsafeBitmap, range, rect);
                }
            }
        }

        private static void BlurHorizontal(UnsafeBitmap unsafeBitmap, int range, Rectangle rect)
        {
            int left = rect.X;
            int top = rect.Y;
            int right = rect.Right;
            int bottom = rect.Bottom;
            int halfRange = range / 2;
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

                    if (x >= left)
                    {
                        newColors[x] = new ColorBgra((byte)(b / hits), (byte)(g / hits), (byte)(r / hits), (byte)(a / hits));
                    }
                }

                for (int x = left; x < right; x++)
                {
                    unsafeBitmap.SetPixel(x, y, newColors[x]);
                }
            }
        }

        private static void BlurVertical(UnsafeBitmap unsafeBitmap, int range, Rectangle rect)
        {
            int left = rect.X;
            int top = rect.Y;
            int right = rect.Right;
            int bottom = rect.Bottom;
            int halfRange = range / 2;
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

                    if (y >= top)
                    {
                        newColors[y] = new ColorBgra((byte)(b / hits), (byte)(g / hits), (byte)(r / hits), (byte)(a / hits));
                    }
                }

                for (int y = top; y < bottom; y++)
                {
                    unsafeBitmap.SetPixel(x, y, newColors[y]);
                }
            }
        }

        #endregion

        #region 阴影

        public static Bitmap PaintShadow(this GraphicsPath path, int width, int height, int range = 10) => PaintShadow(path, width, height, Color.Black, range);
        public static Bitmap PaintShadow(this GraphicsPath path, int width, int height, Color color, int range = 10)
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