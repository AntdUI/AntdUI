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
// GITEE: https://gitee.com/AntdUI/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

using System.Drawing;
using System.Windows.Forms;

namespace Demo
{
    partial class Overview
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            btn_mode = new AntdUI.Button();
            btn_global = new AntdUI.Dropdown();
            btn_setting = new AntdUI.Button();
            virtualPanel = new AntdUI.VirtualPanel();
            windowBar = new AntdUI.PageHeader();
            txt_search = new AntdUI.Input();
            colorTheme = new AntdUI.ColorPicker();
            windowBar.SuspendLayout();
            SuspendLayout();
            // 
            // btn_mode
            // 
            btn_mode.Dock = DockStyle.Right;
            btn_mode.Ghost = true;
            btn_mode.IconSvg = "SunOutlined";
            btn_mode.Location = new Point(1006, 0);
            btn_mode.Name = "btn_mode";
            btn_mode.Radius = 0;
            btn_mode.Size = new Size(50, 40);
            btn_mode.TabIndex = 6;
            btn_mode.ToggleIconSvg = "MoonOutlined";
            btn_mode.WaveSize = 0;
            btn_mode.Click += btn_mode_Click;
            // 
            // btn_global
            // 
            btn_global.Dock = DockStyle.Right;
            btn_global.DropDownRadius = 6;
            btn_global.Ghost = true;
            btn_global.IconSvg = "GlobalOutlined";
            btn_global.Location = new Point(1056, 0);
            btn_global.Name = "btn_global";
            btn_global.Placement = AntdUI.TAlignFrom.BR;
            btn_global.Radius = 0;
            btn_global.Size = new Size(50, 40);
            btn_global.TabIndex = 7;
            btn_global.WaveSize = 0;
            btn_global.SelectedValueChanged += btn_global_Changed;
            // 
            // btn_setting
            // 
            btn_setting.Dock = DockStyle.Right;
            btn_setting.Ghost = true;
            btn_setting.IconSvg = "SettingOutlined";
            btn_setting.Location = new Point(1106, 0);
            btn_setting.Name = "btn_setting";
            btn_setting.Radius = 0;
            btn_setting.Size = new Size(50, 40);
            btn_setting.TabIndex = 8;
            btn_setting.WaveSize = 0;
            btn_setting.Click += btn_setting_Click;
            // 
            // virtualPanel
            // 
            virtualPanel.Dock = DockStyle.Fill;
            virtualPanel.JustifyContent = AntdUI.TJustifyContent.SpaceEvenly;
            virtualPanel.Location = new Point(0, 40);
            virtualPanel.Name = "virtualPanel";
            virtualPanel.Shadow = 20;
            virtualPanel.ShadowOpacityAnimation = true;
            virtualPanel.Size = new Size(1300, 680);
            virtualPanel.TabIndex = 2;
            virtualPanel.Waterfall = true;
            virtualPanel.ItemClick += ItemClick;
            // 
            // windowBar
            // 
            windowBar.BackgroundImageLayout = ImageLayout.Stretch;
            windowBar.Controls.Add(txt_search);
            windowBar.Controls.Add(colorTheme);
            windowBar.Controls.Add(btn_mode);
            windowBar.Controls.Add(btn_global);
            windowBar.Controls.Add(btn_setting);
            windowBar.DividerMargin = 3;
            windowBar.DividerShow = true;
            windowBar.Dock = DockStyle.Top;
            windowBar.Icon = Properties.Resources.logo;
            windowBar.Location = new Point(0, 0);
            windowBar.Name = "windowBar";
            windowBar.ShowButton = true;
            windowBar.ShowIcon = true;
            windowBar.Size = new Size(1300, 40);
            windowBar.SubText = "Overview";
            windowBar.TabIndex = 0;
            windowBar.Text = "AntdUI";
            windowBar.BackClick += btn_back_Click;
            // 
            // txt_search
            // 
            txt_search.Dock = DockStyle.Right;
            txt_search.LocalizationPlaceholderText = "Overview.{id}";
            txt_search.Location = new Point(796, 0);
            txt_search.Name = "txt_search";
            txt_search.Padding = new Padding(0, 2, 0, 2);
            txt_search.PlaceholderText = "输入关键字搜索...";
            txt_search.PrefixSvg = "SearchOutlined";
            txt_search.Size = new Size(170, 40);
            txt_search.TabIndex = 9;
            txt_search.PrefixClick += txt_search_PrefixClick;
            txt_search.TextChanged += txt_search_TextChanged;
            // 
            // colorTheme
            // 
            colorTheme.Dock = DockStyle.Right;
            colorTheme.Location = new Point(966, 0);
            colorTheme.Name = "colorTheme";
            colorTheme.Padding = new Padding(5);
            colorTheme.Size = new Size(40, 40);
            colorTheme.TabIndex = 8;
            colorTheme.Value = Color.FromArgb(22, 119, 255);
            // 
            // Overview
            // 
            BackColor = Color.White;
            ClientSize = new Size(1300, 720);
            Controls.Add(virtualPanel);
            Controls.Add(windowBar);
            Font = new Font("Microsoft YaHei UI", 12F);
            ForeColor = Color.Black;
            MinimumSize = new Size(660, 400);
            Name = "Overview";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "AntdUI Overview";
            windowBar.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.Button btn_mode;
        private AntdUI.Dropdown btn_global;
        private AntdUI.Button btn_setting;
        private AntdUI.VirtualPanel virtualPanel;
        private AntdUI.PageHeader windowBar;
        private AntdUI.ColorPicker colorTheme;
        private AntdUI.Input txt_search;
    }
}