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

namespace Demo.Controls
{
    partial class Signal
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
            signal1 = new AntdUI.Signal();
            signal2 = new AntdUI.Signal();
            signal3 = new AntdUI.Signal();
            button2 = new AntdUI.Button();
            button1 = new AntdUI.Button();
            stackPanel1 = new AntdUI.StackPanel();
            signal5 = new AntdUI.Signal();
            signal4 = new AntdUI.Signal();
            divider1 = new AntdUI.Divider();
            stackPanel2 = new AntdUI.StackPanel();
            signal6 = new AntdUI.Signal();
            signal7 = new AntdUI.Signal();
            signal8 = new AntdUI.Signal();
            signal9 = new AntdUI.Signal();
            signal10 = new AntdUI.Signal();
            divider2 = new AntdUI.Divider();
            stackPanel3 = new AntdUI.StackPanel();
            signal12 = new AntdUI.Signal();
            signal13 = new AntdUI.Signal();
            signal14 = new AntdUI.Signal();
            signal15 = new AntdUI.Signal();
            divider3 = new AntdUI.Divider();
            stackPanel4 = new AntdUI.StackPanel();
            stackPanel1.SuspendLayout();
            stackPanel2.SuspendLayout();
            stackPanel3.SuspendLayout();
            stackPanel4.SuspendLayout();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Description = "展示设备信号。";
            header1.Dock = System.Windows.Forms.DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F);
            header1.LocalizationDescription = "Signal.Description";
            header1.LocalizationText = "Signal.Text";
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            header1.Size = new Size(596, 74);
            header1.TabIndex = 0;
            header1.Text = "Signal 信号强度";
            header1.UseTitleFont = true;
            // 
            // signal1
            // 
            signal1.Location = new Point(3, 3);
            signal1.Name = "signal1";
            signal1.Size = new Size(74, 42);
            signal1.TabIndex = 0;
            signal1.Value = 1;
            // 
            // signal2
            // 
            signal2.Location = new Point(83, 3);
            signal2.Name = "signal2";
            signal2.Size = new Size(74, 42);
            signal2.TabIndex = 1;
            signal2.Value = 2;
            // 
            // signal3
            // 
            signal3.Location = new Point(163, 3);
            signal3.Name = "signal3";
            signal3.Size = new Size(74, 42);
            signal3.TabIndex = 2;
            signal3.Value = 3;
            // 
            // button2
            // 
            button2.AutoSizeMode = AntdUI.TAutoSize.Width;
            button2.LocalizationText = "Signal.Subtract";
            button2.Location = new Point(59, 7);
            button2.Name = "button2";
            button2.Size = new Size(50, 38);
            button2.TabIndex = 10;
            button2.Text = "减";
            button2.Type = AntdUI.TTypeMini.Success;
            button2.Click += button2_Click;
            // 
            // button1
            // 
            button1.AutoSizeMode = AntdUI.TAutoSize.Width;
            button1.LocalizationText = "Signal.Add";
            button1.Location = new Point(3, 7);
            button1.Name = "button1";
            button1.Size = new Size(50, 38);
            button1.TabIndex = 9;
            button1.Text = "加";
            button1.Type = AntdUI.TTypeMini.Primary;
            button1.Click += button1_Click;
            // 
            // stackPanel1
            // 
            stackPanel1.Controls.Add(signal5);
            stackPanel1.Controls.Add(signal4);
            stackPanel1.Controls.Add(signal3);
            stackPanel1.Controls.Add(signal2);
            stackPanel1.Controls.Add(signal1);
            stackPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            stackPanel1.Location = new Point(0, 102);
            stackPanel1.Name = "stackPanel1";
            stackPanel1.Size = new Size(596, 48);
            stackPanel1.TabIndex = 1;
            // 
            // signal5
            // 
            signal5.Location = new Point(323, 3);
            signal5.Name = "signal5";
            signal5.Size = new Size(74, 42);
            signal5.TabIndex = 4;
            signal5.Value = 5;
            // 
            // signal4
            // 
            signal4.Location = new Point(243, 3);
            signal4.Name = "signal4";
            signal4.Size = new Size(74, 42);
            signal4.TabIndex = 3;
            signal4.Value = 4;
            // 
            // divider1
            // 
            divider1.Dock = System.Windows.Forms.DockStyle.Top;
            divider1.Font = new Font("Microsoft YaHei UI", 10F);
            divider1.LocalizationText = "Signal.{id}";
            divider1.Location = new Point(0, 74);
            divider1.Name = "divider1";
            divider1.Orientation = AntdUI.TOrientation.Left;
            divider1.Size = new Size(596, 28);
            divider1.TabIndex = 0;
            divider1.Text = "基本用法";
            // 
            // stackPanel2
            // 
            stackPanel2.Controls.Add(signal6);
            stackPanel2.Controls.Add(signal7);
            stackPanel2.Controls.Add(signal8);
            stackPanel2.Controls.Add(signal9);
            stackPanel2.Controls.Add(signal10);
            stackPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            stackPanel2.Location = new Point(0, 178);
            stackPanel2.Name = "stackPanel2";
            stackPanel2.Size = new Size(596, 48);
            stackPanel2.TabIndex = 2;
            // 
            // signal6
            // 
            signal6.Location = new Point(323, 3);
            signal6.Name = "signal6";
            signal6.Padding = new System.Windows.Forms.Padding(24, 0, 24, 0);
            signal6.Size = new Size(74, 42);
            signal6.StyleLine = true;
            signal6.TabIndex = 4;
            signal6.Value = 5;
            // 
            // signal7
            // 
            signal7.Location = new Point(243, 3);
            signal7.Name = "signal7";
            signal7.Padding = new System.Windows.Forms.Padding(24, 0, 24, 0);
            signal7.Size = new Size(74, 42);
            signal7.StyleLine = true;
            signal7.TabIndex = 3;
            signal7.Value = 4;
            // 
            // signal8
            // 
            signal8.Location = new Point(163, 3);
            signal8.Name = "signal8";
            signal8.Padding = new System.Windows.Forms.Padding(24, 0, 24, 0);
            signal8.Size = new Size(74, 42);
            signal8.StyleLine = true;
            signal8.TabIndex = 2;
            signal8.Value = 3;
            // 
            // signal9
            // 
            signal9.Location = new Point(83, 3);
            signal9.Name = "signal9";
            signal9.Padding = new System.Windows.Forms.Padding(24, 0, 24, 0);
            signal9.Size = new Size(74, 42);
            signal9.StyleLine = true;
            signal9.TabIndex = 1;
            signal9.Value = 2;
            // 
            // signal10
            // 
            signal10.Location = new Point(3, 3);
            signal10.Name = "signal10";
            signal10.Padding = new System.Windows.Forms.Padding(24, 0, 24, 0);
            signal10.Size = new Size(74, 42);
            signal10.StyleLine = true;
            signal10.TabIndex = 0;
            signal10.Value = 1;
            // 
            // divider2
            // 
            divider2.Dock = System.Windows.Forms.DockStyle.Top;
            divider2.Font = new Font("Microsoft YaHei UI", 10F);
            divider2.LocalizationText = "Signal.{id}";
            divider2.Location = new Point(0, 150);
            divider2.Name = "divider2";
            divider2.Orientation = AntdUI.TOrientation.Left;
            divider2.Size = new Size(596, 28);
            divider2.TabIndex = 0;
            divider2.Text = "线条样式";
            // 
            // stackPanel3
            // 
            stackPanel3.Controls.Add(signal12);
            stackPanel3.Controls.Add(signal13);
            stackPanel3.Controls.Add(signal14);
            stackPanel3.Controls.Add(signal15);
            stackPanel3.Dock = System.Windows.Forms.DockStyle.Top;
            stackPanel3.Location = new Point(0, 254);
            stackPanel3.Name = "stackPanel3";
            stackPanel3.Size = new Size(596, 48);
            stackPanel3.TabIndex = 3;
            // 
            // signal12
            // 
            signal12.FillFully = Color.Orange;
            signal12.Loading = true;
            signal12.Location = new Point(243, 3);
            signal12.Name = "signal12";
            signal12.Padding = new System.Windows.Forms.Padding(24, 0, 24, 0);
            signal12.Size = new Size(74, 42);
            signal12.StyleLine = true;
            signal12.TabIndex = 3;
            // 
            // signal13
            // 
            signal13.FillFully = Color.Orange;
            signal13.Loading = true;
            signal13.Location = new Point(163, 3);
            signal13.Name = "signal13";
            signal13.Size = new Size(74, 42);
            signal13.TabIndex = 2;
            // 
            // signal14
            // 
            signal14.Loading = true;
            signal14.Location = new Point(83, 3);
            signal14.Name = "signal14";
            signal14.Padding = new System.Windows.Forms.Padding(24, 0, 24, 0);
            signal14.Size = new Size(74, 42);
            signal14.StyleLine = true;
            signal14.TabIndex = 1;
            // 
            // signal15
            // 
            signal15.Loading = true;
            signal15.Location = new Point(3, 3);
            signal15.Name = "signal15";
            signal15.Size = new Size(74, 42);
            signal15.TabIndex = 0;
            // 
            // divider3
            // 
            divider3.Dock = System.Windows.Forms.DockStyle.Top;
            divider3.Font = new Font("Microsoft YaHei UI", 10F);
            divider3.LocalizationText = "Signal.{id}";
            divider3.Location = new Point(0, 226);
            divider3.Name = "divider3";
            divider3.Orientation = AntdUI.TOrientation.Left;
            divider3.Size = new Size(596, 28);
            divider3.TabIndex = 0;
            divider3.Text = "动画";
            // 
            // stackPanel4
            // 
            stackPanel4.Controls.Add(button2);
            stackPanel4.Controls.Add(button1);
            stackPanel4.Dock = System.Windows.Forms.DockStyle.Top;
            stackPanel4.Location = new Point(0, 302);
            stackPanel4.Name = "stackPanel4";
            stackPanel4.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            stackPanel4.Size = new Size(596, 48);
            stackPanel4.TabIndex = 10;
            // 
            // Signal
            // 
            Controls.Add(stackPanel4);
            Controls.Add(stackPanel3);
            Controls.Add(divider3);
            Controls.Add(stackPanel2);
            Controls.Add(divider2);
            Controls.Add(stackPanel1);
            Controls.Add(divider1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 12F);
            Name = "Signal";
            Size = new Size(596, 445);
            stackPanel1.ResumeLayout(false);
            stackPanel2.ResumeLayout(false);
            stackPanel3.ResumeLayout(false);
            stackPanel4.ResumeLayout(false);
            stackPanel4.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.PageHeader header1;
        private AntdUI.Signal signal1;
        private AntdUI.Signal signal2;
        private AntdUI.Signal signal3;
        private AntdUI.Button button2;
        private AntdUI.Button button1;
        private AntdUI.StackPanel stackPanel1;
        private AntdUI.Divider divider1;
        private AntdUI.Signal signal5;
        private AntdUI.Signal signal4;
        private AntdUI.StackPanel stackPanel2;
        private AntdUI.Signal signal6;
        private AntdUI.Signal signal7;
        private AntdUI.Signal signal8;
        private AntdUI.Signal signal9;
        private AntdUI.Signal signal10;
        private AntdUI.Divider divider2;
        private AntdUI.StackPanel stackPanel3;
        private AntdUI.Signal signal12;
        private AntdUI.Signal signal13;
        private AntdUI.Signal signal14;
        private AntdUI.Signal signal15;
        private AntdUI.Divider divider3;
        private AntdUI.StackPanel stackPanel4;
    }
}