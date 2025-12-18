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