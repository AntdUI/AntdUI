// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

namespace AntdUI.Svg
{
    /// <summary>
    /// The <see cref="SvgText"/> element defines a graphics element consisting of text.
    /// </summary>
    public class SvgText : SvgTextBase
    {
        public override string ClassName { get => "text"; }

        /// <summary>
        /// Initializes the <see cref="SvgText"/> class.
        /// </summary>
        public SvgText() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgText"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        public SvgText(string text)
            : this()
        {
            Text = text;
        }
    }
}