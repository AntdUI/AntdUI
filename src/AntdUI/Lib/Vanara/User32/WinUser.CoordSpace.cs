// THIS FILE IS PART OF Vanara PROJECT
// THE Vanara PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHT (C) dahall. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/dahall/Vanara

using System.Drawing;
using System.Runtime.InteropServices;

namespace Vanara.PInvoke
{
    public static partial class User32
    {
        /// <summary>The <c>ClientToScreen</c> function converts the client-area coordinates of a specified point to screen coordinates.</summary>
        /// <param name="hWnd">A handle to the window whose client area is used for the conversion.</param>
        /// <param name="lpPoint">
        /// A pointer to a POINT structure that contains the client coordinates to be converted. The new screen coordinates are copied into
        /// this structure if the function succeeds.
        /// </param>
        /// <returns>
        /// <para>If the function succeeds, the return value is nonzero.</para>
        /// <para>If the function fails, the return value is zero.</para>
        /// </returns>
        /// <remarks>
        /// <para>
        /// The <c>ClientToScreen</c> function replaces the client-area coordinates in the POINT structure with the screen coordinates. The
        /// screen coordinates are relative to the upper-left corner of the screen. Note, a screen-coordinate point that is above the
        /// window's client area has a negative y-coordinate. Similarly, a screen coordinate to the left of a client area has a negative x-coordinate.
        /// </para>
        /// <para>All coordinates are device coordinates.</para>
        /// <para>Examples</para>
        /// <para>For an example, see "Drawing Lines with the Mouse" in Using Mouse Input.</para>
        /// </remarks>
        // https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-clienttoscreen BOOL ClientToScreen( HWND hWnd, LPPOINT
        // lpPoint );
        [DllImport("user32.dll", SetLastError = false, ExactSpelling = true)]
        [PInvokeData("winuser.h", MSDNShortId = "3b1e2699-7f5f-444d-9072-f2ca7c8fa511")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ClientToScreen(HWND hWnd, ref Point lpPoint);

        /// <summary>The ScreenToClient function converts the screen coordinates of a specified point on the screen to client-area coordinates.</summary>
        /// <param name="hWnd">A handle to the window whose client area will be used for the conversion.</param>
        /// <param name="lpPoint">A pointer to a POINT structure that specifies the screen coordinates to be converted.</param>
        /// <returns>
        /// If the function succeeds, the return value is true. If the function fails, the return value is false. To get extended error
        /// information, call GetLastError.
        /// </returns>
        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        [System.Security.SecurityCritical]
        public static extern bool ScreenToClient(HWND hWnd, [In, Out] ref Point lpPoint);
    }
}