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

namespace Demo.Controls
{
    partial class Tabs
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
            AntdUI.Tabs.StyleLine styleLine1 = new AntdUI.Tabs.StyleLine();
            AntdUI.Tabs.StyleCard styleCard1 = new AntdUI.Tabs.StyleCard();
            AntdUI.Tabs.StyleLine styleLine2 = new AntdUI.Tabs.StyleLine();
            header1 = new AntdUI.PageHeader();
            panel1 = new System.Windows.Forms.Panel();
            tabs3 = new AntdUI.Tabs();
            divider3 = new AntdUI.Divider();
            tabs_close = new AntdUI.Tabs();
            divider2 = new AntdUI.Divider();
            tabs1 = new AntdUI.Tabs();
            tabPage1 = new AntdUI.TabPage();
            label1 = new AntdUI.Label();
            tabPage2 = new AntdUI.TabPage();
            label2 = new AntdUI.Label();
            tabPage3 = new AntdUI.TabPage();
            label3 = new AntdUI.Label();
            tabPage4 = new AntdUI.TabPage();
            label4 = new AntdUI.Label();
            tabPage5 = new AntdUI.TabPage();
            label5 = new AntdUI.Label();
            tabPage6 = new AntdUI.TabPage();
            label6 = new AntdUI.Label();
            tabPage7 = new AntdUI.TabPage();
            label7 = new AntdUI.Label();
            tabPage8 = new AntdUI.TabPage();
            label8 = new AntdUI.Label();
            tabPage9 = new AntdUI.TabPage();
            label9 = new AntdUI.Label();
            tabPage10 = new AntdUI.TabPage();
            label10 = new AntdUI.Label();
            tabPage11 = new AntdUI.TabPage();
            label11 = new AntdUI.Label();
            tabPage12 = new AntdUI.TabPage();
            label12 = new AntdUI.Label();
            tabPage13 = new AntdUI.TabPage();
            label13 = new AntdUI.Label();
            tabPage14 = new AntdUI.TabPage();
            label14 = new AntdUI.Label();
            tabPage15 = new AntdUI.TabPage();
            label15 = new AntdUI.Label();
            divider1 = new AntdUI.Divider();
            tabPage16 = new AntdUI.TabPage();
            tabPage17 = new AntdUI.TabPage();
            tabPage18 = new AntdUI.TabPage();
            panel1.SuspendLayout();
            tabs3.SuspendLayout();
            tabs1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            tabPage3.SuspendLayout();
            tabPage4.SuspendLayout();
            tabPage5.SuspendLayout();
            tabPage6.SuspendLayout();
            tabPage7.SuspendLayout();
            tabPage8.SuspendLayout();
            tabPage9.SuspendLayout();
            tabPage10.SuspendLayout();
            tabPage11.SuspendLayout();
            tabPage12.SuspendLayout();
            tabPage13.SuspendLayout();
            tabPage14.SuspendLayout();
            tabPage15.SuspendLayout();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Description = "选项卡切换组件。";
            header1.Dock = DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F);
            header1.LocalizationDescription = "Tabs.Description";
            header1.LocalizationText = "Tabs.Text";
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new Padding(0, 0, 0, 10);
            header1.Size = new Size(750, 74);
            header1.TabIndex = 0;
            header1.Text = "Tabs 标签页";
            header1.UseTitleFont = true;
            // 
            // panel1
            // 
            panel1.AutoScroll = true;
            panel1.Controls.Add(tabs3);
            panel1.Controls.Add(divider3);
            panel1.Controls.Add(tabs_close);
            panel1.Controls.Add(divider2);
            panel1.Controls.Add(tabs1);
            panel1.Controls.Add(divider1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 74);
            panel1.Name = "panel1";
            panel1.Size = new Size(750, 670);
            panel1.TabIndex = 0;
            panel1.Text = "stackPanel1";
            // 
            // tabs3
            // 
            tabs3.Centered = true;
            tabs3.Controls.Add(tabPage16);
            tabs3.Controls.Add(tabPage17);
            tabs3.Controls.Add(tabPage18);
            tabs3.Cursor = Cursors.Hand;
            tabs3.Dock = DockStyle.Top;
            tabs3.Gap = 12;
            tabs3.Location = new Point(0, 324);
            tabs3.Name = "tabs3";
            tabs3.Pages.Add(tabPage16);
            tabs3.Pages.Add(tabPage17);
            tabs3.Pages.Add(tabPage18);
            tabs3.Size = new Size(750, 120);
            tabs3.Style = styleLine1;
            tabs3.TabIndex = 3;
            tabs3.Text = "tabs3";
            // 
            // divider3
            // 
            divider3.Dock = DockStyle.Top;
            divider3.Font = new Font("Microsoft YaHei UI", 10F);
            divider3.LocalizationText = "Tabs.{id}";
            divider3.Location = new Point(0, 296);
            divider3.Name = "divider3";
            divider3.Orientation = AntdUI.TOrientation.Left;
            divider3.Size = new Size(750, 28);
            divider3.TabIndex = 0;
            divider3.Text = "居中位置";
            // 
            // tabs_close
            // 
            tabs_close.Cursor = Cursors.Hand;
            tabs_close.Dock = DockStyle.Top;
            tabs_close.Gap = 12;
            tabs_close.Location = new Point(0, 176);
            tabs_close.Name = "tabs_close";
            tabs_close.Size = new Size(750, 120);
            styleCard1.Closable = true;
            styleCard1.Gap = 6;
            tabs_close.Style = styleCard1;
            tabs_close.TabIndex = 2;
            tabs_close.Text = "tabs2";
            tabs_close.Type = AntdUI.TabType.Card;
            // 
            // divider2
            // 
            divider2.Dock = DockStyle.Top;
            divider2.Font = new Font("Microsoft YaHei UI", 10F);
            divider2.LocalizationText = "Tabs.{id}";
            divider2.Location = new Point(0, 148);
            divider2.Name = "divider2";
            divider2.Orientation = AntdUI.TOrientation.Left;
            divider2.Size = new Size(750, 28);
            divider2.TabIndex = 0;
            divider2.Text = "卡片样式";
            // 
            // tabs1
            // 
            tabs1.Controls.Add(tabPage1);
            tabs1.Controls.Add(tabPage2);
            tabs1.Controls.Add(tabPage3);
            tabs1.Controls.Add(tabPage4);
            tabs1.Controls.Add(tabPage5);
            tabs1.Controls.Add(tabPage6);
            tabs1.Controls.Add(tabPage7);
            tabs1.Controls.Add(tabPage8);
            tabs1.Controls.Add(tabPage9);
            tabs1.Controls.Add(tabPage10);
            tabs1.Controls.Add(tabPage11);
            tabs1.Controls.Add(tabPage12);
            tabs1.Controls.Add(tabPage13);
            tabs1.Controls.Add(tabPage14);
            tabs1.Controls.Add(tabPage15);
            tabs1.Dock = DockStyle.Top;
            tabs1.Gap = 12;
            tabs1.Location = new Point(0, 28);
            tabs1.Name = "tabs1";
            tabs1.Pages.Add(tabPage1);
            tabs1.Pages.Add(tabPage2);
            tabs1.Pages.Add(tabPage3);
            tabs1.Pages.Add(tabPage4);
            tabs1.Pages.Add(tabPage5);
            tabs1.Pages.Add(tabPage6);
            tabs1.Pages.Add(tabPage7);
            tabs1.Pages.Add(tabPage8);
            tabs1.Pages.Add(tabPage9);
            tabs1.Pages.Add(tabPage10);
            tabs1.Pages.Add(tabPage11);
            tabs1.Pages.Add(tabPage12);
            tabs1.Pages.Add(tabPage13);
            tabs1.Pages.Add(tabPage14);
            tabs1.Pages.Add(tabPage15);
            tabs1.Size = new Size(750, 120);
            tabs1.Style = styleLine2;
            tabs1.TabIndex = 1;
            tabs1.Text = "tabs1";
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(label1);
            tabPage1.IconSvg = "AppleFilled";
            tabPage1.Location = new Point(3, 38);
            tabPage1.Name = "tabPage1";
            tabPage1.Size = new Size(744, 79);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Tab1";
            // 
            // label1
            // 
            label1.Dock = DockStyle.Fill;
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(744, 79);
            label1.TabIndex = 0;
            label1.Text = "Content of Tab Pane 1";
            label1.TextAlign = ContentAlignment.TopLeft;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(label2);
            tabPage2.Location = new Point(0, 0);
            tabPage2.Name = "tabPage2";
            tabPage2.Size = new Size(0, 0);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Tab2";
            // 
            // label2
            // 
            label2.Dock = DockStyle.Fill;
            label2.Location = new Point(0, 0);
            label2.Name = "label2";
            label2.Size = new Size(0, 0);
            label2.TabIndex = 1;
            label2.Text = "Content of Tab Pane 2";
            label2.TextAlign = ContentAlignment.TopLeft;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(label3);
            tabPage3.Location = new Point(0, 0);
            tabPage3.Name = "tabPage3";
            tabPage3.Size = new Size(0, 0);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Tab3";
            // 
            // label3
            // 
            label3.Dock = DockStyle.Fill;
            label3.Location = new Point(0, 0);
            label3.Name = "label3";
            label3.Size = new Size(0, 0);
            label3.TabIndex = 1;
            label3.Text = "Content of Tab Pane 3";
            label3.TextAlign = ContentAlignment.TopLeft;
            // 
            // tabPage4
            // 
            tabPage4.Controls.Add(label4);
            tabPage4.Location = new Point(0, 0);
            tabPage4.Name = "tabPage4";
            tabPage4.Size = new Size(0, 0);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "Tab4";
            // 
            // label4
            // 
            label4.Dock = DockStyle.Fill;
            label4.Location = new Point(0, 0);
            label4.Name = "label4";
            label4.Size = new Size(0, 0);
            label4.TabIndex = 1;
            label4.Text = "Content of Tab Pane 4";
            label4.TextAlign = ContentAlignment.TopLeft;
            // 
            // tabPage5
            // 
            tabPage5.Controls.Add(label5);
            tabPage5.Location = new Point(0, 0);
            tabPage5.Name = "tabPage5";
            tabPage5.Size = new Size(0, 0);
            tabPage5.TabIndex = 4;
            tabPage5.Text = "Tab5";
            // 
            // label5
            // 
            label5.Dock = DockStyle.Fill;
            label5.Location = new Point(0, 0);
            label5.Name = "label5";
            label5.Size = new Size(0, 0);
            label5.TabIndex = 1;
            label5.Text = "Content of Tab Pane 5";
            label5.TextAlign = ContentAlignment.TopLeft;
            // 
            // tabPage6
            // 
            tabPage6.Controls.Add(label6);
            tabPage6.Location = new Point(0, 0);
            tabPage6.Name = "tabPage6";
            tabPage6.Size = new Size(0, 0);
            tabPage6.TabIndex = 5;
            tabPage6.Text = "Tab6";
            // 
            // label6
            // 
            label6.Dock = DockStyle.Fill;
            label6.Location = new Point(0, 0);
            label6.Name = "label6";
            label6.Size = new Size(0, 0);
            label6.TabIndex = 1;
            label6.Text = "Content of Tab Pane 6";
            label6.TextAlign = ContentAlignment.TopLeft;
            // 
            // tabPage7
            // 
            tabPage7.Controls.Add(label7);
            tabPage7.Location = new Point(0, 0);
            tabPage7.Name = "tabPage7";
            tabPage7.Size = new Size(0, 0);
            tabPage7.TabIndex = 6;
            tabPage7.Text = "Tab7";
            // 
            // label7
            // 
            label7.Dock = DockStyle.Fill;
            label7.Location = new Point(0, 0);
            label7.Name = "label7";
            label7.Size = new Size(0, 0);
            label7.TabIndex = 1;
            label7.Text = "Content of Tab Pane 7";
            label7.TextAlign = ContentAlignment.TopLeft;
            // 
            // tabPage8
            // 
            tabPage8.Controls.Add(label8);
            tabPage8.Location = new Point(0, 0);
            tabPage8.Name = "tabPage8";
            tabPage8.Size = new Size(0, 0);
            tabPage8.TabIndex = 7;
            tabPage8.Text = "Tab8";
            // 
            // label8
            // 
            label8.Dock = DockStyle.Fill;
            label8.Location = new Point(0, 0);
            label8.Name = "label8";
            label8.Size = new Size(0, 0);
            label8.TabIndex = 1;
            label8.Text = "Content of Tab Pane 8";
            label8.TextAlign = ContentAlignment.TopLeft;
            // 
            // tabPage9
            // 
            tabPage9.Controls.Add(label9);
            tabPage9.Location = new Point(0, 0);
            tabPage9.Name = "tabPage9";
            tabPage9.Size = new Size(0, 0);
            tabPage9.TabIndex = 8;
            tabPage9.Text = "Tab9";
            // 
            // label9
            // 
            label9.Dock = DockStyle.Fill;
            label9.Location = new Point(0, 0);
            label9.Name = "label9";
            label9.Size = new Size(0, 0);
            label9.TabIndex = 1;
            label9.Text = "Content of Tab Pane 9";
            label9.TextAlign = ContentAlignment.TopLeft;
            // 
            // tabPage10
            // 
            tabPage10.Controls.Add(label10);
            tabPage10.Location = new Point(0, 0);
            tabPage10.Name = "tabPage10";
            tabPage10.Size = new Size(0, 0);
            tabPage10.TabIndex = 9;
            tabPage10.Text = "Tab10";
            // 
            // label10
            // 
            label10.Dock = DockStyle.Fill;
            label10.Location = new Point(0, 0);
            label10.Name = "label10";
            label10.Size = new Size(0, 0);
            label10.TabIndex = 1;
            label10.Text = "Content of Tab Pane 10";
            label10.TextAlign = ContentAlignment.TopLeft;
            // 
            // tabPage11
            // 
            tabPage11.Controls.Add(label11);
            tabPage11.Location = new Point(0, 0);
            tabPage11.Name = "tabPage11";
            tabPage11.Size = new Size(0, 0);
            tabPage11.TabIndex = 10;
            tabPage11.Text = "Tab11";
            // 
            // label11
            // 
            label11.Dock = DockStyle.Fill;
            label11.Location = new Point(0, 0);
            label11.Name = "label11";
            label11.Size = new Size(0, 0);
            label11.TabIndex = 1;
            label11.Text = "Content of Tab Pane 11";
            label11.TextAlign = ContentAlignment.TopLeft;
            // 
            // tabPage12
            // 
            tabPage12.Controls.Add(label12);
            tabPage12.Location = new Point(0, 0);
            tabPage12.Name = "tabPage12";
            tabPage12.Size = new Size(0, 0);
            tabPage12.TabIndex = 11;
            tabPage12.Text = "Tab12";
            // 
            // label12
            // 
            label12.Dock = DockStyle.Fill;
            label12.Location = new Point(0, 0);
            label12.Name = "label12";
            label12.Size = new Size(0, 0);
            label12.TabIndex = 1;
            label12.Text = "Content of Tab Pane 12";
            label12.TextAlign = ContentAlignment.TopLeft;
            // 
            // tabPage13
            // 
            tabPage13.Controls.Add(label13);
            tabPage13.Location = new Point(0, 0);
            tabPage13.Name = "tabPage13";
            tabPage13.Size = new Size(0, 0);
            tabPage13.TabIndex = 12;
            tabPage13.Text = "Tab13";
            // 
            // label13
            // 
            label13.Dock = DockStyle.Fill;
            label13.Location = new Point(0, 0);
            label13.Name = "label13";
            label13.Size = new Size(0, 0);
            label13.TabIndex = 1;
            label13.Text = "Content of Tab Pane 13";
            label13.TextAlign = ContentAlignment.TopLeft;
            // 
            // tabPage14
            // 
            tabPage14.Controls.Add(label14);
            tabPage14.Location = new Point(0, 0);
            tabPage14.Name = "tabPage14";
            tabPage14.Size = new Size(0, 0);
            tabPage14.TabIndex = 13;
            tabPage14.Text = "Tab14";
            // 
            // label14
            // 
            label14.Dock = DockStyle.Fill;
            label14.Location = new Point(0, 0);
            label14.Name = "label14";
            label14.Size = new Size(0, 0);
            label14.TabIndex = 1;
            label14.Text = "Content of Tab Pane 14";
            label14.TextAlign = ContentAlignment.TopLeft;
            // 
            // tabPage15
            // 
            tabPage15.Controls.Add(label15);
            tabPage15.Location = new Point(0, 0);
            tabPage15.Name = "tabPage15";
            tabPage15.Size = new Size(0, 0);
            tabPage15.TabIndex = 14;
            tabPage15.Text = "Tab15";
            // 
            // label15
            // 
            label15.Dock = DockStyle.Fill;
            label15.Location = new Point(0, 0);
            label15.Name = "label15";
            label15.Size = new Size(0, 0);
            label15.TabIndex = 1;
            label15.Text = "Content of Tab Pane 15";
            label15.TextAlign = ContentAlignment.TopLeft;
            // 
            // divider1
            // 
            divider1.Dock = DockStyle.Top;
            divider1.Font = new Font("Microsoft YaHei UI", 10F);
            divider1.LocalizationText = "Tabs.{id}";
            divider1.Location = new Point(0, 0);
            divider1.Name = "divider1";
            divider1.Orientation = AntdUI.TOrientation.Left;
            divider1.Size = new Size(750, 28);
            divider1.TabIndex = 0;
            divider1.Text = "基本用法";
            // 
            // tabPage16
            // 
            tabPage16.Badge = "99";
            tabPage16.IconSvg = "AppleFilled";
            tabPage16.Location = new Point(3, 38);
            tabPage16.Name = "tabPage16";
            tabPage16.Size = new Size(744, 79);
            tabPage16.TabIndex = 0;
            tabPage16.Text = "Tab1";
            // 
            // tabPage17
            // 
            tabPage17.Location = new Point(0, 0);
            tabPage17.Name = "tabPage17";
            tabPage17.Size = new Size(0, 0);
            tabPage17.TabIndex = 1;
            tabPage17.Text = "Tab2";
            // 
            // tabPage18
            // 
            tabPage18.Location = new Point(0, 0);
            tabPage18.Name = "tabPage18";
            tabPage18.Size = new Size(0, 0);
            tabPage18.TabIndex = 2;
            tabPage18.Text = "Tab3";
            // 
            // Tabs
            // 
            Controls.Add(panel1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 12F);
            Name = "Tabs";
            Size = new Size(750, 744);
            panel1.ResumeLayout(false);
            tabs3.ResumeLayout(false);
            tabs1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            tabPage3.ResumeLayout(false);
            tabPage4.ResumeLayout(false);
            tabPage5.ResumeLayout(false);
            tabPage6.ResumeLayout(false);
            tabPage7.ResumeLayout(false);
            tabPage8.ResumeLayout(false);
            tabPage9.ResumeLayout(false);
            tabPage10.ResumeLayout(false);
            tabPage11.ResumeLayout(false);
            tabPage12.ResumeLayout(false);
            tabPage13.ResumeLayout(false);
            tabPage14.ResumeLayout(false);
            tabPage15.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private AntdUI.PageHeader header1;
        private System.Windows.Forms.Panel panel1;
        private AntdUI.Divider divider1;
        private AntdUI.Tabs tabs1;
        private AntdUI.Divider divider2;
        private AntdUI.Tabs tabs_close;
        private AntdUI.Tabs tabs3;
        private AntdUI.Divider divider3;
        private AntdUI.Label label1;
        private AntdUI.TabPage tabPage1;
        private AntdUI.TabPage tabPage2;
        private AntdUI.TabPage tabPage3;
        private AntdUI.TabPage tabPage4;
        private AntdUI.TabPage tabPage5;
        private AntdUI.TabPage tabPage6;
        private AntdUI.TabPage tabPage7;
        private AntdUI.TabPage tabPage8;
        private AntdUI.TabPage tabPage9;
        private AntdUI.TabPage tabPage10;
        private AntdUI.TabPage tabPage11;
        private AntdUI.TabPage tabPage12;
        private AntdUI.TabPage tabPage13;
        private AntdUI.TabPage tabPage14;
        private AntdUI.TabPage tabPage15;
        private AntdUI.Label label2;
        private AntdUI.Label label3;
        private AntdUI.Label label4;
        private AntdUI.Label label5;
        private AntdUI.Label label6;
        private AntdUI.Label label7;
        private AntdUI.Label label8;
        private AntdUI.Label label9;
        private AntdUI.Label label10;
        private AntdUI.Label label11;
        private AntdUI.Label label12;
        private AntdUI.Label label13;
        private AntdUI.Label label14;
        private AntdUI.Label label15;
        private AntdUI.TabPage tabPage16;
        private AntdUI.TabPage tabPage17;
        private AntdUI.TabPage tabPage18;
    }
}