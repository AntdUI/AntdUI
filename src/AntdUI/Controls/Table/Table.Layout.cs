﻿// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
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
using System.ComponentModel;
using System.Drawing;

namespace AntdUI
{
    partial class Table
    {
        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            if (LoadLayout()) Invalidate();
        }

        string? show_oldrect = null;
        protected override void OnSizeChanged(EventArgs e)
        {
            var rect = ClientRectangle;
            if (IsHandleCreated && rect.Width > 1 && rect.Height > 1)
            {
                string show_rect = rect.Width + "_" + rect.Height;
                if (show_oldrect == show_rect) return;
                show_oldrect = show_rect;
                LoadLayout(rect);
                base.OnSizeChanged(e);
            }
        }

        internal RowTemplate[]? rows = null;
        internal List<object> rows_Expand = new List<object>();
        Rectangle[] dividers = new Rectangle[0], dividerHs = new Rectangle[0];
        MoveHeader[] moveheaders = new MoveHeader[0];

        public bool LoadLayout()
        {
            if (IsHandleCreated)
            {
                if (pauseLayout) return false;
                var rect = ClientRectangle;
                if (rect.Width > 1 && rect.Height > 1) LoadLayout(rect);
                else show_oldrect = null;
                return true;
            }
            return false;
        }

        void LoadLayout(Rectangle rect_t)
        {
            var rect = LayoutDesign(rect_t);
            ScrollBar.SizeChange(rect);
        }

        bool has_check = false;
        Rectangle rect_read, rect_divider;
        Rectangle LayoutDesign(Rectangle rect)
        {
            has_check = false;
            if (dataTmp == null)
            {
                ThreadState?.Dispose();
                ThreadState = null;
                if (visibleHeader && emptyHeader && columns != null && columns.Count > 0)
                {
                    var _rows = LayoutDesign(new TempTable(new TempiColumn[0], new IRow[0], null), out var _columns, out _, out var col_width, out int KeyTreeINDEX);
                    rows = LayoutDesign(rect, _rows, _columns, col_width, KeyTreeINDEX, out int x, out int y, out bool is_exceed);
                    ScrollBar.SetVrSize(is_exceed ? x : 0, y);
                    return rect;
                }
                else
                {
                    ScrollBar.SetVrSize(0, 0);
                    dividers = new Rectangle[0];
                    rows = null;
                }
            }
            else
            {
                var _rows = LayoutDesign(dataTmp, out var _columns, out bool processing, out var col_width, out int KeyTreeINDEX);
                if (visibleHeader && EmptyHeader && _rows.Count == 0)
                {
                    rows = LayoutDesign(rect, _rows, _columns, col_width, KeyTreeINDEX, out int x, out int y, out bool is_exceed);
                    ScrollBar.SetVrSize(is_exceed ? x : 0, y);
                    ThreadState?.Dispose(); ThreadState = null;
                    return rect;
                }
                else if (_rows.Count > 0)
                {
                    rows = LayoutDesign(rect, _rows, _columns, col_width, KeyTreeINDEX, out int x, out int y, out bool is_exceed);
                    ScrollBar.SetVrSize(is_exceed ? x : 0, y);
                    if (processing && Config.HasAnimation(nameof(Table)))
                    {
                        if (ThreadState == null)
                        {
                            ThreadState = new ITask(this, i =>
                            {
                                AnimationStateValue = i;
                                Invalidate();
                            }, 50, 1F, .05F);
                        }
                    }
                    else
                    {
                        ThreadState?.Dispose();
                        ThreadState = null;
                    }
                    return rect;
                }
                else
                {
                    ThreadState?.Dispose(); ThreadState = null;
                    ScrollBar.SetVrSize(0, 0);
                    dividers = new Rectangle[0];
                    rows = null;
                }
            }
            return Rectangle.Empty;
        }

