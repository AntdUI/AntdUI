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

namespace AntdUI
{
    public class PagePageEventArgs : EventArgs
    {
        public PagePageEventArgs(int current, int total, int pageSize, int pageTotal)
        {
            Current = current;
            Total = total;
            PageSize = pageSize;
            PageTotal = pageTotal;
        }
        /// <summary>
        /// 当前页数
        /// </summary>
        public int Current { get; private set; }
        /// <summary>
        /// 数据总数
        /// </summary>
        public int Total { get; private set; }
        /// <summary>
        /// 每页条数
        /// </summary>
        public int PageSize { get; private set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int PageTotal { get; private set; }
    }

    /// <summary>
    /// 显示数据总量
    /// </summary>
    public delegate void PageValueEventHandler(object sender, PagePageEventArgs e);

    /// <summary>
    /// 显示数据总量
    /// </summary>
    public delegate string PageValueRtEventHandler(object sender, PagePageEventArgs e);
}