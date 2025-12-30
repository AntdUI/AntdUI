// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.Windows.Forms;

namespace AntdUI
{
    public class BreadcrumbItemEventArgs : VMEventArgs<BreadcrumbItem>
    {
        public BreadcrumbItemEventArgs(BreadcrumbItem item, MouseEventArgs e) : base(item, e) { }
    }

    /// <summary>
    /// 点击事件
    /// </summary>
    public delegate void BreadcrumbItemEventHandler(object sender, BreadcrumbItemEventArgs e);
}