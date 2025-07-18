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

namespace FontTest
{
    partial class UserControl1
    {
        /// <summary> 
        /// Required designer variable.
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
            panel1 = new Panel();
            input1 = new AntdUI.Input();
            inputNumber1 = new AntdUI.InputNumber();
            select1 = new AntdUI.Select();
            flowLayoutPanel1 = new FlowLayoutPanel();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(input1);
            panel1.Controls.Add(inputNumber1);
            panel1.Controls.Add(select1);
            panel1.Dock = DockStyle.Left;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(178, 337);
            panel1.TabIndex = 1;
            // 
            // input1
            // 
            input1.Dock = DockStyle.Fill;
            input1.Location = new Point(0, 88);
            input1.Multiline = true;
            input1.Name = "input1";
            input1.Size = new Size(178, 249);
            input1.TabIndex = 2;
            input1.Text = "中、目、口、申、田、王、十、回、二";
            // 
            // inputNumber1
            // 
            inputNumber1.Dock = DockStyle.Top;
            inputNumber1.Location = new Point(0, 44);
            inputNumber1.Name = "inputNumber1";
            inputNumber1.PlaceholderText = "字体大小";
            inputNumber1.Size = new Size(178, 44);
            inputNumber1.TabIndex = 1;
            inputNumber1.TabStop = false;
            inputNumber1.Value = 40;
            // 
            // select1
            // 
            select1.Dock = DockStyle.Top;
            select1.ListAutoWidth = true;
            select1.Location = new Point(0, 0);
            select1.Name = "select1";
            select1.PlaceholderText = "选择字体";
            select1.Size = new Size(178, 44);
            select1.TabIndex = 0;
            select1.TabStop = false;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(178, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(8, 8, 0, 0);
            flowLayoutPanel1.Size = new Size(544, 337);
            flowLayoutPanel1.TabIndex = 0;
            // 
            // UserControl1
            // 
            Controls.Add(flowLayoutPanel1);
            Controls.Add(panel1);
            Font = new Font("Microsoft YaHei UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            Name = "UserControl1";
            Size = new Size(722, 337);
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private FlowLayoutPanel flowLayoutPanel1;
        private AntdUI.Select select1;
        private AntdUI.InputNumber inputNumber1;
        private AntdUI.Input input1;
    }
}
