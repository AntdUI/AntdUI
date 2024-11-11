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

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace AntdUI.Core
{
    public class CanvasGDI : Canvas
    {
        Graphics g;
        public CanvasGDI(Graphics gdi)
        {
            g = gdi;
        }

        #region MeasureString

        public Size MeasureString(string? text, Font font) => g.MeasureString(text, font).Size();
        public Size MeasureString(string? text, Font font, int width) => g.MeasureString(text, font, width).Size();
        public Size MeasureString(string? text, Font font, int width, StringFormat? format) => g.MeasureString(text, font, width, format).Size();

        #endregion

        #region String

        public void String(string? text, Font font, Color color, Rectangle rect, StringFormat? format = null)
        {
            CorrectionTextRendering.CORE(font, text, ref rect);
            using (var brush = new SolidBrush(color))
            {
                String(text, font, brush, rect, format);
            }
        }

        public void String(string? text, Font font, Brush brush, Rectangle rect, StringFormat? format = null)
        {
            CorrectionTextRendering.CORE(font, text, ref rect);
            g.DrawString(text, font, brush, rect, format);
        }

        public void String(string? text, Font font, Brush brush, float x, float y) => g.DrawString(text, font, brush, x, y);

        #endregion

        #region Image

        public void Image(Image image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit, ImageAttributes? imageAttr)
        {
            try
            {
                g.DrawImage(image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit, imageAttr, null);
            }
            catch { }
        }

        public void Image(Image image, float x, float y, float w, float h)
        {
            try
            {
                g.DrawImage(image, x, y, w, h);
            }
            catch { }
        }

        public void Image(Image image, int srcX, int srcY, int srcWidth, int srcHeight)
        {
            try
            {
                g.DrawImage(image, srcX, srcY, srcWidth, srcHeight);
            }
            catch { }
        }
        public void Image(Bitmap image, Rectangle rect)
        {
            try
            {
                g.DrawImage(image, rect);
            }
            catch { }
        }
        public void Icon(System.Drawing.Icon icon, Rectangle rect)
        {
            try
            {
                g.DrawIcon(icon, rect);
            }
            catch { }
        }

        public void Image(Image image, Rectangle rect)
        {
            try
            {
                g.DrawImage(image, rect);
            }
            catch { }
        }
        public void Image(Image image, RectangleF rect)
        {
            try
            {
                g.DrawImage(image, rect);
            }
            catch { }
        }
        public void Image(Image image, RectangleF destRect, RectangleF srcRect, GraphicsUnit srcUnit)
        {
            try
            {
                g.DrawImage(image, destRect, srcRect, srcUnit);
            }
            catch { }
        }

        #region 图片透明度

        public void Image(Bitmap bmp, Rectangle rect, float opacity)
        {
            try
            {
                if (opacity >= 1F)
                {
                    g.DrawImage(bmp, rect);
                    return;
                }
                using (var attributes = new ImageAttributes())
                {
                    var matrix = new ColorMatrix { Matrix33 = opacity };
                    attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                    g.DrawImage(bmp, rect, 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, attributes);
                }
            }
            catch { }
        }

        public void Image(Image bmp, Rectangle rect, float opacity)
        {
            try
            {
                if (opacity >= 1F)
                {
                    g.DrawImage(bmp, rect);
                    return;
                }
                using (var attributes = new ImageAttributes())
                {
                    var matrix = new ColorMatrix { Matrix33 = opacity };
                    attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                    g.DrawImage(bmp, rect, 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, attributes);
                }
            }
            catch { }
        }

        #endregion

        public void Image(RectangleF rect, Image image, TFit fit)
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
                    PaintImgContain(this, image, rect);
                    break;
                case TFit.Cover:
                    PaintImgCover(this, image, rect);
                    break;
            }
        }
        public void Image(RectangleF rect, Image image, TFit fit, float radius, bool round)
        {
            try
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
                else PaintImg(this, rect, image, fit);
            }
            catch { }
        }
        public void Image(RectangleF rect, Image image, TFit fit, float radius, TShape shape)
        {
            try
            {
                if (shape == TShape.Circle || shape == TShape.Round || radius > 0)
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
                            if (shape == TShape.Circle) g.FillEllipse(brush, rect);
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
                else PaintImg(this, rect, image, fit);
            }
            catch { }
        }

        static void PaintImg(Canvas g, RectangleF rect, Image image, TFit fit)
        {
            switch (fit)
            {
                case TFit.Fill:
                    g.Image(image, rect);
                    break;
                case TFit.None:
                    g.Image(image, new RectangleF(rect.X + (rect.Width - image.Width) / 2, rect.Y + (rect.Height - image.Height) / 2, image.Width, image.Height));
                    break;
                case TFit.Contain:
                    PaintImgContain(g, image, rect);
                    break;
                case TFit.Cover:
                    PaintImgCover(g, image, rect);
                    break;
            }
        }
        static void PaintImgCover(Canvas g, Image image, RectangleF rect)
        {
            float originWidth = image.Width, originHeight = image.Height;
            if (originWidth == originHeight)
            {
                if (rect.Width == rect.Height) g.Image(image, rect);
                else if (rect.Width > rect.Height) g.Image(image, new RectangleF(0, (rect.Height - rect.Width) / 2, rect.Width, rect.Width));
                else g.Image(image, new RectangleF((rect.Width - rect.Height) / 2, 0, rect.Height, rect.Height));
                return;
            }
            float destWidth = rect.Width, destHeight = rect.Height;
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
            g.Image(image, new RectangleF(rect.X + (destWidth - currentWidth) / 2, rect.Y + (destHeight - currentHeight) / 2, currentWidth, currentHeight), new RectangleF(0, 0, originWidth, originHeight), GraphicsUnit.Pixel);
        }
        static void PaintImgContain(Canvas g, Image image, RectangleF rect)
        {
            float originWidth = image.Width, originHeight = image.Height;
            if (originWidth == originHeight)
            {
                if (rect.Width == rect.Height) g.Image(image, rect);
                else if (rect.Width > rect.Height) g.Image(image, new RectangleF((rect.Width - rect.Height) / 2, 0, rect.Height, rect.Height));
                else g.Image(image, new RectangleF(0, (rect.Height - rect.Width) / 2, rect.Width, rect.Width));
                return;
            }
            float destWidth = rect.Width, destHeight = rect.Height;
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
            g.Image(image, new RectangleF(rect.X + (destWidth - currentWidth) / 2, rect.Y + (destHeight - currentHeight) / 2, currentWidth, currentHeight), new RectangleF(0, 0, originWidth, originHeight), GraphicsUnit.Pixel);
        }

        #endregion

        #region Fill

        public void Fill(Brush brush, GraphicsPath path) => g.FillPath(brush, path);
        public void Fill(Brush brush, Rectangle rect) => g.FillRectangle(brush, rect);
        public void Fill(Brush brush, int x, int y, int w, int h) => g.FillRectangle(brush, x, y, w, h);
        public void Fill(Brush brush, RectangleF rect) => g.FillRectangle(brush, rect);
        public void Fill(Color color, GraphicsPath path)
        {
            using (var brush = new SolidBrush(color))
            {
                Fill(brush, path);
            }
        }
        public void Fill(Color color, Rectangle rect)
        {
            using (var brush = new SolidBrush(color))
            {
                Fill(brush, rect);
            }
        }
        public void Fill(Color color, RectangleF rect)
        {
            using (var brush = new SolidBrush(color))
            {
                Fill(brush, rect);
            }
        }

        public void FillEllipse(Brush brush, Rectangle rect) => g.FillEllipse(brush, rect);
        public void FillEllipse(Brush brush, RectangleF rect) => g.FillEllipse(brush, rect);
        public void FillEllipse(Color color, Rectangle rect)
        {
            using (var brush = new SolidBrush(color))
            {
                FillEllipse(brush, rect);
            }
        }
        public void FillEllipse(Color color, RectangleF rect)
        {
            using (var brush = new SolidBrush(color))
            {
                FillEllipse(brush, rect);
            }
        }

        public void FillPolygon(Brush brush, Point[] points) => g.FillPolygon(brush, points);
        public void FillPolygon(Brush brush, PointF[] points) => g.FillPolygon(brush, points);
        public void FillPolygon(Color color, PointF[] points)
        {
            using (var brush = new SolidBrush(color))
            {
                FillPolygon(brush, points);
            }
        }

        public void FillPie(Brush brush, Rectangle rect, float startAngle, float sweepAngle) => g.FillPie(brush, rect, startAngle, sweepAngle);
        public void FillPie(Brush brush, float x, float y, float w, float h, float startAngle, float sweepAngle) => g.FillPie(brush, x, y, w, h, startAngle, sweepAngle);

        #endregion

        #region Draw

        public void Draw(Pen pen, GraphicsPath path) => g.DrawPath(pen, path);
        public void Draw(Pen pen, Rectangle rect) => g.DrawRectangle(pen, rect);

        public void Draw(Brush brush, float width, GraphicsPath path)
        {
            using (var pen = new Pen(brush, width))
            {
                Draw(pen, path);
            }
        }
        public void Draw(Color color, float width, GraphicsPath path)
        {
            using (var pen = new Pen(color, width))
            {
                Draw(pen, path);
            }
        }
        public void Draw(Color color, float width, Rectangle rect)
        {
            using (var pen = new Pen(color, width))
            {
                Draw(pen, rect);
            }
        }
        public void Draw(Color color, float width, DashStyle dashStyle, GraphicsPath path)
        {
            using (var pen = new Pen(color, width))
            {
                pen.DashStyle = dashStyle;
                Draw(pen, path);
            }
        }

        public void DrawEllipse(Pen pen, Rectangle rect) => g.DrawEllipse(pen, rect);
        public void DrawEllipse(Pen pen, RectangleF rect) => g.DrawEllipse(pen, rect);
        public void DrawEllipse(Color color, float width, RectangleF rect)
        {
            using (var pen = new Pen(color, width))
            {
                g.DrawEllipse(pen, rect);
            }
        }

        public void DrawPolygon(Pen pen, Point[] points) => g.DrawPolygon(pen, points);
        public void DrawPolygon(Pen pen, PointF[] points) => g.DrawPolygon(pen, points);

        public void DrawArc(Pen pen, Rectangle rect, float startAngle, float sweepAngle)
        {
            try
            {
                g.DrawArc(pen, rect, startAngle, sweepAngle);
            }
            catch { }
        }
        public void DrawArc(Pen pen, RectangleF rect, float startAngle, float sweepAngle)
        {
            try
            {
                g.DrawArc(pen, rect, startAngle, sweepAngle);
            }
            catch { }
        }

        public void DrawPie(Pen pen, Rectangle rect, float startAngle, float sweepAngle) => g.DrawPie(pen, rect, startAngle, sweepAngle);

        public void DrawLine(Pen pen, Point pt1, Point pt2) => g.DrawLine(pen, pt1, pt2);
        public void DrawLine(Pen pen, PointF pt1, PointF pt2) => g.DrawLine(pen, pt1, pt2);
        public void DrawLine(Pen pen, float x, float y, float x2, float y2) => g.DrawLine(pen, x, y, x2, y2);

        public void DrawLines(Pen pen, Point[] points) => g.DrawLines(pen, points);
        public void DrawLines(Pen pen, PointF[] points) => g.DrawLines(pen, points);
        public void DrawLines(Color color, float width, PointF[] points)
        {
            using (var pen = new Pen(color, width))
            {
                DrawLines(pen, points);
            }
        }


        #endregion

        #region Base

        public GraphicsState Save() => g.Save();
        public void Restore(GraphicsState state) => g.Restore(state);
        public void SetClip(Rectangle rect) => g.SetClip(rect);
        public void SetClip(RectangleF rect) => g.SetClip(rect);
        public void SetClip(GraphicsPath path) => g.SetClip(path);
        public void ResetClip() => g.ResetClip();
        public void ResetTransform() => g.ResetTransform();
        public void TranslateTransform(float dx, float dy) => g.TranslateTransform(dx, dy);
        public void RotateTransform(float angle) => g.RotateTransform(angle);
        public float DpiX => g.DpiX;
        public float DpiY => g.DpiY;
        public CompositingMode CompositingMode
        {
            get => g.CompositingMode;
            set => g.CompositingMode = value;
        }
        public void Dispose() => g.Dispose();

        #endregion
    }
}