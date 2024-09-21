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
    partial class Timeline
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
            AntdUI.TimelineItem timelineItem1 = new AntdUI.TimelineItem();
            AntdUI.TimelineItem timelineItem2 = new AntdUI.TimelineItem();
            AntdUI.TimelineItem timelineItem3 = new AntdUI.TimelineItem();
            AntdUI.TimelineItem timelineItem4 = new AntdUI.TimelineItem();
            AntdUI.TimelineItem timelineItem5 = new AntdUI.TimelineItem();
            AntdUI.TimelineItem timelineItem6 = new AntdUI.TimelineItem();
            AntdUI.TimelineItem timelineItem7 = new AntdUI.TimelineItem();
            AntdUI.TimelineItem timelineItem8 = new AntdUI.TimelineItem();
            AntdUI.TimelineItem timelineItem9 = new AntdUI.TimelineItem();
            AntdUI.TimelineItem timelineItem10 = new AntdUI.TimelineItem();
            header1 = new AntdUI.PageHeader();
            panel1 = new System.Windows.Forms.Panel();
            timeline2 = new AntdUI.Timeline();
            timeline1 = new AntdUI.Timeline();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Description = "垂直展示的时间流信息。";
            header1.Dock = DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F);
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new Padding(0, 0, 0, 10);
            header1.Size = new Size(1300, 74);
            header1.TabIndex = 0;
            header1.Text = "Timeline 时间轴";
            header1.UseTitleFont = true;
            // 
            // panel1
            // 
            panel1.AutoScroll = true;
            panel1.Controls.Add(timeline2);
            panel1.Controls.Add(timeline1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 74);
            panel1.Name = "panel1";
            panel1.Size = new Size(1300, 602);
            panel1.TabIndex = 7;
            // 
            // timeline2
            // 
            timeline2.Dock = DockStyle.Left;
            timeline2.Font = new Font("Microsoft YaHei UI", 11F);
            timelineItem1.Text = "Create a services site 2015-09-01";
            timelineItem1.Type = AntdUI.TTypeMini.Success;
            timelineItem2.Text = "Solve initial network problems 2015-09-01";
            timelineItem3.Text = "Technical testing 2015-09-01";
            timelineItem4.Fill = Color.BlueViolet;
            timelineItem4.Text = "Network problems being solved 2015-09-01";
            timelineItem5.Icon = Properties.Resources.bg1;
            timelineItem5.Text = "Solve initial network problems 1\r\n\r\nSolve initial network problems 2\r\n\r\nSolve initial network problems 3 2015-09-01";
            timeline2.Items.Add(timelineItem1);
            timeline2.Items.Add(timelineItem2);
            timeline2.Items.Add(timelineItem3);
            timeline2.Items.Add(timelineItem4);
            timeline2.Items.Add(timelineItem5);
            timeline2.Location = new Point(337, 0);
            timeline2.Name = "timeline2";
            timeline2.Size = new Size(343, 602);
            timeline2.TabIndex = 20;
            timeline2.Text = "timeline2";
            // 
            // timeline1
            // 
            timeline1.Dock = DockStyle.Left;
            timeline1.Font = new Font("Microsoft YaHei UI", 11F);
            timelineItem6.Text = "Create a services site 2015-09-01";
            timelineItem6.Type = AntdUI.TTypeMini.Success;
            timelineItem7.Text = "Solve initial network problems 2015-09-01";
            timelineItem8.Text = "Technical testing 2015-09-01";
            timelineItem8.Type = AntdUI.TTypeMini.Error;
            timelineItem9.Fill = Color.BlueViolet;
            timelineItem9.Text = "Network problems being solved 2015-09-01";
            timelineItem10.Text = "Solve initial network problems 1\r\n\r\nSolve initial network problems 2\r\n\r\nSolve initial network problems 3 2015-09-01";
            timelineItem10.Type = AntdUI.TTypeMini.Default;
            timeline1.Items.Add(timelineItem6);
            timeline1.Items.Add(timelineItem7);
            timeline1.Items.Add(timelineItem8);
            timeline1.Items.Add(timelineItem9);
            timeline1.Items.Add(timelineItem10);
            timeline1.Location = new Point(0, 0);
            timeline1.Name = "timeline1";
            timeline1.Size = new Size(337, 602);
            timeline1.TabIndex = 19;
            timeline1.Text = "timeline1";
            // 
            // Timeline
            // 
            Controls.Add(panel1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 12F);
            Name = "Timeline";
            Size = new Size(1300, 676);
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private AntdUI.PageHeader header1;
        private System.Windows.Forms.Panel panel1;
        private AntdUI.Timeline timeline1;
        private AntdUI.Timeline timeline2;
    }
}