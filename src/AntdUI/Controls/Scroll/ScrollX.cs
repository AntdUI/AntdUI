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

namespace AntdUI
{
    public class ScrollX
    {
        IControl? control;
        public ScrollX(IControl _control)
        {
            Invalidate = () =>
            {
                Change?.Invoke();
                _control.Invalidate();
            };
            control = _control;
        }
        public ScrollX(ILayeredForm _form)
        {
            Invalidate = () =>
            {
                Change?.Invoke();
                _form.Print();
            };
            Gap = Back = false;
        }

        Action Invalidate;

        internal Action? Change;

        public bool Back = true;
        public bool Gap = true;

        #region 属性

        bool show = false;
        public bool Show
        {
            get => show;
            set
            {
                if (show == value) return;
                show = value;
                if (!value) val = 0;
            }
        }
        public Rectangle Rect;
        public RectangleF Slider;

        internal float val = 0;
        internal float SetValue(float value)
        {
            if (value < 0) return 0;
            if (value > VrWidthI) return VrWidthI;
            return value;
        }
        /// <summary>
        /// 滚动条进度
        /// </summary>
        public float Value
        {
            get => val;
            set
            {
                var _val = SetValue(value);
                if (val == _val) return;
                val = _val;
                Invalidate();
            }
        }

        /// <summary>
        /// 虚拟宽度
        /// </summary>
        public float VrWidth { get; set; } = 0F;
        public float VrWidthI { get; set; } = 0F;

        /// <summary>
        /// 设置容器虚拟宽度
        /// </summary>
        /// <param name="len">总X</param>
        /// <param name="width">容器宽度</param>
        public void SetVrSize(float len, int width)
        {
            if (len > 0 && len > width)
            {
                VrWidthI = len - width;
                VrWidth = len;
                Show = VrWidth > width;
                if (Show)
                {
                    if (val > (len - width)) Value = (len - width);
                }
            }
            else
            {
                VrWidth = VrWidthI = 0F;
                Show = false;
            }
        }


        #endregion

        public virtual void SizeChange(Rectangle rect)
        {
            Rect = new Rectangle(rect.X, rect.Bottom - 20, rect.Width, 20);
        }

        /// <summary>
        /// 渲染滚动条竖
        /// </summary>
        /// <param name="g"></param>
        public virtual void Paint(Graphics g)
        {
            if (Show)
            {
                if (Back)
                {
                    using (var brush = new SolidBrush(Color.FromArgb(10, Style.Db.TextBase)))
                    {
                        g.FillRectangle(brush, Rect);
                    }
                }
                float width = (Rect.Width / VrWidth) * Rect.Width;
                if (width < 20) width = 20;
                if (Gap) width -= 12;
                float x = val == 0 ? 0 : (val / (VrWidth - Rect.Width)) * (Rect.Width - width);
                if (Hover) Slider = new RectangleF(Rect.X + x, Rect.Y + 6, width, 8);
                else Slider = new RectangleF(Rect.X + x, Rect.Y + 7, width, 6);
                if (Gap)
                {
                    if (Slider.X < 6) Slider.X = 6;
                    else if (Slider.X > Rect.Width - width - 6) Slider.X = Rect.Width - width - 6;
                }
                using (var brush = new SolidBrush(Color.FromArgb(141, Style.Db.TextBase)))
                {
                    using (var path = Slider.RoundPath(Slider.Height))
                    {
                        g.FillPath(brush, path);
                    }
                }
            }
        }

        bool ShowDown = false;
        bool hover = false;
        bool Hover
        {
            get => hover;
            set
            {
                if (hover == value) return;
                hover = value;
                Invalidate();
            }
        }
        public virtual bool MouseDown(Point e)
        {
            if (Show && Rect.Contains(e))
            {
                if (!Slider.Contains(e))
                {
                    float x = (e.X - Slider.Width / 2F) / Rect.Width;
                    Value = x * VrWidth;
                }
                ShowDown = true;
                return false;
            }
            return true;
        }

        public virtual bool MouseUp(Point e)
        {
            ShowDown = false;
            return true;
        }

        public virtual bool MouseMove(Point e)
        {
            if (ShowDown)
            {
                Hover = true;
                float x = (e.X - Slider.Width / 2F) / Rect.Width;
                Value = x * VrWidth;
                return false;
            }
            else if (Show && Rect.Contains(e))
            {
                Hover = true;
                control?.SetCursor(false);
                return false;
            }
            else Hover = false;
            return true;
        }

        public bool MouseWheel(int delta)
        {
            if (Show && delta != 0)
            {
                Value -= delta;
                return true;
            }
            return false;
        }

        public void Leave()
        {
            Hover = false;
        }
    }
}