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
using System.Windows.Forms;

namespace AntdUI.Design
{
    internal partial class FrmColorEditor : UserControl
    {
        Action<Color> action;
        public FrmColorEditor(object? val)
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.DoubleBuffer, true);
            UpdateStyles();

            InitializeComponent();

            if (val is Color _val) color_alpha = Value = _val;
            else color_alpha = Value = Colour.Primary.Get();
            ValueNAlpha = Color.FromArgb(255, Value);
            var hsv = ValueNAlpha.ToHSV();
            hsv.s = hsv.v = 1;
            ValueHue = hsv.HSVToColor();

            input1.Text = Value.ToHex();
            input1.TextChanged += input1_TextChanged;
            action = val => input1.Text = val.ToHex();
        }

        protected override void OnLoad(EventArgs e)
        {
            Helper.DpiAuto(Config.Dpi, this);
            base.OnLoad(e);
            count++;
        }

        #region 渲染

        int count = 1;
        protected override void OnPaint(PaintEventArgs e)
        {
            if (count == 0) return;
            count = 0;
            var rect = ClientRectangle;
            var g = e.Graphics.High();
            using (var brush_bg = new SolidBrush(BackColor))
            {
                int w = rect.Width, dot_size = 16, dot_bor_size = 2, line_h = 8, panel_color = 28;//260
                int colors_h = (int)(w * 0.58F);
                if (Config.Dpi != 1F)
                {
                    gap = (int)(12 * Config.Dpi);
                    dot_size = (int)(dot_size * Config.Dpi);
                    dot_bor_size = (int)(dot_bor_size * Config.Dpi);
                    line_h = (int)(line_h * Config.Dpi);
                    panel_color = (int)(panel_color * Config.Dpi);
                }

                if (bmp_dot_12 == null)
                {
                    bmp_dot_12 = new Bitmap(gap + 12, gap + 12);
                    using (var g2 = Graphics.FromImage(bmp_dot_12).High())
                    {
                        using (var brush = new SolidBrush(Colour.BgBase.Get()))
                        {
                            float yy = (bmp_dot_12.Height - gap) / 2F;
                            var rect2 = new RectangleF(6, 6, bmp_dot_12.Height - 12, bmp_dot_12.Height - 12);
                            g2.FillEllipse(Brushes.Black, rect2);
                            Helper.Blur(bmp_dot_12, 6);
                            g2.CompositingMode = CompositingMode.SourceCopy;
                            g2.FillEllipse(Brushes.Transparent, rect2);
                        }
                    }
                }

                rect_colors = new Rectangle(0, 0, w, colors_h);
                var rect_color = new Rectangle(gap + (w - gap * 2) - panel_color, rect_colors.Bottom + gap, panel_color, panel_color);
                rect_hue = new Rectangle(gap, rect_colors.Bottom + gap, w - gap * 3 - panel_color, line_h);
                rect_alpha = new Rectangle(rect_hue.X, rect_hue.Bottom + gap, rect_hue.Width, line_h);

                int line_h2 = line_h / 2;
                rect_colors_big = new Rectangle(rect_colors.X - line_h2, rect_colors.Y - line_h2, rect_colors.Width + line_h, rect_colors.Height + line_h);
                rect_hue_big = new Rectangle(rect_hue.X - line_h2, rect_hue.Y - line_h2, rect_hue.Width + line_h, rect_hue.Height + line_h);
                rect_alpha_big = new Rectangle(rect_alpha.X - line_h2, rect_alpha.Y - line_h2, rect_alpha.Width + line_h, rect_alpha.Height + line_h);

                #region 调色板

                if (bmp_colors != null && bmp_colors.Width != rect_colors.Width)
                {
                    bmp_colors.Dispose();
                    bmp_colors = null;
                }
                if (bmp_hue != null && bmp_hue.Width != rect_hue.Width)
                {
                    bmp_hue.Dispose();
                    bmp_hue = null;
                }
                if (bmp_alpha_read != null && bmp_alpha_read.Width != rect_alpha.Width)
                {
                    bmp_alpha_read.Dispose();
                    bmp_alpha_read = null;
                }
                if (bmp_alpha != null && bmp_alpha.Width != rect_alpha.Width)
                {
                    bmp_alpha.Dispose();
                    bmp_alpha = null;
                }

                if (bmp_colors == null)
                {
                    bmp_colors = new Bitmap(rect_colors.Width, rect_colors.Height);
                    using (var g2 = Graphics.FromImage(bmp_colors).High())
                    {
                        PaintColors(g2, new Rectangle(0, 0, bmp_colors.Width, bmp_colors.Height));
                    }
                    GetColorsPoint(bmp_colors);
                }
                g.Image(bmp_colors, rect_colors);

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
                g.Image(bmp_hue, rect_hue);
                using (var path = rect_hue.RoundPath(rect_hue.Height))
                {
                    path.AddRectangle(new Rectangle(rect_hue.X - 1, rect_hue.Y - 1, rect_hue.Width + 2, rect_hue.Height + 2));
                    g.Fill(brush_bg, path);
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
                g.Image(bmp_alpha, rect_alpha);
                using (var path = rect_alpha.RoundPath(rect_alpha.Height))
                {
                    path.AddRectangle(new Rectangle(rect_alpha.X - 1, rect_alpha.Y - 1, rect_alpha.Width + 2, rect_alpha.Height + 2));
                    g.Fill(brush_bg, path);
                }

                #endregion

                #region 渲染指标

                using (var brush_val = new SolidBrush(Value))
                using (var brush_hue = new SolidBrush(ValueHue))
                using (var pen = new Pen(Colour.BgBase.Get(), dot_bor_size))
                {
                    #region 调色板

                    var _rect_colors = new Rectangle(rect_colors.X + point_colors.X - dot_size / 2, rect_colors.Y + point_colors.Y - dot_size / 2, dot_size, dot_size);
                    g.FillEllipse(brush_val, _rect_colors);
                    g.DrawEllipse(pen, _rect_colors);

                    #endregion

                    #region 色相

                    var _rect_hue = new Rectangle(rect_hue.X + point_hue - gap / 2, rect_hue.Y + rect_hue.Height / 2 - gap / 2, gap, gap);

                    g.Image(bmp_dot_12, new Rectangle(rect_hue.X + point_hue - bmp_dot_12.Height / 2, rect_hue.Y + (rect_hue.Height - bmp_dot_12.Height) / 2, bmp_dot_12.Width, bmp_dot_12.Height));
                    g.FillEllipse(brush_hue, _rect_hue);
                    g.DrawEllipse(pen, _rect_hue);

                    #endregion

                    #region 透明度

                    brush_val.Color = color_alpha;
                    var _rect_alpha = new Rectangle(rect_alpha.X + point_alpha - gap / 2, rect_alpha.Y + rect_alpha.Height / 2 - gap / 2, gap, gap);

                    g.Image(bmp_dot_12, new Rectangle(rect_alpha.X + point_alpha - bmp_dot_12.Height / 2, rect_alpha.Y + (rect_alpha.Height - bmp_dot_12.Height) / 2, bmp_dot_12.Width, bmp_dot_12.Height));
                    g.FillEllipse(brush_val, _rect_alpha);
                    g.DrawEllipse(pen, _rect_alpha);

                    #endregion

                    g.FillEllipse(brush_val, rect_color);
                }

                #endregion

                #region 色卡集

                int gap2 = gap * 2, y = rect_alpha.Bottom + panel_color + line_h * 2 + gap, wr = (w - gap2) / gap2 - 1, windex = 0, wave = (int)Math.Ceiling(input1.WaveSize * Config.Dpi);
                input1.Left = gap - wave;
                input1.Height = panel_color + gap;
                input1.Width = w - gap2 + wave * 2;
                input1.Top = rect_alpha.Bottom + line_h;
                var colors_rect = new List<Rectangle>(colors.Length);
                foreach (var item in colors)
                {
                    var rect_colors = new Rectangle(rect_alpha.X + (gap2 * windex), y, gap, gap);
                    colors_rect.Add(rect_colors);
                    using (var brush = new SolidBrush(item))
                    {
                        g.Fill(brush, rect_colors);
                    }
                    windex++;
                    if (windex > wr)
                    {
                        windex = 0;
                        y += gap2;
                    }
                }
                rects_colors = colors_rect.ToArray();
                Height = y;

                #endregion
            }
            base.OnPaint(e);
        }

        static readonly Color[] colors = new Color[] {
            "#f44336".ToColor(),
            "#e91e63".ToColor(),
            "#9c27b0".ToColor(),
            "#673ab7".ToColor(),
            "#3f51b5".ToColor(),
            "#2196f3".ToColor(),
            "#03a9f4".ToColor(),
            "#00bcd4".ToColor(),
            "#009688".ToColor(),
            "#4caf50".ToColor(),
            "#cddc39".ToColor(),
            "#ffeb3b".ToColor(),
            "#ffc107".ToColor(),
            "#ff9800".ToColor(),
            "#ff5722".ToColor(),
            "#795548".ToColor(),
            "#9e9e9e".ToColor(),
            "#607d8b".ToColor()
        };
        Rectangle[] rects_colors = new Rectangle[0];

        public Color Value; Color ValueNAlpha, ValueHue;

        #region 渲染帮助

        Bitmap? bmp_dot_12;
        int gap = 12;

        #region 取色器

        bool down_colors = false;
        Point point_colors = Point.Empty;
        Rectangle rect_colors_big;
        Rectangle rect_colors;
        Bitmap? bmp_colors;
        void PaintColors(Canvas g, Rectangle rect)
        {
            using (var brush = new SolidBrush(ValueHue))
            {
                g.Fill(brush, rect);
            }
            RectangleF w = new RectangleF(rect.X, rect.Y, rect.Width - 2F, rect.Height), b = new RectangleF(rect.X, rect.Y + 2F, rect.Width, rect.Height - 4F);
            using (var brush = new LinearGradientBrush(w, Color.White, Color.Transparent, 0F))
            {
                g.Fill(brush, w);
            }
            using (var brush = new LinearGradientBrush(b, Color.Transparent, Color.Black, 90F))
            {
                g.Fill(brush, b);
            }
            using (var brush = new SolidBrush(Color.Black))
            {
                g.Fill(brush, new RectangleF(rect.X, rect.Height - 2F, rect.Width, 2F));
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
        Bitmap? bmp_hue;
        void PaintHue(Canvas g, Rectangle rect)
        {
            int width = (rect.Width - 4) / 6;
            Rectangle rect1 = new Rectangle(2, 0, width, rect.Height), rect2 = new Rectangle(rect1.X + width, 0, width, rect.Height),
                rect3 = new Rectangle(rect1.X + width * 2, 0, width, rect.Height), rect4 = new Rectangle(rect1.X + width * 3, 0, width, rect.Height),
                rect5 = new Rectangle(rect1.X + width * 4, 0, width, rect.Height), rect6 = new Rectangle(rect1.X + width * 5, 0, width, rect.Height);

            using (var brush = new SolidBrush(Color.FromArgb(255, 0, 0)))
            {
                g.Fill(brush, rect);
            }
            using (var brush = new LinearGradientBrush(rect1, Color.FromArgb(255, 0, 0), Color.FromArgb(255, 255, 0), 0F))
            {
                g.Fill(brush, rect1);
            }
            using (var brush = new LinearGradientBrush(rect2, Color.FromArgb(255, 255, 0), Color.FromArgb(0, 255, 0), 0F))
            {
                g.Fill(brush, rect2);
            }
            using (var brush = new LinearGradientBrush(rect3, Color.FromArgb(0, 255, 0), Color.FromArgb(0, 255, 255), 0F))
            {
                g.Fill(brush, rect3);
            }
            using (var brush = new LinearGradientBrush(rect4, Color.FromArgb(0, 255, 255), Color.FromArgb(0, 0, 255), 0F))
            {
                g.Fill(brush, rect4);
            }
            using (var brush = new LinearGradientBrush(rect5, Color.FromArgb(0, 0, 255), Color.FromArgb(255, 0, 255), 0F))
            {
                g.Fill(brush, rect5);
            }
            using (var brush = new LinearGradientBrush(rect6, Color.FromArgb(255, 0, 255), Color.FromArgb(255, 0, 0), 0F))
            {
                g.Fill(brush, rect6);
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
        Bitmap? bmp_alpha, bmp_alpha_read;
        Color color_alpha = Color.White;
        void PaintAlpha(Canvas g, Rectangle rect, bool add)
        {
            if (add)
            {
                int he = rect.Height / 2;
                int u_x = 0;
                bool ad = false;
                while (u_x < rect.Width)
                {
                    ad = !ad;
                    using (var brush = new SolidBrush(Colour.FillSecondary.Get()))
                    {
                        g.Fill(brush, new Rectangle(u_x, ad ? 0 : he, he, he));
                    }
                    u_x += he;
                }
            }
            rect.Offset(1, 0);
            using (var brush = new LinearGradientBrush(rect, Color.Transparent, ValueNAlpha, 0F))
            {
                g.Fill(brush, rect);
            }
            using (var brush = new SolidBrush(ValueNAlpha))
            {
                g.Fill(brush, new Rectangle(rect.Width - 1, 0, 4, rect.Height));
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

        #endregion

        #region 鼠标

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (rect_colors_big.Contains(e.X, e.Y))
                {
                    if (bmp_colors != null)
                    {
                        point_colors = e.Location;
                        if (point_colors.X < 0) point_colors.X = 0;
                        else if (point_colors.X > bmp_colors.Width - 1) point_colors.X = bmp_colors.Width - 1;
                        if (point_colors.Y < 0) point_colors.Y = 0;
                        else if (point_colors.Y > bmp_colors.Height - 1) point_colors.Y = bmp_colors.Height - 1;
                        color_alpha = Value = bmp_colors.GetPixel(point_colors.X, point_colors.Y);
                        ValueNAlpha = Color.FromArgb(255, Value);
                        action(Value);
                        bmp_alpha?.Dispose();
                        bmp_alpha = null;
                        count++;
                        Invalidate();
                        down_colors = true;
                    }
                }
                else if (rect_hue_big.Contains(e.X, e.Y))
                {
                    if (bmp_hue != null)
                    {
                        point_hue = e.X - gap;
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
                        count++;
                        Invalidate();
                        down_hue = true;
                    }
                }
                else if (rect_alpha_big.Contains(e.X, e.Y))
                {
                    if (bmp_alpha_read != null)
                    {
                        point_alpha = e.X - gap;
                        if (point_alpha < 0) point_alpha = 0;
                        else if (point_alpha > bmp_alpha_read.Width - 1) point_alpha = bmp_alpha_read.Width - 1;
                        color_alpha = Value = Color.FromArgb(bmp_alpha_read.GetPixel(point_alpha, 1).A, ValueNAlpha);
                        action(Value);
                        count++;
                        Invalidate();
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
                point_colors = e.Location;
                if (point_colors.X < 0) point_colors.X = 0;
                else if (point_colors.X > bmp_colors.Width - 1) point_colors.X = bmp_colors.Width - 1;
                if (point_colors.Y < 0) point_colors.Y = 0;
                else if (point_colors.Y > bmp_colors.Height - 1) point_colors.Y = bmp_colors.Height - 1;
                color_alpha = Value = bmp_colors.GetPixel(point_colors.X, point_colors.Y);
                ValueNAlpha = Color.FromArgb(255, Value);
                action(Value);
                bmp_alpha?.Dispose();
                bmp_alpha = null;
                count++;
                Invalidate();
            }
            else if (down_hue && bmp_hue != null)
            {
                point_hue = e.X - gap;
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
                count++;
                Invalidate();
            }
            else if (down_alpha && bmp_alpha_read != null)
            {
                point_alpha = e.X - gap;
                if (point_alpha < 0) point_alpha = 0;
                else if (point_alpha > bmp_alpha_read.Width - 1) point_alpha = bmp_alpha_read.Width - 1;
                color_alpha = Value = Color.FromArgb(bmp_alpha_read.GetPixel(point_alpha, 1).A, ValueNAlpha);
                action(Value);
                count++;
                Invalidate();
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (down_colors) { down_colors = false; return; }
            if (down_hue) { down_hue = false; return; }
            if (down_alpha) { down_alpha = false; return; }
            if (e.Button == MouseButtons.Left)
            {
                for (int i = 0; i < rects_colors.Length; i++)
                {
                    if (rects_colors[i].Contains(e.X, e.Y))
                    {
                        action(colors[i]);
                        return;
                    }
                }
            }
            base.OnMouseUp(e);
        }

        #endregion

        private void input1_TextChanged(object? sender, EventArgs e)
        {
            bmp_colors?.Dispose();
            bmp_colors = null;
            bmp_hue?.Dispose();
            bmp_hue = null;
            bmp_alpha?.Dispose();
            bmp_alpha = null;
            color_alpha = Value = input1.Text.ToColor();
            ValueNAlpha = Color.FromArgb(255, Value);
            var hsv = ValueNAlpha.ToHSV();
            hsv.s = hsv.v = 1;
            ValueHue = hsv.HSVToColor();
            count++;
            Invalidate();
        }
    }
}