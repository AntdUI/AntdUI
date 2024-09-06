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
            progress2 = new AntdUI.Progress();
            progress3 = new AntdUI.Progress();
            panel8 = new System.Windows.Forms.Panel();
            progress4 = new AntdUI.Progress();
            progress5 = new AntdUI.Progress();
            progress6 = new AntdUI.Progress();
            panel7 = new System.Windows.Forms.Panel();
            progress7 = new AntdUI.Progress();
            progress9 = new AntdUI.Progress();
            progress8 = new AntdUI.Progress();
            panel9 = new System.Windows.Forms.Panel();
            divider3 = new AntdUI.Divider();
            divider2 = new AntdUI.Divider();
            panel10 = new System.Windows.Forms.Panel();
            divider1 = new AntdUI.Divider();
            panel8.SuspendLayout();
            panel7.SuspendLayout();
            panel9.SuspendLayout();
            panel10.SuspendLayout();
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
            header1.Size = new Size(650, 74);
            header1.TabIndex = 0;
            header1.Text = "Progress 进度条";
            header1.UseTitleFont = true;
            // 
            // progress1
            // 
            progress1.ContainerControl = this;
            progress1.Dock = DockStyle.Top;
            progress1.Loading = true;
            progress1.Location = new Point(10, 0);
            progress1.Name = "progress1";
            progress1.ShowText = true;
            progress1.Size = new Size(613, 30);
            progress1.TabIndex = 0;
            progress1.Text = "%";
            progress1.Value = 0.5F;
            progress1.Click += Progress_Blue_1;
            // 
            // progress2
            // 
            progress2.ContainerControl = this;
            progress2.Dock = DockStyle.Top;
            progress2.Location = new Point(10, 30);
            progress2.Name = "progress2";
            progress2.ShowText = true;
            progress2.Size = new Size(613, 30);
            progress2.State = AntdUI.TType.Success;
            progress2.TabIndex = 1;
            progress2.Text = "%";
            progress2.Value = 1F;
            // 
            // progress3
            // 
            progress3.ContainerControl = this;
            progress3.Dock = DockStyle.Top;
            progress3.Location = new Point(10, 60);
            progress3.Name = "progress3";
            progress3.ShowText = true;
            progress3.Size = new Size(613, 30);
            progress3.State = AntdUI.TType.Error;
            progress3.TabIndex = 2;
            progress3.Text = "%";
            progress3.Value = 0.7F;
            progress3.Click += Progress_Red;
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
            progress4.ContainerControl = this;
            progress4.Font = new Font("Microsoft YaHei UI", 16F);
            progress4.Loading = true;
            progress4.Location = new Point(10, 7);
            progress4.Name = "progress4";
            progress4.Radius = 5;
            progress4.Shape = AntdUI.TShape.Circle;
            progress4.ShowText = true;
            progress4.Size = new Size(90, 90);
            progress4.TabIndex = 1;
            progress4.Text = "%";
            progress4.Value = 0.68F;
            progress4.Click += Progress_Blue_2;
            // 
            // progress5
            // 
            progress5.ContainerControl = this;
            progress5.Font = new Font("Microsoft YaHei UI", 16F);
            progress5.Location = new Point(158, 7);
            progress5.Name = "progress5";
            progress5.Radius = 5;
            progress5.Shape = AntdUI.TShape.Circle;
            progress5.ShowText = true;
            progress5.Size = new Size(90, 90);
            progress5.State = AntdUI.TType.Success;
            progress5.TabIndex = 2;
            progress5.Text = "%";
            progress5.Value = 1F;
            // 
            // progress6
            // 
            progress6.ContainerControl = this;
            progress6.Font = new Font("Microsoft YaHei UI", 16F);
            progress6.Location = new Point(306, 7);
            progress6.Name = "progress6";
            progress6.Radius = 5;
            progress6.Shape = AntdUI.TShape.Circle;
            progress6.ShowText = true;
            progress6.Size = new Size(90, 90);
            progress6.State = AntdUI.TType.Error;
            progress6.TabIndex = 3;
            progress6.Text = "%";
            progress6.Value = 0.7F;
            progress6.Click += Progress_Red;
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
            progress7.ContainerControl = this;
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
            progress9.ContainerControl = this;
            progress9.Location = new Point(297, 3);
            progress9.Mini = true;
            progress9.Name = "progress9";
            progress9.Radius = 4;
            progress9.Size = new Size(128, 36);
            progress9.State = AntdUI.TType.Error;
            progress9.TabIndex = 11;
            progress9.Text = "Failed";
            progress9.Value = 0.7F;
            progress9.Click += Progress_Red;
            // 
            // progress8
            // 
            progress8.ContainerControl = this;
            progress8.Location = new Point(164, 3);
            progress8.Mini = true;
            progress8.Name = "progress8";
            progress8.Radius = 4;
            progress8.Size = new Size(128, 36);
            progress8.State = AntdUI.TType.Success;
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
            panel9.Location = new Point(0, 74);
            panel9.Name = "panel9";
            panel9.Size = new Size(650, 338);
            panel9.TabIndex = 7;
            // 
            // divider3
            // 
            divider3.Dock = DockStyle.Top;
            divider3.Font = new Font("Microsoft YaHei UI", 10F);
            divider3.Location = new Point(0, 269);
            divider3.Margin = new Padding(10);
            divider3.Name = "divider3";
            divider3.Orientation = AntdUI.TOrientation.Left;
            divider3.Size = new Size(633, 22);
            divider3.TabIndex = 19;
            divider3.Text = "响应式进度圈";
            // 
            // divider2
            // 
            divider2.Dock = DockStyle.Top;
            divider2.Font = new Font("Microsoft YaHei UI", 10F);
            divider2.Location = new Point(0, 127);
            divider2.Margin = new Padding(10);
            divider2.Name = "divider2";
            divider2.Orientation = AntdUI.TOrientation.Left;
            divider2.Size = new Size(633, 22);
            divider2.TabIndex = 3;
            divider2.Text = "圈形的进度";
            // 
            // panel10
            // 
            panel10.Controls.Add(progress3);
            panel10.Controls.Add(progress2);
            panel10.Controls.Add(progress1);
            panel10.Dock = DockStyle.Top;
            panel10.Location = new Point(0, 22);
            panel10.Name = "panel10";
            panel10.Padding = new Padding(10, 0, 10, 0);
            panel10.Size = new Size(633, 105);
            panel10.TabIndex = 2;
            // 
            // divider1
            // 
            divider1.Dock = DockStyle.Top;
            divider1.Font = new Font("Microsoft YaHei UI", 10F);
            divider1.Location = new Point(0, 0);
            divider1.Margin = new Padding(10);
            divider1.Name = "divider1";
            divider1.Orientation = AntdUI.TOrientation.Left;
            divider1.Size = new Size(633, 22);
            divider1.TabIndex = 1;
            divider1.Text = "标准的进度条";
            // 
            // Progress
            // 
            Controls.Add(panel9);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 12F);
            Name = "Progress";
            Size = new Size(650, 412);
            panel8.ResumeLayout(false);
            panel7.ResumeLayout(false);
            panel9.ResumeLayout(false);
            panel10.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private AntdUI.PageHeader header1;
        private AntdUI.Progress progress3;
        private AntdUI.Progress progress2;
        private AntdUI.Progress progress1;
        private AntdUI.Progress progress9;
        private AntdUI.Progress progress6;
        private AntdUI.Progress progress8;
        private AntdUI.Progress progress5;
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