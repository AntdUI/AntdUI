// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
// THE AntdUI PROJECT IS AN WINFORM LIBRARY LICENSED UNDER THE Apache-2.0 License.
// LICENSED UNDER THE Apache License, VERSION 2.0 (THE "License")
// YOU MAY NOT USE THIS FILE EXCEPT IN COMPLIANCE WITH THE License.
// YOU MAY OBTAIN A COPY OF THE LICENSE AT
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// UNLESS REQUIRED BY APPLICABLE LAW OR AGREED TO IN WRITING, SOFTWARE
// DISTRIBUTED UNDER THE LICENSE IS DISTRIBUTED ON AN "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED.
// SEE THE LICENSE FOR THE SPECIFIC LANGUAGE GOVERNING PERMISSIONS AND
// LIMITATIONS UNDER THE License.
// GITEE: https://gitee.com/AntdUI/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

using System.Drawing;

namespace Demo.Controls
{
    partial class Shield
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
            header1 = new AntdUI.PageHeader();
            shield1 = new AntdUI.Shield();
            shield2 = new AntdUI.Shield();
            shield3 = new AntdUI.Shield();
            shield4 = new AntdUI.Shield();
            shield5 = new AntdUI.Shield();
            shield6 = new AntdUI.Shield();
            shield7 = new AntdUI.Shield();
            shield8 = new AntdUI.Shield();
            flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Description = "展示徽章图标。";
            header1.Dock = System.Windows.Forms.DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F);
            header1.LocalizationDescription = "Shield.Description";
            header1.LocalizationText = "Shield.Text";
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            header1.Size = new Size(740, 74);
            header1.TabIndex = 4;
            header1.Text = "Shield 徽章";
            header1.UseTitleFont = true;
            // 
            // shield1
            // 
            shield1.AutoSizeMode = AntdUI.TAutoSize.Auto;
            shield1.Color = Color.FromArgb(0, 126, 198);
            shield1.Label = "AntdUI";
            shield1.Location = new Point(0, 0);
            shield1.Margin = new System.Windows.Forms.Padding(0, 0, 10, 0);
            shield1.Name = "shield1";
            shield1.Radius = 0;
            shield1.Size = new Size(108, 28);
            shield1.TabIndex = 0;
            shield1.Text = "v2.0.7";
            // 
            // shield2
            // 
            shield2.AutoSizeMode = AntdUI.TAutoSize.Auto;
            shield2.Color = Color.FromArgb(247, 70, 88);
            shield2.Label = "QQ群";
            shield2.LocalizationLabel = "Shield.qq";
            shield2.Location = new Point(118, 0);
            shield2.LogoSvg = "QqOutlined";
            shield2.Margin = new System.Windows.Forms.Padding(0, 0, 10, 0);
            shield2.Name = "shield2";
            shield2.Radius = 0;
            shield2.Size = new Size(157, 28);
            shield2.TabIndex = 0;
            shield2.Text = "328884096";
            // 
            // shield3
            // 
            shield3.AutoSizeMode = AntdUI.TAutoSize.Auto;
            shield3.Label = "downloads";
            shield3.Location = new Point(582, 0);
            shield3.Margin = new System.Windows.Forms.Padding(0, 0, 10, 0);
            shield3.Name = "shield3";
            shield3.Radius = 0;
            shield3.Size = new Size(119, 28);
            shield3.TabIndex = 0;
            shield3.Text = "61k";
            // 
            // shield4
            // 
            shield4.AutoSizeMode = AntdUI.TAutoSize.Auto;
            shield4.Color = Color.FromArgb(22, 119, 255);
            shield4.Label = "AntDesign";
            shield4.Location = new Point(436, 0);
            shield4.LogoSvg = "AntDesignOutlined";
            shield4.Margin = new System.Windows.Forms.Padding(0, 0, 10, 0);
            shield4.Name = "shield4";
            shield4.Radius = 0;
            shield4.Size = new Size(136, 28);
            shield4.TabIndex = 0;
            shield4.Text = "5.0";
            // 
            // shield5
            // 
            shield5.AutoSizeMode = AntdUI.TAutoSize.Auto;
            shield5.Color = Color.FromArgb(78, 177, 186);
            shield5.Label = "license";
            shield5.Location = new Point(285, 0);
            shield5.Margin = new System.Windows.Forms.Padding(0, 0, 10, 0);
            shield5.Name = "shield5";
            shield5.Radius = 0;
            shield5.Size = new Size(141, 28);
            shield5.TabIndex = 0;
            shield5.Text = "Apache 2.0";
            // 
            // shield6
            // 
            shield6.AutoSizeMode = AntdUI.TAutoSize.Auto;
            shield6.Color = Color.FromArgb(0, 126, 198);
            shield6.Font = new Font("Microsoft YaHei UI", 12F);
            shield6.Label = "flat style";
            shield6.Location = new Point(14, 149);
            shield6.Name = "shield6";
            shield6.ShadowOffsetY = 2;
            shield6.Size = new Size(157, 34);
            shield6.TabIndex = 0;
            shield6.Text = "you like";
            // 
            // shield7
            // 
            shield7.AutoSizeMode = AntdUI.TAutoSize.Auto;
            shield7.Color = Color.BlueViolet;
            shield7.Font = new Font("Microsoft YaHei UI", 12F);
            shield7.Location = new Point(208, 149);
            shield7.Name = "shield7";
            shield7.ShadowOffsetY = 2;
            shield7.Size = new Size(147, 34);
            shield7.TabIndex = 0;
            shield7.Text = "just the message";
            // 
            // shield8
            // 
            shield8.AutoSizeMode = AntdUI.TAutoSize.Auto;
            shield8.Bold = true;
            shield8.Font = new Font("Microsoft YaHei UI", 12F);
            shield8.Label = "style";
            shield8.Location = new Point(14, 206);
            shield8.Name = "shield8";
            shield8.Radius = 0;
            shield8.Size = new Size(178, 34);
            shield8.TabIndex = 0;
            shield8.Text = "for-the-badge";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            flowLayoutPanel1.Controls.Add(shield1);
            flowLayoutPanel1.Controls.Add(shield2);
            flowLayoutPanel1.Controls.Add(shield5);
            flowLayoutPanel1.Controls.Add(shield4);
            flowLayoutPanel1.Controls.Add(shield3);
            flowLayoutPanel1.Location = new Point(14, 80);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(712, 64);
            flowLayoutPanel1.TabIndex = 0;
            // 
            // Shield
            // 
            Controls.Add(flowLayoutPanel1);
            Controls.Add(shield7);
            Controls.Add(shield8);
            Controls.Add(shield6);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 10F);
            Name = "Shield";
            Size = new Size(740, 514);
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private AntdUI.PageHeader header1;
        private AntdUI.Button button1;
        private AntdUI.Shield shield1;
        private AntdUI.Shield shield2;
        private AntdUI.Shield shield3;
        private AntdUI.Shield shield4;
        private AntdUI.Shield shield5;
        private AntdUI.Shield shield6;
        private AntdUI.Shield shield7;
        private AntdUI.Shield shield8;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}
