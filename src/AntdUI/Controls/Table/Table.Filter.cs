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
using System.Data;
using System.Linq;

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
                List<Column> columns = new List<Column>();
                foreach (var col in Columns)
                {
                    if (col.Filter != null && col.Filter.Enabled) columns.Add(col);
                }
                if (columns.Count > 0)
                {
                    Column[] result = new Column[columns.Count];
                    columns.CopyTo(result, 0);
                    return result;
                }
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
                if (rows == null || rows.Length == 0)
                {
                    return Columns.ToArray();
                }
                List<Column> columns = new List<Column>();
                CELL[] cells = rows[0].cells;
                foreach (CELL cELL in cells)
                {
                    if (cELL.COLUMN.Visible)
                    {
                        columns.Add(cELL.COLUMN);
                    }
                }

                return columns.ToArray();
            }
        }

        /// <summary>
        /// 获取指定的列
        /// </summary>
        /// <param name="key">字段ID</param>
        /// <param name="ignoreCase">是否忽略大小写</param>
        public Column? GetColumnByFieldKey(string key, bool ignoreCase = false)
        {
            foreach (var col in Columns)
            {
                if (col.Key.Equals(key, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal)) return col;
            }
            return null;
        }

        /// <summary>
        /// 获取筛选数据 (无筛选数据时返回当前视图数据)
        /// </summary>
        /// <returns>筛选后的数据对象数组（注意：返回的是原始数据对象的引用）</returns>
        public object[]? FilterList()
        {
            if (dataTmp == null) return null;
            if (dataTmp.rowsFilter == null || dataTmp.rowsFilter.Length == 0)
            {
                var list = new object[dataTmp.rows.Length];
                for (int i = 0; i < dataTmp.rows.Length; i++) list[i] = dataTmp.rows[i].record;
                return list;
            }
            List<object> data = new List<object>(dataTmp.rowsFilter.Length);
            foreach (var row in dataTmp.rowsFilter) data.Add(row.record);
            return data.ToArray();
        }

        /// <summary>
        /// 更新筛选视图
        /// </summary>
        public void UpdateFilter()
        {
            if (dataTmp == null) return;
            Column[]? filterColumns = FilterColumns;
            if (filterColumns == null || filterColumns.Length == 0) dataTmp.rowsFilter = null;
            else
            {
                filterColumns = filterColumns.OrderBy(c => c.Filter?.FilterIndex).ToArray();//按筛选优化级排序
                List<IRow> rows = new List<IRow>();
                foreach (var col in filterColumns)
                {
                    if (col.Visible == false || col.Filter == null || col.Filter.Enabled == false) continue;
                    bool firstFilter = col == filterColumns[0];
                    List<IRow> found = UpdateFilter(col.Filter, rows, firstFilter);//AND, 结果中筛选，可能需要支持 OR
                    if (firstFilter)
                    {
                        if (found.Count > 0)
                        {
                            foreach (var item in found)
                            {
                                if (rows.Contains(item) == false) rows.Add(item);
                            }
                        }
                    }
                    else rows = found;
                }
                if (rows.Count == 0)
                {
                    dataTmp?.ClearFilter();
                    // 直接清空筛选条件，不触发UpdateFilter()调用
                    foreach (var col in filterColumns)
                    {
                        if (col.Filter != null)
                        {
                            col.Filter.FilterValues?.Clear();
                            col.Filter.FilterValues = null;
                            col.Filter.FilterIndex = -1;
                        }
                    }
                }
                else
                {
                    dataTmp.rowsFilter = new IRow[rows.Count];
                    rows.CopyTo(dataTmp.rowsFilter, 0);
                }
            }
            FilterDataChanged?.Invoke(this, new TableFilterDataChangedEventArgs(FilterList()));
            if (LoadLayout()) Invalidate();

        }
        /// <summary>
        /// 更新筛选视图
        /// </summary>
        internal List<IRow> UpdateFilter(FilterOption option, List<IRow>? foundSource, bool first)
        {
            List<IRow> list = new List<IRow>();
            IRow[]? source;
            if (foundSource != null && foundSource.Count > 0)
            {
                source = new IRow[foundSource.Count];
                foundSource.CopyTo(source, 0);
            }
            else source = first && option.Enabled || option.ActiveSource == FilterSource.DataSource ? dataTmp?.RowsCache : dataTmp?.rows;

            if (source == null || source.Length == 0 || option.Key == null || option.FilterValues == null) return list;
            foreach (var value in option.FilterValues)
            {
                foreach (var row in source)
                {
                    object? val = row[option.Key];
                    try
                    {
                        switch (option.Condition)
                        {
                            case FilterConditions.Equal:
                                if (val == null || value == null)
                                {
                                    if (val == value) list.Add(row);
                                }
                                else
                                {
                                    if (val.GetType() == typeof(DateTime))
                                    {
                                        if (((DateTime)val).Date == ((DateTime)value).Date) list.Add(row);
                                    }
                                    else
                                    {
                                        // 尝试进行数值类型比较
                                        if (TryCompareAsNumeric(val, value, out bool isEqual) && isEqual) list.Add(row);
                                        else if (val.Equals(value)) list.Add(row);
                                    }
                                }
                                break;
                            case FilterConditions.NotEqual:
                                if (val == null || value == null)
                                {
                                    if (val != value) list.Add(row);
                                }
                                else
                                {
                                    if (val.GetType() == typeof(DateTime))
                                    {
                                        if (((DateTime)val).Date != ((DateTime)value).Date) list.Add(row);
                                    }
                                    else
                                    {
                                        // 尝试进行数值类型比较
                                        if (TryCompareAsNumeric(val, value, out bool isEqual) && !isEqual) list.Add(row);
                                        else if (!val.Equals(value)) list.Add(row);
                                    }
                                }
                                break;
                            case FilterConditions.Greater:
                            case FilterConditions.Less:
                                if (val == null || val == DBNull.Value) continue;
                                if (value == null || value == DBNull.Value) continue;
                                if (val.GetType() == typeof(string))
                                {
                                    if (option.Condition == FilterConditions.Greater)
                                    {
                                        if (val.ToString().StartsWith(value.ToString())) list.Add(row);
                                    }
                                    else
                                    {
                                        if (val.ToString().EndsWith(value.ToString())) list.Add(row);
                                    }
                                }
                                else if (val.GetType() == typeof(DateTime))
                                {
                                    if (option.Condition == FilterConditions.Greater)
                                    {
                                        if ((DateTime)val > (DateTime)value) list.Add(row);
                                    }
                                    else
                                    {
                                        if ((DateTime)val < (DateTime)value) list.Add(row);
                                    }
                                }
                                else
                                {
                                    decimal num = Convert.ToDecimal(val);
                                    decimal num2 = Convert.ToDecimal(value);
                                    if (option.Condition == FilterConditions.Greater)
                                    { if (num > num2) list.Add(row); }
                                    else
                                    { if (num < num2) list.Add(row); }
                                }
                                break;
                            case FilterConditions.None:
                                break;
                            default:
                                bool emptyVal = val == null || val == DBNull.Value;
                                bool emptyVal2 = value == null || value == DBNull.Value;
                                if (emptyVal && emptyVal2) list.Add(row);
                                else
                                {
                                    if (option.Condition == FilterConditions.Contain)
                                    {
                                        if (emptyVal == false && emptyVal2 == false && val.ToString().Contains(value.ToString())) list.Add(row);
                                    }
                                    else
                                    {
                                        if (emptyVal == false && emptyVal2 == false && val.ToString().Contains(value.ToString()) == false) list.Add(row);
                                    }
                                }
                                break;
                        }
                    }
                    catch { }
                }
            }
            return list;
        }

        /// <summary>
        /// 尝试将两个对象作为数值进行比较
        /// </summary>
        /// <param name="val1">第一个值</param>
        /// <param name="val2">第二个值</param>
        /// <param name="isEqual">比较结果</param>
        /// <returns>是否成功进行了数值比较</returns>
        private static bool TryCompareAsNumeric(object val1, object val2, out bool isEqual)
        {
            isEqual = false;

            try
            {
                // 如果任一值已经是数值类型，直接比较
                if (IsNumericType(val1) && IsNumericType(val2))
                {
                    decimal d1 = Convert.ToDecimal(val1);
                    decimal d2 = Convert.ToDecimal(val2);
                    isEqual = d1 == d2;
                    return true;
                }

                // 尝试解析字符串为数值
                if (val1 is string str1 && decimal.TryParse(str1, out decimal parsed1))
                {
                    if (val2 is string str2 && decimal.TryParse(str2, out decimal parsed2))
                    {
                        isEqual = parsed1 == parsed2;
                        return true;
                    }
                    else if (IsNumericType(val2))
                    {
                        decimal d2 = Convert.ToDecimal(val2);
                        isEqual = parsed1 == d2;
                        return true;
                    }
                }
                else if (val2 is string str2 && decimal.TryParse(str2, out decimal parsed2))
                {
                    if (IsNumericType(val1))
                    {
                        decimal d1 = Convert.ToDecimal(val1);
                        isEqual = d1 == parsed2;
                        return true;
                    }
                }
            }
            catch
            {
                // 解析失败，返回false
                return false;
            }

            return false;
        }

        /// <summary>
        /// 判断对象是否为数值类型
        /// </summary>
        /// <param name="obj">要判断的对象</param>
        /// <returns>是否为数值类型</returns>
        private static bool IsNumericType(object obj)
        {
            return obj is byte || obj is sbyte ||
                   obj is short || obj is ushort ||
                   obj is int || obj is uint ||
                   obj is long || obj is ulong ||
                   obj is float || obj is double ||
                   obj is decimal;
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
        /// 用于标识筛选优化级，-1表示未启用筛选
        /// </summary>
        public int FilterIndex { get; internal set; } = -1;
        /// <summary>
        /// 列KEY
        /// </summary>
        public string? Key { get => Column?.Key; }
        /// <summary>
        /// 是否已启用筛选
        /// </summary>
        public bool Enabled { get => FilterValues != null && FilterValues.Count > 0; }
        /// <summary>
        /// 列数据类型 (默认string)
        /// </summary>
        public Type DataType { get; set; } = typeof(string);
        /// <summary>
        /// 允许空值
        /// </summary>
        public bool AllowNull { get; set; } = true;
        /// <summary>
        /// 筛选来源
        /// </summary>
        public FilterSource ActiveSource { get; set; } = FilterSource.CurrentFirst;
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
            internal set
            {
                _filterValues = value;

                Column[]? filterColumns = Table?.FilterColumns;
                FilterIndex = filterColumns != null ? filterColumns.Length : -1;
            }
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
            FilterIndex = -1;
            if (update) UpdateFilter();
        }

        internal void UpdateFilter() => Table?.UpdateFilter();

        /// <summary>
        /// 外部应用筛选 (当前列)
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="filterValues">单个筛选值</param>
        public bool Apply(FilterConditions condition, object filterValue)
        {
            if (Column == null) return false;
            object[] filterValues = new object[] { filterValue };
            return Apply(Column, condition, filterValues);
        }

        /// <summary>
        /// 外部应用筛选
        /// </summary>
        /// <param name="column">筛选列</param>
        /// <param name="condition">条件</param>
        /// <param name="filterValues">单个筛选值</param>
        public bool Apply(Column column, FilterConditions condition, object filterValue)
        {
            object[] filterValues = new object[] { filterValue };
            return Apply(column, condition, filterValues);
        }
        /// <summary>
        /// 外部应用筛选 (当前列)
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="filterValues">筛选值</param>
        /// <returns></returns>
        public bool Apply(FilterConditions condition, object[] filterValues)
        {
            if (Column == null) return false;
            return Apply(Column, condition, filterValues);
        }
        /// <summary>
        /// 外部应用筛选
        /// </summary>
        /// <param name="column">筛选列</param>
        /// <param name="condition">条件</param>
        /// <param name="filterValues">筛选值</param>
        /// <returns></returns>
        public bool Apply(Column column, FilterConditions condition, object[] filterValues)
        {
            if (column == null || condition == FilterConditions.None) return false;

            Column = column;
            Table = column.PARENT;

            Condition = condition;
            FilterValues = new List<object?>(filterValues);
            UpdateFilter();
            return Enabled;
        }
    }
}