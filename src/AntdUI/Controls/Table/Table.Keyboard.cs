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
                case Keys.Control | Keys.C:
                    if (ClipboardCopy && rows != null && !inEditMode && selectedIndex.Length > 0)
                    {
                        CopyData(selectedIndex);
                        if (HandShortcutKeys) return true;
                    }
                    break;
                case Keys.Down:
                    if (rows != null)
                    {
                        if (selectedIndex.Length == 0) ScrollBar.ValueY += 50;
                        else if (selectedIndex[selectedIndex.Length - 1] < (rows.Length - 1 - rowSummary))
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
                        if (HandShortcutKeys) return true;
                    }
                    break;
                case Keys.Up:
                    if (rows != null)
                    {
                        if (selectedIndex.Length == 0) ScrollBar.ValueY -= 50;
                        else if (selectedIndex[0] > 1)
                        {
                            SelectedIndex--;
                            ScrollLine(selectedIndex[0], rows);
                        }
                        if (HandShortcutKeys) return true;
                    }
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
                    int x = KeyLeft();
                    if (ScrollBar.ShowX)
                    {
                        ScrollBar.ValueX -= x;
                        if (HandShortcutKeys) return true;
                    }
                    break;
                case Keys.Right:
                    int xr = KeyRight();
                    if (ScrollBar.ShowX)
                    {
                        ScrollBar.ValueX += xr;
                        if (HandShortcutKeys) return true;
                    }
                    break;
                case Keys.Enter:
                case Keys.Space:
                    if (rows != null && selectedIndex.Length > 0)
                    {
                        var it = rows[selectedIndex[0]];
                        CellClick?.Invoke(this, new TableClickEventArgs(it.RECORD, selectedIndex[0], 0, null, RealRect(it.RECT, ScrollBar.ValueX, ScrollBar.ValueY), new MouseEventArgs(MouseButtons.None, 0, 0, 0, 0)));
                        if (EditMode != TEditMode.None && cellFocused != null) EnterEditMode(selectedIndex[0], cellFocused.INDEX);
                    }
                    break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        int KeyLeft()
        {
            if (cellFocused == null || cellFocused.INDEX <= 0) return 50;
            cellFocused = cellFocused.ROW.cells[cellFocused.INDEX - 1];
            if (cellFocused == null) return 50;
            int x = (fixedColumnR != null && fixedColumnR.Contains(cellFocused.INDEX)) ? 0 : cellFocused.RECT.Width;
            Invalidate(cellFocused.ROW.RECT);
            return x;
        }
        int KeyRight()
        {
            if (cellFocused == null) return 50;
            int next = cellFocused.INDEX + 1;
            if (next < cellFocused.ROW.cells.Length)
            {
                cellFocused = cellFocused?.ROW.cells[next];
                if (cellFocused == null) return 50;
                int x = (fixedColumnL != null && fixedColumnL.Contains(cellFocused.INDEX)) || cellFocused.RECT.Right < Width ? 0 : cellFocused.RECT.Width;
                Invalidate(cellFocused.ROW.RECT);
                return x;
            }
            return 50;
        }
    }
}