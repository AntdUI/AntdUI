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
            SegmentedItem segmentedItem1 = new SegmentedItem();
            SegmentedItem segmentedItem2 = new SegmentedItem();
            SegmentedItem segmentedItem3 = new SegmentedItem();
            SegmentedItem segmentedItem4 = new SegmentedItem();
            TreeItem treeItem1 = new TreeItem();
            segmentedSource = new Segmented();
            treeList = new Tree();
            inputSearch = new Input();
            selectCondition = new Select();
            tablePanel = new System.Windows.Forms.TableLayoutPanel();
            tablePanel.SuspendLayout();
            SuspendLayout();
            // 
            // segmentedSource
            // 
            segmentedSource.Dock = System.Windows.Forms.DockStyle.Top;
            segmentedSource.Full = true;
            segmentedSource.IconRatio = 1F;
            segmentedItem1.Badge = null;
            segmentedItem1.BadgeAlign = TAlign.TR;
            segmentedItem1.BadgeBack = null;
            segmentedItem1.BadgeMode = false;
            segmentedItem1.BadgeOffsetX = 0;
            segmentedItem1.BadgeOffsetY = 0;
            segmentedItem1.BadgeSize = 0.6F;
            segmentedItem1.BadgeSvg = null;
            segmentedItem1.Enabled = false;
            segmentedItem1.Text = "清除";
            segmentedItem2.Badge = null;
            segmentedItem2.BadgeAlign = TAlign.TR;
            segmentedItem2.BadgeBack = null;
            segmentedItem2.BadgeMode = false;
            segmentedItem2.BadgeOffsetX = 0;
            segmentedItem2.BadgeOffsetY = 0;
            segmentedItem2.BadgeSize = 0.6F;
            segmentedItem2.BadgeSvg = null;
            segmentedItem2.IconSvg = "FilterOutlined";
            segmentedItem2.Text = "按视图";
            segmentedItem3.Badge = null;
            segmentedItem3.BadgeAlign = TAlign.TR;
            segmentedItem3.BadgeBack = null;
            segmentedItem3.BadgeMode = false;
            segmentedItem3.BadgeOffsetX = 0;
            segmentedItem3.BadgeOffsetY = 0;
            segmentedItem3.BadgeSize = 0.6F;
            segmentedItem3.BadgeSvg = null;
            segmentedItem3.IconSvg = "FunnelPlotOutlined";
            segmentedItem3.Text = "自动";
            segmentedItem4.Badge = null;
            segmentedItem4.BadgeAlign = TAlign.TR;
            segmentedItem4.BadgeBack = null;
            segmentedItem4.BadgeMode = false;
            segmentedItem4.BadgeOffsetX = 0;
            segmentedItem4.BadgeOffsetY = 0;
            segmentedItem4.BadgeSize = 0.6F;
            segmentedItem4.BadgeSvg = null;
            segmentedItem4.IconSvg = "TableOutlined";
            segmentedItem4.Text = "数据源";
            segmentedSource.Items.Add(segmentedItem1);
            segmentedSource.Items.Add(segmentedItem2);
            segmentedSource.Items.Add(segmentedItem3);
            segmentedSource.Items.Add(segmentedItem4);
            segmentedSource.Location = new System.Drawing.Point(0, 0);
            segmentedSource.Margin = new System.Windows.Forms.Padding(2);
            segmentedSource.Name = "segmentedSource";
            segmentedSource.SelectIndex = 2;
            segmentedSource.Size = new System.Drawing.Size(180, 40);
            segmentedSource.TabIndex = 4;
            // 
            // treeList
            // 
            treeList.Checkable = true;
            treeList.Dock = System.Windows.Forms.DockStyle.Fill;
            treeItem1.Checked = true;
            treeItem1.CheckState = System.Windows.Forms.CheckState.Checked;
            treeItem1.Select = true;
            treeItem1.Text = "全选";
            treeList.Items.Add(treeItem1);
            treeList.Location = new System.Drawing.Point(0, 108);
            treeList.Margin = new System.Windows.Forms.Padding(0);
            treeList.Multiple = true;
            treeList.Name = "treeList";
            treeList.SelectItem = treeItem1;
            treeList.Size = new System.Drawing.Size(180, 132);
            treeList.TabIndex = 2;
            treeList.Text = "tree1";
            treeList.CheckedChanged += treeList_CheckedChanged;
            // 
            // inputSearch
            // 
            inputSearch.AllowClear = true;
            inputSearch.Dock = System.Windows.Forms.DockStyle.Top;
            inputSearch.Location = new System.Drawing.Point(0, 74);
            inputSearch.Margin = new System.Windows.Forms.Padding(0);
            inputSearch.Name = "inputSearch";
            inputSearch.PlaceholderText = "搜索";
            inputSearch.Size = new System.Drawing.Size(180, 34);
            inputSearch.TabIndex = 1;
            inputSearch.TextChanged += inputSearch_TextChanged;
            // 
            // selectCondition
            // 
            selectCondition.IconRatio = 1F;
            selectCondition.IconRatioRight = 0F;
            selectCondition.List = true;
            selectCondition.ListAutoWidth = true;
            selectCondition.Location = new System.Drawing.Point(0, 0);
            selectCondition.Margin = new System.Windows.Forms.Padding(0);
            selectCondition.MaxCount = 8;
            selectCondition.Name = "selectCondition";
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
            tablePanel.Location = new System.Drawing.Point(0, 40);
            tablePanel.Name = "tablePanel";
            tablePanel.RowCount = 1;
            tablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tablePanel.Size = new System.Drawing.Size(180, 34);
            tablePanel.TabIndex = 5;
            // 
            // FilterControl
            // 
            Controls.Add(treeList);
            Controls.Add(inputSearch);
            Controls.Add(tablePanel);
            Controls.Add(segmentedSource);
            Name = "FilterControl";
            Size = new System.Drawing.Size(180, 240);
            tablePanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Segmented segmentedSource;
        private Select selectCondition;
        private Input inputSearch;
        private Tree treeList;
        private System.Windows.Forms.TableLayoutPanel tablePanel;
    }
}