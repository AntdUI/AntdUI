// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Drawing;
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

        #region 设置

        public InputVerifyCharEventArgs SetResult(bool value = false)
        {
            Result = value;
            return this;
        }
        public InputVerifyCharEventArgs SetReplaceText(string? value)
        {
            ReplaceText = value;
            return this;
        }

        #endregion
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

        #region 设置

        public InputVerifyKeyboardEventArgs SetResult(bool value = false)
        {
            Result = value;
            return this;
        }

        #endregion
    }


    public delegate void InputVerifyKeyboardEventHandler(object sender, InputVerifyKeyboardEventArgs e);

    public class InputNumberEventArgs : DecimalEventArgs
    {
        public InputNumberEventArgs(decimal value) : base(value) { }
    }

    public delegate string InputNumberRtEventHandler(object sender, InputNumberEventArgs e);

    #region 业务

    public class TextOpConfig
    {
        /// <summary>
        /// 字体
        /// </summary>
        public Font? Font { get; set; }

        /// <summary>
        /// 文本颜色
        /// </summary>
        public Color? Fore { get; set; }

        /// <summary>
        /// 背景色
        /// </summary>
        public Color? Back { get; set; }

        /// <summary>
        /// 刷新UI
        /// </summary>
        public bool Redraw { get; set; } = true;

        /// <summary>
        /// 光标行为
        /// </summary>
        public CursorBehavior CursorBehavior { get; set; } = CursorBehavior.KeepOriginal;

        #region 设置

        public TextOpConfig SetFont(Font? value)
        {
            Font = value;
            return this;
        }
        public TextOpConfig SetFore(Color? value)
        {
            Fore = value;
            return this;
        }
        public TextOpConfig SetBack(Color? value)
        {
            Back = value;
            return this;
        }
        public TextOpConfig SetRedraw(bool value = false)
        {
            Redraw = value;
            return this;
        }
        public TextOpConfig SetCursorBehavior(CursorBehavior value = CursorBehavior.EndOfNewText)
        {
            CursorBehavior = value;
            return this;
        }

        #endregion
    }

    /// <summary>
    /// 光标行为枚举
    /// </summary>
    public enum CursorBehavior
    {
        /// <summary>
        /// 保持光标原有位置
        /// </summary>
        KeepOriginal,
        /// <summary>
        /// 移动到新增/插入文本的开头
        /// </summary>
        StartOfNewText,
        /// <summary>
        /// 移动到新增/插入文本的末尾
        /// </summary>
        EndOfNewText,
        /// <summary>
        /// 移动到整个文本的末尾
        /// </summary>
        EndOfWholeText
    }

    #endregion
}