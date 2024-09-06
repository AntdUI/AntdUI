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

namespace Overview.Controls
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

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            header1 = new AntdUI.PageHeader();
            panel1 = new System.Windows.Forms.Panel();
            panel3 = new System.Windows.Forms.Panel();
            radio8 = new AntdUI.Radio();
            radio7 = new AntdUI.Radio();
            radio6 = new AntdUI.Radio();
            radio5 = new AntdUI.Radio();
            panel2 = new System.Windows.Forms.Panel();
            radio4 = new AntdUI.Radio();
            radio3 = new AntdUI.Radio();
            radio2 = new AntdUI.Radio();
            radio1 = new AntdUI.Radio();
            panel1.SuspendLayout();
            panel3.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Description = "单选框。";
            header1.Dock = DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F);
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new Padding(0, 0, 0, 10);
            header1.Size = new Size(1300, 74);
            header1.TabIndex = 0;
            header1.Text = "Radio 单选框";
            header1.UseTitleFont = true;
            // 
            // panel1
            // 
            panel1.AutoScroll = true;
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(panel2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 74);
            panel1.Name = "panel1";
            panel1.Size = new Size(1300, 602);
            panel1.TabIndex = 6;
            // 
            // panel3
            // 
            panel3.Controls.Add(radio8);
            panel3.Controls.Add(radio7);
            panel3.Controls.Add(radio6);
            panel3.Controls.Add(radio5);
            panel3.Location = new Point(0, 56);
            panel3.Name = "panel3";
            panel3.Size = new Size(622, 50);
            panel3.TabIndex = 0;
            // 
            // radio8
            // 
            radio8.AutoCheck = true;
            radio8.AutoSizeMode = AntdUI.TAutoSize.Width;
            radio8.Checked = true;
            radio8.Dock = DockStyle.Left;
            radio8.Enabled = false;
            radio8.Fill = Color.FromArgb(100, 0, 0);
            radio8.Location = new Point(456, 0);
            radio8.Name = "radio8";
            radio8.Size = new Size(154, 50);
            radio8.TabIndex = 3;
            radio8.Text = "Option D";
            // 
            // radio7
            // 
            radio7.AutoCheck = true;
            radio7.AutoSizeMode = AntdUI.TAutoSize.Width;
            radio7.Dock = DockStyle.Left;
            radio7.Fill = Color.FromArgb(150, 0, 0);
            radio7.Location = new Point(304, 0);
            radio7.Name = "radio7";
            radio7.Size = new Size(152, 50);
            radio7.TabIndex = 2;
            radio7.Text = "Option C";
            // 
            // radio6
            // 
            radio6.AutoCheck = true;
            radio6.AutoSizeMode = AntdUI.TAutoSize.Width;
            radio6.Dock = DockStyle.Left;
            radio6.Fill = Color.FromArgb(200, 0, 0);
            radio6.Location = new Point(153, 0);
            radio6.Name = "radio6";
            radio6.Size = new Size(151, 50);
            radio6.TabIndex = 1;
            radio6.Text = "Option B";
            // 
            // radio5
            // 
            radio5.AutoCheck = true;
            radio5.AutoSizeMode = AntdUI.TAutoSize.Width;
            radio5.Dock = DockStyle.Left;
            radio5.Fill = Color.FromArgb(250, 0, 0);
            radio5.Location = new Point(0, 0);
            radio5.Name = "radio5";
            radio5.Size = new Size(153, 50);
            radio5.TabIndex = 0;
            radio5.Text = "Option A";
            // 
            // panel2
            // 
            panel2.Controls.Add(radio4);
            panel2.Controls.Add(radio3);
            panel2.Controls.Add(radio2);
            panel2.Controls.Add(radio1);
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(622, 50);
            panel2.TabIndex = 0;
            // 
            // radio4
            // 
            radio4.AutoCheck = true;
            radio4.AutoSizeMode = AntdUI.TAutoSize.Width;
            radio4.Dock = DockStyle.Left;
            radio4.Enabled = false;
            radio4.Location = new Point(450, 0);
            radio4.Name = "radio4";
            radio4.Size = new Size(150, 50);
            radio4.TabIndex = 4;
            radio4.Text = "Option 4";
            // 
            // radio3
            // 
            radio3.AutoCheck = true;
            radio3.AutoSizeMode = AntdUI.TAutoSize.Width;
            radio3.Dock = DockStyle.Left;
            radio3.Location = new Point(300, 0);
            radio3.Name = "radio3";
            radio3.Size = new Size(150, 50);
            radio3.TabIndex = 3;
            radio3.Text = "Option 3";
            // 
            // radio2
            // 
            radio2.AutoCheck = true;
            radio2.AutoSizeMode = AntdUI.TAutoSize.Width;
            radio2.Dock = DockStyle.Left;
            radio2.Location = new Point(150, 0);
            radio2.Name = "radio2";
            radio2.Size = new Size(150, 50);
            radio2.TabIndex = 2;
            radio2.Text = "Option 2";
            // 
            // radio1
            // 
            radio1.AutoCheck = true;
            radio1.AutoSizeMode = AntdUI.TAutoSize.Width;
            radio1.Dock = DockStyle.Left;
            radio1.Location = new Point(0, 0);
            radio1.Name = "radio1";
            radio1.Size = new Size(150, 50);
            radio1.TabIndex = 0;
            radio1.Text = "Option 1";
            // 
            // Radio
            // 
            Controls.Add(panel1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 16F);
            Name = "Radio";
            Size = new Size(1300, 676);
            panel1.ResumeLayout(false);
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
    }
}