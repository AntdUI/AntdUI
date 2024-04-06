// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace AntdUI.Svg
{
    public class SvgUse : SvgVisualElement
    {
        public override string ClassName { get => "use"; }

        private Uri _referencedElement;

        [SvgAttribute("href", SvgAttributeAttribute.XLinkNamespace)]
        public virtual Uri ReferencedElement
        {
            get { return _referencedElement; }
            set { _referencedElement = value; }
        }

        private bool ElementReferencesUri(SvgElement element, List<Uri> elementUris)
        {
            var useElement = element as SvgUse;
            if (useElement != null)
            {
                if (elementUris.Contains(useElement.ReferencedElement))
                {
                    return true;
                }
                // also detect cycles in referenced elements
                var refElement = OwnerDocument.IdManager.GetElementById(useElement.ReferencedElement);
                if (refElement is SvgUse)
                {
                    elementUris.Add(useElement.ReferencedElement);
                }
                return useElement.ReferencedElementReferencesUri(elementUris);
            }
            var groupElement = element as SvgGroup;
            if (groupElement != null)
            {
                foreach (var child in groupElement.Children)
                {
                    if (ElementReferencesUri(child, elementUris))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool ReferencedElementReferencesUri(List<Uri> elementUris)
        {
            var refElement = OwnerDocument.IdManager.GetElementById(ReferencedElement);
            return ElementReferencesUri(refElement, elementUris);
        }

        /// <summary>
        /// Checks for any direct or indirect recursions in referenced elements, 
        /// including recursions via groups.
        /// </summary>
        /// <returns>True if any recursions are found.</returns>
        private bool HasRecursiveReference()
        {
            var refElement = OwnerDocument.IdManager.GetElementById(ReferencedElement);
            var uris = new List<Uri>() { ReferencedElement };
            return ElementReferencesUri(refElement, uris);
        }

        [SvgAttribute("x")]
        public virtual SvgUnit X
        {
            get { return Attributes.GetAttribute<SvgUnit>("x"); }
            set { Attributes["x"] = value; }
        }

        [SvgAttribute("y")]
        public virtual SvgUnit Y
        {
            get { return Attributes.GetAttribute<SvgUnit>("y"); }
            set { Attributes["y"] = value; }
        }


        [SvgAttribute("width")]
        public virtual SvgUnit Width
        {
            get { return Attributes.GetAttribute<SvgUnit>("width"); }
            set { Attributes["width"] = value; }
        }

        [SvgAttribute("height")]
        public virtual SvgUnit Height
        {
            get { return Attributes.GetAttribute<SvgUnit>("height"); }
            set { Attributes["height"] = value; }
        }

        /// <summary>
        /// Applies the required transforms to <see cref="ISvgRenderer"/>.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> to be transformed.</param>
        protected internal override bool PushTransforms(ISvgRenderer renderer)
        {
            if (!base.PushTransforms(renderer)) return false;
            renderer.TranslateTransform(X.ToDeviceValue(renderer, UnitRenderingType.Horizontal, this),
                                        Y.ToDeviceValue(renderer, UnitRenderingType.Vertical, this),
                                        MatrixOrder.Prepend);
            return true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgUse"/> class.
        /// </summary>
        public SvgUse()
        {
            X = 0;
            Y = 0;
            Width = 0;
            Height = 0;
        }

        public override GraphicsPath Path(ISvgRenderer renderer)
        {
            SvgVisualElement element = (SvgVisualElement)OwnerDocument.IdManager.GetElementById(ReferencedElement);
            return (element != null && !HasRecursiveReference()) ? element.Path(renderer) : null;
        }

        /// <summary>
        /// Gets an <see cref="SvgPoint"/> representing the top left point of the rectangle.
        /// </summary>
        public SvgPoint Location
        {
            get { return new SvgPoint(X, Y); }
        }

        /// <summary>
        /// Gets the bounds of the element.
        /// </summary>
        /// <value>The bounds.</value>
        public override RectangleF Bounds
        {
            get
            {
                var ew = Width.ToDeviceValue(null, UnitRenderingType.Horizontal, this);
                var eh = Height.ToDeviceValue(null, UnitRenderingType.Vertical, this);
                if (ew > 0 && eh > 0)
                    return TransformedBounds(new RectangleF(Location.ToDeviceValue(null, this),
                        new SizeF(ew, eh)));
                var element = OwnerDocument.IdManager.GetElementById(ReferencedElement) as SvgVisualElement;
                if (element != null)
                {
                    return element.Bounds;
                }

                return new RectangleF();
            }
        }

        protected override bool Renderable { get { return false; } }

        protected override void Render(ISvgRenderer renderer)
        {
            if (Visible && Displayable && ReferencedElement != null && !HasRecursiveReference() && PushTransforms(renderer))
            {
                SetClip(renderer);

                var element = OwnerDocument.IdManager.GetElementById(ReferencedElement) as SvgVisualElement;
                if (element != null)
                {
                    var ew = Width.ToDeviceValue(renderer, UnitRenderingType.Horizontal, this);
                    var eh = Height.ToDeviceValue(renderer, UnitRenderingType.Vertical, this);
                    if (ew > 0 && eh > 0)
                    {
                        var viewBox = element.Attributes.GetAttribute<SvgViewBox>("viewBox");
                        if (viewBox != SvgViewBox.Empty && Math.Abs(ew - viewBox.Width) > float.Epsilon && Math.Abs(eh - viewBox.Height) > float.Epsilon)
                        {
                            var sw = ew / viewBox.Width;
                            var sh = eh / viewBox.Height;
                            renderer.ScaleTransform(sw, sh, MatrixOrder.Prepend);
                        }
                    }

                    var origParent = element.Parent;
                    element._parent = this;
                    // as the new parent may have other styles that are inherited,
                    // we have to redraw the paths for the children
                    element.InvalidateChildPaths();
                    element.RenderElement(renderer);
                    element._parent = origParent;
                }

                ResetClip(renderer);
                PopTransforms(renderer);
            }
        }
    }
}