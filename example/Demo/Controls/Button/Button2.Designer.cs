using System.Drawing;
using System.Windows.Forms;

namespace Demo.Controls
{
    partial class Button2
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
            b7 = new AntdUI.Button();
            b8 = new AntdUI.Button();
            SuspendLayout();
            // 
            // b1
            // 
            b1.AutoSizeMode = AntdUI.TAutoSize.Auto;
            b1.Dock = DockStyle.Left;
            b1.IconSvg = "PoweroffOutlined";
            b1.LoadingWaveVertical = true;
            b1.Location = new Point(0, 0);
            b1.Name = "b1";
            b1.Shape = AntdUI.TShape.Circle;
            b1.Size = new Size(36, 36);
            b1.TabIndex = 1;
            b1.Type = AntdUI.TTypeMini.Primary;
            b1.Click += btn_Click;
            // 
            // b2
            // 
            b2.AutoSizeMode = AntdUI.TAutoSize.Auto;
            b2.Dock = DockStyle.Left;
            b2.IconSvg = "SearchOutlined";
            b2.LocalizationText = "Button.Search";
            b2.Location = new Point(36, 0);
            b2.Name = "b2";
            b2.Size = new Size(63, 36);
            b2.TabIndex = 2;
            b2.Text = "搜索";
            b2.Type = AntdUI.TTypeMini.Primary;
            b2.Click += btn_Click;
            // 
            // b3
            // 
            b3.AutoSizeMode = AntdUI.TAutoSize.Auto;
            b3.BorderWidth = 1F;
            b3.Dock = DockStyle.Left;
            b3.IconSvg = "PoweroffOutlined";
            b3.LoadingWaveVertical = true;
            b3.Location = new Point(99, 0);
            b3.Name = "b3";
            b3.Shape = AntdUI.TShape.Circle;
            b3.Size = new Size(36, 36);
            b3.TabIndex = 3;
            b3.Click += btn_Click;
            // 
            // b4
            // 
            b4.AutoSizeMode = AntdUI.TAutoSize.Auto;
            b4.BorderWidth = 1F;
            b4.Dock = DockStyle.Left;
            b4.IconSvg = "SearchOutlined";
            b4.LocalizationText = "Button.Search";
            b4.Location = new Point(135, 0);
            b4.Name = "b4";
            b4.Size = new Size(63, 36);
            b4.TabIndex = 4;
            b4.Text = "搜索";
            b4.Click += btn_Click;
            // 
            // b5
            // 
            b5.AutoSizeMode = AntdUI.TAutoSize.Auto;
            b5.BorderWidth = 2F;
            b5.Dock = DockStyle.Left;
            b5.IconSvg = "SearchOutlined";
            b5.LoadingWaveVertical = true;
            b5.Location = new Point(198, 0);
            b5.Name = "b5";
            b5.Shape = AntdUI.TShape.Circle;
            b5.Size = new Size(36, 36);
            b5.TabIndex = 5;
            b5.Type = AntdUI.TTypeMini.Error;
            b5.Click += btn_Click;
            // 
            // b6
            // 
            b6.AutoSizeMode = AntdUI.TAutoSize.Auto;
            b6.Dock = DockStyle.Left;
            b6.IconSvg = "SearchOutlined";
            b6.LocalizationText = "Button.Search";
            b6.Location = new Point(234, 0);
            b6.Name = "b6";
            b6.Size = new Size(63, 36);
            b6.TabIndex = 6;
            b6.Text = "搜索";
            b6.Type = AntdUI.TTypeMini.Error;
            b6.Click += btn_Click;
            // 
            // b7
            // 
            b7.AutoSizeMode = AntdUI.TAutoSize.Auto;
            b7.BackExtend = "135, #6253E1, #04BEFE";
            b7.Dock = DockStyle.Left;
            b7.IconSvg = "SearchOutlined";
            b7.Location = new Point(297, 0);
            b7.Name = "b7";
            b7.Size = new Size(132, 36);
            b7.TabIndex = 7;
            b7.Text = "Gradient Button";
            b7.Type = AntdUI.TTypeMini.Primary;
            b7.Click += btn_Click;
            // 
            // b8
            // 
            b8.AutoSizeMode = AntdUI.TAutoSize.Auto;
            b8.Dock = DockStyle.Left;
            b8.Ghost = true;
            b8.IconSvg = "AntDesignOutlined";
            b8.Location = new Point(429, 0);
            b8.Name = "b8";
            b8.Size = new Size(36, 36);
            b8.TabIndex = 8;
            b8.Type = AntdUI.TTypeMini.Primary;
            b8.Click += btn_Click;
            // 
            // Button2
            // 
            AutoScroll = true;
            Controls.Add(b8);
            Controls.Add(b7);
            Controls.Add(b6);
            Controls.Add(b5);
            Controls.Add(b4);
            Controls.Add(b3);
            Controls.Add(b2);
            Controls.Add(b1);
            Name = "Button2";
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
        private AntdUI.Button b7;
        private AntdUI.Button b8;
    }
}