        List<RowTemplate> LayoutDesign(TempTable dataTmp, out List<Column> Columns, out bool Processing, out Dictionary<int, object> ColWidth, out int KeyTreeIndex)
        {
            var _rows = new List<RowTemplate>(dataTmp.rows.Length);
            var _columns = new List<Column>(dataTmp.columns.Length);
            int processing = 0;
            var col_width = new Dictionary<int, object>();
            string? KeyTree = null;
            int KeyTreeINDEX = -1;
            if (columns == null)
            {
                if (SortHeader == null)
                {
                    foreach (var it in dataTmp.columns) _columns.Add(new Column(it.key, it.text ?? it.key) { INDEX = _columns.Count });
                }
                else
                {
                    foreach (var i in SortHeader)
                    {
                        var it = dataTmp.columns[i];
                        _columns.Add(new Column(it.key, it.text ?? it.key) { INDEX = i });
                    }
                }
            }
            else
            {
                int x = 0;
                ForColumn(columns, it =>
                {
                    int INDEX = _columns.Count;
                    _columns.Add(it);
                    ColumnWidth(it, ref col_width, x);
                    x++;
                    if (it.KeyTree != null)
                    {
                        foreach (var item in dataTmp.columns)
                        {
                            if (item.key == it.KeyTree)
                            {
                                KeyTree = it.KeyTree;
                                break;
                            }
                        }
                    }
                    return INDEX;
                });
                if (KeyTree != null)
                {
                    foreach (var it in _columns)
                    {
                        if (it.KeyTree == KeyTree) KeyTreeINDEX = it.INDEX;
                    }
                }
            }

            if (KeyTree == null)
            {
                ForRow(dataTmp, row =>
                {
                    var cells = new List<CELL>(_columns.Count);
                    foreach (var column in _columns) AddRows(ref cells, ref processing, column, row, column.Key);
                    if (cells.Count > 0) AddRows(ref _rows, cells.ToArray(), row.i, row.record);
                });
            }
            else
            {
                ForRow(dataTmp, row =>
                {
                    var cells = new List<CELL>(_columns.Count);
                    foreach (var column in _columns) AddRows(ref cells, ref processing, column, row, column.Key);
                    if (cells.Count > 0) ForTree(ref _rows, ref processing, AddRows(ref _rows, cells.ToArray(), row.i, row.record), row, _columns, KeyTree, KeyTreeINDEX, 0, true);
                });
            }
            if (dataTmp.summary != null)
            {
                foreach (var row in dataTmp.summary)
                {
                    var cells = new List<CELL>(_columns.Count);
                    foreach (var column in _columns) AddRows(ref cells, ref processing, column, row, column.Key);
                    if (cells.Count > 0)
                    {
                        var tmp = AddRows(ref _rows, cells.ToArray(), row.i, row.record);
                        tmp.Type = RowType.Summary;
                    }
                }
            }
            Columns = _columns;
            Processing = processing > 0;
            ColWidth = col_width;
            KeyTreeIndex = KeyTreeINDEX;
            dataOne = false;

            return _rows;
        }

