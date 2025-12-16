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
                    case Keys.Up:
                    case Keys.Down:
                        return true;
                    case Keys.Back:
                        return !readOnly;
                }
            }
            return base.IsInputKey(keyData);
        }

        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData) => false;

        public void HandKeyBoard(Keys key)
        {
            if (OnVerifyKeyboard(key))
            {
                switch (key)
                {
                    case Keys.Back:
                        ProcessShortcutKeys(ShortcutKeys.Back);
                        break;
                    case Keys.Control | Keys.Back:
                        ProcessShortcutKeys(ShortcutKeys.BackControl);
                        break;
                    case Keys.Delete:
                        ProcessShortcutKeys(ShortcutKeys.Delete);
                        break;
                    case Keys.Tab:
                        ProcessShortcutKeys(ShortcutKeys.Tab);
                        break;
                    case Keys.Enter:
                        ProcessShortcutKeys(ShortcutKeys.Enter);
                        break;
                    case Keys.Control | Keys.A:
                        ProcessShortcutKeys(ShortcutKeys.ControlA);
                        break;
                    case Keys.Control | Keys.C:
                        ProcessShortcutKeys(ShortcutKeys.ControlC);
                        break;
                    case Keys.Control | Keys.X:
                        ProcessShortcutKeys(ShortcutKeys.ControlX);
                        break;
                    case Keys.Control | Keys.V:
                        ProcessShortcutKeys(ShortcutKeys.ControlV);
                        break;
                    case Keys.Control | Keys.Z:
                        ProcessShortcutKeys(ShortcutKeys.ControlZ);
                        break;
                    case Keys.Control | Keys.Y:
                        ProcessShortcutKeys(ShortcutKeys.ControlY);
                        break;

                    case Keys.Left:
                        ProcessShortcutKeys(ShortcutKeys.Left);
                        break;
                    case Keys.Left | Keys.Shift:
                        ProcessShortcutKeys(ShortcutKeys.LeftShift);
                        break;
                    case Keys.Up:
                        ProcessShortcutKeys(ShortcutKeys.Up);
                        break;
                    case Keys.Up | Keys.Shift:
                        ProcessShortcutKeys(ShortcutKeys.UpShift);
                        break;
                    case Keys.Right:
                        ProcessShortcutKeys(ShortcutKeys.Right);
                        break;
                    case Keys.Right | Keys.Shift:
                        ProcessShortcutKeys(ShortcutKeys.RightShift);
                        break;
                    case Keys.Down:
                        ProcessShortcutKeys(ShortcutKeys.Down);
                        break;
                    case Keys.Down | Keys.Shift:
                        ProcessShortcutKeys(ShortcutKeys.DownShift);
                        break;
                    case Keys.Home:
                        ProcessShortcutKeys(ShortcutKeys.Home);
                        break;
                    case Keys.Home | Keys.Control:
                        ProcessShortcutKeys(ShortcutKeys.HomeControl);
                        break;
                    case Keys.Home | Keys.Shift:
                        ProcessShortcutKeys(ShortcutKeys.HomeShift);
                        break;
                    case Keys.Home | Keys.Control | Keys.Shift:
                        ProcessShortcutKeys(ShortcutKeys.HomeControlShift);
                        break;
                    case Keys.End:
                        ProcessShortcutKeys(ShortcutKeys.End);
                        break;
                    case Keys.End | Keys.Control:
                        ProcessShortcutKeys(ShortcutKeys.EndControl);
                        break;
                    case Keys.End | Keys.Shift:
                        ProcessShortcutKeys(ShortcutKeys.EndShift);
                        break;
                    case Keys.End | Keys.Control | Keys.Shift:
                        ProcessShortcutKeys(ShortcutKeys.EndControlShift);
                        break;
                    case Keys.PageUp:
                        ProcessShortcutKeys(ShortcutKeys.PageUp);
                        break;
                    case Keys.PageDown:
                        ProcessShortcutKeys(ShortcutKeys.PageDown);
                        break;
                }
            }
        }
        void HandKeyDown(Keys key)
        {
            if (OnVerifyKeyboard(key))
            {
                switch (key)
                {
                    case Keys.Tab:
                        if (AcceptsTab) ProcessShortcutKeys(ShortcutKeys.Tab);
                        break;
                    case Keys.Left:
                        ProcessShortcutKeys(ShortcutKeys.Left);
                        break;
                    case Keys.Left | Keys.Shift:
                        ProcessShortcutKeys(ShortcutKeys.LeftShift);
                        break;
                    case Keys.Up:
                        ProcessShortcutKeys(ShortcutKeys.Up);
                        break;
                    case Keys.Up | Keys.Shift:
                        ProcessShortcutKeys(ShortcutKeys.UpShift);
                        break;
                    case Keys.Right:
                        ProcessShortcutKeys(ShortcutKeys.Right);
                        break;
                    case Keys.Right | Keys.Shift:
                        ProcessShortcutKeys(ShortcutKeys.RightShift);
                        break;
                    case Keys.Down:
                        ProcessShortcutKeys(ShortcutKeys.Down);
                        break;
                    case Keys.Down | Keys.Shift:
                        ProcessShortcutKeys(ShortcutKeys.DownShift);
                        break;

                    case Keys.Back:
                        ProcessShortcutKeys(ShortcutKeys.Back);
                        break;
                    case Keys.Back | Keys.Control:
                        ProcessShortcutKeys(ShortcutKeys.BackControl);
                        break;
                    case Keys.Delete:
                        ProcessShortcutKeys(ShortcutKeys.Delete);
                        break;

                    case Keys.Enter:
                        ProcessShortcutKeys(ShortcutKeys.Enter);
                        break;
                    case Keys.Control | Keys.A:
                        ProcessShortcutKeys(ShortcutKeys.ControlA);
                        break;
                    case Keys.Control | Keys.C:
                        ProcessShortcutKeys(ShortcutKeys.ControlC);
                        break;
                    case Keys.Control | Keys.X:
                        ProcessShortcutKeys(ShortcutKeys.ControlX);
                        break;
                    case Keys.Control | Keys.V:
                        ProcessShortcutKeys(ShortcutKeys.ControlV);
                        break;
                    case Keys.Control | Keys.Z:
                        ProcessShortcutKeys(ShortcutKeys.ControlZ);
                        break;
                    case Keys.Control | Keys.Y:
                        ProcessShortcutKeys(ShortcutKeys.ControlY);
                        break;

                    case Keys.Home:
                        ProcessShortcutKeys(ShortcutKeys.Home);
                        break;
                    case Keys.Home | Keys.Control:
                        ProcessShortcutKeys(ShortcutKeys.HomeControl);
                        break;
                    case Keys.Home | Keys.Shift:
                        ProcessShortcutKeys(ShortcutKeys.HomeShift);
                        break;
                    case Keys.Home | Keys.Control | Keys.Shift:
                        ProcessShortcutKeys(ShortcutKeys.HomeControlShift);
                        break;
                    case Keys.End:
                        ProcessShortcutKeys(ShortcutKeys.End);
                        break;
                    case Keys.End | Keys.Control:
                        ProcessShortcutKeys(ShortcutKeys.EndControl);
                        break;
                    case Keys.End | Keys.Shift:
                        ProcessShortcutKeys(ShortcutKeys.EndShift);
                        break;
                    case Keys.End | Keys.Control | Keys.Shift:
                        ProcessShortcutKeys(ShortcutKeys.EndControlShift);
                        break;
                    case Keys.PageUp:
                        ProcessShortcutKeys(ShortcutKeys.PageUp);
                        break;
                    case Keys.PageDown:
                        ProcessShortcutKeys(ShortcutKeys.PageDown);
                        break;
                }
            }
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
        public void ProcessShortcutKeys(ShortcutKeys keyData)
        {
            switch (keyData)
            {
                case ShortcutKeys.Back:
                    ProcessBackSpaceKey();
                    break;
                case ShortcutKeys.BackControl:
                    ProcessBackSpaceKey(true);
                    break;
                case ShortcutKeys.Delete:
                    ProcessDelete();
                    break;
                //========================================================
                case ShortcutKeys.Left:
                    ProcessLeftKey(false);
                    break;
                case ShortcutKeys.LeftShift:
                    ProcessLeftKey(true);
                    break;
                case ShortcutKeys.Up:
                    if (multiline) ProcessUpKey(false);
                    else ProcessLeftKey(false);
                    break;
                case ShortcutKeys.UpShift:
                    if (multiline) ProcessUpKey(true);
                    else ProcessLeftKey(false);
                    break;
                case ShortcutKeys.Right:
                    ProcessRightKey(false);
                    break;
                case ShortcutKeys.RightShift:
                    ProcessRightKey(true);
                    break;
                case ShortcutKeys.Down:
                    if (multiline) ProcessDownKey(false);
                    else ProcessRightKey(false);
                    break;
                case ShortcutKeys.DownShift:
                    if (multiline) ProcessDownKey(true);
                    else ProcessRightKey(false);
                    break;
                case ShortcutKeys.Home:
                    ProcessHomeKey(false, false);
                    break;
                case ShortcutKeys.End:
                    ProcessEndKey(false, false);
                    break;
                case ShortcutKeys.HomeControl:
                    ProcessHomeKey(true, false);
                    break;
                case ShortcutKeys.EndControl:
                    ProcessEndKey(true, false);
                    break;
                case ShortcutKeys.HomeShift:
                    ProcessHomeKey(false, true);
                    break;
                case ShortcutKeys.EndShift:
                    ProcessEndKey(false, true);
                    break;
                case ShortcutKeys.HomeControlShift:
                    ProcessHomeKey(true, true);
                    break;
                case ShortcutKeys.EndControlShift:
                    ProcessEndKey(true, true);
                    break;
                //========================================================
                case ShortcutKeys.Tab:
                    if (AcceptsTab) EnterText("\t");
                    break;
                case ShortcutKeys.Enter:
                    if (multiline) EnterText(Environment.NewLine);
                    break;
                //========================================================
                case ShortcutKeys.ControlA:
                    SelectAll();
                    break;
                case ShortcutKeys.ControlC:
                    Copy();
                    break;
                case ShortcutKeys.ControlX:
                    Cut();
                    break;
                case ShortcutKeys.ControlV:
                    Paste();
                    break;
                case ShortcutKeys.ControlZ:
                    Undo();
                    break;
                case ShortcutKeys.ControlY:
                    Redo();
                    break;
                case ShortcutKeys.PageUp:
                    if (ScrollY.Show && cache_font != null)
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
                    }
                    break;
                case ShortcutKeys.PageDown:
                    if (ScrollY.Show && cache_font != null)
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
                    }
                    break;
            }
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
                if (ScrollY.Show) ScrollY.Value = 0;
                set_s = SetSelectionStart(0);
                set_e = SetSelectionLength(selectionLength + index);
            }
            else
            {
                set_e = SetSelectionLength(0);
                if (ctrl)
                {
                    if (ScrollY.Show) ScrollY.Value = 0;
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
                        if (ScrollY.Show) ScrollY.Value = 0;
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
                if (ScrollY.Show) ScrollY.Value = ScrollY.Max;
                SelectionLength += cache_font.Length - selectionStartTemp;
            }
            else
            {
                bool set_s, set_e = false;
                if (ctrl)
                {
                    if (ScrollY.Show) ScrollY.Value = ScrollY.Max;
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
                        if (ScrollY.Show) ScrollY.Value = ScrollY.Max;
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