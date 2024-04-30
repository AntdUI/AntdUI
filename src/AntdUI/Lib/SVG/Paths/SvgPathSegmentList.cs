// THIS FILE IS PART OF SVG PROJECT
// THE SVG PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MS-PL License.
// COPYRIGHT (C) svg-net. ALL RIGHTS RESERVED.
// GITHUB: https://github.com/svg-net/SVG

using System.Collections.Generic;

namespace AntdUI.Svg.Pathing
{
    public sealed class SvgPathSegmentList : IList<SvgPathSegment>
    {
        internal SvgPath _owner;
        private List<SvgPathSegment> _segments;

        public SvgPathSegmentList()
        {
            _segments = new List<SvgPathSegment>();
        }

        public SvgPathSegment Last
        {
            get { return _segments[_segments.Count - 1]; }
        }

        public int IndexOf(SvgPathSegment item)
        {
            return _segments.IndexOf(item);
        }

        public void Insert(int index, SvgPathSegment item)
        {
            _segments.Insert(index, item);
            if (_owner != null)
            {
                _owner.OnPathUpdated();
            }
        }

        public void RemoveAt(int index)
        {
            _segments.RemoveAt(index);
            if (_owner != null)
            {
                _owner.OnPathUpdated();
            }
        }

        public SvgPathSegment this[int index]
        {
            get { return _segments[index]; }
            set { _segments[index] = value; _owner.OnPathUpdated(); }
        }

        public void Add(SvgPathSegment item)
        {
            _segments.Add(item);
            if (_owner != null)
            {
                _owner.OnPathUpdated();
            }
        }

        public void Clear()
        {
            _segments.Clear();
        }

        public bool Contains(SvgPathSegment item)
        {
            return _segments.Contains(item);
        }

        public void CopyTo(SvgPathSegment[] array, int arrayIndex)
        {
            _segments.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _segments.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(SvgPathSegment item)
        {
            bool removed = _segments.Remove(item);

            if (removed)
            {
                if (_owner != null)
                {
                    _owner.OnPathUpdated();
                }
            }

            return removed;
        }

        public IEnumerator<SvgPathSegment> GetEnumerator()
        {
            return _segments.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _segments.GetEnumerator();
        }
    }
}