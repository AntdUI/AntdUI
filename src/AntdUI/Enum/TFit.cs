// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

namespace AntdUI
{
    public enum TFit
    {
        /// <summary>
        /// 调整替换后的内容大小，以填充元素的内容框。如有必要，将拉伸或挤压物体以适应该对象
        /// </summary>
        Fill,
        /// <summary>
        /// 缩放替换后的内容以保持其纵横比，同时将其放入元素的内容框
        /// </summary>
        Contain,
        /// <summary>
        /// 调整替换内容的大小，以在填充元素的整个内容框时保持其长宽比。该对象将被裁剪以适应
        /// </summary>
        Cover,
        /// <summary>
        /// 不对替换的内容调整大小
        /// </summary>
        None
    }
}