        RowTemplate[] LayoutDesign(Rectangle rect, List<RowTemplate> _rows, List<Column> _columns, Dictionary<int, object> col_width, int KeyTreeINDEX, out int _x, out int _y, out bool _is_exceed)
        {
            if (rows != null)
            {
                List<object?> dir_Select = new List<object?>(rows.Length), dir_Hover = new List<object?>(1);
                foreach (var item in rows)
                {
                    if (item.Select) dir_Select.Add(item.RECORD);
                    if (item.Hover) dir_Hover.Add(item.RECORD);
                }
                if (dir_Select.Count > 0 || dir_Hover.Count > 0)
                {
                    foreach (var item in _rows)
                    {
                        if (dir_Select.Contains(item.RECORD)) item.Select = true;
                        if (dir_Hover.Contains(item.RECORD)) item.Hover = true;
                    }
                }
            }

            #region 添加表头

            var _cols = new List<TCellColumn>(_columns.Count);
            foreach (var it in _columns) _cols.Add(new TCellColumn(this, it));
            AddRows(ref _rows, _cols.ToArray(), dataSource);

            #endregion

            #region 计算坐标

            int x = 0, y = 0;
            bool is_exceed = false;

            rect_read.X = rect.X;
            rect_read.Y = rect.Y;

            Helper.GDI(g =>
            {
                var dpi = Config.Dpi;
                int check_size = (int)(_checksize * dpi), switchsize = (int)(_switchsize * dpi), treesize = (int)(TreeButtonSize * dpi),
                gap = (int)(_gap * dpi), gap2 = gap * 2, gapTree = (int)(_gapTree * dpi), gapTree2 = gapTree * 2, sort_size = (int)(DragHandleSize * dpi), sort_ico_size = (int)(DragHandleIconSize * dpi),
                split = (int)(BorderCellWidth * dpi), split2 = split / 2, sp2 = split * 2,
                split_move = (int)(6F * dpi), split_move2 = split_move / 2;

                check_radius = check_size * .12F * dpi;
                check_border = check_size * .04F * dpi;

                #region 布局高宽

                var read_width_cell = new Dictionary<int, AutoWidth>(_rows[0].cells.Length);
                for (int cel_i = 0; cel_i < _rows[0].cells.Length; cel_i++) read_width_cell.Add(cel_i, new AutoWidth());
                for (int row_i = 0; row_i < _rows.Count; row_i++)
                {
                    var row = _rows[row_i];
                    row.INDEX = row_i;
                    if (row.ShowExpand)
                    {
                        float max_height = 0;
                        if (row.IsColumn)
                        {
                            for (int cel_i = 0; cel_i < row.cells.Length; cel_i++)
                            {
                                var it = row.cells[cel_i];
                                it.INDEX = cel_i;
                                var text_size = it.GetSize(g, columnfont ?? Font, rect.Width, gap, gap2);
                                if (it.COLUMN is ColumnSort) read_width_cell[cel_i].value = -2;
                                else if (it.COLUMN is ColumnCheck check && check.NoTitle) read_width_cell[cel_i].value = -1;
                                else
                                {
                                    int width = text_size.Width;
                                    if (read_width_cell[cel_i].value < width) read_width_cell[cel_i].value = width;
                                    if (read_width_cell[cel_i].minvalue < it.MinWidth) read_width_cell[cel_i].minvalue = it.MinWidth;
                                }
                                if (max_height < text_size.Height) max_height = text_size.Height;
                            }
                            if (rowHeightHeader.HasValue) row.Height = (int)(rowHeightHeader.Value * dpi);
                            else if (rowHeight.HasValue) row.Height = (int)(rowHeight.Value * dpi);
                            else row.Height = (int)Math.Round(max_height) + gap2;
                        }
                        else
                        {
                            for (int cel_i = 0; cel_i < row.cells.Length; cel_i++)
                            {
                                var it = row.cells[cel_i];
                                it.INDEX = cel_i;
                                if (it.COLUMN is ColumnSort || (it is TCellCheck check && check.NoTitle))
                                {
                                    if (max_height < gap2) max_height = gap2;
                                }
                                else
                                {
                                    var text_size = it.GetSize(g, Font, rect.Width, gap, gap2);
                                    int width = text_size.Width;
                                    if (it.ROW.CanExpand && _rows[0].cells[cel_i].INDEX == KeyTreeINDEX) width += (treesize + gapTree2) * (it.ROW.ExpandDepth + 1) - treesize / 2;
                                    if (max_height < text_size.Height) max_height = text_size.Height;
                                    if (read_width_cell[cel_i].value < width) read_width_cell[cel_i].value = width;
                                }
                            }

                            if (rowHeight.HasValue) row.Height = (int)(rowHeight.Value * dpi);
                            else row.Height = (int)Math.Round(max_height) + gap2;
                        }
                    }
                }
                foreach (var it in read_width_cell)
                {
                    var maxWidth = _columns[it.Key].MaxWidth;
                    if (maxWidth != null)
                    {
                        if (maxWidth.EndsWith("%") && float.TryParse(maxWidth.TrimEnd('%'), out var f))
                        {
                            int max = (int)(rect.Width * f / 100F);
                            if (it.Value.value > max)
                            {
                                it.Value.value = max;
                                if (col_width.TryGetValue(it.Key, out _)) col_width[it.Key] = max;
                                else col_width.Add(it.Key, max);
                            }
                        }
                        else if (int.TryParse(maxWidth, out var i))
                        {
                            int max = (int)(i * Config.Dpi);
                            if (it.Value.value > max)
                            {
                                it.Value.value = max;
                                if (col_width.TryGetValue(it.Key, out _)) col_width[it.Key] = max;
                                else col_width.Add(it.Key, max);
                            }
                        }
                    }
                }

                if (bordered)
                {
                    rect_read.Width = rect.Width - sp2;
                    rect_read.Height = rect.Height - sp2;
                }
                else
                {
                    rect_read.Width = rect.Width;
                    rect_read.Height = rect.Height;
                }

                var width_cell = CalculateWidth(rect, col_width, read_width_cell, gap2, check_size, sort_size, ref is_exceed);

                #endregion

                #region 最终坐标

                if (StackedHeaderRows != null) _rows[0].Height += _rows[0].Height * StackedHeaderRows.Length;

                int use_y;
                if (visibleHeader) use_y = rect.Y;
                else use_y = rect.Y - _rows[0].Height;
                int i2 = 0;
                foreach (var cell in _rows[0].cells)
                {
                    cell.COLUMN.WidthPixel = width_cell[i2];
                    i2++;
                }
                foreach (var row in _rows)
                {
                    if (row.ShowExpand)
                    {
                        int use_x = rect.X;
                        row.RECT = new Rectangle(rect.X, use_y, rect_read.Width, row.Height);
                        for (int i = 0; i < row.cells.Length; i++)
                        {
                            var it = row.cells[i];
                            var _rect = new Rectangle(use_x, use_y, width_cell[i], row.RECT.Height);
                            int ox = 0;
                            if (row.INDEX > 0 && _rows[0].cells[i].INDEX == KeyTreeINDEX)
                            {
                                int x = gapTree * row.ExpandDepth;
                                ox = x + (gapTree + treesize);
                                row.RectExpand = new Rectangle(use_x + x + split_move, use_y + (row.Height - treesize) / 2, treesize, treesize);
                            }

                            if (it is TCellCheck check) check.SetSize(_rect, check_size);
                            else if (it is TCellRadio radio) radio.SetSize(_rect, check_size);
                            else if (it is TCellSwitch _switch) _switch.SetSize(_rect, switchsize);
                            else if (it is TCellSort sort) sort.SetSize(_rect, sort_size, sort_ico_size);
                            else if (it is TCellColumn column)
                            {
                                it.SetSize(g, Font, _rect, ox, gap, gap2);
                                if (column.COLUMN is ColumnCheck columnCheck && columnCheck.NoTitle)
                                {
                                    column.COLUMN.SortOrder = false;
                                    columnCheck.PARENT = this;
                                    column.RECT_REAL = new Rectangle(_rect.X + (_rect.Width - check_size) / 2, _rect.Y + (_rect.Height - check_size) / 2, check_size, check_size);
                                }
                                else
                                {
                                    if (column.COLUMN.SortOrder) column.RECT_REAL = new Rectangle(_rect.X + gap, _rect.Y, _rect.Width - gap2 - column.SortWidth, _rect.Height);
                                    else column.RECT_REAL = new Rectangle(_rect.X + gap, _rect.Y, _rect.Width - gap2, _rect.Height);
                                    if (x < column.RECT_REAL.Right) x = column.RECT_REAL.Right;
                                }
                            }
                            else it.SetSize(g, Font, _rect, ox, gap, gap2);

                            if (x < _rect.Right) x = _rect.Right;
                            if (y < _rect.Bottom) y = _rect.Bottom;
                            use_x += width_cell[i];
                        }
                        use_y += row.Height;
                    }
                }

                #endregion

                List<Rectangle> _dividerHs = new List<Rectangle>(), _dividers = new List<Rectangle>();
                var MoveHeaders = new List<MoveHeader>();

                int last_index = _rows.Count - 1;
                var last_row = _rows[last_index];
                while (!last_row.ShowExpand)
                {
                    last_index--;
                    last_row = _rows[last_index];
                }
                var last = last_row.cells[last_row.cells.Length - 1];

                bool isempty = emptyHeader && _rows.Count == 1;
                if ((rect.Y + rect.Height) > last.RECT.Bottom && !isempty) rect_read.Height = last.RECT.Bottom - rect.Y;

                rect_divider = new Rectangle(rect_read.X + split, rect_read.Y + split, rect_read.Width, rect_read.Height);

                var moveheaders_dir = new Dictionary<int, MoveHeader>(moveheaders.Length);
                foreach (var item in moveheaders) moveheaders_dir.Add(item.i, item);
                foreach (var row in _rows)
                {
                    if (row.IsColumn)
                    {
                        if (EnableHeaderResizing)
                        {
                            for (int i = 0; i < row.cells.Length; i++)
                            {
                                var it = row.cells[i];
                                MoveHeaders.Add(new MoveHeader(moveheaders_dir, new Rectangle(it.RECT.Right - split_move2, rect.Y, split_move, it.RECT.Height), i, it.RECT.Width, it.MinWidth));
                            }
                        }
                        if (bordered)
                        {
                            if (isempty)
                            {
                                for (int i = 0; i < row.cells.Length - 1; i++)
                                {
                                    var it = row.cells[i];
                                    _dividerHs.Add(new Rectangle(it.RECT.Right - split2, rect.Y + split, split, it.RECT.Height - split));
                                }
                            }
                            else
                            {
                                for (int i = 0; i < row.cells.Length - 1; i++)
                                {
                                    var it = row.cells[i];
                                    _dividerHs.Add(new Rectangle(it.RECT.Right - split2, rect.Y + split, split, rect_read.Height));
                                }
                            }
                            if (visibleHeader) _dividers.Add(new Rectangle(rect.X, row.RECT.Bottom - split2, rect_read.Width, split));
                        }
                        else
                        {
                            for (int i = 0; i < row.cells.Length - 1; i++)
                            {
                                var it = row.cells[i];
                                _dividerHs.Add(new Rectangle(it.RECT.Right - split2, it.RECT.Y + gap, split, it.RECT.Height - gap2));
                            }
                        }
                    }
                    else
                    {
                        if (bordered) _dividers.Add(new Rectangle(rect.X, row.RECT.Bottom - split2, rect_read.Width, split));
                        else _dividers.Add(new Rectangle(row.RECT.X, row.RECT.Bottom - split2, row.RECT.Width, split));
                    }
                }
                if (bordered && !isempty) _dividers.RemoveAt(_dividers.Count - 1);
                dividerHs = _dividerHs.ToArray();
                dividers = _dividers.ToArray();
                moveheaders = MoveHeaders.ToArray();
            });

            #endregion

            _x = x;
            _y = y;
            _is_exceed = is_exceed;
            return _rows.ToArray();
        }

