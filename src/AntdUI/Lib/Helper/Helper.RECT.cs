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
using System.Drawing;
using System.Windows.Forms;

namespace AntdUI
{
    partial class Helper
    {
        #region 区域

        /// <summary>
        /// 得到容器标准坐标
        /// </summary>
        /// <param name="rect">区域</param>
        /// <param name="text_height">字体高度</param>
        /// <param name="icon_l">左边图标</param>
        /// <param name="icon_r">右边图标</param>
        /// <param name="right">左右翻转</param>
        /// <param name="muit">多选</param>
        public static RectTextLR IconRect(this Rectangle rect, int text_height, bool icon_l, bool icon_r, bool right, bool muit, float gap_ratio = .4F, float sp_ratio = .25F, float icon_ratio = .7F)
        {
            var rectlr = new RectTextLR();
            int sps = (int)(text_height * gap_ratio), h = (int)(text_height * icon_ratio), sps2 = sps * 2;
            if (muit)
            {
                if (icon_l && icon_r)
                {
                    int sp = (int)(text_height * sp_ratio);
                    rectlr.text = new Rectangle(rect.X + sps + h + sp, rect.Y + sps, rect.Width - ((sps + h + sp) * 2), rect.Height - sps2);

                    rectlr.l = new Rectangle(rect.X + sps, rect.Y + sps + (text_height - h) / 2, h, h);
                    rectlr.r = new Rectangle(rectlr.text.Right + sp, rectlr.l.Y, h, h);
                    if (right)
                    {
                        var r = rectlr.r;
                        rectlr.r = rectlr.l;
                        rectlr.l = r;
                        return rectlr;
                    }
                }
                else if (icon_l)
                {
                    int sp = (int)(text_height * sp_ratio);
                    if (right)
                    {
                        rectlr.text = new Rectangle(rect.X + sps, rect.Y + sps, rect.Width - sps2 - h - sp, rect.Height - sps2);
                        rectlr.l = new Rectangle(rectlr.text.Right + sp, rect.Y + sps + (text_height - h) / 2, h, h);
                        return rectlr;
                    }
                    rectlr.text = new Rectangle(rect.X + sps + h + sp, rect.Y + sps, rect.Width - sps2 - h - sp, rect.Height - sps2);
                    rectlr.l = new Rectangle(rect.X + sps, rect.Y + sps + (text_height - h) / 2, h, h);
                }
                else if (icon_r)
                {
                    int sp = (int)(text_height * sp_ratio);
                    if (right)
                    {
                        rectlr.text = new Rectangle(rect.X + sps + h + sp, rect.Y + sps, rect.Width - sps2 - h - sp, rect.Height - sps2);
                        rectlr.r = new Rectangle(rect.X + sps, rect.Y + sps + (text_height - h) / 2, h, h);
                        return rectlr;
                    }
                    rectlr.text = new Rectangle(rect.X + sps, rect.Y + sps, rect.Width - sps2 - h - sp, rect.Height - sps2);
                    rectlr.r = new Rectangle(rectlr.text.Right + sp, rect.Y + sps + (text_height - h) / 2, h, h);
                }
                else rectlr.text = new Rectangle(rect.X + sps, rect.Y + sps, rect.Width - sps2, rect.Height - sps2);
            }
            else
            {
                if (icon_l && icon_r)
                {
                    int sp = (int)(text_height * sp_ratio);
                    rectlr.text = new Rectangle(rect.X + sps + h + sp, rect.Y + (rect.Height - text_height) / 2, rect.Width - ((sps + h + sp) * 2), text_height);

                    rectlr.l = new Rectangle(rect.X + sps, rect.Y + (rect.Height - h) / 2, h, h);
                    rectlr.r = new Rectangle(rectlr.text.Right + sp, rectlr.l.Y, h, h);
                    if (right)
                    {
                        var r = rectlr.r;
                        rectlr.r = rectlr.l;
                        rectlr.l = r;
                        return rectlr;
                    }
                }
                else if (icon_l)
                {
                    int sp = (int)(text_height * sp_ratio);
                    if (right)
                    {
                        rectlr.text = new Rectangle(rect.X + sps, rect.Y + (rect.Height - text_height) / 2, rect.Width - sps2 - h - sp, text_height);
                        rectlr.l = new Rectangle(rectlr.text.Right + sp, rect.Y + (rect.Height - h) / 2, h, h);
                        return rectlr;
                    }
                    rectlr.text = new Rectangle(rect.X + sps + h + sp, rect.Y + (rect.Height - text_height) / 2, rect.Width - sps2 - h - sp, text_height);
                    rectlr.l = new Rectangle(rect.X + sps, rect.Y + (rect.Height - h) / 2, h, h);
                }
                else if (icon_r)
                {
                    int sp = (int)(text_height * sp_ratio);
                    if (right)
                    {
                        rectlr.text = new Rectangle(rect.X + sps + h + sp, rect.Y + (rect.Height - text_height) / 2, rect.Width - sps2 - h - sp, text_height);
                        rectlr.r = new Rectangle(rect.X + sps, rect.Y + (rect.Height - h) / 2, h, h);
                        return rectlr;
                    }
                    rectlr.text = new Rectangle(rect.X + sps, rect.Y + (rect.Height - text_height) / 2, rect.Width - sps2 - h - sp, text_height);
                    rectlr.r = new Rectangle(rectlr.text.Right + sp, rect.Y + (rect.Height - h) / 2, h, h);
                }
                else rectlr.text = new Rectangle(rect.X + sps, rect.Y + (rect.Height - text_height) / 2, rect.Width - sps2, text_height);
            }
            return rectlr;
        }

