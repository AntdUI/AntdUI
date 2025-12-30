// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI


namespace AntdUI
{
    /// <summary>
    /// 列汇总
    /// </summary>
    public class SummaryItemOption
    {
        public SummaryItemOption() { }
        public SummaryItemOption(TSummaryType summaryType) { SummaryType = summaryType; }
        public SummaryItemOption(TSummaryType summaryType, string displayFormat) : this(summaryType)
        {
            DisplayFormat = displayFormat;
        }

        /// <summary>
        /// 汇总类型
        /// </summary>
        public TSummaryType SummaryType { get; set; } = TSummaryType.None;

        /// <summary>
        /// 格式化 (0.#####, {0:P2}...)
        /// </summary>
        /// <remarks>当SummaryType=Text时，此属性为设置要显示的内容</remarks>
        public string? DisplayFormat { get; set; }

        /// <summary>
        /// SummaryType为Text时，显示的文本
        /// </summary>
        public string? DisplayText { get; set; }

        /// <summary>
        /// 统计模式是否为只统计选中的行 (false时为当前视图的所有行)
        /// </summary>
        public bool SelectionMode { get; set; }
    }
}