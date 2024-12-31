// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace AntdUI.Svg.FilterEffects
{
    public class ImageBuffer : IDictionary<string, Bitmap?>
    {
        private const string BufferKey = "__!!BUFFER";

        private Dictionary<string, Bitmap?> _images;
        private RectangleF _bounds;
        private ISvgRenderer _renderer;
        private Action<ISvgRenderer> _renderMethod;
        private float _inflate;

        public Matrix Transform { get; set; }

        public Bitmap? Buffer => _images[BufferKey];
        public int Count => _images.Count;
        public Bitmap? this[string key]
        {
            get => ProcessResult(key, _images[ProcessKey(key)]);
            set
            {
                if (value == null) return;
                _images[ProcessKey(key)] = value;
                if (key != null) _images[BufferKey] = value;
            }
        }

        public ImageBuffer(RectangleF bounds, float inflate, ISvgRenderer renderer, Matrix transform, Action<ISvgRenderer> renderMethod)
        {
            _bounds = bounds;
            _inflate = inflate;
            _renderer = renderer;
            Transform = transform;
            _renderMethod = renderMethod;
            _images = new Dictionary<string, Bitmap?>();
            _images[SvgFilterPrimitive.BackgroundAlpha] = null;
            _images[SvgFilterPrimitive.BackgroundImage] = null;
            _images[SvgFilterPrimitive.FillPaint] = null;
            _images[SvgFilterPrimitive.SourceAlpha] = null;
            _images[SvgFilterPrimitive.SourceGraphic] = null;
            _images[SvgFilterPrimitive.StrokePaint] = null;
        }

        public void Add(string key, Bitmap? value) => _images.Add(ProcessKey(key), value);
        public bool ContainsKey(string key) => _images.ContainsKey(ProcessKey(key));
        public void Clear() => _images.Clear();
        public IEnumerator<KeyValuePair<string, Bitmap?>> GetEnumerator() => _images.GetEnumerator();
        public bool Remove(string key)
        {
            switch (key)
            {
                case SvgFilterPrimitive.BackgroundAlpha:
                case SvgFilterPrimitive.BackgroundImage:
                case SvgFilterPrimitive.FillPaint:
                case SvgFilterPrimitive.SourceAlpha:
                case SvgFilterPrimitive.SourceGraphic:
                case SvgFilterPrimitive.StrokePaint:
                    return false;
                default:
                    return _images.Remove(ProcessKey(key));
            }
        }

        public bool TryGetValue(string key, out Bitmap? value)
        {
            if (_images.TryGetValue(ProcessKey(key), out value))
            {
                value = ProcessResult(key, value);
                return true;
            }
            else return false;
        }

        private Bitmap? ProcessResult(string key, Bitmap? curr)
        {
            if (curr == null)
            {
                switch (key)
                {
                    case SvgFilterPrimitive.BackgroundAlpha:
                    case SvgFilterPrimitive.BackgroundImage:
                    case SvgFilterPrimitive.FillPaint:
                    case SvgFilterPrimitive.StrokePaint:
                        // Do nothing
                        return null;
                    case SvgFilterPrimitive.SourceAlpha:
                        _images[key] = CreateSourceAlpha();
                        return _images[key];
                    case SvgFilterPrimitive.SourceGraphic:
                        _images[key] = CreateSourceGraphic();
                        return _images[key];
                }
            }
            return curr;
        }

        private string ProcessKey(string key)
        {
            if (string.IsNullOrEmpty(key)) return _images.ContainsKey(BufferKey) ? BufferKey : SvgFilterPrimitive.SourceGraphic;
            return key;
        }

        private Bitmap CreateSourceGraphic()
        {
            var graphic = new Bitmap((int)(_bounds.Width + 2 * _inflate * _bounds.Width + _bounds.X),
                                     (int)(_bounds.Height + 2 * _inflate * _bounds.Height + _bounds.Y));
            using (var renderer = SvgRenderer.FromImage(graphic))
            {
                renderer.SetBoundable(_renderer.GetBoundable());
                var transform = new Matrix();
                transform.Translate(_bounds.Width * _inflate, _bounds.Height * _inflate);
                renderer.Transform = transform;
                //renderer.Transform = _renderer.Transform;
                //renderer.Clip = _renderer.Clip;
                _renderMethod.Invoke(renderer);
            }
            return graphic;
        }

        private Bitmap CreateSourceAlpha()
        {
            var graphic = new Bitmap((int)(_bounds.Width + 2 * _inflate * _bounds.Width + _bounds.X), (int)(_bounds.Height + 2 * _inflate * _bounds.Height + _bounds.Y));
            using (var renderer = SvgRenderer.FromImage(graphic))
            using (var transform = new Matrix())
            {
                renderer.SetBoundable(_renderer.GetBoundable());
                transform.Translate(_bounds.Width * _inflate, _bounds.Height * _inflate);
                renderer.Transform = transform;
                //renderer.Transform = _renderer.Transform;
                //renderer.Clip = _renderer.Clip;
                _renderMethod.Invoke(renderer);
            }
            return graphic;
        }


        bool ICollection<KeyValuePair<string, Bitmap?>>.IsReadOnly => false;

        ICollection<string> IDictionary<string, Bitmap?>.Keys => _images.Keys;
        ICollection<Bitmap?> IDictionary<string, Bitmap?>.Values => _images.Values;

        void ICollection<KeyValuePair<string, Bitmap?>>.Add(KeyValuePair<string, Bitmap?> item) => _images.Add(item.Key, item.Value);
        bool ICollection<KeyValuePair<string, Bitmap?>>.Contains(KeyValuePair<string, Bitmap?> item) => ((IDictionary<string, Bitmap?>)_images).Contains(item);
        void ICollection<KeyValuePair<string, Bitmap?>>.CopyTo(KeyValuePair<string, Bitmap?>[] array, int arrayIndex) => ((IDictionary<string, Bitmap?>)_images).CopyTo(array, arrayIndex);
        bool ICollection<KeyValuePair<string, Bitmap?>>.Remove(KeyValuePair<string, Bitmap?> item) => _images.Remove(item.Key);
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => _images.GetEnumerator();
    }
}