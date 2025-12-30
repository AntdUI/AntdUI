// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Windows.Forms;

namespace AntdUI
{
    #region ChatList

    public class ChatItemEventArgs : VMEventArgs<Chat.IChatItem>
    {
        public ChatItemEventArgs(Chat.IChatItem item, MouseEventArgs e) : base(item, e) { }
    }

    /// <summary>
    /// 点击事件
    /// </summary>
    public delegate void ClickEventHandler(object sender, ChatItemEventArgs e);

    #endregion

    #region MsgList

    /// <summary>
    /// MsgItem事件参数
    /// </summary>
    public class MsgItemEventArgs : EventArgs
    {
        public MsgItemEventArgs(Chat.MsgItem item) { Item = item; }
        /// <summary>
        /// 消息项目
        /// </summary>
        public Chat.MsgItem Item { get; private set; }
    }

    /// <summary>
    /// MsgItem点击事件参数
    /// </summary>
    public class MsgItemClickEventArgs : MouseEventArgs
    {
        public MsgItemClickEventArgs(Chat.MsgItem item, MouseEventArgs e) : base(e.Button, e.Clicks, e.X, e.Y, e.Delta)
        {
            Item = item;
        }
        /// <summary>
        /// 消息项目
        /// </summary>
        public Chat.MsgItem Item { get; private set; }
    }

    /// <summary>
    /// 项目选中事件处理器
    /// </summary>
    public delegate void ItemSelectedEventHandler(object sender, MsgItemEventArgs e);

    /// <summary>
    /// 项目点击事件处理器
    /// </summary>
    public delegate void ItemClickEventHandler(object sender, MsgItemClickEventArgs e);

    #endregion
}