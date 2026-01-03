// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// TimePicker 时间选择框
    /// </summary>
    /// <seealso cref="Input"/>
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
                OnValueChanged(value);
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

        /// <summary>
        /// 文本改变时是否更新Value值
        /// </summary>
        [Description("文本改变时是否更新Value值"), Category("行为"), DefaultValue(false)]
        public bool EnabledValueTextChange { get; set; }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            Text = new DateTime(1997, 1, 1, _value.Hours, _value.Minutes, _value.Seconds).ToString(Format);
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
                using (var bmp = SvgDb.IcoTime.SvgToBmp(rect_r.Width, rect_r.Height, Colour.TextQuaternary.Get(nameof(TimePicker), ColorScheme)))
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

        protected virtual void OnValueChanged(TimeSpan e) => ValueChanged?.Invoke(this, new TimeSpanNEventArgs(e));

        /// <summary>
        /// 下拉展开 属性值更改时发生
        /// </summary>
        [Description("下拉展开 属性值更改时发生"), Category("行为")]
        public event BoolEventHandler? ExpandDropChanged;

        protected virtual void OnExpandDropChanged(bool e) => ExpandDropChanged?.Invoke(this, new BoolEventArgs(e));

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
                        try
                        {
                            subForm = new LayeredFormTimePicker(this, _value, date => Value = date);
                            subForm.Disposed += (a, b) =>
                            {
                                subForm = null;
                                ExpandDrop = false;
                            };
                            subForm.Show(this);
                        }
                        catch
                        {
                            subForm = null;
                        }
                    }
                }
                else subForm?.IClose();
                OnExpandDropChanged(value);
            }
        }

        protected override void OnLostFocus(EventArgs e)
        {
            ILostFocus();
            base.OnLostFocus(e);
        }

        void ILostFocus()
        {
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

        ILayeredForm? subForm;
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
            else if (keyData == Keys.Enter && DateTime.TryParse("1997-1-1 " + Text, out var _d)) PValue(_d);
            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void OnSetText(string text, bool isempty)
        {
            if (EnabledValueTextChange && !isempty)
            {
                if (DateTime.TryParseExact(text, Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var _d)) PValue(_d);
            }
        }

        void PValue(DateTime value)
        {
            Value = new TimeSpan(value.Hour, value.Minute, value.Second);
            if (subForm is LayeredFormTimePicker _SubForm)
            {
                _SubForm.SelDate = Value;
                _SubForm.Print();
            }
        }

        #endregion
    }
}