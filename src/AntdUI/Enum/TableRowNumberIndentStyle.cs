// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
// THE AntdUI PROJECT IS AN WINFORM LIBRARY LICENSED UNDER THE Apache-2.0 License.
// LICENSED UNDER THE Apache License, VERSION 2.0 (THE "License")
// YOU MAY NOT USE THIS FILE EXCEPT IN COMPLIANCE WITH THE License.
// YOU OBTAIN A COPY OF THE LICENSE AT
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
    ///  行号列缩进样式（VisibleGrouped 模式下子行显示方式）
    /// </summary>
    public enum TableRowNumberIndentStyle
    {
        /// <summary>
        /// 无缩进
        /// </summary>
        None = 0,
        /// <summary>
        /// 仅缩进
        /// </summary>
        Indent = 1,
        /// <summary>
        /// 缩进 + 分割线
        /// </summary>
        IndentLine = 2,
        /// <summary>
        /// 缩进 + 点号格式（如：1.1, 1.2）
        /// </summary>
        IndentDot = 3,
        /// <summary>
        /// 缩进 + 横线格式（如：1-1, 1-2）
        /// </summary>
        IndentDash = 4,
        /// <summary>
        /// 仅点号格式（如：1.1, 1.2）（无缩进）
        /// </summary>
        Dot = 5,
        /// <summary>
        /// 仅横线格式（如：1-1, 1-2）（无缩进）
        /// </summary>
        Dash = 6
    }
}