        #region 通用循环

        void ForColumn(ColumnCollection columns, Func<Column, int> action)
        {
            if (SortHeader == null)
            {
                foreach (var it in columns)
                {
                    it.PARENT = this;
                    if (it.Visible) it.INDEX = action(it);
                }
            }
            else
            {
                var dir = new Dictionary<int, Column>();
                foreach (var it in columns)
                {
                    if (it.Visible) dir.Add(dir.Count, it);
                }
                try
                {
                    foreach (var index in SortHeader)
                    {
                        var it = dir[index];
                        it.PARENT = this;
                        it.INDEX = index;
                        if (it.Visible) action(it);
                    }
                }
                catch { }
            }
        }
        void ForRow(TempTable data_temp, Action<IRow> action)
        {
            if (SortData == null || SortData.Length != data_temp.rows.Length)
            {
                foreach (var row in data_temp.rows) action(row);
            }
            else
            {
                foreach (var i in SortData) action(data_temp.rows[i]);
            }
        }
        bool ForTree(ref List<RowTemplate> _rows, ref int processing, RowTemplate row_new, IRow row, List<Column> _columns, string KeyTree, int KeyTreeINDEX, int depth, bool show)
        {
            if (DefaultExpand && dataOne)
            {
                if (!rows_Expand.Contains(row.record))
                {
                    rows_Expand.Add(row.record);
                    ExpandChanged?.Invoke(this, new TableExpandEventArgs(row.record, true));
                }
            }
            row_new.ShowExpand = show;
            row_new.ExpandDepth = depth;
            row_new.KeyTreeINDEX = KeyTreeINDEX;
            row_new.Expand = rows_Expand.Contains(row.record);
            int count = 0;
            var list_tree = ForTreeValue(row, KeyTree);
            if (list_tree != null)
            {
                show = show && row_new.Expand;
                row_new.CanExpand = true;
                count++;
                for (int i = 0; i < list_tree.Count; i++)
                {
                    var item_tree = GetRow(list_tree[i], _columns.Count);
                    if (item_tree.Count > 0)
                    {
                        var row_tree = new IRow(i, list_tree[i], item_tree);
                        var cells_tree = new List<CELL>(_columns.Count);
                        foreach (var column in _columns) AddRows(ref cells_tree, ref processing, column, row_tree, column.Key);
                        if (ForTree(ref _rows, ref processing, AddRows(ref _rows, cells_tree.ToArray(), row.i, row_tree.record), row_tree, _columns, KeyTree, KeyTreeINDEX, depth + 1, show)) count++;
                    }
                }
            }
            return count > 0;
        }

