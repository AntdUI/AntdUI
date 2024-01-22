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
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// Divider 分割线
    /// </summary>
    /// <remarks>区隔内容的分割线。</remarks>
    [Description("Divider 分割线")]
    [ToolboxItem(true)]
    [DefaultProperty("Color")]
    [Designer(typeof(IControlDesigner))]
    public class Divider : IControl
    {
        #region 属性

        /// <summary>
        /// 是否竖向
        /// </summary>
        [Description("是否竖向"), Category("外观"), DefaultValue(false)]
        public bool Vertical { get; set; } = false;

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
            }
        }

        Color? color;
        /// <summary>
        /// 线颜色
        /// </summary>
        [Description("线颜色"), Category("外观"), DefaultValue(null)]
        public Color? ColorSplit
        {
            get => color;
            set
            {
                if (color == value) return;
                color = value;
                Invalidate();
            }
        }

        string? text = null;
        /// <summary>
        /// 文本
        /// </summary>
        [Description("文本"), Category("外观"), DefaultValue(null)]
        public new string? Text
        {
            get => text;
            set
            {
                if (text == value) return;
                text = value;
                Invalidate();
            }
        }

        #endregion

        protected override void OnPaint(PaintEventArgs e)
        {
            var _rect = ClientRectangle;
            var rect = _rect.PaddingRect(Margin);
            if (rect.Width == 0 || rect.Height == 0) return;
            var g = e.Graphics.High();
            using (var brush = new SolidBrush(color.HasValue ? color.Value : Style.Db.Split))
            {
                if (text != null)
                {
                    if (Vertical)
                    {
                        var text_ = string.Join(Environment.NewLine, text.ToCharArray());
                        var size = g.MeasureString(text_, Font, 0, Helper.stringFormatCenter);

                        float f_margin = rect.Height * orientationMargin, font_margin = size.Width * textPadding, x = rect.X + (rect.Width - thickness) / 2F;
                        switch (Orientation)
                        {
                            case TOrientation.Left:
                                if (f_margin > 0)
                                {
                                    var font_irect = new RectangleF(rect.X + (rect.Width - size.Width) / 2F, rect.Y + f_margin + font_margin, size.Width, size.Height);
                                    g.FillRectangle(brush, new RectangleF(x, rect.Y, thickness, f_margin));
                                    g.FillRectangle(brush, new RectangleF(x, font_irect.Bottom + font_margin, thickness, rect.Height - size.Height - f_margin - font_margin * 2F));
                                    PaintText(g, text_, font_irect, Helper.stringFormatCenter3, Enabled);
                                }
                                else
                                {
                                    var font_irect = new RectangleF(rect.X + (rect.Width - size.Width) / 2F, rect.Y, size.Width, size.Height);
                                    g.FillRectangle(brush, new RectangleF(x, font_irect.Bottom + font_margin, thickness, rect.Height - size.Height - font_margin));
                                    PaintText(g, text_, font_irect, Helper.stringFormatCenter3, Enabled);
                                }
                                break;
                            case TOrientation.Right:
                                if (f_margin > 0)
                                {
                                    var font_irect = new RectangleF(rect.X + (rect.Width - size.Width) / 2F, rect.Bottom - size.Height - f_margin - font_margin, size.Width, size.Height);
                                    g.FillRectangle(brush, new RectangleF(x, rect.Y, thickness, rect.Height - size.Height - f_margin - font_margin * 2F));
                                    g.FillRectangle(brush, new RectangleF(x, font_irect.Bottom + font_margin, thickness, f_margin));
                                    PaintText(g, text, font_irect, Helper.stringFormatCenter3, Enabled);
                                }
                                else
                                {
                                    var font_irect = new RectangleF(rect.X + (rect.Width - size.Width) / 2F, rect.Bottom - size.Height, size.Width, size.Height);
                                    g.FillRectangle(brush, new RectangleF(x, rect.Y, thickness, rect.Height - size.Height - font_margin));
                                    PaintText(g, text, font_irect, Helper.stringFormatCenter3, Enabled);
                                }
                                break;
                            default:
                                float f_h = (rect.Height - size.Height) / 2F - f_margin - font_margin;
                                g.FillRectangle(brush, new RectangleF(x, rect.Y, thickness, f_h));
                                g.FillRectangle(brush, new RectangleF(x, rect.Y + f_h + size.Height + (f_margin + font_margin) * 2F, thickness, f_h));
                                PaintText(g, text_, _rect, Helper.stringFormatCenter3, Enabled);
                                break;
                        }
                    }
                    else
                    {
                        var size = g.MeasureString(text, Font);
                        float f_margin = rect.Width * orientationMargin, font_margin = size.Height * textPadding, y = rect.Y + (rect.Height - thickness) / 2F;
                        switch (Orientation)
                        {
                            case TOrientation.Left:
                                if (f_margin > 0)
                                {
                                    var font_irect = new RectangleF(rect.X + f_margin + font_margin, rect.Y + (rect.Height - size.Height) / 2F, size.Width, size.Height);
                                    g.FillRectangle(brush, new RectangleF(rect.X, y, f_margin, thickness));
                                    g.FillRectangle(brush, new RectangleF(font_irect.Right + font_margin, y, rect.Width - size.Width - f_margin - font_margin * 2F, thickness));
                                    PaintText(g, text, font_irect, Helper.stringFormatCenter, Enabled);
                                }
                                else
                                {
                                    var font_irect = new RectangleF(rect.X, rect.Y + (rect.Height - size.Height) / 2F, size.Width, size.Height);
                                    g.FillRectangle(brush, new RectangleF(font_irect.Right + font_margin, y, rect.Width - size.Width - font_margin, thickness));
                                    PaintText(g, text, font_irect, Helper.stringFormatCenter, Enabled);
                                }
                                break;
                            case TOrientation.Right:
                                if (f_margin > 0)
                                {
                                    var font_irect = new RectangleF(rect.Right - size.Width - f_margin - font_margin, rect.Y + (rect.Height - size.Height) / 2F, size.Width, size.Height);
                                    g.FillRectangle(brush, new RectangleF(rect.X, y, rect.Width - size.Width - f_margin - font_margin * 2F, thickness));
                                    g.FillRectangle(brush, new RectangleF(font_irect.Right + font_margin, y, f_margin, thickness));
                                    PaintText(g, text, font_irect, Helper.stringFormatCenter, Enabled);
                                }
                                else
                                {
                                    var font_irect = new RectangleF(rect.Right - size.Width, rect.Y + (rect.Height - size.Height) / 2F, size.Width, size.Height);
                                    g.FillRectangle(brush, new RectangleF(rect.X, y, rect.Width - size.Width - font_margin, thickness));
                                    PaintText(g, text, font_irect, Helper.stringFormatCenter, Enabled);
                                }
                                break;
                            default:
                                float f_w = (rect.Width - size.Width) / 2F - f_margin - font_margin;
                                g.FillRectangle(brush, new RectangleF(rect.X, y, f_w, thickness));
                                g.FillRectangle(brush, new RectangleF(rect.X + f_w + size.Width + (f_margin + font_margin) * 2F, y, f_w, thickness));
                                PaintText(g, text, _rect, Helper.stringFormatCenter, Enabled);
                                break;
                        }
                    }
                }
                else
                {
                    if (Vertical) g.FillRectangle(brush, new RectangleF(rect.X + (rect.Width - thickness) / 2F, rect.Y, thickness, rect.Height));
                    else g.FillRectangle(brush, new RectangleF(rect.X, rect.Y + (rect.Height - thickness) / 2F, rect.Width, thickness));
                }
            }
            this.PaintBadge(g);
            base.OnPaint(e);
        }
    }
}