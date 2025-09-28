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
using System.Drawing.Design;

namespace AntdUI
{
    /// <summary>
    /// Divider 分割线
    /// </summary>
    /// <remarks>区隔内容的分割线。</remarks>
    [Description("Divider 分割线")]
    [ToolboxItem(true)]
    [Designer(typeof(IControlDesigner))]
    public class Divider : IControl
    {
        #region 属性

        bool vertical = false;
        /// <summary>
        /// 是否竖向
        /// </summary>
        [Description("是否竖向"), Category("外观"), DefaultValue(false)]
        public bool Vertical
        {
            get => vertical;
            set
            {
                if (vertical == value) return;
                vertical = value;
                if (vertical) s_f.FormatFlags |= StringFormatFlags.DirectionVertical;
                else s_f.FormatFlags ^= StringFormatFlags.DirectionVertical;
                Invalidate();
            }
        }

        TOrientation orientation = TOrientation.None;
        /// <summary>
        /// 方向
        /// </summary>
        [Description("方向"), Category("外观"), DefaultValue(TOrientation.None)]
        public TOrientation Orientation
        {
            get => orientation;
            set
            {
                if (orientation == value) return;
                orientation = value;
                Invalidate();
            }
        }

        float orientationMargin = 0.02F;
        /// <summary>
        /// 文本与边缘距离，取值 0 ～ 1
        /// </summary>
        [Description("文本与边缘距离，取值 0 ～ 1"), Category("外观"), DefaultValue(0.02F)]
        public float OrientationMargin
        {
            get => orientationMargin;
            set
            {
                if (orientationMargin == value) return;
                orientationMargin = value;
                Invalidate();
            }
        }

        float textPadding = 0.4F;
        /// <summary>
        /// 文本与线距离，同等字体大小
        /// </summary>
        [Description("文本与线距离，同等字体大小"), Category("外观"), DefaultValue(0.4F)]
        public float TextPadding
        {
            get => textPadding;
            set
            {
                if (textPadding == value) return;
                textPadding = value;
                Invalidate();
            }
        }

        float thickness = 0.6F;
        /// <summary>
        /// 厚度
        /// </summary>
        [Description("厚度"), Category("外观"), DefaultValue(0.6F)]
        public float Thickness
        {
            get => thickness;
            set
            {
                if (thickness == value) return;
                thickness = value;
                Invalidate();
                OnPropertyChanged(nameof(Thickness));
            }
        }

