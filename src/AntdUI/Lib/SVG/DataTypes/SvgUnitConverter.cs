// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System;
using System.Globalization;

namespace AntdUI.Svg
{
    internal class SvgUnitConverter
    {
        public static SvgUnit Parse(string? value)
        {
            if (value == null) return new SvgUnit(SvgUnitType.User, 0.0f);
            string unit = value;
            int identifierIndex = -1;
            if (unit == "none") return SvgUnit.None;
            if (unit == "medium") unit = "1em";
            else if (unit == "small") unit = "0.8em";
            else if (unit == "x-small") unit = "0.7em";
            else if (unit == "xx-small") unit = "0.6em";
            else if (unit == "large") unit = "1.2em";
            else if (unit == "x-large") unit = "1.4em";
            else if (unit == "xx-large") unit = "1.7em";

            for (int i = 0; i < unit.Length; i++)
            {
                // If the character is a percent sign or a letter which is not an exponent 'e'
                if (unit[i] == '%' || (char.IsLetter(unit[i]) && !((unit[i] == 'e' || unit[i] == 'E') && i < unit.Length - 1 && !char.IsLetter(unit[i + 1]))))
                {
                    identifierIndex = i;
                    break;
                }
            }

            float.TryParse((identifierIndex > -1) ? unit.Substring(0, identifierIndex) : unit, NumberStyles.Float, CultureInfo.InvariantCulture, out var val);

            if (identifierIndex == -1) return new SvgUnit(val);

            switch (unit.Substring(identifierIndex).Trim().ToLower())
            {
                case "mm":
                    return new SvgUnit(SvgUnitType.Millimeter, val);
                case "cm":
                    return new SvgUnit(SvgUnitType.Centimeter, val);
                case "in":
                    return new SvgUnit(SvgUnitType.Inch, val);
                case "px":
                    return new SvgUnit(SvgUnitType.Pixel, val);
                case "pt":
                    return new SvgUnit(SvgUnitType.Point, val);
                case "pc":
                    return new SvgUnit(SvgUnitType.Pica, val);
                case "%":
                    return new SvgUnit(SvgUnitType.Percentage, val);
                case "em":
                    return new SvgUnit(SvgUnitType.Em, val);
                case "ex":
                    return new SvgUnit(SvgUnitType.Ex, val);
                default:
                    throw new FormatException("Unit is in an invalid format '" + unit + "'.");
            }
        }
    }
}