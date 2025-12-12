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

        public static void IconRectL(this Rectangle rect, int text_height, out Rectangle icon_rect, out Rectangle text_rect, float size = .8F)
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
        public static Size DeflateSize(this Size size, Padding padding)
        {
            size.Width += padding.Horizontal;
            size.Height += padding.Vertical;
            return size;
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
        public static Rectangle PaddingRect(this Rectangle rect, Padding padding, int x, int y, int r, int b) => new Rectangle(rect.X + padding.Left + x, rect.Y + padding.Top + y, rect.Width - padding.Horizontal - x - r, rect.Height - padding.Vertical - y - b);
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
                int pr = (int)Math.Ceiling(size), pr2 = pr * 2;
                return new Rectangle(rect.X + padding.Left + pr, rect.Y + padding.Top + pr, rect.Width - padding.Horizontal - pr2, rect.Height - padding.Vertical - pr2);
            }
            return new Rectangle(rect.X + padding.Left, rect.Y + padding.Top, rect.Width - padding.Horizontal, rect.Height - padding.Vertical);
        }
        public static Rectangle PaddingRect(this Rectangle rect, Padding padding, int x, int y, int r, int b, float size = 0F)
        {
            if (size > 0)
            {
                int pr = (int)Math.Ceiling(size), pr2 = pr * 2;
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
        public static Rectangle ReadRect(this Rectangle rect, float size, TShape shape, TJoinMode joinMode, bool joinLeft, bool joinRight)
        {
            if (shape == TShape.Circle)
            {
                int pr = (int)Math.Ceiling(size), pr2 = pr * 2;
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
            return ReadRect(rect, size, joinMode, joinLeft, joinRight);
        }

        /// <summary>
        /// 得到真实渲染区域
        /// </summary>
        /// <param name="rect">容器区域</param>
        /// <param name="size">动画区域</param>
        /// <param name="joinLeft">连接左边</param>
        /// <param name="joinRight">连接右边</param>
        public static Rectangle ReadRect(this Rectangle rect, float size, TJoinMode joinMode, bool joinLeft, bool joinRight)
        {
            int pr = (int)Math.Ceiling(size), pr2 = pr * 2;
            switch (joinMode)
            {
                case TJoinMode.Left:
                    return new Rectangle(rect.Width - (rect.Width - pr), rect.Y + pr, rect.Width - pr, rect.Height - pr2);
                case TJoinMode.Right:
                    return new Rectangle(rect.X, rect.Y + pr, rect.Width - pr, rect.Height - pr2);
                case TJoinMode.LR:
                    return new Rectangle(rect.X, rect.Y + pr, rect.Width, rect.Height - pr2);

                case TJoinMode.Top:
                    return new Rectangle(rect.X + pr, rect.Height - (rect.Height - pr), rect.Width - pr2, rect.Height - pr);
                case TJoinMode.Bottom:
                    return new Rectangle(rect.X + pr, rect.Y, rect.Width - pr2, rect.Height - pr);
                case TJoinMode.TB:
                    return new Rectangle(rect.X + pr, rect.Y, rect.Width - pr2, rect.Height);
                case TJoinMode.None:
                default:
                    if (joinLeft && joinRight) return new Rectangle(rect.X, rect.Y + pr, rect.Width, rect.Height - pr2);
                    else if (joinLeft) return new Rectangle(rect.X, rect.Y + pr, rect.Width - pr, rect.Height - pr2);
                    else if (joinRight) return new Rectangle(rect.Width - (rect.Width - pr), rect.Y + pr, rect.Width - pr, rect.Height - pr2);
                    return new Rectangle(rect.X + pr, rect.Y + pr, rect.Width - pr2, rect.Height - pr2);
            }
        }

        #endregion

        #region 文本方向

        [Obsolete("use FormatFlags enum")]
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

        [Obsolete("use FormatFlags enum")]
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

        public static FormatFlags SetAlignment(ContentAlignment textAlign)
        {
            switch (textAlign)
            {
                case ContentAlignment.TopLeft:
                    //内容在垂直方向上顶部对齐，在水平方向上左边对齐
                    return FormatFlags.Left | FormatFlags.Top;
                case ContentAlignment.TopCenter:
                    //内容在垂直方向上顶部对齐，在水平方向上居中对齐
                    return FormatFlags.HorizontalCenter | FormatFlags.Top;
                case ContentAlignment.TopRight:
                    //内容在垂直方向上顶部对齐，在水平方向上右边对齐
                    return FormatFlags.Right | FormatFlags.Top;
                case ContentAlignment.MiddleLeft:
                    //内容在垂直方向上中间对齐，在水平方向上左边对齐
                    return FormatFlags.Left | FormatFlags.VerticalCenter;
                case ContentAlignment.MiddleCenter:
                    //内容在垂直方向上中间对齐，在水平方向上居中对齐
                    return FormatFlags.Center;
                case ContentAlignment.MiddleRight:
                    //内容在垂直方向上中间对齐，在水平方向上右边对齐
                    return FormatFlags.Right | FormatFlags.VerticalCenter;
                case ContentAlignment.BottomLeft:
                    //内容在垂直方向上底边对齐，在水平方向上左边对齐
                    return FormatFlags.Left | FormatFlags.Bottom;
                case ContentAlignment.BottomCenter:
                    //内容在垂直方向上底边对齐，在水平方向上居中对齐
                    return FormatFlags.HorizontalCenter | FormatFlags.Bottom;
                case ContentAlignment.BottomRight:
                    //内容在垂直方向上底边对齐，在水平方向上右边对齐
                    return FormatFlags.Right | FormatFlags.Bottom;
                default: return FormatFlags.Center;
            }
        }
        public static FormatFlags SetAlignment(this ContentAlignment textAlign, FormatFlags format) => FormatFlagsBase(format) | SetAlignment(textAlign);

        public static FormatFlags SetAlignment(this HorizontalAlignment textAlign, FormatFlags format)
        {
            var flags = FormatFlagsBase(format);

            if (format.HasFlag(FormatFlags.VerticalCenter)) flags |= FormatFlags.VerticalCenter;
            else if (format.HasFlag(FormatFlags.Top)) flags |= FormatFlags.Top;
            else if (format.HasFlag(FormatFlags.Bottom)) flags |= FormatFlags.Bottom;

            switch (textAlign)
            {
                case HorizontalAlignment.Left:
                    flags |= FormatFlags.Left;
                    break;
                case HorizontalAlignment.Center:
                    flags |= FormatFlags.HorizontalCenter;
                    break;
                case HorizontalAlignment.Right:
                    flags |= FormatFlags.Right;
                    break;
            }

            return flags;
        }
        public static FormatFlags SetHorizontalAlignment(FormatFlags textAlign, FormatFlags format)
        {
            var flags = FormatFlagsBase(format);

            if (format.HasFlag(FormatFlags.VerticalCenter)) flags |= FormatFlags.VerticalCenter;
            else if (format.HasFlag(FormatFlags.Top)) flags |= FormatFlags.Top;
            else if (format.HasFlag(FormatFlags.Bottom)) flags |= FormatFlags.Bottom;

            if (textAlign.HasFlag(FormatFlags.HorizontalCenter)) flags |= FormatFlags.HorizontalCenter;
            else if (textAlign.HasFlag(FormatFlags.Left)) flags |= FormatFlags.Left;
            else if (textAlign.HasFlag(FormatFlags.Right)) flags |= FormatFlags.Right;
            else flags |= FormatFlags.Left;

            return flags;
        }
        public static FormatFlags SetVerticalAlignment(FormatFlags textAlign, FormatFlags format)
        {
            var flags = FormatFlagsBase(format);

            if (format.HasFlag(FormatFlags.HorizontalCenter)) flags |= FormatFlags.HorizontalCenter;
            else if (format.HasFlag(FormatFlags.Left)) flags |= FormatFlags.Left;
            else if (format.HasFlag(FormatFlags.Right)) flags |= FormatFlags.Right;

            if (textAlign.HasFlag(FormatFlags.VerticalCenter)) flags |= FormatFlags.VerticalCenter;
            else if (textAlign.HasFlag(FormatFlags.Top)) flags |= FormatFlags.Top;
            else if (textAlign.HasFlag(FormatFlags.Right)) flags |= FormatFlags.Bottom;
            else flags |= FormatFlags.Top;

            return flags;
        }
        public static FormatFlags FormatFlagsBase(FormatFlags format)
        {
            FormatFlags flags = 0;
            if (format.HasFlag(FormatFlags.NoWrap)) flags |= FormatFlags.NoWrap;
            if (format.HasFlag(FormatFlags.EllipsisCharacter)) flags |= FormatFlags.EllipsisCharacter;
            if (format.HasFlag(FormatFlags.HotkeyPrefixShow)) flags |= FormatFlags.HotkeyPrefixShow;
            if (format.HasFlag(FormatFlags.DirectionVertical)) flags |= FormatFlags.DirectionVertical;

            return flags;
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

        public static PointF[] TriangleLinesVertical(this Rectangle rect, float prog, float d = 0.7F)
        {
            float size = rect.Width * d, size2 = size / 2F;
            float x = rect.X + rect.Width / 2F, y = rect.Y + (rect.Height / 2F);
            if (prog == 0)
            {
                return new PointF[] {
                    new PointF(x - size2, y),
                    new PointF(x + size2, y)
                };
            }
            else if (prog > 0)
            {
                float h = size2 * prog, h2 = h / 2;
                return new PointF[] {
                    new PointF(x - size2, y + h2),
                    new PointF(x, y - h2),
                    new PointF(x + size2, y + h2)
                };
            }
            else
            {
                float h = size2 * -prog, h2 = h / 2;
                return new PointF[] {
                    new PointF(x - size2, y - h2),
                    new PointF(x, y + h2),
                    new PointF(x + size2, y - h2)
                };
            }
        }
        public static PointF[] TriangleLinesHorizontal(this Rectangle rect, float prog, float d = 0.7F)
        {
            // 计算箭头尺寸和中心点坐标
            float size = rect.Height * d, size2 = size / 2F;
            float x = rect.X + rect.Width / 2F, y = rect.Y + rect.Height / 2F;
            if (prog == 0)
            {
                return new PointF[] {
                    new PointF(x, y - size2),
                    new PointF(x, y + size2)
                };
            }
            else if (prog > 0)
            {
                float w = size2 * prog, w2 = w / 2;
                return new PointF[] {
                    new PointF(x + w2, y - size2), // 右侧底点
                    new PointF(x - w2, y),         // 尖端
                    new PointF(x + w2, y + size2)  // 左侧顶点
                };
            }
            else
            {
                float w = size2 * -prog, w2 = w / 2;
                return new PointF[] {
                    new PointF(x - w2, y - size2), // 左侧顶点
                    new PointF(x + w2, y), // 尖端
                    new PointF(x - w2, y + size2) // 右侧底点
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
        public static PointF[] AlignLines(this TAlign align, float arrow_size, RectangleF rect)
        {
            if (align == TAlign.Top)
            {
                //↑上
                float x = rect.X + rect.Width / 2F, y = rect.Y - arrow_size;
                return new PointF[] { new PointF(x, y), new PointF(x - arrow_size, y + arrow_size), new PointF(x + arrow_size, y + arrow_size) };
            }
            else if (align == TAlign.Bottom)
            {
                //↓ 下
                float x = rect.X + rect.Width / 2F, y = rect.Y + rect.Height;
                return new PointF[] { new PointF(x - arrow_size, y), new PointF(x + arrow_size, y), new PointF(x, y + arrow_size) };
            }
            else if (align == TAlign.Left)
            {
                //← 左
                float x = rect.X - arrow_size, y = rect.Y + rect.Height / 2F;
                return new PointF[] { new PointF(x, y), new PointF(x + arrow_size, y - arrow_size), new PointF(x + arrow_size, y + arrow_size) };
            }
            else if (align == TAlign.Right)
            {
                //→ 右
                float x = rect.X + rect.Width, y = rect.Y + rect.Height / 2F;
                return new PointF[] { new PointF(x, y - arrow_size), new PointF(x, y + arrow_size), new PointF(x + arrow_size, y) };
            }
            else if (align == TAlign.LT)
            {
                //↖ 左上
                float x = rect.X - arrow_size, y = rect.Y + arrow_size * 3F;
                return new PointF[] { new PointF(x, y), new PointF(x + arrow_size, y - arrow_size), new PointF(x + arrow_size, y + arrow_size) };
            }
            else if (align == TAlign.RT)
            {
                //↗ 右上
                float x = rect.X + rect.Width, y = rect.Y + arrow_size * 3F;
                return new PointF[] { new PointF(x, y - arrow_size), new PointF(x, y + arrow_size), new PointF(x + arrow_size, y) };
            }
            else if (align == TAlign.LB)
            {
                //↙ 左下
                float x = rect.X - arrow_size, y = rect.Y + rect.Height - arrow_size * 3F;
                return new PointF[] { new PointF(x, y), new PointF(x + arrow_size, y - arrow_size), new PointF(x + arrow_size, y + arrow_size) };
            }
            else if (align == TAlign.RB)
            {
                //↘ 右下
                float x = rect.X + rect.Width, y = rect.Y + rect.Height - arrow_size * 3F;
                return new PointF[] { new PointF(x, y - arrow_size), new PointF(x, y + arrow_size), new PointF(x + arrow_size, y) };
            }

            else if (align == TAlign.BL)
            {
                //↙ 下左
                float x = rect.X + arrow_size * 3F, y = rect.Y + rect.Height;
                return new PointF[] { new PointF(x - arrow_size, y), new PointF(x + arrow_size, y), new PointF(x, y + arrow_size) };
            }
            else if (align == TAlign.BR)
            {
                //↘ 下右
                float x = rect.X + rect.Width - arrow_size * 3F, y = rect.Y + rect.Height;
                return new PointF[] { new PointF(x - arrow_size, y), new PointF(x + arrow_size, y), new PointF(x, y + arrow_size) };
            }

            else if (align == TAlign.TL)
            {
                //↖ 上左
                float x = rect.X + arrow_size * 3F, y = rect.Y - arrow_size;
                return new PointF[] { new PointF(x, y), new PointF(x - arrow_size, y + arrow_size), new PointF(x + arrow_size, y + arrow_size) };
            }
            else if (align == TAlign.TR)
            {
                //↗ 上右
                float x = rect.X + rect.Width - arrow_size * 3F, y = rect.Y - arrow_size;
                return new PointF[] { new PointF(x, y), new PointF(x - arrow_size, y + arrow_size), new PointF(x + arrow_size, y + arrow_size) };
            }

            else
            {
                //↑上
                float x = rect.X + rect.Width / 2F, y = rect.Y - arrow_size;
                return new PointF[] { new PointF(x, y), new PointF(x - arrow_size, y + arrow_size), new PointF(x + arrow_size, y + arrow_size) };
            }
        }
        public static PointF[] AlignLines(this TAlign align, float arrow_size, RectangleF rect, float itemSize)
        {
            if (itemSize > 0)
            {
                if (align == TAlign.Top)
                {
                    //↑上
                    float x = rect.X + rect.Width / 2F, y = rect.Y - arrow_size;
                    return new PointF[] { new PointF(x, y), new PointF(x - arrow_size, y + arrow_size), new PointF(x + arrow_size, y + arrow_size) };
                }
                else if (align == TAlign.Bottom)
                {
                    //↓ 下
                    float x = rect.X + rect.Width / 2F, y = rect.Y + rect.Height;
                    return new PointF[] { new PointF(x - arrow_size, y), new PointF(x + arrow_size, y), new PointF(x, y + arrow_size) };
                }
                else if (align == TAlign.Left)
                {
                    //← 左
                    float x = rect.X - arrow_size, y = rect.Y + rect.Height / 2F;
                    return new PointF[] { new PointF(x, y), new PointF(x + arrow_size, y - arrow_size), new PointF(x + arrow_size, y + arrow_size) };
                }
                else if (align == TAlign.Right)
                {
                    //→ 右
                    float x = rect.X + rect.Width, y = rect.Y + rect.Height / 2F;
                    return new PointF[] { new PointF(x, y - arrow_size), new PointF(x, y + arrow_size), new PointF(x + arrow_size, y) };
                }
                else if (align == TAlign.LT)
                {
                    //↖ 左上
                    float x = rect.X - arrow_size, y = rect.Y + itemSize;
                    return new PointF[] { new PointF(x, y), new PointF(x + arrow_size, y - arrow_size), new PointF(x + arrow_size, y + arrow_size) };
                }
                else if (align == TAlign.RT)
                {
                    //↗ 右上
                    float x = rect.X + rect.Width, y = rect.Y + itemSize;
                    return new PointF[] { new PointF(x, y - arrow_size), new PointF(x, y + arrow_size), new PointF(x + arrow_size, y) };
                }
                else if (align == TAlign.LB)
                {
                    //↙ 左下
                    float x = rect.X - arrow_size, y = rect.Y + rect.Height - itemSize;
                    return new PointF[] { new PointF(x, y), new PointF(x + arrow_size, y - arrow_size), new PointF(x + arrow_size, y + arrow_size) };
                }
                else if (align == TAlign.RB)
                {
                    //↘ 右下
                    float x = rect.X + rect.Width, y = rect.Y + rect.Height - itemSize;
                    return new PointF[] { new PointF(x, y - arrow_size), new PointF(x, y + arrow_size), new PointF(x + arrow_size, y) };
                }

                else if (align == TAlign.BL)
                {
                    //↙ 下左
                    float x = rect.X + itemSize, y = rect.Y + rect.Height;
                    return new PointF[] { new PointF(x - arrow_size, y), new PointF(x + arrow_size, y), new PointF(x, y + arrow_size) };
                }
                else if (align == TAlign.BR)
                {
                    //↘ 下右
                    float x = rect.X + rect.Width - itemSize, y = rect.Y + rect.Height;
                    return new PointF[] { new PointF(x - arrow_size, y), new PointF(x + arrow_size, y), new PointF(x, y + arrow_size) };
                }

                else if (align == TAlign.TL)
                {
                    //↖ 上左
                    float x = rect.X + itemSize, y = rect.Y - arrow_size;
                    return new PointF[] { new PointF(x, y), new PointF(x - arrow_size, y + arrow_size), new PointF(x + arrow_size, y + arrow_size) };
                }
                else if (align == TAlign.TR)
                {
                    //↗ 上右
                    float x = rect.X + rect.Width - itemSize, y = rect.Y - arrow_size;
                    return new PointF[] { new PointF(x, y), new PointF(x - arrow_size, y + arrow_size), new PointF(x + arrow_size, y + arrow_size) };
                }

                else
                {
                    //↑上
                    float x = rect.X + rect.Width / 2F, y = rect.Y - arrow_size;
                    return new PointF[] { new PointF(x, y), new PointF(x - arrow_size, y + arrow_size), new PointF(x + arrow_size, y + arrow_size) };
                }
            }
            return AlignLines(align, arrow_size, rect);
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
        public static PointF[] AlignLines(this TAlign align, float arrow_size, RectangleF rect, RectangleF rect_read, int ox)
        {
            if (align == TAlign.Top)
            {
                //↑上
                float x = ox, y = rect_read.Y + rect_read.Height;
                return new PointF[] { new PointF(x - arrow_size, y), new PointF(x + arrow_size, y), new PointF(x, y + arrow_size) };
            }
            else if (align == TAlign.Bottom)
            {
                //↓ 下
                float x = ox, y = rect_read.Y - arrow_size;
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
                    return new Point(point.X - width, point.Y + (size.Height - height) / 2);
                case TAlign.LT:
                    return new Point(point.X - width, point.Y);
                case TAlign.LB:
                    return new Point(point.X - width, point.Y + size.Height - height);
                case TAlign.Right:
                    return new Point(point.X + size.Width, point.Y + (size.Height - height) / 2);
                case TAlign.RT:
                    return new Point(point.X + size.Width, point.Y);
                case TAlign.RB:
                    return new Point(point.X + size.Width, point.Y + size.Height - height);
                default:
                    return new Point(point.X + (size.Width - width) / 2, point.Y - height);
            }
        }

        public static Point AlignPoint(this TAlign align, Rectangle rect, int width, int height) => AlignPoint(align, rect.Location, rect.Size, width, height);

        public static T ValueBlend<T>(T start, T end, double t)
           where T : struct
        {
            if (typeof(T) == typeof(float))
            {
                float s = Convert.ToSingle(start);
                float e = Convert.ToSingle(end);
                return (T)(object)(s + (e - s) * (float)t);
            }
            else if (typeof(T) == typeof(double))
            {
                double s = Convert.ToDouble(start);
                double e = Convert.ToDouble(end);
                return (T)(object)(s + (e - s) * t);
            }
            else if (typeof(T) == typeof(int))
            {
                int s = Convert.ToInt32(start);
                int e = Convert.ToInt32(end);
                return (T)(object)(s + (int)Math.Round((e - s) * t));
            }
            else if (typeof(T) == typeof(Color))
            {
                Color startColor = (Color)(object)start;
                Color endColor = (Color)(object)end;

                return (T)(object)Color.FromArgb(
                    (int)Math.Round(startColor.A + (endColor.A - startColor.A) * t),
                    (int)Math.Round(startColor.R + (endColor.R - startColor.R) * t),
                    (int)Math.Round(startColor.G + (endColor.G - startColor.G) * t),
                    (int)Math.Round(startColor.B + (endColor.B - startColor.B) * t)
                );
            }
            else
            {
                throw new NotSupportedException($"Type {typeof(T)} is not supported by ValueBlend.");
            }
        }

        #endregion

        /// <summary>
        /// 所有屏幕之外的位置
        /// </summary>
        public static Point OffScreenArea(int w, int h)
        {
            var allScreens = Screen.AllScreens;
            if (allScreens.Length == 0) return new Point(-w, -h);
            int x = 0, y = 0;
            foreach (var screen in allScreens)
            {
                var bounds = screen.Bounds;
                x = Math.Min(x, bounds.X);
                y = Math.Min(y, bounds.Y);
            }
            return new Point(x - w, y - h);
        }
    }
}