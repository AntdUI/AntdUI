// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using AntdUI.Svg.Pathing;
using System.Drawing.Drawing2D;

namespace AntdUI.Svg
{
    /// <summary>
    /// Represents an SVG path element.
    /// </summary>
    public class SvgPath : SvgMarkerElement
    {
        public override string ClassName { get => "path"; }

        private GraphicsPath _path;

        /// <summary>
        /// Gets or sets a <see cref="SvgPathSegmentList"/> of path data.
        /// </summary>
        [SvgAttribute("d", true)]
        public SvgPathSegmentList PathData
        {
            get { return Attributes.GetAttribute<SvgPathSegmentList>("d"); }
            set
            {
                Attributes["d"] = value;
                value._owner = this;
                IsPathDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the length of the path.
        /// </summary>
        [SvgAttribute("pathLength", true)]
        public float PathLength
        {
            get { return Attributes.GetAttribute<float>("pathLength"); }
            set { Attributes["pathLength"] = value; }
        }

        /// <summary>
        /// Gets the <see cref="GraphicsPath"/> for this element.
        /// </summary>
        public override GraphicsPath Path(ISvgRenderer renderer)
        {
            if (_path == null || IsPathDirty)
            {
                _path = new GraphicsPath();

                foreach (var segment in PathData)
                {
                    segment.AddToPath(_path);
                }

                if (_path.PointCount == 0)
                {
                    if (PathData.Count > 0)
                    {
                        // special case with one move command only, see #223
                        var segment = PathData.Last;
                        _path.AddLine(segment.End, segment.End);
                        Fill = SvgPaintServer.None;
                    }
                    else
                    {
                        _path = null;
                    }
                }
                IsPathDirty = false;
            }
            return _path;
        }

        internal void OnPathUpdated()
        {
            IsPathDirty = true;
            OnAttributeChanged(new AttributeEventArgs { Attribute = "d", Value = Attributes.GetAttribute<SvgPathSegmentList>("d") });
        }

        /// <summary>
        /// Gets the bounds of the element.
        /// </summary>
        /// <value>The bounds.</value>
        public override System.Drawing.RectangleF Bounds
        {
            get { return Path(null).GetBounds(); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgPath"/> class.
        /// </summary>
        public SvgPath()
        {
            var pathData = new SvgPathSegmentList();
            Attributes["d"] = pathData;
            pathData._owner = this;
        }
    }
}