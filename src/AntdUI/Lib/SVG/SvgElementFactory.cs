// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using AntdUI.Svg.DataTypes;
using AntdUI.Svg.Transforms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Xml;

namespace AntdUI.Svg
{
    /// <summary>
    /// Provides the methods required in order to parse and create <see cref="SvgElement"/> instances from XML.
    /// </summary>
    internal class SvgElementFactory
    {
        /// <summary>
        /// Creates an <see cref="SvgDocument"/> from the current node in the specified <see cref="XmlTextReader"/>.
        /// </summary>
        /// <param name="reader">The <see cref="XmlTextReader"/> containing the node to parse into an <see cref="SvgDocument"/>.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="reader"/> parameter cannot be <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">The CreateDocument method can only be used to parse root &lt;svg&gt; elements.</exception>
        public T CreateDocument<T>(XmlReader reader) where T : SvgDocument, new()
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            if (reader.LocalName != "svg")
            {
                throw new InvalidOperationException("The CreateDocument method can only be used to parse root <svg> elements.");
            }

            return (T)CreateElement<T>(reader, true, null);
        }

        /// <summary>
        /// Creates an <see cref="SvgElement"/> from the current node in the specified <see cref="XmlTextReader"/>.
        /// </summary>
        /// <param name="reader">The <see cref="XmlTextReader"/> containing the node to parse into a subclass of <see cref="SvgElement"/>.</param>
        /// <param name="document">The <see cref="SvgDocument"/> that the created element belongs to.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="reader"/> and <paramref name="document"/> parameters cannot be <c>null</c>.</exception>
        public SvgElement CreateElement(XmlReader reader, SvgDocument document)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }
            return CreateElement<SvgDocument>(reader, false, document);
        }

        private SvgElement CreateElement<T>(XmlReader reader, bool fragmentIsDocument, SvgDocument document) where T : SvgDocument, new()
        {
            SvgElement createdElement;
            string elementName = reader.LocalName;
            string elementNS = reader.NamespaceURI;

            //Trace.TraceInformation("Begin CreateElement: {0}", elementName);

            if (elementNS == SvgAttributeAttribute.SvgNamespace || string.IsNullOrEmpty(elementNS))
            {
                if (elementName == "svg") createdElement = (fragmentIsDocument) ? new T() : new SvgFragment();
                else
                {
                    switch (elementName)
                    {
                        case "circle":
                            createdElement = new SvgCircle();
                            break;
                        case "ellipse":
                            createdElement = new SvgEllipse();
                            break;
                        case "line":
                            createdElement = new SvgLine();
                            break;
                        case "polygon":
                            createdElement = new SvgPolygon();
                            break;
                        case "polyline":
                            createdElement = new SvgPolyline();
                            break;
                        case "rect":
                            createdElement = new SvgRectangle();
                            break;
                        case "clipPath":
                            createdElement = new SvgClipPath();
                            break;
                        case "defs":
                            createdElement = new SvgDefinitionList();
                            break;
                        case "desc":
                            createdElement = new SvgDescription();
                            break;
                        case "metadata":
                            createdElement = new SvgDocumentMetadata();
                            break;
                        case "g":
                            createdElement = new SvgGroup();
                            break;
                        case "switch":
                            createdElement = new SvgSwitch();
                            break;
                        case "title":
                            createdElement = new SvgTitle();
                            break;
                        case "use":
                            createdElement = new SvgUse();
                            break;
                        case "foreignObject":
                            createdElement = new SvgForeignObject();
                            break;
                        case "stop":
                            createdElement = new SvgGradientStop();
                            break;
                        case "linearGradient":
                            createdElement = new SvgLinearGradientServer();
                            break;
                        case "marker":
                            createdElement = new SvgMarker();
                            break;
                        case "pattern":
                            createdElement = new SvgPatternServer();
                            break;
                        case "radialGradient":
                            createdElement = new SvgRadialGradientServer();
                            break;
                        case "path":
                            createdElement = new SvgPath();
                            break;
                        case "font":
                            createdElement = new SvgFont();
                            break;
                        case "font-face":
                            createdElement = new SvgFontFace();
                            break;
                        case "font-face-src":
                            createdElement = new SvgFontFaceSrc();
                            break;
                        case "font-face-uri":
                            createdElement = new SvgFontFaceUri();
                            break;
                        case "glyph":
                            createdElement = new SvgGlyph();
                            break;
                        case "vkern":
                            createdElement = new SvgVerticalKern();
                            break;
                        case "hkern":
                            createdElement = new SvgHorizontalKern();
                            break;
                        case "missing-glyph":
                            createdElement = new SvgMissingGlyph();
                            break;
                        case "text":
                            createdElement = new SvgText();
                            break;
                        case "textPath":
                            createdElement = new SvgTextPath();
                            break;
                        case "tref":
                            createdElement = new SvgTextRef();
                            break;
                        case "tspan":
                            createdElement = new SvgTextSpan();
                            break;
                        case "feColorMatrix":
                            createdElement = new FilterEffects.SvgColourMatrix();
                            break;
                        case "feGaussianBlur":
                            createdElement = new FilterEffects.SvgGaussianBlur();
                            break;
                        case "feMerge":
                            createdElement = new FilterEffects.SvgMerge();
                            break;
                        case "feMergeNode":
                            createdElement = new FilterEffects.SvgMergeNode();
                            break;
                        case "feOffset":
                            createdElement = new FilterEffects.SvgOffset();
                            break;
                        case "filter":
                            createdElement = new FilterEffects.SvgFilter();
                            break;
                        case "symbol":
                            createdElement = new Document_Structure.SvgSymbol();
                            break;
                        default:
                            createdElement = new SvgUnknownElement(elementName);
                            break;
                    }
                }
                if (createdElement != null) SetAttributes(createdElement, reader, document);
            }
            else
            {
                // All non svg element (html, ...)
                createdElement = new NonSvgElement(elementName);
                SetAttributes(createdElement, reader, document);
            }
            return createdElement;
        }

        private void SetAttributes(SvgElement element, XmlReader reader, SvgDocument document)
        {
            //Trace.TraceInformation("Begin SetAttributes");

            //string[] styles = null;
            //string[] style = null;
            //int i = 0;

            while (reader.MoveToNextAttribute())
            {
                if (IsStyleAttribute(reader.LocalName)) element.AddStyle(reader.LocalName, reader.Value, SvgElement.StyleSpecificity_PresAttribute);
                else SetPropertyValue(element, reader.LocalName, reader.Value, document);
            }

            //Trace.TraceInformation("End SetAttributes");
        }

        private static bool IsStyleAttribute(string name)
        {
            switch (name)
            {
                case "alignment-baseline":
                case "baseline-shift":
                case "clip":
                case "clip-path":
                case "clip-rule":
                case "color":
                case "color-interpolation":
                case "color-interpolation-filters":
                case "color-profile":
                case "color-rendering":
                case "cursor":
                case "direction":
                case "display":
                case "dominant-baseline":
                case "enable-background":
                case "fill":
                case "fill-opacity":
                case "fill-rule":
                case "filter":
                case "flood-color":
                case "flood-opacity":
                case "font":
                case "font-family":
                case "font-size":
                case "font-size-adjust":
                case "font-stretch":
                case "font-style":
                case "font-variant":
                case "font-weight":
                case "glyph-orientation-horizontal":
                case "glyph-orientation-vertical":
                case "image-rendering":
                case "kerning":
                case "letter-spacing":
                case "lighting-color":
                case "marker":
                case "marker-end":
                case "marker-mid":
                case "marker-start":
                case "mask":
                case "opacity":
                case "overflow":
                case "pointer-events":
                case "shape-rendering":
                case "stop-color":
                case "stop-opacity":
                case "stroke":
                case "stroke-dasharray":
                case "stroke-dashoffset":
                case "stroke-linecap":
                case "stroke-linejoin":
                case "stroke-miterlimit":
                case "stroke-opacity":
                case "stroke-width":
                case "text-anchor":
                case "text-decoration":
                case "text-rendering":
                case "text-transform":
                case "unicode-bidi":
                case "visibility":
                case "word-spacing":
                case "writing-mode":
                    return true;
            }
            return false;
        }

        private static Dictionary<Type, Dictionary<string, PropertyDescriptorCollection>> _propertyDescriptors = new Dictionary<Type, Dictionary<string, PropertyDescriptorCollection>>();
        private static object syncLock = new object();

        internal static bool SetPropertyValue(SvgElement element, string attributeName, string attributeValue, SvgDocument document, bool isStyle = false)
        {
            try
            {
                if (element is SvgFragment doc)
                {
                    if (attributeName == "viewBox")
                    {
                        doc.ViewBox = SvgViewBoxConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "x")
                    {
                        doc.X = SvgUnitConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "y")
                    {
                        doc.Y = SvgUnitConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "width")
                    {
                        doc.Width = SvgUnitConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "height")
                    {
                        doc.Height = SvgUnitConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "overflow")
                    {
                        doc.Overflow = (SvgOverflow)Enum.Parse(typeof(SvgOverflow), attributeValue, true);
                        return true;
                    }
                    else if (attributeName == "preserveAspectRatio")
                    {
                        doc.AspectRatio = SvgPreserveAspectRatioConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "focusable" || attributeName == "data-icon" || attributeName == "aria-hidden" || attributeName == "xlink" || attributeName == "xmlns" || attributeName == "t" || attributeName == "class" || attributeName == "version" || attributeName == "p-id")
                    {
                        return SetPropertyValueNULL(element, attributeName, attributeValue);
                    }
                }
                if (element is SvgTextBase textBase)
                {
                    if (attributeName == "x")
                    {
                        textBase.X = SvgUnitCollectionConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "y")
                    {
                        textBase.Y = SvgUnitCollectionConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "dx")
                    {
                        textBase.Dx = SvgUnitCollectionConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "dx")
                    {
                        textBase.Dy = SvgUnitCollectionConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "rotate")
                    {
                        textBase.Rotate = attributeValue;
                        return true;
                    }
                    else if (attributeName == "textLength")
                    {
                        textBase.TextLength = SvgUnitConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "lengthAdjust")
                    {
                        textBase.LengthAdjust = (SvgTextLengthAdjust)Enum.Parse(typeof(SvgTextLengthAdjust), attributeValue, true);
                        return true;
                    }
                    else if (attributeName == "letter-spacing")
                    {
                        textBase.LetterSpacing = SvgUnitConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "word-spacing")
                    {
                        textBase.WordSpacing = SvgUnitConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "display")
                    {
                        textBase.Display = attributeValue;
                        return true;
                    }
                    else if (attributeName == "visibility")
                    {
                        textBase.Visible = bool.Parse(attributeValue);
                        return true;
                    }

                }
                if (element is SvgVisualElement svgVisual)
                {
                    if (attributeName == "display")
                    {
                        svgVisual.Display = attributeValue;
                        return true;
                    }
                    else if (attributeName == "visibility")
                    {
                        svgVisual.Visible = bool.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "enable-background")
                    {
                        svgVisual.EnableBackground = attributeValue;
                        return true;
                    }
                }

                #region »ù´¡

                if (attributeName == "id")
                {
                    element.ID = attributeValue;
                    return true;
                }
                else if (attributeName == "fill")
                {
                    element.Fill = SvgPaintServerConverter.Parse(attributeValue, document);
                    return true;
                }
                else if (attributeName == "fill-rule")
                {
                    element.FillRule = (SvgFillRule)Enum.Parse(typeof(SvgFillRule), attributeValue, true);
                    return true;
                }
                else if (attributeName == "stroke")
                {
                    element.Stroke = SvgPaintServerConverter.Parse(attributeValue, document);
                    return true;
                }
                else if (attributeName == "opacity")
                {
                    if (attributeValue == "undefined") element.Opacity = 1;
                    else element.Opacity = float.Parse(attributeValue);
                    return true;
                }
                else if (attributeName == "fill-opacity")
                {
                    if (attributeValue == "undefined") element.FillOpacity = 1;
                    else element.FillOpacity = float.Parse(attributeValue);
                    return true;
                }
                else if (attributeName == "stroke-width")
                {
                    element.StrokeWidth = SvgUnitConverter.Parse(attributeValue);
                    return true;
                }
                else if (attributeName == "stroke-linecap")
                {
                    element.StrokeLineCap = (SvgStrokeLineCap)Enum.Parse(typeof(SvgStrokeLineCap), attributeValue, true);
                    return true;
                }
                else if (attributeName == "stroke-linejoin")
                {
                    try
                    {
                        element.StrokeLineJoin = (SvgStrokeLineJoin)Enum.Parse(typeof(SvgStrokeLineJoin), attributeValue, true);
                    }
                    catch { }
                    return true;
                }
                else if (attributeName == "stroke-miterlimit")
                {
                    element.StrokeMiterLimit = float.Parse(attributeValue);
                    return true;
                }
                else if (attributeName == "stroke-dasharray")
                {
                    element.StrokeDashArray = SvgUnitCollectionConverter.Parse(attributeValue);
                    return true;
                }
                else if (attributeName == "stroke-dashoffset")
                {
                    element.StrokeDashOffset = SvgUnitConverter.Parse(attributeValue);
                    return true;
                }
                else if (attributeName == "stroke-opacity")
                {
                    if (attributeValue == "undefined") element.StrokeOpacity = 1;
                    else element.StrokeOpacity = float.Parse(attributeValue);
                    return true;
                }
                else if (attributeName == "stop-color")
                {
                    element.StopColor = SvgPaintServerConverter.Parse(attributeValue, document);
                    return true;
                }
                else if (attributeName == "shape-rendering")
                {
                    element.ShapeRendering = (SvgShapeRendering)Enum.Parse(typeof(SvgShapeRendering), attributeValue, true);
                    return true;
                }
                else if (attributeName == "text-anchor")
                {
                    element.TextAnchor = (SvgTextAnchor)Enum.Parse(typeof(SvgTextAnchor), attributeValue, true);
                    return true;
                }
                else if (attributeName == "transform")
                {
                    element.Transforms = SvgTransformConverter.Parse(attributeValue);
                    return true;
                }
                else if (attributeName == "font-family")
                {
                    element.FontFamily = attributeValue;
                    return true;
                }
                else if (attributeName == "font-size")
                {
                    element.FontSize = SvgUnitConverter.Parse(attributeValue);
                    return true;
                }
                else if (attributeName == "font-style")
                {
                    element.FontStyle = (SvgFontStyle)Enum.Parse(typeof(SvgFontStyle), attributeValue, true);
                    return true;
                }
                else if (attributeName == "font-variant")
                {
                    element.FontVariant = (SvgFontVariant)Enum.Parse(typeof(SvgFontVariant), attributeValue, true);
                    return true;
                }
                else if (attributeName == "text-decoration")
                {
                    element.TextDecoration = (SvgTextDecoration)Enum.Parse(typeof(SvgTextDecoration), attributeValue, true);
                    return true;
                }
                else if (attributeName == "font-weight")
                {
                    element.FontWeight = (SvgFontWeight)Enum.Parse(typeof(SvgFontWeight), attributeValue, true);
                    return true;
                }
                else if (attributeName == "text-transform")
                {
                    element.TextTransformation = (SvgTextTransformation)Enum.Parse(typeof(SvgTextTransformation), attributeValue, true);
                    return true;
                }

                #endregion

                else if (element is SvgCircle circle)
                {
                    if (attributeName == "cx")
                    {
                        circle.CenterX = SvgUnitConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "cy")
                    {
                        circle.CenterY = SvgUnitConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "r")
                    {
                        circle.Radius = SvgUnitConverter.Parse(attributeValue);
                        return true;
                    }
                }
                else if (element is SvgEllipse ellipse)
                {
                    if (attributeName == "cx")
                    {
                        ellipse.CenterX = SvgUnitConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "cy")
                    {
                        ellipse.CenterY = SvgUnitConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "rx")
                    {
                        ellipse.RadiusX = SvgUnitConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "ry")
                    {
                        ellipse.RadiusY = SvgUnitConverter.Parse(attributeValue);
                        return true;
                    }
                }
                else if (element is SvgLine line)
                {
                    if (attributeName == "x1")
                    {
                        line.StartX = SvgUnitConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "y1")
                    {
                        line.StartY = SvgUnitConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "x2")
                    {
                        line.EndX = SvgUnitConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "y2")
                    {
                        line.EndY = SvgUnitConverter.Parse(attributeValue);
                        return true;
                    }
                }
                else if (element is SvgPolygon polygon)
                {
                    if (attributeName == "points")
                    {
                        polygon.Points = SvgPointCollectionConverter.Parse(attributeValue);
                        return true;
                    }
                }
                else if (element is SvgPolyline polyline)
                {
                    if (attributeName == "points")
                    {
                        polyline.Points = SvgPointCollectionConverter.Parse(attributeValue);
                        return true;
                    }
                }
                else if (element is SvgRectangle rect)
                {
                    if (attributeName == "x")
                    {
                        rect.X = SvgUnitConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "y")
                    {
                        rect.Y = SvgUnitConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "width")
                    {
                        rect.Width = SvgUnitConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "height")
                    {
                        rect.Height = SvgUnitConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "rx")
                    {
                        rect.CornerRadiusX = SvgUnitConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "ry")
                    {
                        rect.CornerRadiusY = SvgUnitConverter.Parse(attributeValue);
                        return true;
                    }
                }
                else if (element is SvgClipPath clipPath)
                {
                    if (attributeName == "clipPathUnits")
                    {
                        clipPath.ClipPathUnits = (SvgCoordinateUnits)Enum.Parse(typeof(SvgCoordinateUnits), attributeValue, true);
                        return true;
                    }
                }
                //else if (element is SvgDefinitionList defs)
                //{
                //}
                //else if (element is SvgDescription desc)
                //{
                //}
                //else if (element is SvgDocumentMetadata metadata)
                //{
                //}
                //else if (element is SvgGroup group)
                //{
                //}
                //else if (element is SvgSwitch _switch)
                //{
                //}
                //else if (element is SvgTitle title)
                //{
                //}
                else if (element is SvgUse use)
                {
                    if (attributeName == "x")
                    {
                        use.X = SvgUnitConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "y")
                    {
                        use.Y = SvgUnitConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "width")
                    {
                        use.Width = SvgUnitConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "height")
                    {
                        use.Height = SvgUnitConverter.Parse(attributeValue);
                        return true;
                    }
                }
                //else if (element is SvgForeignObject foreignObject)
                //{
                //}
                else if (element is SvgGradientStop stop)
                {
                    if (attributeName == "offset")
                    {
                        stop.Offset = SvgUnitConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "stop-color")
                    {
                        stop.StopColor = SvgPaintServerConverter.Parse(attributeValue, document);
                        return true;
                    }
                    else if (attributeName == "stop-opacity")
                    {
                        stop.Opacity = float.Parse(attributeValue);
                        return true;
                    }
                }
                else if (element is SvgMarker marker)
                {
                    if (attributeName == "refX")
                    {
                        marker.RefX = SvgUnitConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "refY")
                    {
                        marker.RefY = SvgUnitConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "orient")
                    {
                        marker.Orient = SvgOrientConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "overflow")
                    {
                        marker.Overflow = (SvgOverflow)Enum.Parse(typeof(SvgOverflow), attributeValue, true);
                        return true;
                    }
                    else if (attributeName == "viewBox")
                    {
                        marker.ViewBox = SvgViewBoxConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "preserveAspectRatio")
                    {
                        marker.AspectRatio = SvgPreserveAspectRatioConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "markerWidth")
                    {
                        marker.MarkerWidth = SvgUnitConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "markerHeight")
                    {
                        marker.MarkerHeight = SvgUnitConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "markerUnits")
                    {
                        marker.MarkerUnits = (SvgMarkerUnits)Enum.Parse(typeof(SvgMarkerUnits), attributeValue, true);
                        return true;
                    }
                }
                else if (element is SvgPatternServer pattern)
                {
                    if (attributeName == "overflow")
                    {
                        pattern.Overflow = (SvgOverflow)Enum.Parse(typeof(SvgOverflow), attributeValue, true);
                        return true;
                    }
                    else if (attributeName == "viewBox")
                    {
                        pattern.ViewBox = SvgViewBoxConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "preserveAspectRatio")
                    {
                        pattern.AspectRatio = SvgPreserveAspectRatioConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "x")
                    {
                        pattern.X = SvgUnitConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "y")
                    {
                        pattern.Y = SvgUnitConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "width")
                    {
                        pattern.Width = SvgUnitConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "height")
                    {
                        pattern.Height = SvgUnitConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "patternUnits")
                    {
                        pattern.PatternUnits = (SvgCoordinateUnits)Enum.Parse(typeof(SvgCoordinateUnits), attributeValue, true);
                        return true;
                    }
                    else if (attributeName == "patternContentUnits")
                    {
                        pattern.PatternContentUnits = (SvgCoordinateUnits)Enum.Parse(typeof(SvgCoordinateUnits), attributeValue, true);
                        return true;
                    }
                    else if (attributeName == "href")
                    {
                        pattern.InheritGradient = SvgPaintServerConverter.Parse(attributeValue, document);
                        return true;
                    }
                    else if (attributeName == "patternTransform")
                    {
                        pattern.PatternTransform = SvgTransformConverter.Parse(attributeValue);
                        return true;
                    }
                }
                else if (element is SvgLinearGradientServer linearGradient)
                {
                    if (attributeName == "id")
                    {
                        linearGradient.ID = attributeValue;
                        return true;
                    }
                    if (attributeName == "x1")
                    {
                        linearGradient.X1 = SvgUnitConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "y1")
                    {
                        linearGradient.Y1 = SvgUnitConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "x2")
                    {
                        linearGradient.X2 = SvgUnitConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "y2")
                    {
                        linearGradient.Y2 = SvgUnitConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "gradientTransform")
                    {
                        linearGradient.GradientTransform = SvgTransformConverter.Parse(attributeValue);
                        return true;
                    }
                }
                else if (element is SvgRadialGradientServer radialGradient)
                {
                    if (attributeName == "cx")
                    {
                        radialGradient.CenterX = SvgUnitConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "cy")
                    {
                        radialGradient.CenterY = SvgUnitConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "r")
                    {
                        radialGradient.Radius = SvgUnitConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "fx")
                    {
                        radialGradient.FocalX = SvgUnitConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "fy")
                    {
                        radialGradient.FocalY = SvgUnitConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "gradientTransform")
                    {
                        radialGradient.GradientTransform = SvgTransformConverter.Parse(attributeValue);
                        return true;
                    }
                }
                else if (element is SvgPath path)
                {
                    if (attributeName == "d")
                    {
                        path.PathData = SvgPathConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "pathLength")
                    {
                        path.PathLength = float.Parse(attributeValue);
                        return true;
                    }
                }
                else if (element is SvgFont font)
                {
                    if (attributeName == "horiz-adv-x")
                    {
                        font.HorizAdvX = float.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "horiz-origin-x")
                    {
                        font.HorizOriginX = float.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "horiz-origin-y")
                    {
                        font.HorizOriginY = float.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "vert-adv-y")
                    {
                        font.VertAdvY = float.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "vert-origin-x")
                    {
                        font.VertOriginX = float.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "vert-origin-y")
                    {
                        font.VertOriginY = float.Parse(attributeValue);
                        return true;
                    }
                }
                else if (element is SvgFontFace fontface)
                {
                    if (attributeName == "alphabetic")
                    {
                        fontface.Alphabetic = float.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "ascent")
                    {
                        fontface.Ascent = float.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "ascent-height")
                    {
                        fontface.AscentHeight = float.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "descent")
                    {
                        fontface.Descent = float.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "panose-1")
                    {
                        fontface.Panose1 = attributeValue;
                        return true;
                    }
                    else if (attributeName == "units-per-em")
                    {
                        fontface.UnitsPerEm = float.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "x-height")
                    {
                        fontface.XHeight = float.Parse(attributeValue);
                        return true;
                    }
                }
                //else if (element is SvgFontFaceSrc fontfacesrc)
                //{
                //}
                //else if (element is SvgFontFaceUri fontfaceuri)
                //{
                //}
                else if (element is SvgGlyph glyph)
                {
                    if (attributeName == "d")
                    {
                        glyph.PathData = SvgPathConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "glyph-name")
                    {
                        glyph.GlyphName = attributeValue;
                        return true;
                    }
                    else if (attributeName == "unicode")
                    {
                        glyph.Unicode = attributeValue;
                        return true;
                    }
                    else if (attributeName == "horiz-adv-x")
                    {
                        glyph.HorizAdvX = float.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "vert-adv-y")
                    {
                        glyph.VertAdvY = float.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "vert-origin-x")
                    {
                        glyph.VertOriginX = float.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "vert-origin-y")
                    {
                        glyph.VertOriginY = float.Parse(attributeValue);
                        return true;
                    }
                }
                //else if (element is SvgMissingGlyph missingglyph)
                //{
                //}
                //else if (element is SvgVerticalKern vkern)
                //{
                //}
                //else if (element is SvgHorizontalKern hkern)
                //{
                //}
                //else if (element is SvgText text)
                //{
                //}
                else if (element is SvgTextPath textPath)
                {
                    if (attributeName == "startOffset")
                    {
                        textPath.StartOffset = SvgUnitConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "method")
                    {
                        textPath.Method = (SvgTextPathMethod)Enum.Parse(typeof(SvgTextPathMethod), attributeValue, true);
                        return true;
                    }
                    else if (attributeName == "spacing")
                    {
                        textPath.Spacing = (SvgTextPathSpacing)Enum.Parse(typeof(SvgTextPathSpacing), attributeValue, true);
                        return true;
                    }
                }
                //else if (element is SvgTextRef tref)
                //{
                //}
                //else if (element is SvgTextSpan tspan)
                //{
                //}
                else if (element is FilterEffects.SvgColourMatrix feColorMatrix)
                {
                    if (attributeName == "values")
                    {
                        feColorMatrix.Values = attributeValue;
                        return true;
                    }
                    if (attributeName == "type")
                    {
                        feColorMatrix.Type = (FilterEffects.SvgColourMatrixType)Enum.Parse(typeof(FilterEffects.SvgColourMatrixType), attributeValue, true);
                        return true;
                    }
                    else if (attributeName == "in")
                    {
                        feColorMatrix.Input = attributeValue;
                        return true;
                    }
                    else if (attributeName == "result")
                    {
                        feColorMatrix.Result = attributeValue;
                        return true;
                    }
                }
                else if (element is FilterEffects.SvgGaussianBlur feGaussianBlur)
                {
                    if (attributeName == "stdDeviation")
                    {
                        feGaussianBlur.StdDeviation = float.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "in")
                    {
                        feGaussianBlur.Input = attributeValue;
                        return true;
                    }
                    else if (attributeName == "result")
                    {
                        feGaussianBlur.Result = attributeValue;
                        return true;
                    }
                }
                else if (element is FilterEffects.SvgMerge feMerge)
                {
                    if (attributeName == "in")
                    {
                        feMerge.Input = attributeValue;
                        return true;
                    }
                    else if (attributeName == "result")
                    {
                        feMerge.Result = attributeValue;
                        return true;
                    }
                }
                else if (element is FilterEffects.SvgMergeNode feMergeNode)
                {
                    if (attributeName == "in")
                    {
                        feMergeNode.Input = attributeValue;
                        return true;
                    }
                }
                else if (element is FilterEffects.SvgOffset feOffset)
                {
                    if (attributeName == "dx")
                    {
                        feOffset.Dx = SvgUnitConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "dy")
                    {
                        feOffset.Dy = SvgUnitConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "in")
                    {
                        feOffset.Input = attributeValue;
                        return true;
                    }
                    else if (attributeName == "result")
                    {
                        feOffset.Result = attributeValue;
                        return true;
                    }
                }
                else if (element is FilterEffects.SvgFilter filter)
                {
                    if (attributeName == "x")
                    {
                        filter.X = SvgUnitConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "y")
                    {
                        filter.Y = SvgUnitConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "width")
                    {
                        filter.Width = SvgUnitConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "height")
                    {
                        filter.Height = SvgUnitConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "color-interpolation-filters")
                    {
                        filter.ColorInterpolationFilters = (SvgColourInterpolation)Enum.Parse(typeof(SvgColourInterpolation), attributeValue, true);
                        return true;
                    }
                }
                else if (element is Document_Structure.SvgSymbol symbol)
                {
                    if (attributeName == "viewBox")
                    {
                        symbol.ViewBox = SvgViewBoxConverter.Parse(attributeValue);
                        return true;
                    }
                    else if (attributeName == "preserveAspectRatio")
                    {
                        symbol.AspectRatio = SvgPreserveAspectRatioConverter.Parse(attributeValue);
                        return true;
                    }
                }
            }
            catch { }

            var elementType = element.GetType();

            PropertyDescriptorCollection properties;
            lock (syncLock)
            {
                if (_propertyDescriptors.Keys.Contains(elementType))
                {
                    if (_propertyDescriptors[elementType].Keys.Contains(attributeName))
                    {
                        properties = _propertyDescriptors[elementType][attributeName];
                    }
                    else
                    {
                        properties = TypeDescriptor.GetProperties(elementType, new[] { new SvgAttributeAttribute(attributeName) });
                        _propertyDescriptors[elementType].Add(attributeName, properties);
                    }
                }
                else
                {
                    properties = TypeDescriptor.GetProperties(elementType, new[] { new SvgAttributeAttribute(attributeName) });
                    _propertyDescriptors.Add(elementType, new Dictionary<string, PropertyDescriptorCollection>());

                    _propertyDescriptors[elementType].Add(attributeName, properties);
                }
            }

            if (properties.Count > 0)
            {
                PropertyDescriptor descriptor = properties[0];

                try
                {
                    var value = descriptor.Converter.ConvertFrom(document, CultureInfo.InvariantCulture, attributeValue);
                    descriptor.SetValue(element, value);
                }
                catch
                { }
            }
            else return SetPropertyValueNULL(element, attributeName, attributeValue);
            return true;
        }

        internal static bool SetPropertyValueNULL(SvgElement element, string attributeName, string attributeValue, bool isStyle = false)
        {
            //check for namespace declaration in svg element
            if (string.Equals(element.ElementName, "svg", StringComparison.OrdinalIgnoreCase))
            {
                if (string.Equals(attributeName, "xmlns", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(attributeName, "xlink", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(attributeName, "xmlns:xlink", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(attributeName, "version", StringComparison.OrdinalIgnoreCase))
                {
                    //nothing to do
                }
                else
                {
                    //attribute is not a svg attribute, store it in custom attributes
                    element.CustomAttributes[attributeName] = attributeValue;
                }
            }
            else
            {
                if (isStyle)
                {
                    // custom styles shall remain as style
                    return false;
                }
                //attribute is not a svg attribute, store it in custom attributes
                element.CustomAttributes[attributeName] = attributeValue;
            }
            return true;
        }
    }
}