// THIS FILE IS PART OF Vanara PROJECT
// THE Vanara PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHT (C) dahall. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/dahall/Vanara

using Microsoft.Win32.SafeHandles;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Vanara.PInvoke
{
    /// <summary>Signals that a structure or class holds a handle to a synchronization object.</summary>
    public interface IGraphicsObjectHandle : IUserHandle { }

    /// <summary>Signals that a structure or class holds a HANDLE.</summary>
    public interface IHandle
    {
        /// <summary>Returns the value of the handle field.</summary>
        /// <returns>An IntPtr representing the value of the handle field.</returns>
        IntPtr DangerousGetHandle();
    }

    /// <summary>Signals that a structure or class holds a handle to a synchronization object.</summary>
    public interface IKernelHandle : IHandle { }

    /// <summary>Signals that a structure or class holds a handle to a synchronization object.</summary>
    public interface IUserHandle : IHandle { }

    /// <summary>Provides a handle to a Windows cursor.</summary>
    [StructLayout(LayoutKind.Sequential), DebuggerDisplay("{handle}")]
    public struct HCURSOR : IGraphicsObjectHandle
    {
        private readonly IntPtr handle;

        /// <summary>Initializes a new instance of the <see cref="HCURSOR"/> struct.</summary>
        /// <param name="preexistingHandle">An <see cref="IntPtr"/> object that represents the pre-existing handle to use.</param>
        public HCURSOR(IntPtr preexistingHandle) => handle = preexistingHandle;

        /// <summary>Returns an invalid handle by instantiating a <see cref="HCURSOR"/> object with <see cref="IntPtr.Zero"/>.</summary>
        public static HCURSOR NULL => new HCURSOR(IntPtr.Zero);

        /// <summary>Gets a value indicating whether this instance is a null handle.</summary>
        public bool IsNull => handle == IntPtr.Zero;

        /// <summary>Performs an explicit conversion from <see cref="HCURSOR"/> to <see cref="IntPtr"/>.</summary>
        /// <param name="h">The handle.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator IntPtr(HCURSOR h) => h.handle;

        /// <summary>Performs an implicit conversion from <see cref="IntPtr"/> to <see cref="HCURSOR"/>.</summary>
        /// <param name="h">The pointer to a handle.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator HCURSOR(IntPtr h) => new HCURSOR(h);

        /// <summary>Implements the operator !=.</summary>
        /// <param name="h1">The first handle.</param>
        /// <param name="h2">The second handle.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(HCURSOR h1, HCURSOR h2) => !(h1 == h2);

        /// <summary>Implements the operator ==.</summary>
        /// <param name="h1">The first handle.</param>
        /// <param name="h2">The second handle.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(HCURSOR h1, HCURSOR h2) => h1.Equals(h2);

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is HCURSOR h && handle == h.handle;

        /// <inheritdoc/>
        public override int GetHashCode() => handle.GetHashCode();

        /// <inheritdoc/>
        public IntPtr DangerousGetHandle() => handle;
    }

    /// <summary>Provides a handle to a module or library instance.</summary>
    [StructLayout(LayoutKind.Sequential), DebuggerDisplay("{handle}")]
    public struct HINSTANCE : IKernelHandle
    {
        private readonly IntPtr handle;

        /// <summary>Initializes a new instance of the <see cref="HINSTANCE"/> struct.</summary>
        /// <param name="preexistingHandle">An <see cref="IntPtr"/> object that represents the pre-existing handle to use.</param>
        public HINSTANCE(IntPtr preexistingHandle) => handle = preexistingHandle;

        /// <summary>Returns an invalid handle by instantiating a <see cref="HINSTANCE"/> object with <see cref="IntPtr.Zero"/>.</summary>
        public static HINSTANCE NULL => new HINSTANCE(IntPtr.Zero);

        /// <summary>Gets a value indicating whether this instance is a null handle.</summary>
        public bool IsNull => handle == IntPtr.Zero;

        /// <summary>Performs an explicit conversion from <see cref="HINSTANCE"/> to <see cref="IntPtr"/>.</summary>
        /// <param name="h">The handle.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator IntPtr(HINSTANCE h) => h.handle;

        /// <summary>Performs an implicit conversion from <see cref="IntPtr"/> to <see cref="HINSTANCE"/>.</summary>
        /// <param name="h">The pointer to a handle.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator HINSTANCE(IntPtr h) => new HINSTANCE(h);

        /// <summary>Implements the operator !=.</summary>
        /// <param name="h1">The first handle.</param>
        /// <param name="h2">The second handle.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(HINSTANCE h1, HINSTANCE h2) => !(h1 == h2);

        /// <summary>Implements the operator ==.</summary>
        /// <param name="h1">The first handle.</param>
        /// <param name="h2">The second handle.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(HINSTANCE h1, HINSTANCE h2) => h1.Equals(h2);

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is HINSTANCE h && handle == h.handle;

        /// <inheritdoc/>
        public override int GetHashCode() => handle.GetHashCode();

        /// <inheritdoc/>
        public IntPtr DangerousGetHandle() => handle;
    }

    /// <summary>Provides a handle to a window or dialog.</summary>
    [StructLayout(LayoutKind.Sequential), DebuggerDisplay("{handle}")]
    public struct HWND : IUserHandle
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
        public override bool Equals(object obj) => obj is HWND h && handle == h.handle;

        /// <inheritdoc/>
        public override int GetHashCode() => handle.GetHashCode();

        /// <inheritdoc/>
        public IntPtr DangerousGetHandle() => handle;
    }

    /// <summary>Base class for all native handles.</summary>
    /// <seealso cref="Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid"/>
    /// <seealso cref="System.IEquatable{T}"/>
    /// <seealso cref="Vanara.PInvoke.IHandle"/>
    [DebuggerDisplay("{handle}")]
    public abstract class SafeHANDLE : SafeHandleZeroOrMinusOneIsInvalid, IEquatable<SafeHANDLE>, IHandle
    {
        /// <summary>Initializes a new instance of the <see cref="SafeHANDLE"/> class.</summary>
        public SafeHANDLE() : base(true)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="SafeHANDLE"/> class and assigns an existing handle.</summary>
        /// <param name="preexistingHandle">An <see cref="IntPtr"/> object that represents the pre-existing handle to use.</param>
        /// <param name="ownsHandle">
        /// <see langword="true"/> to reliably release the handle during the finalization phase; otherwise, <see langword="false"/> (not recommended).
        /// </param>
        protected SafeHANDLE(IntPtr preexistingHandle, bool ownsHandle = true) : base(ownsHandle) => SetHandle(preexistingHandle);

        /// <summary>Gets a value indicating whether this instance is null.</summary>
        /// <value><c>true</c> if this instance is null; otherwise, <c>false</c>.</value>
        public bool IsNull => handle == IntPtr.Zero;

        /// <summary>Implements the operator !=.</summary>
        /// <param name="h1">The first handle.</param>
        /// <param name="h2">The second handle.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(SafeHANDLE h1, IHandle h2) => !(h1 == h2);

        /// <summary>Implements the operator ==.</summary>
        /// <param name="h1">The first handle.</param>
        /// <param name="h2">The second handle.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(SafeHANDLE h1, IHandle h2) => h1?.Equals(h2) ?? h2 is null;

        /// <summary>Implements the operator !=.</summary>
        /// <param name="h1">The first handle.</param>
        /// <param name="h2">The second handle.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(SafeHANDLE h1, IntPtr h2) => !(h1 == h2);

        /// <summary>Implements the operator ==.</summary>
        /// <param name="h1">The first handle.</param>
        /// <param name="h2">The second handle.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(SafeHANDLE h1, IntPtr h2) => h1?.Equals(h2) ?? false;

        /// <summary>Determines whether the specified <see cref="SafeHANDLE"/>, is equal to this instance.</summary>
        /// <param name="other">The <see cref="SafeHANDLE"/> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="SafeHANDLE"/> is equal to this instance; otherwise, <c>false</c>.</returns>
        public bool Equals(SafeHANDLE other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return handle == other.handle && IsClosed == other.IsClosed;
        }

        /// <summary>Determines whether the specified <see cref="System.Object"/>, is equal to this instance.</summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            switch (obj)
            {
                case IHandle ih:
                    return handle.Equals(ih.DangerousGetHandle());
                case SafeHandle sh:
                    return handle.Equals(sh.DangerousGetHandle());
                case IntPtr p:
                    return handle.Equals(p);
                default:
                    return base.Equals(obj);
            }
        }

        /// <summary>Returns a hash code for this instance.</summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode() => base.GetHashCode();

        /// <summary>Releases the ownership of the underlying handle and returns the current handle.</summary>
        /// <returns>The value of the current handle.</returns>
        public IntPtr ReleaseOwnership()
        {
            var ret = handle;
            SetHandleAsInvalid();
            return ret;
        }

        /// <summary>
        /// Internal method that actually releases the handle. This is called by <see cref="ReleaseHandle"/> for valid handles and afterwards
        /// zeros the handle.
        /// </summary>
        /// <returns><c>true</c> to indicate successful release of the handle; <c>false</c> otherwise.</returns>
        protected abstract bool InternalReleaseHandle();

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
        protected override bool ReleaseHandle()
        {
            if (IsInvalid) return true;
            if (!InternalReleaseHandle()) return false;
            handle = IntPtr.Zero;
            return true;
        }
    }
}