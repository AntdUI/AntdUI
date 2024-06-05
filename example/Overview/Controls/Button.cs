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

namespace Overview.Controls
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
            panel2.Width = button2.Width + button15.Width + button10.Width + panel2.Padding.Horizontal + (int)(panel2.Shadow * AntdUI.Config.Dpi) * 2;
        }
        private void Btn(object sender, EventArgs e)
        {
            AntdUI.Button btn = (AntdUI.Button)sender;
            btn.Loading = true;
            bool change = false;
            if (btn.Parent == panel2)
            {
                change = true;
                panel2.Width = button2.Width + button15.Width + button10.Width + panel2.Padding.Horizontal + (int)(panel2.Shadow * AntdUI.Config.Dpi) * 2;
            }
            Task.Run(() =>
            {
                Thread.Sleep(2000);
                btn.Loading = false;
            }).ContinueWith(action =>
            {
                if (change)
                {
                    panel2.Invoke(() =>
                    {
                        panel2.Width = button2.Width + button15.Width + button10.Width + panel2.Padding.Horizontal + (int)(panel2.Shadow * AntdUI.Config.Dpi) * 2;
                    });
                }
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
                Task.Run(() =>
                {
                    Thread.Sleep(2000);
                    btn.Loading = false;
                    btn.Invoke(() =>
                    {
                        btn.Enabled = true;
                    });
                });
            }
            else if (nnn > 5)
            {
                btn.Loading = true;
                Task.Run(() =>
                {
                    Thread.Sleep(2000);
                    btn.Loading = false;
                });
            }
            else
            {
                btn.Enabled = false;
                Task.Run(() =>
                {
                    Thread.Sleep(2000);
                    btn.Invoke(() =>
                    {
                        btn.Enabled = true;
                    });
                });
            }
        }
    }
}