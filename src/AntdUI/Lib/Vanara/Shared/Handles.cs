// THIS FILE IS PART OF Vanara PROJECT
// THE Vanara PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHT (C) dahall. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/dahall/Vanara

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Vanara.PInvoke
{
    /// <summary>Signals that a structure or class holds a handle to a synchronization object.</summary>
    public interface IHandle
    {
        IntPtr DangerousGetHandle();
    }

    /// <summary>Provides a handle to a window or dialog.</summary>
    [StructLayout(LayoutKind.Sequential), DebuggerDisplay("{handle}")]
    public struct HWND : IHandle
    {
        private readonly IntPtr handle;

        /// <summary>Initializes a new instance of the <see cref="HWND"/> struct.</summary>
        /// <param name="preexistingHandle">An <see cref="IntPtr"/> object that represents the pre-existing handle to use.</param>
        public HWND(IntPtr preexistingHandle) => handle = preexistingHandle;

        /// <summary>
        /// Places the window at the bottom of the Z order. If the hWnd parameter identifies a topmost window, the window loses its
        /// topmost status and is placed at the bottom of all other windows.
        /// </summary>
        public static HWND HWND_BOTTOM = new IntPtr(1);

        /// <summary>Use as parent in CreateWindow or CreateWindowEx call to indicate a message-only window.</summary>
        public static HWND HWND_MESSAGE = new IntPtr(-3);

        /// <summary>
        /// Places the window above all non-topmost windows (that is, behind all topmost windows). This flag has no effect if the window
        /// is already a non-topmost window.
        /// </summary>
        public static HWND HWND_NOTOPMOST = new IntPtr(-2);

        /// <summary>Places the window at the top of the Z order.</summary>
        public static HWND HWND_TOP = new IntPtr(0);

        /// <summary>Places the window above all non-topmost windows. The window maintains its topmost position even when it is deactivated.</summary>
        public static HWND HWND_TOPMOST = new IntPtr(-1);

        /// <summary>Returns an invalid handle by instantiating a <see cref="HWND"/> object with <see cref="IntPtr.Zero"/>.</summary>
        public static HWND NULL => new HWND(IntPtr.Zero);

        /// <summary>Gets a value indicating whether this instance is a null handle.</summary>
        public bool IsNull => handle == IntPtr.Zero;

        /// <summary>Performs an explicit conversion from <see cref="HWND"/> to <see cref="IntPtr"/>.</summary>
        /// <param name="h">The handle.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator IntPtr(HWND h) => h.handle;

        /// <summary>Performs an implicit conversion from <see cref="IntPtr"/> to <see cref="HWND"/>.</summary>
        /// <param name="h">The pointer to a handle.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator HWND(IntPtr h) => new HWND(h);

        /// <summary>Implements the operator !=.</summary>
        /// <param name="h1">The first handle.</param>
        /// <param name="h2">The second handle.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(HWND h1, HWND h2) => !(h1 == h2);

        /// <summary>Implements the operator ==.</summary>
        /// <param name="h1">The first handle.</param>
        /// <param name="h2">The second handle.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(HWND h1, HWND h2) => h1.Equals(h2);

        /// <inheritdoc/>
        public override bool Equals(object? obj) => obj is HWND h && handle == h.handle;

        /// <inheritdoc/>
        public override int GetHashCode() => handle.GetHashCode();

        /// <inheritdoc/>
        public IntPtr DangerousGetHandle() => handle;
    }
}