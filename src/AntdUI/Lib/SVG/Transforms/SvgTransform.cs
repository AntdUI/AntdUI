// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System;
using System.Drawing.Drawing2D;

namespace AntdUI.Svg.Transforms
{
    public abstract class SvgTransform : ICloneable
    {
        public abstract Matrix Matrix(float w, float h);
        public abstract string WriteToString();

        public abstract object Clone();

        public override string ToString()
        {
            return WriteToString();
        }
    }
}