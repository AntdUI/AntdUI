// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

namespace AntdUI
{
    /// <summary>
    /// 汇总类型
    /// </summary>
    public enum TSummaryType
    {
        /// <summary>
        /// 不汇总 (默认)
        /// </summary>
        None = 0,
        /// <summary>
        /// 仅显示文本 (如：TOTAL：, 汇总:, SUM...)
        /// </summary>
        Text = 1,
        /// <summary>
        /// 总和
        /// </summary>
        SUM = 2,
        /// <summary>
        /// 平均
        /// </summary>
        AVG = 3,
        /// <summary>
        /// 最小值
        /// </summary>
        MIN = 4,
        /// <summary>
        /// 最大值
        /// </summary>
        MAX = 5,
        /// <summary>
        /// 计数
        /// </summary>
        Count = 6,
        /// <summary>
        /// 自定义
        /// </summary>
        Custom = 7,
    }
}