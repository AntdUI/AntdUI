// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

namespace AntdUI.Svg
{
    /// <summary>Indicates how the user agent should determine the spacing between glyphs that are to be rendered along a path.</summary>
    public enum SvgTextPathSpacing
    {
        /// <summary>Indicates that the glyphs should be rendered exactly according to the spacing rules as specified in Text on a path layout rules.</summary>
        Exact,

        /// <summary>Indicates that the user agent should use text-on-a-path layout algorithms to adjust the spacing between glyphs in order to achieve visually appealing results.</summary>
        Auto
    }
}