// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System;

namespace AntdUI.Svg
{
    /// <summary>This property describes transformations that are added to the text of an element.</summary>
    [Flags]
    public enum SvgTextTransformation
    {
        /// <summary>The value is inherited from the parent element.</summary>
        Inherit = 0,

        /// <summary>The text is not transformed.</summary>
        None = 1,

        /// <summary>First letter of each word of the text is converted to uppercase.</summary>
        Capitalize = 2,

        /// <summary>The text is converted to uppercase.</summary>
        Uppercase = 4,

        /// <summary>The text is converted to lowercase.</summary>
        Lowercase = 8
    }
}