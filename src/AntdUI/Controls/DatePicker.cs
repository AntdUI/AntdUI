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

        /// <summary>
        /// 支持清除
        /// </summary>
        [Description("支持清除"), Category("行为"), DefaultValue(false)]
        public bool AllowClear { get; set; } = false;

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
            if (_value.HasValue)
            {
                Text = _value.Value.ToString(Format);
                textBox.Visible = true;
            }
            base.CreateHandle();
        }

        #endregion

        #region 事件

        public event DateTimeNEventHandler? ValueChanged;
        /// <summary>
        /// 预置点击时发生
        /// </summary>
        [Description("预置点击时发生"), Category("行为")]
        public event ObjectNEventHandler? PresetsClickChanged = null;

        #endregion

        #region 渲染

        internal override void PaintOther(Graphics g, RectangleF rect_read)
        {
            if (AllowClear && _mouseHover && _value.HasValue)
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

        internal override void RectTI()
        {
            GetRectTI(textBox.Height, HasImage, true);
        }

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
        internal override void TextGotFocus()
        {
            TextFocus = true;
        }
        internal override void TextLostFocus()
        {
            TextFocus = false;
        }

        #endregion

        #region 鼠标

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && _mouseHover && rect_icon_r.Contains(e.Location))
            {
                if (_value.HasValue && AllowClear)
                {
                    Value = null;
                    Invalidate();
                }
                else
                {
                    if (textBox.Focused) TextFocus = !textFocus;
                    else textBox.Focus();
                }
                return;
            }
            base.OnMouseClick(e);
        }

        bool hover_clear = false;
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (AllowClear && _mouseHover && _value.HasValue)
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

        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape && subForm != null)
            {
                subForm.IClose();
                return true;
            }
            else if (keyData == Keys.Enter && DateTime.TryParse(textBox.Text, out var _d))
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