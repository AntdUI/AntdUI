using System.Drawing;
using System.Windows.Forms;

namespace Demo.Controls
{
    partial class Button3
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
            SuspendLayout();
            // 
            // b1
            // 
            b1.AutoSizeMode = AntdUI.TAutoSize.Auto;
            b1.Dock = DockStyle.Left;
            b1.IconSvg = "DownloadOutlined";
            b1.LoadingWaveVertical = true;
            b1.Location = new Point(0, 0);
            b1.Name = "b1";
            b1.Size = new Size(36, 36);
            b1.TabIndex = 1;
            b1.Type = AntdUI.TTypeMini.Primary;
            b1.Click += btn_Click;
            // 
            // b2
            // 
            b2.AutoSizeMode = AntdUI.TAutoSize.Auto;
            b2.Dock = DockStyle.Left;
            b2.IconSvg = "DownloadOutlined";
            b2.RespondRealAreas = true;
            b2.LoadingWaveVertical = true;
            b2.Location = new Point(36, 0);
            b2.Name = "b2";
            b2.Shape = AntdUI.TShape.Circle;
            b2.Size = new Size(36, 36);
            b2.TabIndex = 2;
            b2.Type = AntdUI.TTypeMini.Primary;
            b2.Click += btn_Click;
            // 
            // b3
            // 
            b3.AutoSizeMode = AntdUI.TAutoSize.Height;
            b3.Dock = DockStyle.Left;
            b3.IconSvg = "DownloadOutlined";
            b3.RespondRealAreas = true;
            b3.LoadingWaveVertical = true;
            b3.Location = new Point(72, 0);
            b3.Name = "b3";
            b3.Shape = AntdUI.TShape.Round;
            b3.Size = new Size(67, 36);
            b3.TabIndex = 3;
            b3.Type = AntdUI.TTypeMini.Primary;
            b3.Click += btn_Click;
            // 
            // b4
            // 
            b4.AutoSizeMode = AntdUI.TAutoSize.Auto;
            b4.Dock = DockStyle.Left;
            b4.IconSvg = "DownloadOutlined";
            b4.RespondRealAreas = true;
            b4.Location = new Point(139, 0);
            b4.Name = "b4";
            b4.Shape = AntdUI.TShape.Round;
            b4.Size = new Size(98, 36);
            b4.TabIndex = 4;
            b4.Text = "Download";
            b4.Type = AntdUI.TTypeMini.Primary;
            b4.Click += btn_Click;
            // 
            // b5
            // 
            b5.AutoSizeMode = AntdUI.TAutoSize.Auto;
            b5.Dock = DockStyle.Left;
            b5.IconSvg = "DownloadOutlined";
            b5.Location = new Point(237, 0);
            b5.Name = "b5";
            b5.Size = new Size(98, 36);
            b5.TabIndex = 5;
            b5.Text = "Download";
            b5.Type = AntdUI.TTypeMini.Primary;
            b5.Click += btn_Click;
            // 
            // Button3
            // 
            AutoScroll = true;
            Controls.Add(b5);
            Controls.Add(b4);
            Controls.Add(b3);
            Controls.Add(b2);
            Controls.Add(b1);
            Name = "Button3";
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
    }
}