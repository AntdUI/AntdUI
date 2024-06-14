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
            pagination1 = new AntdUI.Pagination();
            table2 = new AntdUI.Table();
            table1 = new AntdUI.Table();
            tabs1 = new AntdUI.Tabs();
            tabPage1 = new TabPage();
            panel1 = new AntdUI.Panel();
            checkbox5 = new AntdUI.Checkbox();
            checkbox4 = new AntdUI.Checkbox();
            checkbox3 = new AntdUI.Checkbox();
            checkbox2 = new AntdUI.Checkbox();
            checkbox1 = new AntdUI.Checkbox();
            tabPage2 = new TabPage();
            checkbox6 = new AntdUI.Checkbox();
            tabs1.SuspendLayout();
            tabPage1.SuspendLayout();
            panel1.SuspendLayout();
            tabPage2.SuspendLayout();
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
            // pagination1
            // 
            pagination1.Dock = DockStyle.Bottom;
            pagination1.Font = new Font("Microsoft YaHei UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            pagination1.Location = new Point(3, 30);
            pagination1.Name = "pagination1";
            pagination1.RightToLeft = RightToLeft.Yes;
            pagination1.ShowSizeChanger = true;
            pagination1.Size = new Size(186, 34);
            pagination1.TabIndex = 5;
            pagination1.Total = 100;
            pagination1.ValueChanged += pagination1_ValueChanged;
            pagination1.ShowTotalChanged += pagination1_ShowTotalChanged;
            // 
            // table2
            // 
            table2.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            table2.Dock = DockStyle.Fill;
            table2.Font = new Font("Microsoft YaHei UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            table2.Location = new Point(3, 3);
            table2.Name = "table2";
            table2.Size = new Size(186, 27);
            table2.TabIndex = 3;
            table2.Text = "table2";
            // 
            // table1
            // 
            table1.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            table1.Dock = DockStyle.Fill;
            table1.Font = new Font("Microsoft YaHei UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            table1.Location = new Point(3, 46);
            table1.Name = "table1";
            table1.Radius = 6;
            table1.Size = new Size(1286, 486);
            table1.TabIndex = 0;
            table1.Text = "table1";
            table1.CellButtonClick += table1_CellButtonClick;
            // 
            // tabs1
            // 
            tabs1.Appearance = TabAppearance.FlatButtons;
            tabs1.Controls.Add(tabPage1);
            tabs1.Controls.Add(tabPage2);
            tabs1.Dock = DockStyle.Fill;
            tabs1.ItemSize = new Size(80, 54);
            tabs1.Location = new Point(0, 79);
            tabs1.Name = "tabs1";
            tabs1.SelectedIndex = 0;
            tabs1.Size = new Size(1300, 597);
            tabs1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(table1);
            tabPage1.Controls.Add(panel1);
            tabPage1.Location = new Point(4, 58);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(1292, 535);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "常规";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.Controls.Add(checkbox2);
            panel1.Controls.Add(checkbox6);
            panel1.Controls.Add(checkbox5);
            panel1.Controls.Add(checkbox4);
            panel1.Controls.Add(checkbox3);
            panel1.Controls.Add(checkbox1);
            panel1.Dock = DockStyle.Top;
            panel1.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Padding = new Padding(10, 0, 0, 0);
            panel1.Size = new Size(1286, 43);
            panel1.TabIndex = 1;
            panel1.Text = "panel1";
            // 
            // checkbox5
            // 
            checkbox5.Dock = DockStyle.Left;
            checkbox5.Location = new Point(386, 0);
            checkbox5.Name = "checkbox5";
            checkbox5.Size = new Size(97, 43);
            checkbox5.TabIndex = 4;
            checkbox5.Text = "奇偶列";
            checkbox5.CheckedChanged += checkbox5_CheckedChanged;
            // 
            // checkbox4
            // 
            checkbox4.Dock = DockStyle.Left;
            checkbox4.Location = new Point(259, 0);
            checkbox4.Name = "checkbox4";
            checkbox4.Size = new Size(127, 43);
            checkbox4.TabIndex = 3;
            checkbox4.Text = "显示列边框";
            checkbox4.CheckedChanged += checkbox4_CheckedChanged;
            // 
            // checkbox3
            // 
            checkbox3.Dock = DockStyle.Left;
            checkbox3.Location = new Point(126, 0);
            checkbox3.Name = "checkbox3";
            checkbox3.Size = new Size(133, 43);
            checkbox3.TabIndex = 2;
            checkbox3.Text = "列拖拽排序";
            checkbox3.CheckedChanged += checkbox3_CheckedChanged;
            // 
            // checkbox2
            // 
            checkbox2.Dock = DockStyle.Left;
            checkbox2.Location = new Point(599, 0);
            checkbox2.Name = "checkbox2";
            checkbox2.Size = new Size(178, 43);
            checkbox2.TabIndex = 1;
            checkbox2.Text = "手动调整列头宽度";
            checkbox2.CheckedChanged += checkbox2_CheckedChanged;
            // 
            // checkbox1
            // 
            checkbox1.Checked = true;
            checkbox1.Dock = DockStyle.Left;
            checkbox1.Location = new Point(10, 0);
            checkbox1.Name = "checkbox1";
            checkbox1.Size = new Size(116, 43);
            checkbox1.TabIndex = 0;
            checkbox1.Text = "固定表头";
            checkbox1.CheckedChanged += checkbox1_CheckedChanged;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(table2);
            tabPage2.Controls.Add(pagination1);
            tabPage2.Location = new Point(4, 29);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(192, 67);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "分页";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // checkbox6
            // 
            checkbox6.Dock = DockStyle.Left;
            checkbox6.Location = new Point(483, 0);
            checkbox6.Name = "checkbox6";
            checkbox6.Size = new Size(116, 43);
            checkbox6.TabIndex = 5;
            checkbox6.Text = "年龄排序";
            checkbox6.CheckedChanged += checkbox6_CheckedChanged;
            // 
            // Table
            // 
            Controls.Add(tabs1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 16F, FontStyle.Regular, GraphicsUnit.Point);
            Name = "Table";
            Size = new Size(1300, 676);
            tabs1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private AntdUI.Header header1;
        private AntdUI.Table table1;
        private AntdUI.Table table2;
        private AntdUI.Pagination pagination1;
        private AntdUI.Tabs tabs1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private AntdUI.Panel panel1;
        private AntdUI.Checkbox checkbox1;
        private AntdUI.Checkbox checkbox2;
        private AntdUI.Checkbox checkbox3;
        private AntdUI.Checkbox checkbox5;
        private AntdUI.Checkbox checkbox4;
        private AntdUI.Checkbox checkbox6;
    }
}