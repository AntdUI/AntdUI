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

namespace Demo.Controls
{
    partial class GridPanel
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
            pageHeader1 = new AntdUI.PageHeader();
            stackPanel1 = new AntdUI.StackPanel();
            label1 = new AntdUI.Label();
            input1 = new AntdUI.Input();
            gridPanel1 = new AntdUI.GridPanel();
            button6 = new AntdUI.Button();
            button5 = new AntdUI.Button();
            button4 = new AntdUI.Button();
            button3 = new AntdUI.Button();
            button2 = new AntdUI.Button();
            button1 = new AntdUI.Button();
            stackPanel1.SuspendLayout();
            gridPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // pageHeader1
            // 
            pageHeader1.Description = "精准划分区域的格栅布局容器。";
            pageHeader1.Dock = System.Windows.Forms.DockStyle.Top;
            pageHeader1.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
            pageHeader1.LocalizationDescription = "GridPanel.Description";
            pageHeader1.LocalizationText = "GridPanel.Text";
            pageHeader1.Location = new System.Drawing.Point(0, 0);
            pageHeader1.Name = "pageHeader1";
            pageHeader1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            pageHeader1.Size = new System.Drawing.Size(1000, 75);
            pageHeader1.TabIndex = 0;
            pageHeader1.Text = "GridPanel 格栅布局";
            pageHeader1.UseTitleFont = true;
            // 
            // stackPanel1
            // 
            stackPanel1.Controls.Add(label1);
            stackPanel1.Controls.Add(input1);
            stackPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            stackPanel1.Location = new System.Drawing.Point(0, 75);
            stackPanel1.Name = "stackPanel1";
            stackPanel1.Size = new System.Drawing.Size(1000, 72);
            stackPanel1.TabIndex = 1;
            stackPanel1.Text = "stackPanel1";
            // 
            // label1
            // 
            label1.Location = new System.Drawing.Point(530, 3);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(698, 66);
            label1.TabIndex = 1;
            label1.Text = "以-为分解。-前为列宽属性，-后为行高属性列宽以；分组。每一组代表一行\r\n行高属性个数要与行数对应,每个数之间用空格间隔";
            // 
            // input1
            // 
            input1.Location = new System.Drawing.Point(3, 3);
            input1.Multiline = true;
            input1.Name = "input1";
            input1.PrefixText = "Span属性";
            input1.Size = new System.Drawing.Size(521, 66);
            input1.SuffixText = "";
            input1.TabIndex = 0;
            input1.Text = "10% 20%;20% 20%;30% 50%;-50% 10% 10%";
            input1.TextChanged += input1_TextChanged;
            // 
            // gridPanel1
            // 
            gridPanel1.Controls.Add(button6);
            gridPanel1.Controls.Add(button5);
            gridPanel1.Controls.Add(button4);
            gridPanel1.Controls.Add(button3);
            gridPanel1.Controls.Add(button2);
            gridPanel1.Controls.Add(button1);
            gridPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            gridPanel1.Location = new System.Drawing.Point(0, 147);
            gridPanel1.Name = "gridPanel1";
            gridPanel1.Size = new System.Drawing.Size(1000, 653);
            gridPanel1.Span = "10% 20%;20% 20%;30% 50%";
            gridPanel1.TabIndex = 2;
            gridPanel1.Text = "gridPanel1";
            // 
            // button6
            // 
            button6.BackExtend = "135, #6253E1, #04BEFE";
            button6.Location = new System.Drawing.Point(303, 437);
            button6.Name = "button6";
            button6.Size = new System.Drawing.Size(494, 211);
            button6.TabIndex = 5;
            button6.Text = "button6";
            button6.Type = AntdUI.TTypeMini.Primary;
            // 
            // button5
            // 
            button5.BackExtend = "135, #6253E1, #04BEFE";
            button5.Location = new System.Drawing.Point(3, 437);
            button5.Name = "button5";
            button5.Size = new System.Drawing.Size(294, 211);
            button5.TabIndex = 4;
            button5.Text = "button5";
            button5.Type = AntdUI.TTypeMini.Primary;
            // 
            // button4
            // 
            button4.BackExtend = "135, #6253E1, #04BEFE";
            button4.Location = new System.Drawing.Point(203, 220);
            button4.Name = "button4";
            button4.Size = new System.Drawing.Size(194, 211);
            button4.TabIndex = 3;
            button4.Text = "button4";
            button4.Type = AntdUI.TTypeMini.Primary;
            // 
            // button3
            // 
            button3.BackExtend = "135, #6253E1, #04BEFE";
            button3.Location = new System.Drawing.Point(3, 220);
            button3.Name = "button3";
            button3.Size = new System.Drawing.Size(194, 211);
            button3.TabIndex = 2;
            button3.Text = "button3";
            button3.Type = AntdUI.TTypeMini.Primary;
            // 
            // button2
            // 
            button2.BackExtend = "135, #6253E1, #04BEFE";
            button2.Location = new System.Drawing.Point(103, 3);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(194, 211);
            button2.TabIndex = 1;
            button2.Text = "button2";
            button2.Type = AntdUI.TTypeMini.Primary;
            // 
            // button1
            // 
            button1.BackExtend = "135, #6253E1, #04BEFE";
            button1.Location = new System.Drawing.Point(3, 3);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(94, 211);
            button1.TabIndex = 0;
            button1.Text = "button1";
            button1.Type = AntdUI.TTypeMini.Primary;
            // 
            // GridPanel
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(gridPanel1);
            Controls.Add(stackPanel1);
            Controls.Add(pageHeader1);
            Name = "GridPanel";
            Size = new System.Drawing.Size(1000, 800);
            stackPanel1.ResumeLayout(false);
            gridPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.PageHeader pageHeader1;
        private AntdUI.StackPanel stackPanel1;
        private AntdUI.Input input1;
        private AntdUI.Label label1;
        private AntdUI.GridPanel gridPanel1;
        private AntdUI.Button button4;
        private AntdUI.Button button3;
        private AntdUI.Button button2;
        private AntdUI.Button button1;
        private AntdUI.Button button6;
        private AntdUI.Button button5;
    }
}
