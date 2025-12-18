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
using System.Threading;
using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class Tour : UserControl
    {
        AntdUI.BaseForm form;
        public Tour(AntdUI.BaseForm _form)
        {
            form = _form;
            InitializeComponent();
        }

        Random random = new Random();
        AntdUI.TourForm tourForm;
        private void Btn(object sender, EventArgs e)
        {
            if (sender == button3)
            {
                OpenTour(button1);
                return;
            }
            AntdUI.Button btn = (AntdUI.Button)sender;
            if (tourForm == null) OpenTour(btn);
            else
            {
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
        }

        void OpenTour(AntdUI.Button btn)
        {
            if (tourForm == null)
            {
                Form popover = null;
                tourForm = AntdUI.Tour.open(new AntdUI.Tour.Config(form, item =>
                {
                    switch (item.Index)
                    {
                        case 0:
                            item.Set(btn);
                            break;
                        case 1:
                            item.Set(button2);
                            break;
                        case 2:
                            item.Set(button7);
                            break;
                        case 3:
                            item.Set(button8);
                            break;
                        case 4:
                            item.Set(button3);
                            break;
                        case 5:
                            item.Set(button4);
                            break;
                        case 6:
                            item.Set(button5);
                            break;
                        case 7:
                            item.Set(button6);
                            break;
                        case 8:
                            var rect = form.ClientRectangle;
                            item.Set(new Rectangle(rect.X + (rect.Width - 200) / 2, rect.Y + (rect.Height - 200) / 2, 200, 200));
                            break;
                        default:
                            item.Close();
                            tourForm = null;
                            break;
                    }
                }, info =>
                {
                    popover?.Close();
                    popover = null;
                    if (info.Rect.HasValue) popover = AntdUI.Popover.open(new AntdUI.Popover.Config(info.Form, new TourPopover(info, info.Index == 8 ? "DIV Rectangle" : "Button " + (info.Index + 1), "Tour Step " + (info.Index + 1), (info.Index + 1), 9)) { Offset = info.Rect.Value, Focus = false });
                }));
            }
            else tourForm.Next();
        }
    }
}