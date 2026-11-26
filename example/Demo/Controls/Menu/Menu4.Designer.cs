using System.Drawing;
using System.Windows.Forms;

namespace Demo.Controls
{
    partial class Menu4
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
            btn_toggle = new AntdUI.Button();
            menu1 = new AntdUI.Menu();
            SuspendLayout();
            // 
            // btn_toggle
            // 
            btn_toggle.IconSvg = "MenuOutlined";
            btn_toggle.Location = new Point(20, 18);
            btn_toggle.Name = "btn_toggle";
            btn_toggle.Radius = 8;
            btn_toggle.Size = new Size(36, 36);
            btn_toggle.TabIndex = 0;
            btn_toggle.Type = AntdUI.TTypeMini.Primary;
            btn_toggle.WaveSize = 0;
            btn_toggle.Click += btn_toggle_Click;
            // 
            // menu1
            // 
            menuItem1.IconSvg = "CheckCircleOutlined";
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
            menuItem8.IconSvg = "MailOutlined";
            menuItem8.Text = "Navigation Two";
            menuItem9.Text = "Option 5";
            menuItem10.Text = "Option 6";
            menuItem11.Text = "Option 7";
            menuItem12.Text = "Option 8";
            menuItem8.Sub.Add(menuItem9);
            menuItem8.Sub.Add(menuItem10);
            menuItem8.Sub.Add(menuItem11);
            menuItem8.Sub.Add(menuItem12);
            menuItem13.IconSvg = "AppstoreOutlined";
            menuItem13.Text = "Navigation Three";
            menuItem14.Text = "Option 9";
            menuItem15.Text = "Option 10";
            menuItem13.Sub.Add(menuItem14);
            menuItem13.Sub.Add(menuItem15);
            menu1.BackColor = Color.FromArgb(0, 21, 41);
            menu1.Collapsed = true;
            menu1.ColorScheme = AntdUI.TAMode.Dark;
            menu1.Font = new Font("Microsoft YaHei UI", 10F);
            menu1.Items.Add(menuItem1);
            menu1.Items.Add(menuItem8);
            menu1.Items.Add(menuItem13);
            menu1.Location = new Point(20, 68);
            menu1.Name = "menu1";
            menu1.ScrollBarBlock = true;
            menu1.Size = new Size(66, 510);
            menu1.TabIndex = 1;
            // 
            // Menu4
            // 
            AutoScroll = true;
            Controls.Add(menu1);
            Controls.Add(btn_toggle);
            Name = "Menu4";
            Size = new Size(421, 600);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private AntdUI.Button btn_toggle;
        private AntdUI.Menu menu1;
    }
}
