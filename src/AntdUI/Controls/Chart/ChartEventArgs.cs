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
using System.Drawing;

namespace AntdUI
{
    /// <summary>
    /// 图表数据点点击事件参数
    /// </summary>
    public class ChartPointClickEventArgs : EventArgs
    {
        /// <summary>
        /// 数据点
        /// </summary>
        public ChartDataPoint DataPoint { get; }

        /// <summary>
        /// 数据集索引
        /// </summary>
        public int DatasetIndex { get; }

        /// <summary>
        /// 数据点索引
        /// </summary>
        public int PointIndex { get; }

        public ChartPointClickEventArgs(ChartDataPoint dataPoint, int datasetIndex = -1, int pointIndex = -1)
        {
            DataPoint = dataPoint;
            DatasetIndex = datasetIndex;
            PointIndex = pointIndex;
        }
    }

    /// <summary>
    /// 图表区域点击事件参数
    /// </summary>
    public class ChartAreaClickEventArgs : EventArgs
    {
        /// <summary>
        /// 点击位置
        /// </summary>
        public Point Location { get; }

        public ChartAreaClickEventArgs(Point location)
        {
            Location = location;
        }
    }

    /// <summary>
    /// 图表数据点悬停事件参数
    /// </summary>
    public class ChartPointHoverEventArgs : EventArgs
    {
        /// <summary>
        /// 数据点
        /// </summary>
        public ChartDataPoint DataPoint { get; }

        /// <summary>
        /// 鼠标位置
        /// </summary>
        public Point Location { get; }

        /// <summary>
        /// 数据集索引
        /// </summary>
        public int DatasetIndex { get; }

        /// <summary>
        /// 数据点索引
        /// </summary>
        public int PointIndex { get; }

        public ChartPointHoverEventArgs(ChartDataPoint dataPoint, Point location, int datasetIndex = -1, int pointIndex = -1)
        {
            DataPoint = dataPoint;
            Location = location;
            DatasetIndex = datasetIndex;
            PointIndex = pointIndex;
        }
    }
}
