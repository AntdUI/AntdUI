// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System.Drawing;

namespace AntdUI.Svg.FilterEffects
{
    /// <summary>
    /// Note: this is not used in calculations to bitmap - used only to allow for svg xml output
    /// </summary>
    public class SvgOffset : SvgFilterPrimitive
    {
        public override string ClassName { get => "feOffset"; }

        /// <summary>
        /// The amount to offset the input graphic along the x-axis. The offset amount is expressed in the coordinate system established by attribute ‘primitiveUnits?on the ‘filter?element.
        /// If the attribute is not specified, then the effect is as if a value of 0 were specified.
        /// Note: this is not used in calculations to bitmap - used only to allow for svg xml output
        /// </summary>
        [SvgAttribute("dx")]
        public SvgUnit Dx { get; set; }

        /// <summary>
        /// The amount to offset the input graphic along the y-axis. The offset amount is expressed in the coordinate system established by attribute ‘primitiveUnits?on the ‘filter?element.
        /// If the attribute is not specified, then the effect is as if a value of 0 were specified.
        /// Note: this is not used in calculations to bitmap - used only to allow for svg xml output
        /// </summary>
        [SvgAttribute("dy")]
        public SvgUnit Dy { get; set; }

        public override void Process(ImageBuffer buffer)
        {
            var inputImage = buffer[Input];
            var result = new Bitmap(inputImage.Width, inputImage.Height);

            var pts = new PointF[] { new PointF(Dx.ToDeviceValue(null, UnitRenderingType.Horizontal, null), Dy.ToDeviceValue(null, UnitRenderingType.Vertical, null)) };
            buffer.Transform.TransformVectors(pts);

            using (var g = Graphics.FromImage(result))
            {
                g.DrawImage(inputImage, new Rectangle((int)pts[0].X, (int)pts[0].Y, inputImage.Width, inputImage.Height), 0, 0, inputImage.Width, inputImage.Height, GraphicsUnit.Pixel);
                g.Flush();
            }
            buffer[Result] = result;
        }
    }
}