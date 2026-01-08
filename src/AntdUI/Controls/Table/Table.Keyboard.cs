// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.Windows.Forms;

namespace AntdUI
{
    partial class Table
    {
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Control | Keys.A:
                    if (MultipleRows && dataTmp != null)
                    {
                        var list = new int[dataTmp.rows.Length];
                        for (int i = 0; i < dataTmp.rows.Length; i++) list[i] = (i + 1);
                        SelectedIndexs = list;
                        if (HandShortcutKeys) return true;
                    }
                    break;
                case Keys.Control | Keys.C:
                    if (ClipboardCopy && rows != null && !inEditMode && selectedIndex.Length > 0)
                    {
                        if (ClipboardCopyFocusedCell && FocusedCell != null) CopyData(FocusedCell);
                        else CopyData(selectedIndex);
                        if (HandShortcutKeys) return true;
                    }
                    break;
                case Keys.Down:
                    if (IKeyDown() && HandShortcutKeys) return true;
                    break;
                case Keys.Up:
                    if (IKeyUp() && HandShortcutKeys) return true;
                    break;
                case Keys.PageUp:
                    if (ScrollBar.ShowY)
                    {
                        ScrollBar.ValueY -= rect_read.Height;
                        if (HandShortcutKeys) return true;
                    }
                    break;
                case Keys.PageDown:
                    if (ScrollBar.ShowY)
                    {
                        ScrollBar.ValueY += rect_read.Height;
                        if (HandShortcutKeys) return true;
                    }
                    break;
                case Keys.Left:
                    int x = IKeyLeft();
                    if (ScrollBar.ShowX)
                    {
                        ScrollBar.ValueX -= x;
                        if (HandShortcutKeys) return true;
                    }
                    break;
                case Keys.Right:
                    int xr = IKeyRight();
                    if (ScrollBar.ShowX)
                    {
                        ScrollBar.ValueX += xr;
                        if (HandShortcutKeys) return true;
                    }
                    break;
                case Keys.Enter:
                case Keys.Space:
                    IKeyEnter();
                    break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        bool IKeyUp()
        {
            if (rows == null) return false;
            if (selectedIndex.Length == 0) ScrollBar.ValueY -= 50;
            else if (selectedIndex[0] > 1)
            {
                int value = NextIndexUp(rows, selectedIndex[0] - 1);
                SelectedIndex = value;
                ScrollLine(value, rows);
            }
            return true;
        }
        bool IKeyDown()
        {
            if (rows == null) return false;
            if (selectedIndex.Length == 0) ScrollBar.ValueY += 50;
            else if (selectedIndex[selectedIndex.Length - 1] < (rows.Length - 1 - rowSummary))
            {
                int value = NextIndexDown(rows, selectedIndex[selectedIndex.Length - 1] + 1);
                SelectedIndex = value;
                ScrollLine(value, rows);
            }
            else if (selectedIndex.Length > 1)
            {
                int value = selectedIndex[selectedIndex.Length - 1];
                SelectedIndex = value;
                ScrollLine(value, rows);
            }
            return true;
        }
        int IKeyLeft()
        {
            if (focusedxy == null || focusedxy[0] <= 0) return 50;
            if (rows == null) return 50;
            try
            {
                var row = rows[focusedxy[1]];
                var cel = row.cells[focusedxy[0] - 1];
                SetFocusedCell(cel);
                return (fixedColumnR != null && fixedColumnR.Contains(cel.INDEX)) ? 0 : cel.RECT.Width;
            }
            catch { }
            return 50;
        }
        int IKeyRight()
        {
            if (focusedxy == null || rows == null) return 50;
            int next = focusedxy[0] + 1;
            try
            {
                var row = rows[focusedxy[1]];
                if (next < row.cells.Length)
                {
                    var cel = row.cells[next];
                    SetFocusedCell(cel);
                    return (fixedColumnL != null && fixedColumnL.Contains(cel.INDEX)) || cel.RECT.Right < Width ? 0 : cel.RECT.Width;
                }
            }
            catch { }
            return 50;
        }

        void IKeyEnter()
        {
            if (rows == null) return;
            try
            {
                if (selectedIndex.Length > 0)
                {
                    int index = selectedIndex[0];
                    if (index > rows.Length) return;
                    var it = rows[index];
                    OnCellClick(it.RECORD, it.Type, index, 0, FocusedColumn, RealRect(it.RECT, ScrollBar.ValueX, ScrollBar.ValueY), new MouseEventArgs(MouseButtons.None, 0, 0, 0, 0));
                    if (editmode == TEditMode.None || focusedxy == null) return;
                    if (inEditMode)
                    {
                        var id_tmp = "edit_" + focusedxy[0] + "_" + focusedxy[1];
                        foreach (var item in _editControls)
                        {
                            if (item.Key.Name == id_tmp) return;
                        }
                        if (EnableFocusNavigation && (navigationConfig?.Contains(rows[0].cells[focusedxy[0]].COLUMN.Key, out _) ?? false)) return;
                    }
                    _currentEdit = null;
                    EnterEditMode(index, focusedxy[0]);
                }
            }
            catch { }
        }

        int NextIndexDown(RowTemplate[] rows, int value)
        {
            if (value < rows.Length - 1)
            {
                while (true)
                {
                    if (value < rows.Length - 1)
                    {
                        if (rows[value].ShowExpand) return value;
                        else value++;
                    }
                    else return rows.Length - 1;
                }
            }
            return value;
        }
        int NextIndexUp(RowTemplate[] rows, int value)
        {
            if (value > 0)
            {
                while (true)
                {
                    if (value > 0)
                    {
                        if (rows.Length < value) return rows.Length - 1;
                        else if (rows[value].ShowExpand) return value;
                        else value--;
                    }
                    else return 0;
                }
            }
            return value;
        }
    }
}