﻿// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System.ComponentModel;

namespace AntdUI.Svg
{
    /// <summary>Indicates what happens if the gradient starts or ends inside the bounds of the target rectangle.</summary>
    /// <remarks>
    ///     <para>Possible values are: 'pad', which says to use the terminal colors of the gradient to fill the remainder of the target region, 'reflect', which says to reflect the gradient pattern start-to-end, end-to-start, start-to-end, etc. continuously until the target rectangle is filled, and repeat, which says to repeat the gradient pattern start-to-end, start-to-end, start-to-end, etc. continuously until the target region is filled.</para>
    ///     <para>If the attribute is not specified, the effect is as if a value of 'pad' were specified.</para>
    /// </remarks>
    [TypeConverter(typeof(SvgGradientSpreadMethodConverter))]
    public enum SvgGradientSpreadMethod
    {
        /// <summary>Use the terminal colors of the gradient to fill the remainder of the target region.</summary>
        Pad,

        /// <summary>Reflect the gradient pattern start-to-end, end-to-start, start-to-end, etc. continuously until the target rectangle is filled.</summary>
        Reflect,

        /// <summary>Repeat the gradient pattern start-to-end, start-to-end, start-to-end, etc. continuously until the target region is filled.</summary>
        Repeat
    }
}