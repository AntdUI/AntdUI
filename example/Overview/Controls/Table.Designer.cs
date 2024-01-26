// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
// THE AntdUI PROJECT IS AN WINFORM LIBRARY LICENSED UNDER THE GPL-3.0 License.
// LICENSED UNDER THE GPL License, VERSION 3.0 (THE "License")
// YOU MAY NOT USE THIS FILE EXCEPT IN COMPLIANCE WITH THE License.
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
    partial class Table
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
            panel1 = new System.Windows.Forms.Panel();
            pagination1 = new AntdUI.Pagination();
            table2 = new AntdUI.Table();
            divider2 = new AntdUI.Divider();
            table1 = new AntdUI.Table();
            divider1 = new AntdUI.Divider();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Dock = DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new Padding(6);
            header1.Size = new Size(1300, 79);
            header1.TabIndex = 5;
            header1.Text = "Table 表格";
            header1.TextDesc = "展示行列数据。";
            // 
            // panel1
            // 
            panel1.AutoScroll = true;
            panel1.Controls.Add(pagination1);
            panel1.Controls.Add(table2);
            panel1.Controls.Add(divider2);
            panel1.Controls.Add(table1);
            panel1.Controls.Add(divider1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 79);
            panel1.Name = "panel1";
            panel1.Size = new Size(1300, 597);
            panel1.TabIndex = 6;
            // 
            // pagination1
            // 
            pagination1.Dock = DockStyle.Top;
            pagination1.Font = new Font("Microsoft YaHei UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            pagination1.Location = new Point(0, 644);
            pagination1.Name = "pagination1";
            pagination1.RightToLeft = RightToLeft.Yes;
            pagination1.ShowSizeChanger = true;
            pagination1.Size = new Size(1283, 34);
            pagination1.TabIndex = 5;
            pagination1.Total = 100;
            pagination1.ValueChanged += pagination1_ValueChanged;
            pagination1.ShowTotalChanged += pagination1_ShowTotalChanged;
            // 
            // table2
            // 
            table2.Dock = DockStyle.Top;
            table2.Font = new Font("Microsoft YaHei UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            table2.Location = new Point(0, 344);
            table2.Name = "table2";
            table2.Size = new Size(1283, 300);
            table2.TabIndex = 3;
            table2.Text = "table2";
            // 
            // divider2
            // 
            divider2.Dock = DockStyle.Top;
            divider2.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            divider2.Location = new Point(0, 322);
            divider2.Name = "divider2";
            divider2.Orientation = AntdUI.TOrientation.Left;
            divider2.Size = new Size(1283, 22);
            divider2.TabIndex = 4;
            divider2.TabStop = false;
            divider2.Text = "分页";
            // 
            // table1
            // 
            table1.Dock = DockStyle.Top;
            table1.Font = new Font("Microsoft YaHei UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            table1.Location = new Point(0, 22);
            table1.Name = "table1";
            table1.Size = new Size(1283, 300);
            table1.TabIndex = 0;
            table1.Text = "table1";
            table1.CellButtonClick += table1_CellButtonClick;
            // 
            // divider1
            // 
            divider1.Dock = DockStyle.Top;
            divider1.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            divider1.Location = new Point(0, 0);
            divider1.Name = "divider1";
            divider1.Orientation = AntdUI.TOrientation.Left;
            divider1.Size = new Size(1283, 22);
            divider1.TabIndex = 2;
            divider1.TabStop = false;
            divider1.Text = "常规";
            // 
            // Table
            // 
            Controls.Add(panel1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 16F, FontStyle.Regular, GraphicsUnit.Point);
            Name = "Table";
            Size = new Size(1300, 676);
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private AntdUI.Header header1;
        private System.Windows.Forms.Panel panel1;
        private AntdUI.Divider divider1;
        private AntdUI.Table table1;
        private AntdUI.Table table2;
        private AntdUI.Divider divider2;
        private AntdUI.Pagination pagination1;
    }
}