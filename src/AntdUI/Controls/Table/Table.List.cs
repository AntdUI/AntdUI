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

namespace AntdUI
{
    public class ColumnCollection : IEnumerable<Column>
    {
        internal Table? table;
        void R()
        {
            if (table == null) return;
            table.LoadLayout();
        }
        List<Column> list;
        public ColumnCollection() { list = new List<Column>(); }
        public ColumnCollection(int count) { list = new List<Column>(count); }
        public ColumnCollection(IEnumerable<Column> collection) { list = new List<Column>(collection); }

        public Column this[int index]
        {
            get => list[index];
            set => list[index] = value;
        }
        public Column? this[string key]
        {
            get
            {
                foreach (var item in list)
                {
                    if (item.Key == key) return item;
                }
                return null;
            }
            set
            {
                if (value == null) return;
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Key == key)
                    {
                        list[i] = value;
                        return;
                    }
                }
            }
        }

        #region 原生

        #region 列表

        public IEnumerator<Column> GetEnumerator() => list.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => list.GetEnumerator();
        public Column[] ToArray() => list.ToArray();
        public void ForEach(Action<Column> action) => list.ForEach(action);
        public bool TrueForAll(Predicate<Column> match) => list.TrueForAll(match);

        #endregion

        public int Count => list.Count;

        #region 添加

        public void Add(Column item)
        {
            list.Add(item);
            R();
        }
        public void AddRange(IEnumerable<Column> collection)
        {
            list.AddRange(collection);
            R();
        }

        public void Insert(int index, Column item)
        {
            list.Insert(index, item);
            R();
        }
        public void InsertRange(int index, IEnumerable<Column> collection)
        {
            list.InsertRange(index, collection);
            R();
        }

        #endregion

        #region 判断/查找

        public bool Contains(Column item) => list.Contains(item);

        public int IndexOf(Column item) => list.IndexOf(item);
        public int IndexOf(Column item, int index) => list.IndexOf(item, index);
        public int IndexOf(Column item, int index, int count) => list.IndexOf(item, index, count);

        public int LastIndexOf(Column item) => list.LastIndexOf(item);
        public int LastIndexOf(Column item, int index) => list.LastIndexOf(item, index);
        public int LastIndexOf(Column item, int index, int count) => list.LastIndexOf(item, index, count);

        public bool Exists(Predicate<Column> match) => list.Exists(match);

        public Column? Find(Predicate<Column> match) => list.Find(match);
        public List<Column> FindAll(Predicate<Column> match) => list.FindAll(match);
        public int FindIndex(Predicate<Column> match) => list.FindIndex(match);
        public int FindIndex(int startIndex, Predicate<Column> match) => list.FindIndex(startIndex, match);
        public int FindIndex(int startIndex, int count, Predicate<Column> match) => list.FindIndex(startIndex, count, match);

        public Column? FindLast(Predicate<Column> match) => list.FindLast(match);
        public int FindLastIndex(Predicate<Column> match) => list.FindLastIndex(match);
        public int FindLastIndex(int startIndex, Predicate<Column> match) => list.FindLastIndex(startIndex, match);
        public int FindLastIndex(int startIndex, int count, Predicate<Column> match) => list.FindLastIndex(startIndex, count, match);

        public List<Column> GetRange(int index, int count) => list.GetRange(index, count);

        #endregion

        #region 删除

        public void Clear() { list.Clear(); }

        public bool Remove(Column item)
        {
            var r = list.Remove(item);
            R();
            return r;
        }
        public void RemoveAt(int index)
        {
            list.RemoveAt(index);
            R();
        }
        public int RemoveAll(Predicate<Column> match)
        {
            var r = list.RemoveAll(match);
            R();
            return r;
        }
        public void RemoveRange(int index, int count)
        {
            list.RemoveRange(index, count);
            R();
        }

        #endregion

        public void Reverse() => list.Reverse();
        public void Reverse(int index, int count) => list.Reverse(index, count);

        public void Sort() => list.Sort();
        public void Sort(IComparer<Column> comparer) => list.Sort(comparer);
        public void Sort(Comparison<Column> comparison) => list.Sort(comparison);
        public void Sort(int index, int count, IComparer<Column> comparer) => list.Sort(index, count, comparer);

        public void CopyTo(Column[] array) => list.CopyTo(array);
        public void CopyTo(Column[] array, int arrayIndex) => list.CopyTo(array, arrayIndex);
        public void CopyTo(int index, Column[] array, int arrayIndex, int count) => list.CopyTo(index, array, arrayIndex, count);

        public int BinarySearch(Column item) => list.BinarySearch(item);
        public int BinarySearch(Column item, IComparer<Column> comparer) => list.BinarySearch(item, comparer);
        public int BinarySearch(int index, int count, Column item, IComparer<Column> comparer) => list.BinarySearch(index, count, item, comparer);

        #endregion
    }
}