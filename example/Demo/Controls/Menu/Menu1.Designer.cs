using System.Drawing;
using System.Windows.Forms;

namespace Demo.Controls
{
    partial class Menu1
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
            AntdUI.MenuItem menuItem1 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem2 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem3 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem4 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem5 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem6 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem7 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem8 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem9 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem10 = new AntdUI.MenuItem();
            menu1 = new AntdUI.Menu();
            SuspendLayout();
            // 
            // menu1
            // 
            menuItem1.IconSvg = "MailOutlined";
            menuItem1.ID = "mail";
            menuItem1.Select = true;
            menuItem1.Text = "Navigation One";
            menuItem2.Enabled = false;
            menuItem2.IconSvg = "AppstoreOutlined";
            menuItem2.ID = "app";
            menuItem2.Text = "Navigation Two";
            menuItem3.IconSvg = "SettingOutlined";
            menuItem3.ID = "SubMenu";
            menuItem4.Enabled = false;
            menuItem4.Text = "Item 1";
            menuItem5.Text = "Option 1";
            menuItem6.Text = "Option 2";
            menuItem7.Enabled = false;
            menuItem7.Text = "Item 2";
            menuItem8.Text = "Option 3";
            menuItem9.Text = "Option 4";
            menuItem3.Sub.Add(menuItem4);
            menuItem3.Sub.Add(menuItem5);
            menuItem3.Sub.Add(menuItem6);
            menuItem3.Sub.Add(menuItem7);
            menuItem3.Sub.Add(menuItem8);
            menuItem3.Sub.Add(menuItem9);
            menuItem3.Text = "Navigation Three - Submenu";
            menuItem10.ID = "link";
            menuItem10.Text = "Navigation Four - Link";
            menu1.Dock = DockStyle.Top;
            menu1.Font = new Font("Microsoft YaHei UI", 10F);
            menu1.Items.Add(menuItem1);
            menu1.Items.Add(menuItem2);
            menu1.Items.Add(menuItem3);
            menu1.Items.Add(menuItem10);
            menu1.Location = new Point(0, 0);
            menu1.Mode = AntdUI.TMenuMode.Horizontal;
            menu1.Name = "menu1";
            menu1.Radius = 0;
            menu1.Size = new Size(845, 46);
            menu1.TabIndex = 0;
            // 
            // Menu1
            // 
            Controls.Add(menu1);
            Name = "Menu1";
            Size = new Size(845, 46);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private AntdUI.Menu menu1;
    }
}
