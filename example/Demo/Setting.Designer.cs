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

namespace Demo
{
    partial class Setting
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tablePanel = new System.Windows.Forms.TableLayoutPanel();
            label1 = new AntdUI.Label();
            label2 = new AntdUI.Switch();
            label3 = new AntdUI.Label();
            label4 = new AntdUI.Label();
            label5 = new AntdUI.Label();
            switch1 = new AntdUI.Switch();
            switch2 = new AntdUI.Switch();
            switch3 = new AntdUI.Switch();
            tablePanel.SuspendLayout();
            SuspendLayout();
            // 
            // tablePanel
            // 
            tablePanel.ColumnCount = 2;
            tablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            tablePanel.Controls.Add(label1, 0, 0);
            tablePanel.Controls.Add(label2, 1, 0);
            tablePanel.Controls.Add(label3, 0, 1);
            tablePanel.Controls.Add(label4, 0, 2);
            tablePanel.Controls.Add(label5, 0, 3);
            tablePanel.Controls.Add(switch1, 1, 1);
            tablePanel.Controls.Add(switch2, 1, 2);
            tablePanel.Controls.Add(switch3, 1, 3);
            tablePanel.Dock = System.Windows.Forms.DockStyle.Top;
            tablePanel.Location = new System.Drawing.Point(0, 0);
            tablePanel.Name = "tablePanel";
            tablePanel.RowCount = 5;
            tablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            tablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            tablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            tablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            tablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tablePanel.Size = new System.Drawing.Size(246, 212);
            tablePanel.TabIndex = 0;
            // 
            // label1
            // 
            label1.Dock = System.Windows.Forms.DockStyle.Fill;
            label1.LocalizationText = "AnimationEnabled";
            label1.Location = new System.Drawing.Point(3, 3);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(180, 40);
            label1.TabIndex = 0;
            label1.Text = "动画使能";
            // 
            // label2
            // 
            label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            label2.Location = new System.Drawing.Point(191, 8);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(50, 30);
            label2.TabIndex = 0;
            label2.Text = "label1";
            // 
            // label3
            // 
            label3.Dock = System.Windows.Forms.DockStyle.Fill;
            label3.LocalizationText = "ShadowEnabled";
            label3.Location = new System.Drawing.Point(3, 49);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(180, 40);
            label3.TabIndex = 0;
            label3.Text = "阴影使能";
            // 
            // label4
            // 
            label4.Dock = System.Windows.Forms.DockStyle.Fill;
            label4.LocalizationText = "PopupWindow";
            label4.Location = new System.Drawing.Point(3, 95);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(180, 40);
            label4.TabIndex = 0;
            label4.Text = "弹出在窗口";
            // 
            // label5
            // 
            label5.Dock = System.Windows.Forms.DockStyle.Fill;
            label5.LocalizationText = "ScrollBarHidden";
            label5.Location = new System.Drawing.Point(3, 141);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(180, 40);
            label5.TabIndex = 0;
            label5.Text = "滚动条隐藏样式";
            // 
            // switch1
            // 
            switch1.Anchor = System.Windows.Forms.AnchorStyles.None;
            switch1.Location = new System.Drawing.Point(191, 54);
            switch1.Name = "switch1";
            switch1.Size = new System.Drawing.Size(50, 30);
            switch1.TabIndex = 0;
            switch1.Text = "label1";
            // 
            // switch2
            // 
            switch2.Anchor = System.Windows.Forms.AnchorStyles.None;
            switch2.Location = new System.Drawing.Point(191, 100);
            switch2.Name = "switch2";
            switch2.Size = new System.Drawing.Size(50, 30);
            switch2.TabIndex = 0;
            switch2.Text = "label1";
            // 
            // switch3
            // 
            switch3.Anchor = System.Windows.Forms.AnchorStyles.None;
            switch3.Location = new System.Drawing.Point(191, 146);
            switch3.Name = "switch3";
            switch3.Size = new System.Drawing.Size(50, 30);
            switch3.TabIndex = 0;
            switch3.Text = "label1";
            // 
            // Setting
            // 
            Controls.Add(tablePanel);
            Name = "Setting";
            Size = new System.Drawing.Size(246, 205);
            tablePanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tablePanel;
        private AntdUI.Label label1;
        private AntdUI.Switch label2;
        private AntdUI.Label label3;
        private AntdUI.Label label4;
        private AntdUI.Label label5;
        private AntdUI.Switch switch1;
        private AntdUI.Switch switch2;
        private AntdUI.Switch switch3;
    }
}