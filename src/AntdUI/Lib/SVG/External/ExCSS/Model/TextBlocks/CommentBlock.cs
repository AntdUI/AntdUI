// THIS FILE IS PART OF ExCSS PROJECT
// THE ExCSS PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHT (C) TylerBrinks. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/TylerBrinks/ExCSS

namespace AntdUI.Svg.ExCSS.Model.TextBlocks
{
    internal class CommentBlock : Block
    {
        private readonly static CommentBlock OpenBlock;
        private readonly static CommentBlock CloseBlock;

        static CommentBlock()
        {
            OpenBlock = new CommentBlock { GrammarSegment = GrammarSegment.CommentOpen };
            CloseBlock = new CommentBlock { GrammarSegment = GrammarSegment.CommentClose };
        }

        CommentBlock()
        {
        }


        internal static CommentBlock Open
        {
            get { return OpenBlock; }
        }

        internal static CommentBlock Close
        {
            get { return CloseBlock; }
        }

        public override string ToString()
        {
            return GrammarSegment == GrammarSegment.CommentOpen ? "<!--" : "-->";
        }
    }
}
