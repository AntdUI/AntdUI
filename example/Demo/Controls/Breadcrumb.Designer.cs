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
    partial class Breadcrumb
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
            AntdUI.BreadcrumbItem breadcrumbItem1 = new AntdUI.BreadcrumbItem();
            AntdUI.BreadcrumbItem breadcrumbItem2 = new AntdUI.BreadcrumbItem();
            AntdUI.BreadcrumbItem breadcrumbItem3 = new AntdUI.BreadcrumbItem();
            AntdUI.BreadcrumbItem breadcrumbItem4 = new AntdUI.BreadcrumbItem();
            AntdUI.BreadcrumbItem breadcrumbItem5 = new AntdUI.BreadcrumbItem();
            AntdUI.BreadcrumbItem breadcrumbItem6 = new AntdUI.BreadcrumbItem();
            AntdUI.BreadcrumbItem breadcrumbItem7 = new AntdUI.BreadcrumbItem();
            header1 = new AntdUI.PageHeader();
            breadcrumb1 = new AntdUI.Breadcrumb();
            breadcrumb2 = new AntdUI.Breadcrumb();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Description = "显示当前页面在系统层级结构中的位置，并能向上返回。";
            header1.Dock = DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F);
            header1.LocalizationDescription = "Breadcrumb.Description";
            header1.LocalizationText = "Breadcrumb.Text";
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new Padding(0, 0, 0, 10);
            header1.Size = new Size(740, 74);
            header1.TabIndex = 0;
            header1.Text = "Breadcrumb 面包屑";
            header1.UseTitleFont = true;
            // 
            // breadcrumb1
            // 
            breadcrumb1.Dock = DockStyle.Top;
            breadcrumbItem1.Text = "Home";
            breadcrumbItem2.Text = "Application Center";
            breadcrumbItem3.Text = "Application List";
            breadcrumbItem4.Text = "An Application";
            breadcrumb1.Items.Add(breadcrumbItem1);
            breadcrumb1.Items.Add(breadcrumbItem2);
            breadcrumb1.Items.Add(breadcrumbItem3);
            breadcrumb1.Items.Add(breadcrumbItem4);
            breadcrumb1.Location = new Point(0, 74);
            breadcrumb1.Name = "breadcrumb1";
            breadcrumb1.Size = new Size(740, 40);
            breadcrumb1.TabIndex = 1;
            breadcrumb1.ItemClick += breadcrumb1_ItemClick;
            // 
            // breadcrumb2
            // 
            breadcrumb2.Dock = DockStyle.Top;
            breadcrumbItem5.IconSvg = "HomeOutlined";
            breadcrumbItem6.IconSvg = "UserOutlined";
            breadcrumbItem6.Text = "Application List";
            breadcrumbItem7.Text = "Application";
            breadcrumb2.Items.Add(breadcrumbItem5);
            breadcrumb2.Items.Add(breadcrumbItem6);
            breadcrumb2.Items.Add(breadcrumbItem7);
            breadcrumb2.Location = new Point(0, 114);
            breadcrumb2.Name = "breadcrumb2";
            breadcrumb2.Size = new Size(740, 40);
            breadcrumb2.TabIndex = 2;
            breadcrumb2.ItemClick += breadcrumb1_ItemClick;
            // 
            // Breadcrumb
            // 
            Controls.Add(breadcrumb2);
            Controls.Add(breadcrumb1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 12F);
            Name = "Breadcrumb";
            Size = new Size(740, 402);
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.PageHeader header1;
        private AntdUI.Breadcrumb breadcrumb1;
        private AntdUI.Breadcrumb breadcrumb2;
    }
}