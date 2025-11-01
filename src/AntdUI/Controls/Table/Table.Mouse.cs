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
// GITCODE: https://gitcode.com/AntdUI/AntdUI
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
            CloseTip();
            if (ScrollBar.MouseDownY(e.X, e.Y) && ScrollBar.MouseDownX(e.X, e.Y))
            {
                base.OnMouseDown(e);
                if (rows == null) return;
                OnTouchDown(e.X, e.Y);
                var db = CellContains(rows, true, e.X, e.Y);
                if (db == null)
                {
                    FocusedCell = null;
                    return;
                }
                else
                {
                    var style = CellFocusedStyle ?? Config.DefaultCellFocusedStyle;
                    if (style == TableCellFocusedStyle.None) FocusedCell = null;
                    else FocusedCell = db.cell;
                    if (e.Button == MouseButtons.Left)
                    {
                        if (MultipleRows && ModifierKeys.HasFlag(Keys.Shift))
                        {
                            if (shift_index == -1) SelectedIndexs = SetIndexs(db.i_row);
                            else
                            {
                                if (shift_index > db.i_row) SelectedIndexs = SetIndexs(db.i_row, shift_index);
                                else SelectedIndexs = SetIndexs(shift_index, db.i_row);
                            }
                        }
                        else if (MultipleRows && ModifierKeys.HasFlag(Keys.Control)) SelectedIndexs = SetIndexs(db.i_row);
                        else SelectedIndex = db.i_row;
                    }
                    shift_index = db.i_row;
                    if (dataSource is BindingSource bindingSource) bindingSource.Position = db.i_row - 1;
                    var it = db.cell.ROW;
                    if (db.mode > 0)
                    {
                        if (moveheaders.Length > 0)
                        {
                            foreach (var item in moveheaders)
                            {
                                if (item.rect.Contains(db.x, db.y))
                                {
                                    item.x = e.X;
                                    Window.CanHandMessage = false;
                                    item.MouseDown = true;
                                    return;
                                }
                            }
                        }
                        cellMouseDown = new DownCellTMP<CELL>(it, db.cell, db, e.Clicks > 1);
                        if (!cellMouseDown.doubleClick && db.col is ColumnCheck columnCheck && columnCheck.NoTitle)
                        {
                            if (e.Button == MouseButtons.Left && db.cell.CONTAIN_REAL(db.x, db.y))
                            {
                                CheckAll(db.i_cel, columnCheck, !columnCheck.Checked);
                                return;
                            }
                        }

                        if (db.cell is TCellColumn cellColumn && (cellColumn.rect_up.Contains(db.x - db.offset_x, db.y - db.offset_xi) ||
                            cellColumn.rect_down.Contains(db.x - db.offset_x, db.y - db.offset_xi) ||
                            (db.col.Filter != null && cellColumn.rect_filter.Contains(db.x - db.offset_x, db.y - db.offset_xi)))) return;
                        if (ColumnDragSort && db.col.DragSort)
                        {
                            dragHeader = new DragHeader(e.X, e.Y, db.col.INDEX_REAL, e.X);
                            return;
                        }
                    }
                    else
                    {
                        if (db.col is ColumnSort sort && db.cell.CONTAIN_REAL(db.x, db.y))
                        {
                            dragBody = new DragHeader(e.X, e.Y, db.cell.ROW.INDEX, e.Y);
                            return;
                        }
                        if (db.cell.ROW.CanExpand && db.cell.ROW.RECORD != null && db.cell.ROW.RectExpand.Contains(db.x, db.y))
                        {
                            if (db.cell.ROW.Expand) rows_Expand.Remove(db.cell.ROW.RECORD);
                            else rows_Expand.Add(db.cell.ROW.RECORD);
                            ExpandChanged?.Invoke(this, new TableExpandEventArgs(db.cell.ROW.RECORD, !db.cell.ROW.Expand));
                            if (LoadLayout()) Invalidate();
                            return;
                        }
                        MouseDownRow(e, it, db);
                    }
                }
            }
        }

        void MouseDownRow(MouseEventArgs e, RowTemplate it, CELLDB db)
        {
            cellMouseDown = new DownCellTMP<CELL>(it, db.cell, db, e.Clicks > 1);
            if (focusedCell != null) Invalidate(focusedCell.ROW.RECT);//同行切换单元格时，及时刷新
            if (db.cell is Template template)
            {
                if (e.Button == MouseButtons.Left)
                {
                    foreach (var item in template.Value)
                    {
                        if (item is CellLink btn_template)
                        {
                            if (btn_template.Enabled)
                            {
                                if (btn_template.Rect.Contains(db.x, db.y))
                                {
                                    btnMouseDown = new DownCellTMP<CellLink>(it, btn_template, db, cellMouseDown.doubleClick);
                                    btn_template.ExtraMouseDown = true;
                                    OnCellButtonDown(btn_template, it.RECORD, db.i_row, db.i_cel, db.col, RealRect(btn_template.Rect, db.offset_xi, db.offset_y), e);
                                    return;
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (var item in template.Value)
                    {
                        if (item is CellLink btn_template)
                        {
                            if (btn_template.Enabled)
                            {
                                if (btn_template.Rect.Contains(db.x, db.y))
                                {
                                    btnMouseDown = new DownCellTMP<CellLink>(it, btn_template, db, cellMouseDown.doubleClick);
                                    OnCellButtonDown(btn_template, it.RECORD, db.i_row, db.i_cel, db.col, RealRect(btn_template.Rect, db.offset_xi, db.offset_y), e);
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
                        OnSortRowsTree(record, d, from, to);
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
                        OnSortRows(-1);
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
                    if (cellMDown == null)
                    {
                        EditModeClose();
                        return;
                    }
                    MouseUpRow(rows, cellMDown, btnMDown, e);
                }
                else
                {
                    if (btnMDown == null)
                    {
                        EditModeClose();
                        return;
                    }
                    if (btnMDown.cell.ExtraMouseDown)
                    {
                        OnCellButtonUp(btnMDown.cell, btnMDown.row.RECORD, btnMDown.i_row, btnMDown.i_cel, btnMDown.cell.PARENT.COLUMN, RealRect(btnMDown), e);
                        btnMDown.cell.ExtraMouseDown = false;
                    }
                }
            }
        }

        void filter_PopupEndEventMethod(object sender, CancelEventArgs e)
        {
            if (FilterPopupEnd != null)
            {
                if (sender is Popover.Config config)
                {
                    var arg = new TableFilterPopupEndEventArgs(config.Tag is FilterOption option ? option : null, FilterList());
                    FilterPopupEnd(sender, arg);
                    e.Cancel = arg.Cancel;
                }
            }
            if (e.Cancel == false)
            {
                inEditMode = false;
                OnMouseLeave(EventArgs.Empty);
            }
        }

        void MouseUpRow(RowTemplate[] rows, DownCellTMP<CELL> it, DownCellTMP<CellLink>? btn, MouseEventArgs e)
        {
            var db = CellContains(rows, true, e.X, e.Y);
            if (db == null) MouseUpBtn(it, btn, e);
            else if (it.i_row != db.i_row || it.i_cel != db.i_cel) MouseUpBtn(it, btn, e, db);
            else
            {
                if (selectedIndex.Length == 1) SelectedIndex = it.i_row;
                if (MouseUpBtn(it, btn, e, db)) return;
                if (e.Button == MouseButtons.Left)
                {
                    if (it.cell is TCellCheck checkCell)
                    {
                        if (checkCell.CONTAIN_REAL(db.x, db.y))
                        {
                            if (checkCell.COLUMN is ColumnCheck columnCheck && columnCheck.Call != null)
                            {
                                var value = columnCheck.Call(!checkCell.Checked, it.row.RECORD, it.i_row, it.i_cel);
                                if (checkCell.Checked != value)
                                {
                                    checkCell.Checked = value;
                                    SetValue(it.cell, checkCell.Checked);
                                    OnCheckedChanged(checkCell.Checked, it.row.RECORD, it.i_row, it.i_cel, db.col);
                                }
                            }
                            else if (checkCell.AutoCheck)
                            {
                                checkCell.Checked = !checkCell.Checked;
                                SetValue(it.cell, checkCell.Checked);
                                OnCheckedChanged(checkCell.Checked, it.row.RECORD, it.i_row, it.i_cel, db.col);
                            }
                        }
                    }
                    else if (it.cell is TCellRadio radioCell)
                    {
                        if (radioCell.CONTAIN_REAL(db.x, db.y) && !radioCell.Checked)
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
                                OnCheckedChanged(radioCell.Checked, it.row.RECORD, it.i_row, it.i_cel, db.col);
                            }
                        }
                    }
                    else if (it.cell is TCellSwitch switchCell)
                    {
                        if (switchCell.CONTAIN_REAL(db.x, db.y) && !switchCell.Loading)
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
                                OnCheckedChanged(switchCell.Checked, it.row.RECORD, it.i_row, it.i_cel, db.col);
                            }
                        }
                    }
                    else if (it.cell is Template template)
                    {
                        foreach (var item in template.Value)
                        {
                            if (item is CellCheckbox checkbox)
                            {
                                if (checkbox.Rect.Contains(db.x, db.y) && checkbox.Enabled && checkbox.AutoCheck) checkbox.Checked = !checkbox.Checked;
                            }
                            else if (item is CellRadio radio)
                            {
                                if (radio.Rect.Contains(db.x, db.y) && radio.Enabled && radio.AutoCheck) radio.Checked = !radio.Checked;
                            }
                            else if (item is CellSwitch _switch)
                            {
                                if (_switch.Rect.Contains(db.x, db.y) && _switch.Enabled && _switch.AutoCheck) _switch.Checked = !_switch.Checked;
                            }
                        }
                    }
                    else if (it.row.IsColumn && it.cell is TCellColumn col)
                    {
                        if (it.cell.COLUMN.Filter != null && col.rect_filter.Contains(db.x - col.offsetx, db.y - col.offsety))
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
                            fnt ??= Font;
                            var editor = new FilterControl(this, focusColumn, customSource)
                            {
                                Font = fnt
                            };
                            if (filterHeight > 0) editor.Height = filterHeight;
                            Point location = PointToScreen(col.rect_filter.Location);
                            Point locaionOrigin = location;
                            location.X -= (focusColumn.Fixed ? 0 : ScrollBar.ValueX);
                            if (fixedColumnR != null && fixedColumnR.Contains(Columns.IndexOf(focusColumn)))
                            {
                                int gap = (int)(_gap.Width * Config.Dpi);
                                location.X -= (showFixedColumnR ? gap : gap * 2);
                            }
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
                            int r_x_f = db.x - col.offsetx, r_y_f = db.y - col.offsety;
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
                                var result = OnSortModeChanged(sortMode, col.COLUMN);
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
                                    OnSortRows(it.i_cel);
                                }
                            }
                        }
                    }
                }
                bool enterEdit = false;
                if (it.doubleClick)
                {
                    OnCellDoubleClick(it.row.RECORD, db.i_row, db.i_cel, db.col, RealRect(db.cell.RECT, db.offset_xi, db.offset_y), e);
                    if (e.Button == MouseButtons.Left && editmode == TEditMode.DoubleClick) enterEdit = true;
                }
                else
                {
                    OnCellClick(it.row.RECORD, db.i_row, db.i_cel, db.col, RealRect(db.cell.RECT, db.offset_xi, db.offset_y), e);
                    if (e.Button == MouseButtons.Left && editmode == TEditMode.Click) enterEdit = true;
                }
                if (enterEdit)
                {
                    EditModeClose();
                    int i_row = db.i_row, i_cel = db.i_cel;
                    db.cell = RealCELL(db.cell, rows, ref i_row, ref i_cel, ref it, out var crect);
                    if (CanEditMode(db.cell))
                    {
                        int val = ScrollLine(i_row, rows);
                        OnEditMode(it.row, db.cell, crect, i_row, i_cel, db.col, db.offset_xi, db.offset_y - val);
                    }
                }
            }
        }
        bool MouseUpBtn(DownCellTMP<CELL> it, DownCellTMP<CellLink>? btn, MouseEventArgs e, CELLDB db)
        {
            if (btn == null) return false;
            btn.cell.ExtraMouseDown = false;
            if (e.Button == MouseButtons.Left && btn.cell.Rect.Contains(db.x, db.y))
            {
                btn.cell.Click();

                if (btn.cell.DropDownItems != null && btn.cell.DropDownItems.Count > 0)
                {
                    subForm?.IClose();
                    subForm = null;
                    var rect = btn.cell.Rect;
                    rect.Offset(-db.offset_xi, -db.offset_y);
                    subForm = new LayeredFormSelectDown(this, btn.cell.DropDownItems, btn.cell, rect);
                    subForm.Show(this);
                }
                OnCellButtonUp(btn.cell, it.row.RECORD, it.i_row, it.i_cel, db.col, RealRect(btn.cell.Rect, db.offset_xi, db.offset_y), e);
                OnCellButtonClick(btn.cell, it.row.RECORD, it.i_row, it.i_cel, db.col, RealRect(btn.cell.Rect, db.offset_xi, db.offset_y), e);
                return true;
            }
            OnCellButtonUp(btn.cell, it.row.RECORD, it.i_row, it.i_cel, db.col, RealRect(btn.cell.Rect, db.offset_xi, db.offset_y), e);
            return false;
        }
        bool MouseUpBtn(DownCellTMP<CELL> it, DownCellTMP<CellLink>? btn, MouseEventArgs e)
        {
            if (btn == null) return false;
            btn.cell.ExtraMouseDown = false;
            OnCellButtonUp(btn.cell, it.row.RECORD, it.i_row, it.i_cel, btn.col, RealRect(btn.cell.Rect, it.offset_xi, it.offset_y), e);
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

                var db = CellContains(rows, false, xr, e.Y);
                if (db != null)
                {
                    if (db.col.INDEX_REAL == dragHeader.i) dragHeader.im = -1;
                    else dragHeader.im = db.col.INDEX_REAL;
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

                var db = CellContains(rows, false, e.X, yr);
                if (db != null)
                {
                    if (db.i_row == dragBody.i) dragBody.im = -1;
                    else dragBody.im = db.i_row;
                    Invalidate();
                    return;
                }
                int last_i = rows.Length - 1 - rowSummary;
                if (rows[last_i].INDEX == dragBody.i) dragBody.im = -1;
                else dragBody.im = rows[last_i].INDEX;
                Invalidate();
                return;
            }
            if (ScrollBar.MouseMoveY(e.X, e.Y) && ScrollBar.MouseMoveX(e.X, e.Y) && OnTouchMove(e.X, e.Y))
            {
                if (rows == null || inEditMode) return;
                var db = CellContains(rows, true, e.X, e.Y);
                if (db == null)
                {
                    foreach (RowTemplate it in rows)
                    {
                        if (it.IsColumn) continue;
                        hovers = -1;
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
                    hovers = db.cell.ROW.INDEX_REAL;
                    if (db.mode > 0)
                    {
                        for (int i = 1; i < rows.Length; i++)
                        {
                            rows[i].Hover = false;
                            foreach (var cel_tmp in rows[i].cells)
                            {
                                if (cel_tmp is Template template) ILeave(template);
                            }
                        }
                        var cel = (TCellColumn)db.cell;
                        if (moveheaders.Length > 0)
                        {
                            foreach (var item in moveheaders)
                            {
                                if (item.rect.Contains(db.x, db.y))
                                {
                                    SetCursor(CursorType.VSplit);
                                    return;
                                }
                            }
                        }
                        if (has_check && cel.COLUMN is ColumnCheck columnCheck && columnCheck.NoTitle && cel.CONTAIN_REAL(db.x, db.y)) SetCursor(true);
                        else if (cel.COLUMN.Filter != null && cel.rect_filter.Contains(db.x - cel.offsetx, db.y - cel.offsetx)) SetCursor(true);
                        else if (cel.COLUMN.SortOrder) SetCursor(true);
                        else if (ColumnDragSort && cel.COLUMN.DragSort) SetCursor(CursorType.SizeAll);
                        else SetCursor(false);
                    }
                    else
                    {
                        int countmove = 0;
                        for (int i = 1; i < rows.Length; i++)
                        {
                            var row = rows[i];
                            if (row.INDEX == db.i_row)
                            {
                                if (db.cell is TCellSort sort)
                                {
                                    sort.Hover = sort.Contains(db.x, db.y);
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
                            if (db.cell.ROW.CanExpand && db.cell.ROW.RectExpand.Contains(db.x, db.y)) { SetCursor(true); return; }
                            SetCursor(MouseMoveRow(db, e));
                        }
                    }
                }
            }
            else ILeave();
        }

        #region 鼠标悬浮

        protected override bool CanMouseMove { get; set; } = true;
        protected override void OnMouseHover(int x, int y)
        {
            if (x == -1 || y == -1)
            {
                CloseTip();
                return;
            }
            if (rows == null || inEditMode) return;
            var db = CellContains(rows, false, x, y);
            if (db == null) OnCellHover();
            else
            {
                OnCellHover(db.cell.ROW.RECORD, db.i_row, db.i_cel, db.col, RealRect(db.cell.RECT, db.offset_xi, db.offset_y), new MouseEventArgs(MouseButtons.None, 0, x, y, 0));
                if (db.mode == 0) MouseHoverRow(db);
            }
        }

        bool MouseMoveRow(CELLDB db, MouseEventArgs e)
        {
            if (db.cell is TCellCheck checkCell)
            {
                if (checkCell.AutoCheck && checkCell.CONTAIN_REAL(db.x, db.y)) return true;
                return false;
            }
            else if (db.cell is TCellRadio radioCell)
            {
                if (radioCell.AutoCheck && radioCell.CONTAIN_REAL(db.x, db.y)) return true;
                return false;
            }
            else if (db.cell is TCellSwitch switchCell)
            {
                if ((switchCell.AutoCheck || (switchCell.COLUMN is ColumnSwitch columnSwitch && columnSwitch.Call != null)))
                {
                    switchCell.ExtraMouseHover = switchCell.CONTAIN_REAL(db.x, db.y);
                    if (switchCell.ExtraMouseHover) return true;
                }
                else switchCell.ExtraMouseHover = false;
                return false;
            }
            else if (db.cell is Template template)
            {
                int hand = 0;
                foreach (var item in template.Value)
                {
                    if (item is CellLink btn_template)
                    {
                        if (btn_template.Enabled)
                        {
                            btn_template.ExtraMouseHover = btn_template.Rect.Contains(db.x, db.y);
                            if (btn_template.ExtraMouseHover) hand++;
                        }
                        else btn_template.ExtraMouseHover = false;
                    }
                    else if (item is CellCheckbox checkbox_template)
                    {
                        if (checkbox_template.Enabled && checkbox_template.AutoCheck)
                        {
                            checkbox_template.ExtraMouseHover = checkbox_template.Rect.Contains(db.x, db.y);
                            if (checkbox_template.ExtraMouseHover) hand++;
                        }
                        else checkbox_template.ExtraMouseHover = false;
                    }
                    else if (item is CellRadio radio_template)
                    {
                        if (radio_template.Enabled && radio_template.AutoCheck)
                        {
                            radio_template.ExtraMouseHover = radio_template.Rect.Contains(db.x, db.y);
                            if (radio_template.ExtraMouseHover) hand++;
                        }
                        else radio_template.ExtraMouseHover = false;
                    }
                    else if (item is CellSwitch switch_template)
                    {
                        if (switch_template.Enabled && switch_template.AutoCheck)
                        {
                            switch_template.ExtraMouseHover = switch_template.Rect.Contains(db.x, db.y);
                            if (switch_template.ExtraMouseHover) hand++;
                        }
                        else switch_template.ExtraMouseHover = false;
                    }
                }
                return hand > 0;
            }
            return false;
        }
        bool MouseHoverRow(CELLDB db)
        {
            if (db.cell is TCellCheck) return false;
            else if (db.cell is TCellRadio) return false;
            else if (db.cell is TCellSwitch) return false;
            else if (db.cell is Template template)
            {
                var tipcel = MouseHoverCell(template, db.x, db.y);
                if (tipcel == null) CloseTip();
                else
                {
                    if (tipcel is CellLink btn_template)
                    {
                        if (btn_template.Tooltip == null) CloseTip();
                        else OpenTip(RealRect(btn_template.Rect, db.offset_xi, db.offset_y), btn_template.Tooltip);
                    }
                    else if (tipcel is CellImage img_template)
                    {
                        if (img_template.Tooltip == null) CloseTip();
                        else OpenTip(RealRect(img_template.Rect, db.offset_xi, db.offset_y), img_template.Tooltip);
                    }
                }
            }
            else if (ShowTip)
            {
                var text = db.cell.ToString();
                if (!string.IsNullOrEmpty(text) && !db.col.LineBreak && db.cell.MinWidth > db.cell.RECT_REAL.Width + 1) OpenTip(RealRect(db.cell.RECT_REAL, db.offset_xi, db.offset_y), text);
                else CloseTip();
            }
            return false;
        }
        ICell? MouseHoverCell(Template template, int x, int y)
        {
            foreach (var item in template.Value)
            {
                if (item is CellLink btn_template)
                {
                    if (btn_template.Enabled && btn_template.Rect.Contains(x, y)) return btn_template;
                }
                else if (item is CellImage img_template)
                {
                    if (img_template.Rect.Contains(x, y)) return img_template;
                }
            }
            return null;
        }

        #region Tip

        TooltipForm? toolTip;

        public void CloseTip()
        {
            toolTip?.IClose();
            toolTip = null;
        }

        public void OpenTip(Rectangle rect, string tooltip, TooltipConfig? config = null)
        {
            if (toolTip == null)
            {
                toolTip = new TooltipForm(this, rect, tooltip, config ?? TooltipConfig ?? new TooltipConfig
                {
                    Font = Font,
                    ArrowAlign = TAlign.Top,
                }, true);
                toolTip.Show(this);
            }
            else if (toolTip.SetText(rect, tooltip))
            {
                CloseTip();
                OpenTip(rect, tooltip);
            }
        }

        #endregion

        #endregion

        #endregion

        #region 判断是否在内部

        CELLDB? CellContains(RowTemplate[] rows, bool sethover, int ex, int ey)
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
                            tmp!.mode = 2;
                            return tmp;
                        }
                    }
                    else if (it.CONTAINS(ex, py))
                    {
                        if (CellContains(it, ex, ey, sx, sy, px, py, out var tmp))
                        {
                            tmp!.mode = 1;
                            return tmp;
                        }
                    }
                }
                else if (it.Type == RowType.Summary) continue;
                else if (it.Contains(ex, py, sethover))
                {
                    if (sethover) hovers = it.INDEX_REAL;
                    if (CellContains(it, ex, ey, sx, sy, px, py, out var tmp))
                    {
                        tmp!.mode = 0;
                        return tmp;
                    }
                }
            }
            return null;
        }

        bool CellContainsFixed(RowTemplate it, int ex, int ey, int sx, int sy, int px, int py, out CELLDB? cell)
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
                        cell = new CELLDB(cel, ex, ey, 0, 0, sy, it.INDEX, i, cel.COLUMN);
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
                        cell = new CELLDB(cel, ex + sFixedR, ey, -sFixedR, sFixedR, sy, it.INDEX, i, cel.COLUMN);
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
                    cell = new CELLDB(cel, px, ey, sx, sx, sy, it.INDEX, i, cel.COLUMN);
                    return true;
                }
            }
            cell = null;
            return false;
        }

        bool CellContains(RowTemplate it, int ex, int ey, int sx, int sy, int px, int py, out CELLDB? cell)
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
                        cell = new CELLDB(cel, ex, py, 0, 0, sy, it.INDEX, i, cel.COLUMN);
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
                        cell = new CELLDB(cel, ex + sFixedR, py, -sFixedR, sFixedR, sy, it.INDEX, i, cel.COLUMN);
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
                    cell = new CELLDB(cel, px, py, sx, sx, sy, it.INDEX, i, cel.COLUMN);
                    return true;
                }
            }
            cell = null;
            return false;
        }

        #endregion

        DownCellTMP<CELL>? cellMouseDown;
        DownCellTMP<CellLink>? btnMouseDown;
        class DownCellTMP<T>
        {
            public DownCellTMP(RowTemplate _row, T _cell, CELLDB db, bool _doubleClick)
            {
                row = _row;
                cell = _cell;
                i_row = db.i_row;
                i_cel = db.i_cel;
                offset_x = db.offset_x;
                offset_xi = db.offset_xi;
                offset_y = db.offset_y;
                col = db.col;
                doubleClick = _doubleClick;
            }
            public bool doubleClick { get; set; }
            public T cell { get; set; }
            public RowTemplate row { get; set; }
            public int i_row { get; set; }
            public int i_cel { get; set; }
            public int offset_x { get; set; }
            public int offset_xi { get; set; }
            public int offset_y { get; set; }
            public Column col { get; set; }
        }

        Rectangle RealRect(DownCellTMP<CellLink> link) => RealRect(link.cell.Rect, link.offset_xi, link.offset_y);
        Rectangle RealRect(Rectangle rect, int ox, int oy) => new Rectangle(rect.X - ox, rect.Y - oy, rect.Width, rect.Height);

        CELL RealCELL(CELL cell, RowTemplate[] rows, ref int i_row, ref int i_cel, ref DownCellTMP<CELL> it, out Rectangle rect)
        {
            if (CellRanges == null || CellRanges.Length == 0)
            {
                rect = cell.RECT;
                return cell;
            }
            foreach (var range in CellRanges)
            {
                if (range.IsInRange(i_row - 1, i_cel))
                {
                    try
                    {
                        RowTemplate FirstRow = rows[range.FirstRow + 1], LastRow = rows[range.LastRow + 1];
                        CELL FirstCell = FirstRow.cells[range.FirstColumn], LastCell = LastRow.cells[range.LastColumn];
                        rect = RectMergeCells(FirstCell, LastCell, out _);
                        i_row = range.FirstRow + 1;
                        i_cel = range.FirstColumn;
                        it.row = FirstRow;
                        return FirstCell;
                    }
                    catch { }
                }
            }
            rect = cell.RECT;
            return cell;
        }
        CELL RealCELL(CELL cell, RowTemplate[] rows, int i_row, int i_cel, out Rectangle rect)
        {
            if (CellRanges == null || CellRanges.Length == 0)
            {
                rect = cell.RECT;
                return cell;
            }
            foreach (var range in CellRanges)
            {
                if (range.IsInRange(i_row - 1, i_cel))
                {
                    try
                    {
                        RowTemplate FirstRow = rows[range.FirstRow + 1], LastRow = rows[range.LastRow + 1];
                        CELL FirstCell = FirstRow.cells[range.FirstColumn], LastCell = LastRow.cells[range.LastColumn];
                        rect = RectMergeCells(FirstCell, LastCell, out _);
                        return FirstCell;
                    }
                    catch { }
                }
            }
            rect = cell.RECT;
            return cell;
        }

        #endregion

        DragHeader? dragHeader;
        DragHeader? dragBody;

        #region 排序

        int[]? SortHeader;
        int[]? SortData;
        List<SortModel> SortDatas(Column column)
        {
            if (dataTmp == null || dataTmp.rows.Length == 0) return new List<SortModel>(0);
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

        protected override void OnLostFocus(EventArgs e)
        {
            if (LostFocusClearSelection) SelectedIndex = -1;
            CloseTip();
            base.OnLostFocus(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (RectangleToScreen(ClientRectangle).Contains(MousePosition)) return;
            ScrollBar.Leave();
            ILeave();
        }
        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            ScrollBar.Leave();
            ILeave();
        }

        void ILeave()
        {
            SetCursor(false);
            if (rows == null || inEditMode) return;
            hovers = -1;
            foreach (var it in rows)
            {
                it.Hover = false;
                foreach (var cel in it.cells)
                {
                    if (cel is TCellSort sort) sort.Hover = false;
                    else if (cel is Template template) ILeave(template);
                }
            }
            CloseTip();
            OnCellHover();
        }

        void ILeave(Template template)
        {
            foreach (var it in template.Value)
            {
                if (it is CellLink btn) btn.ExtraMouseHover = false;
                else if (it is CellCheckbox checkbox) checkbox.ExtraMouseHover = false;
                else if (it is CellRadio radio) radio.ExtraMouseHover = false;
                else if (it is CellSwitch _switch) _switch.ExtraMouseHover = false;
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