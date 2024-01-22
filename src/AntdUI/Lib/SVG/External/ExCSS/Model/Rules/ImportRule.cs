// THIS FILE IS PART OF ExCSS PROJECT
// THE ExCSS PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHT (C) TylerBrinks. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/TylerBrinks/ExCSS

using AntdUI.Svg.ExCSS.Model;
using AntdUI.Svg.ExCSS.Model.Extensions;

// ReSharper disable once CheckNamespace
namespace AntdUI.Svg.ExCSS
{
    public class ImportRule : RuleSet, ISupportsMedia
    {
        private string _href;
        private readonly MediaTypeList _media;

        public ImportRule()
        {
            _media = new MediaTypeList();
            RuleType = RuleType.Import;
        }

        public string Href
        {
            get { return _href; }
            set { _href = value; }
        }

        public MediaTypeList Media
        {
            get { return _media; }
        }

        public override string ToString()
        {
            return ToString(false);
        }

        public override string ToString(bool friendlyFormat, int indentation = 0)
        {
            return _media.Count > 0
                ? string.Format("@import url({0}) {1};", _href, _media).NewLineIndent(friendlyFormat, indentation)
                : string.Format("@import url({0});", _href).NewLineIndent(friendlyFormat, indentation);
        }
    }
}
