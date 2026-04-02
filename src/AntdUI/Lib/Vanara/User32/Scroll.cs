// THIS FILE IS PART OF Vanara PROJECT
// THE Vanara PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHT (C) dahall. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/dahall/Vanara

using System;
using System.Runtime.InteropServices;

namespace AntdUI
{
    partial class Win32
    {
        public static partial class User32
        {
            public static void HideScrollBar(IntPtr hWnd) => ShowScrollBar(hWnd, 0x3, false);

            [DllImport("user32.dll", SetLastError = true)]
            static extern bool ShowScrollBar(IntPtr hWnd, int wBar, bool bShow);
        }
    }
}