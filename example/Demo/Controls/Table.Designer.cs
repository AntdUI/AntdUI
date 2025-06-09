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

        private void InitializeComponent()
        {
            header1 = new AntdUI.PageHeader();
            pagination1 = new AntdUI.Pagination();
            table1 = new AntdUI.Table();
            panel1 = new AntdUI.Panel();
            checkAddressLineBreak = new AntdUI.Checkbox();
            checkVisibleHeader = new AntdUI.Checkbox();
            checkEnableHeaderResizing = new AntdUI.Checkbox();
            checkSortOrder = new AntdUI.Checkbox();
            checkSetRowStyle = new AntdUI.Checkbox();
            checkBordered = new AntdUI.Checkbox();
            checkRowsDragSort = new AntdUI.Checkbox();
            checkColumnDragSort = new AntdUI.Checkbox();
            checkFixedHeader = new AntdUI.Checkbox();
            panel_main = new AntdUI.Panel();
            panel1.SuspendLayout();
            panel_main.SuspendLayout();
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
            pagination1.Location = new Point(4, 562);
            pagination1.Name = "pagination1";
            pagination1.RightToLeft = RightToLeft.Yes;
            pagination1.ShowSizeChanger = true;
            pagination1.Size = new Size(1292, 40);
            pagination1.TabIndex = 3;
            pagination1.Total = 100;
            pagination1.ValueChanged += pagination1_ValueChanged;
            pagination1.ShowTotalChanged += pagination1_ShowTotalChanged;
            // 
            // table1
            // 
            table1.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            table1.CellImpactHeight = false;
            table1.Dock = DockStyle.Fill;
            table1.GapCell = 6;
            table1.Location = new Point(4, 43);
            table1.Name = "table1";
            table1.Radius = 6;
            table1.Size = new Size(1292, 519);
            table1.TabIndex = 2;
            table1.CellClick += table1_CellClick;
            table1.CellButtonClick += table1_CellButtonClick;
            // 
            // panel1
            // 
            panel1.Back = Color.Transparent;
            panel1.Controls.Add(checkAddressLineBreak);
            panel1.Controls.Add(checkVisibleHeader);
            panel1.Controls.Add(checkEnableHeaderResizing);
            panel1.Controls.Add(checkSortOrder);
            panel1.Controls.Add(checkSetRowStyle);
            panel1.Controls.Add(checkBordered);
            panel1.Controls.Add(checkRowsDragSort);
            panel1.Controls.Add(checkColumnDragSort);
            panel1.Controls.Add(checkFixedHeader);
            panel1.Dock = DockStyle.Top;
            panel1.Font = new Font("Microsoft YaHei UI", 10F);
            panel1.Location = new Point(4, 0);
            panel1.Name = "panel1";
            panel1.Radius = 0;
            panel1.Size = new Size(1292, 43);
            panel1.TabIndex = 1;
            // 
            // checkAddressLineBreak
            // 
            checkAddressLineBreak.AutoSizeMode = AntdUI.TAutoSize.Width;
            checkAddressLineBreak.Dock = DockStyle.Left;
            checkAddressLineBreak.LocalizationText = "Table.{id}";
            checkAddressLineBreak.Location = new Point(875, 0);
            checkAddressLineBreak.Name = "checkAddressLineBreak";
            checkAddressLineBreak.Size = new Size(99, 43);
            checkAddressLineBreak.TabIndex = 8;
            checkAddressLineBreak.Text = "地址换行";
            checkAddressLineBreak.CheckedChanged += checkAddressLineBreak_CheckedChanged;
            // 
            // checkVisibleHeader
            // 
            checkVisibleHeader.AutoSizeMode = AntdUI.TAutoSize.Width;
            checkVisibleHeader.Checked = true;
            checkVisibleHeader.Dock = DockStyle.Left;
            checkVisibleHeader.LocalizationText = "Table.{id}";
            checkVisibleHeader.Location = new Point(776, 0);
            checkVisibleHeader.Name = "checkVisibleHeader";
            checkVisibleHeader.Size = new Size(99, 43);
            checkVisibleHeader.TabIndex = 7;
            checkVisibleHeader.Text = "显示表头";
            checkVisibleHeader.CheckedChanged += checkVisibleHeader_CheckedChanged;
            // 
            // checkEnableHeaderResizing
            // 
            checkEnableHeaderResizing.AutoSizeMode = AntdUI.TAutoSize.Width;
            checkEnableHeaderResizing.Dock = DockStyle.Left;
            checkEnableHeaderResizing.LocalizationText = "Table.{id}";
            checkEnableHeaderResizing.Location = new Point(622, 0);
            checkEnableHeaderResizing.Name = "checkEnableHeaderResizing";
            checkEnableHeaderResizing.Size = new Size(154, 43);
            checkEnableHeaderResizing.TabIndex = 6;
            checkEnableHeaderResizing.Text = "手动调整列头宽度";
            checkEnableHeaderResizing.CheckedChanged += checkEnableHeaderResizing_CheckedChanged;
            // 
            // checkSortOrder
            // 
            checkSortOrder.AutoSizeMode = AntdUI.TAutoSize.Width;
            checkSortOrder.Dock = DockStyle.Left;
            checkSortOrder.LocalizationText = "Table.{id}";
            checkSortOrder.Location = new Point(523, 0);
            checkSortOrder.Name = "checkSortOrder";
            checkSortOrder.Size = new Size(99, 43);
            checkSortOrder.TabIndex = 5;
            checkSortOrder.Text = "年龄排序";
            checkSortOrder.CheckedChanged += checkSortOrder_CheckedChanged;
            // 
            // checkSetRowStyle
            // 
            checkSetRowStyle.AutoSizeMode = AntdUI.TAutoSize.Width;
            checkSetRowStyle.Dock = DockStyle.Left;
            checkSetRowStyle.LocalizationText = "Table.{id}";
            checkSetRowStyle.Location = new Point(438, 0);
            checkSetRowStyle.Name = "checkSetRowStyle";
            checkSetRowStyle.Size = new Size(85, 43);
            checkSetRowStyle.TabIndex = 4;
            checkSetRowStyle.Text = "奇偶列";
            checkSetRowStyle.CheckedChanged += checkSetRowStyle_CheckedChanged;
            // 
            // checkBordered
            // 
            checkBordered.AutoSizeMode = AntdUI.TAutoSize.Width;
            checkBordered.Dock = DockStyle.Left;
            checkBordered.LocalizationText = "Table.{id}";
            checkBordered.Location = new Point(325, 0);
            checkBordered.Name = "checkBordered";
            checkBordered.Size = new Size(113, 43);
            checkBordered.TabIndex = 3;
            checkBordered.Text = "显示列边框";
            checkBordered.CheckedChanged += checkBordered_CheckedChanged;
            // 
            // checkRowsDragSort
            // 
            checkRowsDragSort.AutoSizeMode = AntdUI.TAutoSize.Width;
            checkRowsDragSort.Dock = DockStyle.Left;
            checkRowsDragSort.LocalizationText = "Table.{id}";
            checkRowsDragSort.Location = new Point(212, 0);
            checkRowsDragSort.Name = "checkRowsDragSort";
            checkRowsDragSort.Size = new Size(113, 43);
            checkRowsDragSort.TabIndex = 2;
            checkRowsDragSort.Text = "行拖拽排序";
            checkRowsDragSort.CheckedChanged += checkRowsDragSort_CheckedChanged;
            // 
            // checkColumnDragSort
            // 
            checkColumnDragSort.AutoSizeMode = AntdUI.TAutoSize.Width;
            checkColumnDragSort.Dock = DockStyle.Left;
            checkColumnDragSort.LocalizationText = "Table.{id}";
            checkColumnDragSort.Location = new Point(99, 0);
            checkColumnDragSort.Name = "checkColumnDragSort";
            checkColumnDragSort.Size = new Size(113, 43);
            checkColumnDragSort.TabIndex = 1;
            checkColumnDragSort.Text = "列拖拽排序";
            checkColumnDragSort.CheckedChanged += checkColumnDragSort_CheckedChanged;
            // 
            // checkFixedHeader
            // 
            checkFixedHeader.AutoSizeMode = AntdUI.TAutoSize.Width;
            checkFixedHeader.Checked = true;
            checkFixedHeader.Dock = DockStyle.Left;
            checkFixedHeader.LocalizationText = "Table.{id}";
            checkFixedHeader.Location = new Point(0, 0);
            checkFixedHeader.Name = "checkFixedHeader";
            checkFixedHeader.Size = new Size(99, 43);
            checkFixedHeader.TabIndex = 0;
            checkFixedHeader.Text = "固定表头";
            checkFixedHeader.CheckedChanged += checkFixedHeader_CheckedChanged;
            // 
            // panel_main
            // 
            panel_main.Controls.Add(table1);
            panel_main.Controls.Add(pagination1);
            panel_main.Controls.Add(panel1);
            panel_main.Dock = DockStyle.Fill;
            panel_main.Location = new Point(0, 74);
            panel_main.Name = "panel_main";
            panel_main.Padding = new Padding(4, 0, 4, 0);
            panel_main.Radius = 0;
            panel_main.Size = new Size(1300, 602);
            panel_main.TabIndex = 4;
            panel_main.Text = "panel2";
            // 
            // Table
            // 
            Controls.Add(panel_main);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 12F);
            Name = "Table";
            Size = new Size(1300, 676);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel_main.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.PageHeader header1;
        private AntdUI.Table table1;
        private AntdUI.Pagination pagination1;
        private AntdUI.Panel panel1;
        private AntdUI.Checkbox checkFixedHeader;
        private AntdUI.Checkbox checkEnableHeaderResizing;
        private AntdUI.Checkbox checkColumnDragSort;
        private AntdUI.Checkbox checkRowsDragSort;
        private AntdUI.Checkbox checkSetRowStyle;
        private AntdUI.Checkbox checkBordered;
        private AntdUI.Checkbox checkSortOrder;
        private AntdUI.Checkbox checkVisibleHeader;
        private AntdUI.Panel panel_main;
        private AntdUI.Checkbox checkAddressLineBreak;
    }
}