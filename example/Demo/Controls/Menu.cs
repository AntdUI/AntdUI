// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
// THE AntdUI PROJECT IS AN WINFORM LIBRARY LICENSED UNDER THE Apache-2.0 License.
// LICENSED UNDER THE Apache License, VERSION 2.0 (THE "License")
// YOU MAY NOT USE THIS FILE EXCEPT IN COMPLIANCE WITH THE License.
// YOU MAY OBTAIN A COPY OF THE LICENSE AT
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// UNLESS REQUIRED BY APPLICABLE LAW OR AGREED TO IN WRITING, SOFTWARE
// DISTRIBUTED UNDER THE LICENSE IS DISTRIBUTED ON AN "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED.
// SEE THE LICENSE FOR THE SPECIFIC LANGUAGE GOVERNING PERMISSIONS AND
// LIMITATIONS UNDER THE License.
// GITCODE: https://gitcode.com/AntdUI/AntdUI
// GITEE: https://gitee.com/AntdUI/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

using System.Drawing;
using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class Menu : UserControl
    {
        AntdUI.BaseForm form;
        public Menu(AntdUI.BaseForm _form)
        {
            form = _form;
            InitializeComponent();
            var lang = AntdUI.Localization.CurrentLanguage;
            if (lang.StartsWith("en")) switch2.Width = switch4.Width = 108;
        }

        private void switch1_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            if (e.Value)
            {
                menu2.ColorScheme = AntdUI.TAMode.Dark;
                if (AntdUI.Config.IsDark) menu2.BackColor = BackColor;
                else menu2.BackColor = Color.FromArgb(0, 21, 41);
            }
            else
            {
                menu2.ColorScheme = AntdUI.TAMode.Light;
                if (AntdUI.Config.IsDark) menu2.BackColor = Color.White;
                else menu2.BackColor = BackColor;
            }
        }

        private void switch2_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            menu2.Collapsed = e.Value;
            if (e.Value) menu2.Width = menu2.CollapseWidth;
            else menu2.Width = menu2.CollapsedWidth;
        }
        private void switch3_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            if (e.Value)
            {
                menu3.ColorScheme = AntdUI.TAMode.Dark;
                if (AntdUI.Config.IsDark) menu3.BackColor = BackColor;
                else menu3.BackColor = Color.FromArgb(0, 21, 41);
            }
            else
            {
                menu3.ColorScheme = AntdUI.TAMode.Light;
                if (AntdUI.Config.IsDark) menu3.BackColor = Color.White;
                else menu3.BackColor = BackColor;
            }
        }
        private void switch4_CheckedChanged(object sender, AntdUI.BoolEventArgs e) => menu3.Collapsed = e.Value;
        private void switch5_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            if (e.Value)
            {
                menu2.Tag = menu2.Mode;
                menu2.Mode = AntdUI.TMenuMode.InlineNoText;
                menu2.Width = (int)(66 * form.Dpi);
            }
            else if (menu2.Tag is AntdUI.TMenuMode mode)
            {
                menu2.Mode = mode;
                menu2.Width = (int)(251 * form.Dpi);
            }
        }
    }
}