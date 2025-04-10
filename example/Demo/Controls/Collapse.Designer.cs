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
    partial class Collapse
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
            collapse1 = new AntdUI.Collapse();
            collapseItem1 = new AntdUI.CollapseItem();
            label1 = new Label();
            collapseItem2 = new AntdUI.CollapseItem();
            label2 = new Label();
            collapseItem3 = new AntdUI.CollapseItem();
            label3 = new Label();
            panel1 = new System.Windows.Forms.Panel();
            collapse2 = new AntdUI.Collapse();
            collapseItem4 = new AntdUI.CollapseItem();
            label4 = new Label();
            collapseItem5 = new AntdUI.CollapseItem();
            label5 = new Label();
            collapseItem6 = new AntdUI.CollapseItem();
            label6 = new Label();
            divider2 = new AntdUI.Divider();
            divider1 = new AntdUI.Divider();
            collapse1.SuspendLayout();
            collapseItem1.SuspendLayout();
            collapseItem2.SuspendLayout();
            collapseItem3.SuspendLayout();
            panel1.SuspendLayout();
            collapse2.SuspendLayout();
            collapseItem4.SuspendLayout();
            collapseItem5.SuspendLayout();
            collapseItem6.SuspendLayout();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Description = "可以折叠/展开的内容区域。";
            header1.Dock = DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F);
            header1.LocalizationDescription = "Collapse.Description";
            header1.LocalizationText = "Collapse.Text";
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new Padding(0, 0, 0, 10);
            header1.Size = new Size(740, 74);
            header1.TabIndex = 0;
            header1.Text = "Collapse 折叠面板";
            header1.UseTitleFont = true;
            // 
            // collapse1
            // 
            collapse1.Dock = DockStyle.Top;
            collapse1.Items.Add(collapseItem1);
            collapse1.Items.Add(collapseItem2);
            collapse1.Items.Add(collapseItem3);
            collapse1.Location = new Point(0, 28);
            collapse1.Name = "collapse1";
            collapse1.Size = new Size(723, 448);
            collapse1.TabIndex = 0;
            // 
            // collapseItem1
            // 
            collapseItem1.Controls.Add(label1);
            collapseItem1.Location = new Point(-702, -48);
            collapseItem1.Name = "collapseItem1";
            collapseItem1.Size = new Size(702, 48);
            collapseItem1.TabIndex = 0;
            collapseItem1.Text = "This is panel header 1";
            // 
            // label1
            // 
            label1.AutoEllipsis = true;
            label1.Dock = DockStyle.Fill;
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(702, 48);
            label1.TabIndex = 0;
            label1.Text = "A dog is a type of domesticated animal. Known for its loyalty and faithfulness, it can be found as a welcome guest in many households across the world.";
            // 
            // collapseItem2
            // 
            collapseItem2.Controls.Add(label2);
            collapseItem2.Location = new Point(-702, -48);
            collapseItem2.Name = "collapseItem2";
            collapseItem2.Size = new Size(702, 48);
            collapseItem2.TabIndex = 1;
            collapseItem2.Text = "This is panel header 2";
            // 
            // label2
            // 
            label2.AutoEllipsis = true;
            label2.Dock = DockStyle.Fill;
            label2.Location = new Point(0, 0);
            label2.Name = "label2";
            label2.Size = new Size(702, 48);
            label2.TabIndex = 1;
            label2.Text = "A dog is a type of domesticated animal. Known for its loyalty and faithfulness, it can be found as a welcome guest in many households across the world.";
            // 
            // collapseItem3
            // 
            collapseItem3.Controls.Add(label3);
            collapseItem3.Location = new Point(-702, -48);
            collapseItem3.Name = "collapseItem3";
            collapseItem3.Size = new Size(702, 48);
            collapseItem3.TabIndex = 2;
            collapseItem3.Text = "This is panel header 3";
            // 
            // label3
            // 
            label3.AutoEllipsis = true;
            label3.Dock = DockStyle.Fill;
            label3.Location = new Point(0, 0);
            label3.Name = "label3";
            label3.Size = new Size(702, 48);
            label3.TabIndex = 1;
            label3.Text = "A dog is a type of domesticated animal. Known for its loyalty and faithfulness, it can be found as a welcome guest in many households across the world.";
            // 
            // panel1
            // 
            panel1.AutoScroll = true;
            panel1.Controls.Add(collapse2);
            panel1.Controls.Add(divider2);
            panel1.Controls.Add(collapse1);
            panel1.Controls.Add(divider1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 74);
            panel1.Name = "panel1";
            panel1.Size = new Size(740, 386);
            panel1.TabIndex = 0;
            // 
            // collapse2
            // 
            collapse2.BorderWidth = 0F;
            collapse2.Dock = DockStyle.Top;
            collapse2.Gap = 8;
            collapse2.Items.Add(collapseItem4);
            collapse2.Items.Add(collapseItem5);
            collapse2.Items.Add(collapseItem6);
            collapse2.Location = new Point(0, 504);
            collapse2.Name = "collapse2";
            collapse2.Size = new Size(723, 247);
            collapse2.TabIndex = 2;
            collapse2.Unique = true;
            // 
            // collapseItem4
            // 
            collapseItem4.Controls.Add(label4);
            collapseItem4.Expand = true;
            collapseItem4.Location = new Point(19, 66);
            collapseItem4.Name = "collapseItem4";
            collapseItem4.Size = new Size(685, 48);
            collapseItem4.TabIndex = 0;
            collapseItem4.Text = "This is panel header 1";
            // 
            // label4
            // 
            label4.AutoEllipsis = true;
            label4.Dock = DockStyle.Fill;
            label4.Location = new Point(0, 0);
            label4.Name = "label4";
            label4.Size = new Size(685, 48);
            label4.TabIndex = 0;
            label4.Text = "A dog is a type of domesticated animal. Known for its loyalty and faithfulness, it can be found as a welcome guest in many households across the world.";
            // 
            // collapseItem5
            // 
            collapseItem5.Controls.Add(label5);
            collapseItem5.Location = new Point(-702, -48);
            collapseItem5.Name = "collapseItem5";
            collapseItem5.Size = new Size(702, 48);
            collapseItem5.TabIndex = 1;
            collapseItem5.Text = "This is panel header 2";
            // 
            // label5
            // 
            label5.AutoEllipsis = true;
            label5.Dock = DockStyle.Fill;
            label5.Location = new Point(0, 0);
            label5.Name = "label5";
            label5.Size = new Size(702, 48);
            label5.TabIndex = 1;
            label5.Text = "A dog is a type of domesticated animal. Known for its loyalty and faithfulness, it can be found as a welcome guest in many households across the world.";
            // 
            // collapseItem6
            // 
            collapseItem6.Controls.Add(label6);
            collapseItem6.Location = new Point(-702, -48);
            collapseItem6.Name = "collapseItem6";
            collapseItem6.Size = new Size(702, 48);
            collapseItem6.TabIndex = 2;
            collapseItem6.Text = "This is panel header 3";
            // 
            // label6
            // 
            label6.AutoEllipsis = true;
            label6.Dock = DockStyle.Fill;
            label6.Location = new Point(0, 0);
            label6.Name = "label6";
            label6.Size = new Size(702, 48);
            label6.TabIndex = 1;
            label6.Text = "A dog is a type of domesticated animal. Known for its loyalty and faithfulness, it can be found as a welcome guest in many households across the world.";
            // 
            // divider2
            // 
            divider2.Dock = DockStyle.Top;
            divider2.Font = new Font("Microsoft YaHei UI", 10F);
            divider2.LocalizationText = "Collapse.{id}";
            divider2.Location = new Point(0, 476);
            divider2.Name = "divider2";
            divider2.Orientation = AntdUI.TOrientation.Left;
            divider2.Size = new Size(723, 28);
            divider2.TabIndex = 1;
            divider2.Text = "无边框";
            // 
            // divider1
            // 
            divider1.Dock = DockStyle.Top;
            divider1.Font = new Font("Microsoft YaHei UI", 10F);
            divider1.LocalizationText = "Collapse.{id}";
            divider1.Location = new Point(0, 0);
            divider1.Name = "divider1";
            divider1.Orientation = AntdUI.TOrientation.Left;
            divider1.Size = new Size(723, 28);
            divider1.TabIndex = 0;
            divider1.Text = "常规";
            // 
            // Collapse
            // 
            Controls.Add(panel1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 12F);
            Name = "Collapse";
            Size = new Size(740, 460);
            collapse1.ResumeLayout(false);
            collapseItem1.ResumeLayout(false);
            collapseItem2.ResumeLayout(false);
            collapseItem3.ResumeLayout(false);
            panel1.ResumeLayout(false);
            collapse2.ResumeLayout(false);
            collapseItem4.ResumeLayout(false);
            collapseItem5.ResumeLayout(false);
            collapseItem6.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.PageHeader header1;
        private AntdUI.Collapse collapse1;
        private AntdUI.CollapseItem collapseItem1;
        private AntdUI.CollapseItem collapseItem2;
        private AntdUI.CollapseItem collapseItem3;
        private Label label1;
        private Label label2;
        private Label label3;
        private System.Windows.Forms.Panel panel1;
        private AntdUI.Divider divider1;
        private AntdUI.Collapse collapse2;
        private AntdUI.CollapseItem collapseItem4;
        private Label label4;
        private AntdUI.CollapseItem collapseItem5;
        private Label label5;
        private AntdUI.CollapseItem collapseItem6;
        private Label label6;
        private AntdUI.Divider divider2;
    }
}