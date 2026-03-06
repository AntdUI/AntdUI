// THIS FILE IS PART OF Vanara PROJECT
// THE Vanara PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHT (C) dahall. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/dahall/Vanara

using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace AntdUI
{
    partial class Win32
    {
        partial class User32
        {
            // 导入获取屏幕DPI的API（Win8.1+）
            [DllImport("shcore.dll")]
            public static extern int GetDpiForMonitor(IntPtr hmonitor, MonitorDpiType dpiType, out uint dpiX, out uint dpiY);

            // 导入获取屏幕句柄的API（备用，确保hMonitor准确）
            [DllImport("user32.dll")]
            public static extern IntPtr MonitorFromRect(ref Rectangle lprc, uint dwFlags);

            public const uint MONITOR_DEFAULTTONEAREST = 0x00000002; // 取最近的屏幕

            /// <summary>
            /// DPI类型枚举（优先用Effective_DPI）
            /// </summary>
            public enum MonitorDpiType
            {
                MDT_Effective_DPI = 0, // 系统推荐的有效DPI（UI适配首选）
                MDT_Angular_DPI = 1,   // 物理角DPI（理论值）
                MDT_Raw_DPI = 2        // 原始物理DPI（屏幕硬件DPI）
            }
        }
    }
}