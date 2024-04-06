// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System;
using System.ComponentModel;
using System.Drawing;

namespace AntdUI.Svg
{
    /// <summary>
    /// Represents a colour stop in a gradient.
    /// </summary>
    public class SvgGradientStop : SvgElement
    {
        public override string ClassName => "stop";

        private SvgUnit _offset;

        /// <summary>
        /// Gets or sets the offset, i.e. where the stop begins from the beginning, of the gradient stop.
        /// </summary>
        [SvgAttribute("offset")]
        public SvgUnit Offset
        {
            get { return _offset; }
            set
            {
                SvgUnit unit = value;

                if (value.Type == SvgUnitType.Percentage)
                {
                    if (value.Value > 100)
                    {
                        unit = new SvgUnit(value.Type, 100);
                    }
                    else if (value.Value < 0)
                    {
                        unit = new SvgUnit(value.Type, 0);
                    }
                }
                else if (value.Type == SvgUnitType.User)
                {
                    if (value.Value > 1)
                    {
                        unit = new SvgUnit(value.Type, 1);
                    }
                    else if (value.Value < 0)
                    {
                        unit = new SvgUnit(value.Type, 0);
                    }
                }

                _offset = unit.ToPercentage();
            }
        }

        /// <summary>
        /// Gets or sets the colour of the gradient stop.
        /// </summary>
        [SvgAttribute("stop-color")]
        [TypeConverter(typeof(SvgPaintServerFactory))]
        public override SvgPaintServer StopColor
        {
            get
            {
                var direct = Attributes.GetAttribute<SvgPaintServer>("stop-color", SvgColourServer.NotSet);
                if (direct == SvgColourServer.Inherit) return Attributes["stop-color"] as SvgPaintServer ?? SvgColourServer.NotSet;
                return direct;
            }
            set { Attributes["stop-color"] = value; }
        }

        /// <summary>
        /// Gets or sets the opacity of the gradient stop (0-1).
        /// </summary>
        [SvgAttribute("stop-opacity")]
        public override float Opacity
        {
            get { return (Attributes["stop-opacity"] == null) ? 1.0f : (float)Attributes["stop-opacity"]; }
            set { Attributes["stop-opacity"] = FixOpacityValue(value); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgGradientStop"/> class.
        /// </summary>
        public SvgGradientStop()
        {
            _offset = new SvgUnit(0.0f);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgGradientStop"/> class.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <param name="colour">The colour.</param>
        public SvgGradientStop(SvgUnit offset, Color colour)
        {
            _offset = offset;
        }

        public Color GetColor(SvgElement parent)
        {
            var core = SvgDeferredPaintServer.TryGet<SvgColourServer>(StopColor, parent);
            if (core == null) throw new InvalidOperationException("Invalid paint server for gradient stop detected.");
            return core.Colour;
        }
    }
}