        public static void IconRectL(this Rectangle rect, int text_height, out Rectangle icon_rect, out Rectangle text_rect, float size = 0.8F)
        {
            int h = (int)(text_height * size);
            int dot_size_ = h / 2;
            int dot_txt_left = h * 2;

            icon_rect = new Rectangle(rect.X + dot_size_, rect.Y + (rect.Height - h) / 2, h, h);
            text_rect = new Rectangle(rect.X + dot_txt_left, rect.Y, rect.Width - dot_txt_left, rect.Height);
        }

        #region DisplayRectangle

        public static Rectangle DeflateRect(this Rectangle rect, Padding padding)
        {
            rect.X += padding.Left;
            rect.Y += padding.Top;
            rect.Width -= padding.Horizontal;
            rect.Height -= padding.Vertical;
            return rect;
        }

        public static Rectangle DeflateRect(this Rectangle rect, Padding padding, ShadowConfig config, TAlignMini align, float borderWidth = 0F)
        {
            if (config.Shadow > 0)
            {
                int shadow = (int)(config.Shadow * Config.Dpi), s2 = shadow * 2, shadowOffsetX = Math.Abs((int)(config.ShadowOffsetX * Config.Dpi)), shadowOffsetY = Math.Abs((int)(config.ShadowOffsetY * Config.Dpi));
                int x, y, w, h;
                switch (align)
                {
                    case TAlignMini.Top:
                        x = rect.X + padding.Left;
                        w = rect.Width - padding.Horizontal;

                        y = rect.Y + padding.Top + shadow;
                        h = rect.Height - padding.Vertical - shadow;
                        break;
                    case TAlignMini.Bottom:
                        x = rect.X + padding.Left;
                        w = rect.Width - padding.Horizontal;

                        y = rect.Y + padding.Top;
                        h = rect.Height - padding.Vertical - shadow;
                        break;
                    case TAlignMini.Left:
                        y = rect.Y + padding.Top;
                        h = rect.Height - padding.Vertical;

                        x = rect.X + padding.Left + shadow;
                        w = rect.Width - padding.Horizontal - shadow;
                        break;
                    case TAlignMini.Right:
                        y = rect.Y + padding.Top;
                        h = rect.Height - padding.Vertical;

                        x = rect.X + padding.Left;
                        w = rect.Width - padding.Horizontal - shadow;
                        break;
                    case TAlignMini.None:
                    default:
                        x = rect.X + padding.Left + shadow;
                        y = rect.Y + padding.Top + shadow;
                        w = rect.Width - padding.Horizontal - s2;
                        h = rect.Height - padding.Vertical - s2;
                        break;
                }

                if (config.ShadowOffsetX < 0)
                {
                    x += shadowOffsetX;
                    w -= shadowOffsetX;
                }
                if (config.ShadowOffsetY < 0)
                {
                    y += shadowOffsetY;
                    h -= shadowOffsetY;
                }

                if (borderWidth > 0)
                {
                    int pr = (int)Math.Ceiling(borderWidth * Config.Dpi), pr2 = pr * 2;
                    return new Rectangle(x + pr, y + pr, w - pr2, h - pr2);
                }
                return new Rectangle(x, y, w, h);
            }
            else
            {
                if (borderWidth > 0)
                {
                    int pr = (int)Math.Ceiling(borderWidth * Config.Dpi), pr2 = pr * 2;
                    return new Rectangle(rect.X + padding.Left + pr, rect.Y + padding.Top + pr, rect.Width - padding.Horizontal - pr2, rect.Height - padding.Vertical - pr2);
                }
                return new Rectangle(rect.X + padding.Left, rect.Y + padding.Top, rect.Width - padding.Horizontal, rect.Height - padding.Vertical);
            }
        }

