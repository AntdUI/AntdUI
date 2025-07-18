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

using System;
using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class Carousel : UserControl
    {
        Form form;
        public Carousel(Form _form)
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