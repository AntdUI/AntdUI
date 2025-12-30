// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;

namespace AntdUI
{
    public class IntXYEventArgs : EventArgs
    {
        public IntXYEventArgs(int x, int y) { X = x; Y = y; }
        public int X { get; private set; }
        public int Y { get; private set; }
    }

    public delegate void IntXYEventHandler(object sender, IntXYEventArgs e);
}