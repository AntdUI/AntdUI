// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using AntdUI.Svg.Transforms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace AntdUI.Svg
{
    /// <summary>
    /// Provides the base class for all paint servers that wish to render a gradient.
    /// </summary>
    public abstract class SvgGradientServer : SvgPaintServer, ISvgSupportsCoordinateUnits
    {
        private SvgCoordinateUnits _gradientUnits;
        private SvgGradientSpreadMethod _spreadMethod;
        private SvgPaintServer _inheritGradient;
        private List<SvgGradientStop> _stops;

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgGradientServer"/> class.
        /// </summary>
        internal SvgGradientServer()
        {
            GradientUnits = SvgCoordinateUnits.ObjectBoundingBox;
            SpreadMethod = SvgGradientSpreadMethod.Pad;
            _stops = new List<SvgGradientStop>();
        }

        /// <summary>
        /// Called by the underlying <see cref="SvgElement"/> when an element has been added to the
        /// 'Children' collection.
        /// </summary>
        /// <param name="child">The <see cref="SvgElement"/> that has been added.</param>
        /// <param name="index">An <see cref="int"/> representing the index where the element was added to the collection.</param>
        protected override void AddElement(SvgElement child, int index)
        {
            if (child is SvgGradientStop)
            {
                Stops.Add((SvgGradientStop)child);
            }

            base.AddElement(child, index);
        }

        /// <summary>
        /// Called by the underlying <see cref="SvgElement"/> when an element has been removed from the
        /// 'Children' collection.
        /// </summary>
        /// <param name="child">The <see cref="SvgElement"/> that has been removed.</param>
        protected override void RemoveElement(SvgElement child)
        {
            if (child is SvgGradientStop)
            {
                Stops.Remove((SvgGradientStop)child);
            }

            base.RemoveElement(child);
        }

        /// <summary>
        /// Gets the ramp of colors to use on a gradient.
        /// </summary>
        public List<SvgGradientStop> Stops
        {
            get { return _stops; }
        }

        /// <summary>
        /// Specifies what happens if the gradient starts or ends inside the bounds of the target rectangle.
        /// </summary>
        [SvgAttribute("spreadMethod")]
        public SvgGradientSpreadMethod SpreadMethod
        {
            get { return _spreadMethod; }
            set { _spreadMethod = value; }
        }

        /// <summary>
        /// Gets or sets the coordinate system of the gradient.
        /// </summary>
        [SvgAttribute("gradientUnits")]
        public SvgCoordinateUnits GradientUnits
        {
            get { return _gradientUnits; }
            set { _gradientUnits = value; }
        }

        /// <summary>
        /// Gets or sets another gradient fill from which to inherit the stops from.
        /// </summary>
        [SvgAttribute("href", SvgAttributeAttribute.XLinkNamespace)]
        public SvgPaintServer InheritGradient
        {
            get { return _inheritGradient; }
            set
            {
                _inheritGradient = value;
            }
        }

        [SvgAttribute("gradientTransform")]
        public SvgTransformCollection GradientTransform
        {
            get { return (Attributes.GetAttribute<SvgTransformCollection>("gradientTransform")); }
            set { Attributes["gradientTransform"] = value; }
        }

        protected Matrix EffectiveGradientTransform
        {
            get
            {
                var transform = new Matrix();

                if (GradientTransform != null)
                {
                    transform.Multiply(GradientTransform.GetMatrix());
                }
                return transform;
            }
        }

        /// <summary>
        /// Gets a <see cref="ColorBlend"/> representing the <see cref="SvgGradientServer"/>'s gradient stops.
        /// </summary>
        /// <param name="renderer">The renderer <see cref="ISvgRenderer"/>.</param>
        /// <param name="opacity">The opacity of the colour blend.</param>
        /// <param name="radial">True if it's a radial gradiant.</param>
        protected ColorBlend GetColorBlend(ISvgRenderer renderer, float opacity, bool radial)
        {
            int colourBlends = Stops.Count;
            bool insertStart = false;
            bool insertEnd = false;

            //gradient.Transform = renderingElement.Transforms.Matrix;

            //stops should be processed in reverse order if it's a radial gradient

            // May need to increase the number of colour blends because the range *must* be from 0.0 to 1.0.
            // E.g. 0.5 - 0.8 isn't valid therefore the rest need to be calculated.

            // If the first stop doesn't start at zero
            if (Stops[0].Offset.Value > 0)
            {
                colourBlends++;

                if (radial)
                {
                    insertEnd = true;
                }
                else
                {
                    insertStart = true;
                }
            }

            // If the last stop doesn't end at 1 a stop
            float lastValue = Stops[Stops.Count - 1].Offset.Value;
            if (lastValue < 100 || lastValue < 1)
            {
                colourBlends++;
                if (radial)
                {
                    insertStart = true;
                }
                else
                {
                    insertEnd = true;
                }
            }

            ColorBlend blend = new ColorBlend(colourBlends);

            // Set positions and colour values
            int actualStops = 0;
            float mergedOpacity = 0.0f;
            float position = 0.0f;
            Color colour = System.Drawing.Color.Black;

            for (int i = 0; i < colourBlends; i++)
            {
                var currentStop = Stops[radial ? Stops.Count - 1 - actualStops : actualStops];
                var boundWidth = renderer.GetBoundable().Bounds.Width;

                mergedOpacity = opacity * currentStop.Opacity;
                position =
                    radial
                    ? 1 - (currentStop.Offset.ToDeviceValue(renderer, UnitRenderingType.Horizontal, this) / boundWidth)
                    : (currentStop.Offset.ToDeviceValue(renderer, UnitRenderingType.Horizontal, this) / boundWidth);
                position = (float)Math.Round(position, 1, MidpointRounding.AwayFromZero);
                colour = System.Drawing.Color.FromArgb((int)Math.Round(mergedOpacity * 255), currentStop.GetColor(this));

                actualStops++;

                // Insert this colour before itself at position 0
                if (insertStart && i == 0)
                {
                    blend.Positions[i] = 0.0f;
                    blend.Colors[i] = colour;

                    i++;
                }

                blend.Positions[i] = position;
                blend.Colors[i] = colour;

                // Insert this colour after itself at position 0
                if (insertEnd && i == colourBlends - 2)
                {
                    i++;

                    blend.Positions[i] = 1.0f;
                    blend.Colors[i] = colour;
                }
            }

            return blend;
        }

        protected void LoadStops(SvgVisualElement parent)
        {
            var core = SvgDeferredPaintServer.TryGet<SvgGradientServer>(_inheritGradient, parent);
            if (Stops.Count == 0 && core != null)
            {
                _stops.AddRange(core.Stops);
            }
        }

        protected static double CalculateDistance(PointF first, PointF second)
        {
            return Math.Sqrt(Math.Pow(first.X - second.X, 2) + Math.Pow(first.Y - second.Y, 2));
        }

        protected static float CalculateLength(PointF vector)
        {
            return (float)Math.Sqrt(Math.Pow(vector.X, 2) + Math.Pow(vector.Y, 2));
        }

        public SvgCoordinateUnits GetUnits()
        {
            return _gradientUnits;
        }
    }
}