// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using AntdUI.Svg.Transforms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace AntdUI.Svg
{
    /// <summary>
    /// The base class of which all SVG elements are derived from.
    /// </summary>
    public abstract partial class SvgElement : ISvgElement, ISvgTransformable, ICloneable, ISvgNode
    {
        internal const int StyleSpecificity_PresAttribute = 0;
        internal const int StyleSpecificity_InlineStyle = 1 << 16;

        internal SvgElement _parent;
        private string _elementName;
        private SvgAttributeCollection _attributes;
        private EventHandlerList _eventHandlers;
        private SvgElementCollection _children;
        private Region _graphicsClip;
        private Matrix _graphicsMatrix;
        private SvgCustomAttributeCollection _customAttributes;
        private List<ISvgNode> _nodes = new List<ISvgNode>();


        private Dictionary<string, SortedDictionary<int, string>> _styles = new Dictionary<string, SortedDictionary<int, string>>();


        public void AddStyle(string name, string value, int specificity)
        {
            SortedDictionary<int, string> rules;
            if (!_styles.TryGetValue(name, out rules))
            {
                rules = new SortedDictionary<int, string>();
                _styles[name] = rules;
            }
            while (rules.ContainsKey(specificity)) specificity++;
            rules[specificity] = value;
        }
        public void FlushStyles()
        {
            if (_styles.Any())
            {
                var styles = new Dictionary<string, SortedDictionary<int, string>>();
                foreach (var s in _styles)
                {
                    if (!SvgElementFactory.SetPropertyValue(this, s.Key, s.Value.Last().Value, OwnerDocument, isStyle: true))
                    {
                        styles.Add(s.Key, s.Value);
                    }
                }
                _styles = styles;
            }
        }

        /// <summary>
        /// Gets the name of the element.
        /// </summary>
        protected internal string ElementName
        {
            get
            {
                if (string.IsNullOrEmpty(_elementName))
                {
                    _elementName = ClassName;
                }
                return _elementName;
            }
            internal set { _elementName = value; }
        }
        public virtual string ClassName => string.Empty;

        /// <summary>
        /// Gets or sets the color <see cref="SvgPaintServer"/> of this element which drives the currentColor property.
        /// </summary>
        [SvgAttribute("color", true)]
        public virtual SvgPaintServer Color
        {
            get { return (Attributes["color"] == null) ? SvgColourServer.NotSet : (SvgPaintServer)Attributes["color"]; }
            set { Attributes["color"] = value; }
        }

        /// <summary>
        /// Gets or sets the content of the element.
        /// </summary>
        private string _content;
        public virtual string Content
        {
            get
            {
                return _content;
            }
            set
            {
                if (_content != null)
                {
                    var oldVal = _content;
                    _content = value;
                    if (_content != oldVal)
                        OnContentChanged(new ContentEventArgs { Content = value });
                }
                else
                {
                    _content = value;
                    OnContentChanged(new ContentEventArgs { Content = value });
                }
            }
        }

        /// <summary>
        /// Gets an <see cref="EventHandlerList"/> of all events belonging to the element.
        /// </summary>
        protected virtual EventHandlerList Events
        {
            get { return _eventHandlers; }
        }

        /// <summary>
        /// Gets a collection of all child <see cref="SvgElement"/> objects.
        /// </summary>
        public virtual SvgElementCollection Children
        {
            get { return _children; }
        }

        public IList<ISvgNode> Nodes
        {
            get { return _nodes; }
        }

        public IEnumerable<SvgElement> Descendants()
        {
            return AsEnumerable().Descendants();
        }
        private IEnumerable<SvgElement> AsEnumerable()
        {
            yield return this;
        }

        /// <summary>
        /// Gets a value to determine whether the element has children.
        /// </summary>
        public virtual bool HasChildren()
        {
            return (Children.Count > 0);
        }

        /// <summary>
        /// Gets the parent <see cref="SvgElement"/>.
        /// </summary>
        /// <value>An <see cref="SvgElement"/> if one exists; otherwise null.</value>
        public virtual SvgElement Parent
        {
            get { return _parent; }
        }

        public IEnumerable<SvgElement> Parents
        {
            get
            {
                var curr = this;
                while (curr.Parent != null)
                {
                    curr = curr.Parent;
                    yield return curr;
                }
            }
        }
        public IEnumerable<SvgElement> ParentsAndSelf
        {
            get
            {
                var curr = this;
                yield return curr;
                while (curr.Parent != null)
                {
                    curr = curr.Parent;
                    yield return curr;
                }
            }
        }

        /// <summary>
        /// Gets the owner <see cref="SvgDocument"/>.
        /// </summary>
        public virtual SvgDocument OwnerDocument
        {
            get
            {
                if (this is SvgDocument)
                {
                    return this as SvgDocument;
                }
                else
                {
                    if (Parent != null)
                        return Parent.OwnerDocument;
                    else
                        return null;
                }
            }
        }

        /// <summary>
        /// Gets a collection of element attributes.
        /// </summary>
        protected internal virtual SvgAttributeCollection Attributes
        {
            get
            {
                if (_attributes == null)
                {
                    _attributes = new SvgAttributeCollection(this);
                }

                return _attributes;
            }
        }

        /// <summary>
        /// Gets a collection of custom attributes
        /// </summary>
        public SvgCustomAttributeCollection CustomAttributes
        {
            get { return _customAttributes; }
        }

        /// <summary>
        /// Applies the required transforms to <see cref="ISvgRenderer"/>.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> to be transformed.</param>
        protected internal virtual bool PushTransforms(ISvgRenderer renderer)
        {
            _graphicsMatrix = renderer.Transform;
            _graphicsClip = renderer.GetClip();

            // Return if there are no transforms
            if (Transforms == null || Transforms.Count == 0) return true;
            Matrix transformMatrix = renderer.Transform.Clone();
            var bound = renderer.GetBoundable();
            foreach (SvgTransform transformation in Transforms)
            {
                transformMatrix.Multiply(transformation.Matrix(bound.Bounds.Width, bound.Bounds.Height));
            }
            renderer.Transform = transformMatrix;
            return true;
        }

        /// <summary>
        /// Removes any previously applied transforms from the specified <see cref="ISvgRenderer"/>.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> that should have transforms removed.</param>
        protected internal virtual void PopTransforms(ISvgRenderer renderer)
        {
            renderer.Transform = _graphicsMatrix;
            _graphicsMatrix = null;
            renderer.SetClip(_graphicsClip);
            _graphicsClip = null;
        }

        /// <summary>
        /// Applies the required transforms to <see cref="ISvgRenderer"/>.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> to be transformed.</param>
        void ISvgTransformable.PushTransforms(ISvgRenderer renderer)
        {
            PushTransforms(renderer);
        }

        /// <summary>
        /// Removes any previously applied transforms from the specified <see cref="ISvgRenderer"/>.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> that should have transforms removed.</param>
        void ISvgTransformable.PopTransforms(ISvgRenderer renderer)
        {
            PopTransforms(renderer);
        }

        /// <summary>
        /// Gets or sets the element transforms.
        /// </summary>
        /// <value>The transforms.</value>
        [SvgAttribute("transform")]
        public SvgTransformCollection Transforms
        {
            get { return (Attributes.GetAttribute<SvgTransformCollection>("transform")); }
            set
            {
                var old = Transforms;
                if (old != null)
                    old.TransformChanged -= Attributes_AttributeChanged;
                value.TransformChanged += Attributes_AttributeChanged;
                Attributes["transform"] = value;
            }
        }

        /// <summary>
        /// Transforms the given rectangle with the set transformation, if any.
        /// Can be applied to bounds calculated without considering the element transformation. 
        /// </summary>
        /// <param name="bounds">The rectangle to be transformed.</param>
        /// <returns>The transformed rectangle, or the original rectangle if no transformation exists.</returns>
        protected RectangleF TransformedBounds(RectangleF bounds)
        {
            if (Transforms != null && Transforms.Count > 0)
            {
                var path = new GraphicsPath();
                path.AddRectangle(bounds);
                path.Transform(Transforms.GetMatrix());
                return path.GetBounds();
            }
            return bounds;
        }

        /// <summary>
        /// Gets or sets the ID of the element.
        /// </summary>
        /// <exception cref="SvgException">The ID is already used within the <see cref="SvgDocument"/>.</exception>
        [SvgAttribute("id")]
        public string ID
        {
            get { return Attributes.GetAttribute<string>("id"); }
            set
            {
                SetAndForceUniqueID(value, false);
            }
        }

        /// <summary>
        /// Gets or sets the space handling.
        /// </summary>
        /// <value>The space handling.</value>
        [SvgAttribute("space", SvgAttributeAttribute.XmlNamespace)]
        public virtual XmlSpaceHandling SpaceHandling
        {
            get { return (Attributes["space"] == null) ? XmlSpaceHandling.@default : (XmlSpaceHandling)Attributes["space"]; }
            set { Attributes["space"] = value; }
        }

        public void SetAndForceUniqueID(string value, bool autoForceUniqueID = true, Action<SvgElement, string, string> logElementOldIDNewID = null)
        {
            // Don't do anything if it hasn't changed
            if (string.Compare(ID, value) == 0)
            {
                return;
            }

            if (OwnerDocument != null)
            {
                OwnerDocument.IdManager.Remove(this);
            }

            Attributes["id"] = value;

            if (OwnerDocument != null)
            {
                OwnerDocument.IdManager.AddAndForceUniqueID(this, null, autoForceUniqueID, logElementOldIDNewID);
            }
        }

        /// <summary>
        /// Only used by the ID Manager
        /// </summary>
        /// <param name="newID"></param>
        internal void ForceUniqueID(string newID)
        {
            Attributes["id"] = newID;
        }

        /// <summary>
        /// Called by the underlying <see cref="SvgElement"/> when an element has been added to the
        /// <see cref="Children"/> collection.
        /// </summary>
        /// <param name="child">The <see cref="SvgElement"/> that has been added.</param>
        /// <param name="index">An <see cref="int"/> representing the index where the element was added to the collection.</param>
        protected virtual void AddElement(SvgElement child, int index)
        {
        }

        /// <summary>
        /// Fired when an Element was added to the children of this Element
        /// </summary>
        public event EventHandler<ChildAddedEventArgs> ChildAdded;

        /// <summary>
        /// Calls the <see cref="AddElement"/> method with the specified parameters.
        /// </summary>
        /// <param name="child">The <see cref="SvgElement"/> that has been added.</param>
        /// <param name="index">An <see cref="int"/> representing the index where the element was added to the collection.</param>
        internal void OnElementAdded(SvgElement child, int index)
        {
            AddElement(child, index);
            SvgElement sibling = null;
            if (index < (Children.Count - 1))
            {
                sibling = Children[index + 1];
            }
            var handler = ChildAdded;
            if (handler != null)
            {
                handler(this, new ChildAddedEventArgs { NewChild = child, BeforeSibling = sibling });
            }
        }

        /// <summary>
        /// Called by the underlying <see cref="SvgElement"/> when an element has been removed from the
        /// <see cref="Children"/> collection.
        /// </summary>
        /// <param name="child">The <see cref="SvgElement"/> that has been removed.</param>
        protected virtual void RemoveElement(SvgElement child)
        {
        }

        /// <summary>
        /// Calls the <see cref="RemoveElement"/> method with the specified <see cref="SvgElement"/> as the parameter.
        /// </summary>
        /// <param name="child">The <see cref="SvgElement"/> that has been removed.</param>
        internal void OnElementRemoved(SvgElement child)
        {
            RemoveElement(child);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgElement"/> class.
        /// </summary>
        public SvgElement()
        {
            _children = new SvgElementCollection(this);
            _eventHandlers = new EventHandlerList();
            _customAttributes = new SvgCustomAttributeCollection(this);

            Transforms = new SvgTransformCollection();

            //subscribe to attribute events
            Attributes.AttributeChanged += Attributes_AttributeChanged;
            CustomAttributes.AttributeChanged += Attributes_AttributeChanged;
        }

        //dispatch attribute event
        void Attributes_AttributeChanged(object sender, AttributeEventArgs e)
        {
            OnAttributeChanged(e);
        }

        /// <summary>
        /// Renders this element to the <see cref="ISvgRenderer"/>.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> that the element should use to render itself.</param>
        public void RenderElement(ISvgRenderer renderer)
        {
            Render(renderer);
        }

        /// <summary>Derrived classes may decide that the element should not be written. For example, the text element shouldn't be written if it's empty.</summary>
        public virtual bool ShouldWriteElement()
        {
            //Write any element who has a name.
            return (ElementName != String.Empty);
        }

        /// <summary>
        /// Renders the <see cref="SvgElement"/> and contents to the specified <see cref="ISvgRenderer"/> object.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> object to render to.</param>
        protected virtual void Render(ISvgRenderer renderer)
        {
            PushTransforms(renderer);
            RenderChildren(renderer);
            PopTransforms(renderer);
        }

        /// <summary>
        /// Renders the children of this <see cref="SvgElement"/>.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> to render the child <see cref="SvgElement"/>s to.</param>
        protected virtual void RenderChildren(ISvgRenderer renderer)
        {
            foreach (SvgElement element in Children)
            {
                element.Render(renderer);
            }
        }

        /// <summary>
        /// Renders the <see cref="SvgElement"/> and contents to the specified <see cref="ISvgRenderer"/> object.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> object to render to.</param>
        void ISvgElement.Render(ISvgRenderer renderer)
        {
            Render(renderer);
        }

        /// <summary>
        /// Recursive method to add up the paths of all children
        /// </summary>
        /// <param name="elem"></param>
        /// <param name="path"></param>
        protected void AddPaths(SvgElement elem, GraphicsPath path)
        {
            foreach (var child in elem.Children)
            {
                // Skip to avoid double calculate Symbol element
                // symbol element is only referenced by use element 
                // So here we need to skip when it is directly considered
                if (child is Svg.Document_Structure.SvgSymbol)
                    continue;

                if (child is SvgVisualElement)
                {
                    if (!(child is SvgGroup))
                    {
                        var childPath = ((SvgVisualElement)child).Path(null);

                        if (childPath != null)
                        {
                            childPath = (GraphicsPath)childPath.Clone();
                            if (child.Transforms != null)
                                childPath.Transform(child.Transforms.GetMatrix());

                            if (childPath.PointCount > 0) path.AddPath(childPath, false);
                        }
                    }
                }

                if (!(child is SvgPaintServer)) AddPaths(child, path);
            }
        }

        /// <summary>
        /// Recursive method to add up the paths of all children
        /// </summary>
        /// <param name="elem"></param>
        /// <param name="renderer"></param>
        protected GraphicsPath GetPaths(SvgElement elem, ISvgRenderer renderer)
        {
            var ret = new GraphicsPath();

            foreach (var child in elem.Children)
            {
                if (child is SvgVisualElement)
                {
                    if (!(child is SvgGroup))
                    {
                        var childPath = ((SvgVisualElement)child).Path(renderer);

                        // Non-group element can have child element which we have to consider. i.e tspan in text element
                        if (child.Children.Count > 0)
                            childPath.AddPath(GetPaths(child, renderer), false);

                        if (childPath != null && childPath.PointCount > 0)
                        {
                            childPath = (GraphicsPath)childPath.Clone();
                            if (child.Transforms != null)
                                childPath.Transform(child.Transforms.GetMatrix());

                            ret.AddPath(childPath, false);
                        }
                    }
                    else
                    {
                        var childPath = GetPaths(child, renderer);
                        if (childPath != null && childPath.PointCount > 0)
                        {
                            if (child.Transforms != null)
                                childPath.Transform(child.Transforms.GetMatrix());

                            ret.AddPath(childPath, false);
                        }
                    }
                }

            }

            return ret;
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public virtual object Clone()
        {
            return MemberwiseClone();
        }

        protected void OnAttributeChanged(AttributeEventArgs args)
        {

        }

        /// <summary>
        /// Fired when an Atrribute of this Element has changed
        /// </summary>
        public event EventHandler<ContentEventArgs> ContentChanged;

        protected void OnContentChanged(ContentEventArgs args)
        {
            var handler = ContentChanged;
            if (handler != null)
            {
                handler(this, args);
            }
        }
    }

    public class SVGArg : EventArgs
    {
        public string SessionID;
    }


    /// <summary>
    /// Describes the Attribute which was set
    /// </summary>
    public class AttributeEventArgs : SVGArg
    {
        public string Attribute;
        public object Value;
    }

    /// <summary>
    /// Content of this whas was set
    /// </summary>
    public class ContentEventArgs : SVGArg
    {
        public string Content;
    }

    /// <summary>
    /// Describes the Attribute which was set
    /// </summary>
    public class ChildAddedEventArgs : SVGArg
    {
        public SvgElement NewChild;
        public SvgElement BeforeSibling;
    }

    /// <summary>
    /// Represents a string argument
    /// </summary>
    public class StringArg : SVGArg
    {
        public string s;
    }

    public class MouseScrollArg : SVGArg
    {
        public int Scroll;

        /// <summary>
        /// Alt modifier key pressed
        /// </summary>
        public bool AltKey;

        /// <summary>
        /// Shift modifier key pressed
        /// </summary>
        public bool ShiftKey;

        /// <summary>
        /// Control modifier key pressed
        /// </summary>
        public bool CtrlKey;
    }

    public interface ISvgNode
    {
        string Content { get; }
    }

    /// <summary>This interface mostly indicates that a node is not to be drawn when rendering the SVG.</summary>
    public interface ISvgDescriptiveElement
    {
    }

    internal interface ISvgElement
    {
        SvgElement Parent { get; }
        SvgElementCollection Children { get; }
        IList<ISvgNode> Nodes { get; }

        string ClassName { get; }

        void Render(ISvgRenderer renderer);
    }
}