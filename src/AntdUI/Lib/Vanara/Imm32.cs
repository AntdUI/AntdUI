// THIS FILE IS PART OF Vanara PROJECT
// THE Vanara PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHT (C) dahall. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/dahall/Vanara

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace AntdUI
{
    partial class Win32
    {
        public static class Imm32
        {
            [DllImport("Imm32.dll")]
            public static extern IntPtr ImmGetContext(IntPtr hWnd);

            [DllImport("Imm32.dll")]
            public static extern bool ImmGetOpenStatus(IntPtr himc);

            [DllImport("Imm32.dll")]
            public static extern bool ImmReleaseContext(IntPtr hWnd, IntPtr hIMC);
            [DllImport("Imm32.dll", CharSet = CharSet.Unicode)]
            public static extern int ImmGetCompositionString(IntPtr hIMC, int dwIndex, byte[] lpBuf, int dwBufLen);
            [DllImport("Imm32.dll")]
            public static extern bool ImmSetCandidateWindow(IntPtr hImc, ref CANDIDATEFORM fuck);
            [DllImport("Imm32.dll")]
            public static extern bool ImmSetCompositionWindow(IntPtr hIMC, ref COMPOSITIONFORM lpCompForm);
            [DllImport("Imm32.dll")]
            public static extern bool ImmSetCompositionFont(IntPtr hIMC, ref LOGFONT logFont);

            public const int SRCCOPY = 0x00CC0020;

            public const int GCS_COMPSTR = 0x0008;
            public const int GCS_RESULTSTR = 0x0800;

            public const int WM_GETDLGCODE = 0x0087;
            public const int DLGC_WANTALLKEYS = 0x0004;
            public const int DLGC_WANTARROWS = 0x0001;
            public const int DLGC_WANTCHARS = 0x0080;
            public const int DLGC_WANTTAB = 0x0001;

            public const int WM_IME_REQUEST = 0x0288;
            public const int WM_IME_COMPOSITION = 0x010F;
            public const int WM_IME_ENDCOMPOSITION = 0x010E;
            public const int WM_IME_STARTCOMPOSITION = 0x010D;
            // bit field for IMC_SETCOMPOSITIONWINDOW, IMC_SETCANDIDATEWINDOW
            public const int CFS_DEFAULT = 0x0000;
            public const int CFS_RECT = 0x0001;
            public const int CFS_POINT = 0x0002;
            public const int CFS_FORCE_POSITION = 0x0020;
            public const int CFS_CANDIDATEPOS = 0x0040;
            public const int CFS_EXCLUDE = 0x0080;

            public const int WM_KEYFIRST = 0x100;
            public const int WM_KEYLAST = 0x108;

            public const int WM_IME_CHAR = 0x0286;

            public struct CANDIDATEFORM
            {
                public int dwIndex;
                public int dwStyle;
                public Point ptCurrentPos;
                public Rectangle rcArea;
            }

            public struct COMPOSITIONFORM
            {
                public int dwStyle;
                public Point ptCurrentPos;
                public Rectangle rcArea;
            }

            public struct LOGFONT
            {
                public int lfHeight;
                public int lfWidth;
                public int lfEscapement;
                public int lfOrientation;
                public int lfWeight;
                public byte lfItalic;
                public byte lfUnderline;
                public byte lfStrikeOut;
                public byte lfCharSet;
                public byte lfOutPrecision;
                public byte lfClipPrecision;
                public byte lfQuality;
                public byte lfPitchAndFamily;
                public string lfFaceName;
            }

            private static byte[] m_byString = new byte[1024];

            public static string? ImmGetCompositionString(IntPtr hIMC, int dwIndex)
            {
                if (hIMC == IntPtr.Zero) return null;
                int nLen = ImmGetCompositionString(hIMC, dwIndex, m_byString, m_byString.Length);
                if (nLen > 0) return Encoding.Unicode.GetString(m_byString, 0, nLen);
                return null;
            }
        }
    }
}