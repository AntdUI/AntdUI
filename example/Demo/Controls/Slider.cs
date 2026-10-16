// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class Slider : UserControl
    {
        AntdUI.BaseForm form;
        public Slider(AntdUI.BaseForm _form)
        {
            form = _form;
            InitializeComponent();
        }

        private string slider7_ValueFormatChanged(object sender, AntdUI.IntEventArgs e)
        {
            return e.Value + "℃";
        }
    }
}