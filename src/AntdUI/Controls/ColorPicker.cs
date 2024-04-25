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
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// ColorPicker 颜色选择器
    /// </summary>
    /// <remarks>提供颜色选取的组件。</remarks>
    [Description("ColorPicker 颜色选择器")]
    [ToolboxItem(true)]
    [DefaultProperty("Value")]
    [DefaultEvent("ValueChanged")]
    public class ColorPicker : IControl, SubLayeredForm
    {
        public ColorPicker()
        {
            base.BackColor = Color.Transparent;
        }

        #region 属性

        #region 系统

        /// <summary>
        /// 背景颜色
        /// </summary>
        [Description("背景颜色"), Category("外观"), DefaultValue(null)]
        public new Color? BackColor
        {
            get => back;
            set
            {
                if (back == value) return;
                back = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 文字颜色
        /// </summary>
        [Description("文字颜色"), Category("外观"), DefaultValue(null)]
        public new Color? ForeColor
        {
            get => fore;
            set
            {
                if (fore == value) fore = value;
                fore = value;
                Invalidate();
            }
        }

        #endregion

        Color? fore;
        /// <summary>
        /// 文字颜色
        /// </summary>
        [Description("文字颜色"), Category("外观"), DefaultValue(null)]
        [Obsolete("使用 ForeColor 属性替代"), Browsable(false)]
        public Color? Fore
        {
            get => fore;
            set
            {
                if (fore == value) return;
                fore = value;
                Invalidate();
            }
        }

        #region 背景

        internal Color? back;
        /// <summary>
        /// 背景颜色
        /// </summary>
        [Description("背景颜色"), Category("外观"), DefaultValue(null)]
        [Obsolete("使用 BackColor 属性替代"), Browsable(false)]
        public Color? Back
        {
            get => back;
            set
            {
                if (back == value) return;
                back = value;
                Invalidate();
            }
        }

        #endregion

        #region 边框

        internal float borderWidth = 1F;
        /// <summary>
        /// 边框宽度
        /// </summary>
        [Description("边框宽度"), Category("边框"), DefaultValue(1F)]
        public float BorderWidth
        {
            get => borderWidth;
            set
            {
                if (borderWidth == value) return;
                borderWidth = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 边框颜色
        /// </summary>
        internal Color? borderColor;
        [Description("边框颜色"), Category("边框"), DefaultValue(null)]
        public Color? BorderColor
        {
            get => borderColor;
            set
            {
                if (borderColor == value) return;
                borderColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 悬停边框颜色
        /// </summary>
        [Description("悬停边框颜色"), Category("边框"), DefaultValue(null)]
        public Color? BorderHover { get; set; }

        /// <summary>
        /// 激活边框颜色
        /// </summary>
        [Description("激活边框颜色"), Category("边框"), DefaultValue(null)]
        public Color? BorderActive { get; set; }

        #endregion

        /// <summary>
        /// 边距，用于激活动画
        /// </summary>
        [Description("边距，用于激活动画"), Category("外观"), DefaultValue(4)]
        public int Margins { get; set; } = 4;

        internal int radius = 6;
        /// <summary>
        /// 圆角
        /// </summary>
        [Description("圆角"), Category("外观"), DefaultValue(6)]
        public int Radius
        {
            get => radius;
            set
            {
                if (radius == value) return;
                radius = value;
                Invalidate();
            }
        }

        internal bool round = false;
        /// <summary>
        /// 圆角样式
        /// </summary>
        [Description("圆角样式"), Category("外观"), DefaultValue(false)]
        public bool Round
        {
            get => round;
            set
            {
                if (round == value) return;
                round = value;
                Invalidate();
            }
        }

        Color _value = Style.Db.Primary;
        [Description("颜色的值"), Category("值"), DefaultValue(typeof(Color), "Transparent")]
        public Color Value
        {
            get => _value;
            set
            {
                if (value == _value) return;
                _value = value;
                ValueChanged?.Invoke(this, value);
                if (BeforeAutoSize()) Invalidate();
            }
        }

        bool showText = false;
        [Description("显示Hex文字"), Category("值"), DefaultValue(false)]
        public bool ShowText
        {
            get => showText;
            set
            {
                if (showText == value) return;
                showText = value;
                if (BeforeAutoSize()) Invalidate();
            }
        }

        bool joinLeft = false;
        /// <summary>
        /// 连接左边
        /// </summary>
        [Description("连接左边"), Category("外观"), DefaultValue(false)]
        public bool JoinLeft
        {
            get => joinLeft;
            set
            {
                if (joinLeft == value) return;
                joinLeft = value;
                if (BeforeAutoSize()) Invalidate();
            }
        }

        bool joinRight = false;
        /// <summary>
        /// 连接右边
        /// </summary>
        [Description("连接右边"), Category("外观"), DefaultValue(false)]
        public bool JoinRight
        {
            get => joinRight;
            set
            {
                if (joinRight == value) return;
                joinRight = value;
                if (BeforeAutoSize()) Invalidate();
            }
        }

        #endregion

        #region 事件

        /// <summary>
        /// Value 属性值更改时发生
        /// </summary>
        [Description("Value 属性值更改时发生"), Category("行为")]
        public event ColorEventHandler? ValueChanged = null;

        #endregion

        #region 渲染

        internal StringFormat stringLeft = Helper.SF_NoWrap(lr: StringAlignment.Near);
        protected override void OnPaint(PaintEventArgs e)
        {
            RectangleF rect = ClientRectangle.PaddingRect(Padding);
            var g = e.Graphics.High();
            var rect_read = ReadRectangle;
            float _radius = round ? rect_read.Height : radius * Config.Dpi;

            bool enabled = Enabled;

            using (var path = Path(rect_read, _radius))
            {
                Color _fore = fore.HasValue ? fore.Value : Style.Db.Text, _back = back.HasValue ? back.Value : Style.Db.BgContainer,
                    _border = borderColor.HasValue ? borderColor.Value : Style.Db.BorderColor,
                    _borderHover = BorderHover.HasValue ? BorderHover.Value : Style.Db.PrimaryHover,
                _borderActive = BorderActive.HasValue ? BorderActive.Value : Style.Db.Primary;
                PaintClick(g, path, rect, _borderActive, radius);
                float size_color = rect_read.Height * 0.75F;
                if (enabled)
                {
                    using (var brush = new SolidBrush(_back))
                    {
                        g.FillPath(brush, path);
                    }
                    if (borderWidth > 0)
                    {
                        if (AnimationHover)
                        {
                            using (var brush = new Pen(_border, borderWidth))
                            {
                                g.DrawPath(brush, path);
                            }
                            using (var brush = new Pen(Color.FromArgb(AnimationHoverValue, _borderHover), borderWidth))
                            {
                                g.DrawPath(brush, path);
                            }
                        }
                        else if (ExtraMouseDown)
                        {
                            using (var brush = new Pen(_borderActive, borderWidth))
                            {
                                g.DrawPath(brush, path);
                            }
                        }
                        else if (ExtraMouseHover)
                        {
                            using (var brush = new Pen(_borderHover, borderWidth))
                            {
                                g.DrawPath(brush, path);
                            }
                        }
                        else
                        {
                            using (var brush = new Pen(_border, borderWidth))
                            {
                                g.DrawPath(brush, path);
                            }
                        }
                    }
                }
                else
                {
                    using (var brush = new SolidBrush(Style.Db.FillTertiary))
                    {
                        g.FillPath(brush, path);
                    }
                    if (borderWidth > 0)
                    {
                        using (var brush = new Pen(_border, borderWidth))
                        {
                            g.DrawPath(brush, path);
                        }
                    }
                }
                if (showText)
                {
                    float gap = (rect_read.Height - size_color) / 2F;
                    var rect_color = new RectangleF(rect_read.X + gap, rect_read.Y + gap, size_color, size_color);
                    using (var path_color = rect_color.RoundPath(_radius * 0.75F))
                    {
                        using (var brush = new SolidBrush(_value))
                        {
                            g.FillPath(brush, path_color);
                        }
                    }
                    using (var brush = new SolidBrush(_fore))
                    {
                        var wi = gap * 2 + size_color;
                        g.DrawString("#" + _value.ToHex(), Font, brush, new RectangleF(rect_read.X + wi, rect_read.Y, rect_read.Width - wi, rect_read.Height), stringLeft);
                    }
                }
                else
                {
                    float size_colorw = rect_read.Width * 0.75F;
                    var rect_color = new RectangleF(rect_read.X + (rect_read.Width - size_colorw) / 2, rect_read.Y + (rect_read.Height - size_color) / 2, size_colorw, size_color);
                    using (var path_color = rect_color.RoundPath(_radius * 0.75F))
                    {
                        using (var brush = new SolidBrush(_value))
                        {
                            g.FillPath(brush, path_color);
                        }
                    }
                }
            }
            this.PaintBadge(g);
            base.OnPaint(e);
        }

        #region 渲染帮助

        internal GraphicsPath Path(RectangleF rect_read, float _radius)
        {
            if (JoinLeft && JoinRight) return rect_read.RoundPath(0);
            else if (JoinRight) return rect_read.RoundPath(_radius, true, false, false, true);
            else if (JoinLeft) return rect_read.RoundPath(_radius, false, true, true, false);
            return rect_read.RoundPath(_radius);
        }

        #region 点击动画

        internal void PaintClick(Graphics g, GraphicsPath path, RectangleF rect, Color color, float radius)
        {
            if (AnimationFocus)
            {
                if (AnimationFocusValue > 0)
                {
                    using (var brush = new SolidBrush(Color.FromArgb(AnimationFocusValue, color)))
                    {
                        using (var path_click = rect.RoundPath(radius, round))
                        {
                            path_click.AddPath(path, false);
                            g.FillPath(brush, path_click);
                        }
                    }
                }
            }
            else if (ExtraMouseDown && Margins > 0)
            {
                using (var brush = new SolidBrush(Color.FromArgb(30, color)))
                {
                    using (var path_click = rect.RoundPath(radius, round))
                    {
                        path_click.AddPath(path, false);
                        g.FillPath(brush, path_click);
                    }
                }
            }
        }

        #endregion

        #endregion

        public override Rectangle ReadRectangle
        {
            get => ClientRectangle.PaddingRect(Padding).ReadRect(Margins + (int)(borderWidth * Config.Dpi / 2F), JoinLeft, JoinRight);
        }

        public override GraphicsPath RenderRegion
        {
            get
            {
                var rect_read = ReadRectangle;
                float _radius = round ? rect_read.Height : radius;
                return Path(rect_read, _radius);
            }
        }

        #endregion

        #region 事件/焦点

        bool AnimationFocus = false;
        int AnimationFocusValue = 0;

        #endregion

        #region 鼠标

        internal bool _mouseDown = false;
        internal bool ExtraMouseDown
        {
            get => _mouseDown;
            set
            {
                if (_mouseDown == value) return;
                _mouseDown = value;
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
                if (Enabled)
                {
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

        protected override void OnMouseEnter(EventArgs e)
        {
            Cursor = Cursors.Hand;
            base.OnMouseEnter(e);
            ExtraMouseHover = true;
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            Cursor = DefaultCursor;
            base.OnMouseLeave(e);
            ExtraMouseHover = false;
        }
        protected override void OnLeave(EventArgs e)
        {
            Cursor = DefaultCursor;
            base.OnLeave(e);
            ExtraMouseHover = false;
        }


        ILayeredForm? subForm = null;
        public ILayeredForm? SubForm() { return subForm; }
        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ExtraMouseDown = true;
                if (subForm == null)
                {
                    subForm = new LayeredFormColorPicker(this, ReadRectangle, color =>
                    {
                        Value = color;
                    });
                    subForm.Disposed += (a, b) =>
                    {
                        ExtraMouseDown = false;
                        subForm = null;
                    };
                    subForm.Show();
                }
            }
            base.OnMouseClick(e);
        }

        #endregion

        #region 释放/动画

        protected override void Dispose(bool disposing)
        {
            ThreadFocus?.Dispose();
            ThreadHover?.Dispose();
            base.Dispose(disposing);
        }
        ITask? ThreadHover = null;
        ITask? ThreadFocus = null;

        #endregion

        #region 自动大小

        /// <summary>
        /// 自动大小
        /// </summary>
        [Browsable(true)]
        [Description("自动大小"), Category("外观"), DefaultValue(false)]
        public override bool AutoSize
        {
            get => base.AutoSize;
            set
            {
                if (base.AutoSize == value) return;
                base.AutoSize = value;
                if (value)
                {
                    if (autoSize == TAutoSize.None) autoSize = TAutoSize.Auto;
                }
                else autoSize = TAutoSize.None;
                BeforeAutoSize();
            }
        }

        TAutoSize autoSize = TAutoSize.None;
        /// <summary>
        /// 自动大小模式
        /// </summary>
        [Description("自动大小模式"), Category("外观"), DefaultValue(TAutoSize.None)]
        public TAutoSize AutoSizeMode
        {
            get => autoSize;
            set
            {
                if (autoSize == value) return;
                autoSize = value;
                base.AutoSize = autoSize != TAutoSize.None;
                BeforeAutoSize();
            }
        }

        protected override void OnFontChanged(EventArgs e)
        {
            BeforeAutoSize();
            base.OnFontChanged(e);
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            return PSize;
        }

        internal Size PSize
        {
            get
            {
                return Helper.GDI(g =>
                {
                    var font_size = g.MeasureString(_value.A == 255 ? "#DDDCCC" : "#DDDDCCCC", Font);
                    float gap = 20 * Config.Dpi;
                    if (showText)
                    {
                        int s = (int)Math.Ceiling(font_size.Height + Margins + gap);
                        return new Size(s + (int)font_size.Width, s);
                    }
                    else
                    {
                        int s = (int)Math.Ceiling(font_size.Height + Margins + gap);
                        return new Size(s, s);
                    }
                });
            }
        }

        protected override void OnResize(EventArgs e)
        {
            BeforeAutoSize();
            base.OnResize(e);
        }

        internal bool BeforeAutoSize()
        {
            if (autoSize == TAutoSize.None) return true;
            if (InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    BeforeAutoSize();
                }));
                return false;
            }
            switch (autoSize)
            {
                case TAutoSize.Width:
                    Width = PSize.Width;
                    break;
                case TAutoSize.Height:
                    Height = PSize.Height;
                    break;
                case TAutoSize.Auto:
                default:
                    Size = PSize;
                    break;
            }
            Invalidate();
            return false;
        }

        #endregion
    }
}