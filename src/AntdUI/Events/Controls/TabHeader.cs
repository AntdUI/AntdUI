// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

namespace AntdUI
{
    public class TabChangedEventArgs : VEventArgs<TagTabItem>
    {
        public TabChangedEventArgs(TagTabItem value, int tabIndex) : base(value)
        {
            Index = tabIndex;
        }

        public int Index { get; private set; }
    }

    public class TabCloseEventArgs : TabChangedEventArgs
    {
        public TabCloseEventArgs(TagTabItem value, int tabIndex) : base(value, tabIndex) { }

        /// <summary>
        /// 取消操作
        /// </summary>
        public bool Cancel { get; set; }

        #region 设置

        public TabCloseEventArgs SetCancel(bool value = true)
        {
            Cancel = value;
            return this;
        }

        #endregion
    }
}