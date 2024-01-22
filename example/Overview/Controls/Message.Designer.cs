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
    partial class Message
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
            panel1 = new System.Windows.Forms.Panel();
            panel3 = new System.Windows.Forms.Panel();
            button5 = new AntdUI.Button();
            button6 = new AntdUI.Button();
            button7 = new AntdUI.Button();
            button8 = new AntdUI.Button();
            divider2 = new AntdUI.Divider();
            panel2 = new System.Windows.Forms.Panel();
            button4 = new AntdUI.Button();
            button3 = new AntdUI.Button();
            button2 = new AntdUI.Button();
            button1 = new AntdUI.Button();
            divider1 = new AntdUI.Divider();
            panel1.SuspendLayout();
            panel3.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Dock = DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new Padding(6);
            header1.Size = new Size(543, 79);
            header1.TabIndex = 5;
            header1.Text = "Message 全局提示";
            header1.TextDesc = "全局展示操作反馈信息。";
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
            panel1.Size = new Size(543, 378);
            panel1.TabIndex = 6;
            // 
            // panel3
            // 
            panel3.Controls.Add(button5);
            panel3.Controls.Add(button6);
            panel3.Controls.Add(button7);
            panel3.Controls.Add(button8);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(0, 107);
            panel3.Name = "panel3";
            panel3.Size = new Size(543, 63);
            panel3.TabIndex = 4;
            // 
            // button5
            // 
            button5.BorderWidth = 2F;
            button5.Location = new Point(362, 13);
            button5.Name = "button5";
            button5.Size = new Size(110, 40);
            button5.TabIndex = 1;
            button5.Text = "Info";
            button5.Type = AntdUI.TTypeMini.Info;
            button5.Click += button5_Click;
            // 
            // button6
            // 
            button6.BorderWidth = 2F;
            button6.Location = new Point(246, 13);
            button6.Name = "button6";
            button6.Size = new Size(110, 40);
            button6.TabIndex = 1;
            button6.Text = "Warning";
            button6.Type = AntdUI.TTypeMini.Warn;
            button6.Click += button6_Click;
            // 
            // button7
            // 
            button7.BorderWidth = 2F;
            button7.Location = new Point(130, 13);
            button7.Name = "button7";
            button7.Size = new Size(110, 40);
            button7.TabIndex = 1;
            button7.Text = "Error";
            button7.Type = AntdUI.TTypeMini.Error;
            button7.Click += button7_Click;
            // 
            // button8
            // 
            button8.BorderWidth = 2F;
            button8.Location = new Point(14, 13);
            button8.Name = "button8";
            button8.Size = new Size(110, 40);
            button8.TabIndex = 1;
            button8.Text = "Success";
            button8.Type = AntdUI.TTypeMini.Success;
            button8.Click += button8_Click;
            // 
            // divider2
            // 
            divider2.Dock = DockStyle.Top;
            divider2.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            divider2.Location = new Point(0, 85);
            divider2.Margin = new Padding(10);
            divider2.Name = "divider2";
            divider2.Orientation = AntdUI.TOrientation.Left;
            divider2.Size = new Size(543, 22);
            divider2.TabIndex = 3;
            divider2.Text = "加载中\r\n";
            // 
            // panel2
            // 
            panel2.Controls.Add(button4);
            panel2.Controls.Add(button3);
            panel2.Controls.Add(button2);
            panel2.Controls.Add(button1);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 22);
            panel2.Name = "panel2";
            panel2.Size = new Size(543, 63);
            panel2.TabIndex = 2;
            // 
            // button4
            // 
            button4.BorderWidth = 2F;
            button4.Location = new Point(362, 10);
            button4.Name = "button4";
            button4.Size = new Size(110, 40);
            button4.TabIndex = 1;
            button4.Text = "Info";
            button4.Click += button4_Click;
            // 
            // button3
            // 
            button3.BorderWidth = 2F;
            button3.Location = new Point(246, 10);
            button3.Name = "button3";
            button3.Size = new Size(110, 40);
            button3.TabIndex = 1;
            button3.Text = "Warning";
            button3.Click += button3_Click;
            // 
            // button2
            // 
            button2.BorderWidth = 2F;
            button2.Location = new Point(130, 9);
            button2.Name = "button2";
            button2.Size = new Size(110, 40);
            button2.TabIndex = 1;
            button2.Text = "Error";
            button2.Click += button2_Click;
            // 
            // button1
            // 
            button1.BorderWidth = 2F;
            button1.Location = new Point(14, 9);
            button1.Name = "button1";
            button1.Size = new Size(110, 40);
            button1.TabIndex = 1;
            button1.Text = "Success";
            button1.Click += button1_Click;
            // 
            // divider1
            // 
            divider1.Dock = DockStyle.Top;
            divider1.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            divider1.Location = new Point(0, 0);
            divider1.Margin = new Padding(10);
            divider1.Name = "divider1";
            divider1.Orientation = AntdUI.TOrientation.Left;
            divider1.Size = new Size(543, 22);
            divider1.TabIndex = 0;
            divider1.Text = "四种样式";
            // 
            // Message
            // 
            Controls.Add(panel1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            Name = "Message";
            Size = new Size(543, 457);
            panel1.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.Header header1;
        private System.Windows.Forms.Panel panel1;
        private AntdUI.Divider divider1;
        private AntdUI.Button button1;
        private AntdUI.Button button3;
        private AntdUI.Button button2;
        private AntdUI.Button button4;
        private System.Windows.Forms.Panel panel3;
        private AntdUI.Button button5;
        private AntdUI.Button button6;
        private AntdUI.Button button7;
        private AntdUI.Button button8;
        private AntdUI.Divider divider2;
        private System.Windows.Forms.Panel panel2;
    }
}