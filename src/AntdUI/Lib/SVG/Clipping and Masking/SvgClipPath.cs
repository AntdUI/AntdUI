// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using AntdUI.Svg.Transforms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace AntdUI.Svg
{
    /// <summary>
    /// Defines a path that can be used by other <see cref="ISvgClipable"/> elements.
    /// </summary>
    public sealed class SvgClipPath : SvgElement
    {
        public override string ClassName { get => "clipPath"; }

        private bool _pathDirty = true;

        /// <summary>
        /// Specifies the coordinate system for the clipping path.
        /// </summary>
        [SvgAttribute("clipPathUnits")]
        public SvgCoordinateUnits ClipPathUnits { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgClipPath"/> class.
        /// </summary>
        public SvgClipPath()
        {
            ClipPathUnits = SvgCoordinateUnits.Inherit;
        }

        private GraphicsPath cachedClipPath = null;

        /// <summary>
        /// Gets this <see cref="SvgClipPath"/>'s region to be used as a clipping region.
        /// </summary>
        /// <returns>A new <see cref="Region"/> containing the <see cref="Region"/> to be used for clipping.</returns>
        public Region GetClipRegion(SvgVisualElement owner)
        {
            if (cachedClipPath == null || _pathDirty)
            {
                cachedClipPath = new GraphicsPath();

                foreach (SvgElement element in Children)
                {
                    CombinePaths(cachedClipPath, element);
                }

                _pathDirty = false;
            }

            var result = cachedClipPath;
            if (ClipPathUnits == SvgCoordinateUnits.ObjectBoundingBox)
            {
                result = (GraphicsPath)cachedClipPath.Clone();
                using (var transform = new Matrix())
                {
                    var bounds = owner.Bounds;
                    transform.Scale(bounds.Width, bounds.Height, MatrixOrder.Append);
                    transform.Translate(bounds.Left, bounds.Top, MatrixOrder.Append);
                    result.Transform(transform);
                }
            }

            return new Region(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="element"></param>
        private void CombinePaths(GraphicsPath path, SvgElement element)
        {
            var graphicsElement = element as SvgVisualElement;

            if (graphicsElement != null && graphicsElement.Path(null) != null)
            {
                path.FillMode = (graphicsElement.ClipRule == SvgClipRule.NonZero) ? FillMode.Winding : FillMode.Alternate;

                GraphicsPath childPath = graphicsElement.Path(null);

                if (graphicsElement.Transforms != null)
                {
                    foreach (SvgTransform transform in graphicsElement.Transforms)
                    {
                        childPath.Transform(transform.Matrix);
                    }
                }

                if (childPath.PointCount > 0) path.AddPath(childPath, false);
            }

            foreach (SvgElement child in element.Children)
            {
                CombinePaths(path, child);
            }
        }

        /// <summary>
        /// Called by the underlying <see cref="SvgElement"/> when an element has been added to the
        /// 'Children' collection.
        /// </summary>
        /// <param name="child">The <see cref="SvgElement"/> that has been added.</param>
        /// <param name="index">An <see cref="int"/> representing the index where the element was added to the collection.</param>
        protected override void AddElement(SvgElement child, int index)
        {
            base.AddElement(child, index);
            _pathDirty = true;
        }

        /// <summary>
        /// Called by the underlying <see cref="SvgElement"/> when an element has been removed from the
        /// <see cref="SvgElement.Children"/> collection.
        /// </summary>
        /// <param name="child">The <see cref="SvgElement"/> that has been removed.</param>
        protected override void RemoveElement(SvgElement child)
        {
            base.RemoveElement(child);
            _pathDirty = true;
        }

        /// <summary>
        /// Renders the <see cref="SvgElement"/> and contents to the specified <see cref="ISvgRenderer"/> object.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> object to render to.</param>
        protected override void Render(ISvgRenderer renderer)
        {
            // Do nothing
        }
    }
}