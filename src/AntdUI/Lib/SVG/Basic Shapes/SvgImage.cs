// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text;

namespace AntdUI.Svg
{
    /// <summary>
    /// 表示 &lt;image&gt; 元素。
    /// 仅支持内嵌 data: URI（base64 / URL 编码）的 svg、png、jpg 等图片，以及本地文件路径。
    /// </summary>
    public class SvgImage : SvgVisualElement
    {
        public override string ClassName => "image";

        private string? _href;
        private bool _loaded;
        private Image? _bitmap;
        private SvgDocument? _vector;

        /// <summary>
        /// Gets or sets the position where the left point of the image should start.
        /// </summary>
        [SvgAttribute("x")]
        public virtual SvgUnit X
        {
            get { return Attributes.GetAttribute<SvgUnit>("x"); }
            set { Attributes["x"] = value; IsPathDirty = true; }
        }

        /// <summary>
        /// Gets or sets the position where the top point of the image should start.
        /// </summary>
        [SvgAttribute("y")]
        public virtual SvgUnit Y
        {
            get { return Attributes.GetAttribute<SvgUnit>("y"); }
            set { Attributes["y"] = value; IsPathDirty = true; }
        }

        /// <summary>
        /// Gets or sets the width of the image.
        /// </summary>
        [SvgAttribute("width")]
        public virtual SvgUnit Width
        {
            get { return Attributes.GetAttribute<SvgUnit>("width"); }
            set { Attributes["width"] = value; IsPathDirty = true; }
        }

        /// <summary>
        /// Gets or sets the height of the image.
        /// </summary>
        [SvgAttribute("height")]
        public virtual SvgUnit Height
        {
            get { return Attributes.GetAttribute<SvgUnit>("height"); }
            set { Attributes["height"] = value; IsPathDirty = true; }
        }

        /// <summary>
        /// Gets or sets the location of the referenced image (data: URI 或文件路径).
        /// </summary>
        [SvgAttribute("href", SvgAttributeAttribute.XLinkNamespace)]
        public virtual string? Href
        {
            get { return _href; }
            set
            {
                if (_href == value) return;
                _href = value;
                Reset();
            }
        }

        /// <summary>
        /// Gets or sets the aspect of the image viewport.
        /// </summary>
        [SvgAttribute("preserveAspectRatio")]
        public virtual SvgAspectRatio? AspectRatio
        {
            get { return Attributes.GetAttribute<SvgAspectRatio>("preserveAspectRatio"); }
            set { Attributes["preserveAspectRatio"] = value; }
        }

        /// <summary>
        /// 图片是否已解析出可绘制的内容。
        /// </summary>
        public bool IsLoaded
        {
            get
            {
                EnsureLoaded();
                return _bitmap != null || _vector != null;
            }
        }

        protected override bool RequiresSmoothRendering => true;

        /// <summary>
        /// Gets the <see cref="GraphicsPath"/> for this element.
        /// </summary>
        public override GraphicsPath Path(ISvgRenderer renderer)
        {
            var path = new GraphicsPath();
            var bounds = Bounds;
            if (bounds.Width > 0f && bounds.Height > 0f) path.AddRectangle(bounds);
            return path;
        }

        /// <summary>
        /// Gets the bounds of the element.
        /// </summary>
        public override RectangleF Bounds
        {
            get
            {
                var x = X.ToDeviceValue(null, UnitRenderingType.Horizontal, this);
                var y = Y.ToDeviceValue(null, UnitRenderingType.Vertical, this);
                var w = Width.ToDeviceValue(null, UnitRenderingType.Horizontal, this);
                var h = Height.ToDeviceValue(null, UnitRenderingType.Vertical, this);
                return TransformedBounds(new RectangleF(x, y, w, h));
            }
        }

        protected override void Render(ISvgRenderer renderer)
        {
            if (!Visible || !Displayable) return;

            EnsureLoaded();
            if (_bitmap == null && _vector == null) return;

            if (!PushTransforms(renderer)) return;
            try
            {
                SetClip(renderer);

                var x = X.ToDeviceValue(renderer, UnitRenderingType.Horizontal, this);
                var y = Y.ToDeviceValue(renderer, UnitRenderingType.Vertical, this);
                var w = Width.ToDeviceValue(renderer, UnitRenderingType.Horizontal, this);
                var h = Height.ToDeviceValue(renderer, UnitRenderingType.Vertical, this);
                if (w <= 0f || h <= 0f) return;

                var box = new RectangleF(x, y, w, h);
                if (_bitmap != null) RenderBitmap(renderer, box);
                else RenderVector(renderer, box);
            }
            catch { }
            finally
            {
                ResetClip(renderer);
                PopTransforms(renderer);
            }
        }

