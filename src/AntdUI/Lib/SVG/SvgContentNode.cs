// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

namespace AntdUI.Svg
{
    public class SvgContentNode : ISvgNode
    {
        public string Content { get; set; }

        /// <summary>
        /// Create a deep copy of this <see cref="ISvgNode"/>.
        /// </summary>
        /// <returns>A deep copy of this <see cref="ISvgNode"/></returns>
        public ISvgNode DeepCopy()
        {
            // Since strings are immutable in C#, we can just use the same reference here.
            return new SvgContentNode { Content = Content };
        }
    }
}