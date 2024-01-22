// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System;

namespace AntdUI.Svg
{
    /// <summary>
    /// Specifies the SVG name of an <see cref="SvgElement"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class SvgElementAttribute : Attribute
    {
        /// <summary>
        /// Gets the name of the SVG element.
        /// </summary>
        public string ElementName { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgElementAttribute"/> class with the specified element name;
        /// </summary>
        /// <param name="elementName">The name of the SVG element.</param>
        public SvgElementAttribute(string elementName)
        {
            this.ElementName = elementName;
        }
    }
}