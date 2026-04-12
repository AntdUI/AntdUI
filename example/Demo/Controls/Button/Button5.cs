// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class Button5 : UserControl
    {
        AntdUI.BaseForm form;
        public Button5(AntdUI.BaseForm _form)
        {
            form = _form;
            InitializeComponent();
        }

        private void btn_Click(object sender, System.EventArgs e)
        {
            if (sender is AntdUI.Button btn) btn.TestClickButton();
        }
    }
}