// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Drawing;
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

    #region 绘制

    public delegate void MenuItemPaintBeginEventHandler(object sender, MenuItemPaintBeginEventArgs e);

    public class MenuItemPaintBeginEventArgs : MenuItemPaintEventArgs
    {
        public MenuItemPaintBeginEventArgs(Canvas canvas, MenuItem it, Rectangle rect, float radius, Color fore, Color fore_active, Color back_hover, Color back_active) : base(canvas, it, rect, radius, fore, fore_active, back_hover, back_active)
        {
        }

        /// <summary>
        /// 是否处理
        /// </summary>
        public bool Handled { get; set; }

        #region 设置

        public MenuItemPaintEventArgs SetHandled(bool value = true)
        {
            Handled = value;
            return this;
        }

        #endregion
    }

    public delegate void MenuItemPaintEventHandler(object sender, MenuItemPaintEventArgs e);

    public class MenuItemPaintEventArgs : EventArgs
    {
        public MenuItemPaintEventArgs(Canvas canvas, MenuItem it, Rectangle rect, float radius, Color fore, Color fore_active, Color back_hover, Color back_active)
        {
            g = canvas;
            Item = it;
            Rect = rect;
            Radius = radius;
            Fore = fore;
            ForeActive = fore_active;
            BackHover = back_hover;
            BackActive = back_active;
        }

        /// <summary>
        /// 画板
        /// </summary>
        public Canvas g { get; private set; }

        /// <summary>
        /// 菜单项
        /// </summary>
        public MenuItem Item { get; private set; }

        /// <summary>
        /// 区域
        /// </summary>
        public Rectangle Rect { get; private set; }

        /// <summary>
        /// 圆角值
        /// </summary>
        public float Radius { get; private set; }

        /// <summary>
        /// 文本色
        /// </summary>
        public Color Fore { get; private set; }

        /// <summary>
        /// 焦点文本色
        /// </summary>
        public Color ForeActive { get; private set; }

        /// <summary>
        /// 悬停背景色
        /// </summary>
        public Color BackHover { get; private set; }

        /// <summary>
        /// 焦点背景色
        /// </summary>
        public Color BackActive { get; private set; }
    }

    #endregion
}