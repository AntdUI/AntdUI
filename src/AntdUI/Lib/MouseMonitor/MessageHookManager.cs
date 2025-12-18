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
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AntdUI
{
    public class MessageHookManager : IDisposable
    {
        public MessageHookManager()
        {
            _mouseProc = MouseHookCallback;
            _keyboardProc = KeyboardHookCallback;
            try
            {
                using (var process = System.Diagnostics.Process.GetCurrentProcess())
                using (var module = process.MainModule)
                {
                    hMod = GetModuleHandle(module!.ModuleName!);
                    if (!InstallHooks()) throw new Exception("Hook Error");
                }
            }
            catch
            {
                // 钩子初始化失败，记录日志
                Dispose();
                throw;
            }
        }

        #region 钩子常量和API

        private const int WH_MOUSE_LL = 14, WH_KEYBOARD_LL = 13;
        public const int WM_LBUTTONDOWN = 0x201, WM_RBUTTONDOWN = 0x204, WM_MBUTTONDOWN = 0x207, WM_NCMOUSEMOVE = 0xa0, WM_MOUSEMOVE = 0x200, WM_MOUSELEAVE = 0x2a3, WM_KEYDOWN = 0x100;

        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);
        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        #endregion

        #region 注册的消息处理器列表

        private readonly object _lock = new object();
        private List<IMessage> _messageHandlers = new List<IMessage>();
        private IMessage[] Handlers()
        {
            lock (_lock)
            {
                return _messageHandlers.ToArray();
            }
        }

        #endregion

        #region 钩子句柄和回调

        private IntPtr _mouseHookHandle = IntPtr.Zero;
        private IntPtr _keyboardHookHandle = IntPtr.Zero;
        private LowLevelMouseProc _mouseProc;
        private LowLevelKeyboardProc _keyboardProc;

        #endregion

        #region 钩子回调函数

        private IntPtr MouseHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            var r = CallNextHookEx(_mouseHookHandle, nCode, wParam, lParam);
            if (nCode >= 0)
            {
                try
                {
                    switch (wParam.ToInt32())
                    {
                        case WM_LBUTTONDOWN:
                        case WM_RBUTTONDOWN:
                        case WM_MBUTTONDOWN:
                            foreach (var handler in Handlers())
                            {
                                try
                                {
                                    handler.IMOUSECLICK();
                                }
                                catch { }
                            }
                            break;
                        case WM_NCMOUSEMOVE:
                        case WM_MOUSEMOVE:
                            foreach (var handler in Handlers())
                            {
                                try
                                {
                                    handler.IMOUSEMOVE();
                                }
                                catch { }
                            }
                            break;
                    }
                }
                catch
                { }
            }
            return r;
        }

        private IntPtr KeyboardHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam.ToInt32() == WM_KEYDOWN)
            {
                try
                {
                    var key = (Keys)Marshal.ReadInt32(lParam);
                    int count = 0;
                    foreach (var handler in Handlers())
                    {
                        try
                        {
                            if (handler.IKEYS(key)) count++;
                        }
                        catch { }
                    }
#if NET40 || NET46 || NET48 || NET6_0
                    if (count > 0) return (IntPtr)1;
#else
                    if (count > 0) return 1;
#endif
                }
                catch { }
            }

            return CallNextHookEx(_keyboardHookHandle, nCode, wParam, lParam);
        }

        #endregion

        #region 注册和注销消息处理器

        /// <summary>
        /// 注册消息处理器
        /// </summary>
        public void RegisterHandler(IMessage handler)
        {
            lock (_lock)
            {
                int count = _messageHandlers.Count;
                if (_messageHandlers.Contains(handler)) return;
                _messageHandlers.Add(handler);
                if (count == 0) InstallHooks();
            }
        }

        /// <summary>
        /// 注销消息处理器
        /// </summary>
        public void UnregisterHandler(IMessage handler)
        {
            lock (_lock)
            {
                if (_messageHandlers.Contains(handler))
                {
                    _messageHandlers.Remove(handler);
                    if (_messageHandlers.Count == 0) UninstallHooks();
                }
            }
        }

        #endregion

        #region 安装卸载

        nint hMod;
        /// <summary>
        /// 安装钩子
        /// </summary>
        private bool InstallHooks()
        {
            if (_mouseHookHandle == IntPtr.Zero)
            {
                try
                {
                    // 注册鼠标钩子
                    _mouseHookHandle = SetWindowsHookEx(WH_MOUSE_LL, _mouseProc, hMod, 0);
                    if (_mouseHookHandle == IntPtr.Zero) return false;

                    // 注册键盘钩子
                    _keyboardHookHandle = SetWindowsHookEx(WH_KEYBOARD_LL, _keyboardProc, hMod, 0);
                    if (_keyboardHookHandle == IntPtr.Zero) return false;
                    return true;
                }
                catch { return false; }
            }
            else return true;
        }

        /// <summary>
        /// 卸载钩子
        /// </summary>
        private void UninstallHooks()
        {
            // 卸载鼠标钩子
            if (_mouseHookHandle != IntPtr.Zero)
            {
                UnhookWindowsHookEx(_mouseHookHandle);
                _mouseHookHandle = IntPtr.Zero;
            }

            // 卸载键盘钩子
            if (_keyboardHookHandle != IntPtr.Zero)
            {
                UnhookWindowsHookEx(_keyboardHookHandle);
                _keyboardHookHandle = IntPtr.Zero;
            }
        }

        #endregion

        #region IDisposable实现

        public void Dispose()
        {
            UninstallHooks();
            lock (_lock)
            {
                _messageHandlers.Clear();
            }
        }

        #endregion
    }
}