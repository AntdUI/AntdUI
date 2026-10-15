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
            slider1.KeyboardModifyEnabled = true;
            slider2.KeyboardModifyEnabled = true;
            slider3.KeyboardModifyEnabled = true;
            slider4.KeyboardModifyEnabled = true;
            slider5.KeyboardModifyEnabled = true;
            slider6.KeyboardModifyEnabled = true;
            slider7.KeyboardModifyEnabled = true;
            slider8.KeyboardModifyEnabled = true;
            slider1.WheelModifyEnabled = true;
            slider2.WheelModifyEnabled = true;
            slider3.WheelModifyEnabled = true;
            slider4.WheelModifyEnabled = true;
            slider5.WheelModifyEnabled = true;
            slider6.WheelModifyEnabled = true;
            slider7.WheelModifyEnabled = true;
            slider8.WheelModifyEnabled = true;
        }

        private string slider7_ValueFormatChanged(object sender, AntdUI.IntEventArgs e)
        {
            return e.Value + "℃";
        }
    }
}