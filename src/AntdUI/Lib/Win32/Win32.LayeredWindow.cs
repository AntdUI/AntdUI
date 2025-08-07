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
// GITCODE: https://gitcode.com/AntdUI/AntdUI
// GITEE: https://gitee.com/AntdUI/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace AntdUI
{
    partial class Win32
    {
        #region 初始化

        public static IntPtr screenDC;
        static Win32()
        {
            screenDC = GetDC(IntPtr.Zero);
        }

        ~Win32()
        {
            ReleaseDC(IntPtr.Zero, screenDC);
        }

        #endregion

        public static RenderResult SetBits(IntPtr memDc, Bitmap? bmp, Rectangle rect, IntPtr intPtr, byte alpha, out IntPtr hBitmap, out IntPtr oldBits)
        {
            if (bmp == null || bmp.PixelFormat == System.Drawing.Imaging.PixelFormat.DontCare)
            {
                hBitmap = oldBits = IntPtr.Zero;
                return RenderResult.Invalid;
            }
            hBitmap = bmp.GetHbitmap(Color.FromArgb(0));
            oldBits = SelectObject(memDc, hBitmap);
            var r = SetBits(memDc, rect, intPtr, alpha);
            return r;
        }
        public static bool Dispose(IntPtr memDc, ref IntPtr hBitmap, ref IntPtr oldBits)
        {
            if (hBitmap == IntPtr.Zero) return false;
            SelectObject(memDc, oldBits);
            DeleteObject(hBitmap);
            hBitmap = IntPtr.Zero;
            return true;
        }
        public static RenderResult SetBits(IntPtr memDc, Rectangle rect, IntPtr intPtr, byte alpha)
        {
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
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("gdi32.dll", ExactSpelling = true)]
        static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObj);

        [DllImport("user32.dll", ExactSpelling = true)]
        static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern int DeleteDC(IntPtr hDC);

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