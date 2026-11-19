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
            AntdUI.MenuItem menuItem130 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem131 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem132 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem133 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem134 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem135 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem136 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem137 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem138 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem139 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem140 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem141 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem142 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem143 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem144 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem145 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem146 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem147 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem148 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem149 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem150 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem151 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem152 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem153 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem154 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem155 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem156 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem157 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem158 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem159 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem160 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem161 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem162 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem163 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem164 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem165 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem166 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem167 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem168 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem169 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem170 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem171 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem172 = new AntdUI.MenuItem();
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
            select1 = new AntdUI.Select();
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
            header1.LocalizationText = "Menu";
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
            menuItem130.IconSvg = "MailOutlined";
            menuItem130.ID = "mail";
            menuItem130.Select = true;
            menuItem130.Text = "Navigation One";
            menuItem131.IconSvg = "AppstoreOutlined";
            menuItem131.ID = "app";
            menuItem131.Text = "Navigation Two";
            menuItem132.IconSvg = "SettingOutlined";
            menuItem132.ID = "SubMenu";
            menuItem133.Text = "Option 1";
            menuItem134.Text = "Option 2";
            menuItem135.Text = "Option 3";
            menuItem136.Text = "Option 4";
            menuItem132.Sub.Add(menuItem133);
            menuItem132.Sub.Add(menuItem134);
            menuItem132.Sub.Add(menuItem135);
            menuItem132.Sub.Add(menuItem136);
            menuItem132.Text = "Navigation Three - Submenu";
            menu1.Items.Add(menuItem130);
            menu1.Items.Add(menuItem131);
            menu1.Items.Add(menuItem132);
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
            menuItem137.IconSvg = "MailOutlined";
            menuItem137.Select = true;
            menuItem138.IconSvg = "MenuOutlined";
            menuItem138.Select = true;
            menuItem139.IconSvg = "CheckCircleOutlined";
            menuItem139.Select = true;
            menuItem139.Text = "Option 1";
            menuItem140.IconSvg = "ClockCircleOutlined";
            menuItem140.Text = "Option 2";
            menuItem138.Sub.Add(menuItem139);
            menuItem138.Sub.Add(menuItem140);
            menuItem138.Text = "Item 1";
            menuItem141.Expand = false;
            menuItem141.IconSvg = "InfoCircleOutlined";
            menuItem142.Text = "Option 3";
            menuItem143.Text = "Option 4";
            menuItem141.Sub.Add(menuItem142);
            menuItem141.Sub.Add(menuItem143);
            menuItem141.Text = "Item 2";
            menuItem137.Sub.Add(menuItem138);
            menuItem137.Sub.Add(menuItem141);
            menuItem137.Text = "Navigation One";
            menuItem144.Expand = false;
            menuItem144.IconSvg = "AppstoreOutlined";
            menuItem145.Text = "Option 5";
            menuItem146.Text = "Option 6";
            menuItem148.Text = "Option 7";
            menuItem149.Text = "Option 8";
            menuItem147.Sub.Add(menuItem148);
            menuItem147.Sub.Add(menuItem149);
            menuItem147.Text = "Submenu";
            menuItem144.Sub.Add(menuItem145);
            menuItem144.Sub.Add(menuItem146);
            menuItem144.Sub.Add(menuItem147);
            menuItem144.Text = "Navigation Two";
            menuItem150.Expand = false;
            menuItem150.IconSvg = "SettingOutlined";
            menuItem151.Text = "Option 9";
            menuItem152.Text = "Option 10";
            menuItem153.Text = "Option 11";
            menuItem154.Text = "Option 12";
            menuItem150.Sub.Add(menuItem151);
            menuItem150.Sub.Add(menuItem152);
            menuItem150.Sub.Add(menuItem153);
            menuItem150.Sub.Add(menuItem154);
            menuItem150.Text = "Navigation Three";
            menu2.Items.Add(menuItem137);
            menu2.Items.Add(menuItem144);
            menu2.Items.Add(menuItem150);
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
            menuItem155.IconSvg = "MailOutlined";
            menuItem157.Text = "Option 1";
            menuItem158.Text = "Option 2";
            menuItem156.Sub.Add(menuItem157);
            menuItem156.Sub.Add(menuItem158);
            menuItem156.Text = "Item 1";
            menuItem160.Text = "Option 3";
            menuItem161.Text = "Option 4";
            menuItem159.Sub.Add(menuItem160);
            menuItem159.Sub.Add(menuItem161);
            menuItem159.Text = "Item 2";
            menuItem155.Sub.Add(menuItem156);
            menuItem155.Sub.Add(menuItem159);
            menuItem155.Text = "Navigation One";
            menuItem162.Expand = false;
            menuItem162.IconSvg = "AppstoreOutlined";
            menuItem163.Text = "Option 5";
            menuItem164.Text = "Option 6";
            menuItem166.Text = "Option 7";
            menuItem167.Text = "Option 8";
            menuItem165.Sub.Add(menuItem166);
            menuItem165.Sub.Add(menuItem167);
            menuItem165.Text = "Submenu";
            menuItem162.Sub.Add(menuItem163);
            menuItem162.Sub.Add(menuItem164);
            menuItem162.Sub.Add(menuItem165);
            menuItem162.Text = "Navigation Two";
            menuItem168.Expand = false;
            menuItem168.IconSvg = "SettingOutlined";
            menuItem169.Text = "Option 9";
            menuItem170.Text = "Option 10";
            menuItem171.Text = "Option 11";
            menuItem172.Text = "Option 12";
            menuItem168.Sub.Add(menuItem169);
            menuItem168.Sub.Add(menuItem170);
            menuItem168.Sub.Add(menuItem171);
            menuItem168.Sub.Add(menuItem172);
            menuItem168.Text = "Navigation Three";
            menu3.Items.Add(menuItem155);
            menu3.Items.Add(menuItem162);
            menu3.Items.Add(menuItem168);
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
            panel1.Controls.Add(select1);
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
            // select1
            // 
            select1.Items.AddRange(new object[] { "Hover", "Click" });
            select1.List = true;
            select1.Location = new Point(267, 130);
            select1.Name = "select1";
            select1.SelectedIndex = 1;
            select1.SelectedValue = "Tom";
            select1.Size = new Size(120, 41);
            select1.TabIndex = 6;
            select1.Text = "Hover";
            select1.SelectedIndexChanged += Select1_SelectedIndexChanged;
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
        private AntdUI.Select select1;
    }
}