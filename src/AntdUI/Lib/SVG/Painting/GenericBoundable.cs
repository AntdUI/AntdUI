// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System.Drawing;

namespace AntdUI.Svg
{
    internal class GenericBoundable : ISvgBoundable
    {
        private RectangleF _rect;

        public GenericBoundable(RectangleF rect)
        {
            _rect = rect;
        }
        public GenericBoundable(float x, float y, float width, float height)
        {
            _rect = new RectangleF(x, y, width, height);
        }

        public PointF Location
        {
            get { return _rect.Location; }
        }

        public SizeF Size
        {
            get { return _rect.Size; }
        }

        public RectangleF Bounds
        {
            get { return _rect; }
        }
    }
}