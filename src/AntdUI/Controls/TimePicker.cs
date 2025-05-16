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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// TimePicker 时间选择框
    /// </summary>
    /// <remarks>输入或选择时间的控件。</remarks>
    [Description("TimePicker 时间选择框")]
    [ToolboxItem(true)]
    [DefaultProperty("Value")]
    [DefaultEvent("ValueChanged")]
    public class TimePicker : Input, SubLayeredForm
    {
        #region 属性

        /// <summary>
        /// 格式化
        /// </summary>
        [Description("格式化"), Category("行为"), DefaultValue("HH:mm:ss")]
        public string Format { get; set; } = "HH:mm:ss";

        TimeSpan _value = new TimeSpan(0, 0, 0);
        /// <summary>
        /// 控件当前日期
        /// </summary>
        [Description("控件当前日期"), Category("数据"), DefaultValue(typeof(TimeSpan), "00:00:00")]
        public TimeSpan Value
        {
            get => _value;
            set
            {
                _value = value;
                Text = new DateTime(1997, 1, 1, value.Hours, value.Minutes, value.Seconds).ToString(Format);
                ValueChanged?.Invoke(this, new TimeSpanNEventArgs(value));
                OnPropertyChanged(nameof(Value));
            }
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

        /// <summary>
        /// 显示此刻
        /// </summary>
        [Description("显示此刻"), Category("外观"), DefaultValue(true)]
        public bool ShowButtonNow { get; set; } = true;

        protected override void OnHandleCreated(EventArgs e)
        {
            Text = new DateTime(1997, 1, 1, _value.Hours, _value.Minutes, _value.Seconds).ToString(Format);
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
                using (var bmp = SvgDb.IcoTime.SvgToBmp(rect_r.Width, rect_r.Height, Colour.TextQuaternary.Get("TimePicker", ColorScheme)))
                {
                    if (bmp == null) return;
                    g.Image(bmp, rect_r);
                }
            }
        }

        #endregion

        #endregion

        #region 事件

        public event TimeSpanNEventHandler? ValueChanged;

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
                        subForm = new LayeredFormCalendarTime(this, ReadRectangle, _value, date =>
                        {
                            Value = date;
                        });
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
                    Value = new TimeSpan(0, 0, 0);
                    return;
                }
                if (DateTime.TryParse("1997-1-1 " + Text, out var _d)) Value = new TimeSpan(_d.Hour, _d.Minute, _d.Second);
                Text = new DateTime(1997, 1, 1, _value.Hours, _value.Minutes, _value.Seconds).ToString(Format);
            }
        }

        #region 动画

        ILayeredForm? subForm = null;
        public ILayeredForm? SubForm() => subForm;

        #endregion

        #endregion

        #region 鼠标

        protected override void OnClearValue()
        {
            Value = new TimeSpan(0, 0, 0);
        }

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
            else if (keyData == Keys.Enter && DateTime.TryParse("1997-1-1 " + Text, out var _d))
            {
                Value = new TimeSpan(_d.Hour, _d.Minute, _d.Second);
                if (subForm is LayeredFormCalendarTime _SubForm)
                {
                    _SubForm.SelDate = Value;
                    _SubForm.Print();
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion
    }
}