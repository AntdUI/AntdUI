// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

namespace AntdUI.Svg
{
    //TODO Need to split this enum into separate inherited enums for GradientCoordinateUnits, ClipPathCoordinateUnits, etc. as each should have its own converter since they have different defaults.
    /// <summary>
    /// Defines the various coordinate units certain SVG elements may use.
    /// </summary>
    public enum SvgCoordinateUnits
    {
        //TODO Inherit is not actually valid
        Inherit,

        /// <summary>
        /// Indicates that the coordinate system of the owner element is to be used.
        /// </summary>
        ObjectBoundingBox,

        /// <summary>
        /// Indicates that the coordinate system of the entire document is to be used.
        /// </summary>
        UserSpaceOnUse
    }
}