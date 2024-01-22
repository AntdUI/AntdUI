// THIS FILE IS PART OF ExCSS PROJECT
// THE ExCSS PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHT (C) TylerBrinks. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/TylerBrinks/ExCSS

namespace AntdUI.Svg.ExCSS
{
    internal sealed class FirstChildSelector : BaseSelector, IToString
    {
        FirstChildSelector()
        { }

        static FirstChildSelector _instance;

        public static FirstChildSelector Instance
        {
            get { return _instance ?? (_instance = new FirstChildSelector()); }
        }

        public override string ToString(bool friendlyFormat, int indentation = 0)
        {
            return ":" + PseudoSelectorPrefix.PseudoFirstchild;
        }
    }
}