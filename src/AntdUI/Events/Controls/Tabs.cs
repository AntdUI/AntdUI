// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.Windows.Forms;

namespace AntdUI
{
    public class ClosingPageEventArgs : VEventArgs<TabPage>
    {
        public ClosingPageEventArgs(TabPage value) : base(value) { }
    }

    public delegate bool ClosingPageEventHandler(object sender, ClosingPageEventArgs e);

    public class TabsItemEventArgs : VMEventArgs<TabPage>
    {
        public TabsItemEventArgs(TabPage item, int index, Tabs.IStyle style, MouseEventArgs e) : base(item, e)
        {
            Index = index;
            Style = style;
        }

        public int Index { get; private set; }

        public Tabs.IStyle Style { get; private set; }

        /// <summary>
        /// 是否取消
        /// </summary>
        public bool Cancel { get; set; }

        #region 设置

        public TabsItemEventArgs SetCancel(bool value = true)
        {
            Cancel = value;
            return this;
        }

        #endregion
    }

    public class TabsIndexChangingEventArgs : IntEventArgs
    {
        public TabsIndexChangingEventArgs(int value) : base(value)
        {
        }

        /// <summary>
        /// 是否取消
        /// </summary>
        public bool Cancel { get; set; }

        #region 设置

        public TabsIndexChangingEventArgs SetCancel(bool value = true)
        {
            Cancel = value;
            return this;
        }

        #endregion
    }

    public delegate void TabsIndexChangingEventHandler(object sender, TabsIndexChangingEventArgs e);

    /// <summary>
    /// 点击事件
    /// </summary>
    public delegate void TabsItemEventHandler(object sender, TabsItemEventArgs e);
}