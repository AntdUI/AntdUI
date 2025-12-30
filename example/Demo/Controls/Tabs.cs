// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.Drawing;
using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class Tabs : UserControl
    {
        AntdUI.BaseForm form;
        public Tabs(AntdUI.BaseForm _form)
        {
            form = _form;
            InitializeComponent();

            for (int i = 0; i < 30; i++)
            {
                var it = new AntdUI.TabPage
                {
                    ReadOnly = i == 0,
                    IconSvg = i == 0 ? "AppleFilled" : null,
                    Text = "Tab" + (i + 1).ToString(),
                };
                it.Controls.Add(new AntdUI.Label
                {
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.TopLeft,
                    Text = "Content of Tab Pane " + (i + 1)
                });
                tabs_close.Pages.Add(it);
            }
        }
    }
}