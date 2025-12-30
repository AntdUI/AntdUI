// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.ComponentModel;
using System.Drawing;

namespace AntdUI
{
    /// <summary>
    /// 图表数据点
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ChartDataPoint
    {
        /// <summary>
        /// 标签
        /// </summary>
        [Description("标签"), Category("数据")]
        public string? Label { get; set; }

        /// <summary>
        /// X轴值
        /// </summary>
        [Description("X轴值"), Category("数据")]
        public double X { get; set; }

        /// <summary>
        /// Y轴值
        /// </summary>
        [Description("Y轴值"), Category("数据")]
        public double Y { get; set; }

        /// <summary>
        /// 半径值（用于气泡图等）
        /// </summary>
        [Description("半径值"), Category("数据")]
        public double Radius { get; set; }

        /// <summary>
        /// 颜色
        /// </summary>
        [Description("颜色"), Category("外观")]
        public Color? Color { get; set; }

        /// <summary>
        /// 是否可见
        /// </summary>
        [Description("是否可见"), Category("行为"), DefaultValue(true)]
        public bool Visible { get; set; } = true;

        /// <summary>
        /// 工具提示文本
        /// </summary>
        [Description("工具提示文本"), Category("行为")]
        public string? Tooltip { get; set; }

        /// <summary>
        /// 创建数据点
        /// </summary>
        public ChartDataPoint() { }

        /// <summary>
        /// 创建数据点
        /// </summary>
        /// <param name="label">标签</param>
        /// <param name="x">X轴值</param>
        /// <param name="y">Y轴值</param>
        public ChartDataPoint(string? label, double x, double y)
        {
            Label = label;
            X = x;
            Y = y;
        }

        /// <summary>
        /// 创建数据点
        /// </summary>
        /// <param name="label">标签</param>
        /// <param name="x">X轴值</param>
        /// <param name="y">Y轴值</param>
        /// <param name="radius">半径值</param>
        public ChartDataPoint(string? label, double x, double y, double radius)
        {
            Label = label;
            X = x;
            Y = y;
            Radius = radius;
        }

        /// <summary>
        /// 创建数据点
        /// </summary>
        /// <param name="label">标签</param>
        /// <param name="x">X轴值</param>
        /// <param name="y">Y轴值</param>
        /// <param name="color">颜色</param>
        public ChartDataPoint(string? label, double x, double y, Color color)
        {
            Label = label;
            X = x;
            Y = y;
            Color = color;
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}
