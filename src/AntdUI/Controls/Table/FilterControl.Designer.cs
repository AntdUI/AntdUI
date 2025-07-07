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
            gridPanel1 = new GridPanel();
            treeList = new Tree();
            inputSearch = new Input();
            flowPanelConditionEdit = new System.Windows.Forms.Panel();
            selectCondition = new Select();
            gridPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // segmentedSource
            // 
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
            segmentedSource.Location = new System.Drawing.Point(3, 3);
            segmentedSource.Name = "segmentedSource";
            segmentedSource.SelectIndex = 2;
            segmentedSource.Size = new System.Drawing.Size(289, 50);
            segmentedSource.TabIndex = 4;
            segmentedSource.Text = "segmented2";
            // 
            // gridPanel1
            // 
            gridPanel1.Controls.Add(treeList);
            gridPanel1.Controls.Add(inputSearch);
            gridPanel1.Controls.Add(flowPanelConditionEdit);
            gridPanel1.Controls.Add(selectCondition);
            gridPanel1.Controls.Add(segmentedSource);
            gridPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            gridPanel1.Location = new System.Drawing.Point(4, 4);
            gridPanel1.Name = "gridPanel1";
            gridPanel1.Size = new System.Drawing.Size(295, 382);
            gridPanel1.Span = "100%;20% 80%;100%;100%;- 56 48 48 100%";
            gridPanel1.TabIndex = 0;
            gridPanel1.Text = "gridPanel1";
            // 
            // treeList
            // 
            treeList.Checkable = true;
            treeItem1.Checked = true;
            treeItem1.CheckState = System.Windows.Forms.CheckState.Checked;
            treeItem1.Select = true;
            treeItem1.Text = "全选";
            treeList.Items.Add(treeItem1);
            treeList.Location = new System.Drawing.Point(3, 155);
            treeList.Multiple = true;
            treeList.Name = "treeList";
            treeList.SelectItem = treeItem1;
            treeList.Size = new System.Drawing.Size(289, 224);
            treeList.TabIndex = 2;
            treeList.Text = "tree1";
            treeList.CheckedChanged += treeList_CheckedChanged;
            // 
            // inputSearch
            // 
            inputSearch.AllowClear = true;
            inputSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            inputSearch.Location = new System.Drawing.Point(3, 107);
            inputSearch.Name = "inputSearch";
            inputSearch.PlaceholderText = "搜索";
            inputSearch.Size = new System.Drawing.Size(289, 42);
            inputSearch.TabIndex = 1;
            inputSearch.TextChanged += inputSearch_TextChanged;
            // 
            // flowPanelConditionEdit
            // 
            flowPanelConditionEdit.BackColor = System.Drawing.Color.Transparent;
            flowPanelConditionEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            flowPanelConditionEdit.Location = new System.Drawing.Point(62, 59);
            flowPanelConditionEdit.Name = "flowPanelConditionEdit";
            flowPanelConditionEdit.Size = new System.Drawing.Size(230, 42);
            flowPanelConditionEdit.TabIndex = 0;
            flowPanelConditionEdit.Text = "flowPanel1";
            // 
            // selectCondition
            // 
            selectCondition.Dock = System.Windows.Forms.DockStyle.Fill;
            selectCondition.IconRatio = 1.25F;
            selectCondition.IconRatioRight = 0F;
            selectCondition.List = true;
            selectCondition.ListAutoWidth = true;
            selectCondition.Location = new System.Drawing.Point(3, 59);
            selectCondition.MaxCount = 8;
            selectCondition.Name = "selectCondition";
            selectCondition.Size = new System.Drawing.Size(53, 42);
            selectCondition.TabIndex = 3;
            // 
            // FilterControl
            // 
            Controls.Add(gridPanel1);
            Name = "FilterControl";
            Size = new System.Drawing.Size(303, 390);
            gridPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Segmented segmentedSource;
        private GridPanel gridPanel1;
        private System.Windows.Forms.Panel flowPanelConditionEdit;
        private Select selectCondition;
        private Input inputSearch;
        private Tree treeList;
    }
}