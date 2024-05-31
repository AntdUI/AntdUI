// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System;
using System.Drawing.Drawing2D;

namespace AntdUI.Svg.Transforms
{
    public abstract class SvgTransform : ICloneable
    {
        public abstract Matrix Matrix { get; }
        public abstract string WriteToString();

        public abstract object Clone();

        #region Equals implementation
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj is SvgTransform other)
            {
                var thisMatrix = Matrix.Elements;
                var otherMatrix = other.Matrix.Elements;
                for (int i = 0; i < 6; i++)
                {
                    if (thisMatrix[i] != otherMatrix[i]) return false;
                }
                return true;
            }
            else return false;
        }

        public override int GetHashCode()
        {
            int hashCode = Matrix.GetHashCode();
            return hashCode;
        }


        public static bool operator ==(SvgTransform lhs, SvgTransform rhs)
        {
            if (ReferenceEquals(lhs, rhs))
                return true;
            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
                return false;
            return lhs.Equals(rhs);
        }

        public static bool operator !=(SvgTransform lhs, SvgTransform rhs)
        {
            return !(lhs == rhs);
        }
        #endregion

        public override string ToString()
        {
            return WriteToString();
        }
    }
}