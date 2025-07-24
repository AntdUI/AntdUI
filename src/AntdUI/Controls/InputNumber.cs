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
        public new bool AllowClear
        {
            get => false;
            set => base.AllowClear = false;
        }

        decimal? minimum, maximum;
        [Description("最小值"), Category("数据"), DefaultValue(null)]
        public decimal? Minimum
        {
            get => minimum;
            set
            {
                minimum = value;
                if (minimum.HasValue && maximum.HasValue && minimum.Value > maximum.Value) maximum = minimum.Value;
                Value = Constrain(currentValue);
            }
        }

        [Description("最大值"), Category("数据"), DefaultValue(null)]
        public decimal? Maximum
        {
            get => maximum;
            set
            {
                maximum = value;
                if (minimum.HasValue && maximum.HasValue && minimum.Value > maximum.Value) minimum = maximum.Value;
                Value = Constrain(currentValue);
            }
        }

        decimal Constrain(decimal value)
        {
            if (minimum.HasValue && value < minimum.Value) value = minimum.Value;
            if (maximum.HasValue && value > maximum.Value) value = maximum.Value;
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
                ValueChanged?.Invoke(this, new DecimalEventArgs(currentValue));
                OnPropertyChanged(nameof(Value));
            }
        }

        bool showcontrol = true;
        /// <summary>
        /// 显示控制器
        /// </summary>
        [Description("显示控制器"), Category("交互"), DefaultValue(true)]
        public bool ShowControl
        {
            get => showcontrol;
            set
            {
                if (showcontrol == value) return;
                showcontrol = value;
                Invalidate();
            }
        }

        bool wheelModifyEnabled = true;
        /// <summary>
        /// 鼠标滚轮修改值
        /// </summary>
        [Description("鼠标滚轮修改值"), Category("交互"), DefaultValue(true)]
        public bool WheelModifyEnabled
        {
            get => wheelModifyEnabled;
            set
            {
                if (wheelModifyEnabled == value) return;
                wheelModifyEnabled = value;
                Invalidate();
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

        protected override void OnHandleCreated(EventArgs e)
        {
            Text = GetNumberText(currentValue);
            base.OnHandleCreated(e);
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
        Rectangle rect_button, rect_button_up, rect_button_bottom;
        public InputNumber()
        {
            var key = nameof(InputNumber);
            hover_button = new ITaskOpacity(key, this);
            hover_button_up = new ITaskOpacity(key, this);
            hover_button_bottom = new ITaskOpacity(key, this);
        }

        protected override void Dispose(bool disposing)
        {
            hover_button.Dispose();
            hover_button_up.Dispose();
            hover_button_bottom.Dispose();
            base.Dispose(disposing);
        }

        static NumberFormatInfo numberFormatInfo = CultureInfo.CurrentCulture.NumberFormat;
        static string decimalSeparator = numberFormatInfo.NumberDecimalSeparator, groupSeparator = numberFormatInfo.NumberGroupSeparator, negativeSign = numberFormatInfo.NegativeSign;

        protected override bool Verify(char key, out string? change)
        {
            change = null;
            string keyInput = key.ToString();
            if (char.IsDigit(key)) return true; // 数字可以
            else if (keyInput.Equals(decimalSeparator) || keyInput.Equals(groupSeparator) || keyInput.Equals(negativeSign)) return true;// 小数分隔符可以
            else if (key == '\b') return true;// Backspace键可以
            else if (Hexadecimal && ((key >= 'a' && key <= 'f') || (key >= 'A' && key <= 'F'))) return true;// 十六进制数字可以
            return false;
        }

        protected override void PaintOtherBor(Canvas g, Rectangle rect_read, float _radius, Color back, Color borColor, Color borderActive)
        {
            if (hover_button.Animation || hover_button.Switch)
            {
                float radius = round ? rect_read.Height / 2F : _radius;
                int width = (int)(22 * Config.Dpi);
                rect_button = new Rectangle(rect_read.Right - width, rect_read.Y, width, rect_read.Height);
                rect_button_up = new Rectangle(rect_button.X, rect_button.Y, rect_button.Width, rect_button.Height / 2);
                rect_button_bottom = new Rectangle(rect_button.X, rect_button_up.Bottom, rect_button.Width, rect_button_up.Height);

                using (var path = rect_button.RoundPath(radius, false, true, true, false))
                {
                    g.Fill(back, path);
                }

                if (hover_button.Animation)
                {
                    using (var pen = new Pen(Helper.ToColor(hover_button.Value, borColor), Config.Dpi))
                    {
                        using (var path = rect_button_up.RoundPath(radius, false, true, false, false))
                        {
                            g.Draw(pen, path);
                        }
                        using (var path = rect_button_bottom.RoundPath(radius, false, false, true, false))
                        {
                            g.Draw(pen, path);
                        }
                    }
                }
                else if (hover_button.Switch)
                {
                    using (var pen = new Pen(borColor, Config.Dpi))
                    {
                        using (var path = rect_button_up.RoundPath(radius, false, true, false, false))
                        {
                            g.Draw(pen, path);
                        }
                        using (var path = rect_button_bottom.RoundPath(radius, false, false, true, false))
                        {
                            g.Draw(pen, path);
                        }
                    }
                }

                if (hover_button_up.Animation)
                {
                    using (var pen = new Pen(borColor.BlendColors(hover_button_up.Value, borderActive), Config.Dpi))
                    {
                        g.DrawLines(pen, TAlignMini.Top.TriangleLines(rect_button_up));
                    }
                }
                else if (hover_button_up.Switch)
                {
                    using (var pen = new Pen(borderActive, Config.Dpi))
                    {
                        g.DrawLines(pen, TAlignMini.Top.TriangleLines(rect_button_up));
                    }
                }
                else
                {
                    using (var pen = new Pen(borColor, Config.Dpi))
                    {
                        g.DrawLines(pen, TAlignMini.Top.TriangleLines(rect_button_up));
                    }
                }

                if (hover_button_bottom.Animation)
                {
                    using (var pen = new Pen(borColor.BlendColors(hover_button_bottom.Value, borderActive), Config.Dpi))
                    {
                        g.DrawLines(pen, TAlignMini.Bottom.TriangleLines(rect_button_bottom));
                    }
                }
                else if (hover_button_bottom.Switch)
                {
                    using (var pen = new Pen(borderActive, Config.Dpi))
                    {
                        g.DrawLines(pen, TAlignMini.Bottom.TriangleLines(rect_button_bottom));
                    }
                }
                else
                {
                    using (var pen = new Pen(borColor, Config.Dpi))
                    {
                        g.DrawLines(pen, TAlignMini.Bottom.TriangleLines(rect_button_bottom));
                    }
                }
            }
        }

        #endregion

        #region 鼠标

        protected override void ChangeMouseHover(bool Hover, bool Focus)
        {
            if (showcontrol && !ReadOnly)
            {
                bool old = hover_button.Switch;
                hover_button.Switch = (Hover || Focus);
                if (old == hover_button.Switch) return;
                if (hover_button.Switch)
                {
                    if (rect_button.Width > 0) UR = rect_button.Width;
                    else UR = (int)(22 * Config.Dpi);
                }
                else UR = 0;
                CalculateRect();
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (showcontrol)
            {
                if (!ReadOnly && rect_button.Contains(e.X, e.Y))
                {
                    if (rect_button_up.Contains(e.X, e.Y))
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
                else hover_button_up.Switch = hover_button_bottom.Switch = false;
            }
            base.OnMouseMove(e);
        }

        bool isdownup = false, isdowndown = false;
        int downid = 0, temp_old = 0;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (showcontrol && !ReadOnly && rect_button.Contains(e.X, e.Y))
            {
                if (decimal.TryParse(Text, out var _d)) Value = _d;
                if (rect_button_up.Contains(e.X, e.Y))
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
                            Invoke(() =>
                            {
                                if (isdownup) Value = currentValue + Increment;
                                else if (isdowndown) Value = currentValue - Increment;
                            });
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

        protected override void OnLostFocus(EventArgs e)
        {
            ILostFocus();
            base.OnLostFocus(e);
        }

        void ILostFocus()
        {
            if (IsHandleCreated)
            {
                if (IsTextEmpty)
                {
                    Value = minimum ?? 0;
                    return;
                }
                if (decimal.TryParse(Text, out var _d)) Value = _d;
                Text = GetNumberText(currentValue);
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            if (ReadOnly || !wheelModifyEnabled) return;
            if (e.Delta > 0) Value = currentValue + Increment;
            else Value = currentValue - Increment;
            if (e is HandledMouseEventArgs handled) handled.Handled = true;
        }

        #endregion
    }
}