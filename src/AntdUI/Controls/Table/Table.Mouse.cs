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
            var cel_sel = CellContains(rows, e.X, e.Y, out int r_x, out int r_y, out int offset_x, out _, out int offset_y, out int i_row, out int i_cel, out int mode);
            if (cel_sel != null)
            {
                var it = rows[i_row];
                var item = it.cells[i_cel];
                CellClick?.Invoke(this, e, it.RECORD, i_row, i_cel, new Rectangle(item.RECT.X - offset_x, item.RECT.Y - offset_y, item.RECT.Width, item.RECT.Height));
            }
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            if (rows == null) return;
            var cel_sel = CellContains(rows, e.X, e.Y, out int r_x, out int r_y, out int offset_x, out int offset_xi, out int offset_y, out int i_row, out int i_cel, out int mode);
            if (cel_sel != null)
            {
                var it = rows[i_row];
                var item = it.cells[i_cel];
                if (editmode == TEditMode.DoubleClick)
                {
                    //进入编辑模式
                    EditModeClose();
                    OnEditMode(it, item, i_row, i_cel, offset_xi, offset_y);
                }
                CellDoubleClick?.Invoke(this, e, it.RECORD, i_row, i_cel, new Rectangle(item.RECT.X - offset_x, item.RECT.Y - offset_y, item.RECT.Width, item.RECT.Height));
            }
        }

        #region 鼠标按下

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (ClipboardCopy) Focus();
            if (scrollBar.MouseDownY(e.Location) && scrollBar.MouseDownX(e.Location))
            {
                base.OnMouseDown(e);
                if (rows == null) return;
                var cel_sel = CellContains(rows, e.X, e.Y, out int r_x, out int r_y, out _, out _, out _, out int i_row, out int i_cel, out int mode);
                if (cel_sel == null) return;
                else
                {
                    var it = rows[i_row];
                    if (mode > 0)
                    {
                        if (moveheaders.Length > 0)
                        {
                            foreach (var item in moveheaders)
                            {
                                if (item.rect.Contains(r_x, r_y))
                                {
                                    item.x = e.X;
                                    Window.CanHandMessage = false;
                                    item.MouseDown = true;
                                    return;
                                }
                            }
                        }
                        var cell = (TCellColumn)it.cells[i_cel];
                        cell.MouseDown = true;
                        if (cell.column is ColumnCheck columnCheck)
                        {
                            if (e.Button == MouseButtons.Left && cell.Contains(r_x, r_y))
                            {
                                ChangeCheckOverall(rows, it, columnCheck, !columnCheck.Checked);
                                return;
                            }
                        }
                        if (ColumnDragSort)
                        {
                            dragHeader = new DragHeader
                            {
                                i = cell.INDEX,
                                x = e.X
                            };
                            Cursor = Cursors.SizeAll;
                            return;
                        }
                    }
                    else MouseDownRow(e, it.cells[i_cel], r_x, r_y);
                }
            }
        }

        void MouseDownRow(MouseEventArgs e, TCell cell, int x, int y)
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
                                return;
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region 鼠标松开

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (moveheaders.Length > 0)
            {
                foreach (var item in moveheaders)
                {
                    if (item.MouseDown)
                    {
                        int width = item.width + e.X - item.x;
                        if (width < item.min_width) width = item.min_width;
                        if (tmpcol_width.ContainsKey(item.i)) tmpcol_width[item.i] = width;
                        else tmpcol_width.Add(item.i, width);
                        item.MouseDown = false;
                        Window.CanHandMessage = true;
                        LoadLayout();
                        Invalidate();
                        return;
                    }
                }
            }
            else if (dragHeader != null)
            {
                if (dragHeader.im != -1)
                {
                    //执行排序
                    if (rows == null) return;
                    var cells = rows[0].cells;
                    var sortHeader = new List<int>(cells.Length);
                    int dim = dragHeader.im, di = dragHeader.i;
                    if (SortHeader != null)
                    {
                        foreach (var item in SortHeader)
                        {
                            var it = (TCellColumn)cells[item];
                            if (dragHeader.im == it.INDEX) dim = it.column.INDEX;
                            if (dragHeader.i == it.INDEX) di = it.column.INDEX;
                        }
                    }
                    foreach (TCellColumn it in cells)
                    {
                        int index = it.column.INDEX;
                        if (index == dim)
                        {
                            if (dragHeader.last) sortHeader.Add(index);
                            if (sortHeader.Contains(di)) sortHeader.Remove(di);
                            sortHeader.Add(di);
                        }
                        if (!sortHeader.Contains(index)) sortHeader.Add(index);
                    }
                    SortHeader = sortHeader.ToArray();
                    LoadLayout();
                }
                dragHeader = null;
                Invalidate();
                return;
            }
            if (scrollBar.MouseUpY() && scrollBar.MouseUpX())
            {
                if (rows == null) return;
                for (int i_row = 0; i_row < rows.Length; i_row++)
                {
                    var it = rows[i_row];
                    for (int i_col = 0; i_col < it.cells.Length; i_col++)
                    {
                        if (MouseUpRow(rows, it, it.cells[i_col], e, i_row, i_col)) return;
                    }
                }
            }
        }

        Input? edit_input = null;
        bool MouseUpRow(RowTemplate[] rows, RowTemplate it, TCell cell, MouseEventArgs e, int i_r, int i_c)
        {
            if (cell.MouseDown)
            {
                var cel_sel = CellContains(rows, e.X, e.Y, out int r_x, out int r_y, out int offset_x, out int offset_xi, out int offset_y, out int i_row, out int i_cel, out int mode);
                if (cel_sel == null || (i_r != i_row || i_c != i_cel)) cell.MouseDown = false;
                else
                {
                    SelectedIndex = i_r;
                    if (e.Button == MouseButtons.Left)
                    {
                        if (cell is TCellCheck checkCell)
                        {
                            if (checkCell.Contains(r_x, r_y))
                            {
                                checkCell.Checked = !checkCell.Checked;
                                it.Checked = checkCell.Checked;
                                cell.PROPERTY?.SetValue(cell.VALUE, checkCell.Checked);
                                CheckedChanged?.Invoke(this, checkCell.Checked, it.RECORD, i_r, i_c);
                            }
                        }
                        else if (cell is TCellRadio radioCell)
                        {
                            if (radioCell.Contains(r_x, r_y) && !radioCell.Checked)
                            {
                                if (rows != null)
                                {
                                    for (int i = 0; i < rows.Length; i++)
                                    {
                                        if (i != i_r)
                                        {
                                            var cell2 = rows[i].cells[i_c];
                                            if (cell2 is TCellRadio radioCell2 && radioCell2.Checked)
                                            {
                                                radioCell2.Checked = false;
                                                rows[i].Checked = false;
                                                cell2.PROPERTY?.SetValue(cell2.VALUE, radioCell2.Checked);
                                            }
                                        }
                                    }
                                }
                                radioCell.Checked = true;
                                it.Checked = true;
                                cell.PROPERTY?.SetValue(cell.VALUE, radioCell.Checked);
                                CheckedChanged?.Invoke(this, radioCell.Checked, it.RECORD, i_r, i_c);
                            }
                        }
                        else if (cell is TCellSwitch switchCell)
                        {
                            if (switchCell.Contains(r_x, r_y) && !switchCell.Loading && switchCell.column.Call != null)
                            {
                                switchCell.Loading = true;
                                ITask.Run(() =>
                                {
                                    var value = switchCell.column.Call(!switchCell.Checked, it.RECORD, i_r, i_c);
                                    if (switchCell.Checked == value) return;
                                    switchCell.Checked = value;
                                    cell.PROPERTY?.SetValue(cell.VALUE, value);
                                }).ContinueWith(action =>
                                {
                                    switchCell.Loading = false;
                                });
                            }
                        }
                        else if (it.IsColumn && ((TCellColumn)cell).column.SortOrder)
                        {
                            //点击排序
                            var col = (TCellColumn)cell;
                            int SortMode;
                            if (col.rect_up.Contains(r_x, r_y)) SortMode = 1;
                            else if (col.rect_down.Contains(r_x, r_y)) SortMode = 2;
                            else
                            {
                                SortMode = col.column.SortMode + 1;
                                if (SortMode > 2) SortMode = 0;
                            }
                            if (col.column.SortMode != SortMode)
                            {
                                col.column.SortMode = SortMode;
                                foreach (TCellColumn item in it.cells)
                                {
                                    if (item.column.SortOrder && item.INDEX != i_c) item.column.SortMode = 0;
                                }
                                Invalidate();
                                switch (SortMode)
                                {
                                    case 1:
                                        SortDataASC(col.column.Key);
                                        break;
                                    case 2:
                                        SortDataDESC(col.column.Key);
                                        break;
                                    case 0:
                                    default:
                                        SortData = null;
                                        break;
                                }
                                LoadLayout();
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
                                        if (btn.RECT.Contains(r_x, r_y))
                                        {
                                            if (btn.Value is CellButton) btn.Click();
                                            CellButtonClick?.Invoke(this, btn.Value, e, it.RECORD, i_r, i_c);
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
                            OnEditMode(it, cell, i_r, i_c, offset_xi, offset_y);
                        }
                    }
                    cell.MouseDown = false;
                }
                return true;
            }
            return false;
        }

        bool inEditMode = false;
        void EditModeClose()
        {
            if (edit_input != null)
            {
                scrollBar.OnInvalidate = null;
                edit_input?.Dispose();
                edit_input = null;
            }
            inEditMode = false;
        }
        void OnEditMode(RowTemplate it, TCell cell, int i_row, int i_col, int sx, int sy)
        {
            if (rows == null) return;
            if (it.AnimationHover)
            {
                it.ThreadHover?.Dispose();
                it.ThreadHover = null;
            }
            if (cell is TCellText cellText)
            {
                object? value = null;
                if (cell.PROPERTY != null && cell.VALUE != null) value = cell.PROPERTY.GetValue(cell.VALUE);
                else value = cell.VALUE;

                bool isok = true;
                if (CellBeginEdit != null) isok = CellBeginEdit(this, value, it.RECORD, i_row, i_col);
                if (!isok) return;
                inEditMode = true;

                scrollBar.OnInvalidate = () => EditModeClose();
                BeginInvoke(new Action(() =>
                {
                    for (int i = 0; i < rows.Length; i++) rows[i].hover = i == i_row;
                    int height = Helper.GDI(g =>
                    {
                        return height = (int)Math.Ceiling(g.MeasureString(Config.NullText, Font).Height * 1.66F);
                    });
                    edit_input = ShowInput(cell, sx, sy, height, value, _value =>
                    {
                        bool isok_end = true;
                        if (CellEndEdit != null) isok_end = CellEndEdit(this, _value, it.RECORD, i_row, i_col);
                        if (isok_end)
                        {
                            cellText.value = _value;
                            if (cell.PROPERTY != null)
                            {
                                if (GetValue(value, _value, out var o)) cell.PROPERTY.SetValue(cell.VALUE, o);
                            }
                            else if (it.RECORD is DataRow datarow)
                            {
                                if (GetValue(value, _value, out var o)) datarow[i_col] = o;
                            }
                            EditModeClose();
                        }
                    });
                    if (cellText.column.Align == ColumnAlign.Center) edit_input.TextAlign = HorizontalAlignment.Center;
                    else if (cellText.column.Align == ColumnAlign.Right) edit_input.TextAlign = HorizontalAlignment.Right;
                    CellBeginEditInputStyle?.Invoke(this, value, it.RECORD, i_row, i_col, ref edit_input);
                    Controls.Add(edit_input);
                    edit_input.Focus();
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
                        bool isok = true;
                        if (CellBeginEdit != null) isok = CellBeginEdit(this, value, it.RECORD, i_row, i_col);
                        if (!isok) return;
                        inEditMode = true;

                        scrollBar.OnInvalidate = () => EditModeClose();
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
                            edit_input = ShowInput(cell, sx, sy, height, value, _value =>
                            {
                                bool isok_end = true;
                                if (CellEndEdit != null) isok_end = CellEndEdit(this, _value, it.RECORD, i_row, i_col);
                                if (isok_end)
                                {
                                    if (value is CellText text2)
                                    {
                                        text2.Text = _value;
                                        if (cell.PROPERTY != null) cell.PROPERTY.SetValue(cell.VALUE, text2);
                                    }
                                    else
                                    {
                                        text.Value.Text = _value;
                                        if (cell.PROPERTY != null)
                                        {
                                            if (GetValue(value, _value, out var o)) cell.PROPERTY.SetValue(cell.VALUE, o);
                                        }
                                        else if (it.RECORD is DataRow datarow)
                                        {
                                            if (GetValue(value, _value, out var o)) datarow[i_col] = o;
                                        }
                                    }
                                    EditModeClose();
                                }
                            });
                            CellBeginEditInputStyle?.Invoke(this, value, it.RECORD, i_row, i_col, ref edit_input);
                            if (template.PARENT != null)
                            {
                                if (template.PARENT.column.Align == ColumnAlign.Center) edit_input.TextAlign = HorizontalAlignment.Center;
                                else if (template.PARENT.column.Align == ColumnAlign.Right) edit_input.TextAlign = HorizontalAlignment.Right;
                            }
                            Controls.Add(edit_input);
                            edit_input.Focus();
                        }));
                        return;
                    }
                }
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

        Input ShowInput(TCell cell, int sx, int sy, int height, object? value, Action<string> call)
        {
            Input input;
            if (value is CellText text2)
            {
                input = new Input
                {
                    Location = new Point(cell.RECT.X - sx, cell.RECT.Y - sy + (cell.RECT.Height - height) / 2),
                    Size = new Size(cell.RECT.Width, height),
                    Text = text2.Text ?? ""
                };
            }
            else
            {
                input = new Input
                {
                    Location = new Point(cell.RECT.X - sx, cell.RECT.Y - sy + (cell.RECT.Height - height) / 2),
                    Size = new Size(cell.RECT.Width, height),
                    Text = value?.ToString() ?? ""
                };
            }
            string old_text = input.Text;
            input.KeyPress += (a, b) =>
            {
                if (b.KeyChar == 13 && a is Input input)
                {
                    b.Handled = true;
                    if (old_text == input.Text)
                    {
                        EditModeClose();
                        return;
                    }
                    call(input.Text);
                }
            };
            input.LostFocus += (a, b) =>
            {
                if (old_text == input.Text)
                {
                    EditModeClose();
                    return;
                }
                call(input.Text);
            };
            return input;
        }

        #endregion

        #region 鼠标移动

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (moveheaders.Length > 0)
            {
                foreach (var item in moveheaders)
                {
                    if (item.MouseDown)
                    {
                        int width = item.width + e.X - item.x;
                        if (width < item.min_width) return;
                        if (tmpcol_width.ContainsKey(item.i)) tmpcol_width[item.i] = width;
                        else tmpcol_width.Add(item.i, width);
                        LoadLayout();
                        Invalidate();
                        Cursor = Cursors.VSplit;
                        return;
                    }
                }
            }
            else if (dragHeader != null)
            {
                dragHeader.xr = e.X - dragHeader.x;
                if (rows == null) return;
                int xr = dragHeader.x + dragHeader.xr;
                var cells = rows[0].cells;
                dragHeader.last = e.X > dragHeader.x;
                foreach (var it in cells)
                {
                    if (it.RECT.Contains(xr, it.RECT.Y + 1))
                    {
                        if (it.INDEX == dragHeader.i) dragHeader.im = -1;
                        else dragHeader.im = it.INDEX;
                        Invalidate();
                        return;
                    }
                }
                if (cells[cells.Length - 1].INDEX == dragHeader.i) dragHeader.im = -1;
                else dragHeader.im = cells[cells.Length - 1].INDEX;
                Invalidate();
                return;
            }
            if (scrollBar.MouseMoveY(e.Location) && scrollBar.MouseMoveX(e.Location))
            {
                if (rows == null || inEditMode) return;
                var cel_sel = CellContains(rows, e.X, e.Y, out int r_x, out int r_y, out int offset_x, out int offset_xi, out int offset_y, out int i_row, out int i_cel, out int mode);
                if (cel_sel == null)
                {
                    foreach (RowTemplate it in rows)
                    {
                        if (it.IsColumn) continue;
                        it.Hover = false;
                        foreach (var cel_tmp in it.cells)
                        {
                            if (cel_tmp is Template template && template.HasBtn)
                            {
                                foreach (var item in template.value)
                                {
                                    if (item is TemplateButton btn) btn.ExtraMouseHover = false;
                                }
                            }
                        }
                    }
                    SetCursor(false);
                }
                else
                {
                    if (mode > 0)
                    {
                        for (int i = 1; i < rows.Length; i++)
                        {
                            rows[i].Hover = false;
                            foreach (var cel_tmp in rows[i].cells)
                            {
                                if (cel_tmp is Template template && template.HasBtn)
                                {
                                    foreach (var item in template.value)
                                    {
                                        if (item is TemplateButton btn) btn.ExtraMouseHover = false;
                                    }
                                }
                            }
                        }
                        var cel = (TCellColumn)cel_sel;
                        if (moveheaders.Length > 0)
                        {
                            foreach (var item in moveheaders)
                            {
                                if (item.rect.Contains(r_x, r_y))
                                {
                                    Cursor = Cursors.VSplit;
                                    return;
                                }
                            }
                        }
                        if (cel.SortWidth > 0) SetCursor(true);
                        else if (has_check && cel.column is ColumnCheck && cel.Contains(r_x, r_y)) SetCursor(true);
                        else if (ColumnDragSort)
                        {
                            Cursor = Cursors.SizeAll;
                            return;
                        }
                        else SetCursor(false);
                    }
                    else
                    {
                        for (int i = 1; i < rows.Length; i++)
                        {
                            if (i == i_row) rows[i].Hover = true;
                            else
                            {
                                rows[i].Hover = false;
                                foreach (var cel_tmp in rows[i].cells)
                                {
                                    if (cel_tmp is Template template && template.HasBtn)
                                    {
                                        foreach (var item in template.value)
                                        {
                                            if (item is TemplateButton btn) btn.ExtraMouseHover = false;
                                        }
                                    }
                                }
                            }
                        }
                        SetCursor(MouseMoveRow(cel_sel, r_x, r_y, offset_x, offset_xi, offset_y));
                    }
                }
            }
            else ILeave();
        }

        bool MouseMoveRow(TCell cel, int x, int y, int offset_x, int offset_xi, int offset_y)
        {
            if (cel is TCellCheck checkCell)
            {
                if (checkCell.Contains(x, y)) return true;
                return false;
            }
            else if (cel is TCellRadio radioCell)
            {
                if (radioCell.Contains(x, y)) return true;
                return false;
            }
            else if (cel is TCellSwitch switchCell)
            {
                if (switchCell.column.Call == null) return false;
                if (switchCell.Contains(x, y))
                {
                    switchCell.ExtraMouseHover = true;
                    return true;
                }
                else switchCell.ExtraMouseHover = false;
                return false;
            }
            else if (cel is Template template && template.HasBtn)
            {
                int hand = 0;
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
                return hand > 0;
            }
            else if (ShowTip)
            {
                var moveid = cel.INDEX + "_" + cel.ROW.INDEX;
                if (oldmove != moveid)
                {
                    CloseTip();
                    oldmove = moveid;
                    if (cel.MinWidth > cel.rect.Width)
                    {
                        var text = cel.ToString();
                        if (!string.IsNullOrEmpty(text))
                        {
                            var _rect = RectangleToScreen(ClientRectangle);
                            var rect = new Rectangle(_rect.X + cel.RECT.X - offset_xi, _rect.Y + cel.RECT.Y - offset_y, cel.RECT.Width, cel.RECT.Height);
                            if (tooltipForm == null)
                            {
                                tooltipForm = new TooltipForm(rect, text, new TooltipConfig
                                {
                                    Font = Font,
                                    ArrowAlign = TAlign.Top,
                                });
                                tooltipForm.Show(this);
                            }
                            else tooltipForm.SetText(rect, text);
                        }
                    }
                }
            }
            return false;
        }
        string? oldmove = null;
        TooltipForm? tooltipForm = null;
        void CloseTip(bool clear = false)
        {
            tooltipForm?.IClose();
            tooltipForm = null;
            if (clear) oldmove = null;
        }

        #endregion

        #region 判断是否在内部

        TCell? CellContains(RowTemplate[] rows, int ex, int ey, out int r_x, out int r_y, out int offset_x, out int offset_xi, out int offset_y, out int i_row, out int i_cel, out int mode)
        {
            mode = 0;
            int sx = scrollBar.ValueX, sy = scrollBar.ValueY;
            int px = ex + sx, py = ey + sy;
            foreach (RowTemplate it in rows)
            {
                if (it.IsColumn)
                {
                    if (fixedHeader)
                    {
                        if (it.CONTAINS(ex, ey))
                        {
                            mode = 2;
                            var hasi = new List<int>();
                            if (showFixedColumnL && fixedColumnL != null)
                            {
                                foreach (var i in fixedColumnL)
                                {
                                    hasi.Add(i);
                                    var cel = it.cells[i];
                                    if (cel.CONTAINS(ex, ey))
                                    {
                                        r_x = ex;
                                        r_y = ey;

                                        offset_x = offset_xi = 0;
                                        offset_y = sy;

                                        i_row = it.INDEX;
                                        i_cel = i;
                                        return cel;
                                    }
                                }
                            }
                            if (showFixedColumnR && fixedColumnR != null)
                            {
                                foreach (var i in fixedColumnR)
                                {
                                    hasi.Add(i);
                                    var cel = it.cells[i];
                                    if (cel.CONTAINS(ex + sFixedR, ey))
                                    {
                                        r_x = ex + sFixedR;
                                        r_y = ey;

                                        offset_x = -sFixedR;
                                        offset_xi = sFixedR;
                                        offset_y = sy;

                                        i_row = it.INDEX;
                                        i_cel = i;
                                        return cel;
                                    }
                                }
                            }
                            for (int i = 0; i < it.cells.Length; i++)
                            {
                                if (hasi.Contains(i)) continue;
                                var cel = it.cells[i];
                                if (cel.CONTAINS(px, ey))
                                {
                                    r_x = px;
                                    r_y = ey;

                                    offset_x = offset_xi = sx;
                                    offset_y = sy;

                                    i_row = it.INDEX;
                                    i_cel = i;
                                    return cel;
                                }
                            }
                        }
                    }
                    else if (it.CONTAINS(ex, py))
                    {
                        mode = 1;
                        var hasi = new List<int>();
                        if (showFixedColumnL && fixedColumnL != null)
                        {
                            foreach (var i in fixedColumnL)
                            {
                                hasi.Add(i);
                                var cel = it.cells[i];
                                if (cel.CONTAINS(ex, py))
                                {
                                    r_x = ex;
                                    r_y = py;

                                    offset_x = offset_xi = 0;
                                    offset_y = sy;

                                    i_row = it.INDEX;
                                    i_cel = i;
                                    return cel;
                                }
                            }
                        }
                        if (showFixedColumnR && fixedColumnR != null)
                        {
                            foreach (var i in fixedColumnR)
                            {
                                hasi.Add(i);
                                var cel = it.cells[i];
                                if (cel.CONTAINS(ex + sFixedR, py))
                                {
                                    r_x = ex + sFixedR;
                                    r_y = py;

                                    offset_x = -sFixedR;
                                    offset_xi = sFixedR;
                                    offset_y = sy;

                                    i_row = it.INDEX;
                                    i_cel = i;
                                    return cel;
                                }
                            }
                        }
                        for (int i = 0; i < it.cells.Length; i++)
                        {
                            if (hasi.Contains(i)) continue;
                            var cel = it.cells[i];
                            if (cel.CONTAINS(px, py))
                            {
                                r_x = px;
                                r_y = py;

                                offset_x = offset_xi = sx;
                                offset_y = sy;

                                i_row = it.INDEX;
                                i_cel = i;
                                return cel;
                            }
                        }
                    }
                }
                else if (it.Contains(ex, py))
                {
                    var hasi = new List<int>();
                    if (showFixedColumnL && fixedColumnL != null)
                    {
                        foreach (var i in fixedColumnL)
                        {
                            hasi.Add(i);
                            var cel = it.cells[i];
                            if (cel.CONTAINS(ex, py))
                            {
                                r_x = ex;
                                r_y = py;

                                offset_x = offset_xi = 0;
                                offset_y = sy;

                                i_row = it.INDEX;
                                i_cel = i;
                                return cel;
                            }
                        }
                    }
                    if (showFixedColumnR && fixedColumnR != null)
                    {
                        foreach (var i in fixedColumnR)
                        {
                            hasi.Add(i);
                            var cel = it.cells[i];
                            if (cel.CONTAINS(ex + sFixedR, py))
                            {
                                r_x = ex + sFixedR;
                                r_y = py;

                                offset_x = -sFixedR;
                                offset_xi = sFixedR;
                                offset_y = sy;

                                i_row = it.INDEX;
                                i_cel = i;
                                return cel;
                            }
                        }
                    }
                    for (int i = 0; i < it.cells.Length; i++)
                    {
                        if (hasi.Contains(i)) continue;
                        var cel = it.cells[i];
                        if (cel.CONTAINS(px, py))
                        {
                            r_x = px;
                            r_y = py;

                            offset_x = offset_xi = sx;
                            offset_y = sy;

                            i_row = it.INDEX;
                            i_cel = i;
                            return cel;
                        }
                    }
                }
            }
            r_x = r_y = offset_x = offset_xi = offset_y = i_row = i_cel = 0;
            return null;
        }

        #endregion

        DragHeader? dragHeader = null;

        #region 排序

        int[]? SortHeader = null;
        int[]? SortData = null;
        List<SortModel> SortDatas(string key)
        {
            if (data_temp == null) return new List<SortModel>(0);
            var list = new List<SortModel>(data_temp.rows.Length);
            for (int i_r = 0; i_r < data_temp.rows.Length; i_r++)
            {
                var value = OGetValue(data_temp, i_r, key);
                list.Add(new SortModel(i_r, value?.ToString()));
            }
            return list;
        }
        void SortDataASC(string key)
        {
            var list = SortDatas(key);
            list.Sort((x, y) => FilesNameComparerClass.Compare(x, y));
            var SortTmp = new List<int>(list.Count);
            foreach (var it in list) SortTmp.Add(it.i);
            SortData = SortTmp.ToArray();
        }
        void SortDataDESC(string key)
        {
            var list = SortDatas(key);
            list.Sort((y, x) => FilesNameComparerClass.Compare(x, y));
            var SortTmp = new List<int>(list.Count);
            foreach (var it in list) SortTmp.Add(it.i);
            SortData = SortTmp.ToArray();
        }

        #endregion

        protected override void OnLostFocus(EventArgs e)
        {
            if (LostFocusClearSelection) SelectedIndex = -1;
            CloseTip(true);
            base.OnLostFocus(e);
        }

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
            scrollBar.Leave();
            ILeave();
            CloseTip(true);
        }
        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            scrollBar.Leave();
            ILeave();
            CloseTip(true);
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
            scrollBar.MouseWheel(e.Delta);
            base.OnMouseWheel(e);
        }
    }
}