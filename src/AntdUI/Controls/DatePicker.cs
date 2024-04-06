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
                _value = value;
                ValueChanged?.Invoke(this, value);
                Text = value.HasValue ? value.Value.ToString(Format) : "";
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
        [Description("下拉箭头是否显示"), Category("外观"), DefaultValue(false)]
        public bool DropDownArrow { get; set; } = false;

        protected override void CreateHandle()
        {
            if (_value.HasValue) Text = _value.Value.ToString(Format);
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

        string default_icon = "<svg viewBox=\"64 64 896 896\"><path d=\"M880 184H712v-64c0-4.4-3.6-8-8-8h-56c-4.4 0-8 3.6-8 8v64H384v-64c0-4.4-3.6-8-8-8h-56c-4.4 0-8 3.6-8 8v64H144c-17.7 0-32 14.3-32 32v664c0 17.7 14.3 32 32 32h736c17.7 0 32-14.3 32-32V216c0-17.7-14.3-32-32-32zm-40 656H184V460h656v380zM184 392V256h128v48c0 4.4 3.6 8 8 8h56c4.4 0 8-3.6 8-8v-48h256v48c0 4.4 3.6 8 8 8h56c4.4 0 8-3.6 8-8v-48h128v136H184z\"></path></svg>";
        internal override void PaintRIcon(Graphics g, Rectangle rect_r)
        {
            if (showicon)
            {
                using (var bmp = default_icon.SvgToBmp(rect_r.Width, rect_r.Height, Style.Db.TextQuaternary))
                {
                    if (bmp == null) return;
                    g.DrawImage(bmp, rect_r);
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

        #region 动画

        ILayeredForm? subForm = null;
        public ILayeredForm? SubForm() { return subForm; }

        bool textFocus = false;
        bool TextFocus
        {
            get => textFocus;
            set
            {
                if (textFocus == value) return;
                textFocus = value;
                if (value)
                {
                    if (subForm == null)
                    {
                        subForm = new LayeredFormCalendar(this, ReadRectangle, _value, date =>
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
            base.OnLostFocus(e);
        }

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
                if (Focused)
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
            else if (keyData == Keys.Enter && DateTime.TryParse(Text, out var _d))
            {
                Value = _d;
                if (subForm is LayeredFormCalendar _SubForm)
                {
                    _SubForm.SelDate = _SubForm.Date = _d;
                    _SubForm.Print();
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
        public DateBadge(string date, int count)
        {
            Date = date;
            Count = count;
        }
        public DateBadge(string date, int count, Color fill)
        {
            Date = date;
            Count = count;
            Fill = fill;
        }
        /// <summary>
        /// 日期 yyyy-MM-dd
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// 徽标计数 0是点
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 填充颜色
        /// </summary>
        public Color? Fill { get; set; }
    }
}