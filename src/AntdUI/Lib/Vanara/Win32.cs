// THIS FILE IS PART OF Vanara PROJECT
// THE Vanara PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHT (C) dahall. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/dahall/Vanara

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AntdUI
{
    public partial class Win32
    {
        public static bool IsValidHandle(IntPtr handle)
        {
            // if (?:) will be eliminated by jit
            return (IntPtr.Size == 4)
                ? (handle.ToInt32() > 0)
                : (handle.ToInt64() > 0);
        }

        #region RECT

        /// <summary>Defines the coordinates of the upper-left and lower-right corners of a rectangle.</summary>
        /// <remarks>
        /// By convention, the right and bottom edges of the rectangle are normally considered exclusive. In other words, the pixel whose
        /// coordinates are ( right, bottom ) lies immediately outside of the rectangle. For example, when RECT is passed to the FillRect
        /// function, the rectangle is filled up to, but not including, the right column and bottom row of pixels. This structure is identical to
        /// the RECT structure.
        /// </remarks>
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT : IEquatable<PRECT>, IEquatable<RECT>, IEquatable<Rectangle>
        {
            /// <summary>The x-coordinate of the upper-left corner of the rectangle.</summary>
            public int left;

            /// <summary>The y-coordinate of the upper-left corner of the rectangle.</summary>
            public int top;

            /// <summary>he x-coordinate of the lower-right corner of the rectangle.</summary>
            public int right;

            /// <summary>he y-coordinate of the lower-right corner of the rectangle.</summary>
            public int bottom;

            /// <summary>Initializes a new instance of the <see cref="RECT"/> struct.</summary>
            /// <param name="left">The left.</param>
            /// <param name="top">The top.</param>
            /// <param name="right">The right.</param>
            /// <param name="bottom">The bottom.</param>
            public RECT(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }

            /// <summary>Initializes a new instance of the <see cref="RECT"/> struct.</summary>
            /// <param name="r">The rectangle.</param>
            public RECT(Rectangle r) : this(r.Left, r.Top, r.Right, r.Bottom)
            {
            }

            /// <summary>Gets or sets the x-coordinate of the upper-left corner of this <see cref="RECT"/> structure.</summary>
            /// <value>The x-coordinate of the upper-left corner of this <see cref="RECT"/> structure. The default is 0.</value>
            public int X
            {
                get => left;
                set
                {
                    right -= (left - value);
                    left = value;
                }
            }

            /// <summary>Gets or sets the y-coordinate of the upper-left corner of this <see cref="RECT"/> structure.</summary>
            /// <value>The y-coordinate of the upper-left corner of this <see cref="RECT"/> structure. The default is 0.</value>
            public int Y
            {
                get => top;
                set
                {
                    bottom -= (top - value);
                    top = value;
                }
            }

            /// <summary>Gets or sets the height of this <see cref="RECT"/> structure.</summary>
            /// <value>The height of this <see cref="RECT"/> structure. The default is 0.</value>
            public int Height
            {
                get => bottom - top;
                set => bottom = value + top;
            }

            /// <summary>Gets or sets the width of this <see cref="RECT"/> structure.</summary>
            /// <value>The width of this <see cref="RECT"/> structure. The default is 0.</value>
            public int Width
            {
                get => right - left;
                set => right = value + left;
            }

            /// <summary>Gets or sets the coordinates of the upper-left corner of this <see cref="RECT"/> structure.</summary>
            /// <value>A Point that represents the upper-left corner of this <see cref="RECT"/> structure.</value>
            public Point Location
            {
                get => new Point(left, top);
                set
                {
                    X = value.X;
                    Y = value.Y;
                }
            }

            /// <summary>Gets or sets the size of this <see cref="RECT"/> structure.</summary>
            /// <value>A Size that represents the width and height of this <see cref="RECT"/> structure.</value>
            public Size Size
            {
                get => new Size(Width, Height);
                set
                {
                    Width = value.Width;
                    Height = value.Height;
                }
            }

            /// <summary>Tests whether all numeric properties of this <see cref="RECT"/> have values of zero.</summary>
            /// <value><c>true</c> if this instance is empty; otherwise, <c>false</c>.</value>
            public bool IsEmpty => left == 0 && top == 0 && right == 0 && bottom == 0;

            /// <summary>Performs an implicit conversion from <see cref="RECT"/> to <see cref="Rectangle"/>.</summary>
            /// <param name="r">The <see cref="RECT"/> structure.</param>
            /// <returns>The result of the conversion.</returns>
            public static implicit operator Rectangle(RECT r) => new Rectangle(r.left, r.top, r.Width, r.Height);

            /// <summary>Performs an implicit conversion from <see cref="Rectangle"/> to <see cref="RECT"/>.</summary>
            /// <param name="r">The Rectangle structure.</param>
            /// <returns>The result of the conversion.</returns>
            public static implicit operator RECT(Rectangle r) => new RECT(r);

            /// <summary>Tests whether two <see cref="RECT"/> structures have equal values.</summary>
            /// <param name="r1">The r1.</param>
            /// <param name="r2">The r2.</param>
            /// <returns>The result of the operator.</returns>
            public static bool operator ==(RECT r1, RECT r2) => r1.Equals(r2);

            /// <summary>Tests whether two <see cref="RECT"/> structures have different values.</summary>
            /// <param name="r1">The first <see cref="RECT"/> structure.</param>
            /// <param name="r2">The second <see cref="RECT"/> structure.</param>
            /// <returns>The result of the operator.</returns>
            public static bool operator !=(RECT r1, RECT r2) => !r1.Equals(r2);

            /// <summary>Determines whether the specified <see cref="RECT"/>, is equal to this instance.</summary>
            /// <param name="r">The <see cref="RECT"/> to compare with this instance.</param>
            /// <returns><c>true</c> if the specified <see cref="RECT"/> is equal to this instance; otherwise, <c>false</c>.</returns>
            public bool Equals(RECT r) => r.left == left && r.top == top && r.right == right && r.bottom == bottom;

            /// <summary>Determines whether the specified <see cref="PRECT"/>, is equal to this instance.</summary>
            /// <param name="r">The <see cref="PRECT"/> to compare with this instance.</param>
            /// <returns><c>true</c> if the specified <see cref="PRECT"/> is equal to this instance; otherwise, <c>false</c>.</returns>
            public bool Equals(PRECT? r) => Equals(r?.rect);

            /// <summary>Determines whether the specified <see cref="Rectangle"/>, is equal to this instance.</summary>
            /// <param name="r">The <see cref="Rectangle"/> to compare with this instance.</param>
            /// <returns><c>true</c> if the specified <see cref="Rectangle"/> is equal to this instance; otherwise, <c>false</c>.</returns>
            public bool Equals(Rectangle r) => r.Left == left && r.Top == top && r.Right == right && r.Bottom == bottom;

            /// <summary>Determines whether the specified <see cref="object"/>, is equal to this instance.</summary>
            /// <param name="obj">The <see cref="object"/> to compare with this instance.</param>
            /// <returns><c>true</c> if the specified <see cref="object"/> is equal to this instance; otherwise, <c>false</c>.</returns>
            public override bool Equals(object? obj)
            {
                switch (obj)
                {
                    case null:
                        return false;

                    case RECT r:
                        return Equals(r);

                    case PRECT r:
                        return Equals(r);

                    case Rectangle r:
                        return Equals(r);

                    default:
                        return false;
                }
            }

            /// <summary>Returns a hash code for this instance.</summary>
            /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
            public override int GetHashCode() => ((Rectangle)this).GetHashCode();

            /// <summary>Returns a <see cref="string"/> that represents this instance.</summary>
            /// <returns>A <see cref="string"/> that represents this instance.</returns>
            public override string ToString() => $"{{left={left},top={top},right={right},bottom={bottom}}}";

            /// <summary>Represents an empty instance where all values are set to 0.</summary>
            public static readonly RECT Empty = new RECT();
        }

        /// <summary>Defines the coordinates of the upper-left and lower-right corners of a rectangle.</summary>
        /// <remarks>
        /// By convention, the right and bottom edges of the rectangle are normally considered exclusive. In other words, the pixel whose
        /// coordinates are ( right, bottom ) lies immediately outside of the rectangle. For example, when RECT is passed to the FillRect
        /// function, the rectangle is filled up to, but not including, the right column and bottom row of pixels. This structure is identical to
        /// the RECT structure.
        /// </remarks>
        [StructLayout(LayoutKind.Sequential)]
        public class PRECT : IEquatable<PRECT>, IEquatable<RECT>, IEquatable<Rectangle>
        {
            internal RECT rect;

            /// <summary>Initializes a new instance of the <see cref="PRECT"/> class with all values set to 0.</summary>
            public PRECT()
            {
            }

            /// <summary>Initializes a new instance of the <see cref="PRECT"/> class.</summary>
            /// <param name="left">The left.</param>
            /// <param name="top">The top.</param>
            /// <param name="right">The right.</param>
            /// <param name="bottom">The bottom.</param>
            public PRECT(int left, int top, int right, int bottom) => rect = new RECT(left, top, right, bottom);

            /// <summary>Initializes a new instance of the <see cref="PRECT"/> class.</summary>
            /// <param name="r">The <see cref="Rectangle"/> structure.</param>
            public PRECT(Rectangle r) => rect = new RECT(r);

            /// <summary>Initializes a new instance of the <see cref="PRECT"/> class.</summary>
            /// <param name="r">The r.</param>
            [ExcludeFromCodeCoverage]
            private PRECT(RECT r) => rect = r;

            /// <summary>The x-coordinate of the upper-left corner of the rectangle.</summary>
            public int left
            {
                get => rect.left;
                set => rect.left = value;
            }

            /// <summary>The y-coordinate of the upper-left corner of the rectangle.</summary>
            public int top
            {
                get => rect.top;
                set => rect.top = value;
            }

            /// <summary>he x-coordinate of the lower-right corner of the rectangle.</summary>
            public int right
            {
                get => rect.right;
                set => rect.right = value;
            }

            /// <summary>he y-coordinate of the lower-right corner of the rectangle.</summary>
            public int bottom
            {
                get => rect.bottom;
                set => rect.bottom = value;
            }

            /// <summary>Gets or sets the x-coordinate of the upper-left corner of this <see cref="PRECT"/> structure.</summary>
            /// <value>The x-coordinate of the upper-left corner of this <see cref="PRECT"/> structure. The default is 0.</value>
            public int X
            {
                get => rect.X;
                set => rect.X = value;
            }

            /// <summary>Gets or sets the y-coordinate of the upper-left corner of this <see cref="PRECT"/> structure.</summary>
            /// <value>The y-coordinate of the upper-left corner of this <see cref="PRECT"/> structure. The default is 0.</value>
            public int Y
            {
                get => rect.Y;
                set => rect.Y = value;
            }

            /// <summary>Gets or sets the height of this <see cref="PRECT"/> structure.</summary>
            /// <value>The height of this <see cref="PRECT"/> structure. The default is 0.</value>
            public int Height
            {
                get => rect.Height;
                set => rect.Height = value;
            }

            /// <summary>Gets or sets the width of this <see cref="PRECT"/> structure.</summary>
            /// <value>The width of this <see cref="PRECT"/> structure. The default is 0.</value>
            public int Width
            {
                get => rect.Width;
                set => rect.Width = value;
            }

            /// <summary>Gets or sets the coordinates of the upper-left corner of this <see cref="PRECT"/> structure.</summary>
            /// <value>A Point that represents the upper-left corner of this <see cref="PRECT"/> structure.</value>
            public Point Location
            {
                get => rect.Location;
                set => rect.Location = value;
            }

            /// <summary>Gets or sets the size of this <see cref="PRECT"/> structure.</summary>
            /// <value>A Size that represents the width and height of this <see cref="PRECT"/> structure.</value>
            public Size Size
            {
                get => rect.Size;
                set => rect.Size = value;
            }

            /// <summary>Tests whether all numeric properties of this <see cref="RECT"/> have values of zero.</summary>
            /// <value><c>true</c> if this instance is empty; otherwise, <c>false</c>.</value>
            public bool IsEmpty => rect.IsEmpty;

            /// <summary>Performs an implicit conversion from <see cref="PRECT"/> to <see cref="Rectangle"/>.</summary>
            /// <param name="r">The r.</param>
            /// <returns>The result of the conversion.</returns>
            public static implicit operator Rectangle(PRECT r) => r.rect;

            /// <summary>Performs an implicit conversion from <see cref="System.Nullable{Rectangle}"/> to <see cref="PRECT"/>.</summary>
            /// <param name="r">The r.</param>
            /// <returns>The result of the conversion.</returns>
            public static implicit operator PRECT?(Rectangle? r) => r.HasValue ? new PRECT(r.Value) : null;

            /// <summary>Performs an implicit conversion from <see cref="Rectangle"/> to <see cref="PRECT"/>.</summary>
            /// <param name="r">The r.</param>
            /// <returns>The result of the conversion.</returns>
            public static implicit operator PRECT(Rectangle r) => new PRECT(r);

            /// <summary>Performs an implicit conversion from <see cref="RECT"/> to <see cref="PRECT"/>.</summary>
            /// <param name="r">The r.</param>
            /// <returns>The result of the conversion.</returns>
            public static implicit operator PRECT(RECT r) => new PRECT(r);

            /// <summary>Implements the operator ==.</summary>
            /// <param name="r1">The r1.</param>
            /// <param name="r2">The r2.</param>
            /// <returns>The result of the operator.</returns>
            public static bool operator ==(PRECT r1, PRECT r2)
            {
                if (ReferenceEquals(r1, r2))
                    return true;
                if ((object)r1 == null || (object)r2 == null)
                    return false;
                return r1.Equals(r2);
            }

            /// <summary>Implements the operator !=.</summary>
            /// <param name="r1">The r1.</param>
            /// <param name="r2">The r2.</param>
            /// <returns>The result of the operator.</returns>
            public static bool operator !=(PRECT r1, PRECT r2) => !(r1 == r2);

            /// <summary>Determines whether the specified <see cref="PRECT"/>, is equal to this instance.</summary>
            /// <param name="r">The <see cref="PRECT"/> to compare with this instance.</param>
            /// <returns><c>true</c> if the specified <see cref="PRECT"/> is equal to this instance; otherwise, <c>false</c>.</returns>
            public bool Equals(PRECT? r) => rect == r?.rect;

            /// <summary>Determines whether the specified <see cref="RECT"/>, is equal to this instance.</summary>
            /// <param name="r">The <see cref="RECT"/> to compare with this instance.</param>
            /// <returns><c>true</c> if the specified <see cref="RECT"/> is equal to this instance; otherwise, <c>false</c>.</returns>
            public bool Equals(RECT r) => rect.Equals(r);

            /// <summary>Determines whether the specified <see cref="Rectangle"/>, is equal to this instance.</summary>
            /// <param name="r">The <see cref="Rectangle"/> to compare with this instance.</param>
            /// <returns><c>true</c> if the specified <see cref="Rectangle"/> is equal to this instance; otherwise, <c>false</c>.</returns>
            public bool Equals(Rectangle r) => rect.Equals(r);

            /// <summary>Determines whether the specified <see cref="object"/>, is equal to this instance.</summary>
            /// <param name="obj">The <see cref="object"/> to compare with this instance.</param>
            /// <returns><c>true</c> if the specified <see cref="object"/> is equal to this instance; otherwise, <c>false</c>.</returns>
            public override bool Equals(object? obj) => rect.Equals(obj);

            /// <summary>Returns a hash code for this instance.</summary>
            /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
            public override int GetHashCode() => rect.GetHashCode();

            /// <summary>Returns a <see cref="string"/> that represents this instance.</summary>
            /// <returns>A <see cref="string"/> that represents this instance.</returns>
            public override string ToString() => rect.ToString();
        }

        #endregion

        #region Handles

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

        #endregion

        #region Macros

        public class Macros
        {
            /// <summary>Retrieves the low-order byte from the given 16-bit value.</summary>
            /// <param name="wValue">The value to be converted.</param>
            /// <returns>The return value is the low-order byte of the specified value.</returns>
            public static byte LOBYTE(ushort wValue) => (byte)(wValue & 0xff);

            /// <summary>Gets the lower 8-bytes from a <see cref="long"/> value.</summary>
            /// <param name="lValue">The <see cref="long"/> value.</param>
            /// <returns>The lower 8-bytes as a <see cref="uint"/>.</returns>
            public static uint LowPart(long lValue) => (uint)(lValue & 0xffffffff);

            /// <summary>Retrieves the low-order word from the specified 32-bit value.</summary>
            /// <param name="dwValue">The value to be converted.</param>
            /// <returns>The return value is the low-order word of the specified value.</returns>
            public static ushort LOWORD(uint dwValue) => (ushort)(dwValue & 0xffff);

            /// <summary>Retrieves the low-order word from the specified 32-bit value.</summary>
            /// <param name="dwValue">The value to be converted.</param>
            /// <returns>The return value is the low-order word of the specified value.</returns>
            public static ushort LOWORD(IntPtr dwValue) => unchecked((ushort)(long)dwValue);

            /// <summary>Retrieves the low-order word from the specified 32-bit value.</summary>
            /// <param name="dwValue">The value to be converted.</param>
            /// <returns>The return value is the low-order word of the specified value.</returns>
            public static ushort LOWORD(UIntPtr dwValue) => unchecked((ushort)(ulong)dwValue);

            /// <summary>Creates a LONG value by concatenating the specified values.</summary>
            /// <param name="wLow">The low-order word of the new value.</param>
            /// <param name="wHigh">The high-order word of the new value.</param>
            /// <returns>The return value is a LONG value.</returns>
            public static int MAKELONG(int wLow, int wHigh) => (wHigh << 16) | (wLow & 0xffff);

            /// <summary>Creates a LONG64 value by concatenating the specified values.</summary>
            /// <param name="dwLow">The low-order double word of the new value.</param>
            /// <param name="dwHigh">The high-order double word of the new value.</param>
            /// <returns>The return value is a LONG64 value.</returns>
            public static long MAKELONG64(long dwLow, long dwHigh) => (dwHigh << 32) | (dwLow & 0xffffffff);

            /// <summary>Creates a value for use as an lParam parameter in a message. The macro concatenates the specified values.</summary>
            /// <param name="wLow">The low-order word of the new value.</param>
            /// <param name="wHigh">The high-order word of the new value.</param>
            /// <returns>The return value is an LPARAM value.</returns>
            public static IntPtr MAKELPARAM(int wLow, int wHigh) => new IntPtr(MAKELONG(wLow, wHigh));
        }

        #endregion

        #region Theme

        public static bool IsCompositionEnabled
        {
            get
            {
                try
                {
                    int enabled = 0;
                    DwmApi.DwmIsCompositionEnabled(ref enabled);
                    return enabled == 1;
                }
                catch { }
                return false;
            }
        }

        public static bool WindowTheme(Form form, bool dark, bool one = false)
        {
            var r = UseImmersiveDarkMode(form.Handle, dark);
            if (r)
            {
                if (one && !dark) return r;
                var code = dark ? "DarkMode_Explorer" : "ClearMode_Explorer";
                foreach (Control it in form.Controls) WindowTheme(it, code);
            }
            return r;
        }
        public static void WindowTheme(Control control)
        {
            if (Config.IsDark) WindowTheme(control, "DarkMode_Explorer");
        }
        public static void WindowTheme(Control control, TAMode mode)
        {
            switch (mode)
            {
                case TAMode.Light:
                    WindowTheme(control, "ClearMode_Explorer");
                    break;
                case TAMode.Dark:
                    WindowTheme(control, "DarkMode_Explorer");
                    break;
                case TAMode.Auto:
                default:
                    var code = Config.IsDark ? "DarkMode_Explorer" : "ClearMode_Explorer";
                    WindowTheme(control, code);
                    break;
            }
        }
        public static void WindowTheme(Control control, bool dark)
        {
            var code = dark ? "DarkMode_Explorer" : "ClearMode_Explorer";
            WindowTheme(control, code);
        }
        static void WindowTheme(Control control, string code)
        {
            if (HasScrollbar(control, out bool set))
            {
                if (set) DwmApi.SetWindowTheme(control.Handle, code, null);
                foreach (Control it in control.Controls)
                {
                    if (HasScrollbar(it, out bool set2))
                    {
                        if (set2) DwmApi.SetWindowTheme(it.Handle, code, null);
                        foreach (Control item in it.Controls) WindowTheme(item, code);
                    }
                }
            }
        }
        static bool HasScrollbar(Control control, out bool set)
        {
            set = false;
            if (control is IControl) return false;
            if (control is ScrollableControl scrollableControl)
            {
                set = scrollableControl.AutoScroll;
                return true;
            }
            if (control is System.Windows.Forms.Panel panel)
            {
                set = panel.AutoScroll;
                return true;
            }
            if (control is TextBoxBase) return true;
            if (control is ListBox) return true;
            if (control is DataGridView) return true;
            if (control is TreeView) return true;
            if (control is SplitContainer) return true;
            if (control is CheckedListBox) return true;
            if (control is WebBrowser) return true;
            return false;
        }

        #region Win32

        private const int DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1 = 19;
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
        private const int DWMWA_BORDER_COLOR = 34;
        public static bool UseImmersiveDarkMode(IntPtr handle, bool enabled)
        {
            if (OS.Win10OrGreater(17763))
            {
                var attribute = DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1;
                if (OS.Win10OrGreater(18985)) attribute = DWMWA_USE_IMMERSIVE_DARK_MODE;
                int useImmersiveDarkMode = enabled ? 1 : 0;
                return DwmApi.DwmSetWindowAttribute(handle, attribute, ref useImmersiveDarkMode, sizeof(int)) == 0;
            }

            return false;
        }

        public static bool SetWindowBorderColor(IntPtr handle, Color? color)
        {
            if (color.HasValue) return SetWindowBorderColor(handle, color.Value.R | (uint)color.Value.G << 8 | (uint)color.Value.B << 16);
            else return SetWindowBorderColor(handle, 0xFFFFFFFF);
        }

        public static bool SetWindowBorderColor(IntPtr handle, uint color)
        {
            try
            {
                return DwmApi.DwmSetWindowAttribute(handle, DWMWA_BORDER_COLOR, ref color, sizeof(uint)) == 0;
            }
            catch { }
            return false;
        }

        #endregion

        #endregion
    }
}