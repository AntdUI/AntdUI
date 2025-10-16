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

namespace Demo
{
    public partial class TabHeaderForm : AntdUI.Window
    {
        public TabHeaderForm()
        {
            InitializeComponent();
            AntdUI.Config.Theme().Header(tabHeader1, "#f3f3f3", "#111111").Call(dark =>
            {
                tabHeader1.BackActive = dark ? Color.Black : Color.White;
            });
            tabHeader1.AddTab("关于", "SlackSquareFilled");
            tabHeader1.AddTab(new AntdUI.TagTabItem("关于"));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var tab = new AntdUI.TagTabItem(DateTime.Now.ToString(), "TikTokFilled").SetLoading(true);
            tabHeader1.AddTab(tab);
            AntdUI.ITask.Run(() =>
            {
                System.Threading.Thread.Sleep(2000); // 模拟加载延时
                tab.Loading = false;
            });
        }

        private void tabHeader1_AddClick(object sender, EventArgs e)
        {
            var tab = new AntdUI.TagTabItem("苹果", "AppleFilled").SetLoading(true);
            tabHeader1.AddTab(tab, true);
            AntdUI.ITask.Run(() =>
            {
                System.Threading.Thread.Sleep(2000); // 模拟加载延时
                tab.Loading = false;
            });
        }
    }
}
