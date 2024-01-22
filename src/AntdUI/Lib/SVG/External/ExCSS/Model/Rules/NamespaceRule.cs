// THIS FILE IS PART OF ExCSS PROJECT
// THE ExCSS PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHT (C) TylerBrinks. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/TylerBrinks/ExCSS

using AntdUI.Svg.ExCSS.Model.Extensions;
// ReSharper disable once CheckNamespace


namespace AntdUI.Svg.ExCSS
{
    public class NamespaceRule : RuleSet
    {
        public NamespaceRule()
        {
            RuleType = RuleType.Namespace;
        }

        public string Uri { get; set; }

        public string Prefix { get; set; }

        public override string ToString()
        {
            return ToString(false);
        }

        public override string ToString(bool friendlyFormat, int indentation = 0)
        {
            return string.IsNullOrEmpty(Prefix)
                 ? string.Format("@namespace '{0}';", Uri).NewLineIndent(friendlyFormat, indentation)
                 : string.Format("@namespace {0} '{1}';", Prefix, Uri).NewLineIndent(friendlyFormat, indentation);
        }
    }
}
