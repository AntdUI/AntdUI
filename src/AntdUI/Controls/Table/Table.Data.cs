// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

namespace AntdUI
{
    partial class Table
    {
        internal TempTable? dataTmp = null;
        bool dataOne = true;
        List<int> selects = new List<int>();
        int hovers = -1;
        void ExtractData()
        {
            dataOne = true;
            dataTmp = null;
            if (columns != null)
            {
                // 重置复选状态
                foreach (var item in columns)
                {
                    if (item is ColumnCheck check) check.CheckState = CheckState.Unchecked;
                }
            }
            if (dataSource == null)
            {
                rows_Expand.Clear();
                // 空数据
                ScrollBar.ValueX = ScrollBar.ValueY = 0;
                return;
            }
            if (dataSource is BindingSource bindingSource)
            {
                bindingSource.ListChanged -= Binding_ListChanged;
                bindingSource.ListChanged += Binding_ListChanged;
                ExtractData(bindingSource.DataSource);
            }
            else ExtractData(dataSource);
        }

        bool ExtractDataSummary()
        {
            if (dataTmp == null || dataTmp.rowsFilter == null) return true;
            if (columns != null)
            {
                // 重置复选状态
                foreach (var item in columns)
                {
                    if (item is ColumnCheck check) check.CheckState = CheckState.Unchecked;
                }
            }
            if (summary == null)
            {
                dataTmp.summary = null;
                return false;
            }
            if (dataSource is DataTable table)
            {
                if (table.Columns.Count > 0 && table.Rows.Count > 0)
                {
                    var columns = new List<TempiColumn>(table.Columns.Count);
                    for (int i = 0; i < table.Columns.Count; i++) columns.Add(new TempiColumn(i, table.Columns[i]));
                    var _columns = columns.ToArray();
                    dataTmp.summary = ExtractSummary(_columns);
                }
            }
            else if (dataSource is IList list)
            {
                var columns = new TempiColumn[0];
                var rows = new List<IRow>(list.Count + 1);
                for (int i = 0; i < list.Count; i++) GetRowAuto(ref rows, list[i], i, ref columns);
                dataTmp.summary = ExtractSummary(columns);
            }
            return false;
        }

        bool ExtractData(object? dataSource)
        {
            if (dataSource is DataTable table)
            {
                if (table.Columns.Count > 0 && table.Rows.Count > 0)
                {
                    var columns = new List<TempiColumn>(table.Columns.Count);
                    var rows = new List<IRow>(table.Rows.Count + 1);
                    for (int i = 0; i < table.Columns.Count; i++) columns.Add(new TempiColumn(i, table.Columns[i]));
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        var row = table.Rows[i];
                        if (row != null)
                        {
                            var cells = new Dictionary<string, object?>(columns.Count);
                            foreach (var it in columns) cells.Add(it.key, row[it.key]);
                            if (cells.Count > 0) rows.Add(new IRow(i, row, cells));
                        }
                    }
                    var _columns = columns.ToArray();
                    dataTmp = new TempTable(_columns, rows.ToArray(), ExtractSummary(_columns));
                    return true;
                }
            }
            else if (dataSource is IList list)
            {
                if (list.Count > 0)
                {
                    var columns = new TempiColumn[0];
                    var rows = new List<IRow>(list.Count + 1);
                    for (int i = 0; i < list.Count; i++) GetRowAuto(ref rows, list[i], i, ref columns);
                    dataTmp = new TempTable(columns, rows.ToArray(), ExtractSummary(columns));
                    return true;
                }
            }
            return false;
        }

