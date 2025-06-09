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
// GITEE: https://gitee.com/AntdUI/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

using System.Drawing;
using System.Windows.Forms;

namespace Demo.Controls
{
    partial class InputNumber
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
            input1 = new AntdUI.InputNumber();
            input2 = new AntdUI.InputNumber();
            input3 = new AntdUI.InputNumber();
            input4 = new AntdUI.InputNumber();
            input5 = new AntdUI.InputNumber();
            input6 = new AntdUI.InputNumber();
            panel1 = new System.Windows.Forms.Panel();
            panel2 = new System.Windows.Forms.Panel();
            divider1 = new AntdUI.Divider();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Description = "通过鼠标或键盘，输入范围内的数值。";
            header1.Dock = DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F);
            header1.LocalizationDescription = "InputNumber.Description";
            header1.LocalizationText = "InputNumber.Text";
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new Padding(0, 0, 0, 10);
            header1.Size = new Size(555, 74);
            header1.TabIndex = 0;
            header1.Text = "InputNumber 数字输入框";
            header1.UseTitleFont = true;
            // 
            // input1
            // 
            input1.Location = new Point(18, 6);
            input1.Name = "input1";
            input1.Size = new Size(220, 44);
            input1.TabIndex = 0;
            input1.Text = "1";
            input1.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // input2
            // 
            input2.DecimalPlaces = 1;
            input2.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            input2.Location = new Point(18, 54);
            input2.Name = "input2";
            input2.Size = new Size(220, 44);
            input2.TabIndex = 2;
            input2.Text = "1.0";
            input2.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // input3
            // 
            input3.LocalizationPlaceholderText = "InputNumber.{id}";
            input3.Location = new Point(244, 6);
            input3.Name = "input3";
            input3.PlaceholderText = "请输入数字";
            input3.Radius = 0;
            input3.Size = new Size(220, 44);
            input3.TabIndex = 1;
            input3.Text = "0";
            // 
            // input4
            // 
            input4.Increment = new decimal(new int[] { 1000, 0, 0, 0 });
            input4.Location = new Point(244, 54);
            input4.Name = "input4";
            input4.Radius = 0;
            input4.ReadOnly = true;
            input4.Size = new Size(220, 44);
            input4.TabIndex = 3;
            input4.Text = "10,000";
            input4.ThousandsSeparator = true;
            input4.Value = new decimal(new int[] { 10000, 0, 0, 0 });
            // 
            // input5
            // 
            input5.Location = new Point(18, 101);
            input5.Name = "input5";
            input5.PrefixSvg = "PoweroffOutlined";
            input5.Size = new Size(220, 44);
            input5.TabIndex = 4;
            input5.Text = "10";
            input5.Value = new decimal(new int[] { 10, 0, 0, 0 });
            // 
            // input6
            // 
            input6.BorderWidth = 2F;
            input6.Location = new Point(244, 102);
            input6.Name = "input6";
            input6.Size = new Size(220, 44);
            input6.TabIndex = 5;
            input6.Text = "1";
            input6.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // panel1
            // 
            panel1.AutoScroll = true;
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(divider1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 74);
            panel1.Name = "panel1";
            panel1.Size = new Size(555, 480);
            panel1.TabIndex = 6;
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
            panel2.Location = new Point(0, 28);
            panel2.Name = "panel2";
            panel2.Size = new Size(555, 162);
            panel2.TabIndex = 0;
            // 
            // divider1
            // 
            divider1.Dock = DockStyle.Top;
            divider1.Font = new Font("Microsoft YaHei UI", 10F);
            divider1.LocalizationText = "InputNumber.{id}";
            divider1.Location = new Point(0, 0);
            divider1.Name = "divider1";
            divider1.Orientation = AntdUI.TOrientation.Left;
            divider1.Size = new Size(555, 28);
            divider1.TabIndex = 1;
            divider1.TabStop = false;
            divider1.Text = "常规";
            // 
            // InputNumber
            // 
            Controls.Add(panel1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 12F);
            Name = "InputNumber";
            Size = new Size(555, 554);
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.PageHeader header1;
        private AntdUI.InputNumber input1;
        private AntdUI.InputNumber input2;
        private AntdUI.InputNumber input3;
        private AntdUI.InputNumber input4;
        private AntdUI.InputNumber input5;
        private AntdUI.InputNumber input6;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private AntdUI.Divider divider1;
    }
}