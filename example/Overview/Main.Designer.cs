// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
// THE AntdUI PROJECT IS AN WINFORM LIBRARY LICENSED UNDER THE GPL-3.0 License.
// LICENSED UNDER THE GPL License, VERSION 3.0 (THE "License")
// YOU MAY NOT USE THIS FILE EXCEPT IN COMPLIANCE WITH THE License.
// UNLESS REQUIRED BY APPLICABLE LAW OR AGREED TO IN WRITING, SOFTWARE
// DISTRIBUTED UNDER THE LICENSE IS DISTRIBUTED ON AN "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED.
// SEE THE LICENSE FOR THE SPECIFIC LANGUAGE GOVERNING PERMISSIONS AND
// LIMITATIONS UNDER THE License.
// GITEE: https://gitee.com/antdui/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

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
            divider2 = new AntdUI.Divider();
            flowPanel = new FlowLayoutPanel();
            windowBar = new AntdUI.WindowBar();
            colorPicker1 = new AntdUI.ColorPicker();
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
            btn_mode.Location = new Point(1094, 0);
            btn_mode.Margins = 0;
            btn_mode.Name = "btn_mode";
            btn_mode.Radius = 0;
            btn_mode.Size = new Size(50, 40);
            btn_mode.TabIndex = 6;
            btn_mode.Click += btn_mode_Click;
            // 
            // divider2
            // 
            divider2.Dock = DockStyle.Top;
            divider2.Location = new Point(0, 40);
            divider2.Name = "divider2";
            divider2.Size = new Size(1300, 4);
            divider2.TabIndex = 0;
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
            windowBar.Controls.Add(colorPicker1);
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
            // colorPicker1
            // 
            colorPicker1.Dock = DockStyle.Right;
            colorPicker1.Location = new Point(1054, 0);
            colorPicker1.Name = "colorPicker1";
            colorPicker1.Padding = new Padding(5);
            colorPicker1.Size = new Size(40, 40);
            colorPicker1.TabIndex = 8;
            colorPicker1.Value = Color.FromArgb(22, 119, 255);
            // 
            // Main
            // 
            BackColor = Color.White;
            ClientSize = new Size(1300, 720);
            Controls.Add(flowPanel);
            Controls.Add(divider2);
            Controls.Add(windowBar);
            Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            ForeColor = Color.Black;
            Name = "Main";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Overview";
            windowBar.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private AntdUI.Button btn_mode;
        private AntdUI.Divider divider2;
        private FlowLayoutPanel flowPanel;
        private AntdUI.Button btn_back;
        private AntdUI.WindowBar windowBar;
        private AntdUI.ColorPicker colorPicker1;
    }
}