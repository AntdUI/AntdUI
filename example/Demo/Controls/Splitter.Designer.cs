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
    partial class Splitter
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
            tableLayoutPanel1 = new TableLayoutPanel();
            splitter1 = new AntdUI.Splitter();
            label1 = new AntdUI.Label();
            label2 = new AntdUI.Label();
            splitter2 = new AntdUI.Splitter();
            label3 = new AntdUI.Label();
            label4 = new AntdUI.Label();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitter1).BeginInit();
            splitter1.Panel1.SuspendLayout();
            splitter1.Panel2.SuspendLayout();
            splitter1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitter2).BeginInit();
            splitter2.Panel1.SuspendLayout();
            splitter2.Panel2.SuspendLayout();
            splitter2.SuspendLayout();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Description = "自由切分指定区域。";
            header1.Dock = DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F);
            header1.LocalizationDescription = "Splitter.Description";
            header1.LocalizationText = "Splitter.Text";
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new Padding(0, 0, 0, 10);
            header1.Size = new Size(616, 74);
            header1.TabIndex = 0;
            header1.Text = "Splitter 分隔面板";
            header1.UseTitleFont = true;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(splitter1, 0, 0);
            tableLayoutPanel1.Controls.Add(splitter2, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 74);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(616, 511);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // splitter1
            // 
            splitter1.Dock = DockStyle.Fill;
            splitter1.Lazy = false;
            splitter1.Location = new Point(3, 3);
            splitter1.Name = "splitter1";
            // 
            // splitter1.Panel1
            // 
            splitter1.Panel1.Controls.Add(label1);
            // 
            // splitter1.Panel2
            // 
            splitter1.Panel2.Controls.Add(label2);
            splitter1.Size = new Size(610, 249);
            splitter1.SplitterDistance = 290;
            splitter1.SplitterWidth = 2;
            splitter1.TabIndex = 1;
            // 
            // label1
            // 
            label1.Dock = DockStyle.Fill;
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(290, 249);
            label1.TabIndex = 0;
            label1.Text = "First";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            label2.Dock = DockStyle.Fill;
            label2.Location = new Point(0, 0);
            label2.Name = "label2";
            label2.Size = new Size(318, 249);
            label2.TabIndex = 0;
            label2.Text = "Second";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // splitter2
            // 
            splitter2.CollapsePanel = AntdUI.Splitter.ADCollapsePanel.Panel1;
            splitter2.Dock = DockStyle.Fill;
            splitter2.Location = new Point(3, 258);
            splitter2.Name = "splitter2";
            // 
            // splitter2.Panel1
            // 
            splitter2.Panel1.Controls.Add(label3);
            splitter2.Panel1MinSize = 0;
            // 
            // splitter2.Panel2
            // 
            splitter2.Panel2.Controls.Add(label4);
            splitter2.Panel2MinSize = 0;
            splitter2.Size = new Size(610, 250);
            splitter2.SplitterDistance = 290;
            splitter2.SplitterSize = 80;
            splitter2.SplitterWidth = 10;
            splitter2.TabIndex = 2;
            // 
            // label3
            // 
            label3.Dock = DockStyle.Fill;
            label3.Location = new Point(0, 0);
            label3.Name = "label3";
            label3.Size = new Size(290, 250);
            label3.TabIndex = 0;
            label3.Text = "First";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            label4.Dock = DockStyle.Fill;
            label4.Location = new Point(0, 0);
            label4.Name = "label4";
            label4.Size = new Size(310, 250);
            label4.TabIndex = 0;
            label4.Text = "Second";
            label4.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Splitter
            // 
            Controls.Add(tableLayoutPanel1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 12F);
            Name = "Splitter";
            Size = new Size(616, 585);
            tableLayoutPanel1.ResumeLayout(false);
            splitter1.Panel1.ResumeLayout(false);
            splitter1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitter1).EndInit();
            splitter1.ResumeLayout(false);
            splitter2.Panel1.ResumeLayout(false);
            splitter2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitter2).EndInit();
            splitter2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.PageHeader header1;
        private TableLayoutPanel tableLayoutPanel1;
        private AntdUI.Splitter splitter1;
        private AntdUI.Label label1;
        private AntdUI.Label label2;
        private AntdUI.Splitter splitter2;
        private AntdUI.Label label3;
        private AntdUI.Label label4;
    }
}