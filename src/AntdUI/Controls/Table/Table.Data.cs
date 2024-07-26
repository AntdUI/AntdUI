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
// GITEE: https://gitee.com/antdui/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

namespace AntdUI
{
    partial class Table
    {
        TempTable? data_temp = null;
        void ExtractData()
        {
            data_temp = null;
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
                if (EmptyHeader && columns != null && columns.Count > 0) data_temp = new TempTable(new TempiColumn[0], new IRow[0]);
                // 空数据
                scrollBar.ValueX = scrollBar.ValueY = 0;
                return;
            }
            if (dataSource is DataTable table)
            {
                if (table.Columns.Count > 0 && table.Rows.Count > 0)
                {
                    var columns = new List<TempiColumn>(table.Columns.Count);
                    var rows = new List<IRow>(table.Rows.Count + 1);
                    for (int i = 0; i < table.Columns.Count; i++) columns.Add(new TempiColumn(i, table.Columns[i].ColumnName));
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
                    data_temp = new TempTable(columns.ToArray(), rows.ToArray());
                }
            }
            else if (dataSource is IList list)
            {
                var columns = new List<TempiColumn>();
                var rows = new List<IRow>(list.Count + 1);
                for (int i = 0; i < list.Count; i++)
                {
                    var row = list[i];
                    if (row != null)
                    {
                        if (row is IList<AntItem> arows) rows.Add(new IRow(i, row, arows));
                        else
                        {
                            var cells = GetRowAuto(row, ref columns);
                            if (cells.Count > 0) rows.Add(new IRow(i, row, cells));
                        }
                    }
                }
                data_temp = new TempTable(columns.ToArray(), rows.ToArray());
            }
        }

        public void Binding<T>(AntList<T> list)
        {
            if (list == null) return;
            dataSource = list;
            list.action = (code, obj) =>
            {
                if (data_temp != null)
                {
                    if (code == "add")
                    {
                        if (obj is int i)
                        {
                            var row = list[i];
                            if (row != null)
                            {
                                var cells = GetRow(row, data_temp.columns.Length);
                                if (cells.Count == 0) return;
                                var rows = new List<IRow>(data_temp.rows.Length + 1);
                                rows.AddRange(data_temp.rows);
                                rows.Insert(i, new IRow(i, row, cells));
                                data_temp.rows = ChangeList(rows);
                                LoadLayout();
                                Invalidate();
                            }
                        }
                        else if (obj is int[] i_s)
                        {
                            var _list = new List<IRow>();
                            foreach (int id in i_s)
                            {
                                var row = list[id];
                                if (row != null)
                                {
                                    var cells = GetRow(row, data_temp.columns.Length);
                                    if (cells.Count > 0) _list.Add(new IRow(id, row, cells));
                                }
                            }
                            var rows = new List<IRow>(data_temp.rows.Length + _list.Count);
                            rows.AddRange(data_temp.rows);
                            rows.InsertRange(i_s[0], _list);
                            data_temp.rows = ChangeList(rows);
                            LoadLayout();
                            Invalidate();
                        }
                    }
                    else if (code == "edit")
                    {
                        LoadLayout();
                        Invalidate();
                    }
                    else if (code == "del")
                    {
                        if (obj is int i)
                        {
                            if (i >= 0 && i < data_temp.rows.Length)
                            {
                                var rows = new List<IRow>(data_temp.rows.Length);
                                rows.AddRange(data_temp.rows);
                                rows.RemoveAt(i);
                                data_temp.rows = ChangeList(rows);
                                LoadLayout();
                                Invalidate();
                            }
                        }
                        else if (obj is string)
                        {
                            data_temp.rows = new IRow[0];
                            LoadLayout();
                            Invalidate();
                        }
                    }
                }
            };
            var columns = new List<TempiColumn>();
            var rows = new List<IRow>(list.Count + 1);
            for (int i = 0; i < list.Count; i++)
            {
                var row = list[i];
                if (row != null)
                {
                    if (row is IList<AntItem> arows) rows.Add(new IRow(i, row, arows));
                    else
                    {
                        var cells = GetRowAuto(row, ref columns);
                        if (cells.Count > 0) rows.Add(new IRow(i, row, cells));
                    }
                }
            }
            data_temp = new TempTable(columns.ToArray(), rows.ToArray());

            LoadLayout();
            Invalidate();
        }

        IRow[] ChangeList(List<IRow> rows)
        {
            for (int i = 0; i < rows.Count; i++) rows[i].i = i;
            return rows.ToArray();
        }

        Dictionary<string, object?> GetRowAuto(object row, ref List<TempiColumn> columns)
        {
            if (columns.Count == 0) return GetRow(row, ref columns);
            else return GetRow(row, columns.Count);
        }

        Dictionary<string, object?> GetRow(object row, int len)
        {
            if (row is IList<AntItem> arows)
            {
                var cells = new Dictionary<string, object?>(arows.Count);
                foreach (AntItem it in arows) cells.Add(it.key, it);
                return cells;
            }
            else
            {
                var cells = new Dictionary<string, object?>(len);
                foreach (PropertyDescriptor it in TypeDescriptor.GetProperties(row)) cells.Add(it.Name, it);
                return cells;
            }
        }

        Dictionary<string, object?> GetRow(object row, ref List<TempiColumn> columns)
        {
            var list = TypeDescriptor.GetProperties(row);
            var cells = new Dictionary<string, object?>(list.Count);
            int index = 0;
            foreach (PropertyDescriptor it in list)
            {
                columns.Add(new TempiColumn(index, it.Name)); index++;
                cells.Add(it.Name, it);
            }
            return cells;
        }

        bool showFixedColumnL = false, showFixedColumnR = false;
        int sFixedR = 0;
        List<int>? fixedColumnL = null, fixedColumnR = null;
        internal void ExtractHeaderFixed()
        {
            if (columns == null) return;
            var dir = new List<int>();
            int index = 0;
            foreach (var column in columns)
            {
                if (column.Visible)
                {
                    if (column.Fixed) dir.Add(index);
                    index++;
                }
            }
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

        class TempTable
        {
            public TempTable(TempiColumn[] _columns, IRow[] _rows)
            {
                columns = _columns;
                rows = _rows;
            }
            /// <summary>
            /// 表头 - 列
            /// </summary>
            public TempiColumn[] columns { get; set; }
            /// <summary>
            /// 数据 - 行
            /// </summary>
            public IRow[] rows { get; set; }
        }

        class TempiColumn
        {
            public TempiColumn(int index, string name)
            {
                i = index;
                key = name;
            }
            /// <summary>
            /// 表头名称
            /// </summary>
            public string key { get; set; }

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
        }
    }
}