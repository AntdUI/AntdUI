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
    partial class Icon
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
            AntdUI.SegmentedItem segmentedItem1 = new AntdUI.SegmentedItem();
            AntdUI.SegmentedItem segmentedItem2 = new AntdUI.SegmentedItem();
            header1 = new AntdUI.PageHeader();
            segmented1 = new AntdUI.Segmented();
            panel1 = new System.Windows.Forms.Panel();
            txt_search = new AntdUI.Input();
            vpanel = new AntdUI.VirtualPanel();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Description = "语义化的矢量图形。";
            header1.Dock = DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F);
            header1.LocalizationDescription = "Icon.Description";
            header1.LocalizationText = "Icon.Text";
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new Padding(0, 0, 0, 10);
            header1.Size = new Size(845, 74);
            header1.TabIndex = 0;
            header1.Text = "Icon 图标";
            header1.UseTitleFont = true;
            // 
            // segmented1
            // 
            segmented1.Dock = DockStyle.Fill;
            segmented1.IconAlign = AntdUI.TAlignMini.Left;
            segmentedItem1.IconSvg = "BorderOutlined";
            segmentedItem1.LocalizationText = "Outlined";
            segmentedItem1.Text = "线框风格";
            segmentedItem2.IconSvg = "<svg viewBox=\"0 0 1024 1024\"><path d=\"M864 64H160C107 64 64 107 64 160v704c0 53 43 96 96 96h704c53 0 96-43 96-96V160c0-53-43-96-96-96z\"></path></svg>";
            segmentedItem2.LocalizationText = "Filled";
            segmentedItem2.Text = "实底风格";
            segmented1.Items.Add(segmentedItem1);
            segmented1.Items.Add(segmentedItem2);
            segmented1.Location = new Point(10, 0);
            segmented1.Name = "segmented1";
            segmented1.SelectIndex = 0;
            segmented1.Size = new Size(495, 40);
            segmented1.TabIndex = 0;
            segmented1.SelectIndexChanged += segmented1_SelectIndexChanged;
            // 
            // panel1
            // 
            panel1.Controls.Add(segmented1);
            panel1.Controls.Add(txt_search);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 74);
            panel1.Name = "panel1";
            panel1.Padding = new Padding(10, 0, 10, 0);
            panel1.Size = new Size(845, 40);
            panel1.TabIndex = 5;
            // 
            // txt_search
            // 
            txt_search.Dock = DockStyle.Right;
            txt_search.LocalizationPlaceholderText = "Icon.PlaceholderText";
            txt_search.Location = new Point(505, 0);
            txt_search.Name = "txt_search";
            txt_search.PlaceholderText = "在此搜索图标，点击图标可复制代码";
            txt_search.Size = new Size(330, 40);
            txt_search.SuffixSvg = "SearchOutlined";
            txt_search.TabIndex = 1;
            txt_search.SuffixClick += txt_search_SuffixClick;
            txt_search.TextChanged += txt_search_TextChanged;
            // 
            // vpanel
            // 
            vpanel.Dock = DockStyle.Fill;
            vpanel.JustifyContent = AntdUI.TJustifyContent.SpaceEvenly;
            vpanel.Location = new Point(0, 114);
            vpanel.Name = "vpanel";
            vpanel.Padding = new Padding(10);
            vpanel.Size = new Size(845, 647);
            vpanel.TabIndex = 6;
            vpanel.Waterfall = true;
            vpanel.ItemClick += vpanel_ItemClick;
            // 
            // Icon
            // 
            Controls.Add(vpanel);
            Controls.Add(panel1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 11F);
            Name = "Icon";
            Size = new Size(845, 761);
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.PageHeader header1;
        private AntdUI.Segmented segmented1;
        private System.Windows.Forms.Panel panel1;
        private AntdUI.Input txt_search;
        private AntdUI.VirtualPanel vpanel;
    }
}