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
                foreach (var col in this.Columns)
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
        /// 更新筛选视图
        /// </summary>
        public void UpdateFilter()
        {
            Column[]? filterColumns = FilterColumns;
            if (filterColumns == null || filterColumns.Length == 0)
            {
                dataTmp.rowsFilter = null;
                if (LoadLayout()) Invalidate();
                return;
            }
            filterColumns = filterColumns.OrderBy(c => c.Filter.FilterIndex).ToArray();//按筛选优化级排序

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
                else
                    rows = found;
            }
            if (rows.Count == 0)
            {
                dataTmp?.ClearFilter();
            }
            else
            {
                dataTmp.rowsFilter = new IRow[rows.Count];
                rows.CopyTo(dataTmp.rowsFilter, 0);

            }
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
                                if (val.Equals(value)) list.Add(row);
                                break;
                            case FilterConditions.NotEqual:
                                if (val.Equals(value) == false) list.Add(row);
                                break;
                            case FilterConditions.Greater:
                            case FilterConditions.Less:
                                if (val == null || val == DBNull.Value) continue;
                                if (value == null || value == DBNull.Value) continue;
                                if (val.GetType() == typeof(string))
                                {
                                    if (option.Condition == FilterConditions.Greater)
                                    { if (val.ToString().StartsWith(value.ToString())) list.Add(row); }
                                    else
                                    {
                                        if (val.ToString().EndsWith(value.ToString())) list.Add(row);
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
                                if (emptyVal && emptyVal2)
                                    list.Add(row);
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
        List<object>? _filterValues;
        /// <summary>
        /// 已应用的筛选值列表
        /// </summary>
        internal List<object>? FilterValues
        {
            get => _filterValues;
            set
            {
                _filterValues = value;

                Column[]? filterColumns = Table?.FilterColumns;
                FilterIndex = filterColumns != null ? filterColumns.Length : -1;
            }
        }

        /// <summary>
        /// 清除筛选内容
        /// </summary>
        public void ClearFilter()
        {
            if (FilterValues != null)
            {
                FilterValues.Clear();
                FilterValues = null;
            }
            FilterIndex = -1;
            this.UpdateFilter();
        }

        internal void UpdateFilter() => Table?.UpdateFilter();
    }
}