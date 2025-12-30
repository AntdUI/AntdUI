// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class Battery : UserControl
    {
        AntdUI.BaseForm form;
        public Battery(AntdUI.BaseForm _form)
        {
            form = _form;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) => SetBattery(5, Controls);

        private void button2_Click(object sender, EventArgs e) => SetBattery(-5, Controls);

        void SetBattery(int value, ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                if (control is AntdUI.Battery battery) battery.Value += value;
                if (control.Controls.Count > 0) SetBattery(value, control.Controls);
            }
        }
    }
}