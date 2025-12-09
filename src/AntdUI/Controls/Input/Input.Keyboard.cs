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
                    case Keys.Left:
                    case Keys.Right:
                        if (isempty) return base.IsInputKey(keyData);
                        return true;
                    case Keys.Up:
                    case Keys.Down:
                        if (isempty) return base.IsInputKey(keyData);
                        return Multiline;
                    case Keys.Back:
                        return !readOnly;
                }
            }
            return base.IsInputKey(keyData);
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
            AcceptsTab = true;
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
                        bool set_e = SetSelectionLength(0);
                        var caret = GetCaretPostion(CaretInfo.Rect.X, CaretInfo.Rect.Y - CaretInfo.Rect.Height);
                        if (caret == null)
                        {
                            bool set_s = SetSelectionStart(0);
                            if (set_s || set_e) Invalidate();
                        }
                        else
                        {
                            bool set_s = SetSelectionStart(caret.i, false), set_caret = SetCaretPostion(caret.index);
                            if (set_s || set_caret) Invalidate();
                        }
                        if (HandShortcutKeys) return true;
                    }
                    break;
                case ShortcutKeys.PageDown:
                    if (ScrollYShow && cache_font != null)
                    {
                        bool set_s, set_e = SetSelectionLength(0), set_caret = false;
                        var caret = GetCaretPostion(CaretInfo.Rect.X, CaretInfo.Rect.Y + CaretInfo.Rect.Height);
                        if (caret == null) set_s = SetSelectionStart(cache_font.Length);
                        else
                        {
                            set_s = SetSelectionStart(caret.i, false);
                            set_caret = SetCaretPostion(caret.index);
                        }
                        if (set_s || set_e || set_caret) Invalidate();
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
                    bool set_t = SetText(string.Join("", texts)), set_s = SetSelectionStart(start, false), set_caret = SetCaretPostion(start == 0 ? 0 : pos, false);
                    if (set_t || set_s || set_caret) Invalidate();
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
            bool set_t = SetText(string.Join("", texts)), set_e = SetSelectionLength(0), set_s = SetSelectionStart(start);
            if (set_t || set_s || set_e) Invalidate();
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
                bool set_t = SetText(string.Join("", texts)), set_e = SetSelectionLength(0), set_s = SetSelectionStart(start);
                if (set_t || set_s || set_e) Invalidate();
            }
            else if (selectionStart < cache_font.Length)
            {
                int start = selectionStart;
                var texts = new List<string>(cache_font.Length);
                foreach (var it in cache_font)
                {
                    if (start != it.i) texts.Add(it.text);
                }
                bool set_t = SetText(string.Join("", texts)), set_s = SetSelectionStart(start);
                if (set_t || set_s) Invalidate();
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
                bool set_e = SetSelectionLength(selectionLength + 1), set_caret = SetCaretPostion(CurrentPosIndex - 1);
                if (set_e || set_caret) Invalidate();
                return;
            }
            if (SelectionLength > 0)
            {
                bool set_s, set_e;
                if (selectionStartTemp < selectionStart) set_s = SetSelectionStart(selectionStartTemp);
                else
                {
                    int old = selectionStart;
                    selectionStart--;
                    set_s = SetSelectionStart(old);
                }
                set_e = SetSelectionLength(0);
                if (set_s || set_e) Invalidate();
            }
            else
            {
                bool set_e = SetSelectionLength(0), set_s = SetSelectionStart(selectionStart - 1);
                if (set_s || set_e) Invalidate();
            }
        }

        void ProcessRightKey(bool shift)
        {
            if (shift)
            {
                bool set_e2;
                if (selectionStart > selectionStartTemp)
                {
                    selectionStartTemp++;
                    set_e2 = SetSelectionLength(selectionLength - 1);
                }
                else set_e2 = SetSelectionLength(selectionLength + 1);
                bool set_caret = SetCaretPostion(CurrentPosIndex + 1);
                if (set_e2 || set_caret) Invalidate();
                return;
            }
            bool set_s, set_e;
            if (SelectionLength > 0)
            {
                if (selectionStartTemp > selectionStart) set_s = SetSelectionStart(selectionStartTemp + selectionLength);
                else
                {
                    int old = selectionStart;
                    selectionStart--;
                    set_s = SetSelectionStart(old + selectionLength);
                }
                set_e = SetSelectionLength(0);
            }
            else
            {
                set_e = SetSelectionLength(0);
                set_s = SetSelectionStart(selectionStart + 1);
            }
            if (set_s || set_e) Invalidate();
        }

        void ProcessUpKey(bool shift)
        {
            bool set_s, set_e;
            if (shift)
            {
                if (cache_font == null)
                {
                    set_e = SetSelectionLength(0);
                    set_s = SetSelectionStart(selectionStart - 1);
                }
                else
                {
                    int index = selectionStartTemp, cend = cache_font.Length - 1;
                    if (index > cend) index = cend;
                    var nearest = GetCaretPostion(CaretInfo.Rect.X, CaretInfo.Rect.Y - CaretInfo.Rect.Height);
                    if (nearest == null || nearest.i == selectionStartTemp)
                    {
                        set_s = SetSelectionStart(index - 1);
                        set_e = SetSelectionLength(selectionLength + 1);
                    }
                    else
                    {
                        set_s = SetSelectionStart(nearest.i);
                        set_e = SetSelectionLength(selectionLength + (index - nearest.i + (index >= cend ? 1 : 0)));
                    }
                }
            }
            else
            {
                set_e = SetSelectionLength(0);
                if (cache_font == null) set_s = SetSelectionStart(selectionStart - 1);
                else
                {
                    var nearest = GetCaretPostion(CaretInfo.Rect.X, CaretInfo.Rect.Y - CaretInfo.Rect.Height);
                    if (nearest == null) set_s = SetSelectionStart(selectionStart - 1);
                    else
                    {
                        if (nearest.i == selectionStart) set_s = SetSelectionStart(selectionStart - 1);
                        else set_s = SetSelectionStart(nearest.i);
                    }
                }
            }
            if (set_s || set_e) Invalidate();
        }

        void ProcessDownKey(bool shift)
        {
            bool set_s, set_e;
            if (shift)
            {
                if (cache_font == null)
                {
                    set_e = SetSelectionLength(0);
                    set_s = SetSelectionStart(selectionStart + 1);
                }
                else
                {
                    int index = selectionStartTemp + selectionLength;
                    if (index > cache_font.Length - 1) return;
                    var nearest = GetCaretPostion(CaretInfo.Rect.X, CaretInfo.Rect.Y + CaretInfo.Rect.Height);
                    if (nearest == null || nearest.i == index) set_e = SetSelectionLength(selectionLength + 1);
                    else set_e = SetSelectionLength(selectionLength + (nearest.i - index));
                    if (nearest == null) CurrentPosIndex = selectionStart + selectionLength;
                    else CurrentPosIndex = nearest.index;
                    set_s = SetCaretPostion();
                }
            }
            else
            {
                set_e = SetSelectionLength(0);
                if (cache_font == null) set_s = SetSelectionStart(selectionStart + 1);
                else
                {
                    var nearest = GetCaretPostion(CaretInfo.Rect.X, CaretInfo.Rect.Y + CaretInfo.Rect.Height);
                    if (nearest == null || nearest.i == selectionStart) set_s = SetSelectionStart(selectionStart + 1);
                    else set_s = SetSelectionStart(nearest.i);
                }
            }
            if (set_s || set_e) Invalidate();
        }

        void ProcessHomeKey(bool ctrl, bool shift)
        {
            bool set_s = false, set_e;
            if (ctrl && shift)
            {
                int index = selectionStartTemp;
                if (index == 0) return;
                if (ScrollYShow) ScrollY = 0;
                set_s = SetSelectionStart(0);
                set_e = SetSelectionLength(selectionLength + index);
            }
            else
            {
                set_e = SetSelectionLength(0);
                if (ctrl)
                {
                    if (ScrollYShow) ScrollY = 0;
                    set_s = SetSelectionStart(0);
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
                            set_s = SetSelectionStart(start);
                        }
                    }
                    else
                    {
                        if (ScrollYShow) ScrollY = 0;
                        set_s = SetSelectionStart(0);
                    }
                }
            }
            if (set_s || set_e) Invalidate();
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
                bool set_s, set_e = false;
                if (ctrl)
                {
                    if (ScrollYShow) ScrollY = ScrollYMax;
                    set_e = SetSelectionLength(0);
                    set_s = SetSelectionStart(cache_font.Length);
                }
                else
                {
                    if (multiline)
                    {
                        int index = selectionStartTemp + selectionLength;
                        if (index > cache_font.Length - 1) return;
                        int start = FindEndY(cache_font, index) + 1;
                        if (start == index) return;
                        set_s = SetSelectionStart(start);
                    }
                    else
                    {
                        if (ScrollYShow) ScrollY = ScrollYMax;
                        set_e = SetSelectionLength(0);
                        set_s = SetSelectionStart(cache_font.Length);
                    }
                }
                if (set_s || set_e) Invalidate();
            }
        }

        #endregion
    }
}