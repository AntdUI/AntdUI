// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System.ComponentModel;
using System.Drawing;

namespace AntdUI.Svg
{
    public struct SvgPoint
    {
        private SvgUnit x;
        private SvgUnit y;

        public SvgUnit X
        {
            get { return x; }
            set { x = value; }
        }

        public SvgUnit Y
        {
            get { return y; }
            set { y = value; }
        }

        public PointF ToDeviceValue(ISvgRenderer renderer, SvgElement owner)
        {
            return SvgUnit.GetDevicePoint(X, Y, renderer, owner);
        }

        public bool IsEmpty()
        {
            return (X.Value == 0.0f && Y.Value == 0.0f);
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;

            if (!(obj.GetType() == typeof(SvgPoint))) return false;

            var point = (SvgPoint)obj;
            return (point.X.Equals(X) && point.Y.Equals(Y));
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public SvgPoint(string _x, string _y)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(SvgUnit));

            x = (SvgUnit)converter.ConvertFrom(_x);
            y = (SvgUnit)converter.ConvertFrom(_y);
        }

        public SvgPoint(SvgUnit _x, SvgUnit _y)
        {
            x = _x;
            y = _y;
        }
    }
}