// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

namespace AntdUI.Svg.FilterEffects
{
    public abstract class SvgFilterPrimitive : SvgElement
    {
        public const string SourceGraphic = "SourceGraphic";
        public const string SourceAlpha = "SourceAlpha";
        public const string BackgroundImage = "BackgroundImage";
        public const string BackgroundAlpha = "BackgroundAlpha";
        public const string FillPaint = "FillPaint";
        public const string StrokePaint = "StrokePaint";

        [SvgAttribute("in")]
        public string Input
        {
            get { return this.Attributes.GetAttribute<string>("in"); }
            set { this.Attributes["in"] = value; }
        }

        [SvgAttribute("result")]
        public string Result
        {
            get { return this.Attributes.GetAttribute<string>("result"); }
            set { this.Attributes["result"] = value; }
        }

        protected SvgFilter Owner
        {
            get { return (SvgFilter)this.Parent; }
        }

        public abstract void Process(ImageBuffer buffer);
    }
}