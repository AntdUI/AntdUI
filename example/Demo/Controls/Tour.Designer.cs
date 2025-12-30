using System.Drawing;
using System.Windows.Forms;

namespace Demo.Controls
{
    partial class Tour
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
            header1 = new AntdUI.PageHeader();
            panel_main = new System.Windows.Forms.Panel();
            panel5 = new System.Windows.Forms.Panel();
            button1 = new AntdUI.Button();
            button2 = new AntdUI.Button();
            button7 = new AntdUI.Button();
            button8 = new AntdUI.Button();
            button3 = new AntdUI.Button();
            button4 = new AntdUI.Button();
            button5 = new AntdUI.Button();
            button6 = new AntdUI.Button();
            panel_main.SuspendLayout();
            panel5.SuspendLayout();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Description = "用于分步引导用户了解产品功能的气泡组件。";
            header1.Dock = DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F);
            header1.LocalizationDescription = "Tour.Description";
            header1.LocalizationText = "Tour.Text";
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new Padding(0, 0, 0, 10);
            header1.Size = new Size(845, 74);
            header1.TabIndex = 0;
            header1.Text = "Tour 漫游式引导";
            header1.UseTitleFont = true;
            // 
            // panel_main
            // 
            panel_main.AutoScroll = true;
            panel_main.Controls.Add(panel5);
            panel_main.Dock = DockStyle.Fill;
            panel_main.Location = new Point(0, 74);
            panel_main.Name = "panel_main";
            panel_main.Size = new Size(845, 687);
            panel_main.TabIndex = 6;
            // 
            // panel5
            // 
            panel5.Controls.Add(button1);
            panel5.Controls.Add(button2);
            panel5.Controls.Add(button7);
            panel5.Controls.Add(button8);
            panel5.Controls.Add(button3);
            panel5.Controls.Add(button4);
            panel5.Controls.Add(button5);
            panel5.Controls.Add(button6);
            panel5.Dock = DockStyle.Top;
            panel5.Location = new Point(0, 0);
            panel5.Name = "panel5";
            panel5.Size = new Size(845, 111);
            panel5.TabIndex = 4;
            // 
            // button1
            // 
            button1.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button1.BorderWidth = 1F;
            button1.IconSvg = "SearchOutlined";
            button1.Location = new Point(3, 3);
            button1.Name = "button1";
            button1.Size = new Size(115, 47);
            button1.TabIndex = 0;
            button1.Text = "Button";
            button1.Type = AntdUI.TTypeMini.Primary;
            button1.Click += Btn;
            // 
            // button2
            // 
            button2.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button2.BorderWidth = 1F;
            button2.IconPosition = AntdUI.TAlignMini.Right;
            button2.Location = new Point(124, 3);
            button2.Name = "button2";
            button2.Size = new Size(87, 47);
            button2.TabIndex = 1;
            button2.Text = "Button";
            button2.Type = AntdUI.TTypeMini.Primary;
            button2.Click += Btn;
            // 
            // button7
            // 
            button7.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button7.BorderWidth = 1F;
            button7.IconSvg = "SearchOutlined";
            button7.Location = new Point(3, 56);
            button7.Name = "button7";
            button7.Size = new Size(115, 47);
            button7.TabIndex = 6;
            button7.Text = "Button";
            button7.Click += Btn;
            // 
            // button8
            // 
            button8.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button8.BorderWidth = 1F;
            button8.IconPosition = AntdUI.TAlignMini.Right;
            button8.Location = new Point(124, 56);
            button8.Name = "button8";
            button8.Size = new Size(87, 47);
            button8.TabIndex = 7;
            button8.Text = "Button";
            button8.Click += Btn;
            // 
            // button3
            // 
            button3.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button3.BorderWidth = 1F;
            button3.IconPosition = AntdUI.TAlignMini.Top;
            button3.IconSvg = "SearchOutlined";
            button3.LoadingWaveCount = 4;
            button3.LoadingWaveSize = 6;
            button3.LoadingWaveVertical = true;
            button3.Location = new Point(245, 3);
            button3.Name = "button3";
            button3.Size = new Size(115, 75);
            button3.TabIndex = 2;
            button3.Text = "Button";
            button3.Type = AntdUI.TTypeMini.Primary;
            button3.Click += Btn;
            // 
            // button4
            // 
            button4.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button4.BorderWidth = 1F;
            button4.IconPosition = AntdUI.TAlignMini.Bottom;
            button4.IconSvg = "SearchOutlined";
            button4.LoadingWaveCount = 4;
            button4.LoadingWaveSize = 6;
            button4.LoadingWaveVertical = true;
            button4.Location = new Point(366, 3);
            button4.Name = "button4";
            button4.Size = new Size(115, 75);
            button4.TabIndex = 3;
            button4.Text = "Button";
            button4.Type = AntdUI.TTypeMini.Primary;
            button4.Click += Btn;
            // 
            // button5
            // 
            button5.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button5.BorderWidth = 1F;
            button5.IconPosition = AntdUI.TAlignMini.Top;
            button5.IconSvg = "SearchOutlined";
            button5.LoadingWaveCount = 4;
            button5.LoadingWaveSize = 6;
            button5.LoadingWaveVertical = true;
            button5.Location = new Point(487, 3);
            button5.Name = "button5";
            button5.Size = new Size(115, 75);
            button5.TabIndex = 4;
            button5.Text = "Button";
            button5.Click += Btn;
            // 
            // button6
            // 
            button6.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button6.BorderWidth = 1F;
            button6.IconPosition = AntdUI.TAlignMini.Bottom;
            button6.IconSvg = "SearchOutlined";
            button6.LoadingWaveCount = 4;
            button6.LoadingWaveSize = 6;
            button6.LoadingWaveVertical = true;
            button6.Location = new Point(608, 3);
            button6.Name = "button6";
            button6.Size = new Size(115, 75);
            button6.TabIndex = 5;
            button6.Text = "Button";
            button6.Click += Btn;
            // 
            // Tour
            // 
            Controls.Add(panel_main);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 12F);
            Name = "Tour";
            Size = new Size(845, 761);
            panel_main.ResumeLayout(false);
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.PageHeader header1;
        private System.Windows.Forms.Panel panel_main;
        private System.Windows.Forms.Panel panel5;
        private AntdUI.Button button1;
        private AntdUI.Button button2;
        private AntdUI.Button button3;
        private AntdUI.Button button4;
        private AntdUI.Button button7;
        private AntdUI.Button button8;
        private AntdUI.Button button5;
        private AntdUI.Button button6;
    }
}