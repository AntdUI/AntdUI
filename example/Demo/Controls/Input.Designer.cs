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
    partial class Input
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
            input1 = new AntdUI.Input();
            header1 = new AntdUI.PageHeader();
            panel1 = new System.Windows.Forms.Panel();
            panel3 = new System.Windows.Forms.Panel();
            label1 = new AntdUI.Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            ic4 = new AntdUI.Input();
            ic3 = new AntdUI.Input();
            ic2 = new AntdUI.Input();
            ic1 = new AntdUI.Input();
            panel5 = new AntdUI.Panel();
            input10 = new AntdUI.Input();
            button1 = new AntdUI.Button();
            panel6 = new System.Windows.Forms.Panel();
            input18 = new AntdUI.Input();
            button2 = new AntdUI.Button();
            panel4 = new AntdUI.Panel();
            input19 = new AntdUI.Input();
            button3 = new AntdUI.Button();
            divider6 = new AntdUI.Divider();
            flowLayoutPanel5 = new FlowLayoutPanel();
            input13 = new AntdUI.Input();
            input14 = new AntdUI.Input();
            divider5 = new AntdUI.Divider();
            flowLayoutPanel4 = new FlowLayoutPanel();
            input11 = new AntdUI.Input();
            input12 = new AntdUI.Input();
            divider4 = new AntdUI.Divider();
            flowLayoutPanel3 = new FlowLayoutPanel();
            input9 = new AntdUI.Input();
            input17 = new AntdUI.Input();
            input15 = new AntdUI.Input();
            input16 = new AntdUI.Input();
            divider3 = new AntdUI.Divider();
            flowLayoutPanel2 = new FlowLayoutPanel();
            input5 = new AntdUI.Input();
            input6 = new AntdUI.Input();
            input7 = new AntdUI.Input();
            input8 = new AntdUI.Input();
            divider2 = new AntdUI.Divider();
            flowLayoutPanel1 = new FlowLayoutPanel();
            input2 = new AntdUI.Input();
            input3 = new AntdUI.Input();
            input4 = new AntdUI.Input();
            divider1 = new AntdUI.Divider();
            panel1.SuspendLayout();
            panel3.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            panel5.SuspendLayout();
            panel6.SuspendLayout();
            panel4.SuspendLayout();
            flowLayoutPanel5.SuspendLayout();
            flowLayoutPanel4.SuspendLayout();
            flowLayoutPanel3.SuspendLayout();
            flowLayoutPanel2.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // input1
            // 
            input1.Location = new Point(3, 3);
            input1.Name = "input1";
            input1.Size = new Size(260, 44);
            input1.TabIndex = 0;
            // 
            // header1
            // 
            header1.Description = "通过鼠标或键盘输入内容，是最基础的表单域的包装。";
            header1.Dock = DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F);
            header1.LocalizationDescription = "Input.Description";
            header1.LocalizationText = "Input.Text";
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new Padding(0, 0, 0, 10);
            header1.Size = new Size(555, 74);
            header1.TabIndex = 0;
            header1.Text = "Input 输入框";
            header1.UseTitleFont = true;
            // 
            // panel1
            // 
            panel1.AutoScroll = true;
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(divider6);
            panel1.Controls.Add(flowLayoutPanel5);
            panel1.Controls.Add(divider5);
            panel1.Controls.Add(flowLayoutPanel4);
            panel1.Controls.Add(divider4);
            panel1.Controls.Add(flowLayoutPanel3);
            panel1.Controls.Add(divider3);
            panel1.Controls.Add(flowLayoutPanel2);
            panel1.Controls.Add(divider2);
            panel1.Controls.Add(flowLayoutPanel1);
            panel1.Controls.Add(divider1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 74);
            panel1.Name = "panel1";
            panel1.Size = new Size(555, 480);
            panel1.TabIndex = 6;
            // 
            // panel3
            // 
            panel3.Controls.Add(label1);
            panel3.Controls.Add(tableLayoutPanel1);
            panel3.Controls.Add(panel5);
            panel3.Controls.Add(panel6);
            panel3.Controls.Add(panel4);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(0, 679);
            panel3.Name = "panel3";
            panel3.Size = new Size(538, 148);
            panel3.TabIndex = 12;
            // 
            // label1
            // 
            label1.AutoSizeMode = AntdUI.TAutoSize.Width;
            label1.LocalizationText = "Input.Code";
            label1.Location = new Point(262, 71);
            label1.Name = "label1";
            label1.Size = new Size(96, 23);
            label1.TabIndex = 0;
            label1.Text = "请输入验证码";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.Controls.Add(ic4, 3, 0);
            tableLayoutPanel1.Controls.Add(ic3, 2, 0);
            tableLayoutPanel1.Controls.Add(ic2, 1, 0);
            tableLayoutPanel1.Controls.Add(ic1, 0, 0);
            tableLayoutPanel1.Location = new Point(253, 97);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(232, 44);
            tableLayoutPanel1.TabIndex = 3;
            // 
            // ic4
            // 
            ic4.Dock = DockStyle.Fill;
            ic4.Location = new Point(180, 0);
            ic4.Margin = new Padding(6, 0, 6, 0);
            ic4.Name = "ic4";
            ic4.Size = new Size(46, 44);
            ic4.TabIndex = 4;
            ic4.TextAlign = HorizontalAlignment.Center;
            ic4.VerifyKeyboard += VerifyKeyboard;
            ic4.TextChanged += CodeTextChanged;
            ic4.KeyPress += CodeKeyPress;
            // 
            // ic3
            // 
            ic3.Dock = DockStyle.Fill;
            ic3.Location = new Point(122, 0);
            ic3.Margin = new Padding(6, 0, 6, 0);
            ic3.Name = "ic3";
            ic3.Size = new Size(46, 44);
            ic3.TabIndex = 3;
            ic3.TextAlign = HorizontalAlignment.Center;
            ic3.VerifyKeyboard += VerifyKeyboard;
            ic3.TextChanged += CodeTextChanged;
            ic3.KeyPress += CodeKeyPress;
            // 
            // ic2
            // 
            ic2.Dock = DockStyle.Fill;
            ic2.Location = new Point(64, 0);
            ic2.Margin = new Padding(6, 0, 6, 0);
            ic2.Name = "ic2";
            ic2.Size = new Size(46, 44);
            ic2.TabIndex = 2;
            ic2.TextAlign = HorizontalAlignment.Center;
            ic2.VerifyKeyboard += VerifyKeyboard;
            ic2.TextChanged += CodeTextChanged;
            ic2.KeyPress += CodeKeyPress;
            // 
            // ic1
            // 
            ic1.Dock = DockStyle.Fill;
            ic1.Location = new Point(6, 0);
            ic1.Margin = new Padding(6, 0, 6, 0);
            ic1.Name = "ic1";
            ic1.Size = new Size(46, 44);
            ic1.TabIndex = 1;
            ic1.TextAlign = HorizontalAlignment.Center;
            ic1.VerifyKeyboard += VerifyKeyboard;
            ic1.TextChanged += CodeTextChanged;
            ic1.KeyPress += CodeKeyPress;
            // 
            // panel5
            // 
            panel5.Controls.Add(input10);
            panel5.Controls.Add(button1);
            panel5.Location = new Point(253, 16);
            panel5.Name = "panel5";
            panel5.Size = new Size(220, 50);
            panel5.TabIndex = 1;
            panel5.Text = "panel4";
            // 
            // input10
            // 
            input10.Dock = DockStyle.Fill;
            input10.JoinMode = AntdUI.TJoinMode.Right;
            input10.LocalizationPlaceholderText = "Input.{id}";
            input10.Location = new Point(59, 0);
            input10.Name = "input10";
            input10.PlaceholderText = "输入点什么搜索";
            input10.Size = new Size(161, 50);
            input10.TabIndex = 0;
            // 
            // button1
            // 
            button1.AutoSizeMode = AntdUI.TAutoSize.Width;
            button1.Dock = DockStyle.Left;
            button1.JoinMode = AntdUI.TJoinMode.Left;
            button1.LocalizationText = "Input.Search";
            button1.Location = new Point(0, 0);
            button1.Name = "button1";
            button1.Size = new Size(59, 50);
            button1.TabIndex = 1;
            button1.Text = "搜索";
            button1.Type = AntdUI.TTypeMini.Primary;
            button1.Click += Btn;
            // 
            // panel6
            // 
            panel6.Controls.Add(input18);
            panel6.Controls.Add(button2);
            panel6.Location = new Point(18, 76);
            panel6.Name = "panel6";
            panel6.Size = new Size(220, 46);
            panel6.TabIndex = 2;
            panel6.Text = "panel4";
            // 
            // input18
            // 
            input18.Dock = DockStyle.Fill;
            input18.JoinMode = AntdUI.TJoinMode.Left;
            input18.LocalizationPlaceholderText = "Input.{id}";
            input18.Location = new Point(0, 0);
            input18.Name = "input18";
            input18.PlaceholderText = "输入点什么搜索";
            input18.Size = new Size(170, 46);
            input18.TabIndex = 0;
            // 
            // button2
            // 
            button2.Dock = DockStyle.Right;
            button2.IconSvg = "SearchOutlined";
            button2.JoinMode = AntdUI.TJoinMode.Right;
            button2.Location = new Point(170, 0);
            button2.Name = "button2";
            button2.Size = new Size(50, 46);
            button2.TabIndex = 1;
            button2.Type = AntdUI.TTypeMini.Primary;
            button2.Click += Btn;
            // 
            // panel4
            // 
            panel4.Controls.Add(input19);
            panel4.Controls.Add(button3);
            panel4.Location = new Point(18, 16);
            panel4.Name = "panel4";
            panel4.Size = new Size(220, 50);
            panel4.TabIndex = 0;
            panel4.Text = "panel4";
            // 
            // input19
            // 
            input19.Dock = DockStyle.Fill;
            input19.JoinMode = AntdUI.TJoinMode.Left;
            input19.LocalizationPlaceholderText = "Input.{id}";
            input19.Location = new Point(0, 0);
            input19.Name = "input19";
            input19.PlaceholderText = "输入点什么搜索";
            input19.Size = new Size(161, 50);
            input19.TabIndex = 0;
            // 
            // button3
            // 
            button3.AutoSizeMode = AntdUI.TAutoSize.Width;
            button3.Dock = DockStyle.Right;
            button3.JoinMode = AntdUI.TJoinMode.Right;
            button3.LocalizationText = "Input.Search";
            button3.Location = new Point(161, 0);
            button3.Name = "button3";
            button3.Size = new Size(59, 50);
            button3.TabIndex = 1;
            button3.Text = "搜索";
            button3.Type = AntdUI.TTypeMini.Primary;
            button3.Click += Btn;
            // 
            // divider6
            // 
            divider6.Dock = DockStyle.Top;
            divider6.Font = new Font("Microsoft YaHei UI", 10F);
            divider6.LocalizationText = "Input.{id}";
            divider6.Location = new Point(0, 651);
            divider6.Name = "divider6";
            divider6.Orientation = AntdUI.TOrientation.Left;
            divider6.Size = new Size(538, 28);
            divider6.TabIndex = 11;
            divider6.TabStop = false;
            divider6.Text = "组合";
            // 
            // flowLayoutPanel5
            // 
            flowLayoutPanel5.Controls.Add(input13);
            flowLayoutPanel5.Controls.Add(input14);
            flowLayoutPanel5.Dock = DockStyle.Top;
            flowLayoutPanel5.Location = new Point(0, 592);
            flowLayoutPanel5.Name = "flowLayoutPanel5";
            flowLayoutPanel5.Size = new Size(538, 59);
            flowLayoutPanel5.TabIndex = 10;
            // 
            // input13
            // 
            input13.LocalizationPlaceholderText = "Input.{id}";
            input13.Location = new Point(3, 3);
            input13.Name = "input13";
            input13.PlaceholderText = "请输入密码";
            input13.Size = new Size(260, 44);
            input13.TabIndex = 1;
            input13.UseSystemPasswordChar = true;
            // 
            // input14
            // 
            input14.AllowClear = true;
            input14.LocalizationPlaceholderText = "Input.{id}";
            input14.Location = new Point(269, 3);
            input14.Name = "input14";
            input14.PlaceholderText = "清除按钮";
            input14.Size = new Size(260, 44);
            input14.TabIndex = 0;
            input14.UseSystemPasswordChar = true;
            // 
            // divider5
            // 
            divider5.Dock = DockStyle.Top;
            divider5.Font = new Font("Microsoft YaHei UI", 10F);
            divider5.LocalizationText = "Input.{id}";
            divider5.Location = new Point(0, 564);
            divider5.Name = "divider5";
            divider5.Orientation = AntdUI.TOrientation.Left;
            divider5.Size = new Size(538, 28);
            divider5.TabIndex = 9;
            divider5.TabStop = false;
            divider5.Text = "密码框";
            // 
            // flowLayoutPanel4
            // 
            flowLayoutPanel4.Controls.Add(input11);
            flowLayoutPanel4.Controls.Add(input12);
            flowLayoutPanel4.Dock = DockStyle.Top;
            flowLayoutPanel4.Location = new Point(0, 425);
            flowLayoutPanel4.Name = "flowLayoutPanel4";
            flowLayoutPanel4.Size = new Size(538, 139);
            flowLayoutPanel4.TabIndex = 8;
            // 
            // input11
            // 
            input11.Location = new Point(3, 3);
            input11.Multiline = true;
            input11.Name = "input11";
            input11.Size = new Size(260, 130);
            input11.TabIndex = 1;
            // 
            // input12
            // 
            input12.AutoScroll = true;
            input12.Location = new Point(269, 3);
            input12.Multiline = true;
            input12.Name = "input12";
            input12.Size = new Size(260, 130);
            input12.TabIndex = 0;
            // 
            // divider4
            // 
            divider4.Dock = DockStyle.Top;
            divider4.Font = new Font("Microsoft YaHei UI", 10F);
            divider4.LocalizationText = "Input.{id}";
            divider4.Location = new Point(0, 397);
            divider4.Name = "divider4";
            divider4.Orientation = AntdUI.TOrientation.Left;
            divider4.Size = new Size(538, 28);
            divider4.TabIndex = 7;
            divider4.TabStop = false;
            divider4.Text = "多行文本";
            // 
            // flowLayoutPanel3
            // 
            flowLayoutPanel3.Controls.Add(input9);
            flowLayoutPanel3.Controls.Add(input17);
            flowLayoutPanel3.Controls.Add(input15);
            flowLayoutPanel3.Controls.Add(input16);
            flowLayoutPanel3.Dock = DockStyle.Top;
            flowLayoutPanel3.Location = new Point(0, 294);
            flowLayoutPanel3.Name = "flowLayoutPanel3";
            flowLayoutPanel3.Size = new Size(538, 103);
            flowLayoutPanel3.TabIndex = 6;
            // 
            // input9
            // 
            input9.AllowClear = true;
            input9.BackColor = Color.Transparent;
            input9.Location = new Point(3, 3);
            input9.Name = "input9";
            input9.PlaceholderText = "Underlined";
            input9.Radius = 0;
            input9.Size = new Size(260, 36);
            input9.TabIndex = 1;
            input9.Variant = AntdUI.TVariant.Underlined;
            input9.WaveSize = 0;
            // 
            // input17
            // 
            input17.AllowClear = true;
            input17.BackColor = Color.Transparent;
            input17.BorderWidth = 0F;
            input17.Location = new Point(269, 3);
            input17.Name = "input17";
            input17.PlaceholderText = "Borderless";
            input17.Radius = 0;
            input17.Size = new Size(260, 36);
            input17.TabIndex = 1;
            input17.WaveSize = 0;
            // 
            // input15
            // 
            input15.BackExtend = "-20deg, #e9defa, #fbfcdb";
            input15.BorderWidth = 0F;
            input15.Location = new Point(3, 45);
            input15.Name = "input15";
            input15.PlaceholderColorExtend = "120deg, #f093fb, #f5576c";
            input15.PlaceholderText = "Filled";
            input15.Size = new Size(260, 44);
            input15.TabIndex = 2;
            // 
            // input16
            // 
            input16.BackExtend = "120deg, #f093fb, #f5576c";
            input16.BorderWidth = 0F;
            input16.Location = new Point(269, 45);
            input16.Name = "input16";
            input16.PlaceholderColorExtend = "#dfe9f3, #ffffff";
            input16.PlaceholderText = "Filled";
            input16.Size = new Size(260, 44);
            input16.TabIndex = 3;
            // 
            // divider3
            // 
            divider3.Dock = DockStyle.Top;
            divider3.Font = new Font("Microsoft YaHei UI", 10F);
            divider3.LocalizationText = "Input.{id}";
            divider3.Location = new Point(0, 266);
            divider3.Name = "divider3";
            divider3.Orientation = AntdUI.TOrientation.Left;
            divider3.Size = new Size(538, 28);
            divider3.TabIndex = 5;
            divider3.TabStop = false;
            divider3.Text = "形态变体";
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.Controls.Add(input5);
            flowLayoutPanel2.Controls.Add(input6);
            flowLayoutPanel2.Controls.Add(input7);
            flowLayoutPanel2.Controls.Add(input8);
            flowLayoutPanel2.Dock = DockStyle.Top;
            flowLayoutPanel2.Location = new Point(0, 163);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new Size(538, 103);
            flowLayoutPanel2.TabIndex = 4;
            // 
            // input5
            // 
            input5.LocalizationPlaceholderText = "Input.{id}";
            input5.Location = new Point(3, 3);
            input5.Name = "input5";
            input5.PlaceholderText = "请输入";
            input5.PrefixText = "https://";
            input5.Size = new Size(260, 44);
            input5.SuffixText = ".com";
            input5.TabIndex = 0;
            // 
            // input6
            // 
            input6.Location = new Point(269, 3);
            input6.Name = "input6";
            input6.PrefixSvg = "SearchOutlined";
            input6.Size = new Size(260, 44);
            input6.TabIndex = 1;
            // 
            // input7
            // 
            input7.Location = new Point(3, 53);
            input7.Name = "input7";
            input7.PrefixSvg = "SearchOutlined";
            input7.Size = new Size(260, 44);
            input7.SuffixFore = Color.FromArgb(252, 136, 72);
            input7.SuffixSvg = "TaobaoCircleFilled";
            input7.TabIndex = 2;
            // 
            // input8
            // 
            input8.BorderActive = Color.Red;
            input8.BorderColor = Color.FromArgb(255, 80, 0);
            input8.BorderHover = Color.FromArgb(255, 120, 80);
            input8.BorderWidth = 2F;
            input8.LocalizationSuffixText = "Input.Tao";
            input8.Location = new Point(269, 53);
            input8.Name = "input8";
            input8.PrefixFore = Color.FromArgb(255, 80, 0);
            input8.PrefixSvg = "TaobaoCircleFilled";
            input8.Size = new Size(260, 44);
            input8.SuffixFore = Color.FromArgb(252, 136, 72);
            input8.SuffixText = "淘，我喜欢";
            input8.TabIndex = 3;
            // 
            // divider2
            // 
            divider2.Dock = DockStyle.Top;
            divider2.Font = new Font("Microsoft YaHei UI", 10F);
            divider2.LocalizationText = "Input.{id}";
            divider2.Location = new Point(0, 135);
            divider2.Name = "divider2";
            divider2.Orientation = AntdUI.TOrientation.Left;
            divider2.Size = new Size(538, 28);
            divider2.TabIndex = 3;
            divider2.TabStop = false;
            divider2.Text = "前后缀";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(input1);
            flowLayoutPanel1.Controls.Add(input2);
            flowLayoutPanel1.Controls.Add(input3);
            flowLayoutPanel1.Controls.Add(input4);
            flowLayoutPanel1.Dock = DockStyle.Top;
            flowLayoutPanel1.Location = new Point(0, 28);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(538, 107);
            flowLayoutPanel1.TabIndex = 2;
            // 
            // input2
            // 
            input2.AllowClear = true;
            input2.LocalizationPlaceholderText = "Input.{id}";
            input2.Location = new Point(269, 3);
            input2.Name = "input2";
            input2.PlaceholderText = "清除按钮";
            input2.Size = new Size(260, 44);
            input2.TabIndex = 1;
            // 
            // input3
            // 
            input3.LocalizationPlaceholderText = "Input.{id}";
            input3.Location = new Point(3, 53);
            input3.Name = "input3";
            input3.PlaceholderText = "圆角边框";
            input3.Round = true;
            input3.Size = new Size(260, 44);
            input3.TabIndex = 2;
            // 
            // input4
            // 
            input4.BorderWidth = 2F;
            input4.LocalizationPlaceholderText = "Input.{id}";
            input4.Location = new Point(269, 53);
            input4.Name = "input4";
            input4.PlaceholderText = "加粗边框";
            input4.Size = new Size(260, 44);
            input4.TabIndex = 3;
            // 
            // divider1
            // 
            divider1.Dock = DockStyle.Top;
            divider1.Font = new Font("Microsoft YaHei UI", 10F);
            divider1.LocalizationText = "Input.{id}";
            divider1.Location = new Point(0, 0);
            divider1.Name = "divider1";
            divider1.Orientation = AntdUI.TOrientation.Left;
            divider1.Size = new Size(538, 28);
            divider1.TabIndex = 1;
            divider1.TabStop = false;
            divider1.Text = "常规";
            // 
            // Input
            // 
            Controls.Add(panel1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 12F);
            Name = "Input";
            Size = new Size(555, 554);
            panel1.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            panel6.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            flowLayoutPanel5.ResumeLayout(false);
            flowLayoutPanel4.ResumeLayout(false);
            flowLayoutPanel3.ResumeLayout(false);
            flowLayoutPanel2.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.Input input1;
        private AntdUI.PageHeader header1;
        private System.Windows.Forms.Panel panel1;
        private AntdUI.Divider divider1;
        private AntdUI.Input input7;
        private AntdUI.Input input6;
        private AntdUI.Input input5;
        private AntdUI.Input input4;
        private AntdUI.Input input3;
        private AntdUI.Input input2;
        private AntdUI.Input input8;
        private AntdUI.Input input9;
        private AntdUI.Input input12;
        private AntdUI.Input input11;
        private AntdUI.Input input14;
        private AntdUI.Input input13;
        private FlowLayoutPanel flowLayoutPanel5;
        private FlowLayoutPanel flowLayoutPanel4;
        private FlowLayoutPanel flowLayoutPanel3;
        private FlowLayoutPanel flowLayoutPanel2;
        private FlowLayoutPanel flowLayoutPanel1;
        private AntdUI.Divider divider5;
        private AntdUI.Divider divider4;
        private AntdUI.Divider divider3;
        private AntdUI.Divider divider2;
        private AntdUI.Input input15;
        private AntdUI.Input input16;
        private AntdUI.Input input17;
        private AntdUI.Divider divider6;
        private System.Windows.Forms.Panel panel3;
        private AntdUI.Panel panel5;
        private AntdUI.Input input10;
        private AntdUI.Button button1;
        private System.Windows.Forms.Panel panel6;
        private AntdUI.Input input18;
        private AntdUI.Button button2;
        private AntdUI.Panel panel4;
        private AntdUI.Input input19;
        private AntdUI.Button button3;
        private TableLayoutPanel tableLayoutPanel1;
        private AntdUI.Input ic4;
        private AntdUI.Input ic3;
        private AntdUI.Input ic2;
        private AntdUI.Input ic1;
        private AntdUI.  Label label1;
    }
}