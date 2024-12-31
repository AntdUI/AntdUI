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
using System.Drawing;
using System.Windows.Forms;

namespace AntdUI
{
    public class ScrollBar
    {
        #region 属性

        internal int Radius { get; set; }
        internal bool RB { get; set; }

        #region 布局容器

        public ScrollBar(FlowPanel control, bool enabledY = true, bool enabledX = false)
        {
            OnInvalidate = ChangeSize = () =>
            {
                control.IOnSizeChanged();
            };
            Invalidate = rect =>
            {
                OnInvalidate?.Invoke();
            };
            EnabledX = enabledX;
            EnabledY = enabledY;
            Init();
        }
        public ScrollBar(StackPanel control)
        {
            OnInvalidate = ChangeSize = () =>
            {
                control.IOnSizeChanged();
            };
            Invalidate = rect =>
            {
                OnInvalidate?.Invoke();
            };
            if (control.Vertical) EnabledY = true;
            else EnabledX = true;
            Init();
        }

        #endregion

        public ScrollBar(IControl control, bool enabledY = true, bool enabledX = false, int radius = 0, bool radiusy = false)
        {
            Radius = radius;
            RB = radiusy;
            Invalidate = rect =>
            {
                OnInvalidate?.Invoke();
                if (rect.HasValue) control.Invalidate(rect.Value);
                else control.Invalidate();
            };
            ChangeSize = () =>
            {
                control.IOnSizeChanged();
            };
            EnabledX = enabledX;
            EnabledY = enabledY;
            Init();
        }

        public ScrollBar(Action change, Action<Rectangle?> invalidate, bool enabledY = true, bool enabledX = false)
        {
            EnabledX = enabledX;
            EnabledY = enabledY;
            ChangeSize = change;
            Invalidate = invalidate;
            Init();
        }

        void Init()
        {
            SIZE = (int)(16 * Config.Dpi);
            SIZE_BAR = (int)(6 * Config.Dpi);
            SIZE_MINIY = (int)(Config.ScrollMinSizeY * Config.Dpi);
        }

        Action? ChangeSize;
        Action<Rectangle?> Invalidate;
        internal Action? OnInvalidate;

        /// <summary>
        /// 是否显示背景
        /// </summary>
        public bool Back { get; set; } = true;

        /// <summary>
        /// 滚动条大小
        /// </summary>
        public int SIZE { get; set; } = 20;

        /// <summary>
        /// 常态下滚动条大小
        /// </summary>
        public int SIZE_BAR { get; set; } = 8;

        public int SIZE_MINIY { get; set; } = 30;

        #endregion

        #region 纵向

        /// <summary>
        /// 是否启用纵向滚动条
        /// </summary>
        public bool EnabledY { get; set; }

        Rectangle RectY;

        bool showY = false;
        /// <summary>
        /// 是否显示纵向滚动条
        /// </summary>
        public bool ShowY
        {
            get => showY;
            set
            {
                if (showY == value) return;
                showY = value;
                Invalidate(null);
                ChangeSize?.Invoke();
            }
        }

        int valueY = 0;
        /// <summary>
        /// 当前值纵向滚动条
        /// </summary>
        public int ValueY
        {
            get => valueY;
            set
            {
                if (value < 0) value = 0;
                if (maxY > 0)
                {
                    int valueI = maxY - RectY.Height;
                    if (value > valueI) value = valueI;
                }
                if (valueY == value) return;
                valueY = value;
                Invalidate(null);
            }
        }

        int maxY = 0;
        /// <summary>
        /// 当前值纵向滚动条
        /// </summary>
        public int MaxY
        {
            get => maxY;
            set
            {
                if (maxY == value) return;
                maxY = value;
                Invalidate(null);
            }
        }

        bool hoverY = false;
        /// <summary>
        /// 滑动态纵向滚动条y
        /// </summary>
        public bool HoverY
        {
            get => hoverY;
            set
            {
                if (hoverY == value) return;
                hoverY = value;
                if (Config.Animation)
                {
                    ThreadHoverY?.Dispose();
                    AnimationHoverY = true;
                    var t = Animation.TotalFrames(10, 100);
                    if (value)
                    {
                        ThreadHoverY = new ITask((i) =>
                        {
                            AnimationHoverYValue = Animation.Animate(i, t, 1F, AnimationType.Ball);
                            Invalidate(RectY);
                            return true;
                        }, 10, t, () =>
                        {
                            AnimationHoverYValue = 1F;
                            AnimationHoverY = false;
                            Invalidate(RectY);
                        });
                    }
                    else
                    {
                        ThreadHoverY = new ITask((i) =>
                        {
                            AnimationHoverYValue = 1F - Animation.Animate(i, t, 1F, AnimationType.Ball);
                            Invalidate(RectY);
                            return true;
                        }, 10, t, () =>
                        {
                            AnimationHoverYValue = 0F;
                            AnimationHoverY = false;
                            Invalidate(RectY);
                        });
                    }
                }
                else Invalidate(RectY);
            }
        }

