using System.Drawing;
using System.Windows.Forms;

namespace Demo.Controls
{
    partial class Button6
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
            panel_btns = new AntdUI.Panel();
            b3 = new AntdUI.Button();
            b2 = new AntdUI.Button();
            b1 = new AntdUI.Button();
            panel2 = new System.Windows.Forms.Panel();
            b5 = new AntdUI.Button();
            b4 = new AntdUI.Button();
            panel_btns.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel_btns
            // 
            panel_btns.Controls.Add(b3);
            panel_btns.Controls.Add(b2);
            panel_btns.Controls.Add(b1);
            panel_btns.Dock = DockStyle.Left;
            panel_btns.Location = new Point(8, 0);
            panel_btns.Name = "panel_btns";
            panel_btns.Shadow = 8;
            panel_btns.Size = new Size(359, 46);
            panel_btns.TabIndex = 1;
            panel_btns.Text = "panel6";
            // 
            // b3
            // 
            b3.AutoSizeMode = AntdUI.TAutoSize.Width;
            b3.BorderWidth = 2F;
            b3.Dock = DockStyle.Left;
            b3.JoinMode = AntdUI.TJoinMode.Right;
            b3.Location = new Point(218, 8);
            b3.Margin = new Padding(0);
            b3.Name = "b3";
            b3.Size = new Size(105, 30);
            b3.TabIndex = 3;
            b3.Text = "Default Button";
            b3.Click += btn_Click;
            // 
            // b2
            // 
            b2.AutoSizeMode = AntdUI.TAutoSize.Width;
            b2.BorderWidth = 2F;
            b2.Dock = DockStyle.Left;
            b2.JoinMode = AntdUI.TJoinMode.LR;
            b2.Location = new Point(113, 8);
            b2.Margin = new Padding(0);
            b2.Name = "b2";
            b2.Size = new Size(105, 30);
            b2.TabIndex = 2;
            b2.Text = "Default Button";
            b2.Click += btn_Click;
            // 
            // b1
            // 
            b1.AutoSizeMode = AntdUI.TAutoSize.Width;
            b1.BorderWidth = 2F;
            b1.Dock = DockStyle.Left;
            b1.JoinMode = AntdUI.TJoinMode.Left;
            b1.Location = new Point(8, 8);
            b1.Margin = new Padding(0);
            b1.Name = "b1";
            b1.Size = new Size(105, 30);
            b1.TabIndex = 1;
            b1.Text = "Default Button";
            b1.Click += btn_Click;
            // 
            // panel2
            // 
            panel2.Controls.Add(b5);
            panel2.Controls.Add(b4);
            panel2.Location = new Point(481, 4);
            panel2.Name = "panel2";
            panel2.Size = new Size(140, 44);
            panel2.TabIndex = 2;
            // 
            // b5
            // 
            b5.BackActive = Color.FromArgb(17, 24, 39);
            b5.BackColor = Color.FromArgb(17, 24, 39);
            b5.BackHover = Color.FromArgb(17, 24, 39);
            b5.Dock = DockStyle.Fill;
            b5.JoinMode = AntdUI.TJoinMode.Right;
            b5.Location = new Point(46, 0);
            b5.Name = "b5";
            b5.Radius = 4;
            b5.Size = new Size(94, 44);
            b5.TabIndex = 5;
            b5.Text = "Button";
            b5.Type = AntdUI.TTypeMini.Primary;
            // 
            // b4
            // 
            b4.BackActive = Color.FromArgb(147, 51, 234);
            b4.BackColor = Color.FromArgb(168, 85, 247);
            b4.BackHover = Color.FromArgb(147, 51, 234);
            b4.Dock = DockStyle.Left;
            b4.IconSvg = "SearchOutlined";
            b4.JoinMode = AntdUI.TJoinMode.Left;
            b4.Location = new Point(0, 0);
            b4.Name = "b4";
            b4.Radius = 4;
            b4.Size = new Size(46, 44);
            b4.TabIndex = 4;
            b4.Type = AntdUI.TTypeMini.Primary;
            b4.Click += btn_Click;
            // 
            // Button6
            // 
            AutoScroll = true;
            Controls.Add(panel2);
            Controls.Add(panel_btns);
            Name = "Button6";
            Padding = new Padding(8, 0, 0, 0);
            Size = new Size(561, 60);
            panel_btns.ResumeLayout(false);
            panel_btns.PerformLayout();
            panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.Panel panel_btns;
        private AntdUI.Button b3;
        private AntdUI.Button b2;
        private AntdUI.Button b1;
        private System.Windows.Forms.Panel panel2;
        private AntdUI.Button b5;
        private AntdUI.Button b4;
    }
}