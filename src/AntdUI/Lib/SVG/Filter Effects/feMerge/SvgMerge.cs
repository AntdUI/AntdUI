// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System.Drawing;
using System.Linq;

namespace AntdUI.Svg.FilterEffects
{
    [SvgElement("feMerge")]
    public class SvgMerge : SvgFilterPrimitive
    {
        public override void Process(ImageBuffer buffer)
        {
            var children = this.Children.OfType<SvgMergeNode>().ToList();
            var inputImage = buffer[children.First().Input];
            var result = new Bitmap(inputImage.Width, inputImage.Height);
            using (var g = Graphics.FromImage(result))
            {
                foreach (var child in children)
                {
                    g.DrawImage(buffer[child.Input], new Rectangle(0, 0, inputImage.Width, inputImage.Height),
                                0, 0, inputImage.Width, inputImage.Height, GraphicsUnit.Pixel);
                }
                g.Flush();
            }
            buffer[this.Result] = result;
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgMerge>();
        }

    }
}