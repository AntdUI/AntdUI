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
            colorLight10 = new AntdUI.ColorPanel();
            colorLight9 = new AntdUI.ColorPanel();
            colorLight8 = new AntdUI.ColorPanel();
            colorLight7 = new AntdUI.ColorPanel();
            colorLight6 = new AntdUI.ColorPanel();
            colorLight5 = new AntdUI.ColorPanel();
            colorLight4 = new AntdUI.ColorPanel();
            colorLight3 = new AntdUI.ColorPanel();
            colorLight2 = new AntdUI.ColorPanel();
            colorLight1 = new AntdUI.ColorPanel();
            textBox1 = new AntdUI.Input();
            panel11 = new AntdUI.Panel();
            color_dark = new AntdUI.ColorPanelLeft();
            tableLPanel = new TableLayoutPanel();
            panel1 = new AntdUI.Panel();
            panel2 = new AntdUI.Panel();
            tablePanelDark = new TableLayoutPanel();
            colorDark10 = new AntdUI.ColorPanel();
            colorDark9 = new AntdUI.ColorPanel();
            colorDark8 = new AntdUI.ColorPanel();
            colorDark7 = new AntdUI.ColorPanel();
            colorDark6 = new AntdUI.ColorPanel();
            colorDark5 = new AntdUI.ColorPanel();
            colorDark4 = new AntdUI.ColorPanel();
            colorDark3 = new AntdUI.ColorPanel();
            colorDark2 = new AntdUI.ColorPanel();
            colorDark1 = new AntdUI.ColorPanel();
            panel_primary.SuspendLayout();
            tablePanel.SuspendLayout();
            panel11.SuspendLayout();
            tableLPanel.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            tablePanelDark.SuspendLayout();
            SuspendLayout();
            // 
            // panel_primary
            // 
            panel_primary.Back = Color.FromArgb(22, 119, 255);
            panel_primary.BackColor = Color.Transparent;
            panel_primary.Controls.Add(color_primary);
            panel_primary.ForeColor = Color.White;
            panel_primary.Location = new Point(13, 13);
            panel_primary.Name = "panel_primary";
            panel_primary.Size = new Size(234, 80);
            panel_primary.TabIndex = 1;
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
            color_primary.TextDesc = "#1677FF";
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
            tablePanel.Controls.Add(colorLight10, 9, 0);
            tablePanel.Controls.Add(colorLight9, 8, 0);
            tablePanel.Controls.Add(colorLight8, 7, 0);
            tablePanel.Controls.Add(colorLight7, 6, 0);
            tablePanel.Controls.Add(colorLight6, 5, 0);
            tablePanel.Controls.Add(colorLight5, 4, 0);
            tablePanel.Controls.Add(colorLight4, 3, 0);
            tablePanel.Controls.Add(colorLight3, 2, 0);
            tablePanel.Controls.Add(colorLight2, 1, 0);
            tablePanel.Controls.Add(colorLight1, 0, 0);
            tablePanel.Dock = DockStyle.Bottom;
            tablePanel.Location = new Point(0, 115);
            tablePanel.Name = "tablePanel";
            tablePanel.RowCount = 1;
            tablePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tablePanel.Size = new Size(964, 110);
            tablePanel.TabIndex = 2;
            // 
            // colorLight10
            // 
            colorLight10.Dock = DockStyle.Fill;
            colorLight10.Location = new Point(864, 20);
            colorLight10.Margin = new Padding(0, 20, 0, 0);
            colorLight10.Name = "colorLight10";
            colorLight10.Size = new Size(100, 90);
            colorLight10.TabIndex = 9;
            colorLight10.Text = "color-10";
            colorLight10.Click += ColorPanel_Click;
            colorLight10.Leave += ColorPanel_MouseLeave;
            colorLight10.MouseEnter += ColorPanel_MouseEnter;
            colorLight10.MouseLeave += ColorPanel_MouseLeave;
            // 
            // colorLight9
            // 
            colorLight9.Dock = DockStyle.Fill;
            colorLight9.Location = new Point(768, 20);
            colorLight9.Margin = new Padding(0, 20, 0, 0);
            colorLight9.Name = "colorLight9";
            colorLight9.Size = new Size(96, 90);
            colorLight9.TabIndex = 8;
            colorLight9.Text = "color-9";
            colorLight9.Click += ColorPanel_Click;
            colorLight9.Leave += ColorPanel_MouseLeave;
            colorLight9.MouseEnter += ColorPanel_MouseEnter;
            colorLight9.MouseLeave += ColorPanel_MouseLeave;
            // 
            // colorLight8
            // 
            colorLight8.Dock = DockStyle.Fill;
            colorLight8.Location = new Point(672, 20);
            colorLight8.Margin = new Padding(0, 20, 0, 0);
            colorLight8.Name = "colorLight8";
            colorLight8.Size = new Size(96, 90);
            colorLight8.TabIndex = 7;
            colorLight8.Text = "color-8";
            colorLight8.Click += ColorPanel_Click;
            colorLight8.Leave += ColorPanel_MouseLeave;
            colorLight8.MouseEnter += ColorPanel_MouseEnter;
            colorLight8.MouseLeave += ColorPanel_MouseLeave;
            // 
            // colorLight7
            // 
            colorLight7.Dock = DockStyle.Fill;
            colorLight7.Location = new Point(576, 20);
            colorLight7.Margin = new Padding(0, 20, 0, 0);
            colorLight7.Name = "colorLight7";
            colorLight7.Size = new Size(96, 90);
            colorLight7.TabIndex = 6;
            colorLight7.Text = "color-7";
            colorLight7.Click += ColorPanel_Click;
            colorLight7.Leave += ColorPanel_MouseLeave;
            colorLight7.MouseEnter += ColorPanel_MouseEnter;
            colorLight7.MouseLeave += ColorPanel_MouseLeave;
            // 
            // colorLight6
            // 
            colorLight6.Dock = DockStyle.Fill;
            colorLight6.Location = new Point(480, 20);
            colorLight6.Margin = new Padding(0, 20, 0, 0);
            colorLight6.Name = "colorLight6";
            colorLight6.Size = new Size(96, 90);
            colorLight6.TabIndex = 5;
            colorLight6.Text = "color-6";
            colorLight6.Click += ColorPanel_Click;
            colorLight6.Leave += ColorPanel_MouseLeave;
            colorLight6.MouseEnter += ColorPanel_MouseEnter;
            colorLight6.MouseLeave += ColorPanel_MouseLeave;
            // 
            // colorLight5
            // 
            colorLight5.Dock = DockStyle.Fill;
            colorLight5.Location = new Point(384, 20);
            colorLight5.Margin = new Padding(0, 20, 0, 0);
            colorLight5.Name = "colorLight5";
            colorLight5.Size = new Size(96, 90);
            colorLight5.TabIndex = 4;
            colorLight5.Text = "color-5";
            colorLight5.Click += ColorPanel_Click;
            colorLight5.Leave += ColorPanel_MouseLeave;
            colorLight5.MouseEnter += ColorPanel_MouseEnter;
            colorLight5.MouseLeave += ColorPanel_MouseLeave;
            // 
            // colorLight4
            // 
            colorLight4.Dock = DockStyle.Fill;
            colorLight4.Location = new Point(288, 20);
            colorLight4.Margin = new Padding(0, 20, 0, 0);
            colorLight4.Name = "colorLight4";
            colorLight4.Size = new Size(96, 90);
            colorLight4.TabIndex = 3;
            colorLight4.Text = "color-4";
            colorLight4.Click += ColorPanel_Click;
            colorLight4.Leave += ColorPanel_MouseLeave;
            colorLight4.MouseEnter += ColorPanel_MouseEnter;
            colorLight4.MouseLeave += ColorPanel_MouseLeave;
            // 
            // colorLight3
            // 
            colorLight3.Dock = DockStyle.Fill;
            colorLight3.Location = new Point(192, 20);
            colorLight3.Margin = new Padding(0, 20, 0, 0);
            colorLight3.Name = "colorLight3";
            colorLight3.Size = new Size(96, 90);
            colorLight3.TabIndex = 2;
            colorLight3.Text = "color-3";
            colorLight3.Click += ColorPanel_Click;
            colorLight3.Leave += ColorPanel_MouseLeave;
            colorLight3.MouseEnter += ColorPanel_MouseEnter;
            colorLight3.MouseLeave += ColorPanel_MouseLeave;
            // 
            // colorLight2
            // 
            colorLight2.Dock = DockStyle.Fill;
            colorLight2.Location = new Point(96, 20);
            colorLight2.Margin = new Padding(0, 20, 0, 0);
            colorLight2.Name = "colorLight2";
            colorLight2.Size = new Size(96, 90);
            colorLight2.TabIndex = 1;
            colorLight2.Text = "color-2";
            colorLight2.Click += ColorPanel_Click;
            colorLight2.Leave += ColorPanel_MouseLeave;
            colorLight2.MouseEnter += ColorPanel_MouseEnter;
            colorLight2.MouseLeave += ColorPanel_MouseLeave;
            // 
            // colorLight1
            // 
            colorLight1.Dock = DockStyle.Fill;
            colorLight1.Location = new Point(0, 20);
            colorLight1.Margin = new Padding(0, 20, 0, 0);
            colorLight1.Name = "colorLight1";
            colorLight1.Size = new Size(96, 90);
            colorLight1.TabIndex = 0;
            colorLight1.Text = "color-1";
            colorLight1.Click += ColorPanel_Click;
            colorLight1.Leave += ColorPanel_MouseLeave;
            colorLight1.MouseEnter += ColorPanel_MouseEnter;
            colorLight1.MouseLeave += ColorPanel_MouseLeave;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(270, 13);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(114, 38);
            textBox1.TabIndex = 3;
            textBox1.Text = "#1677FF";
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // panel11
            // 
            panel11.Back = Color.FromArgb(24, 144, 255);
            panel11.BackColor = Color.Transparent;
            panel11.Controls.Add(color_dark);
            panel11.ForeColor = Color.White;
            panel11.Location = new Point(13, 13);
            panel11.Margin = new Padding(4);
            panel11.Name = "panel11";
            panel11.Size = new Size(234, 80);
            panel11.TabIndex = 1;
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
            // tableLPanel
            // 
            tableLPanel.ColumnCount = 1;
            tableLPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLPanel.Controls.Add(panel1, 0, 0);
            tableLPanel.Controls.Add(panel2, 0, 1);
            tableLPanel.Dock = DockStyle.Fill;
            tableLPanel.Location = new Point(0, 0);
            tableLPanel.Name = "tableLPanel";
            tableLPanel.RowCount = 2;
            tableLPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 49F));
            tableLPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 51F));
            tableLPanel.Size = new Size(964, 461);
            tableLPanel.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Back = Color.White;
            panel1.BackColor = Color.White;
            panel1.ColorScheme = AntdUI.TAMode.Light;
            panel1.Controls.Add(textBox1);
            panel1.Controls.Add(tablePanel);
            panel1.Controls.Add(panel_primary);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(0);
            panel1.Name = "panel1";
            panel1.Radius = 0;
            panel1.Size = new Size(964, 225);
            panel1.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.Back = Color.FromArgb(20, 20, 20);
            panel2.BackColor = Color.FromArgb(20, 20, 20);
            panel2.ColorScheme = AntdUI.TAMode.Dark;
            panel2.Controls.Add(tablePanelDark);
            panel2.Controls.Add(panel11);
            panel2.Dock = DockStyle.Fill;
            panel2.ForeColor = Color.White;
            panel2.Location = new Point(0, 225);
            panel2.Margin = new Padding(0);
            panel2.Name = "panel2";
            panel2.Radius = 0;
            panel2.Size = new Size(964, 236);
            panel2.TabIndex = 1;
            // 
            // tablePanelDark
            // 
            tablePanelDark.ColumnCount = 10;
            tablePanelDark.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tablePanelDark.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tablePanelDark.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tablePanelDark.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tablePanelDark.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tablePanelDark.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tablePanelDark.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tablePanelDark.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tablePanelDark.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tablePanelDark.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tablePanelDark.Controls.Add(colorDark10, 9, 0);
            tablePanelDark.Controls.Add(colorDark9, 8, 0);
            tablePanelDark.Controls.Add(colorDark8, 7, 0);
            tablePanelDark.Controls.Add(colorDark7, 6, 0);
            tablePanelDark.Controls.Add(colorDark6, 5, 0);
            tablePanelDark.Controls.Add(colorDark5, 4, 0);
            tablePanelDark.Controls.Add(colorDark4, 3, 0);
            tablePanelDark.Controls.Add(colorDark3, 2, 0);
            tablePanelDark.Controls.Add(colorDark2, 1, 0);
            tablePanelDark.Controls.Add(colorDark1, 0, 0);
            tablePanelDark.Dock = DockStyle.Bottom;
            tablePanelDark.Location = new Point(0, 126);
            tablePanelDark.Name = "tablePanelDark";
            tablePanelDark.RowCount = 1;
            tablePanelDark.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tablePanelDark.Size = new Size(964, 110);
            tablePanelDark.TabIndex = 3;
            // 
            // colorDark10
            // 
            colorDark10.Dock = DockStyle.Fill;
            colorDark10.Location = new Point(864, 20);
            colorDark10.Margin = new Padding(0, 20, 0, 0);
            colorDark10.Name = "colorDark10";
            colorDark10.Size = new Size(100, 90);
            colorDark10.TabIndex = 9;
            colorDark10.Text = "color-10";
            colorDark10.Click += ColorPanel_Click;
            colorDark10.MouseEnter += ColorPanel_MouseEnter;
            colorDark10.MouseLeave += ColorPanel_MouseLeave;
            // 
            // colorDark9
            // 
            colorDark9.Dock = DockStyle.Fill;
            colorDark9.Location = new Point(768, 20);
            colorDark9.Margin = new Padding(0, 20, 0, 0);
            colorDark9.Name = "colorDark9";
            colorDark9.Size = new Size(96, 90);
            colorDark9.TabIndex = 8;
            colorDark9.Text = "color-9";
            colorDark9.Click += ColorPanel_Click;
            colorDark9.MouseEnter += ColorPanel_MouseEnter;
            colorDark9.MouseLeave += ColorPanel_MouseLeave;
            // 
            // colorDark8
            // 
            colorDark8.Dock = DockStyle.Fill;
            colorDark8.Location = new Point(672, 20);
            colorDark8.Margin = new Padding(0, 20, 0, 0);
            colorDark8.Name = "colorDark8";
            colorDark8.Size = new Size(96, 90);
            colorDark8.TabIndex = 7;
            colorDark8.Text = "color-8";
            colorDark8.Click += ColorPanel_Click;
            colorDark8.MouseEnter += ColorPanel_MouseEnter;
            colorDark8.MouseLeave += ColorPanel_MouseLeave;
            // 
            // colorDark7
            // 
            colorDark7.Dock = DockStyle.Fill;
            colorDark7.Location = new Point(576, 20);
            colorDark7.Margin = new Padding(0, 20, 0, 0);
            colorDark7.Name = "colorDark7";
            colorDark7.Size = new Size(96, 90);
            colorDark7.TabIndex = 6;
            colorDark7.Text = "color-7";
            colorDark7.Click += ColorPanel_Click;
            colorDark7.MouseEnter += ColorPanel_MouseEnter;
            colorDark7.MouseLeave += ColorPanel_MouseLeave;
            // 
            // colorDark6
            // 
            colorDark6.Dock = DockStyle.Fill;
            colorDark6.Location = new Point(480, 20);
            colorDark6.Margin = new Padding(0, 20, 0, 0);
            colorDark6.Name = "colorDark6";
            colorDark6.Size = new Size(96, 90);
            colorDark6.TabIndex = 5;
            colorDark6.Text = "color-6";
            colorDark6.Click += ColorPanel_Click;
            colorDark6.MouseEnter += ColorPanel_MouseEnter;
            colorDark6.MouseLeave += ColorPanel_MouseLeave;
            // 
            // colorDark5
            // 
            colorDark5.Dock = DockStyle.Fill;
            colorDark5.Location = new Point(384, 20);
            colorDark5.Margin = new Padding(0, 20, 0, 0);
            colorDark5.Name = "colorDark5";
            colorDark5.Size = new Size(96, 90);
            colorDark5.TabIndex = 4;
            colorDark5.Text = "color-5";
            colorDark5.Click += ColorPanel_Click;
            colorDark5.MouseEnter += ColorPanel_MouseEnter;
            colorDark5.MouseLeave += ColorPanel_MouseLeave;
            // 
            // colorDark4
            // 
            colorDark4.Dock = DockStyle.Fill;
            colorDark4.Location = new Point(288, 20);
            colorDark4.Margin = new Padding(0, 20, 0, 0);
            colorDark4.Name = "colorDark4";
            colorDark4.Size = new Size(96, 90);
            colorDark4.TabIndex = 3;
            colorDark4.Text = "color-4";
            colorDark4.Click += ColorPanel_Click;
            colorDark4.MouseEnter += ColorPanel_MouseEnter;
            colorDark4.MouseLeave += ColorPanel_MouseLeave;
            // 
            // colorDark3
            // 
            colorDark3.Dock = DockStyle.Fill;
            colorDark3.Location = new Point(192, 20);
            colorDark3.Margin = new Padding(0, 20, 0, 0);
            colorDark3.Name = "colorDark3";
            colorDark3.Size = new Size(96, 90);
            colorDark3.TabIndex = 2;
            colorDark3.Text = "color-3";
            colorDark3.Click += ColorPanel_Click;
            colorDark3.MouseEnter += ColorPanel_MouseEnter;
            colorDark3.MouseLeave += ColorPanel_MouseLeave;
            // 
            // colorDark2
            // 
            colorDark2.Dock = DockStyle.Fill;
            colorDark2.Location = new Point(96, 20);
            colorDark2.Margin = new Padding(0, 20, 0, 0);
            colorDark2.Name = "colorDark2";
            colorDark2.Size = new Size(96, 90);
            colorDark2.TabIndex = 1;
            colorDark2.Text = "color-2";
            colorDark2.Click += ColorPanel_Click;
            colorDark2.MouseEnter += ColorPanel_MouseEnter;
            colorDark2.MouseLeave += ColorPanel_MouseLeave;
            // 
            // colorDark1
            // 
            colorDark1.Dock = DockStyle.Fill;
            colorDark1.Location = new Point(0, 20);
            colorDark1.Margin = new Padding(0, 20, 0, 0);
            colorDark1.Name = "colorDark1";
            colorDark1.Size = new Size(96, 90);
            colorDark1.TabIndex = 0;
            colorDark1.Text = "color-1";
            colorDark1.Click += ColorPanel_Click;
            colorDark1.MouseEnter += ColorPanel_MouseEnter;
            colorDark1.MouseLeave += ColorPanel_MouseLeave;
            // 
            // Colors
            // 
            ClientSize = new Size(964, 461);
            Controls.Add(tableLPanel);
            Font = new Font("Microsoft YaHei UI", 14F);
            Icon = Properties.Resources.icon;
            MinimumSize = new Size(980, 500);
            Name = "Colors";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "色板生成工具";
            panel_primary.ResumeLayout(false);
            tablePanel.ResumeLayout(false);
            panel11.ResumeLayout(false);
            tableLPanel.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            tablePanelDark.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.Panel panel_primary;
        private TableLayoutPanel tablePanel;
        private AntdUI.Input textBox1;
        private AntdUI.Panel panel11;
        private AntdUI.ColorPanel colorLight1;
        private AntdUI.ColorPanel colorLight10;
        private AntdUI.ColorPanel colorLight9;
        private AntdUI.ColorPanel colorLight8;
        private AntdUI.ColorPanel colorLight7;
        private AntdUI.ColorPanel colorLight6;
        private AntdUI.ColorPanel colorLight5;
        private AntdUI.ColorPanel colorLight4;
        private AntdUI.ColorPanel colorLight3;
        private AntdUI.ColorPanel colorLight2;
        private AntdUI.ColorPanelLeft color_primary;
        private AntdUI.ColorPanelLeft color_dark;
        private TableLayoutPanel tableLPanel;
        private AntdUI.Panel panel1;
        private AntdUI.Panel panel2;
        private TableLayoutPanel tablePanelDark;
        private AntdUI.ColorPanel colorDark10;
        private AntdUI.ColorPanel colorDark9;
        private AntdUI.ColorPanel colorDark8;
        private AntdUI.ColorPanel colorDark7;
        private AntdUI.ColorPanel colorDark6;
        private AntdUI.ColorPanel colorDark5;
        private AntdUI.ColorPanel colorDark4;
        private AntdUI.ColorPanel colorDark3;
        private AntdUI.ColorPanel colorDark2;
        private AntdUI.ColorPanel colorDark1;
    }
}