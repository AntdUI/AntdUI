// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
// THE AntdUI PROJECT IS AN WINFORM LIBRARY LICENSED UNDER THE GPL-3.0 License.
// LICENSED UNDER THE GPL License, VERSION 3.0 (THE "License")
// YOU MAY NOT USE THIS FILE EXCEPT IN COMPLIANCE WITH THE License.
// UNLESS REQUIRED BY APPLICABLE LAW OR AGREED TO IN WRITING, SOFTWARE
// DISTRIBUTED UNDER THE LICENSE IS DISTRIBUTED ON AN "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED.
// SEE THE LICENSE FOR THE SPECIFIC LANGUAGE GOVERNING PERMISSIONS AND
// LIMITATIONS UNDER THE License.
// GITEE: https://gitee.com/antdui/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace AntdUI
{
    internal class Win32
    {
        internal static void SetBits(Bitmap bitmap, Win32Point topLoc, Win32Size topSize, IntPtr intPtr, byte a = 255)
        {
            IntPtr oldBits = IntPtr.Zero, screenDC = GetDC(IntPtr.Zero), hBitmap = IntPtr.Zero, memDc = CreateCompatibleDC(screenDC);
            try
            {
                Win32Point srcLoc = new Win32Point(0, 0);
                BLENDFUNCTION blendFunc = new BLENDFUNCTION();
                hBitmap = bitmap.GetHbitmap(Color.FromArgb(0));
                oldBits = SelectObject(memDc, hBitmap);
                blendFunc.BlendOp = AC_SRC_OVER;
                blendFunc.SourceConstantAlpha = a;
                blendFunc.AlphaFormat = AC_SRC_ALPHA;
                blendFunc.BlendFlags = 0;
                UpdateLayeredWindow(intPtr, screenDC, ref topLoc, ref topSize, memDc, ref srcLoc, 0, ref blendFunc, ULW_ALPHA);
            }
            finally
            {
                if (hBitmap != IntPtr.Zero)
                {
                    SelectObject(memDc, oldBits);
                    DeleteObject(hBitmap);
                }
                ReleaseDC(IntPtr.Zero, screenDC);
                DeleteDC(memDc);
            }
        }

        internal static void SetBits(Bitmap bitmap, Rectangle rect, IntPtr intPtr, byte a = 255)
        {
            IntPtr oldBits = IntPtr.Zero, screenDC = GetDC(IntPtr.Zero), hBitmap = IntPtr.Zero, memDc = CreateCompatibleDC(screenDC);
            try
            {
                var srcLoc = new Win32Point(0, 0);
                var blendFunc = new BLENDFUNCTION();
                hBitmap = bitmap.GetHbitmap(Color.FromArgb(0));
                oldBits = SelectObject(memDc, hBitmap);
                blendFunc.BlendOp = AC_SRC_OVER;
                blendFunc.SourceConstantAlpha = a;
                blendFunc.AlphaFormat = AC_SRC_ALPHA;
                blendFunc.BlendFlags = 0;
                var topLoc = new Win32Point(rect.X, rect.Y);
                var topSize = new Win32Size(rect.Width, rect.Height);
                UpdateLayeredWindow(intPtr, screenDC, ref topLoc, ref topSize, memDc, ref srcLoc, 0, ref blendFunc, ULW_ALPHA);
            }
            finally
            {
                if (hBitmap != IntPtr.Zero)
                {
                    SelectObject(memDc, oldBits);
                    DeleteObject(hBitmap);
                }
                ReleaseDC(IntPtr.Zero, screenDC);
                DeleteDC(memDc);
            }
        }

        public const int WM_CONTEXTMENU = 0x007B;
        public const int GWL_EXSTYLE = -20;
        public const int WS_EX_TRANSPARENT = 0x00000020;
        public const int WS_EX_LAYERED = 0x00080000;

        [StructLayout(LayoutKind.Sequential)]
        public struct Win32Size
        {
            public int cx;
            public int cy;

            public Win32Size(int x, int y)
            {
                cx = x;
                cy = y;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BLENDFUNCTION
        {
            public byte BlendOp;
            public byte BlendFlags;
            public byte SourceConstantAlpha;
            public byte AlphaFormat;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Win32Point
        {
            public int x;
            public int y;

            public Win32Point(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        public const byte AC_SRC_OVER = 0;
        public const int ULW_ALPHA = 2;
        public const byte AC_SRC_ALPHA = 1;

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetWindowDC(IntPtr handle);

        /// <summary>
        /// <para>该函数将指定的消息发送到一个或多个窗口。</para>
        /// <para>此函数为指定的窗口调用窗口程序直到窗口程序处理完消息再返回。</para>
        /// <para>而函数PostMessage不同，将一个消息寄送到一个线程的消息队列后立即返回。</para>
        /// return 返回值 : 指定消息处理的结果，依赖于所发送的消息。
        /// </summary>
        /// <param name="hWnd">要接收消息的那个窗口的句柄</param>
        /// <param name="Msg">消息的标识符</param>
        /// <param name="wParam">具体取决于消息</param>
        /// <param name="lParam">具体取决于消息</param>
        [DllImport("User32.dll", CharSet = CharSet.Auto, EntryPoint = "SendMessageA")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        [DllImport("gdi32.dll")]
        public static extern int CreateRoundRectRgn(int x1, int y1, int x2, int y2, int x3, int y3);

        [DllImport("user32.dll")]
        public static extern int SetWindowRgn(IntPtr hwnd, int hRgn, bool bRedraw);

        [DllImport("user32", EntryPoint = "GetWindowLong")]
        public static extern int GetWindowLong(IntPtr hwnd, int nIndex);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hwnd, int nIndex, int dwNewLong);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("gdi32.dll", ExactSpelling = true)]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObj);

        [DllImport("user32.dll", ExactSpelling = true)]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern int DeleteDC(IntPtr hDC);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern int DeleteObject(IntPtr hObj);

        /// <summary>
        /// 
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
        public static extern int UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref Win32Point pptDst, ref Win32Size psize, IntPtr hdcSrc, ref Win32Point pptSrc, int crKey, ref BLENDFUNCTION pblend, int dwFlags);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr ExtCreateRegion(IntPtr lpXform, uint nCount, IntPtr rgnData);

        public const int WM_MOUSEMOVE = 0x0200;
        public const int WM_LBUTTONDOWN = 0x0201;
        public const int WM_LBUTTONUP = 0x0202;
        public const int WM_RBUTTONDOWN = 0x0204;
        public const int WM_LBUTTONDBLCLK = 0x0203;

        public const int WM_MOUSELEAVE = 0x02A3;



        public const int WM_PAINT = 0x000F;
        public const int WM_ERASEBKGND = 0x0014;

        public const int WM_PRINT = 0x0317;

        //const int EN_HSCROLL       =   0x0601;
        //const int EN_VSCROLL       =   0x0602;

        public const int WM_HSCROLL = 0x0114;
        public const int WM_VSCROLL = 0x0115;


        public const int EM_GETSEL = 0x00B0;
        public const int EM_LINEINDEX = 0x00BB;
        public const int EM_LINEFROMCHAR = 0x00C9;

        public const int EM_POSFROMCHAR = 0x00D6;
    }
}