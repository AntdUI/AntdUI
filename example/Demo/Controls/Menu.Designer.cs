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
            AntdUI.MenuItem menuItem22 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem23 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem24 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem25 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem26 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem27 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem28 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem29 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem30 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem31 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem32 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem33 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem34 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem35 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem36 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem37 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem38 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem39 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem40 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem41 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem42 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem43 = new AntdUI.MenuItem();
            header1 = new AntdUI.PageHeader();
            divider1 = new AntdUI.Divider();
            menu1 = new AntdUI.Menu();
            divider2 = new AntdUI.Divider();
            menu2 = new AntdUI.Menu();
            tableLayoutPanel1 = new TableLayoutPanel();
            panel2 = new System.Windows.Forms.Panel();
            switch4 = new AntdUI.Switch();
            menu3 = new AntdUI.Menu();
            switch3 = new AntdUI.Switch();
            panel1 = new System.Windows.Forms.Panel();
            switch5 = new AntdUI.Switch();
            switch2 = new AntdUI.Switch();
            switch1 = new AntdUI.Switch();
            divider3 = new AntdUI.Divider();
            tableLayoutPanel1.SuspendLayout();
            panel2.SuspendLayout();
            panel1.SuspendLayout();
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
            // divider1
            // 
            divider1.Dock = DockStyle.Top;
            divider1.Font = new Font("Microsoft YaHei UI", 10F);
            divider1.LocalizationText = "Menu.{id}";
            divider1.Location = new Point(0, 74);
            divider1.Name = "divider1";
            divider1.Orientation = AntdUI.TOrientation.Left;
            divider1.Size = new Size(1300, 28);
            divider1.TabIndex = 1;
            divider1.Text = "顶部导航";
            // 
            // menu1
            // 
            menu1.Dock = DockStyle.Top;
            menu1.Font = new Font("Microsoft YaHei UI", 10F);
            menuItem1.IconSvg = "MailOutlined";
            menuItem1.ID = "mail";
            menuItem1.Select = true;
            menuItem1.Text = "Navigation One";
            menuItem2.IconSvg = "AppstoreOutlined";
            menuItem2.ID = "app";
            menuItem2.Text = "Navigation Two";
            menuItem3.IconSvg = "SettingOutlined";
            menuItem3.ID = "SubMenu";
            menuItem4.Text = "Option 1";
            menuItem5.Text = "Option 2";
            menuItem6.Text = "Option 3";
            menuItem7.Text = "Option 4";
            menuItem3.Sub.Add(menuItem4);
            menuItem3.Sub.Add(menuItem5);
            menuItem3.Sub.Add(menuItem6);
            menuItem3.Sub.Add(menuItem7);
            menuItem3.Text = "Navigation Three - Submenu";
            menu1.Items.Add(menuItem1);
            menu1.Items.Add(menuItem2);
            menu1.Items.Add(menuItem3);
            menu1.Location = new Point(0, 102);
            menu1.Mode = AntdUI.TMenuMode.Horizontal;
            menu1.Name = "menu1";
            menu1.Radius = 0;
            menu1.Size = new Size(1300, 46);
            menu1.TabIndex = 2;
            // 
            // divider2
            // 
            divider2.Dock = DockStyle.Fill;
            divider2.Font = new Font("Microsoft YaHei UI", 10F);
            divider2.LocalizationText = "Menu.{id}";
            divider2.Location = new Point(0, 40);
            divider2.Margin = new Padding(0);
            divider2.Name = "divider2";
            divider2.Orientation = AntdUI.TOrientation.Left;
            divider2.Size = new Size(650, 28);
            divider2.TabIndex = 3;
            divider2.Text = "内嵌菜单";
            // 
            // menu2
            // 
            menu2.Dock = DockStyle.Left;
            menuItem8.IconSvg = "MailOutlined";
            menuItem8.Select = true;
            menuItem9.IconSvg = "MenuOutlined";
            menuItem9.Select = true;
            menuItem10.IconSvg = "CheckCircleOutlined";
            menuItem10.Select = true;
            menuItem10.Text = "Option 1";
            menuItem11.IconSvg = "ClockCircleOutlined";
            menuItem11.Text = "Option 2";
            menuItem9.Sub.Add(menuItem10);
            menuItem9.Sub.Add(menuItem11);
            menuItem9.Text = "Item 1";
            menuItem12.Expand = false;
            menuItem12.IconSvg = "InfoCircleOutlined";
            menuItem13.Text = "Option 3";
            menuItem14.Text = "Option 4";
            menuItem12.Sub.Add(menuItem13);
            menuItem12.Sub.Add(menuItem14);
            menuItem12.Text = "Item 2";
            menuItem8.Sub.Add(menuItem9);
            menuItem8.Sub.Add(menuItem12);
            menuItem8.Text = "Navigation One";
            menuItem15.Expand = false;
            menuItem15.IconSvg = "AppstoreOutlined";
            menuItem16.Text = "Option 5";
            menuItem17.Text = "Option 6";
            menuItem19.Text = "Option 7";
            menuItem20.Text = "Option 8";
            menuItem18.Sub.Add(menuItem19);
            menuItem18.Sub.Add(menuItem20);
            menuItem18.Text = "Submenu";
            menuItem15.Sub.Add(menuItem16);
            menuItem15.Sub.Add(menuItem17);
            menuItem15.Sub.Add(menuItem18);
            menuItem15.Text = "Navigation Two";
            menuItem21.Expand = false;
            menuItem21.IconSvg = "SettingOutlined";
            menuItem22.Text = "Option 9";
            menuItem23.Text = "Option 10";
            menuItem24.Text = "Option 11";
            menuItem25.Text = "Option 12";
            menuItem21.Sub.Add(menuItem22);
            menuItem21.Sub.Add(menuItem23);
            menuItem21.Sub.Add(menuItem24);
            menuItem21.Sub.Add(menuItem25);
            menuItem21.Text = "Navigation Three";
            menu2.Items.Add(menuItem8);
            menu2.Items.Add(menuItem15);
            menu2.Items.Add(menuItem21);
            menu2.Location = new Point(0, 0);
            menu2.Name = "menu2";
            menu2.ScrollBarBlock = true;
            menu2.Size = new Size(251, 460);
            menu2.TabIndex = 4;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(panel2, 1, 1);
            tableLayoutPanel1.Controls.Add(panel1, 0, 1);
            tableLayoutPanel1.Controls.Add(divider2, 0, 0);
            tableLayoutPanel1.Controls.Add(divider3, 1, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 148);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.Padding = new Padding(0, 40, 0, 0);
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(1300, 528);
            tableLayoutPanel1.TabIndex = 5;
            // 
            // panel2
            // 
            panel2.Controls.Add(switch4);
            panel2.Controls.Add(menu3);
            panel2.Controls.Add(switch3);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(650, 68);
            panel2.Margin = new Padding(0);
            panel2.Name = "panel2";
            panel2.Size = new Size(650, 460);
            panel2.TabIndex = 0;
            // 
            // switch4
            // 
            switch4.CheckedText = "折叠";
            switch4.LocalizationCheckedText = "Menu.collapse";
            switch4.LocalizationUnCheckedText = "Menu.expand";
            switch4.Location = new Point(267, 52);
            switch4.Name = "switch4";
            switch4.Size = new Size(84, 33);
            switch4.TabIndex = 5;
            switch4.UnCheckedText = "展开";
            switch4.CheckedChanged += switch4_CheckedChanged;
            // 
            // menu3
            // 
            menu3.Dock = DockStyle.Left;
            menuItem26.IconSvg = "MailOutlined";
            menuItem28.Text = "Option 1";
            menuItem29.Text = "Option 2";
            menuItem27.Sub.Add(menuItem28);
            menuItem27.Sub.Add(menuItem29);
            menuItem27.Text = "Item 1";
            menuItem31.Text = "Option 3";
            menuItem32.Text = "Option 4";
            menuItem30.Sub.Add(menuItem31);
            menuItem30.Sub.Add(menuItem32);
            menuItem30.Text = "Item 2";
            menuItem26.Sub.Add(menuItem27);
            menuItem26.Sub.Add(menuItem30);
            menuItem26.Text = "Navigation One";
            menuItem33.Expand = false;
            menuItem33.IconSvg = "AppstoreOutlined";
            menuItem34.Text = "Option 5";
            menuItem35.Text = "Option 6";
            menuItem37.Text = "Option 7";
            menuItem38.Text = "Option 8";
            menuItem36.Sub.Add(menuItem37);
            menuItem36.Sub.Add(menuItem38);
            menuItem36.Text = "Submenu";
            menuItem33.Sub.Add(menuItem34);
            menuItem33.Sub.Add(menuItem35);
            menuItem33.Sub.Add(menuItem36);
            menuItem33.Text = "Navigation Two";
            menuItem39.Expand = false;
            menuItem39.IconSvg = "SettingOutlined";
            menuItem40.Text = "Option 9";
            menuItem41.Text = "Option 10";
            menuItem42.Text = "Option 11";
            menuItem43.Text = "Option 12";
            menuItem39.Sub.Add(menuItem40);
            menuItem39.Sub.Add(menuItem41);
            menuItem39.Sub.Add(menuItem42);
            menuItem39.Sub.Add(menuItem43);
            menuItem39.Text = "Navigation Three";
            menu3.Items.Add(menuItem26);
            menu3.Items.Add(menuItem33);
            menu3.Items.Add(menuItem39);
            menu3.Location = new Point(0, 0);
            menu3.Mode = AntdUI.TMenuMode.Vertical;
            menu3.Name = "menu3";
            menu3.ScrollBarBlock = true;
            menu3.Size = new Size(247, 460);
            menu3.TabIndex = 4;
            // 
            // switch3
            // 
            switch3.CheckedText = "Dark";
            switch3.Location = new Point(267, 13);
            switch3.Name = "switch3";
            switch3.Size = new Size(84, 33);
            switch3.TabIndex = 5;
            switch3.UnCheckedText = "Light";
            switch3.CheckedChanged += switch3_CheckedChanged;
            // 
            // panel1
            // 
            panel1.Controls.Add(switch5);
            panel1.Controls.Add(switch2);
            panel1.Controls.Add(switch1);
            panel1.Controls.Add(menu2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 68);
            panel1.Margin = new Padding(0);
            panel1.Name = "panel1";
            panel1.Size = new Size(650, 460);
            panel1.TabIndex = 0;
            // 
            // switch5
            // 
            switch5.CheckedText = "平展";
            switch5.LocalizationCheckedText = "Menu.flatten";
            switch5.LocalizationUnCheckedText = "Menu.indent";
            switch5.Location = new Point(267, 91);
            switch5.Name = "switch5";
            switch5.Size = new Size(84, 33);
            switch5.TabIndex = 5;
            switch5.UnCheckedText = "缩进";
            switch5.CheckedChanged += switch5_CheckedChanged;
            // 
            // switch2
            // 
            switch2.CheckedText = "折叠";
            switch2.LocalizationCheckedText = "Menu.collapse";
            switch2.LocalizationUnCheckedText = "Menu.expand";
            switch2.Location = new Point(267, 52);
            switch2.Name = "switch2";
            switch2.Size = new Size(84, 33);
            switch2.TabIndex = 5;
            switch2.UnCheckedText = "展开";
            switch2.CheckedChanged += switch2_CheckedChanged;
            // 
            // switch1
            // 
            switch1.CheckedText = "Dark";
            switch1.Location = new Point(267, 13);
            switch1.Name = "switch1";
            switch1.Size = new Size(84, 33);
            switch1.TabIndex = 5;
            switch1.UnCheckedText = "Light";
            switch1.CheckedChanged += switch1_CheckedChanged;
            // 
            // divider3
            // 
            divider3.Dock = DockStyle.Fill;
            divider3.Font = new Font("Microsoft YaHei UI", 10F);
            divider3.LocalizationText = "Menu.{id}";
            divider3.Location = new Point(650, 40);
            divider3.Margin = new Padding(0);
            divider3.Name = "divider3";
            divider3.Orientation = AntdUI.TOrientation.Left;
            divider3.Size = new Size(650, 28);
            divider3.TabIndex = 3;
            divider3.Text = "垂直菜单";
            // 
            // Menu
            // 
            Controls.Add(tableLayoutPanel1);
            Controls.Add(menu1);
            Controls.Add(divider1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 12F);
            Name = "Menu";
            Size = new Size(1300, 676);
            tableLayoutPanel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.PageHeader header1;
        private AntdUI.Divider divider1;
        private AntdUI.Menu menu1;
        private AntdUI.Divider divider2;
        private AntdUI.Menu menu2;
        private TableLayoutPanel tableLayoutPanel1;
        private AntdUI.Divider divider3;
        private AntdUI.Menu menu3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private AntdUI.Switch switch1;
        private AntdUI.Switch switch2;
        private AntdUI.Switch switch4;
        private AntdUI.Switch switch5;
        private AntdUI.Switch switch3;
    }
}