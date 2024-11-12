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
    partial class Menu
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
            AntdUI.MenuItem menuItem1 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem2 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem3 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem4 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem5 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem6 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem7 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem8 = new AntdUI.MenuItem();
            header1 = new AntdUI.PageHeader();
            menu1 = new AntdUI.Menu();
            menu2 = new AntdUI.Menu();
            divider1 = new AntdUI.Divider();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Description = "为页面和功能提供导航的菜单列表。";
            header1.Dock = DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F);
            header1.LocalizationDescription = "Menu.Description";
            header1.LocalizationText = "Menu.Text";
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new Padding(0, 0, 0, 10);
            header1.Size = new Size(1300, 74);
            header1.TabIndex = 0;
            header1.Text = "Menu 导航菜单";
            header1.UseTitleFont = true;
            // 
            // menu1
            // 
            menu1.Dock = DockStyle.Left;
            menu1.Font = new Font("Microsoft YaHei UI", 12F);
            menuItem1.PARENTITEM = null;
            menuItem2.PARENTITEM = menuItem1;
            menuItem2.Text = "Option 1";
            menuItem3.PARENTITEM = menuItem1;
            menuItem4.PARENTITEM = menuItem3;
            menuItem4.Text = "Sub2 1";
            menuItem5.PARENTITEM = menuItem3;
            menuItem5.Text = "Sub2 2";
            menuItem6.PARENTITEM = menuItem3;
            menuItem6.Text = "Sub2 3";
            menuItem3.Sub.Add(menuItem4);
            menuItem3.Sub.Add(menuItem5);
            menuItem3.Sub.Add(menuItem6);
            menuItem3.Text = "Option 2";
            menuItem7.PARENTITEM = menuItem1;
            menuItem7.Text = "Option 3";
            menuItem1.Sub.Add(menuItem2);
            menuItem1.Sub.Add(menuItem3);
            menuItem1.Sub.Add(menuItem7);
            menuItem1.Text = "Navigation One";
            menuItem8.PARENTITEM = null;
            menuItem8.Text = "Navigation Two";
            menu1.Items.Add(menuItem1);
            menu1.Items.Add(menuItem8);
            menu1.Location = new Point(0, 74);
            menu1.Margin = new Padding(2);
            menu1.Name = "menu1";
            menu1.Size = new Size(278, 602);
            menu1.TabIndex = 10;
            menu1.Text = "menu1";
            // 
            // menu2
            // 
            menu2.Dock = DockStyle.Left;
            menu2.Indent = true;
            menu2.Location = new Point(304, 74);
            menu2.Name = "menu2";
            menu2.Size = new Size(278, 602);
            menu2.TabIndex = 11;
            menu2.Text = "menu2";
            // 
            // divider1
            // 
            divider1.Dock = DockStyle.Left;
            divider1.Location = new Point(278, 74);
            divider1.Name = "divider1";
            divider1.Size = new Size(26, 602);
            divider1.TabIndex = 12;
            divider1.Vertical = true;
            // 
            // Menu
            // 
            Controls.Add(menu2);
            Controls.Add(divider1);
            Controls.Add(menu1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 16F);
            Name = "Menu";
            Size = new Size(1300, 676);
            ResumeLayout(false);
        }

        #endregion
        private AntdUI.PageHeader header1;
        private AntdUI.Menu menu1;
        private AntdUI.Menu menu2;
        private AntdUI.Divider divider1;
    }
}