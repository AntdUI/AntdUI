// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
                    if (attributeName == "opacity" && attributeValue == "undefined")
                    {
                        attributeValue = "1";
                    }
                    descriptor.SetValue(element, descriptor.Converter.ConvertFrom(document, CultureInfo.InvariantCulture, attributeValue));
                }
                catch
                {
                    Trace.TraceWarning(string.Format("Attribute '{0}' cannot be set - type '{1}' cannot convert from string '{2}'.", attributeName, descriptor.PropertyType.FullName, attributeValue));
                }
            }
            else
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
            }
            return true;
        }
    }
}