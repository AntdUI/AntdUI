// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System.ComponentModel;

namespace AntdUI.Svg
{
    /// <summary>Specifies the shape to be used at the corners of paths or basic shapes when they are stroked.</summary>
    [TypeConverter(typeof(SvgStrokeLineJoinConverter))]
    public enum SvgStrokeLineJoin
    {
        /// <summary>The value is inherited from the parent element.</summary>
        Inherit,

        /// <summary>The corners of the paths are joined sharply.</summary>
        Miter,

        /// <summary>The corners of the paths are rounded off.</summary>
        Round,

        /// <summary>The corners of the paths are "flattened".</summary>
        Bevel
    }
}
