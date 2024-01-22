// THIS FILE IS PART OF ExCSS PROJECT
// THE ExCSS PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHT (C) TylerBrinks. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/TylerBrinks/ExCSS

using System.Globalization;

namespace AntdUI.Svg.ExCSS.Model.TextBlocks
{
    internal class DelimiterBlock : CharacterBlock
    {
        internal DelimiterBlock()
        {
            GrammarSegment = GrammarSegment.Delimiter;
        }

        internal DelimiterBlock(char value) : base(value)
        {
            GrammarSegment = GrammarSegment.Delimiter;
        }

        public override string ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }
    }
}
