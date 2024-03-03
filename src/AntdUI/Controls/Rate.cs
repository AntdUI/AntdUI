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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// Rate 评分
    /// </summary>
    /// <remarks>评分组件。</remarks>
    [Description("Rate 评分")]
    [ToolboxItem(true)]
    [DefaultProperty("Value")]
    [DefaultEvent("ValueChanged")]
    public class Rate : IControl
    {
        #region 属性

        Color fill = Color.FromArgb(250, 219, 20);
        [Description("颜色"), Category("外观"), DefaultValue(typeof(Color), "250, 219, 20")]
        public Color Fill
        {
            get => fill;
            set
            {
                if (fill == value) return;
                fill = value;
                icon_active?.Dispose();
                icon_active = null;
                Invalidate();
            }
        }

        /// <summary>
        /// 是否允许半选
        /// </summary>
        [Description("是否允许半选"), Category("行为"), DefaultValue(false)]
        public bool AllowHalf { get; set; } = false;

        int count = 5;
        /// <summary>
        /// Star 总数
        /// </summary>
        [Description("Star 总数"), Category("外观"), DefaultValue(5)]
        public int Count
        {
            get => count;
            set
            {
                if (count == value) return;
                count = value;
                OnSizeChanged(EventArgs.Empty);
                Invalidate();
            }
        }

        float _value = 0;
        /// <summary>
        /// 当前值
        /// </summary>
        [Description("当前值"), Category("数据"), DefaultValue(0F)]
        public float Value
        {
            get => _value;
            set
            {
                if (_value == value) return;
                _value = value;
                if (rect_stars.Length > 0)
                {
                    int _value_ = (int)_value;
                    for (int i = 0; i < rect_stars.Length; i++)
                    {
                        bool _active = _value_ > i, _active_ = _value > i;
                        rect_stars[i].Animatio(_active_, rect_stars[i].hover, _active_ && !_active);
                    }
                }
                Invalidate();
                ValueChanged?.Invoke(this, _value);
            }
        }

        /// <summary>
        /// Value 属性值更改时发生
        /// </summary>
        [Description("Value 属性值更改时发生"), Category("行为")]
        public event FloatEventHandler? ValueChanged;

        /// <summary>
        /// 自定义每项的提示信息
        /// </summary>
        [Description("自定义每项的提示信息"), Category("数据"), DefaultValue(null)]
        public string[]? Tooltips { get; set; }

        string? character = null;
        /// <summary>
        /// 自定义字符
        /// </summary>
        [Description("自定义字符"), Category("外观"), DefaultValue(null)]
        public string? Character
        {
            get => character;
            set
            {
                if (character == value) return;
                character = value;
                icon?.Dispose();
                icon_active?.Dispose();
                icon = icon_active = null;
                Invalidate();
            }
        }

        #endregion

        string icon_default = "<svg viewBox=\"64 64 896 896\"><path d=\"M908.1 353.1l-253.9-36.9L540.7 86.1c-3.1-6.3-8.2-11.4-14.5-14.5-15.8-7.8-35-1.3-42.9 14.5L369.8 316.2l-253.9 36.9c-7 1-13.4 4.3-18.3 9.3a32.05 32.05 0 00.6 45.3l183.7 179.1-43.4 252.9a31.95 31.95 0 0046.4 33.7L512 754l227.1 119.4c6.2 3.3 13.4 4.4 20.3 3.2 17.4-3 29.1-19.5 26.1-36.9l-43.4-252.9 183.7-179.1c5-4.9 8.3-11.3 9.3-18.3 2.7-17.5-9.5-33.7-27-36.3z\"></path></svg>";

        #region 渲染

        Bitmap? icon = null, icon_active = null;
        protected override void OnPaint(PaintEventArgs e)
        {
            var rect = ClientRectangle.PaddingRect(Padding);
            if (rect.Width == 0 || rect.Height == 0 || count < 1) return;

            int size = rect.Height;//6

            #region 初始化位图

            if (icon == null || icon.Width != size)
            {
                icon?.Dispose();
                icon = SvgExtend.SvgToBmp(character == null ? icon_default : character, size, size, Style.Db.FillSecondary);
            }
            if (icon_active == null || icon_active.Width != size)
            {
                icon_active?.Dispose();
                icon_active = SvgExtend.SvgToBmp(character == null ? icon_default : character, size, size, fill);
            }

            #endregion

            var g = e.Graphics.High();
            for (int i = 0; i < rect_stars.Length; i++)
            {
                var it = rect_stars[i];
                if (it.AnimationActive)
                {
                    int readvalue = (int)((it.rect.Width - it.rect_i.Width) * it.AnimationActiveValueS), readsize = it.rect_i.Width + readvalue, readsize2 = readvalue / 2;
                    var rect_ = new Rectangle(it.rect_i.X - readsize2, it.rect_i.Y - readsize2, readsize, readsize);
                    g.DrawImage(icon, rect_);
                    using (var attributes = new ImageAttributes())
                    {
                        var matrix = new ColorMatrix { Matrix33 = it.AnimationActiveValueO };
                        attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                        if (it.half) g.DrawImage(icon_active, new Rectangle(rect_.X, rect_.Y, rect_.Width / 2, rect_.Height), 0, 0, icon_active.Width / 2, icon_active.Height, GraphicsUnit.Pixel, attributes);
                        else g.DrawImage(icon_active, rect_, 0, 0, icon_active.Width, icon_active.Height, GraphicsUnit.Pixel, attributes);
                    }
                }
                else if (it.hover)
                {
                    if (it.half)
                    {
                        g.DrawImage(icon, it.rect);
                        g.DrawImage(icon_active, new Rectangle(it.rect.X, it.rect.Y, it.rect.Width / 2, it.rect.Height), new Rectangle(0, 0, icon_active.Width / 2, icon_active.Height), GraphicsUnit.Pixel);
                    }
                    else g.DrawImage(icon_active, it.rect);
                }
                else if (it.active)
                {
                    if (it.half)
                    {
                        g.DrawImage(icon, it.rect_i);
                        g.DrawImage(icon_active, new Rectangle(it.rect_i.X, it.rect_i.Y, it.rect_i.Width / 2, it.rect_i.Height), new Rectangle(0, 0, icon_active.Width / 2, icon_active.Height), GraphicsUnit.Pixel);
                    }
                    else g.DrawImage(icon_active, it.rect_i);
                }
                else g.DrawImage(icon, it.rect_i);
            }
        }

        #endregion

        #region 坐标

        RectStar[] rect_stars = new RectStar[0];
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            var rect = ClientRectangle.PaddingRect(Padding);
            if (rect.Width == 0 || rect.Height == 0 || count < 1) return;

            int size = rect.Height, msize = size - (int)(size * 0.8F), msize2 = msize / 2, gap = (int)(8F * Config.Dpi), t_size = size + gap;
            var list = new List<RectStar>(count);
            int _value_ = (int)_value;
            for (int i = 0; i < count; i++)
            {
                bool _active = _value_ > i, _active_ = _value > i;
                var it = new RectStar(this, new Rectangle(rect.X + t_size * i, rect.Y, t_size, size), new Rectangle(rect.X + t_size * i, rect.Y, size, size), msize, msize2)
                {
                    active = _active_,
                };
                if (_active_ && !_active) it.half = true;
                list.Add(it);
            }
            rect_stars = list.ToArray();
            if (autoSize)
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        Width = list[list.Count - 1].rect.Right;
                    }));
                }
                else Width = list[list.Count - 1].rect.Right;
            }
        }
        class RectStar
        {
            Rate rate;
            public RectStar(Rate _rate, Rectangle _rect_mouse, Rectangle _rect, int msize, int msize2)
            {
                rate = _rate;
                rect_mouse = _rect_mouse;
                rect = _rect;
                rect_i = new Rectangle(_rect.X + msize2, _rect.Y + msize2, _rect.Width - msize, _rect.Height - msize);
            }
            public Rectangle rect_mouse { get; set; }
            public Rectangle rect { get; set; }
            public Rectangle rect_i { get; set; }

            #region 动画

            /// <summary>
            /// 是否移动
            /// </summary>
            internal bool hover = false;
            /// <summary>
            /// 是否激活
            /// </summary>
            internal bool active = false;

            internal bool half = false;

            internal float AnimationActiveValueO = 0;
            internal float AnimationActiveValueS = 0;
            internal bool AnimationActive = false;
            ITask? ThreadActive = null;
            internal bool Animatio(bool _active, bool _hover, bool _half)
            {
                if (active == _active && hover == _hover)
                {
                    if (half != _half) half = _half; rate.Invalidate();
                    return false;
                }
                bool active_old = active, hover_old = hover;
                active = _active;
                hover = _hover;
                half = _half;
                if (Config.Animation)
                {
                    //System.Diagnostics.Debug.WriteLine(i + " [active_old]=" + active_old + " [hover_old]=" + hover_old + "[active]=" + active + " [hover]=" + hover);
                    ThreadActive?.Dispose();
                    AnimationActive = true;
                    var t = Animation.TotalFrames(10, 100);
                    if (active || hover)
                    {
                        if (active && hover)
                        {
                            ThreadActive = new ITask((i) =>
                            {
                                AnimationActiveValueS = AnimationActiveValueO = Animation.Animate(i, t, 1F, AnimationType.Ball);
                                rate.Invalidate();
                                return true;
                            }, 10, t, () =>
                            {
                                AnimationActive = false;
                                AnimationActiveValueS = AnimationActiveValueO = 1;
                                rate.Invalidate();
                            });
                        }
                        else
                        {
                            if (active_old && hover_old)
                            {
                                //仅缩小
                                ThreadActive = new ITask((i) =>
                                {
                                    AnimationActiveValueS = 1F - Animation.Animate(i, t, 1F, AnimationType.Ball);
                                    rate.Invalidate();
                                    return true;
                                }, 10, t, () =>
                                {
                                    AnimationActive = false;
                                    AnimationActiveValueS = 0;
                                    AnimationActiveValueO = 1;
                                    rate.Invalidate();
                                });
                            }
                            else
                            {
                                ThreadActive = new ITask((i) =>
                                {
                                    AnimationActiveValueO = Animation.Animate(i, t, 1F, AnimationType.Ball);
                                    rate.Invalidate();
                                    return true;
                                }, 10, t, () =>
                                {
                                    AnimationActive = false;
                                    AnimationActiveValueS = 0;
                                    AnimationActiveValueO = 1;
                                    rate.Invalidate();
                                });
                            }
                        }
                    }
                    else
                    {
                        if (active_old && !hover_old)
                        {
                            //仅不透明
                            ThreadActive = new ITask((i) =>
                            {
                                AnimationActiveValueO = 1F - Animation.Animate(i, t, 1F, AnimationType.Ball);
                                rate.Invalidate();
                                return true;
                            }, 10, t, () =>
                            {
                                AnimationActive = false;
                                AnimationActiveValueS = AnimationActiveValueO = 0;
                                rate.Invalidate();
                            });
                        }
                        else
                        {
                            ThreadActive = new ITask((i) =>
                            {
                                AnimationActiveValueS = AnimationActiveValueO = 1F - Animation.Animate(i, t, 1F, AnimationType.Ball);
                                rate.Invalidate();
                                return true;
                            }, 10, t, () =>
                            {
                                AnimationActive = false;
                                AnimationActiveValueS = AnimationActiveValueO = 0;
                                rate.Invalidate();
                            });
                        }
                    }
                }
                else rate.Invalidate();
                return true;
            }

            #endregion
        }

        #endregion

        #region 自动大小

        bool autoSize = false;
        [Browsable(true)]
        [Description("自动宽度"), Category("外观"), DefaultValue(false)]
        public override bool AutoSize
        {
            get => autoSize;
            set
            {
                if (autoSize == value) return;
                autoSize = value;
                if (value) OnSizeChanged(EventArgs.Empty);
            }
        }

        #endregion

        #region 鼠标

        #region 提示

        TooltipForm? tooltipForm = null;
        string? tooltipText = null;
        void ShowTips(RectangleF dot_rect, string text)
        {
            if (text == tooltipText && tooltipForm != null) return;
            tooltipText = text;
            var _rect = RectangleToScreen(ClientRectangle);
            var rect = new Rectangle(_rect.X + (int)dot_rect.X, _rect.Y + (int)dot_rect.Y, (int)dot_rect.Width, (int)dot_rect.Height);
            if (tooltipForm == null)
            {
                tooltipForm = new TooltipForm(rect, tooltipText, new TooltipConfig
                {
                    Font = Font,
                    ArrowAlign = TAlign.Top,
                });
                tooltipForm.Show(this);
            }
            else
            {
                tooltipForm.SetText(rect, tooltipText);
            }
        }

        void CloseTips()
        {
            tooltipForm?.IClose();
            tooltipForm = null;
        }

        #endregion
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            for (int i = 0; i < rect_stars.Length; i++)
            {
                var it = rect_stars[i];
                bool hover = it.rect_mouse.Contains(e.Location);
                if (hover)
                {
                    bool half = false;
                    if (AllowHalf) half = new Rectangle(it.rect.X, it.rect.Y, it.rect.Width / 2, it.rect.Height).Contains(e.Location);
                    it.Animatio(true, true, half);
                    for (int j = 0; j < rect_stars.Length; j++)
                    {
                        if (i != j) rect_stars[j].Animatio(j < i, false, false);
                    }
                    if (Tooltips != null && Tooltips.Length > i) ShowTips(it.rect, Tooltips[i]);
                    else CloseTips();
                    return;
                }
            }
            //全部都没激活
            _Leave();
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _Leave();
        }

        void _Leave()
        {
            CloseTips();
            int _value_ = (int)_value;
            for (int i = 0; i < rect_stars.Length; i++)
            {
                bool _active = _value_ > i, _active_ = _value > i;
                rect_stars[i].Animatio(_active_, false, _active_ && !_active);
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                for (int i = 0; i < rect_stars.Length; i++)
                {
                    if (rect_stars[i].rect_mouse.Contains(e.Location))
                    {
                        var it = rect_stars[i];
                        if (AllowHalf)
                        {
                            if (new Rectangle(it.rect.X, it.rect.Y, it.rect.Width / 2, it.rect.Height).Contains(e.Location))
                            {
                                Value = i + 0.5F;
                                return;
                            }
                        }
                        int value = i + 1;
                        Value = value;
                        return;
                    }
                }
            }
            base.OnMouseClick(e);
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            icon?.Dispose();
            icon_active?.Dispose();
            base.Dispose(disposing);
        }
    }
}