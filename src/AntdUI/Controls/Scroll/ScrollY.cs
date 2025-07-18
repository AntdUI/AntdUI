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
using System.Drawing;
using System.Windows.Forms;

namespace AntdUI
{
    public class ScrollY
    {
        TAMode ColorScheme = TAMode.Auto;

        public ScrollY(FlowLayoutPanel _control)
        {
            SIZE = SystemInformation.VerticalScrollBarWidth;
            Invalidate = () => _control.Invalidate(Rect);
        }
        public ScrollY(ILayeredForm _form)
        {
            Invalidate = () => _form.Print();
            Gap = Back = false;
        }

        Action Invalidate;

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
        public float SetValue(float value)
        {
            if (value < 0) return 0;
            if (value > VrValueI) return VrValueI;
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
        /// 虚拟高度
        /// </summary>
        public float VrValue { get; set; }
        public float VrValueI { get; set; }
        public int Height { get; set; }

        /// <summary>
        /// 设置容器虚拟高度
        /// </summary>
        /// <param name="len">总Y</param>
        /// <param name="height">容器高度</param>
        public void SetVrSize(float len, int height)
        {
            Height = height;
            if (len > 0 && len > height)
            {
                if (ShowX) len += SIZE;
                VrValueI = len - height;
                VrValue = len;
                Show = VrValue > height;
                if (Show)
                {
                    if (val > (len - height)) Value = (len - height);
                }
            }
            else
            {
                VrValue = VrValueI = 0F;
                Show = false;
            }
        }

        /// <summary>
        /// 设置容器虚拟高度
        /// </summary>
        /// <param name="len">总Y</param>
        public void SetVrSize(float len)
        {
            SetVrSize(len, Rect.Height);
        }

        #endregion

        public int SIZE { get; set; } = 20;
        public bool ShowX { get; set; }
        public virtual void SizeChange(Rectangle rect) => Rect = new Rectangle(rect.Right - SIZE, rect.Y, SIZE, rect.Height);

        /// <summary>
        /// 渲染滚动条竖
        /// </summary>
        /// <param name="g"></param>
        public virtual void Paint(Canvas g) => Paint(g, Colour.TextBase.Get("ScrollBar", ColorScheme));
        public virtual void Paint(Canvas g, Color baseColor)
        {
            if (Show)
            {
                if (Back && IsPaintScroll())
                {
                    using (var brush = new SolidBrush(Color.FromArgb(10, baseColor)))
                    {
                        if (ShowX) g.Fill(brush, new Rectangle(Rect.X, Rect.Y, Rect.Width, Rect.Height - SIZE));
                        else g.Fill(brush, Rect);
                    }
                }
                float height = (Rect.Height / VrValue) * Rect.Height;
                if (height < SIZE) height = SIZE;
                if (Gap) height -= 12;
                float y = val == 0 ? 0 : (val / (VrValue - Rect.Height)) * ((ShowX ? (Rect.Height - SIZE) : Rect.Height) - height);
                if (Hover) Slider = new RectangleF(Rect.X + 6, Rect.Y + y, 8, height);
                else Slider = new RectangleF(Rect.X + 7, Rect.Y + y, 6, height);
                if (Gap)
                {
                    if (Slider.Y < 6) Slider.Y = 6;
                    else if (Slider.Y > (ShowX ? (Rect.Height - SIZE) : Rect.Height) - height - 6) Slider.Y = (ShowX ? (Rect.Height - SIZE) : Rect.Height) - height - 6;
                }
                using (var path = Slider.RoundPath(Slider.Width))
                {
                    g.Fill(Color.FromArgb(141, baseColor), path);
                }
            }
        }

        bool IsPaintScroll()
        {
            if (Config.ScrollBarHide) return hover;
            else return true;
        }

        public bool ShowDown = false;
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
        public virtual bool MouseDown(Point e) => MouseDown(e.X, e.Y);
        public virtual bool MouseDown(int x, int y)
        {
            if (Show && Rect.Contains(x, y))
            {
                if (!Slider.Contains(x, y))
                {
                    float Y = (y - Slider.Height / 2F) / Rect.Height;
                    Value = Y * VrValue;
                }
                ShowDown = true;
                return false;
            }
            return true;
        }
        public virtual bool MouseDown(Point e, Action<float> cal) => MouseDown(e.X, e.Y, cal);
        public virtual bool MouseDown(int x, int y, Action<float> cal)
        {
            if (Show && Rect.Contains(x, y))
            {
                if (!Slider.Contains(x, y))
                {
                    float Y = (y - Slider.Height / 2F) / Rect.Height;
                    var old_value = val;
                    Value = Y * VrValue;
                    if (old_value != val) cal(val);
                }
                ShowDown = true;
                return false;
            }
            return true;
        }

        public virtual bool MouseUp(Point e) => MouseUp(e.X, e.Y);
        public virtual bool MouseUp(int x, int y)
        {
            if (ShowDown)
            {
                ShowDown = false;
                return false;
            }
            return true;
        }

        public virtual bool MouseMove(Point e) => MouseMove(e.X, e.Y);
        public virtual bool MouseMove(int x, int y)
        {
            if (ShowDown)
            {
                Hover = true;
                float Y = (y - Slider.Height / 2F) / Rect.Height;
                Value = Y * VrValue;
                return false;
            }
            else if (Show && Rect.Contains(x, y))
            {
                Hover = true;
                return false;
            }
            else Hover = false;
            return true;
        }

        public virtual bool MouseMove(Point e, Action<float> cal) => MouseMove(e.X, e.Y, cal);
        public virtual bool MouseMove(int x, int y, Action<float> cal)
        {
            if (ShowDown)
            {
                Hover = true;
                float Y = (y - Slider.Height / 2F) / Rect.Height;
                var old_value = val;
                Value = Y * VrValue;
                if (old_value != val) cal(val);
                return false;
            }
            else if (Show && Rect.Contains(x, y))
            {
                Hover = true;
                return false;
            }
            else Hover = false;
            return true;
        }

        public bool MouseWheel(int Delta)
        {
            if (Show && Delta != 0)
            {
                int delta = Delta / SystemInformation.MouseWheelScrollDelta * (int)(Config.ScrollStep * Config.Dpi);
                Value -= delta;
                return true;
            }
            return false;
        }

        internal bool MouseWheelCore(int delta)
        {
            if (Show && delta != 0)
            {
                Value -= delta;
                return true;
            }
            return false;
        }

        public void Leave() => Hover = false;
    }
}