using System.Drawing;
using System.Windows.Forms;

namespace Demo.Controls
{
    partial class Menu3
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
            AntdUI.MenuItem menuItem19 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem20 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem21 = new AntdUI.MenuItem();
            menu1 = new AntdUI.Menu();
            panel1 = new System.Windows.Forms.Panel();
            switch1 = new AntdUI.Switch();
            switch2 = new AntdUI.Switch();
            switch3 = new AntdUI.Switch();
            select1 = new AntdUI.Select();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // menu1
            // 
            menuItem1.IconSvg = "MailOutlined";
            menuItem1.Select = true;
            menuItem2.IconSvg = "MenuOutlined";
            menuItem2.Select = true;
            menuItem3.IconSvg = "CheckCircleOutlined";
            menuItem3.Select = true;
            menuItem3.Text = "Option 1";
            menuItem4.IconSvg = "ClockCircleOutlined";
            menuItem4.Text = "Option 2";
            menuItem2.Sub.Add(menuItem3);
            menuItem2.Sub.Add(menuItem4);
            menuItem2.Text = "Item 1";
            menuItem5.Expand = false;
            menuItem5.IconSvg = "InfoCircleOutlined";
            menuItem6.Text = "Option 3";
            menuItem7.Text = "Option 4";
            menuItem5.Sub.Add(menuItem6);
            menuItem5.Sub.Add(menuItem7);
            menuItem5.Text = "Item 2";
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
            menuItem20.Text = "Option 12";
            menuItem14.Sub.Add(menuItem15);
            menuItem14.Sub.Add(menuItem16);
            menuItem14.Sub.Add(menuItem17);
            menuItem14.Sub.Add(menuItem20);
            menuItem14.Text = "Navigation Three";
            menuItem18.Enabled = false;
            menuItem18.Text = "Group";
            menuItem19.Text = "Option 13";
            menuItem21.Text = "Option 14";
            menu1.Dock = DockStyle.Left;
            menu1.Font = new Font("Microsoft YaHei UI", 10F);
            menu1.Items.Add(menuItem1);
            menu1.Items.Add(menuItem8);
            menu1.Items.Add(menuItem14);
            menu1.Items.Add(menuItem18);
            menu1.Items.Add(menuItem19);
            menu1.Items.Add(menuItem21);
            menu1.Location = new Point(0, 0);
            menu1.Name = "menu1";
            menu1.ScrollBarBlock = true;
            menu1.Size = new Size(251, 460);
            menu1.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Controls.Add(select1);
            panel1.Controls.Add(switch3);
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
            // switch3
            // 
            switch3.CheckedText = "平展";
            switch3.LocalizationCheckedText = "Menu.flatten";
            switch3.LocalizationUnCheckedText = "Menu.indent";
            switch3.Location = new Point(267, 91);
            switch3.Name = "switch3";
            switch3.Size = new Size(84, 33);
            switch3.TabIndex = 4;
            switch3.UnCheckedText = "缩进";
            switch3.CheckedChanged += switch3_CheckedChanged;
            // 
            // select1
            // 
            select1.Items.AddRange(new object[] { "Hover", "Click" });
            select1.List = true;
            select1.Location = new Point(267, 130);
            select1.Name = "select1";
            select1.SelectedIndex = 1;
            select1.SelectedValue = "Tom";
            select1.Size = new Size(120, 41);
            select1.TabIndex = 5;
            select1.Text = "Hover";
            select1.SelectedIndexChanged += select1_SelectedIndexChanged;
            // 
            // Menu3
            // 
            AutoScroll = true;
            Controls.Add(panel1);
            Name = "Menu3";
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
        private AntdUI.Switch switch3;
        private AntdUI.Select select1;
    }
}
