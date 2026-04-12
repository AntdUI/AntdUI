// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class Button3 : UserControl
    {
        AntdUI.BaseForm form;
        public Button3(AntdUI.BaseForm _form)
        {
            form = _form;
            InitializeComponent();
            b4.LoadingWaveCount = b5.LoadingWaveCount = 2;
            b4.LoadingWaveColor = b5.LoadingWaveColor = AntdUI.Style.Db.Success;
        }

        private void btn_Click(object sender, System.EventArgs e)
        {
            if (sender is AntdUI.Button btn) btn.TestClickButton();
        }
    }
}