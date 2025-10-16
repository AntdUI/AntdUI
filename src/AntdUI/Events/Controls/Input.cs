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
using System.Windows.Forms;

namespace AntdUI
{
    public class InputVerifyCharEventArgs : EventArgs
    {
        public InputVerifyCharEventArgs(char c)
        {
            Char = c;
        }

        /// <summary>
        /// 输入字符
        /// </summary>
        public char Char { get; private set; }

        /// <summary>
        /// 替换文本
        /// </summary>
        public string? ReplaceText { get; set; }

        /// <summary>
        /// 验证结果
        /// </summary>
        public bool Result { get; set; } = true;
    }


    public delegate void InputVerifyCharEventHandler(object sender, InputVerifyCharEventArgs e);

    public class InputVerifyKeyboardEventArgs : EventArgs
    {
        public InputVerifyKeyboardEventArgs(Keys keyData)
        {
            KeyData = keyData;
        }

        public Keys KeyData { get; private set; }

        /// <summary>
        /// 验证结果
        /// </summary>
        public bool Result { get; set; } = true;
    }


    public delegate void InputVerifyKeyboardEventHandler(object sender, InputVerifyKeyboardEventArgs e);

    public class InputNumberEventArgs : VEventArgs<decimal>
    {
        public InputNumberEventArgs(decimal value) : base(value) { }
    }

    public delegate string InputNumberRtEventHandler(object sender, InputNumberEventArgs e);
}