        static IList<object>? ForTreeValue(IRow row, string KeyTree)
        {
            if (row.cells.TryGetValue(KeyTree, out var ov_tree))
            {
                if (ov_tree is AntItem item)
                {
                    var value_tree = item.value;
                    if (value_tree is IList<AntItem[]> list_tree && list_tree.Count > 0)
                    {
                        var value = new List<object>(list_tree.Count);
                        foreach (var it in list_tree) value.Add(it);
                        return value.ToArray();
                    }
                }
                else if (ov_tree is PropertyDescriptor prop)
                {
                    var value_tree = prop.GetValue(row.record);
                    if (value_tree is IList<object> list_tree && list_tree.Count > 0) return list_tree;
                }
            }
            return null;
        }


        #endregion

        Dictionary<int, int> tmpcol_width = new Dictionary<int, int>(0);
        Dictionary<int, int> CalculateWidth(Rectangle rect, Dictionary<int, object> col_width, Dictionary<int, AutoWidth> read_width, int gap2, int check_size, int sort_size, ref bool is_exceed)
        {
            int use_width = rect.Width;
            float max_width = 0;
            foreach (var it in read_width)
            {
                if (tmpcol_width.TryGetValue(it.Key, out var tw)) max_width += tw;
                else if (col_width.TryGetValue(it.Key, out var value))
                {
                    if (value is int val_int)
                    {
                        if (val_int == -1) max_width += it.Value.value;
                        else if (val_int == -2) max_width += it.Value.minvalue;
                        else max_width += val_int;
                    }
                    if (value is float val_float) max_width += rect.Width * val_float;
                }
                else if (it.Value.value == -1F)
                {
                    int size = check_size * 2;
                    max_width += size;
                    use_width -= size;
                }
                else if (it.Value.value == -2F)
                {
                    int size = sort_size + gap2;
                    max_width += size;
                    use_width -= size;
                }
                else max_width += it.Value.value;
            }

            var width_cell = new Dictionary<int, int>(read_width.Count);
            if (max_width > rect.Width)
            {
                is_exceed = true;
                foreach (var it in read_width)
                {
                    if (tmpcol_width.TryGetValue(it.Key, out var tw)) width_cell.Add(it.Key, tw);
                    else if (col_width.TryGetValue(it.Key, out var value))
                    {
                        if (value is int val_int)
                        {
                            if (val_int == -1) width_cell.Add(it.Key, it.Value.value);
                            else if (val_int == -2) width_cell.Add(it.Key, it.Value.value);
                            else width_cell.Add(it.Key, val_int);
                        }
                        else if (value is float val_float) width_cell.Add(it.Key, (int)Math.Ceiling(rect.Width * val_float));
                    }
                    else if (it.Value.value == -1F) width_cell.Add(it.Key, check_size * 2);
                    else if (it.Value.value == -2F) width_cell.Add(it.Key, sort_size + gap2);
                    else width_cell.Add(it.Key, it.Value.value);
                }
            }
            else
            {
                var fill_count = new List<int>();
                foreach (var it in read_width)
                {
                    if (tmpcol_width.TryGetValue(it.Key, out var tw)) width_cell.Add(it.Key, tw);
                    else if (col_width.TryGetValue(it.Key, out var value))
                    {
                        if (value is int val_int)
                        {
                            if (val_int == -1) width_cell.Add(it.Key, it.Value.value);
                            else if (val_int == -2) fill_count.Add(it.Key);
                            else width_cell.Add(it.Key, val_int);
                        }
                        else if (value is float val_float) width_cell.Add(it.Key, (int)Math.Ceiling(rect.Width * val_float));
                    }
                    else if (it.Value.value == -1F) width_cell.Add(it.Key, check_size * 2);
                    else if (it.Value.value == -2F) width_cell.Add(it.Key, sort_size + gap2);
                    else width_cell.Add(it.Key, (int)Math.Ceiling(use_width * (it.Value.value / max_width)));
                }
                int sum_wi = 0;
                foreach (var it in width_cell) sum_wi += it.Value;
                if (fill_count.Count > 0)
                {
                    int width = (rect.Width - sum_wi) / fill_count.Count;
                    foreach (var it in fill_count) width_cell.Add(it, width);
                    sum_wi = rect.Width;
                }
                if (rect_read.Width > sum_wi)
                {
                    if (AutoSizeColumnsMode == ColumnsMode.Fill)
                    {
                        //填充
                        var percentage = new Dictionary<int, int>(width_cell.Count);
                        foreach (var it in width_cell)
                        {
                            if (tmpcol_width.TryGetValue(it.Key, out _) || col_width.TryGetValue(it.Key, out _)) percentage.Add(it.Key, it.Value);
                            else percentage.Add(it.Key, (int)Math.Round(rect_read.Width * (it.Value * 1.0) / sum_wi));
                        }
                        width_cell = percentage;
                    }
                    else rect_read.Width = sum_wi;
                }
            }
            return width_cell;
        }

