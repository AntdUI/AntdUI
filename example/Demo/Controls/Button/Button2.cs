// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class Button2 : UserControl
    {
        AntdUI.BaseForm form;
        public Button2(AntdUI.BaseForm _form)
        {
            form = _form;
            InitializeComponent();
            b8.Text = b8.ProductName + " " + b8.ProductVersion;
        }

        private void btn_Click(object sender, System.EventArgs e)
        {
            if (sender is AntdUI.Button btn) btn.TestClickButton();
        }
    }
}