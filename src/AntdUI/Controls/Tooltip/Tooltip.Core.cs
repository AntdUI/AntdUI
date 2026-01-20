// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace AntdUI
{
    internal static class ITooltipLib
    {
        #region 渲染

        public static Size RenderMeasure(this ITooltip core, Canvas g, int? maxWidth, out bool multiline, out int gap, out int arrowSize)
        {
            multiline = core.Text.Contains("\n");
            gap = (int)(3 * g.Dpi);
            int gap2 = gap * 2, paddingy = (int)(6 * g.Dpi) * 2 + gap2, paddingx = (int)(8 * g.Dpi) * 2 + gap2;
            var font_size = g.MeasureText(core.Text, core.Font);
            if (core.ArrowSize.HasValue) arrowSize = (int)(core.ArrowSize * g.Dpi);
            else arrowSize = (int)(g.MeasureText(Config.NullText, core.Font).Height * 0.3F);
            if (core.CustomWidth.HasValue)
            {
                int width = (int)Math.Ceiling(core.CustomWidth.Value * g.Dpi);
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

        public static void Render(this ITooltip core, Canvas g, Rectangle rect, bool multiline, int arrowSize, int arrowX, FormatFlags s_c, FormatFlags s_l)
        {
            int gap = (int)(3 * g.Dpi), paddingy = (int)(6 * g.Dpi), paddingx = (int)(8 * g.Dpi), gap2 = gap * 2, paddingy2 = paddingy * 2, paddingx2 = paddingx * 2;
            int radius = (int)(core.Radius * g.Dpi);
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
}