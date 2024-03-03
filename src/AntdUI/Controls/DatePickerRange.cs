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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// DatePickerRange 日期范围选择框
    /// </summary>
    /// <remarks>输入或选择日期范围的控件。</remarks>
    [Description("DatePickerRange 日期范围选择框")]
    [ToolboxItem(true)]
    [DefaultProperty("Value")]
    [DefaultEvent("ValueChanged")]
    public class DatePickerRange : IControl, IButtonControl, SubLayeredForm, IEventListener
    {
        #region 属性

        /// <summary>
        /// 支持清除
        /// </summary>
        [Description("支持清除"), Category("行为"), DefaultValue(false)]
        public bool AllowClear { get; set; } = false;

        /// <summary>
        /// 格式化
        /// </summary>
        [Description("格式化"), Category("行为"), DefaultValue("yyyy-MM-dd")]
        public string Format { get; set; } = "yyyy-MM-dd";

        DateTime[]? _value = null;
        [Description("控件当前日期"), Category("数据"), DefaultValue(null)]
        public DateTime[]? Value
        {
            get => _value;
            set
            {
                _value = value;
                ValueChanged?.Invoke(this, value);
                if (value == null || value.Length == 0)
                {
                    TextStart = TextEnd = "";
                }
                else if (value.Length == 2)
                {
                    TextStart = value[0].ToString(Format);
                    TextEnd = value[1].ToString(Format);
                }
                else
                {
                    TextStart = value[0].ToString(Format);
                    TextEnd = "";
                }
            }
        }

        /// <summary>
        /// 日期徽标回调
        /// </summary>
        public Func<DateTime[], List<DateBadge>?>? BadgeAction = null;

        BaseCollection? items;
        /// <summary>
        /// 预置
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor", typeof(UITypeEditor))]
        [Description("预置"), Category("数据"), DefaultValue(null)]
        public BaseCollection Presets
        {
            get
            {
                items ??= new BaseCollection();
                return items;
            }
            set => items = value;
        }

        /// <summary>
        /// 菜单弹出位置
        /// </summary>
        [Description("菜单弹出位置"), Category("行为"), DefaultValue(TAlignFrom.BL)]
        public TAlignFrom Placement { get; set; } = TAlignFrom.BL;

        /// <summary>
        /// 下拉箭头是否显示
        /// </summary>
        [Description("下拉箭头是否显示"), Category("外观"), DefaultValue(true)]
        public bool DropDownArrow { get; set; } = true;

        #region 文本

        internal string text_start = "", text_end = "";
        /// <summary>
        /// 文本S
        /// </summary>
        [Description("文本S"), Category("外观"), DefaultValue("")]
        public string TextStart
        {
            get => textStart.Text;
            set
            {
                if (text_start == value) return;
                text_start = value;
                if (value == null) textStart.Text = null;
                else
                {
                    int old = textStart.SelectionStart, len = textStart.Text.Length;
                    int mode = 0;
                    if (old == len) mode = 1;
                    textStart.Text = value;
                    if (mode == 1) textStart.SelectionStart = value.Length;
                }
                ShowTextBox();
            }
        }

        /// <summary>
        /// 文本E
        /// </summary>
        [Description("文本E"), Category("外观"), DefaultValue("")]
        public string TextEnd
        {
            get => textEnd.Text;
            set
            {
                if (text_end == value) return;
                text_end = value;
                if (value == null) textEnd.Text = null;
                else
                {
                    int old = textEnd.SelectionStart, len = textEnd.Text.Length;
                    int mode = 0;
                    if (old == len) mode = 1;
                    textEnd.Text = value;
                    if (mode == 1) textEnd.SelectionStart = value.Length;
                }
                ShowTextBox();
            }
        }

        #endregion

        internal Color? fore;
        /// <summary>
        /// 文字颜色
        /// </summary>
        [Description("文字颜色"), Category("外观"), DefaultValue(null)]
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
        public Color? Back
        {
            get => back;
            set
            {
                if (back == value) return;
                back = value;
                Color _back = back.HasValue ? back.Value : Style.Db.BgContainer;
                textStart.BackColor = _back;
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
                SetTextSize();
            }
        }

        internal Image? image = null;
        /// <summary>
        /// 图标
        /// </summary>
        [Description("图标"), Category("外观"), DefaultValue(null)]
        public Image? Image
        {
            get => image;
            set
            {
                if (image == value) return;
                image = value;
                SetTextSize();
            }
        }

        /// <summary>
        /// 连接左边
        /// </summary>
        [Description("连接左边"), Category("外观"), DefaultValue(false)]
        public bool JoinLeft { get; set; } = false;

        /// <summary>
        /// 连接右边
        /// </summary>
        [Description("连接右边"), Category("外观"), DefaultValue(false)]
        public bool JoinRight { get; set; } = false;

        #endregion

        #region 事件

        public delegate void DateTimesEventHandler(object sender, DateTime[]? value);
        public event DateTimesEventHandler? ValueChanged;

        /// <summary>
        /// 预置点击时发生
        /// </summary>
        [Description("预置点击时发生"), Category("行为")]
        public event ObjectNEventHandler? PresetsClickChanged = null;

        #endregion

        #region 渲染

        internal StringFormat stringLeft = new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Near, Trimming = StringTrimming.EllipsisCharacter, FormatFlags = StringFormatFlags.NoWrap };
        protected override void OnPaint(PaintEventArgs e)
        {
            RectangleF rect = ClientRectangle.PaddingRect(Padding);
            var g = e.Graphics.High();
            var rect_read = ReadRectangle;
            float _radius = round ? rect_read.Height : radius * Config.Dpi;

            bool enabled = Enabled, _showWaterS = showWaterS, _showWaterE = showWaterE;

            using (var path = Path(rect_read, _radius))
            {
                Color _fore = fore.HasValue ? fore.Value : Style.Db.Text, _back = back.HasValue ? back.Value : Style.Db.BgContainer,
                    _border = borderColor.HasValue ? borderColor.Value : Style.Db.BorderColor,
                    _borderHover = BorderHover.HasValue ? BorderHover.Value : Style.Db.PrimaryHover,
                _borderActive = BorderActive.HasValue ? BorderActive.Value : Style.Db.Primary;
                PaintClick(g, path, rect, _borderActive, _radius);

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

                    if (!string.IsNullOrEmpty(textStart.Text)) _showWaterS = false;
                    if (!string.IsNullOrEmpty(textEnd.Text)) _showWaterE = false;
                }
                else
                {
                    using (var brush = new SolidBrush(Style.Db.FillTertiary))
                    {
                        g.FillPath(brush, path);
                    }
                    if (!string.IsNullOrEmpty(textStart.Text))
                    {
                        _showWaterS = false;
                        using (var brush = new SolidBrush(Style.Db.TextQuaternary))
                        {
                            g.DrawString(textStart.Text, Font, brush, rect_text_start, stringLeft);
                        }
                    }
                    if (!string.IsNullOrEmpty(textEnd.Text))
                    {
                        _showWaterE = false;
                        using (var brush = new SolidBrush(Style.Db.TextQuaternary))
                        {
                            g.DrawString(textEnd.Text, Font, brush, rect_text_end, stringLeft);
                        }
                    }
                    if (borderWidth > 0)
                    {
                        using (var brush = new Pen(_border, borderWidth))
                        {
                            g.DrawPath(brush, path);
                        }
                    }
                }
            }
            if (_showWaterS || _showWaterE)
            {
                using (var brush = new SolidBrush(Style.Db.TextQuaternary))
                {
                    if (_showWaterS) g.DrawString(placeholderS, Font, brush, rect_text_start, stringLeft);
                    if (_showWaterE) g.DrawString(placeholderE, Font, brush, rect_text_end, stringLeft);
                }
            }
            using (var pen = new Pen(Style.Db.TextQuaternary, rect_text_line.Height * 0.1F))
            {
                float h = rect_text_line.Height * 0.32F, y = rect_text_line.Y + (rect_text_line.Height) / 2;
                g.DrawLines(pen, new PointF[] {
                    new PointF(rect_text_line.Left, y),
                    new PointF(rect_text_line.Right, y),
                    new PointF(rect_text_line.Right-h, y-h)
                });
            }
            if (AnimationBar)
            {
                float h = rect_text_line.Height * 0.14F;
                var _borderActive = BorderActive.HasValue ? BorderActive.Value : Style.Db.Primary;
                using (var brush = new SolidBrush(_borderActive))
                {
                    g.FillRectangle(brush, new RectangleF(AnimationBarValue.X, rect_read.Bottom - h, AnimationBarValue.Width, h));
                }
            }
            else if (textStart.Focused || textEnd.Focused)
            {
                float h = rect_text_line.Height * 0.14F;
                var _borderActive = BorderActive.HasValue ? BorderActive.Value : Style.Db.Primary;
                using (var brush = new SolidBrush(_borderActive))
                {
                    if (textStart.Focused) g.FillRectangle(brush, new RectangleF(rect_text_start.X, rect_read.Bottom - h, rect_text_start.Width, h));
                    else g.FillRectangle(brush, new RectangleF(rect_text_end.X, rect_read.Bottom - h, rect_text_end.Width, h));
                }
            }
            PaintOther(g, rect_read);
            if (image != null) g.DrawImage(image, rect_icon_l);
            base.OnPaint(e);
        }

        #region 渲染帮助

        internal virtual void PaintOther(Graphics g, RectangleF rect_read)
        {
            if (AllowClear && _mouseHover && _value != null)
            {
                using (var brush = new SolidBrush(hover_clear ? Style.Db.TextTertiary : Style.Db.TextQuaternary))
                {
                    g.FillEllipse(brush, rect_icon_r);
                }
                g.PaintIconError(rect_icon_r, Style.Db.BgBase);
            }
            else
            {
                using (var pen = new Pen(Style.Db.TextQuaternary, 2F))
                {
                    var rect = new RectangleF(rect_icon_r.X - 1, rect_icon_r.Y, rect_icon_r.Width + 2, rect_icon_r.Height);
                    g.DrawRectangles(pen, new RectangleF[] { rect });
                    g.DrawLines(pen, new PointF[] {
                        new PointF(rect.X+1F, rect.Y+5),
                        new PointF(rect.Right-1F, rect.Y+5),
                    });
                    g.DrawLines(pen, new PointF[] {
                        new PointF(rect.X+4, rect.Y-2.6F),
                        new PointF(rect.X+4, rect.Y+2.6F),
                    });
                    g.DrawLines(pen, new PointF[] {
                        new PointF(rect.Right-4, rect.Y-2.6F),
                        new PointF(rect.Right-4, rect.Y+2.6F),
                    });
                }
            }
        }

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
                float _radius = round ? rect_read.Height : radius * Config.Dpi;
                return Path(rect_read, _radius);
            }
        }

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

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (showWaterS && (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right))
            {
                if (rect_text_end.Contains(e.Location)) TextShowEndFocus(true);
                else TextShowStartFocus(true);
            }
            if (e.Button == MouseButtons.Left && _mouseHover && rect_icon_r.Contains(e.Location))
            {
                if (_value != null && AllowClear)
                {
                    Value = null;
                    Invalidate();
                }
                return;
            }
            base.OnMouseClick(e);
        }

        bool hover_clear = false;
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (AllowClear && _mouseHover && _value != null)
            {
                var hover = rect_icon_r.Contains(e.Location);
                if (hover_clear != hover)
                {
                    hover_clear = hover;
                    SetCursor(hover);
                    Invalidate();
                }
            }
            else SetCursor(false);
            base.OnMouseMove(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            if (CanShow()) Cursor = Cursors.IBeam;
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

        #endregion

        #region 原生文本框

        internal ITextBox textStart = new ITextBox();

        internal ITextBox textEnd = new ITextBox
        {
            TabIndex = 1
        };

        public DatePickerRange()
        {
            SetStyle(ControlStyles.Selectable, true);
            textStart.Font = textEnd.Font = Font;
            Controls.Add(textStart);
            Controls.Add(textEnd);
            textStart.KeyDown += (s, e) =>
            {
                if (e.Control && e.KeyCode == Keys.A) textStart.SelectAll();
            };
            textEnd.KeyDown += (s, e) =>
            {
                if (e.Control && e.KeyCode == Keys.A) textEnd.SelectAll();
            };
            textStart.KeyPress += (s, e) =>
            {
                if (e.KeyChar == 13)
                {
                    if (DateTime.TryParse(textStart.Text, out var start))
                    {
                        if (DateTime.TryParse(textEnd.Text, out var end))
                        {
                            Value = new DateTime[] { start, end };
                            if (subForm != null)
                            {
                                subForm.SelDate = Value;
                                subForm.Print();
                            }
                        }
                        else textEnd.Focus();
                        e.Handled = true;
                    }
                }
            };
            textEnd.KeyPress += (s, e) =>
            {
                if (e.KeyChar == 13)
                {
                    if (DateTime.TryParse(textEnd.Text, out var end))
                    {
                        if (DateTime.TryParse(textStart.Text, out var start))
                        {
                            Value = new DateTime[] { start, end };
                            if (subForm != null)
                            {
                                subForm.SelDate = Value;
                                subForm.Print();
                            }
                        }
                        else textStart.Focus();
                        e.Handled = true;
                    }
                }
            };
            textStart.MouseEnter += TextBox_MouseEnter;
            textStart.MouseLeave += TextBox_MouseLeave;
            textEnd.MouseEnter += TextBox_MouseEnter;
            textEnd.MouseLeave += TextBox_MouseLeave;

            textStart.GotFocus += TextBox_GotFocus;
            textStart.MouseClick += TextBox_GotFocus;
            textStart.LostFocus += TextBoxStart_LostFocus;

            textEnd.GotFocus += TextBox_GotFocus;
            textEnd.MouseClick += TextBox_GotFocus;
            textEnd.LostFocus += TextBoxEnd_LostFocus;
        }

        #region 事件/焦点

        private void TextBox_MouseLeave(object? sender, EventArgs e)
        {
            ExtraMouseHover = false;
        }

        private void TextBox_MouseEnter(object? sender, EventArgs e)
        {
            ExtraMouseHover = true;
        }

        bool AnimationFocus = false;
        int AnimationFocusValue = 0;

        protected override void OnGotFocus(EventArgs e)
        {
            textStart.Focus();
            base.OnGotFocus(e);
        }

        private void TextBox_GotFocus(object? sender, EventArgs e)
        {
            StartEndFocused();
            ExtraMouseDown = TextFocus = textStart.Focused || textEnd.Focused;
        }
        private void TextBoxStart_LostFocus(object? sender, EventArgs e)
        {
            if (showWaterS && string.IsNullOrEmpty(textStart.Text)) TextShowStart(false);
            StartEndFocused();
            ExtraMouseDown = TextFocus = textStart.Focused || textEnd.Focused;
        }

        private void TextBoxEnd_LostFocus(object? sender, EventArgs e)
        {
            if (showWaterE && string.IsNullOrEmpty(textEnd.Text)) TextShowEnd(false);
            StartEndFocused();
            ExtraMouseDown = TextFocus = textStart.Focused || textEnd.Focused;
        }

        #region 下拉

        bool AnimationBar = false;
        RectangleF AnimationBarValue;
        ITask? ThreadBar = null;

        LayeredFormCalendarRange? subForm = null;
        public ILayeredForm? SubForm() { return subForm; }

        bool textFocus = false;

        string startEndFocused = "00";
        void StartEndFocused()
        {
            bool s = textStart.Focused, e = textEnd.Focused;
            string temp = (s ? 1 : 0).ToString() + (e ? 1 : 0).ToString();
            if (startEndFocused == temp) return;
            startEndFocused = temp;
            if (Config.Animation && (s || e))
            {
                AnimationBar = true;
                RectangleF NewValue;
                if (s) NewValue = rect_text_start;
                else NewValue = rect_text_end;
                float p_val = Math.Abs(NewValue.X - AnimationBarValue.X) * 0.09F, p_val2 = (NewValue.X - AnimationBarValue.X) * 0.5F;
                bool left = NewValue.X > AnimationBarValue.X;
                ThreadBar?.Dispose();
                ThreadBar = new ITask(this, () =>
                {
                    if (left)
                    {
                        if (AnimationBarValue.X > p_val2) AnimationBarValue.X += p_val / 2F;
                        else AnimationBarValue.X += p_val;
                        if (AnimationBarValue.X > NewValue.X)
                        {
                            AnimationBarValue.X = NewValue.X;
                            Invalidate();
                            return false;
                        }
                    }
                    else
                    {
                        AnimationBarValue.X -= p_val;
                        if (AnimationBarValue.X < NewValue.X)
                        {
                            AnimationBarValue.X = NewValue.X;
                            Invalidate();
                            return false;
                        }
                    }
                    if (Placement == TAlignFrom.TR || Placement == TAlignFrom.BR) subForm?.SetArrow(AnimationBarValue.X - rect_text_end.X);
                    else subForm?.SetArrow(AnimationBarValue.X);
                    Invalidate();
                    return true;
                }, 10, () =>
                {
                    if (Placement == TAlignFrom.TR || Placement == TAlignFrom.BR) subForm?.SetArrow(NewValue.X - rect_text_end.X);
                    else subForm?.SetArrow(NewValue.X);
                    AnimationBarValue = NewValue;
                    AnimationBar = false;
                    Invalidate();
                });
            }
            else
            {
                if (s) AnimationBarValue = rect_text_start;
                else AnimationBarValue = rect_text_end;
            }
        }

        bool TextFocus
        {
            get => textFocus;
            set
            {
                if (textFocus == value) return;
                textFocus = value;
                if (value)
                {
                    subForm?.IClose();
                    subForm = new LayeredFormCalendarRange(this, ReadRectangle, _value, date =>
                    {
                        Value = date;
                    }, btn =>
                    {
                        PresetsClickChanged?.Invoke(this, btn);
                    }, BadgeAction);
                    subForm.Disposed += (a, b) =>
                    {
                        subForm = null;
                        TextFocus = false;
                    };
                    subForm.Show(this);
                }
                else subForm?.IClose();
            }
        }

        #endregion

        #endregion

        #region 位置/触发

        internal void GetRectTI(int text_height, bool icon_l, bool icon_r)
        {
            var rect_ = ReadRectangle.IconRect(text_height, icon_l, icon_r, RightToLeft == RightToLeft.Yes, false);
            int gap = (int)Math.Ceiling(rect_.text.Height * 2.4F), width = (rect_.text.Width - gap) / 2;

            rect_text_line = new Rectangle(rect_.text.X + (rect_.text.Width - rect_.text.Height) / 2, rect_.text.Y, rect_.text.Height, rect_.text.Height);
            rect_text_start = new Rectangle(rect_.text.X, rect_.text.Y, width, rect_.text.Height);
            rect_text_end = new Rectangle(rect_.text.X + width + gap, rect_.text.Y, width, rect_.text.Height);

            rect_icon_l = rect_.l;
            rect_icon_r = rect_.r;
        }

        internal Rectangle rect_text_start, rect_text_line, rect_text_end, rect_icon_l, rect_icon_r;

        internal void SetTextSize()
        {
            GetRectTI(textStart.Height, image != null, true);
            textStart.SetRect(rect_text_start);
            textEnd.SetRect(rect_text_end);
        }

        protected override void OnFontChanged(EventArgs e)
        {
            textStart.Font = textEnd.Font = Font;
            SetTextSize();
            base.OnFontChanged(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            SetTextSize();
            base.OnSizeChanged(e);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            ShowTextBox();
        }

        protected override void OnCreateControl()
        {
            Color _fore = fore.HasValue ? fore.Value : Style.Db.Text, _back = back.HasValue ? back.Value : Style.Db.BgContainer;
            textStart.Font = textEnd.Font = Font;
            textStart.BackColor = textEnd.BackColor = _back;
            textStart.ForeColor = textEnd.ForeColor = _fore;
            SetTextSize();
            base.OnCreateControl();
        }

        #endregion

        #region 水印/显示

        internal virtual bool CanShow() { return true; }
        internal virtual void ShowTextBox()
        {
            if (Enabled)
            {
                if (showWaterS)
                {
                    //显示水印
                    TextShowStart(textStart.Focused || !string.IsNullOrEmpty(textStart.Text));
                }
                else TextShowStart(true);

                if (showWaterE)
                {
                    //显示水印
                    TextShowEnd(textEnd.Focused || !string.IsNullOrEmpty(textEnd.Text));
                }
                else TextShowEnd(true);
            }
            else
            {
                TextShowStart(false);
                TextShowEnd(false);
            }
        }
        internal void TextShowStart(bool val)
        {
            if (val && CanShow() || !val)
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        TextShowStart(val);
                    }));
                    return;
                }
                textStart.Visible = val;
            }
        }
        internal void TextShowEnd(bool val)
        {
            if (val && CanShow() || !val)
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        TextShowEnd(val);
                    }));
                    return;
                }
                textEnd.Visible = val;
            }
        }
        internal void TextShowStartFocus(bool val)
        {
            if (val && CanShow() || !val)
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        TextShowStartFocus(val);
                    }));
                    return;
                }
                textStart.Visible = val;
                if (val) textStart.Focus();
            }
        }
        internal void TextShowEndFocus(bool val)
        {
            if (val && CanShow() || !val)
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        TextShowEndFocus(val);
                    }));
                    return;
                }
                textEnd.Visible = val;
                if (val) textEnd.Focus();
            }
        }

        #endregion

        #region 继承文本框

        /// <summary>
        /// 控制能否更改编辑控件中的文本
        /// </summary>
        [Description("控制能否更改编辑控件中的文本"), Category("行为"), DefaultValue(false)]
        public bool ReadOnly
        {
            get => textStart.ReadOnly;
            set
            {
                textStart.ReadOnly = textEnd.ReadOnly = value;
            }
        }

        /// <summary>
        /// 指定可以在编辑控件中输入的最大字符数
        /// </summary>
        [Description("指定可以在编辑控件中输入的最大字符数"), Category("行为"), DefaultValue(32767)]
        public int MaxLength
        {
            get => textStart.MaxLength;
            set
            {
                textStart.MaxLength = textEnd.MaxLength = value;
            }
        }

        internal bool showWaterS = false, showWaterE = false;
        string? placeholderS = null, placeholderE = null;
        /// <summary>
        /// 显示的水印文本
        /// </summary>
        [Description("显示的水印文本S"), Category("行为"), DefaultValue(null)]
        public virtual string? PlaceholderStart
        {
            get => placeholderS;
            set
            {
                if (placeholderS == value) return;
                placeholderS = value;
                showWaterS = value != null;
                ShowTextBox();
                Invalidate();
            }
        }

        /// <summary>
        /// 显示的水印文本
        /// </summary>
        [Description("显示的水印文本E"), Category("行为"), DefaultValue(null)]
        public virtual string? PlaceholderEnd
        {
            get => placeholderE;
            set
            {
                if (placeholderE == value) return;
                placeholderE = value;
                showWaterE = value != null;
                ShowTextBox();
                Invalidate();
            }
        }

        #endregion

        #endregion

        #region 主题变化

        protected override void CreateHandle()
        {
            base.CreateHandle();
            EventManager.Instance.AddListener(1, this);
        }

        #region 释放/动画

        protected override void Dispose(bool disposing)
        {
            ThreadBar?.Dispose();
            ThreadFocus?.Dispose();
            ThreadHover?.Dispose();
            EventManager.Instance.RemoveListener(1, this);
            base.Dispose(disposing);
        }
        ITask? ThreadHover = null;
        ITask? ThreadFocus = null;

        #endregion

        public void HandleEvent(int eventId, IEventArgs? args)
        {
            switch (eventId)
            {
                case 1:
                    Color _fore = fore.HasValue ? fore.Value : Style.Db.Text, _back = back.HasValue ? back.Value : Style.Db.BgContainer;
                    textStart.BackColor = textEnd.BackColor = _back;
                    textStart.ForeColor = textEnd.ForeColor = _fore;
                    Refresh();
                    break;
            }
        }

        #endregion

        #region 按钮点击

        DialogResult dialogResult = DialogResult.None;

        [DefaultValue(DialogResult.None)]
        public DialogResult DialogResult
        {
            get { return dialogResult; }
            set
            {
                if (dialogResult != value)
                {
                    dialogResult = value;
                }
            }
        }

        /// <summary>
        /// 是否默认按钮
        /// </summary>
        public void NotifyDefault(bool value)
        {
            if (showWaterS) TextShowStartFocus(true);
        }

        public void PerformClick()
        {
        }

        #endregion
    }

    class ITextBox : TextBox
    {
        public ITextBox()
        {
            BorderStyle = BorderStyle.None;
            Anchor = AnchorStyles.Left | AnchorStyles.Right;
        }

        Rectangle recttag;

        public void SetRect(Rectangle rect)
        {
            recttag = rect;
            Size = rect.Size;
            Location = rect.Location;
        }
        protected override void OnVisibleChanged(EventArgs e)
        {
            Size = recttag.Size;
            Location = recttag.Location;
            base.OnVisibleChanged(e);
        }
    }
}