        #endregion

        #region 横向

        /// <summary>
        /// 是否启用横向滚动条
        /// </summary>
        public bool EnabledX { get; set; }

        Rectangle RectX;

        bool showX = false;
        /// <summary>
        /// 是否显示横向滚动条
        /// </summary>
        public bool ShowX
        {
            get => showX;
            set
            {
                if (showX == value) return;
                showX = value;
                Invalidate(null);
                ChangeSize?.Invoke();
            }
        }

        int valueX = 0;
        /// <summary>
        /// 当前值横向滚动条
        /// </summary>
        public int ValueX
        {
            get => valueX;
            set
            {
                if (value < 0) value = 0;
                if (maxX > 0)
                {
                    int valueI = maxX - RectX.Width;
                    if (value > valueI) value = valueI;
                }
                if (valueX == value) return;
                valueX = value;
                Invalidate(null);
            }
        }

        int maxX = 0;
        /// <summary>
        /// 当前值横向滚动条
        /// </summary>
        public int MaxX
        {
            get => maxX;
            set
            {
                if (maxX == value) return;
                maxX = value;
                Invalidate(null);
            }
        }

        bool hoverX = false;
        /// <summary>
        /// 滑动态横向滚动条
        /// </summary>
        public bool HoverX
        {
            get => hoverX;
            set
            {
                if (hoverX == value) return;
                hoverX = value;
                if (Config.Animation)
                {
                    ThreadHoverX?.Dispose();
                    AnimationHoverX = true;
                    var t = Animation.TotalFrames(10, 100);
                    if (value)
                    {
                        ThreadHoverX = new ITask((i) =>
                        {
                            AnimationHoverXValue = Animation.Animate(i, t, 1F, AnimationType.Ball);
                            Invalidate(RectX);
                            return true;
                        }, 10, t, () =>
                        {
                            AnimationHoverXValue = 1F;
                            AnimationHoverX = false;
                            Invalidate(RectX);
                        });
                    }
                    else
                    {
                        ThreadHoverX = new ITask((i) =>
                        {
                            AnimationHoverXValue = 1F - Animation.Animate(i, t, 1F, AnimationType.Ball);
                            Invalidate(RectX);
                            return true;
                        }, 10, t, () =>
                        {
                            AnimationHoverXValue = 0F;
                            AnimationHoverX = false;
                            Invalidate(RectX);
                        });
                    }
                }
                else Invalidate(RectX);
            }
        }

        public void Clear()
        {
            valueX = valueY = 0;
        }

        #endregion

        #region 布局

        /// <summary>
        /// 设置容器大小
        /// </summary>
        public void SizeChange(Rectangle rect)
        {
            RectX = new Rectangle(rect.X, rect.Bottom - SIZE, rect.Width, SIZE);
            RectY = new Rectangle(rect.Right - SIZE, rect.Top, SIZE, rect.Height);
            SetShow(oldx, oldy);
        }

        int oldx = 0, oldy = 0;
        /// <summary>
        /// 设置容器虚拟宽度
        /// </summary>
        public void SetVrSize(int x, int y)
        {
            oldx = x; oldy = y;
            SetShow(oldx, oldy);
        }

        #region 设置是否显示

        string show_oldx = "", show_oldy = "";
        void SetShow(int x, int y)
        {
            SetShow(x, RectX.Width, y, RectY.Height);
        }
        void SetShow(int x, int x2, int y, int y2)
        {
            string show_x = x + "_" + x2, show_y = y + "_" + y2;
            if (show_oldx == show_x && show_oldy == show_y) return;
            show_oldx = show_x;
            show_oldy = show_y;
            if (x2 > 0 && x > 0 && x > x2)
            {
                maxX = x;
                ShowX = maxX > x2;
                if (ShowX)
                {
                    int valueI = x - x2;
                    if (valueX > valueI) ValueX = valueI;
                }
            }
            else
            {
                maxX = valueX = 0;
                ShowX = false;
            }
            if (y2 > 0 && y > 0 && y > y2)
            {
                maxY = y;
                ShowY = maxY > y2;
                if (ShowY)
                {
                    int valueI = y - y2;
                    if (valueY > valueI) ValueY = valueI;
                }
            }
            else
            {
                maxY = valueY = 0;
                ShowY = false;
            }

            if (showX && showY)
            {
                maxX += SIZE;
                maxY += SIZE;
            }
        }