        #region 绘制

        private void RenderBitmap(ISvgRenderer renderer, RectangleF box)
        {
            var image = _bitmap;
            if (image == null) return;

            var opacity = FixOpacityValue(Opacity);
            if (opacity <= 0f) return;

            var destination = GetDestination(box, image.Width, image.Height);
            var source = new RectangleF(0f, 0f, image.Width, image.Height);

            Region? previous = null;
            if (IsSlice(box, image.Width, image.Height))
            {
                // slice 会溢出视口，按规范裁剪到 image 元素的范围
                previous = renderer.GetClip();
                renderer.SetClip(new Region(box), CombineMode.Intersect);
            }

            try
            {
                if (opacity >= 1f) renderer.DrawImage(image, destination, source, GraphicsUnit.Pixel);
                else renderer.DrawImage(image, destination, source, GraphicsUnit.Pixel, opacity);
            }
            finally
            {
                if (previous != null) renderer.SetClip(previous);
            }
        }

        private void RenderVector(ISvgRenderer renderer, RectangleF box)
        {
            var document = _vector;
            if (document == null) return;

            var opacity = FixOpacityValue(Opacity);
            if (opacity <= 0f) return;

            if (opacity >= 1f)
            {
                RenderDocument(renderer, document, box);
                return;
            }

            // 半透明：先离屏栅格化再整体叠加
            var width = (int)Math.Ceiling(box.Width);
            var height = (int)Math.Ceiling(box.Height);
            if (width <= 0 || height <= 0) return;

            using (var canvas = new Bitmap(width, height))
            {
                using (var canvasRenderer = SvgRenderer.FromImage(canvas))
                {
                    RenderDocument(canvasRenderer, document, new RectangleF(0f, 0f, width, height));
                }
                renderer.DrawImage(canvas, box, new RectangleF(0f, 0f, width, height), GraphicsUnit.Pixel, opacity);
            }
        }

        private static void RenderDocument(ISvgRenderer renderer, SvgDocument document, RectangleF box)
        {
            // 百分比宽高在无父级 boundable 时无法计算，直接给出目标尺寸
            document.Width = box.Width;
            document.Height = box.Height;

            renderer.TranslateTransform(box.X, box.Y, MatrixOrder.Prepend);
            renderer.SetBoundable(new GenericBoundable(0f, 0f, box.Width, box.Height));
            try
            {
                document.RenderElement(renderer);
            }
            finally
            {
                renderer.PopBoundable();
            }
        }

        private bool IsSlice(RectangleF box, float sourceWidth, float sourceHeight)
        {
            var aspect = AspectRatio;
            return aspect != null && aspect.Slice && aspect.Align != SvgPreserveAspectRatio.none
                && sourceWidth > 0f && sourceHeight > 0f && !IsSameRatio(box, sourceWidth, sourceHeight);
        }

        private static bool IsSameRatio(RectangleF box, float sourceWidth, float sourceHeight)
        {
            return Math.Abs(box.Width * sourceHeight - box.Height * sourceWidth) < float.Epsilon;
        }

        /// <summary>
        /// 计算 preserveAspectRatio 之后的绘制区域。
        /// </summary>
        private RectangleF GetDestination(RectangleF box, float sourceWidth, float sourceHeight)
        {
            var aspect = AspectRatio;
            var align = aspect == null ? SvgPreserveAspectRatio.xMidYMid : aspect.Align;
            if (align == SvgPreserveAspectRatio.none || sourceWidth <= 0f || sourceHeight <= 0f) return box;

            var scaleX = box.Width / sourceWidth;
            var scaleY = box.Height / sourceHeight;
            var scale = (aspect != null && aspect.Slice) ? Math.Max(scaleX, scaleY) : Math.Min(scaleX, scaleY);

            var width = sourceWidth * scale;
            var height = sourceHeight * scale;
            var x = box.X;
            var y = box.Y;

            switch (align)
            {
                case SvgPreserveAspectRatio.xMidYMin:
                case SvgPreserveAspectRatio.xMidYMid:
                case SvgPreserveAspectRatio.xMidYMax:
                    x += (box.Width - width) / 2f;
                    break;
                case SvgPreserveAspectRatio.xMaxYMin:
                case SvgPreserveAspectRatio.xMaxYMid:
                case SvgPreserveAspectRatio.xMaxYMax:
                    x += box.Width - width;
                    break;
            }

            switch (align)
            {
                case SvgPreserveAspectRatio.xMinYMid:
                case SvgPreserveAspectRatio.xMidYMid:
                case SvgPreserveAspectRatio.xMaxYMid:
                    y += (box.Height - height) / 2f;
                    break;
                case SvgPreserveAspectRatio.xMinYMax:
                case SvgPreserveAspectRatio.xMidYMax:
                case SvgPreserveAspectRatio.xMaxYMax:
                    y += box.Height - height;
                    break;
            }

            return new RectangleF(x, y, width, height);
        }

