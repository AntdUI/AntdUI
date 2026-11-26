// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class Menu4 : UserControl
    {
        AntdUI.BaseForm form;
        public Menu4(AntdUI.BaseForm _form)
        {
            form = _form;
            InitializeComponent();
        }

        private void btn_toggle_Click(object sender, EventArgs e)
        {
            menu1.Collapsed = !menu1.Collapsed;
            menu1.Width = menu1.Collapsed ? (int)(66 * form.Dpi) : (int)(251 * form.Dpi);
        }
    }
}
