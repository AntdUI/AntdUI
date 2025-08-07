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
    partial class Rate
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
            rate6 = new AntdUI.Rate();
            rate5 = new AntdUI.Rate();
            rate4 = new AntdUI.Rate();
            rate3 = new AntdUI.Rate();
            rate2 = new AntdUI.Rate();
            rate1 = new AntdUI.Rate();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Description = "评分组件。";
            header1.Dock = DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F);
            header1.LocalizationDescription = "Rate.Description";
            header1.LocalizationText = "Rate.Text";
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new Padding(0, 0, 0, 10);
            header1.Size = new Size(614, 74);
            header1.TabIndex = 0;
            header1.Text = "Rate 评分";
            header1.UseTitleFont = true;
            // 
            // panel1
            // 
            panel1.Controls.Add(rate6);
            panel1.Controls.Add(rate5);
            panel1.Controls.Add(rate4);
            panel1.Controls.Add(rate3);
            panel1.Controls.Add(rate2);
            panel1.Controls.Add(rate1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 74);
            panel1.Name = "panel1";
            panel1.Size = new Size(614, 372);
            panel1.TabIndex = 0;
            // 
            // rate6
            // 
            rate6.AllowHalf = true;
            rate6.Character = "牛皮";
            rate6.Dock = DockStyle.Top;
            rate6.Fill = Color.FromArgb(3, 169, 244);
            rate6.LocalizationCharacter = "Rate.{id}";
            rate6.Location = new Point(0, 189);
            rate6.Name = "rate6";
            rate6.Size = new Size(614, 75);
            rate6.TabIndex = 5;
            rate6.Text = "rate6";
            rate6.Value = 2.5F;
            // 
            // rate5
            // 
            rate5.Character = "好";
            rate5.Dock = DockStyle.Top;
            rate5.Fill = Color.FromArgb(255, 87, 34);
            rate5.LocalizationCharacter = "Rate.{id}";
            rate5.Location = new Point(0, 136);
            rate5.Name = "rate5";
            rate5.Size = new Size(614, 53);
            rate5.TabIndex = 4;
            rate5.Text = "rate5";
            rate5.Value = 2F;
            // 
            // rate4
            // 
            rate4.AllowHalf = true;
            rate4.Dock = DockStyle.Top;
            rate4.Fill = Color.Salmon;
            rate4.Location = new Point(0, 90);
            rate4.Name = "rate4";
            rate4.Size = new Size(614, 46);
            rate4.TabIndex = 3;
            rate4.Text = "rate4";
            rate4.Value = 2.5F;
            // 
            // rate3
            // 
            rate3.AllowHalf = true;
            rate3.Dock = DockStyle.Top;
            rate3.Location = new Point(0, 60);
            rate3.Name = "rate3";
            rate3.Size = new Size(614, 30);
            rate3.TabIndex = 2;
            rate3.Text = "rate3";
            rate3.Value = 2.5F;
            // 
            // rate2
            // 
            rate2.Dock = DockStyle.Top;
            rate2.Location = new Point(0, 30);
            rate2.Name = "rate2";
            rate2.Size = new Size(614, 30);
            rate2.TabIndex = 1;
            rate2.Text = "rate2";
            rate2.Value = 2F;
            // 
            // rate1
            // 
            rate1.Dock = DockStyle.Top;
            rate1.Location = new Point(0, 0);
            rate1.Name = "rate1";
            rate1.Size = new Size(614, 30);
            rate1.TabIndex = 0;
            rate1.Text = "rate1";
            // 
            // Rate
            // 
            Controls.Add(panel1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 12F);
            Name = "Rate";
            Size = new Size(614, 446);
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.PageHeader header1;
        private System.Windows.Forms.Panel panel1;
        private AntdUI.Rate rate4;
        private AntdUI.Rate rate3;
        private AntdUI.Rate rate2;
        private AntdUI.Rate rate1;
        private AntdUI.Rate rate5;
        private AntdUI.Rate rate6;
    }
}