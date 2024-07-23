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
using System.Windows.Forms;

namespace AntdUI
{
    partial class Tabs
    {
        public class TabCollection : IList<TabPage>, IList
        {
            List<TabPage> list;
            Tabs owner;
            public TabCollection(Tabs tabs) { owner = tabs; list = new List<TabPage>(); }

            public TabPage this[int index]
            {
                get => list[index];
                set => list[index] = value;
            }
            object? IList.this[int index]
            {
                get => list[index];
                set
                {
                    if (value is TabPage item) list[index] = item;
                }
            }

            #region 原生

            #region 列表

            public IEnumerator<TabPage> GetEnumerator() => list.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => list.GetEnumerator();
            public TabPage[] ToArray() => list.ToArray();
            public void ForEach(Action<TabPage> action) => list.ForEach(action);
            public bool TrueForAll(Predicate<TabPage> match) => list.TrueForAll(match);

            #endregion

            public int Count => list.Count;

            public bool IsReadOnly => false;

            public bool IsFixedSize => false;

            public bool IsSynchronized => true;

            public object SyncRoot => this;

            #region 核心交互

            void Init(TabPage item)
            {
                item.Dock = DockStyle.Fill;
                owner.ShowPage();
                item.PARENT = owner;
            }

            void Del(TabPage item, int old, int index)
            {
                if (old == index)
                {
                    owner.Controls.Remove(item);
                    int _new = index - 1;
                    if (_new > -1) owner.SelectedIndex = _new;
                }
            }

            #endregion

            #region 添加

            public void Add(TabPage item)
            {
                Init(item);
                list.Add(item);
                owner.LoadLayout();
            }
            public int Add(object? value)
            {
                if (value is TabPage item)
                {
                    Init(item);
                    list.Add(item);
                    owner.LoadLayout();
                }
                return list.Count;
            }

            public void AddRange(IEnumerable<TabPage> collection)
            {
                foreach (TabPage item in collection) Init(item);
                list.AddRange(collection);
                owner.LoadLayout();
            }

            public void Insert(int index, TabPage item)
            {
                Init(item);
                list.Insert(index, item);
                owner.LoadLayout();
            }
            public void Insert(int index, object? value)
            {
                if (value is TabPage item)
                {
                    Init(item);
                    list.Insert(index, item);
                    owner.LoadLayout();
                }
            }
            public void InsertRange(int index, IEnumerable<TabPage> collection)
            {
                foreach (TabPage item in collection) Init(item);
                list.InsertRange(index, collection);
                owner.LoadLayout();
            }

            #endregion

            #region 判断/查找

            public bool Contains(TabPage item) => list.Contains(item);
            public bool Contains(object? value)
            {
                if (value is TabPage item) return list.Contains(item);
                return false;
            }

            public int IndexOf(object? value)
            {
                if (value is TabPage item) return list.IndexOf(item);
                return -1;
            }
            public int IndexOf(TabPage item) => list.IndexOf(item);
            public int IndexOf(TabPage item, int index) => list.IndexOf(item, index);
            public int IndexOf(TabPage item, int index, int count) => list.IndexOf(item, index, count);

            public int LastIndexOf(TabPage item) => list.LastIndexOf(item);
            public int LastIndexOf(TabPage item, int index) => list.LastIndexOf(item, index);
            public int LastIndexOf(TabPage item, int index, int count) => list.LastIndexOf(item, index, count);

            public bool Exists(Predicate<TabPage> match) => list.Exists(match);

            public TabPage? Find(Predicate<TabPage> match) => list.Find(match);
            public List<TabPage> FindAll(Predicate<TabPage> match) => list.FindAll(match);
            public int FindIndex(Predicate<TabPage> match) => list.FindIndex(match);
            public int FindIndex(int startIndex, Predicate<TabPage> match) => list.FindIndex(startIndex, match);
            public int FindIndex(int startIndex, int count, Predicate<TabPage> match) => list.FindIndex(startIndex, count, match);

            public TabPage? FindLast(Predicate<TabPage> match) => list.FindLast(match);
            public int FindLastIndex(Predicate<TabPage> match) => list.FindLastIndex(match);
            public int FindLastIndex(int startIndex, Predicate<TabPage> match) => list.FindLastIndex(startIndex, match);
            public int FindLastIndex(int startIndex, int count, Predicate<TabPage> match) => list.FindLastIndex(startIndex, count, match);

            public List<TabPage> GetRange(int index, int count) => list.GetRange(index, count);

            #endregion

            #region 删除

            public void Clear()
            {
                list.Clear();
                owner.Controls.Clear();
                owner.SelectedIndex = 0;
                owner.Invalidate();
            }

            public void Remove(object? value)
            {
                if (value is TabPage item)
                {
                    int old = owner.SelectedIndex;
                    int index = list.IndexOf(item);
                    if (index > -1)
                    {
                        list.Remove(item);
                        Del(item, old, index);
                        owner.LoadLayout();
                    }
                }
            }
            public bool Remove(TabPage item)
            {
                int old = owner.SelectedIndex;
                int index = list.IndexOf(item);
                if (index > -1)
                {
                    bool flag = list.Remove(item);
                    Del(item, old, index);
                    owner.LoadLayout();
                    return flag;
                }
                return false;
            }
            public void RemoveAt(int index)
            {
                int old = owner.SelectedIndex;
                var item = list[index];
                list.RemoveAt(index);
                Del(item, old, index);
                owner.LoadLayout();
            }
            public int RemoveAll(Predicate<TabPage> match)
            {
                int i = list.RemoveAll(match);
                owner.LoadLayout();
                return i;
            }
            public void RemoveRange(int index, int count)
            {
                list.RemoveRange(index, count);
                owner.LoadLayout();
            }

            #endregion

            public void Reverse() => list.Reverse();
            public void Reverse(int index, int count) => list.Reverse(index, count);

            public void Sort() => list.Sort();
            public void Sort(IComparer<TabPage> comparer) => list.Sort(comparer);
            public void Sort(Comparison<TabPage> comparison) => list.Sort(comparison);
            public void Sort(int index, int count, IComparer<TabPage> comparer) => list.Sort(index, count, comparer);

            public void CopyTo(TabPage[] array) => list.CopyTo(array);
            public void CopyTo(TabPage[] array, int arrayIndex) => list.CopyTo(array, arrayIndex);
            public void CopyTo(int index, TabPage[] array, int arrayIndex, int count) => list.CopyTo(index, array, arrayIndex, count);

            public int BinarySearch(TabPage item) => list.BinarySearch(item);
            public int BinarySearch(TabPage item, IComparer<TabPage> comparer) => list.BinarySearch(item, comparer);
            public int BinarySearch(int index, int count, TabPage item, IComparer<TabPage> comparer) => list.BinarySearch(index, count, item, comparer);


            public void CopyTo(Array array, int index)
            {
                //list.CopyTo(array, index);
            }

            #endregion
        }
    }
}