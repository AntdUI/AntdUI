using System.Drawing;
using System.Windows.Forms;

namespace Demo.Controls
{
    partial class Chart
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
            SuspendLayout();
            // 
            // header1
            // 
            header1.Description = "可视化图表库。";
            header1.Dock = DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F);
            header1.LocalizationDescription = "Chart.Description";
            header1.LocalizationText = "Chart.Text";
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new Padding(0, 0, 0, 10);
            header1.Size = new Size(800, 74);
            header1.TabIndex = 0;
            header1.Text = "Chart 图表控件";
            header1.UseTitleFont = true;
            // 
            // panel_main
            // 
            panel_main.Dock = DockStyle.Fill;
            panel_main.Location = new Point(0, 74);
            panel_main.Name = "panel_main";
            panel_main.Size = new Size(800, 526);
            panel_main.TabIndex = 1;
            // 
            // Chart
            // 
            Controls.Add(panel_main);
            Controls.Add(header1);
            Name = "Chart";
            Size = new Size(800, 600);
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.PageHeader header1;
        private System.Windows.Forms.Panel panel_main;
    }
}
