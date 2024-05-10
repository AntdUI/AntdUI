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
using System.Windows.Forms;

namespace AntdUI
{
    partial class Input
    {
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Back:
                    ProcessBackSpaceKey();
                    return true;
                case Keys.Delete:
                    ProcessDelete();
                    return true;
                //========================================================
                case Keys.Left:
                    ProcessLeftKey();
                    return true;
                case Keys.Up:
                    if (multiline) ProcessUpKey();
                    else ProcessLeftKey();
                    return true;
                case Keys.Right:
                    ProcessRightKey();
                    return true;
                case Keys.Down:
                    if (multiline) ProcessDownKey();
                    else ProcessRightKey();
                    return true;
                case Keys.Home:
                    ProcessHomeKey();
                    return true;
                case Keys.End:
                    ProcessEndKey();
                    return true;
                //========================================================
                case Keys.Tab:
                    if (multiline && AcceptsTab)
                    {
                        EnterText("\t");
                        return true;
                    }
                    break;
                case Keys.Enter:
                    if (multiline && AcceptsReturn)
                    {
                        EnterText(Environment.NewLine);
                        return true;
                    }
                    break;
                //========================================================
                case Keys.Control | Keys.A:
                    SelectAll();
                    return true;
                case Keys.Control | Keys.C:
                    Copy();
                    return true;
                case Keys.Control | Keys.X:
                    Cut();
                    return true;
                case Keys.Control | Keys.V:
                    Paste();
                    return true;
                case Keys.Control | Keys.Z:
                    Undo();
                    return true;
                case Keys.Control | Keys.Y:
                    Redo();
                    return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (e.KeyChar < 32)
            {
                base.OnKeyPress(e);
                return;
            }
            if (Verify(e.KeyChar, out var change))
            {
                EnterText((change ?? e.KeyChar).ToString());
                base.OnKeyPress(e);
            }
            else e.Handled = true;
        }

        /// <summary>
        /// 删除文本
        /// </summary>
        void ProcessBackSpaceKey()
        {
            if (ReadOnly) return;
            if (cache_font == null)
            {
                IBackSpaceKey();
                return;
            }
            if (selectionLength > 0)
            {
                int start = selectionStart, end = selectionLength;
                AddHistoryRecord();
                int end_temp = start + end;
                var texts = new List<string>();
                foreach (var it in cache_font)
                {
                    if (it.i < start || it.i >= end_temp) texts.Add(it.text);
                }
                Text = string.Join("", texts);
                SelectionLength = 0;
                SelectionStart = start;
            }
            else if (selectionStart > 0)
            {
                int start = selectionStart - 1;
                var texts = new List<string>();
                foreach (var it in cache_font)
                {
                    if (start != it.i) texts.Add(it.text);
                }
                Text = string.Join("", texts);
                SelectionStart = start;
            }
        }

        internal virtual void IBackSpaceKey() { }

        /// <summary>
        /// 删除文本
        /// </summary>
        void ProcessDelete()
        {
            if (cache_font == null || ReadOnly) return;
            if (selectionLength > 0)
            {
                int start = selectionStart, end = selectionLength;
                AddHistoryRecord();
                int end_temp = start + end;
                var texts = new List<string>();
                foreach (var it in cache_font)
                {
                    if (it.i < start || it.i >= end_temp) texts.Add(it.text);
                }
                Text = string.Join("", texts);
                SelectionLength = 0;
                SelectionStart = start;
            }
            else if (selectionStart < cache_font.Length)
            {
                int start = selectionStart;
                var texts = new List<string>();
                foreach (var it in cache_font)
                {
                    if (start != it.i) texts.Add(it.text);
                }
                Text = string.Join("", texts);
                SelectionStart = start;
            }
        }

        void ProcessLeftKey()
        {
            SelectionLength = 0;
            SelectionStart--;
        }

        void ProcessRightKey()
        {
            SelectionLength = 0;
            SelectionStart++;
        }

        void ProcessUpKey()
        {
            SelectionLength = 0;
            if (cache_font == null) SelectionStart--;
            else
            {
                int end = SelectionStart;
                if (end > cache_font.Length - 1) end = cache_font.Length - 1;
                var it = cache_font[end];
                var nearest = FindNearestFont(it.rect.X + it.rect.Width / 2, it.rect.Y - it.rect.Height / 2, cache_font);
                if (nearest == null || nearest.i == selectionStart) SelectionStart--;
                else SelectionStart = nearest.i;
            }
        }

        void ProcessDownKey()
        {
            SelectionLength = 0;
            if (cache_font == null) SelectionStart++;
            else
            {
                int end = SelectionStart;
                if (end > cache_font.Length - 1) return;
                var it = cache_font[end];
                var nearest = FindNearestFont(it.rect.X + it.rect.Width / 2, it.rect.Bottom + it.rect.Height / 2, cache_font);
                if (nearest == null || nearest.i == selectionStart) SelectionStart++;
                else SelectionStart = nearest.i;
            }
        }

        void ProcessHomeKey()
        {
            SelectionLength = 0;
            SelectionStart = 0;
        }

        void ProcessEndKey()
        {
            if (cache_font == null) return;
            SelectionLength = 0;
            SelectionStart = cache_font.Length;
        }
    }
}