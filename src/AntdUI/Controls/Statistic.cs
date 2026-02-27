// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// Statistic 统计数值
    /// </summary>
    /// <remarks>展示统计数值。</remarks>
    [Description("Statistic 统计数值")]
    [ToolboxItem(true)]
    public class Statistic : IControl
    {
        #region 属性

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
                OnPropertyChanged(nameof(Radius));
            }
        }

        #region 背景

        Color? back;
        /// <summary>
        /// 背景颜色
        /// </summary>
        [Description("背景颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? Back
        {
            get => back;
            set
            {
                if (back == value) return;
                back = value;
                Invalidate();
                OnPropertyChanged(nameof(Back));
            }
        }

        string? backExtend;
        /// <summary>
        /// 背景渐变色
        /// </summary>
        [Description("背景渐变色"), Category("外观"), DefaultValue(null)]
        public string? BackExtend
        {
            get => backExtend;
            set
            {
                if (backExtend == value) return;
                backExtend = value;
                Invalidate();
                OnPropertyChanged(nameof(BackExtend));
            }
        }

        Image? backImage;
        /// <summary>
        /// 背景图片
        /// </summary>
        [Description("背景图片"), Category("外观"), DefaultValue(null)]
        public new Image? BackgroundImage
        {
            get => backImage;
            set
            {
                if (backImage == value) return;
                backImage = value;
                Invalidate();
                OnPropertyChanged(nameof(BackgroundImage));
            }
        }

        TFit backFit = TFit.Fill;
        /// <summary>
        /// 背景图片布局
        /// </summary>
        [Description("背景图片布局"), Category("外观"), DefaultValue(TFit.Fill)]
        public new TFit BackgroundImageLayout
        {
            get => backFit;
            set
            {
                if (backFit == value) return;
                backFit = value;
                Invalidate();
                OnPropertyChanged(nameof(BackgroundImageLayout));
            }
        }

        #endregion

        #region 边框

        float borderWidth = 1F;
        /// <summary>
        /// 边框宽度
        /// </summary>
        [Description("边框宽度"), Category("边框"), DefaultValue(1F)]
        public float BorderWidth
        {
            get => borderWidth;
            set
            {
                if (borderWidth == value) return;
                borderWidth = value;
                IOnSizeChanged();
                OnPropertyChanged(nameof(BorderWidth));
            }
        }

        Color? borderColor;
        /// <summary>
        /// 边框颜色
        /// </summary>
        [Description("边框颜色"), Category("边框"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? BorderColor
        {
            get => borderColor;
            set
            {
                if (borderColor == value) return;
                borderColor = value;
                if (borderWidth > 0) Invalidate();
                OnPropertyChanged(nameof(BorderColor));
            }
        }

        DashStyle borderStyle = DashStyle.Solid;
        /// <summary>
        /// 边框样式
        /// </summary>
        [Description("边框样式"), Category("边框"), DefaultValue(DashStyle.Solid)]
        public DashStyle BorderStyle
        {
            get => borderStyle;
            set
            {
                if (borderStyle == value) return;
                borderStyle = value;
                if (borderWidth > 0) Invalidate();
                OnPropertyChanged(nameof(BorderStyle));
            }
        }

        #endregion

        #region 比例

        /// <summary>
        /// 文本高度比例
        /// </summary>
        [Description("文本高度比例"), Category("外观"), DefaultValue(null)]
        public float? TextRatio { get; set; }

        /// <summary>
        /// 值高度比例
        /// </summary>
        [Description("值高度比例"), Category("外观"), DefaultValue(null)]
        public float? ValueRatio { get; set; }

        /// <summary>
        /// 图表高度比例
        /// </summary>
        [Description("图表高度比例"), Category("外观"), DefaultValue(null)]
        public float? ChartLineRatio { get; set; }

        #endregion

        string? _value;
        [Description("数值内容"), Category("外观"), DefaultValue(null)]
        public string? Value
        {
            get => this.GetLangI(LocalizationValue, _value);
            set
            {
                if (_value == value) return;
                _value = value;
                Invalidate();
            }
        }

        [Description("文本"), Category("国际化"), DefaultValue(null)]
        public string? LocalizationText { get; set; }

        [Description("数值内容"), Category("国际化"), DefaultValue(null)]
        public string? LocalizationValue { get; set; }

        [Description("图表饼图数据"), Category("数据"), DefaultValue(null)]
        public float? ChartPieData { get; set; }

        [Description("图表折线图数据"), Category("数据"), DefaultValue(null)]
        public double[]? ChartLineData { get; set; }

        /// <summary>
        /// 图表饼图背景颜色
        /// </summary>
        [Description("图表饼图背景颜色"), Category("图表折线图"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? ChartPieBg { get; set; }

        /// <summary>
        /// 图表饼图颜色
        /// </summary>
        [Description("图表饼图颜色"), Category("图表折线图"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? ChartPieColor { get; set; }

        /// <summary>
        /// 图表折线图颜色
        /// </summary>
        [Description("图表折线图颜色"), Category("图表折线图"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? ChartLineColor { get; set; }

        #endregion

        #region 渲染

        FormatFlags sf = FormatFlags.VerticalCenter | FormatFlags.Left | FormatFlags.NoWrapEllipsis;
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics.High();
            var bor = borderWidth * Dpi;
            var rect = ClientRectangle.PaddingRect(Padding, bor / 2F);
            var size = g.MeasureString(Config.NullText, Font);
            int gap = (int)(size.Height * 0.18F), gap2 = gap * 2, gapbig = gap * gap;
            using (var font = new Font(Font.FontFamily.Name, Font.Size * 2.2F))
            {
                var text = this.GetLangI(LocalizationText, Text);
                Size size_text = g.MeasureString(text, Font), size_value = g.MeasureString(Value, font);
                if (ChartLineData == null)
                {
                    float v_height_total = size_text.Height + size_value.Height + gap, v_height_text = TextRatio ?? ((size_text.Height + gap) / v_height_total), v_height_value = ValueRatio ?? (size_value.Height / v_height_total);
                    PaintBase(g, rect, font, text, gap, v_height_text, v_height_value, size_text.Height, size_value.Height, bor);
                }
                else
                {
                    int chart_h = (int)(size.Height * (ChartLineRatio ?? 2.08F));
                    float v_height_total = size_text.Height + size_value.Height + chart_h + gap2, v_height_text = TextRatio ?? ((ValueRatio ?? size_text.Height + gap) / v_height_total),
                        v_height_value = ValueRatio ?? (size_value.Height / v_height_total), v_height_chart = (chart_h / v_height_total);
                    int h_chart = (int)(rect.Height * v_height_chart);
                    var rect_tmp = PaintBase(g, rect, font, text, gap, v_height_text, v_height_value, size_text.Height, size_value.Height, bor);
                    var rect_chart = new Rectangle(rect_tmp.X, rect_tmp.Bottom - gap, rect_tmp.Width, h_chart);
                    using (var brush = new LinearGradientBrush(rect_chart, ChartLineColor ?? Colour.Primary.Get(nameof(Statistic), ColorScheme), Color.Transparent, 90F))
                    {
                        double max = 0;
                        foreach (var it in ChartLineData)
                        {
                            if (it > max) max = it;
                        }
                        float step = rect_chart.Width * 1F / ChartLineData.Length, c = 0;
                        var points = new List<PointF>(ChartLineData.Length + 4);
                        foreach (var it in ChartLineData)
                        {
                            points.Add(new PointF(rect_tmp.X + c, rect.Bottom - (float)(rect_chart.Height * (it / max))));
                            c += step;
                        }
                        points.Insert(0, new PointF(rect_tmp.X - gap2, rect.Bottom));
                        points.Insert(0, new PointF(rect_tmp.X - gap, points[0].Y));
                        points.Add(new PointF(rect_tmp.Right + gap, points[points.Count - 1].Y));
                        points.Add(new PointF(rect_tmp.Right + gap2, rect.Bottom));
                        g.SetClip(rect_chart);
                        g.FillClosedCurve(brush, points.ToArray());
                        g.ResetClip();
                    }
                }
            }
        }

        Rectangle PaintBase(Canvas g, Rectangle rect, Font font, string? text, int gap, float v_height_text, float v_height_value, int text_h, int value_h, float bor)
        {
            float _radius = radius * Dpi;
            using (var path = rect.RoundPath(radius))
            {
                using (var brush = backExtend.BrushEx(rect, back ?? Colour.BgContainer.Get(nameof(Statistic), ColorScheme)))
                {
                    g.Fill(brush, path);
                }
                if (backImage != null) g.Image(rect, backImage, backFit, _radius, false);

                int h_text = (int)(rect.Height * v_height_text), h_value = (int)(rect.Height * v_height_value), padd = h_text - text_h;
                int use_x = 0;
                if (ChartPieData != null)
                {
                    int padd_2 = padd / 2, pie_w = (int)(text_h * 0.6F), pie_w2 = pie_w / 2, pie_size = text_h + value_h + gap + padd;
                    var rect_pie = new Rectangle(rect.X + padd + pie_w2, rect.Y + padd_2 + pie_w2, pie_size - pie_w, pie_size - pie_w);

                    g.DrawEllipse(ChartPieBg ?? Colour.FillSecondary.Get(nameof(Statistic), ColorScheme), pie_w, rect_pie);

                    if (ChartPieData.Value > 0)
                    {
                        var max = (int)Math.Round(360 * ChartPieData.Value);
                        using (var brush = new Pen(ChartPieColor ?? Colour.Primary.Get(nameof(Statistic), ColorScheme), pie_w))
                        {
                            //brush.StartCap = brush.EndCap = LineCap.Round;
                            g.DrawArc(brush, rect_pie, -90, max);
                        }
                    }
                    use_x += pie_size + padd;
                }
                var rect_real = new Rectangle(rect.X + padd + use_x, rect.Y, rect.Width - padd * 2 - use_x, rect.Height);
                var rect_text = new Rectangle(rect_real.X, rect_real.Y, rect_real.Width, h_text);
                var rect_value = new Rectangle(rect_real.X, rect_real.Y + rect_text.Height + gap, rect_real.Width, h_value);
                g.String(text, Font, Colour.TextTertiary.Get(nameof(Statistic), ColorScheme), rect_text, sf);
                g.String(Value, font, Colour.TextBase.Get(nameof(Statistic), ColorScheme), rect_value, sf);
                if (bor > 0)
                {
                    using (var pen = new Pen(borderColor ?? Colour.BorderColor.Get(nameof(Statistic), ColorScheme), bor))
                    {
                        pen.DashStyle = borderStyle;
                        if (ChartPieData == null) g.DrawLine(pen, rect.X, rect_text.Bottom, rect.Right, rect_text.Bottom);
                        g.Draw(pen, path);
                    }
                }
                return rect_value;
            }
        }

        #endregion
    }
}