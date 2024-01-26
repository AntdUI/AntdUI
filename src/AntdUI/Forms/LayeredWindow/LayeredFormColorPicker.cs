// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
// THE AntdUI PROJECT IS AN WINFORM LIBRARY LICENSED UNDER THE GPL-3.0 License.
// LICENSED UNDER THE GPL License, VERSION 3.0 (THE "License")
// YOU MAY NOT USE THIS FILE EXCEPT IN COMPLIANCE WITH THE License.
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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace AntdUI
{
    public class LayeredFormColorPicker : ILayeredFormOpacityDown
    {
        internal float Radius = 10;
        TAlign ArrowAlign = TAlign.None;
        int ArrowSize = 8;
        int gap = 12, w = 258, h = 224, dot_size = 16, dot_bor_size = 2, line_h = 8, panel_color = 28;//260
        Color Value, ValueNAlpha, ValueHue;
        Action<Color> action;
        public LayeredFormColorPicker(ColorPicker control, RectangleF rect_read, Action<Color> _action)
        {
            color_alpha = Value = control.Value;
            ValueNAlpha = Color.FromArgb(255, Value);
            var hsv = ValueNAlpha.ToHSV();
            hsv.s = hsv.v = 1;
            ValueHue = hsv.HSVToColor();
            Radius = control.radius;
            PARENT = control;
            action = _action;
            int colors_h = 160;
            if (Config.Dpi != 1F)
            {
                colors_h = (int)(colors_h * Config.Dpi);

                gap = (int)(gap * Config.Dpi);
                dot_size = (int)(dot_size * Config.Dpi);
                dot_bor_size = (int)(dot_bor_size * Config.Dpi);
                line_h = (int)(line_h * Config.Dpi);
                panel_color = (int)(panel_color * Config.Dpi);
                w = (int)(w * Config.Dpi);
                h = (int)(h * Config.Dpi);
            }
            rect_colors = new Rectangle(10 + gap, 10 + gap, w - gap * 2, colors_h);
            rect_color = new Rectangle(10 + gap + (w - gap * 2) - panel_color, rect_colors.Bottom + gap, panel_color, panel_color);
            rect_hue = new Rectangle(10 + gap, rect_colors.Bottom + gap, w - gap * 3 - panel_color, line_h);
            rect_alpha = new Rectangle(rect_hue.X, rect_hue.Bottom + gap, rect_hue.Width, line_h);

            int line_h2 = line_h / 2;
            rect_colors_big = new Rectangle(rect_colors.X - line_h2, rect_colors.Y - line_h2, rect_colors.Width + line_h, rect_colors.Height + line_h);
            rect_hue_big = new Rectangle(rect_hue.X - line_h2, rect_hue.Y - line_h2, rect_hue.Width + line_h, rect_hue.Height + line_h);
            rect_alpha_big = new Rectangle(rect_alpha.X - line_h2, rect_alpha.Y - line_h2, rect_alpha.Width + line_h, rect_alpha.Height + line_h);


            bmp_dot_12 = new Bitmap(gap + 12, gap + 12);
            using (var g2 = Graphics.FromImage(bmp_dot_12).High())
            {
                using (var brush = new SolidBrush(Style.Db.BgBase))
                {
                    float yy = (bmp_dot_12.Height - gap) / 2F;
                    var rect = new RectangleF(6, 6, bmp_dot_12.Height - 12, bmp_dot_12.Height - 12);
                    g2.FillEllipse(Brushes.Black, rect);
                    Helper.Blur(bmp_dot_12, 6);
                    g2.CompositingMode = CompositingMode.SourceCopy;
                    g2.FillEllipse(Brushes.Transparent, rect);
                }
            }

            SetSizeW(w + 20);
            EndHeight = h + 20;
            var point = control.PointToScreen(Point.Empty);

            ArrowAlign = TAlign.BL;
            SetLocation(point.X + (int)rect_read.X - 10, point.Y + control.Height - 10 + ArrowSize);

            Location = TargetRect.Location;
            Size = TargetRect.Size;
        }

        public override void LoadOK()
        {
            BeginInvoke(new Action(() =>
            {
                Location = TargetRect.Location;
                Size = TargetRect.Size;
                Input input = new Input
                {
                    Dock = DockStyle.Bottom,
                    Size = new Size(0, 30)
                };
                Controls.Add(input);
            }));
            base.LoadOK();
        }

        #region 鼠标

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (rect_colors_big.Contains(e.Location))
                {
                    if (bmp_colors != null)
                    {
                        point_colors = new Point(e.X - 10 - gap, e.Y - 10 - gap);
                        if (point_colors.X < 0) point_colors.X = 0;
                        else if (point_colors.X > bmp_colors.Width - 1) point_colors.X = bmp_colors.Width - 1;
                        if (point_colors.Y < 0) point_colors.Y = 0;
                        else if (point_colors.Y > bmp_colors.Height - 1) point_colors.Y = bmp_colors.Height - 1;
                        color_alpha = Value = bmp_colors.GetPixel(point_colors.X, point_colors.Y);
                        ValueNAlpha = Color.FromArgb(255, Value);
                        action(Value);
                        bmp_alpha?.Dispose();
                        bmp_alpha = null;
                        Print();
                        down_colors = true;
                    }
                }
                else if (rect_hue_big.Contains(e.Location))
                {
                    if (bmp_hue != null)
                    {
                        point_hue = e.X - 10 - gap;
                        if (point_hue < 0) point_hue = 0;
                        else if (point_hue > bmp_hue.Width - 1) point_hue = bmp_hue.Width - 1;
                        ValueHue = bmp_hue.GetPixel(point_hue, 1);

                        var hsv = ValueHue.ToHSV();
                        var hsv_value = Value.ToHSV();
                        hsv_value.h = hsv.h;
                        color_alpha = Value = hsv_value.HSVToColor();
                        ValueNAlpha = Color.FromArgb(255, Value);
                        action(Value);
                        bmp_colors?.Dispose();
                        bmp_colors = null;
                        bmp_alpha?.Dispose();
                        bmp_alpha = null;
                        Print();
                        down_hue = true;
                    }
                }
                else if (rect_alpha_big.Contains(e.Location))
                {
                    if (bmp_alpha_read != null)
                    {
                        point_alpha = e.X - 10 - gap;
                        if (point_alpha < 0) point_alpha = 0;
                        else if (point_alpha > bmp_alpha_read.Width - 1) point_alpha = bmp_alpha_read.Width - 1;
                        color_alpha = Value = Color.FromArgb(bmp_alpha_read.GetPixel(point_alpha, 1).A, ValueNAlpha);
                        action(Value);
                        Print();
                        down_alpha = true;
                    }
                }
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (down_colors && bmp_colors != null)
            {
                point_colors = new Point(e.X - 10 - gap, e.Y - 10 - gap);
                if (point_colors.X < 0) point_colors.X = 0;
                else if (point_colors.X > bmp_colors.Width - 1) point_colors.X = bmp_colors.Width - 1;
                if (point_colors.Y < 0) point_colors.Y = 0;
                else if (point_colors.Y > bmp_colors.Height - 1) point_colors.Y = bmp_colors.Height - 1;
                color_alpha = Value = bmp_colors.GetPixel(point_colors.X, point_colors.Y);
                ValueNAlpha = Color.FromArgb(255, Value);
                action(Value);
                bmp_alpha?.Dispose();
                bmp_alpha = null;
                Print();
            }
            else if (down_hue && bmp_hue != null)
            {
                point_hue = e.X - 10 - gap;
                if (point_hue < 0) point_hue = 0;
                else if (point_hue > bmp_hue.Width - 1) point_hue = bmp_hue.Width - 1;
                ValueHue = bmp_hue.GetPixel(point_hue, 1);

                var hsv = ValueHue.ToHSV();
                var hsv_value = Value.ToHSV();
                hsv_value.h = hsv.h;
                color_alpha = Value = hsv_value.HSVToColor();
                ValueNAlpha = Color.FromArgb(255, Value);
                action(Value);
                bmp_colors?.Dispose();
                bmp_colors = null;
                bmp_alpha?.Dispose();
                bmp_alpha = null;
                Print();
            }
            else if (down_alpha && bmp_alpha_read != null)
            {
                point_alpha = e.X - 10 - gap;
                if (point_alpha < 0) point_alpha = 0;
                else if (point_alpha > bmp_alpha_read.Width - 1) point_alpha = bmp_alpha_read.Width - 1;
                color_alpha = Value = Color.FromArgb(bmp_alpha_read.GetPixel(point_alpha, 1).A, ValueNAlpha);
                action(Value);
                Print();
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (down_colors) down_colors = false;
            if (down_hue) down_hue = false;
            if (down_alpha) down_alpha = false;
            base.OnMouseUp(e);
        }

        #endregion

        #region 渲染

        public override Bitmap PrintBit()
        {
            var rect = TargetRectXY;
            var rect_read = new Rectangle(10, 10, rect.Width - 20, rect.Height - 20);
            Bitmap original_bmp = new Bitmap(rect.Width, rect.Height);
            using (var g = Graphics.FromImage(original_bmp).High())
            {
                using (var brush_bg = new SolidBrush(Style.Db.BgElevated))
                {
                    using (var path = rect_read.RoundPath(Radius))
                    {
                        DrawShadow(g, rect, rect.Width, EndHeight);

                        g.FillPath(brush_bg, path);
                        if (ArrowAlign != TAlign.None) g.FillPolygon(brush_bg, ArrowAlign.AlignLines(ArrowSize, rect, rect_read));
                    }
                    #region 渲染

                    #region 调色板

                    if (bmp_colors == null)
                    {
                        bmp_colors = new Bitmap(rect_colors.Width, rect_colors.Height);
                        using (var g2 = Graphics.FromImage(bmp_colors).High())
                        {
                            PaintColors(g2, new Rectangle(0, 0, bmp_colors.Width, bmp_colors.Height));
                        }
                        GetColorsPoint(bmp_colors);
                    }
                    g.DrawImage(bmp_colors, rect_colors);
                    using (var path = rect_colors.RoundPath(Radius))
                    {
                        path.AddRectangle(new Rectangle(rect_colors.X - 1, rect_colors.Y - 1, rect_colors.Width + 2, rect_colors.Height + 2));
                        g.FillPath(brush_bg, path);
                    }

                    #endregion

                    #region 色相

                    if (bmp_hue == null)
                    {
                        bmp_hue = new Bitmap(rect_hue.Width, rect_hue.Height);
                        using (var g2 = Graphics.FromImage(bmp_hue).High())
                        {
                            PaintHue(g2, new Rectangle(0, 0, bmp_hue.Width, bmp_hue.Height));
                        }
                        GetHuePoint(bmp_hue);
                    }
                    g.DrawImage(bmp_hue, rect_hue);
                    using (var path = rect_hue.RoundPath(rect_hue.Height))
                    {
                        path.AddRectangle(new Rectangle(rect_hue.X - 1, rect_hue.Y - 1, rect_hue.Width + 2, rect_hue.Height + 2));
                        g.FillPath(brush_bg, path);
                    }

                    #endregion

                    #region 透明度

                    if (bmp_alpha_read == null)
                    {
                        bmp_alpha_read = new Bitmap(rect_alpha.Width, rect_alpha.Height);
                        using (var g2 = Graphics.FromImage(bmp_alpha_read).High())
                        {
                            PaintAlpha(g2, new Rectangle(0, 0, bmp_alpha_read.Width, bmp_alpha_read.Height), false);
                        }
                        GetAlphaPoint(bmp_alpha_read);
                    }
                    if (bmp_alpha == null)
                    {
                        bmp_alpha = new Bitmap(rect_alpha.Width, rect_alpha.Height);
                        using (var g2 = Graphics.FromImage(bmp_alpha).High())
                        {
                            PaintAlpha(g2, new Rectangle(0, 0, bmp_alpha.Width, bmp_alpha.Height), true);
                        }
                    }
                    g.DrawImage(bmp_alpha, rect_alpha);
                    using (var path = rect_alpha.RoundPath(rect_alpha.Height))
                    {
                        path.AddRectangle(new Rectangle(rect_alpha.X - 1, rect_alpha.Y - 1, rect_alpha.Width + 2, rect_alpha.Height + 2));
                        g.FillPath(brush_bg, path);
                    }

                    #endregion

                    #region 渲染指标

                    using (var brush_val = new SolidBrush(Value))
                    using (var brush_hue = new SolidBrush(ValueHue))
                    using (var pen = new Pen(Style.Db.BgBase, dot_bor_size))
                    {
                        #region 调色板

                        var _rect_colors = new Rectangle(rect_colors.X + point_colors.X - dot_size / 2, rect_colors.Y + point_colors.Y - dot_size / 2, dot_size, dot_size);
                        g.FillEllipse(brush_val, _rect_colors);
                        g.DrawEllipse(pen, _rect_colors);

                        #endregion

                        #region 色相

                        var _rect_hue = new Rectangle(rect_hue.X + point_hue - gap / 2, rect_hue.Y + rect_hue.Height / 2 - gap / 2, gap, gap);

                        g.DrawImage(bmp_dot_12, new Rectangle(rect_hue.X + point_hue - bmp_dot_12.Height / 2, rect_hue.Y + (rect_hue.Height - bmp_dot_12.Height) / 2, bmp_dot_12.Width, bmp_dot_12.Height));
                        g.FillEllipse(brush_hue, _rect_hue);
                        g.DrawEllipse(pen, _rect_hue);

                        #endregion

                        #region 透明度

                        brush_val.Color = color_alpha;
                        var _rect_alpha = new Rectangle(rect_alpha.X + point_alpha - gap / 2, rect_alpha.Y + rect_alpha.Height / 2 - gap / 2, gap, gap);

                        g.DrawImage(bmp_dot_12, new Rectangle(rect_alpha.X + point_alpha - bmp_dot_12.Height / 2, rect_alpha.Y + (rect_alpha.Height - bmp_dot_12.Height) / 2, bmp_dot_12.Width, bmp_dot_12.Height));
                        g.FillEllipse(brush_val, _rect_alpha);
                        g.DrawEllipse(pen, _rect_alpha);

                        #endregion

                        using (var path = rect_color.RoundPath(Radius))
                        {
                            g.FillPath(brush_val, path);
                        }
                    }

                    #endregion

                    #endregion
                }
            }
            return original_bmp;
        }

        #region 渲染帮助

        Bitmap bmp_dot_12;
        Rectangle rect_color;

        #region 取色器

        bool down_colors = false;
        Point point_colors = Point.Empty;
        Rectangle rect_colors_big;
        Rectangle rect_colors;
        Bitmap? bmp_colors = null;
        void PaintColors(Graphics g, Rectangle rect)
        {
            using (var brush = new SolidBrush(ValueHue))
            {
                g.FillRectangle(brush, rect);
            }
            RectangleF w = new RectangleF(rect.X, rect.Y, rect.Width - 2F, rect.Height), b = new RectangleF(rect.X, rect.Y + 2F, rect.Width, rect.Height - 4F);
            using (var brush = new LinearGradientBrush(w, Color.White, Color.Transparent, 0F))
            {
                g.FillRectangle(brush, w);
            }
            using (var brush = new LinearGradientBrush(b, Color.Transparent, Color.Black, 90F))
            {
                g.FillRectangle(brush, b);
            }
            using (var brush = new SolidBrush(Color.Black))
            {
                g.FillRectangle(brush, new RectangleF(rect.X, rect.Height - 2F, rect.Width, 2F));
            }
        }
        void GetColorsPoint(Bitmap bmp_colors)
        {
            for (int x = 0; x < bmp_colors.Width; x++)
            {
                for (int y = 0; y < bmp_colors.Height; y++)
                {
                    if (bmp_colors.GetPixel(x, y) == ValueNAlpha)
                    {
                        point_colors = new Point(x, y);
                        return;
                    }
                }
            }
        }

        #endregion

        #region 色相

        bool down_hue = false;
        int point_hue = 0;
        Rectangle rect_hue_big;
        Rectangle rect_hue;
        Bitmap? bmp_hue = null;
        void PaintHue(Graphics g, Rectangle rect)
        {
            int width = (rect.Width - 4) / 6;
            Rectangle rect1 = new Rectangle(2, 0, width, rect.Height), rect2 = new Rectangle(rect1.X + width, 0, width, rect.Height),
                rect3 = new Rectangle(rect1.X + width * 2, 0, width, rect.Height), rect4 = new Rectangle(rect1.X + width * 3, 0, width, rect.Height),
                rect5 = new Rectangle(rect1.X + width * 4, 0, width, rect.Height), rect6 = new Rectangle(rect1.X + width * 5, 0, width, rect.Height);

            using (var brush = new SolidBrush(Color.FromArgb(255, 0, 0)))
            {
                g.FillRectangle(brush, rect);
            }
            using (var brush = new LinearGradientBrush(rect1, Color.FromArgb(255, 0, 0), Color.FromArgb(255, 255, 0), 0F))
            {
                g.FillRectangle(brush, rect1);
            }
            using (var brush = new LinearGradientBrush(rect2, Color.FromArgb(255, 255, 0), Color.FromArgb(0, 255, 0), 0F))
            {
                g.FillRectangle(brush, rect2);
            }
            using (var brush = new LinearGradientBrush(rect3, Color.FromArgb(0, 255, 0), Color.FromArgb(0, 255, 255), 0F))
            {
                g.FillRectangle(brush, rect3);
            }
            using (var brush = new LinearGradientBrush(rect4, Color.FromArgb(0, 255, 255), Color.FromArgb(0, 0, 255), 0F))
            {
                g.FillRectangle(brush, rect4);
            }
            using (var brush = new LinearGradientBrush(rect5, Color.FromArgb(0, 0, 255), Color.FromArgb(255, 0, 255), 0F))
            {
                g.FillRectangle(brush, rect5);
            }
            using (var brush = new LinearGradientBrush(rect6, Color.FromArgb(255, 0, 255), Color.FromArgb(255, 0, 0), 0F))
            {
                g.FillRectangle(brush, rect6);
            }
        }
        void GetHuePoint(Bitmap bmp_hue)
        {
            int y = bmp_hue.Height / 2;
            var colors = new List<Color>();
            for (int x = 0; x < bmp_hue.Width; x++)
            {
                var color = bmp_hue.GetPixel(x, y);
                if (color == ValueHue)
                {
                    point_hue = x;
                    return;
                }
                colors.Add(color);
            }
            point_hue = find_i(colors, ValueHue);
        }

        int find_i(List<Color> cols, Color x)
        {
            int mini = int.MaxValue;
            int mind = mini;
            for (int i = 0; i < cols.Count; i++)
            {
                int dr, db, dg;
                dr = x.R - cols[i].R;
                dg = x.G - cols[i].G;
                db = x.B - cols[i].B;
                int d = dr * dr + dg * dg + db * db;
                if (d < mind)
                {
                    mind = d;
                    mini = i;
                }
            }
            return mini;
        }

        #endregion

        #region 透明度

        bool down_alpha = false;
        int point_alpha = 0;
        Rectangle rect_alpha_big;
        Rectangle rect_alpha;
        Bitmap? bmp_alpha = null, bmp_alpha_read = null;
        Color color_alpha = Color.White;
        void PaintAlpha(Graphics g, Rectangle rect, bool add)
        {
            if (add)
            {
                int he = rect.Height / 2;
                int u_x = 0;
                bool ad = false;
                while (u_x < rect.Width)
                {
                    ad = !ad;
                    using (var brush = new SolidBrush(Style.Db.FillSecondary))
                    {
                        g.FillRectangle(brush, new Rectangle(u_x, ad ? 0 : he, he, he));
                    }
                    u_x += he;
                }
            }
            rect.Offset(1, 0);
            using (var brush = new LinearGradientBrush(rect, Color.Transparent, ValueNAlpha, 0F))
            {
                g.FillRectangle(brush, rect);
            }
            using (var brush = new SolidBrush(ValueNAlpha))
            {
                g.FillRectangle(brush, new Rectangle(rect.Width - 1, 0, 4, rect.Height));
            }
        }
        void GetAlphaPoint(Bitmap bmp_alpha)
        {
            int y = bmp_alpha.Height / 2;
            var colors = new List<Color>();
            for (int x = 0; x < bmp_alpha.Width; x++)
            {
                var color = bmp_alpha.GetPixel(x, y);
                if (color.A == Value.A)
                {
                    point_alpha = x;
                    return;
                }
                colors.Add(color);
            }
            point_alpha = find_i(colors, ValueNAlpha);
        }

        #endregion

        #endregion

        Bitmap? shadow_temp = null;
        /// <summary>
        /// 绘制阴影
        /// </summary>
        /// <param name="g">GDI</param>
        /// <param name="rect_client">客户区域</param>
        /// <param name="shadow_width">最终阴影宽度</param>
        /// <param name="shadow_height">最终阴影高度</param>
        void DrawShadow(Graphics g, Rectangle rect_client, int shadow_width, int shadow_height)
        {
            if (shadow_temp == null || (shadow_temp.Width != shadow_width || shadow_temp.Height != shadow_height))
            {
                shadow_temp?.Dispose();
                using (var path = new Rectangle(10, 10, shadow_width - 20, shadow_height - 20).RoundPath(Radius))
                {
                    shadow_temp = path.PaintShadow(shadow_width, shadow_height);
                }
            }
            using (var attributes = new ImageAttributes())
            {
                var matrix = new ColorMatrix { Matrix33 = 0.2F };
                attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                g.DrawImage(shadow_temp, rect_client, 0, 0, shadow_temp.Width, shadow_temp.Height, GraphicsUnit.Pixel, attributes);
            }
        }

        #endregion
    }
}
