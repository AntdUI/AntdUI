using System.Drawing;
using System.Windows.Forms;

namespace Demo.Controls
{
    partial class Button10
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
            vpanel = new FlowLayoutPanel();
            vpanel.SuspendLayout();
            SuspendLayout();
            // 
            // b1
            // 
            b1.AutoSizeMode = AntdUI.TAutoSize.Auto;
            b1.LoadingValue = -1F;
            b1.LocalizationText = "Button.{id}";
            b1.Location = new Point(3, 3);
            b1.Name = "b1";
            b1.Size = new Size(154, 65);
            b1.TabIndex = 1;
            b1.Text = "静态波浪投影";
            b1.Type = AntdUI.TTypeMini.Primary;
            b1.WaveColor = Color.Aqua;
            b1.WaveShadow = true;
            b1.WaveSize = 8;
            b1.Click += b1_Click;
            // 
            // b2
            // 
            b2.AutoSizeMode = AntdUI.TAutoSize.Auto;
            b2.BackColor = Color.Black;
            b2.Loading = true;
            b2.LoadingSpeedTime = 2000;
            b2.LoadingValue = -1F;
            b2.LocalizationText = "Button.{id}";
            b2.Location = new Point(163, 3);
            b2.Name = "b2";
            b2.Size = new Size(154, 65);
            b2.TabIndex = 2;
            b2.Text = "彩虹波浪投影";
            b2.Type = AntdUI.TTypeMini.Primary;
            b2.WaveColorExtend = "90deg, #0095ff, #e74dff, #ff0000, #83ffa6, #0095ff";
            b2.WaveShadow = true;
            b2.WaveSize = 8;
            // 
            // b3
            // 
            b3.AutoSizeMode = AntdUI.TAutoSize.Auto;
            b3.Loading = true;
            b3.LocalizationText = "Button.{id}";
            b3.Location = new Point(323, 3);
            b3.Name = "b3";
            b3.Size = new Size(169, 65);
            b3.TabIndex = 3;
            b3.Text = "彩虹波浪投影";
            b3.Type = AntdUI.TTypeMini.Primary;
            b3.WaveColorExtend = "90deg, #ff4242, #a1ff42, #42a1ff, #42d0ff, #a142ff";
            b3.WaveShadow = true;
            b3.WaveSize = 8;
            // 
            // b4
            // 
            b4.AutoSizeMode = AntdUI.TAutoSize.Auto;
            b4.Loading = true;
            b4.LoadingWaveCount = 3;
            b4.LoadingWaveSize = 4;
            b4.LoadingWaveValue = 0.8F;
            b4.LoadingWaveVertical = true;
            b4.LocalizationText = "Button.{id}";
            b4.Location = new Point(498, 3);
            b4.Name = "b4";
            b4.Size = new Size(205, 77);
            b4.TabIndex = 4;
            b4.Text = "长彩虹波浪投影";
            b4.Type = AntdUI.TTypeMini.Primary;
            b4.WaveColorExtend = "90deg, #ff4242, #a1ff42, #42a1ff, #42d0ff, #a142ff";
            b4.WaveShadow = true;
            b4.WaveShadowSize = 14;
            b4.WaveSize = 2;
            // 
            // vpanel
            // 
            vpanel.AutoScroll = true;
            vpanel.Controls.Add(b1);
            vpanel.Controls.Add(b2);
            vpanel.Controls.Add(b3);
            vpanel.Controls.Add(b4);
            vpanel.Dock = DockStyle.Fill;
            vpanel.Location = new Point(0, 0);
            vpanel.Name = "vpanel";
            vpanel.Size = new Size(1000, 90);
            vpanel.TabIndex = 0;
            // 
            // Button10
            // 
            Controls.Add(vpanel);
            Name = "Button10";
            Size = new Size(1000, 90);
            vpanel.ResumeLayout(false);
            vpanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.Button b3;
        private AntdUI.Button b1;
        private AntdUI.Button b4;
        private AntdUI.Button b2;
        private FlowLayoutPanel vpanel;
    }
}