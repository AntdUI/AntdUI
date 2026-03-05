// THIS FILE IS PART OF Vanara PROJECT
// THE Vanara PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHT (C) dahall. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/dahall/Vanara

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace AntdUI
{
    partial class Win32
    {
        public static partial class User32
        {
            public static bool GetClipBoardText(out string? text)
            {
                IntPtr handle = default, pointer = default;
                try
                {
                    if (!OpenClipboard(IntPtr.Zero)) { text = null; return false; }
                    handle = GetClipboardData(13);
                    if (handle == default) { text = null; return false; }
                    pointer = GlobalLock(handle);
                    if (pointer == default) { text = null; return false; }
                    var size = GlobalSize(handle);
                    var buff = new byte[size];
                    Marshal.Copy(pointer, buff, 0, size);
                    text = Encoding.Unicode.GetString(buff).TrimEnd('\0');
                    return true;
                }
                catch
                {
                    text = null;
                    return false;
                }
                finally
                {
                    if (pointer != default) GlobalUnlock(handle);
                    CloseClipboard();
                }
            }
            public static bool SetClipBoardText(string? text)
            {
                IntPtr hGlobal = default;
                try
                {
                    if (!OpenClipboard(IntPtr.Zero)) return false;
                    if (!EmptyClipboard()) return false;
                    if (text == null) return true;

                    var bytes = (text.Length + 1) * 2;
                    hGlobal = Marshal.AllocHGlobal(bytes);
                    if (hGlobal == default) return false;
                    var target = GlobalLock(hGlobal);
                    if (target == default) return false;
                    try
                    {
                        Marshal.Copy(text.ToCharArray(), 0, target, text.Length);
                    }
                    finally
                    {
                        GlobalUnlock(target);
                    }
                    if (SetClipboardData(13, hGlobal) == default) return false;
                    hGlobal = default;
                    return true;
                }
                catch { return false; }
                finally
                {
                    if (hGlobal != default) Marshal.FreeHGlobal(hGlobal);
                    CloseClipboard();
                }
            }

            [DllImport("user32.dll", SetLastError = true)]
            public static extern IntPtr GetClipboardData(uint uFormat);

            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern IntPtr GlobalLock(IntPtr hMem);

            [DllImport("Kernel32.dll", SetLastError = true)]
            public static extern int GlobalSize(IntPtr hMem);

            [DllImport("kernel32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool GlobalUnlock(IntPtr hMem);

            /// <summary>
            /// 打开剪切板
            /// </summary>
            /// <param name="hWndNewOwner"></param>
            [DllImport("user32.dll")]
            public static extern bool OpenClipboard(IntPtr hWndNewOwner);

            /// <summary>
            /// 关闭剪切板
            /// </summary>
            [DllImport("user32.dll")]
            public static extern bool CloseClipboard();

            /// <summary>
            /// 清空剪贴板
            /// </summary>
            /// <returns></returns>
            [DllImport("user32.dll")]
            public static extern bool EmptyClipboard();

            /// <summary>
            /// 设置剪切板内容
            /// </summary>
            /// <param name="uFormat"></param>
            /// <param name="hMem"></param>
            [DllImport("user32.dll")]
            public static extern IntPtr SetClipboardData(uint uFormat, IntPtr hMem);
        }
    }
}