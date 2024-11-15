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
    partial class PageHeader
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
            pageHeader1 = new AntdUI.PageHeader();
            button1 = new AntdUI.Button();
            divider1 = new AntdUI.Divider();
            pageHeader2 = new AntdUI.PageHeader();
            pageHeader3 = new AntdUI.PageHeader();
            divider2 = new AntdUI.Divider();
            pageHeader4 = new AntdUI.PageHeader();
            pageHeader1.SuspendLayout();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Description = "页头位于页容器中，页容器顶部，起到了内容概览和引导页级操作的作用。包括由面包屑、标题、页面内容简介、页面级操作等、页面级导航组成。";
            header1.Dock = DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F);
            header1.LocalizationDescription = "PageHeader.Description";
            header1.LocalizationText = "PageHeader.Text";
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new Padding(0, 0, 0, 10);
            header1.Size = new Size(740, 74);
            header1.TabIndex = 0;
            header1.Text = "PageHeader 页头";
            header1.UseTitleFont = true;
            // 
            // pageHeader1
            // 
            pageHeader1.Controls.Add(button1);
            pageHeader1.Dock = DockStyle.Top;
            pageHeader1.Location = new Point(0, 102);
            pageHeader1.Name = "pageHeader1";
            pageHeader1.Padding = new Padding(0, 0, 8, 0);
            pageHeader1.Size = new Size(740, 36);
            pageHeader1.SubText = "This is a subtitle";
            pageHeader1.TabIndex = 1;
            pageHeader1.Text = "Title";
            // 
            // button1
            // 
            button1.Dock = DockStyle.Right;
            button1.LocalizationText = "PageHeader.Type";
            button1.Location = new Point(657, 0);
            button1.Name = "button1";
            button1.Size = new Size(75, 36);
            button1.TabIndex = 0;
            button1.Text = "显示返回";
            button1.ToggleType = AntdUI.TTypeMini.Primary;
            button1.Click += button1_Click;
            // 
            // divider1
            // 
            divider1.Dock = DockStyle.Top;
            divider1.Font = new Font("Microsoft YaHei UI", 10F);
            divider1.LocalizationText = "PageHeader.{id}";
            divider1.Location = new Point(0, 74);
            divider1.Name = "divider1";
            divider1.Orientation = AntdUI.TOrientation.Left;
            divider1.Size = new Size(740, 28);
            divider1.TabIndex = 0;
            divider1.Text = "标准样式";
            // 
            // pageHeader2
            // 
            pageHeader2.Description = "This is a description";
            pageHeader2.Dock = DockStyle.Top;
            pageHeader2.Location = new Point(0, 138);
            pageHeader2.Name = "pageHeader2";
            pageHeader2.Padding = new Padding(0, 0, 0, 6);
            pageHeader2.Size = new Size(740, 60);
            pageHeader2.SubText = "This is a subtitle";
            pageHeader2.TabIndex = 3;
            pageHeader2.Text = "Title";
            // 
            // pageHeader3
            // 
            pageHeader3.Dock = DockStyle.Top;
            pageHeader3.Location = new Point(0, 226);
            pageHeader3.Name = "pageHeader3";
            pageHeader3.Padding = new Padding(0, 0, 8, 0);
            pageHeader3.ShowButton = true;
            pageHeader3.ShowIcon = true;
            pageHeader3.Size = new Size(740, 36);
            pageHeader3.SubText = "This is a subtitle";
            pageHeader3.TabIndex = 5;
            pageHeader3.Text = "Title";
            // 
            // divider2
            // 
            divider2.Dock = DockStyle.Top;
            divider2.Font = new Font("Microsoft YaHei UI", 10F);
            divider2.LocalizationText = "PageHeader.{id}";
            divider2.Location = new Point(0, 198);
            divider2.Name = "divider2";
            divider2.Orientation = AntdUI.TOrientation.Left;
            divider2.Size = new Size(740, 28);
            divider2.TabIndex = 4;
            divider2.Text = "关闭按钮";
            // 
            // pageHeader4
            // 
            pageHeader4.Dock = DockStyle.Top;
            pageHeader4.Location = new Point(0, 262);
            pageHeader4.MaximizeBox = false;
            pageHeader4.Name = "pageHeader4";
            pageHeader4.Padding = new Padding(0, 0, 8, 0);
            pageHeader4.ShowButton = true;
            pageHeader4.ShowIcon = true;
            pageHeader4.Size = new Size(740, 36);
            pageHeader4.SubText = "This is a subtitle";
            pageHeader4.TabIndex = 6;
            pageHeader4.Text = "Title";
            // 
            // PageHeader
            // 
            Controls.Add(pageHeader4);
            Controls.Add(pageHeader3);
            Controls.Add(divider2);
            Controls.Add(pageHeader2);
            Controls.Add(pageHeader1);
            Controls.Add(divider1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 11F);
            Name = "PageHeader";
            Size = new Size(740, 402);
            pageHeader1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.PageHeader header1;
        private AntdUI.PageHeader pageHeader1;
        private AntdUI.Divider divider1;
        private AntdUI.Button button1;
        private AntdUI.PageHeader pageHeader2;
        private AntdUI.PageHeader pageHeader3;
        private AntdUI.Divider divider2;
        private AntdUI.PageHeader pageHeader4;
    }
}