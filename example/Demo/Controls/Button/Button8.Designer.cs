using Demo.Properties;
using System.Drawing;
using System.Windows.Forms;

namespace Demo.Controls
{
    partial class Button8
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
            SuspendLayout();
            // 
            // b1
            // 
            b1.AutoSizeMode = AntdUI.TAutoSize.Auto;
            b1.AutoToggle = true;
            b1.BorderWidth = 1F;
            b1.Dock = DockStyle.Left;
            b1.IconHoverSvg = "SmileFilled";
            b1.IconSvg = "SmileOutlined";
            b1.Location = new Point(0, 0);
            b1.Name = "b1";
            b1.Size = new Size(36, 36);
            b1.TabIndex = 1;
            b1.ToggleIconHoverSvg = "MehFilled";
            b1.ToggleIconSvg = "MehOutlined";
            b1.ToggleType = AntdUI.TTypeMini.Primary;
            // 
            // b2
            // 
            b2.AutoSizeMode = AntdUI.TAutoSize.Auto;
            b2.AutoToggle = true;
            b2.Dock = DockStyle.Left;
            b2.IconHoverSvg = "SmileFilled";
            b2.IconSvg = "SmileOutlined";
            b2.Location = new Point(36, 0);
            b2.Name = "b2";
            b2.Size = new Size(36, 36);
            b2.TabIndex = 2;
            b2.ToggleIconHoverSvg = "MehFilled";
            b2.ToggleIconSvg = "MehOutlined";
            b2.Type = AntdUI.TTypeMini.Primary;
            // 
            // b3
            // 
            b3.AutoSizeMode = AntdUI.TAutoSize.Auto;
            b3.AutoToggle = true;
            b3.BorderWidth = 1F;
            b3.Dock = DockStyle.Left;
            b3.IconSvg = "StarOutlined";
            b3.Location = new Point(72, 0);
            b3.Name = "b3";
            b3.Size = new Size(36, 36);
            b3.TabIndex = 3;
            b3.ToggleIconSvg = "StarFilled";
            b3.ToggleType = AntdUI.TTypeMini.Warn;
            // 
            // Button8
            // 
            AutoScroll = true;
            Controls.Add(b3);
            Controls.Add(b2);
            Controls.Add(b1);
            Name = "Button8";
            Size = new Size(612, 60);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private AntdUI.Button b1;
        private AntdUI.Button b2;
        private AntdUI.Button b3;
    }
}