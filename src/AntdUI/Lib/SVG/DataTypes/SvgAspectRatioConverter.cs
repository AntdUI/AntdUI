// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System;

namespace AntdUI.Svg.DataTypes
{
    internal class SvgPreserveAspectRatioConverter
    {
        public static SvgAspectRatio Parse(string value)
        {
            if (value == null) return new SvgAspectRatio();
            bool bDefer = false, bSlice = false;
            var sParts = value.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int nAlignIndex = 0;
            if (sParts[0].Equals("defer"))
            {
                bDefer = true;
                nAlignIndex++;
                if (sParts.Length < 2) throw new ArgumentOutOfRangeException("value is not a member of SvgPreserveAspectRatio");
            }
            var eAlign = (SvgPreserveAspectRatio)Enum.Parse(typeof(SvgPreserveAspectRatio), sParts[nAlignIndex]);
            nAlignIndex++;
            if (sParts.Length > nAlignIndex)
            {
                switch (sParts[nAlignIndex])
                {
                    case "meet":
                        break;
                    case "slice":
                        bSlice = true;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("value is not a member of SvgPreserveAspectRatio");
                }
            }
            nAlignIndex++;
            if (sParts.Length > nAlignIndex) throw new ArgumentOutOfRangeException("value is not a member of SvgPreserveAspectRatio");
            return new SvgAspectRatio(eAlign, bSlice, bDefer);
        }
    }
}