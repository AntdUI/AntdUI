// THIS FILE IS PART OF Vanara PROJECT
// THE Vanara PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHT (C) dahall. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/dahall/Vanara

using System;
using System.Runtime.InteropServices;

namespace Vanara.PInvoke
{
    public static partial class User32
    {
        /// <summary>Window sizing and positioning flags.</summary>
        [PInvokeData("winuser.h", MSDNShortId = "setwindowpos")]
        [Flags]
        public enum SetWindowPosFlags : uint
        {
            /// <summary>
            /// If the calling thread and the thread that owns the window are attached to different input queues, the system posts the
            /// request to the thread that owns the window. This prevents the calling thread from blocking its execution while other threads
            /// process the request.
            /// </summary>
            SWP_ASYNCWINDOWPOS = 0x4000,

            /// <summary>Prevents generation of the WM_SYNCPAINT message.</summary>
            SWP_DEFERERASE = 0x2000,

            /// <summary>Draws a frame (defined in the window's class description) around the window.</summary>
            SWP_DRAWFRAME = 0x0020,

            /// <summary>
            /// Applies new frame styles set using the SetWindowLong function. Sends a WM_NCCALCSIZE message to the window, even if the
            /// window's size is not being changed. If this flag is not specified, WM_NCCALCSIZE is sent only when the window's size is being changed.
            /// </summary>
            SWP_FRAMECHANGED = 0x0020,

            /// <summary>Hides the window.</summary>
            SWP_HIDEWINDOW = 0x0080,

            /// <summary>
            /// Does not activate the window. If this flag is not set, the window is activated and moved to the top of either the topmost or
            /// non-topmost group (depending on the setting of the hWndInsertAfter parameter).
            /// </summary>
            SWP_NOACTIVATE = 0x0010,

            /// <summary>
            /// Discards the entire contents of the client area. If this flag is not specified, the valid contents of the client area are
            /// saved and copied back into the client area after the window is sized or repositioned.
            /// </summary>
            SWP_NOCOPYBITS = 0x0100,

            /// <summary>Retains the current position (ignores X and Y parameters).</summary>
            SWP_NOMOVE = 0x0002,

            /// <summary>Does not change the owner window's position in the Z order.</summary>
            SWP_NOOWNERZORDER = 0x0200,

            /// <summary>
            /// Does not redraw changes. If this flag is set, no repainting of any kind occurs. This applies to the client area, the
            /// nonclient area (including the title bar and scroll bars), and any part of the parent window uncovered as a result of the
            /// window being moved. When this flag is set, the application must explicitly invalidate or redraw any parts of the window and
            /// parent window that need redrawing.
            /// </summary>
            SWP_NOREDRAW = 0x0008,

            /// <summary>Same as the SWP_NOOWNERZORDER flag.</summary>
            SWP_NOREPOSITION = 0x0200,

            /// <summary>Prevents the window from receiving the WM_WINDOWPOSCHANGING message.</summary>
            SWP_NOSENDCHANGING = 0x0400,

            /// <summary>Retains the current size (ignores the cx and cy parameters).</summary>
            SWP_NOSIZE = 0x0001,

            /// <summary>Retains the current Z order (ignores the hWndInsertAfter parameter).</summary>
            SWP_NOZORDER = 0x0004,

            /// <summary>Displays the window.</summary>
            SWP_SHOWWINDOW = 0x0040,
        }

