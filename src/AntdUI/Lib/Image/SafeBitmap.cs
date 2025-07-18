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
using System.Drawing.Imaging;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;

namespace AntdUI
{
    public class SafeBitmap : IDisposable
    {
        private readonly MemoryMappedFile _mmf;
        private readonly MemoryMappedViewAccessor _accessor;
        private readonly int _stride;
        private readonly int _width;
        private readonly int _height;
        private readonly Bitmap _bitmap;
        private bool _isDisposed = false;
        private GCHandle _pinnedBitmap;

        public SafeBitmap(int width, int height)
        {
            _width = width;
            _height = height;
            _stride = (width * 32 + 31) / 32 * 4; // 计算每行字节数，按4字节对齐
            long capacity = (long)_stride * height;

            // 创建内存映射文件
            _mmf = MemoryMappedFile.CreateNew(null, capacity);
            _accessor = _mmf.CreateViewAccessor();

            // 分配托管内存用于Bitmap
            byte[] bitmapData = new byte[capacity];
            _pinnedBitmap = GCHandle.Alloc(bitmapData, GCHandleType.Pinned);

            // 创建Bitmap，使用托管内存
            _bitmap = new Bitmap(width, height, _stride, PixelFormat.Format32bppArgb, _pinnedBitmap.AddrOfPinnedObject());
        }

        public Graphics Graphics => Graphics.FromImage(_bitmap);

        // 安全的像素更新方法
        public void UpdatePixels(Action<byte[]> updateAction)
        {
            byte[] bitmapData = new byte[_stride * _height];
            _accessor.ReadArray(0, bitmapData, 0, bitmapData.Length);

            updateAction(bitmapData);

            _accessor.WriteArray(0, bitmapData, 0, bitmapData.Length);
        }

        // 获取 Bitmap 用于显示或保存
        public Bitmap Bitmap => _bitmap;
        public int Width => _width;
        public int Height => _height;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _bitmap?.Dispose();

                    if (_pinnedBitmap.IsAllocated) _pinnedBitmap.Free();

                    _accessor?.Dispose();
                    _mmf?.Dispose();
                }
                _isDisposed = true;
            }
        }
    }
}