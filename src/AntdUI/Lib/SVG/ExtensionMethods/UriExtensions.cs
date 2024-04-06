// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System;

namespace AntdUI.Svg.ExtensionMethods
{
    public static class UriExtensions
    {
        public static Uri? ReplaceWithNullIfNone(this Uri uri)
        {
            if (uri == null) { return null; }
            return string.Equals(uri.ToString(), "none", StringComparison.OrdinalIgnoreCase) ? null : uri;
        }
    }
}