        /// <summary>
        /// <para>
        /// Calculates the required size of the window rectangle, based on the desired client-rectangle size. The window rectangle can then
        /// be passed to the CreateWindow function to create a window whose client area is the desired size.
        /// </para>
        /// <para>To specify an extended window style, use the AdjustWindowRectEx function.</para>
        /// </summary>
        /// <param name="lpRect">
        /// <para>Type: <c>LPRECT</c></para>
        /// <para>
        /// A pointer to a RECT structure that contains the coordinates of the top-left and bottom-right corners of the desired client area.
        /// When the function returns, the structure contains the coordinates of the top-left and bottom-right corners of the window to
        /// accommodate the desired client area.
        /// </para>
        /// </param>
        /// <param name="dwStyle">
        /// <para>Type: <c>DWORD</c></para>
        /// <para>
        /// The window style of the window whose required size is to be calculated. Note that you cannot specify the <c>WS_OVERLAPPED</c> style.
        /// </para>
        /// </param>
        /// <param name="bMenu">
        /// <para>Type: <c>BOOL</c></para>
        /// <para>Indicates whether the window has a menu.</para>
        /// </param>
        /// <returns>
        /// <para>Type: <c>Type: <c>BOOL</c></c></para>
        /// <para>If the function succeeds, the return value is nonzero.</para>
        /// <para>If the function fails, the return value is zero. To get extended error information, call GetLastError.</para>
        /// </returns>
        /// <remarks>
        /// <para>
        /// A client rectangle is the smallest rectangle that completely encloses a client area. A window rectangle is the smallest rectangle
        /// that completely encloses the window, which includes the client area and the nonclient area.
        /// </para>
        /// <para>The <c>AdjustWindowRect</c> function does not add extra space when a menu bar wraps to two or more rows.</para>
        /// <para>
        /// The <c>AdjustWindowRect</c> function does not take the <c>WS_VSCROLL</c> or <c>WS_HSCROLL</c> styles into account. To account for
        /// the scroll bars, call the GetSystemMetrics function with <c>SM_CXVSCROLL</c> or <c>SM_CYHSCROLL</c>.
        /// </para>
        /// </remarks>
        // https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-adjustwindowrect BOOL AdjustWindowRect( LPRECT lpRect,
        // DWORD dwStyle, BOOL bMenu );
        [DllImport("user32.dll", SetLastError = true, ExactSpelling = true)]
        [PInvokeData("winuser.h", MSDNShortId = "adjustwindowrect")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AdjustWindowRect(ref RECT lpRect, WindowStyles dwStyle, [MarshalAs(UnmanagedType.Bool)] bool bMenu);

        /// <summary>
        /// <para>
        /// Calculates the required size of the window rectangle, based on the desired size of the client rectangle. The window rectangle can
        /// then be passed to the CreateWindowEx function to create a window whose client area is the desired size.
        /// </para>
        /// </summary>
        /// <param name="lpRect">
        /// <para>Type: <c>LPRECT</c></para>
        /// <para>
        /// A pointer to a RECT structure that contains the coordinates of the top-left and bottom-right corners of the desired client area.
        /// When the function returns, the structure contains the coordinates of the top-left and bottom-right corners of the window to
        /// accommodate the desired client area.
        /// </para>
        /// </param>
        /// <param name="dwStyle">
        /// <para>Type: <c>DWORD</c></para>
        /// <para>
        /// The window style of the window whose required size is to be calculated. Note that you cannot specify the <c>WS_OVERLAPPED</c> style.
        /// </para>
        /// </param>
        /// <param name="bMenu">
        /// <para>Type: <c>BOOL</c></para>
        /// <para>Indicates whether the window has a menu.</para>
        /// </param>
        /// <param name="dwExStyle">
        /// <para>Type: <c>DWORD</c></para>
        /// <para>The extended window style of the window whose required size is to be calculated.</para>
        /// </param>
        /// <returns>
        /// <para>Type: <c>Type: <c>BOOL</c></c></para>
        /// <para>If the function succeeds, the return value is nonzero.</para>
        /// <para>If the function fails, the return value is zero. To get extended error information, call GetLastError.</para>
        /// </returns>
        /// <remarks>
        /// <para>
        /// A client rectangle is the smallest rectangle that completely encloses a client area. A window rectangle is the smallest rectangle
        /// that completely encloses the window, which includes the client area and the nonclient area.
        /// </para>
        /// <para>The <c>AdjustWindowRectEx</c> function does not add extra space when a menu bar wraps to two or more rows.</para>
        /// <para>
        /// The <c>AdjustWindowRectEx</c> function does not take the <c>WS_VSCROLL</c> or <c>WS_HSCROLL</c> styles into account. To account
        /// for the scroll bars, call the GetSystemMetrics function with <c>SM_CXVSCROLL</c> or <c>SM_CYHSCROLL</c>.
        /// </para>
        /// <para>
        /// This API is not DPI aware, and should not be used if the calling thread is per-monitor DPI aware. For the DPI-aware version of
        /// this API, see AdjustWindowsRectExForDPI. For more information on DPI awareness, see the Windows High DPI documentation.
        /// </para>
        /// </remarks>
        // https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-adjustwindowrectex BOOL AdjustWindowRectEx( LPRECT lpRect,
        // DWORD dwStyle, BOOL bMenu, DWORD dwExStyle );
        [DllImport("user32.dll", SetLastError = true, ExactSpelling = true)]
        [PInvokeData("winuser.h", MSDNShortId = "adjustwindowrectex")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AdjustWindowRectEx(ref RECT lpRect, WindowStyles dwStyle, [MarshalAs(UnmanagedType.Bool)] bool bMenu, WindowStylesEx dwExStyle);

        /// <summary>
        /// <para>
        /// Destroys the specified window. The function sends WM_DESTROY and WM_NCDESTROY messages to the window to deactivate it and remove
        /// the keyboard focus from it. The function also destroys the window's menu, flushes the thread message queue, destroys timers,
        /// removes clipboard ownership, and breaks the clipboard viewer chain (if the window is at the top of the viewer chain).
        /// </para>
        /// <para>
        /// If the specified window is a parent or owner window, <c>DestroyWindow</c> automatically destroys the associated child or owned
        /// windows when it destroys the parent or owner window. The function first destroys child or owned windows, and then it destroys the
        /// parent or owner window.
        /// </para>
        /// <para><c>DestroyWindow</c> also destroys modeless dialog boxes created by the CreateDialog function.</para>
        /// </summary>
        /// <param name="hWnd">
        /// <para>Type: <c>HWND</c></para>
        /// <para>A handle to the window to be destroyed.</para>
        /// </param>
        /// <returns>
        /// <para>Type: <c>Type: <c>BOOL</c></c></para>
        /// <para>If the function succeeds, the return value is nonzero.</para>
        /// <para>If the function fails, the return value is zero. To get extended error information, call GetLastError.</para>
        /// </returns>
        /// <remarks>
        /// <para>A thread cannot use <c>DestroyWindow</c> to destroy a window created by a different thread.</para>
        /// <para>
        /// If the window being destroyed is a child window that does not have the <c>WS_EX_NOPARENTNOTIFY</c> style, a WM_PARENTNOTIFY
        /// message is sent to the parent.
        /// </para>
        /// <para>Examples</para>
        /// <para>For an example, see Destroying a Window.</para>
        /// </remarks>
        // https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-destroywindow BOOL DestroyWindow( HWND hWnd );
        [DllImport("user32.dll", SetLastError = true, ExactSpelling = true)]
        [PInvokeData("winuser.h", MSDNShortId = "destroywindow")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DestroyWindow(HWND hWnd);

        /// <summary>
        /// Retrieves the dimensions of the bounding rectangle of the specified window. The dimensions are given in screen coordinates that
        /// are relative to the upper-left corner of the screen.
        /// </summary>
        /// <param name="hWnd">A handle to the window.</param>
        /// <param name="lpRect">
        /// A pointer to a RECT structure that receives the screen coordinates of the upper-left and lower-right corners of the window.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is true. If the function fails, the return value is false. To get extended error
        /// information, call GetLastError.
        /// </returns>
        [PInvokeData("WinUser.h", MSDNShortId = "ms633519")]
        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        [System.Security.SecurityCritical]
        public static extern bool GetWindowRect(HWND hWnd, out RECT lpRect);

        /// <summary>
        /// <para>Determines whether a window is maximized.</para>
        /// </summary>
        /// <param name="hWnd">
        /// <para>Type: <c>HWND</c></para>
        /// <para>A handle to the window to be tested.</para>
        /// </param>
        /// <returns>
        /// <para>Type: <c>Type: <c>BOOL</c></c></para>
        /// <para>If the window is zoomed, the return value is nonzero.</para>
        /// <para>If the window is not zoomed, the return value is zero.</para>
        /// </returns>
        // https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-iszoomed BOOL IsZoomed( HWND hWnd );
        [DllImport("user32.dll", SetLastError = false, ExactSpelling = true)]
        [PInvokeData("winuser.h", MSDNShortId = "iszoomed")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsZoomed(HWND hWnd);

        /// <summary>
        /// Releases the mouse capture from a window in the current thread and restores normal mouse input processing. A window that has
        /// captured the mouse receives all mouse input, regardless of the position of the cursor, except when a mouse button is clicked
        /// while the cursor hot spot is in the window of another thread.
        /// </summary>
        /// <returns>
        /// <para>Type: <c>BOOL</c></para>
        /// <para>If the function succeeds, the return value is nonzero.</para>
        /// <para>If the function fails, the return value is zero. To get extended error information, call GetLastError.</para>
        /// </returns>
        /// <remarks>
        /// <para>An application calls this function after calling the SetCapture function.</para>
        /// <para>Examples</para>
        /// <para>For an example, see Drawing Lines with the Mouse.</para>
        /// </remarks>
        // https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-releasecapture BOOL ReleaseCapture( );
        [DllImport("user32.dll", SetLastError = true, ExactSpelling = true)]
        [PInvokeData("winuser.h", MSDNShortId = "")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ReleaseCapture();


        [DllImport("user32.dll")]
        public static extern IntPtr FindWindowEx(HWND hwnd1, HWND hwnd2, string lpsz1, string lpsz2);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool MoveWindow(IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool bRepaint = true);


        [StructLayout(LayoutKind.Sequential)]
        public struct PAINTSTRUCT
        {
            public IntPtr hdc;
            public bool fErase;
            public RECT rcPaint;
            public bool fRestore;
            public bool fIncUpdate;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] rgbReserved;
        }
        [DllImport("user32.dll")]
        public static extern int BeginPaint(HWND hwnd, ref PAINTSTRUCT lpPaint);

        [DllImport("user32.dll")]
        public static extern int EndPaint(HWND hwnd, ref PAINTSTRUCT lpPaint);

        [DllImport("user32.dll", SetLastError = false, ExactSpelling = true)]
        [PInvokeData("winuser.h", MSDNShortId = "isiconic")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsIconic(HWND hWnd);

        [DllImport("user32.dll", SetLastError = false, CharSet = CharSet.Auto)]
        [PInvokeData("winuser.h")]
        public static extern IntPtr DefWindowProc(HWND hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", ExactSpelling = true)]
        [PInvokeData("winuser.h", MSDNShortId = "c6cb7f74-237e-4d3e-a852-894da36e990c")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool RedrawWindow(HWND hWnd, [In] PRECT? lprcUpdate, HWND hrgnUpdate, RedrawWindowFlags flags);

        [DllImport("user32.dll", SetLastError = false, ExactSpelling = true)]
        [PInvokeData("winuser.h", MSDNShortId = "51a50f1f-7b4d-4acd-83a0-1877f5181766")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UpdateWindow(HWND hWnd);

        [DllImport("user32.dll", SetLastError = true, ExactSpelling = true)]
        [PInvokeData("winuser.h", MSDNShortId = "setwindowpos")]
        [System.Security.SecurityCritical]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(HWND hWnd, HWND hWndInsertAfter, int X, int Y, int cx, int cy, SetWindowPosFlags uFlags);

        [DllImport("user32.dll", SetLastError = false, ExactSpelling = true)]
        [PInvokeData("winuser.h", MSDNShortId = "")]
        public static extern void DisableProcessWindowsGhosting();

        [PInvokeData("winuser.h", MSDNShortId = "c6cb7f74-237e-4d3e-a852-894da36e990c")]
        [Flags]
        public enum RedrawWindowFlags
        {
            /// <summary>Invalidates lprcUpdate or hrgnUpdate (only one may be non-NULL). If both are NULL, the entire window is invalidated.</summary>
            RDW_INVALIDATE = 0x0001,

            /// <summary>Causes a WM_PAINT message to be posted to the window regardless of whether any portion of the window is invalid.</summary>
            RDW_INTERNALPAINT = 0x0002,

            /// <summary>
            /// Causes the window to receive a WM_ERASEBKGND message when the window is repainted. The RDW_INVALIDATE flag must also be
            /// specified; otherwise, RDW_ERASE has no effect.
            /// </summary>
            RDW_ERASE = 0x0004,

            /// <summary>
            /// Validates lprcUpdate or hrgnUpdate (only one may be non-NULL). If both are NULL, the entire window is validated. This flag
            /// does not affect internal WM_PAINT messages.
            /// </summary>
            RDW_VALIDATE = 0x0008,

            /// <summary>
            /// Suppresses any pending internal WM_PAINT messages. This flag does not affect WM_PAINT messages resulting from a non-NULL
            /// update area.
            /// </summary>
            RDW_NOINTERNALPAINT = 0x0010,

            /// <summary>Suppresses any pending WM_ERASEBKGND messages.</summary>
            RDW_NOERASE = 0x0020,

            /// <summary>Excludes child windows, if any, from the repainting operation.</summary>
            RDW_NOCHILDREN = 0x0040,

            /// <summary>Includes child windows, if any, in the repainting operation.</summary>
            RDW_ALLCHILDREN = 0x0080,

            /// <summary>
            /// Causes the affected windows (as specified by the RDW_ALLCHILDREN and RDW_NOCHILDREN flags) to receive WM_NCPAINT,
            /// WM_ERASEBKGND, and WM_PAINT messages, if necessary, before the function returns.
            /// </summary>
            RDW_UPDATENOW = 0x0100,

            /// <summary>
            /// Causes the affected windows (as specified by the RDW_ALLCHILDREN and RDW_NOCHILDREN flags) to receive WM_NCPAINT and
            /// WM_ERASEBKGND messages, if necessary, before the function returns. WM_PAINT messages are received at the ordinary time.
            /// </summary>
            RDW_ERASENOW = 0x0200,

            /// <summary>
            /// Causes any part of the nonclient area of the window that intersects the update region to receive a WM_NCPAINT message. The
            /// RDW_INVALIDATE flag must also be specified; otherwise, RDW_FRAME has no effect. The WM_NCPAINT message is typically not sent
            /// during the execution of RedrawWindow unless either RDW_UPDATENOW or RDW_ERASENOW is specified.
            /// </summary>
            RDW_FRAME = 0x0400,

            /// <summary>
            /// Suppresses any pending WM_NCPAINT messages. This flag must be used with RDW_VALIDATE and is typically used with
            /// RDW_NOCHILDREN. RDW_NOFRAME should be used with care, as it could cause parts of a window to be painted improperly.
            /// </summary>
            RDW_NOFRAME = 0x0800,
        }
    }
}