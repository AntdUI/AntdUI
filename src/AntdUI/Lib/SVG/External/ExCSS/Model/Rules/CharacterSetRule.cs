// THIS FILE IS PART OF ExCSS PROJECT
// THE ExCSS PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHT (C) TylerBrinks. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/TylerBrinks/ExCSS

using AntdUI.Svg.ExCSS.Model.Extensions;

// ReSharper disable once CheckNamespace
namespace AntdUI.Svg.ExCSS
{
    public class CharacterSetRule : RuleSet
    {
        public CharacterSetRule()
        {
            RuleType = RuleType.Charset;
        }

        public string Encoding { get; internal set; }

        public override string ToString()
        {
            return ToString(false);
        }

        public override string ToString(bool friendlyFormat, int indentation = 0)
        {
            return string.Format("@charset '{0}';", Encoding).NewLineIndent(friendlyFormat, indentation);
        }
    }
}
