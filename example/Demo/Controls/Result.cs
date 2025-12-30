// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class Result : UserControl
    {
        AntdUI.BaseForm form;
        public Result(AntdUI.BaseForm _form)
        {
            form = _form;
            InitializeComponent();
        }
    }
}