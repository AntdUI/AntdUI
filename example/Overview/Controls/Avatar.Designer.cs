﻿// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
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

namespace Overview.Controls
{
    partial class Avatar
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
            avatar4 = new AntdUI.Avatar();
            avatar1 = new AntdUI.Avatar();
            avatar6 = new AntdUI.Avatar();
            avatar9 = new AntdUI.Avatar();
            avatar10 = new AntdUI.Avatar();
            avatar3 = new AntdUI.Avatar();
            avatar2 = new AntdUI.Avatar();
            avatar11 = new AntdUI.Avatar();
            panel1 = new System.Windows.Forms.Panel();
            avatar12 = new AntdUI.Avatar();
            avatar5 = new AntdUI.Avatar();
            avatar7 = new AntdUI.Avatar();
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
            header1.Text = "Avatar 头像";
            header1.TextDesc = "用来代表用户或事物，支持图片、图标或字符展示。";
            // 
            // avatar4
            // 
            avatar4.Image = Properties.Resources.img1;
            avatar4.Location = new Point(8, 6);
            avatar4.Name = "avatar4";
            avatar4.Round = true;
            avatar4.Size = new Size(54, 54);
            avatar4.TabIndex = 7;
            // 
            // avatar1
            // 
            avatar1.Image = Properties.Resources.img1;
            avatar1.Location = new Point(68, 6);
            avatar1.Name = "avatar1";
            avatar1.Radius = 10;
            avatar1.Size = new Size(54, 54);
            avatar1.TabIndex = 8;
            // 
            // avatar6
            // 
            avatar6.Back = Color.FromArgb(253, 227, 207);
            avatar6.Badge = "1";
            avatar6.ForeColor = Color.FromArgb(245, 106, 0);
            avatar6.Location = new Point(128, 6);
            avatar6.Name = "avatar6";
            avatar6.Round = true;
            avatar6.Size = new Size(54, 54);
            avatar6.TabIndex = 9;
            avatar6.Text = "U";
            // 
            // avatar9
            // 
            avatar9.Back = Color.FromArgb(135, 208, 104);
            avatar9.Badge = "99+";
            avatar9.ForeColor = Color.White;
            avatar9.Location = new Point(188, 6);
            avatar9.Name = "avatar9";
            avatar9.Round = true;
            avatar9.Size = new Size(54, 54);
            avatar9.TabIndex = 9;
            avatar9.Text = "U";
            // 
            // avatar10
            // 
            avatar10.Back = Color.FromArgb(0, 144, 255);
            avatar10.Badge = "0";
            avatar10.Font = new Font("Microsoft YaHei UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            avatar10.ForeColor = Color.White;
            avatar10.Location = new Point(248, 6);
            avatar10.Name = "avatar10";
            avatar10.Radius = 10;
            avatar10.Size = new Size(54, 54);
            avatar10.TabIndex = 10;
            avatar10.Text = "名";
            // 
            // avatar3
            // 
            avatar3.Image = Properties.Resources.img1;
            avatar3.Location = new Point(8, 75);
            avatar3.Name = "avatar3";
            avatar3.Radius = 10;
            avatar3.Size = new Size(155, 60);
            avatar3.TabIndex = 2;
            // 
            // avatar2
            // 
            avatar2.Image = Properties.Resources.img1;
            avatar2.Location = new Point(169, 75);
            avatar2.Name = "avatar2";
            avatar2.Radius = 10;
            avatar2.Size = new Size(73, 126);
            avatar2.TabIndex = 4;
            // 
            // avatar11
            // 
            avatar11.Back = Color.FromArgb(0, 144, 255);
            avatar11.ForeColor = Color.White;
            avatar11.Location = new Point(248, 75);
            avatar11.Name = "avatar11";
            avatar11.Radius = 6;
            avatar11.Size = new Size(73, 126);
            avatar11.TabIndex = 11;
            avatar11.Text = "U";
            // 
            // panel1
            // 
            panel1.AutoScroll = true;
            panel1.Controls.Add(avatar5);
            panel1.Controls.Add(avatar7);
            panel1.Controls.Add(avatar4);
            panel1.Controls.Add(avatar1);
            panel1.Controls.Add(avatar11);
            panel1.Controls.Add(avatar6);
            panel1.Controls.Add(avatar2);
            panel1.Controls.Add(avatar12);
            panel1.Controls.Add(avatar9);
            panel1.Controls.Add(avatar10);
            panel1.Controls.Add(avatar3);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 79);
            panel1.Name = "panel1";
            panel1.Size = new Size(614, 367);
            panel1.TabIndex = 6;
            // 
            // avatar12
            // 
            avatar12.Back = Color.FromArgb(135, 208, 104);
            avatar12.Badge = "999+";
            avatar12.ForeColor = Color.White;
            avatar12.Location = new Point(8, 141);
            avatar12.Name = "avatar12";
            avatar12.Radius = 10;
            avatar12.Size = new Size(155, 60);
            avatar12.TabIndex = 9;
            avatar12.Text = "U";
            // 
            // avatar5
            // 
            avatar5.Image = Properties.Resources.img1;
            avatar5.Location = new Point(333, 6);
            avatar5.Margin = new Padding(2, 3, 2, 3);
            avatar5.Name = "avatar5";
            avatar5.Padding = new Padding(8);
            avatar5.Round = true;
            avatar5.Shadow = 8;
            avatar5.Size = new Size(60, 60);
            avatar5.TabIndex = 15;
            // 
            // avatar7
            // 
            avatar7.Back = Color.FromArgb(253, 227, 207);
            avatar7.Badge = "1";
            avatar7.ForeColor = Color.FromArgb(245, 106, 0);
            avatar7.Location = new Point(415, 6);
            avatar7.Margin = new Padding(2, 3, 2, 3);
            avatar7.Name = "avatar7";
            avatar7.Padding = new Padding(8);
            avatar7.Round = true;
            avatar7.Shadow = 8;
            avatar7.ShadowOffsetY = 4;
            avatar7.Size = new Size(60, 60);
            avatar7.TabIndex = 16;
            avatar7.Text = "U";
            // 
            // Avatar
            // 
            Controls.Add(panel1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 16F, FontStyle.Regular, GraphicsUnit.Point);
            Name = "Avatar";
            Size = new Size(614, 446);
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private AntdUI.Header header1;
        private AntdUI.Avatar avatar3;
        private AntdUI.Avatar avatar2;
        private AntdUI.Avatar avatar4;
        private AntdUI.Avatar avatar1;
        private AntdUI.Avatar avatar6;
        private AntdUI.Avatar avatar9;
        private AntdUI.Avatar avatar10;
        private AntdUI.Avatar avatar11;
        private System.Windows.Forms.Panel panel1;
        private AntdUI.Avatar avatar12;
        private AntdUI.Avatar avatar5;
        private AntdUI.Avatar avatar7;
    }
}