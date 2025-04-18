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
using System.Globalization;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// DatePicker 日期选择框
    /// </summary>
    /// <remarks>输入或选择日期的控件。</remarks>
    [Description("DatePicker 日期选择框")]
    [ToolboxItem(true)]
    [DefaultProperty("Value")]
    [DefaultEvent("ValueChanged")]
    public class DatePicker : Input, SubLayeredForm
    {
        #region 属性

        string dateFormat = "yyyy-MM-dd";
        internal bool ShowTime = false;

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
                Text = _value.HasValue ? _value.Value.ToString(dateFormat) : "";
                OnPropertyChanged(nameof(Format));
            }
        }

        DateTime? _value = null;
        /// <summary>
        /// 控件当前日期
        /// </summary>
        [Description("控件当前日期"), Category("数据"), DefaultValue(null)]
        public DateTime? Value
        {
            get => _value;
            set
            {
                if (_value == value) return;
                _value = value;
                ValueChanged?.Invoke(this, new DateTimeNEventArgs(value));
                SetText(value);
                OnPropertyChanged(nameof(Value));
            }
        }

        void SetText(DateTime? value) => Text = value.HasValue ? value.Value.ToString(Format) : "";

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
        /// 时间值水平对齐
        /// </summary>
        [Description("时间值水平对齐"), Category("外观"), DefaultValue(false)]
        public bool ValueTimeHorizontal { get; set; }

        /// <summary>
        /// 下拉箭头是否显示
        /// </summary>
        [Description("下拉箭头是否显示"), Category("外观"), DefaultValue(false)]
        public bool DropDownArrow { get; set; }

        public TDatePicker picker = TDatePicker.Date;
        /// <summary>
        /// 设置选择器类型
        /// </summary>
        [Description("设置选择器类型"), Category("外观"), DefaultValue(TDatePicker.Date)]
        public TDatePicker Picker
        {
            get => picker;
            set
            {
                if (picker == value) return;
                picker = value;
                if (picker == TDatePicker.Month) Format = "yyyy-MM";
                else if (picker == TDatePicker.Year) Format = "yyyy";
                else Format = "yyyy-MM-dd";
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            SetText(_value);
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

        public override bool HasSuffix => showicon;

        protected override void PaintRIcon(Canvas g, Rectangle rect_r)
        {
            if (showicon)
            {
                using (var bmp = SvgDb.IcoDate.SvgToBmp(rect_r.Width, rect_r.Height, Colour.TextQuaternary.Get("DatePicker", ColorScheme)))
                {
                    if (bmp == null) return;
                    g.Image(bmp, rect_r);
                }
            }
        }

        #endregion

        #endregion

        #region 事件

        public event DateTimeNEventHandler? ValueChanged;
        /// <summary>
        /// 预置点击时发生
        /// </summary>
        [Description("预置点击时发生"), Category("行为")]
        public event ObjectNEventHandler? PresetsClickChanged = null;

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
                        subForm = new LayeredFormCalendar(this, ReadRectangle, _value, Picker, date =>
                        {
                            Value = date;
                        }, btn =>
                        {
                            PresetsClickChanged?.Invoke(this, new ObjectNEventArgs(btn));
                        }, BadgeAction);
                        subForm.Disposed += (a, b) =>
                        {
                            subForm = null;
                            ExpandDrop = false;
                        };
                        subForm.Show(this);
                    }
                }
                else subForm?.IClose();
            }
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            ExpandDrop = false;
            if (IsHandleCreated)
            {
                if (IsTextEmpty)
                {
                    Value = null;
                    return;
                }
                if (DateTime.TryParseExact(Text, Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var _d))
                {
                    Value = _d;
                    if (subForm != null)
                    {
                        subForm.SelDate = subForm.Date = _d;
                        subForm.Print();
                    }
                }
                SetText(_value);
            }
        }

        #region 动画

        LayeredFormCalendar? subForm = null;
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
            else if (keyData == Keys.Enter && DateTime.TryParse(Text, out var _d))
            {
                Value = _d;
                if (subForm != null)
                {
                    subForm.SelDate = subForm.Date = _d;
                    subForm.Print();
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion
    }

    /// <summary>
    /// 日期上徽标
    /// </summary>
    public class DateBadge
    {
        public DateBadge(string date)
        {
            Date = date;
        }
        public DateBadge(string date, Color fill)
        {
            Date = date;
            Fill = fill;
        }
        public DateBadge(string date, string content)
        {
            Date = date;
            Content = content;
        }
        public DateBadge(string date, int count)
        {
            Round = true;
            Date = date;
            if (count > 0)
            {
                if (count == 999) Content = "999";
                else if (count > 1000) Content = (count / 1000).ToString().Substring(0, 1) + "K+";
                else if (count > 99) Content = "99+";
                else Content = count.ToString();
            }
        }
        public DateBadge(string date, int count, Color fill) : this(date, count)
        {
            Fill = fill;
        }
        public DateBadge(string date, string content, Color fill) : this(date, content)
        {
            Fill = fill;
        }
        /// <summary>
        /// 日期 yyyy-MM-dd
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// 徽标内容 空字符串是点
        /// </summary>
        public string? Content { get; set; }

        /// <summary>
        /// 填充颜色
        /// </summary>
        public Color? Fill { get; set; }

        public bool Round { get; set; }

        public int Radius { get; set; } = 6;

        /// <summary>
        /// 徽标方向
        /// </summary>
        public TAlignFrom Align { get; set; } = TAlignFrom.TR;

        /// <summary>
        /// 徽标比例
        /// </summary>
        public float Size { get; set; } = .6F;

        /// <summary>
        /// 徽标偏移X
        /// </summary>
        public int OffsetX { get; set; } = 2;

        /// <summary>
        /// 徽标偏移Y
        /// </summary>
        public int OffsetY { get; set; } = 2;
    }
}