// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

namespace ExtendedTest
{
    partial class DockDemo
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
            dock = new AntdUI.DockPanel();
            toolbar = new AntdUI.Panel();
            label1 = new AntdUI.Label();
            btnLoad = new AntdUI.Button();
            btnSave = new AntdUI.Button();
            btnRestore = new AntdUI.Button();
            btnAutoHide = new AntdUI.Button();
            btnFloat = new AntdUI.Button();
            toolbar.SuspendLayout();
            SuspendLayout();
            // 
            // dock
            // 
            dock.Dock = System.Windows.Forms.DockStyle.Fill;
            dock.Location = new System.Drawing.Point(0, 44);
            dock.Name = "dock";
            dock.Size = new System.Drawing.Size(831, 309);
            dock.TabIndex = 0;
            // 
            // toolbar
            // 
            toolbar.Controls.Add(label1);
            toolbar.Controls.Add(btnLoad);
            toolbar.Controls.Add(btnSave);
            toolbar.Controls.Add(btnRestore);
            toolbar.Controls.Add(btnAutoHide);
            toolbar.Controls.Add(btnFloat);
            toolbar.Dock = System.Windows.Forms.DockStyle.Top;
            toolbar.Location = new System.Drawing.Point(0, 0);
            toolbar.Name = "toolbar";
            toolbar.Padding = new System.Windows.Forms.Padding(8);
            toolbar.Size = new System.Drawing.Size(831, 44);
            toolbar.TabIndex = 1;
            // 
            // label1
            // 
            label1.BackColor = System.Drawing.Color.Transparent;
            label1.Dock = System.Windows.Forms.DockStyle.Fill;
            label1.Location = new System.Drawing.Point(498, 8);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(325, 28);
            label1.TabIndex = 0;
            label1.Text = "Dock buttons let you Float/AutoHide/Restore/Save/Load. Close tabs with × on each tab header.";
            // 
            // btnLoad
            // 
            btnLoad.AutoSizeMode = AntdUI.TAutoSize.Width;
            btnLoad.Dock = System.Windows.Forms.DockStyle.Left;
            btnLoad.Location = new System.Drawing.Point(403, 8);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new System.Drawing.Size(95, 28);
            btnLoad.TabIndex = 4;
            btnLoad.Text = "Load Layout";
            btnLoad.Click += btnLoad_Click;
            // 
            // btnSave
            // 
            btnSave.AutoSizeMode = AntdUI.TAutoSize.Width;
            btnSave.Dock = System.Windows.Forms.DockStyle.Left;
            btnSave.Location = new System.Drawing.Point(310, 8);
            btnSave.Name = "btnSave";
            btnSave.Size = new System.Drawing.Size(93, 28);
            btnSave.TabIndex = 3;
            btnSave.Text = "Save Layout";
            btnSave.Click += btnSave_Click;
            // 
            // btnRestore
            // 
            btnRestore.AutoSizeMode = AntdUI.TAutoSize.Width;
            btnRestore.Dock = System.Windows.Forms.DockStyle.Left;
            btnRestore.Location = new System.Drawing.Point(223, 8);
            btnRestore.Name = "btnRestore";
            btnRestore.Size = new System.Drawing.Size(87, 28);
            btnRestore.TabIndex = 2;
            btnRestore.Text = "Restore All";
            btnRestore.Click += btnRestore_Click;
            // 
            // btnAutoHide
            // 
            btnAutoHide.AutoSizeMode = AntdUI.TAutoSize.Width;
            btnAutoHide.Dock = System.Windows.Forms.DockStyle.Left;
            btnAutoHide.Location = new System.Drawing.Point(100, 8);
            btnAutoHide.Name = "btnAutoHide";
            btnAutoHide.Size = new System.Drawing.Size(123, 28);
            btnAutoHide.TabIndex = 1;
            btnAutoHide.Text = "Auto-Hide Active";
            btnAutoHide.Click += btnAutoHide_Click;
            // 
            // btnFloat
            // 
            btnFloat.AutoSizeMode = AntdUI.TAutoSize.Width;
            btnFloat.Dock = System.Windows.Forms.DockStyle.Left;
            btnFloat.Location = new System.Drawing.Point(8, 8);
            btnFloat.Name = "btnFloat";
            btnFloat.Size = new System.Drawing.Size(92, 28);
            btnFloat.TabIndex = 0;
            btnFloat.Text = "Float Active";
            btnFloat.Click += btnFloat_Click;
            // 
            // DockDemo
            // 
            Controls.Add(dock);
            Controls.Add(toolbar);
            Name = "DockDemo";
            Size = new System.Drawing.Size(831, 353);
            toolbar.ResumeLayout(false);
            toolbar.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.DockPanel dock;
        private AntdUI.Panel toolbar;
        private AntdUI.Button btnRestore;
        private AntdUI.Button btnAutoHide;
        private AntdUI.Button btnFloat;
        private AntdUI.Button btnLoad;
        private AntdUI.Button btnSave;
        private AntdUI.Label label1;
    }
}
