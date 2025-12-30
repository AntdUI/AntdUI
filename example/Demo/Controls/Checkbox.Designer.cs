using System.Drawing;
using System.Windows.Forms;

namespace Demo.Controls
{
    partial class Checkbox
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
            panel5 = new System.Windows.Forms.Panel();
            button2 = new AntdUI.Button();
            button1 = new AntdUI.Button();
            checkbox9 = new AntdUI.Checkbox();
            divider3 = new AntdUI.Divider();
            panel3 = new System.Windows.Forms.Panel();
            checkbox8 = new AntdUI.Checkbox();
            checkbox7 = new AntdUI.Checkbox();
            checkbox6 = new AntdUI.Checkbox();
            checkbox5 = new AntdUI.Checkbox();
            divider2 = new AntdUI.Divider();
            panel2 = new System.Windows.Forms.Panel();
            checkbox4 = new AntdUI.Checkbox();
            checkbox3 = new AntdUI.Checkbox();
            checkbox2 = new AntdUI.Checkbox();
            checkbox1 = new AntdUI.Checkbox();
            divider1 = new AntdUI.Divider();
            panel1.SuspendLayout();
            panel4.SuspendLayout();
            panel5.SuspendLayout();
            panel3.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Description = "多选框。";
            header1.Dock = DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F);
            header1.LocalizationDescription = "Checkbox.Description";
            header1.LocalizationText = "Checkbox.Text";
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new Padding(0, 0, 0, 10);
            header1.Size = new Size(650, 74);
            header1.TabIndex = 0;
            header1.Text = "Checkbox 多选框";
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
            panel1.Size = new Size(650, 323);
            panel1.TabIndex = 7;
            // 
            // panel4
            // 
            panel4.Controls.Add(panel5);
            panel4.Controls.Add(checkbox9);
            panel4.Dock = DockStyle.Top;
            panel4.Location = new Point(0, 180);
            panel4.Name = "panel4";
            panel4.Size = new Size(650, 90);
            panel4.TabIndex = 5;
            // 
            // panel5
            // 
            panel5.Controls.Add(button2);
            panel5.Controls.Add(button1);
            panel5.Font = new Font("Microsoft YaHei UI", 9F);
            panel5.Location = new Point(0, 46);
            panel5.Name = "panel5";
            panel5.Size = new Size(204, 38);
            panel5.TabIndex = 2;
            // 
            // button2
            // 
            button2.AutoSizeMode = AntdUI.TAutoSize.Width;
            button2.Dock = DockStyle.Left;
            button2.Location = new Point(74, 0);
            button2.Name = "button2";
            button2.Size = new Size(67, 38);
            button2.TabIndex = 1;
            button2.Text = "Disable";
            button2.Type = AntdUI.TTypeMini.Primary;
            button2.Click += button2_Click;
            // 
            // button1
            // 
            button1.AutoSizeMode = AntdUI.TAutoSize.Width;
            button1.Dock = DockStyle.Left;
            button1.Location = new Point(0, 0);
            button1.Name = "button1";
            button1.Size = new Size(74, 38);
            button1.TabIndex = 0;
            button1.Text = "Uncheck";
            button1.Type = AntdUI.TTypeMini.Primary;
            button1.Click += button1_Click;
            // 
            // checkbox9
            // 
            checkbox9.AutoSizeMode = AntdUI.TAutoSize.Auto;
            checkbox9.Checked = true;
            checkbox9.Location = new Point(0, 0);
            checkbox9.Name = "checkbox9";
            checkbox9.Size = new Size(176, 42);
            checkbox9.TabIndex = 0;
            checkbox9.Text = "Checked-Enabled";
            checkbox9.CheckedChanged += checkbox9_CheckedChanged;
            checkbox9.EnabledChanged += checkbox9_EnabledChanged;
            // 
            // divider3
            // 
            divider3.Dock = DockStyle.Top;
            divider3.Font = new Font("Microsoft YaHei UI", 10F);
            divider3.LocalizationText = "Checkbox.{id}";
            divider3.Location = new Point(0, 152);
            divider3.Name = "divider3";
            divider3.Orientation = AntdUI.TOrientation.Left;
            divider3.Size = new Size(650, 28);
            divider3.TabIndex = 4;
            divider3.Text = "联动";
            // 
            // panel3
            // 
            panel3.Controls.Add(checkbox8);
            panel3.Controls.Add(checkbox7);
            panel3.Controls.Add(checkbox6);
            panel3.Controls.Add(checkbox5);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(0, 104);
            panel3.Name = "panel3";
            panel3.Size = new Size(650, 48);
            panel3.TabIndex = 1;
            // 
            // checkbox8
            // 
            checkbox8.AutoSizeMode = AntdUI.TAutoSize.Width;
            checkbox8.Checked = true;
            checkbox8.Dock = DockStyle.Left;
            checkbox8.Enabled = false;
            checkbox8.Fill = Color.FromArgb(100, 0, 0);
            checkbox8.Location = new Point(334, 0);
            checkbox8.Name = "checkbox8";
            checkbox8.Size = new Size(113, 48);
            checkbox8.TabIndex = 7;
            checkbox8.Text = "Option D";
            // 
            // checkbox7
            // 
            checkbox7.AutoSizeMode = AntdUI.TAutoSize.Width;
            checkbox7.Dock = DockStyle.Left;
            checkbox7.Fill = Color.FromArgb(150, 0, 0);
            checkbox7.Location = new Point(223, 0);
            checkbox7.Name = "checkbox7";
            checkbox7.Size = new Size(111, 48);
            checkbox7.TabIndex = 6;
            checkbox7.Text = "Option C";
            // 
            // checkbox6
            // 
            checkbox6.AutoSizeMode = AntdUI.TAutoSize.Width;
            checkbox6.Dock = DockStyle.Left;
            checkbox6.Fill = Color.FromArgb(200, 0, 0);
            checkbox6.Location = new Point(112, 0);
            checkbox6.Name = "checkbox6";
            checkbox6.Size = new Size(111, 48);
            checkbox6.TabIndex = 5;
            checkbox6.Text = "Option B";
            // 
            // checkbox5
            // 
            checkbox5.AutoSizeMode = AntdUI.TAutoSize.Width;
            checkbox5.Dock = DockStyle.Left;
            checkbox5.Fill = Color.FromArgb(250, 0, 0);
            checkbox5.Location = new Point(0, 0);
            checkbox5.Name = "checkbox5";
            checkbox5.Size = new Size(112, 48);
            checkbox5.TabIndex = 4;
            checkbox5.Text = "Option A";
            // 
            // divider2
            // 
            divider2.Dock = DockStyle.Top;
            divider2.Font = new Font("Microsoft YaHei UI", 10F);
            divider2.LocalizationText = "Checkbox.{id}";
            divider2.Location = new Point(0, 76);
            divider2.Name = "divider2";
            divider2.Orientation = AntdUI.TOrientation.Left;
            divider2.Size = new Size(650, 28);
            divider2.TabIndex = 3;
            divider2.Text = "自定义颜色";
            // 
            // panel2
            // 
            panel2.Controls.Add(checkbox4);
            panel2.Controls.Add(checkbox3);
            panel2.Controls.Add(checkbox2);
            panel2.Controls.Add(checkbox1);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 28);
            panel2.Name = "panel2";
            panel2.Size = new Size(650, 48);
            panel2.TabIndex = 0;
            // 
            // checkbox4
            // 
            checkbox4.AutoSizeMode = AntdUI.TAutoSize.Width;
            checkbox4.Dock = DockStyle.Left;
            checkbox4.Enabled = false;
            checkbox4.Location = new Point(330, 0);
            checkbox4.Name = "checkbox4";
            checkbox4.Size = new Size(110, 48);
            checkbox4.TabIndex = 3;
            checkbox4.Text = "Option 4";
            // 
            // checkbox3
            // 
            checkbox3.AutoSizeMode = AntdUI.TAutoSize.Width;
            checkbox3.Dock = DockStyle.Left;
            checkbox3.Location = new Point(220, 0);
            checkbox3.Name = "checkbox3";
            checkbox3.Size = new Size(110, 48);
            checkbox3.TabIndex = 2;
            checkbox3.Text = "Option 3";
            // 
            // checkbox2
            // 
            checkbox2.AutoSizeMode = AntdUI.TAutoSize.Width;
            checkbox2.Dock = DockStyle.Left;
            checkbox2.Location = new Point(110, 0);
            checkbox2.Name = "checkbox2";
            checkbox2.Size = new Size(110, 48);
            checkbox2.TabIndex = 1;
            checkbox2.Text = "Option 2";
            // 
            // checkbox1
            // 
            checkbox1.AutoSizeMode = AntdUI.TAutoSize.Width;
            checkbox1.Dock = DockStyle.Left;
            checkbox1.Location = new Point(0, 0);
            checkbox1.Name = "checkbox1";
            checkbox1.Size = new Size(110, 48);
            checkbox1.TabIndex = 0;
            checkbox1.Text = "Option 1";
            // 
            // divider1
            // 
            divider1.Dock = DockStyle.Top;
            divider1.Font = new Font("Microsoft YaHei UI", 10F);
            divider1.LocalizationText = "Checkbox.{id}";
            divider1.Location = new Point(0, 0);
            divider1.Name = "divider1";
            divider1.Orientation = AntdUI.TOrientation.Left;
            divider1.Size = new Size(650, 28);
            divider1.TabIndex = 2;
            divider1.Text = "基本用法";
            // 
            // Checkbox
            // 
            Controls.Add(panel1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 12F);
            Name = "Checkbox";
            Size = new Size(650, 397);
            panel1.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.PageHeader header1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private AntdUI.Checkbox checkbox8;
        private AntdUI.Checkbox checkbox7;
        private AntdUI.Checkbox checkbox6;
        private AntdUI.Checkbox checkbox5;
        private System.Windows.Forms.Panel panel2;
        private AntdUI.Checkbox checkbox4;
        private AntdUI.Checkbox checkbox3;
        private AntdUI.Checkbox checkbox2;
        private AntdUI.Checkbox checkbox1;
        private AntdUI.Divider divider1;
        private AntdUI.Divider divider2;
        private System.Windows.Forms.Panel panel4;
        private AntdUI.Divider divider3;
        private AntdUI.Checkbox checkbox9;
        private System.Windows.Forms.Panel panel5;
        private AntdUI.Button button2;
        private AntdUI.Button button1;
    }
}