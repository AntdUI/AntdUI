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
    partial class Progress
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
            panel1 = new System.Windows.Forms.Panel();
            panel2 = new System.Windows.Forms.Panel();
            progress1 = new AntdUI.Progress();
            label1 = new Label();
            panel3 = new System.Windows.Forms.Panel();
            panel4 = new System.Windows.Forms.Panel();
            progress2 = new AntdUI.Progress();
            label2 = new AntdUI.Icon.IconComplete();
            panel5 = new System.Windows.Forms.Panel();
            panel6 = new System.Windows.Forms.Panel();
            progress3 = new AntdUI.Progress();
            label3 = new AntdUI.Icon.IconError();
            panel8 = new System.Windows.Forms.Panel();
            progress4 = new AntdUI.Progress();
            progress5 = new AntdUI.Progress();
            iComplete1 = new AntdUI.Icon.IconComplete();
            progress6 = new AntdUI.Progress();
            iError2 = new AntdUI.Icon.IconError();
            panel7 = new System.Windows.Forms.Panel();
            progress7 = new AntdUI.Progress();
            progress9 = new AntdUI.Progress();
            progress8 = new AntdUI.Progress();
            panel9 = new System.Windows.Forms.Panel();
            divider1 = new AntdUI.Divider();
            panel10 = new System.Windows.Forms.Panel();
            divider2 = new AntdUI.Divider();
            divider3 = new AntdUI.Divider();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            panel4.SuspendLayout();
            panel5.SuspendLayout();
            panel6.SuspendLayout();
            panel8.SuspendLayout();
            progress5.SuspendLayout();
            progress6.SuspendLayout();
            panel7.SuspendLayout();
            panel9.SuspendLayout();
            panel10.SuspendLayout();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Dock = DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new Padding(6);
            header1.Size = new Size(650, 79);
            header1.TabIndex = 5;
            header1.Text = "Progress 进度条";
            header1.TextDesc = "展示操作的当前进度。";
            // 
            // panel1
            // 
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(10, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(613, 30);
            panel1.TabIndex = 3;
            // 
            // panel2
            // 
            panel2.Controls.Add(progress1);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Padding = new Padding(0, 10, 0, 10);
            panel2.Size = new Size(563, 30);
            panel2.TabIndex = 11;
            // 
            // progress1
            // 
            progress1.Fill = Color.FromArgb(0, 144, 255);
            progress1.Dock = DockStyle.Fill;
            progress1.Loading = true;
            progress1.Location = new Point(0, 10);
            progress1.Name = "progress1";
            progress1.Size = new Size(563, 10);
            progress1.TabIndex = 10;
            progress1.Value = 0.5F;
            progress1.Click += Progress_Blue_1;
            // 
            // label1
            // 
            label1.Dock = DockStyle.Right;
            label1.Location = new Point(563, 0);
            label1.Name = "label1";
            label1.Size = new Size(50, 30);
            label1.TabIndex = 0;
            label1.Text = "50%";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // panel3
            // 
            panel3.Controls.Add(panel4);
            panel3.Controls.Add(label2);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(10, 30);
            panel3.Name = "panel3";
            panel3.Padding = new Padding(0, 6, 0, 6);
            panel3.Size = new Size(613, 30);
            panel3.TabIndex = 2;
            // 
            // panel4
            // 
            panel4.Controls.Add(progress2);
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(0, 6);
            panel4.Name = "panel4";
            panel4.Padding = new Padding(0, 4, 0, 4);
            panel4.Size = new Size(563, 18);
            panel4.TabIndex = 11;
            // 
            // progress2
            // 
            progress2.Fill = Color.FromArgb(0, 204, 0);
            progress2.Dock = DockStyle.Fill;
            progress2.Location = new Point(0, 4);
            progress2.Name = "progress2";
            progress2.Size = new Size(563, 10);
            progress2.TabIndex = 10;
            progress2.Value = 1F;
            // 
            // label2
            // 
            label2.Dock = DockStyle.Right;
            label2.Location = new Point(563, 6);
            label2.Name = "label2";
            label2.Size = new Size(50, 18);
            label2.TabIndex = 0;
            label2.TabStop = false;
            // 
            // panel5
            // 
            panel5.Controls.Add(panel6);
            panel5.Controls.Add(label3);
            panel5.Dock = DockStyle.Top;
            panel5.Location = new Point(10, 60);
            panel5.Name = "panel5";
            panel5.Padding = new Padding(0, 6, 0, 6);
            panel5.Size = new Size(613, 30);
            panel5.TabIndex = 1;
            // 
            // panel6
            // 
            panel6.Controls.Add(progress3);
            panel6.Dock = DockStyle.Fill;
            panel6.Location = new Point(0, 6);
            panel6.Name = "panel6";
            panel6.Padding = new Padding(0, 4, 0, 4);
            panel6.Size = new Size(563, 18);
            panel6.TabIndex = 11;
            // 
            // progress3
            // 
            progress3.Fill = Color.FromArgb(255, 79, 87);
            progress3.Dock = DockStyle.Fill;
            progress3.Location = new Point(0, 4);
            progress3.Name = "progress3";
            progress3.Size = new Size(563, 10);
            progress3.TabIndex = 10;
            progress3.Value = 0.7F;
            progress3.Click += Progress_Red;
            // 
            // label3
            // 
            label3.Dock = DockStyle.Right;
            label3.Location = new Point(563, 6);
            label3.Name = "label3";
            label3.Size = new Size(50, 18);
            label3.TabIndex = 0;
            label3.TabStop = false;
            // 
            // panel8
            // 
            panel8.Controls.Add(progress4);
            panel8.Controls.Add(progress5);
            panel8.Controls.Add(progress6);
            panel8.Dock = DockStyle.Top;
            panel8.Location = new Point(0, 149);
            panel8.Name = "panel8";
            panel8.Size = new Size(633, 120);
            panel8.TabIndex = 18;
            // 
            // progress4
            // 
            progress4.Font = new Font("Microsoft YaHei UI", 16F, FontStyle.Regular, GraphicsUnit.Point);
            progress4.Loading = true;
            progress4.Location = new Point(10, 7);
            progress4.Name = "progress4";
            progress4.Radius = 5;
            progress4.Shape = AntdUI.TShape.Circle;
            progress4.Size = new Size(90, 90);
            progress4.TabIndex = 16;
            progress4.Text = "68%";
            progress4.Value = 0.68F;
            progress4.Click += Progress_Blue_2;
            // 
            // progress5
            // 
            progress5.Fill = Color.FromArgb(0, 204, 0);
            progress5.Controls.Add(iComplete1);
            progress5.Font = new Font("Microsoft YaHei UI", 16F, FontStyle.Regular, GraphicsUnit.Point);
            progress5.Location = new Point(158, 7);
            progress5.Name = "progress5";
            progress5.Radius = 5;
            progress5.Shape = AntdUI.TShape.Circle;
            progress5.Size = new Size(90, 90);
            progress5.TabIndex = 14;
            progress5.Value = 1F;
            // 
            // iComplete1
            // 
            iComplete1.Anchor = AnchorStyles.None;
            iComplete1.Back = Color.Transparent;
            iComplete1.Color = Color.FromArgb(0, 204, 0);
            iComplete1.Location = new Point(25, 25);
            iComplete1.Name = "iComplete1";
            iComplete1.Size = new Size(40, 40);
            iComplete1.TabIndex = 11;
            // 
            // progress6
            // 
            progress6.Fill = Color.FromArgb(255, 79, 87);
            progress6.Controls.Add(iError2);
            progress6.Font = new Font("Microsoft YaHei UI", 16F, FontStyle.Regular, GraphicsUnit.Point);
            progress6.Location = new Point(306, 7);
            progress6.Name = "progress6";
            progress6.Radius = 5;
            progress6.Shape = AntdUI.TShape.Circle;
            progress6.Size = new Size(90, 90);
            progress6.TabIndex = 12;
            progress6.Value = 0.7F;
            progress6.Click += Progress_Red;
            // 
            // iError2
            // 
            iError2.Anchor = AnchorStyles.None;
            iError2.Back = Color.Transparent;
            iError2.Color = Color.FromArgb(255, 79, 87);
            iError2.Location = new Point(25, 25);
            iError2.Name = "iError2";
            iError2.Size = new Size(40, 40);
            iError2.TabIndex = 11;
            iError2.Click += Progress_Red;
            // 
            // panel7
            // 
            panel7.Controls.Add(progress7);
            panel7.Controls.Add(progress9);
            panel7.Controls.Add(progress8);
            panel7.Dock = DockStyle.Top;
            panel7.Location = new Point(0, 291);
            panel7.Name = "panel7";
            panel7.Size = new Size(633, 48);
            panel7.TabIndex = 17;
            // 
            // progress7
            // 
            progress7.Back = Color.FromArgb(40, 22, 119, 255);
            progress7.Location = new Point(6, 3);
            progress7.Mini = true;
            progress7.Name = "progress7";
            progress7.Radius = 4;
            progress7.Size = new Size(148, 36);
            progress7.TabIndex = 15;
            progress7.Text = "In Progress";
            progress7.Value = 0.68F;
            progress7.Click += Progress_Blue_2;
            // 
            // progress9
            // 
            progress9.Back = Color.FromArgb(40, 255, 79, 87);
            progress9.Fill = Color.FromArgb(255, 79, 87);
            progress9.Location = new Point(297, 3);
            progress9.Mini = true;
            progress9.Name = "progress9";
            progress9.Radius = 4;
            progress9.Size = new Size(128, 36);
            progress9.TabIndex = 11;
            progress9.Text = "Failed";
            progress9.Value = 0.7F;
            progress9.Click += Progress_Red;
            // 
            // progress8
            // 
            progress8.Back = Color.FromArgb(40, 0, 204, 0);
            progress8.Fill = Color.FromArgb(0, 204, 0);
            progress8.Location = new Point(164, 3);
            progress8.Mini = true;
            progress8.Name = "progress8";
            progress8.Radius = 4;
            progress8.Size = new Size(128, 36);
            progress8.TabIndex = 13;
            progress8.Text = "Success";
            progress8.Value = 1F;
            // 
            // panel9
            // 
            panel9.AutoScroll = true;
            panel9.Controls.Add(panel7);
            panel9.Controls.Add(divider3);
            panel9.Controls.Add(panel8);
            panel9.Controls.Add(divider2);
            panel9.Controls.Add(panel10);
            panel9.Controls.Add(divider1);
            panel9.Dock = DockStyle.Fill;
            panel9.Location = new Point(0, 79);
            panel9.Name = "panel9";
            panel9.Size = new Size(650, 333);
            panel9.TabIndex = 7;
            // 
            // divider1
            // 
            divider1.Dock = DockStyle.Top;
            divider1.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            divider1.Location = new Point(0, 0);
            divider1.Margin = new Padding(10);
            divider1.Name = "divider1";
            divider1.Orientation = AntdUI.TOrientation.Left;
            divider1.Size = new Size(633, 22);
            divider1.TabIndex = 1;
            divider1.Text = "标准的进度条";
            // 
            // panel10
            // 
            panel10.Controls.Add(panel5);
            panel10.Controls.Add(panel3);
            panel10.Controls.Add(panel1);
            panel10.Dock = DockStyle.Top;
            panel10.Location = new Point(0, 22);
            panel10.Name = "panel10";
            panel10.Padding = new Padding(10, 0, 10, 0);
            panel10.Size = new Size(633, 105);
            panel10.TabIndex = 2;
            // 
            // divider2
            // 
            divider2.Dock = DockStyle.Top;
            divider2.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            divider2.Location = new Point(0, 127);
            divider2.Margin = new Padding(10);
            divider2.Name = "divider2";
            divider2.Orientation = AntdUI.TOrientation.Left;
            divider2.Size = new Size(633, 22);
            divider2.TabIndex = 3;
            divider2.Text = "圈形的进度";
            // 
            // divider3
            // 
            divider3.Dock = DockStyle.Top;
            divider3.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            divider3.Location = new Point(0, 269);
            divider3.Margin = new Padding(10);
            divider3.Name = "divider3";
            divider3.Orientation = AntdUI.TOrientation.Left;
            divider3.Size = new Size(633, 22);
            divider3.TabIndex = 19;
            divider3.Text = "响应式进度圈";
            // 
            // Progress
            // 
            Controls.Add(panel9);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            Name = "Progress";
            Size = new Size(650, 412);
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel5.ResumeLayout(false);
            panel6.ResumeLayout(false);
            panel8.ResumeLayout(false);
            progress5.ResumeLayout(false);
            progress6.ResumeLayout(false);
            panel7.ResumeLayout(false);
            panel9.ResumeLayout(false);
            panel10.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private AntdUI.Header header1;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel6;
        private AntdUI.Progress progress3;
        private AntdUI.Icon.IconError label3;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private AntdUI.Progress progress2;
        private AntdUI.Icon.IconComplete label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private AntdUI.Progress progress1;
        private Label label1;
        private AntdUI.Progress progress9;
        private AntdUI.Progress progress6;
        private AntdUI.Icon.IconError iError2;
        private AntdUI.Progress progress8;
        private AntdUI.Progress progress5;
        private AntdUI.Icon.IconComplete iComplete1;
        private AntdUI.Progress progress7;
        private AntdUI.Progress progress4;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Panel panel10;
        private AntdUI.Divider divider1;
        private AntdUI.Divider divider3;
        private AntdUI.Divider divider2;
    }
}