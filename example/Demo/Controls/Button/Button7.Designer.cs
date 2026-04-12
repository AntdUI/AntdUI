using System.Drawing;
using System.Windows.Forms;

namespace Demo.Controls
{
    partial class Button7
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
            SuspendLayout();
            // 
            // b1
            // 
            b1.AutoSizeMode = AntdUI.TAutoSize.Auto;
            b1.BackExtend = "135, #6253E1, #04BEFE";
            b1.Dock = DockStyle.Left;
            b1.IconSvg = "AntDesignOutlined";
            b1.Location = new Point(8, 0);
            b1.Name = "b1";
            b1.Size = new Size(132, 36);
            b1.TabIndex = 1;
            b1.Text = "Gradient Button";
            b1.Type = AntdUI.TTypeMini.Primary;
            b1.Click += btn_Click;
            // 
            // b2
            // 
            b2.AutoSizeMode = AntdUI.TAutoSize.Auto;
            b2.BackActive = Color.FromArgb(80, 0, 0, 0);
            b2.BackExtend = "135deg, #8A2387, #E94057, #F27121";
            b2.BackHover = Color.FromArgb(40, 0, 0, 0);
            b2.Dock = DockStyle.Left;
            b2.IconSvg = "StarFilled";
            b2.Location = new Point(140, 0);
            b2.Name = "b2";
            b2.Size = new Size(132, 36);
            b2.TabIndex = 2;
            b2.Text = "Gradient Button";
            b2.Type = AntdUI.TTypeMini.Primary;
            b2.Click += btn_Click;
            // 
            // b3
            // 
            b3.AutoSizeMode = AntdUI.TAutoSize.Auto;
            b3.BackActive = Color.FromArgb(200, 255, 255, 255);
            b3.BackExtend = "90deg, #ff0000aa, #ff7f00aa, #ffff00aa, #00ff00aa, #0000ffaa, #4b0082aa, #9400d3aa";
            b3.BackHover = Color.FromArgb(100, 255, 255, 255);
            b3.Dock = DockStyle.Left;
            b3.ForeColor = Color.Black;
            b3.IconSvg = "SunFilled";
            b3.Location = new Point(272, 0);
            b3.Name = "b3";
            b3.Radius = 12;
            b3.Size = new Size(143, 36);
            b3.TabIndex = 3;
            b3.Text = "Rainbow Gradient";
            b3.Type = AntdUI.TTypeMini.Primary;
            b3.Click += btn_Click;
            // 
            // b4
            // 
            b4.AutoSizeMode = AntdUI.TAutoSize.Auto;
            b4.AutoToggle = true;
            b4.BackActive = Color.FromArgb(80, 0, 0, 0);
            b4.BackExtend = "135deg, #ff9ff3, #54a0ff";
            b4.BackHover = Color.FromArgb(40, 0, 0, 0);
            b4.Dock = DockStyle.Left;
            b4.Location = new Point(415, 0);
            b4.Name = "b4";
            b4.Shape = AntdUI.TShape.Round;
            b4.Size = new Size(93, 36);
            b4.TabIndex = 4;
            b4.Text = "Hello World";
            b4.ToggleBackActive = Color.FromArgb(80, 0, 0, 0);
            b4.ToggleBackExtend = "135deg, #a8edea, #fed6e3";
            b4.ToggleBackHover = Color.FromArgb(40, 0, 0, 0);
            b4.ToggleFore = Color.Black;
            b4.Type = AntdUI.TTypeMini.Primary;
            b4.Click += btn_Click;
            // 
            // Button7
            // 
            AutoScroll = true;
            Controls.Add(b4);
            Controls.Add(b3);
            Controls.Add(b2);
            Controls.Add(b1);
            Name = "Button7";
            Size = new Size(612, 60);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private AntdUI.Button b1;
        private AntdUI.Button b2;
        private AntdUI.Button b3;
        private AntdUI.Button b4;
    }
}