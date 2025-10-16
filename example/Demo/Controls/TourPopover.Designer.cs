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
    partial class TourPopover
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
            label1 = new AntdUI.Label();
            label2 = new AntdUI.Label();
            label3 = new AntdUI.Label();
            btn_previous = new AntdUI.Button();
            btn_next = new AntdUI.Button();
            panel1 = new AntdUI.Panel();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Dock = DockStyle.Top;
            label1.Font = new Font("Microsoft YaHei UI", 16F, FontStyle.Bold);
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(168, 36);
            label1.TabIndex = 0;
            // 
            // label2
            // 
            label2.BackColor = Color.Transparent;
            label2.Dock = DockStyle.Fill;
            label2.Location = new Point(0, 0);
            label2.Name = "label2";
            label2.Size = new Size(28, 39);
            label2.TabIndex = 0;
            // 
            // label3
            // 
            label3.Dock = DockStyle.Fill;
            label3.Font = new Font("Microsoft YaHei UI", 12F);
            label3.Location = new Point(0, 36);
            label3.Name = "label3";
            label3.Size = new Size(168, 71);
            label3.TabIndex = 0;
            // 
            // btn_previous
            // 
            btn_previous.AutoSizeMode = AntdUI.TAutoSize.Width;
            btn_previous.BorderWidth = 1F;
            btn_previous.Dock = DockStyle.Right;
            btn_previous.LocalizationText = "Previous";
            btn_previous.Location = new Point(28, 0);
            btn_previous.Name = "btn_previous";
            btn_previous.Size = new Size(70, 39);
            btn_previous.TabIndex = 2;
            btn_previous.Text = "上一页";
            btn_previous.Click += btn_previous_Click;
            // 
            // btn_next
            // 
            btn_next.AutoSizeMode = AntdUI.TAutoSize.Width;
            btn_next.BorderWidth = 1F;
            btn_next.Dock = DockStyle.Right;
            btn_next.LocalizationText = "Next";
            btn_next.Location = new Point(98, 0);
            btn_next.Name = "btn_next";
            btn_next.Size = new Size(70, 39);
            btn_next.TabIndex = 1;
            btn_next.Text = "下一页";
            btn_next.Type = AntdUI.TTypeMini.Primary;
            btn_next.Click += btn_next_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(label2);
            panel1.Controls.Add(btn_previous);
            panel1.Controls.Add(btn_next);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 107);
            panel1.Name = "panel1";
            panel1.Radius = 0;
            panel1.Size = new Size(168, 39);
            panel1.TabIndex = 2;
            // 
            // TourPopover
            // 
            Controls.Add(label3);
            Controls.Add(panel1);
            Controls.Add(label1);
            Name = "TourPopover";
            Size = new Size(168, 146);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.Label label1;
        private AntdUI.Label label2;
        private AntdUI.Label label3;
        private AntdUI.Button btn_previous;
        private AntdUI.Button btn_next;
        private AntdUI.Panel panel1;
    }
}