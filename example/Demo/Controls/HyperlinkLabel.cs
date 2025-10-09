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

using System.Drawing;
using System.Windows.Forms;

namespace Demo.Controls
{
    public partial class HyperlinkLabel : UserControl
    {
        Form form;

        public HyperlinkLabel(Form _form)
        {
            form = _form;
            InitializeComponent();

            // 测试样式
            hyperlinkLabel4.NormalStyle = new AntdUI.HyperlinkLabel.LinkAppearance
            {
                LinkColor = AntdUI.Style.Db.Error,
                UnderlineThickness = 1
            };
            hyperlinkLabel4.HoverStyle = new AntdUI.HyperlinkLabel.LinkAppearance
            {
                LinkColor = AntdUI.Style.Db.ErrorActive,
                UnderlineThickness = 2
            };
        }

        private void hyperlinkLabel_LinkClicked(object sender, AntdUI.HyperlinkLabel.LinkClickedEventArgs e)
        {
            if (sender == hyperlinkLabel2) AntdUI.Message.success(FindForm(), "居中链接被点击: " + e.Text);
            else if (sender == hyperlinkLabel3) AntdUI.Message.success(FindForm(), "带徽章的链接被点击: " + e.Text);
            else AntdUI.Message.success(form, "点击了: " + e.Text);
        }
    }
}