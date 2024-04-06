// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

namespace AntdUI.Svg
{
    public abstract class SvgKern : SvgElement
    {
        [SvgAttribute("g1")]
        public string Glyph1
        {
            get { return Attributes["g1"] as string; }
            set { Attributes["g1"] = value; }
        }
        [SvgAttribute("g2")]
        public string Glyph2
        {
            get { return Attributes["g2"] as string; }
            set { Attributes["g2"] = value; }
        }
        [SvgAttribute("u1")]
        public string Unicode1
        {
            get { return Attributes["u1"] as string; }
            set { Attributes["u1"] = value; }
        }
        [SvgAttribute("u2")]
        public string Unicode2
        {
            get { return Attributes["u2"] as string; }
            set { Attributes["u2"] = value; }
        }
        [SvgAttribute("k")]
        public float Kerning
        {
            get { return (Attributes["k"] == null ? 0 : (float)Attributes["k"]); }
            set { Attributes["k"] = value; }
        }
    }

    public class SvgVerticalKern : SvgKern
    {
        public override string ClassName { get => "vkern"; }
    }

    public class SvgHorizontalKern : SvgKern
    {
        public override string ClassName { get => "hkern"; }
    }
}