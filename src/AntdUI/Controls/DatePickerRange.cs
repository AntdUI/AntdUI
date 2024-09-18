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
    public class DatePickerRange : Input, SubLayeredForm
    {
        #region 属性

        bool showS = true, showE = true;
        string? placeholderS = null, placeholderE = null;
        /// <summary>
        /// 显示的水印文本
        /// </summary>
        [Description("显示的水印文本S"), Category("行为"), DefaultValue(null)]
        public string? PlaceholderStart
        {
            get => placeholderS;
            set
            {
                if (placeholderS == value) return;
                placeholderS = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 显示的水印文本
        /// </summary>
        [Description("显示的水印文本E"), Category("行为"), DefaultValue(null)]
        public string? PlaceholderEnd
        {
            get => placeholderE;
            set
            {
                if (placeholderE == value) return;
                placeholderE = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 水印文本
        /// </summary>
        [Browsable(false), Description("水印文本"), Category("行为"), DefaultValue(null)]
        public override string? PlaceholderText { get => null; }

        string dateFormat = "yyyy-MM-dd";
        bool ShowTime = false;

        /// <summary>
        /// 格式化
        /// </summary>
        [Description("格式化"), Category("行为"), DefaultValue("yyyy-MM-dd")]
        public string Format
        {
            get => dateFormat;
            set
            {
                if (dateFormat == value) return;
                dateFormat = value;
                ShowTime = dateFormat.Contains("H");
            }
        }

        DateTime[]? _value = null;
        [Description("控件当前日期"), Category("数据"), DefaultValue(null)]
        public DateTime[]? Value
        {
            get => _value;
            set
            {
                _value = value;
                ValueChanged?.Invoke(this, new DateTimesEventArgs(value));
                if (value == null) Text = "";
                else Text = value[0].ToString(Format) + "\t" + value[1].ToString(Format);
            }
        }

        /// <summary>
        /// 最小日期
        /// </summary>
        [Description("最小日期"), Category("数据"), DefaultValue(null)]
        public DateTime? MinDate { get; set; }

        /// <summary>
        /// 最大日期
        /// </summary>
        [Description("最大日期"), Category("数据"), DefaultValue(null)]
        public DateTime? MaxDate { get; set; }

        protected override void OnTextChanged(EventArgs e)
        {
            if (isempty) showS = showE = true;
            else
            {
                string text = Text;
                int index = text.IndexOf("\t");
                if (index > -1)
                {
                    showS = index == 0;
                    showE = string.IsNullOrEmpty(text.Substring(index + 1));
                }
                else showS = showE = false;
            }
            base.OnTextChanged(e);
        }

        protected override bool Verify(char key, out string? change)
        {
            if (StartFocused)
            {
                int index = Text.IndexOf("\t");
                if (index != -1)
                {
                    if (SelectionStart > index) SelectionStart = index;
                }
            }
            else if (EndFocused)
            {
                int index = Text.IndexOf("\t");
                if (index == -1)
                {
                    change = "\t" + key;
                    return true;
                }
            }
            return base.Verify(key, out change);
        }

        string? swapSvg = null;
        /// <summary>
        /// 交换图标SVG
        /// </summary>
        [Description("交换图标SVG"), Category("外观"), DefaultValue(null)]
        public string? SwapSvg
        {
            get => swapSvg;
            set
            {
                if (swapSvg == value) return;
                swapSvg = value;
                Invalidate();
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

        /// <summary>
        /// 焦点时展开下拉
        /// </summary>
        [Description("焦点时展开下拉"), Category("行为"), DefaultValue(true)]
        public bool FocusExpandDropdown { get; set; } = true;

        protected override void OnHandleCreated(EventArgs e)
        {
            if (_value != null) Text = _value[0].ToString(Format) + "\t" + _value[1].ToString(Format);
            base.OnHandleCreated(e);
        }

        #region 自带图标

        bool showicon = true;
        /// <summary>
        /// 是否显示图标
        /// </summary>
        [Description("是否显示图标"), Category("外观"), DefaultValue(true)]
        public bool ShowIcon
        {
            get => showicon;
            set
            {
                if (showicon == value) return;
                showicon = value;
                CalculateRect();
                Invalidate();
            }
        }

        public override bool HasSuffix
        {
            get => showicon;
        }

        internal override void PaintRIcon(Graphics g, Rectangle rect_r)
        {
            if (showicon)
            {
                using (var bmp = SvgDb.IcoDate.SvgToBmp(rect_r.Width, rect_r.Height, Style.Db.TextQuaternary))
                {
                    if (bmp == null) return;
                    g.DrawImage(bmp, rect_r);
                }
            }
        }

        #endregion

        #endregion

        #region 事件

        public event DateTimesEventHandler? ValueChanged;

        /// <summary>
        /// 预置点击时发生
        /// </summary>
        [Description("预置点击时发生"), Category("行为")]
        public event ObjectNEventHandler? PresetsClickChanged = null;

        #endregion

        #region 焦点

        bool textFocus = false;
        bool TextFocus
        {
            get => textFocus;
            set
            {
                if (textFocus == value) return;
                textFocus = value;
                if (!ReadOnly && value)
                {
                    if (subForm == null)
                    {
                        if (ShowTime)
                        {
                            subForm = new LayeredFormCalendarTimeRange(this, ReadRectangle, _value, date =>
                            {
                                Value = date;
                            }, btn =>
                            {
                                PresetsClickChanged?.Invoke(this, new ObjectNEventArgs(btn));
                            }, BadgeAction);
                            subForm.Disposed += (a, b) =>
                            {
                                subForm = null;
                                TextFocus = false;
                            };
                            subForm.Show(this);
                        }
                        else
                        {
                            subForm = new LayeredFormCalendarRange(this, ReadRectangle, _value, date =>
                            {
                                Value = date;
                            }, btn =>
                            {
                                PresetsClickChanged?.Invoke(this, new ObjectNEventArgs(btn));
                            }, BadgeAction);
                            subForm.Disposed += (a, b) =>
                            {
                                subForm = null;
                                TextFocus = false;
                            };
                            subForm.Show(this);
                        }
                    }
                }
                else subForm?.IClose();
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            if (!StartFocused && !EndFocused) StartFocused = true;
            StartEndFocused();
            if (FocusExpandDropdown) TextFocus = true;
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            TextFocus = StartFocused = EndFocused = false;
            StartEndFocused();
            AnimationBarValue = RectangleF.Empty;
            if (IsHandleCreated)
            {
                string text = Text;
                int index = text.IndexOf("\t");
                if (index > 0)
                {
                    string stext = text.Substring(0, index), etext = text.Substring(index + 1);
                    if (DateTime.TryParse(stext, out var date_s) && DateTime.TryParse(etext, out var date_e)) Value = new DateTime[] { date_s, date_e };
                }
            }
            base.OnLostFocus(e);
        }

        #region 动画

        ILayeredForm? subForm = null;
        public ILayeredForm? SubForm() => subForm;

        #endregion

        #endregion

        #region 鼠标

        internal override void OnClearValue()
        {
            Value = null;
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && _mouseHover)
            {
                if (HasFocus)
                {
                    if (textFocus) return;
                    TextFocus = !textFocus;
                }
                else Focus();
                return;
            }
            base.OnMouseClick(e);
        }

        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape && subForm != null)
            {
                subForm.IClose();
                return true;
            }
            else if (keyData == Keys.Enter)
            {
                if (StartFocused || EndFocused)
                {
                    string text = Text;
                    int index = text.IndexOf("\t");
                    if (StartFocused)
                    {
                        string stext;
                        if (index == -1) stext = text;
                        else stext = text.Substring(0, index);
                        if (DateTime.TryParse(stext, out var date_s))
                        {
                            if (index == -1)
                            {
                                Text = date_s.ToString(Format) + '\t';
                                SelectionStart = Text.Length;
                            }
                            else
                            {
                                var etext = text.Substring(index + 1);
                                if (DateTime.TryParse(etext, out var date_e)) Text = date_s.ToString(Format) + '\t' + date_e.ToString(Format);
                                else Text = date_s.ToString(Format) + '\t' + etext;
                            }
                            if (subForm is LayeredFormCalendarRange layered_range)
                            {
                                layered_range.Date = date_s;
                                layered_range.SetDateS(date_s);
                                layered_range.Print();
                            }
                            else if (subForm is LayeredFormCalendarTimeRange layered_time) layered_time.IClose();
                            StartFocused = false;
                            EndFocused = true;
                            StartEndFocused();
                            SetCaretPostion();
                        }
                    }
                    else
                    {
                        string etext;
                        if (index == -1) etext = text;
                        else etext = text.Substring(index + 1);
                        if (DateTime.TryParse(etext, out var date_e))
                        {
                            if (index == -1) Text = '\t' + date_e.ToString(Format);
                            else
                            {
                                string stext = text.Substring(0, index);
                                if (DateTime.TryParse(stext, out var date_s))
                                {
                                    Text = date_s.ToString(Format) + '\t' + date_e.ToString(Format);
                                    if (subForm is LayeredFormCalendarRange layered_range)
                                    {
                                        layered_range.Date = date_e;
                                        layered_range.SetDateE(date_s, date_e);
                                        layered_range.Print();
                                    }
                                    else if (subForm is LayeredFormCalendarTimeRange layered_time) layered_time.IClose();
                                }
                                else Text = text.Substring(0, index) + '\t' + date_e.ToString(Format);
                            }
                        }
                    }
                    return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        internal override bool IMouseDown(Point e)
        {
            if (rect_d_l.Contains(e) || rect_d_ico.Contains(e))
            {
                EndFocused = false;
                StartFocused = true;
                StartEndFocused();
            }
            else if (rect_d_r.Contains(e))
            {
                StartFocused = false;
                EndFocused = true;
                StartEndFocused();
            }
            return false;
        }

        #endregion

        #region 渲染

        internal override bool ModeRange { get => true; }
        internal override void ModeRangeCaretPostion(bool Null)
        {
            if (Null)
            {
                if (TextAlign == HorizontalAlignment.Center)
                {
                    if (StartFocused) SetCaretX(rect_d_l.X + rect_d_l.Width / 2);
                    else if (EndFocused) SetCaretX(rect_d_r.X + rect_d_r.Width / 2);
                }
                else if (TextAlign == HorizontalAlignment.Right)
                {
                    if (StartFocused) SetCaretX(rect_d_l.Right);
                    else if (EndFocused) SetCaretX(rect_d_r.Right);
                }
                else
                {
                    if (StartFocused) SetCaretX(rect_d_l.X);
                    else if (EndFocused) SetCaretX(rect_d_r.X);
                }
            }
            else
            {
                if (StartFocused)
                {
                    if (!rect_d_l.Contains(CurrentCaret)) ModeRangeCaretPostion(true);
                }
                else if (EndFocused)
                {
                    if (!rect_d_r.Contains(CurrentCaret)) ModeRangeCaretPostion(true);
                }
            }
        }
        internal override void PaintOtherBor(Graphics g, RectangleF rect_read, float radius, Color back, Color borderColor, Color borderActive)
        {
            if ((showS && placeholderS != null) || (showE && placeholderE != null))
            {
                using (var fore = new SolidBrush(Style.Db.TextQuaternary))
                {
                    if (showS && placeholderS != null) g.DrawStr(placeholderS, Font, fore, rect_d_l, sf_placeholder);
                    if (showE && placeholderE != null) g.DrawStr(placeholderE, Font, fore, rect_d_r, sf_placeholder);
                }
            }
            if (AnimationBar)
            {
                float h = rect_text.Height * 0.14F;
                var BarColor = BorderActive ?? Style.Db.Primary;
                using (var brush = new SolidBrush(BarColor))
                {
                    g.FillRectangle(brush, new RectangleF(AnimationBarValue.X, rect_read.Bottom - h, AnimationBarValue.Width, h));
                }
            }
            else if (StartFocused || EndFocused)
            {
                float h = rect_text.Height * 0.14F;
                var BarColor = BorderActive ?? Style.Db.Primary;
                using (var brush = new SolidBrush(BarColor))
                {
                    if (StartFocused) g.FillRectangle(brush, new RectangleF(rect_d_l.X, rect_read.Bottom - h, rect_d_l.Width, h));
                    else g.FillRectangle(brush, new RectangleF(rect_d_r.X, rect_read.Bottom - h, rect_d_r.Width, h));
                }
            }
            using (var bmp = SvgExtend.GetImgExtend(swapSvg ?? SvgDb.IcoSwap, rect_d_ico, Style.Db.TextQuaternary))
            {
                if (bmp == null) return;
                g.DrawImage(bmp, rect_d_ico);
            }
        }

        #endregion

        #region 指示条

        bool AnimationBar = false;
        RectangleF AnimationBarValue = RectangleF.Empty;
        ITask? ThreadBar = null;

        string StartEndFocusedTmp = "00";
        void StartEndFocused()
        {
            bool s = StartFocused, e = EndFocused;
            string temp = (s ? 1 : 0).ToString() + (e ? 1 : 0).ToString();
            if (StartEndFocusedTmp == temp) return;
            StartEndFocusedTmp = temp;

            if (Config.Animation && (s || e))
            {
                RectangleF NewValue;
                if (s) NewValue = rect_d_l;
                else NewValue = rect_d_r;
                if (AnimationBarValue == RectangleF.Empty) AnimationBarValue = new RectangleF(NewValue.X - 10, NewValue.Y, 0, NewValue.Height);
                float p_val = Math.Abs(NewValue.X - AnimationBarValue.X) * 0.09F;
                if (p_val > 0)
                {
                    float p_val2 = (NewValue.X - AnimationBarValue.X) * 0.5F, p_w_val = Math.Abs(NewValue.Width - AnimationBarValue.Width) * 0.1F;
                    AnimationBar = true;
                    bool left = NewValue.X > AnimationBarValue.X;
                    ThreadBar?.Dispose();
                    ThreadBar = new ITask(this, () =>
                    {
                        if (AnimationBarValue.Width != NewValue.Width)
                        {
                            AnimationBarValue.Width += p_w_val;
                            if (AnimationBarValue.Width > NewValue.Width) AnimationBarValue.Width = NewValue.Width;
                        }
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
                        if (subForm is LayeredFormCalendarRange layered)
                        {
                            if (Placement == TAlignFrom.TR || Placement == TAlignFrom.BR) layered.SetArrow(AnimationBarValue.X - rect_d_r.X);
                            else layered.SetArrow(AnimationBarValue.X);
                        }
                        Invalidate();
                        return true;
                    }, 10, () =>
                    {
                        if (subForm is LayeredFormCalendarRange layered)
                        {
                            if (Placement == TAlignFrom.TR || Placement == TAlignFrom.BR) layered.SetArrow(NewValue.X - rect_d_r.X);
                            else layered.SetArrow(NewValue.X);
                        }
                        AnimationBarValue = NewValue;
                        AnimationBar = false;
                        Invalidate();
                    });
                    return;
                }
            }

            if (s) AnimationBarValue = rect_d_l;
            else AnimationBarValue = rect_d_r;
        }

        bool StartFocused = false, EndFocused = false;

        protected override void Dispose(bool disposing)
        {
            ThreadBar?.Dispose();
            base.Dispose(disposing);
        }

        #endregion
    }
}