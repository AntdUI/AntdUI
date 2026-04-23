// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

namespace ExtendedTest
{
    partial class RibbonDemo
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
            ribbon = new AntdUI.Ribbon();
            SuspendLayout();
            // 
            // ribbon
            // 
            ribbon.Dock = System.Windows.Forms.DockStyle.Top;
            ribbon.Location = new System.Drawing.Point(0, 0);
            ribbon.Name = "ribbon";
            ribbon.Size = new System.Drawing.Size(150, 122);
            ribbon.TabIndex = 0;
            // 
            // UserControl1
            // 
            Controls.Add(ribbon);
            Name = "UserControl1";
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.Ribbon ribbon;
    }
}
