// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.Windows.Forms;

namespace AntdUI
{
    public class StepsItemEventArgs : VMEventArgs<StepsItem>
    {
        public StepsItemEventArgs(StepsItem item, MouseEventArgs e) : base(item, e) { }
    }

    /// <summary>
    /// 点击项时发生
    /// </summary>
    public delegate void StepsItemEventHandler(object sender, StepsItemEventArgs e);
}