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

using System.ComponentModel;
using System.Drawing;

namespace AntdUI.Controls.Charts
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
