using System.Drawing;
using System.Windows.Forms;

namespace Demo.Controls
{
    partial class Button9
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
            b2 = new AntdUI.Button();
            b1 = new AntdUI.Button();
            b3 = new AntdUI.Button();
            b4 = new AntdUI.Button();
            b5 = new AntdUI.Button();
            SuspendLayout();
            // 
            // b2
            // 
            b2.AutoSizeMode = AntdUI.TAutoSize.Auto;
            b2.BorderWidth = 1F;
            b2.Dock = DockStyle.Left;
            b2.Location = new Point(68, 0);
            b2.Name = "b2";
            b2.Size = new Size(52, 36);
            b2.TabIndex = 2;
            b2.Text = "Error";
            b2.Type = AntdUI.TTypeMini.Error;
            b2.Click += b2_Click;
            // 
            // b1
            // 
            b1.AutoSizeMode = AntdUI.TAutoSize.Auto;
            b1.BorderWidth = 1F;
            b1.Dock = DockStyle.Left;
            b1.Location = new Point(0, 0);
            b1.Name = "b1";
            b1.Size = new Size(68, 36);
            b1.TabIndex = 1;
            b1.Text = "Primary";
            b1.Type = AntdUI.TTypeMini.Primary;
            b1.Click += b1_Click;
            // 
            // b3
            // 
            b3.AutoSizeMode = AntdUI.TAutoSize.Auto;
            b3.BorderWidth = 1F;
            b3.Dock = DockStyle.Left;
            b3.Ghost = true;
            b3.Location = new Point(120, 0);
            b3.Name = "b3";
            b3.Size = new Size(98, 36);
            b3.TabIndex = 3;
            b3.Text = "Error Default";
            b3.Type = AntdUI.TTypeMini.Error;
            b3.Click += b2_Click;
            // 
            // b4
            // 
            b4.AutoSizeMode = AntdUI.TAutoSize.Auto;
            b4.BorderWidth = 1F;
            b4.Dock = DockStyle.Left;
            b4.Location = new Point(218, 0);
            b4.Name = "b4";
            b4.Size = new Size(55, 36);
            b4.TabIndex = 4;
            b4.Text = "Warn";
            b4.Type = AntdUI.TTypeMini.Warn;
            b4.Click += b4_Click;
            // 
            // b5
            // 
            b5.AutoSizeMode = AntdUI.TAutoSize.Auto;
            b5.BorderWidth = 1F;
            b5.Dock = DockStyle.Left;
            b5.Location = new Point(273, 0);
            b5.Name = "b5";
            b5.Size = new Size(69, 36);
            b5.TabIndex = 5;
            b5.Text = "Success";
            b5.Type = AntdUI.TTypeMini.Success;
            b5.Click += b5_Click;
            // 
            // Button9
            // 
            AutoScroll = true;
            Controls.Add(b5);
            Controls.Add(b4);
            Controls.Add(b3);
            Controls.Add(b2);
            Controls.Add(b1);
            Name = "Button9";
            Size = new Size(612, 43);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private AntdUI.Button b2;
        private AntdUI.Button b1;
        private AntdUI.Button b3;
        private AntdUI.Button b4;
        private AntdUI.Button b5;
    }
}