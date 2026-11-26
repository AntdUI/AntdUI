using System.Drawing;
using System.Windows.Forms;

namespace Demo.Controls
{
    partial class Menu5
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
            AntdUI.MenuItem menuItem11 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem12 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem13 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem14 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem15 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem16 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem17 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem18 = new AntdUI.MenuItem();
            menu1 = new AntdUI.Menu();
            panel1 = new System.Windows.Forms.Panel();
            switch1 = new AntdUI.Switch();
            switch2 = new AntdUI.Switch();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // menu1
            // 
            menuItem1.IconSvg = "MailOutlined";
            menuItem2.Text = "Item 1";
            menuItem3.Text = "Option 1";
            menuItem4.Text = "Option 2";
            menuItem2.Sub.Add(menuItem3);
            menuItem2.Sub.Add(menuItem4);
            menuItem5.Text = "Item 2";
            menuItem6.Text = "Option 3";
            menuItem7.Text = "Option 4";
            menuItem5.Sub.Add(menuItem6);
            menuItem5.Sub.Add(menuItem7);
            menuItem1.Sub.Add(menuItem2);
            menuItem1.Sub.Add(menuItem5);
            menuItem1.Text = "Navigation One";
            menuItem8.Expand = false;
            menuItem8.IconSvg = "AppstoreOutlined";
            menuItem9.Text = "Option 5";
            menuItem10.Text = "Option 6";
            menuItem12.Text = "Option 7";
            menuItem13.Text = "Option 8";
            menuItem11.Sub.Add(menuItem12);
            menuItem11.Sub.Add(menuItem13);
            menuItem11.Text = "Submenu";
            menuItem8.Sub.Add(menuItem9);
            menuItem8.Sub.Add(menuItem10);
            menuItem8.Sub.Add(menuItem11);
            menuItem8.Text = "Navigation Two";
            menuItem14.Expand = false;
            menuItem14.IconSvg = "SettingOutlined";
            menuItem15.Text = "Option 9";
            menuItem16.Text = "Option 10";
            menuItem17.Text = "Option 11";
            menuItem18.Text = "Option 12";
            menuItem14.Sub.Add(menuItem15);
            menuItem14.Sub.Add(menuItem16);
            menuItem14.Sub.Add(menuItem17);
            menuItem14.Sub.Add(menuItem18);
            menuItem14.Text = "Navigation Three";
            menu1.Dock = DockStyle.Left;
            menu1.Font = new Font("Microsoft YaHei UI", 10F);
            menu1.Items.Add(menuItem1);
            menu1.Items.Add(menuItem8);
            menu1.Items.Add(menuItem14);
            menu1.Location = new Point(0, 0);
            menu1.Mode = AntdUI.TMenuMode.Vertical;
            menu1.Name = "menu1";
            menu1.ScrollBarBlock = true;
            menu1.Size = new Size(247, 460);
            menu1.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Controls.Add(switch2);
            panel1.Controls.Add(switch1);
            panel1.Controls.Add(menu1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(650, 460);
            panel1.TabIndex = 1;
            // 
            // switch1
            // 
            switch1.CheckedText = "Dark";
            switch1.Location = new Point(267, 13);
            switch1.Name = "switch1";
            switch1.Size = new Size(84, 33);
            switch1.TabIndex = 2;
            switch1.UnCheckedText = "Light";
            switch1.CheckedChanged += switch1_CheckedChanged;
            // 
            // switch2
            // 
            switch2.CheckedText = "折叠";
            switch2.LocalizationCheckedText = "Menu.collapse";
            switch2.LocalizationUnCheckedText = "Menu.expand";
            switch2.Location = new Point(267, 52);
            switch2.Name = "switch2";
            switch2.Size = new Size(84, 33);
            switch2.TabIndex = 3;
            switch2.UnCheckedText = "展开";
            switch2.CheckedChanged += switch2_CheckedChanged;
            // 
            // Menu5
            // 
            AutoScroll = true;
            Controls.Add(panel1);
            Name = "Menu5";
            Size = new Size(650, 460);
            panel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private AntdUI.Menu menu1;
        private System.Windows.Forms.Panel panel1;
        private AntdUI.Switch switch1;
        private AntdUI.Switch switch2;
    }
}
