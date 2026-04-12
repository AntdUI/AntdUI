using System.Drawing;
using System.Windows.Forms;

namespace Demo.Controls
{
    partial class Button5
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
            gridPanel1 = new AntdUI.GridPanel();
            gridPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // b1
            // 
            b1.BorderWidth = 1F;
            b1.IconSvg = "SearchOutlined";
            b1.Location = new Point(0, 0);
            b1.Margin = new Padding(0);
            b1.Name = "b1";
            b1.Size = new Size(102, 40);
            b1.TabIndex = 1;
            b1.Text = "Button";
            b1.Type = AntdUI.TTypeMini.Primary;
            b1.Click += btn_Click;
            // 
            // b2
            // 
            b2.BorderWidth = 1F;
            b2.IconPosition = AntdUI.TAlignMini.Right;
            b2.IconSvg = "SearchOutlined";
            b2.Location = new Point(102, 0);
            b2.Margin = new Padding(0);
            b2.Name = "b2";
            b2.Size = new Size(102, 40);
            b2.TabIndex = 2;
            b2.Text = "Button";
            b2.Type = AntdUI.TTypeMini.Primary;
            b2.Click += btn_Click;
            // 
            // b3
            // 
            b3.BorderWidth = 1F;
            b3.IconSvg = "SearchOutlined";
            b3.Location = new Point(0, 40);
            b3.Margin = new Padding(0);
            b3.Name = "b3";
            b3.Size = new Size(102, 40);
            b3.TabIndex = 3;
            b3.Text = "Button";
            b3.Click += btn_Click;
            // 
            // b4
            // 
            b4.BorderWidth = 1F;
            b4.IconPosition = AntdUI.TAlignMini.Right;
            b4.IconSvg = "SearchOutlined";
            b4.Location = new Point(102, 40);
            b4.Margin = new Padding(0);
            b4.Name = "b4";
            b4.Size = new Size(102, 40);
            b4.TabIndex = 4;
            b4.Text = "Button";
            b4.Click += btn_Click;
            // 
            // b5
            // 
            b5.AutoSizeMode = AntdUI.TAutoSize.Auto;
            b5.BorderWidth = 1F;
            b5.Dock = DockStyle.Left;
            b5.IconPosition = AntdUI.TAlignMini.Top;
            b5.IconSvg = "SearchOutlined";
            b5.LoadingWaveCount = 4;
            b5.LoadingWaveSize = 6;
            b5.LoadingWaveVertical = true;
            b5.Location = new Point(204, 0);
            b5.Name = "b5";
            b5.Size = new Size(59, 55);
            b5.TabIndex = 5;
            b5.Text = "Button";
            b5.Type = AntdUI.TTypeMini.Primary;
            b5.Click += btn_Click;
            // 
            // b6
            // 
            b6.AutoSizeMode = AntdUI.TAutoSize.Auto;
            b6.BorderWidth = 1F;
            b6.Dock = DockStyle.Left;
            b6.IconPosition = AntdUI.TAlignMini.Bottom;
            b6.IconSvg = "SearchOutlined";
            b6.LoadingWaveCount = 4;
            b6.LoadingWaveSize = 6;
            b6.LoadingWaveVertical = true;
            b6.Location = new Point(263, 0);
            b6.Name = "b6";
            b6.Size = new Size(59, 55);
            b6.TabIndex = 6;
            b6.Text = "Button";
            b6.Type = AntdUI.TTypeMini.Primary;
            b6.Click += btn_Click;
            // 
            // b7
            // 
            b7.AutoSizeMode = AntdUI.TAutoSize.Auto;
            b7.BorderWidth = 1F;
            b7.Dock = DockStyle.Left;
            b7.IconPosition = AntdUI.TAlignMini.Top;
            b7.IconSvg = "SearchOutlined";
            b7.LoadingWaveCount = 4;
            b7.LoadingWaveSize = 6;
            b7.LoadingWaveVertical = true;
            b7.Location = new Point(322, 0);
            b7.Name = "b7";
            b7.Size = new Size(59, 55);
            b7.TabIndex = 7;
            b7.Text = "Button";
            b7.Click += btn_Click;
            // 
            // b8
            // 
            b8.AutoSizeMode = AntdUI.TAutoSize.Auto;
            b8.BorderWidth = 1F;
            b8.Dock = DockStyle.Left;
            b8.IconPosition = AntdUI.TAlignMini.Bottom;
            b8.IconSvg = "SearchOutlined";
            b8.LoadingWaveCount = 4;
            b8.LoadingWaveSize = 6;
            b8.LoadingWaveVertical = true;
            b8.Location = new Point(381, 0);
            b8.Name = "b8";
            b8.Size = new Size(59, 55);
            b8.TabIndex = 8;
            b8.Text = "Button";
            b8.Click += btn_Click;
            // 
            // gridPanel1
            // 
            gridPanel1.Controls.Add(b4);
            gridPanel1.Controls.Add(b3);
            gridPanel1.Controls.Add(b2);
            gridPanel1.Controls.Add(b1);
            gridPanel1.Dock = DockStyle.Left;
            gridPanel1.Location = new Point(0, 0);
            gridPanel1.Margin = new Padding(0);
            gridPanel1.Name = "gridPanel1";
            gridPanel1.Size = new Size(204, 80);
            gridPanel1.TabIndex = 1;
            // 
            // Button5
            // 
            AutoScroll = true;
            Controls.Add(b8);
            Controls.Add(b7);
            Controls.Add(b6);
            Controls.Add(b5);
            Controls.Add(gridPanel1);
            Name = "Button5";
            Size = new Size(612, 80);
            gridPanel1.ResumeLayout(false);
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
        private AntdUI.GridPanel gridPanel1;
    }
}