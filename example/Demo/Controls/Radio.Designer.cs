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
    partial class Radio
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
            panel4 = new System.Windows.Forms.Panel();
            panel5 = new System.Windows.Forms.Panel();
            button2 = new AntdUI.Button();
            button1 = new AntdUI.Button();
            radio9 = new AntdUI.Radio();
            divider3 = new AntdUI.Divider();
            panel3 = new System.Windows.Forms.Panel();
            radio8 = new AntdUI.Radio();
            radio7 = new AntdUI.Radio();
            radio6 = new AntdUI.Radio();
            radio5 = new AntdUI.Radio();
            divider2 = new AntdUI.Divider();
            panel2 = new System.Windows.Forms.Panel();
            radio4 = new AntdUI.Radio();
            radio3 = new AntdUI.Radio();
            radio2 = new AntdUI.Radio();
            radio1 = new AntdUI.Radio();
            divider1 = new AntdUI.Divider();
            panel1.SuspendLayout();
            panel4.SuspendLayout();
            panel5.SuspendLayout();
            panel3.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Description = "单选框。";
            header1.Dock = DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F);
            header1.LocalizationDescription = "Radio.Description";
            header1.LocalizationText = "Radio.Text";
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new Padding(0, 0, 0, 10);
            header1.Size = new Size(650, 74);
            header1.TabIndex = 0;
            header1.Text = "Radio 单选框";
            header1.UseTitleFont = true;
            // 
            // panel1
            // 
            panel1.Controls.Add(panel4);
            panel1.Controls.Add(divider3);
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(divider2);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(divider1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 74);
            panel1.Name = "panel1";
            panel1.Size = new Size(650, 323);
            panel1.TabIndex = 6;
            // 
            // panel4
            // 
            panel4.Controls.Add(panel5);
            panel4.Controls.Add(radio9);
            panel4.Dock = DockStyle.Top;
            panel4.Location = new Point(0, 184);
            panel4.Name = "panel4";
            panel4.Size = new Size(650, 96);
            panel4.TabIndex = 3;
            // 
            // panel5
            // 
            panel5.Controls.Add(button2);
            panel5.Controls.Add(button1);
            panel5.Font = new Font("Microsoft YaHei UI", 9F);
            panel5.Location = new Point(0, 46);
            panel5.Name = "panel5";
            panel5.Size = new Size(204, 38);
            panel5.TabIndex = 2;
            // 
            // button2
            // 
            button2.AutoSizeMode = AntdUI.TAutoSize.Width;
            button2.Dock = DockStyle.Left;
            button2.Location = new Point(83, 0);
            button2.Name = "button2";
            button2.Size = new Size(76, 38);
            button2.TabIndex = 1;
            button2.Text = "Disable";
            button2.Type = AntdUI.TTypeMini.Primary;
            button2.Click += button2_Click;
            // 
            // button1
            // 
            button1.AutoSizeMode = AntdUI.TAutoSize.Width;
            button1.Dock = DockStyle.Left;
            button1.Location = new Point(0, 0);
            button1.Name = "button1";
            button1.Size = new Size(83, 38);
            button1.TabIndex = 0;
            button1.Text = "Uncheck";
            button1.Type = AntdUI.TTypeMini.Primary;
            button1.Click += button1_Click;
            // 
            // radio9
            // 
            radio9.AutoSizeMode = AntdUI.TAutoSize.Auto;
            radio9.Checked = true;
            radio9.Location = new Point(0, 0);
            radio9.Name = "radio9";
            radio9.Size = new Size(187, 43);
            radio9.TabIndex = 0;
            radio9.Text = "Checked-Enabled";
            radio9.CheckedChanged += radio9_CheckedChanged;
            radio9.EnabledChanged += radio9_EnabledChanged;
            // 
            // divider3
            // 
            divider3.Dock = DockStyle.Top;
            divider3.Font = new Font("Microsoft YaHei UI", 10F);
            divider3.LocalizationText = "Radio.{id}";
            divider3.Location = new Point(0, 156);
            divider3.Name = "divider3";
            divider3.Orientation = AntdUI.TOrientation.Left;
            divider3.Size = new Size(650, 28);
            divider3.TabIndex = 0;
            divider3.Text = "联动";
            // 
            // panel3
            // 
            panel3.Controls.Add(radio8);
            panel3.Controls.Add(radio7);
            panel3.Controls.Add(radio6);
            panel3.Controls.Add(radio5);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(0, 106);
            panel3.Name = "panel3";
            panel3.Size = new Size(650, 50);
            panel3.TabIndex = 2;
            // 
            // radio8
            // 
            radio8.AutoSizeMode = AntdUI.TAutoSize.Width;
            radio8.Checked = true;
            radio8.Dock = DockStyle.Left;
            radio8.Enabled = false;
            radio8.Fill = Color.FromArgb(100, 0, 0);
            radio8.Location = new Point(359, 0);
            radio8.Name = "radio8";
            radio8.Size = new Size(121, 50);
            radio8.TabIndex = 3;
            radio8.Text = "Option D";
            // 
            // radio7
            // 
            radio7.AutoSizeMode = AntdUI.TAutoSize.Width;
            radio7.Dock = DockStyle.Left;
            radio7.Fill = Color.FromArgb(150, 0, 0);
            radio7.Location = new Point(239, 0);
            radio7.Name = "radio7";
            radio7.Size = new Size(120, 50);
            radio7.TabIndex = 2;
            radio7.Text = "Option C";
            // 
            // radio6
            // 
            radio6.AutoSizeMode = AntdUI.TAutoSize.Width;
            radio6.Dock = DockStyle.Left;
            radio6.Fill = Color.FromArgb(200, 0, 0);
            radio6.Location = new Point(120, 0);
            radio6.Name = "radio6";
            radio6.Size = new Size(119, 50);
            radio6.TabIndex = 1;
            radio6.Text = "Option B";
            // 
            // radio5
            // 
            radio5.AutoSizeMode = AntdUI.TAutoSize.Width;
            radio5.Dock = DockStyle.Left;
            radio5.Fill = Color.FromArgb(250, 0, 0);
            radio5.Location = new Point(0, 0);
            radio5.Name = "radio5";
            radio5.Size = new Size(120, 50);
            radio5.TabIndex = 0;
            radio5.Text = "Option A";
            // 
            // divider2
            // 
            divider2.Dock = DockStyle.Top;
            divider2.Font = new Font("Microsoft YaHei UI", 10F);
            divider2.LocalizationText = "Radio.{id}";
            divider2.Location = new Point(0, 78);
            divider2.Name = "divider2";
            divider2.Orientation = AntdUI.TOrientation.Left;
            divider2.Size = new Size(650, 28);
            divider2.TabIndex = 0;
            divider2.Text = "自定义颜色";
            // 
            // panel2
            // 
            panel2.Controls.Add(radio4);
            panel2.Controls.Add(radio3);
            panel2.Controls.Add(radio2);
            panel2.Controls.Add(radio1);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 28);
            panel2.Name = "panel2";
            panel2.Size = new Size(650, 50);
            panel2.TabIndex = 1;
            // 
            // radio4
            // 
            radio4.AutoSizeMode = AntdUI.TAutoSize.Width;
            radio4.Dock = DockStyle.Left;
            radio4.Enabled = false;
            radio4.Location = new Point(354, 0);
            radio4.Name = "radio4";
            radio4.Size = new Size(118, 50);
            radio4.TabIndex = 3;
            radio4.Text = "Option 4";
            // 
            // radio3
            // 
            radio3.AutoSizeMode = AntdUI.TAutoSize.Width;
            radio3.Dock = DockStyle.Left;
            radio3.Location = new Point(236, 0);
            radio3.Name = "radio3";
            radio3.Size = new Size(118, 50);
            radio3.TabIndex = 2;
            radio3.Text = "Option 3";
            // 
            // radio2
            // 
            radio2.AutoSizeMode = AntdUI.TAutoSize.Width;
            radio2.Dock = DockStyle.Left;
            radio2.Location = new Point(118, 0);
            radio2.Name = "radio2";
            radio2.Size = new Size(118, 50);
            radio2.TabIndex = 1;
            radio2.Text = "Option 2";
            // 
            // radio1
            // 
            radio1.AutoSizeMode = AntdUI.TAutoSize.Width;
            radio1.Dock = DockStyle.Left;
            radio1.Location = new Point(0, 0);
            radio1.Name = "radio1";
            radio1.Size = new Size(118, 50);
            radio1.TabIndex = 0;
            radio1.Text = "Option 1";
            // 
            // divider1
            // 
            divider1.Dock = DockStyle.Top;
            divider1.Font = new Font("Microsoft YaHei UI", 10F);
            divider1.LocalizationText = "Radio.{id}";
            divider1.Location = new Point(0, 0);
            divider1.Name = "divider1";
            divider1.Orientation = AntdUI.TOrientation.Left;
            divider1.Size = new Size(650, 28);
            divider1.TabIndex = 0;
            divider1.Text = "基本用法";
            // 
            // Radio
            // 
            Controls.Add(panel1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 12F);
            Name = "Radio";
            Size = new Size(650, 397);
            panel1.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.PageHeader header1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private AntdUI.Radio radio4;
        private AntdUI.Radio radio3;
        private AntdUI.Radio radio2;
        private AntdUI.Radio radio1;
        private AntdUI.Radio radio8;
        private AntdUI.Radio radio7;
        private AntdUI.Radio radio6;
        private AntdUI.Radio radio5;
        private AntdUI.Divider divider2;
        private AntdUI.Divider divider1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private AntdUI.Button button2;
        private AntdUI.Button button1;
        private AntdUI.Radio radio9;
        private AntdUI.Divider divider3;
    }
}