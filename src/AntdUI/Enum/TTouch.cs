// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;

namespace AntdUI
{
    [Flags]
    public enum TTouchAction
    {
        None = 0,
        Ready = 1 << 0,
        X = 1 << 1,
        Y = 1 << 2,
        All = X | Y
    }
}