// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

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