// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace AntdUI
{
    /// <summary>
    /// 交错 BGRA 像素缓冲（LockBits 原生布局）
    /// 源图整段仅提取一次，动画期间所有帧复用；采样时 4 通道位于同一缓存行，避免 4 条独立数组的缓存流
    /// </summary>
    internal class BgraImage : IDisposable
    {
        public byte[] Pixels = new byte[0];
        public int Width = 0;
        public int Height = 0;

        public static BgraImage FromBitmap(Bitmap bmp)
        {
            int w = bmp.Width, h = bmp.Height;
            var data = bmp.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            try
            {
                int stride = data.Stride;
                var pixels = new byte[w * h * 4];
                if (stride == w * 4) Marshal.Copy(data.Scan0, pixels, 0, stride * h);
                else
                {
                    // 逐行去除 stride 填充
                    for (int y = 0; y < h; y++)
                    {
                        Marshal.Copy(new IntPtr(data.Scan0.ToInt64() + (long)y * stride), pixels, y * w * 4, w * 4);
                    }
                }
                return new BgraImage { Pixels = pixels, Width = w, Height = h };
            }
            finally
            {
                bmp.UnlockBits(data);
            }
        }

        public void Dispose()
        {
            Pixels = new byte[0];
            Width = 0;
            Height = 0;
        }
    }
}