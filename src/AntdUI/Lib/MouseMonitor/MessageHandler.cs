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
    public class MessageHandler : IMessageFilter, IDisposable
    {
        #region 构造函数和析构函数

        IMessage msg;
        public MessageHandler(IMessage message)
        {
            msg = message;
            // 注册相应的处理机制
            if (Config.UseHook && RegisterToHook()) return;
            _isFilter = true;
            Application.AddMessageFilter(this);
        }

        #endregion

        private bool _isHook = false, _isFilter = false;

        #region 钩子注册和注销

        MessageHookManager? messageHookManager;
        public MessageHookManager InstanceHook()
        {
            messageHookManager ??= new MessageHookManager();
            return messageHookManager;
        }

        /// <summary>
        /// 注册到钩子管理器
        /// </summary>
        public bool RegisterToHook()
        {
            if (_isHook) return true;
            try
            {
                InstanceHook().RegisterHandler(msg);
                _isHook = true;
                return true;
            }
            catch
            {
                Config.UseHook = false;
                return false;
            }
        }

        /// <summary>
        /// 从钩子管理器注销
        /// </summary>
        public void UnregisterFromHook()
        {
            if (_isHook)
            {
                messageHookManager!.UnregisterHandler(msg);
                _isHook = false;
            }
        }

        #endregion

        #region IMessageFilter

        /// <summary>
        /// 消息过滤实现
        /// </summary>
        public bool PreFilterMessage(ref System.Windows.Forms.Message m)
        {
            switch (m.Msg)
            {
                case MessageHookManager.WM_LBUTTONDOWN:
                case MessageHookManager.WM_RBUTTONDOWN:
                case MessageHookManager.WM_MBUTTONDOWN:
                case MessageHookManager.WM_NCMOUSEMOVE:
                    msg.IMOUSECLICK();
                    break;
                case MessageHookManager.WM_MOUSELEAVE:
                    msg.IMOUSELEAVE();
                    break;
                case MessageHookManager.WM_KEYDOWN:
                    return msg.IKEYS((Keys)(int)m.WParam);
            }
            return false;
        }

        #endregion

        public void Dispose()
        {
            UnregisterFromHook();
            if (_isFilter)
            {
                _isFilter = false;
                Application.RemoveMessageFilter(this);
            }
        }
    }
}