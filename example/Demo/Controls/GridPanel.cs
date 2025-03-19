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
// GITEE: https://gitee.com/antdui/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

using System;
using System.Drawing;
using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class GridPanel : UserControl
    {
        Form form;
        public GridPanel(Form _form)
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
