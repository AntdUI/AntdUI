// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System;
using System.Runtime.Serialization;

namespace AntdUI.Svg.Exceptions
{
    [Serializable]
    public class SvgMemoryException : Exception
    {
        public SvgMemoryException() { }
        public SvgMemoryException(string message) : base(message) { }
        public SvgMemoryException(string message, Exception inner) : base(message, inner) { }

        protected SvgMemoryException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) { }
    }
}