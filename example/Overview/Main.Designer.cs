﻿// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
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

using System.Drawing;
using System.Windows.Forms;

namespace Overview
{
    partial class Main
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

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btn_back = new AntdUI.Button();
            btn_mode = new AntdUI.Button();
            virtualPanel = new AntdUI.VirtualPanel();
            windowBar = new AntdUI.WindowBar();
            txt_search = new AntdUI.Input();
            colorTheme = new AntdUI.ColorPicker();
            btn_setting = new AntdUI.Button();
            windowBar.SuspendLayout();
            SuspendLayout();
            // 
            // btn_back
            // 
            btn_back.Dock = DockStyle.Left;
            btn_back.Ghost = true;
            btn_back.IconSvg = Properties.Resources.app_back;
            btn_back.Location = new Point(0, 0);
            btn_back.Name = "btn_back";
            btn_back.Size = new Size(90, 40);
            btn_back.TabIndex = 7;
            btn_back.Text = "返回";
            btn_back.Visible = false;
            btn_back.Click += btn_back_Click;
            // 
            // btn_mode
            // 
            btn_mode.Dock = DockStyle.Right;
            btn_mode.Font = new Font("Microsoft YaHei UI", 18F);
            btn_mode.Ghost = true;
            btn_mode.IconSvg = Properties.Resources.app_light;
            btn_mode.Location = new Point(1056, 0);
            btn_mode.Name = "btn_mode";
            btn_mode.Radius = 0;
            btn_mode.Size = new Size(50, 40);
            btn_mode.TabIndex = 6;
            btn_mode.WaveSize = 0;
            btn_mode.Click += btn_mode_Click;
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
            windowBar.Controls.Add(txt_search);
            windowBar.Controls.Add(colorTheme);
            windowBar.Controls.Add(btn_mode);
            windowBar.Controls.Add(btn_back);
            windowBar.Controls.Add(btn_setting);
            windowBar.DividerMargin = 3;
            windowBar.DividerShow = true;
            windowBar.Dock = DockStyle.Top;
            windowBar.Icon = Properties.Resources.logo;
            windowBar.Location = new Point(0, 0);
            windowBar.Name = "windowBar";
            windowBar.Size = new Size(1300, 40);
            windowBar.SubText = "Overview";
            windowBar.TabIndex = 0;
            windowBar.Text = "Ant Design 5.0";
            // 
            // txt_search
            // 
            txt_search.Dock = DockStyle.Right;
            txt_search.Location = new Point(846, 0);
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
            colorTheme.Location = new Point(1016, 0);
            colorTheme.Name = "colorTheme";
            colorTheme.Padding = new Padding(5);
            colorTheme.Size = new Size(40, 40);
            colorTheme.TabIndex = 8;
            colorTheme.Value = Color.FromArgb(22, 119, 255);
            // 
            // btn_setting
            // 
            btn_setting.Dock = DockStyle.Right;
            btn_setting.Ghost = true;
            btn_setting.IconSvg = Properties.Resources.setting;
            btn_setting.Location = new Point(1106, 0);
            btn_setting.Name = "btn_setting";
            btn_setting.Radius = 0;
            btn_setting.Size = new Size(50, 40);
            btn_setting.TabIndex = 7;
            btn_setting.WaveSize = 0;
            btn_setting.Click += btn_setting_Click;
            // 
            // Main
            // 
            BackColor = Color.White;
            ClientSize = new Size(1300, 720);
            Controls.Add(virtualPanel);
            Controls.Add(windowBar);
            Font = new Font("Microsoft YaHei UI", 12F);
            ForeColor = Color.Black;
            MinimumSize = new Size(660, 400);
            Name = "Main";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Overview";
            windowBar.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private AntdUI.Button btn_mode;
        private AntdUI.VirtualPanel virtualPanel;
        private AntdUI.Button btn_back;
        private AntdUI.WindowBar windowBar;
        private AntdUI.ColorPicker colorTheme;
        private AntdUI.Input txt_search;
        private AntdUI.Button btn_setting;
    }
}