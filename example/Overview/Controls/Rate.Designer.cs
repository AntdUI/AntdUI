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

namespace Overview.Controls
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

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Rate));
            header1 = new AntdUI.Header();
            panel1 = new System.Windows.Forms.Panel();
            rate4 = new AntdUI.Rate();
            rate3 = new AntdUI.Rate();
            rate2 = new AntdUI.Rate();
            rate1 = new AntdUI.Rate();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Dock = DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new Padding(6);
            header1.Size = new Size(614, 79);
            header1.TabIndex = 4;
            header1.Text = "Rate 评分";
            header1.TextDesc = "评分组件。";
            // 
            // panel1
            // 
            panel1.AutoScroll = true;
            panel1.Controls.Add(rate4);
            panel1.Controls.Add(rate3);
            panel1.Controls.Add(rate2);
            panel1.Controls.Add(rate1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 79);
            panel1.Name = "panel1";
            panel1.Size = new Size(614, 367);
            panel1.TabIndex = 6;
            // 
            // rate4
            // 
            rate4.AllowHalf = true;
            rate4.Character = resources.GetString("rate4.Character");
            rate4.Dock = DockStyle.Top;
            rate4.Fill = Color.Salmon;
            rate4.Location = new Point(0, 90);
            rate4.Name = "rate4";
            rate4.Size = new Size(614, 46);
            rate4.TabIndex = 33;
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
            rate3.TabIndex = 32;
            rate3.Text = "rate3";
            rate3.Value = 2.5F;
            // 
            // rate2
            // 
            rate2.Character = resources.GetString("rate2.Character");
            rate2.Dock = DockStyle.Top;
            rate2.Location = new Point(0, 30);
            rate2.Name = "rate2";
            rate2.Size = new Size(614, 30);
            rate2.TabIndex = 31;
            rate2.Text = "rate2";
            rate2.Value = 2F;
            // 
            // rate1
            // 
            rate1.Dock = DockStyle.Top;
            rate1.Location = new Point(0, 0);
            rate1.Name = "rate1";
            rate1.Size = new Size(614, 30);
            rate1.TabIndex = 30;
            rate1.Text = "rate1";
            // 
            // Rate
            // 
            Controls.Add(panel1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 16F, FontStyle.Regular, GraphicsUnit.Point);
            Name = "Rate";
            Size = new Size(614, 446);
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private AntdUI.Header header1;
        private System.Windows.Forms.Panel panel1;
        private AntdUI.Rate rate4;
        private AntdUI.Rate rate3;
        private AntdUI.Rate rate2;
        private AntdUI.Rate rate1;
    }
}