// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
// THE AntdUI PROJECT IS AN WINFORM LIBRARY LICENSED UNDER THE GPL-3.0 License.
// LICENSED UNDER THE GPL License, VERSION 3.0 (THE "License")
// YOU MAY NOT USE THIS FILE EXCEPT IN COMPLIANCE WITH THE License.
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
    /// Input 输入框
    /// </summary>
    /// <remarks>通过鼠标或键盘输入内容，是最基础的表单域的包装。</remarks>
    [Description("Input 输入框")]
    [ToolboxItem(true)]
    [DefaultProperty("Text")]
    [DefaultEvent("TextChanged")]
    public class Input : IControl, IButtonControl, IEventListener
    {
        #region 属性

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
                textBox.BackColor = _back;
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

        TType status = TType.None;
        /// <summary>
        /// 设置校验状态
        /// </summary>
        [Description("设置校验状态"), Category("外观"), DefaultValue(TType.None)]
        public TType Status
        {
            get => status;
            set
            {
                if (status == value) return;
                status = value;
                Invalidate();
            }
        }

        Image? image = null;
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

        string? imageSvg = null;
        [Description("图标SVG"), Category("外观"), DefaultValue(null)]
        public string? ImageSvg
        {
            get => imageSvg;
            set
            {
                if (imageSvg == value) return;
                imageSvg = value;
                SetTextSize();
            }
        }

        /// <summary>
        /// 是否包含图片
        /// </summary>
        public bool HasImage
        {
            get => imageSvg != null || image != null;
        }

        /// <summary>
        /// 文本
        /// </summary>
        [Description("文本"), Category("外观"), DefaultValue("")]
        public new string Text
        {
            get => textBox.Text;
            set
            {
                if (textBox.Text == value) return;
                if (InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        SetTextValue(value);
                    }));
                }
                else SetTextValue(value);
            }
        }

        void SetTextValue(string value)
        {
            if (value == null) textBox.Text = null;
            else
            {
                var oldtext = textBox.Text;
                int old = textBox.SelectionStart, len = oldtext.Length;
                int mode = 0;
                if (old == len && !string.IsNullOrEmpty(oldtext)) mode = 1;
                textBox.Text = value;
                if (mode == 1) textBox.SelectionStart = value.Length;
            }
            ShowTextBox();
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

        #region 渲染

        internal StringFormat stringLeft = new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Near, Trimming = StringTrimming.EllipsisCharacter, FormatFlags = StringFormatFlags.NoWrap };
        internal StringFormat stringTL = new StringFormat { LineAlignment = StringAlignment.Near, Alignment = StringAlignment.Near, Trimming = StringTrimming.EllipsisCharacter };
        protected override void OnPaint(PaintEventArgs e)
        {
            RectangleF rect = ClientRectangle.PaddingRect(Padding);
            var g = e.Graphics.High();
            var rect_read = ReadRectangle;
            float _radius = round ? rect_read.Height : radius * Config.Dpi;

            bool enabled = Enabled, _showWater = showWater;

            using (var path = Path(rect_read, _radius))
            {
                Color _back = back.HasValue ? back.Value : Style.Db.BgContainer,
                   _border = borderColor.HasValue ? borderColor.Value : Style.Db.BorderColor,
                   _borderHover = BorderHover.HasValue ? BorderHover.Value : Style.Db.PrimaryHover,
               _borderActive = BorderActive.HasValue ? BorderActive.Value : Style.Db.Primary;

                switch (status)
                {
                    case TType.Success:
                        _border = Style.Db.SuccessBorder;
                        _borderHover = Style.Db.SuccessHover;
                        _borderActive = Style.Db.Success;
                        break;
                    case TType.Error:
                        _border = Style.Db.ErrorBorder;
                        _borderHover = Style.Db.ErrorHover;
                        _borderActive = Style.Db.Error;
                        break;
                    case TType.Warn:
                        _border = Style.Db.WarningBorder;
                        _borderHover = Style.Db.WarningHover;
                        _borderActive = Style.Db.Warning;
                        break;
                }

                PaintClick(g, path, rect, _borderActive, _radius);

                if (enabled)
                {
                    using (var brush = new SolidBrush(_back))
                    {
                        g.FillPath(brush, path);
                    }
                    PaintOtherBor(g, rect_read, _radius, _border, _borderActive);
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

                    if (!string.IsNullOrEmpty(textBox.Text)) _showWater = false;
                }
                else
                {
                    using (var brush = new SolidBrush(Style.Db.FillTertiary))
                    {
                        g.FillPath(brush, path);
                    }
                    if (!string.IsNullOrEmpty(textBox.Text))
                    {
                        _showWater = false;
                        using (var brush = new SolidBrush(Style.Db.TextQuaternary))
                        {
                            g.DrawString(textBox.Text, Font, brush, rect_text, textBox.Multiline ? stringTL : stringLeft);
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
            if (_showWater)
            {
                using (var brush = new SolidBrush(Style.Db.TextQuaternary))
                {
                    g.DrawString(placeholderText, Font, brush, textBox.Bounds, textBox.Multiline ? stringTL : stringLeft);
                }
            }
            PaintOther(g, rect_read);
            PaintImage(g);
            this.PaintBadge(g);
            base.OnPaint(e);
        }

        #region 渲染帮助

        #region 渲染图片

        /// <summary>
        /// 渲染图片
        /// </summary>
        /// <param name="g">GDI</param>
        /// <param name="color">颜色</param>
        /// <param name="rectl">图标区域</param>
        void PaintImage(Graphics g)
        {
            if (imageSvg != null)
            {
                using (var _bmp = SvgExtend.GetImgExtend(imageSvg, rect_icon_l, fore.HasValue ? fore.Value : Style.Db.Text))
                {
                    if (_bmp != null) g.DrawImage(_bmp, rect_icon_l);
                }
                return;
            }
            else if (image != null)
            {
                g.DrawImage(image, rect_icon_l);
                return;
            }
        }

        #endregion

        internal virtual void PaintOtherBor(Graphics g, RectangleF rect_read, float radius, Color borderColor, Color borderActive)
        {
        }

        internal virtual void PaintOther(Graphics g, RectangleF rect_read)
        {
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
                ChangeMouseHover(_mouseHover, value);
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
                ChangeMouseHover(value, _mouseDown);
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
        internal virtual void ChangeMouseHover(bool Hover, bool Focus) { }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (showWater && (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right))
            {
                TextShowFocus(true);
            }
            base.OnMouseClick(e);
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

        internal TextBox textBox = new TextBox
        {
            BorderStyle = BorderStyle.None,
            TabIndex = 0,
            Anchor = AnchorStyles.Left | AnchorStyles.Right
        };

        public Input()
        {
            SetStyle(ControlStyles.Selectable, true);
            textBox.Font = Font;
            textBox.Size = Size;
            Controls.Add(textBox);
            SetTextSize();
            textBox.KeyDown += (s, e) =>
            {
                if (e.Control && e.KeyCode == Keys.A) textBox.SelectAll();
            };
            textBox.MouseEnter += TextBox_MouseEnter;
            textBox.MouseLeave += TextBox_MouseLeave;
            textBox.GotFocus += TextBox_GotFocus;
            textBox.MouseClick += TextBox_GotFocus;
            textBox.LostFocus += TextBox_LostFocus;
            textBox.Leave += TextBox_MouseLeave;
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
            textBox.Focus();
            base.OnGotFocus(e);
        }

        private void TextBox_GotFocus(object? sender, EventArgs e)
        {
            ExtraMouseDown = true;
            TextGotFocus();
        }
        private void TextBox_LostFocus(object? sender, EventArgs e)
        {
            if (showWater && string.IsNullOrEmpty(textBox.Text)) TextShow(false);
            ExtraMouseDown = false;
            TextLostFocus();
        }
        internal virtual void TextGotFocus() { }
        internal virtual void TextLostFocus() { }

        #endregion

        #region 位置/触发

        internal virtual void GetRectTI(int text_height, bool icon_l, bool icon_r)
        {
            if (textBox.Multiline)
            {
                using (var bmp = new Bitmap(1, 1))
                {
                    using (var g = Graphics.FromImage(bmp))
                    {
                        var font_size = g.MeasureString(Config.NullText, Font);
                        var rect_ = ReadRectangle.IconRect(font_size.Height, icon_l, icon_r, RightToLeft == RightToLeft.Yes, true);
                        rect_text = rect_.text;
                        rect_icon_l = rect_.l;
                        rect_icon_r = rect_.r;
                    }
                }
            }
            else
            {
                var rect_ = ReadRectangle.IconRect(text_height, icon_l, icon_r, RightToLeft == RightToLeft.Yes, false);
                rect_text = rect_.text;
                rect_icon_l = rect_.l;
                rect_icon_r = rect_.r;
            }
        }

        internal Rectangle rect_text, rect_icon_l, rect_icon_r;
        internal virtual void RectTI()
        {
            GetRectTI(textBox.Height, HasImage, false);
        }
        internal virtual void SetTextSize()
        {
            RectTI();
            textBox.Size = rect_text.Size;
            textBox.Location = rect_text.Location;
        }

        protected override void OnFontChanged(EventArgs e)
        {
            textBox.Font = Font;
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
            textBox.Font = Font;
            textBox.BackColor = _back;
            textBox.ForeColor = _fore;
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
                if (showWater)
                {
                    //显示水印
                    TextShow(textBox.Focused || !string.IsNullOrEmpty(textBox.Text));
                }
                else TextShow(true);
            }
            else TextShow(false);
        }
        internal void TextShow(bool val)
        {
            if (val && CanShow() || !val)
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        TextShow(val);
                    }));
                    return;
                }
                textBox.Visible = val;
            }
        }
        internal void TextShowFocus(bool val)
        {
            if (val && CanShow() || !val)
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        TextShowFocus(val);
                    }));
                    return;
                }
                textBox.Visible = val;
                if (val) textBox.Focus();
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 将文本追加到当前文本中
        /// </summary>
        /// <param name="text">追加的文本</param>
        public void AppendText(string text)
        {
            textBox.AppendText(text);
        }

        /// <summary>
        /// 清除所有文本
        /// </summary>
        public void Clear()
        {
            textBox.Clear();
        }

        /// <summary>
        /// 清除撤消缓冲区信息
        /// </summary>
        public void ClearUndo()
        {
            textBox.ClearUndo();
        }

        /// <summary>
        /// 复制
        /// </summary>
        public void Copy()
        {
            textBox.Copy();
        }

        /// <summary>
        /// 剪贴
        /// </summary>
        public void Cut()
        {
            textBox.Cut();
        }

        /// <summary>
        /// 粘贴
        /// </summary>
        public void Paste()
        {
            textBox.Paste();
        }

        /// <summary>
        /// 取消全部选中
        /// </summary>
        public void DeselectAll()
        {
            textBox.DeselectAll();
        }

        /// <summary>
        /// 撤消
        /// </summary>
        public void Undo()
        {
            textBox.Undo();
        }

        /// <summary>
        /// 文本选择范围
        /// </summary>
        /// <param name="start">第一个字符的位置</param>
        /// <param name="length">字符长度</param>
        public void Select(int start, int length)
        {
            textBox.Select(start, length);
        }

        /// <summary>
        /// 选择所有文本
        /// </summary>
        public void SelectAll()
        {
            textBox.SelectAll();
        }

        #endregion

        #region 继承文本框

        /// <summary>
        /// 多行文本
        /// </summary>
        [Description("多行文本"), Category("行为"), DefaultValue(false)]
        public bool Multiline
        {
            get => textBox.Multiline;
            set
            {
                if (textBox.Multiline == value) return;
                textBox.Multiline = value;
                SetTextSize();
            }
        }

        /// <summary>
        /// 文本对齐方向
        /// </summary>
        [Description("文本对齐方向"), Category("外观"), DefaultValue(HorizontalAlignment.Left)]
        public HorizontalAlignment TextAlign
        {
            get => textBox.TextAlign;
            set
            {
                textBox.TextAlign = value;
            }
        }

        /// <summary>
        /// 使用密码框
        /// </summary>
        [Description("使用密码框"), Category("行为"), DefaultValue(false)]
        public bool UseSystemPasswordChar
        {
            get => textBox.UseSystemPasswordChar;
            set
            {
                textBox.UseSystemPasswordChar = value;
            }
        }

        /// <summary>
        /// 只读
        /// </summary>
        [Description("只读"), Category("行为"), DefaultValue(false)]
        public bool ReadOnly
        {
            get => textBox.ReadOnly;
            set
            {
                textBox.ReadOnly = value;
            }
        }

        /// <summary>
        /// 文本最大长度
        /// </summary>
        [Description("文本最大长度"), Category("行为"), DefaultValue(32767)]
        public int MaxLength
        {
            get => textBox.MaxLength;
            set
            {
                textBox.MaxLength = value;
            }
        }

        /// <summary>
        /// 自定义密码字符
        /// </summary>
        [Description("自定义密码字符"), Category("行为"), DefaultValue((char)0)]
        public char PasswordChar
        {
            get => textBox.PasswordChar;
            set
            {
                textBox.PasswordChar = value;
            }
        }

        internal bool showWater = false;
        string? placeholderText = null;
        /// <summary>
        /// 水印文本
        /// </summary>
        [Description("水印文本"), Category("行为"), DefaultValue(null)]
        public virtual string? PlaceholderText
        {
            get => placeholderText;
            set
            {
                if (placeholderText == value) return;
                placeholderText = value;
                showWater = value != null;
                ShowTextBox();
                Invalidate();
            }
        }

        public new event EventHandler TextChanged
        {
            add => textBox.TextChanged += value;
            remove => textBox.TextChanged -= value;
        }

        public new event KeyPressEventHandler KeyPress
        {
            add => textBox.KeyPress += value;
            remove => textBox.KeyPress -= value;
        }

        public new event KeyEventHandler KeyDown
        {
            add => textBox.KeyDown += value;
            remove => textBox.KeyDown -= value;
        }

        public new event KeyEventHandler KeyUp
        {
            add => textBox.KeyUp += value;
            remove => textBox.KeyUp -= value;
        }
        public new event EventHandler GotFocus
        {
            add => textBox.GotFocus += value;
            remove => textBox.GotFocus -= value;
        }

        public new event EventHandler LostFocus
        {
            add => textBox.LostFocus += value;
            remove => textBox.LostFocus -= value;
        }

        /// <summary>
        /// 所选文本的起点
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectionStart
        {
            get => textBox.SelectionStart;
            set => textBox.SelectionStart = value;
        }

        /// <summary>
        /// 所选文本的长度
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectionLength
        {
            get => textBox.SelectionLength;
            set => textBox.SelectionLength = value;
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
                    textBox.BackColor = _back;
                    textBox.ForeColor = _fore;
                    Refresh();
                    break;
            }
        }

        #endregion

        #region 按钮点击

        [DefaultValue(DialogResult.None)]
        public DialogResult DialogResult { get; set; } = DialogResult.None;

        /// <summary>
        /// 是否默认按钮
        /// </summary>
        public void NotifyDefault(bool value)
        {
            if (showWater) TextShowFocus(true);
        }

        public void PerformClick()
        {
        }

        #endregion
    }
}