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
    public class iCollection<T> : BaseCollection
    {
        public new T? this[int index]
        {
            get
            {
                var v = get(index);
                if (v is T obj) return obj;
                return default;
            }
            set => set(index, value);
        }

        public void AddRange(T[] items)
        {
            var m_arrItem = EnsureSpace(items.Length);
            foreach (T item in items)
            {
                m_arrItem[count++] = item;
                PropertyChanged(item);
            }
            action?.Invoke(true);
        }
        public void AddRange(IList<T> items)
        {
            var m_arrItem = EnsureSpace(items.Count);
            foreach (T item in items)
            {
                m_arrItem[count++] = item;
                PropertyChanged(item);
            }
            action?.Invoke(true);
        }
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

    public class NotifyProperty : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}