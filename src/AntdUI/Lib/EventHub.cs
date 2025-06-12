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
// GITEE: https://gitee.com/AntdUI/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

using System;
using System.Collections.Concurrent;

namespace AntdUI
{
    public static class EventHub
    {
        static ConcurrentDictionary<int, WeakReference> dic = new ConcurrentDictionary<int, WeakReference>();

        public static void AddListener(this IEventListener listener)
        {
            var id = listener.GetHashCode();
            if (dic.TryAdd(id, new WeakReference(listener))) listener.Disposed += (s, e) => dic.TryRemove(id, out _);
        }

        internal static void Add(this BaseForm listener)
        {
            var id = listener.GetHashCode();
            if (dic.TryAdd(id, new WeakReference(listener))) listener.Disposed += (s, e) => dic.TryRemove(id, out _);
        }

        public static void Dispatch(EventType id, object? tag = null)
        {
            foreach (var item in dic)
            {
                if (item.Value.IsAlive)
                {
                    if (item.Value.Target is IEventListener listener) listener.HandleEvent(id, tag);
                    else if (item.Value.Target is BaseForm baseForm) baseForm.themeConfig!.HandleEvent(id, tag);
                }
                else dic.TryRemove(item.Key, out _);
            }
        }
    }

    /// <summary>
    /// 事件监听者
    /// </summary>
    public interface IEventListener
    {
        void HandleEvent(EventType id, object? tag);

        int GetHashCode();

        event EventHandler? Disposed;
    }

    public enum EventType
    {
        /// <summary>
        /// DPI 改变
        /// </summary>
        DPI = 1,
        /// <summary>
        /// 主题 改变
        /// </summary>
        THEME = 2,
        /// <summary>
        /// 语言 改变
        /// </summary>
        LANG = 3,
        /// <summary>
        /// Window 状态改变
        /// </summary>
        WINDOW_STATE = 70,
        /// <summary>
        /// 自定义
        /// </summary>
        DIV = 100
    }
}