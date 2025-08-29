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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;

namespace AntdUI.Controls.Charts
{
    /// <summary>
    /// 图例项
    /// </summary>
    public class LegendItem
    {
        public string Label { get; set; } = string.Empty;
        public Color Color { get; set; }
    }

    /// <summary>
    /// Chart 图表控件
    /// </summary>
    /// <remarks>用于显示各种类型的数据图表，支持多种图表类型和交互功能。</remarks>
    [Description("Chart 图表控件")]
    [ToolboxItem(true)]
    [DefaultProperty("ChartType")]
    public class Chart : IControl, IEventListener
    {
        private System.Windows.Forms.Timer? animationTimer;
        private TooltipForm? tooltipForm;

        public Chart() : base(ControlType.Default)
        {
            // 初始化动画定时器
            animationTimer = new System.Windows.Forms.Timer();
            animationTimer.Interval = 16; // 60 FPS
            animationTimer.Tick += AnimationTimer_Tick;

            // 初始化工具提示
            // tooltipForm将在需要时动态创建

            // 注册主题变化事件
            EventHub.AddListener(this);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) animationTimer?.Dispose();
            base.Dispose(disposing);
        }

        #region 属性

        TChartType chartType = TChartType.Line;
        /// <summary>
        /// 图表类型
        /// </summary>
        [Description("图表类型"), Category("图表"), DefaultValue(TChartType.Line)]
        public TChartType ChartType
        {
            get => chartType;
            set
            {
                if (chartType == value) return;
                chartType = value;
                StartAnimation(); // 启动动画
                Invalidate();
                OnPropertyChanged(nameof(ChartType));
            }
        }

