// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.Drawing;
using System.Windows.Forms;

namespace ExtendedTest
{
    public partial class OutlookBarDemo : UserControl
    {
        MainForm form;
        public OutlookBarDemo(MainForm main)
        {
            form = main;
            InitializeComponent();
            bar.HeaderSplitResize = bar.Expanded = true;

            // Mail panel — many items to exercise scrolling; include tooltips on first few items.
            var mail = new AntdUI.OutlookPanel("Mail", "MailOutlined") { Tooltip = "Mail (F1 demonstrates ContentControl is NOT this one)" };
            for (int i = 1; i <= 40; i++)
            {
                int n = i;
                var mi = new AntdUI.OutlookItem("Inbox item #" + n, "MailOutlined", (s, e) => Append("OutlookItem clicked: Inbox item #" + n));
                if (n <= 3) mi.Tooltip = "Open inbox item #" + n;
                mail.Items.Add(mi);
            }

            // Explorer panel — uses ContentControl (TreeView). Demonstrates F1: host survives collapse/expand.
            // BorderStyle.None — OutlookBar draws its own themed body frame. A FixedSingle here would create a
            // doubled/mismatched border since the OS frame uses SystemColors, not our Colour tokens.
            var tree = new TreeView
            {
                BorderStyle = BorderStyle.None,
                HideSelection = false,
                BackColor = Color.White,
            };
            var root = tree.Nodes.Add("Workspace");
            root.Nodes.Add("Project A");
            root.Nodes.Add("Project B");
            root.Nodes.Add("Project C").Nodes.Add("Sub");
            root.Expand();
            tree.AfterSelect += (s, e) => Append("Tree selected: " + (e.Node?.Text ?? ""));
            var explorerPanel = new AntdUI.OutlookPanel("Explorer", "FolderOutlined")
            {
                ContentControl = tree,
                Tooltip = "Explorer — hosts a TreeView via ContentControl",
            };

            var calendar = new AntdUI.OutlookPanel("Calendar", "CalendarOutlined");
            string[] events = { "Standup", "1:1 with Alex", "Design review", "Lunch", "Deploy window", "Retro" };
            foreach (var ev in events) calendar.Items.Add(new AntdUI.OutlookItem(ev, "ClockCircleOutlined", (s, e) => Append("OutlookItem clicked: " + ev)));

            var contacts = new AntdUI.OutlookPanel("Contacts", "UserOutlined");
            string[] people = { "Alice", "Bob", "Charlie", "Dana", "Eve", "Frank", "Gina", "Henry" };
            foreach (var p in people) contacts.Items.Add(new AntdUI.OutlookItem(p, "UserOutlined", (s, e) => Append("OutlookItem clicked: " + p)));

            var tasks = new AntdUI.OutlookPanel("Tasks", "CheckSquareOutlined");
            for (int i = 1; i <= 12; i++)
            {
                int n = i;
                tasks.Items.Add(new AntdUI.OutlookItem("Task " + n, "FileOutlined", (s, e) => Append("OutlookItem clicked: Task " + n)));
            }

            var footerSettings = new AntdUI.OutlookPanel("Settings", "SettingOutlined") { InFooter = true };
            footerSettings.Items.Add(new AntdUI.OutlookItem("Preferences", "ControlOutlined", (s, e) => Append("Settings > Preferences")));
            footerSettings.Items.Add(new AntdUI.OutlookItem("Accounts", "TeamOutlined", (s, e) => Append("Settings > Accounts")));

            var footerNotes = new AntdUI.OutlookPanel("Notes", "FileTextOutlined") { InFooter = true };
            footerNotes.Items.Add(new AntdUI.OutlookItem("Note 1", "FileOutlined", (s, e) => Append("Notes > Note 1")));

            bar.Panels.Add(mail);
            bar.Panels.Add(explorerPanel);
            bar.Panels.Add(calendar);
            bar.Panels.Add(contacts);
            bar.Panels.Add(tasks);
            bar.Panels.Add(footerSettings);
            bar.Panels.Add(footerNotes);
            bar.SelectedIndex = 0;

            bar.SelectedPanelChanged += (s, e) => Append("SelectedPanelChanged: index=" + e.Value);
            bar.ExpandedChanged += (s, e) => Append("ExpandedChanged: " + bar.Expanded);

            // Controls on the right to exercise toggles
            var rightPanel = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent, Padding = new Padding(12) };

            var toolbar = new Panel { Dock = DockStyle.Top, Height = 44, BackColor = Color.Transparent };
            var btnSplitter = new AntdUI.Button { Dock = DockStyle.Left, Text = "Toggle Splitter", Width = 150 };
            btnSplitter.Click += (s, e) => { bar.HeaderSplitResize = !bar.HeaderSplitResize; Append("HeaderSplitResize = " + bar.HeaderSplitResize); };
            toolbar.Controls.Add(btnSplitter);

            var btnPanelsVisible = new AntdUI.Button { Dock = DockStyle.Left, Text = "PanelsVisible=2", Width = 150 };
            btnPanelsVisible.Click += (s, e) => { bar.PanelsVisible = 2; Append("PanelsVisible=2 → " + bar.PanelsVisible); };
            toolbar.Controls.Add(btnPanelsVisible);

            rightPanel.Controls.Add(toolbar);

            Controls.Add(rightPanel);
            Controls.Add(bar);
        }

        void Append(string s) => form.Append(s);
    }
}
