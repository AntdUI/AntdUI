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
    partial class Panel
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
            panel8 = new AntdUI.Panel();
            label5 = new Label();
            divider1 = new AntdUI.Divider();
            label7 = new Label();
            panel1 = new AntdUI.Panel();
            label1 = new Label();
            divider3 = new AntdUI.Divider();
            label3 = new Label();
            panel9 = new AntdUI.Panel();
            label9 = new Label();
            label8 = new Label();
            avatar2 = new AntdUI.Avatar();
            panel4 = new AntdUI.Panel();
            label12 = new Label();
            label13 = new Label();
            avatar1 = new AntdUI.Avatar();
            panel5 = new AntdUI.Panel();
            button3 = new AntdUI.Button();
            button4 = new AntdUI.Button();
            flowLayoutPanel1.SuspendLayout();
            panel8.SuspendLayout();
            panel1.SuspendLayout();
            panel9.SuspendLayout();
            panel4.SuspendLayout();
            panel5.SuspendLayout();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Dock = DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new Padding(6);
            header1.Size = new Size(835, 79);
            header1.TabIndex = 4;
            header1.Text = "Panel 面板";
            header1.TextDesc = "内容区域。";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.Controls.Add(panel8);
            flowLayoutPanel1.Controls.Add(panel1);
            flowLayoutPanel1.Controls.Add(panel9);
            flowLayoutPanel1.Controls.Add(panel4);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(0, 79);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(835, 555);
            flowLayoutPanel1.TabIndex = 5;
            // 
            // panel8
            // 
            panel8.ArrowAlign = AntdUI.TAlign.TL;
            panel8.ArrowSize = 10;
            panel8.Controls.Add(label5);
            panel8.Controls.Add(divider1);
            panel8.Controls.Add(label7);
            panel8.Location = new Point(3, 3);
            panel8.Name = "panel8";
            panel8.Radius = 10;
            panel8.Shadow = 24;
            panel8.ShadowOpacity = 0.18F;
            panel8.Size = new Size(269, 221);
            panel8.TabIndex = 15;
            // 
            // label5
            // 
            label5.BackColor = Color.Transparent;
            label5.Dock = DockStyle.Fill;
            label5.Font = new Font("Microsoft YaHei UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            label5.Location = new Point(24, 73);
            label5.Name = "label5";
            label5.Padding = new Padding(20, 10, 0, 0);
            label5.Size = new Size(221, 124);
            label5.TabIndex = 2;
            label5.Text = "Card content\r\n\r\nCard content\r\n\r\nCard content";
            // 
            // divider1
            // 
            divider1.BackColor = Color.Transparent;
            divider1.Dock = DockStyle.Top;
            divider1.Location = new Point(24, 72);
            divider1.Margin = new Padding(10);
            divider1.Name = "divider1";
            divider1.Size = new Size(221, 1);
            divider1.TabIndex = 1;
            // 
            // label7
            // 
            label7.BackColor = Color.Transparent;
            label7.Dock = DockStyle.Top;
            label7.Font = new Font("Microsoft YaHei UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point);
            label7.Location = new Point(24, 24);
            label7.Name = "label7";
            label7.Padding = new Padding(20, 0, 0, 0);
            label7.Size = new Size(221, 48);
            label7.TabIndex = 0;
            label7.Text = "Card title";
            label7.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            panel1.ArrowAlign = AntdUI.TAlign.LT;
            panel1.ArrowSize = 10;
            panel1.Controls.Add(label1);
            panel1.Controls.Add(divider3);
            panel1.Controls.Add(label3);
            panel1.Location = new Point(278, 3);
            panel1.Name = "panel1";
            panel1.Radius = 0;
            panel1.Shadow = 24;
            panel1.ShadowOpacity = 0.18F;
            panel1.Size = new Size(269, 221);
            panel1.TabIndex = 18;
            // 
            // label1
            // 
            label1.BackColor = Color.Transparent;
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Microsoft YaHei UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(24, 73);
            label1.Name = "label1";
            label1.Padding = new Padding(20, 10, 0, 0);
            label1.Size = new Size(221, 124);
            label1.TabIndex = 2;
            label1.Text = "Card content\r\n\r\nCard content\r\n\r\nCard content";
            // 
            // divider3
            // 
            divider3.BackColor = Color.Transparent;
            divider3.Dock = DockStyle.Top;
            divider3.Location = new Point(24, 72);
            divider3.Margin = new Padding(10);
            divider3.Name = "divider3";
            divider3.Size = new Size(221, 1);
            divider3.TabIndex = 1;
            // 
            // label3
            // 
            label3.BackColor = Color.Transparent;
            label3.Dock = DockStyle.Top;
            label3.Font = new Font("Microsoft YaHei UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point);
            label3.Location = new Point(24, 24);
            label3.Name = "label3";
            label3.Padding = new Padding(20, 0, 0, 0);
            label3.Size = new Size(221, 48);
            label3.TabIndex = 0;
            label3.Text = "Card title";
            label3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // panel9
            // 
            panel9.ArrowAlign = AntdUI.TAlign.TL;
            panel9.ArrowSize = 10;
            panel9.Back = Color.FromArgb(0, 144, 255);
            panel9.Controls.Add(label9);
            panel9.Controls.Add(label8);
            panel9.Controls.Add(avatar2);
            panel9.ForeColor = Color.White;
            panel9.Location = new Point(3, 230);
            panel9.Name = "panel9";
            panel9.Padding = new Padding(14);
            panel9.Radius = 10;
            panel9.Shadow = 24;
            panel9.ShadowOpacity = 0.18F;
            panel9.Size = new Size(320, 258);
            panel9.TabIndex = 14;
            // 
            // label9
            // 
            label9.BackColor = Color.Transparent;
            label9.Dock = DockStyle.Fill;
            label9.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label9.Location = new Point(38, 178);
            label9.Name = "label9";
            label9.Padding = new Padding(2, 0, 2, 0);
            label9.Size = new Size(244, 42);
            label9.TabIndex = 12;
            label9.Text = "Here is the content, here is the conten";
            // 
            // label8
            // 
            label8.BackColor = Color.Transparent;
            label8.Dock = DockStyle.Top;
            label8.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            label8.Location = new Point(38, 148);
            label8.Name = "label8";
            label8.Size = new Size(244, 30);
            label8.TabIndex = 11;
            label8.Text = "Tour Title";
            label8.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // avatar2
            // 
            avatar2.BackColor = Color.Transparent;
            avatar2.Dock = DockStyle.Top;
            avatar2.Image = Properties.Resources.img1;
            avatar2.Location = new Point(38, 38);
            avatar2.Name = "avatar2";
            avatar2.Radius = 6;
            avatar2.Size = new Size(244, 110);
            avatar2.TabIndex = 9;
            // 
            // panel4
            // 
            panel4.ArrowAlign = AntdUI.TAlign.Top;
            panel4.ArrowSize = 10;
            panel4.Controls.Add(label12);
            panel4.Controls.Add(label13);
            panel4.Controls.Add(avatar1);
            panel4.Controls.Add(panel5);
            panel4.Location = new Point(329, 230);
            panel4.Name = "panel4";
            panel4.Padding = new Padding(14);
            panel4.Radius = 10;
            panel4.Shadow = 24;
            panel4.Size = new Size(320, 299);
            panel4.TabIndex = 19;
            // 
            // label12
            // 
            label12.BackColor = Color.Transparent;
            label12.Dock = DockStyle.Fill;
            label12.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label12.Location = new Point(38, 178);
            label12.Name = "label12";
            label12.Padding = new Padding(2, 0, 2, 0);
            label12.Size = new Size(244, 43);
            label12.TabIndex = 12;
            label12.Text = "Here is the content, here is the conten";
            // 
            // label13
            // 
            label13.BackColor = Color.Transparent;
            label13.Dock = DockStyle.Top;
            label13.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            label13.Location = new Point(38, 148);
            label13.Name = "label13";
            label13.Size = new Size(244, 30);
            label13.TabIndex = 11;
            label13.Text = "Tour Title";
            label13.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // avatar1
            // 
            avatar1.BackColor = Color.Transparent;
            avatar1.Dock = DockStyle.Top;
            avatar1.Image = Properties.Resources.img1;
            avatar1.Location = new Point(38, 38);
            avatar1.Name = "avatar1";
            avatar1.Radius = 6;
            avatar1.Size = new Size(244, 110);
            avatar1.TabIndex = 9;
            // 
            // panel5
            // 
            panel5.Back = Color.Transparent;
            panel5.BackColor = Color.Transparent;
            panel5.Controls.Add(button3);
            panel5.Controls.Add(button4);
            panel5.Dock = DockStyle.Bottom;
            panel5.Location = new Point(38, 221);
            panel5.Name = "panel5";
            panel5.Radius = 0;
            panel5.Size = new Size(244, 40);
            panel5.TabIndex = 13;
            // 
            // button3
            // 
            button3.Back = Color.FromArgb(217, 217, 217);
            button3.BorderWidth = 1.4F;
            button3.Dock = DockStyle.Right;
            button3.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            button3.ForeColor = Color.Black;
            button3.Location = new Point(98, 0);
            button3.Name = "button3";
            button3.Size = new Size(73, 40);
            button3.TabIndex = 1;
            button3.Text = "Cancel";
            // 
            // button4
            // 
            button4.Dock = DockStyle.Right;
            button4.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            button4.ForeColor = Color.White;
            button4.Location = new Point(171, 0);
            button4.Name = "button4";
            button4.Size = new Size(73, 40);
            button4.TabIndex = 0;
            button4.Text = "OK";
            button4.Type = AntdUI.TTypeMini.Primary;
            // 
            // Panel
            // 
            Controls.Add(flowLayoutPanel1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 16F, FontStyle.Regular, GraphicsUnit.Point);
            Name = "Panel";
            Size = new Size(835, 634);
            flowLayoutPanel1.ResumeLayout(false);
            panel8.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel9.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel5.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private AntdUI.Header header1;
        private FlowLayoutPanel flowLayoutPanel1;
        private AntdUI.Panel panel9;
        private Label label9;
        private Label label8;
        private AntdUI.Avatar avatar2;
        private AntdUI.Panel panel8;
        private Label label5;
        private AntdUI.Divider divider1;
        private Label label7;
        private AntdUI.Panel panel1;
        private Label label1;
        private AntdUI.Divider divider3;
        private Label label3;
        private AntdUI.Panel panel4;
        private Label label12;
        private Label label13;
        private AntdUI.Avatar avatar1;
        private AntdUI.Panel panel5;
        private AntdUI.Button button3;
        private AntdUI.Button button4;
    }
}