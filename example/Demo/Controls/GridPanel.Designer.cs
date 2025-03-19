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
            header1 = new AntdUI.PageHeader();
            label1 = new AntdUI.Label();
            input1 = new AntdUI.Input();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            gridPanel1 = new AntdUI.GridPanel();
            button12 = new AntdUI.Button();
            button11 = new AntdUI.Button();
            button10 = new AntdUI.Button();
            button9 = new AntdUI.Button();
            button8 = new AntdUI.Button();
            button7 = new AntdUI.Button();
            button6 = new AntdUI.Button();
            button5 = new AntdUI.Button();
            button4 = new AntdUI.Button();
            button3 = new AntdUI.Button();
            button2 = new AntdUI.Button();
            button1 = new AntdUI.Button();
            tableLayoutPanel1.SuspendLayout();
            gridPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Description = "精准划分区域的格栅布局容器。";
            header1.Dock = System.Windows.Forms.DockStyle.Top;
            header1.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            header1.LocalizationDescription = "GridPanel.Description";
            header1.LocalizationText = "GridPanel.Text";
            header1.Location = new System.Drawing.Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            header1.Size = new System.Drawing.Size(1000, 75);
            header1.TabIndex = 0;
            header1.Text = "GridPanel 格栅布局";
            header1.UseTitleFont = true;
            // 
            // label1
            // 
            label1.Dock = System.Windows.Forms.DockStyle.Fill;
            label1.LocalizationText = "GridPanel.Describe";
            label1.Location = new System.Drawing.Point(503, 3);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(494, 60);
            label1.TabIndex = 1;
            label1.Text = "以-为分解。-前为列宽属性，-后为行高属性列宽以；分组。每一组代表一行\r\n行高属性个数要与行数对应,每个数之间用空格间隔";
            // 
            // input1
            // 
            input1.Dock = System.Windows.Forms.DockStyle.Fill;
            input1.LocalizationPrefixText = "GridPanel.Prefix";
            input1.Location = new System.Drawing.Point(0, 0);
            input1.Margin = new System.Windows.Forms.Padding(0);
            input1.Multiline = true;
            input1.Name = "input1";
            input1.PrefixText = "Span属性";
            input1.Size = new System.Drawing.Size(500, 66);
            input1.SuffixText = "";
            input1.TabIndex = 0;
            input1.Text = "100%;25% 25% 25% 25%;33.33% 33.33% 33.33%;50% 50%;66.66% 33.33%;-20% 20% 18% 21% 21%";
            input1.TextChanged += input1_TextChanged;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(label1, 1, 0);
            tableLayoutPanel1.Controls.Add(input1, 0, 0);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            tableLayoutPanel1.Location = new System.Drawing.Point(0, 75);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new System.Drawing.Size(1000, 66);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // gridPanel1
            // 
            gridPanel1.Controls.Add(button12);
            gridPanel1.Controls.Add(button11);
            gridPanel1.Controls.Add(button10);
            gridPanel1.Controls.Add(button9);
            gridPanel1.Controls.Add(button8);
            gridPanel1.Controls.Add(button7);
            gridPanel1.Controls.Add(button6);
            gridPanel1.Controls.Add(button5);
            gridPanel1.Controls.Add(button4);
            gridPanel1.Controls.Add(button3);
            gridPanel1.Controls.Add(button2);
            gridPanel1.Controls.Add(button1);
            gridPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            gridPanel1.Font = new System.Drawing.Font("Microsoft YaHei UI", 22F);
            gridPanel1.Location = new System.Drawing.Point(0, 141);
            gridPanel1.Name = "gridPanel1";
            gridPanel1.Size = new System.Drawing.Size(1000, 659);
            gridPanel1.Span = "100%;25% 25% 25% 25%;33.33% 33.33% 33.33%;50% 50%;66.66% 33.33%;-20% 20% 18% 21% 21%";
            gridPanel1.TabIndex = 3;
            gridPanel1.Paint += gridPanel1_Paint;
            // 
            // button12
            // 
            button12.Ghost = true;
            button12.Location = new System.Drawing.Point(667, 523);
            button12.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            button12.Name = "button12";
            button12.Radius = 0;
            button12.Size = new System.Drawing.Size(333, 134);
            button12.TabIndex = 12;
            button12.Text = "33.33%";
            button12.WaveSize = 0;
            // 
            // button11
            // 
            button11.Location = new System.Drawing.Point(0, 523);
            button11.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            button11.Name = "button11";
            button11.Radius = 0;
            button11.Size = new System.Drawing.Size(667, 134);
            button11.TabIndex = 11;
            button11.Text = "66.66%";
            button11.Type = AntdUI.TTypeMini.Primary;
            button11.WaveSize = 0;
            // 
            // button10
            // 
            button10.Ghost = true;
            button10.Location = new System.Drawing.Point(500, 385);
            button10.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            button10.Name = "button10";
            button10.Radius = 0;
            button10.Size = new System.Drawing.Size(500, 134);
            button10.TabIndex = 10;
            button10.Text = "50%";
            button10.WaveSize = 0;
            // 
            // button9
            // 
            button9.Location = new System.Drawing.Point(0, 385);
            button9.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            button9.Name = "button9";
            button9.Radius = 0;
            button9.Size = new System.Drawing.Size(500, 134);
            button9.TabIndex = 9;
            button9.Text = "50%";
            button9.Type = AntdUI.TTypeMini.Primary;
            button9.WaveSize = 0;
            // 
            // button8
            // 
            button8.Location = new System.Drawing.Point(666, 266);
            button8.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            button8.Name = "button8";
            button8.Radius = 0;
            button8.Size = new System.Drawing.Size(333, 115);
            button8.TabIndex = 8;
            button8.Text = "33.33%";
            button8.Type = AntdUI.TTypeMini.Primary;
            button8.WaveSize = 0;
            // 
            // button7
            // 
            button7.Ghost = true;
            button7.Location = new System.Drawing.Point(333, 266);
            button7.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            button7.Name = "button7";
            button7.Radius = 0;
            button7.Size = new System.Drawing.Size(333, 115);
            button7.TabIndex = 7;
            button7.Text = "33.33%";
            button7.WaveSize = 0;
            // 
            // button6
            // 
            button6.Location = new System.Drawing.Point(0, 266);
            button6.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            button6.Name = "button6";
            button6.Radius = 0;
            button6.Size = new System.Drawing.Size(333, 115);
            button6.TabIndex = 6;
            button6.Text = "33.33%";
            button6.Type = AntdUI.TTypeMini.Primary;
            button6.WaveSize = 0;
            // 
            // button5
            // 
            button5.Ghost = true;
            button5.Location = new System.Drawing.Point(750, 134);
            button5.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            button5.Name = "button5";
            button5.Radius = 0;
            button5.Size = new System.Drawing.Size(250, 128);
            button5.TabIndex = 5;
            button5.Text = "25%";
            button5.WaveSize = 0;
            // 
            // button4
            // 
            button4.Location = new System.Drawing.Point(500, 134);
            button4.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            button4.Name = "button4";
            button4.Radius = 0;
            button4.Size = new System.Drawing.Size(250, 128);
            button4.TabIndex = 4;
            button4.Text = "25%";
            button4.Type = AntdUI.TTypeMini.Primary;
            button4.WaveSize = 0;
            // 
            // button3
            // 
            button3.Ghost = true;
            button3.Location = new System.Drawing.Point(250, 134);
            button3.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            button3.Name = "button3";
            button3.Radius = 0;
            button3.Size = new System.Drawing.Size(250, 128);
            button3.TabIndex = 3;
            button3.Text = "25%";
            button3.WaveSize = 0;
            // 
            // button2
            // 
            button2.Location = new System.Drawing.Point(0, 134);
            button2.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            button2.Name = "button2";
            button2.Radius = 0;
            button2.Size = new System.Drawing.Size(250, 128);
            button2.TabIndex = 2;
            button2.Text = "25%";
            button2.Type = AntdUI.TTypeMini.Primary;
            button2.WaveSize = 0;
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(0, 2);
            button1.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            button1.Name = "button1";
            button1.Radius = 0;
            button1.Size = new System.Drawing.Size(1000, 128);
            button1.TabIndex = 1;
            button1.Text = "100%";
            button1.Type = AntdUI.TTypeMini.Primary;
            button1.WaveSize = 0;
            // 
            // GridPanel
            // 
            Controls.Add(gridPanel1);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(header1);
            Name = "GridPanel";
            Size = new System.Drawing.Size(1000, 800);
            tableLayoutPanel1.ResumeLayout(false);
            gridPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.PageHeader header1;
        private AntdUI.Input input1;
        private AntdUI.Label label1;
        private AntdUI.GridPanel gridPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private AntdUI.Button button12;
        private AntdUI.Button button11;
        private AntdUI.Button button10;
        private AntdUI.Button button9;
        private AntdUI.Button button8;
        private AntdUI.Button button7;
        private AntdUI.Button button6;
        private AntdUI.Button button5;
        private AntdUI.Button button4;
        private AntdUI.Button button3;
        private AntdUI.Button button2;
        private AntdUI.Button button1;
    }
}
