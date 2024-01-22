// THIS FILE IS PART OF ExCSS PROJECT
// THE ExCSS PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHT (C) TylerBrinks. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/TylerBrinks/ExCSS

using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace AntdUI.Svg.ExCSS
{
    public interface IRuleContainer
    {
        List<RuleSet> Declarations { get; }
    }
}