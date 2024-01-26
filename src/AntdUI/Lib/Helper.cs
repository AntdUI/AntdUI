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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace AntdUI
{
    public static class Helper
    {
        internal readonly static StringFormat stringFormatCenter = new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center, Trimming = StringTrimming.EllipsisCharacter, FormatFlags = StringFormatFlags.NoWrap };
        internal readonly static StringFormat stringFormatCenter2 = new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center, Trimming = StringTrimming.None, FormatFlags = StringFormatFlags.NoWrap };
        internal readonly static StringFormat stringFormatCenter3 = new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center, Trimming = StringTrimming.EllipsisCharacter };
        internal readonly static StringFormat stringFormatCenter4 = new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center };
        internal readonly static StringFormat stringFormatLeft = new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Near, Trimming = StringTrimming.EllipsisCharacter, FormatFlags = StringFormatFlags.NoWrap };

        #region 渲染帮助

        #region 图片渲染

        internal static void PaintImg(this Graphics g, RectangleF rect, Image image, TFit fit, float radius, bool round)
        {
            if (round || radius > 0)
            {
                using (var bmp = new Bitmap((int)rect.Width, (int)rect.Height))
                {
                    using (var g2 = Graphics.FromImage(bmp).High())
                    {
                        PaintImg(g2, new RectangleF(0, 0, rect.Width, rect.Height), image, fit);
                    }
                    using (var brush = new TextureBrush(bmp, WrapMode.Clamp))
                    {
                        brush.TranslateTransform(rect.X, rect.Y);
                        if (round) g.FillEllipse(brush, rect);
                        else
                        {
                            using (var path = rect.RoundPath(radius))
                            {
                                g.FillPath(brush, path);
                            }
                        }
                    }
                }
            }
            else PaintImg(g, rect, image, fit);
        }

        internal static void PaintImg(this Graphics g, RectangleF rect, Image image, TFit fit)
        {
            switch (fit)
            {
                case TFit.Fill:
                    g.DrawImage(image, rect);
                    break;
                case TFit.None:
                    g.DrawImage(image, new RectangleF(rect.X + (rect.Width - image.Width) / 2, rect.Y + (rect.Height - image.Height) / 2, image.Width, image.Height));
                    break;
                case TFit.Contain:
                    PaintImgContain(g, image, rect);
                    break;
                case TFit.Cover:
                    PaintImgCover(g, image, rect);
                    break;
            }
        }
        internal static void PaintImgCover(this Graphics g, Image image, RectangleF rect)
        {
            float destWidth = rect.Width, destHeight = rect.Height;
            float originWidth = image.Width, originHeight = image.Height;
            float currentWidth, currentHeight;
            if ((originWidth * destHeight) > (originHeight * destWidth))
            {
                currentHeight = destHeight;
                currentWidth = (originWidth * destHeight) / originHeight;
            }
            else
            {
                currentWidth = destWidth;
                currentHeight = (destWidth * originHeight) / originWidth;
            }
            g.DrawImage(image, new RectangleF(rect.X + (destWidth - currentWidth) / 2, rect.Y + (destHeight - currentHeight) / 2, currentWidth, currentHeight), new RectangleF(0, 0, originWidth, originHeight), GraphicsUnit.Pixel);
        }
        internal static void PaintImgContain(this Graphics g, Image image, RectangleF rect)
        {
            float destWidth = rect.Width, destHeight = rect.Height;
            float originWidth = image.Width, originHeight = image.Height;
            float currentWidth, currentHeight;
            if ((originWidth * destHeight) > (originHeight * destWidth))
            {
                currentWidth = destWidth;
                currentHeight = (destWidth * originHeight) / originWidth;
            }
            else
            {
                currentHeight = destHeight;
                currentWidth = (originWidth * destHeight) / originHeight;
            }
            g.DrawImage(image, new RectangleF(rect.X + (destWidth - currentWidth) / 2, rect.Y + (destHeight - currentHeight) / 2, currentWidth, currentHeight), new RectangleF(0, 0, originWidth, originHeight), GraphicsUnit.Pixel);
        }

        #endregion

        #region 图标渲染

        internal static void PaintIconComplete(this Graphics g, RectangleF rect, Color color)
        {
            using (var brush = new SolidBrush(color))
            {
                PaintIconComplete(g, rect, brush);
            }
        }

        internal static void PaintIconComplete(this Graphics g, RectangleF rect, SolidBrush brush)
        {
            float wh = rect.Height / 2F;
            float x = rect.X + wh, y = rect.Y + wh;
            float y1 = y - wh * 0.092F, y2 = y - wh * 0.356F;
            float x_1 = wh * 0.434F, x_2 = wh * 0.282F;
            g.FillPolygon(brush, new PointF[] {
                new PointF(x - x_1, y1),
                new PointF(x - x_2, y1),
                new PointF(x - wh * 0.096F, y + wh * 0.149F),
                new PointF(x + x_2, y2),
                new PointF(x + x_1, y2),
                new PointF(x - wh * 0.1F, y + wh * 0.357F),
            });
        }

        internal static void PaintIconError(this Graphics g, RectangleF rect, Color color, float dot = 0.34F, float width = 0.07F)
        {
            using (var brush = new Pen(color, rect.Height * width))
            {
                float size = rect.Height * dot;
                PointF p1 = new PointF(rect.X + size, rect.Y + size), p2 = new PointF(rect.X + rect.Width - size, rect.Y + rect.Height - size);
                g.DrawLines(brush, new PointF[] { p1, p2 });
                g.DrawLines(brush, new PointF[] { new PointF(p2.X, p1.Y), new PointF(p1.X, p2.Y) });
            }
        }

        internal static void PaintIconInfo(this Graphics g, RectangleF rect, Color color)
        {
            using (var brush = new SolidBrush(color))
            {
                float wh = rect.Height / 2F;

                float w = rect.Height * 0.07F, w2 = rect.Height * 0.11F, h = rect.Height * 0.32F;
                var rect_1 = new RectangleF(rect.X + (rect.Width - w) / 2F, rect.Y + rect.Height - h - wh * 0.5F, w, h);
                g.FillRectangle(brush, rect_1);
                g.FillEllipse(brush, new RectangleF(rect.X + (rect.Width - w2) / 2F, rect_1.Top - w - w2, w2, w2));
                //g.TranslateTransform(rect.Width, rect.Height);
                //g.RotateTransform(180);
            }
        }
        internal static void PaintIconWarn(this Graphics g, RectangleF rect, Color color)
        {
            using (var brush = new SolidBrush(color))
            {
                float wh = rect.Height / 2F;
                float w = rect.Height * 0.07F, w2 = rect.Height * 0.11F;
                var rect_1 = new RectangleF(rect.X + (rect.Width - w) / 2F, rect.Y + wh * 0.5F, w, rect.Height * 0.32F);
                g.FillRectangle(brush, rect_1);
                g.FillEllipse(brush, new RectangleF(rect.X + (rect.Width - w2) / 2F, rect_1.Top + rect_1.Height + w, w2, w2));
            }
        }

        #endregion

        public static Graphics High(this Graphics g)
        {
            Config.SetDpi(g.DpiX / 96F);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            if (Config.TextRenderingHint.HasValue) g.TextRenderingHint = Config.TextRenderingHint.Value;
            return g;
        }

        #endregion

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
        internal static RectTextLR IconRect(this Rectangle rect, int text_height, bool icon_l, bool icon_r, bool right, bool muit, float gap_ratio = .4F, float sp_ratio = .25F, float icon_ratio = .7F)
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
        internal static RectTextLR IconRect(this Rectangle rect, float text_height, bool icon_l, bool icon_r, bool right, bool muit, float gap_ratio = .4F, float sp_ratio = .25F, float icon_ratio = .7F)
        {
            return rect.IconRect((int)Math.Round(text_height), icon_l, icon_r, right, muit, gap_ratio, sp_ratio, icon_ratio);
        }

        internal static void IconRectL(this Rectangle rect, SizeF font_size, out RectangleF icon_rect, out RectangleF text_rect, float size = 0.8F)
        {
            float h = font_size.Height * size;
            float dot_size_ = h / 2;
            float dot_txt_left = h * 2;

            icon_rect = new RectangleF(rect.X + dot_size_, rect.Y + (rect.Height - h) / 2, h, h);
            text_rect = new RectangleF(rect.X + dot_txt_left, rect.Y, rect.Width - dot_txt_left, rect.Height);
        }

        #region 三角

        internal static PointF[] TriangleLines(this Rectangle rect, float prog, float d = 0.7F)
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
                float h = size2 * prog, h2 = h / 2, yc = rect.Y + (rect.Height - h2) / 2F;
                return new PointF[] {
                    new PointF(x - size2,yc + h2),
                    new PointF(x, yc - h2),
                    new PointF(x + size2,yc + h2)
                };
            }
            else
            {
                float h = size2 * -prog, h2 = h / 2, yc = rect.Y + (rect.Height - h2) / 2F;
                return new PointF[] {
                    new PointF(x - size2,yc - h2),
                    new PointF(x, yc + h2),
                    new PointF(x + size2,yc - h2)
                };
            }
        }
        internal static PointF[] TriangleLines(this RectangleF rect, float prog, float d = 0.7F)
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
                float h = size2 * prog, h2 = h / 2, yc = rect.Y + (rect.Height - h2) / 2F;
                return new PointF[] {
                    new PointF(x - size2,yc + h2),
                    new PointF(x, yc - h2),
                    new PointF(x + size2,yc + h2)
                };
            }
            else
            {
                float h = size2 * -prog, h2 = h / 2, yc = rect.Y + (rect.Height - h2) / 2F;
                return new PointF[] {
                    new PointF(x - size2,yc - h2),
                    new PointF(x, yc + h2),
                    new PointF(x + size2,yc - h2)
                };
            }
        }
        internal static PointF[] TriangleLines(this TAlignMini align, RectangleF rect, float b = 0.375F)
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

        #endregion

        #region DisplayRectangle

        public static Rectangle DeflateRect(this Rectangle rect, Padding padding)
        {
            rect.X += padding.Left;
            rect.Y += padding.Top;
            rect.Width -= padding.Horizontal;
            rect.Height -= padding.Vertical;
            return rect;
        }

        public static Rectangle DeflateRect(this Rectangle rect, Padding padding, ShadowConfig config, float borderWidth = 0F)
        {
            if (config.Shadow > 0)
            {
                int shadow = (int)(config.Shadow * Config.Dpi), s2 = shadow * 2,
                    shadowOffsetX = Math.Abs((int)(config.ShadowOffsetX * Config.Dpi)), shadowOffsetY = Math.Abs((int)(config.ShadowOffsetY * Config.Dpi));
                int x = rect.X + padding.Left + shadow, y = rect.Y + padding.Top + shadow;
                if (config.ShadowOffsetX < 0) x += shadowOffsetX;
                if (config.ShadowOffsetY < 0) y += shadowOffsetY;
                if (borderWidth > 0)
                {
                    int pr = (int)Math.Ceiling(borderWidth), pr2 = pr * 2;
                    return new Rectangle(x + pr, y + pr,
                        rect.Width - pr2 - shadowOffsetX - padding.Horizontal - s2,
                        rect.Height - pr2 - shadowOffsetY - padding.Vertical - s2);
                }
                return new Rectangle(x, y,
                    rect.Width - shadowOffsetX - padding.Horizontal - s2,
                    rect.Height - shadowOffsetY - padding.Vertical - s2);
            }
            else
            {
                if (borderWidth > 0)
                {
                    int pr = (int)Math.Ceiling(borderWidth), pr2 = pr * 2;
                    return new Rectangle(rect.X + padding.Left + pr, rect.Y + padding.Top + pr, rect.Width - padding.Horizontal - pr2, rect.Height - padding.Vertical - pr2);
                }
                return new Rectangle(rect.X + padding.Left, rect.Y + padding.Top, rect.Width - padding.Horizontal, rect.Height - padding.Vertical);
            }
        }

        #endregion

        public static Rectangle PaddingRect(this Rectangle rect, ShadowConfig config, float borderWidth = 0F)
        {
            if (config.Shadow > 0)
            {
                int shadow = (int)(config.Shadow * Config.Dpi), s2 = shadow * 2,
                    shadowOffsetX = Math.Abs((int)(config.ShadowOffsetX * Config.Dpi)), shadowOffsetY = Math.Abs((int)(config.ShadowOffsetY * Config.Dpi));
                int x = rect.X + shadow, y = rect.Y + shadow;
                if (config.ShadowOffsetX < 0) x += shadowOffsetX;
                if (config.ShadowOffsetY < 0) y += shadowOffsetY;
                if (borderWidth > 0)
                {
                    int pr = (int)Math.Ceiling(borderWidth / 2F), pr2 = pr * 2;
                    return new Rectangle(x + pr, y + pr,
                        rect.Width - pr2 - shadowOffsetX - s2,
                        rect.Height - pr2 - shadowOffsetY - s2);
                }
                return new Rectangle(x, y,
                    rect.Width - shadowOffsetX - s2,
                    rect.Height - shadowOffsetY - s2);
            }
            else
            {
                if (borderWidth > 0)
                {
                    int pr = (int)Math.Ceiling(borderWidth / 2F), pr2 = pr * 2;
                    return new Rectangle(rect.X + pr, rect.Y + pr, rect.Width - pr2, rect.Height - pr2);
                }
                return rect;
            }
        }
        public static Rectangle PaddingRect(this Rectangle rect, Padding padding, int x, int y, int r, int b)
        {
            return new Rectangle(rect.X + padding.Left + x, rect.Y + padding.Top + y, rect.Width - padding.Horizontal - r, rect.Height - padding.Vertical - b);
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

        /// <summary>
        /// 得到真实渲染区域
        /// </summary>
        /// <param name="rect">容器区域</param>
        /// <param name="size">动画区域</param>
        /// <param name="shape">形状</param>
        /// <param name="joinLeft">连接左边</param>
        /// <param name="joinRight">连接右边</param>
        internal static Rectangle ReadRect(this Rectangle rect, float size, TShape shape, bool joinLeft, bool joinRight)
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
        internal static Rectangle ReadRect(this Rectangle rect, float size, bool joinLeft, bool joinRight)
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

        #region Align

        internal static void SetAlignment(this ContentAlignment textAlign, ref StringFormat stringFormat)
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
        /// <summary>
        /// 转换大致位置
        /// </summary>
        /// <param name="align">方向</param>
        internal static TAlignMini AlignMini(this TAlign align)
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

        #endregion

        /// <summary>
        /// 得到三角绘制区域
        /// </summary>
        /// <param name="align">方向</param>
        /// <param name="arrow_size">三角大小</param>
        /// <param name="rect">全局区域</param>
        /// <param name="rect_read">内容区域</param>
        internal static PointF[] AlignLines(this TAlign align, float arrow_size, RectangleF rect, RectangleF rect_read)
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

        /// <summary>
        /// 弹出坐标
        /// </summary>
        /// <param name="align">方向</param>
        /// <param name="point">控件坐标</param>
        /// <param name="size">控件大小</param>
        /// <param name="width">提示框宽度</param>
        /// <param name="height">提示框高度</param>
        internal static Point AlignPoint(this TAlign align, Point point, Size size, int width, int height)
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

        internal static Point AlignPoint(this TAlign align, Rectangle rect, Rectangle size)
        {
            return AlignPoint(align, rect.Location, rect.Size, size.Width, size.Height);
        }

        internal static Point AlignPoint(this TAlign align, Rectangle rect, int width, int height)
        {
            return AlignPoint(align, rect.Location, rect.Size, width, height);
        }

        #region 圆角

        public static GraphicsPath RoundPath(this Rectangle rect, float radius)
        {
            return RoundPathCore(rect, radius);
        }

        internal static GraphicsPath RoundPath(this RectangleF rect, float radius, TShape shape)
        {
            return RoundPath(rect, radius, shape == TShape.Round);
        }

        internal static GraphicsPath RoundPath(this RectangleF rect, float radius, bool round)
        {
            if (round) return CapsulePathCore(rect);
            return RoundPathCore(rect, radius);
        }

        public static GraphicsPath RoundPath(this RectangleF rect, float radius)
        {
            return RoundPathCore(rect, radius);
        }

        /// <summary>
        /// 自定义圆角
        /// </summary>
        /// <param name="rect">区域</param>
        /// <param name="radius">圆角大小</param>
        /// <param name="TL">↖</param>
        /// <param name="TR">↗</param>
        /// <param name="BR">↘</param>
        /// <param name="BL">↙</param>
        public static GraphicsPath RoundPath(this RectangleF rect, float radius, bool TL, bool TR, bool BR, bool BL)
        {
            var path = new GraphicsPath();
            if (radius <= 0F) path.AddRectangle(rect);
            else
            {
                float diameter = radius * 2F;
                var arc = new RectangleF(rect.X, rect.Y, diameter, diameter);

                // TL
                if (TL) path.AddArc(arc, 180, 90);
                else path.AddLine(rect.X, rect.Y, rect.Right - diameter, rect.Y);

                // TR
                arc.X = rect.Right - diameter;
                if (TR) path.AddArc(arc, 270, 90);
                else path.AddLine(rect.Right, rect.Y, rect.Right, rect.Bottom - diameter);

                // BR
                arc.Y = rect.Bottom - diameter;
                if (BR) path.AddArc(arc, 0, 90);
                else path.AddLine(rect.Right, rect.Bottom, rect.X + diameter, rect.Bottom);

                // BL
                arc.X = rect.Left;
                if (BL) path.AddArc(arc, 90, 90);
                else path.AddLine(rect.X, rect.Bottom, rect.X, rect.Y + diameter);

                path.CloseFigure();
            }
            return path;
        }

        static GraphicsPath RoundPathCore(RectangleF rect, float radius)
        {
            var path = new GraphicsPath();
            if (radius > 0F)
            {
                if (radius >= (Math.Min(rect.Width, rect.Height) / 2F)) AddCapsule(path, rect);
                else
                {
                    float diameter = radius * 2F;
                    var arc = new RectangleF(rect.X, rect.Y, diameter, diameter);

                    // TL
                    path.AddArc(arc, 180, 90);

                    // TR
                    arc.X = rect.Right - diameter;
                    path.AddArc(arc, 270, 90);

                    // BR
                    arc.Y = rect.Bottom - diameter;
                    path.AddArc(arc, 0, 90);

                    // BL
                    arc.X = rect.Left;
                    path.AddArc(arc, 90, 90);

                    path.CloseFigure();
                }
            }
            else path.AddRectangle(rect);
            return path;
        }
        static GraphicsPath CapsulePathCore(RectangleF rect)
        {
            var path = new GraphicsPath();
            AddCapsule(path, rect);
            return path;
        }
        static void AddCapsule(GraphicsPath path, RectangleF rect)
        {
            float diameter;
            RectangleF arc;
            if (rect.Width > 0 && rect.Height > 0)
            {
                if (rect.Width > rect.Height)
                {
                    // Horizontal capsule
                    diameter = rect.Height;
                    SizeF sizeF = new SizeF(diameter, diameter);
                    arc = new RectangleF(rect.Location, sizeF);
                    path.AddArc(arc, 90, 180);
                    arc.X = rect.Right - diameter;
                    path.AddArc(arc, 270, 180);
                }
                else if (rect.Width < rect.Height)
                {
                    // Vertical capsule
                    diameter = rect.Width;
                    SizeF sizeF = new SizeF(diameter, diameter);
                    arc = new RectangleF(rect.Location, sizeF);
                    path.AddArc(arc, 180, 180);
                    arc.Y = rect.Bottom - diameter;
                    path.AddArc(arc, 0, 180);
                }
                else
                {
                    // Circle
                    path.AddEllipse(rect);
                }
            }
            else path.AddEllipse(rect);
            path.CloseFigure();
        }

        #endregion

        #region 图像处理

        #region 模糊

        public static void Blur(Bitmap bmp, int range)
        {
            Blur(bmp, range, new Rectangle(0, 0, bmp.Width, bmp.Height));
        }
        public static void Blur(Bitmap bmp, int range, Rectangle rect)
        {
            if (range > 1)
            {
                using (UnsafeBitmap unsafeBitmap = new UnsafeBitmap(bmp, true))
                {
                    BlurHorizontal(unsafeBitmap, range, rect);
                    BlurVertical(unsafeBitmap, range, rect);
                    BlurHorizontal(unsafeBitmap, range, rect);
                    BlurVertical(unsafeBitmap, range, rect);
                }
            }
        }

        private static void BlurHorizontal(UnsafeBitmap unsafeBitmap, int range, Rectangle rect)
        {
            int left = rect.X;
            int top = rect.Y;
            int right = rect.Right;
            int bottom = rect.Bottom;
            int halfRange = range / 2;
            ColorBgra[] newColors = new ColorBgra[unsafeBitmap.Width];

            for (int y = top; y < bottom; y++)
            {
                int hits = 0;
                int r = 0;
                int g = 0;
                int b = 0;
                int a = 0;

                for (int x = left - halfRange; x < right; x++)
                {
                    int oldPixel = x - halfRange - 1;
                    if (oldPixel >= left)
                    {
                        ColorBgra color = unsafeBitmap.GetPixel(oldPixel, y);

                        if (color.Bgra != 0)
                        {
                            r -= color.Red;
                            g -= color.Green;
                            b -= color.Blue;
                            a -= color.Alpha;
                        }

                        hits--;
                    }

                    int newPixel = x + halfRange;
                    if (newPixel < right)
                    {
                        ColorBgra color = unsafeBitmap.GetPixel(newPixel, y);

                        if (color.Bgra != 0)
                        {
                            r += color.Red;
                            g += color.Green;
                            b += color.Blue;
                            a += color.Alpha;
                        }

                        hits++;
                    }

                    if (x >= left)
                    {
                        newColors[x] = new ColorBgra((byte)(b / hits), (byte)(g / hits), (byte)(r / hits), (byte)(a / hits));
                    }
                }

                for (int x = left; x < right; x++)
                {
                    unsafeBitmap.SetPixel(x, y, newColors[x]);
                }
            }
        }

        private static void BlurVertical(UnsafeBitmap unsafeBitmap, int range, Rectangle rect)
        {
            int left = rect.X;
            int top = rect.Y;
            int right = rect.Right;
            int bottom = rect.Bottom;
            int halfRange = range / 2;
            ColorBgra[] newColors = new ColorBgra[unsafeBitmap.Height];

            for (int x = left; x < right; x++)
            {
                int hits = 0;
                int r = 0;
                int g = 0;
                int b = 0;
                int a = 0;

                for (int y = top - halfRange; y < bottom; y++)
                {
                    int oldPixel = y - halfRange - 1;
                    if (oldPixel >= top)
                    {
                        ColorBgra color = unsafeBitmap.GetPixel(x, oldPixel);

                        if (color.Bgra != 0)
                        {
                            r -= color.Red;
                            g -= color.Green;
                            b -= color.Blue;
                            a -= color.Alpha;
                        }

                        hits--;
                    }

                    int newPixel = y + halfRange;
                    if (newPixel < bottom)
                    {
                        ColorBgra color = unsafeBitmap.GetPixel(x, newPixel);

                        if (color.Bgra != 0)
                        {
                            r += color.Red;
                            g += color.Green;
                            b += color.Blue;
                            a += color.Alpha;
                        }

                        hits++;
                    }

                    if (y >= top)
                    {
                        newColors[y] = new ColorBgra((byte)(b / hits), (byte)(g / hits), (byte)(r / hits), (byte)(a / hits));
                    }
                }

                for (int y = top; y < bottom; y++)
                {
                    unsafeBitmap.SetPixel(x, y, newColors[y]);
                }
            }
        }

        #endregion

        #region 阴影

        public static Bitmap PaintShadow(this GraphicsPath path, int width, int height, int range = 10)
        {
            return PaintShadow(path, width, height, Color.Black, range);
        }
        public static Bitmap PaintShadow(this GraphicsPath path, int width, int height, Color color, int range = 10)
        {
            var bmp_shadow = new Bitmap(width, height);
            using (var g = Graphics.FromImage(bmp_shadow))
            {
                using (var brush = new SolidBrush(color))
                {
                    g.FillPath(brush, path);
                }
                Blur(bmp_shadow, range);
            }
            return bmp_shadow;
        }

        #endregion

        #endregion

        #region 其他

        #region 徽标

        public static void PaintBadge(this IControl control, Graphics g)
        {
            control.PaintBadge(control.ReadRectangle, g);
        }

        public static void PaintBadge(this IControl control, RectangleF rect, Graphics g)
        {
            var color = control.BadgeBack.HasValue ? control.BadgeBack.Value : Style.Db.Error;
            if (control.Badge != null)
            {
                using (var brush_fore = new SolidBrush(Style.Db.ErrorColor))
                {
                    if (control.Badge == " ")
                    {
                        var rect_badge = new RectangleF(rect.Right - 7F, 1F, 6, 6);
                        using (var brush = new SolidBrush(color))
                        {
                            g.FillEllipse(brush, rect_badge);
                            using (var pen = new Pen(brush_fore.Color, 1F))
                            {
                                g.DrawEllipse(pen, rect_badge);
                            }
                        }
                    }
                    else
                    {
                        using (var font = new Font(control.Font.FontFamily, control.BadgeSize))
                        {
                            var size = g.MeasureString(control.Badge, font);
                            var size_badge = size.Height * 1.2F;
                            if (size.Height > size.Width)
                            {
                                var rect_badge = new RectangleF(rect.Right - size_badge - 1F, 1F, size_badge, size_badge);
                                using (var brush = new SolidBrush(color))
                                {
                                    g.FillEllipse(brush, rect_badge);
                                    using (var pen = new Pen(brush_fore.Color, 1F))
                                    {
                                        g.DrawEllipse(pen, rect_badge);
                                    }
                                }
                                g.DrawString(control.Badge, font, brush_fore, rect_badge, stringFormatCenter2);
                            }
                            else
                            {
                                var w_badge = size.Width * 1.2F;
                                var rect_badge = new RectangleF(rect.Right - w_badge - 1F, 1F, w_badge, size_badge);
                                using (var brush = new SolidBrush(color))
                                {
                                    using (var path = rect_badge.RoundPath(rect_badge.Height))
                                    {
                                        g.FillPath(brush, path);
                                        using (var pen = new Pen(brush_fore.Color, 1F))
                                        {
                                            g.DrawPath(pen, path);
                                        }
                                    }
                                }
                                g.DrawString(control.Badge, font, brush_fore, rect_badge, stringFormatCenter2);
                            }
                        }
                    }
                }
            }
        }

        public static void PaintBadge(this IControl control, DateBadge badge, Font font, RectangleF rect, Graphics g)
        {
            var color = badge.Fill.HasValue ? badge.Fill.Value : control.BadgeBack.HasValue ? control.BadgeBack.Value : Style.Db.Error;
            using (var brush_fore = new SolidBrush(Style.Db.ErrorColor))
            {
                if (badge.Count == 0)
                {
                    var rect_badge = new RectangleF(rect.Right - 10F, rect.Top + 2F, 8, 8);
                    using (var brush = new SolidBrush(color))
                    {
                        g.FillEllipse(brush, rect_badge);
                        using (var pen = new Pen(brush_fore.Color, 1F))
                        {
                            g.DrawEllipse(pen, rect_badge);
                        }
                    }
                }
                else
                {
                    string countStr;
                    if (badge.Count == 999) countStr = "999";
                    else if (badge.Count > 1000) countStr = (badge.Count / 1000).ToString().Substring(0, 1) + "K+";
                    else if (badge.Count > 99) countStr = "99+";
                    else countStr = badge.Count.ToString();

                    var size = g.MeasureString(countStr, font);
                    var size_badge = size.Height * 1.2F;
                    if (size.Height > size.Width)
                    {
                        var rect_badge = new RectangleF(rect.Right - size_badge + 6F, rect.Top - 8F, size_badge, size_badge);
                        using (var brush = new SolidBrush(color))
                        {
                            g.FillEllipse(brush, rect_badge);
                            using (var pen = new Pen(brush_fore.Color, 1F))
                            {
                                g.DrawEllipse(pen, rect_badge);
                            }
                        }
                        g.DrawString(countStr, font, brush_fore, rect_badge, stringFormatCenter2);
                    }
                    else
                    {
                        var w_badge = size.Width * 1.2F;
                        var rect_badge = new RectangleF(rect.Right - w_badge + 6F, rect.Top - 8F, w_badge, size_badge);
                        using (var brush = new SolidBrush(color))
                        {
                            using (var path = rect_badge.RoundPath(rect_badge.Height))
                            {
                                g.FillPath(brush, path);
                                using (var pen = new Pen(brush_fore.Color, 1F))
                                {
                                    g.DrawPath(pen, path);
                                }
                            }
                        }
                        g.DrawString(countStr, font, brush_fore, rect_badge, stringFormatCenter2);
                    }
                }
            }
        }
        public static void PaintBadge(this Tabs control, TabsBadge badge, RectangleF rect, Font font, Graphics g)
        {
            var color = badge.Fill.HasValue ? badge.Fill.Value : control.BadgeBack.HasValue ? control.BadgeBack.Value : Style.Db.Error;
            using (var brush_fore = new SolidBrush(Style.Db.ErrorColor))
            {
                if (badge.Count == 0)
                {
                    var rect_badge = new RectangleF(rect.Right - 7F, 1F, 6, 6);
                    using (var brush = new SolidBrush(color))
                    {
                        g.FillEllipse(brush, rect_badge);
                        using (var pen = new Pen(brush_fore.Color, 1F))
                        {
                            g.DrawEllipse(pen, rect_badge);
                        }
                    }
                }
                else
                {
                    string countStr;
                    if (badge.Count == 999) countStr = "999";
                    else if (badge.Count > 1000) countStr = (badge.Count / 1000).ToString().Substring(0, 1) + "K+";
                    else if (badge.Count > 99) countStr = "99+";
                    else countStr = badge.Count.ToString();

                    var size = g.MeasureString(countStr, font);
                    var size_badge = size.Height * 1.2F;
                    if (size.Height > size.Width)
                    {
                        var rect_badge = new RectangleF(rect.Right - size_badge - 1F, 1F, size_badge, size_badge);
                        using (var brush = new SolidBrush(color))
                        {
                            g.FillEllipse(brush, rect_badge);
                            using (var pen = new Pen(brush_fore.Color, 1F))
                            {
                                g.DrawEllipse(pen, rect_badge);
                            }
                        }
                        g.DrawString(countStr, font, brush_fore, rect_badge, stringFormatCenter2);
                    }
                    else
                    {
                        var w_badge = size.Width * 1.2F;
                        var rect_badge = new RectangleF(rect.Right - w_badge - 1F, 1F, w_badge, size_badge);
                        using (var brush = new SolidBrush(color))
                        {
                            using (var path = rect_badge.RoundPath(rect_badge.Height))
                            {
                                g.FillPath(brush, path);
                                using (var pen = new Pen(brush_fore.Color, 1F))
                                {
                                    g.DrawPath(pen, path);
                                }
                            }
                        }
                        g.DrawString(countStr, font, brush_fore, rect_badge, stringFormatCenter2);
                    }
                }
            }
        }

        #endregion

        public static void PaintShadow(this Graphics g, ShadowConfig config, Rectangle _rect, RectangleF rect, float radius, bool round)
        {
            int shadow = (int)(config.Shadow * Config.Dpi), shadowOffsetX = (int)(config.ShadowOffsetX * Config.Dpi), shadowOffsetY = (int)(config.ShadowOffsetY * Config.Dpi);
            using (var bmp_shadow = new Bitmap(_rect.Width, _rect.Height))
            {
                using (var g_shadow = Graphics.FromImage(bmp_shadow))
                {
                    using (var path = RoundPath(rect, radius, round))
                    {
                        using (var brush = new SolidBrush(config.ShadowColor.HasValue ? config.ShadowColor.Value : Style.Db.TextBase))
                        {
                            g_shadow.FillPath(brush, path);
                        }
                    }
                    Blur(bmp_shadow, shadow);
                }
                using (var attributes = new ImageAttributes())
                {
                    var matrix = new ColorMatrix
                    {
                        Matrix33 = config.ShadowOpacity
                    };
                    attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                    g.DrawImage(bmp_shadow, new Rectangle(_rect.X + shadowOffsetX, _rect.Y + shadowOffsetY, _rect.Width, _rect.Height), 0, 0, _rect.Width, _rect.Height, GraphicsUnit.Pixel, attributes);
                }
            }
        }

        #endregion

        public static float Calculate(this float val, float add)
        {
            return (float)Math.Round(val + add, 3);
        }

        #region DPI

        internal static Dictionary<Control, AnchorDock> DpiSuspend(Control.ControlCollection controls)
        {
            var dir = new Dictionary<Control, AnchorDock>();
            foreach (Control control in controls)
            {
                dir.Add(control, new AnchorDock(control));
                if (controls.Count > 0) DpiSuspend(ref dir, control.Controls);
            }
            return dir;
        }
        internal static void DpiSuspend(ref Dictionary<Control, AnchorDock> dir, Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                dir.Add(control, new AnchorDock(control));
                if (controls.Count > 0) DpiSuspend(ref dir, control.Controls);
            }
        }


        internal static void DpiResume(Dictionary<Control, AnchorDock> dir, Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                if (dir.TryGetValue(control, out var find))
                {
                    control.Dock = find.Dock;
                    control.Anchor = find.Anchor;
                }
                if (controls.Count > 0) DpiResume(dir, control.Controls);
            }
        }


        internal static void DpiLS(float dpi, Control control)
        {
            var size = new Size((int)(control.Width * dpi), (int)(control.Height * dpi));
            Point point;
            if (control is Form)
            {
                var screen = Screen.FromPoint(control.Location);
                if (size.Width > screen.WorkingArea.Width && size.Height > screen.WorkingArea.Height)
                {
                    if (control.Width > screen.WorkingArea.Width && control.Height > screen.WorkingArea.Height)
                    {
                        size = screen.WorkingArea.Size;
                        point = screen.WorkingArea.Location;
                    }
                    else
                    {
                        size = control.Size;
                        point = control.Location;
                    }
                }
                else
                {
                    if (size.Width > screen.WorkingArea.Width) size.Width = screen.WorkingArea.Width;
                    if (size.Height > screen.WorkingArea.Height) size.Height = screen.WorkingArea.Height;

                    point = new Point(control.Left + (control.Width - size.Width) / 2, control.Top + (control.Height - size.Height) / 2);
                }
            }
            else point = new Point((int)(control.Left * dpi), (int)(control.Top * dpi));

            if (!control.MinimumSize.IsEmpty) control.MinimumSize = new Size((int)(control.MinimumSize.Width * dpi), (int)(control.MinimumSize.Height * dpi));
            if (!control.MaximumSize.IsEmpty) control.MaximumSize = new Size((int)(control.MaximumSize.Width * dpi), (int)(control.MaximumSize.Height * dpi));
            control.Padding = SetPadding(dpi, control.Padding);
            control.Margin = SetPadding(dpi, control.Margin);
            control.Size = size;
            control.Location = point;
            if (control.Controls.Count > 0)
            {
                if (control is Pagination || control is Input) return;
                foreach (Control it in control.Controls)
                {
                    DpiLS(dpi, it);
                }
            }
        }
        internal static Padding SetPadding(float dpi, Padding padding)
        {
            if (padding.All == 0) return padding;
            else if (padding.All > 0) return new Padding((int)(padding.All * dpi));
            else return new Padding((int)(padding.Left * dpi), (int)(padding.Top * dpi), (int)(padding.Right * dpi), (int)(padding.Bottom * dpi));
        }

        #endregion
    }

    internal class AnchorDock
    {
        public AnchorDock(Control control)
        {
            Dock = control.Dock;
            Anchor = control.Anchor;
            control.Dock = DockStyle.None;
            control.Anchor = AnchorStyles.Left | AnchorStyles.Top;
        }
        public DockStyle Dock { get; set; }
        public AnchorStyles Anchor { get; set; }
    }

    public class RectTextLR
    {
        public Rectangle text { get; set; }
        public Rectangle l { get; set; }
        public Rectangle r { get; set; }
    }
}