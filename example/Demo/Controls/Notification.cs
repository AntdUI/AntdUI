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
using System.Drawing;
using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class Notification : UserControl
    {
        Form form;
        public Notification(Form _form)
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

        private void button5_Click(object sender, EventArgs e)
        {
            AntdUI.Notification.info(form, "Notification " + button5.Text, "This is the content of the notification. This is the content of the notification. This is the content of the notification.", AntdUI.TAlignFrom.Top, Font);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            AntdUI.Notification.info(form, "Notification " + button6.Text, "This is the content of the notification. This is the content of the notification. This is the content of the notification.", AntdUI.TAlignFrom.Bottom, Font);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            AntdUI.Notification.success(form, "Notification Title", "This is the content of the notification. This is the content of the notification. This is the content of the notification.", AntdUI.TAlignFrom.TR, Font);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            AntdUI.Notification.error(form, "Notification Title", "This is the content of the notification. This is the content of the notification. This is the content of the notification.", AntdUI.TAlignFrom.TR, Font);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            AntdUI.Notification.warn(form, "Notification Title", "This is the content of the notification. This is the content of the notification. This is the content of the notification.", AntdUI.TAlignFrom.TR, Font);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            AntdUI.Notification.info(form, "Notification Title", "This is the content of the notification. This is the content of the notification. This is the content of the notification.", AntdUI.TAlignFrom.TR, Font);
        }
    }
}