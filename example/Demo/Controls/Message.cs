// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Threading;
using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class Message : UserControl
    {
        AntdUI.BaseForm form;
        public Message(AntdUI.BaseForm _form)
        {
            form = _form;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked) AntdUI.Message.open(new AntdUI.Message.Config(form, "This is a success message", AntdUI.TType.Success).SetEnableSound());
            else AntdUI.Message.success(form, "This is a success message", Font);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked) AntdUI.Message.open(new AntdUI.Message.Config(form, "This is a error message", AntdUI.TType.Error).SetEnableSound());
            else AntdUI.Message.error(form, "This is a error message", Font);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked) AntdUI.Message.open(new AntdUI.Message.Config(form, "This is a warn message", AntdUI.TType.Warn).SetEnableSound());
            else AntdUI.Message.warn(form, "This is a warn message", Font);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked) AntdUI.Message.open(new AntdUI.Message.Config(form, "Hello, Ant Design!", AntdUI.TType.Info).SetEnableSound());
            else AntdUI.Message.info(form, "Hello, Ant Design!", Font);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            button8.Enabled = false;
            AntdUI.Message.loading(form, "Action in progress..", (config) =>
            {
                for (int i = 0; i < 100; i++)
                {
                    Thread.Sleep(10);
                    config.Text = AntdUI.Localization.Get("Loading", "加载中") + " " + (i + 1) + "%";
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

        private void button9_Click(object sender, EventArgs e)
        {
            AntdUI.Message.open(new AntdUI.Message.Config(form, "这是一条被替换的长消息内容，宽度与高度都发生了变化，其余消息应当平滑过渡补位。", AntdUI.TType.Warn)
            {
                AutoClose = 10,
                MaxWidth = 320
            }.SetID("replace"));
        }

        private void button10_Click(object sender, EventArgs e)
        {
            AntdUI.Message.open(new AntdUI.Message.Config(form, "初始内容", AntdUI.TType.Success)
            {
                AutoClose = 10
            }.SetID("replace"));
        }

        private void button11_Click(object sender, EventArgs e)
        {
            AntdUI.Message.close_id("replace");
        }

        private void button12_Click(object sender, EventArgs e)
        {
            AntdUI.Message.close_all();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            // 并发弹出：立即显示，无队列堵塞
            int count = AntdUI.Message.contains("concurrent") ? 1 : 0;
            for (int i = 0; i < 5; i++)
            {
                int no = i + count;
                AntdUI.Message.open(new AntdUI.Message.Config(form, "并发消息 " + (no + 1) + " - " + DateTime.Now.ToString("HH:mm:ss.fff"), AntdUI.TType.Info)
                {
                    AutoClose = 10
                }.SetID("concurrent" + no));
            }
        }
    }
}