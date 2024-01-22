﻿// THIS FILE IS PART OF Vanara PROJECT
// THE Vanara PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHT (C) dahall. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/dahall/Vanara

using System.Runtime.InteropServices;

namespace Vanara.PInvoke
{
    /// <summary>User32.dll function with GDI params.</summary>
    public static partial class User32
    {
        /// <summary>Values to use a return codes when handling the WM_HCHITTEST message.</summary>
        public enum HitTestValues : short
        {
            /// <summary>In the border of a window that does not have a sizing border.</summary>
            HTBORDER = 18,

            /// <summary>In the lower-horizontal border of a resizable window (the user can click the mouse to resize the window vertically).</summary>
            HTBOTTOM = 15,

            /// <summary>
            /// In the lower-left corner of a border of a resizable window (the user can click the mouse to resize the window diagonally).
            /// </summary>
            HTBOTTOMLEFT = 16,

            /// <summary>
            /// In the lower-right corner of a border of a resizable window (the user can click the mouse to resize the window diagonally).
            /// </summary>
            HTBOTTOMRIGHT = 17,

            /// <summary>In a title bar.</summary>
            HTCAPTION = 2,

            /// <summary>In a client area.</summary>
            HTCLIENT = 1,

            /// <summary>In a Close button.</summary>
            HTCLOSE = 20,

            /// <summary>
            /// On the screen background or on a dividing line between windows (same as HTNOWHERE, except that the DefWindowProc function
            /// produces a system beep to indicate an error).
            /// </summary>
            HTERROR = -2,

            /// <summary>In a size box (same as HTSIZE).</summary>
            HTGROWBOX = 4,

            /// <summary>In a Help button.</summary>
            HTHELP = 21,

            /// <summary>In a horizontal scroll bar.</summary>
            HTHSCROLL = 6,

            /// <summary>In the left border of a resizable window (the user can click the mouse to resize the window horizontally).</summary>
            HTLEFT = 10,

            /// <summary>In a menu.</summary>
            HTMENU = 5,

            /// <summary>In a Maximize button.</summary>
            HTMAXBUTTON = 9,

            /// <summary>In a Minimize button.</summary>
            HTMINBUTTON = 8,

            /// <summary>On the screen background or on a dividing line between windows.</summary>
            HTNOWHERE = 0,

            /* /// <summary>Not implemented.</summary>
			HTOBJECT = 19, */

            /// <summary>In a Minimize button.</summary>
            HTREDUCE = HTMINBUTTON,

            /// <summary>In the right border of a resizable window (the user can click the mouse to resize the window horizontally).</summary>
            HTRIGHT = 11,

            /// <summary>In a size box (same as HTGROWBOX).</summary>
            HTSIZE = HTGROWBOX,

            /// <summary>In a window menu or in a Close button in a child window.</summary>
            HTSYSMENU = 3,

            /// <summary>In the upper-horizontal border of a window.</summary>
            HTTOP = 12,

            /// <summary>In the upper-left corner of a window border.</summary>
            HTTOPLEFT = 13,

            /// <summary>In the upper-right corner of a window border.</summary>
            HTTOPRIGHT = 14,

            /// <summary>
            /// In a window currently covered by another window in the same thread (the message will be sent to underlying windows in the
            /// same thread until one of them returns a code that is not HTTRANSPARENT).
            /// </summary>
            HTTRANSPARENT = -1,

            /// <summary>In the vertical scroll bar.</summary>
            HTVSCROLL = 7,

            /// <summary>In a Maximize button.</summary>
            HTZOOM = HTMAXBUTTON,
        }

        /// <summary>Contains information about the size and position of a window.</summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct WINDOWPOS
        {
            /// <summary>A handle to the window.</summary>
            public HWND hwnd;

            /// <summary>
            /// The position of the window in Z order (front-to-back position). This member can be a handle to the window behind which this
            /// window is placed, or can be one of the special values listed with the SetWindowPos function.
            /// </summary>
            public HWND hwndInsertAfter;

            /// <summary>The position of the left edge of the window.</summary>
            public int x;

            /// <summary>The position of the top edge of the window.</summary>
            public int y;

            /// <summary>The window width, in pixels.</summary>
            public int cx;

            /// <summary>The window height, in pixels.</summary>
            public int cy;

            /// <summary>The window position. This member can be one or more of the following values.</summary>
            public SetWindowPosFlags flags;
        }
    }
}