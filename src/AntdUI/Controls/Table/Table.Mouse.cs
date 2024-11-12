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
using System.Drawing;
using System.Windows.Forms;

namespace AntdUI
{
    partial class Table
    {
        #region 鼠标按下

        TCell? cellMouseDown = null;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            cellMouseDown = null;
            if (ClipboardCopy) Focus();
            if (ScrollBar.MouseDownY(e.Location) && ScrollBar.MouseDownX(e.Location))
            {
                base.OnMouseDown(e);
                if (rows == null) return;
                OnTouchDown(e.X, e.Y);
                var cel_sel = CellContains(rows, e.X, e.Y, out int r_x, out int r_y, out _, out _, out _, out int i_row, out int i_cel, out int mode);
                if (cel_sel == null) return;
                else
                {
                    SelectedIndex = i_row;
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
                        var cell = it.cells[i_cel];
                        cell.MouseDown = e.Clicks > 1 ? 2 : 1;
                        cellMouseDown = cell;
                        if (cell.MouseDown == 1 && cell.COLUMN is ColumnCheck columnCheck && columnCheck.NoTitle)
                        {
                            if (e.Button == MouseButtons.Left && cell.CONTAIN_REAL(r_x, r_y))
                            {
                                CheckAll(i_cel, columnCheck, !columnCheck.Checked);
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
                            return;
                        }
                    }
                    else
                    {
                        if (cel_sel.ROW.CanExpand && cel_sel.ROW.RECORD != null && cel_sel.ROW.RectExpand.Contains(r_x, r_y))
                        {
                            if (cel_sel.ROW.Expand) rows_Expand.Remove(cel_sel.ROW.RECORD);
                            else rows_Expand.Add(cel_sel.ROW.RECORD);
                            LoadLayout();
                            Invalidate();
                            return;
                        }
                        MouseDownRow(e, it.cells[i_cel], r_x, r_y);
                    }
                }
            }
        }

