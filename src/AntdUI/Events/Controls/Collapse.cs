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

namespace AntdUI
{
    public class CollapseExpandEventArgs : VEventArgs<CollapseItem>
    {
        public CollapseExpandEventArgs(CollapseItem value, bool expand, Rectangle rectTitle, Rectangle rectControl) : base(value)
        {
            Expand = expand;
            RectTitle = rectTitle;
            RectControl = rectControl;
        }

        public bool Expand { get; private set; }
        public Rectangle RectTitle { get; private set; }
        public Rectangle RectControl { get; private set; }
    }

    public delegate void CollapseExpandEventHandler(object sender, CollapseExpandEventArgs e);

    public class CollapseExpandingEventArgs : CollapseExpandEventArgs
    {
        public CollapseExpandingEventArgs(CollapseItem value, bool expand, Rectangle rectTitle, Rectangle rectControl, Point location) : base(value, expand, rectTitle, rectControl)
        {
            Location = location;
        }

        public Point Location { get; private set; }
    }

    /// <summary>
    /// Collapse 类型展开/折叠进行时事件
    /// </summary>
    public delegate void CollapseExpandingEventHandler(object sender, CollapseExpandingEventArgs e);

    public class CollapseButtonClickEventArgs : VEventArgs<CollapseGroupButton>
    {
        public CollapseButtonClickEventArgs(CollapseGroupButton value, CollapseItem parent) : base(value)
        {
            Parent = parent;
        }

        public CollapseItem Parent { get; private set; }
    }

    /// <summary>
    /// CollapseItem.Button单击事件
    /// </summary>
    public delegate void CollapseButtonClickEventHandler(object sender, CollapseButtonClickEventArgs e);

    public class CollapseSwitchCheckedChangedEventArgs : CollapseButtonClickEventArgs
    {
        public CollapseSwitchCheckedChangedEventArgs(CollapseGroupButton switchItem, CollapseItem parent, bool _checked) : base(switchItem, parent) { Checked = _checked; }

        public bool Checked { get; private set; }

    }

    public delegate void CollapseSwitchCheckedChangedEventHandler(object sender, CollapseSwitchCheckedChangedEventArgs e);

    public class CollapseEditChangedEventArgs : VEventArgs<object>
    {
        public CollapseEditChangedEventArgs(Collapse parent, CollapseItem parentItem, object value) : base(value)
        {
            Parent = parent;
            ParentItem = parentItem;
        }
        public Collapse Parent { get; private set; }
        public CollapseItem ParentItem { get; private set; }

    }
    public delegate void CollapseEditChangedEventHandler(object sender, CollapseEditChangedEventArgs e);

    public class CollapseCustomInputEditEventArgs : EventArgs
    {
        public CollapseCustomInputEditEventArgs() { }

        public IControl Edit { get; set; }
    }
    public delegate void CollapseCustomInputEditEventHandler(object sender, CollapseCustomInputEditEventArgs e);
}