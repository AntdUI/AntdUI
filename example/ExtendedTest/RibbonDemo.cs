// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Drawing;
using System.Windows.Forms;

namespace ExtendedTest
{
    public partial class RibbonDemo : UserControl
    {
        MainForm form;
        public RibbonDemo(MainForm main)
        {
            form = main;
            InitializeComponent();

            var home = new AntdUI.RibbonTabPage("Home",
                new AntdUI.RibbonItemGroup("Clipboard",
                    new AntdUI.RibbonItem("Paste", "FileAddOutlined", OnItem) { Size = AntdUI.RibbonItemSize.Large },
                    new AntdUI.RibbonItem("Cut", "ScissorOutlined", OnItem) { Size = AntdUI.RibbonItemSize.Small },
                    new AntdUI.RibbonItem("Copy", "CopyOutlined", OnItem) { Size = AntdUI.RibbonItemSize.Small },
                    new AntdUI.RibbonItem("Format", "HighlightOutlined", OnItem) { Size = AntdUI.RibbonItemSize.Small }
                ),
                new AntdUI.RibbonItemGroup("Font",
                    new AntdUI.RibbonItem("Bold", "BoldOutlined", OnItem) { Size = AntdUI.RibbonItemSize.Small, Toggle = true },
                    new AntdUI.RibbonItem("Italic", "ItalicOutlined", OnItem) { Size = AntdUI.RibbonItemSize.Small, Toggle = true },
                    new AntdUI.RibbonItem("Underline", "UnderlineOutlined", OnItem) { Size = AntdUI.RibbonItemSize.Small, Toggle = true },
                    new AntdUI.RibbonItem("Strike", "StrikethroughOutlined", OnItem) { Size = AntdUI.RibbonItemSize.Small, Toggle = true }
                ),
                new AntdUI.RibbonItemGroup("Insert",
                    new AntdUI.RibbonItem("Picture", "PictureOutlined", OnItem),
                    new AntdUI.RibbonItem("Table", "TableOutlined", OnItem),
                    new AntdUI.RibbonItem("Chart", "BarChartOutlined", OnItem),
                    new AntdUI.RibbonItem("Link", "LinkOutlined", OnItem) { Size = AntdUI.RibbonItemSize.Small }
                )
            );

            var view = new AntdUI.RibbonTabPage("View",
                new AntdUI.RibbonItemGroup("Show",
                    new AntdUI.RibbonItem("Grid", "BorderOutlined", OnItem) { Toggle = true, Checked = true },
                    new AntdUI.RibbonItem("Ruler", "ColumnWidthOutlined", OnItem) { Toggle = true },
                    new AntdUI.RibbonItem("Guides", "AppstoreOutlined", OnItem) { Toggle = true }
                ),
                new AntdUI.RibbonItemGroup("Zoom",
                    new AntdUI.RibbonItem("Zoom In", "ZoomInOutlined", OnItem),
                    new AntdUI.RibbonItem("Zoom Out", "ZoomOutOutlined", OnItem),
                    new AntdUI.RibbonItem("100%", "OneToOneOutlined", OnItem) { Size = AntdUI.RibbonItemSize.Small },
                    new AntdUI.RibbonItem("Fit", "FullscreenOutlined", OnItem) { Size = AntdUI.RibbonItemSize.Small }
                )
            );

            var help = new AntdUI.RibbonTabPage("Help",
                new AntdUI.RibbonItemGroup("About",
                    new AntdUI.RibbonItem("Docs", "FileTextOutlined", OnItem),
                    new AntdUI.RibbonItem("Info", "InfoCircleOutlined", OnItem)
                )
            );

            ribbon.TabPages.Add(home);
            ribbon.TabPages.Add(view);
            ribbon.TabPages.Add(help);
            ribbon.SelectedIndex = 0;
            ribbon.ItemClick += (s, e) => Append("Ribbon: " + e.Item.Text + (e.Item.Toggle ? " (checked=" + e.Item.Checked + ")" : ""));

            var toolbar = new AntdUI.Panel { Dock = DockStyle.Top, Height = 40, BackColor = Color.Transparent, Padding = new Padding(8) };
            var btnCollapse = new AntdUI.Button { Dock = DockStyle.Left, Text = "Toggle Collapse", Width = 160 };
            btnCollapse.Click += (s, e) =>
            {
                ribbon.Collapsed = !ribbon.Collapsed;
                Append("Ribbon.Collapsed = " + ribbon.Collapsed + " (double-click the tab strip also toggles)");
            };
            toolbar.Controls.Add(btnCollapse);

            Controls.Add(toolbar);
            Controls.Add(ribbon);
        }

        void OnItem(object? sender, EventArgs e)
        {
        }

        void Append(string s) => form.Append(s);
    }
}
