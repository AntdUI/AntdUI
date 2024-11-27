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

using System.Drawing;
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
                        else if (selectedIndex[selectedIndex.Length - 1] < rows.Length - 1)
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
                    if (ScrollBar.ShowX)
                    {
                        ScrollBar.ValueX -= 50;
                        if (HandShortcutKeys) return true;
                    }
                    break;
                case Keys.Right:
                    if (ScrollBar.ShowX)
                    {
                        ScrollBar.ValueX += 50;
                        if (HandShortcutKeys) return true;
                    }
                    break;
                case Keys.Enter:
                case Keys.Space:
                    if (rows != null && selectedIndex.Length > 0)
                    {
                        var it = rows[selectedIndex[0]];
                        CellClick?.Invoke(this, new TableClickEventArgs(it.RECORD, selectedIndex[0], 0, new Rectangle(it.RECT.X - ScrollBar.ValueX, it.RECT.Y - ScrollBar.ValueY, it.RECT.Width, it.RECT.Height), new MouseEventArgs(MouseButtons.Left, 0, 0, 0, 0)));
                    }
                    break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}