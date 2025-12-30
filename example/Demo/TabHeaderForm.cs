// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Diagnostics;
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
            tabHeader1.AddTab(new AntdUI.TagTabItem("关于").SetID("about_tab").SetBadge("New").SetBadgeBack(Color.Red));
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

        private void tabHeader1_TabClosing(object sender, AntdUI.TabCloseEventArgs e)
        {
            // 在事件处理中输出ID以便判断准确性
            Debug.Print($"正在关闭标签，ID: {e.Value.ID ?? "未设置ID"}, 索引: {e.Index}, 文本: {e.Value.Text}");
        }

        private void tabHeader1_TabChanged(object sender, AntdUI.TabChangedEventArgs e)
        {
            // 在事件处理中输出ID以便判断准确性
            Debug.Print($"切换到标签，ID: {e.Value.ID ?? "未设置ID"}, 索引: {e.Index}, 文本: {e.Value.Text}");
        }
    }
}
