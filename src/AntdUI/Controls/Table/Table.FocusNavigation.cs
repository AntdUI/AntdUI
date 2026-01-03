// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections.Generic;
using System.ComponentModel;

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
        [Description("是否启用焦点自动跳转"), Category("行为"), DefaultValue(false)]
        public bool EnableFocusNavigation { get; set; } = false;

        /// <summary>
        /// 焦点跳转时自动选中行（行背景色跟随）
        /// </summary>
        [Description("焦点跳转时自动选中行"), Category("行为"), DefaultValue(true)]
        public bool FocusNavigationAutoSelectRow { get; set; } = true;

        /// <summary>
        /// 焦点跳转时自动滚动到新行
        /// </summary>
        [Description("焦点跳转时自动滚动到新行"), Category("行为"), DefaultValue(true)]
        public bool FocusNavigationAutoScroll { get; set; } = true;

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

            EnableFocusNavigation = _focusNavigationMap.Count > 0 || fieldSequence.Length > 0;
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
                if (columns == null) return;

                // 找到目标列
                var targetColumn = columns.Find(c => c.Key == columnKey);

                if (targetColumn != null && targetColumn.Editable)
                {
                    BeginInvoke(() =>
                    {
                        // 更新选中行索引，使行背景色跟随焦点切换
                        if (FocusNavigationAutoSelectRow && (selectedIndex.Length == 0 || selectedIndex[0] != rowIndex)) SelectedIndex = rowIndex;

                        // 滚动到新行，确保行在可见范围内
                        if (FocusNavigationAutoScroll) ScrollLine(rowIndex);

                        // 获取列索引
                        var columnIndex = columns.IndexOf(targetColumn);
                        if (columnIndex >= 0)
                        {
                            SetFocusedCell(null);
                            EnterEditMode(rowIndex, columnIndex);

                            // 如果启用文本全选，延迟设置文本全选
                            if (_selectAll == true)
                            {
                                BeginInvoke(() =>
                                {
                                    foreach (var it in _editControls)
                                    {
                                        it.Key.SelectAll();
                                        it.Key.Focus();
                                        return;
                                    }
                                });
                            }
                        }
                    });
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
                        // 优先使用 rows.Length（可见行数），这样可以正确处理树形展开/折叠的情况
                        int totalRows = 0;
                        if (rows != null && rows.Length > 0) totalRows = rows.Length - rowSummary;  // 使用可见行数（考虑树形展开/折叠）
                        else if (dataSource is BindingList<object> bindingList) totalRows = bindingList.Count;
                        else if (dataSource is System.Collections.IList list) totalRows = list.Count;
                        else if (dataSource is System.Data.DataTable dataTable) totalRows = dataTable.Rows.Count;

                        if (totalRows > 0)
                        {
                            int nextRowIndex = rowIndex + 1;
                            if (nextRowIndex < totalRows) BeginInvoke(() => MoveToNextEditableCell(nextRowIndex, _firstFieldKey!));
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