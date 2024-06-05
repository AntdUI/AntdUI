// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System.Drawing.Drawing2D;
using System.Globalization;

namespace AntdUI.Svg.Transforms
{
    public sealed class SvgRotate : SvgTransform
    {
        public float Angle { get; set; }

        public float CenterX { get; set; }

        public float CenterY { get; set; }

        public override Matrix Matrix(float w, float h)
        {
            Matrix matrix = new Matrix();
            matrix.Translate(CenterX, CenterY);
            matrix.Rotate(Angle);
            matrix.Translate(-CenterX, -CenterY);
            return matrix;
        }

        public override string WriteToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "rotate({0}, {1}, {2})", Angle, CenterX, CenterY);
        }

        public SvgRotate(float angle)
        {
            Angle = angle;
        }

        public SvgRotate(float angle, float centerX, float centerY)
            : this(angle)
        {
            CenterX = centerX;
            CenterY = centerY;
        }


        public override object Clone()
        {
            return new SvgRotate(Angle, CenterX, CenterY);
        }
    }
}