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
// GITEE: https://gitee.com/AntdUI/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

using System;
using System.Collections.Generic;
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

        Color? fill;
        /// <summary>
        /// 颜色
        /// </summary>
        [Description("颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? Fill
        {
            get => fill;
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

        Color? trackColor;
        /// <summary>
        /// 滑轨颜色
        /// </summary>
        [Description("滑轨颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? TrackColor
        {
            get => trackColor;
            set
            {
                if (trackColor == value) return;
                trackColor = value;
                Invalidate();
            }
        }

        int _minValue = 0;
        /// <summary>
        /// 最小值
        /// </summary>
        [Description("最小值"), Category("数据"), DefaultValue(0)]
        public int MinValue
        {
            get => _minValue;
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
            get => _maxValue;
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
            get => _value;
            set
            {
                if (value < _minValue) value = _minValue;
                else if (value > _maxValue) value = _maxValue;
                if (_value == value) return;
                _value = value;
                ValueChanged?.Invoke(this, new IntEventArgs(_value));
                Invalidate();
                OnPropertyChanged(nameof(Value));
            }
        }

        /// <summary>
        /// Value格式化时发生
        /// </summary>
        [Description("Value格式化时发生"), Category("行为")]
        public event ValueFormatEventHandler? ValueFormatChanged;

        TooltipForm? tooltipForm;
        string? tooltipText;
        internal void ShowTips(int Value, RectangleF dot_rect)
        {
            var text = ValueFormatChanged == null ? Value.ToString() : ValueFormatChanged.Invoke(this, new IntEventArgs(Value));
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

        internal void CloseTips()
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
                IOnSizeChanged();
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
        public bool ShowValue { get; set; }

        int lineSize = 4;
        /// <summary>
        /// 线条粗细
        /// </summary>
        [Description("线条粗细"), Category("外观"), DefaultValue(4)]
        public int LineSize
        {
            get => lineSize;
            set
            {
                if (lineSize == value) return;
                lineSize = value;
                Invalidate();
            }
        }

        internal int dotSize = 10;
        /// <summary>
        /// 点大小
        /// </summary>
        [Description("点大小"), Category("外观"), DefaultValue(10)]
        public int DotSize
        {
            get => dotSize;
            set
            {
                if (dotSize == value) return;
                dotSize = value;
                Invalidate();
            }
        }

        internal int dotSizeActive = 12;
        /// <summary>
        /// 点激活大小
        /// </summary>
        [Description("点激活大小"), Category("外观"), DefaultValue(12)]
        public int DotSizeActive
        {
            get => dotSizeActive;
            set
            {
                if (dotSizeActive == value) return;
                dotSizeActive = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 是否只能拖拽到刻度上
        /// </summary>
        [Description("是否只能拖拽到刻度上"), Category("数据"), DefaultValue(false)]
        public bool Dots { get; set; }

        SliderMarkItemCollection? marks;
        /// <summary>
        /// 刻度标记
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("刻度标记"), Category("数据"), DefaultValue(null)]
        public SliderMarkItemCollection Marks
        {
            get
            {
                marks ??= new SliderMarkItemCollection(this);
                return marks;
            }
            set => marks = value.BindData(this);
        }

        /// <summary>
        /// 刻度文本间距
        /// </summary>
        [Description("刻度文本间距"), Category("外观"), DefaultValue(4)]
        public int MarkTextGap { get; set; } = 4;

        #endregion

        #region 渲染

        internal Rectangle rect_read;
        protected override void OnPaint(PaintEventArgs e)
        {
            var padding = Padding;
            var _rect = ClientRectangle.PaddingRect(padding);
            if (_rect.Width == 0 || _rect.Height == 0) return;
            int LineSize = (int)(lineSize * Config.Dpi), DotS = (int)((dotSizeActive > dotSize ? dotSizeActive : dotSize) * Config.Dpi), DotS2 = DotS * 2;
            if (align == TAlignMini.Top || align == TAlignMini.Bottom)
            {
                if (padding.Top > DotS || padding.Bottom > DotS)
                {
                    if (padding.Top > DotS && padding.Bottom > DotS) rect_read = new Rectangle(_rect.X + (_rect.Width - LineSize) / 2, _rect.Y, LineSize, _rect.Height);
                    else if (padding.Top > DotS) rect_read = new Rectangle(_rect.X + (_rect.Width - LineSize) / 2, _rect.Y, LineSize, _rect.Height - DotS);
                    else rect_read = new Rectangle(_rect.X + (_rect.Width - LineSize) / 2, _rect.Y + DotS, LineSize, _rect.Height - DotS);
                }
                else rect_read = new Rectangle(_rect.X + (_rect.Width - LineSize) / 2, _rect.Y + DotS, LineSize, _rect.Height - DotS2);
            }
            else
            {
                if (padding.Left > DotS || padding.Right > DotS)
                {
                    if (padding.Left > DotS && padding.Right > DotS) rect_read = new Rectangle(_rect.X, _rect.Y + (_rect.Height - LineSize) / 2, _rect.Width, LineSize);
                    else if (padding.Left > DotS) rect_read = new Rectangle(_rect.X, _rect.Y + (_rect.Height - LineSize) / 2, _rect.Width - DotS, LineSize);
                    else rect_read = new Rectangle(_rect.X + DotS, _rect.Y + (_rect.Height - LineSize) / 2, _rect.Width - DotS, LineSize);
                }
                else rect_read = new Rectangle(_rect.X + DotS, _rect.Y + (_rect.Height - LineSize) / 2, _rect.Width - DotS2, LineSize);
            }

            var enabled = Enabled;
            Color color = enabled ? fill ?? Colour.InfoBorder.Get("Slider", ColorScheme) : Colour.FillTertiary.Get("Slider", ColorScheme), color_dot = enabled ? fill ?? Colour.InfoBorder.Get("Slider", ColorScheme) : Colour.SliderHandleColorDisabled.Get("Slider", ColorScheme), color_hover = FillHover ?? Colour.InfoHover.Get("Slider", ColorScheme), color_active = FillActive ?? Colour.Primary.Get("Slider", ColorScheme);

            var g = e.Graphics.High();
            IPaint(g, _rect, enabled, color, color_dot, color_hover, color_active);
            this.PaintBadge(g);
            base.OnPaint(e);
        }

        internal virtual void IPaint(Canvas g, Rectangle rect, bool enabled, Color color, Color color_dot, Color color_hover, Color color_active)
        {
            float prog = ProgValue(_value);

            #region 线条

            using (var path = rect_read.RoundPath(rect_read.Height / 2))
            {
                using (var brush = new SolidBrush(trackColor ?? Colour.FillQuaternary.Get("Slider", ColorScheme)))
                {
                    g.Fill(brush, path);
                    if (AnimationHover) g.Fill(Helper.ToColorN(AnimationHoverValue, brush.Color), path);
                    else if (ExtraMouseHover) g.Fill(brush, path);
                }

                if (prog > 0)
                {
                    g.SetClip(RectLine(rect_read, prog));
                    if (AnimationHover)
                    {
                        g.Fill(color, path);
                        g.Fill(Helper.ToColor(255 * AnimationHoverValue, color_hover), path);
                    }
                    else g.Fill(ExtraMouseHover ? color_hover : color, path);
                    g.ResetClip();
                }
            }

            #endregion

            using (var brush = new SolidBrush(Colour.BgBase.Get("Slider", ColorScheme)))
            {
                PaintMarksEllipse(g, rect, rect_read, brush, color, LineSize);
                PaintEllipse(g, rect, rect_read, prog, brush, color_dot, color_hover, color_active, LineSize);
            }
        }

        readonly StringFormat s_f = Helper.SF_NoWrap();
        internal RectangleF rectEllipse;
        internal void PaintEllipse(Canvas g, Rectangle rect, RectangleF rect_read, float prog, SolidBrush brush, Color color, Color color_hover, Color color_active, int LineSize)
        {
            int DotSize = (int)(dotSize * Config.Dpi), DotSizeActive = (int)(dotSizeActive * Config.Dpi);
            rectEllipse = RectDot(rect, rect_read, prog, DotSizeActive + LineSize);

            var rect_ellipse_rl = RectDot(rect, rect_read, prog, DotSize + LineSize);
            if (ShowValue && ExtraMouseDotHover) ShowTips(_value, rect_ellipse_rl);

            if (AnimationDotHover)
            {
                float value = ((DotSizeActive - DotSize) * AnimationDotHoverValue);
                using (var brush_shadow = new SolidBrush(color_active.rgba(.2F)))
                {
                    g.FillEllipse(brush_shadow, RectDot(rect, rect_read, prog, DotSizeActive + LineSize + LineSize * 2 * AnimationDotHoverValue));
                }
                using (var brush_dot = new SolidBrush(color_active))
                {
                    g.FillEllipse(brush_dot, RectDot(rect, rect_read, prog, DotSize + LineSize + value));
                }
                g.FillEllipse(brush, RectDot(rect, rect_read, prog, DotSize + value));
            }
            else if (ExtraMouseDotHover)
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
        internal void PaintMarksEllipse(Canvas g, Rectangle rect, Rectangle rect_read, SolidBrush brush, Color color, int LineSize)
        {
            if (marks != null && marks.Count > 0)
            {
                using (var fore = new SolidBrush(Colour.Text.Get("Slider", ColorScheme)))
                {
                    int markTextGap = (int)(MarkTextGap * Config.Dpi);
                    int size2 = LineSize, size = size2 * 2;
                    foreach (var it in marks)
                    {
                        float uks = ProgValue(it.Value);
                        if (!string.IsNullOrWhiteSpace(it.Text))
                        {
                            if (it.Fore.HasValue)
                            {
                                using (var fore2 = new SolidBrush(it.Fore.Value))
                                {
                                    g.String(it.Text, Font, fore2, RectDotText(rect, rect_read, (int)uks, markTextGap, g.MeasureString(it.Text, Font)), s_f);
                                }
                            }
                            else g.String(it.Text, Font, fore, RectDotText(rect, rect_read, (int)uks, markTextGap, g.MeasureString(it.Text, Font)), s_f);
                        }
                        using (var brush_dot = new SolidBrush(color))
                        {
                            g.FillEllipse(brush_dot, RectDot(rect, rect_read, uks, size));
                        }
                        g.FillEllipse(brush, RectDot(rect, rect_read, uks, size2));
                    }
                }
            }
        }

        #region 计算区域

        internal float ProgValue(int val)
        {
            int max = _maxValue - _minValue;
            switch (align)
            {
                case TAlignMini.Top:
                case TAlignMini.Bottom:
                    float h = rect_read.Height;
                    return val >= _maxValue ? h : h * ((val - _minValue) * 1F / max);
                default:
                    float w = rect_read.Width;
                    return val >= _maxValue ? w : w * ((val - _minValue) * 1F / max);
            }
        }

        internal RectangleF RectLine(RectangleF rect, float prog)
        {
            switch (align)
            {
                case TAlignMini.Right:
                    return new RectangleF(rect.X + rect.Width - prog, rect.Y, prog, rect.Height);
                case TAlignMini.Top:
                    return new RectangleF(rect.X, rect.Y, rect.Width, prog);
                case TAlignMini.Bottom:
                    return new RectangleF(rect.X, rect.Y + rect.Height - prog, rect.Width, prog);
                default:
                    return new RectangleF(rect.X, rect.Y, prog, rect.Height);
            }
        }
        internal RectangleF RectDot(Rectangle rect, RectangleF rect_read, float prog, float size)
        {
            switch (align)
            {
                case TAlignMini.Right:
                    return new RectangleF(rect_read.X + (rect_read.Width - prog - (size / 2F)), rect.Y + (rect.Height - size) / 2F, size, size);
                case TAlignMini.Top:
                    return new RectangleF(rect.X + (rect.Width - size) / 2F, rect_read.Y + prog - size / 2F, size, size);
                case TAlignMini.Bottom:
                    return new RectangleF(rect.X + (rect.Width - size) / 2F, rect_read.Y + (rect_read.Height - prog - (size / 2F)), size, size);
                default:
                    return new RectangleF(rect_read.X + prog - size / 2F, rect.Y + (rect.Height - size) / 2F, size, size);
            }
        }
        internal Rectangle RectDotText(Rectangle rect, Rectangle rect_read, int prog, int gap, Size size)
        {
            switch (align)
            {
                case TAlignMini.Right:
                    return new Rectangle(rect_read.X + (rect_read.Width - prog - size.Width / 2), rect_read.Bottom + rect_read.Height + gap, size.Width, size.Height);
                case TAlignMini.Top:
                    return new Rectangle(rect_read.Right + rect_read.Width + gap, rect_read.Y + prog - size.Height / 2, size.Width, size.Height);
                case TAlignMini.Bottom:
                    return new Rectangle(rect_read.Right + rect_read.Width + gap, rect_read.Y + (rect_read.Height - prog - size.Height / 2), size.Width, size.Height);
                default:
                    return new Rectangle(rect_read.X + prog - size.Width / 2, rect_read.Bottom + rect_read.Height + gap, size.Width, size.Height);
            }
        }
        internal RectangleF RectDotH(Rectangle rect, Rectangle rect_read, float prog, int DotSize)
        {
            switch (align)
            {
                case TAlignMini.Right:
                    return new RectangleF(rect_read.X + (rect_read.Width - prog - DotSize / 2F), rect.Y, DotSize, rect.Height);
                case TAlignMini.Top:
                    return new RectangleF(rect.X, rect_read.Y + prog - DotSize / 2F, rect.Width, DotSize);
                case TAlignMini.Bottom:
                    return new RectangleF(rect.X, rect_read.Y + (rect_read.Height - prog - DotSize / 2F), rect.Width, DotSize);
                default:
                    return new RectangleF(rect_read.X + prog - DotSize / 2F, rect.Y, DotSize, rect.Height);
            }
        }

        #endregion

        #endregion

        #region 鼠标

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                Value = FindIndex(e.X, e.Y, true);
                mouseFlat = true;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (mouseFlat)
            {
                ExtraMouseDotHover = true;
                Value = FindIndex(e.X, e.Y, false);
            }
            else ExtraMouseDotHover = rectEllipse.Contains(e.X, e.Y);
        }

        internal int FindIndex(int x, int y, bool mark)
        {
            int max = _maxValue - _minValue;
            if (marks != null && marks.Count > 0)
            {
                if (Dots)
                {
                    var rect = ClientRectangle;
                    int DotSize = (int)(dotSize * Config.Dpi);
                    var mark_list = new List<float>(marks.Count);
                    int i = 0;
                    switch (align)
                    {
                        case TAlignMini.Right:
                            foreach (var it in marks) mark_list.Add(rect_read.X + (rect_read.Width - (it.Value >= _maxValue ? rect_read.Width : rect_read.Width * ((it.Value - _minValue) * 1F / max))));
                            i = FindNumber(x, mark_list);
                            break;
                        case TAlignMini.Top:
                            foreach (var it in marks) mark_list.Add(rect_read.Y + (it.Value >= _maxValue ? rect_read.Height : rect_read.Height * ((it.Value - _minValue) * 1F / max)));
                            i = FindNumber(y, mark_list);
                            break;
                        case TAlignMini.Bottom:
                            foreach (var it in marks) mark_list.Add(rect_read.Y + (rect_read.Height - (it.Value >= _maxValue ? rect_read.Height : rect_read.Height * ((it.Value - _minValue) * 1F / max))));
                            i = FindNumber(y, mark_list);
                            break;
                        default:
                            foreach (var it in marks) mark_list.Add(rect_read.X + (it.Value >= _maxValue ? rect_read.Width : rect_read.Width * ((it.Value - _minValue) * 1F / max)));
                            i = FindNumber(x, mark_list);
                            break;
                    }
                    return marks[i].Value;
                }
                if (mark)
                {
                    var rect = ClientRectangle;
                    int DotSize = (int)(dotSize * Config.Dpi);
                    foreach (var it in marks)
                    {
                        float uks = ProgValue(it.Value);
                        var rect_dot = RectDotH(rect, rect_read, uks, DotSize);
                        if (rect_dot.Contains(x, y)) return it.Value;
                    }
                }
            }
            switch (align)
            {
                case TAlignMini.Right:
                    float xr = 1F - ((x - rect_read.X) * 1.0F / rect_read.Width);
                    if (xr > 0) return (int)Math.Round(xr * max) + _minValue;
                    else return _minValue;
                case TAlignMini.Top:
                    float yt = (y - rect_read.Y) * 1.0F / rect_read.Height;
                    if (yt > 0) return (int)Math.Round(yt * max) + _minValue;
                    else return _minValue;
                case TAlignMini.Bottom:
                    float yb = 1F - ((y - rect_read.Y) * 1.0F / rect_read.Height);
                    if (yb > 0) return (int)Math.Round(yb * max) + _minValue;
                    else return _minValue;
                default:
                    float xl = (x - rect_read.X) * 1.0F / rect_read.Width;
                    if (xl > 0) return (int)Math.Round(xl * max) + _minValue;
                    else return _minValue;
            }
        }

        internal int FindNumber(int target, List<float> array)
        {
            int Index = 0;
            float Difference = int.MaxValue;
            for (int i = 0; i < array.Count; i++)
            {
                float difference = Math.Abs(target - array[i]);
                if (difference < Difference)
                {
                    Difference = difference;
                    Index = i;
                }
            }
            return Index;
        }

        bool mouseFlat = false;
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            mouseFlat = false;
            Invalidate();
        }

        internal float AnimationHoverValue = 0F;
        internal bool AnimationHover = false;
        bool _mouseHover = false;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal bool ExtraMouseHover
        {
            get => _mouseHover;
            set
            {
                if (_mouseHover == value) return;
                _mouseHover = value;
                var enabled = Enabled;
                SetCursor(value && enabled);
                if (Config.HasAnimation(nameof(Slider)))
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

        internal float AnimationDotHoverValue = 0F;
        internal bool AnimationDotHover = false;
        bool _mouseDotHover = false;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal bool ExtraMouseDotHover
        {
            get => _mouseDotHover;
            set
            {
                if (_mouseDotHover == value) return;
                _mouseDotHover = value;
                if (!value) CloseTips();
                if (Config.HasAnimation(nameof(Slider)))
                {
                    ThreadHover?.Dispose();
                    ThreadHover = null;
                    ThreadDotHover?.Dispose();
                    AnimationDotHover = true;
                    if (value)
                    {
                        ThreadDotHover = new ITask(this, () =>
                        {
                            AnimationDotHoverValue = AnimationDotHoverValue.Calculate(0.1F);
                            if (AnimationDotHoverValue > 1) { AnimationDotHoverValue = 1; return false; }
                            Invalidate();
                            return true;
                        }, 10, () =>
                        {
                            AnimationDotHover = false;
                            Invalidate();
                        });
                    }
                    else
                    {
                        ThreadDotHover = new ITask(this, () =>
                        {
                            AnimationDotHoverValue = AnimationDotHoverValue.Calculate(-0.1F);
                            if (AnimationDotHoverValue <= 0) { AnimationDotHoverValue = 0F; return false; }
                            Invalidate();
                            return true;
                        }, 10, () =>
                        {
                            AnimationDotHover = false;
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
            ThreadHover?.Dispose();
            ThreadDotHover?.Dispose();
            base.Dispose(disposing);
        }
        internal ITask? ThreadHover;
        ITask? ThreadDotHover;

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
            ExtraMouseHover = ExtraMouseDotHover = false;
        }
        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            CloseTips();
            ExtraMouseHover = ExtraMouseDotHover = false;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 设置最小最大值
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        public void SetMinMax(int min, int max)
        {
            if (min > max) return;
            _minValue = min;
            _maxValue = max;
            if (_value < min) _value = min;
            else if (_value > max) _value = max;
            Invalidate();
        }

        #endregion
    }

    public class SliderMarkItemCollection : iCollection<SliderMarkItem>
    {
        public SliderMarkItemCollection(IControl it)
        {
            BindData(it);
        }

        internal SliderMarkItemCollection BindData(IControl it)
        {
            action = render =>
            {
                it.Invalidate();
            };
            return this;
        }
    }

    public class SliderMarkItem
    {
        int _value = 0;
        /// <summary>
        /// 文本
        /// </summary>
        [Description("值"), Category("外观"), DefaultValue(0)]
        public int Value
        {
            get => _value;
            set
            {
                if (_value == value) return;
                _value = value;
                Invalidates();
            }
        }


        Color? fore;
        /// <summary>
        /// 文本颜色
        /// </summary>
        [Description("文本颜色"), Category("外观"), DefaultValue(null)]
        public Color? Fore
        {
            get => fore;
            set
            {
                if (fore == value) return;
                fore = value;
                Invalidates();
            }
        }

        string? text;
        /// <summary>
        /// 文本
        /// </summary>
        [Description("文本"), Category("外观"), DefaultValue(null)]
        public string? Text
        {
            get => text;
            set
            {
                if (text == value) return;
                text = value;
                Invalidates();
            }
        }

        /// <summary>
        /// 用户定义数据
        /// </summary>
        [Description("用户定义数据"), Category("数据"), DefaultValue(null)]
        public object? Tag { get; set; }

        internal Slider? PARENT { get; set; }

        void Invalidates()
        {
            if (PARENT == null) return;
            PARENT.Invalidate();
        }

        public override string ToString() => _value + " " + text;
    }
}