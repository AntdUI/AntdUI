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
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace AntdUI
{
    partial class Table
    {
        /// <summary>
        /// 焦点跳转配置字典 - 定义字段间的跳转顺序
        /// </summary>
        private Dictionary<string, string> _focusNavigationMap = new Dictionary<string, string>();
        private string? _firstFieldKey;
        private bool? _selectAll;
        private bool? _lineBreak;

        /// <summary>
        /// 是否启用焦点自动跳转
        /// </summary>
        [Description("文是否启用焦点自动跳转字颜色"), Category("行为"), DefaultValue(false)]
        public bool EnableFocusNavigation { get; set; } = false;

        /// <summary>
        /// 配置焦点跳转顺序  
        /// </summary>
        /// <param name="fieldSequence">字段顺序数组</param>
        /// <param name="selectAll">是否选中文本</param>
        /// <param name="lineBreak">是否换行，不换行本行则回到第一个设置的字段</param>
        public void ConfigureFocusNavigation(string[] fieldSequence, bool selectAll = true, bool lineBreak = true)
        {
            _focusNavigationMap.Clear();

            for (int i = 0; i < fieldSequence.Length - 1; i++)
            {
                _focusNavigationMap[fieldSequence[i]] = fieldSequence[i + 1];
            }

            // 记录第一个字段，用于跨行跳转
            if (fieldSequence.Length > 0) _firstFieldKey = fieldSequence[0];

            EnableFocusNavigation = _focusNavigationMap.Count > 0;
            _selectAll = selectAll;
            _lineBreak = lineBreak;
        }

        /// <summary>
        /// 移动到下一个可编辑单元格
        /// </summary>
        /// <param name="rowIndex">行索引</param>
        /// <param name="columnKey">列键名</param>
        private void MoveToNextEditableCell(int rowIndex, string columnKey)
        {
            try
            {
                if (!EnableFocusNavigation) return;

                // 找到目标列
                var targetColumn = Columns.FirstOrDefault(c => c.Key == columnKey);

                if (targetColumn != null && targetColumn.Editable)
                {
                    BeginInvoke(new Action(() =>
                    {
                        // 获取列索引
                        var columnIndex = Columns.IndexOf(targetColumn);

                        if (columnIndex >= 0)
                        {
                            EnterEditMode(rowIndex, columnIndex);

                            // 如果启用文本全选，延迟设置文本全选
                            if (_selectAll == true)
                            {
                                BeginInvoke(new Action(() =>
                                {
                                    // 查找当前编辑的输入控件并设置全选
                                    var editControls = Controls.OfType<Input>().Where(c => c.Visible && c.Focused).ToList();
                                    foreach (var input in editControls)
                                    {
                                        input.SelectAll();
                                        input.Focus();
                                        break; // 只处理第一个找到的输入控件
                                    }
                                }));
                            }
                        }
                    }));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[MoveToNextEditableCell] 焦点跳转失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 焦点跳转
        /// </summary>
        /// <param name="columnKey">当前编辑的列键名</param>
        /// <param name="rowIndex">行索引</param>
        public void FocusNavigation(string columnKey, int rowIndex)
        {
            // 检查是否启用焦点跳转、列键是否有效 
            if (!EnableFocusNavigation || string.IsNullOrEmpty(columnKey)) return;

            // 查找下一个字段
            if (_focusNavigationMap.TryGetValue(columnKey, out var nextColumnKey)) BeginInvoke(() => MoveToNextEditableCell(rowIndex, nextColumnKey));
            else
            {
                if (!string.IsNullOrEmpty(_firstFieldKey))
                {
                    // 当前字段是最后一个字段
                    if (_lineBreak == true)
                    {
                        // 允许换行：尝试跳转到下一行的第一个字段
                        // 检查是否有下一行
                        // 支持多种数据源类型
                        int totalRows = 0;
                        if (dataSource is BindingList<object> bindingList) totalRows = bindingList.Count;
                        else if (dataSource is System.Collections.IList list) totalRows = list.Count;
                        else if (dataSource is System.Data.DataTable dataTable) totalRows = dataTable.Rows.Count;

                        if (totalRows > 0)
                        {
                            int nextRowIndex = rowIndex + 1;

                            if (nextRowIndex <= totalRows) BeginInvoke(() => MoveToNextEditableCell(nextRowIndex, _firstFieldKey!));
                        }
                    }
                    else
                    {
                        // 不换行：回到本行的第一个字段
                        BeginInvoke(() => MoveToNextEditableCell(rowIndex, _firstFieldKey!));
                    }
                }
            }
        }

        /// <summary>
        /// 单元格编辑模式下按下回车键事件处理
        /// </summary>
        public void Table_CellEditEnter(TableCellEditEnterEventArgs e)
        {
            if (EnableFocusNavigation)
            {
                if (e.Column?.Key == null) return;
                // 处理焦点跳转
                FocusNavigation(e.Column.Key, e.RowIndex);
            }
        }
    }
}