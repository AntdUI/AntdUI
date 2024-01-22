// THIS FILE IS PART OF ExCSS PROJECT
// THE ExCSS PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHT (C) TylerBrinks. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/TylerBrinks/ExCSS

using AntdUI.Svg.ExCSS.Model;
using System;

// ReSharper disable once CheckNamespace
namespace AntdUI.Svg.ExCSS
{
    public struct CombinatorSelector
    {
        public BaseSelector Selector;
        public Combinator Delimiter;

        public CombinatorSelector(BaseSelector selector, Combinator delimiter)
        {
            Selector = selector;
            Delimiter = delimiter;
        }

        public char Character
        {
            get
            {
                switch (Delimiter)
                {
                    case Combinator.Child:
                        return Specification.GreaterThan;

                    case Combinator.AdjacentSibling:
                        return Specification.PlusSign;

                    case Combinator.Descendent:
                        return Specification.Space;

                    case Combinator.Sibling:
                        return Specification.Tilde;

                    case Combinator.Namespace:
                        return Specification.Pipe;

                    default:
                        throw new NotImplementedException("Unknown combinator: " + Delimiter);
                }
            }
        }
    }
}

