// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System;

namespace AntdUI.Svg
{
    public class SvgFontFaceUri : SvgElement
    {
        public override string ClassName { get => "font-face-uri"; }

        private Uri _referencedElement;

        [SvgAttribute("href", SvgAttributeAttribute.XLinkNamespace)]
        public virtual Uri ReferencedElement
        {
            get { return _referencedElement; }
            set { _referencedElement = value; }
        }
    }
}