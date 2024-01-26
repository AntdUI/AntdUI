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