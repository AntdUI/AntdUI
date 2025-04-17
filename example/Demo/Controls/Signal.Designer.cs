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
    partial class Signal
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
            signal1 = new AntdUI.Signal();
            signal2 = new AntdUI.Signal();
            signal3 = new AntdUI.Signal();
            button2 = new AntdUI.Button();
            button1 = new AntdUI.Button();
            header1 = new AntdUI.PageHeader();
            SuspendLayout();
            // 
            // signal1
            // 
            signal1.Location = new System.Drawing.Point(23, 116);
            signal1.Name = "signal1";
            signal1.Size = new System.Drawing.Size(108, 58);
            signal1.TabIndex = 1;
            signal1.Text = "signal1";
            signal1.Value = 1;
            // 
            // signal2
            // 
            signal2.Location = new System.Drawing.Point(173, 116);
            signal2.Name = "signal2";
            signal2.Size = new System.Drawing.Size(135, 58);
            signal2.TabIndex = 2;
            signal2.Text = "signal2";
            signal2.Value = 2;
            // 
            // signal3
            // 
            signal3.Location = new System.Drawing.Point(353, 116);
            signal3.Name = "signal3";
            signal3.Size = new System.Drawing.Size(127, 58);
            signal3.TabIndex = 3;
            signal3.Text = "signal3";
            signal3.Value = 4;
            // 
            // button2
            // 
            button2.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button2.Location = new System.Drawing.Point(212, 198);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(45, 41);
            button2.TabIndex = 10;
            button2.Text = "减";
            button2.Type = AntdUI.TTypeMini.Success;
            button2.Click += button2_Click;
            // 
            // button1
            // 
            button1.AutoSizeMode = AntdUI.TAutoSize.Auto;
            button1.Location = new System.Drawing.Point(54, 198);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(45, 41);
            button1.TabIndex = 9;
            button1.Text = "加";
            button1.Type = AntdUI.TTypeMini.Primary;
            button1.Click += button1_Click;
            // 
            // header1
            // 
            header1.Description = "信号显示";
            header1.Dock = System.Windows.Forms.DockStyle.Top;
            header1.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            header1.LocalizationDescription = "Button.Description";
            header1.LocalizationText = "Button.Text";
            header1.Location = new System.Drawing.Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            header1.Size = new System.Drawing.Size(579, 81);
            header1.TabIndex = 11;
            header1.Text = "Signal 信号显示";
            header1.UseTitleFont = true;
            // 
            // Signal
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(header1);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(signal3);
            Controls.Add(signal2);
            Controls.Add(signal1);
            Name = "Signal";
            Size = new System.Drawing.Size(579, 395);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private AntdUI.Signal signal1;
        private AntdUI.Signal signal2;
        private AntdUI.Signal signal3;
        private AntdUI.Button button2;
        private AntdUI.Button button1;
        private AntdUI.PageHeader header1;
    }
}
