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
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace AntdUI
{
    public interface Canvas : IDisposable
    {
        #region MeasureString

        Size MeasureString(string? text, Font font);
        Size MeasureString(string? text, Font font, int width);
        Size MeasureString(string? text, Font font, int width, StringFormat? format);

        #endregion

        #region String

        void String(string? text, Font font, Color color, Rectangle rect, StringFormat? format = null);
        void String(string? text, Font font, Brush brush, Rectangle rect, StringFormat? format = null);
        void String(string? text, Font font, Color color, RectangleF rect, StringFormat? format = null);
        void String(string? text, Font font, Brush brush, RectangleF rect, StringFormat? format = null);
        void String(string? text, Font font, Color color, int x, int y);
        void String(string? text, Font font, Brush brush, int x, int y);
        void String(string? text, Font font, Color color, float x, float y);
        void String(string? text, Font font, Brush brush, float x, float y);
        void String(string? text, Font font, Color color, Point point);
        void String(string? text, Font font, Brush brush, Point point);
        void String(string? text, Font font, Color color, PointF point);
        void String(string? text, Font font, Brush brush, PointF point);

        #endregion

        #region Image

        bool Image(Image image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit, ImageAttributes? imageAttr);

        bool Image(Image image, float x, float y, float w, float h);
        bool Image(Image image, int x, int y, int w, int h);
        bool Image(Image image, int x, int y);
        bool Icon(Icon icon, Rectangle rect);

        bool Image(Image image, Rectangle rect);
        bool Image(Image image, RectangleF rect);
        bool Image(Image image, Rectangle destRect, Rectangle srcRect, GraphicsUnit srcUnit);
        bool Image(Image image, RectangleF destRect, RectangleF srcRect, GraphicsUnit srcUnit);

        #region 图片透明度

        bool Image(Image bmp, Rectangle rect, float opacity);

        #endregion

        bool Image(RectangleF rect, Image image, TFit fit, float radius, bool round);
        bool Image(RectangleF rect, Image image, TFit fit, float radius, TShape shape);
        bool Image(RectangleF rect, Image image, TFit fit);

        #endregion

        #region Fill

        void Fill(Brush brush, GraphicsPath path);
        void Fill(Brush brush, Rectangle rect);
        void Fill(Brush brush, RectangleF rect);
        void Fill(Brush brush, int x, int y, int w, int h);
        void Fill(Brush brush, float x, float y, float w, float h);

        void Fill(Color brush, GraphicsPath path);
        void Fill(Color brush, Rectangle rect);
        void Fill(Color brush, RectangleF rect);
        void Fill(Color brush, int x, int y, int w, int h);
        void Fill(Color brush, float x, float y, float w, float h);

        void FillEllipse(Brush brush, Rectangle rect);
        void FillEllipse(Brush brush, RectangleF rect);
        void FillEllipse(Color color, Rectangle rect);
        void FillEllipse(Color color, RectangleF rect);

        void FillPolygon(Brush brush, Point[] points);
        void FillPolygon(Brush brush, PointF[] points);
        void FillPolygon(Color color, Point[] points);
        void FillPolygon(Color color, PointF[] points);

        void FillPie(Brush brush, Rectangle rect, float startAngle, float sweepAngle);
        void FillPie(Brush brush, RectangleF rect, float startAngle, float sweepAngle);
        void FillPie(Brush brush, float x, float y, float w, float h, float startAngle, float sweepAngle);

        #endregion

        #region Draw

        void Draw(Pen pen, GraphicsPath path);
        void Draw(Pen pen, Rectangle rect);
        void Draw(Pen pen, RectangleF rect);
        void Draw(Color color, float width, GraphicsPath path);
        void Draw(Brush brush, float width, GraphicsPath path);
        void Draw(Color color, float width, Rectangle rect);
        void Draw(Color color, float width, RectangleF rect);
        void Draw(Color color, float width, DashStyle dashStyle, GraphicsPath path);
        void Draw(Color color, float width, DashStyle dashStyle, Rectangle rect);
        void Draw(Color color, float width, DashStyle dashStyle, RectangleF rect);

        void DrawEllipse(Pen pen, Rectangle rect);
        void DrawEllipse(Pen pen, RectangleF rect);
        void DrawEllipse(Color color, float width, Rectangle rect);
        void DrawEllipse(Color color, float width, RectangleF rect);

        void DrawPolygon(Pen pen, Point[] points);
        void DrawPolygon(Pen pen, PointF[] points);
        void DrawPolygon(Color color, float width, Point[] points);
        void DrawPolygon(Color color, float width, PointF[] points);

        void DrawArc(Pen pen, Rectangle rect, float startAngle, float sweepAngle);
        void DrawArc(Pen pen, RectangleF rect, float startAngle, float sweepAngle);

        void DrawArc(Color color, float width, Rectangle rect, float startAngle, float sweepAngle);
        void DrawArc(Color color, float width, RectangleF rect, float startAngle, float sweepAngle);

        void DrawPie(Pen pen, Rectangle rect, float startAngle, float sweepAngle);
        void DrawPie(Pen pen, RectangleF rect, float startAngle, float sweepAngle);

        void DrawPie(Color color, float width, Rectangle rect, float startAngle, float sweepAngle);
        void DrawPie(Color color, float width, RectangleF rect, float startAngle, float sweepAngle);

        void DrawLine(Pen pen, Point points, Point points2);
        void DrawLine(Pen pen, PointF points, PointF points2);
        void DrawLine(Pen pen, int x, int y, int x2, int y2);
        void DrawLine(Pen pen, float x, float y, float x2, float y2);
        void DrawLines(Color color, float width, Point[] points);
        void DrawLines(Color color, float width, PointF[] points);

        void DrawLines(Pen pen, Point[] points);
        void DrawLines(Pen pen, PointF[] points);

        #endregion

        #region Base

        GraphicsState Save();
        void Restore(GraphicsState state);
        void SetClip(Rectangle rect);
        void SetClip(RectangleF rect);
        void SetClip(GraphicsPath path);
        void SetClip(Rectangle rect, CombineMode combineMode);
        void SetClip(RectangleF rect, CombineMode combineMode);
        void SetClip(GraphicsPath path, CombineMode combineMode);
        void ResetClip();
        void ResetTransform();
        void TranslateTransform(float dx, float dy);
        void RotateTransform(float angle);
        float DpiX { get; }
        float DpiY { get; }
        Matrix Transform { get; set; }
        CompositingMode CompositingMode { get; set; }

        #endregion
    }
}