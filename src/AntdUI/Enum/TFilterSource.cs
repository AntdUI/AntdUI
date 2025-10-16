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