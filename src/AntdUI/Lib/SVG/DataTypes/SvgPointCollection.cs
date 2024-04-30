// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace AntdUI.Svg
{
    /// <summary>
    /// Represents a list of <see cref="SvgUnit"/> used with the <see cref="SvgPolyline"/> and <see cref="SvgPolygon"/>.
    /// </summary>
    public class SvgPointCollection : List<SvgUnit>
    {
        public override string ToString()
        {
            var builder = new StringBuilder();
            for (var i = 0; i < Count; i += 2)
            {
                if (i + 1 < Count)
                {
                    if (i > 1)
                    {
                        builder.Append(" ");
                    }
                    // we don't need unit type
                    builder.Append(this[i].Value.ToString(CultureInfo.InvariantCulture));
                    builder.Append(",");
                    builder.Append(this[i + 1].Value.ToString(CultureInfo.InvariantCulture));
                }
            }
            return builder.ToString();
        }
    }
    internal class SvgPointCollectionConverter
    {
        public static SvgPointCollection Parse(string value)
        {
            var strValue = value.Trim();
            if (string.Compare(strValue, "none", StringComparison.InvariantCultureIgnoreCase) == 0) return null;
            var parser = new CoordinateParser(strValue);
            var result = new SvgPointCollection();
            while (parser.TryGetFloat(out var pointValue)) result.Add(new SvgUnit(SvgUnitType.User, pointValue));
            return result;
        }
    }
}