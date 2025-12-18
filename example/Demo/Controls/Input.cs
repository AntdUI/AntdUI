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
    public partial class Input : UserControl
    {
        AntdUI.BaseForm form;
        public Input(AntdUI.BaseForm _form)
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

        private void VerifyKeyboard(object sender, AntdUI.InputVerifyKeyboardEventArgs e)
        {
            if (sender is AntdUI.Input input && e.KeyData == (Keys.Control | Keys.V))
            {
                e.Result = false;
                var strText = AntdUI.Helper.ClipboardGetText();
                if (strText == null || string.IsNullOrEmpty(strText)) return;
                int i = 0;
                foreach (var it in strText)
                {
                    if (i == 0) input.Text = it.ToString();
                    else
                    {
                        var find = tableLayoutPanel1.Controls.Find("ic" + (input.TabIndex + i), false);
                        if (find.Length == 1 && find[0] is AntdUI.Input input_next)
                        {
                            input_next.Text = it.ToString();
                            input_next.Focus();
                        }
                        else return;
                    }
                    i++;
                }

                var find_last = tableLayoutPanel1.Controls.Find("ic" + (input.TabIndex + i), false);
                if (find_last.Length == 1 && find_last[0] is AntdUI.Input input_last_next)
                {
                    input_last_next.Text = "";
                    input_last_next.Focus();
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