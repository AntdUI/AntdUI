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

namespace Overview.Controls
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Menu));
            AntdUI.MenuItem menuItem3 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem4 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem5 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem6 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem7 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem8 = new AntdUI.MenuItem();
            header1 = new AntdUI.Header();
            menu1 = new AntdUI.Menu();
            menu2 = new AntdUI.Menu();
            divider1 = new AntdUI.Divider();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Dock = DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new Padding(6);
            header1.Size = new Size(1300, 79);
            header1.TabIndex = 5;
            header1.Text = "Menu 导航菜单";
            header1.TextDesc = "为页面和功能提供导航的菜单列表。";
            // 
            // menu1
            // 
            menu1.Dock = DockStyle.Left;
            menu1.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            menuItem2.IconSvg = resources.GetString("menuItem2.IconSvg");
            menuItem2.Text = "Option 1";
            menuItem3.IconSvg = resources.GetString("menuItem3.IconSvg");
            menuItem4.IconSvg = resources.GetString("menuItem4.IconSvg");
            menuItem4.Text = "Sub2 1";
            menuItem5.Text = "Sub2 2";
            menuItem6.Text = "Sub2 3";
            menuItem3.Sub.AddRange(new AntdUI.MenuItem[] { menuItem4, menuItem5, menuItem6 });
            menuItem3.Text = "Option 2";
            menuItem7.Text = "Option 3";
            menuItem1.Sub.AddRange(new AntdUI.MenuItem[] { menuItem2, menuItem3, menuItem7 });
            menuItem1.Text = "Navigation One";
            menuItem8.Text = "Navigation Two";
            menu1.Items.AddRange(new AntdUI.MenuItem[] { menuItem1, menuItem8 });
            menu1.Location = new Point(0, 79);
            menu1.Margin = new Padding(2);
            menu1.Name = "menu1";
            menu1.Size = new Size(278, 597);
            menu1.TabIndex = 10;
            menu1.Text = "menu1";
            // 
            // menu2
            // 
            menu2.Dock = DockStyle.Left;
            menu2.Indent = true;
            menu2.Location = new Point(304, 79);
            menu2.Name = "menu2";
            menu2.Size = new Size(278, 597);
            menu2.TabIndex = 11;
            menu2.Text = "menu2";
            // 
            // divider1
            // 
            divider1.Dock = DockStyle.Left;
            divider1.Location = new Point(278, 79);
            divider1.Name = "divider1";
            divider1.Size = new Size(26, 597);
            divider1.TabIndex = 12;
            divider1.Vertical = true;
            // 
            // Menu
            // 
            Controls.Add(menu2);
            Controls.Add(divider1);
            Controls.Add(menu1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 16F, FontStyle.Regular, GraphicsUnit.Point);
            Name = "Menu";
            Size = new Size(1300, 676);
            ResumeLayout(false);
        }

        #endregion
        private AntdUI.Header header1;
        private AntdUI.Menu menu1;
        private AntdUI.Menu menu2;
        private AntdUI.Divider divider1;
    }
}