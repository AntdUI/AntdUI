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
    /// <seealso cref="Input"/>
    /// <remarks>输入或选择日期范围的控件。</remarks>
    [Description("DatePickerRange 日期范围选择框")]
    [ToolboxItem(true)]
    [DefaultProperty("Value")]
    [DefaultEvent("ValueChanged")]
    public class DatePickerRange : Input, SubLayeredForm
    {
        #region 属性

        bool showS = true, showE = true;
        string? placeholderS, placeholderE;
        /// <summary>
        /// 显示的水印文本
        /// </summary>
        [Description("显示的水印文本S"), Category("行为"), DefaultValue(null)]
        [Localizable(true)]
        public string? PlaceholderStart
        {
            get => this.GetLangI(LocalizationPlaceholderStart, placeholderS);
            set
            {
                if (placeholderS == value) return;
                placeholderS = value;
                Invalidate();
            }
        }

        [Description("显示的水印文本S"), Category("国际化"), DefaultValue(null)]
        public string? LocalizationPlaceholderStart { get; set; }

        /// <summary>
        /// 显示的水印文本
        /// </summary>
        [Description("显示的水印文本E"), Category("行为"), DefaultValue(null)]
        [Localizable(true)]
        public string? PlaceholderEnd
        {
            get => this.GetLangI(LocalizationPlaceholderEnd, placeholderE);
            set
            {
                if (placeholderE == value) return;
                placeholderE = value;
                Invalidate();
            }
        }

        [Description("显示的水印文本E"), Category("国际化"), DefaultValue(null)]
        public string? LocalizationPlaceholderEnd { get; set; }

        /// <summary>
        /// 水印文本
        /// </summary>
        [Browsable(false), Description("水印文本"), Category("行为"), DefaultValue(null)]
        public override string? PlaceholderText => null;

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
                if (_value == null) Text = "";
                else Text = _value[0].ToString(dateFormat) + "\t" + _value[1].ToString(dateFormat);
            }
        }

        DateTime[]? _value;
        [Description("控件当前日期"), Category("数据"), DefaultValue(null)]
        public DateTime[]? Value
        {
            get => _value;
            set
            {
                if (Helper.AreDateTimeArraysEqual(_value, value)) return;
                _value = value;
                ValueChanged?.Invoke(this, new DateTimesEventArgs(value));
                SetText(value);
                OnPropertyChanged(nameof(Value));
            }
        }

        void SetText(DateTime[]? value)
        {
            if (value == null) Text = "";
            else Text = value[0].ToString(Format) + "\t" + value[1].ToString(Format);
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

        /// <summary>
        /// 时间值水平对齐
        /// </summary>
        [Description("时间值水平对齐"), Category("外观"), DefaultValue(false)]
        public bool ValueTimeHorizontal { get; set; }

        /// <summary>
        /// 交互重置（是否每次都开始时间选择）
        /// </summary>
        [Description("交互重置（是否每次都开始时间选择）"), Category("外观"), DefaultValue(true)]
        public bool InteractiveReset { get; set; } = true;

        /// <summary>
        /// 选择器类型
        /// </summary>
        [Description("选择器类型"), Category("外观"), DefaultValue(TDatePicker.Date)]
        public TDatePicker Picker { get; set; } = TDatePicker.Date;

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

        string? swapSvg;
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
        public Func<DateTime[], List<DateBadge>?>? BadgeAction;

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

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            SetText(_value);
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

        protected override void PaintRIcon(Canvas g, Rectangle rect_r)
        {
            if (showicon)
            {
                using (var bmp = SvgDb.IcoDate.SvgToBmp(rect_r.Width, rect_r.Height, Colour.TextQuaternary.Get(nameof(DatePicker), ColorScheme)))
                {
                    if (bmp == null) return;
                    g.Image(bmp, rect_r);
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
        public event ObjectNEventHandler? PresetsClickChanged;

        #endregion

        #region 焦点

        bool expandDrop = false;
        /// <summary>
        /// 展开下拉菜单
        /// </summary>
        [Browsable(false)]
        [Description("展开下拉菜单"), Category("行为"), DefaultValue(false)]
        public bool ExpandDrop
        {
            get => expandDrop;
            set
            {
                if (expandDrop == value) return;
                expandDrop = value;
                if (!ReadOnly && value)
                {
                    if (subForm == null)
                    {
                        int bar = 0;
                        if (EndFocused) bar = rect_d_r.X;
                        else bar = rect_d_l.X;
                        try
                        {
                            if (ShowTime)
                            {
                                subForm = new LayeredFormDatePickerRangeTime(this, EndFocused, bar, date => Value = date, btn => PresetsClickChanged?.Invoke(this, new ObjectNEventArgs(btn)), BadgeAction);
                                subForm.Disposed += (a, b) =>
                                {
                                    subForm = null;
                                    ExpandDrop = false;
                                };
                                subForm.Show(this);
                            }
                            else
                            {
                                subForm = new LayeredFormDatePickerRange(this, EndFocused, bar, date => Value = date, btn => PresetsClickChanged?.Invoke(this, new ObjectNEventArgs(btn)), BadgeAction);
                                subForm.Disposed += (a, b) =>
                                {
                                    subForm = null;
                                    ExpandDrop = false;
                                };
                                subForm.Show(this);
                            }
                        }
                        catch
                        {
                            subForm = null;
                        }
                    }
                }
                else subForm?.IClose();
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            if (!StartFocused && !EndFocused) StartFocused = true;
            StartEndFocused();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            ILostFocus();
            base.OnLostFocus(e);
        }

        void ILostFocus()
        {
            ExpandDrop = StartFocused = EndFocused = false;
            StartEndFocused();
            AnimationBarValue = RectangleF.Empty;
            if (IsHandleCreated)
            {
                if (IsTextEmpty)
                {
                    Value = null;
                    return;
                }
                string text = Text;
                int index = text.IndexOf("\t");
                if (index > 0)
                {
                    string stext = text.Substring(0, index), etext = text.Substring(index + 1);
                    if (DateTime.TryParse(stext, out var date_s) && DateTime.TryParse(etext, out var date_e)) Value = new DateTime[] { date_s, date_e };
                    SetText(_value);
                }
            }
        }

        #region 动画

        ILayeredForm? subForm;
        public ILayeredForm? SubForm() => subForm;

        #endregion

        #endregion

        #region 鼠标

        protected override void OnClearValue() => Value = null;

        protected override void OnClickContent()
        {
            if (HasFocus)
            {
                if (expandDrop) return;
                ExpandDrop = !expandDrop;
            }
            else Focus();
        }

        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape && subForm != null) subForm.IClose();
            else if (keyData == Keys.Down && subForm == null) ExpandDrop = true;
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
                            if (subForm is LayeredFormDatePickerRange layered_range) layered_range.SetDateS(date_s);
                            else if (subForm is LayeredFormDatePickerRangeTime layered_time) layered_time.IClose();
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
                                    if (subForm is LayeredFormDatePickerRange layered_range)
                                    {
                                        layered_range.SetDateE(date_s, date_e);
                                    }
                                    else if (subForm is LayeredFormDatePickerRangeTime layered_time) layered_time.IClose();
                                }
                                else Text = text.Substring(0, index) + '\t' + date_e.ToString(Format);
                            }
                        }
                    }
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override bool IMouseDown(int x, int y)
        {
            if (rect_d_l.Contains(x, y) || rect_d_ico.Contains(x, y))
            {
                EndFocused = false;
                StartFocused = true;
                StartEndFocused();
            }
            else if (rect_d_r.Contains(x, y))
            {
                StartFocused = false;
                EndFocused = true;
                StartEndFocused();
            }
            return false;
        }

        #endregion

        #region 渲染

        protected override bool ModeRange => true;
        protected override void ModeRangeCaretPostion(bool Null)
        {
            if (Null)
            {
                if (TextAlign == HorizontalAlignment.Center)
                {
                    if (StartFocused) CaretInfo.X = rect_d_l.X + rect_d_l.Width / 2;
                    else if (EndFocused) CaretInfo.X = rect_d_r.X + rect_d_r.Width / 2;
                }
                else if (TextAlign == HorizontalAlignment.Right)
                {
                    if (StartFocused) CaretInfo.X = rect_d_l.Right;
                    else if (EndFocused) CaretInfo.X = rect_d_r.Right;
                }
                else
                {
                    if (StartFocused) CaretInfo.X = rect_d_l.X;
                    else if (EndFocused) CaretInfo.X = rect_d_r.X;
                }
            }
            else
            {
                if (StartFocused)
                {
                    if (!rect_d_l.Contains(CaretInfo.Rect)) ModeRangeCaretPostion(true);
                }
                else if (EndFocused)
                {
                    if (!rect_d_r.Contains(CaretInfo.Rect)) ModeRangeCaretPostion(true);
                }
            }
        }
        protected override void PaintOtherBor(Canvas g, Rectangle rect_read, float radius, Color back, Color borderColor, Color borderActive)
        {
            string? placeholderS = PlaceholderStart, placeholderE = PlaceholderEnd;
            if ((showS && placeholderS != null) || (showE && placeholderE != null))
            {
                using (var fore = new SolidBrush(Colour.TextQuaternary.Get(nameof(DatePicker), ColorScheme)))
                {
                    if (showS && placeholderS != null) g.String(placeholderS, Font, fore, rect_d_l, sf_placeholder);
                    if (showE && placeholderE != null) g.String(placeholderE, Font, fore, rect_d_r, sf_placeholder);
                }
            }
            if (AnimationBar)
            {
                float h = rect_text.Height * 0.14F;
                var BarColor = BorderActive ?? Colour.Primary.Get(nameof(DatePicker), ColorScheme);
                g.Fill(BarColor, new RectangleF(AnimationBarValue.X, rect_read.Bottom - h, AnimationBarValue.Width, h));
            }
            else if (StartFocused || EndFocused)
            {
                float h = rect_text.Height * 0.14F;
                var BarColor = BorderActive ?? Colour.Primary.Get(nameof(DatePicker), ColorScheme);
                using (var brush = new SolidBrush(BarColor))
                {
                    if (StartFocused) g.Fill(brush, new RectangleF(rect_d_l.X, rect_read.Bottom - h, rect_d_l.Width, h));
                    else g.Fill(brush, new RectangleF(rect_d_r.X, rect_read.Bottom - h, rect_d_r.Width, h));
                }
            }
            g.GetImgExtend(swapSvg ?? SvgDb.IcoSwap, rect_d_ico, Colour.TextQuaternary.Get(nameof(DatePicker), ColorScheme));
        }

        #endregion

        #region 指示条

        bool AnimationBar = false;
        RectangleF AnimationBarValue = RectangleF.Empty;
        ITask? ThreadBar;

        string StartEndFocusedTmp = "00";
        void StartEndFocused()
        {
            bool s = StartFocused, e = EndFocused;
            string temp = (s ? 1 : 0).ToString() + (e ? 1 : 0).ToString();
            if (StartEndFocusedTmp == temp) return;
            StartEndFocusedTmp = temp;

            if (Config.HasAnimation(nameof(DatePicker)) && (s || e))
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
                        if (subForm is LayeredFormDatePickerRange layered)
                        {
                            if (Placement == TAlignFrom.TR || Placement == TAlignFrom.BR) layered.SetArrow(AnimationBarValue.X - rect_d_r.X);
                            else layered.SetArrow(AnimationBarValue.X);
                        }
                        else if (subForm is LayeredFormDatePickerRangeTime layeredt)
                        {
                            if (Placement == TAlignFrom.TR || Placement == TAlignFrom.BR) layeredt.SetArrow(AnimationBarValue.X - rect_d_r.X);
                            else layeredt.SetArrow(AnimationBarValue.X);
                        }
                        Invalidate();
                        return true;
                    }, 10, () =>
                    {
                        if (subForm is LayeredFormDatePickerRange layered)
                        {
                            if (Placement == TAlignFrom.TR || Placement == TAlignFrom.BR) layered.SetArrow(NewValue.X - rect_d_r.X);
                            else layered.SetArrow(NewValue.X);
                        }
                        else if (subForm is LayeredFormDatePickerRangeTime layeredt)
                        {
                            if (Placement == TAlignFrom.TR || Placement == TAlignFrom.BR) layeredt.SetArrow(NewValue.X - rect_d_r.X);
                            else layeredt.SetArrow(NewValue.X);
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