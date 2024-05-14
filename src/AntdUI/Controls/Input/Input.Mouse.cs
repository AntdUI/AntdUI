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
        bool mouseDown = false, mouseDownMove = false;
        Point oldMouseDown;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            is_prefix_down = is_suffix_down = false;
            if (e.Button == MouseButtons.Left)
            {
                if (cache_font != null && e.Clicks > 1)
                {
                    mouseDownMove = mouseDown = false;

                    var index = GetCaretPostion(e.Location.X + scrollx, e.Location.Y + scrolly);

                    int start = 0, end;

                    if (index > 0) start = FindStart(cache_font, index - 2);
                    if (index >= cache_font.Length) end = cache_font.Length;
                    else end = FindEnd(cache_font, index);

                    SelectionStart = start;
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
                    float y = (e.Y - ScrollSlider.Height / 2F) / ScrollRect.Height, VrValue = ScrollYMax + ScrollRect.Height;
                    ScrollY = (int)(y * VrValue);
                    ScrollYDown = true;
                    return;
                }
                mouseDownMove = false;
                Select();
                oldMouseDown = e.Location;
                SelectionStart = GetCaretPostion(e.Location.X + scrollx, e.Location.Y + scrolly);
                SelectionLength = 0;
                if (cache_font != null) mouseDown = true;
            }
        }

        internal virtual bool IMouseDown(Point e)
        {
            return false;
        }

        bool hover_clear = false;
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (ScrollYDown)
            {
                float y = (e.Y - ScrollSlider.Height / 2F) / ScrollRect.Height, VrValue = ScrollYMax + ScrollRect.Height;
                ScrollY = (int)(y * VrValue);
                return;
            }
            else if (mouseDown && cache_font != null)
            {
                mouseDownMove = true;
                Cursor = Cursors.IBeam;
                var index = GetCaretPostion(oldMouseDown.X + scrollx + (e.Location.X - oldMouseDown.X), oldMouseDown.Y + scrolly + (e.Location.Y - oldMouseDown.Y));
                SelectionLength = Math.Abs(index - selectionStart);
                if (index > selectionStart) selectionStartTemp = selectionStart;
                else selectionStartTemp = index;
                SetCaretPostion(index);
            }
            else
            {
                if (ScrollYShow && autoscroll) ScrollHover = ScrollRect.Contains(e.Location);
                if (is_clear)
                {
                    var hover = rect_r.Contains(e.Location);
                    if (hover_clear != hover)
                    {
                        hover_clear = hover;
                        Invalidate();
                    }
                    if (hover) { Cursor = Cursors.Hand; return; }
                }
                if (HasPrefix && rect_l.Contains(e.Location) && PrefixClick != null)
                {
                    Cursor = Cursors.Hand; return;
                }
                if (HasSuffix && rect_r.Contains(e.Location) && SuffixClick != null)
                {
                    Cursor = Cursors.Hand; return;
                }
                if (IMouseMove(e.Location)) Cursor = Cursors.Hand;
                else if (ReadShowCaret)
                {
                    if (rect_text.Contains(e.Location)) Cursor = Cursors.Hand;
                    else Cursor = DefaultCursor;
                }
                else
                {
                    if (rect_text.Contains(e.Location)) Cursor = Cursors.IBeam;
                    else Cursor = DefaultCursor;
                }
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (ScrollYShow && autoscroll && e.Delta != 0) ScrollY -= e.Delta;
            base.OnMouseWheel(e);
        }

        internal virtual bool IMouseMove(Point e)
        {
            return false;
        }

        internal virtual void OnClearValue()
        {
            Text = "";
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            ScrollYDown = false;
            if (is_clear_down)
            {
                if (rect_r.Contains(e.Location)) OnClearValue();
                is_clear_down = false;
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
            if (mouseDown && mouseDownMove && cache_font != null)
            {
                var index = GetCaretPostion(e.Location.X + scrollx, e.Location.Y + scrolly);
                if (selectionStart == index) SelectionLength = 0;
                else if (index > selectionStart)
                {
                    SelectionLength = Math.Abs(index - selectionStart);
                    SelectionStart = selectionStart;
                }
                else
                {
                    int x = scrollx;
                    SelectionLength = Math.Abs(index - selectionStart);
                    SelectionStart = index;
                    ScrollX = x;
                }
            }
            mouseDown = false;
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

            "\r","\t","\n"
        };

        /// <summary>
        /// 查找前面
        /// </summary>
        int FindStart(CacheFont[] cache_font, int index)
        {
            for (int i = index; i >= 0; i--)
            {
                if (sptext.Contains(cache_font[i].text))
                {
                    return i + 1;
                }
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
                if (sptext.Contains(cache_font[i].text))
                {
                    return i;
                }
            }
            return end;
        }

        #region 鼠标进出

        internal bool _mouseDown = false;
        internal bool ExtraMouseDown
        {
            get => _mouseDown;
            set
            {
                if (_mouseDown == value) return;
                _mouseDown = value;
                ChangeMouseHover(_mouseHover, value);
                if (Config.Animation && Margins > 0)
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

        internal virtual void ChangeMouseHover(bool Hover, bool Focus) { }

        #endregion

        #region 事件

        [Description("前缀 点击时发生"), Category("行为")]
        public event MouseEventHandler? PrefixClick = null;

        [Description("后缀 点击时发生"), Category("行为")]
        public event MouseEventHandler? SuffixClick = null;

        #endregion
    }
}