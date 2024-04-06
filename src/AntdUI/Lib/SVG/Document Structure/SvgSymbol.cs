// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System.Drawing;

namespace AntdUI.Svg.Document_Structure
{
    /// <summary>
    /// An element used to group SVG shapes.
    /// </summary>
    public class SvgSymbol : SvgVisualElement
    {
        public override string ClassName { get => "symbol"; }


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
        /// Gets the <see cref="System.Drawing.Drawing2D.GraphicsPath"/> for this element.
        /// </summary>
        /// <value></value>
        public override System.Drawing.Drawing2D.GraphicsPath Path(ISvgRenderer renderer)
        {
            return GetPaths(this, renderer);
        }

        /// <summary>
        /// Gets the bounds of the element.
        /// </summary>
        /// <value>The bounds.</value>
        public override System.Drawing.RectangleF Bounds
        {
            get
            {
                var r = new RectangleF();
                foreach (var c in Children)
                {
                    if (c is SvgVisualElement)
                    {
                        // First it should check if rectangle is empty or it will return the wrong Bounds.
                        // This is because when the Rectangle is Empty, the Union method adds as if the first values where X=0, Y=0
                        if (r.IsEmpty)
                        {
                            r = ((SvgVisualElement)c).Bounds;
                        }
                        else
                        {
                            var childBounds = ((SvgVisualElement)c).Bounds;
                            if (!childBounds.IsEmpty)
                            {
                                r = RectangleF.Union(r, childBounds);
                            }
                        }
                    }
                }

                return TransformedBounds(r);
            }
        }

        protected override bool Renderable { get { return false; } }

        /// <summary>
        /// Applies the required transforms to <see cref="ISvgRenderer"/>.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> to be transformed.</param>
        protected internal override bool PushTransforms(ISvgRenderer renderer)
        {
            if (!base.PushTransforms(renderer)) return false;
            ViewBox.AddViewBoxTransform(AspectRatio, renderer, null);
            return true;
        }

        // Only render if the parent is set to a Use element
        protected override void Render(ISvgRenderer renderer)
        {
            if (_parent is SvgUse) base.Render(renderer);
        }
    }
}