        void ColumnWidth(Column it, ref Dictionary<int, object> col_width, int x)
        {
            if (it.Width == null) return;
            if (it.Width.EndsWith("%") && float.TryParse(it.Width.TrimEnd('%'), out var f)) col_width.Add(x, f / 100F);
            else if (int.TryParse(it.Width, out var i)) col_width.Add(x, (int)(i * Config.Dpi));
            else if (it.Width.Contains("fill")) col_width.Add(x, -2);//填充剩下的
            else col_width.Add(x, -1); //AUTO
        }

        #region 动画

        ITask? ThreadState = null;
        internal float AnimationStateValue = 0;

        #endregion

        float check_radius = 0F, check_border = 1F;
        void AddRows(ref List<CELL> cells, ref int processing, Column column, IRow row, string key)
        {
            if (column is ColumnSort columnSort) AddRows(ref cells, new TCellSort(this, columnSort));
            else if (row.cells.TryGetValue(key, out var ov))
            {
                var value = OGetValue(ov, row.record, out var property, out var rv);
                if (column.Render == null) AddRows(ref cells, ref processing, column, rv, value, property);
                else AddRows(ref cells, ref processing, column, rv, column.Render?.Invoke(value, row.record, row.i), property);
            }
            else AddRows(ref cells, ref processing, column, null, column.Render?.Invoke(null, row.record, row.i), null);
        }

        /// <summary>
        /// 添加行
        /// </summary>
        /// <param name="cells">列</param>
        /// <param name="processing">动画</param>
        /// <param name="column">表头</param>
        /// <param name="ov">原始值</param>
        /// <param name="value">真值</param>
        /// <param name="prop">反射</param>
        void AddRows(ref List<CELL> cells, ref int processing, Column column, object? ov, object? value, PropertyDescriptor? prop)
        {
            if (value == null) cells.Add(new TCellText(this, column, prop, ov, null));
            else
            {
                if (column is ColumnCheck columnCheck)
                {
                    //复选框
                    has_check = true;
                    bool value_check = false;
                    if (value is bool check) value_check = check;
                    AddRows(ref cells, new TCellCheck(this, columnCheck, prop, ov, value_check));
                }
                else if (column is ColumnRadio columnRadio)
                {
                    //单选框
                    has_check = true;
                    bool value_check = false;
                    if (value is bool check) value_check = check;
                    AddRows(ref cells, new TCellRadio(this, columnRadio, prop, ov, value_check));
                }
                else if (column is ColumnSwitch columnSwitch)
                {
                    //开关
                    bool value_check = false;
                    if (value is bool check) value_check = check;
                    AddRows(ref cells, new TCellSwitch(this, columnSwitch, prop, ov, value_check));
                }
                else if (value is IList<ICell> icells) AddRows(ref cells, new Template(this, column, prop, ov, ref processing, icells));
                else if (value is ICell icell) AddRows(ref cells, new Template(this, column, prop, ov, ref processing, new ICell[] { icell }));
                else if (column is TemplateColumn tc) AddRows(ref cells, tc.CreateCell(this, tc, prop, ov, ref processing, value));
                else cells.Add(new TCellText(this, column, prop, ov, value.ToString()));
            }
            if (ov is INotifyPropertyChanged notify)
            {
                notify.PropertyChanged -= Notify_PropertyChanged;
                notify.PropertyChanged += Notify_PropertyChanged;
            }
        }

