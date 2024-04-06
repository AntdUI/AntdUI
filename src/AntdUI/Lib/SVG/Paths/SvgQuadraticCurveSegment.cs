// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System.Drawing;

namespace AntdUI.Svg.Pathing
{
    public sealed class SvgQuadraticCurveSegment : SvgPathSegment
    {
        private PointF _controlPoint;

        public PointF ControlPoint
        {
            get { return _controlPoint; }
            set { _controlPoint = value; }
        }

        private PointF FirstControlPoint
        {
            get
            {
                float x1 = Start.X + (ControlPoint.X - Start.X) * 2 / 3;
                float y1 = Start.Y + (ControlPoint.Y - Start.Y) * 2 / 3;

                return new PointF(x1, y1);
            }
        }

        private PointF SecondControlPoint
        {
            get
            {
                float x2 = ControlPoint.X + (End.X - ControlPoint.X) / 3;
                float y2 = ControlPoint.Y + (End.Y - ControlPoint.Y) / 3;

                return new PointF(x2, y2);
            }
        }

        public SvgQuadraticCurveSegment(PointF start, PointF controlPoint, PointF end)
        {
            Start = start;
            _controlPoint = controlPoint;
            End = end;
        }

        public override void AddToPath(System.Drawing.Drawing2D.GraphicsPath graphicsPath)
        {
            graphicsPath.AddBezier(Start, FirstControlPoint, SecondControlPoint, End);
        }

        public override string ToString()
        {
            return "Q" + ControlPoint.ToSvgString() + " " + End.ToSvgString();
        }

    }
}