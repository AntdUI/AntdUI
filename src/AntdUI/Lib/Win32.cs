// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
// THE AntdUI PROJECT IS AN WINFORM LIBRARY LICENSED UNDER THE Apache-2.0 License.
// LICENSED UNDER THE Apache License, VERSION 2.0 (THE "License")
// YOU MAY NOT USE THIS FILE EXCEPT IN COMPLIANCE WITH THE License.
// YOU MAY OBTAIN A COPY OF THE LICENSE AT
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// UNLESS REQUIRED BY APPLICABLE LAW OR AGREED TO IN WRITING, SOFTWARE
// DISTRIBUTED UNDER THE LICENSE IS DISTRIBUTED ON AN "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED.
// SEE THE LICENSE FOR THE SPECIFIC LANGUAGE GOVERNING PERMISSIONS AND
// LIMITATIONS UNDER THE License.
// GITEE: https://gitee.com/AntdUI/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace AntdUI
{
    internal class Win32
    {
        #region LayeredWindowSDK

        #region 初始化

        static IntPtr screenDC, memDc;
        static Win32()
        {
            screenDC = GetDC(IntPtr.Zero);
            memDc = CreateCompatibleDC(screenDC);
        }

        ~Win32()
        {
            DeleteDC(memDc);
            ReleaseDC(IntPtr.Zero, screenDC);
        }

        #endregion

        public static RenderResult SetBits(Bitmap? bmp, Rectangle rect, IntPtr intPtr, byte alpha = 255)
        {
            if (bmp == null || bmp.PixelFormat == System.Drawing.Imaging.PixelFormat.DontCare) return RenderResult.Invalid;
            IntPtr hBitmap = bmp.GetHbitmap(Color.FromArgb(0)), oldBits = SelectObject(memDc, hBitmap);
            try
            {
                var srcLoc = new Win32Point(0, 0);
                var blendFunc = new BLENDFUNCTION
                {
                    BlendOp = AC_SRC_OVER,
                    SourceConstantAlpha = alpha,
                    AlphaFormat = AC_SRC_ALPHA,
                    BlendFlags = 0
                };
                var topLoc = new Win32Point(rect.X, rect.Y);
                var topSize = new Win32Size(rect.Width, rect.Height);
                UpdateLayeredWindow(intPtr, screenDC, ref topLoc, ref topSize, memDc, ref srcLoc, 0, ref blendFunc, ULW_ALPHA);
            }
            catch { return RenderResult.Error; }
            finally
            {
                if (hBitmap != IntPtr.Zero)
                {
                    SelectObject(memDc, oldBits);
                    DeleteObject(hBitmap);
                }
            }
            return RenderResult.OK;
        }

        [StructLayout(LayoutKind.Sequential)]
        ref struct Win32Size
        {
            public int cx, cy;
            public Win32Size(int x, int y)
            {
                cx = x;
                cy = y;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        ref struct Win32Point
        {
            public int x, y;
            public Win32Point(int _x, int _y)
            {
                x = _x;
                y = _y;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct BLENDFUNCTION
        {
            public byte BlendOp;
            public byte BlendFlags;
            public byte SourceConstantAlpha;
            public byte AlphaFormat;
        }

        const byte AC_SRC_OVER = 0;
        const int ULW_ALPHA = 2;
        const byte AC_SRC_ALPHA = 1;

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        static extern IntPtr CreateCompatibleDC(IntPtr hDC);

        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("gdi32.dll", ExactSpelling = true)]
        static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObj);

        [DllImport("user32.dll", ExactSpelling = true)]
        static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        static extern int DeleteDC(IntPtr hDC);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        static extern int DeleteObject(IntPtr hObj);

        /// <summary>
        /// 分层窗口
        /// </summary>
        /// <param name="hwnd">一个分层窗口的句柄。分层窗口在用CreateWindowEx函数创建窗口时应指定WS_EX_LAYERED扩展样式。 Windows 8： WS_EX_LAYERED扩展样式支持顶级窗口和子窗口。之前的Windows版本中WS_EX_LAYERED扩展样式仅支持顶级窗口</param>
        /// <param name="hdcDst">屏幕的设备上下文(DC)句柄。如果指定为NULL，那么将会在调用函数时自己获得。它用来在窗口内容更新时与调色板颜色匹配。如果hdcDst为NULL，将会使用默认调色板。如果hdcSrc指定为NULL，那么hdcDst必须指定为NULL。</param>
        /// <param name="pptDst">指向分层窗口相对于屏幕的位置的POINT结构的指针。如果保持当前位置不变，pptDst可以指定为NULL。</param>
        /// <param name="psize">指向分层窗口的大小的SIZE结构的指针。如果窗口的大小保持不变，psize可以指定为NULL。如果hdcSrc指定为NULL，psize必须指定为NULL。</param>
        /// <param name="hdcSrc">分层窗口绘图表面的设备上下文句柄。这个句柄可以通过调用函数CreateCompatibleDC获得。如果窗口的形状和可视范围保持不变，hdcSrc可以指定为NULL。</param>
        /// <param name="pptSrc">指向分层窗口绘图表面在设备上下文位置的POINT结构的指针。如果hdcSrc指定为NULL，pptSrc就应该指定为NULL。</param>
        /// <param name="crKey">指定合成分层窗口时使用的颜色值。要生成一个类型为COLORREF的值，使用RGB宏。</param>
        /// <param name="pblend">指向指定合成分层窗口时使用的透明度结构的指针。</param>
        /// <param name="dwFlags">可以是以下值之一。如果hdcSrc指定为NULL，dwFlags应该指定为0。</param>
        /// <returns></returns>
        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        static extern int UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref Win32Point pptDst, ref Win32Size psize, IntPtr hdcSrc, ref Win32Point pptSrc, int crKey, ref BLENDFUNCTION pblend, int dwFlags);

        #endregion

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
        extern static IntPtr GetClipboardData(uint uFormat);

        [DllImport("kernel32.dll", SetLastError = true)]
        extern static IntPtr GlobalLock(IntPtr hMem);

        [DllImport("Kernel32.dll", SetLastError = true)]
        extern static int GlobalSize(IntPtr hMem);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        extern static bool GlobalUnlock(IntPtr hMem);

        /// <summary>
        /// 打开剪切板
        /// </summary>
        /// <param name="hWndNewOwner"></param>
        [DllImport("user32.dll")]
        extern static bool OpenClipboard(IntPtr hWndNewOwner);

        /// <summary>
        /// 关闭剪切板
        /// </summary>
        [DllImport("user32.dll")]
        extern static bool CloseClipboard();

        /// <summary>
        /// 清空剪贴板
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll")]
        extern static bool EmptyClipboard();

        /// <summary>
        /// 设置剪切板内容
        /// </summary>
        /// <param name="uFormat"></param>
        /// <param name="hMem"></param>
        [DllImport("user32.dll")]
        extern static IntPtr SetClipboardData(uint uFormat, IntPtr hMem);

        #endregion
    }

    public enum RenderResult
    {
        /// <summary>
        /// 成功
        /// </summary>
        OK,
        /// <summary>
        /// 异常
        /// </summary>
        Error,
        /// <summary>
        /// 跳过
        /// </summary>
        Skip,
        /// <summary>
        /// 图片无效
        /// </summary>
        Invalid
    }
}