// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

namespace AntdUI
{
    /// <summary>
    /// 筛选数据源
    /// </summary>
    public enum FilterSource
    {
        /// <summary>
        /// 当前筛选结果集
        /// </summary>
        Current = 0,
        /// <summary>
        /// 先从当前筛选结果集获取数据源，如果没有数据则从原始数据源获取
        /// </summary>
        CurrentFirst = 1,
        /// <summary>
        /// 始终从原始数据源获取数据
        /// </summary>
        DataSource = 2,
    }

    /// <summary>
    /// 筛选条件
    /// </summary>
    public enum FilterConditions
    {
        /// <summary>
        /// 等于
        /// </summary>
        Equal = 0,
        /// <summary>
        /// 不等于
        /// </summary>
        NotEqual = 1,
        /// <summary>
        /// 大于
        /// </summary>
        Greater = 2,
        /// <summary>
        /// 小于
        /// </summary>
        Less = 3,
        /// <summary>
        /// 存在...
        /// </summary>
        Contain = 4,
        /// <summary>
        /// 不存在...
        /// </summary>
        NotContain = 5,
        /// <summary>
        /// 不启用
        /// </summary>
        None = 6,
    }
}