// THIS FILE IS PART OF ExCSS PROJECT
// THE ExCSS PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHT (C) TylerBrinks. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/TylerBrinks/ExCSS

namespace AntdUI.Svg.ExCSS.Model.TextBlocks
{
    internal abstract class Block
    {
        internal GrammarSegment GrammarSegment { get; set; }

        internal static PipeBlock Column
        {
            get { return PipeBlock.Token; }
        }

        internal static DelimiterBlock Delim(char value)
        {
            return new DelimiterBlock(value);
        }

        internal static NumericBlock Number(string value)
        {
            return new NumericBlock(value);
        }

        internal static RangeBlock Range(string start, string end)
        {
            return new RangeBlock().SetRange(start, end);
        }
    }
}
