// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace AntdUI.Svg
{
    /// <summary>
    /// An <see cref="SvgFragment"/> represents an SVG fragment that can be the root element or an embedded fragment of an SVG document.
    /// </summary>
    public class SvgFragment : SvgElement, ISvgViewPort, ISvgBoundable
    {
        public override string ClassName { get => "svg"; }

        /// <summary>
        /// Gets the SVG namespace string.
        /// </summary>
        public static readonly Uri Namespace = new Uri("http://www.w3.org/2000/svg");

        PointF ISvgBoundable.Location
        {
            get
            {
                return PointF.Empty;
            }
        }

        SizeF ISvgBoundable.Size
        {
            get
            {
                return GetDimensions();
            }
        }

        RectangleF ISvgBoundable.Bounds
        {
            get
            {
                return new RectangleF(((ISvgBoundable)this).Location, ((ISvgBoundable)this).Size);
            }
        }

        private SvgUnit _x;
        private SvgUnit _y;

        /// <summary>
        /// Gets or sets the position where the left point of the svg should start.
        /// </summary>
        [SvgAttribute("x")]
        public SvgUnit X
        {
            get { return _x; }
            set
            {
                if (_x != value)
                {
                    _x = value;
                    OnAttributeChanged(new AttributeEventArgs { Attribute = "x", Value = value });
                }
            }
        }

        /// <summary>
        /// Gets or sets the position where the top point of the svg should start.
        /// </summary>
        [SvgAttribute("y")]
        public SvgUnit Y
        {
            get { return _y; }
            set
            {
                if (_y != value)
                {
                    _y = value;
                    OnAttributeChanged(new AttributeEventArgs { Attribute = "y", Value = value });
                }
            }
        }

        /// <summary>
        /// Gets or sets the width of the fragment.
        /// </summary>
        /// <value>The width.</value>
        [SvgAttribute("width")]
        public SvgUnit Width
        {
            get { return Attributes.GetAttribute<SvgUnit>("width"); }
            set { Attributes["width"] = value; }
        }

        /// <summary>
        /// Gets or sets the height of the fragment.
        /// </summary>
        /// <value>The height.</value>
        [SvgAttribute("height")]
        public SvgUnit Height
        {
            get { return Attributes.GetAttribute<SvgUnit>("height"); }
            set { Attributes["height"] = value; }
        }

        [SvgAttribute("overflow")]
        public virtual SvgOverflow Overflow
        {
            get { return Attributes.GetAttribute<SvgOverflow>("overflow"); }
            set { Attributes["overflow"] = value; }
        }

        /// <summary>
        /// Gets or sets the viewport of the element.
        /// </summary>
        /// <value></value>
        [SvgAttribute("viewBox")]
        public SvgViewBox ViewBox
        {
            get { return Attributes.GetAttribute<SvgViewBox>("viewBox"); }
            set { Attributes["viewBox"] = value; }
        }

        /// <summary>
        /// Gets or sets the aspect of the viewport.
        /// </summary>
        /// <value></value>
        [SvgAttribute("preserveAspectRatio")]
        public SvgAspectRatio AspectRatio
        {
            get { return Attributes.GetAttribute<SvgAspectRatio>("preserveAspectRatio"); }
            set { Attributes["preserveAspectRatio"] = value; }
        }

        /// <summary>
        /// Refers to the size of the font from baseline to baseline when multiple lines of text are set solid in a multiline layout environment.
        /// </summary>
        [SvgAttribute("font-size")]
        public override SvgUnit FontSize
        {
            get { return (Attributes["font-size"] == null) ? SvgUnit.Empty : (SvgUnit)Attributes["font-size"]; }
            set { Attributes["font-size"] = value; }
        }

        /// <summary>
        /// Indicates which font family is to be used to render the text.
        /// </summary>
        [SvgAttribute("font-family")]
        public override string FontFamily
        {
            get { return Attributes["font-family"] as string; }
            set { Attributes["font-family"] = value; }
        }

        /// <summary>
        /// Applies the required transforms to <see cref="ISvgRenderer"/>.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> to be transformed.</param>
        protected internal override bool PushTransforms(ISvgRenderer renderer)
        {
            if (!base.PushTransforms(renderer)) return false;
            ViewBox.AddViewBoxTransform(AspectRatio, renderer, this);
            return true;
        }

        protected override void Render(ISvgRenderer renderer)
        {
            switch (Overflow)
            {
                case SvgOverflow.Auto:
                case SvgOverflow.Visible:
                case SvgOverflow.Inherit:
                    base.Render(renderer);
                    break;
                default:
                    var prevClip = renderer.GetClip();
                    try
                    {
                        var size = (Parent == null ? renderer.GetBoundable().Bounds.Size : GetDimensions());
                        var clip = new RectangleF(X.ToDeviceValue(renderer, UnitRenderingType.Horizontal, this), Y.ToDeviceValue(renderer, UnitRenderingType.Vertical, this), size.Width, size.Height);
                        renderer.SetClip(new Region(clip), CombineMode.Intersect);
                        base.Render(renderer);
                    }
                    finally
                    {
                        renderer.SetClip(prevClip, CombineMode.Replace);
                    }
                    break;
            }
        }

        /// <summary>
        /// Gets the <see cref="GraphicsPath"/> for this element.
        /// </summary>
        /// <value></value>
        public GraphicsPath Path
        {
            get
            {
                var path = new GraphicsPath();
                AddPaths(this, path);
                return path;
            }
        }

        /// <summary>
        /// Gets the bounds of the svg element.
        /// </summary>
        /// <value>The bounds.</value>
        public RectangleF Bounds
        {
            get
            {
                var bounds = new RectangleF();
                foreach (var child in Children)
                {
                    RectangleF childBounds = new RectangleF();
                    if (child is SvgFragment)
                    {
                        childBounds = ((SvgFragment)child).Bounds;
                        childBounds.Offset(((SvgFragment)child).X, ((SvgFragment)child).Y);
                    }
                    else if (child is SvgVisualElement) childBounds = ((SvgVisualElement)child).Bounds;

                    if (!childBounds.IsEmpty)
                    {
                        if (bounds.IsEmpty) bounds = childBounds;
                        else bounds = RectangleF.Union(bounds, childBounds);
                    }
                }
                return TransformedBounds(bounds);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgFragment"/> class.
        /// </summary>
        public SvgFragment()
        {
            _x = 0.0f;
            _y = 0.0f;
            Height = new SvgUnit(SvgUnitType.Percentage, 100.0f);
            Width = new SvgUnit(SvgUnitType.Percentage, 100.0f);
            ViewBox = SvgViewBox.Empty;
            AspectRatio = new SvgAspectRatio(SvgPreserveAspectRatio.xMidYMid);
        }

        public SizeF GetDimensions()
        {
            float w, h;
            var isWidthperc = Width.Type == SvgUnitType.Percentage;
            var isHeightperc = Height.Type == SvgUnitType.Percentage;

            RectangleF bounds = new RectangleF();
            if (isWidthperc || isHeightperc)
            {
                if (ViewBox.Width > 0 && ViewBox.Height > 0) bounds = new RectangleF(ViewBox.MinX, ViewBox.MinY, ViewBox.Width, ViewBox.Height);
                else bounds = Bounds; //do just one call to the recursive bounds property
            }

            if (isWidthperc) w = (bounds.Width + bounds.X) * (Width.Value * 0.01f);
            else w = Width.ToDeviceValue(null, UnitRenderingType.Horizontal, this);
            if (isHeightperc) h = (bounds.Height + bounds.Y) * (Height.Value * 0.01f);
            else h = Height.ToDeviceValue(null, UnitRenderingType.Vertical, this);
            return new SizeF(w, h);
        }
    }
}