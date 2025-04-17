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
// GITEE: https://gitee.com/AntdUI/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

namespace Demo.Controls
{
    partial class Battery
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
            battery1 = new AntdUI.Battery();
            battery4 = new AntdUI.Battery();
            battery5 = new AntdUI.Battery();
            button1 = new AntdUI.Button();
            button2 = new AntdUI.Button();
            header1 = new AntdUI.PageHeader();
            SuspendLayout();
            // 
            // battery1
            // 
            battery1.DotSize = 16;
            battery1.Location = new System.Drawing.Point(198, 69);
            battery1.Name = "battery1";
            battery1.Size = new System.Drawing.Size(187, 50);
            battery1.TabIndex = 0;
            battery1.Text = "battery1";
            battery1.Value = 30;
            // 
            // battery4
            // 
            battery4.DotSize = 16;
            battery4.Location = new System.Drawing.Point(3, 69);
            battery4.Name = "battery4";
            battery4.Size = new System.Drawing.Size(189, 50);
            battery4.TabIndex = 5;
            battery4.Text = "battery4";
            battery4.Value = 60;
            // 
            // battery5
            // 
            battery5.DotSize = 16;
            battery5.Location = new System.Drawing.Point(415, 69);
            battery5.Name = "battery5";
            battery5.Size = new System.Drawing.Size(181, 50);
            battery5.TabIndex = 6;
            battery5.Text = "battery5";
            battery5.Value = 15;
            // 
            // button1
            // 
            button1.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button1.Location = new System.Drawing.Point(30, 244);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(70, 41);
            button1.TabIndex = 7;
            button1.Text = "加电量";
            button1.Type = AntdUI.TTypeMini.Primary;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button2.Location = new System.Drawing.Point(147, 244);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(70, 41);
            button2.TabIndex = 8;
            button2.Text = "减电量";
            button2.Type = AntdUI.TTypeMini.Success;
            button2.Click += button2_Click;
            // 
            // header1
            // 
            header1.Description = "电量显示操作";
            header1.Dock = System.Windows.Forms.DockStyle.Top;
            header1.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            header1.LocalizationDescription = "Button.Description";
            header1.LocalizationText = "Button.Text";
            header1.Location = new System.Drawing.Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            header1.Size = new System.Drawing.Size(596, 81);
            header1.TabIndex = 9;
            header1.Text = "Battery 电量显示";
            header1.UseTitleFont = true;
            // 
            // Battery
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(header1);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(battery5);
            Controls.Add(battery4);
            Controls.Add(battery1);
            Name = "Battery";
            Size = new System.Drawing.Size(596, 445);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private AntdUI.Battery battery1;
        private AntdUI.Battery battery4;
        private AntdUI.Battery battery5;
        private AntdUI.Button button1;
        private AntdUI.Button button2;
        private AntdUI.PageHeader header1;
    }
}
