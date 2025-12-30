// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Drawing;
using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class Notification : UserControl
    {
        AntdUI.BaseForm form;
        public Notification(AntdUI.BaseForm _form)
        {
            form = _form;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AntdUI.Notification.info(form, "Notification " + button1.Text, "Hello, Ant Design!", AntdUI.TAlignFrom.TL, Font);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AntdUI.Notification.open(new AntdUI.Notification.Config(form, "Notification " + button2.Text, "Hello, Ant Design!", AntdUI.TType.Info, AntdUI.TAlignFrom.TR, Font)
            {
                Radius = 10,
                FontStyleTitle = FontStyle.Bold,
                Link = new AntdUI.Notification.ConfigLink("前往查看", () =>
                {
                    MessageBox.Show("点击超链接");
                    return true;
                })
            });
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AntdUI.Notification.info(form, "Notification " + button3.Text, "Hello, Ant Design!", AntdUI.TAlignFrom.BL, Font);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AntdUI.Notification.info(form, "Notification " + button4.Text, "Hello, Ant Design!", AntdUI.TAlignFrom.BR, Font);
        }

        readonly string text = "This is the content of the notification. This is the content of the notification. This is the content of the notification.";

        private void button5_Click(object sender, EventArgs e)
        {
            AntdUI.Notification.info(form, "Notification " + button5.Text, text, AntdUI.TAlignFrom.Top, Font);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            AntdUI.Notification.info(form, "Notification " + button6.Text, text, AntdUI.TAlignFrom.Bottom, Font);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            AntdUI.Notification.success(form, "Notification Title", text, AntdUI.TAlignFrom.TR, Font);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            AntdUI.Notification.error(form, "Notification Title", text, AntdUI.TAlignFrom.TR, Font);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            AntdUI.Notification.warn(form, "Notification Title", text, AntdUI.TAlignFrom.TR, Font);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            AntdUI.Notification.info(form, "Notification Title", text, AntdUI.TAlignFrom.TR, Font);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            AntdUI.Notification.open(new AntdUI.Notification.Config(form, "Notification Title", text, AntdUI.TType.Success, AntdUI.TAlignFrom.TR).SetEnableSound());
        }

        private void button13_Click(object sender, EventArgs e)
        {
            AntdUI.Notification.open(new AntdUI.Notification.Config(form, "Notification Title", text, AntdUI.TType.Error, AntdUI.TAlignFrom.TR).SetEnableSound());
        }

        private void button12_Click(object sender, EventArgs e)
        {
            AntdUI.Notification.open(new AntdUI.Notification.Config(form, "Notification Title", text, AntdUI.TType.Warn, AntdUI.TAlignFrom.TR).SetEnableSound());
        }

        private void button11_Click(object sender, EventArgs e)
        {
            AntdUI.Notification.open(new AntdUI.Notification.Config(form, "Notification Title", text, AntdUI.TType.Info, AntdUI.TAlignFrom.TR).SetEnableSound());
        }
    }
}