        #endregion

        public static Rectangle PaddingRect(this Rectangle rect, ShadowConfig config, TAlignMini align, float borderWidth = 0F)
        {
            if (config.Shadow > 0)
            {
                int shadow = (int)(config.Shadow * Config.Dpi), s2 = shadow * 2, shadowOffsetX = Math.Abs((int)(config.ShadowOffsetX * Config.Dpi)), shadowOffsetY = Math.Abs((int)(config.ShadowOffsetY * Config.Dpi));

                int x, y, w, h;
                switch (align)
                {
                    case TAlignMini.Top:
                        x = rect.X;
                        w = rect.Width;

                        y = rect.Y + shadow;
                        h = rect.Height - shadow;
                        break;
                    case TAlignMini.Bottom:
                        x = rect.X;
                        w = rect.Width;

                        y = rect.Y;
                        h = rect.Height - shadow;
                        break;
                    case TAlignMini.Left:
                        y = rect.Y;
                        h = rect.Height;

                        x = rect.X + shadow;
                        w = rect.Width - shadow;
                        break;
                    case TAlignMini.Right:
                        y = rect.Y;
                        h = rect.Height;

                        x = rect.X;
                        w = rect.Width - shadow;
                        break;
                    case TAlignMini.None:
                    default:
                        x = rect.X + shadow;
                        y = rect.Y + shadow;
                        w = rect.Width - s2;
                        h = rect.Height - s2;
                        break;
                }

                if (config.ShadowOffsetX < 0)
                {
                    x += shadowOffsetX;
                    w -= shadowOffsetX;
                }
                if (config.ShadowOffsetY < 0)
                {
                    y += shadowOffsetY;
                    h -= shadowOffsetY;
                }

                if (borderWidth > 0)
                {
                    int pr = (int)Math.Ceiling(borderWidth * Config.Dpi / 2F), pr2 = pr * 2;
                    return new Rectangle(x + pr, y + pr, w - pr2, h - pr2);
                }
                return new Rectangle(x, y, w, h);
            }
            else
            {
                if (borderWidth > 0)
                {
                    int pr = (int)Math.Ceiling((borderWidth * Config.Dpi) / 2F), pr2 = pr * 2;
                    return new Rectangle(rect.X + pr, rect.Y + pr, rect.Width - pr2, rect.Height - pr2);
                }
                return rect;
            }
        }
        public static Rectangle PaddingRect(this Rectangle rect, Padding padding, int x, int y, int r, int b)
        {
            return new Rectangle(rect.X + padding.Left + x, rect.Y + padding.Top + y, rect.Width - padding.Horizontal - r, rect.Height - padding.Vertical - b);
        }
        public static Rectangle PaddingRect(this Rectangle rect, params Padding[] paddings)
        {
            foreach (var padding in paddings)
            {
                rect.X += padding.Left;
                rect.Y += padding.Top;
                rect.Width -= padding.Horizontal;
                rect.Height -= padding.Vertical;
            }
            return rect;
        }

