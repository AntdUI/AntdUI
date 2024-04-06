// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AntdUI.Svg
{
    /// <summary>
    /// Manages access to <see cref="SystemFonts"/> and any privately loaded fonts.
    /// When a font is requested in the render process, if the font is not found as an embedded SvgFont, the render
    /// process will SvgFontManager.FindFont method.
    /// </summary>

    public static class SvgFontManager
    {
        private static readonly Dictionary<string, FontFamily> SystemFonts;
        static SvgFontManager()
        {
            SystemFonts = FontFamily.Families.ToDictionary(ff => ff.Name.ToLower());
        }

        /// <summary>
        /// This method searches a dictionary of fonts (pre loaded with the system fonts). If a
        /// font can't be found and a callback has been provided - then the callback should perform
        /// any validation and return a font (or null if not found/error).
        /// Where a font can't be located it is the responsibility of the caller to perform any
        /// exception handling.
        /// </summary>
        /// <param name="name">A <see cref="string"/> containing the FamilyName of the font.</param>
        /// <returns>An <see cref="FontFamily"/> of the loaded font or null is not located.</returns>
        public static FontFamily? FindFont(string name)
        {
            if (name == null) return null;
            string key = name.ToLower();
            if (SystemFonts.TryGetValue(key, out var ff)) return ff;
            return null;
        }
    }
}