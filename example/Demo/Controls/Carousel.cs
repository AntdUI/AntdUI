// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class Carousel : UserControl
    {
        AntdUI.BaseForm form;
        public Carousel(AntdUI.BaseForm _form)
        {
            form = _form;
            InitializeComponent();
            if (carousel1.Image != null) slider1.MaxValue = carousel1.Image.Count - 1;
            slider1.Value = carousel1.SelectIndex;
            carousel1.SelectIndexChanged += (a, b) =>
            {
                slider1.Value = carousel1.SelectIndex;
            };
            slider1.ValueChanged += (s, e) =>
            {
                carousel1.SelectIndex = e.Value;
            };
        }

        Random random = new Random();
        private void image3d1_Click(object sender, EventArgs e)
        {
            var num = random.Next(1, 9);
            switch (num)
            {
                case 1:
                    image3d1.Image = Properties.Resources.bg1; break;
                case 2:
                    image3d1.Image = Properties.Resources.bg2; break;
                case 3:
                    image3d1.Image = Properties.Resources.bg3; break;
                case 4:
                    image3d1.Image = Properties.Resources.bg4; break;
                case 5:
                    image3d1.Image = Properties.Resources.bg5; break;
                case 6:
                    image3d1.Image = Properties.Resources.bg6; break;
                default:
                    image3d1.Image = Properties.Resources.bg7;
                    break;
            }
        }

        private void image3d2_Click(object sender, EventArgs e)
        {
            var num = random.Next(1, 9);
            switch (num)
            {
                case 1:
                    image3d2.Image = Properties.Resources.bg1; break;
                case 2:
                    image3d2.Image = Properties.Resources.bg2; break;
                case 3:
                    image3d2.Image = Properties.Resources.bg3; break;
                case 4:
                    image3d2.Image = Properties.Resources.bg4; break;
                case 5:
                    image3d2.Image = Properties.Resources.bg5; break;
                case 6:
                    image3d2.Image = Properties.Resources.bg6; break;
                default:
                    image3d2.Image = Properties.Resources.bg7;
                    break;
            }
        }
    }
}