        /// <summary>
        /// 获取边距
        /// </summary>
        /// <param name="rect">区域</param>
        /// <param name="padding">边距</param>
        /// <param name="size">边框</param>
        public static Rectangle PaddingRect(this Rectangle rect, Padding padding, float size = 0F)
        {
            if (size > 0)
            {
                int pr = (int)Math.Round(size), pr2 = pr * 2;
                return new Rectangle(rect.X + padding.Left + pr, rect.Y + padding.Top + pr, rect.Width - padding.Horizontal - pr2, rect.Height - padding.Vertical - pr2);
            }
            return new Rectangle(rect.X + padding.Left, rect.Y + padding.Top, rect.Width - padding.Horizontal, rect.Height - padding.Vertical);
        }
        public static Rectangle PaddingRect(this Rectangle rect, Padding padding, int x, int y, int r, int b, float size = 0F)
        {
            if (size > 0)
            {
                int pr = (int)Math.Round(size), pr2 = pr * 2;
                return new Rectangle(rect.X + padding.Left + pr + x, rect.Y + padding.Top + pr + y, rect.Width - padding.Horizontal - pr2 - r, rect.Height - padding.Vertical - pr2 - b);
            }
            return new Rectangle(rect.X + padding.Left + x, rect.Y + padding.Top + y, rect.Width - padding.Horizontal - r, rect.Height - padding.Vertical - b);
        }

        /// <summary>
        /// 得到真实渲染区域
        /// </summary>
        /// <param name="rect">容器区域</param>
        /// <param name="size">动画区域</param>
        /// <param name="shape">形状</param>
        /// <param name="joinLeft">连接左边</param>
        /// <param name="joinRight">连接右边</param>
        public static Rectangle ReadRect(this Rectangle rect, float size, TShape shape, bool joinLeft, bool joinRight)
        {
            if (shape == TShape.Circle)
            {
                int pr = (int)Math.Round(size), pr2 = pr * 2;
                if (rect.Width > rect.Height)
                {
                    int h = rect.Height - pr2;
                    return new Rectangle(rect.X + (rect.Width - h) / 2, rect.Y + pr, h, h);
                }
                else
                {
                    int w = rect.Width - pr2;
                    return new Rectangle(rect.X + pr, rect.Y + (rect.Height - w) / 2, w, w);
                }
            }
            return ReadRect(rect, size, joinLeft, joinRight);
        }

        /// <summary>
        /// 得到真实渲染区域
        /// </summary>
        /// <param name="rect">容器区域</param>
        /// <param name="size">动画区域</param>
        /// <param name="joinLeft">连接左边</param>
        /// <param name="joinRight">连接右边</param>
        public static Rectangle ReadRect(this Rectangle rect, float size, bool joinLeft, bool joinRight)
        {
            int pr = (int)Math.Round(size), pr2 = pr * 2;
            if (joinLeft && joinRight) return new Rectangle(rect.X, rect.Y + pr, rect.Width, rect.Height - pr2);
            else if (joinLeft)
            {
                var r = new Rectangle(rect.X, rect.Y + pr, rect.Width - pr, rect.Height - pr2);
                rect.X = -pr;
                rect.Width += pr;
                return r;
            }
            else if (joinRight)
            {
                var r = new Rectangle(rect.Width - (rect.Width - pr), rect.Y + pr, rect.Width - pr, rect.Height - pr2);
                rect.X = 0;
                rect.Width += pr;
                return r;
            }
            return new Rectangle(rect.X + pr, rect.Y + pr, rect.Width - pr2, rect.Height - pr2);
        }

        #endregion

        #region 文本方向

