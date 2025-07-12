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
// GITEE: https://gitee.com/AntdUI/AntdUI
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
        #region 鼠标

        #region 鼠标按下

        int shift_index = -1;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            cellMouseDown = null;
            btnMouseDown = null;
            if (ClipboardCopy) Focus();
            subForm?.IClose();
            subForm = null;
            if (ScrollBar.MouseDownY(e.Location) && ScrollBar.MouseDownX(e.Location))
            {
                base.OnMouseDown(e);
                if (rows == null) return;
                OnTouchDown(e.X, e.Y);
                var cell = CellContains(rows, true, e.X, e.Y, out int r_x, out int r_y, out _, out _, out _, out int i_row, out int i_cel, out var column, out int mode);
                if (cell == null) return;
                else
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        if (MultipleRows && ModifierKeys.HasFlag(Keys.Shift))
                        {
                            if (shift_index == -1) SelectedIndexs = SetIndexs(i_row);
                            else
                            {
                                if (shift_index > i_row) SelectedIndexs = SetIndexs(i_row, shift_index);
                                else SelectedIndexs = SetIndexs(shift_index, i_row);
                            }
                        }
                        else if (MultipleRows && ModifierKeys.HasFlag(Keys.Control)) SelectedIndexs = SetIndexs(i_row);
                        else SelectedIndex = i_row;
                    }
                    shift_index = i_row;
                    if (dataSource is BindingSource bindingSource) bindingSource.Position = i_row - 1;
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
                        cellMouseDown = new DownCellTMP<CELL>(it, cell, i_row, i_cel, e.Clicks > 1);
                        if (!cellMouseDown.doubleClick && cell.COLUMN is ColumnCheck columnCheck && columnCheck.NoTitle)
                        {
                            if (e.Button == MouseButtons.Left && cell.CONTAIN_REAL(r_x, r_y))
                            {
                                CheckAll(i_cel, columnCheck, !columnCheck.Checked);
                                return;
                            }
                        }
                        if (ColumnDragSort && cell.COLUMN.DragSort)
                        {
                            dragHeader = new DragHeader(e.X, e.Y, cell.COLUMN.INDEX_REAL, e.X);
                            return;
                        }
                    }
                    else
                    {
                        if (cell.COLUMN is ColumnSort sort && cell.CONTAIN_REAL(r_x, r_y))
                        {
                            dragBody = new DragHeader(e.X, e.Y, cell.ROW.INDEX, e.Y);
                            return;
                        }
                        if (cell.ROW.CanExpand && cell.ROW.RECORD != null && cell.ROW.RectExpand.Contains(r_x, r_y))
                        {
                            if (cell.ROW.Expand) rows_Expand.Remove(cell.ROW.RECORD);
                            else rows_Expand.Add(cell.ROW.RECORD);
                            ExpandChanged?.Invoke(this, new TableExpandEventArgs(cell.ROW.RECORD, !cell.ROW.Expand));
                            if (LoadLayout()) Invalidate();
                            return;
                        }
                        MouseDownRow(e, it, it.cells[i_cel], r_x, r_y, i_row, i_cel, column);
                    }
                }
            }
        }

        void MouseDownRow(MouseEventArgs e, RowTemplate it, CELL cell, int x, int y, int i_r, int i_c, Column? column)
        {
            cellMouseDown = new DownCellTMP<CELL>(it, cell, i_r, i_c, e.Clicks > 1);
            if (cell is Template template)
            {
                if (e.Button == MouseButtons.Left)
                {
                    foreach (var item in template.Value)
                    {
                        if (item is CellLink btn_template)
                        {
                            if (btn_template.Enabled)
                            {
                                if (btn_template.Rect.Contains(x, y))
                                {
                                    btnMouseDown = new DownCellTMP<CellLink>(it, btn_template, i_r, i_c, cellMouseDown.doubleClick);
                                    btn_template.ExtraMouseDown = true;
                                    CellButtonDown?.Invoke(this, new TableButtonEventArgs(btn_template, it.RECORD, i_r, i_c, column, e));
                                    return;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (CellButtonDown == null) return;
                    foreach (var item in template.Value)
                    {
                        if (item is CellLink btn_template)
                        {
                            if (btn_template.Enabled)
                            {
                                if (btn_template.Rect.Contains(x, y))
                                {
                                    btnMouseDown = new DownCellTMP<CellLink>(it, btn_template, i_r, i_c, cellMouseDown.doubleClick);
                                    CellButtonDown(this, new TableButtonEventArgs(btn_template, it.RECORD, i_r, i_c, column, e));
                                    return;
                                }
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
            var cellMDown = cellMouseDown;
            var btnMDown = btnMouseDown;
            cellMouseDown = null;
            btnMouseDown = null;
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
                        OnTouchCancel();
                        return;
                    }
                }
            }
            if (dragHeader != null)
            {
                bool hand = dragHeader.hand;
                if (hand && dragHeader.enable && dragHeader.im != -1)
                {
                    //执行排序
                    if (columns == null) return;
                    var sortHeader = new List<int>(columns.Count);
                    if (SortHeader == null)
                    {
                        foreach (var it in columns) sortHeader.Add(it.INDEX_REAL);
                    }
                    else sortHeader.AddRange(SortHeader);

                    int sourceIndex = sortHeader.IndexOf(dragHeader.i), targetIndex = sortHeader.IndexOf(dragHeader.im);
                    int sourceRealIndex = sortHeader[sourceIndex];
                    sortHeader.RemoveAt(sourceIndex);
                    // 调整插入位置，处理拖到最后位置的情况
                    sortHeader.Insert(targetIndex, sourceRealIndex);
                    SortHeader = sortHeader.ToArray();
                    ExtractHeaderFixed();
                    LoadLayout();
                }
                dragHeader = null;
                if (hand)
                {
                    Invalidate();
                    OnTouchCancel();
                    return;
                }
            }
            if (dragBody != null)
            {
                bool hand = dragBody.hand;
                if (hand && dragBody.enable && dragBody.im != -1)
                {
                    //执行排序
                    if (rows == null) return;
                    var sortData = new List<int>(rows.Length);
                    int dim = dragBody.im, di = dragBody.i;
                    foreach (var it in rows)
                    {
                        it.hover = false;
                        if (dragBody.im == it.INDEX) { it.hover = true; dim = it.INDEX_REAL; }
                        if (dragBody.i == it.INDEX) di = it.INDEX_REAL;
                    }
                    if (dim == di)
                    {
                        object? record = null;
                        int from = dragBody.i, to = dragBody.im, count = 0;
                        foreach (var it in rows)
                        {
                            if (it.INDEX_REAL == di)
                            {
                                if (record == null)
                                {
                                    record = it.RECORD;
                                    from -= it.INDEX + 1;
                                    to -= it.INDEX + 1;
                                }
                                else count++;
                            }
                        }
                        var d = new int[count];
                        for (int i = 0; i < count; i++)
                        {
                            if (i == from) d[i] = to;
                            else if (i == to) d[i] = from;
                            else d[i] = i;
                        }
                        SortRowsTree?.Invoke(this, new TableSortTreeEventArgs(record, d, from, to));
                    }
                    else
                    {
                        SetIndex(dragBody.im);
                        foreach (var it in rows)
                        {
                            int index = it.INDEX_REAL;
                            if (index > -1)
                            {
                                if (index == dim)
                                {
                                    if (dragBody.last) sortData.Add(index);
                                    if (sortData.Contains(di)) sortData.Remove(di);
                                    sortData.Add(di);
                                }
                                if (!sortData.Contains(index)) sortData.Add(index);
                            }
                        }
                        SortData = sortData.ToArray();
                        LoadLayout();
                        SortRows?.Invoke(this, new IntEventArgs(-1));
                    }
                }
                dragBody = null;
                if (hand)
                {
                    Invalidate();
                    OnTouchCancel();
                    return;
                }
            }
            if (ScrollBar.MouseUpY() && ScrollBar.MouseUpX())
            {
                if (rows == null) return;
                if (OnTouchUp())
                {
                    if (cellMDown == null) return;
                    MouseUpRow(rows, cellMDown, btnMDown, e);
                }
                else
                {
                    if (btnMDown == null) return;
                    if (btnMDown.cell.ExtraMouseDown)
                    {
                        CellButtonUp?.Invoke(this, new TableButtonEventArgs(btnMDown.cell, btnMDown.row.RECORD, btnMDown.i_row, btnMDown.i_cel, btnMDown.cell.PARENT.COLUMN, e));
                        btnMDown.cell.ExtraMouseDown = false;
                    }
                }
            }
        }

        void filter_PopupEndEventMethod(object sender, CancelEventArgs e)
        {
            if (FilterPopupEnd != null)
            {
                Popover.Config? config = sender as Popover.Config;
                if (config == null) return;
                var arg = new TableFilterPopupEndEventArgs(config.Tag is FilterOption ? (FilterOption)config.Tag : null, FilterList());
                FilterPopupEnd(sender, arg);
                e.Cancel = arg.Cancel;
            }
            if (e.Cancel == false)
            {
                inEditMode = false;
                OnMouseLeave(EventArgs.Empty);
            }
        }

        void MouseUpRow(RowTemplate[] rows, DownCellTMP<CELL> it, DownCellTMP<CellLink>? btn, MouseEventArgs e)
        {
            var cel_sel = CellContains(rows, true, e.X, e.Y, out int r_x, out int r_y, out int offset_x, out int offset_xi, out int offset_y, out int i_row, out int i_cel, out var column, out int mode);
            if (cel_sel == null || (it.i_row != i_row || it.i_cel != i_cel)) MouseUpBtn(it, btn, e, r_x, r_y, offset_xi, offset_y, null);
            else
            {
                if (selectedIndex.Length == 1) SelectedIndex = it.i_row;
                if (MouseUpBtn(it, btn, e, r_x, r_y, offset_xi, offset_y, column)) return;
                if (e.Button == MouseButtons.Left)
                {
                    if (it.cell is TCellCheck checkCell)
                    {
                        if (checkCell.CONTAIN_REAL(r_x, r_y))
                        {
                            if (checkCell.COLUMN is ColumnCheck columnCheck && columnCheck.Call != null)
                            {
                                var value = columnCheck.Call(!checkCell.Checked, it.row.RECORD, it.i_row, it.i_cel);
                                if (checkCell.Checked != value)
                                {
                                    checkCell.Checked = value;
                                    SetValue(it.cell, checkCell.Checked);
                                    CheckedChanged?.Invoke(this, new TableCheckEventArgs(checkCell.Checked, it.row.RECORD, it.i_row, it.i_cel, column));
                                }
                            }
                            else if (checkCell.AutoCheck)
                            {
                                checkCell.Checked = !checkCell.Checked;
                                SetValue(it.cell, checkCell.Checked);
                                CheckedChanged?.Invoke(this, new TableCheckEventArgs(checkCell.Checked, it.row.RECORD, it.i_row, it.i_cel, column));
                            }
                        }
                    }
                    else if (it.cell is TCellRadio radioCell)
                    {
                        if (radioCell.CONTAIN_REAL(r_x, r_y) && !radioCell.Checked)
                        {
                            bool isok = false;
                            if (radioCell.COLUMN is ColumnRadio columnRadio && columnRadio.Call != null)
                            {
                                var value = columnRadio.Call(true, it.row.RECORD, it.i_row, it.i_cel);
                                if (value) isok = true;
                            }
                            else if (radioCell.AutoCheck) isok = true;
                            if (isok)
                            {
                                for (int i = 0; i < rows.Length; i++)
                                {
                                    if (i != it.i_row)
                                    {
                                        var cell_selno = rows[i].cells[it.i_cel];
                                        if (cell_selno is TCellRadio radioCell2 && radioCell2.Checked)
                                        {
                                            radioCell2.Checked = false;
                                            SetValue(cell_selno, false);
                                        }
                                    }
                                }
                                radioCell.Checked = true;
                                SetValue(it.cell, radioCell.Checked);
                                CheckedChanged?.Invoke(this, new TableCheckEventArgs(radioCell.Checked, it.row.RECORD, it.i_row, it.i_cel, column));
                            }
                        }
                    }
                    else if (it.cell is TCellSwitch switchCell)
                    {
                        if (switchCell.CONTAIN_REAL(r_x, r_y) && !switchCell.Loading)
                        {
                            if (switchCell.COLUMN is ColumnSwitch columnSwitch && columnSwitch.Call != null)
                            {
                                switchCell.Loading = true;
                                ITask.Run(() =>
                                {
                                    var value = columnSwitch.Call(!switchCell.Checked, it.row.RECORD, it.i_row, it.i_cel);
                                    if (switchCell.Checked == value) return;
                                    switchCell.Checked = value;
                                    SetValue(it.cell, value);
                                }).ContinueWith(action =>
                                {
                                    switchCell.Loading = false;
                                });
                            }
                            else if (switchCell.AutoCheck)
                            {
                                switchCell.Checked = !switchCell.Checked;
                                SetValue(it.cell, switchCell.Checked);
                                CheckedChanged?.Invoke(this, new TableCheckEventArgs(switchCell.Checked, it.row.RECORD, it.i_row, it.i_cel, column));
                            }
                        }
                    }
                    else if (it.cell is Template template)
                    {
                        foreach (var item in template.Value)
                        {
                            if (item is CellCheckbox checkbox)
                            {
                                if (checkbox.Rect.Contains(r_x, r_y) && checkbox.Enabled && checkbox.AutoCheck) checkbox.Checked = !checkbox.Checked;
                            }
                            else if (item is CellRadio radio)
                            {
                                if (radio.Rect.Contains(r_x, r_y) && radio.Enabled && radio.AutoCheck) radio.Checked = !radio.Checked;
                            }
                        }
                    }
                    else if (it.row.IsColumn && it.cell is TCellColumn col)
                    {
                        if (it.cell.COLUMN.Filter != null && col.rect_filter.Contains(r_x - col.offsetx, r_y - col.offsety))
                        {
                            //点击筛选
                            var focusColumn = it.cell.COLUMN;
                            IList<object>? customSource = null;
                            Font? fnt = null;
                            int filterHeight = 0;
                            if (FilterPopupBegin != null)
                            {
                                var arg = new TableFilterPopupBeginEventArgs(focusColumn);
                                FilterPopupBegin(this, arg);
                                if (arg.Cancel) return;
                                customSource = arg.CustomSource;
                                fnt = arg.Font;
                                filterHeight = arg.Height;
                            }
                            if (fnt == null) fnt = Font;
                            var editor = new FilterControl(this, focusColumn, customSource)
                            {
                                Font = fnt
                            };
                            if (filterHeight > 0) editor.Height = filterHeight;
                            Point location = PointToScreen(col.rect_filter.Location);
                            Point locaionOrigin = location;
                            location.X -= (focusColumn.Fixed ? 0 : ScrollBar.ValueX);
                            if (fixedColumnR != null && fixedColumnR.Contains(Columns.IndexOf(focusColumn))) location.X -= (showFixedColumnR ? _gap : _gap * 2);
                            location.X += col.rect_filter.Width / 2;
                            location.Y += col.rect_filter.Height;
                            Rectangle? rectScreen = Screen.FromPoint(location).WorkingArea;
                            TAlign align = TAlign.Bottom;
                            if (rectScreen.HasValue)
                            {
                                if (location.X - (editor.Width / 2) < rectScreen.Value.Left)
                                {
                                    align = TAlign.Right;
                                    location.X = editor.Width / 2;
                                }
                                else if (location.X + editor.Width > rectScreen.Value.Right)
                                {
                                    align = TAlign.Left;
                                    location.X = rectScreen.Value.Right - editor.Width / 2;
                                }
                                else if (location.Y + editor.Height > rectScreen.Value.Bottom)
                                {
                                    align = TAlign.Top;
                                    location.Y = locaionOrigin.Y;
                                }
                            }

                            Popover.open(new Popover.Config(this, editor)
                            {
                                Dpi = (fnt.Size / 9F) * Config.Dpi,
                                Tag = focusColumn.Filter,
                                ArrowAlign = align,
                                Font = fnt,
                                CustomPoint = new Rectangle(location, Size.Empty),
                                Padding = new Size(6, 6),
                                OnClosing = filter_PopupEndEventMethod
                            });
                        }
                        else if (it.cell.COLUMN.SortOrder)
                        {
                            //点击排序
                            SortMode sortMode = SortMode.NONE;
                            int r_x_f = r_x - col.offsetx, r_y_f = r_y - col.offsety;
                            if (col.rect_up.Contains(r_x_f, r_y_f)) sortMode = SortMode.ASC;
                            else if (col.rect_down.Contains(r_x_f, r_y_f)) sortMode = SortMode.DESC;
                            else
                            {
                                sortMode = col.COLUMN.SortMode + 1;
                                if (sortMode > SortMode.DESC) sortMode = SortMode.NONE;
                            }
                            if (col.COLUMN.SetSortMode(sortMode))
                            {
                                foreach (var item in it.row.cells)
                                {
                                    if (item.COLUMN.SortOrder && item.INDEX != it.i_cel) item.COLUMN.SetSortMode(SortMode.NONE);
                                }
                                var result = SortModeChanged?.Invoke(this, new TableSortModeEventArgs(sortMode, col.COLUMN)) ?? false;
                                if (result) Invalidate();
                                else
                                {
                                    Invalidate();
                                    switch (sortMode)
                                    {
                                        case SortMode.ASC:
                                            SortDataASC(col.COLUMN);
                                            break;
                                        case SortMode.DESC:
                                            SortDataDESC(col.COLUMN);
                                            break;
                                        case SortMode.NONE:
                                        default:
                                            SortData = null;
                                            break;
                                    }
                                    LoadLayout();
                                    SortRows?.Invoke(this, new IntEventArgs(it.i_cel));
                                }
                            }
                        }
                    }
                }
                bool enterEdit = false;
                if (it.doubleClick)
                {
                    CellDoubleClick?.Invoke(this, new TableClickEventArgs(it.row.RECORD, i_row, i_cel, column, new Rectangle(cel_sel.RECT.X - offset_x, cel_sel.RECT.Y - offset_y, cel_sel.RECT.Width, cel_sel.RECT.Height), e));
                    if (e.Button == MouseButtons.Left && editmode == TEditMode.DoubleClick) enterEdit = true;
                }
                else
                {
                    CellClick?.Invoke(this, new TableClickEventArgs(it.row.RECORD, i_row, i_cel, column, new Rectangle(cel_sel.RECT.X - offset_x, cel_sel.RECT.Y - offset_y, cel_sel.RECT.Width, cel_sel.RECT.Height), e));
                    if (e.Button == MouseButtons.Left && editmode == TEditMode.Click) enterEdit = true;
                }
                if (enterEdit)
                {
                    EditModeClose();
                    if (CanEditMode(it.row, cel_sel))
                    {
                        int val = ScrollLine(i_row, rows);
                        OnEditMode(it.row, cel_sel, i_row, i_cel, column, offset_xi, offset_y - val);
                    }
                }
            }
        }
        bool MouseUpBtn(DownCellTMP<CELL> it, DownCellTMP<CellLink>? btn, MouseEventArgs e, int r_x, int r_y, int offset_xi, int offset_y, Column? column)
        {
            if (btn == null) return false;
            btn.cell.ExtraMouseDown = false;
            if (e.Button == MouseButtons.Left && btn.cell.Rect.Contains(r_x, r_y))
            {
                btn.cell.Click();

                if (btn.cell.DropDownItems != null && btn.cell.DropDownItems.Count > 0)
                {
                    subForm?.IClose();
                    subForm = null;
                    var rect = btn.cell.Rect;
                    rect.Offset(-offset_xi, -offset_y);
                    subForm = new LayeredFormSelectDown(this, btn.cell.DropDownItems, btn.cell, rect);
                    subForm.Show(this);
                }

                var arge = new TableButtonEventArgs(btn.cell, it.row.RECORD, it.i_row, it.i_cel, column, e);
                CellButtonUp?.Invoke(this, arge);
                CellButtonClick?.Invoke(this, arge);
                return true;
            }
            CellButtonUp?.Invoke(this, new TableButtonEventArgs(btn.cell, it.row.RECORD, it.i_row, it.i_cel, column, e));
            return false;
        }
        LayeredFormSelectDown? subForm;

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
                        if (LoadLayout()) Invalidate();
                        SetCursor(CursorType.VSplit);
                        return;
                    }
                }
            }
            if (dragHeader != null)
            {
                dragHeader.SetEnable(e.X, e.Y);
                SetCursor(CursorType.SizeAll);
                dragHeader.hand = true;
                dragHeader.xr = e.X - dragHeader.x;
                if (rows == null) return;
                int xr = dragHeader.x + dragHeader.xr;
                var cells = rows[0].cells;
                dragHeader.last = e.X > dragHeader.x;

                var cel_sel = CellContains(rows, false, xr, e.Y, out int r_x, out int r_y, out int offset_x, out int offset_xi, out int offset_y, out int i_row, out int i_cel, out var column, out int mode);
                if (cel_sel != null)
                {
                    var it = cells[i_cel];
                    if (it.COLUMN.INDEX_REAL == dragHeader.i) dragHeader.im = -1;
                    else dragHeader.im = it.COLUMN.INDEX_REAL;
                    Invalidate();
                    return;
                }
                var last = cells[cells.Length - 1].COLUMN.INDEX_REAL;
                if (last == dragHeader.i) dragHeader.im = -1;
                else dragHeader.im = last;
                Invalidate();
                return;
            }
            if (dragBody != null)
            {
                dragBody.SetEnable(e.X, e.Y);
                SetCursor(CursorType.SizeAll);
                dragBody.hand = true;
                dragBody.xr = e.Y - dragBody.x;
                if (rows == null) return;
                int yr = dragBody.x + dragBody.xr;
                dragBody.last = e.Y > dragBody.x;

                var cel_sel = CellContains(rows, false, e.X, yr, out int r_x, out int r_y, out int offset_x, out int offset_xi, out int offset_y, out int i_row, out int i_cel, out var column, out int mode);
                if (cel_sel != null)
                {
                    if (i_row == dragBody.i) dragBody.im = -1;
                    else dragBody.im = i_row;
                    Invalidate();
                    return;
                }
                int last_i = rows.Length - 1 - rowSummary;
                if (rows[last_i].INDEX == dragBody.i) dragBody.im = -1;
                else dragBody.im = rows[last_i].INDEX;
                Invalidate();
                return;
            }
            if (ScrollBar.MouseMoveY(e.Location) && ScrollBar.MouseMoveX(e.Location) && OnTouchMove(e.X, e.Y))
            {
                if (rows == null || inEditMode) return;
                var cel_sel = CellContains(rows, true, e.X, e.Y, out int r_x, out int r_y, out int offset_x, out int offset_xi, out int offset_y, out int i_row, out int i_cel, out var column, out int mode);
                if (cel_sel == null)
                {
                    MouseMoveCell(e);
                    foreach (RowTemplate it in rows)
                    {
                        if (it.IsColumn) continue;
                        it.Hover = false;
                        foreach (var cel_tmp in it.cells)
                        {
                            if (cel_tmp is TCellSort sort) sort.Hover = false;
                            else if (cel_tmp is Template template) ILeave(template);
                        }
                    }
                    SetCursor(false);
                }
                else
                {
                    MouseMoveCell(cel_sel, i_row, i_cel, column, offset_x, offset_y, e);
                    if (mode > 0)
                    {
                        for (int i = 1; i < rows.Length; i++)
                        {
                            rows[i].Hover = false;
                            foreach (var cel_tmp in rows[i].cells)
                            {
                                if (cel_tmp is Template template) ILeave(template);
                            }
                        }
                        var cel = (TCellColumn)cel_sel;
                        if (moveheaders.Length > 0)
                        {
                            foreach (var item in moveheaders)
                            {
                                if (item.rect.Contains(r_x, r_y))
                                {
                                    SetCursor(CursorType.VSplit);
                                    return;
                                }
                            }
                        }
                        if (has_check && cel.COLUMN is ColumnCheck columnCheck && columnCheck.NoTitle && cel.CONTAIN_REAL(r_x, r_y)) SetCursor(true);
                        else if (cel.COLUMN.Filter != null && cel.rect_filter.Contains(r_x - cel.offsetx, r_y - cel.offsetx)) SetCursor(true);
                        else if (cel.COLUMN.SortOrder) SetCursor(true);
                        else if (ColumnDragSort && cel.COLUMN.DragSort) SetCursor(CursorType.SizeAll);
                        else SetCursor(false);
                    }
                    else
                    {
                        int countmove = 0;
                        for (int i = 1; i < rows.Length; i++)
                        {
                            if (i == i_row)
                            {
                                if (cel_sel is TCellSort sort)
                                {
                                    sort.Hover = sort.Contains(r_x, r_y);
                                    if (sort.Hover) countmove++;
                                }
                                rows[i].Hover = true;
                            }
                            else
                            {
                                rows[i].Hover = false;
                                foreach (var cel_tmp in rows[i].cells)
                                {
                                    if (cel_tmp is TCellSort sort) sort.Hover = false;
                                    else if (cel_tmp is Template template) ILeave(template);
                                }
                            }
                        }
                        if (countmove > 0) SetCursor(CursorType.SizeAll);
                        else
                        {
                            if (cel_sel.ROW.CanExpand && cel_sel.ROW.RectExpand.Contains(r_x, r_y)) { SetCursor(true); return; }
                            SetCursor(MouseMoveRow(cel_sel, r_x, r_y, offset_x, offset_xi, offset_y, e));
                        }
                    }
                }
            }
            else ILeave();
        }

        bool MouseMoveRow(CELL cel, int x, int y, int offset_x, int offset_xi, int offset_y, MouseEventArgs e)
        {
            if (cel is TCellCheck checkCell)
            {
                if (checkCell.AutoCheck && checkCell.CONTAIN_REAL(x, y)) return true;
                return false;
            }
            else if (cel is TCellRadio radioCell)
            {
                if (radioCell.AutoCheck && radioCell.CONTAIN_REAL(x, y)) return true;
                return false;
            }
            else if (cel is TCellSwitch switchCell)
            {
                if ((switchCell.AutoCheck || (switchCell.COLUMN is ColumnSwitch columnSwitch && columnSwitch.Call != null)))
                {
                    switchCell.ExtraMouseHover = switchCell.CONTAIN_REAL(x, y);
                    if (switchCell.ExtraMouseHover) return true;
                }
                else switchCell.ExtraMouseHover = false;
                return false;
            }
            else if (cel is Template template)
            {
                ICell? tipcel = null;
                int hand = 0;
                foreach (var item in template.Value)
                {
                    if (item is CellLink btn_template)
                    {
                        if (btn_template.Enabled)
                        {
                            btn_template.ExtraMouseHover = btn_template.Rect.Contains(x, y);
                            if (btn_template.ExtraMouseHover)
                            {
                                hand++;
                                tipcel = btn_template;
                            }
                        }
                        else btn_template.ExtraMouseHover = false;
                    }
                    else if (item is CellImage img_template)
                    {
                        if (img_template.Tooltip != null && img_template.Rect.Contains(x, y)) tipcel = img_template;
                    }
                    else if (item is CellCheckbox checkbox_template)
                    {
                        if (checkbox_template.Enabled && checkbox_template.AutoCheck)
                        {
                            checkbox_template.ExtraMouseHover = checkbox_template.Rect.Contains(x, y);
                            if (checkbox_template.ExtraMouseHover) hand++;
                        }
                        else checkbox_template.ExtraMouseHover = false;
                    }
                    else if (item is CellRadio radio_template)
                    {
                        if (radio_template.Enabled && radio_template.AutoCheck)
                        {
                            radio_template.ExtraMouseHover = radio_template.Rect.Contains(x, y);
                            if (radio_template.ExtraMouseHover) hand++;
                        }
                        else radio_template.ExtraMouseHover = false;
                    }
                }
                if (tipcel == null) CloseTip();
                else
                {
                    if (tipcel is CellLink btn_template)
                    {
                        if (btn_template.Tooltip == null) CloseTip();
                        else
                        {
                            var _rect = RectangleToScreen(ClientRectangle);
                            var rect = new Rectangle(_rect.X + btn_template.Rect.X - offset_xi, _rect.Y + btn_template.Rect.Y - offset_y, btn_template.Rect.Width, btn_template.Rect.Height);
                            if (tooltipForm == null)
                            {
                                tooltipForm = new TooltipForm(this, rect, btn_template.Tooltip, TooltipConfig ?? new TooltipConfig
                                {
                                    Font = Font,
                                    ArrowAlign = TAlign.Top,
                                });
                                tooltipForm.Show(this);
                            }
                            else tooltipForm.SetText(rect, btn_template.Tooltip);
                        }
                    }
                    else if (tipcel is CellImage img_template)
                    {
                        if (img_template.Tooltip == null) CloseTip();
                        else
                        {
                            var _rect = RectangleToScreen(ClientRectangle);
                            var rect = new Rectangle(_rect.X + img_template.Rect.X - offset_xi, _rect.Y + img_template.Rect.Y - offset_y, img_template.Rect.Width, img_template.Rect.Height);
                            if (tooltipForm == null)
                            {
                                tooltipForm = new TooltipForm(this, rect, img_template.Tooltip, TooltipConfig ?? new TooltipConfig
                                {
                                    Font = Font,
                                    ArrowAlign = TAlign.Top,
                                });
                                tooltipForm.Show(this);
                            }
                            else tooltipForm.SetText(rect, img_template.Tooltip);
                        }
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
                    if (!cel.COLUMN.LineBreak && cel.MinWidth > cel.RECT_REAL.Width + 1)
                    {
                        var text = cel.ToString();
                        if (!string.IsNullOrEmpty(text))
                        {
                            var _rect = RectangleToScreen(ClientRectangle);
                            var rect = new Rectangle(_rect.X + cel.RECT.X - offset_xi, _rect.Y + cel.RECT.Y - offset_y, cel.RECT.Width, cel.RECT.Height);
                            if (tooltipForm == null)
                            {
                                tooltipForm = new TooltipForm(this, rect, text, TooltipConfig ?? new TooltipConfig
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

        void MouseMoveCell(CELL cel, int i_row, int i_cel, Column? column, int offset_x, int offset_y, MouseEventArgs e)
        {
            if (CellHover == null) return;
            var moveid = i_row + "_" + i_cel;
            if (oldmove2 == moveid) return;
            oldmove2 = moveid;
            CellHover(this, new TableHoverEventArgs(cel.ROW.RECORD, i_row, i_cel, column, new Rectangle(cel.RECT.X - offset_x, cel.RECT.Y - offset_y, cel.RECT.Width, cel.RECT.Height), e));
        }
        void MouseMoveCell(MouseEventArgs? e)
        {
            if (CellHover == null || oldmove2 == null) return;
            oldmove2 = null;
            CellHover(this, new TableHoverEventArgs(e ?? new MouseEventArgs(MouseButtons.None, 0, 0, 0, 0)));
        }

        string? oldmove, oldmove2;
        TooltipForm? tooltipForm;
        void CloseTip(bool clear = false)
        {
            tooltipForm?.IClose();
            tooltipForm = null;
            if (clear) oldmove = null;
        }

        #endregion

        #region 判断是否在内部

        CELL? CellContains(RowTemplate[] rows, bool sethover, int ex, int ey, out int r_x, out int r_y, out int offset_x, out int offset_xi, out int offset_y, out int i_row, out int i_cel, out Column? column, out int mode)
        {
            int sx = ScrollBar.ValueX, sy = ScrollBar.ValueY;
            int px = ex + sx, py = ey + sy;
            foreach (RowTemplate it in rows)
            {
                if (it.IsColumn)
                {
                    if (fixedHeader)
                    {
                        if (CellContainsFixed(it, ex, ey, sx, sy, px, py, out var tmp))
                        {
                            mode = 2;
                            r_x = tmp!.r_x;
                            r_y = tmp.r_y;
                            offset_x = tmp.offset_x;
                            offset_xi = tmp.offset_xi;
                            offset_y = tmp.offset_y;

                            i_row = tmp.i_row;
                            i_cel = tmp.i_cel;
                            column = tmp.col;
                            return tmp.cell;
                        }
                    }
                    else if (it.CONTAINS(ex, py))
                    {
                        if (CellContains(it, ex, ey, sx, sy, px, py, out var tmp))
                        {
                            mode = 1;
                            r_x = tmp!.r_x;
                            r_y = tmp.r_y;
                            offset_x = tmp.offset_x;
                            offset_xi = tmp.offset_xi;
                            offset_y = tmp.offset_y;

                            i_row = tmp.i_row;
                            i_cel = tmp.i_cel;
                            column = tmp.col;
                            return tmp.cell;
                        }
                    }
                }
                else if (it.Type == RowType.Summary) continue;
                else if (it.Contains(ex, py, sethover))
                {
                    if (CellContains(it, ex, ey, sx, sy, px, py, out var tmp))
                    {
                        mode = 0;
                        r_x = tmp!.r_x;
                        r_y = tmp.r_y;
                        offset_x = tmp.offset_x;
                        offset_xi = tmp.offset_xi;
                        offset_y = tmp.offset_y;

                        i_row = tmp.i_row;
                        i_cel = tmp.i_cel;
                        column = tmp.col;
                        return tmp.cell;
                    }
                }
            }
            mode = 0;
            r_x = r_y = offset_x = offset_xi = offset_y = i_row = i_cel = 0;
            column = null;
            return null;
        }

        bool CellContainsFixed(RowTemplate it, int ex, int ey, int sx, int sy, int px, int py, out ContainCellTMP? cell)
        {
            var hasi = new List<int>();
            if (showFixedColumnL && fixedColumnL != null)
            {
                foreach (var i in fixedColumnL)
                {
                    hasi.Add(i);
                    var cel = it.cells[i];
                    if (cel.CONTAIN(ex, ey))
                    {
                        cell = new ContainCellTMP(cel, ex, ey, 0, 0, sy, it.INDEX, i, cel.COLUMN);
                        return true;
                    }
                }
            }
            if (showFixedColumnR && fixedColumnR != null)
            {
                foreach (var i in fixedColumnR)
                {
                    hasi.Add(i);
                    var cel = it.cells[i];
                    if (cel.CONTAIN(ex + sFixedR, ey))
                    {
                        cell = new ContainCellTMP(cel, ex + sFixedR, ey, -sFixedR, sFixedR, sy, it.INDEX, i, cel.COLUMN);
                        return true;
                    }
                }
            }
            for (int i = 0; i < it.cells.Length; i++)
            {
                if (hasi.Contains(i)) continue;
                var cel = it.cells[i];
                if (cel.CONTAIN(px, ey))
                {
                    cell = new ContainCellTMP(cel, px, ey, sx, sx, sy, it.INDEX, i, cel.COLUMN);
                    return true;
                }
            }
            cell = null;
            return false;
        }

        bool CellContains(RowTemplate it, int ex, int ey, int sx, int sy, int px, int py, out ContainCellTMP? cell)
        {
            var hasi = new List<int>();
            if (showFixedColumnL && fixedColumnL != null)
            {
                foreach (var i in fixedColumnL)
                {
                    hasi.Add(i);
                    var cel = it.cells[i];
                    if (cel.CONTAIN(ex, py))
                    {
                        cell = new ContainCellTMP(cel, ex, py, 0, 0, sy, it.INDEX, i, cel.COLUMN);
                        return true;
                    }
                }
            }
            if (showFixedColumnR && fixedColumnR != null)
            {
                foreach (var i in fixedColumnR)
                {
                    hasi.Add(i);
                    var cel = it.cells[i];
                    if (cel.CONTAIN(ex + sFixedR, py))
                    {
                        cell = new ContainCellTMP(cel, ex + sFixedR, py, -sFixedR, sFixedR, sy, it.INDEX, i, cel.COLUMN);
                        return true;
                    }
                }
            }
            for (int i = 0; i < it.cells.Length; i++)
            {
                if (hasi.Contains(i)) continue;
                var cel = it.cells[i];
                if (cel.CONTAIN(px, py))
                {
                    cell = new ContainCellTMP(cel, px, py, sx, sx, sy, it.INDEX, i, cel.COLUMN);
                    return true;
                }
            }
            cell = null;
            return false;
        }

        class ContainCellTMP
        {
            public ContainCellTMP(CELL _cell, int _r_x, int _r_y, int _offset_x, int _offset_xi, int _offset_y, int _i_row, int _i_cel, Column _col)
            {
                cell = _cell;
                r_x = _r_x;
                r_y = _r_y;
                offset_x = _offset_x;
                offset_xi = _offset_xi;
                offset_y = _offset_y;
                i_row = _i_row;
                i_cel = _i_cel;
                col = _col;
            }

            public CELL cell { get; set; }

            public int r_x { get; set; }
            public int r_y { get; set; }
            public int offset_x { get; set; }
            public int offset_xi { get; set; }
            public int offset_y { get; set; }
            public int i_row { get; set; }
            public int i_cel { get; set; }
            public Column col { get; set; }
        }

        #endregion

        DownCellTMP<CELL>? cellMouseDown;
        DownCellTMP<CellLink>? btnMouseDown;
        class DownCellTMP<T>
        {
            public DownCellTMP(RowTemplate _row, T _cell, int _i_row, int _i_cel, bool _doubleClick)
            {
                row = _row;
                cell = _cell;
                i_row = _i_row;
                i_cel = _i_cel;
                doubleClick = _doubleClick;
            }
            public bool doubleClick { get; set; }
            public T cell { get; set; }
            public RowTemplate row { get; set; }
            public int i_row { get; set; }
            public int i_cel { get; set; }
        }

        #endregion

        DragHeader? dragHeader;
        DragHeader? dragBody;

        #region 排序

        int[]? SortHeader;
        int[]? SortData;
        List<SortModel> SortDatas(Column column)
        {
            if (dataTmp == null) return new List<SortModel>(0);
            var list = new List<SortModel>(dataTmp.rows.Length);
            if (dataTmp.rows[0].cells.ContainsKey(column.Key))
            {
                for (int i_r = 0; i_r < dataTmp.rows.Length; i_r++) list.Add(new SortModel(i_r, OGetValue(dataTmp, i_r, column.Key)?.ToString()));
            }
            else if (column.Render == null) return list;
            else
            {
                for (int i_r = 0; i_r < dataTmp.rows.Length; i_r++)
                {
                    var obj = column.Render(null, dataTmp.rows[i_r].record, i_r);
                    list.Add(new SortModel(i_r, obj?.ToString()));
                }
            }
            return list;
        }

        void SortDataASC(Column column)
        {
            var list = SortDatas(column);
            if (CustomSort == null) list.Sort((x, y) => FilesNameComparerClass.Compare(x.v, y.v));
            else list.Sort((x, y) => CustomSort(x.v, y.v));
            var SortTmp = new List<int>(list.Count);
            foreach (var it in list) SortTmp.Add(it.i);
            SortData = SortTmp.ToArray();
        }
        void SortDataDESC(Column column)
        {
            var list = SortDatas(column);
            if (CustomSort == null) list.Sort((y, x) => FilesNameComparerClass.Compare(x.v, y.v));
            else list.Sort((y, x) => CustomSort(x.v, y.v));
            var SortTmp = new List<int>(list.Count);
            foreach (var it in list) SortTmp.Add(it.i);
            SortData = SortTmp.ToArray();
        }

        #endregion

        bool focused = false;
        protected override void OnGotFocus(EventArgs e)
        {
            focused = true;
            base.OnGotFocus(e);
        }
        protected override void OnLostFocus(EventArgs e)
        {
            focused = false;
            if (LostFocusClearSelection) SelectedIndex = -1;
            CloseTip(true);
            base.OnLostFocus(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            ScrollBar.Leave();
            ILeave();
            CloseTip(true);
        }
        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            ScrollBar.Leave();
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
                    if (cel is TCellSort sort) sort.Hover = false;
                    else if (cel is Template template) ILeave(template);
                }
            }
            MouseMoveCell(null);
        }

        void ILeave(Template template)
        {
            foreach (var it in template.Value)
            {
                if (it is CellLink btn) btn.ExtraMouseHover = false;
                else if (it is CellCheckbox checkbox) checkbox.ExtraMouseHover = false;
                else if (it is CellRadio radio) radio.ExtraMouseHover = false;
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            subForm?.IClose();
            subForm = null;
            CloseTip();
            ScrollBar.MouseWheel(e);
            base.OnMouseWheel(e);
        }
        protected override bool OnTouchScrollX(int value) => ScrollBar.MouseWheelXCore(value);
        protected override bool OnTouchScrollY(int value) => ScrollBar.MouseWheelYCore(value);
    }
}