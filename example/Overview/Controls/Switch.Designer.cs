﻿// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
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
    partial class Switch
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
            switch8 = new AntdUI.Switch();
            switch4 = new AntdUI.Switch();
            switch7 = new AntdUI.Switch();
            switch2 = new AntdUI.Switch();
            switch6 = new AntdUI.Switch();
            switch3 = new AntdUI.Switch();
            switch5 = new AntdUI.Switch();
            switch1 = new AntdUI.Switch();
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
            header1.TabIndex = 4;
            header1.Text = "Switch 开关";
            header1.TextDesc = "开关选择器。";
            // 
            // switch8
            // 
            switch8.Enabled = false;
            switch8.Location = new Point(293, 167);
            switch8.Name = "switch8";
            switch8.Size = new Size(60, 38);
            switch8.TabIndex = 5;
            // 
            // switch4
            // 
            switch4.Location = new Point(109, 167);
            switch4.Name = "switch4";
            switch4.Size = new Size(60, 38);
            switch4.TabIndex = 6;
            // 
            // switch7
            // 
            switch7.Checked = true;
            switch7.Enabled = false;
            switch7.Location = new Point(201, 167);
            switch7.Name = "switch7";
            switch7.Size = new Size(60, 38);
            switch7.TabIndex = 7;
            // 
            // switch2
            // 
            switch2.Checked = true;
            switch2.Location = new Point(17, 167);
            switch2.Name = "switch2";
            switch2.Size = new Size(60, 38);
            switch2.TabIndex = 8;
            // 
            // switch6
            // 
            switch6.Enabled = false;
            switch6.Location = new Point(293, 96);
            switch6.Name = "switch6";
            switch6.Size = new Size(60, 38);
            switch6.TabIndex = 9;
            // 
            // switch3
            // 
            switch3.Location = new Point(109, 96);
            switch3.Name = "switch3";
            switch3.Size = new Size(60, 38);
            switch3.TabIndex = 10;
            // 
            // switch5
            // 
            switch5.Checked = true;
            switch5.Enabled = false;
            switch5.Location = new Point(201, 96);
            switch5.Name = "switch5";
            switch5.Size = new Size(60, 38);
            switch5.TabIndex = 11;
            // 
            // switch1
            // 
            switch1.Checked = true;
            switch1.Location = new Point(17, 96);
            switch1.Name = "switch1";
            switch1.Size = new Size(60, 38);
            switch1.TabIndex = 12;
            // 
            // Switch
            // 
            Controls.Add(switch8);
            Controls.Add(switch4);
            Controls.Add(switch7);
            Controls.Add(switch2);
            Controls.Add(switch6);
            Controls.Add(switch3);
            Controls.Add(switch5);
            Controls.Add(switch1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 16F, FontStyle.Regular, GraphicsUnit.Point);
            Name = "Switch";
            Size = new Size(543, 275);
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.Header header1;
        private AntdUI.Switch switch8;
        private AntdUI.Switch switch4;
        private AntdUI.Switch switch7;
        private AntdUI.Switch switch2;
        private AntdUI.Switch switch6;
        private AntdUI.Switch switch3;
        private AntdUI.Switch switch5;
        private AntdUI.Switch switch1;
    }
}