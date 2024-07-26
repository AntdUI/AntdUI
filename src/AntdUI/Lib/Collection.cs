// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
// THE AntdUI PROJECT IS AN WINFORM LIBRARY LICENSED UNDER THE Apache-2.0 License.
// LICENSED UNDER THE Apache License, VERSION 2.0 (THE "License")
// YOU MAY NOT USE THIS FILE EXCEPT IN COMPLIANCE WITH THE License.
// YOU MAY OBTAIN A COPY OF THE LICENSE AT
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// UNLESS REQUIRED BY APPLICABLE LAW OR AGREED TO IN WRITING, SOFTWARE
// DISTRIBUTED UNDER THE LICENSE IS DISTRIBUTED ON AN "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED.
// SEE THE LICENSE FOR THE SPECIFIC LANGUAGE GOVERNING PERMISSIONS AND
// LIMITATIONS UNDER THE License.
// GITEE: https://gitee.com/antdui/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace AntdUI
{
    public class iCollection<T> : IList<T>, IList
    {
        #region 刷新UI

        internal Action<bool>? action;
        internal void PropertyChanged(T value)
        {
            if (value is NotifyProperty notify)
            {
                notify.PropertyChanged += (a, b) =>
                {
                    action?.Invoke(false);
                };
            }
        }

        #endregion

        List<T> list;
        public iCollection() { list = new List<T>(); }
        public iCollection(int capacity) { list = new List<T>(capacity); }
        public iCollection(IEnumerable<T> collection) { list = new List<T>(collection); }

        public T this[int index]
        {
            get => list[index];
            set
            {
                list[index] = value;
                PropertyChanged(value);
            }
        }
        object? IList.this[int index]
        {
            get => list[index];
            set
            {
                if (value is T item)
                {
                    list[index] = item;
                    PropertyChanged(item);
                }
            }
        }

        #region 原生

        #region 列表

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < list.Count; i++) yield return list[i];
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < list.Count; i++) yield return list[i];
        }
        public T[] ToArray() => list.ToArray();
        public void ForEach(Action<T> action) => list.ForEach(action);
        public bool TrueForAll(Predicate<T> match) => list.TrueForAll(match);

        #endregion

        public int Count => list.Count;

        public bool IsReadOnly => false;

        public bool IsFixedSize => false;

        public bool IsSynchronized => true;

        public object SyncRoot => this;

        #region 添加

        public void Add(T item)
        {
            list.Add(item);
            PropertyChanged(item);
            action?.Invoke(true);
        }
        public int Add(object? value)
        {
            if (value is T item)
            {
                list.Add(item);
                PropertyChanged(item);
                action?.Invoke(true);
            }
            return list.Count;
        }

        public void AddRange(IEnumerable<T> collection)
        {
            list.AddRange(collection);
            foreach (var item in collection) PropertyChanged(item);
            action?.Invoke(true);
        }

        public void Insert(int index, T item)
        {
            list.Insert(index, item);
            PropertyChanged(item);
            action?.Invoke(true);
        }
        public void Insert(int index, object? value)
        {
            if (value is T item)
            {
                list.Insert(index, item);
            }
        }
        public void InsertRange(int index, IEnumerable<T> collection)
        {
            list.InsertRange(index, collection);
            foreach (var item in collection) PropertyChanged(item);
            action?.Invoke(true);
        }

        #endregion

        #region 判断/查找

        public bool Contains(T item) => list.Contains(item);
        public bool Contains(object? value)
        {
            if (value is T item) return list.Contains(item);
            return false;
        }

        public int IndexOf(object? value)
        {
            if (value is T item) return list.IndexOf(item);
            return -1;
        }
        public int IndexOf(T item) => list.IndexOf(item);
        public int IndexOf(T item, int index) => list.IndexOf(item, index);
        public int IndexOf(T item, int index, int count) => list.IndexOf(item, index, count);

        public int LastIndexOf(T item) => list.LastIndexOf(item);
        public int LastIndexOf(T item, int index) => list.LastIndexOf(item, index);
        public int LastIndexOf(T item, int index, int count) => list.LastIndexOf(item, index, count);

        public bool Exists(Predicate<T> match) => list.Exists(match);

        public T? Find(Predicate<T> match) => list.Find(match);
        public List<T> FindAll(Predicate<T> match) => list.FindAll(match);
        public int FindIndex(Predicate<T> match) => list.FindIndex(match);
        public int FindIndex(int startIndex, Predicate<T> match) => list.FindIndex(startIndex, match);
        public int FindIndex(int startIndex, int count, Predicate<T> match) => list.FindIndex(startIndex, count, match);

        public T? FindLast(Predicate<T> match) => list.FindLast(match);
        public int FindLastIndex(Predicate<T> match) => list.FindLastIndex(match);
        public int FindLastIndex(int startIndex, Predicate<T> match) => list.FindLastIndex(startIndex, match);
        public int FindLastIndex(int startIndex, int count, Predicate<T> match) => list.FindLastIndex(startIndex, count, match);

        public List<T> GetRange(int index, int count) => list.GetRange(index, count);

        #endregion

        #region 删除

        public void Clear()
        {
            list.Clear();
            action?.Invoke(true);
        }

        public void Remove(object? value)
        {
            if (value is T item)
            {
                list.Remove(item);
                action?.Invoke(true);
            }
        }
        public bool Remove(T item)
        {
            bool flag = list.Remove(item);
            if (flag) action?.Invoke(true);
            return flag;
        }
        public void RemoveAt(int index)
        {
            list.RemoveAt(index);
            action?.Invoke(true);
        }
        public int RemoveAll(Predicate<T> match)
        {
            int i = list.RemoveAll(match);
            action?.Invoke(true);
            return i;
        }
        public void RemoveRange(int index, int count)
        {
            list.RemoveRange(index, count);
            action?.Invoke(true);
        }

        #endregion

        public void Reverse() => list.Reverse();
        public void Reverse(int index, int count) => list.Reverse(index, count);

        public void Sort() => list.Sort();
        public void Sort(IComparer<T> comparer) => list.Sort(comparer);
        public void Sort(Comparison<T> comparison) => list.Sort(comparison);
        public void Sort(int index, int count, IComparer<T> comparer) => list.Sort(index, count, comparer);

        public void CopyTo(T[] array) => list.CopyTo(array);
        public void CopyTo(T[] array, int arrayIndex) => list.CopyTo(array, arrayIndex);
        public void CopyTo(int index, T[] array, int arrayIndex, int count) => list.CopyTo(index, array, arrayIndex, count);

        public int BinarySearch(T item) => list.BinarySearch(item);
        public int BinarySearch(T item, IComparer<T> comparer) => list.BinarySearch(item, comparer);
        public int BinarySearch(int index, int count, T item, IComparer<T> comparer) => list.BinarySearch(index, count, item, comparer);


        public void CopyTo(Array array, int index)
        {
            //list.CopyTo(array, index);
        }

        #endregion
    }

    public class BaseCollection : IList
    {
        internal Action<bool>? action;
        internal void PropertyChanged(object value)
        {
            if (value is NotifyProperty notify)
            {
                notify.PropertyChanged += (a, b) =>
                {
                    action?.Invoke(false);
                };
            }
        }

        public virtual object? this[int index]
        {
            get => get(index);
            set => set(index, value);
        }

        internal object? get(int index)
        {
            if (list == null || index < 0 || index >= count) return default;
            return list[index];
        }
        internal void set(int index, object? value)
        {
            if (value == null || list == null || index < 0 || index >= count) return;
            list[index] = value;
            PropertyChanged(value);
        }

        public int Add(object? value)
        {
            if (value == null) return -1;
            EnsureSpace(1)[count++] = value;
            PropertyChanged(value);
            action?.Invoke(true);
            return IndexOf(value);
        }

        public void AddRange(object[] items)
        {
            var m_arrItem = EnsureSpace(items.Length);
            foreach (var item in items)
            {
                m_arrItem[count++] = item;
                PropertyChanged(item);
            }
            action?.Invoke(true);
        }
        public void AddRange(IList<object> items)
        {
            var m_arrItem = EnsureSpace(items.Count);
            foreach (var item in items)
            {
                m_arrItem[count++] = item;
                PropertyChanged(item);
            }
            action?.Invoke(true);
        }

        public void Clear()
        {
            count = 0;
            list = null;
            action?.Invoke(true);
        }

        public bool Contains(object? value)
        {
            if (value == null) return false;
            return IndexOf(value) != -1;
        }

        public void CopyTo(Array array, int index)
        {
            if (list == null) return;
            Array.Copy(list, 0, array, index, count);
        }

        public IEnumerator GetEnumerator()
        {
            for (int i = 0, Len = count; i < Len; i++)
                yield return list[i];
        }

        public int IndexOf(object? value)
        {
            if (list == null || value == null) return -1;
            return Array.IndexOf(list, value);
        }

        public void Insert(int index, object? value)
        {
            if (value == null || index < 0 || index >= count) return;
            var m_arrItem = EnsureSpace(1);
            for (int i = count; i > index; i--)
                m_arrItem[i] = m_arrItem[i - 1];
            m_arrItem[index] = value;
            count++;
            PropertyChanged(value);
            action?.Invoke(true);
        }

        public void Remove(object? value)
        {
            int index = IndexOf(value);
            if (index > -1) RemoveAt(index);
        }

        public void RemoveAt(int index)
        {
            if (list == null || index < 0 || index >= count) return;
            count--;
            for (int i = index, Len = count; i < Len; i++)
                list[i] = list[i + 1];
            action?.Invoke(true);
        }

        #region 核心

        internal object[]? list;
        internal object[] EnsureSpace(int elements)
        {
            if (list == null) list = new object[Math.Max(elements, 4)];
            else if (count + elements > list.Length)
            {
                var arrTemp = new object[Math.Max(count + elements, list.Length * 2)];
                list.CopyTo(arrTemp, 0);
                list = arrTemp;
            }
            return list;
        }

        public bool IsFixedSize => false;

        public bool IsReadOnly => false;

        public int Count => count;

        public bool IsSynchronized => true;

        public object SyncRoot => this;


        internal int count = 0;

        #endregion
    }

    public class AntList<T> : IList<T>
    {
        public AntList() { }

        public AntList(int count)
        {
            EnsureSpace(count);
        }

        #region 通知

        internal Action<string, object>? action;

        #endregion

        public T this[int index]
        {
            get
            {
                if (list == null || index < 0 || index >= count) throw new Exception("Null List");
                return list[index];
            }
            set
            {
                if (value == null || list == null || index < 0 || index >= count) return;
                list[index] = value;
                action?.Invoke("edit", index);
            }
        }

        #region 添加

        public void Add(T item)
        {
            if (item == null) return;
            int index = count++;
            EnsureSpace(1)[index] = item;
            action?.Invoke("add", index);
        }

        public void AddRange(T[] items)
        {
            var m_arrItem = EnsureSpace(items.Length);
            var list = new List<int>(items.Length);
            foreach (var item in items)
            {
                int index = count++;
                list.Add(index);
                m_arrItem[index] = item;
            }
            action?.Invoke("add", list.ToArray());
        }

        public void AddRange(IList<T> items)
        {
            var m_arrItem = EnsureSpace(items.Count);
            var list = new List<int>(items.Count);
            foreach (var item in items)
            {
                int index = count++;
                list.Add(index);
                m_arrItem[index] = item;
            }
            action?.Invoke("add", list.ToArray());
        }

        #endregion

        public void Clear()
        {
            count = 0;
            list = null;
            action?.Invoke("del", "all");
        }

        public bool Contains(T item)
        {
            if (item == null) return false;
            return IndexOf(item) != -1;
        }

        public void CopyTo(T[] array, int index)
        {
            if (list == null) return;
            Array.Copy(list, 0, array, index, count);
        }

        public int IndexOf(T item)
        {
            if (list == null || item == null) return -1;
            return Array.IndexOf(list, item);
        }

        public void Insert(int index, T item)
        {
            if (item == null || index < 0 || index >= count) return;
            var m_arrItem = EnsureSpace(1);
            for (int i = count; i > index; i--) m_arrItem[i] = m_arrItem[i - 1];
            m_arrItem[index] = item;
            count++;
            action?.Invoke("add", index);
        }

        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index > -1) { RemoveAt(index); return true; }
            return false;
        }

        public void RemoveAt(int index)
        {
            if (list == null || index < 0 || index >= count) return;
            count--;
            for (int i = index, Len = count; i < Len; i++) list[i] = list[i + 1];
            action?.Invoke("del", index);
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0, Len = count; i < Len; i++)
                yield return list[i];
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int i = 0, Len = count; i < Len; i++)
                yield return list[i];
        }

        public int Count => count;

        public bool IsReadOnly => false;

        #region 核心

        int count = 0;
        T[]? list;
        T[] EnsureSpace(int elements)
        {
            if (list == null) list = new T[Math.Max(elements, 4)];
            else if (count + elements > list.Length)
            {
                var arrTemp = new T[Math.Max(count + elements, list.Length * 2)];
                list.CopyTo(arrTemp, 0);
                list = arrTemp;
            }
            return list;
        }

        #endregion
    }

    public class AntItem : NotifyProperty
    {
        public AntItem(string k)
        {
            key = k;
        }
        public AntItem(string k, object? v)
        {
            key = k;
            value = v;
        }
        public string key { get; set; }

        object? _value = null;
        public object? value
        {
            get => _value;
            set
            {
                if (_value == value) return;
                _value = value;
                OnPropertyChanged(key);
            }
        }

        public bool Try<T>(out T val)
        {
            if (_value is T v) { val = v; return true; }
            val = default;
            return false;
        }
    }

    public class NotifyProperty : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}