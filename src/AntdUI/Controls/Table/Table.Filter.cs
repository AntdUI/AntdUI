// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections.Generic;

namespace AntdUI
{
    partial class Table
    {
        /// <summary>
        /// 已启用筛选的列
        /// </summary>
        public Column[]? FilterColumns
        {
            get
            {
                if (columns == null) return null;
                var tmp = new List<Column>(columns.Count);
                foreach (var col in columns)
                {
                    if (col.Filter == null) continue;
                    if (col.Filter.Enabled) tmp.Add(col);
                }
                if (tmp.Count > 0) return tmp.ToArray();
                return null;
            }
        }

        /// <summary>
        /// 按当前顺序返回可见的列集合
        /// </summary>
        public Column[]? VisibleColumns
        {
            get
            {
                if (columns == null) return null;
                var tmp = new List<Column>(columns.Count);
                foreach (var it in columns)
                {
                    if (it.Visible) tmp.Add(it);
                }
                return tmp.ToArray();
            }
        }

        /// <summary>
        /// 获取指定的列
        /// </summary>
        /// <param name="key">字段ID</param>
        /// <param name="ignoreCase">是否忽略大小写</param>
        public Column? GetColumnByFieldKey(string key, bool ignoreCase = false)
        {
            if (columns == null) return null;
            foreach (var col in columns)
            {
                if (col.Key.Equals(key, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal)) return col;
            }
            return null;
        }

        /// <summary>
        /// 获取筛选数据 (无筛选数据时返回当前视图数据)
        /// </summary>
        /// <returns>筛选后的数据对象数组（注意：返回的是原始数据对象的引用）</returns>
        public object[] FilterList() => FilterList(IFilterList());
        internal object[] FilterList(IRow[]? row)
        {
            if (row == null)
            {
                if (dataTmp == null) return new object[0];
                var list = new List<object>(dataTmp.RowsCache.Length);
                foreach (var it in dataTmp.RowsCache) list.Add(it.record);
                return list.ToArray();
            }
            var data = new List<object>(row.Length);
            foreach (var it in row) data.Add(it.record);
            return data.ToArray();
        }

        public IRow[]? IFilterList()
        {
            if (dataTmp == null) return null;
            return IFilterList(dataTmp);
        }
        internal IRow[]? IFilterList(TempTable dataTmp)
        {
            if (columns == null) return null;
            var dir = new Dictionary<string, List<object?>>(columns.Count);
            foreach (var col in columns)
            {
                if (col.Visible && col.Filter != null && col.Filter.Enabled) dir.Add(col.Key, col.Filter.FilterValues!);
            }
            if (dir.Count > 0)
            {
                // 筛选符合条件的行
                var filteredRows = new List<IRow>(dataTmp.RowsCache.Length);
                foreach (var row in dataTmp.RowsCache)
                {
                    if (MatchFilter(row, dir)) filteredRows.Add(row);
                }
                return filteredRows.ToArray();
            }
            return null;
        }
        internal IRow[] IFilterList(string columns_skip)
        {
            if (dataTmp == null || columns == null) return new IRow[0];
            var dir = new Dictionary<string, List<object?>>(columns.Count);
            foreach (var col in columns)
            {
                if (col.Visible && col.Filter != null && col.Filter.Enabled)
                {
                    if (columns_skip == col.Key) continue;
                    dir.Add(col.Key, col.Filter.FilterValues!);
                }
            }
            if (dir.Count > 0)
            {
                // 筛选符合条件的行
                var filteredRows = new List<IRow>(dataTmp.RowsCache.Length);
                foreach (var row in dataTmp.RowsCache)
                {
                    if (MatchFilter(row, dir)) filteredRows.Add(row);
                }
                return filteredRows.ToArray();
            }
            return dataTmp.RowsCache;
        }

        /// <summary>
        /// 更新筛选视图
        /// </summary>
        public void UpdateFilter()
        {
            if (dataTmp == null) return;
            if (columns == null)
            {
                dataTmp.ClearFilter();
                return;
            }
            var tmp = IFilterList(dataTmp);
            dataTmp.rowsFilter = tmp;
            OnFilterDataChanged(FilterList(tmp));
            OnUpdateSummaries();
            if (LoadLayout()) Invalidate();
        }

