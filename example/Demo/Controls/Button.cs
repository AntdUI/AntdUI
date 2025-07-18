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
    public partial class Button : UserControl
    {
        Form form;
        public Button(Form _form)
        {
            form = _form;
            InitializeComponent();
            button54.LoadingWaveColor = AntdUI.Style.Db.Success;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            panel_btns.Width = btng1.Width + btng2.Width + btng3.Width + panel_btns.Padding.Horizontal + (int)(panel_btns.Shadow * AntdUI.Config.Dpi) * 2;
        }
        private void Btns(object sender, EventArgs e)
        {
            AntdUI.Button btn = (AntdUI.Button)sender;
            btn.Loading = true;
            bool change = false;
            if (btn.Parent == panel_btns)
            {
                change = true;
                UpdatePanelWidth();
            }
            AntdUI.ITask.Run(() =>
            {
                Thread.Sleep(2000);
                if (btn.IsDisposed) return;
                btn.Loading = false;
                btn.Invoke(new Action(() =>
                {
                    if (btn.IsDisposed) return;
                    if (change) UpdatePanelWidth();
                }));
            });
        }

        Random random = new Random();
        private void Btn(object sender, EventArgs e)
        {
            AntdUI.Button btn = (AntdUI.Button)sender;
            btn.LoadingWaveValue = 0;
            if (random.Next(0, 10) > 3)
            {
                btn.Loading = true;
                AntdUI.ITask.Run(() =>
                {
                    Thread.Sleep(1000);
                    for (int i = 0; i < 101; i++)
                    {
                        btn.LoadingWaveValue = i / 100F;
                        Thread.Sleep(20);
                    }
                    Thread.Sleep(2000);
                }, () =>
                {
                    if (btn.IsDisposed) return;
                    btn.Loading = false;
                });
            }
            else
            {
                btn.Loading = true;
                AntdUI.ITask.Run(() =>
                {
                    Thread.Sleep(2000);
                    if (btn.IsDisposed) return;
                    btn.Loading = false;
                });
            }
        }

        private void UpdatePanelWidth()
        {
            if (panel_btns.IsDisposed) return;
            panel_btns.Width = btng1.Width + btng2.Width + btng3.Width + panel_btns.Padding.Horizontal + (int)(panel_btns.Shadow * AntdUI.Config.Dpi) * 2;
        }

        private void switch1_CheckedChanged(object sender, AntdUI.BoolEventArgs e) => panel1.Enabled = e.Value;

        private void switch2_CheckedChanged(object sender, AntdUI.BoolEventArgs e) => panel2.Enabled = e.Value;

        private void switch3_CheckedChanged(object sender, AntdUI.BoolEventArgs e) => panel3.Enabled = e.Value;

        private void switch4_CheckedChanged(object sender, AntdUI.BoolEventArgs e) => panel4.Enabled = e.Value;

        private void switch5_CheckedChanged(object sender, AntdUI.BoolEventArgs e) => panel5.Enabled = e.Value;

        private void switch6_CheckedChanged(object sender, AntdUI.BoolEventArgs e) => panel6.Enabled = e.Value;
    }
}