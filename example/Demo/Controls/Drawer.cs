// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Drawing;
using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class Drawer : UserControl
    {
        AntdUI.BaseForm form;
        public Drawer(AntdUI.BaseForm _form)
        {
            form = _form;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var align = AntdUI.TAlignMini.Right;
            if (radio1.Checked) align = AntdUI.TAlignMini.Top;
            if (radio4.Checked) align = AntdUI.TAlignMini.Left;
            if (radio3.Checked) align = AntdUI.TAlignMini.Bottom;

            AntdUI.Drawer.open(form, new Avatar(form)
            {
                Size = new Size(500, 300)
            }, align);
        }
    }
}