        #endregion

        #endregion

        #region 渲染

        public virtual void Paint(Canvas g)
        {
            Paint(g, Colour.TextBase.Get("ScrollBar"));
        }
        public virtual void Paint(Canvas g, Color baseColor)
        {
            if (Config.ScrollBarHide)
            {
                if (showY && showX)
                {
                    if (Back && ((hoverY || AnimationHoverY) || (hoverX || AnimationHoverX)))
                    {
                        if (hoverY || AnimationHoverY)
                        {
                            using (var brush = BackBrushY(baseColor))
                            {
                                if (Radius > 0)
                                {
                                    float radius = Radius * Config.Dpi;
                                    using (var path = Helper.RoundPath(RectY, radius, false, true, RB, false))
                                    {
                                        g.Fill(brush, path);
                                    }
                                }
                                else g.Fill(brush, RectY);
                            }
                        }
                        else
                        {
                            using (var brush = BackBrushX(baseColor))
                            {
                                if (RB && Radius > 0)
                                {
                                    float radius = Radius * Config.Dpi;
                                    using (var path = Helper.RoundPath(new Rectangle(RectX.X, RectX.Y, RectX.Width, RectX.Height), radius, false, false, true, true))
                                    {
                                        g.Fill(brush, path);
                                    }
                                }
                                else g.Fill(brush, new Rectangle(RectX.X, RectX.Y, RectX.Width, RectX.Height));
                            }
                        }
                    }
                    PaintX(g, baseColor);
                    PaintY(g, baseColor);
                }
                else if (showY)
                {
                    if (Back && (hoverY || AnimationHoverY))
                    {
                        using (var brush = BackBrushY(baseColor))
                        {
                            if (Radius > 0)
                            {
                                float radius = Radius * Config.Dpi;
                                using (var path = Helper.RoundPath(RectY, radius, false, true, RB, false))
                                {
                                    g.Fill(brush, path);
                                }
                            }
                            else g.Fill(brush, RectY);
                        }
                    }
                    PaintY(g, baseColor);
                }
                else if (showX)
                {
                    if (Back && (hoverX || AnimationHoverX))
                    {
                        using (var brush = BackBrushX(baseColor))
                        {
                            if (RB && Radius > 0)
                            {
                                float radius = Radius * Config.Dpi;
                                using (var path = Helper.RoundPath(new Rectangle(RectX.X, RectX.Y, RectX.Width, RectX.Height), radius, false, false, true, true))
                                {
                                    g.Fill(brush, path);
                                }
                            }
                            else g.Fill(brush, RectX);
                        }
                    }
                    PaintX(g, baseColor);
                }
            }
            else
            {
                if (showY && showX)
                {
                    if (Back)
                    {
                        using (var brush = new SolidBrush(Color.FromArgb(10, baseColor)))
                        {
                            var rectX = new Rectangle(RectX.X, RectX.Y, RectX.Width - RectY.Width, RectX.Height);
                            if (Radius > 0)
                            {
                                float radius = Radius * Config.Dpi;
                                using (var path = Helper.RoundPath(RectY, radius, false, true, RB, false))
                                {
                                    g.Fill(brush, path);
                                }
                                if (RB)
                                {
                                    using (var path = Helper.RoundPath(rectX, radius, false, false, false, true))
                                    {
                                        g.Fill(brush, path);
                                    }
                                }
                                else g.Fill(brush, rectX);
                            }
                            else
                            {
                                g.Fill(brush, RectY);
                                g.Fill(brush, rectX);
                            }
                        }
                    }
                    PaintX(g, baseColor);
                    PaintY(g, baseColor);
                }
                else if (showY)
                {
                    if (Back)
                    {
                        using (var brush = new SolidBrush(Color.FromArgb(10, baseColor)))
                        {
                            if (Radius > 0)
                            {
                                float radius = Radius * Config.Dpi;
                                using (var path = Helper.RoundPath(RectY, radius, false, true, RB, false))
                                {
                                    g.Fill(brush, path);
                                }
                            }
                            else g.Fill(brush, RectY);
                        }
                    }
                    PaintY(g, baseColor);
                }
                else if (showX)
                {
                    if (Back)
                    {
                        using (var brush = new SolidBrush(Color.FromArgb(10, baseColor)))
                        {
                            if (RB && Radius > 0)
                            {
                                float radius = Radius * Config.Dpi;
                                using (var path = Helper.RoundPath(new Rectangle(RectX.X, RectX.Y, RectX.Width, RectX.Height), radius, false, false, true, true))
                                {
                                    g.Fill(brush, path);
                                }
                            }
                            else g.Fill(brush, RectX);
                        }
                    }
                    PaintX(g, baseColor);
                }
            }
        }