        void AddRows(ref List<CELL> cells, CELL data)
        {
            cells.Add(data);
            if (data is Template template)
            {
                foreach (var it in template.Value)
                {
                    it.Changed = layout =>
                    {
                        if (layout) LoadLayout();
                        Invalidate();
                    };
                }
            }
        }

        RowTemplate AddRows(ref List<RowTemplate> rows, CELL[] cells, int row_i, object? record)
        {
            var row = new RowTemplate(this, cells, row_i, record);
            if (enableDir.Contains(row_i)) row.ENABLE = false;
            foreach (var it in row.cells) it.SetROW(row);
            rows.Add(row);
            return row;
        }
        RowTemplate AddRows(ref List<RowTemplate> rows, TCellColumn[] cells, object? record)
        {
            var row = new RowTemplate(this, cells, -1, record)
            {
                Type = RowType.Column
            };
            for (int i = 0; i < row.cells.Length; i++)
            {
                var it = row.cells[i];
                if (it.COLUMN is ColumnCheck checkColumn && checkColumn.NoTitle)
                {
                    if (rows.Count > 0)
                    {
                        int t_count = rows.Count, check_count = 0;
                        for (int row_i = 0; row_i < rows.Count; row_i++)
                        {
                            if (rows[row_i].Type == RowType.Summary) t_count--;
                            else
                            {
                                var cell = rows[row_i].cells[i];
                                if (cell is TCellCheck checkCell && checkCell.Checked) check_count++;
                            }
                        }
                        if (t_count == check_count) checkColumn.CheckState = System.Windows.Forms.CheckState.Checked;
                        else if (check_count > 0) checkColumn.CheckState = System.Windows.Forms.CheckState.Indeterminate;
                        else checkColumn.CheckState = System.Windows.Forms.CheckState.Unchecked;
                    }
                    else checkColumn.CheckState = System.Windows.Forms.CheckState.Unchecked;
                }
                it.SetROW(row);
            }
            rows.Insert(0, row);
            return row;
        }

        string? OGetValue(TempTable data_temp, int i_r, string key)
        {
            if (data_temp.rows[i_r].cells.TryGetValue(key, out var value))
            {
                if (value is AntItem item)
                {
                    var val = item.value;
                    if (val is IList<ICell> icells)
                    {
                        var vals = new List<string>(icells.Count);
                        foreach (var cell in icells)
                        {
                            var str = cell.ToString();
                            if (!string.IsNullOrEmpty(str)) vals.Add(str);
                        }
                        return string.Join(" ", vals);
                    }
                    else return val?.ToString();
                }
                else if (value is PropertyDescriptor prop)
                {
                    var val = prop.GetValue(data_temp.rows[i_r].record);
                    if (val is IList<ICell> icells)
                    {
                        var vals = new List<string>(icells.Count);
                        foreach (var cell in icells)
                        {
                            var str = cell.ToString();
                            if (!string.IsNullOrEmpty(str)) vals.Add(str);
                        }
                        return string.Join(" ", vals);
                    }
                    else return val?.ToString();
                }
                else return value?.ToString();
            }
            return null;
        }

        object? OGetValue(object? ov, object record, out PropertyDescriptor? property, out object? value)
        {
            value = ov;
            property = null;
            if (ov is AntItem cell) return cell.value;
            else if (ov is PropertyDescriptor prop)
            {
                value = record;
                property = prop;
                return prop.GetValue(record);
            }
            return ov;
        }

        #region MVVM