        #endregion

        #region 解析

        private void Reset()
        {
            _loaded = false;
            _vector = null;
            if (_bitmap != null)
            {
                _bitmap.Dispose();
                _bitmap = null;
            }
        }

        private void EnsureLoaded()
        {
            if (_loaded) return;
            _loaded = true;

            var href = _href;
            if (string.IsNullOrEmpty(href)) return;

            try
            {
                var source = href.Trim();
                string? text = null;
                byte[]? data = null;
                var mime = string.Empty;

                var comma = source.IndexOf(',');
                if (comma > 5 && source.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
                {
                    var meta = source.Substring(5, comma - 5);
                    var payload = source.Substring(comma + 1);
                    var parameters = meta.Split(';');
                    if (parameters.Length > 0) mime = parameters[0].Trim().ToLowerInvariant();

                    var isBase64 = false;
                    for (var i = 1; i < parameters.Length; i++)
                    {
                        if (parameters[i].Trim().Equals("base64", StringComparison.OrdinalIgnoreCase))
                        {
                            isBase64 = true;
                            break;
                        }
                    }

                    if (isBase64) data = FromBase64(payload);
                    else text = Unescape(payload);
                }
                else
                {
                    var path = ResolvePath(source);
                    if (path == null) return;
                    data = File.ReadAllBytes(path);
                }

                if (text != null)
                {
                    _vector = ParseVector(text);
                    return;
                }
                if (data == null || data.Length == 0) return;

                if (mime.Contains("svg") || LooksLikeSvg(data)) _vector = ParseVector(Encoding.UTF8.GetString(data));
                else _bitmap = ParseBitmap(data);
            }
            catch { }
        }

        private static byte[]? FromBase64(string payload)
        {
            try
            {
                return Convert.FromBase64String(payload.Trim());
            }
            catch { }
            try
            {
                return Convert.FromBase64String(Unescape(payload).Trim());
            }
            catch { }
            return null;
        }

        private static string Unescape(string value)
        {
            try
            {
                return value.IndexOf('%') > -1 ? Uri.UnescapeDataString(value) : value;
            }
            catch
            {
                return value;
            }
        }

        private static bool LooksLikeSvg(byte[] data)
        {
            var start = 0;
            // 跳过 UTF8 BOM
            if (data.Length > 2 && data[0] == 0xEF && data[1] == 0xBB && data[2] == 0xBF) start = 3;
            for (var i = start; i < data.Length && i < 512; i++)
            {
                var c = (char)data[i];
                if (char.IsWhiteSpace(c)) continue;
                return c == '<';
            }
            return false;
        }

        private static SvgDocument? ParseVector(string? xml)
        {
            if (string.IsNullOrEmpty(xml)) return null;
            var svg = xml!.Trim();
            // 去掉 XML 声明，SvgDocument 要求根节点为 <svg>
            if (svg.StartsWith("<?xml", StringComparison.OrdinalIgnoreCase))
            {
                var end = svg.IndexOf("?>", StringComparison.Ordinal);
                if (end > 0) svg = svg.Substring(end + 2).TrimStart();
            }
            if (svg.Length == 0) return null;
            return SvgDocument.FromSvg<SvgDocument>(svg);
        }

        private static Image? ParseBitmap(byte[] data)
        {
            try
            {
                using (var stream = new MemoryStream(data))
                using (var image = Image.FromStream(stream))
                {
                    // GDI+ 要求流保持打开，这里复制一份
                    return new Bitmap(image);
                }
            }
            catch
            {
                return null;
            }
        }

        private string? ResolvePath(string source)
        {
            try
            {
                if (source.IndexOf(':') > 1)
                {
                    // 绝对 URI，仅支持本地文件
                    if (!Uri.TryCreate(source, UriKind.Absolute, out var uri) || uri == null || !uri.IsFile) return null;
                    return File.Exists(uri.LocalPath) ? uri.LocalPath : null;
                }

                var baseUri = OwnerDocument?.BaseUri;
                if (baseUri != null && baseUri.IsFile)
                {
                    var directory = System.IO.Path.GetDirectoryName(baseUri.LocalPath);
                    if (!string.IsNullOrEmpty(directory))
                    {
                        var full = System.IO.Path.GetFullPath(System.IO.Path.Combine(directory!, source));
                        if (File.Exists(full)) return full;
                    }
                }

                return File.Exists(source) ? source : null;
            }
            catch
            {
                return null;
            }
        }

        #endregion
    }
}
