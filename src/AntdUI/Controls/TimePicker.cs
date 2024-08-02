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
                ValueChanged?.Invoke(this, value);
            }
        }

        /// <summary>
        /// 菜单弹出位置
        /// </summary>
        [Description("菜单弹出位置"), Category("行为"), DefaultValue(TAlignFrom.BL)]
        public TAlignFrom Placement { get; set; } = TAlignFrom.BL;

        /// <summary>
        /// 下拉箭头是否显示
        /// </summary>
        [Description("下拉箭头是否显示"), Category("外观"), DefaultValue(false)]
        public bool DropDownArrow { get; set; } = false;

        /// <summary>
        /// 焦点时展开下拉
        /// </summary>
        [Description("焦点时展开下拉"), Category("行为"), DefaultValue(true)]
        public bool FocusExpandDropdown { get; set; } = true;

        protected override void CreateHandle()
        {
            Text = new DateTime(1997, 1, 1, _value.Hours, _value.Minutes, _value.Seconds).ToString(Format);
            base.CreateHandle();
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
                using (var bmp = SvgDb.IcoTime.SvgToBmp(rect_r.Width, rect_r.Height, Style.Db.TextQuaternary))
                {
                    if (bmp == null) return;
                    g.DrawImage(bmp, rect_r);
                }
            }
        }

        #endregion

        #endregion

        #region 事件

        public event TimeSpanNEventHandler? ValueChanged;

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
                if (FocusExpandDropdown && !ReadOnly && value)
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
                            TextFocus = false;
                        };
                        subForm.Show(this);
                    }
                }
                else subForm?.IClose();
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            TextFocus = true;
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            TextFocus = false;
            if (IsHandleCreated)
            {
                if (DateTime.TryParse("1997-1-1 " + Text, out var _d)) Value = new TimeSpan(_d.Hour, _d.Minute, _d.Second);
                else Text = new DateTime(1997, 1, 1, _value.Hours, _value.Minutes, _value.Seconds).ToString(Format);
            }
            base.OnLostFocus(e);
        }

        #region 动画

        ILayeredForm? subForm = null;
        public ILayeredForm? SubForm() { return subForm; }

        #endregion

        #endregion

        #region 鼠标

        internal override void OnClearValue()
        {
            Value = new TimeSpan(0, 0, 0);
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
            //else if (keyData == Keys.Enter && DateTime.TryParse(Text, out var _d))
            //{
            //    Value = _d;
            //    if (subForm is LayeredFormCalendar _SubForm)
            //    {
            //        _SubForm.SelDate = _SubForm.Date = _d;
            //        _SubForm.Print();
            //    }
            //}
            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion
    }
}