        private void Notify_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (sender == null || e.PropertyName == null) return;
            PropertyChanged(sender, e.PropertyName);
        }

        void PropertyChanged(object data, string key)
        {
            if (rows == null || key == null) return;
            for (int cel_i = 0; cel_i < rows[0].cells.Length; cel_i++)
            {
                var column = rows[0].cells[cel_i];
                if (key == column.COLUMN.Key)
                {
                    if (data is AntItem item)
                    {
                        foreach (var row in rows)
                        {
                            if (row.RECORD is IList<AntItem> items && items.Contains(item))
                            {
                                RefreshItem(rows, row, row.cells[cel_i], cel_i, item.value);
                                return;
                            }
                        }
                    }
                    else
                    {
                        foreach (var row in rows)
                        {
                            if (row.RECORD == data)
                            {
                                var cel = row.cells[cel_i];
                                if (cel.PROPERTY != null) RefreshItem(rows, row, cel, cel_i, cel.PROPERTY.GetValue(data));
                                return;
                            }
                        }
                    }
                    return;
                }
                else if (key == column.COLUMN.KeyTree)
                {
                    int count = rows.Length;
                    LoadLayout();
                    int countnew = rows.Length;
                    if (selectedIndex.Length > 0 && countnew < count) SetIndex(selectedIndex[0] - count - countnew);
                    Invalidate();
                    return;
                }
            }
        }

        void RefreshItem(RowTemplate[] rows, RowTemplate row, CELL cel, int cel_i, object? value)
        {
            if (cel is Template template)
            {
                int count = 0;
                if (value == null) count++;
                else if (value is IList<ICell> cells)
                {
                    if (template.Value.Count == cells.Count)
                    {
                        for (int i = 0; i < template.Value.Count; i++)
                        {
                            template.Value[i] = cells[i];
                            count++;
                        }
                    }
                    else count++;
                }
                else if (value is ICell cell)
                {
                    if (template.Value.Count == 1)
                    {
                        template.Value[0] = cell;
                        count++;
                    }
                    else count++;
                }
                else count++;
                if (count > 0)
                {
                    LoadLayout();
                    Invalidate();
                }
            }
            else if (cel is TCellText text)
            {
                if (value is IList<ICell> || value is ICell) LoadLayout();
                else text.value = value?.ToString();
                Invalidate();
            }
            else if (cel is TCellCheck check)
            {
                if (value is bool b) check.Checked = b;
                row.Select = RowISelect(row);
                if (cel.COLUMN is ColumnCheck checkColumn && checkColumn.NoTitle)
                {
                    int t_count = rows.Length - 1, check_count = 0;
                    for (int row_i = 1; row_i < rows.Length; row_i++)
                    {
                        var it = rows[row_i];
                        if (it.Type == RowType.Summary) t_count--;
                        else
                        {
                            var cell = it.cells[cel_i];
                            if (cell is TCellCheck checkCell && checkCell.Checked) check_count++;
                        }
                    }
                    if (t_count == check_count) checkColumn.CheckState = System.Windows.Forms.CheckState.Checked;
                    else if (check_count > 0) checkColumn.CheckState = System.Windows.Forms.CheckState.Indeterminate;
                    else checkColumn.CheckState = System.Windows.Forms.CheckState.Unchecked;
                }
                Invalidate();
            }
            else if (cel is TCellRadio radio)
            {
                if (value is bool b) radio.Checked = b;
                row.Select = RowISelect(row);
                Invalidate();
            }
            else if (cel is TCellSwitch _switch)
            {
                if (value is bool b) _switch.Checked = b;
                Invalidate();
            }
            else
            {
                LoadLayout();
                Invalidate();
            }
        }

        internal void CheckAll(int i_cel, ColumnCheck columnCheck, bool value)
        {
            if (rows == null) return;
            int count = 0, nocount = 0;
            bool old = pauseLayout;
            pauseLayout = true;
            for (int i_row = 1; i_row < rows.Length; i_row++)
            {
                var item = rows[i_row].cells[i_cel];
                if (item.ROW.Type == RowType.Summary) continue;
                if (item is TCellCheck checkCell)
                {
                    count++;
                    if (checkCell.Checked != value)
                    {
                        checkCell.Checked = value;
                        SetValue(item, checkCell.Checked);
                        CheckedChanged?.Invoke(this, new TableCheckEventArgs(value, rows[i_row].RECORD, i_row, i_cel, item.COLUMN));
                    }
                    else nocount++;
                }
            }
            pauseLayout = old;
            if (count > 0 && nocount == count) columnCheck.Checked = value;
        }

        void SetValue(CELL cel, object? value)
        {
            if (cel.PROPERTY == null)
            {
                if (cel.VALUE is AntItem arow) arow.value = value;
                else if (cel.ROW.RECORD is System.Data.DataRow datarow)
                {
                    datarow[cel.INDEX] = cel.VALUE = value;
                    if (dataTmp == null) return;
                    dataTmp.rows[cel.ROW.INDEX - 1].SetValue(cel.INDEX, value);
                }
            }
            else cel.PROPERTY.SetValue(cel.VALUE, value);
        }

        bool RowISelect(RowTemplate row)
        {
            for (int cel_i = 0; cel_i < row.cells.Length; cel_i++)
            {
                var cel = row.cells[cel_i];
                if (cel is TCellCheck check)
                {
                    if (check.Checked) return true;
                }
                else if (cel is TCellRadio radio)
                {
                    if (radio.Checked) return true;
                }
            }
            return false;
        }

        #endregion
    }
}