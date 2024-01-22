// THIS FILE IS PART OF Vanara PROJECT
// THE Vanara PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHT (C) dahall. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/dahall/Vanara

using System.Runtime.InteropServices;

namespace Vanara.PInvoke
{
    /// <summary>Encapsulates classes exposed by DWNAPI.DLL</summary>
    public static partial class DwmApi
    {
        /// <summary>Extends the window frame into the client area.</summary>
        /// <param name="hWnd">The handle to the window in which the frame will be extended into the client area.</param>
        /// <param name="pMarInset">
        /// A pointer to a MARGINS structure that describes the margins to use when extending the frame into the client area.
        /// </param>
        /// <returns>If this function succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [DllImport("dwmapi.dll", SetLastError = false, ExactSpelling = true)]
        [System.Security.SecurityCritical]
        [PInvokeData("dwmapi.h")]
        public static extern void DwmExtendFrameIntoClientArea(HWND hWnd, in MARGINS pMarInset);

        /// <summary>Returned by the GetThemeMargins function to define the margins of windows that have visual styles applied.</summary>
        [StructLayout(LayoutKind.Sequential)]
        [PInvokeData("dwmapi.h")]
        public struct MARGINS
        {
            /// <summary>Width of the left border that retains its size.</summary>
            public int cxLeftWidth;

            /// <summary>Width of the right border that retains its size.</summary>
            public int cxRightWidth;

            /// <summary>Height of the top border that retains its size.</summary>
            public int cyTopHeight;

            /// <summary>Height of the bottom border that retains its size.</summary>
            public int cyBottomHeight;

            /// <summary>Retrieves a <see cref="MARGINS"/> instance with all values set to 0.</summary>
            public static readonly MARGINS Empty = new MARGINS(0);

            /// <summary>Retrieves a <see cref="MARGINS"/> instance with all values set to -1.</summary>
            public static readonly MARGINS Infinite = new MARGINS(-1);

            /// <summary>Initializes a new instance of the <see cref="MARGINS"/> struct.</summary>
            /// <param name="left">The left border value.</param>
            /// <param name="right">The right border value.</param>
            /// <param name="top">The top border value.</param>
            /// <param name="bottom">The bottom border value.</param>
            public MARGINS(int left, int right, int top, int bottom)
            {
                cxLeftWidth = left;
                cxRightWidth = right;
                cyTopHeight = top;
                cyBottomHeight = bottom;
            }

            /// <summary>Initializes a new instance of the <see cref="MARGINS"/> struct.</summary>
            /// <param name="allMargins">Value to assign to all margins.</param>
            public MARGINS(int allMargins) => cxLeftWidth = cxRightWidth = cyTopHeight = cyBottomHeight = allMargins;

            /// <summary>Gets or sets the left border value.</summary>
            /// <value>The left border.</value>
            public int Left { get => cxLeftWidth; set => cxLeftWidth = value; }

            /// <summary>Gets or sets the right border value.</summary>
            /// <value>The right border.</value>
            public int Right { get => cxRightWidth; set => cxRightWidth = value; }

            /// <summary>Gets or sets the top border value.</summary>
            /// <value>The top border.</value>
            public int Top { get => cyTopHeight; set => cyTopHeight = value; }

            /// <summary>Gets or sets the bottom border value.</summary>
            /// <value>The bottom border.</value>
            public int Bottom { get => cyBottomHeight; set => cyBottomHeight = value; }

            /// <summary>Determines if two <see cref="MARGINS"/> values are not equal.</summary>
            /// <param name="m1">The first margin.</param>
            /// <param name="m2">The second margin.</param>
            /// <returns><c>true</c> if the values are unequal; <c>false</c> otherwise.</returns>
            public static bool operator !=(MARGINS m1, MARGINS m2) => !m1.Equals(m2);

            /// <summary>Determines if two <see cref="MARGINS"/> values are equal.</summary>
            /// <param name="m1">The first margin.</param>
            /// <param name="m2">The second margin.</param>
            /// <returns><c>true</c> if the values are equal; <c>false</c> otherwise.</returns>
            public static bool operator ==(MARGINS m1, MARGINS m2) => m1.Equals(m2);

            /// <summary>Determines whether the specified <see cref="object"/>, is equal to this instance.</summary>
            /// <param name="obj">The <see cref="object"/> to compare with this instance.</param>
            /// <returns><c>true</c> if the specified <see cref="object"/> is equal to this instance; otherwise, <c>false</c>.</returns>
            public override bool Equals(object obj) => obj is MARGINS m2
                ? cxLeftWidth == m2.cxLeftWidth && cxRightWidth == m2.cxRightWidth && cyTopHeight == m2.cyTopHeight &&
                  cyBottomHeight == m2.cyBottomHeight
                : base.Equals(obj);

            /// <summary>Returns a hash code for this instance.</summary>
            /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
            public override int GetHashCode()
            {
                int RotateLeft(int value, int nBits)
                {
                    nBits = nBits % 0x20;
                    return (value << nBits) | (value >> (0x20 - nBits));
                }
                return cxLeftWidth ^ RotateLeft(cyTopHeight, 8) ^ RotateLeft(cxRightWidth, 0x10) ^ RotateLeft(cyBottomHeight, 0x18);
            }

            /// <summary>Returns a <see cref="string"/> that represents this instance.</summary>
            /// <returns>A <see cref="string"/> that represents this instance.</returns>
            public override string ToString() => $"{{Left={cxLeftWidth},Right={cxRightWidth},Top={cyTopHeight},Bottom={cyBottomHeight}}}";
        }
    }
}