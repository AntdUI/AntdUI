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
    partial class Tag
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
            panel5 = new System.Windows.Forms.Panel();
            tag16 = new AntdUI.Tag();
            divider3 = new AntdUI.Divider();
            panel4 = new System.Windows.Forms.Panel();
            tag15 = new AntdUI.Tag();
            tag10 = new AntdUI.Tag();
            tag14 = new AntdUI.Tag();
            tag9 = new AntdUI.Tag();
            tag13 = new AntdUI.Tag();
            tag8 = new AntdUI.Tag();
            tag12 = new AntdUI.Tag();
            tag7 = new AntdUI.Tag();
            tag11 = new AntdUI.Tag();
            tag6 = new AntdUI.Tag();
            divider2 = new AntdUI.Divider();
            panel2 = new System.Windows.Forms.Panel();
            panel3 = new System.Windows.Forms.Panel();
            tag5 = new AntdUI.Tag();
            tag4 = new AntdUI.Tag();
            tag3 = new AntdUI.Tag();
            tag2 = new AntdUI.Tag();
            tag1 = new AntdUI.Tag();
            divider1 = new AntdUI.Divider();
            panel1.SuspendLayout();
            panel5.SuspendLayout();
            panel4.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Description = "进行标记和分类的小标签";
            header1.Dock = DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F);
            header1.LocalizationDescription = "Tag.Description";
            header1.LocalizationText = "Tag.Text";
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new Padding(0, 0, 0, 10);
            header1.Size = new Size(1300, 74);
            header1.TabIndex = 0;
            header1.Text = "Tag 标签";
            header1.UseTitleFont = true;
            // 
            // panel1
            // 
            panel1.AutoScroll = true;
            panel1.Controls.Add(panel5);
            panel1.Controls.Add(divider3);
            panel1.Controls.Add(panel4);
            panel1.Controls.Add(divider2);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(divider1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 74);
            panel1.Name = "panel1";
            panel1.Size = new Size(1300, 602);
            panel1.TabIndex = 5;
            // 
            // panel5
            // 
            panel5.Controls.Add(tag16);
            panel5.Dock = DockStyle.Top;
            panel5.Location = new Point(0, 231);
            panel5.Name = "panel5";
            panel5.Size = new Size(1300, 61);
            panel5.TabIndex = 6;
            // 
            // tag16
            // 
            tag16.AutoSizeMode = AntdUI.TAutoSize.Auto;
            tag16.BackColor = Color.FromArgb(59, 89, 153);
            tag16.ForeColor = Color.White;
            tag16.Image = Properties.Resources.img1;
            tag16.LocalizationText = "Tag.{id}";
            tag16.Location = new Point(13, 13);
            tag16.Name = "tag16";
            tag16.Size = new Size(107, 27);
            tag16.TabIndex = 0;
            tag16.Text = "自定义图标";
            // 
            // divider3
            // 
            divider3.Dock = DockStyle.Top;
            divider3.LocalizationText = "Tag.{id}";
            divider3.Location = new Point(0, 203);
            divider3.Name = "divider3";
            divider3.Orientation = AntdUI.TOrientation.Left;
            divider3.Size = new Size(1300, 28);
            divider3.TabIndex = 5;
            divider3.Text = "图标按钮";
            // 
            // panel4
            // 
            panel4.Controls.Add(tag15);
            panel4.Controls.Add(tag10);
            panel4.Controls.Add(tag14);
            panel4.Controls.Add(tag9);
            panel4.Controls.Add(tag13);
            panel4.Controls.Add(tag8);
            panel4.Controls.Add(tag12);
            panel4.Controls.Add(tag7);
            panel4.Controls.Add(tag11);
            panel4.Controls.Add(tag6);
            panel4.Dock = DockStyle.Top;
            panel4.Location = new Point(0, 115);
            panel4.Name = "panel4";
            panel4.Size = new Size(1300, 88);
            panel4.TabIndex = 4;
            // 
            // tag15
            // 
            tag15.AutoSizeMode = AntdUI.TAutoSize.Auto;
            tag15.BorderWidth = 0F;
            tag15.Location = new Point(265, 45);
            tag15.Name = "tag15";
            tag15.Size = new Size(65, 27);
            tag15.TabIndex = 2;
            tag15.Text = "default";
            // 
            // tag10
            // 
            tag10.AutoSizeMode = AntdUI.TAutoSize.Auto;
            tag10.Location = new Point(265, 13);
            tag10.Name = "tag10";
            tag10.Size = new Size(65, 27);
            tag10.TabIndex = 2;
            tag10.Text = "default";
            // 
            // tag14
            // 
            tag14.AutoSizeMode = AntdUI.TAutoSize.Auto;
            tag14.BorderWidth = 0F;
            tag14.Location = new Point(216, 45);
            tag14.Name = "tag14";
            tag14.Size = new Size(45, 27);
            tag14.TabIndex = 2;
            tag14.Text = "info";
            tag14.Type = AntdUI.TTypeMini.Info;
            // 
            // tag9
            // 
            tag9.AutoSizeMode = AntdUI.TAutoSize.Auto;
            tag9.Location = new Point(216, 13);
            tag9.Name = "tag9";
            tag9.Size = new Size(45, 27);
            tag9.TabIndex = 2;
            tag9.Text = "info";
            tag9.Type = AntdUI.TTypeMini.Info;
            // 
            // tag13
            // 
            tag13.AutoSizeMode = AntdUI.TAutoSize.Auto;
            tag13.BorderWidth = 0F;
            tag13.Location = new Point(139, 45);
            tag13.Name = "tag13";
            tag13.Size = new Size(72, 27);
            tag13.TabIndex = 2;
            tag13.Text = "warning";
            tag13.Type = AntdUI.TTypeMini.Warn;
            // 
            // tag8
            // 
            tag8.AutoSizeMode = AntdUI.TAutoSize.Auto;
            tag8.Location = new Point(139, 13);
            tag8.Name = "tag8";
            tag8.Size = new Size(72, 27);
            tag8.TabIndex = 2;
            tag8.Text = "warning";
            tag8.Type = AntdUI.TTypeMini.Warn;
            // 
            // tag12
            // 
            tag12.AutoSizeMode = AntdUI.TAutoSize.Auto;
            tag12.BorderWidth = 0F;
            tag12.Location = new Point(85, 45);
            tag12.Name = "tag12";
            tag12.Size = new Size(51, 27);
            tag12.TabIndex = 2;
            tag12.Text = "error";
            tag12.Type = AntdUI.TTypeMini.Error;
            // 
            // tag7
            // 
            tag7.AutoSizeMode = AntdUI.TAutoSize.Auto;
            tag7.Location = new Point(85, 13);
            tag7.Name = "tag7";
            tag7.Size = new Size(51, 27);
            tag7.TabIndex = 2;
            tag7.Text = "error";
            tag7.Type = AntdUI.TTypeMini.Error;
            // 
            // tag11
            // 
            tag11.AutoSizeMode = AntdUI.TAutoSize.Auto;
            tag11.BorderWidth = 0F;
            tag11.Location = new Point(13, 45);
            tag11.Name = "tag11";
            tag11.Size = new Size(68, 27);
            tag11.TabIndex = 2;
            tag11.Text = "success";
            tag11.Type = AntdUI.TTypeMini.Success;
            // 
            // tag6
            // 
            tag6.AutoSizeMode = AntdUI.TAutoSize.Auto;
            tag6.Location = new Point(13, 13);
            tag6.Name = "tag6";
            tag6.Size = new Size(68, 27);
            tag6.TabIndex = 2;
            tag6.Text = "success";
            tag6.Type = AntdUI.TTypeMini.Success;
            // 
            // divider2
            // 
            divider2.Dock = DockStyle.Top;
            divider2.LocalizationText = "Tag.{id}";
            divider2.Location = new Point(0, 87);
            divider2.Name = "divider2";
            divider2.Orientation = AntdUI.TOrientation.Left;
            divider2.Size = new Size(1300, 28);
            divider2.TabIndex = 3;
            divider2.Text = "多彩标签\r\n";
            // 
            // panel2
            // 
            panel2.Controls.Add(panel3);
            panel2.Controls.Add(tag2);
            panel2.Controls.Add(tag1);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 28);
            panel2.Name = "panel2";
            panel2.Size = new Size(1300, 59);
            panel2.TabIndex = 2;
            // 
            // panel3
            // 
            panel3.Controls.Add(tag5);
            panel3.Controls.Add(tag4);
            panel3.Controls.Add(tag3);
            panel3.Location = new Point(221, 13);
            panel3.Name = "panel3";
            panel3.Size = new Size(387, 26);
            panel3.TabIndex = 3;
            // 
            // tag5
            // 
            tag5.AutoSizeMode = AntdUI.TAutoSize.Auto;
            tag5.CloseIcon = true;
            tag5.Dock = DockStyle.Left;
            tag5.Location = new Point(148, 0);
            tag5.Name = "tag5";
            tag5.Padding = new Padding(0, 0, 6, 0);
            tag5.Size = new Size(74, 27);
            tag5.TabIndex = 3;
            tag5.Text = "Tag 3";
            // 
            // tag4
            // 
            tag4.AutoSizeMode = AntdUI.TAutoSize.Auto;
            tag4.CloseIcon = true;
            tag4.Dock = DockStyle.Left;
            tag4.Location = new Point(74, 0);
            tag4.Name = "tag4";
            tag4.Padding = new Padding(0, 0, 6, 0);
            tag4.Size = new Size(74, 27);
            tag4.TabIndex = 2;
            tag4.Text = "Tag 2";
            // 
            // tag3
            // 
            tag3.AutoSizeMode = AntdUI.TAutoSize.Auto;
            tag3.CloseIcon = true;
            tag3.Dock = DockStyle.Left;
            tag3.Location = new Point(0, 0);
            tag3.Name = "tag3";
            tag3.Padding = new Padding(0, 0, 6, 0);
            tag3.Size = new Size(74, 27);
            tag3.TabIndex = 1;
            tag3.Text = "Tag 1";
            // 
            // tag2
            // 
            tag2.AutoSizeMode = AntdUI.TAutoSize.Auto;
            tag2.CloseIcon = true;
            tag2.Location = new Point(72, 13);
            tag2.Name = "tag2";
            tag2.Size = new Size(140, 27);
            tag2.TabIndex = 2;
            tag2.Text = "Prevent Default";
            // 
            // tag1
            // 
            tag1.AutoSizeMode = AntdUI.TAutoSize.Auto;
            tag1.Location = new Point(13, 13);
            tag1.Name = "tag1";
            tag1.Size = new Size(55, 27);
            tag1.TabIndex = 1;
            tag1.Text = "Tag 1";
            // 
            // divider1
            // 
            divider1.Dock = DockStyle.Top;
            divider1.LocalizationText = "Tag.{id}";
            divider1.Location = new Point(0, 0);
            divider1.Name = "divider1";
            divider1.Orientation = AntdUI.TOrientation.Left;
            divider1.Size = new Size(1300, 28);
            divider1.TabIndex = 1;
            divider1.Text = "基本";
            // 
            // Tag
            // 
            Controls.Add(panel1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 10F);
            Name = "Tag";
            Size = new Size(1300, 676);
            panel1.ResumeLayout(false);
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.PageHeader header1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private AntdUI.Divider divider1;
        private AntdUI.Tag tag1;
        private AntdUI.Tag tag2;
        private System.Windows.Forms.Panel panel3;
        private AntdUI.Tag tag5;
        private AntdUI.Tag tag4;
        private AntdUI.Tag tag3;
        private System.Windows.Forms.Panel panel4;
        private AntdUI.Tag tag15;
        private AntdUI.Tag tag10;
        private AntdUI.Tag tag14;
        private AntdUI.Tag tag9;
        private AntdUI.Tag tag13;
        private AntdUI.Tag tag8;
        private AntdUI.Tag tag12;
        private AntdUI.Tag tag7;
        private AntdUI.Tag tag11;
        private AntdUI.Tag tag6;
        private AntdUI.Divider divider2;
        private System.Windows.Forms.Panel panel5;
        private AntdUI.Tag tag16;
        private AntdUI.Divider divider3;
    }
}