        public static void SetAlignment(this ContentAlignment textAlign, ref StringFormat stringFormat)
        {
            switch (textAlign)
            {
                case ContentAlignment.TopLeft:
                    stringFormat.Alignment = StringAlignment.Near;
                    stringFormat.LineAlignment = StringAlignment.Near;
                    //内容在垂直方向上顶部对齐，在水平方向上左边对齐
                    break;
                case ContentAlignment.TopCenter:
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Near;
                    //内容在垂直方向上顶部对齐，在水平方向上居中对齐

                    break;
                case ContentAlignment.TopRight:
                    //内容在垂直方向上顶部对齐，在水平方向上右边对齐
                    stringFormat.Alignment = StringAlignment.Far;
                    stringFormat.LineAlignment = StringAlignment.Near;
                    break;
                case ContentAlignment.MiddleLeft:
                    //内容在垂直方向上中间对齐，在水平方向上左边对齐
                    stringFormat.Alignment = StringAlignment.Near;
                    stringFormat.LineAlignment = StringAlignment.Center;

                    break;
                case ContentAlignment.MiddleCenter:
                    //内容在垂直方向上中间对齐，在水平方向上居中对齐
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;
                    break;
                case ContentAlignment.MiddleRight:
                    //内容在垂直方向上中间对齐，在水平方向上右边对齐
                    stringFormat.Alignment = StringAlignment.Far;
                    stringFormat.LineAlignment = StringAlignment.Center;

                    break;
                case ContentAlignment.BottomLeft:
                    //内容在垂直方向上底边对齐，在水平方向上左边对齐
                    stringFormat.Alignment = StringAlignment.Near;
                    stringFormat.LineAlignment = StringAlignment.Far;
                    break;
                case ContentAlignment.BottomCenter:
                    //内容在垂直方向上底边对齐，在水平方向上居中对齐
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Far;

                    break;
                case ContentAlignment.BottomRight:
                    //内容在垂直方向上底边对齐，在水平方向上右边对齐
                    stringFormat.Alignment = StringAlignment.Far;
                    stringFormat.LineAlignment = StringAlignment.Far;
                    break;
            }
        }
        public static void SetAlignment(this HorizontalAlignment textAlign, ref StringFormat stringFormat)
        {
            switch (textAlign)
            {
                case HorizontalAlignment.Left:
                    //内容在垂直方向上中间对齐，在水平方向上左边对齐
                    stringFormat.Alignment = StringAlignment.Near;
                    break;
                case HorizontalAlignment.Center:
                    //内容在垂直方向上中间对齐，在水平方向上居中对齐
                    stringFormat.Alignment = StringAlignment.Center;
                    break;
                case HorizontalAlignment.Right:
                    //内容在垂直方向上中间对齐，在水平方向上右边对齐
                    stringFormat.Alignment = StringAlignment.Far;
                    break;
            }
        }

        #endregion

        #region 三角

        public static PointF[] CheckArrow(this Rectangle rect)
        {
            float size = rect.Height * 0.15F, size2 = rect.Height * 0.2F, size3 = rect.Height * 0.26F;
            return new PointF[] {
                new PointF(rect.X+size,rect.Y+rect.Height/2),
                new PointF(rect.X+rect.Width*0.4F,rect.Y+(rect.Height-size3)),
                new PointF(rect.X+rect.Width-size2,rect.Y+size2),
            };
        }

        public static PointF[] TriangleLines(this Rectangle rect, float prog, float d = 0.7F)
        {
            float size = rect.Width * d, size2 = size / 2;
            float x = rect.X + rect.Width / 2F, y = rect.Y + rect.Height / 2F;
            if (prog == 0)
            {
                return new PointF[] {
                    new PointF(x - size2, y),
                    new PointF(x + size2,y)
                };
            }
            else if (prog > 0)
            {
                float h = size2 * prog, h2 = h / 2;
                return new PointF[] {
                    new PointF(x - size2,y + h2),
                    new PointF(x, y - h2),
                    new PointF(x + size2,y + h2)
                };
            }
            else
            {
                float h = size2 * -prog, h2 = h / 2;
                return new PointF[] {
                    new PointF(x - size2,y - h2),
                    new PointF(x, y + h2),
                    new PointF(x + size2,y - h2)
                };
            }
        }

