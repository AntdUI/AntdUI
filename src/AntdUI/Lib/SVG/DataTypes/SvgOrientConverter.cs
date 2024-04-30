// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System;

namespace AntdUI.Svg.DataTypes
{
    internal class SvgOrientConverter
    {
        public static SvgOrient Parse(string value)
        {
            if (value == null) return new SvgOrient(0.0f);
            switch (value)
            {
                case "auto":
                    return new SvgOrient(true);
                default:
                    if (!float.TryParse(value.ToString(), out var fTmp)) throw new ArgumentOutOfRangeException("value must be a valid float.");
                    return new SvgOrient(fTmp);
            }
        }
    }
}