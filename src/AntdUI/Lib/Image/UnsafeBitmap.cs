// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace AntdUI
{
    public unsafe class UnsafeBitmap : IDisposable
    {
        public ColorBgra* Pointer { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public int PixelCount => Width * Height;

        private Bitmap bitmap;
        private BitmapData bitmapData;

        bool IsDispose { get; set; }
        public UnsafeBitmap(Bitmap bmp, bool write, bool dispose = false) : this(bmp, write ? ImageLockMode.ReadWrite : ImageLockMode.ReadOnly)
        {
            IsDispose = dispose;
        }

        public UnsafeBitmap(Bitmap bmp, ImageLockMode imageLockMode)
        {
            bitmap = bmp;
            Width = bmp.Width;
            Height = bmp.Height;
            bitmapData = bitmap.LockBits(new Rectangle(0, 0, Width, Height), imageLockMode, PixelFormat.Format32bppArgb);
            Pointer = (ColorBgra*)bitmapData.Scan0.ToPointer();
        }

        public static bool operator ==(UnsafeBitmap? bmp1, UnsafeBitmap? bmp2)
        {
            if (bmp1 is null && bmp2 is null) return true;
            if (bmp1 is null || bmp2 is null) return false;
            return bmp1.Equals(bmp2);
        }

        public static bool operator !=(UnsafeBitmap? bmp1, UnsafeBitmap? bmp2)
        {
            if (bmp1 is null && bmp2 is null) return false;
            if (bmp1 is null || bmp2 is null) return true;
            return !bmp1.Equals(bmp2);
        }

        public override bool Equals(object? obj) => obj is UnsafeBitmap unsafeBitmap && unsafeBitmap.bitmap.Equals(bitmap);

        public override int GetHashCode() => PixelCount;

        public bool IsTransparent()
        {
            int pixelCount = PixelCount;

            ColorBgra* pointer = Pointer;

            for (int i = 0; i < pixelCount; i++)
            {
                if (pointer->Alpha < 255) return true;

                pointer++;
            }

            return false;
        }

        public ColorBgra GetPixel(int i) => Pointer[i];

        public ColorBgra GetPixel(int x, int y) => Pointer[x + (y * Width)];

        public Dictionary<Point, ColorBgra> GetPixelAll()
        {
            int pixelCount = PixelCount, width = Width;
            var dict = new Dictionary<Point, ColorBgra>(pixelCount);
            for (int i = 0; i < pixelCount; i++)
            {
                int y = Math.DivRem(i, width, out int x);
                dict.Add(new Point(x, y), Pointer[i]);
            }
            return dict;
        }

        public void SetPixel(int i, ColorBgra color) => Pointer[i] = color;

        public void SetPixel(int i, uint color) => Pointer[i] = color;

        public void SetPixel(int x, int y, ColorBgra color) => Pointer[x + (y * Width)] = color;

        public void SetPixel(int x, int y, uint color) => Pointer[x + (y * Width)] = color;

        public void ClearPixel(int i) => Pointer[i] = 0;

        public void ClearPixel(int x, int y) => Pointer[x + (y * Width)] = 0;

        public void Dispose()
        {
            bitmap.UnlockBits(bitmapData);
            bitmapData = null!;
            Pointer = null;
            if (IsDispose) bitmap.Dispose();
        }
    }
}