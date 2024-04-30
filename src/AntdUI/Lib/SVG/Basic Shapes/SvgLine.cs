// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System.Drawing;
using System.Drawing.Drawing2D;

namespace AntdUI.Svg
{
    /// <summary>
    /// Represents and SVG line element.
    /// </summary>
    public class SvgLine : SvgMarkerElement
    {
        public override string ClassName { get => "line"; }

        private SvgUnit _startX;
        private SvgUnit _startY;
        private SvgUnit _endX;
        private SvgUnit _endY;
        private GraphicsPath _path;

        [SvgAttribute("x1")]
        public SvgUnit StartX
        {
            get { return _startX; }
            set
            {
                if (_startX != value)
                {
                    _startX = value;
                    IsPathDirty = true;
                    OnAttributeChanged(new AttributeEventArgs { Attribute = "x1", Value = value });
                }
            }
        }

        [SvgAttribute("y1")]
        public SvgUnit StartY
        {
            get { return _startY; }
            set
            {
                if (_startY != value)
                {
                    _startY = value;
                    IsPathDirty = true;
                    OnAttributeChanged(new AttributeEventArgs { Attribute = "y1", Value = value });
                }
            }
        }

        [SvgAttribute("x2")]
        public SvgUnit EndX
        {
            get { return _endX; }
            set
            {
                if (_endX != value)
                {
                    _endX = value;
                    IsPathDirty = true;
                    OnAttributeChanged(new AttributeEventArgs { Attribute = "x2", Value = value });
                }
            }
        }

        [SvgAttribute("y2")]
        public SvgUnit EndY
        {
            get { return _endY; }
            set
            {
                if (_endY != value)
                {
                    _endY = value;
                    IsPathDirty = true;
                    OnAttributeChanged(new AttributeEventArgs { Attribute = "y2", Value = value });
                }
            }
        }

        public override SvgPaintServer Fill
        {
            get { return null; /* Line can't have a fill */ }
            set
            {
                // Do nothing
            }
        }

        public SvgLine()
        {
        }

        public override GraphicsPath Path(ISvgRenderer renderer)
        {
            if ((_path == null || IsPathDirty) && base.StrokeWidth > 0)
            {
                PointF start = new PointF(StartX.ToDeviceValue(renderer, UnitRenderingType.Horizontal, this),
                                          StartY.ToDeviceValue(renderer, UnitRenderingType.Vertical, this));
                PointF end = new PointF(EndX.ToDeviceValue(renderer, UnitRenderingType.Horizontal, this),
                                        EndY.ToDeviceValue(renderer, UnitRenderingType.Vertical, this));

                _path = new GraphicsPath();

                // If it is to render, don't need to consider stroke width.
                // i.e stroke width only to be considered when calculating boundary
                if (renderer != null)
                {
                    _path.AddLine(start, end);
                    IsPathDirty = false;
                }
                else
                {    // only when calculating boundary 
                    _path.StartFigure();
                    var radius = base.StrokeWidth / 2;
                    _path.AddEllipse(start.X - radius, start.Y - radius, 2 * radius, 2 * radius);
                    _path.AddEllipse(end.X - radius, end.Y - radius, 2 * radius, 2 * radius);
                    _path.CloseFigure();
                }
            }
            return _path;
        }
    }
}