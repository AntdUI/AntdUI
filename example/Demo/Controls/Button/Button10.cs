// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class Button10 : UserControl
    {
        AntdUI.BaseForm form;
        public Button10(AntdUI.BaseForm _form)
        {
            form = _form;
            InitializeComponent();
        }

        private void b1_Click(object sender, EventArgs e)
        {
            b2.Loading = b3.Loading = b4.Loading = !b2.Loading;
        }
    }
}