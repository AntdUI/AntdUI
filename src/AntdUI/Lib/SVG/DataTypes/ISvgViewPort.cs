// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

namespace AntdUI.Svg
{
    /// <summary>
    /// Provides properties and methods to be implemented by view port elements.
    /// </summary>
    public interface ISvgViewPort
    {
        /// <summary>
        /// Gets or sets the viewport of the element.
        /// </summary>
        SvgViewBox ViewBox { get; set; }
        SvgAspectRatio AspectRatio { get; set; }
        SvgOverflow Overflow { get; set; }
    }
}
