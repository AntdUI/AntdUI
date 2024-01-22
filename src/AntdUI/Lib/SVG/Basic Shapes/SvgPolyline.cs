// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace AntdUI.Svg
{
    /// <summary>
    /// SvgPolyline defines a set of connected straight line segments. Typically, <see cref="SvgPolyline"/> defines open shapes.
    /// </summary>
    [SvgElement("polyline")]
    public class SvgPolyline : SvgPolygon
    {
        private GraphicsPath _Path;
        public override GraphicsPath Path(ISvgRenderer renderer)
        {
            if ((_Path == null || this.IsPathDirty) && base.StrokeWidth > 0)
            {
                _Path = new GraphicsPath();

                try
                {
                    for (int i = 0; (i + 1) < Points.Count; i += 2)
                    {
                        PointF endPoint = new PointF(Points[i].ToDeviceValue(renderer, UnitRenderingType.Horizontal, this),
                                                     Points[i + 1].ToDeviceValue(renderer, UnitRenderingType.Vertical, this));

                        if (renderer == null)
                        {
                            var radius = base.StrokeWidth / 2;
                            _Path.AddEllipse(endPoint.X - radius, endPoint.Y - radius, 2 * radius, 2 * radius);
                            continue;
                        }

                        // TODO: Remove unrequired first line
                        if (_Path.PointCount == 0)
                        {
                            _Path.AddLine(endPoint, endPoint);
                        }
                        else
                        {
                            _Path.AddLine(_Path.GetLastPoint(), endPoint);
                        }
                    }
                }
                catch (Exception exc)
                {
                    Trace.TraceError("Error rendering points: " + exc.Message);
                }
                if (renderer != null)
                    this.IsPathDirty = false;
            }
            return _Path;
        }
    }
}