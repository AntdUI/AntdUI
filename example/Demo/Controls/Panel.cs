// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class Panel : UserControl
    {
        Overview form;
        public Panel(Overview _form)
        {
            form = _form;
            InitializeComponent();
            button4.Click += button_Click;
        }

        private void button_Click(object sender, System.EventArgs e)
        {
            form.OpenPage("VirtualPanel");
        }
    }
}