// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.Windows.Forms;

namespace AntdUI
{
    public class SegmentedItemEventArgs : VMEventArgs<SegmentedItem>
    {
        public SegmentedItemEventArgs(SegmentedItem item, MouseEventArgs e) : base(item, e) { }
    }

    /// <summary>
    /// 点击事件
    /// </summary>
    public delegate void SegmentedItemEventHandler(object sender, SegmentedItemEventArgs e);
}