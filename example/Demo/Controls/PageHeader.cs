// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class PageHeader : UserControl
    {
        AntdUI.BaseForm form;
        public PageHeader(AntdUI.BaseForm _form)
        {
            form = _form;
            InitializeComponent();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            button1.Toggle = pageHeader1.ShowBack = pageHeader2.ShowBack = pageHeader4.ShowBack = !button1.Toggle;
        }
    }
}