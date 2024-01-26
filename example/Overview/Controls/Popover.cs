// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
// THE AntdUI PROJECT IS AN WINFORM LIBRARY LICENSED UNDER THE GPL-3.0 License.
// LICENSED UNDER THE GPL License, VERSION 3.0 (THE "License")
// YOU MAY NOT USE THIS FILE EXCEPT IN COMPLIANCE WITH THE License.
// UNLESS REQUIRED BY APPLICABLE LAW OR AGREED TO IN WRITING, SOFTWARE
// DISTRIBUTED UNDER THE LICENSE IS DISTRIBUTED ON AN "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED.
// SEE THE LICENSE FOR THE SPECIFIC LANGUAGE GOVERNING PERMISSIONS AND
// LIMITATIONS UNDER THE License.
// GITEE: https://gitee.com/antdui/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

namespace Overview.Controls
{
    public partial class Popover : UserControl
    {
        public Popover()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AntdUI.Popover.open(button1, "Basic Popover", "Content" + Environment.NewLine + "Content");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AntdUI.Popover.open(button2, new Button() { Size = new Size(400, 300) });
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