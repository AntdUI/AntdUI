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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

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

        public static Graphics High(this Graphics g)
        {
            Config.SetDpi(g);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            if (Config.TextRenderingHint.HasValue) g.TextRenderingHint = Config.TextRenderingHint.Value;
            return g;
        }
        public static Graphics HighLay(this Graphics g)
        {
            Config.SetDpi(g);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            return g;
        }

        public static void GDI(Action<Graphics> action)
        {
            using (var bmp = new Bitmap(1, 1))
            {
                using (var g = Graphics.FromImage(bmp))
                {
                    Config.SetDpi(g);
                    action(g);
                }
            }
        }

        public static T GDI<T>(Func<Graphics, T> action)
        {
            using (var bmp = new Bitmap(1, 1))
            {
                using (var g = Graphics.FromImage(bmp))
                {
                    Config.SetDpi(g);
                    return action(g);
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

        #region 图片渲染

        public static void PaintImg(this Graphics g, RectangleF rect, Image image, TFit fit, float radius, bool round)
        {
            if (round || radius > 0)
            {
                using (var bmp = new Bitmap((int)rect.Width, (int)rect.Height))
                {
                    using (var g2 = Graphics.FromImage(bmp).High())
                    {
                        PaintImg(g2, new RectangleF(0, 0, rect.Width, rect.Height), image, fit);
                    }
                    using (var brush = new TextureBrush(bmp, WrapMode.Clamp))
                    {
                        brush.TranslateTransform(rect.X, rect.Y);
                        if (round) g.FillEllipse(brush, rect);
                        else
                        {
                            using (var path = rect.RoundPath(radius))
                            {
                                g.FillPath(brush, path);
                            }
                        }
                    }
                }
            }
            else PaintImg(g, rect, image, fit);
        }
        public static void PaintImg(this Graphics g, RectangleF rect, Image image, TFit fit, float radius, TShape shape)
        {
            if (shape == TShape.Circle || shape == TShape.Round || radius > 0)
            {
                using (var bmp = new Bitmap((int)rect.Width, (int)rect.Height))
                {
                    using (var g2 = Graphics.FromImage(bmp).High())
                    {
                        PaintImg(g2, new RectangleF(0, 0, rect.Width, rect.Height), image, fit);
                    }
                    using (var brush = new TextureBrush(bmp, WrapMode.Clamp))
                    {
                        brush.TranslateTransform(rect.X, rect.Y);
                        if (shape == TShape.Circle) g.FillEllipse(brush, rect);
                        else
                        {
                            using (var path = rect.RoundPath(radius))
                            {
                                g.FillPath(brush, path);
                            }
                        }
                    }
                }
            }
            else PaintImg(g, rect, image, fit);
        }

        internal static void PaintImg(this Graphics g, RectangleF rect, Image image, TFit fit)
        {
            switch (fit)
            {
                case TFit.Fill:
                    g.DrawImage(image, rect);
                    break;
                case TFit.None:
                    g.DrawImage(image, new RectangleF(rect.X + (rect.Width - image.Width) / 2, rect.Y + (rect.Height - image.Height) / 2, image.Width, image.Height));
                    break;
                case TFit.Contain:
                    PaintImgContain(g, image, rect);
                    break;
                case TFit.Cover:
                    PaintImgCover(g, image, rect);
                    break;
            }
        }
        internal static void PaintImgCover(this Graphics g, Image image, RectangleF rect)
        {
            float originWidth = image.Width, originHeight = image.Height;
            if (originWidth == originHeight)
            {
                if (rect.Width == rect.Height) g.DrawImage(image, rect);
                else if (rect.Width > rect.Height) g.DrawImage(image, new RectangleF(0, (rect.Height - rect.Width) / 2, rect.Width, rect.Width));
                else g.DrawImage(image, new RectangleF((rect.Width - rect.Height) / 2, 0, rect.Height, rect.Height));
                return;
            }
            float destWidth = rect.Width, destHeight = rect.Height;
            float currentWidth, currentHeight;
            if ((originWidth * destHeight) > (originHeight * destWidth))
            {
                currentHeight = destHeight;
                currentWidth = (originWidth * destHeight) / originHeight;
            }
            else
            {
                currentWidth = destWidth;
                currentHeight = (destWidth * originHeight) / originWidth;
            }
            g.DrawImage(image, new RectangleF(rect.X + (destWidth - currentWidth) / 2, rect.Y + (destHeight - currentHeight) / 2, currentWidth, currentHeight), new RectangleF(0, 0, originWidth, originHeight), GraphicsUnit.Pixel);
        }
        internal static void PaintImgContain(this Graphics g, Image image, RectangleF rect)
        {
            float originWidth = image.Width, originHeight = image.Height;
            if (originWidth == originHeight)
            {
                if (rect.Width == rect.Height) g.DrawImage(image, rect);
                else if (rect.Width > rect.Height) g.DrawImage(image, new RectangleF((rect.Width - rect.Height) / 2, 0, rect.Height, rect.Height));
                else g.DrawImage(image, new RectangleF(0, (rect.Height - rect.Width) / 2, rect.Width, rect.Width));
                return;
            }
            float destWidth = rect.Width, destHeight = rect.Height;
            float currentWidth, currentHeight;
            if ((originWidth * destHeight) > (originHeight * destWidth))
            {
                currentWidth = destWidth;
                currentHeight = (destWidth * originHeight) / originWidth;
            }
            else
            {
                currentHeight = destHeight;
                currentWidth = (originWidth * destHeight) / originHeight;
            }
            g.DrawImage(image, new RectangleF(rect.X + (destWidth - currentWidth) / 2, rect.Y + (destHeight - currentHeight) / 2, currentWidth, currentHeight), new RectangleF(0, 0, originWidth, originHeight), GraphicsUnit.Pixel);
        }

        #endregion

        #region 图标渲染

        internal static void PaintIcons(this Graphics g, TType icon, Rectangle rect)
        {
            switch (icon)
            {
                case TType.Success:
                    using (var bmp = SvgExtend.GetImgExtend(SvgDb.IcoSuccess, rect, Style.Db.Success))
                    {
                        if (bmp == null) return;
                        g.DrawImage(bmp, rect);
                    }
                    break;
                case TType.Info:
                    using (var bmp = SvgExtend.GetImgExtend(SvgDb.IcoInfo, rect, Style.Db.Info))
                    {
                        if (bmp == null) return;
                        g.DrawImage(bmp, rect);
                    }
                    break;
                case TType.Warn:
                    using (var bmp = SvgExtend.GetImgExtend(SvgDb.IcoWarn, rect, Style.Db.Warning))
                    {
                        if (bmp == null) return;
                        g.DrawImage(bmp, rect);
                    }
                    break;
                case TType.Error:
                    using (var bmp = SvgExtend.GetImgExtend(SvgDb.IcoError, rect, Style.Db.Error))
                    {
                        if (bmp == null) return;
                        g.DrawImage(bmp, rect);
                    }
                    break;
            }
        }
        internal static void PaintIcons(this Graphics g, TType icon, Rectangle rect, Color back)
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
                        g.DrawImage(bmp, rect);
                    }
                    break;
                case TType.Info:
                    using (var bmp = SvgExtend.GetImgExtend(SvgDb.IcoInfo, rect, Style.Db.Info))
                    {
                        if (bmp == null) return;
                        g.DrawImage(bmp, rect);
                    }
                    break;
                case TType.Warn:
                    using (var bmp = SvgExtend.GetImgExtend(SvgDb.IcoWarn, rect, Style.Db.Warning))
                    {
                        if (bmp == null) return;
                        g.DrawImage(bmp, rect);
                    }
                    break;
                case TType.Error:
                    using (var bmp = SvgExtend.GetImgExtend(SvgDb.IcoError, rect, Style.Db.Error))
                    {
                        if (bmp == null) return;
                        g.DrawImage(bmp, rect);
                    }
                    break;
            }
        }
        internal static void PaintIconGhosts(this Graphics g, TType icon, Rectangle rect, Color color)
        {
            switch (icon)
            {
                case TType.Success:
                    using (var bmp = SvgExtend.GetImgExtend(SvgDb.IcoSuccessGhost, rect, color))
                    {
                        if (bmp == null) return;
                        g.DrawImage(bmp, rect);
                    }
                    break;
                case TType.Info:
                    using (var bmp = SvgExtend.GetImgExtend(SvgDb.IcoInfoGhost, rect, color))
                    {
                        if (bmp == null) return;
                        g.DrawImage(bmp, rect);
                    }
                    break;
                case TType.Warn:
                    using (var bmp = SvgExtend.GetImgExtend(SvgDb.IcoWarnGhost, rect, color))
                    {
                        if (bmp == null) return;
                        g.DrawImage(bmp, rect);
                    }
                    break;
                case TType.Error:
                    using (var bmp = SvgExtend.GetImgExtend(SvgDb.IcoErrorGhost, rect, color))
                    {
                        if (bmp == null) return;
                        g.DrawImage(bmp, rect);
                    }
                    break;
            }
        }
        internal static void PaintIconClose(this Graphics g, Rectangle rect, Color color)
        {
            PaintIconCore(g, rect, SvgDb.IcoErrorGhost, color);
        }
        internal static void PaintIconClose(this Graphics g, Rectangle rect, Color color, float dot)
        {
            PaintIconCore(g, rect, SvgDb.IcoErrorGhost, color, dot);
        }

        /// <summary>
        /// 绘制带圆背景的镂空图标
        /// </summary>
        internal static void PaintIconCoreGhost(this Graphics g, Rectangle rect, string svg, Color back, Color fore)
        {
            using (var brush = new SolidBrush(back))
            {
                g.FillEllipse(brush, rect);
            }
            using (var bmp = SvgExtend.GetImgExtend(svg, rect, fore))
            {
                if (bmp == null) return;
                g.DrawImage(bmp, rect);
            }
        }
        internal static void PaintIconCore(this Graphics g, Rectangle rect, string svg, Color back, Color fore)
        {
            using (var brush = new SolidBrush(back))
            {
                g.FillEllipse(brush, new Rectangle(rect.X + 1, rect.Y + 1, rect.Width - 2, rect.Height - 2));
            }
            using (var bmp = SvgExtend.GetImgExtend(svg, rect, fore))
            {
                if (bmp == null) return;
                g.DrawImage(bmp, rect);
            }
        }
        internal static void PaintIconCore(this Graphics g, Rectangle rect, string svg, Color color)
        {
            using (var bmp = SvgExtend.GetImgExtend(svg, rect, color))
            {
                if (bmp == null) return;
                g.DrawImage(bmp, rect);
            }
        }
        internal static void PaintIconCore(this Graphics g, Rectangle rect, string svg, Color color, float dot)
        {
            int size = (int)(rect.Height * dot);
            var rect_ico = new Rectangle(rect.X + (rect.Width - size) / 2, rect.Y + (rect.Height - size) / 2, size, size);
            using (var bmp = SvgExtend.GetImgExtend(svg, rect_ico, color))
            {
                if (bmp == null) return;
                g.DrawImage(bmp, rect_ico);
            }
        }

        #endregion

        #region 图片透明度

        public static void DrawImage(this Graphics g, Bitmap bmp, Rectangle rect, float opacity)
        {
            using (var attributes = new ImageAttributes())
            {
                var matrix = new ColorMatrix { Matrix33 = opacity };
                attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                g.DrawImage(bmp, rect, 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, attributes);
            }
        }

        public static void DrawImage(this Graphics g, Image bmp, Rectangle rect, float opacity)
        {
            if (opacity == 1F)
            {
                g.DrawImage(bmp, rect);
                return;
            }
            using (var attributes = new ImageAttributes())
            {
                var matrix = new ColorMatrix { Matrix33 = opacity };
                attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                g.DrawImage(bmp, rect, 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, attributes);
            }
        }

        #endregion

        #region 阴影/徽标

        #region 徽标

        public static void PaintBadge(this IControl control, Graphics g)
        {
            control.PaintBadge(control.ReadRectangle, g);
        }

        public static void PaintBadge(this IControl control, RectangleF rect, Graphics g)
        {
            var color = control.BadgeBack ?? Style.Db.Error;
            if (control.Badge != null)
            {
                using (var brush_fore = new SolidBrush(Style.Db.ErrorColor))
                {
                    float borsize = 1F * Config.Dpi;
                    using (var font = new Font(control.Font.FontFamily, control.Font.Size * control.BadgeSize))
                    {
                        if (string.IsNullOrEmpty(control.Badge) || control.Badge == "" || control.Badge == " ")
                        {
                            var size = (int)Math.Floor(g.MeasureString(Config.NullText, font).Width / 2);
                            var rect_badge = new RectangleF(rect.Right - size - control.BadgeOffsetX * Config.Dpi, control.BadgeOffsetY * Config.Dpi, size, size);

                            using (var brush = new SolidBrush(color))
                            {
                                if (control.BadgeMode)
                                {
                                    float b2 = borsize * 2, rr = size * 0.2F, rr2 = rr * 2;
                                    g.FillEllipse(brush_fore, new RectangleF(rect_badge.X - borsize, rect_badge.Y - borsize, rect_badge.Width + b2, rect_badge.Height + b2));
                                    using (var path = rect_badge.RoundPath(1, true))
                                    {
                                        path.AddEllipse(new RectangleF(rect_badge.X + rr, rect_badge.Y + rr, rect_badge.Width - rr2, rect_badge.Height - rr2));
                                        g.FillPath(brush, path);
                                    }
                                }
                                else
                                {
                                    g.FillEllipse(brush, rect_badge);
                                    using (var pen = new Pen(brush_fore.Color, borsize))
                                    {
                                        g.DrawEllipse(pen, rect_badge);
                                    }
                                }
                            }
                        }
                        else
                        {
                            using (var s_f = SF_NoWrap())
                            {
                                var size = g.MeasureString(control.Badge, font);
                                var size_badge = size.Height * 1.2F;
                                if (size.Height > size.Width)
                                {
                                    var rect_badge = new RectangleF(rect.Right - size_badge - control.BadgeOffsetX * Config.Dpi, control.BadgeOffsetY * Config.Dpi, size_badge, size_badge);
                                    using (var brush = new SolidBrush(color))
                                    {
                                        g.FillEllipse(brush, rect_badge);
                                        using (var pen = new Pen(brush_fore.Color, borsize))
                                        {
                                            g.DrawEllipse(pen, rect_badge);
                                        }
                                    }
                                    g.DrawStr(control.Badge, font, brush_fore, rect_badge, s_f);
                                }
                                else
                                {
                                    var w_badge = size.Width * 1.2F;
                                    var rect_badge = new RectangleF(rect.Right - w_badge - control.BadgeOffsetX * Config.Dpi, control.BadgeOffsetY * Config.Dpi, w_badge, size_badge);
                                    using (var brush = new SolidBrush(color))
                                    {
                                        using (var path = rect_badge.RoundPath(rect_badge.Height))
                                        {
                                            g.FillPath(brush, path);
                                            using (var pen = new Pen(brush_fore.Color, borsize))
                                            {
                                                g.DrawPath(pen, path);
                                            }
                                        }
                                    }
                                    g.DrawStr(control.Badge, font, brush_fore, rect_badge, s_f);
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void PaintBadge(this IControl control, DateBadge badge, Font font, RectangleF rect, Graphics g)
        {
            var color = badge.Fill ?? control.BadgeBack ?? Style.Db.Error;
            using (var brush_fore = new SolidBrush(Style.Db.ErrorColor))
            {
                float borsize = 1F * Config.Dpi;
                if (badge.Count == 0)
                {
                    var rect_badge = new RectangleF(rect.Right - 10F, rect.Top + 2F, 8, 8);
                    using (var brush = new SolidBrush(color))
                    {
                        g.FillEllipse(brush, rect_badge);
                        using (var pen = new Pen(brush_fore.Color, borsize))
                        {
                            g.DrawEllipse(pen, rect_badge);
                        }
                    }
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
                        var size_badge = size.Height * 1.2F;
                        if (size.Height > size.Width)
                        {
                            var rect_badge = new RectangleF(rect.Right - size_badge + 6F, rect.Top - 8F, size_badge, size_badge);
                            using (var brush = new SolidBrush(color))
                            {
                                g.FillEllipse(brush, rect_badge);
                                using (var pen = new Pen(brush_fore.Color, borsize))
                                {
                                    g.DrawEllipse(pen, rect_badge);
                                }
                            }
                            g.DrawStr(countStr, font, brush_fore, rect_badge, s_f);
                        }
                        else
                        {
                            var w_badge = size.Width * 1.2F;
                            var rect_badge = new RectangleF(rect.Right - w_badge + 6F, rect.Top - 8F, w_badge, size_badge);
                            using (var brush = new SolidBrush(color))
                            {
                                using (var path = rect_badge.RoundPath(rect_badge.Height))
                                {
                                    g.FillPath(brush, path);
                                    using (var pen = new Pen(brush_fore.Color, borsize))
                                    {
                                        g.DrawPath(pen, path);
                                    }
                                }
                            }
                            g.DrawStr(countStr, font, brush_fore, rect_badge, s_f);
                        }
                    }
                }
            }
        }

        #endregion

        public static void PaintShadow(this Graphics g, ShadowConfig config, Rectangle _rect, Rectangle rect, float radius, bool round)
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
                    g.DrawImage(bmp_shadow, new Rectangle(_rect.X + shadowOffsetX, _rect.Y + shadowOffsetY, _rect.Width, _rect.Height), 0, 0, _rect.Width, _rect.Height, GraphicsUnit.Pixel, attributes);
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

        public static Bitmap PaintShadow(this GraphicsPath path, int width, int height, int range = 10)
        {
            return PaintShadow(path, width, height, Color.Black, range);
        }
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