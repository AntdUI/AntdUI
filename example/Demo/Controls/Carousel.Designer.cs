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

using System.Drawing;
using System.Windows.Forms;

namespace Demo.Controls
{
    partial class Carousel
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
            AntdUI.CarouselItem carouselItem5 = new AntdUI.CarouselItem();
            AntdUI.CarouselItem carouselItem6 = new AntdUI.CarouselItem();
            AntdUI.CarouselItem carouselItem7 = new AntdUI.CarouselItem();
            AntdUI.CarouselItem carouselItem8 = new AntdUI.CarouselItem();
            AntdUI.CarouselItem carouselItem9 = new AntdUI.CarouselItem();
            AntdUI.CarouselItem carouselItem10 = new AntdUI.CarouselItem();
            AntdUI.CarouselItem carouselItem11 = new AntdUI.CarouselItem();
            AntdUI.CarouselItem carouselItem12 = new AntdUI.CarouselItem();
            AntdUI.CarouselItem carouselItem13 = new AntdUI.CarouselItem();
            AntdUI.CarouselItem carouselItem14 = new AntdUI.CarouselItem();
            AntdUI.CarouselItem carouselItem15 = new AntdUI.CarouselItem();
            header1 = new AntdUI.PageHeader();
            flowLayoutPanel1 = new FlowLayoutPanel();
            carousel2 = new AntdUI.Carousel();
            carousel3 = new AntdUI.Carousel();
            panel2 = new System.Windows.Forms.Panel();
            carousel1 = new AntdUI.Carousel();
            slider1 = new AntdUI.Slider();
            image3d1 = new AntdUI.Image3D();
            image3d2 = new AntdUI.Image3D();
            flowLayoutPanel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Description = "旋转木马，一组轮播的区域。";
            header1.Dock = DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F);
            header1.LocalizationDescription = "Carousel.Description";
            header1.LocalizationText = "Carousel.Text";
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new Padding(0, 0, 0, 10);
            header1.Size = new Size(1300, 74);
            header1.TabIndex = 0;
            header1.Text = "Carousel 走马灯";
            header1.UseTitleFont = true;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.Controls.Add(carousel2);
            flowLayoutPanel1.Controls.Add(carousel3);
            flowLayoutPanel1.Controls.Add(panel2);
            flowLayoutPanel1.Controls.Add(image3d1);
            flowLayoutPanel1.Controls.Add(image3d2);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(0, 74);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(1300, 602);
            flowLayoutPanel1.TabIndex = 5;
            // 
            // carousel2
            // 
            carousel2.DotPosition = AntdUI.TAlignMini.Left;
            carouselItem1.Img = Properties.Resources.img1;
            carouselItem2.Img = Properties.Resources.bg1;
            carouselItem3.Img = Properties.Resources.bg7;
            carouselItem4.Img = Properties.Resources.bg2;
            carousel2.Image.Add(carouselItem1);
            carousel2.Image.Add(carouselItem2);
            carousel2.Image.Add(carouselItem3);
            carousel2.Image.Add(carouselItem4);
            carousel2.Location = new Point(3, 3);
            carousel2.Name = "carousel2";
            carousel2.Radius = 8;
            carousel2.Size = new Size(393, 211);
            carousel2.TabIndex = 3;
            // 
            // carousel3
            // 
            carousel3.DotPosition = AntdUI.TAlignMini.Top;
            carouselItem5.Img = Properties.Resources.img1;
            carouselItem6.Img = Properties.Resources.bg1;
            carouselItem7.Img = Properties.Resources.bg7;
            carouselItem8.Img = Properties.Resources.bg2;
            carouselItem9.Img = Properties.Resources.bg3;
            carouselItem10.Img = Properties.Resources.bg4;
            carouselItem11.Img = Properties.Resources.bg5;
            carouselItem12.Img = Properties.Resources.bg6;
            carousel3.Image.Add(carouselItem5);
            carousel3.Image.Add(carouselItem6);
            carousel3.Image.Add(carouselItem7);
            carousel3.Image.Add(carouselItem8);
            carousel3.Image.Add(carouselItem9);
            carousel3.Image.Add(carouselItem10);
            carousel3.Image.Add(carouselItem11);
            carousel3.Image.Add(carouselItem12);
            carousel3.Location = new Point(402, 3);
            carousel3.Name = "carousel3";
            carousel3.Size = new Size(393, 211);
            carousel3.TabIndex = 6;
            // 
            // panel2
            // 
            panel2.Controls.Add(carousel1);
            panel2.Controls.Add(slider1);
            panel2.Location = new Point(801, 3);
            panel2.Name = "panel2";
            panel2.Size = new Size(225, 275);
            panel2.TabIndex = 5;
            // 
            // carousel1
            // 
            carousel1.Autoplay = true;
            carousel1.BackColor = Color.Transparent;
            carousel1.Dock = DockStyle.Fill;
            carousel1.DotPosition = AntdUI.TAlignMini.Bottom;
            carouselItem13.Img = Properties.Resources.bg1;
            carouselItem14.Img = Properties.Resources.bg7;
            carouselItem15.Img = Properties.Resources.bg2;
            carousel1.Image.Add(carouselItem13);
            carousel1.Image.Add(carouselItem14);
            carousel1.Image.Add(carouselItem15);
            carousel1.Location = new Point(0, 0);
            carousel1.Name = "carousel1";
            carousel1.Radius = 60;
            carousel1.SelectIndex = 2;
            carousel1.Size = new Size(225, 215);
            carousel1.TabIndex = 4;
            // 
            // slider1
            // 
            slider1.Dock = DockStyle.Bottom;
            slider1.Location = new Point(0, 215);
            slider1.Name = "slider1";
            slider1.Size = new Size(225, 60);
            slider1.TabIndex = 3;
            slider1.Text = "slider1";
            // 
            // image3d1
            // 
            image3d1.Image = Properties.Resources.bg7;
            image3d1.Location = new Point(3, 284);
            image3d1.Name = "image3d1";
            image3d1.Padding = new Padding(20);
            image3d1.Radius = 10;
            image3d1.Shadow = 20;
            image3d1.Size = new Size(300, 180);
            image3d1.TabIndex = 14;
            image3d1.Click += image3d1_Click;
            // 
            // image3d2
            // 
            image3d2.Duration = 200;
            image3d2.Image = Properties.Resources.bg1;
            image3d2.Location = new Point(309, 284);
            image3d2.Name = "image3d2";
            image3d2.Padding = new Padding(20);
            image3d2.Radius = 10;
            image3d2.Size = new Size(300, 180);
            image3d2.TabIndex = 13;
            image3d2.Vertical = true;
            image3d2.Click += image3d2_Click;
            // 
            // Carousel
            // 
            Controls.Add(flowLayoutPanel1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 16F);
            Name = "Carousel";
            Size = new Size(1300, 676);
            flowLayoutPanel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.PageHeader header1;
        private FlowLayoutPanel flowLayoutPanel1;
        private AntdUI.Carousel carousel2;
        private AntdUI.Carousel carousel1;
        private AntdUI.Slider slider1;
        private System.Windows.Forms.Panel panel2;
        private AntdUI.Carousel carousel3;
        private AntdUI.Image3D image3d2;
        private AntdUI.Image3D image3d1;
    }
}