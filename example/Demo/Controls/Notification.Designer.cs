using System.Drawing;
using System.Windows.Forms;

namespace Demo.Controls
{
    partial class Notification
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
            panel1 = new System.Windows.Forms.Panel();
            panel4 = new System.Windows.Forms.Panel();
            button11 = new AntdUI.Button();
            button12 = new AntdUI.Button();
            button13 = new AntdUI.Button();
            button14 = new AntdUI.Button();
            divider3 = new AntdUI.Divider();
            panel3 = new System.Windows.Forms.Panel();
            button7 = new AntdUI.Button();
            button8 = new AntdUI.Button();
            button9 = new AntdUI.Button();
            button10 = new AntdUI.Button();
            divider2 = new AntdUI.Divider();
            panel2 = new System.Windows.Forms.Panel();
            button4 = new AntdUI.Button();
            button6 = new AntdUI.Button();
            button2 = new AntdUI.Button();
            button5 = new AntdUI.Button();
            button3 = new AntdUI.Button();
            button1 = new AntdUI.Button();
            divider1 = new AntdUI.Divider();
            panel1.SuspendLayout();
            panel4.SuspendLayout();
            panel3.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Description = "全局展示通知提醒信息。";
            header1.Dock = DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F);
            header1.LocalizationDescription = "Notification.Description";
            header1.LocalizationText = "Notification.Text";
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new Padding(0, 0, 0, 10);
            header1.Size = new Size(543, 74);
            header1.TabIndex = 0;
            header1.Text = "Notification 通知提醒框";
            header1.UseTitleFont = true;
            // 
            // panel1
            // 
            panel1.Controls.Add(panel4);
            panel1.Controls.Add(divider3);
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(divider2);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(divider1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 74);
            panel1.Name = "panel1";
            panel1.Size = new Size(543, 442);
            panel1.TabIndex = 6;
            // 
            // panel4
            // 
            panel4.Controls.Add(button11);
            panel4.Controls.Add(button12);
            panel4.Controls.Add(button13);
            panel4.Controls.Add(button14);
            panel4.Dock = DockStyle.Top;
            panel4.Location = new Point(0, 342);
            panel4.Name = "panel4";
            panel4.Size = new Size(543, 63);
            panel4.TabIndex = 6;
            // 
            // button11
            // 
            button11.BorderWidth = 2F;
            button11.Location = new Point(347, 8);
            button11.Name = "button11";
            button11.Size = new Size(110, 40);
            button11.TabIndex = 1;
            button11.Text = "Info";
            button11.Type = AntdUI.TTypeMini.Info;
            button11.Click += button11_Click;
            // 
            // button12
            // 
            button12.BorderWidth = 2F;
            button12.Location = new Point(236, 8);
            button12.Name = "button12";
            button12.Size = new Size(110, 40);
            button12.TabIndex = 1;
            button12.Text = "Warning";
            button12.Type = AntdUI.TTypeMini.Warn;
            button12.Click += button12_Click;
            // 
            // button13
            // 
            button13.BorderWidth = 2F;
            button13.Location = new Point(125, 8);
            button13.Name = "button13";
            button13.Size = new Size(110, 40);
            button13.TabIndex = 1;
            button13.Text = "Error";
            button13.Type = AntdUI.TTypeMini.Error;
            button13.Click += button13_Click;
            // 
            // button14
            // 
            button14.BorderWidth = 2F;
            button14.Location = new Point(14, 8);
            button14.Name = "button14";
            button14.Size = new Size(110, 40);
            button14.TabIndex = 1;
            button14.Text = "Success";
            button14.Type = AntdUI.TTypeMini.Success;
            button14.Click += button14_Click;
            // 
            // divider3
            // 
            divider3.Dock = DockStyle.Top;
            divider3.Font = new Font("Microsoft YaHei UI", 10F);
            divider3.LocalizationText = "Notification.{id}";
            divider3.Location = new Point(0, 314);
            divider3.Name = "divider3";
            divider3.Orientation = AntdUI.TOrientation.Left;
            divider3.Size = new Size(543, 28);
            divider3.TabIndex = 5;
            divider3.Text = "Windows 消息框声音";
            // 
            // panel3
            // 
            panel3.Controls.Add(button7);
            panel3.Controls.Add(button8);
            panel3.Controls.Add(button9);
            panel3.Controls.Add(button10);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(0, 251);
            panel3.Name = "panel3";
            panel3.Size = new Size(543, 63);
            panel3.TabIndex = 4;
            // 
            // button7
            // 
            button7.BorderWidth = 2F;
            button7.Location = new Point(347, 8);
            button7.Name = "button7";
            button7.Size = new Size(110, 40);
            button7.TabIndex = 1;
            button7.Text = "Info";
            button7.Type = AntdUI.TTypeMini.Info;
            button7.Click += button7_Click;
            // 
            // button8
            // 
            button8.BorderWidth = 2F;
            button8.Location = new Point(236, 8);
            button8.Name = "button8";
            button8.Size = new Size(110, 40);
            button8.TabIndex = 1;
            button8.Text = "Warning";
            button8.Type = AntdUI.TTypeMini.Warn;
            button8.Click += button8_Click;
            // 
            // button9
            // 
            button9.BorderWidth = 2F;
            button9.Location = new Point(125, 8);
            button9.Name = "button9";
            button9.Size = new Size(110, 40);
            button9.TabIndex = 1;
            button9.Text = "Error";
            button9.Type = AntdUI.TTypeMini.Error;
            button9.Click += button9_Click;
            // 
            // button10
            // 
            button10.BorderWidth = 2F;
            button10.Location = new Point(14, 8);
            button10.Name = "button10";
            button10.Size = new Size(110, 40);
            button10.TabIndex = 1;
            button10.Text = "Success";
            button10.Type = AntdUI.TTypeMini.Success;
            button10.Click += button10_Click;
            // 
            // divider2
            // 
            divider2.Dock = DockStyle.Top;
            divider2.Font = new Font("Microsoft YaHei UI", 10F);
            divider2.LocalizationText = "Notification.{id}";
            divider2.Location = new Point(0, 223);
            divider2.Name = "divider2";
            divider2.Orientation = AntdUI.TOrientation.Left;
            divider2.Size = new Size(543, 28);
            divider2.TabIndex = 3;
            divider2.Text = "四种样式";
            // 
            // panel2
            // 
            panel2.Controls.Add(button4);
            panel2.Controls.Add(button6);
            panel2.Controls.Add(button2);
            panel2.Controls.Add(button5);
            panel2.Controls.Add(button3);
            panel2.Controls.Add(button1);
            panel2.Dock = DockStyle.Top;
            panel2.ForeColor = Color.White;
            panel2.Location = new Point(0, 28);
            panel2.Name = "panel2";
            panel2.Size = new Size(543, 195);
            panel2.TabIndex = 2;
            // 
            // button4
            // 
            button4.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button4.IconSvg = "RadiusBottomrightOutlined";
            button4.Location = new Point(169, 69);
            button4.Name = "button4";
            button4.Size = new Size(147, 46);
            button4.TabIndex = 1;
            button4.Text = "bottomRight";
            button4.Type = AntdUI.TTypeMini.Primary;
            button4.Click += button4_Click;
            // 
            // button6
            // 
            button6.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button6.IconSvg = "BorderBottomOutlined";
            button6.Location = new Point(169, 129);
            button6.Name = "button6";
            button6.Size = new Size(106, 46);
            button6.TabIndex = 1;
            button6.Text = "bottom";
            button6.Type = AntdUI.TTypeMini.Primary;
            button6.Click += button6_Click;
            // 
            // button2
            // 
            button2.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button2.IconSvg = "RadiusUprightOutlined";
            button2.Location = new Point(169, 9);
            button2.Name = "button2";
            button2.Size = new Size(116, 46);
            button2.TabIndex = 1;
            button2.Text = "topRight";
            button2.Type = AntdUI.TTypeMini.Primary;
            button2.Click += button2_Click;
            // 
            // button5
            // 
            button5.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button5.IconSvg = "BorderTopOutlined";
            button5.Location = new Point(14, 129);
            button5.Name = "button5";
            button5.Size = new Size(75, 46);
            button5.TabIndex = 1;
            button5.Text = "top";
            button5.Type = AntdUI.TTypeMini.Primary;
            button5.Click += button5_Click;
            // 
            // button3
            // 
            button3.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button3.IconSvg = "RadiusBottomleftOutlined";
            button3.Location = new Point(14, 69);
            button3.Name = "button3";
            button3.Size = new Size(135, 46);
            button3.TabIndex = 1;
            button3.Text = "bottomLeft";
            button3.Type = AntdUI.TTypeMini.Primary;
            button3.Click += button3_Click;
            // 
            // button1
            // 
            button1.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button1.IconSvg = "RadiusUpleftOutlined";
            button1.Location = new Point(14, 9);
            button1.Name = "button1";
            button1.Size = new Size(104, 46);
            button1.TabIndex = 1;
            button1.Text = "topLeft";
            button1.Type = AntdUI.TTypeMini.Primary;
            button1.Click += button1_Click;
            // 
            // divider1
            // 
            divider1.Dock = DockStyle.Top;
            divider1.Font = new Font("Microsoft YaHei UI", 10F);
            divider1.LocalizationText = "Notification.{id}";
            divider1.Location = new Point(0, 0);
            divider1.Name = "divider1";
            divider1.Orientation = AntdUI.TOrientation.Left;
            divider1.Size = new Size(543, 28);
            divider1.TabIndex = 0;
            divider1.Text = "六种方向";
            // 
            // Notification
            // 
            Controls.Add(panel1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 12F);
            Name = "Notification";
            Size = new Size(543, 516);
            panel1.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.PageHeader header1;
        private System.Windows.Forms.Panel panel1;
        private AntdUI.Divider divider1;
        private System.Windows.Forms.Panel panel2;
        private AntdUI.Button button1;
        private AntdUI.Button button4;
        private AntdUI.Button button2;
        private AntdUI.Button button3;
        private AntdUI.Button button6;
        private AntdUI.Button button5;
        private System.Windows.Forms.Panel panel3;
        private AntdUI.Button button7;
        private AntdUI.Button button8;
        private AntdUI.Button button9;
        private AntdUI.Button button10;
        private AntdUI.Divider divider2;
        private System.Windows.Forms.Panel panel4;
        private AntdUI.Button button11;
        private AntdUI.Button button12;
        private AntdUI.Button button13;
        private AntdUI.Button button14;
        private AntdUI.Divider divider3;
    }
}