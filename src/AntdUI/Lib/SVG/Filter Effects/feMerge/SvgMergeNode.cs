// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

namespace AntdUI.Svg.FilterEffects
{
    public class SvgMergeNode : SvgElement
    {
        public override string ClassName { get => "feMergeNode"; }

        [SvgAttribute("in")]
        public string Input
        {
            get { return Attributes.GetAttribute<string>("in"); }
            set { Attributes["in"] = value; }
        }
    }
}