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
                    if (ClipboardCopy && rows != null && selectedIndex > -1) return CopyData(selectedIndex);
                    return base.ProcessCmdKey(ref msg, keyData);
                case Keys.Down:
                    if (rows != null)
                    {
                        if (selectedIndex < rows.Length - 1)
                        {
                            SelectedIndex++;
                            var selectRow = rows[selectedIndex];
                            int sy = ScrollBar.ValueY;
                            if (selectRow.RECT.Y < sy || selectRow.RECT.Bottom > sy + rect_read.Height) ScrollLine(selectedIndex, rows);
                        }
                        return true;
                    }
                    break;
                case Keys.Up:
                    if (rows != null)
                    {
                        if (selectedIndex > 1)
                        {
                            SelectedIndex--;
                            var selectRow = rows[selectedIndex];
                            int sy = ScrollBar.ValueY;
                            if (selectRow.RECT.Y < sy || selectRow.RECT.Bottom > sy + rect_read.Height) ScrollLine(selectedIndex, rows);
                        }
                        return true;
                    }
                    break;
                case Keys.PageUp:
                    if (ScrollBar.ShowY)
                    {
                        ScrollBar.ValueY -= rect_read.Height;
                        return true;
                    }
                    break;
                case Keys.PageDown:
                    if (ScrollBar.ShowY)
                    {
                        ScrollBar.ValueY += rect_read.Height;
                        return true;
                    }
                    break;
                case Keys.Enter:
                case Keys.Space:
                    if (rows != null && selectedIndex > -1)
                    {
                        var it = rows[selectedIndex];
                        CellClick?.Invoke(this, new TableClickEventArgs(it.RECORD, selectedIndex, 0, new Rectangle(it.RECT.X - ScrollBar.ValueX, it.RECT.Y - ScrollBar.ValueY, it.RECT.Width, it.RECT.Height), new MouseEventArgs(MouseButtons.Left, 0, 0, 0, 0)));
                        return true;
                    }
                    break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}