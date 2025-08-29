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

namespace AntdUI
{
    /// <summary>
    /// 图表类型
    /// </summary>
    public enum TChartType
    {
        /// <summary>
        /// 面积图 - 绘制数据点并强调累积总量
        /// </summary>
        Area,
        /// <summary>
        /// 柱状图 - 用柱状图直观比较不同类别的数据
        /// </summary>
        Bar,
        /// <summary>
        /// 气泡图 - 用不同大小的圆圈表示数据点
        /// </summary>
        Bubble,
        /// <summary>
        /// 环形图 - 以环形饼图显示数据
        /// </summary>
        Doughnut,
        /// <summary>
        /// 水平柱状图 - 使用水平柱状图提供另一种视角
        /// </summary>
        HorizontalBar,
        /// <summary>
        /// 折线图 - 连接数据点以显示趋势和模式
        /// </summary>
        Line,
        /// <summary>
        /// 饼图 - 将数据表示为传统饼图的切片
        /// </summary>
        Pie,
        /// <summary>
        /// 极坐标面积图 - 径向排列数据点以获得独特视图
        /// </summary>
        PolarArea,
        /// <summary>
        /// 雷达图 - 在雷达图上呈现多变量数据点
        /// </summary>
        Radar,
        /// <summary>
        /// 散点图 - 散点数据以识别关系
        /// </summary>
        Scatter,
        /// <summary>
        /// 样条曲线图 - 平滑曲线无缝连接数据点
        /// </summary>
        Spline,
        /// <summary>
        /// 样条面积图 - 将样条曲线与面积图混合以获得影响
        /// </summary>
        SplineArea,
        /// <summary>
        /// 堆叠柱状图 - 堆叠数据类别以获得整体表示
        /// </summary>
        StackedBar,
        /// <summary>
        /// 堆叠水平柱状图 - 水平堆叠柱状图以获得引人入胜的可视化效果
        /// </summary>
        StackedHorizontalBar,
        /// <summary>
        /// 阶梯面积图 - 创建阶梯面积图以显示明显的趋势
        /// </summary>
        SteppedArea,
        /// <summary>
        /// 阶梯线图 - 用阶梯线段说明数据变化
        /// </summary>
        SteppedLine
    }
}
