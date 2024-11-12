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

namespace Demo.Controls
{
    partial class Checkbox
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
            checkbox8 = new AntdUI.Checkbox();
            checkbox7 = new AntdUI.Checkbox();
            checkbox6 = new AntdUI.Checkbox();
            checkbox5 = new AntdUI.Checkbox();
            panel2 = new System.Windows.Forms.Panel();
            checkbox4 = new AntdUI.Checkbox();
            checkbox3 = new AntdUI.Checkbox();
            checkbox2 = new AntdUI.Checkbox();
            checkbox1 = new AntdUI.Checkbox();
            panel1.SuspendLayout();
            panel3.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Description = "多选框。";
            header1.Dock = DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F);
            header1.LocalizationDescription = "Checkbox.Description";
            header1.LocalizationText = "Checkbox.Text";
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new Padding(0, 0, 0, 10);
            header1.Size = new Size(1300, 74);
            header1.TabIndex = 0;
            header1.Text = "Checkbox 多选框";
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
            panel1.TabIndex = 7;
            // 
            // panel3
            // 
            panel3.Controls.Add(checkbox8);
            panel3.Controls.Add(checkbox7);
            panel3.Controls.Add(checkbox6);
            panel3.Controls.Add(checkbox5);
            panel3.Location = new Point(0, 56);
            panel3.Name = "panel3";
            panel3.Size = new Size(622, 50);
            panel3.TabIndex = 0;
            // 
            // checkbox8
            // 
            checkbox8.AutoCheck = true;
            checkbox8.AutoSizeMode = AntdUI.TAutoSize.Width;
            checkbox8.Checked = true;
            checkbox8.Dock = DockStyle.Left;
            checkbox8.Enabled = false;
            checkbox8.Fill = Color.FromArgb(100, 0, 0);
            checkbox8.Location = new Point(456, 0);
            checkbox8.Name = "checkbox8";
            checkbox8.Size = new Size(154, 50);
            checkbox8.TabIndex = 3;
            checkbox8.Text = "Option D";
            // 
            // checkbox7
            // 
            checkbox7.AutoCheck = true;
            checkbox7.AutoSizeMode = AntdUI.TAutoSize.Width;
            checkbox7.Dock = DockStyle.Left;
            checkbox7.Fill = Color.FromArgb(150, 0, 0);
            checkbox7.Location = new Point(304, 0);
            checkbox7.Name = "checkbox7";
            checkbox7.Size = new Size(152, 50);
            checkbox7.TabIndex = 2;
            checkbox7.Text = "Option C";
            // 
            // checkbox6
            // 
            checkbox6.AutoCheck = true;
            checkbox6.AutoSizeMode = AntdUI.TAutoSize.Width;
            checkbox6.Dock = DockStyle.Left;
            checkbox6.Fill = Color.FromArgb(200, 0, 0);
            checkbox6.Location = new Point(153, 0);
            checkbox6.Name = "checkbox6";
            checkbox6.Size = new Size(151, 50);
            checkbox6.TabIndex = 1;
            checkbox6.Text = "Option B";
            // 
            // checkbox5
            // 
            checkbox5.AutoCheck = true;
            checkbox5.AutoSizeMode = AntdUI.TAutoSize.Width;
            checkbox5.Dock = DockStyle.Left;
            checkbox5.Fill = Color.FromArgb(250, 0, 0);
            checkbox5.Location = new Point(0, 0);
            checkbox5.Name = "checkbox5";
            checkbox5.Size = new Size(153, 50);
            checkbox5.TabIndex = 0;
            checkbox5.Text = "Option A";
            // 
            // panel2
            // 
            panel2.Controls.Add(checkbox4);
            panel2.Controls.Add(checkbox3);
            panel2.Controls.Add(checkbox2);
            panel2.Controls.Add(checkbox1);
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(622, 50);
            panel2.TabIndex = 0;
            // 
            // checkbox4
            // 
            checkbox4.AutoCheck = true;
            checkbox4.AutoSizeMode = AntdUI.TAutoSize.Width;
            checkbox4.Dock = DockStyle.Left;
            checkbox4.Enabled = false;
            checkbox4.Location = new Point(450, 0);
            checkbox4.Name = "checkbox4";
            checkbox4.Size = new Size(150, 50);
            checkbox4.TabIndex = 4;
            checkbox4.Text = "Option 4";
            // 
            // checkbox3
            // 
            checkbox3.AutoCheck = true;
            checkbox3.AutoSizeMode = AntdUI.TAutoSize.Width;
            checkbox3.Dock = DockStyle.Left;
            checkbox3.Location = new Point(300, 0);
            checkbox3.Name = "checkbox3";
            checkbox3.Size = new Size(150, 50);
            checkbox3.TabIndex = 3;
            checkbox3.Text = "Option 3";
            // 
            // checkbox2
            // 
            checkbox2.AutoCheck = true;
            checkbox2.AutoSizeMode = AntdUI.TAutoSize.Width;
            checkbox2.Dock = DockStyle.Left;
            checkbox2.Location = new Point(150, 0);
            checkbox2.Name = "checkbox2";
            checkbox2.Size = new Size(150, 50);
            checkbox2.TabIndex = 2;
            checkbox2.Text = "Option 2";
            // 
            // checkbox1
            // 
            checkbox1.AutoCheck = true;
            checkbox1.AutoSizeMode = AntdUI.TAutoSize.Width;
            checkbox1.Dock = DockStyle.Left;
            checkbox1.Location = new Point(0, 0);
            checkbox1.Name = "checkbox1";
            checkbox1.Size = new Size(150, 50);
            checkbox1.TabIndex = 0;
            checkbox1.Text = "Option 1";
            // 
            // Checkbox
            // 
            Controls.Add(panel1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 16F);
            Name = "Checkbox";
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
        private System.Windows.Forms.Panel panel3;
        private AntdUI.Checkbox checkbox8;
        private AntdUI.Checkbox checkbox7;
        private AntdUI.Checkbox checkbox6;
        private AntdUI.Checkbox checkbox5;
        private System.Windows.Forms.Panel panel2;
        private AntdUI.Checkbox checkbox4;
        private AntdUI.Checkbox checkbox3;
        private AntdUI.Checkbox checkbox2;
        private AntdUI.Checkbox checkbox1;
    }
}