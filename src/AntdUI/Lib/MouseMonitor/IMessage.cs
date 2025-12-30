// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.Windows.Forms;

namespace AntdUI
{
    public interface IMessage
    {
        void IMOUSECLICK();

        void IMOUSEMOVE();

        /// <summary>
        /// KEYDOWN
        /// </summary>
        bool IKEYS(Keys key);
    }
}