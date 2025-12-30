// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Drawing;
using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class Popover : UserControl
    {
        AntdUI.BaseForm form;
        public Popover(AntdUI.BaseForm _form)
        {
            form = _form;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AntdUI.Popover.open(button1, "Basic Popover", "Content" + Environment.NewLine + "Content");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AntdUI.Popover.open(new AntdUI.Popover.Config(button2, new Button(form) { Size = new Size(800, 300) }) { });
        }

        private void buttonTL_Click(object sender, EventArgs e)
        {
            AntdUI.Popover.open(buttonTL, "Title", "Content" + Environment.NewLine + "Content", AntdUI.TAlign.TL);
        }

        private void buttonTop_Click(object sender, EventArgs e)
        {
            AntdUI.Popover.open(buttonTop, "Title", "Content" + Environment.NewLine + "Content", AntdUI.TAlign.Top);
        }

        private void buttonTR_Click(object sender, EventArgs e)
        {
            AntdUI.Popover.open(buttonTR, "Title", "Content" + Environment.NewLine + "Content", AntdUI.TAlign.TR);
        }

        private void buttonRT_Click(object sender, EventArgs e)
        {
            AntdUI.Popover.open(buttonRT, "Title", "Content" + Environment.NewLine + "Content", AntdUI.TAlign.RT);
        }

        private void buttonRight_Click(object sender, EventArgs e)
        {
            AntdUI.Popover.open(buttonRight, "Title", "Content" + Environment.NewLine + "Content", AntdUI.TAlign.Right);
        }

        private void buttonRB_Click(object sender, EventArgs e)
        {
            AntdUI.Popover.open(buttonRB, "Title", "Content" + Environment.NewLine + "Content", AntdUI.TAlign.RB);
        }

        private void buttonBR_Click(object sender, EventArgs e)
        {
            AntdUI.Popover.open(buttonBR, "Title", "Content" + Environment.NewLine + "Content", AntdUI.TAlign.BR);
        }

        private void buttonBottom_Click(object sender, EventArgs e)
        {
            AntdUI.Popover.open(buttonBottom, "Title", "Content" + Environment.NewLine + "Content", AntdUI.TAlign.Bottom);
        }

        private void buttonBL_Click(object sender, EventArgs e)
        {
            AntdUI.Popover.open(buttonBL, "Title", "Content" + Environment.NewLine + "Content", AntdUI.TAlign.BL);
        }

        private void buttonLB_Click(object sender, EventArgs e)
        {
            AntdUI.Popover.open(buttonLB, "Title", "Content" + Environment.NewLine + "Content", AntdUI.TAlign.LB);
        }

        private void buttonLeft_Click(object sender, EventArgs e)
        {
            AntdUI.Popover.open(buttonLeft, "Title", "Content" + Environment.NewLine + "Content", AntdUI.TAlign.Left);
        }

        private void buttonLT_Click(object sender, EventArgs e)
        {
            AntdUI.Popover.open(buttonLT, "Title", "Content" + Environment.NewLine + "Content", AntdUI.TAlign.LT);
        }
    }
}