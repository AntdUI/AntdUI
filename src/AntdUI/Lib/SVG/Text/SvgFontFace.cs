// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

namespace AntdUI.Svg
{
    public class SvgFontFace : SvgElement
    {
        public override string ClassName { get => "font-face"; }

        [SvgAttribute("alphabetic")]
        public float Alphabetic
        {
            get { return (Attributes["alphabetic"] == null ? 0 : (float)Attributes["alphabetic"]); }
            set { Attributes["alphabetic"] = value; }
        }

        [SvgAttribute("ascent")]
        public float Ascent
        {
            get
            {
                if (Attributes["ascent"] == null)
                {
                    var font = Parent as SvgFont;
                    return (font == null ? 0 : UnitsPerEm - font.VertOriginY);
                }
                else
                {
                    return (float)Attributes["ascent"];
                }
            }
            set { Attributes["ascent"] = value; }
        }

        [SvgAttribute("ascent-height")]
        public float AscentHeight
        {
            get { return (Attributes["ascent-height"] == null ? Ascent : (float)Attributes["ascent-height"]); }
            set { Attributes["ascent-height"] = value; }
        }

        [SvgAttribute("descent")]
        public float Descent
        {
            get
            {
                if (Attributes["descent"] == null)
                {
                    var font = Parent as SvgFont;
                    return (font == null ? 0 : font.VertOriginY);
                }
                else
                {
                    return (float)Attributes["descent"];
                }
            }
            set { Attributes["descent"] = value; }
        }

        /// <summary>
        /// Indicates which font family is to be used to render the text.
        /// </summary>
        [SvgAttribute("font-family")]
        public override string FontFamily
        {
            get { return Attributes["font-family"] as string; }
            set { Attributes["font-family"] = value; }
        }

        /// <summary>
        /// Refers to the size of the font from baseline to baseline when multiple lines of text are set solid in a multiline layout environment.
        /// </summary>
        [SvgAttribute("font-size")]
        public override SvgUnit FontSize
        {
            get { return (Attributes["font-size"] == null) ? SvgUnit.Empty : (SvgUnit)Attributes["font-size"]; }
            set { Attributes["font-size"] = value; }
        }

        /// <summary>
        /// Refers to the style of the font.
        /// </summary>
        [SvgAttribute("font-style")]
        public override SvgFontStyle FontStyle
        {
            get { return (Attributes["font-style"] == null) ? SvgFontStyle.All : (SvgFontStyle)Attributes["font-style"]; }
            set { Attributes["font-style"] = value; }
        }

        /// <summary>
        /// Refers to the varient of the font.
        /// </summary>
        [SvgAttribute("font-variant")]
        public override SvgFontVariant FontVariant
        {
            get { return (Attributes["font-variant"] == null) ? SvgFontVariant.Inherit : (SvgFontVariant)Attributes["font-variant"]; }
            set { Attributes["font-variant"] = value; }
        }

        /// <summary>
        /// Refers to the boldness of the font.
        /// </summary>
        [SvgAttribute("font-weight")]
        public override SvgFontWeight FontWeight
        {
            get { return (Attributes["font-weight"] == null) ? SvgFontWeight.Inherit : (SvgFontWeight)Attributes["font-weight"]; }
            set { Attributes["font-weight"] = value; }
        }

        [SvgAttribute("panose-1")]
        public string Panose1
        {
            get { return Attributes["panose-1"] as string; }
            set { Attributes["panose-1"] = value; }
        }

        [SvgAttribute("units-per-em")]
        public float UnitsPerEm
        {
            get { return (Attributes["units-per-em"] == null ? 1000 : (float)Attributes["units-per-em"]); }
            set { Attributes["units-per-em"] = value; }
        }

        [SvgAttribute("x-height")]
        public float XHeight
        {
            get { return (Attributes["x-height"] == null ? float.MinValue : (float)Attributes["x-height"]); }
            set { Attributes["x-height"] = value; }
        }
    }
}