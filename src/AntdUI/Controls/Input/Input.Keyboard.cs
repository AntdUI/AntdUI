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

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AntdUI
{
    partial class Input
    {
        protected override bool IsInputKey(Keys keyData)
        {
            if ((keyData & Keys.Alt) != Keys.Alt)
            {
                switch (keyData & Keys.KeyCode)
                {
                    case Keys.Tab:
                        return Multiline && AcceptsTab && ((keyData & Keys.Control) == 0);
                    case Keys.Escape:
                    case Keys.Up:
                    case Keys.Down:
                        return Multiline;
                    case Keys.Back:
                        return !readOnly;
                }
            }
            return true;
        }

        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData) => false;

        public bool HandKeyBoard(Keys key)
        {
            if (OnVerifyKeyboard(key))
            {
                switch (key)
                {
                    case Keys.Back:
                        return ProcessShortcutKeys(ShortcutKeys.Back);
                    case Keys.Control | Keys.Back:
                        return ProcessShortcutKeys(ShortcutKeys.BackControl);
                    case Keys.Delete:
                        return ProcessShortcutKeys(ShortcutKeys.Delete);

                    case Keys.Tab:
                        return ProcessShortcutKeys(ShortcutKeys.Tab);
                    case Keys.Enter:
                        return ProcessShortcutKeys(ShortcutKeys.Enter);
                    case Keys.Control | Keys.A:
                        return ProcessShortcutKeys(ShortcutKeys.ControlA);
                    case Keys.Control | Keys.C:
                        return ProcessShortcutKeys(ShortcutKeys.ControlC);
                    case Keys.Control | Keys.X:
                        return ProcessShortcutKeys(ShortcutKeys.ControlX);
                    case Keys.Control | Keys.V:
                        return ProcessShortcutKeys(ShortcutKeys.ControlV);
                    case Keys.Control | Keys.Z:
                        return ProcessShortcutKeys(ShortcutKeys.ControlZ);
                    case Keys.Control | Keys.Y:
                        return ProcessShortcutKeys(ShortcutKeys.ControlY);

                    case Keys.Left:
                        return ProcessShortcutKeys(ShortcutKeys.Left);
                    case Keys.Left | Keys.Shift:
                        return ProcessShortcutKeys(ShortcutKeys.LeftShift);
                    case Keys.Up:
                        return ProcessShortcutKeys(ShortcutKeys.Up);
                    case Keys.Up | Keys.Shift:
                        return ProcessShortcutKeys(ShortcutKeys.UpShift);
                    case Keys.Right:
                        return ProcessShortcutKeys(ShortcutKeys.Right);
                    case Keys.Right | Keys.Shift:
                        return ProcessShortcutKeys(ShortcutKeys.RightShift);
                    case Keys.Down:
                        return ProcessShortcutKeys(ShortcutKeys.Down);
                    case Keys.Down | Keys.Shift:
                        return ProcessShortcutKeys(ShortcutKeys.DownShift);
                    case Keys.Home:
                        return ProcessShortcutKeys(ShortcutKeys.Home);
                    case Keys.Home | Keys.Control:
                        return ProcessShortcutKeys(ShortcutKeys.HomeControl);
                    case Keys.Home | Keys.Shift:
                        return ProcessShortcutKeys(ShortcutKeys.HomeShift);
                    case Keys.Home | Keys.Control | Keys.Shift:
                        return ProcessShortcutKeys(ShortcutKeys.HomeControlShift);
                    case Keys.End:
                        return ProcessShortcutKeys(ShortcutKeys.End);
                    case Keys.End | Keys.Control:
                        return ProcessShortcutKeys(ShortcutKeys.EndControl);
                    case Keys.End | Keys.Shift:
                        return ProcessShortcutKeys(ShortcutKeys.EndShift);
                    case Keys.End | Keys.Control | Keys.Shift:
                        return ProcessShortcutKeys(ShortcutKeys.EndControlShift);
                    case Keys.PageUp:
                        return ProcessShortcutKeys(ShortcutKeys.PageUp);
                    case Keys.PageDown:
                        return ProcessShortcutKeys(ShortcutKeys.PageDown);
                }
            }
            return false;
        }
        bool HandKeyDown(Keys key)
        {
            if (OnVerifyKeyboard(key))
            {
                switch (key)
                {
                    case Keys.Tab:
                        if (AcceptsTab) return ProcessShortcutKeys(ShortcutKeys.Tab);
                        break;
                    case Keys.Left:
                        return ProcessShortcutKeys(ShortcutKeys.Left);
                    case Keys.Left | Keys.Shift:
                        return ProcessShortcutKeys(ShortcutKeys.LeftShift);
                    case Keys.Up:
                        return ProcessShortcutKeys(ShortcutKeys.Up);
                    case Keys.Up | Keys.Shift:
                        return ProcessShortcutKeys(ShortcutKeys.UpShift);
                    case Keys.Right:
                        return ProcessShortcutKeys(ShortcutKeys.Right);
                    case Keys.Right | Keys.Shift:
                        return ProcessShortcutKeys(ShortcutKeys.RightShift);
                    case Keys.Down:
                        return ProcessShortcutKeys(ShortcutKeys.Down);
                    case Keys.Down | Keys.Shift:
                        return ProcessShortcutKeys(ShortcutKeys.DownShift);

                    case Keys.Back:
                        return ProcessShortcutKeys(ShortcutKeys.Back);
                    case Keys.Back | Keys.Control:
                        return ProcessShortcutKeys(ShortcutKeys.BackControl);
                    case Keys.Delete:
                        return ProcessShortcutKeys(ShortcutKeys.Delete);

                    case Keys.Enter:
                        return ProcessShortcutKeys(ShortcutKeys.Enter);
                    case Keys.Control | Keys.A:
                        return ProcessShortcutKeys(ShortcutKeys.ControlA);
                    case Keys.Control | Keys.C:
                        return ProcessShortcutKeys(ShortcutKeys.ControlC);
                    case Keys.Control | Keys.X:
                        return ProcessShortcutKeys(ShortcutKeys.ControlX);
                    case Keys.Control | Keys.V:
                        return ProcessShortcutKeys(ShortcutKeys.ControlV);
                    case Keys.Control | Keys.Z:
                        return ProcessShortcutKeys(ShortcutKeys.ControlZ);
                    case Keys.Control | Keys.Y:
                        return ProcessShortcutKeys(ShortcutKeys.ControlY);

                    case Keys.Home:
                        return ProcessShortcutKeys(ShortcutKeys.Home);
                    case Keys.Home | Keys.Control:
                        return ProcessShortcutKeys(ShortcutKeys.HomeControl);
                    case Keys.Home | Keys.Shift:
                        return ProcessShortcutKeys(ShortcutKeys.HomeShift);
                    case Keys.Home | Keys.Control | Keys.Shift:
                        return ProcessShortcutKeys(ShortcutKeys.HomeControlShift);
                    case Keys.End:
                        return ProcessShortcutKeys(ShortcutKeys.End);
                    case Keys.End | Keys.Control:
                        return ProcessShortcutKeys(ShortcutKeys.EndControl);
                    case Keys.End | Keys.Shift:
                        return ProcessShortcutKeys(ShortcutKeys.EndShift);
                    case Keys.End | Keys.Control | Keys.Shift:
                        return ProcessShortcutKeys(ShortcutKeys.EndControlShift);
                    case Keys.PageUp:
                        return ProcessShortcutKeys(ShortcutKeys.PageUp);
                    case Keys.PageDown:
                        return ProcessShortcutKeys(ShortcutKeys.PageDown);
                }
            }
            return false;
        }

        bool HandKeyUp(Keys key)
        {
            return false;
        }

        Keys GetKeyBoard(int code)
        {
            Keys keyData = (Keys)code;
            bool Control = ModifierKeys.HasFlag(Keys.Control), Shift = ModifierKeys.HasFlag(Keys.Shift);
            if (Control && Shift) return keyData | Keys.Control | Keys.Shift;
            else if (Control)
            {
                if (keyData == Keys.Control) return keyData;
                return keyData | Keys.Control;
            }
            else if (Shift)
            {
                if (keyData == Keys.Shift) return keyData;
                return keyData | Keys.Shift;
            }
            return keyData;
        }

        internal void IKeyPress(char keyChar)
        {
            if (keyChar < 32 || keyChar == 127) return;
            if (Verify(keyChar, out var change)) EnterText(change ?? keyChar.ToString());
        }

        #region 处理快捷键

        protected virtual bool OnVerifyKeyboard(Keys keyData)
        {
            if (VerifyKeyboard == null) return true;
            var args = new InputVerifyKeyboardEventArgs(keyData);
            VerifyKeyboard(this, args);
            return args.Result;
        }

        /// <summary>
        /// 主动触发快捷键
        /// </summary>
        /// <param name="keyData">键盘数据</param>
        /// <returns>返回true拦截消息</returns>
        public bool ProcessShortcutKeys(ShortcutKeys keyData)
        {
            switch (keyData)
            {
                case ShortcutKeys.Back:
                    ProcessBackSpaceKey();
                    if (HandShortcutKeys) return true;
                    break;
                case ShortcutKeys.BackControl:
                    ProcessBackSpaceKey(true);
                    if (HandShortcutKeys) return true;
                    break;
                case ShortcutKeys.Delete:
                    ProcessDelete();
                    if (HandShortcutKeys) return true;
                    break;
                //========================================================
                case ShortcutKeys.Left:
                    ProcessLeftKey(false);
                    if (HandShortcutKeys) return true;
                    break;
                case ShortcutKeys.LeftShift:
                    ProcessLeftKey(true);
                    if (HandShortcutKeys) return true;
                    break;
                case ShortcutKeys.Up:
                    if (multiline) ProcessUpKey(false);
                    else ProcessLeftKey(false);
                    if (HandShortcutKeys) return multiline;
                    break;
                case ShortcutKeys.UpShift:
                    if (multiline) ProcessUpKey(true);
                    else ProcessLeftKey(false);
                    if (HandShortcutKeys) return multiline;
                    break;
                case ShortcutKeys.Right:
                    ProcessRightKey(false);
                    if (HandShortcutKeys) return true;
                    break;
                case ShortcutKeys.RightShift:
                    ProcessRightKey(true);
                    if (HandShortcutKeys) return true;
                    break;
                case ShortcutKeys.Down:
                    if (multiline) ProcessDownKey(false);
                    else ProcessRightKey(false);
                    if (HandShortcutKeys) return multiline;
                    break;
                case ShortcutKeys.DownShift:
                    if (multiline) ProcessDownKey(true);
                    else ProcessRightKey(false);
                    if (HandShortcutKeys) return multiline;
                    break;
                case ShortcutKeys.Home:
                    ProcessHomeKey(false, false);
                    if (HandShortcutKeys) return true;
                    break;
                case ShortcutKeys.End:
                    ProcessEndKey(false, false);
                    if (HandShortcutKeys) return true;
                    break;
                case ShortcutKeys.HomeControl:
                    ProcessHomeKey(true, false);
                    if (HandShortcutKeys) return true;
                    break;
                case ShortcutKeys.EndControl:
                    ProcessEndKey(true, false);
                    if (HandShortcutKeys) return true;
                    break;
                case ShortcutKeys.HomeShift:
                    ProcessHomeKey(false, true);
                    if (HandShortcutKeys) return true;
                    break;
                case ShortcutKeys.EndShift:
                    ProcessEndKey(false, true);
                    if (HandShortcutKeys) return true;
                    break;
                case ShortcutKeys.HomeControlShift:
                    ProcessHomeKey(true, true);
                    if (HandShortcutKeys) return true;
                    break;
                case ShortcutKeys.EndControlShift:
                    ProcessEndKey(true, true);
                    if (HandShortcutKeys) return true;
                    break;
                //========================================================
                case ShortcutKeys.Tab:
                    if (AcceptsTab)
                    {
                        EnterText("\t");
                        if (HandShortcutKeys) return true;
                    }
                    break;
                case ShortcutKeys.Enter:
                    if (multiline)
                    {
                        EnterText(Environment.NewLine);
                        if (HandShortcutKeys) return true;
                    }
                    break;
                //========================================================
                case ShortcutKeys.ControlA:
                    SelectAll();
                    if (HandShortcutKeys) return true;
                    break;
                case ShortcutKeys.ControlC:
                    Copy();
                    if (HandShortcutKeys) return true;
                    break;
                case ShortcutKeys.ControlX:
                    Cut();
                    if (HandShortcutKeys) return true;
                    break;
                case ShortcutKeys.ControlV:
                    Paste();
                    if (HandShortcutKeys) return true;
                    break;
                case ShortcutKeys.ControlZ:
                    Undo();
                    if (HandShortcutKeys) return true;
                    break;
                case ShortcutKeys.ControlY:
                    Redo();
                    if (HandShortcutKeys) return true;
                    break;
                case ShortcutKeys.PageUp:
                    if (ScrollYShow && cache_font != null)
                    {
                        SelectionLength = 0;
                        var caret = GetCaretPostion(CaretInfo.Rect.X, CaretInfo.Rect.Y - CaretInfo.Rect.Height);
                        if (caret == null) SetSelectionStart(0);
                        else
                        {
                            SetSelectionStart(caret.i, false);
                            SetCaretPostion(caret.index);
                        }
                        if (HandShortcutKeys) return true;
                    }
                    break;
                case ShortcutKeys.PageDown:
                    if (ScrollYShow && cache_font != null)
                    {
                        SelectionLength = 0;
                        var caret = GetCaretPostion(CaretInfo.Rect.X, CaretInfo.Rect.Y + CaretInfo.Rect.Height);
                        if (caret == null) SetSelectionStart(cache_font.Length);
                        else
                        {
                            SetSelectionStart(caret.i, false);
                            SetCaretPostion(caret.index);
                        }
                        if (HandShortcutKeys) return true;
                    }
                    break;
            }
            return false;
        }

        /// <summary>
        /// 快捷键枚举
        /// </summary>
        public enum ShortcutKeys
        {
            None,
            /// <summary>
            /// BackSpace
            /// </summary>
            Back,
            /// <summary>
            /// BackSpace
            /// </summary>
            BackControl,
            /// <summary>
            /// 删除
            /// </summary>
            Delete,
            /// <summary>
            /// 制表符
            /// </summary>
            Tab,
            /// <summary>
            /// 回车
            /// </summary>
            Enter,
            /// <summary>
            /// 全选
            /// </summary>
            ControlA,
            /// <summary>
            /// 复制
            /// </summary>
            ControlC,
            /// <summary>
            /// 剪贴
            /// </summary>
            ControlX,
            /// <summary>
            /// 粘贴
            /// </summary>
            ControlV,
            /// <summary>
            /// 撤消
            /// </summary>
            ControlZ,
            /// <summary>
            /// 重做
            /// </summary>
            ControlY,
            Left,
            LeftShift,
            Right,
            RightShift,
            Up,
            UpShift,
            Down,
            DownShift,
            Home,
            HomeShift,
            HomeControl,
            HomeControlShift,
            End,
            EndShift,
            EndControl,
            EndControlShift,
            PageUp,
            PageDown
        }

        /// <summary>
        /// 删除文本
        /// </summary>
        void ProcessBackSpaceKey(bool ctrl = false)
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
                ProcessBackSpaceKey(cache_font, start, end);
            }
            else if (selectionStart > 0)
            {
                AddHistoryRecord();
                int tmp = selectionStart - 1;
                if (ctrl && tmp > 1) ProcessBackSpaceKey(cache_font, FindStart(cache_font, tmp - 1), tmp + 1);
                else
                {
                    int start = tmp, pos = CurrentPosIndex - 1;
                    var texts = new List<string>(cache_font.Length);
                    foreach (var it in cache_font)
                    {
                        if (start != it.i) texts.Add(it.text);
                    }
                    Text = string.Join("", texts);
                    SetSelectionStart(start, false);
                    if (start == 0) SetCaretPostion(0);
                    else SetCaretPostion(pos);
                }
            }
        }
        void ProcessBackSpaceKey(CacheFont[] cache_font, int start, int end)
        {
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
            if (shift)
            {
                int old = selectionStartTemp;
                if (selectionStartTemp == selectionStart || selectionStartTemp < selectionStart) selectionStartTemp--;
                if (selectionStartTemp < 0) selectionStartTemp = 0;
                if (old == selectionStartTemp) return;
                SelectionLength++;
                SetCaretPostion(CurrentPosIndex - 1);
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
                SetSelectionStart(selectionStart - 1);
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
                }
                else SelectionLength++;
                SetCaretPostion(CurrentPosIndex + 1);
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
                    var nearest = GetCaretPostion(CaretInfo.Rect.X, CaretInfo.Rect.Y - CaretInfo.Rect.Height);
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
                    var nearest = GetCaretPostion(CaretInfo.Rect.X, CaretInfo.Rect.Y - CaretInfo.Rect.Height);
                    if (nearest == null) SetSelectionStart(selectionStart - 1);
                    else
                    {
                        if (nearest.i == selectionStart) SetSelectionStart(selectionStart - 1);
                        else SetSelectionStart(nearest.i);
                    }
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
                    SetSelectionStart(selectionStart + 1);
                }
                else
                {
                    int index = selectionStartTemp + selectionLength;
                    if (index > cache_font.Length - 1) return;
                    var nearest = GetCaretPostion(CaretInfo.Rect.X, CaretInfo.Rect.Y + CaretInfo.Rect.Height);
                    if (nearest == null || nearest.i == index) SelectionLength++;
                    else SelectionLength += nearest.i - index;
                    if (nearest == null) CurrentPosIndex = selectionStart + selectionLength;
                    else CurrentPosIndex = nearest.index;
                    SetCaretPostion();
                }
            }
            else
            {
                SelectionLength = 0;
                if (cache_font == null) SetSelectionStart(selectionStart + 1);
                else
                {
                    var nearest = GetCaretPostion(CaretInfo.Rect.X, CaretInfo.Rect.Y + CaretInfo.Rect.Height);
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

        #endregion
    }
}