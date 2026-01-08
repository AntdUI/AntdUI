// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;

namespace AntdUI
{
    [Flags]
    public enum CloseMode : int
    {
        /// <summary>
        /// 不做任何处理
        /// </summary>
        None = 0,
        /// <summary>
        /// 点击下拉其他区域
        /// </summary>
        Click = 1,
        /// <summary>
        /// 离开下拉或控件
        /// </summary>
        Leave = 2,
        /// <summary>
        /// 不包含控件
        /// </summary>
        NoControl = 4
    }
}