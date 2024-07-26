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

namespace Demo
{
    partial class Main
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

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            AntdUI.CarouselItem carouselItem1 = new AntdUI.CarouselItem();
            AntdUI.CarouselItem carouselItem2 = new AntdUI.CarouselItem();
            AntdUI.CarouselItem carouselItem3 = new AntdUI.CarouselItem();
            AntdUI.CarouselItem carouselItem4 = new AntdUI.CarouselItem();
            button1 = new AntdUI.Button();
            button2 = new AntdUI.Button();
            button3 = new AntdUI.Button();
            radio1 = new AntdUI.Radio();
            radio2 = new AntdUI.Radio();
            radio3 = new AntdUI.Radio();
            radio4 = new AntdUI.Radio();
            radio5 = new AntdUI.Radio();
            checkbox1 = new AntdUI.Checkbox();
            checkbox2 = new AntdUI.Checkbox();
            checkbox3 = new AntdUI.Checkbox();
            checkbox4 = new AntdUI.Checkbox();
            checkbox5 = new AntdUI.Checkbox();
            switch1 = new AntdUI.Switch();
            switch2 = new AntdUI.Switch();
            switch3 = new AntdUI.Switch();
            switch4 = new AntdUI.Switch();
            switch5 = new AntdUI.Switch();
            switch6 = new AntdUI.Switch();
            switch7 = new AntdUI.Switch();
            switch8 = new AntdUI.Switch();
            button4 = new AntdUI.Button();
            avatar4 = new AntdUI.Avatar();
            avatar1 = new AntdUI.Avatar();
            tooltip1 = new AntdUI.Tooltip();
            progress1 = new AntdUI.Progress();
            progress2 = new AntdUI.Progress();
            progress3 = new AntdUI.Progress();
            progress4 = new AntdUI.Progress();
            progress5 = new AntdUI.Progress();
            progress6 = new AntdUI.Progress();
            progress7 = new AntdUI.Progress();
            progress8 = new AntdUI.Progress();
            progress9 = new AntdUI.Progress();
            panel_top = new Panel();
            label_title = new Label();
            btn_min = new AntdUI.Button();
            btn_max = new AntdUI.Button();
            btn_close = new AntdUI.Button();
            tooltipComponent1 = new AntdUI.TooltipComponent();
            avatar2 = new AntdUI.Carousel();
            panel8 = new AntdUI.Panel();
            label5 = new Label();
            divider1 = new AntdUI.Divider();
            label7 = new Label();
            panel9 = new AntdUI.Panel();
            label9 = new Label();
            label8 = new Label();
            tooltipComponent2 = new AntdUI.TooltipComponent();
            badge1 = new AntdUI.Badge();
            badge2 = new AntdUI.Badge();
            badge3 = new AntdUI.Badge();
            badge4 = new AntdUI.Badge();
            badge5 = new AntdUI.Badge();
            badge6 = new AntdUI.Badge();
            avatar3 = new AntdUI.Avatar();
            input1 = new AntdUI.Input();
            button5 = new AntdUI.Button();
            button6 = new AntdUI.Button();
            panel_top.SuspendLayout();
            panel8.SuspendLayout();
            panel9.SuspendLayout();
            SuspendLayout();
            // 
            // button1
            // 
            button1.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button1.Location = new Point(40, 78);
            button1.Margin = new Padding(4);
            button1.Name = "button1";
            button1.Size = new Size(145, 47);
            button1.TabIndex = 0;
            button1.Text = "Primary Button";
            button1.Type = AntdUI.TTypeMini.Primary;
            button1.Click += Button_Click;
            // 
            // button2
            // 
            button2.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button2.BorderWidth = 2F;
            button2.Location = new Point(210, 78);
            button2.Margin = new Padding(4);
            button2.Name = "button2";
            button2.Size = new Size(141, 47);
            button2.TabIndex = 0;
            button2.Text = "Default Button";
            button2.Click += Button_Click;
            // 
            // button3
            // 
            button3.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button3.BorderWidth = 2F;
            button3.Ghost = true;
            button3.Location = new Point(380, 78);
            button3.Margin = new Padding(4);
            button3.Name = "button3";
            button3.Size = new Size(143, 47);
            button3.TabIndex = 0;
            button3.Text = "Danger Button";
            button3.Type = AntdUI.TTypeMini.Error;
            button3.Click += Button_Click;
            // 
            // radio1
            // 
            radio1.AutoSizeMode = AntdUI.TAutoSize.Auto;
            radio1.Location = new Point(40, 168);
            radio1.Name = "radio1";
            radio1.Size = new Size(117, 43);
            radio1.TabIndex = 1;
            radio1.Text = "Option A";
            // 
            // radio2
            // 
            radio2.AutoSizeMode = AntdUI.TAutoSize.Auto;
            radio2.Location = new Point(210, 168);
            radio2.Name = "radio2";
            radio2.Size = new Size(115, 43);
            radio2.TabIndex = 1;
            radio2.Text = "Option B";
            // 
            // radio3
            // 
            radio3.AutoSizeMode = AntdUI.TAutoSize.Auto;
            radio3.Checked = true;
            radio3.Location = new Point(380, 168);
            radio3.Name = "radio3";
            radio3.Size = new Size(117, 43);
            radio3.TabIndex = 1;
            radio3.Text = "Option C";
            // 
            // radio4
            // 
            radio4.AutoSizeMode = AntdUI.TAutoSize.Auto;
            radio4.Enabled = false;
            radio4.Location = new Point(550, 168);
            radio4.Name = "radio4";
            radio4.Size = new Size(118, 43);
            radio4.TabIndex = 1;
            radio4.Text = "Option D";
            // 
            // radio5
            // 
            radio5.AutoSizeMode = AntdUI.TAutoSize.Auto;
            radio5.Enabled = false;
            radio5.Location = new Point(720, 168);
            radio5.Name = "radio5";
            radio5.Size = new Size(115, 43);
            radio5.TabIndex = 1;
            radio5.Text = "Option E";
            // 
            // checkbox1
            // 
            checkbox1.AutoSizeMode = AntdUI.TAutoSize.Auto;
            checkbox1.Location = new Point(40, 239);
            checkbox1.Name = "checkbox1";
            checkbox1.Size = new Size(117, 43);
            checkbox1.TabIndex = 2;
            checkbox1.Text = "Option A";
            // 
            // checkbox2
            // 
            checkbox2.AutoSizeMode = AntdUI.TAutoSize.Auto;
            checkbox2.Location = new Point(210, 239);
            checkbox2.Name = "checkbox2";
            checkbox2.Size = new Size(115, 43);
            checkbox2.TabIndex = 2;
            checkbox2.Text = "Option B";
            // 
            // checkbox3
            // 
            checkbox3.AutoSizeMode = AntdUI.TAutoSize.Auto;
            checkbox3.Checked = true;
            checkbox3.Location = new Point(380, 239);
            checkbox3.Name = "checkbox3";
            checkbox3.Size = new Size(117, 43);
            checkbox3.TabIndex = 2;
            checkbox3.Text = "Option C";
            // 
            // checkbox4
            // 
            checkbox4.AutoSizeMode = AntdUI.TAutoSize.Auto;
            checkbox4.Enabled = false;
            checkbox4.Location = new Point(550, 239);
            checkbox4.Name = "checkbox4";
            checkbox4.Size = new Size(118, 43);
            checkbox4.TabIndex = 2;
            checkbox4.Text = "Option D";
            // 
            // checkbox5
            // 
            checkbox5.AutoSizeMode = AntdUI.TAutoSize.Auto;
            checkbox5.Enabled = false;
            checkbox5.Location = new Point(720, 239);
            checkbox5.Name = "checkbox5";
            checkbox5.Size = new Size(115, 43);
            checkbox5.TabIndex = 2;
            checkbox5.Text = "Option E";
            // 
            // switch1
            // 
            switch1.Checked = true;
            switch1.Location = new Point(907, 170);
            switch1.Name = "switch1";
            switch1.Size = new Size(60, 38);
            switch1.TabIndex = 3;
            // 
            // switch2
            // 
            switch2.Checked = true;
            switch2.Location = new Point(907, 241);
            switch2.Name = "switch2";
            switch2.Size = new Size(60, 38);
            switch2.TabIndex = 3;
            // 
            // switch3
            // 
            switch3.Location = new Point(999, 170);
            switch3.Name = "switch3";
            switch3.Size = new Size(60, 38);
            switch3.TabIndex = 3;
            // 
            // switch4
            // 
            switch4.Location = new Point(999, 241);
            switch4.Name = "switch4";
            switch4.Size = new Size(60, 38);
            switch4.TabIndex = 3;
            // 
            // switch5
            // 
            switch5.Checked = true;
            switch5.Enabled = false;
            switch5.Location = new Point(1091, 170);
            switch5.Name = "switch5";
            switch5.Size = new Size(60, 38);
            switch5.TabIndex = 3;
            // 
            // switch6
            // 
            switch6.Enabled = false;
            switch6.Location = new Point(1183, 170);
            switch6.Name = "switch6";
            switch6.Size = new Size(60, 38);
            switch6.TabIndex = 3;
            // 
            // switch7
            // 
            switch7.Checked = true;
            switch7.Enabled = false;
            switch7.Location = new Point(1091, 241);
            switch7.Name = "switch7";
            switch7.Size = new Size(60, 38);
            switch7.TabIndex = 3;
            // 
            // switch8
            // 
            switch8.Enabled = false;
            switch8.Location = new Point(1183, 241);
            switch8.Name = "switch8";
            switch8.Size = new Size(60, 38);
            switch8.TabIndex = 3;
            // 
            // button4
            // 
            button4.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button4.Location = new Point(550, 78);
            button4.Margin = new Padding(4);
            button4.Name = "button4";
            button4.Shape = AntdUI.TShape.Circle;
            button4.Size = new Size(47, 47);
            button4.TabIndex = 0;
            button4.Text = "按";
            button4.Type = AntdUI.TTypeMini.Primary;
            button4.Click += Button_Click;
            // 
            // avatar4
            // 
            avatar4.Image = Properties.Resources.img1;
            avatar4.Location = new Point(628, 79);
            avatar4.Name = "avatar4";
            avatar4.Round = true;
            avatar4.Size = new Size(40, 40);
            avatar4.TabIndex = 8;
            tooltipComponent1.SetTip(avatar4, "圆形头像");
            // 
            // avatar1
            // 
            avatar1.Image = Properties.Resources.img1;
            avatar1.Location = new Point(710, 79);
            avatar1.Name = "avatar1";
            avatar1.Radius = 10;
            avatar1.Size = new Size(40, 40);
            avatar1.TabIndex = 8;
            tooltipComponent1.SetTip(avatar1, "圆角头像");
            // 
            // tooltip1
            // 
            tooltip1.Location = new Point(907, 76);
            tooltip1.MaximumSize = new Size(117, 51);
            tooltip1.MinimumSize = new Size(117, 51);
            tooltip1.Name = "tooltip1";
            tooltip1.Size = new Size(117, 51);
            tooltip1.TabIndex = 9;
            tooltip1.Text = "Prompt Text";
            // 
            // progress1
            // 
            progress1.ContainerControl = this;
            progress1.Loading = true;
            progress1.Location = new Point(40, 332);
            progress1.Name = "progress1";
            progress1.Padding = new Padding(0, 10, 0, 10);
            progress1.ShowText = true;
            progress1.Size = new Size(480, 30);
            progress1.TabIndex = 10;
            progress1.Text = "%";
            progress1.Value = 0.5F;
            progress1.Click += Progress_Blue_1;
            // 
            // progress2
            // 
            progress2.ContainerControl = this;
            progress2.Location = new Point(40, 378);
            progress2.Name = "progress2";
            progress2.Padding = new Padding(0, 10, 0, 10);
            progress2.ShowText = true;
            progress2.Size = new Size(480, 30);
            progress2.State = AntdUI.TType.Success;
            progress2.TabIndex = 10;
            progress2.Text = "%";
            progress2.Value = 1F;
            // 
            // progress3
            // 
            progress3.ContainerControl = this;
            progress3.Location = new Point(40, 424);
            progress3.Name = "progress3";
            progress3.Padding = new Padding(0, 10, 0, 10);
            progress3.ShowText = true;
            progress3.Size = new Size(480, 30);
            progress3.State = AntdUI.TType.Error;
            progress3.TabIndex = 10;
            progress3.Text = "%";
            progress3.Value = 0.7F;
            progress3.Click += Progress_Red;
            // 
            // progress4
            // 
            progress4.ContainerControl = this;
            progress4.Font = new Font("Microsoft YaHei UI Light", 16F, FontStyle.Regular, GraphicsUnit.Point);
            progress4.Loading = true;
            progress4.Location = new Point(42, 488);
            progress4.Name = "progress4";
            progress4.Radius = 5;
            progress4.Shape = AntdUI.TShape.Circle;
            progress4.ShowText = true;
            progress4.Size = new Size(110, 110);
            progress4.TabIndex = 10;
            progress4.Text = "%";
            progress4.Value = 0.68F;
            progress4.Click += Progress_Blue_2;
            // 
            // progress5
            // 
            progress5.ContainerControl = this;
            progress5.Font = new Font("Microsoft YaHei UI Light", 16F, FontStyle.Regular, GraphicsUnit.Point);
            progress5.Location = new Point(201, 488);
            progress5.Name = "progress5";
            progress5.Radius = 5;
            progress5.Shape = AntdUI.TShape.Circle;
            progress5.ShowText = true;
            progress5.Size = new Size(110, 110);
            progress5.State = AntdUI.TType.Success;
            progress5.TabIndex = 10;
            progress5.Text = "%";
            progress5.Value = 1F;
            // 
            // progress6
            // 
            progress6.ContainerControl = this;
            progress6.Font = new Font("Microsoft YaHei UI Light", 16F, FontStyle.Regular, GraphicsUnit.Point);
            progress6.Location = new Point(360, 488);
            progress6.Name = "progress6";
            progress6.Radius = 5;
            progress6.Shape = AntdUI.TShape.Circle;
            progress6.ShowText = true;
            progress6.Size = new Size(110, 110);
            progress6.State = AntdUI.TType.Error;
            progress6.TabIndex = 10;
            progress6.Text = "%";
            progress6.Value = 0.7F;
            progress6.Click += Progress_Red;
            // 
            // progress7
            // 
            progress7.Back = Color.FromArgb(40, 22, 119, 255);
            progress7.ContainerControl = this;
            progress7.Location = new Point(42, 622);
            progress7.Mini = true;
            progress7.Name = "progress7";
            progress7.Radius = 4;
            progress7.Size = new Size(139, 36);
            progress7.TabIndex = 10;
            progress7.Text = "In Progress";
            progress7.Value = 0.68F;
            progress7.Click += Progress_Blue_2;
            // 
            // progress8
            // 
            progress8.Back = Color.FromArgb(40, 0, 204, 0);
            progress8.ContainerControl = this;
            progress8.Location = new Point(201, 622);
            progress8.Mini = true;
            progress8.Name = "progress8";
            progress8.Radius = 4;
            progress8.Size = new Size(128, 36);
            progress8.State = AntdUI.TType.Success;
            progress8.TabIndex = 10;
            progress8.Text = "Success";
            progress8.Value = 1F;
            // 
            // progress9
            // 
            progress9.Back = Color.FromArgb(40, 255, 79, 87);
            progress9.ContainerControl = this;
            progress9.Location = new Point(360, 622);
            progress9.Mini = true;
            progress9.Name = "progress9";
            progress9.Radius = 4;
            progress9.Size = new Size(128, 36);
            progress9.State = AntdUI.TType.Error;
            progress9.TabIndex = 10;
            progress9.Text = "Failed";
            progress9.Value = 0.7F;
            progress9.Click += Progress_Red;
            // 
            // panel_top
            // 
            panel_top.Controls.Add(label_title);
            panel_top.Controls.Add(btn_min);
            panel_top.Controls.Add(btn_max);
            panel_top.Controls.Add(btn_close);
            panel_top.Dock = DockStyle.Top;
            panel_top.Location = new Point(0, 0);
            panel_top.Name = "panel_top";
            panel_top.Size = new Size(1300, 40);
            panel_top.TabIndex = 0;
            // 
            // label_title
            // 
            label_title.Dock = DockStyle.Fill;
            label_title.Location = new Point(0, 0);
            label_title.Name = "label_title";
            label_title.Padding = new Padding(10, 0, 0, 0);
            label_title.Size = new Size(1142, 40);
            label_title.TabIndex = 0;
            label_title.Text = "Ant Design 5.0";
            label_title.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btn_min
            // 
            btn_min.BackActive = Color.FromArgb(172, 172, 172);
            btn_min.BackColor = Color.Transparent;
            btn_min.BackHover = Color.FromArgb(223, 223, 223);
            btn_min.Dock = DockStyle.Right;
            btn_min.Font = new Font("Microsoft YaHei UI Light", 18F, FontStyle.Regular, GraphicsUnit.Point);
            btn_min.Ghost = true;
            btn_min.Image = Properties.Resources.app_minb;
            btn_min.Location = new Point(1142, 0);
            btn_min.Name = "btn_min";
            btn_min.Size = new Size(50, 40);
            btn_min.TabIndex = 3;
            btn_min.Click += btn_min_Click;
            // 
            // btn_max
            // 
            btn_max.BackActive = Color.FromArgb(172, 172, 172);
            btn_max.BackColor = Color.Transparent;
            btn_max.BackHover = Color.FromArgb(223, 223, 223);
            btn_max.Dock = DockStyle.Right;
            btn_max.Font = new Font("Microsoft YaHei UI Light", 18F, FontStyle.Regular, GraphicsUnit.Point);
            btn_max.Ghost = true;
            btn_max.Image = Properties.Resources.app_maxb;
            btn_max.Location = new Point(1192, 0);
            btn_max.Name = "btn_max";
            btn_max.Size = new Size(50, 40);
            btn_max.TabIndex = 2;
            btn_max.Click += btn_max_Click;
            // 
            // btn_close
            // 
            btn_close.BackActive = Color.FromArgb(145, 31, 20);
            btn_close.BackColor = Color.Transparent;
            btn_close.BackHover = Color.FromArgb(196, 43, 28);
            btn_close.Dock = DockStyle.Right;
            btn_close.Font = new Font("Microsoft YaHei UI Light", 20F, FontStyle.Regular, GraphicsUnit.Point);
            btn_close.Ghost = true;
            btn_close.Image = Properties.Resources.app_closeb;
            btn_close.ImageHover = Properties.Resources.app_close;
            btn_close.Location = new Point(1242, 0);
            btn_close.Name = "btn_close";
            btn_close.Size = new Size(58, 40);
            btn_close.TabIndex = 1;
            btn_close.Click += btn_close_Click;
            // 
            // tooltipComponent1
            // 
            tooltipComponent1.Font = new Font("Microsoft YaHei UI Light", 9F, FontStyle.Regular, GraphicsUnit.Point);
            // 
            // avatar2
            // 
            avatar2.BackColor = Color.Transparent;
            avatar2.Dock = DockStyle.Top;
            avatar2.DotPosition = AntdUI.TAlignMini.Bottom;
            carouselItem1.Img = Properties.Resources.img1;
            carouselItem2.Img = Properties.Resources.bg2;
            carouselItem3.Img = Properties.Resources.bg3;
            carouselItem4.Img = Properties.Resources.bg1;
            avatar2.Image.AddRange(new AntdUI.CarouselItem[] { carouselItem1, carouselItem2, carouselItem3, carouselItem4 });
            avatar2.Location = new Point(38, 38);
            avatar2.Name = "avatar2";
            avatar2.Radius = 6;
            avatar2.Size = new Size(327, 150);
            avatar2.TabIndex = 9;
            tooltipComponent2.SetTip(avatar2, "宽横幅图片");
            // 
            // panel8
            // 
            panel8.ArrowAlign = AntdUI.TAlign.TL;
            panel8.ArrowSize = 10;
            panel8.Controls.Add(label5);
            panel8.Controls.Add(divider1);
            panel8.Controls.Add(label7);
            panel8.Location = new Point(550, 319);
            panel8.Name = "panel8";
            panel8.Radius = 10;
            panel8.Shadow = 24;
            panel8.ShadowOpacity = 0.18F;
            panel8.ShadowOpacityAnimation = true;
            panel8.Size = new Size(285, 228);
            panel8.TabIndex = 13;
            // 
            // label5
            // 
            label5.BackColor = Color.Transparent;
            label5.Dock = DockStyle.Fill;
            label5.Font = new Font("Microsoft YaHei UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            label5.Location = new Point(24, 73);
            label5.Name = "label5";
            label5.Padding = new Padding(20, 10, 0, 0);
            label5.Size = new Size(237, 131);
            label5.TabIndex = 2;
            label5.Text = "Card content\r\n\r\nCard content\r\n\r\nCard content";
            // 
            // divider1
            // 
            divider1.BackColor = Color.Transparent;
            divider1.Dock = DockStyle.Top;
            divider1.Location = new Point(24, 72);
            divider1.Margin = new Padding(16);
            divider1.Name = "divider1";
            divider1.Size = new Size(237, 1);
            divider1.TabIndex = 1;
            // 
            // label7
            // 
            label7.BackColor = Color.Transparent;
            label7.Dock = DockStyle.Top;
            label7.Font = new Font("Microsoft YaHei UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point);
            label7.Location = new Point(24, 24);
            label7.Name = "label7";
            label7.Padding = new Padding(20, 0, 0, 0);
            label7.Size = new Size(237, 48);
            label7.TabIndex = 0;
            label7.Text = "Card title";
            label7.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // panel9
            // 
            panel9.ArrowAlign = AntdUI.TAlign.Top;
            panel9.ArrowSize = 10;
            panel9.Back = Color.FromArgb(0, 144, 255);
            panel9.Controls.Add(label9);
            panel9.Controls.Add(label8);
            panel9.Controls.Add(avatar2);
            panel9.ForeColor = Color.White;
            panel9.Location = new Point(862, 318);
            panel9.Name = "panel9";
            panel9.Padding = new Padding(14);
            panel9.Radius = 10;
            panel9.Shadow = 24;
            panel9.ShadowOpacity = 0.18F;
            panel9.ShadowOpacityAnimation = true;
            panel9.Size = new Size(403, 304);
            panel9.TabIndex = 13;
            // 
            // label9
            // 
            label9.BackColor = Color.Transparent;
            label9.Dock = DockStyle.Fill;
            label9.Location = new Point(38, 218);
            label9.Name = "label9";
            label9.Padding = new Padding(2, 0, 2, 0);
            label9.Size = new Size(327, 48);
            label9.TabIndex = 12;
            label9.Text = "Here is the content, here is the conten, \r\nthere is the content...";
            // 
            // label8
            // 
            label8.BackColor = Color.Transparent;
            label8.Dock = DockStyle.Top;
            label8.Font = new Font("Microsoft YaHei UI Light", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label8.Location = new Point(38, 188);
            label8.Name = "label8";
            label8.Size = new Size(327, 30);
            label8.TabIndex = 11;
            label8.Text = "Tour Title";
            label8.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // tooltipComponent2
            // 
            tooltipComponent2.Font = new Font("Microsoft YaHei UI Light", 12F, FontStyle.Regular, GraphicsUnit.Point);
            // 
            // badge1
            // 
            badge1.Location = new Point(572, 580);
            badge1.Name = "badge1";
            badge1.Size = new Size(111, 34);
            badge1.State = AntdUI.TState.Success;
            badge1.TabIndex = 14;
            badge1.Text = "Success";
            // 
            // badge2
            // 
            badge2.Location = new Point(689, 580);
            badge2.Name = "badge2";
            badge2.Size = new Size(74, 34);
            badge2.State = AntdUI.TState.Error;
            badge2.TabIndex = 14;
            badge2.Text = "Error";
            // 
            // badge3
            // 
            badge3.Location = new Point(794, 580);
            badge3.Name = "badge3";
            badge3.Size = new Size(31, 34);
            badge3.State = AntdUI.TState.Error;
            badge3.TabIndex = 14;
            // 
            // badge4
            // 
            badge4.Location = new Point(572, 623);
            badge4.Name = "badge4";
            badge4.Size = new Size(111, 34);
            badge4.State = AntdUI.TState.Processing;
            badge4.TabIndex = 14;
            badge4.Text = "Processing";
            // 
            // badge5
            // 
            badge5.Location = new Point(689, 623);
            badge5.Name = "badge5";
            badge5.Size = new Size(74, 34);
            badge5.State = AntdUI.TState.Warn;
            badge5.TabIndex = 14;
            badge5.Text = "Warn";
            // 
            // badge6
            // 
            badge6.Location = new Point(794, 623);
            badge6.Name = "badge6";
            badge6.Size = new Size(31, 34);
            badge6.State = AntdUI.TState.Processing;
            badge6.TabIndex = 14;
            // 
            // avatar3
            // 
            avatar3.BackColor = Color.FromArgb(0, 144, 255);
            avatar3.Font = new Font("Microsoft YaHei UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            avatar3.ForeColor = Color.White;
            avatar3.Location = new Point(795, 79);
            avatar3.Name = "avatar3";
            avatar3.Radius = 10;
            avatar3.Size = new Size(40, 40);
            avatar3.TabIndex = 8;
            avatar3.Text = "U";
            // 
            // input1
            // 
            input1.Location = new Point(1091, 77);
            input1.Name = "input1";
            input1.Size = new Size(152, 44);
            input1.TabIndex = 15;
            input1.Text = "Input Text";
            // 
            // button5
            // 
            button5.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button5.BackColor = Color.FromArgb(100, 22, 119, 255);
            button5.BackgroundImage = Properties.Resources.bg3;
            button5.BackgroundImageLayout = AntdUI.TFit.Cover;
            button5.BackHover = Color.FromArgb(100, 64, 150, 255);
            button5.ForeColor = Color.White;
            button5.Ghost = true;
            button5.Image = Properties.Resources.search;
            button5.Location = new Point(887, 625);
            button5.Margin = new Padding(4);
            button5.Name = "button5";
            button5.Size = new Size(166, 47);
            button5.TabIndex = 0;
            button5.Text = "Search Button";
            button5.Click += Button_Click;
            // 
            // button6
            // 
            button6.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button6.BackColor = Color.FromArgb(100, 22, 119, 255);
            button6.BackgroundImage = Properties.Resources.bg2;
            button6.BackgroundImageLayout = AntdUI.TFit.Cover;
            button6.BackHover = Color.FromArgb(100, 64, 150, 255);
            button6.ForeColor = Color.White;
            button6.Ghost = true;
            button6.Image = Properties.Resources.search;
            button6.Location = new Point(1080, 625);
            button6.Margin = new Padding(4);
            button6.Margins = 6;
            button6.Name = "button6";
            button6.Size = new Size(170, 49);
            button6.TabIndex = 0;
            button6.Text = "Search Button";
            button6.Click += Button_Click;
            // 
            // Main
            // 
            BackColor = Color.White;
            ClientSize = new Size(1300, 720);
            Controls.Add(progress3);
            Controls.Add(progress2);
            Controls.Add(progress1);
            Controls.Add(input1);
            Controls.Add(badge3);
            Controls.Add(badge2);
            Controls.Add(badge6);
            Controls.Add(badge5);
            Controls.Add(badge4);
            Controls.Add(badge1);
            Controls.Add(panel9);
            Controls.Add(panel8);
            Controls.Add(panel_top);
            Controls.Add(progress9);
            Controls.Add(progress6);
            Controls.Add(progress8);
            Controls.Add(progress5);
            Controls.Add(progress7);
            Controls.Add(progress4);
            Controls.Add(tooltip1);
            Controls.Add(avatar3);
            Controls.Add(avatar1);
            Controls.Add(avatar4);
            Controls.Add(switch8);
            Controls.Add(switch4);
            Controls.Add(switch7);
            Controls.Add(switch2);
            Controls.Add(switch6);
            Controls.Add(switch3);
            Controls.Add(switch5);
            Controls.Add(switch1);
            Controls.Add(checkbox5);
            Controls.Add(checkbox4);
            Controls.Add(checkbox3);
            Controls.Add(checkbox2);
            Controls.Add(checkbox1);
            Controls.Add(radio3);
            Controls.Add(radio5);
            Controls.Add(radio4);
            Controls.Add(radio2);
            Controls.Add(radio1);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button4);
            Controls.Add(button6);
            Controls.Add(button5);
            Controls.Add(button1);
            Font = new Font("Microsoft YaHei UI Light", 12F, FontStyle.Regular, GraphicsUnit.Point);
            ForeColor = Color.Black;
            Margin = new Padding(3, 4, 3, 4);
            MinimumSize = new Size(1300, 720);
            Name = "Main";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Ant Design 5.0";
            panel_top.ResumeLayout(false);
            panel8.ResumeLayout(false);
            panel9.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private AntdUI.Button button1;
        private AntdUI.Button button2;
        private AntdUI.Button button3;
        private AntdUI.Radio radio1;
        private AntdUI.Radio radio2;
        private AntdUI.Radio radio3;
        private AntdUI.Radio radio4;
        private AntdUI.Radio radio5;
        private AntdUI.Checkbox checkbox1;
        private AntdUI.Checkbox checkbox2;
        private AntdUI.Checkbox checkbox3;
        private AntdUI.Checkbox checkbox4;
        private AntdUI.Checkbox checkbox5;
        private AntdUI.Switch switch1;
        private AntdUI.Switch switch2;
        private AntdUI.Switch switch3;
        private AntdUI.Switch switch4;
        private AntdUI.Switch switch5;
        private AntdUI.Switch switch6;
        private AntdUI.Switch switch7;
        private AntdUI.Switch switch8;
        private AntdUI.Button button4;
        private AntdUI.Avatar avatar4;
        private AntdUI.Avatar avatar1;
        private AntdUI.Tooltip tooltip1;
        private AntdUI.Progress progress1;
        private AntdUI.Progress progress2;
        private AntdUI.Progress progress3;
        private AntdUI.Progress progress4;
        private AntdUI.Progress progress5;
        private AntdUI.Progress progress6;
        private AntdUI.Progress progress7;
        private AntdUI.Progress progress8;
        private AntdUI.Progress progress9;
        private Panel panel_top;
        private Label label_title;
        private AntdUI.Button btn_close;
        private AntdUI.Button btn_min;
        private AntdUI.Button btn_max;
        private AntdUI.TooltipComponent tooltipComponent1;
        private AntdUI.Panel panel8;
        private Label label5;
        private AntdUI.Divider divider1;
        private Label label7;
        private AntdUI.Panel panel9;
        private Label label9;
        private Label label8;
        private AntdUI.Carousel avatar2;
        private AntdUI.TooltipComponent tooltipComponent2;
        private AntdUI.Badge badge1;
        private AntdUI.Badge badge2;
        private AntdUI.Badge badge3;
        private AntdUI.Badge badge4;
        private AntdUI.Badge badge5;
        private AntdUI.Badge badge6;
        private AntdUI.Avatar avatar3;
        private AntdUI.Input input1;
        private AntdUI.Button button5;
        private AntdUI.Button button6;
    }
}