        Color? color;
        /// <summary>
        /// 线颜色
        /// </summary>
        [Description("线颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? ColorSplit
        {
            get => color;
            set
            {
                if (color == value) return;
                color = value;
                Invalidate();
                OnPropertyChanged(nameof(ColorSplit));
            }
        }

        string? text;
        /// <summary>
        /// 文本
        /// </summary>
        [Description("文本"), Category("外观"), DefaultValue(null)]
        public override string? Text
        {
            get => this.GetLangI(LocalizationText, text);
            set
            {
                if (text == value) return;
                text = value;
                Invalidate();
                OnTextChanged(EventArgs.Empty);
                OnPropertyChanged(nameof(Text));
            }
        }

        [Description("文本"), Category("国际化"), DefaultValue(null)]
        public string? LocalizationText { get; set; }

        #endregion

        readonly StringFormat s_f = Helper.SF_ALL();
        protected override void OnDraw(DrawEventArgs e)
        {
            var rect = e.Rect.PaddingRect(Margin);
            if (rect.Width == 0 || rect.Height == 0) return;
            var g = e.Canvas;
            using (var brush = color.Brush(Colour.Split.Get(nameof(Divider), ColorScheme)))
            {
                if (Text == null)
                {
                    if (Vertical) g.Fill(brush, new RectangleF(rect.X + (rect.Width - thickness) / 2, rect.Y, thickness, rect.Height));
                    else g.Fill(brush, new RectangleF(rect.X, rect.Y + (rect.Height - thickness) / 2, rect.Width, thickness));
                }
                else
                {
                    var enabled = Enabled;
                    var size = g.MeasureText(Text, Font, 0, s_f);
                    if (Vertical)
                    {
                        int f_margin = (int)(rect.Height * orientationMargin), font_margin = (int)(size.Width * TextPadding);
                        float x = rect.X + (rect.Width - thickness) / 2F;
                        switch (Orientation)
                        {
                            case TOrientation.Left:
                                if (f_margin > 0)
                                {
                                    var font_irect = new Rectangle(rect.X + (rect.Width - size.Width) / 2, rect.Y + f_margin + font_margin, size.Width, size.Height);
                                    g.Fill(brush, new RectangleF(x, rect.Y, thickness, f_margin));
                                    g.Fill(brush, new RectangleF(x, font_irect.Bottom + font_margin, thickness, rect.Height - size.Height - f_margin - font_margin * 2F));
                                    g.DrawText(Text, Font, enabled ? ForeColor : Colour.TextQuaternary.Get(nameof(Divider), "foreDisabled", ColorScheme), font_irect, s_f);
                                }
                                else
                                {
                                    var font_irect = new Rectangle(rect.X + (rect.Width - size.Width) / 2, rect.Y, size.Width, size.Height);
                                    g.Fill(brush, new RectangleF(x, font_irect.Bottom + font_margin, thickness, rect.Height - size.Height - font_margin));
                                    g.DrawText(Text, Font, enabled ? ForeColor : Colour.TextQuaternary.Get(nameof(Divider), "foreDisabled", ColorScheme), font_irect, s_f);
                                }
                                break;
                            case TOrientation.Right:
                                if (f_margin > 0)
                                {
                                    var font_irect = new Rectangle(rect.X + (rect.Width - size.Width) / 2, rect.Bottom - size.Height - f_margin - font_margin, size.Width, size.Height);
                                    g.Fill(brush, new RectangleF(x, rect.Y, thickness, rect.Height - size.Height - f_margin - font_margin * 2F));
                                    g.Fill(brush, new RectangleF(x, font_irect.Bottom + font_margin, thickness, f_margin));
                                    g.DrawText(Text, Font, enabled ? ForeColor : Colour.TextQuaternary.Get(nameof(Divider), "foreDisabled", ColorScheme), font_irect, s_f);
                                }
                                else
                                {
                                    var font_irect = new Rectangle(rect.X + (rect.Width - size.Width) / 2, rect.Bottom - size.Height, size.Width, size.Height);
                                    g.Fill(brush, new RectangleF(x, rect.Y, thickness, rect.Height - size.Height - font_margin));
                                    g.DrawText(Text, Font, enabled ? ForeColor : Colour.TextQuaternary.Get(nameof(Divider), "foreDisabled", ColorScheme), font_irect, s_f);
                                }
                                break;
                            default:
                                float f_h = (rect.Height - size.Height) / 2 - f_margin - font_margin;
                                g.Fill(brush, new RectangleF(x, rect.Y, thickness, f_h));
                                g.Fill(brush, new RectangleF(x, rect.Y + f_h + size.Height + (f_margin + font_margin) * 2F, thickness, f_h));
                                g.DrawText(Text, Font, enabled ? ForeColor : Colour.TextQuaternary.Get(nameof(Divider), "foreDisabled", ColorScheme), e.Rect, s_f);
                                break;
                        }
                    }
                    else
                    {
                        int f_margin = (int)(rect.Width * orientationMargin), font_margin = (int)(size.Height * TextPadding);
                        float y = rect.Y + (rect.Height - thickness) / 2F;
                        switch (Orientation)
                        {
                            case TOrientation.Left:
                                if (f_margin > 0)
                                {
                                    var font_irect = new Rectangle(rect.X + f_margin + font_margin, rect.Y + (rect.Height - size.Height) / 2, size.Width, size.Height);
                                    g.Fill(brush, new RectangleF(rect.X, y, f_margin, thickness));
                                    g.Fill(brush, new RectangleF(font_irect.Right + font_margin, y, rect.Width - size.Width - f_margin - font_margin * 2F, thickness));
                                    g.DrawText(Text, Font, enabled ? ForeColor : Colour.TextQuaternary.Get(nameof(Divider), "foreDisabled", ColorScheme), font_irect, s_f);
                                }
                                else
                                {
                                    var font_irect = new Rectangle(rect.X, rect.Y + (rect.Height - size.Height) / 2, size.Width, size.Height);
                                    g.Fill(brush, new RectangleF(font_irect.Right + font_margin, y, rect.Width - size.Width - font_margin, thickness));
                                    g.DrawText(Text, Font, enabled ? ForeColor : Colour.TextQuaternary.Get(nameof(Divider), "foreDisabled", ColorScheme), font_irect, s_f);
                                }
                                break;
                            case TOrientation.Right:
                                if (f_margin > 0)
                                {
                                    var font_irect = new Rectangle(rect.Right - size.Width - f_margin - font_margin, rect.Y + (rect.Height - size.Height) / 2, size.Width, size.Height);
                                    g.Fill(brush, new RectangleF(rect.X, y, rect.Width - size.Width - f_margin - font_margin * 2F, thickness));
                                    g.Fill(brush, new RectangleF(font_irect.Right + font_margin, y, f_margin, thickness));
                                    g.DrawText(Text, Font, enabled ? ForeColor : Colour.TextQuaternary.Get(nameof(Divider), "foreDisabled", ColorScheme), font_irect, s_f);
                                }
                                else
                                {
                                    var font_irect = new Rectangle(rect.Right - size.Width, rect.Y + (rect.Height - size.Height) / 2, size.Width, size.Height);
                                    g.Fill(brush, new RectangleF(rect.X, y, rect.Width - size.Width - font_margin, thickness));
                                    g.DrawText(Text, Font, enabled ? ForeColor : Colour.TextQuaternary.Get(nameof(Divider), "foreDisabled", ColorScheme), font_irect, s_f);
                                }
                                break;
                            default:
                                float f_w = (rect.Width - size.Width) / 2 - f_margin - font_margin;
                                g.Fill(brush, new RectangleF(rect.X, y, f_w, thickness));
                                g.Fill(brush, new RectangleF(rect.X + f_w + size.Width + (f_margin + font_margin) * 2F, y, f_w, thickness));
                                g.DrawText(Text, Font, enabled ? ForeColor : Colour.TextQuaternary.Get(nameof(Divider), "foreDisabled", ColorScheme), e.Rect, s_f);
                                break;
                        }
                    }
                }
            }
            base.OnDraw(e);
        }
    }
}