        SolidBrush BackBrushY(Color color)
        {
            if (AnimationHoverY) return new SolidBrush(Color.FromArgb((int)(10 * AnimationHoverYValue), color));
            else return new SolidBrush(Color.FromArgb(10, color));
        }
        SolidBrush BackBrushX(Color color)
        {
            if (AnimationHoverX) return new SolidBrush(Color.FromArgb((int)(10 * AnimationHoverXValue), color));
            else return new SolidBrush(Color.FromArgb(10, color));
        }
        void PaintY(Canvas g, Color color)
        {
            if (AnimationHoverY)
            {
                using (var brush = new SolidBrush(Color.FromArgb(110 + (int)((141 - 110) * AnimationHoverYValue), color)))
                {
                    var slider = RectSliderY();
                    using (var path = slider.RoundPath(slider.Width))
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
                using (var path = slider.RoundPath(slider.Width))
                {
                    g.Fill(Color.FromArgb(alpha, color), path);
                }
            }
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
            float read = RectX.Width - (showY ? SIZE : 0), width = ((RectX.Width * 1F) / maxX) * read;
            if (width < SIZE) width = SIZE;
            float x = (valueX * 1.0F / (maxX - RectX.Width)) * (read - width), gap = (RectX.Height - SIZE_BAR) / 2F;
            return new RectangleF(RectX.X + x + gap, RectX.Y + gap, width - gap * 2, SIZE_BAR);
        }

        RectangleF RectSliderY()
        {
            float gap = (RectY.Width - SIZE_BAR) / 2, min = SIZE_MINIY + gap * 2, read = RectY.Height - (showX ? SIZE : 0), height = ((RectY.Height * 1F) / maxY) * read;
            if (height < min) height = min;
            else if (height < SIZE) height = SIZE;
            float y = (valueY * 1.0F / (maxY - RectY.Height)) * (read - height);
            return new RectangleF(RectY.X + gap, RectY.Y + y + gap, SIZE_BAR, height - gap * 2);
        }

        RectangleF RectSliderFullX()
        {
            float read = RectX.Width - (showY ? SIZE : 0), width = ((RectX.Width * 1F) / maxX) * read;
            if (width < SIZE) width = SIZE;
            float x = (valueX * 1.0F / (maxX - RectX.Width)) * (read - width);
            return new RectangleF(RectX.X + x, RectX.Y, width, RectX.Height);
        }
        RectangleF RectSliderFullY()
        {
            float read = RectY.Height - (showX ? SIZE : 0), height = ((RectY.Height * 1F) / maxY) * read;
            if (height < SIZE_MINIY) height = SIZE_MINIY;
            else if (height < SIZE) height = SIZE;
            float y = (valueY * 1.0F / (maxY - RectY.Height)) * (read - height);
            return new RectangleF(RectY.X, RectY.Y + y, RectY.Width, height);
        }

        #endregion

        #endregion

        #region 鼠标

        #region 按下

        Point old;
        bool SliderDownX = false;
        float SliderX = 0;
        public bool MouseDownX(Point e)
        {
            if (EnabledX && ShowX && RectX.Contains(e))
            {
                old = e;
                var slider = RectSliderFullX();
                if (!slider.Contains(e))
                {
                    float read = RectX.Width - (showY ? SIZE : 0), x = (e.X - slider.Width / 2F) / read;
                    ValueX = (int)Math.Round(x * maxX);
                    SliderX = RectSliderFullX().X;
                }
                else SliderX = slider.X;
                SliderDownX = true;
                Window.CanHandMessage = false;
                return false;
            }
            return true;
        }

        bool SliderDownY = false;
        float SliderY = 0;
        public bool MouseDownY(Point e)
        {
            if (EnabledY && ShowY && RectY.Contains(e))
            {
                old = e;
                var slider = RectSliderFullY();
                if (!slider.Contains(e))
                {
                    float read = RectY.Height - (showX ? SIZE : 0), y = (e.Y - slider.Height / 2F) / read;
                    ValueY = (int)Math.Round(y * maxY);
                    SliderY = RectSliderFullY().Y;
                }
                else SliderY = slider.Y;
                SliderDownY = true;
                Window.CanHandMessage = false;
                return false;
            }
            return true;
        }

        #endregion

        #region 移动

        public bool MouseMoveX(Point e)
        {
            if (EnabledX && !SliderDownY)
            {
                if (SliderDownX)
                {
                    HoverX = true;
                    var slider = RectSliderFullX();
                    float read = RectX.Width - (showY ? SIZE : 0), x = SliderX + e.X - old.X;
                    ValueX = (int)(x / (read - slider.Width) * (maxX - RectX.Width));
                    return false;
                }
                else if (ShowX && RectX.Contains(e))
                {
                    HoverX = true;
                    return false;
                }
                else HoverX = false;
            }
            return true;
        }
        public bool MouseMoveY(Point e)
        {
            if (EnabledY && !SliderDownX)
            {
                if (SliderDownY)
                {
                    HoverY = true;
                    var slider = RectSliderFullY();
                    float read = RectY.Height - (showX ? SIZE : 0), y = SliderY + e.Y - old.Y;
                    ValueY = (int)(y / (read - slider.Height) * (maxY - RectY.Height));
                    return false;
                }
                else if (ShowY && RectY.Contains(e))
                {
                    HoverY = true;
                    return false;
                }
                else HoverY = false;
            }
            return true;
        }

        #endregion

        public bool MouseUpX()
        {
            if (SliderDownX)
            {
                SliderDownX = false;
                Window.CanHandMessage = true;
                return false;
            }
            return true;
        }
        public bool MouseUpY()
        {
            if (SliderDownY)
            {
                SliderDownY = false;
                Window.CanHandMessage = true;
                return false;
            }
            return true;
        }


        public bool MouseWheelX(int delta)
        {
            if (EnabledX && ShowX && delta != 0)
            {
                int value = ValueX - delta;
                ValueX = value;
                if (ValueX != value) return false;
                return true;
            }
            return false;
        }

        public bool MouseWheelY(int delta)
        {
            if (EnabledY && ShowY && delta != 0)
            {
                int value = ValueY - delta;
                ValueY = value;
                if (ValueY != value) return false;
                return true;
            }
            return false;
        }
        public bool MouseWheel(int delta)
        {
            if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift && EnabledX && ShowX)
            {
                if (delta != 0)
                {
                    ValueX -= delta;
                    return true;
                }
            }
            else if (EnabledY)
            {
                if (ShowY && delta != 0)
                {
                    ValueY -= delta;
                    return true;
                }
            }
            else if (EnabledX)
            {
                if (ShowX && delta != 0)
                {
                    ValueX -= delta;
                    return true;
                }
            }
            return false;
        }

