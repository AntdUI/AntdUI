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

namespace Overview.Controls
{
    partial class Button
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
            header1 = new AntdUI.Header();
            button5 = new AntdUI.Button();
            button16 = new AntdUI.Button();
            button9 = new AntdUI.Button();
            button7 = new AntdUI.Button();
            panel3 = new System.Windows.Forms.Panel();
            panel1 = new System.Windows.Forms.Panel();
            panel2 = new AntdUI.Panel();
            button10 = new AntdUI.Button();
            button15 = new AntdUI.Button();
            button2 = new AntdUI.Button();
            panel5 = new System.Windows.Forms.Panel();
            button8 = new AntdUI.Button();
            button20 = new AntdUI.Button();
            divider3 = new AntdUI.Divider();
            flowLayoutPanel2 = new FlowLayoutPanel();
            button3 = new AntdUI.Button();
            button25 = new AntdUI.Button();
            button23 = new AntdUI.Button();
            button218 = new AntdUI.Button();
            button210 = new AntdUI.Button();
            button216 = new AntdUI.Button();
            button220 = new AntdUI.Button();
            divider4 = new AntdUI.Divider();
            flowLayoutPanel1 = new FlowLayoutPanel();
            button1 = new AntdUI.Button();
            button4 = new AntdUI.Button();
            button26 = new AntdUI.Button();
            button6 = new AntdUI.Button();
            divider2 = new AntdUI.Divider();
            panel4 = new FlowLayoutPanel();
            button17 = new AntdUI.Button();
            button18 = new AntdUI.Button();
            button19 = new AntdUI.Button();
            divider1 = new AntdUI.Divider();
            panel3.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel5.SuspendLayout();
            flowLayoutPanel2.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            panel4.SuspendLayout();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Dock = DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new Padding(6);
            header1.Size = new Size(1300, 79);
            header1.TabIndex = 4;
            header1.Text = "Button 按钮";
            header1.TextDesc = "按钮用于开始一个即时操作。";
            // 
            // button5
            // 
            button5.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button5.BorderWidth = 2F;
            button5.Ghost = true;
            button5.Location = new Point(546, 3);
            button5.Name = "button5";
            button5.Size = new Size(154, 46);
            button5.TabIndex = 0;
            button5.Text = "Danger Default";
            button5.Type = AntdUI.TTypeMini.Error;
            button5.Click += Btn2;
            // 
            // button16
            // 
            button16.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button16.Location = new Point(448, 3);
            button16.Name = "button16";
            button16.Size = new Size(92, 46);
            button16.TabIndex = 0;
            button16.Text = "Danger";
            button16.Type = AntdUI.TTypeMini.Error;
            button16.Click += Btn2;
            // 
            // button9
            // 
            button9.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button9.ImageSvg = Properties.Resources.icon_search;
            button9.Location = new Point(56, 3);
            button9.Name = "button9";
            button9.Size = new Size(94, 46);
            button9.TabIndex = 0;
            button9.Text = "搜索";
            button9.Type = AntdUI.TTypeMini.Primary;
            button9.Click += Btn2;
            // 
            // button7
            // 
            button7.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button7.ImageSvg = Properties.Resources.icon_poweroff;
            button7.Location = new Point(3, 3);
            button7.Name = "button7";
            button7.Shape = AntdUI.TShape.Circle;
            button7.Size = new Size(47, 47);
            button7.TabIndex = 0;
            button7.Type = AntdUI.TTypeMini.Primary;
            button7.Click += Btn2;
            // 
            // panel3
            // 
            panel3.AutoScroll = true;
            panel3.Controls.Add(panel1);
            panel3.Controls.Add(divider3);
            panel3.Controls.Add(flowLayoutPanel2);
            panel3.Controls.Add(divider4);
            panel3.Controls.Add(flowLayoutPanel1);
            panel3.Controls.Add(divider2);
            panel3.Controls.Add(panel4);
            panel3.Controls.Add(divider1);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(0, 79);
            panel3.Name = "panel3";
            panel3.Size = new Size(1300, 597);
            panel3.TabIndex = 6;
            // 
            // panel1
            // 
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(panel5);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 346);
            panel1.Name = "panel1";
            panel1.Size = new Size(1300, 118);
            panel1.TabIndex = 8;
            // 
            // panel2
            // 
            panel2.Controls.Add(button10);
            panel2.Controls.Add(button15);
            panel2.Controls.Add(button2);
            panel2.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            panel2.Location = new Point(14, 10);
            panel2.Name = "panel2";
            panel2.Padding = new Padding(4);
            panel2.Shadow = 18;
            panel2.Size = new Size(417, 83);
            panel2.TabIndex = 0;
            panel2.Text = "panel2";
            // 
            // button10
            // 
            button10.AutoSizeMode = AntdUI.TAutoSize.Width;
            button10.BackColor = Color.FromArgb(217, 217, 217);
            button10.BorderWidth = 2F;
            button10.Dock = DockStyle.Left;
            button10.JoinLeft = true;
            button10.Location = new Point(264, 22);
            button10.Margin = new Padding(0);
            button10.Name = "button10";
            button10.Size = new Size(121, 39);
            button10.TabIndex = 0;
            button10.Text = "Default Button";
            button10.Click += Btn;
            // 
            // button15
            // 
            button15.AutoSizeMode = AntdUI.TAutoSize.Width;
            button15.BackColor = Color.FromArgb(217, 217, 217);
            button15.BorderWidth = 2F;
            button15.Dock = DockStyle.Left;
            button15.JoinLeft = true;
            button15.JoinRight = true;
            button15.Location = new Point(143, 22);
            button15.Margin = new Padding(0);
            button15.Name = "button15";
            button15.Size = new Size(121, 39);
            button15.TabIndex = 0;
            button15.Text = "Default Button";
            button15.Click += Btn;
            // 
            // button2
            // 
            button2.AutoSizeMode = AntdUI.TAutoSize.Width;
            button2.BackColor = Color.FromArgb(217, 217, 217);
            button2.BorderWidth = 2F;
            button2.Dock = DockStyle.Left;
            button2.JoinRight = true;
            button2.Location = new Point(22, 22);
            button2.Margin = new Padding(0);
            button2.Name = "button2";
            button2.Size = new Size(121, 39);
            button2.TabIndex = 0;
            button2.Text = "Default Button";
            button2.Click += Btn;
            // 
            // panel5
            // 
            panel5.Controls.Add(button8);
            panel5.Controls.Add(button20);
            panel5.Font = new Font("Microsoft YaHei UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            panel5.Location = new Point(462, 27);
            panel5.Name = "panel5";
            panel5.Size = new Size(147, 48);
            panel5.TabIndex = 16;
            // 
            // button8
            // 
            button8.BackColor = Color.FromArgb(17, 24, 39);
            button8.BackActive = Color.FromArgb(17, 24, 39);
            button8.BackHover = Color.FromArgb(17, 24, 39);
            button8.Dock = DockStyle.Fill;
            button8.JoinLeft = true;
            button8.Location = new Point(50, 0);
            button8.Margins = 0;
            button8.Name = "button8";
            button8.Radius = 4;
            button8.Size = new Size(97, 48);
            button8.TabIndex = 1;
            button8.Text = "Button";
            button8.Type = AntdUI.TTypeMini.Primary;
            // 
            // button20
            // 
            button20.BackColor = Color.FromArgb(168, 85, 247);
            button20.BackActive = Color.FromArgb(147, 51, 234);
            button20.BackHover = Color.FromArgb(147, 51, 234);
            button20.Dock = DockStyle.Left;
            button20.ImageSvg = Properties.Resources.icon_search;
            button20.JoinRight = true;
            button20.Location = new Point(0, 0);
            button20.Margins = 0;
            button20.Name = "button20";
            button20.Radius = 4;
            button20.Size = new Size(50, 48);
            button20.TabIndex = 0;
            button20.Type = AntdUI.TTypeMini.Primary;
            // 
            // divider3
            // 
            divider3.Dock = DockStyle.Top;
            divider3.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            divider3.Location = new Point(0, 324);
            divider3.Name = "divider3";
            divider3.Orientation = AntdUI.TOrientation.Left;
            divider3.Size = new Size(1300, 22);
            divider3.TabIndex = 7;
            divider3.Text = "组合按钮";
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.Controls.Add(button3);
            flowLayoutPanel2.Controls.Add(button25);
            flowLayoutPanel2.Controls.Add(button23);
            flowLayoutPanel2.Controls.Add(button218);
            flowLayoutPanel2.Controls.Add(button210);
            flowLayoutPanel2.Controls.Add(button216);
            flowLayoutPanel2.Controls.Add(button220);
            flowLayoutPanel2.Dock = DockStyle.Top;
            flowLayoutPanel2.Location = new Point(0, 231);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new Size(1300, 93);
            flowLayoutPanel2.TabIndex = 12;
            // 
            // button3
            // 
            button3.BorderWidth = 1F;
            button3.Location = new Point(3, 3);
            button3.Name = "button3";
            button3.ShowArrow = true;
            button3.Size = new Size(152, 44);
            button3.TabIndex = 22;
            button3.Text = "button21";
            button3.TextAlign = ContentAlignment.MiddleLeft;
            button3.Type = AntdUI.TTypeMini.Primary;
            button3.Click += Btn2;
            // 
            // button25
            // 
            button25.BorderWidth = 1F;
            button25.ImageSvg = Properties.Resources.icon_search;
            button25.Location = new Point(161, 3);
            button25.Name = "button25";
            button25.ShowArrow = true;
            button25.Size = new Size(152, 44);
            button25.TabIndex = 24;
            button25.Text = "button21";
            button25.TextAlign = ContentAlignment.MiddleLeft;
            button25.Type = AntdUI.TTypeMini.Primary;
            button25.Click += Btn2;
            // 
            // button23
            // 
            button23.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button23.BorderWidth = 1F;
            button23.ImageSvg = Properties.Resources.icon_poweroff;
            button23.IsLink = true;
            button23.Location = new Point(319, 3);
            button23.Name = "button23";
            button23.ShowArrow = true;
            button23.Size = new Size(160, 46);
            button23.TabIndex = 26;
            button23.Text = "button21";
            button23.TextAlign = ContentAlignment.MiddleLeft;
            button23.Click += Btn2;
            // 
            // button218
            // 
            button218.BorderWidth = 1F;
            button218.Ghost = true;
            button218.Location = new Point(485, 3);
            button218.Name = "button218";
            button218.ShowArrow = true;
            button218.Size = new Size(152, 44);
            button218.TabIndex = 21;
            button218.Text = "button21";
            button218.TextAlign = ContentAlignment.MiddleLeft;
            button218.Type = AntdUI.TTypeMini.Primary;
            button218.Click += Btn2;
            // 
            // button210
            // 
            button210.BorderWidth = 1F;
            button210.Location = new Point(643, 3);
            button210.Name = "button210";
            button210.ShowArrow = true;
            button210.Size = new Size(152, 44);
            button210.TabIndex = 20;
            button210.Text = "button21";
            button210.TextAlign = ContentAlignment.MiddleLeft;
            button210.Type = AntdUI.TTypeMini.Error;
            button210.Click += Btn2;
            // 
            // button216
            // 
            button216.BorderWidth = 1F;
            button216.Ghost = true;
            button216.IsLink = true;
            button216.Location = new Point(801, 3);
            button216.Name = "button216";
            button216.ShowArrow = true;
            button216.Size = new Size(152, 44);
            button216.TabIndex = 25;
            button216.Text = "button21";
            button216.TextAlign = ContentAlignment.MiddleLeft;
            button216.Click += Btn2;
            // 
            // button220
            // 
            button220.Ghost = true;
            button220.IsLink = true;
            button220.Location = new Point(959, 3);
            button220.Name = "button220";
            button220.ShowArrow = true;
            button220.Size = new Size(152, 44);
            button220.TabIndex = 23;
            button220.Text = "button21";
            button220.TextAlign = ContentAlignment.MiddleLeft;
            button220.Click += Btn2;
            // 
            // divider4
            // 
            divider4.Dock = DockStyle.Top;
            divider4.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            divider4.Location = new Point(0, 209);
            divider4.Name = "divider4";
            divider4.Orientation = AntdUI.TOrientation.Left;
            divider4.Size = new Size(1300, 22);
            divider4.TabIndex = 11;
            divider4.Text = "连接按钮";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(button7);
            flowLayoutPanel1.Controls.Add(button9);
            flowLayoutPanel1.Controls.Add(button1);
            flowLayoutPanel1.Controls.Add(button4);
            flowLayoutPanel1.Controls.Add(button26);
            flowLayoutPanel1.Controls.Add(button6);
            flowLayoutPanel1.Dock = DockStyle.Top;
            flowLayoutPanel1.Location = new Point(0, 137);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(1300, 72);
            flowLayoutPanel1.TabIndex = 10;
            // 
            // button1
            // 
            button1.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button1.BorderWidth = 1F;
            button1.ImageSvg = Properties.Resources.icon_poweroff;
            button1.Location = new Point(156, 3);
            button1.Name = "button1";
            button1.Shape = AntdUI.TShape.Circle;
            button1.Size = new Size(47, 47);
            button1.TabIndex = 1;
            button1.Click += Btn2;
            // 
            // button4
            // 
            button4.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button4.BorderWidth = 1F;
            button4.ImageSvg = Properties.Resources.icon_search;
            button4.Location = new Point(209, 3);
            button4.Name = "button4";
            button4.Size = new Size(94, 46);
            button4.TabIndex = 2;
            button4.Text = "搜索";
            button4.Click += Btn2;
            // 
            // button26
            // 
            button26.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button26.BorderWidth = 2F;
            button26.ImageSvg = Properties.Resources.icon_search;
            button26.Location = new Point(309, 3);
            button26.Name = "button26";
            button26.Shape = AntdUI.TShape.Circle;
            button26.Size = new Size(47, 47);
            button26.TabIndex = 6;
            button26.Type = AntdUI.TTypeMini.Error;
            button26.Click += Btn2;
            // 
            // button6
            // 
            button6.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button6.ImageSvg = Properties.Resources.icon_search;
            button6.Location = new Point(362, 3);
            button6.Name = "button6";
            button6.Size = new Size(94, 46);
            button6.TabIndex = 7;
            button6.Text = "搜索";
            button6.Type = AntdUI.TTypeMini.Error;
            button6.Click += Btn2;
            // 
            // divider2
            // 
            divider2.Dock = DockStyle.Top;
            divider2.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            divider2.Location = new Point(0, 115);
            divider2.Name = "divider2";
            divider2.Orientation = AntdUI.TOrientation.Left;
            divider2.Size = new Size(1300, 22);
            divider2.TabIndex = 9;
            divider2.Text = "图标按钮";
            // 
            // panel4
            // 
            panel4.Controls.Add(button17);
            panel4.Controls.Add(button18);
            panel4.Controls.Add(button19);
            panel4.Controls.Add(button16);
            panel4.Controls.Add(button5);
            panel4.Dock = DockStyle.Top;
            panel4.Location = new Point(0, 22);
            panel4.Name = "panel4";
            panel4.Size = new Size(1300, 93);
            panel4.TabIndex = 2;
            // 
            // button17
            // 
            button17.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button17.Location = new Point(3, 3);
            button17.Name = "button17";
            button17.Size = new Size(152, 46);
            button17.TabIndex = 0;
            button17.Text = "Primary Button";
            button17.Type = AntdUI.TTypeMini.Primary;
            button17.Click += Btn2;
            // 
            // button18
            // 
            button18.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button18.BorderWidth = 1F;
            button18.Location = new Point(161, 3);
            button18.Name = "button18";
            button18.Size = new Size(150, 46);
            button18.TabIndex = 0;
            button18.Text = "Default Button";
            button18.Click += Btn2;
            // 
            // button19
            // 
            button19.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button19.Ghost = true;
            button19.Location = new Point(317, 3);
            button19.Name = "button19";
            button19.Size = new Size(125, 46);
            button19.TabIndex = 0;
            button19.Text = "Text Button";
            button19.Click += Btn2;
            // 
            // divider1
            // 
            divider1.Dock = DockStyle.Top;
            divider1.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            divider1.Location = new Point(0, 0);
            divider1.Name = "divider1";
            divider1.Orientation = AntdUI.TOrientation.Left;
            divider1.Size = new Size(1300, 22);
            divider1.TabIndex = 1;
            divider1.Text = "按钮类型";
            // 
            // Button
            // 
            Controls.Add(panel3);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            Name = "Button";
            Size = new Size(1300, 676);
            panel3.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel5.ResumeLayout(false);
            flowLayoutPanel2.ResumeLayout(false);
            flowLayoutPanel2.PerformLayout();
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.Header header1;
        private AntdUI.Button button16;
        private AntdUI.Button button5;
        private AntdUI.Button button7;
        private AntdUI.Button button9;
        private System.Windows.Forms.Panel panel3;
        private AntdUI.Divider divider1;
        private FlowLayoutPanel panel4;
        private AntdUI.Button button19;
        private AntdUI.Button button18;
        private AntdUI.Button button17;
        private System.Windows.Forms.Panel panel1;
        private AntdUI.Panel panel2;
        private AntdUI.Divider divider3;
        private AntdUI.Button button10;
        private AntdUI.Button button15;
        private AntdUI.Button button2;
        private System.Windows.Forms.Panel panel5;
        private AntdUI.Button button8;
        private AntdUI.Button button20;
        private AntdUI.Button button26;
        private FlowLayoutPanel flowLayoutPanel1;
        private AntdUI.Button button1;
        private AntdUI.Button button4;
        private AntdUI.Button button6;
        private AntdUI.Divider divider2;
        private FlowLayoutPanel flowLayoutPanel2;
        private AntdUI.Divider divider4;
        private AntdUI.Button button3;
        private AntdUI.Button button25;
        private AntdUI.Button button23;
        private AntdUI.Button button218;
        private AntdUI.Button button210;
        private AntdUI.Button button216;
        private AntdUI.Button button220;
    }
}