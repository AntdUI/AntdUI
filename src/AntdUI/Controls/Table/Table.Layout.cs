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
using System.Windows.Forms;

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

        protected override void OnSizeChanged(EventArgs e)
        {
            LoadLayout();
            base.OnSizeChanged(e);
        }

        internal RowTemplate[]? rows = null;
        Rectangle[] dividers = new Rectangle[0], dividerHs = new Rectangle[0];

        void LoadLayout()
        {
            var rect = ChangeLayout();
            if (scrollX.Show) scrollX.SizeChange(new Rectangle(rect.X, rect.Y, rect.Width - 20, rect.Height));
            else scrollX.SizeChange(rect);
            scrollY.SizeChange(rect);
        }
        bool has_check = false;
        Rectangle rect_read, rect_divider;
        Rectangle ChangeLayout()
        {
            has_check = false;
            if (data_temp != null)
            {
                var _rows = new List<RowTemplate>(data_temp.rows.Length);
                var _columns = new List<Column>(data_temp.columns.Length);
                int processing = 0, check_count = 0;
                var col_width = new Dictionary<int, object>();
                if (columns == null) foreach (var it in data_temp.columns) _columns.Add(new Column(it.key, it.key));
                else
                {
                    int x = 0;
                    foreach (var it in columns)
                    {
                        _columns.Add(it);
                        if (it.Width != null)
                        {
                            if (it.Width.EndsWith("%") && float.TryParse(it.Width.TrimEnd('%'), out var f)) col_width.Add(x, f / 100F);
                            else if (int.TryParse(it.Width, out var i)) col_width.Add(x, (int)(i * Config.Dpi));
                        }
                        x++;
                    }
                }

                foreach (var row in data_temp.rows)
                {
                    var cells = new List<TCell>(_columns.Count);
                    foreach (var column in _columns)
                    {
                        var value = row.cells[column.Key];
                        if (value is PropertyDescriptor prop) AddRows(ref cells, ref processing, ref check_count, column, row.record, prop);
                        else cells.Add(new TCellText(this, null, value, column, value?.ToString()));
                    }
                    if (cells.Count > 0) AddRows(ref _rows, cells.ToArray(), row.record);
                }

                if (_rows.Count > 0)
                {
                    #region 添加表头

                    var _cols = new List<TCellColumn>(_columns.Count);
                    CheckState checkState = CheckState.Unchecked;
                    foreach (var it in _columns)
                    {
                        if (check_count > 0 && it is ColumnCheck check)
                        {
                            checkState = check_count == _rows.Count ? CheckState.Checked : CheckState.Indeterminate;
                            check.CheckState = checkState;
                        }
                        _cols.Add(new TCellColumn(this, it));
                    }
                    AddRowsColumn(ref _rows, _cols.ToArray(), dataSource, checkState);

                    #endregion

                    #region 计算坐标

                    float x = 0, y = 0;
                    bool is_exceed = false;

                    var rect = ClientRectangle.PaddingRect(Padding);

                    rect_read.X = rect.X;
                    rect_read.Y = rect.Y;

                    Helper.GDI(g =>
                    {
                        var dpi = Config.Dpi;
                        check_radius = _checksize * 0.25F * dpi;
                        check_border = _checksize * 0.125F * dpi;
                        int check_size = (int)(_checksize * dpi), gap = (int)(_gap * dpi), gap2 = gap * 2,
                        split = (int)(1F * dpi), split2 = split / 2;

                        #region 布局高宽

                        var temp_width_cell = new Dictionary<int, float>();
                        for (int row_i = 0; row_i < _rows.Count; row_i++)
                        {
                            var row = _rows[row_i];
                            row.INDEX = row_i;
                            float max_height = 0;
                            for (int cel_i = 0; cel_i < row.cells.Length; cel_i++)
                            {
                                if (!temp_width_cell.ContainsKey(cel_i)) temp_width_cell.Add(cel_i, 0F);
                                var it = row.cells[cel_i];
                                it.INDEX = cel_i;
                                if (it is TCellCheck check)
                                {
                                    if (max_height < gap2) max_height = gap2;
                                    temp_width_cell[cel_i] = -1F;
                                }
                                else
                                {
                                    var text_size = it.GetSize(g, Font, gap, gap2);
                                    if (max_height < text_size.Height) max_height = text_size.Height;
                                    if (temp_width_cell[cel_i] < text_size.Width) temp_width_cell[cel_i] = text_size.Width;
                                }
                            }
                            row.Height = (int)Math.Round(max_height) + gap2;
                        }

                        int use_width = rect.Width;
                        float max_width = 0;
                        foreach (var it in temp_width_cell)
                        {
                            if (col_width.ContainsKey(it.Key))
                            {
                                var value = col_width[it.Key];
                                if (value is int val_int) max_width += val_int;
                                if (value is float val_float) max_width += rect.Width * val_float;
                            }
                            else if (it.Value == -1F) use_width -= check_size * 2;
                            else max_width += it.Value;
                        }
                        rect_read.Width = rect.Width;
                        rect_read.Height = rect.Height;

                        var width_cell = new Dictionary<int, int>();
                        if (max_width > rect.Width)
                        {
                            is_exceed = true;
                            foreach (var it in temp_width_cell)
                            {
                                if (col_width.ContainsKey(it.Key))
                                {
                                    var value = col_width[it.Key];
                                    if (value is int val_int) width_cell.Add(it.Key, val_int);
                                    else if (value is float val_float) width_cell.Add(it.Key, (int)Math.Ceiling(rect.Width * val_float));
                                }
                                else if (it.Value == -1F)
                                {
                                    int _check_size = check_size * 2;
                                    width_cell.Add(it.Key, _check_size);
                                }
                                else
                                {
                                    int value = (int)Math.Ceiling(it.Value);
                                    width_cell.Add(it.Key, value);
                                }
                            }
                        }
                        else
                        {
                            foreach (var it in temp_width_cell)
                            {
                                if (col_width.ContainsKey(it.Key))
                                {
                                    var value = col_width[it.Key];
                                    if (value is int val_int) width_cell.Add(it.Key, val_int);
                                    else if (value is float val_float) width_cell.Add(it.Key, (int)Math.Ceiling(rect.Width * val_float));
                                }
                                else if (it.Value == -1F)
                                {
                                    int _check_size = check_size * 2;
                                    width_cell.Add(it.Key, _check_size);
                                }
                                else
                                {
                                    int value = (int)Math.Ceiling(use_width * (it.Value / max_width));
                                    width_cell.Add(it.Key, value);
                                }
                            }
                            int sum_wi = 0;
                            foreach (var it in width_cell)
                            {
                                sum_wi += it.Value;
                            }
                            if (rect_read.Width > sum_wi) rect_read.Width = sum_wi;
                        }

                        #endregion

                        #region 最终坐标

                        int use_y = rect.Y;
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
                                else if (it is TCellColumn column)
                                {
                                    it.SetSize(g, Font, _rect, gap, gap2);
                                    if (column.tag is ColumnCheck columnCheck)
                                    {
                                        columnCheck.PARENT = this;
                                        //全选
                                        column.rect = new Rectangle(_rect.X + (_rect.Width - check_size) / 2, _rect.Y + (_rect.Height - check_size) / 2, check_size, check_size);
                                    }
                                    else
                                    {
                                        column.rect = new Rectangle(_rect.X + gap, _rect.Y + gap, _rect.Width - gap2, _rect.Height - gap2);
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

                        var last_row = _rows[_rows.Count - 1];
                        var last = last_row.cells[last_row.cells.Length - 1];

                        if ((rect.Y + rect.Height) > last.RECT.Bottom) rect_read.Height = last.RECT.Bottom - rect.Y;

                        int sp2 = split * 2;
                        rect_divider = new Rectangle(rect_read.X + split, rect_read.Y + split, rect_read.Width - sp2, rect_read.Height - sp2);
                        foreach (var row in _rows)
                        {
                            if (row.IsColumn)
                            {
                                if (bordered)
                                {
                                    for (int i = 0; i < row.cells.Length - 1; i++)
                                    {
                                        var it = (TCellColumn)row.cells[i];
                                        _dividerHs.Add(new Rectangle(it.rect.Right + gap - split2, rect.Y, split, rect_read.Height));
                                    }
                                    _dividers.Add(new Rectangle(rect.X, row.RECT.Bottom - split2, rect_read.Width, split));
                                }
                                else
                                {
                                    for (int i = 0; i < row.cells.Length - 1; i++)
                                    {
                                        var it = (TCellColumn)row.cells[i];
                                        _dividerHs.Add(new Rectangle(it.rect.Right + gap - split2, it.rect.Y, split, it.rect.Height));
                                    }
                                }
                            }
                            else
                            {
                                if (bordered) _dividers.Add(new Rectangle(rect.X, row.RECT.Bottom - split2, rect_read.Width, split));
                                else _dividers.Add(new Rectangle(row.RECT.X, row.RECT.Bottom - split2, row.RECT.Width, split));
                            }
                        }
                        if (bordered) _dividers.RemoveAt(_dividers.Count - 1);
                        dividerHs = _dividerHs.ToArray();
                        dividers = _dividers.ToArray();
                    });

                    rows = _rows.ToArray();

                    #endregion

                    scrollX.SetVrSize(is_exceed ? x : 0, rect.Width);
                    scrollY.SetVrSize(y, rect.Height);

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

                    scrollX.SetVrSize(0, 0);
                    scrollY.SetVrSize(0, 0);
                    dividers = new Rectangle[0];
                    rows = null;
                }
            }
            return Rectangle.Empty;
        }

        #region 动画

        ITask? ThreadState = null;
        internal float AnimationStateValue = 0;

        #endregion

        float check_radius = 0F, check_border = 2F;
        void AddRows(ref List<TCell> cells, ref int processing, ref int check_count, Column column, object ov, PropertyDescriptor prop)
        {
            if (column is ColumnCheck)
            {
                //复选框
                has_check = true;
                var value = prop.GetValue(ov);
                if (value is bool check && check)
                {
                    check_count++;
                    AddRows(ref cells, new TCellCheck(this, prop, ov, true));
                }
                else AddRows(ref cells, new TCellCheck(this, prop, ov, false));
            }
            else if (column is ColumnRadio)
            {
                //单选框
                has_check = true;
                var value = prop.GetValue(ov);
                if (value is bool check && check)
                {
                    check_count++;
                    AddRows(ref cells, new TCellRadio(this, prop, ov, true));
                }
                else AddRows(ref cells, new TCellRadio(this, prop, ov, false));
            }
            else
            {
                var value = prop.GetValue(ov);
                if (value is IList<ICell> icells) AddRows(ref cells, new Template(this, prop, ov, column, ref processing, icells));
                else if (value is ICell icell) AddRows(ref cells, new Template(this, prop, ov, column, ref processing, new ICell[] { icell }));
                else AddRows(ref cells, new TCellText(this, prop, ov, column, value?.ToString()));
            }
        }

        bool handcheck = false;

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
                            if (key == "StateProcessing") LoadLayout();
                            else Invalidate();
                        };
                    }
                    else if (it is TemplateTag tag)
                    {
                        tag.Value.Changed = key =>
                        {
                            Invalidate();
                        };
                    }
                    else if (it is TemplateImage image)
                    {
                        image.Value.Changed = key =>
                        {
                            Invalidate();
                        };
                    }
                    else if (it is TemplateButton link)
                    {
                        link.Value.Changed = key =>
                        {
                            Invalidate();
                        };
                    }
                    else if (it is TemplateText text)
                    {
                        text.Value.Changed = key =>
                        {
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

        private void Notify_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (sender == null || e.PropertyName == null) return;
            PropertyChanged(sender, e.PropertyName);
        }

        void PropertyChanged(object data, string key)
        {
            if (rows == null || handcheck || key == null) return;
            if (columns == null)
            {
                for (int cel = 0; cel < rows[0].cells.Length; cel++)
                {
                    TCellColumn _cell = (TCellColumn)rows[0].cells[cel];
                    if (key == _cell.value)
                    {
                        foreach (var row in rows)
                        {
                            if (row.RECORD == data)
                            {
                                var temp = row.cells[cel];
                                ValueChanged(temp, data);
                                return;
                            }
                        }
                        return;
                    }
                }
            }
            else
            {
                #region 判断复选框

                for (int cel = 0; cel < columns.Length; cel++)
                {
                    var it = columns[cel];
                    if (it.Key == key)
                    {
                        if (it is ColumnCheck check)
                        {
                            int check_count = 0;
                            for (int i = 1; i < rows.Length; i++)
                            {
                                foreach (var cell in rows[i].cells)
                                {
                                    if (cell is TCellCheck checkCell)
                                    {
                                        if (checkCell.Checked) check_count++;
                                        break;
                                    }
                                }
                            }
                            if (rows.Length - 1 == check_count)
                            {
                                //全选
                                rows[0].CheckState = CheckState.Checked;
                            }
                            else if (check_count > 0) rows[0].CheckState = CheckState.Indeterminate;
                            else rows[0].CheckState = CheckState.Unchecked;
                            check.SetCheckState(rows[0].CheckState);
                        }
                        else
                        {
                            foreach (var row in rows)
                            {
                                if (row.RECORD == data)
                                {
                                    var temp = row.cells[cel];
                                    ValueChanged(temp, data);
                                    return;
                                }
                            }
                        }
                        return;
                    }
                }

                #endregion
            }
        }
        void ValueChanged(TCell temp, object data)
        {
            if (temp.PROPERTY != null)
            {
                if (temp is Template template)
                {
                    foreach (ITemplate it in template.value)
                    {
                        if (it is TemplateText text)
                        {
                            var value = temp.PROPERTY.GetValue(data);
                            if (value is CellText text2) text.Value = text2;
                            else text.Value.Text = value?.ToString();
                            return;
                        }
                    }
                }
                else if (temp is TCellText text)
                {
                    var value = temp.PROPERTY.GetValue(data);
                    text.value = value?.ToString();
                    Invalidate();
                }
                else LoadLayout();
            }
        }
        void AddRows(ref List<RowTemplate> rows, TCell[] cells, object Record)
        {
            var row = new RowTemplate(this, cells, Record);
            foreach (var it in row.cells) it.ROW = row;
            rows.Add(row);
        }
        void AddRowsColumn(ref List<RowTemplate> rows, TCell[] cells, object Record, CheckState check)
        {
            var row = new RowTemplate(this, cells, Record, check);
            foreach (var it in row.cells) it.ROW = row;
            rows.Insert(0, row);
        }
    }
}