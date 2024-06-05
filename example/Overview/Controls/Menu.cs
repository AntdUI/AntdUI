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
    public partial class Menu : UserControl
    {
        Form form;
        public Menu(Form _form)
        {
            form = _form;
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Task.Run(() =>
            {
                Thread.Sleep(500);
                var random = new Random();
                for (int i = 0; i < random.Next(7, 20); i++)
                {
                    var it = new AntdUI.MenuItem("Menu " + (i + 1));
                    if (random.Next(0, 9) > 2)
                    {
                        for (int j = 0; j < random.Next(3, 9); j++)
                        {
                            var it2 = new AntdUI.MenuItem("Option " + (j + 1));
                            if (random.Next(0, 9) > 7)
                            {
                                for (int k = 0; k < random.Next(3, 9); k++)
                                {
                                    it2.Sub.Add(new AntdUI.MenuItem("Sub " + (k + 1)));
                                }
                            }
                            it.Sub.Add(it2);
                        }
                    }
                    menu2.Items.Add(it);
                }
            });
        }
    }
}