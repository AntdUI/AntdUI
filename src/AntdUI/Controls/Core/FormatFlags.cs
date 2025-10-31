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

namespace AntdUI
{
    [Flags]
    public enum FormatFlags : int
    {
        /// <summary>
        /// 内容顶部对齐
        /// </summary>
        Top = 1,
        /// <summary>
        /// 内容垂直居中
        /// </summary>
        VerticalCenter = 2,
        /// <summary>
        /// 内容底部对齐
        /// </summary>
        Bottom = 4,

        /// <summary>
        /// 内容向左对齐
        /// </summary>
        Left = 8,
        /// <summary>
        /// 内容水平居中
        /// </summary>
        HorizontalCenter = 16,
        /// <summary>
        /// 内容向右对齐
        /// </summary>
        Right = 32,

        /// <summary>
        /// 文本内容不换行（\n无效）
        /// </summary>
        NoWrap = 64,
        /// <summary>
        /// 内容超出显示省略号
        /// </summary>
        EllipsisCharacter = 128,

        /// <summary>
        /// 垂直水平居中（组合值）
        /// </summary>
        Center = VerticalCenter | HorizontalCenter,

        /// <summary>
        /// 不换行且超出显示省略号（组合值）
        /// </summary>
        NoWrapEllipsis = NoWrap | EllipsisCharacter
    }
}