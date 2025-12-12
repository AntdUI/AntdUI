// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
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