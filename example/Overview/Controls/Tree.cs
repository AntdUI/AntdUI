// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
// THE AntdUI PROJECT IS AN WINFORM LIBRARY LICENSED UNDER THE GPL-3.0 License.
// LICENSED UNDER THE GPL License, VERSION 3.0 (THE "License")
// YOU MAY NOT USE THIS FILE EXCEPT IN COMPLIANCE WITH THE License.
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
    public partial class Tree : UserControl
    {
        public Tree()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            tree1.PauseLayout = tree2.PauseLayout = true;
            Task.Run(() =>
            {
                Thread.Sleep(200);
                var random = new Random();
                for (int i = 0; i < random.Next(7, 20); i++)
                {
                    var it = new AntdUI.TreeItem("Tree1 " + (i + 1));
                    AddSub(it, 1, random);
                    tree1.Items.Add(it);
                }
                for (int i = 0; i < random.Next(7, 20); i++)
                {
                    var it = new AntdUI.TreeItem("Tree2 " + (i + 1));
                    AddSub(it, 1, random);
                    tree2.Items.Add(it);
                }
            }).ContinueWith(action =>
            {
                tree1.PauseLayout = tree2.PauseLayout = false;
            });
        }

        void AddSub(AntdUI.TreeItem it, int d, Random random)
        {
            if (random.Next(0, 10) > 5 && d < 10)
            {
                var list = new List<AntdUI.TreeItem>();
                for (int i = 0; i < random.Next(3, 9); i++)
                {
                    var its = new AntdUI.TreeItem("Sub_" + d + " " + (i + 1));
                    if (d == 1)
                    {
                        int c = random.Next(0, 10);
                        if (c > 6) its.Icon = Properties.Resources.bg1;
                    }
                    AddSub(its, d + 1, random);
                    list.Add(its);
                }
                it.Sub.AddRange(list);
            }
        }
    }
}