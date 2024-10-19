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
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AntdUI
{
    public class LayeredFormColorPicker : ILayeredFormOpacityDown
    {
        internal float Radius = 10;
        TAlign ArrowAlign = TAlign.None;
        readonly int ArrowSize = 8;
        int gap = 12, w = 258, h = 224, dot_size = 16, dot_bor_size = 2, line_h = 8, panel_color = 28;//260
        Color Value, ValueNAlpha, ValueHue;
        Action<Color> action;
        TColorMode mode;
        public LayeredFormColorPicker(ColorPicker control, Rectangle rect_read, Action<Color> _action)
        {
            control.Parent.SetTopMost(Handle);
            Font = control.Font;
            mode = control.Mode;
            MessageCloseMouseLeave = control.Trigger == Trigger.Hover;
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

            if (control.Mode == TColorMode.Rgb)
            {
                if (control.DisabledAlpha)
                {
                    w = Helper.GDI(g =>
                    {
                        var size = g.MeasureString("255%", Font);
                        return (int)Math.Ceiling((size.Width + size.Height) * 3.4F);
                    });
                }
                else
                {
                    w = Helper.GDI(g =>
                    {
                        var size = g.MeasureString("255%", Font);
                        return (int)Math.Ceiling((size.Width + size.Height) * 4.4F);
                    });
                }
                int chtmp = (int)Math.Ceiling(w * .62F);
                int chxc = chtmp - colors_h;
                colors_h = chtmp;
                h += chxc;
            }

            rect_colors = new Rectangle(10 + gap, 10 + gap, w - gap * 2, colors_h);
            rect_color = new Rectangle(10 + gap + (w - gap * 2) - panel_color, rect_colors.Bottom + gap, panel_color, panel_color);
            rect_hue = new Rectangle(10 + gap, rect_colors.Bottom + gap, w - gap * 3 - panel_color, line_h);
            int yb = rect_hue.Bottom + gap;
            int line_h2 = line_h / 2;
            if (control.DisabledAlpha)
            {
                rect_alpha = rect_alpha_big = new Rectangle(-1, -1, 0, 0);
                rect_hue.Offset(0, line_h + line_h2 / 2);
            }
            else
            {
                rect_alpha = new Rectangle(rect_hue.X, rect_hue.Bottom + gap, rect_hue.Width, line_h);
                rect_alpha_big = new Rectangle(rect_alpha.X - line_h2, rect_alpha.Y - line_h2, rect_alpha.Width + line_h, rect_alpha.Height + line_h);
            }
            rect_colors_big = new Rectangle(rect_colors.X - line_h2, rect_colors.Y - line_h2, rect_colors.Width + line_h, rect_colors.Height + line_h);
            rect_hue_big = new Rectangle(rect_hue.X - line_h2, rect_hue.Y - line_h2, rect_hue.Width + line_h, rect_hue.Height + line_h);

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

            h += panel_color + gap;

            int r_w = w + 20, r_h = h + 20;
            SetSize(r_w, r_h);
            CLocation(control.PointToScreen(Point.Empty), control.Placement, control.DropDownArrow, ArrowSize, 10, r_w, r_h, rect_read, ref Inverted, ref ArrowAlign, true);

            Location = TargetRect.Location;
            Size = TargetRect.Size;

            var rect_input = new Rectangle(rect_colors_big.X + 4, yb + line_h + gap, rect_colors_big.Width - 8, panel_color);
            int wsize = (int)(4 * Config.Dpi), wsize2 = wsize * 2;
            switch (control.Mode)
            {
                case TColorMode.Rgb:
                    if (control.DisabledAlpha)
                    {
                        int wr4 = rect_input.Width / 3 - wsize;
                        Rectangle rect_r = new Rectangle(rect_input.X, rect_input.Y, wr4, rect_input.Height),
                            rect_g = new Rectangle(rect_input.X + wr4 + wsize, rect_input.Y, wr4, rect_input.Height),
                            rect_b = new Rectangle(rect_input.X + wr4 * 2 + wsize2, rect_input.Y, wr4, rect_input.Height);

                        var input_r = new InputNumber
                        {
                            ShowControl = false,
                            PrefixText = "R",
                            Location = rect_r.Location,
                            Size = rect_r.Size,
                            Value = Value.R,
                            Maximum = 255
                        };
                        input_r.ValueChanged += (a, e) =>
                        {
                            if (isinput) ChangeColor(Color.FromArgb(Value.A, (int)e.Value, Value.G, Value.B));
                        };

                        var input_g = new InputNumber
                        {
                            ShowControl = false,
                            PrefixText = "G",
                            Location = rect_g.Location,
                            Size = rect_g.Size,
                            Value = Value.G,
                            Maximum = 255
                        };
                        input_g.ValueChanged += (a, e) =>
                        {
                            if (isinput) ChangeColor(Color.FromArgb(Value.A, Value.R, (int)e.Value, Value.B));
                        };

                        var input_b = new InputNumber
                        {
                            ShowControl = false,
                            PrefixText = "B",
                            Location = rect_b.Location,
                            Size = rect_b.Size,
                            Value = Value.B,
                            Maximum = 255
                        };
                        input_b.ValueChanged += (a, e) =>
                        {
                            if (isinput) ChangeColor(Color.FromArgb(Value.A, Value.R, Value.G, (int)e.Value));
                        };
                        inputs = new InputRect[] {
                            new InputRect(input_r, rect_r, wsize, wsize2),
                            new InputRect(input_g, rect_g, wsize, wsize2),
                            new InputRect(input_b, rect_b, wsize, wsize2)
                        };

                        input_r.TakePaint = input_g.TakePaint = input_b.TakePaint = () =>
                        {
                            Print();
                        };
                        Controls.Add(input_r);
                        Controls.Add(input_g);
                        Controls.Add(input_b);
                    }
                    else
                    {
                        int wr4 = rect_input.Width / 4 - wsize;
                        Rectangle rect_r = new Rectangle(rect_input.X, rect_input.Y, wr4, rect_input.Height),
                            rect_g = new Rectangle(rect_input.X + wr4 + wsize, rect_input.Y, wr4, rect_input.Height),
                            rect_b = new Rectangle(rect_input.X + wr4 * 2 + wsize2, rect_input.Y, wr4, rect_input.Height),
                            rect_a = new Rectangle(rect_input.X + wr4 * 3 + wsize * 3, rect_input.Y, wr4, rect_input.Height);

                        var input_r = new InputNumber
                        {
                            ShowControl = false,
                            PrefixText = "R",
                            Location = rect_r.Location,
                            Size = rect_r.Size,
                            Value = Value.R,
                            Maximum = 255
                        };
                        input_r.ValueChanged += (a, e) =>
                        {
                            if (isinput) ChangeColor(Color.FromArgb(Value.A, (int)e.Value, Value.G, Value.B));
                        };

                        var input_g = new InputNumber
                        {
                            ShowControl = false,
                            PrefixText = "G",
                            Location = rect_g.Location,
                            Size = rect_g.Size,
                            Value = Value.G,
                            Maximum = 255
                        };
                        input_g.ValueChanged += (a, e) =>
                        {
                            if (isinput) ChangeColor(Color.FromArgb(Value.A, Value.R, (int)e.Value, Value.B));
                        };

                        var input_b = new InputNumber
                        {
                            ShowControl = false,
                            PrefixText = "B",
                            Location = rect_b.Location,
                            Size = rect_b.Size,
                            Value = Value.B,
                            Maximum = 255
                        };
                        input_b.ValueChanged += (a, e) =>
                        {
                            if (isinput) ChangeColor(Color.FromArgb(Value.A, Value.R, Value.G, (int)e.Value));
                        };

                        var input_a = new InputNumber
                        {
                            ShowControl = false,
                            SuffixText = "%",
                            Location = rect_a.Location,
                            Size = rect_a.Size,
                            Value = (int)(Value.A / 255F * 100F),
                            Maximum = 100
                        };
                        input_a.ValueChanged += (s, e) =>
                        {
                            if (isinput) ChangeColor(Color.FromArgb((int)(255F * ((int)e.Value / 100F)), Value.R, Value.G, Value.B), true);
                        };

                        inputs = new InputRect[] {
                            new InputRect(input_r, rect_r, wsize, wsize2),
                            new InputRect(input_g, rect_g, wsize, wsize2),
                            new InputRect(input_b, rect_b, wsize, wsize2),
                            new InputRect(input_a, rect_a, wsize, wsize2)
                        };

                        input_r.TakePaint = input_g.TakePaint = input_b.TakePaint = input_a.TakePaint = () =>
                        {
                            Print();
                        };
                        Controls.Add(input_r);
                        Controls.Add(input_g);
                        Controls.Add(input_b);
                        Controls.Add(input_a);
                    }
                    break;
                case TColorMode.Hex:
                default:
                    var input = new Input
                    {
                        RECTDIV = rect_input,
                        Padding = new Padding(rect_colors_big.X + 4, 0, rect_colors_big.X + 4, 0),
                        Location = new Point(0, rect_input.Y),
                        Size = new Size(w + 20, rect_input.Height),
                        TextAlign = HorizontalAlignment.Center,
                        Text = "#" + Value.ToHex()
                    };
                    input.TakePaint = () =>
                    {
                        Print();
                    };
                    input.TextChanged += (a, b) =>
                    {
                        if (isinput) ChangeColor(input.Text.ToColor());
                    };
                    Controls.Add(input);
                    inputs = new InputRect[] { new InputRect(input, rect_input, wsize, wsize2) };
                    break;
            }
        }

        void ChangeColor(Color color, bool a = false)
        {
            color_alpha = Value = color;
            ValueNAlpha = Color.FromArgb(255, Value);
            var hsv = ValueNAlpha.ToHSV();
            hsv.s = hsv.v = 1;
            ValueHue = hsv.HSVToColor();

            action(Value);
            bmp_colors?.Dispose();
            bmp_colors = null;
            bmp_hue?.Dispose();
            bmp_hue = null;
            bmp_alpha?.Dispose();
            bmp_alpha = null;
            if (a)
            {
                bmp_alpha_read?.Dispose();
                bmp_alpha_read = null;
            }
            Print();
        }

        public override void LoadOK()
        {
            DisableMouse = false;
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

        bool isinput = true;
        void SetValue()
        {
            isinput = false;
            switch (mode)
            {
                case TColorMode.Rgb:
                    ((InputNumber)inputs[0].input).Value = Value.R;
                    ((InputNumber)inputs[1].input).Value = Value.G;
                    ((InputNumber)inputs[2].input).Value = Value.B;
                    if (inputs.Length > 3) ((InputNumber)inputs[3].input).Value = (int)(Value.A / 255F * 100F);
                    break;
                case TColorMode.Hex:
                default:
                    inputs[0].input.Text = "#" + Value.ToHex();
                    break;
            }
            action(Value);
            isinput = true;
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (rect_colors_big.Contains(e.Location))
                {
                    //顶部渐变色卡
                    if (bmp_colors != null)
                    {
                        point_colors = new Point(e.X - 10 - gap, e.Y - 10 - gap);
                        if (point_colors.X < 0) point_colors.X = 0;
                        else if (point_colors.X > bmp_colors.Width - 1) point_colors.X = bmp_colors.Width - 1;
                        if (point_colors.Y < 0) point_colors.Y = 0;
                        else if (point_colors.Y > bmp_colors.Height - 1) point_colors.Y = bmp_colors.Height - 1;
                        color_alpha = Value = Color.FromArgb(Value.A, bmp_colors.GetPixel(point_colors.X, point_colors.Y));
                        ValueNAlpha = Color.FromArgb(255, Value);
                        SetValue();
                        bmp_alpha?.Dispose();
                        bmp_alpha = null;
                        Print();
                        down_colors = true;
                    }
                }
                else if (rect_hue_big.Contains(e.Location))
                {
                    //色相
                    if (bmp_hue != null)
                    {
                        point_hue = e.X - 10 - gap;
                        if (point_hue < 0) point_hue = 0;
                        else if (point_hue > bmp_hue.Width - 1) point_hue = bmp_hue.Width - 1;
                        ValueHue = bmp_hue.GetPixel(point_hue, 1);

                        var hsv = ValueHue.ToHSV();
                        var hsv_value = Value.ToHSV();
                        hsv_value.h = hsv.h;
                        color_alpha = Value = Color.FromArgb(Value.A, hsv_value.HSVToColor());
                        ValueNAlpha = Color.FromArgb(255, Value);
                        SetValue();
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
                    //透明度
                    if (bmp_alpha_read != null)
                    {
                        point_alpha = e.X - 10 - gap;
                        if (point_alpha < 0) point_alpha = 0;
                        else if (point_alpha > bmp_alpha_read.Width - 1) point_alpha = bmp_alpha_read.Width - 1;
                        color_alpha = Value = Color.FromArgb(bmp_alpha_read.GetPixel(point_alpha, 1).A, ValueNAlpha);
                        SetValue();
                        Print();
                        down_alpha = true;
                    }
                }
            }
            base.OnMouseDown(e);
        }

        bool DisableMouse = true;

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (DisableMouse) return;
            if (down_colors && bmp_colors != null)
            {
                point_colors = new Point(e.X - 10 - gap, e.Y - 10 - gap);
                if (point_colors.X < 0) point_colors.X = 0;
                else if (point_colors.X > bmp_colors.Width - 1) point_colors.X = bmp_colors.Width - 1;
                if (point_colors.Y < 0) point_colors.Y = 0;
                else if (point_colors.Y > bmp_colors.Height - 1) point_colors.Y = bmp_colors.Height - 1;
                color_alpha = Value = Color.FromArgb(Value.A, bmp_colors.GetPixel(point_colors.X, point_colors.Y));
                ValueNAlpha = Color.FromArgb(255, Value);
                SetValue();
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
                color_alpha = Value = Color.FromArgb(Value.A, hsv_value.HSVToColor());
                ValueNAlpha = Color.FromArgb(255, Value);
                SetValue();
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
                SetValue();
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
                        DrawShadow(g, rect);
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

                    if (rect_alpha.Width > 0)
                    {
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

                        if (rect_alpha.Width > 0)
                        {
                            brush_val.Color = color_alpha;
                            var _rect_alpha = new Rectangle(rect_alpha.X + point_alpha - gap / 2, rect_alpha.Y + rect_alpha.Height / 2 - gap / 2, gap, gap);

                            g.DrawImage(bmp_dot_12, new Rectangle(rect_alpha.X + point_alpha - bmp_dot_12.Height / 2, rect_alpha.Y + (rect_alpha.Height - bmp_dot_12.Height) / 2, bmp_dot_12.Width, bmp_dot_12.Height));
                            g.FillEllipse(brush_val, _rect_alpha);
                            g.DrawEllipse(pen, _rect_alpha);
                        }

                        #endregion

                        using (var path = rect_color.RoundPath(Radius))
                        {
                            g.FillPath(brush_val, path);
                        }
                    }

                    #endregion

                    #endregion
                }
                foreach (var input in inputs) input.input.IPaint(g, input.rect, input.rect_read);
            }
            return original_bmp;
        }

        class InputRect
        {
            public InputRect(Input _input, Rectangle _rect_read, int wsize, int wsize2)
            {
                input = _input;
                _input.RECTDIV = rect = new Rectangle(_rect_read.X - wsize, _rect_read.Y - wsize, _rect_read.Width + wsize2, _rect_read.Height + wsize2);
                rect_read = _rect_read;
            }
            public Input input { get; set; }
            public Rectangle rect { get; set; }
            public Rectangle rect_read { get; set; }
        }
        InputRect[] inputs;

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
                using (var brush = new SolidBrush(Style.Db.FillSecondary))
                {
                    int he = rect.Height / 2;
                    int u_x = 0;
                    bool ad = false;
                    while (u_x < rect.Width)
                    {
                        ad = !ad;
                        g.FillRectangle(brush, new Rectangle(u_x, ad ? 0 : he, he, he));
                        u_x += he;
                    }
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
        /// <param name="rect">客户区域</param>
        void DrawShadow(Graphics g, Rectangle rect)
        {
            if (Config.ShadowEnabled)
            {
                if (shadow_temp == null)
                {
                    shadow_temp?.Dispose();
                    using (var path = new Rectangle(10, 10, rect.Width - 20, rect.Height - 20).RoundPath(Radius))
                    {
                        shadow_temp = path.PaintShadow(rect.Width, rect.Height);
                    }
                }
                g.DrawImage(shadow_temp, rect, 0.2F);
            }
        }

        #endregion

        public bool IProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)
        {
            foreach (var input in inputs)
            {
                if (input.input.Focused) return input.input.IProcessCmdKey(ref msg, keyData);
            }
            return false;
        }

        public void IKeyPress(KeyPressEventArgs e)
        {
            foreach (var input in inputs)
            {
                if (input.input.Focused)
                {
                    input.input.IKeyPress(e);
                    return;
                }
            }
        }
    }
}