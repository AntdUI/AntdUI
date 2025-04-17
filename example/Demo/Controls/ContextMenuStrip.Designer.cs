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
    partial class ContextMenuStrip
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
            SuspendLayout();
            // 
            // header1
            // 
            header1.Description = "任意点击当前页面的右键";
            header1.Dock = System.Windows.Forms.DockStyle.Top;
            header1.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            header1.LocalizationDescription = "Button.Description";
            header1.LocalizationText = "Button.Text";
            header1.Location = new System.Drawing.Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            header1.Size = new System.Drawing.Size(935, 74);
            header1.TabIndex = 1;
            header1.Text = "鼠标右键操作";
            header1.UseTitleFont = true;
            // 
            // ContextMenuStrip
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(header1);
            Name = "ContextMenuStrip";
            Size = new System.Drawing.Size(935, 749);
            Load += ContextMenuStrip_Load;
            MouseClick += ContextMenuStrip_MouseClick;
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.PageHeader header1;
    }
}
