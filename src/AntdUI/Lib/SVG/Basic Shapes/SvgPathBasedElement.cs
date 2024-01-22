// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System.Drawing.Drawing2D;

namespace AntdUI.Svg
{
    /// <summary>
    /// Represents an element that is using a GraphicsPath as rendering base.
    /// </summary>
    public abstract class SvgPathBasedElement : SvgVisualElement
    {
        public override System.Drawing.RectangleF Bounds
        {
            get
            {
                var path = this.Path(null);
                if (path != null)
                {
                    if (Transforms != null && Transforms.Count > 0)
                    {
                        path = (GraphicsPath)path.Clone();
                        path.Transform(Transforms.GetMatrix());
                    }
                    return path.GetBounds();
                }
                return new System.Drawing.RectangleF();
            }
        }
    }
}
