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
// GITEE: https://gitee.com/AntdUI/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

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
                btn_next.Text = "Íê³É";
            }
            btn_previous.Visible = step > 1;
            if (btn_previous.Visible)
            {
                int w1 = (int)(label1.PSize.Width / AntdUI.Config.Dpi), w = (int)((label2.PSize.Width + btn_previous.PSize.Width + btn_next.PSize.Width) / AntdUI.Config.Dpi);
                Width = w1 > w ? w1 : w;
            }
        }

        private void btn_previous_Click(object sender, EventArgs e) => popover.Tour.Previous();

        private void btn_next_Click(object sender, EventArgs e) => popover.Tour.Next();
    }
}