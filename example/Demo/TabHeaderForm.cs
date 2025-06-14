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

using AntdUI;
using System;
using System.Drawing;

namespace Demo
{
    public partial class TabHeaderForm : Window
    {
        public TabHeaderForm()
        {
            InitializeComponent();
            var homeImage = SvgExtend.GetImgExtend("HomeOutlined", new Rectangle(0, 0, 16, 16));
            tabHeader1.AddTab("首页", homeImage);
            tabHeader1.AddTab("关于");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var timeImage = SvgExtend.GetImgExtend("FieldTimeOutlined", new Rectangle(0, 0, 16, 16));
            tabHeader1.AddTab(DateTime.Now.ToString(), timeImage);
        }
    }
}
