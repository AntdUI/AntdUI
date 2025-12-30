// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using AntdUI;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class Chart : UserControl
    {
        AntdUI.BaseForm form;
        private AntdUI.Chart mainChart;
        private AntdUI.Select selectChartType;

        public Chart(AntdUI.BaseForm _form)
        {
            form = _form;
            InitializeComponent();
            InitializeControls();

            // 更新图表类型
            mainChart.ChartType = TChartType.Line;

            // 根据图表类型更新标题和样式
            UpdateChartStyle(TChartType.Line);
            LoadChartData();
        }

        private void InitializeControls()
        {
            // 创建控制面板
            var controlPanel = new AntdUI.Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                Padding = new Padding(10, 5, 10, 5)
            };

            // 创建图表类型选择标签
            var typeLabel = new AntdUI.Label
            {
                Text = "图表类型:",
                Font = new Font("微软雅黑", 10, FontStyle.Regular),
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleLeft,
                Location = new Point(10, 15),
                Size = new Size(80, 20)
            };

            // 创建图表类型下拉选择组件
            selectChartType = new AntdUI.Select
            {
                List = true,
                Location = new Point(100, 10),
                Size = new Size(200, 40),
                Font = new Font("微软雅黑", 10, FontStyle.Regular)
            };

            // 添加图表类型选项
            selectChartType.Items.Add(new SelectItem("折线图", TChartType.Line));
            selectChartType.Items.Add(new SelectItem("柱状图", TChartType.Bar));
            selectChartType.Items.Add(new SelectItem("饼图", TChartType.Pie));
            selectChartType.Items.Add(new SelectItem("面积图", TChartType.Area));
            selectChartType.Items.Add(new SelectItem("雷达图", TChartType.Radar));
            selectChartType.Items.Add(new SelectItem("散点图", TChartType.Scatter));
            selectChartType.Items.Add(new SelectItem("气泡图", TChartType.Bubble));
            selectChartType.Items.Add(new SelectItem("极坐标面积图", TChartType.PolarArea));
            selectChartType.Items.Add(new SelectItem("甜甜圈图", TChartType.Doughnut));
            selectChartType.Items.Add(new SelectItem("样条曲线图", TChartType.Spline));
            selectChartType.Items.Add(new SelectItem("样条面积图", TChartType.SplineArea));
            selectChartType.Items.Add(new SelectItem("水平柱状图", TChartType.HorizontalBar));
            selectChartType.Items.Add(new SelectItem("堆叠柱状图", TChartType.StackedBar));
            selectChartType.Items.Add(new SelectItem("堆叠水平柱状图", TChartType.StackedHorizontalBar));
            selectChartType.Items.Add(new SelectItem("阶梯线图", TChartType.SteppedLine));
            selectChartType.Items.Add(new SelectItem("阶梯面积图", TChartType.SteppedArea));

            selectChartType.SelectedIndex = 0; // 默认选择折线图

            // 注册选择变化事件
            selectChartType.SelectedIndexChanged += SelectChartType_SelectedIndexChanged;

            // 创建导出按钮
            var exportButton = new AntdUI.Button
            {
                Text = "导出图表",
                Type = TTypeMini.Primary,
                Location = new Point(320, 10),
                Width = 100,
                Height = 40,
                Font = new Font("微软雅黑", 10, FontStyle.Regular)
            };

            // 注册导出按钮点击事件
            exportButton.Click += ExportButton_Click;

            // 添加控件到控制面板
            controlPanel.Controls.Add(typeLabel);
            controlPanel.Controls.Add(selectChartType);
            controlPanel.Controls.Add(exportButton);

            // 创建主图表
            mainChart = new AntdUI.Chart
            {
                ShowLegend = true,
                ShowGrid = true,
                ShowAxes = true,
                ShowTooltip = true,
                EnableAnimation = true,
                Padding = 20,
                Dock = DockStyle.Fill
            };

            // 注册图表事件
            mainChart.PointClick += Chart_PointClick;
            mainChart.AreaClick += Chart_AreaClick;
            mainChart.PointHover += Chart_PointHover;

            // 添加控件到主面板
            panel_main.Controls.Add(mainChart);
            panel_main.Controls.Add(controlPanel);
        }

        private void SelectChartType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectChartType.SelectedValue is TChartType chartType)
            {
                // 更新图表类型
                mainChart.ChartType = chartType;

                // 根据图表类型更新标题和样式
                UpdateChartStyle(chartType);

                // 重新加载数据
                LoadChartData();
            }
        }

        private void UpdateChartStyle(TChartType chartType)
        {
            switch (chartType)
            {
                case TChartType.Line:
                    mainChart.Title = "销售趋势图";
                    mainChart.TitleColor = Color.FromArgb(24, 144, 255);
                    mainChart.ShowGrid = true;
                    mainChart.ShowAxes = true;
                    mainChart.ShowTooltip = true;
                    break;
                case TChartType.Bar:
                    mainChart.Title = "月度销售柱状图";
                    mainChart.TitleColor = Color.FromArgb(82, 196, 26);
                    mainChart.ShowGrid = true;
                    mainChart.ShowAxes = true;
                    break;
                case TChartType.Pie:
                    mainChart.Title = "市场份额分布";
                    mainChart.TitleColor = Color.FromArgb(250, 140, 22);
                    mainChart.ShowGrid = false;
                    mainChart.ShowAxes = false;
                    mainChart.PieColors = new Color[] { Color.FromArgb(35, 137, 255), Color.FromArgb(13, 204, 204), Color.FromArgb(241, 142, 86), Color.FromArgb(215, 135, 255), Color.FromArgb(104, 199, 56) };
                    break;
                case TChartType.Area:
                    mainChart.Title = "收入面积图";
                    mainChart.TitleColor = Color.FromArgb(245, 34, 45);
                    mainChart.ShowGrid = true;
                    mainChart.ShowAxes = true;
                    break;
                case TChartType.Radar:
                    mainChart.Title = "能力雷达图";
                    mainChart.TitleColor = Color.FromArgb(23, 131, 255);
                    mainChart.ShowGrid = true;
                    mainChart.ShowAxes = false;
                    break;
                case TChartType.Scatter:
                    mainChart.Title = "散点分布图";
                    mainChart.TitleColor = Color.FromArgb(19, 206, 102);
                    mainChart.ShowGrid = true;
                    mainChart.ShowAxes = true;
                    break;
                case TChartType.Bubble:
                    mainChart.Title = "气泡图";
                    mainChart.TitleColor = Color.FromArgb(255, 77, 79);
                    mainChart.ShowGrid = true;
                    mainChart.ShowAxes = true;
                    break;
                case TChartType.PolarArea:
                    mainChart.Title = "极坐标面积图";
                    mainChart.TitleColor = Color.FromArgb(64, 169, 255);
                    mainChart.ShowGrid = true;
                    mainChart.ShowAxes = false;
                    mainChart.PieColors = new Color[] { Color.FromArgb(35, 137, 255), Color.FromArgb(13, 204, 204), Color.FromArgb(241, 142, 86), Color.FromArgb(215, 135, 255), Color.FromArgb(104, 199, 56) };
                    break;
                case TChartType.Doughnut:
                    mainChart.Title = "甜甜圈图";
                    mainChart.TitleColor = Color.FromArgb(250, 173, 20);
                    mainChart.ShowGrid = false;
                    mainChart.ShowAxes = false;
                    break;
                case TChartType.Spline:
                    mainChart.Title = "样条曲线图";
                    mainChart.TitleColor = Color.FromArgb(82, 196, 26);
                    mainChart.ShowGrid = true;
                    mainChart.ShowAxes = true;
                    break;
                case TChartType.SplineArea:
                    mainChart.Title = "样条面积图";
                    mainChart.TitleColor = Color.FromArgb(114, 46, 209);
                    mainChart.ShowGrid = true;
                    mainChart.ShowAxes = true;
                    break;
                case TChartType.HorizontalBar:
                    mainChart.Title = "水平柱状图";
                    mainChart.TitleColor = Color.FromArgb(245, 34, 45);
                    mainChart.ShowGrid = true;
                    mainChart.ShowAxes = true;
                    break;
                case TChartType.StackedBar:
                    mainChart.Title = "堆叠柱状图";
                    mainChart.TitleColor = Color.FromArgb(24, 144, 255);
                    mainChart.ShowGrid = true;
                    mainChart.ShowAxes = true;
                    break;
                case TChartType.StackedHorizontalBar:
                    mainChart.Title = "堆叠水平柱状图";
                    mainChart.TitleColor = Color.FromArgb(82, 196, 26);
                    mainChart.ShowGrid = true;
                    mainChart.ShowAxes = true;
                    break;
                case TChartType.SteppedLine:
                    mainChart.Title = "阶梯线图";
                    mainChart.TitleColor = Color.FromArgb(255, 77, 79);
                    mainChart.ShowGrid = true;
                    mainChart.ShowAxes = true;
                    break;
                case TChartType.SteppedArea:
                    mainChart.Title = "阶梯面积图";
                    mainChart.TitleColor = Color.FromArgb(19, 206, 102);
                    mainChart.ShowGrid = true;
                    mainChart.ShowAxes = true;
                    break;
            }
        }

        private void LoadChartData()
        {
            var chartType = mainChart.ChartType;

            switch (chartType)
            {
                case TChartType.Line:
                    LoadLineChartData();
                    break;
                case TChartType.Bar:
                    LoadBarChartData();
                    break;
                case TChartType.Pie:
                    LoadPieChartData();
                    break;
                case TChartType.Area:
                    LoadAreaChartData();
                    break;
                case TChartType.Radar:
                    LoadRadarChartData();
                    break;
                case TChartType.Scatter:
                    LoadScatterChartData();
                    break;
                case TChartType.Bubble:
                    LoadBubbleChartData();
                    break;
                case TChartType.PolarArea:
                    LoadPolarChartData();
                    break;
                case TChartType.Doughnut:
                    LoadDoughnutChartData();
                    break;
                case TChartType.Spline:
                    LoadSplineChartData();
                    break;
                case TChartType.SplineArea:
                    LoadSplineAreaChartData();
                    break;
                case TChartType.HorizontalBar:
                    LoadHorizontalBarChartData();
                    break;
                case TChartType.StackedBar:
                    LoadStackedBarChartData();
                    break;
                case TChartType.StackedHorizontalBar:
                    LoadStackedHorizontalBarChartData();
                    break;
                case TChartType.SteppedLine:
                    LoadSteppedLineChartData();
                    break;
                case TChartType.SteppedArea:
                    LoadSteppedAreaChartData();
                    break;
            }
        }

        private void LoadLineChartData()
        {
            mainChart.ChartType = TChartType.Line;
            mainChart.ClearDatasets();

            var salesDataset = new ChartDataset("销售额", Color.FromArgb(24, 144, 255))
            {
                BorderColor = Color.FromArgb(24, 144, 255),
                BorderWidth = 3,
                Opacity = 0.8f
            };

            salesDataset.AddPoint("1月", 1, 120);
            salesDataset.AddPoint("2月", 2, 150);
            salesDataset.AddPoint("3月", 3, 180);
            salesDataset.AddPoint("4月", 4, 200);
            salesDataset.AddPoint("5月", 5, 220);
            salesDataset.AddPoint("6月", 6, 250);

            var profitDataset = new ChartDataset("利润", Color.FromArgb(82, 196, 26))
            {
                BorderColor = Color.FromArgb(82, 196, 26),
                BorderWidth = 3,
                Opacity = 0.8f
            };

            profitDataset.AddPoint("1月", 1, 30);
            profitDataset.AddPoint("2月", 2, 40);
            profitDataset.AddPoint("3月", 3, 50);
            profitDataset.AddPoint("4月", 4, 60);
            profitDataset.AddPoint("5月", 5, 70);
            profitDataset.AddPoint("6月", 6, 80);

            mainChart.AddDataset(salesDataset);
            mainChart.AddDataset(profitDataset);
        }

        private void LoadBarChartData()
        {
            mainChart.ChartType = TChartType.Bar;
            mainChart.ClearDatasets();

            var salesDataset = new ChartDataset("销售额", Color.FromArgb(82, 196, 26))
            {
                BorderColor = Color.FromArgb(82, 196, 26),
                BorderWidth = 2,
                Opacity = 0.8f
            };

            salesDataset.AddPoint("1月", 1, 120);
            salesDataset.AddPoint("2月", 2, 150);
            salesDataset.AddPoint("3月", 3, 180);
            salesDataset.AddPoint("4月", 4, 200);
            salesDataset.AddPoint("5月", 5, 220);
            salesDataset.AddPoint("6月", 6, 250);
            salesDataset.AddPoint("7月", 7, 250);

            mainChart.AddDataset(salesDataset);
        }

        private void LoadPieChartData()
        {
            mainChart.ChartType = TChartType.Pie;
            mainChart.ClearDatasets();

            var pieDataset = new ChartDataset("市场份额", Color.FromArgb(250, 140, 22));

            pieDataset.AddPoint("产品A", 0, 35);
            pieDataset.AddPoint("产品B", 0, 25);
            pieDataset.AddPoint("产品C", 0, 20);
            pieDataset.AddPoint("产品D", 0, 15);
            pieDataset.AddPoint("其他", 0, 5);

            mainChart.AddDataset(pieDataset);
        }

        private void LoadAreaChartData()
        {
            mainChart.ChartType = TChartType.Area;
            mainChart.ClearDatasets();

            var incomeDataset = new ChartDataset("总收入", Color.FromArgb(245, 34, 45))
            {
                BorderColor = Color.FromArgb(245, 34, 45),
                BorderWidth = 2,
                Opacity = 0.6f
            };

            incomeDataset.AddPoint("1月", 1, 100);
            incomeDataset.AddPoint("2月", 2, 130);
            incomeDataset.AddPoint("3月", 3, 160);
            incomeDataset.AddPoint("4月", 4, 190);
            incomeDataset.AddPoint("5月", 5, 220);
            incomeDataset.AddPoint("6月", 6, 280);

            var costDataset = new ChartDataset("总成本", Color.FromArgb(114, 46, 209))
            {
                BorderColor = Color.FromArgb(114, 46, 209),
                BorderWidth = 2,
                Opacity = 0.6f
            };

            costDataset.AddPoint("1月", 1, 80);
            costDataset.AddPoint("2月", 2, 100);
            costDataset.AddPoint("3月", 3, 120);
            costDataset.AddPoint("4月", 4, 140);
            costDataset.AddPoint("5月", 5, 160);
            costDataset.AddPoint("6月", 6, 180);

            mainChart.AddDataset(incomeDataset);
            mainChart.AddDataset(costDataset);
        }

        private void LoadRadarChartData()
        {
            mainChart.ChartType = TChartType.Radar;
            mainChart.ClearDatasets();

            var performanceDataset = new ChartDataset("性能指标", Color.FromArgb(23, 131, 255))
            {
                BorderColor = Color.FromArgb(23, 131, 255),
                BorderWidth = 2,
                Opacity = 0.6f
            };

            performanceDataset.AddPoint("技术能力", 0, 85);
            performanceDataset.AddPoint("沟通能力", 0, 90);
            performanceDataset.AddPoint("团队合作", 0, 75);
            performanceDataset.AddPoint("创新能力", 0, 80);
            performanceDataset.AddPoint("执行力", 0, 95);

            mainChart.AddDataset(performanceDataset);
        }

        private void LoadScatterChartData()
        {
            mainChart.ChartType = TChartType.Scatter;
            mainChart.ClearDatasets();

            var scatterDataset = new ChartDataset("数据分布", Color.FromArgb(19, 206, 102))
            {
                BorderColor = Color.FromArgb(19, 206, 102),
                BorderWidth = 2,
                Opacity = 0.8f
            };

            // 添加随机散点数据
            var random = new Random(42); // 固定种子以确保可重现
            for (int i = 0; i < 20; i++)
            {
                var x = random.Next(10, 90);
                var y = random.Next(10, 90);
                scatterDataset.AddPoint($"点{i + 1}", x, y);
            }

            mainChart.AddDataset(scatterDataset);
        }

        private void LoadBubbleChartData()
        {
            mainChart.ChartType = TChartType.Bubble;
            mainChart.ClearDatasets();

            var bubbleDataset = new ChartDataset("气泡数据", Color.FromArgb(255, 77, 79))
            {
                BorderColor = Color.FromArgb(255, 77, 79),
                BorderWidth = 1,
                Opacity = 0.7f
            };

            // 添加气泡数据 (x, y, size)
            bubbleDataset.AddPoint("A", 20, 30, 15);
            bubbleDataset.AddPoint("B", 40, 60, 25);
            bubbleDataset.AddPoint("C", 60, 40, 20);
            bubbleDataset.AddPoint("D", 80, 80, 35);
            bubbleDataset.AddPoint("E", 30, 70, 18);

            mainChart.AddDataset(bubbleDataset);
        }

        private void LoadPolarChartData()
        {
            mainChart.ChartType = TChartType.PolarArea;
            mainChart.ClearDatasets();

            var polarDataset = new ChartDataset("极坐标数据", Color.FromArgb(64, 169, 255))
            {
                BorderColor = Color.FromArgb(64, 169, 255),
                BorderWidth = 2,
                Opacity = 0.8f
            };

            polarDataset.AddPoint("类别A", 0, 70);
            polarDataset.AddPoint("类别B", 0, 85);
            polarDataset.AddPoint("类别C", 0, 60);
            polarDataset.AddPoint("类别D", 0, 90);
            polarDataset.AddPoint("类别E", 0, 75);

            mainChart.AddDataset(polarDataset);
        }

        private void LoadDoughnutChartData()
        {
            mainChart.ChartType = TChartType.Doughnut;
            mainChart.ClearDatasets();

            var doughnutDataset = new ChartDataset("甜甜圈数据", Color.FromArgb(250, 173, 20));

            doughnutDataset.AddPoint("部门A", 0, 30);
            doughnutDataset.AddPoint("部门B", 0, 25);
            doughnutDataset.AddPoint("部门C", 0, 20);
            doughnutDataset.AddPoint("部门D", 0, 15);
            doughnutDataset.AddPoint("其他", 0, 10);

            mainChart.AddDataset(doughnutDataset);
        }

        private void LoadSplineChartData()
        {
            mainChart.ChartType = TChartType.Spline;
            mainChart.ClearDatasets();

            var splineDataset = new ChartDataset("样条曲线", Color.FromArgb(82, 196, 26))
            {
                BorderColor = Color.FromArgb(82, 196, 26),
                BorderWidth = 3,
                Opacity = 0.8f
            };

            splineDataset.AddPoint("1月", 1, 100);
            splineDataset.AddPoint("2月", 2, 120);
            splineDataset.AddPoint("3月", 3, 110);
            splineDataset.AddPoint("4月", 4, 140);
            splineDataset.AddPoint("5月", 5, 130);
            splineDataset.AddPoint("6月", 6, 160);

            mainChart.AddDataset(splineDataset);
        }

        private void LoadSplineAreaChartData()
        {
            mainChart.ChartType = TChartType.SplineArea;
            mainChart.ClearDatasets();

            var splineAreaDataset = new ChartDataset("样条面积", Color.FromArgb(114, 46, 209))
            {
                BorderColor = Color.FromArgb(114, 46, 209),
                BorderWidth = 2,
                Opacity = 0.6f
            };

            splineAreaDataset.AddPoint("1月", 1, 80);
            splineAreaDataset.AddPoint("2月", 2, 100);
            splineAreaDataset.AddPoint("3月", 3, 90);
            splineAreaDataset.AddPoint("4月", 4, 120);
            splineAreaDataset.AddPoint("5月", 5, 110);
            splineAreaDataset.AddPoint("6月", 6, 140);

            mainChart.AddDataset(splineAreaDataset);
        }

        private void LoadHorizontalBarChartData()
        {
            mainChart.ChartType = TChartType.HorizontalBar;
            mainChart.ClearDatasets();

            var horizontalBarDataset = new ChartDataset("水平柱状图", Color.FromArgb(245, 34, 45))
            {
                FillColor = Color.FromArgb(34, 136, 255),
                BorderColor = Color.FromArgb(34, 136, 255),
                BorderWidth = 2,
                Opacity = 0.8f
            };

            horizontalBarDataset.AddPoint("产品A", 120, 1);
            horizontalBarDataset.AddPoint("产品B", 150, 2);
            horizontalBarDataset.AddPoint("产品C", 180, 3);
            horizontalBarDataset.AddPoint("产品D", 200, 4);
            horizontalBarDataset.AddPoint("产品E", 220, 5);

            mainChart.AddDataset(horizontalBarDataset);
        }

        private void LoadStackedBarChartData()
        {
            mainChart.ChartType = TChartType.StackedBar;
            mainChart.ClearDatasets();

            var dataset1 = new ChartDataset("系列1", Color.FromArgb(24, 144, 255))
            {
                BorderColor = Color.FromArgb(24, 144, 255),
                BorderWidth = 1,
                Opacity = 0.8f
            };

            var dataset2 = new ChartDataset("系列2", Color.FromArgb(82, 196, 26))
            {
                BorderColor = Color.FromArgb(82, 196, 26),
                BorderWidth = 1,
                Opacity = 0.8f
            };

            var dataset3 = new ChartDataset("系列3", Color.FromArgb(250, 173, 20))
            {
                BorderColor = Color.FromArgb(250, 173, 20),
                BorderWidth = 1,
                Opacity = 0.8f
            };

            // 为每个数据点添加相同X值的数据
            dataset1.AddPoint("1月", 1, 30);
            dataset1.AddPoint("2月", 2, 40);
            dataset1.AddPoint("3月", 3, 35);
            dataset1.AddPoint("4月", 4, 50);

            dataset2.AddPoint("1月", 1, 20);
            dataset2.AddPoint("2月", 2, 25);
            dataset2.AddPoint("3月", 3, 30);
            dataset2.AddPoint("4月", 4, 35);

            dataset3.AddPoint("1月", 1, 15);
            dataset3.AddPoint("2月", 2, 20);
            dataset3.AddPoint("3月", 3, 25);
            dataset3.AddPoint("4月", 4, 30);

            mainChart.AddDataset(dataset1);
            mainChart.AddDataset(dataset2);
            mainChart.AddDataset(dataset3);
        }

        private void LoadStackedHorizontalBarChartData()
        {
            mainChart.ChartType = TChartType.StackedHorizontalBar;
            mainChart.ClearDatasets();

            var dataset1 = new ChartDataset("系列1", Color.FromArgb(24, 144, 255))
            {
                BorderColor = Color.FromArgb(24, 144, 255),
                BorderWidth = 1,
                Opacity = 0.8f
            };

            var dataset2 = new ChartDataset("系列2", Color.FromArgb(82, 196, 26))
            {
                BorderColor = Color.FromArgb(82, 196, 26),
                BorderWidth = 1,
                Opacity = 0.8f
            };

            var dataset3 = new ChartDataset("系列3", Color.FromArgb(250, 173, 20))
            {
                BorderColor = Color.FromArgb(250, 173, 20),
                BorderWidth = 1,
                Opacity = 0.8f
            };

            // 为每个数据点添加相同Y值的数据
            dataset1.AddPoint("产品A", 30, 1);
            dataset1.AddPoint("产品B", 40, 2);
            dataset1.AddPoint("产品C", 35, 3);
            dataset1.AddPoint("产品D", 50, 4);

            dataset2.AddPoint("产品A", 20, 1);
            dataset2.AddPoint("产品B", 25, 2);
            dataset2.AddPoint("产品C", 30, 3);
            dataset2.AddPoint("产品D", 35, 4);

            dataset3.AddPoint("产品A", 15, 1);
            dataset3.AddPoint("产品B", 20, 2);
            dataset3.AddPoint("产品C", 25, 3);
            dataset3.AddPoint("产品D", 30, 4);

            mainChart.AddDataset(dataset1);
            mainChart.AddDataset(dataset2);
            mainChart.AddDataset(dataset3);
        }

        private void LoadSteppedLineChartData()
        {
            mainChart.ChartType = TChartType.SteppedLine;
            mainChart.ClearDatasets();

            var steppedLineDataset = new ChartDataset("阶梯线", Color.FromArgb(255, 77, 79))
            {
                BorderColor = Color.FromArgb(255, 77, 79),
                BorderWidth = 3,
                Opacity = 0.8f
            };

            steppedLineDataset.AddPoint("1月", 1, 50);
            steppedLineDataset.AddPoint("2月", 2, 80);
            steppedLineDataset.AddPoint("3月", 3, 80);
            steppedLineDataset.AddPoint("4月", 4, 120);
            steppedLineDataset.AddPoint("5月", 5, 120);
            steppedLineDataset.AddPoint("6月", 6, 150);

            mainChart.AddDataset(steppedLineDataset);
        }

        private void LoadSteppedAreaChartData()
        {
            mainChart.ChartType = TChartType.SteppedArea;
            mainChart.ClearDatasets();

            var steppedAreaDataset = new ChartDataset("阶梯面积", Color.FromArgb(19, 206, 102))
            {
                BorderColor = Color.FromArgb(19, 206, 102),
                BorderWidth = 2,
                Opacity = 0.6f
            };

            steppedAreaDataset.AddPoint("1月", 1, 60);
            steppedAreaDataset.AddPoint("2月", 2, 90);
            steppedAreaDataset.AddPoint("3月", 3, 90);
            steppedAreaDataset.AddPoint("4月", 4, 130);
            steppedAreaDataset.AddPoint("5月", 5, 130);
            steppedAreaDataset.AddPoint("6月", 6, 160);

            mainChart.AddDataset(steppedAreaDataset);
        }

        private void Chart_PointClick(object sender, ChartPointClickEventArgs e)
        {
            var chart = sender as AntdUI.Chart;
            string chartName = chart?.Title ?? "未知图表";
            AntdUI.Message.info(form, $"点击了 {chartName} 的数据点: {e.DataPoint.Label}\nX: {e.DataPoint.X}\nY: {e.DataPoint.Y}");
        }

        private void Chart_AreaClick(object sender, ChartAreaClickEventArgs e)
        {
            var chart = sender as AntdUI.Chart;
            string chartName = chart?.Title ?? "未知图表";
            Console.WriteLine($"点击了 {chartName} 的图表区域: {e.Location}");
        }

        private void Chart_PointHover(object sender, ChartPointHoverEventArgs e)
        {
            var chart = sender as AntdUI.Chart;
            string chartName = chart?.Title ?? "未知图表";
            Console.WriteLine($"悬停在 {chartName} 的数据点: {e.DataPoint.Label}");
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            try
            {
                // 导出当前图表为PNG图片
                var bitmap = mainChart.DrawBitmap();
                if (bitmap != null)
                {
                    // 创建保存文件对话框
                    using (var saveDialog = new SaveFileDialog())
                    {
                        saveDialog.Filter = "PNG图片|*.png|JPEG图片|*.jpg|所有文件|*.*";
                        saveDialog.DefaultExt = "png";
                        saveDialog.FileName = $"图表_{DateTime.Now:yyyyMMdd_HHmmss}.png";

                        if (saveDialog.ShowDialog() == DialogResult.OK)
                        {
                            // 根据文件扩展名选择保存格式
                            var format = System.Drawing.Imaging.ImageFormat.Png;
                            if (saveDialog.FileName.ToLower().EndsWith(".jpg") || saveDialog.FileName.ToLower().EndsWith(".jpeg"))
                            {
                                format = System.Drawing.Imaging.ImageFormat.Jpeg;
                            }

                            // 保存图片
                            bitmap.Save(saveDialog.FileName, format);
                            bitmap.Dispose();

                            // 显示成功消息
                            AntdUI.Message.success(form, "图表导出成功！");
                        }
                    }
                }
                else
                {
                    AntdUI.Message.error(form, "导出失败，请重试。");
                }
            }
            catch (Exception ex)
            {
                AntdUI.Message.error(form, $"导出过程中发生错误：{ex.Message}");
            }
        }
    }
}
