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

using System.Drawing;

namespace Demo.Controls
{
    partial class Transfer
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
            panel1 = new AntdUI.Panel();
            label_target = new AntdUI.Label();
            label_source = new AntdUI.Label();
            switch_oneWay = new AntdUI.Switch();
            btn_reload = new AntdUI.Button();
            label1 = new AntdUI.Label();
            transfer1 = new AntdUI.Transfer();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Description = "双栏穿梭选择框，用于在两个区域之间移动元素。";
            header1.Dock = System.Windows.Forms.DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F);
            header1.LocalizationDescription = "Transfer.Description";
            header1.LocalizationText = "Transfer.Text";
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            header1.Size = new Size(800, 74);
            header1.TabIndex = 0;
            header1.Text = "Transfer 穿梭框";
            header1.UseTitleFont = true;
            // 
            // panel1
            // 
            panel1.Controls.Add(label_target);
            panel1.Controls.Add(label_source);
            panel1.Controls.Add(switch_oneWay);
            panel1.Controls.Add(btn_reload);
            panel1.Controls.Add(label1);
            panel1.Dock = System.Windows.Forms.DockStyle.Top;
            panel1.Location = new Point(0, 74);
            panel1.Name = "panel1";
            panel1.Radius = 0;
            panel1.Size = new Size(800, 36);
            panel1.TabIndex = 1;
            // 
            // label_target
            // 
            label_target.AutoSizeMode = AntdUI.TAutoSize.Width;
            label_target.BackColor = Color.Transparent;
            label_target.Dock = System.Windows.Forms.DockStyle.Right;
            label_target.LocalizationText = "Transfer.TargetT";
            label_target.Location = new Point(614, 0);
            label_target.Name = "label_target";
            label_target.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label_target.Size = new Size(63, 36);
            label_target.TabIndex = 5;
            label_target.Text = "目标列表: ";
            label_target.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label_source
            // 
            label_source.AutoSizeMode = AntdUI.TAutoSize.Width;
            label_source.BackColor = Color.Transparent;
            label_source.Dock = System.Windows.Forms.DockStyle.Right;
            label_source.LocalizationText = "Transfer.SourceT";
            label_source.Location = new Point(677, 0);
            label_source.Name = "label_source";
            label_source.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label_source.Size = new Size(51, 36);
            label_source.TabIndex = 4;
            label_source.Text = "源列表: ";
            label_source.TextAlign = ContentAlignment.MiddleRight;
            // 
            // switch_oneWay
            // 
            switch_oneWay.BackColor = Color.Transparent;
            switch_oneWay.Dock = System.Windows.Forms.DockStyle.Left;
            switch_oneWay.Location = new Point(59, 0);
            switch_oneWay.Name = "switch_oneWay";
            switch_oneWay.Padding = new System.Windows.Forms.Padding(0, 4, 0, 4);
            switch_oneWay.Size = new Size(40, 36);
            switch_oneWay.TabIndex = 0;
            // 
            // btn_reload
            // 
            btn_reload.AutoSizeMode = AntdUI.TAutoSize.Width;
            btn_reload.BorderWidth = 1F;
            btn_reload.Dock = System.Windows.Forms.DockStyle.Right;
            btn_reload.LocalizationText = "Transfer.Reload";
            btn_reload.Location = new Point(728, 0);
            btn_reload.Name = "btn_reload";
            btn_reload.Size = new Size(72, 36);
            btn_reload.TabIndex = 6;
            btn_reload.Text = "重新加载";
            btn_reload.Click += btn_reload_Click;
            // 
            // label1
            // 
            label1.AutoSizeMode = AntdUI.TAutoSize.Width;
            label1.BackColor = Color.Transparent;
            label1.Dock = System.Windows.Forms.DockStyle.Left;
            label1.LocalizationText = "Transfer.One";
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Size = new Size(59, 36);
            label1.TabIndex = 1;
            label1.Text = "单向模式:";
            // 
            // transfer1
            // 
            transfer1.Dock = System.Windows.Forms.DockStyle.Fill;
            transfer1.Location = new Point(0, 110);
            transfer1.MinimumSize = new Size(300, 200);
            transfer1.Name = "transfer1";
            transfer1.Padding = new System.Windows.Forms.Padding(8);
            transfer1.Size = new Size(800, 390);
            transfer1.TabIndex = 0;
            // 
            // Transfer
            // 
            Controls.Add(transfer1);
            Controls.Add(panel1);
            Controls.Add(header1);
            Name = "Transfer";
            Size = new Size(800, 500);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private AntdUI.PageHeader header1;
        private AntdUI.Panel panel1;
        private AntdUI.Label label1;
        private AntdUI.Switch switch_oneWay;
        private AntdUI.Label label_source;
        private AntdUI.Label label_target;
        private AntdUI.Button btn_reload;
        private AntdUI.Transfer transfer1;
    }
}