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
        public static class Shell32
        {
            [DllImport("shell32.dll", SetLastError = false, CallingConvention = CallingConvention.Winapi)]
            public static extern void DragAcceptFiles(IntPtr hWnd, bool fAccept);

            [DllImport("shell32.dll", SetLastError = false, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
            public static extern uint DragQueryFile(IntPtr hWnd, uint iFile, StringBuilder? lpszFile, int cch);

            [DllImport("shell32.dll", SetLastError = false, CallingConvention = CallingConvention.Winapi)]
            public static extern void DragFinish(IntPtr hDrop);

            public const uint GetIndexCount = 0xFFFFFFFFU;
        }
    }
}