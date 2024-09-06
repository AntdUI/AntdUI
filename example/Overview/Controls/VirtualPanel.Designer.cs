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
    partial class VirtualPanel
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
            header1 = new AntdUI.PageHeader();
            vpanel = new AntdUI.VirtualPanel();
            panel1 = new System.Windows.Forms.Panel();
            checkbox1 = new AntdUI.Checkbox();
            select3 = new AntdUI.Select();
            select2 = new AntdUI.Select();
            select1 = new AntdUI.Select();
            button1 = new System.Windows.Forms.Button();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Description = "内容区域。";
            header1.Dock = DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F);
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new Padding(0, 0, 0, 10);
            header1.Size = new Size(835, 74);
            header1.TabIndex = 0;
            header1.Text = "VirtualPanel 虚拟容器";
            header1.UseTitleFont = true;
            // 
            // vpanel
            // 
            vpanel.Dock = DockStyle.Fill;
            vpanel.Gap = 10;
            vpanel.Location = new Point(0, 173);
            vpanel.Name = "vpanel";
            vpanel.Radius = 20;
            vpanel.Size = new Size(835, 461);
            vpanel.TabIndex = 1;
            // 
            // panel1
            // 
            panel1.Controls.Add(checkbox1);
            panel1.Controls.Add(select3);
            panel1.Controls.Add(select2);
            panel1.Controls.Add(select1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 74);
            panel1.Name = "panel1";
            panel1.Size = new Size(835, 99);
            panel1.TabIndex = 5;
            // 
            // checkbox1
            // 
            checkbox1.AutoCheck = true;
            checkbox1.Location = new Point(229, 54);
            checkbox1.Name = "checkbox1";
            checkbox1.Size = new Size(91, 30);
            checkbox1.TabIndex = 1;
            checkbox1.Text = "瀑布流";
            checkbox1.CheckedChanged += checkbox1_CheckedChanged;
            // 
            // select3
            // 
            select3.List = true;
            select3.Location = new Point(279, 3);
            select3.Name = "select3";
            select3.PrefixText = "align-content";
            select3.Size = new Size(256, 40);
            select3.TabIndex = 0;
            // 
            // select2
            // 
            select2.List = true;
            select2.Location = new Point(3, 49);
            select2.Name = "select2";
            select2.PrefixText = "align-items";
            select2.Size = new Size(212, 40);
            select2.TabIndex = 0;
            // 
            // select1
            // 
            select1.List = true;
            select1.Location = new Point(3, 3);
            select1.Name = "select1";
            select1.PrefixText = "justify-content";
            select1.Size = new Size(270, 40);
            select1.TabIndex = 0;
            // 
            // button1
            // 
            button1.Location = new Point(753, 23);
            button1.Name = "button1";
            button1.Size = new Size(69, 30);
            button1.TabIndex = 1;
            button1.Text = "花瓣";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // VirtualPanel
            // 
            Controls.Add(button1);
            Controls.Add(vpanel);
            Controls.Add(panel1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 11F);
            Name = "VirtualPanel";
            Size = new Size(835, 634);
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private AntdUI.PageHeader header1;
        private AntdUI.VirtualPanel vpanel;
        private System.Windows.Forms.Panel panel1;
        private AntdUI.Select select1;
        private AntdUI.Select select3;
        private AntdUI.Select select2;
        private System.Windows.Forms.Button button1;
        private AntdUI.Checkbox checkbox1;
    }
}