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
            LoadLayout();
            Invalidate();
            base.OnFontChanged(e);
        }

        string? show_oldrect = null;
        protected override void OnSizeChanged(EventArgs e)
        {
            var rect = ClientRectangle;
            if (rect.Width > 1 && rect.Height > 1)
            {
                string show_rect = rect.Width + "_" + rect.Height;
                if (show_oldrect == show_rect) return;
                show_oldrect = show_rect;
                LoadLayout(rect);
                base.OnSizeChanged(e);
            }
        }

        internal RowTemplate[]? rows = null;
        Rectangle[] dividers = new Rectangle[0], dividerHs = new Rectangle[0];
        MoveHeader[] moveheaders = new MoveHeader[0];

        public void LoadLayout()
        {
            var rect = ClientRectangle;
            if (rect.Width > 1 && rect.Height > 1) LoadLayout(rect);
            else show_oldrect = null;
        }

        void LoadLayout(Rectangle rect_t)
        {
            var rect = ChangeLayout(rect_t);
            scrollBar.SizeChange(rect);
        }

        bool has_check = false;
        Rectangle rect_read, rect_divider;
        Rectangle ChangeLayout(Rectangle rect)
        {
            has_check = false;
            if (data_temp == null)
            {
                ThreadState?.Dispose(); ThreadState = null;
                scrollBar.SetVrSize(0, 0);
                dividers = new Rectangle[0];
                rows = null;
            }
            else
            {
                var _rows = new List<RowTemplate>(data_temp.rows.Length);
                var _columns = new List<Column>(data_temp.columns.Length);
                int processing = 0;
                var col_width = new Dictionary<int, object>();
                if (columns == null)
                {
                    if (SortHeader == null)
                    {
                        foreach (var it in data_temp.columns) _columns.Add(new Column(it.key, it.key) { INDEX = _columns.Count });
                    }
                    else
                    {
                        foreach (var i in SortHeader)
                        {
                            var it = data_temp.columns[i];
                            _columns.Add(new Column(it.key, it.key) { INDEX = i });
                        }
                    }
                }
                else
                {
                    int x = 0;
                    if (SortHeader == null)
                    {
                        foreach (var it in columns)
                        {
                            it.PARENT = this;
                            if (it.Visible)
                            {
                                it.INDEX = _columns.Count;
                                _columns.Add(it);
                                ColumnWidth(it, ref col_width, x);
                                x++;
                            }
                        }
                    }
                    else
                    {
                        var dir = new Dictionary<int, Column>();
                        foreach (var it in columns)
                        {
                            if (it.Visible) dir.Add(dir.Count, it);
                        }
                        foreach (var index in SortHeader)
                        {
                            var it = dir[index];
                            it.PARENT = this;
                            it.INDEX = index;
                            if (it.Visible)
                            {
                                _columns.Add(it);
                                ColumnWidth(it, ref col_width, x);
                                x++;
                            }
                        }
                    }
                }

                if (SortData == null || SortData.Length != data_temp.rows.Length)
                {
                    foreach (var row in data_temp.rows)
                    {
                        var cells = new List<TCell>(_columns.Count);
                        foreach (var column in _columns) AddRows(ref cells, ref processing, column, row, row.cells[column.Key]);
                        if (cells.Count > 0) AddRows(ref _rows, cells.ToArray(), row.record);
                    }
                }
                else
                {
                    foreach (var item in SortData)
                    {
                        var row = data_temp.rows[item];
                        var cells = new List<TCell>(_columns.Count);
                        foreach (var column in _columns) AddRows(ref cells, ref processing, column, row, row.cells[column.Key]);
                        if (cells.Count > 0) AddRows(ref _rows, cells.ToArray(), row.record);
                    }
                }

                if ((visibleHeader && EmptyHeader) && _rows.Count == 0)
                {
                    rows = ChangeLayoutCore(rect, _rows, _columns, col_width, out int x, out int y, out bool is_exceed);
                    scrollBar.SetVrSize(is_exceed ? x : 0, y);

                    ThreadState?.Dispose(); ThreadState = null;

                    return rect;
                }
                else if (_rows.Count > 0)
                {
                    rows = ChangeLayoutCore(rect, _rows, _columns, col_width, out int x, out int y, out bool is_exceed);
                    scrollBar.SetVrSize(is_exceed ? x : 0, y);

                    if (processing == 0) { ThreadState?.Dispose(); ThreadState = null; }
                    else
                    {
                        if (Config.Animation && ThreadState == null)
                        {
                            ThreadState = new ITask(this, i =>
                            {
                                AnimationStateValue = i;
                                Invalidate();
                            }, 50, 1F, 0.05F);
                        }
                    }

                    return rect;
                }
                else
                {
                    ThreadState?.Dispose(); ThreadState = null;
                    scrollBar.SetVrSize(0, 0);
                    dividers = new Rectangle[0];
                    rows = null;
                }
            }
            return Rectangle.Empty;
        }

        RowTemplate[] ChangeLayoutCore(Rectangle rect, List<RowTemplate> _rows, List<Column> _columns, Dictionary<int, object> col_width, out int _x, out int _y, out bool _is_exceed)
        {
            List<object?> dir_Select, dir_Hover = new List<object?>(1);
            if (rows != null)
            {
                dir_Select = new List<object?>(rows.Length);
                foreach (var item in rows)
                {
                    if (item.Select) dir_Select.Add(item.RECORD);
                    if (item.Hover) dir_Hover.Add(item.RECORD);
                }
            }
            else dir_Select = new List<object?>(0);
            foreach (var item in _rows)
            {
                if (dir_Select.Contains(item.RECORD)) item.Select = true;
                if (dir_Hover.Contains(item.RECORD)) item.Hover = true;
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
                int check_size = (int)(_checksize * dpi), gap = (int)(_gap * dpi), gap2 = gap * 2,
                split = (int)(1F * dpi), split2 = split / 2,
                split_move = (int)(6F * dpi), split_move2 = split_move / 2;

                check_radius = check_size * 0.12F * dpi;
                check_border = check_size * 0.04F * dpi;

                #region 布局高宽

                var read_width_cell = new Dictionary<int, AutoWidth>(_rows[0].cells.Length);
                for (int cel_i = 0; cel_i < _rows[0].cells.Length; cel_i++) read_width_cell.Add(cel_i, new AutoWidth());
                for (int row_i = 0; row_i < _rows.Count; row_i++)
                {
                    var row = _rows[row_i];
                    row.INDEX = row_i;
                    float max_height = 0;
                    if (row.IsColumn)
                    {
                        for (int cel_i = 0; cel_i < row.cells.Length; cel_i++)
                        {
                            var it = row.cells[cel_i];
                            it.INDEX = cel_i;
                            if (it is TCellCheck check && check.NoTitle)
                            {
                                if (max_height < gap2) max_height = gap2;
                                read_width_cell[cel_i].value = -1;
                            }
                            else
                            {
                                var text_size = it.GetSize(g, columnfont ?? Font, rect.Width, gap, gap2);
                                int width = (int)Math.Ceiling(text_size.Width);
                                if (max_height < text_size.Height) max_height = text_size.Height;
                                if (read_width_cell[cel_i].value < width) read_width_cell[cel_i].value = width;
                                if (read_width_cell[cel_i].minvalue < it.MinWidth) read_width_cell[cel_i].minvalue = it.MinWidth;
                            }
                        }
                    }
                    else
                    {
                        for (int cel_i = 0; cel_i < row.cells.Length; cel_i++)
                        {
                            var it = row.cells[cel_i];
                            it.INDEX = cel_i;
                            if (it is TCellCheck check)
                            {
                                if (check.NoTitle)
                                {
                                    if (max_height < gap2) max_height = gap2;
                                    read_width_cell[cel_i].value = -1;
                                }
                            }
                            else
                            {
                                var text_size = it.GetSize(g, Font, rect.Width, gap, gap2);
                                int width = (int)Math.Ceiling(text_size.Width);
                                if (max_height < text_size.Height) max_height = text_size.Height;
                                if (read_width_cell[cel_i].value < width) read_width_cell[cel_i].value = width;
                            }
                        }
                    }
                    row.Height = (int)Math.Round(max_height) + gap2;
                }
                foreach (var it in read_width_cell)
                {
                    var maxWidth = _columns[it.Key].MaxWidth;
                    if (maxWidth != null)
                    {
                        if (maxWidth.EndsWith("%") && float.TryParse(maxWidth.TrimEnd('%'), out var f))
                        {
                            int max = (int)(rect.Width * f / 100F);
                            if (it.Value.value > max) it.Value.value = max;
                        }
                        else if (int.TryParse(maxWidth, out var i))
                        {
                            int max = (int)(i * Config.Dpi);
                            if (it.Value.value > max) it.Value.value = max;
                        }
                    }
                }

                rect_read.Width = rect.Width;
                rect_read.Height = rect.Height;

                var width_cell = CalculateWidth(rect, col_width, read_width_cell, check_size, ref is_exceed);

                #endregion

                #region 最终坐标

                int use_y;
                if (visibleHeader) use_y = rect.Y;
                else { use_y = rect.Y - _rows[0].Height; }
                foreach (var row in _rows)
                {
                    int use_x = rect.X;
                    row.RECT = new Rectangle(rect.X, use_y, rect_read.Width, row.Height);
                    for (int i = 0; i < row.cells.Length; i++)
                    {
                        var it = row.cells[i];
                        var _rect = new Rectangle(use_x, use_y, width_cell[i], row.RECT.Height);
                        if (it is TCellCheck check) check.SetSize(_rect, check_size);
                        else if (it is TCellRadio radio) radio.SetSize(_rect, check_size);
                        else if (it is TCellSwitch _switch) _switch.SetSize(_rect, check_size);
                        else if (it is TCellColumn column)
                        {
                            it.SetSize(g, Font, _rect, gap, gap2);
                            if (column.column is ColumnCheck columnCheck && columnCheck.NoTitle)
                            {
                                column.column.SortOrder = false;
                                columnCheck.PARENT = this;
                                //全选
                                column.rect = new Rectangle(_rect.X + (_rect.Width - check_size) / 2, _rect.Y + (_rect.Height - check_size) / 2, check_size, check_size);
                            }
                            else
                            {
                                if (column.column.SortOrder) column.rect = new Rectangle(_rect.X + gap, _rect.Y + gap, _rect.Width - gap2 - column.SortWidth, _rect.Height - gap2);
                                else column.rect = new Rectangle(_rect.X + gap, _rect.Y + gap, _rect.Width - gap2, _rect.Height - gap2);
                                if (x < column.rect.Right) x = column.rect.Right;
                            }
                        }
                        else it.SetSize(g, Font, _rect, gap, gap2);

                        if (x < _rect.Right) x = _rect.Right;
                        if (y < _rect.Bottom) y = _rect.Bottom;
                        use_x += width_cell[i];
                    }
                    use_y += row.Height;
                }

                #endregion

                List<Rectangle> _dividerHs = new List<Rectangle>(), _dividers = new List<Rectangle>();
                var MoveHeaders = new List<MoveHeader>();

                var last_row = _rows[_rows.Count - 1];
                var last = last_row.cells[last_row.cells.Length - 1];

                bool iseg = EmptyHeader && _rows.Count == 1;
                if ((rect.Y + rect.Height) > last.RECT.Bottom && !iseg) rect_read.Height = last.RECT.Bottom - rect.Y;

                int sp2 = split * 2;
                rect_divider = new Rectangle(rect_read.X + split, rect_read.Y + split, rect_read.Width - sp2, rect_read.Height - sp2);

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
                                var it = (TCellColumn)row.cells[i];
                                MoveHeaders.Add(new MoveHeader(moveheaders_dir, new Rectangle(it.RECT.Right - split_move2, rect.Y, split_move, it.RECT.Height), i, it.RECT.Width, it.MinWidth));
                            }
                        }
                        if (bordered)
                        {
                            if (iseg)
                            {
                                for (int i = 0; i < row.cells.Length - 1; i++)
                                {
                                    var it = (TCellColumn)row.cells[i];
                                    _dividerHs.Add(new Rectangle(it.RECT.Right - split2, rect.Y, split, it.RECT.Height));
                                }
                            }
                            else
                            {
                                for (int i = 0; i < row.cells.Length - 1; i++)
                                {
                                    var it = (TCellColumn)row.cells[i];
                                    _dividerHs.Add(new Rectangle(it.RECT.Right - split2, rect.Y, split, rect_read.Height));
                                }
                            }
                            if (visibleHeader) _dividers.Add(new Rectangle(rect.X, row.RECT.Bottom - split2, rect_read.Width, split));
                        }
                        else
                        {
                            for (int i = 0; i < row.cells.Length - 1; i++)
                            {
                                var it = (TCellColumn)row.cells[i];
                                _dividerHs.Add(new Rectangle(it.RECT.Right - split2, it.rect.Y, split, it.rect.Height));
                            }
                        }
                    }
                    else
                    {
                        if (bordered) _dividers.Add(new Rectangle(rect.X, row.RECT.Bottom - split2, rect_read.Width, split));
                        else _dividers.Add(new Rectangle(row.RECT.X, row.RECT.Bottom - split2, row.RECT.Width, split));
                    }
                }
                if (bordered && !iseg) _dividers.RemoveAt(_dividers.Count - 1);
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

        Dictionary<int, int> tmpcol_width = new Dictionary<int, int>(0);
        Dictionary<int, int> CalculateWidth(Rectangle rect, Dictionary<int, object> col_width, Dictionary<int, AutoWidth> read_width, int check_size, ref bool is_exceed)
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
                    int size = check_size * 2;//复选框大小
                    max_width += size;
                    use_width -= size;
                }
                else max_width += it.Value.value;
            }

            var width_cell = new Dictionary<int, int>();
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
                    else if (it.Value.value == -1F)
                    {
                        int _check_size = check_size * 2;
                        width_cell.Add(it.Key, _check_size);
                    }
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
                    else if (it.Value.value == -1F)
                    {
                        int _check_size = check_size * 2;
                        width_cell.Add(it.Key, _check_size);
                    }
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
                            percentage.Add(it.Key, (int)Math.Round(rect_read.Width * (it.Value * 1.0) / sum_wi));
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
            if (it.Width != null)
            {
                if (it.Width.EndsWith("%") && float.TryParse(it.Width.TrimEnd('%'), out var f)) col_width.Add(x, f / 100F);
                else if (int.TryParse(it.Width, out var i)) col_width.Add(x, (int)(i * Config.Dpi));
                else if (it.Width.Contains("fill")) col_width.Add(x, -2);//填充剩下的
                else col_width.Add(x, -1); //AUTO
            }
        }

        #region 动画

        ITask? ThreadState = null;
        internal float AnimationStateValue = 0;

        #endregion

        float check_radius = 0F, check_border = 1F;
        void AddRows(ref List<TCell> cells, ref int processing, Column column, IRow row, object? ov)
        {
            if (ov is PropertyDescriptor prop) AddRows(ref cells, ref processing, column, row.record, prop);
            else
            {
                if (ov == null) cells.Add(new TCellText(this, null, ov, column, null));
                else
                {
                    if (ov is AntItem cell)
                    {
                        if (column is ColumnCheck columnCheck)
                        {
                            //复选框
                            has_check = true;
                            bool value = false;
                            if (cell.value is bool check) value = check;
                            AddRows(ref cells, new TCellCheck(this, null, ov, value, columnCheck));
                        }
                        else if (column is ColumnRadio columnRadio)
                        {
                            //单选框
                            has_check = true;
                            bool value = false;
                            if (cell.value is bool check) value = check;
                            AddRows(ref cells, new TCellRadio(this, null, ov, value, columnRadio));
                        }
                        else if (column is ColumnSwitch columnSwitch)
                        {
                            //开关
                            bool value = false;
                            if (cell.value is bool check) value = check;
                            AddRows(ref cells, new TCellSwitch(this, null, ov, value, columnSwitch));
                        }
                        else
                        {
                            if (cell.value is IList<ICell> icells) AddRows(ref cells, new Template(this, null, ov, column, ref processing, icells));
                            else if (cell.value is ICell icell) AddRows(ref cells, new Template(this, null, ov, column, ref processing, new ICell[] { icell }));
                            else AddRows(ref cells, new TCellText(this, null, ov, column, cell.value?.ToString()));
                        }
                    }
                    else cells.Add(new TCellText(this, null, ov, column, ov.ToString()));
                }
                //cells.Add(new TCellText(this, null, ov, column, ov?.ToString()));
            }
        }
        void AddRows(ref List<TCell> cells, ref int processing, Column column, object ov, PropertyDescriptor prop)
        {
            if (column is ColumnCheck columnCheck)
            {
                //复选框
                has_check = true;
                bool value = false;
                if (prop.GetValue(ov) is bool check) value = check;
                AddRows(ref cells, new TCellCheck(this, prop, ov, value, columnCheck));
            }
            else if (column is ColumnRadio columnRadio)
            {
                //单选框
                has_check = true;
                bool value = false;
                if (prop.GetValue(ov) is bool check) value = check;
                AddRows(ref cells, new TCellRadio(this, prop, ov, value, columnRadio));
            }
            else if (column is ColumnSwitch columnSwitch)
            {
                //开关
                bool value = false;
                if (prop.GetValue(ov) is bool check) value = check;
                AddRows(ref cells, new TCellSwitch(this, prop, ov, value, columnSwitch));
            }
            else
            {
                var value = prop.GetValue(ov);
                if (value is IList<ICell> icells) AddRows(ref cells, new Template(this, prop, ov, column, ref processing, icells));
                else if (value is ICell icell) AddRows(ref cells, new Template(this, prop, ov, column, ref processing, new ICell[] { icell }));
                else AddRows(ref cells, new TCellText(this, prop, ov, column, value?.ToString()));
            }
        }

        string? OGetValue(TempTable data_temp, int i_r, string key)
        {
            var value = data_temp.rows[i_r].cells[key];
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

        void AddRows(ref List<TCell> cells, TCell data)
        {
            cells.Add(data);
            var data_temp = data;
            if (data is Template template)
            {
                foreach (var it in template.value)
                {
                    if (it is TemplateBadge badge)
                    {
                        badge.Value.Changed = key =>
                        {
                            if (key == "SProcessing" || key == "EProcessing" || key == "Text") LoadLayout();
                            Invalidate();
                        };
                    }
                    else if (it is TemplateTag tag)
                    {
                        tag.Value.Changed = key =>
                        {
                            if (key == "Text") LoadLayout();
                            Invalidate();
                        };
                    }
                    else if (it is TemplateProgress progress)
                    {
                        progress.Value.Changed = key =>
                        {
                            if (key == "Shape") LoadLayout();
                            Invalidate();
                        };
                    }
                    else if (it is TemplateImage image)
                    {
                        image.Value.Changed = key =>
                        {
                            if (key == "Size") LoadLayout();
                            Invalidate();
                        };
                    }
                    else if (it is TemplateButton link)
                    {
                        link.Value.Changed = key =>
                        {
                            if (key == "Text" || key == "Image" || key == "ImageSvg" || key == "IconRatio" || key == "Shape" || key == "ShowArrow") LoadLayout();
                            Invalidate();
                        };
                    }
                    else if (it is TemplateText text)
                    {
                        text.Value.Changed = key =>
                        {
                            if (key == "IconRatio" || key == "Prefix" || key == "PrefixSvg" || key == "Suffix" || key == "SuffixSvg") LoadLayout();
                            Invalidate();
                        };
                    }
                }
            }
            if (data.VALUE is INotifyPropertyChanged notify)
            {
                notify.PropertyChanged -= Notify_PropertyChanged;
                notify.PropertyChanged += Notify_PropertyChanged;
            }
        }

        void AddRows(ref List<RowTemplate> rows, TCell[] cells, object? record)
        {
            var row = new RowTemplate(this, cells, record);
            foreach (var it in row.cells) it.ROW = row;
            rows.Add(row);
        }
        void AddRows(ref List<RowTemplate> rows, TCellColumn[] cells, object? record)
        {
            var row = new RowTemplate(this, cells, record)
            {
                IsColumn = true
            };
            foreach (var it in row.cells) it.ROW = row;
            rows.Insert(0, row);
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
                var column = (TCellColumn)rows[0].cells[cel_i];
                if (key == column.column.Key)
                {
                    if (data is AntItem item)
                    {
                        foreach (var row in rows)
                        {
                            if (row.RECORD is IList<AntItem> items && items.Contains(item))
                            {
                                RefreshItem(rows, column, row, row.cells[cel_i], cel_i, item.value);
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
                                if (cel.PROPERTY != null) RefreshItem(rows, column, row, cel, cel_i, cel.PROPERTY.GetValue(data));
                                return;
                            }
                        }
                    }
                    return;
                }
            }
        }

        void RefreshItem(RowTemplate[] rows, TCellColumn column, RowTemplate row, TCell cel, int cel_i, object? value)
        {
            if (cel is Template template)
            {
                int count = 0;
                if (value == null) count++;
                else if (value is IList<ICell> cells)
                {
                    if (template.value.Count == cells.Count)
                    {
                        for (int i = 0; i < template.value.Count; i++)
                        {
                            if (template.value[i].SValue(cells[i])) count++;
                        }
                    }
                    else count++;
                }
                else if (value is ICell cell)
                {
                    if (template.value.Count == 1)
                    {
                        if (template.value[0].SValue(cel)) count++;
                    }
                    else count++;
                }
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
                if (column.column is ColumnCheck checkColumn && checkColumn.NoTitle)
                {
                    int t_count = rows.Length - 1, check_count = 0;
                    for (int row_i = 1; row_i < rows.Length; row_i++)
                    {
                        var it = rows[row_i];
                        var cell = it.cells[cel_i];
                        if (cell is TCellCheck checkCell && checkCell.Checked) check_count++;
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

        public void CheckAll(int i_cel, bool value)
        {
            if (rows == null) return;
            for (int i_row = 1; i_row < rows.Length; i_row++)
            {
                var item = rows[i_row].cells[i_cel];
                if (item is TCellCheck checkCell)
                {
                    if (checkCell.Checked != value)
                    {
                        checkCell.Checked = value;
                        SetValue(item, checkCell.Checked);
                        CheckedChanged?.Invoke(this, value, rows[i_row].RECORD, i_row, i_cel);
                    }
                }
            }
        }

        void SetValue(TCell cel, object? value)
        {
            if (cel.PROPERTY == null)
            {
                if (cel.VALUE is AntItem arow) arow.value = value;
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