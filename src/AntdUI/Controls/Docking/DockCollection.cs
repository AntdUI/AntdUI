// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.Collections;
using System.Collections.Generic;

namespace AntdUI
{
    /// <summary>
    /// Ordered, event-aware collection of <see cref="IDockContent"/>.
    /// </summary>
    public class DockCollection : IEnumerable<IDockContent>
    {
        readonly List<IDockContent> inner = new List<IDockContent>();

        public int Count { get { return inner.Count; } }

        public IDockContent this[int index] { get { return inner[index]; } }

        public void Add(IDockContent item)
        {
            if (item == null) return;
            inner.Add(item);
            Added?.Invoke(this, new DockContentEventArgs(item));
        }

        public void Insert(int index, IDockContent item)
        {
            if (item == null) return;
            if (index < 0) index = 0;
            if (index > inner.Count) index = inner.Count;
            inner.Insert(index, item);
            Added?.Invoke(this, new DockContentEventArgs(item));
        }

        public bool Remove(IDockContent item)
        {
            if (item == null) return false;
            int idx = inner.IndexOf(item);
            if (idx < 0) return false;
            inner.RemoveAt(idx);
            Removed?.Invoke(this, new DockContentEventArgs(item));
            return true;
        }

        public int IndexOf(IDockContent item) { return inner.IndexOf(item); }

        public bool Contains(IDockContent item) { return inner.Contains(item); }

        public void Clear()
        {
            if (inner.Count == 0) return;
            var copy = inner.ToArray();
            inner.Clear();
            for (int i = 0; i < copy.Length; i++)
            {
                if (Removed != null) Removed.Invoke(this, new DockContentEventArgs(copy[i]));
            }
        }

        public IEnumerator<IDockContent> GetEnumerator() { return inner.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return inner.GetEnumerator(); }

        public event DockContentEventHandler? Added;
        public event DockContentEventHandler? Removed;
    }
}
