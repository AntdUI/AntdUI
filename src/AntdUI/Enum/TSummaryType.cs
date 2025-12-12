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