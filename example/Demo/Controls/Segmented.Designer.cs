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
    partial class Segmented
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
            AntdUI.SegmentedItem segmentedItem1 = new AntdUI.SegmentedItem();
            AntdUI.SegmentedItem segmentedItem2 = new AntdUI.SegmentedItem();
            AntdUI.SegmentedItem segmentedItem3 = new AntdUI.SegmentedItem();
            AntdUI.SegmentedItem segmentedItem4 = new AntdUI.SegmentedItem();
            AntdUI.SegmentedItem segmentedItem5 = new AntdUI.SegmentedItem();
            AntdUI.SegmentedItem segmentedItem6 = new AntdUI.SegmentedItem();
            AntdUI.SegmentedItem segmentedItem7 = new AntdUI.SegmentedItem();
            AntdUI.SegmentedItem segmentedItem8 = new AntdUI.SegmentedItem();
            AntdUI.SegmentedItem segmentedItem9 = new AntdUI.SegmentedItem();
            AntdUI.SegmentedItem segmentedItem10 = new AntdUI.SegmentedItem();
            AntdUI.SegmentedItem segmentedItem11 = new AntdUI.SegmentedItem();
            AntdUI.SegmentedItem segmentedItem12 = new AntdUI.SegmentedItem();
            AntdUI.SegmentedItem segmentedItem13 = new AntdUI.SegmentedItem();
            header1 = new AntdUI.PageHeader();
            panel1 = new System.Windows.Forms.Panel();
            segmented3 = new AntdUI.Segmented();
            divider2 = new AntdUI.Divider();
            segmented2 = new AntdUI.Segmented();
            divider1 = new AntdUI.Divider();
            segmented1 = new AntdUI.Segmented();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Description = "分段控制器。";
            header1.Dock = DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F);
            header1.LocalizationDescription = "Segmented.Description";
            header1.LocalizationText = "Segmented.Text";
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new Padding(0, 0, 0, 10);
            header1.Size = new Size(1300, 74);
            header1.TabIndex = 0;
            header1.Text = "Segmented 分段控制器";
            header1.UseTitleFont = true;
            // 
            // panel1
            // 
            panel1.AutoScroll = true;
            panel1.Controls.Add(segmented3);
            panel1.Controls.Add(divider2);
            panel1.Controls.Add(segmented2);
            panel1.Controls.Add(divider1);
            panel1.Controls.Add(segmented1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 74);
            panel1.Name = "panel1";
            panel1.Size = new Size(1300, 602);
            panel1.TabIndex = 7;
            // 
            // segmented3
            // 
            segmented3.Dock = DockStyle.Top;
            segmented3.IconAlign = AntdUI.TAlignMini.Left;
            segmentedItem1.IconSvg = "GiftFilled";
            segmentedItem1.Text = "Daily";
            segmentedItem2.IconSvg = "TrophyFilled";
            segmentedItem2.Text = "Weekly";
            segmentedItem3.IconSvg = "DashboardFilled";
            segmentedItem3.Text = "Monthly";
            segmentedItem4.IconActiveSvg = "HeartFilled";
            segmentedItem4.IconSvg = "LikeFilled";
            segmentedItem4.Text = "Quarterly";
            segmented3.Items.Add(segmentedItem1);
            segmented3.Items.Add(segmentedItem2);
            segmented3.Items.Add(segmentedItem3);
            segmented3.Items.Add(segmentedItem4);
            segmented3.Location = new Point(0, 149);
            segmented3.Name = "segmented3";
            segmented3.SelectIndex = 0;
            segmented3.Size = new Size(1300, 36);
            segmented3.TabIndex = 12;
            // 
            // divider2
            // 
            divider2.Dock = DockStyle.Top;
            divider2.Location = new Point(0, 129);
            divider2.Name = "divider2";
            divider2.Size = new Size(1300, 20);
            divider2.TabIndex = 15;
            // 
            // segmented2
            // 
            segmented2.Dock = DockStyle.Top;
            segmentedItem5.IconSvg = "GiftFilled";
            segmentedItem5.Text = "Daily";
            segmentedItem6.IconSvg = "TrophyFilled";
            segmentedItem6.Text = "Weekly";
            segmentedItem7.IconSvg = "DashboardFilled";
            segmentedItem7.Text = "Monthly";
            segmentedItem8.IconActiveSvg = "HeartFilled";
            segmentedItem8.IconSvg = "LikeFilled";
            segmentedItem8.Text = "Quarterly";
            segmented2.Items.Add(segmentedItem5);
            segmented2.Items.Add(segmentedItem6);
            segmented2.Items.Add(segmentedItem7);
            segmented2.Items.Add(segmentedItem8);
            segmented2.Location = new Point(0, 56);
            segmented2.Margin = new Padding(1);
            segmented2.Name = "segmented2";
            segmented2.SelectIndex = 0;
            segmented2.Size = new Size(1300, 73);
            segmented2.TabIndex = 12;
            // 
            // divider1
            // 
            divider1.Dock = DockStyle.Top;
            divider1.Location = new Point(0, 36);
            divider1.Name = "divider1";
            divider1.Size = new Size(1300, 20);
            divider1.TabIndex = 14;
            // 
            // segmented1
            // 
            segmented1.Dock = DockStyle.Top;
            segmentedItem9.Text = "Daily";
            segmentedItem10.Text = "Weekly";
            segmentedItem11.Text = "Monthly";
            segmentedItem12.Text = "Quarterly";
            segmentedItem13.Text = "Yearly";
            segmented1.Items.Add(segmentedItem9);
            segmented1.Items.Add(segmentedItem10);
            segmented1.Items.Add(segmentedItem11);
            segmented1.Items.Add(segmentedItem12);
            segmented1.Items.Add(segmentedItem13);
            segmented1.Location = new Point(0, 0);
            segmented1.Name = "segmented1";
            segmented1.Size = new Size(1300, 36);
            segmented1.TabIndex = 13;
            segmented1.Text = "segmented1";
            // 
            // Segmented
            // 
            Controls.Add(panel1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 10F);
            Name = "Segmented";
            Size = new Size(1300, 676);
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private AntdUI.PageHeader header1;
        private System.Windows.Forms.Panel panel1;
        private AntdUI.Segmented segmented2;
        private AntdUI.Segmented segmented1;
        private AntdUI.Segmented segmented3;
        private AntdUI.Divider divider2;
        private AntdUI.Divider divider1;
    }
}