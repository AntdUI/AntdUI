// THIS FILE IS PART OF ExCSS PROJECT
// THE ExCSS PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHT (C) TylerBrinks. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/TylerBrinks/ExCSS

using AntdUI.Svg.ExCSS.Model.Extensions;

// ReSharper disable once CheckNamespace
namespace AntdUI.Svg.ExCSS
{
    public class Property
    {
        private Term _term;
        private bool _important;

        public Property(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public Term Term
        {
            get { return _term; }
            set { _term = value; }
        }

        public bool Important
        {
            get { return _important; }
            set { _important = value; }
        }

        public override string ToString()
        {
            return ToString(false);
        }

        public string ToString(bool friendlyFormat, int indentation = 0)
        {
            var value = Name + ":" + _term;

            if (_important)
            {
                value += " !important";
            }

            return value.Indent(friendlyFormat, indentation);
        }
    }
}
