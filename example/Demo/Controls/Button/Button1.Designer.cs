using System.Drawing;
using System.Windows.Forms;

namespace Demo.Controls
{
    partial class Button1
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
            b1 = new AntdUI.Button();
            b2 = new AntdUI.Button();
            b3 = new AntdUI.Button();
            b4 = new AntdUI.Button();
            b5 = new AntdUI.Button();
            b6 = new AntdUI.Button();
            SuspendLayout();
            // 
            // b1
            // 
            b1.AutoSizeMode = AntdUI.TAutoSize.Auto;
            b1.BorderWidth = 1F;
            b1.Dock = DockStyle.Left;
            b1.Location = new Point(0, 0);
            b1.Name = "b1";
            b1.Size = new Size(111, 36);
            b1.TabIndex = 1;
            b1.Text = "Primary Button";
            b1.Type = AntdUI.TTypeMini.Primary;
            b1.Click += btn_Click;
            // 
            // b2
            // 
            b2.AutoSizeMode = AntdUI.TAutoSize.Auto;
            b2.BorderWidth = 1F;
            b2.Dock = DockStyle.Left;
            b2.Location = new Point(111, 0);
            b2.Name = "b2";
            b2.Size = new Size(109, 36);
            b2.TabIndex = 2;
            b2.Text = "Default Button";
            b2.Click += btn_Click;
            // 
            // b3
            // 
            b3.AutoSizeMode = AntdUI.TAutoSize.Auto;
            b3.Dock = DockStyle.Left;
            b3.Ghost = true;
            b3.Location = new Point(220, 0);
            b3.Name = "b3";
            b3.Size = new Size(91, 36);
            b3.TabIndex = 3;
            b3.Text = "Text Button";
            b3.Click += btn_Click;
            // 
            // b4
            // 
            b4.AutoSizeMode = AntdUI.TAutoSize.Auto;
            b4.Dock = DockStyle.Left;
            b4.Ghost = true;
            b4.Location = new Point(311, 0);
            b4.Name = "b4";
            b4.Size = new Size(90, 36);
            b4.TabIndex = 4;
            b4.Text = "Link Button";
            b4.Type = AntdUI.TTypeMini.Primary;
            b4.Click += btn_Click;
            // 
            // b5
            // 
            b5.AutoSizeMode = AntdUI.TAutoSize.Auto;
            b5.BorderWidth = 1F;
            b5.Dock = DockStyle.Left;
            b5.Location = new Point(401, 0);
            b5.Name = "b5";
            b5.Size = new Size(109, 36);
            b5.TabIndex = 5;
            b5.Text = "Danger Button";
            b5.Type = AntdUI.TTypeMini.Error;
            b5.Click += btn_Click;
            // 
            // b6
            // 
            b6.AutoSizeMode = AntdUI.TAutoSize.Auto;
            b6.BorderWidth = 1F;
            b6.Dock = DockStyle.Left;
            b6.Ghost = true;
            b6.Location = new Point(510, 0);
            b6.Name = "b6";
            b6.Size = new Size(112, 36);
            b6.TabIndex = 6;
            b6.Text = "Danger Default";
            b6.Type = AntdUI.TTypeMini.Error;
            b6.Click += btn_Click;
            // 
            // Button1
            // 
            AutoScroll = true;
            Controls.Add(b6);
            Controls.Add(b5);
            Controls.Add(b4);
            Controls.Add(b3);
            Controls.Add(b2);
            Controls.Add(b1);
            Name = "Button1";
            Size = new Size(629, 60);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private AntdUI.Button b1;
        private AntdUI.Button b2;
        private AntdUI.Button b3;
        private AntdUI.Button b4;
        private AntdUI.Button b5;
        private AntdUI.Button b6;
    }
}