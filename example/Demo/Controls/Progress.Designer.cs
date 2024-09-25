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
            header1 = new AntdUI.PageHeader();
            progress1 = new AntdUI.Progress();
            panel_l = new System.Windows.Forms.Panel();
            progress8 = new AntdUI.Progress();
            progress9 = new AntdUI.Progress();
            progress10 = new AntdUI.Progress();
            progress11 = new AntdUI.Progress();
            divider4 = new AntdUI.Divider();
            progress4 = new AntdUI.Progress();
            progress3 = new AntdUI.Progress();
            progress2 = new AntdUI.Progress();
            divider1 = new AntdUI.Divider();
            divider3 = new AntdUI.Divider();
            divider2 = new AntdUI.Divider();
            panel_main = new TableLayoutPanel();
            panel_r = new System.Windows.Forms.Panel();
            progress15 = new AntdUI.Progress();
            progress16 = new AntdUI.Progress();
            progress17 = new AntdUI.Progress();
            divider5 = new AntdUI.Divider();
            tableLayoutPanel2 = new TableLayoutPanel();
            progress14 = new AntdUI.Progress();
            progress13 = new AntdUI.Progress();
            progress12 = new AntdUI.Progress();
            tableLayoutPanel1 = new TableLayoutPanel();
            progress7 = new AntdUI.Progress();
            progress6 = new AntdUI.Progress();
            progress5 = new AntdUI.Progress();
            panel_l.SuspendLayout();
            panel_main.SuspendLayout();
            panel_r.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Description = "展示操作的当前进度。";
            header1.Dock = DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F);
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new Padding(0, 0, 0, 10);
            header1.Size = new Size(735, 74);
            header1.TabIndex = 0;
            header1.Text = "Progress 进度条";
            header1.UseTitleFont = true;
            // 
            // progress1
            // 
            progress1.ContainerControl = this;
            progress1.Dock = DockStyle.Top;
            progress1.Location = new Point(0, 22);
            progress1.Name = "progress1";
            progress1.Padding = new Padding(10, 0, 10, 0);
            progress1.Size = new Size(367, 30);
            progress1.TabIndex = 0;
            progress1.Value = 0.3F;
            // 
            // panel_l
            // 
            panel_l.AutoScroll = true;
            panel_l.Controls.Add(progress8);
            panel_l.Controls.Add(progress9);
            panel_l.Controls.Add(progress10);
            panel_l.Controls.Add(progress11);
            panel_l.Controls.Add(divider4);
            panel_l.Controls.Add(progress4);
            panel_l.Controls.Add(progress3);
            panel_l.Controls.Add(progress2);
            panel_l.Controls.Add(progress1);
            panel_l.Controls.Add(divider1);
            panel_l.Dock = DockStyle.Fill;
            panel_l.Location = new Point(0, 0);
            panel_l.Margin = new Padding(0);
            panel_l.Name = "panel_l";
            panel_l.Size = new Size(367, 375);
            panel_l.TabIndex = 0;
            // 
            // progress8
            // 
            progress8.ContainerControl = this;
            progress8.Dock = DockStyle.Top;
            progress8.Font = new Font("Microsoft YaHei UI", 8F);
            progress8.Location = new Point(0, 224);
            progress8.Name = "progress8";
            progress8.Padding = new Padding(10, 0, 80, 0);
            progress8.Size = new Size(367, 20);
            progress8.State = AntdUI.TType.Success;
            progress8.TabIndex = 26;
            progress8.Value = 1F;
            // 
            // progress9
            // 
            progress9.ContainerControl = this;
            progress9.Dock = DockStyle.Top;
            progress9.Font = new Font("Microsoft YaHei UI", 8F);
            progress9.Location = new Point(0, 204);
            progress9.Name = "progress9";
            progress9.Padding = new Padding(10, 0, 80, 0);
            progress9.Size = new Size(367, 20);
            progress9.State = AntdUI.TType.Error;
            progress9.TabIndex = 25;
            progress9.Value = 0.7F;
            // 
            // progress10
            // 
            progress10.ContainerControl = this;
            progress10.Dock = DockStyle.Top;
            progress10.Font = new Font("Microsoft YaHei UI", 8F);
            progress10.Loading = true;
            progress10.Location = new Point(0, 184);
            progress10.Name = "progress10";
            progress10.Padding = new Padding(10, 0, 80, 0);
            progress10.Size = new Size(367, 20);
            progress10.TabIndex = 24;
            progress10.Value = 0.5F;
            // 
            // progress11
            // 
            progress11.ContainerControl = this;
            progress11.Dock = DockStyle.Top;
            progress11.Font = new Font("Microsoft YaHei UI", 8F);
            progress11.Location = new Point(0, 164);
            progress11.Name = "progress11";
            progress11.Padding = new Padding(10, 0, 80, 0);
            progress11.Size = new Size(367, 20);
            progress11.TabIndex = 23;
            progress11.Value = 0.3F;
            // 
            // divider4
            // 
            divider4.Dock = DockStyle.Top;
            divider4.Font = new Font("Microsoft YaHei UI", 10F);
            divider4.Location = new Point(0, 142);
            divider4.Margin = new Padding(10);
            divider4.Name = "divider4";
            divider4.Orientation = AntdUI.TOrientation.Left;
            divider4.Size = new Size(367, 22);
            divider4.TabIndex = 19;
            divider4.Text = "小型进度条";
            // 
            // progress4
            // 
            progress4.ContainerControl = this;
            progress4.Dock = DockStyle.Top;
            progress4.Location = new Point(0, 112);
            progress4.Name = "progress4";
            progress4.Padding = new Padding(10, 0, 10, 0);
            progress4.Size = new Size(367, 30);
            progress4.State = AntdUI.TType.Success;
            progress4.TabIndex = 22;
            progress4.Value = 1F;
            // 
            // progress3
            // 
            progress3.ContainerControl = this;
            progress3.Dock = DockStyle.Top;
            progress3.Location = new Point(0, 82);
            progress3.Name = "progress3";
            progress3.Padding = new Padding(10, 0, 10, 0);
            progress3.Size = new Size(367, 30);
            progress3.State = AntdUI.TType.Error;
            progress3.TabIndex = 21;
            progress3.Value = 0.7F;
            // 
            // progress2
            // 
            progress2.ContainerControl = this;
            progress2.Dock = DockStyle.Top;
            progress2.Loading = true;
            progress2.Location = new Point(0, 52);
            progress2.Name = "progress2";
            progress2.Padding = new Padding(10, 0, 10, 0);
            progress2.Size = new Size(367, 30);
            progress2.TabIndex = 20;
            progress2.Value = 0.5F;
            // 
            // divider1
            // 
            divider1.Dock = DockStyle.Top;
            divider1.Font = new Font("Microsoft YaHei UI", 10F);
            divider1.Location = new Point(0, 0);
            divider1.Margin = new Padding(10);
            divider1.Name = "divider1";
            divider1.Orientation = AntdUI.TOrientation.Left;
            divider1.Size = new Size(367, 22);
            divider1.TabIndex = 1;
            divider1.Text = "标准的进度条";
            // 
            // divider3
            // 
            divider3.Dock = DockStyle.Top;
            divider3.Font = new Font("Microsoft YaHei UI", 10F);
            divider3.Location = new Point(0, 154);
            divider3.Margin = new Padding(10);
            divider3.Name = "divider3";
            divider3.Orientation = AntdUI.TOrientation.Left;
            divider3.Size = new Size(368, 22);
            divider3.TabIndex = 19;
            divider3.Text = "响应式进度圈";
            // 
            // divider2
            // 
            divider2.Dock = DockStyle.Top;
            divider2.Font = new Font("Microsoft YaHei UI", 10F);
            divider2.Location = new Point(0, 0);
            divider2.Margin = new Padding(10);
            divider2.Name = "divider2";
            divider2.Orientation = AntdUI.TOrientation.Left;
            divider2.Size = new Size(368, 22);
            divider2.TabIndex = 0;
            divider2.Text = "圈形的进度";
            // 
            // panel_main
            // 
            panel_main.ColumnCount = 2;
            panel_main.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            panel_main.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            panel_main.Controls.Add(panel_l, 0, 0);
            panel_main.Controls.Add(panel_r, 1, 0);
            panel_main.Dock = DockStyle.Fill;
            panel_main.Location = new Point(0, 74);
            panel_main.Name = "panel_main";
            panel_main.RowCount = 1;
            panel_main.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            panel_main.Size = new Size(735, 375);
            panel_main.TabIndex = 0;
            // 
            // panel_r
            // 
            panel_r.Controls.Add(progress15);
            panel_r.Controls.Add(progress16);
            panel_r.Controls.Add(progress17);
            panel_r.Controls.Add(divider5);
            panel_r.Controls.Add(tableLayoutPanel2);
            panel_r.Controls.Add(divider3);
            panel_r.Controls.Add(tableLayoutPanel1);
            panel_r.Controls.Add(divider2);
            panel_r.Dock = DockStyle.Fill;
            panel_r.Location = new Point(367, 0);
            panel_r.Margin = new Padding(0);
            panel_r.Name = "panel_r";
            panel_r.Size = new Size(368, 375);
            panel_r.TabIndex = 1;
            // 
            // progress15
            // 
            progress15.ContainerControl = this;
            progress15.Dock = DockStyle.Top;
            progress15.Location = new Point(0, 316);
            progress15.Name = "progress15";
            progress15.Padding = new Padding(10, 0, 10, 0);
            progress15.Shape = AntdUI.TShapeProgress.Steps;
            progress15.Size = new Size(368, 30);
            progress15.State = AntdUI.TType.Success;
            progress15.Steps = 6;
            progress15.StepSize = 2;
            progress15.TabIndex = 25;
            progress15.Value = 1F;
            // 
            // progress16
            // 
            progress16.ContainerControl = this;
            progress16.Dock = DockStyle.Top;
            progress16.Location = new Point(0, 286);
            progress16.Name = "progress16";
            progress16.Padding = new Padding(10, 0, 10, 0);
            progress16.Shape = AntdUI.TShapeProgress.Steps;
            progress16.Size = new Size(368, 30);
            progress16.Steps = 5;
            progress16.TabIndex = 24;
            progress16.Value = 0.3F;
            // 
            // progress17
            // 
            progress17.ContainerControl = this;
            progress17.Dock = DockStyle.Top;
            progress17.Location = new Point(0, 256);
            progress17.Name = "progress17";
            progress17.Padding = new Padding(10, 0, 10, 0);
            progress17.Shape = AntdUI.TShapeProgress.Steps;
            progress17.Size = new Size(368, 30);
            progress17.TabIndex = 23;
            progress17.Value = 0.5F;
            // 
            // divider5
            // 
            divider5.Dock = DockStyle.Top;
            divider5.Font = new Font("Microsoft YaHei UI", 10F);
            divider5.Location = new Point(0, 234);
            divider5.Margin = new Padding(10);
            divider5.Name = "divider5";
            divider5.Orientation = AntdUI.TOrientation.Left;
            divider5.Size = new Size(368, 22);
            divider5.TabIndex = 21;
            divider5.Text = "步骤进度条\r\n";
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 3;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel2.Controls.Add(progress14, 2, 0);
            tableLayoutPanel2.Controls.Add(progress13, 1, 0);
            tableLayoutPanel2.Controls.Add(progress12, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Top;
            tableLayoutPanel2.Location = new Point(0, 176);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.Padding = new Padding(0, 8, 0, 8);
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(368, 58);
            tableLayoutPanel2.TabIndex = 20;
            // 
            // progress14
            // 
            progress14.Anchor = AnchorStyles.None;
            progress14.Back = Color.FromArgb(40, 255, 79, 87);
            progress14.Location = new Point(264, 11);
            progress14.Margin = new Padding(0);
            progress14.Name = "progress14";
            progress14.Radius = 4;
            progress14.Shape = AntdUI.TShapeProgress.Mini;
            progress14.Size = new Size(84, 36);
            progress14.State = AntdUI.TType.Error;
            progress14.TabIndex = 13;
            progress14.Text = "Failed";
            progress14.UseSystemText = true;
            progress14.Value = 0.7F;
            // 
            // progress13
            // 
            progress13.Anchor = AnchorStyles.None;
            progress13.Back = Color.FromArgb(40, 0, 204, 0);
            progress13.Location = new Point(133, 11);
            progress13.Margin = new Padding(0);
            progress13.Name = "progress13";
            progress13.Radius = 4;
            progress13.Shape = AntdUI.TShapeProgress.Mini;
            progress13.Size = new Size(99, 36);
            progress13.State = AntdUI.TType.Success;
            progress13.TabIndex = 12;
            progress13.Text = "Success";
            progress13.UseSystemText = true;
            progress13.Value = 1F;
            // 
            // progress12
            // 
            progress12.Anchor = AnchorStyles.None;
            progress12.Back = Color.FromArgb(40, 22, 119, 255);
            progress12.Location = new Point(2, 11);
            progress12.Margin = new Padding(0);
            progress12.Name = "progress12";
            progress12.Radius = 4;
            progress12.Shape = AntdUI.TShapeProgress.Mini;
            progress12.Size = new Size(118, 36);
            progress12.TabIndex = 11;
            progress12.Text = "In Progress";
            progress12.UseSystemText = true;
            progress12.Value = 0.68F;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.Controls.Add(progress7, 2, 0);
            tableLayoutPanel1.Controls.Add(progress6, 1, 0);
            tableLayoutPanel1.Controls.Add(progress5, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Top;
            tableLayoutPanel1.Location = new Point(0, 22);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.Padding = new Padding(0, 8, 0, 8);
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(368, 132);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // progress7
            // 
            progress7.Dock = DockStyle.Fill;
            progress7.Font = new Font("Microsoft YaHei UI Light", 16F);
            progress7.Location = new Point(247, 11);
            progress7.Name = "progress7";
            progress7.Radius = 5;
            progress7.Shape = AntdUI.TShapeProgress.Circle;
            progress7.Size = new Size(118, 110);
            progress7.State = AntdUI.TType.Success;
            progress7.TabIndex = 13;
            progress7.Value = 1F;
            // 
            // progress6
            // 
            progress6.Dock = DockStyle.Fill;
            progress6.Font = new Font("Microsoft YaHei UI Light", 16F);
            progress6.Location = new Point(125, 11);
            progress6.Name = "progress6";
            progress6.Radius = 5;
            progress6.Shape = AntdUI.TShapeProgress.Circle;
            progress6.Size = new Size(116, 110);
            progress6.State = AntdUI.TType.Error;
            progress6.TabIndex = 12;
            progress6.Value = 0.7F;
            // 
            // progress5
            // 
            progress5.Dock = DockStyle.Fill;
            progress5.Font = new Font("Microsoft YaHei UI Light", 16F);
            progress5.Loading = true;
            progress5.Location = new Point(3, 11);
            progress5.Name = "progress5";
            progress5.Radius = 5;
            progress5.Shape = AntdUI.TShapeProgress.Circle;
            progress5.Size = new Size(116, 110);
            progress5.TabIndex = 11;
            progress5.Value = 0.75F;
            // 
            // Progress
            // 
            Controls.Add(panel_main);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 11F);
            Name = "Progress";
            Size = new Size(735, 449);
            panel_l.ResumeLayout(false);
            panel_main.ResumeLayout(false);
            panel_r.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private AntdUI.PageHeader header1;
        private AntdUI.Progress progress1;
        private System.Windows.Forms.Panel panel_l;
        private AntdUI.Divider divider1;
        private AntdUI.Divider divider3;
        private AntdUI.Divider divider2;
        private TableLayoutPanel panel_main;
        private System.Windows.Forms.Panel panel_r;
        private AntdUI.Progress progress2;
        private AntdUI.Progress progress3;
        private AntdUI.Progress progress4;
        private TableLayoutPanel tableLayoutPanel1;
        private AntdUI.Progress progress5;
        private AntdUI.Progress progress6;
        private AntdUI.Progress progress7;
        private AntdUI.Progress progress8;
        private AntdUI.Progress progress9;
        private AntdUI.Progress progress10;
        private AntdUI.Progress progress11;
        private AntdUI.Divider divider4;
        private TableLayoutPanel tableLayoutPanel2;
        private AntdUI.Progress progress12;
        private AntdUI.Progress progress13;
        private AntdUI.Progress progress14;
        private AntdUI.Progress progress15;
        private AntdUI.Progress progress16;
        private AntdUI.Progress progress17;
        private AntdUI.Divider divider5;
    }
}