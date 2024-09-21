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
    partial class Tooltip
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
            header1 = new AntdUI.PageHeader();
            panel1 = new System.Windows.Forms.Panel();
            panel3 = new System.Windows.Forms.Panel();
            button9 = new AntdUI.Button();
            button6 = new AntdUI.Button();
            button8 = new AntdUI.Button();
            button5 = new AntdUI.Button();
            button7 = new AntdUI.Button();
            button2 = new AntdUI.Button();
            button12 = new AntdUI.Button();
            button4 = new AntdUI.Button();
            button11 = new AntdUI.Button();
            button3 = new AntdUI.Button();
            button10 = new AntdUI.Button();
            button1 = new AntdUI.Button();
            divider2 = new AntdUI.Divider();
            panel2 = new System.Windows.Forms.Panel();
            tooltip1 = new AntdUI.Tooltip();
            label4 = new Label();
            divider1 = new AntdUI.Divider();
            tooltipComponent1 = new AntdUI.TooltipComponent();
            tooltipTL = new AntdUI.TooltipComponent();
            tooltipTop = new AntdUI.TooltipComponent();
            tooltipTR = new AntdUI.TooltipComponent();
            tooltipRT = new AntdUI.TooltipComponent();
            tooltipRight = new AntdUI.TooltipComponent();
            tooltipRB = new AntdUI.TooltipComponent();
            tooltipBR = new AntdUI.TooltipComponent();
            tooltipBottom = new AntdUI.TooltipComponent();
            tooltipBL = new AntdUI.TooltipComponent();
            tooltipLB = new AntdUI.TooltipComponent();
            tooltipLeft = new AntdUI.TooltipComponent();
            tooltipLT = new AntdUI.TooltipComponent();
            panel1.SuspendLayout();
            panel3.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Description = "简单的文字提示气泡框。";
            header1.Dock = DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F);
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new Padding(0, 0, 0, 10);
            header1.Size = new Size(589, 74);
            header1.TabIndex = 0;
            header1.Text = "Tooltip 文字提示";
            header1.UseTitleFont = true;
            // 
            // panel1
            // 
            panel1.AutoScroll = true;
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(divider2);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(divider1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 74);
            panel1.Name = "panel1";
            panel1.Size = new Size(589, 384);
            panel1.TabIndex = 6;
            // 
            // panel3
            // 
            panel3.Controls.Add(button9);
            panel3.Controls.Add(button6);
            panel3.Controls.Add(button8);
            panel3.Controls.Add(button5);
            panel3.Controls.Add(button7);
            panel3.Controls.Add(button2);
            panel3.Controls.Add(button12);
            panel3.Controls.Add(button4);
            panel3.Controls.Add(button11);
            panel3.Controls.Add(button3);
            panel3.Controls.Add(button10);
            panel3.Controls.Add(button1);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(0, 106);
            panel3.Name = "panel3";
            panel3.Size = new Size(589, 247);
            panel3.TabIndex = 5;
            // 
            // button9
            // 
            button9.BorderWidth = 2F;
            button9.Location = new Point(343, 147);
            button9.Name = "button9";
            button9.Size = new Size(60, 40);
            button9.TabIndex = 2;
            button9.Text = "RB";
            tooltipRB.SetTip(button9, "prompt text");
            // 
            // button6
            // 
            button6.BorderWidth = 2F;
            button6.Location = new Point(51, 147);
            button6.Name = "button6";
            button6.Size = new Size(60, 40);
            button6.TabIndex = 2;
            button6.Text = "LB";
            tooltipLB.SetTip(button6, "prompt text");
            // 
            // button8
            // 
            button8.BorderWidth = 2F;
            button8.Location = new Point(343, 101);
            button8.Name = "button8";
            button8.Size = new Size(86, 40);
            button8.TabIndex = 2;
            button8.Text = "Right";
            tooltipRight.SetTip(button8, "prompt text");
            // 
            // button5
            // 
            button5.BorderWidth = 2F;
            button5.Location = new Point(25, 101);
            button5.Name = "button5";
            button5.Size = new Size(86, 40);
            button5.TabIndex = 2;
            button5.Text = "Left";
            tooltipLeft.SetTip(button5, "prompt text");
            // 
            // button7
            // 
            button7.BorderWidth = 2F;
            button7.Location = new Point(343, 55);
            button7.Name = "button7";
            button7.Size = new Size(60, 40);
            button7.TabIndex = 2;
            button7.Text = "RT";
            tooltipRT.SetTip(button7, "prompt text");
            // 
            // button2
            // 
            button2.BorderWidth = 2F;
            button2.Location = new Point(51, 55);
            button2.Name = "button2";
            button2.Size = new Size(60, 40);
            button2.TabIndex = 2;
            button2.Text = "LT";
            tooltipLT.SetTip(button2, "prompt text");
            // 
            // button12
            // 
            button12.BorderWidth = 2F;
            button12.Location = new Point(283, 193);
            button12.Name = "button12";
            button12.Size = new Size(60, 40);
            button12.TabIndex = 2;
            button12.Text = "BR";
            tooltipBR.SetTip(button12, "prompt text");
            // 
            // button4
            // 
            button4.BorderWidth = 2F;
            button4.Location = new Point(283, 13);
            button4.Name = "button4";
            button4.Size = new Size(60, 40);
            button4.TabIndex = 2;
            button4.Text = "TR";
            tooltipTR.SetTip(button4, "prompt text");
            // 
            // button11
            // 
            button11.BorderWidth = 2F;
            button11.Location = new Point(177, 193);
            button11.Name = "button11";
            button11.Size = new Size(100, 40);
            button11.TabIndex = 2;
            button11.Text = "Bottom";
            tooltipBottom.SetTip(button11, "prompt text");
            // 
            // button3
            // 
            button3.BorderWidth = 2F;
            button3.Location = new Point(177, 13);
            button3.Name = "button3";
            button3.Size = new Size(100, 40);
            button3.TabIndex = 2;
            button3.Text = "Top";
            tooltipTop.SetTip(button3, "prompt text");
            // 
            // button10
            // 
            button10.BorderWidth = 2F;
            button10.Location = new Point(111, 193);
            button10.Name = "button10";
            button10.Size = new Size(60, 40);
            button10.TabIndex = 2;
            button10.Text = "BL";
            tooltipBL.SetTip(button10, "prompt text");
            // 
            // button1
            // 
            button1.BorderWidth = 2F;
            button1.Location = new Point(111, 13);
            button1.Name = "button1";
            button1.Size = new Size(60, 40);
            button1.TabIndex = 2;
            button1.Text = "TL";
            tooltipTL.SetTip(button1, "prompt text");
            // 
            // divider2
            // 
            divider2.Dock = DockStyle.Top;
            divider2.Font = new Font("Microsoft YaHei UI", 10F);
            divider2.Location = new Point(0, 84);
            divider2.Margin = new Padding(10);
            divider2.Name = "divider2";
            divider2.Orientation = AntdUI.TOrientation.Left;
            divider2.Size = new Size(589, 22);
            divider2.TabIndex = 4;
            divider2.Text = "位置";
            // 
            // panel2
            // 
            panel2.Controls.Add(tooltip1);
            panel2.Controls.Add(label4);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 22);
            panel2.Name = "panel2";
            panel2.Size = new Size(589, 62);
            panel2.TabIndex = 3;
            // 
            // tooltip1
            // 
            tooltip1.Location = new Point(142, 3);
            tooltip1.MaximumSize = new Size(335, 51);
            tooltip1.MinimumSize = new Size(335, 51);
            tooltip1.Name = "tooltip1";
            tooltip1.Size = new Size(335, 51);
            tooltip1.TabIndex = 1;
            tooltip1.Text = "Thanks for using antd. Have a nice day!";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 10);
            label4.Name = "label4";
            label4.Size = new Size(106, 21);
            label4.TabIndex = 0;
            label4.Text = "最简单的用法";
            tooltipComponent1.SetTip(label4, "prompt text");
            // 
            // divider1
            // 
            divider1.Dock = DockStyle.Top;
            divider1.Font = new Font("Microsoft YaHei UI", 10F);
            divider1.Location = new Point(0, 0);
            divider1.Margin = new Padding(10);
            divider1.Name = "divider1";
            divider1.Orientation = AntdUI.TOrientation.Left;
            divider1.Size = new Size(589, 22);
            divider1.TabIndex = 1;
            divider1.Text = "基本";
            // 
            // tooltipComponent1
            // 
            tooltipComponent1.Font = new Font("Microsoft YaHei UI", 12F);
            // 
            // tooltipTL
            // 
            tooltipTL.ArrowAlign = AntdUI.TAlign.TL;
            tooltipTL.Font = new Font("Microsoft YaHei UI", 12F);
            // 
            // tooltipTop
            // 
            tooltipTop.Font = new Font("Microsoft YaHei UI", 12F);
            // 
            // tooltipTR
            // 
            tooltipTR.ArrowAlign = AntdUI.TAlign.TR;
            tooltipTR.Font = new Font("Microsoft YaHei UI", 12F);
            // 
            // tooltipRT
            // 
            tooltipRT.ArrowAlign = AntdUI.TAlign.RT;
            tooltipRT.Font = new Font("Microsoft YaHei UI", 12F);
            // 
            // tooltipRight
            // 
            tooltipRight.ArrowAlign = AntdUI.TAlign.Right;
            tooltipRight.Font = new Font("Microsoft YaHei UI", 12F);
            // 
            // tooltipRB
            // 
            tooltipRB.ArrowAlign = AntdUI.TAlign.RB;
            tooltipRB.Font = new Font("Microsoft YaHei UI", 12F);
            // 
            // tooltipBR
            // 
            tooltipBR.ArrowAlign = AntdUI.TAlign.BR;
            tooltipBR.Font = new Font("Microsoft YaHei UI", 12F);
            // 
            // tooltipBottom
            // 
            tooltipBottom.ArrowAlign = AntdUI.TAlign.Bottom;
            tooltipBottom.Font = new Font("Microsoft YaHei UI", 12F);
            // 
            // tooltipBL
            // 
            tooltipBL.ArrowAlign = AntdUI.TAlign.BL;
            tooltipBL.Font = new Font("Microsoft YaHei UI", 12F);
            // 
            // tooltipLB
            // 
            tooltipLB.ArrowAlign = AntdUI.TAlign.LB;
            tooltipLB.Font = new Font("Microsoft YaHei UI", 12F);
            // 
            // tooltipLeft
            // 
            tooltipLeft.ArrowAlign = AntdUI.TAlign.Left;
            tooltipLeft.Font = new Font("Microsoft YaHei UI", 12F);
            // 
            // tooltipLT
            // 
            tooltipLT.ArrowAlign = AntdUI.TAlign.LT;
            tooltipLT.Font = new Font("Microsoft YaHei UI", 12F);
            // 
            // Tooltip
            // 
            Controls.Add(panel1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 12F);
            Name = "Tooltip";
            Size = new Size(589, 458);
            panel1.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private AntdUI.PageHeader header1;
        private System.Windows.Forms.Panel panel1;
        private AntdUI.Divider divider1;
        private System.Windows.Forms.Panel panel2;
        private Label label4;
        private AntdUI.TooltipComponent tooltipComponent1;
        private System.Windows.Forms.Panel panel3;
        private AntdUI.Divider divider2;
        private AntdUI.Button button2;
        private AntdUI.Button button4;
        private AntdUI.Button button3;
        private AntdUI.Button button1;
        private AntdUI.Button button9;
        private AntdUI.Button button6;
        private AntdUI.Button button8;
        private AntdUI.Button button5;
        private AntdUI.Button button7;
        private AntdUI.Button button12;
        private AntdUI.Button button11;
        private AntdUI.Button button10;
        private AntdUI.TooltipComponent tooltipTL;
        private AntdUI.TooltipComponent tooltipTop;
        private AntdUI.TooltipComponent tooltipTR;
        private AntdUI.TooltipComponent tooltipRB;
        private AntdUI.TooltipComponent tooltipRight;
        private AntdUI.TooltipComponent tooltipRT;
        private AntdUI.TooltipComponent tooltipBR;
        private AntdUI.TooltipComponent tooltipBottom;
        private AntdUI.TooltipComponent tooltipBL;
        private AntdUI.TooltipComponent tooltipLB;
        private AntdUI.TooltipComponent tooltipLeft;
        private AntdUI.TooltipComponent tooltipLT;
        private AntdUI.Tooltip tooltip1;
    }
}