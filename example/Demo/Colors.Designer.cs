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
// GITCODE: https://gitcode.com/AntdUI/AntdUI
// GITEE: https://gitee.com/AntdUI/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

using System.Drawing;
using System.Windows.Forms;

namespace Demo
{
    partial class Colors
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
            panel_primary = new AntdUI.Panel();
            color_primary = new AntdUI.ColorPanelLeft();
            tablePanel = new TableLayoutPanel();
            colorPanel10 = new AntdUI.ColorPanel();
            colorPanel9 = new AntdUI.ColorPanel();
            colorPanel8 = new AntdUI.ColorPanel();
            colorPanel7 = new AntdUI.ColorPanel();
            colorPanel6 = new AntdUI.ColorPanel();
            colorPanel5 = new AntdUI.ColorPanel();
            colorPanel4 = new AntdUI.ColorPanel();
            colorPanel3 = new AntdUI.ColorPanel();
            colorPanel2 = new AntdUI.ColorPanel();
            colorPanel1 = new AntdUI.ColorPanel();
            textBox1 = new AntdUI.Input();
            panel11 = new AntdUI.Panel();
            color_dark = new AntdUI.ColorPanelLeft();
            panel_primary.SuspendLayout();
            tablePanel.SuspendLayout();
            panel11.SuspendLayout();
            SuspendLayout();
            // 
            // panel_primary
            // 
            panel_primary.Back = Color.FromArgb(24, 144, 255);
            panel_primary.BackColor = Color.Transparent;
            panel_primary.Controls.Add(color_primary);
            panel_primary.ForeColor = Color.White;
            panel_primary.Location = new Point(13, 13);
            panel_primary.Name = "panel_primary";
            panel_primary.Size = new Size(234, 80);
            panel_primary.TabIndex = 1;
            panel_primary.Text = "panel1";
            // 
            // color_primary
            // 
            color_primary.Dock = DockStyle.Fill;
            color_primary.Location = new Point(0, 0);
            color_primary.Name = "color_primary";
            color_primary.Padding = new Padding(13, 0, 0, 0);
            color_primary.Size = new Size(234, 80);
            color_primary.TabIndex = 0;
            color_primary.Text = "Selected Brand Color";
            color_primary.TextDesc = "#1890FF";
            // 
            // tablePanel
            // 
            tablePanel.ColumnCount = 10;
            tablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tablePanel.Controls.Add(colorPanel10, 9, 0);
            tablePanel.Controls.Add(colorPanel9, 8, 0);
            tablePanel.Controls.Add(colorPanel8, 7, 0);
            tablePanel.Controls.Add(colorPanel7, 6, 0);
            tablePanel.Controls.Add(colorPanel6, 5, 0);
            tablePanel.Controls.Add(colorPanel5, 4, 0);
            tablePanel.Controls.Add(colorPanel4, 3, 0);
            tablePanel.Controls.Add(colorPanel3, 2, 0);
            tablePanel.Controls.Add(colorPanel2, 1, 0);
            tablePanel.Controls.Add(colorPanel1, 0, 0);
            tablePanel.Dock = DockStyle.Bottom;
            tablePanel.Location = new Point(0, 174);
            tablePanel.Name = "tablePanel";
            tablePanel.RowCount = 1;
            tablePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tablePanel.Size = new Size(969, 110);
            tablePanel.TabIndex = 2;
            // 
            // colorPanel10
            // 
            colorPanel10.Dock = DockStyle.Fill;
            colorPanel10.Location = new Point(864, 20);
            colorPanel10.Margin = new Padding(0, 20, 0, 0);
            colorPanel10.Name = "colorPanel10";
            colorPanel10.Size = new Size(105, 90);
            colorPanel10.TabIndex = 13;
            colorPanel10.Text = "color-10";
            colorPanel10.Click += ColorPanel_Click;
            colorPanel10.Leave += ColorPanel_MouseLeave;
            colorPanel10.MouseEnter += ColorPanel_MouseEnter;
            colorPanel10.MouseLeave += ColorPanel_MouseLeave;
            // 
            // colorPanel9
            // 
            colorPanel9.Dock = DockStyle.Fill;
            colorPanel9.Location = new Point(768, 20);
            colorPanel9.Margin = new Padding(0, 20, 0, 0);
            colorPanel9.Name = "colorPanel9";
            colorPanel9.Size = new Size(96, 90);
            colorPanel9.TabIndex = 12;
            colorPanel9.Text = "color-9";
            colorPanel9.Click += ColorPanel_Click;
            colorPanel9.Leave += ColorPanel_MouseLeave;
            colorPanel9.MouseEnter += ColorPanel_MouseEnter;
            colorPanel9.MouseLeave += ColorPanel_MouseLeave;
            // 
            // colorPanel8
            // 
            colorPanel8.Dock = DockStyle.Fill;
            colorPanel8.Location = new Point(672, 20);
            colorPanel8.Margin = new Padding(0, 20, 0, 0);
            colorPanel8.Name = "colorPanel8";
            colorPanel8.Size = new Size(96, 90);
            colorPanel8.TabIndex = 11;
            colorPanel8.Text = "color-8";
            colorPanel8.Click += ColorPanel_Click;
            colorPanel8.Leave += ColorPanel_MouseLeave;
            colorPanel8.MouseEnter += ColorPanel_MouseEnter;
            colorPanel8.MouseLeave += ColorPanel_MouseLeave;
            // 
            // colorPanel7
            // 
            colorPanel7.Dock = DockStyle.Fill;
            colorPanel7.Location = new Point(576, 20);
            colorPanel7.Margin = new Padding(0, 20, 0, 0);
            colorPanel7.Name = "colorPanel7";
            colorPanel7.Size = new Size(96, 90);
            colorPanel7.TabIndex = 10;
            colorPanel7.Text = "color-7";
            colorPanel7.Click += ColorPanel_Click;
            colorPanel7.Leave += ColorPanel_MouseLeave;
            colorPanel7.MouseEnter += ColorPanel_MouseEnter;
            colorPanel7.MouseLeave += ColorPanel_MouseLeave;
            // 
            // colorPanel6
            // 
            colorPanel6.Dock = DockStyle.Fill;
            colorPanel6.Location = new Point(480, 20);
            colorPanel6.Margin = new Padding(0, 20, 0, 0);
            colorPanel6.Name = "colorPanel6";
            colorPanel6.Size = new Size(96, 90);
            colorPanel6.TabIndex = 9;
            colorPanel6.Text = "color-6";
            colorPanel6.Click += ColorPanel_Click;
            colorPanel6.Leave += ColorPanel_MouseLeave;
            colorPanel6.MouseEnter += ColorPanel_MouseEnter;
            colorPanel6.MouseLeave += ColorPanel_MouseLeave;
            // 
            // colorPanel5
            // 
            colorPanel5.Dock = DockStyle.Fill;
            colorPanel5.Location = new Point(384, 20);
            colorPanel5.Margin = new Padding(0, 20, 0, 0);
            colorPanel5.Name = "colorPanel5";
            colorPanel5.Size = new Size(96, 90);
            colorPanel5.TabIndex = 8;
            colorPanel5.Text = "color-5";
            colorPanel5.Click += ColorPanel_Click;
            colorPanel5.Leave += ColorPanel_MouseLeave;
            colorPanel5.MouseEnter += ColorPanel_MouseEnter;
            colorPanel5.MouseLeave += ColorPanel_MouseLeave;
            // 
            // colorPanel4
            // 
            colorPanel4.Dock = DockStyle.Fill;
            colorPanel4.Location = new Point(288, 20);
            colorPanel4.Margin = new Padding(0, 20, 0, 0);
            colorPanel4.Name = "colorPanel4";
            colorPanel4.Size = new Size(96, 90);
            colorPanel4.TabIndex = 7;
            colorPanel4.Text = "color-4";
            colorPanel4.Click += ColorPanel_Click;
            colorPanel4.Leave += ColorPanel_MouseLeave;
            colorPanel4.MouseEnter += ColorPanel_MouseEnter;
            colorPanel4.MouseLeave += ColorPanel_MouseLeave;
            // 
            // colorPanel3
            // 
            colorPanel3.Dock = DockStyle.Fill;
            colorPanel3.Location = new Point(192, 20);
            colorPanel3.Margin = new Padding(0, 20, 0, 0);
            colorPanel3.Name = "colorPanel3";
            colorPanel3.Size = new Size(96, 90);
            colorPanel3.TabIndex = 6;
            colorPanel3.Text = "color-3";
            colorPanel3.Click += ColorPanel_Click;
            colorPanel3.Leave += ColorPanel_MouseLeave;
            colorPanel3.MouseEnter += ColorPanel_MouseEnter;
            colorPanel3.MouseLeave += ColorPanel_MouseLeave;
            // 
            // colorPanel2
            // 
            colorPanel2.Dock = DockStyle.Fill;
            colorPanel2.Location = new Point(96, 20);
            colorPanel2.Margin = new Padding(0, 20, 0, 0);
            colorPanel2.Name = "colorPanel2";
            colorPanel2.Size = new Size(96, 90);
            colorPanel2.TabIndex = 5;
            colorPanel2.Text = "color-2";
            colorPanel2.Click += ColorPanel_Click;
            colorPanel2.Leave += ColorPanel_MouseLeave;
            colorPanel2.MouseEnter += ColorPanel_MouseEnter;
            colorPanel2.MouseLeave += ColorPanel_MouseLeave;
            // 
            // colorPanel1
            // 
            colorPanel1.Dock = DockStyle.Fill;
            colorPanel1.Location = new Point(0, 20);
            colorPanel1.Margin = new Padding(0, 20, 0, 0);
            colorPanel1.Name = "colorPanel1";
            colorPanel1.Size = new Size(96, 90);
            colorPanel1.TabIndex = 4;
            colorPanel1.Text = "color-1";
            colorPanel1.Click += ColorPanel_Click;
            colorPanel1.Leave += ColorPanel_MouseLeave;
            colorPanel1.MouseEnter += ColorPanel_MouseEnter;
            colorPanel1.MouseLeave += ColorPanel_MouseLeave;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(13, 100);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(114, 38);
            textBox1.TabIndex = 3;
            textBox1.Text = "#1890FF";
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // panel11
            // 
            panel11.Back = Color.FromArgb(24, 144, 255);
            panel11.BackColor = Color.Transparent;
            panel11.Controls.Add(color_dark);
            panel11.ForeColor = Color.White;
            panel11.Location = new Point(268, 13);
            panel11.Margin = new Padding(4);
            panel11.Name = "panel11";
            panel11.Size = new Size(234, 80);
            panel11.TabIndex = 1;
            panel11.Text = "panel1";
            // 
            // color_dark
            // 
            color_dark.Dock = DockStyle.Fill;
            color_dark.Location = new Point(0, 0);
            color_dark.Name = "color_dark";
            color_dark.Padding = new Padding(13, 0, 0, 0);
            color_dark.Size = new Size(234, 80);
            color_dark.TabIndex = 0;
            color_dark.Text = "Dark Brand Color";
            // 
            // Colors
            // 
            ClientSize = new Size(969, 284);
            Controls.Add(textBox1);
            Controls.Add(tablePanel);
            Controls.Add(panel11);
            Controls.Add(panel_primary);
            Font = new Font("Microsoft YaHei UI", 14F);
            MinimumSize = new Size(980, 310);
            Name = "Colors";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "色板生成工具";
            panel_primary.ResumeLayout(false);
            tablePanel.ResumeLayout(false);
            panel11.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.Panel panel_primary;
        private TableLayoutPanel tablePanel;
        private AntdUI.Input textBox1;
        private AntdUI.Panel panel11;
        private AntdUI.ColorPanel colorPanel1;
        private AntdUI.ColorPanel colorPanel10;
        private AntdUI.ColorPanel colorPanel9;
        private AntdUI.ColorPanel colorPanel8;
        private AntdUI.ColorPanel colorPanel7;
        private AntdUI.ColorPanel colorPanel6;
        private AntdUI.ColorPanel colorPanel5;
        private AntdUI.ColorPanel colorPanel4;
        private AntdUI.ColorPanel colorPanel3;
        private AntdUI.ColorPanel colorPanel2;
        private AntdUI.ColorPanelLeft color_primary;
        private AntdUI.ColorPanelLeft color_dark;
    }
}