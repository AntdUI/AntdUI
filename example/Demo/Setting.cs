// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.Windows.Forms;

namespace Demo
{
    public partial class Setting : UserControl
    {
        AntdUI.BaseForm form;

        public bool Animation, ShadowEnabled, ShowInWindow, ScrollBarHide, TextRenderingHighQuality;
        public Setting(AntdUI.BaseForm _form)
        {
            form = _form;
            InitializeComponent();
            switch1.Checked = Animation = AntdUI.Config.Animation;
            switch2.Checked = ShadowEnabled = AntdUI.Config.ShadowEnabled;
            switch3.Checked = ShowInWindow = AntdUI.Config.ShowInWindow;
            switch4.Checked = ScrollBarHide = AntdUI.Config.ScrollBarHide;
            switch5.Checked = TextRenderingHighQuality = AntdUI.Config.TextRenderingHighQuality;

            switch1.CheckedChanged += (s, e) =>
            {
                Animation = e.Value;
            };

            switch2.CheckedChanged += (s, e) =>
            {
                ShadowEnabled = e.Value;
            };

            switch3.CheckedChanged += (s, e) =>
            {
                ShowInWindow = e.Value;
            };

            switch4.CheckedChanged += (s, e) =>
            {
                ScrollBarHide = e.Value;
            };

            switch5.CheckedChanged += (s, e) =>
            {
                TextRenderingHighQuality = e.Value;
            };
        }
    }
}
