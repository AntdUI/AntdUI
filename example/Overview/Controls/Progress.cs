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

namespace Overview.Controls
{
    public partial class Progress : UserControl
    {
        Form form;
        public Progress(Form _form)
        {
            form = _form;
            InitializeComponent();
        }

        private void Progress_Blue_1(object sender, EventArgs e)
        {
            progress1.Value = 0F;
            AntdUI.ITask.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        progress1.Value += 0.001F;
                        if (progress1.Value >= 1)
                        {
                            Thread.Sleep(1000);
                            progress1.Value = 0.5F;
                            return;
                        }
                        Thread.Sleep(10);
                    }
                    catch
                    {
                        return;
                    }
                }
            });
        }

        private void Progress_Blue_2(object sender, EventArgs e)
        {
            progress4.Value = progress7.Value = 0F;
            AntdUI.ITask.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        progress7.Value = progress4.Value += 0.001F;
                        if (progress4.Value >= 1)
                        {
                            Thread.Sleep(1000);
                            progress4.Value = progress7.Value = 0.68F;
                            return;
                        }
                        Thread.Sleep(10);
                    }
                    catch
                    {
                        return;
                    }
                }
            });
        }

        private void Progress_Red(object sender, EventArgs e)
        {
            progress3.State = progress6.State = AntdUI.TType.None;
            progress3.Value = progress6.Value = progress9.Value = 0F;
            AntdUI.ITask.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        progress3.Value = progress6.Value = progress9.Value += 0.001F;
                        if (progress6.Value >= 0.7)
                        {
                            progress3.Value = progress6.Value = progress9.Value = 0.7F;
                            progress3.State = progress6.State = AntdUI.TType.Error;
                            return;
                        }
                        Thread.Sleep(10);
                    }
                    catch
                    {
                        return;
                    }
                }
            });
        }
    }
}