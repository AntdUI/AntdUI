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
    partial class ColorPicker
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
            flowLayoutPanel1 = new FlowLayoutPanel();
            colorPicker3 = new AntdUI.ColorPicker();
            colorPicker1 = new AntdUI.ColorPicker();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Dock = DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new Padding(6);
            header1.Size = new Size(740, 79);
            header1.TabIndex = 4;
            header1.Text = "ColorPicker 颜色选择器";
            header1.TextDesc = "提供颜色选取的组件。";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.Controls.Add(colorPicker3);
            flowLayoutPanel1.Controls.Add(colorPicker1);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(0, 79);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(740, 323);
            flowLayoutPanel1.TabIndex = 5;
            // 
            // colorPicker3
            // 
            colorPicker3.AutoSize = true;
            colorPicker3.Location = new Point(3, 3);
            colorPicker3.Name = "colorPicker3";
            colorPicker3.Size = new Size(47, 47);
            colorPicker3.TabIndex = 26;
            colorPicker3.Value = Color.FromArgb(22, 119, 255);
            // 
            // colorPicker1
            // 
            colorPicker1.AutoSize = true;
            colorPicker1.Location = new Point(56, 3);
            colorPicker1.Name = "colorPicker1";
            colorPicker1.ShowText = true;
            colorPicker1.Size = new Size(133, 47);
            colorPicker1.TabIndex = 26;
            colorPicker1.Value = Color.FromArgb(22, 119, 255);
            // 
            // ColorPicker
            // 
            Controls.Add(flowLayoutPanel1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            Name = "ColorPicker";
            Size = new Size(740, 402);
            flowLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.Header header1;
        private FlowLayoutPanel flowLayoutPanel1;
        private AntdUI.ColorPicker colorPicker3;
        private AntdUI.ColorPicker colorPicker1;
    }
}