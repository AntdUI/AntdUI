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
                        if (showFixedColumnL && fixedColumnL != null && fixedColumnL.Contains(column)) OnEditMode(_row, item, crect, row, column, item.COLUMN, 0, ScrollBar.ValueY, false);
                        else if (showFixedColumnR && fixedColumnR != null && fixedColumnR.Contains(column)) OnEditMode(_row, item, crect, row, column, item.COLUMN, sFixedR, ScrollBar.ValueY, false);
                        else OnEditMode(_row, item, crect, row, column, item.COLUMN, ScrollBar.ValueX, ScrollBar.ValueY, true);
                        return true;
                    }
                }
                catch { }
            }
            return false;
        }

        bool inEditMode = false;
        /// <summary>
        /// 关闭编辑模式
        /// </summary>
        public void EditModeClose()
        {
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
                            if (obj[1] is Action<bool, object?> call)
                            {
                                obj[1] = null;
                                call(obj[0] == select.SelectedValue, select.SelectedValue);
                            }
                        }
                        else id.KeyPress -= InputEdit_KeyPress;

                        if (obj[1] != null)
                        {
                            if (obj[1] is Action<bool, string> call)
                            {
                                obj[1] = null;
                                call(((string)obj[0]!) == id.Text, id.Text);
                            }
                        }
                        Focus();
                        id.Dispose();
                        Controls.Remove(id);
                    }
                }
                inEditMode = false;
            }
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
                    var tmp_input = CreateInput(cell, sx, sy, multiline, val, rect);
                    tmp_input.Name = "edit_" + i_col + "_" + i_row;
                    if (column.Align == ColumnAlign.Center) tmp_input.TextAlign = HorizontalAlignment.Center;
                    else if (column.Align == ColumnAlign.Right) tmp_input.TextAlign = HorizontalAlignment.Right;
                    var arge = new TableBeginEditInputStyleEventArgs(value, it.RECORD, i_row, i_col, column, tmp_input);
                    OnCellBeginEditInputStyle(arge);
                    if (arge.Input is Select select)
                    {
                        ShowSelect(select, region, column.INDEX_REAL, (cf, _value) =>
                        {
                            bool isok_end = OnCellEndValueEdit(_value, it.RECORD, i_row, i_col, column);
                            if (isok_end && !cf)
                            {
                                if (cell is TCellSelect cellSelect)
                                {
                                    cellSelect!.value = cellSelect.COLUMN[_value];
                                    SetValue(cell, _value);
                                    if (multiline) LoadLayout();
                                }
                                else
                                {
                                    SetValue(cell, _value);
                                    LoadLayout();
                                }
                                OnCellEditComplete(it.RECORD, i_row, i_col, column);
                            }
                        });
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
                    var tmp_input = CreateInput(cell, sx, sy, multiline, value, rect);
                    tmp_input.Name = "edit_" + i_col + "_" + i_row;
                    if (column.Align == ColumnAlign.Center) tmp_input.TextAlign = HorizontalAlignment.Center;
                    else if (column.Align == ColumnAlign.Right) tmp_input.TextAlign = HorizontalAlignment.Right;
                    var arge = new TableBeginEditInputStyleEventArgs(value, it.RECORD, i_row, i_col, column, tmp_input);
                    OnCellBeginEditInputStyle(arge);
                    ShowInput(arge.Input, region, column.INDEX_REAL, (cf, _value) =>
                    {
                        arge.Call?.Invoke(new TableEndEditEventArgs(_value, it.RECORD, i_row, i_col, column));
                        bool isok_end = OnCellEndEdit(_value, it.RECORD, i_row, i_col, column);
                        if (isok_end && !cf)
                        {
                            if (GetValue(value, _value, cell.PROPERTY, out var o))
                            {
                                cellText.SetValue(o);
                                SetValue(cell, o);
                                if (multiline) LoadLayout();
                            }
                            OnCellEditComplete(it.RECORD, i_row, i_col, column);
                        }
                    });
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
                            var tmp_input = CreateInput(cell, sx, sy, multiline, value, rect);
                            tmp_input.Name = "edit_" + i_col + "_" + i_row;
                            if (column.Align == ColumnAlign.Center) tmp_input.TextAlign = HorizontalAlignment.Center;
                            else if (column.Align == ColumnAlign.Right) tmp_input.TextAlign = HorizontalAlignment.Right;
                            var arge = new TableBeginEditInputStyleEventArgs(value, it.RECORD, i_row, i_col, column, tmp_input);
                            OnCellBeginEditInputStyle(arge);
                            ShowInput(arge.Input, region, column.INDEX_REAL, (cf, _value) =>
                            {
                                arge.Call?.Invoke(new TableEndEditEventArgs(_value, it.RECORD, i_row, i_col, column));
                                bool isok_end = OnCellEndEdit(_value, it.RECORD, i_row, i_col, column);
                                if (isok_end && !cf)
                                {
                                    if (value is CellText text2)
                                    {
                                        text2.Text = _value;
                                        SetValue(cell, text2);
                                    }
                                    else
                                    {
                                        text.Text = _value;
                                        if (GetValue(value, _value, cell.PROPERTY, out var o)) SetValue(cell, o);
                                    }
                                    OnCellEditComplete(it.RECORD, i_row, i_col, column);
                                }
                            });
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

        Input CreateInput(CELL cell, int sx, int sy, bool multiline, object? value, Rectangle rect)
        {
            switch (EditInputStyle)
            {
                case TEditInputStyle.Full:
                    var inputFull = CreateInput(multiline, value, cell.COLUMN, RectInput(cell, rect, sx, sy, 1F, 4));
                    inputFull.Radius = 0;
                    return inputFull;
                case TEditInputStyle.Excel:
                    var inputExcel = CreateInput(multiline, value, cell.COLUMN, RectInput(cell, rect, sx, sy, 2.5F, 0));
                    inputExcel.WaveSize = 0;
                    inputExcel.Radius = 0;
                    inputExcel.BorderWidth = 2.5F;
                    return inputExcel;
                case TEditInputStyle.Default:
                default:
                    return CreateInput(multiline, value, cell.COLUMN, RectInputDefault(cell, rect, sx, sy, 1F, 4));
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
            Select edit = new Select
            {
                List = true
            };
            if (column.MaxCount.HasValue) edit.MaxCount = column.MaxCount.Value;
            edit.Items.AddRange(column.Items.ToArray());
            return edit;
        }
        void ShowInput(Input input, bool region, int column, Action<bool, string> call)
        {
            if (AddEditInput(input, input.Text, call, region, column))
            {
                input.KeyPress += InputEdit_KeyPress;
                input.Focus();
            }
        }

        void ShowSelect(Select select, bool region, int column, Action<bool, object?> call)
        {
            if (AddEditInput(select, select.SelectedValue, call, region, column))
            {
                select.SelectedValueChanged += InputEdit_SelectedValueChanged;
                select.ClosedItem += InputEdit_SelectedValueChanged;
                select.Focus();
            }
        }

        void InputEdit_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (sender is Input input)
            {
                if (e.KeyChar == 13)
                {
                    e.Handled = true;
                    // 使用存储的单元格信息触发 CellEnter 事件
                    EditModeClose();
                    if (_currentEdit == null) return;
                    OnCellEditEnter(sender, _currentEdit);
                }
            }
        }
        void InputEdit_SelectedValueChanged(object? sender, ObjectNEventArgs e) => EditModeClose();
        void InputEdit_LostFocus(object? sender, EventArgs e)
        {
            if (EditLostFocus) EditModeClose();
        }

        #endregion

        #region 集合处理

        ConcurrentDictionary<Input, object?[]> _editControls = new ConcurrentDictionary<Input, object?[]>();

        // 存储当前编辑的单元格信息
        private TableCellEditEnterEventArgs? _currentEdit;

        /// <summary>
        /// 添加空间到编辑
        /// </summary>
        bool AddEditInput(Input input, object? txt, object? action, bool region, int column)
        {
            if (_editControls.TryAdd(input, new object?[] { txt, action }))
            {
                if (region) EditInputRegion(input, column);
                Controls.Add(input);
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

        #endregion
    }
}