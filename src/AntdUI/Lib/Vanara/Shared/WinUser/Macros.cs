// THIS FILE IS PART OF Vanara PROJECT
// THE Vanara PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHT (C) dahall. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/dahall/Vanara

using System;

namespace Vanara.PInvoke
{
    /// <summary>Platform invokable enumerated types, constants and functions from windows.h</summary>
    public static partial class Macros
    {
        /// <summary>Determines whether a value is an integer identifier for a resource.</summary>
        /// <param name="ptr">The pointer to be tested whether it contains an integer resource identifier.</param>
        /// <returns>If the value is a resource identifier, the return value is TRUE. Otherwise, the return value is FALSE.</returns>
        [PInvokeData("WinBase.h", MSDNShortId = "ms648028")]
        public static bool IS_INTRESOURCE(IntPtr ptr) => unchecked((ulong)ptr.ToInt64()) >> 16 == 0;

        /// <summary>Retrieves the low-order byte from the given 16-bit value.</summary>
        /// <param name="wValue">The value to be converted.</param>
        /// <returns>The return value is the low-order byte of the specified value.</returns>
        public static byte LOBYTE(ushort wValue) => (byte)(wValue & 0xff);

        /// <summary>Gets the lower 8-bytes from a <see cref="long"/> value.</summary>
        /// <param name="lValue">The <see cref="long"/> value.</param>
        /// <returns>The lower 8-bytes as a <see cref="uint"/>.</returns>
        public static uint LowPart(this long lValue) => (uint)(lValue & 0xffffffff);

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

        /// <summary>
        /// Converts an integer value to a resource type compatible with the resource-management functions. This macro is used in place of a
        /// string containing the name of the resource.
        /// </summary>
        /// <param name="id">The integer value to be converted.</param>
        /// <returns>The return value is string representation of the integer value.</returns>
        [PInvokeData("WinUser.h", MSDNShortId = "ms648029")]
        public static ResourceId MAKEINTRESOURCE(int id) => id;

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
}