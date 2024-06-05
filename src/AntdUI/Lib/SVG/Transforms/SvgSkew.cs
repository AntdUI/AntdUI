﻿// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System;
using System.Drawing.Drawing2D;
using System.Globalization;

namespace AntdUI.Svg.Transforms
{
    /// <summary>
    /// The class which applies the specified skew vector to this Matrix.
    /// </summary>
    public sealed class SvgSkew : SvgTransform
    {
        public float AngleX { get; set; }

        public float AngleY { get; set; }

        public override Matrix Matrix(float w, float h)
        {
            var matrix = new Matrix();
            matrix.Shear(
                (float)Math.Tan(AngleX / 180 * Math.PI),
                (float)Math.Tan(AngleY / 180 * Math.PI));
            return matrix;
        }

        public override string WriteToString()
        {
            if (AngleY == 0) return string.Format(CultureInfo.InvariantCulture, "skewX({0})", AngleX);
            else return string.Format(CultureInfo.InvariantCulture, "skewY({0})", AngleY);
        }

        public SvgSkew(float x, float y)
        {
            AngleX = x;
            AngleY = y;
        }


        public override object Clone()
        {
            return new SvgSkew(AngleX, AngleY);
        }
    }
}