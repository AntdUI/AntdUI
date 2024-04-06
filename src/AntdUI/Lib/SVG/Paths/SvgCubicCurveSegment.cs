// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System.Drawing;

namespace AntdUI.Svg.Pathing
{
    public sealed class SvgCubicCurveSegment : SvgPathSegment
    {
        private PointF _firstControlPoint;
        private PointF _secondControlPoint;

        public PointF FirstControlPoint
        {
            get { return _firstControlPoint; }
            set { _firstControlPoint = value; }
        }

        public PointF SecondControlPoint
        {
            get { return _secondControlPoint; }
            set { _secondControlPoint = value; }
        }

        public SvgCubicCurveSegment(PointF start, PointF firstControlPoint, PointF secondControlPoint, PointF end)
        {
            Start = start;
            End = end;
            _firstControlPoint = firstControlPoint;
            _secondControlPoint = secondControlPoint;
        }

        public override void AddToPath(System.Drawing.Drawing2D.GraphicsPath graphicsPath)
        {
            graphicsPath.AddBezier(Start, FirstControlPoint, SecondControlPoint, End);
        }

        public override string ToString()
        {
            return "C" + FirstControlPoint.ToSvgString() + " " + SecondControlPoint.ToSvgString() + " " + End.ToSvgString();
        }
    }
}