        public static PointF[] TriangleLines(this TAlignMini align, RectangleF rect, float b = 0.375F)
        {
            float size = rect.Height * b, size2 = size / 2F;
            float x = rect.X + rect.Width / 2F, y = rect.Y + rect.Height / 2F;
            float h2 = size2 / 2;

            switch (align)
            {
                case TAlignMini.Top:
                    return new PointF[] {
                        new PointF(x - size2,y + h2),
                        new PointF(x, y - h2),
                        new PointF(x + size2,y + h2)
                    };
                case TAlignMini.Bottom:
                    return new PointF[] {
                        new PointF(x - size2,y - h2),
                        new PointF(x, y + h2),
                        new PointF(x + size2,y - h2)
                    };
                case TAlignMini.Left:
                    return new PointF[] {
                        new PointF(x + h2,y - size2),
                        new PointF(x - h2, y),
                        new PointF(x + h2,y + size2)
                    };
                case TAlignMini.Right:
                default:
                    return new PointF[] {
                        new PointF(x - h2,y - size2),
                        new PointF(x + h2, y),
                        new PointF(x - h2,y + size2)
                    };
            }
        }

        /// <summary>
        /// 得到三角绘制区域
        /// </summary>
        /// <param name="align">方向</param>
        /// <param name="arrow_size">三角大小</param>
        /// <param name="rect">全局区域</param>
        /// <param name="rect_read">内容区域</param>
        public static PointF[] AlignLines(this TAlign align, float arrow_size, RectangleF rect, RectangleF rect_read)
        {
            if (align == TAlign.Top)
            {
                //↑上
                float x = rect.Width / 2F, y = rect_read.Y + rect_read.Height;
                return new PointF[] { new PointF(x - arrow_size, y), new PointF(x + arrow_size, y), new PointF(x, y + arrow_size) };
            }
            else if (align == TAlign.Bottom)
            {
                //↓ 下
                float x = rect.Width / 2F, y = rect_read.Y - arrow_size;
                return new PointF[] { new PointF(x, y), new PointF(x - arrow_size, y + arrow_size), new PointF(x + arrow_size, y + arrow_size) };
            }
            else if (align == TAlign.Left || align == TAlign.LT || align == TAlign.LB)
            {
                //← 左
                float x = rect_read.X + rect_read.Width, y = rect.Height / 2F;
                return new PointF[] { new PointF(x, y - arrow_size), new PointF(x, y + arrow_size), new PointF(x + arrow_size, y) };
            }
            else if (align == TAlign.Right || align == TAlign.RT || align == TAlign.RB)
            {
                //→ 右
                float x = rect_read.X - arrow_size, y = rect.Height / 2F;
                return new PointF[] { new PointF(x, y), new PointF(x + arrow_size, y - arrow_size), new PointF(x + arrow_size, y + arrow_size) };
            }

            #region 下

            else if (align == TAlign.BL)
            {
                //↙ 下左
                float x = rect_read.X + arrow_size * 3F, y = rect_read.Y - arrow_size;
                return new PointF[] { new PointF(x, y), new PointF(x - arrow_size, y + arrow_size), new PointF(x + arrow_size, y + arrow_size) };
            }
            else if (align == TAlign.BR)
            {
                //↘ 下右
                float x = rect_read.X + rect_read.Width - arrow_size * 3F, y = rect_read.Y - arrow_size;
                return new PointF[] { new PointF(x, y), new PointF(x - arrow_size, y + arrow_size), new PointF(x + arrow_size, y + arrow_size) };
            }

            #endregion

            #region 上

            else if (align == TAlign.TL)
            {
                //↖ 上左
                float x = rect_read.X + arrow_size * 3F, y = rect_read.Y + rect_read.Height;
                return new PointF[] { new PointF(x - arrow_size, y), new PointF(x + arrow_size, y), new PointF(x, y + arrow_size) };
            }
            else if (align == TAlign.TR)
            {
                //↗ 上右
                float x = rect_read.X + rect_read.Width - arrow_size * 3F, y = rect_read.Y + rect_read.Height;
                return new PointF[] { new PointF(x - arrow_size, y), new PointF(x + arrow_size, y), new PointF(x, y + arrow_size) };
            }

            #endregion

            else
            {
                //↑上
                float x = rect.Width / 2F, y = rect_read.Y + rect_read.Height;
                return new PointF[] { new PointF(x - arrow_size, y), new PointF(x + arrow_size, y), new PointF(x, y + arrow_size) };
            }
        }

