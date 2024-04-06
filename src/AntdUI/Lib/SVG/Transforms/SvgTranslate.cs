// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System.Drawing.Drawing2D;
using System.Globalization;

namespace AntdUI.Svg.Transforms
{
    public sealed class SvgTranslate : SvgTransform
    {
        private float x;
        private float y;

        public float X
        {
            get { return x; }
            set { x = value; }
        }

        public float Y
        {
            get { return y; }
            set { y = value; }
        }

        public override System.Drawing.Drawing2D.Matrix Matrix
        {
            get
            {
                Matrix matrix = new Matrix();
                matrix.Translate(X, Y);
                return matrix;
            }
        }

        public override string WriteToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "translate({0}, {1})", X, Y);
        }

        public SvgTranslate(float _x, float _y)
        {
            x = _x;
            y = _y;
        }

        public SvgTranslate(float x) : this(x, 0.0f)
        { }

        public override object Clone()
        {
            return new SvgTranslate(x, y);
        }
    }
}