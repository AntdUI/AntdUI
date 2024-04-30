// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace AntdUI.Svg
{
    public abstract class SvgTextBase : SvgVisualElement
    {
        [CLSCompliant(false)] protected SvgUnitCollection _x = new SvgUnitCollection();
        [CLSCompliant(false)] protected SvgUnitCollection _y = new SvgUnitCollection();
        [CLSCompliant(false)] protected SvgUnitCollection _dy = new SvgUnitCollection();
        [CLSCompliant(false)] protected SvgUnitCollection _dx = new SvgUnitCollection();
        private string _rotate;
        private List<float> _rotations = new List<float>();

        /// <summary>
        /// Gets or sets the text to be rendered.
        /// </summary>
        public virtual string Text
        {
            get { return base.Content; }
            set
            {
                Nodes.Clear();
                Children.Clear();
                if (value != null)
                {
                    Nodes.Add(new SvgContentNode { Content = value });
                }
                IsPathDirty = true;
                Content = value;
            }
        }

        public override XmlSpaceHandling SpaceHandling
        {
            get { return base.SpaceHandling; }
            set { base.SpaceHandling = value; IsPathDirty = true; }
        }

        /// <summary>
        /// Gets or sets the X.
        /// </summary>
        /// <value>The X.</value>
        [SvgAttribute("x")]
        public virtual SvgUnitCollection X
        {
            get { return _x; }
            set
            {
                if (_x != value)
                {
                    _x = value;
                    IsPathDirty = true;
                    OnAttributeChanged(new AttributeEventArgs { Attribute = "x", Value = value });
                }
            }
        }

        /// <summary>
        /// Gets or sets the dX.
        /// </summary>
        /// <value>The dX.</value>
        [SvgAttribute("dx")]
        public virtual SvgUnitCollection Dx
        {
            get { return _dx; }
            set
            {
                if (_dx != value)
                {
                    _dx = value;
                    IsPathDirty = true;
                    OnAttributeChanged(new AttributeEventArgs { Attribute = "dx", Value = value });
                }
            }
        }

        /// <summary>
        /// Gets or sets the Y.
        /// </summary>
        /// <value>The Y.</value>
        [SvgAttribute("y")]
        public virtual SvgUnitCollection Y
        {
            get { return _y; }
            set
            {
                if (_y != value)
                {
                    _y = value;
                    IsPathDirty = true;
                    OnAttributeChanged(new AttributeEventArgs { Attribute = "y", Value = value });
                }
            }
        }

        /// <summary>
        /// Gets or sets the dY.
        /// </summary>
        /// <value>The dY.</value>
        [SvgAttribute("dy")]
        public virtual SvgUnitCollection Dy
        {
            get { return _dy; }
            set
            {
                if (_dy != value)
                {
                    _dy = value;
                    IsPathDirty = true;
                    OnAttributeChanged(new AttributeEventArgs { Attribute = "dy", Value = value });
                }
            }
        }

        /// <summary>
        /// Gets or sets the rotate.
        /// </summary>
        /// <value>The rotate.</value>
        [SvgAttribute("rotate")]
        public virtual string Rotate
        {
            get { return _rotate; }
            set
            {
                if (_rotate != value)
                {
                    _rotate = value;
                    _rotations.Clear();
                    _rotations.AddRange(from r in _rotate.Split(new char[] { ',', ' ', '\r', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries) select float.Parse(r));
                    IsPathDirty = true;
                    OnAttributeChanged(new AttributeEventArgs { Attribute = "rotate", Value = value });
                }
            }
        }

        /// <summary>
        /// The pre-calculated length of the text
        /// </summary>
        [SvgAttribute("textLength", true)]
        public virtual SvgUnit TextLength
        {
            get { return (Attributes["textLength"] == null ? SvgUnit.None : (SvgUnit)Attributes["textLength"]); }
            set { Attributes["textLength"] = value; IsPathDirty = true; }
        }

        /// <summary>
        /// Gets or sets the text anchor.
        /// </summary>
        /// <value>The text anchor.</value>
        [SvgAttribute("lengthAdjust", true)]
        public virtual SvgTextLengthAdjust LengthAdjust
        {
            get { return (Attributes["lengthAdjust"] == null) ? SvgTextLengthAdjust.Spacing : (SvgTextLengthAdjust)Attributes["lengthAdjust"]; }
            set { Attributes["lengthAdjust"] = value; IsPathDirty = true; }
        }

        /// <summary>
        /// Specifies spacing behavior between text characters.
        /// </summary>
        [SvgAttribute("letter-spacing", true)]
        public virtual SvgUnit LetterSpacing
        {
            get { return (Attributes["letter-spacing"] == null ? SvgUnit.None : (SvgUnit)Attributes["letter-spacing"]); }
            set { Attributes["letter-spacing"] = value; IsPathDirty = true; }
        }

        /// <summary>
        /// Specifies spacing behavior between words.
        /// </summary>
        [SvgAttribute("word-spacing", true)]
        public virtual SvgUnit WordSpacing
        {
            get { return (Attributes["word-spacing"] == null ? SvgUnit.None : (SvgUnit)Attributes["word-spacing"]); }
            set { Attributes["word-spacing"] = value; IsPathDirty = true; }
        }

        /// <summary>
        /// Gets or sets the fill.
        /// </summary>
        /// <remarks>
        /// <para>Unlike other <see cref="SvgVisualElement"/>s, <see cref="SvgText"/> has a default fill of black rather than transparent.</para>
        /// </remarks>
        /// <value>The fill.</value>
        public override SvgPaintServer Fill
        {
            get { return (Attributes["fill"] == null) ? new SvgColourServer(System.Drawing.Color.Black) : (SvgPaintServer)Attributes["fill"]; }
            set { Attributes["fill"] = value; }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return Text;
        }

        /// <summary>
        /// Gets the bounds of the element.
        /// </summary>
        /// <value>The bounds.</value>
        public override RectangleF Bounds
        {
            get
            {
                var path = Path(null);
                foreach (var elem in Children.OfType<SvgVisualElement>())
                {
                    //When empty Text span, don't add path
                    var span = elem as SvgTextSpan;
                    if (span != null && span.Text == null)
                        continue;
                    path.AddPath(elem.Path(null), false);
                }
                if (Transforms != null && Transforms.Count > 0)
                {
                    path.Transform(Transforms.GetMatrix());
                }
                return path.GetBounds();
            }
        }

        /// <summary>
        /// Renders the <see cref="SvgElement"/> and contents to the specified <see cref="Graphics"/> object.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> object to render to.</param>
        /// <remarks>Necessary to make sure that any internal tspan elements get rendered as well</remarks>
        protected override void Render(ISvgRenderer renderer)
        {
            if ((Path(renderer) != null) && Visible && Displayable)
            {
                PushTransforms(renderer);
                SetClip(renderer);

                var opacity = Math.Min(Math.Max(Opacity, 0), 1);
                if (opacity == 1f)
                {
                    RenderFillAndStroke(renderer);
                    RenderChildren(renderer);
                }
                else
                {
                    IsPathDirty = true;
                    var bounds = Bounds;
                    IsPathDirty = true;

                    using (var canvas = new Bitmap((int)Math.Ceiling(bounds.Width), (int)Math.Ceiling(bounds.Height)))
                    {
                        using (var canvasRenderer = SvgRenderer.FromImage(canvas))
                        {
                            canvasRenderer.SetBoundable(renderer.GetBoundable());
                            canvasRenderer.TranslateTransform(-bounds.X, -bounds.Y);
                            RenderFillAndStroke(canvasRenderer);
                            RenderChildren(canvasRenderer);
                        }
                        var srcRect = new RectangleF(0f, 0f, bounds.Width, bounds.Height);
                        renderer.DrawImage(canvas, bounds, srcRect, GraphicsUnit.Pixel, opacity);
                    }
                }

                ResetClip(renderer);
                PopTransforms(renderer);
            }
        }

        internal virtual IEnumerable<ISvgNode> GetContentNodes()
        {
            return (Nodes == null || Nodes.Count < 1 ? Children.OfType<ISvgNode>().Where(o => !(o is ISvgDescriptiveElement)) : Nodes);
        }
        protected virtual GraphicsPath GetBaselinePath(ISvgRenderer renderer)
        {
            return null;
        }
        protected virtual float GetAuthorPathLength()
        {
            return 0;
        }

        private GraphicsPath _path;

        /// <summary>
        /// Gets the <see cref="GraphicsPath"/> for this element.
        /// </summary>
        /// <value></value>
        public override GraphicsPath Path(ISvgRenderer renderer)
        {
            //if there is a TSpan inside of this text element then path should not be null (even if this text is empty!)
            var nodes = GetContentNodes().Where(x => x is SvgContentNode &&
                                                     string.IsNullOrEmpty(x.Content.Trim(new[] { '\r', '\n', '\t' })));

            if (_path == null || IsPathDirty || nodes.Count() == 1)
            {
                renderer = (renderer ?? SvgRenderer.FromNull());
                SetPath(new TextDrawingState(renderer, this));
            }
            return _path;
        }

        private void SetPath(TextDrawingState state)
        {
            SetPath(state, true);
        }

        /// <summary>
        /// Sets the path on this element and all child elements.  Uses the state
        /// object to track the state of the drawing
        /// </summary>
        /// <param name="state">State of the drawing operation</param>
        /// <param name="doMeasurements">If true, calculate and apply text length adjustments.</param>
        private void SetPath(TextDrawingState state, bool doMeasurements)
        {
            TextDrawingState origState = null;
            bool alignOnBaseline = state.BaselinePath != null && (TextAnchor == SvgTextAnchor.Middle || TextAnchor == SvgTextAnchor.End);

            if (doMeasurements)
            {
                if (TextLength != SvgUnit.None)
                {
                    origState = state.Clone();
                }
                else if (alignOnBaseline)
                {
                    origState = state.Clone();
                    state.BaselinePath = null;
                }
            }

            foreach (var node in GetContentNodes())
            {
                SvgTextBase textNode = node as SvgTextBase;

                if (textNode == null)
                {
                    if (!string.IsNullOrEmpty(node.Content)) state.DrawString(PrepareText(node.Content));
                }
                else
                {
                    TextDrawingState newState = new TextDrawingState(state, textNode);

                    textNode.SetPath(newState);
                    state.NumChars += newState.NumChars;
                    state.Current = newState.Current;
                }
            }

            var path = state.GetPath() ?? new GraphicsPath();

            // Apply any text length adjustments
            if (doMeasurements)
            {
                if (TextLength != SvgUnit.None)
                {
                    var specLength = TextLength.ToDeviceValue(state.Renderer, UnitRenderingType.Horizontal, this);
                    var actLength = state.TextBounds.Width;
                    var diff = (actLength - specLength);
                    if (Math.Abs(diff) > 1.5)
                    {
                        if (LengthAdjust == SvgTextLengthAdjust.Spacing)
                        {
                            if (X.Count < 2)
                            {
                                origState.LetterSpacingAdjust = -1 * diff / (state.NumChars - origState.NumChars - 1);
                                SetPath(origState, false);
                                return;
                            }
                        }
                        else
                        {
                            using (var matrix = new Matrix())
                            {
                                matrix.Translate(-1 * state.TextBounds.X, 0, MatrixOrder.Append);
                                matrix.Scale(specLength / actLength, 1, MatrixOrder.Append);
                                matrix.Translate(state.TextBounds.X, 0, MatrixOrder.Append);
                                path.Transform(matrix);
                            }
                        }
                    }
                }
                else if (alignOnBaseline)
                {
                    var bounds = path.GetBounds();
                    if (TextAnchor == SvgTextAnchor.Middle)
                    {
                        origState.StartOffsetAdjust = -1 * bounds.Width / 2;
                    }
                    else
                    {
                        origState.StartOffsetAdjust = -1 * bounds.Width;
                    }
                    SetPath(origState, false);
                    return;
                }
            }


            _path = path;
            IsPathDirty = false;
        }

        private static readonly Regex MultipleSpaces = new Regex(@" {2,}", RegexOptions.Compiled);

        /// <summary>
        /// Prepare the text according to the whitespace handling rules and text transformations.  <see href="http://www.w3.org/TR/SVG/text.html">SVG Spec</see>.
        /// </summary>
        /// <param name="value">Text to be prepared</param>
        /// <returns>Prepared text</returns>
        protected string PrepareText(string value)
        {
            value = ApplyTransformation(value);
            if (SpaceHandling == XmlSpaceHandling.preserve)
            {
                return value.Replace('\t', ' ').Replace("\r\n", " ").Replace('\r', ' ').Replace('\n', ' ');
            }
            else
            {
                var convValue = MultipleSpaces.Replace(value.Replace("\r", "").Replace("\n", "").Replace('\t', ' '), " ");
                return convValue;
            }
        }

        private string ApplyTransformation(string value)
        {
            switch (TextTransformation)
            {
                case SvgTextTransformation.Capitalize:
                    return value.ToUpper();

                case SvgTextTransformation.Uppercase:
                    return value.ToUpper();

                case SvgTextTransformation.Lowercase:
                    return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value);
            }

            return value;
        }

        [SvgAttribute("onchange")]
        public event EventHandler<StringArg> Change;

        //change
        protected void OnChange(string newString, string sessionID)
        {
            RaiseChange(this, new StringArg { s = newString, SessionID = sessionID });
        }

        protected void RaiseChange(object sender, StringArg s)
        {
            var handler = Change;
            if (handler != null)
            {
                handler(sender, s);
            }
        }

        private class FontBoundable : ISvgBoundable
        {
            private IFontDefn _font;
            private float _width = 1;

            public FontBoundable(IFontDefn font)
            {
                _font = font;
            }
            public FontBoundable(IFontDefn font, float width)
            {
                _font = font;
                _width = width;
            }

            public PointF Location
            {
                get { return PointF.Empty; }
            }

            public SizeF Size
            {
                get { return new SizeF(_width, _font.Size); }
            }

            public RectangleF Bounds
            {
                get { return new RectangleF(Location, Size); }
            }
        }

        private class TextDrawingState
        {
            private float _xAnchor = float.MinValue;
            private IList<GraphicsPath> _anchoredPaths = new List<GraphicsPath>();
            private GraphicsPath _currPath = null;
            private GraphicsPath _finalPath = null;
            private float _authorPathLength = 0;

            public GraphicsPath BaselinePath { get; set; }
            public PointF Current { get; set; }
            public RectangleF TextBounds { get; set; }
            public SvgTextBase Element { get; set; }
            public float LetterSpacingAdjust { get; set; }
            public int NumChars { get; set; }
            public TextDrawingState Parent { get; set; }
            public ISvgRenderer Renderer { get; set; }
            public float StartOffsetAdjust { get; set; }

            private TextDrawingState() { }
            public TextDrawingState(ISvgRenderer renderer, SvgTextBase element)
            {
                Element = element;
                Renderer = renderer;
                Current = PointF.Empty;
                TextBounds = RectangleF.Empty;
                _xAnchor = 0;
                BaselinePath = element.GetBaselinePath(renderer);
                _authorPathLength = element.GetAuthorPathLength();
            }
            public TextDrawingState(TextDrawingState parent, SvgTextBase element)
            {
                Element = element;
                Renderer = parent.Renderer;
                Parent = parent;
                Current = parent.Current;
                TextBounds = parent.TextBounds;
                BaselinePath = element.GetBaselinePath(parent.Renderer) ?? parent.BaselinePath;
                var currPathLength = element.GetAuthorPathLength();
                _authorPathLength = currPathLength == 0 ? parent._authorPathLength : currPathLength;
            }

            public GraphicsPath GetPath()
            {
                FlushPath();
                return _finalPath;
            }

            public TextDrawingState Clone()
            {
                var result = new TextDrawingState();
                result._anchoredPaths = _anchoredPaths.ToList();
                result.BaselinePath = BaselinePath;
                result._xAnchor = _xAnchor;
                result.Current = Current;
                result.TextBounds = TextBounds;
                result.Element = Element;
                result.NumChars = NumChars;
                result.Parent = Parent;
                result.Renderer = Renderer;
                return result;
            }

            public void DrawString(string value)
            {
                // Get any defined anchors
                var xAnchors = GetValues(value.Length, e => e._x, UnitRenderingType.HorizontalOffset);
                var yAnchors = GetValues(value.Length, e => e._y, UnitRenderingType.VerticalOffset);
                using (var font = Element.GetFont(Renderer))
                {
                    var fontBaselineHeight = font.Ascent(Renderer);
                    PathStatistics pathStats = null;
                    var pathScale = 1.0;
                    if (BaselinePath != null)
                    {
                        pathStats = new PathStatistics(BaselinePath.PathData);
                        if (_authorPathLength > 0) pathScale = _authorPathLength / pathStats.TotalLength;
                    }

                    // Get all of the offsets (explicit and defined by spacing)
                    IList<float> xOffsets;
                    IList<float> yOffsets;
                    IList<float> rotations;
                    float baselineShift = 0.0f;

                    try
                    {
                        Renderer.SetBoundable(new FontBoundable(font, (float)(pathStats == null ? 1 : pathStats.TotalLength)));
                        xOffsets = GetValues(value.Length, e => e._dx, UnitRenderingType.Horizontal);
                        yOffsets = GetValues(value.Length, e => e._dy, UnitRenderingType.Vertical);
                        if (StartOffsetAdjust != 0.0f)
                        {
                            if (xOffsets.Count < 1)
                            {
                                xOffsets.Add(StartOffsetAdjust);
                            }
                            else
                            {
                                xOffsets[0] += StartOffsetAdjust;
                            }
                        }

                        if (Element.LetterSpacing.Value != 0.0f || Element.WordSpacing.Value != 0.0f || LetterSpacingAdjust != 0.0f)
                        {
                            var spacing = Element.LetterSpacing.ToDeviceValue(Renderer, UnitRenderingType.Horizontal, Element) + LetterSpacingAdjust;
                            var wordSpacing = Element.WordSpacing.ToDeviceValue(Renderer, UnitRenderingType.Horizontal, Element);
                            if (Parent == null && NumChars == 0 && xOffsets.Count < 1) xOffsets.Add(0);
                            for (int i = (Parent == null && NumChars == 0 ? 1 : 0); i < value.Length; i++)
                            {
                                if (i >= xOffsets.Count)
                                {
                                    xOffsets.Add(spacing + (char.IsWhiteSpace(value[i]) ? wordSpacing : 0));
                                }
                                else
                                {
                                    xOffsets[i] += spacing + (char.IsWhiteSpace(value[i]) ? wordSpacing : 0);
                                }
                            }
                        }

                        rotations = GetValues(value.Length, e => e._rotations);

                        // Calculate Y-offset due to baseline shift.  Don't inherit the value so that it is not accumulated multiple times.               
                        var baselineShiftText = Element.Attributes.GetAttribute<string>("baseline-shift");

                        switch (baselineShiftText)
                        {
                            case null:
                            case "":
                            case "baseline":
                            case "inherit":
                                // do nothing
                                break;
                            case "sub":
                                baselineShift = new SvgUnit(SvgUnitType.Ex, 1).ToDeviceValue(Renderer, UnitRenderingType.Vertical, Element);
                                break;
                            case "super":
                                baselineShift = -1 * new SvgUnit(SvgUnitType.Ex, 1).ToDeviceValue(Renderer, UnitRenderingType.Vertical, Element);
                                break;
                            default:
                                var shiftUnit = SvgUnitConverter.Parse(baselineShiftText);
                                baselineShift = -1 * shiftUnit.ToDeviceValue(Renderer, UnitRenderingType.Vertical, Element);
                                break;
                        }

                        if (baselineShift != 0.0f)
                        {
                            if (yOffsets.Any())
                            {
                                yOffsets[0] += baselineShift;
                            }
                            else
                            {
                                yOffsets.Add(baselineShift);
                            }
                        }
                    }
                    finally
                    {
                        Renderer.PopBoundable();
                    }

                    var xTextStart = Current.X;
                    // NOTE: Assuming a horizontal left-to-right font
                    // Render absolutely positioned items in the horizontal direction
                    var yPos = Current.Y;
                    for (int i = 0; i < xAnchors.Count - 1; i++)
                    {
                        FlushPath();
                        _xAnchor = xAnchors[i] + (xOffsets.Count > i ? xOffsets[i] : 0);
                        EnsurePath();
                        yPos = (yAnchors.Count > i ? yAnchors[i] : yPos) + (yOffsets.Count > i ? yOffsets[i] : 0);

                        xTextStart = xTextStart.Equals(Current.X) ? _xAnchor : xTextStart;
                        DrawStringOnCurrPath(value[i].ToString(), font, new PointF(_xAnchor, yPos),
                                             fontBaselineHeight, (rotations.Count > i ? rotations[i] : rotations.LastOrDefault()));
                    }

                    // Render any remaining characters
                    var renderChar = 0;
                    var xPos = Current.X;
                    if (xAnchors.Any())
                    {
                        FlushPath();
                        renderChar = xAnchors.Count - 1;
                        xPos = xAnchors.Last();
                        _xAnchor = xPos;
                    }
                    EnsurePath();


                    // Render individual characters as necessary
                    var lastIndividualChar = renderChar + Math.Max(Math.Max(Math.Max(Math.Max(xOffsets.Count, yOffsets.Count), yAnchors.Count), rotations.Count) - renderChar - 1, 0);
                    if (rotations.LastOrDefault() != 0.0f || pathStats != null) lastIndividualChar = value.Length;
                    if (lastIndividualChar > renderChar)
                    {
                        var charBounds = font.MeasureCharacters(Renderer, value.Substring(renderChar, Math.Min(lastIndividualChar + 1, value.Length) - renderChar));
                        PointF pathPoint;
                        float rotation;
                        float halfWidth;
                        for (int i = renderChar; i < lastIndividualChar; i++)
                        {
                            xPos += (float)pathScale * (xOffsets.Count > i ? xOffsets[i] : 0) + (charBounds[i - renderChar].X - (i == renderChar ? 0 : charBounds[i - renderChar - 1].X));
                            yPos = (yAnchors.Count > i ? yAnchors[i] : yPos) + (yOffsets.Count > i ? yOffsets[i] : 0);
                            if (pathStats == null)
                            {
                                xTextStart = xTextStart.Equals(Current.X) ? xPos : xTextStart;
                                DrawStringOnCurrPath(value[i].ToString(), font, new PointF(xPos, yPos),
                                                     fontBaselineHeight, (rotations.Count > i ? rotations[i] : rotations.LastOrDefault()));
                            }
                            else
                            {
                                xPos = Math.Max(xPos, 0);
                                halfWidth = charBounds[i - renderChar].Width / 2;
                                if (pathStats.OffsetOnPath(xPos + halfWidth))
                                {
                                    pathStats.LocationAngleAtOffset(xPos + halfWidth, out pathPoint, out rotation);
                                    pathPoint = new PointF((float)(pathPoint.X - halfWidth * Math.Cos(rotation * Math.PI / 180) - (float)pathScale * yPos * Math.Sin(rotation * Math.PI / 180)),
                                                           (float)(pathPoint.Y - halfWidth * Math.Sin(rotation * Math.PI / 180) + (float)pathScale * yPos * Math.Cos(rotation * Math.PI / 180)));
                                    xTextStart = xTextStart.Equals(Current.X) ? pathPoint.X : xTextStart;
                                    DrawStringOnCurrPath(value[i].ToString(), font, pathPoint, fontBaselineHeight, rotation);
                                }
                            }
                        }

                        // Add the kerning to the next character
                        if (lastIndividualChar < value.Length)
                        {
                            xPos += charBounds[charBounds.Count - 1].X - charBounds[charBounds.Count - 2].X;
                        }
                        else
                        {
                            xPos += charBounds.Last().Width;
                        }
                    }

                    // Render the string normally
                    if (lastIndividualChar < value.Length)
                    {
                        xPos += (xOffsets.Count > lastIndividualChar ? xOffsets[lastIndividualChar] : 0);
                        yPos = (yAnchors.Count > lastIndividualChar ? yAnchors[lastIndividualChar] : yPos) +
                                (yOffsets.Count > lastIndividualChar ? yOffsets[lastIndividualChar] : 0);
                        xTextStart = xTextStart.Equals(Current.X) ? xPos : xTextStart;
                        DrawStringOnCurrPath(value.Substring(lastIndividualChar), font, new PointF(xPos, yPos),
                                             fontBaselineHeight, rotations.LastOrDefault());
                        var bounds = font.MeasureString(Renderer, value.Substring(lastIndividualChar));
                        xPos += bounds.Width;
                    }


                    NumChars += value.Length;
                    // Undo any baseline shift.  This is not persisted, unlike normal vertical offsets.
                    Current = new PointF(xPos, yPos - baselineShift);
                    TextBounds = new RectangleF(xTextStart, 0, Current.X - xTextStart, 0);
                }
            }

            private void DrawStringOnCurrPath(string value, IFontDefn font, PointF location, float fontBaselineHeight, float rotation)
            {
                var drawPath = _currPath;
                if (rotation != 0.0f) drawPath = new GraphicsPath();
                font.AddStringToPath(Renderer, drawPath, value, new PointF(location.X, location.Y - fontBaselineHeight));
                if (rotation != 0.0f && drawPath.PointCount > 0)
                {
                    using (var matrix = new Matrix())
                    {
                        matrix.Translate(-1 * location.X, -1 * location.Y, MatrixOrder.Append);
                        matrix.Rotate(rotation, MatrixOrder.Append);
                        matrix.Translate(location.X, location.Y, MatrixOrder.Append);
                        drawPath.Transform(matrix);
                        _currPath.AddPath(drawPath, false);
                    }
                }

            }

            private void EnsurePath()
            {
                if (_currPath == null)
                {
                    _currPath = new GraphicsPath();
                    _currPath.StartFigure();

                    var currState = this;
                    while (currState != null && currState._xAnchor <= float.MinValue)
                    {
                        currState = currState.Parent;
                    }
                    currState._anchoredPaths.Add(_currPath);
                }
            }

            private void FlushPath()
            {
                if (_currPath != null)
                {
                    _currPath.CloseFigure();

                    // Abort on empty paths (e.g. rendering a space)
                    if (_currPath.PointCount < 1)
                    {
                        _anchoredPaths.Clear();
                        _xAnchor = float.MinValue;
                        _currPath = null;
                        return;
                    }

                    if (_xAnchor > float.MinValue)
                    {
                        float minX = float.MaxValue;
                        float maxX = float.MinValue;
                        RectangleF bounds;
                        foreach (var path in _anchoredPaths)
                        {
                            bounds = path.GetBounds();
                            if (bounds.Left < minX) minX = bounds.Left;
                            if (bounds.Right > maxX) maxX = bounds.Right;
                        }

                        var xOffset = 0f; //_xAnchor - minX;
                        switch (Element.TextAnchor)
                        {
                            case SvgTextAnchor.Middle:
                                if (_anchoredPaths.Count() == 1) xOffset -= TextBounds.Width / 2;
                                else xOffset -= (maxX - minX) / 2;
                                break;
                            case SvgTextAnchor.End:
                                if (_anchoredPaths.Count() == 1) xOffset -= TextBounds.Width;
                                else xOffset -= (maxX - minX);
                                break;
                        }

                        if (xOffset != 0)
                        {
                            using (var matrix = new Matrix())
                            {
                                matrix.Translate(xOffset, 0);
                                foreach (var path in _anchoredPaths)
                                {
                                    path.Transform(matrix);
                                }
                            }
                        }

                        _anchoredPaths.Clear();
                        _xAnchor = float.MinValue;

                    }

                    if (_finalPath == null)
                    {
                        _finalPath = _currPath;
                    }
                    else
                    {
                        _finalPath.AddPath(_currPath, false);
                    }

                    _currPath = null;
                }
            }

            private IList<float> GetValues(int maxCount, Func<SvgTextBase, IEnumerable<float>> listGetter)
            {
                var currState = this;
                int charCount = 0;
                var results = new List<float>();
                int resultCount = 0;

                while (currState != null)
                {
                    charCount += currState.NumChars;
                    results.AddRange(listGetter.Invoke(currState.Element).Skip(charCount).Take(maxCount));
                    if (results.Count > resultCount)
                    {
                        maxCount -= results.Count - resultCount;
                        charCount += results.Count - resultCount;
                        resultCount = results.Count;
                    }

                    if (maxCount < 1) return results;

                    currState = currState.Parent;
                }

                return results;
            }
            private IList<float> GetValues(int maxCount, Func<SvgTextBase, IEnumerable<SvgUnit>> listGetter, UnitRenderingType renderingType)
            {
                var currState = this;
                int charCount = 0;
                var results = new List<float>();
                int resultCount = 0;

                while (currState != null)
                {
                    charCount += currState.NumChars;
                    results.AddRange(listGetter.Invoke(currState.Element).Skip(charCount).Take(maxCount).Select(p => p.ToDeviceValue(currState.Renderer, renderingType, currState.Element)));
                    if (results.Count > resultCount)
                    {
                        maxCount -= results.Count - resultCount;
                        charCount += results.Count - resultCount;
                        resultCount = results.Count;
                    }

                    if (maxCount < 1) return results;

                    currState = currState.Parent;
                }

                return results;
            }
        }

        /// <summary>Empty text elements are not legal - only write this element if it has children.</summary>
        public override bool ShouldWriteElement()
        {
            return (HasChildren() || Nodes.Count > 0);
        }
    }
}