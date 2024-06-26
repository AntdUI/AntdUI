// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

namespace AntdUI.Svg
{
    public class NonSvgElement : SvgElement
    {
        public NonSvgElement()
        {
        }

        public NonSvgElement(string elementName)
        {
            ElementName = elementName;
        }

        /// <summary>
        /// Publish the element name to be able to differentiate non-svg elements.
        /// </summary>
        public string Name
        {
            get
            {
                return ElementName;
            }
        }
    }
}