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
using System.Data;
using System.Drawing;
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
                    var item = _row.cells[column];
                    EditModeClose();
                    if (CanEditMode(_row, item))
                    {
                        ScrollLine(row, rows);
                        if (showFixedColumnL && fixedColumnL != null && fixedColumnL.Contains(column)) OnEditMode(_row, item, row, column, item.COLUMN, 0, ScrollBar.ValueY);
                        else if (showFixedColumnR && fixedColumnR != null && fixedColumnR.Contains(column)) OnEditMode(_row, item, row, column, item.COLUMN, sFixedR, ScrollBar.ValueY);
                        else OnEditMode(_row, item, row, column, item.COLUMN, ScrollBar.ValueX, ScrollBar.ValueY);
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
            if (inEditMode)
            {
                ScrollBar.OnInvalidate = null;
                if (!focused)
                {
                    if (InvokeRequired) Invoke(Focus);
                    else Focus();
                }
                inEditMode = false;
            }
        }

        bool CanEditMode(RowTemplate it, CELL cell)
        {
            if (rows == null) return false;
            if (cell.COLUMN.Editable)
            {
                if (cell is TCellText cellText) return true;
                else if (cell is Template templates)
                {
                    foreach (var template in templates.Value)
                    {
                        if (template is CellText text) return true;
                    }
                }
            }
            return false;
        }
        void OnEditMode(RowTemplate it, CELL cell, int i_row, int i_col, Column? column, int sx, int sy)
        {
            if (rows == null) return;
            if (it.AnimationHover)
            {
                it.ThreadHover?.Dispose();
                it.ThreadHover = null;
            }
            bool multiline = cell.COLUMN.LineBreak;
            if (cell is TCellText cellText)
            {
                object? value = null;
                if (cell.PROPERTY != null && cell.VALUE != null) value = cell.PROPERTY.GetValue(cell.VALUE);
                else if (cell.VALUE is AntItem item) value = item.value;
                else value = cell.VALUE;

                bool isok = true;
                if (CellBeginEdit != null) isok = CellBeginEdit(this, new TableEventArgs(value, it.RECORD, i_row, i_col, column));
                if (!isok) return;
                inEditMode = true;

                ScrollBar.OnInvalidate = () => EditModeClose();
                BeginInvoke(() =>
                {
                    for (int i = 0; i < rows.Length; i++) rows[i].hover = i == i_row;
                    int height = EditInputHeight(value, cell);
                    var tmp_input = CreateInput(cell, sx, sy, height, multiline, value);
                    if (cellText.COLUMN.Align == ColumnAlign.Center) tmp_input.TextAlign = HorizontalAlignment.Center;
                    else if (cellText.COLUMN.Align == ColumnAlign.Right) tmp_input.TextAlign = HorizontalAlignment.Right;
                    var arge = new TableBeginEditInputStyleEventArgs(value, it.RECORD, i_row, i_col, column, tmp_input);
                    CellBeginEditInputStyle?.Invoke(this, arge);
                    ShowInput(arge.Input, (cf, _value) =>
                    {
                        var e = new TableEndEditEventArgs(_value, it.RECORD, i_row, i_col, column);
                        arge.Call?.Invoke(e);
                        bool isok_end = CellEndEdit?.Invoke(this, e) ?? true;
                        if (isok_end && !cf)
                        {
                            if (GetValue(value, _value, out var o))
                            {
                                cellText.value = _value;
                                if (it.RECORD is DataRow datarow)
                                {
                                    cellText.VALUE = cellText.value = _value;
                                    datarow[i_col] = o;
                                }
                                else SetValue(cell, o);
                                if (multiline) LoadLayout();
                            }
                            CellEditComplete?.Invoke(this, new ITableEventArgs(it.RECORD, i_row, i_col, column));
                        }
                    });
                    Controls.Add(arge.Input);
                    arge.Input.Focus();
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
                        bool isok = true;
                        if (CellBeginEdit != null) isok = CellBeginEdit(this, new TableEventArgs(value, it.RECORD, i_row, i_col, column));
                        if (!isok) return;
                        inEditMode = true;

                        ScrollBar.OnInvalidate = () => EditModeClose();
                        BeginInvoke(() =>
                        {
                            for (int i = 0; i < rows.Length; i++) rows[i].hover = i == i_row;
                            int height = EditInputHeight(value, cell);
                            var tmp_input = CreateInput(cell, sx, sy, height, multiline, value);
                            if (template.PARENT.COLUMN.Align == ColumnAlign.Center) tmp_input.TextAlign = HorizontalAlignment.Center;
                            else if (template.PARENT.COLUMN.Align == ColumnAlign.Right) tmp_input.TextAlign = HorizontalAlignment.Right;
                            var arge = new TableBeginEditInputStyleEventArgs(value, it.RECORD, i_row, i_col, column, tmp_input);
                            CellBeginEditInputStyle?.Invoke(this, arge);
                            ShowInput(arge.Input, (cf, _value) =>
                            {
                                var e = new TableEndEditEventArgs(_value, it.RECORD, i_row, i_col, column);
                                arge.Call?.Invoke(e);
                                bool isok_end = CellEndEdit?.Invoke(this, e) ?? true;
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
                                        if (GetValue(value, _value, out var o))
                                        {
                                            if (it.RECORD is DataRow datarow) datarow[i_col] = o;
                                            else SetValue(cell, o);
                                        }
                                    }
                                    CellEditComplete?.Invoke(this, new ITableEventArgs(it.RECORD, i_row, i_col, column));
                                }
                            });
                            Controls.Add(arge.Input);
                            arge.Input.Focus();
                        });
                        return;
                    }
                }
            }
        }

        int EditInputHeight(object? value, CELL cell)
        {
            if (cell.COLUMN.LineBreak) return cell.RECT.Height;
            else
            {
                int gap = (int)(Math.Max(_gap, 8) * Config.Dpi), height2 = cell.RECT_REAL.Height + gap,
                    height_real = Helper.GDI(g => g.MeasureString(value?.ToString(), Font).Height + gap);
                return height_real > height2 ? height_real : height2;
            }
        }

        bool GetValue(object? value, string _value, out object read)
        {
            if (value is int)
            {
                if (int.TryParse(_value, out var v))
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
            else
            {
                read = _value;
                return true;
            }
            read = _value;
            return false;
        }

        Input CreateInput(CELL cell, int sx, int sy, int height, bool multiline, object? value)
        {
            if (value is CellText text2)
            {
                return new Input
                {
                    Multiline = multiline,
                    Location = new Point(cell.RECT.X - sx, cell.RECT.Y - sy + (cell.RECT.Height - height) / 2),
                    Size = new Size(cell.RECT.Width, height),
                    Text = text2.Text ?? ""
                };
            }
            else
            {
                return new Input
                {
                    Multiline = multiline,
                    Location = new Point(cell.RECT.X - sx, cell.RECT.Y - sy + (cell.RECT.Height - height) / 2),
                    Size = new Size(cell.RECT.Width, height),
                    Text = value?.ToString() ?? ""
                };
            }
        }
        void ShowInput(Input input, Action<bool, string> call)
        {
            string old_text = input.Text;
            bool isone = true;
            input.KeyPress += (a, b) =>
            {
                if (a is Input input && isone)
                {
                    if (b.KeyChar == 13)
                    {
                        isone = false;
                        b.Handled = true;
                        ScrollBar.OnInvalidate = null;
                        call(old_text == input.Text, input.Text);
                        inEditMode = false;
                        input.Dispose();
                    }
                }
            };
            input.LostFocus += (a, b) =>
            {
                if (a is Input input && isone)
                {
                    isone = false;
                    input.Visible = false;
                    ScrollBar.OnInvalidate = null;
                    call(old_text == input.Text, input.Text);
                    inEditMode = false;
                    Focus();
                    if (Modal.ModalCount > 0)
                    {
                        ITask.Run(() =>
                        {
                            System.Threading.Thread.Sleep(200);
                            BeginInvoke(() => input.Dispose());
                        });
                        return;
                    }
                    input.Dispose();
                }
            };
        }

        #endregion
    }
}