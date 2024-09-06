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
    partial class Badge
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
            tBadge1 = new AntdUI.Badge();
            tBadge2 = new AntdUI.Badge();
            tBadge3 = new AntdUI.Badge();
            tBadge4 = new AntdUI.Badge();
            tBadge5 = new AntdUI.Badge();
            panel1 = new System.Windows.Forms.Panel();
            panel3 = new System.Windows.Forms.Panel();
            tBadge6 = new AntdUI.Badge();
            tBadge7 = new AntdUI.Badge();
            tBadge8 = new AntdUI.Badge();
            tBadge9 = new AntdUI.Badge();
            tBadge10 = new AntdUI.Badge();
            panel5 = new System.Windows.Forms.Panel();
            panel6 = new System.Windows.Forms.Panel();
            tBadge21 = new AntdUI.Badge();
            tBadge22 = new AntdUI.Badge();
            tBadge23 = new AntdUI.Badge();
            tBadge24 = new AntdUI.Badge();
            tBadge25 = new AntdUI.Badge();
            tBadge26 = new AntdUI.Badge();
            tBadge27 = new AntdUI.Badge();
            tBadge28 = new AntdUI.Badge();
            tBadge29 = new AntdUI.Badge();
            tBadge30 = new AntdUI.Badge();
            header1 = new AntdUI.PageHeader();
            flowLayoutPanel1 = new FlowLayoutPanel();
            divider1 = new AntdUI.Divider();
            label2 = new Label();
            panel1.SuspendLayout();
            panel3.SuspendLayout();
            panel5.SuspendLayout();
            panel6.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // tBadge1
            // 
            tBadge1.Dock = DockStyle.Top;
            tBadge1.Location = new Point(6, 6);
            tBadge1.Name = "tBadge1";
            tBadge1.Size = new Size(202, 32);
            tBadge1.State = AntdUI.TState.Success;
            tBadge1.TabIndex = 0;
            tBadge1.Text = "Success";
            // 
            // tBadge2
            // 
            tBadge2.Dock = DockStyle.Top;
            tBadge2.Location = new Point(6, 38);
            tBadge2.Name = "tBadge2";
            tBadge2.Size = new Size(202, 32);
            tBadge2.State = AntdUI.TState.Error;
            tBadge2.TabIndex = 0;
            tBadge2.Text = "Error";
            // 
            // tBadge3
            // 
            tBadge3.Dock = DockStyle.Top;
            tBadge3.Location = new Point(6, 70);
            tBadge3.Name = "tBadge3";
            tBadge3.Size = new Size(202, 32);
            tBadge3.TabIndex = 0;
            tBadge3.Text = "Default";
            // 
            // tBadge4
            // 
            tBadge4.Dock = DockStyle.Top;
            tBadge4.Location = new Point(6, 102);
            tBadge4.Name = "tBadge4";
            tBadge4.Size = new Size(202, 32);
            tBadge4.State = AntdUI.TState.Processing;
            tBadge4.TabIndex = 0;
            tBadge4.Text = "Processing";
            // 
            // tBadge5
            // 
            tBadge5.Dock = DockStyle.Top;
            tBadge5.Location = new Point(6, 134);
            tBadge5.Name = "tBadge5";
            tBadge5.Size = new Size(202, 32);
            tBadge5.State = AntdUI.TState.Warn;
            tBadge5.TabIndex = 0;
            tBadge5.Text = "Warning";
            // 
            // panel1
            // 
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(tBadge5);
            panel1.Controls.Add(tBadge4);
            panel1.Controls.Add(tBadge3);
            panel1.Controls.Add(tBadge2);
            panel1.Controls.Add(tBadge1);
            panel1.Font = new Font("Microsoft YaHei UI", 16F);
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Padding = new Padding(6);
            panel1.Size = new Size(214, 223);
            panel1.TabIndex = 2;
            // 
            // panel3
            // 
            panel3.Controls.Add(tBadge6);
            panel3.Controls.Add(tBadge7);
            panel3.Controls.Add(tBadge8);
            panel3.Controls.Add(tBadge9);
            panel3.Controls.Add(tBadge10);
            panel3.Dock = DockStyle.Top;
            panel3.Font = new Font("Microsoft YaHei UI", 16F);
            panel3.Location = new Point(6, 166);
            panel3.Name = "panel3";
            panel3.Size = new Size(202, 48);
            panel3.TabIndex = 2;
            // 
            // tBadge6
            // 
            tBadge6.Dock = DockStyle.Left;
            tBadge6.Location = new Point(160, 0);
            tBadge6.Name = "tBadge6";
            tBadge6.Size = new Size(40, 48);
            tBadge6.State = AntdUI.TState.Warn;
            tBadge6.TabIndex = 1;
            // 
            // tBadge7
            // 
            tBadge7.Dock = DockStyle.Left;
            tBadge7.Location = new Point(120, 0);
            tBadge7.Name = "tBadge7";
            tBadge7.Size = new Size(40, 48);
            tBadge7.State = AntdUI.TState.Processing;
            tBadge7.TabIndex = 2;
            // 
            // tBadge8
            // 
            tBadge8.Dock = DockStyle.Left;
            tBadge8.Location = new Point(80, 0);
            tBadge8.Name = "tBadge8";
            tBadge8.Size = new Size(40, 48);
            tBadge8.TabIndex = 3;
            // 
            // tBadge9
            // 
            tBadge9.Dock = DockStyle.Left;
            tBadge9.Location = new Point(40, 0);
            tBadge9.Name = "tBadge9";
            tBadge9.Size = new Size(40, 48);
            tBadge9.State = AntdUI.TState.Error;
            tBadge9.TabIndex = 4;
            // 
            // tBadge10
            // 
            tBadge10.Dock = DockStyle.Left;
            tBadge10.Location = new Point(0, 0);
            tBadge10.Name = "tBadge10";
            tBadge10.Size = new Size(40, 48);
            tBadge10.State = AntdUI.TState.Success;
            tBadge10.TabIndex = 5;
            // 
            // panel5
            // 
            panel5.Controls.Add(panel6);
            panel5.Controls.Add(tBadge26);
            panel5.Controls.Add(tBadge27);
            panel5.Controls.Add(tBadge28);
            panel5.Controls.Add(tBadge29);
            panel5.Controls.Add(tBadge30);
            panel5.Font = new Font("Microsoft YaHei UI", 10F);
            panel5.Location = new Point(223, 3);
            panel5.Name = "panel5";
            panel5.Padding = new Padding(6);
            panel5.Size = new Size(214, 204);
            panel5.TabIndex = 2;
            // 
            // panel6
            // 
            panel6.Controls.Add(tBadge21);
            panel6.Controls.Add(tBadge22);
            panel6.Controls.Add(tBadge23);
            panel6.Controls.Add(tBadge24);
            panel6.Controls.Add(tBadge25);
            panel6.Dock = DockStyle.Top;
            panel6.Font = new Font("Microsoft YaHei UI", 16F);
            panel6.Location = new Point(6, 166);
            panel6.Name = "panel6";
            panel6.Size = new Size(202, 38);
            panel6.TabIndex = 2;
            // 
            // tBadge21
            // 
            tBadge21.Dock = DockStyle.Left;
            tBadge21.Location = new Point(160, 0);
            tBadge21.Name = "tBadge21";
            tBadge21.Size = new Size(40, 38);
            tBadge21.State = AntdUI.TState.Warn;
            tBadge21.TabIndex = 1;
            // 
            // tBadge22
            // 
            tBadge22.Dock = DockStyle.Left;
            tBadge22.Location = new Point(120, 0);
            tBadge22.Name = "tBadge22";
            tBadge22.Size = new Size(40, 38);
            tBadge22.State = AntdUI.TState.Processing;
            tBadge22.TabIndex = 2;
            // 
            // tBadge23
            // 
            tBadge23.Dock = DockStyle.Left;
            tBadge23.Location = new Point(80, 0);
            tBadge23.Name = "tBadge23";
            tBadge23.Size = new Size(40, 38);
            tBadge23.TabIndex = 3;
            // 
            // tBadge24
            // 
            tBadge24.Dock = DockStyle.Left;
            tBadge24.Location = new Point(40, 0);
            tBadge24.Name = "tBadge24";
            tBadge24.Size = new Size(40, 38);
            tBadge24.State = AntdUI.TState.Error;
            tBadge24.TabIndex = 4;
            // 
            // tBadge25
            // 
            tBadge25.Dock = DockStyle.Left;
            tBadge25.Location = new Point(0, 0);
            tBadge25.Name = "tBadge25";
            tBadge25.Size = new Size(40, 38);
            tBadge25.State = AntdUI.TState.Success;
            tBadge25.TabIndex = 5;
            // 
            // tBadge26
            // 
            tBadge26.Dock = DockStyle.Top;
            tBadge26.Location = new Point(6, 134);
            tBadge26.Name = "tBadge26";
            tBadge26.Size = new Size(202, 32);
            tBadge26.State = AntdUI.TState.Warn;
            tBadge26.TabIndex = 0;
            tBadge26.Text = "Warning";
            // 
            // tBadge27
            // 
            tBadge27.Dock = DockStyle.Top;
            tBadge27.Location = new Point(6, 102);
            tBadge27.Name = "tBadge27";
            tBadge27.Size = new Size(202, 32);
            tBadge27.State = AntdUI.TState.Processing;
            tBadge27.TabIndex = 0;
            tBadge27.Text = "Processing";
            // 
            // tBadge28
            // 
            tBadge28.Dock = DockStyle.Top;
            tBadge28.Location = new Point(6, 70);
            tBadge28.Name = "tBadge28";
            tBadge28.Size = new Size(202, 32);
            tBadge28.TabIndex = 0;
            tBadge28.Text = "Default";
            // 
            // tBadge29
            // 
            tBadge29.Dock = DockStyle.Top;
            tBadge29.Location = new Point(6, 38);
            tBadge29.Name = "tBadge29";
            tBadge29.Size = new Size(202, 32);
            tBadge29.State = AntdUI.TState.Error;
            tBadge29.TabIndex = 0;
            tBadge29.Text = "Error";
            // 
            // tBadge30
            // 
            tBadge30.Dock = DockStyle.Top;
            tBadge30.Location = new Point(6, 6);
            tBadge30.Name = "tBadge30";
            tBadge30.Size = new Size(202, 32);
            tBadge30.State = AntdUI.TState.Success;
            tBadge30.TabIndex = 0;
            tBadge30.Text = "Success";
            // 
            // header1
            // 
            header1.Description = "图标右上角的圆形徽标数字。";
            header1.Dock = DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F);
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new Padding(0, 0, 0, 10);
            header1.Size = new Size(543, 74);
            header1.TabIndex = 0;
            header1.Text = "Badge 徽标数";
            header1.UseTitleFont = true;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.Controls.Add(panel1);
            flowLayoutPanel1.Controls.Add(panel5);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(0, 96);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(543, 489);
            flowLayoutPanel1.TabIndex = 4;
            // 
            // divider1
            // 
            divider1.Dock = DockStyle.Top;
            divider1.Font = new Font("Microsoft YaHei UI", 10F);
            divider1.Location = new Point(0, 74);
            divider1.Name = "divider1";
            divider1.Orientation = AntdUI.TOrientation.Left;
            divider1.Size = new Size(543, 22);
            divider1.TabIndex = 5;
            divider1.Text = "基本";
            // 
            // label2
            // 
            label2.Dock = DockStyle.Top;
            label2.Location = new Point(6, 6);
            label2.Name = "label2";
            label2.Size = new Size(202, 40);
            label2.TabIndex = 1;
            label2.Text = "Light";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Badge
            // 
            Controls.Add(flowLayoutPanel1);
            Controls.Add(divider1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 16F);
            Name = "Badge";
            Size = new Size(543, 585);
            panel1.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel5.ResumeLayout(false);
            panel6.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.Badge tBadge1;
        private AntdUI.Badge tBadge2;
        private AntdUI.Badge tBadge3;
        private AntdUI.Badge tBadge4;
        private AntdUI.Badge tBadge5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private AntdUI.Badge tBadge6;
        private AntdUI.Badge tBadge7;
        private AntdUI.Badge tBadge8;
        private AntdUI.Badge tBadge9;
        private AntdUI.Badge tBadge10;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel6;
        private AntdUI.Badge tBadge21;
        private AntdUI.Badge tBadge22;
        private AntdUI.Badge tBadge23;
        private AntdUI.Badge tBadge24;
        private AntdUI.Badge tBadge25;
        private AntdUI.Badge tBadge26;
        private AntdUI.Badge tBadge27;
        private AntdUI.Badge tBadge28;
        private AntdUI.Badge tBadge29;
        private AntdUI.Badge tBadge30;
        private AntdUI.PageHeader header1;
        private FlowLayoutPanel flowLayoutPanel1;
        private AntdUI.Divider divider1;
        private Label label2;
    }
}