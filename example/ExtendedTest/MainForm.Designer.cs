// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.Drawing;
using System.Windows.Forms;

namespace ExtendedTest
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
            AntdUI.Tabs.StyleLine styleLine2 = new AntdUI.Tabs.StyleLine();
            top = new AntdUI.PageHeader();
            colorTheme = new AntdUI.ColorPicker();
            btn_mode = new AntdUI.Button();
            tab = new AntdUI.Tabs();
            log = new AntdUI.Input();
            top.SuspendLayout();
            SuspendLayout();
            // 
            // top
            // 
            top.Controls.Add(colorTheme);
            top.Controls.Add(btn_mode);
            top.Dock = DockStyle.Top;
            top.Location = new Point(0, 0);
            top.Name = "top";
            top.ShowButton = true;
            top.ShowIcon = true;
            top.Size = new Size(1280, 36);
            top.TabIndex = 0;
            top.Text = "AntdUI Extended Controls — Test Harness";
            // 
            // colorTheme
            // 
            colorTheme.Dock = DockStyle.Right;
            colorTheme.Location = new Point(1064, 0);
            colorTheme.Name = "colorTheme";
            colorTheme.Padding = new Padding(4);
            colorTheme.Size = new Size(36, 36);
            colorTheme.TabIndex = 10;
            colorTheme.ValueChanged += colorTheme_ValueChanged;
            // 
            // btn_mode
            // 
            btn_mode.Dock = DockStyle.Right;
            btn_mode.Ghost = true;
            btn_mode.IconSvg = "SunOutlined";
            btn_mode.Location = new Point(1100, 0);
            btn_mode.Name = "btn_mode";
            btn_mode.Radius = 0;
            btn_mode.Size = new Size(36, 36);
            btn_mode.TabIndex = 9;
            btn_mode.ToggleIconSvg = "MoonOutlined";
            btn_mode.WaveSize = 0;
            btn_mode.Click += btn_mode_Click;
            // 
            // tab
            // 
            tab.Dock = DockStyle.Fill;
            tab.Location = new Point(0, 36);
            tab.Name = "tab";
            tab.Size = new Size(1280, 664);
            tab.Style = styleLine2;
            tab.TabIndex = 1;
            // 
            // log
            // 
            log.BorderWidth = 0F;
            log.Dock = DockStyle.Bottom;
            log.Location = new Point(0, 700);
            log.Multiline = true;
            log.Name = "log";
            log.Radius = 0;
            log.ReadOnly = true;
            log.Size = new Size(1280, 140);
            log.TabIndex = 2;
            log.WaveSize = 0;
            // 
            // MainForm
            // 
            ClientSize = new Size(1280, 840);
            Controls.Add(tab);
            Controls.Add(log);
            Controls.Add(top);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "AntdUI Extended Controls — Test Harness";
            top.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.PageHeader top;
        private AntdUI.ColorPicker colorTheme;
        private AntdUI.Button btn_mode;
        private AntdUI.Tabs tab;
        private AntdUI.Input log;
    }
}