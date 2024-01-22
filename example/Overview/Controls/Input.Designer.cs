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

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            input1 = new AntdUI.Input();
            input2 = new AntdUI.Input();
            input3 = new AntdUI.Input();
            input4 = new AntdUI.Input();
            input5 = new AntdUI.Input();
            input6 = new AntdUI.Input();
            header1 = new AntdUI.Header();
            panel1 = new System.Windows.Forms.Panel();
            panel3 = new System.Windows.Forms.Panel();
            panel5 = new AntdUI.Panel();
            input8 = new AntdUI.Input();
            button1 = new AntdUI.Button();
            panel6 = new System.Windows.Forms.Panel();
            input9 = new AntdUI.Input();
            button2 = new AntdUI.Button();
            panel4 = new AntdUI.Panel();
            input7 = new AntdUI.Input();
            button3 = new AntdUI.Button();
            divider2 = new AntdUI.Divider();
            panel2 = new System.Windows.Forms.Panel();
            divider1 = new AntdUI.Divider();
            panel1.SuspendLayout();
            panel3.SuspendLayout();
            panel5.SuspendLayout();
            panel6.SuspendLayout();
            panel4.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // input1
            // 
            input1.Location = new Point(18, 6);
            input1.Name = "input1";
            input1.Size = new Size(220, 44);
            input1.TabIndex = 0;
            input1.Text = "input1";
            // 
            // input2
            // 
            input2.Location = new Point(18, 54);
            input2.Name = "input2";
            input2.Size = new Size(220, 44);
            input2.TabIndex = 2;
            input2.Text = "input1";
            // 
            // input3
            // 
            input3.Location = new Point(244, 6);
            input3.Name = "input3";
            input3.PlaceholderText = "请输入账号";
            input3.Round = true;
            input3.Size = new Size(220, 44);
            input3.TabIndex = 1;
            // 
            // input4
            // 
            input4.Location = new Point(244, 54);
            input4.Name = "input4";
            input4.PasswordChar = '●';
            input4.Round = true;
            input4.Size = new Size(220, 44);
            input4.TabIndex = 3;
            input4.Text = "321";
            // 
            // input5
            // 
            input5.ImageSvg = Properties.Resources.icon_poweroff;
            input5.Location = new Point(18, 101);
            input5.Name = "input5";
            input5.Radius = 10;
            input5.Size = new Size(220, 44);
            input5.TabIndex = 4;
            input5.Text = "321";
            // 
            // input6
            // 
            input6.BorderWidth = 2F;
            input6.Location = new Point(244, 102);
            input6.Name = "input6";
            input6.Size = new Size(220, 44);
            input6.TabIndex = 5;
            input6.Text = "input1";
            // 
            // header1
            // 
            header1.Dock = DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new Padding(6);
            header1.Size = new Size(555, 79);
            header1.TabIndex = 0;
            header1.TabStop = false;
            header1.Text = "Input 输入框";
            header1.TextDesc = "通过鼠标或键盘输入内容，是最基础的表单域的包装。";
            // 
            // panel1
            // 
            panel1.AutoScroll = true;
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(divider2);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(divider1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 79);
            panel1.Name = "panel1";
            panel1.Size = new Size(555, 475);
            panel1.TabIndex = 6;
            // 
            // panel3
            // 
            panel3.Controls.Add(panel5);
            panel3.Controls.Add(panel6);
            panel3.Controls.Add(panel4);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(0, 206);
            panel3.Name = "panel3";
            panel3.Size = new Size(555, 139);
            panel3.TabIndex = 1;
            // 
            // panel5
            // 
            panel5.Controls.Add(input8);
            panel5.Controls.Add(button1);
            panel5.Location = new Point(253, 16);
            panel5.Name = "panel5";
            panel5.Shadow = 0;
            panel5.Size = new Size(220, 50);
            panel5.TabIndex = 1;
            panel5.Text = "panel4";
            // 
            // input8
            // 
            input8.Dock = DockStyle.Fill;
            input8.JoinLeft = true;
            input8.Location = new Point(59, 0);
            input8.Margins = 6;
            input8.Name = "input8";
            input8.PlaceholderText = "输入点什么搜索";
            input8.Size = new Size(161, 50);
            input8.TabIndex = 0;
            // 
            // button1
            // 
            button1.AutoSize = AntdUI.TAutoSize.Width;
            button1.Dock = DockStyle.Left;
            button1.JoinRight = true;
            button1.Location = new Point(0, 0);
            button1.Margins = 6;
            button1.Name = "button1";
            button1.Size = new Size(59, 50);
            button1.TabIndex = 1;
            button1.Text = "搜索";
            button1.Type = AntdUI.TTypeMini.Primary;
            button1.Click += Btn;
            // 
            // panel6
            // 
            panel6.Controls.Add(input9);
            panel6.Controls.Add(button2);
            panel6.Location = new Point(18, 76);
            panel6.Name = "panel6";
            panel6.Size = new Size(220, 46);
            panel6.TabIndex = 2;
            panel6.Text = "panel4";
            // 
            // input9
            // 
            input9.Dock = DockStyle.Fill;
            input9.JoinRight = true;
            input9.Location = new Point(0, 0);
            input9.Margins = 6;
            input9.Name = "input9";
            input9.PlaceholderText = "输入点什么搜索";
            input9.Size = new Size(170, 46);
            input9.TabIndex = 0;
            // 
            // button2
            // 
            button2.BackColor = Color.Transparent;
            button2.Dock = DockStyle.Right;
            button2.ImageSvg = Properties.Resources.icon_search;
            button2.JoinLeft = true;
            button2.Location = new Point(170, 0);
            button2.Margins = 6;
            button2.Name = "button2";
            button2.Size = new Size(50, 46);
            button2.TabIndex = 1;
            button2.Type = AntdUI.TTypeMini.Primary;
            button2.Click += Btn;
            // 
            // panel4
            // 
            panel4.Controls.Add(input7);
            panel4.Controls.Add(button3);
            panel4.Location = new Point(18, 16);
            panel4.Name = "panel4";
            panel4.Shadow = 0;
            panel4.Size = new Size(220, 50);
            panel4.TabIndex = 0;
            panel4.Text = "panel4";
            // 
            // input7
            // 
            input7.Dock = DockStyle.Fill;
            input7.JoinRight = true;
            input7.Location = new Point(0, 0);
            input7.Margins = 6;
            input7.Name = "input7";
            input7.PlaceholderText = "输入点什么搜索";
            input7.Size = new Size(161, 50);
            input7.TabIndex = 0;
            // 
            // button3
            // 
            button3.AutoSize = AntdUI.TAutoSize.Width;
            button3.Dock = DockStyle.Right;
            button3.JoinLeft = true;
            button3.Location = new Point(161, 0);
            button3.Margins = 6;
            button3.Name = "button3";
            button3.Size = new Size(59, 50);
            button3.TabIndex = 1;
            button3.Text = "搜索";
            button3.Type = AntdUI.TTypeMini.Primary;
            button3.Click += Btn;
            // 
            // divider2
            // 
            divider2.Dock = DockStyle.Top;
            divider2.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            divider2.Location = new Point(0, 184);
            divider2.Name = "divider2";
            divider2.Orientation = AntdUI.TOrientation.Left;
            divider2.Size = new Size(555, 22);
            divider2.TabIndex = 1;
            divider2.TabStop = false;
            divider2.Text = "组合";
            // 
            // panel2
            // 
            panel2.Controls.Add(input6);
            panel2.Controls.Add(input5);
            panel2.Controls.Add(input4);
            panel2.Controls.Add(input2);
            panel2.Controls.Add(input3);
            panel2.Controls.Add(input1);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 22);
            panel2.Name = "panel2";
            panel2.Size = new Size(555, 162);
            panel2.TabIndex = 0;
            // 
            // divider1
            // 
            divider1.Dock = DockStyle.Top;
            divider1.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            divider1.Location = new Point(0, 0);
            divider1.Name = "divider1";
            divider1.Orientation = AntdUI.TOrientation.Left;
            divider1.Size = new Size(555, 22);
            divider1.TabIndex = 1;
            divider1.TabStop = false;
            divider1.Text = "常规";
            // 
            // Input
            // 
            Controls.Add(panel1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            Name = "Input";
            Size = new Size(555, 554);
            panel1.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel5.ResumeLayout(false);
            panel6.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.Input input1;
        private AntdUI.Input input2;
        private AntdUI.Input input3;
        private AntdUI.Input input4;
        private AntdUI.Input input5;
        private AntdUI.Input input6;
        private AntdUI.Header header1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private AntdUI.Divider divider1;
        private AntdUI.Divider divider2;
        private System.Windows.Forms.Panel panel3;
        private AntdUI.Panel panel5;
        private AntdUI.Input input8;
        private AntdUI.Button button1;
        private System.Windows.Forms.Panel panel6;
        private AntdUI.Input input9;
        private AntdUI.Button button2;
        private AntdUI.Panel panel4;
        private AntdUI.Input input7;
        private AntdUI.Button button3;
    }
}