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
            AntdUI.SegmentedItem segmentedItem14 = new AntdUI.SegmentedItem();
            AntdUI.SegmentedItem segmentedItem15 = new AntdUI.SegmentedItem();
            AntdUI.SegmentedItem segmentedItem16 = new AntdUI.SegmentedItem();
            AntdUI.SegmentedItem segmentedItem17 = new AntdUI.SegmentedItem();
            AntdUI.SegmentedItem segmentedItem18 = new AntdUI.SegmentedItem();
            header1 = new AntdUI.PageHeader();
            panel1 = new System.Windows.Forms.Panel();
            segmented5 = new AntdUI.Segmented();
            divider4 = new AntdUI.Divider();
            segmented4 = new AntdUI.Segmented();
            divider3 = new AntdUI.Divider();
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
            // segmented5
            // 
            segmented5.Dock = DockStyle.Top;
            segmented5.IconRatio = 0.8F;
            segmentedItem1.IconSvg = "SunOutlined";
            segmentedItem2.IconSvg = "MoonOutlined";
            segmented5.Items.Add(segmentedItem1);
            segmented5.Items.Add(segmentedItem2);
            segmented5.Location = new Point(0, 292);
            segmented5.Name = "segmented5";
            segmented5.Round = true;
            segmented5.SelectIndex = 0;
            segmented5.Size = new Size(1300, 39);
            segmented5.TabIndex = 4;
            // 
            // divider4
            // 
            divider4.Dock = DockStyle.Top;
            divider4.Location = new Point(0, 272);
            divider4.Name = "divider4";
            divider4.Size = new Size(1300, 20);
            divider4.TabIndex = 0;
            // 
            // segmented4
            // 
            segmented4.Dock = DockStyle.Top;
            segmentedItem3.Text = "small";
            segmentedItem4.Text = "middle";
            segmentedItem5.Text = "large";
            segmented4.Items.Add(segmentedItem3);
            segmented4.Items.Add(segmentedItem4);
            segmented4.Items.Add(segmentedItem5);
            segmented4.Location = new Point(0, 233);
            segmented4.Name = "segmented4";
            segmented4.SelectIndex = 1;
            segmented4.Size = new Size(1300, 39);
            segmented4.TabIndex = 3;
            // 
            // divider3
            // 
            divider3.Dock = DockStyle.Top;
            divider3.Location = new Point(0, 213);
            divider3.Name = "divider3";
            divider3.Size = new Size(1300, 20);
            divider3.TabIndex = 0;
            // 
            // segmented3
            // 
            segmented3.Dock = DockStyle.Top;
            segmented3.IconAlign = AntdUI.TAlignMini.Left;
            segmentedItem6.IconSvg = "GiftFilled";
            segmentedItem6.Text = "Daily";
            segmentedItem7.IconSvg = "TrophyFilled";
            segmentedItem7.Text = "Weekly";
            segmentedItem8.IconSvg = "DashboardFilled";
            segmentedItem8.Text = "Monthly";
            segmentedItem9.IconActiveSvg = "HeartFilled";
            segmentedItem9.IconSvg = "LikeFilled";
            segmentedItem9.Text = "Quarterly";
            segmented3.Items.Add(segmentedItem6);
            segmented3.Items.Add(segmentedItem7);
            segmented3.Items.Add(segmentedItem8);
            segmented3.Items.Add(segmentedItem9);
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
            segmentedItem10.IconSvg = "GiftFilled";
            segmentedItem10.Text = "Daily";
            segmentedItem11.IconSvg = "TrophyFilled";
            segmentedItem11.Text = "Weekly";
            segmentedItem12.IconSvg = "DashboardFilled";
            segmentedItem12.Text = "Monthly";
            segmentedItem13.IconActiveSvg = "HeartFilled";
            segmentedItem13.IconSvg = "LikeFilled";
            segmentedItem13.Text = "Quarterly";
            segmented2.Items.Add(segmentedItem10);
            segmented2.Items.Add(segmentedItem11);
            segmented2.Items.Add(segmentedItem12);
            segmented2.Items.Add(segmentedItem13);
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
            segmentedItem14.Text = "Daily";
            segmentedItem15.Text = "Weekly";
            segmentedItem16.Text = "Monthly";
            segmentedItem17.Text = "Quarterly";
            segmentedItem18.Text = "Yearly";
            segmented1.Items.Add(segmentedItem14);
            segmented1.Items.Add(segmentedItem15);
            segmented1.Items.Add(segmentedItem16);
            segmented1.Items.Add(segmentedItem17);
            segmented1.Items.Add(segmentedItem18);
            segmented1.Location = new Point(0, 0);
            segmented1.Name = "segmented1";
            segmented1.Size = new Size(1300, 39);
            segmented1.TabIndex = 0;
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