// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using AntdUI;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class Button9 : UserControl
    {
        AntdUI.BaseForm form;
        public Button9(AntdUI.BaseForm _form)
        {
            form = _form;
            InitializeComponent();
        }

        private void b1_Click(object sender, EventArgs e)
        {
            if (sender is AntdUI.Button btn)
            {
                if (btn.AnimationBlinkState)
                {
                    btn.IconSvg = null;
                    btn.StopAnimationBlink();
                }
                else
                {
                    btn.IconSvg = "ClockCircleFilled";
                    btn.AnimationBlinkTransition(200, 10, AnimationType.Ease,
                        Color.FromArgb(255, 0, 0),
                        Color.FromArgb(0, 255, 0),
                        Color.FromArgb(0, 0, 255));
                }
            }
        }

        private void b2_Click(object sender, EventArgs e)
        {
            if (sender is AntdUI.Button btn)
            {
                if (btn.AnimationBlinkState)
                {
                    btn.IconSvg = null;
                    btn.StopAnimationBlink();
                }
                else
                {
                    btn.IconSvg = "ClockCircleFilled";
                    btn.AnimationBlinkTransition(500, 10, AnimationType.Ball, "#fca5a5".ToColor(), "#ff9a9e".ToColor(), "#ff0844".ToColor(), "#ffb199".ToColor());
                }
            }
        }

        private void b4_Click(object sender, EventArgs e)
        {
            if (sender is AntdUI.Button btn)
            {
                if (btn.AnimationBlinkState)
                {
                    btn.IconSvg = null;
                    btn.StopAnimationBlink();
                }
                else
                {
                    btn.IconSvg = "ClockCircleFilled";
                    btn.AnimationBlink(200, "#fa709a".ToColor(), "#fee140".ToColor());
                }
            }
        }

        private void b5_Click(object sender, EventArgs e)
        {
            if (sender is AntdUI.Button btn)
            {
                if (btn.AnimationBlinkState)
                {
                    btn.IconSvg = null;
                    btn.StopAnimationBlink();
                }
                else
                {
                    btn.IconSvg = "ClockCircleFilled";
                    btn.AnimationBlinkTransition(1000, 10, AnimationType.Liner,
                        "#ff0000".ToColor(), "#ff7f00".ToColor(), "#ffff00".ToColor(),
                        "#00ff00".ToColor(), "#0000ff".ToColor(), "#4b0082".ToColor(), "#9400d3".ToColor());
                }
            }
        }
    }
}