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

using System.Windows.Forms;

namespace AntdUI
{
    public class ClosingPageEventArgs : VEventArgs<TabPage>
    {
        public ClosingPageEventArgs(TabPage value) : base(value) { }
    }

    public delegate bool ClosingPageEventHandler(object sender, ClosingPageEventArgs e);

    public class TabsItemEventArgs : VMEventArgs<TabPage>
    {
        public TabsItemEventArgs(TabPage item, int index, Tabs.IStyle style, MouseEventArgs e) : base(item, e)
        {
            Index = index;
            Style = style;
        }

        public int Index { get; private set; }

        public Tabs.IStyle Style { get; private set; }

        /// <summary>
        /// 是否取消
        /// </summary>
        public bool Cancel { get; set; }

        #region 设置

        public TabsItemEventArgs SetCancel(bool value = true)
        {
            Cancel = value;
            return this;
        }

        #endregion
    }

    /// <summary>
    /// 点击事件
    /// </summary>
    public delegate void TabsItemEventHandler(object sender, TabsItemEventArgs e);
}