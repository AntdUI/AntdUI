// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AntdUI
{
    public class LayeredFormColorPicker : ILayeredShadowForm
    {
        int Radius2 = 8;
        int gap = 12, w = 258, h = 224, dot_size = 16, dot_bor_size = 2, line_h = 8, panel_color = 28, panel_color_input = 0, btn_size = 24;//260
        int offy = 0;
        Color Value, ValueNAlpha, ValueHue, ValueDefault;
        Action<Color>? action;
        TColorMode mode;
        bool AllowClear = false, ShowClose = false, ShowReset = false, Dfont = false;
        TAMode ColorScheme;

        #region 初始化

        public LayeredFormColorPicker(ColorPicker control, Rectangle rect_read, Action<Color> _action)
        {
            ColorScheme = control.ColorScheme;
            control.Parent.SetTopMost(Handle);
            AllowClear = control.AllowClear;
            ShowClose = control.ShowClose;
            ShowReset = control.ShowReset;
            if (control.DropDownFontRatio == 1F) Font = control.Font;
            else
            {
                Dfont = true;
                Font = new Font(control.Font.FontFamily, control.Font.Size * control.DropDownFontRatio);
            }
            mode = control.Mode;
            if (control.Trigger == Trigger.Hover) CloseMode = CloseMode.Leave;
            color_alpha = Value = ValueDefault = control.Value;
            ValueNAlpha = Color.FromArgb(255, Value);
            var hsv = ValueNAlpha.ToHSV();
            hsv.s = hsv.v = 1;
            ValueHue = hsv.HSVToColor();
            PARENT = control;
            action = _action;

            int colors_h = InitDpi(Dpi, control.Radius, 8, 160);
            InitWidth(ref colors_h);
            int yb = InitSize(control, colors_h);

            CLocation(control, control.Placement, control.DropDownArrow, ArrowSize);

            inputs = InitInput(control, yb);
        }
        public LayeredFormColorPicker(ColorPicker.Config config)
        {
            ColorScheme = config.ColorScheme;
            config.Target.SetTopMost(Handle);
            AllowClear = config.AllowClear;
            ShowClose = config.ShowClose;
            ShowReset = config.ShowReset;
            config.Target.SetFont(config.Font, this);
            if (config.FontRatio != 1F && config.Font == null)
            {
                Dfont = true;
                Font = new Font(Font.FontFamily, Font.Size * config.FontRatio);
            }
            mode = config.Mode;
            color_alpha = Value = ValueDefault = config.Value ?? Style.Db.Primary;
            ValueNAlpha = Color.FromArgb(255, Value);
            var hsv = ValueNAlpha.ToHSV();
            hsv.s = hsv.v = 1;
            ValueHue = hsv.HSVToColor();
            action = config.Call;

            int colors_h = InitDpi(Dpi, config.Radius, config.ArrowSize, 160);
            InitWidth(ref colors_h);
            int yb = InitSize(config, colors_h);

            if (config.Target.Value is Control control)
            {
                var calculateCoordinate = new CalculateCoordinate(this, control, TargetRect, Radius, ArrowSize, shadow, shadow2, config.Offset);
                calculateCoordinate.Auto(config.Placement, animateConfig, true, out int rx, out int ry, out ArrowLine);
                SetLocation(rx, ry);
            }
            else
            {
                var screen = Screen.FromPoint(MousePosition).WorkingArea;
                SetLocationCenter(screen);
            }
            inputs = InitInput(config, yb);
        }

        int InitDpi(float dpi, int radius, int arrowSize, int colors_h)
        {
            Radius = (int)(radius * dpi);
            Radius2 = (int)(Radius * 0.75F);

            if (dpi == 1F)
            {
                ArrowSize = arrowSize;
                return colors_h;
            }

            ArrowSize = (int)(arrowSize * dpi);
            gap = (int)(gap * dpi);
            dot_size = (int)(dot_size * dpi);
            dot_bor_size = (int)(dot_bor_size * dpi);
            btn_size = (int)(btn_size * dpi);
            line_h = (int)(line_h * dpi);
            panel_color = (int)(panel_color * dpi);
            w = (int)(w * dpi);
            h = (int)(h * dpi);

            return (int)(colors_h * dpi);
        }

        void InitWidth(ref int colors_h)
        {
            if (mode == TColorMode.Rgb)
            {
                w = Helper.GDI(g =>
                {
                    var size = g.MeasureString("255%", Font);
                    panel_color_input = (int)(size.Height * 1.52F);
                    return (size.Width + size.Height) * 4;
                });
                int chtmp = (int)Math.Ceiling(w * .62F);
                int chxc = chtmp - colors_h;
                colors_h = chtmp;
                h += chxc;
            }
            else panel_color_input = Helper.GDI(g => (int)(g.MeasureString("255%", Font).Height * 1.52F));
        }

        int InitSize(IColorPicker config, int colors_h)
        {
            int y = 0;

            if (AllowClear || ShowClose || ShowReset)
            {
                int uw = gap + btn_size, ux = w - uw;
                if (AllowClear)
                {
                    rect_btn = new Rectangle(ux, y + gap, btn_size, btn_size);
                    ux -= uw;
                }
                if (ShowReset)
                {
                    rect_reset = new Rectangle(ux, y + gap, btn_size, btn_size);
                    ux -= uw;
                }
                if (ShowClose) rect_close = new Rectangle(gap, y + gap, btn_size, btn_size);
                offy = btn_size + line_h + line_h / 2;
                y += offy;
                h += offy;
            }

            rect_colors = new Rectangle(gap, y + gap, w - gap * 2, colors_h);
            rect_color = new Rectangle(gap + (w - gap * 2) - panel_color, rect_colors.Bottom + gap, panel_color, panel_color);
            rect_hue = new Rectangle(gap, rect_colors.Bottom + gap, w - gap * 3 - panel_color, line_h);
            int yb = rect_hue.Bottom + gap;
            int line_h2 = line_h / 2;
            if (config.DisabledAlpha)
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

            int gap2 = gap * 2, gap_2 = gap / 2;
            bmp_dot_12 = new Bitmap(gap2, gap2);
            using (var g2 = Graphics.FromImage(bmp_dot_12).High())
            {
                using (var brush = new SolidBrush(Colour.BgBase.Get(name, ColorScheme)))
                {
                    float yy = (bmp_dot_12.Height - gap) / 2F;
                    var rect = new RectangleF(gap_2, gap_2, bmp_dot_12.Height - gap, bmp_dot_12.Height - gap);
                    g2.FillEllipse(brush, rect);
                    Helper.Blur(bmp_dot_12, gap_2);
                    g2.CompositingMode = CompositingMode.SourceCopy;
                    g2.FillEllipse(Brushes.Transparent, rect);
                }
            }

            h += panel_color_input + gap;
            h += InitColors(config, h);

            SetSize(w, h);
            return yb;
        }

        #endregion

        #region 文本框

        InputRect[] InitInput(IColorPicker config, int yb)
        {
            InputRect[] inputs;
            int wave = (int)(4 * Dpi), wave2 = wave * 2;
            var rect_input = new Rectangle(rect_colors_big.X + wave, yb + line_h + gap, rect_colors_big.Width - wave2, panel_color_input);
            Action call = () =>
            {
                if (RunAnimation) return;
                Print();
            };
            switch (config.Mode)
            {
                case TColorMode.Rgb:
                    if (config.DisabledAlpha)
                    {
                        int wr4 = rect_input.Width / 3 - wave;
                        Rectangle rect_r = new Rectangle(rect_input.X, rect_input.Y, wr4, rect_input.Height),
                            rect_g = new Rectangle(rect_input.X + wr4 + wave, rect_input.Y, wr4, rect_input.Height),
                            rect_b = new Rectangle(rect_input.X + wr4 * 2 + wave2, rect_input.Y, wr4, rect_input.Height);

                        InputNumberByLayeredForm input_r = GetInputNumber(rect_r, Value.R, call, "R"),
                            input_g = GetInputNumber(rect_g, Value.G, call, "G"),
                            input_b = GetInputNumber(rect_b, Value.B, call, "B");

                        inputs = new InputRect[] {
                            new InputRect(input_r, rect_r, wave, wave2),
                            new InputRect(input_g, rect_g, wave, wave2),
                            new InputRect(input_b, rect_b, wave, wave2)
                        };
                    }
                    else
                    {
                        int wr4 = rect_input.Width / 4 - wave;
                        Rectangle rect_r = new Rectangle(rect_input.X, rect_input.Y, wr4, rect_input.Height),
                            rect_g = new Rectangle(rect_input.X + wr4 + wave, rect_input.Y, wr4, rect_input.Height),
                            rect_b = new Rectangle(rect_input.X + wr4 * 2 + wave2, rect_input.Y, wr4, rect_input.Height),
                            rect_a = new Rectangle(rect_input.X + wr4 * 3 + wave * 3, rect_input.Y, wr4, rect_input.Height);


                        InputNumberByLayeredForm input_r = GetInputNumber(rect_r, Value.R, call, "R"),
                            input_g = GetInputNumber(rect_g, Value.G, call, "G"),
                            input_b = GetInputNumber(rect_b, Value.B, call, "B"),
                            input_a = GetInputNumber(rect_a, (int)(Value.A / 255F * 100F), call);

                        inputs = new InputRect[] {
                            new InputRect(input_r, rect_r, wave, wave2),
                            new InputRect(input_g, rect_g, wave, wave2),
                            new InputRect(input_b, rect_b, wave, wave2),
                            new InputRect(input_a, rect_a, wave, wave2)
                        };
                    }
                    break;
                case TColorMode.Hex:
                default:
                    inputs = new InputRect[] { new InputRect(GetInput(rect_input, call), rect_input, wave, wave2) };
                    break;
            }
            foreach (var it in inputs) Controls.Add(it.input);
            isinput = true;
            return inputs;
        }

        InputByLayeredForm GetInput(Rectangle rect, Action call)
        {
            var input = new InputByLayeredForm
            {
                MOffset = shadow,
                PaddGap = 0F,
                IconGap = 0F,
                IconRatioRight = 0F,
                PrefixFore = Style.Db.TextTertiary,
                PrefixText = "#",
                Location = new Point(shadow + rect.X, shadow + rect.Y),
                Size = rect.Size,
                TextAlign = HorizontalAlignment.Center,
                Text = Value.ToHex(),
                TakePaint = call
            };
            input.TextChanged += (a, e) =>
            {
                if (isinput) ChangeColor(input.Text.ToColor());
            };
            return input;
        }

        InputNumberByLayeredForm GetInputNumber(Rectangle rect, byte value, Action call, string PrefixText)
        {
            var input = new InputNumberByLayeredForm
            {
                MOffset = shadow,
                PaddGap = 0F,
                IconGap = 0F,
                IconRatioRight = 0F,
                PrefixFore = Style.Db.TextTertiary,
                PrefixText = PrefixText,
                TabStop = false,
                ShowControl = false,
                Location = new Point(shadow + rect.X, shadow + rect.Y),
                Size = rect.Size,
                Value = value,
                Maximum = 255,
                TakePaint = call
            };
            input.ValueChanged += (a, e) =>
            {
                if (isinput) ChangeColor(Color.FromArgb(Value.A, Value.R, Value.G, Value.B));
            };
            return input;
        }
        InputNumberByLayeredForm GetInputNumber(Rectangle rect, int value, Action call)
        {
            var input = new InputNumberByLayeredForm
            {
                MOffset = shadow,
                PaddGap = 0F,
                IconGap = 0F,
                IconRatioRight = 0F,
                SuffixFore = Style.Db.TextTertiary,
                SuffixText = "%",
                TabStop = false,
                ShowControl = false,
                Location = new Point(shadow + rect.X, shadow + rect.Y),
                Size = rect.Size,
                Value = value,
                Maximum = 100,
                TakePaint = call
            };
            input.ValueChanged += (a, e) =>
            {
                if (isinput) ChangeColor(Color.FromArgb(Value.A, Value.R, Value.G, Value.B));
            };
            return input;
        }

        #endregion

        #region 色卡

        ColorRect[]? preset_color;
        int InitColors(IColorPicker config, int y)
        {
            if (config.Presets == null || config.Presets.Length == 0) return 0;

            int w = rect_colors.Width;

            int count = (int)Math.Floor(w * 1F / btn_size);
            if (count % 2 == 1) count--;
            int tmp_w = btn_size * count, color_gap = (w - tmp_w) / (count - 1), x = (w - (tmp_w + color_gap * (count - 1))) / 2,
                h = 0, use_x = 0, tsize = btn_size + color_gap;

            var list = new List<ColorRect>(config.Presets.Length);
            foreach (var it in config.Presets)
            {
                list.Add(new ColorRect
                {
                    color = it,
                    rect = new Rectangle(rect_colors.X + x + use_x, y + h, btn_size, btn_size)
                });
                use_x += tsize;
                if (use_x > w)
                {
                    h += tsize;
                    use_x = 0;
                }
            }
            if (use_x > 0) h += tsize;

            preset_color = list.ToArray();

            return h + gap - color_gap;
        }

        class ColorRect
        {
            public Color color { get; set; }
            public bool hover { get; set; }
            public Rectangle rect { get; set; }
        }

        #endregion

        int ArrowSize = 0;

        public override string name => nameof(ColorPicker);

        void ChangeColor(Color color, bool a = false)
        {
            color_alpha = Value = color;
            ValueNAlpha = Color.FromArgb(255, Value);
            var hsv = ValueNAlpha.ToHSV();
            hsv.s = hsv.v = 1;
            ValueHue = hsv.HSVToColor();

            action?.Invoke(Value);
            colors_mouse = null;
            bmp_hue?.Dispose();
            bmp_hue = null;
            bmp_alpha?.Dispose();
            bmp_alpha = null;
            if (a)
            {
                bmp_alpha_read?.Dispose();
                bmp_alpha_read = null;
            }
            Print(true);
        }

        public override void LoadOK()
        {
            BeginInvoke(() =>
            {
                Location = TargetRect.Location;
                Size = TargetRect.Size;
                Input input = new Input
                {
                    Dock = DockStyle.Bottom,
                    Size = new Size(0, 30)
                };
                Controls.Add(input);
            });
            base.LoadOK();
        }

        #region 鼠标

        bool isinput = false;
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
                    inputs[0].input.Text = Value.ToHex();
                    break;
            }
            action?.Invoke(Value);
            isinput = true;
        }
        protected override void OnMouseDown(MouseButtons button, int clicks, int x, int y, int delta)
        {
            if (button == MouseButtons.Left)
            {
                if (rect_colors_big.Contains(x, y))
                {
                    //顶部渐变色卡
                    if (colors_mouse != null)
                    {
                        var value = GetColors(x, y, colors_mouse);

                        color_alpha = Value = Color.FromArgb(Value.A, value);
                        ValueNAlpha = Color.FromArgb(255, Value);
                        SetValue();
                        bmp_alpha?.Dispose();
                        bmp_alpha = null;
                        Print(true);
                        down_colors = true;
                    }
                }
                else if (rect_hue_big.Contains(x, y))
                {
                    //色相
                    if (bmp_hue != null)
                    {
                        point_hue = x - gap;
                        if (point_hue < 0) point_hue = 0;
                        else if (point_hue > bmp_hue.Width - 1) point_hue = bmp_hue.Width - 1;
                        ValueHue = bmp_hue.GetPixel(point_hue, 1);

                        var hsv = ValueHue.ToHSV();
                        var hsv_value = Value.ToHSV();
                        hsv_value.h = hsv.h;
                        color_alpha = Value = Color.FromArgb(Value.A, hsv_value.HSVToColor());
                        ValueNAlpha = Color.FromArgb(255, Value);
                        SetValue();
                        colors_mouse = null;
                        bmp_alpha?.Dispose();
                        bmp_alpha = null;
                        Print(true);
                        down_hue = true;
                    }
                }
                else if (rect_alpha_big.Contains(x, y))
                {
                    //透明度
                    if (bmp_alpha_read != null)
                    {
                        point_alpha = x - gap;
                        if (point_alpha < 0) point_alpha = 0;
                        else if (point_alpha > bmp_alpha_read.Width - 1) point_alpha = bmp_alpha_read.Width - 1;
                        color_alpha = Value = Color.FromArgb(bmp_alpha_read.GetPixel(point_alpha, 1).A, ValueNAlpha);
                        SetValue();
                        Print(true);
                        down_alpha = true;
                    }
                }
                else if (AllowClear && rect_btn.Contains(x, y))
                {
                    if (PARENT is ColorPicker color && color.HasValue)
                    {
                        color.ClearValue();
                        Print();
                    }
                }
                else if (ShowClose && rect_close.Contains(x, y)) IClose();
                else if (ShowReset && rect_reset.Contains(x, y))
                {
                    if (Value != ValueDefault)
                    {
                        ChangeColor(ValueDefault);
                        SetValue();
                    }
                }
                if (preset_color == null) return;
                foreach (var it in preset_color)
                {
                    if (it.rect.Contains(x, y))
                    {
                        ChangeColor(it.color);
                        SetValue();
                        return;
                    }
                }
            }
        }

        protected override void OnMouseMove(MouseButtons button, int clicks, int x, int y, int delta)
        {
            int count = 0;
            if (AllowClear)
            {
                var hover = rect_btn.Contains(x, y);
                if (hover != hover_btn)
                {
                    hover_btn = hover;
                    count++;
                }
            }
            if (ShowClose)
            {
                var hover = rect_close.Contains(x, y);
                if (hover != hover_close)
                {
                    hover_close = hover;
                    count++;
                }
            }
            if (ShowReset)
            {
                var hover = rect_reset.Contains(x, y);
                if (hover != hover_reset)
                {
                    hover_reset = hover;
                    count++;
                }
            }
            if (preset_color != null)
            {
                foreach (var it in preset_color)
                {
                    if (it.rect.Contains(x, y))
                    {
                        if (it.hover) continue;
                        else
                        {
                            it.hover = true;
                            count++;
                        }
                    }
                    else if (it.hover)
                    {
                        it.hover = false;
                        count++;
                    }
                }
            }
            if (count > 0) Print();
            if (down_colors && colors_mouse != null)
            {
                var value = GetColors(x, y, colors_mouse);

                color_alpha = Value = Color.FromArgb(Value.A, value);
                ValueNAlpha = Color.FromArgb(255, Value);
                SetValue();
                bmp_alpha?.Dispose();
                bmp_alpha = null;
                Print(true);
            }
            else if (down_hue && bmp_hue != null)
            {
                point_hue = x - gap;
                if (point_hue < 0) point_hue = 0;
                else if (point_hue > bmp_hue.Width - 1) point_hue = bmp_hue.Width - 1;
                ValueHue = bmp_hue.GetPixel(point_hue, 1);

                var hsv = ValueHue.ToHSV();
                var hsv_value = Value.ToHSV();
                hsv_value.h = hsv.h;
                color_alpha = Value = Color.FromArgb(Value.A, hsv_value.HSVToColor());
                ValueNAlpha = Color.FromArgb(255, Value);
                SetValue();
                colors_mouse = null;
                bmp_alpha?.Dispose();
                bmp_alpha = null;
                Print(true);
            }
            else if (down_alpha && bmp_alpha_read != null)
            {
                point_alpha = x - gap;
                if (point_alpha < 0) point_alpha = 0;
                else if (point_alpha > bmp_alpha_read.Width - 1) point_alpha = bmp_alpha_read.Width - 1;
                color_alpha = Value = Color.FromArgb(bmp_alpha_read.GetPixel(point_alpha, 1).A, ValueNAlpha);
                SetValue();
                Print(true);
            }
        }

        Color GetColors(int x, int y, Dictionary<string, Color> dir)
        {
            point_colors = new Point(x - gap, y - offy - gap);

            int w = rect_colors.Width - 1, h = rect_colors.Height - 1;
            if (point_colors.X < 0) point_colors.X = 0;
            else if (point_colors.X > w) point_colors.X = w;
            if (point_colors.Y < 0) point_colors.Y = 0;
            else if (point_colors.Y > h) point_colors.Y = h;

            return dir[point_colors.X + "_" + point_colors.Y];
        }

        protected override void OnMouseUp(MouseButtons button, int clicks, int x, int y, int delta)
        {
            if (down_colors) down_colors = false;
            if (down_hue) down_hue = false;
            if (down_alpha) down_alpha = false;
        }

        #endregion

        #region 渲染

        bool hover_btn = false, hover_close = false, hover_reset;
        Rectangle rect_btn, rect_close, rect_reset;

        public override void PrintContent(Canvas g, Rectangle rect, GraphicsState state)
        {
            using (var brush_bg = new SolidBrush(Colour.BgElevated.Get(name, ColorScheme)))
            {
                #region 渲染

                if (AllowClear)
                {
                    using (var path = rect_btn.RoundPath(Radius2))
                    {
                        g.SetClip(path);
                        using (var pen = new Pen(Color.FromArgb(245, 34, 45), rect_btn.Height * .12F))
                        {
                            g.DrawLine(pen, new Point(rect_btn.X, rect_btn.Bottom), new Point(rect_btn.Right, rect_btn.Y));
                        }
                        g.ResetClip();
                        g.Draw(hover_btn ? Colour.BorderColor.Get(name, ColorScheme) : Colour.Split.Get(name, ColorScheme), Dpi, path);
                    }
                }

                if (ShowClose)
                {
                    using (var path = rect_close.RoundPath(Radius2))
                    {
                        g.Draw(hover_close ? Colour.BorderColor.Get(name, ColorScheme) : Colour.Split.Get(name, ColorScheme), Dpi, path);
                    }
                    g.PaintIconClose(rect_close, Colour.TextTertiary.Get(name, ColorScheme), .8F);
                }

                if (ShowReset)
                {
                    using (var path = rect_reset.RoundPath(Radius2))
                    {
                        g.Draw(hover_reset ? Colour.BorderColor.Get(name, ColorScheme) : Colour.Split.Get(name, ColorScheme), Dpi, path);
                    }
                    g.PaintIconReset(rect_reset, ValueDefault, .8F);
                }

                #region 调色板

                using (var bmp = new Bitmap(rect_colors.Width, rect_colors.Height))
                {
                    using (var g2 = Graphics.FromImage(bmp).High())
                    {
                        PaintColors(g2, new Rectangle(0, 0, bmp.Width, bmp.Height));
                    }
                    colors_mouse ??= GetColorsPoint(bmp);
                    g.Image(bmp, rect_colors);
                }
                using (var path = rect_colors.RoundPath(Radius))
                {
                    path.AddRectangle(new Rectangle(rect_colors.X - 1, rect_colors.Y - 1, rect_colors.Width + 2, rect_colors.Height + 2));
                    g.Fill(brush_bg, path);
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
                g.Image(bmp_hue, rect_hue);
                using (var path = rect_hue.RoundPath(rect_hue.Height))
                {
                    path.AddRectangle(new Rectangle(rect_hue.X - 1, rect_hue.Y - 1, rect_hue.Width + 2, rect_hue.Height + 2));
                    g.Fill(brush_bg, path);
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
                    g.Image(bmp_alpha, rect_alpha);

                    using (var path = rect_alpha.RoundPath(rect_alpha.Height))
                    {
                        path.AddRectangle(new Rectangle(rect_alpha.X - 1, rect_alpha.Y - 1, rect_alpha.Width + 2, rect_alpha.Height + 2));
                        g.Fill(brush_bg, path);
                    }
                }

                #endregion

                #region 渲染指标

                using (var brush_val = new SolidBrush(Value))
                using (var brush_hue = new SolidBrush(ValueHue))
                using (var pen = new Pen(Colour.BgBase.Get(name, ColorScheme), dot_bor_size))
                {
                    #region 调色板

                    var _rect_colors = new Rectangle(rect_colors.X + point_colors.X - dot_size / 2, rect_colors.Y + point_colors.Y - dot_size / 2, dot_size, dot_size);
                    g.FillEllipse(brush_val, _rect_colors);
                    g.DrawEllipse(pen, _rect_colors);

                    #endregion

                    #region 色相

                    if (bmp_dot_12 != null)
                    {
                        var _rect_hue = new Rectangle(rect_hue.X + point_hue - gap / 2, rect_hue.Y + rect_hue.Height / 2 - gap / 2, gap, gap);

                        g.Image(bmp_dot_12, new Rectangle(rect_hue.X + point_hue - bmp_dot_12.Height / 2, rect_hue.Y + (rect_hue.Height - bmp_dot_12.Height) / 2, bmp_dot_12.Width, bmp_dot_12.Height));
                        g.FillEllipse(brush_hue, _rect_hue);
                        g.DrawEllipse(pen, _rect_hue);
                    }

                    #endregion

                    #region 透明度

                    if (rect_alpha.Width > 0 && bmp_dot_12 != null)
                    {
                        brush_val.Color = color_alpha;
                        var _rect_alpha = new Rectangle(rect_alpha.X + point_alpha - gap / 2, rect_alpha.Y + rect_alpha.Height / 2 - gap / 2, gap, gap);

                        g.Image(bmp_dot_12, new Rectangle(rect_alpha.X + point_alpha - bmp_dot_12.Height / 2, rect_alpha.Y + (rect_alpha.Height - bmp_dot_12.Height) / 2, bmp_dot_12.Width, bmp_dot_12.Height));
                        g.FillEllipse(brush_val, _rect_alpha);
                        g.DrawEllipse(pen, _rect_alpha);
                    }

                    #endregion

                    using (var path = rect_color.RoundPath(Radius))
                    {
                        g.Fill(brush_val, path);
                    }
                }

                #endregion

                #region 色卡集

                if (preset_color != null)
                {
                    using (var pen = new Pen(Colour.TextQuaternary.Get(name, ColorScheme), Dpi))
                    {
                        foreach (var it in preset_color)
                        {
                            using (var path = it.rect.RoundPath(Radius2))
                            {
                                g.Fill(it.color, path);
                                g.Draw(pen, path);
                                if (it.hover)
                                {
                                    int size = dot_bor_size * 2, size2 = size * 2;
                                    var rect_color = new Rectangle(it.rect.X - size, it.rect.Y - size, it.rect.Width + size2, it.rect.Height + size2);
                                    using (var path_hove = rect_color.RoundPath(Radius2 + size))
                                    {
                                        g.Draw(pen, path_hove);
                                    }
                                }
                            }
                        }
                    }
                }

                #endregion

                #endregion
            }
            foreach (var input in inputs) input.input.IPaint(g, input.rect, input.rect_read);
        }

        public override void PrintBg(Canvas g, Rectangle rect, GraphicsPath path)
        {
            using (var brush = new SolidBrush(Colour.BgElevated.Get(name, ColorScheme)))
            {
                g.Fill(brush, path);
                if (shadow == 0)
                {
                    int bor = (int)(Dpi), bor2 = bor * 2;
                    using (var path2 = new Rectangle(rect.X + bor, rect.Y + bor, rect.Width - bor2, rect.Height - bor2).RoundPath(Radius))
                    {
                        g.Draw(Colour.BorderColor.Get(name, ColorScheme), bor, path2);
                    }
                    return;
                }
                if (ArrowLine != null) g.FillPolygon(brush, ArrowLine);
            }
        }

        class InputRect
        {
            public InputRect(Input _input, Rectangle _rect_read, int wsize, int wsize2)
            {
                input = _input;
                rect = new Rectangle(_rect_read.X - wsize, _rect_read.Y - wsize, _rect_read.Width + wsize2, _rect_read.Height + wsize2);
                rect_read = _rect_read;
                _input.RECTDIV = _rect_read;
            }
            public Input input { get; set; }
            public Rectangle rect { get; set; }
            public Rectangle rect_read { get; set; }
        }
        InputRect[] inputs;

        #region 渲染帮助

        Bitmap? bmp_dot_12;
        Rectangle rect_color;

        #region 取色器

        bool down_colors = false;
        Point point_colors = Point.Empty;
        Rectangle rect_colors_big;
        Rectangle rect_colors;
        Dictionary<string, Color>? colors_mouse;
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
        Dictionary<string, Color> GetColorsPoint(Bitmap bmp_colors)
        {
            int w = bmp_colors.Width, h = bmp_colors.Height;
            var list = new Dictionary<string, Color>(w * h);
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    try
                    {
                        var value = bmp_colors.GetPixel(x, y);
                        list.Add(x + "_" + y, value);
                        if (value == ValueNAlpha) point_colors = new Point(x, y);
                    }
                    catch { return list; }
                }
            }
            return list;
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
                using (var brush = new SolidBrush(Colour.FillSecondary.Get(name, ColorScheme)))
                {
                    int he = rect.Height / 2;
                    int u_x = 0;
                    bool ad = false;
                    while (u_x < rect.Width)
                    {
                        ad = !ad;
                        g.Fill(brush, new Rectangle(u_x, ad ? 0 : he, he, he));
                        u_x += he;
                    }
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

        protected override void Dispose(bool disposing)
        {
            bmp_hue?.Dispose();
            bmp_hue = null;
            bmp_alpha?.Dispose();
            bmp_alpha = null;
            bmp_alpha_read?.Dispose();
            bmp_alpha_read = null;
            bmp_dot_12?.Dispose();
            bmp_dot_12 = null;
            if (Dfont) Font.Dispose();
            base.Dispose(disposing);
        }

        #endregion

        public void IProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)
        {
            foreach (var input in inputs)
            {
                if (input.input.Focused) input.input.HandKeyBoard(keyData);
            }
        }

        public void IKeyPress(KeyPressEventArgs e)
        {
            foreach (var input in inputs)
            {
                if (input.input.Focused)
                {
                    input.input.IKeyPress(e.KeyChar);
                    return;
                }
            }
        }
    }
}