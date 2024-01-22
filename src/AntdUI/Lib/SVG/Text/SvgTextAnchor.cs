// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System.ComponentModel;

namespace AntdUI.Svg
{
    /// <summary>
    /// Text anchor is used to align (start-, middle- or end-alignment) a string of text relative to a given point.
    /// </summary>
    [TypeConverter(typeof(SvgTextAnchorConverter))]
    public enum SvgTextAnchor
    {
        /// <summary>The value is inherited from the parent element.</summary>
        Inherit,
        /// <summary>
        /// The rendered characters are aligned such that the start of the text string is at the initial current text position.
        /// </summary>
        Start,
        /// <summary>
        /// The rendered characters are aligned such that the middle of the text string is at the current text position.
        /// </summary>
        Middle,
        /// <summary>
        /// The rendered characters are aligned such that the end of the text string is at the initial current text position.
        /// </summary>
        End
    }
}