        /// <summary>
        /// 检查行是否符合列的筛选条件
        /// </summary>
        /// <param name="row">要检查的行</param>
        /// <param name="dir">筛选集合</param>
        /// <returns>是否符合条件</returns>
        bool MatchFilter(IRow row, Dictionary<string, List<object?>> dir)
        {
            // 检查行是否符合所有筛选条件
            foreach (var col in dir)
            {
                if (!MatchFilter(row, col.Key, col.Value)) return false;
            }
            return true;
        }
        /// <summary>
        /// 检查行是否符合列的筛选条件
        /// </summary>
        /// <param name="row">要检查的行</param>
        /// <param name="column">筛选列</param>
        /// <returns>是否符合条件</returns>
        bool MatchFilter(IRow row, string key, List<object?> filterValues)
        {
            // 获取行中对应列的值
            var cellValue = row[key];
            // 检查单元格值是否存在于筛选值列表中
            foreach (var filterValue in filterValues)
            {
                if (filterValue == null && cellValue == null) return true;
                if (filterValue?.Equals(cellValue) ?? false) return true;
            }
            return false;
        }
    }

    /// <summary>
    /// 列筛选
    /// </summary>
    public class FilterOption
    {
        /// <summary>
        /// 实例化筛选
        /// </summary>
        public FilterOption() : this(typeof(string)) { }

        /// <summary>
        /// 实例化筛选
        /// </summary>
        /// <param name="dataType">数据类型</param>
        public FilterOption(Type dataType) : this(dataType, dataType == typeof(string) ? FilterConditions.Contain : FilterConditions.Equal) { }

        /// <summary>
        /// 实例化筛选
        /// </summary>
        /// <param name="dataType">数据类型</param>
        /// <param name="conidtion">筛选条件</param>
        public FilterOption(Type dataType, FilterConditions conidtion)
        {
            DataType = dataType;
            Condition = conidtion;

        }
        internal Table? Table { get; set; }
        internal Column? Column { get; set; }

        /// <summary>
        /// 列KEY
        /// </summary>
        public string? Key => Column?.Key;

        /// <summary>
        /// 是否已启用筛选
        /// </summary>
        public bool Enabled => FilterValues != null;

        /// <summary>
        /// 列数据类型 (默认string)
        /// </summary>
        public Type DataType { get; set; } = typeof(string);

        /// <summary>
        /// 允许空值
        /// </summary>
        public bool AllowNull { get; set; } = true;

        /// <summary>
        /// 筛选条件
        /// </summary>
        public FilterConditions Condition { get; set; } = FilterConditions.Contain;

        List<object?>? _filterValues;
        /// <summary>
        /// 已应用的筛选值列表
        /// </summary>
        public List<object?>? FilterValues
        {
            get => _filterValues;
            internal set => _filterValues = value;
        }

        /// <summary>
        /// 清除筛选内容
        /// </summary>
        public void ClearFilter(bool update = true)
        {
            if (FilterValues != null)
            {
                FilterValues.Clear();
                FilterValues = null;
            }
            if (update) UpdateFilter();
        }

        internal void UpdateFilter() => Table?.UpdateFilter();

        /// <summary>
        /// 外部应用筛选
        /// </summary>
        /// <param name="filterValue">单个筛选值（列数据）</param>
        public bool Apply(object filterValue) => Apply(new object[] { filterValue });

        /// <summary>
        /// 外部应用筛选
        /// </summary>
        /// <param name="filterValues">筛选值（列数据）</param>
        public bool Apply(params object[] filterValues)
        {
            FilterValues = new List<object?>(filterValues);
            UpdateFilter();
            return Enabled;
        }

        /// <summary>
        /// 外部应用筛选
        /// </summary>
        /// <param name="filterValues">筛选值（列数据）</param>
        public bool Apply(IList<object> filterValues)
        {
            FilterValues = new List<object?>(filterValues);
            UpdateFilter();
            return Enabled;
        }
    }
}