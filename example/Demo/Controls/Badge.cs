// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class Badge : UserControl
    {
        AntdUI.BaseForm form;
        public Badge(AntdUI.BaseForm _form)
        {
            form = _form;
            InitializeComponent();
        }

        private void button5_Click(object sender, System.EventArgs e)
        {
            button4.Enabled = false;
            AntdUI.ITask.Run(() =>
            {
                System.Threading.Thread.Sleep(2000);
                button4.Enabled = true;
            });
        }
    }
}