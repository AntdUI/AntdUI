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
// GITEE: https://gitee.com/antdui/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

using System;
using System.Threading;
using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class Button : UserControl
    {
        Form form;
        public Button(Form _form)
        {
            form = _form;
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            panel6.Width = btng1.Width + btng2.Width + btng3.Width + panel6.Padding.Horizontal + (int)(panel6.Shadow * AntdUI.Config.Dpi) * 2;
        }
        private void Btn(object sender, EventArgs e)
        {
            AntdUI.Button btn = (AntdUI.Button)sender;
            btn.Loading = true;
            bool change = false;
            if (btn.Parent == panel6)
            {
                change = true;
                UpdatePanelWidth();
            }
            AntdUI.ITask.Run(() =>
            {
                Thread.Sleep(2000);
                if (btn.IsDisposed) return;
                btn.Invoke(new Action(() =>
                {
                    if (btn.IsDisposed) return;
                    btn.Loading = false;
                    if (change) UpdatePanelWidth();
                }));
            });
        }

        Random random = new Random();
        private void Btn2(object sender, EventArgs e)
        {
            AntdUI.Button btn = (AntdUI.Button)sender;
            int nnn = random.Next(0, 20);
            if (nnn > 10)
            {
                btn.Loading = true;
                btn.Enabled = false;
                AntdUI.ITask.Run(() =>
                {
                    Thread.Sleep(2000);
                    if (btn.IsDisposed) return;
                    btn.Invoke(new Action(() =>
                    {
                        if (btn.IsDisposed) return;
                        btn.Loading = false;
                        btn.Enabled = true;
                    }));
                });
            }
            else if (nnn > 5)
            {
                btn.Loading = true;
                AntdUI.ITask.Run(() =>
                {
                    Thread.Sleep(2000);
                    if (btn.IsDisposed) return;
                    btn.Invoke(new Action(() =>
                    {
                        if (btn.IsDisposed) return;
                        btn.Loading = false;
                    }));
                });
            }
            else
            {
                btn.Enabled = false;
                AntdUI.ITask.Run(() =>
                {
                    Thread.Sleep(2000);
                    if (btn.IsDisposed) return;
                    btn.Invoke(new Action(() =>
                    {
                        if (btn.IsDisposed) return;
                        btn.Enabled = true;
                    }));
                });
            }
        }

        private void UpdatePanelWidth()
        {
            if (panel6.IsDisposed) return;
            panel6.Width = btng1.Width + btng2.Width + btng3.Width + panel6.Padding.Horizontal + (int)(panel6.Shadow * AntdUI.Config.Dpi) * 2;
        }
    }
}