        /// <summary>
        /// 数据集集合
        /// </summary>
        [Description("数据集集合"), Category("数据")]
        [Editor(typeof(Design.CollectionEditor), typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public List<ChartDataset> Datasets { get; set; } = new List<ChartDataset>();

        /// <summary>
        /// 标题
        /// </summary>
        [Description("标题"), Category("外观")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string? Title { get; set; }

        /// <summary>
        /// 标题字体
        /// </summary>
        [Description("标题字体"), Category("外观")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Font? TitleFont { get; set; }

        /// <summary>
        /// 标题颜色
        /// </summary>
        [Description("标题颜色"), Category("外观")]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color? TitleColor { get; set; }

        /// <summary>
        /// 显示图例
        /// </summary>
        [Description("显示图例"), Category("外观"), DefaultValue(true)]
        public bool ShowLegend { get; set; } = true;

        /// <summary>
        /// 图例位置
        /// </summary>
        [Description("图例位置"), Category("外观"), DefaultValue(ContentAlignment.TopRight)]
        public ContentAlignment LegendPosition { get; set; } = ContentAlignment.TopRight;

        /// <summary>
        /// 显示网格线
        /// </summary>
        [Description("显示网格线"), Category("外观"), DefaultValue(true)]
        public bool ShowGrid { get; set; } = true;

        /// <summary>
        /// 网格线颜色
        /// </summary>
        [Description("网格线颜色"), Category("外观")]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color? GridColor { get; set; }

        /// <summary>
        /// 显示坐标轴
        /// </summary>
        [Description("显示坐标轴"), Category("外观"), DefaultValue(true)]
        public bool ShowAxes { get; set; } = true;

        /// <summary>
        /// 坐标轴颜色
        /// </summary>
        [Description("坐标轴颜色"), Category("外观")]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color? AxisColor { get; set; }

        /// <summary>
        /// 显示工具提示
        /// </summary>
        [Description("显示工具提示"), Category("行为"), DefaultValue(true)]
        public bool ShowTooltip { get; set; } = true;

        /// <summary>
        /// 启用动画
        /// </summary>
        [Description("启用动画"), Category("行为"), DefaultValue(true)]
        public bool EnableAnimation { get; set; } = true;

        /// <summary>
        /// 动画持续时间（毫秒）
        /// </summary>
        [Description("动画持续时间（毫秒）"), Category("行为"), DefaultValue(1000)]
        public int AnimationDuration { get; set; } = 1000;

        /// <summary>
        /// 内边距
        /// </summary>
        [Description("内边距"), Category("布局"), DefaultValue(20)]
        public new int Padding { get; set; } = 20;

        /// <summary>
        /// 图例内边距
        /// </summary>
        [Description("图例内边距"), Category("布局"), DefaultValue(10)]
        public int LegendPadding { get; set; } = 10;

        /// <summary>
        /// 图例背景颜色
        /// </summary>
        [Description("图例背景颜色"), Category("外观")]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color? LegendBackColor { get; set; }

        /// <summary>
        /// 图例边框颜色
        /// </summary>
        [Description("图例边框颜色"), Category("外观")]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color? LegendBorderColor { get; set; }

        /// <summary>
        /// 饼图颜色方案
        /// </summary>
        [Description("饼图颜色方案"), Category("外观")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color[]? PieColors { get; set; }

        /// <summary>
        /// 动画进度 (0-1)
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public float AnimationProgress { get; private set; } = 1.0f;

        /// <summary>
        /// 当前悬停的数据点
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ChartDataPoint? HoveredPoint { get; private set; }

        #endregion

        #region 事件

        /// <summary>
        /// 数据点点击事件
        /// </summary>
        [Description("数据点点击事件"), Category("行为")]
        public event EventHandler<ChartPointClickEventArgs>? PointClick;

        /// <summary>
        /// 图表区域点击事件
        /// </summary>
        [Description("图表区域点击事件"), Category("行为")]
        public event EventHandler<ChartAreaClickEventArgs>? AreaClick;

        /// <summary>
        /// 数据点悬停事件
        /// </summary>
        [Description("数据点悬停事件"), Category("行为")]
        public event EventHandler<ChartPointHoverEventArgs>? PointHover;

        #endregion

        #region 动画和工具提示

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            if (EnableAnimation && Config.Animation)
            {
                AnimationProgress += 0.05f;
                if (AnimationProgress >= 1.0f)
                {
                    AnimationProgress = 1.0f;
                    animationTimer?.Stop();
                }
                Invalidate();
            }
        }

        private void StartAnimation()
        {
            if (EnableAnimation && Config.Animation)
            {
                AnimationProgress = 0.0f;
                animationTimer?.Start();
            }
        }

        private void ShowTooltipInternal(ChartDataPoint point, Point location)
        {
            if (!ShowTooltip) return;

            // 先关闭现有的tooltip
            HideTooltip();

            var tooltipText = $"{point.Label}\nX: {point.X:F2}\nY: {point.Y:F2}";
            if (point.Radius > 0)
            {
                tooltipText += $"\nSize: {point.Radius:F2}";
            }

            // 将控件坐标转换为屏幕坐标
            var screenLocation = location;

            // 创建tooltip配置，使用屏幕坐标
            var config = new Tooltip.Config(this, tooltipText)
            {
                Offset = new Rectangle(screenLocation.X, screenLocation.Y, 1, 1),
                Font = Font,
                ArrowAlign = TAlign.Top
            };

            // 创建并显示tooltip
            tooltipForm = new TooltipForm(this, tooltipText, config);
            tooltipForm.Show(this);
        }

        private void HideTooltip()
        {
            tooltipForm?.Close();
            tooltipForm = null;
        }

        #endregion

        #region IEventListener 实现

        public void HandleEvent(EventType id, object? tag)
        {
            if (id == EventType.THEME)
            {
                // 主题变化时重新绘制
                Invalidate();
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 添加数据集
        /// </summary>
        /// <param name="dataset">数据集</param>
        public void AddDataset(ChartDataset dataset)
        {
            Datasets.Add(dataset);
            Invalidate();
        }

        /// <summary>
        /// 移除数据集
        /// </summary>
        /// <param name="dataset">数据集</param>
        public void RemoveDataset(ChartDataset dataset)
        {
            Datasets.Remove(dataset);
            Invalidate();
        }

        /// <summary>
        /// 清除所有数据集
        /// </summary>
        public void ClearDatasets()
        {
            Datasets.Clear();
            Invalidate();
        }

        /// <summary>
        /// 刷新图表
        /// </summary>
        public void RefreshChart()
        {
            Invalidate();
        }

        /// <summary>
        /// 导出图表为PNG图片
        /// </summary>
        /// <returns>图片对象</returns>
        public Bitmap? ExportChart()
        {
            return ExportChart(ImageFormat.Png);
        }

        /// <summary>
        /// 导出图表为指定格式的图片
        /// </summary>
        /// <param name="format">图片格式</param>
        /// <returns>图片对象</returns>
        public Bitmap? ExportChart(ImageFormat format)
        {
            try
            {
                var bitmap = new Bitmap(Width, Height);
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                    DrawChart(graphics, new Rectangle(0, 0, Width, Height));
                }
                return bitmap;
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region 重写方法

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            var rect = ClientRectangle;
            if (rect.Width <= 0 || rect.Height <= 0) return;

            DrawChart(g, rect);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            HandleMouseClick(e.Location);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (ShowTooltip)
            {
                HandleMouseMove(e.Location);
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            HideTooltip();
        }

        #endregion

        #region 私有方法

        private void DrawChart(Graphics g, Rectangle rect)
        {
            // 绘制背景
            var backColor = Config.IsDark ? Color.FromArgb(30, 30, 30) : BackColor;
            using (var brush = new SolidBrush(backColor))
            {
                g.FillRectangle(brush, rect);
            }

            // 计算图表区域
            var chartRect = CalculateChartRect(rect);

            // 绘制标题
            if (!string.IsNullOrEmpty(Title))
            {
                DrawTitle(g, rect, chartRect);
            }

            // 绘制图例
            if (ShowLegend && Datasets.Count > 0)
            {
                DrawLegend(g, rect, chartRect);
            }

            // 绘制网格
            if (ShowGrid)
            {
                DrawGrid(g, chartRect);
            }

            // 绘制坐标轴
            if (ShowAxes)
            {
                DrawAxes(g, chartRect);
            }

            // 绘制图表数据
            DrawChartData(g, chartRect);
        }

        private Rectangle CalculateChartRect(Rectangle rect)
        {
            var chartRect = rect;
            chartRect.Inflate(-Padding, -Padding);

            // 为标题留出空间
            if (!string.IsNullOrEmpty(Title))
            {
                var titleHeight = TitleFont?.Height ?? Font.Height;
                chartRect.Y += titleHeight + 10;
                chartRect.Height -= titleHeight + 10;
            }

            // 为图例留出空间
            if (ShowLegend && Datasets.Count > 0)
            {
                var legendHeight = CalculateLegendHeight();
                switch (LegendPosition)
                {
                    case ContentAlignment.TopLeft:
                    case ContentAlignment.TopCenter:
                    case ContentAlignment.TopRight:
                        chartRect.Y += legendHeight + LegendPadding;
                        chartRect.Height -= legendHeight + LegendPadding;
                        break;
                    case ContentAlignment.BottomLeft:
                    case ContentAlignment.BottomCenter:
                    case ContentAlignment.BottomRight:
                        chartRect.Height -= legendHeight + LegendPadding;
                        break;
                }
            }

            return chartRect;
        }

        private void DrawTitle(Graphics g, Rectangle rect, Rectangle chartRect)
        {
            if (string.IsNullOrEmpty(Title)) return;

            var titleFont = TitleFont ?? Font;
            var titleColor = TitleColor.HasValue ? TitleColor.Value : (Config.IsDark ? Color.White : ForeColor);
            var titleBrush = new SolidBrush(titleColor);

            var titleSize = g.MeasureString(Title, titleFont);
            var titleX = rect.X + (rect.Width - titleSize.Width) / 2;
            var titleY = rect.Y + 5;

            g.DrawString(Title, titleFont, titleBrush, titleX, titleY);
        }

        private void DrawLegend(Graphics g, Rectangle rect, Rectangle chartRect)
        {
            if (Datasets.Count == 0) return;

            var legendItems = new List<LegendItem>();
            foreach (var dataset in Datasets)
            {
                if (dataset.Visible)
                {
                    var color = dataset.LegendBoxFillColor.HasValue ? dataset.LegendBoxFillColor.Value : (dataset.FillColor.HasValue ? dataset.FillColor.Value : Color.Gray);
                    legendItems.Add(new LegendItem { Label = dataset.Label ?? "数据集", Color = color });
                }
            }

            if (legendItems.Count == 0) return;

            // 计算图例尺寸
            var legendSize = CalculateLegendSize(g, legendItems);
            var legendRect = CalculateLegendRect(rect, legendSize);

            // 绘制图例背景
            var legendBackColor = LegendBackColor ?? (Config.IsDark ? Color.FromArgb(45, 45, 45) : Color.FromArgb(240, 240, 240));
            using (var brush = new SolidBrush(legendBackColor))
            {
                g.FillRectangle(brush, legendRect);
            }

            // 绘制图例边框
            var legendBorderColor = LegendBorderColor ?? (Config.IsDark ? Color.FromArgb(70, 70, 70) : Color.LightGray);
            using (var pen = new Pen(legendBorderColor))
            {
                g.DrawRectangle(pen, legendRect);
            }

            // 绘制图例项
            var itemSpacing = 5;
            var currentY = legendRect.Y + 5;

            foreach (var item in legendItems)
            {
                // 计算文本尺寸
                var textSize = g.MeasureString(item.Label, Font);
                var itemHeight = Math.Max(15, (int)textSize.Height); // 最小高度15像素

                // 绘制颜色框
                var colorRect = new Rectangle(legendRect.X + 5, currentY + (itemHeight - 15) / 2, 15, 15);
                using (var brush = new SolidBrush(item.Color))
                {
                    g.FillRectangle(brush, colorRect);
                }
                using (var pen = new Pen(Config.IsDark ? Color.White : Color.Black))
                {
                    g.DrawRectangle(pen, colorRect);
                }

                // 绘制标签
                var textColor = Config.IsDark ? Color.White : ForeColor;
                using (var brush = new SolidBrush(textColor))
                {
                    g.DrawString(item.Label, Font, brush, legendRect.X + 25, currentY + (itemHeight - textSize.Height) / 2);
                }

                currentY += itemHeight + itemSpacing;
            }
        }

        private Rectangle CalculateLegendRect(Rectangle rect, Size legendSize)
        {
            var legendWidth = legendSize.Width;
            var legendHeight = legendSize.Height;
            var legendX = 0;
            var legendY = 0;

            switch (LegendPosition)
            {
                case ContentAlignment.TopLeft:
                    legendX = rect.X + 5;
                    legendY = rect.Y + 5;
                    break;
                case ContentAlignment.TopCenter:
                    legendX = rect.X + (rect.Width - legendWidth) / 2;
                    legendY = rect.Y + 5;
                    break;
                case ContentAlignment.TopRight:
                    legendX = rect.Right - legendWidth - 5;
                    legendY = rect.Y + 5;
                    break;
                case ContentAlignment.MiddleLeft:
                    legendX = rect.X + 5;
                    legendY = rect.Y + (rect.Height - legendHeight) / 2;
                    break;
                case ContentAlignment.MiddleCenter:
                    legendX = rect.X + (rect.Width - legendWidth) / 2;
                    legendY = rect.Y + (rect.Height - legendHeight) / 2;
                    break;
                case ContentAlignment.MiddleRight:
                    legendX = rect.Right - legendWidth - 5;
                    legendY = rect.Y + (rect.Height - legendHeight) / 2;
                    break;
                case ContentAlignment.BottomLeft:
                    legendX = rect.X + 5;
                    legendY = rect.Bottom - legendHeight - 5;
                    break;
                case ContentAlignment.BottomCenter:
                    legendX = rect.X + (rect.Width - legendWidth) / 2;
                    legendY = rect.Bottom - legendHeight - 5;
                    break;
                case ContentAlignment.BottomRight:
                    legendX = rect.Right - legendWidth - 5;
                    legendY = rect.Bottom - legendHeight - 5;
                    break;
            }

            return new Rectangle(legendX, legendY, legendWidth, legendHeight);
        }

        private Size CalculateLegendSize(Graphics g, List<LegendItem> legendItems)
        {
            var itemSpacing = 5;
            var colorBoxWidth = 15;
            var colorBoxSpacing = 10;
            var padding = 5;

            // 计算最大文本宽度和总高度
            var maxTextWidth = 0;
            var totalHeight = padding;

            foreach (var item in legendItems)
            {
                var textSize = g.MeasureString(item.Label, Font);
                maxTextWidth = Math.Max(maxTextWidth, (int)textSize.Width);
                var itemHeight = Math.Max(15, (int)textSize.Height);
                totalHeight += itemHeight + itemSpacing;
            }

            // 减去最后一个间距
            if (legendItems.Count > 0)
            {
                totalHeight -= itemSpacing;
            }
            totalHeight += padding;

            // 计算总宽度：左边距 + 颜色框宽度 + 间距 + 文本宽度 + 右边距
            var totalWidth = padding + colorBoxWidth + colorBoxSpacing + maxTextWidth + padding;

            return new Size(totalWidth, totalHeight);
        }

        private int CalculateLegendHeight()
        {
            var itemHeight = 20;
            var itemSpacing = 5;
            var visibleCount = Datasets.Count(d => d.Visible);
            return visibleCount * (itemHeight + itemSpacing) + 10;
        }

        private void DrawGrid(Graphics g, Rectangle chartRect)
        {
            var defaultGridColor = Config.IsDark ? Color.FromArgb(60, 60, 60) : Color.LightGray;
            var gridColor = GridColor.HasValue ? GridColor.Value : defaultGridColor;
            using (var pen = new Pen(gridColor, 1))
            {
                pen.DashStyle = DashStyle.Dot;

                // 绘制垂直网格线
                var stepX = chartRect.Width / 10;
                for (int i = 1; i < 10; i++)
                {
                    var x = chartRect.X + i * stepX;
                    g.DrawLine(pen, x, chartRect.Y, x, chartRect.Bottom);
                }

                // 绘制水平网格线
                var stepY = chartRect.Height / 10;
                for (int i = 1; i < 10; i++)
                {
                    var y = chartRect.Y + i * stepY;
                    g.DrawLine(pen, chartRect.X, y, chartRect.Right, y);
                }
            }
        }

        private void DrawAxes(Graphics g, Rectangle chartRect)
        {
            var defaultAxisColor = Config.IsDark ? Color.FromArgb(80, 80, 80) : Color.LightGray;
            var axisColor = AxisColor.HasValue ? AxisColor.Value : defaultAxisColor;
            using (var pen = new Pen(axisColor, 2))
            {
                // X轴
                g.DrawLine(pen, chartRect.X, chartRect.Bottom, chartRect.Right, chartRect.Bottom);
                // Y轴
                g.DrawLine(pen, chartRect.X, chartRect.Y, chartRect.X, chartRect.Bottom);
            }
        }

        private void DrawChartData(Graphics g, Rectangle chartRect)
        {
            switch (ChartType)
            {
                case TChartType.Line:
                    DrawLineChart(g, chartRect);
                    break;
                case TChartType.Bar:
                    DrawBarChart(g, chartRect);
                    break;
                case TChartType.Area:
                    DrawAreaChart(g, chartRect);
                    break;
                case TChartType.Pie:
                    DrawPieChart(g, chartRect);
                    break;
                case TChartType.Doughnut:
                    DrawDoughnutChart(g, chartRect);
                    break;
                case TChartType.Scatter:
                    DrawScatterChart(g, chartRect);
                    break;
                case TChartType.Bubble:
                    DrawBubbleChart(g, chartRect);
                    break;
                case TChartType.Radar:
                    DrawRadarChart(g, chartRect);
                    break;
                case TChartType.PolarArea:
                    DrawPolarAreaChart(g, chartRect);
                    break;
                case TChartType.Spline:
                    DrawSplineChart(g, chartRect);
                    break;
                case TChartType.SplineArea:
                    DrawSplineAreaChart(g, chartRect);
                    break;
                case TChartType.HorizontalBar:
                    DrawHorizontalBarChart(g, chartRect);
                    break;
                case TChartType.StackedBar:
                    DrawStackedBarChart(g, chartRect);
                    break;
                case TChartType.StackedHorizontalBar:
                    DrawStackedHorizontalBarChart(g, chartRect);
                    break;
                case TChartType.SteppedLine:
                    DrawSteppedLineChart(g, chartRect);
                    break;
                case TChartType.SteppedArea:
                    DrawSteppedAreaChart(g, chartRect);
                    break;
            }
        }

        private void HandleMouseClick(Point location)
        {
            // 实现鼠标点击处理逻辑
            var chartRect = CalculateChartRect(ClientRectangle);
            if (chartRect.Contains(location))
            {
                var point = FindNearestDataPoint(location, chartRect);
                if (point != null)
                {
                    PointClick?.Invoke(this, new ChartPointClickEventArgs(point));
                }
                else
                {
                    AreaClick?.Invoke(this, new ChartAreaClickEventArgs(location));
                }
            }
        }

        private void HandleMouseMove(Point location)
        {
            if (!ShowTooltip) return;

            var chartRect = CalculateChartRect(ClientRectangle);
            if (chartRect.Contains(location))
            {
                var point = FindNearestDataPointForHover(location, chartRect);
                if (point != null)
                {
                    // 只有当悬停的数据点发生变化时才更新tooltip
                    if (HoveredPoint != point)
                    {
                        HoveredPoint = point;
                        ShowTooltipInternal(point, location);
                        PointHover?.Invoke(this, new ChartPointHoverEventArgs(point, location));
                    }
                }
                else
                {
                    // 没有找到数据点时隐藏tooltip
                    if (HoveredPoint != null)
                    {
                        HoveredPoint = null;
                        HideTooltip();
                    }
                }
            }
            else
            {
                // 鼠标移出图表区域时隐藏tooltip
                if (HoveredPoint != null)
                {
                    HoveredPoint = null;
                    HideTooltip();
                }
            }
        }

        private ChartDataPoint? FindNearestDataPointForHover(Point location, Rectangle chartRect)
        {
            if (Datasets.Count == 0) return null;

            var minDistance = double.MaxValue;
            ChartDataPoint? nearestPoint = null;

            foreach (var dataset in Datasets.Where(d => d.Visible))
            {
                for (int i = 0; i < dataset.DataPoints.Count; i++)
                {
                    var point = dataset.DataPoints[i];
                    if (!point.Visible) continue;

                    var pointLocation = GetDataPointLocation(point, chartRect);
                    var distance = Math.Sqrt(Math.Pow(location.X - pointLocation.X, 2) + Math.Pow(location.Y - pointLocation.Y, 2));

                    if (distance < minDistance && distance < 10) // 10像素的悬停范围
                    {
                        minDistance = distance;
                        nearestPoint = point;
                    }
                }
            }

            return nearestPoint;
        }

        private ChartDataPoint? FindNearestDataPoint(Point location, Rectangle chartRect)
        {
            if (Datasets.Count == 0) return null;

            var minDistance = double.MaxValue;
            ChartDataPoint? nearestPoint = null;
            var nearestDatasetIndex = -1;
            var nearestPointIndex = -1;

            foreach (var dataset in Datasets.Where(d => d.Visible))
            {
                for (int i = 0; i < dataset.DataPoints.Count; i++)
                {
                    var point = dataset.DataPoints[i];
                    if (!point.Visible) continue;

                    var pointLocation = GetDataPointLocation(point, chartRect);
                    var distance = Math.Sqrt(Math.Pow(location.X - pointLocation.X, 2) + Math.Pow(location.Y - pointLocation.Y, 2));

                    if (distance < minDistance && distance < 10) // 10像素的点击范围
                    {
                        minDistance = distance;
                        nearestPoint = point;
                        nearestDatasetIndex = Datasets.IndexOf(dataset);
                        nearestPointIndex = i;
                    }
                }
            }

            if (nearestPoint != null)
            {
                PointClick?.Invoke(this, new ChartPointClickEventArgs(nearestPoint, nearestDatasetIndex, nearestPointIndex));
            }

            return nearestPoint;
        }

        private Point GetDataPointLocation(ChartDataPoint point, Rectangle chartRect)
        {
            // 根据图表类型计算数据点的屏幕坐标
            switch (ChartType)
            {
                case TChartType.Line:
                case TChartType.Area:
                case TChartType.Spline:
                case TChartType.SplineArea:
                case TChartType.SteppedLine:
                case TChartType.SteppedArea:
                    return GetLineChartPointLocation(point, chartRect);
                case TChartType.Bar:
                case TChartType.HorizontalBar:
                case TChartType.StackedBar:
                case TChartType.StackedHorizontalBar:
                    return GetBarChartPointLocation(point, chartRect);
                case TChartType.Scatter:
                case TChartType.Bubble:
                    return GetScatterChartPointLocation(point, chartRect);
                case TChartType.Pie:
                case TChartType.Doughnut:
                    return GetPieChartPointLocation(point, chartRect);
                default:
                    return new Point(chartRect.X + chartRect.Width / 2, chartRect.Y + chartRect.Height / 2);
            }
        }

        private Point GetLineChartPointLocation(ChartDataPoint point, Rectangle chartRect)
        {
            var xRange = GetXRange();
            var yRange = GetYRange();

            if (xRange == 0 || yRange == 0) return new Point(chartRect.X, chartRect.Y);

            var x = chartRect.X + (float)((point.X - GetMinX()) / xRange * chartRect.Width);
            var y = chartRect.Bottom - (float)((point.Y - GetMinY()) / yRange * chartRect.Height);

            return new Point((int)x, (int)y);
        }

        private Point GetBarChartPointLocation(ChartDataPoint point, Rectangle chartRect)
        {
            var xRange = GetXRange();
            var yRange = GetYRange();

            if (xRange == 0 || yRange == 0) return new Point(chartRect.X, chartRect.Y);

            var x = chartRect.X + (float)((point.X - GetMinX()) / xRange * chartRect.Width);
            var y = chartRect.Bottom - (float)((point.Y - GetMinY()) / yRange * chartRect.Height);

            return new Point((int)x, (int)y);
        }

        private Point GetScatterChartPointLocation(ChartDataPoint point, Rectangle chartRect)
        {
            var xRange = GetXRange();
            var yRange = GetYRange();

            if (xRange == 0 || yRange == 0) return new Point(chartRect.X, chartRect.Y);

            var x = chartRect.X + (float)((point.X - GetMinX()) / xRange * chartRect.Width);
            var y = chartRect.Bottom - (float)((point.Y - GetMinY()) / yRange * chartRect.Height);

            return new Point((int)x, (int)y);
        }

        private Point GetPieChartPointLocation(ChartDataPoint point, Rectangle chartRect)
        {
            // 饼图的数据点位置计算比较复杂，这里简化处理
            var centerX = chartRect.X + chartRect.Width / 2;
            var centerY = chartRect.Y + chartRect.Height / 2;
            var radius = Math.Min(chartRect.Width, chartRect.Height) / 2 - 20;

            // 根据数据点的索引计算角度
            var pointIndex = 0;
            foreach (var dataset in Datasets.Where(d => d.Visible))
            {
                for (int i = 0; i < dataset.DataPoints.Count; i++)
                {
                    if (dataset.DataPoints[i] == point)
                    {
                        var totalPoints = Datasets.Where(d => d.Visible).Sum(d => d.DataPoints.Count);
                        var angle = 2 * Math.PI * pointIndex / totalPoints - Math.PI / 2; // 从12点钟方向开始
                        var x = centerX + (int)(radius * Math.Cos(angle));
                        var y = centerY + (int)(radius * Math.Sin(angle));
                        return new Point(x, y);
                    }
                    pointIndex++;
                }
            }

            return new Point(centerX, centerY);
        }

        private double GetMinX()
        {
            if (Datasets.Count == 0) return 0;
            return Datasets.Where(d => d.Visible).Min(d => d.DataPoints.Count > 0 ? d.DataPoints.Min(p => p.X) : 0);
        }

        private double GetMaxX()
        {
            if (Datasets.Count == 0) return 0;
            return Datasets.Where(d => d.Visible).Max(d => d.DataPoints.Count > 0 ? d.DataPoints.Max(p => p.X) : 0);
        }

        private double GetMinY()
        {
            if (Datasets.Count == 0) return 0;
            return Datasets.Where(d => d.Visible).Min(d => d.DataPoints.Count > 0 ? d.DataPoints.Min(p => p.Y) : 0);
        }

        private double GetMaxY()
        {
            if (Datasets.Count == 0) return 0;
            return Datasets.Where(d => d.Visible).Max(d => d.DataPoints.Count > 0 ? d.DataPoints.Max(p => p.Y) : 0);
        }

        private double GetXRange()
        {
            return GetMaxX() - GetMinX();
        }

        private double GetYRange()
        {
            return GetMaxY() - GetMinY();
        }

        #endregion

        #region 图表绘制方法

        private void DrawLineChart(Graphics g, Rectangle chartRect)
        {
            if (Datasets.Count == 0) return;

            var xRange = GetXRange();
            var yRange = GetYRange();
            if (xRange == 0 || yRange == 0) return;

            foreach (var dataset in Datasets.Where(d => d.Visible && d.DataPoints.Count > 1))
            {
                var points = new List<Point>();
                foreach (var dataPoint in dataset.DataPoints.Where(p => p.Visible))
                {
                    var x = chartRect.X + (float)((dataPoint.X - GetMinX()) / xRange * chartRect.Width);
                    var y = chartRect.Bottom - (float)((dataPoint.Y - GetMinY()) / yRange * chartRect.Height * AnimationProgress);
                    points.Add(new Point((int)x, (int)y));
                }

                if (points.Count < 2) continue;

                // 绘制线条
                var lineColor = dataset.BorderColor.HasValue ? dataset.BorderColor.Value : (dataset.FillColor.HasValue ? dataset.FillColor.Value : Color.Blue);
                using (var pen = new Pen(lineColor, dataset.BorderWidth))
                {
                    g.DrawLines(pen, points.ToArray());
                }

                // 绘制数据点
                var pointColor = dataset.FillColor.HasValue ? dataset.FillColor.Value : lineColor;
                using (var brush = new SolidBrush(pointColor))
                {
                    foreach (var point in points)
                    {
                        g.FillEllipse(brush, point.X - 3, point.Y - 3, 6, 6);
                    }
                }
            }
        }

        private void DrawBarChart(Graphics g, Rectangle chartRect)
        {
            if (Datasets.Count == 0) return;

            var xRange = GetXRange();
            var yRange = GetYRange();
            if (xRange == 0 || yRange == 0) return;

            var barWidth = chartRect.Width / (Datasets.Count * 10); // 动态计算柱宽
            var datasetIndex = 0;

            foreach (var dataset in Datasets.Where(d => d.Visible))
            {
                var barColor = dataset.FillColor.HasValue ? dataset.FillColor.Value : Color.Blue;
                using (var brush = new SolidBrush(barColor))
                using (var pen = new Pen(dataset.BorderColor.HasValue ? dataset.BorderColor.Value : Color.Black, dataset.BorderWidth))
                {
                    foreach (var dataPoint in dataset.DataPoints.Where(p => p.Visible))
                    {
                        var x = chartRect.X + (float)((dataPoint.X - GetMinX()) / xRange * chartRect.Width);
                        var barHeight = (float)((dataPoint.Y - GetMinY()) / yRange * chartRect.Height * AnimationProgress);
                        var y = chartRect.Bottom - barHeight;

                        var barRect = new RectangleF(x - barWidth / 2, y, barWidth, barHeight);
                        g.FillRectangle(brush, barRect);
                        g.DrawRectangle(pen, barRect.X, barRect.Y, barRect.Width, barRect.Height);
                    }
                }
                datasetIndex++;
            }
        }

        private void DrawAreaChart(Graphics g, Rectangle chartRect)
        {
            if (Datasets.Count == 0) return;

            var xRange = GetXRange();
            var yRange = GetYRange();
            if (xRange == 0 || yRange == 0) return;

            foreach (var dataset in Datasets.Where(d => d.Visible && d.DataPoints.Count > 1))
            {
                var points = new List<Point>();
                foreach (var dataPoint in dataset.DataPoints.Where(p => p.Visible))
                {
                    var x = chartRect.X + (float)((dataPoint.X - GetMinX()) / xRange * chartRect.Width);
                    var y = chartRect.Bottom - (float)((dataPoint.Y - GetMinY()) / yRange * chartRect.Height * AnimationProgress);
                    points.Add(new Point((int)x, (int)y));
                }

                if (points.Count < 2) continue;

                // 添加底部点以形成封闭区域
                points.Add(new Point(points[points.Count - 1].X, chartRect.Bottom));
                points.Add(new Point(points[0].X, chartRect.Bottom));

                // 绘制填充区域
                var fillColor = dataset.FillColor.HasValue ? dataset.FillColor.Value : Color.Blue;
                using (var brush = new SolidBrush(Color.FromArgb((int)(255 * dataset.Opacity), fillColor)))
                {
                    g.FillPolygon(brush, points.ToArray());
                }

                // 绘制边框
                var borderColor = dataset.BorderColor.HasValue ? dataset.BorderColor.Value : fillColor;
                using (var pen = new Pen(borderColor, dataset.BorderWidth))
                {
                    g.DrawLines(pen, points.Take(points.Count - 2).ToArray());
                }
            }
        }

        private void DrawPieChart(Graphics g, Rectangle chartRect)
        {
            if (Datasets.Count == 0) return;

            var centerX = chartRect.X + chartRect.Width / 2;
            var centerY = chartRect.Y + chartRect.Height / 2;
            var radius = Math.Min(chartRect.Width, chartRect.Height) / 2 - 20;

            var totalValue = 0.0;
            var visiblePoints = new List<ChartDataPoint>();

            // 收集所有可见的数据点
            foreach (var dataset in Datasets.Where(d => d.Visible))
            {
                foreach (var point in dataset.DataPoints.Where(p => p.Visible))
                {
                    visiblePoints.Add(point);
                    totalValue += Math.Abs(point.Y);
                }
            }

            if (totalValue == 0) return;

            var startAngle = -90f; // 从12点钟方向开始，使用float类型
            var defaultColors = new Color[] { Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.Orange, Color.Purple, Color.Pink, Color.Cyan };
            var colors = PieColors ?? defaultColors;

            for (int i = 0; i < visiblePoints.Count; i++)
            {
                var point = visiblePoints[i];
                var sweepAngle = (float)(Math.Abs(point.Y) / totalValue * 360 * AnimationProgress);

                var color = point.Color.HasValue ? point.Color.Value : colors[i % colors.Length];
                using (var brush = new SolidBrush(color))
                using (var pen = new Pen(Color.White, 2))
                {
                    var rect = new Rectangle(centerX - radius, centerY - radius, radius * 2, radius * 2);
                    g.FillPie(brush, rect, startAngle, sweepAngle);
                    g.DrawPie(pen, rect, startAngle, sweepAngle);
                }

                // 绘制标签
                if (!string.IsNullOrEmpty(point.Label))
                {
                    var labelAngle = startAngle + sweepAngle / 2;
                    var labelRadius = (int)(radius * 0.7f);
                    var labelX = centerX + (int)(labelRadius * Math.Cos(labelAngle * Math.PI / 180));
                    var labelY = centerY + (int)(labelRadius * Math.Sin(labelAngle * Math.PI / 180));

                    var labelColor = Config.IsDark ? Color.White : Color.Black;
                    using (var brush = new SolidBrush(labelColor))
                    {
                        var labelSize = g.MeasureString(point.Label, Font);
                        g.DrawString(point.Label, Font, brush, labelX - labelSize.Width / 2, labelY - labelSize.Height / 2);
                    }
                }

                startAngle += sweepAngle;
            }
        }

        private void DrawDoughnutChart(Graphics g, Rectangle chartRect)
        {
            if (Datasets.Count == 0) return;

            var centerX = chartRect.X + chartRect.Width / 2;
            var centerY = chartRect.Y + chartRect.Height / 2;
            var outerRadius = Math.Min(chartRect.Width, chartRect.Height) / 2 - 20;
            var innerRadius = outerRadius * 0.6f; // 内半径是外半径的60%

            var totalValue = 0.0;
            var visiblePoints = new List<ChartDataPoint>();

            // 收集所有可见的数据点
            foreach (var dataset in Datasets.Where(d => d.Visible))
            {
                foreach (var point in dataset.DataPoints.Where(p => p.Visible))
                {
                    visiblePoints.Add(point);
                    totalValue += Math.Abs(point.Y);
                }
            }

            if (totalValue == 0) return;

            var startAngle = -90f; // 从12点钟方向开始，使用float类型
            var defaultColors = new Color[] { Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.Orange, Color.Purple, Color.Pink, Color.Cyan };
            var colors = PieColors ?? defaultColors;

            for (int i = 0; i < visiblePoints.Count; i++)
            {
                var point = visiblePoints[i];
                var sweepAngle = (float)(Math.Abs(point.Y) / totalValue * 360 * AnimationProgress);

                var color = point.Color.HasValue ? point.Color.Value : colors[i % colors.Length];
                using (var brush = new SolidBrush(color))
                using (var pen = new Pen(Color.White, 2))
                {
                    // 绘制外圆
                    var outerRect = new Rectangle(centerX - outerRadius, centerY - outerRadius, outerRadius * 2, outerRadius * 2);
                    g.FillPie(brush, outerRect, startAngle, sweepAngle);

                    // 绘制内圆（挖空）
                    var innerRect = new Rectangle((int)(centerX - innerRadius), (int)(centerY - innerRadius), (int)(innerRadius * 2), (int)(innerRadius * 2));
                    var innerBackColor = Config.IsDark ? Color.FromArgb(30, 30, 30) : BackColor;
                    using (var innerBrush = new SolidBrush(innerBackColor))
                    {
                        g.FillPie(innerBrush, innerRect, startAngle, sweepAngle);
                    }

                    // 绘制边框
                    g.DrawArc(pen, outerRect, startAngle, sweepAngle);
                    g.DrawArc(pen, innerRect, startAngle, sweepAngle);
                }

                // 绘制标签
                if (!string.IsNullOrEmpty(point.Label))
                {
                    var labelAngle = startAngle + sweepAngle / 2;
                    var labelRadius = (outerRadius + innerRadius) / 2;
                    var labelX = centerX + (int)(labelRadius * Math.Cos(labelAngle * Math.PI / 180));
                    var labelY = centerY + (int)(labelRadius * Math.Sin(labelAngle * Math.PI / 180));

                    var labelColor = Config.IsDark ? Color.White : Color.Black;
                    using (var brush = new SolidBrush(labelColor))
                    {
                        var labelSize = g.MeasureString(point.Label, Font);
                        g.DrawString(point.Label, Font, brush, labelX - labelSize.Width / 2, labelY - labelSize.Height / 2);
                    }
                }

                startAngle += sweepAngle;
            }
        }

        private void DrawScatterChart(Graphics g, Rectangle chartRect)
        {
            if (Datasets.Count == 0) return;

            var xRange = GetXRange();
            var yRange = GetYRange();
            if (xRange == 0 || yRange == 0) return;

            foreach (var dataset in Datasets.Where(d => d.Visible))
            {
                var pointColor = dataset.FillColor.HasValue ? dataset.FillColor.Value : Color.Blue;
                var borderColor = dataset.BorderColor.HasValue ? dataset.BorderColor.Value : Color.Black;
                var pointSize = 6;

                using (var brush = new SolidBrush(pointColor))
                using (var pen = new Pen(borderColor, dataset.BorderWidth))
                {
                    foreach (var dataPoint in dataset.DataPoints.Where(p => p.Visible))
                    {
                        var x = chartRect.X + (float)((dataPoint.X - GetMinX()) / xRange * chartRect.Width);
                        var y = chartRect.Bottom - (float)((dataPoint.Y - GetMinY()) / yRange * chartRect.Height);

                        var rect = new RectangleF(x - pointSize / 2, y - pointSize / 2, pointSize, pointSize);
                        g.FillEllipse(brush, rect);
                        g.DrawEllipse(pen, rect.X, rect.Y, rect.Width, rect.Height);
                    }
                }
            }
        }

        private void DrawBubbleChart(Graphics g, Rectangle chartRect)
        {
            if (Datasets.Count == 0) return;

            var xRange = GetXRange();
            var yRange = GetYRange();
            var radiusRange = GetRadiusRange();
            if (xRange == 0 || yRange == 0) return;

            foreach (var dataset in Datasets.Where(d => d.Visible))
            {
                var pointColor = dataset.FillColor.HasValue ? dataset.FillColor.Value : Color.Blue;
                var borderColor = dataset.BorderColor.HasValue ? dataset.BorderColor.Value : Color.Black;

                using (var brush = new SolidBrush(Color.FromArgb((int)(255 * dataset.Opacity), pointColor)))
                using (var pen = new Pen(borderColor, dataset.BorderWidth))
                {
                    foreach (var dataPoint in dataset.DataPoints.Where(p => p.Visible))
                    {
                        var x = chartRect.X + (float)((dataPoint.X - GetMinX()) / xRange * chartRect.Width);
                        var y = chartRect.Bottom - (float)((dataPoint.Y - GetMinY()) / yRange * chartRect.Height);

                        var radius = radiusRange > 0 ? (float)(dataPoint.Radius / radiusRange * 20) : 10; // 最大半径20像素
                        radius = Math.Max(5, Math.Min(radius, 30)); // 限制半径范围

                        var rect = new RectangleF(x - radius, y - radius, radius * 2, radius * 2);
                        g.FillEllipse(brush, rect);
                        g.DrawEllipse(pen, rect.X, rect.Y, rect.Width, rect.Height);
                    }
                }
            }
        }

        private double GetRadiusRange()
        {
            if (Datasets.Count == 0) return 0;
            return Datasets.Where(d => d.Visible).Max(d => d.DataPoints.Count > 0 ? d.DataPoints.Max(p => p.Radius) : 0);
        }

        private void DrawRadarChart(Graphics g, Rectangle chartRect)
        {
            if (Datasets.Count == 0) return;

            var centerX = chartRect.X + chartRect.Width / 2;
            var centerY = chartRect.Y + chartRect.Height / 2;
            var radius = Math.Min(chartRect.Width, chartRect.Height) / 2 - 40;

            // 绘制雷达网格
            var gridLevels = 5;
            var gridColor = GridColor.HasValue ? GridColor.Value : Color.LightGray;
            using (var gridPen = new Pen(gridColor, 1))
            {
                for (int level = 1; level <= gridLevels; level++)
                {
                    var currentRadius = radius * level / gridLevels;
                    var points = new Point[6]; // 6边形
                    for (int i = 0; i < 6; i++)
                    {
                        var angle = i * 60 - 90; // 从12点钟方向开始
                        var x = centerX + (int)(currentRadius * Math.Cos(angle * Math.PI / 180));
                        var y = centerY + (int)(currentRadius * Math.Sin(angle * Math.PI / 180));
                        points[i] = new Point(x, y);
                    }
                    g.DrawPolygon(gridPen, points);
                }

                // 绘制轴线
                for (int i = 0; i < 6; i++)
                {
                    var angle = i * 60 - 90;
                    var x = centerX + (int)(radius * Math.Cos(angle * Math.PI / 180));
                    var y = centerY + (int)(radius * Math.Sin(angle * Math.PI / 180));
                    g.DrawLine(gridPen, centerX, centerY, x, y);
                }
            }

            // 绘制数据
            foreach (var dataset in Datasets.Where(d => d.Visible && d.DataPoints.Count > 0))
            {
                var dataPoints = dataset.DataPoints.Where(p => p.Visible).ToList();
                if (dataPoints.Count == 0) continue;

                var maxValue = dataPoints.Max(p => Math.Abs(p.Y));
                if (maxValue == 0) continue;

                var points = new Point[dataPoints.Count];
                for (int i = 0; i < dataPoints.Count; i++)
                {
                    var point = dataPoints[i];
                    var angle = i * (360.0 / dataPoints.Count) - 90;
                    var value = Math.Abs(point.Y) / maxValue;
                    var currentRadius = radius * value;
                    var x = centerX + (int)(currentRadius * Math.Cos(angle * Math.PI / 180));
                    var y = centerY + (int)(currentRadius * Math.Sin(angle * Math.PI / 180));
                    points[i] = new Point(x, y);
                }

                // 绘制填充区域
                var fillColor = dataset.FillColor.HasValue ? dataset.FillColor.Value : Color.Blue;
                using (var brush = new SolidBrush(Color.FromArgb((int)(255 * dataset.Opacity * 0.3), fillColor)))
                {
                    g.FillPolygon(brush, points);
                }

                // 绘制边框
                var borderColor = dataset.BorderColor.HasValue ? dataset.BorderColor.Value : fillColor;
                using (var pen = new Pen(borderColor, dataset.BorderWidth))
                {
                    g.DrawPolygon(pen, points);
                }

                // 绘制数据点
                using (var brush = new SolidBrush(fillColor))
                {
                    foreach (var point in points)
                    {
                        g.FillEllipse(brush, point.X - 3, point.Y - 3, 6, 6);
                    }
                }
            }
        }

        private void DrawPolarAreaChart(Graphics g, Rectangle chartRect)
        {
            if (Datasets.Count == 0) return;

            var centerX = chartRect.X + chartRect.Width / 2;
            var centerY = chartRect.Y + chartRect.Height / 2;
            var maxRadius = Math.Min(chartRect.Width, chartRect.Height) / 2 - 40; // 增加边距避免重叠

            var visiblePoints = new List<ChartDataPoint>();

            // 收集所有可见的数据点
            foreach (var dataset in Datasets.Where(d => d.Visible))
            {
                foreach (var point in dataset.DataPoints.Where(p => p.Visible))
                {
                    visiblePoints.Add(point);
                }
            }

            if (visiblePoints.Count == 0) return;

            // 计算最大数值用于半径缩放
            var maxValue = visiblePoints.Max(p => Math.Abs(p.Y));
            if (maxValue == 0) return;

            // 每个数据点占据固定的角度
            var anglePerPoint = 360.0f / visiblePoints.Count;
            var startAngle = -90f; // 从12点钟方向开始

            var defaultColors = new Color[] { Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.Orange, Color.Purple, Color.Pink, Color.Cyan };
            var colors = PieColors ?? defaultColors;

            for (int i = 0; i < visiblePoints.Count; i++)
            {
                var point = visiblePoints[i];
                var sweepAngle = anglePerPoint; // 固定角度
                var radius = (int)(Math.Abs(point.Y) / maxValue * maxRadius * AnimationProgress);

                // 确保半径至少为1像素，避免FillPie参数错误
                radius = Math.Max(1, radius);

                var color = point.Color.HasValue ? point.Color.Value : colors[i % colors.Length];
                using (var brush = new SolidBrush(Color.FromArgb((int)(255 * 0.7), color)))
                using (var pen = new Pen(color, 2))
                {
                    // 确保矩形尺寸有效
                    var rect = new Rectangle(centerX - radius, centerY - radius, radius * 2, radius * 2);

                    // 检查矩形是否有效
                    if (rect.Width > 0 && rect.Height > 0)
                    {
                        g.FillPie(brush, rect, startAngle, sweepAngle);
                        g.DrawPie(pen, rect, startAngle, sweepAngle);
                    }
                }

                // 绘制标签
                if (!string.IsNullOrEmpty(point.Label))
                {
                    var labelAngle = startAngle + sweepAngle / 2;
                    var labelRadius = Math.Max(1, (int)(radius * 0.7f));
                    var labelX = centerX + (int)(labelRadius * Math.Cos(labelAngle * Math.PI / 180));
                    var labelY = centerY + (int)(labelRadius * Math.Sin(labelAngle * Math.PI / 180));

                    var labelColor = Config.IsDark ? Color.White : Color.Black;
                    using (var brush = new SolidBrush(labelColor))
                    {
                        var labelSize = g.MeasureString(point.Label, Font);
                        g.DrawString(point.Label, Font, brush, labelX - labelSize.Width / 2, labelY - labelSize.Height / 2);
                    }
                }

                startAngle += sweepAngle;
            }
        }

        private void DrawSplineChart(Graphics g, Rectangle chartRect)
        {
            if (Datasets.Count == 0) return;

            var xRange = GetXRange();
            var yRange = GetYRange();
            if (xRange == 0 || yRange == 0) return;

            foreach (var dataset in Datasets.Where(d => d.Visible && d.DataPoints.Count > 2))
            {
                var points = new List<PointF>();
                foreach (var dataPoint in dataset.DataPoints.Where(p => p.Visible))
                {
                    var x = chartRect.X + (float)((dataPoint.X - GetMinX()) / xRange * chartRect.Width);
                    var y = chartRect.Bottom - (float)((dataPoint.Y - GetMinY()) / yRange * chartRect.Height);
                    points.Add(new PointF(x, y));
                }

                if (points.Count < 3) continue;

                // 绘制样条曲线
                var lineColor = dataset.BorderColor.HasValue ? dataset.BorderColor.Value : (dataset.FillColor.HasValue ? dataset.FillColor.Value : Color.Blue);
                using (var pen = new Pen(lineColor, dataset.BorderWidth))
                {
                    DrawSplineCurve(g, pen, points.ToArray());
                }

                // 绘制数据点
                var pointColor = dataset.FillColor.HasValue ? dataset.FillColor.Value : lineColor;
                using (var brush = new SolidBrush(pointColor))
                {
                    foreach (var point in points)
                    {
                        g.FillEllipse(brush, point.X - 3, point.Y - 3, 6, 6);
                    }
                }
            }
        }

        private void DrawSplineCurve(Graphics g, Pen pen, PointF[] points)
        {
            if (points.Length < 3) return;

            var path = new GraphicsPath();
            path.AddCurve(points, 0.5f); // 张力参数0.5
            g.DrawPath(pen, path);
        }

        private void DrawSplineAreaChart(Graphics g, Rectangle chartRect)
        {
            if (Datasets.Count == 0) return;

            var xRange = GetXRange();
            var yRange = GetYRange();
            if (xRange == 0 || yRange == 0) return;

            foreach (var dataset in Datasets.Where(d => d.Visible && d.DataPoints.Count > 2))
            {
                var points = new List<PointF>();
                foreach (var dataPoint in dataset.DataPoints.Where(p => p.Visible))
                {
                    var x = chartRect.X + (float)((dataPoint.X - GetMinX()) / xRange * chartRect.Width);
                    var y = chartRect.Bottom - (float)((dataPoint.Y - GetMinY()) / yRange * chartRect.Height);
                    points.Add(new PointF(x, y));
                }

                if (points.Count < 3) continue;

                // 添加底部点以形成封闭区域
                points.Add(new PointF(points[points.Count - 1].X, chartRect.Bottom));
                points.Add(new PointF(points[0].X, chartRect.Bottom));

                // 绘制填充区域
                var fillColor = dataset.FillColor ?? Color.Blue;
                using (var brush = new SolidBrush(Color.FromArgb((int)(255 * dataset.Opacity), fillColor)))
                {
                    var path = new GraphicsPath();
                    path.AddCurve(points.Take(points.Count - 2).ToArray(), 0.5f);
                    path.AddLine(points[points.Count - 3], points[points.Count - 2]);
                    path.AddLine(points[points.Count - 2], points[points.Count - 1]);
                    path.CloseFigure();
                    g.FillPath(brush, path);
                }

                // 绘制边框
                var borderColor = dataset.BorderColor.HasValue ? dataset.BorderColor.Value : fillColor;
                using (var pen = new Pen(borderColor, dataset.BorderWidth))
                {
                    var path = new GraphicsPath();
                    path.AddCurve(points.Take(points.Count - 2).ToArray(), 0.5f);
                    g.DrawPath(pen, path);
                }
            }
        }

        private void DrawHorizontalBarChart(Graphics g, Rectangle chartRect)
        {
            if (Datasets.Count == 0) return;

            var xRange = GetXRange();
            var yRange = GetYRange();
            if (xRange == 0 || yRange == 0) return;

            var barHeight = chartRect.Height / (Datasets.Count * 10); // 动态计算柱高
            var datasetIndex = 0;

            foreach (var dataset in Datasets.Where(d => d.Visible))
            {
                var barColor = dataset.FillColor ?? Color.Blue;
                using (var brush = new SolidBrush(barColor))
                using (var pen = new Pen(dataset.BorderColor ?? Color.Black, dataset.BorderWidth))
                {
                    foreach (var dataPoint in dataset.DataPoints.Where(p => p.Visible))
                    {
                        var y = chartRect.Y + (float)((dataPoint.Y - GetMinY()) / yRange * chartRect.Height);
                        var barWidth = (float)((dataPoint.X - GetMinX()) / xRange * chartRect.Width);
                        var x = chartRect.X;

                        var barRect = new RectangleF(x, y - barHeight / 2, barWidth, barHeight);
                        g.FillRectangle(brush, barRect);
                        g.DrawRectangle(pen, barRect.X, barRect.Y, barRect.Width, barRect.Height);
                    }
                }
                datasetIndex++;
            }
        }

        private void DrawStackedBarChart(Graphics g, Rectangle chartRect)
        {
            if (Datasets.Count == 0) return;

            var xRange = GetXRange();
            var yRange = GetYRange();
            if (xRange == 0 || yRange == 0) return;

            var barWidth = chartRect.Width / 10; // 固定柱宽
            var visibleDatasets = Datasets.Where(d => d.Visible).ToList();
            var maxPoints = visibleDatasets.Max(d => d.DataPoints.Count);

            for (int pointIndex = 0; pointIndex < maxPoints; pointIndex++)
            {
                var x = chartRect.X + (pointIndex * chartRect.Width / maxPoints) + (chartRect.Width / maxPoints - barWidth) / 2;
                var currentY = chartRect.Bottom;

                foreach (var dataset in visibleDatasets)
                {
                    if (pointIndex < dataset.DataPoints.Count)
                    {
                        var dataPoint = dataset.DataPoints[pointIndex];
                        if (!dataPoint.Visible) continue;

                        var barHeight = (float)((dataPoint.Y - GetMinY()) / yRange * chartRect.Height);
                        var y = currentY - barHeight;

                        var barColor = dataset.FillColor ?? Color.Blue;
                        using (var brush = new SolidBrush(barColor))
                        using (var pen = new Pen(dataset.BorderColor ?? Color.Black, dataset.BorderWidth))
                        {
                            var barRect = new RectangleF(x, y, barWidth, barHeight);
                            g.FillRectangle(brush, barRect);
                            g.DrawRectangle(pen, barRect.X, barRect.Y, barRect.Width, barRect.Height);
                        }

                        currentY = (int)y;
                    }
                }
            }
        }

        private void DrawStackedHorizontalBarChart(Graphics g, Rectangle chartRect)
        {
            if (Datasets.Count == 0) return;

            var xRange = GetXRange();
            var yRange = GetYRange();
            if (xRange == 0 || yRange == 0) return;

            var barHeight = chartRect.Height / 10; // 固定柱高
            var visibleDatasets = Datasets.Where(d => d.Visible).ToList();
            var maxPoints = visibleDatasets.Max(d => d.DataPoints.Count);

            for (int pointIndex = 0; pointIndex < maxPoints; pointIndex++)
            {
                var y = chartRect.Y + (pointIndex * chartRect.Height / maxPoints) + (chartRect.Height / maxPoints - barHeight) / 2;
                var currentX = chartRect.X;

                foreach (var dataset in visibleDatasets)
                {
                    if (pointIndex < dataset.DataPoints.Count)
                    {
                        var dataPoint = dataset.DataPoints[pointIndex];
                        if (!dataPoint.Visible) continue;

                        var barWidth = (float)((dataPoint.X - GetMinX()) / xRange * chartRect.Width);
                        var x = currentX;

                        var barColor = dataset.FillColor ?? Color.Blue;
                        using (var brush = new SolidBrush(barColor))
                        using (var pen = new Pen(dataset.BorderColor ?? Color.Black, dataset.BorderWidth))
                        {
                            var barRect = new RectangleF(x, y, barWidth, barHeight);
                            g.FillRectangle(brush, barRect);
                            g.DrawRectangle(pen, barRect.X, barRect.Y, barRect.Width, barRect.Height);
                        }

                        currentX += (int)barWidth;
                    }
                }
            }
        }

        private void DrawSteppedLineChart(Graphics g, Rectangle chartRect)
        {
            if (Datasets.Count == 0) return;

            var xRange = GetXRange();
            var yRange = GetYRange();
            if (xRange == 0 || yRange == 0) return;

            foreach (var dataset in Datasets.Where(d => d.Visible && d.DataPoints.Count > 1))
            {
                var points = new List<Point>();
                foreach (var dataPoint in dataset.DataPoints.Where(p => p.Visible))
                {
                    var x = chartRect.X + (float)((dataPoint.X - GetMinX()) / xRange * chartRect.Width);
                    var y = chartRect.Bottom - (float)((dataPoint.Y - GetMinY()) / yRange * chartRect.Height);
                    points.Add(new Point((int)x, (int)y));
                }

                if (points.Count < 2) continue;

                // 绘制阶梯线
                var lineColor = dataset.BorderColor.HasValue ? dataset.BorderColor.Value : (dataset.FillColor.HasValue ? dataset.FillColor.Value : Color.Blue);
                using (var pen = new Pen(lineColor, dataset.BorderWidth))
                {
                    for (int i = 0; i < points.Count - 1; i++)
                    {
                        var currentPoint = points[i];
                        var nextPoint = points[i + 1];

                        // 水平线
                        g.DrawLine(pen, currentPoint.X, currentPoint.Y, nextPoint.X, currentPoint.Y);
                        // 垂直线
                        g.DrawLine(pen, nextPoint.X, currentPoint.Y, nextPoint.X, nextPoint.Y);
                    }
                }

                // 绘制数据点
                var pointColor = dataset.FillColor.HasValue ? dataset.FillColor.Value : lineColor;
                using (var brush = new SolidBrush(pointColor))
                {
                    foreach (var point in points)
                    {
                        g.FillEllipse(brush, point.X - 3, point.Y - 3, 6, 6);
                    }
                }
            }
        }

        private void DrawSteppedAreaChart(Graphics g, Rectangle chartRect)
        {
            if (Datasets.Count == 0) return;

            var xRange = GetXRange();
            var yRange = GetYRange();
            if (xRange == 0 || yRange == 0) return;

            foreach (var dataset in Datasets.Where(d => d.Visible && d.DataPoints.Count > 1))
            {
                var points = new List<Point>();
                foreach (var dataPoint in dataset.DataPoints.Where(p => p.Visible))
                {
                    var x = chartRect.X + (float)((dataPoint.X - GetMinX()) / xRange * chartRect.Width);
                    var y = chartRect.Bottom - (float)((dataPoint.Y - GetMinY()) / yRange * chartRect.Height);
                    points.Add(new Point((int)x, (int)y));
                }

                if (points.Count < 2) continue;

                // 生成阶梯点
                var steppedPoints = new List<Point>();
                for (int i = 0; i < points.Count - 1; i++)
                {
                    var currentPoint = points[i];
                    var nextPoint = points[i + 1];

                    steppedPoints.Add(currentPoint);
                    steppedPoints.Add(new Point(nextPoint.X, currentPoint.Y));
                }
                steppedPoints.Add(points[points.Count - 1]);

                // 添加底部点以形成封闭区域
                steppedPoints.Add(new Point(steppedPoints[steppedPoints.Count - 1].X, chartRect.Bottom));
                steppedPoints.Add(new Point(steppedPoints[0].X, chartRect.Bottom));

                // 绘制填充区域
                var fillColor = dataset.FillColor.HasValue ? dataset.FillColor.Value : Color.Blue;
                using (var brush = new SolidBrush(Color.FromArgb((int)(255 * dataset.Opacity), fillColor)))
                {
                    g.FillPolygon(brush, steppedPoints.ToArray());
                }

                // 绘制边框
                var borderColor = dataset.BorderColor.HasValue ? dataset.BorderColor.Value : fillColor;
                using (var pen = new Pen(borderColor, dataset.BorderWidth))
                {
                    g.DrawLines(pen, steppedPoints.Take(steppedPoints.Count - 2).ToArray());
                }
            }
        }

        #endregion
    }
}