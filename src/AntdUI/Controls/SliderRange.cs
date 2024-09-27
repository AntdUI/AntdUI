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

using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// SliderRange 滑动范围输入条
    /// </summary>
    /// <remarks>滑动型输入器，展示当前值和可选范围。</remarks>
    [Description("SliderRange 滑动范围输入条")]
    [ToolboxItem(true)]
    [DefaultProperty("Value")]
    [DefaultEvent("ValueChanged")]
    public class SliderRange : Slider
    {
        #region 属性

        int _value2 = 10;
        /// <summary>
        /// 当前值2
        /// </summary>
        [Description("当前值2"), Category("数据"), DefaultValue(10)]
        public int Value2
        {
            get => _value2;
            set
            {
                if (value < MinValue) value = MinValue;
                else if (value > MaxValue) value = MaxValue;
                if (_value2 == value) return;
                _value2 = value;
                Value2Changed?.Invoke(this, new IntEventArgs(_value2));
                Invalidate();
            }
        }

        /// <summary>
        /// Value 属性值更改时发生
        /// </summary>
        [Description("Value 属性值更改时发生"), Category("行为")]
        public event IntEventHandler? Value2Changed;

        #endregion

        #region 渲染

        internal override void IPaint(Graphics g, Rectangle rect, bool enabled, Color color, Color color_dot, Color color_hover, Color color_active)
        {
            float prog = ProgValue(Value), prog2 = ProgValue(_value2);

            #region 线条

            using (var path = rect_read.RoundPath(rect_read.Height / 2))
            {
                using (var brush = new SolidBrush(Style.Db.FillQuaternary))
                {
                    g.FillPath(brush, path);
                    if (AnimationHover)
                    {
                        using (var brush_hover = new SolidBrush(Helper.ToColorN(AnimationHoverValue, brush.Color)))
                        {
                            g.FillPath(brush_hover, path);
                        }
                    }
                    else if (ExtraMouseHover) g.FillPath(brush, path);
                }

                if (prog != prog2)
                {
                    g.SetClip(RectLine(rect_read, prog, prog2));
                    if (AnimationHover)
                    {
                        using (var brush = new SolidBrush(color))
                        {
                            g.FillPath(brush, path);
                        }
                        using (var brush = new SolidBrush(Helper.ToColor(255 * AnimationHoverValue, color_hover)))
                        {
                            g.FillPath(brush, path);
                        }
                    }
                    else
                    {
                        using (var brush = new SolidBrush(ExtraMouseHover ? color_hover : color))
                        {
                            g.FillPath(brush, path);
                        }
                    }
                    g.ResetClip();
                }
            }

            #endregion

            using (var brush = new SolidBrush(Style.Db.BgBase))
            {
                PaintMarksEllipse(g, rect, rect_read, brush, color, LineSize);
                PaintEllipse(g, rect, rect_read, prog, brush, color_dot, color_hover, color_active, LineSize);
                if (prog != prog2) PaintEllipse2(g, rect, rect_read, prog2, brush, color_dot, color_hover, color_active, LineSize);
            }
        }

        RectangleF rectEllipse2;
        internal void PaintEllipse2(Graphics g, Rectangle rect, RectangleF rect_read, float prog, SolidBrush brush, Color color, Color color_hover, Color color_active, int LineSize)
        {
            int DotSize = (int)(dotSize * Config.Dpi), DotSizeActive = (int)(dotSizeActive * Config.Dpi);
            rectEllipse2 = RectDot(rect, rect_read, prog, DotSizeActive + LineSize);

            var rect_ellipse_rl = RectDot(rect, rect_read, prog, DotSize + LineSize);
            if (ShowValue && ExtraMouseDot2Hover) ShowTips(_value2, rect_ellipse_rl);

            if (AnimationDot2Hover)
            {
                float value = ((DotSizeActive - DotSize) * AnimationDot2HoverValue);
                using (var brush_shadow = new SolidBrush(color_active.rgba(.2F)))
                {
                    g.FillEllipse(brush_shadow, RectDot(rect, rect_read, prog, DotSizeActive + LineSize + LineSize * 2 * AnimationDot2HoverValue));
                }
                using (var brush_dot = new SolidBrush(color_active))
                {
                    g.FillEllipse(brush_dot, RectDot(rect, rect_read, prog, DotSize + LineSize + value));
                }
                g.FillEllipse(brush, RectDot(rect, rect_read, prog, DotSize + value));
            }
            else if (ExtraMouseDot2Hover)
            {
                using (var brush_shadow = new SolidBrush(color_active.rgba(.2F)))
                {
                    g.FillEllipse(brush_shadow, RectDot(rect, rect_read, prog, DotSizeActive + LineSize * 3));
                }
                using (var brush_dot = new SolidBrush(color_active))
                {
                    g.FillEllipse(brush_dot, RectDot(rect, rect_read, prog, DotSizeActive + LineSize));
                }
                g.FillEllipse(brush, RectDot(rect, rect_read, prog, DotSizeActive));
            }
            else
            {
                if (AnimationHover)
                {
                    using (var brush_dot_old = new SolidBrush(color))
                    using (var brush_dot = new SolidBrush(Helper.ToColor(255 * AnimationHoverValue, color_hover)))
                    {
                        var rect_dot = RectDot(rect, rect_read, prog, DotSize + LineSize);
                        g.FillEllipse(brush_dot_old, rect_dot);
                        g.FillEllipse(brush_dot, rect_dot);
                    }
                }
                else
                {
                    using (var brush_dot = new SolidBrush(ExtraMouseHover ? color_hover : color))
                    {
                        g.FillEllipse(brush_dot, RectDot(rect, rect_read, prog, DotSize + LineSize));
                    }
                }
                g.FillEllipse(brush, RectDot(rect, rect_read, prog, DotSize));
            }
        }

        internal RectangleF RectLine(RectangleF rect, float prog, float prog2)
        {
            switch (Align)
            {
                case TAlignMini.Right:
                    return new RectangleF(rect.X + rect.Width - prog2, rect.Y, prog2 - prog, rect.Height);
                case TAlignMini.Top:
                    return new RectangleF(rect.X, rect.Y + prog, rect.Width, prog2 - prog);
                case TAlignMini.Bottom:
                    return new RectangleF(rect.X, rect.Y + rect.Height - prog2, rect.Width, prog2 - prog);
                default:
                    return new RectangleF(rect.X + prog, rect.Y, prog2 - prog, rect.Height);
            }
        }

        #endregion

        #region 鼠标

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (rectEllipse.Contains(e.X, e.Y))
                {
                    base.OnMouseDown(e);
                    return;
                }
                if (rectEllipse2.Contains(e.X, e.Y))
                {
                    Value2 = FindIndex(e.X, e.Y, true);
                    mouseFlat = true;
                    return;
                }
                var mark_list = new List<float>(2);
                int i = 0;
                int max = MaxValue - MinValue;
                switch (Align)
                {
                    case TAlignMini.Right:
                        mark_list.Add(rect_read.Width - (Value >= MaxValue ? rect_read.Width : rect_read.Width * ((Value - MinValue) * 1F / max)));
                        mark_list.Add(rect_read.Width - (_value2 >= MaxValue ? rect_read.Width : rect_read.Width * ((_value2 - MinValue) * 1F / max)));
                        i = FindNumber(e.X, mark_list);
                        break;
                    case TAlignMini.Top:
                        mark_list.Add(Value >= MaxValue ? rect_read.Height : rect_read.Height * ((Value - MinValue) * 1F / max));
                        mark_list.Add(_value2 >= MaxValue ? rect_read.Height : rect_read.Height * ((_value2 - MinValue) * 1F / max));
                        i = FindNumber(e.Y, mark_list);
                        break;
                    case TAlignMini.Bottom:
                        mark_list.Add(rect_read.Height - (Value >= MaxValue ? rect_read.Height : rect_read.Height * ((Value - MinValue) * 1F / max)));
                        mark_list.Add(rect_read.Height - (_value2 >= MaxValue ? rect_read.Height : rect_read.Height * ((_value2 - MinValue) * 1F / max)));
                        i = FindNumber(e.Y, mark_list);
                        break;
                    default:
                        mark_list.Add(Value >= MaxValue ? rect_read.Width : rect_read.Width * ((Value - MinValue) * 1F / max));
                        mark_list.Add(_value2 >= MaxValue ? rect_read.Width : rect_read.Width * ((_value2 - MinValue) * 1F / max));
                        i = FindNumber(e.X, mark_list);
                        break;
                }
                if (i == 1)
                {
                    Value2 = FindIndex(e.X, e.Y, true);
                    mouseFlat = true;
                    return;
                }
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (mouseFlat)
            {
                ExtraMouseDot2Hover = true;
                Value2 = FindIndex(e.X, e.Y, false);
            }
            else ExtraMouseDot2Hover = rectEllipse2.Contains(e.X, e.Y);
        }

        bool mouseFlat = false;
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            mouseFlat = false;
            Invalidate();
        }

        internal float AnimationDot2HoverValue = 0F;
        internal bool AnimationDot2Hover = false;
        bool _mouseDotHover = false;
        internal bool ExtraMouseDot2Hover
        {
            get => _mouseDotHover;
            set
            {
                if (_mouseDotHover == value) return;
                _mouseDotHover = value;
                if (!value) CloseTips();
                if (Config.Animation)
                {
                    ThreadHover?.Dispose();
                    ThreadHover = null;
                    ThreadDot2Hover?.Dispose();
                    AnimationDot2Hover = true;
                    if (value)
                    {
                        ThreadDot2Hover = new ITask(this, () =>
                        {
                            AnimationDot2HoverValue = AnimationDot2HoverValue.Calculate(0.1F);
                            if (AnimationDot2HoverValue > 1) { AnimationDot2HoverValue = 1; return false; }
                            Invalidate();
                            return true;
                        }, 10, () =>
                        {
                            AnimationDot2Hover = false;
                            Invalidate();
                        });
                    }
                    else
                    {
                        ThreadDot2Hover = new ITask(this, () =>
                        {
                            AnimationDot2HoverValue = AnimationDot2HoverValue.Calculate(-0.1F);
                            if (AnimationDot2HoverValue <= 0) { AnimationDot2HoverValue = 0F; return false; }
                            Invalidate();
                            return true;
                        }, 10, () =>
                        {
                            AnimationDot2Hover = false;
                            Invalidate();
                        });
                    }
                }
                else Invalidate();
            }
        }

        #region 动画

        protected override void Dispose(bool disposing)
        {
            ThreadDot2Hover?.Dispose();
            base.Dispose(disposing);
        }
        ITask? ThreadDot2Hover = null;

        #endregion

        #endregion
    }
}