// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using AntdUI.Svg.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml;

namespace AntdUI.Svg
{
    /// <summary>
    /// The class used to create and load SVG documents.
    /// </summary>
    public class SvgDocument : SvgFragment, ITypeDescriptorContext
    {
        private SvgElementIdManager _idManager;

        private Dictionary<string, IEnumerable<SvgFontFace>> _fontDefns = null;

        internal Dictionary<string, IEnumerable<SvgFontFace>> FontDefns()
        {
            if (_fontDefns == null)
            {
                _fontDefns = (from f in Descendants().OfType<SvgFontFace>()
                              group f by f.FontFamily into family
                              select family).ToDictionary(f => f.Key, f => (IEnumerable<SvgFontFace>)f);
            }
            return _fontDefns;
        }

        public Uri BaseUri { get; set; }

        /// <summary>
        /// Gets an <see cref="SvgElementIdManager"/> for this document.
        /// </summary>
        protected internal virtual SvgElementIdManager IdManager
        {
            get
            {
                if (_idManager == null)
                {
                    _idManager = new SvgElementIdManager(this);
                }

                return _idManager;
            }
        }

        /// <summary>
        /// Overwrites the current IdManager with a custom implementation. 
        /// Be careful with this: If elements have been inserted into the document before,
        /// you have to take care that the new IdManager also knows of them.
        /// </summary>
        /// <param name="manager"></param>
        public void OverwriteIdManager(SvgElementIdManager manager)
        {
            _idManager = manager;
        }

        /// <summary>
        /// Gets or sets the Pixels Per Inch of the rendered image.
        /// </summary>
        public static int Ppi { get => (int)(Config.Dpi * 96); }

        /// <summary>
        /// Gets or sets an external Cascading Style Sheet (CSS)
        /// </summary>
        public string ExternalCSSHref { get; set; }

        #region ITypeDescriptorContext Members

        IContainer ITypeDescriptorContext.Container
        {
            get { throw new NotImplementedException(); }
        }

        object ITypeDescriptorContext.Instance
        {
            get { return this; }
        }

        void ITypeDescriptorContext.OnComponentChanged()
        {
            throw new NotImplementedException();
        }

        bool ITypeDescriptorContext.OnComponentChanging()
        {
            throw new NotImplementedException();
        }

        PropertyDescriptor ITypeDescriptorContext.PropertyDescriptor
        {
            get { throw new NotImplementedException(); }
        }

        object IServiceProvider.GetService(Type serviceType)
        {
            throw new NotImplementedException();
        }

        #endregion

        /// <summary>
        /// Retrieves the <see cref="SvgElement"/> with the specified ID.
        /// </summary>
        /// <param name="id">A <see cref="string"/> containing the ID of the element to find.</param>
        /// <returns>An <see cref="SvgElement"/> of one exists with the specified ID; otherwise false.</returns>
        public virtual SvgElement GetElementById(string id)
        {
            return IdManager.GetElementById(id);
        }

        /// <summary>
        /// Retrieves the <see cref="SvgElement"/> with the specified ID.
        /// </summary>
        /// <param name="id">A <see cref="string"/> containing the ID of the element to find.</param>
        /// <returns>An <see cref="SvgElement"/> of one exists with the specified ID; otherwise false.</returns>
        public virtual TSvgElement GetElementById<TSvgElement>(string id) where TSvgElement : SvgElement
        {
            return (GetElementById(id) as TSvgElement);
        }

        /// <summary>
        /// Opens the document at the specified path and loads the SVG contents.
        /// </summary>
        /// <param name="path">A <see cref="string"/> containing the path of the file to open.</param>
        /// <param name="entities">A dictionary of custom entity definitions to be used when resolving XML entities within the document.</param>
        /// <returns>An <see cref="SvgDocument"/> with the contents loaded.</returns>
        /// <exception cref="FileNotFoundException">The document at the specified <paramref name="path"/> cannot be found.</exception>
        public static T Open<T>(string path) where T : SvgDocument, new()
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }

            if (!File.Exists(path))
            {
                throw new FileNotFoundException("The specified document cannot be found.", path);
            }

