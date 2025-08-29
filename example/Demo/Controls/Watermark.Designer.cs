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

namespace Demo.Controls
{
    partial class Watermark
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
            if(disposing)
            {
                // 清理水印
                ClearWatermark();
            }
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
            panel1 = new System.Windows.Forms.Panel();
            panel8 = new AntdUI.Panel();
            divider1 = new AntdUI.Divider();
            label7 = new System.Windows.Forms.Label();
            btnClear = new AntdUI.Button();
            btnPanel = new AntdUI.Button();
            lblGap = new AntdUI.Label();
            trackGap = new AntdUI.Slider();
            lblRotate = new AntdUI.Label();
            trackRotate = new AntdUI.Slider();
            lblOpacity = new AntdUI.Label();
            trackOpacity = new AntdUI.Slider();
            label2 = new AntdUI.Label();
            inputContent2 = new AntdUI.Input();
            label1 = new AntdUI.Label();
            inputContent = new AntdUI.Input();
            label6 = new AntdUI.Label();
            colorPicker = new AntdUI.ColorPicker();
            btnForm = new AntdUI.Button();
            panel1.SuspendLayout();
            panel8.SuspendLayout();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Description = "给页面的某个区域加上水印。";
            header1.Dock = System.Windows.Forms.DockStyle.Top;
            header1.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            header1.LocalizationDescription = "Watermark.Description";
            header1.LocalizationText = "Watermark.Text";
            header1.Location = new System.Drawing.Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            header1.Size = new System.Drawing.Size(835, 74);
            header1.TabIndex = 1;
            header1.Text = "Watermark 水印";
            header1.UseTitleFont = true;
            header1.BackClick += header1_BackClick;
            // 
            // panel1
            // 
            panel1.Controls.Add(panel8);
            panel1.Controls.Add(btnClear);
            panel1.Controls.Add(btnPanel);
            panel1.Controls.Add(lblGap);
            panel1.Controls.Add(trackGap);
            panel1.Controls.Add(lblRotate);
            panel1.Controls.Add(trackRotate);
            panel1.Controls.Add(lblOpacity);
            panel1.Controls.Add(trackOpacity);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(inputContent2);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(inputContent);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(colorPicker);
            panel1.Controls.Add(btnForm);
            panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            panel1.Location = new System.Drawing.Point(0, 74);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(835, 560);
            panel1.TabIndex = 4;
            // 
            // panel8
            // 
            panel8.ArrowAlign = AntdUI.TAlign.TL;
            panel8.ArrowSize = 10;
            panel8.Controls.Add(divider1);
            panel8.Controls.Add(label7);
            panel8.Location = new System.Drawing.Point(368, 17);
            panel8.Name = "panel8";
            panel8.Radius = 10;
            panel8.Shadow = 24;
            panel8.ShadowOpacity = 0.18F;
            panel8.ShadowOpacityAnimation = true;
            panel8.Size = new System.Drawing.Size(451, 397);
            panel8.TabIndex = 41;
            // 
            // divider1
            // 
            divider1.BackColor = System.Drawing.Color.Transparent;
            divider1.Dock = System.Windows.Forms.DockStyle.Top;
            divider1.Location = new System.Drawing.Point(24, 72);
            divider1.Margin = new System.Windows.Forms.Padding(10);
            divider1.Name = "divider1";
            divider1.Size = new System.Drawing.Size(403, 1);
            divider1.TabIndex = 1;
            // 
            // label7
            // 
            label7.BackColor = System.Drawing.Color.Transparent;
            label7.Dock = System.Windows.Forms.DockStyle.Top;
            label7.Font = new System.Drawing.Font("Microsoft YaHei UI", 15.75F, System.Drawing.FontStyle.Bold);
            label7.Location = new System.Drawing.Point(24, 24);
            label7.Name = "label7";
            label7.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            label7.Size = new System.Drawing.Size(403, 48);
            label7.TabIndex = 0;
            label7.Text = "面板水印";
            label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnClear
            // 
            btnClear.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            btnClear.Location = new System.Drawing.Point(250, 400);
            btnClear.Name = "btnClear";
            btnClear.Size = new System.Drawing.Size(99, 40);
            btnClear.TabIndex = 40;
            btnClear.Text = "清除水印";
            btnClear.Type = AntdUI.TTypeMini.Error;
            btnClear.Click += btnClear_Click;
            // 
            // btnPanel
            // 
            btnPanel.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            btnPanel.Location = new System.Drawing.Point(145, 400);
            btnPanel.Name = "btnPanel";
            btnPanel.Size = new System.Drawing.Size(99, 40);
            btnPanel.TabIndex = 39;
            btnPanel.Text = "面板水印";
            btnPanel.Type = AntdUI.TTypeMini.Primary;
            btnPanel.Click += btnPanel_Click;
            // 
            // lblGap
            // 
            lblGap.Location = new System.Drawing.Point(38, 322);
            lblGap.Name = "lblGap";
            lblGap.Size = new System.Drawing.Size(206, 26);
            lblGap.TabIndex = 38;
            lblGap.Text = "间距: 50px";
            // 
            // trackGap
            // 
            trackGap.Location = new System.Drawing.Point(38, 354);
            trackGap.MaxValue = 500;
            trackGap.MinValue = 50;
            trackGap.Name = "trackGap";
            trackGap.ShowValue = true;
            trackGap.Size = new System.Drawing.Size(245, 40);
            trackGap.TabIndex = 37;
            trackGap.Value = 50;
            trackGap.ValueChanged += trackGap_ValueChanged;
            // 
            // lblRotate
            // 
            lblRotate.Location = new System.Drawing.Point(38, 244);
            lblRotate.Name = "lblRotate";
            lblRotate.Size = new System.Drawing.Size(206, 26);
            lblRotate.TabIndex = 36;
            lblRotate.Text = "旋转角度：-22°";
            // 
            // trackRotate
            // 
            trackRotate.Location = new System.Drawing.Point(38, 276);
            trackRotate.MaxValue = 90;
            trackRotate.MinValue = -90;
            trackRotate.Name = "trackRotate";
            trackRotate.ShowValue = true;
            trackRotate.Size = new System.Drawing.Size(245, 40);
            trackRotate.TabIndex = 35;
            trackRotate.Value = 22;
            trackRotate.ValueChanged += trackRotate_ValueChanged;
            // 
            // lblOpacity
            // 
            lblOpacity.Location = new System.Drawing.Point(38, 166);
            lblOpacity.Name = "lblOpacity";
            lblOpacity.Size = new System.Drawing.Size(206, 26);
            lblOpacity.TabIndex = 34;
            lblOpacity.Text = "透明度：15%";
            // 
            // trackOpacity
            // 
            trackOpacity.Location = new System.Drawing.Point(38, 198);
            trackOpacity.MinValue = 1;
            trackOpacity.Name = "trackOpacity";
            trackOpacity.ShowValue = true;
            trackOpacity.Size = new System.Drawing.Size(245, 40);
            trackOpacity.TabIndex = 33;
            trackOpacity.Value = 15;
            trackOpacity.ValueChanged += trackOpacity_ValueChanged;
            // 
            // label2
            // 
            label2.Location = new System.Drawing.Point(20, 116);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(81, 26);
            label2.TabIndex = 32;
            label2.Text = "水印颜色：";
            label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // inputContent2
            // 
            inputContent2.Location = new System.Drawing.Point(102, 56);
            inputContent2.Name = "inputContent2";
            inputContent2.Size = new System.Drawing.Size(260, 44);
            inputContent2.TabIndex = 31;
            // 
            // label1
            // 
            label1.Location = new System.Drawing.Point(20, 65);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(81, 26);
            label1.TabIndex = 30;
            label1.Text = "副内容：";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // inputContent
            // 
            inputContent.Location = new System.Drawing.Point(102, 6);
            inputContent.Name = "inputContent";
            inputContent.Size = new System.Drawing.Size(260, 44);
            inputContent.TabIndex = 29;
            inputContent.Text = "AntdUI Watermark";
            // 
            // label6
            // 
            label6.Location = new System.Drawing.Point(20, 15);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(81, 26);
            label6.TabIndex = 28;
            label6.Text = "水印内容：";
            label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // colorPicker
            // 
            colorPicker.AutoSizeMode = AntdUI.TAutoSize.Auto;
            colorPicker.Location = new System.Drawing.Point(102, 106);
            colorPicker.Name = "colorPicker";
            colorPicker.ShowText = true;
            colorPicker.Size = new System.Drawing.Size(125, 46);
            colorPicker.TabIndex = 27;
            colorPicker.Value = System.Drawing.Color.Black;
            colorPicker.ValueChanged += colorPicker_ValueChanged;
            // 
            // btnForm
            // 
            btnForm.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            btnForm.Location = new System.Drawing.Point(38, 400);
            btnForm.Name = "btnForm";
            btnForm.Size = new System.Drawing.Size(99, 40);
            btnForm.TabIndex = 1;
            btnForm.Text = "窗体水印";
            btnForm.Type = AntdUI.TTypeMini.Primary;
            btnForm.Click += btnForm_Click;
            // 
            // Watermark
            // 
            Controls.Add(panel1);
            Controls.Add(header1);
            Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            Name = "Watermark";
            Size = new System.Drawing.Size(835, 634);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel8.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.PageHeader header1;
        private System.Windows.Forms.Panel panel1;
        private AntdUI.Button btnForm;
        private AntdUI.ColorPicker colorPicker;
        private AntdUI.Label label6;
        private AntdUI.Input inputContent;
        private AntdUI.Input inputContent2;
        private AntdUI.Label label1;
        private AntdUI.Label label2;
        private AntdUI.Slider trackOpacity;
        private AntdUI.Label lblOpacity;
        private AntdUI.Label lblRotate;
        private AntdUI.Slider trackRotate;
        private AntdUI.Label lblGap;
        private AntdUI.Slider trackGap;
        private AntdUI.Button btnPanel;
        private AntdUI.Button btnClear;
        private AntdUI.Panel panel8;
        private AntdUI.Divider divider1;
        private System.Windows.Forms.Label label7;
    }
}
