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
using System.Drawing;
using System.Windows.Forms;

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
            
            // 添加带徽标的标签
            var aboutTab = new AntdUI.TagTabItem("关于", "SlackSquareFilled");
            aboutTab.ID = "about_tab";
            aboutTab.Badge = "New";
            aboutTab.BadgeBack = Color.Red;
            tabHeader1.AddTab(aboutTab);
            
            // 添加普通标签
            var normalTab = new AntdUI.TagTabItem("普通标签");
            normalTab.ID = "normal_tab";
            tabHeader1.AddTab(normalTab);
            
            // 订阅TabHeader的事件
            tabHeader1.TabChanged += TabHeader1_TabChanged;
            tabHeader1.TabClosing += TabHeader1_TabClosing;
        }

        private void TabHeader1_TabClosing(object sender, AntdUI.TabCloseEventArgs e)
        {
            // 在事件处理中输出ID以便判断准确性
            Debug.Print($"正在关闭标签，ID: {e.Value.ID ?? "未设置ID"}, 索引: {e.Index}, 文本: {e.Value.Text}");
        }

        private void TabHeader1_TabChanged(object sender, AntdUI.TabChangedEventArgs e)
        {
            // 在事件处理中输出ID以便判断准确性
            Debug.Print($"切换到标签，ID: {e.Value.ID ?? "未设置ID"}, 索引: {e.Index}, 文本: {e.Value.Text}");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var tab = new AntdUI.TagTabItem(DateTime.Now.ToString(), "TikTokFilled").SetLoading(true);
            tab.ID = "time_" + DateTime.Now.Ticks;
            tab.Badge = "Hot";
            tab.BadgeBack = Color.Orange;
            tabHeader1.AddTab(tab);
            AntdUI.ITask.Run(() =>
            {
                System.Threading.Thread.Sleep(2000); // 模拟加载延时
                tab.Loading = false;
            });
        }

        // 在类的成员变量中添加计数器
        private int tabCounter = 0;
        private void tabHeader1_AddClick(object sender, EventArgs e)
        {
            tabCounter++; // 递增计数器

            var tab = new AntdUI.TagTabItem($"苹果{tabCounter}", "AppleFilled").SetLoading(true);
            tab.ID = $"apple_{tabCounter}";
            
            tabHeader1.AddTab(tab, true);

            AntdUI.ITask.Run(() =>
            {
                System.Threading.Thread.Sleep(2000); // 模拟加载延时
                tab.Loading = false;
            });
        }
    }
}
