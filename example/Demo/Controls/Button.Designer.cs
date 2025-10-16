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

        private void InitializeComponent()
        {
            header1 = new AntdUI.PageHeader();
            panel_main = new System.Windows.Forms.Panel();
            panel6 = new System.Windows.Forms.Panel();
            panel_btns = new AntdUI.Panel();
            btng3 = new AntdUI.Button();
            btng2 = new AntdUI.Button();
            btng1 = new AntdUI.Button();
            panel7 = new System.Windows.Forms.Panel();
            button41 = new AntdUI.Button();
            button40 = new AntdUI.Button();
            divider5 = new AntdUI.Divider();
            switch6 = new AntdUI.Switch();
            panel5 = new System.Windows.Forms.Panel();
            button30 = new AntdUI.Button();
            button31 = new AntdUI.Button();
            button36 = new AntdUI.Button();
            button37 = new AntdUI.Button();
            button32 = new AntdUI.Button();
            button33 = new AntdUI.Button();
            button34 = new AntdUI.Button();
            button35 = new AntdUI.Button();
            divider4 = new AntdUI.Divider();
            switch5 = new AntdUI.Switch();
            panel4 = new FlowLayoutPanel();
            button20 = new AntdUI.Button();
            button21 = new AntdUI.Button();
            button22 = new AntdUI.Button();
            button23 = new AntdUI.Button();
            button24 = new AntdUI.Button();
            button25 = new AntdUI.Button();
            button26 = new AntdUI.Button();
            divider3 = new AntdUI.Divider();
            switch4 = new AntdUI.Switch();
            panel3 = new FlowLayoutPanel();
            button50 = new AntdUI.Button();
            button51 = new AntdUI.Button();
            button52 = new AntdUI.Button();
            button53 = new AntdUI.Button();
            button54 = new AntdUI.Button();
            divider6 = new AntdUI.Divider();
            switch3 = new AntdUI.Switch();
            panel2 = new FlowLayoutPanel();
            button10 = new AntdUI.Button();
            button11 = new AntdUI.Button();
            button12 = new AntdUI.Button();
            button13 = new AntdUI.Button();
            button14 = new AntdUI.Button();
            button15 = new AntdUI.Button();
            button16 = new AntdUI.Button();
            divider2 = new AntdUI.Divider();
            switch2 = new AntdUI.Switch();
            panel1 = new FlowLayoutPanel();
            button1 = new AntdUI.Button();
            button2 = new AntdUI.Button();
            button3 = new AntdUI.Button();
            button4 = new AntdUI.Button();
            button5 = new AntdUI.Button();
            divider1 = new AntdUI.Divider();
            switch1 = new AntdUI.Switch();
            panel_main.SuspendLayout();
            panel6.SuspendLayout();
            panel_btns.SuspendLayout();
            panel7.SuspendLayout();
            divider5.SuspendLayout();
            panel5.SuspendLayout();
            divider4.SuspendLayout();
            panel4.SuspendLayout();
            divider3.SuspendLayout();
            panel3.SuspendLayout();
            divider6.SuspendLayout();
            panel2.SuspendLayout();
            divider2.SuspendLayout();
            panel1.SuspendLayout();
            divider1.SuspendLayout();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Description = "按钮用于开始一个即时操作。";
            header1.Dock = DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F);
            header1.LocalizationDescription = "Button.Description";
            header1.LocalizationText = "Button.Text";
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new Padding(0, 0, 0, 10);
            header1.Size = new Size(845, 74);
            header1.TabIndex = 0;
            header1.Text = "Button 按钮";
            header1.UseTitleFont = true;
            // 
            // panel_main
            // 
            panel_main.AutoScroll = true;
            panel_main.Controls.Add(panel6);
            panel_main.Controls.Add(divider5);
            panel_main.Controls.Add(panel5);
            panel_main.Controls.Add(divider4);
            panel_main.Controls.Add(panel4);
            panel_main.Controls.Add(divider3);
            panel_main.Controls.Add(panel3);
            panel_main.Controls.Add(divider6);
            panel_main.Controls.Add(panel2);
            panel_main.Controls.Add(divider2);
            panel_main.Controls.Add(panel1);
            panel_main.Controls.Add(divider1);
            panel_main.Dock = DockStyle.Fill;
            panel_main.Location = new Point(0, 74);
            panel_main.Name = "panel_main";
            panel_main.Size = new Size(845, 687);
            panel_main.TabIndex = 6;
            // 
            // panel6
            // 
            panel6.Controls.Add(panel_btns);
            panel6.Controls.Add(panel7);
            panel6.Dock = DockStyle.Top;
            panel6.Location = new Point(0, 524);
            panel6.Name = "panel6";
            panel6.Size = new Size(845, 100);
            panel6.TabIndex = 5;
            // 
            // panel_btns
            // 
            panel_btns.Controls.Add(btng3);
            panel_btns.Controls.Add(btng2);
            panel_btns.Controls.Add(btng1);
            panel_btns.Font = new Font("Microsoft YaHei UI", 10F);
            panel_btns.Location = new Point(4, 4);
            panel_btns.Name = "panel_btns";
            panel_btns.Padding = new Padding(4);
            panel_btns.Shadow = 18;
            panel_btns.Size = new Size(389, 80);
            panel_btns.TabIndex = 0;
            panel_btns.Text = "panel6";
            // 
            // btng3
            // 
            btng3.AutoSizeMode = AntdUI.TAutoSize.Width;
            btng3.BackColor = Color.FromArgb(217, 217, 217);
            btng3.BorderWidth = 2F;
            btng3.Dock = DockStyle.Left;
            btng3.JoinMode = AntdUI.TJoinMode.Right;
            btng3.Location = new Point(252, 22);
            btng3.Margin = new Padding(0);
            btng3.Name = "btng3";
            btng3.Size = new Size(115, 36);
            btng3.TabIndex = 2;
            btng3.Text = "Default Button";
            btng3.Click += Btns;
            // 
            // btng2
            // 
            btng2.AutoSizeMode = AntdUI.TAutoSize.Width;
            btng2.BackColor = Color.FromArgb(217, 217, 217);
            btng2.BorderWidth = 2F;
            btng2.Dock = DockStyle.Left;
            btng2.JoinMode = AntdUI.TJoinMode.LR;
            btng2.Location = new Point(137, 22);
            btng2.Margin = new Padding(0);
            btng2.Name = "btng2";
            btng2.Size = new Size(115, 36);
            btng2.TabIndex = 1;
            btng2.Text = "Default Button";
            btng2.Click += Btns;
            // 
            // btng1
            // 
            btng1.AutoSizeMode = AntdUI.TAutoSize.Width;
            btng1.BackColor = Color.FromArgb(217, 217, 217);
            btng1.BorderWidth = 2F;
            btng1.Dock = DockStyle.Left;
            btng1.JoinMode = AntdUI.TJoinMode.Left;
            btng1.Location = new Point(22, 22);
            btng1.Margin = new Padding(0);
            btng1.Name = "btng1";
            btng1.Size = new Size(115, 36);
            btng1.TabIndex = 0;
            btng1.Text = "Default Button";
            btng1.Click += Btns;
            // 
            // panel7
            // 
            panel7.Controls.Add(button41);
            panel7.Controls.Add(button40);
            panel7.Font = new Font("Microsoft YaHei UI", 14F);
            panel7.Location = new Point(440, 25);
            panel7.Name = "panel7";
            panel7.Size = new Size(140, 38);
            panel7.TabIndex = 1;
            // 
            // button41
            // 
            button41.BackActive = Color.FromArgb(17, 24, 39);
            button41.BackColor = Color.FromArgb(17, 24, 39);
            button41.BackHover = Color.FromArgb(17, 24, 39);
            button41.Dock = DockStyle.Fill;
            button41.JoinMode = AntdUI.TJoinMode.Right;
            button41.Location = new Point(46, 0);
            button41.Name = "button41";
            button41.Radius = 4;
            button41.Size = new Size(94, 38);
            button41.TabIndex = 1;
            button41.Text = "Button";
            button41.Type = AntdUI.TTypeMini.Primary;
            button41.WaveSize = 0;
            // 
            // button40
            // 
            button40.BackActive = Color.FromArgb(147, 51, 234);
            button40.BackColor = Color.FromArgb(168, 85, 247);
            button40.BackHover = Color.FromArgb(147, 51, 234);
            button40.Dock = DockStyle.Left;
            button40.IconSvg = "SearchOutlined";
            button40.JoinMode = AntdUI.TJoinMode.Left;
            button40.Location = new Point(0, 0);
            button40.Name = "button40";
            button40.Radius = 4;
            button40.Size = new Size(46, 38);
            button40.TabIndex = 0;
            button40.Type = AntdUI.TTypeMini.Primary;
            button40.WaveSize = 0;
            // 
            // divider5
            // 
            divider5.Controls.Add(switch6);
            divider5.Dock = DockStyle.Top;
            divider5.Font = new Font("Microsoft YaHei UI", 10F);
            divider5.LocalizationText = "Button.{id}";
            divider5.Location = new Point(0, 496);
            divider5.Name = "divider5";
            divider5.Orientation = AntdUI.TOrientation.Left;
            divider5.Size = new Size(845, 28);
            divider5.TabIndex = 4;
            divider5.Text = "组合按钮";
            // 
            // switch6
            // 
            switch6.Checked = true;
            switch6.Dock = DockStyle.Right;
            switch6.Location = new Point(803, 0);
            switch6.Name = "switch6";
            switch6.Size = new Size(42, 28);
            switch6.TabIndex = 1;
            switch6.CheckedChanged += switch6_CheckedChanged;
            // 
            // panel5
            // 
            panel5.Controls.Add(button30);
            panel5.Controls.Add(button31);
            panel5.Controls.Add(button36);
            panel5.Controls.Add(button37);
            panel5.Controls.Add(button32);
            panel5.Controls.Add(button33);
            panel5.Controls.Add(button34);
            panel5.Controls.Add(button35);
            panel5.Dock = DockStyle.Top;
            panel5.Location = new Point(0, 396);
            panel5.Name = "panel5";
            panel5.Size = new Size(845, 100);
            panel5.TabIndex = 4;
            // 
            // button30
            // 
            button30.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button30.BorderWidth = 1F;
            button30.IconSvg = "SearchOutlined";
            button30.Location = new Point(3, 3);
            button30.Name = "button30";
            button30.Size = new Size(100, 46);
            button30.TabIndex = 0;
            button30.Text = "Button";
            button30.Type = AntdUI.TTypeMini.Primary;
            button30.Click += Btn;
            // 
            // button31
            // 
            button31.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button31.BorderWidth = 1F;
            button31.IconPosition = AntdUI.TAlignMini.Right;
            button31.IconSvg = "SearchOutlined";
            button31.Location = new Point(113, 3);
            button31.Name = "button31";
            button31.Size = new Size(100, 46);
            button31.TabIndex = 1;
            button31.Text = "Button";
            button31.Type = AntdUI.TTypeMini.Primary;
            button31.Click += Btn;
            // 
            // button36
            // 
            button36.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button36.BorderWidth = 1F;
            button36.IconSvg = "SearchOutlined";
            button36.Location = new Point(3, 52);
            button36.Name = "button36";
            button36.Size = new Size(100, 46);
            button36.TabIndex = 6;
            button36.Text = "Button";
            button36.Click += Btn;
            // 
            // button37
            // 
            button37.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button37.BorderWidth = 1F;
            button37.IconPosition = AntdUI.TAlignMini.Right;
            button37.IconSvg = "SearchOutlined";
            button37.Location = new Point(113, 52);
            button37.Name = "button37";
            button37.Size = new Size(100, 46);
            button37.TabIndex = 7;
            button37.Text = "Button";
            button37.Click += Btn;
            // 
            // button32
            // 
            button32.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button32.BorderWidth = 1F;
            button32.IconPosition = AntdUI.TAlignMini.Top;
            button32.IconSvg = "SearchOutlined";
            button32.LoadingWaveCount = 4;
            button32.LoadingWaveSize = 6;
            button32.LoadingWaveVertical = true;
            button32.Location = new Point(223, 3);
            button32.Name = "button32";
            button32.Size = new Size(77, 69);
            button32.TabIndex = 2;
            button32.Text = "Button";
            button32.Type = AntdUI.TTypeMini.Primary;
            button32.Click += Btn;
            // 
            // button33
            // 
            button33.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button33.BorderWidth = 1F;
            button33.IconPosition = AntdUI.TAlignMini.Bottom;
            button33.IconSvg = "SearchOutlined";
            button33.LoadingWaveCount = 4;
            button33.LoadingWaveSize = 6;
            button33.LoadingWaveVertical = true;
            button33.Location = new Point(310, 3);
            button33.Name = "button33";
            button33.Size = new Size(77, 69);
            button33.TabIndex = 3;
            button33.Text = "Button";
            button33.Type = AntdUI.TTypeMini.Primary;
            button33.Click += Btn;
            // 
            // button34
            // 
            button34.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button34.BorderWidth = 1F;
            button34.IconPosition = AntdUI.TAlignMini.Top;
            button34.IconSvg = "SearchOutlined";
            button34.LoadingWaveCount = 4;
            button34.LoadingWaveSize = 6;
            button34.LoadingWaveVertical = true;
            button34.Location = new Point(397, 3);
            button34.Name = "button34";
            button34.Size = new Size(77, 69);
            button34.TabIndex = 4;
            button34.Text = "Button";
            button34.Click += Btn;
            // 
            // button35
            // 
            button35.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button35.BorderWidth = 1F;
            button35.IconPosition = AntdUI.TAlignMini.Bottom;
            button35.IconSvg = "SearchOutlined";
            button35.LoadingWaveCount = 4;
            button35.LoadingWaveSize = 6;
            button35.LoadingWaveVertical = true;
            button35.Location = new Point(484, 3);
            button35.Name = "button35";
            button35.Size = new Size(77, 69);
            button35.TabIndex = 5;
            button35.Text = "Button";
            button35.Click += Btn;
            // 
            // divider4
            // 
            divider4.Controls.Add(switch5);
            divider4.Dock = DockStyle.Top;
            divider4.Font = new Font("Microsoft YaHei UI", 10F);
            divider4.LocalizationText = "Button.{id}";
            divider4.Location = new Point(0, 368);
            divider4.Name = "divider4";
            divider4.Orientation = AntdUI.TOrientation.Left;
            divider4.Size = new Size(845, 28);
            divider4.TabIndex = 4;
            divider4.Text = "图标位置";
            // 
            // switch5
            // 
            switch5.Checked = true;
            switch5.Dock = DockStyle.Right;
            switch5.Location = new Point(803, 0);
            switch5.Name = "switch5";
            switch5.Size = new Size(42, 28);
            switch5.TabIndex = 1;
            switch5.CheckedChanged += switch5_CheckedChanged;
            // 
            // panel4
            // 
            panel4.Controls.Add(button20);
            panel4.Controls.Add(button21);
            panel4.Controls.Add(button22);
            panel4.Controls.Add(button23);
            panel4.Controls.Add(button24);
            panel4.Controls.Add(button25);
            panel4.Controls.Add(button26);
            panel4.Dock = DockStyle.Top;
            panel4.Location = new Point(0, 268);
            panel4.Name = "panel4";
            panel4.Size = new Size(845, 100);
            panel4.TabIndex = 3;
            // 
            // button20
            // 
            button20.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button20.BorderWidth = 1F;
            button20.Location = new Point(3, 3);
            button20.Name = "button20";
            button20.ShowArrow = true;
            button20.Size = new Size(100, 46);
            button20.TabIndex = 0;
            button20.Text = "Button";
            button20.Type = AntdUI.TTypeMini.Primary;
            button20.Click += Btn;
            // 
            // button21
            // 
            button21.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button21.BorderWidth = 1F;
            button21.IconSvg = "SearchOutlined";
            button21.Location = new Point(109, 3);
            button21.Name = "button21";
            button21.ShowArrow = true;
            button21.Size = new Size(119, 46);
            button21.TabIndex = 1;
            button21.Text = "Button";
            button21.Type = AntdUI.TTypeMini.Primary;
            button21.Click += Btn;
            // 
            // button22
            // 
            button22.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button22.BorderWidth = 1F;
            button22.IconSvg = "PoweroffOutlined";
            button22.IsLink = true;
            button22.Location = new Point(234, 3);
            button22.Name = "button22";
            button22.ShowArrow = true;
            button22.Size = new Size(119, 46);
            button22.TabIndex = 2;
            button22.Text = "Button";
            button22.Click += Btn;
            // 
            // button23
            // 
            button23.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button23.BorderWidth = 1F;
            button23.Ghost = true;
            button23.Location = new Point(359, 3);
            button23.Name = "button23";
            button23.ShowArrow = true;
            button23.Size = new Size(100, 46);
            button23.TabIndex = 3;
            button23.Text = "Button";
            button23.Type = AntdUI.TTypeMini.Primary;
            button23.Click += Btn;
            // 
            // button24
            // 
            button24.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button24.BorderWidth = 1F;
            button24.Location = new Point(465, 3);
            button24.Name = "button24";
            button24.ShowArrow = true;
            button24.Size = new Size(100, 46);
            button24.TabIndex = 4;
            button24.Text = "Button";
            button24.Type = AntdUI.TTypeMini.Error;
            button24.Click += Btn;
            // 
            // button25
            // 
            button25.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button25.BorderWidth = 1F;
            button25.Ghost = true;
            button25.IsLink = true;
            button25.Location = new Point(571, 3);
            button25.Name = "button25";
            button25.ShowArrow = true;
            button25.Size = new Size(100, 46);
            button25.TabIndex = 5;
            button25.Text = "Button";
            button25.Click += Btn;
            // 
            // button26
            // 
            button26.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button26.Ghost = true;
            button26.IsLink = true;
            button26.Location = new Point(677, 3);
            button26.Name = "button26";
            button26.ShowArrow = true;
            button26.Size = new Size(100, 46);
            button26.TabIndex = 6;
            button26.Text = "Button";
            button26.Click += Btn;
            // 
            // divider3
            // 
            divider3.Controls.Add(switch4);
            divider3.Dock = DockStyle.Top;
            divider3.Font = new Font("Microsoft YaHei UI", 10F);
            divider3.LocalizationText = "Button.{id}";
            divider3.Location = new Point(0, 240);
            divider3.Name = "divider3";
            divider3.Orientation = AntdUI.TOrientation.Left;
            divider3.Size = new Size(845, 28);
            divider3.TabIndex = 3;
            divider3.Text = "连接按钮";
            // 
            // switch4
            // 
            switch4.Checked = true;
            switch4.Dock = DockStyle.Right;
            switch4.Location = new Point(803, 0);
            switch4.Name = "switch4";
            switch4.Size = new Size(42, 28);
            switch4.TabIndex = 1;
            switch4.CheckedChanged += switch4_CheckedChanged;
            // 
            // panel3
            // 
            panel3.Controls.Add(button50);
            panel3.Controls.Add(button51);
            panel3.Controls.Add(button52);
            panel3.Controls.Add(button53);
            panel3.Controls.Add(button54);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(0, 188);
            panel3.Name = "panel3";
            panel3.Size = new Size(845, 52);
            panel3.TabIndex = 2;
            // 
            // button50
            // 
            button50.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button50.IconSvg = "DownloadOutlined";
            button50.LoadingWaveVertical = true;
            button50.Location = new Point(3, 3);
            button50.Name = "button50";
            button50.Size = new Size(46, 46);
            button50.TabIndex = 0;
            button50.Type = AntdUI.TTypeMini.Primary;
            button50.Click += Btn;
            // 
            // button51
            // 
            button51.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button51.IconSvg = "DownloadOutlined";
            button51.LoadingWaveVertical = true;
            button51.Location = new Point(55, 3);
            button51.Name = "button51";
            button51.Shape = AntdUI.TShape.Circle;
            button51.Size = new Size(46, 46);
            button51.TabIndex = 1;
            button51.Type = AntdUI.TTypeMini.Primary;
            button51.Click += Btn;
            // 
            // button52
            // 
            button52.AutoSizeMode = AntdUI.TAutoSize.Height;
            button52.IconSvg = "DownloadOutlined";
            button52.LoadingWaveVertical = true;
            button52.Location = new Point(107, 3);
            button52.Name = "button52";
            button52.Shape = AntdUI.TShape.Round;
            button52.Size = new Size(67, 46);
            button52.TabIndex = 2;
            button52.Type = AntdUI.TTypeMini.Primary;
            button52.Click += Btn;
            // 
            // button53
            // 
            button53.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button53.IconSvg = "DownloadOutlined";
            button53.Location = new Point(180, 3);
            button53.Name = "button53";
            button53.Shape = AntdUI.TShape.Round;
            button53.Size = new Size(127, 46);
            button53.TabIndex = 3;
            button53.Text = "Download";
            button53.Type = AntdUI.TTypeMini.Primary;
            button53.Click += Btn;
            // 
            // button54
            // 
            button54.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button54.IconSvg = "DownloadOutlined";
            button54.Location = new Point(313, 3);
            button54.Name = "button54";
            button54.Size = new Size(127, 46);
            button54.TabIndex = 4;
            button54.Text = "Download";
            button54.Type = AntdUI.TTypeMini.Primary;
            button54.Click += Btn;
            // 
            // divider6
            // 
            divider6.Controls.Add(switch3);
            divider6.Dock = DockStyle.Top;
            divider6.Font = new Font("Microsoft YaHei UI", 10F);
            divider6.LocalizationText = "Button.{id}";
            divider6.Location = new Point(0, 160);
            divider6.Name = "divider6";
            divider6.Orientation = AntdUI.TOrientation.Left;
            divider6.Size = new Size(845, 28);
            divider6.TabIndex = 2;
            divider6.Text = "按钮形状";
            // 
            // switch3
            // 
            switch3.Checked = true;
            switch3.Dock = DockStyle.Right;
            switch3.Location = new Point(803, 0);
            switch3.Name = "switch3";
            switch3.Size = new Size(42, 28);
            switch3.TabIndex = 1;
            switch3.CheckedChanged += switch3_CheckedChanged;
            // 
            // panel2
            // 
            panel2.Controls.Add(button10);
            panel2.Controls.Add(button11);
            panel2.Controls.Add(button12);
            panel2.Controls.Add(button13);
            panel2.Controls.Add(button14);
            panel2.Controls.Add(button15);
            panel2.Controls.Add(button16);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 108);
            panel2.Name = "panel2";
            panel2.Size = new Size(845, 52);
            panel2.TabIndex = 1;
            // 
            // button10
            // 
            button10.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button10.IconSvg = "PoweroffOutlined";
            button10.LoadingWaveVertical = true;
            button10.Location = new Point(3, 3);
            button10.Name = "button10";
            button10.Shape = AntdUI.TShape.Circle;
            button10.Size = new Size(46, 46);
            button10.TabIndex = 0;
            button10.Type = AntdUI.TTypeMini.Primary;
            button10.Click += Btn;
            // 
            // button11
            // 
            button11.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button11.IconSvg = "SearchOutlined";
            button11.LocalizationText = "Button.Search";
            button11.Location = new Point(55, 3);
            button11.Name = "button11";
            button11.Size = new Size(80, 46);
            button11.TabIndex = 1;
            button11.Text = "搜索";
            button11.Type = AntdUI.TTypeMini.Primary;
            button11.Click += Btn;
            // 
            // button12
            // 
            button12.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button12.BorderWidth = 1F;
            button12.IconSvg = "PoweroffOutlined";
            button12.LoadingWaveVertical = true;
            button12.Location = new Point(141, 3);
            button12.Name = "button12";
            button12.Shape = AntdUI.TShape.Circle;
            button12.Size = new Size(46, 46);
            button12.TabIndex = 2;
            button12.Click += Btn;
            // 
            // button13
            // 
            button13.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button13.BorderWidth = 1F;
            button13.IconSvg = "SearchOutlined";
            button13.LocalizationText = "Button.Search";
            button13.Location = new Point(193, 3);
            button13.Name = "button13";
            button13.Size = new Size(80, 46);
            button13.TabIndex = 3;
            button13.Text = "搜索";
            button13.Click += Btn;
            // 
            // button14
            // 
            button14.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button14.BorderWidth = 2F;
            button14.IconSvg = "SearchOutlined";
            button14.LoadingWaveVertical = true;
            button14.Location = new Point(279, 3);
            button14.Name = "button14";
            button14.Shape = AntdUI.TShape.Circle;
            button14.Size = new Size(46, 46);
            button14.TabIndex = 4;
            button14.Type = AntdUI.TTypeMini.Error;
            button14.Click += Btn;
            // 
            // button15
            // 
            button15.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button15.IconSvg = "SearchOutlined";
            button15.LocalizationText = "Button.Search";
            button15.Location = new Point(331, 3);
            button15.Name = "button15";
            button15.Size = new Size(80, 46);
            button15.TabIndex = 5;
            button15.Text = "搜索";
            button15.Type = AntdUI.TTypeMini.Error;
            button15.Click += Btn;
            // 
            // button16
            // 
            button16.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button16.BackExtend = "135, #6253E1, #04BEFE";
            button16.IconSvg = "SearchOutlined";
            button16.Location = new Point(417, 3);
            button16.Name = "button16";
            button16.Size = new Size(171, 46);
            button16.TabIndex = 6;
            button16.Text = "Gradient Button";
            button16.Type = AntdUI.TTypeMini.Primary;
            button16.Click += Btn;
            // 
            // divider2
            // 
            divider2.Controls.Add(switch2);
            divider2.Dock = DockStyle.Top;
            divider2.Font = new Font("Microsoft YaHei UI", 10F);
            divider2.LocalizationText = "Button.{id}";
            divider2.Location = new Point(0, 80);
            divider2.Name = "divider2";
            divider2.Orientation = AntdUI.TOrientation.Left;
            divider2.Size = new Size(845, 28);
            divider2.TabIndex = 1;
            divider2.Text = "按钮图标";
            // 
            // switch2
            // 
            switch2.Checked = true;
            switch2.Dock = DockStyle.Right;
            switch2.Location = new Point(803, 0);
            switch2.Name = "switch2";
            switch2.Size = new Size(42, 28);
            switch2.TabIndex = 1;
            switch2.CheckedChanged += switch2_CheckedChanged;
            // 
            // panel1
            // 
            panel1.Controls.Add(button1);
            panel1.Controls.Add(button2);
            panel1.Controls.Add(button3);
            panel1.Controls.Add(button4);
            panel1.Controls.Add(button5);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 28);
            panel1.Name = "panel1";
            panel1.Size = new Size(845, 52);
            panel1.TabIndex = 0;
            // 
            // button1
            // 
            button1.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button1.Location = new Point(3, 3);
            button1.Name = "button1";
            button1.Size = new Size(145, 46);
            button1.TabIndex = 0;
            button1.Text = "Primary Button";
            button1.Type = AntdUI.TTypeMini.Primary;
            button1.Click += Btn;
            // 
            // button2
            // 
            button2.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button2.BorderWidth = 1F;
            button2.Location = new Point(154, 3);
            button2.Name = "button2";
            button2.Size = new Size(142, 46);
            button2.TabIndex = 1;
            button2.Text = "Default Button";
            button2.Click += Btn;
            // 
            // button3
            // 
            button3.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button3.Ghost = true;
            button3.Location = new Point(302, 3);
            button3.Name = "button3";
            button3.Size = new Size(118, 46);
            button3.TabIndex = 2;
            button3.Text = "Text Button";
            button3.Click += Btn;
            // 
            // button4
            // 
            button4.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button4.Location = new Point(426, 3);
            button4.Name = "button4";
            button4.Size = new Size(86, 46);
            button4.TabIndex = 3;
            button4.Text = "Danger";
            button4.Type = AntdUI.TTypeMini.Error;
            button4.Click += Btn;
            // 
            // button5
            // 
            button5.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button5.BorderWidth = 2F;
            button5.Ghost = true;
            button5.Location = new Point(518, 3);
            button5.Name = "button5";
            button5.Size = new Size(146, 46);
            button5.TabIndex = 4;
            button5.Text = "Danger Default";
            button5.Type = AntdUI.TTypeMini.Error;
            button5.Click += Btn;
            // 
            // divider1
            // 
            divider1.Controls.Add(switch1);
            divider1.Dock = DockStyle.Top;
            divider1.Font = new Font("Microsoft YaHei UI", 10F);
            divider1.LocalizationText = "Button.{id}";
            divider1.Location = new Point(0, 0);
            divider1.Name = "divider1";
            divider1.Orientation = AntdUI.TOrientation.Left;
            divider1.Size = new Size(845, 28);
            divider1.TabIndex = 0;
            divider1.Text = "按钮类型";
            // 
            // switch1
            // 
            switch1.Checked = true;
            switch1.Dock = DockStyle.Right;
            switch1.Location = new Point(803, 0);
            switch1.Name = "switch1";
            switch1.Size = new Size(42, 28);
            switch1.TabIndex = 0;
            switch1.CheckedChanged += switch1_CheckedChanged;
            // 
            // Button
            // 
            Controls.Add(panel_main);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 12F);
            Name = "Button";
            Size = new Size(845, 761);
            panel_main.ResumeLayout(false);
            panel6.ResumeLayout(false);
            panel_btns.ResumeLayout(false);
            panel_btns.PerformLayout();
            panel7.ResumeLayout(false);
            divider5.ResumeLayout(false);
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            divider4.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            divider3.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            divider6.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            divider2.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            divider1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.PageHeader header1;
        private System.Windows.Forms.Panel panel_main;
        private AntdUI.Divider divider1;
        private FlowLayoutPanel panel1;
        private AntdUI.Button button1;
        private AntdUI.Button button2;
        private AntdUI.Button button3;
        private AntdUI.Button button4;
        private AntdUI.Button button5;
        private AntdUI.Divider divider2;
        private FlowLayoutPanel panel2;
        private AntdUI.Button button10;
        private AntdUI.Button button11;
        private AntdUI.Button button12;
        private AntdUI.Button button13;
        private AntdUI.Button button14;
        private AntdUI.Button button15;
        private AntdUI.Button button16;
        private AntdUI.Divider divider3;
        private FlowLayoutPanel panel4;
        private AntdUI.Button button20;
        private AntdUI.Button button21;
        private AntdUI.Button button22;
        private AntdUI.Button button23;
        private AntdUI.Button button24;
        private AntdUI.Button button25;
        private AntdUI.Button button26;
        private AntdUI.Divider divider4;
        private System.Windows.Forms.Panel panel5;
        private AntdUI.Button button30;
        private AntdUI.Button button31;
        private AntdUI.Button button32;
        private AntdUI.Button button33;
        private AntdUI.Button button36;
        private AntdUI.Button button37;
        private AntdUI.Button button34;
        private AntdUI.Button button35;
        private AntdUI.Divider divider5;
        private System.Windows.Forms.Panel panel6;
        private AntdUI.Panel panel_btns;
        private AntdUI.Button btng3;
        private AntdUI.Button btng2;
        private AntdUI.Button btng1;
        private System.Windows.Forms.Panel panel7;
        private AntdUI.Button button41;
        private AntdUI.Button button40;
        private AntdUI.Divider divider6;
        private FlowLayoutPanel panel3;
        private AntdUI.Button button50;
        private AntdUI.Button button51;
        private AntdUI.Button button52;
        private AntdUI.Button button53;
        private AntdUI.Button button54;
        private AntdUI.Switch switch1;
        private AntdUI.Switch switch5;
        private AntdUI.Switch switch4;
        private AntdUI.Switch switch3;
        private AntdUI.Switch switch2;
        private AntdUI.Switch switch6;
    }
}