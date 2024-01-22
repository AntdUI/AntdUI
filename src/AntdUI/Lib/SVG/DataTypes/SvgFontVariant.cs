// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System.ComponentModel;

namespace AntdUI.Svg
{
    [TypeConverter(typeof(SvgFontVariantConverter))]
    public enum SvgFontVariant
    {
        Normal,
        Smallcaps,
        Inherit
    }
}
