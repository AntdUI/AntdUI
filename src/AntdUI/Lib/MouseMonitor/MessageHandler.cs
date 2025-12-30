// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

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

        #region 方法

        public void SetEnabled(bool value)
        {
            if (_isFilter)
            {
                if (value) Application.AddMessageFilter(this);
                else Application.RemoveMessageFilter(this);
            }
            else
            {
                if (value) RegisterToHook();
                else UnregisterFromHook();
            }
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
                    msg.IMOUSECLICK();
                    break;
                case MessageHookManager.WM_NCMOUSEMOVE:
                case MessageHookManager.WM_MOUSEMOVE:
                case MessageHookManager.WM_MOUSELEAVE:
                    msg.IMOUSEMOVE();
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