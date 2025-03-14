// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System.Drawing;
using System.Linq;

namespace AntdUI.Svg
{
    /// <summary>
    /// A wrapper for a paint server which isn't defined currently in the parse process, but
    /// should be defined by the time the image needs to render.
    /// </summary>
    public class SvgDeferredPaintServer : SvgPaintServer
    {
        private bool _serverLoaded = false;
        private SvgPaintServer _concreteServer;

        public SvgDocument Document { get; set; }
        public string DeferredId { get; set; }

        public SvgDeferredPaintServer() { }
        public SvgDeferredPaintServer(SvgDocument document, string id)
        {
            Document = document;
            DeferredId = id;
        }

        public void EnsureServer(SvgElement styleOwner)
        {
            if (!_serverLoaded)
            {
                if (DeferredId == "currentColor" && styleOwner != null)
                {
                    var colorElement = (from e in styleOwner.ParentsAndSelf.OfType<SvgElement>()
                                        where e.Color != SvgPaintServer.None && e.Color != SvgColourServer.NotSet &&
                                              e.Color != SvgColourServer.Inherit && e.Color != SvgColourServer.None
                                        select e).FirstOrDefault();
                    _concreteServer = (colorElement == null ? SvgColourServer.NotSet : colorElement.Color);
                }
                else
                {
                    _concreteServer = Document.IdManager.GetElementById(DeferredId) as SvgPaintServer;
                }
                _serverLoaded = true;
            }
        }

        public override Brush GetBrush(SvgVisualElement styleOwner, ISvgRenderer renderer, float opacity, bool forStroke = false)
        {
            EnsureServer(styleOwner);
            return _concreteServer.GetBrush(styleOwner, renderer, opacity, forStroke);
        }

        public override bool Equals(object? obj)
        {
            var other = obj as SvgDeferredPaintServer;
            if (other == null)
                return false;

            return Document == other.Document && DeferredId == other.DeferredId;
        }

        public override int GetHashCode()
        {
            if (Document == null || DeferredId == null) return 0;
            return Document.GetHashCode() ^ DeferredId.GetHashCode();
        }

        public override string ToString()
        {
            if (DeferredId == "currentColor")
            {
                return DeferredId;
            }
            else
            {
                return string.Format("url({0})", DeferredId);
            }
        }

        public static T TryGet<T>(SvgPaintServer server, SvgElement parent) where T : SvgPaintServer
        {
            var deferred = server as SvgDeferredPaintServer;
            if (deferred == null)
            {
                return server as T;
            }
            else
            {
                deferred.EnsureServer(parent);
                return deferred._concreteServer as T;
            }
        }
    }
}