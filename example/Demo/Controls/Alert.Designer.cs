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
// GITCODE: https://gitcode.com/AntdUI/AntdUI
// GITEE: https://gitee.com/AntdUI/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

using System.Drawing;
using System.Windows.Forms;

namespace Demo.Controls
{
    partial class Alert
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
            alert14 = new AntdUI.Alert();
            alert13 = new AntdUI.Alert();
            divider3 = new AntdUI.Divider();
            panel3 = new System.Windows.Forms.Panel();
            alert12 = new AntdUI.Alert();
            alert11 = new AntdUI.Alert();
            alert10 = new AntdUI.Alert();
            alert9 = new AntdUI.Alert();
            divider2 = new AntdUI.Divider();
            panel2 = new System.Windows.Forms.Panel();
            alert8 = new AntdUI.Alert();
            alert7 = new AntdUI.Alert();
            alert6 = new AntdUI.Alert();
            alert5 = new AntdUI.Alert();
            alert4 = new AntdUI.Alert();
            alert3 = new AntdUI.Alert();
            alert2 = new AntdUI.Alert();
            alert1 = new AntdUI.Alert();
            divider1 = new AntdUI.Divider();
            panel1.SuspendLayout();
            panel3.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Description = "警告提示，展现需要关注的信息。";
            header1.Dock = DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F);
            header1.LocalizationDescription = "Alert.Description";
            header1.LocalizationText = "Alert.Text";
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new Padding(0, 0, 0, 10);
            header1.Size = new Size(614, 74);
            header1.TabIndex = 0;
            header1.Text = "Alert 警告提示";
            header1.UseTitleFont = true;
            // 
            // panel1
            // 
            panel1.AutoScroll = true;
            panel1.Controls.Add(alert14);
            panel1.Controls.Add(alert13);
            panel1.Controls.Add(divider3);
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(divider2);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(divider1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 74);
            panel1.Name = "panel1";
            panel1.Size = new Size(614, 606);
            panel1.TabIndex = 5;
            // 
            // alert14
            // 
            alert14.BackColor = Color.Black;
            alert14.Dock = DockStyle.Top;
            alert14.Font = new Font("楷体", 12F, FontStyle.Bold);
            alert14.ForeColor = Color.White;
            alert14.LocalizationText = "Alert.{id}";
            alert14.Location = new Point(0, 454);
            alert14.Loop = true;
            alert14.Name = "alert14";
            alert14.Radius = 0;
            alert14.Size = new Size(614, 40);
            alert14.TabIndex = 4;
            alert14.Text = "中国吉利 因快乐而伟大 老用户置换5000补贴，限时活动";
            // 
            // alert13
            // 
            alert13.Dock = DockStyle.Top;
            alert13.Icon = AntdUI.TType.Warn;
            alert13.Location = new Point(0, 424);
            alert13.Loop = true;
            alert13.Name = "alert13";
            alert13.Radius = 0;
            alert13.Size = new Size(614, 30);
            alert13.TabIndex = 3;
            alert13.Text = "I can be a React component, multiple React components, or just some text.";
            // 
            // divider3
            // 
            divider3.Dock = DockStyle.Top;
            divider3.Font = new Font("Microsoft YaHei UI", 10F);
            divider3.LocalizationText = "Alert.{id}";
            divider3.Location = new Point(0, 396);
            divider3.Name = "divider3";
            divider3.Orientation = AntdUI.TOrientation.Left;
            divider3.Size = new Size(614, 28);
            divider3.TabIndex = 0;
            divider3.Text = "轮播的公告";
            // 
            // panel3
            // 
            panel3.Controls.Add(alert12);
            panel3.Controls.Add(alert11);
            panel3.Controls.Add(alert10);
            panel3.Controls.Add(alert9);
            panel3.Dock = DockStyle.Top;
            panel3.Font = new Font("Microsoft YaHei UI", 10F);
            panel3.Location = new Point(0, 145);
            panel3.Name = "panel3";
            panel3.Size = new Size(614, 251);
            panel3.TabIndex = 2;
            // 
            // alert12
            // 
            alert12.BorderWidth = 1F;
            alert12.Icon = AntdUI.TType.Error;
            alert12.Location = new Point(293, 123);
            alert12.Name = "alert12";
            alert12.Size = new Size(264, 111);
            alert12.TabIndex = 3;
            alert12.Text = "Error Description Error Description Error Description Error Description";
            alert12.TextTitle = "Error Text";
            // 
            // alert11
            // 
            alert11.BorderWidth = 1F;
            alert11.Icon = AntdUI.TType.Warn;
            alert11.Location = new Point(293, 6);
            alert11.Name = "alert11";
            alert11.Size = new Size(264, 111);
            alert11.TabIndex = 1;
            alert11.Text = "Warning Description Warning Description Warning Description ";
            alert11.TextTitle = "Warning Text";
            // 
            // alert10
            // 
            alert10.BorderWidth = 1F;
            alert10.Icon = AntdUI.TType.Info;
            alert10.Location = new Point(16, 123);
            alert10.Name = "alert10";
            alert10.Size = new Size(264, 111);
            alert10.TabIndex = 2;
            alert10.Text = "Info Description Info Description Info Description Info Description";
            alert10.TextTitle = "Info Text";
            // 
            // alert9
            // 
            alert9.BorderWidth = 1F;
            alert9.Icon = AntdUI.TType.Success;
            alert9.Location = new Point(16, 6);
            alert9.Name = "alert9";
            alert9.Size = new Size(264, 111);
            alert9.TabIndex = 0;
            alert9.Text = "Success Description Success Description Success Description";
            alert9.TextTitle = "Success Text";
            // 
            // divider2
            // 
            divider2.Dock = DockStyle.Top;
            divider2.Font = new Font("Microsoft YaHei UI", 10F);
            divider2.LocalizationText = "Alert.{id}";
            divider2.Location = new Point(0, 117);
            divider2.Name = "divider2";
            divider2.Orientation = AntdUI.TOrientation.Left;
            divider2.Size = new Size(614, 28);
            divider2.TabIndex = 0;
            divider2.Text = "含有辅助性文字介绍";
            // 
            // panel2
            // 
            panel2.Controls.Add(alert8);
            panel2.Controls.Add(alert7);
            panel2.Controls.Add(alert6);
            panel2.Controls.Add(alert5);
            panel2.Controls.Add(alert4);
            panel2.Controls.Add(alert3);
            panel2.Controls.Add(alert2);
            panel2.Controls.Add(alert1);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 28);
            panel2.Name = "panel2";
            panel2.Size = new Size(614, 89);
            panel2.TabIndex = 1;
            // 
            // alert8
            // 
            alert8.Icon = AntdUI.TType.Error;
            alert8.Location = new Point(454, 43);
            alert8.Name = "alert8";
            alert8.Size = new Size(122, 31);
            alert8.TabIndex = 7;
            alert8.Text = "Error Text";
            // 
            // alert7
            // 
            alert7.Icon = AntdUI.TType.Warn;
            alert7.Location = new Point(301, 43);
            alert7.Name = "alert7";
            alert7.Size = new Size(148, 31);
            alert7.TabIndex = 6;
            alert7.Text = "Warning Text";
            // 
            // alert6
            // 
            alert6.Icon = AntdUI.TType.Info;
            alert6.Location = new Point(454, 6);
            alert6.Name = "alert6";
            alert6.Size = new Size(122, 31);
            alert6.TabIndex = 3;
            alert6.Text = "Info Text";
            // 
            // alert5
            // 
            alert5.Icon = AntdUI.TType.Success;
            alert5.Location = new Point(301, 6);
            alert5.Name = "alert5";
            alert5.Size = new Size(148, 31);
            alert5.TabIndex = 2;
            alert5.Text = "Success Text";
            // 
            // alert4
            // 
            alert4.BorderWidth = 1F;
            alert4.Icon = AntdUI.TType.Error;
            alert4.Location = new Point(169, 43);
            alert4.Name = "alert4";
            alert4.Size = new Size(122, 31);
            alert4.TabIndex = 5;
            alert4.Text = "Error Text";
            // 
            // alert3
            // 
            alert3.BorderWidth = 1F;
            alert3.Icon = AntdUI.TType.Warn;
            alert3.Location = new Point(16, 43);
            alert3.Name = "alert3";
            alert3.Size = new Size(148, 31);
            alert3.TabIndex = 4;
            alert3.Text = "Warning Text";
            // 
            // alert2
            // 
            alert2.BorderWidth = 1F;
            alert2.Icon = AntdUI.TType.Info;
            alert2.Location = new Point(169, 6);
            alert2.Name = "alert2";
            alert2.Size = new Size(122, 31);
            alert2.TabIndex = 1;
            alert2.Text = "Info Text";
            // 
            // alert1
            // 
            alert1.BorderWidth = 1F;
            alert1.Icon = AntdUI.TType.Success;
            alert1.Location = new Point(16, 6);
            alert1.Name = "alert1";
            alert1.Size = new Size(148, 31);
            alert1.TabIndex = 0;
            alert1.Text = "Success Text";
            // 
            // divider1
            // 
            divider1.Dock = DockStyle.Top;
            divider1.Font = new Font("Microsoft YaHei UI", 10F);
            divider1.LocalizationText = "Alert.{id}";
            divider1.Location = new Point(0, 0);
            divider1.Name = "divider1";
            divider1.Orientation = AntdUI.TOrientation.Left;
            divider1.Size = new Size(614, 28);
            divider1.TabIndex = 0;
            divider1.Text = "四种样式";
            // 
            // Alert
            // 
            Controls.Add(panel1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 12F);
            Name = "Alert";
            Size = new Size(614, 680);
            panel1.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.PageHeader header1;
        private System.Windows.Forms.Panel panel1;
        private AntdUI.Alert alert4;
        private AntdUI.Alert alert3;
        private AntdUI.Alert alert2;
        private AntdUI.Alert alert1;
        private AntdUI.Divider divider1;
        private AntdUI.Alert alert8;
        private AntdUI.Alert alert7;
        private AntdUI.Alert alert6;
        private AntdUI.Alert alert5;
        private AntdUI.Alert alert13;
        private AntdUI.Divider divider3;
        private System.Windows.Forms.Panel panel3;
        private AntdUI.Alert alert9;
        private AntdUI.Alert alert12;
        private AntdUI.Alert alert10;
        private AntdUI.Alert alert11;
        private AntdUI.Divider divider2;
        private System.Windows.Forms.Panel panel2;
        private AntdUI.Alert alert14;
    }
}