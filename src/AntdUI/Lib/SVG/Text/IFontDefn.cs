// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace AntdUI.Svg
{
    public interface IFontDefn : IDisposable
    {
        float Size { get; }
        float SizeInPoints { get; }
        void AddStringToPath(ISvgRenderer renderer, GraphicsPath path, string text, PointF location);
        float Ascent(ISvgRenderer renderer);
        IList<RectangleF> MeasureCharacters(ISvgRenderer renderer, string text);
        SizeF MeasureString(ISvgRenderer renderer, string text);
    }
}
