// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

namespace AntdUI.Svg
{
    public class SvgUnknownElement : SvgElement
    {
        public SvgUnknownElement()
        {

        }

        public SvgUnknownElement(string elementName)
        {
            this.ElementName = elementName;
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgUnknownElement>();
        }

        public override SvgElement DeepCopy<T>()
        {
            var newObj = base.DeepCopy<T>() as SvgUnknownElement;

            return newObj;
        }
    }
}