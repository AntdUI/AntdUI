// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.Drawing;
using System.Windows.Forms;

namespace MarkdownDemo
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
            header1 = new AntdUI.PageHeader();
            splitter1 = new AntdUI.Splitter();
            input = new AntdUI.Input();
            render = new AntdUI.Markdown();
            ((System.ComponentModel.ISupportInitialize)splitter1).BeginInit();
            splitter1.Panel1.SuspendLayout();
            splitter1.Panel2.SuspendLayout();
            splitter1.SuspendLayout();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Dock = DockStyle.Top;
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new Padding(0, 0, 0, 6);
            header1.ShowButton = true;
            header1.ShowIcon = true;
            header1.Size = new Size(1280, 60);
            header1.SubText = "左侧编辑实时渲染；拖入 .md 文件自动设置 BasePath";
            header1.TabIndex = 0;
            header1.Text = "Markdown 渲染演示";
            // 
            // splitter1
            // 
            splitter1.Dock = DockStyle.Fill;
            splitter1.Location = new Point(0, 60);
            splitter1.Name = "splitter1";
            // 
            // splitter1.Panel1
            // 
            splitter1.Panel1.Controls.Add(input);
            // 
            // splitter1.Panel2
            // 
            splitter1.Panel2.Controls.Add(render);
            splitter1.Size = new Size(1280, 780);
            splitter1.SplitterDistance = 637;
            splitter1.SplitterWidth = 6;
            splitter1.TabIndex = 1;
            // 
            // input
            // 
            input.AcceptsTab = true;
            input.AutoScroll = true;
            input.Dock = DockStyle.Fill;
            input.Location = new Point(0, 0);
            input.Multiline = true;
            input.Name = "input";
            input.PlaceholderText = "在此输入 Markdown，右侧实时渲染；也可直接拖入 .md 文件";
            input.Size = new Size(637, 780);
            input.TabIndex = 0;
            // 
            // render
            // 
            render.Dock = DockStyle.Fill;
            render.Location = new Point(0, 0);
            render.Name = "render";
            render.Padding = new Padding(12);
            render.Size = new Size(637, 780);
            render.TabIndex = 0;
            // 
            // MainForm
            // 
            ClientSize = new Size(1280, 840);
            Controls.Add(splitter1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 11F);
            MinimumSize = new Size(800, 560);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Markdown 渲染演示";
            splitter1.Panel1.ResumeLayout(false);
            splitter1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitter1).EndInit();
            splitter1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.PageHeader header1;
        private AntdUI.Splitter splitter1;
        private AntdUI.Input input;
        private AntdUI.Markdown render;
    }
}