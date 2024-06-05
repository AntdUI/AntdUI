// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System;
using System.Drawing.Drawing2D;

namespace AntdUI.Svg
{
    /// <summary>
    /// The <see cref="SvgText"/> element defines a graphics element consisting of text.
    /// </summary>
    public class SvgTextPath : SvgTextBase
    {
        public override string ClassName { get => "textPath"; }

        private Uri _referencedPath;

        public override SvgUnitCollection Dx
        {
            get { return null; }
            set { /* do nothing */ }
        }

        [SvgAttribute("startOffset")]
        public virtual SvgUnit StartOffset
        {
            get { return (_dx.Count < 1 ? SvgUnit.None : _dx[0]); }
            set
            {
                if (_dx.Count < 1)
                {
                    _dx.Add(value);
                }
                else
                {
                    _dx[0] = value;
                }
            }
        }

        [SvgAttribute("method")]
        public virtual SvgTextPathMethod Method
        {
            get { return (Attributes["method"] == null ? SvgTextPathMethod.Align : (SvgTextPathMethod)Attributes["method"]); }
            set { Attributes["method"] = value; }
        }

        [SvgAttribute("spacing")]
        public virtual SvgTextPathSpacing Spacing
        {
            get { return (Attributes["spacing"] == null ? SvgTextPathSpacing.Exact : (SvgTextPathSpacing)Attributes["spacing"]); }
            set { Attributes["spacing"] = value; }
        }

        [SvgAttribute("href", SvgAttributeAttribute.XLinkNamespace)]
        public virtual Uri ReferencedPath
        {
            get { return _referencedPath; }
            set { _referencedPath = value; }
        }

        protected override GraphicsPath GetBaselinePath(ISvgRenderer renderer)
        {
            var path = OwnerDocument.IdManager.GetElementById(ReferencedPath) as SvgVisualElement;
            if (path == null) return null;
            var pathData = (GraphicsPath)path.Path(renderer).Clone();
            if (path.Transforms.Count > 0)
            {
                Matrix transformMatrix = new Matrix(1, 0, 0, 1, 0, 0);

                foreach (var transformation in path.Transforms)
                {
                    transformMatrix.Multiply(transformation.Matrix(0, 0));
                }

                pathData.Transform(transformMatrix);
            }
            return pathData;
        }
        protected override float GetAuthorPathLength()
        {
            var path = OwnerDocument.IdManager.GetElementById(ReferencedPath) as SvgPath;
            if (path == null) return 0;
            return path.PathLength;
        }
    }
}