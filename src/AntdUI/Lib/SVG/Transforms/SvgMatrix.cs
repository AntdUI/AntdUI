// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Globalization;

namespace AntdUI.Svg.Transforms
{
    /// <summary>
    /// The class which applies custom transform to this Matrix (Required for projects created by the Inkscape).
    /// </summary>
    public sealed class SvgMatrix : SvgTransform
    {
        private List<float> points;
        public List<float> Points
        {
            get { return points; }
            set { points = value; }
        }

        public override Matrix Matrix(float w, float h)
        {
            Matrix matrix = new Matrix(
                points[0],
                points[1],
                points[2],
                points[3],
                points[4],
                points[5]
            );
            return matrix;
        }

        public override string WriteToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "matrix({0}, {1}, {2}, {3}, {4}, {5})",
                points[0], points[1], points[2], points[3], points[4], points[5]);
        }

        public SvgMatrix(List<float> m)
        {
            points = m;
        }


        public override object Clone()
        {
            return new SvgMatrix(Points);
        }

    }
}