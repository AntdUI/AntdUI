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

using System.Drawing;
using System.Windows.Forms;

namespace Demo.Controls
{
    partial class Result
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

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            header1 = new AntdUI.PageHeader();
            label1 = new AntdUI.Label();
            label2 = new AntdUI.Label();
            button1 = new AntdUI.Button();
            button2 = new AntdUI.Button();
            iconComplete1 = new AntdUI.Icon.IconComplete();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Description = "用于反馈一系列操作任务的处理结果。";
            header1.Dock = DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F);
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new Padding(0, 0, 0, 10);
            header1.Size = new Size(677, 74);
            header1.TabIndex = 0;
            header1.Text = "Result 结果";
            header1.UseTitleFont = true;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label1.Font = new Font("HarmonyOS Sans SC", 18F);
            label1.Location = new Point(70, 223);
            label1.Name = "label1";
            label1.Size = new Size(537, 42);
            label1.TabIndex = 1;
            label1.Text = "Successfully Purchased Cloud Server ECS!";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label2.ForeColor = Color.FromArgb(115, 0, 0, 0);
            label2.Location = new Point(58, 270);
            label2.Name = "label2";
            label2.Size = new Size(561, 52);
            label2.TabIndex = 1;
            label2.Text = "Order number: 2017182818828182881 Cloud server configuration takes 1-5 minutes, please wait.";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Top;
            button1.Location = new Point(207, 380);
            button1.Name = "button1";
            button1.Size = new Size(117, 46);
            button1.TabIndex = 2;
            button1.Text = "Go Console";
            button1.Type = AntdUI.TTypeMini.Primary;
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Top;
            button2.BorderWidth = 1F;
            button2.Location = new Point(336, 380);
            button2.Name = "button2";
            button2.Size = new Size(104, 46);
            button2.TabIndex = 3;
            button2.Text = "Buy Again";
            // 
            // iconComplete1
            // 
            iconComplete1.Anchor = AnchorStyles.Top;
            iconComplete1.Location = new Point(288, 99);
            iconComplete1.Name = "iconComplete1";
            iconComplete1.Size = new Size(100, 100);
            iconComplete1.TabIndex = 4;
            // 
            // Result
            // 
            Controls.Add(iconComplete1);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 12F);
            Name = "Result";
            Size = new Size(677, 450);
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.PageHeader header1;
        private AntdUI.Label label1;
        private AntdUI.Label label2;
        private AntdUI.Button button1;
        private AntdUI.Button button2;
        private AntdUI.Icon.IconComplete iconComplete1;
    }
}