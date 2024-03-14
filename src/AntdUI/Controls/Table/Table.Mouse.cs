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
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace AntdUI
{
    partial class Table
    {
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (rows == null || CellClick == null) return;
            int sx = (int)scrollX.Value, sy = (int)scrollY.Value;
            int px = e.X + sx, py = e.Y + sy;
            for (int i_row = 0; i_row < rows.Length; i_row++)
            {
                var it = rows[i_row];
                if (it.Contains(e.X, py))
                {
                    if (showFixedColumnL && fixedColumnL != null)
                    {
                        foreach (var i_col in fixedColumnL)
                        {
                            var item = it.cells[i_col];
                            if (item.CONTAINS(e.X, py))
                            {
                                CellClick.Invoke(this, e, it.RECORD, i_row, i_col, new Rectangle(item.RECT.X, item.RECT.Y - sy, item.RECT.Width, item.RECT.Height));
                                return;
                            }
                        }
                    }
                    if (showFixedColumnR && fixedColumnR != null)
                    {
                        foreach (var i_col in fixedColumnR)
                        {
                            var item = it.cells[i_col];
                            if (item.CONTAINS(e.X + sFixedR, py))
                            {
                                CellClick.Invoke(this, e, it.RECORD, i_row, i_col, new Rectangle(item.RECT.X - sFixedR, item.RECT.Y - sy, item.RECT.Width, item.RECT.Height));
                                return;
                            }
                        }
                    }
                    for (int i_col = 0; i_col < it.cells.Length; i_col++)
                    {
                        var item = it.cells[i_col];
                        if (item.CONTAINS(px, py))
                        {
                            CellClick.Invoke(this, e, it.RECORD, i_row, i_col, new Rectangle(item.RECT.X - sx, item.RECT.Y - sy, item.RECT.Width, item.RECT.Height));
                            return;
                        }
                    }
                    return;
                }
            }
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            if (rows == null) return;
            int sx = (int)scrollX.Value, sy = (int)scrollY.Value;
            int px = e.X + sx, py = e.Y + sy;
            for (int i_row = 0; i_row < rows.Length; i_row++)
            {
                var it = rows[i_row];
                if (it.Contains(e.X, py))
                {
                    if (showFixedColumnL && fixedColumnL != null)
                    {
                        foreach (var i_col in fixedColumnL)
                        {
                            var item = it.cells[i_col];
                            if (item.CONTAINS(e.X, py))
                            {
                                if (editmode == TEditMode.DoubleClick)
                                {
                                    //进入编辑模式
                                    EditModeClose();
                                    OnEditMode(it, item, i_row, i_col, 0, sy);
                                }
                                CellDoubleClick?.Invoke(this, e, it.RECORD, i_row, i_col, new Rectangle(item.RECT.X, item.RECT.Y - sy, item.RECT.Width, item.RECT.Height));
                                return;
                            }
                        }
                    }
                    if (showFixedColumnR && fixedColumnR != null)
                    {
                        foreach (var i_col in fixedColumnR)
                        {
                            var item = it.cells[i_col];
                            if (item.CONTAINS(e.X + sFixedR, py))
                            {
                                if (editmode == TEditMode.DoubleClick)
                                {
                                    //进入编辑模式
                                    EditModeClose();
                                    OnEditMode(it, item, i_row, i_col, sFixedR, sy);
                                }
                                CellDoubleClick?.Invoke(this, e, it.RECORD, i_row, i_col, new Rectangle(item.RECT.X - sFixedR, item.RECT.Y - sy, item.RECT.Width, item.RECT.Height));
                                return;
                            }
                        }
                    }
                    for (int i_col = 0; i_col < it.cells.Length; i_col++)
                    {
                        var item = it.cells[i_col];
                        if (item.CONTAINS(px, py))
                        {
                            if (editmode == TEditMode.DoubleClick)
                            {
                                //进入编辑模式
                                EditModeClose();
                                OnEditMode(it, item, i_row, i_col, sx, sy);
                            }
                            CellDoubleClick?.Invoke(this, e, it.RECORD, i_row, i_col, new Rectangle(item.RECT.X - sx, item.RECT.Y - sy, item.RECT.Width, item.RECT.Height));
                            return;
                        }
                    }
                    return;
                }
            }
        }

        #region 鼠标按下

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (scrollY.MouseDown(e.Location) && scrollX.MouseDown(e.Location))
            {
                base.OnMouseDown(e);
                if (rows == null) return;
                int px = e.X + (int)scrollX.Value, py = e.Y + (int)scrollY.Value;
                for (int i_row = 0; i_row < rows.Length; i_row++)
                {
                    var it = rows[i_row];
                    if (it.IsColumn)
                    {
                        if (fixedHeader)
                        {
                            if (it.CONTAINS(e.X, e.Y))
                            {
                                if (has_check)
                                {
                                    if (showFixedColumnL && fixedColumnL != null)
                                    {
                                        foreach (var i in fixedColumnL)
                                        {
                                            if (MouseDownRowColumn(e, it, (TCellColumn)it.cells[i], rows, e.X, e.Y)) return;
                                        }
                                    }
                                    if (showFixedColumnR && fixedColumnR != null)
                                    {
                                        foreach (var i in fixedColumnR)
                                        {
                                            if (MouseDownRowColumn(e, it, (TCellColumn)it.cells[i], rows, e.X + sFixedR, e.Y)) return;
                                        }
                                    }
                                    foreach (TCellColumn item in it.cells)
                                    {
                                        if (MouseDownRowColumn(e, it, item, rows, px, e.Y)) return;
                                    }
                                }
                                return;
                            }
                        }
                        else if (it.CONTAINS(e.X, py))
                        {
                            if (has_check)
                            {
                                if (showFixedColumnL && fixedColumnL != null)
                                {
                                    foreach (var i in fixedColumnL)
                                    {
                                        if (MouseDownRowColumn(e, it, (TCellColumn)it.cells[i], rows, e.X, py)) return;
                                    }
                                }
                                if (showFixedColumnR && fixedColumnR != null)
                                {
                                    foreach (var i in fixedColumnR)
                                    {
                                        if (MouseDownRowColumn(e, it, (TCellColumn)it.cells[i], rows, e.X + sFixedR, py)) return;
                                    }
                                }
                                foreach (TCellColumn item in it.cells)
                                {
                                    if (MouseDownRowColumn(e, it, item, rows, px, py)) return;
                                }
                            }
                            return;
                        }
                    }
                    else if (it.Contains(e.X, py))
                    {
                        if (showFixedColumnL && fixedColumnL != null)
                        {
                            foreach (var i in fixedColumnL) if (MouseDownRow(e, it.cells[i], e.X, py)) return;
                        }
                        if (showFixedColumnR && fixedColumnR != null)
                        {
                            foreach (var i in fixedColumnR) if (MouseDownRow(e, it.cells[i], e.X + sFixedR, py)) return;
                        }
                        for (int i_col = 0; i_col < it.cells.Length; i_col++) if (MouseDownRow(e, it.cells[i_col], px, py)) return;
                        return;
                    }
                }
            }
        }

        bool MouseDownRowColumn(MouseEventArgs e, RowTemplate it, TCellColumn cell, RowTemplate[] rows, int x, int y)
        {
            if (cell.tag is ColumnCheck columnCheck)
            {
                if (e.Button == MouseButtons.Left && cell.Contains(x, y))
                {
                    ChangeCheckOverall(rows, it, columnCheck, !columnCheck.Checked);
                    return true;
                }
            }
            return false;
        }
        bool MouseDownRow(MouseEventArgs e, TCell cell, int x, int y)
        {
            if (cell.CONTAINS(x, y))
            {
                cell.MouseDown = true;
                if (cell is Template template && template.HasBtn && e.Button == MouseButtons.Left)
                {
                    foreach (var item in template.value)
                    {
                        if (item is TemplateButton btn_template)
                        {
                            if (btn_template.Value.Enabled)
                            {
                                if (btn_template.RECT.Contains(x, y))
                                {
                                    btn_template.ExtraMouseDown = true;
                                    return true;
                                }
                            }
                        }
                    }
                }
                return true;
            }
            return false;
        }

        #endregion

        #region 鼠标松开

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            scrollY.MouseUp(e.Location);
            scrollX.MouseUp(e.Location);
            if (rows == null) return;
            int sx = (int)scrollX.Value, sy = (int)scrollY.Value;
            int px = e.X + sx, py = e.Y + sy;
            for (int i_row = 0; i_row < rows.Length; i_row++)
            {
                var it = rows[i_row];
                if (showFixedColumnL && fixedColumnL != null)
                {
                    foreach (var i in fixedColumnL) if (MouseUpRow(it, it.cells[i], e, i_row, i, e.X, py, 0, sy)) return;
                }
                if (showFixedColumnR && fixedColumnR != null)
                {
                    foreach (var i in fixedColumnR) if (MouseUpRow(it, it.cells[i], e, i_row, i, e.X + sFixedR, py, sFixedR, sy)) return;
                }
                for (int i_col = 0; i_col < it.cells.Length; i_col++)
                {
                    var item = it.cells[i_col];
                    if (MouseUpRow(it, item, e, i_row, i_col, px, py, sx, sy)) return;
                }
            }
        }

        Input? edit_input = null;
        bool MouseUpRow(RowTemplate it, TCell cell, MouseEventArgs e, int i_row, int i_col, int x, int y, int sx, int sy)
        {
            if (cell.MouseDown)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (cell is TCellCheck checkCell)
                    {
                        if (checkCell.Contains(x, y))
                        {
                            checkCell.Checked = !checkCell.Checked;
                            it.Checked = checkCell.Checked;
                            cell.PROPERTY?.SetValue(cell.VALUE, checkCell.Checked);
                            CheckedChanged?.Invoke(this, checkCell.Checked, it.RECORD, i_row, i_col);
                        }
                    }
                    else if (cell is TCellRadio radioCell)
                    {
                        if (radioCell.Contains(x, y) && !radioCell.Checked)
                        {
                            if (rows != null)
                            {
                                for (int i = 0; i < rows.Length; i++)
                                {
                                    if (i != i_row)
                                    {
                                        var cell2 = rows[i].cells[i_col];
                                        if (cell2 is TCellRadio radioCell2 && radioCell2.Checked)
                                        {
                                            radioCell2.Checked = false;
                                            cell2.PROPERTY?.SetValue(cell2.VALUE, radioCell2.Checked);
                                        }
                                    }
                                }
                            }
                            radioCell.Checked = true;
                            cell.PROPERTY?.SetValue(cell.VALUE, radioCell.Checked);
                            CheckedChanged?.Invoke(this, radioCell.Checked, it.RECORD, i_row, i_col);
                        }
                    }
                    else if (cell is Template template && template.HasBtn)
                    {
                        foreach (var item in template.value)
                        {
                            if (item is TemplateButton btn)
                            {
                                if (btn.ExtraMouseDown)
                                {
                                    if (btn.RECT.Contains(x, y))
                                    {
                                        if (btn.Value is CellButton) btn.Click();
                                        CellButtonClick?.Invoke(this, btn.Value, e, it.RECORD, i_row, i_col);
                                    }
                                    btn.ExtraMouseDown = false;
                                }
                            }
                        }
                    }
                    else if (editmode == TEditMode.Click)
                    {
                        //进入编辑模式
                        EditModeClose();
                        OnEditMode(it, cell, i_row, i_col, sx, sy);
                    }
                }
                cell.MouseDown = false;
                return true;
            }
            return false;
        }

        bool inEditMode = false;
        void EditModeClose()
        {
            if (edit_input != null)
            {
                scrollX.Change = scrollY.Change = null;
                edit_input?.Dispose();
                edit_input = null;
            }
            inEditMode = false;
        }
        void OnEditMode(RowTemplate it, TCell cell, int i_row, int i_col, int sx, int sy)
        {
            if (rows == null) return;
            if (cell is TCellText cellText)
            {
                object? value = null;
                if (cell.PROPERTY != null && cell.VALUE != null) value = cell.PROPERTY.GetValue(cell.VALUE);
                else value = cell.VALUE;

                bool isok = true;
                if (CellBeginEdit != null) isok = CellBeginEdit(this, value, it.RECORD, i_row, i_col);
                if (!isok) return;
                inEditMode = true;

                scrollX.Change = scrollY.Change = () => EditModeClose();
                BeginInvoke(new Action(() =>
                {
                    for (int i = 0; i < rows.Length; i++)
                    {
                        rows[i].hover = i == i_row;
                    }
                    int height = Helper.GDI(g =>
                    {
                        return height = (int)Math.Ceiling(g.MeasureString(Config.NullText, Font).Height * 1.66F);
                    });
                    if (value is int val_int)
                    {
                        edit_input = new InputNumber
                        {
                            BackColor = Color.Transparent,
                            Location = new Point(cell.RECT.X - sx, cell.RECT.Y - sy + (cell.RECT.Height - height) / 2),
                            Size = new Size(cell.RECT.Width, height),
                            Value = val_int
                        };
                    }
                    else if (value is double val_double)
                    {
                        edit_input = new InputNumber
                        {
                            BackColor = Color.Transparent,
                            Location = new Point(cell.RECT.X - sx, cell.RECT.Y - sy + (cell.RECT.Height - height) / 2),
                            Size = new Size(cell.RECT.Width, height),
                            Value = new decimal(val_double)
                        };
                    }
                    else if (value is float val_float)
                    {
                        edit_input = new InputNumber
                        {
                            BackColor = Color.Transparent,
                            Location = new Point(cell.RECT.X - sx, cell.RECT.Y - sy + (cell.RECT.Height - height) / 2),
                            Size = new Size(cell.RECT.Width, height),
                            Value = new decimal(val_float)
                        };
                    }
                    else
                    {
                        edit_input = new Input
                        {
                            BackColor = Color.Transparent,
                            Location = new Point(cell.RECT.X - sx, cell.RECT.Y - sy + (cell.RECT.Height - height) / 2),
                            Size = new Size(cell.RECT.Width, height),
                            Text = value?.ToString() ?? ""
                        };
                    }
                    edit_input.KeyPress += (a, b) =>
                    {
                        if (b.KeyChar == 13 && a is Input input)
                        {
                            b.Handled = true;
                            bool isok_end = true;
                            if (CellEndEdit != null) isok_end = CellEndEdit(this, input.Text, it.RECORD, i_row, i_col);
                            if (isok_end)
                            {
                                cellText.value = edit_input.Text;
                                if (cell.PROPERTY != null)
                                {
                                    if (value is int)
                                    {
                                        if (int.TryParse(edit_input.Text, out var value)) cell.PROPERTY.SetValue(cell.VALUE, value);
                                    }
                                    else if (value is double)
                                    {
                                        if (double.TryParse(edit_input.Text, out var value)) cell.PROPERTY.SetValue(cell.VALUE, value);
                                    }
                                    else if (value is float)
                                    {
                                        if (float.TryParse(edit_input.Text, out var value)) cell.PROPERTY.SetValue(cell.VALUE, value);
                                    }
                                    else cell.PROPERTY.SetValue(cell.VALUE, edit_input.Text);
                                }
                                else if (it.RECORD is DataRow datarow)
                                {
                                    if (value is int)
                                    {
                                        if (int.TryParse(edit_input.Text, out var value)) datarow[i_col] = value;
                                    }
                                    else if (value is double)
                                    {
                                        if (double.TryParse(edit_input.Text, out var value)) datarow[i_col] = value;
                                    }
                                    else if (value is float)
                                    {
                                        if (float.TryParse(edit_input.Text, out var value)) datarow[i_col] = value;
                                    }
                                    else datarow[i_col] = edit_input.Text;
                                }
                                EditModeClose();
                            }
                        }
                    };
                    edit_input.LostFocus += (a, b) =>
                    {
                        EditModeClose();
                    };
                    Controls.Add(edit_input);
                }));
            }
            else if (cell is Template templates)
            {
                foreach (ITemplate template in templates.value)
                {
                    if (template is TemplateText text)
                    {
                        object? value = null;
                        if (cell.PROPERTY != null && cell.VALUE != null) value = cell.PROPERTY.GetValue(cell.VALUE);
                        else value = cell.VALUE;

                        //if (value is CellText text2) value=text2.Text;

                        bool isok = true;
                        if (CellBeginEdit != null) isok = CellBeginEdit(this, value, it.RECORD, i_row, i_col);
                        if (!isok) return;
                        inEditMode = true;

                        scrollX.Change = scrollY.Change = () => EditModeClose();
                        BeginInvoke(new Action(() =>
                        {
                            for (int i = 0; i < rows.Length; i++)
                            {
                                rows[i].hover = i == i_row;
                            }
                            int height = Helper.GDI(g =>
                            {
                                return height = (int)Math.Ceiling(g.MeasureString(Config.NullText, Font).Height * 1.66F);
                            });
                            if (value is int val_int)
                            {
                                edit_input = new InputNumber
                                {
                                    BackColor = Color.Transparent,
                                    Location = new Point(cell.RECT.X - sx, cell.RECT.Y - sy + (cell.RECT.Height - height) / 2),
                                    Size = new Size(cell.RECT.Width, height),
                                    Value = val_int
                                };
                            }
                            else if (value is double val_double)
                            {
                                edit_input = new InputNumber
                                {
                                    BackColor = Color.Transparent,
                                    Location = new Point(cell.RECT.X - sx, cell.RECT.Y - sy + (cell.RECT.Height - height) / 2),
                                    Size = new Size(cell.RECT.Width, height),
                                    Value = new decimal(val_double)
                                };
                            }
                            else if (value is float val_float)
                            {
                                edit_input = new InputNumber
                                {
                                    BackColor = Color.Transparent,
                                    Location = new Point(cell.RECT.X - sx, cell.RECT.Y - sy + (cell.RECT.Height - height) / 2),
                                    Size = new Size(cell.RECT.Width, height),
                                    Value = new decimal(val_float)
                                };
                            }
                            else
                            {
                                if (value is CellText text2)
                                {
                                    edit_input = new Input
                                    {
                                        BackColor = Color.Transparent,
                                        Location = new Point(cell.RECT.X - sx, cell.RECT.Y - sy + (cell.RECT.Height - height) / 2),
                                        Size = new Size(cell.RECT.Width, height),
                                        Text = text2.Text ?? ""
                                    };
                                }
                                else
                                {
                                    edit_input = new Input
                                    {
                                        BackColor = Color.Transparent,
                                        Location = new Point(cell.RECT.X - sx, cell.RECT.Y - sy + (cell.RECT.Height - height) / 2),
                                        Size = new Size(cell.RECT.Width, height),
                                        Text = value?.ToString() ?? ""
                                    };
                                }
                            }
                            edit_input.KeyPress += (a, b) =>
                            {
                                if (b.KeyChar == 13 && a is Input input)
                                {
                                    b.Handled = true;
                                    bool isok_end = true;
                                    if (CellEndEdit != null) isok_end = CellEndEdit(this, input.Text, it.RECORD, i_row, i_col);
                                    if (isok_end)
                                    {
                                        if (value is CellText text2)
                                        {
                                            text2.Text = edit_input.Text;
                                            if (cell.PROPERTY != null) cell.PROPERTY.SetValue(cell.VALUE, text2);
                                        }
                                        else
                                        {
                                            text.Value.Text = edit_input.Text;
                                            if (cell.PROPERTY != null)
                                            {
                                                if (value is int)
                                                {
                                                    if (int.TryParse(edit_input.Text, out var value)) cell.PROPERTY.SetValue(cell.VALUE, value);
                                                }
                                                else if (value is double)
                                                {
                                                    if (double.TryParse(edit_input.Text, out var value)) cell.PROPERTY.SetValue(cell.VALUE, value);
                                                }
                                                else if (value is float)
                                                {
                                                    if (float.TryParse(edit_input.Text, out var value)) cell.PROPERTY.SetValue(cell.VALUE, value);
                                                }
                                                else cell.PROPERTY.SetValue(cell.VALUE, edit_input.Text);
                                            }
                                            else if (it.RECORD is DataRow datarow)
                                            {
                                                if (value is int)
                                                {
                                                    if (int.TryParse(edit_input.Text, out var value)) datarow[i_col] = value;
                                                }
                                                else if (value is double)
                                                {
                                                    if (double.TryParse(edit_input.Text, out var value)) datarow[i_col] = value;
                                                }
                                                else if (value is float)
                                                {
                                                    if (float.TryParse(edit_input.Text, out var value)) datarow[i_col] = value;
                                                }
                                                else datarow[i_col] = edit_input.Text;
                                            }
                                        }
                                        EditModeClose();
                                    }
                                }
                            };
                            edit_input.LostFocus += (a, b) =>
                            {
                                EditModeClose();
                            };
                            Controls.Add(edit_input);
                        }));
                        return;
                    }
                }
            }
        }

        #endregion

        #region 鼠标移动

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (scrollY.MouseMove(e.Location) && scrollX.MouseMove(e.Location))
            {
                if (rows == null || inEditMode) return;
                int hand = 0;
                int px = e.X + (int)scrollX.Value, py = e.Y + (int)scrollY.Value;
                foreach (RowTemplate it in rows)
                {
                    if (it.IsColumn)
                    {
                        if (fixedHeader)
                        {
                            if (it.CONTAINS(e.X, e.Y))
                            {
                                if (has_check)
                                {
                                    if (showFixedColumnL && fixedColumnL != null)
                                    {
                                        foreach (var i in fixedColumnL) MouseMoveRowColumn((TCellColumn)it.cells[i], ref hand, e.X, e.Y);
                                    }
                                    if (showFixedColumnR && fixedColumnR != null)
                                    {
                                        foreach (var i in fixedColumnR) MouseMoveRowColumn((TCellColumn)it.cells[i], ref hand, e.X + sFixedR, e.Y);
                                    }
                                    foreach (TCellColumn item in it.cells) MouseMoveRowColumn(item, ref hand, px, e.Y);
                                }
                                for (int i = 1; i < rows.Length; i++) rows[i].Hover = false;
                                SetCursor(hand > 0);
                                return;
                            }
                        }
                        else if (it.CONTAINS(e.X, py))
                        {
                            if (has_check)
                            {
                                if (showFixedColumnL && fixedColumnL != null)
                                {
                                    foreach (var i in fixedColumnL) MouseMoveRowColumn((TCellColumn)it.cells[i], ref hand, e.X, py);
                                }
                                if (showFixedColumnR && fixedColumnR != null)
                                {
                                    foreach (var i in fixedColumnR) MouseMoveRowColumn((TCellColumn)it.cells[i], ref hand, e.X + sFixedR, py);
                                }
                                foreach (TCellColumn item in it.cells) MouseMoveRowColumn(item, ref hand, px, py);
                            }
                            for (int i = 1; i < rows.Length; i++) rows[i].Hover = false;
                            SetCursor(hand > 0);
                            return;
                        }
                    }
                    else if (it.Contains(e.X, py))
                    {
                        var hasi = new List<int>();
                        if (showFixedColumnL && fixedColumnL != null)
                        {
                            foreach (var i in fixedColumnL)
                            {
                                hasi.Add(i);
                                MouseMoveRow(it.cells[i], ref hand, e.X, py);
                            }
                        }
                        if (showFixedColumnR && fixedColumnR != null)
                        {
                            foreach (var i in fixedColumnR)
                            {
                                hasi.Add(i);
                                MouseMoveRow(it.cells[i], ref hand, e.X + sFixedR, py);
                            }
                        }
                        for (int i = 0; i < it.cells.Length; i++)
                        {
                            if (hasi.Contains(i)) continue;
                            MouseMoveRow(it.cells[i], ref hand, px, py);
                        }
                    }
                    else
                    {
                        foreach (var cel in it.cells)
                        {
                            if (cel is Template template && template.HasBtn)
                            {
                                foreach (var item in template.value)
                                {
                                    if (item is TemplateButton btn) btn.ExtraMouseHover = false;
                                }
                            }
                        }
                    }
                }
                SetCursor(hand > 0);
            }
            else ILeave();
        }

        void MouseMoveRowColumn(TCellColumn cel, ref int hand, int x, int y)
        {
            if (cel.tag is ColumnCheck)
            {
                if (cel.Contains(x, y)) hand++;
            }
        }
        void MouseMoveRow(TCell cel, ref int hand, int x, int y)
        {
            if (cel is TCellCheck checkCell)
            {
                if (checkCell.Contains(x, y)) hand++;
            }
            else if (cel is TCellRadio radioCell)
            {
                if (radioCell.Contains(x, y)) hand++;
            }
            else if (cel is Template template && template.HasBtn)
            {
                foreach (var item in template.value)
                {
                    if (item is TemplateButton btn_template)
                    {
                        if (btn_template.Value.Enabled)
                        {
                            btn_template.ExtraMouseHover = btn_template.RECT.Contains(x, y);
                            if (btn_template.ExtraMouseHover) hand++;
                        }
                        else btn_template.ExtraMouseHover = false;
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// 全局复选框改动时发生
        /// </summary>
        internal void ChangeCheckOverall(RowTemplate[] rows, RowTemplate it, ColumnCheck columnCheck, bool value)
        {
            handcheck = true;
            for (int i_row = 1; i_row < rows.Length; i_row++)
            {
                for (int i_col = 0; i_col < rows[i_row].cells.Length; i_col++)
                {
                    var item = rows[i_row].cells[i_col];
                    if (item is TCellCheck checkCell)
                    {
                        if (checkCell.Checked == value) continue;
                        checkCell.Checked = value;
                        rows[i_row].Checked = value;
                        item.PROPERTY?.SetValue(item.VALUE, checkCell.Checked);
                        CheckedChanged?.Invoke(this, value, rows[i_row].RECORD, i_row, i_col);
                    }
                }
            }
            it.CheckState = value ? CheckState.Checked : CheckState.Unchecked;
            columnCheck.SetCheckState(it.CheckState);
            handcheck = false;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            scrollX.Leave();
            scrollY.Leave();
            ILeave();
        }
        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            scrollX.Leave();
            scrollY.Leave();
            ILeave();
        }

        void ILeave()
        {
            SetCursor(false);
            if (rows == null || inEditMode) return;
            foreach (var it in rows)
            {
                it.Hover = false;
                foreach (var cel in it.cells)
                {
                    if (cel is Template template && template.HasBtn)
                    {
                        foreach (var item in template.value)
                        {
                            if (item is TemplateButton btn) btn.ExtraMouseHover = false;
                        }
                    }
                }
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            scrollY.MouseWheel(e.Delta);
            base.OnMouseWheel(e);
        }
    }
}