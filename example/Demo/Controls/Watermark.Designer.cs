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
            panel2 = new AntdUI.Panel();
            btnClear = new AntdUI.Button();
            btnPanel = new AntdUI.Button();
            btnForm = new AntdUI.Button();
            panel8 = new AntdUI.Panel();
            divider1 = new AntdUI.Divider();
            label1 = new AntdUI.Label();
            lblGap = new AntdUI.Label();
            trackGap = new AntdUI.Slider();
            lblRotate = new AntdUI.Label();
            trackRotate = new AntdUI.Slider();
            lblOpacity = new AntdUI.Label();
            trackOpacity = new AntdUI.Slider();
            label4 = new AntdUI.Label();
            label3 = new AntdUI.Label();
            label2 = new AntdUI.Label();
            lblForeColor = new AntdUI.Label();
            inputContent2 = new AntdUI.Input();
            lblSub = new AntdUI.Label();
            inputContent = new AntdUI.Input();
            lblContent = new AntdUI.Label();
            colorPicker = new AntdUI.ColorPicker();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
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
            // 
            // panel1
            // 
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(panel8);
            panel1.Controls.Add(lblGap);
            panel1.Controls.Add(trackGap);
            panel1.Controls.Add(lblRotate);
            panel1.Controls.Add(trackRotate);
            panel1.Controls.Add(lblOpacity);
            panel1.Controls.Add(trackOpacity);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(lblForeColor);
            panel1.Controls.Add(inputContent2);
            panel1.Controls.Add(lblSub);
            panel1.Controls.Add(inputContent);
            panel1.Controls.Add(lblContent);
            panel1.Controls.Add(colorPicker);
            panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            panel1.Location = new System.Drawing.Point(0, 74);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(835, 560);
            panel1.TabIndex = 4;
            // 
            // panel2
            // 
            panel2.Controls.Add(btnClear);
            panel2.Controls.Add(btnPanel);
            panel2.Controls.Add(btnForm);
            panel2.Location = new System.Drawing.Point(54, 440);
            panel2.Name = "panel2";
            panel2.Radius = 0;
            panel2.Size = new System.Drawing.Size(708, 40);
            panel2.TabIndex = 7;
            // 
            // btnClear
            // 
            btnClear.AutoSizeMode = AntdUI.TAutoSize.Width;
            btnClear.Dock = System.Windows.Forms.DockStyle.Left;
            btnClear.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            btnClear.LocalizationText = "Watermark.{id}";
            btnClear.Location = new System.Drawing.Point(186, 0);
            btnClear.Name = "btnClear";
            btnClear.Size = new System.Drawing.Size(93, 40);
            btnClear.TabIndex = 2;
            btnClear.Text = "清除水印";
            btnClear.Type = AntdUI.TTypeMini.Error;
            btnClear.Click += btnClear_Click;
            // 
            // btnPanel
            // 
            btnPanel.AutoSizeMode = AntdUI.TAutoSize.Width;
            btnPanel.Dock = System.Windows.Forms.DockStyle.Left;
            btnPanel.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            btnPanel.LocalizationText = "Watermark.{id}";
            btnPanel.Location = new System.Drawing.Point(93, 0);
            btnPanel.Name = "btnPanel";
            btnPanel.Size = new System.Drawing.Size(93, 40);
            btnPanel.TabIndex = 1;
            btnPanel.Text = "面板水印";
            btnPanel.Type = AntdUI.TTypeMini.Primary;
            btnPanel.Click += btnPanel_Click;
            // 
            // btnForm
            // 
            btnForm.AutoSizeMode = AntdUI.TAutoSize.Width;
            btnForm.Dock = System.Windows.Forms.DockStyle.Left;
            btnForm.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            btnForm.LocalizationText = "Watermark.{id}";
            btnForm.Location = new System.Drawing.Point(0, 0);
            btnForm.Name = "btnForm";
            btnForm.Size = new System.Drawing.Size(93, 40);
            btnForm.TabIndex = 0;
            btnForm.Text = "窗体水印";
            btnForm.Type = AntdUI.TTypeMini.Primary;
            btnForm.Click += btnForm_Click;
            // 
            // panel8
            // 
            panel8.ArrowAlign = AntdUI.TAlign.TL;
            panel8.ArrowSize = 10;
            panel8.Controls.Add(divider1);
            panel8.Controls.Add(label1);
            panel8.Location = new System.Drawing.Point(384, 22);
            panel8.Name = "panel8";
            panel8.Radius = 10;
            panel8.Shadow = 24;
            panel8.ShadowOpacity = 0.18F;
            panel8.ShadowOpacityAnimation = true;
            panel8.Size = new System.Drawing.Size(451, 397);
            panel8.TabIndex = 20;
            // 
            // divider1
            // 
            divider1.BackColor = System.Drawing.Color.Transparent;
            divider1.Dock = System.Windows.Forms.DockStyle.Top;
            divider1.Location = new System.Drawing.Point(24, 72);
            divider1.Margin = new System.Windows.Forms.Padding(10);
            divider1.Name = "divider1";
            divider1.Size = new System.Drawing.Size(403, 1);
            divider1.TabIndex = 0;
            // 
            // label1
            // 
            label1.BackColor = System.Drawing.Color.Transparent;
            label1.Dock = System.Windows.Forms.DockStyle.Top;
            label1.Font = new System.Drawing.Font("Microsoft YaHei UI", 15.75F, System.Drawing.FontStyle.Bold);
            label1.LocalizationText = "Watermark.btnPanel";
            label1.Location = new System.Drawing.Point(24, 24);
            label1.Name = "label1";
            label1.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            label1.Size = new System.Drawing.Size(403, 48);
            label1.TabIndex = 0;
            label1.Text = "面板水印";
            // 
            // lblGap
            // 
            lblGap.LocalizationText = "Watermark.{id}";
            lblGap.Location = new System.Drawing.Point(19, 327);
            lblGap.Name = "lblGap";
            lblGap.Size = new System.Drawing.Size(98, 26);
            lblGap.TabIndex = 0;
            lblGap.Text = "间距：";
            lblGap.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // trackGap
            // 
            trackGap.Location = new System.Drawing.Point(54, 359);
            trackGap.MaxValue = 500;
            trackGap.MinValue = 50;
            trackGap.Name = "trackGap";
            trackGap.ShowValue = true;
            trackGap.Size = new System.Drawing.Size(245, 40);
            trackGap.TabIndex = 6;
            trackGap.Value = 50;
            trackGap.ValueChanged += trackGap_ValueChanged;
            // 
            // lblRotate
            // 
            lblRotate.LocalizationText = "Watermark.{id}";
            lblRotate.Location = new System.Drawing.Point(19, 249);
            lblRotate.Name = "lblRotate";
            lblRotate.Size = new System.Drawing.Size(98, 26);
            lblRotate.TabIndex = 0;
            lblRotate.Text = "旋转角度：";
            lblRotate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // trackRotate
            // 
            trackRotate.Location = new System.Drawing.Point(54, 281);
            trackRotate.MaxValue = 90;
            trackRotate.MinValue = -90;
            trackRotate.Name = "trackRotate";
            trackRotate.ShowValue = true;
            trackRotate.Size = new System.Drawing.Size(245, 40);
            trackRotate.TabIndex = 5;
            trackRotate.Value = 22;
            trackRotate.ValueChanged += trackRotate_ValueChanged;
            // 
            // lblOpacity
            // 
            lblOpacity.LocalizationText = "Watermark.{id}";
            lblOpacity.Location = new System.Drawing.Point(19, 171);
            lblOpacity.Name = "lblOpacity";
            lblOpacity.Size = new System.Drawing.Size(98, 26);
            lblOpacity.TabIndex = 0;
            lblOpacity.Text = "透明度：";
            lblOpacity.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // trackOpacity
            // 
            trackOpacity.Location = new System.Drawing.Point(54, 203);
            trackOpacity.MinValue = 1;
            trackOpacity.Name = "trackOpacity";
            trackOpacity.ShowValue = true;
            trackOpacity.Size = new System.Drawing.Size(245, 40);
            trackOpacity.TabIndex = 4;
            trackOpacity.Value = 15;
            trackOpacity.ValueChanged += trackOpacity_ValueChanged;
            // 
            // label4
            // 
            label4.Location = new System.Drawing.Point(124, 327);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(81, 26);
            label4.TabIndex = 0;
            label4.Text = "50px";
            // 
            // label3
            // 
            label3.Location = new System.Drawing.Point(124, 249);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(81, 26);
            label3.TabIndex = 0;
            label3.Text = "-22°";
            // 
            // label2
            // 
            label2.Location = new System.Drawing.Point(124, 171);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(81, 26);
            label2.TabIndex = 0;
            label2.Text = "15%";
            // 
            // lblForeColor
            // 
            lblForeColor.LocalizationText = "Watermark.{id}";
            lblForeColor.Location = new System.Drawing.Point(19, 121);
            lblForeColor.Name = "lblForeColor";
            lblForeColor.Size = new System.Drawing.Size(98, 26);
            lblForeColor.TabIndex = 0;
            lblForeColor.Text = "水印颜色：";
            lblForeColor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // inputContent2
            // 
            inputContent2.Location = new System.Drawing.Point(118, 61);
            inputContent2.Name = "inputContent2";
            inputContent2.Size = new System.Drawing.Size(260, 44);
            inputContent2.TabIndex = 2;
            // 
            // lblSub
            // 
            lblSub.LocalizationText = "Watermark.{id}";
            lblSub.Location = new System.Drawing.Point(19, 70);
            lblSub.Name = "lblSub";
            lblSub.Size = new System.Drawing.Size(98, 26);
            lblSub.TabIndex = 0;
            lblSub.Text = "副内容：";
            lblSub.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // inputContent
            // 
            inputContent.Location = new System.Drawing.Point(118, 11);
            inputContent.Name = "inputContent";
            inputContent.Size = new System.Drawing.Size(260, 44);
            inputContent.TabIndex = 1;
            inputContent.Text = "AntdUI Watermark";
            // 
            // lblContent
            // 
            lblContent.LocalizationText = "Watermark.{id}";
            lblContent.Location = new System.Drawing.Point(19, 20);
            lblContent.Name = "lblContent";
            lblContent.Size = new System.Drawing.Size(98, 26);
            lblContent.TabIndex = 0;
            lblContent.Text = "水印内容：";
            lblContent.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // colorPicker
            // 
            colorPicker.AutoSizeMode = AntdUI.TAutoSize.Auto;
            colorPicker.Location = new System.Drawing.Point(118, 111);
            colorPicker.Name = "colorPicker";
            colorPicker.ShowText = true;
            colorPicker.Size = new System.Drawing.Size(125, 46);
            colorPicker.TabIndex = 3;
            colorPicker.Value = System.Drawing.Color.FromArgb(22, 119, 255);
            colorPicker.ValueChanged += colorPicker_ValueChanged;
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
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel8.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.PageHeader header1;
        private System.Windows.Forms.Panel panel1;
        private AntdUI.Button btnForm;
        private AntdUI.ColorPicker colorPicker;
        private AntdUI.Label lblContent;
        private AntdUI.Input inputContent;
        private AntdUI.Input inputContent2;
        private AntdUI.Label lblSub;
        private AntdUI.Label lblForeColor;
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
        private AntdUI.Label label1;
        private AntdUI.Panel panel2;
        private AntdUI.Label label4;
        private AntdUI.Label label3;
        private AntdUI.Label label2;
    }
}
