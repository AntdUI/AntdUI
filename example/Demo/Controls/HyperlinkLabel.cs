// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class HyperlinkLabel : UserControl
    {
        AntdUI.BaseForm form;

        public HyperlinkLabel(AntdUI.BaseForm _form)
        {
            form = _form;
            InitializeComponent();

            // 测试样式
            hyperlinkLabel4.NormalStyle = new AntdUI.HyperlinkLabel.LinkAppearance
            {
                LinkColor = AntdUI.Style.Db.Error,
                UnderlineThickness = 1
            };
            hyperlinkLabel4.HoverStyle = new AntdUI.HyperlinkLabel.LinkAppearance
            {
                LinkColor = AntdUI.Style.Db.ErrorActive,
                UnderlineThickness = 2
            };
        }

        private void hyperlinkLabel_LinkClicked(object sender, AntdUI.HyperlinkLabel.LinkClickedEventArgs e)
        {
            if (sender == hyperlinkLabel2) AntdUI.Message.success(FindForm(), "居中链接被点击: " + e.Text);
            else if (sender == hyperlinkLabel3) AntdUI.Message.success(FindForm(), "带徽章的链接被点击: " + e.Text);
            else AntdUI.Message.success(form, "点击了: " + e.Text);
        }
    }
}