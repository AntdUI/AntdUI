// THIS FILE IS PART OF ExCSS PROJECT
// THE ExCSS PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHT (C) TylerBrinks. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/TylerBrinks/ExCSS

using System;
using System.Globalization;

namespace AntdUI.Svg.ExCSS.Model.TextBlocks
{
    internal class NumericBlock : Block
    {
        private readonly string _data;

        internal NumericBlock(string number)
        {
            _data = number;
            GrammarSegment = GrammarSegment.Number;
        }

        public Single Value
        {
            get { return Single.Parse(_data, CultureInfo.InvariantCulture); }
        }

        public override string ToString()
        {
            return _data;
        }
    }
}
