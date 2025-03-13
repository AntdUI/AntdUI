// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System;
using System.Drawing;

namespace AntdUI.Svg
{
    public sealed class SvgColourServer : SvgPaintServer
    {
        /// <summary>
        /// An unspecified <see cref="SvgPaintServer"/>.
        /// </summary>
        public static readonly SvgPaintServer NotSet = new SvgColourServer();
        /// <summary>
        /// A <see cref="SvgPaintServer"/> that should inherit from its parent.
        /// </summary>
        public static readonly SvgPaintServer Inherit = new SvgColourServer();

        public SvgColourServer() : this(System.Drawing.Color.Black)
        {
        }

        public SvgColourServer(Color colour)
        {
            _colour = colour;
        }

        private Color _colour;

        public Color Colour
        {
            get { return _colour; }
            set { _colour = value; }
        }

        public override Brush GetBrush(SvgVisualElement styleOwner, ISvgRenderer renderer, float opacity, bool forStroke = false)
        {
            // is none?
            if (this == SvgPaintServer.None) return new SolidBrush(System.Drawing.Color.Transparent);

            // default fill color is black, default stroke color is none
            if (this == SvgColourServer.NotSet && forStroke) return new SolidBrush(System.Drawing.Color.Transparent);

            int alpha = (int)Math.Round((opacity * (Colour.A / 255.0)) * 255);
            Color colour = System.Drawing.Color.FromArgb(alpha, Colour);

            return new SolidBrush(colour);
        }

        public override string ToString()
        {
            if (this == SvgPaintServer.None)
                return "none";
            else if (this == SvgColourServer.NotSet)
                return string.Empty;
            else if (this == SvgColourServer.Inherit)
                return "inherit";

            Color c = Colour;

            // Return the name if it exists
            if (c.IsKnownColor)
                return c.Name;

            // Return the hex value
            return String.Format("#{0}", c.ToArgb().ToString("x").Substring(2));
        }

        public override bool Equals(object? obj)
        {
            var objColor = obj as SvgColourServer;
            if (objColor == null)
                return false;

            if ((this == SvgPaintServer.None && obj != SvgPaintServer.None) ||
                (this != SvgPaintServer.None && obj == SvgPaintServer.None) ||
                (this == SvgColourServer.NotSet && obj != SvgColourServer.NotSet) ||
                (this != SvgColourServer.NotSet && obj == SvgColourServer.NotSet) ||
                (this == SvgColourServer.Inherit && obj != SvgColourServer.Inherit) ||
                (this != SvgColourServer.Inherit && obj == SvgColourServer.Inherit)) return false;

            return GetHashCode() == objColor.GetHashCode();
        }

        public override int GetHashCode()
        {
            return _colour.GetHashCode();
        }
    }
}