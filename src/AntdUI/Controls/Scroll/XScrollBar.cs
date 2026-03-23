// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AntdUI
{
    [ToolboxItem(true)]
    public class XScrollBar : IControl
    {
        #region 属性

        /// <summary>
        /// 是否显示背景
        /// </summary>
        [Description("是否显示背景"), Category("外观"), DefaultValue(true)]
        public bool Back { get; set; } = true;

        /// <summary>
        /// 圆角
        /// </summary>
        [Description("圆角"), Category("外观"), DefaultValue(6)]
        public int Radius { get; set; } = 6;

        /// <summary>
        /// 常态下滚动条大小
        /// </summary>
        [Description("常态下滚动条大小"), Category("外观"), DefaultValue(6)]
        public int SIZE_BAR { get; set; } = 6;

        #endregion

        #region 横向

        int valueX = 0;
        /// <summary>
        /// 当前值
        /// </summary>
        [Description("当前值"), Category("值"), DefaultValue(0)]
        public int Value
        {
            get => valueX;
            set
            {
                if (value < 0) value = 0;
                if (max > 0)
                {
                    int valueI = max - Width;
                    if (value > valueI) value = valueI;
                }
                if (valueX == value) return;
                valueX = value;
                ValueChanged?.Invoke(this, EventArgs.Empty);
                Invalidate();
            }
        }

        int max = 100;
        /// <summary>
        /// 最大值
        /// </summary>
        [Description("最大值"), Category("值"), DefaultValue(100)]
        public int Maximum
        {
            get => max;
            set
            {
                if (max == value) return;
                max = value;
                Invalidate();
            }
        }

        bool hoverX = false;
        /// <summary>
        /// 滑动态
        /// </summary>
        [Description("滑动态"), Category("值"), DefaultValue(false)]
        public bool HoverX
        {
            get => hoverX;
            set
            {
                if (hoverX == value) return;
                hoverX = value;
                if (Config.HasAnimation(nameof(ScrollBar)))
                {
                    ThreadHoverX?.Dispose();
                    AnimationHoverX = true;
                    ThreadHoverX = new AnimationTask(new AnimationFixedConfig(i =>
                    {
                        AnimationHoverXValue = i;
                        Invalidate();
                    }, 10, Animation.TotalFrames(10, 100), value, AnimationType.Ball).SetEnd(() => AnimationHoverX = false));
                }
                else Invalidate();
            }
        }

        #endregion

        public void Clear() => valueX = 0;

        #region 渲染

        protected override void OnDraw(DrawEventArgs e)
        {
            base.OnDraw(e);
            if (Enabled)
            {
                var g = e.Canvas;
                var baseColor = Colour.TextBase.Get(nameof(ScrollBar));
                if (Config.ScrollBarHide)
                {
                    if (Back && (hoverX || AnimationHoverX))
                    {
                        using (var brush = BackBrushX(baseColor))
                        {
                            if (Radius > 0)
                            {
                                float radius = Radius * Dpi;
                                using (var path = ClientRectangle.RoundPath(radius, false, false, true, true))
                                {
                                    g.Fill(brush, path);
                                }
                            }
                            else g.Fill(brush, ClientRectangle);
                        }
                    }
                    PaintX(g, baseColor);
                }
                else
                {
                    if (Back)
                    {
                        using (var brush = new SolidBrush(Color.FromArgb(10, baseColor)))
                        {
                            if (Radius > 0)
                            {
                                float radius = Radius * Dpi;
                                using (var path = ClientRectangle.RoundPath(radius, false, false, true, true))
                                {
                                    g.Fill(brush, path);
                                }
                            }
                            else g.Fill(brush, ClientRectangle);
                        }
                    }
                    PaintX(g, baseColor);
                }
            }
        }

        SolidBrush BackBrushX(Color color)
        {
            if (AnimationHoverX) return new SolidBrush(Color.FromArgb((int)(10 * AnimationHoverXValue), color));
            else return new SolidBrush(Color.FromArgb(10, color));
        }
        void PaintX(Canvas g, Color color)
        {
            if (AnimationHoverX)
            {
                using (var brush = new SolidBrush(Color.FromArgb(110 + (int)((141 - 110) * AnimationHoverXValue), color)))
                {
                    var slider = RectSliderX();
                    using (var path = slider.RoundPath(slider.Height))
                    {
                        g.Fill(brush, path);
                    }
                }
            }
            else
            {
                int alpha;
                if (SliderDownX) alpha = 172;
                else alpha = hoverX ? 141 : 110;
                var slider = RectSliderX();
                using (var path = slider.RoundPath(slider.Height))
                {
                    g.Fill(Color.FromArgb(alpha, color), path);
                }
            }
        }

        #region 坐标

        RectangleF RectSliderX()
        {
            var Rect = ClientRectangle;
            float read = Rect.Width, width = (read / max) * read;
            float x = (valueX * 1.0F / (max - read)) * (read - width), gap = (Rect.Height - SIZE_BAR) / 2F;
            return new RectangleF(Rect.X + x + gap, Rect.Y + gap, width - gap * 2, SIZE_BAR);
        }

        RectangleF RectSliderFullX()
        {
            var Rect = ClientRectangle;
            float read = Rect.Width, width = (read / max) * read;
            float x = (valueX * 1.0F / (max - read)) * (read - width);
            return new RectangleF(Rect.X + x, Rect.Y, width, Rect.Height);
        }

        public int VrValueI => max - Width;

        public int ReadSize => Width;

        string? show_oldx;
        int oldx = 0;
        public bool SetSize(int width) => SetShow(oldx, width);
        public bool SetShow(int max) => SetShow(max, Width);
        public bool SetShow(int _max, int _width)
        {
            oldx = _max;
            string show_x = _max + "_" + _width;
            if (show_oldx == show_x) return false;
            show_oldx = show_x;
            if (_width > 0 && _max > 0 && _max > _width)
            {
                max = _max;
                bool show = max > _width;
                if (show)
                {
                    int valueI = _max - _width;
                    if (valueX > valueI) Value = valueI;
                }
                if (Visible != show)
                {
                    Visible = show;
                    return true;
                }
            }
            else
            {
                max = valueX = 0;
                if (Visible)
                {
                    Visible = false;
                    return true;
                }
            }
            return false;
        }

        #endregion

        #endregion

        #region 鼠标

        #region 按下

        int oldX;
        bool SliderDownX = false;
        float SliderX = 0;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            oldX = e.X;
            var slider = RectSliderFullX();
            if (slider.Contains(e.X, e.Y)) SliderX = slider.X;
            else
            {
                float read = Width, x = (e.X - slider.Width / 2F) / read;
                Value = (int)Math.Round(x * max);
                SliderX = RectSliderFullX().X;
            }
            SliderDownX = true;
            Window.CanHandMessage = false;
        }

        #endregion

        #region 移动

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            HoverX = true;
            if (SliderDownX)
            {
                var slider = RectSliderFullX();
                float read = Width, x = SliderX + (e.X - oldX);
                Value = (int)(x / (read - slider.Width) * (max - Width));
            }
        }

        #endregion

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (SliderDownX)
            {
                SliderDownX = false;
                Window.CanHandMessage = true;
            }
        }

        #region 滚动

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            bool result = MouseWheelX(e.Delta);
            if (result && e is HandledMouseEventArgs handled) handled.Handled = true;
            base.OnMouseWheel(e);
        }

        public bool MouseWheelX(int delta)
        {
            if (delta == 0) return false;
            int value = Value - delta;
            Value = value;
            if (Value != value) return false;
            return true;
        }

        #endregion

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            HoverX = true;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            HoverX = false;
        }

        #endregion

        #region 动画

        AnimationTask? ThreadHoverX;
        float AnimationHoverXValue = 0F;
        bool AnimationHoverX = false;

        #endregion

        #region 方法

        /// <summary>
        /// 判断是否到达横向滚动条最右边
        /// </summary>
        public bool IsAtRight => Visible && valueX >= (max - Width);

        #endregion

        #region 事件

        public event EventHandler? ValueChanged;

        #endregion

        protected override void Dispose(bool disposing)
        {
            ThreadHoverX?.Dispose();
            base.Dispose(disposing);
        }
    }
}