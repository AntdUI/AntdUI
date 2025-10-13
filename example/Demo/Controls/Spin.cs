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
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class Spin : UserControl
    {
        Form form;
        public Spin(Form _form)
        {
            form = _form;
            InitializeComponent();
        }

        private void btnPanel_Click(object sender, EventArgs e)
        {
            AntdUI.Spin.open(this, AntdUI.Localization.Get("Loading2", "正在加载中..."), config =>
            {
                Thread.Sleep(1000);
                for (int i = 0; i < 101; i++)
                {
                    config.Value = i / 100F;
                    config.Text = AntdUI.Localization.Get("Processing", "处理中") + " " + i + "%";
                    Thread.Sleep(20);
                }
                Thread.Sleep(1000);
                config.Value = null;
                config.Text = AntdUI.Localization.Get("PleaseWait", "请耐心等候...");
                Thread.Sleep(2000);

            }, () =>
            {
                System.Diagnostics.Debug.WriteLine("加载结束");
            });
        }

        private void btnControl_Click(object sender, EventArgs e)
        {
            stackPanel2.Spin(AntdUI.Localization.Get("Loading2", "正在加载中..."), config =>
            {
                Thread.Sleep(1000);
                for (int i = 0; i < 101; i++)
                {
                    config.Value = i / 100F;
                    config.Text = AntdUI.Localization.Get("Processing", "处理中") + " " + i + "%";
                    Thread.Sleep(20);
                }
                Thread.Sleep(1000);
                config.Value = null;
                config.Text = AntdUI.Localization.Get("PleaseWait", "请耐心等候...");
                Thread.Sleep(2000);
            }, () =>
            {
                System.Diagnostics.Debug.WriteLine("加载结束");
            });
        }

        private void btnWindow_Click(object sender, EventArgs e)
        {
            AntdUI.Spin.open(form, AntdUI.Localization.Get("Loading2", "正在加载中..."), config =>
            {
                Thread.Sleep(1000);
                for (int i = 0; i < 101; i++)
                {
                    config.Value = i / 100F;
                    config.Text = AntdUI.Localization.Get("Processing", "处理中") + " " + i + "%";
                    Thread.Sleep(20);
                }
                Thread.Sleep(1000);
                config.Value = null;
                config.Text = AntdUI.Localization.Get("PleaseWait", "请耐心等候...");
                Thread.Sleep(2000);
            }, () =>
            {
                System.Diagnostics.Debug.WriteLine("加载结束");
            });
        }

        private void buttonError_Click(object sender, EventArgs e)
        {
            AntdUI.Spin.open(form, AntdUI.Localization.Get("Loading2", "正在加载中..."), config =>
            {
                Thread.Sleep(1000);
                for (int i = 0; i < 101; i++)
                {
                    config.Value = i / 100F;
                    config.Text = AntdUI.Localization.Get("Processing", "处理中") + " " + i + "%";

                    if (i > 50)
                        throw new Exception("模拟执行中发生了错误");
                    Thread.Sleep(20);
                }

                Thread.Sleep(1000);
                config.Value = null;
                config.Text = AntdUI.Localization.Get("PleaseWait", "请耐心等候...");
                Thread.Sleep(2000);
            }, () =>
            {
                // 如果发生错误则不会执行这里
                System.Diagnostics.Debug.WriteLine("加载结束");
            },
            ex => //错误回调
            {
                Debug.Print($"执行时发生了错误:{ex.Message}");
            });
        }
    }
}