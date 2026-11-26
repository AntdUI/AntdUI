using System.Drawing;
using System.Windows.Forms;

namespace Demo.Controls
{
    partial class Menu2
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
            menu1 = new MnemonicMenu();
            label1 = new AntdUI.Label();
            SuspendLayout();
            // 
            // menu1
            // 
            menuItem1.Text = "文件(&F)";
            menuItem2.Text = "新建(&N)";
            menuItem3.Text = "打开(&O)";
            menuItem4.Text = "保存(&S)";
            menuItem1.Sub.Add(menuItem2);
            menuItem1.Sub.Add(menuItem3);
            menuItem1.Sub.Add(menuItem4);
            menuItem5.Text = "编辑(&E)";
            menuItem6.Text = "转到(&G)";
            menuItem7.Text = "查找和替换(&F)";
            menuItem5.Sub.Add(menuItem6);
            menuItem5.Sub.Add(menuItem7);
            menuItem8.Text = "视图(&V)";
            menuItem9.Text = "Git(&G)";
            menuItem10.Text = "项目(&P)";
            menuItem11.Text = "生成(&B)";
            menuItem12.Text = "调试(&D)";
            menuItem13.Text = "测试(&S)";
            menuItem14.Text = "工具(&T)";
            menuItem15.Text = "扩展(&X)";
            menuItem16.Text = "窗口(&W)";
            menuItem17.Text = "帮助(&H)";
            menu1.Dock = DockStyle.Top;
            menu1.Font = new Font("Microsoft YaHei UI", 10F);
            menu1.Items.Add(menuItem1);
            menu1.Items.Add(menuItem5);
            menu1.Items.Add(menuItem8);
            menu1.Items.Add(menuItem9);
            menu1.Items.Add(menuItem10);
            menu1.Items.Add(menuItem11);
            menu1.Items.Add(menuItem12);
            menu1.Items.Add(menuItem13);
            menu1.Items.Add(menuItem14);
            menu1.Items.Add(menuItem15);
            menu1.Items.Add(menuItem16);
            menu1.Items.Add(menuItem17);
            menu1.Location = new Point(0, 0);
            menu1.Mode = AntdUI.TMenuMode.Horizontal;
            menu1.Name = "menu1";
            menu1.Radius = 0;
            menu1.Size = new Size(845, 42);
            menu1.TabIndex = 0;
            menu1.Trigger = AntdUI.Trigger.Click;
            menu1.UseMnemonic = true;
            menu1.Mnemonic += menu1_Mnemonic;
            // 
            // label1
            // 
            label1.AutoSize = false;
            label1.Dock = DockStyle.Bottom;
            label1.Font = new Font("Microsoft YaHei UI", 10F);
            label1.Location = new Point(0, 42);
            label1.Name = "label1";
            label1.Padding = new Padding(10, 0, 0, 0);
            label1.Size = new Size(845, 38);
            label1.TabIndex = 1;
            label1.Text = "按 Alt + F / E / V / G / P / B / D / S / T / X / W / H 激活菜单，激活后会弹出 Message 提示。";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // Menu2
            // 
            AutoScroll = true;
            Controls.Add(label1);
            Controls.Add(menu1);
            Name = "Menu2";
            Size = new Size(845, 100);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MnemonicMenu menu1;
        private AntdUI.Label label1;
    }
}
