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

namespace AntdUI
{
    /// <summary>
    /// 容器
    /// </summary>
    public abstract class ICell
    {
        #region 模板

        /// <summary>
        /// 获取大小
        /// </summary>
        /// <param name="g">GDI</param>
        /// <param name="font">字体</param>
        /// <param name="gap">边距</param>
        /// <param name="gap2">边距2</param>
        internal abstract Size GetSize(Canvas g, Font font, int gap, int gap2);

        /// <summary>
        /// 设置渲染位置坐标
        /// </summary>
        /// <param name="g"></param>
        /// <param name="font">字体</param>
        /// <param name="rect">区域</param>
        /// <param name="size">真实区域</param>
        /// <param name="gap">边距</param>
        /// <param name="gap2">边距2</param>
        internal abstract void SetRect(Canvas g, Font font, Rectangle rect, Size size, int gap, int gap2);

        internal abstract void PaintBack(Canvas g);
        internal abstract void Paint(Canvas g, Font font, SolidBrush fore);

        /// <summary>
        /// 模板父级
        /// </summary>
        internal Table.Template? PARENT { get; set; }

        #endregion

        internal Action<bool>? Changed { get; set; }
        public void OnPropertyChanged(bool layout = false)
        {
            Changed?.Invoke(layout);
        }
    }
}