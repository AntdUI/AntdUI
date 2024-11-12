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

namespace Demo.Controls
{
    partial class TableAOT
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
            AntdUI.Tabs.StyleLine styleLine1 = new AntdUI.Tabs.StyleLine();
            header1 = new AntdUI.PageHeader();
            pagination1 = new AntdUI.Pagination();
            table2 = new AntdUI.Table();
            table1 = new AntdUI.Table();
            tabs1 = new AntdUI.Tabs();
            tabPage1 = new AntdUI.TabPage();
            panel1 = new AntdUI.Panel();
            checkVisibleHeader = new AntdUI.Checkbox();
            checkEnableHeaderResizing = new AntdUI.Checkbox();
            checkSortOrder = new AntdUI.Checkbox();
            checkSetRowStyle = new AntdUI.Checkbox();
            checkBordered = new AntdUI.Checkbox();
            checkColumnDragSort = new AntdUI.Checkbox();
            checkFixedHeader = new AntdUI.Checkbox();
            tabPage2 = new AntdUI.TabPage();
            tabs1.SuspendLayout();
            tabPage1.SuspendLayout();
            panel1.SuspendLayout();
            tabPage2.SuspendLayout();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Description = "展示行列数据。";
            header1.Dock = DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F);
            header1.LocalizationDescription = "Table.Description";
            header1.LocalizationText = "Table.Text";
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new Padding(0, 0, 0, 10);
            header1.Size = new Size(1300, 74);
            header1.TabIndex = 0;
            header1.Text = "Table 表格";
            header1.UseTitleFont = true;
            // 
            // pagination1
            // 
            pagination1.Dock = DockStyle.Bottom;
            pagination1.Font = new Font("Microsoft YaHei UI", 11F);
            pagination1.Location = new Point(3, 509);
            pagination1.Name = "pagination1";
            pagination1.RightToLeft = RightToLeft.Yes;
            pagination1.ShowSizeChanger = true;
            pagination1.Size = new Size(1288, 40);
            pagination1.TabIndex = 5;
            pagination1.Total = 100;
            pagination1.ValueChanged += pagination1_ValueChanged;
            pagination1.ShowTotalChanged += pagination1_ShowTotalChanged;
            // 
            // table2
            // 
            table2.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            table2.Dock = DockStyle.Fill;
            table2.Font = new Font("Microsoft YaHei UI", 11F);
            table2.Location = new Point(3, 3);
            table2.Name = "table2";
            table2.Size = new Size(1288, 506);
            table2.TabIndex = 3;
            table2.Text = "table2";
            // 
            // table1
            // 
            table1.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            table1.Dock = DockStyle.Fill;
            table1.Font = new Font("Microsoft YaHei UI", 11F);
            table1.Location = new Point(3, 46);
            table1.Name = "table1";
            table1.Radius = 6;
            table1.Size = new Size(1288, 508);
            table1.TabIndex = 0;
            table1.CellClick += table1_CellClick;
            table1.CellButtonClick += table1_CellButtonClick;
            // 
            // tabs1
            // 
            tabs1.Dock = DockStyle.Fill;
            tabs1.Font = new Font("Microsoft YaHei UI", 12F);
            tabs1.Gap = 12;
            tabs1.Location = new Point(0, 74);
            tabs1.Name = "tabs1";
            tabs1.Padding = new Padding(0, 4, 0, 0);
            tabs1.Pages.Add(tabPage1);
            tabs1.Pages.Add(tabPage2);
            tabs1.Size = new Size(1300, 602);
            styleLine1.Radius = 2;
            tabs1.Style = styleLine1;
            tabs1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(table1);
            tabPage1.Controls.Add(panel1);
            tabPage1.Dock = DockStyle.Fill;
            tabPage1.Location = new Point(3, 42);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(1294, 557);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "常规";
            // 
            // panel1
            // 
            panel1.Controls.Add(checkVisibleHeader);
            panel1.Controls.Add(checkEnableHeaderResizing);
            panel1.Controls.Add(checkSortOrder);
            panel1.Controls.Add(checkSetRowStyle);
            panel1.Controls.Add(checkBordered);
            panel1.Controls.Add(checkColumnDragSort);
            panel1.Controls.Add(checkFixedHeader);
            panel1.Dock = DockStyle.Top;
            panel1.Font = new Font("Microsoft YaHei UI", 12F);
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Padding = new Padding(10, 0, 0, 0);
            panel1.Size = new Size(1288, 43);
            panel1.TabIndex = 1;
            panel1.Text = "panel1";
            // 
            // checkVisibleHeader
            // 
            checkVisibleHeader.AutoCheck = true;
            checkVisibleHeader.AutoSizeMode = AntdUI.TAutoSize.Width;
            checkVisibleHeader.Checked = true;
            checkVisibleHeader.Dock = DockStyle.Left;
            checkVisibleHeader.LocalizationText = "Table.{id}";
            checkVisibleHeader.Location = new Point(781, 0);
            checkVisibleHeader.Name = "checkVisibleHeader";
            checkVisibleHeader.Size = new Size(115, 43);
            checkVisibleHeader.TabIndex = 6;
            checkVisibleHeader.Text = "显示表头";
            checkVisibleHeader.CheckedChanged += checkVisibleHeader_CheckedChanged;
            // 
            // checkEnableHeaderResizing
            // 
            checkEnableHeaderResizing.AutoCheck = true;
            checkEnableHeaderResizing.AutoSizeMode = AntdUI.TAutoSize.Width;
            checkEnableHeaderResizing.Dock = DockStyle.Left;
            checkEnableHeaderResizing.LocalizationText = "Table.{id}";
            checkEnableHeaderResizing.Location = new Point(600, 0);
            checkEnableHeaderResizing.Name = "checkEnableHeaderResizing";
            checkEnableHeaderResizing.Size = new Size(181, 43);
            checkEnableHeaderResizing.TabIndex = 5;
            checkEnableHeaderResizing.Text = "手动调整列头宽度";
            checkEnableHeaderResizing.CheckedChanged += checkEnableHeaderResizing_CheckedChanged;
            // 
            // checkSortOrder
            // 
            checkSortOrder.AutoCheck = true;
            checkSortOrder.AutoSizeMode = AntdUI.TAutoSize.Width;
            checkSortOrder.Dock = DockStyle.Left;
            checkSortOrder.LocalizationText = "Table.{id}";
            checkSortOrder.Location = new Point(485, 0);
            checkSortOrder.Name = "checkSortOrder";
            checkSortOrder.Size = new Size(115, 43);
            checkSortOrder.TabIndex = 4;
            checkSortOrder.Text = "年龄排序";
            checkSortOrder.CheckedChanged += checkSortOrder_CheckedChanged;
            // 
            // checkSetRowStyle
            // 
            checkSetRowStyle.AutoCheck = true;
            checkSetRowStyle.AutoSizeMode = AntdUI.TAutoSize.Width;
            checkSetRowStyle.Dock = DockStyle.Left;
            checkSetRowStyle.LocalizationText = "Table.{id}";
            checkSetRowStyle.Location = new Point(387, 0);
            checkSetRowStyle.Name = "checkSetRowStyle";
            checkSetRowStyle.Size = new Size(98, 43);
            checkSetRowStyle.TabIndex = 3;
            checkSetRowStyle.Text = "奇偶列";
            checkSetRowStyle.CheckedChanged += checkSetRowStyle_CheckedChanged;
            // 
            // checkBordered
            // 
            checkBordered.AutoCheck = true;
            checkBordered.AutoSizeMode = AntdUI.TAutoSize.Width;
            checkBordered.Dock = DockStyle.Left;
            checkBordered.LocalizationText = "Table.{id}";
            checkBordered.Location = new Point(256, 0);
            checkBordered.Name = "checkBordered";
            checkBordered.Size = new Size(131, 43);
            checkBordered.TabIndex = 2;
            checkBordered.Text = "显示列边框";
            checkBordered.CheckedChanged += checkBordered_CheckedChanged;
            // 
            // checkColumnDragSort
            // 
            checkColumnDragSort.AutoCheck = true;
            checkColumnDragSort.AutoSizeMode = AntdUI.TAutoSize.Width;
            checkColumnDragSort.Dock = DockStyle.Left;
            checkColumnDragSort.LocalizationText = "Table.{id}";
            checkColumnDragSort.Location = new Point(125, 0);
            checkColumnDragSort.Name = "checkColumnDragSort";
            checkColumnDragSort.Size = new Size(131, 43);
            checkColumnDragSort.TabIndex = 1;
            checkColumnDragSort.Text = "列拖拽排序";
            checkColumnDragSort.CheckedChanged += checkColumnDragSort_CheckedChanged;
            // 
            // checkFixedHeader
            // 
            checkFixedHeader.AutoCheck = true;
            checkFixedHeader.AutoSizeMode = AntdUI.TAutoSize.Width;
            checkFixedHeader.Checked = true;
            checkFixedHeader.Dock = DockStyle.Left;
            checkFixedHeader.LocalizationText = "Table.{id}";
            checkFixedHeader.Location = new Point(10, 0);
            checkFixedHeader.Name = "checkFixedHeader";
            checkFixedHeader.Size = new Size(115, 43);
            checkFixedHeader.TabIndex = 0;
            checkFixedHeader.Text = "固定表头";
            checkFixedHeader.CheckedChanged += checkFixedHeader_CheckedChanged;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(table2);
            tabPage2.Controls.Add(pagination1);
            tabPage2.Dock = DockStyle.Fill;
            tabPage2.Location = new Point(3, 42);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(1294, 552);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "分页";
            // 
            // TableAOT
            // 
            Controls.Add(tabs1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 16F);
            Name = "TableAOT";
            Size = new Size(1300, 676);
            tabs1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            tabPage2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private AntdUI.PageHeader header1;
        private AntdUI.Table table1;
        private AntdUI.Table table2;
        private AntdUI.Pagination pagination1;
        private AntdUI.Tabs tabs1;
        private AntdUI.TabPage tabPage1;
        private AntdUI.TabPage tabPage2;
        private AntdUI.Panel panel1;
        private AntdUI.Checkbox checkFixedHeader;
        private AntdUI.Checkbox checkEnableHeaderResizing;
        private AntdUI.Checkbox checkColumnDragSort;
        private AntdUI.Checkbox checkSetRowStyle;
        private AntdUI.Checkbox checkBordered;
        private AntdUI.Checkbox checkSortOrder;
        private AntdUI.Checkbox checkVisibleHeader;
    }
}