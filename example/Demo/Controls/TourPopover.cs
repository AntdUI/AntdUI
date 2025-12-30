// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class TourPopover : UserControl
    {
        AntdUI.Tour.Popover popover;
        public TourPopover(AntdUI.Tour.Popover _popover, string title, string text, int step, int max)
        {
            popover = _popover;
            InitializeComponent();
            label1.Text = title;
            label3.Text = text;
            label2.Text = step + " / " + max;
            if (step == max)
            {
                btn_next.LocalizationText = "Finish";
                btn_next.Text = "完成";
            }
            btn_previous.Visible = step > 1;
            if (btn_previous.Visible)
            {
                int w1 = label1.PSize.Width, w = (label2.PSize.Width + btn_previous.PSize.Width + btn_next.PSize.Width) - (int)(40 * AntdUI.Config.Dpi);
                Width = w1 > w ? w1 : w;
            }
        }

        private void btn_previous_Click(object sender, EventArgs e) => popover.Tour.Previous();

        private void btn_next_Click(object sender, EventArgs e) => popover.Tour.Next();
    }
}