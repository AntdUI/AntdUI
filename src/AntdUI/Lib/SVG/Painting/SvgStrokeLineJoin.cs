// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

namespace AntdUI.Svg
{
    /// <summary>Specifies the shape to be used at the corners of paths or basic shapes when they are stroked.</summary>
    public enum SvgStrokeLineJoin
    {
        /// <summary>The value is inherited from the parent element.</summary>
        Inherit,

        /// <summary>The corners of the paths are joined sharply.</summary>
        Miter,

        /// <summary>
        /// The corners of the paths are joined sharply, but clipped at the miter limit instead of
        /// falling back to SvgStrokeLineJoin.Bevel. This is a new value that might not be supported by
        /// most browsers.
        /// </summary>
        MiterClip,

        /// <summary>The corners of the paths are rounded off.</summary>
        Round,

        /// <summary>The corners of the paths are "flattened".</summary>
        Bevel,

        /// <summary>
        /// The corners of the paths are joined by arcs that have the same curvature as the curves they
        /// join. This is a new value that might not be supported by most browsers.
        /// </summary>
        Arcs,

        Square
    }
}