            using (var stream = File.OpenRead(path))
            {
                var doc = Open<T>(stream);
                doc.BaseUri = new Uri(System.IO.Path.GetFullPath(path));
                return doc;
            }
        }


        /// <summary>
        /// Attempts to create an SVG document from the specified string data.
        /// </summary>
        /// <param name="svg">The SVG data.</param>
        public static T FromSvg<T>(string svg) where T : SvgDocument, new()
        {
            if (string.IsNullOrEmpty(svg))
            {
                throw new ArgumentNullException("svg");
            }

            using (var strReader = new System.IO.StringReader(svg))
            {
                var reader = new SvgTextReader(strReader);
                reader.WhitespaceHandling = WhitespaceHandling.None;
                return Open<T>(reader);
            }
        }

        /// <summary>
        /// Opens an SVG document from the specified <see cref="Stream"/> and adds the specified entities.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> containing the SVG document to open.</param>
        /// <param name="entities">Custom entity definitions.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="stream"/> parameter cannot be <c>null</c>.</exception>
        public static T Open<T>(Stream stream) where T : SvgDocument, new()
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            // Don't close the stream via a dispose: that is the client's job.
            var reader = new SvgTextReader(stream);
            reader.WhitespaceHandling = WhitespaceHandling.None;
            return Open<T>(reader);
        }

        private static T? Open<T>(XmlReader reader) where T : SvgDocument, new()
        {
            var elementStack = new Stack<SvgElement>();
            bool elementEmpty;
            SvgElement element, parent;
            T? svgDocument = null;
            var elementFactory = new SvgElementFactory();

            try
            {
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            // Does this element have a value or children
                            // (Must do this check here before we progress to another node)
                            elementEmpty = reader.IsEmptyElement;
                            // Create element
                            if (elementStack.Count > 0)
                            {
                                element = elementFactory.CreateElement(reader, svgDocument);
                            }
                            else
                            {
                                svgDocument = elementFactory.CreateDocument<T>(reader);
                                element = svgDocument;
                            }

                            // Add to the parents children
                            if (elementStack.Count > 0)
                            {
                                parent = elementStack.Peek();
                                if (parent != null && element != null)
                                {
                                    parent.Children.Add(element);
                                    parent.Nodes.Add(element);
                                }
                            }

                            // Push element into stack
                            elementStack.Push(element);

                            // Need to process if the element is empty
                            if (elementEmpty)
                            {
                                goto case XmlNodeType.EndElement;
                            }

                            break;
                        case XmlNodeType.EndElement:
                            // Pop the element out of the stack
                            element = elementStack.Pop();
                            if (element.Nodes.OfType<SvgContentNode>().Any()) element.Content = (from e in element.Nodes select e.Content).Aggregate((p, c) => p + c);
                            else element.Nodes.Clear(); // No sense wasting the space where it isn't needed
                            break;
                        case XmlNodeType.CDATA:
                        case XmlNodeType.Text:
                            element = elementStack.Peek();
                            element.Nodes.Add(new SvgContentNode() { Content = reader.Value });
                            break;
                        case XmlNodeType.EntityReference:
                            reader.ResolveEntity();
                            element = elementStack.Peek();
                            element.Nodes.Add(new SvgContentNode() { Content = reader.Value });
                            break;
                    }
                }
            }
            catch
            { }

            if (svgDocument != null) FlushStyles(svgDocument);
            return svgDocument;
        }

        private static void FlushStyles(SvgElement elem)
        {
            elem.FlushStyles();
            foreach (var child in elem.Children)
            {
                FlushStyles(child);
            }
        }

        private void Draw(ISvgRenderer renderer, ISvgBoundable boundable)
        {
            renderer.SetBoundable(boundable);
            Render(renderer);
        }

        /// <summary>
        /// Renders the <see cref="SvgDocument"/> to the specified <see cref="ISvgRenderer"/>.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> to render the document with.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="renderer"/> parameter cannot be <c>null</c>.</exception>
        public void Draw(ISvgRenderer renderer)
        {
            if (renderer == null)
            {
                throw new ArgumentNullException("renderer");
            }

            Draw(renderer, this);
        }

        /// <summary>
        /// Renders the <see cref="SvgDocument"/> to the specified <see cref="Graphics"/>.
        /// </summary>
        /// <param name="graphics">The <see cref="Graphics"/> to be rendered to.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="graphics"/> parameter cannot be <c>null</c>.</exception>
        public void Draw(Graphics graphics)
        {
            Draw(graphics, null);
        }

        /// <summary>
        /// Renders the <see cref="SvgDocument"/> to the specified <see cref="Graphics"/>.
        /// </summary>
        /// <param name="graphics">The <see cref="Graphics"/> to be rendered to.</param>
        /// <param name="size">The <see cref="SizeF"/> to render the document. If <c>null</c> document is rendered at the default document size.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="graphics"/> parameter cannot be <c>null</c>.</exception>
        public void Draw(Graphics graphics, SizeF? size)
        {
            if (graphics == null)
            {
                throw new ArgumentNullException("graphics");
            }

            using (var renderer = SvgRenderer.FromGraphics(graphics))
            {
                var boundable = size.HasValue ? (ISvgBoundable)new GenericBoundable(0, 0, size.Value.Width, size.Value.Height) : this;
                Draw(renderer, boundable);
            }
        }

        /// <summary>
        /// Renders the <see cref="SvgDocument"/> and returns the image as a <see cref="Bitmap"/>.
        /// </summary>
        /// <returns>A <see cref="Bitmap"/> containing the rendered document.</returns>
        public virtual Bitmap Draw()
        {
            Bitmap bitmap = null;
            try
            {
                try
                {
                    var size = GetDimensions();
                    bitmap = new Bitmap((int)Math.Round(size.Width), (int)Math.Round(size.Height));
                }
                catch (ArgumentException e)
                {
                    // When processing too many files at one the system can run out of memory
                    throw new SvgMemoryException("Cannot process SVG file, cannot allocate the required memory", e);
                }

                //bitmap.SetResolution(300, 300);

                Draw(bitmap);
            }
            catch
            {
                if (bitmap != null)
                    bitmap.Dispose();
                throw;
            }

            //Trace.TraceInformation("End Render");
            return bitmap;
        }

        /// <summary>
        /// Renders the <see cref="SvgDocument"/> into a given Bitmap <see cref="Bitmap"/>.
        /// </summary>
        public virtual void Draw(Bitmap bitmap)
        {
            //Trace.TraceInformation("Begin Render");

            using (var renderer = SvgRenderer.FromImage(bitmap))
            {
                // EO, 2014-12-05: Requested to ensure proper zooming out (reduce size). Otherwise it clip the image.
                Overflow = SvgOverflow.Auto;

                var boundable = new GenericBoundable(0, 0, bitmap.Width, bitmap.Height);
                Draw(renderer, boundable);
            }

            //Trace.TraceInformation("End Render");
        }

        /// <summary>
        /// Renders the <see cref="SvgDocument"/> in given size and returns the image as a <see cref="Bitmap"/>.
        /// If one of rasterWidth and rasterHeight is zero, the image is scaled preserving aspect ratio,
        /// otherwise the aspect ratio is ignored.
        /// </summary>
        /// <returns>A <see cref="Bitmap"/> containing the rendered document.</returns>
        public virtual Bitmap Draw(int rasterWidth, int rasterHeight)
        {
            var imageSize = GetDimensions();
            var bitmapSize = imageSize;
            RasterizeDimensions(ref bitmapSize, rasterWidth, rasterHeight);

            if (bitmapSize.Width == 0 || bitmapSize.Height == 0)
                return null;

            Bitmap bitmap = null;
            try
            {
                try
                {
                    bitmap = new Bitmap((int)Math.Round(bitmapSize.Width), (int)Math.Round(bitmapSize.Height));
                }
                catch (ArgumentException e)
                {
                    // When processing too many files at one the system can run out of memory
                    throw new SvgMemoryException("Cannot process SVG file, cannot allocate the required memory", e);
                }

                using (var renderer = SvgRenderer.FromImage(bitmap))
                {
                    renderer.ScaleTransform(bitmapSize.Width / imageSize.Width, bitmapSize.Height / imageSize.Height);
                    var boundable = new GenericBoundable(0, 0, imageSize.Width, imageSize.Height);
                    Draw(renderer, boundable);
                }
            }
            catch
            {
                if (bitmap != null)
                    bitmap.Dispose();
                throw;
            }

            return bitmap;
        }

        /// <summary>
        /// If both or one of raster height and width is not given (0), calculate that missing value from original SVG size
        /// while keeping original SVG size ratio
        /// </summary>
        /// <param name="size"></param>
        /// <param name="rasterWidth"></param>
        /// <param name="rasterHeight"></param>
        public virtual void RasterizeDimensions(ref SizeF size, int rasterWidth, int rasterHeight)
        {
            if (size == null || size.Width == 0)
                return;

            // Ratio of height/width of the original SVG size, to be used for scaling transformation
            float ratio = size.Height / size.Width;

            size.Width = rasterWidth > 0 ? (float)rasterWidth : size.Width;
            size.Height = rasterHeight > 0 ? (float)rasterHeight : size.Height;

            if (rasterHeight == 0 && rasterWidth > 0)
            {
                size.Height = (int)(rasterWidth * ratio);
            }
            else if (rasterHeight > 0 && rasterWidth == 0)
            {
                size.Width = (int)(rasterHeight / ratio);
            }
        }
    }
}