// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System.Drawing;
using System.Drawing.Drawing2D;

namespace AntdUI.Svg
{
    /// <summary>
    /// The 'switch' element evaluates the 'requiredFeatures', 'requiredExtensions' and 'systemLanguage' attributes on its direct child elements in order, and then processes and renders the first child for which these attributes evaluate to true
    /// </summary>
    public class SvgSwitch : SvgVisualElement
    {
        public override string ClassName { get => "switch"; }

        public SvgSwitch()
        {
        }

        /// <summary>
        /// Gets the <see cref="GraphicsPath"/> for this element.
        /// </summary>
        /// <value></value>
        public override GraphicsPath Path(ISvgRenderer renderer)
        {
            return GetPaths(this, renderer);
        }

        /// <summary>
        /// Gets the bounds of the element.
        /// </summary>
        /// <value>The bounds.</value>
        public override RectangleF Bounds
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

        /// <summary>
        /// Renders the <see cref="SvgElement"/> and contents to the specified <see cref="Graphics"/> object.
        /// </summary>
        /// <param name="renderer">The <see cref="Graphics"/> object to render to.</param>
        protected override void Render(ISvgRenderer renderer)
        {
            if (!Visible || !Displayable)
                return;

            PushTransforms(renderer);
            SetClip(renderer);
            base.RenderChildren(renderer);
            ResetClip(renderer);
            PopTransforms(renderer);
        }
    }
}