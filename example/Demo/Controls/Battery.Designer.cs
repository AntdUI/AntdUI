using System.Drawing;

namespace Demo.Controls
{
    partial class Battery
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
            battery1 = new AntdUI.Battery();
            battery4 = new AntdUI.Battery();
            battery5 = new AntdUI.Battery();
            button1 = new AntdUI.Button();
            button2 = new AntdUI.Button();
            divider1 = new AntdUI.Divider();
            stackPanel1 = new AntdUI.StackPanel();
            battery3 = new AntdUI.Battery();
            battery2 = new AntdUI.Battery();
            stackPanel2 = new AntdUI.StackPanel();
            battery6 = new AntdUI.Battery();
            battery7 = new AntdUI.Battery();
            battery8 = new AntdUI.Battery();
            battery9 = new AntdUI.Battery();
            battery10 = new AntdUI.Battery();
            divider2 = new AntdUI.Divider();
            stackPanel3 = new AntdUI.StackPanel();
            battery11 = new AntdUI.Battery();
            battery12 = new AntdUI.Battery();
            battery13 = new AntdUI.Battery();
            battery14 = new AntdUI.Battery();
            battery15 = new AntdUI.Battery();
            divider3 = new AntdUI.Divider();
            stackPanel4 = new AntdUI.StackPanel();
            stackPanel1.SuspendLayout();
            stackPanel2.SuspendLayout();
            stackPanel3.SuspendLayout();
            stackPanel4.SuspendLayout();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Description = "展示设备电量。";
            header1.Dock = System.Windows.Forms.DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F);
            header1.LocalizationDescription = "Battery.Description";
            header1.LocalizationText = "Battery.Text";
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            header1.Size = new Size(596, 74);
            header1.TabIndex = 0;
            header1.Text = "Battery 电量";
            header1.UseTitleFont = true;
            // 
            // battery1
            // 
            battery1.Location = new Point(91, 3);
            battery1.Name = "battery1";
            battery1.Size = new Size(82, 30);
            battery1.TabIndex = 1;
            battery1.Value = 30;
            // 
            // battery4
            // 
            battery4.Location = new Point(179, 3);
            battery4.Name = "battery4";
            battery4.Size = new Size(82, 30);
            battery4.TabIndex = 2;
            battery4.Value = 60;
            // 
            // battery5
            // 
            battery5.Location = new Point(3, 3);
            battery5.Name = "battery5";
            battery5.Size = new Size(82, 30);
            battery5.TabIndex = 0;
            battery5.Value = 15;
            // 
            // button1
            // 
            button1.AutoSizeMode = AntdUI.TAutoSize.Width;
            button1.LocalizationText = "Battery.Add";
            button1.Location = new Point(3, 7);
            button1.Name = "button1";
            button1.Size = new Size(83, 38);
            button1.TabIndex = 0;
            button1.Text = "加电量";
            button1.Type = AntdUI.TTypeMini.Primary;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.AutoSizeMode = AntdUI.TAutoSize.Width;
            button2.LocalizationText = "Battery.Subtract";
            button2.Location = new Point(92, 7);
            button2.Name = "button2";
            button2.Size = new Size(83, 38);
            button2.TabIndex = 1;
            button2.Text = "减电量";
            button2.Type = AntdUI.TTypeMini.Success;
            button2.Click += button2_Click;
            // 
            // divider1
            // 
            divider1.Dock = System.Windows.Forms.DockStyle.Top;
            divider1.Font = new Font("Microsoft YaHei UI", 10F);
            divider1.LocalizationText = "Battery.{id}";
            divider1.Location = new Point(0, 74);
            divider1.Name = "divider1";
            divider1.Orientation = AntdUI.TOrientation.Left;
            divider1.Size = new Size(596, 28);
            divider1.TabIndex = 0;
            divider1.Text = "基本用法";
            // 
            // stackPanel1
            // 
            stackPanel1.Controls.Add(battery3);
            stackPanel1.Controls.Add(battery2);
            stackPanel1.Controls.Add(battery4);
            stackPanel1.Controls.Add(battery1);
            stackPanel1.Controls.Add(battery5);
            stackPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            stackPanel1.Location = new Point(0, 102);
            stackPanel1.Name = "stackPanel1";
            stackPanel1.Size = new Size(596, 36);
            stackPanel1.TabIndex = 1;
            // 
            // battery3
            // 
            battery3.Location = new Point(355, 3);
            battery3.Name = "battery3";
            battery3.Size = new Size(82, 30);
            battery3.TabIndex = 4;
            battery3.Value = 100;
            // 
            // battery2
            // 
            battery2.Location = new Point(267, 3);
            battery2.Name = "battery2";
            battery2.Size = new Size(82, 30);
            battery2.TabIndex = 3;
            battery2.Value = 80;
            // 
            // stackPanel2
            // 
            stackPanel2.Controls.Add(battery6);
            stackPanel2.Controls.Add(battery7);
            stackPanel2.Controls.Add(battery8);
            stackPanel2.Controls.Add(battery9);
            stackPanel2.Controls.Add(battery10);
            stackPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            stackPanel2.Location = new Point(0, 166);
            stackPanel2.Name = "stackPanel2";
            stackPanel2.Size = new Size(596, 36);
            stackPanel2.TabIndex = 2;
            // 
            // battery6
            // 
            battery6.Location = new Point(355, 3);
            battery6.Name = "battery6";
            battery6.ShowText = false;
            battery6.Size = new Size(82, 30);
            battery6.TabIndex = 4;
            battery6.Value = 100;
            // 
            // battery7
            // 
            battery7.Location = new Point(267, 3);
            battery7.Name = "battery7";
            battery7.ShowText = false;
            battery7.Size = new Size(82, 30);
            battery7.TabIndex = 3;
            battery7.Value = 80;
            // 
            // battery8
            // 
            battery8.Location = new Point(179, 3);
            battery8.Name = "battery8";
            battery8.ShowText = false;
            battery8.Size = new Size(82, 30);
            battery8.TabIndex = 2;
            battery8.Value = 60;
            // 
            // battery9
            // 
            battery9.Location = new Point(91, 3);
            battery9.Name = "battery9";
            battery9.ShowText = false;
            battery9.Size = new Size(82, 30);
            battery9.TabIndex = 1;
            battery9.Value = 30;
            // 
            // battery10
            // 
            battery10.Location = new Point(3, 3);
            battery10.Name = "battery10";
            battery10.ShowText = false;
            battery10.Size = new Size(82, 30);
            battery10.TabIndex = 0;
            battery10.Value = 15;
            // 
            // divider2
            // 
            divider2.Dock = System.Windows.Forms.DockStyle.Top;
            divider2.Font = new Font("Microsoft YaHei UI", 10F);
            divider2.LocalizationText = "Battery.{id}";
            divider2.Location = new Point(0, 138);
            divider2.Name = "divider2";
            divider2.Orientation = AntdUI.TOrientation.Left;
            divider2.Size = new Size(596, 28);
            divider2.TabIndex = 0;
            divider2.Text = "无文字";
            // 
            // stackPanel3
            // 
            stackPanel3.Controls.Add(battery11);
            stackPanel3.Controls.Add(battery12);
            stackPanel3.Controls.Add(battery13);
            stackPanel3.Controls.Add(battery14);
            stackPanel3.Controls.Add(battery15);
            stackPanel3.Dock = System.Windows.Forms.DockStyle.Top;
            stackPanel3.Location = new Point(0, 230);
            stackPanel3.Name = "stackPanel3";
            stackPanel3.Size = new Size(596, 36);
            stackPanel3.TabIndex = 3;
            // 
            // battery11
            // 
            battery11.Location = new Point(355, 3);
            battery11.Name = "battery11";
            battery11.Size = new Size(82, 30);
            battery11.TabIndex = 4;
            battery11.Value = 100;
            // 
            // battery12
            // 
            battery12.DotSize = 6;
            battery12.Location = new Point(267, 3);
            battery12.Name = "battery12";
            battery12.Size = new Size(82, 30);
            battery12.TabIndex = 3;
            battery12.Value = 80;
            // 
            // battery13
            // 
            battery13.DotSize = 4;
            battery13.Location = new Point(179, 3);
            battery13.Name = "battery13";
            battery13.Size = new Size(82, 30);
            battery13.TabIndex = 2;
            battery13.Value = 60;
            // 
            // battery14
            // 
            battery14.DotSize = 2;
            battery14.Location = new Point(91, 3);
            battery14.Name = "battery14";
            battery14.Size = new Size(82, 30);
            battery14.TabIndex = 1;
            battery14.Value = 30;
            // 
            // battery15
            // 
            battery15.DotSize = 0;
            battery15.Location = new Point(3, 3);
            battery15.Name = "battery15";
            battery15.Size = new Size(82, 30);
            battery15.TabIndex = 0;
            battery15.Value = 15;
            // 
            // divider3
            // 
            divider3.Dock = System.Windows.Forms.DockStyle.Top;
            divider3.Font = new Font("Microsoft YaHei UI", 10F);
            divider3.LocalizationText = "Battery.{id}";
            divider3.Location = new Point(0, 202);
            divider3.Name = "divider3";
            divider3.Orientation = AntdUI.TOrientation.Left;
            divider3.Size = new Size(596, 28);
            divider3.TabIndex = 0;
            divider3.Text = "点形状";
            // 
            // stackPanel4
            // 
            stackPanel4.Controls.Add(button2);
            stackPanel4.Controls.Add(button1);
            stackPanel4.Dock = System.Windows.Forms.DockStyle.Top;
            stackPanel4.Location = new Point(0, 266);
            stackPanel4.Name = "stackPanel4";
            stackPanel4.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            stackPanel4.Size = new Size(596, 48);
            stackPanel4.TabIndex = 10;
            // 
            // Battery
            // 
            Controls.Add(stackPanel4);
            Controls.Add(stackPanel3);
            Controls.Add(divider3);
            Controls.Add(stackPanel2);
            Controls.Add(divider2);
            Controls.Add(stackPanel1);
            Controls.Add(divider1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 12F);
            Name = "Battery";
            Size = new Size(596, 445);
            stackPanel1.ResumeLayout(false);
            stackPanel2.ResumeLayout(false);
            stackPanel3.ResumeLayout(false);
            stackPanel4.ResumeLayout(false);
            stackPanel4.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.PageHeader header1;
        private AntdUI.Battery battery1;
        private AntdUI.Battery battery4;
        private AntdUI.Battery battery5;
        private AntdUI.Button button1;
        private AntdUI.Button button2;
        private AntdUI.Divider divider1;
        private AntdUI.StackPanel stackPanel1;
        private AntdUI.Battery battery3;
        private AntdUI.Battery battery2;
        private AntdUI.StackPanel stackPanel2;
        private AntdUI.Battery battery6;
        private AntdUI.Battery battery7;
        private AntdUI.Battery battery8;
        private AntdUI.Battery battery9;
        private AntdUI.Battery battery10;
        private AntdUI.Divider divider2;
        private AntdUI.StackPanel stackPanel3;
        private AntdUI.Battery battery11;
        private AntdUI.Battery battery12;
        private AntdUI.Battery battery13;
        private AntdUI.Battery battery14;
        private AntdUI.Battery battery15;
        private AntdUI.Divider divider3;
        private AntdUI.StackPanel stackPanel4;
    }
}