        public void Leave()
        {
            HoverX = HoverY = false;
        }

        #endregion

        #region 融合

        public bool Show
        {
            get
            {
                if (EnabledY) return showY;
                return showX;
            }
        }
        public int Value
        {
            get
            {
                if (EnabledY) return valueY;
                return valueX;
            }
            set
            {
                if (EnabledY) ValueY = value;
                else ValueX = value;
            }
        }

        public int VrValueI
        {
            get
            {
                if (EnabledY) return maxY - RectY.Height;
                return maxX - RectX.Width;
            }
        }

        public int ReadSize
        {
            get
            {
                if (EnabledY) return RectY.Height;
                return RectX.Width;
            }
        }

        public int Max
        {
            get
            {
                if (EnabledY) return maxY;
                return maxX;
            }
        }

        /// <summary>
        /// 设置容器虚拟宽度
        /// </summary>
        public void SetVrSize(int len)
        {
            if (EnabledY) SetVrSize(oldx, len);
            else SetVrSize(len, oldy);
        }

        public bool MouseDown(Point e)
        {
            if (EnabledY) return MouseDownY(e);
            return MouseDownX(e);
        }
        public bool MouseMove(Point e)
        {
            if (EnabledY) return MouseMoveY(e);
            return MouseMoveX(e);
        }
        public bool MouseUp()
        {
            if (EnabledY) return MouseUpY();
            return MouseUpX();
        }

        #endregion

        #region 动画

        ITask? ThreadHoverY = null;
        float AnimationHoverYValue = 0F;
        bool AnimationHoverY = false;

        ITask? ThreadHoverX = null;
        float AnimationHoverXValue = 0F;
        bool AnimationHoverX = false;

        #endregion

        public void Dispose()
        { ThreadHoverY?.Dispose(); ThreadHoverX?.Dispose(); }
    }
}