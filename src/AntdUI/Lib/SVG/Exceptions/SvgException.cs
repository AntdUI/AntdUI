// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System;

namespace AntdUI.Svg
{
    public class SvgException : FormatException
    {
        public SvgException(string message) : base(message)
        {
        }
    }

    public class SvgIDException : FormatException
    {
        public SvgIDException(string message)
            : base(message)
        {
        }
    }

    public class SvgIDExistsException : SvgIDException
    {
        public SvgIDExistsException(string message)
            : base(message)
        {
        }
    }

    public class SvgIDWrongFormatException : SvgIDException
    {
        public SvgIDWrongFormatException(string message)
            : base(message)
        {
        }
    }
}