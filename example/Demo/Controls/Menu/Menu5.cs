// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.Drawing;
using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class Menu5 : UserControl
    {
        AntdUI.BaseForm form;
        public Menu5(AntdUI.BaseForm _form)
        {
            form = _form;
            InitializeComponent();
            if (AntdUI.Localization.CurrentLanguage.StartsWith("en")) switch2.Width = 108;
        }

        private void switch1_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            if (e.Value)
            {
                menu1.ColorScheme = AntdUI.TAMode.Dark;
                menu1.BackColor = AntdUI.Config.IsDark ? BackColor : Color.FromArgb(0, 21, 41);
            }
            else
            {
                menu1.ColorScheme = AntdUI.TAMode.Light;
                menu1.BackColor = AntdUI.Config.IsDark ? Color.White : BackColor;
            }
        }

        private void switch2_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            menu1.Collapsed = e.Value;
            menu1.Width = e.Value ? (int)(66 * form.Dpi) : (int)(251 * form.Dpi);
        }
    }
}
