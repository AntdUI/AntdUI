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
// GITEE: https://gitee.com/antdui/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace AntdUI
{
    public class ImageData : IDisposable
    {
        byte[,] _red, _green, _blue, _alpha;
        bool _disposed = false;

        public byte[,] A
        {
            get => _alpha;
            set => _alpha = value;
        }
        public byte[,] B
        {
            get => _blue;
            set => _blue = value;
        }
        public byte[,] G
        {
            get => _green;
            set => _green = value;
        }
        public byte[,] R
        {
            get => _red;
            set => _red = value;
        }

        public ImageData Clone()
        {
            ImageData cb = new ImageData();
            cb.A = (byte[,])_alpha.Clone();
            cb.B = (byte[,])_blue.Clone();
            cb.G = (byte[,])_green.Clone();
            cb.R = (byte[,])_red.Clone();
            return cb;
        }

        public void FromBitmap(Bitmap srcBmp)
        {
            int w = srcBmp.Width;
            int h = srcBmp.Height;
            _alpha = new byte[w, h];
            _blue = new byte[w, h];
            _green = new byte[w, h];
            _red = new byte[w, h];
            BitmapData bmpData = srcBmp.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            IntPtr ptr = bmpData.Scan0;
            int bytes = bmpData.Stride * srcBmp.Height;
            byte[] rgbValues = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);
            int offset = bmpData.Stride - w * 4;
            int index = 0;
            int y = 0, x;
            while (y < h)
            {
                x = 0;
                while (x < w)
                {
                    _blue[x, y] = rgbValues[index];
                    _green[x, y] = rgbValues[index + 1];
                    _red[x, y] = rgbValues[index + 2];
                    _alpha[x, y] = rgbValues[index + 3];
                    index += 4;
                    x++;
                }
                index += offset;
                y++;
            }
            srcBmp.UnlockBits(bmpData);
        }

        public Bitmap ToBitmap()
        {
            int width = 0, height = 0;
            if (_alpha != null)
            {
                width = Math.Max(width, _alpha.GetLength(0));
                height = Math.Max(height, _alpha.GetLength(1));
            }
            if (_blue != null)
            {
                width = Math.Max(width, _blue.GetLength(0));
                height = Math.Max(height, _blue.GetLength(1));
            }
            if (_green != null)
            {
                width = Math.Max(width, _green.GetLength(0));
                height = Math.Max(height, _green.GetLength(1));
            }
            if (_red != null)
            {
                width = Math.Max(width, _red.GetLength(0));
                height = Math.Max(height, _red.GetLength(1));
            }
            var bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            var bmpData = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            int bytes = bmpData.Stride * bmp.Height;
            byte[] rgbValues = new byte[bytes];
            int offset = bmpData.Stride - width * 4;
            int i = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    rgbValues[i] = checkArray(_blue, x, y) ? _blue[x, y] : (byte)0;
                    rgbValues[i + 1] = checkArray(_green, x, y) ? _green[x, y] : (byte)0;
                    rgbValues[i + 2] = checkArray(_red, x, y) ? _red[x, y] : (byte)0;
                    rgbValues[i + 3] = checkArray(_alpha, x, y) ? _alpha[x, y] : (byte)255;
                    i += 4;
                }
                i += offset;
            }
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, bmpData.Scan0, bytes);
            bmp.UnlockBits(bmpData);
            return bmp;
        }

        private static bool checkArray(byte[,] array, int x, int y)
        {
            if (array == null) return false;
            if (x < array.GetLength(0) && y < array.GetLength(1))
                return true;
            else return false;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _alpha = null;
                    _blue = null;
                    _green = null;
                    _red = null;
                }
                _disposed = true;
            }
        }
    }
}