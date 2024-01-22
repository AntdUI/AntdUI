// THIS FILE IS PART OF ExCSS PROJECT
// THE ExCSS PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHT (C) TylerBrinks. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/TylerBrinks/ExCSS

namespace AntdUI.Svg.ExCSS.Model.TextBlocks
{
    internal abstract class CharacterBlock : Block
    {
        private readonly char _value;

        protected CharacterBlock()
        {
            _value = Specification.Null;
        }

        protected CharacterBlock(char value)
        {
            _value = value;
        }

        internal char Value
        {
            get { return _value; }
        }
    }
}
