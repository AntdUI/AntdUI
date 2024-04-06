// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System;
using System.Collections.Generic;
using System.Linq;

namespace AntdUI.Svg
{
    public class SvgTextRef : SvgTextBase
    {
        public override string ClassName { get => "tref"; }

        private Uri _referencedElement;

        [SvgAttribute("href", SvgAttributeAttribute.XLinkNamespace)]
        public virtual Uri ReferencedElement
        {
            get { return _referencedElement; }
            set { _referencedElement = value; }
        }

        internal override IEnumerable<ISvgNode> GetContentNodes()
        {
            var refText = OwnerDocument.IdManager.GetElementById(ReferencedElement) as SvgTextBase;
            IEnumerable<ISvgNode> contentNodes = null;

            if (refText == null)
            {
                contentNodes = base.GetContentNodes();
            }
            else
            {
                contentNodes = refText.GetContentNodes();
            }

            contentNodes = contentNodes.Where(o => !(o is ISvgDescriptiveElement));

            return contentNodes;
        }
    }
}