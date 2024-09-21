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
    partial class Steps
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
            AntdUI.StepsItem stepsItem1 = new AntdUI.StepsItem();
            AntdUI.StepsItem stepsItem2 = new AntdUI.StepsItem();
            AntdUI.StepsItem stepsItem3 = new AntdUI.StepsItem();
            AntdUI.StepsItem stepsItem4 = new AntdUI.StepsItem();
            AntdUI.StepsItem stepsItem5 = new AntdUI.StepsItem();
            AntdUI.StepsItem stepsItem6 = new AntdUI.StepsItem();
            AntdUI.StepsItem stepsItem7 = new AntdUI.StepsItem();
            AntdUI.StepsItem stepsItem8 = new AntdUI.StepsItem();
            AntdUI.StepsItem stepsItem9 = new AntdUI.StepsItem();
            AntdUI.StepsItem stepsItem10 = new AntdUI.StepsItem();
            AntdUI.StepsItem stepsItem11 = new AntdUI.StepsItem();
            AntdUI.StepsItem stepsItem12 = new AntdUI.StepsItem();
            AntdUI.StepsItem stepsItem13 = new AntdUI.StepsItem();
            AntdUI.StepsItem stepsItem14 = new AntdUI.StepsItem();
            AntdUI.StepsItem stepsItem15 = new AntdUI.StepsItem();
            AntdUI.StepsItem stepsItem16 = new AntdUI.StepsItem();
            AntdUI.StepsItem stepsItem17 = new AntdUI.StepsItem();
            AntdUI.StepsItem stepsItem18 = new AntdUI.StepsItem();
            AntdUI.StepsItem stepsItem19 = new AntdUI.StepsItem();
            AntdUI.StepsItem stepsItem20 = new AntdUI.StepsItem();
            AntdUI.StepsItem stepsItem21 = new AntdUI.StepsItem();
            AntdUI.StepsItem stepsItem22 = new AntdUI.StepsItem();
            AntdUI.StepsItem stepsItem23 = new AntdUI.StepsItem();
            AntdUI.StepsItem stepsItem24 = new AntdUI.StepsItem();
            AntdUI.StepsItem stepsItem25 = new AntdUI.StepsItem();
            header1 = new AntdUI.PageHeader();
            panel1 = new System.Windows.Forms.Panel();
            steps6 = new AntdUI.Steps();
            steps5 = new AntdUI.Steps();
            steps4 = new AntdUI.Steps();
            steps3 = new AntdUI.Steps();
            steps2 = new AntdUI.Steps();
            steps1 = new AntdUI.Steps();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Description = "引导用户按照流程完成任务的导航条。";
            header1.Dock = DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F);
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new Padding(0, 0, 0, 10);
            header1.Size = new Size(1300, 74);
            header1.TabIndex = 0;
            header1.Text = "Steps 步骤条";
            header1.UseTitleFont = true;
            // 
            // panel1
            // 
            panel1.AutoScroll = true;
            panel1.Controls.Add(steps6);
            panel1.Controls.Add(steps5);
            panel1.Controls.Add(steps4);
            panel1.Controls.Add(steps3);
            panel1.Controls.Add(steps2);
            panel1.Controls.Add(steps1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 74);
            panel1.Name = "panel1";
            panel1.Size = new Size(1300, 602);
            panel1.TabIndex = 7;
            // 
            // steps6
            // 
            steps6.Current = 2;
            steps6.Dock = DockStyle.Left;
            stepsItem1.Description = "你想干嘛";
            stepsItem1.Title = "Step 1";
            stepsItem2.Description = "包裹大概是被偷了";
            stepsItem2.Title = "Step 2";
            stepsItem3.Title = "Step 3";
            stepsItem4.Description = "哈哈哈";
            stepsItem4.Title = "Step 4";
            stepsItem5.Description = "开心超人";
            stepsItem5.SubTitle = "终局";
            stepsItem5.Title = "Step 5";
            steps6.Items.Add(stepsItem1);
            steps6.Items.Add(stepsItem2);
            steps6.Items.Add(stepsItem3);
            steps6.Items.Add(stepsItem4);
            steps6.Items.Add(stepsItem5);
            steps6.Location = new Point(666, 153);
            steps6.Name = "steps6";
            steps6.Padding = new Padding(10, 0, 0, 0);
            steps6.Size = new Size(202, 449);
            steps6.Status = AntdUI.TStepState.Finish;
            steps6.TabIndex = 32;
            steps6.Text = "steps4";
            steps6.Vertical = true;
            steps6.Click += Steps_Click;
            // 
            // steps5
            // 
            steps5.Current = 2;
            steps5.Dock = DockStyle.Left;
            stepsItem6.Description = "你想干嘛";
            stepsItem6.Icon = Properties.Resources.img1;
            stepsItem6.Title = "Step 1";
            stepsItem7.Description = "包裹大概是被偷了";
            stepsItem7.Title = "Step 2";
            stepsItem8.Title = "Step 3";
            stepsItem9.Description = "哈哈哈";
            stepsItem9.Title = "Step 4";
            stepsItem10.SubTitle = "星球大战";
            stepsItem10.Title = "Step 5";
            steps5.Items.Add(stepsItem6);
            steps5.Items.Add(stepsItem7);
            steps5.Items.Add(stepsItem8);
            steps5.Items.Add(stepsItem9);
            steps5.Items.Add(stepsItem10);
            steps5.Location = new Point(444, 153);
            steps5.Name = "steps5";
            steps5.Padding = new Padding(10, 0, 0, 0);
            steps5.Size = new Size(222, 449);
            steps5.TabIndex = 33;
            steps5.Text = "steps4";
            steps5.Vertical = true;
            steps5.Click += Steps_Click;
            // 
            // steps4
            // 
            steps4.Current = 1;
            steps4.Dock = DockStyle.Left;
            stepsItem11.Description = "你想干嘛";
            stepsItem11.Title = "Step 1";
            stepsItem12.Description = "包裹大概是被偷了";
            stepsItem12.Title = "Step 2";
            stepsItem13.Title = "Step 3";
            stepsItem14.Description = "哈哈哈";
            steps4.Items.Add(stepsItem11);
            steps4.Items.Add(stepsItem12);
            steps4.Items.Add(stepsItem13);
            steps4.Items.Add(stepsItem14);
            steps4.Location = new Point(222, 153);
            steps4.Name = "steps4";
            steps4.Padding = new Padding(10, 0, 0, 0);
            steps4.Size = new Size(222, 449);
            steps4.Status = AntdUI.TStepState.Wait;
            steps4.TabIndex = 34;
            steps4.Text = "steps4";
            steps4.Vertical = true;
            steps4.Click += Steps_Click;
            // 
            // steps3
            // 
            steps3.Current = 1;
            steps3.Dock = DockStyle.Left;
            stepsItem15.Description = "你想干嘛";
            stepsItem15.Title = "Step 1";
            stepsItem16.Description = "包裹大概是被偷了";
            stepsItem16.Title = "Step 2";
            stepsItem17.Description = "退退退退退";
            stepsItem17.SubTitle = "￥60.0";
            stepsItem17.Title = "Step 3";
            stepsItem18.Description = "哈哈哈";
            steps3.Items.Add(stepsItem15);
            steps3.Items.Add(stepsItem16);
            steps3.Items.Add(stepsItem17);
            steps3.Items.Add(stepsItem18);
            steps3.Location = new Point(0, 153);
            steps3.Name = "steps3";
            steps3.Padding = new Padding(10, 0, 0, 0);
            steps3.Size = new Size(222, 449);
            steps3.Status = AntdUI.TStepState.Error;
            steps3.TabIndex = 31;
            steps3.Text = "steps3";
            steps3.Vertical = true;
            steps3.Click += Steps_Click;
            // 
            // steps2
            // 
            steps2.Current = 1;
            steps2.Dock = DockStyle.Top;
            stepsItem19.Description = "你想干嘛";
            stepsItem19.Title = "Step 1";
            stepsItem20.Description = "包裹大概是被偷了";
            stepsItem20.Title = "Step 2";
            stepsItem21.Title = "Step 3";
            stepsItem22.Title = "Step 4";
            steps2.Items.Add(stepsItem19);
            steps2.Items.Add(stepsItem20);
            steps2.Items.Add(stepsItem21);
            steps2.Items.Add(stepsItem22);
            steps2.Location = new Point(0, 77);
            steps2.Name = "steps2";
            steps2.Padding = new Padding(10, 0, 10, 10);
            steps2.Size = new Size(1300, 76);
            steps2.Status = AntdUI.TStepState.Error;
            steps2.TabIndex = 29;
            steps2.Text = "steps2";
            steps2.Click += Steps_Click;
            // 
            // steps1
            // 
            steps1.Current = 1;
            steps1.Dock = DockStyle.Top;
            stepsItem23.Description = "This is a description.";
            stepsItem23.Title = "Finished";
            stepsItem24.Description = "This is a description.";
            stepsItem24.SubTitle = "Left 00:00:08";
            stepsItem24.Title = "In Progress";
            stepsItem25.Description = "This is a description.";
            stepsItem25.Title = "Waiting";
            steps1.Items.Add(stepsItem23);
            steps1.Items.Add(stepsItem24);
            steps1.Items.Add(stepsItem25);
            steps1.Location = new Point(0, 0);
            steps1.Name = "steps1";
            steps1.Size = new Size(1300, 77);
            steps1.TabIndex = 28;
            steps1.Text = "steps1";
            steps1.Click += Steps_Click;
            // 
            // Steps
            // 
            Controls.Add(panel1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 12F);
            Name = "Steps";
            Size = new Size(1300, 676);
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private AntdUI.PageHeader header1;
        private System.Windows.Forms.Panel panel1;
        private AntdUI.Steps steps1;
        private AntdUI.Steps steps2;
        private AntdUI.Steps steps6;
        private AntdUI.Steps steps5;
        private AntdUI.Steps steps4;
        private AntdUI.Steps steps3;
    }
}