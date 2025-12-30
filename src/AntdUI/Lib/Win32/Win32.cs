// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace AntdUI
{
    public partial class Win32
    {
        #region 文本框

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

        #endregion

        #region 剪贴板

        internal static bool GetClipBoardText(out string? text)
        {
            IntPtr handle = default, pointer = default;
            try
            {
                if (!OpenClipboard(IntPtr.Zero)) { text = null; return false; }
                handle = GetClipboardData(13);
                if (handle == default) { text = null; return false; }
                pointer = GlobalLock(handle);
                if (pointer == default) { text = null; return false; }
                var size = GlobalSize(handle);
                var buff = new byte[size];
                Marshal.Copy(pointer, buff, 0, size);
                text = Encoding.Unicode.GetString(buff).TrimEnd('\0');
                return true;
            }
            catch
            {
                text = null;
                return false;
            }
            finally
            {
                if (pointer != default) GlobalUnlock(handle);
                CloseClipboard();
            }
        }
        internal static bool SetClipBoardText(string? text)
        {
            IntPtr hGlobal = default;
            try
            {
                if (!OpenClipboard(IntPtr.Zero)) return false;
                if (!EmptyClipboard()) return false;
                if (text == null) return true;

                var bytes = (text.Length + 1) * 2;
                hGlobal = Marshal.AllocHGlobal(bytes);
                if (hGlobal == default) return false;
                var target = GlobalLock(hGlobal);
                if (target == default) return false;
                try
                {
                    Marshal.Copy(text.ToCharArray(), 0, target, text.Length);
                }
                finally
                {
                    GlobalUnlock(target);
                }
                if (SetClipboardData(13, hGlobal) == default) return false;
                hGlobal = default;
                return true;
            }
            catch { return false; }
            finally
            {
                if (hGlobal != default) Marshal.FreeHGlobal(hGlobal);
                CloseClipboard();
            }
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr GetClipboardData(uint uFormat);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GlobalLock(IntPtr hMem);

        [DllImport("Kernel32.dll", SetLastError = true)]
        static extern int GlobalSize(IntPtr hMem);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GlobalUnlock(IntPtr hMem);

        /// <summary>
        /// 打开剪切板
        /// </summary>
        /// <param name="hWndNewOwner"></param>
        [DllImport("user32.dll")]
        static extern bool OpenClipboard(IntPtr hWndNewOwner);

        /// <summary>
        /// 关闭剪切板
        /// </summary>
        [DllImport("user32.dll")]
        static extern bool CloseClipboard();

        /// <summary>
        /// 清空剪贴板
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll")]
        static extern bool EmptyClipboard();

        /// <summary>
        /// 设置剪切板内容
        /// </summary>
        /// <param name="uFormat"></param>
        /// <param name="hMem"></param>
        [DllImport("user32.dll")]
        static extern IntPtr SetClipboardData(uint uFormat, IntPtr hMem);

        #endregion
    }
}