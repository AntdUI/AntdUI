// THIS FILE IS PART OF ExCSS PROJECT
// THE ExCSS PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHT (C) TylerBrinks. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/TylerBrinks/ExCSS

using AntdUI.Svg.ExCSS.Model.Extensions;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace AntdUI.Svg.ExCSS
{
    public class KeyframesRule : RuleSet, IRuleContainer
    {
        private readonly List<RuleSet> _ruleSets;
        private string _identifier;

        public KeyframesRule()
        {
            _ruleSets = new List<RuleSet>();
            RuleType = RuleType.Keyframes;
        }

        public string Identifier
        {
            get { return _identifier; }
            set { _identifier = value; }
        }

        //TODO change to "keyframes"
        public List<RuleSet> Declarations
        {
            get { return _ruleSets; }
        }

        public override string ToString()
        {
            return ToString(false);
        }

        public override string ToString(bool friendlyFormat, int indentation = 0)
        {
            var join = friendlyFormat ? "".NewLineIndent(true, indentation) : "";

            var declarationList = _ruleSets.Select(d => d.ToString(friendlyFormat, indentation + 1));
            var declarations = string.Join(join, declarationList.ToArray());

            return ("@keyframes " + _identifier + "{").NewLineIndent(friendlyFormat, indentation) +
                declarations.NewLineIndent(friendlyFormat, indentation) +
                "}".NewLineIndent(friendlyFormat, indentation);
        }
    }
}
