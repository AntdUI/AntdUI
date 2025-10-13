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
using System.Threading;
using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class Message : UserControl
    {
        Form form;
        public Message(Form _form)
        {
            form = _form;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AntdUI.Message.success(form, "This is a success message", Font);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AntdUI.Message.error(form, "This is a error message", Font);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AntdUI.Message.warn(form, "This is a warn message", Font);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AntdUI.Message.info(form, "Hello, Ant Design!", Font);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            button8.Enabled = false;
            AntdUI.Message.loading(form, "Action in progress..", (config) =>
            {
                for (int i = 0; i < 100; i++)
                {
                    Thread.Sleep(10);
                    config.Text = AntdUI.Localization.Get("Loading", "Мгдижа") + " " + (i + 1) + "%";
                    config.Refresh();
                }
                Thread.Sleep(1000);
                config.Text = "Action in progress..";
                config.Refresh();
                Thread.Sleep(2000);
                config.OK("This is a success message");
                Invoke(new Action(() =>
                {
                    button8.Enabled = true;
                }));
            }, Font);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            button7.Enabled = false;
            AntdUI.Message.loading(form, "Action in progress..", (config) =>
            {
                Thread.Sleep(3000);
                config.Error("This is a error message");
                Invoke(new Action(() =>
                {
                    button7.Enabled = true;
                }));
            }, Font);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            button6.Enabled = false;
            AntdUI.Message.loading(form, "Action in progress..", (config) =>
            {
                Thread.Sleep(3000);
                config.Warn("This is a warn message");
                Invoke(new Action(() =>
                {
                    button6.Enabled = true;
                }));
            }, Font);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            button5.Enabled = false;
            AntdUI.Message.loading(form, "Action in progress..", (config) =>
            {
                Thread.Sleep(3000);
                config.Info("Hello, Ant Design!");
                Invoke(new Action(() =>
                {
                    button5.Enabled = true;
                }));
            }, Font);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            AntdUI.Message.open(new AntdUI.Message.Config(form, "This is a success message", AntdUI.TType.Success).SetEnableSound());
        }

        private void button11_Click(object sender, EventArgs e)
        {
            AntdUI.Message.open(new AntdUI.Message.Config(form, "This is a error message", AntdUI.TType.Error).SetEnableSound());
        }

        private void button10_Click(object sender, EventArgs e)
        {
            AntdUI.Message.open(new AntdUI.Message.Config(form, "This is a warn message", AntdUI.TType.Warn).SetEnableSound());
        }

        private void button9_Click(object sender, EventArgs e)
        {
            AntdUI.Message.open(new AntdUI.Message.Config(form, "Hello, Ant Design!", AntdUI.TType.Info).SetEnableSound());
        }
    }
}