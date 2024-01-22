// THIS FILE IS PART OF ExCSS PROJECT
// THE ExCSS PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHT (C) TylerBrinks. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/TylerBrinks/ExCSS

namespace AntdUI.Svg.ExCSS
{
    internal abstract class NthChildSelector : BaseSelector, IToString
    {
        public int Step;
        public int Offset;
        internal string FunctionText { get; set; }

        internal string FormatSelector(string functionName)
        {
            var format = Offset < 0
                ? ":{0}({1}n{2})"
                : ":{0}({1}n+{2})";

            return string.IsNullOrEmpty(FunctionText)
                ? string.Format(format, functionName, Step, Offset)
                : string.Format(":{0}({1})", functionName, FunctionText);
        }

        public abstract override string ToString(bool friendlyFormat, int indentation = 0);
    }
}