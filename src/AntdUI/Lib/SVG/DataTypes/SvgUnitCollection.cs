// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System;
using System.Collections.Generic;
using System.Linq;

namespace AntdUI.Svg
{
    /// <summary>
    /// Represents a list of <see cref="SvgUnit"/>.
    /// </summary>
    public class SvgUnitCollection : List<SvgUnit>
    {
        public override string ToString()
        {
            // The correct separator should be a single white space.
            // More see:
            // http://www.w3.org/TR/SVG/coords.html
            // "Superfluous white space and separators such as commas can be eliminated
            // (e.g., 'M 100 100 L 200 200' contains unnecessary spaces and could be expressed more compactly as 'M100 100L200 200')."
            // http://www.w3.org/TR/SVGTiny12/paths.html#PathDataGeneralInformation
            // https://developer.mozilla.org/en-US/docs/Web/SVG/Attribute/d#Notes
            return string.Join(" ", this.Select(u => u.ToString()).ToArray());
        }

        public static bool IsNullOrEmpty(SvgUnitCollection collection)
        {
            return collection == null || collection.Count < 1 ||
                (collection.Count == 1 && (collection[0] == SvgUnit.Empty || collection[0] == SvgUnit.None));
        }
    }

    internal class SvgUnitCollectionConverter
    {
        public static SvgUnitCollection Parse(string value)
        {
            if (string.Compare((value).Trim(), "none", StringComparison.InvariantCultureIgnoreCase) == 0) return null;
            string[] points = (value).Trim().Split(new char[] { ',', ' ', '\r', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            var units = new SvgUnitCollection();
            foreach (string point in points)
            {
                var newUnit = SvgUnitConverter.Parse(point.Trim());
                if (!newUnit.IsNone) units.Add(newUnit);
            }
            return units;
        }
    }
}