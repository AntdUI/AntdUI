// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ExtendedTest
{
    public partial class DockDemo : UserControl
    {
        AntdUI.DockContent? explorerContent;
        AntdUI.DockContent? propertiesContent;
        AntdUI.DockContent? outputContent;
        AntdUI.DockContent? editorContent;

        MainForm form;
        public DockDemo(MainForm main)
        {
            form = main;
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            RestoreAll();
            base.OnLoad(e);
        }

        private void btnFloat_Click(object sender, EventArgs e)
        {
            var active = dock.ActiveContent;
            if (active != null && active.CanFloat) dock.FloatContent(active);
            else Append("No active content or cannot float.");
        }

        private void btnAutoHide_Click(object sender, EventArgs e)
        {
            var active = dock.ActiveContent;
            if (active != null && active.CanAutoHide) dock.AutoHideContent(active);
            else Append("No active content or cannot auto-hide.");
        }

        private void btnRestore_Click(object sender, EventArgs e) => RestoreAll();

        private void btnSave_Click(object sender, EventArgs e)
        {
            var xml = AntdUI.DockPersistor.Save(dock);
            File.WriteAllText(LayoutPath, xml);
            Append("Layout saved: " + LayoutPath);
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (!File.Exists(LayoutPath)) { Append("No saved layout."); return; }
            var xml = File.ReadAllText(LayoutPath);
            AntdUI.DockPersistor.Load(dock, xml, ResolveContent);
            Append("Layout loaded from " + LayoutPath);
        }

        static string LayoutPath => Path.Combine(Path.GetTempPath(), "antdui_docklayout.xml");

        void RestoreAll()
        {
            var panes = new System.Collections.Generic.List<AntdUI.DockPane>(dock.Panes);
            foreach (var p in panes)
            {
                var contents = new System.Collections.Generic.List<AntdUI.IDockContent>(p.Contents);
                foreach (var c in contents) dock.RemoveContent(c);
            }

            explorerContent = new AntdUI.DockContent(BuildTextSurface("Explorer", "📁 src/\n  AntdUI/\n    Controls/\n    Forms/\n📁 example/\n📁 test/"), "explorer", "Explorer", "FolderOutlined");
            propertiesContent = new AntdUI.DockContent(BuildTextSurface("Properties", "Name: DockPanel\nType: IControl\nNamespace: AntdUI\nDpi: auto\nTheme: live"), "properties", "Properties", "UnorderedListOutlined");
            outputContent = new AntdUI.DockContent(BuildTextSurface("Output", "[info] built in 5.0s\n[info] 0 errors, 0 warnings\n[info] launched ExtendedTest\n[dock] ready"), "output", "Output", "CodeOutlined");
            editorContent = new AntdUI.DockContent(BuildTextSurface("Editor — Welcome.cs", "// Drag a tab header to move it.\n// Click the pin icon to auto-hide.\n// Close with × on the tab.\n\nnamespace Demo\n{\n    public class Welcome { }\n}"), "editor", "Welcome.cs", "FileTextOutlined");

            explorerContent.CanClose = true;
            propertiesContent.CanClose = true;
            outputContent.CanClose = true;
            editorContent.CanClose = true;

            dock.BeginBatch();
            try
            {
                dock.AddContent(explorerContent, AntdUI.DockPosition.Left, 0.20f);
                dock.AddContent(propertiesContent, AntdUI.DockPosition.Right, 0.22f);
                dock.AddContent(outputContent, AntdUI.DockPosition.Bottom, 0.25f);
                dock.AddContent(editorContent, AntdUI.DockPosition.Fill);
            }
            finally { dock.EndBatch(); }
            Append("Default layout restored.");
        }

        AntdUI.IDockContent? ResolveContent(string persistId)
        {
            switch (persistId)
            {
                case "explorer": return explorerContent ??= new AntdUI.DockContent(BuildTextSurface("Explorer", ""), "explorer", "Explorer", "FolderOutlined");
                case "properties": return propertiesContent ??= new AntdUI.DockContent(BuildTextSurface("Properties", ""), "properties", "Properties", "UnorderedListOutlined");
                case "output": return outputContent ??= new AntdUI.DockContent(BuildTextSurface("Output", ""), "output", "Output", "CodeOutlined");
                case "editor": return editorContent ??= new AntdUI.DockContent(BuildTextSurface("Editor", ""), "editor", "Welcome.cs", "FileTextOutlined");
            }
            return null;
        }

        static Control BuildTextSurface(string header, string body)
        {
            var p = new UserControl { Dock = DockStyle.Fill, Padding = new Padding(12) };
            var title = new AntdUI.Label
            {
                Dock = DockStyle.Top,
                Text = header,
                Height = 28,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            var text = new AntdUI.Input
            {
                Dock = DockStyle.Fill,
                BorderWidth = 0,
                WaveSize = 0,
                Radius = 0,
                Multiline = true,
                Text = body
            };
            p.Controls.Add(text);
            p.Controls.Add(title);
            return p;
        }

        void Append(string s) => form.Append(s);
    }
}