        void MouseDownRow(MouseEventArgs e, TCell cell, int x, int y)
        {
            cellMouseDown = cell;
            cell.MouseDown = e.Clicks > 1 ? 2 : 1;
            if (cell is Template template && e.Button == MouseButtons.Left)
            {
                foreach (var item in template.value)
                {
                    if (item.Value is CellLink btn_template)
                    {
                        if (btn_template.Enabled)
                        {
                            if (btn_template.Rect.Contains(x, y))
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
                        OnTouchCancel();
                        return;
                    }
                }
            }
            if (dragHeader != null)
            {
                bool hand = dragHeader.hand;
                if (hand && dragHeader.im != -1)
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
                            var it = cells[item];
                            if (dragHeader.im == it.INDEX) dim = it.COLUMN.INDEX;
                            if (dragHeader.i == it.INDEX) di = it.COLUMN.INDEX;
                        }
                    }
                    foreach (var it in cells)
                    {
                        int index = it.COLUMN.INDEX;
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
                    if (cellMouseDown == null) return;
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
            cellMouseDown = null;
        }

        bool MouseUpRow(RowTemplate[] rows, RowTemplate it, TCell cell, MouseEventArgs e, int i_r, int i_c)
        {
            if (cellMouseDown == cell && cell.MouseDown > 0)
            {
                var cel_sel = CellContains(rows, e.X, e.Y, out int r_x, out int r_y, out int offset_x, out int offset_xi, out int offset_y, out int i_row, out int i_cel, out int mode);
                if (cel_sel == null || (i_r != i_row || i_c != i_cel)) cell.MouseDown = 0;
                else
                {
                    SelectedIndex = i_r;
                    if (e.Button == MouseButtons.Left)
                    {
                        if (cell is TCellCheck checkCell)
                        {
                            if (checkCell.CONTAIN_REAL(r_x, r_y))
                            {
                                if (checkCell.COLUMN is ColumnCheck columnCheck && columnCheck.Call != null)
                                {
                                    var value = columnCheck.Call(!checkCell.Checked, it.RECORD, i_r, i_c);
                                    if (checkCell.Checked != value)
                                    {
                                        checkCell.Checked = value;
                                        SetValue(cell, checkCell.Checked);
                                        CheckedChanged?.Invoke(this, new TableCheckEventArgs(checkCell.Checked, it.RECORD, i_r, i_c));
                                    }
                                }
                                else if (checkCell.AutoCheck)
                                {
                                    checkCell.Checked = !checkCell.Checked;
                                    SetValue(cell, checkCell.Checked);
                                    CheckedChanged?.Invoke(this, new TableCheckEventArgs(checkCell.Checked, it.RECORD, i_r, i_c));
                                }
                            }
                        }
                        else if (cell is TCellRadio radioCell)
                        {
                            if (radioCell.CONTAIN_REAL(r_x, r_y) && !radioCell.Checked)
                            {
                                bool isok = false;
                                if (radioCell.COLUMN is ColumnRadio columnRadio && columnRadio.Call != null)
                                {
                                    var value = columnRadio.Call(true, it.RECORD, i_r, i_c);
                                    if (value) isok = true;
                                }
                                else if (radioCell.AutoCheck) isok = true;
                                if (isok)
                                {
                                    for (int i = 0; i < rows.Length; i++)
                                    {
                                        if (i != i_r)
                                        {
                                            var cell_selno = rows[i].cells[i_c];
                                            if (cell_selno is TCellRadio radioCell2 && radioCell2.Checked)
                                            {
                                                radioCell2.Checked = false;
                                                SetValue(cell_selno, false);
                                            }
                                        }
                                    }
                                    radioCell.Checked = true;
                                    SetValue(cell, radioCell.Checked);
                                    CheckedChanged?.Invoke(this, new TableCheckEventArgs(radioCell.Checked, it.RECORD, i_r, i_c));
                                }
                            }
                        }
                        else if (cell is TCellSwitch switchCell)
                        {
                            if (switchCell.CONTAIN_REAL(r_x, r_y) && !switchCell.Loading)
                            {
                                if (switchCell.COLUMN is ColumnSwitch columnSwitch && columnSwitch.Call != null)
                                {
                                    switchCell.Loading = true;
                                    ITask.Run(() =>
                                    {
                                        var value = columnSwitch.Call(!switchCell.Checked, it.RECORD, i_r, i_c);
                                        if (switchCell.Checked == value) return;
                                        switchCell.Checked = value;
                                        SetValue(cell, value);
                                    }).ContinueWith(action =>
                                    {
                                        switchCell.Loading = false;
                                    });
                                }
                                else if (switchCell.AutoCheck)
                                {
                                    switchCell.Checked = !switchCell.Checked;
                                    SetValue(cell, switchCell.Checked);
                                    CheckedChanged?.Invoke(this, new TableCheckEventArgs(switchCell.Checked, it.RECORD, i_r, i_c));
                                }
                            }
                        }
                        else if (it.IsColumn && cell.COLUMN.SortOrder && cell is TCellColumn col)
                        {
                            //点击排序
                            int SortMode;
                            if (col.rect_up.Contains(r_x, r_y)) SortMode = 1;
                            else if (col.rect_down.Contains(r_x, r_y)) SortMode = 2;
                            else
                            {
                                SortMode = col.COLUMN.SortMode + 1;
                                if (SortMode > 2) SortMode = 0;
                            }
                            if (col.COLUMN.SortMode != SortMode)
                            {
                                col.COLUMN.SortMode = SortMode;
                                foreach (var item in it.cells)
                                {
                                    if (item.COLUMN.SortOrder && item.INDEX != i_c) item.COLUMN.SortMode = 0;
                                }
                                Invalidate();
                                switch (SortMode)
                                {
                                    case 1:
                                        SortDataASC(col.COLUMN.Key);
                                        break;
                                    case 2:
                                        SortDataDESC(col.COLUMN.Key);
                                        break;
                                    case 0:
                                    default:
                                        SortData = null;
                                        break;
                                }
                                LoadLayout();
                                SortRows?.Invoke(this, new IntEventArgs(i_c));
                            }
                        }
                        else if (cell is Template template)
                        {
                            foreach (var item in template.value)
                            {
                                if (item.Value is CellLink btn)
                                {
                                    if (btn.ExtraMouseDown)
                                    {
                                        if (btn.Rect.Contains(r_x, r_y))
                                        {
                                            btn.Click();
                                            CellButtonClick?.Invoke(this, new TableButtonEventArgs(btn, it.RECORD, i_r, i_c, e));
                                        }
                                        btn.ExtraMouseDown = false;
                                    }
                                }
                            }
                        }
                    }
                    bool doubleClick = cell.MouseDown == 2;
                    cell.MouseDown = 0;
                    CellClick?.Invoke(this, new TableClickEventArgs(it.RECORD, i_row, i_cel, new Rectangle(cel_sel.RECT.X - offset_x, cel_sel.RECT.Y - offset_y, cel_sel.RECT.Width, cel_sel.RECT.Height), e));

                    bool enterEdit = false;
                    if (doubleClick)
                    {
                        CellDoubleClick?.Invoke(this, new TableClickEventArgs(it.RECORD, i_row, i_cel, new Rectangle(cel_sel.RECT.X - offset_x, cel_sel.RECT.Y - offset_y, cel_sel.RECT.Width, cel_sel.RECT.Height), e));
                        if (e.Button == MouseButtons.Left && editmode == TEditMode.DoubleClick) enterEdit = true;
                    }
                    else
                    {
                        if (e.Button == MouseButtons.Left && editmode == TEditMode.Click) enterEdit = true;
                    }
                    if (enterEdit)
                    {
                        EditModeClose();
                        int val = ScrollLine(i_row, rows);
                        OnEditMode(it, cel_sel, i_row, i_cel, offset_xi, offset_y - val);
                    }
                }
                return true;
            }
            return false;
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
                        SetCursor(CursorType.VSplit);
                        return;
                    }
                }
            }
            if (dragHeader != null)
            {
                SetCursor(CursorType.SizeAll);
                dragHeader.hand = true;
                dragHeader.xr = e.X - dragHeader.x;
                if (rows == null) return;
                int xr = dragHeader.x + dragHeader.xr;
                var cells = rows[0].cells;
                dragHeader.last = e.X > dragHeader.x;

                var cel_sel = CellContains(rows, xr, e.Y, out int r_x, out int r_y, out int offset_x, out int offset_xi, out int offset_y, out int i_row, out int i_cel, out int mode);
                if (cel_sel != null)
                {
                    var it = cells[i_cel];
                    if (it.INDEX == dragHeader.i) dragHeader.im = -1;
                    else dragHeader.im = it.INDEX;
                    Invalidate();
                    return;
                }
                if (cells[cells.Length - 1].INDEX == dragHeader.i) dragHeader.im = -1;
                else dragHeader.im = cells[cells.Length - 1].INDEX;
                Invalidate();
                return;
            }
            if (ScrollBar.MouseMoveY(e.Location) && ScrollBar.MouseMoveX(e.Location) && OnTouchMove(e.X, e.Y))
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
                            if (cel_tmp is Template template)
                            {
                                foreach (var item in template.value)
                                {
                                    if (item.Value is CellLink btn) btn.ExtraMouseHover = false;
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
                                if (cel_tmp is Template template)
                                {
                                    foreach (var item in template.value)
                                    {
                                        if (item.Value is CellLink btn) btn.ExtraMouseHover = false;
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
                                    SetCursor(CursorType.VSplit);
                                    return;
                                }
                            }
                        }
                        if (cel.SortWidth > 0) SetCursor(true);
                        else if (has_check && cel.COLUMN is ColumnCheck columnCheck && columnCheck.NoTitle && cel.CONTAIN_REAL(r_x, r_y)) SetCursor(true);
                        else if (ColumnDragSort)
                        {
                            SetCursor(CursorType.SizeAll);
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
                                    if (cel_tmp is Template template)
                                    {
                                        foreach (var item in template.value)
                                        {
                                            if (item.Value is CellLink btn) btn.ExtraMouseHover = false;
                                        }
                                    }
                                }
                            }
                        }
                        if (cel_sel.ROW.CanExpand && cel_sel.ROW.RectExpand.Contains(r_x, r_y)) { SetCursor(true); return; }
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
                foreach (var item in template.value)
                {
                    if (item.Value is CellLink btn_template)
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
                    else if (item.Value is CellImage img_template)
                    {
                        if (img_template.Tooltip != null && img_template.Rect.Contains(x, y)) tipcel = img_template;
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
                                tooltipForm = new TooltipForm(this, rect, btn_template.Tooltip, new TooltipConfig
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
                                tooltipForm = new TooltipForm(this, rect, img_template.Tooltip, new TooltipConfig
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
                    if (cel.MinWidth > cel.RECT_REAL.Width)
                    {
                        var text = cel.ToString();
                        if (!string.IsNullOrEmpty(text))
                        {
                            var _rect = RectangleToScreen(ClientRectangle);
                            var rect = new Rectangle(_rect.X + cel.RECT.X - offset_xi, _rect.Y + cel.RECT.Y - offset_y, cel.RECT.Width, cel.RECT.Height);
                            if (tooltipForm == null)
                            {
                                tooltipForm = new TooltipForm(this, rect, text, new TooltipConfig
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
            int sx = ScrollBar.ValueX, sy = ScrollBar.ValueY;
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
                                    if (cel.CONTAIN(ex, ey))
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
                                    if (cel.CONTAIN(ex + sFixedR, ey))
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
                                if (cel.CONTAIN(px, ey))
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
                                if (cel.CONTAIN(ex, py))
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
                                if (cel.CONTAIN(ex + sFixedR, py))
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
                            if (cel.CONTAIN(px, py))
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
                            if (cel.CONTAIN(ex, py))
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
                            if (cel.CONTAIN(ex + sFixedR, py))
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
                        if (cel.CONTAIN(px, py))
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
            if (dataTmp == null) return new List<SortModel>(0);
            var list = new List<SortModel>(dataTmp.rows.Length);
            for (int i_r = 0; i_r < dataTmp.rows.Length; i_r++)
            {
                var value = OGetValue(dataTmp, i_r, key);
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
                    if (cel is Template template)
                    {
                        foreach (var item in template.value)
                        {
                            if (item.Value is CellLink btn) btn.ExtraMouseHover = false;
                        }
                    }
                }
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            ScrollBar.MouseWheel(e.Delta);
            base.OnMouseWheel(e);
        }
        protected override bool OnTouchScrollX(int value) => ScrollBar.MouseWheelX(value);
        protected override bool OnTouchScrollY(int value) => ScrollBar.MouseWheelY(value);
    }
}