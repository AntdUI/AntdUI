// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AntdUI.Svg
{
    /// <summary>
    /// A wrapper for a paint server has a fallback if the primary server doesn't work.
    /// </summary>
    public class SvgFallbackPaintServer : SvgPaintServer
    {
        private IEnumerable<SvgPaintServer> _fallbacks;
        private SvgPaintServer _primary;

        public SvgFallbackPaintServer() : base() { }
        public SvgFallbackPaintServer(SvgPaintServer primary, IEnumerable<SvgPaintServer> fallbacks) : this()
        {
            _fallbacks = fallbacks;
            _primary = primary;
        }

        public override Brush GetBrush(SvgVisualElement styleOwner, ISvgRenderer renderer, float opacity, bool forStroke = false)
        {
            try
            {
                _primary.GetCallback = () => _fallbacks.FirstOrDefault();
                return _primary.GetBrush(styleOwner, renderer, opacity, forStroke);
            }
            finally
            {
                _primary.GetCallback = null;
            }
        }

        public override SvgElement DeepCopy()
        {
            return base.DeepCopy<SvgFallbackPaintServer>();
        }
        public override SvgElement DeepCopy<T>()
        {
            var newObj = base.DeepCopy<T>() as SvgFallbackPaintServer;
            newObj._fallbacks = this._fallbacks;
            newObj._primary = this._primary;
            return newObj;
        }
    }
}
