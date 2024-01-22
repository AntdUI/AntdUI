// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System.Drawing.Drawing2D;

namespace AntdUI.Svg
{
    /// <summary>
    /// Defines the methods and properties required for an SVG element to be styled.
    /// </summary>
    public interface ISvgStylable
    {
        SvgPaintServer Fill { get; set; }
        SvgPaintServer Stroke { get; set; }
        SvgFillRule FillRule { get; set; }
        float Opacity { get; set; }
        float FillOpacity { get; set; }
        float StrokeOpacity { get; set; }
        SvgUnit StrokeWidth { get; set; }
        SvgStrokeLineCap StrokeLineCap { get; set; }
        SvgStrokeLineJoin StrokeLineJoin { get; set; }
        float StrokeMiterLimit { get; set; }
        SvgUnitCollection StrokeDashArray { get; set; }
        SvgUnit StrokeDashOffset { get; set; }
        GraphicsPath Path(ISvgRenderer renderer);
    }
}