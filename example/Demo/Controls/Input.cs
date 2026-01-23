// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Threading.Tasks;
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

        #region 验证码逻辑

        private void CodeGotFocus(object sender, EventArgs e)
        {
            if (sender is AntdUI.Input input)
            {
                var input_next = FindNullInput(input);
                if (input_next == null) return;
                BeginInvoke(input_next.Focus);
            }
        }

        AntdUI.Input FindNullInput(AntdUI.Input input)
        {
            int index = input.TabIndex - 1;
            AntdUI.Input tmp = null;
            while (true)
            {
                var find = tableLayoutPanel1.Controls.Find("ic" + index, false);
                if (find.Length == 1 && find[0] is AntdUI.Input next)
                {
                    index--;
                    if (string.IsNullOrWhiteSpace(next.Text)) tmp = next;
                }
                else return tmp;
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
                    else
                    {
                        input10.Focus();
                        tableLayoutPanel1.Enabled = false;
                        AntdUI.Spin.open(tableLayoutPanel1, async config =>
                        {
                            await Task.Delay(1000);
                            ic1.Clear();
                            ic2.Clear();
                            ic3.Clear();
                            ic4.Clear();
                        }, () =>
                        {
                            BeginInvoke(() =>
                            {
                                tableLayoutPanel1.Enabled = true;
                                ic1.Focus();
                            });
                        });
                        AntdUI.Message.info(form, AntdUI.Localization.Get("Input.Code2", "验证码：") + ic1.Text + " " + ic2.Text + " " + ic3.Text + " " + ic4.Text);
                    }
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
            if (e.KeyChar == 8 && sender is AntdUI.Input input)
            {
                //Back
                input.Clear();
                input.ClearUndo();
                var find = tableLayoutPanel1.Controls.Find("ic" + (input.TabIndex - 1), false);
                if (find.Length == 1 && find[0] is AntdUI.Input input_next)
                {
                    input_next.Text = "";
                    input_next.Focus();
                }
            }
        }

        #endregion
    }
}