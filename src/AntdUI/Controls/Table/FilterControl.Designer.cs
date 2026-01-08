// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

namespace AntdUI
{
    partial class FilterControl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            dv = new Table();
            inputSearch = new Input();
            selectCondition = new Select();
            tablePanel = new System.Windows.Forms.TableLayoutPanel();
            btnPanel = new System.Windows.Forms.TableLayoutPanel();
            btn_ok = new Button();
            btn_clean = new Button();
            tablePanel.SuspendLayout();
            btnPanel.SuspendLayout();
            SuspendLayout();
            // 
            // dv
            // 
            dv.AutoSizeColumnsMode = ColumnsMode.Fill;
            dv.Dock = System.Windows.Forms.DockStyle.Fill;
            dv.Gaps = new System.Drawing.Size(4, 8);
            dv.Location = new System.Drawing.Point(0, 66);
            dv.Margin = new System.Windows.Forms.Padding(0);
            dv.Name = "dv";
            dv.Size = new System.Drawing.Size(200, 200);
            dv.TabIndex = 2;
            dv.CheckedChanged += dv_CheckedChanged;
            // 
            // inputSearch
            // 
            inputSearch.AllowClear = true;
            inputSearch.BackColor = System.Drawing.Color.Transparent;
            inputSearch.Dock = System.Windows.Forms.DockStyle.Top;
            inputSearch.Location = new System.Drawing.Point(0, 34);
            inputSearch.Margin = new System.Windows.Forms.Padding(0);
            inputSearch.Name = "inputSearch";
            inputSearch.Radius = 0;
            inputSearch.Size = new System.Drawing.Size(200, 32);
            inputSearch.TabIndex = 1;
            inputSearch.Variant = TVariant.Underlined;
            inputSearch.WaveSize = 0;
            inputSearch.TextChanged += inputSearch_TextChanged;
            // 
            // selectCondition
            // 
            selectCondition.AutoPrefixSvg = true;
            selectCondition.AutoText = false;
            selectCondition.Dock = System.Windows.Forms.DockStyle.Fill;
            selectCondition.IconRatio = 0.8F;
            selectCondition.List = true;
            selectCondition.ListAutoWidth = true;
            selectCondition.Location = new System.Drawing.Point(0, 0);
            selectCondition.Margin = new System.Windows.Forms.Padding(0);
            selectCondition.MaxCount = 0;
            selectCondition.Name = "selectCondition";
            selectCondition.ShowIcon = false;
            selectCondition.Size = new System.Drawing.Size(34, 34);
            selectCondition.TabIndex = 3;
            // 
            // tablePanel
            // 
            tablePanel.ColumnCount = 2;
            tablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            tablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tablePanel.Controls.Add(selectCondition, 0, 0);
            tablePanel.Dock = System.Windows.Forms.DockStyle.Top;
            tablePanel.Location = new System.Drawing.Point(0, 0);
            tablePanel.Name = "tablePanel";
            tablePanel.RowCount = 1;
            tablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tablePanel.Size = new System.Drawing.Size(200, 34);
            tablePanel.TabIndex = 5;
            // 
            // btnPanel
            // 
            btnPanel.ColumnCount = 2;
            btnPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            btnPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            btnPanel.Controls.Add(btn_ok, 1, 0);
            btnPanel.Controls.Add(btn_clean, 0, 0);
            btnPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            btnPanel.Location = new System.Drawing.Point(0, 266);
            btnPanel.Name = "btnPanel";
            btnPanel.RowCount = 1;
            btnPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            btnPanel.Size = new System.Drawing.Size(200, 34);
            btnPanel.TabIndex = 3;
            // 
            // btn_ok
            // 
            btn_ok.Dock = System.Windows.Forms.DockStyle.Fill;
            btn_ok.LocalizationText = "OK";
            btn_ok.Location = new System.Drawing.Point(100, 0);
            btn_ok.Margin = new System.Windows.Forms.Padding(0);
            btn_ok.Name = "btn_ok";
            btn_ok.Size = new System.Drawing.Size(100, 34);
            btn_ok.TabIndex = 0;
            btn_ok.Text = "确定";
            btn_ok.Type = TTypeMini.Primary;
            btn_ok.Click += btn_ok_Click;
            // 
            // btn_clean
            // 
            btn_clean.Dock = System.Windows.Forms.DockStyle.Fill;
            btn_clean.Enabled = false;
            btn_clean.Ghost = true;
            btn_clean.LocalizationText = "Filter.Clean";
            btn_clean.Location = new System.Drawing.Point(0, 0);
            btn_clean.Margin = new System.Windows.Forms.Padding(0);
            btn_clean.Name = "btn_clean";
            btn_clean.Size = new System.Drawing.Size(100, 34);
            btn_clean.TabIndex = 1;
            btn_clean.Text = "重置";
            btn_clean.Type = TTypeMini.Primary;
            btn_clean.Click += btn_clean_Click;
            // 
            // FilterControl
            // 
            Controls.Add(dv);
            Controls.Add(inputSearch);
            Controls.Add(tablePanel);
            Controls.Add(btnPanel);
            Name = "FilterControl";
            Size = new System.Drawing.Size(200, 300);
            tablePanel.ResumeLayout(false);
            btnPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Select selectCondition;
        private Input inputSearch;
        private Table dv;
        private System.Windows.Forms.TableLayoutPanel tablePanel;
        private System.Windows.Forms.TableLayoutPanel btnPanel;
        private Button btn_ok;
        private Button btn_clean;
    }
}