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

using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;

namespace AntdUI
{
    /// <summary>
    /// 图表数据集
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ChartDataset
    {
        /// <summary>
        /// 数据集标签
        /// </summary>
        [Description("数据集标签"), Category("数据")]
        public string? Label { get; set; }

        /// <summary>
        /// 数据点集合
        /// </summary>
        [Description("数据点集合"), Category("数据")]
        [Editor(typeof(Design.CollectionEditor), typeof(UITypeEditor))]
        public List<ChartDataPoint> DataPoints { get; set; } = new List<ChartDataPoint>();

        /// <summary>
        /// 填充颜色
        /// </summary>
        [Description("填充颜色"), Category("外观")]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? FillColor { get; set; }

        /// <summary>
        /// 边框颜色
        /// </summary>
        [Description("边框颜色"), Category("外观")]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? BorderColor { get; set; }

        /// <summary>
        /// 边框宽度
        /// </summary>
        [Description("边框宽度"), Category("外观"), DefaultValue(1)]
        public float BorderWidth { get; set; } = 1;

        /// <summary>
        /// 图例框填充颜色
        /// </summary>
        [Description("图例框填充颜色"), Category("外观")]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? LegendBoxFillColor { get; set; }

        /// <summary>
        /// 是否可见
        /// </summary>
        [Description("是否可见"), Category("行为"), DefaultValue(true)]
        public bool Visible { get; set; } = true;

        /// <summary>
        /// Y轴格式
        /// </summary>
        [Description("Y轴格式"), Category("格式")]
        public string? YFormat { get; set; }

        /// <summary>
        /// X轴格式
        /// </summary>
        [Description("X轴格式"), Category("格式")]
        public string? XFormat { get; set; }

        /// <summary>
        /// 半径格式（用于气泡图等）
        /// </summary>
        [Description("半径格式"), Category("格式")]
        public string? RadiusFormat { get; set; }

        /// <summary>
        /// 透明度
        /// </summary>
        [Description("透明度"), Category("外观"), DefaultValue(1.0f)]
        public float Opacity { get; set; } = 1.0f;

        /// <summary>
        /// 创建数据集
        /// </summary>
        public ChartDataset() { }

        /// <summary>
        /// 创建数据集
        /// </summary>
        /// <param name="label">标签</param>
        public ChartDataset(string? label)
        {
            Label = label;
        }

        /// <summary>
        /// 创建数据集
        /// </summary>
        /// <param name="label">标签</param>
        /// <param name="fillColor">填充颜色</param>
        public ChartDataset(string? label, Color fillColor)
        {
            Label = label;
            FillColor = fillColor;
        }

        /// <summary>
        /// 添加数据点
        /// </summary>
        /// <param name="point">数据点</param>
        public void AddPoint(ChartDataPoint point)
        {
            DataPoints.Add(point);
        }

        /// <summary>
        /// 添加数据点
        /// </summary>
        /// <param name="label">标签</param>
        /// <param name="x">X轴值</param>
        /// <param name="y">Y轴值</param>
        public void AddPoint(string? label, double x, double y)
        {
            DataPoints.Add(new ChartDataPoint(label, x, y));
        }

        /// <summary>
        /// 添加数据点
        /// </summary>
        /// <param name="label">标签</param>
        /// <param name="x">X轴值</param>
        /// <param name="y">Y轴值</param>
        /// <param name="radius">半径值</param>
        public void AddPoint(string? label, double x, double y, double radius)
        {
            DataPoints.Add(new ChartDataPoint(label, x, y, radius));
        }

        /// <summary>
        /// 清除所有数据点
        /// </summary>
        public void Clear()
        {
            DataPoints.Clear();
        }

        /// <summary>
        /// 获取数据点数量
        /// </summary>
        public int Count => DataPoints.Count;

        public override string ToString()
        {
            return $"{Label} ({Count} 个数据点)";
        }
    }
}
