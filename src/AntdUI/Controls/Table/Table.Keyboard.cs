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
                        SelectedIndexs = dataTmp.GetInts(1);
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
                    if (ScrollBar.ShowX)
                    {
                        if (IKeyLeft() && HandShortcutKeys) return true;
                    }
                    break;
                case Keys.Right:
                    if (ScrollBar.ShowX)
                    {
                        if (IKeyRight() && HandShortcutKeys) return true;
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
            if (rows == null || _editControls.Count > 0) return false;
            if (selectedIndex.Length == 0) ScrollBar.ValueY -= (int)(60 * Dpi);
            else if (selectedIndex[0] > 1)
            {
                int value = selectedIndex[0] - 1;
                if (value > 0)
                {
                    if (dataLen < value) value = dataLen - 1;
                }
                else value = 0;
                SelectedIndex = value;
                ScrollLine(value, rows);
            }
            return true;
        }
        bool IKeyDown()
        {
            if (rows == null || _editControls.Count > 0) return false;
            if (selectedIndex.Length == 0) ScrollBar.ValueY += (int)(60 * Dpi);
            else if (selectedIndex[selectedIndex.Length - 1] < dataLen)
            {
                int value = selectedIndex[selectedIndex.Length - 1] + 1;
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
        bool IKeyLeft()
        {
            if (rows == null || _editControls.Count > 0) return false;
            if (focusedxy == null || focusedxy[0] <= 0)
            {
                ScrollBar.ValueX -= (int)(60 * Dpi);
                return false;
            }
            try
            {
                var row = rows[focusedxy[1]];
                if (row == null)
                {
                    ScrollBar.ValueX -= (int)(60 * Dpi);
                    return false;
                }
                var cel = row.cells[focusedxy[0] - 1];
                SetFocusedCell(cel);
                ScrollColumn(cel.COLUMN);
                return true;
            }
            catch { }
            ScrollBar.ValueX -= (int)(60 * Dpi);
            return false;
        }
        bool IKeyRight()
        {
            if (rows == null || _editControls.Count > 0) return false;
            if (focusedxy == null)
            {
                ScrollBar.ValueX += (int)(60 * Dpi);
                return false;
            }
            int next = focusedxy[0] + 1;
            try
            {
                var row = rows[focusedxy[1]];
                if (row == null)
                {
                    ScrollBar.ValueX += (int)(60 * Dpi);
                    return false;
                }
                if (next < row.cells.Length)
                {
                    var cel = row.cells[next];
                    SetFocusedCell(cel);
                    ScrollColumn(cel.COLUMN);
                    return true;
                }
            }
            catch { }
            ScrollBar.ValueX += (int)(60 * Dpi);
            return false;
        }

        void IKeyEnter()
        {
            try
            {
                if (selectedIndex.Length > 0)
                {
                    int index = selectedIndex[0];
                    if (index > dataLen) return;
                    var it = rows?[index];
                    if (it == null) return;
                    OnCellClick(it.RECORD, it.Type, index, 0, FocusedColumn, RealRect(it.RECT, ScrollBar.ValueX, ScrollBar.ValueY), new MouseEventArgs(MouseButtons.None, 0, 0, 0, 0));
                    if (editmode == TEditMode.None || focusedxy == null) return;
                    if (inEditMode)
                    {
                        var id_tmp = "edit_" + focusedxy[0] + "_" + focusedxy[1];
                        foreach (var item in _editControls)
                        {
                            if (item.Key.Name == id_tmp) return;
                        }
                        if (EnableFocusNavigation && (navigationConfig?.Contains(rows![0].cells[focusedxy[0]].COLUMN.Key, out _) ?? false)) return;
                    }
                    _currentEdit = null;
                    EnterEditMode(index, focusedxy[0]);
                }
            }
            catch { }
        }
    }
}