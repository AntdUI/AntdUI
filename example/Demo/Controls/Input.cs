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
// GITEE: https://gitee.com/AntdUI/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

using System;
using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class Input : UserControl
    {
        Form form;
        public Input(Form _form)
        {
            form = _form;
            InitializeComponent();
        }

        private void Btn(object sender, EventArgs e)
        {
            if (sender is AntdUI.Button btn)
            {
                btn.Loading = true;
                AntdUI.ITask.Run(() =>
                {
                    System.Threading.Thread.Sleep(2000);
                    btn.Loading = false;
                });
            }
        }

        private void CodeTextChanged(object sender, EventArgs e)
        {
            if (sender is AntdUI.Input input)
            {
                if (!string.IsNullOrWhiteSpace(input.Text))
                {
                    var find = tableLayoutPanel1.Controls.Find("ic" + (input.TabIndex + 1), false);
                    if (find.Length == 1 && find[0] is AntdUI.Input input_next)
                    {
                        input_next.Text = "";
                        input_next.Focus();
                    }
                }
                else
                {
                }
            }
        }

        private void CodeKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 8 && sender is AntdUI.Input input && input.Text.Length == 0)
            {
                //Back
                var find = tableLayoutPanel1.Controls.Find("ic" + (input.TabIndex - 1), false);
                if (find.Length == 1 && find[0] is AntdUI.Input input_next)
                {
                    input_next.Text = "";
                    input_next.Focus();
                }
            }
        }
    }
}