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

        NavigationConfig? navigationConfig;
        /// <summary>
        /// 配置焦点跳转顺序  
        /// </summary>
        /// <param name="fieldSequence">字段顺序数组</param>
        /// <param name="selectAll">是否选中文本</param>
        /// <param name="lineBreak">是否换行，不换行本行则回到第一个设置的字段</param>
        public void ConfigureFocusNavigation(string[] fieldSequence, bool selectAll = true, bool lineBreak = true)
        {
            if (fieldSequence.Length > 0)
            {
                navigationConfig = new NavigationConfig(fieldSequence, selectAll, lineBreak);
                EnableFocusNavigation = true;
            }
            else
            {
                navigationConfig = null;
                EnableFocusNavigation = false;
            }
        }

        /// <summary>
        /// 焦点跳转
        /// </summary>
        /// <param name="e">当前编辑的列</param>
        public void FocusNavigation(TableCellEditEnterEventArgs e)
        {
            if (navigationConfig == null) return;
            if (navigationConfig.Contains(e.Column.Key, out var nextColumnKey))
            {
                // 查找下一个字段
                if (nextColumnKey == null)
                {
                    // 当前字段是最后一个字段
                    if (navigationConfig.LineBreak == true)
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
                            int nextRowIndex = e.RowIndex + 1;
                            if (nextRowIndex < totalRows) MoveToNextEditableCell(navigationConfig, nextRowIndex, navigationConfig.FirstFieldKey);
                        }
                    }
                    else MoveToNextEditableCell(navigationConfig, e.RowIndex, navigationConfig.FirstFieldKey);// 不换行：回到本行的第一个字段
                }
                else MoveToNextEditableCell(navigationConfig, e.RowIndex, nextColumnKey);
            }
        }

        /// <summary>
        /// 移动到下一个可编辑单元格
        /// </summary>
        /// <param name="rowIndex">行索引</param>
        /// <param name="columnKey">列键名</param>
        private void MoveToNextEditableCell(NavigationConfig navigationConfig, int rowIndex, string columnKey)
        {
            if (rows == null) return;
            foreach (var it in rows[0].cells)
            {
                if (it.COLUMN.Key == columnKey)
                {
                    if (it.COLUMN.Editable)
                    {
                        if (MoveToNextEditableCell(rowIndex, it.COLUMN, it.INDEX) && navigationConfig.SelectAll) BeginInvoke(MoveToNextEditableCellSelectAll);
                    }
                    return;
                }
            }
        }
        private bool MoveToNextEditableCell(int rowIndex, Column column, int columnIndex)
        {
            try
            {
                // 更新选中行索引，使行背景色跟随焦点切换
                if (FocusNavigationAutoSelectRow && (selectedIndex.Length == 0 || selectedIndex[0] != rowIndex)) SelectedIndex = rowIndex;

                // 滚动到新行，确保行在可见范围内
                if (FocusNavigationAutoScroll) ScrollLine(rowIndex);

                // 获取列索引
                if (columnIndex >= 0)
                {
                    SetFocusedCell(null);
                    EnterEditMode(rowIndex, columnIndex);
                    // 如果启用文本全选，延迟设置文本全选
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[MoveToNextEditableCell] 焦点跳转失败: {ex.Message}");
            }
            return false;
        }
        private void MoveToNextEditableCellSelectAll()
        {
            try
            {
                foreach (var it in _editControls)
                {
                    it.Key.SelectAll();
                    it.Key.Focus();
                    return;
                }
            }
            catch
            {
            }
        }

        public class NavigationConfig
        {
            public NavigationConfig(string[] fieldSequence, bool selectAll, bool lineBreak)
            {
                SelectAll = selectAll;
                LineBreak = lineBreak;
                int len = fieldSequence.Length - 1;
                FirstFieldKey = fieldSequence[0];
                LastFieldKey = fieldSequence[len];
                Map = new Dictionary<string, string>(len);
                for (int i = 0; i < len; i++) Map.Add(fieldSequence[i], fieldSequence[i + 1]);
            }

            /// <summary>
            /// 焦点跳转配置字典 - 定义字段间的跳转顺序
            /// </summary>
            public Dictionary<string, string> Map { get; set; }

            public string FirstFieldKey { get; set; }
            public string LastFieldKey { get; set; }

            public bool SelectAll { get; set; }
            public bool LineBreak { get; set; }

            public bool Contains(string key, out string? next)
            {
                if (Map.TryGetValue(key, out next)) return true;
                next = null;
                return FirstFieldKey == key || LastFieldKey == key;
            }
        }
    }
}