// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System.Linq;

namespace AntdUI.Svg
{
    public class SvgFont : SvgElement
    {
        public override string ClassName { get => "font"; }

        [SvgAttribute("horiz-adv-x")]
        public float HorizAdvX
        {
            get { return (Attributes["horiz-adv-x"] == null ? 0 : (float)Attributes["horiz-adv-x"]); }
            set { Attributes["horiz-adv-x"] = value; }
        }
        [SvgAttribute("horiz-origin-x")]
        public float HorizOriginX
        {
            get { return (Attributes["horiz-origin-x"] == null ? 0 : (float)Attributes["horiz-origin-x"]); }
            set { Attributes["horiz-origin-x"] = value; }
        }
        [SvgAttribute("horiz-origin-y")]
        public float HorizOriginY
        {
            get { return (Attributes["horiz-origin-y"] == null ? 0 : (float)Attributes["horiz-origin-y"]); }
            set { Attributes["horiz-origin-y"] = value; }
        }
        [SvgAttribute("vert-adv-y")]
        public float VertAdvY
        {
            get { return (Attributes["vert-adv-y"] == null ? Children.OfType<SvgFontFace>().First().UnitsPerEm : (float)Attributes["vert-adv-y"]); }
            set { Attributes["vert-adv-y"] = value; }
        }
        [SvgAttribute("vert-origin-x")]
        public float VertOriginX
        {
            get { return (Attributes["vert-origin-x"] == null ? HorizAdvX / 2 : (float)Attributes["vert-origin-x"]); }
            set { Attributes["vert-origin-x"] = value; }
        }
        [SvgAttribute("vert-origin-y")]
        public float VertOriginY
        {
            get
            {
                return (Attributes["vert-origin-y"] == null ?
                          (Children.OfType<SvgFontFace>().First().Attributes["ascent"] == null ? 0 : Children.OfType<SvgFontFace>().First().Ascent) :
                          (float)Attributes["vert-origin-y"]);
            }
            set { Attributes["vert-origin-y"] = value; }
        }

        protected override void Render(ISvgRenderer renderer)
        { }
    }
}