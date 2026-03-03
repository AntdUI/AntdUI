// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;

namespace AntdUI
{
    [Flags]
    public enum FormatFlags : int
    {
        /// <summary>
        /// 内容垂直顶部对齐
        /// </summary>
        Top = 1,
        /// <summary>
        /// 内容垂直居中
        /// </summary>
        VerticalCenter = 2,
        /// <summary>
        /// 内容垂直底部对齐
        /// </summary>
        Bottom = 4,

        /// <summary>
        /// 内容水平向左对齐
        /// </summary>
        Left = 8,
        /// <summary>
        /// 内容水平居中
        /// </summary>
        HorizontalCenter = 16,
        /// <summary>
        /// 内容水平向右对齐
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
        /// 显示热键符号
        /// </summary>
        HotkeyPrefixShow = 256,

        DirectionVertical = 512,

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