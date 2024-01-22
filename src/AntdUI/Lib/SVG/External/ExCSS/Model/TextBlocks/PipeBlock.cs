// THIS FILE IS PART OF ExCSS PROJECT
// THE ExCSS PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHT (C) TylerBrinks. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/TylerBrinks/ExCSS

namespace AntdUI.Svg.ExCSS.Model.TextBlocks
{
    internal class PipeBlock : Block
    {
        private readonly static PipeBlock TokenBlock;

        static PipeBlock()
        {
            TokenBlock = new PipeBlock();
        }

        PipeBlock()
        {
            GrammarSegment = GrammarSegment.Column;
        }

        internal static PipeBlock Token
        {
            get { return TokenBlock; }
        }

        public override string ToString()
        {
            return "||";
        }
    }
}
