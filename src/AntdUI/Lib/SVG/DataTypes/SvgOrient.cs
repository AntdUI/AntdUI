// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

namespace AntdUI.Svg
{
    /// <summary>
    /// Represents an orientation in an Scalable Vector Graphics document.
    /// </summary>
    public class SvgOrient
    {
        private bool _isAuto = true;
        private float _angle;

        public SvgOrient()
        {
            IsAuto = false;
            Angle = 0;
        }

        public SvgOrient(bool isAuto)
        {
            IsAuto = isAuto;
        }

        public SvgOrient(float angle)
        {
            Angle = angle;
        }

        /// <summary>
        /// Gets the value of the unit.
        /// </summary>
        public float Angle
        {
            get { return _angle; }
            set
            {
                _angle = value;
                _isAuto = false;
            }
        }


        /// <summary>
        /// Gets the value of the unit.
        /// </summary>
        public bool IsAuto
        {
            get { return _isAuto; }
            set
            {
                _isAuto = value;
                _angle = 0f;
            }
        }


        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns>
        /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (!(obj.GetType() == typeof(SvgOrient))) return false;

            var unit = (SvgOrient)obj;
            return (unit.IsAuto == IsAuto && unit.Angle == Angle);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            if (IsAuto) return "auto";
            else return Angle.ToString();
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="float"/> to <see cref="Svg.SvgOrient"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator SvgOrient(float value)
        {
            return new SvgOrient(value);
        }
    }
}