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
// GITCODE: https://gitcode.com/AntdUI/AntdUI
// GITEE: https://gitee.com/AntdUI/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

using System.Drawing;
using System.Windows.Forms;

namespace Demo.Controls
{
    partial class Drawer
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
            header1 = new AntdUI.PageHeader();
            panel1 = new System.Windows.Forms.Panel();
            panel2 = new System.Windows.Forms.Panel();
            panel3 = new System.Windows.Forms.Panel();
            radio2 = new AntdUI.Radio();
            radio4 = new AntdUI.Radio();
            radio3 = new AntdUI.Radio();
            radio1 = new AntdUI.Radio();
            button1 = new AntdUI.Button();
            divider1 = new AntdUI.Divider();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Description = "屏幕边缘滑出的浮层面板。";
            header1.Dock = DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F);
            header1.LocalizationDescription = "Drawer.Description";
            header1.LocalizationText = "Drawer.Text";
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new Padding(0, 0, 0, 10);
            header1.Size = new Size(543, 74);
            header1.TabIndex = 0;
            header1.Text = "Drawer 抽屉";
            header1.UseTitleFont = true;
            // 
            // panel1
            // 
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(divider1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 74);
            panel1.Name = "panel1";
            panel1.Size = new Size(543, 383);
            panel1.TabIndex = 6;
            // 
            // panel2
            // 
            panel2.Controls.Add(panel3);
            panel2.Controls.Add(button1);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 28);
            panel2.Name = "panel2";
            panel2.Size = new Size(543, 71);
            panel2.TabIndex = 2;
            // 
            // panel3
            // 
            panel3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel3.Controls.Add(radio2);
            panel3.Controls.Add(radio4);
            panel3.Controls.Add(radio3);
            panel3.Controls.Add(radio1);
            panel3.Location = new Point(109, 10);
            panel3.Name = "panel3";
            panel3.Size = new Size(419, 46);
            panel3.TabIndex = 1;
            // 
            // radio2
            // 
            radio2.AutoSizeMode = AntdUI.TAutoSize.Width;
            radio2.Checked = true;
            radio2.Dock = DockStyle.Left;
            radio2.Location = new Point(258, 0);
            radio2.Name = "radio2";
            radio2.Size = new Size(86, 46);
            radio2.TabIndex = 1;
            radio2.Text = "right";
            // 
            // radio4
            // 
            radio4.AutoSizeMode = AntdUI.TAutoSize.Width;
            radio4.Dock = DockStyle.Left;
            radio4.Location = new Point(184, 0);
            radio4.Name = "radio4";
            radio4.Size = new Size(74, 46);
            radio4.TabIndex = 3;
            radio4.Text = "left";
            // 
            // radio3
            // 
            radio3.AutoSizeMode = AntdUI.TAutoSize.Width;
            radio3.Dock = DockStyle.Left;
            radio3.Location = new Point(76, 0);
            radio3.Name = "radio3";
            radio3.Size = new Size(108, 46);
            radio3.TabIndex = 2;
            radio3.Text = "bottom";
            // 
            // radio1
            // 
            radio1.AutoSizeMode = AntdUI.TAutoSize.Width;
            radio1.Dock = DockStyle.Left;
            radio1.Location = new Point(0, 0);
            radio1.Name = "radio1";
            radio1.Size = new Size(76, 46);
            radio1.TabIndex = 0;
            radio1.Text = "top";
            // 
            // button1
            // 
            button1.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button1.Location = new Point(18, 10);
            button1.Name = "button1";
            button1.Size = new Size(77, 47);
            button1.TabIndex = 0;
            button1.Text = "Open";
            button1.Type = AntdUI.TTypeMini.Primary;
            button1.Click += button1_Click;
            // 
            // divider1
            // 
            divider1.Dock = DockStyle.Top;
            divider1.Font = new Font("Microsoft YaHei UI", 10F);
            divider1.LocalizationText = "Drawer.{id}";
            divider1.Location = new Point(0, 0);
            divider1.Margin = new Padding(10);
            divider1.Name = "divider1";
            divider1.Orientation = AntdUI.TOrientation.Left;
            divider1.Size = new Size(543, 28);
            divider1.TabIndex = 0;
            divider1.Text = "基本";
            // 
            // Drawer
            // 
            Controls.Add(panel1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 12F);
            Name = "Drawer";
            Size = new Size(543, 457);
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.PageHeader header1;
        private System.Windows.Forms.Panel panel1;
        private AntdUI.Divider divider1;
        private System.Windows.Forms.Panel panel2;
        private AntdUI.Button button1;
        private System.Windows.Forms.Panel panel3;
        private AntdUI.Radio radio2;
        private AntdUI.Radio radio4;
        private AntdUI.Radio radio3;
        private AntdUI.Radio radio1;
    }
}