// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.Globalization;
using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class InputNumber : UserControl
    {
        AntdUI.BaseForm form;
        public InputNumber(AntdUI.BaseForm _form)
        {
            form = _form;
            InitializeComponent();
            inputNumberWithValueFormatter.ValueFormatter += (sender, e) =>
            {
                if (e.Value % 1 == 0) return ((int)e.Value).ToString("N0");
                else return e.Value.ToString($"N{inputNumberWithValueFormatter.DecimalPlaces}", CultureInfo.CurrentCulture);
            };
        }
    }
}