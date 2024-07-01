﻿// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
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
        public bool IProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)
        {
            return ProcessCmdKey(ref msg, keyData);
        }

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
                    ProcessLeftKey(false);
                    return true;
                case Keys.Left | Keys.Shift:
                    ProcessLeftKey(true);
                    return true;
                case Keys.Up:
                    if (multiline) ProcessUpKey(false);
                    else ProcessLeftKey(false);
                    return true;
                case Keys.Up | Keys.Shift:
                    if (multiline) ProcessUpKey(true);
                    else ProcessLeftKey(false);
                    return true;
                case Keys.Right:
                    ProcessRightKey(false);
                    return true;
                case Keys.Right | Keys.Shift:
                    ProcessRightKey(true);
                    return true;
                case Keys.Down:
                    if (multiline) ProcessDownKey(false);
                    else ProcessRightKey(false);
                    return true;
                case Keys.Down | Keys.Shift:
                    if (multiline) ProcessDownKey(true);
                    else ProcessRightKey(false);
                    return true;
                case Keys.Home:
                    ProcessHomeKey(false, false);
                    return true;
                case Keys.End:
                    ProcessEndKey(false, false);
                    return true;
                case Keys.Control | Keys.Home:
                    ProcessHomeKey(true, false);
                    return true;
                case Keys.Control | Keys.End:
                    ProcessEndKey(true, false);
                    return true;
                case Keys.Shift | Keys.Home:
                    ProcessHomeKey(false, true);
                    return true;
                case Keys.Shift | Keys.End:
                    ProcessEndKey(false, true);
                    return true;
                case Keys.Control | Keys.Shift | Keys.Home:
                    ProcessHomeKey(true, true);
                    return true;
                case Keys.Control | Keys.Shift | Keys.End:
                    ProcessEndKey(true, true);
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
                    if (multiline)
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
                case Keys.PageUp:
                    if (ScrollYShow && cache_font != null)
                    {
                        SelectionLength = 0;
                        var index = GetCaretPostion(CurrentCaret.X, CurrentCaret.Y - (rect_text.Height - cache_font[0].rect.Height));
                        SelectionStart = index;
                        return true;
                    }
                    break;
                case Keys.PageDown:
                    if (ScrollYShow && cache_font != null)
                    {
                        SelectionLength = 0;
                        var index = GetCaretPostion(CurrentCaret.X, CurrentCaret.Y + (rect_text.Height - cache_font[0].rect.Height));
                        SelectionStart = index;
                        return true;
                    }
                    break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        internal void IKeyPress(KeyPressEventArgs e)
        {
            OnKeyPress(e);
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
                EnterText(change ?? e.KeyChar.ToString());
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
                int start = selectionStartTemp, end = selectionLength;
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
                int start = selectionStartTemp, end = selectionLength;
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

        void ProcessLeftKey(bool shift)
        {
            if (shift)
            {
                int old = selectionStartTemp;
                if (selectionStartTemp == selectionStart || selectionStartTemp < selectionStart) selectionStartTemp--;
                if (selectionStartTemp < 0) selectionStartTemp = 0;
                if (old == selectionStartTemp) return;
                SelectionLength++;
                CurrentPosIndex = selectionStartTemp;
                SetCaretPostion();
                return;
            }
            if (SelectionLength > 0)
            {
                if (selectionStartTemp < selectionStart) SelectionStart = selectionStartTemp;
                else
                {
                    int old = selectionStart;
                    selectionStart--;
                    SelectionStart = old;
                }
                SelectionLength = 0;
            }
            else
            {
                SelectionLength = 0;
                SelectionStart--;
            }
        }

        void ProcessRightKey(bool shift)
        {
            if (shift)
            {
                if (selectionStart > selectionStartTemp)
                {
                    selectionStartTemp++;
                    SelectionLength--;
                    CurrentPosIndex = selectionStartTemp + selectionLength;
                }
                else
                {
                    SelectionLength++;
                    CurrentPosIndex = selectionStart + selectionLength;
                }
                SetCaretPostion();
                return;
            }
            if (SelectionLength > 0)
            {
                if (selectionStartTemp > selectionStart) SelectionStart = selectionStartTemp + selectionLength;
                else
                {
                    int old = selectionStart;
                    selectionStart--;
                    SelectionStart = old + selectionLength;
                }
                SelectionLength = 0;
            }
            else
            {
                SelectionLength = 0;
                SelectionStart++;
            }
        }

        void ProcessUpKey(bool shift)
        {
            if (shift)
            {
                if (cache_font == null)
                {
                    SelectionLength = 0;
                    SelectionStart--;
                }
                else
                {
                    int index = selectionStartTemp;
                    if (index > cache_font.Length - 1) index = cache_font.Length - 1;
                    var it = cache_font[index];
                    var nearest = FindNearestFont(it.rect.X + it.rect.Width / 2, it.rect.Y - it.rect.Height / 2, cache_font, out _);
                    if (nearest == null || nearest.i == selectionStartTemp)
                    {
                        SelectionStart = index - 1;
                        SelectionLength++;
                    }
                    else
                    {
                        SelectionStart = nearest.i;
                        SelectionLength += index - nearest.i;
                    }
                }
            }
            else
            {
                SelectionLength = 0;
                if (cache_font == null) SelectionStart--;
                else
                {
                    int end = SelectionStart;
                    if (end > cache_font.Length - 1) end = cache_font.Length - 1;
                    var it = cache_font[end];
                    var nearest = FindNearestFont(it.rect.X + it.rect.Width / 2, it.rect.Y - it.rect.Height / 2, cache_font, out _);
                    if (nearest == null || nearest.i == selectionStart) SelectionStart--;
                    else SelectionStart = nearest.i;
                }
            }
        }

        void ProcessDownKey(bool shift)
        {
            if (shift)
            {
                if (cache_font == null)
                {
                    SelectionLength = 0;
                    SelectionStart++;
                }
                else
                {
                    int index = selectionStartTemp + selectionLength;
                    if (index > cache_font.Length - 1) return;
                    var it = cache_font[index];
                    CacheFont? nearest;
                    if (it.retun == 1) nearest = FindNearestFont(it.rect_old.Right, it.rect_old.Bottom + it.rect_old.Height / 2, cache_font, out _);
                    else nearest = FindNearestFont(it.rect.X + it.rect.Width / 2, it.rect.Bottom + it.rect.Height / 2, cache_font, out _);
                    if (nearest == null || nearest.i == index) SelectionLength++;
                    else SelectionLength += nearest.i - index;
                    CurrentPosIndex = selectionStart + selectionLength;
                    SetCaretPostion();
                }
            }
            else
            {
                SelectionLength = 0;
                if (cache_font == null) SelectionStart++;
                else
                {
                    int end = SelectionStart;
                    if (end > cache_font.Length - 1) return;
                    var it = cache_font[end];
                    CacheFont? nearest;
                    if (it.retun == 1) nearest = FindNearestFont(it.rect_old.Right, it.rect_old.Bottom + it.rect_old.Height / 2, cache_font, out _);
                    else nearest = FindNearestFont(it.rect.X + it.rect.Width / 2, it.rect.Bottom + it.rect.Height / 2, cache_font, out _);
                    if (nearest == null || nearest.i == selectionStart) SelectionStart++;
                    else SelectionStart = nearest.i;
                }
            }
        }

        void ProcessHomeKey(bool ctrl, bool shift)
        {
            if (ctrl && shift)
            {
                int index = selectionStartTemp;
                if (index == 0) return;
                if (ScrollYShow) ScrollY = 0;
                SelectionStart = 0;
                SelectionLength += index;
            }
            else
            {
                SelectionLength = 0;
                if (ctrl)
                {
                    if (ScrollYShow) ScrollY = 0;
                    SelectionStart = 0;
                }
                else
                {
                    if (cache_font == null) return;
                    int index = selectionStartTemp;
                    if (index > 0)
                    {
                        int start = FindStart(cache_font, index - 1);
                        if (start == index) return;
                        SelectionStart = start;
                    }
                }
            }
        }

        void ProcessEndKey(bool ctrl, bool shift)
        {
            if (cache_font == null) return;
            if (ctrl && shift)
            {
                int index = selectionStartTemp + selectionLength;
                if (index > cache_font.Length - 1) return;
                if (ScrollYShow) ScrollY = ScrollYMax;
                SelectionLength += cache_font.Length - selectionStartTemp;
            }
            else
            {
                if (ctrl)
                {
                    if (ScrollYShow) ScrollY = ScrollYMax;
                    SelectionLength = 0;
                    SelectionStart = cache_font.Length;
                }
                else
                {
                    int index = selectionStartTemp + selectionLength;
                    if (index > cache_font.Length - 1) return;
                    int start = FindEnd(cache_font, index);
                    if (start == index) return;
                    SelectionStart = start;
                }
            }
        }
    }
}