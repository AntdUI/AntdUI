// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace AntdUI
{
    partial class Table
    {
        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            if (LoadLayout()) Invalidate();
        }

        string? show_oldrect;
        protected override void OnSizeChanged(EventArgs e)
        {
            var rect = ClientRectangle;
            if (IsHandleCreated && rect.Width > 1 && rect.Height > 1)
            {
                string show_rect = rect.Width + "_" + rect.Height;
                if (show_oldrect == show_rect) return;
                show_oldrect = show_rect;
                LoadLayout(rect);
                EditModeClose();
                base.OnSizeChanged(e);
            }
        }

        internal RowTemplate[]? rows;
        int rowSummary = 0;
        internal List<object> rows_Expand = new List<object>();
        int[][] dividers = new int[0][], dividerHs = new int[0][];
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
            var rect = LayoutDesign(rect_t.PaddingRect(Padding, borderWidth));
            ScrollBar.SizeChange(rect);
        }

        bool has_check = false;
        Rectangle rect_read, rect_divider;
        public Rectangle RectRead => rect_read;
        Rectangle LayoutDesign(Rectangle rect)
        {
            rowSummary = 0;
            has_check = false;
            if (dataTmp == null)
            {
                ThreadState?.Dispose();
                ThreadState = null;
                if (visibleHeader && emptyHeader && columns != null && columns.Count > 0)
                {
                    var _rows = LayoutDesign(rect, new TempTable(new TempiColumn[0], new IRow[0], null), out bool Processing, out var Columns, out var ColWidth, out int KeyTreeIndex);
                    rows = LayoutDesign(rect, _rows, Columns, ColWidth, KeyTreeIndex, out int x, out int y, out bool is_exceed);
                    ScrollBar.SetVrSize(is_exceed ? x : 0, y);
                    return rect;
                }
                else
                {
                    ScrollBar.SetVrSize(0, 0);
                    dividers = new int[0][];
                    rows = null;
                }
            }
            else
            {
                var _rows = LayoutDesign(rect, dataTmp, out bool Processing, out var Columns, out var ColWidth, out int KeyTreeIndex);
                if (visibleHeader && EmptyHeader && _rows.Count == 0)
                {
                    rows = LayoutDesign(rect, _rows, Columns, ColWidth, KeyTreeIndex, out int x, out int y, out bool is_exceed);
                    ScrollBar.SetVrSize(is_exceed ? x : 0, y);
                    ThreadState?.Dispose(); ThreadState = null;
                    return rect;
                }
                else if (_rows.Count > 0)
                {
                    rows = LayoutDesign(rect, _rows, Columns, ColWidth, KeyTreeIndex, out int x, out int y, out bool is_exceed);
                    if (scrollBarAvoidHeader && visibleHeader && fixedHeader)
                    {
                        int headerHeight = rows[0].Height;
                        if (headerHeight > 0 && rect.Height > headerHeight)
                        {
                            y -= headerHeight;
                            rect = new Rectangle(rect.X, rect.Y + headerHeight, rect.Width, rect.Height - headerHeight);
                        }
                    }
                    ScrollBar.SetVrSize(is_exceed ? x : 0, y);
                    if (Processing && Config.HasAnimation(nameof(Table)))
                    {
                        if (ThreadState == null)
                        {
                            ThreadState = new AnimationTask(new AnimationLinearConfig(this, i =>
                            {
                                AnimationStateValue = i / 100F;
                                Invalidate();
                                return true;
                            }, 50, 100, 5));
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
                    ThreadState?.Dispose();
                    ThreadState = null;
                    ScrollBar.SetVrSize(0, 0);
                    dividers = new int[0][];
                    rows = null;
                }
            }
            return Rectangle.Empty;
        }

        List<RowTemplate?> LayoutDesign(Rectangle rect, TempTable dataTmp, out bool Processing, out List<Column> Columns, out Dictionary<int, object> ColWidth, out int KeyTreeIndex)
        {
            int processing = 0;
            var col_width = new Dictionary<int, object>();
            string? KeyTree = null;
            int KeyTreeINDEX = -1;

            #region 处理表头

            var _columns = new List<Column>(dataTmp.columns.Length);
            if (columns == null || columns.Count == 0) ForColumn(dataTmp.columns, it => _columns.Add(it));
            else
            {
                int x = 0;
                ForColumn(columns, it =>
                {
                    int INDEX = _columns.Count;
                    _columns.Add(it);
                    if (it.Width != null) col_width.Add(x, ColumnWidth(it.Width));
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

            #endregion

            int start = 0, end = dataTmp.rows.Length;
            var _rows = new List<RowTemplate?>(end);

            if (VirtualMode)
            {
                var dpi = Dpi;
                Helper.GDI(g =>
                {
                    int gap = (int)(_gap.Height * Dpi) * 2;

                    #region 虚拟计算需要布局坐标的宽高

                    var font_size = g.MeasureString(Config.NullText, Font);

                    int RowHeight = font_size.Height + gap, RowHeightHeader = RowHeight;

                    if (rowHeight.HasValue) RowHeightHeader = RowHeight = (int)(rowHeight.Value * dpi);
                    if (rowHeightHeader.HasValue) RowHeightHeader = (int)(rowHeightHeader.Value * dpi);

                    _RowHeightHeader = RowHeightHeader;
                    _RowHeight = RowHeight;

                    int sy = ScrollBar.ValueY, visibleRowCount = (int)Math.Ceiling((double)rect.Height / RowHeight) + 2;

                    start = (int)Math.Floor((double)sy / RowHeight);
                    end = start + visibleRowCount;

                    #endregion
                });
            }
            else _RowHeightHeader = _RowHeight = null;

            if (KeyTree == null)
            {
                ForRow(dataTmp, start, end, row =>
                {
                    if (row == null)
                    {
                        _rows.Add(null);
                        return;
                    }
                    var cells = new List<CELL>(_columns.Count);
                    foreach (var column in _columns) AddRows(ref cells, ref processing, column, row, column.Key);
                    if (cells.Count > 0) AddRows(ref _rows, cells.ToArray(), row.i, row.record);
                });
            }
            else
            {
                ForRow(dataTmp, start, end, row =>
                {
                    if (row == null)
                    {
                        _rows.Add(null);
                        return;
                    }
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
                    foreach (var column in _columns) AddRows(ref cells, ref processing, column, row, column.Key, true);
                    if (cells.Count > 0)
                    {
                        rowSummary++;
                        var tmp = AddRows(ref _rows, cells.ToArray(), row.i, row.record);
                        tmp.Type = RowType.Summary;
                    }
                }
            }

            dataOne = false;
            Processing = processing > 0;
            Columns = _columns;
            ColWidth = col_width;
            KeyTreeIndex = KeyTreeINDEX;
            return _rows;
        }

        int? _RowHeightHeader, _RowHeight;

        RowTemplate[] LayoutDesign(Rectangle rect, List<RowTemplate?> _rows, List<Column> _columns, Dictionary<int, object> col_width, int KeyTreeINDEX, out int _x, out int _y, out bool _is_exceed)
        {
            #region 添加表头

            var _cols = new List<TCellColumn>(_columns.Count);
            foreach (var it in _columns) _cols.Add(new TCellColumn(this, it));
            AddRows(ref _rows, _cols.ToArray(), dataSource ?? _rows);

            #endregion

            #region 计算坐标

            int x = 0, y = 0;
            bool is_exceed = false;
            var rect_real = new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
            var rowlist = new List<RowTemplate>(_rows.Count);
            Helper.GDI(g =>
            {
                var dpi = Dpi;
                var font_size = g.MeasureString(Config.NullText, Font);
                var gap = new TableGaps(_gap, dpi);
                int check_size = (int)(_checksize * dpi), switchsize = (int)(_switchsize * dpi), treesize = (int)(TreeButtonSize * dpi),
                 gapTree = (int)(_gapTree * dpi), gapTree2 = gapTree * 2, sort_size = (int)(DragHandleSize * dpi), sort_ico_size = (int)(DragHandleIconSize * dpi), split_move = (int)(6F * dpi);

                check_radius = check_size * .12F * dpi;
                check_border = check_size * .04F * dpi;

                var firstrow = _rows[0]!;

                #region 布局高宽

                var read_width_cell = new Dictionary<int, AutoWidth>(firstrow.cells.Length);
                for (int cel_i = 0; cel_i < firstrow.cells.Length; cel_i++) read_width_cell.Add(cel_i, new AutoWidth());
                var tmp_width_cell = new Dictionary<int, int>();

                #region 处理需要的行

                int heightEs = 0;
                for (int row_i = 0; row_i < _rows.Count; row_i++)
                {
                    var row = _rows[row_i];
                    if (row == null) continue;
                    row.INDEX = row_i;
                    if (row_i == hovers) row.hover = true;
                    if (row.ShowExpand)
                    {
                        int max_height = 0;
                        if (row.IsColumn)
                        {
                            for (int cel_i = 0; cel_i < row.cells.Length; cel_i++)
                            {
                                var it = row.cells[cel_i];
                                it.INDEX = cel_i;
                                var text_size = it.GetSize(g, columnfont ?? Font, font_size, rect.Width, gap);
                                var readWidthCell = read_width_cell[cel_i];
                                if (it.COLUMN is ColumnSort) readWidthCell.value = -2;
                                else if (it.COLUMN is ColumnCheck check && check.NoTitle) readWidthCell.value = -1;
                                else
                                {
                                    int width = text_size.Width;
                                    if (readWidthCell.value < width) readWidthCell.value = width;
                                    if (readWidthCell.minvalue < it.MinWidth) readWidthCell.minvalue = it.MinWidth;
                                }
                                if (max_height < text_size.Height) max_height = text_size.Height;
                            }

                            if (_RowHeightHeader.HasValue) row.Height = max_height = _RowHeightHeader.Value;
                            else if (rowHeightHeader.HasValue) row.Height = (int)(rowHeightHeader.Value * dpi);
                            else if (rowHeight.HasValue) row.Height = (int)(rowHeight.Value * dpi);
                            else row.Height = max_height + gap.y2;
                            if (visibleHeader) heightEs += row.Height;
                            tmp_width_cell = CalculateWidth(rect, heightEs * _rows.Count, false, ref rect_real, col_width, read_width_cell, gap.x2, check_size, sort_size, ref is_exceed);
                            var del_tmp_width_cell = new List<int>(tmp_width_cell.Count);
                            foreach (var it in tmp_width_cell)
                            {
                                if (col_width.TryGetValue(it.Key, out var value))
                                {
                                    if (value is float) del_tmp_width_cell.Add(it.Key);
                                }
                            }
                            if (del_tmp_width_cell.Count > 0)
                            {
                                foreach (var it in del_tmp_width_cell) tmp_width_cell.Remove(it);
                            }
                        }
                        else
                        {
                            for (int cel_i = 0; cel_i < row.cells.Length; cel_i++)
                            {
                                var it = row.cells[cel_i];
                                it.INDEX = cel_i;
                                if (it.COLUMN is ColumnSort || (it is TCellCheck check && check.NoTitle))
                                {
                                    if (max_height < gap.y2) max_height = gap.y2;
                                }
                                else
                                {
                                    int tmpw = rect.Width;
                                    if (tmp_width_cell.TryGetValue(cel_i, out int tv)) tmpw = tv;
                                    var text_size = it.GetSize(g, Font, font_size, tmpw, gap);
                                    int width = text_size.Width;
                                    if (it.ROW.CanExpand && firstrow.cells[cel_i].INDEX == KeyTreeINDEX) width += (treesize + gapTree2) * (it.ROW.ExpandDepth + 1) - treesize / 2;
                                    if (max_height < text_size.Height) max_height = text_size.Height;
                                    if (read_width_cell[cel_i].value < width) read_width_cell[cel_i].value = width;
                                }
                            }

                            if (_RowHeight.HasValue) row.Height = max_height = _RowHeight.Value;
                            else if (rowHeight.HasValue) row.Height = (int)(rowHeight.Value * dpi);
                            else row.Height = max_height + gap.y2;
                            heightEs += row.Height;
                        }
                    }
                }

                #endregion

                foreach (var it in read_width_cell)
                {
                    var minWidth = _columns[it.Key].MinWidth;
                    if (minWidth != null)
                    {
                        if (minWidth.EndsWith("%") && float.TryParse(minWidth.TrimEnd('%'), out var f))
                        {
                            int min = (int)(rect.Width * f / 100F);
                            if (min > firstrow.cells[it.Key].MinWidth) firstrow.cells[it.Key].MinWidth = min;
                            if (it.Value.value < min)
                            {
                                it.Value.value = min;
                                if (col_width.TryGetValue(it.Key, out _)) col_width[it.Key] = min;
                                else col_width.Add(it.Key, min);
                            }
                        }
                        else if (int.TryParse(minWidth, out var i))
                        {
                            int min = (int)(i * Dpi);
                            if (min > firstrow.cells[it.Key].MinWidth) firstrow.cells[it.Key].MinWidth = min;
                            if (it.Value.value < min)
                            {
                                it.Value.value = min;
                                if (col_width.TryGetValue(it.Key, out _)) col_width[it.Key] = min;
                                else col_width.Add(it.Key, min);
                            }
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
                            int max = (int)(i * Dpi);
                            if (it.Value.value > max)
                            {
                                it.Value.value = max;
                                if (col_width.TryGetValue(it.Key, out _)) col_width[it.Key] = max;
                                else col_width.Add(it.Key, max);
                            }
                        }
                    }
                }

                var width_cell = CalculateWidth(rect, heightEs, true, ref rect_real, col_width, read_width_cell, gap.x2, check_size, sort_size, ref is_exceed);

                #endregion

                #region 最终坐标

                if (StackedHeaderRows != null) firstrow.Height += firstrow.Height * StackedHeaderRows.Length;

                int use_y;
                if (visibleHeader) use_y = rect.Y;
                else use_y = rect.Y - firstrow.Height;
                int i2 = 0;
                foreach (var cell in firstrow.cells)
                {
                    cell.COLUMN.WidthPixel = width_cell[i2];
                    i2++;
                }
                foreach (var row in _rows)
                {
                    if (row == null)
                    {
                        use_y += _RowHeight!.Value;
                        continue;
                    }
                    rowlist.Add(row);
                    if (row.ShowExpand)
                    {
                        int use_x = rect.X;
                        row.RECT = new Rectangle(rect.X, use_y, rect_real.Width, row.Height);
                        for (int i = 0; i < row.cells.Length; i++)
                        {
                            var it = row.cells[i];
                            var _rect = new Rectangle(use_x, use_y, width_cell[i], row.RECT.Height);
                            int ox = 0;
                            if (row.INDEX > 0 && firstrow.cells[i].INDEX == KeyTreeINDEX)
                            {
                                int xt = gapTree * row.ExpandDepth;
                                ox = xt + (gapTree + treesize);
                                row.RectExpand = new Rectangle(use_x + xt + split_move, use_y + (row.Height - treesize) / 2, treesize, treesize);
                            }

                            if (it is TCellCheck check) check.SetSize(_rect, check_size);
                            else if (it is TCellRadio radio) radio.SetSize(_rect, check_size);
                            else if (it is TCellSwitch _switch) _switch.SetSize(_rect, switchsize);
                            else if (it is TCellSort sort) sort.SetSize(_rect, sort_size, sort_ico_size);
                            else if (it is TCellSelect select) select.SetSize(g, Font, font_size, _rect, ox, gap);
                            else if (it is TCellColumn column)
                            {
                                it.SetSize(g, Font, font_size, _rect, ox, gap);
                                if (column.COLUMN is ColumnCheck columnCheck && columnCheck.NoTitle)
                                {
                                    column.COLUMN.SortOrder = false;
                                    columnCheck.PARENT = this;
                                    column.RECT_REAL = new Rectangle(_rect.X + (_rect.Width - check_size) / 2, _rect.Y + (_rect.Height - check_size) / 2, check_size, check_size);
                                }
                                else
                                {
                                    column.RECT_REAL = new Rectangle(_rect.X + gap.x, _rect.Y, _rect.Width - gap.x2 - column.SFWidth, _rect.Height);
                                    if (x < column.RECT_REAL.Right) x = column.RECT_REAL.Right;
                                }
                            }
                            else it.SetSize(g, Font, font_size, _rect, ox, gap);

                            if (x < _rect.Right) x = _rect.Right;
                            if (y < _rect.Bottom) y = _rect.Bottom;
                            use_x += width_cell[i];
                        }
                        use_y += row.Height;
                    }
                }

                x -= rect_real.X;
                y -= rect_real.Y;

                #endregion

                int last_index = _rows.Count - 1;
                var last_row = _rows[last_index];
                while (!last_row!.ShowExpand)
                {
                    last_index--;
                    last_row = _rows[last_index];
                }
                var last = last_row.cells[last_row.cells.Length - 1];

                bool isempty = emptyHeader && _rows.Count == 1;
                if (!isempty && (rect.Y + rect.Height) > last.RECT.Bottom) rect_real.Height = last.RECT.Bottom - rect.Y + (int)Math.Ceiling(borderWidth * dpi);
                rect_divider = new Rectangle(rect_real.X, rect_real.Y, rect_real.Width, rect_real.Height);

                var MoveHeaders = new List<MoveHeader>();
                var moveheaders_dir = new Dictionary<int, MoveHeader>(moveheaders.Length);
                foreach (var item in moveheaders) moveheaders_dir.Add(item.i, item);

                if (BorderCellWidth > 0)
                {
                    List<int[]> _dividerHs = new List<int[]>(firstrow.cells.Length), _dividers = new List<int[]>(_rows.Count);
                    foreach (var row in rowlist)
                    {
                        if (row.IsColumn)
                        {
                            if (EnableHeaderResizing)
                            {
                                int split_move2 = split_move / 2;
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
                                        _dividerHs.Add(new int[] { it.RECT.Right, rect.Y, it.RECT.Height });
                                    }
                                }
                                else
                                {
                                    for (int i = 0; i < row.cells.Length - 1; i++)
                                    {
                                        var it = row.cells[i];
                                        _dividerHs.Add(new int[] { it.RECT.Right, rect.Y, rect_real.Height });
                                    }
                                }
                            }
                            else
                            {
                                for (int i = 0; i < row.cells.Length - 1; i++)
                                {
                                    var it = row.cells[i];
                                    _dividerHs.Add(new int[] { it.RECT.Right, it.RECT.Y + gap.y, it.RECT.Height - gap.y2 });
                                }
                            }
                            if (visibleHeader) _dividers.Add(new int[] { row.RECT.Bottom, rect.X, rect_real.Width });
                        }
                        else
                        {
                            if (bordered) _dividers.Add(new int[] { row.RECT.Bottom, rect.X, rect_real.Width });
                            else _dividers.Add(new int[] { row.RECT.Bottom, row.RECT.X, row.RECT.Width });
                        }
                    }
                    if (bordered && !isempty) _dividers.RemoveAt(_dividers.Count - 1);
                    dividerHs = _dividerHs.ToArray();
                    dividers = _dividers.ToArray();
                }
                else
                {
                    dividers = new int[0][];
                    dividerHs = new int[0][];
                }
                moveheaders = MoveHeaders.ToArray();
            });

            #endregion

            _x = x;
            _y = y;
            rect_read = rect_real;
            _is_exceed = is_exceed;
            return rowlist.ToArray();
        }

        public virtual void OnShowXChanged(bool value) { }
        public virtual void OnShowYChanged(bool value) { }
        public virtual void OnValueXChanged(int value)
        {
            EditModeClose();
        }
        public virtual void OnValueYChanged(int value)
        {
            if (VirtualMode) LoadLayout();
            EditModeClose();
        }

        #region 通用循环

        void ForColumn(TempiColumn[] columns, Action<Column> action)
        {
            if (SortHeader == null)
            {
                foreach (var it in columns)
                {
                    int index = 0;
                    action(new Column(it.key, it.text ?? it.key) { INDEX = index, INDEX_REAL = index });
                    index++;
                }
            }
            else
            {
                foreach (var i in SortHeader)
                {
                    var it = columns[i];
                    action(new Column(it.key, it.text ?? it.key) { INDEX = i, INDEX_REAL = i });
                }
            }
        }
        void ForColumn(ColumnCollection columns, Func<Column, int> action)
        {
            int index = 0;
            if (SortHeader == null)
            {
                foreach (var it in columns)
                {
                    it.PARENT = this;
                    it.INDEX_REAL = index;
                    if (it.Visible) it.INDEX = action(it);
                    index++;
                }
            }
            else
            {
                var dir = new Dictionary<int, Column>();
                foreach (var it in columns)
                {

                    it.INDEX_REAL = index;
                    dir.Add(dir.Count, it);
                    index++;
                }
                try
                {
                    foreach (var i in SortHeader)
                    {
                        var it = dir[i];
                        it.PARENT = this;
                        if (it.Visible) it.INDEX = action(it);
                    }
                }
                catch { }
            }
        }
        void ForColumnI(ColumnCollection columns, Action<Column> action)
        {
            if (SortHeader == null)
            {
                foreach (var it in columns)
                {
                    if (it.Visible) action(it);
                }
            }
            else
            {
                var dir = new Dictionary<int, Column>();
                foreach (var it in columns) dir.Add(dir.Count, it);
                try
                {
                    foreach (var index in SortHeader)
                    {
                        var it = dir[index];
                        if (it.Visible) action(it);
                    }
                }
                catch { }
            }
        }
        void ForRow(TempTable data_temp, int i_start, int i_end, Action<IRow?> action)
        {
            int len = data_temp.rows.Length, lenr = len - 1;
            if (SortData == null || SortData.Length != data_temp.rows.Length)
            {
                for (int i = 0; i < len; i++)
                {
                    if ((i == 0 || i == lenr) || (i >= i_start && i < i_end)) action(data_temp.rows[i]);
                    else action(null);
                }
            }
            else
            {
                for (int i = 0; i < len; i++)
                {
                    if ((i == 0 || i == lenr) || (i >= i_start && i < i_end)) action(data_temp.rows[SortData[i]]);
                    else action(null);
                }
            }
        }
        bool ForTree(ref List<RowTemplate?> _rows, ref int processing, RowTemplate row_new, IRow row, List<Column> _columns, string KeyTree, int KeyTreeINDEX, int depth, bool show)
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
                for (int i = 0; i < list_tree.Length; i++)
                {
                    var item_tree = GetRow(list_tree[i], _columns.Count);
                    if (item_tree.Count > 0)
                    {
                        var row_tree = new IRow(i, list_tree[i], item_tree);
                        var cells_tree = new List<CELL>(_columns.Count);
                        foreach (var column in _columns) AddRows(ref cells_tree, ref processing, column, row_tree, column.Key);
                        var _row = AddRows(ref _rows, cells_tree.ToArray(), row.i, row_tree.record);
                        _row.INDEX_REAL_KEY = i;
                        if (ForTree(ref _rows, ref processing, _row, row_tree, _columns, KeyTree, KeyTreeINDEX, depth + 1, show)) count++;
                    }
                }
            }
            return count > 0;
        }

        static object[]? ForTreeValue(IRow row, string KeyTree)
        {
            if (row.cells.TryGetValue(KeyTree, out var ov))
            {
                if (ov is AntItem item) return ForTreeValue(item.value);
                else if (ov is PropertyDescriptor prop) return ForTreeValue(prop.GetValue(row.record));
            }
            return null;
        }
        static object[]? ForTreeValue(object? value)
        {
            if (value == null) return null;
            if (value is IList<AntItem[]> list_items && list_items.Count > 0)
            {
                var list = new List<object>(list_items.Count);
                foreach (var it in list_items) list.Add(it);
                return list.ToArray();
            }
            else if (value is IList<object> lists && lists.Count > 0) return lists.ToArray();
            else if (value is IEnumerable<object> listi)
            {
                var list = listi.ToList();
                return list.Count > 0 ? list.ToArray() : null;
            }
            return null;
        }

        #endregion

        Dictionary<int, int> tmpcol_width = new Dictionary<int, int>(0);

        /// <summary>
        /// 计算宽度
        /// </summary>
        /// <param name="rect">区域</param>
        /// <param name="heightEs">预估高度</param>
        /// <param name="col_width">表头宽度代码</param>
        /// <param name="read_width">每列的当前值和最大值</param>
        /// <param name="gap2">边距2</param>
        /// <param name="check_size">复选框大小</param>
        /// <param name="sort_size">拖拽大小</param>
        /// <param name="is_exceed">是否超出容器宽度</param>
        Dictionary<int, int> CalculateWidth(Rectangle rect, int heightEs, bool change, ref Rectangle rect_read, Dictionary<int, object> col_width, Dictionary<int, AutoWidth> read_width, int gap2, int check_size, int sort_size, ref bool is_exceed)
        {
            bool showX = heightEs > rect_read.Bottom;
            int use_width = rect.Width, ex_width = showX ? ScrollBar.SIZE : 0;
            float max_width = ex_width;
            Dictionary<int, float> col_width_tmp = new Dictionary<int, float>(col_width.Count);
            foreach (var it in read_width)
            {
                if (tmpcol_width.TryGetValue(it.Key, out var tw))
                {
                    max_width += tw;
                    col_width_tmp.Add(it.Key, tw);
                }
                else if (col_width.TryGetValue(it.Key, out var value))
                {
                    float _value = 0;
                    if (value is int val_int)
                    {
                        if (val_int == -1) _value = it.Value.value;
                        else if (val_int == -2) _value = it.Value.minvalue;
                        else _value = val_int;
                    }
                    if (value is float val_float) _value = rect.Width * val_float;
                    max_width += _value;
                    col_width_tmp.Add(it.Key, _value);
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
                var fill_count = new List<int>(col_width.Count);
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
                        int tmpw = rect_read.Width - ex_width;
                        int fill_wi = 0;
                        foreach (var it in col_width_tmp) fill_wi += (int)Math.Round(it.Value);
                        sum_wi -= fill_wi;
                        int tw = tmpw - fill_wi;
                        //填充
                        var percentage = new Dictionary<int, int>(width_cell.Count);
                        foreach (var it in width_cell)
                        {
                            if (col_width_tmp.TryGetValue(it.Key, out _)) percentage.Add(it.Key, it.Value);
                            else percentage.Add(it.Key, (int)Math.Round(tw * (it.Value * 1.0 / sum_wi)));
                        }
                        width_cell = percentage;
                    }
                    else if (change) rect_read.Width = sum_wi;
                }
            }
            return width_cell;
        }

        /// <summary>
        /// 通过表头属性获取列宽/代码
        /// </summary>
        /// <param name="width">宽度属性</param>
        /// <returns>float 百分比/int 固定值/-2 FILL/-1 AUTO</returns>
        object ColumnWidth(string width)
        {
            if (width.EndsWith("%") && float.TryParse(width.TrimEnd('%'), out var f)) return f / 100F;
            else if (int.TryParse(width, out var i)) return (int)(i * Dpi);
            else if (width.Contains("fill")) return -2;//填充剩下的
            else return -1; //AUTO
        }

        #region 动画

        AnimationTask? ThreadState;
        internal float AnimationStateValue = 0;

        #endregion

        float check_radius = 0F, check_border = 1F;
        void AddRows(ref List<CELL> cells, ref int processing, Column column, IRow row, string key, bool summary = false)
        {
            if (summary)
            {
                if (row.cells.TryGetValue(key, out var ov))
                {
                    var value = OGetValue(ov, row.record, out var property, out var rv);
                    AddRows(ref cells, ref processing, column, rv, value, property, summary);
                }
                else AddRows(ref cells, ref processing, column, null, null, null, summary);
            }
            else
            {
                if (column is ColumnSort columnSort) cells.Add(new TCellSort(this, columnSort));
                else if (row.cells.TryGetValue(key, out var ov))
                {
                    var value = OGetValue(ov, row.record, out var property, out var rv);
                    if (column.Render == null) AddRows(ref cells, ref processing, column, rv, value, property, summary);
                    else AddRows(ref cells, ref processing, column, rv, column.Render?.Invoke(value, row.record, row.i), property, summary);
                }
                else AddRows(ref cells, ref processing, column, null, column.Render?.Invoke(null, row.record, row.i), null, summary);
            }
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
        void AddRows(ref List<CELL> cells, ref int processing, Column column, object? ov, object? value, PropertyDescriptor? prop, bool summary)
        {
            if (value == null) cells.Add(new TCellText(this, column, prop, ov, null));
            else cells.Add(GetCELL(column, ref processing, ov, value, prop, summary));
            if (ov is INotifyPropertyChanged notify)
            {
                notify.PropertyChanged -= Notify_PropertyChanged;
                notify.PropertyChanged += Notify_PropertyChanged;
            }
        }

        CELL GetCELL(Column column, ref int processing, object? ov, object value, PropertyDescriptor? prop, bool summary)
        {
            if (summary) return AddRows(column, ref processing, ov, value, prop);
            else if (column is ColumnCheck columnCheck)
            {
                //复选框
                has_check = true;
                bool value_check = false, val_int = false;
                if (value is bool check) value_check = check;
                else if (value is int check_int)
                {
                    value_check = check_int > 0;
                    val_int = true;
                }
                return new TCellCheck(this, columnCheck, prop, ov, value_check, val_int);
            }
            else if (column is ColumnRadio columnRadio)
            {
                //单选框
                has_check = true;
                bool value_check = false, val_int = false;
                if (value is bool check) value_check = check;
                else if (value is int check_int)
                {
                    value_check = check_int > 0;
                    val_int = true;
                }
                return new TCellRadio(this, columnRadio, prop, ov, value_check, val_int);
            }
            else if (column is ColumnSwitch columnSwitch)
            {
                //开关
                bool value_check = false, val_int = false;
                if (value is bool check) value_check = check;
                else if (value is int check_int)
                {
                    value_check = check_int > 0;
                    val_int = true;
                }
                return new TCellSwitch(this, columnSwitch, prop, ov, value_check, val_int);
            }
            else if (column is ColumnSelect columnSelect)
            {
                //键值类型
                return new TCellSelect(this, columnSelect, prop, ov, value);
            }
            else return AddRows(column, ref processing, ov, value, prop);
        }
        CELL AddRows(Column column, ref int processing, object? ov, object value, PropertyDescriptor? prop)
        {
            if (value is IList<ICell> icells)
            {
                var tmp = new Template(this, column, prop, ov, ref processing, icells);
                foreach (var it in tmp.Value)
                {
                    it.Changed = layout =>
                    {
                        if (layout) LoadLayout();
                        Invalidate();
                    };
                }
                return tmp;
            }
            else if (value is ICell icell)
            {
                var tmp = new Template(this, column, prop, ov, ref processing, new ICell[] { icell });
                foreach (var it in tmp.Value)
                {
                    it.Changed = layout =>
                    {
                        if (layout) LoadLayout();
                        Invalidate();
                    };
                }
                return tmp;
            }
            else if (column is TemplateColumn tc) return tc.CreateCell(this, tc, prop, ov, ref processing, value);
            else return new TCellText(this, column, prop, ov, value);
        }

        RowTemplate AddRows(ref List<RowTemplate?> rows, CELL[] cells, int row_i, object record)
        {
            var row = new RowTemplate(this, cells, row_i, record);
            if (enableDir.Contains(record)) row.ENABLE = false;
            foreach (var it in row.cells) it.SetROW(row);
            rows.Add(row);
            return row;
        }
        RowTemplate AddRows(ref List<RowTemplate?> rows, TCellColumn[] cells, object record)
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
                        if (VirtualMode && dataTmp != null)
                        {
                            t_count = dataTmp.rows.Length;
                            ForRow(dataTmp, 0, dataTmp.rows.Length, row =>
                            {
                                if (row == null) return;
                                if (row[checkColumn.Key] is bool tmp)
                                {
                                    if (tmp) check_count++;
                                }
                                else if (row[checkColumn.Key] is int tmp_int)
                                {
                                    if (tmp_int == 1) check_count++;
                                }
                            });
                        }
                        else
                        {
                            for (int row_i = 0; row_i < rows.Count; row_i++)
                            {
                                var tmp = rows[row_i];
                                if (tmp == null) continue;
                                if (tmp.Type == RowType.Summary) t_count--;
                                else
                                {
                                    var cell = tmp.cells[i];
                                    if (cell is TCellCheck checkCell && checkCell.Checked) check_count++;
                                }
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
            else if (ov is TableSubValue subValue)
            {
                value = subValue.record;
                property = subValue.prop;
                return property.GetValue(value);
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
                if (value is bool b)
                {
                    check.Checked = b;
                    if (b) selects.Add(row.INDEX_REAL);
                    else selects.Remove(row.INDEX_REAL);
                }
                else if (value is int b_int)
                {
                    check.Checked = b_int == 1;
                    if (check.Checked) selects.Add(row.INDEX_REAL);
                    else selects.Remove(row.INDEX_REAL);
                }
                if (cel.COLUMN is ColumnCheck checkColumn && checkColumn.NoTitle)
                {
                    if (pauseLayout) return;
                    IsCheckAll(cel_i, checkColumn);
                }
                Invalidate();
            }
            else if (cel is TCellRadio radio)
            {
                if (value is bool b)
                {
                    radio.Checked = b;
                    if (b) selects.Add(row.INDEX_REAL);
                    else selects.Remove(row.INDEX_REAL);
                }
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
            int t_count = 0;
            bool old = pauseLayout;
            pauseLayout = true;
            if (VirtualMode)
            {
                if (dataTmp == null) return;
                var index = new List<int>(dataTmp.rows.Length);
                int value_int = value ? 1 : 0;
                ForRow(dataTmp, 0, dataTmp.rows.Length, row =>
                {
                    if (row == null) return;
                    t_count++;
                    var obj = row[columnCheck.Key];
                    if (obj is bool tmp)
                    {
                        if (tmp == value)
                        {
                            index.Add(row.i);
                            return;
                        }
                        row.SetValue(columnCheck.Key, value);
                    }
                    else if (obj is int tmp_int)
                    {
                        if (tmp_int == value_int)
                        {
                            index.Add(row.i);
                            return;
                        }
                        row.SetValue(columnCheck.Key, value_int);
                    }
                    index.Add(row.i);
                });
                selects.Clear();
                if (value) selects.AddRange(index);
            }
            else
            {
                for (int i_row = 1; i_row < rows.Length; i_row++)
                {
                    var item = rows[i_row].cells[i_cel];
                    if (item.ROW.Type == RowType.Summary) continue;
                    if (item is TCellCheck checkCell)
                    {
                        t_count++;
                        if (checkCell.Checked == value) continue;
                        checkCell.Checked = value;
                        if (checkCell.ValInt)
                        {
                            SetValue(item, checkCell.Checked ? 1 : 0);
                            OnCheckedChanged(value, rows[i_row].RECORD, i_row, i_cel, item.COLUMN);
                        }
                        else
                        {
                            SetValue(item, checkCell.Checked);
                            OnCheckedChanged(value, rows[i_row].RECORD, i_row, i_cel, item.COLUMN);
                        }
                    }
                }
            }
            pauseLayout = old;
            if (t_count > 0)
            {
                if (value) columnCheck.CheckState = System.Windows.Forms.CheckState.Checked;
                else columnCheck.CheckState = System.Windows.Forms.CheckState.Unchecked;
            }
        }
        internal void IsCheckAll(int cel, ColumnCheck column)
        {
            int t_count = 0, check_count = 0;
            if (VirtualMode)
            {
                if (dataTmp == null) return;
                ForRow(dataTmp, 0, dataTmp.rows.Length, row =>
                {
                    if (row == null) return;
                    t_count++;
                    if (row[column.Key] is bool tmp)
                    {
                        if (tmp) check_count++;
                    }
                    else if (row[column.Key] is int tmp_int)
                    {
                        if (tmp_int == 1) check_count++;
                    }
                });
            }
            else
            {
                if (rows == null) return;
                t_count = rows.Length - 1;
                for (int row_i = 1; row_i < rows.Length; row_i++)
                {
                    var it = rows[row_i];
                    if (it.Type == RowType.Summary) t_count--;
                    else
                    {
                        var cell = it.cells[cel];
                        if (cell is TCellCheck checkCell && checkCell.Checked) check_count++;
                    }
                }
            }
            if (t_count == check_count) column.CheckState = System.Windows.Forms.CheckState.Checked;
            else if (check_count > 0) column.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            else column.CheckState = System.Windows.Forms.CheckState.Unchecked;
        }

        void SetValueCheck(CELL_CHECK cel) => SetValueCheck(cel, !cel.Checked);
        void SetValueCheck(CELL_CHECK cel, bool value)
        {
            cel.Checked = value;
            if (cel.ValInt) SetValue(cel, value ? 1 : 0);
            else SetValue(cel, value);
        }
        void SetValue(CELL cel, object? value)
        {
            if (cel.PROPERTY == null)
            {
                if (cel.VALUE is AntItem arow) arow.value = value;
                else if (cel.ROW.RECORD is System.Data.DataRow datarow)
                {
                    int col = cel.COLUMN.INDEX_REAL, row = cel.ROW.INDEX_REAL;
                    if (datarow.Table.Columns.Contains(cel.COLUMN.Key)) datarow[cel.COLUMN.Key] = cel.VALUE = value;
                    else datarow[col] = cel.VALUE = value;
                    if (dataTmp == null) return;
                    dataTmp.rows[row].SetValue(cel.COLUMN.Key, value);
                }
            }
            else cel.PROPERTY.SetValue(cel.VALUE, value);
        }

        #endregion
    }
}