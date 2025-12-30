// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class Modal : UserControl
    {
        AntdUI.BaseForm form;
        public Modal(AntdUI.BaseForm _form)
        {
            form = _form;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AntdUI.Modal.open(new AntdUI.Modal.Config(form, "This is a success message", "Some contents...Some contents...Some contents...Some contents...Some contents...Some contents...Some contents...", AntdUI.TType.Success)
            {
                OnButtonStyle = (id, btn) =>
                {
                    btn.BackExtend = "135, #6253E1, #04BEFE";
                },
                CancelText = null,
                OkText = "知道了"
            });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AntdUI.Modal.open(form, "This is a error message", "Some contents...Some contents...Some contents...Some contents...Some contents...Some contents...Some contents...", AntdUI.TType.Error);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button3.Enabled = false;
            AntdUI.Modal.open(new AntdUI.Modal.Config(form, "This is a warn message", "Some contents...Some contents...Some contents...Some contents...Some contents...Some contents...Some contents...", AntdUI.TType.Warn)
            {
                Btns = new AntdUI.Modal.Btn[] { new AntdUI.Modal.Btn("按钮Name", "自定义按钮", AntdUI.TTypeMini.Warn) },
                OnBtns = btn =>
                {
                    MessageBox.Show("触发的Name：" + btn.Name);
                    return true;
                },
                OnOk = config =>
                {
                    System.Threading.Thread.Sleep(1000);
                    return true;
                }
            });
            button3.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AntdUI.Modal.open(form, "Hello, Ant Design!", "Some contents...Some contents...Some contents...Some contents...Some contents...Some contents...Some contents...");
        }
    }
}