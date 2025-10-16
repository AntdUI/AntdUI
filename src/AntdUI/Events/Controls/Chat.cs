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