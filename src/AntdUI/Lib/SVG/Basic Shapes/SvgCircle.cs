// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System.Drawing.Drawing2D;

namespace AntdUI.Svg
{
    /// <summary>
    /// An SVG element to render circles to the document.
    /// </summary>
    public class SvgCircle : SvgPathBasedElement
    {
        public override string ClassName { get => "circle"; }

        private GraphicsPath _path;

        /// <summary>
        /// Gets the center point of the circle.
        /// </summary>
        /// <value>The center.</value>
        public SvgPoint Center
        {
            get { return new SvgPoint(CenterX, CenterY); }
        }

        [SvgAttribute("cx")]
        public virtual SvgUnit CenterX
        {
            get { return Attributes.GetAttribute<SvgUnit>("cx"); }
            set { Attributes["cx"] = value; IsPathDirty = true; }
        }

        [SvgAttribute("cy")]
        public virtual SvgUnit CenterY
        {
            get { return Attributes.GetAttribute<SvgUnit>("cy"); }
            set { Attributes["cy"] = value; IsPathDirty = true; }
        }

        [SvgAttribute("r")]
        public virtual SvgUnit Radius
        {
            get { return Attributes.GetAttribute<SvgUnit>("r"); }
            set { Attributes["r"] = value; IsPathDirty = true; }
        }

        /// <summary>
        /// Gets the <see cref="GraphicsPath"/> representing this element.
        /// </summary>
        public override GraphicsPath Path(ISvgRenderer renderer)
        {
            if (_path == null || IsPathDirty)
            {
                var halfStrokeWidth = base.StrokeWidth / 2;

                // If it is to render, don't need to consider stroke width.
                // i.e stroke width only to be considered when calculating boundary
                if (renderer != null)
                {
                    halfStrokeWidth = 0;
                    IsPathDirty = false;
                }

                _path = new GraphicsPath();
                _path.StartFigure();
                var center = Center.ToDeviceValue(renderer, this);
                var radius = Radius.ToDeviceValue(renderer, UnitRenderingType.Other, this) + halfStrokeWidth;
                _path.AddEllipse(center.X - radius, center.Y - radius, 2 * radius, 2 * radius);
                _path.CloseFigure();
            }
            return _path;
        }

        /// <summary>
        /// Renders the circle using the specified <see cref="ISvgRenderer"/> object.
        /// </summary>
        /// <param name="renderer">The renderer object.</param>
        protected override void Render(ISvgRenderer renderer)
        {
            // Don't draw if there is no radius set
            if (Radius.Value > 0.0f)
            {
                base.Render(renderer);
            }
        }
    }
}