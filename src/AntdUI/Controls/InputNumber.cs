﻿// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
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

using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// InputNumber 数字输入框
    /// </summary>
    /// <remarks>通过鼠标或键盘，输入范围内的数值。</remarks>
    [Description("InputNumber 数字输入框")]
    [ToolboxItem(true)]
    [DefaultProperty("Value")]
    [DefaultEvent("ValueChanged")]
    public class InputNumber : Input
    {
        #region 属性

        /// <summary>
        /// 支持清除
        /// </summary>
        [Browsable(false), Description("支持清除"), Category("行为"), DefaultValue(false)]
        public override bool AllowClear
        {
            get => false;
            set { base.AllowClear = false; }
        }

        decimal minimum = decimal.Zero;
        [Description("最小值"), Category("数据"), DefaultValue(typeof(decimal), "0")]
        public decimal Minimum
        {
            get => minimum;
            set
            {
                minimum = value;
                if (minimum > maximum) maximum = value;
                Value = Constrain(currentValue);
            }
        }

        decimal maximum = 100;
        [Description("最大值"), Category("数据"), DefaultValue(typeof(decimal), "100")]
        public decimal Maximum
        {
            get => maximum;
            set
            {
                maximum = value;
                if (minimum > maximum) minimum = maximum;
                Value = Constrain(currentValue);
            }
        }

        decimal Constrain(decimal value)
        {
            if (value < minimum) value = minimum;
            if (value > maximum) value = maximum;
            return value;
        }

        decimal currentValue = 0;
        [Description("当前值"), Category("数据"), DefaultValue(typeof(decimal), "0")]
        public decimal Value
        {
            get => currentValue;
            set
            {
                if (currentValue == value) return;
                currentValue = Constrain(value);
                Text = GetNumberText(currentValue);
                ValueChanged?.Invoke(this, currentValue);
            }
        }

        int decimalPlaces = 0;
        /// <summary>
        /// 显示的小数点位数
        /// </summary>
        [Description("显示的小数点位数"), Category("数据"), DefaultValue(0)]
        public int DecimalPlaces
        {
            get => decimalPlaces;
            set
            {
                if (decimalPlaces == value) return;
                decimalPlaces = value;
                Text = GetNumberText(currentValue);
            }
        }

        bool thousandsSeparator = false;
        /// <summary>
        /// 是否显示千分隔符
        /// </summary>
        [Description("是否显示千分隔符"), Category("数据"), DefaultValue(false)]
        public bool ThousandsSeparator
        {
            get => thousandsSeparator;
            set
            {
                if (thousandsSeparator == value) return;
                thousandsSeparator = value;
                Text = GetNumberText(currentValue);
            }
        }

        bool hexadecimal = false;
        /// <summary>
        /// 值是否应以十六进制显示
        /// </summary>
        [Description("值是否应以十六进制显示"), Category("数据"), DefaultValue(false)]
        public bool Hexadecimal
        {
            get => hexadecimal;
            set
            {
                if (hexadecimal == value) return;
                hexadecimal = value;
                Text = GetNumberText(currentValue);
            }
        }

        /// <summary>
        /// 当按下箭头键时，是否持续增加/减少
        /// </summary>
        [Description("当按下箭头键时，是否持续增加/减少"), Category("行为"), DefaultValue(true)]
        public bool InterceptArrowKeys { get; set; } = true;

        string GetNumberText(decimal num)
        {
            if (Hexadecimal) return ((long)num).ToString("X", CultureInfo.InvariantCulture);
            return num.ToString((ThousandsSeparator ? "N" : "F") + DecimalPlaces.ToString(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// 每次单击箭头键时增加/减少的数量
        /// </summary>
        [Description("每次单击箭头键时增加/减少的数量"), Category("数据"), DefaultValue(typeof(decimal), "1")]
        public decimal Increment { get; set; } = 1;

        protected override void CreateHandle()
        {
            Text = GetNumberText(currentValue);
            base.CreateHandle();
        }

        #endregion

        #region 事件

        /// <summary>
        /// Value 属性值更改时发生
        /// </summary>
        [Description("Value 属性值更改时发生"), Category("行为")]
        public event DecimalEventHandler? ValueChanged;

        #endregion

        #region 渲染

        ITaskOpacity hover_button, hover_button_up, hover_button_bottom;
        RectangleF rect_button, rect_button_up, rect_button_bottom;
        public InputNumber()
        {
            hover_button = new ITaskOpacity(this);
            hover_button_up = new ITaskOpacity(this);
            hover_button_bottom = new ITaskOpacity(this);
        }

        protected override void Dispose(bool disposing)
        {
            hover_button.Dispose();
            hover_button_up.Dispose();
            hover_button_bottom.Dispose();
            base.Dispose(disposing);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            var numberFormatInfo = CultureInfo.CurrentCulture.NumberFormat;
            string decimalSeparator = numberFormatInfo.NumberDecimalSeparator, groupSeparator = numberFormatInfo.NumberGroupSeparator, negativeSign = numberFormatInfo.NegativeSign;

            string keyInput = e.KeyChar.ToString();
            if (char.IsDigit(e.KeyChar))
            {
                // 数字可以
            }
            else if (keyInput.Equals(decimalSeparator) || keyInput.Equals(groupSeparator) || keyInput.Equals(negativeSign))
            {
                // 小数分隔符可以
            }
            else if (e.KeyChar == '\b')
            {
                // Backspace键可以
            }
            else if (Hexadecimal && ((e.KeyChar >= 'a' && e.KeyChar <= 'f') || (e.KeyChar >= 'A' && e.KeyChar <= 'F')))
            {
                // 十六进制数字可以
            }
            else
            {
                // 吃掉这个无效的钥匙
                e.Handled = true;
                return;
                //User32.MessageBeep(User32.MB.OK);//发出嘟嘟声
            }
            base.OnKeyPress(e);
        }

        protected override bool VerifyImeResultStr(string strResult)
        {
            var numberFormatInfo = CultureInfo.CurrentCulture.NumberFormat;
            string decimalSeparator = numberFormatInfo.NumberDecimalSeparator, groupSeparator = numberFormatInfo.NumberGroupSeparator, negativeSign = numberFormatInfo.NegativeSign;

            foreach (char key in strResult)
            {
                string keyInput = key.ToString();
                if (char.IsDigit(key))
                {
                    // 数字可以
                }
                else if (keyInput.Equals(decimalSeparator) || keyInput.Equals(groupSeparator) || keyInput.Equals(negativeSign))
                {
                    // 小数分隔符可以
                }
                else if (key == '\b')
                {
                    // Backspace键可以
                }
                else if (Hexadecimal && ((key >= 'a' && key <= 'f') || (key >= 'A' && key <= 'F')))
                {
                    // 十六进制数字可以
                }
                else return false;
            }
            return true;
        }

        internal override void PaintOtherBor(Graphics g, RectangleF rect_read, float _radius, Color back, Color borColor, Color borderActive)
        {
            if (hover_button.Animation || hover_button.Switch)
            {
                float radius = round ? rect_read.Height / 2F : _radius;
                int width = (int)(22 * Config.Dpi);
                rect_button = new RectangleF(rect_read.Right - width, rect_read.Y, width, rect_read.Height);
                rect_button_up = new RectangleF(rect_button.X, rect_button.Y, rect_button.Width, rect_button.Height / 2);
                rect_button_bottom = new RectangleF(rect_button.X, rect_button_up.Bottom, rect_button.Width, rect_button_up.Height);

                using (var path = rect_button.RoundPath(radius, false, true, true, false))
                {
                    using (var brush = new SolidBrush(back))
                    { g.FillPath(brush, path); }
                }

                if (hover_button.Animation)
                {
                    using (var pen = new Pen(Color.FromArgb(hover_button.Value, borColor), 1 * Config.Dpi))
                    {
                        using (var path = rect_button_up.RoundPath(radius, false, true, false, false))
                        {
                            g.DrawPath(pen, path);
                        }
                        using (var path = rect_button_bottom.RoundPath(radius, false, false, true, false))
                        {
                            g.DrawPath(pen, path);
                        }
                    }
                }
                else if (hover_button.Switch)
                {
                    using (var pen = new Pen(borColor, 1 * Config.Dpi))
                    {
                        using (var path = rect_button_up.RoundPath(radius, false, true, false, false))
                        {
                            g.DrawPath(pen, path);
                        }
                        using (var path = rect_button_bottom.RoundPath(radius, false, false, true, false))
                        {
                            g.DrawPath(pen, path);
                        }
                    }
                }

                if (hover_button_up.Animation)
                {
                    using (var pen_def = new Pen(borColor, 1 * Config.Dpi))
                    {
                        g.DrawLines(pen_def, TAlignMini.Top.TriangleLines(rect_button_up));
                        using (var brush_hove = new Pen(Color.FromArgb(hover_button_up.Value, borderActive), pen_def.Width))
                        {
                            g.DrawLines(brush_hove, TAlignMini.Top.TriangleLines(rect_button_up));
                        }
                    }
                }
                else if (hover_button_up.Switch)
                {
                    using (var pen = new Pen(borderActive, 1 * Config.Dpi))
                    {
                        g.DrawLines(pen, TAlignMini.Top.TriangleLines(rect_button_up));
                    }
                }
                else
                {
                    using (var pen_def = new Pen(borColor, 1 * Config.Dpi))
                    {
                        g.DrawLines(pen_def, TAlignMini.Top.TriangleLines(rect_button_up));
                    }
                }

                if (hover_button_bottom.Animation)
                {
                    using (var pen_def = new Pen(borColor, 1 * Config.Dpi))
                    {
                        g.DrawLines(pen_def, TAlignMini.Bottom.TriangleLines(rect_button_bottom));
                        using (var brush_hove = new Pen(Color.FromArgb(hover_button_bottom.Value, borderActive), pen_def.Width))
                        {
                            g.DrawLines(brush_hove, TAlignMini.Bottom.TriangleLines(rect_button_bottom));
                        }
                    }
                }
                else if (hover_button_bottom.Switch)
                {
                    using (var pen = new Pen(borderActive, 1 * Config.Dpi))
                    {
                        g.DrawLines(pen, TAlignMini.Bottom.TriangleLines(rect_button_bottom));
                    }
                }
                else
                {
                    using (var pen_def = new Pen(borColor, 1 * Config.Dpi))
                    {
                        g.DrawLines(pen_def, TAlignMini.Bottom.TriangleLines(rect_button_bottom));
                    }
                }
            }
        }

        #endregion

        #region 鼠标

        internal override void ChangeMouseHover(bool Hover, bool Focus)
        {
            hover_button.Switch = Hover || Focus;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (rect_button.Contains(e.Location))
            {
                if (rect_button_up.Contains(e.Location))
                {
                    hover_button_bottom.Switch = false;
                    hover_button_up.Switch = true;
                }
                else
                {
                    hover_button_up.Switch = false;
                    hover_button_bottom.Switch = true;
                }
                SetCursor(true);
                return;
            }
            else
            {
                hover_button_up.Switch = hover_button_bottom.Switch = false;
                SetCursor(false);
            }
            base.OnMouseMove(e);
        }

        bool isdownup = false, isdowndown = false;
        int downid = 0, temp_old = 0;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (rect_button.Contains(e.Location))
            {
                if (rect_button_up.Contains(e.Location))
                {
                    Value = currentValue + Increment;
                    isdownup = true;
                }
                else
                {
                    Value = currentValue - Increment;
                    isdowndown = true;
                }
                if ((isdownup || isdowndown) && InterceptArrowKeys)
                {
                    int _downid = downid = temp_old;
                    temp_old++;
                    if (temp_old > 9999) temp_old = 0;
                    ITask.Run(() =>
                    {
                        System.Threading.Thread.Sleep(500);
                        while (isdownup || isdowndown && _downid == downid)
                        {
                            var old = currentValue;
                            if (isdownup) Value = currentValue + Increment;
                            else if (isdowndown) Value = currentValue - Increment;
                            if (old == currentValue) return;
                            System.Threading.Thread.Sleep(200);
                        }
                    });
                }
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            isdownup = isdowndown = false;
        }

        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                if (decimal.TryParse(Text, out var _d)) Value = _d;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion
    }
}