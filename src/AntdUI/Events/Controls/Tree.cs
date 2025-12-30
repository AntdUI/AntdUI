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
    public class TreeSelectEventArgs : VMEventArgs<TreeItem>
    {
        public TreeSelectEventArgs(TreeItem item, Rectangle rect, TreeCType type, MouseEventArgs e) : base(item, e)
        {
            Rect = rect;
            Type = type;
        }
        public Rectangle Rect { get; private set; }
        public TreeCType Type { get; private set; }
    }

    public delegate void TreeSelectEventHandler(object sender, TreeSelectEventArgs e);

    public class TreeHoverEventArgs : EventArgs
    {
        public TreeHoverEventArgs(TreeItem item, Rectangle rect, bool hover)
        {
            Item = item;
            Hover = hover;
            Rect = rect;
        }
        public TreeItem Item { get; private set; }
        public Rectangle Rect { get; private set; }
        public bool Hover { get; private set; }
    }

    public delegate void TreeHoverEventHandler(object sender, TreeHoverEventArgs e);

    public class TreeCheckedEventArgs : EventArgs
    {
        public TreeCheckedEventArgs(TreeItem item, bool value)
        {
            Item = item;
            Value = value;
        }
        public TreeItem Item { get; private set; }
        public bool Value { get; private set; }
    }

    public delegate void TreeCheckedEventHandler(object sender, TreeCheckedEventArgs e);

    public class TreeExpandEventArgs : EventArgs
    {
        public TreeExpandEventArgs(TreeItem item, bool value)
        {
            Item = item;
            Value = value;
        }
        public TreeItem Item { get; private set; }
        public bool Value { get; private set; }
        public bool CanExpand { get; set; } = true;

        #region …Ë÷√

        public TreeExpandEventArgs SetCanExpand(bool value = false)
        {
            CanExpand = value;
            return this;
        }

        #endregion
    }

    public delegate void TreeExpandEventHandler(object sender, TreeExpandEventArgs e);
}