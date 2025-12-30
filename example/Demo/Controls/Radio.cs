// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class Radio : UserControl
    {
        AntdUI.BaseForm form;
        public Radio(AntdUI.BaseForm _form)
        {
            form = _form;
            InitializeComponent();
        }

        private void button1_Click(object sender, System.EventArgs e) => radio9.Checked = !radio9.Checked;

        private void button2_Click(object sender, System.EventArgs e) => radio9.Enabled = !radio9.Enabled;

        private void radio9_CheckedChanged(object sender, AntdUI.BoolEventArgs e) => SetText();
        private void radio9_EnabledChanged(object sender, System.EventArgs e) => SetText();

        void SetText()
        {
            radio9.Text = (radio9.Checked ? "Checked" : "Unchecked") + "-" + (radio9.Enabled ? "Enabled" : "Disabled");
            button1.Text = radio9.Checked ? "Uncheck" : "Check";
            button2.Text = radio9.Enabled ? "Disable" : "Enable";
        }
    }
}