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
    partial class TimePicker
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
            panel1 = new System.Windows.Forms.Panel();
            panel2 = new System.Windows.Forms.Panel();
            datePicker3 = new AntdUI.TimePicker();
            datePicker2 = new AntdUI.TimePicker();
            datePicker1 = new AntdUI.TimePicker();
            divider1 = new AntdUI.Divider();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Description = "输入或选择时间的控件。";
            header1.Dock = DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F);
            header1.LocalizationDescription = "TimePicker.Description";
            header1.LocalizationText = "TimePicker.Text";
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new Padding(0, 0, 0, 10);
            header1.Size = new Size(770, 74);
            header1.TabIndex = 0;
            header1.Text = "TimePicker 时间选择框";
            header1.UseTitleFont = true;
            // 
            // panel1
            // 
            panel1.AutoScroll = true;
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(divider1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 74);
            panel1.Name = "panel1";
            panel1.Size = new Size(770, 496);
            panel1.TabIndex = 6;
            // 
            // panel2
            // 
            panel2.Controls.Add(datePicker3);
            panel2.Controls.Add(datePicker2);
            panel2.Controls.Add(datePicker1);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 28);
            panel2.Name = "panel2";
            panel2.Size = new Size(770, 118);
            panel2.TabIndex = 3;
            // 
            // datePicker3
            // 
            datePicker3.AllowClear = true;
            datePicker3.DropDownArrow = true;
            datePicker3.Location = new Point(19, 56);
            datePicker3.Name = "datePicker3";
            datePicker3.PlaceholderText = "请选择日期";
            datePicker3.Size = new Size(112, 44);
            datePicker3.TabIndex = 1;
            datePicker3.Text = "00:00:00";
            // 
            // datePicker2
            // 
            datePicker2.AllowClear = true;
            datePicker2.Location = new Point(162, 6);
            datePicker2.Name = "datePicker2";
            datePicker2.Placement = AntdUI.TAlignFrom.BR;
            datePicker2.Size = new Size(112, 44);
            datePicker2.TabIndex = 2;
            datePicker2.Text = "00:00:00";
            // 
            // datePicker1
            // 
            datePicker1.Location = new Point(19, 6);
            datePicker1.Name = "datePicker1";
            datePicker1.PlaceholderText = "请选择日期";
            datePicker1.Size = new Size(112, 44);
            datePicker1.TabIndex = 3;
            datePicker1.Text = "00:00:00";
            datePicker1.ValueTimeHorizontal = true;
            // 
            // divider1
            // 
            divider1.Dock = DockStyle.Top;
            divider1.Font = new Font("Microsoft YaHei UI", 10F);
            divider1.LocalizationText = "TimePicker.{id}";
            divider1.Location = new Point(0, 0);
            divider1.Name = "divider1";
            divider1.Orientation = AntdUI.TOrientation.Left;
            divider1.Size = new Size(770, 28);
            divider1.TabIndex = 2;
            divider1.Text = "选择日期";
            // 
            // TimePicker
            // 
            Controls.Add(panel1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 12F);
            Name = "TimePicker";
            Size = new Size(770, 570);
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.PageHeader header1;
        private System.Windows.Forms.Panel panel1;
        private AntdUI.Divider divider1;
        private System.Windows.Forms.Panel panel2;
        private AntdUI.TimePicker datePicker2;
        private AntdUI.TimePicker datePicker1;
        private AntdUI.TimePicker datePicker3;
    }
}