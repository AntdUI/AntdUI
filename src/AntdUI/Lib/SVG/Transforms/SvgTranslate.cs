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

        public override Matrix Matrix(float w, float h)
        {
            Matrix matrix = new Matrix();
            if (RX || RY)
            {
                if (RX && RY) matrix.Translate(w * (X / 100F), h * (Y / 100F));
                else if (RX) matrix.Translate(w * (X / 100F), Y);
                else matrix.Translate(X, h * (Y / 100F));
            }
            else matrix.Translate(X, Y);
            return matrix;
        }

        public override string WriteToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "translate({0}, {1})", X, Y);
        }

        public SvgTranslate(float _x, bool ratio_x, float _y, bool ratio_y)
        {
            x = _x;
            y = _y;
            RX = ratio_x;
            RY = ratio_y;
        }

        bool RX = false, RY = false;

        public override object Clone()
        {
            return new SvgTranslate(x, RX, y, RY);
        }
    }
}