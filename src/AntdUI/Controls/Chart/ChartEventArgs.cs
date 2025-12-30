// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

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
