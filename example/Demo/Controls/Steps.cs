// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
// THE AntdUI PROJECT IS AN WINFORM LIBRARY LICENSED UNDER THE Apache-2.0 License.
// LICENSED UNDER THE Apache License, VERSION 2.0 (THE "License")
// YOU MAY NOT USE THIS FILE EXCEPT IN COMPLIANCE WITH THE License.
// YOU MAY OBTAIN A COPY OF THE LICENSE AT
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// UNLESS REQUIRED BY APPLICABLE LAW OR AGREED TO IN WRITING, SOFTWARE
// DISTRIBUTED UNDER THE LICENSE IS DISTRIBUTED ON AN "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED.
// SEE THE LICENSE FOR THE SPECIFIC LANGUAGE GOVERNING PERMISSIONS AND
// LIMITATIONS UNDER THE License.
// GITCODE: https://gitcode.com/AntdUI/AntdUI
// GITEE: https://gitee.com/AntdUI/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

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