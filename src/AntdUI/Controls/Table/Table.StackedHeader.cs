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

using System.ComponentModel;
using System.Drawing;

namespace AntdUI
{
    partial class Table
    {
        /// <summary>
        /// 堆叠表头行
        /// </summary>
        [Browsable(false), Description("堆叠表头行"), Category("数据"), DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public StackedHeaderRow[]? StackedHeaderRows { get; set; }
    }

    /// <summary>
    /// 堆叠表头行
    /// </summary>
    public class StackedHeaderRow
    {
        public StackedHeaderRow(string childColumns, string headerText)
        {
            StackedColumns = new StackedColumn[] { new StackedColumn(childColumns, headerText) };
        }

        public StackedHeaderRow(StackedColumn stackedColumn)
        {
            StackedColumns = new StackedColumn[] { stackedColumn };
        }

        public StackedHeaderRow(params StackedColumn[] stackedColumns)
        {
            StackedColumns = stackedColumns;
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 堆叠标题行的堆叠列
        /// </summary>
        public StackedColumn[] StackedColumns { get; set; }

        /// <summary>
        /// 设置名称
        /// </summary>
        public StackedHeaderRow SetName(string? value)
        {
            Name = value;
            return this;
        }
    }

    /// <summary>
    /// 堆叠列
    /// </summary>
    public class StackedColumn
    {
        public StackedColumn(string childColumns, string headerText)
        {
            ChildColumns = childColumns.Split(',');
            HeaderText = headerText;
        }

        public StackedColumn(string[] childColumns, string headerText)
        {
            ChildColumns = childColumns;
            HeaderText = headerText;
        }

        /// <summary>
        /// 需要堆叠在指定堆叠列下的子列的名称。
        /// </summary>
        public string[] ChildColumns { get; set; }

        /// <summary>
        /// 堆叠列的标题文本
        /// </summary>
        public string HeaderText { get; set; }

        /// <summary>
        /// 文本颜色
        /// </summary>
        public Color? ForeColor { get; set; }

        /// <summary>
        /// 设置文本颜色
        /// </summary>
        public StackedColumn SetForeColor(Color? value)
        {
            ForeColor = value;
            return this;
        }
    }
}