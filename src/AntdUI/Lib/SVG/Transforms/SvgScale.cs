// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System.Globalization;

namespace AntdUI.Svg.Transforms
{
    public sealed class SvgScale : SvgTransform
    {
        private float scaleFactorX;
        private float scaleFactorY;

        public float X
        {
            get { return scaleFactorX; }
            set { scaleFactorX = value; }
        }

        public float Y
        {
            get { return scaleFactorY; }
            set { scaleFactorY = value; }
        }

        public override System.Drawing.Drawing2D.Matrix Matrix
        {
            get
            {
                var matrix = new System.Drawing.Drawing2D.Matrix();
                matrix.Scale(X, Y);
                return matrix;
            }
        }

        public override string WriteToString()
        {
            if (X == Y) return string.Format(CultureInfo.InvariantCulture, "scale({0})", X);
            return string.Format(CultureInfo.InvariantCulture, "scale({0}, {1})", X, Y);
        }

        public SvgScale(float x) : this(x, x) { }

        public SvgScale(float x, float y)
        {
            scaleFactorX = x;
            scaleFactorY = y;
        }

        public override object Clone()
        {
            return new SvgScale(X, Y);
        }
    }
}