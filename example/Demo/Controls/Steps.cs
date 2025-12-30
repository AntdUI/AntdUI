// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class Steps : UserControl
    {
        AntdUI.BaseForm form;
        public Steps(AntdUI.BaseForm _form)
        {
            form = _form;
            InitializeComponent();
        }

        private void Steps_Click(object sender, EventArgs e)
        {
            if (sender is AntdUI.Steps step)
            {
                if (step.Current > step.Items.Count - 1) step.Current = 0;
                else step.Current++;
            }
        }

        private void switch1_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            steps2.MilestoneCurrentCompleted = steps4.MilestoneCurrentCompleted = !e.Value;
        }
    }
}