        #endregion

        #region 角度方向

        /// <summary>
        /// 转换大致位置
        /// </summary>
        /// <param name="align">方向</param>
        public static TAlignMini AlignMini(this TAlign align)
        {
            if (align == TAlign.BL || align == TAlign.Bottom || align == TAlign.BR)
                return TAlignMini.Bottom;
            else if (align == TAlign.TL || align == TAlign.Top || align == TAlign.TR)
                return TAlignMini.Top;
            else if (align == TAlign.RT || align == TAlign.Right || align == TAlign.RB)
                return TAlignMini.Right;
            else if (align == TAlign.LT || align == TAlign.Left || align == TAlign.LB)
                return TAlignMini.Left;
            return TAlignMini.None;
        }

        /// <summary>
        /// 转换反向大致位置
        /// </summary>
        /// <param name="align">方向</param>
        /// <param name="vertical">是否竖向</param>
        public static TAlign AlignMiniReverse(this TAlign align, bool vertical)
        {
            if (vertical)
            {
                if (align == TAlign.TL || align == TAlign.BL || align == TAlign.LB || align == TAlign.Left || align == TAlign.LT) return TAlign.Right;
                return TAlign.Left;
            }
            else
            {
                if (align == TAlign.TL || align == TAlign.Top || align == TAlign.TR || align == TAlign.RT) return TAlign.Bottom;
                return TAlign.Top;
            }
        }

        /// <summary>
        /// 弹出坐标
        /// </summary>
        /// <param name="align">方向</param>
        /// <param name="point">控件坐标</param>
        /// <param name="size">控件大小</param>
        /// <param name="width">提示框宽度</param>
        /// <param name="height">提示框高度</param>
        public static Point AlignPoint(this TAlign align, Point point, Size size, int width, int height)
        {
            switch (align)
            {
                case TAlign.Top:
                    return new Point(point.X + (size.Width - width) / 2, point.Y - height);
                case TAlign.TL:
                    return new Point(point.X, point.Y - height);
                case TAlign.TR:
                    return new Point(point.X + size.Width - width, point.Y - height);
                case TAlign.Bottom:
                    return new Point(point.X + (size.Width - width) / 2, point.Y + size.Height);
                case TAlign.BL:
                    return new Point(point.X, point.Y + size.Height);
                case TAlign.BR:
                    return new Point(point.X + size.Width - width, point.Y + size.Height);
                case TAlign.Left:
                case TAlign.LT:
                case TAlign.LB:
                    return new Point(point.X - width, point.Y + (size.Height - height) / 2);
                case TAlign.Right:
                case TAlign.RT:
                case TAlign.RB:
                    return new Point(point.X + size.Width, point.Y + (size.Height - height) / 2);
                default:
                    return new Point(point.X + (size.Width - width) / 2, point.Y - height);
            }
        }

        public static Point AlignPoint(this TAlign align, Rectangle rect, Rectangle size)
        {
            return AlignPoint(align, rect.Location, rect.Size, size.Width, size.Height);
        }

        public static Point AlignPoint(this TAlign align, Rectangle rect, int width, int height)
        {
            return AlignPoint(align, rect.Location, rect.Size, width, height);
        }

        #endregion
    }
}