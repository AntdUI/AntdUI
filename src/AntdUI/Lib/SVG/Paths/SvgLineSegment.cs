// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System.Drawing;

namespace AntdUI.Svg.Pathing
{
    public sealed class SvgLineSegment : SvgPathSegment
    {
        public SvgLineSegment(PointF start, PointF end)
        {
            Start = start;
            End = end;
        }

        public override void AddToPath(System.Drawing.Drawing2D.GraphicsPath graphicsPath)
        {
            graphicsPath.AddLine(Start, End);
        }

        public override string ToString()
        {
            return "L" + End.ToSvgString();
        }
    }
}