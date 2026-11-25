// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Windows.Forms;

namespace AntdUI
{
    internal class DoubleBufferForm : Form
    {
        bool UFocus = false;
        public DoubleBufferForm(Form form, Control control, bool focus = true) : this()
        {
            UFocus = !focus;
            Tag = form;
            control.Dock = DockStyle.Fill;
            Controls.Add(control);
        }

        public DoubleBufferForm()
        {
            SetStyle(
                 ControlStyles.UserPaint |
                 ControlStyles.AllPaintingInWmPaint |
                 ControlStyles.OptimizedDoubleBuffer |
                 ControlStyles.ResizeRedraw, true);
            UpdateStyles();
            ShowInTaskbar = false;
        }

        /// <summary>所属多层级浮层会话；句柄创建后自动注册为 Content 层并触发一次 Z 序重排。</summary>
        internal LayeredWindowSession? Session;

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            Session?.Attach(this, LayeredRole.Content);
        }

        #region 无焦点窗体

        protected override bool ShowWithoutActivation => UFocus;

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if (UFocus && m.Msg == 0x21)
            {
                m.Result = new IntPtr(3);
                return;
            }
            try
            {
                base.WndProc(ref m);
            }
            catch { }
        }

        #endregion
    }
}