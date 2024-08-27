﻿// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
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
// GITEE: https://gitee.com/antdui/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

using System.Windows.Forms;

namespace Overview
{
    public partial class Setting : UserControl
    {
        Form form;

        public bool Animation, ShadowEnabled, ShowInWindow, ScrollBarHide;
        public Setting(Form _form)
        {
            InitializeComponent();
            label2.Checked = Animation = AntdUI.Config.Animation;
            switch1.Checked = ShadowEnabled = AntdUI.Config.ShadowEnabled;
            switch2.Checked = ShowInWindow = AntdUI.Config.ShowInWindow;
            switch3.Checked = ScrollBarHide = AntdUI.Config.ScrollBarHide;

            label2.CheckedChanged += (s, e) =>
            {
                Animation = e.Value;
            };

            switch1.CheckedChanged += (s, e) =>
            {
                ShadowEnabled = e.Value;
            };

            switch2.CheckedChanged += (s, e) =>
            {
                ShowInWindow = e.Value;
            };

            switch3.CheckedChanged += (s, e) =>
            {
                ScrollBarHide = e.Value;
            };
        }
    }
}
