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

                    var index = GetCaretPostion(e.X + scrollx, e.Y + scrolly);

                    int start = 0, end;

                    if (index > 0) start = FindStart(cache_font, index - 2);
                    if (index >= cache_font.Length) end = cache_font.Length;
                    else end = FindEnd(cache_font, index);

                    SetSelectionStart(start);
                    SelectionLength = end - start;
                    return;
                }
                if (is_clear && rect_r.Contains(e.Location))
                {
                    is_clear_down = true;
                    return;
                }
                if (HasPrefix && rect_l.Contains(e.Location) && PrefixClick != null)
                {
                    is_prefix_down = true;
                    return;
                }
                if (HasSuffix && rect_r.Contains(e.Location) && SuffixClick != null)
                {
                    is_suffix_down = true;
                    return;
                }
                if (IMouseDown(e.Location)) return;

                if (ScrollYShow && autoscroll && ScrollHover)
                {
                    float y = (e.Y - ScrollSliderFull / 2F) / ScrollRect.Height, VrValue = ScrollYMax + ScrollRect.Height;
                    ScrollY = (int)(y * VrValue);
                    ScrollYDown = true;
                    SetCursor(false);
                    return;
                }
                mDownMove = false;
                mDownLocation = e.Location;
                if (BanInput) return;
                int indeX = GetCaretPostion(e.X + scrollx, e.Y + scrolly);
                if (ModifierKeys.HasFlag(Keys.Shift))
                {
                    if (indeX > selectionStartTemp)
                    {
                        if (selectionStart != selectionStartTemp) SetSelectionStart(selectionStartTemp);
                        SelectionLength = indeX - selectionStartTemp;
                    }
                    else
                    {
                        int len = selectionStartTemp - indeX;
                        SetSelectionStart(indeX);
                        SelectionLength = len;
                    }
                }
                else
                {
                    SetSelectionStart(indeX);
                    SelectionLength = 0;
                    SetCaretPostion(selectionStart);
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
                float y = (e.Y - ScrollSliderFull / 2F) / ScrollRect.Height, VrValue = ScrollYMax + ScrollRect.Height;
                ScrollY = (int)(y * VrValue);
                return;
            }
            else if (mDown && cache_font != null)
            {
                mDownMove = true;
                SetCursor(CursorType.IBeam);
                var index = GetCaretPostion(mDownLocation.X + scrollx + (e.X - mDownLocation.X), mDownLocation.Y + scrolly + (e.Y - mDownLocation.Y));
                SelectionLength = Math.Abs(index - selectionStart);
                if (index > selectionStart) selectionStartTemp = selectionStart;
                else selectionStartTemp = index;
                SetCaretPostion(index);
            }
            else
            {
                bool setScroll = true;
                if (ScrollYShow && autoscroll)
                {
                    ScrollHover = ScrollRect.Contains(e.Location);
                    if (ScrollHover) setScroll = false;
                }
                if (is_clear)
                {
                    var hover = rect_r.Contains(e.Location);
                    if (hover_clear != hover)
                    {
                        hover_clear = hover;
                        Invalidate();
                    }
                    if (hover) { SetCursor(true); return; }
                }
                if ((HasPrefix && rect_l.Contains(e.Location) && PrefixClick != null) || (HasSuffix && rect_r.Contains(e.Location) && SuffixClick != null))
                {
                    SetCursor(true);
                    return;
                }
                if (IMouseMove(e.Location)) SetCursor(true);
                else if (CaretInfo.ReadShow)
                {
                    if (rect_text.Contains(e.Location)) SetCursor(true);
                    else SetCursor(false);
                }
                else
                {
                    if (setScroll && rect_text.Contains(e.Location)) SetCursor(CursorType.IBeam);
                    else SetCursor(false);
                }
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (ScrollYShow && autoscroll && e.Delta != 0) ScrollY -= e.Delta;
            base.OnMouseWheel(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            bool md = mDown;
            mDown = false;
            ScrollYDown = false;
            if (is_clear_down)
            {
                if (rect_r.Contains(e.Location)) OnClearValue();
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
            if (IMouseUp(e.Location)) return;
            if (md && mDownMove && mDownLocation != e.Location && cache_font != null)
            {
                var index = GetCaretPostion(e.X + scrollx, e.Y + scrolly);
                if (selectionStart == index) SelectionLength = 0;
                else if (index > selectionStart)
                {
                    SelectionLength = Math.Abs(index - selectionStart);
                    SetSelectionStart(selectionStart);
                }
                else
                {
                    int x = scrollx;
                    SelectionLength = Math.Abs(index - selectionStart);
                    SetSelectionStart(index);
                    ScrollX = x;
                }
            }
            else OnClickContent();
        }


        List<string> sptext = new List<string>{
            "，",
            ",",
            "。",
            ".",
            "；",
            ";",
            " ",
            "/",
            "\\",

            "\r","\t","\n","\r\n"
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
                if (Config.Animation && WaveSize > 0)
                {
                    ThreadFocus?.Dispose();
                    AnimationFocus = true;
                    if (value)
                    {
                        ThreadFocus = new ITask(this, () =>
                        {
                            AnimationFocusValue += 4;
                            if (AnimationFocusValue > 30) return false;
                            Invalidate();
                            return true;
                        }, 20, () =>
                        {
                            AnimationFocus = false;
                            Invalidate();
                        });
                    }
                    else
                    {
                        ThreadFocus = new ITask(this, () =>
                        {
                            AnimationFocusValue -= 4;
                            if (AnimationFocusValue < 1) return false;
                            Invalidate();
                            return true;
                        }, 20, () =>
                        {
                            AnimationFocus = false;
                            Invalidate();
                        });
                    }
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
                    if (Config.Animation && !ExtraMouseDown)
                    {
                        ThreadHover?.Dispose();
                        AnimationHover = true;
                        if (value)
                        {
                            ThreadHover = new ITask(this, () =>
                            {
                                AnimationHoverValue += 20;
                                if (AnimationHoverValue > 255) { AnimationHoverValue = 255; return false; }
                                Invalidate();
                                return true;
                            }, 10, () =>
                            {
                                AnimationHover = false;
                                Invalidate();
                            });
                        }
                        else
                        {
                            ThreadHover = new ITask(this, () =>
                            {
                                AnimationHoverValue -= 20;
                                if (AnimationHoverValue < 1) { AnimationHoverValue = 0; return false; }
                                Invalidate();
                                return true;
                            }, 10, () =>
                            {
                                AnimationHover = false;
                                Invalidate();
                            });
                        }
                    }
                    else AnimationHoverValue = 255;
                    Invalidate();
                }
            }
        }

        #endregion

        #region 事件

        [Description("前缀 点击时发生"), Category("行为")]
        public event MouseEventHandler? PrefixClick = null;

        [Description("后缀 点击时发生"), Category("行为")]
        public event MouseEventHandler? SuffixClick = null;

        [Description("验证字符时发生"), Category("行为")]
        public event InputVerifyCharEventHandler? VerifyChar = null;

        [Description("验证键盘时发生"), Category("行为")]
        public event InputVerifyKeyboardEventHandler? VerifyKeyboard = null;

        #endregion
    }
}