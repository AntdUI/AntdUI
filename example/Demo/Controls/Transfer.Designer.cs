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
            panel1 = new AntdUI.Panel();
            transfer1 = new AntdUI.Transfer();
            panel2 = new AntdUI.Panel();
            btn_reload = new AntdUI.Button();
            label_target = new AntdUI.Label();
            label_source = new AntdUI.Label();
            label1 = new AntdUI.Label();
            switch_oneWay = new AntdUI.Switch();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            panel1.Controls.Add(transfer1);
            panel1.Location = new Point(20, 100);
            panel1.Name = "panel1";
            panel1.Size = new Size(760, 380);
            panel1.TabIndex = 0;
            // 
            // transfer1
            // 
            transfer1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            transfer1.Location = new Point(20, 50);
            transfer1.MinimumSize = new Size(300, 200);
            transfer1.Name = "transfer1";
            transfer1.Size = new Size(720, 310);
            transfer1.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            panel2.Controls.Add(btn_reload);
            panel2.Controls.Add(label_target);
            panel2.Controls.Add(label_source);
            panel2.Controls.Add(label1);
            panel2.Controls.Add(switch_oneWay);
            panel2.Location = new Point(20, 20);
            panel2.Name = "panel2";
            panel2.Size = new Size(760, 70);
            panel2.TabIndex = 1;
            // 
            // btn_reload
            // 
            btn_reload.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btn_reload.BorderWidth = 1F;
            btn_reload.Location = new Point(650, 30);
            btn_reload.Name = "btn_reload";
            btn_reload.Size = new Size(90, 30);
            btn_reload.TabIndex = 6;
            btn_reload.Text = "重新加载";
            btn_reload.Click += btn_reload_Click;
            // 
            // label_target
            // 
            label_target.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            label_target.Location = new Point(520, 30);
            label_target.Name = "label_target";
            label_target.Size = new Size(120, 30);
            label_target.TabIndex = 5;
            label_target.Text = "目标列表: 0项";
            label_target.TextAlign = ContentAlignment.TopRight;
            // 
            // label_source
            // 
            label_source.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            label_source.Location = new Point(400, 30);
            label_source.Name = "label_source";
            label_source.Size = new Size(120, 30);
            label_source.TabIndex = 4;
            label_source.Text = "源列表: 10项";
            label_source.TextAlign = ContentAlignment.TopRight;
            // 
            // label1
            // 
            label1.Location = new Point(20, 30);
            label1.Name = "label1";
            label1.Size = new Size(80, 30);
            label1.TabIndex = 1;
            label1.Text = "单向模式:";
            // 
            // switch_oneWay
            // 
            switch_oneWay.Location = new Point(100, 30);
            switch_oneWay.Name = "switch_oneWay";
            switch_oneWay.Size = new Size(50, 30);
            switch_oneWay.TabIndex = 0;
            // 
            // Transfer
            // 
            Controls.Add(panel2);
            Controls.Add(panel1);
            Name = "Transfer";
            Size = new Size(800, 500);
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private AntdUI.Panel panel1;
        private AntdUI.Transfer transfer1;
        private AntdUI.Panel panel2;
        private AntdUI.Label label1;
        private AntdUI.Switch switch_oneWay;
        //private AntdUI.Label label2;
        //private AntdUI.Switch switch_search;
        private AntdUI.Label label_source;
        private AntdUI.Label label_target;
        private AntdUI.Button btn_reload;
    }
}