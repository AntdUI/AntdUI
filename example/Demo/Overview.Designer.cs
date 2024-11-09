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
// GITEE: https://gitee.com/antdui/AntdUI
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

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Overview));
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
            resources.ApplyResources(btn_mode, "btn_mode");
            btn_mode.Ghost = true;
            btn_mode.IconSvg = "SunOutlined";
            btn_mode.Name = "btn_mode";
            btn_mode.Radius = 0;
            btn_mode.ToggleIconSvg = "MoonOutlined";
            btn_mode.WaveSize = 0;
            btn_mode.Click += btn_mode_Click;
            // 
            // btn_global
            // 
            resources.ApplyResources(btn_global, "btn_global");
            btn_global.DropDownRadius = 6;
            btn_global.Ghost = true;
            btn_global.IconSvg = "GlobalOutlined";
            btn_global.Name = "btn_global";
            btn_global.Placement = AntdUI.TAlignFrom.BR;
            btn_global.Radius = 0;
            btn_global.WaveSize = 0;
            btn_global.SelectedValueChanged += btn_global_Changed;
            // 
            // btn_setting
            // 
            resources.ApplyResources(btn_setting, "btn_setting");
            btn_setting.Ghost = true;
            btn_setting.IconSvg = "SettingOutlined";
            btn_setting.Name = "btn_setting";
            btn_setting.Radius = 0;
            btn_setting.WaveSize = 0;
            btn_setting.Click += btn_setting_Click;
            // 
            // virtualPanel
            // 
            resources.ApplyResources(virtualPanel, "virtualPanel");
            virtualPanel.JustifyContent = AntdUI.TJustifyContent.SpaceEvenly;
            virtualPanel.Name = "virtualPanel";
            virtualPanel.Shadow = 20;
            virtualPanel.ShadowOpacityAnimation = true;
            virtualPanel.Waterfall = true;
            virtualPanel.ItemClick += ItemClick;
            // 
            // windowBar
            // 
            resources.ApplyResources(windowBar, "windowBar");
            windowBar.Controls.Add(txt_search);
            windowBar.Controls.Add(colorTheme);
            windowBar.Controls.Add(btn_mode);
            windowBar.Controls.Add(btn_global);
            windowBar.Controls.Add(btn_setting);
            windowBar.DividerMargin = 3;
            windowBar.DividerShow = true;
            windowBar.Icon = Properties.Resources.logo;
            windowBar.Name = "windowBar";
            windowBar.ShowButton = true;
            windowBar.ShowIcon = true;
            windowBar.BackClick += btn_back_Click;
            // 
            // txt_search
            // 
            resources.ApplyResources(txt_search, "txt_search");
            txt_search.Name = "txt_search";
            txt_search.PrefixSvg = "SearchOutlined";
            txt_search.PrefixClick += txt_search_PrefixClick;
            txt_search.TextChanged += txt_search_TextChanged;
            // 
            // colorTheme
            // 
            resources.ApplyResources(colorTheme, "colorTheme");
            colorTheme.Name = "colorTheme";
            colorTheme.Value = Color.FromArgb(22, 119, 255);
            // 
            // Overview
            // 
            resources.ApplyResources(this, "$this");
            BackColor = Color.White;
            Controls.Add(virtualPanel);
            Controls.Add(windowBar);
            ForeColor = Color.Black;
            Name = "Overview";
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