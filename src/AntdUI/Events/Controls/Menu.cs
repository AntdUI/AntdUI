// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.Windows.Forms;

namespace AntdUI
{
    public class MenuSelectEventArgs : VEventArgs<MenuItem>
    {
        public MenuSelectEventArgs(MenuItem value) : base(value) { }
    }


    public delegate void SelectEventHandler(object sender, MenuSelectEventArgs e);

    public delegate bool SelectBoolEventHandler(object sender, MenuSelectEventArgs e);

    public class MenuItemEventArgs : VMEventArgs<MenuItem>
    {
        public MenuItemEventArgs(MenuItem item, MouseEventArgs e) : base(item, e) { }
        public MenuItemEventArgs(MenuItem item, MouseEventArgs e, int click) : base(item, e, click) { }
    }

    /// <summary>
    /// 点击事件
    /// </summary>
    public delegate void MenuItemEventHandler(object sender, MenuItemEventArgs e);

    public class MenuCustomButtonEventArgs : VEventArgs<MenuButton>
    {
        public MenuCustomButtonEventArgs(MenuButton value, MenuItem item) : base(value) { Item = item; }

        public MenuItem Item { get; private set; }
    }

    public delegate void MenuCustomButtonEventHandler(object sender, MenuCustomButtonEventArgs e);
}