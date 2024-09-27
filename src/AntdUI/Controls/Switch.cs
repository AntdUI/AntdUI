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
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// Switch 开关
    /// </summary>
    /// <remarks>开关选择器。</remarks>
    [Description("Switch 开关")]
    [ToolboxItem(true)]
    [DefaultProperty("Checked")]
    [DefaultEvent("CheckedChanged")]
    public class Switch : IControl
    {
        public Switch()
        {
            Cursor = Cursors.Hand;
        }

        #region 属性

        Color? fore;
        /// <summary>
        /// 文字颜色
        /// </summary>
        [Description("文字颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
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

        bool AnimationCheck = false;
        float AnimationCheckValue = 0;

        bool _checked = false;
        /// <summary>
        /// 选中状态
        /// </summary>
        [Description("选中状态"), Category("数据"), DefaultValue(false)]
        public bool Checked
        {
            get => _checked;
            set
            {
                if (_checked == value) return;
                _checked = value;
                CheckedChanged?.Invoke(this, new BoolEventArgs(value));
                ThreadCheck?.Dispose();
                if (IsHandleCreated && Config.Animation)
                {
                    AnimationCheck = true;
                    if (value)
                    {
                        ThreadCheck = new ITask(this, () =>
                        {
                            AnimationCheckValue = AnimationCheckValue.Calculate(0.1F);
                            if (AnimationCheckValue > 1) { AnimationCheckValue = 1F; return false; }
                            Invalidate();
                            return true;
                        }, 10, () =>
                        {
                            AnimationCheck = false;
                            Invalidate();
                        });
                    }
                    else
                    {
                        ThreadCheck = new ITask(this, () =>
                        {
                            AnimationCheckValue = AnimationCheckValue.Calculate(-0.1F);
                            if (AnimationCheckValue <= 0) { AnimationCheckValue = 0F; return false; }
                            Invalidate();
                            return true;
                        }, 10, () =>
                        {
                            AnimationCheck = false;
                            Invalidate();
                        });
                    }
                }
                else AnimationCheckValue = value ? 1F : 0F;
                Invalidate();
            }
        }

        /// <summary>
        /// 点击时自动改变选中状态
        /// </summary>
        [Description("点击时自动改变选中状态"), Category("行为"), DefaultValue(false)]
        public bool AutoCheck { get; set; } = true;

        /// <summary>
        /// 波浪大小
        /// </summary>
        [Description("波浪大小"), Category("外观"), DefaultValue(4)]
        public int WaveSize { get; set; } = 4;

        [Description("间距"), Category("外观"), DefaultValue(2)]
        public int Gap { get; set; } = 2;

        string? _checkedText = null, _unCheckedText = null;

        [Description("选中时显示的文本"), Category("外观"), DefaultValue(null)]
        public string? CheckedText
        {
            get => _checkedText;
            set
            {
                if (_checkedText == value) return;
                _checkedText = value;
                if (_checked) Invalidate();
            }
        }

        [Description("未选中时显示的文本"), Category("外观"), DefaultValue(null)]
        public string? UnCheckedText
        {
            get => _unCheckedText;
            set
            {
                if (_unCheckedText == value) return;
                _unCheckedText = value;
                if (!_checked) Invalidate();
            }
        }

        #endregion

        #region 事件

        /// <summary>
        /// Checked 属性值更改时发生
        /// </summary>
        [Description("Checked 属性值更改时发生"), Category("行为")]
        public event BoolEventHandler? CheckedChanged = null;

        #endregion

        #region 渲染

        protected override void OnPaint(PaintEventArgs e)
        {
            var rect = ClientRectangle;//绘制控件尺寸
            var g = e.Graphics.High();

            var rect_read = ReadRectangle;//读取控件尺寸
            bool enabled = Enabled;
            using (var path = rect_read.RoundPath(rect_read.Height))
            {
                Color _color = fill ?? Style.Db.Primary;
                PaintClick(g, path, rect, rect_read, _color);
                using (var brush = new SolidBrush(Style.Db.TextQuaternary))
                {
                    g.FillPath(brush, path);
                    if (AnimationHover)
                    {
                        using (var brush2 = new SolidBrush(Helper.ToColorN(AnimationHoverValue, brush.Color)))
                        {
                            g.FillPath(brush2, path);
                        }
                    }
                    else if (ExtraMouseHover) g.FillPath(brush, path);
                }
                float gap = (int)(Gap * Config.Dpi), gap2 = gap * 2F;
                if (AnimationCheck)
                {
                    var alpha = 255 * AnimationCheckValue;
                    using (var brush = new SolidBrush(Helper.ToColor(alpha, _color)))
                    {
                        g.FillPath(brush, path);
                    }
                    var dot_rect = new RectangleF(rect_read.X + gap + (rect_read.Width - rect_read.Height) * AnimationCheckValue, rect_read.Y + gap, rect_read.Height - gap2, rect_read.Height - gap2);
                    using (var brush = new SolidBrush(enabled ? Style.Db.BgBase : Color.FromArgb(200, Style.Db.BgBase)))
                    {
                        g.FillEllipse(brush, dot_rect);
                    }
                }
                else if (_checked)
                {
                    var colorhover = FillHover ?? Style.Db.PrimaryHover;
                    using (var brush = new SolidBrush(enabled ? _color : Color.FromArgb(200, _color)))
                    {
                        g.FillPath(brush, path);
                    }
                    if (AnimationHover)
                    {
                        using (var brush2 = new SolidBrush(Helper.ToColorN(AnimationHoverValue, colorhover)))
                        {
                            g.FillPath(brush2, path);
                        }
                    }
                    else if (ExtraMouseHover)
                    {
                        using (var brush = new SolidBrush(colorhover))
                        {
                            g.FillPath(brush, path);
                        }
                    }
                    var dot_rect = new RectangleF(rect_read.X + gap + rect_read.Width - rect_read.Height, rect_read.Y + gap, rect_read.Height - gap2, rect_read.Height - gap2);
                    using (var brush = new SolidBrush(enabled ? Style.Db.BgBase : Color.FromArgb(200, Style.Db.BgBase)))
                    {
                        g.FillEllipse(brush, dot_rect);
                    }
                }
                else
                {
                    var dot_rect = new RectangleF(rect_read.X + gap, rect_read.Y + gap, rect_read.Height - gap2, rect_read.Height - gap2);
                    using (var brush = new SolidBrush(enabled ? Style.Db.BgBase : Color.FromArgb(200, Style.Db.BgBase)))
                    {
                        g.FillEllipse(brush, dot_rect);
                    }
                }

                // 绘制文本
                string? textToRender = Checked ? CheckedText : UnCheckedText;
                if (textToRender != null)
                {
                    Color _fore = fore ?? Style.Db.PrimaryColor;
                    using (var brush = new SolidBrush(_fore))
                    {
                        var textSize = g.MeasureString(textToRender, Font);
                        var textRect = Checked
                            ? new RectangleF(rect_read.X + (rect_read.Width - rect_read.Height + gap2) / 2 - textSize.Width / 2, rect_read.Y + rect_read.Height / 2 - textSize.Height / 2, textSize.Width, textSize.Height)
                            : new RectangleF(rect_read.X + (rect_read.Height - gap + (rect_read.Width - rect_read.Height + gap) / 2 - textSize.Width / 2), rect_read.Y + rect_read.Height / 2 - textSize.Height / 2, textSize.Width, textSize.Height);
                        g.DrawStr(textToRender, Font, brush, textRect);
                    }
                }
            }
            this.PaintBadge(g);
            base.OnPaint(e);
        }
        internal void PaintClick(Graphics g, GraphicsPath path, Rectangle rect, RectangleF rect_read, Color color)
        {
            if (AnimationClick)
            {
                float maxw = rect_read.Width + ((rect.Width - rect_read.Width) * AnimationClickValue), maxh = rect_read.Height + ((rect.Height - rect_read.Height) * AnimationClickValue),
                    alpha = 100 * (1F - AnimationClickValue);
                using (var brush = new SolidBrush(Helper.ToColor(alpha, color)))
                {
                    using (var path_click = new RectangleF((rect.Width - maxw) / 2F, (rect.Height - maxh) / 2F, maxw, maxh).RoundPath(maxh))
                    {
                        path_click.AddPath(path, false);
                        g.FillPath(brush, path_click);
                    }
                }
            }
        }

        public override Rectangle ReadRectangle
        {
            get => ClientRectangle.PaddingRect(Padding, WaveSize * Config.Dpi);
        }

        public override GraphicsPath RenderRegion
        {
            get
            {
                var rect_read = ReadRectangle;
                return rect_read.RoundPath(rect_read.Height);
            }
        }

        #endregion

        #region 鼠标

        protected override void OnClick(EventArgs e)
        {
            if (AutoCheck) Checked = !_checked;
            base.OnClick(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            Focus();
            base.OnMouseDown(e);
        }

        float AnimationHoverValue = 0;
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
                if (enabled)
                {
                    if (Config.Animation)
                    {
                        ThreadHover?.Dispose();
                        AnimationHover = true;
                        if (value)
                        {
                            ThreadHover = new ITask(this, () =>
                            {
                                AnimationHoverValue = AnimationHoverValue.Calculate(0.1F);
                                if (AnimationHoverValue > 1) { AnimationHoverValue = 1F; return false; }
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
                    else AnimationHoverValue = 255;
                    Invalidate();
                }
            }
        }

        #region 动画

        protected override void Dispose(bool disposing)
        {
            ThreadClick?.Dispose();
            ThreadCheck?.Dispose();
            ThreadHover?.Dispose();
            base.Dispose(disposing);
        }
        ITask? ThreadHover = null;
        ITask? ThreadCheck = null;
        ITask? ThreadClick = null;

        bool AnimationClick = false;
        float AnimationClickValue = 0;
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (Config.Animation && e.Button == MouseButtons.Left)
            {
                ThreadClick?.Dispose();
                AnimationClickValue = 0;
                AnimationClick = true;
                ThreadClick = new ITask(this, () =>
                {
                    if (AnimationClickValue > 0.6) AnimationClickValue = AnimationClickValue.Calculate(0.04F);
                    else AnimationClickValue = AnimationClickValue.Calculate(0.1F);
                    if (AnimationClickValue > 1) { AnimationClickValue = 0F; return false; }
                    Invalidate();
                    return true;
                }, 50, () =>
                {
                    AnimationClick = false;
                    Invalidate();
                });
            }
        }

        #endregion

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            ExtraMouseHover = true;
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            ExtraMouseHover = false;
        }
        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            ExtraMouseHover = false;
        }

        #endregion
    }
}