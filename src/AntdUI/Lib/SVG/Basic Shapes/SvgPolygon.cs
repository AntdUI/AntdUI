// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System.Diagnostics;
using System.Drawing.Drawing2D;

namespace AntdUI.Svg
{
    /// <summary>
    /// SvgPolygon defines a closed shape consisting of a set of connected straight line segments.
    /// </summary>
    public class SvgPolygon : SvgMarkerElement
    {
        public override string ClassName { get => "polygon"; }

        private GraphicsPath _path;

        /// <summary>
        /// The points that make up the SvgPolygon
        /// </summary>
        [SvgAttribute("points")]
        public SvgPointCollection Points
        {
            get { return Attributes.GetAttribute<SvgPointCollection>("points"); }
            set { Attributes["points"] = value; IsPathDirty = true; }
        }

        public override GraphicsPath Path(ISvgRenderer renderer)
        {
            if (_path == null || IsPathDirty)
            {
                _path = new GraphicsPath();
                _path.StartFigure();

                try
                {
                    var points = Points;
                    for (int i = 2; (i + 1) < points.Count; i += 2)
                    {
                        var endPoint = SvgUnit.GetDevicePoint(points[i], points[i + 1], renderer, this);

                        // If it is to render, don't need to consider stroke width.
                        // i.e stroke width only to be considered when calculating boundary
                        if (renderer == null)
                        {
                            var radius = base.StrokeWidth * 2;
                            _path.AddEllipse(endPoint.X - radius, endPoint.Y - radius, 2 * radius, 2 * radius);
                            continue;
                        }

                        //first line
                        if (_path.PointCount == 0)
                        {
                            _path.AddLine(SvgUnit.GetDevicePoint(points[i - 2], points[i - 1], renderer, this), endPoint);
                        }
                        else
                        {
                            _path.AddLine(_path.GetLastPoint(), endPoint);
                        }
                    }
                }
                catch
                {
                    Trace.TraceError("Error parsing points");
                }

                _path.CloseFigure();
                if (renderer != null)
                    IsPathDirty = false;
            }
            return _path;
        }
    }
}