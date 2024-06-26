// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

namespace AntdUI.Svg.Pathing
{
    public sealed class SvgClosePathSegment : SvgPathSegment
    {
        public override void AddToPath(System.Drawing.Drawing2D.GraphicsPath graphicsPath)
        {
            var pathData = graphicsPath.PathData;

            if (pathData.Points.Length > 0)
            {
                // Important for custom line caps.  Force the path the close with an explicit line, not just an implicit close of the figure.

                if (!pathData.Points[0].Equals(pathData.Points[pathData.Points.Length - 1]))
                {
                    int i = pathData.Points.Length - 1;
                    while (i >= 0 && pathData.Types[i] > 0) i--;
                    if (i < 0) i = 0;
                    graphicsPath.AddLine(pathData.Points[pathData.Points.Length - 1], pathData.Points[i]);
                }

                graphicsPath.CloseFigure();
            }
        }

        public override string ToString()
        {
            return "z";
        }
    }
}