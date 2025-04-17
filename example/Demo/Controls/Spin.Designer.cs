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
    partial class Spin
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
            LoadBtn = new AntdUI.Button();
            header1 = new AntdUI.PageHeader();
            SuspendLayout();
            // 
            // LoadBtn
            // 
            LoadBtn.AutoSizeMode = AntdUI.TAutoSize.Auto;
            LoadBtn.Location = new System.Drawing.Point(438, 262);
            LoadBtn.Name = "LoadBtn";
            LoadBtn.Size = new System.Drawing.Size(82, 41);
            LoadBtn.TabIndex = 1;
            LoadBtn.Text = "加载耗时";
            LoadBtn.Type = AntdUI.TTypeMini.Primary;
            LoadBtn.Click += LoadBtn_Click;
            // 
            // header1
            // 
            header1.Description = "耗时加载操作";
            header1.Dock = System.Windows.Forms.DockStyle.Top;
            header1.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            header1.LocalizationDescription = "Button.Description";
            header1.LocalizationText = "Button.Text";
            header1.Location = new System.Drawing.Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            header1.Size = new System.Drawing.Size(1041, 81);
            header1.TabIndex = 12;
            header1.Text = "Spin 加载操作";
            header1.UseTitleFont = true;
            // 
            // Spin
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(header1);
            Controls.Add(LoadBtn);
            Name = "Spin";
            Size = new System.Drawing.Size(1041, 607);
            Load += Spin_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private AntdUI.Button LoadBtn;
        private AntdUI.PageHeader header1;
    }
}
