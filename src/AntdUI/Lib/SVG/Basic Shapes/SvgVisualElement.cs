// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace AntdUI.Svg
{
    /// <summary>
    /// The class that all SVG elements should derive from when they are to be rendered.
    /// </summary>
    public abstract partial class SvgVisualElement : SvgElement, ISvgBoundable, ISvgStylable, ISvgClipable
    {
        private bool? _requiresSmoothRendering;
        private Region _previousClip;

        /// <summary>
        /// Gets the <see cref="GraphicsPath"/> for this element.
        /// </summary>
        public abstract GraphicsPath Path(ISvgRenderer renderer);

        PointF ISvgBoundable.Location
        {
            get
            {
                return Bounds.Location;
            }
        }

        SizeF ISvgBoundable.Size
        {
            get
            {
                return Bounds.Size;
            }
        }

        /// <summary>
        /// Gets the bounds of the element.
        /// </summary>
        /// <value>The bounds.</value>
        public abstract RectangleF Bounds { get; }

        /// <summary>
        /// Gets the associated <see cref="SvgClipPath"/> if one has been specified.
        /// </summary>
        [SvgAttribute("clip")]
        public virtual string Clip
        {
            get { return Attributes.GetInheritedAttribute<string>("clip"); }
            set { Attributes["clip"] = value; }
        }

        /// <summary>
        /// Gets the associated <see cref="SvgClipPath"/> if one has been specified.
        /// </summary>
        [SvgAttribute("clip-path")]
        public virtual Uri ClipPath
        {
            get { return Attributes.GetAttribute<Uri>("clip-path"); }
            set { Attributes["clip-path"] = value; }
        }

        /// <summary>
        /// Gets or sets the algorithm which is to be used to determine the clipping region.
        /// </summary>
        [SvgAttribute("clip-rule")]
        public SvgClipRule ClipRule
        {
            get { return Attributes.GetAttribute<SvgClipRule>("clip-rule", SvgClipRule.NonZero); }
            set { Attributes["clip-rule"] = value; }
        }

        /// <summary>
        /// Gets the associated <see cref="SvgClipPath"/> if one has been specified.
        /// </summary>
        [SvgAttribute("filter")]
        public virtual Uri Filter
        {
            get { return Attributes.GetInheritedAttribute<Uri>("filter"); }
            set { Attributes["filter"] = value; }
        }

        /// <summary>
        /// Gets or sets a value to determine if anti-aliasing should occur when the element is being rendered.
        /// </summary>
        protected virtual bool RequiresSmoothRendering
        {
            get
            {
                _requiresSmoothRendering ??= ConvertShapeRendering2AntiAlias(ShapeRendering);

                return _requiresSmoothRendering.Value;
            }
        }

        private bool ConvertShapeRendering2AntiAlias(SvgShapeRendering shapeRendering)
        {
            switch (shapeRendering)
            {
                case SvgShapeRendering.OptimizeSpeed:
                case SvgShapeRendering.CrispEdges:
                case SvgShapeRendering.GeometricPrecision:
                    return false;
                default:
                    // SvgShapeRendering.Auto
                    // SvgShapeRendering.Inherit
                    return true;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgVisualElement"/> class.
        /// </summary>
        public SvgVisualElement()
        {
            IsPathDirty = true;
        }

        protected virtual bool Renderable { get { return true; } }

        /// <summary>
        /// Renders the <see cref="SvgElement"/> and contents to the specified <see cref="Graphics"/> object.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> object to render to.</param>
        protected override void Render(ISvgRenderer renderer)
        {
            Render(renderer, true);
        }

        private void Render(ISvgRenderer renderer, bool renderFilter)
        {
            if (Visible && Displayable && PushTransforms(renderer) &&
                (!Renderable || Path(renderer) != null))
            {
                if (!(renderFilter && RenderFilter(renderer)))
                {
                    SetClip(renderer);

                    if (Renderable)
                    {
                        var opacity = Math.Min(Math.Max(Opacity, 0), 1);
                        if (opacity == 1f) RenderFillAndStroke(renderer);
                        else
                        {
                            IsPathDirty = true;
                            var bounds = Bounds;
                            IsPathDirty = true;

                            using (var canvas = new Bitmap((int)Math.Ceiling(bounds.Width), (int)Math.Ceiling(bounds.Height)))
                            {
                                using (var canvasRenderer = SvgRenderer.FromImage(canvas))
                                {
                                    canvasRenderer.SetBoundable(renderer.GetBoundable());
                                    canvasRenderer.TranslateTransform(-bounds.X, -bounds.Y);

                                    RenderFillAndStroke(canvasRenderer);
                                }
                                var srcRect = new RectangleF(0f, 0f, bounds.Width, bounds.Height);
                                renderer.DrawImage(canvas, bounds, srcRect, GraphicsUnit.Pixel, opacity);
                            }
                        }
                    }
                    else
                    {
                        base.RenderChildren(renderer);
                    }

                    ResetClip(renderer);
                    PopTransforms(renderer);
                }
            }
        }

        private bool RenderFilter(ISvgRenderer renderer)
        {
            var rendered = false;

            var filterPath = Filter;
            if (filterPath != null)
            {
                if (filterPath.ToString().StartsWith("url("))
                {
                    filterPath = new Uri(filterPath.ToString().Substring(4, filterPath.ToString().Length - 5), UriKind.RelativeOrAbsolute);
                }
                var element = OwnerDocument.IdManager.GetElementById(filterPath);
                if (element is FilterEffects.SvgFilter)
                {
                    PopTransforms(renderer);
                    try
                    {
                        ((FilterEffects.SvgFilter)element).ApplyFilter(this, renderer, (r) => Render(r, false));
                    }
                    catch
                    {
                    }
                    rendered = true;
                }
            }

            return rendered;
        }

        protected void RenderFillAndStroke(ISvgRenderer renderer)
        {
            // If this element needs smoothing enabled turn anti-aliasing on
            if (RequiresSmoothRendering)
            {
                renderer.SmoothingMode = SmoothingMode.AntiAlias;
            }

            RenderFill(renderer);
            RenderStroke(renderer);

            // Reset the smoothing mode
            if (RequiresSmoothRendering && renderer.SmoothingMode == SmoothingMode.AntiAlias)
            {
                renderer.SmoothingMode = SmoothingMode.Default;
            }
        }

        /// <summary>
        /// Renders the fill of the <see cref="SvgVisualElement"/> to the specified <see cref="ISvgRenderer"/>
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> object to render to.</param>
        protected internal virtual void RenderFill(ISvgRenderer renderer)
        {
            if (Fill != null)
            {
                using (var brush = Fill.GetBrush(this, renderer, Math.Min(Math.Max(FillOpacity, 0), 1)))
                {
                    if (brush != null)
                    {
                        Path(renderer).FillMode = FillRule == SvgFillRule.NonZero ? FillMode.Winding : FillMode.Alternate;
                        renderer.FillPath(brush, Path(renderer));
                    }
                }
            }
        }

        /// <summary>
        /// Renders the stroke of the <see cref="SvgVisualElement"/> to the specified <see cref="ISvgRenderer"/>
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> object to render to.</param>
        protected internal virtual bool RenderStroke(ISvgRenderer renderer)
        {
            if (Stroke != null && Stroke != SvgPaintServer.None && StrokeWidth > 0f)
            {
                var strokeWidth = StrokeWidth.ToDeviceValue(renderer, UnitRenderingType.Other, this);
                using (var brush = Stroke.GetBrush(this, renderer, FixOpacityValue(StrokeOpacity), true))
                {
                    if (brush != null)
                    {
                        var path = Path(renderer);
                        var bounds = path.GetBounds();
                        if (path.PointCount < 1) return false;
                        if (bounds.Width <= 0f && bounds.Height <= 0f)
                        {
                            switch (StrokeLineCap)
                            {
                                case SvgStrokeLineCap.Round:
                                    using (var capPath = new GraphicsPath())
                                    {
                                        capPath.AddEllipse(path.PathPoints[0].X - strokeWidth / 2f, path.PathPoints[0].Y - strokeWidth / 2f, strokeWidth, strokeWidth);
                                        renderer.FillPath(brush, capPath);
                                    }
                                    break;
                                case SvgStrokeLineCap.Square:
                                    using (var capPath = new GraphicsPath())
                                    {
                                        capPath.AddRectangle(new RectangleF(path.PathPoints[0].X - strokeWidth / 2f, path.PathPoints[0].Y - strokeWidth / 2f, strokeWidth, strokeWidth));
                                        renderer.FillPath(brush, capPath);
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            using (var pen = new Pen(brush, strokeWidth))
                            {
                                if (StrokeDashArray != null && StrokeDashArray.Count > 0)
                                {
                                    if (StrokeDashArray.Count % 2 != 0)
                                    {
                                        // handle odd dash arrays by repeating them once
                                        StrokeDashArray.AddRange(StrokeDashArray);
                                    }

                                    /* divide by stroke width - GDI behaviour that I don't quite understand yet.*/
                                    pen.DashPattern = StrokeDashArray.ConvertAll(u => ((u.ToDeviceValue(renderer, UnitRenderingType.Other, this) <= 0) ? 1 : u.ToDeviceValue(renderer, UnitRenderingType.Other, this)) /
                                        ((strokeWidth <= 0) ? 1 : strokeWidth)).ToArray();

                                    if (StrokeLineCap == SvgStrokeLineCap.Round)
                                    {
                                        // to handle round caps, we have to adapt the dash pattern
                                        // by increasing the dash length by the stroke width - GDI draws the rounded 
                                        // edge inside the dash line, SVG draws it outside the line
                                        var pattern = new float[pen.DashPattern.Length];
                                        int offset = 1; // the values are already normalized to dash width
                                        for (int i = 0; i < pen.DashPattern.Length; i++)
                                        {
                                            pattern[i] = pen.DashPattern[i] + offset;
                                            offset *= -1; // increase dash length, decrease spaces
                                        }
                                        pen.DashPattern = pattern;
                                        pen.DashCap = DashCap.Round;
                                    }

                                    if (StrokeDashOffset != null && StrokeDashOffset.Value != 0)
                                    {
                                        pen.DashOffset = ((StrokeDashOffset.ToDeviceValue(renderer, UnitRenderingType.Other, this) <= 0) ? 1 : StrokeDashOffset.ToDeviceValue(renderer, UnitRenderingType.Other, this)) /
                                            ((strokeWidth <= 0) ? 1 : strokeWidth);
                                    }
                                }
                                switch (StrokeLineJoin)
                                {
                                    case SvgStrokeLineJoin.Bevel:
                                        pen.LineJoin = LineJoin.Bevel;
                                        break;
                                    case SvgStrokeLineJoin.Round:
                                        pen.LineJoin = LineJoin.Round;
                                        break;
                                    case SvgStrokeLineJoin.MiterClip:
                                        pen.LineJoin = LineJoin.MiterClipped;
                                        break;
                                    // System.Drawing has no support for Arcs unfortunately
                                    default:
                                        pen.LineJoin = LineJoin.Miter;
                                        break;
                                }
                                pen.MiterLimit = StrokeMiterLimit;
                                switch (StrokeLineCap)
                                {
                                    case SvgStrokeLineCap.Round:
                                        pen.StartCap = LineCap.Round;
                                        pen.EndCap = LineCap.Round;
                                        break;
                                    case SvgStrokeLineCap.Square:
                                        pen.StartCap = LineCap.Square;
                                        pen.EndCap = LineCap.Square;
                                        break;
                                }

                                renderer.DrawPath(pen, path);

                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Sets the clipping region of the specified <see cref="ISvgRenderer"/>.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> to have its clipping region set.</param>
        protected internal virtual void SetClip(ISvgRenderer renderer)
        {
            if (ClipPath != null || !string.IsNullOrEmpty(Clip))
            {
                _previousClip = renderer.GetClip();

                if (ClipPath != null)
                {
                    SvgClipPath clipPath = OwnerDocument.GetElementById<SvgClipPath>(ClipPath.ToString());
                    if (clipPath != null) renderer.SetClip(clipPath.GetClipRegion(this), CombineMode.Intersect);
                }

                var clip = Clip;
                if (!string.IsNullOrEmpty(clip) && clip.StartsWith("rect("))
                {
                    clip = clip.Trim();
                    var offsets = (from o in clip.Substring(5, clip.Length - 6).Split(',')
                                   select float.Parse(o.Trim())).ToList();
                    var bounds = Bounds;
                    var clipRect = new RectangleF(bounds.Left + offsets[3], bounds.Top + offsets[0],
                                                  bounds.Width - (offsets[3] + offsets[1]),
                                                  bounds.Height - (offsets[2] + offsets[0]));
                    renderer.SetClip(new Region(clipRect), CombineMode.Intersect);
                }
            }
        }

        /// <summary>
        /// Resets the clipping region of the specified <see cref="ISvgRenderer"/> back to where it was before the <see cref="SetClip"/> method was called.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> to have its clipping region reset.</param>
        protected internal virtual void ResetClip(ISvgRenderer renderer)
        {
            if (_previousClip != null)
            {
                renderer.SetClip(_previousClip);
                _previousClip = null;
            }
        }

        /// <summary>
        /// Sets the clipping region of the specified <see cref="ISvgRenderer"/>.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> to have its clipping region set.</param>
        void ISvgClipable.SetClip(ISvgRenderer renderer)
        {
            SetClip(renderer);
        }

        /// <summary>
        /// Resets the clipping region of the specified <see cref="ISvgRenderer"/> back to where it was before the <see cref="SetClip"/> method was called.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> to have its clipping region reset.</param>
        void ISvgClipable.ResetClip(ISvgRenderer renderer)
        {
            ResetClip(renderer);
        }
    }
}