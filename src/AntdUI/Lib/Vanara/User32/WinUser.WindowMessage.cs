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
        partial class User32
        {
            [DllImport("user32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool ChangeWindowMessageFilterEx(IntPtr hWnd, uint message, ChangeFilterAction action, in ChangeFilterStruct pChangeFilterStruct);

            [StructLayout(LayoutKind.Sequential)]
            public struct ChangeFilterStruct
            {
                public uint CbSize;
                public ChangeFilterStatu ExtStatus;
            }

            public enum ChangeFilterAction : uint
            {
                MSGFLT_RESET,
                MSGFLT_ALLOW,
                MSGFLT_DISALLOW
            }

            public enum ChangeFilterStatu : uint
            {
                MSGFLTINFO_NONE,
                MSGFLTINFO_ALREADYALLOWED_FORWND,
                MSGFLTINFO_ALREADYDISALLOWED_FORWND,
                MSGFLTINFO_ALLOWED_HIGHER
            }

            public const uint WM_COPYGLOBALDATA = 0x0049;
            public const uint WM_COPYDATA = 0x004A;
            public const uint WM_DROPFILES = 0x0233;
        }
    }
}