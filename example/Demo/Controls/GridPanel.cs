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
    public partial class GridPanel : UserControl
    {
        AntdUI.BaseForm form;
        public GridPanel(AntdUI.BaseForm _form)
        {
            form = _form;
            InitializeComponent();
            button1.BackColor = button2.BackColor = button4.BackColor = button8.BackColor = button6.BackColor = button9.BackColor = button11.BackColor = Color.FromArgb(200, AntdUI.Style.Db.Primary);
            gridPanel1.Span = input1.Text.Trim();
        }

        private void gridPanel1_Paint(object sender, PaintEventArgs e)
        {
            int len = 12, w = gridPanel1.Width / (len * 2), w2 = w * 2, h = gridPanel1.Height, x = 0;
            using (var brush = new SolidBrush(AntdUI.Style.Db.FillTertiary))
            {
                for (int i = 0; i < len; i++)
                {
                    e.Graphics.FillRectangle(brush, x, 0, w, h);
                    x += w2;
                }
            }
        }

        private void input1_TextChanged(object sender, EventArgs e) => gridPanel1.Span = input1.Text.Trim();
    }
}
