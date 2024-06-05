// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System.Drawing.Drawing2D;
using System.Globalization;

namespace AntdUI.Svg.Transforms
{
    /// <summary>
    /// The class which applies the specified shear vector to this Matrix.
    /// </summary>
    public sealed class SvgShear : SvgTransform
    {
        private float shearFactorX;
        private float shearFactorY;

        public float X
        {
            get { return shearFactorX; }
            set { shearFactorX = value; }
        }

        public float Y
        {
            get { return shearFactorY; }
            set { shearFactorY = value; }
        }

        public override Matrix Matrix(float w, float h)
        {
            Matrix matrix = new Matrix();
            matrix.Shear(X, Y);
            return matrix;
        }

        public override string WriteToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "shear({0}, {1})", X, Y);
        }

        public SvgShear(float x) : this(x, x) { }

        public SvgShear(float x, float y)
        {
            shearFactorX = x;
            shearFactorY = y;
        }


        public override object Clone()
        {
            return new SvgShear(X, Y);
        }
    }
}