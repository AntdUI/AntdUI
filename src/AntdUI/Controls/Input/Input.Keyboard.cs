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
        public void IProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData) => ProcessCmdKey(ref msg, keyData);

        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)
        {
            bool result = base.ProcessCmdKey(ref msg, keyData);
            switch (keyData)
            {
                case Keys.Back:
                    ProcessBackSpaceKey();
                    if (HandShortcutKeys) return true;
                    break;
                case Keys.Delete:
                    ProcessDelete();
                    if (HandShortcutKeys) return true;
                    break;
                //========================================================
                case Keys.Left:
                    ProcessLeftKey(false);
                    if (HandShortcutKeys) return true;
                    break;
                case Keys.Left | Keys.Shift:
                    ProcessLeftKey(true);
                    if (HandShortcutKeys) return true;
                    break;
                case Keys.Up:
                    if (multiline) ProcessUpKey(false);
                    else ProcessLeftKey(false);
                    if (HandShortcutKeys) return multiline;
                    break;
                case Keys.Up | Keys.Shift:
                    if (multiline) ProcessUpKey(true);
                    else ProcessLeftKey(false);
                    if (HandShortcutKeys) return multiline;
                    break;
                case Keys.Right:
                    ProcessRightKey(false);
                    if (HandShortcutKeys) return true;
                    break;
                case Keys.Right | Keys.Shift:
                    ProcessRightKey(true);
                    if (HandShortcutKeys) return true;
                    break;
                case Keys.Down:
                    if (multiline) ProcessDownKey(false);
                    else ProcessRightKey(false);
                    if (HandShortcutKeys) return multiline;
                    break;
                case Keys.Down | Keys.Shift:
                    if (multiline) ProcessDownKey(true);
                    else ProcessRightKey(false);
                    if (HandShortcutKeys) return multiline;
                    break;
                case Keys.Home:
                    SpeedScrollTo = true;
                    ProcessHomeKey(false, false);
                    SpeedScrollTo = false;
                    if (HandShortcutKeys) return true;
                    break;
                case Keys.End:
                    SpeedScrollTo = true;
                    ProcessEndKey(false, false);
                    SpeedScrollTo = false;
                    if (HandShortcutKeys) return true;
                    break;
                case Keys.Control | Keys.Home:
                    SpeedScrollTo = true;
                    ProcessHomeKey(true, false);
                    SpeedScrollTo = false;
                    if (HandShortcutKeys) return true;
                    break;
                case Keys.Control | Keys.End:
                    SpeedScrollTo = true;
                    ProcessEndKey(true, false);
                    SpeedScrollTo = false;
                    if (HandShortcutKeys) return true;
                    break;
                case Keys.Shift | Keys.Home:
                    SpeedScrollTo = true;
                    ProcessHomeKey(false, true);
                    SpeedScrollTo = false;
                    if (HandShortcutKeys) return true;
                    break;
                case Keys.Shift | Keys.End:
                    SpeedScrollTo = true;
                    ProcessEndKey(false, true);
                    SpeedScrollTo = false;
                    if (HandShortcutKeys) return true;
                    break;
                case Keys.Control | Keys.Shift | Keys.Home:
                    SpeedScrollTo = true;
                    ProcessHomeKey(true, true);
                    SpeedScrollTo = false;
                    if (HandShortcutKeys) return true;
                    break;
                case Keys.Control | Keys.Shift | Keys.End:
                    SpeedScrollTo = true;
                    ProcessEndKey(true, true);
                    SpeedScrollTo = false;
                    if (HandShortcutKeys) return true;
                    break;
                //========================================================
                case Keys.Tab:
                    if (multiline && AcceptsTab)
                    {
                        EnterText("\t");
                        if (HandShortcutKeys) return true;
                    }
                    break;
                case Keys.Enter:
                    if (multiline)
                    {
                        EnterText(Environment.NewLine);
                        if (HandShortcutKeys) return true;
                    }
                    break;
                //========================================================
                case Keys.Control | Keys.A:
                    SelectAll();
                    if (HandShortcutKeys) return true;
                    break;
                case Keys.Control | Keys.C:
                    Copy();
                    if (HandShortcutKeys) return true;
                    break;
                case Keys.Control | Keys.X:
                    Cut();
                    if (HandShortcutKeys) return true;
                    break;
                case Keys.Control | Keys.V:
                    Paste();
                    if (HandShortcutKeys) return true;
                    break;
                case Keys.Control | Keys.Z:
                    Undo();
                    if (HandShortcutKeys) return true;
                    break;
                case Keys.Control | Keys.Y:
                    Redo();
                    if (HandShortcutKeys) return true;
                    break;
                case Keys.PageUp:
                    if (ScrollYShow && cache_font != null)
                    {
                        SpeedScrollTo = true;
                        SelectionLength = 0;
                        var index = GetCaretPostion(CaretInfo.Rect.X, CaretInfo.Rect.Y - (rect_text.Height - cache_font[0].rect.Height));
                        SetSelectionStart(index);
                        SpeedScrollTo = false;
                        if (HandShortcutKeys) return true;
                    }
                    break;
                case Keys.PageDown:
                    if (ScrollYShow && cache_font != null)
                    {
                        SpeedScrollTo = true;
                        SelectionLength = 0;
                        var index = GetCaretPostion(CaretInfo.Rect.X, CaretInfo.Rect.Y + (rect_text.Height - cache_font[0].rect.Height));
                        SetSelectionStart(index);
                        SpeedScrollTo = false;
                        if (HandShortcutKeys) return true;
                    }
                    break;
            }
            return result;
        }

        internal void IKeyPress(char keyChar)
        {
            if (keyChar < 32) return;
            if (Verify(keyChar, out var change)) EnterText(change ?? keyChar.ToString());
        }

        /// <summary>
        /// 删除文本
        /// </summary>
        void ProcessBackSpaceKey()
        {
            if (ReadOnly || BanInput) return;
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
                var texts = new List<string>(end);
                foreach (var it in cache_font)
                {
                    if (it.i < start || it.i >= end_temp) texts.Add(it.text);
                }
                Text = string.Join("", texts);
                SelectionLength = 0;
                SetSelectionStart(start);
            }
            else if (selectionStart > 0)
            {
                int start = selectionStart - 1;
                if (start == 0) CaretInfo.FirstRet = true;
                var texts = new List<string>(cache_font.Length);
                foreach (var it in cache_font)
                {
                    if (start != it.i) texts.Add(it.text);
                }
                Text = string.Join("", texts);
                SetSelectionStart(start);
            }
        }

        /// <summary>
        /// 删除文本
        /// </summary>
        void ProcessDelete()
        {
            if (cache_font == null || ReadOnly || BanInput) return;
            if (selectionLength > 0)
            {
                int start = selectionStartTemp, end = selectionLength;
                AddHistoryRecord();
                int end_temp = start + end;
                var texts = new List<string>(end);
                foreach (var it in cache_font)
                {
                    if (it.i < start || it.i >= end_temp) texts.Add(it.text);
                }
                Text = string.Join("", texts);
                SelectionLength = 0;
                SetSelectionStart(start);
            }
            else if (selectionStart < cache_font.Length)
            {
                int start = selectionStart;
                var texts = new List<string>(cache_font.Length);
                foreach (var it in cache_font)
                {
                    if (start != it.i) texts.Add(it.text);
                }
                Text = string.Join("", texts);
                SetSelectionStart(start);
            }
        }

        void ProcessLeftKey(bool shift)
        {
            tmpUp = 0;
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
                if (selectionStartTemp < selectionStart) SetSelectionStart(selectionStartTemp);
                else
                {
                    int old = selectionStart;
                    selectionStart--;
                    SetSelectionStart(old);
                }
                SelectionLength = 0;
            }
            else
            {
                SelectionLength = 0;
                if (selectionStart == 1) CaretInfo.FirstRet = true;
                SetSelectionStart(selectionStart - 1);
            }
        }

        void ProcessRightKey(bool shift)
        {
            tmpUp = 0;
            if (CaretInfo.FirstRet)
            {
                CaretInfo.FirstRet = false;
                CaretInfo.Place = true;
            }
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
                if (selectionStartTemp > selectionStart) SetSelectionStart(selectionStartTemp + selectionLength);
                else
                {
                    int old = selectionStart;
                    selectionStart--;
                    SetSelectionStart(old + selectionLength);
                }
                SelectionLength = 0;
            }
            else
            {
                SelectionLength = 0;
                SetSelectionStart(selectionStart + 1);
            }
        }

        int tmpUp = 0;
        void ProcessUpKey(bool shift)
        {
            if (shift)
            {
                if (cache_font == null)
                {
                    SelectionLength = 0;
                    SetSelectionStart(selectionStart - 1);
                }
                else
                {
                    int index = selectionStartTemp, cend = cache_font.Length - 1;
                    if (index > cend) index = cend;
                    var it = cache_font[index];
                    var nearest = FindNearestFont(it.rect.X + it.rect.Width / 2, it.rect.Y - it.rect.Height / 2, cache_font, out bool two);
                    CaretInfo.FirstRet = two;
                    if (nearest == null || nearest.i == selectionStartTemp)
                    {
                        SetSelectionStart(index - 1);
                        SelectionLength++;
                    }
                    else
                    {
                        SetSelectionStart(nearest.i);
                        SelectionLength += index - nearest.i + (index >= cend ? 1 : 0);
                    }
                }
            }
            else
            {
                SelectionLength = 0;
                if (cache_font == null) SetSelectionStart(selectionStart - 1);
                else
                {
                    int end = SelectionStart;
                    if (end > cache_font.Length - 1) end = cache_font.Length - 1;
                    var it = cache_font[end];
                    var nearest = FindNearestFont(it.rect.X + it.rect.Width / 2, it.rect.Y - it.rect.Height / 2, cache_font, out bool two);
                    CaretInfo.FirstRet = two;
                    if (nearest == null) SetSelectionStart(selectionStart - 1);
                    else
                    {
                        if (nearest.i == 0)
                        {
                            if (tmpUp > 0)
                            {
                                CaretInfo.FirstRet = true;
                                tmpUp = 0;
                                SetCaretPostion(nearest.i);
                            }
                            else if (!CaretInfo.FirstRet) tmpUp++;
                        }
                        else tmpUp = 0;
                        if (nearest.i == selectionStart) SetSelectionStart(selectionStart - 1);
                        else SetSelectionStart(nearest.i);
                    }
                }
            }
        }

        void ProcessDownKey(bool shift)
        {
            tmpUp = 0;
            if (CaretInfo.FirstRet)
            {
                CaretInfo.FirstRet = false;
                CaretInfo.Place = true;
                tmpUp++;
            }
            if (shift)
            {
                if (cache_font == null)
                {
                    SelectionLength = 0;
                    SetSelectionStart(selectionStart + 1);
                }
                else
                {
                    int index = selectionStartTemp + selectionLength;
                    if (index > cache_font.Length - 1) return;
                    var it = cache_font[index];
                    var nearest = FindNearestFont(it.rect.X + it.rect.Width / 2, it.rect.Bottom + it.rect.Height / 2, cache_font, out bool two);
                    CaretInfo.FirstRet = two;
                    if (nearest == null || nearest.i == index) SelectionLength++;
                    else SelectionLength += nearest.i - index;
                    CurrentPosIndex = selectionStart + selectionLength;
                    SetCaretPostion();
                }
            }
            else
            {
                SelectionLength = 0;
                if (cache_font == null) SetSelectionStart(selectionStart + 1);
                else
                {
                    int end = SelectionStart;
                    if (end > cache_font.Length - 1) return;
                    var it = cache_font[end];
                    var nearest = FindNearestFont(it.rect.X + it.rect.Width / 2, it.rect.Bottom + it.rect.Height / 2, cache_font, out bool two);
                    CaretInfo.FirstRet = two;
                    if (nearest == null || nearest.i == selectionStart) SetSelectionStart(selectionStart + 1);
                    else SetSelectionStart(nearest.i);
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
                SetSelectionStart(0);
                SelectionLength += index;
            }
            else
            {
                SelectionLength = 0;
                if (ctrl)
                {
                    if (ScrollYShow) ScrollY = 0;
                    SetSelectionStart(0);
                }
                else
                {
                    if (multiline)
                    {
                        if (cache_font == null) return;
                        int index = selectionStartTemp;
                        if (index > 0)
                        {
                            int start = FindStartY(cache_font, index - 1);
                            if (start == index) return;
                            CaretInfo.Place = false;
                            SetSelectionStart(start);
                        }
                    }
                    else
                    {
                        if (ScrollYShow) ScrollY = 0;
                        SetSelectionStart(0);
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
                    SetSelectionStart(cache_font.Length);
                }
                else
                {
                    if (multiline)
                    {
                        int index = selectionStartTemp + selectionLength;
                        if (index > cache_font.Length - 1) return;
                        int start = FindEndY(cache_font, index) + 1;
                        if (start == index) return;
                        CaretInfo.Place = true;
                        SetSelectionStart(start);
                    }
                    else
                    {
                        if (ScrollYShow) ScrollY = ScrollYMax;
                        SelectionLength = 0;
                        SetSelectionStart(cache_font.Length);
                    }
                }
            }
        }
    }
}