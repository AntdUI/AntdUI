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

namespace Demo
{
    partial class TabHeaderForm
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
            AntdUI.TagTabItem tagTabItem1 = new AntdUI.TagTabItem();
            tabHeader1 = new AntdUI.TabHeader();
            button1 = new AntdUI.Button();
            SuspendLayout();
            // 
            // tabHeader1
            // 
            tabHeader1.BackActive = System.Drawing.Color.White;
            tabHeader1.BackColor = System.Drawing.Color.FromArgb(232, 232, 232);
            tabHeader1.BorderWidth = 1F;
            tabHeader1.Dock = System.Windows.Forms.DockStyle.Top;
            tabHeader1.DragSort = true;
            tabHeader1.IconSvg = "ChromeFilled";
            tagTabItem1.Hover = false;
            tagTabItem1.HoverClose = false;
            tagTabItem1.ShowClose = false;
            tagTabItem1.IconSvg = "WeiboSquareFilled";
            tagTabItem1.Tag = null;
            tagTabItem1.Text = "🦄 首页";
            tabHeader1.Items.Add(tagTabItem1);
            tabHeader1.Location = new System.Drawing.Point(0, 0);
            tabHeader1.Name = "tabHeader1";
            tabHeader1.ShowAdd = true;
            tabHeader1.ShowButton = true;
            tabHeader1.ShowIcon = true;
            tabHeader1.Size = new System.Drawing.Size(830, 44);
            tabHeader1.TabIndex = 0;
            tabHeader1.AddClick += tabHeader1_AddClick;
            // 
            // button1
            // 
            button1.BorderWidth = 1F;
            button1.Location = new System.Drawing.Point(12, 66);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(105, 38);
            button1.TabIndex = 1;
            button1.Text = "添加标签页";
            button1.Click += button1_Click;
            // 
            // TabHeaderForm
            // 
            BackColor = System.Drawing.Color.White;
            ClientSize = new System.Drawing.Size(830, 414);
            Controls.Add(button1);
            Controls.Add(tabHeader1);
            Name = "TabHeaderForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "TabHeader";
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.TabHeader tabHeader1;
        private AntdUI.Button button1;
    }
}