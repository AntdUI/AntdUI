// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class Segmented : UserControl
    {
        AntdUI.BaseForm form;
        public Segmented(AntdUI.BaseForm _form)
        {
            form = _form;
            InitializeComponent();
        }
    }
}