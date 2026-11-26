// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class Menu2 : UserControl
    {
        public Menu2(AntdUI.BaseForm form)
        {
            InitializeComponent();
        }

        void menu1_Mnemonic(object sender, char e)
        {
            AntdUI.Message.info(new AntdUI.Target(this), "助记键激活：Alt + " + char.ToUpperInvariant(e), Font);
        }
    }

    /// <summary>
    /// 带助记键回调的菜单：AntdUI.Menu 的 ProcessMnemonic 在命中 (&) 字符时返回 true
    /// </summary>
    class MnemonicMenu : AntdUI.Menu
    {
        public event EventHandler<char> Mnemonic;

        protected override bool ProcessMnemonic(char charCode)
        {
            bool handled = base.ProcessMnemonic(charCode);
            if (handled) Mnemonic?.Invoke(this, charCode);
            return handled;
        }
    }
}
