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
            divider_top = new AntdUI.Divider();
            flowPanel = new AntdUI.FlowPanel();
            windowBar = new AntdUI.WindowBar();
            txt_search = new AntdUI.Input();
            colorTheme = new AntdUI.ColorPicker();
            windowBar.SuspendLayout();
            SuspendLayout();
            // 
            // btn_back
            // 
            btn_back.Dock = DockStyle.Left;
            btn_back.Ghost = true;
            btn_back.ImageSvg = Properties.Resources.app_back;
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
            btn_mode.Font = new Font("Microsoft YaHei UI", 18F, FontStyle.Regular, GraphicsUnit.Point);
            btn_mode.Ghost = true;
            btn_mode.ImageSvg = Properties.Resources.app_light;
            btn_mode.Location = new Point(1106, 0);
            btn_mode.Margins = 0;
            btn_mode.Name = "btn_mode";
            btn_mode.Radius = 0;
            btn_mode.Size = new Size(50, 40);
            btn_mode.TabIndex = 6;
            btn_mode.Click += btn_mode_Click;
            // 
            // divider_top
            // 
            divider_top.Dock = DockStyle.Top;
            divider_top.Location = new Point(0, 40);
            divider_top.Name = "divider_top";
            divider_top.Size = new Size(1300, 4);
            divider_top.TabIndex = 0;
            // 
            // flowPanel
            // 
            flowPanel.AutoScroll = true;
            flowPanel.Dock = DockStyle.Fill;
            flowPanel.Location = new Point(0, 44);
            flowPanel.Name = "flowPanel";
            flowPanel.Size = new Size(1300, 676);
            flowPanel.TabIndex = 2;
            // 
            // windowBar
            // 
            windowBar.Controls.Add(txt_search);
            windowBar.Controls.Add(colorTheme);
            windowBar.Controls.Add(btn_mode);
            windowBar.Controls.Add(btn_back);
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
            txt_search.Location = new Point(896, 0);
            txt_search.Name = "txt_search";
            txt_search.Padding = new Padding(0, 4, 0, 4);
            txt_search.PlaceholderText = "输入关键字搜索...";
            txt_search.PrefixSvg = Properties.Resources.icon_search;
            txt_search.Size = new Size(170, 40);
            txt_search.TabIndex = 9;
            txt_search.PrefixClick += txt_search_PrefixClick;
            txt_search.TextChanged += txt_search_TextChanged;
            // 
            // colorTheme
            // 
            colorTheme.Dock = DockStyle.Right;
            colorTheme.Location = new Point(1066, 0);
            colorTheme.Name = "colorTheme";
            colorTheme.Padding = new Padding(5);
            colorTheme.Size = new Size(40, 40);
            colorTheme.TabIndex = 8;
            colorTheme.Value = Color.FromArgb(22, 119, 255);
            // 
            // Main
            // 
            BackColor = Color.White;
            ClientSize = new Size(1300, 720);
            Controls.Add(flowPanel);
            Controls.Add(divider_top);
            Controls.Add(windowBar);
            Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
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
        private AntdUI.Divider divider_top;
        private AntdUI.FlowPanel flowPanel;
        private AntdUI.Button btn_back;
        private AntdUI.WindowBar windowBar;
        private AntdUI.ColorPicker colorTheme;
        private AntdUI.Input txt_search;
    }
}