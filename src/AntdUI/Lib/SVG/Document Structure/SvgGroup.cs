// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System.Drawing;
using System.Drawing.Drawing2D;

namespace AntdUI.Svg
{
    /// <summary>
    /// An element used to group SVG shapes.
    /// </summary>
    public class SvgGroup : SvgMarkerElement
    {
        public override string ClassName { get => "g"; }

        bool markersSet = false;

        /// <summary>
        /// If the group has marker attributes defined, add them to all children
        /// that are able to display markers. Only done once.
        /// </summary>
        private void AddMarkers()
        {
            if (!markersSet)
            {
                if (MarkerStart != null || MarkerMid != null || MarkerEnd != null)
                {
                    foreach (var c in Children)
                    {
                        if (c is SvgMarkerElement)
                        {
                            if (MarkerStart != null && ((SvgMarkerElement)c).MarkerStart == null)
                            {
                                ((SvgMarkerElement)c).MarkerStart = MarkerStart;
                            }
                            if (MarkerMid != null && ((SvgMarkerElement)c).MarkerMid == null)
                            {
                                ((SvgMarkerElement)c).MarkerMid = MarkerMid;
                            }
                            if (MarkerEnd != null && ((SvgMarkerElement)c).MarkerEnd == null)
                            {
                                ((SvgMarkerElement)c).MarkerEnd = MarkerEnd;
                            }
                        }
                    }
                }
                markersSet = true;
            }
        }

        /// <summary>
        /// Add group markers to children before rendering them.
        /// This is only done on first rendering.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> to render the child <see cref="SvgElement"/>s to.</param>
        protected override void Render(ISvgRenderer renderer)
        {
            AddMarkers();
            base.Render(renderer);
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

        protected override bool Renderable { get { return false; } }
    }
}