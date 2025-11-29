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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// 透明文本框
    /// </summary>
    partial class Input
    {
        bool mDown = false, mDownMove = false;
        Point mDownLocation;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            Focus();
            Select();
            is_prefix_down = is_suffix_down = false;
            if (e.Button == MouseButtons.Left)
            {
                if (cache_font != null && e.Clicks > 1 && !BanInput)
                {
                    mDownMove = mDown = false;

                    var caret2 = GetCaretPostion(e.X + scrollx, e.Y + scrolly);
                    if (caret2 == null) return;
                    int start = 0, end;

                    if (caret2.i > 0) start = FindStart(cache_font, caret2.i - 2);
                    if (caret2.i >= cache_font.Length) end = cache_font.Length;
                    else end = FindEnd(cache_font, caret2.i);

                    SetSelectionStart(start);
                    SelectionLength = end - start;

                    return;
                }
                if (is_clear && rect_r.Contains(e.X, e.Y))
                {
                    is_clear_down = true;
                    return;
                }
                if ((HasPrefix || prefixText != null) && rect_l.Contains(e.X, e.Y) && PrefixClick != null)
                {
                    is_prefix_down = true;
                    return;
                }
                if ((HasSuffix || suffixText != null) && rect_r.Contains(e.X, e.Y) && SuffixClick != null)
                {
                    is_suffix_down = true;
                    return;
                }
                if (IMouseDown(e.X, e.Y)) return;

                if (ScrollYShow && autoscroll && ScrollHover)
                {
                    float yratio = ((e.Y - ScrollRect.Top) - ScrollSliderFull / 2) / (ScrollRect.Height - ScrollSliderFull);
                    ScrollY = (int)(yratio * ScrollYMax);
                    ScrollYDown = true;
                    SetCursor(false);
                    Window.CanHandMessage = false;
                    return;
                }
                mDownMove = false;
                mDownLocation = e.Location;
                if (BanInput) return;
                var caret = GetCaretPostion(e.X + scrollx, e.Y + scrolly);
                if (caret == null)
                {
                    SetSelectionStart(0);
                    SelectionLength = 0;
                    SetCaretPostion(selectionStart);
                }
                else
                {
                    if (ModifierKeys.HasFlag(Keys.Shift))
                    {
                        if (caret.i > selectionStartTemp)
                        {
                            if (selectionStart != selectionStartTemp) SetSelectionStart(selectionStartTemp);
                            SelectionLength = caret.i - selectionStartTemp;
                        }
                        else
                        {
                            int len = selectionStartTemp - caret.i;
                            SetSelectionStart(caret.i, false);
                            SelectionLength = len;
                        }
                    }
                    else
                    {
                        SetSelectionStart(caret.i);
                        SelectionLength = 0;
                    }
                    SetCaretPostion(caret.index);
                }
                if (cache_font != null) mDown = true;
                else if (ModeRange) SetCaretPostion();
            }
        }

        bool hover_clear = false;
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (ScrollYDown)
            {
                float yratio = ((e.Y - ScrollRect.Top) - ScrollSliderFull / 2) / (ScrollRect.Height - ScrollSliderFull);
                ScrollY = (int)(yratio * ScrollYMax);
                Window.CanHandMessage = false;
                return;
            }
            else if (mDown && cache_font != null)
            {
                mDownMove = true;
                SetCursor(CursorType.IBeam);
                var caret = GetCaretPostion(mDownLocation.X + scrollx + (e.X - mDownLocation.X), mDownLocation.Y + scrolly + (e.Y - mDownLocation.Y));
                if (caret == null)
                {
                    Window.CanHandMessage = false;
                    return;
                }
                else
                {
                    SelectionLength = Math.Abs(caret.i - selectionStart);
                    if (caret.i > selectionStart) selectionStartTemp = selectionStart;
                    else selectionStartTemp = caret.i;
                    SetCaretPostion(caret.index);
                }
                Window.CanHandMessage = false;
            }
            else
            {
                bool setScroll = true;
                if (ScrollYShow && autoscroll)
                {
                    ScrollHover = ScrollRect.Contains(e.X, e.Y);
                    if (ScrollHover) setScroll = false;
                }
                if (is_clear)
                {
                    var hover = rect_r.Contains(e.X, e.Y);
                    if (hover_clear != hover)
                    {
                        hover_clear = hover;
                        Invalidate();
                    }
                    if (hover) { SetCursor(true); return; }
                }
                if (((HasPrefix || prefixText != null) && rect_l.Contains(e.X, e.Y) && PrefixClick != null) || ((HasSuffix || suffixText != null) && rect_r.Contains(e.X, e.Y) && SuffixClick != null))
                {
                    SetCursor(true);
                    return;
                }
                if (IMouseMove(e.X, e.Y)) SetCursor(true);
                else if (CaretInfo.ReadShow)
                {
                    if (rect_text.Contains(e.X, e.Y)) SetCursor(true);
                    else SetCursor(false);
                }
                else
                {
                    if (setScroll && rect_text.Contains(e.X, e.Y)) SetCursor(CursorType.IBeam);
                    else SetCursor(false);
                }
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (MouseWheelCore(e.Delta) && e is HandledMouseEventArgs handled) handled.Handled = true;
            base.OnMouseWheel(e);
        }
        bool MouseWheelCore(int delta)
        {
            if (ScrollYShow && autoscroll)
            {
                if (delta == 0) return false;
                var old = scrolly;
                ScrollY -= delta;
                if (old == scrolly) return false;
                return true;
            }
            return false;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            bool md = mDown;
            mDown = false;
            Window.CanHandMessage = true;
            ScrollYDown = false;
            if (is_clear_down)
            {
                if (rect_r.Contains(e.X, e.Y))
                {
                    OnClearValue();
                    OnClearClick(e);
                }
                is_clear_down = false;
                return;
            }
            if (is_prefix_down && PrefixClick != null)
            {
                PrefixClick(this, e);
                is_prefix_down = is_suffix_down = false;
                return;
            }
            if (is_suffix_down && SuffixClick != null)
            {
                SuffixClick(this, e);
                is_prefix_down = is_suffix_down = false;
                return;
            }
            if (IMouseUp(e.X, e.Y)) return;
            if (md && mDownMove && mDownLocation != e.Location && cache_font != null)
            {
                var caret = GetCaretPostion(e.X + scrollx, e.Y + scrolly);
                if (caret == null) { }
                else
                {
                    if (selectionStart == caret.i) SelectionLength = 0;
                    else if (caret.i > selectionStart)
                    {
                        SelectionLength = Math.Abs(caret.i - selectionStart);
                        SetCaretPostion(caret.index);
                    }
                    else
                    {
                        int x = scrollx;
                        SelectionLength = Math.Abs(caret.i - selectionStart);
                        SetSelectionStart(caret.i, false);
                        SetCaretPostion(caret.index);
                        ScrollX = x;
                    }
                }
            }
            else OnClickContent();
        }

        List<string> sptext = new List<string>{
            " ", "\r","\t","\n","\r\n",
            ",", "，", "。", "！", "？", ".",  "；", "：", ";",
            "(", ")","（", "）","<", ">", "【", "】","《", "》",
            "/", "\\"
        };

        #region 查找

        /// <summary>
        /// 查找前面
        /// </summary>
        int FindStart(CacheFont[] cache_font, int index)
        {
            for (int i = index; i >= 0; i--)
            {
                if (sptext.Contains(cache_font[i].text)) return i + 1;
            }
            return 0;
        }

        /// <summary>
        /// 查找后面
        /// </summary>
        int FindEnd(CacheFont[] cache_font, int index)
        {
            int end = cache_font.Length;
            for (int i = index; i < end; i++)
            {
                if (sptext.Contains(cache_font[i].text)) return i;
            }
            return end;
        }

        /// <summary>
        /// 查找行前面
        /// </summary>
        int FindStartY(CacheFont[] cache_font, int index)
        {
            int line = cache_font[index].line;
            int tmp = 0;
            for (int i = index; i >= 0; i--)
            {
                if (cache_font[i].line == line) tmp = i;
                else return tmp;
            }
            return 0;
        }


        /// <summary>
        /// 查找行后面
        /// </summary>
        int FindEndY(CacheFont[] cache_font, int index)
        {
            int line = cache_font[index].line;
            int tmp = 0;
            int end = cache_font.Length;
            for (int i = index + 1; i < end; i++)
            {
                if (cache_font[i].line == line) tmp = i;
                else return tmp == 0 ? index : tmp;
            }
            return end;
        }

        #endregion

        #region 鼠标进出

        internal bool _mouseDown = false;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal bool ExtraMouseDown
        {
            get => _mouseDown;
            set
            {
                if (_mouseDown == value) return;
                _mouseDown = value;
                ChangeMouseHover(_mouseHover, value);
                if (Config.HasAnimation(nameof(Input)) && WaveSize > 0 && TakePaint == null)
                {
                    ThreadFocus?.Dispose();
                    AnimationFocus = true;
                    var config = new AnimationLinearConfig(this, i =>
                    {
                        AnimationFocusValue = i;
                        Invalidate();
                        return true;
                    }, 20).SetValue(AnimationFocusValue);
                    if (value) config.SetAdd(4).SetMax(30);
                    else config.SetAdd(-4).SetMax(0);
                    ThreadFocus = new AnimationTask(config.SetEnd(() => AnimationFocus = false));
                }
                else Invalidate();
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            ExtraMouseHover = true;
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            ExtraMouseHover = false;
            base.OnMouseLeave(e);
        }

        internal int AnimationHoverValue = 0;
        internal bool AnimationHover = false;
        internal bool _mouseHover = false;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal bool ExtraMouseHover
        {
            get => _mouseHover;
            set
            {
                if (_mouseHover == value) return;
                _mouseHover = value;
                ChangeMouseHover(value, _mouseDown);
                if (Enabled)
                {
                    OnAllowClear();
                    if (Config.HasAnimation(nameof(Input)) && !ExtraMouseDown && TakePaint == null)
                    {
                        ThreadHover?.Dispose();
                        AnimationHover = true;
                        ThreadHover = new AnimationTask(new AnimationLinearConfig(this, i =>
                        {
                            AnimationHoverValue = i;
                            Invalidate();
                            return true;
                        }, 10).SetValueColor(AnimationHoverValue, value, 20).SetEnd(() => AnimationHover = false));
                    }
                    else AnimationHoverValue = 255;
                    Invalidate();
                }
            }
        }

        #endregion

        #region 事件

        [Description("清空 点击时发生"), Category("行为")]
        public event MouseEventHandler? ClearClick;

        [Description("前缀 点击时发生"), Category("行为")]
        public event MouseEventHandler? PrefixClick;

        [Description("后缀 点击时发生"), Category("行为")]
        public event MouseEventHandler? SuffixClick;

        [Description("验证字符时发生"), Category("行为")]
        public event InputVerifyCharEventHandler? VerifyChar;

        [Description("验证键盘时发生"), Category("行为")]
        public event InputVerifyKeyboardEventHandler? VerifyKeyboard;

        protected virtual void OnClearClick(MouseEventArgs e) => ClearClick?.Invoke(this, e);

        #endregion
    }
}