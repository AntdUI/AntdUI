using System.Drawing;
using System.Windows.Forms;

namespace Demo.Controls
{
    partial class ColorPicker
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
            flowLayoutPanel1 = new FlowLayoutPanel();
            colorPicker3 = new AntdUI.ColorPicker();
            colorPicker1 = new AntdUI.ColorPicker();
            colorPicker2 = new AntdUI.ColorPicker();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Description = "提供颜色选取的组件。";
            header1.Dock = DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F);
            header1.LocalizationDescription = "ColorPicker.Description";
            header1.LocalizationText = "ColorPicker.Text";
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new Padding(0, 0, 0, 10);
            header1.Size = new Size(740, 74);
            header1.TabIndex = 0;
            header1.Text = "ColorPicker 颜色选择器";
            header1.UseTitleFont = true;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.Controls.Add(colorPicker3);
            flowLayoutPanel1.Controls.Add(colorPicker1);
            flowLayoutPanel1.Controls.Add(colorPicker2);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(0, 74);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(740, 328);
            flowLayoutPanel1.TabIndex = 5;
            // 
            // colorPicker3
            // 
            colorPicker3.AutoSizeMode = AntdUI.TAutoSize.Auto;
            colorPicker3.Location = new Point(3, 3);
            colorPicker3.Name = "colorPicker3";
            colorPicker3.Size = new Size(46, 46);
            colorPicker3.TabIndex = 26;
            colorPicker3.Value = Color.FromArgb(22, 119, 255);
            // 
            // colorPicker1
            // 
            colorPicker1.AutoSizeMode = AntdUI.TAutoSize.Auto;
            colorPicker1.Location = new Point(55, 3);
            colorPicker1.Name = "colorPicker1";
            colorPicker1.ShowText = true;
            colorPicker1.Size = new Size(125, 46);
            colorPicker1.TabIndex = 26;
            colorPicker1.Value = Color.FromArgb(22, 119, 255);
            // 
            // colorPicker2
            // 
            colorPicker2.AllowClear = true;
            colorPicker2.AutoSizeMode = AntdUI.TAutoSize.Auto;
            colorPicker2.Location = new Point(186, 3);
            colorPicker2.Name = "colorPicker2";
            colorPicker2.Round = true;
            colorPicker2.ShowClose = true;
            colorPicker2.ShowReset = true;
            colorPicker2.ShowSymbol = true;
            colorPicker2.Size = new Size(46, 46);
            colorPicker2.TabIndex = 26;
            colorPicker2.Text = "C";
            colorPicker2.Value = Color.FromArgb(22, 119, 255);
            // 
            // ColorPicker
            // 
            Controls.Add(flowLayoutPanel1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 12F);
            Name = "ColorPicker";
            Size = new Size(740, 402);
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.PageHeader header1;
        private FlowLayoutPanel flowLayoutPanel1;
        private AntdUI.ColorPicker colorPicker3;
        private AntdUI.ColorPicker colorPicker2;
        private AntdUI.ColorPicker colorPicker1;
    }
}