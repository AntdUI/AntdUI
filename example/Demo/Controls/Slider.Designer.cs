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
    partial class Slider
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
            AntdUI.SliderMarkItem sliderMarkItem1 = new AntdUI.SliderMarkItem();
            AntdUI.SliderMarkItem sliderMarkItem2 = new AntdUI.SliderMarkItem();
            AntdUI.SliderMarkItem sliderMarkItem3 = new AntdUI.SliderMarkItem();
            AntdUI.SliderMarkItem sliderMarkItem4 = new AntdUI.SliderMarkItem();
            AntdUI.SliderMarkItem sliderMarkItem5 = new AntdUI.SliderMarkItem();
            AntdUI.SliderMarkItem sliderMarkItem6 = new AntdUI.SliderMarkItem();
            AntdUI.SliderMarkItem sliderMarkItem7 = new AntdUI.SliderMarkItem();
            AntdUI.SliderMarkItem sliderMarkItem8 = new AntdUI.SliderMarkItem();
            AntdUI.SliderMarkItem sliderMarkItem9 = new AntdUI.SliderMarkItem();
            AntdUI.SliderMarkItem sliderMarkItem10 = new AntdUI.SliderMarkItem();
            AntdUI.SliderMarkItem sliderMarkItem11 = new AntdUI.SliderMarkItem();
            AntdUI.SliderMarkItem sliderMarkItem12 = new AntdUI.SliderMarkItem();
            AntdUI.SliderMarkItem sliderMarkItem13 = new AntdUI.SliderMarkItem();
            AntdUI.SliderMarkItem sliderMarkItem14 = new AntdUI.SliderMarkItem();
            AntdUI.SliderMarkItem sliderMarkItem15 = new AntdUI.SliderMarkItem();
            AntdUI.SliderMarkItem sliderMarkItem16 = new AntdUI.SliderMarkItem();
            AntdUI.SliderMarkItem sliderMarkItem17 = new AntdUI.SliderMarkItem();
            AntdUI.SliderMarkItem sliderMarkItem18 = new AntdUI.SliderMarkItem();
            AntdUI.SliderMarkItem sliderMarkItem19 = new AntdUI.SliderMarkItem();
            AntdUI.SliderMarkItem sliderMarkItem20 = new AntdUI.SliderMarkItem();
            AntdUI.SliderMarkItem sliderMarkItem21 = new AntdUI.SliderMarkItem();
            AntdUI.SliderMarkItem sliderMarkItem22 = new AntdUI.SliderMarkItem();
            AntdUI.SliderMarkItem sliderMarkItem23 = new AntdUI.SliderMarkItem();
            AntdUI.SliderMarkItem sliderMarkItem24 = new AntdUI.SliderMarkItem();
            AntdUI.SliderMarkItem sliderMarkItem25 = new AntdUI.SliderMarkItem();
            AntdUI.SliderMarkItem sliderMarkItem26 = new AntdUI.SliderMarkItem();
            AntdUI.SliderMarkItem sliderMarkItem27 = new AntdUI.SliderMarkItem();
            AntdUI.SliderMarkItem sliderMarkItem28 = new AntdUI.SliderMarkItem();
            AntdUI.SliderMarkItem sliderMarkItem29 = new AntdUI.SliderMarkItem();
            AntdUI.SliderMarkItem sliderMarkItem30 = new AntdUI.SliderMarkItem();
            AntdUI.SliderMarkItem sliderMarkItem31 = new AntdUI.SliderMarkItem();
            AntdUI.SliderMarkItem sliderMarkItem32 = new AntdUI.SliderMarkItem();
            AntdUI.SliderMarkItem sliderMarkItem33 = new AntdUI.SliderMarkItem();
            AntdUI.SliderMarkItem sliderMarkItem34 = new AntdUI.SliderMarkItem();
            AntdUI.SliderMarkItem sliderMarkItem35 = new AntdUI.SliderMarkItem();
            AntdUI.SliderMarkItem sliderMarkItem36 = new AntdUI.SliderMarkItem();
            AntdUI.SliderMarkItem sliderMarkItem37 = new AntdUI.SliderMarkItem();
            AntdUI.SliderMarkItem sliderMarkItem38 = new AntdUI.SliderMarkItem();
            AntdUI.SliderMarkItem sliderMarkItem39 = new AntdUI.SliderMarkItem();
            AntdUI.SliderMarkItem sliderMarkItem40 = new AntdUI.SliderMarkItem();
            AntdUI.SliderMarkItem sliderMarkItem41 = new AntdUI.SliderMarkItem();
            AntdUI.SliderMarkItem sliderMarkItem42 = new AntdUI.SliderMarkItem();
            AntdUI.SliderMarkItem sliderMarkItem43 = new AntdUI.SliderMarkItem();
            AntdUI.SliderMarkItem sliderMarkItem44 = new AntdUI.SliderMarkItem();
            header1 = new AntdUI.PageHeader();
            panel1 = new System.Windows.Forms.Panel();
            panel3 = new System.Windows.Forms.Panel();
            slider9 = new AntdUI.Slider();
            slider8 = new AntdUI.Slider();
            slider6 = new AntdUI.Slider();
            slider5 = new AntdUI.Slider();
            slider4 = new AntdUI.Slider();
            slider7 = new AntdUI.Slider();
            divider2 = new AntdUI.Divider();
            panel2 = new System.Windows.Forms.Panel();
            slider2 = new AntdUI.Slider();
            slider3 = new AntdUI.Slider();
            slider1 = new AntdUI.Slider();
            divider1 = new AntdUI.Divider();
            panel1.SuspendLayout();
            panel3.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Description = "滑动型输入器，展示当前值和可选范围。";
            header1.Dock = DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F);
            header1.LocalizationDescription = "Slider.Description";
            header1.LocalizationText = "Slider.Text";
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new Padding(0, 0, 0, 10);
            header1.Size = new Size(1179, 74);
            header1.TabIndex = 0;
            header1.Text = "Slider 滑动输入条";
            header1.UseTitleFont = true;
            // 
            // panel1
            // 
            panel1.AutoScroll = true;
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(divider2);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(divider1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 74);
            panel1.Name = "panel1";
            panel1.Size = new Size(1179, 602);
            panel1.TabIndex = 5;
            // 
            // panel3
            // 
            panel3.Controls.Add(slider9);
            panel3.Controls.Add(slider8);
            panel3.Controls.Add(slider6);
            panel3.Controls.Add(slider5);
            panel3.Controls.Add(slider4);
            panel3.Controls.Add(slider7);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(0, 110);
            panel3.Name = "panel3";
            panel3.Size = new Size(1179, 407);
            panel3.TabIndex = 3;
            // 
            // slider9
            // 
            slider9.Align = AntdUI.TAlignMini.Bottom;
            slider9.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            slider9.Dots = true;
            slider9.Location = new Point(291, 6);
            sliderMarkItem1.Fore = Color.FromArgb(115, 0, 0, 0);
            sliderMarkItem1.Text = "0°C";
            sliderMarkItem2.Text = "26°C";
            sliderMarkItem2.Value = 26;
            sliderMarkItem3.Text = "37°C";
            sliderMarkItem3.Value = 37;
            sliderMarkItem4.Fore = Color.FromArgb(255, 85, 0);
            sliderMarkItem4.Text = "100°C";
            sliderMarkItem4.Value = 100;
            slider9.Marks.Add(sliderMarkItem1);
            slider9.Marks.Add(sliderMarkItem2);
            slider9.Marks.Add(sliderMarkItem3);
            slider9.Marks.Add(sliderMarkItem4);
            slider9.Name = "slider9";
            slider9.Padding = new Padding(0, 0, 34, 0);
            slider9.Size = new Size(84, 398);
            slider9.TabIndex = 1;
            slider9.ValueFormatChanged += slider7_ValueFormatChanged;
            // 
            // slider8
            // 
            slider8.Align = AntdUI.TAlignMini.Bottom;
            slider8.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            slider8.Dots = true;
            slider8.Location = new Point(152, 6);
            sliderMarkItem5.Fore = Color.FromArgb(115, 0, 0, 0);
            sliderMarkItem5.Text = "0°C";
            sliderMarkItem6.Text = "20°C";
            sliderMarkItem6.Value = 20;
            sliderMarkItem7.Text = "40°C";
            sliderMarkItem7.Value = 40;
            sliderMarkItem8.Fore = Color.BlueViolet;
            sliderMarkItem8.Text = "50°C";
            sliderMarkItem8.Value = 50;
            sliderMarkItem9.Text = "60°C";
            sliderMarkItem9.Value = 60;
            sliderMarkItem10.Text = "80%";
            sliderMarkItem10.Value = 80;
            sliderMarkItem11.Text = "100°C";
            sliderMarkItem11.Value = 100;
            slider8.Marks.Add(sliderMarkItem5);
            slider8.Marks.Add(sliderMarkItem6);
            slider8.Marks.Add(sliderMarkItem7);
            slider8.Marks.Add(sliderMarkItem8);
            slider8.Marks.Add(sliderMarkItem9);
            slider8.Marks.Add(sliderMarkItem10);
            slider8.Marks.Add(sliderMarkItem11);
            slider8.Name = "slider8";
            slider8.Padding = new Padding(0, 0, 34, 0);
            slider8.Size = new Size(84, 398);
            slider8.TabIndex = 1;
            slider8.ValueFormatChanged += slider7_ValueFormatChanged;
            // 
            // slider6
            // 
            slider6.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            slider6.Dots = true;
            slider6.DotSize = 24;
            slider6.DotSizeActive = 28;
            slider6.Fill = Color.FromArgb(140, 194, 32);
            slider6.FillActive = Color.FromArgb(140, 194, 32);
            slider6.FillHover = Color.FromArgb(152, 255, 46);
            slider6.LineSize = 12;
            slider6.Location = new Point(525, 196);
            sliderMarkItem12.Text = "徐\n泾\n东";
            sliderMarkItem13.Text = "虹\n桥\n火\n车\n站";
            sliderMarkItem13.Value = 1;
            sliderMarkItem14.Text = "北\n新\n泾";
            sliderMarkItem14.Value = 2;
            sliderMarkItem15.Text = "威\n宁\n路";
            sliderMarkItem15.Value = 3;
            sliderMarkItem16.Text = "中\n山\n公\n园";
            sliderMarkItem16.Value = 4;
            sliderMarkItem17.Text = "静\n安\n寺";
            sliderMarkItem17.Value = 5;
            sliderMarkItem18.Text = "南\n京\n西\n路";
            sliderMarkItem18.Value = 6;
            sliderMarkItem19.Text = "人\n民\n广\n场";
            sliderMarkItem19.Value = 7;
            sliderMarkItem20.Text = "世\n纪\n大\n道";
            sliderMarkItem20.Value = 8;
            sliderMarkItem21.Text = "张\n江\n高\n科";
            sliderMarkItem21.Value = 9;
            sliderMarkItem22.Text = "浦\n东\n国\n际\n机\n场";
            sliderMarkItem22.Value = 10;
            slider6.Marks.Add(sliderMarkItem12);
            slider6.Marks.Add(sliderMarkItem13);
            slider6.Marks.Add(sliderMarkItem14);
            slider6.Marks.Add(sliderMarkItem15);
            slider6.Marks.Add(sliderMarkItem16);
            slider6.Marks.Add(sliderMarkItem17);
            slider6.Marks.Add(sliderMarkItem18);
            slider6.Marks.Add(sliderMarkItem19);
            slider6.Marks.Add(sliderMarkItem20);
            slider6.Marks.Add(sliderMarkItem21);
            slider6.Marks.Add(sliderMarkItem22);
            slider6.MarkTextGap = 8;
            slider6.MaxValue = 10;
            slider6.Name = "slider6";
            slider6.Padding = new Padding(4, 0, 4, 110);
            slider6.Size = new Size(647, 195);
            slider6.TabIndex = 1;
            // 
            // slider5
            // 
            slider5.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            slider5.Dots = true;
            slider5.DotSize = 24;
            slider5.DotSizeActive = 28;
            slider5.Fill = Color.FromArgb(230, 0, 42);
            slider5.FillActive = Color.FromArgb(230, 0, 42);
            slider5.FillHover = Color.FromArgb(255, 0, 42);
            slider5.LineSize = 12;
            slider5.Location = new Point(525, 6);
            sliderMarkItem23.Text = "莘\n庄";
            sliderMarkItem24.Text = "外\n环\n路";
            sliderMarkItem24.Value = 1;
            sliderMarkItem25.Text = "莲\n花\n路";
            sliderMarkItem25.Value = 2;
            sliderMarkItem26.Text = "锦\n江\n乐\n园";
            sliderMarkItem26.Value = 3;
            sliderMarkItem27.Text = "上\n海\n南\n站";
            sliderMarkItem27.Value = 4;
            sliderMarkItem28.Text = "漕\n宝\n路";
            sliderMarkItem28.Value = 5;
            sliderMarkItem29.Text = "上\n海\n体\n育\n馆";
            sliderMarkItem29.Value = 6;
            sliderMarkItem30.Text = "徐\n家\n汇";
            sliderMarkItem30.Value = 7;
            sliderMarkItem31.Text = "人\n民\n广\n场";
            sliderMarkItem31.Value = 8;
            sliderMarkItem32.Text = "汉\n中\n路";
            sliderMarkItem32.Value = 9;
            sliderMarkItem33.Text = "上\n海\n火\n车\n站";
            sliderMarkItem33.Value = 10;
            sliderMarkItem34.Text = "中\n山\n北\n路";
            sliderMarkItem34.Value = 11;
            sliderMarkItem35.Text = "上\n海\n马\n戏\n城";
            sliderMarkItem35.Value = 12;
            sliderMarkItem36.Text = "富\n锦\n路";
            sliderMarkItem36.Value = 13;
            slider5.Marks.Add(sliderMarkItem23);
            slider5.Marks.Add(sliderMarkItem24);
            slider5.Marks.Add(sliderMarkItem25);
            slider5.Marks.Add(sliderMarkItem26);
            slider5.Marks.Add(sliderMarkItem27);
            slider5.Marks.Add(sliderMarkItem28);
            slider5.Marks.Add(sliderMarkItem29);
            slider5.Marks.Add(sliderMarkItem30);
            slider5.Marks.Add(sliderMarkItem31);
            slider5.Marks.Add(sliderMarkItem32);
            slider5.Marks.Add(sliderMarkItem33);
            slider5.Marks.Add(sliderMarkItem34);
            slider5.Marks.Add(sliderMarkItem35);
            slider5.Marks.Add(sliderMarkItem36);
            slider5.MarkTextGap = 8;
            slider5.MaxValue = 13;
            slider5.Name = "slider5";
            slider5.Padding = new Padding(4, 0, 4, 100);
            slider5.Size = new Size(647, 172);
            slider5.TabIndex = 1;
            slider5.Value = 6;
            // 
            // slider4
            // 
            slider4.Align = AntdUI.TAlignMini.Top;
            slider4.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            slider4.DotSize = 12;
            slider4.DotSizeActive = 15;
            slider4.LineSize = 6;
            slider4.Location = new Point(430, 6);
            sliderMarkItem37.Value = 20;
            sliderMarkItem38.Value = 30;
            sliderMarkItem39.Value = 50;
            sliderMarkItem40.Value = 70;
            slider4.Marks.Add(sliderMarkItem37);
            slider4.Marks.Add(sliderMarkItem38);
            slider4.Marks.Add(sliderMarkItem39);
            slider4.Marks.Add(sliderMarkItem40);
            slider4.Name = "slider4";
            slider4.Padding = new Padding(0, 4, 0, 4);
            slider4.ShowValue = true;
            slider4.Size = new Size(84, 398);
            slider4.TabIndex = 1;
            // 
            // slider7
            // 
            slider7.Align = AntdUI.TAlignMini.Bottom;
            slider7.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            slider7.Location = new Point(13, 6);
            sliderMarkItem41.Fore = Color.FromArgb(115, 0, 0, 0);
            sliderMarkItem41.Text = "0°C";
            sliderMarkItem42.Text = "26°C";
            sliderMarkItem42.Value = 26;
            sliderMarkItem43.Text = "37°C";
            sliderMarkItem43.Value = 37;
            sliderMarkItem44.Fore = Color.FromArgb(255, 85, 0);
            sliderMarkItem44.Text = "100°C";
            sliderMarkItem44.Value = 100;
            slider7.Marks.Add(sliderMarkItem41);
            slider7.Marks.Add(sliderMarkItem42);
            slider7.Marks.Add(sliderMarkItem43);
            slider7.Marks.Add(sliderMarkItem44);
            slider7.Name = "slider7";
            slider7.Padding = new Padding(0, 0, 34, 0);
            slider7.Size = new Size(84, 398);
            slider7.TabIndex = 1;
            slider7.ValueFormatChanged += slider7_ValueFormatChanged;
            // 
            // divider2
            // 
            divider2.Dock = DockStyle.Top;
            divider2.Font = new Font("Microsoft YaHei UI", 10F);
            divider2.LocalizationText = "Slider.{id}";
            divider2.Location = new Point(0, 82);
            divider2.Name = "divider2";
            divider2.Orientation = AntdUI.TOrientation.Left;
            divider2.Size = new Size(1179, 28);
            divider2.TabIndex = 0;
            divider2.TabStop = false;
            divider2.Text = "固定点";
            // 
            // panel2
            // 
            panel2.Controls.Add(slider2);
            panel2.Controls.Add(slider3);
            panel2.Controls.Add(slider1);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 28);
            panel2.Name = "panel2";
            panel2.Size = new Size(1179, 54);
            panel2.TabIndex = 1;
            // 
            // slider2
            // 
            slider2.Fill = Color.FromArgb(255, 87, 34);
            slider2.FillActive = Color.FromArgb(255, 87, 34);
            slider2.FillHover = Color.FromArgb(255, 133, 34);
            slider2.Location = new Point(305, 6);
            slider2.Name = "slider2";
            slider2.ShowValue = true;
            slider2.Size = new Size(245, 40);
            slider2.TabIndex = 1;
            // 
            // slider3
            // 
            slider3.Align = AntdUI.TAlignMini.Right;
            slider3.Location = new Point(597, 6);
            slider3.Name = "slider3";
            slider3.ShowValue = true;
            slider3.Size = new Size(245, 40);
            slider3.TabIndex = 1;
            // 
            // slider1
            // 
            slider1.Location = new Point(13, 6);
            slider1.Name = "slider1";
            slider1.ShowValue = true;
            slider1.Size = new Size(245, 40);
            slider1.TabIndex = 1;
            // 
            // divider1
            // 
            divider1.Dock = DockStyle.Top;
            divider1.Font = new Font("Microsoft YaHei UI", 10F);
            divider1.LocalizationText = "Slider.{id}";
            divider1.Location = new Point(0, 0);
            divider1.Name = "divider1";
            divider1.Orientation = AntdUI.TOrientation.Left;
            divider1.Size = new Size(1179, 28);
            divider1.TabIndex = 0;
            divider1.TabStop = false;
            divider1.Text = "基本";
            // 
            // Slider
            // 
            Controls.Add(panel1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 12F);
            Name = "Slider";
            Size = new Size(1179, 676);
            panel1.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.PageHeader header1;
        private System.Windows.Forms.Panel panel1;
        private AntdUI.Divider divider1;
        private System.Windows.Forms.Panel panel3;
        private AntdUI.Divider divider2;
        private System.Windows.Forms.Panel panel2;
        private AntdUI.Slider slider1;
        private AntdUI.Slider slider2;
        private AntdUI.Slider slider3;
        private AntdUI.Slider slider7;
        private AntdUI.Slider slider8;
        private AntdUI.Slider slider9;
        private AntdUI.Slider slider4;
        private AntdUI.Slider slider6;
        private AntdUI.Slider slider5;
    }
}