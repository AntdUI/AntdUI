// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

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