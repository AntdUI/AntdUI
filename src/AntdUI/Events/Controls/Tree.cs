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

        #region 设置

        public TreeExpandEventArgs SetCanExpand(bool value = false)
        {
            CanExpand = value;
            return this;
        }

        #endregion
    }

    public delegate void TreeExpandEventHandler(object sender, TreeExpandEventArgs e);

    public class TreeDropEventArgs : EventArgs
    {
        public TreeDropEventArgs(TreeItem dragNode, TreeItem? targetParent, int targetIndex, TreeDropMode mode)
        {
            DragNode = dragNode;
            TargetParent = targetParent;
            TargetIndex = targetIndex;
            DropMode = mode;
        }
        /// <summary>
        /// 被拖拽节点（对齐 antd info.dragNode）
        /// </summary>
        public TreeItem DragNode { get; private set; }
        /// <summary>
        /// 迁移后归属父节点（根层级为 null）。注：antd info.node 指目标节点本身，与本字段语义不同
        /// </summary>
        public TreeItem? TargetParent { get; private set; }
        /// <summary>
        /// 目标位置索引（本控件独有精确插入索引，antd 无对应字段）
        /// </summary>
        public int TargetIndex { get; private set; }
        /// <summary>
        /// 放置模式（对齐 antd dropPosition + dropToGap 压缩表达）
        /// </summary>
        public TreeDropMode DropMode { get; private set; }
        /// <summary>
        /// 取消本次拖拽迁移
        /// </summary>
        public bool Cancel { get; set; } = false;

        #region 设置

        public TreeDropEventArgs SetCancel(bool value = true)
        {
            Cancel = value;
            return this;
        }

        #endregion
    }

    public delegate void TreeDropEventHandler(object sender, TreeDropEventArgs e);

    public class TreeDropDoneEventArgs : EventArgs
    {
        public TreeDropDoneEventArgs(TreeItem dragNode, TreeItem? targetParent, int targetIndex, TreeDropMode mode)
        {
            DragNode = dragNode;
            TargetParent = targetParent;
            TargetIndex = targetIndex;
            DropMode = mode;
        }
        /// <summary>
        /// 被拖拽节点（对齐 antd info.dragNode）
        /// </summary>
        public TreeItem DragNode { get; private set; }
        /// <summary>
        /// 迁移后归属父节点（根层级为 null）。注：antd info.node 指目标节点本身，与本字段语义不同
        /// </summary>
        public TreeItem? TargetParent { get; private set; }
        /// <summary>
        /// 目标位置索引（本控件独有精确插入索引，antd 无对应字段）
        /// </summary>
        public int TargetIndex { get; private set; }
        /// <summary>
        /// 放置模式（对齐 antd dropPosition + dropToGap 压缩表达）
        /// </summary>
        public TreeDropMode DropMode { get; private set; }
    }

    public delegate void TreeDropDoneEventHandler(object sender, TreeDropDoneEventArgs e);
}