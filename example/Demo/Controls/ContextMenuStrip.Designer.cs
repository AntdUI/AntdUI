using System.Drawing;

namespace Demo.Controls
{
    partial class ContextMenuStrip
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
            components = new System.ComponentModel.Container();
            header1 = new AntdUI.PageHeader();
            button1 = new AntdUI.Button();
            notifyIcon1 = new System.Windows.Forms.NotifyIcon(components);
            button2 = new AntdUI.Button();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Description = "任意点击当前页面的右键";
            header1.Dock = System.Windows.Forms.DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F);
            header1.LocalizationDescription = "ContextMenuStrip.Description";
            header1.LocalizationText = "ContextMenuStrip.Text";
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            header1.Size = new Size(596, 74);
            header1.TabIndex = 1;
            header1.Text = "ContextMenuStrip 右键菜单";
            header1.UseTitleFont = true;
            // 
            // button1
            // 
            button1.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button1.Location = new Point(14, 93);
            button1.Name = "button1";
            button1.Size = new Size(71, 47);
            button1.TabIndex = 2;
            button1.Text = "Click";
            button1.Type = AntdUI.TTypeMini.Primary;
            button1.MouseClick += button1_MouseClick;
            // 
            // notifyIcon1
            // 
            notifyIcon1.Text = "AntdUI - Demo";
            notifyIcon1.BalloonTipClicked += notifyIcon1_BalloonTipClicked;
            notifyIcon1.MouseDown += notifyIcon1_MouseDown;
            // 
            // button2
            // 
            button2.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button2.Location = new Point(100, 93);
            button2.Name = "button2";
            button2.Size = new Size(116, 47);
            button2.TabIndex = 2;
            button2.Text = "NotifyIcon";
            button2.Type = AntdUI.TTypeMini.Primary;
            button2.Click += button2_Click;
            // 
            // ContextMenuStrip
            // 
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 12F);
            Name = "ContextMenuStrip";
            Size = new Size(596, 445);
            MouseClick += ContextMenuStrip_MouseClick;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private AntdUI.PageHeader header1;
        private AntdUI.Button button1;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private AntdUI.Button button2;
    }
}