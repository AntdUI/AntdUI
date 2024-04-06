// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

namespace AntdUI.Svg
{
    public class SvgMissingGlyph : SvgGlyph
    {
        public override string ClassName { get => "missing-glyph"; }

        [SvgAttribute("glyph-name")]
        public override string GlyphName
        {
            get { return Attributes["glyph-name"] as string ?? "__MISSING_GLYPH__"; }
            set { Attributes["glyph-name"] = value; }
        }
    }
}