// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class Preview : UserControl
    {
        AntdUI.BaseForm form;
        public Preview(AntdUI.BaseForm _form)
        {
            form = _form;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AntdUI.Preview.open(new AntdUI.Preview.Config(form, Properties.Resources.img1));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AntdUI.Preview.open(new AntdUI.Preview.Config(form, new Image[] { Properties.Resources.bg1, Properties.Resources.bg7, Properties.Resources.bg2, Properties.Resources.bg3 })
            {
                Btns = new AntdUI.Preview.Btn[] {
                    new AntdUI.Preview.Btn("download","<svg viewBox=\"64 64 896 896\"><path d=\"M505.7 661a8 8 0 0012.6 0l112-141.7c4.1-5.2.4-12.9-6.3-12.9h-74.1V168c0-4.4-3.6-8-8-8h-60c-4.4 0-8 3.6-8 8v338.3H400c-6.7 0-10.4 7.7-6.3 12.9l112 141.8zM878 626h-60c-4.4 0-8 3.6-8 8v154H214V634c0-4.4-3.6-8-8-8h-60c-4.4 0-8 3.6-8 8v198c0 17.7 14.3 32 32 32h684c17.7 0 32-14.3 32-32V634c0-4.4-3.6-8-8-8z\"></path></svg>")
                },
                OnBtns = (id, data) =>
                {
                    switch (id)
                    {
                        case "download":
                            //我点击了下载按钮
                            break;
                    }
                }
            });
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AntdUI.Preview.open(new AntdUI.Preview.Config(form, new object[] { 1, 2, 3, 7 }, (i, tag, call) =>
            {
                if (i == 0) return Properties.Resources.bg1;
                else if (i == 2) return Properties.Resources.bg3;
                else if (i == 3)
                {
                    for (int prog = 0; prog <= 100; prog++)
                    {
                        System.Threading.Thread.Sleep(10);
                        call(prog / 100F, prog + "%");
                    }
                    System.Threading.Thread.Sleep(500);
                    return Properties.Resources.bg7;
                }
                for (int prog = 0; prog <= 20; prog++)
                {
                    System.Threading.Thread.Sleep(50);
                    call(prog / 100F, prog + "%");
                }
                System.Threading.Thread.Sleep(500);
                call(0.2F, "加载失败");
                return null;
            })
            {
                Btns = new AntdUI.Preview.Btn[] {
                    new AntdUI.Preview.Btn("download","<svg viewBox=\"64 64 896 896\"><path d=\"M505.7 661a8 8 0 0012.6 0l112-141.7c4.1-5.2.4-12.9-6.3-12.9h-74.1V168c0-4.4-3.6-8-8-8h-60c-4.4 0-8 3.6-8 8v338.3H400c-6.7 0-10.4 7.7-6.3 12.9l112 141.8zM878 626h-60c-4.4 0-8 3.6-8 8v154H214V634c0-4.4-3.6-8-8-8h-60c-4.4 0-8 3.6-8 8v198c0 17.7 14.3 32 32 32h684c17.7 0 32-14.3 32-32V634c0-4.4-3.6-8-8-8z\"></path></svg>")
                },
                OnBtns = (id, data) =>
                {
                    switch (id)
                    {
                        case "download":
                            //我点击了下载按钮
                            break;
                    }
                }
            });
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var imgTextList = new List<AntdUI.Preview.ImageTextContent>()
            {
                new AntdUI.Preview.ImageTextContent(Properties.Resources.bg1, "绿绿的树叶(顶部居中)", Color.Green)
                {
                    TextAlign = ContentAlignment.TopCenter
                },
                new AntdUI.Preview.ImageTextContent(Properties.Resources.bg7, "韦一敏,这盛世如你所愿(顶部居左)", Color.Red)
                {
                    TextAlign = ContentAlignment.TopLeft
                },
                new AntdUI.Preview.ImageTextContent(Properties.Resources.bg2, "这叫艺术(顶部居右)")
                {
                    TextAlign = ContentAlignment.TopRight
                },
                new AntdUI.Preview.ImageTextContent(Properties.Resources.bg1, "绿绿的树叶(中部居中)", Color.Green)
                {
                    TextAlign = ContentAlignment.MiddleCenter
                },
                new AntdUI.Preview.ImageTextContent(Properties.Resources.bg7, "韦一敏,这盛世如你所愿(中部居左)", Color.Red)
                {
                    TextAlign = ContentAlignment.MiddleLeft
                },
                new AntdUI.Preview.ImageTextContent(Properties.Resources.bg2, "这叫艺术(中部居右)")
                {
                    TextAlign = ContentAlignment.MiddleRight
                },
                new AntdUI.Preview.ImageTextContent(Properties.Resources.bg1, "绿绿的树叶(底部居中)", Color.Green)
                {
                    TextAlign = ContentAlignment.BottomCenter
                },
                new AntdUI.Preview.ImageTextContent(Properties.Resources.bg7, "韦一敏,这盛世如你所愿(底部居左)",Color.Red)
                {
                    TextAlign = ContentAlignment.BottomLeft
                },
                new AntdUI.Preview.ImageTextContent(Properties.Resources.bg2, "这叫艺术(底部居右)")
                {
                    TextAlign = ContentAlignment.BottomRight
                },
                new AntdUI.Preview.ImageTextContent(Properties.Resources.bg7)
                {
                    Text = "这个是自动换行,这个是自动换行,这个是自动换行,这个是自动换行,这个是自动换行,这个是自动换行,这个是自动换行,这个是自动换行,这个是自动换行,这个是自动换行,这个是自动换行",
                    TextAlign = ContentAlignment.BottomLeft
                }
            };
            AntdUI.Preview.open(new AntdUI.Preview.Config(form, imgTextList));
        }
    }
}