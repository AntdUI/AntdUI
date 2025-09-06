﻿// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
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