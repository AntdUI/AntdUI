// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System.IO;
using System.Xml;

namespace AntdUI.Svg
{
    internal sealed class SvgTextReader : XmlTextReader
    {
        public SvgTextReader(Stream stream)
            : base(stream)
        {
            EntityHandling = EntityHandling.ExpandEntities;
        }

        public SvgTextReader(TextReader reader)
            : base(reader)
        {
            EntityHandling = EntityHandling.ExpandEntities;
        }
    }
}