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
    partial class Spin
    {
        /// <summary> 
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        ///  Clean up any resources being used.
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

        private void InitializeComponent()
        {
            btnPanel = new AntdUI.Button();
            header1 = new AntdUI.PageHeader();
            divider1 = new AntdUI.Divider();
            stackPanel1 = new AntdUI.StackPanel();
            spin2 = new AntdUI.Spin();
            spin1 = new AntdUI.Spin();
            stackPanel2 = new AntdUI.StackPanel();
            spin3 = new AntdUI.Spin();
            spin4 = new AntdUI.Spin();
            divider2 = new AntdUI.Divider();
            divider3 = new AntdUI.Divider();
            stackPanel3 = new AntdUI.StackPanel();
            btnWindow = new AntdUI.Button();
            btnControl = new AntdUI.Button();
            buttonError = new AntdUI.Button();
            stackPanel1.SuspendLayout();
            stackPanel2.SuspendLayout();
            stackPanel3.SuspendLayout();
            SuspendLayout();
            // 
            // btnPanel
            // 
            btnPanel.AutoSizeMode = AntdUI.TAutoSize.Width;
            btnPanel.LocalizationText = "Spin.{id}";
            btnPanel.Location = new Point(3, 3);
            btnPanel.Name = "btnPanel";
            btnPanel.Size = new Size(93, 42);
            btnPanel.TabIndex = 1;
            btnPanel.Text = "当前容器";
            btnPanel.Type = AntdUI.TTypeMini.Primary;
            btnPanel.Click += btnPanel_Click;
            // 
            // header1
            // 
            header1.Description = "用于页面和区块的加载中状态。";
            header1.Dock = System.Windows.Forms.DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F);
            header1.LocalizationDescription = "Spin.Description";
            header1.LocalizationText = "Spin.Text";
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            header1.Size = new Size(596, 81);
            header1.TabIndex = 12;
            header1.Text = "Spin 加载中";
            header1.UseTitleFont = true;
            // 
            // divider1
            // 
            divider1.Dock = System.Windows.Forms.DockStyle.Top;
            divider1.Font = new Font("Microsoft YaHei UI", 10F);
            divider1.LocalizationText = "Spin.{id}";
            divider1.Location = new Point(0, 81);
            divider1.Name = "divider1";
            divider1.Orientation = AntdUI.TOrientation.Left;
            divider1.Size = new Size(596, 28);
            divider1.TabIndex = 0;
            divider1.Text = "直接使用";
            // 
            // stackPanel1
            // 
            stackPanel1.Controls.Add(spin2);
            stackPanel1.Controls.Add(spin1);
            stackPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            stackPanel1.Location = new Point(0, 109);
            stackPanel1.Name = "stackPanel1";
            stackPanel1.Size = new Size(596, 48);
            stackPanel1.TabIndex = 1;
            // 
            // spin2
            // 
            spin2.Fill = Color.FromArgb(255, 87, 34);
            spin2.Location = new Point(83, 3);
            spin2.Name = "spin2";
            spin2.Size = new Size(74, 42);
            spin2.TabIndex = 1;
            // 
            // spin1
            // 
            spin1.Location = new Point(3, 3);
            spin1.Name = "spin1";
            spin1.Size = new Size(74, 42);
            spin1.TabIndex = 0;
            // 
            // stackPanel2
            // 
            stackPanel2.Controls.Add(spin3);
            stackPanel2.Controls.Add(spin4);
            stackPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            stackPanel2.Location = new Point(0, 185);
            stackPanel2.Name = "stackPanel2";
            stackPanel2.Size = new Size(596, 100);
            stackPanel2.TabIndex = 2;
            // 
            // spin3
            // 
            spin3.Fill = Color.FromArgb(255, 87, 34);
            spin3.LocalizationText = "Loading";
            spin3.Location = new Point(129, 3);
            spin3.Name = "spin3";
            spin3.Size = new Size(120, 94);
            spin3.TabIndex = 1;
            spin3.Text = "加载中";
            // 
            // spin4
            // 
            spin4.LocalizationText = "Loading";
            spin4.Location = new Point(3, 3);
            spin4.Name = "spin4";
            spin4.Size = new Size(120, 94);
            spin4.TabIndex = 0;
            spin4.Text = "加载中";
            // 
            // divider2
            // 
            divider2.Dock = System.Windows.Forms.DockStyle.Top;
            divider2.Font = new Font("Microsoft YaHei UI", 10F);
            divider2.LocalizationText = "Spin.{id}";
            divider2.Location = new Point(0, 157);
            divider2.Name = "divider2";
            divider2.Orientation = AntdUI.TOrientation.Left;
            divider2.Size = new Size(596, 28);
            divider2.TabIndex = 0;
            divider2.Text = "显示文字";
            // 
            // divider3
            // 
            divider3.Dock = System.Windows.Forms.DockStyle.Top;
            divider3.Font = new Font("Microsoft YaHei UI", 10F);
            divider3.LocalizationText = "Spin.{id}";
            divider3.Location = new Point(0, 285);
            divider3.Name = "divider3";
            divider3.Orientation = AntdUI.TOrientation.Left;
            divider3.Size = new Size(596, 28);
            divider3.TabIndex = 0;
            divider3.Text = "基本用法";
            // 
            // stackPanel3
            // 
            stackPanel3.Controls.Add(buttonError);
            stackPanel3.Controls.Add(btnWindow);
            stackPanel3.Controls.Add(btnControl);
            stackPanel3.Controls.Add(btnPanel);
            stackPanel3.Dock = System.Windows.Forms.DockStyle.Top;
            stackPanel3.Location = new Point(0, 313);
            stackPanel3.Name = "stackPanel3";
            stackPanel3.Size = new Size(596, 48);
            stackPanel3.TabIndex = 3;
            // 
            // btnWindow
            // 
            btnWindow.AutoSizeMode = AntdUI.TAutoSize.Width;
            btnWindow.LocalizationText = "Spin.{id}";
            btnWindow.Location = new Point(201, 3);
            btnWindow.Name = "btnWindow";
            btnWindow.Size = new Size(93, 42);
            btnWindow.TabIndex = 3;
            btnWindow.Text = "整个窗口";
            btnWindow.Type = AntdUI.TTypeMini.Primary;
            btnWindow.Click += btnWindow_Click;
            // 
            // btnControl
            // 
            btnControl.AutoSizeMode = AntdUI.TAutoSize.Width;
            btnControl.LocalizationText = "Spin.{id}";
            btnControl.Location = new Point(102, 3);
            btnControl.Name = "btnControl";
            btnControl.Size = new Size(93, 42);
            btnControl.TabIndex = 2;
            btnControl.Text = "上面控件";
            btnControl.Type = AntdUI.TTypeMini.Primary;
            btnControl.Click += btnControl_Click;
            // 
            // buttonError
            // 
            buttonError.AutoSizeMode = AntdUI.TAutoSize.Width;
            buttonError.LocalizationText = "Spin.{id}";
            buttonError.Location = new Point(300, 3);
            buttonError.Name = "buttonError";
            buttonError.Size = new Size(109, 42);
            buttonError.TabIndex = 4;
            buttonError.Text = "带错误回调";
            buttonError.Type = AntdUI.TTypeMini.Error;
            buttonError.Click += buttonError_Click;
            // 
            // Spin
            // 
            Controls.Add(stackPanel3);
            Controls.Add(divider3);
            Controls.Add(stackPanel2);
            Controls.Add(divider2);
            Controls.Add(stackPanel1);
            Controls.Add(divider1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 12F);
            Name = "Spin";
            Size = new Size(596, 445);
            stackPanel1.ResumeLayout(false);
            stackPanel2.ResumeLayout(false);
            stackPanel3.ResumeLayout(false);
            stackPanel3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.Button btnPanel;
        private AntdUI.PageHeader header1;
        private AntdUI.Divider divider1;
        private AntdUI.StackPanel stackPanel1;
        private AntdUI.Spin spin1;
        private AntdUI.Spin spin2;
        private AntdUI.StackPanel stackPanel2;
        private AntdUI.Spin spin3;
        private AntdUI.Spin spin4;
        private AntdUI.Divider divider2;
        private AntdUI.Divider divider3;
        private AntdUI.StackPanel stackPanel3;
        private AntdUI.Button btnWindow;
        private AntdUI.Button btnControl;
        private AntdUI.Button buttonError;
    }
}