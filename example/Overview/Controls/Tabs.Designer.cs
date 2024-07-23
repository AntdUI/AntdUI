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
// GITEE: https://gitee.com/antdui/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

namespace Overview.Controls
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

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            AntdUI.Tabs.StyleLine styleLine1 = new AntdUI.Tabs.StyleLine();
            AntdUI.CarouselItem carouselItem1 = new AntdUI.CarouselItem();
            AntdUI.CarouselItem carouselItem2 = new AntdUI.CarouselItem();
            AntdUI.CarouselItem carouselItem3 = new AntdUI.CarouselItem();
            AntdUI.CarouselItem carouselItem4 = new AntdUI.CarouselItem();
            AntdUI.Tabs.StyleCard styleCard1 = new AntdUI.Tabs.StyleCard();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Tabs));
            header1 = new AntdUI.Header();
            tabs1 = new AntdUI.Tabs();
            tabPage1 = new AntdUI.TabPage();
            carousel2 = new AntdUI.Carousel();
            label1 = new Label();
            tabPage2 = new AntdUI.TabPage();
            label2 = new Label();
            tabPage3 = new AntdUI.TabPage();
            label3 = new Label();
            tabPage4 = new AntdUI.TabPage();
            label4 = new Label();
            tabs2 = new AntdUI.Tabs();
            tabPage5 = new AntdUI.TabPage();
            label5 = new Label();
            tabPage6 = new AntdUI.TabPage();
            label6 = new Label();
            tabPage7 = new AntdUI.TabPage();
            label7 = new Label();
            tabs1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            tabPage3.SuspendLayout();
            tabPage4.SuspendLayout();
            tabs2.SuspendLayout();
            tabPage5.SuspendLayout();
            tabPage6.SuspendLayout();
            tabPage7.SuspendLayout();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Dock = DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new Padding(6);
            header1.Size = new Size(663, 79);
            header1.TabIndex = 4;
            header1.Text = "Tabs 标签页";
            header1.TextDesc = "选项卡切换组件。";
            // 
            // tabs1
            // 
            tabs1.Dock = DockStyle.Top;
            tabs1.Location = new Point(0, 79);
            tabs1.Name = "tabs1";
            tabs1.Padding = new Padding(4);
            tabs1.Pages.Add(tabPage1);
            tabs1.Pages.Add(tabPage2);
            tabs1.Pages.Add(tabPage3);
            tabs1.Pages.Add(tabPage4);
            tabs1.Size = new Size(663, 314);
            tabs1.Style = styleLine1;
            tabs1.TabIndex = 5;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(carousel2);
            tabPage1.Controls.Add(label1);
            tabPage1.Dock = DockStyle.Fill;
            tabPage1.Location = new Point(7, 32);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(649, 275);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Tab 1";
            // 
            // carousel2
            // 
            carousel2.DotPosition = AntdUI.TAlignMini.Top;
            carouselItem1.Img = Properties.Resources.img1;
            carouselItem2.Img = Properties.Resources.bg1;
            carouselItem3.Img = Properties.Resources.bg7;
            carouselItem4.Img = Properties.Resources.bg2;
            carousel2.Image.AddRange(new AntdUI.CarouselItem[] { carouselItem1, carouselItem2, carouselItem3, carouselItem4 });
            carousel2.Location = new Point(22, 43);
            carousel2.Name = "carousel2";
            carousel2.Radius = 8;
            carousel2.Size = new Size(393, 211);
            carousel2.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Top;
            label1.Location = new Point(3, 3);
            label1.Name = "label1";
            label1.Size = new Size(49, 20);
            label1.TabIndex = 0;
            label1.Text = "Tab 1";
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(label2);
            tabPage2.Dock = DockStyle.Fill;
            tabPage2.Location = new Point(4, 29);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(192, 67);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Tab 2";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Top;
            label2.Location = new Point(3, 3);
            label2.Name = "label2";
            label2.Size = new Size(41, 17);
            label2.TabIndex = 1;
            label2.Text = "Tab 2";
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(label3);
            tabPage3.Dock = DockStyle.Fill;
            tabPage3.Location = new Point(4, 29);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(192, 67);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "TabPage3";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = DockStyle.Top;
            label3.Location = new Point(3, 3);
            label3.Name = "label3";
            label3.Size = new Size(41, 17);
            label3.TabIndex = 1;
            label3.Text = "Tab 3";
            // 
            // tabPage4
            // 
            tabPage4.Controls.Add(label4);
            tabPage4.Dock = DockStyle.Fill;
            tabPage4.Location = new Point(4, 29);
            tabPage4.Name = "tabPage4";
            tabPage4.Padding = new Padding(3);
            tabPage4.Size = new Size(192, 67);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "TabPage4 233";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Dock = DockStyle.Top;
            label4.Location = new Point(3, 3);
            label4.Name = "label4";
            label4.Size = new Size(41, 17);
            label4.TabIndex = 1;
            label4.Text = "Tab 4";
            // 
            // tabs2
            // 
            tabs2.Alignment = TabAlignment.Left;
            tabs2.Dock = DockStyle.Fill;
            tabs2.Location = new Point(0, 393);
            tabs2.Margin = new Padding(8);
            tabs2.Name = "tabs2";
            tabs2.Pages.Add(tabPage5);
            tabs2.Pages.Add(tabPage6);
            tabs2.Pages.Add(tabPage7);
            tabs2.Size = new Size(663, 162);
            styleCard1.Gap = 8;
            tabs2.Style = styleCard1;
            tabs2.TabIndex = 7;
            tabs2.Type = AntdUI.TabType.Card;
            // 
            // tabPage5
            // 
            tabPage5.Controls.Add(label5);
            tabPage5.Dock = DockStyle.Fill;
            tabPage5.Location = new Point(72, 8);
            tabPage5.Name = "tabPage5";
            tabPage5.Padding = new Padding(3);
            tabPage5.Size = new Size(583, 146);
            tabPage5.TabIndex = 6;
            tabPage5.Text = "Tab 1";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Dock = DockStyle.Top;
            label5.Location = new Point(3, 3);
            label5.Name = "label5";
            label5.Size = new Size(49, 20);
            label5.TabIndex = 0;
            label5.Text = "Tab 1";
            // 
            // tabPage6
            // 
            tabPage6.Controls.Add(label6);
            tabPage6.Dock = DockStyle.Fill;
            tabPage6.IconSvg = resources.GetString("tabPage6.IconSvg");
            tabPage6.Location = new Point(85, 3);
            tabPage6.Name = "tabPage6";
            tabPage6.Padding = new Padding(3);
            tabPage6.Size = new Size(575, 156);
            tabPage6.TabIndex = 0;
            tabPage6.Text = "Tab 2";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Dock = DockStyle.Top;
            label6.Location = new Point(3, 3);
            label6.Name = "label6";
            label6.Size = new Size(41, 17);
            label6.TabIndex = 1;
            label6.Text = "Tab 2";
            // 
            // tabPage7
            // 
            tabPage7.Controls.Add(label7);
            tabPage7.Dock = DockStyle.Fill;
            tabPage7.IconSvg = resources.GetString("tabPage7.IconSvg");
            tabPage7.Location = new Point(85, 3);
            tabPage7.Name = "tabPage7";
            tabPage7.Padding = new Padding(3);
            tabPage7.Size = new Size(575, 156);
            tabPage7.TabIndex = 0;
            tabPage7.Text = "Tab 3";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Dock = DockStyle.Top;
            label7.Location = new Point(3, 3);
            label7.Name = "label7";
            label7.Size = new Size(41, 17);
            label7.TabIndex = 1;
            label7.Text = "Tab 3";
            // 
            // Tabs
            // 
            Controls.Add(tabs2);
            Controls.Add(tabs1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            Name = "Tabs";
            Size = new Size(663, 555);
            tabs1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            tabPage3.ResumeLayout(false);
            tabPage3.PerformLayout();
            tabPage4.ResumeLayout(false);
            tabPage4.PerformLayout();
            tabs2.ResumeLayout(false);
            tabPage5.ResumeLayout(false);
            tabPage5.PerformLayout();
            tabPage6.ResumeLayout(false);
            tabPage6.PerformLayout();
            tabPage7.ResumeLayout(false);
            tabPage7.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.Header header1;
        private AntdUI.Tabs tabs1;
        private AntdUI.TabPage tabPage1;
        private AntdUI.Carousel carousel2;
        private Label label1;
        private AntdUI.TabPage tabPage2;
        private Label label2;
        private AntdUI.TabPage tabPage3;
        private Label label3;
        private AntdUI.TabPage tabPage4;
        private Label label4;
        private AntdUI.Tabs tabs2;
        private AntdUI.TabPage tabPage5;
        private Label label5;
        private AntdUI.TabPage tabPage6;
        private AntdUI.TabPage tabPage7;
        private Label label6;
        private Label label7;
    }
}