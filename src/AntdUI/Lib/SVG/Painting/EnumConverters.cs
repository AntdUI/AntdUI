﻿// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using AntdUI.Svg.DataTypes;
using System;
using System.ComponentModel;
using System.Globalization;

namespace AntdUI.Svg
{
    //just overrides canconvert and returns true
    public class BaseConverter : TypeConverter
    {

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }
    }

    public sealed class SvgBoolConverter : BaseConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value == null)
            {
                return true;
            }

            if (!(value is string))
            {
                throw new ArgumentOutOfRangeException("value must be a string.");
            }

            // Note: currently only used by SvgVisualElement.Visible but if
            // conversion is used elsewhere these checks below will need to change
            string visibility = (string)value;
            if ((visibility == "hidden") || (visibility == "collapse"))
                return false;
            else
                return true;
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return ((bool)value) ? "visible" : "hidden";
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    //converts enums to lower case strings
    public class EnumBaseConverter<T> : BaseConverter
        where T : struct
    {
        public enum CaseHandling
        {
            CamelCase,
            LowerCase
        }
        /// <summary> Defines if the enum literal shall be converted to lower camel case or lower case.</summary>
        public CaseHandling CaseHandlingMode { get; }

        /// <summary>If specified, upon conversion, the default value will result in 'null'.</summary>
        public T? DefaultValue { get; protected set; }

        /// <summary>Creates a new instance.</summary>
        public EnumBaseConverter() { }

        /// <summary>Creates a new instance.</summary>
        /// <param name="defaultValue">Specified the default value of the enum.</param>
        /// <param name="caseHandling">Specified the case.</param>
        public EnumBaseConverter(T defaultValue, CaseHandling caseHandling = CaseHandling.CamelCase)
        {
            this.DefaultValue = defaultValue;
            this.CaseHandlingMode = caseHandling;
        }

        /// <summary>Attempts to convert the provided value to <typeparamref name="T"/>.</summary>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value == null)
            {
                if (this.DefaultValue.HasValue)
                    return this.DefaultValue.Value;

                return Activator.CreateInstance(typeof(T));
            }

            if (!(value is string))
            {
                throw new ArgumentOutOfRangeException("value must be a string.");
            }

            return (T)Enum.Parse(typeof(T), (string)value, true);
        }

        /// <summary>Attempts to convert the value to the destination type.</summary>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is T)
            {
                //If the value id the default value, no need to write the attribute.
                if (this.DefaultValue.HasValue && Enum.Equals(value, this.DefaultValue.Value))
                    return null;
                else
                {
                    string stringValue = ((T)value).ToString();
                    if (CaseHandlingMode == CaseHandling.LowerCase)
                    {
                        return stringValue.ToLower();
                    }

                    //most SVG attributes should be camelCase.
                    stringValue = string.Format("{0}{1}", stringValue[0].ToString().ToLower(), stringValue.Substring(1));

                    return stringValue;
                }
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    public sealed class SvgFillRuleConverter : EnumBaseConverter<SvgFillRule>
    {
        public SvgFillRuleConverter() : base(SvgFillRule.NonZero, CaseHandling.LowerCase) { }
    }

    public sealed class SvgColourInterpolationConverter : EnumBaseConverter<SvgColourInterpolation>
    {
        public SvgColourInterpolationConverter() : base(SvgColourInterpolation.SRGB) { }
    }

    public sealed class SvgClipRuleConverter : EnumBaseConverter<SvgClipRule>
    {
        public SvgClipRuleConverter() : base(SvgClipRule.NonZero, CaseHandling.LowerCase) { }
    }

    public sealed class SvgTextAnchorConverter : EnumBaseConverter<SvgTextAnchor>
    {
        public SvgTextAnchorConverter() : base(SvgTextAnchor.Start) { }
    }

    public sealed class SvgStrokeLineCapConverter : EnumBaseConverter<SvgStrokeLineCap>
    {
        public SvgStrokeLineCapConverter() : base(SvgStrokeLineCap.Butt) { }
    }

    public sealed class SvgStrokeLineJoinConverter : EnumBaseConverter<SvgStrokeLineJoin>
    {
        public SvgStrokeLineJoinConverter() : base(SvgStrokeLineJoin.Miter) { }
    }

    public sealed class SvgMarkerUnitsConverter : EnumBaseConverter<SvgMarkerUnits>
    {
        public SvgMarkerUnitsConverter() : base(SvgMarkerUnits.StrokeWidth) { }
    }

    public sealed class SvgFontStyleConverter : EnumBaseConverter<SvgFontStyle>
    {
        public SvgFontStyleConverter() : base(SvgFontStyle.All) { }
    }

    public sealed class SvgOverflowConverter : EnumBaseConverter<SvgOverflow>
    {
        public SvgOverflowConverter() : base(SvgOverflow.Auto) { }
    }

    public sealed class SvgTextLengthAdjustConverter : EnumBaseConverter<SvgTextLengthAdjust>
    {
        public SvgTextLengthAdjustConverter() : base(SvgTextLengthAdjust.Spacing) { }
    }

    public sealed class SvgTextPathMethodConverter : EnumBaseConverter<SvgTextPathMethod>
    {
        public SvgTextPathMethodConverter() : base(SvgTextPathMethod.Align) { }
    }

    public sealed class SvgTextPathSpacingConverter : EnumBaseConverter<SvgTextPathSpacing>
    {
        public SvgTextPathSpacingConverter() : base(SvgTextPathSpacing.Exact) { }
    }

    public sealed class SvgShapeRenderingConverter : EnumBaseConverter<SvgShapeRendering>
    {
        public SvgShapeRenderingConverter() : base(SvgShapeRendering.Inherit) { }
    }

    public sealed class SvgTextRenderingConverter : EnumBaseConverter<SvgTextRendering>
    {
        public SvgTextRenderingConverter() : base(SvgTextRendering.Inherit) { }
    }

    public sealed class SvgImageRenderingConverter : EnumBaseConverter<SvgImageRendering>
    {
        public SvgImageRenderingConverter() : base(SvgImageRendering.Inherit) { }
    }

    public sealed class SvgFontVariantConverter : EnumBaseConverter<SvgFontVariant>
    {
        public SvgFontVariantConverter() : base(SvgFontVariant.Normal) { }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value.ToString() == "small-caps")
                return SvgFontVariant.Smallcaps;

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is SvgFontVariant && (SvgFontVariant)value == SvgFontVariant.Smallcaps)
            {
                return "small-caps";
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    public sealed class SvgCoordinateUnitsConverter : EnumBaseConverter<SvgCoordinateUnits>
    {
        //TODO Inherit is not actually valid. See TODO on SvgCoordinateUnits enum.
        public SvgCoordinateUnitsConverter() : base(SvgCoordinateUnits.Inherit) { }
    }

    public sealed class SvgGradientSpreadMethodConverter : EnumBaseConverter<SvgGradientSpreadMethod>
    {
        public SvgGradientSpreadMethodConverter() : base(SvgGradientSpreadMethod.Pad) { }
    }

    public sealed class SvgTextDecorationConverter : EnumBaseConverter<SvgTextDecoration>
    {
        public SvgTextDecorationConverter() : base(SvgTextDecoration.None) { }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value.ToString() == "line-through")
                return SvgTextDecoration.LineThrough;

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is SvgTextDecoration && (SvgTextDecoration)value == SvgTextDecoration.LineThrough)
            {
                return "line-through";
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    public sealed class SvgFontWeightConverter : EnumBaseConverter<SvgFontWeight>
    {
        //TODO Defaulting to Normal although it should be All if this is used on a font face.
        public SvgFontWeightConverter() : base(SvgFontWeight.Normal) { }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                switch ((string)value)
                {
                    case "100": return SvgFontWeight.W100;
                    case "200": return SvgFontWeight.W200;
                    case "300": return SvgFontWeight.W300;
                    case "400": return SvgFontWeight.W400;
                    case "500": return SvgFontWeight.W500;
                    case "600": return SvgFontWeight.W600;
                    case "700": return SvgFontWeight.W700;
                    case "800": return SvgFontWeight.W800;
                    case "900": return SvgFontWeight.W900;
                }
            }
            return base.ConvertFrom(context, culture, value);
        }
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is SvgFontWeight)
            {
                switch ((SvgFontWeight)value)
                {
                    case SvgFontWeight.W100: return "100";
                    case SvgFontWeight.W200: return "200";
                    case SvgFontWeight.W300: return "300";
                    case SvgFontWeight.W400: return "400";
                    case SvgFontWeight.W500: return "500";
                    case SvgFontWeight.W600: return "600";
                    case SvgFontWeight.W700: return "700";
                    case SvgFontWeight.W800: return "800";
                    case SvgFontWeight.W900: return "900";
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    public sealed class SvgTextTransformationConverter : EnumBaseConverter<SvgTextTransformation>
    {
        public SvgTextTransformationConverter() : base(SvgTextTransformation.None) { }
    }

    public static class Enums
    {
        [CLSCompliant(false)]
        public static bool TryParse<TEnum>(string value, out TEnum result) where TEnum : struct, IConvertible
        {
            try
            {
                result = (TEnum)Enum.Parse(typeof(TEnum), value, true);
                return true;
            }
            catch
            {
                result = default(TEnum);
                return false;
            }
        }
    }
}
