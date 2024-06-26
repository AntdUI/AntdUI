﻿// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace AntdUI.Svg
{
    public interface ISvgRenderer : IDisposable
    {
        float DpiY { get; }
        void DrawImage(Image image, RectangleF destRect, RectangleF srcRect, GraphicsUnit graphicsUnit);
        void DrawImageUnscaled(Image image, Point location);
        void DrawPath(Pen pen, GraphicsPath path);
        void FillPath(Brush brush, GraphicsPath path);
        ISvgBoundable GetBoundable();
        Region GetClip();
        ISvgBoundable PopBoundable();
        void RotateTransform(float fAngle, MatrixOrder order = MatrixOrder.Append);
        void ScaleTransform(float sx, float sy, MatrixOrder order = MatrixOrder.Append);
        void SetBoundable(ISvgBoundable boundable);
        void SetClip(Region region, CombineMode combineMode = CombineMode.Replace);
        SmoothingMode SmoothingMode { get; set; }
        Matrix Transform { get; set; }
        void TranslateTransform(float dx, float dy, MatrixOrder order = MatrixOrder.Append);
        void DrawImage(Image image, RectangleF destRect, RectangleF srcRect, GraphicsUnit graphicsUnit, float opacity);
    }
}