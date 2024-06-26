// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using AntdUI.Svg.Transforms;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace AntdUI.Svg
{
    /// <summary>
    /// A pattern is used to fill or stroke an object using a pre-defined graphic object which can be replicated ("tiled") at fixed intervals in x and y to cover the areas to be painted.
    /// </summary>
    public sealed class SvgPatternServer : SvgPaintServer, ISvgViewPort, ISvgSupportsCoordinateUnits
    {
        public override string ClassName => "pattern";

        private SvgUnit _width;
        private SvgUnit _height;
        private SvgUnit _x;
        private SvgUnit _y;
        private SvgPaintServer _inheritGradient;
        private SvgViewBox _viewBox;
        private SvgCoordinateUnits _patternUnits = SvgCoordinateUnits.Inherit;
        private SvgCoordinateUnits _patternContentUnits = SvgCoordinateUnits.Inherit;

        [SvgAttribute("overflow")]
        public SvgOverflow Overflow
        {
            get { return Attributes.GetAttribute<SvgOverflow>("overflow"); }
            set { Attributes["overflow"] = value; }
        }


        /// <summary>
        /// Specifies a supplemental transformation which is applied on top of any 
        /// transformations necessary to create a new pattern coordinate system.
        /// </summary>
        [SvgAttribute("viewBox")]
        public SvgViewBox ViewBox
        {
            get { return _viewBox; }
            set { _viewBox = value; }
        }

        /// <summary>
        /// Gets or sets the aspect of the viewport.
        /// </summary>
        /// <value></value>
        [SvgAttribute("preserveAspectRatio")]
        public SvgAspectRatio AspectRatio
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the width of the pattern.
        /// </summary>
        [SvgAttribute("width")]
        public SvgUnit Width
        {
            get { return _width; }
            set { _width = value; }
        }

        /// <summary>
        /// Gets or sets the width of the pattern.
        /// </summary>
        [SvgAttribute("patternUnits")]
        public SvgCoordinateUnits PatternUnits
        {
            get { return _patternUnits; }
            set { _patternUnits = value; }
        }

        /// <summary>
        /// Gets or sets the width of the pattern.
        /// </summary>
        [SvgAttribute("patternContentUnits")]
        public SvgCoordinateUnits PatternContentUnits
        {
            get { return _patternContentUnits; }
            set { _patternContentUnits = value; }
        }

        /// <summary>
        /// Gets or sets the height of the pattern.
        /// </summary>
        [SvgAttribute("height")]
        public SvgUnit Height
        {
            get { return _height; }
            set { _height = value; }
        }

        /// <summary>
        /// Gets or sets the X-axis location of the pattern.
        /// </summary>
        [SvgAttribute("x")]
        public SvgUnit X
        {
            get { return _x; }
            set { _x = value; }
        }

        /// <summary>
        /// Gets or sets the Y-axis location of the pattern.
        /// </summary>
        [SvgAttribute("y")]
        public SvgUnit Y
        {
            get { return _y; }
            set { _y = value; }
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

        [SvgAttribute("patternTransform")]
        public SvgTransformCollection PatternTransform
        {
            get { return (Attributes.GetAttribute<SvgTransformCollection>("patternTransform")); }
            set { Attributes["patternTransform"] = value; }
        }

        private Matrix EffectivePatternTransform
        {
            get
            {
                var transform = new Matrix();

                if (PatternTransform != null)
                {
                    transform.Multiply(PatternTransform.GetMatrix());
                }
                return transform;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgPatternServer"/> class.
        /// </summary>
        public SvgPatternServer()
        {
            _x = SvgUnit.None;
            _y = SvgUnit.None;
            _width = SvgUnit.None;
            _height = SvgUnit.None;
        }

        private SvgUnit NormalizeUnit(SvgUnit orig)
        {
            return (orig.Type == SvgUnitType.Percentage && PatternUnits == SvgCoordinateUnits.ObjectBoundingBox ?
                    new SvgUnit(SvgUnitType.User, orig.Value / 100) :
                    orig);
        }

        /// <summary>
        /// Gets a <see cref="Brush"/> representing the current paint server.
        /// </summary>
        /// <param name="renderingElement">The owner <see cref="SvgVisualElement"/>.</param>
        /// <param name="renderer">The renderer object.</param>
        /// <param name="opacity">The opacity of the brush.</param>
        /// <param name="forStroke">Not used.</param>
        public override Brush GetBrush(SvgVisualElement renderingElement, ISvgRenderer renderer, float opacity, bool forStroke = false)
        {
            var chain = new List<SvgPatternServer>();
            var curr = this;
            while (curr != null)
            {
                chain.Add(curr);
                curr = SvgDeferredPaintServer.TryGet<SvgPatternServer>(curr._inheritGradient, renderingElement);
            }

            var childElem = chain.Where((p) => p.Children != null && p.Children.Count > 0).FirstOrDefault();
            if (childElem == null) return null;
            var widthElem = chain.Where((p) => p.Width != null && p.Width != SvgUnit.None).FirstOrDefault();
            var heightElem = chain.Where((p) => p.Height != null && p.Height != SvgUnit.None).FirstOrDefault();
            if (widthElem == null && heightElem == null) return null;

            var viewBoxElem = chain.Where((p) => p.ViewBox != null && p.ViewBox != SvgViewBox.Empty).FirstOrDefault();
            var viewBox = viewBoxElem == null ? SvgViewBox.Empty : viewBoxElem.ViewBox;
            var xElem = chain.Where((p) => p.X != null && p.X != SvgUnit.None).FirstOrDefault();
            var yElem = chain.Where((p) => p.Y != null && p.Y != SvgUnit.None).FirstOrDefault();
            var xUnit = xElem == null ? SvgUnit.Empty : xElem.X;
            var yUnit = yElem == null ? SvgUnit.Empty : yElem.Y;

            var patternUnitElem = chain.Where((p) => p.PatternUnits != SvgCoordinateUnits.Inherit).FirstOrDefault();
            var patternUnits = (patternUnitElem == null ? SvgCoordinateUnits.ObjectBoundingBox : patternUnitElem.PatternUnits);
            var patternContentUnitElem = chain.Where((p) => p.PatternContentUnits != SvgCoordinateUnits.Inherit).FirstOrDefault();
            var patternContentUnits = (patternContentUnitElem == null ? SvgCoordinateUnits.UserSpaceOnUse : patternContentUnitElem.PatternContentUnits);

            try
            {
                if (patternUnits == SvgCoordinateUnits.ObjectBoundingBox) renderer.SetBoundable(renderingElement);

                using (var patternMatrix = new Matrix())
                {
                    var bounds = renderer.GetBoundable().Bounds;
                    var xScale = (patternUnits == SvgCoordinateUnits.ObjectBoundingBox ? bounds.Width : 1);
                    var yScale = (patternUnits == SvgCoordinateUnits.ObjectBoundingBox ? bounds.Height : 1);

                    float x = xScale * NormalizeUnit(xUnit).ToDeviceValue(renderer, UnitRenderingType.Horizontal, this);
                    float y = yScale * NormalizeUnit(yUnit).ToDeviceValue(renderer, UnitRenderingType.Vertical, this);

                    float width = xScale * NormalizeUnit(widthElem.Width).ToDeviceValue(renderer, UnitRenderingType.Horizontal, this);
                    float height = yScale * NormalizeUnit(heightElem.Height).ToDeviceValue(renderer, UnitRenderingType.Vertical, this);

                    // Apply a scale if needed
                    patternMatrix.Scale((patternContentUnits == SvgCoordinateUnits.ObjectBoundingBox ? bounds.Width : 1) *
                                        (viewBox.Width > 0 ? width / viewBox.Width : 1),
                                        (patternContentUnits == SvgCoordinateUnits.ObjectBoundingBox ? bounds.Height : 1) *
                                        (viewBox.Height > 0 ? height / viewBox.Height : 1), MatrixOrder.Prepend);

                    Bitmap image = new Bitmap((int)width, (int)height);
                    using (var iRenderer = SvgRenderer.FromImage(image))
                    {
                        iRenderer.SetBoundable((_patternContentUnits == SvgCoordinateUnits.ObjectBoundingBox) ? new GenericBoundable(0, 0, width, height) : renderer.GetBoundable());
                        iRenderer.Transform = patternMatrix;
                        iRenderer.SmoothingMode = SmoothingMode.AntiAlias;
                        iRenderer.SetClip(new Region(new RectangleF(0, 0, viewBox.Width > 0 ? viewBox.Width : width, viewBox.Height > 0 ? viewBox.Height : height)));

                        foreach (SvgElement child in childElem.Children)
                        {
                            child.RenderElement(iRenderer);
                        }
                    }

                    TextureBrush textureBrush = new TextureBrush(image);
                    var brushTransform = EffectivePatternTransform.Clone();
                    brushTransform.Translate(x, y, MatrixOrder.Append);
                    textureBrush.Transform = brushTransform;
                    return textureBrush;
                }
            }
            finally
            {
                if (PatternUnits == SvgCoordinateUnits.ObjectBoundingBox) renderer.PopBoundable();
            }
        }

        public SvgCoordinateUnits GetUnits()
        {
            return _patternUnits;
        }
    }
}