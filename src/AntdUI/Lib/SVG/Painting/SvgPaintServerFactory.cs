// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AntdUI.Svg
{
    internal class SvgPaintServerConverter
    {
        public static SvgPaintServer Parse(string value, SvgDocument context)
        {
            if (string.Equals(value.Trim(), "none", StringComparison.OrdinalIgnoreCase) || string.IsNullOrEmpty(value) || value.Trim().Length < 1) return SvgPaintServer.None;
            else return Create(value, context);
        }

        public static SvgPaintServer Create(string value, SvgDocument document)
        {
            // If it's pointing to a paint server
            if (string.IsNullOrEmpty(value)) return SvgColourServer.NotSet;
            else if (value == "inherit") return SvgColourServer.Inherit;
            else if (value == "currentColor") return new SvgDeferredPaintServer(document, value);
            else
            {
                var servers = new List<SvgPaintServer>();
                while (!string.IsNullOrEmpty(value))
                {
                    if (value.StartsWith("url(#"))
                    {
                        var leftParen = value.IndexOf(')', 5);
                        Uri id = new Uri(value.Substring(5, leftParen - 5), UriKind.Relative);
                        value = value.Substring(leftParen + 1).Trim();
                        servers.Add((SvgPaintServer)document.IdManager.GetElementById(id));
                    }
                    // If referenced to to a different (linear or radial) gradient
                    else if (document.IdManager.GetElementById(value) != null && document.IdManager.GetElementById(value).GetType().BaseType == typeof(SvgGradientServer))
                    {
                        return (SvgPaintServer)document.IdManager.GetElementById(value);
                    }
                    else if (value.StartsWith("#")) // Otherwise try and parse as colour
                    {
                        switch (CountHexDigits(value, 1))
                        {
                            case 3:
                                servers.Add(new SvgColourServer(ParseColor(value.Substring(0, 4))));
                                value = value.Substring(4).Trim();
                                break;
                            case 6:
                                servers.Add(new SvgColourServer(ParseColor(value.Substring(0, 7))));
                                value = value.Substring(7).Trim();
                                break;
                            default:
                                return new SvgDeferredPaintServer(document, value);
                        }
                    }
                    else return new SvgColourServer(ParseColor(value.Trim()));
                }

                if (servers.Count > 1) return new SvgFallbackPaintServer(servers[0], servers.Skip(1));
                return servers[0];
            }
        }

        public static Color ParseColor(string colour)
        {
            colour = colour.Trim();

            if (colour.StartsWith("rgb"))
            {
                try
                {
                    int start = colour.IndexOf("(") + 1;

                    //get the values from the RGB string
                    string[] values = colour.Substring(start, colour.IndexOf(")") - start).Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    //determine the alpha value if this is an RGBA (it will be the 4th value if there is one)
                    int alphaValue = 255;
                    if (values.Length > 3)
                    {
                        //the alpha portion of the rgba is not an int 0-255 it is a decimal between 0 and 1
                        //so we have to determine the corosponding byte value
                        var alphastring = values[3];
                        if (alphastring.StartsWith(".")) alphastring = "0" + alphastring;
                        var alphaDecimal = decimal.Parse(alphastring);
                        if (alphaDecimal <= 1) alphaValue = (int)Math.Round(alphaDecimal * 255);
                        else alphaValue = (int)Math.Round(alphaDecimal);
                    }

                    if (values[0].Trim().EndsWith("%")) return Color.FromArgb(alphaValue, (int)Math.Round(255 * float.Parse(values[0].Trim().TrimEnd('%')) / 100f), (int)Math.Round(255 * float.Parse(values[1].Trim().TrimEnd('%')) / 100f), (int)Math.Round(255 * float.Parse(values[2].Trim().TrimEnd('%')) / 100f));
                    else return Color.FromArgb(alphaValue, int.Parse(values[0]), int.Parse(values[1]), int.Parse(values[2]));
                }
                catch
                {
                    throw new SvgException("Colour is in an invalid format: '" + colour + "'");
                }
            }
            // HSL support
            else if (colour.StartsWith("hsl"))
            {
                try
                {
                    int start = colour.IndexOf("(") + 1;

                    //get the values from the RGB string
                    string[] values = colour.Substring(start, colour.IndexOf(")") - start).Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (values[1].EndsWith("%"))
                    {
                        values[1] = values[1].TrimEnd('%');
                    }
                    if (values[2].EndsWith("%"))
                    {
                        values[2] = values[2].TrimEnd('%');
                    }
                    // Get the HSL values in a range from 0 to 1.
                    double h = double.Parse(values[0]) / 360.0;
                    double s = double.Parse(values[1]) / 100.0;
                    double l = double.Parse(values[2]) / 100.0;
                    // Convert the HSL color to an RGB color
                    Color colorpart = Hsl2Rgb(h, s, l);
                    return colorpart;
                }
                catch
                {
                    throw new SvgException("Colour is in an invalid format: '" + colour + "'");
                }
            }
            else if (colour.StartsWith("#")) return colour.ToColor();
            switch (colour.ToLowerInvariant())
            {
                case "activeborder": return SystemColors.ActiveBorder;
                case "activecaption": return SystemColors.ActiveCaption;
                case "appworkspace": return SystemColors.AppWorkspace;
                case "background": return SystemColors.Desktop;
                case "buttonface": return SystemColors.Control;
                case "buttonhighlight": return SystemColors.ControlLightLight;
                case "buttonshadow": return SystemColors.ControlDark;
                case "buttontext": return SystemColors.ControlText;
                case "captiontext": return SystemColors.ActiveCaptionText;
                case "graytext": return SystemColors.GrayText;
                case "highlight": return SystemColors.Highlight;
                case "highlighttext": return SystemColors.HighlightText;
                case "inactiveborder": return SystemColors.InactiveBorder;
                case "inactivecaption": return SystemColors.InactiveCaption;
                case "inactivecaptiontext": return SystemColors.InactiveCaptionText;
                case "infobackground": return SystemColors.Info;
                case "infotext": return SystemColors.InfoText;
                case "menu": return SystemColors.Menu;
                case "menutext": return SystemColors.MenuText;
                case "scrollbar": return SystemColors.ScrollBar;
                case "threeddarkshadow": return SystemColors.ControlDarkDark;
                case "threedface": return SystemColors.Control;
                case "threedhighlight": return SystemColors.ControlLight;
                case "threedlightshadow": return SystemColors.ControlLightLight;
                case "window": return SystemColors.Window;
                case "windowframe": return SystemColors.WindowFrame;
                case "windowtext": return SystemColors.WindowText;
            }
            return Color.Transparent;
        }

        /// <summary>
        /// Converts HSL color (with HSL specified from 0 to 1) to RGB color.
        /// Taken from http://www.geekymonkey.com/Programming/CSharp/RGB2HSL_HSL2RGB.htm
        /// </summary>
        /// <param name="h"></param>
        /// <param name="sl"></param>
        /// <param name="l"></param>
        /// <returns></returns>
        private static Color Hsl2Rgb(double h, double sl, double l)
        {
            double r = l;   // default to gray
            double g = l;
            double b = l;
            double v = (l <= 0.5) ? (l * (1.0 + sl)) : (l + sl - l * sl);
            if (v > 0)
            {
                double m;
                double sv;
                int sextant;
                double fract, vsf, mid1, mid2;

                m = l + l - v;
                sv = (v - m) / v;
                h *= 6.0;
                sextant = (int)h;
                fract = h - sextant;
                vsf = v * sv * fract;
                mid1 = m + vsf;
                mid2 = v - vsf;
                switch (sextant)
                {
                    case 0:
                        r = v;
                        g = mid1;
                        b = m;
                        break;
                    case 1:
                        r = mid2;
                        g = v;
                        b = m;
                        break;
                    case 2:
                        r = m;
                        g = v;
                        b = mid1;
                        break;
                    case 3:
                        r = m;
                        g = mid2;
                        b = v;
                        break;
                    case 4:
                        r = mid1;
                        g = m;
                        b = v;
                        break;
                    case 5:
                        r = v;
                        g = m;
                        b = mid2;
                        break;
                }
            }
            Color rgb = Color.FromArgb((int)Math.Round(r * 255.0), (int)Math.Round(g * 255.0), (int)Math.Round(b * 255.0));
            return rgb;
        }

        private static int CountHexDigits(string value, int start)
        {
            int i = Math.Max(start, 0);
            int count = 0;
            while (i < value.Length &&
                   ((value[i] >= '0' && value[i] <= '9') ||
                    (value[i] >= 'a' && value[i] <= 'f') ||
                    (value[i] >= 'A' && value[i] <= 'F')))
            {
                count++;
                i++;
            }
            return count;
        }
    }
}