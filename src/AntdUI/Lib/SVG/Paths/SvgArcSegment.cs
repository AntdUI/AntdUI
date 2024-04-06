// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace AntdUI.Svg.Pathing
{
    public sealed class SvgArcSegment : SvgPathSegment
    {
        private const double RadiansPerDegree = Math.PI / 180.0;
        private const double DoublePI = Math.PI * 2;

        public float RadiusX { get; set; }
        public float RadiusY { get; set; }

        public float Angle { get; set; }

        public SvgArcSweep Sweep { get; set; }
        public SvgArcSize Size { get; set; }

        public SvgArcSegment(PointF start, float radiusX, float radiusY, float angle, SvgArcSize size, SvgArcSweep sweep, PointF end)
            : base(start, end)
        {
            RadiusX = Math.Abs(radiusX);
            RadiusY = Math.Abs(radiusY);
            Angle = angle;
            Sweep = sweep;
            Size = size;
        }

        private static double CalculateVectorAngle(double ux, double uy, double vx, double vy)
        {
            double ta = Math.Atan2(uy, ux);
            double tb = Math.Atan2(vy, vx);

            if (tb >= ta) return tb - ta;

            return DoublePI - (ta - tb);
        }

        public override void AddToPath(GraphicsPath graphicsPath)
        {
            if (Start == End) return;
            if (RadiusX == 0.0f && RadiusY == 0.0f)
            {
                graphicsPath.AddLine(Start, End);
                return;
            }

            double sinPhi = Math.Sin(Angle * RadiansPerDegree);
            double cosPhi = Math.Cos(Angle * RadiansPerDegree);

            double x1dash = cosPhi * (Start.X - End.X) / 2.0 + sinPhi * (Start.Y - End.Y) / 2.0;
            double y1dash = -sinPhi * (Start.X - End.X) / 2.0 + cosPhi * (Start.Y - End.Y) / 2.0;

            double root;
            double numerator = RadiusX * RadiusX * RadiusY * RadiusY - RadiusX * RadiusX * y1dash * y1dash - RadiusY * RadiusY * x1dash * x1dash;

            float rx = RadiusX;
            float ry = RadiusY;

            if (numerator < 0.0)
            {
                float s = (float)Math.Sqrt(1.0 - numerator / (RadiusX * RadiusX * RadiusY * RadiusY));

                rx *= s;
                ry *= s;
                root = 0.0;
            }
            else root = ((Size == SvgArcSize.Large && Sweep == SvgArcSweep.Positive) || (Size == SvgArcSize.Small && Sweep == SvgArcSweep.Negative) ? -1.0 : 1.0) * Math.Sqrt(numerator / (RadiusX * RadiusX * y1dash * y1dash + RadiusY * RadiusY * x1dash * x1dash));

            double cxdash = root * rx * y1dash / ry;
            double cydash = -root * ry * x1dash / rx;

            double cx = cosPhi * cxdash - sinPhi * cydash + (Start.X + End.X) / 2.0;
            double cy = sinPhi * cxdash + cosPhi * cydash + (Start.Y + End.Y) / 2.0;

            double theta1 = CalculateVectorAngle(1.0, 0.0, (x1dash - cxdash) / rx, (y1dash - cydash) / ry);
            double dtheta = CalculateVectorAngle((x1dash - cxdash) / rx, (y1dash - cydash) / ry, (-x1dash - cxdash) / rx, (-y1dash - cydash) / ry);

            if (Sweep == SvgArcSweep.Negative && dtheta > 0)
            {
                dtheta -= 2.0 * Math.PI;
            }
            else if (Sweep == SvgArcSweep.Positive && dtheta < 0)
            {
                dtheta += 2.0 * Math.PI;
            }

            int segments = (int)Math.Ceiling((double)Math.Abs(dtheta / (Math.PI / 2.0)));
            double delta = dtheta / segments;
            double t = 8.0 / 3.0 * Math.Sin(delta / 4.0) * Math.Sin(delta / 4.0) / Math.Sin(delta / 2.0);

            double startX = Start.X;
            double startY = Start.Y;

            for (int i = 0; i < segments; ++i)
            {
                double cosTheta1 = Math.Cos(theta1);
                double sinTheta1 = Math.Sin(theta1);
                double theta2 = theta1 + delta;
                double cosTheta2 = Math.Cos(theta2);
                double sinTheta2 = Math.Sin(theta2);

                double endpointX = cosPhi * rx * cosTheta2 - sinPhi * ry * sinTheta2 + cx;
                double endpointY = sinPhi * rx * cosTheta2 + cosPhi * ry * sinTheta2 + cy;

                double dx1 = t * (-cosPhi * rx * sinTheta1 - sinPhi * ry * cosTheta1);
                double dy1 = t * (-sinPhi * rx * sinTheta1 + cosPhi * ry * cosTheta1);

                double dxe = t * (cosPhi * rx * sinTheta2 + sinPhi * ry * cosTheta2);
                double dye = t * (sinPhi * rx * sinTheta2 - cosPhi * ry * cosTheta2);

                graphicsPath.AddBezier((float)startX, (float)startY, (float)(startX + dx1), (float)(startY + dy1),
                    (float)(endpointX + dxe), (float)(endpointY + dye), (float)endpointX, (float)endpointY);

                theta1 = theta2;
                startX = (float)endpointX;
                startY = (float)endpointY;
            }
        }

        public override string ToString()
        {
            var arcFlag = Size == SvgArcSize.Large ? "1" : "0";
            var sweepFlag = Sweep == SvgArcSweep.Positive ? "1" : "0";
            return "A" + RadiusX.ToString() + " " + RadiusY.ToString() + " " + Angle.ToString() + " " + arcFlag + " " + sweepFlag + " " + End.ToSvgString();
        }
    }

    [Flags]
    public enum SvgArcSweep
    {
        Negative = 0,
        Positive = 1
    }

    [Flags]
    public enum SvgArcSize
    {
        Small = 0,
        Large = 1
    }
}