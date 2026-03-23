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
    public class YScrollBar : IControl
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

        #region 纵向

        int valueY = 0;
        /// <summary>
        /// 当前值
        /// </summary>
        [Description("当前值"), Category("值"), DefaultValue(0)]
        public int Value
        {
            get => valueY;
            set
            {
                if (value < 0) value = 0;
                if (max > 0)
                {
                    int valueI = max - Height;
                    if (value > valueI) value = valueI;
                }
                if (valueY == value) return;
                valueY = value;
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

        bool hoverY = false;
        /// <summary>
        /// 滑动态
        /// </summary>
        [Description("滑动态"), Category("值"), DefaultValue(false)]
        public bool HoverY
        {
            get => hoverY;
            set
            {
                if (hoverY == value) return;
                hoverY = value;
                if (Config.HasAnimation(nameof(ScrollBar)))
                {
                    ThreadHoverY?.Dispose();
                    AnimationHoverY = true;
                    ThreadHoverY = new AnimationTask(new AnimationFixedConfig(i =>
                    {
                        AnimationHoverYValue = i;
                        Invalidate();
                    }, 10, Animation.TotalFrames(10, 100), value, AnimationType.Ball).SetEnd(() => AnimationHoverY = false));
                }
                else Invalidate();
            }
        }

        #endregion

        public void Clear() => valueY = 0;

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
                    if (Back && (hoverY || AnimationHoverY))
                    {
                        using (var brush = BackBrushY(baseColor))
                        {
                            if (Radius > 0)
                            {
                                float radius = Radius * Dpi;
                                using (var path = ClientRectangle.RoundPath(radius, false, true, false, false))
                                {
                                    g.Fill(brush, path);
                                }
                            }
                            else g.Fill(brush, ClientRectangle);
                        }
                    }
                    PaintY(g, baseColor);
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
                                using (var path = ClientRectangle.RoundPath(radius, false, true, false, false))
                                {
                                    g.Fill(brush, path);
                                }
                            }
                            else g.Fill(brush, ClientRectangle);
                        }
                    }
                    PaintY(g, baseColor);
                }
            }
        }

        SolidBrush BackBrushY(Color color)
        {
            if (AnimationHoverY) return new SolidBrush(Color.FromArgb((int)(10 * AnimationHoverYValue), color));
            else return new SolidBrush(Color.FromArgb(10, color));
        }
        void PaintY(Canvas g, Color color)
        {
            if (AnimationHoverY)
            {
                using (var brush = new SolidBrush(Color.FromArgb(110 + (int)((141 - 110) * AnimationHoverYValue), color)))
                {
                    var slider = RectSliderY();
                    using (var path = slider.RoundPath(slider.Height))
                    {
                        g.Fill(brush, path);
                    }
                }
            }
            else
            {
                int alpha;
                if (SliderDownY) alpha = 172;
                else alpha = hoverY ? 141 : 110;
                var slider = RectSliderY();
                using (var path = slider.RoundPath(slider.Height))
                {
                    g.Fill(Color.FromArgb(alpha, color), path);
                }
            }
        }

        #region 坐标

        RectangleF RectSliderY()
        {
            var Rect = ClientRectangle;
            float read = Rect.Height, height = (read / max) * read;
            float y = (valueY * 1.0F / (max - read)) * (read - height), gap = (Rect.Width - SIZE_BAR) / 2F;
            return new RectangleF(Rect.X + gap, Rect.Y + y + gap, SIZE_BAR, height - gap * 2);
        }

        RectangleF RectSliderFullY()
        {
            var Rect = ClientRectangle;
            float read = Rect.Height, height = (read / max) * read;
            float y = (valueY * 1.0F / (max - read)) * (read - height);
            return new RectangleF(Rect.X, Rect.Y + y, Rect.Width, height);
        }

        public int VrValueI => max - Height;

        public int ReadSize => Height;

        string? show_oldy;
        int oldy = 0;
        public bool SetSize(int height) => SetShow(oldy, height);
        public bool SetShow(int max) => SetShow(max, Height);
        public bool SetShow(int _max, int _height)
        {
            oldy = _max;
            string show_y = _max + "_" + _height;
            if (show_oldy == show_y) return false;
            show_oldy = show_y;
            if (_height > 0 && _max > 0 && _max > _height)
            {
                max = _max;
                bool show = max > _height;
                if (show)
                {
                    int valueI = _max - _height;
                    if (valueY > valueI) Value = valueI;
                }
                if (Visible != show)
                {
                    Visible = show;
                    return true;
                }
            }
            else
            {
                max = valueY = 0;
                if (Visible)
                {
                    Visible = false;
                    return true;
                }
            }
            return false;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            SetShow(oldY, Height);
        }

        #endregion

        #endregion

        #region 鼠标

        #region 按下

        int oldY;
        bool SliderDownY = false;
        float SliderY = 0;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            oldY = e.Y;
            var slider = RectSliderFullY();
            if (slider.Contains(e.X, e.Y)) SliderY = slider.X;
            else
            {
                float read = Height, y = (e.Y - slider.Height / 2F) / read;
                Value = (int)Math.Round(y * max);
                SliderY = RectSliderFullY().Y;
            }
            SliderDownY = true;
            Window.CanHandMessage = false;
        }

        #endregion

        #region 移动

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            HoverY = true;
            if (SliderDownY)
            {
                var slider = RectSliderFullY();
                float read = Height, y = SliderY + (e.Y - oldY);
                Value = (int)(y / (read - slider.Height) * (max - Height));
            }
        }

        #endregion

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (SliderDownY)
            {
                SliderDownY = false;
                Window.CanHandMessage = true;
            }
        }

        #region 滚动

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            bool result = MouseWheelY(e.Delta);
            if (result && e is HandledMouseEventArgs handled) handled.Handled = true;
            base.OnMouseWheel(e);
        }

        public bool MouseWheelY(int delta)
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
            HoverY = true;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            HoverY = false;
        }

        #endregion

        #region 动画

        AnimationTask? ThreadHoverY;
        float AnimationHoverYValue = 0F;
        bool AnimationHoverY = false;

        #endregion

        #region 方法

        /// <summary>
        /// 判断是否到达纵向滚动条最底部
        /// </summary>
        public bool IsAtBottom => Visible && valueY >= (max - Height);

        #endregion

        #region 事件

        public event EventHandler? ValueChanged;

        #endregion

        protected override void Dispose(bool disposing)
        {
            ThreadHoverY?.Dispose();
            base.Dispose(disposing);
        }
    }
}