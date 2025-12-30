// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;

namespace AntdUI
{
    public class ImagePreviewButtonEventArgs : EventArgs
    {
        public ImagePreviewButtonEventArgs(ImagePreviewItem item, string id, object? tag)
        {
            Item = item;
            Name = id;
            Tag = tag;
        }

        public ImagePreviewItem Item { get; private set; }
        public string Name { get; private set; }
        public object? Tag { get; private set; }
    }

    /// <summary>
    /// 点击事件
    /// </summary>
    public delegate void ImagePreviewButtonEventHandler(object sender, ImagePreviewButtonEventArgs e);
}