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
// GITEE: https://gitee.com/AntdUI/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

using System.Drawing;
using System.Windows.Forms;

namespace Demo.Controls
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

        private void InitializeComponent()
        {
            header1 = new AntdUI.PageHeader();
            panel1 = new System.Windows.Forms.Panel();
            avatar11 = new AntdUI.Avatar();
            avatar10 = new AntdUI.Avatar();
            avatar9 = new AntdUI.Avatar();
            avatar8 = new AntdUI.Avatar();
            avatar7 = new AntdUI.Avatar();
            avatar6 = new AntdUI.Avatar();
            avatar5 = new AntdUI.Avatar();
            avatar4 = new AntdUI.Avatar();
            avatar3 = new AntdUI.Avatar();
            avatar2 = new AntdUI.Avatar();
            avatar1 = new AntdUI.Avatar();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Description = "用来代表用户或事物，支持图片、图标或字符展示。";
            header1.Dock = DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F);
            header1.LocalizationDescription = "Avatar.Description";
            header1.LocalizationText = "Avatar.Text";
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new Padding(0, 0, 0, 10);
            header1.Size = new Size(614, 74);
            header1.TabIndex = 0;
            header1.Text = "Avatar 头像";
            header1.UseTitleFont = true;
            // 
            // panel1
            // 
            panel1.AutoScroll = true;
            panel1.Controls.Add(avatar11);
            panel1.Controls.Add(avatar10);
            panel1.Controls.Add(avatar9);
            panel1.Controls.Add(avatar8);
            panel1.Controls.Add(avatar7);
            panel1.Controls.Add(avatar6);
            panel1.Controls.Add(avatar5);
            panel1.Controls.Add(avatar4);
            panel1.Controls.Add(avatar3);
            panel1.Controls.Add(avatar2);
            panel1.Controls.Add(avatar1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 74);
            panel1.Name = "panel1";
            panel1.Size = new Size(614, 372);
            panel1.TabIndex = 0;
            // 
            // avatar11
            // 
            avatar11.BackColor = Color.FromArgb(0, 144, 255);
            avatar11.ForeColor = Color.White;
            avatar11.Location = new Point(248, 75);
            avatar11.Name = "avatar11";
            avatar11.Radius = 6;
            avatar11.Size = new Size(73, 126);
            avatar11.TabIndex = 10;
            avatar11.Text = "U";
            // 
            // avatar10
            // 
            avatar10.Image = Properties.Resources.img1;
            avatar10.Location = new Point(169, 75);
            avatar10.Name = "avatar10";
            avatar10.Radius = 10;
            avatar10.Size = new Size(73, 126);
            avatar10.TabIndex = 9;
            // 
            // avatar9
            // 
            avatar9.BackColor = Color.FromArgb(135, 208, 104);
            avatar9.Badge = "999+";
            avatar9.ForeColor = Color.White;
            avatar9.Location = new Point(8, 141);
            avatar9.Name = "avatar9";
            avatar9.Radius = 10;
            avatar9.Size = new Size(155, 60);
            avatar9.TabIndex = 8;
            avatar9.Text = "U";
            // 
            // avatar8
            // 
            avatar8.Image = Properties.Resources.img1;
            avatar8.Location = new Point(8, 75);
            avatar8.Name = "avatar8";
            avatar8.Radius = 10;
            avatar8.Size = new Size(155, 60);
            avatar8.TabIndex = 7;
            // 
            // avatar7
            // 
            avatar7.BackColor = Color.FromArgb(253, 227, 207);
            avatar7.Badge = "2";
            avatar7.ForeColor = Color.FromArgb(245, 106, 0);
            avatar7.Location = new Point(415, 6);
            avatar7.Name = "avatar7";
            avatar7.Padding = new Padding(8, 6, 8, 10);
            avatar7.Round = true;
            avatar7.Shadow = 8;
            avatar7.ShadowOffsetY = 4;
            avatar7.Size = new Size(60, 60);
            avatar7.TabIndex = 6;
            avatar7.Text = "U";
            // 
            // avatar6
            // 
            avatar6.Image = Properties.Resources.img1;
            avatar6.Location = new Point(333, 6);
            avatar6.Margin = new Padding(2, 3, 2, 3);
            avatar6.Name = "avatar6";
            avatar6.Padding = new Padding(8);
            avatar6.Round = true;
            avatar6.Shadow = 8;
            avatar6.Size = new Size(60, 60);
            avatar6.TabIndex = 5;
            // 
            // avatar5
            // 
            avatar5.BackColor = Color.FromArgb(0, 144, 255);
            avatar5.Badge = " ";
            avatar5.ForeColor = Color.White;
            avatar5.LocalizationText = "Avatar.{id}";
            avatar5.Location = new Point(248, 6);
            avatar5.Name = "avatar5";
            avatar5.Radius = 10;
            avatar5.Size = new Size(54, 54);
            avatar5.TabIndex = 4;
            avatar5.Text = "名";
            // 
            // avatar4
            // 
            avatar4.BackColor = Color.FromArgb(135, 208, 104);
            avatar4.Badge = "99+";
            avatar4.ForeColor = Color.White;
            avatar4.Location = new Point(188, 6);
            avatar4.Name = "avatar4";
            avatar4.Round = true;
            avatar4.Size = new Size(54, 54);
            avatar4.TabIndex = 3;
            avatar4.Text = "U";
            // 
            // avatar3
            // 
            avatar3.BackColor = Color.FromArgb(253, 227, 207);
            avatar3.Badge = "1";
            avatar3.ForeColor = Color.FromArgb(245, 106, 0);
            avatar3.Location = new Point(128, 6);
            avatar3.Name = "avatar3";
            avatar3.Round = true;
            avatar3.Size = new Size(54, 54);
            avatar3.TabIndex = 2;
            avatar3.Text = "U";
            // 
            // avatar2
            // 
            avatar2.Image = Properties.Resources.img1;
            avatar2.Location = new Point(68, 6);
            avatar2.Name = "avatar2";
            avatar2.Radius = 10;
            avatar2.Size = new Size(54, 54);
            avatar2.TabIndex = 1;
            // 
            // avatar1
            // 
            avatar1.Image = Properties.Resources.img1;
            avatar1.Location = new Point(8, 6);
            avatar1.Name = "avatar1";
            avatar1.Round = true;
            avatar1.Size = new Size(54, 54);
            avatar1.TabIndex = 0;
            // 
            // Avatar
            // 
            Controls.Add(panel1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 14F);
            Name = "Avatar";
            Size = new Size(614, 446);
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.PageHeader header1;
        private System.Windows.Forms.Panel panel1;
        private AntdUI.Avatar avatar1;
        private AntdUI.Avatar avatar2;
        private AntdUI.Avatar avatar3;
        private AntdUI.Avatar avatar4;
        private AntdUI.Avatar avatar5;
        private AntdUI.Avatar avatar6;
        private AntdUI.Avatar avatar7;
        private AntdUI.Avatar avatar8;
        private AntdUI.Avatar avatar9;
        private AntdUI.Avatar avatar10;
        private AntdUI.Avatar avatar11;
    }
}