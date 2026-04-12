// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class Button6 : UserControl
    {
        AntdUI.BaseForm form;
        public Button6(AntdUI.BaseForm _form)
        {
            form = _form;
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            UpdatePanelWidth();
            b1.SizeChanged += btn_SizeChanged;
            b2.SizeChanged += btn_SizeChanged;
            b3.SizeChanged += btn_SizeChanged;
        }

        private void UpdatePanelWidth()
        {
            panel_btns.Width = b1.Width + b2.Width + b3.Width + panel_btns.Padding.Horizontal + (int)(panel_btns.Shadow * form.Dpi) * 2;
        }

        private void btn_SizeChanged(object sender, EventArgs e) => UpdatePanelWidth();

        private void btn_Click(object sender, System.EventArgs e)
        {
            if (sender is AntdUI.Button btn) btn.TestClickButton();
        }
    }
}