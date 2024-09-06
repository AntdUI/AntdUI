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
    partial class Select
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
            panel1 = new System.Windows.Forms.Panel();
            panel4 = new System.Windows.Forms.Panel();
            select8 = new AntdUI.Select();
            selectMultiple1 = new AntdUI.SelectMultiple();
            divider3 = new AntdUI.Divider();
            panel3 = new System.Windows.Forms.Panel();
            panel5 = new System.Windows.Forms.Panel();
            select7 = new AntdUI.Select();
            button1 = new AntdUI.Button();
            panel8 = new System.Windows.Forms.Panel();
            select6 = new AntdUI.Select();
            button4 = new AntdUI.Button();
            divider2 = new AntdUI.Divider();
            panel2 = new System.Windows.Forms.Panel();
            select2 = new AntdUI.Select();
            select4 = new AntdUI.Select();
            select3 = new AntdUI.Select();
            select5 = new AntdUI.Select();
            select1 = new AntdUI.Select();
            divider1 = new AntdUI.Divider();
            panel1.SuspendLayout();
            panel4.SuspendLayout();
            panel3.SuspendLayout();
            panel5.SuspendLayout();
            panel8.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Description = "下拉选择器。";
            header1.Dock = DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F);
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new Padding(0, 0, 0, 10);
            header1.Size = new Size(555, 74);
            header1.TabIndex = 0;
            header1.Text = "Select 选择器";
            header1.UseTitleFont = true;
            // 
            // panel1
            // 
            panel1.AutoScroll = true;
            panel1.Controls.Add(panel4);
            panel1.Controls.Add(divider3);
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(divider2);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(divider1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 74);
            panel1.Name = "panel1";
            panel1.Size = new Size(555, 480);
            panel1.TabIndex = 6;
            // 
            // panel4
            // 
            panel4.Controls.Add(select8);
            panel4.Controls.Add(selectMultiple1);
            panel4.Dock = DockStyle.Top;
            panel4.Location = new Point(0, 279);
            panel4.Name = "panel4";
            panel4.Size = new Size(555, 100);
            panel4.TabIndex = 5;
            // 
            // select8
            // 
            select8.DropDownArrow = true;
            select8.Items.AddRange(new AntdUI.ISelectItem[] { new AntdUI.SelectItem(0, "Lucy"), new AntdUI.SelectItem(1, "Tom"), new AntdUI.SelectItem(1, "AduSkin"), new AntdUI.DividerSelectItem(), new AntdUI.SelectItem(0, "WangLi"), new AntdUI.SelectItem(0, "HUAWEI"), new AntdUI.SelectItem(0, "XIAOMI") });
            select8.List = true;
            select8.ListAutoWidth = true;
            select8.Location = new Point(18, 20);
            select8.Margin = new Padding(2, 3, 2, 3);
            select8.Name = "select8";
            select8.PlaceholderText = "（选择）";
            select8.Placement = AntdUI.TAlignFrom.TR;
            select8.Size = new Size(126, 41);
            select8.TabIndex = 2;
            // 
            // selectMultiple1
            // 
            selectMultiple1.Items.AddRange(new object[] { "Lucy", "Tom", "AduSkin", "WangLi", "HUAWEI", "XIAOMI" });
            selectMultiple1.Location = new Point(170, 20);
            selectMultiple1.Name = "selectMultiple1";
            selectMultiple1.Size = new Size(316, 41);
            selectMultiple1.TabIndex = 0;
            // 
            // divider3
            // 
            divider3.Dock = DockStyle.Top;
            divider3.Font = new Font("Microsoft YaHei UI", 10F);
            divider3.Location = new Point(0, 257);
            divider3.Name = "divider3";
            divider3.Orientation = AntdUI.TOrientation.Left;
            divider3.Size = new Size(555, 22);
            divider3.TabIndex = 4;
            divider3.TabStop = false;
            divider3.Text = "更多";
            // 
            // panel3
            // 
            panel3.Controls.Add(panel5);
            panel3.Controls.Add(panel8);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(0, 157);
            panel3.Name = "panel3";
            panel3.Size = new Size(555, 100);
            panel3.TabIndex = 3;
            // 
            // panel5
            // 
            panel5.Controls.Add(select7);
            panel5.Controls.Add(button1);
            panel5.Enabled = false;
            panel5.Location = new Point(266, 18);
            panel5.Name = "panel5";
            panel5.Size = new Size(220, 46);
            panel5.TabIndex = 3;
            panel5.Text = "panel4";
            // 
            // select7
            // 
            select7.AllowClear = true;
            select7.Dock = DockStyle.Fill;
            select7.Items.AddRange(new object[] { "Lucy", "Tom", "AduSkin", "WangLi", "HUAWEI", "XIAOMI" });
            select7.JoinRight = true;
            select7.Location = new Point(0, 0);
            select7.Name = "select7";
            select7.PlaceholderText = "输入点什么搜索";
            select7.Size = new Size(170, 46);
            select7.TabIndex = 0;
            // 
            // button1
            // 
            button1.Dock = DockStyle.Right;
            button1.IconSvg = "SearchOutlined";
            button1.JoinLeft = true;
            button1.Location = new Point(170, 0);
            button1.Name = "button1";
            button1.Size = new Size(50, 46);
            button1.TabIndex = 1;
            button1.Type = AntdUI.TTypeMini.Primary;
            // 
            // panel8
            // 
            panel8.Controls.Add(select6);
            panel8.Controls.Add(button4);
            panel8.Location = new Point(18, 18);
            panel8.Name = "panel8";
            panel8.Size = new Size(220, 46);
            panel8.TabIndex = 3;
            panel8.Text = "panel4";
            // 
            // select6
            // 
            select6.AllowClear = true;
            select6.Dock = DockStyle.Fill;
            select6.Items.AddRange(new object[] { "Lucy", "Tom", "AduSkin", "WangLi", "HUAWEI", "XIAOMI" });
            select6.JoinRight = true;
            select6.Location = new Point(0, 0);
            select6.Name = "select6";
            select6.PlaceholderText = "输入点什么搜索";
            select6.Size = new Size(170, 46);
            select6.TabIndex = 0;
            // 
            // button4
            // 
            button4.Dock = DockStyle.Right;
            button4.IconSvg = "SearchOutlined";
            button4.JoinLeft = true;
            button4.Location = new Point(170, 0);
            button4.Name = "button4";
            button4.Size = new Size(50, 46);
            button4.TabIndex = 1;
            button4.Type = AntdUI.TTypeMini.Primary;
            // 
            // divider2
            // 
            divider2.Dock = DockStyle.Top;
            divider2.Font = new Font("Microsoft YaHei UI", 10F);
            divider2.Location = new Point(0, 135);
            divider2.Name = "divider2";
            divider2.Orientation = AntdUI.TOrientation.Left;
            divider2.Size = new Size(555, 22);
            divider2.TabIndex = 2;
            divider2.TabStop = false;
            divider2.Text = "组合";
            // 
            // panel2
            // 
            panel2.Controls.Add(select2);
            panel2.Controls.Add(select4);
            panel2.Controls.Add(select3);
            panel2.Controls.Add(select5);
            panel2.Controls.Add(select1);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 22);
            panel2.Name = "panel2";
            panel2.Size = new Size(555, 113);
            panel2.TabIndex = 0;
            // 
            // select2
            // 
            select2.AllowClear = true;
            select2.Enabled = false;
            select2.Items.AddRange(new object[] { "Lucy", "Tom", "AduSkin", "WangLi", "HUAWEI", "XIAOMI" });
            select2.Location = new Point(142, 12);
            select2.Name = "select2";
            select2.SelectedIndex = 1;
            select2.SelectedValue = "Tom";
            select2.Size = new Size(120, 41);
            select2.TabIndex = 0;
            select2.Text = "Tom";
            // 
            // select4
            // 
            select4.AllowClear = true;
            select4.Items.AddRange(new object[] { "Lucy", "Tom", "AduSkin", "WangLi", "HUAWEI", "XIAOMI" });
            select4.List = true;
            select4.Location = new Point(390, 12);
            select4.Name = "select4";
            select4.PlaceholderText = "无文本";
            select4.Size = new Size(120, 41);
            select4.TabIndex = 0;
            // 
            // select3
            // 
            select3.AllowClear = true;
            select3.Items.AddRange(new object[] { "Lucy", "Tom", "AduSkin", "WangLi", "HUAWEI", "XIAOMI" });
            select3.Location = new Point(266, 12);
            select3.Name = "select3";
            select3.SelectedIndex = 1;
            select3.SelectedValue = "Tom";
            select3.Size = new Size(120, 41);
            select3.TabIndex = 0;
            select3.Text = "Tom";
            // 
            // select5
            // 
            select5.AllowClear = true;
            select5.DropDownArrow = true;
            select5.Items.AddRange(new object[] { "Lucy", "Tom", "AduSkin", "WangLi", "HUAWEI", "XIAOMI" });
            select5.Location = new Point(18, 59);
            select5.Name = "select5";
            select5.PlaceholderText = "带箭头的";
            select5.Size = new Size(120, 41);
            select5.TabIndex = 0;
            // 
            // select1
            // 
            select1.Items.AddRange(new object[] { "Lucy", "Tom", "AduSkin", "WangLi", "HUAWEI", "XIAOMI" });
            select1.Location = new Point(18, 12);
            select1.Name = "select1";
            select1.SelectedIndex = 1;
            select1.SelectedValue = "Tom";
            select1.Size = new Size(120, 41);
            select1.TabIndex = 0;
            select1.Text = "Tom";
            // 
            // divider1
            // 
            divider1.Dock = DockStyle.Top;
            divider1.Font = new Font("Microsoft YaHei UI", 10F);
            divider1.Location = new Point(0, 0);
            divider1.Name = "divider1";
            divider1.Orientation = AntdUI.TOrientation.Left;
            divider1.Size = new Size(555, 22);
            divider1.TabIndex = 1;
            divider1.TabStop = false;
            divider1.Text = "常规";
            // 
            // Select
            // 
            Controls.Add(panel1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 12F);
            Name = "Select";
            Size = new Size(555, 554);
            panel1.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel5.ResumeLayout(false);
            panel8.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private AntdUI.PageHeader header1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private AntdUI.Divider divider1;
        private AntdUI.Select select1;
        private AntdUI.Select select2;
        private AntdUI.Select select4;
        private AntdUI.Select select3;
        private AntdUI.Select select5;
        private System.Windows.Forms.Panel panel3;
        private AntdUI.Divider divider2;
        private System.Windows.Forms.Panel panel4;
        private AntdUI.Divider divider3;
        private System.Windows.Forms.Panel panel5;
        private AntdUI.Select select7;
        private AntdUI.Button button1;
        private System.Windows.Forms.Panel panel8;
        private AntdUI.Select select6;
        private AntdUI.Button button4;
        private AntdUI.Select select8;
        private AntdUI.SelectMultiple selectMultiple1;
    }
}