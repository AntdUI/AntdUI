// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System.Drawing.Drawing2D;

namespace AntdUI.Svg
{
    /// <summary>
    /// Represents and SVG ellipse element.
    /// </summary>
    public class SvgEllipse : SvgPathBasedElement
    {
        public override string ClassName { get => "ellipse"; }

        private GraphicsPath _path;

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

        [SvgAttribute("rx")]
        public virtual SvgUnit RadiusX
        {
            get { return Attributes.GetAttribute<SvgUnit>("rx"); }
            set { Attributes["rx"] = value; IsPathDirty = true; }
        }

        [SvgAttribute("ry")]
        public virtual SvgUnit RadiusY
        {
            get { return Attributes.GetAttribute<SvgUnit>("ry"); }
            set { Attributes["ry"] = value; IsPathDirty = true; }
        }

        /// <summary>
        /// Gets the <see cref="GraphicsPath"/> for this element.
        /// </summary>
        /// <value></value>
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

                var center = SvgUnit.GetDevicePoint(CenterX, CenterY, renderer, this);
                var radiusX = RadiusX.ToDeviceValue(renderer, UnitRenderingType.Other, this) + halfStrokeWidth;
                var radiusY = RadiusY.ToDeviceValue(renderer, UnitRenderingType.Other, this) + halfStrokeWidth;

                _path = new GraphicsPath();
                _path.StartFigure();
                _path.AddEllipse(center.X - radiusX, center.Y - radiusY, 2 * radiusX, 2 * radiusY);
                _path.CloseFigure();
            }
            return _path;
        }

        /// <summary>
        /// Renders the <see cref="SvgElement"/> and contents using the specified <see cref="ISvgRenderer"/> object.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> object used for rendering.</param>
        protected override void Render(ISvgRenderer renderer)
        {
            if (RadiusX.Value > 0.0f && RadiusY.Value > 0.0f)
            {
                base.Render(renderer);
            }
        }
    }
}