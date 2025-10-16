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

        private void InitializeComponent()
        {
            AntdUI.SegmentedItem segmentedItem19 = new AntdUI.SegmentedItem();
            AntdUI.SegmentedItem segmentedItem20 = new AntdUI.SegmentedItem();
            AntdUI.SegmentedItem segmentedItem21 = new AntdUI.SegmentedItem();
            AntdUI.SegmentedItem segmentedItem22 = new AntdUI.SegmentedItem();
            AntdUI.SegmentedItem segmentedItem23 = new AntdUI.SegmentedItem();
            AntdUI.SegmentedItem segmentedItem24 = new AntdUI.SegmentedItem();
            AntdUI.SegmentedItem segmentedItem25 = new AntdUI.SegmentedItem();
            AntdUI.SegmentedItem segmentedItem26 = new AntdUI.SegmentedItem();
            AntdUI.SegmentedItem segmentedItem27 = new AntdUI.SegmentedItem();
            AntdUI.SegmentedItem segmentedItem28 = new AntdUI.SegmentedItem();
            AntdUI.SegmentedItem segmentedItem29 = new AntdUI.SegmentedItem();
            AntdUI.SegmentedItem segmentedItem30 = new AntdUI.SegmentedItem();
            AntdUI.SegmentedItem segmentedItem31 = new AntdUI.SegmentedItem();
            AntdUI.SegmentedItem segmentedItem32 = new AntdUI.SegmentedItem();
            AntdUI.SegmentedItem segmentedItem33 = new AntdUI.SegmentedItem();
            AntdUI.SegmentedItem segmentedItem34 = new AntdUI.SegmentedItem();
            AntdUI.SegmentedItem segmentedItem35 = new AntdUI.SegmentedItem();
            AntdUI.SegmentedItem segmentedItem36 = new AntdUI.SegmentedItem();
            header1 = new AntdUI.PageHeader();
            panel1 = new System.Windows.Forms.Panel();
            segmented3 = new AntdUI.Segmented();
            divider2 = new AntdUI.Divider();
            segmented2 = new AntdUI.Segmented();
            divider1 = new AntdUI.Divider();
            segmented1 = new AntdUI.Segmented();
            divider3 = new AntdUI.Divider();
            segmented4 = new AntdUI.Segmented();
            divider4 = new AntdUI.Divider();
            segmented5 = new AntdUI.Segmented();
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
            panel1.Controls.Add(segmented5);
            panel1.Controls.Add(divider4);
            panel1.Controls.Add(segmented4);
            panel1.Controls.Add(divider3);
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
            segmentedItem19.Badge = null;
            segmentedItem19.BadgeAlign = AntdUI.TAlign.TR;
            segmentedItem19.BadgeBack = null;
            segmentedItem19.BadgeMode = false;
            segmentedItem19.BadgeOffsetX = 0;
            segmentedItem19.BadgeOffsetY = 0;
            segmentedItem19.BadgeSize = 0.6F;
            segmentedItem19.BadgeSvg = null;
            segmentedItem19.IconSvg = "GiftFilled";
            segmentedItem19.Text = "Daily";
            segmentedItem20.Badge = null;
            segmentedItem20.BadgeAlign = AntdUI.TAlign.TR;
            segmentedItem20.BadgeBack = null;
            segmentedItem20.BadgeMode = false;
            segmentedItem20.BadgeOffsetX = 0;
            segmentedItem20.BadgeOffsetY = 0;
            segmentedItem20.BadgeSize = 0.6F;
            segmentedItem20.BadgeSvg = null;
            segmentedItem20.IconSvg = "TrophyFilled";
            segmentedItem20.Text = "Weekly";
            segmentedItem21.Badge = null;
            segmentedItem21.BadgeAlign = AntdUI.TAlign.TR;
            segmentedItem21.BadgeBack = null;
            segmentedItem21.BadgeMode = false;
            segmentedItem21.BadgeOffsetX = 0;
            segmentedItem21.BadgeOffsetY = 0;
            segmentedItem21.BadgeSize = 0.6F;
            segmentedItem21.BadgeSvg = null;
            segmentedItem21.IconSvg = "DashboardFilled";
            segmentedItem21.Text = "Monthly";
            segmentedItem22.Badge = null;
            segmentedItem22.BadgeAlign = AntdUI.TAlign.TR;
            segmentedItem22.BadgeBack = null;
            segmentedItem22.BadgeMode = false;
            segmentedItem22.BadgeOffsetX = 0;
            segmentedItem22.BadgeOffsetY = 0;
            segmentedItem22.BadgeSize = 0.6F;
            segmentedItem22.BadgeSvg = null;
            segmentedItem22.IconActiveSvg = "HeartFilled";
            segmentedItem22.IconSvg = "LikeFilled";
            segmentedItem22.Text = "Quarterly";
            segmented3.Items.Add(segmentedItem19);
            segmented3.Items.Add(segmentedItem20);
            segmented3.Items.Add(segmentedItem21);
            segmented3.Items.Add(segmentedItem22);
            segmented3.Location = new Point(0, 168);
            segmented3.Name = "segmented3";
            segmented3.SelectIndex = 0;
            segmented3.Size = new Size(1300, 45);
            segmented3.TabIndex = 2;
            // 
            // divider2
            // 
            divider2.Dock = DockStyle.Top;
            divider2.Location = new Point(0, 148);
            divider2.Name = "divider2";
            divider2.Size = new Size(1300, 20);
            divider2.TabIndex = 0;
            // 
            // segmented2
            // 
            segmented2.Dock = DockStyle.Top;
            segmentedItem23.Badge = null;
            segmentedItem23.BadgeAlign = AntdUI.TAlign.TR;
            segmentedItem23.BadgeBack = null;
            segmentedItem23.BadgeMode = false;
            segmentedItem23.BadgeOffsetX = 0;
            segmentedItem23.BadgeOffsetY = 0;
            segmentedItem23.BadgeSize = 0.6F;
            segmentedItem23.BadgeSvg = null;
            segmentedItem23.IconSvg = "GiftFilled";
            segmentedItem23.Text = "Daily";
            segmentedItem24.Badge = null;
            segmentedItem24.BadgeAlign = AntdUI.TAlign.TR;
            segmentedItem24.BadgeBack = null;
            segmentedItem24.BadgeMode = false;
            segmentedItem24.BadgeOffsetX = 0;
            segmentedItem24.BadgeOffsetY = 0;
            segmentedItem24.BadgeSize = 0.6F;
            segmentedItem24.BadgeSvg = null;
            segmentedItem24.IconSvg = "TrophyFilled";
            segmentedItem24.Text = "Weekly";
            segmentedItem25.Badge = null;
            segmentedItem25.BadgeAlign = AntdUI.TAlign.TR;
            segmentedItem25.BadgeBack = null;
            segmentedItem25.BadgeMode = false;
            segmentedItem25.BadgeOffsetX = 0;
            segmentedItem25.BadgeOffsetY = 0;
            segmentedItem25.BadgeSize = 0.6F;
            segmentedItem25.BadgeSvg = null;
            segmentedItem25.IconSvg = "DashboardFilled";
            segmentedItem25.Text = "Monthly";
            segmentedItem26.Badge = null;
            segmentedItem26.BadgeAlign = AntdUI.TAlign.TR;
            segmentedItem26.BadgeBack = null;
            segmentedItem26.BadgeMode = false;
            segmentedItem26.BadgeOffsetX = 0;
            segmentedItem26.BadgeOffsetY = 0;
            segmentedItem26.BadgeSize = 0.6F;
            segmentedItem26.BadgeSvg = null;
            segmentedItem26.IconActiveSvg = "HeartFilled";
            segmentedItem26.IconSvg = "LikeFilled";
            segmentedItem26.Text = "Quarterly";
            segmented2.Items.Add(segmentedItem23);
            segmented2.Items.Add(segmentedItem24);
            segmented2.Items.Add(segmentedItem25);
            segmented2.Items.Add(segmentedItem26);
            segmented2.Location = new Point(0, 59);
            segmented2.Margin = new Padding(1);
            segmented2.Name = "segmented2";
            segmented2.SelectIndex = 0;
            segmented2.Size = new Size(1300, 89);
            segmented2.TabIndex = 1;
            // 
            // divider1
            // 
            divider1.Dock = DockStyle.Top;
            divider1.Location = new Point(0, 39);
            divider1.Name = "divider1";
            divider1.Size = new Size(1300, 20);
            divider1.TabIndex = 0;
            // 
            // segmented1
            // 
            segmented1.Dock = DockStyle.Top;
            segmentedItem27.Badge = null;
            segmentedItem27.BadgeAlign = AntdUI.TAlign.TR;
            segmentedItem27.BadgeBack = null;
            segmentedItem27.BadgeMode = false;
            segmentedItem27.BadgeOffsetX = 0;
            segmentedItem27.BadgeOffsetY = 0;
            segmentedItem27.BadgeSize = 0.6F;
            segmentedItem27.BadgeSvg = null;
            segmentedItem27.Text = "Daily";
            segmentedItem28.Badge = null;
            segmentedItem28.BadgeAlign = AntdUI.TAlign.TR;
            segmentedItem28.BadgeBack = null;
            segmentedItem28.BadgeMode = false;
            segmentedItem28.BadgeOffsetX = 0;
            segmentedItem28.BadgeOffsetY = 0;
            segmentedItem28.BadgeSize = 0.6F;
            segmentedItem28.BadgeSvg = null;
            segmentedItem28.Text = "Weekly";
            segmentedItem29.Badge = null;
            segmentedItem29.BadgeAlign = AntdUI.TAlign.TR;
            segmentedItem29.BadgeBack = null;
            segmentedItem29.BadgeMode = false;
            segmentedItem29.BadgeOffsetX = 0;
            segmentedItem29.BadgeOffsetY = 0;
            segmentedItem29.BadgeSize = 0.6F;
            segmentedItem29.BadgeSvg = null;
            segmentedItem29.Text = "Monthly";
            segmentedItem30.Badge = null;
            segmentedItem30.BadgeAlign = AntdUI.TAlign.TR;
            segmentedItem30.BadgeBack = null;
            segmentedItem30.BadgeMode = false;
            segmentedItem30.BadgeOffsetX = 0;
            segmentedItem30.BadgeOffsetY = 0;
            segmentedItem30.BadgeSize = 0.6F;
            segmentedItem30.BadgeSvg = null;
            segmentedItem30.Text = "Quarterly";
            segmentedItem31.Badge = null;
            segmentedItem31.BadgeAlign = AntdUI.TAlign.TR;
            segmentedItem31.BadgeBack = null;
            segmentedItem31.BadgeMode = false;
            segmentedItem31.BadgeOffsetX = 0;
            segmentedItem31.BadgeOffsetY = 0;
            segmentedItem31.BadgeSize = 0.6F;
            segmentedItem31.BadgeSvg = null;
            segmentedItem31.Text = "Yearly";
            segmented1.Items.Add(segmentedItem27);
            segmented1.Items.Add(segmentedItem28);
            segmented1.Items.Add(segmentedItem29);
            segmented1.Items.Add(segmentedItem30);
            segmented1.Items.Add(segmentedItem31);
            segmented1.Location = new Point(0, 0);
            segmented1.Name = "segmented1";
            segmented1.Size = new Size(1300, 39);
            segmented1.TabIndex = 0;
            // 
            // divider3
            // 
            divider3.Dock = DockStyle.Top;
            divider3.Location = new Point(0, 213);
            divider3.Name = "divider3";
            divider3.Size = new Size(1300, 20);
            divider3.TabIndex = 0;
            // 
            // segmented4
            // 
            segmented4.Dock = DockStyle.Top;
            segmentedItem32.Badge = null;
            segmentedItem32.BadgeAlign = AntdUI.TAlign.TR;
            segmentedItem32.BadgeBack = null;
            segmentedItem32.BadgeMode = false;
            segmentedItem32.BadgeOffsetX = 0;
            segmentedItem32.BadgeOffsetY = 0;
            segmentedItem32.BadgeSize = 0.6F;
            segmentedItem32.BadgeSvg = null;
            segmentedItem32.Text = "small";
            segmentedItem33.Badge = null;
            segmentedItem33.BadgeAlign = AntdUI.TAlign.TR;
            segmentedItem33.BadgeBack = null;
            segmentedItem33.BadgeMode = false;
            segmentedItem33.BadgeOffsetX = 0;
            segmentedItem33.BadgeOffsetY = 0;
            segmentedItem33.BadgeSize = 0.6F;
            segmentedItem33.BadgeSvg = null;
            segmentedItem33.Text = "middle";
            segmentedItem34.Badge = null;
            segmentedItem34.BadgeAlign = AntdUI.TAlign.TR;
            segmentedItem34.BadgeBack = null;
            segmentedItem34.BadgeMode = false;
            segmentedItem34.BadgeOffsetX = 0;
            segmentedItem34.BadgeOffsetY = 0;
            segmentedItem34.BadgeSize = 0.6F;
            segmentedItem34.BadgeSvg = null;
            segmentedItem34.Text = "large";
            segmented4.Items.Add(segmentedItem32);
            segmented4.Items.Add(segmentedItem33);
            segmented4.Items.Add(segmentedItem34);
            segmented4.Location = new Point(0, 233);
            segmented4.Name = "segmented4";
            segmented4.SelectIndex = 1;
            segmented4.Size = new Size(1300, 39);
            segmented4.TabIndex = 3;
            // 
            // divider4
            // 
            divider4.Dock = DockStyle.Top;
            divider4.Location = new Point(0, 272);
            divider4.Name = "divider4";
            divider4.Size = new Size(1300, 20);
            divider4.TabIndex = 0;
            // 
            // segmented5
            // 
            segmented5.Dock = DockStyle.Top;
            segmented5.IconRatio = 0.8F;
            segmentedItem35.Badge = null;
            segmentedItem35.BadgeAlign = AntdUI.TAlign.TR;
            segmentedItem35.BadgeBack = null;
            segmentedItem35.BadgeMode = false;
            segmentedItem35.BadgeOffsetX = 0;
            segmentedItem35.BadgeOffsetY = 0;
            segmentedItem35.BadgeSize = 0.6F;
            segmentedItem35.BadgeSvg = null;
            segmentedItem35.IconSvg = "SunOutlined";
            segmentedItem36.Badge = null;
            segmentedItem36.BadgeAlign = AntdUI.TAlign.TR;
            segmentedItem36.BadgeBack = null;
            segmentedItem36.BadgeMode = false;
            segmentedItem36.BadgeOffsetX = 0;
            segmentedItem36.BadgeOffsetY = 0;
            segmentedItem36.BadgeSize = 0.6F;
            segmentedItem36.BadgeSvg = null;
            segmentedItem36.IconSvg = "MoonOutlined";
            segmented5.Items.Add(segmentedItem35);
            segmented5.Items.Add(segmentedItem36);
            segmented5.Location = new Point(0, 292);
            segmented5.Name = "segmented5";
            segmented5.Round = true;
            segmented5.SelectIndex = 0;
            segmented5.Size = new Size(1300, 39);
            segmented5.TabIndex = 4;
            // 
            // Segmented
            // 
            Controls.Add(panel1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 12F);
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
        private AntdUI.Segmented segmented4;
        private AntdUI.Divider divider3;
        private AntdUI.Segmented segmented5;
        private AntdUI.Divider divider4;
    }
}