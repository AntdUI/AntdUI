// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace AntdUI.Svg
{
    /// <summary>
    /// It is often desirable to specify that a given set of graphics stretch to fit a particular container element. The viewBox attribute provides this capability.
    /// </summary>
    public struct SvgViewBox
    {
        public static readonly SvgViewBox Empty = new SvgViewBox();

        /// <summary>
        /// Gets or sets the position where the viewport starts horizontally.
        /// </summary>
        public float MinX { get; set; }

        /// <summary>
        /// Gets or sets the position where the viewport starts vertically.
        /// </summary>
        public float MinY { get; set; }

        /// <summary>
        /// Gets or sets the width of the viewport.
        /// </summary>
        public float Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the viewport.
        /// </summary>
        public float Height { get; set; }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Svg.SvgViewBox"/> to <see cref="System.Drawing.RectangleF"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator RectangleF(SvgViewBox value)
        {
            return new RectangleF(value.MinX, value.MinY, value.Width, value.Height);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Drawing.RectangleF"/> to <see cref="Svg.SvgViewBox"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator SvgViewBox(RectangleF value)
        {
            return new SvgViewBox(value.X, value.Y, value.Width, value.Height);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgViewBox"/> struct.
        /// </summary>
        /// <param name="minX">The min X.</param>
        /// <param name="minY">The min Y.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public SvgViewBox(float minX, float minY, float width, float height) : this()
        {
            MinX = minX;
            MinY = minY;
            Width = width;
            Height = height;
        }

        #region Equals and GetHashCode implementation
        public override bool Equals(object? obj)
        {
            return (obj is SvgViewBox) && Equals((SvgViewBox)obj);
        }

        public bool Equals(SvgViewBox other)
        {
            return MinX == other.MinX
                && MinY == other.MinY
                && Width == other.Width
                && Height == other.Height;
        }

        public override int GetHashCode()
        {
            int hashCode = 0;
            unchecked
            {
                hashCode += 1000000007 * MinX.GetHashCode();
                hashCode += 1000000009 * MinY.GetHashCode();
                hashCode += 1000000021 * Width.GetHashCode();
                hashCode += 1000000033 * Height.GetHashCode();
            }
            return hashCode;
        }

        public static bool operator ==(SvgViewBox lhs, SvgViewBox rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(SvgViewBox lhs, SvgViewBox rhs)
        {
            return !(lhs == rhs);
        }
        #endregion

        public void AddViewBoxTransform(SvgAspectRatio aspectRatio, ISvgRenderer renderer, SvgFragment frag)
        {
            var x = (frag == null ? 0 : frag.X.ToDeviceValue(renderer, UnitRenderingType.Horizontal, frag));
            var y = (frag == null ? 0 : frag.Y.ToDeviceValue(renderer, UnitRenderingType.Vertical, frag));

            if (Equals(SvgViewBox.Empty))
            {
                renderer.TranslateTransform(x, y, MatrixOrder.Prepend);
                return;
            }

            var width = (frag == null ? Width : frag.Width.ToDeviceValue(renderer, UnitRenderingType.Horizontal, frag));
            var height = (frag == null ? Height : frag.Height.ToDeviceValue(renderer, UnitRenderingType.Vertical, frag));

            var fScaleX = width / Width;
            var fScaleY = height / Height; //(MinY < 0 ? -1 : 1) * 
            var fMinX = -MinX * fScaleX;
            var fMinY = -MinY * fScaleY;

            if (aspectRatio == null) aspectRatio = new SvgAspectRatio(SvgPreserveAspectRatio.xMidYMid, false);
            if (aspectRatio.Align != SvgPreserveAspectRatio.none)
            {
                if (aspectRatio.Slice)
                {
                    fScaleX = Math.Max(fScaleX, fScaleY);
                    fScaleY = Math.Max(fScaleX, fScaleY);
                }
                else
                {
                    fScaleX = Math.Min(fScaleX, fScaleY);
                    fScaleY = Math.Min(fScaleX, fScaleY);
                }
                float fViewMidX = (Width / 2) * fScaleX;
                float fViewMidY = (Height / 2) * fScaleY;
                float fMidX = width / 2;
                float fMidY = height / 2;
                fMinX = -MinX * fScaleX;
                fMinY = -MinY * fScaleY;

                switch (aspectRatio.Align)
                {
                    case SvgPreserveAspectRatio.xMinYMin:
                        break;
                    case SvgPreserveAspectRatio.xMidYMin:
                        fMinX += fMidX - fViewMidX;
                        break;
                    case SvgPreserveAspectRatio.xMaxYMin:
                        fMinX += width - Width * fScaleX;
                        break;
                    case SvgPreserveAspectRatio.xMinYMid:
                        fMinY += fMidY - fViewMidY;
                        break;
                    case SvgPreserveAspectRatio.xMidYMid:
                        fMinX += fMidX - fViewMidX;
                        fMinY += fMidY - fViewMidY;
                        break;
                    case SvgPreserveAspectRatio.xMaxYMid:
                        fMinX += width - Width * fScaleX;
                        fMinY += fMidY - fViewMidY;
                        break;
                    case SvgPreserveAspectRatio.xMinYMax:
                        fMinY += height - Height * fScaleY;
                        break;
                    case SvgPreserveAspectRatio.xMidYMax:
                        fMinX += fMidX - fViewMidX;
                        fMinY += height - Height * fScaleY;
                        break;
                    case SvgPreserveAspectRatio.xMaxYMax:
                        fMinX += width - Width * fScaleX;
                        fMinY += height - Height * fScaleY;
                        break;
                    default:
                        break;
                }
            }

            renderer.TranslateTransform(x, y, MatrixOrder.Prepend);
            renderer.TranslateTransform(fMinX, fMinY, MatrixOrder.Prepend);
            renderer.ScaleTransform(fScaleX, fScaleY, MatrixOrder.Prepend);
        }
    }
    internal class SvgViewBoxConverter
    {
        public static SvgViewBox Parse(string value)
        {
            var coords = (value).Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (coords.Length != 4) throw new SvgException("The 'viewBox' attribute must be in the format 'minX, minY, width, height'.");
            return new SvgViewBox(float.Parse(coords[0]), float.Parse(coords[1]), float.Parse(coords[2]), float.Parse(coords[3]));
        }
    }
}