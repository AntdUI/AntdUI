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
            get { return this.Attributes["g1"] as string; }
            set { this.Attributes["g1"] = value; }
        }
        [SvgAttribute("g2")]
        public string Glyph2
        {
            get { return this.Attributes["g2"] as string; }
            set { this.Attributes["g2"] = value; }
        }
        [SvgAttribute("u1")]
        public string Unicode1
        {
            get { return this.Attributes["u1"] as string; }
            set { this.Attributes["u1"] = value; }
        }
        [SvgAttribute("u2")]
        public string Unicode2
        {
            get { return this.Attributes["u2"] as string; }
            set { this.Attributes["u2"] = value; }
        }
        [SvgAttribute("k")]
        public float Kerning
        {
            get { return (this.Attributes["k"] == null ? 0 : (float)this.Attributes["k"]); }
            set { this.Attributes["k"] = value; }
        }
    }

    [SvgElement("vkern")]
    public class SvgVerticalKern : SvgKern
    {
        public override SvgElement DeepCopy()
        {
            return base.DeepCopy<SvgVerticalKern>();
        }
    }
    [SvgElement("hkern")]
    public class SvgHorizontalKern : SvgKern
    {
        public override SvgElement DeepCopy()
        {
            return base.DeepCopy<SvgHorizontalKern>();
        }
    }
}
