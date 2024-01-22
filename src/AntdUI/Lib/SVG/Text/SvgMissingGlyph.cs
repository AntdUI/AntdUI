// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

namespace AntdUI.Svg
{
    [SvgElement("missing-glyph")]
    public class SvgMissingGlyph : SvgGlyph
    {
        [SvgAttribute("glyph-name")]
        public override string GlyphName
        {
            get { return this.Attributes["glyph-name"] as string ?? "__MISSING_GLYPH__"; }
            set { this.Attributes["glyph-name"] = value; }
        }
    }
}
