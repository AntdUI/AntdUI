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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// Slider 滑动输入条
    /// </summary>
    /// <remarks>滑动型输入器，展示当前值和可选范围。</remarks>
    [Description("Slider 滑动输入条")]
    [ToolboxItem(true)]
    [DefaultProperty("Value")]
    [DefaultEvent("ValueChanged")]
    public class Slider : IControl
    {
        #region 属性

        /// <summary>
        /// 固定点
        /// </summary>
        [Description("固定点"), Category("数据"), DefaultValue(null)]
        public int[]? Dots { get; set; }

        Color? fill;
        /// <summary>
        /// 颜色
        /// </summary>
        [Description("颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? Fill
        {
            get { return fill; }
            set
            {
                if (fill == value) return;
                fill = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 悬停颜色
        /// </summary>
        [Description("悬停颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? FillHover { get; set; }

        /// <summary>
        /// 激活颜色
        /// </summary>
        [Description("激活颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? FillActive { get; set; }

        int _minValue = 0;
        /// <summary>
        /// 最小值
        /// </summary>
        [Description("最小值"), Category("数据"), DefaultValue(0)]
        public int MinValue
        {
            get { return _minValue; }
            set
            {
                if (value > _maxValue) return;
                if (_minValue == value) return;
                _minValue = value;
                Invalidate();
            }
        }

        int _maxValue = 100;
        /// <summary>
        /// 最大值
        /// </summary>
        [Description("最大值"), Category("数据"), DefaultValue(100)]
        public int MaxValue
        {
            get { return _maxValue; }
            set
            {
                if (value < _minValue || value < _value) return;
                if (_maxValue == value) return;
                _maxValue = value;
                Invalidate();
            }
        }

        int _value = 0;
        /// <summary>
        /// 当前值
        /// </summary>
        [Description("当前值"), Category("数据"), DefaultValue(0)]
        public int Value
        {
            get { return _value; }
            set
            {
                if (value < _minValue) value = _minValue;
                else if (value > _maxValue) value = _maxValue;
                if (_value == value) return;
                _value = value;
                ValueChanged?.Invoke(this, _value);
                Invalidate();
            }
        }

        public delegate string ValueFormatEventHandler(int value);
        /// <summary>
        /// Value格式化时发生
        /// </summary>
        [Description("Value格式化时发生"), Category("行为")]
        public event ValueFormatEventHandler? ValueFormatChanged;

        TooltipForm? tooltipForm = null;
        string? tooltipText = null;
        void ShowTips(RectangleF dot_rect)
        {
            var text = ValueFormatChanged == null ? Value.ToString() : ValueFormatChanged.Invoke(Value);
            if (text == tooltipText && tooltipForm != null) return;
            tooltipText = text;
            var _rect = RectangleToScreen(ClientRectangle);
            var rect = new Rectangle(_rect.X + (int)dot_rect.X, _rect.Y + (int)dot_rect.Y, (int)dot_rect.Width, (int)dot_rect.Height);
            if (tooltipForm == null)
            {
                tooltipForm = new TooltipForm(this, rect, tooltipText, new TooltipConfig
                {
                    Font = Font,
                    ArrowAlign = (align == TAlignMini.Top || align == TAlignMini.Bottom) ? TAlign.Right : TAlign.Top,
                });
                tooltipForm.Show(this);
            }
            else tooltipForm.SetText(rect, tooltipText);
        }

        void CloseTips()
        {
            tooltipForm?.IClose();
            tooltipForm = null;
        }

        TAlignMini align = TAlignMini.Left;
        /// <summary>
        /// 方向
        /// </summary>
        [Description("方向"), Category("外观"), DefaultValue(TAlignMini.Left)]
        public TAlignMini Align
        {
            get => align;
            set
            {
                if (align == value) return;
                align = value;
                OnSizeChanged(EventArgs.Empty);
                Invalidate();
            }
        }

        /// <summary>
        /// Value 属性值更改时发生
        /// </summary>
        [Description("Value 属性值更改时发生"), Category("行为")]
        public event IntEventHandler? ValueChanged;

        /// <summary>
        /// 是否显示数值
        /// </summary>
        [Description("是否显示数值"), Category("行为"), DefaultValue(false)]
        public bool ShowValue { get; set; } = false;

        int lineSize = 4;
        /// <summary>
        /// 线条粗细
        /// </summary>
        [Description("线条粗细"), Category("外观"), DefaultValue(4)]
        public int LineSize
        {
            get { return lineSize; }
            set
            {
                if (lineSize == value) return;
                lineSize = value;
                Invalidate();
            }
        }

        int dotSize = 14;
        /// <summary>
        /// 点大小
        /// </summary>
        [Description("点大小"), Category("外观"), DefaultValue(14)]
        public int DotSize
        {
            get { return dotSize; }
            set
            {
                if (dotSize == value) return;
                dotSize = value;
                Invalidate();
            }
        }

        int dotSizeActive = 20;
        /// <summary>
        /// 点激活大小
        /// </summary>
        [Description("点激活大小"), Category("外观"), DefaultValue(20)]
        public int DotSizeActive
        {
            get { return dotSizeActive; }
            set
            {
                if (dotSizeActive == value) return;
                dotSizeActive = value;
                Invalidate();
            }
        }

        #endregion

        #region 渲染

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics.High();
            var _rect = ClientRectangle;

            var back = Style.Db.FillQuaternary;
            using (var brush = new SolidBrush(back))
            {
                g.FillRectangle(brush, rect_read);

                if (AnimationHover)
                {
                    using (var brush2 = new SolidBrush(Helper.ToColorN(AnimationHoverValue, back)))
                    {
                        g.FillRectangle(brush2, rect_read);
                    }
                }
                else if (ExtraMouseHover) g.FillRectangle(brush, rect_read);
            }
            float prog = ProgValue(_value, rect_read.Width, rect_read.Height);
            if (_value > _minValue)
            {
                var rect_prog = RectLine(rect_read, prog);
                Color color = fill ?? Style.Db.InfoBorder, color_hover = FillHover ?? Style.Db.InfoHover;
                if (AnimationHover)
                {
                    using (var brush = new SolidBrush(color))
                    {
                        g.FillRectangle(brush, rect_prog);
                    }
                    using (var brush = new SolidBrush(Helper.ToColor(255 * AnimationHoverValue, color_hover)))
                    {
                        g.FillRectangle(brush, rect_prog);
                    }
                }
                else
                {
                    using (var brush = new SolidBrush(ExtraMouseHover ? color_hover : color))
                    {
                        g.FillRectangle(brush, rect_prog);
                    }
                }
            }
            PaintEllipse(g, _rect, rect_read, prog);
            this.PaintBadge(g);
        }

        internal void PaintEllipse(Graphics g, Rectangle _rect, RectangleF rect, float prog)
        {
            var color = fill ?? Style.Db.InfoBorder;
            var color_active = FillActive ?? Style.Db.Primary;
            int DotSize = (int)(dotSize * Config.Dpi), DotSizeActive = (int)(dotSizeActive * Config.Dpi);

            using (var brush = new SolidBrush(Style.Db.BgBase))
            {
                if (Dots != null && Dots.Length > 0)
                {
                    foreach (var it in Dots)
                    {
                        float size = DotSize * 0.9F;
                        float uks = ProgValue(it, rect.Width, rect.Height);
                        var rect_dot = RectDot(_rect, rect, uks, size);
                        g.FillEllipse(brush, rect_dot);
                        PaintEllipse(g, rect_dot, color, 1);
                    }
                }
                var rect_ellipse_rl = RectDot(_rect, rect, prog, DotSize);
                if (ShowValue && _mouseHover) ShowTips(rect_ellipse_rl);
                if (AnimationHover)
                {
                    int size2 = DotSizeActive - DotSize;
                    var size = DotSize + size2 * AnimationHoverValue;
                    var rect_ellipse = RectDot(_rect, rect, prog, size);
                    g.FillEllipse(brush, rect_ellipse);
                    PaintEllipse(g, rect_ellipse, color_active, 2 + 2 * AnimationHoverValue);
                }
                else if (ExtraMouseHover)
                {
                    var rect_ellipse = RectDot(_rect, rect, prog, DotSizeActive);
                    g.FillEllipse(brush, rect_ellipse);
                    PaintEllipse(g, rect_ellipse, color_active, 4);
                }
                else
                {
                    g.FillEllipse(brush, rect_ellipse_rl);
                    PaintEllipse(g, rect_ellipse_rl, color, 2);
                }
            }
        }

        #region 计算区域

        internal float ProgValue(int val, float w, float h)
        {
            int max = _maxValue - _minValue;
            switch (align)
            {
                case TAlignMini.Top:
                case TAlignMini.Bottom:
                    return val >= _maxValue ? h : h * ((val - _minValue) * 1F / max);
                default:
                    return val >= _maxValue ? w : w * ((val - _minValue) * 1F / max);
            }
        }

        internal RectangleF RectLine(RectangleF rect, float prog)
        {
            switch (align)
            {
                case TAlignMini.Right:
                    return new RectangleF(rect_read.X + rect_read.Width - prog, rect_read.Y, prog, rect_read.Height);
                case TAlignMini.Top:
                    return new RectangleF(rect_read.X, rect_read.Y, rect_read.Width, prog);
                case TAlignMini.Bottom:
                    return new RectangleF(rect_read.X, rect_read.Y + rect_read.Height - prog, rect_read.Width, prog);
                default:
                    return new RectangleF(rect_read.X, rect_read.Y, prog, rect_read.Height);
            }
        }
        internal RectangleF RectDot(Rectangle _rect, RectangleF rect, float prog, float size)
        {
            switch (align)
            {
                case TAlignMini.Right:
                    return new RectangleF(rect.X + (rect.Width - prog - (size / 2F)), _rect.Y + (_rect.Height - size) / 2F, size, size);
                case TAlignMini.Top:
                    return new RectangleF(_rect.X + (_rect.Width - size) / 2F, rect.Y + prog - size / 2F, size, size);
                case TAlignMini.Bottom:
                    return new RectangleF(_rect.X + (_rect.Width - size) / 2F, rect.Y + (rect.Height - prog - (size / 2F)), size, size);
                default:
                    return new RectangleF(rect.X + prog - size / 2F, _rect.Y + (_rect.Height - size) / 2F, size, size);
            }
        }
        internal RectangleF RectDotH(Rectangle rect_read, RectangleF _rect, float prog, int DotSize)
        {
            switch (align)
            {
                case TAlignMini.Right:
                    return new RectangleF(rect_read.X + (rect_read.Width - prog - (DotSize / 2)), _rect.Y, DotSize, _rect.Height);
                case TAlignMini.Top:
                    return new RectangleF(_rect.X, rect_read.Y + prog - DotSize / 2, _rect.Width, DotSize);
                case TAlignMini.Bottom:
                    return new RectangleF(_rect.X, rect_read.Y + (rect_read.Height - prog - (DotSize / 2)), _rect.Width, DotSize);
                default:
                    return new RectangleF(rect_read.X + prog - DotSize / 2, _rect.Y, DotSize, _rect.Height);
            }
        }

        #endregion

        internal void PaintEllipse(Graphics g, RectangleF rect_ellipse, Color color, float size)
        {
            using (var brush = new Pen(color, size))
            {
                g.DrawEllipse(brush, rect_ellipse);
            }
        }

        #endregion

        #region 坐标计算

        Rectangle rect_read;
        protected override void OnSizeChanged(EventArgs e)
        {
            var _rect = ClientRectangle;
            float dpi = Config.Dpi;
            int LineSize = (int)(lineSize * dpi), DotSizeActive = (int)(dotSizeActive * dpi), DotSizeActive2 = DotSizeActive * 2;
            if (align == TAlignMini.Top || align == TAlignMini.Bottom) rect_read = new Rectangle(_rect.Left + (_rect.Width - LineSize) / 2, DotSizeActive, LineSize, _rect.Height - DotSizeActive2);
            else rect_read = new Rectangle(DotSizeActive, _rect.Top + (_rect.Height - LineSize) / 2, _rect.Width - DotSizeActive2, LineSize);
            base.OnSizeChanged(e);
        }

        #endregion

        #region 鼠标

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                int max = _maxValue - _minValue;
                Value = _MouseDown(ClientRectangle, e.Location, max);
                mouseFlat = true;
            }
        }

        int _MouseDown(Rectangle _rect, Point loc, int max)
        {
            switch (align)
            {
                case TAlignMini.Right:
                    if (Dots != null && Dots.Length > 0)
                    {
                        int DotSize = (int)(dotSize * Config.Dpi);
                        foreach (var it in Dots)
                        {
                            float uks = ProgValue(it, rect_read.Width, rect_read.Height);
                            var rect_dot = RectDotH(rect_read, _rect, uks, DotSize);
                            if (rect_dot.Contains(loc)) return it;
                        }
                    }
                    float xr = 1F - ((loc.X - rect_read.X) * 1.0F / rect_read.Width);
                    if (xr > 0) return (int)Math.Round(xr * max) + _minValue;
                    else return _minValue;
                case TAlignMini.Top:
                    if (Dots != null && Dots.Length > 0)
                    {
                        int DotSize = (int)(dotSize * Config.Dpi);
                        foreach (var it in Dots)
                        {
                            float uks = ProgValue(it, rect_read.Width, rect_read.Height);
                            var rect_dot = RectDotH(rect_read, _rect, uks, DotSize);
                            if (rect_dot.Contains(loc)) return it;
                        }
                    }
                    float yt = (loc.Y - rect_read.Y) * 1.0F / rect_read.Height;
                    if (yt > 0) return (int)Math.Round(yt * max) + _minValue;
                    else return _minValue;
                case TAlignMini.Bottom:
                    if (Dots != null && Dots.Length > 0)
                    {
                        int DotSize = (int)(dotSize * Config.Dpi);
                        foreach (var it in Dots)
                        {
                            float uks = ProgValue(it, rect_read.Width, rect_read.Height);
                            var rect_dot = RectDotH(rect_read, _rect, uks, DotSize);
                            if (rect_dot.Contains(loc)) return it;
                        }
                    }
                    float yb = 1F - ((loc.Y - rect_read.Y) * 1.0F / rect_read.Height);
                    if (yb > 0) return (int)Math.Round(yb * max) + _minValue;
                    else return _minValue;
                default:
                    if (Dots != null && Dots.Length > 0)
                    {
                        int DotSize = (int)(dotSize * Config.Dpi);
                        foreach (var it in Dots)
                        {
                            float uks = ProgValue(it, rect_read.Width, rect_read.Height);
                            var rect_dot = RectDotH(rect_read, _rect, uks, DotSize);
                            if (rect_dot.Contains(loc)) return it;
                        }
                    }
                    float xl = (loc.X - rect_read.X) * 1.0F / rect_read.Width;
                    if (xl > 0) return (int)Math.Round(xl * max) + _minValue;
                    else return _minValue;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (mouseFlat)
            {
                int max = _maxValue - _minValue;
                switch (align)
                {
                    case TAlignMini.Right:
                        float xr = 1F - ((e.X - rect_read.X) * 1.0F / rect_read.Width);
                        if (xr > 0) Value = (int)Math.Round(xr * max) + _minValue;
                        else Value = _minValue;
                        break;
                    case TAlignMini.Top:
                        float yt = (e.Y - rect_read.Y) * 1.0F / rect_read.Height;
                        if (yt > 0) Value = (int)Math.Round(yt * max) + _minValue;
                        else Value = _minValue;
                        break;
                    case TAlignMini.Bottom:
                        float yb = 1F - ((e.Y - rect_read.Y) * 1.0F / rect_read.Height);
                        if (yb > 0) Value = (int)Math.Round(yb * max) + _minValue;
                        else Value = _minValue;
                        break;
                    default:
                        float xl = (e.X - rect_read.X) * 1.0F / rect_read.Width;
                        if (xl > 0) Value = (int)Math.Round(xl * max) + _minValue;
                        else Value = _minValue;
                        break;
                }
            }
        }

        bool mouseFlat = false;
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            mouseFlat = false;
            Invalidate();
        }

        float AnimationHoverValue = 0F;
        bool AnimationHover = false;
        bool _mouseHover = false;
        bool ExtraMouseHover
        {
            get => _mouseHover;
            set
            {
                if (_mouseHover == value) return;
                _mouseHover = value;
                var enabled = Enabled;
                SetCursor(value && enabled);
                if (Config.Animation)
                {
                    ThreadHover?.Dispose();
                    AnimationHover = true;
                    if (value)
                    {
                        ThreadHover = new ITask(this, () =>
                        {
                            AnimationHoverValue = AnimationHoverValue.Calculate(0.1F);
                            if (AnimationHoverValue > 1) { AnimationHoverValue = 1; return false; }
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
                            AnimationHoverValue = AnimationHoverValue.Calculate(-0.1F);
                            if (AnimationHoverValue <= 0) { AnimationHoverValue = 0F; return false; }
                            Invalidate();
                            return true;
                        }, 10, () =>
                        {
                            AnimationHover = false;
                            Invalidate();
                        });
                    }
                }
                Invalidate();
            }
        }

        #region 动画

        protected override void Dispose(bool disposing)
        {
            ThreadHover?.Dispose();
            base.Dispose(disposing);
        }
        ITask? ThreadHover = null;

        #endregion

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            ExtraMouseHover = true;
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            CloseTips();
            ExtraMouseHover = false;
        }
        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            CloseTips();
            ExtraMouseHover = false;
        }

        #endregion
    }
}