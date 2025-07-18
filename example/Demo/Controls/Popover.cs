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
using System.Drawing;
using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class Popover : UserControl
    {
        Form form;
        public Popover(Form _form)
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