        IRow[]? ExtractSummary(TempiColumn[] columns)
        {
            if (summary == null) return null;
            else if (summary is DataTable table)
            {
                if (table.Columns.Count > 0 && table.Rows.Count > 0)
                {
                    var rows = new List<IRow>(table.Rows.Count);
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        var row = table.Rows[i];
                        if (row != null)
                        {
                            var cells = new Dictionary<string, object?>(columns.Length);
                            foreach (var it in columns) cells.Add(it.key, row[it.key]);
                            if (cells.Count > 0) rows.Add(new IRow(i, row, cells));
                        }
                    }
                    return rows.ToArray();
                }
            }
            else if (summary is IList list)
            {
                var rows = new List<IRow>(list.Count);
                for (int i = 0; i < list.Count; i++) GetRowAuto(ref rows, list[i], i, ref columns);
                return rows.ToArray();
            }
            else
            {
                var rows = new List<IRow>(1);
                GetRowAuto(ref rows, summary, 0, ref columns);
                if (rows.Count > 0) return rows.ToArray();
            }
            return null;
        }

        #region AntList

        public void Binding<T>(AntList<T> list)
        {
            dataOne = true;
            if (list == null) return;
            dataSource = list;
            list.action = (code, obj) =>
            {
                if (dataTmp != null)
                {
                    try
                    {
                        if (code == "add")
                        {
                            if (obj is int i)
                            {
                                var row = list[i];
                                if (row != null)
                                {
                                    var cells = GetRow(row, dataTmp.columns.Length);
                                    if (cells.Count == 0) return;
                                    int len = dataTmp.rows.Length + 1;
                                    if (len < i) return;
                                    var rows = new List<IRow>(len);
                                    rows.AddRange(dataTmp.rows);
                                    rows.Insert(i, new IRow(i, row, cells));
                                    dataTmp.rows = ChangeList(rows);
                                    if (LoadLayout()) Invalidate();
                                }
                            }
                            else if (obj is int[] i_s)
                            {
                                var _list = new List<IRow>(i_s.Length);
                                foreach (int id in i_s)
                                {
                                    var row = list[id];
                                    if (row != null)
                                    {
                                        var cells = GetRow(row, dataTmp.columns.Length);
                                        if (cells.Count > 0) _list.Add(new IRow(id, row, cells));
                                    }
                                }
                                var rows = new List<IRow>(dataTmp.rows.Length + _list.Count);
                                rows.AddRange(dataTmp.rows);
                                rows.InsertRange(i_s[0], _list);
                                dataTmp.rows = ChangeList(rows);
                                if (LoadLayout()) Invalidate();
                            }
                        }
                        else if (code == "edit")
                        {
                            if (LoadLayout()) Invalidate();
                        }
                        else if (code == "del")
                        {
                            if (obj is int i) DelData(i);
                            else if (obj is string)
                            {
                                dataTmp.rows = new IRow[0];
                                SortData = null; // 清空排序数据
                                if (LoadLayout()) Invalidate();
                            }
                        }
                    }
                    catch
                    {
                        // 刷新数据
                        IBinding(list);
                    }
                }
            };
            IBinding(list);
        }

        void IBinding<T>(AntList<T> list)
        {
            ExtractHeaderFixed();
            var columns = new TempiColumn[0];
            var rows = new List<IRow>(list.Count + 1);
            for (int i = 0; i < list.Count; i++) GetRowAuto(ref rows, list[i], i, ref columns);
            dataTmp = new TempTable(columns, rows.ToArray(), ExtractSummary(columns));
            LoadLayout();
            Invalidate();
        }

        #endregion

        #region BindingList

        public void Binding<T>(BindingList<T> list)
        {
            dataOne = true;
            if (list == null) return;
            list.ListChanged -= Binding_ListChanged;
            list.ListChanged += Binding_ListChanged;
            DataSource = list;
        }

        void Binding_ListChanged(object? sender, ListChangedEventArgs e)
        {
            if (sender == dataSource)
            {
                switch (e.ListChangedType)
                {
                    case ListChangedType.ItemAdded:
                        BindingItemAdded(sender, e.NewIndex);
                        break;
                    case ListChangedType.ItemDeleted:
                        BindingItemDeleted(sender, e.NewIndex);
                        break;
                    case ListChangedType.ItemChanged:
                        BindingItemChanged(sender, e.NewIndex, e.NewIndex == e.OldIndex);
                        break;
                    case ListChangedType.Reset:
                        if (dataTmp == null) return;
                        dataTmp.rows = new IRow[0];
                        SortData = null; // 清空排序数据
                        if (LoadLayout()) Invalidate();
                        break;
                    case ListChangedType.ItemMoved:
                    default:
                        if (sender is IList list) DataSource = list;
                        break;
                }
            }
        }

        void BindingItemAdded(object? sender, int i)
        {
            if (dataTmp == null)
            {
                if (ExtractData(dataSource) && LoadLayout()) Invalidate();
                return;
            }
            if (sender is IList list)
            {
                var row = list[i];
                if (row == null) return;
                if (dataTmp.columns.Length == 0) ExtractData();
                else
                {
                    var cells = GetRow(row, dataTmp.columns.Length);
                    if (cells.Count == 0) return;
                    int len = dataTmp.rows.Length + 1;
                    if (len < i) return;
                    var rows = new List<IRow>(len);
                    rows.AddRange(dataTmp.rows);
                    rows.Insert(i, new IRow(i, row, cells));
                    dataTmp.rows = ChangeList(rows);
                }
                if (LoadLayout()) Invalidate();
            }
        }
        void BindingItemChanged(object? sender, int i, bool eq)
        {
            if (dataTmp == null) return;
            if (sender is IList list)
            {
                var row = list[i];
                if (row == null) return;
                if (eq)
                {
                    var tmp = GetRowIndex(row, dataTmp.rows);
                    if (tmp.HasValue) i = tmp.Value;
                }
                var cells = GetRow(row, dataTmp.columns.Length);
                if (cells.Count == 0) return;
                int len = dataTmp.rows.Length;
                var rows = new List<IRow>(len);
                rows.AddRange(dataTmp.rows);
                rows[i] = new IRow(i, row, cells);
                dataTmp.rows = ChangeList(rows);
                if (LoadLayout()) Invalidate();
            }
        }
        void BindingItemDeleted(object? sender, int i) => DelData(i);
        public int[]? SortDelList(int i)
        {
            if (dataTmp == null) return null;
            if (i >= 0 && i < dataTmp.rows.Length) DelSortData(dataTmp.rows, i);
            return SortData;
        }
        public int[]? SortDelList(object record)
        {
            if (dataTmp == null) return null;
            var i = GetRowIndexReal(record, dataTmp.rows);
            if (i.HasValue) DelSortData(dataTmp.rows, i.Value);
            return SortData;
        }
        void DelData(int i)
        {
            if (dataTmp == null) return;
            if (i >= 0 && i < dataTmp.rows.Length)
            {
                var rows = new List<IRow>(dataTmp.rows.Length);
                rows.AddRange(dataTmp.rows);
                rows.RemoveAt(i);
                DelSortData(dataTmp.rows, i);
                dataTmp.rows = ChangeList(rows);
                if (LoadLayout()) Invalidate();
            }
        }
        void DelSortData(IRow[] rows, int i)
        {
            if (SortData == null || i < 0 || i >= rows.Length) return;
            var newSortData = new List<int>(rows.Length - 1);
            foreach (int originalIndex in SortData)
            {
                if (originalIndex == i) continue;
                int adjustedIndex = originalIndex > i ? originalIndex - 1 : originalIndex;
                if (adjustedIndex >= 0 && adjustedIndex < rows.Length - 1) newSortData.Add(adjustedIndex);
            }
            SortData = newSortData.Count > 0 ? newSortData.ToArray() : null;
        }

        #endregion

        IRow[] ChangeList(List<IRow> rows)
        {
            for (int i = 0; i < rows.Count; i++) rows[i].i = i;
            return rows.ToArray();
        }

        void GetRowAuto(ref List<IRow> rows, object? row, int i, ref TempiColumn[] columns)
        {
            if (row == null) return;
            if (row is IList<AntItem> arows)
            {
                var irow = new IRow(i, row, arows);
                rows.Add(irow);
                if (columns.Length == 0)
                {
                    columns = new TempiColumn[arows.Count];
                    for (int j = 0; j < arows.Count; j++) columns[j] = new TempiColumn(j, arows[j].key);
                }
            }
            else
            {
                var cells = GetRowAuto(row, ref columns);
                if (cells.Count > 0) rows.Add(new IRow(i, row, cells));
            }
        }

        Dictionary<string, object?> GetRowAuto(object row, ref TempiColumn[] columns)
        {
            if (columns.Length == 0) return GetRow(row, out columns);
            else return GetRow(row, columns.Length);
        }

        Dictionary<string, object?> GetRow(object row, int len)
        {
            if (row is IList<AntItem> arows)
            {
                var cells = new Dictionary<string, object?>(arows.Count);
                foreach (AntItem it in arows) cells.Add(it.key, it);
                return cells;
            }
            else if (row is System.Dynamic.ExpandoObject expando)
            {
                // 支持动态对象（ExpandoObject）
                var cells = new Dictionary<string, object?>(len);
                var dict = (IDictionary<string, object?>)expando;
                foreach (var kvp in dict) cells.Add(kvp.Key, kvp.Value);
                return cells;
            }
            else if (row is IDictionary<string, object?> dict)
            {
                // 支持字典对象
                var cells = new Dictionary<string, object?>(len);
                foreach (var kvp in dict) cells.Add(kvp.Key, kvp.Value);
                return cells;
            }
            else
            {
                var cells = new Dictionary<string, object?>(len);
                foreach (PropertyDescriptor it in TypeDescriptor.GetProperties(row))
                {
                    cells.Add(it.Name, it);
                    GetRow(row, it, ref cells);
                }
                return cells;
            }
        }
        object? GetRow(object row, string key)
        {
            if (row is IList<AntItem> arows)
            {
                foreach (var it in arows)
                {
                    if (it.key == key) return it.value;
                }
            }
            else if (row is System.Dynamic.ExpandoObject expando)
            {
                // 支持动态对象（ExpandoObject）
                var dict = (IDictionary<string, object?>)expando;
                foreach (var kvp in dict)
                {
                    if (kvp.Key == key) return kvp.Value;
                }
            }
            else if (row is IDictionary<string, object?> dict)
            {
                // 支持字典对象
                foreach (var kvp in dict)
                {
                    if (kvp.Key == key) return kvp.Value;
                }
            }
            else
            {
                foreach (PropertyDescriptor it in TypeDescriptor.GetProperties(row))
                {
                    if (it.Name == key) return it.GetValue(row);
                }
            }
            return null;
        }

        Dictionary<string, object?> GetRow(object row, out TempiColumn[] _columns)
        {
            if (row is System.Dynamic.ExpandoObject expando)
            {
                // 支持动态对象（ExpandoObject）
                var dict = (IDictionary<string, object?>)expando;
                var cells = new Dictionary<string, object?>(dict.Count);
                var columns = new List<TempiColumn>(dict.Count);
                int index = 0;
                foreach (var kvp in dict)
                {
                    columns.Add(new TempiColumn(index, kvp.Key)); index++;
                    cells.Add(kvp.Key, kvp.Value);
                }
                _columns = columns.ToArray();
                return cells;
            }
            else if (row is IDictionary<string, object?> dict)
            {
                // 支持字典对象
                var cells = new Dictionary<string, object?>(dict.Count);
                var columns = new List<TempiColumn>(dict.Count);
                int index = 0;
                foreach (var kvp in dict)
                {
                    columns.Add(new TempiColumn(index, kvp.Key)); index++;
                    cells.Add(kvp.Key, kvp.Value);
                }
                _columns = columns.ToArray();
                return cells;
            }
            else
            {
                var list = TypeDescriptor.GetProperties(row);
                var cells = new Dictionary<string, object?>(list.Count);
                int index = 0;
                var columns = new List<TempiColumn>(list.Count);
                foreach (PropertyDescriptor it in list)
                {
                    columns.Add(new TempiColumn(index, it.DisplayName ?? it.Name)); index++;
                    cells.Add(it.Name, it);
                    GetRow(row, it, ref cells);
                }
                _columns = columns.ToArray();
                return cells;
            }
        }
        void GetRow(object row, PropertyDescriptor it, ref Dictionary<string, object?> cells)
        {
            if (it.PropertyType.IsClass)
            {
                if (it.PropertyType.IsArray || it.PropertyType.FullName == null) return;
                if (it.PropertyType.FullName.StartsWith("System.") || it.PropertyType.FullName.StartsWith("AntdUI.")) return;
                var value = it.GetValue(row);
                if (value == null) return;
                var list = TypeDescriptor.GetProperties(value);
                foreach (PropertyDescriptor item in list) cells.Add(it.Name + "." + item.Name, new TableSubValue(value, item));
            }
        }
        internal class TableSubValue
        {
            public TableSubValue(object value, PropertyDescriptor p)
            {
                record = value;
                prop = p;
            }

            public object record { get; set; }
            public PropertyDescriptor prop { get; set; }
        }

        bool showFixedColumnL = false, showFixedColumnR = false;
        int sFixedR = 0;
        List<int>? fixedColumnL = null, fixedColumnR = null;
        internal void ExtractHeaderFixed()
        {
            if (columns == null)
            {
                fixedColumnL = fixedColumnR = null;
                return;
            }
            var dir = new List<int>();
            int index = 0;
            ForColumnI(columns, column =>
            {
                if (column.Fixed) dir.Add(index);
                index++;
            });
            if (dir.Count > 0)
            {
                List<int> _fixedColumnL = new List<int>(), _fixedColumnR = new List<int>();
                foreach (int i in dir)
                {
                    if (i == 0) _fixedColumnL.Add(i);
                    else if (_fixedColumnL.Count > 0 && _fixedColumnL[_fixedColumnL.Count - 1] + 1 == i) _fixedColumnL.Add(i);
                }
                foreach (int it in _fixedColumnL) dir.Remove(it);
                if (dir.Count > 0)
                {
                    dir.Reverse();
                    foreach (int i in dir)
                    {
                        if (i == index - 1) _fixedColumnR.Add(i);
                        else if (_fixedColumnR.Count > 0 && _fixedColumnR[_fixedColumnR.Count - 1] - 1 == i) _fixedColumnR.Add(i);
                    }
                }
                if (_fixedColumnL.Count > 0) fixedColumnL = _fixedColumnL; else fixedColumnL = null;
                if (_fixedColumnR.Count > 0) fixedColumnR = _fixedColumnR; else fixedColumnR = null;
            }
            else fixedColumnL = fixedColumnR = null;
        }

        internal class TempTable
        {
            public TempTable(TempiColumn[] _columns, IRow[] _rows, IRow[]? _summary)
            {
                columns = _columns;
                _rowsCache = _rows;
                summary = _summary;
            }

            /// <summary>
            /// 表头 - 列
            /// </summary>
            public TempiColumn[] columns { get; set; }

            IRow[] _rowsCache;
            /// <summary>
            /// 数据 - 行
            /// </summary>
            public IRow[] rows
            {
                get => rowsFilter ?? _rowsCache;
                set
                {
                    lock (_rowsCache)
                    {
                        _rowsCache = value;
                    }
                }
            }

            /// <summary>
            /// 所有行缓存
            /// </summary>
            public IRow[] RowsCache => _rowsCache;

            /// <summary>
            /// 数据 - 行已筛选
            /// </summary>
            public IRow[]? rowsFilter { get; set; }

            public void ClearFilter() => rowsFilter = null;

            /// <summary>
            /// 数据 - 行
            /// </summary>
            public IRow[]? summary { get; set; }

            public void ForRowI(Action<int> action)
            {
                lock (_rowsCache)
                {
                    var tmp = rowsFilter ?? _rowsCache;
                    for (int i = 0; i < tmp.Length; i++) action(i);
                }
            }
            public void ForRow(Action<IRow> action)
            {
                lock (_rowsCache)
                {
                    var tmp = rowsFilter ?? _rowsCache;
                    for (int i = 0; i < tmp.Length; i++) action(tmp[i]);
                }
            }

            public int[] GetInts(int count = 0)
            {
                lock (_rowsCache)
                {
                    var tmp = rowsFilter ?? _rowsCache;
                    var list = new int[tmp.Length];
                    for (int i = 0; i < tmp.Length; i++) list[i] = (i + count);
                    return list;
                }
            }

            public object[] GetRecord()
            {
                lock (_rowsCache)
                {
                    var tmp = rowsFilter ?? _rowsCache;
                    var list = new object[tmp.Length];
                    for (int i = 0; i < tmp.Length; i++) list[i] = tmp[i].record;
                    return list;
                }
            }
            public object[] GetRecord(int[] sortData)
            {
                lock (_rowsCache)
                {
                    var tmp = rowsFilter ?? _rowsCache;
                    var list = new List<object>(tmp.Length);
                    foreach (var i in sortData)
                    {
                        try
                        {
                            list.Add(tmp[i].record);
                        }
                        catch { }
                    }
                    return list.ToArray();
                }
            }
        }

        internal class TempiColumn
        {
            public TempiColumn(int index, DataColumn dataColumn)
            {
                i = index;
                key = dataColumn.ColumnName;
                if (!string.IsNullOrEmpty(dataColumn.Caption)) text = dataColumn.Caption;
            }
            public TempiColumn(int index, string name)
            {
                i = index;
                key = name;
            }
            /// <summary>
            /// 表头名称
            /// </summary>
            public string key { get; set; }

            public string? text { get; set; }

            /// <summary>
            /// 列序号
            /// </summary>
            public int i { get; set; }
        }

        public class IRow
        {
            public IRow(int index, object _record, Dictionary<string, object?> _cells)
            {
                i = index;
                record = _record;
                cells = _cells;
            }

            public IRow(int index, object _record, IList<AntItem> _cells)
            {
                i = index;
                record = _record;
                cells = new Dictionary<string, object?>(_cells.Count);
                foreach (var it in _cells) cells.Add(it.key, it);
            }

            /// <summary>
            /// 行序号
            /// </summary>
            public int i { get; set; }

            /// <summary>
            /// 行原始数据
            /// </summary>
            public object record { get; set; }

            public Dictionary<string, object?> cells { get; set; }

            public object? this[int index]
            {
                get
                {
                    if (cells == null) return null;
                    int i = 0;
                    foreach (var item in cells)
                    {
                        if (i == index)
                        {
                            if (item.Value is PropertyDescriptor prop) return prop.GetValue(record);
                            else if (item.Value is AntItem it) return it.value;
                            else return item.Value;
                        }
                        i++;
                    }
                    return null;
                }
            }
            public object? this[string key]
            {
                get
                {
                    if (cells == null) return null;
                    if (cells.TryGetValue(key, out var value))
                    {
                        if (value is PropertyDescriptor prop) return prop.GetValue(record);
                        else if (value is AntItem it) return it.value;
                        else return value;
                    }
                    return null;
                }
            }

            public void SetValue(int index, object? value)
            {
                if (cells == null) return;

                int i = 0;
                foreach (var item in cells)
                {
                    if (i == index)
                    {
                        cells[item.Key] = value;
                        return;
                    }
                    i++;
                }
            }
            public void SetValue(string key, object? value)
            {
                if (cells == null) return;
                if (cells.TryGetValue(key, out var tmp))
                {
                    if (tmp is PropertyDescriptor prop) prop.SetValue(record, value);
                    else if (tmp is AntItem it) it.value = value;
                    else cells[key] = value;
                }
            }
        }
    }
}