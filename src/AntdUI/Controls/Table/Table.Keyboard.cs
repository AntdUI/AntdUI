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

using System.Windows.Forms;

namespace AntdUI
{
    partial class Table
    {
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Control | Keys.A:// 实现全选逻辑                      
                    if (MultipleRows && rows != null)
                    {
                        SelectedIndexs = SortIndex();
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
            if (focusedCell == null || focusedCell.INDEX <= 0) return 50;
            FocusedCell = focusedCell.ROW.cells[focusedCell.INDEX - 1];
            if (focusedCell == null) return 50;
            int x = (fixedColumnR != null && fixedColumnR.Contains(focusedCell.INDEX)) ? 0 : focusedCell.RECT.Width;
            Invalidate(focusedCell.ROW.RECT);
            return x;
        }
        int IKeyRight()
        {
            if (focusedCell == null) return 50;
            int next = focusedCell.INDEX + 1;
            if (next < focusedCell.ROW.cells.Length)
            {
                FocusedCell = focusedCell?.ROW.cells[next];
                if (focusedCell == null) return 50;
                int x = (fixedColumnL != null && fixedColumnL.Contains(focusedCell.INDEX)) || focusedCell.RECT.Right < Width ? 0 : focusedCell.RECT.Width;
                Invalidate(focusedCell.ROW.RECT);
                return x;
            }
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
                    CellClick?.Invoke(this, new TableClickEventArgs(it.RECORD, index, 0, null, RealRect(it.RECT, ScrollBar.ValueX, ScrollBar.ValueY), new MouseEventArgs(MouseButtons.None, 0, 0, 0, 0)));
                    if (EditMode != TEditMode.None && focusedCell != null) EnterEditMode(index, focusedCell.INDEX);
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
                        if (rows[value].SHOW) return value;
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
                        else if (rows[value].SHOW) return value;
                        else value--;
                    }
                    else return 0;
                }
            }
            return value;
        }
    }
}