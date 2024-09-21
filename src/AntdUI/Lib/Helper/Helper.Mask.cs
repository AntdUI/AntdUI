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

using System.Windows.Forms;

namespace AntdUI
{
    partial class Helper
    {
        /// <summary>
        /// 叠加蒙版
        /// </summary>
        /// <param name="owner">父窗口</param>
        public static ILayeredFormOpacity FormMask(this Form owner)
        {
            var mask = new LayeredFormMask(owner);
            mask.Show(owner);
            return mask;
        }

        /// <summary>
        /// 叠加蒙版（可关闭）
        /// </summary>
        /// <param name="owner">父窗口</param>
        /// <param name="MaskClosable">点击蒙层是否允许关闭</param>
        /// <param name="form">操作对象</param>
        public static ILayeredFormOpacity FormMask(this Form owner, bool MaskClosable, ILayeredForm form)
        {
            var mask = new LayeredFormMask(owner);
            if (MaskClosable)
            {
                try
                {
                    mask.Click += (s1, e1) =>
                    {
                        form.IClose();
                    };
                }
                catch { }
            }
            mask.Show(owner);
            return mask;
        }

        /// <summary>
        /// 叠加蒙版（可关闭）
        /// </summary>
        /// <param name="owner">父窗口</param>
        /// <param name="MaskClosable">点击蒙层是否允许关闭</param>
        /// <param name="form">操作对象</param>
        public static ILayeredFormOpacity FormMask(this Form owner, bool MaskClosable, Form form)
        {
            var mask = new LayeredFormMask(owner);
            if (MaskClosable)
            {
                try
                {
                    mask.Click += (s1, e1) =>
                    {
                        form.Close();
                    };
                }
                catch { }
            }
            mask.Show(owner);
            return mask;
        }
    }
}