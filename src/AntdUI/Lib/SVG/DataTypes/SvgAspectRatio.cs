// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using AntdUI.Svg.DataTypes;
using System.ComponentModel;

namespace AntdUI.Svg
{
    /// <summary>
    /// Description of SvgAspectRatio.
    /// </summary>
    [TypeConverter(typeof(SvgPreserveAspectRatioConverter))]
    public class SvgAspectRatio
    {
        public SvgAspectRatio() : this(SvgPreserveAspectRatio.none)
        {
        }

        public SvgAspectRatio(SvgPreserveAspectRatio align)
            : this(align, false)
        {
        }

        public SvgAspectRatio(SvgPreserveAspectRatio align, bool slice)
            : this(align, slice, false)
        {
        }

        public SvgAspectRatio(SvgPreserveAspectRatio align, bool slice, bool defer)
        {
            Align = align;
            Slice = slice;
            Defer = defer;
        }

        public SvgPreserveAspectRatio Align { get; set; }

        public bool Slice { get; set; }

        public bool Defer { get; set; }

        public override string ToString()
        {
            return TypeDescriptor.GetConverter(typeof(SvgPreserveAspectRatio)).ConvertToString(Align) + (Slice ? " slice" : "");
        }

    }

    [TypeConverter(typeof(SvgPreserveAspectRatioConverter))]
    public enum SvgPreserveAspectRatio
    {
        xMidYMid, //default
        none,
        xMinYMin,
        xMidYMin,
        xMaxYMin,
        xMinYMid,
        xMaxYMid,
        xMinYMax,
        xMidYMax,
        xMaxYMax
    }
}