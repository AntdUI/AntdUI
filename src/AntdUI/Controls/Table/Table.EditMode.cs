// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AntdUI
{
    partial class Table
    {
        #region 编辑模式

        /// <summary>
        /// 进入编辑模式
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="column">列</param>
        public bool EnterEditMode(int row, int column)
        {
            if (rows != null)
            {
                try
                {
                    var _row = rows[row];
                    if (_row == null) return false;
                    var item = RealCELL(_row.cells[column], rows.List, row, column, out var crect);
                    EditModeClose();
                    if (CanEditMode(item))
                    {
                        ScrollLine(crect, RealRegionRow(rows.List));
                        UpdateFixedColumnState();
                        OnEditMode(_row, item, crect, row, column, item.COLUMN, FixedColumnOffsetX(column), ScrollBarRealY, !IsFixedEditColumn(column));
                        return true;
                    }
                }
                catch { }
            }
            return false;
        }

        bool inEditMode = false;

        /// <summary>
        /// 行级编辑会话进行中（一行多个单元格同时处于编辑状态）
        /// </summary>
        internal bool rowEditing = false;

        /// <summary>
        /// 进入行编辑模式：一行多个单元格同时进入编辑状态
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="columns">列索引集合</param>
        public bool EnterEditMode(int row, params int[] columns)
        {
            if (rows == null || columns == null || columns.Length == 0) return false;
            var _row = rows[row];
            if (_row == null) return false;
            EditModeClose();
            bool ok = false;
            foreach (int column in columns.Distinct())
            {
                try
                {
                    var item = RealCELL(_row.cells[column], rows.List, row, column, out var crect);
                    if (!CanEditMode(item)) continue;
                    if (!ok) ScrollLine(crect, RealRegionRow(rows.List));
                    UpdateFixedColumnState();
                    OnEditMode(_row, item, crect, row, column, item.COLUMN, FixedColumnOffsetX(column), ScrollBarRealY, !IsFixedEditColumn(column));
                    ok = true;
                }
                catch { }
            }
            if (ok)
            {
                rowEditing = true;
                // 排在全部创建 BeginInvoke 之后（FIFO），聚焦首个编辑框（进入编辑从第一列开始填写的直觉）
                BeginInvoke(() =>
                {
                    _editControls.Keys.OrderBy(EditOrderKey).FirstOrDefault()?.Focus();
                });
            }
            return ok;
        }

        /// <summary>
        /// 关闭编辑模式
        /// </summary>
        public void EditModeClose()
        {
            rowEditing = false;
            if (inEditMode || _editControls.Count > 0)
            {
                var ids = _editControls.Keys.ToArray();
                foreach (var id in ids)
                {
                    if (_editControls.TryRemove(id, out var obj))
                    {
                        id.LostFocus -= InputEdit_LostFocus;
                        if (id is Select select)
                        {
                            select.SelectedValueChanged -= InputEdit_SelectedValueChanged;
                            select.ClosedItem -= InputEdit_SelectedValueChanged;
                            obj.call?.Invoke(obj.value == select.SelectedValue, select.SelectedValue);
                        }
                        else
                        {
                            id.KeyPress -= InputEdit_KeyPress;
                            obj.call?.Invoke(((string)obj.value!) == id.Text, id.Text);
                        }
                        obj.call = null;
                        Focus();
                        id.Dispose();
                        Controls.Remove(id);
                    }
                }
                inEditMode = false;
            }
        }

        /// <summary>
        /// 关闭指定单元格的编辑控件（行级编辑模式下单个单元格提交）
        /// </summary>
        /// <param name="input">编辑输入框</param>
        public void EditModeClose(Input input)
        {
            if (input == null) return;
            if (_editControls.TryRemove(input, out var obj))
            {
                if (_currentEdit != null && !rowEditing) _currentEdit = null;
                input.LostFocus -= InputEdit_LostFocus;
                if (input is Select select)
                {
                    select.SelectedValueChanged -= InputEdit_SelectedValueChanged;
                    select.ClosedItem -= InputEdit_SelectedValueChanged;
                    obj.call?.Invoke(obj.value == select.SelectedValue, select.SelectedValue);
                }
                else
                {
                    input.KeyPress -= InputEdit_KeyPress;
                    obj.call?.Invoke(((string)obj.value!) == input.Text, input.Text);
                }
                obj.call = null;
                if (_editControls.Count == 0)
                {
                    rowEditing = false;
                    inEditMode = false;
                    Focus();
                }
                input.Dispose();
                Controls.Remove(input);
            }
        }

        /// <summary>
        /// 行级编辑：把指定编辑控件的当前值写回数据对象，但保留控件以便继续修改
        /// </summary>
        /// <param name="input">编辑输入框</param>
        internal void EditModeCommit(Input input)
        {
            if (input == null) return;
            if (_editControls.TryGetValue(input, out var obj))
            {
                if (input is Select select) obj.call?.Invoke(obj.value == select.SelectedValue, select.SelectedValue);
                else obj.call?.Invoke(((string)obj.value!) == input.Text, input.Text);
            }
        }

        /// <summary>
        /// 行级编辑：把全部编辑控件的当前值写回数据对象（保留控件），供保存前取值
        /// </summary>
        internal void EditModeCommitAll()
        {
            if (_editControls.Count == 0) return;
            var ids = _editControls.Keys.ToArray();
            foreach (var id in ids)
            {
                if (!_editControls.TryGetValue(id, out var obj)) continue;
                if (id is Select select) obj.call?.Invoke(obj.value == select.SelectedValue, select.SelectedValue);
                else obj.call?.Invoke(((string)obj.value!) == id.Text, id.Text);
            }
        }

        /// <summary>
        /// 行级编辑：按 Name 中的行列数字排序（避免 edit_10_x 与 edit_2_x 的字典序错乱）
        /// </summary>
        static long EditOrderKey(Input input)
        {
            var parts = (input.Name ?? string.Empty).Split('_');
            if (parts.Length == 3 && int.TryParse(parts[1], out var col) && int.TryParse(parts[2], out var row)) return row * 100000L + col;
            return long.MaxValue;
        }

        /// <summary>
        /// 获取指定列索引当前活跃的行级编辑控件（无则返回 null）
        /// </summary>
        internal Input? EditModeInputAtColumn(int column)
        {
            foreach (var kv in _editControls)
            {
                if (kv.Value.ColumnIndex == column) return kv.Key;
            }
            return null;
        }

        bool CanEditMode(CELL cell)
        {
            if (rows == null) return false;
            if (cell.COLUMN.Editable)
            {
                if (cell is TCellText) return true;
                else if (cell is TCellSelect) return true;
                else if (cell is Template templates)
                {
                    foreach (var template in templates.Value)
                    {
                        if (template is CellText) return true;
                    }
                }
            }
            return false;
        }
        void OnEditMode(RowTemplate it, CELL cell, Rectangle rect, int i_row, int i_col, Column column, int sx, int sy, bool region)
        {
            if (rows == null) return;
            if (it.AnimationHover)
            {
                it.ThreadHover?.Dispose();
                it.ThreadHover = null;
            }

            // 存储当前编辑的单元格信息
            _currentEdit = new TableCellEditEnterEventArgs(it.RECORD, i_row, i_col, column);

            bool multiline = column.LineBreak;
            if (column is ColumnSelect columnSelect)
            {
                object? value = null, val = null;
                if (cell is TCellSelect cellSelect)
                {
                    value = cellSelect.value?.Tag;
                    val = cellSelect.value;
                }
                bool isok = OnCellBeginEdit(value, it.RECORD, i_row, i_col, column);
                if (!isok) return;
                inEditMode = true;

                BeginInvoke(() =>
                {
                    rows.SetHover(i_row);
                    var tmp_input = CreateInput(cell, sx, sy, multiline, val, rect, out bool defaultRect, out float borwidth, out int wavesize);
                    tmp_input.Name = "edit_" + i_col + "_" + i_row;
                    if (column.Align == ColumnAlign.Center) tmp_input.TextAlign = HorizontalAlignment.Center;
                    else if (column.Align == ColumnAlign.Right) tmp_input.TextAlign = HorizontalAlignment.Right;
                    var arge = new TableBeginEditInputStyleEventArgs(value, it.RECORD, i_row, i_col, column, tmp_input);
                    OnCellBeginEditInputStyle(arge);
                    // 行级编辑依赖 Name 定位跳格顺序、TextAlign 对齐；单格编辑不改动用户替换的控件
                    if (rowEditing)
                    {
                        if (string.IsNullOrEmpty(arge.Input.Name)) arge.Input.Name = tmp_input.Name;
                        arge.Input.TextAlign = tmp_input.TextAlign;
                    }
                    if (arge.Input is Select select)
                    {
                        ShowSelect(select, region, column.INDEX_REAL, (echo, r_value) =>
                        {
                            if (!echo && OnCellEndValueEdit(r_value, it.RECORD, i_row, i_col, column))
                            {
                                if (cell is TCellSelect cellSelect)
                                {
                                    cellSelect!.value = cellSelect.COLUMN[r_value];
                                    SetValue(cell, r_value);
                                    if (multiline) LoadLayout();
                                }
                                else
                                {
                                    SetValue(cell, r_value);
                                    LoadLayout();
                                }
                                OnCellEditComplete(it.RECORD, i_row, i_col, column);
                            }
                        });
                        if (_editControls.TryGetValue(arge.Input, out var tagSelect)) tagSelect.SetEditInfo(i_row, i_col, region, defaultRect, borwidth, wavesize);
                    }
                    else arge.Input.Dispose();
                });
            }
            else if (cell is TCellText cellText)
            {
                object? value = null;
                if (cell.PROPERTY != null && cell.VALUE != null) value = cell.PROPERTY.GetValue(cell.VALUE);
                else if (cell.VALUE is AntItem item) value = item.value;
                else value = cell.VALUE;

                bool isok = OnCellBeginEdit(value, it.RECORD, i_row, i_col, column);
                if (!isok) return;
                inEditMode = true;

                BeginInvoke(() =>
                {
                    rows.SetHover(i_row);
                    var tmp_input = CreateInput(cell, sx, sy, multiline, value, rect, out bool defaultRect, out float borwidth, out int wavesize);
                    tmp_input.Name = "edit_" + i_col + "_" + i_row;
                    if (column.Align == ColumnAlign.Center) tmp_input.TextAlign = HorizontalAlignment.Center;
                    else if (column.Align == ColumnAlign.Right) tmp_input.TextAlign = HorizontalAlignment.Right;
                    var arge = new TableBeginEditInputStyleEventArgs(value, it.RECORD, i_row, i_col, column, tmp_input);
                    OnCellBeginEditInputStyle(arge);
                    // 行级编辑依赖 Name 定位跳格顺序、TextAlign 对齐；单格编辑不改动用户替换的控件
                    if (rowEditing)
                    {
                        if (string.IsNullOrEmpty(arge.Input.Name)) arge.Input.Name = tmp_input.Name;
                        arge.Input.TextAlign = tmp_input.TextAlign;
                    }
                    ShowInput(arge.Input, region, column.INDEX_REAL, (echo, r_value) =>
                    {
                        if (r_value is string val_str)
                        {
                            arge.Call?.Invoke(new TableEndEditEventArgs(val_str, it.RECORD, i_row, i_col, column));
                            if (!echo && OnCellEndEdit(val_str, it.RECORD, i_row, i_col, column))
                            {
                                if (GetValue(value, val_str, cell.PROPERTY, out var o))
                                {
                                    cellText.SetValue(o);
                                    SetValue(cell, o);
                                    if (multiline) LoadLayout();
                                }
                                OnCellEditComplete(it.RECORD, i_row, i_col, column);
                            }
                        }
                        else
                        {
                            if (!echo && OnCellEndValueEdit(r_value, it.RECORD, i_row, i_col, column))
                            {
                                SetValue(cell, r_value);
                                LoadLayout();
                                OnCellEditComplete(it.RECORD, i_row, i_col, column);
                            }
                        }
                    });
                    if (_editControls.TryGetValue(arge.Input, out var tagText)) tagText.SetEditInfo(i_row, i_col, region, defaultRect, borwidth, wavesize);
                });
            }
            else if (cell is Template templates)
            {
                foreach (var template in templates.Value)
                {
                    if (template is CellText text)
                    {
                        object? value = null;
                        if (cell.PROPERTY != null && cell.VALUE != null) value = cell.PROPERTY.GetValue(cell.VALUE);
                        else if (cell.VALUE is AntItem item) value = item.value;
                        else value = cell.VALUE;
                        bool isok = OnCellBeginEdit(value, it.RECORD, i_row, i_col, column);
                        if (!isok) return;
                        inEditMode = true;

                        BeginInvoke(() =>
                        {
                            rows.SetHover(i_row);
                            var tmp_input = CreateInput(cell, sx, sy, multiline, value, rect, out bool defaultRect, out float borwidth, out int wavesize);
                            tmp_input.Name = "edit_" + i_col + "_" + i_row;
                            if (column.Align == ColumnAlign.Center) tmp_input.TextAlign = HorizontalAlignment.Center;
                            else if (column.Align == ColumnAlign.Right) tmp_input.TextAlign = HorizontalAlignment.Right;
                            var arge = new TableBeginEditInputStyleEventArgs(value, it.RECORD, i_row, i_col, column, tmp_input);
                            OnCellBeginEditInputStyle(arge);
                            // 行级编辑依赖 Name 定位跳格顺序、TextAlign 对齐；单格编辑不改动用户替换的控件
                            if (rowEditing)
                            {
                                if (string.IsNullOrEmpty(arge.Input.Name)) arge.Input.Name = tmp_input.Name;
                                arge.Input.TextAlign = tmp_input.TextAlign;
                            }
                            ShowInput(arge.Input, region, column.INDEX_REAL, (echo, r_value) =>
                            {
                                if (r_value is string val_str)
                                {
                                    arge.Call?.Invoke(new TableEndEditEventArgs(val_str, it.RECORD, i_row, i_col, column));
                                    if (!echo && OnCellEndEdit(val_str, it.RECORD, i_row, i_col, column))
                                    {
                                        if (value is CellText text2)
                                        {
                                            text2.Text = val_str;
                                            SetValue(cell, text2);
                                        }
                                        else
                                        {
                                            text.Text = val_str;
                                            if (GetValue(value, val_str, cell.PROPERTY, out var o)) SetValue(cell, o);
                                        }
                                        OnCellEditComplete(it.RECORD, i_row, i_col, column);
                                    }
                                }
                                else
                                {
                                    if (!echo && OnCellEndValueEdit(r_value, it.RECORD, i_row, i_col, column))
                                    {
                                        SetValue(cell, r_value);
                                        LoadLayout();
                                        OnCellEditComplete(it.RECORD, i_row, i_col, column);
                                    }
                                }
                            });
                            if (_editControls.TryGetValue(arge.Input, out var tagText)) tagText.SetEditInfo(i_row, i_col, region, defaultRect, borwidth, wavesize);
                        });
                        return;
                    }
                }
            }
        }

        bool GetValue(object? value, string _value, System.ComponentModel.PropertyDescriptor? Property, out object? read)
        {
            if (Property == null)
            {
                read = GetValue(value, _value);
                return true;
            }
            else
            {
                try
                {
                    read = Property.Converter.ConvertFrom(_value);
                    return true;
                }
                catch
                {
                    if (Property.PropertyType.IsGenericType && Property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        read = GetValue(value, _value);
                        return true;
                    }
                    else return GetValue(value, _value, out read);
                }
            }
        }
        object? GetValue(object? value, string _value)
        {
            if (value is int?)
            {
                if (int.TryParse(_value, out var v)) return v;
                return null;
            }
            else if (value is uint?)
            {
                if (uint.TryParse(_value, out var v)) return v;
                return null;
            }
            else if (value is short?)
            {
                if (short.TryParse(_value, out var v)) return v;
                return null;
            }
            else if (value is ushort?)
            {
                if (ushort.TryParse(_value, out var v)) return v;
                return null;
            }
            else if (value is long?)
            {
                if (long.TryParse(_value, out var v)) return v;
                return null;
            }
            else if (value is ulong?)
            {
                if (ulong.TryParse(_value, out var v)) return v;
                return null;
            }
            else if (value is double?)
            {
                if (double.TryParse(_value, out var v)) return v;
                return null;
            }
            else if (value is decimal?)
            {
                if (decimal.TryParse(_value, out var v)) return v;
                return null;
            }
            else if (value is float?)
            {
                if (float.TryParse(_value, out var v)) return v;
                return null;
            }
            else if (value is DateTime?)
            {
                if (DateTime.TryParse(_value, out var v)) return v;
                return null;
            }
            else return _value;
        }
        bool GetValue(object? value, string _value, out object? read)
        {
            if (value is int)
            {
                if (int.TryParse(_value, out var v))
                {
                    read = v;
                    return true;
                }
            }
            else if (value is uint)
            {
                if (uint.TryParse(_value, out var v))
                {
                    read = v;
                    return true;
                }
            }
            else if (value is short)
            {
                if (short.TryParse(_value, out var v))
                {
                    read = v;
                    return true;
                }
            }
            else if (value is ushort)
            {
                if (ushort.TryParse(_value, out var v))
                {
                    read = v;
                    return true;
                }
            }
            else if (value is long)
            {
                if (long.TryParse(_value, out var v))
                {
                    read = v;
                    return true;
                }
            }
            else if (value is ulong)
            {
                if (ulong.TryParse(_value, out var v))
                {
                    read = v;
                    return true;
                }
            }
            else if (value is double)
            {
                if (double.TryParse(_value, out var v))
                {
                    read = v;
                    return true;
                }
            }
            else if (value is decimal)
            {
                if (decimal.TryParse(_value, out var v))
                {
                    read = v;
                    return true;
                }
            }
            else if (value is float)
            {
                if (float.TryParse(_value, out var v))
                {
                    read = v;
                    return true;
                }
            }
            else if (value is DateTime)
            {
                if (DateTime.TryParse(_value, out var v))
                {
                    read = v;
                    return true;
                }
            }
            else
            {
                read = _value;
                return true;
            }
            read = null;
            return false;
        }

        Input CreateInput(CELL cell, int sx, int sy, bool multiline, object? value, Rectangle rect, out bool defaultRect, out float borwidth, out int wavesize)
        {
            switch (EditInputStyle)
            {
                case TEditInputStyle.Full:
                    defaultRect = false; borwidth = 1F; wavesize = 4;
                    var inputFull = CreateInput(multiline, value, cell.COLUMN, RectInput(cell, rect, sx, sy, borwidth, wavesize));
                    inputFull.Radius = 0;
                    return inputFull;
                case TEditInputStyle.Excel:
                    defaultRect = false; borwidth = 2.5F; wavesize = 0;
                    var inputExcel = CreateInput(multiline, value, cell.COLUMN, RectInput(cell, rect, sx, sy, borwidth, wavesize));
                    inputExcel.WaveSize = 0;
                    inputExcel.Radius = 0;
                    inputExcel.BorderWidth = 2.5F;
                    return inputExcel;
                case TEditInputStyle.Default:
                default:
                    defaultRect = true; borwidth = 1F; wavesize = 4;
                    return CreateInput(multiline, value, cell.COLUMN, RectInputDefault(cell, rect, sx, sy, borwidth, wavesize));
            }
        }

        Rectangle RectInput(CELL cell, Rectangle rect, int sx, int sy, float borwidth, int wavesize)
        {
            int bor = (int)((wavesize + borwidth / 2F) * Dpi), bor2 = bor * 2, ry = rect.Y, rh = rect.Height;
            if (EditAutoHeight)
            {
                int texth = this.GDI(g => g.MeasureString(Config.NullText, Font).Height), sps = (int)(texth * .4F), sps2 = sps * 2, h = texth + sps2 + bor2;
                if (h > rect.Height)
                {
                    rh = h - bor2;
                    ry = rect.Y + (rect.Height - rh) / 2;
                    if ((ry + h) - sy > rect_read.Bottom) ry = rect_read.Bottom + sy - rh - bor;
                }
            }
            return new Rectangle(rect.X - sx - bor, ry - sy - bor, rect.Width + bor2, rh + bor2);
        }
        Rectangle RectInputDefault(CELL cell, Rectangle rect, int sx, int sy, float borwidth, int wavesize)
        {
            int bor = (int)((wavesize + borwidth / 2F) * Dpi), bor2 = bor * 2, ry = rect.Y, rh = rect.Height;
            if (EditAutoHeight)
            {
                int texth = this.GDI(g => g.MeasureString(Config.NullText, Font).Height), sps = (int)(texth * .4F), sps2 = sps * 2, h = texth + sps2 + bor2;
                if (h > rect.Height)
                {
                    rh = h;
                    ry = rect.Y + (rect.Height - rh) / 2;
                    if ((ry + h) - sy > rect_read.Bottom) ry = rect_read.Bottom + sy - rh;
                }
            }
            return new Rectangle(rect.X - sx, ry - sy, rect.Width, rh);
        }
        Input CreateInput(bool multiline, object? value, Column column, Rectangle rect)
        {
            Input input;
            if (column is ColumnSelect columnSelect)
            {
                var edit = CreateInputBySelect(columnSelect);
                edit.Multiline = multiline;
                edit.IconRatio = 1f;
                edit.ReadOnly = column.ReadOnly;
                edit.Bounds = rect;
                if (value is SelectItem select)
                {
                    edit.Text = select.Text;
                    edit.SelectedValue = select.Tag;
                    edit.ShowIcon = select.Icon != null || select.IconSvg != null;
                }
                input = edit;
                edit.SelectedValue = value;
            }
            else if (value is CellText text)
            {
                input = new Input
                {
                    AcceptsEscape = true,
                    Multiline = multiline,
                    Bounds = rect,
                    Text = text.Text ?? "",
                    ReadOnly = column.ReadOnly
                };
            }
            else
            {
                input = new Input
                {
                    AcceptsEscape = true,
                    Multiline = multiline,
                    Bounds = rect,
                    Text = column.GetDisplayText(value) ?? string.Empty,
                    ReadOnly = column.ReadOnly
                };
            }
            if (input.ReadOnly) input.BackColor = Colour.BorderSecondary.Get(nameof(Table));
            switch (EditSelection)
            {
                case TEditSelection.First:
                    input.SelectionStart = input.SelectionLength = 0;
                    break;
                case TEditSelection.Last:
                    input.SelectLast();
                    break;
                case TEditSelection.All:
                    input.SelectAll();
                    break;
            }
            return input;
        }
        internal Select CreateInputBySelect(ColumnSelect column)
        {
            var edit = new Select
            {
                AcceptsEscape = true,
                List = true
            };
            if (column.MaxCount.HasValue) edit.MaxCount = column.MaxCount.Value;
            edit.Items.AddRange(column.Items.ToArray());
            return edit;
        }
        void ShowInput(Input input, bool region, int column, Action<bool, object?> call)
        {
            if (input is Select select)
            {
                ShowSelect(select, region, column, call);
                return;
            }
            if (AddEditInput(input, input.Text, call, region, column))
            {
                input.KeyPress += InputEdit_KeyPress;
                // 行级编辑由 EnterEditMode 在全部创建完成后统一聚焦首框，创建过程中不逐个转移焦点
                if (!rowEditing) input.Focus();
            }
        }

        void ShowSelect(Select select, bool region, int column, Action<bool, object?> call)
        {
            if (AddEditInput(select, select.SelectedValue, call, region, column))
            {
                select.SelectedValueChanged += InputEdit_SelectedValueChanged;
                select.ClosedItem += InputEdit_SelectedValueChanged;
                if (!rowEditing) select.Focus();
            }
        }

        void InputEdit_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (sender is Input input)
            {
                if (e.KeyChar == 13)
                {
                    e.Handled = true;
                    if (rowEditing)
                    {
                        // 行级编辑：写回当前值并跳到下一格，控件保留可继续编辑
                        EditModeCommit(input);
                        var next = _editControls.Keys.Where(i => i != input).OrderBy(EditOrderKey).FirstOrDefault();
                        if (next != null) next.Focus();
                        return;
                    }
                    // 使用存储的单元格信息触发 CellEnter 事件
                    EditModeClose();
                    if (_currentEdit == null) return;
                    OnCellEditEnter(sender, _currentEdit);
                }
            }
        }
        void InputEdit_SelectedValueChanged(object? sender, ObjectNEventArgs e)
        {
            if (rowEditing && sender is Input input)
            {
                // 行级编辑：写回选中值，控件保留可再次修改
                EditModeCommit(input);
                return;
            }
            EditModeClose();
        }
        void InputEdit_LostFocus(object? sender, EventArgs e)
        {
            if (rowEditing)
            {
                // 行级编辑：失焦写回当前值，控件保留（退出仅由保存/取消触发）
                if (sender is Input input) EditModeCommit(input);
                return;
            }
            if (EditLostFocus) EditModeClose();
        }

        #endregion

        #region 集合处理

        ConcurrentDictionary<Input, EditControlTag> _editControls = new ConcurrentDictionary<Input, EditControlTag>();
        class EditControlTag
        {
            public EditControlTag(object? v, Action<bool, object?> c)
            {
                value = v;
                call = c;
            }
            public object? value { get; set; }
            public Action<bool, object?>? call { get; set; }

            /// <summary>编辑单元格所在行索引（行级编辑时用于滚动重算位置）</summary>
            public int RowIndex { get; private set; } = -1;
            /// <summary>编辑单元格所在列索引（行级编辑时用于滚动重算位置）</summary>
            public int ColumnIndex { get; private set; } = -1;
            /// <summary>是否已应用固定列裁剪区域</summary>
            public bool Region { get; private set; }
            /// <summary>默认矩形布局（否则为 Full/Excel 布局）</summary>
            public bool DefaultRect { get; private set; }
            /// <summary>边框宽度（布局参数）</summary>
            public float BorWidth { get; private set; } = 1F;
            /// <summary>波浪大小（布局参数）</summary>
            public int WaveSize { get; private set; } = 4;

            internal void SetEditInfo(int i_row, int i_col, bool region, bool defaultRect, float borwidth, int wavesize)
            {
                RowIndex = i_row;
                ColumnIndex = i_col;
                Region = region;
                DefaultRect = defaultRect;
                BorWidth = borwidth;
                WaveSize = wavesize;
            }
        }

        // 存储当前编辑的单元格信息
        private TableCellEditEnterEventArgs? _currentEdit;

        /// <summary>
        /// 添加空间到编辑
        /// </summary>
        bool AddEditInput(Input input, object? txt, Action<bool, object?> action, bool region, int column)
        {
            if (_editControls.TryAdd(input, new EditControlTag(txt, action)))
            {
                if (region) EditInputRegion(input, column);
                Controls.Add(input);
                // 强制首帧同步绘制：批量浮起多个编辑控件时，特定页面容器时序下首个 WM_PAINT 可能丢失
                input.Refresh();
                if (OS.Win7OrLower) return true;
                input.LostFocus += InputEdit_LostFocus;
                return true;
            }
            else input.Dispose();
            return false;
        }
        void EditInputRegion(Input input, int col)
        {
            if (showFixedColumnR && rect_fixed_R.HasValue && showFixedColumnL && rect_fixed_L.HasValue)
            {
                if (fixedColumnL!.Contains(col) || fixedColumnR!.Contains(col)) return;
                input.Region = EditInputRegion(input.Bounds, rect_fixed_R.Value, rect_fixed_L.Value);
            }
            else if (showFixedColumnR && rect_fixed_R.HasValue)
            {
                if (fixedColumnR!.Contains(col)) return;
                input.Region = EditInputRegion(input.Bounds, rect_fixed_R.Value);
            }
            else if (showFixedColumnL && rect_fixed_L.HasValue)
            {
                if (fixedColumnL!.Contains(col)) return;
                input.Region = EditInputRegion(input.Bounds, rect_fixed_L.Value);
            }
        }
        Region? EditInputRegion(Rectangle rect, params Rectangle[] rect_fixeds)
        {
            Region? newRegion = null;
            foreach (var it in rect_fixeds)
            {
                if (rect.IntersectsWith(it))
                {
                    Rectangle intersect = Rectangle.Intersect(rect, it);
                    newRegion ??= new Region(new Rectangle(0, 0, rect.Width, rect.Height));
                    newRegion.Exclude(new Rectangle(intersect.X - rect.X, intersect.Y - rect.Y, intersect.Width, intersect.Height));
                }
            }
            return newRegion;
        }

        /// <summary>
        /// 行级编辑时重算全部编辑控件位置（滚动/尺寸变化后输入框跟随单元格）
        /// </summary>
        internal void EditModeReposition()
        {
            if (rows == null || _editControls.Count == 0) return;
            UpdateFixedColumnState();
            var ids = _editControls.Keys.ToArray();
            foreach (var id in ids)
            {
                if (!_editControls.TryGetValue(id, out var obj)) continue;
                if (obj.RowIndex < 0 || obj.ColumnIndex < 0) continue;
                try
                {
                    var _row = rows[obj.RowIndex];
                    if (_row == null) continue;
                    var item = RealCELL(_row.cells[obj.ColumnIndex], rows.List, obj.RowIndex, obj.ColumnIndex, out var crect);
                    int sx = FixedColumnOffsetX(obj.ColumnIndex);
                    int sy = ScrollBarRealY;
                    if (obj.DefaultRect) id.Bounds = RectInputDefault(item, crect, sx, sy, obj.BorWidth, obj.WaveSize);
                    else id.Bounds = RectInput(item, crect, sx, sy, obj.BorWidth, obj.WaveSize);
                    if (obj.Region) EditInputRegion(id, item.COLUMN.INDEX_REAL);
                }
                catch { }
            }
        }

        /// <summary>
        /// 悬浮固定列状态（showFixedColumnL/R、sFixedR、rect_fixed_L/R）在绘制时才更新，
        /// 滚动事件里读到的是旧值会导致编辑控件错位，这里按 PaintFixedColumnL/R 的同样规则现算
        /// </summary>
        void UpdateFixedColumnState()
        {
            if (rows == null || rows.List.Length == 0) return;
            var lastRow = rows.List[rows.List.Length - 1];
            int sx = ScrollBar.ValueX;
            try
            {
                if (fixedColumnL != null && sx > 0)
                {
                    showFixedColumnL = true;
                    var last = lastRow.cells[fixedColumnL[fixedColumnL.Count - 1]];
                    rect_fixed_L = new Rectangle(rect_read.X, rect_read.Y, last.RECT.Right, rect_read.Height);
                }
                else
                {
                    showFixedColumnL = false;
                    rect_fixed_L = null;
                }
                if (fixedColumnR != null)
                {
                    int scrollBar = ScrollBar.ShowY ? ScrollBar.SIZE : 0, rectR = rect_read.Right - scrollBar;
                    CELL first = lastRow.cells[fixedColumnR[fixedColumnR.Count - 1]], last = lastRow.cells[fixedColumnR[0]];
                    if (sx + rectR < last.RECT.Right)
                    {
                        showFixedColumnR = true;
                        sFixedR = last.RECT.Right - rectR;
                        int w = last.RECT.Right - first.RECT.Left + scrollBar;
                        rect_fixed_R = new Rectangle(rect_read.Right - w, rect_read.Y, w, rect_read.Height);
                    }
                    else
                    {
                        showFixedColumnR = false;
                        rect_fixed_R = null;
                    }
                }
                else
                {
                    showFixedColumnR = false;
                    rect_fixed_R = null;
                }
            }
            catch { }
        }

        /// <summary>
        /// 单元格编辑控件的横向偏移：固定左列为 0（RECT 即屏幕坐标），悬浮固定右列为 sFixedR，其余为滚动值
        /// </summary>
        int FixedColumnOffsetX(int column)
        {
            if (fixedColumnL != null && fixedColumnL.Contains(column)) return 0;
            if (showFixedColumnR && fixedColumnR != null && fixedColumnR.Contains(column)) return sFixedR;
            return ScrollBar.ValueX;
        }

        /// <summary>
        /// 是否固定列（编辑控件无需滚动跟随，也无需按固定列区域裁剪）
        /// </summary>
        bool IsFixedEditColumn(int column)
        {
            if (fixedColumnL != null && fixedColumnL.Contains(column)) return true;
            if (showFixedColumnR && fixedColumnR != null && fixedColumnR.Contains(column)) return true;
            return false;
        }

        #endregion
    }
}