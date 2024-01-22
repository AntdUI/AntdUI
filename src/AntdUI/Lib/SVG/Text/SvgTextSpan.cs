// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

namespace AntdUI.Svg
{
    [SvgElement("tspan")]
    public class SvgTextSpan : SvgTextBase
    {
        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgTextSpan>();
        }

        public override SvgElement DeepCopy<T>()
        {
            var newObj = base.DeepCopy<T>() as SvgTextSpan;
            newObj.X = this.X;
            newObj.Y = this.Y;
            newObj.Dx = this.Dx;
            newObj.Dy = this.Dy;
            newObj.Text = this.Text;

            return newObj;
        }


    }
}