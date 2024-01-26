// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
// THE AntdUI PROJECT IS AN WINFORM LIBRARY LICENSED UNDER THE GPL-3.0 License.
// LICENSED UNDER THE GPL License, VERSION 3.0 (THE "License")
// YOU MAY NOT USE THIS FILE EXCEPT IN COMPLIANCE WITH THE License.
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
            AntdUI.CarouselItem carouselItem1 = new AntdUI.CarouselItem();
            AntdUI.CarouselItem carouselItem2 = new AntdUI.CarouselItem();
            AntdUI.CarouselItem carouselItem3 = new AntdUI.CarouselItem();
            AntdUI.CarouselItem carouselItem4 = new AntdUI.CarouselItem();
            header1 = new AntdUI.Header();
            tabs1 = new AntdUI.Tabs();
            tabPage1 = new TabPage();
            carousel2 = new AntdUI.Carousel();
            label1 = new Label();
            tabPage2 = new TabPage();
            label2 = new Label();
            tabPage3 = new TabPage();
            label3 = new Label();
            tabPage4 = new TabPage();
            label4 = new Label();
            tabs1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            tabPage3.SuspendLayout();
            tabPage4.SuspendLayout();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Dock = DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new Padding(6);
            header1.Size = new Size(688, 79);
            header1.TabIndex = 4;
            header1.Text = "Tabs 标签页";
            header1.TextDesc = "选项卡切换组件。";
            // 
            // tabs1
            // 
            tabs1.Appearance = TabAppearance.FlatButtons;
            tabs1.Controls.Add(tabPage1);
            tabs1.Controls.Add(tabPage2);
            tabs1.Controls.Add(tabPage3);
            tabs1.Controls.Add(tabPage4);
            tabs1.Dock = DockStyle.Fill;
            tabs1.Font = new Font("Microsoft YaHei UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            tabs1.ItemSize = new Size(80, 54);
            tabs1.Location = new Point(0, 79);
            tabs1.Name = "tabs1";
            tabs1.SelectedIndex = 0;
            tabs1.Size = new Size(688, 339);
            tabs1.TabIndex = 5;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(carousel2);
            tabPage1.Controls.Add(label1);
            tabPage1.Location = new Point(4, 58);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(680, 277);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Tab 1";
            tabPage1.UseVisualStyleBackColor = true;
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
            tabPage2.Location = new Point(4, 58);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(680, 277);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Tab 2";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Top;
            label2.Location = new Point(3, 3);
            label2.Name = "label2";
            label2.Size = new Size(49, 20);
            label2.TabIndex = 1;
            label2.Text = "Tab 2";
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(label3);
            tabPage3.Location = new Point(4, 58);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(680, 277);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "TabPage3";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = DockStyle.Top;
            label3.Location = new Point(3, 3);
            label3.Name = "label3";
            label3.Size = new Size(49, 20);
            label3.TabIndex = 1;
            label3.Text = "Tab 3";
            // 
            // tabPage4
            // 
            tabPage4.Controls.Add(label4);
            tabPage4.Location = new Point(4, 58);
            tabPage4.Name = "tabPage4";
            tabPage4.Padding = new Padding(3);
            tabPage4.Size = new Size(680, 277);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "TabPage4 233";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Dock = DockStyle.Top;
            label4.Location = new Point(3, 3);
            label4.Name = "label4";
            label4.Size = new Size(49, 20);
            label4.TabIndex = 1;
            label4.Text = "Tab 4";
            // 
            // Tabs
            // 
            Controls.Add(tabs1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 16F, FontStyle.Regular, GraphicsUnit.Point);
            Name = "Tabs";
            Size = new Size(688, 418);
            tabs1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            tabPage3.ResumeLayout(false);
            tabPage3.PerformLayout();
            tabPage4.ResumeLayout(false);
            tabPage4.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.Header header1;
        private AntdUI.Tabs tabs1;
        private TabPage tabPage1;
        private AntdUI.Carousel carousel2;
        private Label label1;
        private TabPage tabPage2;
        private Label label2;
        private TabPage tabPage3;
        private Label label3;
        private TabPage tabPage4;
        private Label label4;
    }
}