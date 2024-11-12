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
    partial class Tree
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
            tree1 = new AntdUI.Tree();
            tree2 = new AntdUI.Tree();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Description = "多层次的结构列表。";
            header1.Dock = DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F);
            header1.LocalizationDescription = "Tree.Description";
            header1.LocalizationText = "Tree.Text";
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new Padding(0, 0, 0, 10);
            header1.Size = new Size(740, 74);
            header1.TabIndex = 0;
            header1.Text = "Tree 树形控件";
            header1.UseTitleFont = true;
            // 
            // tree1
            // 
            tree1.Checkable = true;
            tree1.Dock = DockStyle.Left;
            tree1.Font = new Font("Microsoft YaHei UI", 12F);
            tree1.Location = new Point(0, 74);
            tree1.Name = "tree1";
            tree1.Size = new Size(328, 328);
            tree1.TabIndex = 18;
            tree1.Text = "menu1";
            // 
            // tree2
            // 
            tree2.BlockNode = true;
            tree2.Dock = DockStyle.Fill;
            tree2.Font = new Font("Microsoft YaHei UI", 12F);
            tree2.Location = new Point(328, 74);
            tree2.Name = "tree2";
            tree2.Size = new Size(412, 328);
            tree2.TabIndex = 19;
            tree2.Text = "menu1";
            // 
            // Tree
            // 
            Controls.Add(tree2);
            Controls.Add(tree1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 12F);
            Name = "Tree";
            Size = new Size(740, 402);
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.PageHeader header1;
        private AntdUI.Tree tree1;
        private AntdUI.Tree tree2;
    }
}