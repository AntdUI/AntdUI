// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AntdUI
{
    partial class Input
    {
        internal bool hasAlt = false;
        protected override bool IsInputKey(Keys keyData)
        {
            if ((keyData & Keys.Alt) != Keys.Alt)
            {
                hasAlt = false;
                switch (keyData & Keys.KeyCode)
                {
                    case Keys.Tab:
                        return Multiline && AcceptsTab && ((keyData & Keys.Control) == 0);
                    case Keys.Escape:
                        return AcceptsEscape;
                    case Keys.Left:
                    case Keys.Right:
                    case Keys.Up:
                    case Keys.Down:
                        return true;
                    case Keys.Back:
                        return !readOnly;
                }
            }
            else hasAlt = true;
            return base.IsInputKey(keyData);
        }

        protected override bool ProcessDialogChar(char charCode)
        {
            if (AdapterSystemMnemonic)
            {
                if ((ModifierKeys & Keys.Alt) == 0)
                {
                    OnKeyPress(new KeyPressEventArgs(charCode));
                    IKeyPress(charCode);
                    return true;
                }
            }
            return base.ProcessDialogChar(charCode);
        }

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
        public virtual void ProcessShortcutKeys(ShortcutKeys keyData)
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
                    // RTL：物理左=视觉左=逻辑前进
                    if (IsRTL) ProcessRightKey(false);
                    else ProcessLeftKey(false);
                    break;
                case ShortcutKeys.LeftShift:
                    if (IsRTL) ProcessRightKey(true);
                    else ProcessLeftKey(true);
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
                    // RTL：物理右=视觉右=逻辑后退
                    if (IsRTL) ProcessLeftKey(false);
                    else ProcessRightKey(false);
                    break;
                case ShortcutKeys.RightShift:
                    if (IsRTL) ProcessLeftKey(true);
                    else ProcessRightKey(true);
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
                    if (AcceptsTab) EnterText("\t", true, true);
                    break;
                case ShortcutKeys.Enter:
                    if (multiline) EnterText(Environment.NewLine, true, true);
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
                        var caret = GetCaretPostionUp();
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
                        var caret = GetCaretPostionDown();
                        if (caret == null) set_s = SetSelectionStart(cache_font.Count);
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
                    bool set_t = SetTextRemove(ref cache_font, start, 1), set_s = SetSelectionStart(start, false), set_caret = SetCaretPostion(start == 0 ? 0 : pos, false);
                    if (set_t || set_s || set_caret) Invalidate();
                }
            }
        }
        void ProcessBackSpaceKey(List<CacheFont> cache_font, int start, int end)
        {
            bool set_t = SetTextRemove(ref cache_font, start, end), set_e = SetSelectionLength(0), set_s = SetSelectionStart(start);
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
                bool set_t = SetTextRemove(ref cache_font, start, end), set_e = SetSelectionLength(0), set_s = SetSelectionStart(start);
                if (set_t || set_s || set_e) Invalidate();
            }
            else if (selectionStart < cache_font.Count)
            {
                int start = selectionStart;
                bool set_t = SetTextRemove(ref cache_font, start, 1), set_s = SetSelectionStart(start);
                if (set_t || set_s) Invalidate();
            }
        }

        void ProcessLeftKey(bool shift)
        {
            if (shift)
            {
                // 锚点不动，插入符左移一位（可反向收缩已选内容）
                int caret = SelectionCaretIndex;
                if (caret > 0 && SetSelectionCaret(caret - 1)) Invalidate();
                return;
            }
            // 取消选中：左移至选区起点，否则左移一位
            bool set_s = SetSelectionStart(SelectionLength > 0 ? selectionStartTemp : selectionStart - 1), set_e = SetSelectionLength(0);
            if (set_s || set_e) Invalidate();
        }

        void ProcessRightKey(bool shift)
        {
            if (shift)
            {
                // 锚点不动，插入符右移一位（可反向收缩已选内容）
                int caret = SelectionCaretIndex;
                if (cache_font != null && caret >= cache_font.Count) return;
                if (SetSelectionCaret(caret + 1)) Invalidate();
                return;
            }
            // 取消选中：右移至选区终点，否则右移一位
            int target = SelectionLength > 0 ? selectionStartTemp + selectionLength : selectionStart + 1;
            bool set_s = SetSelectionStart(target), set_e = SetSelectionLength(0);
            if (set_s || set_e) Invalidate();
        }

        void ProcessUpKey(bool shift)
        {
            if (shift)
            {
                // 锚点不动，插入符上移（可反向收缩已选内容）
                int caret = SelectionCaretIndex, target;
                if (cache_font == null) target = caret - 1;
                else
                {
                    var nearest = GetCaretPostionUp();
                    target = nearest == null ? caret - 1 : nearest.i;
                }
                if (target < caret && SetSelectionCaret(target)) Invalidate();
                return;
            }
            bool set_e = SetSelectionLength(0), set_s;
            if (cache_font == null) set_s = SetSelectionStart(selectionStart - 1);
            else
            {
                var nearest = GetCaretPostionUp();
                if (nearest == null) set_s = false;
                else if (nearest.i == selectionStart) set_s = SetSelectionStart(selectionStart - 1);
                else set_s = SetSelectionStart(nearest.i);
            }
            if (set_s || set_e) Invalidate();
        }

        void ProcessDownKey(bool shift)
        {
            if (shift)
            {
                // 锚点不动，插入符下移（可反向收缩已选内容）
                int caret = SelectionCaretIndex, target;
                if (cache_font == null) target = caret + 1;
                else
                {
                    var nearest = GetCaretPostionDown();
                    if (nearest == null) return;
                    target = nearest.i;
                }
                if (target > caret && SetSelectionCaret(target)) Invalidate();
                return;
            }
            bool set_e = SetSelectionLength(0), set_s;
            if (cache_font == null) set_s = SetSelectionStart(selectionStart + 1);
            else
            {
                var nearest = GetCaretPostionDown();
                if (nearest == null) set_s = false;
                else set_s = SetSelectionStart(nearest.i);
            }
            if (set_s || set_e) Invalidate();
        }

        void ProcessHomeKey(bool ctrl, bool shift)
        {
            int target;
            if (ctrl)
            {
                if (ScrollY.Show) ScrollY.Value = 0;
                target = 0;
            }
            else if (multiline)
            {
                if (cache_font == null) return;
                int index = (shift ? SelectionCaretIndex : selectionStart) - 1;
                if (index < 0) index = 0;
                target = FindStartY(cache_font, index);
            }
            else
            {
                if (ScrollY.Show) ScrollY.Value = 0;
                target = 0;
            }
            if (shift)
            {
                // 锚点不动，插入符移至行首（可反向收缩已选内容）
                if (SetSelectionCaret(target)) Invalidate();
            }
            else
            {
                bool set_e = SetSelectionLength(0), set_s = SetSelectionStart(target);
                if (set_s || set_e) Invalidate();
            }
        }

        void ProcessEndKey(bool ctrl, bool shift)
        {
            if (cache_font == null) return;
            int target;
            if (ctrl)
            {
                if (ScrollY.Show) ScrollY.Value = ScrollY.Max;
                target = cache_font.Count;
            }
            else if (multiline)
            {
                int index = shift ? SelectionCaretIndex : selectionStart;
                if (index > cache_font.Count - 1) index = cache_font.Count - 1;
                target = FindEndY(cache_font, index) + 1;
                if (target > cache_font.Count) target = cache_font.Count;
            }
            else
            {
                if (ScrollY.Show) ScrollY.Value = ScrollY.Max;
                target = cache_font.Count;
            }
            if (shift)
            {
                // 锚点不动，插入符移至行尾（可反向收缩已选内容）
                if (SetSelectionCaret(target)) Invalidate();
            }
            else
            {
                bool set_e = SetSelectionLength(0), set_s = SetSelectionStart(target);
                if (set_s || set_e) Invalidate();
            }
        }

        #endregion
    }
}