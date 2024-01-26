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
    partial class Divider
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
            header1 = new AntdUI.Header();
            flowLayoutPanel1 = new FlowLayoutPanel();
            panel1 = new System.Windows.Forms.Panel();
            divider2 = new AntdUI.Divider();
            label2 = new Label();
            divider1 = new AntdUI.Divider();
            label1 = new Label();
            divider3 = new AntdUI.Divider();
            flowLayoutPanel1.SuspendLayout();
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
            header1.Size = new Size(740, 79);
            header1.TabIndex = 4;
            header1.Text = "Divider 分割线";
            header1.TextDesc = "区隔内容的分割线。";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.Controls.Add(panel1);
            flowLayoutPanel1.Controls.Add(divider3);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(0, 79);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(740, 323);
            flowLayoutPanel1.TabIndex = 5;
            // 
            // panel1
            // 
            panel1.Controls.Add(divider2);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(divider1);
            panel1.Controls.Add(label1);
            panel1.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(512, 213);
            panel1.TabIndex = 1;
            // 
            // divider2
            // 
            divider2.Dock = DockStyle.Top;
            divider2.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            divider2.Location = new Point(0, 116);
            divider2.Name = "divider2";
            divider2.Size = new Size(512, 20);
            divider2.TabIndex = 3;
            divider2.Text = "With Text";
            divider2.Thickness = 2F;
            // 
            // label2
            // 
            label2.Dock = DockStyle.Top;
            label2.Location = new Point(0, 68);
            label2.Name = "label2";
            label2.Size = new Size(512, 48);
            label2.TabIndex = 2;
            label2.Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed nonne merninisti licere mihi ista probare";
            // 
            // divider1
            // 
            divider1.Dock = DockStyle.Top;
            divider1.Location = new Point(0, 48);
            divider1.Name = "divider1";
            divider1.Size = new Size(512, 20);
            divider1.TabIndex = 0;
            // 
            // label1
            // 
            label1.Dock = DockStyle.Top;
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(512, 48);
            label1.TabIndex = 1;
            label1.Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed nonne merninisti licere mihi ista probare";
            // 
            // divider3
            // 
            divider3.Location = new Point(521, 3);
            divider3.Name = "divider3";
            divider3.Size = new Size(29, 317);
            divider3.TabIndex = 4;
            divider3.Text = "分割线";
            divider3.Thickness = 2F;
            divider3.Vertical = true;
            // 
            // Divider
            // 
            Controls.Add(flowLayoutPanel1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 16F, FontStyle.Regular, GraphicsUnit.Point);
            Name = "Divider";
            Size = new Size(740, 402);
            flowLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.Header header1;
        private FlowLayoutPanel flowLayoutPanel1;
        private AntdUI.Divider divider1;
        private System.Windows.Forms.Panel panel1;
        private AntdUI.Divider divider2;
        private Label label2;
        private Label label1;
        private AntdUI.Divider divider3;
    }
}