// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Windows.Forms;

namespace ExtendedTest
{
    public partial class MainForm : AntdUI.Window
    {
        public MainForm()
        {
            InitializeComponent();
            tab.Pages.Add(BuildTabPage("Ribbon", new RibbonDemo(this) { Dock = DockStyle.Fill }));
            tab.Pages.Add(BuildTabPage("OutlookBar", new OutlookBarDemo(this) { Dock = DockStyle.Fill }));
            tab.Pages.Add(BuildTabPage("DockPanel", new DockDemo(this) { Dock = DockStyle.Fill }));
        }

        static AntdUI.TabPage BuildTabPage(string title, Control content)
        {
            var page = new AntdUI.TabPage { Text = title, Dock = DockStyle.Fill };
            page.Controls.Add(content);
            return page;
        }

        private void colorTheme_ValueChanged(object sender, AntdUI.ColorEventArgs e)
        {
            AntdUI.Style.SetPrimary(e.Value);
            Refresh();
        }

        private void btn_mode_Click(object sender, EventArgs e)
        {
            AntdUI.Config.IsDark = !AntdUI.Config.IsDark;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            DraggableMouseDown();
            base.OnMouseDown(e);
        }

        public void Append(string s) => log.Text = s + "\n" + log.Text;
    }
}
