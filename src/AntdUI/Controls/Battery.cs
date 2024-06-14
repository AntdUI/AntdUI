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
using System.Drawing.Design;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// Battery 电量
    /// </summary>
    /// <remarks>展示设备电量。</remarks>
    [Description("Battery 电量")]
    [ToolboxItem(true)]
    [DefaultProperty("Value")]
    public class Battery : IControl
    {
        public Battery()
        {
            base.BackColor = Color.Transparent;
        }

        #region 属性

        Color? back;
        /// <summary>
        /// 背景颜色
        /// </summary>
        [Description("背景颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public new Color? BackColor
        {
            get => back;
            set
            {
                if (back == value) return;
                back = value;
                Invalidate();
            }
        }

        Color? fore;
        /// <summary>
        /// 文字颜色
        /// </summary>
        [Description("文字颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public new Color? ForeColor
        {
            get => fore;
            set
            {
                if (fore == value) fore = value;
                fore = value;
                Invalidate();
            }
        }

        int radius = 4;
        /// <summary>
        /// 圆角
        /// </summary>
        [Description("圆角"), Category("外观"), DefaultValue(4)]
        public int Radius
        {
            get => radius;
            set
            {
                if (radius == value) return;
                radius = value;
                Invalidate();
            }
        }

        int dotsize = 8;
        /// <summary>
        /// 点大小
        /// </summary>
        [Description("点大小"), Category("外观"), DefaultValue(8)]
        public int DotSize
        {
            get => dotsize;
            set
            {
                if (dotsize == value) return;
                dotsize = value;
                Invalidate();
            }
        }

        #region 进度条

        int _value = 0;
        /// <summary>
        /// 进度条
        /// </summary>
        [Description("进度条"), Category("数据"), DefaultValue(0)]
        public int Value
        {
            get => _value;
            set
            {
                if (_value == value) return;
                if (value < 0) value = 0;
                else if (value > 100) value = 100;
                _value = value;
                Invalidate();
            }
        }

        [Description("显示"), Category("外观"), DefaultValue(true)]
        public bool ShowText { get; set; } = true;

        #endregion

        Color fillfully { get; set; } = Color.FromArgb(0, 210, 121);
        [Description("满电颜色"), Category("外观"), DefaultValue(typeof(Color), "0, 210, 121")]
        public Color FillFully
        {
            get => fillfully;
            set
            {
                if (fillfully == value) return;
                fillfully = value;
                Invalidate();
            }
        }

        [Description("警告电量颜色"), Category("外观"), DefaultValue(typeof(Color), "250, 173, 20")]
        public Color FillWarn { get; set; } = Color.FromArgb(250, 173, 20);

        [Description("危险电量颜色"), Category("外观"), DefaultValue(typeof(Color), "255, 77, 79")]
        public Color FillDanger { get; set; } = Color.FromArgb(255, 77, 79);

        #endregion

        #region 渲染

        protected override void OnPaint(PaintEventArgs e)
        {
            var _rect = ClientRectangle;
            var g = e.Graphics.High();
            var size = g.MeasureString("100%", Font);
            var rect = new RectangleF((_rect.Width - size.Width) / 2F, (_rect.Height - size.Height) / 2F, size.Width, size.Height);
            float _radius = radius * Config.Dpi;
            using (var path_pain = rect.RoundPath(_radius))
            {
                if (_value >= 100)
                {
                    using (var brush = new SolidBrush(fillfully))
                    {
                        g.FillPath(brush, path_pain);
                        if (dotsize > 0)
                        {
                            float _dotsize = dotsize * Config.Dpi;
                            using (var path = new RectangleF(rect.Right, rect.Top + (rect.Height - _dotsize) / 2F, _dotsize / 2F, _dotsize).RoundPath(_radius / 2, false, true, true, false))
                            {
                                g.FillPath(brush, path);
                            }
                        }
                    }
                    if (ShowText)
                    {
                        using (var brush = new SolidBrush(fore ?? Style.Db.Text))
                        {
                            g.DrawString("100%", Font, brush, rect, c);
                        }
                    }
                }
                else
                {
                    using (var brush = new SolidBrush(back ?? Style.Db.FillSecondary))
                    {
                        g.FillPath(brush, path_pain);
                        if (dotsize > 0)
                        {
                            float _dotsize = dotsize * Config.Dpi;
                            using (var path = new RectangleF(rect.Right, rect.Top + (rect.Height - _dotsize) / 2F, _dotsize / 2F, _dotsize).RoundPath(_radius / 2, false, true, true, false))
                            {
                                g.FillPath(brush, path);
                            }
                        }
                    }
                    if (_value > 0)
                    {
                        using (var bmp = new Bitmap(_rect.Width, _rect.Height))
                        {
                            using (var g2 = Graphics.FromImage(bmp).High())
                            {
                                Color _color;
                                if (_value > 30) _color = fillfully;
                                else if (_value > 20) _color = FillWarn;
                                else _color = FillDanger;
                                using (var brush = new SolidBrush(_color))
                                {
                                    g2.FillPath(brush, path_pain);
                                }
                                var _w = rect.Width * (_value / 100F);
                                g2.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                                g2.FillRectangle(Brushes.Transparent, new RectangleF(rect.X + _w, 0, rect.Width, bmp.Height));
                            }
                            g.DrawImage(bmp, _rect);
                        }
                    }
                    if (ShowText)
                    {
                        using (var brush = new SolidBrush(fore ?? Style.Db.Text))
                        {
                            g.DrawString(_value + "%", Font, brush, rect, c);
                        }
                    }
                }
            }
            base.OnPaint(e);
        }

        StringFormat c = Helper.SF();

        #endregion
    }
}